using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;


namespace SAT.Mantenimiento
{
    public partial class VisorOrdenTrabajo : System.Web.UI.Page
    {
        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                inicializaPagina();
        }
        protected void btnBuscarOrdenTrabajo_Click(object sender, EventArgs e)
        {
            //Invoca al método que realiza la busqueda de ordenes de trabajo
            buscarOrdenTrabajo();
        }
        /// <summary>
        /// Evento que carga catalogos a partir de un tipo de taller (Interno, Externo)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTipoTaller_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Validando Tipo de Taller
            switch (ddlTipoTaller.SelectedValue)
            {
                //Si el tipo taller es propio.
                case "1":
                    {
                        //Deshabilita y limpia los Controles.
                        txtProveedor.Enabled = false;
                        txtTaller.Text = "";
                        txtProveedor.Text = "";
                        break;
                    }
                //Si el tipo taller es aseguradora.
                case "2":
                    {
                        //Habilita y limpia los Controles.
                        txtProveedor.Enabled = true;
                        txtTaller.Text = "";
                        txtProveedor.Text = "";
                        break;
                    }
                //Si el tpo taller es externo
                case "3":
                    {
                        //Habilita, deshabilita y limpia los Controles.
                        txtTaller.Text = "";
                        txtProveedor.Enabled = true;
                        txtProveedor.Text = "";
                        break;
                    }
            }
            //Invoca al método cargaTalleresInternoExternos().
            cargaTalleresInternoExternos();
        }
        /// <summary>
        /// Evento que habilita o deshabilita los controles de la forma a partir de su estado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkBitUnidadExt_CheckedChanged(object sender, EventArgs e)
        {
            //Validando el Valor del Control
            switch (chkBitUnidadExt.Checked)
            {
                //Si el check es activado
                case true:
                    {
                        //Habilitando Controles
                        txtCliente.Enabled =
                        txtDescUnidad.Enabled = true;
                        //Deshabilitando Control
                        txtUnidad.Enabled = false;

                        //Asignando Valores
                        txtDescUnidad.Text = "";
                        txtUnidad.Text = "";
                        break;
                    }
                //Si el check esta desactivado
                case false:
                    {
                        //Deshabilitando Controles
                        txtCliente.Enabled =
                        txtDescUnidad.Enabled = false;
                        //Habilitando Control
                        txtUnidad.Enabled = true;
                        break;
                    }

            }
        }
        #region Eventos GridView
        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView "Orden de Trabajo"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamano_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando tamaño del GridView
            Controles.CambiaTamañoPaginaGridView(gvOrdenTrabajo, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamano.SelectedValue));
            calculaTotalesOrdenTrabajo();
        }
        /// <summary>
        /// Evento Producido al Exportar al Contenido del GridView "Orden de Trabajo"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportar_Click(object sender, EventArgs e)
        {
            //Exportando Contenido del GridView
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "Id");
        }
        /// <summary>
        /// Evento Producido al Cambiar la Expresión del Ordenamiento del GridView "Orden de Trabajo"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvOrdenTrabajo_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión del Ordenamiento
            lblOrdenar.Text = Controles.CambiaSortExpressionGridView(gvOrdenTrabajo, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression);
            calculaTotalesOrdenTrabajo();
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Página del GridView "Orden de Trabajo"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvOrdenTrabajo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            Controles.CambiaIndicePaginaGridView(gvOrdenTrabajo, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex);
            calculaTotalesOrdenTrabajo();
        }
        /// <summary>
        /// Evento que muestra la ventana modal con losproductos asociados a la orden de compra
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkModales_Click(object sender, EventArgs e)
        {
            //Selecciona la fila del elemento al cual se va a consultar los productos
            Controles.SeleccionaFila(gvOrdenTrabajo, sender, "lnk", false);
            //Obtiene el control que activa el evento
            LinkButton lnk = (LinkButton)sender;
            //Valida cada llamado
            switch(lnk.CommandName)
            {
                case "ActividadFalla":
                    {
                        //Invoca al método
                        cargaActidadFallas();
                        //Abre ventana modal
                        ScriptServer.AlternarVentana(lnk, "AbrirVentana", "contenedorProducto", "Producto");
                        break;
                    }
                case "Asignado":
                    {
                        //Invoca al método asignacionesOrdenTrabajo
                        cargaAsignaciones();
                        ScriptServer.AlternarVentana(lnk, "AbrirVentana", "contenedorProducto", "Producto");
                        break;
                    }
                case "Producto":
                    {
                        //Invoca al método 
                        cargaProductoOrdenTrabajo();
                        //Abre ventana modal
                        ScriptServer.AlternarVentana(lnk, "AbrirVentana", "contenedorProducto", "Producto");
                        break;
                    }
            }
        }
        /// <summary>
        /// Evento que cierra la ventana modal.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkCerrar_Click(object sender, EventArgs e)
        {
            ScriptServer.AlternarVentana(uplnkCerrar, uplnkCerrar.GetType(), "CerrarVentana", "contenedorProducto", "Producto");
        }

        #endregion
        #endregion
        #region Métodos
        /// <summary>
        /// Método que inicializa los valores de la página
        /// </summary>
        private void inicializaPagina()
        {
            //Inicializando Fechas
            txtFecIni.Text = Fecha.ObtieneFechaEstandarMexicoCentro().AddDays(-7).ToString("dd/MM/yyyy HH:mm");
            txtFecFin.Text = Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");
            //Inicializando GridView
            Controles.InicializaGridview(gvOrdenTrabajo);

            //Cargando Tamaño
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamano, "", 26);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlEstatusOrden, "TODOS", 1119);
            //Tipo de Taller
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipoTaller, "TODOS", 1123);
            //Validando el estado del checkbox Solicitud (Activado)
            switch (chkBitUnidadExt.Checked)
            {
                //Si el check es activado
                case true:
                    {
                        //Habilitando Controles
                        txtCliente.Enabled =
                        txtDescUnidad.Enabled = true;
                        //Deshabilitando Control
                        txtUnidad.Enabled = false;

                        //Asignando Valores
                        txtDescUnidad.Text = "";
                        txtUnidad.Text = "";
                        break;
                    }
                //Si el check esta desactivado
                case false:
                    {
                        //Deshabilitando Controles
                        txtCliente.Enabled =
                        txtDescUnidad.Enabled = false;
                        //Habilitando Control
                        txtUnidad.Enabled = true;
                        break;
                    }

            }
        }
        /// <summary>
        /// Métdo que realiza la busqueda de ordenes de trabajo
        /// </summary>
        private void buscarOrdenTrabajo()
        {
            //Variables Auxiliares que almacena la fecha minima 
            DateTime fec_ini_sol, fec_fin_sol, fec_ini_ent, fec_fin_ent;
            fec_ini_sol = fec_fin_sol = fec_ini_ent = fec_fin_ent = DateTime.MinValue;

            //Validando el estado del checkbox Solicitud (Activado)
            if (chkSolicitud.Checked)
            {
                //Asigna Fechas seleccionadas o asigna una fecha default
                DateTime.TryParse(txtFecIni.Text, out fec_ini_sol);
                DateTime.TryParse(txtFecFin.Text, out fec_fin_sol);
            }
            //Validando el estado del checkbox Entrega (Activado)
            if (chkEntrega.Checked)
            {
                //Asigna Fechas seleccionadas o asigna una fecha default
                DateTime.TryParse(txtFecIni.Text, out fec_ini_ent);
                DateTime.TryParse(txtFecFin.Text, out fec_fin_ent);
            }
            //Instancia a la clase reporte 
            using (DataTable dtOrdenTrabajo = SAT_CL.Mantenimiento.Reportes.OrdenTrabajo((((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor),
                                                                                           Convert.ToInt32(Cadena.VerificaCadenaVacia(txtOrdenTrabajo.Text,"0")),Convert.ToByte(ddlEstatusOrden.SelectedValue),
                                                                                           Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtCliente.Text, "ID:", 1)),Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtProveedor.Text,"ID:",1)),
                                                                                           Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtEmpleado.Text,"ID:",1)),fec_ini_sol,fec_fin_sol,fec_ini_ent,fec_fin_ent,
                                                                                           Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtUnidad.Text, "ID:", 1)), txtDescUnidad.Text, Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtTaller.Text, "ID:", 1))))                                                                                            
            {
                //Valida los registros
                if (Validacion.ValidaOrigenDatos(dtOrdenTrabajo))
                {
                    //Carga los datos al GridView
                    Controles.CargaGridView(gvOrdenTrabajo, dtOrdenTrabajo, "Id", lblOrdenar.Text, true, 2);
                    //Añade el resultado de la tabla a la variable de sessión
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtOrdenTrabajo, "Table");                    
                }
                else
                {
                    //Inicializa el gridView
                    Controles.InicializaGridview(gvOrdenTrabajo);
                    //Elimina los datos almacenados en la variable session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                }
            }
            Controles.InicializaIndices(gvOrdenTrabajo);
            calculaTotalesOrdenTrabajo();
        }
        /// <summary>
        /// Método encargado de Cargar la Configuración de los Talleres
        /// </summary>
        private void cargaTalleresInternoExternos()
        {
            //Obteniendo Compania
            string id_compania = ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString();

            //Declarando Script de Configuración
            string script = @"<script type='text/javascript'>
                                //Obteniendo Tipo de Taller (Externo/Interno)
                                var tipoTaller = " + ddlTipoTaller.SelectedValue + @";
                                
                                //Evento Change
                                $('#" + ddlTipoTaller.ClientID + @"').change(function () {
                                    
                                    //Limpiando Control
                                    $('#" + txtTaller.ClientID + @"').val('');

                                    //Invocando Funcion
                                    CargaAutocompleta();
                                });
                                
                                //Declarando Función de Autocompleta
                                function CargaAutocompleta(){
                                    
                                    //Validando Tipo de Entidad
                                    switch (tipoTaller) {
                                        case 0:
                                            {   
                                                //alert('Propio');
                                                //Cargando Catalogo de Talleres Internos
                                                $('#" + txtTaller.ClientID + @"').autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=47&param=" + id_compania + @"'});
                                                break;
                                            }
                                        case 1:
                                            {   
                                                //alert('Propio');
                                                //Cargando Catalogo de Talleres Internos
                                                $('#" + txtTaller.ClientID + @"').autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=40&param=" + id_compania + @"&param2=1'});
                                                break;
                                            }
                                        case 2:
                                        case 3:
                                            {   
                                                //alert('Externo');
                                                //Cargando Catalogo de Talleres Externos
                                                $('#" + txtTaller.ClientID + @"').autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=40&param=" + id_compania + @"&param2=2'});
                                                break;
                                            }
                                        default:
                                            {   
                                                //alert('Default');
                                                //Cargando Catalogo de Talleres Internos
                                                $('#" + txtTaller.ClientID + @"').autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=40&param=" + id_compania + @"&param2=1'});
                                                break;
                                            }
                                    }
                                }
                                
                                //Invocando Funcion
                                CargaAutocompleta();
                                
                              </script>";

            //Registrando Script
            ScriptManager.RegisterStartupScript(this, this.GetType(), "CargaAutocompletaTaller", script, false);
        }
        /// <summary>
        /// Método que muestra el producto asignado a una orden de trabajo
        /// </summary>
        private void cargaProductoOrdenTrabajo()
        {
            //Limpia las columnas del gridview
            gvGenerico.Columns.Clear();
            BoundField actividad = new BoundField();
            actividad.HeaderText = "Actividad";
            actividad.DataField = "Actividad";
            actividad.SortExpression = "Actividad";
            gvGenerico.Columns.Add(actividad);

            BoundField estatus = new BoundField();
            estatus.HeaderText = "Estatus";
            estatus.DataField = "Estatus";
            estatus.SortExpression = "Estatus";
            gvGenerico.Columns.Add(estatus);

            BoundField codigo = new BoundField();
            codigo.HeaderText = "Código Producto";
            codigo.DataField = "CodProducto";
            codigo.SortExpression = "CodProducto";
            gvGenerico.Columns.Add(codigo);

            BoundField producto = new BoundField();
            producto.HeaderText = "Producto";
            producto.DataField = "Producto";
            producto.SortExpression = "Producto";
            gvGenerico.Columns.Add(producto);

            BoundField unidad = new BoundField();
            unidad.HeaderText = "Unidad Medida";
            unidad.DataField = "UnidadMedida";
            unidad.SortExpression = "UnidadMedida";
            gvGenerico.Columns.Add(unidad);


            BoundField precio = new BoundField();
            precio.HeaderText = "Precio";
            precio.DataField = "Precio";
            precio.SortExpression = "Precio"; 
            precio.DataFormatString = "{0:C2}";
            precio.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
            gvGenerico.Columns.Add(precio);

            BoundField cantidad = new BoundField();
            cantidad.HeaderText = "Cantidad Producto";
            cantidad.DataField = "Cantidad";
            cantidad.SortExpression = "Cantidad";
            cantidad.DataFormatString = "{0:0}";
            cantidad.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
            cantidad.FooterStyle.HorizontalAlign = HorizontalAlign.Right;
            gvGenerico.Columns.Add(cantidad); 

            BoundField total = new BoundField();
            total.HeaderText = "Total";
            total.DataField = "Total";
            total.SortExpression = "Total";
            total.DataFormatString = "{0:C2}";
            total.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
            total.FooterStyle.HorizontalAlign = HorizontalAlign.Right;
            gvGenerico.Columns.Add(total);

            //Invoca al método que obtiene los productos de una orden de trabajo
            using(DataTable dtProducto = SAT_CL.Mantenimiento.OrdenTrabajo.CargaProductoOrdenTrabajo((int)gvOrdenTrabajo.SelectedDataKey["Id"]))
            {
                //Valida los datos del datatable
                if(Validacion.ValidaOrigenDatos(dtProducto))
                {
                    //Asigna al grid view los valores encontrados
                    Controles.CargaGridView(gvGenerico,dtProducto,"","");
                    //Asigna valores a la variable de session
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"],dtProducto,"Table1");
                    gvGenerico.FooterRow.Cells[6].Text = string.Format("{0:0}", (dtProducto.Compute("SUM(Cantidad)", "")));
                    gvGenerico.FooterRow.Cells[7].Text = string.Format("{0:C2}", (dtProducto.Compute("SUM(Total)", "")));
                }
                else
                {
                    //Si no existen registros inicializa el GridView
                    Controles.InicializaGridview(gvGenerico);
                    //Elimina los datos del dataset
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"],"Table1");
                    gvGenerico.FooterRow.Cells[6].Text = string.Format("{0:0}", 0);
                    gvGenerico.FooterRow.Cells[7].Text = string.Format("{0:C2}", 0);
                }
            }
            Controles.InicializaIndices(gvGenerico);
            lblEncabezado.Text = "PRODUCTO";
        }
        /// <summary>
        /// Método que muestra las actividades y Fallas de una orden de trabajo
        /// </summary>
        private void cargaActidadFallas()
        {
            //Limpia las columnas del gridview
            gvGenerico.Columns.Clear();
          
            BoundField falla = new BoundField();
            falla.HeaderText = "Falla";
            falla.DataField = "Falla";
            falla.SortExpression = "Falla";
            gvGenerico.Columns.Add(falla);

            BoundField fechafalla = new BoundField();
            fechafalla.HeaderText = "Fecha Falla";
            fechafalla.DataField = "FechaFalla";
            fechafalla.SortExpression = "FechaFalla";
            fechafalla.DataFormatString = "{0:dd/MM/yyyy HH:mm}";
            fechafalla.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
            gvGenerico.Columns.Add(fechafalla);

            BoundField estatus = new BoundField();
            estatus.HeaderText = "Estatus Actividad";
            estatus.DataField = "Estatus";
            estatus.SortExpression = "Estatus";
            gvGenerico.Columns.Add(estatus);

            BoundField actividad = new BoundField();
            actividad.HeaderText = "Actividad";
            actividad.DataField = "Actividad";
            actividad.SortExpression = "Actividad";
            gvGenerico.Columns.Add(actividad);

            BoundField fechainicio = new BoundField();
            fechainicio.HeaderText = "Fecha Inicio";
            fechainicio.DataField = "FechaInicio";
            fechainicio.SortExpression = "FechaInicio";
            fechainicio.DataFormatString = "{0:dd/MM/yyyy HH:mm}";
            fechainicio.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
            gvGenerico.Columns.Add(fechainicio);

            BoundField fechafin = new BoundField();
            fechafin.HeaderText = "Fecha Fin";
            fechafin.DataField = "FechaFin";
            fechafin.SortExpression = "FechaFin";
            fechafin.DataFormatString = "{0:dd/MM/yyyy HH:mm}";
            fechafin.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
            gvGenerico.Columns.Add(fechafin);

            BoundField duracion = new BoundField();
            duracion.HeaderText = "Duración Aprox.";
            duracion.DataField = "Duracion";
            duracion.SortExpression = "Duracion";            
            duracion.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
            gvGenerico.Columns.Add(duracion);

            BoundField duracionreal = new BoundField();
            duracionreal.HeaderText = "Duración Real";
            duracionreal.DataField = "DuracionReal";
            duracionreal.SortExpression = "DuracionReal";
            duracionreal.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
            gvGenerico.Columns.Add(duracionreal);
            //Instancia a la clase para obtener las activiades y fallas de la orden de trabajo
            using (DataTable dtActividadFalla = SAT_CL.Mantenimiento.OrdenTrabajo.CargaActividadesOrdenTrabajo((int)gvOrdenTrabajo.SelectedDataKey["Id"]))
            {
                //Valida los datos del datattable
                if (Validacion.ValidaOrigenDatos(dtActividadFalla))
                {
                    //Asigna al gridview los valores del datatable
                    Controles.CargaGridView(gvGenerico, dtActividadFalla, "", "");
                    //Asigna valores a la variable de session
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtActividadFalla, "Table2");                    
                }
                else
                {
                    //inicializa el gridView
                    Controles.InicializaGridview(gvGenerico);
                    //Elimina los datos del datase (session)
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table2");
                }
            }
            Controles.InicializaIndices(gvGenerico);
            lblEncabezado.Text = "ACTIVIDADES Y FALLAS";
        }
        /// <summary>
        /// Método que mustra las asignaciones de recurso a una orden de trabajo
        /// </summary>
        private void cargaAsignaciones()
        {
            //Limpia las columnas del gridview
            gvGenerico.Columns.Clear();

            BoundField actividad = new BoundField();
            actividad.HeaderText = "Actividad";
            actividad.DataField = "Actividad";
            actividad.SortExpression = "Actividad";
            gvGenerico.Columns.Add(actividad);

            BoundField asignado = new BoundField();
            asignado.HeaderText = "Asignado a";
            asignado.DataField = "Asignado";
            asignado.SortExpression = "Asignado";
            gvGenerico.Columns.Add(asignado);

            BoundField puesto = new BoundField();
            puesto.HeaderText = "Puesto";
            puesto.DataField = "Puesto";
            puesto.SortExpression = "Puesto";
            gvGenerico.Columns.Add(puesto);

            BoundField duracion = new BoundField();
            duracion.HeaderText = "Duracion Actividad";
            duracion.DataField = "Duracion";
            duracion.SortExpression = "Duracion";
            duracion.DataFormatString = "{0:HH:mm}";
            duracion.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
            gvGenerico.Columns.Add(duracion);

            BoundField CostoHora = new BoundField();
            CostoHora.HeaderText = "Costo x Hora";
            CostoHora.DataField = "CostoHora";
            CostoHora.SortExpression = "CostoHora";
            CostoHora.DataFormatString = "{0:C2}";
            CostoHora.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
            gvGenerico.Columns.Add(CostoHora);

            BoundField mano = new BoundField();
            mano.HeaderText = "ManoObra";
            mano.DataField = "ManoObra";
            mano.SortExpression = "ManoObra";
            mano.DataFormatString = "{0:C2}";
            mano.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
            mano.FooterStyle.HorizontalAlign = HorizontalAlign.Right;
            gvGenerico.Columns.Add(mano);

            //Instancia a la clase para obtener a quien se le asigno cada actividad de la orden de trabajo
            using (DataTable dtAsignaciones = SAT_CL.Mantenimiento.OrdenTrabajo.CargaAsignacionesOrdenTrabajo((int)gvOrdenTrabajo.SelectedDataKey["Id"]))
            {
                //Valida los datos del datatable
                if (Validacion.ValidaOrigenDatos(dtAsignaciones))
                {
                    //Asigna valores algridView
                    Controles.CargaGridView(gvGenerico, dtAsignaciones, "", "");
                    //Asigna valores a la variable de session
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtAsignaciones, "Table3");
                    gvGenerico.FooterRow.Cells[5].Text = string.Format("{0:C2}", (dtAsignaciones.Compute("SUM(ManoObra)", "")));
                }
                else
                {
                    //Inicializa el gridView
                    Controles.InicializaGridview(gvGenerico);
                    //Elimina los datos almacenados en session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table3");
                    gvGenerico.FooterRow.Cells[5].Text = string.Format("{0:C2}", 0);
                }
            }
            Controles.InicializaIndices(gvGenerico);
            lblEncabezado.Text = "ASIGNACION DE PERSONAL";
        }

        private void calculaTotalesOrdenTrabajo()
        {
            using (DataTable dtOrdenTrabajo = OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"))
            {
                if (Validacion.ValidaOrigenDatos(dtOrdenTrabajo))
                {
                    gvOrdenTrabajo.FooterRow.Cells[10].Text = string.Format("{0}", (dtOrdenTrabajo.Compute("SUM(Odometro)", "")));
                    gvOrdenTrabajo.FooterRow.Cells[11].Text = string.Format("{0}", (dtOrdenTrabajo.Compute("SUM(Combustible)", "")));
                    gvOrdenTrabajo.FooterRow.Cells[15].Text = string.Format("{0}", (dtOrdenTrabajo.Compute("SUM(ActividadFalla)", "")));
                    gvOrdenTrabajo.FooterRow.Cells[16].Text = string.Format("{0}", (dtOrdenTrabajo.Compute("SUM(Asignado)", "")));
                    gvOrdenTrabajo.FooterRow.Cells[17].Text = string.Format("{0}",(dtOrdenTrabajo.Compute("SUM(Producto)","")));
                }
                else
                {
                    gvOrdenTrabajo.FooterRow.Cells[10].Text = string.Format("{0}", 0);
                    gvOrdenTrabajo.FooterRow.Cells[11].Text = string.Format("{0}", 0);
                    gvOrdenTrabajo.FooterRow.Cells[15].Text = string.Format("{0}", 0);
                    gvOrdenTrabajo.FooterRow.Cells[16].Text = string.Format("{0}", 0);
                    gvOrdenTrabajo.FooterRow.Cells[17].Text = string.Format("{0}", 0);
                }
            }
        }
        #endregion
    }
}