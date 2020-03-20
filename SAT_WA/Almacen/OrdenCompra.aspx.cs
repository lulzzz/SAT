using SAT_CL.Almacen;
using SAT_CL.CXP;
using SAT_CL.FacturacionElectronica;
using SAT_CL.Global;
using System;
using System.Data;
using System.IO;
using System.Text;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.Almacen
{
    /// <summary>
    /// Clase que maneja el funcionamiento del formulario web en base a eventos.
    /// </summary>
    public partial class OrdenCompra : System.Web.UI.Page
    {
        #region Eventos
        
        /// <summary>
        /// Eventoq ue permite determinar si la página es cargada por primera vez o es en base a una solicitud
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Valida si es en funcion a un IsPostBack
            if (!Page.IsPostBack)
                inicializaForma();
        }
        /// <summary>
        /// Método Público Web encargado de Obtener los Archivos del Lado del Cliente
        /// </summary>
        /// <param name="context"></param>
        /// <param name="name"></param>
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static void ArchivoSesion(object context, string file_name)
        {
            //Obteniendo Archivo Codificado en UTF8
            UTF8Encoding utf8 = new UTF8Encoding();
            byte[] archivoXML = utf8.GetBytes(context.ToString());

            //Declarando Documento XML
            XmlDocument doc = new XmlDocument();

            //Obteniendo XML en cadena
            using (MemoryStream ms = new MemoryStream(archivoXML))

                //Cargando Documento XML
                doc.Load(ms);

            //Guardando en sesión el objeto creado
            System.Web.HttpContext.Current.Session["XML"] = doc;
            System.Web.HttpContext.Current.Session["XMLFileName"] = file_name;
        }
        /// <summary>
        /// Evento Producido al Cambiar el Texto del Control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtProducto_TextChanged(object sender, EventArgs e)
        {
            //Instanciando Producto
            using (SAT_CL.Almacen.Producto producto = new SAT_CL.Almacen.Producto(Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtProducto.Text, "ID:", 1))))
            {
                //Validando si Existe
                if (producto.habilitar)
                {
                    //Asignando Precio Unitario del Producto
                    txtPrecioUnitario.Text = string.Format("{0:0.000000}", producto.precio_entrada);

                    //Declarando Variables Auxiliares
                    decimal t_existencia = 0.00M, t_requerido = 0.00M, t_por_entregar = 0.00M;

                    //Obteniendo Totales
                    SAT_CL.Almacen.Producto.ObtieneTotalesProductoInventario(producto.id_producto, out t_existencia, out t_requerido, out t_por_entregar);

                    //Asignando Totales
                    txtTExistencia.Text = string.Format("{0:0.00}", t_existencia);
                    txtTRequerido.Text = string.Format("{0:0.00}", t_requerido);
                    txtTPorEntregar.Text = string.Format("{0:0.00}", t_por_entregar);
                }
                else
                {
                    //Asignando Precio Unitario en '0'
                    txtPrecioUnitario.Text = "0.000000";
                    txtTExistencia.Text =
                    txtTRequerido.Text =
                    txtTPorEntregar.Text = "0.00";
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAgregarFactura_Click(object sender, EventArgs e)
        {
            //Obteniendo Resultado
            guardaXML();
        }
        /// <summary>
        /// Evento que permite seleccionar y ejecutar acciones del menú.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbElementoMenu_Click(object sender, EventArgs e)
        {
            //Creación del objeto botonMenu que obtiene las opciones de los menú desplegable .
            LinkButton botonMenu = (LinkButton)sender;
            //Permite ejecutar acciones determinadas por cada opción del menú
            switch (botonMenu.CommandName)
            {
                //Si la elección del menú es la opción Nuevo
                case "Nuevo":
                    {
                        //Asigna a la variable de session estatus el estado del formulario en nuevo
                        Session["estatus"] = Pagina.Estatus.Nuevo;
                        //Asigna a la variable de session id_registro el valor de 0.
                        Session["id_registro"] = 0;
                        //Invoca al método inicializaForma
                        inicializaForma();
                        //Se realiza un enfoque al primer control 
                        txtCompaniaEmisor.Focus();
                        break;
                    }
                //Si la elección del menú es la opcion Abrir
                case "Abrir":
                    {
                        //Invoca al método inicializaAperturaRegistro();
                        inicializaAperturaRegistro(143, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);
                        break;
                    }

                //Si la elección del menú es la opción Guardar
                case "Guardar":
                    {
                        //Invoca al método guardaOrdenCompra();
                        guardarOrdenCompra();
                        break;
                    }
                //Si la elección del menú es la opción Guardar
                case "Imprimir":
                    {
                        //Obteniendo Ruta
                        string urlReporte = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Almacen/OrdenCompra.aspx", "~/RDLC/Reporte.aspx");

                        //Instanciando nueva ventana de navegador para apertura de registro
                        TSDK.ASP.ScriptServer.AbreNuevaVentana(string.Format("{0}?idTipoReporte={1}&idRegistro={2}", urlReporte, "OrdenCompra", Convert.ToInt32(Session["id_registro"])), "Orden Compra", "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500", Page);
                        break;
                    }
                //Si la elección del menú es la opción Editar
                case "Editar":
                    {
                        //Instanciando Orden de Compra
                        using (SAT_CL.Almacen.OrdenCompra oc = new SAT_CL.Almacen.OrdenCompra((int)Session["id_registro"]))
                        {
                            //Validando que exista la Orden de Compra
                            if (oc.habilitar)
                            {
                                //Asigna a la variable session estaus el estado de la pagina nuevo
                                Session["estatus"] = Pagina.Estatus.Edicion;
                                //Invoca el método inicializaForma();
                                inicializaForma();
                                //Se realiza un enfoque al primer control 
                                txtProveedor.Focus();
                            }
                        }
                        break;
                    }
                //Si la elección del menú es la opción Eliminar
                case "Eliminar":
                    {
                        //Creación del objeto retorno
                        RetornoOperacion retorno = new RetornoOperacion();
                        //Invoca al constructor de la clase OrdenCompra para poder instanciar sus  métodos 
                        using (SAT_CL.Almacen.OrdenCompra oc = new SAT_CL.Almacen.OrdenCompra((int)Session["id_registro"]))
                        {
                            //Valida que exista el registro
                            if (oc.id_orden_compra > 0)
                            {
                                //Asigna los valores al objeto retorno invocando el método DeshabilitarOrdenCompra.
                                retorno = oc.DeshabilitarOrdenCompra(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                            }
                        }
                        //Valida si la inserción a la base de datos se realizo correctamente
                        if (retorno.OperacionExitosa)
                        {
                            //A la variable de sessión estatus le asigna el estado de la pagina en modo lectura
                            Session["estatus"] = Pagina.Estatus.Lectura;
                            //A la variable de session id_registro le asigna el valor insertado en la tabla OrdenCompra
                            Session["id_registro"] = retorno.IdRegistro;
                            //Invoca al método inicializa forma
                            inicializaForma();
                        }
                        
                        //Mensaje de que la operación de deshabilitar registros se realizo correctamente.
                        ScriptServer.MuestraNotificacion(botonMenu, retorno, ScriptServer.PosicionNotificacion.AbajoDerecha);
                        break;
                    }
                case "Etiqueta":
                    {
                        //Obteniendo Ruta
                        string urlReporte = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Almacen/OrdenCompra.aspx", "~/RDLC/Reporte.aspx");

                        //Instanciando nueva ventana de navegador para apertura de registro
                        TSDK.ASP.ScriptServer.AbreNuevaVentana(string.Format("{0}?idTipoReporte={1}&idRegistro={2}", urlReporte, "Etiqueta", Convert.ToInt32(Session["id_registro"])), "Orden Compra", "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500", Page);
                        break;
                    }
                case "Email":
                    {
                        //Creación del objeto retorno
                        RetornoOperacion retorno = new RetornoOperacion();
                        
                        //Validando Sesión
                        switch ((Pagina.Estatus)Session["estatus"])
                        {
                            case Pagina.Estatus.Nuevo:
                                {
                                    //Instanciando Excepción
                                    retorno = new RetornoOperacion("Debe de existir una Orden de Compra");
                                    break;
                                }
                            case Pagina.Estatus.Lectura:
                            case Pagina.Estatus.Edicion:
                                {
                                    //Invoca al constructor de la clase OrdenCompra para poder instanciar sus  métodos 
                                    using (SAT_CL.Almacen.OrdenCompra oc = new SAT_CL.Almacen.OrdenCompra((int)Session["id_registro"]))
                                    using (SAT_CL.Global.CompaniaEmisorReceptor compania = new CompaniaEmisorReceptor(oc.id_compania_emisor))
                                    using (SAT_CL.Global.CompaniaEmisorReceptor proveedor = new CompaniaEmisorReceptor(oc.id_proveedor))
                                    {
                                        //Valida que exista el registro
                                        if (oc.habilitar && compania.habilitar)
                                        {
                                            //Obteniendo PDF y Configurando
                                            string nombre = string.Format("{0}_{1:0000000}", compania.nombre_corto, oc.no_orden_compra);
                                            byte[] impresion = oc.GeneraImpresionOrdenCompraPDF(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                            //Inicializando control
                                            wucEnvioOrdenCompra.InicializaControl("¡Usted ha recibido una Orden de Compra!", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).email,
                                                    string.Format("{0} - Orden de Compra: {1:0000000}", compania.nombre_corto, oc.no_orden_compra),
                                                    proveedor.correo, "Este adjunto es la representación impresa de la Orden de Compra", 
                                                    new Tuple<string, byte[], string>(nombre, impresion, "pdf"));

                                            //Enviando Correo de Forma Automatica
                                            retorno = wucEnvioOrdenCompra.EnviaEmail();
                                        }
                                        else
                                            //Instanciando Excepción
                                            retorno = new RetornoOperacion("No se puede recuperar la Orden de Compra");
                                    }


                                    break;
                                }
                        }

                        //Mensaje de que la operación de deshabilitar registros se realizo correctamente.
                        ScriptServer.MuestraNotificacion(botonMenu, retorno, ScriptServer.PosicionNotificacion.AbajoDerecha);                                    

                        break;
                    }
                //Si la elección del menú es la opción Bitacora
                case "Bitacora":
                    {
                        //Invoca al método bitacora
                        inicializaBitacora(Session["id_registro"].ToString(), "143", "Orden de Compra");
                        break;
                    }
                //Si la elección del menú es la opcion Referencia
                case "Referencias":
                    {
                        //Invoca al método inicializaReferencia que muestra las observaciones hechas a un registro de OrdenCompra
                        inicializaReferenciaRegistro(Session["id_registro"].ToString(), "143",
                                                    ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString());
                        break;
                    }
                //Si la elección del menú es la opcion Archivo
                case "Archivo":
                    {
                        break;
                    }
                //Si la elección del menú es la opcion Facturas
                case "Facturas":
                    {
                        //Instanciando Orden de Compra
                        using (SAT_CL.Almacen.OrdenCompra oc = new SAT_CL.Almacen.OrdenCompra(Convert.ToInt32(Session["id_registro"])))
                        {
                            //Validando que exista la Orden de Compra
                            if (oc.habilitar && oc.estatus == SAT_CL.Almacen.OrdenCompra.Estatus.Cerrada)
                            {
                                //Instanciando Factura de Proveedor
                                using (FacturadoProveedor fac_prov = new FacturadoProveedor(oc.id_factura_proveedor))
                                {
                                    //Validando que exista la Factura
                                    if (fac_prov.habilitar)
                                    {
                                        //Si la Factura esta en Revisión
                                        if ((FacturadoProveedor.EstatusFactura)fac_prov.id_estatus_factura == FacturadoProveedor.EstatusFactura.EnRevision)

                                            //Mostrando Vista de Alerta
                                            mtvConfirmacion.ActiveViewIndex = 0;
                                        else
                                        {
                                            //Validando Estatus de la Factura
                                            switch ((FacturadoProveedor.EstatusFactura)fac_prov.id_estatus_factura)
                                            {
                                                case FacturadoProveedor.EstatusFactura.Aceptada:
                                                    //Instanciando Mensaje
                                                    lblMensaje.Text = "La Factura ha sido Aceptada, no puede sobreescribirse";
                                                    break;
                                                case FacturadoProveedor.EstatusFactura.AplicadaParcial:
                                                case FacturadoProveedor.EstatusFactura.Liquidada:
                                                    //Instanciando Mensaje
                                                    lblMensaje.Text = "La Factura tiene pagos aplicados, no puede sobreescribirse";
                                                    break;
                                                case FacturadoProveedor.EstatusFactura.Rechazada:
                                                    //Instanciando Mensaje
                                                    lblMensaje.Text = "La Factura ha sido Rechazada, no puede sobreescribirse";
                                                    break;
                                            }

                                            //Mostrando Vista de Error
                                            mtvConfirmacion.ActiveViewIndex = 1;
                                        }

                                        //Abriendo Ventana Modal
                                        gestionaVentanaModal(botonMenu, "ConfirmacionFactura");
                                    }
                                    else
                                    {
                                        //Limpiando Sesión
                                        Session["XML"] = null;

                                        //Abriendo Ventana Modal
                                        gestionaVentanaModal(botonMenu, "FacturasLigadas");

                                        //Cargando Factura
                                        cargaFacturaLigada();
                                    }
                                }
                            }
                            else
                                //Mostrando Excepción
                                ScriptServer.MuestraNotificacion(this, "La Orden de Compra debe estar 'Cerrada' para su Facturación", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                        }
                        break;
                    }
            }
        }
        /// <summary>
        /// Evento que controla el almacenamiento del registro a la base de datos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            //Invoca al método guardarOrdenCompra
            guardarOrdenCompra();
        }
        /// <summary>
        /// Evento que anula acciones realizadas sobre el formulario (Inserción y edición de datos)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            //Acorde al estatus de la página valida:
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //En caso de que el estado de la pagina sea Nuevo
                case Pagina.Estatus.Nuevo:
                    {
                        //Asigna a la variable de sessión id_registro el valor de 0.
                        Session["id_registro"] = 0;
                        break;
                    }
                //En caso de que el estado de la página sea de edición
                case Pagina.Estatus.Edicion:
                    {
                        //Asigna a la variable de session estatus el valor de Lectura
                        Session["estatus"] = Pagina.Estatus.Lectura;
                        break;
                    }
            }

            //Invoca al método inicializaForma().
            inicializaForma();
        }
        /// <summary>
        /// Evento que permite agregar productos a una orden de compra.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAbrirModalDOC_Click(object sender, EventArgs e)
        {
            //Validando el Estatus de la Página
            switch((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {
                        //Mostrando Mensaje
                        ScriptServer.MuestraNotificacion(this, "No puede Agregar Detalles, porque no Existe la Orden de Compra", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                        break;
                    }
                case Pagina.Estatus.Lectura:
                case Pagina.Estatus.Edicion:
                    {
                        //Instanciando Orden de Compra
                        using (SAT_CL.Almacen.OrdenCompra oc = new SAT_CL.Almacen.OrdenCompra(Convert.ToInt32(Session["id_registro"])))
                        {
                            //Validando que exista la Orden de Trabajo
                            if (oc.habilitar)
                            {
                                //Validando Estatus
                                if (oc.estatus == SAT_CL.Almacen.OrdenCompra.Estatus.Registrada)
                                {
                                    //Invoca a la ventana Modal.
                                    gestionaVentanaModal(btnAbrirModalDOC, "DetalleOrdenCompra");
                                    //Invoca al método de limpia control modal.
                                    limpiaControlesModalOrdenCompraDetalle();
                                    //El cursor cuando se abra la ventana modal se localizara en el primer cuadro de texto.
                                    txtProducto.Focus();
                                }
                                /*** Mostrando Excepciones Personalizadas ***/
                                else if (oc.estatus == SAT_CL.Almacen.OrdenCompra.Estatus.Solicitada)
                                    //Mostrando Mensaje
                                    ScriptServer.MuestraNotificacion(this, "La Orden de Compra ha sido 'Solicitada', Imposible agregar Detalles", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                                else if (oc.estatus == SAT_CL.Almacen.OrdenCompra.Estatus.Cerrada)
                                    //Mostrando Mensaje
                                    ScriptServer.MuestraNotificacion(this, "La Orden de Compra esta 'Cerrada', Imposible agregar Detalles", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                                else if (oc.estatus == SAT_CL.Almacen.OrdenCompra.Estatus.Cancelada)
                                    //Mostrando Mensaje
                                    ScriptServer.MuestraNotificacion(this, "La Orden de Compra ha sido 'Cancelada', Imposible agregar Detalles", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                                else if (oc.estatus == SAT_CL.Almacen.OrdenCompra.Estatus.AbastecidaParcial)
                                    //Mostrando Mensaje
                                    ScriptServer.MuestraNotificacion(this, "La Orden de Compra se encuentra 'Abastecida Parcial', Imposible agregar Detalles", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                            }
                        }
                        break;
                    }
            }
        }
        /// <summary>
        /// Evento que permite guardar el detalle de orden de compra
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardarDOC_Click(object sender, EventArgs e)
        {
            //Invoca al método de guardarordenCompraDetalle
            guardarOrdenCompraDetalle();
        }
        /// <summary>
        /// Evento que permite cambiar el estado de una orden de compra de registrada a solicitada.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSolicitarOrden_Click(object sender, EventArgs e)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Invoca a la clase ordenCompra para realizar la actualización del estatus de una orden de compra
            using (SAT_CL.Almacen.OrdenCompra oc = new SAT_CL.Almacen.OrdenCompra((int)Session["id_registro"]))
            {
                //Valida si existe el registro
                if (oc.id_orden_compra > 0)
                {
                    //Validando el Estatus de la Orden
                    if (oc.estatus == SAT_CL.Almacen.OrdenCompra.Estatus.Registrada)
                    {
                        //Asigna al objeto retorno el estatus de la orden de compra y el Id de usuario, invocando al método ActualizaEstatusOrdenCompra
                        retorno = oc.ActualizaEstatusOrdenCompra(SAT_CL.Almacen.OrdenCompra.Estatus.Solicitada, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                        //Invoca al constructor de la clase orden de compra detalle para la obtencion de los detalles
                        using (DataTable dtOrdenCompraDetalle = SAT_CL.Almacen.OrdenCompraDetalle.CargaDetallesOrdenCompra(oc.id_orden_compra))
                        {
                            //Valida que existan los registros del la tabla dtOrdenCompraDetalle
                            if (Validacion.ValidaOrigenDatos(dtOrdenCompraDetalle))
                            {
                                //Recorre la tabla dtOrdenCompraDetalle de la orden de compra
                                foreach (DataRow r in dtOrdenCompraDetalle.Rows)
                                {
                                    //Instancia a la clase orden compra para obtener el método de actulizacion detalle orden de compra
                                    using (SAT_CL.Almacen.OrdenCompraDetalle ocd = new SAT_CL.Almacen.OrdenCompraDetalle((int)r["Id"]))
                                    {
                                        //Valida que exista el registro detalle orden de compra
                                        if (ocd.id_orden_compra_detalle > 0)
                                            //Invoca al método de ActualizaOrdenCompraDetalle
                                            ocd.ActualizaEstatusDetalle(SAT_CL.Almacen.OrdenCompraDetalle.Estatus.Solicitada, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                        //En caso de que no exista el registro
                                        else
                                            //Sale del ciclo de validación.
                                            break;
                                    }
                                }

                            }
                            //En caso de que no exista detalles de la orden de compra 
                            else
                                //Envia un mensaje de que no se pudo realizar la orden de compra.
                                retorno = new RetornoOperacion("No se puede actualizar los detalles de la orden de compra");
                        }
                    }
                    else
                        //Envia un mensaje de que no se pudo realizar la orden de compra.
                        retorno = new RetornoOperacion("La Orden de Compra ya ha sido Solicitada");
                }
            }
            //Valida que la operación de actualizar estatus se realizo correctamente
            if (retorno.OperacionExitosa)
            {
                //Asigna a la variable de session el estado de lectura
                Session["estatus"] = Pagina.Estatus.Edicion;
                //Asigna a la variable de sesion el Id del Registro
                Session["id_registro"] = retorno.IdRegistro;
                //Invoca al método inicializaForma
                inicializaForma();
            }

            //Muestra un mensaje de realización de acción exitosa o erronea. 
            ScriptServer.MuestraNotificacion(this, retorno, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarFacXML_Click(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            
            //Declarando Documento XML
            XmlDocument doc = (XmlDocument)Session["XML"];

            //Declarando variables de Montos
            decimal total_p, subtotal_p, descuento_p, traslado_p, retenido_p, monto_tc;

            //Obteniendo Valores
            obtieneCantidades(doc, out total_p, out subtotal_p, out descuento_p, out traslado_p, out retenido_p, out monto_tc);

            //Instanciando Emisor-Compania
            using (SAT_CL.Global.CompaniaEmisorReceptor emisor = SAT_CL.Global.CompaniaEmisorReceptor.ObtieneInstanciaCompaniaRFC(doc["cfdi:Comprobante"]["cfdi:Emisor"].Attributes["rfc"].Value, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
            {
                //Validando que coincida el RFC del Emisor
                if (emisor.id_compania_emisor_receptor > 0)
                {
                    //Validando el Emisora
                    if (Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtProveedor.Text, "ID:", 1)) == emisor.id_compania_emisor_receptor)
                    {
                        //Instanciando Emisor-Compania
                        using (SAT_CL.Global.CompaniaEmisorReceptor receptor = new CompaniaEmisorReceptor(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                        {
                            //Validando que coincida el RFC del Receptor
                            if (receptor.rfc.Equals(doc["cfdi:Comprobante"]["cfdi:Receptor"].Attributes["rfc"].Value))
                            {
                                //Declarando Variables Auxiliares
                                decimal totalImpTrasladados = 0.00M, totalImpRetenidos = 0.00M;

                                /** Retenciones **/
                                //Validando que no exista el Nodo
                                if (doc.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosRetenidos"] == null)
                                {
                                    //Validando que existan Retenciones
                                    if (doc.DocumentElement["cfdi:Impuestos"]["cfdi:Retenciones"] != null)
                                    {
                                        //Validando que existan Nodos
                                        if (doc.DocumentElement["cfdi:Impuestos"]["cfdi:Retenciones"].ChildNodes.Count > 0)
                                        {
                                            //Recorriendo Retenciones
                                            foreach (XmlNode node in doc.DocumentElement["cfdi:Impuestos"]["cfdi:Retenciones"])
                                            {
                                                //Sumando Impuestos Retenidos
                                                totalImpRetenidos += Convert.ToDecimal(node.Attributes["importe"].Value);
                                            }
                                        }
                                    }
                                }
                                else
                                    //Asignando Total de Impuestos
                                    totalImpRetenidos = Convert.ToDecimal(doc.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosRetenidos"].Value);


                                /** Traslados **/
                                //Validando que no exista el Nodo
                                if (doc.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosTrasladados"] == null)
                                {
                                    //Validando que existan Traslados
                                    if (doc.DocumentElement["cfdi:Impuestos"]["cfdi:Traslados"] != null)
                                    {
                                        //Validando que existan Nodos
                                        if (doc.DocumentElement["cfdi:Impuestos"]["cfdi:Traslados"].ChildNodes.Count > 0)
                                        {
                                            //Recorriendo Traslados
                                            foreach (XmlNode node in doc.DocumentElement["cfdi:Impuestos"]["cfdi:Traslados"])
                                            {
                                                //Sumando Impuestos Trasladados
                                                totalImpRetenidos += Convert.ToDecimal(node.Attributes["importe"].Value);
                                            }
                                        }
                                    }
                                }
                                else
                                    //Asignando Total de Impuestos
                                    totalImpTrasladados = Convert.ToDecimal(doc.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosTrasladados"].Value);

                                //Insertando factura
                                result = InsertaFacturaXML(doc, emisor.id_compania_emisor_receptor, emisor.dias_credito, receptor.id_compania_emisor_receptor,
                                            total_p, subtotal_p, descuento_p, traslado_p, retenido_p, monto_tc, totalImpTrasladados, totalImpRetenidos);

                                //Validando Operación Exitosa
                                if (result.OperacionExitosa)
                                {
                                    //Cargando Facturas Ligadas
                                    cargaFacturaLigada();

                                    //Mostrando Facturas
                                    gestionaVentanaModal(btnAceptarFacXML, "FacturasLigadas");
                                    gestionaVentanaModal(btnAceptarFacXML, "FacturaDiferencia");
                                }
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Evento Producido al Cerrar alguna Ventana Modal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarVentanaModal_Click(object sender, EventArgs e)
        {
            //Determinando botón pulsado
            LinkButton lkbCerrar = (LinkButton)sender;

            //De acuerdo al comando del botón
            switch (lkbCerrar.CommandName)
            {
                case "DetalleOrdenCompra":
                    {
                        //Invocando Método de Gestión
                        gestionaVentanaModal(lkbCerrar, lkbCerrar.CommandName);
                        //Invoca al método que limpia los controles de la modal.
                        limpiaControlesModalOrdenCompraDetalle();
                        //Inicializando Indices
                        Controles.InicializaIndices(gvOrdenCompraDetalle);
                        break;
                    }
                default:
                    {
                        //Invocando Método de Gestión
                        gestionaVentanaModal(lkbCerrar, lkbCerrar.CommandName);
                        break;
                    }
            }
        }

        #region Eventos GridView "Orden de Compra Detalle"

        /// <summary>
        /// Evento Producido al Enlazar los Datos con el GridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvOrdenCompraDetalle_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Cargando Menu Contextual
            Controles.CreaMenuContextual(e, "menuContext", "menuOptions", "MostrarMenu", true, true);
        }
        /// <summary>
        /// Evento que permite cargar los valores a los controles de la modal solo si se desea realizar una edición.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEditar_Click(object sender, EventArgs e)
        {
            //Validando que existen Registros en el gridView
            if (gvOrdenCompraDetalle.DataKeys.Count > 0)
            {
                //Seleccionando Fila del grid view para editar los valores
                TSDK.ASP.Controles.SeleccionaFila(gvOrdenCompraDetalle, sender, "lnk", false);
                
                //Invoca al Constructor de la clase ordenCompradetalle  para obtener el detalle de orden de compra
                using (SAT_CL.Almacen.OrdenCompraDetalle ocd = new SAT_CL.Almacen.OrdenCompraDetalle(Convert.ToInt32(gvOrdenCompraDetalle.SelectedDataKey["Id"])))
                {
                    //Valida que el valor del estatus sea diferente de 2(solicitada)
                    if (ocd.estatus == OrdenCompraDetalle.Estatus.Registrada || ocd.estatus == OrdenCompraDetalle.Estatus.Solicitada)
                    {
                        //Invoca a la clase de producto para obtener el nombre del producto
                        using (SAT_CL.Almacen.Producto prod = new SAT_CL.Almacen.Producto(ocd.id_producto))
                        {
                            //Validando que existe el Producto
                            if (prod.habilitar)

                                //Asigna al control los valores obtenidos de la instancia
                                txtProducto.Text = string.Format("{0} ID:{1}", prod.descripcion, prod.id_producto);
                            else
                                //Limpiando Control
                                txtProducto.Text = "";
                        }

                        //Asigna valores a los controles obtenidos de la instancia al gridview
                        txtCantidad.Text = ocd.cantidad.ToString();
                        txtPrecioUnitario.Text = ocd.precio_unitario.ToString();
                        //txtPrecioUnitario.Text = ocd.precio_unitario.ToString();
                        ddlEstatusDetalle.SelectedValue = ocd.id_estatus.ToString();
                        //Focaliza a la caja de texto txtProducto
                        txtProducto.Focus();
                        //Mostrando Ventana Modal
                        gestionaVentanaModal(upgvOrdenCompraDetalle, "DetalleOrdenCompra");
                    }
                    else
                    {   
                        //Mostrando Mensaje de la Operación
                        ScriptServer.MuestraNotificacion(this, "El Detalle debe de estar en Estatus 'Registrada' ó 'Solicitada' para su Edición", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);

                        //Inicializando Indices
                        Controles.InicializaIndices(gvOrdenCompraDetalle);
                    }
                }
            }
        }
        /// <summary>
        /// Evento producido al Eliminar el Detalle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEliminar_Click(object sender, EventArgs e)
        {
            //Validando que existen Registros en el gridView
            if (gvOrdenCompraDetalle.DataKeys.Count > 0)
            {
                //Seleccionando Fila del grid view para editar los valores
                TSDK.ASP.Controles.SeleccionaFila(gvOrdenCompraDetalle, sender, "lnk", false);

                //Declarando Objeto de Retorno
                RetornoOperacion result = new RetornoOperacion();

                //Instanciando Detalle
                using(SAT_CL.Almacen.OrdenCompraDetalle detalle = new SAT_CL.Almacen.OrdenCompraDetalle(Convert.ToInt32(gvOrdenCompraDetalle.SelectedDataKey["Id"])))
                {
                    //Validando que exista el Detalle
                    if (detalle.habilitar)
                    {
                        //Validando que este en Estatus Registrado
                        if (detalle.estatus == OrdenCompraDetalle.Estatus.Registrada)
                        
                            //Deshabilitando Detalle
                            result = detalle.DeshabilitarOrdenCompraDetalle(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                        else
                            //Instanciando Excepción
                            result = new RetornoOperacion("El Detalle debe de estar en Estatus 'Registrado' para poder Eliminarlo");
                    }
                    else
                        //Instanciando Excepción
                        result = new RetornoOperacion("No se puede Acceder al Detalle, es posible que se haya Eliminado");

                    //Si la Operación fue Exitosa
                    if (result.OperacionExitosa)
                    
                        //Cargando Detalles
                        cargaDetallesOrdenCompra();

                    //Inicializando Indices
                    Controles.InicializaIndices(gvOrdenCompraDetalle);
                }

                //Mostrando Mensaje de la Operación
                ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
        }
        /// <summary>
        /// Evento Producido al Dar Click en el Link de Inventario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkInventario_Click(object sender, EventArgs e)
        {
            //Validando que existen Registros en el gridView
            if (gvOrdenCompraDetalle.DataKeys.Count > 0)
            {
                //Obteniendo Control
                LinkButton lkb = (LinkButton)sender;
                
                //Seleccionando Fila del grid view para editar los valores
                TSDK.ASP.Controles.SeleccionaFila(gvOrdenCompraDetalle, sender, "lnk", false);

                //Instanciando Detalle
                using (SAT_CL.Almacen.OrdenCompraDetalle detalle = new SAT_CL.Almacen.OrdenCompraDetalle(Convert.ToInt32(gvOrdenCompraDetalle.SelectedDataKey["Id"])))
                {
                    //Validando que exista el Detalle
                    if (detalle.habilitar)
                    {
                        //Validando el Estatus del Detalle
                        if (detalle.estatus == OrdenCompraDetalle.Estatus.AbastecidaParcial || detalle.estatus == OrdenCompraDetalle.Estatus.Cerrada)
                        {
                            //Inicializando Indices
                            Controles.InicializaIndices(gvProductosInventario);
                            
                            //Cargando Detalles
                            cargaDetallesInventario();

                            //Mostrando Ventana Modal
                            gestionaVentanaModal(lkb, "ProductoInventario");
                        }
                        else
                            //Mostrando Excepción
                            ScriptServer.MuestraNotificacion(lkb, "El Detalle no se ha abastecido", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                    }
                    else
                    {
                        //Inicializando Indices
                        Controles.InicializaIndices(gvOrdenCompraDetalle);
                        
                        //Mostrando Excepción
                        ScriptServer.MuestraNotificacion(lkb, "El Detalle no se ha abastecido", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                    }
                }
            }
        }

        #endregion

        #region Eventos "E-mail"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucEnvioOrdenCompra_CerrarEmail_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucEnvioOrdenCompra_EnviarEmail_Click(object sender, EventArgs e)
        {
            //Validando 
        }

        #endregion

        #region Eventos Surtir

        /// <summary>
        /// Evento Producido al Dar Click en el Boton "Surtir"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSurtir_Click(object sender, EventArgs e)
        {
            //Validando que exista una Fila Seleccionada
            if (gvOrdenCompraDetalle.SelectedIndex != -1)
            {
                //Declarando Objeto de Retorno
                RetornoOperacion result = new RetornoOperacion();

                //Obteniendo Cantidad
                decimal cantidad = Convert.ToDecimal(txtCantidadInv.Text == "" ? "0" : txtCantidadInv.Text);

                //Declarando Variable Auxiliar
                int idDetalleOC = 0;

                //Validando que no sea 0
                if (cantidad > 0)
                {
                    //Validando que la Cantidad no Exceda el Valor Permitido
                    if (Convert.ToDecimal(gvOrdenCompraDetalle.SelectedDataKey["Cantidad"]) >= cantidad)
                    {
                        //Inicializando Bloque Transaccional
                        using (TransactionScope trans = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                        {
                            //Instanciando Orden
                            using (SAT_CL.Almacen.OrdenCompra orden = new SAT_CL.Almacen.OrdenCompra(Convert.ToInt32(Session["id_registro"])))
                            //Instanciando Detalles
                            using (OrdenCompraDetalle ordenDetalle = new OrdenCompraDetalle(Convert.ToInt32(gvOrdenCompraDetalle.SelectedDataKey["Id"])))
                            {
                                //Validando que exista el Detalle y la Requisición
                                if (ordenDetalle.habilitar && orden.habilitar)
                                {
                                    //Declarando Estatus
                                    SAT_CL.Almacen.OrdenCompraDetalle.Estatus estatus;

                                    //Obteniendo Estatus
                                    estatus = Convert.ToDecimal(gvOrdenCompraDetalle.SelectedDataKey["Cantidad"]) == cantidad ? SAT_CL.Almacen.OrdenCompraDetalle.Estatus.Cerrada : SAT_CL.Almacen.OrdenCompraDetalle.Estatus.AbastecidaParcial;

                                    //Actualizando Estatus
                                    result = ordenDetalle.ActualizaEstatusOrdenCompraDetalle(estatus, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                    //Validando Operación Exitosa
                                    if (result.OperacionExitosa)
                                    {
                                        //Obteniendo Valor
                                        idDetalleOC = result.IdRegistro;

                                        //Insertando Salida
                                        result = SAT_CL.Almacen.EntradaSalida.InsertarEntradaSalida(EntradaSalida.TipoOperacion.Entrada, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, 144,
                                                            ordenDetalle.id_orden_compra_detalle, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), orden.id_moneda, orden.id_almacen, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                        //Validando Operación Exitosa
                                        if (result.OperacionExitosa)
                                        {
                                            //Obteniendo Salida
                                            int idEntrada = result.IdRegistro;

                                            //Instanciando Producto
                                            using (SAT_CL.Almacen.Producto prod = new SAT_CL.Almacen.Producto(ordenDetalle.id_producto))
                                            {
                                                //Validando que Exista el Producto
                                                if (prod.habilitar)
                                                {
                                                    //Validando que se requieran los Detalles
                                                    if (rbLote.Checked)
                                                    {
                                                        //Obteniendo Fecha
                                                        DateTime fec_cad;
                                                        DateTime.TryParse(txtFecCad.Text, out fec_cad);
                                                        
                                                        //Actualizando Inventario
                                                        result = SAT_CL.Almacen.Inventario.IncrementarExistencias(prod.id_producto, cantidad, idEntrada, txtLote.Text, "", fec_cad,
                                                                    144, ordenDetalle.id_orden_compra_detalle, 0, "0", "0", "0", prod.precio_entrada, prod.precio_entrada, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                    }
                                                    else if (rbSerie.Checked)
                                                    {
                                                        //Validando si existen Detalles
                                                        if (Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3")))
                                                        {
                                                            //Recorriendo Ciclos
                                                            foreach (DataRow dr in OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3").Rows)
                                                            {
                                                                //Obteniendo Fecha de Caducidad
                                                                DateTime fec_cad = DateTime.MinValue;
                                                                DateTime.TryParse(dr["FechaCaducidad"].ToString(), out fec_cad);

                                                                //Actualizando Inventario
                                                                result = SAT_CL.Almacen.Inventario.IncrementarExistencias(prod.id_producto, 1, idEntrada, "", dr["Serie"].ToString(), fec_cad,
                                                                            144, ordenDetalle.id_orden_compra_detalle, 0, "0", "0", "0", prod.precio_entrada, prod.precio_entrada, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                                                //Validando que la Operación no fuese Exitosa
                                                                if (!result.OperacionExitosa)

                                                                    //Terminando Ciclo
                                                                    break;
                                                            }
                                                        }
                                                        else
                                                            //Instanciando Excepción
                                                            result = new RetornoOperacion("No existen Detalles de Inventario");
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                //Validando Operación Exitosa
                                if (result.OperacionExitosa)
                                {
                                    //Instanciando Detalle
                                    result = new RetornoOperacion(idDetalleOC);

                                    //Completando Transacción
                                    trans.Complete();
                                }
                            }
                        }

                        //Operación Exitosa?
                        if (result.OperacionExitosa)
                        {
                            //Inicializando Forma
                            inicializaForma();
                            
                            //Marcando Fila
                            Controles.MarcaFila(gvOrdenCompraDetalle, idDetalleOC.ToString(), "Id", "Id-Cantidad-Disponibles", OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "", gvOrdenCompraDetalle.PageSize, true, 1);

                            //Ocultando Ventana Modal
                            gestionaVentanaModal(this, "CantidadProducto");
                        }
                    }
                    else
                        //Instanciando Excepción
                        result = new RetornoOperacion(string.Format("La Cantidad Ingresada '{0:0.00}', excede la cantidad permitida '{1:0.00}'", cantidad, gvOrdenCompraDetalle.SelectedDataKey["Cantidad"]));
                }
                else
                    //Instanciando Excepción
                    result = new RetornoOperacion("No se puede abastecer la Cantidad '0'");

                //Mostrando Resultado de la Operación
                ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
        }
        /// <summary>
        /// Evento Producido al Dar Click en las Opciones de Surtir
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkSurtir_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvOrdenCompraDetalle.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvOrdenCompraDetalle, sender, "lnk", false);

                //Obteniendo Control
                LinkButton lnkSurtir = (LinkButton)sender;

                //Validando Comando
                switch (lnkSurtir.CommandName)
                {
                    case "Automatico":
                        {
                            //Instanciando Detalle de la Orden de Compra
                            using (OrdenCompraDetalle ocd = new OrdenCompraDetalle(Convert.ToInt32(gvOrdenCompraDetalle.SelectedDataKey["Id"])))
                            {
                                //Validando que exista un Detalle
                                if (ocd.habilitar)
                                {
                                    //Instanciando Producto
                                    using (SAT_CL.Almacen.Producto prod = new SAT_CL.Almacen.Producto(ocd.id_producto))
                                    {
                                        //Validando que el Estatus 
                                        if (ocd.estatus == OrdenCompraDetalle.Estatus.Solicitada || ocd.estatus == OrdenCompraDetalle.Estatus.AbastecidaParcial)
                                        {
                                            //Asignando Cantidad Requerida y Precio
                                            lblProductoInv.Text = prod.descripcion + " [" + prod.sku + "]";
                                            txtCantidadInv.Text = Convert.ToDecimal(gvOrdenCompraDetalle.SelectedDataKey["Cantidad"]).ToString();
                                            txtPrecioInv.Text = ocd.precio_unitario.ToString();

                                            //Configurando Controles
                                            rbSerie.Checked = false;
                                            rbLote.Checked = true;
                                            configuraInventarioTemporal();

                                            //Mostrando Ventana Modal
                                            gestionaVentanaModal(lnkSurtir, "CantidadProducto");
                                        }
                                        //Estatus Registrado
                                        else if (ocd.estatus == OrdenCompraDetalle.Estatus.Registrada)

                                            //Mostrando Excepción
                                            ScriptServer.MuestraNotificacion(lnkSurtir, "El Detalle de la Orden tiene que estar 'Solicitada', para poderse Abastecer", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);

                                        //Estatus Cerrado
                                        else if (ocd.estatus == OrdenCompraDetalle.Estatus.Cerrada)

                                            //Mostrando Excepción
                                            ScriptServer.MuestraNotificacion(lnkSurtir, "El Detalle de la Orden ya esta 'Cerrada'", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);

                                        //Estatus Cerrado
                                        else if (ocd.estatus == OrdenCompraDetalle.Estatus.Cancelada)

                                            //Mostrando Excepción
                                            ScriptServer.MuestraNotificacion(lnkSurtir, "El Detalle de la Orden esta 'Cancelada', Imposible de Abastecer", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                                    }
                                }
                                else
                                    //Mostrando Excepción
                                    ScriptServer.MuestraNotificacion(lnkSurtir, "No existe el Detalle de la Orden", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                            }
                            break;
                        }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rbLote_CheckedChanged(object sender, EventArgs e)
        {
            //Invocando Método de Configuración
            configuraInventarioTemporal();
        }
        /// <summary>
        /// Evento Producido al Actualizar el GridView de Inventario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkActulizarInvTemp_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvInventarioTemp.DataKeys.Count > 0)
            {
                //Obteniendo Control
                LinkButton lkb = (LinkButton)sender;

                //Validando Comando
                switch (lkb.CommandName)
                {
                    case "Guardar":
                        {
                            //Declarando Objeto de Retorno
                            RetornoOperacion result = new RetornoOperacion();

                            try
                            {
                                //Obteniendo Controles
                                using (TextBox txtSerie = (TextBox)gvInventarioTemp.SelectedRow.FindControl("txtSerieE"),
                                        txtFechaCad = (TextBox)gvInventarioTemp.SelectedRow.FindControl("txtFechaCaducidadE"))
                                {
                                    //Validando que existan los Controles
                                    if (txtSerie != null && txtFechaCad != null)
                                    {
                                        //Obteniendo Fila por Actualizar
                                        foreach (DataRow dr in ((DataSet)Session["DS"]).Tables["Table3"].Select("Id = " + gvInventarioTemp.SelectedDataKey["Id"].ToString()))
                                        {
                                            //Obteniendo Fecha
                                            DateTime fecha;
                                            DateTime.TryParse(txtFechaCad.Text, out fecha);

                                            //Asignando Valores
                                            dr["Serie"] = txtSerie.Text;
                                            dr["FechaCaducidad"] = fecha == DateTime.MinValue ? "" : fecha.ToString("dd/MM/yyyy");
                                        }

                                        //Actualizando Cambios
                                        ((DataSet)Session["DS"]).Tables["Table3"].AcceptChanges();

                                        //Instanciando Mensaje
                                        result = new RetornoOperacion(0, "El Registro ha sido Actualizado Exitosamente", true);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                //Instanciando Excepción
                                result = new RetornoOperacion(ex.Message);
                            }

                            //Validando Operación Exitosa
                            if (result.OperacionExitosa)
                            {
                                //Inicializando Indices
                                Controles.InicializaIndices(gvInventarioTemp);
                                
                                //Cargando GridView
                                Controles.CargaGridView(gvInventarioTemp, ((DataSet)Session["DS"]).Tables["Table3"], "Id", "", true, 1);
                            }                            

                            //Mostrando Operación Exitosa
                            ScriptServer.MuestraNotificacion(lkb, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
                            break;
                        }
                    case "Editar":
                        {
                            //Seleccionando Fila para Edición
                            Controles.SeleccionaFila(gvInventarioTemp, sender, "lnk", true);

                            //Cargando GridView
                            Controles.CargaGridView(gvInventarioTemp, ((DataSet)Session["DS"]).Tables["Table3"], "Id", "", true, 1);
                            break;
                        }
                    case "Cancelar":
                        {
                            //Inicializando Indices
                            Controles.InicializaIndices(gvInventarioTemp);

                            //Cargando GridView
                            Controles.CargaGridView(gvInventarioTemp, ((DataSet)Session["DS"]).Tables["Table3"], "Id", "", true, 1);
                            break;
                        }
                }
            }
        }

        #endregion

        #region Eventos GridView "Factura Proveedor"

        /// <summary>
        /// Evento Producido al Aceptar el Ingreso de la Factura
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarFac_Click(object sender, EventArgs e)
        {
            //Limpiando Sesión
            Session["XML"] = null;

            //Abriendo Ventana Modal
            gestionaVentanaModal(btnAceptarFac, "FacturasLigadas");

            //Cargando Factura
            cargaFacturaLigada();

            //Cerrando Ventana Modal
            gestionaVentanaModal(btnAceptarFac, "ConfirmacionFactura");
        }
        /// <summary>
        /// Evento Producido al Cancelar/Cerrar el Ingreso de la Factura
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarFac_Click(object sender, EventArgs e)
        {
            //Obteniendo Boton
            Button btn = (Button)sender;
            
            //Ocultando Ventana Modal
            gestionaVentanaModal(btn, "ConfirmacionFactura");
        }
        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView "Factura Proveedor"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoFL_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando el Tamaño del GridView
            Controles.CambiaTamañoPaginaGridView(gvFacturasLigadas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), Convert.ToInt32(ddlTamanoFL.SelectedValue));
        }
        /// <summary>
        /// Evento Producido al Cambiar la Expresión de Ordenamiento del GridView "Factura Proveedor"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFacturasLigadas_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {
            //Asignando Expresión de Ordenamiento
            lblOrdenadoGrid.Text = Controles.CambiaSortExpressionGridView(gvFacturasLigadas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.SortExpression, true, 1);
        }
        /// <summary>
        /// Evento Producido al Cambiar el Tamaño de la Página del GridView "Factura Proveedor"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFacturasLigadas_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            //Cambiando Página
            Controles.CambiaIndicePaginaGridView(gvFacturasLigadas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.NewPageIndex);
        }
        /// <summary>
        /// Evento Producido al Exportar el Contenido del GridView "Factura Proveedor"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportar_Click(object sender, EventArgs e)
        {
            //Exportando Contenido
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), "Id");
        }

        #endregion

        #region Eventos GridView "Productos Inventario"

        /// <summary>
        /// Evento Producido al Cambiar el Indice de Página del GridView "Productos Inventario"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvProductosInventario_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Tamaño de Página
            Controles.CambiaTamañoPaginaGridView(gvProductosInventario, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), e.NewPageIndex, true, 1);
        }
        /// <summary>
        /// Evento Producido al Cambiar la Expresión de Ordenamiento del GridView "Productos Inventario"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvProductosInventario_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión de Ordenamiento
            lblOrdenadoProductoInv.Text = Controles.CambiaSortExpressionGridView(gvProductosInventario, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), e.SortExpression, true, 1);
        }
        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView "Productos Inventario"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoProductoInv_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando el Tamaño del GridView
            Controles.CambiaTamañoPaginaGridView(gvProductosInventario, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), Convert.ToInt32(ddlTamanoProductoInv.SelectedValue), true, 1);
        }
        /// <summary>
        /// Evento Producido al Actualizar los Valores
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkActualizar_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvProductosInventario.DataKeys.Count > 0)
            {
                //Obteniendo Control
                LinkButton lkb = (LinkButton)sender;

                //Validando Comando
                switch (lkb.CommandName)
                {
                    case "Cambiar":
                        {
                            //Seleccionando Fila
                            Controles.SeleccionaFila(gvProductosInventario, sender, "lnk", true);

                            //Cargando GridView
                            Controles.CargaGridView(gvProductosInventario, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), "Id", lblOrdenadoProductoInv.Text, true, 1);

                            //Instanciando Registro
                            using (Inventario inv = new Inventario(Convert.ToInt32(gvProductosInventario.SelectedDataKey["Id"])))
                            {
                                //Validando si existe el Registro
                                if (inv.habilitar)
                                {
                                    //Obteniendo Controles
                                    using (TextBox txtLote = (TextBox)gvProductosInventario.SelectedRow.FindControl("txtLoteE"))
                                    using (TextBox txtSerie = (TextBox)gvProductosInventario.SelectedRow.FindControl("txtSerieE"))
                                    using (TextBox txtFecCad = (TextBox)gvProductosInventario.SelectedRow.FindControl("txtFecCad"))
                                    {
                                        //Validando que existan los Controles
                                        if (txtSerie != null && txtFecCad != null && txtLote != null)
                                        {
                                            //Validando que sea una Sola Cantidad
                                            if (inv.cantidad > 1)
                                            {
                                                //Configurando Controles
                                                txtLote.Enabled = true;
                                                txtSerie.Enabled = false;
                                                txtFecCad.Enabled = true;
                                            }
                                            else if (inv.cantidad == 1)
                                            {
                                                //Configurando Controles
                                                txtSerie.Enabled =
                                                txtFecCad.Enabled = true;

                                                //Si no hay Serie pero si hay Lote
                                                if (inv.serie.Equals("") && !inv.lote.Equals(""))
                                                
                                                    //Habilitando Control
                                                    txtLote.Enabled = true;
                                                else
                                                    //Deshabilitando Control
                                                    txtLote.Enabled = false;
                                            }
                                        }
                                    }
                                }
                                else
                                    //Mostrando Resultado
                                    ScriptServer.MuestraNotificacion(lkb, "No existe el Registro de Inventario", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                            }
                            break;
                        }
                    case "Guardar":
                        {
                            //Declarando objeto de Retorno
                            RetornoOperacion result = new RetornoOperacion();

                            //Validando la Selección
                            if (gvProductosInventario.SelectedIndex != -1)
                            {
                                //Instanciando Registro
                                using (Inventario inv = new Inventario(Convert.ToInt32(gvProductosInventario.SelectedDataKey["Id"])))
                                {
                                    //Validando si existe el Registro
                                    if (inv.habilitar)
                                    {
                                        //Obteniendo Controles
                                        TextBox txtLote = (TextBox)gvProductosInventario.SelectedRow.FindControl("txtLoteE");
                                        TextBox txtSerie = (TextBox)gvProductosInventario.SelectedRow.FindControl("txtSerieE");
                                        TextBox txtFecCad = (TextBox)gvProductosInventario.SelectedRow.FindControl("txtFecCad");

                                        //Obteniendo Fecha
                                        DateTime fec_cad;
                                        DateTime.TryParse(txtFecCad.Text, out fec_cad);

                                        //Actualizando Serie y Fecha de Caducidad
                                        result = inv.ActualizarLoteSerieFechaCaducidadInventario(txtLote.Text, txtSerie.Text, fec_cad, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                        //Validando Operación Exitosa
                                        if (result.OperacionExitosa)
                                        {
                                            //Inicializando Indices
                                            Controles.InicializaIndices(gvProductosInventario);
                                            
                                            //Recargando Contenido
                                            cargaDetallesInventario();
                                        }
                                    }
                                    else
                                        //Mostrando Excepción
                                        result = new RetornoOperacion("No existe el Registro de Inventario");
                                }
                            }
                            else
                                //Mostrando Excepción
                                result = new RetornoOperacion("No hay un Registro Seleccionado");

                            //Mostrando Resultado
                            ScriptServer.MuestraNotificacion(lkb, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
                            break;
                        }
                    case "Cancelar":
                        {
                            //Quitamos la fila de modo de edicion 
                            Controles.InicializaIndices(gvProductosInventario);

                            //Cargando GridView
                            Controles.CargaGridView(gvProductosInventario, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), "Id", lblOrdenadoProductoInv.Text, true, 1);
                            break;
                        }
                }
            }
        }

        #endregion

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método que permite inicializar el aspecto del formulario
        /// </summary>
        private void inicializaForma()
        {
            //Invoca al método habilitaControles.
            habilitaControles();
            //Invoca al método habilitaMenu.
            habilitaMenu();
            //Invoca al método cargaCatalogo.
            cargaCatalogo();
            //Invoca al método inicializaValores.
            inicializaValores();
            //Inicializando GridView
            Controles.InicializaGridview(gvFacturasLigadas);
        }
        /// <summary>
        /// Método que permite habilitar los controles en base a cada estado de la apágina
        /// </summary>
        private void habilitaControles()
        {
            //Valida cada estado de la página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //En caso de que el estado sea Nuevo o Edicio
                case Pagina.Estatus.Nuevo:
                    {
                        //Habilitara los controles a exepción de txtCompaniaEmisor
                        btnAbrirModalDOC.Enabled = //Botones pertenecientes a la ventana Modal
                        gvOrdenCompraDetalle.Enabled =
                        btnSolicitarOrden.Enabled = false;
                        txtNoOrdenCompra.Enabled =
                        txtProveedor.Enabled =
                        txtFechaCompromiso.Enabled =
                        txtFechaEntrega.Enabled =
                        txtFechaSolicitud.Enabled =
                        txtAlmacen.Enabled =
                        ddlCondicionesPago.Enabled = true;
                        ddlFormaEntrega.Enabled = 
                        ddlTipoOrden.Enabled =
                        ddlMoneda.Enabled = 
                        txtProducto.Enabled = //Botones pertenecientes a la ventana Modal
                        txtCantidad.Enabled = //Botones pertenecientes a la ventana Modal
                        btnCancelar.Enabled =                          
                        btnGuardarDOC.Enabled = //Botones pertenecientes a la ventana Modal
                        btnGuardar.Enabled = true;
                        break;
                    }
                case Pagina.Estatus.Edicion:
                    {
                        //Habilitara los controles a exepción de txtCompaniaEmisor
                        gvOrdenCompraDetalle.Enabled =
                        txtNoOrdenCompra.Enabled =
                        txtProveedor.Enabled =
                        txtFechaCompromiso.Enabled =
                        txtFechaEntrega.Enabled =
                        txtFechaSolicitud.Enabled =
                        txtAlmacen.Enabled =
                        ddlCondicionesPago.Enabled =
                        ddlFormaEntrega.Enabled =
                        ddlTipoOrden.Enabled =
                        ddlMoneda.Enabled = 
                        txtProducto.Enabled = //Botones pertenecientes a la ventana Modal
                        txtCantidad.Enabled = //Botones pertenecientes a la ventana Modal
                        btnCancelar.Enabled =
                        btnSolicitarOrden.Enabled =
                        btnGuardarDOC.Enabled = //Botones pertenecientes a la ventana Modal
                        btnGuardar.Enabled = true;
                        break;
                    }
                //En caso de que el estado sea Lectura
                case Pagina.Estatus.Lectura:
                    {
                        //Deshabilitara los controles 
                        gvOrdenCompraDetalle.Enabled =
                        txtNoOrdenCompra.Enabled =
                        txtProveedor.Enabled =
                        txtFechaCompromiso.Enabled =
                        txtFechaEntrega.Enabled =
                        txtFechaSolicitud.Enabled =
                        txtAlmacen.Enabled =
                        ddlCondicionesPago.Enabled =
                        ddlFormaEntrega.Enabled =
                        ddlTipoOrden.Enabled =
                        ddlMoneda.Enabled =
                        btnCancelar.Enabled =
                        btnAbrirModalDOC.Enabled =
                        btnSolicitarOrden.Enabled =
                        btnGuardarDOC.Enabled = //Botones pertenecientes a la ventana Modal
                        txtProducto.Enabled = //Botones pertenecientes a la ventana Modal
                        txtCantidad.Enabled = //Botones pertenecientes a la ventana Modal
                        btnGuardar.Enabled = false;
                        break;
                    }
            }
        }
        /// <summary>
        /// Método que permite habilitar las opciones del ménu principal en base al estado de la página
        /// </summary>
        private void habilitaMenu()
        {
            //Valida cada estado de la página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //En caso de que el estado de la página sea Nuevo
                case Pagina.Estatus.Nuevo:
                    {
                        //Habilita y Deshabilita las opciones del ménu principal de la página
                        lkbNuevo.Enabled =
                        lkbAbrir.Enabled =
                        lkbGuardar.Enabled = true;
                        lkbImprimir.Enabled =
                        lkbEliminar.Enabled =
                        lkbEditar.Enabled =
                        lkbEtiqueta.Enabled =
                        lkbBitacora.Enabled =
                        lkbReferencias.Enabled =
                        lkbArchivos.Enabled = 
                        lkbFacturaProveedor.Enabled = false;
                        btnGuardar.Enabled = true;
                        btnGuardarDOC.Enabled =
                        btnSolicitarOrden.Enabled =
                        btnAbrirModalDOC.Enabled = false;  
                        break;
                    }
                case Pagina.Estatus.Edicion:
                    {
                        //Habilita y Deshabilita las opciones del ménu principal de la página
                        lkbNuevo.Enabled =
                        lkbAbrir.Enabled =
                        lkbGuardar.Enabled =
                        lkbImprimir.Enabled =
                        lkbEliminar.Enabled = true;
                        lkbEditar.Enabled =
                        lkbEtiqueta.Enabled =
                        lkbBitacora.Enabled =
                        lkbReferencias.Enabled = true;
                        lkbArchivos.Enabled = false;
                        lkbFacturaProveedor.Enabled =
                        btnAbrirModalDOC.Enabled =
                        btnGuardarDOC.Enabled =
                        btnSolicitarOrden.Enabled =
                        btnGuardar.Enabled = true;
                        break;
                    }
                case Pagina.Estatus.Lectura:
                    {
                        //Habilita y Deshabilita las opciones del ménu principal de la página
                        lkbNuevo.Enabled =
                        lkbAbrir.Enabled = true;
                        lkbGuardar.Enabled = false;
                        lkbImprimir.Enabled =
                        lkbEliminar.Enabled =
                        lkbEditar.Enabled =
                        lkbEtiqueta.Enabled =
                        lkbBitacora.Enabled =
                        lkbReferencias.Enabled = true;
                        lkbArchivos.Enabled = false;
                        lkbFacturaProveedor.Enabled = true;
                        btnAbrirModalDOC.Enabled =
                        btnGuardarDOC.Enabled =
                        btnSolicitarOrden.Enabled =
                        btnGuardar.Enabled = false;
                        break;
                    }
            }
        }
        /// <summary>
        /// Método que permite la carga de catalogos a los controles DrodDownList
        /// </summary>
        private void cargaCatalogo()
        {
            //Carga los valores al DropDownList Estatus
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlEstatus, "", 3129);
            //Carga los valores al DropDownList Estatus de la ventana modal
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlEstatusDetalle,"",3134);
            //Carga los valores al DropDownList ddlTipoOrden
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipoOrden, "", 3130);
            //Carga los valores al DropDownList ddlFromaEntrega
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlFormaEntrega, "", 3131);
            //Carga los valores al DropDownList ddlCondicionesPago
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlCondicionesPago, "", 3132);
            //Carga los valores al DropDownList ddlMoneda
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlMoneda, "", 11);
            //Carga los Valores de Tamaño del GridView
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoFL, "", 26);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoProductoInv, "", 26);
        }
        /// <summary>
        /// Método que permite inicializar los valores de los controles en base a cada estado de la página
        /// </summary>
        private void inicializaValores()
        {
            //Valida cada estadod de la página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //En caso de que el estado de la pagina sea nuevo.
                case Pagina.Estatus.Nuevo:
                    {
                        //Limpia los controles del formulario.
                        lblNoOrdenCompra.Text = "Por Asignar";
                        txtNoOrdenCompra.Text = 
                        txtAlmacen.Text = 
                        txtProveedor.Text = "";
                        txtFechaCompromiso.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");
                        txtFechaEntrega.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");
                        txtFechaSolicitud.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");
                        
                        //Limpia los controles de la ventana modal
                        limpiaControlesModalOrdenCompraDetalle();
                        
                        //Inicializa los valores del GridViewOrdencompraDetalle
                        Controles.InicializaGridview(gvOrdenCompraDetalle);
                        
                        //Invoca al constructor de la clase CompaniaEmisorReceptor para obtener el nombre de la compañia
                        using (SAT_CL.Global.CompaniaEmisorReceptor emisor = new SAT_CL.Global.CompaniaEmisorReceptor(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                        {
                            //Asigina al txtCompania el nombre de la compañia del usuario.
                            txtCompaniaEmisor.Text = emisor.nombre + " ID:" + emisor.id_compania_emisor_receptor.ToString();
                        }

                        //Inicializando Totales
                        txtSubTotal.Text =
                        txtImpTrasladado.Text =
                        txtImpRetenido.Text =
                        txtTotal.Text = string.Format("{0:0.000000}", 0);
                        txtTasaImpTrasladado.Text =
                        txtTasaImpRetenido.Text = string.Format("{0:0.00}", 0);
                        break;
                    }
                //En caso de que el estado de la página sea edición o Lectura
                case Pagina.Estatus.Edicion:
                case Pagina.Estatus.Lectura:
                    {
                        //Invoca al constructor de la clase OrdenCompra y asigna como paramétro el valor de la variable de session id_registro
                        using (SAT_CL.Almacen.OrdenCompra oc = new SAT_CL.Almacen.OrdenCompra((int)Session["id_registro"]))
                        {
                            //Valida que exista el registro
                            if (oc.id_orden_compra > 0)
                            {
                                //Asigna al label Id el identificador de la orden de compra.
                                lblNoOrdenCompra.Text = oc.no_orden_compra.ToString();
                                //Invoca al constructor de la clase CompaniaEmisor para obtener el nombre de las compañias.
                                using (SAT_CL.Global.CompaniaEmisorReceptor emisor = new SAT_CL.Global.CompaniaEmisorReceptor(oc.id_compania_emisor))
                                {
                                    //Valida que el registro exista
                                    if (emisor.id_compania_emisor_receptor > 0)
                                        //Asigna al control los nombres y id de cada compañia
                                        txtCompaniaEmisor.Text = string.Format("{0}   ID:{1}", emisor.nombre, emisor.id_compania_emisor_receptor);
                                }
                                //Invoca al construcotr de la clase CompaniaEmisor para obtener el nombre de los proveedores
                                using (SAT_CL.Global.CompaniaEmisorReceptor receptor = new SAT_CL.Global.CompaniaEmisorReceptor(oc.id_proveedor))
                                {
                                    //Valida que exista el registro
                                    if (receptor.id_compania_emisor_receptor > 0)
                                        //Asigna al control los nombres y id de cada proveedor
                                        txtProveedor.Text = string.Format("{0}   ID:{1}", receptor.nombre, receptor.id_compania_emisor_receptor);
                                }
                                //Instancia Almacen
                                using (SAT_CL.Almacen.Almacen almacen = new SAT_CL.Almacen.Almacen(oc.id_almacen))
                                {
                                    //Valida que exista el registro
                                    if (almacen.id_almacen > 0)
                                        //Asigna al control los nombres y id de cada almacen
                                        txtAlmacen.Text = string.Format("{0}   ID:{1}", almacen.descripcion, almacen.id_almacen);
                                }

                                //Asignando Valores
                                txtNoOrdenCompra.Text = oc.no_documento_proveedor;
                                txtFechaCompromiso.Text = oc.fecha_compromiso.ToString("dd/MM/yyyy HH:mm");
                                txtFechaEntrega.Text = oc.fecha_entrega.ToString("dd/MM/yyyy HH:mm");
                                txtFechaSolicitud.Text = oc.fecha_solicitud.ToString("dd/MM/yyyy HH:mm");
                                ddlCondicionesPago.SelectedValue = oc.id_condiciones_pago.ToString();
                                ddlEstatus.SelectedValue = oc.id_estatus.ToString();
                                ddlFormaEntrega.SelectedValue = oc.id_forma_entrega.ToString();
                                ddlTipoOrden.SelectedValue = oc.id_tipo_orden.ToString();
                                ddlMoneda.SelectedValue = oc.id_moneda.ToString();

                                //Cargando Detalles
                                cargaDetallesOrdenCompra();

                                //Asignando Totales
                                txtSubTotal.Text = string.Format("{0:0.000000}", oc.subtotal);
                                txtImpTrasladado.Text = string.Format("{0:0.000000}", oc.imp_trasladado);
                                txtImpRetenido.Text = string.Format("{0:0.000000}", oc.imp_retenido);
                                txtTotal.Text = string.Format("{0:0.000000}", oc.total);

                                //Calculando Tasas
                                txtTasaImpTrasladado.Text = (oc.imp_trasladado == 0.000000M ? 0.00M : (oc.imp_trasladado * 100) / oc.subtotal).ToString("0.00");
                                txtTasaImpRetenido.Text = (oc.imp_retenido == 0.000000M ? 0.00M : (oc.imp_retenido * 100) / oc.subtotal).ToString("0.00");
                            }
                        }

                        break;
                    }
            }

        }
        /// <summary>
        /// Método encargado de Cargar los Detalles de la Orden de Compra
        /// </summary>
        private void cargaDetallesOrdenCompra()
        {
            //Invoca al dataset para inicializar los valores del gridview si existe en relación a una orden de compra
            using (DataTable dtOrdenCompraDetalle = SAT_CL.Almacen.OrdenCompraDetalle.CargaDetallesOrdenCompra((int)Session["id_registro"]))
            {
                //Valida si existen los datos del datase
                if (Validacion.ValidaOrigenDatos(dtOrdenCompraDetalle))
                {
                    //Si existen, carga los valores del datatable al gridview
                    gvOrdenCompraDetalle.PageSize = dtOrdenCompraDetalle.Rows.Count;
                    Controles.CargaGridView(gvOrdenCompraDetalle, dtOrdenCompraDetalle, "Id-Cantidad-Disponibles", "");
                    //Asigna a la variable de sesion los datos del dataset invocando al método AñadeTablaDataSet
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtOrdenCompraDetalle, "Table");
                }
                //Si no existen
                else
                {
                    //Inicializa el gridView 
                    Controles.InicializaGridview(gvOrdenCompraDetalle);
                    //Elimina los datos del dataset si se realizo una consulta anterior
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                }

                //Invocando Método de Suma
                sumaTotalesDetalles();
            }
        }
        /// <summary>
        /// Método que permite limpiar el contenido de los controles de la ventana modal OrdenCompraDetalle
        /// </summary>
        private void limpiaControlesModalOrdenCompraDetalle()
        {
            //Limpia el contenido de los botones pertenecientes a la ventana Modal
            txtProducto.Text =
            txtPrecioUnitario.Text =
            txtCantidad.Text = 
            txtTExistencia.Text =
            txtTRequerido.Text = 
            txtTPorEntregar.Text = "";
        }
        /// <summary>
        /// Método que permite almacenar los datos que se ingresaron en los controles del formulario
        /// </summary>
        private void guardarOrdenCompra()
        {
            //Creació del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();

            //Obteniendo Fechas
            DateTime fec_compromiso, fec_solicitud, fec_entrega;
            DateTime.TryParse(txtFechaCompromiso.Text, out fec_compromiso);
            DateTime.TryParse(txtFechaSolicitud.Text, out fec_solicitud);
            DateTime.TryParse(txtFechaEntrega.Text, out fec_entrega);

            //Obteniendo Totales
            decimal subTotal = Convert.ToDecimal(txtSubTotal.Text);
            decimal trasladado = Convert.ToDecimal(txtImpTrasladado.Text);
            decimal retenido = Convert.ToDecimal(txtImpRetenido.Text);
            decimal total = Convert.ToDecimal(txtTotal.Text);

            //Invoca al método validaFecha  y asigna el resultado del método al objeto retorno.
            retorno = validaFecha();
            //Valida si el resultado del método se realizo correctamente (La validación de las Fechas)
            if (retorno.OperacionExitosa)
            {
                //Valida cada estado de la página
                switch ((Pagina.Estatus)Session["estatus"])
                {
                    //En caso de que el estado de la página sea nuevo
                    case Pagina.Estatus.Nuevo:
                        {
                            //Asigna al objeto retorno los datos ingresados invocando al método inserción de la clase ordenCompra
                            retorno = SAT_CL.Almacen.OrdenCompra.InsertarOrdenCompra(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, txtNoOrdenCompra.Text,
                                                                (SAT_CL.Almacen.OrdenCompra.TipoOrden)Convert.ToByte(ddlTipoOrden.SelectedValue), Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtProveedor.Text, "ID:", 1)),
                                                                Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtAlmacen.Text, "ID:", 1)), (SAT_CL.Almacen.OrdenCompra.FormaEntrega)Convert.ToByte(ddlFormaEntrega.SelectedValue),
                                                                fec_solicitud, fec_entrega, fec_compromiso, (SAT_CL.Almacen.OrdenCompra.CondicionesPago)Convert.ToByte(ddlCondicionesPago.SelectedValue),0,
                                                                Convert.ToByte(ddlMoneda.SelectedValue), subTotal, trasladado, retenido, total, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                            break;
                        }
                    //En caso de que el estado de la página sea edicion
                    case Pagina.Estatus.Edicion:
                        {
                            //Invoca al constructor de la clase OrdenCompra para poder instanciar sus  métodos 
                            using (SAT_CL.Almacen.OrdenCompra oc = new SAT_CL.Almacen.OrdenCompra((int)Session["id_registro"]))
                            {
                                if (oc.id_orden_compra > 0)
                                {
                                    //Asigna al objeto retorno los datos ingresados en los controles invocando el método de edición de la clase ordenCompra
                                    retorno = oc.EditarOrdenCompra(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, txtNoOrdenCompra.Text, (SAT_CL.Almacen.OrdenCompra.Estatus)Convert.ToByte(ddlEstatus.SelectedValue),
                                                                (SAT_CL.Almacen.OrdenCompra.TipoOrden)Convert.ToByte(ddlTipoOrden.SelectedValue), Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtProveedor.Text, "ID:", 1)),
                                                                Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtAlmacen.Text, "ID:", 1)), (SAT_CL.Almacen.OrdenCompra.FormaEntrega)Convert.ToByte(ddlFormaEntrega.SelectedValue),
                                                                fec_solicitud, fec_entrega, fec_compromiso, (SAT_CL.Almacen.OrdenCompra.CondicionesPago)Convert.ToByte(ddlCondicionesPago.SelectedValue), 0,
                                                                Convert.ToByte(ddlMoneda.SelectedValue), subTotal, trasladado, retenido, total, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                }
                            }
                            break;
                        }
                }
                //Valida si la inserción a la base de datos se realizo correctamente
                if (retorno.OperacionExitosa)
                {
                    //A la variable de sessión estatus le asigna el estado de la pagina en modo lectura
                    Session["estatus"] = Pagina.Estatus.Edicion;
                    //A la variable de session id_registro le asigna el valor insertado en la tabla CostoCombustible
                    Session["id_registro"] = retorno.IdRegistro;
                    //Asigna al lblIdOrdenCompraDetale el valor del id.
                    //Invoca al método inicializa forma
                    inicializaForma();
                }
            }

            //Manda un mensaje dependiendo de la validación de la operación
            ScriptServer.MuestraNotificacion(this, retorno, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Método que permite guardar un detalle de orden de compra. 
        /// </summary>
        private void guardarOrdenCompraDetalle()
        {
            //Creacion del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();

            //Instancia a la clase orden de compra detalle para validar si se realizara una insercion o una edición de datos
            using (SAT_CL.Almacen.OrdenCompra oc = new SAT_CL.Almacen.OrdenCompra((int)Session["id_registro"]))
            {
                //Validando que exista un Registro Seleccionado
                if (gvOrdenCompraDetalle.SelectedIndex == -1)
                
                    //Asigna al objeto retorno los valores de los controles de la modal invocando al método de inserción de la clase ordencompraDetalle
                    retorno = SAT_CL.Almacen.OrdenCompraDetalle.InsertarOrdenCompraDetalle(Convert.ToInt32(Session["id_registro"]), Convert.ToDecimal(txtCantidad.Text),
                                                Convert.ToDecimal(txtPrecioUnitario.Text), Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtProducto.Text, "ID:", 1)), 
                                                ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                else
                {
                    //Valida que registro de la tabla se quiere editar
                    using (SAT_CL.Almacen.OrdenCompraDetalle ocd = new SAT_CL.Almacen.OrdenCompraDetalle(Convert.ToInt32(gvOrdenCompraDetalle.SelectedDataKey["Id"])))
                    {
                        //Valida que exista el registro
                        if (ocd.id_orden_compra_detalle > 0)
                            
                            //Asigna al objeto retorno los valores de los controles invocando el método editar de la clase ordenCompradetalle
                            retorno = ocd.EditarOrdenCompraDetalle(Convert.ToInt32(Session["id_registro"]), ocd.estatus,
                                                Convert.ToDecimal(txtCantidad.Text), Convert.ToDecimal(txtPrecioUnitario.Text), Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtProducto.Text, "ID:", 1)),
                                                ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                    }
                }
            }

            //Valida que se realizo la operación
            if (retorno.OperacionExitosa)
            {
                //Invoca al método que limpia los controles de la ventana modal
                limpiaControlesModalOrdenCompraDetalle();
                //Inicializa la forma.
                inicializaForma();
                //Invoca método de carga
                cargaDetallesOrdenCompra();
            }

            //Manda un mensaje dependiendo de la validación de la operación
            ScriptServer.MuestraNotificacion(this, retorno, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Método encargado de Sumar los Totales de los Detalles
        /// </summary>
        private void sumaTotalesDetalles()
        {
            //Validando que existan Registros
            if (Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table")))
            {
                //Sumando Totales
                gvOrdenCompraDetalle.FooterRow.Cells[5].Text = string.Format("{0:0.000000}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(CantidadInicial)", "")));
                gvOrdenCompraDetalle.FooterRow.Cells[6].Text = string.Format("{0:0.000000}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(CantidadAbastecida)", "")));
                gvOrdenCompraDetalle.FooterRow.Cells[7].Text = string.Format("{0:0.000000}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Cantidad)", "")));
                gvOrdenCompraDetalle.FooterRow.Cells[10].Text = string.Format("{0:C6}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Total)", "")));
            }
            else
            {
                //Inicializando Totales
                gvOrdenCompraDetalle.FooterRow.Cells[5].Text =
                gvOrdenCompraDetalle.FooterRow.Cells[6].Text =
                gvOrdenCompraDetalle.FooterRow.Cells[7].Text = string.Format("{0:0.000000}", 0);
                gvOrdenCompraDetalle.FooterRow.Cells[10].Text = string.Format("{0:C2}", 0);
            }
        }
        /// <summary>
        /// Método encargado de Cargar los Registros de Inventario de cada Detalle
        /// </summary>
        private void cargaDetallesInventario()
        {
            //Obteniendo Detalles de Inventario
            using (DataTable dtInventario = SAT_CL.Almacen.OrdenCompraDetalle.ObtieneDetalleInventario(Convert.ToInt32(gvOrdenCompraDetalle.SelectedDataKey["Id"])))
            {
                //Validando si Existen Registros
                if (Validacion.ValidaOrigenDatos(dtInventario))
                {
                    //Cargando GridView
                    Controles.CargaGridView(gvProductosInventario, dtInventario, "Id", lblOrdenadoProductoInv.Text, true, 1);

                    //Añadiendo Tabla a Sesión
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtInventario, "Table2");
                }
                else
                {
                    //Inicializando GridView
                    Controles.InicializaGridview(gvProductosInventario);

                    //Eliminando Tabla de Sesión
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table2");
                }
            }
        }
        /// <summary>
        /// Métod que valida la fecha de Solicitid y la fecha de compromiso
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion validaFecha()
        {
            //Creación del objeto retorno con valor 1 al constructor de la clase
            RetornoOperacion retorno = new RetornoOperacion(1);
            //Compara los datos encontrados en los controles de fecha inicio y fecha fin(si la fechaInicio es menor a fechaFin y el resultado de la comparacion es a 0)
            if (Convert.ToDateTime(txtFechaSolicitud.Text).CompareTo(Convert.ToDateTime(txtFechaCompromiso.Text)) > 0)
            {
                //Al objeto retorno se le asigna un mensaje de error en la validación de las fechas.
                retorno = new RetornoOperacion(" Fecha de Solicitud debe ser MENOR que Fecha Fecha Compromiso.");
            }
            //Retorna el resultado al método 
            return retorno;
        }
        /// <summary>
        /// Método encargado de Establecer la Configuración del Inventario Temporal
        /// </summary>
        private void configuraInventarioTemporal()
        {
            //Si esta marcado el Control
            if (rbSerie.Checked)
            {
                //Mostrando Control
                gvInventarioTemp.Visible = true;

                //Declarando variables Auxiliares
                decimal cantidad = Convert.ToDecimal(txtCantidadInv.Text == "" ? "0" : txtCantidadInv.Text);
                int contador = 0;

                //Validando que existe la Cantidad por Abastecer
                if (cantidad > 0)
                {
                    //Validando que la Cantidad no Exceda el Valor Permitido
                    if (Convert.ToDecimal(gvOrdenCompraDetalle.SelectedDataKey["Cantidad"]) < cantidad)
                    {
                        //Asignando Cantidad Permitida
                        cantidad = Convert.ToDecimal(gvOrdenCompraDetalle.SelectedDataKey["Cantidad"]);
                        txtCantidadInv.Text = gvOrdenCompraDetalle.SelectedDataKey["Cantidad"].ToString();

                        //Mostrando Excepción
                        ScriptServer.MuestraNotificacion(rbSerie, string.Format("No puede exceder la Cantidad '{0:0.00}'", Convert.ToDecimal(gvOrdenCompraDetalle.SelectedDataKey["Cantidad"])), ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                    }

                    //Inicializando Valores
                    txtLote.Text = 
                    txtFecCad.Text = "";

                    //Deshabilitando Control
                    txtCantidadInv.Enabled = 
                    txtLote.Enabled = 
                    txtFecCad.Enabled = false;

                    //Creando Tabla Temporal
                    using (DataTable dtInventarioTemp = new DataTable())

                    //Creando Columna Autoincrementable
                    using (DataColumn column = new DataColumn("Id"))
                    {
                        //Configurando Columna
                        column.DataType = System.Type.GetType("System.Int32");
                        column.AutoIncrement = true;
                        column.AutoIncrementSeed = 1;
                        column.AutoIncrementStep = 1;

                        //Añadiendo Columnas
                        dtInventarioTemp.Columns.Add(column);
                        dtInventarioTemp.Columns.Add("Serie", typeof(string));
                        dtInventarioTemp.Columns.Add("FechaCaducidad", typeof(string));

                        //Inicializando Ciclo
                        while (contador < cantidad)
                        {
                            //Añadiendo Registros
                            dtInventarioTemp.Rows.Add(contador + 1, "", null);

                            //Incrementando Contador
                            contador++;
                        }

                        //Validando que existen Registros
                        if (Validacion.ValidaOrigenDatos(dtInventarioTemp))
                        {
                            //Cargando GridView
                            Controles.CargaGridView(gvInventarioTemp, dtInventarioTemp, "Id", "", true, 1);

                            //Añadiendo Tabla a Sesión
                            Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtInventarioTemp, "Table3");
                        }
                        else
                        {
                            //Cargando GridView
                            Controles.InicializaGridview(gvInventarioTemp);

                            //Eliminando Tabla de Sesión
                            Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table3");
                        }
                    }

                }
                else
                {
                    //Configurando Controles
                    gvInventarioTemp.Visible = false;
                    txtCantidadInv.Enabled = 
                    txtLote.Enabled = 
                    txtFecCad.Enabled = true;
                    rbSerie.Checked = false;
                    rbLote.Checked = true;
                    txtCantidadInv.Text = gvOrdenCompraDetalle.SelectedDataKey["Cantidad"].ToString();

                    //Mostrando Excepción
                    ScriptServer.MuestraNotificacion(rbSerie, "No puede especificar los detalles con Cantidad '0'", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                }
            }
            else if(rbLote.Checked)
            {
                //Ocultando GridView
                gvInventarioTemp.Visible = false;

                //Inicializando Valores
                txtLote.Text =
                txtFecCad.Text = "";

                //Habilitando Control
                txtCantidadInv.Enabled =
                txtFecCad.Enabled = 
                txtLote.Enabled = true;
            }
        }
        /// <summary>
        ///  Método que permie la consulta del historial de modificicaciones realizadas a un registro 
        /// </summary>
        /// <param name="idRegistro">ID que permite identificar un registro de OrdenCompra</param>
        /// <param name="idTabla">Identificador de la tabla OrdenCompra</param>
        /// <param name="Titulo">Encabezado de la ventana Bitacora</param>
        private void inicializaBitacora(string idRegistro, string idTabla, string Titulo)
        {
            //Creación de la variable url, que almacena una ruta de ubicación de la ventana, que contendra la bitacora de un registros de  OrdenCompra.
            string url = Cadena.RutaRelativaAAbsoluta("~/Almacen/OrdenCompra.aspx",
                                                                "~/Accesorios/BitacoraRegistro.aspx?idT=" + idTabla + "&idR=" + idRegistro + "&tB=" + Titulo);
            //Variable que almacena la resolucion de la ventana bitacora
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Invoca el método de la clase ScriptServer que abrira una nueva ventana acorde a una ubicacion, un titulo de la ventana, una configuracion de dimensiones.
            ScriptServer.AbreNuevaVentana(url, "Bitacora Orden Compra", configuracion, Page);
        }
        /// <summary>
        /// Método que permite la busqueda de registros en la base de datos pertenecientes a la tabla OrdenCompra
        /// </summary>
        /// <param name="idTabla">Id que identifica a la tabla OrdenCompra registrada en la base de datos</param>
        /// <param name="idCompania"> Id que identifica a una compañia</param>
        private void inicializaAperturaRegistro(int idTabla, int idCompania)
        {
            //Asigna a la variable de session id_tabla el valor del parametro idTabla
            Session["id_tabla"] = idTabla;
            //Asigna valores a la variable url con la ruta de ubicacion de la ventana Abrir registros con los datos obtenidos de la tabla OrdenCompra
            string url = Cadena.RutaRelativaAAbsoluta("~/Almacen/OrdenCompra.aspx", "~/Accesorios/AbrirRegistro.aspx?P1=" + idCompania.ToString());
            //Define las dimensiones de la ventana Abrir registros de OrdenCompra
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Invoca al método de la variable ScripServer que abrira una nueva ventana con los registros pertenecientes a la tabla OrdenCompra
            ScriptServer.AbreNuevaVentana(url, "Abrir Registro Orden Compra", configuracion, Page);
        }
        /// <summary>
        /// Método encargado de inicializr la referencias a un registro
        /// </summary>
        /// <param name="id_registro">Id que permite identificar un registro de la tabla OrdenCompra</param>
        /// <param name="id_tabla">Id que permite identificar a la tabla OrdenCompra en la base de datos</param>
        /// <param name="id_compania">Id que permite identificar a que compania pertenece </param>
        private void inicializaReferenciaRegistro(string id_registro, string id_tabla, string id_compania)
        {
            //Asigna a la variable urlDestino una ruta de ubucación de la ventana de Referencia de la tabla OrdenCompra
            string urlDestino = Cadena.RutaRelativaAAbsoluta("~/Almacen/OrdenCompra.aspx", "~/Accesorios/ReferenciaRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idC=" + id_compania);
            //Invoca al método de la clase ScripServer que abrira una nueva ventana con los valores de Referencia de la tabla OrdenCompra
            ScriptServer.AbreNuevaVentana(urlDestino, "Referencia de Registros OrdenCompra", 800, 500, false, false, false, true, true, Page);
        }
        /// <summary>
        /// Método Privado encargado de Gestionar las Ventanas Modales
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="comando"></param>
        private void gestionaVentanaModal(Control sender, string comando)
        {
            //Validando Comando
            switch (comando)
            {
                case "DetalleOrdenCompra":
                    {
                        //Mostrando Ventana Modal
                        ScriptServer.AlternarVentana(sender, "CerrarVentana", "contenidoOrdenCompraDetalle", "ordenCompraDetalle");
                        break;
                    }
                case "CantidadProducto":
                    {
                        //Mostrando Ventana Modal
                        ScriptServer.AlternarVentana(sender, "CantidadProducto", "contenedorVentanaCantidadProducto", "ventanaCantidadProducto");
                        break;
                    }
                case "FacturasLigadas":
                    {
                        //Mostrando Ventana Modal
                        ScriptServer.AlternarVentana(sender, "FacturasLigadas", "contenedorVentanaFacturasLigadas", "ventanaFacturasLigadas");
                        break;
                    }
                case "ConfirmacionFactura":
                    {
                        //Mostrando Ventana Modal
                        ScriptServer.AlternarVentana(sender, "ConfirmacionFactura", "contenedorVentanaConfirmacionFactura", "ventanaConfirmacionFactura");
                        break;
                    }
                case "ProductoInventario":
                    {
                        //Mostrando Ventana Modal
                        ScriptServer.AlternarVentana(sender, "ProductoInvetario", "contenedorVentanaEdicionInventario", "ventanaEdicionInventario");
                        break;
                    }
                case "FacturaDiferencia":
                    {
                        //Mostrando Ventana Modal
                        ScriptServer.AlternarVentana(sender, "FacturaDiferencia", "contenedorVentanaFacturacionDiferencia", "ventanaFacturacionDiferencia");
                        break;
                    }
            }
        }

        #region Métodos "Facturas Ligadas"

        /// <summary>
        /// Método encargado de Cargar la Factura Ligada a la Orden
        /// </summary>
        private void cargaFacturaLigada()
        {
            //Validando Estatus de Página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {
                        //Inicializando GridView
                        Controles.InicializaGridview(gvFacturasLigadas);

                        //Eliminando Tabla de Sesión
                        Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
                        break;
                    }
                case Pagina.Estatus.Lectura:
                    {
                        //Instanciando Factura
                        using (DataTable dtFactura = SAT_CL.Almacen.OrdenCompra.ObtieneFacturaOrdenCompra(Convert.ToInt32(Session["id_registro"])))
                        {
                            //Validando que existen Registros
                            if (Validacion.ValidaOrigenDatos(dtFactura))
                            {
                                //Cargando GridView
                                Controles.CargaGridView(gvFacturasLigadas, dtFactura, "Id", lblOrdenadoGrid.Text, true, 1);

                                //Añadiendo Tabla a Sesión
                                Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtFactura, "Table1");
                            }
                            else
                            {
                                //Inicializando GridView
                                Controles.InicializaGridview(gvFacturasLigadas);

                                //Eliminando Tabla de Sesión
                                Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
                            }
                        }
                        break;
                    }
            }
        }
        /// <summary>
        /// Método privado encargado de Validar la Factura en formato XML
        /// </summary>
        /// <param name="mensaje">Mensaje de Operación</param>
        /// <returns></returns>
        private RetornoOperacion validaFacturaXML()
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Limpiando Mensaje
            string mensaje = "";

            //Validando que exista un Archivo en Sessión
            if (Session["XML"] != null)
            {
                //Declarando Documento XML
                XmlDocument doc = (XmlDocument)Session["XML"];

                //Validando que exista el Documento
                if (doc != null)
                {
                    try
                    {
                        //Instanciando Compania
                        using (SAT_CL.Global.CompaniaEmisorReceptor emi = SAT_CL.Global.CompaniaEmisorReceptor.ObtieneInstanciaCompaniaRFC(doc.DocumentElement["cfdi:Emisor"].Attributes["rfc"].Value.ToUpper(), ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                        {
                            //Declarando Variable Auxiliar
                            int idProveedorEmisor = 0;

                            //Asignando Emisor
                            idProveedorEmisor = emi.id_compania_emisor_receptor;

                            //Instanciando Proveedor
                            result = new RetornoOperacion(idProveedorEmisor);

                            //Validando que el RFC sea igual
                            if (idProveedorEmisor > 0)
                            {
                                //Instanciando Compania
                                using (SAT_CL.Global.CompaniaEmisorReceptor cer = new SAT_CL.Global.CompaniaEmisorReceptor(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                                {
                                    //Validando que el RFC sea igual
                                    if (cer.rfc.ToUpper() == doc.DocumentElement["cfdi:Receptor"].Attributes["rfc"].Value.ToUpper())
                                    {
                                        //Instanciando XSD de validación
                                        using (EsquemasFacturacion ef = new EsquemasFacturacion(doc["cfdi:Comprobante"].Attributes["version"].Value))
                                        {
                                            //Validando que exista el XSD
                                            if (ef.id_esquema_facturacion != 0)
                                            {
                                                //Declarando variables Auxiliares
                                                bool addenda;

                                                //Obteniendo XSD
                                                string[] esquemas = EsquemasFacturacion.CargaEsquemasPadreYComplementosCFDI(ef.version, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, out addenda);

                                                //Validando que Existen Addendas
                                                if (doc.DocumentElement["cfdi:Addenda"] != null)

                                                    //Removiendo Addendas
                                                    doc.DocumentElement["cfdi:Addenda"].RemoveAll();

                                                //Obteniendo Validación
                                                bool value = TSDK.Base.Xml.ValidaXMLSchema(doc.InnerXml, esquemas, out mensaje);

                                                //Instanciando Resultado
                                                result = new RetornoOperacion(mensaje, value);
                                            }
                                        }
                                    }
                                    else
                                        //Asignando Negativa el Objeto de retorno
                                        result = new RetornoOperacion("El RFC de la factura no coincide con el Receptor");
                                }
                            }
                            else
                                //Asignando Negativa el Objeto de retorno
                                result = new RetornoOperacion("El RFC de la factura no coincide con el Emisor");
                        }
                    }
                    catch (Exception e)
                    {
                        //Asignando Negativa el Objeto de retorno
                        result = new RetornoOperacion(e.Message);
                    }
                }
                else
                    //Mensaje de Error
                    result = new RetornoOperacion("No se ha podido cargar el Archivo");
            }
            else
                //Mensaje de Error
                result = new RetornoOperacion("No se ha podido localizar el Archivo");

            //Devolviendo Resultado Obtenido
            return result;
        }

        /// <summary>
        /// Método Público encargado de Guardar la Factura apartir de un XML
        /// </summary>
        private void guardaXML()
        {
            //Declarando Objeto de Operación
            RetornoOperacion result = new RetornoOperacion();

            //Instanciando Orden de Compra
            using (SAT_CL.Almacen.OrdenCompra oc = new SAT_CL.Almacen.OrdenCompra(Convert.ToInt32(Session["id_registro"])))
            {
                //Validando que Exista la Orden
                if (oc.habilitar)
                {
                    //Validando XML
                    result = validaFacturaXML();

                    //Validando Resultado
                    if (result.OperacionExitosa)
                    {
                        try
                        {
                            //Declarando Documento XML
                            XmlDocument doc = (XmlDocument)Session["XML"];

                            //Declarando variables de Montos
                            decimal total_p, subtotal_p, descuento_p, traslado_p, retenido_p, monto_tc;

                            //Obteniendo Valores
                            obtieneCantidades(doc, out total_p, out subtotal_p, out descuento_p, out traslado_p, out retenido_p, out monto_tc);

                            //Instanciando Emisor-Compania
                            using (SAT_CL.Global.CompaniaEmisorReceptor emisor = SAT_CL.Global.CompaniaEmisorReceptor.ObtieneInstanciaCompaniaRFC(doc["cfdi:Comprobante"]["cfdi:Emisor"].Attributes["rfc"].Value, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                            {
                                //Validando que coincida el RFC del Emisor
                                if (emisor.id_compania_emisor_receptor > 0)
                                {
                                    //
                                    if (Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtProveedor.Text, "ID:", 1)) == emisor.id_compania_emisor_receptor)
                                    {
                                        //Instanciando Emisor-Compania
                                        using (SAT_CL.Global.CompaniaEmisorReceptor receptor = new CompaniaEmisorReceptor(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                                        {
                                            //Validando que coincida el RFC del Receptor
                                            if (receptor.rfc.Equals(doc["cfdi:Comprobante"]["cfdi:Receptor"].Attributes["rfc"].Value))
                                            {
                                                //Declarando Variables Auxiliares
                                                decimal totalImpTrasladados = 0.00M, totalImpRetenidos = 0.00M;

                                                /** Retenciones **/
                                                //Validando que no exista el Nodo
                                                if (doc.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosRetenidos"] == null)
                                                {
                                                    //Validando que existan Retenciones
                                                    if (doc.DocumentElement["cfdi:Impuestos"]["cfdi:Retenciones"] != null)
                                                    {
                                                        //Validando que existan Nodos
                                                        if (doc.DocumentElement["cfdi:Impuestos"]["cfdi:Retenciones"].ChildNodes.Count > 0)
                                                        {
                                                            //Recorriendo Retenciones
                                                            foreach (XmlNode node in doc.DocumentElement["cfdi:Impuestos"]["cfdi:Retenciones"])
                                                            {
                                                                //Sumando Impuestos Retenidos
                                                                totalImpRetenidos += Convert.ToDecimal(node.Attributes["importe"].Value);
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                    //Asignando Total de Impuestos
                                                    totalImpRetenidos = Convert.ToDecimal(doc.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosRetenidos"].Value);


                                                /** Traslados **/
                                                //Validando que no exista el Nodo
                                                if (doc.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosTrasladados"] == null)
                                                {
                                                    //Validando que existan Traslados
                                                    if (doc.DocumentElement["cfdi:Impuestos"]["cfdi:Traslados"] != null)
                                                    {
                                                        //Validando que existan Nodos
                                                        if (doc.DocumentElement["cfdi:Impuestos"]["cfdi:Traslados"].ChildNodes.Count > 0)
                                                        {
                                                            //Recorriendo Traslados
                                                            foreach (XmlNode node in doc.DocumentElement["cfdi:Impuestos"]["cfdi:Traslados"])
                                                            {
                                                                //Sumando Impuestos Trasladados
                                                                totalImpRetenidos += Convert.ToDecimal(node.Attributes["importe"].Value);
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                    //Asignando Total de Impuestos
                                                    totalImpTrasladados = Convert.ToDecimal(doc.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosTrasladados"].Value);

                                                //Validando que los Montos Totales Coincidan
                                                if (oc.total == Convert.ToDecimal(doc["cfdi:Comprobante"].Attributes["total"].Value) &&
                                                    oc.subtotal == Convert.ToDecimal(doc["cfdi:Comprobante"].Attributes["subTotal"].Value) &&
                                                    oc.imp_trasladado == totalImpTrasladados && oc.imp_retenido == totalImpRetenidos)
                                                {
                                                    //Insertando factura
                                                    result = InsertaFacturaXML(doc, emisor.id_compania_emisor_receptor, emisor.dias_credito, receptor.id_compania_emisor_receptor,
                                                                total_p, subtotal_p, descuento_p, traslado_p, retenido_p, monto_tc, totalImpTrasladados, totalImpRetenidos);

                                                    //Operación Exitosa?
                                                    if (result.OperacionExitosa)

                                                        //Cargando Facturas Ligadas
                                                        cargaFacturaLigada();

                                                    //Mostrando Mensaje de Operación
                                                    ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
                                                }
                                                else
                                                {
                                                    //Declarando variables
                                                    decimal total = 0.00M, subtotal = 0.00M, trasladado = 0.00M, retenido = 0.00M;

                                                    //Obteniendo Diferencia
                                                    total = Math.Abs(oc.total - Convert.ToDecimal(doc["cfdi:Comprobante"].Attributes["total"].Value));
                                                    subtotal = Math.Abs(oc.subtotal - Convert.ToDecimal(doc["cfdi:Comprobante"].Attributes["subTotal"].Value));
                                                    trasladado = Math.Abs(oc.imp_trasladado - totalImpTrasladados);
                                                    retenido = Math.Abs(oc.imp_retenido - totalImpRetenidos);

                                                    //Validando que exista una Diferencia mas Grande
                                                    if (total < 1 && subtotal < 1 && trasladado < 1 && retenido < 1)
                                                    {
                                                        //Mostrando Ventana Modal de Diferencia
                                                        gestionaVentanaModal(this, "FacturaDiferencia");
                                                        //Ocultando Ventana Modal de Facturas Ligadas
                                                        gestionaVentanaModal(this, "FacturasLigadas");

                                                        //Asignando Totales de la Orden
                                                        lblSubTotalFac.Text = string.Format("{0:0.000000}", oc.subtotal);
                                                        lblTrasladadoFac.Text = string.Format("{0:0.000000}", oc.imp_trasladado);
                                                        lblRetenidoFac.Text = string.Format("{0:0.000000}", oc.imp_retenido);
                                                        lblTotalFac.Text = string.Format("{0:0.000000}", oc.total);

                                                        //Asignando Totales del XML
                                                        lblSubTotalXML.Text = string.Format("{0:0.000000}", Convert.ToDecimal(doc["cfdi:Comprobante"].Attributes["subTotal"].Value));
                                                        lblTrasladadoXML.Text = string.Format("{0:0.000000}", totalImpTrasladados);
                                                        lblRetenidoXML.Text = string.Format("{0:0.000000}", totalImpRetenidos);
                                                        lblTotalXML.Text = string.Format("{0:0.000000}", Convert.ToDecimal(doc["cfdi:Comprobante"].Attributes["total"].Value));
                                                    }
                                                    else
                                                        //Instanciando Excepción
                                                        ScriptServer.MuestraNotificacion(this, "Los Totales de la Factura, no coinciden con los de la Orden de Compra", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                                                }
                                            }
                                            else
                                                //Instanciando Excepcion
                                                ScriptServer.MuestraNotificacion(this, "La Compania Receptora no esta registrada", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                                        }
                                    }
                                    else
                                        //Instanciando Excepcion
                                        ScriptServer.MuestraNotificacion(this, "El Proveedor no coincide con la Factura", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                                }
                                else
                                    //Instanciando Excepcion
                                    ScriptServer.MuestraNotificacion(this, "El Compania Proveedora no esta registrado", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                            }
                        }
                        catch (Exception e)
                        {
                            //Mostrando Excepción
                            ScriptServer.MuestraNotificacion(this, e.Message, ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                        }
                    }
                }
                else
                    //Instanciando Excepcion
                    ScriptServer.MuestraNotificacion(this, "No Existe la Orden de Compra", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
        }
        /// <summary>
        /// Método encargado de Insertar la Factura del XML
        /// </summary>
        /// <param name="doc">Documento XML</param>
        /// <param name="total_p"></param>
        /// <param name="subtotal_p"></param>
        /// <param name="descuento_p"></param>
        /// <param name="traslado_p"></param>
        /// <param name="retenido_p"></param>
        /// <param name="monto_tc"></param>
        /// <param name="totalImpTrasladados"></param>
        /// <param name="totalImpRetenidos"></param>
        private RetornoOperacion InsertaFacturaXML(XmlDocument doc, int id_compania_emisora, int dias_credito, int id_compania_receptora, decimal total_p, 
                                        decimal subtotal_p, decimal descuento_p, decimal traslado_p, decimal retenido_p, decimal monto_tc, 
                                        decimal totalImpTrasladados, decimal totalImpRetenidos)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            
            //Instanciando Orden de Compra
            using (SAT_CL.Almacen.OrdenCompra oc = new SAT_CL.Almacen.OrdenCompra(Convert.ToInt32(Session["id_registro"])))
            {
                //Inicializando Bloque Transaccional
                using (TransactionScope trans = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    //Declarando Variable Auxiliar
                    int idFactura = 0;

                    //Insertando factura
                    result = FacturadoProveedor.InsertaFacturadoProveedor(id_compania_emisora, id_compania_receptora,
                                                        0, doc["cfdi:Comprobante"].Attributes["serie"] == null ? "" : doc["cfdi:Comprobante"].Attributes["serie"].Value,
                                                        doc["cfdi:Comprobante"].Attributes["folio"] == null ? "" : doc["cfdi:Comprobante"].Attributes["folio"].Value,
                                                        doc.DocumentElement["cfdi:Complemento"]["tfd:TimbreFiscalDigital"].Attributes["UUID"].Value,
                                                        Convert.ToDateTime(doc["cfdi:Comprobante"].Attributes["fecha"].Value), (byte)FacturadoProveedor.TipoComprobante.CFDI,
                                                        "I", 1, (byte)FacturadoProveedor.EstatusFactura.EnRevision, (byte)FacturadoProveedor.EstatusRecepion.Recibida,
                                                        0, 0, 0, Convert.ToDecimal(doc["cfdi:Comprobante"].Attributes["total"].Value), Convert.ToDecimal(doc["cfdi:Comprobante"].Attributes["subTotal"].Value),
                                                        doc["cfdi:Comprobante"].Attributes["descuentos"] == null ? 0 : Convert.ToDecimal(doc["cfdi:Comprobante"].Attributes["descuentos"].Value),
                                                        totalImpTrasladados, totalImpRetenidos,
                                                        doc["cfdi:Comprobante"].Attributes["Moneda"] == null ? "" : doc["cfdi:Comprobante"].Attributes["Moneda"].Value,
                                                        monto_tc, Convert.ToDateTime(doc["cfdi:Comprobante"].Attributes["fecha"].Value), total_p, subtotal_p, descuento_p, traslado_p,
                                                        retenido_p, false, DateTime.MinValue, Convert.ToDecimal(doc["cfdi:Comprobante"].Attributes["total"].Value),
                                                        doc["cfdi:Comprobante"].Attributes["condicionesDePago"] == null ? "" : doc["cfdi:Comprobante"].Attributes["condicionesDePago"].Value,
                                                        dias_credito, 1, (byte)FacturadoProveedor.EstatusValidacion.ValidacionSintactica, "",
                                                        ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                    //Validando Operación
                    if (result.OperacionExitosa)
                    {
                        //Obteniendo Factura
                        idFactura = result.IdRegistro;

                        //Obteniendo Nodos de Concepto
                        XmlNodeList xmlNL = doc.GetElementsByTagName("cfdi:Concepto");

                        //Declarando Variables Auxiliares
                        decimal tasa_imp_ret, tasa_imp_tras;
                        bool res = true;
                        int contador = 0;

                        //Recorriendo cada 
                        while (res)
                        {
                            //Obteniendo Concepto
                            XmlNode node = xmlNL[contador];
                            //Obteniendo Cantidades del Concepto
                            obtieneCantidadesConcepto(doc, node, out tasa_imp_tras, out tasa_imp_ret);
                            //Insertando Cocepto de Factura
                            result = FacturadoProveedorConcepto.InsertaFacturaProveedorConcepto(idFactura, Convert.ToDecimal(node.Attributes["cantidad"].Value),
                                                    node.Attributes["unidad"] == null ? "" : node.Attributes["unidad"].Value, node.Attributes["noIdentificacion"] == null ? "" : node.Attributes["noIdentificacion"].Value,
                                                    node.Attributes["descripcion"].Value, 0, Convert.ToDecimal(node.Attributes["valorUnitario"] == null ? "1" : node.Attributes["valorUnitario"].Value),
                                                    Convert.ToDecimal(node.Attributes["importe"] == null ? "1" : node.Attributes["importe"].Value),
                                                    Convert.ToDecimal(node.Attributes["importe"] == null ? "1" : node.Attributes["importe"].Value) * monto_tc,
                                                    tasa_imp_ret, tasa_imp_tras, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                            //Incrementando Contador
                            contador++;
                            //Obteniendo resultado del Ciclo
                            res = contador >= xmlNL.Count ? false : result.OperacionExitosa;
                        }

                        //Validando resultado
                        if (result.OperacionExitosa)
                        {
                            //Declarando Variables Auxiliares
                            string ruta;
                            //Compuesta de un path genérico para esta ventana, el Id de Tabla e Id de Registro, más el nombre asignado al archivo (incluyendo extensión)
                            ruta = string.Format(@"{0}{1}\{2}", SAT_CL.CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Directorio Raíz Gestor Archivos SAT"), 72.ToString("0000"), idFactura.ToString("0000000") + "-" + Session["XMLFileName"].ToString());
                            //Insertamos Registro
                            result = SAT_CL.Global.ArchivoRegistro.InsertaArchivoRegistro(72, idFactura, 8, "FACTURA: " + Session["XMLFileName"].ToString(), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario,
                                Encoding.UTF8.GetBytes(doc.OuterXml), ruta);

                            //Validando Operación Exitosa
                            if (result.OperacionExitosa)
                            {
                                //Validando que exista la Factura
                                if (oc.id_factura_proveedor > 0)
                                {
                                    //Instanciando Factura de Proveedor
                                    using (FacturadoProveedor fp = new FacturadoProveedor(oc.id_factura_proveedor))
                                    {
                                        //Validando que exista la Factura
                                        if (fp.habilitar)

                                            //Deshabilitar Factura de Proveedor
                                            result = fp.DeshabilitaFacturadoProveedor(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                        else
                                            //Instanciando Excepción
                                            result = new RetornoOperacion("No se puede acceder a la factura anterior");
                                    }
                                }
                                else
                                    //Instanciando 
                                    result = new RetornoOperacion(oc.id_orden_compra);

                                //Validando Operaciones
                                if (result.OperacionExitosa)

                                    //Actualiza la Factura de la Orden de Compra
                                    result = oc.ActualizaFacturaOrdenCompra(idFactura, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                //Validando Operación Exitosa
                                if (result.OperacionExitosa)
                                {
                                    //Instanciando Factura
                                    result = new RetornoOperacion(idFactura);

                                    //Completando Transacción
                                    trans.Complete();
                                }
                            }
                        }
                    }
                }
            }

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Privado encargado de Obtener las Cantidades de la Factura
        /// </summary>
        /// <param name="document">Factura XML</param>
        /// <param name="total_p">Total en Pesos</param>
        /// <param name="subtotal_p">Subtotal en Pesos</param>
        /// <param name="descuento_p">Descuento en Pesos</param>
        /// <param name="traslado_p">Importe Trasladado en Pesos</param>
        /// <param name="retenido_p">Importe Retenido en Pesos</param>
        /// <param name="monto_tc">Monto del Tipo de Cambio</param>
        private void obtieneCantidades(XmlDocument document, out decimal total_p, out decimal subtotal_p, out decimal descuento_p, out decimal traslado_p, out decimal retenido_p, out decimal monto_tc)
        {
            //Validando si existe el Tipo de Cambio
            if (document.DocumentElement.Attributes["TipoCambio"] == null)
            {
                //Asignando Atributo Obligatorios
                monto_tc = 1;
                total_p = Convert.ToDecimal(document.DocumentElement.Attributes["total"].Value);
                subtotal_p = Convert.ToDecimal(document.DocumentElement.Attributes["subTotal"].Value);
                traslado_p = document.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosTrasladados"] != null ?
                    Convert.ToDecimal(document.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosTrasladados"].Value) : 0;
                retenido_p = document.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosRetenidos"] != null ?
                    Convert.ToDecimal(document.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosRetenidos"].Value) : 0;

                //Asignando Atributos Opcionales
                descuento_p = document.DocumentElement.Attributes["descuento"] == null ? 0 :
                    Convert.ToDecimal(document.DocumentElement.Attributes["descuento"].Value);
            }
            else
            {
                //Asignando Atributo Obligatorios
                monto_tc = Convert.ToDecimal(document.DocumentElement.Attributes["TipoCambio"].Value);
                total_p = Convert.ToDecimal(document.DocumentElement.Attributes["total"].Value) * monto_tc;
                subtotal_p = Convert.ToDecimal(document.DocumentElement.Attributes["subTotal"].Value) * monto_tc;
                traslado_p = (document.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosTrasladados"] != null ?
                    Convert.ToDecimal(document.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosTrasladados"].Value) : 0) * monto_tc;
                retenido_p = (document.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosRetenidos"] != null ?
                    Convert.ToDecimal(document.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosRetenidos"].Value) : 0) * monto_tc;

                //Asignando Atributos Opcionales
                descuento_p = (document.DocumentElement.Attributes["descuento"] == null ? 0 : Convert.ToDecimal(document.DocumentElement.Attributes["descuento"].Value)) * monto_tc;
            }
        }
        /// <summary>
        /// Método Privado encargado de Obtener las Cantidades de los Conceptos de la Factura
        /// </summary>
        /// <param name="cfdi"></param>
        /// <param name="concepto"></param>
        /// <param name="tasa_imp_tras"></param>
        /// <param name="tasa_imp_ret"></param>
        /// <param name="imp_ret"></param>
        /// <param name="imp_tras"></param>
        private void obtieneCantidadesConcepto(XmlDocument cfdi, XmlNode concepto, out decimal tasa_imp_tras, out decimal tasa_imp_ret)
        {
            //Validación de Retenciones
            if (cfdi.DocumentElement["cfdi:Impuestos"]["cfdi:Retenciones"] != null)
            {
                //Validando que el Importe no sea "0"
                if (Convert.ToDecimal(cfdi.DocumentElement["cfdi:Impuestos"]["cfdi:Retenciones"]["cfdi:Retencion"].Attributes["importe"].Value) > 0 && Convert.ToDecimal(concepto.Attributes["importe"].Value) > 0)

                    //Asignando Valores
                    tasa_imp_ret = (Convert.ToDecimal(cfdi.DocumentElement["cfdi:Impuestos"]["cfdi:Retenciones"]["cfdi:Retencion"].Attributes["importe"].Value) / Convert.ToDecimal(concepto.Attributes["importe"].Value)) * 100;
                else
                    //Asignando Valores
                    tasa_imp_ret = 0;
            }
            else
                //Asignando Valores
                tasa_imp_ret = 0;

            //Validación de Traslados
            if (cfdi.DocumentElement["cfdi:Impuestos"]["cfdi:Traslados"] != null)
            {
                //Asignando Valores
                tasa_imp_tras = Convert.ToDecimal(cfdi.DocumentElement["cfdi:Impuestos"]["cfdi:Traslados"]["cfdi:Traslado"].Attributes["tasa"].Value);
            }
            else
                //Asignando Valores
                tasa_imp_tras = 0;
        }

        #endregion

        #endregion
    }
}