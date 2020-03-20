using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml;
using TSDK.Datos;
using System;
using TSDK.Base;
using SAT_CL.Nomina;

namespace SAT_CL.FacturacionElectronica
{
    /// <summary>
    /// Proporciona los métodos necesarios para la carga del contenido del comprobante XML
    /// </summary>
    public class ComprobanteXML
    {
        #region Atributos

        /// <summary>
        /// Nombre del Stored Procedure utilizado para carga de recursos
        /// </summary>
        private static string _nombre_stored_procedure = "fe.sp_comprobante_xml_tcm";

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Carga los elementos necesarios para generar el XML del comprobante y poder sellarlo
        /// </summary>
        /// <param name="id_comprobante">Id de Comprobante</param>
        /// <param name="ns_sat">Namespace que define el contenido del comprobante para validación del SAT</param>
        /// <param name="ns_w3c">Namespace que se utiliza para definir el contenido del comprobante para validación del W3C</param>
        /// <param name="schema_location">Schema Location predeterminado</param>
        /// <returns></returns>
        public static XElement CargaElementosArmadoComprobante_3_2(int id_comprobante, XNamespace ns_sat, XNamespace ns_w3c, string schema_location)
        {

            //Definiendo objeto de retorno
            XElement e_comprobante = new XElement(ns_sat + "Comprobante",
                                                new XAttribute(XNamespace.Xmlns + "xsi", ns_w3c.ToString()),
                                                new XAttribute(XNamespace.Xmlns + CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Prefijo Namespace SAT", 0), ns_sat.ToString()),
                                                new XAttribute(ns_w3c + "schemaLocation", schema_location));

            //Definiendo parametros de consulta
            object[] param = { 1, id_comprobante, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };

            //Ejecutando Stored Procedure
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Si el origen de datos es válido
                if (Validacion.ValidaOrigenDatos(ds, true))
                {
                    /*-------------------------------------
                    -- Comprobante
                    -------------------------------------*/
                    if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    {
                        //Recorriendo Registros de Tabla
                        foreach (DataRow r in ds.Tables["Table"].Rows)
                            //Recuperando atributos de registro
                            e_comprobante.Add(creaAtributosElemento(r));
                    }
                    /*-------------------------------------
                    -- Emisor
                    -------------------------------------*/
                    e_comprobante.Add(creaElementoGenericoComprobante(ds, "Table1", ns_sat + "Emisor"));
                    //Si se añadió correctamente
                    if (e_comprobante.Element(ns_sat + "Emisor") != null)
                    {
                        //Domicilio Fiscal
                        e_comprobante.Element(ns_sat + "Emisor").Add(creaElementoGenericoComprobante(ds, "Table2", ns_sat + "DomicilioFiscal"));
                        //Expedido en 
                        e_comprobante.Element(ns_sat + "Emisor").Add(creaElementoGenericoComprobante(ds, "Table3", ns_sat + "ExpedidoEn"));
                        //RegimenFiscal
                        e_comprobante.Element(ns_sat + "Emisor").Add(creaElementosComprobante(ds, "Table4", ns_sat + "RegimenFiscal"));
                    }
                    /*-------------------------------------
                    -- Receptor
                    -------------------------------------*/
                    e_comprobante.Add(creaElementoGenericoComprobante(ds, "Table5", ns_sat + "Receptor"));
                    //Si se añadió correctamente
                    if (e_comprobante.Element(ns_sat + "Receptor") != null)
                    {
                        //Domicilio Fiscal
                        e_comprobante.Element(ns_sat + "Receptor").Add(creaElementoGenericoComprobante(ds, "Table6", ns_sat + "Domicilio"));
                    }
                    /*-------------------------------------
                    -- Conceptos
                    -------------------------------------*/
                    if (Validacion.ValidaOrigenDatos(ds, "Table7"))
                    {
                        //Creando elemento principal
                        XElement conceptos = new XElement(ns_sat + "Conceptos");

                        //Filtrando conceptos (Que no sean parte)
                        IEnumerable<DataRow> enum_conceptos = (from DataRow r in ds.Tables["Table7"].Rows
                                                               where r.Field<int>("id_tipo") == 1
                                                               select r).DefaultIfEmpty();

                        //Si se encontraron conceptos padre
                        if (enum_conceptos.Count() > 0)
                        {
                            //Para cada elemento
                            foreach (DataRow c in enum_conceptos)
                            {
                                //Si el concepto existe
                                if (c != null)
                                { 
                                    //Creando elemento padre
                                    XElement padre = new XElement(ns_sat + "Concepto", creaAtributosElemento(OrigenDatos.EliminaColumnaDataRow(c, "id-id_tipo-id_padre")));
                                    //Si se generó correctamente
                                    if (padre != null)
                                    {
                                        //Cargando partes del concepto padre
                                        IEnumerable<DataRow> enum_partes = (from DataRow r in ds.Tables["Table7"].Rows
                                                                            where r.Field<int>("id_tipo") == 2 && r.Field<int>("id_padre") == c.Field<int>("id")
                                                                            select r).DefaultIfEmpty();
                                        //Si existen partes del concepto
                                        if (enum_partes.Count() > 0)
                                        {
                                            //Para cada parte
                                            foreach (DataRow p in enum_partes)
                                            {
                                                //Si la parte existe
                                                if (p != null)
                                                    //Añadiendo parte al concepto padre
                                                    padre.Add(new XElement(ns_sat + "Parte", creaAtributosElemento(OrigenDatos.EliminaColumnaDataRow(p, "id-id_tipo-id_padre"))));
                                            }
                                        }

                                        //TODO:Implementar Información Aduanera en caso de ser requerido

                                        //Añadiendo concepto al elemento que los agrupa
                                        conceptos.Add(padre);
                                    }
                                }
                            }
                        }

                        //Añadiendo conceptos al comprobante
                        e_comprobante.Add(conceptos);
                    }
                    /*-------------------------------------
                    -- Impuestos
                    -------------------------------------*/
                    if (Validacion.ValidaOrigenDatos(ds, "Table8"))
                    {
                        //Creando elemento principal
                        e_comprobante.Add(creaElementoGenericoComprobante(ds, "Table8", ns_sat + "Impuestos"));

                        //Si se añadió correctamente
                        if (e_comprobante.Element(ns_sat + "Impuestos") != null)
                        {
                            //Si hay impuestos retenidos
                            if (Validacion.ValidaOrigenDatos(ds, "Table9"))
                            {
                                //Añadiendo Retenciones
                                e_comprobante.Element(ns_sat + "Impuestos").Add(new XElement(ns_sat + "Retenciones"));
                                e_comprobante.Element(ns_sat + "Impuestos").Element(ns_sat + "Retenciones").Add(creaElementosComprobante(ds, "Table9", ns_sat + "Retencion"));
                            }
                            //Si hay impuestos transladados
                            if (Validacion.ValidaOrigenDatos(ds, "Table10"))
                            {
                                //Añadiendo Traslados
                                e_comprobante.Element(ns_sat + "Impuestos").Add(new XElement(ns_sat + "Traslados"));
                                e_comprobante.Element(ns_sat + "Impuestos").Element(ns_sat + "Traslados").Add(creaElementosComprobante(ds, "Table10", ns_sat + "Traslado"));
                            }
                        }
                    }
                    /*-------------------------------------
                    -- Complemento
                    -------------------------------------*/

                    e_comprobante.Add(new XElement(ns_sat + "Complemento"));
                }
            }

            //Devolviendo elemento comprobante
            return e_comprobante;
        }



