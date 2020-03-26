using SAT_CL.ControlPatio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.ControlPatio
{
    public partial class ControlAccesoGeneral : System.Web.UI.Page
    {
        #region Eventos

        /// <summary>
        /// Evento Producido al Generarse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Aplicando seguridad de la forma
            SAT_CL.Seguridad.Forma.AplicaSeguridadForma(this, "content1", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
            //Validando si se produjo un PostBack
            if (!(Page.IsPostBack))

                //Invocando Método de Inicialización
                inicializaPagina();

            //Construyendo Script
            string script = @"<script type='text/javascript'>
                                $(document).ready(function(){
                                $('#" + txtTransportista.ClientID + "').autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=21&param=" + ddlPatio.SelectedValue + @"'});
                                 $('#" + txtDescripcion.ClientID + "').autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=66&param=" + ddlPatio.SelectedValue + @"'});
                                });
                            </script>";

            //Registrando Script
            ScriptManager.RegisterStartupScript(this, this.GetType(), "CargaTransportista", script, false);
        }
        /// <summary>
        /// Evento generado al cambiar la Vista 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnVista_OnClick(object sender, EventArgs e)
        {   //Determinando el tipo de acción a realizar
            switch (((Button)sender).CommandName)
            {
                case "Entrada":
                    mtvControlAcceso.ActiveViewIndex = 0;
                    btnEntrada.CssClass = "boton_pestana_activo";
                    btnSalida.CssClass = "boton_pestana";
                    break;
                case "Salida":
                    mtvControlAcceso.ActiveViewIndex = 1;
                    btnEntrada.CssClass = "boton_pestana";
                    btnSalida.CssClass = "boton_pestana_activo";
                    txtBPlacas.Text = "";
                    txtBDescripcion.Text = "";
                    break;
            }
        }

        #region Entradas

        /// <summary>
        /// Evento Producido al Presionar el Link "Mas Unidades"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkMasUnidades_Click(object sender, EventArgs e)
        {   //Validando el Comando del Control
            switch (lnkMasUnidades.CommandName)
            {
                case "Todas":
                    {   //Cargando los Tipos de Entidades
                        SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipo, "", 3212);
                        //Ocultando Controles
                        ddlTipo.Visible = false;
                        //Habilitando Controles
                        rdbRabon.Enabled =
                        rdbRemolque.Enabled = true;
                        lnkMasUnidades.CommandName = "Algunas";
                        lnkMasUnidades.Text = "Mas Unidades";
                        ddlTipo.Enabled = true;

                        break;
                    }
                case "Algunas":
                    {   //Mostrando Controles
                        ddlTipo.Visible = true;
                        //Deshabilitando Controles
                        rdbRabon.Enabled =
                        rdbRemolque.Enabled = false;
                        lnkMasUnidades.CommandName = "Todas";
                        lnkMasUnidades.Text = "Menos Unidades";
                        ddlTipo.Enabled = true;

                        break;
                    }
            }
            //Configurando Controles
            configuraControles(ddlTipo.SelectedValue);
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice del Control "Patios"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlPatio_SelectedIndexChanged(object sender, EventArgs e)
        {   //Cargando los Accesos de ese Patio
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlAcceso, 35, "Ninguno", Convert.ToInt32(ddlPatio.SelectedValue), "", 0, "");
            //Actualizamos los indicadores de operacion
            inicializaIndicadoresUnidad();
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice del Control "Tipo"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTipo_SelectedIndexChanged(object sender, EventArgs e)
        {   //Configurando Controles
            configuraControles(ddlTipo.SelectedValue);
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Ingresar Unidades"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEntrar_Click(object sender, EventArgs e)
        {   //Invocando Método de Guardado
            guardaAcceso();
            //Actualizamos los indicadores de unidad
            inicializaIndicadoresUnidad();
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton Agregar Unidad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            //Declarando Variable Auxiliar
            byte id_tipo_detalle = 0;
            string tipo_detalle = "";

            //Validando el Comando del Control
            if (lnkMasUnidades.CommandName == "Todas")
            {
                //Asignando Valores
                id_tipo_detalle = Convert.ToByte(ddlTipo.SelectedValue);
                tipo_detalle = ddlTipo.SelectedItem.Text;
            }
            else
            {
                //Asignando Valores
                id_tipo_detalle = Convert.ToByte(rdbRemolque.Checked ? "1" : "2");
                tipo_detalle = rdbRemolque.Checked ? "Remolque" : "Rabon";
            }
            //Validando el Nombre del Comando
            switch (((Button)sender).CommandName)
            {
                case "Agregar":
                    {   //Validando Tabla de Session
                        if (Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table")))
                        {   //Obteniendo Valor Maximo
                            int max = Convert.ToByte(((DataSet)Session["DS"]).Tables["Table"].AsEnumerable().Max(dr => dr["Ind"]));
                            //Agregando Fila
                            ((DataSet)Session["DS"]).Tables["Table"].Rows.Add(max + 1, 0, Convert.ToInt32(Cadena.VerificaCadenaVacia(Cadena.RegresaCadenaSeparada(txtTransportista.Text, "ID:", 1), "0")), Cadena.RegresaCadenaSeparada(txtTransportista.Text, "ID:", 0).ToString(),
                                         id_tipo_detalle, tipo_detalle, Convert.ToInt32(Cadena.VerificaCadenaVacia(Cadena.RegresaCadenaSeparada(txtDescripcion.Text, "ID:", 1), "0")), Cadena.RegresaCadenaSeparada(txtDescripcion.Text, "ID:", 0).ToString(), txtIdentificador.Text, txtReferencia.Text, ddlEstado.SelectedValue, ddlEstado.SelectedItem.Text);

                        }
                        else
                        {   //Declarando Tabla
                            using (DataTable dtUnidades = new DataTable())
                            {   //Añadiendo Columnas
                                dtUnidades.Columns.Add("Ind", typeof(byte));
                                dtUnidades.Columns.Add("Id", typeof(int));
                                dtUnidades.Columns.Add("IdTransportista", typeof(int));
                                dtUnidades.Columns.Add("Transportista", typeof(string));
                                dtUnidades.Columns.Add("IdTipo", typeof(byte));
                                dtUnidades.Columns.Add("Tipo", typeof(string));
                                dtUnidades.Columns.Add("IdUnidad", typeof(byte));
                                dtUnidades.Columns.Add("Unidad", typeof(string));
                                dtUnidades.Columns.Add("Identificador", typeof(string));
                                dtUnidades.Columns.Add("Referencia", typeof(string));
                                dtUnidades.Columns.Add("IdEstado", typeof(string));
                                dtUnidades.Columns.Add("Estado", typeof(string));
                                //Agregando Fila
                                dtUnidades.Rows.Add(1, 0, Convert.ToInt32(Cadena.VerificaCadenaVacia(Cadena.RegresaCadenaSeparada(txtTransportista.Text, "ID:", 1),"0")),Cadena.RegresaCadenaSeparada(txtTransportista.Text, "ID:", 0).ToString(),
                                 id_tipo_detalle, tipo_detalle, Convert.ToInt32(Cadena.VerificaCadenaVacia(Cadena.RegresaCadenaSeparada(txtDescripcion.Text, "ID:", 1), "0")), Cadena.RegresaCadenaSeparada(txtDescripcion.Text, "ID:", 0).ToString(), txtIdentificador.Text, txtReferencia.Text, ddlEstado.SelectedValue, ddlEstado.SelectedItem.Text);
                                //Añadiendo Tabla a DataSet
                                Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtUnidades, "Table");
                            }
                        }

                        break;
                    }
                case "Guardar":
                    {   //Obteniendo Fila para Actualizar
                        ((DataSet)Session["DS"]).Tables["Table"].Select(string.Format("[Ind] = '{0}'", Convert.ToByte(gvEntidades.SelectedDataKey["Ind"])))
                            .ToList<DataRow>()
                            .ForEach(dr =>
                            {   //Actualizando Valores
                                dr["IdTransportista"] = Convert.ToInt32(Cadena.VerificaCadenaVacia(Cadena.RegresaCadenaSeparada(txtTransportista.Text, "ID:", 1), "0"));
                                dr["Transportista"] = Cadena.RegresaCadenaSeparada(txtTransportista.Text, "ID:", 0).ToString();
                                dr["IdTipo"] = id_tipo_detalle;
                                dr["Tipo"] = tipo_detalle;
                                dr["IdUnidad"] = Convert.ToInt32(Cadena.VerificaCadenaVacia(Cadena.RegresaCadenaSeparada(txtDescripcion.Text, "ID:", 1), "0"));
                                dr["Unidad"] = Cadena.RegresaCadenaSeparada(txtDescripcion.Text, "ID:", 0).ToString();
                                dr["Identificador"] = txtIdentificador.Text;
                                dr["Referencia"] = txtReferencia.Text;
                                dr["IdEstado"] = ddlEstado.SelectedValue;
                                dr["Estado"] = ddlEstado.SelectedItem.Text;
                            });
                        //Aceptando Cambios
                        ((DataSet)Session["DS"]).Tables["Table"].AcceptChanges();
                        //Configurando Control
                        btnAgregar.CommandName = "Agregar";
                        btnAgregar.Text = "";

                        break;
                    }
            }
            //Cargando GridView
            TSDK.ASP.Controles.CargaGridView(gvEntidades, ((DataSet)Session["DS"]).Tables["Table"], "Ind-Id", "", true, 1);
            TSDK.ASP.Controles.InicializaIndices(gvEntidades);
            //Invocando Método que Limpia los Controles
            limpiaControles();
            //Asignando Enfoque al Terminar la Acción
            txtDescripcion.Focus();
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Cancelar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {   //Invocando Método de Limpieza de Controles
            limpiaControles();
            //if(gvEntidades.SelectedIndex == -1)
            //Limpiando el Control del Transportista
            //txtTransportista.Text = "";
            //Inicializando Indices
            TSDK.ASP.Controles.InicializaIndices(gvEntidades);
        }
        /// <summary>
        /// Evento Text Descripcion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtDescripcion_TextChanged(object sender, EventArgs e)
        {
            //Instanciando Unidad
            using (SAT_CL.ControlPatio.UnidadPatio uni = new SAT_CL.ControlPatio.UnidadPatio(Convert.ToInt32(Cadena.VerificaCadenaVacia(Cadena.RegresaCadenaSeparada(txtDescripcion.Text, "ID:", 1), "0"))))
            {
                if (uni.habilitar)
                {
                    txtIdentificador.Text = uni.placas;
                    txtReferencia.Focus();
                }
                else
                {
                    txtIdentificador.Text = "";
                    txtIdentificador.Focus();
                }
            }
        }

        #region Eventos GridView "Entidades"

        /// <summary>
        /// Eventos Producido al dar clic al Link "Exportar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportar_Click(object sender, EventArgs e)
        {   //Validando que existan Registros
            if (gvEntidades.DataKeys.Count > 0)
                //Exportando Contenido del GridView
                TSDK.ASP.Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "Id");
        }
        /// <summary>
        /// Evento Producido al Cambiar el Ordenamiento del GridView "Entidades"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvEntidades_Sorting(object sender, GridViewSortEventArgs e)
        {   //Invocando Método de Cambio de Ordenamiento
            lblOrdenado.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvEntidades, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression);
        }
        /// <summary>
        /// Evento Producido al Cambiar la Paginación del GridView "Entidades"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvEntidades_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {   //Invocando Método de Cambio ed Paginación
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvEntidades, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex);
        }
        /// <summary>
        /// Evento Producido al dar clic al Link "Editar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEditar_Click(object sender, EventArgs e)
        {   //Validando que existan Registros
            if (gvEntidades.DataKeys.Count > 0)
            {   //Seleccionando Fila
                TSDK.ASP.Controles.SeleccionaFila(gvEntidades, sender, "lnk", false);
                //Configurando Controles
                btnAgregar.CommandName = "Guardar";
                btnAgregar.Text = "";
                //Obteniendo Fila
                IEnumerable<DataRow> fila = ((DataSet)Session["DS"]).Tables["Table"].Select(string.Format("[Ind] = '{0}'", Convert.ToByte(gvEntidades.SelectedDataKey["Ind"])))
                                            .ToList<DataRow>();
                //Recorriendo la Fila
                foreach (DataRow dr in fila)
                {   //Validando 
                    if (Convert.ToInt32(dr["IdTipo"]) > 2)
                    {   //Mostrando Controles
                        lblTipo.Visible =
                        ddlTipo.Visible = true;
                        //Deshabilitando Controles
                        rdbRabon.Enabled =
                        rdbRemolque.Enabled = false;
                        lnkMasUnidades.CommandName = "Todas";
                        lnkMasUnidades.Text = "Menos Unidades";
                        ddlTipo.Enabled = true;
                        //Asignando Tipo
                        ddlTipo.SelectedValue = dr["IdTipo"].ToString();
                    }
                    else
                    {   //Ocultando Controles
                        lblTipo.Visible =
                        ddlTipo.Visible = false;
                        //Habilitando Controles
                        rdbRabon.Enabled =
                        rdbRemolque.Enabled = true;
                        lnkMasUnidades.CommandName = "Algunas";
                        lnkMasUnidades.Text = "Mas Unidades";
                        ddlTipo.Enabled = true;
                        //Asignando Tipo
                        rdbRemolque.Checked = dr["IdTipo"].ToString() == "1" ? true : false;
                        rdbRabon.Checked = dr["IdTipo"].ToString() == "2" ? true : false;
                    }
                    //Invocando Método de COnfiguración de Controles
                    configuraControles(dr["IdTipo"].ToString());
                    //Instanciando Transportista
                    using (SAT_CL.ControlPatio.PatioTransportista transportista = new SAT_CL.ControlPatio.PatioTransportista(Convert.ToInt32(dr["IdTransportista"])))
                    {   //Validando que existe el Transportista
                        if (transportista.habilitar)
                            //Asignando Valor
                            txtTransportista.Text = string.Format("{0}ID:{1}", transportista.nombre, transportista.id_transportista_patio);
                        else//Limpiando valor
                            txtTransportista.Text = dr["Transportista"].ToString();
                    }
                    //Instanciando Unidad
                    using (SAT_CL.ControlPatio.UnidadPatio objUnidad = new SAT_CL.ControlPatio.UnidadPatio(Convert.ToInt32(dr["IdUnidad"])))
                    {   //Validando que existe el Transportista
                        if (objUnidad.habilitar)
                            //Asignando Valor
                            txtDescripcion.Text = string.Format("{0}ID:{1}", objUnidad.no_economico, objUnidad.id_unidad_patio);
                        else//Limpiando valor
                            txtDescripcion.Text = dr["Unidad"].ToString();
                    }
                    //Asignando Valores
                    txtIdentificador.Text = dr["Identificador"].ToString();
                    txtReferencia.Text = dr["Referencia"].ToString();
                    ddlEstado.SelectedValue = dr["IdEstado"].ToString();                 
                }
            }
        }
        /// <summary>
        /// Evento Producido al dar clic al Link "Eliminar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEliminar_Click(object sender, EventArgs e)
        {   //Validando que existan Registros
            if (gvEntidades.DataKeys.Count > 0)
            {   //Seleccionando Fila
                TSDK.ASP.Controles.SeleccionaFila(gvEntidades, sender, "lnk", false);
                //Eliminando Fila Seleccionada
                ((DataSet)Session["DS"]).Tables["Table"].AsEnumerable().Where(dr => dr.Field<byte>("Ind") == Convert.ToByte(gvEntidades.SelectedDataKey["Ind"]))
                    .ToList().ForEach(row => row.Delete());
                //Aceptando Cambios
                ((DataSet)Session["DS"]).Tables["Table"].AcceptChanges();
                //Validando Origen de Datos
                if (Validacion.ValidaOrigenDatos(((DataSet)Session["DS"]).Tables["Table"]))
                    //Cargando GridView
                    TSDK.ASP.Controles.CargaGridView(gvEntidades, ((DataSet)Session["DS"]).Tables["Table"], "Ind-Id", "", true, 1);
                else//Inicializando GridView
                    TSDK.ASP.Controles.InicializaGridview(gvEntidades);
                //Inicializando Indices
                TSDK.ASP.Controles.InicializaIndices(gvEntidades);
            }
        }
        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView "Entidades"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamano_SelectedIndexChanged(object sender, EventArgs e)
        {   //Invocando Método de Cambio de Tamaño
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvEntidades, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamano.SelectedValue));
        }

        #endregion

        #endregion

        #region Eventos Salida

        /// <summary>
        /// Evento Producido al dar clic en el Boton "Buscar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {   //Invocando Método de Busqueda
            buscaAccesos();
        }

        #region Eventos GridView "Unidades"

        /// <summary>
        /// Evento Producido al Cambiar la Expresión de Ordenamiento del GridView "Unidades"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvUnidades_Sorting(object sender, GridViewSortEventArgs e)
        {   //Cambiando Expresión de Ordenamiento
            lblOrdenadoUnidad.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvUnidades, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.SortExpression);
            //Inicializando Indices
            TSDK.ASP.Controles.InicializaIndices(gvUnidades);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvUnidades_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {   //Cambiando Indice de Página
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvUnidades, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.NewPageIndex);
            //Inicializando Indices
            TSDK.ASP.Controles.InicializaIndices(gvUnidades);
        }
        /// <summary>
        /// Evento Producido al Presionar el LinkButton "No. Unidad"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkNoUnidad_Click(object sender, EventArgs e)
        {   //Validando que existan Registros en el GridView
            if (gvUnidades.DataKeys.Count > 0)
            {    //Seleccionando Fila
                TSDK.ASP.Controles.SeleccionaFila(gvUnidades, sender, "lnk", false);
                //Obteniendo Unidades Ligadas al Acceso
                using (DataTable dtUnidadesAcceso = DetalleAccesoPatio.ObtieneUnidadesAcceso(Convert.ToInt32(gvUnidades.SelectedDataKey["IdAccesoEntrada"]), Convert.ToInt32(gvUnidades.SelectedDataKey["Id"])))
                {   //Validando que existan Unidades
                    if (Validacion.ValidaOrigenDatos(dtUnidadesAcceso))
                    {   //Añadiendo Tabla a DataSet de Session
                        Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtUnidadesAcceso, "Table2");
                        //Quitando Registros Agregados
                        quitaRegistrosAgregados();
                        //Validando que existan Unidades
                        if (Validacion.ValidaOrigenDatos(((DataSet)Session["DS"]).Tables["Table2"]))
                        {   //Cargando GridView
                            TSDK.ASP.Controles.CargaGridView(gvSalidas, ((DataSet)Session["DS"]).Tables["Table2"], "Id-IdAccesoEnt", "", true, 1);
                            //Cambiando Comando de Nombre
                            btnAceptarSalida.CommandName = "Agregar";
                            //Declarando Script de Ventana Modal
                            string script = @"<script type='text/javascript'>
                                    $('#contenidoAgregarUnidades').animate({ width: 'toggle' });
                                    $('#agregarUnidades').animate({ width: 'toggle' });
                                  </script>";
                            //Registrando Script
                            ScriptManager.RegisterStartupScript(upgvUnidades, upgvUnidades.GetType(), "AgregarUnidades", script, false);
                        }
                        else
                        {   //Inicializando GridView
                            TSDK.ASP.Controles.InicializaGridview(gvSalidas);
                            //Eliminando Tabla de DataSet de Session
                            Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table2");
                            //Obteniendo Fecha
                            DateTime fecha;
                            DateTime.TryParse(txtFecha.Text, out fecha);
                            //Declarando Objeto de Retorno
                            RetornoOperacion result = new RetornoOperacion();
                            //Instanciando Detalle
                            using (DetalleAccesoPatio detail = new DetalleAccesoPatio(Convert.ToInt32(gvUnidades.SelectedDataKey["Id"])))
                            {   //Validando registro
                                if (detail.id_detalle_acceso_patio > 0)
                                {   //Obteniendo Transportista
                                    string transportista = gvUnidades.Rows[gvUnidades.SelectedIndex].Cells[4].Text;
                                    //Validando que exista la Tabla
                                    if (Validacion.ValidaOrigenDatos((DataSet)Session["DS"], "Table3"))
                                    {   //Añadiendo Registro a la Tabla
                                        ((DataSet)Session["DS"]).Tables["Table3"].Rows.Add(detail.id_detalle_acceso_patio,
                                            detail.descripcion_detalle_acceso, transportista, detail.identificacion_detalle_acceso,
                                            gvUnidades.Rows[gvUnidades.SelectedIndex].Cells[6].Text, detail.bit_cargado == true ? "Cargado" : "Vacio");
                                    }
                                    else
                                    {   //Creando Tabla
                                        using (DataTable dtUnidadesTemp = new DataTable())
                                        {   //Creando Columnas
                                            dtUnidadesTemp.Columns.Add("Id", typeof(int));
                                            dtUnidadesTemp.Columns.Add("NoUnidad", typeof(string));
                                            dtUnidadesTemp.Columns.Add("Transportista", typeof(string));
                                            dtUnidadesTemp.Columns.Add("Identificador", typeof(string));
                                            dtUnidadesTemp.Columns.Add("TiempoEst", typeof(string));
                                            dtUnidadesTemp.Columns.Add("Estado", typeof(string));
                                            //Añadiendo Registro a la Tabla
                                            dtUnidadesTemp.Rows.Add(detail.id_detalle_acceso_patio,
                                                detail.descripcion_detalle_acceso, transportista, detail.identificacion_detalle_acceso,
                                                gvUnidades.Rows[gvUnidades.SelectedIndex].Cells[6].Text, detail.bit_cargado == true ? "Cargado" : "Vacio");
                                            //Añadiendo Tabla a Session
                                            Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtUnidadesTemp, "Table3");
                                        }
                                    }
                                    //Validando que exista la Tabla
                                    if (Validacion.ValidaOrigenDatos((DataSet)Session["DS"], "Table3"))
                                    {   //Cargando GridView
                                        TSDK.ASP.Controles.CargaGridView(gvUnidadesTemp, ((DataSet)Session["DS"]).Tables["Table3"], "Id", "", true, 1);
                                        //Instanciando Mensaje
                                        result = new RetornoOperacion("Las Unidades se Agregaron Correctamente");
                                    }
                                    else
                                    {   //Inicializando GridView
                                        TSDK.ASP.Controles.InicializaGridview(gvUnidadesTemp);
                                        //Eliminando Tabla de Session
                                        Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table3");
                                        //Instanciando Mensaje
                                        result = new RetornoOperacion("");
                                    }
                                    //Inicializando Indices
                                    TSDK.ASP.Controles.InicializaIndices(gvUnidades);
                                    //Invocando Método de Busqueda
                                    buscaAccesos();
                                    //Mostrando Mensaje de Operación
                                    lblErrorAsignaSalida.Text = result.Mensaje;
                                }
                            }
                        }
                    }
                    else
                    {   //Inicializando GridView
                        TSDK.ASP.Controles.InicializaGridview(gvSalidas);
                        //Eliminando Tabla de DataSet de Session
                        Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table2");
                        //Obteniendo Fecha
                        DateTime fecha;
                        DateTime.TryParse(txtFecha.Text, out fecha);
                        //Declarando Objeto de Retorno
                        RetornoOperacion result = new RetornoOperacion();
                        //Instanciando Detalle
                        using (DetalleAccesoPatio detail = new DetalleAccesoPatio(Convert.ToInt32(gvUnidades.SelectedDataKey["Id"])))
                        {   //Validando registro
                            if (detail.id_detalle_acceso_patio > 0)
                            {   //Obteniendo Transportista
                                string transportista = gvUnidades.Rows[gvUnidades.SelectedIndex].Cells[4].Text;
                                //Validando que exista la Tabla
                                if (Validacion.ValidaOrigenDatos((DataSet)Session["DS"], "Table3"))
                                {   //Añadiendo Registro a la Tabla
                                    ((DataSet)Session["DS"]).Tables["Table3"].Rows.Add(detail.id_detalle_acceso_patio,
                                        detail.descripcion_detalle_acceso, transportista, detail.identificacion_detalle_acceso,
                                        gvUnidades.Rows[gvUnidades.SelectedIndex].Cells[6].Text, detail.bit_cargado == true ? "Cargado" : "Vacio");
                                }
                                else
                                {   //Creando Tabla
                                    using (DataTable dtUnidadesTemp = new DataTable())
                                    {   //Creando Columnas
                                        dtUnidadesTemp.Columns.Add("Id", typeof(int));
                                        dtUnidadesTemp.Columns.Add("NoUnidad", typeof(string));
                                        dtUnidadesTemp.Columns.Add("Transportista", typeof(string));
                                        dtUnidadesTemp.Columns.Add("Identificador", typeof(string));
                                        dtUnidadesTemp.Columns.Add("TiempoEst", typeof(string));
                                        dtUnidadesTemp.Columns.Add("Estado", typeof(string));
                                        //Añadiendo Registro a la Tabla
                                        dtUnidadesTemp.Rows.Add(detail.id_detalle_acceso_patio,
                                            detail.descripcion_detalle_acceso, transportista, detail.identificacion_detalle_acceso,
                                            gvUnidades.Rows[gvUnidades.SelectedIndex].Cells[6].Text, detail.bit_cargado == true ? "Cargado" : "Vacio");
                                        //Añadiendo Tabla a Session
                                        Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtUnidadesTemp, "Table3");
                                    }
                                }
                                //Validando que exista la Tabla
                                if (Validacion.ValidaOrigenDatos((DataSet)Session["DS"], "Table3"))
                                {   //Cargando GridView
                                    TSDK.ASP.Controles.CargaGridView(gvUnidadesTemp, ((DataSet)Session["DS"]).Tables["Table3"], "Id", "", true, 1);
                                    //Instanciando Mensaje
                                    result = new RetornoOperacion("Las Unidades se Agregaron Correctamente");
                                }
                                else
                                {   //Inicializando GridView
                                    TSDK.ASP.Controles.InicializaGridview(gvUnidadesTemp);
                                    //Eliminando Tabla de Session
                                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table3");
                                    //Instanciando Mensaje
                                    result = new RetornoOperacion("");
                                }
                                //Inicializando Indices
                                TSDK.ASP.Controles.InicializaIndices(gvUnidades);
                                //Invocando Método de Busqueda
                                buscaAccesos();
                                //Mostrando Mensaje de Operación
                                lblErrorAsignaSalida.Text = result.Mensaje;
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Evento Producido al Presionar el LinkButton "Dar Salida"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkDarSalida_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros en el GridView
            if (gvUnidades.DataKeys.Count > 0)
            {   //Seleccionando Fila
                TSDK.ASP.Controles.SeleccionaFila(gvUnidades, sender, "lnk", false);
                //Obteniendo Unidades Ligadas al Acceso
                using (DataTable dtUnidadesAcceso = DetalleAccesoPatio.ObtieneUnidadesAcceso(Convert.ToInt32(gvUnidades.SelectedDataKey["IdAccesoEntrada"]), Convert.ToInt32(gvUnidades.SelectedDataKey["Id"])))
                {
                    //Declarando Objeto de Retorno
                    RetornoOperacion result = new RetornoOperacion();
                    //Validando que existan Unidades
                    if (Validacion.ValidaOrigenDatos(dtUnidadesAcceso))
                    {
                        //Añadiendo Tabla a DataSet de Session
                        Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtUnidadesAcceso, "Table2");

                        //Quitando Registros Agregados
                        quitaRegistrosAgregados();

                        //Validando que existan Unidades
                        if (Validacion.ValidaOrigenDatos(((DataSet)Session["DS"]).Tables["Table2"]))
                        {
                            //Cargando GridView
                            TSDK.ASP.Controles.CargaGridView(gvSalidas, ((DataSet)Session["DS"]).Tables["Table2"], "Id-IdAccesoEnt", "", true, 1);
                            //Cambiando Comando de Nombre
                            btnAceptarSalida.CommandName = "Salir";
                            //Declarando Script de Ventana Modal
                            string script = @"<script type='text/javascript'>
                                    $('#contenidoAgregarUnidades').animate({ width: 'toggle' });
                                    $('#agregarUnidades').animate({ width: 'toggle' });
                                  </script>";
                            //Registrando Script
                            ScriptManager.RegisterStartupScript(upgvUnidades, upgvUnidades.GetType(), "AgregarUnidades", script, false);
                        }
                        else
                        {   //Inicializando GridView
                            TSDK.ASP.Controles.InicializaGridview(gvSalidas);
                            //Eliminando Tabla de DataSet de Session
                            Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table2");
                            //Obteniendo Fecha
                            DateTime fecha;
                            DateTime.TryParse(txtFecha.Text, out fecha);
                            //Inicializando Bloque Transaccional
                            using (TransactionScope trans = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                            {   //Instanciando Detalle
                                using (DetalleAccesoPatio dap = new DetalleAccesoPatio(Convert.ToInt32(gvUnidades.SelectedDataKey["Id"])))
                                //Instanciando Acceso de Entrada
                                using (AccesoPatio ace = new AccesoPatio(dap.id_acceso_entrada))
                                {   //Validando que la Fecha de Entrada sea menor a la de Salida
                                    if (DateTime.Compare(ace.fecha_acceso, fecha) < 0)
                                    {   //Insertando Acceso al Patio
                                        result = AccesoPatio.InsertaAccesoPatio(Convert.ToInt32(ddlPatio.SelectedValue), Convert.ToInt32(ddlAcceso.SelectedValue),
                                                        AccesoPatio.TipoActualizacion.Web, AccesoPatio.TipoAcceso.Salida, fecha, "",
                                                        ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                        //Validando que la Operacion haya sido Exitosa
                                        if (result.OperacionExitosa)
                                        {   //Obteniendo Acceso de Salida
                                            int idAccesoSalida = result.IdRegistro;
                                            //Validando que exista el Detalle de Acceso
                                            if (dap.id_detalle_acceso_patio > 0)
                                            {   //Actualizando la Salida del Detalle
                                                result = dap.ActualizaSalidaDetalle(result.IdRegistro, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                //Validando Operacion
                                                if (result.OperacionExitosa)
                                                    //Completando Transacción
                                                    trans.Complete();
                                            }
                                        }
                                    }
                                    else//Instanciando Excepcion
                                        result = new RetornoOperacion("*La Fecha de Salida debe ser mayor a la de Entrada");
                                }
                            }
                            //Validando que la Operación sea Exitosa
                            if (result.OperacionExitosa)
                            {
                                //Invocando Método de Busqueda
                                buscaAccesos();

                                //Inicializando Indices
                                TSDK.ASP.Controles.InicializaIndices(gvUnidades);
                                TSDK.ASP.Controles.InicializaIndices(gvSalidas);
                                TSDK.ASP.Controles.InicializaGridview(gvSalidas);
                            }

                            //Mostrando Mensajes
                            lblErrorAsignaSalida.Text = result.Mensaje;
                        }
                    }
                    else
                    {
                        //Inicializando GridView
                        TSDK.ASP.Controles.InicializaGridview(gvSalidas);
                        //Eliminando Tabla de DataSet de Session
                        Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table2");
                        //Obteniendo Fecha
                        DateTime fecha;
                        DateTime.TryParse(txtFecha.Text, out fecha);
                        //Inicializando Bloque Transaccional
                        using (TransactionScope trans = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                        {   //Insertando Acceso al Patio
                            result = AccesoPatio.InsertaAccesoPatio(Convert.ToInt32(ddlPatio.SelectedValue), Convert.ToInt32(ddlAcceso.SelectedValue),
                                            AccesoPatio.TipoActualizacion.Web, AccesoPatio.TipoAcceso.Salida, fecha, "",
                                            ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                            //Validando que la Operacion haya sido Exitosa
                            if (result.OperacionExitosa)
                            {   //Obteniendo Acceso de Salida
                                int idAccesoSalida = result.IdRegistro;
                                //Instanciando Detalle de Acceso
                                using (DetalleAccesoPatio dap = new DetalleAccesoPatio(Convert.ToInt32(gvUnidades.SelectedDataKey["Id"])))
                                {
                                    //Validando que exista el Detalle de Acceso
                                    if (dap.id_detalle_acceso_patio > 0)
                                    {
                                        //Actualizando la Salida del Detalle
                                        result = dap.ActualizaSalidaDetalle(result.IdRegistro, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                        //Validando Operacion
                                        if (result.OperacionExitosa)
                                            //Completando Transacción
                                            trans.Complete();
                                    }
                                }
                            }
                        }
                        //Validando que la Operación sea Exitosa
                        if (result.OperacionExitosa)
                        {   //Invocando Método de Busqueda
                            buscaAccesos();

                            //Inicializando Indices
                            TSDK.ASP.Controles.InicializaIndices(gvUnidades);
                            TSDK.ASP.Controles.InicializaIndices(gvSalidas);
                            TSDK.ASP.Controles.InicializaGridview(gvSalidas);
                            txtBDescripcion.Text = "";
                            txtBPlacas.Text = "";
                        }
                        //Mostrando Mensajes
                        lblErrorAsignaSalida.Text = result.Mensaje;

                    }
                    //actualizamos los indicadores de la forma 
                    inicializaIndicadoresUnidad();
                    uplblEntradaSalida.Update();
                    upplblUnidades.Update();
                    upplblTiempo.Update();
                    uplblCargadasxSalir.Update();
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportarUnidades_Click(object sender, EventArgs e)
        {   //Validando que existan registros
            if (gvUnidades.DataKeys.Count > 0)
                //Invocando Método de Exportación
                TSDK.ASP.Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), "Id");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoUnidad_SelectedIndexChanged(object sender, EventArgs e)
        {   //Cambiando Tamaño de Página
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvUnidadesTemp, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), Convert.ToInt32(ddlTamanoUnidad.SelectedValue));
            //Inicializando Indices
            TSDK.ASP.Controles.InicializaIndices(gvUnidades);
        }

        /// <summary>
        /// Evento disparado al dar click en el boton actualizar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkActualizar_Click(object sender, EventArgs e)
        {
            inicializaIndicadoresUnidad();
        }
        #endregion

        #region Eventos GridView "Unidades Temporales"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportarTemp_Click(object sender, EventArgs e)
        {   //Validando que existan registros
            if (gvUnidadesTemp.DataKeys.Count > 0)
            {   //Invocando Método de Exportación
                TSDK.ASP.Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3"), "Id");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvUnidadesTemp_Sorting(object sender, GridViewSortEventArgs e)
        {   //Cambiando Expresión de Ordenamiento
            lblOrdenadoTemp.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvUnidadesTemp, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3"), e.SortExpression);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvUnidadesTemp_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {   //Cambiando Indice de Página
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvUnidadesTemp, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3"), e.NewPageIndex);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEliminar_Click1(object sender, EventArgs e)
        {   //Validando que existan Registros
            if (gvUnidadesTemp.DataKeys.Count > 0)
            {   //Seleccionando Fila
                TSDK.ASP.Controles.SeleccionaFila(gvUnidadesTemp, sender, "lnk", false);
                //Eliminando Fila
                ((DataSet)Session["DS"]).Tables["Table3"].Select(string.Format("[Id] = '{0}'", Convert.ToInt32(gvUnidadesTemp.SelectedDataKey["Id"])))
                                            .ToList<DataRow>().ForEach(dr => dr.Delete());
                //Aceptando los Cambios en la Tabla
                ((DataSet)Session["DS"]).Tables["Table3"].AcceptChanges();
                //Validando si aun existen registros
                if (Validacion.ValidaOrigenDatos((DataSet)Session["DS"], "Table3"))
                    //Cargando Controles
                    TSDK.ASP.Controles.CargaGridView(gvUnidadesTemp, ((DataSet)Session["DS"]).Tables["Table3"], "Id", "", true, 1);
                else
                {   //Inicializando Control
                    TSDK.ASP.Controles.InicializaGridview(gvUnidadesTemp);
                    //Eliminando Tabla de Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table3");
                }
                //Inicializando Indices
                TSDK.ASP.Controles.InicializaIndices(gvUnidadesTemp);
                //Buscando Accesos
                buscaAccesos();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoTemp_SelectedIndexChanged(object sender, EventArgs e)
        {   //Cambiando Tamaño de Página
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvUnidadesTemp, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3"), Convert.ToInt32(ddlTamanoTemp.SelectedValue));
        }

        #endregion

        #region Eventos Ventana Modal

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarSalida_Click(object sender, EventArgs e)
        {   //Obteniendo Filas Seleccionadas
            GridViewRow[] gvRows = TSDK.ASP.Controles.ObtenerFilasSeleccionadas(gvSalidas, "chkVarios");
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Validando el Comando del Boton
            switch (((Button)sender).CommandName)
            {
                case "Salir":
                    {   //Obteniendo Fecha
                        DateTime fecha;
                        DateTime.TryParse(txtFecha.Text, out fecha);
                        //Inicializando Bloque Transaccional
                        using (TransactionScope trans = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                        {   //Instanciando Detalle
                            using (DetalleAccesoPatio dap = new DetalleAccesoPatio(Convert.ToInt32(gvUnidades.SelectedDataKey["Id"])))
                            {   //Instanciando Acceso de Entrada
                                using (AccesoPatio ace = new AccesoPatio(dap.id_acceso_entrada))
                                {   //Validando que la Fecha de Entrada sea menor a la de Salida
                                    if (DateTime.Compare(ace.fecha_acceso, fecha) < 0)
                                    {   //Insertando Acceso al Patio
                                        result = AccesoPatio.InsertaAccesoPatio(Convert.ToInt32(ddlPatio.SelectedValue), Convert.ToInt32(ddlAcceso.SelectedValue),
                                                        AccesoPatio.TipoActualizacion.Web, AccesoPatio.TipoAcceso.Salida, fecha, "",
                                                        ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                        //Validando que la Operacion haya sido Exitosa
                                        if (result.OperacionExitosa)
                                        {   //Obteniendo Acceso de Salida
                                            int idAccesoSalida = result.IdRegistro;
                                            //Validando que exista el Detalle de Acceso
                                            if (dap.id_detalle_acceso_patio > 0)
                                                //Actualizando la Salida del Detalle
                                                result = dap.ActualizaSalidaDetalle(result.IdRegistro, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                            //¿Operacion Exitosa?
                                            if (result.OperacionExitosa)
                                            {   //Recorriendo cada Fila
                                                foreach (GridViewRow gvr in gvRows)
                                                {   //Obteniendo Indice
                                                    gvSalidas.SelectedIndex = gvr.RowIndex;
                                                    //Instanciando Detalle de Acceso
                                                    using (DetalleAccesoPatio detalle = new DetalleAccesoPatio(Convert.ToInt32(gvSalidas.SelectedDataKey["Id"])))
                                                    {   //Validando que exista el Detalle de Acceso
                                                        if (detalle.id_detalle_acceso_patio > 0)
                                                            //Actualizando la Salida del Detalle
                                                            result = detalle.ActualizaSalidaDetalle(idAccesoSalida, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                        //¿Operacion Exitosa?
                                                        if (!result.OperacionExitosa)
                                                            break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else//Instanciando Excepcion
                                        result = new RetornoOperacion("*La Fecha de Salida debe ser mayor a la de Entrada");
                                }
                            }
                            //¿Operacion Exitosa?
                            if (result.OperacionExitosa)
                                //Finalizando Transacción
                                trans.Complete();
                        }

                        break;
                    }
                case "Agregar":
                    {   //Instanciando Detalle
                        using (DetalleAccesoPatio detail = new DetalleAccesoPatio(Convert.ToInt32(gvUnidades.SelectedDataKey["Id"])))
                        {   //Validando registro
                            if (detail.id_detalle_acceso_patio > 0)
                            {   //Obteniendo Transportista
                                string transportista = gvUnidades.Rows[gvUnidades.SelectedIndex].Cells[4].Text;
                                //Validando que exista la Tabla
                                if (Validacion.ValidaOrigenDatos((DataSet)Session["DS"], "Table3"))
                                {   //Añadiendo Registro a la Tabla
                                    ((DataSet)Session["DS"]).Tables["Table3"].Rows.Add(detail.id_detalle_acceso_patio,
                                        detail.descripcion_detalle_acceso, transportista, detail.identificacion_detalle_acceso,
                                        gvUnidades.Rows[gvUnidades.SelectedIndex].Cells[6].Text, detail.bit_cargado == true ? "Cargado" : "Vacio");
                                }
                                else
                                {   //Creando Tabla
                                    using (DataTable dtUnidadesTemp = new DataTable())
                                    {   //Creando Columnas
                                        dtUnidadesTemp.Columns.Add("Id", typeof(int));
                                        dtUnidadesTemp.Columns.Add("NoUnidad", typeof(string));
                                        dtUnidadesTemp.Columns.Add("Transportista", typeof(string));
                                        dtUnidadesTemp.Columns.Add("Identificador", typeof(string));
                                        dtUnidadesTemp.Columns.Add("TiempoEst", typeof(string));
                                        dtUnidadesTemp.Columns.Add("Estado", typeof(string));
                                        //Añadiendo Registro a la Tabla
                                        dtUnidadesTemp.Rows.Add(detail.id_detalle_acceso_patio,
                                            detail.descripcion_detalle_acceso, transportista, detail.identificacion_detalle_acceso,
                                            gvUnidades.Rows[gvUnidades.SelectedIndex].Cells[6].Text, detail.bit_cargado == true ? "Cargado" : "Vacio");
                                        //Añadiendo Tabla a Session
                                        Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtUnidadesTemp, "Table3");
                                    }
                                }
                                //Validando que existan Registros Seleccionados
                                if (gvRows.Length > 0)
                                {   //Recorriendo cada Fila
                                    foreach (GridViewRow gvr in gvRows)
                                    {   //Seleccionando Registro Actual
                                        gvSalidas.SelectedIndex = gvr.RowIndex;
                                        //Instanciando Detalle
                                        using (DetalleAccesoPatio det = new DetalleAccesoPatio(Convert.ToInt32(gvSalidas.SelectedDataKey["Id"])))
                                        {   //Validando que exista el Registro
                                            if (det.id_detalle_acceso_patio > 0)
                                            {   //Añadiendo Registro a la Tabla
                                                ((DataSet)Session["DS"]).Tables["Table3"].Rows.Add(det.id_detalle_acceso_patio,
                                                    det.descripcion_detalle_acceso, transportista, det.identificacion_detalle_acceso,
                                                    gvSalidas.Rows[gvSalidas.SelectedIndex].Cells[4].Text, det.bit_cargado == true ? "Cargado" : "Vacio");
                                            }
                                        }
                                    }
                                }
                                //Validando que exista la Tabla
                                if (Validacion.ValidaOrigenDatos((DataSet)Session["DS"], "Table3"))
                                {   //Cargando GridView
                                    TSDK.ASP.Controles.CargaGridView(gvUnidadesTemp, ((DataSet)Session["DS"]).Tables["Table3"], "Id", "", true, 1);
                                    //Instanciando Mensaje
                                    result = new RetornoOperacion("Las Unidades se Agregaron Correctamente");
                                }
                                else
                                {   //Inicializando GridView
                                    TSDK.ASP.Controles.InicializaGridview(gvUnidadesTemp);
                                    //Eliminando Tabla de Session
                                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table3");
                                    //Instanciando Mensaje
                                    result = new RetornoOperacion("");
                                }
                            }
                        }

                        break;
                    }
            }
            //Invocando Método de Busqueda
            buscaAccesos();
            //Inicializando Indicadores
            inicializaIndicadoresUnidad();
            //Mostrando Mensajes
            lblErrorAsignaSalida.Text = result.Mensaje;
            //Inicializando Indices
            TSDK.ASP.Controles.InicializaIndices(gvUnidades);
            TSDK.ASP.Controles.InicializaIndices(gvSalidas);
            TSDK.ASP.Controles.InicializaGridview(gvSalidas);
            //Declarando Script de Ventana Modal
            string script = @"<script type='text/javascript'>
                                            $('#contenidoAgregarUnidades').animate({ width: 'toggle' });
                                            $('#agregarUnidades').animate({ width: 'toggle' });
                                          </script>";
            //Registrando Script
            ScriptManager.RegisterStartupScript(upbtnAceptarSalida, upbtnAceptarSalida.GetType(), "AgregarUnidades", script, false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarSalida_Click(object sender, EventArgs e)
        {   //Inicializando Indices
            TSDK.ASP.Controles.InicializaIndices(gvUnidades);
            //Restableciendo Controles de la Ventana
            OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table2");
            TSDK.ASP.Controles.InicializaGridview(gvSalidas);
            //Declarando Script de Ventana Modal
            string script = @"<script type='text/javascript'>
                                $('#contenidoAgregarUnidades').animate({ width: 'toggle' });
                                $('#agregarUnidades').animate({ width: 'toggle' });
                              </script>";
            //Registrando Script
            ScriptManager.RegisterStartupScript(upbtnCancelarSalida, upbtnCancelarSalida.GetType(), "AgregarUnidades", script, false);
        }

        #region Eventos GridView Modal

        /// <summary>
        /// Evento Producido al Marcar una Casilla del GridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkTodos_CheckedChanged(object sender, EventArgs e)
        {   //Validando que existan Registros
            if (gvSalidas.DataKeys.Count > 0)
            {   //Obteniendo Control
                CheckBox chk = (CheckBox)sender;
                //Validando el Id del Control
                switch (chk.ID)
                {
                    case "chkTodos":
                        //Seleccionando 
                        TSDK.ASP.Controles.SeleccionaFilasTodas(gvSalidas, "chkVarios", chk.Checked);
                        break;
                }
            }
        }

        #endregion

        #endregion

        /// <summary>
        /// Evento Producido al Presionar el Boton "Asignar Salida"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAsignarSalida_Click(object sender, EventArgs e)
        {
            //Invocando Método de Salida
            guardaAccesoSalida();
        }

        #endregion

        #endregion

        #region Métodos

        /// <summary>
        /// Método Privado encargado de Inicializar la Página
        /// </summary>
        private void inicializaPagina()
        {   //Inicializando GridView's
            TSDK.ASP.Controles.InicializaGridview(gvEntidades);
            TSDK.ASP.Controles.InicializaGridview(gvUnidades);
            TSDK.ASP.Controles.InicializaGridview(gvUnidadesTemp);
            //Invocando método de carga de catalogos
            cargaCatalogo();
            //Inicializamos los indicadores de patio
            inicializaIndicadoresUnidad();
            //rdbRemolque.Enabled =
            //rdbRabon.Enabled = false;
            //Asignando el foco al control transportista
            //txtTransportista.Focus();
            //Invocando Método de Limpieza
            limpiaControles();
            //Limpiando DataSet de Session
            Session["DS"] = null;
        }
        /// <summary>
        /// Método Privado encargado de Cargar los Catalogos de la Página
        /// </summary>
        private void cargaCatalogo()
        {   //Cargando Catalogo de Tamaño del GridView
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamano, "", 26);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoUnidad, "", 26);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoTemp, "", 26);
            //Obteniendo Instancia de Clase
            //Obteniendo Instancia de Clase
            using (UsuarioPatio up = UsuarioPatio.ObtieneInstanciaDefault(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
            {   //Cargando los Patios por Compania
                SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlPatio, 34, "Ninguno", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario, "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "");
                //Asignando Patio por Defecto
                ddlPatio.SelectedValue = up.id_patio.ToString();
                //Cargando los Accesos de ese Patio
                SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlAcceso, 35, "Ninguno", Convert.ToInt32(ddlPatio.SelectedValue), "", 0, "");
                //Asignando Acceso por Defecto
                ddlAcceso.SelectedValue = up.id_acceso_default.ToString();
            }
            //Cargando los Estados de Carga
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlEstado, "", 68);
            ddlEstado.Enabled = true;
            //Cargando los Tipos de Entidades
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipo, "", 67);
        }

        /// <summary>
        /// Inicializamo los indicadores relacionados con unidades dentro de patio
        /// </summary>
        public void inicializaIndicadoresUnidad()
        {
            //Inicializamos los indicadores de unidad en la pagina
            using (DataTable t = SAT_CL.ControlPatio.DetalleAccesoPatio.retornaIndicadoresUnidades(Convert.ToInt32(ddlPatio.SelectedValue)))
            {
                if (Validacion.ValidaOrigenDatos(t))
                    //Si el origen es valido recorremos la tabla 
                    foreach (DataRow r in t.Rows)
                    {
                        lblUnidades.Text = r["Unidades"].ToString();
                        lblTiempo.Text = r["Tiempo"].ToString();
                        lblEntradaSalida.Text = r["EntradasSalidas"].ToString();
                        lblCargadasxSalir.Text = r["CargadasxSalir"].ToString();
                    }
            }
        }

        #region Métodos Entrada

        /// <summary>
        /// Método Privado encargado de Limpiar los Controles
        /// </summary>
        private void limpiaControles()
        {   //Limpiando Valores
            txtTransportista.Text =
            txtIdentificador.Text =
            txtReferencia.Text = 
            txtDescripcion.Text = "";
            txtFecha.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");
            btnAgregar.CommandName = "Agregar";
        }
        /// <summary>
        /// Método Privado encargado de Configurar los Controles
        /// </summary>
        /// <param name="tipo">Tipo de Entidad</param>
        private void configuraControles(string tipo)
        {   //Validando el Tipo de Entidad
            switch (tipo)
            {  
                //Unidades que no Pueden tener Carga
                case "4":
                    {   //Configurando Controles
                        lblDescripcion.Text = "No. Economico";
                        lblIdentificador.Text = "Placas";
                        ddlEstado.Enabled = false;
                        ddlEstado.SelectedValue = "2";
                        break;
                    }
                //Unidades que Pueden tener Carga
                case "1":
                case "2":
                case "3":
                case "5":
                    {   //Configurando Controles
                        lblDescripcion.Text = "No. Economico";
                        lblIdentificador.Text = "Placas";
                        ddlEstado.Enabled = true;
                        ddlEstado.SelectedValue = "1";
                        break;
                    }
            }
        }
        /// <summary>
        /// Método Privado encargado de Guardar Acceso
        /// </summary>
        private void guardaAcceso()
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Validando que exista el Patio
            if (ddlPatio.SelectedValue != "0")
            {
                //Validando que exista el Acceso
                if (ddlAcceso.SelectedValue != "0")
                {
                    //Declarando Objeto de Fecha
                    DateTime fecha = DateTime.MinValue;
                    //Obteniendo Fecha
                    DateTime.TryParse(txtFecha.Text, out fecha);
                    //Declarando VariablesAcceso
                    int idAccesoEntrada = 0;
                    int idunidad = 0;
                    int idtransportista = 0;
                    //Validando que la Fecha sea Valida
                    if (fecha != DateTime.MinValue)
                    {
                        //Validando que existan Unidades Agrupadas
                        if (Validacion.ValidaOrigenDatos((DataSet)Session["DS"], "Table"))
                        {
                            //Inicializando Bloque Transaccional
                            using (TransactionScope trans = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                            {
                                //Recorriendo Filas
                                foreach (DataRow dr in ((DataSet)Session["DS"]).Tables["Table"].Rows)
                                {
                                    //Insertando Acceso
                                    result = AccesoPatio.InsertaAccesoPatio(Convert.ToInt32(ddlPatio.SelectedValue), Convert.ToInt32(ddlAcceso.SelectedValue),
                                                        AccesoPatio.TipoActualizacion.Web, AccesoPatio.TipoAcceso.Entrada, fecha, dr["Referencia"].ToString().ToUpper(), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                    //Validando que la Operación haya sido exitosa
                                    if (!result.OperacionExitosa)
                                        //Si no se inserto se termina el ciclo
                                        break;
                                    //Validando que se insertara el Acceso
                                    if (result.OperacionExitosa)
                                    {
                                        //Guardando Acceso
                                        idAccesoEntrada = result.IdRegistro;
                                        //Inserta Transportista Y Unidad
                                        result = InsertaTransportistaUnidadPatio(out idunidad, out idtransportista, Convert.ToInt32(dr["IdTransportista"]), dr["Transportista"].ToString().ToUpper(), Convert.ToInt32(ddlPatio.SelectedValue), Convert.ToInt32(dr["IdUnidad"]), dr["Unidad"].ToString().ToUpper(), dr["Identificador"].ToString().ToUpper(), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                        //Validando que la Operación haya sido exitosa
                                        if (!result.OperacionExitosa)
                                            //Si no se inserto se termina el ciclo
                                            break;
                                        if (result.OperacionExitosa)
                                        {
                                            //Insertando Detalle
                                            result = DetalleAccesoPatio.InsertaDetalleAccesoPatio(idAccesoEntrada, 0, idtransportista,
                                            0, 0, dr["Estado"].ToString() == "Cargado" ? true : false, fecha, Convert.ToByte(dr["IdTipo"]), dr["Unidad"].ToString().ToUpper(),
                                            dr["Identificador"].ToString().ToUpper(), idunidad, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                            //Validando que la Operación haya sido exitosa
                                            if (!result.OperacionExitosa)
                                                //Si no se inserto se termina el ciclo
                                                break;
                                            //Validando que se insertara el Acceso
                                            if (result.OperacionExitosa)

                                                //Completando Transacción
                                                trans.Complete();
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            //Validando que exista un Transportista
                            if (txtTransportista.Text != "")
                            {
                                //Validando que exista una Descripción
                                if (txtDescripcion.Text != "")
                                {
                                    //Declarando Variable Auxiliar
                                    byte id_tipo_detalle = 0;
                                    string tipo_detalle = "";
                                    //Validando el Comando del Control
                                    if (lnkMasUnidades.CommandName == "Todas")
                                    {   //Asignando Valores
                                        id_tipo_detalle = Convert.ToByte(ddlTipo.SelectedValue);
                                        tipo_detalle = ddlTipo.SelectedItem.Text;
                                    }
                                    else
                                    {   //Asignando Valores
                                        id_tipo_detalle = Convert.ToByte(rdbRemolque.Checked ? "1" : "2");
                                        tipo_detalle = rdbRemolque.Checked ? "Remolque" : "Rabon";
                                    }

                                    //Inicializando Bloque Transaccional
                                    using (TransactionScope trans = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                                    {
                                        //Insertando Acceso
                                        result = AccesoPatio.InsertaAccesoPatio(Convert.ToInt32(ddlPatio.SelectedValue), Convert.ToInt32(ddlAcceso.SelectedValue),
                                                                AccesoPatio.TipoActualizacion.Web, AccesoPatio.TipoAcceso.Entrada, fecha, txtReferencia.Text.ToUpper(), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                        //Validando que se insertara el Acceso
                                        if (result.OperacionExitosa)
                                        {
                                            //Guardando Acceso
                                            idAccesoEntrada = result.IdRegistro;
                                            //Inserta Transportista Y Unidad
                                            result = InsertaTransportistaUnidadPatio(out idunidad, out idtransportista, Convert.ToInt32(Cadena.VerificaCadenaVacia(Cadena.RegresaCadenaSeparada(txtTransportista.Text, "ID:", 1), "0")), Cadena.RegresaCadenaSeparada(txtTransportista.Text, "ID:", 0).ToString().ToUpper(), Convert.ToInt32(ddlPatio.SelectedValue), Convert.ToInt32(Cadena.VerificaCadenaVacia(Cadena.RegresaCadenaSeparada(txtDescripcion.Text, "ID:", 1), "0")), Cadena.RegresaCadenaSeparada(txtDescripcion.Text, "ID:", 0).ToString().ToUpper(), txtIdentificador.Text.ToUpper(), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                            //Insertando Detalle
                                            result = DetalleAccesoPatio.InsertaDetalleAccesoPatio(idAccesoEntrada, 0, idtransportista,
                                                    0, 0, ddlEstado.SelectedItem.Text == "Cargado" ? true : false, fecha, id_tipo_detalle, txtDescripcion.Text.ToUpper(),
                                                    txtIdentificador.Text.ToUpper(), idunidad, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                            //Validando que se insertara el Acceso
                                            if (result.OperacionExitosa)

                                                //Completando Transacción
                                                trans.Complete();
                                        }
                                    }
                                }
                                else//Instanciando Excepción
                                    result = new RetornoOperacion("** La Descripción es Requerido");
                            }
                            else//Instanciando Excepción
                                result = new RetornoOperacion("** El Transportista es Requerido");
                        }

                        //Validando la Operacion
                        if (result.OperacionExitosa)
                        {
                            //Inicializando GridView
                            TSDK.ASP.Controles.InicializaGridview(gvEntidades);

                            //Invocando Método de Limpieza
                            limpiaControles();
                            //txtTransportista.Text = "";

                            //Limpiando DataSet de Session
                            Session["DS"] = null;
                        }
                    }
                    else
                        //Instanciando Excepcion
                        result = new RetornoOperacion("** La Fecha no es Valida");
                }
                else
                    //Instanciando Excepcion
                    result = new RetornoOperacion("** Debe seleccionar un Acceso");
            }
            else
                //Instanciando Excepcion
                result = new RetornoOperacion("** Debe seleccionar un Patio");

            //Mostrando Mensaje
            lblErrorEntrada.Text = result.Mensaje;
        }

        #endregion

        #region Métodos Salida

        /// <summary>
        /// Método Privado encargado de Buscar los Accesos de Unidades Dentro
        /// </summary>
        private void buscaAccesos()
        {   //Obteniendo Reporte de Unidades Dentro
            using (DataTable dtUnidadesDentro = Reporte.ObtieneUnidadesDentro(txtBDescripcion.Text, txtBPlacas.Text, Convert.ToInt32(ddlPatio.SelectedValue)))
            {   //Validando que existan Registros
                if (Validacion.ValidaOrigenDatos(dtUnidadesDentro))
                {   //Validando que existan Tabla
                    if (Validacion.ValidaOrigenDatos((DataSet)Session["DS"], "Table3"))
                    {   //Recoriendo cada registro de la Busqueda
                        foreach (DataRow dr in dtUnidadesDentro.Rows)
                        {   //Recorriendo registros de la Tabla Temporal
                            foreach (DataRow reg in ((DataSet)Session["DS"]).Tables["Table3"].Rows)
                            {   //Validando que coincida el mismo Valor
                                if (dr["Id"].ToString() == reg["Id"].ToString())
                                {   //Eliminando Registro Coincidente
                                    dr.Delete();
                                    //Terminando Ciclo
                                    break;
                                }
                            }
                        }
                    }
                    //Cargando GridView
                    TSDK.ASP.Controles.CargaGridView(gvUnidades, dtUnidadesDentro, "Id-IdAccesoEntrada", "", true, 1);
                    //Añadiendo Tabla al DataSet de Session
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtUnidadesDentro, "Table1");
                }
                else
                {   //Inicializando GridView
                    TSDK.ASP.Controles.InicializaGridview(gvUnidades);
                    //Eliminando Tabla de Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
                }
            }
        }
        /// <summary>
        /// Método Privado encargado de Quitar los Registros que estan Agregados en la Tabla de Salidas
        /// </summary>
        private void quitaRegistrosAgregados()
        {   //Validando que existen registros 
            if (Validacion.ValidaOrigenDatos((DataSet)Session["DS"], "Table3"))
            {   //Recoriendo cada registro de la Busqueda
                foreach (DataRow dr in ((DataSet)Session["DS"]).Tables["Table2"].Rows)
                {   //Recorriendo registros de la Tabla Temporal
                    foreach (DataRow reg in ((DataSet)Session["DS"]).Tables["Table3"].Rows)
                    {   //Validando que coincida el mismo Valor
                        if (dr["Id"].ToString() == reg["Id"].ToString())
                        {   //Eliminando Registro Coincidente
                            dr.Delete();
                            //Terminando Ciclo
                            break;
                        }
                    }
                }
                //Confirmando Cambios
                ((DataSet)Session["DS"]).Tables["Table2"].AcceptChanges();
            }
        }
        /// <summary>
        /// Método Privado encargado de Guardar el Acceso de Salida
        /// </summary>
        private void guardaAccesoSalida()
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Validando que existen Registros
            if (Validacion.ValidaOrigenDatos((DataSet)Session["DS"], "Table3"))
            {
                //Obteniendo Fecha del Acceso
                DateTime fecha = DateTime.MinValue;
                DateTime.TryParse(txtFecha.Text, out fecha);

                //Validando que la Fecha sea Valida
                if (fecha != DateTime.MinValue)
                {
                    //Declarando Variable Auxiliar
                    bool fecha_valida = true;

                    //Iniciando Ciclo de Comparacion
                    foreach (DataRow dr in ((DataSet)Session["DS"]).Tables["Table3"].Rows)
                    {
                        //Instanciando Detalle
                        using (DetalleAccesoPatio dap = new DetalleAccesoPatio(Convert.ToInt32(dr["Id"])))
                        {
                            //Validando que exista el Detalle
                            if (dap.id_detalle_acceso_patio > 0)
                            {
                                //Instanciando Acceso de Entrada
                                using (AccesoPatio ace = new AccesoPatio(dap.id_acceso_entrada))
                                {
                                    //Validando que exista el 
                                    if (ace.id_acceso_patio > 0)
                                        //Obteniendo Resultado de la Comparación
                                        fecha_valida = DateTime.Compare(ace.fecha_acceso, fecha) < 0 ? true : false;
                                    else
                                        //Asignando Resultado a Variable
                                        fecha_valida = false;
                                }
                            }
                            else
                                //Asignando Resultado a Variable
                                fecha_valida = false;
                        }
                        //Validando que la fecha sea Valida
                        if (!fecha_valida)
                            break;
                    }

                    //Validando que la fecha sea Valida
                    if (fecha_valida)
                    {
                        //Inicializando Transacción
                        using (TransactionScope trans = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                        {
                            //Creando Acceso de Salida
                            result = AccesoPatio.InsertaAccesoPatio(Convert.ToInt32(ddlPatio.SelectedValue), Convert.ToInt32(ddlAcceso.SelectedValue),
                                            AccesoPatio.TipoActualizacion.Web, AccesoPatio.TipoAcceso.Salida, fecha, "",
                                            ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                            //Validando la Operacion
                            if (result.OperacionExitosa)
                            {
                                //Obteniendo Acceso de Salida
                                int idAccesoSalida = result.IdRegistro;

                                //Iniciando Ciclo de Comparacion
                                foreach (DataRow dr in ((DataSet)Session["DS"]).Tables["Table3"].Rows)
                                {
                                    //Instanciando Detalle
                                    using (DetalleAccesoPatio dap = new DetalleAccesoPatio(Convert.ToInt32(dr["Id"])))
                                    {
                                        //Validando si el Registro se Instancio correctamente
                                        if (dap.id_detalle_acceso_patio > 0)
                                        {
                                            //Actualizando Acceso de Salida
                                            result = dap.ActualizaSalidaDetalle(idAccesoSalida, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                            //En case de que la Operacion no haya sido exitosa
                                            if (!(result.OperacionExitosa))
                                                //Termina Ciclo
                                                break;
                                        }
                                    }
                                }

                                //Validando la Operacion
                                if (result.OperacionExitosa)
                                    //Completando Transacción
                                    trans.Complete();

                            }
                        }

                        //Validando si fueron Exitosas las Operaciónes
                        if (result.OperacionExitosa)
                        {
                            //Eliminando Tabla de Session
                            Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table3");
                            Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table2");

                            //Inicializando GridView
                            TSDK.ASP.Controles.InicializaGridview(gvUnidadesTemp);
                            TSDK.ASP.Controles.InicializaGridview(gvSalidas);

                            //Invocando Método de Busqueda
                            buscaAccesos();
                        }
                    }
                    else
                        //Instanciando Excepcion
                        result = new RetornoOperacion("** La Fecha de Salida debe ser mayor que la de Entrada");
                }
                else
                    //Instanciando Excepcion
                    result = new RetornoOperacion("** La Fecha no es Valida");

                //Mostrando Mensaje de la Operación
                lblErrorAsignaSalida.Text = result.Mensaje;

                //Actualizamos los indicadores de la forma
                inicializaIndicadoresUnidad();
            }
        }
        /// <summary>
        /// Método Privado encargado de insertar transportista y unidad
        /// </summary>
        private static RetornoOperacion InsertaTransportistaUnidadPatio(out int id_unidad_o, out int id_transportista_o, int id_transportista, string transportista, int tipo_patio, int id_unidad, string unidad,string identificador, int id_usuario)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            id_unidad_o = id_transportista_o = 0;
            //Creamos la transacción 
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Insertar Transportita 
                if (id_transportista > 0)
                {
                    id_transportista_o = id_transportista;
                    result = new RetornoOperacion(true);
                }
                else
                {
                    result = SAT_CL.ControlPatio.PatioTransportista.InsertarPatioTransportista(transportista.ToUpper(), tipo_patio, id_usuario);
                    id_transportista_o = result.IdRegistro;
                }
                //Validamos operacion exitosa
                if (result.OperacionExitosa)
                {
                    if (id_unidad > 0)
                    {
                        id_unidad_o = id_unidad;
                        result = new RetornoOperacion(true);
                    }

                    else
                    {
                        result = SAT_CL.ControlPatio.UnidadPatio.InsertaUnidadPatio(unidad.ToUpper(), identificador.ToUpper(), "", id_transportista > 0 ? id_transportista : result.IdRegistro, id_usuario, true);
                        id_unidad_o = result.IdRegistro;
                    }

                }
                //Validamos Resultado
                if (result.OperacionExitosa)
                {
                    //Finalizamos Transacción
                    scope.Complete();
                }
            }
            //Devolviendo Resultado Obtenido
            return result;
        }


        #endregion

        #endregion
    }
}