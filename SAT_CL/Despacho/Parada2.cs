using Microsoft.SqlServer.Types;
using SAT_CL.ControlEvidencia;
using SAT_CL.Documentacion;
using SAT_CL.EgresoServicio;
using SAT_CL.Global;
using System;
using System.Data;
using System.Transactions;
using TSDK.Base;
using TSDK.Datos;


namespace SAT_CL.Despacho
{
    public partial class Parada
    {
        /// <summary>
        /// Actualiza paradas del movimiento en Vacio a paradas de Servicio
        /// </summary>
        /// <param name="id_servicio">Id Servicio</param>
        /// <param name="id_cliente">Id Cliente</param>
        /// <param name="objParadaOrigen">Parada Origen</param>
        /// <param name="objParadaDestino">Parada Destino</param>
        /// <param name="id_movimiento">Id Movimiento</param>
        /// <param name="id_ruta">Id Ruta</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion ActualizaParadasMovimientoVacioaServicio(int id_servicio, int id_cliente, Parada objParadaOrigen, Parada objParadaDestino, int id_movimiento, int id_ruta, int id_usuario)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Declaramos Variables para Alamcenar el Id de Parada Carga y el Id Parada Descarga.
            int IdParadaCarga = 0;
            int IdParadaDescarga = 0;
            //Creamos la transacción 
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Editamos Parada Origen de Carga
                resultado = objParadaOrigen.EditaParada(id_servicio, 1, Parada.TipoParada.Operativa, objParadaOrigen.id_ubicacion, objParadaOrigen.geo_ubicacion, objParadaOrigen.descripcion,
                                               objParadaOrigen.fecha_llegada, id_usuario);

