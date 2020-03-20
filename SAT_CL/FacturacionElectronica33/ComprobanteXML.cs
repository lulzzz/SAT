using System;
using System.Collections.Generic;
using System.Data;
using System.Xml.Linq;
using TSDK.Datos;
using System.Linq;
using TSDK.Base;
using System.Xml;
using System.Transactions;

namespace SAT_CL.FacturacionElectronica33
{
    
    public class ComprobanteXML
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de alamcenar el Nombre del SP
        /// </summary>
        private static readonly string _nom_sp = "fe33.sp_comprobante_xml_tcp";
        public static readonly XNamespace NS_W3C = "http://www.w3.org/2001/XMLSchema-instance";
        public static readonly XNamespace NS_CFDI_33 = "http://www.sat.gob.mx/cfd/3";
        public static readonly string NS_CFDI_33_PREFIX = "cfdi";
        public static readonly string SCHEMALOCATION_CFDI_33 = "http://www.sat.gob.mx/cfd/3 http://www.sat.gob.mx/sitio_internet/cfd/3/cfdv33.xsd";
        public static readonly XNamespace NS_NOMINA_12 = "http://www.sat.gob.mx/nomina12";
        public static readonly XNamespace NS_PAGO_10 = "http://www.sat.gob.mx/Pagos";
        public static readonly string SCHEMALOCATION_PAGO_10 = "http://www.sat.gob.mx/Pagos http://www.sat.gob.mx/sitio_internet/cfd/Pagos/Pagos10.xsd";
        public static readonly string NS_PAGO_10_PREFIX = "pago10";
        public static readonly string VERSION_PAGO_10 = "1.0";

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método que convierte los caracterés especiales de una cadena, en base a las especificaciones del SAT
        /// </summary>
        /// <param name="cadena">Cadena a validar </param>
        /// <returns></returns>
        private static string suprimeCaracteresEspecialesComprobante(string cadena)
        {
            //Inicialziando cadena de retorno
            string cadenaRetorno = "";

            //Reemplazando los caracteres especiales
            cadenaRetorno = cadena.Replace("&", @"&amp;");
            cadenaRetorno = cadena.Replace("\"", "&quot;");
            cadenaRetorno = cadena.Replace("<", "&lt;");
            cadenaRetorno = cadena.Replace(">", "&gt;");
            cadenaRetorno = cadena.Replace("'", "&apos;");

            //Devolvinedo cadena resultante
            return cadenaRetorno;
        }
        /// <summary>
        /// Método que transforma las columnas y valores de un DataRow en un arreglo XAttribute.
        /// (Las columnas con valor nulo o vacío no serán consideradas).
        /// </summary>
        /// <param name="fila">Fila que será transformada</param>
        /// <returns></returns>
        private static XAttribute[] creaAtributosElemento(DataRow fila)
        {
            //Devolvineod arreglo de atributos
            return creaAtributosElemento(fila, true);
        }
        /// <summary>
        /// Método que transforma las columnas y valores de un DataRow en un arreglo XAttribute.
        /// (Las columnas con valor nulo o vacío no serán consideradas).
        /// </summary>
        /// <param name="fila">Fila que será transformada</param>
        /// <param name="omitir_valor_cero">True para indicar que los valores numéricos iguales a 0 no serán considerados en los atributos de XElement</param>
        /// <returns></returns>
        private static XAttribute[] creaAtributosElemento(DataRow fila, bool omitir_valor_cero)
        {
            //Declarando objeto de retorno
            XAttribute[] atributos = null;

            //Si se tiene un registro que transformar
            if (fila != null)
            {
                //Lista de nombres 
                List<object> nombreColumna = new List<object>();

                //Lista de valores
                List<object> valorColumna = new List<object>();

                //Indice de columna
                int indice = 0;

                //Obteniendo arreglo de valores de fila
                object[] arregloColumnas = fila.ItemArray;

                //Recorriendo las columnas de la fila
                foreach (DataColumn columna in fila.Table.Columns)
                {
                    //Obteniendo el tipo de datos de la columna
                    Type t = columna.DataType;

                    //Determinando el Tipo de filtrado a realizar
                    //Fechas
                    if (t == typeof(DateTime))
                    {
                        //Si el valor de la fecha es diferente de nulo
                        if (arregloColumnas[indice] != DBNull.Value &&
                            arregloColumnas[indice] != null)
                        {
                            //Obteniendo nombre de columna actual
                            nombreColumna.Add(columna.ColumnName);
                            //Añadiendo el valor de columna al arreglo final
                            valorColumna.Add(Convert.ToDateTime(arregloColumnas[indice]).ToString("yyyy-MM-ddTHH:mm:ss"));
                        }
                    }

                    //Enteros
                    if (t == typeof(Int32))
                    {
                        //Si el valor es diferente de nulo
                        if (arregloColumnas[indice] != null)
                            //Si e valor es mayor a 0
                            if (Convert.ToInt32(arregloColumnas[indice]) > 0 || !omitir_valor_cero)
                            {
                                //Obteniendo nombre de columna actual
                                nombreColumna.Add(columna.ColumnName);
                                //Añadiendo el valor de columna al arreglo final
                                valorColumna.Add(arregloColumnas[indice]);
                            }
                    }

                    //Byte 0-254
                    if (t == typeof(Byte))
                    {
                        //Si el valor es diferente de nulo
                        if (arregloColumnas[indice] != null)
                            //Si e valor es mayor a 0
                            if (Convert.ToByte(arregloColumnas[indice]) > 0 || !omitir_valor_cero)
                            {
                                //Obteniendo nombre de columna actual
                                nombreColumna.Add(columna.ColumnName);
                                //Añadiendo el valor de columna al arreglo final
                                valorColumna.Add(arregloColumnas[indice]);
                            }
                    }

                    //Decimales
                    if (t == typeof(Decimal))
                    {
                        //Si el valor es diferente de nulo
                        if (arregloColumnas[indice] != DBNull.Value &&
                            arregloColumnas[indice] != null)
                            //Si e valor es mayor a 0
                            if (Convert.ToDecimal(arregloColumnas[indice]) > 0 || !omitir_valor_cero)
                            {
                                //Obteniendo nombre de columna actual
                                nombreColumna.Add(columna.ColumnName);
                                //Añadiendo el valor de columna al arreglo final
                                valorColumna.Add(arregloColumnas[indice]);
                            }
                    }

                    //Flotantes de precisión doble
                    if (t == typeof(Double))
                    {
                        //Si el valor es diferente de nulo
                        if (arregloColumnas[indice] != null)
                            //Si e valor es mayor a 0
                            if (Convert.ToDouble(arregloColumnas[indice]) > 0 || !omitir_valor_cero)
                            {
                                //Obteniendo nombre de columna actual
                                nombreColumna.Add(columna.ColumnName);
                                //Añadiendo el valor de columna al arreglo final
                                valorColumna.Add(arregloColumnas[indice]);
                            }
                    }

                    //Flotantes de precisión simple
                    if (t == typeof(Single))
                    {
                        //Si el valor es diferente de nulo
                        if (arregloColumnas[indice] != null)
                            //Si e valor es mayor a 0
                            if (Convert.ToSingle(arregloColumnas[indice]) > 0 || !omitir_valor_cero)
                            {
                                //Obteniendo nombre de columna actual
                                nombreColumna.Add(columna.ColumnName);
                                //Añadiendo el valor de columna al arreglo final
                                valorColumna.Add(arregloColumnas[indice]);
                            }
                    }

                    //Cadenas de Texto
                    if (t == typeof(String))
                    {
                        //Si no es un valor vacio
                        if (!string.IsNullOrEmpty(arregloColumnas[indice].ToString()))
                        {
                            //Obteniendo nombre de columna actual
                            nombreColumna.Add(columna.ColumnName);
                            //Añadiendo el valor de columna al arreglo final
                            valorColumna.Add(suprimeCaracteresEspecialesComprobante(arregloColumnas[indice].ToString()));
                        }
                    }

                    //Incrementando indice
                    indice++;
                }

                //Dimensionendo objeto de retorno
                atributos = new XAttribute[valorColumna.Count];

                //Recorriendo lista de elementos
                for (indice = 0; indice < valorColumna.Count; indice++)
                    //Añadiendo nuevo atributo al arreglo de retorno
                    atributos[indice] = new XAttribute(nombreColumna[indice].ToString(), valorColumna[indice]);
            }

            //Devolvineod arreglo de atributos
            return atributos;
        }
        /// <summary>
        /// Construye un elemento sin dependencias a partir de un origen de datos, asignandole el nombre indicado
        /// </summary>
        /// <param name="ds">Origen de Datos</param>
        /// <param name="tabla">Tabla del Origen de Datos</param>
        /// <param name="nombre_elemento">Nombre del elemento [ns + nombre]</param>
        /// <returns></returns>
        private static XElement creaElementoGenericoComprobante(DataSet ds, string tabla, XName nombre_elemento)
        {
            //Definiendo elemento de retorno
            XElement elemento = null;

            //Validando el origen de datos
            if (Validacion.ValidaOrigenDatos(ds, tabla))
            {
                //Definiendo erreglo de atributos
                XAttribute[] atributos = null;

                //Para cada registro
                foreach (DataRow r in ds.Tables[tabla].Rows)
                    atributos = creaAtributosElemento(r);

                //Creando elemento
                elemento = new XElement(nombre_elemento, atributos);
            }

            //Devolviendo elemento creado
            return elemento;
        }
        /// <summary>
        /// Construye un conjunto de elementos partir de un origen de datos, asignandole el nombre indicado
        /// </summary>
        /// <param name="ds">Origen de Datos</param>
        /// <param name="tabla">Tabla del Origen de Datos</param>
        /// <param name="nombre_elemento">Nombre de cada elemento [ns + nombre]</param>
        /// <returns></returns>
        private static XElement[] creaElementosComprobante(DataSet ds, string tabla, XName nombre_elemento)
        {
            //Definiendo elemento de retorno
            List<XElement> elementos = new List<XElement>();

            //Validando el origen de datos
            if (Validacion.ValidaOrigenDatos(ds, tabla))
                creaElementosComprobante(ds.Tables[tabla], nombre_elemento);

            //Devolviendo elemento creado
            return elementos.ToArray();
        }
        /// <summary>
        /// Construye un conjunto de elementos partir de un origen de datos, asignandole el nombre indicado
        /// </summary>
        /// <param name="mit">Origen de Datos</param>
        /// <param name="nombre_elemento">Nombre de cada elemento [ns + nombre]</param>
        /// <returns></returns>
        private static XElement[] creaElementosComprobante(DataTable mit, XName nombre_elemento)
        {
            //Devolviendo elemento creado
            return creaElementosComprobante(mit, nombre_elemento, true);
        }
        /// <summary>
        /// Construye un conjunto de elementos partir de un origen de datos, asignandole el nombre indicado
        /// </summary>
        /// <param name="mit">Origen de Datos</param>
        /// <param name="nombre_elemento">Nombre de cada elemento [ns + nombre]</param>
        /// <param name="omitir_valor_cero">True para indicar que los valores numéricos iguales a 0 no serán considerados en los atributos de XElement</param>
        /// <returns></returns>
        private static XElement[] creaElementosComprobante(DataTable mit, XName nombre_elemento, bool omitir_valor_cero)
        {
            //Definiendo elemento de retorno
            List<XElement> elementos = new List<XElement>();

            //Validando el origen de datos
            if (Validacion.ValidaOrigenDatos(mit, true))
            {
                //Para cada registro
                foreach (DataRow r in mit.Rows)
                    //Añadiendo el elemento a la lista de resultados
                    elementos.Add(new XElement(nombre_elemento, creaAtributosElemento(r, omitir_valor_cero)));
            }

            //Devolviendo elemento creado
            return elementos.ToArray();
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método encargado de Crear la Estructura del XML del CFDI en su versión 3.3
        /// </summary>
        /// <param name="id_comprobante"></param>
        /// <param name="ns_sat"></param>
        /// <param name="ns_w3c"></param>
        /// <param name="schema_location"></param>
        /// <returns></returns>
        public static XDocument CargaElementosArmadoComprobante3_3(int id_comprobante, XNamespace ns_sat, XNamespace ns_w3c, string schema_location)
        {
            //Declarando Objeto de Retorno
            XDocument cfdi3_3 = new XDocument();

            //Definiendo objeto de retorno
            cfdi3_3.Add(new XElement(ns_sat + "Comprobante",
                                                new XAttribute(XNamespace.Xmlns + "xsi", ns_w3c.ToString()),
                                                new XAttribute(XNamespace.Xmlns + CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Prefijo Namespace SAT", 0), ns_sat.ToString()),
                                                new XAttribute(ns_w3c + "schemaLocation", schema_location)));

            //Armando Arreglo de Parametros
            object[] param = { 1, id_comprobante, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };

            //Validando Comprobante
            if (cfdi3_3.Root != null)
            {
                //Instanciando Datos del Comprobante
                using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
                {
                    //Si el origen de datos es válido
                    if (Validacion.ValidaOrigenDatos(ds, true))
                    {                        
                        /** Comprobante **/
                        //Validando Datos del Encabezado
                        if (Validacion.ValidaOrigenDatos(ds, "Table"))
                        {
                            //Recorriendo Registros de Tabla
                            foreach (DataRow r in ds.Tables["Table"].Rows)
                                //Recuperando atributos de registro
                                cfdi3_3.Root.Add(creaAtributosElemento(r));
                        }

                        /** CFDI's Relacionados **/
                        //Validando Datos del Encabezado
                        if (Validacion.ValidaOrigenDatos(ds, "Table1"))
                        {
                            //Obteniendo Tipos de Relación
                            List<string> tipos_relacion = (from DataRow dr in ds.Tables["Table1"].Rows
                                                           select dr.Field<string>("TipoRelacion")).Distinct().ToList();

                            //Recorriendo Tipos de Relación
                            foreach (string tipo in tipos_relacion)
                            {
                                //Obteniendo Comprobantes que el Tipo de relación del Ciclo
                                List<string> uuids = (from DataRow dr in ds.Tables["Table1"].Rows
                                                      where dr.Field<string>("TipoRelacion").Equals(tipo)
                                                      select dr.Field<string>("UUID")).Distinct().ToList();

                                //Creando Elemento Principal de los CFDI's Relacionados
                                XElement cfdi_rel = new XElement(ns_sat + "CfdiRelacionados", new XAttribute("TipoRelacion", tipo));
                                if (cfdi_rel != null)
                                {
                                    //Recorriendo UUID's de los Comprobantes
                                    foreach (string uuid in uuids)
                                        //Creando Nodos relacionados del CFDI
                                        cfdi_rel.Add(new XElement(ns_sat + "CfdiRelacionado", new XAttribute("UUID", uuid)));
                                    
                                    //Añadiendo a Nodo Principal
                                    cfdi3_3.Root.Add(cfdi_rel);
                                }
                            }
                        }

                        /** Emisor **/
                        //Validando Datos
                        if (Validacion.ValidaOrigenDatos(ds, "Table2"))
                        {
                            //Creando Nodo de Emisor
                            cfdi3_3.Root.Add(new XElement(ns_sat + "Emisor"));

                            //Recorriendo Atributos
                            foreach (DataRow dr in ds.Tables["Table2"].Rows)
                            {
                                //Añadiendo al Elemento Emisor
                                cfdi3_3.Root.Element(ns_sat + "Emisor").Add(creaAtributosElemento(dr));
                                //Terminando Ciclo
                                break;
                            }
                        }

                        /** Receptor **/
                        //Validando Datos
                        if (Validacion.ValidaOrigenDatos(ds, "Table3"))
                        {
                            //Creando Nodo de Receptor
                            cfdi3_3.Root.Add(new XElement(ns_sat + "Receptor"));

                            //Recorriendo Atributos
                            foreach (DataRow dr in ds.Tables["Table3"].Rows)
                            {
                                //Añadiendo al Elemento Receptor
                                cfdi3_3.Root.Element(ns_sat + "Receptor").Add(creaAtributosElemento(dr));
                                //Terminando Ciclo
                                break;
                            }
                        }

                        //Declarando Listas Auxiliares
                        List<DataRow> impuestos, info_aduanera, imp_tras, imp_ret;
                        impuestos = info_aduanera = imp_tras = imp_ret = new List<DataRow>();

                        /** Conceptos **/
                        //Validando Datos
                        if (Validacion.ValidaOrigenDatos(ds, "Table4"))
                        {
                            //Creando Nodo de Conceptos
                            cfdi3_3.Root.Add(new XElement(ns_sat + "Conceptos"));

                            //Recorriendo Atributos
                            foreach (DataRow dr in ds.Tables["Table4"].Rows)
                            {
                                //Obteniendo Concepto Actual del Ciclo
                                int idConcepto = Convert.ToInt32(dr["IdConcepto"]);
                                
                                //Declarando Objeto de Nuevo Concepto
                                XElement concepto = new XElement(ns_sat + "Concepto", creaAtributosElemento(OrigenDatos.EliminaColumnaDataRow(dr, "IdConcepto-IdConceptoPadre-CuentaPredial")));
                                
                                //Validando que exista la Cuenta Predial
                                if (!(dr["CuentaPredial"].ToString().Equals("")))
                                
                                    //Creando Elemento de Cuenta Predial
                                    concepto.Add(new XElement(ns_sat + "CuentaPredial", new XAttribute("Numero", dr["CuentaPredial"].ToString())));
                                
                                //Obteniendo Partes en caso de Existir
                                List<DataRow> partes = (from DataRow parte in ds.Tables["Table4"].Rows
                                                        where parte.Field<int>("IdConceptoPadre") == idConcepto
                                                        select parte).ToList();

                                //Validando que existan las partes
                                if (partes.Count > 0)
                                {
                                    //Recorriendo Partes del Concepto
                                    foreach (DataRow parte in partes)
                                    {
                                        //Declarando Elemento de Parte
                                        XElement elemento_parte = new XElement(ns_sat + "Parte", creaAtributosElemento(OrigenDatos.EliminaColumnaDataRow(parte, "IdConcepto-IdConceptoPadre-CuentaPredial")));

                                        //Validando Que exista el Elemento
                                        if (elemento_parte != null)

                                            //Añadiendo a Concepto Base
                                            concepto.Add(elemento_parte);
                                    }
                                }

                                /** Impuestos por Concepto **/
                                //Validando Datos
                                if (Validacion.ValidaOrigenDatos(ds, "Table5"))
                                
                                    //Obteniendo Partes en caso de Existir
                                    impuestos = (from DataRow parte in ds.Tables["Table5"].Rows
                                                 where parte.Field<int>("IdConcepto") == idConcepto
                                                 select parte) .ToList();

                                /** Información Aduanera por Concepto **/
                                //Validando Datos
                                if (Validacion.ValidaOrigenDatos(ds, "Table6"))

                                    //Obteniendo Partes en caso de Existir
                                    info_aduanera = (from DataRow parte in ds.Tables["Table6"].Rows
                                                     where parte.Field<int>("IdConcepto") == idConcepto
                                                     select parte).ToList();

                                //Validando si existen Impuestos de ese Concepto
                                if (impuestos.Count > 0)
                                {
                                    //Creando Nodo de Impuestos
                                    XElement impuesto_concepto = new XElement(ns_sat + "Impuestos");

                                    //Obteniendo Trasladados y Retenciones
                                    imp_tras = (from DataRow traslados in impuestos
                                                where traslados.Field<string>("Detalle").Equals("Traslado")
                                                select traslados).ToList();
                                    imp_ret = (from DataRow traslados in impuestos
                                               where traslados.Field<string>("Detalle").Equals("Retencion")
                                               select traslados).ToList();

                                    //Validando que existan Traslados
                                    if (imp_tras.Count > 0)
                                    {
                                        //Añadiendo Nodo de Traslados
                                        XElement traslados = new XElement(ns_sat + "Traslados");
                                        
                                        //Recorriendo Traslados
                                        foreach (DataRow traslado in imp_tras)
                                        
                                            //Añadiendo Nodo a Traslados
                                            traslados.Add(new XElement(ns_sat + "Traslado", creaAtributosElemento(OrigenDatos.EliminaColumnaDataRow(traslado, "IdConcepto-Detalle"))));

                                        //Añadiendo Traslados a Impuesto
                                        impuesto_concepto.Add(traslados);
                                    }

                                    //Validando que existan Retenciones
                                    if (imp_ret.Count > 0)
                                    {
                                        //Añadiendo Nodo de Retenciones
                                        XElement retenciones = new XElement(ns_sat + "Retenciones");

                                        //Recorriendo Retenciones
                                        foreach (DataRow retencion in imp_ret)

                                            //Añadiendo Nodo a Retenciones
                                            retenciones.Add(new XElement(ns_sat + "Retencion", creaAtributosElemento(OrigenDatos.EliminaColumnaDataRow(retencion, "IdConcepto-Detalle"))));

                                        //Añadiendo Retenciones a Impuesto
                                        impuesto_concepto.Add(retenciones);
                                    }

                                    //Añadiendo Nodo de Impuestos al Concepto
                                    concepto.Add(impuesto_concepto);
                                }

                                //Validando si existe Información Aduanera de ese Concepto
                                if (info_aduanera.Count > 0)
                                {
                                    //Recorriendo Datos de Información Aduanera
                                    foreach (DataRow inf_ad in info_aduanera)
                                    
                                        //Creando Nodo de Impuestos
                                        concepto.Add(new XElement(ns_sat + "InformacionAduanera", new XAttribute("NumeroPedimento", inf_ad.Field<string>("NumeroPedimento"))));
                                }
                                
                                //Añadiendo Concepto al Comprobante
                                cfdi3_3.Root.Element(ns_sat + "Conceptos").Add(concepto);
                            }
                        }

                        /** Impuestos del Comprobante **/
                        //Validando Datos
                        if (Validacion.ValidaOrigenDatos(ds, "Table7"))
                        {
                            //Recorriendo Impuestos
                            foreach (DataRow imp in ds.Tables["Table7"].Rows)
                            {
                                //Impuestos Trasladados por Comprobante
                                List<DataRow> imps_t = (from DataRow it in ds.Tables["Table5"].Rows
                                                      where it.Field<string>("Detalle").Equals("Traslado")
                                                      select it).ToList();
                                //Impuestos Retenidos por Comprobante
                                List<DataRow> imps_r = (from DataRow ir in ds.Tables["Table5"].Rows
                                                        where ir.Field<string>("Detalle").Equals("Retencion")
                                                        select ir).ToList();

                                //Validando que existan
                                if (imps_t.Count > 0 || imps_r.Count > 0)
                                {
                                    //Añadiendo Elemento del Impuesto del Comprobante
                                    XElement impuesto_cfdi = new XElement(ns_sat + "Impuestos", creaAtributosElemento(imp));

                                    //Retenciones
                                    if (imps_r.Count > 0)
                                    {
                                        //Creando Nodo de Traslados
                                        impuesto_cfdi.Add(new XElement(ns_sat + "Retenciones"));

                                        //Declarando Lista Auxiliar
                                        List<Tuple<string, decimal>> impuestos_ret_agrup = new List<Tuple<string, decimal>>();

                                        //Agrupando por Impuesto
                                        List<string> tipo_imp_r = (from DataRow ti in imps_r
                                                                   select ti.Field<string>("Impuesto")).Distinct().ToList();

                                        //Recorriendo Tipos de Impuesto
                                        foreach (string tipo in tipo_imp_r)
                                        {
                                            //Obteniendo Importe Total de la Combinación
                                            decimal importe_r = (from DataRow r in imps_r
                                                                 where r.Field<string>("Impuesto").Equals(tipo)
                                                                 select r.Field<decimal>("Importe")).Sum();

                                            //Añadiendo a Lista de Impuestos Agrupados
                                            impuestos_ret_agrup.Add(new Tuple<string, decimal>(tipo, importe_r));
                                        }

                                        //Recorriendo Resultados Agrupados
                                        foreach (Tuple<string, decimal> retencion in impuestos_ret_agrup)
                                        {
                                            //Creando Traslado
                                            impuesto_cfdi.Element(ns_sat + "Retenciones").Add(new XElement(ns_sat + "Retencion",
                                                    new XAttribute("Impuesto", retencion.Item1), new XAttribute("Importe", retencion.Item2)));
                                        }
                                    }
                                    
                                    //Traslados
                                    if (imps_t.Count > 0)
                                    {
                                        //Creando Nodo de Traslados
                                        impuesto_cfdi.Add(new XElement(ns_sat + "Traslados"));

                                        //Declarando Lista Auxiliar
                                        List<Tuple<string, string, decimal, decimal>> impuestos_tras_agrup = new List<Tuple<string, string, decimal, decimal>>();
                                        
                                        //Agrupando por Impuesto
                                        List<string> tipo_imp_t = (from DataRow ti in imps_t
                                                                   select ti.Field<string>("Impuesto")).Distinct().ToList();

                                        //Recorriendo Tipos de Impuesto
                                        foreach (string tipo in tipo_imp_t)
                                        {
                                            //Obteniendo 
                                            List<Tuple<string, decimal>> imp_imp_t = (from DataRow it in imps_t
                                                                                      where it.Field<string>("Impuesto").Equals(tipo)
                                                                                      select new Tuple<string, decimal>
                                                                                      (it.Field<string>("TipoFactor"), 
                                                                                       it.Field<decimal>("TasaOCuota"))).Distinct().ToList();

                                            //Validando Existencias
                                            if (imp_imp_t.Count > 0)
                                            {
                                                //Recorriendo Coincidencia de Pares
                                                foreach (Tuple<string, decimal> tuple in imp_imp_t)
                                                {
                                                    //Obteniendo Importe Total de la Combinación
                                                    decimal importe_t = (from DataRow r in imps_t
                                                                         where r.Field<string>("TipoFactor").Equals(tuple.Item1)
                                                                         && r.Field<decimal>("TasaOCuota") == tuple.Item2
                                                                         && r.Field<string>("Impuesto").Equals(tipo)
                                                                         select r.Field<decimal>("Importe")).Sum();

                                                    //Añadiendo a Lista de Impuestos Agrupados
                                                    impuestos_tras_agrup.Add(new Tuple<string, string, decimal, decimal>(tipo, tuple.Item1, tuple.Item2, importe_t));
                                                }
                                            }
                                        }

                                        //Recorriendo Resultados Agrupados
                                        foreach (Tuple<string, string, decimal, decimal> traslado in impuestos_tras_agrup)
                                        {
                                            //Creando Traslado
                                            impuesto_cfdi.Element(ns_sat + "Traslados").Add(new XElement(ns_sat + "Traslado",
                                                    new XAttribute("Impuesto", traslado.Item1), new XAttribute("TipoFactor", traslado.Item2),
                                                    new XAttribute("TasaOCuota", traslado.Item3), new XAttribute("Importe", traslado.Item4)));
                                        }
                                    }

                                    //Añadiendo Impuestos al Comprobante
                                    cfdi3_3.Root.Add(impuesto_cfdi);
                                }
                            }
                        }
                    }
                }
            }

            //Devolviendo Resultado Obtenido
            return cfdi3_3;
        }
        
        #region Métodos Nomina 1.2

        /// <summary>
        /// 
        /// </summary>
        /// <param name="version"></param>
        /// <param name="id_nomina_empleado"></param>
        /// <param name="id_comprobante"></param>
        /// <param name="ns_sat"></param>
        /// <param name="nsn"></param>
        /// <param name="ns_w3c"></param>
        /// <param name="ns_wn12"></param>
        /// <param name="schema_location"></param>
        /// <param name="id_emisor"></param>
        /// <param name="id_receptor"></param>
        /// <param name="id_usuario"></param>
        /// <param name="resultado"></param>
        /// <returns></returns>
        public static XElement CargaElementosArmadoComprobanteReciboNomina_V_3_3(string version, int id_nomina_empleado, int id_comprobante, XNamespace ns_sat, XNamespace nsn, XNamespace ns_w3c, XNamespace ns_wn12, string schema_location, int id_emisor, int id_receptor,
                                                                               int id_usuario, out RetornoOperacion resultado)
        {
            //Declaramos Resultado Complemento
            resultado = new RetornoOperacion();

            //Definiendo objeto de retorno
            XElement cfdi3_3 = new XElement(ns_sat + "Comprobante",
                                               new XAttribute(XNamespace.Xmlns + "xsi", ns_w3c.ToString()),
                                                new XAttribute(XNamespace.Xmlns + CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Prefijo Namespace SAT", 0), ns_sat.ToString()),
                                                new XAttribute(ns_w3c + "schemaLocation", schema_location));

            //Definiendo parametros de consulta
            object[] param = { 2, id_comprobante, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };

            //Ejecutando Stored Procedure
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Si el origen de datos es válido
                if (Validacion.ValidaOrigenDatos(ds, true))
                {
                    /**** Comprobante ****/
                    if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    {
                        //Recorriendo Registros de Tabla
                        foreach (DataRow r in ds.Tables["Table"].Rows)
                            //Recuperando atributos de registro
                            cfdi3_3.Add(creaAtributosElemento(r));
                    }

                    /**** CFDI's Relacionados ****/
                    //Validando Datos del Encabezado
                    if (Validacion.ValidaOrigenDatos(ds, "Table1"))
                    {
                        //Obteniendo Tipos de Relación
                        List<string> tipos_relacion = (from DataRow dr in ds.Tables["Table1"].Rows
                                                       select dr.Field<string>("TipoRelacion")).Distinct().ToList();

                        //Recorriendo Tipos de Relación
                        foreach (string tipo in tipos_relacion)
                        {
                            //Obteniendo Comprobantes que el Tipo de relación del Ciclo
                            List<string> uuids = (from DataRow dr in ds.Tables["Table1"].Rows
                                                  where dr.Field<string>("TipoRelacion").Equals(tipo)
                                                  select dr.Field<string>("UUID")).Distinct().ToList();

                            //Creando Elemento Principal de los CFDI's Relacionados
                            cfdi3_3.Add(new XElement(ns_sat + "CfdiRelacionados", new XAttribute("TipoRelacion", tipo)));

                            //Recorriendo UUID's de los Comprobantes
                            foreach (string uuid in uuids)
                                //Creando Nodos relacionados del CFDI
                                cfdi3_3.Element(ns_sat + "CfdiRelacionados").Add(new XElement(ns_sat + "CfdiRelacionado", new XAttribute("UUID", uuid)));
                        }
                    }

                    /**** Emisor ****/
                    cfdi3_3.Add(creaElementoGenericoComprobante(ds, "Table2", ns_sat + "Emisor"));

                    /**** Receptor ****/
                    cfdi3_3.Add(creaElementoGenericoComprobante(ds, "Table3", ns_sat + "Receptor"));

                    //Declarando Listas Auxiliares
                    List<DataRow> impuestos, info_aduanera, imp_tras, imp_ret;
                    impuestos = info_aduanera = imp_tras = imp_ret = new List<DataRow>();

                    /**** Conceptos ****/
                    //Validando Datos
                    if (Validacion.ValidaOrigenDatos(ds, "Table4"))
                    {
                        //Creando Nodo de Conceptos
                        cfdi3_3.Add(new XElement(ns_sat + "Conceptos"));

                        //Recorriendo Atributos
                        foreach (DataRow dr in ds.Tables["Table4"].Rows)
                        {
                            //Obteniendo Concepto Actual del Ciclo
                            int idConcepto = Convert.ToInt32(dr["IdConcepto"]);

                            //Declarando Objeto de Nuevo Concepto
                            XElement concepto = new XElement(ns_sat + "Concepto", creaAtributosElemento(OrigenDatos.EliminaColumnaDataRow(dr, "IdConcepto-IdConceptoPadre-CuentaPredial")));

                            //Validando que exista la Cuenta Predial
                            if (!(dr["CuentaPredial"].ToString().Equals("")))

                                //Creando Elemento de Cuenta Predial
                                concepto.Add(new XElement(ns_sat + "CuentaPredial", new XAttribute("Numero", dr["CuentaPredial"].ToString())));

                            //Obteniendo Partes en caso de Existir
                            List<DataRow> partes = (from DataRow parte in ds.Tables["Table4"].Rows
                                                    where parte.Field<int>("IdConceptoPadre") == idConcepto
                                                    select parte).ToList();

                            //Validando que existan las partes
                            if (partes.Count > 0)
                            {
                                //Recorriendo Partes del Concepto
                                foreach (DataRow parte in partes)
                                {
                                    //Declarando Elemento de Parte
                                    XElement elemento_parte = new XElement(ns_sat + "Parte", creaAtributosElemento(OrigenDatos.EliminaColumnaDataRow(parte, "IdConcepto-IdConceptoPadre-CuentaPredial")));

                                    //Validando Que exista el Elemento
                                    if (elemento_parte != null)

                                        //Añadiendo a Concepto Base
                                        concepto.Add(elemento_parte);
                                }
                            }

                            /** Impuestos por Concepto **/
                            //Validando Datos
                            if (Validacion.ValidaOrigenDatos(ds, "Table5"))

                                //Obteniendo Partes en caso de Existir
                                impuestos = (from DataRow parte in ds.Tables["Table5"].Rows
                                             where parte.Field<int>("IdConcepto") == idConcepto
                                             select parte).ToList();

                            /** Información Aduanera por Concepto **/
                            //Validando Datos
                            if (Validacion.ValidaOrigenDatos(ds, "Table6"))

                                //Obteniendo Partes en caso de Existir
                                info_aduanera = (from DataRow parte in ds.Tables["Table6"].Rows
                                                 where parte.Field<int>("IdConcepto") == idConcepto
                                                 select parte).ToList();

                            //Validando si existen Impuestos de ese Concepto
                            if (impuestos.Count > 0)
                            {
                                //Creando Nodo de Impuestos
                                XElement impuesto_concepto = new XElement(ns_sat + "Impuestos");

                                //Obteniendo Trasladados y Retenciones
                                imp_tras = (from DataRow traslados in impuestos
                                            where traslados.Field<string>("Detalle").Equals("Traslado")
                                            select traslados).ToList();
                                imp_ret = (from DataRow traslados in impuestos
                                           where traslados.Field<string>("Detalle").Equals("Retencion")
                                           select traslados).ToList();

                                //Validando que existan Traslados
                                if (imp_tras.Count > 0)
                                {
                                    //Añadiendo Nodo de Traslados
                                    XElement traslados = new XElement(ns_sat + "Traslados");

                                    //Recorriendo Traslados
                                    foreach (DataRow traslado in imp_tras)

                                        //Añadiendo Nodo a Traslados
                                        traslados.Add(new XElement(ns_sat + "Traslado", creaAtributosElemento(OrigenDatos.EliminaColumnaDataRow(traslado, "IdConcepto-Detalle"))));

                                    //Añadiendo Traslados a Impuesto
                                    impuesto_concepto.Add(traslados);
                                }

                                //Validando que existan Retenciones
                                if (imp_ret.Count > 0)
                                {
                                    //Añadiendo Nodo de Retenciones
                                    XElement retenciones = new XElement(ns_sat + "Retenciones");

                                    //Recorriendo Retenciones
                                    foreach (DataRow retencion in imp_ret)

                                        //Añadiendo Nodo a Retenciones
                                        retenciones.Add(new XElement(ns_sat + "Retencion", creaAtributosElemento(OrigenDatos.EliminaColumnaDataRow(retencion, "IdConcepto-Detalle"))));

                                    //Añadiendo Retenciones a Impuesto
                                    impuesto_concepto.Add(retenciones);
                                }

                                //Añadiendo Nodo de Impuestos al Concepto
                                concepto.Add(impuesto_concepto);
                            }

                            //Validando si existe Información Aduanera de ese Concepto
                            if (info_aduanera.Count > 0)
                            {
                                //Recorriendo Datos de Información Aduanera
                                foreach (DataRow inf_ad in info_aduanera)

                                    //Creando Nodo de Impuestos
                                    concepto.Add(new XElement(ns_sat + "InformacionAduanera", new XAttribute("NumeroPedimento", inf_ad.Field<string>("NumeroPedimento"))));
                            }

                            //Añadiendo Concepto al Comprobante
                            cfdi3_3.Element(ns_sat + "Conceptos").Add(concepto);
                        }
                    }

                    /** Complemento(s) **/
                    using (DataTable addendas = SAT_CL.FacturacionElectronica.AddendaEmisor.CargaComplementosRequeridosEmisorReceptor(id_emisor, id_receptor, "Nómina " + version))
                    {
                        //Si existen addendas configuradas
                        if (Validacion.ValidaOrigenDatos(addendas))
                        {
                            cfdi3_3.Add(new XElement(ns_sat + "Complemento"));
                            //Si se añadió correctamente
                            if (cfdi3_3.Element(ns_sat + "Complemento") != null)

                                //Generamos complemento de Nomina
                                SAT_CL.FacturacionElectronica.ComprobanteXML.ArmadoComplementoNomina(version, id_comprobante, nsn, id_nomina_empleado, id_emisor, id_usuario, out resultado);
                        }
                    }
                }
            }

            //Devolviendo Resultado Obtenido
            return cfdi3_3;
        }
        /// <summary>
        ///  Método encargado de generar el XML  para complemento de Nómina
        /// </summary>
        /// <param name="id_comprobante"></param>
        /// <param name="id_emisor"></param>
        /// <param name="id_receptor"></param>
        /// <param name="nomina"></param>
        /// <param name="percepciones"></param>
        /// <param name="percepion"></param>
        /// <param name="deducciones"></param>
        /// <param name="deduccion"></param>
        /// <param name="incapacidad"></param>
        /// <param name="horasextra"></param>
        /// <param name="ns_sat_nomina"></param>
        /// <param name="id_usuario"></param>
        /// <param name="resultado"></param>
        private static void complementoNominaXML(int id_comprobante, int id_emisor, int id_receptor, DataTable nomina, DataTable percepciones, DataTable percepion,
                                            DataTable deducciones, DataTable deduccion, DataTable incapacidad, DataTable horasextra, XNamespace ns_sat_nomina, int id_usuario, out RetornoOperacion resultado)
        {
            //Declaramos objeto Resultado
            resultado = new RetornoOperacion();

            //Cargamos Complemeto Nomina Emisor Esqueleto  
            using (SAT_CL.FacturacionElectronica.AddendaEmisor objAddendaEmisor = new SAT_CL.FacturacionElectronica.AddendaEmisor(id_emisor, id_receptor))
            {
                //Validamos Esqueleto de Adenda 
                if (objAddendaEmisor.id_emisor_addenda > 0)
                {
                    //Cargamos XML Predeterminado
                    System.Xml.XmlDocument xmlDocumentoReciboN = objAddendaEmisor.xml_predeterminado;

                    //Creamos Dataset para añadir tablas para el complemento de nomina
                    DataSet dsnomina = null;

                    //Añadimos Tabla a Dataset Nomina
                    dsnomina = OrigenDatos.AñadeTablaDataSet(dsnomina, nomina, "nomina");
                    //Añadimos Tabla a Dataset Percepciones
                    dsnomina = OrigenDatos.AñadeTablaDataSet(dsnomina, percepciones, "percepciones");
                    //Añadimos Tabla a Dataset Percepcion
                    dsnomina = OrigenDatos.AñadeTablaDataSet(dsnomina, percepion, "percepcion");
                    //Añadimos Tabla a Dataset Deducciones
                    dsnomina = OrigenDatos.AñadeTablaDataSet(dsnomina, deducciones, "deducciones");
                    //Añadimos Tabla a Dataset Deduccion
                    dsnomina = OrigenDatos.AñadeTablaDataSet(dsnomina, deduccion, "deduccion");
                    //Añadimos Tabla a Dataset Incapacidades
                    dsnomina = OrigenDatos.AñadeTablaDataSet(dsnomina, incapacidad, "incapacidad");
                    //Añadimos Tabla a Dataset Horas Extras
                    dsnomina = OrigenDatos.AñadeTablaDataSet(dsnomina, horasextra, "horasextra");


                    XElement xmlDocumentComplementoNomina = XElement.Parse(xmlDocumentoReciboN.InnerXml);
                    /**** Complemento ****/
                    if (Validacion.ValidaOrigenDatos(dsnomina, "nomina"))
                    {
                        //Recorriendo Registros de Tabla
                        foreach (DataRow r in dsnomina.Tables["nomina"].Rows)

                            //Recuperando atributos de registro
                            xmlDocumentComplementoNomina.Add(creaAtributosElemento(r));
                    }
                    /**** Percepciones ****/
                    //Obtenemos Esquema por Default de Percepcionea
                    foreach (DataRow per in dsnomina.Tables["percepciones"].Rows)
                    {
                        xmlDocumentComplementoNomina.Element(ns_sat_nomina + "Percepciones").Add(creaAtributosElemento(per));

                        //Si se añadió correctamente
                        if (xmlDocumentComplementoNomina.Element(ns_sat_nomina + "Percepciones") != null)
                        {
                            //Validamos que exista atributo Total Excento
                            if (per.Field<decimal>("TotalExento") == 0)
                            {
                                //Creamos Atributo Importe Exento
                                XAttribute xAtributte = new XAttribute("TotalExento", "0.00");

                                xmlDocumentComplementoNomina.Element(ns_sat_nomina + "Percepciones").Add(xAtributte);
                            }
                            //Validamos que exista atributo Total Gravado
                            if (per.Field<decimal>("TotalGravado") == 0)
                            {
                                //Creamos Atributo Importe Exento
                                XAttribute xAtributte = new XAttribute("TotalGravado", "0.00");

                                xmlDocumentComplementoNomina.Element(ns_sat_nomina + "Percepciones").Add(xAtributte);
                            }

                            //rrecorremos cada uno de las Percepciones
                            foreach (DataRow r in dsnomina.Tables["percepcion"].Rows)
                            {
                                //Crear elemento Percepción
                                XElement xmlElemetPerce = new XElement(ns_sat_nomina + "Percepcion");

                                //Limpiamos Elemento
                                xmlElemetPerce.RemoveAttributes();

                                //Validamos que exista atributo
                                if (r.Field<decimal>("ImporteExento") == 0)
                                {
                                    //Creamos Atributo Importe Exento
                                    XAttribute xAtributte = new XAttribute("ImporteExento", "0.00");

                                    xmlElemetPerce.ReplaceAttributes(xAtributte);
                                }
                                //Validamos que exista atributo
                                if (r.Field<decimal>("ImporteGravado") == 0)
                                {
                                    //Creamos Atributo Importe Exento
                                    XAttribute xAtributte = new XAttribute("ImporteGravado", "0.00");

                                    xmlElemetPerce.ReplaceAttributes(xAtributte);
                                }
                                //Creamos atributo Percepcion  con atributo Importe Exento
                                xmlElemetPerce.Add(creaAtributosElemento(r));

                                //Añadimos Percepcion
                                xmlDocumentComplementoNomina.Element(ns_sat_nomina + "Percepciones").Add(xmlElemetPerce);

                            }
                        }
                    }
                    /**** Deducciones ****/
                    //Validamos que existaDeducciones
                    if (Validacion.ValidaOrigenDatos(dsnomina.Tables["deducciones"]))
                    {
                        //Obtenemos Esquema por Default de Deducciones
                        foreach (DataRow ded in dsnomina.Tables["deducciones"].Rows)
                        {
                            xmlDocumentComplementoNomina.Element(ns_sat_nomina + "Deducciones").Add(creaAtributosElemento(ded));

                            //Si se añadió correctamente
                            if (xmlDocumentComplementoNomina.Element(ns_sat_nomina + "Deducciones") != null)
                            {
                                //Validamos que exista atributo Total Excento
                                if (ded.Field<decimal>("TotalExento") == 0)
                                {
                                    //Creamos Atributo Importe Exento
                                    XAttribute xAtributte = new XAttribute("TotalExento", "0.00");

                                    xmlDocumentComplementoNomina.Element(ns_sat_nomina + "Deducciones").Add(xAtributte);
                                }
                                //Validamos que exista atributo Total Gravado
                                if (ded.Field<decimal>("TotalGravado") == 0)
                                {
                                    //Creamos Atributo Importe Exento
                                    XAttribute xAtributte = new XAttribute("TotalGravado", "0.00");

                                    xmlDocumentComplementoNomina.Element(ns_sat_nomina + "Deducciones").Add(xAtributte);
                                }
                                //rrecorremos cada uno de las Deducciones
                                foreach (DataRow r in dsnomina.Tables["deduccion"].Rows)
                                {

                                    //Crear elemento Deducción
                                    XElement xmlElemetDeduc = new XElement(ns_sat_nomina + "Deduccion");

                                    //Limpiamos Elemento
                                    xmlElemetDeduc.RemoveAttributes();

                                    //Validamos que exista atributo
                                    if (r.Field<decimal>("ImporteExento") == 0)
                                    {
                                        //Creamos Atributo Importe Exento
                                        XAttribute xAtributte = new XAttribute("ImporteExento", "0.000000");

                                        xmlElemetDeduc.ReplaceAttributes(xAtributte);
                                    }
                                    //Validamos que exista atributo
                                    if (r.Field<decimal>("ImporteGravado") == 0)
                                    {
                                        //Creamos Atributo Importe Exento
                                        XAttribute xAtributte = new XAttribute("ImporteGravado", "0.000000");

                                        xmlElemetDeduc.ReplaceAttributes(xAtributte);
                                    }
                                    //Añadimos atributos al Elemento Deduccion con atributo Importe Exento
                                    xmlElemetDeduc.Add(creaAtributosElemento(r));

                                    //añadimos elemento Deducciones
                                    xmlDocumentComplementoNomina.Element(ns_sat_nomina + "Deducciones").Add(xmlElemetDeduc);


                                }

                            }
                        }
                    }
                    else
                    {
                        //Eliminamos Nodo Deduccion
                        xmlDocumentComplementoNomina.Element(ns_sat_nomina + "Deducciones").Remove();
                    }
                    /**** Incapacidad ****/
                    //Validamos existencia de Incapacidades
                    if (Validacion.ValidaOrigenDatos(dsnomina.Tables["incapacidad"]))
                    {
                        //Añadimos Elemento Incapacidad
                        xmlDocumentComplementoNomina.Add(new XElement(ns_sat_nomina + "Incapacidades"));

                        //Si se añadió correctamente
                        if (xmlDocumentComplementoNomina.Element(ns_sat_nomina + "Incapacidades") != null)
                        {
                            //Obtenemos Esquema por Default de Incapacidades
                            foreach (DataRow ded in dsnomina.Tables["incapacidad"].Rows)
                            {
                                //Crear elemento Incapacidad
                                XElement xmlElemetIncapacidad = new XElement(ns_sat_nomina + "Incapacidad");

                                //Limpiamos Elemento
                                xmlElemetIncapacidad.RemoveAttributes();

                                //Añadimos atributos al Elemento Incapacidqd
                                xmlElemetIncapacidad.Add(creaAtributosElemento(ded));
                                //Añadimos Elemento Incapacidad
                                xmlDocumentComplementoNomina.Element(ns_sat_nomina + "Incapacidades").Add(xmlElemetIncapacidad);

                            }
                        }
                    }
                    /**** Horas Extra ****/
                    //Validamos existencia de  Horas Extra
                    if (Validacion.ValidaOrigenDatos(dsnomina.Tables["horasextra"]))
                    {
                        //Añadimos Elemento  Horas Extra
                        xmlDocumentComplementoNomina.Add(new XElement(ns_sat_nomina + "HorasExtras"));

                        //Si se añadió correctamente
                        if (xmlDocumentComplementoNomina.Element(ns_sat_nomina + "HorasExtras") != null)
                        {
                            //Obtenemos Esquema por Default de  Horas Extra
                            foreach (DataRow ded in dsnomina.Tables["horasextra"].Rows)
                            {
                                //Crear elemento Incapacidad
                                XElement xmlElemethe = new XElement(ns_sat_nomina + "HorasExtra");

                                //Limpiamos Elemento
                                xmlElemethe.RemoveAttributes();

                                //Añadimos atributos al Elemento  Horas Extra
                                xmlElemethe.Add(creaAtributosElemento(ded));
                                //Añadimos Elemento  Horas Extra
                                xmlDocumentComplementoNomina.Element(ns_sat_nomina + "HorasExtras").Add(xmlElemethe);

                            }
                        }
                    }
                    //Cargamos Validación  de Adenda
                    using (SAT_CL.FacturacionElectronica.Addenda addenda = new SAT_CL.FacturacionElectronica.Addenda(objAddendaEmisor.id_addenda))
                    {
                        //Validamos que exista Addenda
                        if (addenda.id_addenda > 0)
                        {
                            //Creamos XMLDOCUMENT para el complemento de nomina 
                            XmlDocument xml_document_complento = new XmlDocument();

                            //Añadimos elemento generado del xmlDocument 
                            xml_document_complento.Load(xmlDocumentComplementoNomina.CreateReader());

                            //Cargando Esquema de Validacion del XML(XSD) dada una Addenda
                            XmlDocument xml_addenda_validacion = addenda.xsd_validation;

                            //Obteniendo NameSpace
                            string ns = xml_addenda_validacion.DocumentElement.GetAttribute("targetNamespace"), msn;

                            //Declarando variable de Validacion
                            bool validacion_xml;

                            //Obteniendo Resultado de la Validacion
                            validacion_xml = TSDK.Base.Xml.ValidaXMLSchema(xml_document_complento.InnerXml, xml_addenda_validacion.InnerXml, ns, out msn);

                            //Validando si fue correcta la Operacion
                            if (validacion_xml)

                                //Insertamos Complemento
                                resultado = SAT_CL.FacturacionElectronica.AddendaComprobante.IngresarAddendaComprobante(
                                            objAddendaEmisor.id_emisor_addenda, id_comprobante, 0,
                                            xml_document_complento, validacion_xml, id_usuario);
                            else
                                //Instanciando Excepción
                                resultado = new RetornoOperacion(msn);
                        }
                        else
                            //Mostrando Error
                            resultado = new RetornoOperacion("No se puede encontrar esqueleto para validación de XSD");
                    }

                }
                else
                    //Mostrando Error
                    resultado = new RetornoOperacion("No se puede encontrar esqueleto para el armado de  XML");
            }
        }

        #endregion

        #region Métodos Complemento Recepción de Pagos

        /// <summary>
        /// Crea la estructura del CFDI de Comprobante de Recepción de Pagos
        /// </summary>
        /// <param name="id_comprobante"></param>
        /// <param name="ns_sat"></param>
        /// <param name="nsn"></param>
        /// <param name="ns_w3c"></param>
        /// <param name="ns_wrp10"></param>
        /// <param name="schema_location"></param>
        /// <param name="id_emisor"></param>
        /// <param name="id_receptor"></param>
        /// <param name="id_usuario"></param>
        /// <param name="resultado"></param>
        /// <returns></returns>
        public static XElement CargaElementosArmadoComprobanteRecepcionPagos_V1_0(int id_comprobante, int id_usuario, out RetornoOperacion resultado)
        {
            //Declaramos Resultado Complemento
            resultado = new RetornoOperacion();

            //Definiendo objeto raíz del CFDI
            XElement cfdi3_3 = new XElement(NS_CFDI_33 + "Comprobante", new XAttribute(XNamespace.Xmlns + NS_PAGO_10_PREFIX, NS_PAGO_10.ToString()),
                                                new XAttribute(XNamespace.Xmlns + "xsi", NS_W3C.ToString()),
                                                new XAttribute(XNamespace.Xmlns + NS_CFDI_33_PREFIX, NS_CFDI_33.ToString()),
                                                new XAttribute(NS_W3C + "schemaLocation", string.Format("{0} {1}", SCHEMALOCATION_CFDI_33 , SCHEMALOCATION_PAGO_10)));

            //Definiendo parametros de consulta
            object[] param = { 3, id_comprobante, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };

            //Ejecutando Stored Procedure
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Si el origen de datos es válido
                if (Validacion.ValidaOrigenDatos(ds, true))
                {
                    /**** Comprobante ****/
                    if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    {
                        //Recorriendo Registros de Tabla
                        foreach (DataRow r in ds.Tables["Table"].Rows)
                            //Recuperando atributos de registro
                            cfdi3_3.Add(creaAtributosElemento(r));
                    }

                    /****** AÑADIENDO ELEMENTOS EN 0 QUE TIENEN CARACTER OBLIGATORIO ******/
                    cfdi3_3.Add(new XAttribute("Total", 0));
                    cfdi3_3.Add(new XAttribute("SubTotal", 0));

                    /**** CFDI's Relacionados ****/
                    //Validando Datos del Encabezado
                    if (Validacion.ValidaOrigenDatos(ds, "Table1"))
                    {
                        //Obteniendo Tipos de Relación
                        List<string> tipos_relacion = (from DataRow dr in ds.Tables["Table1"].Rows
                                                       select dr.Field<string>("TipoRelacion")).Distinct().ToList();

                        //Recorriendo Tipos de Relación
                        foreach (string tipo in tipos_relacion)
                        {
                            //Obteniendo Comprobantes que el Tipo de relación del Ciclo
                            List<string> uuids = (from DataRow dr in ds.Tables["Table1"].Rows
                                                  where dr.Field<string>("TipoRelacion").Equals(tipo)
                                                  select dr.Field<string>("UUID")).Distinct().ToList();

                            //Creando Elemento Principal de los CFDI's Relacionados
                            cfdi3_3.Add(new XElement(NS_CFDI_33 + "CfdiRelacionados", new XAttribute("TipoRelacion", tipo)));

                            //Recorriendo UUID's de los Comprobantes
                            foreach (string uuid in uuids)
                                //Creando Nodos relacionados del CFDI
                                cfdi3_3.Element(NS_CFDI_33 + "CfdiRelacionados").Add(new XElement(NS_CFDI_33 + "CfdiRelacionado", new XAttribute("UUID", uuid)));
                        }
                    }

                    /**** Emisor ****/
                    cfdi3_3.Add(creaElementoGenericoComprobante(ds, "Table2", NS_CFDI_33 + "Emisor"));

                    /**** Receptor ****/
                    cfdi3_3.Add(creaElementoGenericoComprobante(ds, "Table3", NS_CFDI_33 + "Receptor"));
                    
                    /**** Conceptos ****/
                    //Validando Datos
                    if (Validacion.ValidaOrigenDatos(ds, "Table4"))
                    {
                        //Creando Nodo de Conceptos
                        cfdi3_3.Add(new XElement(NS_CFDI_33 + "Conceptos"));

                        //Recorriendo Atributos
                        foreach (DataRow dr in ds.Tables["Table4"].Rows)
                        {
                            //Obteniendo Concepto Actual del Ciclo
                            int idConcepto = Convert.ToInt32(dr["IdConcepto"]);

                            //Declarando Objeto de Nuevo Concepto
                            XElement concepto = new XElement(NS_CFDI_33 + "Concepto", creaAtributosElemento(OrigenDatos.EliminaColumnaDataRow(dr, "IdConcepto-IdConceptoPadre-CuentaPredial")));

                            /****** AÑADIENDO ELEMENTOS EN 0 QUE TIENEN CARACTER OBLIGATORIO ******/
                            concepto.Add(new XAttribute("Importe", 0));
                            concepto.Add(new XAttribute("ValorUnitario", 0));

                            //Añadiendo Concepto al Comprobante
                            cfdi3_3.Element(NS_CFDI_33 + "Conceptos").Add(concepto);
                        }
                    }

                    //Instanciando comprobante (Para recuperar Emisor y Receptor)
                    using (Comprobante c = new Comprobante(id_comprobante))
                    {
                        /** Complemento de Recepción de Pagos (Emisor del CFDI y Receptor '0' Para Todos los Receptores) **/
                        using (DataTable addendas = SAT_CL.FacturacionElectronica.AddendaEmisor.CargaComplementosRequeridosEmisorReceptor(c.id_compania_emisor, 0, "Comprobante Recepción Pagos 1.0"))
                        {
                            //Si existen addendas configuradas
                            if (Validacion.ValidaOrigenDatos(addendas))
                            {
                                //Añadiendo nodo Complemento
                                cfdi3_3.Add(new XElement(NS_CFDI_33 + "Complemento"));
                                //Añadiendo nodo Pagos (incluyendo namespace del complemento)
                                XElement pagos = XElement.Parse(addendas.Rows[0]["XMLPredeterminado"].ToString());
                                //pagos.Add(new XAttribute(NS_W3C + "schemaLocation", SCHEMALOCATION_PAGO_10));

                                //Generamos complemento de Recepción de Pagos
                                resultado = armadoComplementoRecepcionPagos(id_comprobante, ref pagos);

                                //Si no hay errores de creación de complemento
                                if (resultado.OperacionExitosa)
                                {
                                    //Añadiendo Complemento
                                    cfdi3_3.Element(NS_CFDI_33 + "Complemento").Add(pagos);

                                    //Obteniendo Nodo de Pagos
                                    XNamespace attributo_ns = cfdi3_3.Element(NS_CFDI_33 + "Complemento").Element(NS_PAGO_10 + "Pagos").GetNamespaceOfPrefix(NS_PAGO_10_PREFIX);

                                    //Validando que exista
                                    if (attributo_ns != null)
                                    {
                                        //Eliminando attributo
                                        cfdi3_3.Element(NS_CFDI_33 + "Complemento").Element(NS_PAGO_10 + "Pagos").RemoveAttributes();
                                        //Añadiendo Versión
                                        cfdi3_3.Element(NS_CFDI_33 + "Complemento").Element(NS_PAGO_10 + "Pagos").Add(new XAttribute("Version", VERSION_PAGO_10));
                                    }
                                }
                            }
                            else
                                resultado = new RetornoOperacion("No se ha configurado la emisión de CFDI de Comprobante de Recepción de Pagos.");
                        }
                    }
                }
            }

            //Devolviendo Resultado Obtenido
            return cfdi3_3;
        }
        /// <summary>
        /// Realiza la construcción de la estructura XML del CFDI con Complemento de Recepción de Pagos
        /// </summary>
        /// <param name="id_comprobante">Id de Comprobante de Recepción de Pagos</param>
        /// <param name="complemento_pagos">Complemento de Pagos del CFDI</param>
        /// <param name="resultado">Resultado de generación del complemento</param>
        /// <returns></returns>
        private static RetornoOperacion armadoComplementoRecepcionPagos(int id_comprobante, ref XElement complemento_pagos)
        {
            //Inicializando sin errores
            RetornoOperacion resultado = new RetornoOperacion(id_comprobante);

            //Definiendo parametros de consulta
            object[] param = { 4, id_comprobante, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };

            //Inicializando transacción
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Recuperando información de complemento del comprobante desde ejecución de SP
                using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
                {
                    //Si el origen de datos es válido
                    if (Validacion.ValidaOrigenDatos(ds, true))
                    {
                        //Validando Pagos y Aplicaciones
                        if (Validacion.ValidaOrigenDatos(ds, "Table", "Table1"))
                        {
                            //Creando conjunto de elementos de pago
                            XElement[] arrPagos = creaElementosComprobante(ds.Tables["Table"], NS_PAGO_10 + "Pago");
                            //Si hay elementos
                            if (arrPagos.Count() > 0)
                            {
                                //Para cada elemento
                                foreach (XElement p in arrPagos)
                                {
                                    //Instanciando pago
                                    using (Bancos.EgresoIngreso ingreso = new Bancos.EgresoIngreso(Convert.ToInt32(p.Attribute("ID").Value)))
                                    {
                                        //Si se instanció correctamente
                                        if (ingreso.id_egreso_ingreso > 0)
                                        {
                                            //Filtrando registros documento relacionado
                                            DataRow[] doctos = (from DataRow d in ds.Tables["Table1"].Rows
                                                                where d.Field<int>("ID") == Convert.ToInt32(p.Attribute("ID").Value)
                                                                select d).DefaultIfEmpty().ToArray();

                                            //Si existen documentos
                                            if (doctos.Count(d => d != null) > 0)
                                            {
                                                //Convirtiendo en datatable
                                                DataTable mDoctos = OrigenDatos.ConvierteArregloDataRowADataTable(doctos);
                                                //Eliminando columna ID
                                                OrigenDatos.EliminaColumnasDataTable(mDoctos, "ID");
                                                //Filtrando aplicaciones de pago del pago
                                                XElement[] arrDocRelPago = creaElementosComprobante(mDoctos, NS_PAGO_10 + "DoctoRelacionado", false);

                                                //Por cada elemento, se buscarán Tipos de Cambio en 0 (Ya que se agregaron valores 0 para cubrir requerimiento SAT en ImpSaldoInsoluto)
                                                foreach(XElement drp in arrDocRelPago)
                                                {
                                                    //Verificando si el Importe de TC es 0, se procede a eliminarlo del elemento
                                                    if (Convert.ToDecimal(drp.Attribute("TipoCambioDR").Value) == 0)
                                                        drp.Attribute("TipoCambioDR").Remove();
                                                }

                                                //Si hay elementos devueltos
                                                if (arrDocRelPago.Count() > 0)
                                                    //Si agregan documentos relacionados
                                                    p.Add(arrDocRelPago);
                                                else
                                                    resultado = new RetornoOperacion(string.Format("Error al crear XML de Documentos Relacionados a F.I. '{0}'", ingreso.secuencia_compania));

                                                //Eliminando atributo ID en Pago (Referencia de Filtrado)
                                                p.Attribute("ID").Remove();

                                                //Añadiendo pago a nodo PAGOS
                                                complemento_pagos.Add(p);
                                            }
                                            else
                                                resultado = new RetornoOperacion(string.Format("Error al agregar Documentos Relacionados a F.I. '{0}'", ingreso.secuencia_compania));
                                        }
                                        else
                                            resultado = new RetornoOperacion(string.Format("Error al recuperar F.I. Id: '{0}'", Convert.ToInt32(p.Element(NS_PAGO_10 + "Pago").Attribute("ID").Value)));
                                    }

                                    //Si hay errores
                                    if (!resultado.OperacionExitosa)
                                        //Terminando iteraciones
                                        break;
                                }
                            }
                            //Si no se creo el XML de los pagos
                            else
                                resultado = new RetornoOperacion(string.Format("Error al crear XML de F.I. del CFDI Id: '{0}'", id_comprobante));
                        }
                        //SI no se localizaó información para CFDI de Recepción de Pagos
                        else
                            resultado = new RetornoOperacion(string.Format("No se encontraron F.I. asociadas al CFDI Id: '{0}'", id_comprobante));
                    }
                }

                //Si no hay eerores se confirma transacción
                if (resultado.OperacionExitosa)
                    scope.Complete();
            }

            //Devolviendo resultado
            return resultado;
        }

        #endregion
        
        #endregion
    }
}
