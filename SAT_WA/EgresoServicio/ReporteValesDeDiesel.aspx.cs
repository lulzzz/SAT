using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Datos;
using TSDK.Base;
using System.Transactions;

namespace SAT.EgresoServicio
{
    public partial class ReporteValesDeDiesel : System.Web.UI.Page 
    {
        #region Eventos

        /// <summary>
        /// Evento Producido al Efectuarse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Aplicando seguridad de la forma
            SAT_CL.Seguridad.Forma.AplicaSeguridadForma(this, "content1", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
            //Validando que se haya Producido un PostBack
            if (!Page.IsPostBack)

                //Invocando Inicialización de Página
                inicializaPagina();

            //Invocando Método de Carga
            cargaConfiguracionScript();
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Buscar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            //Buscando vales coincidentes con filtros
            buscaValesDiesel();
        }
        /// <summary>
        /// Evento que permite visualizar la bitacora de un registro
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbBitacora_Click(object sender, EventArgs e)
        {
            //Valida que existan registros en el gridview 
            if (gvValesDiesel.DataKeys.Count > 0)
            {
                //Selecciona el registro del gridview al cual se realizara la consulta de bitacora
                Controles.SeleccionaFila(gvValesDiesel, sender, "lnk", false);
                //Invoca al método inicializaBitacora
                inicicalizaBitacora(gvValesDiesel.SelectedDataKey["Id"].ToString(), "69", "Bitacora Asignación Diesel");
            }
        }
        #endregion

        #region Métodos