        public static XElement CargaElementosArmadoComprobante_3_2(int id_comprobante, XNamespace ns_sat, XNamespace ns_w3c, string schema_location, int id_emisor, int id_receptor)
        {
            //Definiendo objeto de retorno
            XElement e_comprobante = new XElement(ns_sat + "Comprobante",
                                                new XAttribute(XNamespace.Xmlns + "xsi", ns_w3c.ToString()),
                                                new XAttribute(XNamespace.Xmlns + CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Prefijo Namespace SAT", 0), ns_sat.ToString()),
                                                new XAttribute(ns_w3c + "schemaLocation", schema_location));

            //Definiendo parametros de consulta
            object[] param = { 1, id_comprobante, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };

            //Ejecutando Stored Procedure
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Si el origen de datos es válido
                if (Validacion.ValidaOrigenDatos(ds, true))
                {
                    /*-------------------------------------
                    -- Comprobante
                    -------------------------------------*/
                    if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    {
                        //Recorriendo Registros de Tabla
                        foreach (DataRow r in ds.Tables["Table"].Rows)
                            //Recuperando atributos de registro
                            e_comprobante.Add(creaAtributosElemento(r));
                    }
                    /*-------------------------------------
                    -- Emisor
                    -------------------------------------*/
                    e_comprobante.Add(creaElementoGenericoComprobante(ds, "Table1", ns_sat + "Emisor"));
                    //Si se añadió correctamente
                    if (e_comprobante.Element(ns_sat + "Emisor") != null)
                    {
                        //Domicilio Fiscal
                        e_comprobante.Element(ns_sat + "Emisor").Add(creaElementoGenericoComprobante(ds, "Table2", ns_sat + "DomicilioFiscal"));
                        //Expedido en 
                        e_comprobante.Element(ns_sat + "Emisor").Add(creaElementoGenericoComprobante(ds, "Table3", ns_sat + "ExpedidoEn"));
                        //RegimenFiscal
                        e_comprobante.Element(ns_sat + "Emisor").Add(creaElementosComprobante(ds, "Table4", ns_sat + "RegimenFiscal"));
                    }
                    /*-------------------------------------
                    -- Receptor
                    -------------------------------------*/
                    e_comprobante.Add(creaElementoGenericoComprobante(ds, "Table5", ns_sat + "Receptor"));
                    //Si se añadió correctamente
                    if (e_comprobante.Element(ns_sat + "Receptor") != null)
                    {
                        //Domicilio Fiscal
                        e_comprobante.Element(ns_sat + "Receptor").Add(creaElementoGenericoComprobante(ds, "Table6", ns_sat + "Domicilio"));
                    }
                    /*-------------------------------------
                    -- Conceptos
                    -------------------------------------*/
                    if (Validacion.ValidaOrigenDatos(ds, "Table7"))
                    {
                        //Creando elemento principal
                        XElement conceptos = new XElement(ns_sat + "Conceptos");

                        //Filtrando conceptos (Que no sean parte)
                        IEnumerable<DataRow> enum_conceptos = (from DataRow r in ds.Tables["Table7"].Rows
                                                               where r.Field<int>("id_tipo") == 1
                                                               select r).DefaultIfEmpty();

                        //Si se encontraron conceptos padre
                        if (enum_conceptos.Count() > 0)
                        {
                            //Para cada elemento
                            foreach (DataRow c in enum_conceptos)
                            {
                                //Si el concepto existe
                                if (c != null)
                                {
                                    //Creando elemento padre
                                    XElement padre = new XElement(ns_sat + "Concepto", creaAtributosElemento(OrigenDatos.EliminaColumnaDataRow(c, "id-id_tipo-id_padre")));
                                    //Si se generó correctamente
                                    if (padre != null)
                                    {
                                        //Cargando partes del concepto padre
                                        IEnumerable<DataRow> enum_partes = (from DataRow r in ds.Tables["Table7"].Rows
                                                                            where r.Field<int>("id_tipo") == 2 && r.Field<int>("id_padre") == c.Field<int>("id")
                                                                            select r).DefaultIfEmpty();
                                        //Si existen partes del concepto
                                        if (enum_partes.Count() > 0)
                                        {
                                            //Para cada parte
                                            foreach (DataRow p in enum_partes)
                                            {
                                                //Si la parte existe
                                                if (p != null)
                                                    //Añadiendo parte al concepto padre
                                                    padre.Add(new XElement(ns_sat + "Parte", creaAtributosElemento(OrigenDatos.EliminaColumnaDataRow(p, "id-id_tipo-id_padre"))));
                                            }
                                        }

                                        //TODO:Implementar Información Aduanera en caso de ser requerido

                                        //Añadiendo concepto al elemento que los agrupa
                                        conceptos.Add(padre);
                                    }
                                }
                            }
                        }

                        //Añadiendo conceptos al comprobante
                        e_comprobante.Add(conceptos);
                    }
                    /*-------------------------------------
                    -- Impuestos
                    -------------------------------------*/
                    if (Validacion.ValidaOrigenDatos(ds, "Table8"))
                    {
                        //Creando elemento principal
                        e_comprobante.Add(creaElementoGenericoComprobante(ds, "Table8", ns_sat + "Impuestos"));

                        //Si se añadió correctamente
                        if (e_comprobante.Element(ns_sat + "Impuestos") != null)
                        {
                            //Si hay impuestos retenidos
                            if (Validacion.ValidaOrigenDatos(ds, "Table9"))
                            {
                                //Añadiendo Retenciones
                                e_comprobante.Element(ns_sat + "Impuestos").Add(new XElement(ns_sat + "Retenciones"));
                                e_comprobante.Element(ns_sat + "Impuestos").Element(ns_sat + "Retenciones").Add(creaElementosComprobante(ds, "Table9", ns_sat + "Retencion"));
                            }
                            //Si hay impuestos transladados
                            if (Validacion.ValidaOrigenDatos(ds, "Table10"))
                            {
                                //Añadiendo Traslados
                                e_comprobante.Element(ns_sat + "Impuestos").Add(new XElement(ns_sat + "Traslados"));
                                e_comprobante.Element(ns_sat + "Impuestos").Element(ns_sat + "Traslados").Add(creaElementosComprobante(ds, "Table10", ns_sat + "Traslado"));
                            }
                        }
                    }
                    /*-------------------------------------
                    -- Complemento
                    -------------------------------------*/

                    using (DataTable addendas = AddendaEmisor.CargaComplementosRequeridosEmisorReceptor(id_emisor, id_receptor))
                    {
                        //Si existen addendas configuradas
                        if (Validacion.ValidaOrigenDatos(addendas))
                        {
                            e_comprobante.Add(new XElement(ns_sat + "Complemento"));
                        }
                    }

                }
            }

            //Devolviendo elemento comprobante
            return e_comprobante;
        }

