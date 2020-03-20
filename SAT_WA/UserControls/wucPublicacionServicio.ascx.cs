using SAT_CL;
using SAT_CL.Despacho;
using SAT_CL.Global;
using SAT_CL.Seguridad;
using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;
namespace SAT.UserControls
{
    public partial class wucPublicacionServicio : System.Web.UI.UserControl
    {
        #region Atributos

        /// <summary>
        /// Id Servicio
        /// </summary>
        private int _id_servicio;
        /// <summary>
        /// Id Publicación
        /// </summary>
        private int _id_publicacion;
        public event EventHandler ClickPublicar;
        /// <summary>
        /// Declaración de Evento ClickCancelar
        /// </summary>
        public event EventHandler ClickCancelar;
        /// <summary>
        /// Almacena el Nombre del Contenedor
        /// </summary>
        public string Contenedor;
        /// <summary>
        /// Tabla con las Paradas
        /// </summary>
        private DataTable _mitParadas;
        #endregion

        #region Eventos

        /// <summary>
        /// Evento generado al cargar la pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Validando  PostBack
            if (!this.IsPostBack)
            {
                //Carga catalogos
                cargaCatalogos();
            }
            else
            {
                //Recupera Atributos
                recuperaAtributos();


            }
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
        /// Manipula Evento ClickPublicar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void OnClickPublicar(EventArgs e)
        {
            if (ClickPublicar != null)
                ClickPublicar(this, e);

        }

        /// Evento disparado al presionar el boton Publicar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnClick_btnPublicar(object sender, EventArgs e)
        {
            //Validando que exista un Evento
            if (ClickPublicar != null)
                OnClickPublicar(e);
        }
        /// <summary>
        /// Manipula Evento ClickCancelar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void OnClickCancelar(EventArgs e)
        {
            if (ClickCancelar != null)
                ClickCancelar(this, e);

        }
        /// <summary>
        /// Evento disparado al presionar el boton cancelar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnClick_btnCancelar(object sender, EventArgs e)
        {
            //Validando que exista un Evento
            if (ClickCancelar != null)
                OnClickCancelar(new EventArgs());
        }


