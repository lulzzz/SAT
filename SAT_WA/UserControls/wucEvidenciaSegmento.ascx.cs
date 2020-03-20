using SAT_CL.ControlEvidencia;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Datos;

namespace SAT.UserControls
{
    public partial class wucEvidenciaSegmento : System.Web.UI.UserControl
    {
        #region Atributos

        private int _id_servicio;
        private DataSet _ds;
        /// <summary>
        /// Propiedad que establece el Orden de la Tabulación de los Controles
        /// </summary>
        public short TabIndex
        {
            set
            {   
                //Asignando Orden
                ddlTamanoSegmentos.TabIndex =
                lnkExportarSegmento.TabIndex =
                gvSegmentos.TabIndex =
                ddlTamanoEvidencia.TabIndex =
                lnkExportarEvidencia.TabIndex =
                gvEvidencias.TabIndex = value;
            }

            get { return ddlTamanoSegmentos.TabIndex; }
        }
        /// <summary>
        /// Obtiene o establece el valor de habilitación de los controles contenidos
        /// </summary>
        public bool Enabled
        {
            set
            {
                //Asignando Habilitación
                ddlTamanoSegmentos.Enabled =
                lnkExportarSegmento.Enabled =
                gvSegmentos.Enabled =
                ddlTamanoEvidencia.Enabled =
                lnkExportarEvidencia.Enabled =
                gvEvidencias.Enabled = value;
            }

            get { return ddlTamanoSegmentos.Enabled; }
        }

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

                //Asignando Valores
                asignaAtributos();
            else
                //Asignando Valores
                recuperaAtributos();