        #endregion

        #region Métodos privados

        /// <summary>
        /// Método que transforma las columnas y valores de un DataRow en un arreglo XAttribute.
        /// (Las columnas con valor nulo o vacío no serán consideradas).
        /// </summary>
        /// <param name="fila">Fila que será transformada</param>
        /// <returns></returns>
        private static XAttribute[] creaAtributosElemento(DataRow fila)
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
                            if (Convert.ToInt32(arregloColumnas[indice]) > 0)
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
                        if (arregloColumnas[indice] != null)
                            //Si e valor es mayor a 0
                            if (Convert.ToDecimal(arregloColumnas[indice]) > 0)
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
                            if (Convert.ToDouble(arregloColumnas[indice]) > 0)
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
                            if (Convert.ToSingle(arregloColumnas[indice]) > 0)
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
        /// Método encargado de cargar los elementos necesarios del XML para el Recibo de Nomina
        /// </summary>
        /// <param name="id_comprobante"></param>
        /// <param name="ns_sat"></param>
        /// <param name="ns_sat_nomina"></param>
        /// <param name="ns_w3c"></param>
        /// <param name="schema_location"></param>
        /// <param name="id_emisor"></param>
        /// <param name="id_receptor"></param>
        /// <param name="nomina"></param>
        /// <param name="percepion"></param>
        /// <param name="percepciones"></param>
        /// <param name="deduccion"></param>
        /// <param name="deducciones"></param>
        /// <param name="incapacidad"></param>
        /// <param name="horasextra"></param>
        /// <param name="id_usuario"></param>
        /// <param name="resultado"></param>
        /// <returns></returns>
        public static XElement CargaElementosArmadoReciboNomina_3_2(int id_comprobante, XNamespace ns_sat, XNamespace ns_sat_nomina, XNamespace ns_w3c, string schema_location, int id_emisor, int id_receptor,
                                                                               DataTable nomina, DataTable percepion, DataTable percepciones, DataTable deduccion, DataTable deducciones, 
                                                                   DataTable incapacidad, DataTable horasextra, int id_usuario, out RetornoOperacion resultado)
        {

            //Declaramos Resultado Complemento
            resultado = new RetornoOperacion();

            //Definiendo objeto de retorno
            XElement e_comprobante = new XElement(ns_sat + "Comprobante",
                                                new XAttribute(XNamespace.Xmlns + "xsi", ns_w3c.ToString()),
                                                new XAttribute(XNamespace.Xmlns + CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Prefijo Namespace SAT", 0), ns_sat.ToString()),
                                                new XAttribute(ns_w3c + "schemaLocation", schema_location));

            //Definiendo parametros de consulta
            object[] param = { 2, id_comprobante, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };

            //Ejecutando Stored Procedure
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Si el origen de datos es válido
                if (Validacion.ValidaOrigenDatos(ds, true))
                {
                    /*-------------------------------------
                    -- Comprobante
                    -------------------------------------*/
                    if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    {
                        //Recorriendo Registros de Tabla
                        foreach (DataRow r in ds.Tables["Table"].Rows)
                            //Recuperando atributos de registro
                            e_comprobante.Add(creaAtributosElemento(r));
                    }
                    /*-------------------------------------
                    -- Emisor
                    -------------------------------------*/
                    e_comprobante.Add(creaElementoGenericoComprobante(ds, "Table1", ns_sat + "Emisor"));
                    //Si se añadió correctamente
                    if (e_comprobante.Element(ns_sat + "Emisor") != null)
                    {
                        //Domicilio Fiscal
                        e_comprobante.Element(ns_sat + "Emisor").Add(creaElementoGenericoComprobante(ds, "Table2", ns_sat + "DomicilioFiscal"));
                        //Expedido en 
                        e_comprobante.Element(ns_sat + "Emisor").Add(creaElementoGenericoComprobante(ds, "Table3", ns_sat + "ExpedidoEn"));
                        //RegimenFiscal
                        e_comprobante.Element(ns_sat + "Emisor").Add(creaElementosComprobante(ds, "Table4", ns_sat + "RegimenFiscal"));
                    }
                    /*-------------------------------------
                    -- Receptor
                    -------------------------------------*/
                    e_comprobante.Add(creaElementoGenericoComprobante(ds, "Table5", ns_sat + "Receptor"));
                    //Si se añadió correctamente
                    if (e_comprobante.Element(ns_sat + "Receptor") != null)
                    {
                        //Domicilio Fiscal
                        e_comprobante.Element(ns_sat + "Receptor").Add(creaElementoGenericoComprobante(ds, "Table6", ns_sat + "Domicilio"));
                    }
                    /*-------------------------------------
                    -- Conceptos
                    -------------------------------------*/
                    if (Validacion.ValidaOrigenDatos(ds, "Table7"))
                    {
                        //Creando elemento principal
                        XElement conceptos = new XElement(ns_sat + "Conceptos");

                        //Filtrando conceptos (Que no sean parte)
                        IEnumerable<DataRow> enum_conceptos = (from DataRow r in ds.Tables["Table7"].Rows
                                                               where r.Field<int>("id_tipo") == 1
                                                               select r).DefaultIfEmpty();

                        //Si se encontraron conceptos padre
                        if (enum_conceptos.Count() > 0)
                        {
                            //Para cada elemento
                            foreach (DataRow c in enum_conceptos)
                            {
                                //Si el concepto existe
                                if (c != null)
                                {
                                    //Creando elemento padre
                                    XElement padre = new XElement(ns_sat + "Concepto", creaAtributosElemento(OrigenDatos.EliminaColumnaDataRow(c, "id-id_tipo-id_padre")));
                                    //Si se generó correctamente
                                    if (padre != null)
                                    {
                                        //Cargando partes del concepto padre
                                        IEnumerable<DataRow> enum_partes = (from DataRow r in ds.Tables["Table7"].Rows
                                                                            where r.Field<int>("id_tipo") == 2 && r.Field<int>("id_padre") == c.Field<int>("id")
                                                                            select r).DefaultIfEmpty();
                                        //Si existen partes del concepto
                                        if (enum_partes.Count() > 0)
                                        {
                                            //Para cada parte
                                            foreach (DataRow p in enum_partes)
                                            {
                                                //Si la parte existe
                                                if (p != null)
                                                    //Añadiendo parte al concepto padre
                                                    padre.Add(new XElement(ns_sat + "Parte", creaAtributosElemento(OrigenDatos.EliminaColumnaDataRow(p, "id-id_tipo-id_padre"))));
                                            }
                                        }

                                        //TODO:Implementar Información Aduanera en caso de ser requerido

                                        //Añadiendo concepto al elemento que los agrupa
                                        conceptos.Add(padre);
                                    }
                                }
                            }
                        }

                        //Añadiendo conceptos al comprobante
                        e_comprobante.Add(conceptos);
                    }
                    /*-------------------------------------
                    -- Impuestos
                    -------------------------------------*/
                    if (Validacion.ValidaOrigenDatos(ds, "Table8"))
                    {
                        //Creando elemento principal
                        e_comprobante.Add(creaElementoGenericoComprobante(ds, "Table8", ns_sat + "Impuestos"));

                        //Si se añadió correctamente
                        if (e_comprobante.Element(ns_sat + "Impuestos") != null)
                        {
                            //Si hay impuestos retenidos
                            if (Validacion.ValidaOrigenDatos(ds, "Table9"))
                            {
                                //Añadiendo Retenciones
                                e_comprobante.Element(ns_sat + "Impuestos").Add(new XElement(ns_sat + "Retenciones"));
                                e_comprobante.Element(ns_sat + "Impuestos").Element(ns_sat + "Retenciones").Add(creaElementosComprobante(ds, "Table9", ns_sat + "Retencion"));
                            }
                            //Si hay impuestos transladados
                            if (Validacion.ValidaOrigenDatos(ds, "Table10"))
                            {
                                //Añadiendo Traslados
                                e_comprobante.Element(ns_sat + "Impuestos").Add(new XElement(ns_sat + "Traslados"));
                                e_comprobante.Element(ns_sat + "Impuestos").Element(ns_sat + "Traslados").Add(creaElementosComprobante(ds, "Table10", ns_sat + "Traslado"));
                            }
                        }
                    }
                    /*-------------------------------------
                    -- Complemento
                    -------------------------------------*/

                    using (DataTable addendas = AddendaEmisor.CargaComplementosRequeridosEmisorReceptor(id_emisor, id_receptor, "Nómina"))
                    {
                        //Si existen addendas configuradas
                        if (Validacion.ValidaOrigenDatos(addendas))
                        {
                            e_comprobante.Add(new XElement(ns_sat + "Complemento"));
                            //Si se añadió correctamente
                            if (e_comprobante.Element(ns_sat + "Complemento") != null)
                            {
                                //Generamos complemento de Nomina
                                complementoNominaXML(id_comprobante, id_emisor, id_receptor, nomina, percepciones, percepion, deducciones, deduccion, incapacidad, horasextra, ns_sat_nomina, id_usuario, out resultado);
                            }

                        }
                    }

                }
            }

            return e_comprobante;
        }