        /// Evento corting de gridview 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvParadas_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Validando que existan Llaves
            if (gvParadas.DataKeys.Count > 0)
            {   //Asignando Criterio de Ordenacion
                this._mitParadas.DefaultView.Sort = lblOrdenadoParadas.Text;
                //Cambiando Ordenamiento
                lblOrdenadoParadas.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvParadas, this._mitParadas, e.SortExpression, false, 1);
            }
        }
        /// <summary>
        /// Evento cambio de página en gridview 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvParadas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvParadas, this._mitParadas, e.NewPageIndex, false, 1);
        }
        /// <summary>
        /// Evento cambio de tamaño de página de gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoParadas_SelectedIndexChanged(object sender, EventArgs e)
        {
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvParadas, this._mitParadas, Convert.ToInt32(ddlTamanoParadas.SelectedValue), false, 1);
        }
        /// <summary>
        /// Evento click en botón de exportación 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarParadas_Click(object sender, EventArgs e)
        {
            TSDK.ASP.Controles.ExportaContenidoGridView(this._mitParadas, "");
        }
        #endregion

        #region Métodos


        /// <summary>
        /// Recupera Atributos
        /// </summary>
        private void recuperaAtributos()
        {
            //Recuperando Atributos
            if (Convert.ToInt32(ViewState["IdServicio"]) != 0)
                this._id_servicio = Convert.ToInt32(ViewState["IdServicio"]);
            if (ViewState["mitParadas"] != null)
                this._mitParadas = (DataTable)ViewState["mitParadas"];
            if (Convert.ToInt32(ViewState["IdPublicacion"]) != 0)
                this._id_publicacion = Convert.ToInt32(ViewState["IdPublicacion"]);


        }

        /// <summary>
        /// Asigna Atributos
        /// </summary>
        private void asignaAtributos()
        {
            //Asignando Atributos
            ViewState["IdServicio"] = this._id_servicio;
            ViewState["mitParadas"] = this._mitParadas;
            ViewState["IdPublicacion"] = this._id_publicacion;

        }

        /// <summary>
        /// Inicializa Valores
        /// </summary>
        private void inicializaValores()
        {
            //Validando que Exista un Servicio
            if (this._id_servicio != 0)
            {
                //Intanciamos Servicio
                using (SAT_CL.Documentacion.Servicio objServicio = new SAT_CL.Documentacion.Servicio(this._id_servicio))
                {
                    //Instanciamos Servicio Producto
                    using (SAT_CL.Documentacion.ServicioProducto objServicioProducto = new SAT_CL.Documentacion.ServicioProducto(SAT_CL.Documentacion.ServicioProducto.ObtieneProductoParadaInicial(objServicio.id_servicio)))
                    {
                        //Instanciamos Producto
                        using (SAT_CL.Global.Producto objProducto = new SAT_CL.Global.Producto(objServicioProducto.id_producto))
                        {
                            //Inicializamos Controles
                            txtConfirmacion.Text = SAT_CL.Global.Referencia.CargaReferencia(objServicio.id_compania_emisor.ToString(), 1, objServicio.id_servicio, "Referencia de Viaje", "Confirmación");
                            txtNoServicio.Text = objServicio.no_servicio;
                            txtProducto.Text = objProducto.descripcion + " ID:" + objProducto.id_producto.ToString();
                            txtPeso.Text = objServicioProducto.peso.ToString();
                            txtContacto.Text = "";
                            txtObservacion.Text = "";
                            txtTarifa.Text = "";
                            txtTelefono.Text = "";
                        }
                    }
                }
            }
            else
            {
                //Obtenemos Datos de la Publicación Activa del Servicio
                DataTable mitPublicacion = consumoInicializaPublicacion();

                //Validando que eregistros
                if (Validacion.ValidaOrigenDatos(mitPublicacion))
                {
                    //Obtenemos La Referencias del Concepto Origen
                    DataRow re = (from DataRow r in mitPublicacion.Rows
                                  select r).FirstOrDefault();
                    //Validamos que exista elementos
                    if (re != null)
                    {
                        //Inicializamos Controles
                        txtConfirmacion.Text = re.Field<string>("Confirmacion");
                        txtNoServicio.Text = re.Field<string>("NoServicio");
                        txtProducto.Text = re.Field<string>("Producto");
                        txtPeso.Text = Convert.ToInt32(re["Peso"]).ToString();
                        txtContacto.Text = re.Field<string>("Contacto");
                        txtObservacion.Text = re.Field<string>("Observacion");
                        txtTarifa.Text = string.Format("{0:C2}", Convert.ToInt32(re["TarifaPublicacion"]));
                        lblTarifaAceptada.Text = string.Format("{0:C2}", Convert.ToInt32(re["TarifaAceptada"]));
                        txtTelefono.Text = re.Field<string>("TelefonoContacto");
                        txtContacto.Text = re.Field<string>("Contacto");
                        chkFull.Checked = Convert.ToBoolean(re["Full"]);
                        chkManiobras.Checked = Convert.ToBoolean(re["Maniobras"]);
                        chkManiobras.Checked = Convert.ToBoolean(re["RC"]);
                        //Dimensiones tipo unidad
                        CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlDimensiones, "", 1108, 2);
                        ddlDimensiones.SelectedValue = Catalogo.RegresaDescripcionValor(1108,2, re.Field<string>("Dimensiones"));
                    }
                }
            }
        }

        /// <summary>
        /// Método encargado de Cargar los Catalogos del Control de Usuario
        /// </summary>
        private void cargaCatalogos()
        {
            //Dimensiones tipo unidad
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlDimensiones, "", 1108,2);
            //Cargando catalogo de tamaño  
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoParadas, "", 26);
       
        }




        /// <summary>
        /// Inicializa Control de Servicio
        /// </summary>
        /// <param name="id_servicio"></param>
        /// <param name="id_publicacion"></param>
        public void InicializaControl(int id_servicio, int id_publicacion)
        {
            //Asignamos Valor de Id Servicio
            this._id_servicio = id_servicio;
            this._id_publicacion = id_publicacion;
            //Inicializamos Valores
            inicializaValores();
            //Habilitamos Controles
            habilitaControles();
            //Cargamos Paradas
            cargaParadas();
        }

        /// <summary>
        /// Habilita Controles
        /// </summary>
        private void habilitaControles()
        {
            //Validamos Si existe el Id de Unidad
            if (this._id_servicio > 0)
            {
                //Habilitamos Controles
                txtConfirmacion.Enabled =
                txtNoServicio.Enabled =
                txtProducto.Enabled =
                txtPeso.Enabled =
                ddlDimensiones.Enabled =
                txtContacto.Enabled =
                txtObservacion.Enabled =
                txtTarifa.Enabled =
                txtTelefono.Enabled =
                chkFull.Enabled=
                chkManiobras.Enabled=
                chkRC.Enabled=
                gvParadas.Enabled =
                btnPublicar.Enabled = true;
            }
            else
            {
                //Deshabilitamos Controles
                txtConfirmacion.Enabled =
                txtNoServicio.Enabled =
                txtProducto.Enabled =
                txtPeso.Enabled =
                ddlDimensiones.Enabled =
                txtContacto.Enabled =
                txtObservacion.Enabled =
                txtTarifa.Enabled =
                txtTelefono.Enabled =
                chkFull.Enabled =
                chkManiobras.Enabled =
                chkRC.Enabled =
                gvParadas.Enabled =
                btnPublicar.Enabled = false;
            }
        }

        /// <summary>
        /// Método encargado de cargar la Paradas
        /// </summary>
        private void cargaParadas()
        {

            //Validando que Exista un Servicio
            if (this._id_publicacion == 0)
            {
                //Inicializamos Indices del grid View Paradas
                TSDK.ASP.Controles.InicializaIndices(gvParadas);

                //Obtenemos Paradas
                using (DataTable mit = SAT_CL.Despacho.Parada.CargaParadasParaVisualizacion(this._id_servicio))
                {
                    //Valida Origen de Datos
                    if (TSDK.Datos.Validacion.ValidaOrigenDatos(mit))
                    {
                        //Cargamos Grid View
                        TSDK.ASP.Controles.CargaGridView(gvParadas, mit, "Id",lblOrdenadoParadas.Text, false, 0);

                        //Añadiendo Tabla 
                        this._mitParadas = mit;
                    }
                    else
                    {
                        //Inicializando GridView
                        TSDK.ASP.Controles.InicializaGridview(gvParadas);

                        //Eliminando Tabla 
                        this._mitParadas = null;
                    }
                }
            }
            else
            {
                //Cargamos Paradas 
                this._mitParadas = consumoInicializaPublicacionParadas();
                //Mostrandolo en el Grid View
                TSDK.ASP.Controles.CargaGridView(gvParadas, this._mitParadas, "Id", lblOrdenadoParadas.Text, false, 1);

            }
        }



        /// <summary>
        /// Obtenemos Información de las Paradas
        /// </summary>
        /// <returns></returns>
        private DataTable consumoInicializaPublicacionParadas()
        {
            //Establecemos variable resultado
            DataSet ds = new DataSet();
            DataTable mit = null;
            RetornoOperacion resultado = new RetornoOperacion();

            //Consumimos Web Service
            using (DespachoCentral.DespachoClient despacho = new DespachoCentral.DespachoClient())
            {

                string resultado_web_service = despacho.ObtieneParadasServicio(this._id_publicacion, CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Direccion del Servidor", 0), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Nombre de BD", 0),
                                   TSDK.Base.Cadena.VerificaCadenaVacia(SAT_CL.Global.Referencia.CargaReferencia("0", 25, ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "Consumo Web Service", "Contraseña"), ""));

                //Obtenemos Documento generado
                XDocument xDoc = XDocument.Parse(resultado_web_service);

                //Validamos que exista Respuesta
                if (xDoc.Descendants("idRegistro").FirstOrDefault().Value != null)
                {
                    //Traduciendo resultado
                    resultado = new RetornoOperacion(Convert.ToInt32(xDoc.Descendants("idRegistro").FirstOrDefault().Value), xDoc.Descendants("mensaje").FirstOrDefault().Value.ToString(), Convert.ToBoolean(xDoc.Descendants("operacionExitosa").FirstOrDefault().Value));

                }
                //Validamos Resultado
                if (resultado.OperacionExitosa)
                {
                    //Obtenemos DataSet
                    ds.ReadXml(xDoc.Document.Element("ParadasServicio").Element("NewDataSet").CreateReader());
                    //Asignamos tabla
                    mit = ds.Tables["Table"];
                }
                else
                {
                    //Establecmos Mensaje Resultado
                    resultado = new RetornoOperacion("No es posible obtener la respuesta de WS");
                }
                //Si Existe Error
                if (!resultado.OperacionExitosa)
                {
                    //Mostrando Mensaje de Operación
                    TSDK.ASP.ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
                }

                despacho.Close();
            }
            return mit;
        }

        /// <summary>
        /// Obtenemos Información de la Publicación
        /// </summary>
        /// <returns></returns>
        private DataTable consumoInicializaPublicacion()
        {
            //Establecemos variable resultado
            DataSet ds = new DataSet();
            DataTable mit = null;
            RetornoOperacion resultado = new RetornoOperacion();

            //Consumimos Web Service
            using (DespachoCentral.DespachoClient despacho = new DespachoCentral.DespachoClient())
            {

                string resultado_web_service = despacho.ObtieneDatosPublicacionServicio(this._id_publicacion, CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Direccion del Servidor", 0), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Nombre de BD", 0),
                                   TSDK.Base.Cadena.VerificaCadenaVacia(SAT_CL.Global.Referencia.CargaReferencia("0", 25, ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "Consumo Web Service", "Contraseña"), ""));

                //Obtenemos Documento generado
                XDocument xDoc = XDocument.Parse(resultado_web_service);

                //Validamos que exista Respuesta
                if (xDoc.Descendants("idRegistro").FirstOrDefault().Value != null)
                {
                    //Traduciendo resultado
                    resultado = new RetornoOperacion(Convert.ToInt32(xDoc.Descendants("idRegistro").FirstOrDefault().Value), xDoc.Descendants("mensaje").FirstOrDefault().Value.ToString(), Convert.ToBoolean(xDoc.Descendants("operacionExitosa").FirstOrDefault().Value));

                }
                //Validamos Resultado
                if (resultado.OperacionExitosa)
                {
                    //Obtenemos DataSet
                    ds.ReadXml(xDoc.Document.Element("Servicio").Element("NewDataSet").CreateReader());
                    //Asignamos tabla
                    mit = ds.Tables["Table"];
                }
                else
                {
                    //Establecmos Mensaje Resultado
                    resultado = new RetornoOperacion("No es posible obtener la respuesta de WS");
                }
                //Si Existe Error
                if (!resultado.OperacionExitosa)
                {
                    //Mostrando Mensaje de Operación
                    TSDK.ASP.ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
                }

                despacho.Close();
            }
            return mit;
        }

        #endregion

    



        /// <summary>
        /// Publica Servicio
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion publicaServicio()
        {
            //Establecemos variable resultado
            RetornoOperacion resultado = null;

            //Consumimos Web Service
            using (DespachoCentral.DespachoClient despacho = new DespachoCentral.DespachoClient())
            {
                //Instanciamos Compañia
                using (SAT_CL.Global.CompaniaEmisorReceptor objCompania = new CompaniaEmisorReceptor(((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                {
                    //Instanciamos Servicio
                    using (SAT_CL.Documentacion.Servicio objServicio = new SAT_CL.Documentacion.Servicio(this._id_servicio))
                    {

                        string resultado_web_service = despacho.PublicaServicio(objServicio.id_servicio, objServicio.id_compania_emisor, objCompania.nombre,
                                                       objServicio.id_compania_emisor, objCompania.nombre, txtNoServicio.Text, txtConfirmacion.Text, txtObservacion.Text,
                                                       SAT_CL.Tarifas.TipoCargo.ObtieneTipoCargoDescripcion(objCompania.id_compania_emisor_receptor, "Flete"), Convert.ToDecimal(txtTarifa.Text),
                                                       Cadena.RegresaCadenaSeparada(txtProducto.Text, "ID:", 0), Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtProducto.Text, ":ID", 1)),
                                                       Convert.ToDecimal(txtPeso.Text), txtContacto.Text, txtTelefono.Text, chkFull.Checked, chkManiobras.Checked,
                                                       chkRC.Checked, ddlDimensiones.SelectedItem.Text, SAT_CL.Despacho.ConsumoSOAPCentral.ObtieneParadasPublicacionUnidad(SAT_CL.Despacho.Parada.CargaParadasParaPublicacionDeUnidad(objServicio.id_servicio)),
                                                         CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Direccion del Servidor", 0), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Nombre de BD", 0),
                                                    TSDK.Base.Cadena.VerificaCadenaVacia(SAT_CL.Global.Referencia.CargaReferencia("0", 25, objCompania.id_compania_emisor_receptor, "Consumo Web Service", "Contraseña"), ""),
                                                     ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).nombre);



                        //Obtenemos Documento generado
                        XDocument xDoc = XDocument.Parse(resultado_web_service);

                        //Validamos que exista Respuesta
                        if (xDoc != null)
                        {
                            //Traduciendo resultado
                            resultado = new RetornoOperacion(Convert.ToInt32(xDoc.Descendants("idRegistro").FirstOrDefault().Value), xDoc.Descendants("mensaje").FirstOrDefault().Value.ToString(), Convert.ToBoolean(xDoc.Descendants("operacionExitosa").FirstOrDefault().Value));


                            //Validamos Resultado
                            if (resultado.OperacionExitosa)
                            {
                                //Personalizamos Mensaje
                                resultado = new RetornoOperacion("El servicio ha sido Publicado", true);
                            }

                        }
                        else
                        {
                            //Establecmos Mensaje Resultado
                            resultado = new RetornoOperacion("No es posible obtener la respuesta de WS");
                        }
                        //Mostrando Mensaje de Operación
                        TSDK.ASP.ScriptServer.MuestraNotificacion(btnPublicar, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
                    }
                }

                //Cerramos Conexion
                despacho.Close();

            }

            return resultado;
        }


        /// <summary>
        /// Método encargado de Publicar el Servicio
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion PublicaServicio()
        {
            //Declaramos Objeto Retorno
            RetornoOperacion resultado = new RetornoOperacion();

            //Publicamos Servicio
            resultado = publicaServicio();
            //Devolvemos Valor
            return resultado;
        }


    }
}