using SAT_CL.Global;
using SAT_CL.Liquidacion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using TSDK.Base;
using TSDK.Datos;
using System.IO;
using System.Net;
using System.Diagnostics;

namespace SAT_CL.Despacho
{
    public partial class Movimiento
    {
        #region Métodos Operativos

        /// <summary>
        /// Deshabilita un Movimiento en Vacio
        /// </summary>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaMovimientoVacioTerminado(int id_usuario)
        {
            //Declaramos objeto resultado
            RetornoOperacion resultado = new RetornoOperacion("El movimiento debe estar en estatus Terminado");

            //Validando estatus actual del movimiento
            if (this.EstatusMovimiento == Estatus.Terminado)
            {
                //Iniciando transacción
                using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    //Instanciamos la Parada Origen
                    using (Parada objParadaOrigen = new Parada(this._id_parada_origen), objParadadDestino = new Parada(this._id_parada_destino))
                    {
                        //Cargamos Estancias ligadas a un Id Movimiento en Estatus Terminado
                        using (DataTable mitEstanciasOrigen = EstanciaUnidad.CargaEstanciasTerminadasParada(this._id_parada_origen))
                        {
                            //Validando que las unidades involucradas se encuentren:
                            //Asociadas aún al servicio y en estatus ocupado
                            //Disponibles en la ubicación de fin del movimiento y sin ningún movimiento asignado
                            resultado = MovimientoAsignacionRecurso.ValidaRecursosParaReversaTerminaMovimiento(this._id_movimiento);

                            //Validamos Resultado
                            if (resultado.OperacionExitosa)
                            {
                                //Validamos que no existan pagos ligados al movimeinto
                                resultado = validaPagos();

                                //Validamos Resultado
                                if (resultado.OperacionExitosa)
                                {
                                    //Deshabilitamos Estancias 
                                    resultado = EstanciaUnidad.DeshabilitaEstanciaUnidadesMovimientoVacio(this._id_movimiento, this._id_parada_destino, id_usuario);

                                    //Validamos Resultado
                                    if (resultado.OperacionExitosa)
                                    {
                                        //Deshabilitamos Parada Destino
                                        resultado = objParadadDestino.DeshabilitaParada(id_usuario);

                                        //Validamos Resultado
                                        if (resultado.OperacionExitosa)
                                        {
                                            //Reversa los movimientos de la Parada Origen
                                            resultado = ReversaInicioMovimietoVacio(mitEstanciasOrigen, MovimientoAsignacionRecurso.Estatus.Terminado, id_usuario);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    //Si no hay errores
                    if (resultado.OperacionExitosa)
                        //Terminando transacción
                        scope.Complete();
                }
            }
            else
            {
                //Establecemos Mesnaje Error
                resultado = new RetornoOperacion("El estatus del movimiento no permite su eliminación");
            }
            //Devolvemos resultado
            return resultado;
        }

        /// <summary>
        /// Método encargado de Reversa el Inicio de un Movimiento Vacio
        /// </summary>
        /// <param name="mitEstanciasOrigen">Estancias Origen</param>
        /// <param name="estatus">Estatus de la Asignación que se desea editar</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        private RetornoOperacion ReversaInicioMovimietoVacio(DataTable mitEstanciasOrigen, MovimientoAsignacionRecurso.Estatus estatus, int id_usuario)
        {
            //Declaramos objeto Retorno
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Iniciando transacción
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciamos la Parada Origen
                using (Parada objParadaOrigen = new Parada(this._id_parada_origen))
                {
                    //Localizando parada comodín de la ubicación origen
                    int id_parada_origen = Parada.ObtieneParadaComodinUbicacion(objParadaOrigen.id_ubicacion, true, id_usuario);

                    //Validamos existencia de Parada Comodin
                    if (id_parada_origen > 0)
                    {
                        
                            //Iniciamos Estancias de la parada origen del servicio
                            resultado = EstanciaUnidad.IniciaEstanciasTerminadasParada(this._id_parada_origen, id_usuario);
                        
                        //Validamos Resultado
                        if (resultado.OperacionExitosa)
                        {
                            //Actuakizamos Kilometraje de las Unidades de Movimiento
                            resultado = MovimientoAsignacionRecurso.ActualizaKilometrajeUnidadesMovimiento(this._id_movimiento, -this._kms, id_usuario);

                            //Validamos Resultado
                            if (resultado.OperacionExitosa)
                            {
                                //Actualizando estatus de recursos asignados a Disponibles
                                resultado = MovimientoAsignacionRecurso.ActualizaEstatusRecursosTerminadosADisponible(this._id_movimiento, id_parada_origen, objParadaOrigen.fecha_llegada, estatus, id_usuario);

                                //Validamos Resultado
                                if (resultado.OperacionExitosa)
                                {
                                    //Deshabilita Asignaciones
                                    resultado = MovimientoAsignacionRecurso.DeshabilitaMovimientosAsignacionesRecursosVacio(this._id_movimiento, id_usuario);
                                    //Validamos Resultado
                                    if (resultado.OperacionExitosa)
                                    {
                                        //Recuperando los Id de Recurso de las Unidades
                                        List<int> unidades = (from DataRow r in mitEstanciasOrigen.Rows
                                                              select r.Field<int>("IdRecurso")).DefaultIfEmpty().ToList();

                                        //Cambiamos el Id de Parada de las Estrancias
                                        resultado = EstanciaUnidad.CambiaParadaEstanciaUnidadesUbicacion(unidades, id_parada_origen, objParadaOrigen.id_ubicacion, id_usuario);

                                        //Validamos Resultado
                                        if (resultado.OperacionExitosa)
                                        {
                                            //Deshabilitamos Parada  Origen
                                            resultado = objParadaOrigen.DeshabilitaParada(id_usuario);

                                            //Validamos Resultado
                                            if (resultado.OperacionExitosa)
                                            {
                                                //Validamos Deshabilitación de Movimiento
                                                resultado = this.editaMovimiento(this._id_servicio, this._id_segmento_carga, this._secuencia_servicio, (Estatus)this._id_estatus_movimiento, (Tipo)this._id_tipo_movimiento
                                                                  , this._kms, this._kms_maps, this._id_compania_emisor, this._id_parada_origen, this._id_parada_destino, id_usuario, false);
                                            }
                                        }

                                    }
                                }
                            }
                            else
                                //Mostramos Mensaje Error
                                resultado = new RetornoOperacion(string.Format("Error al actualizar kilometraje de unidades asociadas: {0}", resultado.Mensaje));
                        }
                    }
                    else
                        //Mostramos Mensaje Error
                        resultado = new RetornoOperacion("No se encontró datos complementarios de la parada comodin.");
                }
                //Validamos Resultado
                if(resultado.OperacionExitosa)
                {
                    //Validamos resultado
                    scope.Complete();
                }
            }
            //Devolvemos Valor
            return resultado;
        }

        /// <summary>
        /// Método encargado de Deshabilitar un Movimiento Vacio Iniciado
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaMovimientoVacioIniciado(int id_usuario)
        {
            //Declaramos objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();

             //Iniciando transacción
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Validamos que el estatus actual del  movimiento sea Iniciado
                if (this.EstatusMovimiento == Estatus.Iniciado)
                {
                    //Validamos que no existan pagos ligados al movimeinto
                    resultado = validaPagos();

                    //Validamos que no existan Pagos
                    if (resultado.OperacionExitosa)
                    {
                        //Cargamos Estancias ligadas a un Id Movimiento en Estatus Iniciado
                        using (DataTable mitEstanciasOrigen = EstanciaUnidad.CargaEstanciasTerminadasParada(this._id_parada_origen))
                        {
                            //Reversa Inicio de  Moimiento
                            resultado = ReversaInicioMovimietoVacio(mitEstanciasOrigen, MovimientoAsignacionRecurso.Estatus.Iniciado, id_usuario);
                        }

                        //Validamos Resultado
                        if (resultado.OperacionExitosa)
                        {
                            //Instanciamos Parada Destino
                            using (Parada objParadaDestino = new Parada(this._id_parada_destino))
                            {
                                //Deshabilitamos Parada Destino
                                resultado = objParadaDestino.DeshabilitaParada(id_usuario);
                            }
                        }

                    }
                }
                else
                    //Establecemos Mensaje 
                    resultado = new RetornoOperacion("El estatus del movimiento no permite su edición.");
                //Validamos Resultado
                if(resultado.OperacionExitosa)
                {
                    scope.Complete();
                }
            }
            //Devolvemos Valor Retorno
            return resultado;
        }

        /// <summary>
        /// Realiza las acciones necesarias para la reversa en la actualización de fin de movimiento: elimina estancias, inicia asignaciones del movimiento, elimina asignaciones copiadas al movimiento siguiente (en caso de existir), los recursos se colocan en estatus tránsito, se resta el kilometraje del movimiento en el despacho de su servicio y el movimiento se coloca como iniciado
        /// </summary>
        /// <param name="id_usuario">Id de Usuario que actualiza</param>
        /// <returns></returns>
        public RetornoOperacion ReversaTerminaMovimientoVacio(int id_usuario)
        {
            //Declarando objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion("El movimiento debe estar en estatus Terminado");

            //Validando estatus actual del movimiento
            if (this.EstatusMovimiento == Estatus.Terminado)
            {
                //Iniciando transacción
                using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    //Validando que las unidades involucradas se encuentren:
                    //Asociadas aún al servicio y en estatus ocupado
                    //Disponibles en la ubicación de fin del movimiento y sin ningún movimiento asignado
                    resultado = MovimientoAsignacionRecurso.ValidaRecursosParaReversaTerminaMovimiento(this._id_movimiento);

                    //Validamos Resultado
                    if (resultado.OperacionExitosa)
                    {
                        //Validamos que no existan pagos ligados al movimeinto
                        resultado = validaPagos();

                        //Si es válido realizar la reversa
                        if (resultado.OperacionExitosa)
                        {
                            //Actualziando el kilometraje de las unidades involucradas
                            resultado = MovimientoAsignacionRecurso.ActualizaKilometrajeUnidadesMovimiento(this._id_movimiento, -this._kms, id_usuario);
                            //Si no hubo errores
                            if (resultado.OperacionExitosa)
                            {
                                //instanciando parad de origen
                                using (Parada paradaOrigen = new Parada(this._id_parada_origen))
                                {
                                    //Si la parada se encontró
                                    if (paradaOrigen.habilitar)
                                        //Eliminando estancias creadas para dichos recursos
                                        resultado = EstanciaUnidad.DeshabilitaEstanciaUnidadesReversaFinMovimiento(this._id_movimiento, paradaOrigen.fecha_salida, id_usuario);
                                    else
                                        resultado = new RetornoOperacion("No fue localizada la información de la parada de origen.");
                                }

                                //Si no hubo errores
                                if (resultado.OperacionExitosa)
                                {
                                    //Actualziando estatus de movimiento a iniciado
                                    resultado = ActualizaEstatusMovimiento(Estatus.Iniciado, id_usuario);

                                    //Validamos Resultado
                                    if (resultado.OperacionExitosa)
                                    {
                                        //Instanciamos Parada Destino
                                        using (Parada objParadaDestino = new Parada(this._id_parada_destino))
                                            //Editamos Pararda
                                            resultado = objParadaDestino.CambiaParadaARegistrada(id_usuario);
                                    }
                                    else
                                        resultado = new RetornoOperacion("No fue posible actualizar el estatus del movimiento.");
                                }
                            }
                            else
                                resultado = new RetornoOperacion(string.Format("Error al actualizar kilometraje de unidades involucradas: {0}", resultado.Mensaje));
                        }
                    }

                    //Si no hay errores
                    if (resultado.OperacionExitosa)
                        //Terminando transacción
                        scope.Complete();
                }
            }
            //Devolviendo resultado
            return resultado;
        }


        /// <summary>
        /// Mètodo encargado de validar pagos ligados al movimiento
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion validaPagos()
        {
            //Declaramos objeto retorno
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Obtenemos Asignaciones Liquidadas
            using (DataTable mitAsignaciones = MovimientoAsignacionRecurso.CargaAsignacionesLiquidadas(this._id_movimiento))
            {
                //Validamos Asignaciones
                if (Validacion.ValidaOrigenDatos(mitAsignaciones))
                {
                    //Recorremos  cada una de las asignaciones liquidadas
                    foreach (DataRow r in mitAsignaciones.Rows)
                    {
                        //Intsanciamos Asignación
                        using (MovimientoAsignacionRecurso objAsignacion = new MovimientoAsignacionRecurso(r.Field<int>("Id")))
                        {
                            //De acuerdo al tipo de asignación
                            switch ((MovimientoAsignacionRecurso.Tipo)(objAsignacion.id_tipo_asignacion))
                            {
                                //Si es Operador
                                case MovimientoAsignacionRecurso.Tipo.Operador:
                                    //Instanciamos Operador
                                    using (Operador objOperador = new Operador(objAsignacion.id_recurso_asignado))
                                    {
                                        //establecemos mensaje error
                                        resultado = new RetornoOperacion("La asignación del operador " + objOperador.nombre + " se encuentra liquidada.");

                                    }
                                    break;
                                //Si es tercero
                                case MovimientoAsignacionRecurso.Tipo.Tercero:
                                    //Instanciamos Tercero
                                    using (CompaniaEmisorReceptor objCompania = new CompaniaEmisorReceptor(objAsignacion.id_recurso_asignado))
                                    {
                                        //establecemos mensaje error
                                        resultado = new RetornoOperacion("La asignación del Tercero " + objCompania.nombre_corto + " se encuentra liquidada.");

                                    }
                                    break;
                                //Si es Unidad
                                case MovimientoAsignacionRecurso.Tipo.Unidad:
                                    //Instanciamos Unidad
                                    using (Unidad objUnidad = new Unidad(objAsignacion.id_recurso_asignado))
                                    {
                                        //establecemos mensaje error
                                        resultado = new RetornoOperacion("La asignación de la unidad " + objUnidad.numero_unidad + " se encuentra liquidada.");

                                    }
                                    break;
                            }
                        }
                    }
                }
                //Validamos si existen pagos
                else if (PagoMovimiento.ValidaPagoMovimiento(this._id_movimiento))
                {
                    //Establecemos Mensaje Error
                    resultado = new RetornoOperacion("Existen pagos ligados al movimiento.");
                }
            }

            //Retornamos resultado
            return resultado;
        }

        /// <summary>
        /// Edita la Fecha de Termino del Movimiento
        /// </summary>
        /// <param name="fecha_termino">Fecha de termino del movimiento / Inicio de la Estancia</param>
        /// <param name="tipo_llegada">Tipo actualización  de la fecha de llegada de la parada</param>
        /// <param name="tipo_actualizacion_inicio">Tipo actualización  de la fecha de inicio de la estancia</param>
        /// <param name="id_usuario">Id Usuario actualiza</param>
        /// <returns></returns>
        public RetornoOperacion EditaFechaTerminoMovimiento(DateTime fecha_termino, Parada.TipoActualizacionLlegada tipo_llegada, EstanciaUnidad.TipoActualizacionInicio tipo_actualizacion_inicio, int id_usuario)
        {
            //Declarando objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion();

            //Validando estatus actual del movimiento
            if (this.EstatusMovimiento == Estatus.Terminado)
            {
                //Iniciando transacción
                using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {

                    //Validando que las unidades involucradas se encuentren:
                    //Asociadas aún al servicio y en estatus ocupado
                    //Disponibles en la ubicación de fin del movimiento y sin ningún movimiento asignado
                    resultado = MovimientoAsignacionRecurso.ValidaRecursosParaReversaTerminaMovimiento(this._id_movimiento);

                    //Validamos Resultado
                    if (resultado.OperacionExitosa)
                    {
                        //Validamos que no existan pagos ligados al movimeinto
                        resultado = validaPagos();

                        //Validamos Resultado
                        if (resultado.OperacionExitosa)
                        {
                            //Actualizamos la fecha de llegada de las Estancias
                            resultado = EstanciaUnidad.EditaEstanciasUnidadFechaInicio(this._id_parada_destino, fecha_termino, tipo_actualizacion_inicio, id_usuario);

                            //Validamos Resultado
                            if (resultado.OperacionExitosa)
                            {
                                //Instanciamos Parada destino
                                using (Parada objParadaDestino = new Parada(this._id_parada_destino))
                                {
                                    //Editamos fecha de llegada de la parada
                                    resultado = objParadaDestino.EditaFechaLlegadaParada(fecha_termino, tipo_llegada, 0, id_usuario);
                                }
                            }
                        }
                    }
                }
            }
            //Devolvemos Resultado
            return resultado;
        }

        /// <summary>
        /// Determina los criterios de búsqueda definidos para aplicación de una tarifa de pago sobre el movimiento (valores de columna y fila)
        /// </summary>
        /// <param name="columna">Criterio de filtrdo sobre columnas de matriz de tarifa</param>
        /// <param name="fila">Criterio de filtrado sobre filas de matriz de tarifa</param>
        /// <param name="id_base_tarifa">Id de Base de Tarifa</param>
        /// <param name="descripcion_columna">Rótulo de Columna</param>
        /// <param name="descripcion_fila">Rótulo de Fila</param>
        /// <param name="operador_columna">Operador de búsqueda en columna</param>
        /// <param name="operador_fila">Operador de búsqueda en fila</param>
        public RetornoOperacion ExtraeCriteriosMatrizTarifaPago(TarifasPago.Tarifa.CriterioMatrizTarifa columna, TarifasPago.Tarifa.CriterioMatrizTarifa fila, int id_base_tarifa, out string descripcion_columna, out string descripcion_fila, out string operador_columna, out string operador_fila)
        {
            //Declarando objeto de resultado, sin errores
            RetornoOperacion resultado = new RetornoOperacion(this._id_servicio);

            //Asignando valores por default a parámetros de salida
            descripcion_columna = descripcion_fila = operador_columna = operador_fila = "";

            //Determinando el tipo de filtrado columna aplicable
            switch (columna)
            {
                case TarifasPago.Tarifa.CriterioMatrizTarifa.Producto:
                    //Obteniendo el producto correspondiente
                    int id_producto = Documentacion.ServicioProducto.ObtieneProductoPrincipalParada(this._id_parada_origen);

                    //Si el producto existe
                    if (id_producto > 0)
                        descripcion_columna = id_producto.ToString();
                    else
                        resultado = new RetornoOperacion("En este movimiento no se encontró algún producto, no es posible aplicar una tarifa con este criterio.");

                    //Asignando operador de comparación
                    operador_columna = "=";
                    break;
                case TarifasPago.Tarifa.CriterioMatrizTarifa.Ciudad:
                    //Instanciando Parada de destino del movimiento
                    using (Parada parada = new Parada(this._id_parada_destino))
                    using (SAT_CL.Global.Ubicacion ubicacion = new Global.Ubicacion(parada.id_ubicacion))
                        descripcion_columna = ubicacion.id_ciudad.ToString();
                    operador_columna = "=";
                    break;
                case TarifasPago.Tarifa.CriterioMatrizTarifa.Ubicacion:
                    //Instanciando Parada de destino del movimiento
                    using (Parada parada = new Parada(this._id_parada_destino))
                        descripcion_columna = parada.id_ubicacion.ToString();
                    operador_columna = "=";
                    break;
                case TarifasPago.Tarifa.CriterioMatrizTarifa.Distancia:
                    descripcion_columna = this._kms.ToString();
                    operador_columna = "<=";

                    //Si no hay kilometros
                    if (this._kms <= 0)
                        resultado = new RetornoOperacion("El kilometraje calculado es igual a 0, no se puede aplicar tarifa por este criterio.");
                    break;
                case TarifasPago.Tarifa.CriterioMatrizTarifa.Paradas:
                    descripcion_columna = "1";
                    operador_columna = "<=";
                    break;
                case TarifasPago.Tarifa.CriterioMatrizTarifa.Volumen:
                    //Obteniendo volumen total
                    decimal volumen = Documentacion.ServicioProducto.ObtieneTotalVolumenParada(this._id_parada_origen, id_base_tarifa);

                    //Si el volumen es mayor a 0
                    if (volumen > 0)
                        descripcion_columna = volumen.ToString();
                    else
                        resultado = new RetornoOperacion("El volúmen transportado es igual a 0, no se puede aplicar tarifa por este criterio.");

                    operador_columna = "<=";
                    break;
                case TarifasPago.Tarifa.CriterioMatrizTarifa.Peso:
                    //Obteniendo el peso total
                    decimal peso = Documentacion.ServicioProducto.ObtieneTotalPesoParada(this._id_parada_origen, id_base_tarifa);

                    //Si el peso es mayor a 0
                    if (peso > 0)
                        descripcion_columna = peso.ToString();
                    else
                        resultado = new RetornoOperacion("El peso transportado es igual a 0, no se puede aplicar tarifa por este criterio.");

                    operador_columna = "<=";
                    break;
                case TarifasPago.Tarifa.CriterioMatrizTarifa.Cantidad:
                    //Obteniendo el conteo total de productos
                    decimal cantidad = Documentacion.ServicioProducto.ObtieneTotalCantidadParada(this._id_parada_origen);

                    //Si la cantidad es mayor a 0
                    if (cantidad > 0)
                        descripcion_columna = cantidad.ToString();
                    else
                        resultado = new RetornoOperacion("El conteo transportado es igual a 0, no se puede aplicar tarifa por este criterio.");

                    operador_columna = "<="; break;
                default:
                    descripcion_columna = "0";
                    operador_columna = "=";
                    break;
            }

            //Si no hay error de búsqueda de criterios hasta este punto
            if (resultado.OperacionExitosa)
            {
                //Determinando el tipo de filtrado de fila aplicable
                switch (fila)
                {
                    case TarifasPago.Tarifa.CriterioMatrizTarifa.Producto:
                        //Obteniendo el producto correspondiente
                        int id_producto = Documentacion.ServicioProducto.ObtieneProductoPrincipalParada(this._id_parada_origen);

                        //Si el producto existe
                        if (id_producto > 0)
                            descripcion_fila = id_producto.ToString();
                        else
                            resultado = new RetornoOperacion("En este movimiento no se encontró algún producto, no es posible aplicar una tarifa con este criterio.");

                        //Asignando operador de comparación
                        operador_fila = "=";
                        break;
                    case TarifasPago.Tarifa.CriterioMatrizTarifa.Ciudad:
                        //Instanciando Parada de destino del movimiento
                        using (Parada parada = new Parada(this._id_parada_origen))
                        using (SAT_CL.Global.Ubicacion ubicacion = new Global.Ubicacion(parada.id_ubicacion))
                            descripcion_fila = ubicacion.id_ciudad.ToString();
                        operador_fila = "=";
                        break;
                    case TarifasPago.Tarifa.CriterioMatrizTarifa.Ubicacion:
                        //Instanciando Parada de destino del movimiento
                        using (Parada parada = new Parada(this._id_parada_origen))
                            descripcion_fila = parada.id_ubicacion.ToString();
                        operador_fila = "=";
                        break;
                    case TarifasPago.Tarifa.CriterioMatrizTarifa.Distancia:
                        descripcion_fila = this._kms.ToString();
                        operador_fila = "<=";

                        //Si no hay kilometros
                        if (this._kms <= 0)
                            resultado = new RetornoOperacion("El kilometraje calculado es igual a 0, no se puede aplicar tarifa por este criterio.");
                        break;
                    case TarifasPago.Tarifa.CriterioMatrizTarifa.Paradas:
                        descripcion_fila = "1";
                        operador_fila = "<=";
                        break;
                    case TarifasPago.Tarifa.CriterioMatrizTarifa.Volumen:
                        //Obteniendo volumen total
                        decimal volumen = Documentacion.ServicioProducto.ObtieneTotalVolumenParada(this._id_parada_origen, id_base_tarifa);

                        //Si el volumen es mayor a 0
                        if (volumen > 0)
                            descripcion_fila = volumen.ToString();
                        else
                            resultado = new RetornoOperacion("El volúmen transportado es igual a 0, no se puede aplicar tarifa por este criterio.");

                        operador_fila = "<=";
                        break;
                    case TarifasPago.Tarifa.CriterioMatrizTarifa.Peso:
                        //Obteniendo el peso total
                        decimal peso = Documentacion.ServicioProducto.ObtieneTotalPesoParada(this._id_parada_origen, id_base_tarifa);

                        //Si el peso es mayor a 0
                        if (peso > 0)
                            descripcion_fila = peso.ToString();
                        else
                            resultado = new RetornoOperacion("El peso transportado es igual a 0, no se puede aplicar tarifa por este criterio.");

                        operador_fila = "<=";
                        break;
                    case TarifasPago.Tarifa.CriterioMatrizTarifa.Cantidad:
                        //Obteniendo el conteo total de productos
                        decimal cantidad = Documentacion.ServicioProducto.ObtieneTotalCantidadParada(this._id_parada_origen);

                        //Si la cantidad es mayor a 0
                        if (cantidad > 0)
                            descripcion_fila = cantidad.ToString();
                        else
                            resultado = new RetornoOperacion("El conteo transportado es igual a 0, no se puede aplicar tarifa por este criterio.");

                        operador_fila = "<="; break;
                    default:
                        descripcion_fila = "0";
                        operador_fila = "=";
                        break;
                }
            }

            //Devolviendo resultado
            return resultado;
        }
             
        /// <summary>
        /// Obtiene de acuerdo a la unidad de medida predeterminada de la base de tarifa de pago, la cantidad de elementos a considerar para la aplicación del pago del movimiento
        /// </summary>
        /// <param name="id_base_tarifa">Base usada en la tarifa </param>
        /// <param name="id_tipo_pago_base_tarifa">Id de Tipo de Pago predeterminado para registrar el cálculo principal de la tarifa</param>
        public decimal ExtraeCriterioBaseTarifaPago(int id_base_tarifa, out int id_tipo_pago_base_tarifa)
        {
            //Declarando objeto de retorno
            decimal cantidad = 0;

            //Usando la base de tarifa aplicable
            using (TarifasPago.BaseTarifa base_tarifa = new TarifasPago.BaseTarifa(id_base_tarifa))
            {
                //Obteniendo el rubro del servicio a cuantificar
                switch (base_tarifa.base_tarifa)
                {
                    case TarifasPago.BaseTarifa.Base.Distancia:
                        //Obteniendo total de kilometros
                        cantidad = this._kms;
                        break;
                    case TarifasPago.BaseTarifa.Base.Fijo:
                        //Cantidad Fija a 1
                        cantidad = 1;
                        break;
                    case TarifasPago.BaseTarifa.Base.Peso:
                        cantidad = Documentacion.ServicioProducto.ObtieneTotalPesoParada(this._id_parada_origen, id_base_tarifa);
                        break;
                    case TarifasPago.BaseTarifa.Base.Volumen:
                        cantidad = Documentacion.ServicioProducto.ObtieneTotalVolumenParada(this._id_parada_origen, id_base_tarifa);
                        break;
                    case TarifasPago.BaseTarifa.Base.Tiempo:
                        break;
                }

                //Buscando el tipo de cargo predeterminado para la base y la compañía solictante
                using (Liquidacion.TipoPago tp = Liquidacion.TipoPago.ObtieneTipoPagoBaseTarifa(this._id_compania_emisor, id_base_tarifa))
                    id_tipo_pago_base_tarifa = tp.id_tipo_pago;
            }

            //Devolviendo resultado
            return cantidad;
        }

        /// <summary>
        /// Carga los movimientos que no cuentan con kilometraje definido
        /// </summary>
        /// <param name="id_compania">Id de Compañía</param>
        /// <param name="id_ubicacion_origen">Id de Ubicación de Origen</param>
        /// <param name="id_ubicacion_destino">Id de Ubicación de Destino</param>
        /// <returns></returns>
        public static DataTable CargaMovimientosSinKilometraje(int id_compania, int id_ubicacion_origen, int id_ubicacion_destino)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Inicializando arreglo de parámetros
            object[] param = { 26, 0, 0, 0, 0, 0, 0, 0, 0, id_compania, 0, 0, 0, false, null, id_ubicacion_origen, id_ubicacion_destino};


            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }

        #endregion

        #region Métodos Interfaz Terceros

        /// <summary>
        /// Recupera información de interés del movimiento vacío y la envía en un archivo a un servidor FTP especificado
        /// </summary>
        /// <param name="codificacion">Codificacíon del archivo</param>
        /// <returns></returns>
        public RetornoOperacion EnviaInformacionMovimientoVacioFTP(System.Text.Encoding codificacion)
        {
            //Declarando objeto de resultado sin errores
            RetornoOperacion resultado = new RetornoOperacion(this._id_movimiento);

            //Validando que el Movimiento provenga de un Servicio
            if (this._id_servicio > 0)
            {
                //Obteniendo Proveedores FTP
                using (DataTable dtProveedoresFTP = SAT_CL.Monitoreo.ProveedorWS.ObtieneProveedoresFTP(this._id_compania_emisor))
                {
                    //Validando Proveedores
                    if (Validacion.ValidaOrigenDatos(dtProveedoresFTP))
                    {
                        //Recorriendo Datos
                        foreach (DataRow dr in dtProveedoresFTP.Rows)
                        {
                            //Validando la Compania
                            if (this._id_compania_emisor == 76)
                            {
                                switch (dr["Identificador"].ToString())
                                {
                                    case "TEM - Omnitracs FTP":
                                    case "TEM - Unicomm FTP":
                                        {
                                            //Realizando la recuperación de información
                                            using (DataTable mit = Despacho.Reporte.CargaInformacionMovimientoVacioFTP(this._id_movimiento, Convert.ToInt32(dr["Id"])))
                                            {
                                                //Si hay elementos que utilizar
                                                if (mit != null)
                                                {
                                                    //Instanciando compañía a la que pertenece el movimiento
                                                    using (CompaniaEmisorReceptor comp = new CompaniaEmisorReceptor(this._id_compania_emisor))
                                                    {
                                                        //Definiendo nombre de archivo
                                                        string nombre_archivo = string.Format("{0}-MOV{1}_{2:ddMMyyyy_HHmmss}.csv", comp.nombre_corto, this._id_movimiento, Fecha.ObtieneFechaEstandarMexicoCentro());
                                                        byte[] bytesArchivo = null;

                                                        //Creando archivo en memoria
                                                        using (MemoryStream archivo = new MemoryStream())
                                                        {
                                                            //Creando escritos de texto en flujo
                                                            StreamWriter escritor = new StreamWriter(archivo, codificacion);

                                                            //Añadiendo encabezado
                                                            escritor.Write("OPERACION|FECHA VIAJE|DT|PI|ORIGEN|CITA LLEGADA ORIGEN|PI DESTINO|CITA LLEGADA|DESTINO|SHIPPER|RUTA MAESTRA|EJE TRANSITO|NIVEL SEGURIDAD|SCAC|OPERADOR|VEHICULO|PLATPORTABLE1|PORTABLE1|TIPO REMOLQUE|REMOLQUE1|PLATPORTABLE2|PORTABLE2|REMOLQUE2|PLATPORTABLE3|PORTABLE3|SHIPPER2|DESCRIPCION|EVENTO LOG1|EVENTO LOG2");

                                                            //Dando el formato solicitado para el contenido del archivo
                                                            foreach (DataRow r in mit.Rows)
                                                            {
                                                                //Nueva linea de texto
                                                                string linea = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}|{14}|{15}|{16}|{17}|{18}|{19}|{20}|{21}|{22}|{23}|{24}|{25}|{26}",
                                                                                            Cadena.TruncaCadena(r["Operacion"].ToString(), 3, ""), r.Field<DateTime>("FechaViaje").ToString("dd/MM/yyyy HH:mm"), Cadena.TruncaCadena(r["DT"].ToString(), 15, ""),
                                                                                            Cadena.TruncaCadena(r["PiOrigen"].ToString(), 31, ""), "", Cadena.TruncaCadena(r["PiDestino"].ToString(), 31, ""),
                                                                                            "", Cadena.TruncaCadena(r["Shipper"].ToString(), 30, ""), Cadena.TruncaCadena(r["RutaMaestra"].ToString(), 30, ""),
                                                                                            Cadena.TruncaCadena(r["EjeTransito"].ToString(), 30, ""), Cadena.TruncaCadena(r["NivelSeguridad"].ToString(), 8, ""), Cadena.TruncaCadena(r["SCAC"].ToString(), 4, ""),
                                                                                            Cadena.TruncaCadena(r["Operador"].ToString(), 40, ""), Cadena.TruncaCadena(r["Vehiculo"].ToString(), 30, ""), Cadena.TruncaCadena(r["PlatPortable1"].ToString(), 10, ""),
                                                                                            Cadena.TruncaCadena(r["Portable1"].ToString(), 15, ""), Cadena.TruncaCadena(r["TipoRemolque"].ToString(), 30, ""), Cadena.TruncaCadena(r["Remolque1"].ToString(), 30, ""),
                                                                                            Cadena.TruncaCadena(r["PlatPortable2"].ToString(), 10, ""), Cadena.TruncaCadena(r["Portable2"].ToString(), 15, ""), Cadena.TruncaCadena(r["Remolque2"].ToString(), 30, ""),
                                                                                            Cadena.TruncaCadena(r["PlatPortable3"].ToString(), 10, ""), Cadena.TruncaCadena(r["Portable3"].ToString(), 15, ""), Cadena.TruncaCadena(r["Shipper2"].ToString(), 30, ""),
                                                                                            Cadena.TruncaCadena(r["Descripcion"].ToString(), 30, ""), Cadena.TruncaCadena(r["EventoLog1"].ToString(), 15, ""), Cadena.TruncaCadena(r["EventoLog2"].ToString(), 15, ""));

                                                                //Añadiendo linea creada
                                                                if (!string.IsNullOrEmpty(linea))
                                                                    escritor.Write(Environment.NewLine + linea);
                                                            }

                                                            //Confirmando cambios en flujo y liberando recursos de escritor
                                                            escritor.Flush();

                                                            //Obteniendo arreglo de bytes del flujo de archivo
                                                            bytesArchivo = TSDK.Base.Flujo.ConvierteFlujoABytes(archivo);
                                                        }

                                                        //Recuperando datos de autenticación en servidor FTP
                                                        string servidorFTP = dr["Endpoint"].ToString();
                                                        string usuarioFTP = dr["Usuario"].ToString();
                                                        string contrasenaFTP = dr["Contraseña"].ToString();

                                                        try
                                                        {
                                                            //Creando peticioón FTP
                                                            FtpWebRequest peticionFTP = FTP.CreaPeticionFTP(string.Format("{0}/{1}", servidorFTP, nombre_archivo), WebRequestMethods.Ftp.UploadFile, usuarioFTP, contrasenaFTP);
                                                            //Dimensionando archivo por transferir en la petición
                                                            peticionFTP.ContentLength = bytesArchivo.Length;

                                                            //Recuperando flujo de petición ftp
                                                            Stream flujoPeticionFTP = peticionFTP.GetRequestStream();
                                                            //Añadiendo bytes del archivo creado a flujo de petición (escribiendo)
                                                            flujoPeticionFTP.Write(bytesArchivo, 0, bytesArchivo.Length);
                                                            //Cerrando flujo de escritura de archivo
                                                            flujoPeticionFTP.Close();
                                                        }
                                                        //Si no hubo petición devuelta
                                                        catch (NullReferenceException)
                                                        {
                                                            //En caso de error
                                                            resultado = new RetornoOperacion(string.Format("Error al crear petición al servidor FTP '{0}'.", servidorFTP));

                                                            using (EventLog eventLog = new EventLog("Application"))
                                                            {
                                                                eventLog.Source = "Application";
                                                                eventLog.WriteEntry(resultado.Mensaje, EventLogEntryType.Information, 101, 1);
                                                            }
                                                            
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            //En caso de error
                                                            resultado = new RetornoOperacion(ex.Message);

                                                            using (EventLog eventLog = new EventLog("Application"))
                                                            {
                                                                eventLog.Source = "Application";
                                                                eventLog.WriteEntry(resultado.Mensaje, EventLogEntryType.Information, 101, 1);
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                    resultado = new RetornoOperacion("No pudo ser recuperada la información del movimiento vacío para su envío.");
                                            }
                                            break;
                                        }
                                }
                            }
                        }
                    }
                }
            }

            //Devolviendo resultado
            return resultado;
        }

        /// <summary>
        /// Realiza actualizaciones en plataformas de terceros sin afectar el flujo operativo de la aplicación
        /// </summary>
        /// <param name="id_movimiento">Id de Movimiento de interés</param>
        /// <param name="id_usuario">Id de Usuario</param>
        public static RetornoOperacion ActualizaPlataformaTerceros(int id_movimiento, int id_usuario)
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Instanciando movimiento involucrado
            using (Movimiento mov = new Movimiento(id_movimiento))
            {
                //Si el movimiento pertenece a un servicio
                if (mov.id_servicio > 0)
                {
                    //Instanciando servicio del movimiento
                    using (Documentacion.Servicio srv = new Documentacion.Servicio(mov.id_servicio))
                    {
                        //Realizando actualización 
                        resultado = srv.ActualizaPlataformaTerceros(id_usuario);
                    }
                }
            }

            //Devolviendo resultado
            return resultado;
        }

        #endregion
    }
}