        /// <summary>
        /// Método encargado de cargar los elementos necesarios del XML para el Recibo de Nomina para el Comprobente 3.2
        /// </summary>
        /// <param name="version"></param>
        /// <param name="id_comprobante"></param>
        /// <param name="id_nomina_empleado"></param>
        /// <param name="id_comprobante"></param>
        /// <param name="ns_sat"></param>
        /// <param name="nsn"></param>
        /// <param name="ns_w3c"></param>
        /// <param name="schema_location"></param>
        /// <param name="id_emisor"></param>
        /// <param name="id_receptor"></param>
        /// <param name="id_usuario"></param>
        /// <param name="resultado"></param>
        /// <returns></returns>
        public static XElement CargaElementosArmadoComprobanteReciboNominaActualizacion1_V_3_2(string version, int id_nomina_empleado, int id_comprobante, XNamespace ns_sat, XNamespace nsn, XNamespace ns_w3c,XNamespace ns_wn12, string schema_location, int id_emisor, int id_receptor,
                                                                               int id_usuario, out RetornoOperacion resultado)
        {

            //Declaramos Resultado Complemento
            resultado = new RetornoOperacion();

            //Definiendo objeto de retorno
            XElement e_comprobante = new XElement(ns_sat + "Comprobante",
                                               new XAttribute(XNamespace.Xmlns + "xsi", ns_w3c.ToString()),
                                                new XAttribute(XNamespace.Xmlns + CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Prefijo Namespace SAT", 0), ns_sat.ToString()),
                                                new XAttribute(ns_w3c + "schemaLocation", schema_location));


            //Definiendo parametros de consulta
            object[] param = { 3, id_comprobante, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };

            //Ejecutando Stored Procedure
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Si el origen de datos es válido
                if (Validacion.ValidaOrigenDatos(ds, true))
                {
                    /*-------------------------------------
                    -- Comprobante
                    -------------------------------------*/
                    if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    {
                        //Recorriendo Registros de Tabla
                        foreach (DataRow r in ds.Tables["Table"].Rows)
                            //Recuperando atributos de registro
                            e_comprobante.Add(creaAtributosElemento(r));
                    }
                    /*-------------------------------------
                    -- Emisor
                    -------------------------------------*/
                    e_comprobante.Add(creaElementoGenericoComprobante(ds, "Table1", ns_sat + "Emisor"));
                    //Si se añadió correctamente
                    if (e_comprobante.Element(ns_sat + "Emisor") != null)
                    {
                        //RegimenFiscal
                        e_comprobante.Element(ns_sat + "Emisor").Add(creaElementosComprobante(ds, "Table4", ns_sat + "RegimenFiscal"));
                    }
                    /*-------------------------------------
                    -- Receptor
                    -------------------------------------*/
                    e_comprobante.Add(creaElementoGenericoComprobante(ds, "Table5", ns_sat + "Receptor"));
                    /*-------------------------------------
                    -- Conceptos
                    -------------------------------------*/
                    if (Validacion.ValidaOrigenDatos(ds, "Table7"))
                    {
                        //Creando elemento principal
                        XElement conceptos = new XElement(ns_sat + "Conceptos");

                        //Filtrando conceptos (Que no sean parte)
                        IEnumerable<DataRow> enum_conceptos = (from DataRow r in ds.Tables["Table7"].Rows
                                                               where r.Field<int>("id_tipo") == 1
                                                               select r).DefaultIfEmpty();

                        //Si se encontraron conceptos padre
                        if (enum_conceptos.Count() > 0)
                        {
                            //Para cada elemento
                            foreach (DataRow c in enum_conceptos)
                            {
                                //Si el concepto existe
                                if (c != null)
                                {
                                    //Creando elemento padre
                                    XElement padre = new XElement(ns_sat + "Concepto", creaAtributosElemento(OrigenDatos.EliminaColumnaDataRow(c, "id-id_tipo-id_padre")));
                                    //Si se generó correctamente
                                    if (padre != null)
                                    {
                                        //Cargando partes del concepto padre
                                        IEnumerable<DataRow> enum_partes = (from DataRow r in ds.Tables["Table7"].Rows
                                                                            where r.Field<int>("id_tipo") == 2 && r.Field<int>("id_padre") == c.Field<int>("id")
                                                                            select r).DefaultIfEmpty();
                                        //Si existen partes del concepto
                                        if (enum_partes.Count() > 0)
                                        {
                                            //Para cada parte
                                            foreach (DataRow p in enum_partes)
                                            {
                                                //Si la parte existe
                                                if (p != null)
                                                    //Añadiendo parte al concepto padre
                                                    padre.Add(new XElement(ns_sat + "Parte", creaAtributosElemento(OrigenDatos.EliminaColumnaDataRow(p, "id-id_tipo-id_padre"))));
                                            }
                                        }

                                        //TODO:Implementar Información Aduanera en caso de ser requerido

                                        //Añadiendo concepto al elemento que los agrupa
                                        conceptos.Add(padre);
                                    }
                                }
                            }
                        }

                        //Añadiendo conceptos al comprobante
                        e_comprobante.Add(conceptos);
                    }
                    /*-------------------------------------
                    -- Impuestos
                    -------------------------------------*/
                    if (Validacion.ValidaOrigenDatos(ds, "Table8"))
                    {
                        //Creando elemento principal
                        e_comprobante.Add(creaElementoGenericoComprobante(ds, "Table8", ns_sat + "Impuestos"));

                        //Si se añadió correctamente
                        if (e_comprobante.Element(ns_sat + "Impuestos") != null)
                        {
                            //Si hay impuestos retenidos
                            if (Validacion.ValidaOrigenDatos(ds, "Table9"))
                            {
                                //Añadiendo Retenciones
                                e_comprobante.Element(ns_sat + "Impuestos").Add(new XElement(ns_sat + "Retenciones"));
                                e_comprobante.Element(ns_sat + "Impuestos").Element(ns_sat + "Retenciones").Add(creaElementosComprobante(ds, "Table9", ns_sat + "Retencion"));
                            }
                            //Si hay impuestos transladados
                            if (Validacion.ValidaOrigenDatos(ds, "Table10"))
                            {
                                //Añadiendo Traslados
                                e_comprobante.Element(ns_sat + "Impuestos").Add(new XElement(ns_sat + "Traslados"));
                                e_comprobante.Element(ns_sat + "Impuestos").Element(ns_sat + "Traslados").Add(creaElementosComprobante(ds, "Table10", ns_sat + "Traslado"));
                            }
                        }
                    }
                    /*-------------------------------------
                    -- Complemento
                    -------------------------------------*/

                    using (DataTable addendas = AddendaEmisor.CargaComplementosRequeridosEmisorReceptor(id_emisor, id_receptor,"Nómina "+ version))
                    {
                        //Si existen addendas configuradas
                        if (Validacion.ValidaOrigenDatos(addendas))
                        {
                            e_comprobante.Add(new XElement(ns_sat + "Complemento"));
                            //Si se añadió correctamente
                            if (e_comprobante.Element(ns_sat + "Complemento") != null)
                            {
                  

                                //Generamos complemento de Nomina
                                ArmadoComplementoNomina(version, id_comprobante,nsn,  id_nomina_empleado, id_emisor, id_usuario, out resultado);
                            }

                        }
                    }

                }
            }

            return e_comprobante;
        }