        /// <summary>
        /// Método encargado de Inicializar la Página
        /// </summary>
        private void inicializaPagina()
        {
            //Cargando Catalogos
            cargaCatalogos();

            //Inicializando GridView
            TSDK.ASP.Controles.InicializaGridview(gvValesDiesel);

            //Inicializando Fechas
            txtFecIni.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().AddMonths(-1).ToString("dd/MM/yyyy HH:mm");
            txtFecFin.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");
        }
        /// <summary>
        /// Método encargado de Cargar los Catalogos
        /// </summary>
        private void cargaCatalogos()
        {
            //Cargando Tipo Entidad
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipoEnt, "-- Todos", 62);
            ListItem li = new ListItem("Proveedor", "3");
            ddlTipoEnt.Items.Remove(li);
            //Cargando Catalogos de Tamaño del GridView
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamano, "", 26);
            //Estacion Combustible
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlUbicacion, 20, "-- Todas", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, "");
            //Cargando Estatus
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlEstatus, "-- Todos", 47);
        }
        /// <summary>
        /// Método Privado encargado de Cargar el Catalogo Autocompleta
        /// </summary>
        private void cargaConfiguracionScript()
        {
            //Obteniendo Compania
            string id_compania = ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString();

            //Declarando Script
            string script = @"<script type='text/javascript'>
                                //Obteniendo Tipo de Entidad
                                var tipoEntidad = " + ddlTipoEnt.SelectedValue + @";
                                
                                //Evento Change
                                $('#" + ddlTipoEnt.ClientID + @"').change(function () {
                                    
                                    //Limpiando Control
                                    $('#" + txtEntidad.ClientID + @"').val('');

                                    //Invocando Funcion
                                    CargaAutocompleta();
                                });
                                
                                //Declarando Función de Autocompleta
                                function CargaAutocompleta(){
                                    //Validando Tipo de Entidad
                                    switch (tipoEntidad) {
                                        case 1:
                                            {   
                                                //Cargando Catalogo de Unidades
                                                $('#" + txtEntidad.ClientID + @"').autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=12&param=" + id_compania + @"'});
                                                break;
                                            }
                                        case 2:
                                            {   
                                                //Cargando Catalogo de Operadores
                                                $('#" + txtEntidad.ClientID + @"').autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=11&param=" + id_compania + @"'});
                                                break;
                                            }
                                        case 3:
                                            {   
                                                //Cargando Catalogo de Proveedores
                                                $('#" + txtEntidad.ClientID + @"').autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=16&param=" + id_compania + @"'});
                                                break;
                                            }
                                    }
                                }
                                
                                //Invocando Funcion
                                CargaAutocompleta();
                            </script>";

            //Registrando Script
            ScriptManager.RegisterStartupScript(this, this.GetType(), "CargaAutocompletaEntidad", script, false);
        }
        /// <summary>
        /// Realiza la búsqueda de vales de diesel
        /// </summary>
        private void buscaValesDiesel()
        {
            //Declarando Variables Auxiliares
            DateTime fec_ini_sol = DateTime.MinValue;
            DateTime fec_fin_sol = DateTime.MinValue;
            DateTime fec_ini_car = DateTime.MinValue;
            DateTime fec_fin_car = DateTime.MinValue;
            int id_unidad = 0;
            int id_operador = 0;
            int id_proveedor = 0;
            string id_servicio = "";
            string complemento = "";
            //Declarando variables para él filtrado de vales de diesel
            byte tipoAsignacion = Convert.ToByte(ddlTipoEnt.SelectedValue);
            ////Obteniendo Entidad
            //id_unidad = ddlTipoEnt.SelectedValue == "1" ? Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtEntidad.Text, "ID:", 1, "0")) : 0;
            //id_operador = ddlTipoEnt.SelectedValue == "2" ? Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtEntidad.Text, "ID:", 1, "0")) : 0;
            //id_proveedor = ddlTipoEnt.SelectedValue == "3" ? Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtEntidad.Text, "ID:", 1, "0")) : 0;

            switch (tipoAsignacion)
            {
                //CUANDO ES UNA UNIDAD
                case 1:
                    {
                        //ENVIA 
                        complemento = TSDK.Base.Cadena.RegresaCadenaSeparada(txtEntidad.Text, "ID:", 0, "0");
                       // id_unidad = 0;
                        break;
                    }
                //CUANDO ES UN OPERADOR
                case 2:
                    {

                        id_operador = Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtEntidad.Text, "ID:", 1, "0"));
                        complemento = "";
                        break;
                    }
                //CUANDO ES UN PERMISIONARIO
                case 3:
                    {

                        id_proveedor = Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtEntidad.Text, "ID:", 1, "0"));
                        complemento = "";
                        break;
                    }
            }


            //Validando si se Requieren las Fechas
            if (chkIncluir.Checked)
            {
                //Validando el Tipo de Fecha Requerida
                if (rbCarga.Checked)
                {
                    //Obteniendo Fechas
                    DateTime.TryParse(txtFecIni.Text, out fec_ini_car);
                    DateTime.TryParse(txtFecFin.Text, out fec_fin_car);
                }
                else if (rbSolicitud.Checked)
                {
                    //Obteniendo Fechas
                    DateTime.TryParse(txtFecIni.Text, out fec_ini_sol);
                    DateTime.TryParse(txtFecFin.Text, out fec_fin_sol);
                }
            }

            //Inicializando indices de selección
            Controles.InicializaIndices(gvValesDiesel);

            //Obteniendo Reporte de Saldos Globales
            using (DataTable dtValesDiesel = SAT_CL.EgresoServicio.Reportes.ReporteValesDiesel(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                        Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCliente.Text, "ID:", 1, "0")), txtNoVale.Text, txtNoServicio.Text, fec_ini_sol, fec_fin_sol,
                        fec_ini_car, fec_fin_car, Convert.ToInt32(ddlUbicacion.SelectedValue), id_unidad, id_operador, id_proveedor, Convert.ToByte(ddlEstatus.SelectedValue),complemento, txtUnidadDiesel.Text, id_servicio))
            {
                //Cargando GridView
                Controles.CargaGridView(gvValesDiesel, dtValesDiesel, "NoVale-Id-Unidad", "", true, 0);

                //Validando que existan Registros
                if (dtValesDiesel != null)
                    //Añadiendo Tabla a Session
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtValesDiesel, "Table");
                else
                    //Eliminando Tabla de Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
            }

            //Sumando Totales
            sumaTotales();
        }
        /// <summary>
        /// Método encargado de Sumar los Totales del Reporte
        /// </summary>
        private void sumaTotales()
        {
            //Validando que Exista la Tabla
            if (TSDK.Datos.Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table")))
            {
                //Mostrando Totales
                gvValesDiesel.FooterRow.Cells[19].Text = string.Format("{0}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Litros)", "")));
                gvValesDiesel.FooterRow.Cells[20].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(MontoTotal)", "")));                
            }
            else
            {
                //Mostrando Totales en Cero                
                gvValesDiesel.FooterRow.Cells[19].Text = string.Format("{0}", 0);
                gvValesDiesel.FooterRow.Cells[20].Text = string.Format("{0:C2}", 0);
            }
        }
        /// <summary>
        /// Expande o contrae una ventana modal
        /// </summary>
        /// <param name="control">Control que produce la alternancia</param>
        /// <param name="comando">Comando de registro del script</param>
        private void alternaVentanaModal(Control control, string comando)
        { 
            //Determinando que ventana será alternada
            switch (comando)
            {
                case "Diesel":
                    ScriptServer.AlternarVentana(control, comando, "asignacionDieselModal", "asignacionDiesel");
                    break;
                case "Referencias":
                    ScriptServer.AlternarVentana(control, comando, "contenedorVentanaReferencias", "ventanaReferencias");
                    break;
                case "Lectura":
                    ScriptServer.AlternarVentana(control, comando, "modalVentanaLectura", "ventanaLectura");
                    break;
                case "LecturaHistorial":
                    ScriptServer.AlternarVentana(control, comando, "HistorialLectura", "modalLecturaHistorial", "LecturaHistorial");
                    break;
            }
        }
        /// <summary>
        /// Método que muestra las modificaciones realizadas a un registro (Bitacora Registro).
        /// </summary>
        /// <param name="idRegistro">Id que identifica un registro de la tabla Asignación Diesel</param>
        /// <param name="idTabla">Id que identifica a la tabla Asignación Diesel en la base de datos</param>
        /// <param name="Titulo">Nombre que se le asignara a la ventana de Bitacora</param>
        private void inicicalizaBitacora(string idRegistro, string idTabla, string Titulo)
        {
            //Crea la variable url que almacena una ruta de ubicación de la ventana, que contendra la bitacora de un registros de Asignación Diesel.
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/EgresoServicio/ReporteValesDiesel.aspx",
                                                                "~/Accesorios/BitacoraRegistro.aspx?idT=" + idTabla + "&idR=" + idRegistro + "&tB=" + Titulo);
            //Variable que almacena las medidas de la ventana que contendra los datos de Bitacora de Asignación Diesel.
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Invoca el método de la clase ScriptServer que abrira una nueva ventana acorde a una ubicacion, un titulo de la ventana, una configuracion de dimensiones.
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "Bitacora Asignación Diesel", configuracion, Page);
        }
        #endregion

        #region Eventos GridView "Vales de Diesel"

        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView "Vales de Diesel"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamano_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando tamaño del GridView
            Controles.CambiaTamañoPaginaGridView(gvValesDiesel, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamano.SelectedValue));

            //Sumando Totales
            sumaTotales();
        }
        /// <summary>
        /// Evento Producido al Exportar al Contenido del GridView "Vales de Diesel"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportar_Click(object sender, EventArgs e)
        {
            //Exportando Contenido del GridView
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "Id");
        }
        /// <summary>
        /// Evento Producido al Cambiar la Expresión del Ordenamiento del GridView "Vales de Diesel"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvValesDiesel_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión del Ordenamiento
            lblOrdenado.Text = Controles.CambiaSortExpressionGridView(gvValesDiesel, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression);

            //Sumando Totales
            sumaTotales();
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Página del GridView "Vales de Diesel"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvValesDiesel_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            Controles.CambiaIndicePaginaGridView(gvValesDiesel, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex);

            //Sumando Totales
            sumaTotales();
        }
        /// <summary>
        /// Evento Producido al Ver el Historial de Lectura
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbLecturaAlta_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvValesDiesel.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvValesDiesel, sender, "lnk", false);

                //Instanciando Diesel
                using (SAT_CL.EgresoServicio.AsignacionDiesel diesel = new SAT_CL.EgresoServicio.AsignacionDiesel(Convert.ToInt32(gvValesDiesel.SelectedDataKey["Id"])))
                {
                    //Validando que Exista el Vale
                    if (diesel.habilitar)
                    {
                        //Inicializando Control de Lectura
                        wucLectura.InicializaControl(diesel.id_lectura, diesel.id_unidad_diesel, diesel.objDetalleLiquidacion.id_operador);

                        //Configurando Control
                        lkbCerrarLectura.CommandArgument = "Diesel";

                        //Abriendo ventana modal
                        alternaVentanaModal(this.Page, "Lectura");
                    }
                    else
                        //Mostrando Notificación
                        ScriptServer.MuestraNotificacion(this.Page, "No existe el Vale de Diesel", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                }
            }
        }

        #endregion

        #region Eventos Ventana modal Asignación Diesel

        /// <summary>
        /// Click del botón de cierre de alguna ventana modal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarVentanaModal_Click(object sender, EventArgs e)
        {
            //Obteniendo Control
            LinkButton lnk = (LinkButton)sender;

            //Validando el Comando del Control
            switch (lnk.CommandName)
            {
                case "Referencias":
                    //Cerrando ventana modal 
                    alternaVentanaModal(lnk, "Referencias");

                    //Validando Argumento
                    switch (lnk.CommandArgument)
                    {
                        case "Diesel":
                            //Cerrando ventana modal 
                            alternaVentanaModal(lnk, "Diesel");
                            break;
                    }

                    break;
                case "Diesel":
                    //Cerrando ventana modal 
                    alternaVentanaModal(lnk, "Diesel");
                    break;
                case "Lectura":
                    //Cerrando ventana modal 
                    alternaVentanaModal(lnk, "Lectura");

                    /*/Validando Argumento
                    switch (lnk.CommandArgument)
                    {
                        case "Diesel":
                            //Cerrando ventana modal 
                            alternaVentanaModal(lnk, "Diesel");
                            break;
                    }//*/
                    break;
            }
        }
        /// <summary>
        /// Click en botón Cencelar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucAsignacionDiesel_ClickCancelarAsignacion(object sender, EventArgs e)
        {
            //Cerrando ventana modal de edición
            alternaVentanaModal(this, "Diesel");
        }
        /// <summary>
        /// Click en botón Guardar vale
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucAsignacionDiesel_ClickGuardarAsignacion(object sender, EventArgs e)
        {
            //Realizando el guardado
            RetornoOperacion resultado = wucAsignacionDiesel.GuardaDiesel();

            //Si no hay errores
            if (resultado.OperacionExitosa)
            {                
                //Actualizando lista de vales de diesel
                buscaValesDiesel();
                //Cerrando ventana modal
                alternaVentanaModal(this, "Diesel");
            }

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Click en 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucAsignacionDiesel_ClickReferenciaAsignacion(object sender, EventArgs e)
        {
            //Inicializando Control de Referencias
            ucReferenciaViaje.InicializaControl(wucAsignacionDiesel.idAsignacionDiesel, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, 0, 69);

            //Cerrando ventana modal 
            alternaVentanaModal(this.Page, "Diesel");

            //Abriendo Ventana de Referencias
            alternaVentanaModal(this.Page, "Referencias");

            //Actualizando Comando del Boton Cerrar
            lkbCerrarReferencias.CommandArgument = "Diesel";
        }
        /// <summary>
        /// Click en algún botón del GV de Vales de Diesel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbValeDiesel_Click(object sender, EventArgs e)
        {
            //Obteniendo la referencia del botón pulsado
            LinkButton lkb = ((LinkButton)sender);

            //Seleccionando fila
            Controles.SeleccionaFila(gvValesDiesel, sender, "lnk", false);
            //Determiando que botón fue pulsado
            switch (lkb.CommandName)
            { 
                case "Editar":
                    //Inicializando control de edición de vales
                    wucAsignacionDiesel.InicializaControlUsuario(Convert.ToInt32(gvValesDiesel.SelectedDataKey["Id"]));
                    //Mostrando ventana modal correspondiente
                    alternaVentanaModal(lkb, "Diesel");
                    break;
            }
        }

        #endregion        

        #region Eventos Ventana Modal Referencias

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucReferenciaViaje_ClickGuardarReferenciaViaje(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Guardando Referencia
            result = ucReferenciaViaje.GuardaReferenciaViaje();

            //Validando que la Operación fuese exitosa
            if (result.OperacionExitosa)

                //Invocando Método de Busqueda
                buscaValesDiesel();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucReferenciaViaje_ClickEliminarReferenciaViaje(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Eliminando Referencia
            result = ucReferenciaViaje.EliminaReferenciaViaje();

            //Validando que la Operación fuese exitosa
            if (result.OperacionExitosa)

                //Invocando Método de Busqueda
                buscaValesDiesel();
        }

        #endregion

        #region Eventos Lectura

        /// <summary>
        /// Evento Producido al Guardar la Lectura
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucLectura_ClickGuardarLectura(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Inicializando Bloque Transaccional
            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciando Vale de Diesel
                using (SAT_CL.EgresoServicio.AsignacionDiesel diesel = new SAT_CL.EgresoServicio.AsignacionDiesel(Convert.ToInt32(gvValesDiesel.SelectedDataKey["Id"])))
                {
                    //Validando que exista el Vale
                    if (diesel.habilitar)
                    {
                        //Guardando Lectura
                        result = wucLectura.GuardarLectura();

                        //Validando que se Haya Guardado el Registro
                        if (result.OperacionExitosa)
                        {
                            //Obteniendo Lectura de Diesel
                            int idLectura = result.IdRegistro;

                            //Actualizando Lectura
                            result = diesel.ActualizaLecturaDiesel(idLectura, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                            //Validando que se Haya Actualizado el Registro
                            if (result.OperacionExitosa)

                                //Completando Transacción
                                trans.Complete();
                        }
                    }
                }
            }

            //Mostrando Notificación
            ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        /// <summary>
        /// Evento Producido al Eliminar la Lectura
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucLectura_ClickEliminarLectura(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Inicializando Bloque Transaccional
            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciando Vale de Diesel
                using (SAT_CL.EgresoServicio.AsignacionDiesel diesel = new SAT_CL.EgresoServicio.AsignacionDiesel(wucAsignacionDiesel.idAsignacionDiesel))
                {
                    //Validando que exista el Vale
                    if (diesel.habilitar)
                    {
                        //Guardando Lectura
                        result = wucLectura.DeshabilitarLectura();

                        //Validando que se Haya Guardado el Registro
                        if (result.OperacionExitosa)
                        {
                            //Actualizando Lectura
                            result = diesel.ActualizaLecturaDiesel(0, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                            //Validando que se Haya Actualizado el Registro
                            if (result.OperacionExitosa)

                                //Completando Transacción
                                trans.Complete();
                        }
                    }
                }
            }

            //Mostrando Notificación
            ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        #endregion
    }
}