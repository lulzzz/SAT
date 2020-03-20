using SAT_CL;
using SAT_CL.Global;
using SAT_CL.Seguridad;
using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.Operacion
{
    public partial class Publicaciones : System.Web.UI.Page
    {
        /// <summary>
        /// Contenedor de la Forma
        /// </summary>
        private string Contenedor;
        /// <summary>
        /// Evento generado al Cargar la Pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Aplicando seguridad de la forma
            SAT_CL.Seguridad.Forma.AplicaSeguridadForma(this, "content1", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
            
            //Validando  PostBack
            if (!Page.IsPostBack)
            {
                //Inicializando  la forma
                inicializaForma();
            }
        }

        #region Eventos Publicación de Unidades
        /// <summary>
        /// Click en algún Link de GV de Unidad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbAccionUnidad_Click(object sender, EventArgs e)
        {
            //Si hay registros
            if (gvUnidades.DataKeys.Count > 0)
            {
                //Seleccionando la fila correspondiente
                Controles.SeleccionaFila(gvUnidades, sender, "lnk", false);
                //Convirtiendo objeto a linkbutton
                LinkButton lkb = (LinkButton)sender;

                //Determinando el comando por ejecutar
                switch (lkb.CommandName)
                {
                    case "Respuesta":
                        //Mostrando ventana modal correspondiente
                        alternaVentanaModal("resultadoRespuesta", lkb);

                        //De acuerdo al Inficador
                        if (Convert.ToInt32(gvUnidades.SelectedDataKey["Indicador"]) == 1)
                        {
                            //Ocultamos Boton
                            lkbCrearRespuesta.Visible = false;
                        }
                        else
                        {
                            //Mostramos Boton
                            lkbCrearRespuesta.Visible = true;
                        }
                        //cargamos respuesta
                        cargaResultadoRespuesta(gvUnidades);
                        break;
                    case "Ciudades":
                        //Mostrando ventana modal correspondiente
                        alternaVentanaModal("informacionCiudades", lkb);
                        //cargamos respuesta
                        cargaCiudadesPublicacion();
                        break;
                    case "Copiar":
                        //Asignamos Valor
                        lblValor.Text = "No";
                        //Copiamos Servicio;
                        copiarServicioPUFinal(lkb, false);
                        break;
                    case "Oferta Realizada":
                    case "Oferta Aceptada":
                        //Abriendo ventana
                        alternaVentanaModal("crearRespuesta", lkb);
                        //Inicializamos Controles
                        inicializaControlesCrearRespuesta(Convert.ToInt32(gvUnidades.SelectedDataKey["IdRespuesta"]));
                        btnCrearRespuesta.Visible =
                        btnConfirmar.Visible = false;
                        break;
                    case "Ofertar":
                        //Abriendo ventana
                        alternaVentanaModal("crearRespuesta", lkb);
                        //Inicializamos Controles
                        inicializaControlesCrearRespuesta(0);
                        btnCrearRespuesta.Visible = true;
                        btnConfirmar.Visible = false;
                        break;
                    case "Confirmar":
                        //Abriendo ventana
                        alternaVentanaModal("crearRespuesta", lkb);
                        //Inicializamos Controles
                        inicializaControlesCrearRespuesta(Convert.ToInt32(gvUnidades.SelectedDataKey["IdRespuesta"]));
                        btnCrearRespuesta.Visible = false;
                        btnConfirmar.Visible = true;
                        break;
                }
            }
        }

        /// <summary>
        /// Cambio de tamaño de página en GV de Unidades
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoUnidades_SelectedIndexChanged(object sender, EventArgs e)
        {
            Controles.CambiaTamañoPaginaGridView(gvUnidades, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamanoUnidades.SelectedValue), true, 3);
        }
        /// <summary>
        /// Cambio de página activa del GV de Unidades
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvUnidades_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Controles.CambiaIndicePaginaGridView(gvUnidades, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, true, 3);
        }

        /// <summary>
        /// Cambio de Criterio de orden de GV de Unidades
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvUnidades_Sorting(object sender, GridViewSortEventArgs e)
        {
            lblOrdenadoUnidades.Text = Controles.CambiaSortExpressionGridView(gvUnidades, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression, true, 3);
        }

        /// <summary>
        /// Evento generado al dar clic en Buscar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscarUnidades_Click(object sender, EventArgs e)
        {
            //Carga Publicaciones de Unidades
            cargaPublicaciones();
        }

        /// <summary>
        /// Evento generago al cambiar el Tamaño de la Pagina de Ciudades
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvCiudades_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Controles.CambiaIndicePaginaGridView(gvCiudades, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table8"), e.NewPageIndex, false, 3);

        }

        /// <summary>
        /// Evento generado al cambiar el Sort del gv de Ciudades
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvCiudades_Sorting(object sender, GridViewSortEventArgs e)
        {
            lblOrdenadoCiudades.Text = Controles.CambiaSortExpressionGridView(gvCiudades, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table8"), e.SortExpression, false, 3);

        }

        /// <summary>
        /// Evento generado al cambiar el tamaño de gv de Ciudades
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoCiudades_SelectedIndexChanged(object sender, EventArgs e)
        {
            Controles.CambiaTamañoPaginaGridView(gvCiudades, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table8"), Convert.ToInt32(ddlTamanoCiudades.SelectedValue), false, 3);

        }

        /// <summary>
        /// Evento generado al cambiar el tamalo de la Págin de Resultados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvResultadoRespuesta_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Controles.CambiaIndicePaginaGridView(gvResultadoRespuesta, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table6"), e.NewPageIndex, false, 3);

        }

        /// <summary>
        /// Evento generado al cambiar el Sort de Resultados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvResultadoRespuesta_Sorting(object sender, GridViewSortEventArgs e)
        {
            lblOrdenadoResultadoRespuesta.Text = Controles.CambiaSortExpressionGridView(gvResultadoRespuesta, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table6"), e.SortExpression, false, 3);

        }

        /// <summary>
        /// Evento generado al cambiar el Tamaño de sort de Resultados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlResultadoRespuesta_SelectedIndexChanged(object sender, EventArgs e)
        {
            Controles.CambiaTamañoPaginaGridView(gvResultadoRespuesta, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table6"), Convert.ToInt32(ddlTamanoResultadoRespuesta.SelectedValue), false, 3);

        }

        /// <summary>
        /// Evento generado al Ver los Detalles
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbVerDetalleResultado_Click(object sender, EventArgs e)
        {
            //Si hay registros
            if (gvResultadoRespuesta.DataKeys.Count > 0)
            {
                //Seleccionando la fila correspondiente
                Controles.SeleccionaFila(gvResultadoRespuesta, sender, "lnk", false);
                //Mostrando ventana modal btn
                alternaVentanaModal("opcionSeleccionRespuesta", gvResultadoRespuesta);
                //ocultamos ventana modal correspondiente
                alternaVentanaModal("resultadoRespuesta", gvResultadoRespuesta);
                //Cargamos Respuestas
                cargaRespuestas(gvResultadoRespuesta);
            }
        }

        /// <summary>
        /// Evento generado al cambiar el Tamaño de Eventos Deseados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvEventosDeseados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Controles.CambiaIndicePaginaGridView(gvEventosDeseados, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), e.NewPageIndex, false, 3);

        }

        /// <summary>
        /// Evento generado al cambiar el Sort de Eventos Generados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvEventosDeseados_Sorting(object sender, GridViewSortEventArgs e)
        {
            lblOrdenadoEventosDeseados.Text = Controles.CambiaSortExpressionGridView(gvEventosDeseados, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), e.SortExpression, false, 3);

        }



        /// <summary>
        /// Evento generado al enlazar los datos del Gv Eventos Deseados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvEventosDeseados_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Obtenemos la fila actual
            GridViewRow fila = e.Row;
            //Verificamos si la fila es para los elementos de datos o es la fila del footer
            switch (fila.RowType)
            {
                //fila de Footer
                case DataControlRowType.Footer:
                    //instanciamos DropDown de edicion de la fila
                    using (DropDownList ddlActividad = (DropDownList)fila.FindControl("ddlActividadDeseada"))
                    {
                        //Cargando Catalogos 
                        CapaNegocio.m_capaNegocio.CargaCatalogo(ddlActividad, 40, "", 1, "", 0, "");
                    }
                    break;
            }
        }

        /// <summary>
        /// Evento generado al cambiar el Tamaño de Eventos Deseados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoEventosDeseados_SelectedIndexChanged(object sender, EventArgs e)
        {
            Controles.CambiaTamañoPaginaGridView(gvEventosDeseados, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), Convert.ToInt32(ddlTamanoEventosDeseados.SelectedValue), false, 3);

        }

        /// <summary>
        /// Evento generado al responder publicacion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCrearRespuesta_Click(object sender, EventArgs e)
        {

            //Declaramos objeto retorno
            RetornoOperacion resultado = new RetornoOperacion();
            //Respondemos Publicación de Servicio
            resultado = respondePublicacion();
            //Validamos Resultado
            if (resultado.OperacionExitosa)
            {
                //Cerramos Ventana Modal
                alternaVentanaModal("crearRespuesta", btnCrearRespuesta);

                //Cargamos Publicaciónes de Unidad
                cargaPublicaciones();
            }
        }

        /// <summary>
        /// Evento generado al Cambiar el Tamaño del Drop Down List
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoRespuestas_SelectedIndexChanged(object sender, EventArgs e)
        {
            Controles.CambiaTamañoPaginaGridView(gvRespuestas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3"), Convert.ToInt32(ddlTamanoRespuestas.SelectedValue), false, 3);

        }
        /// <summary>
        /// Cambio de página activa del GV de Respuestas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvRespuestas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Controles.CambiaIndicePaginaGridView(gvRespuestas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3"), e.NewPageIndex, false, 4);
        }

        /// <summary>
        /// Cambio de criterio de ordenamiento en GV de Respuestas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvRespuestas_Sorting(object sender, GridViewSortEventArgs e)
        {
            lblOrdenadoRespuestas.Text = Controles.CambiaSortExpressionGridView(gvRespuestas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3"), e.SortExpression, false, 4);
        }
        /// <summary>
        /// Evento generado al cambiar de página de Info de Viajes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvInfoViaje_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Controles.CambiaIndicePaginaGridView(gvInfoViaje, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table4"), e.NewPageIndex, false, 4);

        }

        /// <summary>
        /// Evento generado al cambiar el Tamaño de Inf de Viajes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoInfoViaje_SelectedIndexChanged(object sender, EventArgs e)
        {
            Controles.CambiaTamañoPaginaGridView(gvInfoViaje, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table4"), Convert.ToInt32(ddlTamanoInfoViaje.SelectedValue), false, 4);

        }

        /// <summary>
        /// Evento Generado al enlazar el Grid View
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvRespuestas_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Por cada Fila de Tipo Datos
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Validamos existencia de Paradas
                if (gvRespuestas.DataKeys.Count > 0)
                {
                    //Buscamos Grid View de Eventos
                    using (GridView gvCiudadRespuesta = (GridView)e.Row.FindControl("gvCiudadRespuesta"))
                    {
                        //Carga Eventos para cada una de las Paradas
                        using (DataTable mit = cargaCiudades(Convert.ToInt32(gvRespuestas.DataKeys[e.Row.RowIndex].Value), gvRespuestas))
                        {
                            //Validamos Origen de Datos
                            if (TSDK.Datos.Validacion.ValidaOrigenDatos(mit))
                            {
                                //Cargamos Grid View Eventos
                                TSDK.ASP.Controles.CargaGridView(gvCiudadRespuesta, mit, "IdCiudad", "");

                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Evento Generado al enlazar el Grid View
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvInfoViaje_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Por cada Fila de Tipo Datos
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Validamos existencia de Paradas
                if (gvInfoViaje.DataKeys.Count > 0)
                {
                    //Buscamos Grid View de Eventos
                    using (GridView gvParadas = (GridView)e.Row.FindControl("gvParadas"))
                    {
                        //Carga Eventos para cada una de las Paradas
                        using (DataTable mit = SAT_CL.Despacho.Parada.CargaParadasParaVisualizacion(Convert.ToInt32(gvInfoViaje.DataKeys[e.Row.RowIndex].Value)))
                        {
                            //Validamos Origen de Datos
                            if (TSDK.Datos.Validacion.ValidaOrigenDatos(mit))
                            {
                                //Cargamos Grid View Eventos
                                TSDK.ASP.Controles.CargaGridView(gvParadas, mit, "Id", "");

                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Evento generado al cambiar el Sort de Info de Viajes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvInfoViaje_Sorting(object sender, GridViewSortEventArgs e)
        {
            lblOrdenadoInfoViaje.Text = Controles.CambiaSortExpressionGridView(gvInfoViaje, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table4"), e.SortExpression, false, 4);

        }

        /// <summary>
        /// Evento Generado al Aceptar la Publicacion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbAceptarPublicacion_Click(object sender, EventArgs e)
        {
            //Validamos Origen de Datos
            if (gvInfoViaje.DataKeys.Count > 0)
            {
                //Seleccionando la fila correspondiente
                Controles.SeleccionaFila(gvInfoViaje, sender, "lnk", false);

                //Abre Ventana modal
                alternaVentanaModal("confirmarRespuestaPU", gvInfoViaje);
                //Cerramos Ventana modal
                alternaVentanaModal("informacionViajes", gvInfoViaje);

            }
        }


        /// <summary>
        /// Click en algún Link de GV de Respúestas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbAccionRespuestaPU_Click(object sender, EventArgs e)
        {
            //Si hay registros
            if (gvRespuestas.DataKeys.Count > 0)
            {
                //Seleccionando la fila correspondiente
                Controles.SeleccionaFila(gvRespuestas, sender, "lnk", false);
                //Convirtiendo objeto a linkbutton
                LinkButton lkb = (LinkButton)sender;

                //Determinando el comando por ejecutar
                switch (lkb.CommandName)
                {
                    case "Aceptar":
                        //limpiamos  Control de Tarifa
                        txtTarifaAceptadaPU.Text = "";//string.Format(gvRespuestas.SelectedDataKey["TarifaOfertada"].ToString(), "0:C2");
                        // Abriendo ventana 
                        alternaVentanaModal("aceptarRespuesta", lkb);
                        //Cerramos Modal
                        alternaVentanaModal("opcionSeleccionRespuesta", lkb);
                        break;
                    case "Confirmar":
                        // Abriendo ventana para de Ciudades
                        alternaVentanaModal("informacionViajes", lkb);
                        //Carga Viajes
                        cargaServicios();
                        //Cerramos Modal
                        alternaVentanaModal("opcionSeleccionRespuesta", lkb);
                        break;
                }
            }
        }

        /// <summary>
        /// Evento Generado al Crear la Respuesta
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCrearRespuesta_Click(object sender, EventArgs e)
        {

            //Abriendo ventana
            alternaVentanaModal("crearRespuesta", lkbCrearRespuesta);
            //Cerramos Ventana de Respuestas
            alternaVentanaModal("resultadoRespuesta", lkbCrearRespuesta);
            //Inicializamos Controles
            inicializaControlesCrearRespuesta(0);
        }

        /// <summary>
        /// Insertamos un Evento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbInsertar_Click(object sender, EventArgs e)
        {
            insertaEventosDeseadosTemporal();
        }

        /// <summary>
        /// Evento generado al Eliminar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbEliminar_Click(object sender, EventArgs e)
        {
            //Validamos Registros
            if (gvEventosDeseados.DataKeys.Count > 0)
            {
                //Seleccionando la fila correspondiente
                Controles.SeleccionaFila(gvEventosDeseados, sender, "lnk", false);
                eliminaEventosDeseadosTemporal();
            }
        }
        #endregion

        #region Métodos Publicación de Unidades

        /// <summary>
        /// Método encargado de cargar las Publicaciones de Unidades
        /// </summary>
        private void cargaPublicaciones()
        {
            //Obtenemos Depósito
            using (DataTable mit = consumoCargaPublicaciones())
            {

                //Validando que el DataSet contenga las tablas
                if (Validacion.ValidaOrigenDatos(mit))
                {
                    //Cargando los GridView
                    Controles.CargaGridView(gvUnidades, mit, "Id-IdRespuesta", lblOrdenadoUnidades.Text, true, 3);
                    //Añadiendo Tablas al DataSet de Session
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], mit, "Table");

                }
                else
                {   //Inicializando GridViews
                    Controles.InicializaGridview(gvUnidades);


                    //Eliminando Tablas del DataSet de Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                }
            }
        }

        /// <summary>
        /// Carga las Publicaciones
        /// </summary>
        /// <returns></returns>
        private DataTable consumoCargaPublicaciones()
        {
            //Establecemos variable resultado
            DataSet ds = new DataSet();
            DataTable mit = null;
            RetornoOperacion resultado = new RetornoOperacion();

            string fecha_inicio = "";
            string fecha_fin = "";
            //Validamos Selección de Fechas
            if (chkIncluir.Checked)
            {
                fecha_inicio = Fecha.ConvierteDateTimeString(Convert.ToDateTime(txtFecIni.Text), "yyyy-MM-dd HH:mm");
                fecha_fin = Fecha.ConvierteDateTimeString(Convert.ToDateTime(txtFecFin.Text), "yyyy-MM-dd HH:mm");
            }
            //Consumimos Web Service
            using (DespachoCentral.DespachoClient despacho = new DespachoCentral.DespachoClient())
            {
                //Instanciamos Usuario
                using (SAT_CL.Seguridad.Usuario objUsuario = new SAT_CL.Seguridad.Usuario(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario))
                {
                    //Instanciamos Compañia
                    using (SAT_CL.Global.CompaniaEmisorReceptor objCompania = new CompaniaEmisorReceptor(((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                    {

                        string resultado_web_service = despacho.ObtienePublicacionesCompania(objCompania.id_compania_emisor_receptor, "", txtCiudad.Text, fecha_inicio, fecha_fin, CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Direccion del Servidor", 0), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Nombre de BD", 0),
                                           TSDK.Base.Cadena.VerificaCadenaVacia(SAT_CL.Global.Referencia.CargaReferencia("0", 25, objCompania.id_compania_emisor_receptor, "Consumo Web Service", "Contraseña"), ""));

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
                            ds.ReadXml(xDoc.Document.Element("Publicaciones").Element("NewDataSet").CreateReader());
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
                            TSDK.ASP.ScriptServer.MuestraNotificacion(btnBuscarUnidades, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
                        }
                    }
                }
                despacho.Close();
            }
            return mit;
        }

        /// <summary>
        /// Método encargado de cargar las Respuestas
        /// </summary>
        private void cargaCiudadesPublicacion()
        {
            //Obtenemos Depósito
            using (DataTable mit = consumoCiudades())
            {

                //Validando que el DataSet contenga las tablas
                if (Validacion.ValidaOrigenDatos(mit))
                {
                    //Cargando los GridView
                    Controles.CargaGridView(gvCiudades, mit, "Id", lblOrdenadoCiudades.Text, false, 3);
                    //Añadiendo Tablas al DataSet de Session
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], mit, "Table8");

                }
                else
                {   //Inicializando GridViews
                    Controles.InicializaGridview(gvCiudades);
                    //Eliminando Tablas del DataSet de Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table8");
                }
            }
        }

        /// <summary>
        ///  Método encargado de cargar las Ciudades
        /// </summary>
        /// <param name="id_respuesta"></param>
        /// <param name="control"></param>
        /// <returns></returns>
        private DataTable cargaCiudades(int id_respuesta, System.Web.UI.Control control)
        {
            //Declaramos Objeto resultado
            DataTable mit = new DataTable();
            //Obtenemos las Ciudades
            using (DataSet ds = consumoRespuestas(control))
            {

                //Validando que el DataSet contenga las tablas
                if (Validacion.ValidaOrigenDatos(ds.Tables["Table1"]))
                {
                    //Obtenemos La Referencias del Concepto Origen
                    DataRow[] re = (from DataRow r in ds.Tables["Table1"].Rows
                                    where Convert.ToInt32(r["IdRespuesta"]) == id_respuesta
                                    select r).ToArray();
                    //Validamos que exista elementos
                    if (re.Length > 0)
                    {
                        //Obtenemos Datatable Referencias solo del Concepto Origen
                        mit = TSDK.Datos.OrigenDatos.ConvierteArregloDataRowADataTable(re);
                    }


                }
            }
            //Devolvemos Resultado
            return mit;
        }
        /// <summary>
        /// Carga las Ciudades
        /// </summary>
        /// <returns></returns>
        private DataTable consumoCiudades()
        {
            //Establecemos variable resultado
            DataSet ds = new DataSet();
            DataTable mit = null;
            RetornoOperacion resultado = new RetornoOperacion();

            //Consumimos Web Service
            DespachoCentral.DespachoClient despacho = new DespachoCentral.DespachoClient();

            //Instanciamos Compañia
            using (SAT_CL.Global.CompaniaEmisorReceptor objCompania = new CompaniaEmisorReceptor(((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
            {


                string resultado_web_service = despacho.ObtieneCiudadesPublicacion(Convert.ToInt32(gvUnidades.SelectedValue), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Direccion del Servidor", 0), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Nombre de BD", 0),
                                   TSDK.Base.Cadena.VerificaCadenaVacia(SAT_CL.Global.Referencia.CargaReferencia("0", 25, objCompania.id_compania_emisor_receptor, "Consumo Web Service", "Contraseña"), ""));

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
                    ds.ReadXml(xDoc.Document.Element("CiudadesPublicacion").Element("NewDataSet").CreateReader());
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
                    TSDK.ASP.ScriptServer.MuestraNotificacion(gvUnidades, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
                }
            }
            return mit;
        }
        /// <summary>
        /// Método encargado de cargar las Respuestas
        /// </summary>
        /// <param name="control"></param>
        private void cargaResultadoRespuesta(System.Web.UI.Control control)
        {
            //Obtenemos los Resultados
            using (DataTable mit = consumoResultadoRespuestas(control))
            {

                //Validando que el DataSet contenga las tablas
                if (Validacion.ValidaOrigenDatos(mit))
                {
                    //Cargando los GridView
                    Controles.CargaGridView(gvResultadoRespuesta, mit, "Id", lblOrdenadoResultadoRespuesta.Text, false, 3);
                    //Añadiendo Tablas al DataSet de Session
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], mit, "Table6");

                }
                else
                {   //Inicializando GridView
                    Controles.InicializaGridview(gvResultadoRespuesta);
                    //Eliminando Tablas del DataSet de Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table6");
                }
            }
        }

        /// <summary>
        /// Carga los Resultados de la Respuestas
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        private DataTable consumoResultadoRespuestas(System.Web.UI.Control control)
        {
            //Establecemos variable resultado
            DataSet ds = new DataSet();
            RetornoOperacion resultado = new RetornoOperacion();

            //Consumimos Web Service
            DespachoCentral.DespachoClient despacho = new DespachoCentral.DespachoClient();


            string resultado_web_service = despacho.ObtieneRespuestasPublicacion(Convert.ToInt32(gvUnidades.SelectedValue), Convert.ToInt32(gvUnidades.SelectedDataKey["Indicador"]) == 1 ? true : false, ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Direccion del Servidor", 0), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Nombre de BD", 0),
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
                ds.ReadXml(xDoc.Document.Element("RespuestasPublicacion").Element("NewDataSet").CreateReader());
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
                TSDK.ASP.ScriptServer.MuestraNotificacion(control, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
            return ds.Tables["Table"];
        }

        /// <summary>
        /// Construye el esquema de tablas temporales
        /// </summary>
        private void creaEsquemaTablasTemporales()
        {
            //Ciudades
            DataTable mitEventos = new DataTable();
            mitEventos.Columns.Add("Id", typeof(int));
            mitEventos.Columns.Add("Secuencia", typeof(decimal));
            mitEventos.Columns.Add("Ciudad", typeof(String));
            mitEventos.Columns.Add("Actividad", typeof(string));
            mitEventos.Columns.Add("Cita", typeof(DateTime));

            //Añadiendo Tablas al DataSet de Session
            OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], mitEventos, "Table2");
        }

        /// <summary>
        ///  Carga las Ciudades deseadas a los criterios de búsqueda solicitados
        /// </summary>
        /// <param name="id_respuestas"></param>
        private void cargaEventosDeseados(int id_respuesta)
        {
            //Validamos que no exista Respuesta
            if (id_respuesta == 0)
            {
                //Validamos Origen de Datos
                if (OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2") == null)
                    TSDK.ASP.Controles.InicializaGridview(gvEventosDeseados);
                else
                    //Inicializamos Indices
                    Controles.InicializaIndices(gvEventosDeseados);
                //Mostrandolos en gridview
                TSDK.ASP.Controles.CargaGridView(gvEventosDeseados, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), "Secuencia", lblOrdenadoEventosDeseados.Text, false, 1);
            }
            else
            {
                //Cargamos Ciudades Destinos Deseadas 
                DataTable mit = consumoInicializaEventos();

                //Añadiendo Tablas al DataSet de Session
                Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], mit, "Table2");

                //Inicializamos Indices
                Controles.InicializaIndices(gvEventosDeseados);
                //Mostrandolos en gridview
                TSDK.ASP.Controles.CargaGridView(gvEventosDeseados, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), "Secuencia", lblOrdenadoEventosDeseados.Text, false, 1);

            }
        }

        /// <summary>
        /// Inserta un Evento Temporal Deseada en la tabla temporal
        /// </summary>
        private void insertaEventosDeseadosTemporal()
        {
            //Declaramos Objeto
            int id = 1;

            if (TSDK.Datos.Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2")))
            {
                //Obteniendo secuencia para nuevo evento
                id = (from DataRow r in OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2").Rows
                      select Convert.ToInt32(r["Id"])).DefaultIfEmpty(0).Max() + 1;
            }
            else
            {
                //Creamos Tabla Temporal
                creaEsquemaTablasTemporales();
            }
            //Declarando objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion(id);

            try
            {

                //Creando nueva fila de tabla de eventos
                DataRow nr = OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2").NewRow();
                //Recuperando controles de catálogo
                using (TextBox txtSecuencia = (TextBox)gvEventosDeseados.FooterRow.FindControl("txtSecuenciaDeseada"),
                        txtCiudad = (TextBox)gvEventosDeseados.FooterRow.FindControl("txtCiudadDeseada"),
                          txtCita = (TextBox)gvEventosDeseados.FooterRow.FindControl("txtCitaDeseada"))
                {
                    using (DropDownList ddlActividad = (DropDownList)gvEventosDeseados.FooterRow.FindControl("ddlActividadDeseada"))
                    {
                        //Añadiendo atributos de evento
                        nr.SetField("Id", id);
                        nr.SetField("Secuencia", txtSecuencia.Text);
                        nr.SetField("Ciudad", Cadena.RegresaCadenaSeparada(txtCiudad.Text, ',', 0));
                        nr.SetField("Actividad", ddlActividad.SelectedItem);
                        nr.SetField("Cita", txtCita.Text);
                    }
                }
                //Insertando evento en tabla temporal
                OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2").Rows.Add(nr);

            }
            catch (Exception ex) { resultado = new RetornoOperacion(string.Format("Error al insertar evento: {0}", ex.Message)); }


            //Mostrando resultado
            ScriptServer.MuestraNotificacion(gvEventosDeseados, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);

            //Carga Eventos
            cargaEventosDeseados(0);

        }

        /// <summary>
        /// Realiza la eliminación de del Evento deseado
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion eliminaEventosDeseadosTemporal()
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Guardamos Id
            int Id = Convert.ToInt32(gvEventosDeseados.SelectedValue);
            //Obteniendo la fila en el origen de datos del producto seleccionado
            DataRow r = (from DataRow p in OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2").Rows
                         where Convert.ToInt32(p["Id"]) == Convert.ToInt32(gvEventosDeseados.SelectedValue)
                         select p).FirstOrDefault();

            //Eliminando Ciudad temporal
            OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2").Rows.Remove(r);

            resultado = new RetornoOperacion(Id);

            //Validamos Resultado
            if (resultado.OperacionExitosa)
            {
                cargaEventosDeseados(0);
                //Iniciamizamos Indices
                Controles.InicializaIndices(gvEventosDeseados);
            }

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(gvEventosDeseados, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);

            //Devolviendo resultado
            return resultado;
        }

        /// <summary>
        /// Inicializa de manera general los componentes  de la creación de la Respuesta de una Unidad
        /// </summary>
        private void inicializaValoresCrearRespuesta(int id_respuesta)
        {
            //Validamos Id Respuesta
            if (id_respuesta == 0)
            {
                //Cargamos Publicaciones de Viajes Propias
                txtEstatus.Text = "Publicada";
                txtProducto.Text = "";
                txtPeso.Text = "";
                txtTarifa.Text = "";
                lblTarifaOfertada.Text = "$0.00";
                txtTelefono.Text = "";
                txtObservacion.Text = "";
                txtContacto.Text = "";
            }
            else
            {
                //Obtenemos Datos de la Respuesta
                DataTable mitRespuesta = consumoInicializaRespuesta(id_respuesta);

                //Validando que el DataSet contenga las tablas
                if (Validacion.ValidaOrigenDatos(mitRespuesta))
                {
                    //Obtenemos La Referencias del Concepto Origen
                    DataRow re = (from DataRow r in mitRespuesta.Rows
                                  select r).FirstOrDefault();
                    //Validamos que exista elementos
                    if (re != null)
                    {
                        //Cargamos Publicaciones de Viajes Propias
                        txtEstatus.Text = re.Field<string>("Estatus");
                        txtProducto.Text = re.Field<string>("ProductoOrigen");
                        txtPeso.Text = Convert.ToInt32(re["Peso"]).ToString();
                        txtTarifa.Text = string.Format("{0:C2}", Convert.ToInt32(re["TarifaOfertada"]));
                        lblTarifaOfertada.Text = string.Format("{0:C2}", Convert.ToInt32(re["TarifaAceptada"]));
                        txtTelefono.Text = re.Field<string>("TelefonoContacto");
                        txtObservacion.Text = re.Field<string>("Observacion");
                        txtContacto.Text = re.Field<string>("Contacto");

                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnConfirmar_Click(object sender, EventArgs e)
        {
            // Abriendo ventana para de Ciudades
            alternaVentanaModal("informacionViajes", btnConfirmar);
            //Carga Viajes
            cargaServicios();
            //Abriendo ventana
            alternaVentanaModal("crearRespuesta", btnConfirmar);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnConfirmarPS_Click(object sender, EventArgs e)
        {
            //Cerramos Ventana modal
            alternaVentanaModal("confirmarRespuestaPU", btnConfirmarPS);
            //Cerramos ventana
            alternaVentanaModal("crearRespuestaPS", btnConfirmarPS);
        }
        /// <summary>
        /// Inicializa de manera general los componentes  de la creación de la Respuesta de una Unidad
        /// </summary>
        private void habilitaControlesCrearRespuesta(int id_respuesta)
        {
            //Validamos Id Respuesta
            if (id_respuesta == 0)
            {
                //Habilitamos Controles
                txtEstatus.Enabled = false;
                txtProducto.Enabled =
                txtPeso.Enabled =
                txtTarifa.Enabled =
                txtTelefono.Enabled =
                txtObservacion.Enabled =
                txtContacto.Enabled =
                gvEventosDeseados.Enabled = true;
            }
            else
            {
                //deshabilitamos Controles
                txtEstatus.Enabled = false;
                txtProducto.Enabled =
                txtPeso.Enabled =
                txtTarifa.Enabled =
                txtTelefono.Enabled =
                txtObservacion.Enabled =
                txtContacto.Enabled =
                gvEventosDeseados.Enabled = false;
            }
        }

        /// <summary>
        /// Inicializa de manera general los componentes de la forma
        /// </summary>
        private void inicializaControlesCrearRespuesta(int id_respuesta)
        {
            //Inicializamos Valores
            inicializaValoresCrearRespuesta(id_respuesta);
            //Habilitamos Controles
            habilitaControlesCrearRespuesta(id_respuesta);
            //Validamos Origen de Datos
            if (OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2") != null)
            {
                //Limpiamos Tabla
                OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2").Clear();
            }
            //Cargamos Eventos No Deseados
            cargaEventosDeseados(id_respuesta);
        }

        /// <summary>
        /// Reesponde la Publicación de una Unidad
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion respondePublicacion()
        {
            //Establecemos variable resultado
            RetornoOperacion resultado = null;

            //Consumimos Web Service
            using (DespachoCentral.DespachoClient despacho = new DespachoCentral.DespachoClient())
            {
                //Instanciamos Compañia
                using (SAT_CL.Global.CompaniaEmisorReceptor objCompania = new CompaniaEmisorReceptor(((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                {

                    string resultado_web_service = despacho.RespuestaPublicacion(((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, objCompania.nombre, Convert.ToInt32(gvUnidades.SelectedValue), Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtProducto.Text, ':', 1)),
                                       Cadena.RegresaCadenaSeparada(txtProducto.Text, "ID:", 0), Convert.ToDecimal(txtPeso.Text), Convert.ToDecimal(txtTarifa.Text), txtObservacion.Text, txtContacto.Text, txtTelefono.Text, SAT_CL.Despacho.ConsumoSOAPCentral.ObtieneEventosDeseadas(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2")),
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


                    }
                    else
                    {
                        //Establecmos Mensaje Resultado
                        resultado = new RetornoOperacion("No es posible obtener la respuesta WS");
                    }
                    //Mostrando Mensaje de Operación
                    TSDK.ASP.ScriptServer.MuestraNotificacion(btnCrearRespuesta, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
                }
                despacho.Close();
                return resultado;
            }
        }


        /// <summary>
        /// Carga las Respuestas
        /// </summary>
        /// <param name="control"></param>
        private void cargaRespuestas(System.Web.UI.Control control)
        {
            //Obtenemos Detalle de la Respuesta
            using (DataSet ds = consumoRespuestas(control))
            {

                //Validando que el DataSet contenga las tablas
                if (Validacion.ValidaOrigenDatos(ds.Tables["Table"]))
                {
                    //Cargando los GridView
                    Controles.CargaGridView(gvRespuestas, ds.Tables["Table"], "Id-TarifaOfertada", lblOrdenadoRespuestas.Text, false, 3);
                    //Añadiendo Tablas al DataSet de Session
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], ds.Tables["Table"], "Table3");

                }
                else
                {   //Inicializando GridViews
                    Controles.InicializaGridview(gvRespuestas);
                    //Eliminando Tablas del DataSet de Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table3");
                }
            }
        }

        /// <summary>
        /// Obtenemos Información de los Eventos
        /// </summary>
        /// <returns></returns>
        private DataTable consumoInicializaEventos()
        {
            //Establecemos variable resultado
            DataSet ds = new DataSet();
            DataTable mit = null;
            RetornoOperacion resultado = new RetornoOperacion();

            //Consumimos Web Service
            using (DespachoCentral.DespachoClient despacho = new DespachoCentral.DespachoClient())
            {

                string resultado_web_service = despacho.ObtieneCiudadesEventoRespuesta(Convert.ToInt32(gvUnidades.SelectedDataKey["IdRespuesta"]), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Direccion del Servidor", 0), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Nombre de BD", 0),
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
                    ds.ReadXml(xDoc.Document.Element("CiudadesEventoRespuesta").Element("NewDataSet").CreateReader());
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
        /// Obtenemoslod datos para Inicializar la Respuesta
        /// </summary>
        /// <returns></returns>
        private DataTable consumoInicializaRespuesta(int id_respuesta)
        {
            //Establecemos variable resultado
            DataSet ds = new DataSet();
            DataTable mit = null;
            RetornoOperacion resultado = new RetornoOperacion();

            //Consumimos Web Service
            using (DespachoCentral.DespachoClient despacho = new DespachoCentral.DespachoClient())
            {

                string resultado_web_service = despacho.ObtieneDatosRespuesta(id_respuesta, CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Direccion del Servidor", 0), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Nombre de BD", 0),
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
                    ds.ReadXml(xDoc.Document.Element("Respuesta").Element("NewDataSet").CreateReader());
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
        /// Obtenemoslod datos para Inicializar la Respuesta
        /// </summary>
        /// <returns></returns>
        private DataTable consumoInicializaRespuestaPS()
        {
            //Establecemos variable resultado
            DataSet ds = new DataSet();
            DataTable mit = null;
            RetornoOperacion resultado = new RetornoOperacion();

            //Consumimos Web Service
            using (DespachoCentral.DespachoClient despacho = new DespachoCentral.DespachoClient())
            {

                string resultado_web_service = despacho.ObtieneDatosRespuesta(Convert.ToInt32(gvUnidades.SelectedDataKey["IdRespuesta"]), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Direccion del Servidor", 0), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Nombre de BD", 0),
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
                    ds.ReadXml(xDoc.Document.Element("Respuesta").Element("NewDataSet").CreateReader());
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
        /// Carga las Respuestas
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        private DataSet consumoRespuestas(System.Web.UI.Control control)
        {
            //Establecemos variable resultado
            DataSet ds = new DataSet();
            RetornoOperacion resultado = new RetornoOperacion();

            //Consumimos Web Service
            DespachoCentral.DespachoClient despacho = new DespachoCentral.DespachoClient();


            string resultado_web_service = despacho.VisualizarRespuesta(Convert.ToInt32(gvResultadoRespuesta.SelectedValue), ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Direccion del Servidor", 0), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Nombre de BD", 0),
                               TSDK.Base.Cadena.VerificaCadenaVacia(SAT_CL.Global.Referencia.CargaReferencia("0", 25, ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "Consumo Web Service", "Contraseña"), ""), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario,
                               ((SAT_CL.Seguridad.Usuario)Session["usuario"]).nombre);

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
                ds.ReadXml(xDoc.Document.Element("VisualizacionRespuesta").Element("NewDataSet").CreateReader());
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
                TSDK.ASP.ScriptServer.MuestraNotificacion(control, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
            return ds;
        }
        /// <summary>
        /// Aceptar la Respuesta
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion aceptarRespuesta()
        {
            //Establecemos variable resultado
            RetornoOperacion resultado = null;

            //Consumimos Web Service
            DespachoCentral.DespachoClient despacho = new DespachoCentral.DespachoClient();


            //Instanciamos Compañia
            using (SAT_CL.Global.CompaniaEmisorReceptor objCompania = new CompaniaEmisorReceptor(((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
            {

                string resultado_web_service = despacho.AceptaRespuestaPublicacion(Convert.ToInt32(gvRespuestas.SelectedValue), Convert.ToDecimal(txtTarifaAceptadaPU.Text), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Direccion del Servidor", 0), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Nombre de BD", 0),
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
                        resultado = new RetornoOperacion("La Respuesta ha sido Aceptada", true);
                    }

                }
                else
                {
                    //Establecmos Mensaje Resultado
                    resultado = new RetornoOperacion("No es posible obtener la respuesta de WS");
                }
                //Mostrando Mensaje de Operación
                TSDK.ASP.ScriptServer.MuestraNotificacion(btnAceptarRespuesta, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
            return resultado;
        }

        /// <summary>
        /// Realiza la búsqueda y muestra los servicios 
        /// </summary>
        private void cargaServicios()
        {
            /*/Cargando Unidades
            using (DataTable mit = SAT_CL.Despacho.Reporte.CargaPlaneacionServicios(((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, 0, "", true, false))
            {
                //Guardando en sesión el origen de datos
                if (Validacion.ValidaOrigenDatos(mit))
                {
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], mit, "Table4");
                    //Cargando los GridView
                    Controles.CargaGridView(gvInfoViaje, mit, "id_servicio", lblOrdenadoInfoViaje.Text, false, 3);
                }
                else
                {
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table4");
                    //Inicializando GridViews
                    Controles.InicializaGridview(gvInfoViaje);
                }
            }//*/

        }
        /// <summary>
        /// Método Encargado de Copiar un Servicio de la Publicación de Unidad
        /// </summary>
        private void copiarServicioPSFinal(System.Web.UI.Control control, bool validacion)
        {
            //Declaramos Objeto Retorno
            RetornoOperacion resultado = new RetornoOperacion(0);
            DataSet ds = new DataSet();
            int id_servicio = 0;
            if (validacion == false)
            {
                //Mostrando ventana modal correspondiente
                resultado = muestraElemento(control, out ds);
            }
            //Validamos resultado
            if (resultado.OperacionExitosa)
            {
                //Validamos Tipo de Información
                if (mtvPublicacion.ActiveViewIndex == 0)
                {
                    //Validamos Obtención de Información del Viaje de la Publicacipón de Unidad
                    ds = obtieneServicioPU(control, out resultado);
                }
                else
                {
                    //Validamos Obtención de Información del Viaje de la Publicacipón de Servicio
                    ds = obtieneServicioPS(control, out resultado);
                }
                //Seleccionando el servicio
                DataRow s = (from DataRow r in ds.Tables["Servicio"].Rows
                             select r).FirstOrDefault();

                int id_movimiento = 0;
                //Copiamos Servicio
                resultado = SAT_CL.Documentacion.Servicio.CopiarServicioPublicacion(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                           Convert.ToInt32(s["idCliente"]), s["noViaje"].ToString(), s["confirmacion"].ToString(),
                           s["observacion"].ToString(), Convert.ToInt32(s["idProducto"]), 0, Convert.ToDecimal(s["peso"]), Convert.ToDecimal(s["tarifaAceptada"]),
                          Convert.ToInt32(s["idConcepto"]), ds.Tables["Parada"], 1, out id_movimiento, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                //Validamos resultado
                if (resultado.OperacionExitosa)
                {
                    //Asignamos Id de Servicio
                    id_servicio = resultado.IdRegistro;
                    //Actualizamos Id de Servicio
                    resultado = actualizaIdServicioDestinoPS(id_servicio);
                }

                //Si no hay errores
                if (resultado.OperacionExitosa)
                {
                    //Crenado resultado con Id de Servicio nuevo
                    resultado = new RetornoOperacion(id_servicio);
                    //Cerramos Modal
                    if (lblValor.Text == "Si")
                    {
                        //Cerramos Ventana Modal
                        alternaVentanaModal("elemento1", control);
                    }
                    //De acuerdo al Tipo de Publicación Cargamos Publicaciones
                    if (mtvPublicacion.ActiveViewIndex == 0)
                    {
                        //Cargamos Publicaciones de Unidades
                        cargaPublicaciones();
                    }
                    else
                    {
                        //Carganos Publicaciones de Servicio
                        cargaPublicacionesServicios();
                    }
                }
            }

            //Mostrando Mensaje de Operación
            TSDK.ASP.ScriptServer.MuestraNotificacion(control, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        ///  Método Encargado de Copiar un Servicio de la Publicación de Unidad
        /// </summary>
        /// <param name="control"></param>
        /// <param name="validacion"></param>
        private void copiarServicioPUFinal(System.Web.UI.Control control, bool validacion)
        {
            //Declaramos Objeto Retorno
            RetornoOperacion resultado = new RetornoOperacion();
            DataSet ds = new DataSet();
            int id_servicio = 0;
            if (validacion == false)
            {
                //Mostrando ventana modal correspondiente
                resultado = muestraElemento(control, out ds);
            }
            //Validamos resultado
            if (resultado.OperacionExitosa)
            {
                //Validamos Tipo de Información
                if (mtvPublicacion.ActiveViewIndex == 0)
                {
                    //Validamos Obtención de Información del Viaje de la Publicacipón de Unidad
                    ds = obtieneServicioPU(control, out resultado);
                }
                else
                {
                    //Validamos Obtención de Información del Viaje de la Publicacipón de Servicio
                    ds = obtieneServicioPS(control, out resultado);
                }
                //Seleccionando el servicio
                DataRow s = (from DataRow r in ds.Tables["Servicio"].Rows
                             select r).FirstOrDefault();

                //Declaramos Movimiento
                int id_movimiento = 0;
                //Copiamos Servicio
                resultado = SAT_CL.Documentacion.Servicio.CopiarServicioPublicacion(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                           Convert.ToInt32(s["idCliente"]), s["noViaje"].ToString(), s["confirmacion"].ToString(),
                           s["observacion"].ToString(), Convert.ToInt32(s["idProducto"]), 0, Convert.ToDecimal(s["peso"]), Convert.ToDecimal(s["tarifaAceptada"]),
                          Convert.ToInt32(s["idConcepto"]), ds.Tables["Parada"], 1, out id_movimiento, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                //Validamos resultado
                if (resultado.OperacionExitosa)
                {
                    //Asignamos Id de Servicio
                    id_servicio = resultado.IdRegistro;
                    //Actualizamos Id de Servicio
                    resultado = actualizaIdServicioDestino(id_servicio);
                }

                //Si no hay errores
                if (resultado.OperacionExitosa)
                {
                    //Crenado resultado con Id de Servicio nuevo
                    resultado = new RetornoOperacion(id_servicio);
                    //Cerramos Modal
                    if (lblValor.Text == "Si")
                    {
                        //Cerramos Ventana Modal
                        alternaVentanaModal("elemento", control);
                    }
                }
            }

            //Mostrando Mensaje de Operación
            TSDK.ASP.ScriptServer.MuestraNotificacion(control, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        /// <summary>
        /// Actualiza el Id de Servicio Destino
        /// </summary>
        /// <param name="id_servicio"></param>
        /// <returns></returns>
        private RetornoOperacion actualizaIdServicioDestino(int id_servicio)
        {
            //Establecemos variable resultado
            RetornoOperacion resultado = null;

            //Consumimos Web Service
            using (DespachoCentral.DespachoClient despacho = new DespachoCentral.DespachoClient())
            {
                //Instanciamos Compañia
                using (SAT_CL.Global.CompaniaEmisorReceptor objCompania = new CompaniaEmisorReceptor(((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                {

                    string resultado_web_service = despacho.ActualizaServicioUnidad(Convert.ToInt32(gvUnidades.SelectedValue), id_servicio, CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Direccion del Servidor", 0), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Nombre de BD", 0),
                                       TSDK.Base.Cadena.VerificaCadenaVacia(SAT_CL.Global.Referencia.CargaReferencia("0", 25, objCompania.id_compania_emisor_receptor, "Consumo Web Service", "Contraseña"), ""),
                                       ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).nombre);


                    //Obtenemos Documento generado
                    XDocument xDoc = XDocument.Parse(resultado_web_service);

                    //Validamos que exista Respuesta
                    if (xDoc != null)
                    {
                        //Traduciendo resultado
                        resultado = new RetornoOperacion(Convert.ToInt32(xDoc.Descendants("idRegistro").FirstOrDefault().Value), xDoc.Descendants("mensaje").FirstOrDefault().Value.ToString(), Convert.ToBoolean(xDoc.Descendants("operacionExitosa").FirstOrDefault().Value));


                    }
                    else
                    {
                        //Establecmos Mensaje Resultado
                        resultado = new RetornoOperacion("No es posible obtener la respuesta de WS");
                    }
                }
                //Cerramos Web Service
                despacho.Close();
            }
            return resultado;
        }
        /// <summary>
        /// Obtenemos la Información del Servicio de Unidad
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion documentaServicioUnidad()
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
                    using (SAT_CL.Documentacion.Servicio objServicio = new SAT_CL.Documentacion.Servicio(Convert.ToInt32(gvInfoViaje.SelectedValue)))
                    {
                        //Validamos Estatus del Servicio
                        if (objServicio.estatus == SAT_CL.Documentacion.Servicio.Estatus.Documentado)
                        {
                            //Instanciamos Compañia Receptor
                            using (SAT_CL.Global.CompaniaEmisorReceptor objCompaniaReceptor = new CompaniaEmisorReceptor(objServicio.id_cliente_receptor))
                            {
                                //Obtenemos Resultado
                                string resultado_web_service = despacho.DocumentaServicioUnidad(Convert.ToInt32(gvUnidades.SelectedDataKey["IdRespuesta"]), objServicio.id_servicio, objServicio.id_compania_emisor, objCompania.nombre, objServicio.no_servicio, SAT_CL.Global.Referencia.CargaReferencia(objCompania.id_compania_emisor_receptor.ToString(), 1, objServicio.id_servicio, "Referencia de Viaje", "Confirmación"),
                                                                                              SAT_CL.Tarifas.TipoCargo.ObtieneTipoCargoDescripcion(objCompania.id_compania_emisor_receptor, "Flete"), SAT_CL.Despacho.ConsumoSOAPCentral.ObtieneParadasPublicacionUnidad(SAT_CL.Despacho.Parada.CargaParadasParaPublicacionDeUnidad(objServicio.id_servicio)),
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

                                }
                                else
                                {
                                    //Establecmos Mensaje Resultado
                                    resultado = new RetornoOperacion("No es posible obtener la respuesta WS");
                                }
                            }
                        }
                        else
                        {
                            //Establecemos Mensaje Reusltado 
                            resultado = new RetornoOperacion("El servicio debe estar Documentado.");
                        }

                    }
                }
                despacho.Close();
                return resultado;
            }
        }

        /// <summary>
        /// Copia un Servcio de una Publicacion de Unidad
        /// </summary>
        /// <returns></returns>
        private DataSet obtieneServicioPU(System.Web.UI.Control control, out RetornoOperacion resultado)
        {
            //Establecemos variable resultado
            DataSet ds = new DataSet();
            resultado = new RetornoOperacion();

            //Consumimos Web Service
            using (DespachoCentral.DespachoClient despacho = new DespachoCentral.DespachoClient())
            {
                //Instanciamos Usuario
                using (SAT_CL.Seguridad.Usuario objUsuario = new SAT_CL.Seguridad.Usuario(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario))
                {
                    //Instanciamos Compañia
                    using (SAT_CL.Global.CompaniaEmisorReceptor objCompania = new CompaniaEmisorReceptor(((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                    {

                        string resultado_web_service = despacho.ObtieneDatosServicioUnidad(Convert.ToInt32(gvUnidades.SelectedValue), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Direccion del Servidor", 0), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Nombre de BD", 0),
                                           TSDK.Base.Cadena.VerificaCadenaVacia(SAT_CL.Global.Referencia.CargaReferencia("0", 25, objCompania.id_compania_emisor_receptor, "Consumo Web Service", "Contraseña"), ""));

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
                            ds.ReadXml(xDoc.Document.Element("Confirmacion").CreateReader());
                        }
                    }
                }
                despacho.Close();
            }
            return ds;
        }


        /// <summary>
        ///Insertamos Relación de Concepto
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion insertaRelacionConcepto()
        {
            //Establecemos variable resultado
            RetornoOperacion resultado = null;

            //Consumimos Web Service
            using (GlobalCentral.GlobalClient global = new GlobalCentral.GlobalClient())
            {
                //Instanciamos Compañia
                using (SAT_CL.Global.CompaniaEmisorReceptor objCompania = new CompaniaEmisorReceptor(((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                {

                    string resultado_web_service = global.InsertaRelacionConceptoFletePublicacion(Convert.ToInt32(lblIdElemento.Text), Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtElemento.Text, "ID:", 1)), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Direccion del Servidor", 0), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Nombre de BD", 0),
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
                            resultado = new RetornoOperacion("Tu respuesta ha sido Registrada", true);
                        }

                    }
                    else
                    {
                        //Establecmos Mensaje Resultado
                        resultado = new RetornoOperacion("No es posible obtener la respuesta WS");
                    }
                }
                global.Close();
                return resultado;
            }
        }

        /// <summary>
        ///Insertamos Relación de Ubicacion
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion insertaRelacionUbicacion()
        {
            //Establecemos variable resultado
            RetornoOperacion resultado = null;

            //Consumimos Web Service
            using (GlobalCentral.GlobalClient global = new GlobalCentral.GlobalClient())
            {
                //Instanciamos Compañia
                using (SAT_CL.Global.CompaniaEmisorReceptor objCompania = new CompaniaEmisorReceptor(((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                {

                    string resultado_web_service = global.InsertaRelacionUbicacionPublicacion(Convert.ToInt32(lblIdElemento.Text), Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtElemento.Text, "ID:", 1)), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Direccion del Servidor", 0), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Nombre de BD", 0),
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
                            resultado = new RetornoOperacion("Tu respuesta ha sido Registrada", true);
                        }

                    }
                    else
                    {
                        //Establecmos Mensaje Resultado
                        resultado = new RetornoOperacion("No es posible obtener la respuesta WS");
                    }
                }
                global.Close();
                return resultado;
            }
        }

        /// <summary>
        ///Insertamos Relación de Producto
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion insertaRelacionProducto()
        {
            //Establecemos variable resultado
            RetornoOperacion resultado = null;

            //Consumimos Web Service
            using (GlobalCentral.GlobalClient global = new GlobalCentral.GlobalClient())
            {
                //Instanciamos Compañia
                using (SAT_CL.Global.CompaniaEmisorReceptor objCompania = new CompaniaEmisorReceptor(((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                {

                    string resultado_web_service = global.InsertaRelacionProductoPublicacion(Convert.ToInt32(lblIdElemento.Text), Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtElemento.Text, "ID:", 1)), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Direccion del Servidor", 0), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Nombre de BD", 0),
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
                            resultado = new RetornoOperacion("El Producto ha sido Registrado", true);
                        }

                    }
                    else
                    {
                        //Establecmos Mensaje Resultado
                        resultado = new RetornoOperacion("No es posible obtener la respuesta WS");
                    }
                }
                global.Close();
                return resultado;
            }
        }

        /// <summary>
        ///Insertamos Relación de Cliente
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion insertaRelacionCliente()
        {
            //Establecemos variable resultado
            RetornoOperacion resultado = null;

            //Consumimos Web Service
            using (GlobalCentral.GlobalClient global = new GlobalCentral.GlobalClient())
            {
                //Instanciamos Compañia
                using (SAT_CL.Global.CompaniaEmisorReceptor objCompania = new CompaniaEmisorReceptor(((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                {

                    string resultado_web_service = global.InsertaRelacionClientePublicacion(Convert.ToInt32(lblIdElemento.Text), Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtElemento.Text, "ID:", 1)), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Direccion del Servidor", 0), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Nombre de BD", 0),
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
                            resultado = new RetornoOperacion("Tu respuesta ha sido Registrada", true);
                        }

                    }
                    else
                    {
                        //Establecmos Mensaje Resultado
                        resultado = new RetornoOperacion("No es posible obtener la respuesta WS");
                    }
                }
                global.Close();
                return resultado;
            }
        }
        #endregion

        #region Eventos Publicación de Servicios

        /// <summary>
        /// Cambio de página activa del GV de Viajes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvViajes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Controles.CambiaIndicePaginaGridView(gvViajes, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table9"), e.NewPageIndex, false, 4);
        }
        /// <summary>
        /// Cambio de criterio de ordenamiento en GV de Viajes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvViajes_Sorting(object sender, GridViewSortEventArgs e)
        {
            lblOrdenarViajes.Text = Controles.CambiaSortExpressionGridView(gvViajes, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table9"), e.SortExpression, false, 4);
        }
        /// <summary>
        /// Cambio de tamaño del GV de Viajes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoViajes_SelectedIndexChanged(object sender, EventArgs e)
        {
            Controles.CambiaTamañoPaginaGridView(gvViajes, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table9"), Convert.ToInt32(ddlTamanoViajes.SelectedValue), false, 4);
        }
        /// <summary>
        /// Click en exportación de contenido de GV de Unidades
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarViajes_Click(object sender, EventArgs e)
        {
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table9"), "*".ToArray());
        }

        /// <summary>
        ///Evento geenrado a cambiar el Tamaño de Paradas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoParadasPS_SelectedIndexChanged(object sender, EventArgs e)
        {
            Controles.CambiaTamañoPaginaGridView(gvParadasPS, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table12"), Convert.ToInt32(ddlTamanoParadasPS.SelectedValue), false, 4);

        }

        /// <summary>
        /// Dvento generado al cambiar la Pagina de Paradas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvParadasPS_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Controles.CambiaIndicePaginaGridView(gvParadasPS, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table12"), e.NewPageIndex, false, 4);

        }

        /// <summary>
        /// Evento generado al cambiar el Sort de Paradas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvParadasPS_Sorting(object sender, GridViewSortEventArgs e)
        {
            lblOrdenadoParadasPS.Text = Controles.CambiaSortExpressionGridView(gvParadasPS, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table12"), e.SortExpression, false, 4);

        }

        /// <summary>
        /// Evento generado al cambiar de Página de Resultados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvResultadoRespuestaPS_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Controles.CambiaIndicePaginaGridView(gvResultadoRespuestaPS, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table10"), e.NewPageIndex, false, 4);

        }

        /// <summary>
        /// Evento genetrado al cambiar el Sort de Resultados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvResultadoRespuestaPS_Sorting(object sender, GridViewSortEventArgs e)
        {
            lblOrdenadoResultadoRespuestaPS.Text = Controles.CambiaSortExpressionGridView(gvResultadoRespuestaPS, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table10"), e.SortExpression, false, 4);

        }

        /// <summary>
        /// Evento generado al cambiar el tamaño de Resultados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddTamanolResultadoRespuestaPS_SelectedIndexChanged(object sender, EventArgs e)
        {
            Controles.CambiaTamañoPaginaGridView(gvResultadoRespuestaPS, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table10"), Convert.ToInt32(ddlTamanoRespuestasPS.SelectedValue), false, 4);

        }

        /// <summary>
        /// Evento generado al cambiar el Tamaño de Respuestas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvRespuestasPS_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Controles.CambiaIndicePaginaGridView(gvRespuestasPS, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table7"), e.NewPageIndex, false, 4);

        }

        /// <summary>
        /// Evento generado al cambiar el Sort
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvRespuestasPS_Sorting(object sender, GridViewSortEventArgs e)
        {
            lblOrdenadoRespuestasPS.Text = Controles.CambiaSortExpressionGridView(gvRespuestasPS, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table7"), e.SortExpression, false, 4);

        }

        /// <summary>
        /// Eventp generado al cambiar el tamaño de Respuestas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoRespuestasPS_SelectedIndexChanged(object sender, EventArgs e)
        {
            Controles.CambiaTamañoPaginaGridView(gvRespuestasPS, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table7"), Convert.ToInt32(ddlTamanoRespuestasPS.SelectedValue), false, 4);

        }



        /// <summary>
        /// Evento generado al Crear la Respuesta de una Publicación  de Servicio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCrearRespuestaPS_Click(object sender, EventArgs e)
        {
            //Declaramos objeto retorno
            RetornoOperacion resultado = new RetornoOperacion();
            //Respondemos Publicación de Servicio
            resultado = respondePublicacionServicio();
            //Validamos Resultado
            if (resultado.OperacionExitosa)
            {
                //Cerramos Ventana Modal
                alternaVentanaModal("crearRespuestaPS", btnCrearRespuestaPS);
                //cargamos respuesta
                cargaPublicacionesServicios();
            }
        }

        /// <summary>
        /// Evento generado al  Ver los Detalles de la Respuesta de Publicación de Servicios
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbVerDetalleResultadoPS_Click(object sender, EventArgs e)
        {
            //Si hay registros
            if (gvResultadoRespuestaPS.DataKeys.Count > 0)
            {
                //Seleccionando la fila correspondiente
                Controles.SeleccionaFila(gvResultadoRespuestaPS, sender, "lnk", false);
                //Mostrando ventana modal 
                alternaVentanaModal("opcionSeleccionRespuestaPS", gvResultadoRespuestaPS);
                //ocultamos ventana modal correspondiente
                alternaVentanaModal("resultadoRespuestaPS", gvResultadoRespuestaPS);
                //Cargamos Respuestas de una Publicación de Servicio
                cargaRespuestasPS(gvResultadoRespuestaPS);
            }
        }

        /// <summary>
        /// Click en algún Link de GV de Unidad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbAccionServicioPS_Click(object sender, EventArgs e)
        {
            //Si hay registros
            if (gvViajes.DataKeys.Count > 0)
            {
                //Seleccionando la fila correspondiente
                Controles.SeleccionaFila(gvViajes, sender, "lnk", false);
                //Convirtiendo objeto a linkbutton
                LinkButton lkb = (LinkButton)sender;

                //Determinando el comando por ejecutar
                switch (lkb.CommandName)
                {

                    case "Paradas":
                        //Mostrando ventana modal correspondiente
                        alternaVentanaModal("informacionParadasPS", lkb);
                        //cargamos respuesta
                        cargaParadasPS();
                        break;
                    case "Oferta Realizada":
                    case "Oferta Aceptada":
                        //Abriendo ventana
                        alternaVentanaModal("crearRespuestaPS", lkb);
                        //Inicializamos Controles
                        inicializaControlesCrearRespuestaPS(Convert.ToInt32(gvViajes.SelectedDataKey["IdRespuesta"]));
                        btnCrearRespuestaPS.Visible =
                        btnConfirmarPS.Visible = false;
                        break;
                    case "Ofertar":
                        //Abriendo ventana
                        alternaVentanaModal("crearRespuestaPS", lkb);
                        //Inicializamos Controles
                        inicializaControlesCrearRespuestaPS(0);
                        btnCrearRespuestaPS.Visible = true;
                        btnConfirmarPS.Visible = false;
                        break;
                    case "Por Confirmar":
                        //Abriendo ventana
                        alternaVentanaModal("crearRespuestaPS", lkb);
                        //Inicializamos Controles
                        inicializaControlesCrearRespuestaPS(Convert.ToInt32(gvViajes.SelectedDataKey["IdRespuesta"]));
                        btnCrearRespuestaPS.Visible = false;
                        btnConfirmarPS.Visible = true;
                        break;
                }
            }
        }

        /// <summary>
        /// Click en algún Link de GV de Respúestas de la Públicación de Servicio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbAccionRespuestaPS_Click(object sender, EventArgs e)
        {
            //Si hay registros
            if (gvRespuestasPS.DataKeys.Count > 0)
            {
                //Seleccionando la fila correspondiente
                Controles.SeleccionaFila(gvRespuestasPS, sender, "lnk", false);
                //Convirtiendo objeto a linkbutton
                LinkButton lkb = (LinkButton)sender;

                //Determinando el comando por ejecutar
                switch (lkb.CommandName)
                {
                    case "Aceptar":
                        //limpiamos  Control de Tarifa
                        txtTarifaAceptadaPU.Text = "";//;
                        // Abriendo ventana 
                        alternaVentanaModal("aceptarRespuesta", lkb);
                        //Cerramos Modal
                        alternaVentanaModal("opcionSeleccionRespuestaPS", lkb);
                        break;
                    case "Confirmar":
                        //Cerramos Modal
                        alternaVentanaModal("opcionSeleccionRespuestaPS", lkb);
                        //Cerramos Ventana modal
                        alternaVentanaModal("confirmarRespuestaPU", lkb);
                        break;
                }
            }
        }

        /// <summary>
        /// Evento Generado al Buscar la Publicaciones de los Servicios
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscarPS_Click(object sender, EventArgs e)
        {
            //Carga Publicaciones de Servicio
            cargaPublicacionesServicios();
        }
        #endregion

        #region Métodos Publicación de Servicios

        /// <summary>
        /// Método encargado de cargar las Publicaciones de los Servicios
        /// </summary>
        private void cargaPublicacionesServicios()
        {
            //Obtenemos Depósito
            using (DataTable mit = consumoCargaPublicacionesServicios())
            {

                //Validando que el DataSet contenga las tablas
                if (Validacion.ValidaOrigenDatos(mit))
                {
                    //Cargando los GridView
                    Controles.CargaGridView(gvViajes, mit, "IdServicio-IdRespuesta", lblOrdenarViajes.Text, false, 3);
                    //Añadiendo Tablas al DataSet de Session
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], mit, "Table9");

                }
                else
                {   //Inicializando GridViews
                    Controles.InicializaGridview(gvViajes);

                    //Eliminando Tablas del DataSet de Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table9");
                }
            }
        }

        /// <summary>
        /// Carga las Publicaciones de los Servicio
        /// </summary>
        /// <returns></returns>
        private DataTable consumoCargaPublicacionesServicios()
        {
            //Establecemos variable resultado
            DataSet ds = new DataSet();
            DataTable mit = null;
            RetornoOperacion resultado = new RetornoOperacion();

            //Consumimos Web Service
            using (DespachoCentral.DespachoClient despacho = new DespachoCentral.DespachoClient())
            {
                //Instanciamos Usuario
                using (SAT_CL.Seguridad.Usuario objUsuario = new SAT_CL.Seguridad.Usuario(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario))
                {
                    //Instanciamos Compañia
                    using (SAT_CL.Global.CompaniaEmisorReceptor objCompania = new CompaniaEmisorReceptor(((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                    {

                        string resultado_web_service = despacho.VisualizaServiciosPublicados(objCompania.id_compania_emisor_receptor, txtCompaniaPS.Text, CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Direccion del Servidor", 0), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Nombre de BD", 0),
                                           TSDK.Base.Cadena.VerificaCadenaVacia(SAT_CL.Global.Referencia.CargaReferencia("0", 25, objCompania.id_compania_emisor_receptor, "Consumo Web Service", "Contraseña"), ""));

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
                            ds.ReadXml(xDoc.Document.Element("ServiciosPendientes").Element("NewDataSet").CreateReader());
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
                            TSDK.ASP.ScriptServer.MuestraNotificacion(btnBuscarUnidades, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
                        }
                    }
                }
                despacho.Close();
            }
            return mit;
        }

        /// <summary>
        /// Método encargado de cargar las Respuestas
        /// </summary>
        private void cargaParadasPS()
        {
            //Obtenemos Depósito
            using (DataTable mit = consumoParadasPS())
            {

                //Validando que el DataSet contenga las tablas
                if (Validacion.ValidaOrigenDatos(mit))
                {
                    //Cargando los GridView
                    Controles.CargaGridView(gvParadasPS, mit, "Id", lblOrdenadoParadasPS.Text, false, 3);
                    //Añadiendo Tablas al DataSet de Session
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], mit, "Table12");

                }
                else
                {   //Inicializando GridViews
                    Controles.InicializaGridview(gvParadasPS);
                    //Eliminando Tablas del DataSet de Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table12");
                }
            }
        }

        /// <summary>
        /// Carga las Paradas
        /// </summary>
        /// <returns></returns>
        private DataTable consumoParadasPS()
        {
            //Establecemos variable resultado
            DataSet ds = new DataSet();
            DataTable mit = null;
            RetornoOperacion resultado = new RetornoOperacion();

            //Consumimos Web Service
            DespachoCentral.DespachoClient despacho = new DespachoCentral.DespachoClient();

            //Instanciamos Compañia
            using (SAT_CL.Global.CompaniaEmisorReceptor objCompania = new CompaniaEmisorReceptor(((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
            {
                string resultado_web_service = despacho.ObtieneParadasServicio(Convert.ToInt32(gvViajes.SelectedValue), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Direccion del Servidor", 0), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Nombre de BD", 0),
                                   TSDK.Base.Cadena.VerificaCadenaVacia(SAT_CL.Global.Referencia.CargaReferencia("0", 25, objCompania.id_compania_emisor_receptor, "Consumo Web Service", "Contraseña"), ""));

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
                    TSDK.ASP.ScriptServer.MuestraNotificacion(gvUnidades, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
                }
            }
            return mit;
        }

        /// <summary>
        /// Método encargado de cargar las Respuestas
        /// </summary>
        /// <param name="control"></param>
        private void cargaResultadoRespuestaPS(System.Web.UI.Control control)
        {
            //Obtenemos Depósito
            using (DataTable mit = consumoResultadoRespuestasPS(control))
            {

                //Validando que el DataSet contenga las tablas
                if (Validacion.ValidaOrigenDatos(mit))
                {
                    //Cargando los GridView
                    Controles.CargaGridView(gvResultadoRespuestaPS, mit, "Id", lblOrdenadoResultadoRespuestaPS.Text, false, 3);
                    //Añadiendo Tablas al DataSet de Session
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], mit, "Table10");

                }
                else
                {   //Inicializando GridViews
                    Controles.InicializaGridview(gvResultadoRespuestaPS);
                    //Eliminando Tablas del DataSet de Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table10");
                }
            }
        }

        /// <summary>
        /// Aceptar la Respuesta de la Publicación del Servicio
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion aceptarRespuestaPS()
        {
            //Establecemos variable resultado
            RetornoOperacion resultado = null;

            //Consumimos Web Service
            DespachoCentral.DespachoClient despacho = new DespachoCentral.DespachoClient();


            //Instanciamos Compañia
            using (SAT_CL.Global.CompaniaEmisorReceptor objCompania = new CompaniaEmisorReceptor(((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
            {

                string resultado_web_service = despacho.AceptaRespuestaServicio(Convert.ToInt32(gvRespuestasPS.SelectedValue), Convert.ToDecimal(txtTarifaAceptadaPU.Text), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Direccion del Servidor", 0), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Nombre de BD", 0),
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
                        resultado = new RetornoOperacion("La Respuesta ha sido Aceptada", true);
                    }

                }
                else
                {
                    //Establecmos Mensaje Resultado
                    resultado = new RetornoOperacion("No es posible obtener la respuesta de WS");
                }
                //Mostrando Mensaje de Operación
                TSDK.ASP.ScriptServer.MuestraNotificacion(btnAceptarRespuesta, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
            return resultado;
        }
        /// <summary>
        /// Obtenemos la Información del Servicio  de una Publicación de Servicio
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion documentaServicioUnidadPS()
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
                    using (SAT_CL.Documentacion.Servicio objServicio = new SAT_CL.Documentacion.Servicio(Convert.ToInt32(gvInfoViaje.SelectedValue)))
                    {
                        //Instanciamos Compañia Receptor
                        using (SAT_CL.Global.CompaniaEmisorReceptor objCompaniaReceptor = new CompaniaEmisorReceptor(objServicio.id_cliente_receptor))
                        {
                            //Obtenemos Resultado
                            string resultado_web_service = despacho.DocumentaServicioUnidad(Convert.ToInt32(gvRespuestas.SelectedValue), objServicio.id_servicio, objServicio.id_cliente_receptor, objCompaniaReceptor.nombre,
                                                                                          objServicio.no_servicio, SAT_CL.Global.Referencia.CargaReferencia(objCompania.id_compania_emisor_receptor.ToString(), 1, objServicio.id_servicio, "Referencia de Viaje", "Confirmación"),
                                                                                          SAT_CL.Tarifas.TipoCargo.ObtieneTipoCargoDescripcion(objCompania.id_compania_emisor_receptor, "Flete"), SAT_CL.Despacho.ConsumoSOAPCentral.ObtieneParadasPublicacionUnidad(SAT_CL.Despacho.Parada.CargaParadasParaPublicacionDeUnidad(objServicio.id_servicio)),
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

                            }
                            else
                            {
                                //Establecmos Mensaje Resultado
                                resultado = new RetornoOperacion("No es posible obtener la respuesta WS");
                            }
                        }
                    }
                }
                despacho.Close();
                return resultado;
            }
        }

        /// <summary>
        /// Carga los Resultados de la Respuestas PS
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        private DataTable consumoResultadoRespuestasPS(System.Web.UI.Control control)
        {
            //Establecemos variable resultado
            DataSet ds = new DataSet();
            RetornoOperacion resultado = new RetornoOperacion();

            //Consumimos Web Service
            DespachoCentral.DespachoClient despacho = new DespachoCentral.DespachoClient();


            string resultado_web_service = despacho.ObtieneRespuestasServicio(Convert.ToInt32(gvViajes.SelectedValue), Convert.ToInt32(gvViajes.SelectedDataKey["Indicador"]) == 1 ? true : false, ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Direccion del Servidor", 0), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Nombre de BD", 0),
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
                ds.ReadXml(xDoc.Document.Element("RespuestasServicio").Element("NewDataSet").CreateReader());
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
                TSDK.ASP.ScriptServer.MuestraNotificacion(control, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
            return ds.Tables["Table"];
        }

        /// <summary>
        /// Método encargado de cargar las Respuestas de Una Publicación de Servcicio
        /// </summary>
        private void cargaRespuestasPS(System.Web.UI.Control control)
        {
            //Obtenemos Depósito
            using (DataSet ds = consumoRespuestasPS(control))
            {

                //Validando que el DataSet contenga las tablas
                if (Validacion.ValidaOrigenDatos(ds.Tables["Table"]))
                {
                    //Cargando los GridView
                    Controles.CargaGridView(gvRespuestasPS, ds.Tables["Table"], "Id-TarifaOfertada", lblOrdenadoRespuestasPS.Text, false, 3);
                    //Añadiendo Tablas al DataSet de Session
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], ds.Tables["Table"], "Table7");

                }
                else
                {   //Inicializando GridViews
                    Controles.InicializaGridview(gvRespuestasPS);
                    //Eliminando Tablas del DataSet de Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table7");
                }
            }
        }

        /// <summary>
        /// Carga las Respuestas de la Publicación de Servicio
        /// </summary>
        /// <returns></returns>
        private DataSet consumoRespuestasPS(System.Web.UI.Control control)
        {
            //Establecemos variable resultado
            DataSet ds = new DataSet();
            RetornoOperacion resultado = new RetornoOperacion();

            //Consumimos Web Service
            DespachoCentral.DespachoClient despacho = new DespachoCentral.DespachoClient();


            string resultado_web_service = despacho.VisualizarRespuesta(Convert.ToInt32(gvResultadoRespuestaPS.SelectedValue), ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Direccion del Servidor", 0), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Nombre de BD", 0),
                               TSDK.Base.Cadena.VerificaCadenaVacia(SAT_CL.Global.Referencia.CargaReferencia("0", 25, ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "Consumo Web Service", "Contraseña"), ""), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario,
                               ((SAT_CL.Seguridad.Usuario)Session["usuario"]).nombre);

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
                ds.ReadXml(xDoc.Document.Element("VisualizacionRespuesta").Element("NewDataSet").CreateReader());
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
                TSDK.ASP.ScriptServer.MuestraNotificacion(control, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
            return ds;
        }

        /// <summary>
        /// Inicializa de manera general los componentes  de la creación de la Respuesta de Un Servicio
        /// </summary>
        private void inicializaControlesCrearRespuestaPS(int id_respuesta)
        {
            //Inicializamos Valores
            inicializaValoresCrearRespuestaPS(id_respuesta);
            //Habilitamos Controles 
            habilitaControlesCrearRespuestaPS(id_respuesta);
        }

        /// <summary>
        /// Inicializa de controles de la creación de la respuesta del Servicio
        /// </summary>
        private void inicializaValoresCrearRespuestaPS(int id_respuesta)
        {
            if (id_respuesta == 0)
            {
                //Cargamos Publicaciones de Viajes Propias
                txtEstatusPS.Text = "Publicada";
                txtTarifaPS.Text = "";
                lblTarifaOfertadaPS.Text = "$0.00";
                txtContactoPS.Text = "";
                txtTelefonoPS.Text = "";
                txtObservacionPS.Text = "";
            }
            else
            {
                //Obtenemos Datos de la Respuesta
                DataTable mitRespuesta = consumoInicializaRespuesta(id_respuesta);

                //Validando que el DataSet contenga las tablas
                if (Validacion.ValidaOrigenDatos(mitRespuesta))
                {
                    //Obtenemos La Referencias del Concepto Origen
                    DataRow re = (from DataRow r in mitRespuesta.Rows
                                  select r).FirstOrDefault();
                    //Validamos que exista elementos
                    if (re != null)
                    {
                        //Cargamos Publicaciones de Viajes Propias
                        txtEstatusPS.Text = re.Field<string>("Estatus");
                        txtTarifaPS.Text = string.Format("{0:C2}", Convert.ToInt32(re["TarifaOfertada"]));
                        lblTarifaOfertadaPS.Text = string.Format("{0:C2}", Convert.ToInt32(re["TarifaAceptada"]));
                        txtTelefonoPS.Text = re.Field<string>("TelefonoContacto");
                        txtObservacionPS.Text = re.Field<string>("Observacion");
                        txtContactoPS.Text = re.Field<string>("Contacto");

                    }
                }
            }
        }

        /// <summary>
        /// Inicializa de manera general los componentes  de la creación de la Respuesta de una Unidad
        /// </summary>
        private void habilitaControlesCrearRespuestaPS(int id_respuesta)
        {
            //Validamos Id Respuesta
            if (id_respuesta == 0)
            {
                //habilitamos Controles
                txtEstatusPS.Enabled = false;
                txtTarifaPS.Enabled =
                txtContactoPS.Enabled =
                txtTelefonoPS.Enabled =
                txtObservacionPS.Enabled = true;
            }
            else
            {
                //deshabilitamos Controles
                txtEstatusPS.Enabled = false;
                txtTarifaPS.Enabled =
                txtContactoPS.Enabled =
                txtTelefonoPS.Enabled =
                txtObservacionPS.Enabled = false;
            }
        }
        /// <summary>
        ///  Inicializamos Autocomplete de Cliente
        /// </summary>
        /// <param name="control"></param>
        private void inicializaAutocompleteCliente(System.Web.UI.Control control)
        {
            //Generamos Sript
            string script =
            @"<script type='text/javascript'>
              $('#" + this.txtElemento.ClientID + @"').autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=15&param=" + ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString() + @"', appendTo: '" + this.Contenedor + @"'});
              </script>";

            //Registrando el script sólo para los paneles que producirán actualización del mismo
            System.Web.UI.ScriptManager.RegisterStartupScript(control, control.GetType(), "AutocompleteCliente", script, false);
        }

        /// <summary>
        ///  Inicializamos Autocomplete de Ublicacion
        /// </summary>
        /// <param name="control"></param>
        private void inicializaAutocompleteUbicacion(System.Web.UI.Control control)
        {
            //Generamos Sript
            string script =
            @"<script type='text/javascript'>
              $('#" + this.txtElemento.ClientID + @"').autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=2&param=" + ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString() + @"', appendTo: '" + this.Contenedor + @"'});
              </script>";

            //Registrando el script sólo para los paneles que producirán actualización del mismo
            System.Web.UI.ScriptManager.RegisterStartupScript(control, control.GetType(), "AutocompleteUbicacion", script, false);
        }

        /// <summary>
        ///  Inicializamos Autocomplete de Concepto
        /// </summary>
        /// <param name="control"></param>
        private void inicializaAutocompleteConcepto(System.Web.UI.Control control)
        {
            //Generamos Sript
            string script =
            @"<script type='text/javascript'>
              $('#" + this.txtElemento.ClientID + @"').autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=52&param=" + ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString() + @"', appendTo: '" + this.Contenedor + @"'});
              </script>";

            //Registrando el script sólo para los paneles que producirán actualización del mismo
            System.Web.UI.ScriptManager.RegisterStartupScript(control, control.GetType(), "AutocompleteConcepto", script, false);
        }

        /// <summary>
        /// Inicializamos Autocomplete de Producto
        /// </summary>
        /// <param name="control"></param>
        private void inicializaAutocompleteProducto(System.Web.UI.Control control)
        {
            //Generamos Sript
            string script =
            @"<script type='text/javascript'>
              $('#" + this.txtElemento.ClientID + @"').autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=1&param=" + ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString() + @"', appendTo: '" + this.Contenedor + @"'});
              </script>";

            //Registrando el script sólo para los paneles que producirán actualización del mismo
            System.Web.UI.ScriptManager.RegisterStartupScript(control, control.GetType(), "AutocompleteProducto", script, false);
        }

        /// <summary>
        /// Copia un Servcio de una Publicacion de Servicio
        /// </summary>
        /// <returns></returns>
        private DataSet obtieneServicioPS(System.Web.UI.Control control, out RetornoOperacion resultado)
        {
            //Establecemos variable resultado
            DataSet ds = new DataSet();
            resultado = new RetornoOperacion();

            //Consumimos Web Service
            using (DespachoCentral.DespachoClient despacho = new DespachoCentral.DespachoClient())
            {
                //Instanciamos Usuario
                using (SAT_CL.Seguridad.Usuario objUsuario = new SAT_CL.Seguridad.Usuario(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario))
                {
                    //Instanciamos Compañia
                    using (SAT_CL.Global.CompaniaEmisorReceptor objCompania = new CompaniaEmisorReceptor(((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                    {

                        string resultado_web_service = despacho.ObtieneDatosServicioCentral(Convert.ToInt32(gvViajes.SelectedValue), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Direccion del Servidor", 0), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Nombre de BD", 0),
                                           TSDK.Base.Cadena.VerificaCadenaVacia(SAT_CL.Global.Referencia.CargaReferencia("0", 25, objCompania.id_compania_emisor_receptor, "Consumo Web Service", "Contraseña"), ""));

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
                            ds.ReadXml(xDoc.Document.Element("Confirmacion").CreateReader());
                        }
                    }
                }
                despacho.Close();
            }
            return ds;
        }

        /// <summary>
        /// Reesponde la Publicación de un Servcio
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion respondePublicacionServicio()
        {
            //Establecemos variable resultado
            RetornoOperacion resultado = null;

            //Consumimos Web Service
            using (DespachoCentral.DespachoClient despacho = new DespachoCentral.DespachoClient())
            {
                //Instanciamos Compañia
                using (SAT_CL.Global.CompaniaEmisorReceptor objCompania = new CompaniaEmisorReceptor(((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                {

                    string resultado_web_service = despacho.RespuestaServicio(((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, objCompania.nombre, Convert.ToDecimal(txtTarifaPS.Text), txtObservacionPS.Text, txtContactoPS.Text,
                                       txtTelefonoPS.Text, Convert.ToInt32(gvViajes.SelectedDataKey["IdServicio"]),
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
                            resultado = new RetornoOperacion("Tu respuesta ha sido Registrada", true);
                        }

                    }
                    else
                    {
                        //Establecmos Mensaje Resultado
                        resultado = new RetornoOperacion("No es posible obtener la respuesta WS");
                    }
                    //Mostrando Mensaje de Operación
                    TSDK.ASP.ScriptServer.MuestraNotificacion(btnCrearRespuestaPS, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
                }
                despacho.Close();
                return resultado;
            }
        }
        /// <summary>
        /// Actualiza el Id de Servicio Destino
        /// </summary>
        /// <param name="id_servicio"></param>
        /// <returns></returns>
        private RetornoOperacion actualizaIdServicioDestinoPS(int id_servicio)
        {
            //Establecemos variable resultado
            RetornoOperacion resultado = null;

            //Consumimos Web Service
            using (DespachoCentral.DespachoClient despacho = new DespachoCentral.DespachoClient())
            {
                //Instanciamos Compañia
                using (SAT_CL.Global.CompaniaEmisorReceptor objCompania = new CompaniaEmisorReceptor(((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                {

                    string resultado_web_service = despacho.ActualizaServicio(Convert.ToInt32(gvViajes.SelectedValue), id_servicio, CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Direccion del Servidor", 0), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Nombre de BD", 0),
                                       TSDK.Base.Cadena.VerificaCadenaVacia(SAT_CL.Global.Referencia.CargaReferencia("0", 25, objCompania.id_compania_emisor_receptor, "Consumo Web Service", "Contraseña"), ""),
                                       ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).nombre);


                    //Obtenemos Documento generado
                    XDocument xDoc = XDocument.Parse(resultado_web_service);

                    //Validamos que exista Respuesta
                    if (xDoc != null)
                    {
                        //Traduciendo resultado
                        resultado = new RetornoOperacion(Convert.ToInt32(xDoc.Descendants("idRegistro").FirstOrDefault().Value), xDoc.Descendants("mensaje").FirstOrDefault().Value.ToString(), Convert.ToBoolean(xDoc.Descendants("operacionExitosa").FirstOrDefault().Value));


                    }
                    else
                    {
                        //Establecmos Mensaje Resultado
                        resultado = new RetornoOperacion("No es posible obtener la respuesta de WS");
                    }
                }
                //Cerramos Web Service
                despacho.Close();
            }
            return resultado;
        }

        /// <summary>
        ///Insertamos Relación de Concepto de la Publicación de Servicio
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion insertaRelacionConceptoPS()
        {
            //Establecemos variable resultado
            RetornoOperacion resultado = null;

            //Consumimos Web Service
            using (GlobalCentral.GlobalClient global = new GlobalCentral.GlobalClient())
            {
                //Instanciamos Compañia
                using (SAT_CL.Global.CompaniaEmisorReceptor objCompania = new CompaniaEmisorReceptor(((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                {

                    string resultado_web_service = global.InsertaRelacionConceptoFleteServicio(Convert.ToInt32(lblIdElemento.Text), Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtElemento.Text, "ID:", 1)), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Direccion del Servidor", 0), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Nombre de BD", 0),
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
                            resultado = new RetornoOperacion("El Concepto ha sido Registrado", true);
                        }

                    }
                    else
                    {
                        //Establecmos Mensaje Resultado
                        resultado = new RetornoOperacion("No es posible obtener la respuesta WS");
                    }
                }
                global.Close();
                return resultado;
            }
        }
        /// <summary>
        ///Insertamos Relación de Producto publicación Servicio
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion insertaRelacionProductoPS()
        {
            //Establecemos variable resultado
            RetornoOperacion resultado = null;

            //Consumimos Web Service
            using (GlobalCentral.GlobalClient global = new GlobalCentral.GlobalClient())
            {
                //Instanciamos Compañia
                using (SAT_CL.Global.CompaniaEmisorReceptor objCompania = new CompaniaEmisorReceptor(((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                {

                    string resultado_web_service = global.InsertaRelacionProductoServicio(Convert.ToInt32(lblIdElemento.Text), Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtElemento.Text, "ID:", 1)), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Direccion del Servidor", 0), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Nombre de BD", 0),
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
                            resultado = new RetornoOperacion("El Producto ha sido Registrado", true);
                        }

                    }
                    else
                    {
                        //Establecmos Mensaje Resultado
                        resultado = new RetornoOperacion("No es posible obtener la respuesta WS");
                    }
                }
                global.Close();
                return resultado;
            }
        }
        /// <summary>
        ///Insertamos Relación de Cliente Publicación de Servicio
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion insertaRelacionClientePS()
        {
            //Establecemos variable resultado
            RetornoOperacion resultado = null;

            //Consumimos Web Service
            using (GlobalCentral.GlobalClient global = new GlobalCentral.GlobalClient())
            {
                //Instanciamos Compañia
                using (SAT_CL.Global.CompaniaEmisorReceptor objCompania = new CompaniaEmisorReceptor(((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                {

                    string resultado_web_service = global.InsertaRelacionClienteServicio(Convert.ToInt32(lblIdElemento.Text), Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtElemento.Text, "ID:", 1)), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Direccion del Servidor", 0), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Nombre de BD", 0),
                                       TSDK.Base.Cadena.VerificaCadenaVacia(SAT_CL.Global.Referencia.CargaReferencia("0", 25, objCompania.id_compania_emisor_receptor, "Consumo Web Service", "Contraseña"), ""),
                                       ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).nombre);


                    //Obtenemos Documento generado
                    XDocument xDoc = XDocument.Parse(resultado_web_service);

                    //Validamos que exista Respuesta
                    if (xDoc != null)
                    {
                        //Traduciendo resultado
                        resultado = new RetornoOperacion(Convert.ToInt32(xDoc.Descendants("idRegistro").FirstOrDefault().Value), xDoc.Descendants("mensaje").FirstOrDefault().Value.ToString(), Convert.ToBoolean(xDoc.Descendants("operacionExitosa").FirstOrDefault().Value));

                    }
                    else
                    {
                        //Establecmos Mensaje Resultado
                        resultado = new RetornoOperacion("No es posible obtener la respuesta WS");
                    }
                }
                global.Close();
                return resultado;
            }
        }
        /// <summary>
        ///Insertamos Relación de Ubicacion de la Publicación de Servicio
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion insertaRelacionUbicacionPS()
        {
            //Establecemos variable resultado
            RetornoOperacion resultado = null;

            //Consumimos Web Service
            using (GlobalCentral.GlobalClient global = new GlobalCentral.GlobalClient())
            {
                //Instanciamos Compañia
                using (SAT_CL.Global.CompaniaEmisorReceptor objCompania = new CompaniaEmisorReceptor(((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                {

                    string resultado_web_service = global.InsertaRelacionUbicacionServicio(Convert.ToInt32(lblIdElemento.Text), Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtElemento.Text, "ID:", 1)), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Direccion del Servidor", 0), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Nombre de BD", 0),
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
                            resultado = new RetornoOperacion("La Ubicación ha sido registrada", true);
                        }

                    }
                    else
                    {
                        //Establecmos Mensaje Resultado
                        resultado = new RetornoOperacion("No es posible obtener la respuesta WS");
                    }
                }
                global.Close();
                return resultado;
            }
        }
        #endregion
        #region Eventos Generales
        /// <summary>
        /// Cerrar alguna ventana modal
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

                case "CerrarCrearRespuesta":
                    //Cerramos Ventana Modal
                    alternaVentanaModal("crearRespuesta", lkbCerrar);
                    break;
                case "SeleccionRespuestas":
                    //ocultamos ventana modal correspondiente
                    alternaVentanaModal("opcionSeleccionRespuesta", lkbCerrar);
                    break;
                case "InformacionCiudades":
                    //  Abriendo ventana para la Publicación de la Unidad
                    alternaVentanaModal("informacionCiudades", lkbCerrar);
                    break;
                case "AceptarRespuesta":
                    // Abriendo ventana 
                    alternaVentanaModal("aceptarRespuesta", lkbCerrar);
                    break;
                case "Servicio":
                    // Cerramos ventana para de Inforamción de Viajes
                    alternaVentanaModal("informacionViajes", lkbCerrar);

                    break;
                case "CerrarResultadoRespuesta":
                    //Mostrando ventana modal correspondiente
                    alternaVentanaModal("resultadoRespuesta", lkbCerrar);
                    //Carga Publicaciones de Unidades
                    cargaPublicaciones();
                    break;
                case "ConfirmarRespuestaPU":
                    //Abre Ventana modal
                    alternaVentanaModal("confirmarRespuestaPU", lkbCerrar);
                    break;
                case "CrearRespuestaPS":
                    //Abre Ventana modal
                    alternaVentanaModal("crearRespuestaPS", lkbCerrar);
                    break;
                case "SeleccionRespuestasPS":
                    //ocultamos ventana modal correspondiente
                    alternaVentanaModal("opcionSeleccionRespuestaPS", lkbCerrar);
                    //Mostrando ventana modal correspondiente
                    alternaVentanaModal("resultadoRespuestaPS", lkbCerrar);
                    //Cargamos Resultados
                    cargaResultadoRespuestaPS(lkbCerrar);
                    break;
                case "CerrarResultadoRespuestaPS":
                    //Mostrando ventana modal correspondiente
                    alternaVentanaModal("resultadoRespuestaPS", lkbCerrar);
                    //Cargamos Publicaciones de Servicio
                    cargaPublicacionesServicios();
                    break;
                case "CerrarElemento":
                    //Cerramos Ventana Modal
                    alternaVentanaModal("elemento1", lkbCerrar);
                    break;
                case "InformacionParadasPS":
                    //Cerramos Ventana Modal
                    alternaVentanaModal("informacionParadasPS", lkbCerrar);
                    break;

            }
        }

        /// <summary>
        /// Click en pestaña del menú principal de la forma
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPestana_Click(object sender, EventArgs e)
        {
            //Determinando la pestaña pulsada
            switch (((Button)sender).CommandName)
            {
                case "Unidades":
                    //Cambiando estilos de pestañas
                    btnPestanaUnidades.CssClass = "boton_pestana_activo";
                    btnPestanaViajes.CssClass = "boton_pestana";
                    //Asignando vista activa de la forma
                    mtvPublicacion.SetActiveView(vwUnidades);
                    //Carga Publicaciones
                    cargaPublicaciones();
                    break;
                case "Viajes":
                    //Cambiando estilos de pestañas
                    btnPestanaUnidades.CssClass = "boton_pestana";
                    btnPestanaViajes.CssClass = "boton_pestana_activo";
                    //Asignando vista activa de la forma
                    mtvPublicacion.SetActiveView(vwViajes);
                    cargaPublicacionesServicios();
                    break;
            }
        }

        #endregion

        #region Métodos Generales
        /// <summary>
        /// Inicializa de manera general los componentes de la forma
        /// </summary>
        private void inicializaForma()
        {
            //Cargamos Publicaciones 
            creaEsquemaTablasTemporales();
            //Carga Catalogos
            cargaCatalogosPU();
            cargaCatalogosPS();
            //Inicializa Grid View
            Controles.InicializaGridview(gvUnidades);

        }

        /// <summary>
        /// Muestra u oculta la ventana modal solicitada
        /// </summary>
        /// <param name="nombre_ventana">Nombre de ventana por ocultar</param>
        /// <param name="control">Control que solicita la acción</param>
        private void alternaVentanaModal(string nombre_ventana, System.Web.UI.Control control)
        {
            //Determinando la ventana por alternar
            switch (nombre_ventana)
            {
                case "crearRespuesta":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "ViajesCrearRespuesta", "ViajesContenedorCrearRespuesta");
                    break;
                case "opcionSeleccionRespuesta":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "seleccionRespuestas", "seleccionrespuestas");
                    break;
                case "informacionCiudades":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenedorVentanaInformacionCiudades", "ventanaInformacionCiudades");
                    break;
                case "informacionViajes":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenedorVentanaInformacionViaje", "ventanaInformacionViaje");
                    break;
                case "aceptarRespuesta":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenidoAceptarRespuesta", "confirmacionAceptarRespuesta");
                    break;
                case "resultadoRespuesta":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenedorResultadoRespuesta", "ventanaResultadoRespuesta");
                    break;
                case "confirmarRespuestaPU":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenidoConfirmarRespuestaPU", "confirmacionConfirmarRespuestaPU");
                    break;
                case "crearRespuestaPS":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "crearRespuestaPS", "contenedorCrearRespuestaPS");
                    break;
                case "resultadoRespuestaPS":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenedorResultadoRespuestaPS", "ventanaResultadoRespuestaPS");
                    break;
                case "opcionSeleccionRespuestaPS":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "seleccionRespuestasPS", "contenedorSeleccionRespuestasPS");
                    break;
                case "elemento1":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenidoElemento", "confirmacionElemento");
                    break;
                case "informacionParadasPS":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenedorVentanaInformacionParadasPS", "ventanaInformacionParadasPS");
                    break;

            }
        }


        #endregion

        /// <summary>
        /// Mostramos Venta de Alta de Elementos Correspondiestes(Producto, Cliente, Concepto, Paradas)
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion muestraElemento(System.Web.UI.Control control, out DataSet ds)
        {
            //Limpiamos Valores
            inicializaValoresElemento();
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();
            ds = new DataSet();
            //Validamos Tipo de Información
            if (mtvPublicacion.ActiveViewIndex == 0)
            {
                //Validamos Obtención de Información del Viaje de la Publicacipón de Unidad
                ds = obtieneServicioPU(control, out resultado);
            }
            else
            {
                //Validamos Obtención de Información del Viaje de la Publicacipón de Servicio
                ds = obtieneServicioPS(control, out resultado);
            }
            //Validamos Resultado en Caso de Error
            if (!resultado.OperacionExitosa)
            {
                //Limpiamos Control
                lblDescipcion.Text = "";
                //Inicializamos Elemento 
                lblDescipcion.Text = Cadena.RegresaCadenaSeparada(resultado.Mensaje, "ID:", 0);
                //Inicializamos Elemento 
                lblIdElemento.Text = Cadena.RegresaCadenaSeparada(resultado.Mensaje, "ID:", 1);
                //De acuerdo al Error Obtenido
                switch (resultado.IdRegistro)
                {
                    //No existe el Cliente
                    case -6:
                        //Inicializamos Autocomplete
                        inicializaAutocompleteCliente(control);
                        lblElemento.Text = "Cliente";
                        //Mostramos Modal en Caso de Ser Necesario
                        if (lblValor.Text == "No")
                        {
                            //Abrimos Ventana Modal
                            alternaVentanaModal("elemento1", control);
                            lblValor.Text = "Si";
                        }
                        break;
                    //No existe la Ubicación
                    case -7:
                        //Inicializamos Autocomplete
                        inicializaAutocompleteUbicacion(control);
                        lblElemento.Text = "Ubicacion";
                        //Mostramos Modal en Caso de Ser Necesario
                        if (lblValor.Text == "No")
                        {
                            //Abrimos Ventana Modal
                            alternaVentanaModal("elemento1", control);
                            //Cambiamos Valor
                            lblValor.Text = "Si";
                        }
                        break;
                    case -8:
                        //Inicializamos Aucomplete
                        inicializaAutocompleteConcepto(control);
                        lblElemento.Text = "Concepto";
                        //Mostramos Modal en Caso de Ser Necesario
                        if (lblValor.Text == "No")
                        {
                            //Abrimos Ventana Modal
                            alternaVentanaModal("elemento1", control);
                            lblValor.Text = "Si";
                        }
                        break;
                    //No existe el Producto
                    case -9:
                        //Inicializamos Aucomplete
                        inicializaAutocompleteProducto(control);
                        lblElemento.Text = "Producto";
                        //Mostramos Modal en Caso de Ser Necesario
                        if (lblValor.Text == "No")
                        {
                            //Abrimos Ventana Modal
                            alternaVentanaModal("elemento1", control);
                            lblValor.Text = "Si";
                        }
                        break;
                }
            }
            //Deviolvemos Resultado
            return resultado;
        }

        /// <summary>
        /// Evento generado al Agregar un Elemento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAgregarElemento_Click(object sender, EventArgs e)
        {
            //Agregamos Elemento
            agregaElemento(btnAgregarElemento);
        }

        /// <summary>
        /// Método encargado  de Agregar Elemnto
        /// </summary>
        private void agregaElemento(System.Web.UI.Control control)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();
            //De acuerdo al Elemto Por Registrar
            switch (lblElemento.Text)
            {
                //No existe el Cliente
                case "Cliente":
                    //Validamos Tipo de Relación a Insertar
                    //Unidad
                    if (mtvPublicacion.ActiveViewIndex == 0)
                    {
                        //Insertamos Relación de
                        resultado = insertaRelacionCliente();
                    }
                    else
                    {
                        //Insertamos Relación de
                        resultado = insertaRelacionClientePS();
                    }
                    break;
                //No existe la Ubicación
                case "Ubicacion":
                    //Validamos Tipo de Relación a Insertar
                    //Unidad
                    if (mtvPublicacion.ActiveViewIndex == 0)
                    {
                        //Insertamos Relación
                        resultado = insertaRelacionUbicacion();
                    }
                    else
                    {
                        //Insertamos Relación
                        resultado = insertaRelacionUbicacionPS();
                    }
                    break;
                case "Concepto":
                    //Validamos Tipo de Relación a Insertar
                    //Unidad
                    if (mtvPublicacion.ActiveViewIndex == 0)
                    {
                        //Insertamos Relación
                        resultado = insertaRelacionConcepto();
                    }
                    else
                    {
                        //Insertamos Relación
                        resultado = insertaRelacionConceptoPS();
                    }
                    break;
                //No existe el Producto
                case "Producto":
                    //Validamos Tipo de Relación a Insertar
                    //Unidad
                    if (mtvPublicacion.ActiveViewIndex == 0)
                    {
                        //Insertamos Relación
                        resultado = insertaRelacionProducto();
                    }
                    else
                    {
                        //Insertamos Relación
                        resultado = insertaRelacionProductoPS();
                    }
                    break;

            }
            //Validamos Resultado
            if (resultado.OperacionExitosa)
            {
                //Validamos Tipo de Información
                if (mtvPublicacion.ActiveViewIndex == 0)
                {
                    //Validamos Obtención de Información del Viaje de la Publicacipón de Unidad
                    obtieneServicioPU(control, out resultado);
                }
                else
                {
                    //Validamos Obtención de Información del Viaje de la Publicacipón de Servicio
                    obtieneServicioPS(control, out resultado);
                }

                //Validamos Resultado
                if (resultado.OperacionExitosa)
                {
                    //Unidad
                    if (mtvPublicacion.ActiveViewIndex == 0)
                    {
                        //Mostramos Nuevo Elemento
                        copiarServicioPUFinal(control, true);
                    }
                    else
                    {
                        copiarServicioPSFinal(control, true);
                    }
                }
                else
                {
                    DataSet ds = new DataSet();
                    //Mostramos Informacion
                    muestraElemento(control, out ds);
                }
            }
            //Mostrando Mensaje de Operación
            TSDK.ASP.ScriptServer.MuestraNotificacion(btnAgregarElemento, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }


        /// <summary>
        /// Inicializamos Valores 
        /// </summary>
        private void inicializaValoresElemento()
        {
            //Limpiamos Valores
            lblElemento.Text = "";
            lblDescipcion.Text = "";
            lblIdElemento.Text = "";
            txtElemento.Text = "";
        }

        /// <summary>
        /// Carga de catálogos requeridos
        /// </summary>
        private void cargaCatalogosPU()
        {
            //Catálogo de tamaño de GV
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoCiudades, "", 26);
            //Catálogo de tamaño de GV 
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoUnidades, "", 26);
            //Catálogo de tamaño de GV 
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoRespuestas, "", 26);
            //Catálogo de tamaño de GV 
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoInfoViaje, "", 26);
            //Catálogo de tamaño de GV 
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoResultadoRespuesta, "", 26);
            //Catálogo de tamaño de GV 
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoEventosDeseados, "", 26);
        }

        /// <summary>
        /// Carga de catálogos requeridos
        /// </summary>
        private void cargaCatalogosPS()
        {
            //Catálogo de tamaño de GV 
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoViajes, "", 26);
            //Catálogo de tamaño de GV 
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoRespuestasPS, "", 26);
            //Catálogo de tamaño de GV 
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoParadasPS, "", 26);
            //Catálogo de tamaño de GV 
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddTamanoResultadoRespuestaPS, "", 26);
        }

        /// <summary>
        /// Evento generado al Aceptar la Respuesta
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarRespuesta_Click(object sender, EventArgs e)
        {
            //Declaramos Objeto resultado
            RetornoOperacion resultado = new RetornoOperacion();
            //Validamos Pestaña Activa de Unidad
            if (mtvPublicacion.ActiveViewIndex == 0)
            {
                //Aceptamos Respuesta
                resultado = aceptarRespuesta();
                //Validamos resultado
                if (resultado.OperacionExitosa)
                {
                    //Cerramos ventana Modal
                    alternaVentanaModal("aceptarRespuesta", btnAceptarRespuesta);
                    //Mostrando ventana modal 
                    alternaVentanaModal("opcionSeleccionRespuesta", btnAceptarRespuesta);
                    //Cargamos Respuestas
                    cargaRespuestas(btnAceptarRespuesta);
                }
            }
            else
            {
                //Aceptamos Respuesta de la Publicación de Servicio
                resultado = aceptarRespuestaPS();
                //Validamos resultado
                if (resultado.OperacionExitosa)
                {
                    //Cerramos ventana Modal
                    alternaVentanaModal("aceptarRespuesta", btnAceptarRespuesta);
                    //Mostrando ventana modal 
                    alternaVentanaModal("opcionSeleccionRespuestaPS", btnAceptarRespuesta);
                    //Cargamos Respuestas
                    cargaRespuestasPS(btnAceptarRespuesta);
                }
            }
        }

        /// <summary>
        /// Evento generado al Confirmar la Publicación de la Unidad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnConfirmarRespuesta_Click(object sender, EventArgs e)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Validamos Pestaña Activa de Unidad
            if (mtvPublicacion.ActiveViewIndex == 0)
            {
                //Documentamos Servicio
                resultado = documentaServicioUnidad();

                //Validamos Resultado
                if (resultado.OperacionExitosa)
                {
                    //Cerramos Ventana modal
                    alternaVentanaModal("confirmarRespuestaPU", btnConfirmarRespuesta);
                    //Cargamos Publicaciones de Unidades
                    cargaPublicaciones();
                }
                //Mostrando Mensaje de Operación
                TSDK.ASP.ScriptServer.MuestraNotificacion(btnConfirmarRespuesta, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
            else
            {
                //Asignamos Valor
                lblValor.Text = "No";

                //Copiamos Servicio
                copiarServicioPSFinal(btnConfirmarRespuesta, false);
                //Cerramos Ventana modal
                alternaVentanaModal("confirmarRespuestaPU", btnConfirmarRespuesta);

            }
        }
    }
}