            //Recuperando update panel de imagen con script jqzoom y registrando script de efectos
            ScriptManager.RegisterClientScriptBlock(uphplImagenZoom, uphplImagenZoom.GetType(), "jqzoom", @"$(document).ready(function() {$('.MYCLASS').jqzoom({ 
            zoomType: 'standard',  alwaysOn : false,  zoomWidth: 435,  zoomHeight:300,  position:'left',  
            xOffset: 125,  yOffset:0,  showEffect : 'fadein',  hideEffect: 'fadeout'  });});", true);
        }
        /// <summary>
        /// Evento desencadenado antes de Producirse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            //Asignando Valores
            asignaAtributos();
        }

        #region Eventos GridView "Segmentos"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvSegmentos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Validando el Tipo de Registro
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //
                //LinkButton lnkImp = e.Row.FindControl("lnkImprimirHI");


            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoSegmentos_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño de Registros
            Controles.CambiaTamañoPaginaGridView(gvSegmentos, this._ds.Tables["Table"], Convert.ToInt32(ddlTamanoSegmentos.SelectedValue));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportarSegmento_Click(object sender, EventArgs e)
        {
            //Exportando Excel
            Controles.ExportaContenidoGridView(this._ds.Tables["Table"], "Id");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkVerDocumentos_Click(object sender, EventArgs e)
        {
            //Validando que Existen Registros
            if(gvSegmentos.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                TSDK.ASP.Controles.SeleccionaFila(gvSegmentos, sender, "lnk", false);

                //Cargando Evidencias
                cargaEvidencias(Convert.ToInt32(gvSegmentos.SelectedDataKey["Id"]));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvSegmentos_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Validando que Existen Registros
            if (gvSegmentos.DataKeys.Count > 0)
            {
                //Asignando Ordenamiento
                this._ds.Tables["Table"].DefaultView.Sort = lblOrdenarSeg.Text;

                //Cambiando Ordenamiento
                lblOrdenarSeg.Text = Controles.CambiaSortExpressionGridView(gvSegmentos, this._ds.Tables["Table"], e.SortExpression);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvSegmentos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Validando que Existen Registros
            if (gvSegmentos.DataKeys.Count > 0)
            {
                //Asignando Ordenamiento
                this._ds.Tables["Table"].DefaultView.Sort = lblOrdenarSeg.Text;

                //Cambiando el Tamaño de la Página
                Controles.CambiaIndicePaginaGridView(gvSegmentos, this._ds.Tables["Table"], e.NewPageIndex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkImprimirHI_Click(object sender, EventArgs e)
        {
            //Validando que Existen Registros
            if (gvSegmentos.DataKeys.Count > 0)
            {
                //Asignando Ordenamiento
                TSDK.ASP.Controles.SeleccionaFila(gvSegmentos, sender, "lnk", false);

                //Obteniendo Ruta Relativa
                string urlReporte = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/UserControls/wucEvidenciaSegmento.ascx", "~/RDLC/Reporte.aspx");

                //Instanciando nueva ventana de navegador para apertura de registro
                TSDK.ASP.ScriptServer.AbreNuevaVentana(string.Format("{0}?idTipoReporte={1}&idRegistro={2}&idRegistroB={3}", urlReporte, "HojaInstruccion", Convert.ToInt32(gvSegmentos.SelectedDataKey["Id"]), Convert.ToInt32(gvSegmentos.SelectedDataKey["IdHI"])), "Hoja de Instrucción", "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500", Page);
            }
        }

        #endregion

        #region Eventos GridView "Evidencias"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoEvidencia_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño de Registros
            Controles.CambiaTamañoPaginaGridView(gvEvidencias, this._ds.Tables["Table1"], Convert.ToInt32(ddlTamanoEvidencia.SelectedValue));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportarEvidencia_Click(object sender, EventArgs e)
        {
            //Exportando Excel
            Controles.ExportaContenidoGridView(this._ds.Tables["Table1"], "Id");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvEvidencias_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Validando que Existen Registros
            if (gvEvidencias.DataKeys.Count > 0)
            {
                //Asignando Ordenamiento
                this._ds.Tables["Table1"].DefaultView.Sort = lblOrdenarEvi.Text;

                //Cambiando Ordenamiento
                lblOrdenarEvi.Text = Controles.CambiaSortExpressionGridView(gvEvidencias, this._ds.Tables["Table1"], e.SortExpression);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvEvidencias_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Validando que Existen Registros
            if (gvEvidencias.DataKeys.Count > 0)
            {
                //Asignando Ordenamiento
                this._ds.Tables["Table1"].DefaultView.Sort = lblOrdenarEvi.Text;

                //Cambiando el Tamaño de la Página
                Controles.CambiaIndicePaginaGridView(gvEvidencias, this._ds.Tables["Table1"], e.NewPageIndex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEvidencias_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvEvidencias.DataKeys.Count > 0)
            {
                //Seleccionando la Fila
                TSDK.ASP.Controles.SeleccionaFila(gvEvidencias, sender, "lnk", false);

                //Validando si existen Evidencias
                if (Convert.ToInt32(gvEvidencias.SelectedDataKey["Id"]) > 0)
                {
                    //Cargando Imagenes al DataList
                    cargaImagenesDetalle();
                    //Actualizamos el updatepanel
                    updtlImagenImagenes.Update();
                    uphplImagenZoom.Update();

                    //Ocultando Ventana
                    TSDK.ASP.ScriptServer.AlternarVentana(upgvEvidencias, upgvEvidencias.GetType(), "ImagenesEvidencia", "contenidoVentanaEvidencias", "ventanaEvidencias");
                }
            }
        }

        #region Eventos Modal Evidencias

        /// <summary>
        /// Evento producido al dar click sobre una imagen de la tira de imagenes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbThumbnailDoc_Click(object sender, EventArgs e)
        {
            //Determinando el comando solicitado para su carga en el control de imagen con zoom
            hplImagenZoom.NavigateUrl = string.Format("../Accesorios/VisorImagenID.aspx?t_carga=archivo&url={0}", ((LinkButton)sender).CommandName);

            //Imagen seleccionada
            imgImagenZoom.ImageUrl = string.Format("../Accesorios/VisorImagenID.aspx?t_carga=archivo&t_escala=pixcel&ancho=200&alto=150&url={0}", ((LinkButton)sender).CommandName);

            //Actualiz<ando panel de imagen en zoom
            uphplImagenZoom.Update();
        }
        /// <summary>
        /// Evento disparado al Presionar el Boton "Cerrar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkCerrarImagen_Click(object sender, EventArgs e)
        {
            //Seleccionando Fila
            TSDK.ASP.Controles.InicializaIndices(gvEvidencias);

            //Determinando el comando solicitado para su carga en el control de imagen con zoom
            hplImagenZoom.NavigateUrl = "~/Image/noDisponible.jpg";

            //Imagen seleccionada
            imgImagenZoom.ImageUrl = "~/Image/noDisponible.jpg";

            //Actualiz<ando panel de imagen en zoom
            uphplImagenZoom.Update();

            //Ocultando Ventana
            TSDK.ASP.ScriptServer.AlternarVentana(uplnkCerrarImagen, uplnkCerrarImagen.GetType(), "ImagenesEvidencia", "contenidoVentanaEvidencias", "ventanaEvidencias");
        }


        #endregion

        #endregion

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método encargado de Asignar los Atributos
        /// </summary>
        private void asignaAtributos()
        {
            //Guardando Atributo en ViewState
            ViewState["IdServicio"] = this._id_servicio;

            //Guardando DataSet en ViewState
            ViewState["DS"] = this._ds;
        }
        /// <summary>
        /// Método encargado de Recuperar los Atributos
        /// </summary>
        private void recuperaAtributos()
        {
            //Recuperando Atributos
            if (Convert.ToInt32(ViewState["IdServicio"]) != 0)

                //Asignando Valor Almacenado en ViewState
                this._id_servicio = Convert.ToInt32(ViewState["IdServicio"]);

            //Validando que Existan las Tablas
            if (ViewState["DS"] != null)

                //Asignando Valor Almacenar en ViewState
                this._ds = (DataSet)ViewState["DS"];
        }
        /// <summary>
        /// Método encargado de Cargar los Catalogos de la Forma
        /// </summary>
        private void cargaCatalogos()
        {
            //Tamaño del GridView
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoSegmentos, "", 26);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoEvidencia, "", 26);
        }
        /// <summary>
        /// Método encargado de Cargar los Segmentos
        /// </summary>
        private void cargaSegmentos()
        {
            //Inicializando indice de selección
            Controles.InicializaIndices(gvSegmentos);

            //Obteniendo detalles de viaje
            using (DataTable dt = SAT_CL.Despacho.SegmentoCarga.CargaSegmentosHI(this._id_servicio))
            {
                //Validando que la Tabla Contenga Registros
                if (Validacion.ValidaOrigenDatos(dt))
                {
                    //Cargando GridView de Viajes
                    Controles.CargaGridView(gvSegmentos, dt, "Id-IdHI", lblOrdenarEvi.Text, true, 5);

                    //Añadiendo Tabla a DataSet de Session
                    this._ds = TSDK.Datos.OrigenDatos.AñadeTablaDataSet(this._ds, dt, "Table");
                }
                else
                {
                    //Inicializando gridView
                    Controles.InicializaGridview(gvSegmentos);

                    //Borrando de sesión los viajes cargados anteriormente
                    this._ds = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                }
            }
        }
        /// <summary>
        /// Método encargado de Cargar las Evidencias dado un Segmento de Viaje
        /// </summary>
        private void cargaEvidencias(int id_segmento)
        {
            //Inicializando indice de selección
            Controles.InicializaIndices(gvEvidencias);

            //Validando que Exista una Hoja de Instrucción
            if (Convert.ToInt32(gvSegmentos.SelectedDataKey["IdHI"]) > 0)
            {
                //Obteniendo detalles de viaje
                using (DataTable dt = ControlEvidenciaDocumento.ObtieneControlEvidenciaDocumentosImagenes(this._id_servicio, id_segmento))
                {
                    //Validando que la Tabla Contenga Registros
                    if (Validacion.ValidaOrigenDatos(dt))
                    {
                        //Cargando GridView de Viajes
                        Controles.CargaGridView(gvEvidencias, dt, "Id", lblOrdenarEvi.Text, true, 5);

                        //Añadiendo Tabla a DataSet de Session
                        this._ds = TSDK.Datos.OrigenDatos.AñadeTablaDataSet(this._ds, dt, "Table1");
                    }
                    else
                    {
                        //Inicializando gridView
                        Controles.InicializaGridview(gvEvidencias);

                        //Borrando de sesión los viajes cargados anteriormente
                        this._ds = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
                    }
                }
            }
            else
            {
                //Inicializando gridView
                Controles.InicializaGridview(gvEvidencias);

                //Borrando de sesión los viajes cargados anteriormente
                this._ds = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
            }
        }
        /// <summary>
        /// Método Privado encargado de Cargar la Tira de Imagenes
        /// </summary>
        private void cargaImagenesDetalle()
        {
            //Realizando la carga de URL de imagenes a mostrar
            using (DataTable mit = SAT_CL.ControlEvidencia.HojaInstruccionDocumento.ObtieneImagenesHojaInstruccionDocumentos(Convert.ToInt32(gvEvidencias.SelectedDataKey["Id"])))
            {
                //Validando que existan imagenes a mostrar
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(mit))

                    //Cargando DataList
                    Controles.CargaDataList(dtlImagenImagenes, mit, "URL", "", "");
                else
                    //Inicializando DataList
                    Controles.CargaDataList(dtlImagenImagenes, null, "URL", "", "");
            }
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método encargado de Inicializar el Control de Usuario
        /// </summary>
        /// <param name="id_servicio">Servicio</param>
        public void InicializaControlUsuario(int id_servicio)
        {
            //Asignando Atributo
            this._id_servicio = id_servicio;

            //Invocando Método de Carga
            cargaCatalogos();

            //Inicializando GridView
            TSDK.ASP.Controles.InicializaGridview(gvSegmentos);
            TSDK.ASP.Controles.InicializaGridview(gvEvidencias);

            //Cargando Segmentos
            cargaSegmentos();
        }

        #endregion
    }
}