        /// <summary>
        /// Arma el complemento de Nómina 
        /// </summary>
        /// <param name="version"></param>
        /// <param name="id_comprobante"></param>
        /// <param name="nsn"></param>
        /// <param name="id_nomina_empleado"></param>
        /// <param name="id_emisor"></param>
        /// <param name="id_usuario"></param>
        /// <param name="resultado"></param>
        /// <returns></returns>
        public static XElement ArmadoComplementoNomina(string version, int id_comprobante, XNamespace nsn, int id_nomina_empleado, int id_emisor, int id_usuario,  out RetornoOperacion resultado)
        {

            //Declaramos Resultado Complemento
            resultado = new RetornoOperacion();
           RetornoOperacion resultadoatributos = new RetornoOperacion();
            XElement xmlDocumentComplementoNomina =null;

            //Cargamos Complemeto Nomina  Esqueleto  
            using (AddendaEmisor objAddendaEmisor = new AddendaEmisor(id_emisor, 0,"Nómina " + version))
            {
                //Validamos Esqueleto de Adenda 
                if (objAddendaEmisor.id_emisor_addenda > 0)
                {
                    //Cargamos XML Predeterminado
                     xmlDocumentComplementoNomina = XElement.Parse(objAddendaEmisor.xml_predeterminado.InnerXml);

                    //Obtenemos Id de Elemento Principal
                    int id_elemento_principal = Esquema.ObtieneIdEsquema(version, 1, 0);

                    //Si el origen de datos es válido
                    if (id_elemento_principal != 0)
                    {
                        //Añadimos Atributos Principales
                        resultadoatributos = AñadeAtributosEsquema(version, id_nomina_empleado, xmlDocumentComplementoNomina, id_elemento_principal, 0);



                        //Añadimos Elemntos
                        resultadoatributos = AnadeElementosEsquema(version, nsn, id_nomina_empleado, xmlDocumentComplementoNomina, id_elemento_principal,0);
                    }
                }

                //Cargamos Validación  de Adenda
                using (Addenda addenda = new Addenda(objAddendaEmisor.id_addenda))
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
                        {

                            //Insertamos Complemento
                            resultado = AddendaComprobante.IngresarAddendaComprobante(
                                        objAddendaEmisor.id_emisor_addenda, id_comprobante, 0,
                                        xml_document_complento, validacion_xml, id_usuario);

                        }
                        else
                        {
                            resultado = new RetornoOperacion(msn);
                        }
                    }
                    else
                    {
                        //Mostrando Error
                        resultado = new RetornoOperacion("No se puede encontrar esqueleto para validación de XSD");
                    }
                }
            }

