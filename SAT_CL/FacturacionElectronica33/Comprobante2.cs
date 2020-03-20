using SAT_CL.Global;
using System;
using System.Collections.Generic;
using System.Data;
using System.Transactions;
using System.Xml;
using System.Xml.Linq;
using TSDK.Base;
using TSDK.Base.CertificadoDigital;
using TSDK.Datos;
using System.Linq;
using FEv32 = SAT_CL.FacturacionElectronica;

namespace SAT_CL.FacturacionElectronica33
{
    public partial class Comprobante
    {
        #region Constantes CFDI 3.3

        /// <summary>
        /// Versión de CFDI que utilizará esta clase
        /// </summary>
        public const string VERSION_CFDI = "3.3";
        /// <summary>
        /// Versión para el complemento de Nómina
        /// </summary>
        public const string VERSION_COMPLEMENTO_NOMINA12 = "1.2";
        /// <summary>
        /// Versión para el complemento de Recepción de Pagos
        /// </summary>
        public const string VERSION_COMPLEMENTO_PAGO10 = "1.0";

        #endregion

        #region CFDI V3.3 con Complemento de Pago 1.0
        /// <summary>
        /// Método encargado de Validar los Requisitos Previos al Timbrado
        /// </summary>
        /// <param name="serie"></param>
        /// <param name="folio"></param>
        /// <param name="id_certificado_activo">Identificador de Certificado Activo</param>
        /// <param name="numero_certificado">Número de Certificado</param>
        /// <param name="certificado_base_64">Certificado en Base64</param>
        /// <param name="contrasena_apertura">Contraseña de Apertura</param>
        /// <param name="bytes_certificado">Certificado en Bytes</param>
        /// <param name="tipoFolioSerie">Tipo de Serie y/ó Folio por Calcular (FE, Nómina)</param>
        /// <returns></returns>
        private RetornoOperacion validaRequisitosPreviosTimbradoPagos(string serie, out string folio, out int id_certificado_activo,
                                        out string numero_certificado, out string certificado_base_64, out string contrasena_apertura,
                                        out byte[] bytes_certificado, SerieFolioCFDI.TipoSerieFolioCFDI tipoFolioSerie)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion(1);

            //Inicializando valores de Retorno
            id_certificado_activo = 0;
            folio = numero_certificado = certificado_base_64 = contrasena_apertura = "";
            bytes_certificado = null;

            //Inicializando Bloque Transaccional
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Validando que el Comprobante no este Timbrado
                if (!this._bit_generado)
                {
                    //Si la Moneda es Distinta de XXX
                    if (this._id_moneda != 173)
                    {
                        //Instanciando Excepción
                        result = new RetornoOperacion("El Comprobante debe de tener Moneda 'XXX' (Los códigos asignados para las transacciones en que intervenga ninguna moneda)");
                    }
                }
                else
                    //Instanciando Excepción
                    result = new RetornoOperacion("El Comprobante se encuentra Timbrado");

                //Validando Validación Exitosa
                if (result.OperacionExitosa)
                {
                    //Validando que el Emisor este Vigente
                    using (Global.CompaniaEmisorReceptor emisor = new Global.CompaniaEmisorReceptor(this._id_compania_emisor))
                    {
                        //Validando Existencia
                        if (emisor.habilitar)
                        {
                            //Validando sucursal
                            if (this._id_sucursal > 0)
                            {
                                //Instanciando sucursal
                                using (Global.Sucursal s = new Global.Sucursal(this._id_sucursal))
                                {
                                    //Validando que no exista la Sucursal
                                    if (!s.habilitar)
                                        //Instanciando Excepción
                                        result = new RetornoOperacion("La sucursal no se encuentra activa.");
                                }
                            }
                        }
                        else
                            //Instanciando Excepción
                            result = new RetornoOperacion("El Emisor no se encuentra Activo.");

                        //Validando Validación Exitosa
                        if (result.OperacionExitosa)
                        {
                            //Obteniendo el certificado activo para el emisor y/o sucursal
                            using (CertificadoDigital certificado = CertificadoDigital.RecuperaCertificadoEmisorSucursal(this._id_compania_emisor, this._id_sucursal, CertificadoDigital.TipoCertificado.CSD))
                            {
                                //Validando Existencia del Certificado
                                if (certificado.Habilitar)
                                {
                                    //Obteniendo CER
                                    using (Certificado cer = new Certificado(certificado.ruta_llave_publica))
                                    {
                                        //Comprobando Carga del Certificado
                                        if (cer.Subject != null)
                                        {
                                            //Validando RFC del Emisor contra el del Certificado
                                            if (cer.Subject.RFCPropietario == emisor.rfc)
                                            {
                                                //Asignando parámteros de salida
                                                id_certificado_activo = certificado.id_certificado_digital;
                                                numero_certificado = cer.No_Serie;
                                                certificado_base_64 = cer.CertificadoBase64;
                                                contrasena_apertura = certificado.contrasena_desencriptada;
                                                bytes_certificado = System.IO.File.ReadAllBytes(certificado.ruta_llave_privada);
                                            }
                                        }
                                        else
                                            //Instanciando Excepción
                                            result = new RetornoOperacion("Imposible recuperar los datos de Propietario del Certificado.");
                                    }
                                }
                                else
                                    //Instanciando Excepción
                                    result = new RetornoOperacion("No existe un Certificado de Sello Digital activo.");
                            }
                        }

                        //Si no hay erroes
                        if (result.OperacionExitosa)
                        {
                            //Obteniendo Conceptos
                            using (DataTable dtConceptos = Concepto.ObtieneConceptosComprobante(this._id_comprobante33))
                            {
                                //Validando que no existan Conceptos
                                if (!Validacion.ValidaOrigenDatos(dtConceptos))
                                    //Instanciando Excepción
                                    result = new RetornoOperacion("No existen Conceptos en el Comprobante");
                            }
                        }

                        //Si no existe error
                        if (result.OperacionExitosa)
                        {
                            //Instanciamos emisor y serie para obtener el tipo definido
                            using (SerieFolioCFDI objSerieFolio = new SerieFolioCFDI(serie, version, this._id_compania_emisor))
                            {
                                ///Validamos el Tipo de Serie
                                if (objSerieFolio.tipo_folio_serie == tipoFolioSerie)
                                {
                                    //Realizando búsqueda de folio por asignar
                                    folio = obtieneFolioPorAsignar(this._id_compania_emisor, version, serie);
                                    //Si no existe un folio disponible
                                    if (Convert.ToInt32(folio) <= 0)
                                        //Instanciando Excepción
                                        result = new RetornoOperacion(string.Format("No existe un folio disponible para la serie {0}.", serie.ToUpper()));
                                }
                                else
                                    //Instanciando Excepción
                                    result = new RetornoOperacion(string.Format("La serie no se encuentra disponible {0}.", serie));
                            }
                        }
                    }
                }

                //Validamos Resultado
                if (result.OperacionExitosa)
                {
                    //Finalizamos transacción
                    scope.Complete();
                }
            }

