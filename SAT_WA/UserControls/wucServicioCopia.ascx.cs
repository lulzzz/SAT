using SAT_CL.Seguridad;
using System;
using System.Data;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.UserControls
{
    public partial class wucServicioCopia : System.Web.UI.UserControl
    {
        #region Atributos

        private DataTable _dtServiciosMaestros;
        /// <summary>
        /// Obtiene el Id de Servicio Maestro por copiar
        /// </summary>
        public int id_servicio_maestro { get { return Convert.ToInt32(gvServiciosMaestros.SelectedDataKey.Value); } }
        /// <summary>
        /// Almacena el Nombre del Contenedor
        /// </summary>
        public string Contenedor;

        #endregion

        #region Eventos

        /// <summary>
        /// Evento Producido al Efectuarse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Validando que se produzca un PostBack
            if (!(Page.IsPostBack))
                //Asignando Atributos
                asignaAtributos();
            else
                //Recuperando Atributos
                recuperaAtributos();
        }
        /// <summary>
        /// Evento desencadenado antes de Producirse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {   //Invocando Método de Asignación
            asignaAtributos();
        }

        #region Eventos (Manejadores)

        /// <summary>
        /// Manejador de Evento "Guardar"
        /// </summary>
        public event EventHandler ClickGuardarServicioCopia;
        /// <summary>
        /// Manejador de Evento "Cancelar"
        /// </summary>
        public event EventHandler ClickCancelarServicioCopia;
        /// <summary>
        /// Evento que Manipula el Manejador "Guardar"
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnClickGuardarServicioCopia(EventArgs e)
        {   //Validando que exista el Evento
            if (ClickGuardarServicioCopia != null)
                //Iniciando Evento
                ClickGuardarServicioCopia(this, e);
        }
        /// <summary>
        /// Evento que Manipula el Manejador "Cancelar"
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnClickCancelarServicioCopia(EventArgs e)
        {   //Validando que exista el Evento
            if (ClickCancelarServicioCopia != null)
                //Iniciando Evento
                ClickCancelarServicioCopia(this, e);
        }

        #endregion

        /// <summary>
        /// Evento producido al pulsar él botón Cancelar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarBusquedaMaestro_Click(object sender, EventArgs e)
        {
            //Validando que exista un Evento
            if (ClickCancelarServicioCopia != null)
                //Iniciando Manejador
                OnClickCancelarServicioCopia(e);
        }
        /// <summary>
        /// Evento producido al pulsar él botón Buscar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscarMaestro_Click(object sender, EventArgs e)
        {
            //Realizando la búsqueda de servicios maestros coincidentes
            buscaServiciosMaestros();
        }

        #region Eventos GridView "Servicios Maestros"

        /// <summary>
        /// Evento producido al pulsar el botón copiar de algún servicio maestro
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCopiar_Click(object sender, EventArgs e)
        {
            //Validando existencia de registros
            if (gvServiciosMaestros.DataKeys.Count > 0)
            {
                //Declarando variable de retorno
                RetornoOperacion resultado = new RetornoOperacion();

                //Seleccionando la fila requerida
                Controles.SeleccionaFila(gvServiciosMaestros, sender, "lnk", false);

                //Validando que exista un Evento
                if (ClickGuardarServicioCopia != null)
                    //Iniciando Manejador
                    OnClickGuardarServicioCopia(e);
            }
        }
        /// <summary>
        /// Evento producido al cambiar el indice activo de página en el GV de servicios maestros
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvServiciosMaestros_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Si hay elementos mostrados
            if (this._dtServiciosMaestros != null)
            {
                //Aplicando criterio de orden de origen de datos
                this._dtServiciosMaestros.DefaultView.Sort = lblOrdenadoServiciosMaestros.Text;
                //Cambiando Indice de Página
                Controles.CambiaIndicePaginaGridView(gvServiciosMaestros, this._dtServiciosMaestros, e.NewPageIndex, true, 1);
            }
        }
        /// <summary>
        /// Cambio en el criterio de orden del gv de servicios
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvServiciosMaestros_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Si hay elementos mostrados
            if (this._dtServiciosMaestros != null)
            {
                //Aplicando criterio de orden de origen de datos
                this._dtServiciosMaestros.DefaultView.Sort = lblOrdenadoServiciosMaestros.Text;
                lblOrdenadoServiciosMaestros.Text = Controles.CambiaSortExpressionGridView(gvServiciosMaestros, this._dtServiciosMaestros, e.SortExpression, true, 1);
            }
        }
        /// <summary>
        /// Cambio de tamaño del GV de Servicios maestros
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoServiciosMaestros_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Si hay elementos mostrados
            if (this._dtServiciosMaestros != null)
            {
                //Aplicando criterio de orden de origen de datos
                this._dtServiciosMaestros.DefaultView.Sort = lblOrdenadoServiciosMaestros.Text;
                Controles.CambiaTamañoPaginaGridView(gvServiciosMaestros, this._dtServiciosMaestros, Convert.ToInt32(ddlTamanoServiciosMaestros.SelectedValue), true, 1);
            }
        }
        /// <summary>
        /// Clic en exportar listado de servicios
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarServiciosMaestros_Click(object sender, EventArgs e)
        {
            Controles.ExportaContenidoGridView(this._dtServiciosMaestros, "*".ToCharArray());
        }

        #endregion

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Privado encargado de Asignar los Atributos de la Forma
        /// </summary>
        private void asignaAtributos()
        {   
            //Asignando Atributos
            ViewState["DT"] = this._dtServiciosMaestros;
        }
        /// <summary>
        /// Método Privado encargado de Recuperar el Valor de los Atributos de la Forma
        /// </summary>
        private void recuperaAtributos()
        {   
            //Recuperando Atributos
            if (TSDK.Datos.Validacion.ValidaOrigenDatos((DataTable)ViewState["DT"]))
                
                //Asignando Servicios Maestros
                this._dtServiciosMaestros = (DataTable)ViewState["DT"];
        }
        /// <summary>
        /// Carga el contenido inicial de los controles de búsqueda de servicio maestro
        /// </summary>
        /// <param name="id_origen">Ubicación de Origen</param>
        private void inicializaVentanaServiciosMaestros(int id_origen)
        {
            //Instanciando Ubicaciones
            using (SAT_CL.Global.Ubicacion uo = new SAT_CL.Global.Ubicacion(id_origen))
            {
                //Existe el Origen
                if (uo.id_ubicacion > 0)
                {
                    //Asignando Valor
                    txtUbicacionOrigenMaestro.Text = uo.descripcion + " ID:" + uo.id_ubicacion.ToString();

                    //Buscando Servicios Maestros
                    buscaServiciosMaestros();
                }
                else
                    //Limpiando filtro
                    txtUbicacionOrigenMaestro.Text = "";

                //Limpiando filtros
                txtClienteMaestro.Text =
                txtUbicacionDestinoMaestro.Text = "";

                //Limpiando información de nueva copia
                txtCitaCargaCopia.Text =
                txtCitaDescargaCopia.Text =
                txtNoViajeCopia.Text =
                txtNoConfirmacionViajeCopia.Text = "";

                txtProductoCopia.Text =
                txtCantidadProductoCopia.Text =
                txtPesoProductoCopia.Text = "";
                ddlUnidadCantidadProductoCopia.SelectedValue = "23";
                ddlUnidadPesoProductoCopia.SelectedValue = "18";

                //Cargando catálogos de unidades de medida (cantidad y peso) del producto de copia
                SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlUnidadCantidadProductoCopia, 2, "Otro", 5, "", 0, "");
                SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlUnidadPesoProductoCopia, 2, "Otro", 2, "", 4, "");
                //Cargando Tamaño de GV en catálogo
                SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoServiciosMaestros, "", 26);

                //Limpiando resultados de búsqueda
                TSDK.ASP.Controles.InicializaGridview(gvServiciosMaestros);
                //Asignando foco
                txtClienteMaestro.Focus();
            }
        }
        /// <summary>
        /// Busca los servicios maestros solicitados
        /// </summary>
        private void buscaServiciosMaestros()
        {
            //Obteniendo registros de interés
            using (System.Data.DataTable mit = SAT_CL.Documentacion.Servicio.CargaServiciosMaestros(((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                                                                Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtClienteMaestro.Text, "ID:", 1)),
                                                                                Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtUbicacionOrigenMaestro.Text, "ID:", 1)),
                                                                                Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtUbicacionDestinoMaestro.Text, "ID:", 1))))
            {
                //Almacenando Origen de datos
                this._dtServiciosMaestros = mit;

                //Validando que Existan Servicios Maestros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(this._dtServiciosMaestros))
                {
                    if (this._dtServiciosMaestros.DefaultView != null)
                        this._dtServiciosMaestros.DefaultView.Sort = lblOrdenadoServiciosMaestros.Text;

                    //Llenando GridView
                    Controles.CargaGridView(gvServiciosMaestros, this._dtServiciosMaestros, "Id", lblOrdenadoServiciosMaestros.Text, true, 1);
                }
                else
                    //Llenando GridView
                    Controles.InicializaGridview(gvServiciosMaestros);                
            }
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método encargado de Inicializar el Control
        /// </summary>
        public void InicializaServicioCopia()
        {
            //Invocando Método de Inicialización
            inicializaVentanaServiciosMaestros(0);
        }
        /// <summary>
        /// Método encargado de Inicializar el Control
        /// </summary>
        /// <param name="id_cliente">Cliente del Servicio</param>
        /// <param name="id_origen">Ubicación de Origen</param>
        /// <param name="id_destino">Ubicación de Destino</param>
        public void InicializaServicioCopia(int id_origen)
        {
            //Invocando Método de Inicialización
            inicializaVentanaServiciosMaestros(id_origen);
        }
        /// <summary>
        /// Método encargado de Copiar el Servicio
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion CopiaServicio()
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            int id_servicio = 0;
            //Instanciando servicio maestro
            using (SAT_CL.Documentacion.Servicio sm = new SAT_CL.Documentacion.Servicio(Convert.ToInt32(gvServiciosMaestros.SelectedDataKey.Value)))
            {
                //Validando que Exista el Servicio
                if (sm.id_servicio > 0)
                {
                    //Definiendo datos auxiliares de producto
                    int id_producto_reemplazo = Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtProductoCopia.Text, "ID:", 1));
                    decimal cantidad_producto; Decimal.TryParse(txtCantidadProductoCopia.Text, out cantidad_producto);
                    decimal peso_producto; Decimal.TryParse(txtPesoProductoCopia.Text, out peso_producto);

                    //Si se ha solicitado reemplazo de producto en copia de servicio maestro
                    if (id_producto_reemplazo > 0)
                    {
                        //Creando copia con sobreescritura de producto
                        result = sm.CopiarServicio(Convert.ToDateTime(txtCitaCargaCopia.Text), Convert.ToDateTime(txtCitaDescargaCopia.Text), txtNoViajeCopia.Text.ToUpper(),
                                                    txtNoConfirmacionViajeCopia.Text.ToUpper(), id_producto_reemplazo, cantidad_producto, Convert.ToByte(ddlUnidadCantidadProductoCopia.SelectedValue),
                                                    peso_producto, Convert.ToByte(ddlUnidadPesoProductoCopia.SelectedValue), ((Usuario)Session["usuario"]).id_usuario);
                    }
                    //Si no se pide cambio de producto
                    else
                        //Creando copia de servicio, sólo especificando información de servicio y paradas
                        result = sm.CopiarServicio(Convert.ToDateTime(txtCitaCargaCopia.Text), Convert.ToDateTime(txtCitaDescargaCopia.Text), txtNoViajeCopia.Text.ToUpper(),
                                                    txtNoConfirmacionViajeCopia.Text.ToUpper(),((Usuario)Session["usuario"]).id_usuario);

                            //Establecemos Id de Servicio
                            id_servicio = result.IdRegistro;
                            //Si no hay dificultades con la inserción
                            if (result.OperacionExitosa)
                            {
                                //Instanciamos Servicio
                                using (SAT_CL.Documentacion.Servicio objServicio = new SAT_CL.Documentacion.Servicio(id_servicio))
                                {

                                    //Actualizamos Refrencias de Servicio}
                                    result = objServicio.ActualizacionReferenciaViaje(txtNoViajeCopia.Text.ToUpper(), ((Usuario)Session["usuario"]).id_usuario);
                                }
                            }
                      
                }
            }

            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion
                
    }
}