            return xmlDocumentComplementoNomina;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="version"></param>
        /// <param name="nsn"></param>
        /// <param name="id_nomina_empleado"></param>
        /// <param name="resultadoatributos"></param>
        /// <param name="xmlElemnto"></param>
        /// <param name="id_elemento_principal"></param>
        /// <returns></returns>
        private static RetornoOperacion AnadeElementosEsquema(string version, XNamespace nsn, int id_nomina_empleado, XElement xmlElemnto, int id_elemento_principal,int id_esquema_superior)
        {
            //Declaramos objeto Resulatdo
            RetornoOperacion resultado = new RetornoOperacion();
            //Si existen Elemento
            using (DataTable elementos = Esquema.ObtieneElementos(version, id_elemento_principal, id_nomina_empleado))
            {
                //Recorremos cada uno de los Elemntos
                foreach (DataRow r in elementos.Rows)
                {
                    //Creando elemento principal
                    XElement xElemen = new XElement(nsn + r.Field<string>("Columna"));

                    //Validamos que pertenesca el Registro Superior
                    if (r.Field<int>("IdEsquemaSuperior") ==0|| r.Field<int>("IdEsquemaSuperior") == id_esquema_superior)
                    {
                        //Añadimos Atributos Principales
                        resultado = AñadeAtributosEsquema(version, id_nomina_empleado, xElemen, r.Field<int>("IdEsquema"), r.Field<int>("IdEsquemaRegistro"));
                    }
                    else
                        resultado = new RetornoOperacion();
                    //Si no Existen Atributos
                    if (resultado.OperacionExitosa)
                    {
                         //Añadimos Elemento a la Nòmin
                        xmlElemnto.Add(xElemen);

                        //Obtenemos Agrupador
                        int id_agrupador = Esquema.ObtieneIdEsquema(version, 1, r.Field<int>("IdEsquema"));

                        //Validamos Agrupador
                        if (id_agrupador != 0)
                        {

                            resultado = AnadeElementosEsquema(version, nsn, id_nomina_empleado, xElemen, r.Field<int>("IdEsquema"), r.Field<int>("IdEsquemaRegistro"));
                        }
                    }
                    else
                    {
                        //Validamos que Existan Atributos en SubNodos
                        RetornoOperacion resultadonodos = new RetornoOperacion();

                        //Validamos Resultado
                        resultadonodos = Esquema.ObtieneUltimoElemento(version, r.Field<int>("IdEsquema"), id_nomina_empleado, r.Field<int>("IdEsquemaSuperior"));

                        if (resultadonodos.OperacionExitosa)
                        {
                            //Añadimos Elemento a la Nòmin
                            xmlElemnto.Add(xElemen);

                            //Obtenemos Agrupador
                            int id_agrupador = Esquema.ObtieneIdEsquema(version, 1, r.Field<int>("IdEsquema"));

                            //Validamos Agrupador
                            if (id_agrupador != 0)
                            {

                                resultado = AnadeElementosEsquema(version, nsn, id_nomina_empleado, xElemen, r.Field<int>("IdEsquema"), r.Field<int>("IdEsquemaRegistro"));
                            }
                        }
                    }
                  

                }
            }
            return resultado;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="version"></param>
        /// <param name="xmlDocumentComplementoNomina"></param>
        /// <param name="id_elemento_principal"></param>
        private static RetornoOperacion AñadeAtributosEsquema(string version, int id_nomina_empleado, XElement xmlDocumentComplementoNomina, int id_elemento_principal, int id_grupo_superior)
        {
            //Declaramos Variable Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            using (DataTable mit = Esquema.ObtieneAtributos(version, id_elemento_principal,id_nomina_empleado, id_grupo_superior))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(mit))
                {
                    //Asignamos Valor Verdaderp
                    resultado = new RetornoOperacion(0, "", true);

                    //Recorremos cada una de las Filas
                    foreach (DataRow r in mit.Rows)
                    {
                        //Creamos Tabla
                        DataTable atributos = creaTabla(r);
                        //Validamos Origen de Datos
                        if (Validacion.ValidaOrigenDatos(atributos))
                        {
                            //Recorriendo Registros de Tabla
                            foreach (DataRow a in atributos.Rows)
                                //Recuperando atributos de registro
                                xmlDocumentComplementoNomina.Add(creaAtributosElemento(a));
                        }
                    }
                }

            }
            //Devolvemos Valor
            return resultado;
        }