            //Devolviendo Resultado Obtenido
            return result;
        }

        /// <summary>
        /// Realiza la importación del CFDI de Comprobante de Recepción de Pagos
        /// </summary>
        /// <param name="id_cliente">Id de Cliente al que pertenecen los Pagos (Receptor CFDI)</param>
        /// <param name="comprobante">Información de importación de encabezado de FI</param>
        /// <param name="conceptos">Información del Único Concepto requerido</param>
        /// <param name="lista_fi">Conjunto de FI (pagos realizados por el CLiente)</param>
        /// <param name="id_compania_uso">Id de Emisor del CFDI</param>
        /// <param name="id_sucursal">Id de Sucursal de Emisión del CFDI</param>
        /// <param name="id_uso_cfdi_receptor">Id de Uso del CFDI por parte del Receptor del mismo</param>
        /// <param name="id_usuario">Id de Usuario que realiza la operación</param>
        /// <returns></returns>
        public static RetornoOperacion ImportaComprobante_V3_3_ReciboPago_V1_0(int id_cliente, DataTable comprobante, DataTable conceptos, List<KeyValuePair<int, int>> lista_fi, int id_compania_uso, int id_sucursal, int id_uso_cfdi_receptor, int id_usuario)
        {
            //Declarando objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion();

            //Validando origen de datos
            if (Validacion.ValidaOrigenDatos(comprobante, conceptos))
            {
                //Recuperando primer registro de cada tabla
                DataRow infoComp = comprobante.Rows[0];
                DataRow infoConc = conceptos.Rows[0];

                //Inicializando transacción
                using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    //Instanciando emisor y receptor de CFDI
                    using (CompaniaEmisorReceptor em = new CompaniaEmisorReceptor(id_compania_uso), re = new CompaniaEmisorReceptor(id_cliente))
                    {
                        //Si ambos existen
                        if (em.id_compania_emisor_receptor > 0 && re.id_compania_emisor_receptor > 0)
                        {
                            //Instanciando sucursal
                            using (Sucursal suc = new Sucursal(id_sucursal))
                            {
                                //Instanciando domicilios de emisor, receptor y sucursal de expedición (en caso de aplicar)
                                using (Direccion dem = new Direccion(em.id_direccion), dre = new Direccion(re.id_direccion), dsuc = new Direccion(suc.id_direccion))
                                {
                                    //Si los domicilios de emisor y receptor existen (sucursal es opcional)
                                    if (dem.id_direccion > 0 && dre.id_direccion > 0)
                                    {
                                        //Obteniendo Regimen Fiscal del Emisor
                                        string reg_fis = Catalogo.RegresaDescripcioValorCadena(3197, em.id_regimen_fiscal);
                                        
                                        //Realizando la inserción del Comprobante (CFDI de Pago)
                                        resultado = Comprobante.InsertaComprobante(Convert.ToInt32(infoComp["Id_Tipo_Comprobante"]), (byte)OrigenDatos.ReciboPagoCliente, Comprobante.VERSION_CFDI, Convert.ToByte(infoComp["Id_Forma_Pago"]),
                                                                    Convert.ToByte(infoComp["Id_Metodo_Pago"]), infoComp["Condiciones_Pago"].ToString(), Convert.ToInt32(infoComp["Id_Moneda"]), 0.00M, dem.codigo_postal, dem.id_direccion, 
                                                                    em.id_compania_emisor_receptor, reg_fis, suc.id_sucursal, re.id_compania_emisor_receptor, id_uso_cfdi_receptor, id_usuario);

                                        //Guardando Id de registro Comprobante de Pago
                                        int id_comprobante_pago = resultado.IdRegistro;

                                        //Si no hay inconvenientes
                                        if (resultado.OperacionExitosa)
                                        {
                                            //Realizando la inserción del único concepto
                                            resultado = Concepto.InsertaConcepto(id_comprobante_pago, Convert.ToInt32(infoConc["Id_concepto_Padre"]), Convert.ToDecimal(infoConc["Cantidad"]), Convert.ToInt32(infoConc["Id_unidad"]), Convert.ToInt32(infoConc["Id_tipo_cargo"]), Convert.ToInt32(infoConc["IdClaveSAT"]), infoConc["Descripcion"].ToString(), infoConc["No_identificacion"].ToString(), Convert.ToDecimal(infoConc["Valor_unitario"]), Convert.ToDecimal(infoConc["Importe_captura"]), Convert.ToDecimal(infoConc["Importe_moneda_nacional"]), infoConc["Cuenta_Predial"].ToString(), Convert.ToDecimal(infoConc["Descuento"]), id_usuario);

                                            //Si no hay problemas al registrar el concepto único
                                            if (resultado.OperacionExitosa)
                                                //Agregando pagos relacionados y sus detalles aplicados(documento relacionado)
                                                resultado = EgresoIngresoComprobante.AgregarIngresosACFDIPagos(lista_fi, id_usuario, id_comprobante_pago);
                                        }

                                        //Si no hay errores en el proceso
                                        if (resultado.OperacionExitosa)
                                        {
                                            //Generalizando resultado exitoso
                                            resultado = new RetornoOperacion(id_comprobante_pago);
                                            //Confirmando cambios realizados
                                            scope.Complete();
                                        }
                                    }
                                    else
                                        resultado = new RetornoOperacion("Error al recuperar información de domicilios de Emisor y/o Receptor del CFDI.");
                                }
                            }
                        }
                        else
                            resultado = new RetornoOperacion("Error al recuperar información de Emisor y/o Receptor del CFDI.");
                    }
                }
            }
            //Si no hay información suficiente de encabezado
            else
                resultado = new RetornoOperacion("Error al recuperar información para encabezado de CFDI.");          

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Realiza la importación del CFDI de Comprobante de Recepción de Pagos
        /// </summary>
        /// <param name="id_cliente">Id de Cliente al que pertenecen los Pagos (Receptor CFDI)</param>
        /// <param name="comprobante">Información de importación de encabezado de FI</param>
        /// <param name="conceptos">Información del Único Concepto requerido</param>
        /// <param name="id_compania_uso">Id de Emisor del CFDI</param>
        /// <param name="id_sucursal">Id de Sucursal de Emisión del CFDI</param>
        /// <param name="id_uso_cfdi_receptor">Id de Uso del CFDI por parte del Receptor del mismo</param>
        /// <param name="id_usuario">Id de Usuario que realiza la operación</param>
        /// <returns></returns>
        public static RetornoOperacion ImportaComprobante_V3_3_ReciboPago_V1_0(int id_cliente, DataTable comprobante, DataTable conceptos, int id_compania_uso, int id_sucursal, int id_uso_cfdi_receptor, int id_usuario)
        {
            //Declarando objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion();

            //Validando origen de datos
            if (Validacion.ValidaOrigenDatos(comprobante, conceptos))
            {
                //Recuperando primer registro de cada tabla
                DataRow infoComp = comprobante.Rows[0];
                DataRow infoConc = conceptos.Rows[0];

                //Inicializando transacción
                using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    //Instanciando emisor y receptor de CFDI
                    using (CompaniaEmisorReceptor em = new CompaniaEmisorReceptor(id_compania_uso), re = new CompaniaEmisorReceptor(id_cliente))
                    {
                        //Si ambos existen
                        if (em.id_compania_emisor_receptor > 0 && re.id_compania_emisor_receptor > 0)
                        {
                            //Instanciando sucursal
                            using (Sucursal suc = new Sucursal(id_sucursal))
                            {
                                //Instanciando domicilios de emisor, receptor y sucursal de expedición (en caso de aplicar)
                                using (Direccion dem = new Direccion(em.id_direccion), dre = new Direccion(re.id_direccion), dsuc = new Direccion(suc.id_direccion))
                                {
                                    //Si los domicilios de emisor y receptor existen (sucursal es opcional)
                                    if (dem.id_direccion > 0 && dre.id_direccion > 0)
                                    {
                                        //Obteniendo Regimen Fiscal del Receptor
                                        string reg_fis = Catalogo.RegresaDescripcioValorCadena(3197, re.id_regimen_fiscal);

                                        //Realizando la inserción del Comprobante (CFDI de Pago)
                                        resultado = Comprobante.InsertaComprobante(Convert.ToInt32(infoComp["Id_Tipo_Comprobante"]), (byte)OrigenDatos.ReciboPagoCliente, Comprobante.VERSION_CFDI, Convert.ToByte(infoComp["Id_Forma_Pago"]),
                                            Convert.ToByte(infoComp["Id_Metodo_Pago"]), infoComp["Condiciones_Pago"].ToString(), Convert.ToInt32(infoComp["Id_Moneda"]), 0.00M, dem.codigo_postal, dem.id_direccion, em.id_compania_emisor_receptor, reg_fis, suc.id_sucursal, re.id_compania_emisor_receptor, id_uso_cfdi_receptor, id_usuario);

                                        //Guardando Id de registro Comprobante de Pago
                                        int id_comprobante_pago = resultado.IdRegistro;

                                        //Si no hay inconvenientes
                                        if (resultado.OperacionExitosa)
                                        {
                                            //Realizando la inserción del único concepto
                                            resultado = Concepto.InsertaConcepto(id_comprobante_pago, Convert.ToInt32(infoConc["Id_concepto_Padre"]), Convert.ToDecimal(infoConc["Cantidad"]), Convert.ToInt32(infoConc["Id_unidad"]), Convert.ToInt32(infoConc["Id_tipo_cargo"]), Convert.ToInt32(infoConc["IdClaveSAT"]), infoConc["Descripcion"].ToString(), infoConc["No_identificacion"].ToString(), Convert.ToDecimal(infoConc["Valor_unitario"]), Convert.ToDecimal(infoConc["Importe_captura"]), Convert.ToDecimal(infoConc["Importe_moneda_nacional"]), infoConc["Cuenta_Predial"].ToString(), Convert.ToDecimal(infoConc["Descuento"]), id_usuario);
                                        }

                                        //Si no hay errores en el proceso
                                        if (resultado.OperacionExitosa)
                                        {
                                            //Generalizando resultado exitoso
                                            resultado = new RetornoOperacion(id_comprobante_pago);
                                            //Confirmando cambios realizados
                                            scope.Complete();
                                        }
                                    }
                                    else
                                        resultado = new RetornoOperacion("Error al recuperar información de domicilios de Emisor y/o Receptor del CFDI.");
                                }
                            }
                        }
                        else
                            resultado = new RetornoOperacion("Error al recuperar información de Emisor y/o Receptor del CFDI.");
                    }
                }
            }
            //Si no hay información suficiente de encabezado
            else
                resultado = new RetornoOperacion("Error al recuperar información para encabezado de CFDI.");

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Timbra un comprobante de recepción de pagos con la versión de CFDI activa para esta clase y la versión del complemento
        /// </summary>
        /// <param name="serie">Número de Serie a utilizar</param>
        /// <param name="id_usuario">Id de Usuario que realiza la operación</param>
        /// <param name="ruta_xslt_co33">Ruta del archivo de transformación para Cadena Original CFDI 3.3 (Con referencias en línea)</param>
        /// <param name="ruta_xslt_co_local33">Ruta del archivo de transformación para Cadena Original CFDI 3.3 (Con referencias locales, en caso de no disponibilidad de archivos del SAT)</param>
        /// <returns></returns>
        public RetornoOperacion TimbraComprobanteRecepcionPagoV1_0(string serie, int id_usuario, string ruta_xslt_co33, string ruta_xslt_co_local33)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion retorno = new RetornoOperacion(this._id_comprobante33);
            bool cancelacion = true;

            //Inicializando Bloque Transaccional
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Validando estatus actual de timbrado
                if (!this._bit_generado)
                {
                    //Determinando si este CFDI reemplaza a alguno previo
                    using (Comprobante cfdiSustituido = ComprobanteRelacion.ObtenerCFDISustituido(this._id_comprobante33))
                    {
                        //Si hay un elemento
                        if (cfdiSustituido.habilitar)
                        {
                            //Validando Comprobante de Pago
                            if (!ComprobantePagoDocumentoRelacionado.ValidaComprobantePagoCxC(this._id_comprobante33))
                            {
                                //Realizando operación de cancelación de Timbre ante el SAT
                                CancelacionCDFI.EstatusUUID estatusUUID = CancelacionCDFI.EstatusUUID.SinEstatus;
                                CancelacionCDFI.TipoCancelacion tipo = CancelacionCDFI.TipoCancelacion.SinAsignar;
                                XDocument consulta = new XDocument();
                                retorno = CancelacionCDFI.objCancelacion.CancelacionComprobanteCxC(cfdiSustituido.id_comprobante33, out estatusUUID, out tipo, out consulta, id_usuario);

                                //Si se canceló correctamente
                                if (retorno.OperacionExitosa)
                                {
                                    //Validando Excepciones
                                    if (retorno.Mensaje.Contains("Rechazado por el Cliente.") || retorno.Mensaje.Contains("sigue en Proceso de Cancelación"))
                                    {
                                        //Instanciando Excepción
                                        cancelacion = false;
                                        retorno = new RetornoOperacion(retorno.Mensaje, cancelacion);
                                    }

                                    //Validando Operación
                                    if (retorno.OperacionExitosa)
                                    {
                                        //Recuperando relaciones de pagos del CFDI previo para su actualización a cancelado
                                        using (DataTable mitPagosCFDI = EgresoIngresoComprobante.ObtienePagosCFDIRecepcionPagos(cfdiSustituido.id_comprobante33))
                                        {
                                            //Si hay elementos por actualizar
                                            if (mitPagosCFDI != null)
                                            {
                                                //Para cada elemento
                                                foreach (DataRow p in mitPagosCFDI.Rows)
                                                {
                                                    //Instanciando Relación
                                                    using (EgresoIngresoComprobante eic = new EgresoIngresoComprobante(p.Field<int>("IdEgresoIngresoComp")))
                                                    {
                                                        //Si se localizó la relación
                                                        if (eic.habilitar)
                                                            //Actualizando estatus de relación
                                                            retorno = eic.ActualizaEstatusEgresoIngresoComprobante(EgresoIngresoComprobante.Estatus.Cancelado, id_usuario);
                                                        else
                                                            retorno = new RetornoOperacion(string.Format("Error al Recuperar Relación Pagos - CFDI (Para sustitución) ID: '{0}'", p.Field<int>("IdEgresoIngresoComp")));

                                                        //Si hay errores se interrumpe ciclo
                                                        if (!retorno.OperacionExitosa)
                                                            break;
                                                    }
                                                }
                                            }
                                            else
                                                retorno = new RetornoOperacion("Error al Recuperar Pagos de CFDI Sustituido para su Actualización a Cancelado.");
                                        }
                                    }
                                    //Si la cancelación sigue en proceso o fue rechazada por el cliente
                                    else if (!cancelacion)
                                    {
                                        //Deshabilitando registro CFDI de Pago
                                        retorno = DeshabilitaComprobanteSinFacturadoFacturacion(id_usuario);
                                        //Si no hay errores al deshabilitar comprobante
                                        if (retorno.OperacionExitosa)
                                        {
                                            //Recuperando conjunto de relaciones CFDI de Pago y Egreso/Ingreso activas
                                            using (DataTable mitEIC = EgresoIngresoComprobante.ObtienePagosCFDIRecepcionPagos(this._id_comprobante33))
                                            {
                                                //Si hay elementos
                                                if (mitEIC != null)
                                                {
                                                    //Para cada relación
                                                    foreach (DataRow r in mitEIC.Rows)
                                                    {
                                                        //Deshabilitando registro relación
                                                        retorno = EgresoIngresoComprobante.DeshabilitaEgresoIngresoComprobante(r.Field<int>("IdEgresoIngresoComp"), id_usuario);

                                                        //SI hay errores
                                                        if (!retorno.OperacionExitosa)
                                                            //Terminando iteraciones
                                                            break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                                retorno = new RetornoOperacion("El comprobante esta relacionado a un pago, imposible su cancelación.");
                        }
                    }

                    //Si no hay errores hasta cancelación previa
                    if (retorno.OperacionExitosa && cancelacion)
                    {
                        //Declarando variables auxiliares
                        int id_certificado_activo = 0;
                        string folio = "", numero_certificado = "", certificado_base_64 = "", contrasena_certificado = "";
                        byte[] bytes_certificado = null;
                        serie = serie.ToUpper();

                        //Validando Requisitos Previos
                        retorno = validaRequisitosPreviosTimbradoPagos(serie, out folio, out id_certificado_activo, out numero_certificado,
                                                          out certificado_base_64, out contrasena_certificado, out bytes_certificado,
                                                          SerieFolioCFDI.TipoSerieFolioCFDI.ReciboPagos);

                        //Validando Operación Exitosa
                        if (retorno.OperacionExitosa)
                        {
                            //Asignando Folio al Comprobante
                            retorno = asignaFolioComprobante(this._version, serie, folio, id_certificado_activo, id_usuario);

                            //Validando Operación Exitosa
                            if (retorno.OperacionExitosa)
                            {
                                //Actualizando Atributos
                                if (this.cargaAtributosInstancia(this._id_comprobante33))
                                {
                                    //Instanciando al emisor
                                    using (CompaniaEmisorReceptor em = new CompaniaEmisorReceptor(this._id_compania_emisor))
                                    {
                                        //Validando Emisor
                                        if (em.habilitar)
                                        {
                                            //Creando ruta de guardado del comprobante
                                            string ruta_xml = string.Format(@"{0}{1}\{2}\CFDI_3_3\{3}{4}.xml", CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Directorio Raíz Gestor Archivos SAT", 0), 209.ToString("0000"), em.DirectorioAlmacenamiento, serie, folio);

                                            //Eliminando archivo si es que ya existe
                                            Archivo.EliminaArchivo(ruta_xml);

                                            /**** Declaración de namespaces a utilizar en el Comprobante ****/
                                            //SAT
                                            XNamespace ns = CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Namespace SAT");
                                            
                                            //W3C
                                            //XNamespace nsW3C = CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Namespace W3C");
                                            //Inicialziando el valor de schemaLocation del cfd
                                            //string schemaLocation = CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("SchemaLocation CFDI 3.3");

                                            //Creando Documento xml inicial y configurando la declaración de XML
                                            XDocument documento = new XDocument();
                                            documento.Document.Declaration = new XDeclaration("1.0", "utf-8", "");

                                            //Obteniendo Comprobante en Formato XML
                                            documento.Add(ComprobanteXML.CargaElementosArmadoComprobanteRecepcionPagos_V1_0(this._id_comprobante33, id_usuario, out retorno));

                                            //Validando que exista el Comprobante
                                            if (documento != null)
                                            {
                                                //Si no hubo errores
                                                if (retorno.OperacionExitosa)
                                                {
                                                    /**** Validando Acentos por Receptor ****/
                                                    //Instanciando al receptor
                                                    using (CompaniaEmisorReceptor rec = new CompaniaEmisorReceptor(this._id_compania_receptor))
                                                    {
                                                        //Validamos que exista referencia
                                                        if (rec.habilitar && rec.FacturacionElectronica33 != null)
                                                        {
                                                            //Validamos que exista la Clave
                                                            if (rec.FacturacionElectronica.ContainsKey("Acepta Acentos Comptrobante"))
                                                            {
                                                                //Si es necesario realizar supresión de acentos en comprobante
                                                                if (!Convert.ToBoolean(rec.FacturacionElectronica["Acepta Acentos Comptrobante"]))
                                                                    documento = XDocument.Parse(suprimeCaracteresAcentuadosCFD(documento.ToString()));
                                                            }
                                                        }
                                                    }

                                                    //Asignando Valores
                                                    documento.Root.SetAttributeValue("NoCertificado", numero_certificado);

                                                    //Definiendo bytes de XML
                                                    byte[] bytes_comprobante = Xml.ConvierteXDocumentABytes(documento);

                                                    //Verificando que se haya devuelto los bytes del XDocument
                                                    if (!Conversion.EsArrayVacio(bytes_comprobante))
                                                    {
                                                        //Definiendo variable para almacenar cadena orignal
                                                        string cadena_original = "";

                                                        //Realizando transformación de cadena original
                                                        retorno = FEv32.SelloDigitalSAT.CadenaCFD(new System.IO.MemoryStream(bytes_comprobante), ruta_xslt_co33, ruta_xslt_co_local33, out cadena_original);

                                                        //Validando Operación Exitosa
                                                        if (retorno.OperacionExitosa && !cadena_original.Equals(""))
                                                        {
                                                            //Codificando Cadena Original a UTF-8
                                                            byte[] co_utf_8 = FEv32.SelloDigitalSAT.CodificacionUTF8(cadena_original);

                                                            //Realizando sellado del Comprobante
                                                            string sello_digital = FEv32.SelloDigitalSAT.FirmaCadenaSHA256(co_utf_8, bytes_certificado, contrasena_certificado);

                                                            //Si el sello digital fue generado correctamente
                                                            if (sello_digital != "")
                                                            {
                                                                //Actualizando el sello digital en BD y Marcando como 'Generado'
                                                                retorno = actualizaSelloDigital(sello_digital, id_usuario);

                                                                //Si se actualiza correctamente
                                                                if (retorno.OperacionExitosa)
                                                                {
                                                                    //Actualizando datos de sello digital
                                                                    if (cargaAtributosInstancia(this._id_comprobante33))
                                                                    {
                                                                        //Actualizando Sello y número de certificado en XML
                                                                        documento.Root.SetAttributeValue("Certificado", certificado_base_64);
                                                                        documento.Root.SetAttributeValue("Sello", sello_digital);

                                                                        //Conectando a Web Service del Proveedor de Timbre Fiscal y generando dicho registro
                                                                        retorno = generaTimbreFiscalDigital(ref documento, ns, id_usuario);

                                                                        //Validando Operación Exitosa
                                                                        if (retorno.OperacionExitosa)
                                                                        {
                                                                            //Actualizando contenido de documento a arreglo de bytes
                                                                            bytes_comprobante = Xml.ConvierteXDocumentABytes(documento);

                                                                            //Guardando archivo en unidad de almacenamiento
                                                                            retorno = ArchivoRegistro.InsertaArchivoRegistro(209, this._id_comprobante33, 22, "", id_usuario, bytes_comprobante, ruta_xml);

                                                                            //Validmos Resultado
                                                                            if (retorno.OperacionExitosa)
                                                                                //Generamos Barra Bidimensional
                                                                                retorno = this.generaCodigoBidimensionalComprobante(id_usuario);
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            else
                                                                //Instanciando Excepción
                                                                retorno = new RetornoOperacion("Error, sello digital en blanco.");
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    //Validando Operaciones Exitosas
                    if (retorno.OperacionExitosa)
                    {
                        //Realizando la actualización de estatus de la relación de Pagos y el CFDI de pagos
                        using (DataTable mitPagosCFDI = EgresoIngresoComprobante.ObtienePagosCFDIRecepcionPagos(this._id_comprobante33))
                        {
                            //Si hay elementos por actualizar
                            if (mitPagosCFDI != null)
                            {
                                //Para cada elemento
                                foreach (DataRow p in mitPagosCFDI.Rows)
                                {
                                    //Instanciando Relación
                                    using (EgresoIngresoComprobante eic = new EgresoIngresoComprobante(p.Field<int>("IdEgresoIngresoComp")))
                                    {
                                        //Si se localizó la relación
                                        if (eic.habilitar)
                                            //Actualizando estatus de relación
                                            retorno = eic.ActualizaEstatusEgresoIngresoComprobante(EgresoIngresoComprobante.Estatus.Timbrado, id_usuario);
                                        else
                                            retorno = new RetornoOperacion(string.Format("Error al Recuperar Relación Pagos - CFDI ID: '{0}'", p.Field<int>("IdEgresoIngresoComp")));

                                        //Si hay errores se interrumpe ciclo
                                        if (!retorno.OperacionExitosa)
                                            break;
                                    }
                                }
                            }
                            else
                                retorno = new RetornoOperacion("Error al Recuperar Pagos para su Actualización a Timbrado.");
                        }

                        //Si no hay errores de actualización de relaciones
                        if (retorno.OperacionExitosa)
                        {
                            //Asignando Comprobante
                            retorno = new RetornoOperacion(this._id_comprobante33);
                            //Completando Transacción
                            scope.Complete();
                        }
                    }
                }
                else
                    retorno = new RetornoOperacion("El CFDI ya se ha Timbrado con anterioridad.");
            }

            //Devolviendo Resultado Obtenido
            return retorno;
        }
        /// <summary>
        /// Método encargado de Deshabilitar los Comprobantes en su versión 3.3
        /// </summary>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaComprobanteSinFacturadoFacturacion(int id_usuario)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion result = new RetornoOperacion(this._id_comprobante33);

            //Si el comprobante no se ha timbrado
            if (!this._bit_generado)
            {
                //Creamos la transacción 
                using (System.Transactions.TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    //Validamos el Origen de Datos
                    if ((OrigenDatos)this._id_origen_datos == OrigenDatos.ReciboPagoCliente)
                    {
                        //Obtiene Conceptos Comprobante
                        using (DataTable dtConceptos = Concepto.ObtieneConceptosComprobante(this._id_comprobante33))
                        {
                            //Validando Conceptos
                            if (Validacion.ValidaOrigenDatos(dtConceptos))
                            {
                                //Recorriendo Conceptos
                                foreach (DataRow dr in dtConceptos.Rows)
                                {
                                    //Instanciando Concepto
                                    using (Concepto con = new Concepto(Convert.ToInt32(dr["Id"])))
                                    {
                                        //Validando Concepto
                                        if (con.habilitar)
                                        {
                                            //Deshabilitando Concepto
                                            result = con.DeshabilitaConcepto(id_usuario);

                                            //Validando Resultado
                                            if (!result.OperacionExitosa)

                                                //Terminando Ciclo
                                                break;
                                        }
                                        else
                                        {
                                            //Instanciando Excepción
                                            result = new RetornoOperacion("No se puede recuperar el Concepto");
                                            //Terminando Ciclo
                                            break;
                                        }
                                    }
                                }
                            }

                            //Validando Operaciones
                            if (result.OperacionExitosa)
                            {
                                //Devolviendo Resultado Obtenido
                                result = this.actualizaAtributosBD((EstatusVigencia)this._id_estatus_vigencia, this._id_tipo_comprobante, this._id_origen_datos, this._id_certificado, this._version,
                                                   this._serie, this._folio, this._sello, this._id_forma_pago, this._id_metodo_pago, this._condicion_pago, this._id_moneda, this._subtotal_captura, this._impuestos_captura,
                                                   this._descuentos_captura, this._total_captura, this._subtotal_nacional, this._impuestos_nacional, this._descuentos_nacional, this._total_nacional,
                                                   this._tipo_cambio, this._lugar_expedicion, this._id_direccion_lugar_expedicion, this._confirmacion, this._id_compania_emisor, this._regimen_fiscal, this._id_sucursal,
                                                   this._id_compania_receptor, this._id_uso_receptor, this._fecha_captura, this._fecha_expedicion, this._fecha_cancelacion, this._bit_generado, this._bit_transferido_nuevo,
                                                   this._id_transferencia_nuevo, this._bit_transferido_cancelar, this._id_transferencia_cancelar, id_usuario, false);

                                //Validamos Resultado
                                if (result.OperacionExitosa)

                                    //Completamos Transacción
                                    scope.Complete();
                            }
                        }
                    }

                }
            }

            else
                result = new RetornoOperacion("El comprobante ya se ha timbrado, no es posible editarlo.");

            //Devolvemos Resultado
            return result;
        }
        /// <summary>
        /// Realiza la deshabilitación de un CFDI
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaCFDIComplementoRecepcionPagosV10(int id_usuario)
        {
            //Declarando objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion(this._id_comprobante33);

            //Validando tipo de CFDI
            if (this._id_tipo_comprobante == 5)
            {
                //Inicializando transacción
                using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    //Verificando que no se encuentre generado (timbrado)
                    if (!this._bit_generado)
                    {
                        //Validando que no esté sustituyendo a otro CFDI
                        //Determinando si este CFDI reemplaza a alguno previo
                        using (Comprobante cfdiSustituido = ComprobanteRelacion.ObtenerCFDISustituido(this._id_comprobante33))
                        {
                            //Si hay un elemento
                            if (cfdiSustituido.habilitar)
                                //Señalando que no es posible deshabilitar por la existencia de un CFDI previo
                                resultado = new RetornoOperacion(string.Format("No es posible Eliminar el CFDI, ya que este sustituye al CFDI '{0}{1}'", cfdiSustituido.serie, cfdiSustituido.folio));
                        }

                        //Si no hay errores
                        if (resultado.OperacionExitosa)
                        {
                            //Deshabilitando registro CFDI de Pago
                            resultado = DeshabilitaComprobanteSinFacturadoFacturacion(id_usuario);
                            //Si no hay errores al deshabilitar comprobante
                            if (resultado.OperacionExitosa)
                            {
                                //Recuperando conjunto de relaciones CFDI de Pago y Egreso/Ingreso activas
                                using (DataTable mitEIC = EgresoIngresoComprobante.ObtienePagosCFDIRecepcionPagos(this._id_comprobante33))
                                {
                                    //Si hay elementos
                                    if (mitEIC != null)
                                    {
                                        //Para cada relación
                                        foreach (DataRow r in mitEIC.Rows)
                                        {
                                            //Deshabilitando registro relación
                                            resultado = EgresoIngresoComprobante.DeshabilitaEgresoIngresoComprobante(r.Field<int>("IdEgresoIngresoComp"), id_usuario);

                                            //SI hay errores
                                            if (!resultado.OperacionExitosa)
                                                //Terminando iteraciones
                                                break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                        resultado = new RetornoOperacion("El CFDI ya se ha Timbrado, no es posible eliminarlo.");

                    //Si no hay errores, se confirman los cambios
                    if (resultado.OperacionExitosa)
                        scope.Complete();
                }
            }
            else
                resultado = new RetornoOperacion("El CFDI no es un Comprobante de Recepción de Pagos.");

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Realiza la petición de cancelación de un CFDI previo y genera un nuevo registro para su edición
        /// </summary>
        /// <param name="motivo">Motivo de Sustitución (Cancelación del CFDI Previo)</param>
        /// <param name="id_usuario">Id de Usuario que realiza la operación</param>
        /// <returns></returns>
        public RetornoOperacion SustituyeCFDIComplementoPagosV10(string motivo, int id_usuario)
        {
            //Declarando objeto de resultado (Sin errores)
            RetornoOperacion retorno = new RetornoOperacion(this._id_comprobante33);
            int id_nuevo_comprobante_pago = 0;

            //Inicializando operación transaccional
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Inicializando parametros
                retorno = this.actualizaAtributosBD(EstatusVigencia.PendienteCancelacion, this._id_tipo_comprobante, this._id_origen_datos, this._id_certificado, this._version,
                               this._serie, this._folio, this._sello, this._id_forma_pago, this._id_metodo_pago, this._condicion_pago, this._id_moneda, this._subtotal_captura, this._impuestos_captura,
                               this._descuentos_captura, this._total_captura, this._subtotal_nacional, this._impuestos_nacional, this._descuentos_nacional, this._total_nacional,
                               this._tipo_cambio, this._lugar_expedicion, this._id_direccion_lugar_expedicion, this._confirmacion, this._id_compania_emisor, this._regimen_fiscal, this._id_sucursal,
                               this._id_compania_receptor, this._id_uso_receptor, this._fecha_captura, this._fecha_expedicion, DateTime.MinValue,
                               this._bit_generado, this._bit_transferido_nuevo, this._id_transferencia_nuevo, this._bit_transferido_cancelar, this._id_transferencia_cancelar, id_usuario, this._habilitar);

                //Si no hay errores de cancelación
                if (retorno.OperacionExitosa)
                {
                    //Realizando la actualización de estatus de la relación de Pagos y el CFDI de pagos
                    using (DataTable mitPagosCFDI = EgresoIngresoComprobante.ObtienePagosCFDIRecepcionPagos(this._id_comprobante33))
                    {
                        //Si hay elementos por actualizar
                        if (mitPagosCFDI != null)
                        {
                            //Para cada elemento
                            foreach (DataRow p in mitPagosCFDI.Rows)
                            {
                                //Instanciando Relación
                                using (EgresoIngresoComprobante eic = new EgresoIngresoComprobante(p.Field<int>("IdEgresoIngresoComp")))
                                {
                                    //Si se localizó la relación
                                    if (eic.habilitar)
                                        //Actualizando estatus de relación
                                        retorno = eic.ActualizaEstatusEgresoIngresoComprobante(EgresoIngresoComprobante.Estatus.PorCancelar, id_usuario);
                                    else
                                        retorno = new RetornoOperacion(string.Format("Error al Recuperar Relación Pagos - CFDI ID: '{0}'", p.Field<int>("IdEgresoIngresoComp")));

                                    //Si hay errores se interrumpe ciclo
                                    if (!retorno.OperacionExitosa)
                                        break;
                                }
                            }
                        }
                        else
                            retorno = new RetornoOperacion("Error al Recuperar Pagos para su Actualización a Timbrado.");
                    }

                    //Si no hay errores hasta este punto
                    if (retorno.OperacionExitosa)
                    {
                        //Insertando Referencia
                        retorno = Referencia.InsertaReferencia(this._id_comprobante33, 209, ReferenciaTipo.ObtieneIdReferenciaTipo(0, 209, "Motivo Cancelación", 0, "Facturacion Electrónica"), motivo, Fecha.ObtieneFechaEstandarMexicoCentro(), id_usuario, true);

                        //Si no hay errores, se realiza inserción de motivo de cancelación (sustitución)
                        if (retorno.OperacionExitosa)
                        {
                            //Insertando nuevo CFDI de Pagos
                            using (DataSet dsSustitucion = Reporte.ObtienesDatosEncabezadoSustitucionFacturaElectronicaRecepcionPago(this._id_comprobante33))
                            {
                                //Si hay datos
                                if (Validacion.ValidaOrigenDatos(dsSustitucion))
                                {
                                    //Realizando operaciones de importación
                                    retorno = ImportaComprobante_V3_3_ReciboPago_V1_0(this._id_compania_receptor, dsSustitucion.Tables["Table"], dsSustitucion.Tables["Table1"], this._id_compania_emisor, id_sucursal, this._id_uso_receptor, id_usuario);
                                    //Preservando Id de nuevo comprobante
                                    id_nuevo_comprobante_pago = retorno.IdRegistro;
                                }
                            }
                        }
                        else
                            retorno = new RetornoOperacion("Error al Registrar el Motivo de Sustitución del CFDI Previo.");

                        //Si no hay errores
                        if (retorno.OperacionExitosa)
                        {
                            //Insertando relación con CFDI previo (Sustitución)
                            retorno = ComprobanteRelacion.InsertaComprobanteRelacion(id_nuevo_comprobante_pago, this._id_comprobante33, Convert.ToByte(Catalogo.RegresaValorCadenaValor(3193, "04")), 1, 0, id_usuario);
                        }
                    }
                }

                //Si no se localizan errores al final de la transacción
                if (retorno.OperacionExitosa)
                {
                    retorno = new RetornoOperacion(id_nuevo_comprobante_pago);
                    //Confirmando cambios realizados en BD
                    scope.Complete();
                }

            }

            //Devolviendo resultado
            return retorno;
        }
        /// <summary>
        /// Método encargado de Obtener el Origen y el Registro de un Comprobante
        /// </summary>
        /// <param name="id_comprobante"></param>
        /// <param name="origen_datos"></param>
        /// <param name="id_registro_entidad"></param>
        /// <returns></returns>
        public static RetornoOperacion ObtieneOrigenDatosComprobante(int id_comprobante, out OrigenDatos origen_datos, out int id_registro_entidad)
        {
            //Declarando Objetos de Retorno
            RetornoOperacion retorno = new RetornoOperacion("No se puede determinar el Origen de este Comprobante");
            origen_datos = OrigenDatos.Facturado;
            id_registro_entidad = 0;

            //Declarando Arreglo de Parametros
            object[] param = { 8, id_comprobante, 0, 0, 0, 0, "", "", "", "", 0, 0, "", 0, 0.00M, 0.00M, 0.00M, 0.00M, 0.00M, 0.00M, 0.00M,
                               0.00M, 0.00M, "", 0, "", 0, "", 0, 0, 0, null, null, null, false, false, 0, false, 0, 0, false, "", "" };

            //Obteniendo SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando resultado
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Obteniendo Origen
                    origen_datos = (OrigenDatos)(from DataRow dr in ds.Tables["Table"].Rows
                                                 select Convert.ToInt32(dr["Origen"])).FirstOrDefault();
                    //Obteniendo Entidad
                    id_registro_entidad = (from DataRow dr in ds.Tables["Table"].Rows
                                           select Convert.ToInt32(dr["Registro"])).FirstOrDefault();

                    //Validando Datos
                    if (id_registro_entidad > 0)

                        //Instanciando Retorno Positivo
                        retorno = new RetornoOperacion(id_comprobante);
                }
            }

            //Devolviendo Resultado Obtenido
            return retorno;
        }

        public RetornoOperacion ConsultaCancelacionComprobante(int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion retorno = new RetornoOperacion();
            CancelacionCDFI.RespuestaConsultaCFDI respuestaConsultaCFDI = CancelacionCDFI.RespuestaConsultaCFDI.SinRespuesta;
            XDocument consulta = new XDocument();

            //Obteniendo Origen del Comprobante
            OrigenDatos origen; int id_registro = 0;
            retorno = ObtieneOrigenDatosComprobante(this._id_comprobante33, out origen, out id_registro);

            //Validando Operación
            if (retorno.OperacionExitosa)
            {
                //Cancelando Comprobante
                retorno = CancelacionCDFI.objCancelacion.ConsultaComprobanteCxC(this._id_comprobante33, out respuestaConsultaCFDI, out consulta);

                //Obteniendo Timbre Fiscal
                using (TimbreFiscalDigital tfd = TimbreFiscalDigital.RecuperaTimbreFiscalComprobante(this._id_comprobante33))
                //Instanciando Detalle del Acuse
                using (AcuseCancelacionDetalle detalle = AcuseCancelacionDetalle.ObtieneAcuseTimbre(tfd.id_timbre_fiscal_digital))
                {
                    //Validando TFD
                    if (tfd.habilitar && detalle.habilitar_tacd)
                    {
                        //Inicializando Bloque Transaccional
                        //using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                        //{
                        //Validando Respuesta
                        switch (respuestaConsultaCFDI)
                        {
                            /**** Facturador ****/
                            case CancelacionCDFI.RespuestaConsultaCFDI.ComprobanteObtenido:
                                {
                                    //Obteniendo Estatus
                                    string esCancelable = consulta.Root.Element("EsCancelable").Value,
                                           estado = consulta.Root.Element("Estado").Value,
                                           estatusCancelacion = consulta.Root.Element("EstatusCancelacion").Value;

                                    //Validando si es Cancelable con Aceptación
                                    if (esCancelable.ToUpper().Equals("CANCELABLE CON ACEPTACIÓN"))
                                    {
                                        //Validando Estado del Comprobante
                                        if (estado.ToUpper().Equals("CANCELADO"))
                                        {
                                            //Actualizando Estatus
                                            retorno = detalle.ActualizaEstatusDetalle(AcuseCancelacionDetalle.EstatusCancelacion.Aceptado, id_usuario);

                                            //Validando Operación
                                            if (retorno.OperacionExitosa)
                                            {
                                                //Cancelando Detalle
                                                retorno = CancelaComprobante(id_usuario);

                                                //Validando Operación
                                                if (retorno.OperacionExitosa)
                                                {
                                                    //Validando Origen de Datos
                                                    switch (origen)
                                                    {
                                                        case OrigenDatos.Facturado:
                                                            {
                                                                //Deshabilitando Aplicaciones de Pago
                                                                retorno = SAT_CL.CXC.FichaIngresoAplicacion.DeshabilitaAplicacionesFacturado(id_registro, this._id_comprobante33, id_usuario);
                                                                break;
                                                            }
                                                        //case OrigenDatos.ReciboPagoCliente:
                                                        //    {
                                                        //        //Recuperando relaciones de pagos del CFDI previo para su actualización a cancelado
                                                        //        using (DataTable mitPagosCFDI = EgresoIngresoComprobante.ObtienePagosCFDIRecepcionPagos(this._id_comprobante33))
                                                        //        {
                                                        //            //Si hay elementos por actualizar
                                                        //            if (mitPagosCFDI != null)
                                                        //            {
                                                        //                //Para cada elemento
                                                        //                foreach (DataRow p in mitPagosCFDI.Rows)
                                                        //                {
                                                        //                    //Instanciando Relación
                                                        //                    using (EgresoIngresoComprobante eic = new EgresoIngresoComprobante(p.Field<int>("IdEgresoIngresoComp")))
                                                        //                    {
                                                        //                        //Si se localizó la relación
                                                        //                        if (eic.habilitar)

                                                        //                            //Actualizando estatus de relación
                                                        //                            retorno = eic.ActualizaEstatusEgresoIngresoComprobante(EgresoIngresoComprobante.Estatus.Cancelado, id_usuario);
                                                        //                        else
                                                        //                            //Instanciando Excepción
                                                        //                            retorno = new RetornoOperacion(string.Format("Error al Recuperar Relación Pagos - CFDI (Para sustitución) ID: '{0}'", p.Field<int>("IdEgresoIngresoComp")));

                                                        //                        //Si hay errores se interrumpe ciclo
                                                        //                        if (!retorno.OperacionExitosa)
                                                        //                            break;
                                                        //                    }
                                                        //                }
                                                        //            }
                                                        //            else
                                                        //                //Instanciando Excepción
                                                        //                retorno = new RetornoOperacion("Error al Recuperar Pagos de CFDI Sustituido para su Actualización a Cancelado.");
                                                        //        }
                                                        //        break;
                                                        //    }
                                                    }

                                                    //Validando Operación
                                                    if (retorno.OperacionExitosa)

                                                        //Instanciando Retorno Positivo
                                                        retorno = new RetornoOperacion(this._id_comprobante33, string.Format("El Comprobante '{0}{1}' fue Cancelado Exitosamente.", this._serie, this._folio), true);
                                                }
                                            }
                                        }
                                        //Validando que el CFDI este en Proceso de Cancelación
                                        else if (estado.ToUpper().Equals("VIGENTE") && estatusCancelacion.ToUpper().Equals("EN PROCESO"))
                                        {
                                            //Instanciando Retorno Positivo
                                            retorno = new RetornoOperacion(this._id_comprobante33, string.Format("El Comprobante '{0}{1}' fue sigue en Proceso de Cancelación", this._serie, this._folio), true);
                                        }
                                        //Validando que el CFDI siga vigente 
                                        else if (estado.ToUpper().Equals("VIGENTE") && (estatusCancelacion.ToUpper().Equals("") || estatusCancelacion.ToUpper().Equals("PLAZO VENCIDO") || estatusCancelacion.ToUpper().Equals("SOLICITUD RECHAZADA")))
                                        {
                                            //Cancelando Comprobante
                                            retorno = CancelacionRechazada(id_usuario);

                                            //Validando Operación
                                            if (retorno.OperacionExitosa)
                                            {
                                                //Actualizando Estatus
                                                retorno = detalle.ActualizaEstatusDetalle(AcuseCancelacionDetalle.EstatusCancelacion.Rechazado, id_usuario);

                                                //Validando Operación
                                                if (retorno.OperacionExitosa)

                                                    //Instanciando Retorno Positivo
                                                    retorno = new RetornoOperacion(this._id_comprobante33, string.Format("El Comprobante '{0}{1}' fue Rechazado por el Cliente, Cancelado Exitosamente.", this._serie, this._folio), true);
                                            }
                                        }

                                    }
                                    break;
                                }
                            /**** Facturemos Ya! ****/
                            case CancelacionCDFI.RespuestaConsultaCFDI.Cancelada:
                            case CancelacionCDFI.RespuestaConsultaCFDI.AceptacionCliente:
                                {
                                    //Actualizando Estatus
                                    retorno = detalle.ActualizaEstatusDetalle(AcuseCancelacionDetalle.EstatusCancelacion.Aceptado, id_usuario);

                                    //Validando Operación
                                    if (retorno.OperacionExitosa)
                                    {
                                        //Cancelando Detalle
                                        retorno = CancelaComprobante(id_usuario);

                                        //Validando Operación
                                        if (retorno.OperacionExitosa)
                                        {
                                            //Validando Origen de Datos
                                            switch (origen)
                                            {
                                                case OrigenDatos.Facturado:
                                                    {
                                                        //Deshabilitando Aplicaciones de Pago
                                                        retorno = SAT_CL.CXC.FichaIngresoAplicacion.DeshabilitaAplicacionesFacturado(id_registro, this._id_comprobante33, id_usuario);
                                                        break;
                                                    }
                                                case OrigenDatos.ReciboPagoCliente:
                                                    {
                                                        //Recuperando relaciones de pagos del CFDI previo para su actualización a cancelado
                                                        using (DataTable mitPagosCFDI = EgresoIngresoComprobante.ObtienePagosCFDIRecepcionPagos(this._id_comprobante33))
                                                        {
                                                            //Si hay elementos por actualizar
                                                            if (mitPagosCFDI != null)
                                                            {
                                                                //Para cada elemento
                                                                foreach (DataRow p in mitPagosCFDI.Rows)
                                                                {
                                                                    //Instanciando Relación
                                                                    using (EgresoIngresoComprobante eic = new EgresoIngresoComprobante(p.Field<int>("IdEgresoIngresoComp")))
                                                                    {
                                                                        //Si se localizó la relación
                                                                        if (eic.habilitar)

                                                                            //Actualizando estatus de relación
                                                                            retorno = eic.ActualizaEstatusEgresoIngresoComprobante(EgresoIngresoComprobante.Estatus.Cancelado, id_usuario);
                                                                        else
                                                                            //Instanciando Excepción
                                                                            retorno = new RetornoOperacion(string.Format("Error al Recuperar Relación Pagos - CFDI (Para sustitución) ID: '{0}'", p.Field<int>("IdEgresoIngresoComp")));

                                                                        //Si hay errores se interrumpe ciclo
                                                                        if (!retorno.OperacionExitosa)
                                                                            break;
                                                                    }
                                                                }
                                                            }
                                                            else
                                                                //Instanciando Excepción
                                                                retorno = new RetornoOperacion("Error al Recuperar Pagos de CFDI Sustituido para su Actualización a Cancelado.");
                                                        }
                                                        break;
                                                    }
                                            }

                                            //Validando Operación
                                            if (retorno.OperacionExitosa)

                                                //Instanciando Retorno Positivo
                                                retorno = new RetornoOperacion(this._id_comprobante33, string.Format("El Comprobante '{0}{1}' fue Cancelado Exitosamente.", this._serie, this._folio), true);
                                        }
                                    }
                                    break;
                                }
                        }

                        //Validando Operación Final
                        //if (retorno.OperacionExitosa)
                        //{
                        //    //Completando Transacción
                        //    scope.Complete();
                        //}
                        //}
                    }
                    else
                        //Instanciando Excepción
                        retorno = new RetornoOperacion("No se puede recuperar el Timbre Fiscal Digital");
                }
            }

            //Devolviendo Resultado Obtenido
            return retorno;
        }

        public static RetornoOperacion ConsultaCancelacionComprobante(int id_comprobante, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion retorno = new RetornoOperacion();
            
            //Instanciando CFDI
            using (Comprobante cfdi = new Comprobante(id_comprobante))
            {
                //Validando
                if (cfdi.habilitar)

                    //Consultando CFDI
                    retorno = cfdi.ConsultaCancelacionComprobante(id_usuario);
                else
                    //Instanciando Excepción
                    retorno = new RetornoOperacion("No se puede recuperar el CFDI");
            }

            //Devolviendo Resultado Obtenido
            return retorno;
        }


        #endregion
    }
}