                //Validamos Insercción Parada Carga
                if (resultado.OperacionExitosa)
                {
                    //Establecemos Id parada de Carga generado.
                    IdParadaCarga = resultado.IdRegistro;

                    //Insertamos Evento (Carga)
                    resultado = ParadaEvento.InsertaParadaEvento(id_servicio, 1, ParadaEvento.EstatusParadaEvento.Terminado, IdParadaCarga, 0, 1, id_usuario);

                    //Validamos Insercción del Evento
                    if (resultado.OperacionExitosa)
                    {
                        //Editamos Parada de Descarga
                        resultado = objParadaDestino.EditaParada(id_servicio, 2, Parada.TipoParada.Operativa, objParadaDestino.id_ubicacion, objParadaDestino.geo_ubicacion, objParadaDestino.descripcion,
                                            objParadaDestino.fecha_llegada, TipoActualizacionSalida.Manual, objParadaDestino.fecha_llegada.AddMinutes(1), id_usuario);

                        //Validamos Insercción de la parada de Descarga
                        if (resultado.OperacionExitosa)
                        {
                            //Establecemos Id parada de Descarga generado.
                            IdParadaDescarga = resultado.IdRegistro;

                            //Insertamos Evento (Descarga)
                            resultado = ParadaEvento.InsertaParadaEvento(id_servicio, 2, ParadaEvento.EstatusParadaEvento.Terminado, IdParadaDescarga, 0, 2, id_usuario);

                            //Validamos Insercción del eventote
                            if (resultado.OperacionExitosa)
                            {
                                //Insertamos Segmento de Carga/Descarga
                                resultado = SegmentoCarga.InsertaSegmentoCarga(id_servicio, SegmentoCarga.Estatus.Terminado, 1, IdParadaCarga, IdParadaDescarga, id_ruta, id_usuario);

                                //Validamos Insercción del Segmento
                                if (resultado.OperacionExitosa)
                                {
                                    //Intsnaciamos Movimiento
                                    using (Movimiento objMovimiento = new Movimiento(id_movimiento))
                                    {
                                        //Insertamos Movimiento de Carga/Descarga actualizando el Kilometraje Correspondiente
                                        resultado = objMovimiento.EditaMovimiento(id_servicio, resultado.IdRegistro, 1, (Movimiento.Estatus)objMovimiento.id_estatus_movimiento, Movimiento.Tipo.Cargado, objMovimiento.kms, objMovimiento.kms_maps, objMovimiento.id_compania_emisor, objMovimiento.id_parada_origen, objMovimiento.id_parada_destino, id_usuario);

                                        //Validamos Insercción del Movimiento
                                        if (resultado.OperacionExitosa)
                                        {
                                            //Actualizamos Detalles de Liquidación de las Asignaciones
                                            resultado = MovimientoAsignacionRecurso.ActualizaAsignacionesMovimientoVacioaServicio(id_servicio, id_cliente, id_movimiento, id_usuario);

                                            //Validamos Resultado
                                            if (resultado.OperacionExitosa)
                                            {
                                                //Actualizamos Atributos del Movimiento
                                                if (objMovimiento.ActualizaMovimiento())
                                                {
                                                    decimal km_recorridos = 0, km_cargados = 0, km_vacios = 0, km_tronco = 0;

                                                    //Recuperando información de kilometraje
                                                    objMovimiento.obtenemosKilometrajeMovimiento(out km_recorridos, out km_cargados, out km_vacios, out km_tronco);
                                                    //Si no hay errores
                                                    if (resultado.OperacionExitosa)
                                                    {
                                                        //Insertamos Servicio Despacho con Kilometraje Asignado
                                                        resultado = SAT_CL.Despacho.ServicioDespacho.InsertaServicioDespacho(id_servicio, objParadaOrigen.fecha_llegada, objParadaDestino.fecha_llegada.AddMinutes(1), objParadaOrigen.id_parada,
                                                                     objParadaDestino.id_parada, objParadaOrigen.id_parada, objParadaDestino.id_parada, objMovimiento.kms, km_recorridos,
                                                                     km_cargados, km_vacios, km_tronco, id_usuario);
                                                        //Validamos Insercción de Servicio Despacho
                                                        if (resultado.OperacionExitosa)
                                                        {
                                                            //Intstanciamos Servicio Despacho
                                                            using (ServicioDespacho objServicioDespacho = new ServicioDespacho(ServicioDespacho.TipoCarga.IdServicio, id_servicio))
                                                            {
                                                                //Actualizamos Valores Generales de Unidades Principales
                                                                resultado = objServicioDespacho.ActualizaParadaDestinoServicio(objParadaDestino.id_parada, objParadaDestino.fecha_llegada.AddMinutes(1), id_usuario);
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                    //Mostramos Mensaje Error
                                                    resultado = new RetornoOperacion("No se encontró datos complemenatrios del Movimiento.");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                //Si la Operación fue exitosa
                if (resultado.OperacionExitosa)
                {
                    //Resultado transacción
                    scope.Complete();
                }
            }//Fin Transacción
            return resultado;
        }

        /// <summary>
        /// Inserta Nueva Parada
        /// </summary>
        /// <param name="id_servicio">Id Servicio</param>
        /// <param name="secuencia">Secuencia</param>
        /// <param name="tipo_parada">Tipo Parada</param>
        /// <param name="id_tipo_evento">Tipo Evento</param>
        /// <param name="id_ubicacion">Id Ubicacion</param>
        /// <param name="geo_ubicacion">GeoUbicacion</param>
        /// <param name="cita_parada">Cita Parada</param>
        /// <param name="id_ruta">Id Ruta</param>
        /// <param name="id_compania_emisor">Id Compañia Emisor</param>
        /// <param name="total_paradas">Total registros paradas</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion NuevaParadaDespacho(int id_servicio, decimal secuencia, TipoParada tipo_parada, int id_tipo_evento, int id_ubicacion, SqlGeography geo_ubicacion, DateTime cita_parada,
                                                          int id_ruta, int id_compania_emisor, int total_paradas, int id_usuario)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();
            //Declaramos variables
            int id_parada = 0;
            int id_segmento_nuevo = 0;
            int id_segmento_insercion = 0;
            int id_segmento_edicion = 0;
            decimal secuencia_anterior_carga = 0;
            decimal secuencia_posterior_carga = 0;

            //Creamos la transacción 
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciamos Servicio 
                using (Documentacion.Servicio objServicio = new Documentacion.Servicio(id_servicio))
                {
                    //Validamos Estatus del Servicio
                    if (objServicio.estatus == Servicio.Estatus.Iniciado || objServicio.estatus == Servicio.Estatus.Documentado)
                    {
                        //1. Validamos que el Total de Paradas Conincida con los registros existentes des BD
                        if (total_paradas == ObtieneTotalParadas(id_servicio))
                        {
                            //Obtenemos Parada Posterior
                            using (Parada objParadaPosteriorRegistrada = new Parada(secuencia, id_servicio))
                            {
                                //Validamos Estatus
                                if (objParadaPosteriorRegistrada.Estatus == EstatusParada.Registrado || objParadaPosteriorRegistrada.id_parada == 0)
                                {
                                    //2. Obtenemos Resultado de la validación de las paradas
                                    resultado = validacionCitasParadasParaInserccion(secuencia, id_servicio, total_paradas, cita_parada, tipo_parada);

                                    //Validamos Citas de Paradas
                                    if (resultado.OperacionExitosa)
                                    {
                                        //3. Actualizamos las secuencias de las paradas de acuerdo a la secuencia que se esta ingresando.
                                        resultado = ActualizaSecuenciaParadas(id_servicio, secuencia, id_usuario);

                                        //Validamos Actualizacion de Secuencias.
                                        if (resultado.OperacionExitosa)
                                        {
                                            //4. Insertamo la nueva parada con la secuencia deseada
                                            resultado = InsertaParada(id_servicio, secuencia, tipo_parada, id_ubicacion, cita_parada, geo_ubicacion, id_usuario);

                                            //Validamos Insercion de Parada.
                                            if (resultado.OperacionExitosa)
                                            {
                                                //Obtenemos el id de la parada recien insertada
                                                id_parada = resultado.IdRegistro;

                                                //5.En caso de ser necesario modificamos los segmentos (Solo en inserción de paradas operativas)
                                                if (tipo_parada == TipoParada.Operativa)
                                                {
                                                    //6. Insertamos Evento 
                                                    resultado = ParadaEvento.InsertaParadaEvento(id_servicio, id_parada, 0, id_tipo_evento,cita_parada.AddMinutes(1), id_usuario);

                                                    //Validamos Insercción de Parada operativa
                                                    if (resultado.OperacionExitosa)
                                                    {
                                                        //Buscamos la parada operativa anterior y la parada operativa posterior
                                                        using (Parada objParadaAnteriorCarga = new Parada(BuscaParadaAnteriorOperativa(id_servicio, secuencia)), objParadaPosteriorCarga = new Parada(BuscaParadaPosteriorOperativa(id_servicio, secuencia)))
                                                        {
                                                            //En caso de insertarse un segmento al inicio
                                                            if (objParadaAnteriorCarga.id_parada == 0)
                                                            {
                                                                //6.1 Actualizamos todas las secuencias de los segmentos.
                                                                resultado = SegmentoCarga.ActualizaSecuenciaSegmentos(id_servicio, 0, id_usuario);

                                                                //Validamos Actualizacion de la Secuencias
                                                                if (resultado.OperacionExitosa)
                                                                {
                                                                    //6.2 Insertamos el nuevo segmento con secuencia 1
                                                                    resultado = SegmentoCarga.InsertaSegmentoCarga(id_servicio, 1, id_parada, objParadaPosteriorCarga.id_parada, id_ruta, id_usuario);

                                                                    //Obtenemos el id del segmneto  recien insertado
                                                                    id_segmento_nuevo = resultado.IdRegistro;
                                                                    secuencia_posterior_carga = objParadaPosteriorCarga.secuencia_parada_servicio;
                                                                    //Validamos Resultado
                                                                    if (resultado.OperacionExitosa)
                                                                    {
                                                                        //Insertamos Control Evidencia
                                                                        resultado = SegmentoControlEvidencia.SegmentoControlEvidenciaInsertaParada(id_servicio, 0, resultado.IdRegistro, id_usuario);


                                                                        //Validamos Insercción Segmneto
                                                                        if (resultado.OperacionExitosa)
                                                                        {

                                                                            //6.3 Actualizamos la ubicación de Carga del Servicio
                                                                            resultado = objServicio.EditarServicio(objServicio.id_cliente_receptor, id_ubicacion, cita_parada
                                                                                                                   , objServicio.id_ubicacion_descarga, objServicio.cita_descarga, objServicio.porte
                                                                                                                   , objServicio.referencia_cliente, objServicio.observacion_servicio,
                                                                                                                   id_usuario);

                                                                            //Validamos Actualización de la ubicación de Carga
                                                                            if (resultado.OperacionExitosa)
                                                                            {
                                                                                //6.4 Actualizamos el segmento de los Movimientos
                                                                                resultado = Movimiento.ActualizaSegmentoDeMovimientos(id_servicio, secuencia, objParadaPosteriorCarga.secuencia_parada_servicio, id_segmento_nuevo, id_usuario);
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            else
                                                                //En caso de insertarse un segmento al final
                                                                if (objParadaPosteriorCarga.id_parada == 0)
                                                                {
                                                                    //6.1 No se actualizan secuencia de segmentos 
                                                                    //6.2 Insertamos el nuevo segmento con secuencia maxima mas 1
                                                                    resultado = SegmentoCarga.InsertaSegmentoCarga(id_servicio, objParadaAnteriorCarga.id_parada, id_parada, id_ruta, id_usuario);

                                                                    //Obtenemos el id del segmneto  recien insertado
                                                                    id_segmento_nuevo = resultado.IdRegistro;
                                                                    //Obtenemos la secuencia de la parada anterior de carga
                                                                    secuencia_anterior_carga = objParadaAnteriorCarga.secuencia_parada_servicio;
                                                                    //Validamos Resultado
                                                                    if (resultado.OperacionExitosa)
                                                                    {
                                                                        resultado = SegmentoControlEvidencia.SegmentoControlEvidenciaInsertaParada(id_servicio, resultado.IdRegistro, 0, id_usuario);
                                                                        //Validamos Insercción Segmneto
                                                                        if (resultado.OperacionExitosa)
                                                                        {
                                                                            //6.3 Actualizamos la ubicación de Descarga del servicio
                                                                            resultado = objServicio.EditarServicio(objServicio.id_cliente_receptor, objServicio.id_ubicacion_carga, objServicio.cita_carga
                                                                                                                   , id_ubicacion, cita_parada, objServicio.porte
                                                                                                                   , objServicio.referencia_cliente, objServicio.observacion_servicio,
                                                                                                                   id_usuario);
                                                                            //Validamos Actualización
                                                                            if (resultado.OperacionExitosa)
                                                                            {
                                                                                //6.4 Actualizamos el segmento de los Movimientos
                                                                                resultado = Movimiento.ActualizaSegmentoDeMovimientos(id_servicio, objParadaAnteriorCarga.secuencia_parada_servicio, secuencia, id_segmento_nuevo, id_usuario);
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                //En caso de insertarse un segmento intermedio
                                                                else
                                                                {
                                                                    //Obtenemos la secuencia del segmento que contiene a las paradas anterior y posterior
                                                                    decimal SecuenciaSegmento = SegmentoCarga.BuscaSecuenciaSegmento(id_servicio, objParadaAnteriorCarga.id_parada, objParadaPosteriorCarga.id_parada);

                                                                    //6.1 Actualizamos todas las secuencias de los segmentos.
                                                                    resultado = SegmentoCarga.ActualizaSecuenciaSegmentos(id_servicio, SecuenciaSegmento, id_usuario);

                                                                    //Validamos Actualización de las secuencias
                                                                    if (resultado.OperacionExitosa)
                                                                    {
                                                                        //6.2 Insertamos el nuevo segmento
                                                                        resultado = SegmentoCarga.InsertaSegmentoCarga(id_servicio, SecuenciaSegmento + 1, id_parada, objParadaPosteriorCarga.id_parada, id_ruta, id_usuario);

                                                                        //Obtenemos el id del segmneto  recien insertado
                                                                        id_segmento_nuevo = resultado.IdRegistro;

                                                                        //Obtenemos Secuencias para su edición de movimiento
                                                                        secuencia_anterior_carga = objParadaAnteriorCarga.secuencia_parada_servicio;
                                                                        secuencia_posterior_carga = objParadaPosteriorCarga.secuencia_parada_servicio;

                                                                        //Validamos Insercción del segmento
                                                                        if (resultado.OperacionExitosa)
                                                                        {
                                                                            //6.3 Instanciamos segmento a partir de la secuencia obtenida
                                                                            using (SegmentoCarga objSegmentoCarga = new SegmentoCarga(id_servicio, SecuenciaSegmento))
                                                                            {
                                                                                //6.4 Actualizamos el destino del segmento original
                                                                                resultado = objSegmentoCarga.EditaSegmentoCarga(objSegmentoCarga.id_servicio, objSegmentoCarga.secuencia, objSegmentoCarga.EstatusSegmento,
                                                                                            objParadaAnteriorCarga.id_parada, id_parada, id_ruta, id_usuario);
                                                                                //Validamos Resultado
                                                                                if (resultado.OperacionExitosa)
                                                                                {
                                                                                    resultado = SegmentoControlEvidencia.SegmentoControlEvidenciaInsertaParada(id_servicio, resultado.IdRegistro, id_segmento_nuevo, id_usuario);
                                                                                    //Validamos Insercción Segmneto
                                                                                    if (resultado.OperacionExitosa)
                                                                                    {
                                                                                        //6.5 Actualizamos el segmento de los Movimientos
                                                                                        resultado = Movimiento.ActualizaSegmentoDeMovimientos(id_servicio, secuencia, objParadaPosteriorCarga.secuencia_parada_servicio, id_segmento_nuevo, id_usuario);
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }//If modificacion segmento/insercion segmento intermedio
                                                        }//Using instancia parada carga anterior y posterior
                                                    }//Validamos Insercción de Evento
                                                }//Fin if Modificacion Segmento
                                                //Validamos Actualización
                                                if (resultado.OperacionExitosa)
                                                {
                                                    //7. Modificamos/Insertamos los movimientos 
                                                    using (Parada objParadaAnterior = new Parada(BuscaParadaAnterior(id_servicio, secuencia)), objParadaPosterior = new Parada(BuscaParadaPosterior(id_servicio, secuencia)))
                                                    {
                                                        //Validamos que no existan paradas anterior con la misma ubicacion
                                                        if (objParadaAnterior._id_ubicacion != id_ubicacion || objParadaAnterior.id_ubicacion == 0)
                                                        {
                                                            //Validamos que no existan paradas posterior con la misma ubicacion
                                                            if (objParadaPosterior._id_ubicacion != id_ubicacion || objParadaPosterior.id_ubicacion == 0)
                                                            {
                                                                //En caso de insertarse un movimiento al inicio
                                                                if (objParadaAnterior.id_parada == 0)
                                                                {
                                                                    //Validamos que sea Parada Operativa
                                                                    if (tipo_parada == TipoParada.Operativa)
                                                                    {
                                                                        //7.1 Actualizamos todas las secuencias de los movimientos.
                                                                        resultado = Movimiento.ActualizaSecuenciaMovimientos(id_servicio, 0, id_usuario);

                                                                        //Validamos Actualizacion de la Secuencias
                                                                        if (resultado.OperacionExitosa)
                                                                        {
                                                                            //7.2 Insertamos el nuevo movimiento con secuencia 1
                                                                            resultado = Movimiento.InsertaMovimiento(id_servicio, id_segmento_nuevo, 1, Movimiento.Tipo.Cargado, 0,
                                                                                                                     0, id_compania_emisor, id_parada, objParadaPosterior.id_parada, id_usuario);
                                                                        }
                                                                    }
                                                                    else
                                                                        resultado = new RetornoOperacion("No es posible insertar una Parada de Servicio al Inicio.");
                                                                }
                                                                else
                                                                    //En caso de insertarse un movimiento al final
                                                                    if (objParadaPosterior.id_parada == 0)
                                                                    {
                                                                        //Validamos que sea Parada Operativa
                                                                        if (tipo_parada == TipoParada.Operativa)
                                                                        {
                                                                            //7.1 No actualizamos todas las secuencias de los movimientos
                                                                            //7.2 Insertamos Movimiento al Final
                                                                            resultado = Movimiento.InsertaMovimiento(id_servicio, id_segmento_nuevo, Movimiento.Tipo.Cargado, 0,
                                                                                       0, id_compania_emisor, objParadaAnterior.id_parada, id_parada, id_usuario);

                                                                            //Validamos Insercción de Movimiento
                                                                            if (resultado.OperacionExitosa)
                                                                            {
                                                                                //Asignamos Variable Movimiento
                                                                                int id_movimiento = resultado.IdRegistro;

                                                                                //Si existe fecha de llegada  de la parada
                                                                                if (!Fecha.EsFechaMinima(objParadaAnterior.fecha_llegada))
                                                                                {                                                                                    
                                                                                    //Validamos Actualización de Operadores
                                                                                    if (resultado.OperacionExitosa)
                                                                                    {
                                                                                        //Creamos Asignación de Recursos
                                                                                        resultado = MovimientoAsignacionRecurso.CreaMovimientosAsignacionRecursoParaParadaAlFinal(objParadaAnterior.id_parada, Movimiento.BuscamosMovimientoParadaDestino(id_servicio, objParadaAnterior.id_parada)
                                                                                            , id_movimiento, id_usuario);
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            //Establecemos Mensaje error
                                                                            resultado = new RetornoOperacion("No es posible insertar una Parada de Servicio al final.");
                                                                        }
                                                                    }
                                                                    //En caso de insertar un movimiento intermedio.
                                                                    else
                                                                    {
                                                                        // Obtenemos el movimiento coincidente que contiene la parada anterior y la parada posterior.
                                                                        using (Movimiento objMovimiento = new Movimiento(objParadaAnterior.id_parada, objParadaPosterior.id_parada, id_servicio))
                                                                        {
                                                                            //7.1 Actualizamos la secuencia de los movimientos
                                                                            resultado = Movimiento.ActualizaSecuenciaMovimientos(id_servicio, objMovimiento.secuencia_servicio, id_usuario);

                                                                            //Validamos actualización de las secuencias
                                                                            if (resultado.OperacionExitosa)
                                                                            {
                                                                                //Validando Tipo de Parada Operativa
                                                                                if ((Parada.TipoParada)tipo_parada == Parada.TipoParada.Operativa)
                                                                                {
                                                                                    //De acuerdo a las paradas existentes de carga asignamos Segmento
                                                                                    //Si la secuencia es al final
                                                                                    if (secuencia_posterior_carga == 0)
                                                                                    {
                                                                                        id_segmento_insercion = objMovimiento.id_segmento_carga;
                                                                                        id_segmento_edicion = id_segmento_nuevo;

                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        //Si la secuencia es al Inicio
                                                                                        if (secuencia_anterior_carga == 0)
                                                                                        {
                                                                                            id_segmento_edicion = objMovimiento.id_segmento_carga;
                                                                                            id_segmento_insercion = id_segmento_nuevo;
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            //Si la secuencia es en medio
                                                                                            id_segmento_edicion = objMovimiento.id_segmento_carga;
                                                                                            id_segmento_insercion = id_segmento_nuevo;
                                                                                        }
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    //Validando Tipo de Parada Servicio
                                                                                    id_segmento_insercion = objMovimiento.id_segmento_carga;
                                                                                    id_segmento_edicion = objMovimiento.id_segmento_carga;
                                                                                }

                                                                                //7.2 Insertamos Movimiento
                                                                                resultado = Movimiento.InsertaMovimiento(id_servicio, id_segmento_insercion, objMovimiento.secuencia_servicio + 1,
                                                                                            Movimiento.Tipo.Cargado, 0, 0, id_compania_emisor
                                                                                          , id_parada, objParadaPosterior.id_parada, id_usuario);

                                                                                    //Valiodamos que no exitan Anticipos
                                                                                   //resultado = DetalleLiquidacion.ValidaAnticiposMovimiento(objMovimiento.id_movimiento);

                                                                                    //Validamos Resultado
                                                                                    if (resultado.OperacionExitosa)
                                                                                    {
                                                                                        //7.3 Actualizamos el destino del movimiento obtenido
                                                                                        resultado = objMovimiento.EditaMovimiento(objMovimiento.id_servicio, id_segmento_edicion,
                                                                                                      objMovimiento.secuencia_servicio, (Movimiento.Estatus)objMovimiento.id_estatus_movimiento,
                                                                                                     (Movimiento.Tipo)objMovimiento.id_tipo_movimiento, 0, 0, objMovimiento.id_compania_emisor,
                                                                                                      objMovimiento.id_parada_origen, id_parada, id_usuario);
                                                                                    }

                                                                            }//fin actualización de movimientos
                                                                        }//using movimiento
                                                                    }//if modificación del movimiento/intermedio, final
                                                            }//Validación ubicación posterior
                                                            else
                                                            {
                                                                //Establecmeos Error
                                                                resultado = new RetornoOperacion("No es posible la insercción de paradas continuas con la misma ubicación.");
                                                            }
                                                        }//Validamo ubicación anterior
                                                        else
                                                        {
                                                            //Establecmeos Error
                                                            resultado = new RetornoOperacion("No es posible la insercción de paradas continuas con la misma ubicación.");
                                                        }
                                                    }//using instancia parada anterior y posterior

                                                    //Validamos Actualización del Destino
                                                    if (resultado.OperacionExitosa)
                                                    {
                                                        //Actualizamos Servicio 
                                                        if (objServicio.ActualizaServicio())
                                                        {
                                                            //7.4 Calculamos Kilometraje del Servicio
                                                            resultado = objServicio.CalculaKilometrajeServicio(id_usuario);

                                                            //Validmaos Actualización
                                                            if (resultado.OperacionExitosa)
                                                            {
                                                                //Devolmvemos Id Parada
                                                                resultado = new RetornoOperacion(id_parada);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            resultado = new RetornoOperacion("Imposible actualizar los atributos del servicio.");
                                                        }
                                                    }
                                                }//fin actualizaión movimientos/insercción evento
                                            }//fin if insercion parada
                                        }//Fin Actualización de Secuencias
                                    }//Fin Validación de Citas de Paradas
                                }//Fin validación Estatus
                                else
                                {
                                    //Establecmeos Error
                                    resultado = new RetornoOperacion("Es necesario poner como registrada la parada siguiente.");
                                }
                            }//Using Parada Posterior
                        }//Fin Validación Total Pardas
                        else
                        {
                            //Establecmeos Error
                            resultado = new RetornoOperacion("No se puede registrar la parada ya que fue modificada desde la última vez que fue consultada.");
                        }
                    }//fin validación estatus del servicio
                    else
                    {
                        //Establecemos Error
                        resultado = new RetornoOperacion("El estatus del servicio no permite su edición.");
                    }
                }//using servicio
                //Validamos Resultado
                if (resultado.OperacionExitosa)
                {
                    //Si la secuencia es mayor a 1
                    if (secuencia > 1)
                    {
                        //Instanciando servicio del movimiento
                        using (Documentacion.Servicio srv = new Documentacion.Servicio(id_servicio))
                        {
                            //Realizando actualización de plataforma de terceros (proveedor satelital)
                            srv.ActualizaPlataformaTerceros(id_usuario);
                        }
                    }

                    resultado = new RetornoOperacion(id_parada);
                    //Validamos Transacción
                    scope.Complete();
                }
            }//Fin Parda Transaccion
            //Devolvemos Resultado
            return resultado;
        }
        /// <summary>
        /// Inserta Nueva Parada considerando unicamente reglas de parada sin eventos
        /// </summary>
        /// <param name="id_servicio">Id Servicio</param>
        /// <param name="secuencia">Secuencia</param>
        /// <param name="tipo_parada">Tipo Parada</param>
        /// <param name="id_ubicacion">Id Ubicacion</param>
        /// <param name="geo_ubicacion">GeoUbicacion</param>
        /// <param name="cita_parada">Cita Parada</param>
        /// <param name="id_ruta">Id Ruta</param>
        /// <param name="id_compania_emisor">Id Compañia Emisor</param>
        /// <param name="total_paradas">Total registros paradas</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion NuevaParadaDocumentacion(int id_servicio, decimal secuencia, TipoParada tipo_parada, int id_ubicacion, SqlGeography geo_ubicacion, DateTime cita_parada, int id_ruta, int id_compania_emisor, int total_paradas, int id_usuario)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(1);
            //Declaramos variables
            int id_parada = 0;
            int id_segmento_nuevo = 0;
            int id_segmento_insercion = 0;
            int id_segmento_edicion = 0;
            decimal secuencia_anterior_carga = 0;
            decimal secuencia_posterior_carga = 0;

            //Creamos la transacción 
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciamos Servicio 
                using (Documentacion.Servicio objServicio = new Documentacion.Servicio(id_servicio))
                {
                    //Validamos Estatus del Servicio
                    if (objServicio.estatus == Servicio.Estatus.Iniciado || objServicio.estatus == Servicio.Estatus.Documentado)
                    {                        
                        //1. Validamos que el Total de Paradas Conincida con los registros existentes des BD
                        if (total_paradas == ObtieneTotalParadas(id_servicio))
                        {
                            //Obtenemos Parada Posterior
                            using (Parada objParadaPosteriorRegistrada = new Parada(secuencia, id_servicio))
                            {
                                //Validamos Estatus
                                if (objParadaPosteriorRegistrada.Estatus == EstatusParada.Registrado || objParadaPosteriorRegistrada.id_parada == 0)
                                {
                                    //Si es la primer parada, la que se va a reemplazar por la nueva
                                    if (secuencia == 1)
                                    { 
                                        //Validando si hay movimiento posterior
                                        using (Movimiento mp = new Movimiento(Movimiento.BuscamosMovimientoParadaOrigen(objParadaPosteriorRegistrada.id_servicio, objParadaPosteriorRegistrada.id_parada)))
                                        {
                                            //Si el movimiento tiene asignaciones activas, no es posible insertar la parada
                                            if (MovimientoAsignacionRecurso.CargaMovimientosAsignacion(mp.id_movimiento) != null)
                                                resultado = new RetornoOperacion("No es posible insertar la parada al inicio, existen recursos asignados al primer movimiento.");
                                        }
                                    }

                                    //Si no hay errores
                                    if (resultado.OperacionExitosa)
                                    {
                                        //2. Obtenemos Resultado de la validación de las paradas
                                        resultado = validacionCitasParadasParaInserccion(secuencia, id_servicio, total_paradas, cita_parada, tipo_parada);

                                        //Validamos Citas de Paradas
                                        if (resultado.OperacionExitosa)
                                        {
                                            //3. Actualizamos las secuencias de las paradas de acuerdo a la secuencia que se esta ingresando.
                                            resultado = ActualizaSecuenciaParadas(id_servicio, secuencia, id_usuario);

                                            //Validamos Actualizacion de Secuencias.
                                            if (resultado.OperacionExitosa)
                                            {
                                                //4. Insertamo la nueva parada con la secuencia deseada
                                                resultado = InsertaParada(id_servicio, secuencia, tipo_parada, id_ubicacion, cita_parada, geo_ubicacion, id_usuario);

                                                //Validamos Insercion de Parada.
                                                if (resultado.OperacionExitosa)
                                                {
                                                    //Obtenemos el id de la parada recien insertada
                                                    id_parada = resultado.IdRegistro;
                                                    //5.En caso de ser necesario modificamos los segmentos (Solo en inserción de paradas operativas)
                                                    if (tipo_parada == TipoParada.Operativa)
                                                    {
                                                        // Insertamos Evento (Descarga parcial como predeterminado para operativas y sin eventos para paradas de Servicio)
                                                        resultado = ParadaEvento.InsertaParadaEvento(id_servicio, id_parada, 0, 3, cita_parada.AddMinutes(1), id_usuario);

                                                        //Si no hay problemas al insertar evento predeterminado
                                                        if (resultado.OperacionExitosa)
                                                        {
                                                            //Buscamos la parada operativa anterior y la parada operativa posterior
                                                            using (Parada objParadaAnteriorCarga = new Parada(BuscaParadaAnteriorOperativa(id_servicio, secuencia)), objParadaPosteriorCarga = new Parada(BuscaParadaPosteriorOperativa(id_servicio, secuencia)))
                                                            {
                                                                //En caso de insertarse un segmento al inicio
                                                                if (objParadaAnteriorCarga.id_parada == 0)
                                                                {
                                                                    //6.1 Actualizamos todas las secuencias de los segmentos.
                                                                    resultado = SegmentoCarga.ActualizaSecuenciaSegmentos(id_servicio, 0, id_usuario);

                                                                    //Validamos Actualizacion de la Secuencias
                                                                    if (resultado.OperacionExitosa)
                                                                    {
                                                                        //6.2 Insertamos el nuevo segmento con secuencia 1
                                                                        resultado = SegmentoCarga.InsertaSegmentoCarga(id_servicio, 1, id_parada, objParadaPosteriorCarga.id_parada, id_ruta, id_usuario);

                                                                        //Obtenemos el id del segmneto  recien insertado
                                                                        id_segmento_nuevo = resultado.IdRegistro;
                                                                        secuencia_posterior_carga = objParadaPosteriorCarga.secuencia_parada_servicio;
                                                                        //Validamos Resultado
                                                                        if (resultado.OperacionExitosa)
                                                                        {
                                                                            //Insertamos Control Evidencia
                                                                            resultado = SegmentoControlEvidencia.SegmentoControlEvidenciaInsertaParada(id_servicio, 0, resultado.IdRegistro, id_usuario);

                                                                            //Validamos Insercción Segmneto
                                                                            if (resultado.OperacionExitosa)
                                                                            {

                                                                                //6.3 Actualizamos la ubicación de Carga del Servicio
                                                                                resultado = objServicio.EditarServicio(objServicio.id_cliente_receptor, id_ubicacion, cita_parada
                                                                                                                       , objServicio.id_ubicacion_descarga, objServicio.cita_descarga, objServicio.porte
                                                                                                                       , objServicio.referencia_cliente, objServicio.observacion_servicio,
                                                                                                                       id_usuario);

                                                                                //Validamos Actualización de la ubicación de Carga
                                                                                if (resultado.OperacionExitosa)
                                                                                {
                                                                                    //6.4 Actualizamos el segmento de los Movimientos
                                                                                    resultado = Movimiento.ActualizaSegmentoDeMovimientos(id_servicio, secuencia, objParadaPosteriorCarga.secuencia_parada_servicio, id_segmento_nuevo, id_usuario);
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    //En caso de insertarse un segmento al final
                                                                    if (objParadaPosteriorCarga.id_parada == 0)
                                                                    {
                                                                        //6.1 No se actualizan secuencia de segmentos 
                                                                        //6.2 Insertamos el nuevo segmento con secuencia maxima mas 1
                                                                        resultado = SegmentoCarga.InsertaSegmentoCarga(id_servicio, objParadaAnteriorCarga.id_parada, id_parada, id_ruta, id_usuario);

                                                                        //Obtenemos el id del segmneto  recien insertado
                                                                        id_segmento_nuevo = resultado.IdRegistro;
                                                                        //Obtenemos la secuencia de la parada anterior de carga
                                                                        secuencia_anterior_carga = objParadaAnteriorCarga.secuencia_parada_servicio;
                                                                        //Validamos Resultado
                                                                        if (resultado.OperacionExitosa)
                                                                        {
                                                                            resultado = SegmentoControlEvidencia.SegmentoControlEvidenciaInsertaParada(id_servicio, resultado.IdRegistro, 0, id_usuario);
                                                                            //Validamos Insercción Segmneto
                                                                            if (resultado.OperacionExitosa)
                                                                            {
                                                                                //6.3 Actualizamos la ubicación de Descarga del servicio
                                                                                resultado = objServicio.EditarServicio(objServicio.id_cliente_receptor, objServicio.id_ubicacion_carga, objServicio.cita_carga
                                                                                                                       , id_ubicacion, cita_parada, objServicio.porte
                                                                                                                       , objServicio.referencia_cliente, objServicio.observacion_servicio,
                                                                                                                       id_usuario);
                                                                                //Validamos Actualización
                                                                                if (resultado.OperacionExitosa)
                                                                                {
                                                                                    //6.4 Actualizamos el segmento de los Movimientos
                                                                                    resultado = Movimiento.ActualizaSegmentoDeMovimientos(id_servicio, objParadaAnteriorCarga.secuencia_parada_servicio, secuencia, id_segmento_nuevo, id_usuario);
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                    //En caso de insertarse un segmento intermedio
                                                                    else
                                                                    {
                                                                        //Obtenemos la secuencia del segmento que contiene a las paradas anterior y posterior
                                                                        decimal SecuenciaSegmento = SegmentoCarga.BuscaSecuenciaSegmento(id_servicio, objParadaAnteriorCarga.id_parada, objParadaPosteriorCarga.id_parada);

                                                                        //6.1 Actualizamos todas las secuencias de los segmentos.
                                                                        resultado = SegmentoCarga.ActualizaSecuenciaSegmentos(id_servicio, SecuenciaSegmento, id_usuario);

                                                                        //Validamos Actualización de las secuencias
                                                                        if (resultado.OperacionExitosa)
                                                                        {
                                                                            //6.2 Insertamos el nuevo segmento
                                                                            resultado = SegmentoCarga.InsertaSegmentoCarga(id_servicio, SecuenciaSegmento + 1, id_parada, objParadaPosteriorCarga.id_parada, id_ruta, id_usuario);

                                                                            //Obtenemos el id del segmneto  recien insertado
                                                                            id_segmento_nuevo = resultado.IdRegistro;

                                                                            //Obtenemos Secuencias para su edición de movimiento
                                                                            secuencia_anterior_carga = objParadaAnteriorCarga.secuencia_parada_servicio;
                                                                            secuencia_posterior_carga = objParadaPosteriorCarga.secuencia_parada_servicio;

                                                                            //Validamos Insercción del segmento
                                                                            if (resultado.OperacionExitosa)
                                                                            {
                                                                                //6.3 Instanciamos segmento a partir de la secuencia obtenida
                                                                                using (SegmentoCarga objSegmentoCarga = new SegmentoCarga(id_servicio, SecuenciaSegmento))
                                                                                {
                                                                                    //6.4 Actualizamos el destino del segmento original
                                                                                    resultado = objSegmentoCarga.EditaSegmentoCarga(objSegmentoCarga.id_servicio, objSegmentoCarga.secuencia, objSegmentoCarga.EstatusSegmento,
                                                                                                objParadaAnteriorCarga.id_parada, id_parada, id_ruta, id_usuario);
                                                                                    //Validamos Resultado
                                                                                    if (resultado.OperacionExitosa)
                                                                                    {
                                                                                        resultado = SegmentoControlEvidencia.SegmentoControlEvidenciaInsertaParada(id_servicio, resultado.IdRegistro, id_segmento_nuevo, id_usuario);
                                                                                        //Validamos Insercción Segmneto
                                                                                        if (resultado.OperacionExitosa)
                                                                                        {
                                                                                            //6.5 Actualizamos el segmento de los Movimientos
                                                                                            resultado = Movimiento.ActualizaSegmentoDeMovimientos(id_servicio, secuencia, objParadaPosteriorCarga.secuencia_parada_servicio, id_segmento_nuevo, id_usuario);
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }//If modificacion segmento/insercion segmento intermedio
                                                                }
                                                            }//Using instancia parada carga anterior y posterior
                                                        }//Fin Inserción de evento predeterminado de descarga
                                                        else
                                                            resultado = new RetornoOperacion(string.Format("Error al registrar evento predeterminado de parada: {0}", resultado.Mensaje));
                                                    }//Fin if Parada Operativa

                                                    //Validamos Actualización
                                                    if (resultado.OperacionExitosa)
                                                    {
                                                        //7. Modificamos/Insertamos los movimientos 
                                                        using (Parada objParadaAnterior = new Parada(BuscaParadaAnterior(id_servicio, secuencia)), objParadaPosterior = new Parada(BuscaParadaPosterior(id_servicio, secuencia)))
                                                        {
                                                            //Validamos que no existan paradas anterior con la misma ubicacion
                                                            if (objParadaAnterior._id_ubicacion != id_ubicacion || objParadaAnterior.id_ubicacion == 0)
                                                            {
                                                                //Validamos que no existan paradas posterior con la misma ubicacion
                                                                if (objParadaPosterior._id_ubicacion != id_ubicacion || objParadaPosterior.id_ubicacion == 0)
                                                                {
                                                                    //En caso de insertarse un movimiento al inicio
                                                                    if (objParadaAnterior.id_parada == 0)
                                                                    {
                                                                        //Validamos que sea Parada Operativa
                                                                        if (tipo_parada == TipoParada.Operativa)
                                                                        {
                                                                            //7.1 Actualizamos todas las secuencias de los movimientos.
                                                                            resultado = Movimiento.ActualizaSecuenciaMovimientos(id_servicio, 0, id_usuario);

                                                                            //Validamos Actualizacion de la Secuencias
                                                                            if (resultado.OperacionExitosa)
                                                                            {
                                                                                //7.2 Insertamos el nuevo movimiento con secuencia 1
                                                                                resultado = Movimiento.InsertaMovimiento(id_servicio, id_segmento_nuevo, 1, Movimiento.Tipo.Cargado, 0,
                                                                                                                         0, id_compania_emisor, id_parada, objParadaPosterior.id_parada, id_usuario);
                                                                            }
                                                                        }
                                                                        else
                                                                            resultado = new RetornoOperacion("No es posible insertar una Parada de Servicio al Inicio.");
                                                                    }
                                                                    else
                                                                    {
                                                                        //En caso de insertarse un movimiento al final
                                                                        if (objParadaPosterior.id_parada == 0)
                                                                        {
                                                                            //Validamos que sea Parada Operativa
                                                                            if (tipo_parada == TipoParada.Operativa)
                                                                            {
                                                                                //7.1 No actualizamos todas las secuencias de los movimientos
                                                                                //7.2 Insertamos Movimiento al Final
                                                                                resultado = Movimiento.InsertaMovimiento(id_servicio, id_segmento_nuevo, Movimiento.Tipo.Cargado, 0,
                                                                                           0, id_compania_emisor, objParadaAnterior.id_parada, id_parada, id_usuario);

                                                                                //Validamos Insercción de Movimiento
                                                                                if (resultado.OperacionExitosa)
                                                                                {
                                                                                    //Asignamos Variable Movimiento
                                                                                    int id_movimiento = resultado.IdRegistro;

                                                                                    //Si existe fecha de llegada  de la parada
                                                                                    if (!Fecha.EsFechaMinima(objParadaAnterior.fecha_llegada))
                                                                                    {
                                                                                        //Validamos Actualización de Operadores
                                                                                        if (resultado.OperacionExitosa)

                                                                                            //Creamos Asignación de Recursos
                                                                                            resultado = MovimientoAsignacionRecurso.CreaMovimientosAsignacionRecursoParaParadaAlFinal(objParadaAnterior.id_parada, Movimiento.BuscamosMovimientoParadaDestino(id_servicio, objParadaAnterior.id_parada), id_movimiento, id_usuario);
                                                                                    }
                                                                                }
                                                                            }
                                                                            else
                                                                                //Establecemos Mensaje error
                                                                                resultado = new RetornoOperacion("No es posible insertar una Parada de Servicio al final.");
                                                                        }
                                                                        //En caso de insertar un movimiento intermedio.
                                                                        else
                                                                        {
                                                                            // Obtenemos el movimiento coincidente que contiene la parada anterior y la parada posterior.
                                                                            using (Movimiento objMovimiento = new Movimiento(objParadaAnterior.id_parada, objParadaPosterior.id_parada, id_servicio))
                                                                            {
                                                                                //7.1 Actualizamos la secuencia de los movimientos
                                                                                resultado = Movimiento.ActualizaSecuenciaMovimientos(id_servicio, objMovimiento.secuencia_servicio, id_usuario);

                                                                                //Validamos actualización de las secuencias
                                                                                if (resultado.OperacionExitosa)
                                                                                {
                                                                                    //Validando Tipo de Parada Operativa
                                                                                    if ((Parada.TipoParada)tipo_parada == Parada.TipoParada.Operativa)
                                                                                    {
                                                                                        //De acuerdo a las paradas existentes de carga asignamos Segmento
                                                                                        //Si la secuencia es al final
                                                                                        if (secuencia_posterior_carga == 0)
                                                                                        {
                                                                                            id_segmento_insercion = objMovimiento.id_segmento_carga;
                                                                                            id_segmento_edicion = id_segmento_nuevo;
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            //Si la secuencia es al Inicio
                                                                                            if (secuencia_anterior_carga == 0)
                                                                                            {
                                                                                                id_segmento_edicion = objMovimiento.id_segmento_carga;
                                                                                                id_segmento_insercion = id_segmento_nuevo;
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                //Si la secuencia es en medio
                                                                                                id_segmento_edicion = objMovimiento.id_segmento_carga;
                                                                                                id_segmento_insercion = id_segmento_nuevo;
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        //Validando Tipo de Parada Servicio
                                                                                        id_segmento_insercion = objMovimiento.id_segmento_carga;
                                                                                        id_segmento_edicion = objMovimiento.id_segmento_carga;
                                                                                    }

                                                                                    //7.2 Insertamos Movimiento
                                                                                    resultado = Movimiento.InsertaMovimiento(id_servicio, id_segmento_insercion, objMovimiento.secuencia_servicio + 1,
                                                                                                Movimiento.Tipo.Cargado, 0, 0, id_compania_emisor, id_parada, objParadaPosterior.id_parada, id_usuario);

                                                                                    //Validamos Inserción del movimiento.
                                                                                    if (resultado.OperacionExitosa)
                                                                                    {
                                                                                        //7.3 Actualizamos el destino del movimiento obtenido
                                                                                        resultado = objMovimiento.EditaMovimiento(objMovimiento.id_servicio, id_segmento_edicion,
                                                                                                      objMovimiento.secuencia_servicio, (Movimiento.Estatus)objMovimiento.id_estatus_movimiento,
                                                                                                     (Movimiento.Tipo)objMovimiento.id_tipo_movimiento, 0, 0, objMovimiento.id_compania_emisor,
                                                                                                      objMovimiento.id_parada_origen, id_parada, id_usuario);

                                                                                        /*/Valiodamos que no exitan Anticipos
                                                                                        resultado = DetalleLiquidacion.ValidaAnticiposMovimiento(objMovimiento.id_movimiento);

                                                                                        //Validamos Resultado
                                                                                        if (resultado.OperacionExitosa)
                                                                                        {
                                                                                            
                                                                                        }//*/

                                                                                    }//fin insercción de movimientos
                                                                                }//fin actualización de movimientos
                                                                            }//using movimiento
                                                                        }//if modificación del movimiento/intermedio, final
                                                                    }
                                                                }//Validación ubicación posterior
                                                                else
                                                                {
                                                                    //Establecmeos Error
                                                                    resultado = new RetornoOperacion("No es posible la insercción de paradas continuas con la misma ubicación.");
                                                                }
                                                            }//Validamo ubicación anterior
                                                            else
                                                            {
                                                                //Establecmeos Error
                                                                resultado = new RetornoOperacion("No es posible la insercción de paradas continuas con la misma ubicación.");
                                                            }
                                                        }//using instancia parada anterior y posterior

                                                        //Validamos Actualización del Destino
                                                        if (resultado.OperacionExitosa)
                                                        {
                                                            //Actualizamos Servicio 
                                                            if (objServicio.ActualizaServicio())
                                                            {
                                                                //7.4 Calculamos Kilometraje del Servicio
                                                                resultado = objServicio.CalculaKilometrajeServicio(id_usuario);

                                                                //Validmaos Actualización
                                                                if (resultado.OperacionExitosa)
                                                                {
                                                                    //Devolmvemos Id Parada
                                                                    resultado = new RetornoOperacion(id_parada);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                resultado = new RetornoOperacion("Imposible actualizar los atributos del servicio.");
                                                            }
                                                        }
                                                    }//fin actualizaión movimientos/insercción evento
                                                }//fin if insercion parada
                                            }//Fin Actualización de Secuencias
                                        }//Fin Validación de Citas de Paradas
                                    }
                                }//Fin validación Estatus
                                else
                                {
                                    //Establecmeos Error
                                    resultado = new RetornoOperacion("Es necesario poner como registrada la parada siguiente.");
                                }
                            }//Using Parada Posterior
                        }//Fin Validación Total Pardas
                        else
                            //Establecmeos Error
                            resultado = new RetornoOperacion("No se puede registrar la parada ya que fue modificada desde la última vez que fue consultada.");
                        
                    }//fin validación estatus del servicio
                    else
                        //Establecemos Error
                        resultado = new RetornoOperacion("El estatus del servicio no permite su edición.");

                }//using servicio

                //Validamos Resultado
                if (resultado.OperacionExitosa)
                {
                    //Si la secuencia es mayor a 1
                    if (secuencia > 1)
                    {
                        //Instanciando servicio del movimiento
                        using (Documentacion.Servicio srv = new Documentacion.Servicio(id_servicio))
                        {
                            //Realizando actualización de plataforma de terceros (proveedor satelital)
                            srv.ActualizaPlataformaTerceros(id_usuario);
                        }
                    }
                                        
                    //Asignando resultado con Id de parada creada
                    resultado = new RetornoOperacion(id_parada);
                    //Validamos Transacción
                    scope.Complete();
                }
            }//Fin Parda Transaccion

            //Devolvemos Resultado
            return resultado;
        }

        /// <summary>
        /// Inserta Nueva Parada considerando unicamente reglas de parada sin eventos
        /// </summary>
        /// <param name="id_servicio">Id Servicio</param>
        /// <param name="secuencia">Secuencia</param>
        /// <param name="tipo_parada">Tipo Parada</param>
        /// <param name="id_ubicacion">Id Ubicacion</param>
        /// <param name="geo_ubicacion">GeoUbicacion</param>
        /// <param name="cita_parada">Cita Parada</param>
        /// <param name="id_ruta">Id Ruta</param>
        /// <param name="id_compania_emisor">Id Compañia Emisor</param>
        /// <param name="total_paradas">Total registros paradas</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion NuevaParadaDocumentacion(int id_servicio, decimal secuencia, TipoParada tipo_parada, int id_ubicacion, SqlGeography geo_ubicacion, DateTime cita_parada, int id_ruta, int id_compania_emisor, int total_paradas, int id_usuario, int id_parada_evento)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(1);
            //Declaramos variables
            int id_parada = 0;
            int id_segmento_nuevo = 0;
            int id_segmento_insercion = 0;
            int id_segmento_edicion = 0;
            decimal secuencia_anterior_carga = 0;
            decimal secuencia_posterior_carga = 0;

            //Creamos la transacción 
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciamos Servicio 
                using (Documentacion.Servicio objServicio = new Documentacion.Servicio(id_servicio))
                {
                    //Validamos Estatus del Servicio
                    if (objServicio.estatus == Servicio.Estatus.Iniciado || objServicio.estatus == Servicio.Estatus.Documentado)
                    {
                        //1. Validamos que el Total de Paradas Conincida con los registros existentes des BD
                        if (total_paradas == ObtieneTotalParadas(id_servicio))
                        {
                            //Obtenemos Parada Posterior
                            using (Parada objParadaPosteriorRegistrada = new Parada(secuencia, id_servicio))
                            {
                                //Validamos Estatus
                                if (objParadaPosteriorRegistrada.Estatus == EstatusParada.Registrado || objParadaPosteriorRegistrada.id_parada == 0)
                                {
                                    //Si es la primer parada, la que se va a reemplazar por la nueva
                                    if (secuencia == 1)
                                    {
                                        //Validando si hay movimiento posterior
                                        using (Movimiento mp = new Movimiento(Movimiento.BuscamosMovimientoParadaOrigen(objParadaPosteriorRegistrada.id_servicio, objParadaPosteriorRegistrada.id_parada)))
                                        {
                                            //Si el movimiento tiene asignaciones activas, no es posible insertar la parada
                                            if (MovimientoAsignacionRecurso.CargaMovimientosAsignacion(mp.id_movimiento) != null)
                                                resultado = new RetornoOperacion("No es posible insertar la parada al inicio, existen recursos asignados al primer movimiento.");
                                        }
                                    }

                                    //Si no hay errores
                                    if (resultado.OperacionExitosa)
                                    {
                                        //2. Obtenemos Resultado de la validación de las paradas
                                        resultado = validacionCitasParadasParaInserccion(secuencia, id_servicio, total_paradas, cita_parada, tipo_parada);

                                        //Validamos Citas de Paradas
                                        if (resultado.OperacionExitosa)
                                        {
                                            //3. Actualizamos las secuencias de las paradas de acuerdo a la secuencia que se esta ingresando.
                                            resultado = ActualizaSecuenciaParadas(id_servicio, secuencia, id_usuario);

                                            //Validamos Actualizacion de Secuencias.
                                            if (resultado.OperacionExitosa)
                                            {
                                                //4. Insertamo la nueva parada con la secuencia deseada
                                                resultado = InsertaParada(id_servicio, secuencia, tipo_parada, id_ubicacion, cita_parada, geo_ubicacion, id_usuario);

                                                //Validamos Insercion de Parada.
                                                if (resultado.OperacionExitosa)
                                                {
                                                    //Obtenemos el id de la parada recien insertada
                                                    id_parada = resultado.IdRegistro;
                                                    //5.En caso de ser necesario modificamos los segmentos (Solo en inserción de paradas operativas)
                                                    if (tipo_parada == TipoParada.Operativa)
                                                    {
                                                        //Foreach para eventos
                                                        //Cargando los los eventos 
                                                        using (DataTable tblParadaEventos = SAT_CL.Despacho.ParadaEvento.CargaEventos(id_parada_evento))
                                                        {
                                                            //Validamos Vales
                                                            if (Validacion.ValidaOrigenDatos(tblParadaEventos))
                                                            {
                                                                //Por Cada Vale de Diesel
                                                                foreach (DataRow eventos in tblParadaEventos.Rows)
                                                                {
                                                                    //Validamos resultado
                                                                    if (resultado.OperacionExitosa)
                                                                    {
                                                                        //Insertamos Evento (Carga)
                                                                        //resultado = ParadaEvento.InsertaParadaEvento(id_servicio, IdParadaCarga, 0, eventos.Field<int>("IdTipoEvento"), cita_carga.AddMinutes(1), id_usuario);
                                                                        // Insertamos Evento (Descarga parcial como predeterminado para operativas y sin eventos para paradas de Servicio)
                                                                        resultado = ParadaEvento.InsertaParadaEvento(id_servicio, id_parada, 0, eventos.Field<int>("IdTipoEvento"), cita_parada.AddMinutes(1), id_usuario);
                                                                    }
                                                                    else
                                                                        //Finalizamos ciclo
                                                                        break;
                                                                }

                                                            }
                                                        }
                                                        //Si no hay problemas al insertar evento predeterminado
                                                        if (resultado.OperacionExitosa)
                                                        {
                                                            //Buscamos la parada operativa anterior y la parada operativa posterior
                                                            using (Parada objParadaAnteriorCarga = new Parada(BuscaParadaAnteriorOperativa(id_servicio, secuencia)), objParadaPosteriorCarga = new Parada(BuscaParadaPosteriorOperativa(id_servicio, secuencia)))
                                                            {
                                                                //En caso de insertarse un segmento al inicio
                                                                if (objParadaAnteriorCarga.id_parada == 0)
                                                                {
                                                                    //6.1 Actualizamos todas las secuencias de los segmentos.
                                                                    resultado = SegmentoCarga.ActualizaSecuenciaSegmentos(id_servicio, 0, id_usuario);

                                                                    //Validamos Actualizacion de la Secuencias
                                                                    if (resultado.OperacionExitosa)
                                                                    {
                                                                        //6.2 Insertamos el nuevo segmento con secuencia 1
                                                                        resultado = SegmentoCarga.InsertaSegmentoCarga(id_servicio, 1, id_parada, objParadaPosteriorCarga.id_parada, id_ruta, id_usuario);

                                                                        //Obtenemos el id del segmneto  recien insertado
                                                                        id_segmento_nuevo = resultado.IdRegistro;
                                                                        secuencia_posterior_carga = objParadaPosteriorCarga.secuencia_parada_servicio;
                                                                        //Validamos Resultado
                                                                        if (resultado.OperacionExitosa)
                                                                        {
                                                                            //Insertamos Control Evidencia
                                                                            resultado = SegmentoControlEvidencia.SegmentoControlEvidenciaInsertaParada(id_servicio, 0, resultado.IdRegistro, id_usuario);

                                                                            //Validamos Insercción Segmneto
                                                                            if (resultado.OperacionExitosa)
                                                                            {

                                                                                //6.3 Actualizamos la ubicación de Carga del Servicio
                                                                                resultado = objServicio.EditarServicio(objServicio.id_cliente_receptor, id_ubicacion, cita_parada
                                                                                                                       , objServicio.id_ubicacion_descarga, objServicio.cita_descarga, objServicio.porte
                                                                                                                       , objServicio.referencia_cliente, objServicio.observacion_servicio,
                                                                                                                       id_usuario);

                                                                                //Validamos Actualización de la ubicación de Carga
                                                                                if (resultado.OperacionExitosa)
                                                                                {
                                                                                    //6.4 Actualizamos el segmento de los Movimientos
                                                                                    resultado = Movimiento.ActualizaSegmentoDeMovimientos(id_servicio, secuencia, objParadaPosteriorCarga.secuencia_parada_servicio, id_segmento_nuevo, id_usuario);
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    //En caso de insertarse un segmento al final
                                                                    if (objParadaPosteriorCarga.id_parada == 0)
                                                                    {
                                                                        //6.1 No se actualizan secuencia de segmentos 
                                                                        //6.2 Insertamos el nuevo segmento con secuencia maxima mas 1
                                                                        resultado = SegmentoCarga.InsertaSegmentoCarga(id_servicio, objParadaAnteriorCarga.id_parada, id_parada, id_ruta, id_usuario);

                                                                        //Obtenemos el id del segmneto  recien insertado
                                                                        id_segmento_nuevo = resultado.IdRegistro;
                                                                        //Obtenemos la secuencia de la parada anterior de carga
                                                                        secuencia_anterior_carga = objParadaAnteriorCarga.secuencia_parada_servicio;
                                                                        //Validamos Resultado
                                                                        if (resultado.OperacionExitosa)
                                                                        {
                                                                            resultado = SegmentoControlEvidencia.SegmentoControlEvidenciaInsertaParada(id_servicio, resultado.IdRegistro, 0, id_usuario);
                                                                            //Validamos Insercción Segmneto
                                                                            if (resultado.OperacionExitosa)
                                                                            {
                                                                                //6.3 Actualizamos la ubicación de Descarga del servicio
                                                                                resultado = objServicio.EditarServicio(objServicio.id_cliente_receptor, objServicio.id_ubicacion_carga, objServicio.cita_carga
                                                                                                                       , id_ubicacion, cita_parada, objServicio.porte
                                                                                                                       , objServicio.referencia_cliente, objServicio.observacion_servicio,
                                                                                                                       id_usuario);
                                                                                //Validamos Actualización
                                                                                if (resultado.OperacionExitosa)
                                                                                {
                                                                                    //6.4 Actualizamos el segmento de los Movimientos
                                                                                    resultado = Movimiento.ActualizaSegmentoDeMovimientos(id_servicio, objParadaAnteriorCarga.secuencia_parada_servicio, secuencia, id_segmento_nuevo, id_usuario);
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                    //En caso de insertarse un segmento intermedio
                                                                    else
                                                                    {
                                                                        //Obtenemos la secuencia del segmento que contiene a las paradas anterior y posterior
                                                                        decimal SecuenciaSegmento = SegmentoCarga.BuscaSecuenciaSegmento(id_servicio, objParadaAnteriorCarga.id_parada, objParadaPosteriorCarga.id_parada);

                                                                        //6.1 Actualizamos todas las secuencias de los segmentos.
                                                                        resultado = SegmentoCarga.ActualizaSecuenciaSegmentos(id_servicio, SecuenciaSegmento, id_usuario);

                                                                        //Validamos Actualización de las secuencias
                                                                        if (resultado.OperacionExitosa)
                                                                        {
                                                                            //6.2 Insertamos el nuevo segmento
                                                                            resultado = SegmentoCarga.InsertaSegmentoCarga(id_servicio, SecuenciaSegmento + 1, id_parada, objParadaPosteriorCarga.id_parada, id_ruta, id_usuario);

                                                                            //Obtenemos el id del segmneto  recien insertado
                                                                            id_segmento_nuevo = resultado.IdRegistro;

                                                                            //Obtenemos Secuencias para su edición de movimiento
                                                                            secuencia_anterior_carga = objParadaAnteriorCarga.secuencia_parada_servicio;
                                                                            secuencia_posterior_carga = objParadaPosteriorCarga.secuencia_parada_servicio;

                                                                            //Validamos Insercción del segmento
                                                                            if (resultado.OperacionExitosa)
                                                                            {
                                                                                //6.3 Instanciamos segmento a partir de la secuencia obtenida
                                                                                using (SegmentoCarga objSegmentoCarga = new SegmentoCarga(id_servicio, SecuenciaSegmento))
                                                                                {
                                                                                    //6.4 Actualizamos el destino del segmento original
                                                                                    resultado = objSegmentoCarga.EditaSegmentoCarga(objSegmentoCarga.id_servicio, objSegmentoCarga.secuencia, objSegmentoCarga.EstatusSegmento,
                                                                                                objParadaAnteriorCarga.id_parada, id_parada, id_ruta, id_usuario);
                                                                                    //Validamos Resultado
                                                                                    if (resultado.OperacionExitosa)
                                                                                    {
                                                                                        resultado = SegmentoControlEvidencia.SegmentoControlEvidenciaInsertaParada(id_servicio, resultado.IdRegistro, id_segmento_nuevo, id_usuario);
                                                                                        //Validamos Insercción Segmneto
                                                                                        if (resultado.OperacionExitosa)
                                                                                        {
                                                                                            //6.5 Actualizamos el segmento de los Movimientos
                                                                                            resultado = Movimiento.ActualizaSegmentoDeMovimientos(id_servicio, secuencia, objParadaPosteriorCarga.secuencia_parada_servicio, id_segmento_nuevo, id_usuario);
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }//If modificacion segmento/insercion segmento intermedio
                                                                }
                                                            }//Using instancia parada carga anterior y posterior
                                                        }//Fin Inserción de evento predeterminado de descarga
                                                        else
                                                            resultado = new RetornoOperacion(string.Format("Error al registrar evento predeterminado de parada: {0}", resultado.Mensaje));
                                                    }//Fin if Parada Operativa

                                                    //Validamos Actualización
                                                    if (resultado.OperacionExitosa)
                                                    {
                                                        //7. Modificamos/Insertamos los movimientos 
                                                        using (Parada objParadaAnterior = new Parada(BuscaParadaAnterior(id_servicio, secuencia)), objParadaPosterior = new Parada(BuscaParadaPosterior(id_servicio, secuencia)))
                                                        {
                                                            //Validamos que no existan paradas anterior con la misma ubicacion
                                                            if (objParadaAnterior._id_ubicacion != id_ubicacion || objParadaAnterior.id_ubicacion == 0)
                                                            {
                                                                //Validamos que no existan paradas posterior con la misma ubicacion
                                                                if (objParadaPosterior._id_ubicacion != id_ubicacion || objParadaPosterior.id_ubicacion == 0)
                                                                {
                                                                    //En caso de insertarse un movimiento al inicio
                                                                    if (objParadaAnterior.id_parada == 0)
                                                                    {
                                                                        //Validamos que sea Parada Operativa
                                                                        if (tipo_parada == TipoParada.Operativa)
                                                                        {
                                                                            //7.1 Actualizamos todas las secuencias de los movimientos.
                                                                            resultado = Movimiento.ActualizaSecuenciaMovimientos(id_servicio, 0, id_usuario);

                                                                            //Validamos Actualizacion de la Secuencias
                                                                            if (resultado.OperacionExitosa)
                                                                            {
                                                                                //7.2 Insertamos el nuevo movimiento con secuencia 1
                                                                                resultado = Movimiento.InsertaMovimiento(id_servicio, id_segmento_nuevo, 1, Movimiento.Tipo.Cargado, 0,
                                                                                                                         0, id_compania_emisor, id_parada, objParadaPosterior.id_parada, id_usuario);
                                                                            }
                                                                        }
                                                                        else
                                                                            resultado = new RetornoOperacion("No es posible insertar una Parada de Servicio al Inicio.");
                                                                    }
                                                                    else
                                                                    {
                                                                        //En caso de insertarse un movimiento al final
                                                                        if (objParadaPosterior.id_parada == 0)
                                                                        {
                                                                            //Validamos que sea Parada Operativa
                                                                            if (tipo_parada == TipoParada.Operativa)
                                                                            {
                                                                                //7.1 No actualizamos todas las secuencias de los movimientos
                                                                                //7.2 Insertamos Movimiento al Final
                                                                                resultado = Movimiento.InsertaMovimiento(id_servicio, id_segmento_nuevo, Movimiento.Tipo.Cargado, 0,
                                                                                           0, id_compania_emisor, objParadaAnterior.id_parada, id_parada, id_usuario);

                                                                                //Validamos Insercción de Movimiento
                                                                                if (resultado.OperacionExitosa)
                                                                                {
                                                                                    //Asignamos Variable Movimiento
                                                                                    int id_movimiento = resultado.IdRegistro;

                                                                                    //Si existe fecha de llegada  de la parada
                                                                                    if (!Fecha.EsFechaMinima(objParadaAnterior.fecha_llegada))
                                                                                    {
                                                                                        //Validamos Actualización de Operadores
                                                                                        if (resultado.OperacionExitosa)

                                                                                            //Creamos Asignación de Recursos
                                                                                            resultado = MovimientoAsignacionRecurso.CreaMovimientosAsignacionRecursoParaParadaAlFinal(objParadaAnterior.id_parada, Movimiento.BuscamosMovimientoParadaDestino(id_servicio, objParadaAnterior.id_parada), id_movimiento, id_usuario);
                                                                                    }
                                                                                }
                                                                            }
                                                                            else
                                                                                //Establecemos Mensaje error
                                                                                resultado = new RetornoOperacion("No es posible insertar una Parada de Servicio al final.");
                                                                        }
                                                                        //En caso de insertar un movimiento intermedio.
                                                                        else
                                                                        {
                                                                            // Obtenemos el movimiento coincidente que contiene la parada anterior y la parada posterior.
                                                                            using (Movimiento objMovimiento = new Movimiento(objParadaAnterior.id_parada, objParadaPosterior.id_parada, id_servicio))
                                                                            {
                                                                                //7.1 Actualizamos la secuencia de los movimientos
                                                                                resultado = Movimiento.ActualizaSecuenciaMovimientos(id_servicio, objMovimiento.secuencia_servicio, id_usuario);

                                                                                //Validamos actualización de las secuencias
                                                                                if (resultado.OperacionExitosa)
                                                                                {
                                                                                    //Validando Tipo de Parada Operativa
                                                                                    if ((Parada.TipoParada)tipo_parada == Parada.TipoParada.Operativa)
                                                                                    {
                                                                                        //De acuerdo a las paradas existentes de carga asignamos Segmento
                                                                                        //Si la secuencia es al final
                                                                                        if (secuencia_posterior_carga == 0)
                                                                                        {
                                                                                            id_segmento_insercion = objMovimiento.id_segmento_carga;
                                                                                            id_segmento_edicion = id_segmento_nuevo;
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            //Si la secuencia es al Inicio
                                                                                            if (secuencia_anterior_carga == 0)
                                                                                            {
                                                                                                id_segmento_edicion = objMovimiento.id_segmento_carga;
                                                                                                id_segmento_insercion = id_segmento_nuevo;
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                //Si la secuencia es en medio
                                                                                                id_segmento_edicion = objMovimiento.id_segmento_carga;
                                                                                                id_segmento_insercion = id_segmento_nuevo;
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        //Validando Tipo de Parada Servicio
                                                                                        id_segmento_insercion = objMovimiento.id_segmento_carga;
                                                                                        id_segmento_edicion = objMovimiento.id_segmento_carga;
                                                                                    }

                                                                                    //7.2 Insertamos Movimiento
                                                                                    resultado = Movimiento.InsertaMovimiento(id_servicio, id_segmento_insercion, objMovimiento.secuencia_servicio + 1,
                                                                                                Movimiento.Tipo.Cargado, 0, 0, id_compania_emisor, id_parada, objParadaPosterior.id_parada, id_usuario);

                                                                                    //Validamos Inserción del movimiento.
                                                                                    if (resultado.OperacionExitosa)
                                                                                    {
                                                                                        //7.3 Actualizamos el destino del movimiento obtenido
                                                                                        resultado = objMovimiento.EditaMovimiento(objMovimiento.id_servicio, id_segmento_edicion,
                                                                                                      objMovimiento.secuencia_servicio, (Movimiento.Estatus)objMovimiento.id_estatus_movimiento,
                                                                                                     (Movimiento.Tipo)objMovimiento.id_tipo_movimiento, 0, 0, objMovimiento.id_compania_emisor,
                                                                                                      objMovimiento.id_parada_origen, id_parada, id_usuario);

                                                                                        /*/Valiodamos que no exitan Anticipos
                                                                                        resultado = DetalleLiquidacion.ValidaAnticiposMovimiento(objMovimiento.id_movimiento);

                                                                                        //Validamos Resultado
                                                                                        if (resultado.OperacionExitosa)
                                                                                        {
                                                                                            
                                                                                        }//*/

                                                                                    }//fin insercción de movimientos
                                                                                }//fin actualización de movimientos
                                                                            }//using movimiento
                                                                        }//if modificación del movimiento/intermedio, final
                                                                    }
                                                                }//Validación ubicación posterior
                                                                else
                                                                {
                                                                    //Establecmeos Error
                                                                    resultado = new RetornoOperacion("No es posible la insercción de paradas continuas con la misma ubicación.");
                                                                }
                                                            }//Validamo ubicación anterior
                                                            else
                                                            {
                                                                //Establecmeos Error
                                                                resultado = new RetornoOperacion("No es posible la insercción de paradas continuas con la misma ubicación.");
                                                            }
                                                        }//using instancia parada anterior y posterior

                                                        //Validamos Actualización del Destino
                                                        if (resultado.OperacionExitosa)
                                                        {
                                                            //Actualizamos Servicio 
                                                            if (objServicio.ActualizaServicio())
                                                            {
                                                                //7.4 Calculamos Kilometraje del Servicio
                                                                resultado = objServicio.CalculaKilometrajeServicio(id_usuario);

                                                                //Validmaos Actualización
                                                                if (resultado.OperacionExitosa)
                                                                {
                                                                    //Devolmvemos Id Parada
                                                                    resultado = new RetornoOperacion(id_parada);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                resultado = new RetornoOperacion("Imposible actualizar los atributos del servicio.");
                                                            }
                                                        }
                                                    }//fin actualizaión movimientos/insercción evento
                                                }//fin if insercion parada
                                            }//Fin Actualización de Secuencias
                                        }//Fin Validación de Citas de Paradas
                                    }
                                }//Fin validación Estatus
                                else
                                {
                                    //Establecmeos Error
                                    resultado = new RetornoOperacion("Es necesario poner como registrada la parada siguiente.");
                                }
                            }//Using Parada Posterior
                        }//Fin Validación Total Pardas
                        else
                            //Establecmeos Error
                            resultado = new RetornoOperacion("No se puede registrar la parada ya que fue modificada desde la última vez que fue consultada.");

                    }//fin validación estatus del servicio
                    else
                        //Establecemos Error
                        resultado = new RetornoOperacion("El estatus del servicio no permite su edición.");

                }//using servicio

                //Validamos Resultado
                if (resultado.OperacionExitosa)
                {
                    //Si la secuencia es mayor a 1
                    if (secuencia > 1)
                    {
                        //Instanciando servicio del movimiento
                        using (Documentacion.Servicio srv = new Documentacion.Servicio(id_servicio))
                        {
                            //Realizando actualización de plataforma de terceros (proveedor satelital)
                            srv.ActualizaPlataformaTerceros(id_usuario);
                        }
                    }

                    //Asignando resultado con Id de parada creada
                    resultado = new RetornoOperacion(id_parada);
                    //Validamos Transacción
                    scope.Complete();
                }
            }//Fin Parda Transaccion

            //Devolvemos Resultado
            return resultado;
        }
        /// <summary>
        /// Inserta Nueva Parada
        /// </summary>
        /// <param name="id_servicio">Id Servicio</param>
        /// <param name="secuencia">Secuencia</param>
        /// <param name="tipo_parada">Tipo Parada</param>
        /// <param name="id_tipo_evento">Tipo Evento</param>
        /// <param name="id_ubicacion">Id Ubicacion</param>
        /// <param name="geo_ubicacion">GeoUbicacion</param>
        /// <param name="cita_parada">Cita Parada</param>
        /// <param name="id_ruta">Id Ruta</param>
        /// <param name="id_compania_emisor">Id Compañia Emisor</param>
        /// <param name="total_paradas">Total registros paradas</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion NuevaParadaServicio(int id_servicio, decimal secuencia, TipoParada tipo_parada, int id_tipo_evento, int id_ubicacion, SqlGeography geo_ubicacion, DateTime cita_parada, int id_ruta, int id_compania_emisor, int total_paradas, int id_usuario)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(1);
            //Declaramos variables
            int id_parada = 0;
            int id_segmento_nuevo = 0;
            int id_segmento_insercion = 0;
            int id_segmento_edicion = 0;
            decimal secuencia_anterior_carga = 0;
            decimal secuencia_posterior_carga = 0;

            //Creamos la transacción 
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciamos Servicio
                using (Servicio objServicio = new Servicio(id_servicio))
                {
                    //Validamos Estatus
                    if (objServicio.estatus == Servicio.Estatus.Documentado)
                    {
                        //1. Validamos que el Total de Paradas Conincida con los registros existentes des BD
                        if (total_paradas == ObtieneTotalParadas(id_servicio))
                        {
                            //Si es la primer parada, la que se va a reemplazar por la nueva
                            if (secuencia == 1)
                            {
                                //Validando si hay movimiento posterior
                                using (Movimiento mp = new Movimiento(Movimiento.BuscamosMovimientoParadaOrigen(id_servicio, new Parada(1, id_servicio).id_parada)))
                                {
                                    //Si el movimiento tiene asignaciones activas, no es posible insertar la parada
                                    if (MovimientoAsignacionRecurso.CargaMovimientosAsignacion(mp.id_movimiento) != null)
                                        resultado = new RetornoOperacion("No es posible insertar la parada al inicio, existen recursos asignados al primer movimiento.");
                                }
                            }

                            //Validando que no existan errores
                            if (resultado.OperacionExitosa)
                            {
                                //2. Obtenemos Resultado de la validación de las paradas
                                resultado = validacionCitasParadasParaInserccion(secuencia, id_servicio, total_paradas, cita_parada, tipo_parada);

                                //Validamos Citas de Paradas
                                if (resultado.OperacionExitosa)
                                {
                                    //3. Actualizamos las secuencias de las paradas de acuerdo a la secuencia que se esta ingresando.
                                    resultado = ActualizaSecuenciaParadas(id_servicio, secuencia, id_usuario);

                                    //Validamos Actualizacion de Secuencias.
                                    if (resultado.OperacionExitosa)
                                    {
                                        //4. Insertamo la nueva parada con la secuencia deseada
                                        resultado = InsertaParada(id_servicio, secuencia, tipo_parada, id_ubicacion, cita_parada, geo_ubicacion, id_usuario);

                                        //Validamos Insercion de Parada.
                                        if (resultado.OperacionExitosa)
                                        {
                                            //Obtenemos el id de la parada recien insertada
                                            id_parada = resultado.IdRegistro;

                                            //Validando que sea de Tipo Operativa
                                            if (tipo_parada == TipoParada.Operativa)
                                                //5. Insertamos Evento 
                                                resultado = ParadaEvento.InsertaParadaEvento(id_servicio, id_parada, 0, id_tipo_evento, cita_parada == DateTime.MinValue ? cita_parada : cita_parada.AddMinutes(1), id_usuario);

                                            //Validamos Insercion de Evento.
                                            if (resultado.OperacionExitosa)
                                            {
                                                //6.En caso de ser necesario modificamos los segmentos (Solo en inserción de paradas operativas)
                                                if (tipo_parada == TipoParada.Operativa)
                                                {
                                                    //Buscamos la parada operativa anterior y la parada operativa posterior
                                                    using (Parada objParadaAnteriorCarga = new Parada(BuscaParadaAnteriorOperativa(id_servicio, secuencia)), objParadaPosteriorCarga = new Parada(BuscaParadaPosteriorOperativa(id_servicio, secuencia)))
                                                    {
                                                        //En caso de insertarse un segmento al inicio
                                                        if (objParadaAnteriorCarga.id_parada == 0)
                                                        {
                                                            //6.1 Actualizamos todas las secuencias de los segmentos.
                                                            resultado = SegmentoCarga.ActualizaSecuenciaSegmentos(id_servicio, 0, id_usuario);

                                                            //Validamos Actualizacion de la Secuencias
                                                            if (resultado.OperacionExitosa)
                                                            {
                                                                //6.2 Insertamos el nuevo segmento con secuencia 1
                                                                resultado = SegmentoCarga.InsertaSegmentoCarga(id_servicio, 1, id_parada, objParadaPosteriorCarga.id_parada, id_ruta, id_usuario);

                                                                //Obtenemos el id del segmneto  recien insertado
                                                                id_segmento_nuevo = resultado.IdRegistro;
                                                                secuencia_posterior_carga = objParadaPosteriorCarga.secuencia_parada_servicio;

                                                                //Validamos Insercción Segmneto
                                                                if (resultado.OperacionExitosa)
                                                                {
                                                                    //6.3 Actualizamos la ubicación de Carga del Servicio
                                                                    resultado = objServicio.EditarServicio(objServicio.id_cliente_receptor, id_ubicacion, cita_parada
                                                                                                           , objServicio.id_ubicacion_descarga, objServicio.cita_descarga, objServicio.porte
                                                                                                           , objServicio.referencia_cliente, objServicio.observacion_servicio,
                                                                                                           id_usuario);

                                                                    //Validamos Actualización de la ubicación de Carga
                                                                    if (resultado.OperacionExitosa)
                                                                    {
                                                                        //6.4 Actualizamos el segmento de los Movimientos
                                                                        resultado = Movimiento.ActualizaSegmentoDeMovimientos(id_servicio, secuencia, objParadaPosteriorCarga.secuencia_parada_servicio, id_segmento_nuevo, id_usuario);
                                                                    }
                                                                }

                                                            }
                                                        }
                                                        else
                                                            //En caso de insertarse un segmento al final
                                                            if (objParadaPosteriorCarga.id_parada == 0)
                                                            {
                                                                //6.1 No se actualizan secuencia de segmentos 
                                                                //6.2 Insertamos el nuevo segmento con secuencia maxima mas 1
                                                                resultado = SegmentoCarga.InsertaSegmentoCarga(id_servicio, objParadaAnteriorCarga.id_parada, id_parada, id_ruta, id_usuario);

                                                                //Obtenemos el id del segmneto  recien insertado
                                                                id_segmento_nuevo = resultado.IdRegistro;
                                                                //Obtenemos la secuencia de la parada anterior de carga
                                                                secuencia_anterior_carga = objParadaAnteriorCarga.secuencia_parada_servicio;

                                                                //Validamos Insercción Segmneto
                                                                if (resultado.OperacionExitosa)
                                                                {

                                                                    //6.3 Actualizamos la ubicación de Descarga del servicio
                                                                    resultado = objServicio.EditarServicio(objServicio.id_cliente_receptor, objServicio.id_ubicacion_carga, objServicio.cita_carga
                                                                                                           , id_ubicacion, cita_parada, objServicio.porte
                                                                                                           , objServicio.referencia_cliente, objServicio.observacion_servicio,
                                                                                                           id_usuario);
                                                                    //Validamos Actualización
                                                                    if (resultado.OperacionExitosa)
                                                                    {
                                                                        //6.4 Actualizamos el segmento de los Movimientos
                                                                        resultado = Movimiento.ActualizaSegmentoDeMovimientos(id_servicio, objParadaAnteriorCarga.secuencia_parada_servicio, secuencia, id_segmento_nuevo, id_usuario);
                                                                    }
                                                                }
                                                            }
                                                            //En caso de insertarse un segmento intermedio
                                                            else
                                                            {
                                                                //Obtenemos la secuencia del segmento que contiene a las paradas anterior y posterior
                                                                decimal SecuenciaSegmento = SegmentoCarga.BuscaSecuenciaSegmento(id_servicio, objParadaAnteriorCarga.id_parada, objParadaPosteriorCarga.id_parada);

                                                                //6.1 Actualizamos todas las secuencias de los segmentos.
                                                                resultado = SegmentoCarga.ActualizaSecuenciaSegmentos(id_servicio, SecuenciaSegmento, id_usuario);

                                                                //Validamos Actualización de las secuencias
                                                                if (resultado.OperacionExitosa)
                                                                {
                                                                    //6.2 Insertamos el nuevo segmento
                                                                    resultado = SegmentoCarga.InsertaSegmentoCarga(id_servicio, SecuenciaSegmento + 1, id_parada, objParadaPosteriorCarga.id_parada, id_ruta, id_usuario);

                                                                    //Obtenemos el id del segmneto  recien insertado
                                                                    id_segmento_nuevo = resultado.IdRegistro;

                                                                    //Obtenemos Secuencias para su edición de movimiento
                                                                    secuencia_anterior_carga = objParadaAnteriorCarga.secuencia_parada_servicio;
                                                                    secuencia_posterior_carga = objParadaPosteriorCarga.secuencia_parada_servicio;

                                                                    //Validamos Insercción del segmento
                                                                    if (resultado.OperacionExitosa)
                                                                    {
                                                                        //6.3 Instanciamos segmento a partir de la secuencia obtenida
                                                                        using (SegmentoCarga objSegmentoCarga = new SegmentoCarga(id_servicio, SecuenciaSegmento))
                                                                        {
                                                                            //6.4 Actualizamos el destino del segmento original
                                                                            resultado = objSegmentoCarga.EditaSegmentoCarga(objSegmentoCarga.id_servicio, objSegmentoCarga.secuencia, objSegmentoCarga.EstatusSegmento,
                                                                                        objParadaAnteriorCarga.id_parada, id_parada, id_ruta, id_usuario);

                                                                            //Validamos Insercción Segmneto
                                                                            if (resultado.OperacionExitosa)
                                                                            {
                                                                                //6.5 Actualizamos el segmento de los Movimientos
                                                                                resultado = Movimiento.ActualizaSegmentoDeMovimientos(id_servicio, secuencia, objParadaPosteriorCarga.secuencia_parada_servicio, id_segmento_nuevo, id_usuario);
                                                                            }

                                                                        }
                                                                    }
                                                                }
                                                            }//If modificacion segmento/insercion segmento intermedio
                                                    }//Using instancia parada carga anterior y posterior
                                                }//Fin if Modificacion Segmento
                                                //Validamos Actualización
                                                if (resultado.OperacionExitosa)
                                                {
                                                    //7. Modificamos/Insertamos los movimientos 
                                                    using (Parada objParadaAnterior = new Parada(BuscaParadaAnterior(id_servicio, secuencia)), objParadaPosterior = new Parada(BuscaParadaPosterior(id_servicio, secuencia)))
                                                    {
                                                        //Validamos que no existan paradas anterior con la misma ubicacion
                                                        if (objParadaAnterior._id_ubicacion != id_ubicacion || objParadaAnterior.id_ubicacion == 0)
                                                        {
                                                            //Validamos que no existan paradas posterior con la misma ubicacion
                                                            if (objParadaPosterior._id_ubicacion != id_ubicacion || objParadaPosterior.id_ubicacion == 0)
                                                            {
                                                                //En caso de insertarse un movimiento al inicio
                                                                if (objParadaAnterior.id_parada == 0)
                                                                {
                                                                    //7.1 Actualizamos todas las secuencias de los movimientos.
                                                                    resultado = Movimiento.ActualizaSecuenciaMovimientos(id_servicio, 0, id_usuario);

                                                                    //Validamos Actualizacion de la Secuencias
                                                                    if (resultado.OperacionExitosa)
                                                                    {
                                                                        //7.2 Insertamos el nuevo movimiento con secuencia 1
                                                                        resultado = Movimiento.InsertaMovimiento(id_servicio, id_segmento_nuevo, 1, Movimiento.Tipo.Cargado, 0,
                                                                                                                 0, id_compania_emisor, id_parada, objParadaPosterior.id_parada, id_usuario);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    //En caso de insertarse un movimiento al final
                                                                    if (objParadaPosterior.id_parada == 0)
                                                                    {
                                                                        //Validamos que sea Parada Operativa
                                                                        if (tipo_parada == TipoParada.Operativa)
                                                                        {
                                                                            //7.1 No actualizamos todas las secuencias de los movimientos
                                                                            //7.2 Insertamos Movimiento al Final
                                                                            resultado = Movimiento.InsertaMovimiento(id_servicio, id_segmento_nuevo, Movimiento.Tipo.Cargado, 0,
                                                                                       0, id_compania_emisor, objParadaAnterior.id_parada, id_parada, id_usuario);

                                                                            //Validamos Insercción de Movimiento
                                                                            if (resultado.OperacionExitosa)
                                                                            {
                                                                                //Asignamos Variable Movimiento
                                                                                int id_movimiento = resultado.IdRegistro;

                                                                                //Si existe fecha de llegada  de la parada
                                                                                if (!Fecha.EsFechaMinima(objParadaAnterior.fecha_llegada))
                                                                                {
                                                                                    //Validamos Actualización de Operadores
                                                                                    if (resultado.OperacionExitosa)
                                                                                    {
                                                                                        //Creamos Asignación de Recursos
                                                                                        resultado = MovimientoAsignacionRecurso.CreaMovimientosAsignacionRecursoParaParadaAlFinal(objParadaAnterior.id_parada, Movimiento.BuscamosMovimientoParadaDestino(id_servicio, objParadaAnterior.id_parada)
                                                                                            , id_movimiento, id_usuario);
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            //Establecemos Mensaje error
                                                                            resultado = new RetornoOperacion("No es posible insertar una Parada de Servicio al final.");
                                                                        }
                                                                    }
                                                                    //En caso de insertar un movimiento intermedio.
                                                                    else
                                                                    {
                                                                        // Obtenemos el movimiento coincidente que contiene la parada anterior y la parada posterior.
                                                                        using (Movimiento objMovimiento = new Movimiento(objParadaAnterior.id_parada, objParadaPosterior.id_parada, id_servicio))
                                                                        {
                                                                            //7.1 Actualizamos la secuencia de los movimientos
                                                                            resultado = Movimiento.ActualizaSecuenciaMovimientos(id_servicio, objMovimiento.secuencia_servicio, id_usuario);

                                                                            //Validamos actualización de las secuencias
                                                                            if (resultado.OperacionExitosa)
                                                                            {
                                                                                //Validando Tipo de Parada Operativa
                                                                                if ((Parada.TipoParada)tipo_parada == Parada.TipoParada.Operativa)
                                                                                {
                                                                                    //De acuerdo a las paradas existentes de carga asignamos Segmento
                                                                                    //Si la secuencia es al final
                                                                                    if (secuencia_posterior_carga == 0)
                                                                                    {
                                                                                        id_segmento_insercion = objMovimiento.id_segmento_carga;
                                                                                        id_segmento_edicion = id_segmento_nuevo;

                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        //Si la secuencia es al Inicio
                                                                                        if (secuencia_anterior_carga == 0)
                                                                                        {
                                                                                            id_segmento_edicion = objMovimiento.id_segmento_carga;
                                                                                            id_segmento_insercion = id_segmento_nuevo;
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            //Si la secuencia es en medio
                                                                                            id_segmento_edicion = objMovimiento.id_segmento_carga;
                                                                                            id_segmento_insercion = id_segmento_nuevo;
                                                                                        }
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    //Validando Tipo de Parada Servicio
                                                                                    id_segmento_insercion = objMovimiento.id_segmento_carga;
                                                                                    id_segmento_edicion = objMovimiento.id_segmento_carga;
                                                                                }

                                                                                //7.2 Insertamos Movimiento
                                                                                resultado = Movimiento.InsertaMovimiento(id_servicio, id_segmento_insercion, objMovimiento.secuencia_servicio + 1,
                                                                                            Movimiento.Tipo.Cargado, 0, 0, id_compania_emisor
                                                                                          , id_parada, objParadaPosterior.id_parada, id_usuario);

                                                                                //Validamos Inserción del movimiento.
                                                                                if (resultado.OperacionExitosa)
                                                                                {
                                                                                    //7.3 Actualizamos el destino del movimiento obtenido
                                                                                    resultado = objMovimiento.EditaMovimiento(objMovimiento.id_servicio, id_segmento_edicion,
                                                                                                  objMovimiento.secuencia_servicio, (Movimiento.Estatus)objMovimiento.id_estatus_movimiento,
                                                                                                 (Movimiento.Tipo)objMovimiento.id_tipo_movimiento, 0, 0, objMovimiento.id_compania_emisor,
                                                                                                  objMovimiento.id_parada_origen, id_parada, id_usuario);



                                                                                }//fin insercción de movimientos
                                                                            }//fin actualización de movimientos
                                                                        }//using movimiento
                                                                    }//if modificación del movimiento/intermedio, final
                                                                }
                                                            }//Validación de ubicación parada posterior
                                                            else
                                                            {
                                                                //Establecmeos Error
                                                                resultado = new RetornoOperacion("No es posible la insercción de paradas continuas con la misma ubicación.");
                                                            }
                                                        }//Validación de ubicación parada anterior
                                                        else
                                                        {
                                                            //Establecmeos Error
                                                            resultado = new RetornoOperacion("No es posible la insercción de paradas continuas con la misma ubicación.");
                                                        }
                                                    }//using instancia parada anterior y posterior

                                                    //Calculamos Kilometraje
                                                    //Validamos Resultado
                                                    if (resultado.OperacionExitosa)
                                                    {
                                                        if (objServicio.ActualizaServicio())
                                                        {
                                                            //7.4 Calculamos Kilometraje del Servicio
                                                            resultado = objServicio.CalculaKilometrajeServicio(id_usuario);

                                                            //Validmaos Actualización
                                                            if (resultado.OperacionExitosa)
                                                            {
                                                                //Devolmvemos Id Parada
                                                                resultado = new RetornoOperacion(id_parada);
                                                            }

                                                        }
                                                        else
                                                        {
                                                            resultado = new RetornoOperacion("No se puede actualizar los atributos del servicio.");
                                                        }
                                                    }
                                                }//fin actualizaión movimientos/insercción evento
                                            }//fin if insercion evento
                                        }//fin if insercion parada
                                    }//Fin Actualización de Secuencias
                                }//Fin Validación de Citas de Paradas
                            }
                        }//Fin Validación Total Pardas
                        else
                        {
                            //Establecmeos Error
                            resultado = new RetornoOperacion("No se puede regitrar la parada ya que fue modificada desde la última vez que fue consultada.");
                        }
                    }//Fin validación estatus del servicio
                    else
                    {
                        //Establecemos Mensaje Error
                        resultado = new RetornoOperacion("El estatus del servicio no permite su edición.");
                    }
                }

                //Validamos Resultado
                if (resultado.OperacionExitosa)
                {
                    //Si la secuencia es mayor a 1
                    if (secuencia > 1)
                    {
                        //Instanciando servicio del movimiento
                        using (Documentacion.Servicio srv = new Documentacion.Servicio(id_servicio))
                        {
                            //Realizando actualización de plataforma de terceros (proveedor satelital)
                            srv.ActualizaPlataformaTerceros(id_usuario);
                        }
                    }

                    resultado = new RetornoOperacion(id_parada);
                    //Validamos Transacción
                    scope.Complete();
                }
            }//Fin Parda Transaccion

            //Devolvemos Resultado
            return resultado;
        }

        /// <summary>
        /// Método encargado de Insertar una Parada
        /// </summary>
        /// <param name="id_servicio">Id de Servicio ligado a la parada, puede ser 0 cuando no este ligado al Servicio</param>
        /// <param name="tipo_parada">Define el Tipo de Parada, pretende hacer distinción entre paradas operativas y de Servicio</param>
        /// <param name="id_ubicacion">Id de Ubicación donde se está realizando la parada</param>
        /// <param name="cita_parada">Fecha y Hora de la llegada de espera de la parada</param>
        /// <param name="geo_ubicacion">Geo ubicación en caso de que no este definido el Id de Ubicación</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaParada(int id_servicio, TipoParada tipo_parada, int id_ubicacion, DateTime cita_parada, SqlGeography geo_ubicacion, int id_usuario)
        {
            //Declaramos la variables a utilizar 
            string descripcion = "";
            //Validamos si existe ubicacion
            if (id_ubicacion == 0)
            {
                //En caso de no existir la ubicacion obtenemos el geocodificacion inversa de las coordenadas
                //TODO: Metodo geocodificacion inversa
                descripcion = "Valor metodo geocodificacion inversa";
            }
            else
            {
                //Instanciamos  la Ubicación para obtener la descripción.
                using (Ubicacion objUbicacion = new Ubicacion(id_ubicacion))
                {
                    //Obtenemos la descripcion de la ubicacion
                    descripcion = objUbicacion.descripcion.ToUpper();
                    //Instanciamos Ciudad
                    using (Ciudad objCiudad = new Ciudad(objUbicacion.id_ciudad))
                    {
                        //Validamos que exista la Ciudad
                        if (objCiudad.id_ciudad > 0)
                        {
                            //Obtenemos la descripcion de la ubicacion
                            descripcion += " " + objCiudad.ToString();
                        }
                    }
                }
            }

            //Inicializando arreglo de parámetros
            object[] param = { 1, 0, id_servicio, obtieneSecuenciaParada(id_servicio), tipo_parada, EstatusParada.Registrado, id_ubicacion, geo_ubicacion,
                                 descripcion, tipo_parada == TipoParada.Operativa ? Fecha.ConvierteDateTimeObjeto(cita_parada) : null,
                                 null, 0, 0, null, 0, 0, id_usuario,
                                 true, null, "", ""};

            //Realizando la actualización
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);
        }

        /// <summary>
        /// Método encargado de Insertar una Parada solo para la funcionalidad de cambio de Ubicación de la Unidad
        /// </summary>
        /// <param name="id_servicio">Id de Servicio ligado a la parada, puede ser 0 cuando no este ligado al Servicio</param>
        /// <param name="tipo_parada">Define el Tipo de Parada, pretende hacer distinción entre paradas operativas y de Servicio</param>
        /// <param name="id_ubicacion">Id de Ubicación donde se está realizando la parada</param>
        /// <param name="cita_parada">Fecha y Hora de la llegada de espera de la parada</param>
        /// <param name="geo_ubicacion">Geo ubicación en caso de que no este definido el Id de Ubicación</param>
        /// <param name="fecha_llegada">Fecha de Llegada</param>
        /// <param name="tipo_actualizacion_llegada">Tipo Actualización de la LLegada (Manual, GPS)</param>
        /// <param name="id_razon_llegada_tarde">Razón por la cual Llegó Tarde la Unidad</param>
        /// <param name="fecha_salida">Fecha de Salida</param>
        /// <param name="tipo_actualizacion_salida">Tipo Actualización de la Salida</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaParada(int id_servicio, TipoParada tipo_parada, int id_ubicacion, DateTime cita_parada, SqlGeography geo_ubicacion,
                                                    DateTime fecha_llegada, TipoActualizacionLlegada tipo_actualizacion_llegada, int id_razon_llegada_tarde, DateTime fecha_salida,
                                                     TipoActualizacionSalida tipo_actualizacion_salida, byte id_razon_salida_tarde, int id_usuario)
        {
            //Declaramos la variables a utilizar  
            string descripcion = "";
            //Validamos si existe ubicacion
            if (id_ubicacion == 0)
            {
                //En caso de no existir la ubicacion obtenemos el geocodificacion inversa de las coordenadas
                //TODO: Metodo geocodificacion inversa
                descripcion = "Valor metodo geocodificacion inversa";
            }
            else
            {
                //Instanciamos  la Ubicación para obtener la descripción.
                using (Ubicacion objUbicacion = new Ubicacion(id_ubicacion))
                {
                    //Obtenemos la descripcion de la ubicacion
                    descripcion = objUbicacion.descripcion.ToUpper();
                    //Instanciamos Ciudad
                    using (Ciudad objCiudad = new Ciudad(objUbicacion.id_ciudad))
                    {
                        //Validamos que exista la Ciudad
                        if (objCiudad.id_ciudad > 0)
                        {
                            //Obtenemos la descripcion de la ubicacion
                            descripcion += " " + objCiudad.ToString();
                        }
                    }
                }
            }

            //Inicializando arreglo de parámetros
            object[] param = { 1, 0, id_servicio, 0, tipo_parada, EstatusParada.Terminado, id_ubicacion, geo_ubicacion,
                                 descripcion, tipo_parada == TipoParada.Operativa ? Fecha.ConvierteDateTimeObjeto(cita_parada) : null,
                                 Fecha.ConvierteDateTimeObjeto(fecha_llegada), tipo_actualizacion_llegada, id_razon_llegada_tarde, Fecha.ConvierteDateTimeObjeto(fecha_salida),
                                 tipo_actualizacion_salida, id_razon_salida_tarde, id_usuario, true, null, "", ""};

            //Realizando la actualización
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);
        }

        /// <summary>
        /// Método encargado de Insertar una Parada, asignando el valor de la secuencia
        /// </summary>
        /// <param name="id_servicio">Id de Servicio ligado a la parada, puede ser 0 cuando no este ligado al Servicio</param>
        /// <param name="secuencia">Asigna el numero de Secuencia por default a la parada</param>
        /// <param name="tipo_parada">Define el Tipo de Parada, pretende hacer distinción entre paradas operativas y de Servicio</param>
        /// <param name="id_ubicacion">Id de Ubicación donde se está realizando la parada</param>
        /// <param name="cita_parada">Fecha y Hora de la llegada de espera de la parada</param>
        /// <param name="geo_ubicacion">Geo ubicación en caso de que no este definido el Id de Ubicación</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaParada(int id_servicio, decimal secuencia, TipoParada tipo_parada, int id_ubicacion, DateTime cita_parada, SqlGeography geo_ubicacion, int id_usuario)
        {
            //Declaramos la variables a utilizar 
            string descripcion = "";
            //Validamos si existe ubicacion
            if (id_ubicacion == 0)
            {
                //En caso de no existir la ubicacion obtenemos el geocodificacion inversa de las coordenadas
                //TODO: Metodo geocodificacion inversa
                descripcion = "Valor metodo geocodificacion inversa";
            }
            else
            {
                //Instanciamos  la Ubicación para obtener la descripción.
                using (Ubicacion objUbicacion = new Ubicacion(id_ubicacion))
                {
                    //Obtenemos la descripcion de la ubicacion
                    descripcion = objUbicacion.descripcion.ToUpper();
                    //Instanciamos Ciudad
                    using (Ciudad objCiudad = new Ciudad(objUbicacion.id_ciudad))
                    {
                        //Validamos que exista la Ciudad
                        if (objCiudad.id_ciudad > 0)
                        {
                            //Obtenemos la descripcion de la ubicacion
                            descripcion += " " + objCiudad.ToString();
                        }
                    }
                }
            }

            //Inicializando arreglo de parámetros
            object[] param = { 1, 0, id_servicio, secuencia, tipo_parada, EstatusParada.Registrado, id_ubicacion, geo_ubicacion,
                                 descripcion, tipo_parada == TipoParada.Operativa ? Fecha.ConvierteDateTimeObjeto(cita_parada) : null,
                                 null, 0, 0, null, 0, 0, id_usuario, true, null, "", ""};

            //Realizando la actualización
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);
        }
        /// <summary>
        /// Carga los registros parada con el formato requerido para visualización en el control de documentación
        /// </summary>
        /// <param name="id_servicio">Id de Servicio</param>
        /// <returns></returns>
        public static DataTable CargaParadasServicioVisualziacionControlDocumentacion(int id_servicio)
        {
            //Declatrando objeto de resultado
            DataTable mit = null;

            //Creando arreglo de parámetros para criterios de consulta
            object[] param = { 22, 0, id_servicio, 0, 0, 0, 0, null, "", null, null, 0, 0, null, 0, 0, 0, false, null, "", "" };

            //Buscando coincidencias
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Si hay registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }
    }
}