        public static DataTable creaTabla(DataRow r)
        {
            //Declaramos Tabla a Devolver
            DataTable mit1 = new DataTable("atributo");

            //Validamos Origen de Datos
            if (r != null)
            {
               //Instanciamos Esquema
                using (EsquemaRegistro objEsquemaRegistro = new EsquemaRegistro(r.Field<int>("IdEsquemaRegistro")))
                {
                    //Creamos Columna
                    DataColumn columna = mit1.Columns.Add(r.Field<string>("Columna"));
                    mit1.Rows.Add(objEsquemaRegistro.valor);

                }                
            }
            //Devolvemos Valor
            return mit1;
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
                                            DataTable deducciones, DataTable deduccion, DataTable incapacidad, DataTable horasextra, XNamespace  ns_sat_nomina, int id_usuario, out RetornoOperacion resultado)
        {
            //Declaramos objeto Resultado
            resultado = new RetornoOperacion();

            //Cargamos Complemeto Nomina Emisor Esqueleto  
            using (AddendaEmisor objAddendaEmisor = new AddendaEmisor(id_emisor, id_receptor))
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
                    /*-------------------------------------
                   -- Complemento
                   -------------------------------------*/
                    if (Validacion.ValidaOrigenDatos(dsnomina, "nomina"))
                    {
                        //Recorriendo Registros de Tabla
                        foreach (DataRow r in dsnomina.Tables["nomina"].Rows)

                            //Recuperando atributos de registro
                            xmlDocumentComplementoNomina.Add(creaAtributosElemento(r));
                    }
                    /*-------------------------------------
                   -- Percepciones
                   -------------------------------------*/
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
                    /*-------------------------------------
                  -- Deducciones
                  -------------------------------------*/
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
                    /*-------------------------------------
                    -- Incapacidad
                    -------------------------------------*/
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
                    /*-------------------------------------
                    -- Horas Extra
                    -------------------------------------*/
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
                    using (Addenda addenda = new Addenda(objAddendaEmisor.id_addenda))
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
                            {

                                //Insertamos Complemento
                                resultado = AddendaComprobante.IngresarAddendaComprobante(
                                            objAddendaEmisor.id_emisor_addenda, id_comprobante, 0,
                                            xml_document_complento, validacion_xml, id_usuario);

                            }
                            else
                            {
                                resultado = new RetornoOperacion(msn);
                            }
                        }
                        else
                        {
                            //Mostrando Error
                            resultado = new RetornoOperacion("No se puede encontrar esqueleto para validación de XSD");
                        }
                    }

                }
                else
                {
                    resultado = new RetornoOperacion("No se puede encontrar esqueleto para el armado de  XML");
                }
            }

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
            {
                //Para cada registro
                foreach (DataRow r in ds.Tables[tabla].Rows)
                    //Añadiendo el elemento a la lista de resultados
                    elementos.Add(new XElement(nombre_elemento, creaAtributosElemento(r)));
            }

            //Devolviendo elemento creado
            return elementos.ToArray();
        }
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

        #endregion
    }
}
