using SAT_CL;
using FEv32 = SAT_CL.FacturacionElectronica;
using FEv33 = SAT_CL.FacturacionElectronica33;
using System;
using System.Data;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;
using System.Collections.Generic;

namespace SAT.UserControls
{
    public partial class wucAddendaComprobante : System.Web.UI.UserControl
    {
        #region Atributos

        /// <summary>
        /// Id Comprobante
        /// </summary>
        private int _id_comprobante;
        /// <summary>
        /// Id Addenda Comprobante
        /// </summary>
        private int _id_addenda_comprobante;
        /// <summary>
        /// Id Addenda 
        /// </summary>
        private int _id_addenda;
        /// <summary>
        /// Versión del CFDI
        /// </summary>
        private string _version_cfdi;
        /// <summary>
        /// Almacena el Nombre del Contenedor
        /// </summary>
        public string Contenedor;


        #endregion

        #region Eventos
        /// <summary>
        /// Evento que se dispara al Producir un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Validando  PostBack
            if (!this.IsPostBack)
                asignaAtributos();
            else
                //Si es PostaBacks
                recuperaAtributos();

        }
        /// <summary>
        /// Evento producido previo a la carga
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            //Almacenando valores viewstate
            asignaAtributos();
        }
        /// <summary>
        /// Evento que se dispara al Presionar el Boton "Guardar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_OnClick(object sender, EventArgs e)
        {   //Invoca Método de Guardado
            guardaAddenda();
        }
        /// <summary>
        /// Evento que se dispara al dar Click en el Nodo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void trvFormas_OnSelectedNodeChanged(object sender, EventArgs e)
        {
            //Carga el GridView
            cargaGridView();
            Controles.InicializaIndices(gvAtributos);
            //Limpia Mensaje
            lblError.Text = "";
        }
        /// <summary>
        /// Evento producido al enlazar a datos cada nodo del TreeView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void trvFormas_TreeNodeDataBound(object sender, TreeNodeEventArgs e)
        {
            //Realizando pausa de 1 milisegundo para generar una semilla nueva entre cada llamada de cadena aleatoria
            System.Threading.Thread.Sleep(100);
            //Asignando Value especial a cada TreeNode mediante una cadena aleatoria(Para evitar duplicidades con elementos del mismo nombre)
            e.Node.Value = string.Format("{0}-{1}", e.Node.Value, Cadena.CadenaAleatoria(3));
        }
        /// <summary>
        /// Evento que se dispara al Presionar el Boton "Save"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbSave_OnClick(object sender, EventArgs e)
        {   //Evalua si hay un nodo Seleccionado
            if (trvFormas.SelectedNode != null)
            {   //Obteniendo Fila Seleccionada
                Controles.SeleccionaFila(gvAtributos, sender, "lnk", false);
                //Cargango el Valor Nuevo
                using (TextBox txtValorTemp = (TextBox)gvAtributos.SelectedRow.FindControl("txtValor"))
                {   //Cargando el Nombre del Atributo
                    using (Label lblAtributoTemp = (Label)gvAtributos.SelectedRow.FindControl("lblAtributo"))
                    {   //Recuperando 
                        XmlDocument doc = (XmlDocument)Session["XML"];

                        string path = trvFormas.SelectedNode.DataPath;
                        //Creando Nodo donde se obtiene el Nodo Seleccionado junto con su Ruta
                        XmlNode node = doc.SelectSingleNode(path);
                        //Obteniendo Atributo por Nombre
                        XmlAttribute attrib = node.Attributes[lblAtributoTemp.Text];
                        //Evalua si contiene Atributos
                        if (attrib != null)
                        {   //Asignando Nuevo Valor al Atributo
                            attrib.Value = txtValorTemp.Text;
                            //Mostrando Mensaje de Operacion Exitosa
                            lblError.Text = "*" + lblAtributoTemp.Text + "* Actualizado con Exito";
                        }
                        else//Mostrando Mensaje de Error
                            lblError.Text = "No Existen Atributos";
                        //Guardando Archivo XML en Session 
                        Session["XML"] = doc;
                    }
                }
            }
        }

        /// <summary>
        /// Evento que se dispara al Presionar el Boton "Eliminar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEliminar_OnClick(object sender, EventArgs e)
        {
            //Elimina el registro
            deshabilitaAddendaComp();
        }

        /// <summary>
        /// Evento que se dispara al Presionar cualquier "ImageButton"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void click_EdicionNodo(object sender, ImageClickEventArgs e)
        {   //Evalua si hay un nodo Seleccionado
            if (trvFormas.SelectedNode != null)
            {   //Carga Valores (Recarga el Arbol)
                inicializaValores();
                //Cargando Variables del XML
                XmlDocument doc = (XmlDocument)Session["XML"];
                XmlNode node = doc.SelectSingleNode(trvFormas.SelectedNode.DataPath);
                XmlAttributeCollection atrib_coll = node.Attributes;
                //Obteniendo el Valor los Elementos del Boton Presionado
                ImageButton img_btn = (ImageButton)sender;
                //Evaluando Boton
                switch (img_btn.CommandName)
                {
                    case "Quitar":
                        {   //Valida si no es el Nodo Raiz
                            if (node != doc.DocumentElement)
                            {   //Obtiene el Nodo Padre del Nodo Seleccionado
                                XmlNode father = node.ParentNode;
                                //Remueve el Nodo Seleccionado del Nodo Padre
                                father.RemoveChild(node);
                                //Guardando XML en Session
                                Session["XML"] = doc;
                                //Recarga los Valores de la Pagina
                                inicializaPagina();
                                //Mostrando Mensaje
                                lblAvisoNodo.Text = "*" + node.Name + "* Eliminado con Exito";
                            }
                            else
                                lblAvisoNodo.Text = "No puedes Eliminar el Nodo Principal";
                            break;
                        }
                    case "Copiar":
                        {   //Valida si no es el Nodo Raiz
                            if (node != doc.DocumentElement)
                            {   //Obtiene raiz del Documento
                                XmlNode root = node.ParentNode;
                                //Importando Nuevo Nodo
                                XmlNode newNode = doc.ImportNode(node, true);
                                //Insertando Nodo al mismo nivel del Nodo Copiado
                                root.InsertAfter(newNode, node);
                                //root.AppendChild(newNode);
                                //Guardando XML en Session
                                Session["XML"] = doc;
                                //Recarga los Valores de la Pagina
                                inicializaPagina();
                                //Mostrando Mensaje
                                lblAvisoNodo.Text = "*" + node.Name + "* Copiado con Exito";
                            }
                            else
                                lblAvisoNodo.Text = "No puedes Copiar el Nodo Principal";
                            break;
                        }
                }
            }
            else
                lblAvisoNodo.Text = "Debe Seleccionar un Nodo Primero";
        }
        /// <summary>
        /// Evento Disparado al Presionar el Boton de "Guardar Valor"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_OnClick(object sender, EventArgs e)
        {   //Evalua si hay un nodo Seleccionado
            if (trvFormas.SelectedNode != null)
            {   //Cargando Variables del XML
                XmlDocument doc = (XmlDocument)Session["XML"];
                XmlNode node = doc.SelectSingleNode(trvFormas.SelectedNode.DataPath);
                //Evaluando Caracteristicas de los Controles
                if (Elemento.Enabled == false && btnSave.Text == "Editar Valor")
                {   //Habilitando Control TextBox para Guardar
                    Elemento.Enabled = true;
                    btnSave.Text = "Guardar Valor";
                    lblError.Text = "";
                }
                else if (Elemento.Enabled == true && btnSave.Text == "Guardar Valor")
                {   //Valida si el Nodo Seleccionado contiene Hijos
                    if (node.HasChildNodes)
                    {   //Si es un Elemento vacio
                        if (node.FirstChild.NodeType == XmlNodeType.Text)
                            //Reemplazando Valor por el Nuevo ingresado
                            node.FirstChild.Value = Elemento.Text;
                        //Evaluando si el Nodo Hijo es Elemento o Valor
                        else if (node.FirstChild.NodeType == XmlNodeType.Element)
                            //Añadiendo Valor Nuevo al Elemento
                            node.InnerXml = Elemento.Text + node.InnerXml;
                    }
                    else
                        //Si no contiene es un Nodo Final
                        node.InnerXml = Elemento.Text;

                    //Cambiando Controles
                    Elemento.Enabled = false;
                    btnSave.Text = "Editar Valor";
                    lblError.Text = "Elemento *" + node.Name + "* guardado con Exito";
                    //Guardando en Session
                    Session["XML"] = doc;
                }
            }
            else
                lblError.Text = "Debe Seleccionar un Nodo Primero";
        }

        #endregion

        #region Métodos
        /// <summary>
        /// Método encargado de Inicializar la Pagina
        /// </summary>
        private void inicializaPagina()
        {
            //habilita Controles
            habilitaControles();
            //Inicializa Valores
            inicializaValores();
            //Inicializa Etiqueta Error
            lblError.Text = "";
            lblAvisoNodo.Text = "";
            lblNodo.Text = "";
        }

        /// <summary>
        /// Método encargado de Habilitar los Controles
        /// </summary>
        private void habilitaControles()
        {
            //Validamos Id Adddenda Comprobante
            if (this._id_addenda_comprobante == 0)
            {
                //Habilitando Controles
                trvFormas.Enabled = true;
                Elemento.Enabled = false;
                btnGuardar.Enabled = true;
                btnEliminar.Enabled = true;
                btnEliminar.Text = "Eliminar";
                btnSave.Enabled = true;
            }
            else
            {
                //Habilitando Controles
                trvFormas.Enabled = true;
                Elemento.Enabled = false;
                btnGuardar.Enabled = true;
                btnEliminar.Enabled = true;
                btnEliminar.Text = "Eliminar";
                btnSave.Enabled = true;
            }
        }
        /// <summary>
        /// Recupera Atributos
        /// </summary>
        private void recuperaAtributos()
        {
            //Recuperando Atributos
            if (Convert.ToInt32(ViewState["IdComprobante"]) != 0)
                this._id_comprobante = Convert.ToInt32(ViewState["IdComprobante"]);
            //Recuperando Atributos
            if (Convert.ToInt32(ViewState["IdAddenda"]) != 0)
                this._id_addenda = Convert.ToInt32(ViewState["IdAddenda"]);
            //Recuperando Atributos
            if (Convert.ToInt32(ViewState["IdAddendaComprobante"]) != 0)
                this._id_addenda_comprobante = Convert.ToInt32(ViewState["IdAddendaComprobante"]);
            //Recuperando Atributos
            if (ViewState["VersionCFDI"] != null)
                this._version_cfdi = ViewState["VersionCFDI"].ToString();
        }
        /// <summary>
        /// Asigna Atributos
        /// </summary>
        private void asignaAtributos()
        {
            //Asignando Atributos
            ViewState["IdComprobante"] = this._id_comprobante;
            ViewState["IdAddenda"] = this._id_addenda;
            ViewState["IdAddendaComprobante"] = this._id_addenda_comprobante;
            ViewState["VersionCFDI"] = this._version_cfdi;
        }
        /// <summary>
        /// Método encargado de Inicializar Los Valores
        /// </summary>
        private void inicializaValores()
        {
            //Inicializando GridView Vacio
            Controles.InicializaGridview(gvAtributos);

            //Validamos Id Addenda Comprobante
            if (this._id_addenda_comprobante == 0)
            {
                //Validando Contenido de Sesión
                if (Session["XML"] == null)
                {
                    //Validando Versión
                    switch (this._version_cfdi)
                    {
                        case "3.2":
                            //Invocando Mëtodo de Inicialización
                            inicializaAddendaComprobante32();
                            break;
                        case "3.3":
                            //Invocando Mëtodo de Inicialización
                            inicializaAddendaComprobante33();
                            break;
                        default:
                            //Invocando Mëtodo de Inicialización
                            inicializaAddendaComprobante33();
                            break;
                    }
                }
                else
                {   //Almacenando XmlDocument
                    XmlDocument xdoc = (XmlDocument)Session["XML"];
                    xmlds.Data = xdoc.InnerXml;
                    //
                    xmlds.EnableCaching = false;
                    //Referenciando un Origen de Datos
                    trvFormas.DataSourceID = xmlds.ID;
                    //Capturando Archivo XML en variable de Session
                    Session["XML"] = xdoc;
                }
            }
            else
            {   
                //Instanciando Comprobante de Addenda
                using (SAT_CL.FacturacionElectronica.AddendaComprobante add_comp = new SAT_CL.FacturacionElectronica.AddendaComprobante(this._id_addenda_comprobante))
                {
                    //Validando existencia de la Addenda
                    if (add_comp.habilitar)
                    {
                        //Carga XML
                        XmlDocument xml_temp = add_comp.xml_addenda;
                        //Validando que el "id_registro_b" sea distinto del id de Comprobante
                        if (this._id_comprobante != (this._version_cfdi.Equals("3.3") ? add_comp.id_comprobante33 : add_comp.id_comprobante))
                            //Si es distinto, da preferencia al valor del Registro almacenado
                            this._id_comprobante = (this._version_cfdi.Equals("3.3") ? add_comp.id_comprobante33 : add_comp.id_comprobante);
                        

                        //Instanciando Emisor de Addenda
                        using (SAT_CL.FacturacionElectronica.AddendaEmisor add_emi = new SAT_CL.FacturacionElectronica.AddendaEmisor(add_comp.id_emisor_addenda))
                            //Cargando Catalogo Ingresado
                            this._id_addenda = add_emi.id_addenda;
                        
                        //validando que Session XML contenga Datos
                        if (Session["XML"] == null)
                        {   //Asignandole Información como Origen de Datos
                            xmlds.Data = xml_temp.InnerXml;
                            xmlds.EnableCaching = false;
                            //Referenciando un Origen de Datos
                            trvFormas.DataSourceID = xmlds.ID;
                            //Capturando Archivo XML en variable de Session
                            Session["XML"] = xml_temp;
                        }
                        else
                        {   //Asignandole Información como Origen de Datos
                            xmlds.Data = ((XmlDocument)Session["XML"]).InnerXml;
                            xmlds.EnableCaching = false;
                            //Referenciando un Origen de Datos
                            trvFormas.DataSourceID = xmlds.ID;
                        }
                        //Limpiando TextBox
                        Elemento.Text = "";
                    }
                }
            }
        }
        /// <summary>
        /// Método encargado de Inicializar 
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion inicializaAddendaComprobante32()
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Instanciando Comprobante
            using (FEv32.Comprobante comp = new FEv32.Comprobante(this._id_comprobante))
            {
                //Validando que exista el Comprobante
                if (comp.habilitar)
                {
                    //Instanciando Id de la Addenda del Emisor
                    using (FEv32.AddendaEmisor add_emi = new FEv32.AddendaEmisor(comp.id_compania_emisor, comp.id_compania_receptor, this._id_addenda))
                    using (FEv32.Addenda add = new FEv32.Addenda(this._id_addenda))
                    {
                        //Validando que exista la Addenda Emisor
                        if (add_emi.habilitar)
                        {
                            //Validando Descripción
                            switch (add.descripcion)
                            {
                                case "Addenda LOREAL":
                                    {
                                        //Obteniendo XDocument predeterminado de la Addenda
                                        XDocument plantilla = TSDK.Base.Xml.ConvierteXmlDocumentAXDocument(add_emi.xml_predeterminado);

                                        //Declaración de namespaces a utilizar en el Comprobante
                                        //SAT
                                        XNamespace ns = CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Namespace SAT");
                                        //W3C
                                        XNamespace nsW3C = CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Namespace W3C");
                                        //Inicialziando el valor de schemaLocation del cfd
                                        string schemaLocation = CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("SchemaLocation CFD");

                                        //Creando Documento xml inicial y configurando la declaración de XML
                                        XDocument documento = new XDocument();
                                        documento.Document.Declaration = new XDeclaration("1.0", "utf-8", "");

                                        //Generando xml base del comprobante
                                        //XElement comprobante = ComprobanteXML.CargaElementosArmadoComprobante_3_2(this._id_comprobante, ns, nsW3C, schemaLocation, transaccion);
                                        XElement comprobante = FEv32.ComprobanteXML.CargaElementosArmadoComprobante_3_2(this._id_comprobante, ns, nsW3C, schemaLocation, comp.id_compania_emisor, comp.id_compania_receptor);

                                        try
                                        {
                                            //Validando que exista contenido en la Plantilla
                                            if (comprobante.HasElements)
                                            {
                                                //Validando que exista contenido en la Plantilla
                                                if (plantilla.Root.HasElements)
                                                {
                                                    //Obteniendo Espacio de Nombres
                                                    XNamespace ns_add = plantilla.Root.GetNamespaceOfPrefix("if");
                                                    XElement cuerpo = plantilla.Root.Element(ns_add + "Encabezado").Element(ns_add + "Cuerpo");

                                                    //Precargar los Campos de RI (Referencia Interfactura) del Emisor y Receptor
                                                    plantilla.Root.Element(ns_add + "Emisor").Attribute("RI").SetValue(SAT_CL.Global.Referencia.CargaReferencia(comp.id_compania_emisor, 25, SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(0, 25, "Referencia Interfactura", 0, "Facturacion Electronica")));
                                                    plantilla.Root.Element(ns_add + "Receptor").Attribute("RI").SetValue(SAT_CL.Global.Referencia.CargaReferencia(comp.id_compania_receptor, 25, SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(0, 25, "Referencia Interfactura", 0, "Facturacion Electronica")));

                                                    //Obteniendo Atributos por Cargar del Enzabezado
                                                    string moneda, fecha, condicionPago, folio, serie;
                                                    decimal subtotal, iva_tra, iva_ret, total, iva_tra_pct, iva_ret_pct;
                                                    serie = comprobante.Attribute("serie") != null ? comprobante.Attribute("serie").Value : "";
                                                    folio = comprobante.Attribute("folio") != null ? comprobante.Attribute("folio").Value : "";
                                                    moneda = comprobante.Attribute("Moneda") != null ? comprobante.Attribute("Moneda").Value : "";
                                                    fecha = comprobante.Attribute("fecha") != null ? comprobante.Attribute("fecha").Value : "";
                                                    condicionPago = comprobante.Attribute("condicionesDePago") != null ? comprobante.Attribute("condicionesDePago").Value : "";
                                                    subtotal = Convert.ToDecimal(comprobante.Attribute("subTotal") != null ? comprobante.Attribute("subTotal").Value : "0.000000");
                                                    total = Convert.ToDecimal(comprobante.Attribute("total") != null ? comprobante.Attribute("total").Value : "0.000000");
                                                    iva_ret = Convert.ToDecimal(comprobante.Element(ns + "Impuestos").Attribute("totalImpuestosRetenidos") != null ? comprobante.Element(ns + "Impuestos").Attribute("totalImpuestosRetenidos").Value : "0.000000");
                                                    iva_tra = Convert.ToDecimal(comprobante.Element(ns + "Impuestos").Attribute("totalImpuestosTrasladados") != null ? comprobante.Element(ns + "Impuestos").Attribute("totalImpuestosTrasladados").Value : "0.000000");
                                                    //Calculando Porcentaje de Iva's
                                                    iva_tra_pct = (iva_tra * 100) / subtotal;
                                                    iva_ret_pct = (iva_ret * 100) / subtotal;
                                                    //Redondeando Decimales
                                                    subtotal = Math.Round(subtotal, 2);
                                                    total = Math.Round(total, 2);
                                                    iva_ret = Math.Round(iva_tra, 2);
                                                    iva_tra = Math.Round(iva_tra, 2);
                                                    iva_ret_pct = Math.Round(iva_ret_pct, 2);
                                                    iva_tra_pct = Math.Round(iva_tra_pct, 2);

                                                    //Añadiendo Campos al Encabezado
                                                    //plantilla.Root.Element(ns_add + "Encabezado").Attribute("Serie").SetValue(serie);
                                                    plantilla.Root.Element(ns_add + "Encabezado").Attribute("Folio").SetValue(folio);
                                                    plantilla.Root.Element(ns_add + "Encabezado").Attribute("Moneda").SetValue(moneda);
                                                    plantilla.Root.Element(ns_add + "Encabezado").Attribute("Fecha").SetValue(fecha);
                                                    plantilla.Root.Element(ns_add + "Encabezado").Attribute("CondicionPago").SetValue(condicionPago);
                                                    plantilla.Root.Element(ns_add + "Encabezado").Attribute("SubTotal").SetValue(subtotal.ToString("0.00"));
                                                    plantilla.Root.Element(ns_add + "Encabezado").Attribute("Total").SetValue(total.ToString("0.00"));
                                                    plantilla.Root.Element(ns_add + "Encabezado").Attribute("Iva").SetValue(iva_tra.ToString("0.00"));
                                                    plantilla.Root.Element(ns_add + "Encabezado").Attribute("IVAPCT").SetValue(iva_tra_pct.ToString("0.00"));
                                                    //plantilla.Root.Element(ns_add + "Encabezado").Attribute("IvaRetPCT").SetValue(iva_ret_pct.ToString("0.00"));
                                                    plantilla.Root.Element(ns_add + "Encabezado").Attribute("NumProveedor").SetValue((SAT_CL.Global.Referencia.CargaReferencia(comp.id_compania_receptor, 25, SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(0, 25, "Número de Proveedor", 0, "Facturacion Electronica"))));

                                                    //Obteniendo Conceptos
                                                    using (DataTable dtConceptos = SAT_CL.FacturacionElectronica.Concepto.RecuperaConceptosComprobantes(comp.id_comprobante))
                                                    {
                                                        //Validando Existencia
                                                        if (Validacion.ValidaOrigenDatos(dtConceptos))
                                                        {
                                                            //Eliminando Cuerpos de la Plantilla
                                                            plantilla.Root.Element(ns_add + "Encabezado").Element(ns_add + "Cuerpo").Remove();

                                                            //Recorriendo Conceptos
                                                            foreach (DataRow dr in dtConceptos.Rows)
                                                            {
                                                                //Copiando Plantilla
                                                                XElement concepto = new XElement(cuerpo);

                                                                //Calculando Valor Unitario
                                                                decimal valor_u = Convert.ToDecimal(dr["MonedaCaptura"]) / Convert.ToDecimal(dr["Cantidad"]);

                                                                //Obteniendo Datos de Interes
                                                                using (FEv32.Concepto con = new FEv32.Concepto(Convert.ToInt32(dr["Id"])))
                                                                using (SAT_CL.Tarifas.TipoCargo tc = new SAT_CL.Tarifas.TipoCargo(con.id_descripcion))
                                                                {
                                                                    //Declarando Variables Auxiliares
                                                                    decimal ipct_ret = 0.00M, ipct_tra = 0.00M;

                                                                    //Obteniendo Impuestos
                                                                    using(DataTable dtImpuestos =  FEv32.ConceptoDetalleImpuesto.RecuperaImpuestosConcepto(con.id_concepto))
                                                                    {
                                                                        //Validando Existencia
                                                                        if (Validacion.ValidaOrigenDatos(dtImpuestos))
                                                                        {
                                                                            //Recorriendo
                                                                            foreach (DataRow imp in dtImpuestos.Rows)
                                                                            {
                                                                                //Instanciando Detalle de Impuesto
                                                                                using (FEv32.DetalleImpuesto det_imp = new FEv32.DetalleImpuesto(Convert.ToInt32(imp["IdDetalleImpuesto"])))
                                                                                {
                                                                                    //Validando Registro
                                                                                    if (det_imp.habilitar)
                                                                                    {
                                                                                        //Validando Tipo de Impuesto
                                                                                        switch(det_imp.TipoImpuestosDetalle)
                                                                                        {
                                                                                            case FEv32.DetalleImpuesto.TipoImpuestoDetalle.Retenido:
                                                                                                ipct_ret = det_imp.tasa;
                                                                                                break;
                                                                                            case FEv32.DetalleImpuesto.TipoImpuestoDetalle.Trasladado:
                                                                                                ipct_tra = det_imp.tasa;
                                                                                                break;
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }

                                                                    //Asignando Valores
                                                                    concepto.Attribute("Cantidad").SetValue(Convert.ToDecimal(dr["Cantidad"]).ToString("0.00"));
                                                                    concepto.Attribute("Concepto").SetValue(dr["Descripcion"].ToString());
                                                                    concepto.Attribute("PUnitario").SetValue(valor_u.ToString("0.00"));
                                                                    concepto.Attribute("Importe").SetValue(Convert.ToDecimal(dr["MonedaCaptura"]).ToString("0.00"));
                                                                    concepto.Attribute("UnidadMedida").SetValue(dr["Unidad"].ToString());
                                                                    concepto.Attribute("Partida").SetValue("00001");
                                                                    concepto.Attribute("Iva").SetValue("0.00");
                                                                    concepto.Attribute("IVAPCT").SetValue(Math.Round(ipct_tra, 2));
                                                                    concepto.Attribute("IvaRetPCT").SetValue(Math.Round(ipct_ret, 2));
                                                                }

                                                                //Añadiendo Cuerpo a la Addenda
                                                                plantilla.Root.Element(ns_add + "Encabezado").Add(concepto);
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                    //Instanciando Excepción
                                                    result = new RetornoOperacion("El documento no tiene elementos");
                                            }
                                            else
                                                //Instanciando Excepción
                                                result = new RetornoOperacion("El documento no tiene elementos");
                                        }
                                        catch (Exception ex)
                                        {
                                            //Instanciando Excepción
                                            result = new RetornoOperacion(ex.Message);
                                        }

                                        //Carga XML
                                        XmlDocument xml_temp = TSDK.Base.Xml.ConvierteXDocumentAXmlDocument(plantilla);
                                        //Asignandole Información como Origen de Datos
                                        xmlds.Data = xml_temp.InnerXml;
                                        //
                                        xmlds.EnableCaching = false;
                                        //Referenciando un Origen de Datos
                                        trvFormas.DataSourceID = xmlds.ID;
                                        //Capturando Archivo XML en variable de Session
                                        Session["XML"] = xml_temp;
                                        //Limpiando TextBox
                                        Elemento.Text = "";
                                        break;
                                    }
                                default:
                                    {
                                        //Carga XML
                                        XmlDocument xml_temp = add_emi.xml_predeterminado;
                                        //Asignandole Información como Origen de Datos
                                        xmlds.Data = xml_temp.InnerXml;
                                        //
                                        xmlds.EnableCaching = false;
                                        //Referenciando un Origen de Datos
                                        trvFormas.DataSourceID = xmlds.ID;
                                        //Capturando Archivo XML en variable de Session
                                        Session["XML"] = xml_temp;
                                        //Limpiando TextBox
                                        Elemento.Text = "";
                                        break;
                                    }
                            }
                        }
                    }
                }
            }

            //Devolviendo resultado Obtenido
            return result;
        }

        /// <summary>
        /// Método encargado de Inicializar 
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion inicializaAddendaComprobante33()
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Instanciando Comprobante
            using (FEv33.Comprobante comp = new FEv33.Comprobante(this._id_comprobante))
            {
                //Validando que exista el Comprobante
                if (comp.habilitar)
                {
                    //Instanciando Id de la Addenda del Emisor
                    using (FEv32.AddendaEmisor add_emi = new FEv32.AddendaEmisor(comp.id_compania_emisor, comp.id_compania_receptor, this._id_addenda))
                    using (FEv32.Addenda add = new FEv32.Addenda(this._id_addenda))
                    {
                        //Validando que exista la Addenda Emisor
                        if (add_emi.habilitar)
                        {
                            //Validando Descripción
                            switch (add.descripcion)
                            {
                                case "Addenda MABE v1.0":
                                    {
                                        //Obteniendo XDocument predeterminado de la Addenda
                                        XDocument plantilla = TSDK.Base.Xml.ConvierteXmlDocumentAXDocument(add_emi.xml_predeterminado);

                                        //Declaración de namespaces a utilizar en el Comprobante
                                        //SAT
                                        XNamespace ns = CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Namespace SAT");
                                        //W3C
                                        XNamespace nsW3C = CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Namespace W3C");
                                        //Inicialziando el valor de schemaLocation del cfd
                                        string schemaLocation = CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("SchemaLocation CFD");

                                        //Creando Documento xml inicial y configurando la declaración de XML
                                        XDocument documento = new XDocument();
                                        documento.Document.Declaration = new XDeclaration("1.0", "utf-8", "");

                                        //Generando xml base del comprobante
                                        //XElement comprobante = ComprobanteXML.CargaElementosArmadoComprobante_3_2(this._id_comprobante, ns, nsW3C, schemaLocation, transaccion);
                                        documento = FEv33.ComprobanteXML.CargaElementosArmadoComprobante3_3(this._id_comprobante, ns, nsW3C, schemaLocation);

                                        try
                                        {
                                            //Validando que exista contenido en la Plantilla
                                            if (documento != null)
                                            {
                                                //Validando que exista contenido en la Plantilla
                                                if (plantilla.Root.HasElements)
                                                {
                                                    //Obteniendo Espacio de Nombres
                                                    XNamespace ns_add = plantilla.Root.GetNamespaceOfPrefix("mabe");
                                                    XElement cuerpo = plantilla.Root.Element(ns_add + "Detalles").Element(ns_add + "Detalle");

                                                    //Configurando Nodos
                                                    //** ENCABEZADO **//
                                                    string tipoDocumento = comp.id_tipo_comprobante == 1 ? "FACTURA" : (comp.id_tipo_comprobante == 2 ? "NOTA CREDITO" : "FACTURA");
                                                    plantilla.Root.Attribute("tipoDocumento").SetValue(tipoDocumento);
                                                    plantilla.Root.Attribute("folio").SetValue(comp.serie + comp.folio);
                                                    plantilla.Root.Attribute("fecha").SetValue(comp.fecha_expedicion.ToString("yyyy-MM-dd"));
                                                    plantilla.Root.Attribute("referencia2").SetValue("NA");

                                                    //** MONEDA **//
                                                    plantilla.Root.Element(ns_add + "Moneda").Attribute("tipoMoneda").SetValue(documento.Root.Attribute("Moneda").Value);
                                                    //Validando Tipo de Cambio
                                                    if (comp.tipo_cambio > 0)

                                                        //Asignando Tipo de Cambio
                                                        plantilla.Root.Element(ns_add + "Moneda").Attribute("tipoCambio").SetValue(comp.tipo_cambio);
                                                    else
                                                        //Eliminando Atributo
                                                        plantilla.Root.Element(ns_add + "Moneda").Attribute("tipoCambio").Remove();

                                                    //** PROVEEDOR **//
                                                    plantilla.Root.Element(ns_add + "Proveedor").Attribute("codigo").SetValue(SAT_CL.Global.Referencia.CargaReferencia(comp.id_compania_receptor, 25, SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(2, 25, "Código Proveedor", 0, "Addenda MABE v1.0")));

                                                    //** ENTREGA **//
                                                    using (SAT_CL.Global.CompaniaEmisorReceptor receptor = new SAT_CL.Global.CompaniaEmisorReceptor(comp.id_compania_receptor))
                                                    using (SAT_CL.Global.Direccion dirReceptor = new SAT_CL.Global.Direccion(receptor.id_direccion))
                                                    {
                                                        //Validando Existencia
                                                        if (receptor.habilitar && dirReceptor.habilitar)
                                                        {
                                                            //Asignando Valores
                                                            plantilla.Root.Element(ns_add + "Entrega").Attribute("calle").SetValue(Cadena.TruncaCadena(dirReceptor.calle, 30, ""));
                                                            plantilla.Root.Element(ns_add + "Entrega").Attribute("noExterior").SetValue(dirReceptor.no_exterior);
                                                            plantilla.Root.Element(ns_add + "Entrega").Attribute("noInterior").SetValue(dirReceptor.no_interior.Trim().Equals("") ? "NA" : dirReceptor.no_interior.Trim());
                                                            plantilla.Root.Element(ns_add + "Entrega").Attribute("codigoPostal").SetValue(dirReceptor.codigo_postal);
                                                        }
                                                        else
                                                        {
                                                            //Eliminando Atributos
                                                            plantilla.Root.Element(ns_add + "Entrega").Attribute("calle").Remove();
                                                            plantilla.Root.Element(ns_add + "Entrega").Attribute("noExterior").Remove();
                                                            plantilla.Root.Element(ns_add + "Entrega").Attribute("noInterior").Remove();
                                                            plantilla.Root.Element(ns_add + "Entrega").Attribute("codigoPostal").Remove();
                                                        }

                                                        //Añadiendo Planta de Entrega
                                                        string plantaEntrega = SAT_CL.Global.Referencia.CargaReferencia(comp.id_compania_receptor, 25, SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(2, 25, "Planta Entrega", 0, "Addenda MABE v1.0"));
                                                        plantilla.Root.Element(ns_add + "Entrega").Attribute("plantaEntrega").SetValue(Cadena.TruncaCadena(plantaEntrega, 4, ""));
                                                    }

                                                    //** DETALLES **//
                                                    //Obteniendo Conceptos
                                                    using (DataTable dtConceptos = FEv33.Concepto.ObtieneConceptosComprobante(comp.id_comprobante33))
                                                    {
                                                        //Validando Existencia
                                                        if (Validacion.ValidaOrigenDatos(dtConceptos))
                                                        {
                                                            //Eliminando Detalles de Plantilla
                                                            plantilla.Root.Element(ns_add + "Detalles").RemoveAll();
                                                            int noLineaArticulo = 1;
                                                            
                                                            //Recorriendo Conceptos
                                                            foreach (DataRow dr in dtConceptos.Rows)
                                                            {
                                                                //Copiando Plantilla
                                                                XElement concepto = new XElement(cuerpo);

                                                                //Asignando Atributos
                                                                concepto.Attribute("noLineaArticulo").SetValue(noLineaArticulo);
                                                                concepto.Attribute("codigoArticulo").SetValue(dr["ClaveServProd"].ToString());
                                                                concepto.Attribute("descripcion").SetValue(Cadena.TruncaCadena(dr["Concepto"].ToString(), 40, ""));
                                                                concepto.Attribute("unidad").SetValue(dr["ClaveUnidad"].ToString());
                                                                concepto.Attribute("cantidad").SetValue(Convert.ToDecimal(dr["Cantidad"]).ToString("0.00"));
                                                                concepto.Attribute("precioSinIva").SetValue((Convert.ToDecimal(dr["ImporteMonedaCaptura"]) / Convert.ToDecimal(dr["Cantidad"])).ToString("0.00"));
                                                                concepto.Attribute("importeSinIva").SetValue(Convert.ToDecimal(dr["ImporteMonedaCaptura"]).ToString("0.00"));

                                                                //Añadiendo Nodo a Plantilla
                                                                plantilla.Root.Element(ns_add + "Detalles").Add(concepto);
                                                                noLineaArticulo++;
                                                            }
                                                        }
                                                    }

                                                    //** SUBTOTAL **//
                                                    plantilla.Root.Element(ns_add + "Subtotal").Attribute("importe").SetValue(comp.subtotal_captura.ToString("0.00"));

                                                    //** TOTAL **//
                                                    plantilla.Root.Element(ns_add + "Total").Attribute("importe").SetValue(comp.total_captura.ToString("0.00"));

                                                    //** TRASLADOS **//
                                                    //Validando Existencia de Impuestos
                                                    if (documento.Root.Element(ns + "Impuestos").Element(ns + "Traslados").HasElements)
                                                    {
                                                        //Obteniendo Nodo de Impuestos
                                                        List<XElement> imps_tra = documento.Root.Element(ns + "Impuestos").Element(ns + "Traslados").Elements(ns + "Traslado").ToList();

                                                        //Validando que existan Traslados
                                                        if (imps_tra.Count > 0)
                                                        {
                                                            //Obteniendo Base del Traslado
                                                            XElement traslado = new XElement(plantilla.Root.Element(ns_add + "Traslados").Element(ns_add + "Traslado"));
                                                            
                                                            //Eliminando Traslados
                                                            plantilla.Root.Element(ns_add + "Traslados").RemoveAll();

                                                            //Recorriendo Traslados Encontrados
                                                            foreach (XElement tras in imps_tra)
                                                            {
                                                                //Asignando Cuerpo del Traslado a Nuestra Variable Local
                                                                XElement t = new XElement(traslado);

                                                                //Asignando Atributos
                                                                t.Attribute("tipo").SetValue(SAT_CL.Global.Catalogo.RegresaDescripcionCadenaValor(94, tras.Attribute("Impuesto").Value));
                                                                t.Attribute("tasa").SetValue((Convert.ToDecimal(tras.Attribute("TasaOCuota").Value)).ToString("0.00"));
                                                                t.Attribute("importe").SetValue(tras.Attribute("Importe").Value);

                                                                //Añadiendo Traslado
                                                                plantilla.Root.Element(ns_add + "Traslados").Add(t);
                                                            }
                                                        }
                                                    }
                                                    else
                                                        //Eliminando Traslados
                                                        plantilla.Root.Element(ns_add + "Traslados").Remove();

                                                    //** RETENCIONES **//
                                                    //Validando Existencia de Impuestos
                                                    if (documento.Root.Element(ns + "Impuestos").Element(ns + "Retenciones").HasElements)
                                                    {
                                                        //Obteniendo Nodo de Impuestos
                                                        List<XElement> imps_ret = documento.Root.Element(ns + "Impuestos").Element(ns + "Retenciones").Elements(ns + "Retencion").ToList();

                                                        //Obteniendo Base del Traslado
                                                        XElement retencion = new XElement(plantilla.Root.Element(ns_add + "Retenciones").Element(ns_add + "Retencion"));

                                                        //Eliminando Traslados
                                                        plantilla.Root.Element(ns_add + "Retenciones").RemoveAll();

                                                        //Recorriendo Retenciones Encontrados
                                                        foreach (XElement rets in imps_ret)
                                                        {
                                                            //Asignando Cuerpo del Traslado a Nuestra Variable Local
                                                            XElement r = new XElement(retencion);

                                                            //Obteniendo Tasa
                                                            decimal tasa_ret = 0.00M, ret = 0.00M, round_ret = 0.00M;
                                                            ret = Convert.ToDecimal(rets.Attribute("Importe").Value) / Convert.ToDecimal(documento.Root.Attribute("SubTotal").Value);
                                                            round_ret = Math.Round(ret * 100, 1, MidpointRounding.ToEven);
                                                            decimal tasa_round = round_ret / 100;

                                                            //Validando Tasa sin Redondear
                                                            if (tasa_round >= 0.03M)
                                                                tasa_ret = 0.04M;
                                                            else
                                                                tasa_ret = tasa_round;

                                                            //Asignando Atributos
                                                            r.Attribute("tipo").SetValue("Ret");
                                                            r.Attribute("tasa").SetValue(tasa_ret.ToString("0.00"));
                                                            r.Attribute("importe").SetValue(rets.Attribute("Importe").Value);

                                                            //Añadiendo Traslado
                                                            plantilla.Root.Element(ns_add + "Retenciones").Add(r);
                                                        }
                                                    }
                                                    else
                                                        //Eliminando Traslados
                                                        plantilla.Root.Element(ns_add + "Retenciones").Remove();
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            //Instanciando Excepción
                                            result = new RetornoOperacion(ex.Message);
                                        }

                                        //Carga XML
                                        XmlDocument xml_temp = TSDK.Base.Xml.ConvierteXDocumentAXmlDocument(plantilla);
                                        //Asignandole Información como Origen de Datos
                                        xmlds.Data = xml_temp.InnerXml;
                                        //
                                        xmlds.EnableCaching = false;
                                        //Referenciando un Origen de Datos
                                        trvFormas.DataSourceID = xmlds.ID;
                                        //Capturando Archivo XML en variable de Session
                                        Session["XML"] = xml_temp;
                                        //Limpiando TextBox
                                        Elemento.Text = "";
                                        break;
                                    }
                                case "Addenda LOREAL":
                                    {
                                        //Obteniendo XDocument predeterminado de la Addenda
                                        XDocument plantilla = TSDK.Base.Xml.ConvierteXmlDocumentAXDocument(add_emi.xml_predeterminado);

                                        //Declaración de namespaces a utilizar en el Comprobante
                                        //SAT
                                        XNamespace ns = CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Namespace SAT");
                                        //W3C
                                        XNamespace nsW3C = CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Namespace W3C");
                                        //Inicialziando el valor de schemaLocation del cfd
                                        string schemaLocation = CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("SchemaLocation CFD");

                                        //Creando Documento xml inicial y configurando la declaración de XML
                                        XDocument documento = new XDocument();
                                        documento.Document.Declaration = new XDeclaration("1.0", "utf-8", "");

                                        //Generando xml base del comprobante
                                        //XElement comprobante = ComprobanteXML.CargaElementosArmadoComprobante_3_2(this._id_comprobante, ns, nsW3C, schemaLocation, transaccion);
                                        documento = FEv33.ComprobanteXML.CargaElementosArmadoComprobante3_3(this._id_comprobante, ns, nsW3C, schemaLocation);

                                        try
                                        {
                                            //Validando que exista contenido en la Plantilla
                                            if (documento != null)
                                            {
                                                //Validando que exista contenido en la Plantilla
                                                if (plantilla.Root.HasElements)
                                                {
                                                    //Obteniendo Espacio de Nombres
                                                    XNamespace ns_add = plantilla.Root.GetNamespaceOfPrefix("if");
                                                    XElement cuerpo = plantilla.Root.Element(ns_add + "Encabezado").Element(ns_add + "Cuerpo");

                                                    //Precargar los Campos de RI (Referencia Interfactura) del Emisor y Receptor
                                                    plantilla.Root.Element(ns_add + "Emisor").Attribute("RI").SetValue(SAT_CL.Global.Referencia.CargaReferencia(comp.id_compania_emisor, 25, SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(0, 25, "Referencia Interfactura", 0, "Facturacion Electronica")));
                                                    plantilla.Root.Element(ns_add + "Receptor").Attribute("RI").SetValue(SAT_CL.Global.Referencia.CargaReferencia(comp.id_compania_receptor, 25, SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(0, 25, "Referencia Interfactura", 0, "Facturacion Electronica")));

                                                    //Obteniendo Atributos por Cargar del Enzabezado
                                                    string serie, folio, moneda, fecha, condicionPago;
                                                    decimal subtotal, iva, total, iva_pct;
                                                    serie = documento.Root.Attribute("Serie") != null ? documento.Root.Attribute("Serie").Value : "";
                                                    folio = documento.Root.Attribute("Folio") != null ? documento.Root.Attribute("Folio").Value : "";
                                                    moneda = documento.Root.Attribute("Moneda") != null ? documento.Root.Attribute("Moneda").Value : "MXN";
                                                    fecha = documento.Root.Attribute("Fecha") != null ? documento.Root.Attribute("Fecha").Value : "";
                                                    condicionPago = documento.Root.Attribute("CondicionesDePago") != null ? documento.Root.Attribute("CondicionesDePago").Value : "";
                                                    subtotal = Convert.ToDecimal(documento.Root.Attribute("SubTotal") != null ? documento.Root.Attribute("SubTotal").Value : "0.000000");
                                                    total = Convert.ToDecimal(documento.Root.Attribute("Total") != null ? documento.Root.Attribute("Total").Value : "0.000000");
                                                    iva = Convert.ToDecimal(documento.Root.Element(ns + "Impuestos").Attribute("TotalImpuestosTrasladados") != null ? documento.Root.Element(ns + "Impuestos").Attribute("TotalImpuestosTrasladados").Value : "0.000000");
                                                    //Calculando Porcentaje de Iva
                                                    iva_pct = (iva * 100) / subtotal;
                                                    //Redondeando Decimales
                                                    subtotal = Math.Round(subtotal, 2);
                                                    total = Math.Round(total, 2);
                                                    iva = Math.Round(iva, 2);
                                                    iva_pct = Math.Round(iva_pct, 2);

                                                    //Añadiendo Campos al Encabezado
                                                    plantilla.Root.Element(ns_add + "Encabezado").Attribute("Serie").SetValue(serie);
                                                    plantilla.Root.Element(ns_add + "Encabezado").Attribute("Folio").SetValue(folio);
                                                    plantilla.Root.Element(ns_add + "Encabezado").Attribute("Moneda").SetValue(moneda);
                                                    plantilla.Root.Element(ns_add + "Encabezado").Attribute("Fecha").SetValue(fecha);
                                                    plantilla.Root.Element(ns_add + "Encabezado").Attribute("CondicionPago").SetValue(condicionPago);
                                                    plantilla.Root.Element(ns_add + "Encabezado").Attribute("SubTotal").SetValue(subtotal.ToString("0.00"));
                                                    plantilla.Root.Element(ns_add + "Encabezado").Attribute("Total").SetValue(total.ToString("0.00"));
                                                    plantilla.Root.Element(ns_add + "Encabezado").Attribute("Iva").SetValue(iva.ToString("0.00"));
                                                    plantilla.Root.Element(ns_add + "Encabezado").Attribute("IVAPCT").SetValue(iva_pct.ToString("0.00"));
                                                    plantilla.Root.Element(ns_add + "Encabezado").Attribute("NumProveedor").SetValue((SAT_CL.Global.Referencia.CargaReferencia(comp.id_compania_receptor, 25, SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(0, 25, "Número de Proveedor", 0, "Facturacion Electronica"))));

                                                    //Obteniendo Conceptos
                                                    using (DataTable dtConceptos = FEv33.Concepto.ObtieneConceptosComprobante(comp.id_comprobante33))
                                                    {
                                                        //Validando Existencia
                                                        if (Validacion.ValidaOrigenDatos(dtConceptos))
                                                        {
                                                            //Eliminando Cuerpos de la Plantilla
                                                            plantilla.Root.Element(ns_add + "Encabezado").Element(ns_add + "Cuerpo").Remove();
                                                            //Obteniendo Impuestos del Comprobante
                                                            List<DataRow> imp_con = new List<DataRow>();

                                                            //Obteniendo Impuestos por Concepto
                                                            using (FEv33.Impuesto imp = FEv33.Impuesto.ObtieneImpuestoComprobante(this._id_comprobante))
                                                            using (DataTable dtImp = FEv33.ImpuestoDetalle.ObtieneDetallesImpuesto(imp.id_impuesto))
                                                            {
                                                                //Recorriendo Conceptos
                                                                foreach (DataRow dr in dtConceptos.Rows)
                                                                {
                                                                    //Copiando Plantilla
                                                                    XElement concepto = new XElement(cuerpo);

                                                                    //Calculando Valor Unitario
                                                                    decimal valor_u = Convert.ToDecimal(dr["ImporteMonedaCaptura"]) / Convert.ToDecimal(dr["Cantidad"]);

                                                                    //Validando Impuestos
                                                                    if (Validacion.ValidaOrigenDatos(dtImp))
                                                                    {
                                                                        //Obteniendo Impuestos del Concepto
                                                                        imp_con = (from DataRow ic in dtImp.Rows
                                                                                   where Convert.ToInt32(ic["IdConcepto"]) == Convert.ToInt32(dr["Id"])
                                                                                   select ic).ToList();
                                                                    }

                                                                    //Asignando Valores
                                                                    concepto.Attribute("Cantidad").SetValue(Convert.ToDecimal(dr["Cantidad"]).ToString("0.00"));
                                                                    concepto.Attribute("Concepto").SetValue(Cadena.TruncaCadena(string.Format("{0} - {1}", dr["ClaveServProd"], dr["Concepto"]), 37, "..."));
                                                                    concepto.Attribute("PUnitario").SetValue(valor_u.ToString("0.00"));
                                                                    concepto.Attribute("Importe").SetValue(Convert.ToDecimal(dr["ImporteMonedaCaptura"]).ToString("0.00"));
                                                                    concepto.Attribute("UnidadMedida").SetValue(dr["ClaveUnidad"].ToString());
                                                                    concepto.Attribute("Partida").SetValue("00001");
                                                                    concepto.Attribute("Iva").SetValue("0.00");
                                                                    concepto.Attribute("IVAPCT").SetValue("0.00");

                                                                    //Validando Impuestos por Concepto
                                                                    if (imp_con.Count > 0)
                                                                    {
                                                                        //Obteniendo Traslados y Retenciones
                                                                        List<DataRow> imp_tra = (from DataRow it in imp_con
                                                                                                 where it["Detalle"].ToString().Equals("Traslado")
                                                                                                 select it).ToList();
                                                                        List<DataRow> imp_ret = (from DataRow it in imp_con
                                                                                                 where it["Detalle"].ToString().Equals("Retencion")
                                                                                                 select it).ToList();

                                                                        //Validando que solo exista un Traslado
                                                                        if (imp_tra.Count == 1)
                                                                        {
                                                                            //Asignando valores por Defecto
                                                                            concepto.Attribute("Iva").SetValue(Math.Round(Convert.ToDecimal(imp_tra[0]["ImporteCaptura"]),2));
                                                                            concepto.Attribute("IVAPCT").SetValue(Convert.ToInt32(Convert.ToDecimal(imp_tra[0]["TasaOCuota"]) * 100));
                                                                        }
                                                                        else
                                                                        {
                                                                            //Asignando valores por Defecto
                                                                            concepto.Attribute("Iva").SetValue("0.00");
                                                                            concepto.Attribute("IVAPCT").SetValue("0.00");
                                                                        }

                                                                        //Validando que solo exista una Retención
                                                                        if (imp_ret.Count == 1)
                                                                        
                                                                            //Asignando valor Calculado
                                                                            concepto.Attribute("IvaRetPCT").SetValue(Convert.ToInt32(Convert.ToDecimal(imp_ret[0]["TasaOCuota"]) * 100));
                                                                        else
                                                                            //Asignando valores por Defecto
                                                                            concepto.Attribute("IvaRetPCT").SetValue("0.00");
                                                                    }

                                                                    //Añadiendo Cuerpo a la Addenda
                                                                    plantilla.Root.Element(ns_add + "Encabezado").Add(concepto);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                    //Instanciando Excepción
                                                    result = new RetornoOperacion("El documento no tiene elementos");
                                            }
                                            else
                                                //Instanciando Excepción
                                                result = new RetornoOperacion("El documento no tiene elementos");
                                        }
                                        catch (Exception ex)
                                        {
                                            //Instanciando Excepción
                                            result = new RetornoOperacion(ex.Message);
                                        }

                                        //Carga XML
                                        XmlDocument xml_temp = TSDK.Base.Xml.ConvierteXDocumentAXmlDocument(plantilla);
                                        //Asignandole Información como Origen de Datos
                                        xmlds.Data = xml_temp.InnerXml;
                                        //
                                        xmlds.EnableCaching = false;
                                        //Referenciando un Origen de Datos
                                        trvFormas.DataSourceID = xmlds.ID;
                                        //Capturando Archivo XML en variable de Session
                                        Session["XML"] = xml_temp;
                                        //Limpiando TextBox
                                        Elemento.Text = "";
                                        break;
                                    }
                                default:
                                    {
                                        //Carga XML
                                        XmlDocument xml_temp = add_emi.xml_predeterminado;
                                        //Asignandole Información como Origen de Datos
                                        xmlds.Data = xml_temp.InnerXml;
                                        //
                                        xmlds.EnableCaching = false;
                                        //Referenciando un Origen de Datos
                                        trvFormas.DataSourceID = xmlds.ID;
                                        //Capturando Archivo XML en variable de Session
                                        Session["XML"] = xml_temp;
                                        //Limpiando TextBox
                                        Elemento.Text = "";
                                        break;
                                    }
                            }
                        }
                    }
                }
            }

            //Devolviendo resultado Obtenido
            return result;
        }

        /// <summary>
        /// Método que permite Deshabilitar Registros
        /// </summary>
        private void deshabilitaAddendaComp()
        {
            //Validamos que exista Id Comprobante
            if (this._id_addenda_comprobante != 0)
            {
                //Declarando Objeto de Retorno
                RetornoOperacion result = new RetornoOperacion();
                //Instanciando Comprobante
                using (SAT_CL.FacturacionElectronica.AddendaComprobante add_comp = new SAT_CL.FacturacionElectronica.AddendaComprobante(this._id_addenda_comprobante))
                {   //Obteniendo resultado
                    result = add_comp.DeshabilitaAddendaComprobante(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                }
                if (result.OperacionExitosa)
                {   //Restableciendo Valores
                    this._id_addenda_comprobante = 0;
                    Session["XML"] = null;
                    //Inicializando Paginah
                    inicializaPagina();
                }//Regresando Mensaje
                lblError.Text = result.Mensaje;
            }
        }

        /// <summary>
        /// Método encargado de Guardar los Registros
        /// </summary>
        private void guardaAddenda()
        {
            //Cargando archivo XML
            XmlDocument xdoc = (XmlDocument)Session["XML"];

            //Instanciando Id de la Addenda del Emisor
            using (FEv32.Addenda addenda = new FEv32.Addenda(this._id_addenda))
            {   //Cargando Esquema de Validacion del XML(XSD) dada una Addenda
                XmlDocument xml_pred = addenda.xsd_validation;
                //Obteniendo NameSpace
                string ns = xml_pred.DocumentElement.GetAttribute("targetNamespace"), msn;

                //Declarando Variables Auxiliares
                int idComprobante32 = this._version_cfdi.Equals("3.2") ? this._id_comprobante : 0;
                int idComprobante33 = this._version_cfdi.Equals("3.3") ? this._id_comprobante : 0;

                //Declarando variable de Respuesta
                RetornoOperacion result = new RetornoOperacion();

                //Obteniendo Resultado de la Validacion
                bool validacion_xml = TSDK.Base.Xml.ValidaXMLSchema(xdoc.InnerXml, xml_pred.InnerXml, ns, out msn);
                //Validando si fue correcta la Operacion
                if (validacion_xml)
                {
                    using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                    {
                        //Variables Auxiliares
                        int idCompaniaEmisora = 0, idCompaniaReceptora = 0;
                        string ruta_xml = "";

                        //Validando Versión
                        switch (this._version_cfdi)
                        {
                            case "3.2":
                                {
                                    //Instanciando Comprobante
                                    using (FEv32.Comprobante comp = new FEv32.Comprobante(this._id_comprobante))
                                    {
                                        //Validando Existencia
                                        if (comp.habilitar)
                                        {
                                            //Asignando Valores
                                            idCompaniaEmisora = comp.id_compania_emisor;
                                            idCompaniaReceptora = comp.id_compania_receptor;
                                            ruta_xml = comp.ruta_xml;
                                        }
                                    }
                                    break;
                                }
                            case "3.3":
                                {
                                    //Instanciando Comprobante
                                    using (FEv33.Comprobante comp = new FEv33.Comprobante(this._id_comprobante))
                                    {
                                        //Validando Existencia
                                        if (comp.habilitar)
                                        {
                                            //Asignando Valores
                                            idCompaniaEmisora = comp.id_compania_emisor;
                                            idCompaniaReceptora = comp.id_compania_receptor;
                                            ruta_xml = comp.ruta_xml;
                                        }
                                    }
                                    break;
                                }
                        }

                        //Evaluando Id Addenda Comprobante
                        if (this._id_addenda_comprobante == 0)
                        {
                            using (FEv32.AddendaEmisor add_emi = new FEv32.AddendaEmisor(idCompaniaEmisora, idCompaniaReceptora, Convert.ToInt32(this._id_addenda)))
                            {
                                //Obteniendo resultado de Insercion
                                result = FEv32.AddendaComprobante.IngresarAddendaComprobante(
                                         add_emi.id_emisor_addenda, idComprobante32, idComprobante33,
                                         xdoc, validacion_xml, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                            }
                        }
                        else
                        {
                            //Instanciando Addenda Comprobante
                            using (FEv32.AddendaComprobante add_comp = new FEv32.AddendaComprobante(this._id_addenda_comprobante))
                            {  //Obteniendo resultado de Edicion
                                result = add_comp.EditarAddendaComprobante(
                                         add_comp.id_emisor_addenda, idComprobante32, idComprobante33,
                                         xdoc, validacion_xml, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                            }
                        }

                        //Validando Operaciones
                        if (result.OperacionExitosa)
                        {
                            //Instanciando Addenda Comprobante
                            using (FEv32.AddendaComprobante ac = new FEv32.AddendaComprobante(result.IdRegistro))
                            {
                                //Recuperando contenido xml de addenda
                                XDocument add = XDocument.Parse(ac.xml_addenda.OuterXml);
                                XDocument cfdi = null;
                                XNamespace ns1 = null;

                                //Validando Ruta
                                if (!ruta_xml.Equals(""))
                                {
                                    //Obteniendo Lector 
                                    using (System.IO.FileStream sCfdi = new System.IO.FileStream(ruta_xml, System.IO.FileMode.Open))
                                    {
                                        cfdi = XDocument.Load(sCfdi);
                                        //Recuperando datos de interés desde el XML
                                        ns1 = cfdi.Root.GetNamespaceOfPrefix("cfdi");
                                    }
                                }

                                //Validando Operación
                                if (result.OperacionExitosa)
                                {
                                    //Validando Ruta
                                    if (!ruta_xml.Equals(""))
                                    {
                                        try
                                        {
                                            //Añadiendo a contenido de cfdi el contenido de addenda
                                            if (cfdi.Root.Element(ns1 + "Addenda") == null)
                                                //Si no existe, se añade el elemento
                                                cfdi.Root.Add(new XElement(ns1 + "Addenda", add.Root));
                                            else
                                                //Actualizando contenido de archivo xml con el contenido actual de addenda
                                                cfdi.Root.Element(ns1 + "Addenda").ReplaceWith(new XElement(ns1 + "Addenda", add.Root));

                                            //Salvando cambios en archivo xml de CFDI
                                            cfdi.Save(ruta_xml);
                                        }
                                        catch (Exception ex)
                                        {
                                            result = new RetornoOperacion(string.Format("Error al añadir Addenda al archivo xml de CFDI: {0}.", ex.Message));
                                        }
                                    }
                                }
                            }

                            //Validando Operación
                            if (result.OperacionExitosa)
                            {
                                //Almacenando Valores
                                this._id_addenda_comprobante = result.IdRegistro;
                                //Limpiando sesión
                                Session["XML"] = null;
                                scope.Complete();
                            }
                        }
                        else
                            //Instanciando Excepción
                            result = new RetornoOperacion("Error al actualizar contenido de addenda");

                    }

                    //Si no hay errores
                    if (result.OperacionExitosa)
                        //Inicializando Pagina
                        inicializaPagina();

                    //Mostrando Mensaje
                    lblError.Text = result.Mensaje;
                }
                else
                    lblError.Text = msn;
            }

            ////Instanciando Comprobante
            //using (SAT_CL.FacturacionElectronica.Comprobante
            //      comp = new SAT_CL.FacturacionElectronica.Comprobante(this._id_comprobante))
            //{
            //    ////Verificando que el comprobante no se encuentre timbrado aún
            //    //if (!comp.generado)
            //    //{
            //    //Instanciando Id de la Addenda del Emisor
            //    using (SAT_CL.FacturacionElectronica.Addenda addenda = new SAT_CL.FacturacionElectronica.Addenda(this._id_addenda))
            //    {   //Cargando Esquema de Validacion del XML(XSD) dada una Addenda
            //        XmlDocument xml_pred = addenda.xsd_validation;
            //        //Obteniendo NameSpace
            //        string ns = xml_pred.DocumentElement.GetAttribute("targetNamespace"), msn;
            //        //Declarando variable de Validacion
            //        bool validacion_xml;
            //        //Obteniendo Resultado de la Validacion
            //        validacion_xml = TSDK.Base.Xml.ValidaXMLSchema(xdoc.InnerXml, xml_pred.InnerXml, ns, out msn);
            //        //Validando si fue correcta la Operacion
            //        if (validacion_xml)
            //        {   //Declarando variable de Respuesta
            //            RetornoOperacion result = new RetornoOperacion();

            //            //Evaluando Id Addenda Comprobante
            //            if (this._id_addenda_comprobante == 0)
            //            {
            //                //Intanciando Addenda Emisor
            //                using (SAT_CL.FacturacionElectronica.AddendaEmisor add_emi = new SAT_CL.FacturacionElectronica.AddendaEmisor(comp.id_compania_emisor, comp.id_compania_receptor, Convert.ToInt32(this._id_addenda)))
            //                {
            //                    //Obteniendo resultado de Insercion
            //                    result = SAT_CL.FacturacionElectronica.AddendaComprobante.IngresarAddendaComprobante(
            //                             add_emi.id_emisor_addenda, this._id_comprobante,
            //                             xdoc, validacion_xml, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
            //                }
            //            }
            //            else
            //            {
            //                //Instanciando Addenda Comprobante
            //                using (SAT_CL.FacturacionElectronica.AddendaComprobante add_comp = new SAT_CL.FacturacionElectronica.AddendaComprobante(this._id_addenda_comprobante))
            //                {  //Obteniendo resultado de Edicion
            //                    result = add_comp.EditarAddendaComprobante(
            //                            add_comp.id_emisor_addenda,
            //                            add_comp.id_comprobante,
            //                            xdoc, validacion_xml,
            //                             ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
            //                }
            //            }
            //            if (result.OperacionExitosa)
            //            {   //Almacenando Valores
            //                this._id_addenda_comprobante = result.IdRegistro;
            //                //Inicializando Pagina
            //                inicializaPagina();
            //            }
            //            //Mostrando Mensaje
            //            lblError.Text = result.Mensaje;
            //        }
            //        else
            //            lblError.Text = msn;
            //    }

            //    //}
            //    //else
            //    //    lblError.Text = "Imposible guardar algún cambio ya que el comprobante se encuentra timbrado.";
            //}
        }

        /// <summary>
        /// Método encargado de Cargar el GridView
        /// </summary>
        private void cargaGridView()
        {   //Cargando Variables del XML
            XmlDocument doc = (XmlDocument)Session["XML"];
            //Obteniendo Nodo Seleccionado
            string path = trvFormas.SelectedNode.DataPath;
            XmlNode node = doc.SelectSingleNode(path);
            //Verificando que el Nodo Contenga Atributos
            if (node.Attributes.Count > 0)
            {   //Declarando Tabla y definiendo Columnas que recibiran Valores
                DataTable dt = new DataTable("Table");
                dt.Columns.Add("Atributo", typeof(string));
                dt.Columns.Add("Valor", typeof(string));
                //Buscando Valores en la Colección de Atributos
                foreach (XmlAttribute at1 in node.Attributes)
                {   //Añadiendo Atributos encontrados 
                    dt.Rows.Add(at1.Name, at1.Value);
                }
                //Arreglando Grid a un Tamaño Dinamico
                gvAtributos.PageSize = dt.Rows.Count;
                //Carga valores de la Tabla en el GridView
                Controles.CargaGridView(gvAtributos, dt, "", "Atributo");
                //Si hay Atributos se habilita el GridView
                gvAtributos.Enabled = true;
                //Mensaje de registros Encontrados
                gvAtributos.FooterRow.Cells[0].Text = dt.Rows.Count + " Registro(s) encontrado(s)";
            }
            else
            {   //Inicializando GridView
                Controles.InicializaGridview(gvAtributos);
                //Si no hay Atributos se deshabilita el GridView
                gvAtributos.Enabled = false;
                //Mensaje de registros Encontrados
                gvAtributos.FooterRow.Cells[0].Text = "No existen Atributos";
            }   //Verificando que el Elemento Contiene Nodos Hijos
            if (node.HasChildNodes)
            {   //Verificando que el Primer Nodo Hijo(Valor/Elemento)
                if (node.FirstChild.HasChildNodes)
                    //Si tiene Nodos Hijos, es un Elemento
                    Elemento.Text = "";
                //No tiene Nodos Hijos, es un Valor
                else
                {   //Si el Valor del Primer Hijo es vacio se muestra sin Valor el control
                    if (node.FirstChild.Value == null)
                        Elemento.Text = "";
                    //Si contiene algun valor se muestra el valor obtenido
                    else
                        Elemento.Text = node.FirstChild.Value;
                }
            }
            else//No contiene Nodos Hijos, Por lo tanto No tiene Valores
                Elemento.Text = "";
            //Muestra Nombre del Nodo
            lblNodo.Text = "Nodo: " + node.Name;
        }
        /// <summary>
        /// Inicializa Control Addenda Comprobante
        /// </summary>
        /// <param name="id_comprobante">Id Comprobante</param>
        /// <param name="id_addenda">Id Addenda</param>
        public void InicializaControl(string version, int id_comprobante, int id_addenda)
        {
            //Asignando Versión
            this._version_cfdi = version;

            //Validando Versión
            switch (this._version_cfdi)
            {
                case "3.2":
                    {
                        //Instanciando comprobante
                        using (FEv32.Comprobante c = new FEv32.Comprobante(id_comprobante))
                        {
                            //Obteniendo elemento addenda emisor
                            using (FEv32.AddendaEmisor ae = new FEv32.AddendaEmisor(c.id_compania_emisor, c.id_compania_receptor, id_addenda))
                            {
                                //Obtenemos La Addenda Comprobante en caso de existir
                                using (FEv32.AddendaComprobante objAddendaComprobante = FEv32.AddendaComprobante.RecuperaAddendaComprobante(ae.id_emisor_addenda, id_comprobante))
                                
                                    //Asignamos Valor de Addenda Comprobante
                                    this._id_addenda_comprobante = objAddendaComprobante.id_addenda_comprobante;
                            }
                        }
                        break;
                    }
                case "3.3":
                    {
                        //Instanciando comprobante
                        using (FEv33.Comprobante c = new FEv33.Comprobante(id_comprobante))
                        {
                            //Obteniendo elemento addenda emisor
                            using (FEv32.AddendaEmisor ae = new FEv32.AddendaEmisor(c.id_compania_emisor, c.id_compania_receptor, id_addenda))
                            {
                                //Obtenemos La Addenda Comprobante en caso de existir
                                using (FEv32.AddendaComprobante objAddendaComprobante = FEv32.AddendaComprobante.RecuperaAddendaComprobanteV33(ae.id_emisor_addenda, id_comprobante))
                                
                                    //Asignamos Valor de Addenda Comprobante
                                    this._id_addenda_comprobante = objAddendaComprobante.id_addenda_comprobante; ;
                            }
                        }
                        break;
                    }
            }

            //Asignamos Valores
            this._id_comprobante = id_comprobante;
            this._id_addenda = id_addenda;
            Session["XML"] = null;
            //Inicializamos Pagina
            inicializaPagina();
        }

        #endregion
    }
}