using SAT_CL.Global;
using System;
using System.Data;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.Tarifas
{
    public partial class Tarifas : System.Web.UI.Page
    {
        #region Eventos

        /// <summary>
        /// Evento Producido al efectuarse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {  //Aplicando seguridad de la forma
            SAT_CL.Seguridad.Forma.AplicaSeguridadForma(this, "content1", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
            //Validando que se produjo un PostBack
            if (!(Page.IsPostBack))
                //Inicializando Forma
                inicializaForma();
        }
        /// <summary>
        /// Evento producido al Presionar el Menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbElementoMenu_Click(object sender, EventArgs e)
        {   //Validando estatus de Página
            switch (((LinkButton)sender).CommandName)
            {
                case "Nuevo":
                    {   //Asignando estatus nuevo
                        Session["estatus"] = Pagina.Estatus.Nuevo;
                        //Limpiando Id de sesión
                        Session["id_registro"] = 0;
                        //Limpiando Mensaje de Error
                        lblError.Text = "";
                        //Limpiando contenido de forma
                        inicializaForma();
                        break;
                    }
                case "Abrir":
                    {   //Inicializando Apertura de Registros
                        inicializaAperturaRegistro(47, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);
                        break;
                    }
                case "Guardar":
                    {   //Invocando Método de Guardado
                        guardaTarifa();
                        break;
                    }
                case "Editar":
                    {   //Asignando estatus nuevo
                        Session["estatus"] = Pagina.Estatus.Edicion;
                        //Limpiando contenido de forma
                        inicializaForma();
                        break;
                    }
                case "Eliminar":
                    {   //Instanciando Producto
                        using (SAT_CL.Tarifas.Tarifa tar = new SAT_CL.Tarifas.Tarifa(Convert.ToInt32(Session["id_registro"])))
                        {   //Validando que exista un Producto
                            if (tar.id_tarifa != 0)
                            {   //Declarando Objeto de Retorno
                                RetornoOperacion result = new RetornoOperacion();
                                //Declarando Objeto de Retorno
                                RetornoOperacion resultDetalle = new RetornoOperacion();
                                //Declarando Objeto de Retorno
                                RetornoOperacion resultCargo = new RetornoOperacion();
                                //Inicializando Transacción
                                using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                                {   //Declarando Objetos de Ciclo
                                    bool resCargoCiclo, resDetalleCiclo;
                                    resCargoCiclo = resDetalleCiclo = true;
                                    //Declarando las Filas
                                    DataRow drCargo, drDetalle;
                                    //Obteniendo Detalles
                                    using (DataTable dtDetalles = SAT_CL.Tarifas.TarifaMatriz.ObtieneMatrizTarifa(Convert.ToInt32(Session["id_registro"])))
                                    {   
                                        //Validando Origen de Datos
                                        if (Validacion.ValidaOrigenDatos(dtDetalles))
                                        {
                                            //Inicializando Contador
                                            int contadorDetalle = 0;
                                            //Inicializando Ciclo
                                            while (resDetalleCiclo)
                                            {
                                                //Obteniendo Fila
                                                drDetalle = dtDetalles.Rows[contadorDetalle];
                                                //Instanciando Cargo Recurrente
                                                using (SAT_CL.Tarifas.TarifaMatriz tm = new SAT_CL.Tarifas.TarifaMatriz(Convert.ToInt32(drDetalle["Id"])))
                                                {
                                                    //Validando que exista el Registro
                                                    if (tm.id_tarifa_matriz != 0)
                                                    {
                                                        //Deshabilitando el Registro
                                                        resultDetalle = tm.DeshabilitaTarifaMatriz(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                        //Incrementando Contador
                                                        contadorDetalle += 1;
                                                        //Validando si ya termino el ciclo
                                                        resDetalleCiclo = dtDetalles.Rows.Count > contadorDetalle ? resultDetalle.OperacionExitosa : false;
                                                    }
                                                    else
                                                        //Terminando Ciclo
                                                        resDetalleCiclo = false;
                                                }
                                            }
                                        }
                                        else
                                            //Instanciando Resultado Positivo
                                            resultDetalle = new RetornoOperacion(0);
                                    }
                                    //Obteniendo Cargos
                                    using (DataTable dtCargos = SAT_CL.Tarifas.CargoRecurrente.ObtieneCargosRecurrentes(Convert.ToInt32(Session["id_registro"]), 0))
                                    {   
                                        //Validando Cargos
                                        if (Validacion.ValidaOrigenDatos(dtCargos))
                                        {
                                            //Inicializando Contador
                                            int contadorCargo = 0;
                                            //Inicializando Ciclo
                                            while (resCargoCiclo)
                                            {   
                                                //Obteniendo Fila
                                                drCargo = dtCargos.Rows[contadorCargo];
                                                //Instanciando Cargo Recurrente
                                                using (SAT_CL.Tarifas.CargoRecurrente cr = new SAT_CL.Tarifas.CargoRecurrente(Convert.ToInt32(drCargo["Id"])))
                                                {   
                                                    //Validando que exista el Registro
                                                    if (cr.id_cargo_recurrente != 0)
                                                    {   
                                                        //Deshabilitando el Registro
                                                        resultCargo = cr.DeshabilitaCargoRecurrente(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                        //Incrementando Contador
                                                        contadorCargo += 1;
                                                        //Validando si ya termino el ciclo
                                                        resCargoCiclo = dtCargos.Rows.Count > contadorCargo ? resultCargo.OperacionExitosa : false;
                                                    }
                                                    else
                                                        //Terminando Ciclo
                                                        resCargoCiclo = false;
                                                }
                                            }
                                        }
                                        else
                                            //Instanciando Resultado Positivo
                                            resultCargo = new RetornoOperacion(0);
                                    }
                                    //Validando que ambas Operaciones hayan sido Exitosas
                                    if(resultDetalle.OperacionExitosa && resultCargo.OperacionExitosa)
                                    {   //Deshabilitando Tarifa
                                        result = tar.DeshabilitaTarifa(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                        //Validando que la Deshabilitación haya sido exitosa
                                        if (result.OperacionExitosa)
                                            //Completando Transaccion
                                            trans.Complete();
                                    }
                                }
                                //Validando que la Operación sea exitosa
                                if (result.OperacionExitosa)
                                {   //Limpiando registro de Session
                                    Session["id_registro"] = 0;
                                    //Cambiando a Estatus "Nuevo"
                                    Session["estatus"] = Pagina.Estatus.Nuevo;
                                    //Inicializando Forma
                                    inicializaForma();
                                    //Restableciendo Control
                                    ucCargoRecurrente.InicializaControlUsuario(0, 0, Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:", 1)));
                                    //Inicializando GridView
                                    TSDK.ASP.Controles.InicializaGridview(gvTarifamatriz);
                                    TSDK.ASP.Controles.InicializaIndices(gvTarifamatriz);
                                }
                                //Mostrando Mensaje de Operación
                                lblError.Text = result.Mensaje;
                            }
                        }
                        break;
                    }
                case "Bitacora":
                    {   //Inicializando Bitacora
                        inicializaBitacora(Session["id_registro"].ToString(), "47", "Tarifas");
                        break;
                    }
                case "Referencias":
                    {   //Invocando Método de Inicialización de Referencias
                        inicializaReferenciaRegistro(Session["id_registro"].ToString(), "47", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString());
                        break;
                    }
                case "Archivos":
                    //TODO: Implementar uso de archivos ligados a registro
                    break;
                case "Acerca":
                    //TODO: Implementar uso de acerca de
                    break;
                case "Ayuda":
                    //TODO: Implementar uso de ayuda
                    break;
            }
        }

        #region Eventos Clasificación

        /// <summary>
        /// Evento Producido al Presionar el Boton "Guardar" de la Clasificación
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucClasificacion_ClickGuardar(object sender, EventArgs e)
        {   //Guardando Cambios
            ucClasificacion.GuardaCambiosClasificacion();
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Cancelar" de la Clasificación
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucClasificacion_ClickCancelar(object sender, EventArgs e)
        {   //Cancelando Cambios
            ucClasificacion.CancelaCambiosClasificacion();
        }

        #endregion

        #region Eventos Cargo Recurrente

        /// <summary>
        /// Evento Producido al Guardar los Cargos Recurrentes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucCargoRecurrente_ClickGuardarCargoRecurrente(object sender, EventArgs e)
        {
            ucCargoRecurrente.GuardaCargoRecurrente();
        }
        /// <summary>
        /// Evento Producido al Eliminar los Cargos Recurrentes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucCargoRecurrente_ClickEliminarCargoRecurrente(object sender, EventArgs e)
        {
            ucCargoRecurrente.EliminaCargoRecurrente();
        }

        #endregion

        /// <summary>
        /// Evento Producido al Presionar el Boton "Guardar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {   //Invocando Método de Guardado
            guardaTarifa();
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Cancelar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {   //Validando el Estatus de la Página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Edicion:
                    //Cambiando Estatus a Lectura
                    Session["estatus"] = Pagina.Estatus.Lectura;
                    break;
            }
            //Inicializando Valores de la Forma
            inicializaForma();
            //Deshabilitando Control
            ucClasificacion.Enabled = false;
            //Inicializando Control
            ucCargoRecurrente.Enabled = false;
            //Limpiando Mensaje
            lblError.Text = "";
            //Deshabilitando Controles
            txtRotCol.Enabled =
            txtRotFila.Enabled = 
            txtCatCol.Enabled =
            txtCatFila.Enabled = false;
            //Limpiando Controles
            txtTarifaCargado.Text =
            txtTarifaVacio.Text = "";
            //Quitando Selección del GridView
            gvTarifamatriz.SelectedIndex = -1;
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice del Control "Columnas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlColumnas_SelectedIndexChanged(object sender, EventArgs e)
        {   //Validando el Tipo de Columna
            switch ((SAT_CL.Tarifas.Tarifa.CriterioMatrizTarifa)Convert.ToInt32(ddlColumnas.SelectedValue))
            {   //Opciones de Catalogo
                case SAT_CL.Tarifas.Tarifa.CriterioMatrizTarifa.Producto:
                case SAT_CL.Tarifas.Tarifa.CriterioMatrizTarifa.Ciudad:
                case SAT_CL.Tarifas.Tarifa.CriterioMatrizTarifa.Ubicacion:
                    {   //Asignando Valores
                        txtCatCol.Visible = true;
                        txtRotCol.Visible = false;
                        break;
                    }
                //Opciones de Captura
                case SAT_CL.Tarifas.Tarifa.CriterioMatrizTarifa.Paradas:
                case SAT_CL.Tarifas.Tarifa.CriterioMatrizTarifa.Distancia:
                case SAT_CL.Tarifas.Tarifa.CriterioMatrizTarifa.Peso:
                case SAT_CL.Tarifas.Tarifa.CriterioMatrizTarifa.Volumen:
                case SAT_CL.Tarifas.Tarifa.CriterioMatrizTarifa.Cantidad:
                    {   //Asignando Valores
                        txtCatCol.Visible = false;
                        txtRotCol.Visible = true;
                        break;
                    }
            }
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice del Control "Filas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlFilas_SelectedIndexChanged(object sender, EventArgs e)
        {   //Validando el Tipo de Columna
            switch ((SAT_CL.Tarifas.Tarifa.CriterioMatrizTarifa)Convert.ToInt32(ddlFilas.SelectedValue))
            {   //Opciones de Catalogo
                case SAT_CL.Tarifas.Tarifa.CriterioMatrizTarifa.Producto:
                case SAT_CL.Tarifas.Tarifa.CriterioMatrizTarifa.Ciudad:
                case SAT_CL.Tarifas.Tarifa.CriterioMatrizTarifa.Ubicacion:
                case SAT_CL.Tarifas.Tarifa.CriterioMatrizTarifa.NoAplica:
                    {   //Asignando Valores
                        txtCatFila.Visible = true;
                        txtRotFila.Visible = false;
                        break;
                    }
                //Opciones de Captura
                case SAT_CL.Tarifas.Tarifa.CriterioMatrizTarifa.Paradas:
                case SAT_CL.Tarifas.Tarifa.CriterioMatrizTarifa.Distancia:
                case SAT_CL.Tarifas.Tarifa.CriterioMatrizTarifa.Peso:
                case SAT_CL.Tarifas.Tarifa.CriterioMatrizTarifa.Volumen:
                case SAT_CL.Tarifas.Tarifa.CriterioMatrizTarifa.Cantidad:
                    {   //Asignando Valores
                        txtCatFila.Visible = false;
                        txtRotFila.Visible = true;
                        break;
                    }
            }
        }

        #region Eventos Tarifa Matriz (Detalles)

        #region Eventos GridView "Tarifa Matriz (Detalles)"

        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoReqDisp_SelectedIndexChanged(object sender, EventArgs e)
        {   //Cambiando Tamaño de Página
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvTarifamatriz, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"],"Table"), Convert.ToInt32(ddlTamanoReqDisp.SelectedValue), true, 0);
        }
        /// <summary>
        /// Evento Producido al Exportar el Contenido del GridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportar_Click(object sender, EventArgs e)
        {   //Exportando Contenido
            TSDK.ASP.Controles.ExportaContenidoGridView(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"));
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Paginación
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvTarifamatriz_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {   //Cambiando Indice de Página
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvTarifamatriz, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex);
        }
        /// <summary>
        /// Evento Producido al Cambiar el Ordenamiento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvTarifamatriz_Sorting(object sender, GridViewSortEventArgs e)
        {   //Cambiando Expresión de Ordenamiento
            lblOrdenar.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvTarifamatriz, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression);
        }
        /// <summary>
        /// Evento Producido al Dar Click al LinkButton "Editar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEditarMatriz_Click(object sender, EventArgs e)
        {   //Validando que existan Registros
            if(gvTarifamatriz.DataKeys.Count > 0)
            {   //Seleccionando Fila
                TSDK.ASP.Controles.SeleccionaFila(gvTarifamatriz, sender, "lnk", false);
                //Instanciando Detalle de Tarifa
                using(SAT_CL.Tarifas.TarifaMatriz tm = new SAT_CL.Tarifas.TarifaMatriz(Convert.ToInt32(gvTarifamatriz.SelectedDataKey["Id"])))
                {   //Validando que exista el Registro
                    if(tm.id_tarifa_matriz != 0)
                    {   //Validando Columna Filtro
                        switch ((SAT_CL.Tarifas.Tarifa.CriterioMatrizTarifa)Convert.ToInt32(ddlColumnas.SelectedValue))
                        {   //Opciones de Catalogo
                            case SAT_CL.Tarifas.Tarifa.CriterioMatrizTarifa.Producto:
                            case SAT_CL.Tarifas.Tarifa.CriterioMatrizTarifa.Ciudad:
                            case SAT_CL.Tarifas.Tarifa.CriterioMatrizTarifa.Ubicacion:
                            case SAT_CL.Tarifas.Tarifa.CriterioMatrizTarifa.NoAplica:
                                {   //Mostrando Control de Catalogo
                                    txtCatCol.Text = tm.valor_desc_col + " ID:" + tm.valor_filtro_col;
                                    txtCatCol.Visible = true;
                                    txtCatCol.Enabled =
                                    txtRotCol.Visible = false;
                                    break;
                                }
                            //Opciones de Captura
                            case SAT_CL.Tarifas.Tarifa.CriterioMatrizTarifa.Paradas:
                            case SAT_CL.Tarifas.Tarifa.CriterioMatrizTarifa.Distancia:
                            case SAT_CL.Tarifas.Tarifa.CriterioMatrizTarifa.Peso:
                            case SAT_CL.Tarifas.Tarifa.CriterioMatrizTarifa.Volumen:
                            case SAT_CL.Tarifas.Tarifa.CriterioMatrizTarifa.Cantidad:
                                {   //Mostrando Control de Catalogo
                                    txtRotCol.Text = tm.valor_desc_col;
                                    txtRotCol.Enabled =
                                    txtCatCol.Visible =
                                    txtRotCol.Visible = true;
                                    break;
                                }
                        }
                        //Validando Columna Filtro
                        switch ((SAT_CL.Tarifas.Tarifa.CriterioMatrizTarifa)Convert.ToInt32(ddlFilas.SelectedValue))
                        {   //Opciones de Catalogo
                            case SAT_CL.Tarifas.Tarifa.CriterioMatrizTarifa.Producto:
                            case SAT_CL.Tarifas.Tarifa.CriterioMatrizTarifa.Ciudad:
                            case SAT_CL.Tarifas.Tarifa.CriterioMatrizTarifa.Ubicacion:
                            case SAT_CL.Tarifas.Tarifa.CriterioMatrizTarifa.NoAplica:
                                {   //Mostrando Control de Catalogo
                                    txtCatFila.Text = tm.valor_desc_row + " ID:" + tm.valor_filtro_row;
                                    txtCatFila.Visible = true;
                                    txtCatFila.Enabled =
                                    txtRotFila.Visible = false;
                                    break;
                                }
                            //Opciones de Captura
                            case SAT_CL.Tarifas.Tarifa.CriterioMatrizTarifa.Paradas:
                            case SAT_CL.Tarifas.Tarifa.CriterioMatrizTarifa.Distancia:
                            case SAT_CL.Tarifas.Tarifa.CriterioMatrizTarifa.Peso:
                            case SAT_CL.Tarifas.Tarifa.CriterioMatrizTarifa.Volumen:
                            case SAT_CL.Tarifas.Tarifa.CriterioMatrizTarifa.Cantidad:
                                {   //Mostrando Control de Catalogo
                                    txtRotFila.Text = tm.valor_desc_row;
                                    txtRotFila.Enabled =
                                    txtCatFila.Visible = false;
                                    txtRotFila.Visible = true;
                                    break;
                                }
                        }
                        txtTarifaCargado.Text = tm.tarifa_cargado.ToString();
                        txtTarifaVacio.Text = tm.tarifa_vacio.ToString();
                        lblTipoCargoRecurrente.Text = "Ligado a un Detalle de Tarifa";
                        //Inicializando Control
                        ucCargoRecurrente.Enabled = true;
                        ucCargoRecurrente.InicializaControlUsuario(tm.id_tarifa, tm.id_tarifa_matriz, Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:", 1)));
                    }
                }
            }
        }

        #endregion
        
        /// <summary>
        /// Evento Producido al Presionar el Boton "Guardar Matriz"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardarMatriz_Click(object sender, EventArgs e)
        {   
            //Validando que exista un Tarifa
            if (Convert.ToInt32(Session["id_registro"]) != 0)
            {   //Declarando Variables Contenedoras
                string val_desc_col, val_desc_row, val_id_col, val_id_row = "";
                //Invocando Método de Obtencion
                obtieneValoresColumnaFila(out val_desc_col, out val_desc_row, out val_id_col, out val_id_row);
                //Declarando Objeto de Retorno
                RetornoOperacion result = new RetornoOperacion();
                //Validando que exista una Fila Seleccionada
                if (gvTarifamatriz.SelectedIndex != -1)
                {   //Instanciando Detalle de Tarifa
                    using (SAT_CL.Tarifas.TarifaMatriz tm = new SAT_CL.Tarifas.TarifaMatriz(Convert.ToInt32(gvTarifamatriz.SelectedDataKey["Id"])))
                    {   //Validando que exista el Registro
                        if (tm.id_tarifa_matriz != 0)
                        {   //Editando Matriz
                            result = tm.EditaTarifaMatriz(Convert.ToInt32(Session["id_registro"]), tm.valor_filtro_col, tm.valor_filtro_row, tm.valor_desc_col,
                                                          tm.valor_desc_row, 0, 0, Convert.ToDecimal(txtTarifaCargado.Text), Convert.ToDecimal(txtTarifaVacio.Text),
                                                          ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                        }
                    }
                }
                else
                {   //Insertar Matriz
                    result = SAT_CL.Tarifas.TarifaMatriz.InsertaTarifaMatriz(Convert.ToInt32(Session["id_registro"]), val_id_col, val_id_row, val_desc_col,
                                                                             val_desc_row, 0, 0, Convert.ToDecimal(txtTarifaCargado.Text), Convert.ToDecimal(txtTarifaVacio.Text),
                                                                             ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                }
                //Validando que la Operación fuera Exitosa
                if (result.OperacionExitosa)
                {   //Limpiando Controles
                    limpiaControlesTarifaMatriz();
                    //Habilitando Controles
                    habilitarControlesTarifaMatriz(true);
                    //Cargando Detalles
                    cargaDetallesTarifaMatriz();
                    //Ocultando Panel
                    pnlMatriz.Visible = false;
                }
                //Mostrando Mensaje
                lblErrorMatriz.Text = result.Mensaje;
            }
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Eliminar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarMatriz_Click(object sender, EventArgs e)
        {   //Limpiando Controles
            limpiaControlesTarifaMatriz();
            //Habilitando Controles
            habilitarControlesTarifaMatriz(true);
            //Cargando Detalles
            cargaDetallesTarifaMatriz();
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Eliminar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEliminarMatriz_Click(object sender, EventArgs e)
        {   
            //Validando que existan Registros
            if (gvTarifamatriz.DataKeys.Count > 0)
            {   
                //Declarando Objeto de Retorno
                RetornoOperacion result = new RetornoOperacion();
                RetornoOperacion resultCargo = new RetornoOperacion();
                //Validando que exista una Fila Seleccionada
                if (gvTarifamatriz.SelectedIndex != -1)
                {   
                    //Instanciando Detalle de Tarifa
                    using (SAT_CL.Tarifas.TarifaMatriz tm = new SAT_CL.Tarifas.TarifaMatriz(Convert.ToInt32(gvTarifamatriz.SelectedDataKey["Id"])))
                    {   
                        //Inicializando Transacción
                        using(TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                        {   
                            //Obteniendo cargos Ligados al Detalle
                            using(DataTable dtCargosDetalles = SAT_CL.Tarifas.CargoRecurrente.ObtieneCargosRecurrentesPorDetalle(tm.id_tarifa_matriz))
                            {   
                                //Validando Origen de Datos
                                if(TSDK.Datos.Validacion.ValidaOrigenDatos(dtCargosDetalles))
                                {   
                                    //Declarado Variable Auxiliares
                                    int contador = 0;
                                    bool res = true;
                                    DataRow dr;
                                    
                                    //Iniciando Ciclo
                                    while(res)
                                    {   
                                        //Obteniendo Fila
                                        dr = dtCargosDetalles.Rows[contador];
                                        //Instanciando Cargo Recurrente
                                        using (SAT_CL.Tarifas.CargoRecurrente cr = new SAT_CL.Tarifas.CargoRecurrente(Convert.ToInt32(dr["Id"])))
                                        {   
                                            //Validando que existe el Cargo
                                            if (cr.id_cargo_recurrente != 0)
                                            {   
                                                //Deshabilitando el Registro
                                                resultCargo = cr.DeshabilitaCargoRecurrente(((SAT_CL.Seguridad.Usuario)Session["id_usuario"]).id_usuario);
                                                //Incrementando Contador
                                                contador += 1;
                                                //Validando si el ciclo ya termino
                                                res = dtCargosDetalles.Rows.Count > contador ? resultCargo.OperacionExitosa : false;
                                            }
                                            else//Terminando Ciclo
                                                res = false;
                                        }
                                    }
                                }
                            }
                            //Validando que exista el Registro
                            if (tm.id_tarifa_matriz != 0)
                            {   
                                //Editando Matriz
                                result = tm.DeshabilitaTarifaMatriz(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                
                                //Validando que se hayan Realizado Ambas Deshabilitaciones
                                if(result.OperacionExitosa && resultCargo.OperacionExitosa)
                                    //Completando Transacción
                                    trans.Complete();
                                
                            }
                        }                        
                        
                    }
                }
                else
                    //Instanciando Excepcion
                    result = new RetornoOperacion("No existe un registro seleccionado");
                
                //Validando que la Operación fuera Exitosa
                if (result.OperacionExitosa)
                {   
                    //Limpiando Controles
                    limpiaControlesTarifaMatriz();
                    
                    //Habilitando Controles
                    habilitarControlesTarifaMatriz(true);

                    //Cargando Detalles
                    cargaDetallesTarifaMatriz();
                    
                    //Cargando Cargos recurrentes
                    ucCargoRecurrente.InicializaControlUsuario(Convert.ToInt32(Session["id_registro"]), 0, Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:", 1)));
                }
                
                //Mostrando Error
                lblErrorMatriz.Text = result.Mensaje;
            }
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Agregar Columna"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imgbtnQuitarColumna_Click(object sender, ImageClickEventArgs e)
        {
            //Asignando el Comando
            btnConfirmarEliminacion.CommandName = "Columna";

            //Asignando Valor
            lblValor.Text = "Columna";

            //Alternando Ventana
            TSDK.ASP.ScriptServer.AlternarVentana(upbtnQuitarColumna, upbtnQuitarColumna.GetType(), "Ventana Modal", "contenedorVentanaConfirmacion", "ventanaConfirmacion");
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Quitar Fila"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imgbtnQuitarFila_Click(object sender, ImageClickEventArgs e)
        {   
            //Validando que sea una Fila valida
            if (txtCatFila.Text != "Ninguno ID:0" || txtRotFila.Text != "")
            {
                //Asignando el Comando
                btnConfirmarEliminacion.CommandName = "Fila";
                
                //Asignando Valor
                lblValor.Text = "Fila";

                //Alternando Ventana
                TSDK.ASP.ScriptServer.AlternarVentana(upbtnQuitarFila, upbtnQuitarFila.GetType(), "Ventana Modal", "contenedorVentanaConfirmacion", "ventanaConfirmacion");
            }
            else
                //Mostrando Mensaje de Error
                lblErrorMatriz.Text = "No se pueden quitar los registros que no contienen ninguna Fila.";
        }
        /// <summary>
        /// Evento 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkVerMatriz_Click(object sender, EventArgs e)
        {   
            //Cargando Panel
            pnlMatriz.Visible = true;
            cargaMatrizDetallesTarifa();
        }

        #region Eventos Ventana Modal

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnConfirmarEliminacion_Click(object sender, EventArgs e)
        {
            //Validando el Comando del Control
            switch(((Button)sender).CommandName)
            {
                case "Fila":
                    {
                        //Obteniendo Detalles Ligados a una Tarifa y el Valor de la Columna
                        using (DataTable dt = SAT_CL.Tarifas.TarifaMatriz.ObtieneDetallesTarifaValorColumna(Convert.ToInt32(Session["id_registro"]), "", txtRotFila.Visible == true ? txtRotFila.Text : TSDK.Base.Cadena.RegresaCadenaSeparada(txtCatFila.Text, "ID:", 1)))
                        {
                            //Validando que existan Registros
                            if (TSDK.Datos.Validacion.ValidaOrigenDatos(dt))
                            {
                                //Declarando Objeto de Retorno
                                RetornoOperacion result = new RetornoOperacion();
                                //Creando ambiente transaccional
                                using (TransactionScope transaccion = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                                {
                                    //Declarando Objeto del Ciclo While
                                    bool res1 = true;
                                    //Declarando Contador
                                    int contador = 0;
                                    //Declarando Fila
                                    DataRow dr = null;

                                    //Iniciando Ciclo While
                                    while (res1)
                                    {
                                        //Asignando Fila
                                        dr = dt.Rows[contador];
                                        //Instanciando Tarifa Matriz
                                        using (SAT_CL.Tarifas.TarifaMatriz tm = new SAT_CL.Tarifas.TarifaMatriz(Convert.ToInt32(dr["Id"])))
                                        {
                                            //Validando que exista el Registro
                                            if (tm.id_tarifa_matriz != 0)
                                            {
                                                //Deshabilitando Registro
                                                result = tm.DeshabilitaTarifaMatriz(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                //Incrementando Contador
                                                contador += 1;
                                                //Guardando resultado de la Operación
                                                res1 = dt.Rows.Count > contador ? result.OperacionExitosa : false;
                                            }
                                            else//Asignando Negativo el Resultado
                                                res1 = false;
                                        }
                                    }
                                    //Validando si la Operación fue Exitosa
                                    if (result.OperacionExitosa)
                                        //Finalizando Transacción
                                        transaccion.Complete();
                                }

                                //Validando si la Operación fue Exitosa
                                if (result.OperacionExitosa)
                                {
                                    //Limpiando Controles
                                    limpiaControlesTarifaMatriz();

                                    //Habilitando Controles
                                    habilitarControlesTarifaMatriz(true);

                                    //Cargando Detalles
                                    cargaDetallesTarifaMatriz();
                                }
                                //Mostrando Mensaje de Operación
                                lblErrorMatriz.Text = result.Mensaje;
                            }
                            else//Mostrando Mensaje de Error
                                lblErrorMatriz.Text = "No Existen Detalles con este Valor";
                        }
                        break;
                    }
                case "Columna":
                    {
                        //Declarando Variables Contenedoras
                        string val_desc_col, val_desc_row, val_id_col, val_id_row = "";

                        //Invocando Método de Obtencion
                        obtieneValoresColumnaFila(out val_desc_col, out val_desc_row, out val_id_col, out val_id_row);

                        //Obteniendo Detalles Ligados a una Tarifa y el Valor de la Columna
                        using (DataTable dt = SAT_CL.Tarifas.TarifaMatriz.ObtieneDetallesTarifaValorColumna(Convert.ToInt32(Session["id_registro"]), val_id_col, ""))
                        {
                            //Validando que existan Registros
                            if (TSDK.Datos.Validacion.ValidaOrigenDatos(dt))
                            {
                                //Declarando Objeto de Retorno
                                RetornoOperacion result = new RetornoOperacion();
                                //Creando ambiente transaccional
                                using (TransactionScope transaccion = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                                {
                                    //Declarando Objeto del Ciclo While
                                    bool res1 = true;
                                    //Declarando Contador
                                    int contador = 0;
                                    //Declarando Fila
                                    DataRow dr = null;

                                    //Iniciando Ciclo While
                                    while (res1)
                                    {
                                        //Asignando Fila
                                        dr = dt.Rows[contador];
                                        //Instanciando Tarifa Matriz
                                        using (SAT_CL.Tarifas.TarifaMatriz tm = new SAT_CL.Tarifas.TarifaMatriz(Convert.ToInt32(dr["Id"])))
                                        {
                                            //Validando que exista el Registro
                                            if (tm.id_tarifa_matriz != 0)
                                            {
                                                //Deshabilitando Registro
                                                result = tm.DeshabilitaTarifaMatriz(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                //Incrementando Contador
                                                contador += 1;
                                                //Guardando resultado de la Operación
                                                res1 = dt.Rows.Count > contador ? result.OperacionExitosa : false;
                                            }
                                            else
                                                //Asignando Negativo el Resultado
                                                res1 = false;
                                        }
                                    }

                                    //Validando si la Operación fue Exitosa
                                    if (result.OperacionExitosa)

                                        //Finalizando Transacción
                                        transaccion.Complete();
                                }

                                //Validando si la Operación fue Exitosa
                                if (result.OperacionExitosa)
                                {
                                    //Limpiando Controles
                                    limpiaControlesTarifaMatriz();

                                    //Habilitando Controles
                                    habilitarControlesTarifaMatriz(true);

                                    //Cargando Detalles
                                    cargaDetallesTarifaMatriz();
                                }

                                //Mostrando Mensaje de Operación
                                lblErrorMatriz.Text = result.Mensaje;
                            }
                            else
                                //Mostrando Mensaje de Error
                                lblErrorMatriz.Text = "No Existen Detalles con este Valor";
                        }
                        break;
                    }
            }
            
            //Alternando Ventana
            TSDK.ASP.ScriptServer.AlternarVentana(upbtnConfirmarEliminacion, upbtnConfirmarEliminacion.GetType(), "Ventana Modal", "contenedorVentanaConfirmacion", "ventanaConfirmacion");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarEliminacion_Click(object sender, EventArgs e)
        {
            //Alternando Ventana
            TSDK.ASP.ScriptServer.AlternarVentana(upbtnCancelarEliminacion, upbtnCancelarEliminacion.GetType(), "Ventana Modal", "contenedorVentanaConfirmacion", "ventanaConfirmacion");
        }

        #endregion

        #endregion

        #endregion

        #region Métodos

        /// <summary>
        /// Método Privado encargado de Inicializar la Forma
        /// </summary>
        private void inicializaForma()
        {   //Cargando Catalogos
            cargaCatalogos();
            //Habilitando Menu
            habilitaMenu();
            //Habilitando Controles
            habilitaControles();
            //Inicializando Valores
            inicializaValores();
        }
        /// <summary>
        /// Método Privado encargado de Cargar los Catalogos
        /// </summary>
        private void cargaCatalogos()
        {   //Cargando Catalogos
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTarifaBase, 15, "");
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlColumnas, 16, "Ninguna");
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlFilas, 16, "Ninguna");
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoReqDisp, "", 18);
        }
        /// <summary>
        /// Método Privado encargado de Habilitar el Menu
        /// </summary>
        private void habilitaMenu()
        {   //Validando Estatus de Session
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {   //Archivo
                        lkbNuevo.Enabled =
                        lkbAbrir.Enabled =
                        lkbGuardar.Enabled =
                        btnGuardar.Enabled =
                        lkbSalir.Enabled = true;
                        //Edicion
                        lkbEditar.Enabled = false;
                        //Herramientas
                        lkbBitacora.Enabled =
                        lkbReferencias.Enabled =
                        lkbArchivos.Enabled = false;

                        break;
                    }
                case Pagina.Estatus.Lectura:
                    {   //Archivo
                        lkbNuevo.Enabled =
                        lkbAbrir.Enabled =
                        lkbSalir.Enabled = true;
                        lkbGuardar.Enabled =
                        btnGuardar.Enabled = false;
                        //Edicion
                        lkbEditar.Enabled = true;
                        //Herramientas
                        lkbBitacora.Enabled =
                        lkbReferencias.Enabled =
                        lkbArchivos.Enabled = true;

                        break;
                    }
                case Pagina.Estatus.Edicion:
                    {   //Archivo
                        lkbNuevo.Enabled =
                        lkbAbrir.Enabled =
                        lkbGuardar.Enabled =
                        btnGuardar.Enabled =
                        lkbSalir.Enabled = true;
                        //Edicion
                        lkbEditar.Enabled = false;
                        //Herramientas
                        lkbBitacora.Enabled =
                        lkbReferencias.Enabled = true;
                        lkbArchivos.Enabled = false;

                        break;
                    }
            }
        }
        /// <summary>
        /// Método Privado encargado de Habilitar los Controles de la Forma
        /// </summary>
        private void habilitaControles()
        {   //Validando Estatus de Session
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {   //Habilitando Controles
                        txtDescripcion.Enabled =
                        ddlTarifaBase.Enabled =
                        ddlColumnas.Enabled =
                        ddlFilas.Enabled = 
                        txtCliente.Enabled =
                        txtValorUCargado.Enabled =
                        txtValorUVacio.Enabled =
                        txtFecIni.Enabled =
                        txtFecFin.Enabled =
                        btnGuardar.Enabled =
                        btnCancelar.Enabled = true;
                        //Habilitando Controles
                        habilitarControlesTarifaMatriz(false);
                        ucClasificacion.Enabled = 
                        ucCargoRecurrente.Enabled = false;

                        break;
                    }
                case Pagina.Estatus.Edicion:
                    {   //Habilitando Controles
                        txtDescripcion.Enabled =
                        ddlTarifaBase.Enabled =
                        txtCliente.Enabled =
                        txtValorUCargado.Enabled =
                        txtValorUVacio.Enabled =
                        txtFecIni.Enabled =
                        txtFecFin.Enabled = 
                        btnGuardar.Enabled = 
                        btnCancelar.Enabled = true;
                        //Habilitando Controles
                        habilitarControlesTarifaMatriz(true);
                        ucClasificacion.Enabled =
                        ucCargoRecurrente.Enabled = true;
                        //Deshabilitando Columna y Fila
                        ddlColumnas.Enabled =
                        ddlFilas.Enabled = false;

                        break;
                    }
                case Pagina.Estatus.Lectura:
                    {   //Deshabilitando Controles
                        txtDescripcion.Enabled =
                        ddlTarifaBase.Enabled =
                        ddlColumnas.Enabled =
                        ddlFilas.Enabled =
                        txtCliente.Enabled =
                        txtValorUCargado.Enabled =
                        txtValorUVacio.Enabled =
                        txtFecIni.Enabled =
                        txtFecFin.Enabled =
                        btnGuardar.Enabled =
                        btnCancelar.Enabled = false;
                        //Habilitando Controles
                        habilitarControlesTarifaMatriz(false);
                        ucClasificacion.Enabled = 
                        ucCargoRecurrente.Enabled = false;

                        break;
                    }
            }
        }
        /// <summary>
        /// Método Privado encargado de Inicializar los Valores
        /// </summary>
        private void inicializaValores()
        {   //Validando Estatus de Session
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {   //Asignando Valores
                        lblId.Text = "Por Asignar";
                        txtDescripcion.Text =
                        txtCliente.Text =
                        txtValorUCargado.Text =
                        txtValorUVacio.Text = "";
                        txtFecIni.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy");
                        txtFecFin.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().AddYears(1).ToString("dd/MM/yyyy");
                        txtTransportista.Text = "No Aplica ID:0";
                        //Instanciando Compania
                        using (SAT_CL.Global.CompaniaEmisorReceptor cer = new SAT_CL.Global.CompaniaEmisorReceptor(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                        {   //Validando si existe la Compania
                            if (cer.id_compania_emisor_receptor != 0)
                                //Asignando Descripción de la Compania
                                txtCompania.Text = cer.nombre + " ID:" + cer.id_compania_emisor_receptor.ToString();
                            else//Limpiando Valor
                                txtCompania.Text = "";
                        }
                        //Inicializando Control
                        ucClasificacion.InicializaControl(47, 0, Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:", 1)));
                        ucCargoRecurrente.InicializaControlUsuario(0, 0, Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:", 1)));
                        //Invocando Método de Limpieza
                        limpiaControlesTarifaMatriz();
                        TSDK.ASP.Controles.InicializaGridview(gvTarifamatriz);
                        break;
                    }
                case Pagina.Estatus.Lectura:
                case Pagina.Estatus.Edicion:
                    {   //Instanciando Tarifa
                        using(SAT_CL.Tarifas.Tarifa tar = new SAT_CL.Tarifas.Tarifa(Convert.ToInt32(Session["id_registro"])))
                        {   //Validando que exista la Tarifa
                            if(tar.id_tarifa != 0)
                            {   //Asignando Valores
                                lblId.Text = tar.id_tarifa.ToString();
                                txtDescripcion.Text = tar.descripcion;
                                //Instanciando Compania
                                using (SAT_CL.Global.CompaniaEmisorReceptor cer = new SAT_CL.Global.CompaniaEmisorReceptor(tar.id_compania_emisor))
                                {   //Validando si existe la Compania
                                    if (cer.id_compania_emisor_receptor != 0)
                                        //Asignando Descripción de la Compania
                                        txtCompania.Text = cer.nombre + " ID:" + cer.id_compania_emisor_receptor.ToString();
                                    else//Limpiando Valor
                                        txtCompania.Text = "";
                                }
                                //Instanciando Cliente
                                using(SAT_CL.Global.CompaniaEmisorReceptor cli = new SAT_CL.Global.CompaniaEmisorReceptor(tar.id_cliente_receptor))
                                {   //Validando que exista el Cliente
                                    if (cli.id_compania_emisor_receptor != 0)
                                        //Asignando Descripción
                                        txtCliente.Text = cli.nombre + " ID:" + cli.id_compania_emisor_receptor.ToString();
                                    else//Limpiando Valor
                                        txtCliente.Text = "";
                                }
                                txtTransportista.Text = "No Aplica ID:0";
                                txtValorUCargado.Text = tar.valor_unitario.ToString();
                                txtValorUVacio.Text = tar.valor_unitario_vacio.ToString();
                                txtFecIni.Text = tar.fecha_inicio.ToString("dd/MM/yyyy");
                                txtFecFin.Text = tar.fecha_fin.ToString("dd/MM/yyyy");
                                //Catalogos
                                ddlTarifaBase.SelectedValue = tar.id_base_tarifa.ToString();
                                ddlColumnas.SelectedValue = tar.id_columna_filtro_col.ToString();
                                ddlFilas.SelectedValue = tar.id_columna_filtro_row.ToString();
                                //Inicializando Control
                                ucClasificacion.InicializaControl(47, tar.id_tarifa, Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:", 1)));
                                ucCargoRecurrente.InicializaControlUsuario(tar.id_tarifa, 0, Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:", 1)));
                                //Cargando Detalles
                                cargaDetallesTarifaMatriz();
                                limpiaControlesTarifaMatriz();
                            }
                        }
                        break;
                    }
            }
        }
        /// <summary>
        /// Método Privado encargado de Guardar los Cambios en las Tarifas
        /// </summary>
        private void guardaTarifa()
        {   //Declarando Objeto de Retorno de Operación
            RetornoOperacion result = new RetornoOperacion();
            //Declarando Variable de Fecha
            DateTime FecIni, FecFin;
            //Obteniendo Fechas
            DateTime.TryParse(txtFecIni.Text + " 00:00", out FecIni);
            DateTime.TryParse(txtFecFin.Text + " 23:59", out FecFin);
            //Validando Fechas
            if (FecIni.CompareTo(FecFin) < 0)
            {      //Validando Estatus de Session
                    switch ((Pagina.Estatus)Session["estatus"])
                    {
                        case Pagina.Estatus.Nuevo:
                            {   //Inicializando Transacción
                                using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                                {   //Insertando Tarifa
                                    result = SAT_CL.Tarifas.Tarifa.InsertaTarifa(txtDescripcion.Text.ToUpper(), Convert.ToInt32(ddlTarifaBase.SelectedValue),
                                                    Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:", 1)),
                                                    Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCliente.Text, "ID:", 1)),
                                                    Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtTransportista.Text, "ID:", 1)),
                                                    Convert.ToDecimal(txtValorUCargado.Text), Convert.ToDecimal(txtValorUVacio.Text),
                                                    Convert.ToInt32(ddlColumnas.SelectedValue), Convert.ToInt32(ddlFilas.SelectedValue),
                                                    FecIni, FecFin, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                    //Validando que se Inserto la tarifa
                                    if (result.OperacionExitosa)
                                    {   //Declarando Objeto de Retorno de Clasificacion
                                        RetornoOperacion resultClasificacion = new RetornoOperacion();
                                        //Insertando Clasificación
                                        resultClasificacion = Clasificacion.InsertaClasificacion(47, result.IdRegistro, 0, 0, 0, 0, 0, 0, 0, 0, 0, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                        //Validando que se Inserto la Clasificación
                                        if (resultClasificacion.OperacionExitosa)
                                            //Finalizando Transaccion
                                            trans.Complete();
                                    }
                                }
                                break;
                            }
                        case Pagina.Estatus.Edicion:
                            {   //Instanciando Registro
                                using(SAT_CL.Tarifas.Tarifa tar = new SAT_CL.Tarifas.Tarifa(Convert.ToInt32(Session["id_registro"])))
                                {   //Validando que exista la Tarifa
                                    if(tar.id_tarifa != 0)
                                    {   //Editando la Tarifa
                                        result = tar.EditaTarifa(txtDescripcion.Text.ToUpper(), Convert.ToInt32(ddlTarifaBase.SelectedValue),
                                            Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:", 1)),
                                            Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCliente.Text, "ID:", 1)),
                                            Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtTransportista.Text, "ID:", 1)),
                                            Convert.ToDecimal(txtValorUCargado.Text), Convert.ToDecimal(txtValorUVacio.Text),
                                            Convert.ToInt32(ddlColumnas.SelectedValue), Convert.ToInt32(ddlFilas.SelectedValue),
                                            FecIni, FecFin, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                    }
                                }

                                break;
                            }
                    }
                    //Validando que la Operacion haya sido exitosa
                    if(result.OperacionExitosa)
                    {   //Asignando Valores
                        Session["id_registro"] = result.IdRegistro;
                        //Validando el Estatus de la Página
                        if ((Pagina.Estatus)Session["estatus"] == Pagina.Estatus.Nuevo)
                            //Asignando Estatus de Edición
                            Session["estatus"] = Pagina.Estatus.Edicion;
                        else//Asignando Estatus de Lectura
                            Session["estatus"] = Pagina.Estatus.Lectura;
                        //Inicializando Forma
                        inicializaForma();
                    }
                    //Mostrando Mensaje
                    lblError.Text = result.Mensaje;
            }
            else//Mostrando Error
                lblError.Text = "La Fecha de Inicio es posterior a la Fecha de Fin";
        }
        /// <summary>
        /// Método Privado encargado de Inicializar la Apertura de un Registro
        /// </summary>
        /// <param name="idTabla">Id de Tabla</param>
        private void inicializaAperturaRegistro(int idTabla, int idCompania)
        {   //Asignando Session
            Session["id_tabla"] = idTabla;
            //Construyendo URL 
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Tarifas/Tarifas.aspx", "~/Accesorios/AbrirRegistro.aspx?P1="+idCompania.ToString());
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=650";
            //Abriendo Nueva Ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "Apertura de Registro", configuracion, Page);
        }
        /// <summary>
        /// Método Privado encargado de Inicializar la Forma de Bitacora
        /// </summary>
        /// <param name="idRegistro">Id de Registro</param>
        /// <param name="idTabla">Id de Tabla</param>
        /// <param name="Titulo">Titulo</param>
        private void inicializaBitacora(string idRegistro, string idTabla, string Titulo)
        {   //Construyendo URL 
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Tarifas/Tarifas.aspx", "~/Accesorios/BitacoraRegistro.aspx?idT=" + idTabla + "&idR=" + idRegistro + "&tB=" + Titulo);
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=650";
            //Abriendo Nueva Ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "Bitacora del Registro", configuracion, Page);
        }
        /// <summary>
        /// Método encargado de Referenciar el Registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="id_compania">Id de Compania</param>
        private void inicializaReferenciaRegistro(string id_registro, string id_tabla, string id_compania)
        {   //Declarando variable para armado de URL
            string urlDestino = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Tarifas/Tarifas.aspx", "~/Accesorios/ReferenciaRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idC=" + id_compania);
            //Instanciando nueva ventana de navegador para apertura de referencias de registro
            TSDK.ASP.ScriptServer.AbreNuevaVentana(urlDestino, "Referencia", 800, 650, false, false, false, true, true, Page);
        }
        /// <summary>
        /// Método Privado encargado de Obtener los Valor de las Filas y Columnas
        /// </summary>
        /// <param name="val_desc_col"></param>
        /// <param name="val_desc_row"></param>
        /// <param name="val_id_col"></param>
        /// <param name="val_id_row"></param>
        private void obtieneValoresColumnaFila(out string val_desc_col, out string val_desc_row,
                                               out string val_id_col, out string val_id_row)
        {   //Inicializando valores
            val_desc_col = val_desc_row = val_id_col = val_id_row = "";
            //Validando el Tipo de Columna
            switch ((SAT_CL.Tarifas.Tarifa.CriterioMatrizTarifa)Convert.ToInt32(ddlColumnas.SelectedValue))
            {   //Opciones de Catalogo
                case SAT_CL.Tarifas.Tarifa.CriterioMatrizTarifa.Producto:
                case SAT_CL.Tarifas.Tarifa.CriterioMatrizTarifa.Ciudad:
                case SAT_CL.Tarifas.Tarifa.CriterioMatrizTarifa.Ubicacion:
                    {   //Asignando Valores
                        val_desc_col = TSDK.Base.Cadena.RegresaCadenaSeparada(txtCatCol.Text, " ID:", 0);
                        val_id_col = TSDK.Base.Cadena.RegresaCadenaSeparada(txtCatCol.Text, " ID:", 1);
                        break;
                    }
                    //Opciones de Captura
                case SAT_CL.Tarifas.Tarifa.CriterioMatrizTarifa.Paradas:
                case SAT_CL.Tarifas.Tarifa.CriterioMatrizTarifa.Distancia:
                case SAT_CL.Tarifas.Tarifa.CriterioMatrizTarifa.Peso:
                case SAT_CL.Tarifas.Tarifa.CriterioMatrizTarifa.Volumen:
                case SAT_CL.Tarifas.Tarifa.CriterioMatrizTarifa.Cantidad:
                    {   //Asignando Valores
                        val_desc_col = txtRotCol.Text;
                        val_id_col = txtRotCol.Text;
                        break;
                    }
            }
            //Validando el Tipo de Fila
            switch ((SAT_CL.Tarifas.Tarifa.CriterioMatrizTarifa)Convert.ToInt32(ddlFilas.SelectedValue))
            {   //Opciones de Catalogo
                case SAT_CL.Tarifas.Tarifa.CriterioMatrizTarifa.Producto:
                case SAT_CL.Tarifas.Tarifa.CriterioMatrizTarifa.Ciudad:
                case SAT_CL.Tarifas.Tarifa.CriterioMatrizTarifa.Ubicacion:
                case SAT_CL.Tarifas.Tarifa.CriterioMatrizTarifa.NoAplica:
                    {   //Asignando Valores
                        val_desc_row = TSDK.Base.Cadena.RegresaCadenaSeparada(txtCatFila.Text, " ID:", 0);
                        val_id_row = TSDK.Base.Cadena.RegresaCadenaSeparada(txtCatFila.Text, " ID:", 1);
                        break;
                    }
                //Opciones de Captura
                case SAT_CL.Tarifas.Tarifa.CriterioMatrizTarifa.Paradas:
                case SAT_CL.Tarifas.Tarifa.CriterioMatrizTarifa.Distancia:
                case SAT_CL.Tarifas.Tarifa.CriterioMatrizTarifa.Peso:
                case SAT_CL.Tarifas.Tarifa.CriterioMatrizTarifa.Volumen:
                case SAT_CL.Tarifas.Tarifa.CriterioMatrizTarifa.Cantidad:
                    {   //Asignando Valores
                        val_desc_row = txtRotFila.Text;
                        val_id_row = txtRotFila.Text;
                        break;
                    }
            }
        }

        #region Métodos Tarifa Matriz (Detalles)

        /// <summary>
        /// Método Privado encargado de Cargar los Detalles de las Tarifas
        /// </summary>
        private void cargaDetallesTarifaMatriz()
        {   //Obteniendo Detalles de las Tarifas
            using(DataTable dtDetallesTarifa = SAT_CL.Tarifas.TarifaMatriz.ObtieneMatrizTarifa(Convert.ToInt32(Session["id_registro"])))
            {   //Validando que existan registros
                if(TSDK.Datos.Validacion.ValidaOrigenDatos(dtDetallesTarifa))
                {   //Cargando GridView
                    TSDK.ASP.Controles.CargaGridView(gvTarifamatriz, dtDetallesTarifa, "Id", "");
                    //Añadiendo Tabla a DataSet de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtDetallesTarifa, "Table");
                }
                else
                {   //Inicializando GridView
                    TSDK.ASP.Controles.InicializaGridview(gvTarifamatriz);
                    //Eliminando Tabla de DataSet de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                }
            }
        }
        /// <summary>
        /// Método Privado encargado de Limpiar los Controles
        /// </summary>
        private void limpiaControlesTarifaMatriz()
        {   
            //Limpiando Controles
            txtRotCol.Text =
            txtRotFila.Text =
            txtCatCol.Text =
            txtCatFila.Text =
            txtTarifaCargado.Text =
            txtTarifaVacio.Text = "";
            //Quitando Selección del GridView
            gvTarifamatriz.SelectedIndex = -1;
            //Mostrando Mensaje
            lblTipoCargoRecurrente.Text = "Ligado a la Tarifa";
        }
        /// <summary>
        /// Método Privado que Habilita los Controles de los Detalles
        /// </summary>
        /// <param name="enable">Habilitacion de Controles</param>
        private void habilitarControlesTarifaMatriz(bool enable)
        {   //Asignando Habilitación de Controles
            txtRotCol.Enabled =
            txtRotFila.Enabled =
            txtCatCol.Enabled =
            txtCatFila.Enabled = 
            imgbtnQuitarColumna.Enabled =
            imgbtnQuitarFila.Enabled =
            txtTarifaCargado.Enabled =
            txtTarifaVacio.Enabled =
            btnGuardarMatriz.Enabled =
            btnCancelarMatriz.Enabled =
            btnEliminarMatriz.Enabled =
            ddlTamanoReqDisp.Enabled =
            lnkExportar.Enabled =
            gvTarifamatriz.Enabled = enable;
        }
        /// <summary>
        /// Método Privado encargado de Cargar la Matriz de los Detalles de la Tarifa
        /// </summary>
        private void cargaMatrizDetallesTarifa()
        {   
            //Obteniendo Detalles para la Matriz
            using (DataSet dsDetalles = SAT_CL.Tarifas.TarifaMatriz.ObtieneDetallesMatrizTarifa(Convert.ToInt32(Session["id_registro"])))
            {   
                //Validando que exista contenido en las Tablas
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dsDetalles, "Table") && TSDK.Datos.Validacion.ValidaOrigenDatos(dsDetalles, "Table1"))
                {   
                    //Obteniendo Matriz en Blanco
                    DataTable dtDetallesMatrizCargado = creaTabla(dsDetalles.Tables["Table"], dsDetalles.Tables["Table1"]);
                    DataTable dtDetallesMatrizVacio = dtDetallesMatrizCargado.Copy();
                    
                    //Validando que exista una Matriz
                    if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtDetallesMatrizCargado) && TSDK.Datos.Validacion.ValidaOrigenDatos(dsDetalles, "Table2"))
                    {   
                        //Recorriendo cada Fila
                        foreach(DataRow dr in dsDetalles.Tables["Table2"].Rows)
                        {   
                            //Editando Celdas
                            dtDetallesMatrizCargado.Rows[Convert.ToInt32(dr["PosX"])][Convert.ToInt32(dr["PosY"])] = dr["TarCargado"].ToString();
                            dtDetallesMatrizVacio.Rows[Convert.ToInt32(dr["PosX"])][Convert.ToInt32(dr["PosY"])] = dr["TarVacio"].ToString();
                        }
                        
                        //Eliminando la Primera Fila --Opcional
                        dtDetallesMatrizCargado.Rows[0].Delete();
                        dtDetallesMatrizVacio.Rows[0].Delete();
                        
                        //Cargando GridView
                        TSDK.ASP.Controles.CargaGridView(gvMatrizCargada, dtDetallesMatrizCargado, "", "");
                        TSDK.ASP.Controles.CargaGridView(gvMatrizVacia, dtDetallesMatrizVacio, "", "");
                    }
                }
            }
        }
        /// <summary>
        /// Método Privado encargado de Crear la Tabla de la Matriz en Ceros
        /// </summary>
        /// <param name="Columnas">Conjunto de Columnas Agrupadas</param>
        /// <param name="Filas">Conjunto de Filas Agrupadas</param>
        /// <returns></returns>
        private DataTable creaTabla(DataTable Columnas, DataTable Filas)
        {   
            //Declarando Objeto de Retorno
            DataTable dtDetallesMatriz = new DataTable();
            
            //Validando que exista contenido en las Tablas
            if (TSDK.Datos.Validacion.ValidaOrigenDatos(Columnas) && TSDK.Datos.Validacion.ValidaOrigenDatos(Filas))
            {   
                //Creando la Primer Columna
                dtDetallesMatriz.Columns.Add("Filas/Columnas", typeof(string));
                
                //Iniciando Ciclo para Crear Columnas
                foreach(DataRow dr in Columnas.Rows)
                    //Creando Columna
                    dtDetallesMatriz.Columns.Add(dr["Columnas"].ToString());
                
                //Creando Arreglo Dinamico
                string[] param = new string[Columnas.Rows.Count + 1];
                
                //Declarando Contador
                int count = 1;
                
                //Iniciando Ciclo para Crear Columnas
                foreach (DataRow dr in Columnas.Rows)
                {   
                    //Creando
                    param[count] = dr["Columnas"].ToString();
                    //Incrementando Contador
                    count++;
                }
                
                //Añadiendo Primer Fila con los Nombres de las Columnas
                dtDetallesMatriz.Rows.Add(param);
                
                //Ciclo para llenar el Arreglo de Datos Vacios
                for (int i = 1; i <= Columnas.Rows.Count; i++)
                    //Asignando Valor
                    param[i] = "0.00";

                //Iniciando Ciclo para Crear Filas
                foreach (DataRow dr in Filas.Rows)
                {   
                    //Asignando el Nombre de la Fila
                    param[0] = dr["Filas"].ToString();
                    //Añadiendo Primer Fila con los Nombres de las Columnas
                    dtDetallesMatriz.Rows.Add(param);
                }
            }
            //Devolviendo Resultado Obtenido
            return dtDetallesMatriz;
        }

        #endregion

        #endregion        
    }
}