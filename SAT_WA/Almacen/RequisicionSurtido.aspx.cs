using SAT_CL.Almacen;
using System;
using System.Data;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.Almacen
{
    public partial class RequisicionSurtido : System.Web.UI.Page
    {
        #region Eventos

        /// <summary>
        /// Evento Producido al Efectarse una Llamada al Servidor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Validando que se Produjo un PostBack
            if (!Page.IsPostBack)

                //Inicializando Página
                inicializaPagina();
        }
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
                case "ProductoDesempaquetado":
                    {
                        //Invocando Método de Gestión
                        gestionaVentanaModal(lkbCerrar, lkbCerrar.CommandName);
                        break;
                    }
                case "SurtirProductoManual":
                    {
                        //Invocando Método de Gestión
                        gestionaVentanaModal(lkbCerrar, lkbCerrar.CommandName);
                        break;
                    }
                case "CantidadProducto":
                    {
                        //Invocando Método de Gestión
                        gestionaVentanaModal(lkbCerrar, lkbCerrar.CommandName);
                        break;
                    }
            }
        }
        /// <summary>
        /// Evento Producido al Dar Click en el Boton "Surtir"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSurtir_Click(object sender, EventArgs e)
        {
            //Validando que exista una Fila Seleccionada
            if (gvProductosRequeridos.SelectedIndex != -1)
            {
                //Declarando Objeto de Retorno
                RetornoOperacion result = new RetornoOperacion();

                //Obteniendo Cantidad
                decimal cantidad = Convert.ToDecimal(txtCantidadReq.Text == "" ? "0" : txtCantidadReq.Text);

                //Declarando Variable Auxiliar
                int idDetalleReq = 0;
                
                //Validando que no sea 0
                if (cantidad > 0)
                {
                    //Validando Cantidad Disponible en el Inventario
                    if (Convert.ToDecimal(gvProductosRequeridos.SelectedDataKey["Disponibles"]) >= cantidad)
                    {
                        //Validando que la Cantidad no Exceda el Valor Permitido
                        if (Convert.ToDecimal(gvProductosRequeridos.SelectedDataKey["Cantidad"]) >= cantidad)
                        {
                            //Inicializando Bloque Transaccional
                            using (TransactionScope trans = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                            {
                                //Instanciando Requisición
                                using (Requisicion req = new Requisicion(Convert.ToInt32(Session["id_registro"])))
                                
                                    //Instanciando Detalles
                                using (RequisicionDetalle reqDet = new RequisicionDetalle(Convert.ToInt32(gvProductosRequeridos.SelectedDataKey["Id"])))
                                {
                                    //Validando que exista el Detalle y la Requisición
                                    if (reqDet.Habilitar && req.habilitar)
                                    {
                                        //Declarando Estatus
                                        RequisicionDetalle.EstatusDetalle estatus;

                                        //Obteniendo Estatus
                                        estatus = Convert.ToDecimal(gvProductosRequeridos.SelectedDataKey["Cantidad"]) == cantidad ? RequisicionDetalle.EstatusDetalle.Cerrado : RequisicionDetalle.EstatusDetalle.AbastecidoParcial;

                                        //Actualizando Estatus
                                        result = reqDet.EditaEstatus(estatus, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                        //Validando Operación Exitosa
                                        if (result.OperacionExitosa)
                                        {
                                            //Obteniendo Valor
                                            idDetalleReq = result.IdRegistro;

                                            //Insertando Salida
                                            result = SAT_CL.Almacen.EntradaSalida.InsertarEntradaSalida(EntradaSalida.TipoOperacion.Salida, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, 140,
                                                                reqDet.IdDetalleRequisicion, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), 1, req.id_almacen, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                            //Validando Operación Exitosa
                                            if (result.OperacionExitosa)
                                            {
                                                //Obteniendo Salida
                                                int idSalida = result.IdRegistro;

                                                //Actualizando Inventario
                                                result = SAT_CL.Almacen.Inventario.ReducirExistencias(reqDet.IdProducto, cantidad, req.id_almacen, idSalida, 140, reqDet.IdDetalleRequisicion, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                            }
                                        }
                                    }

                                    //Validando Operación Exitosa
                                    if (result.OperacionExitosa)
                                    {
                                        //Instanciando Detalle
                                        result = new RetornoOperacion(idDetalleReq);

                                        //Completando Transacción
                                        trans.Complete();
                                    }
                                }
                            }

                            //Operación Exitosa?
                            if (result.OperacionExitosa)
                            {
                                //Inicializando Valores
                                inicializaValores();

                                //Marcando Fila
                                Controles.MarcaFila(gvProductosRequeridos, idDetalleReq.ToString(), "Id", "Id-IdProducto-Cantidad-Disponibles", OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), lblOrdenado.Text, Convert.ToInt32(ddlTamano.SelectedValue), true, 1);

                                //Ocultando Ventana Modal
                                gestionaVentanaModal(this, "CantidadProducto");
                            }
                        }
                        else
                            //Instanciando Excepción
                            result = new RetornoOperacion(string.Format("La Cantidad Ingresada '{0:0.00}', excede la cantidad permitida '{1:0.00}'", cantidad, gvProductosRequeridos.SelectedDataKey["Cantidad"]));
                    }
                    else
                        //Instanciando Excepción
                        result = new RetornoOperacion(string.Format("La Cantidad Ingresada '{0:0.00}', excede la cantidad disponible en el Inventario", cantidad, gvProductosRequeridos.SelectedDataKey["Disponibles"]));
                }
                else
                    //Instanciando Excepción
                    result = new RetornoOperacion("No se puede abastecer la Cantidad '0.00'");

                //Mostrando Resultado de la Operación
                ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
        }
        /// <summary>
        /// Evento Producido al Dar Click en el Boton "Abastecer"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSurtidoManual_Click(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Declarando Variables Auxiliares
            decimal cantidad_total = 0.00M;
            int idSalida = 0;

            //Declarando Cantidades
            decimal cantidad_inicial = 0.00M;

            //Si la cantidad en Inventario es mayor que la Cantidad Solicitada
            if (Convert.ToDecimal(gvProductosRequeridos.SelectedDataKey["Cantidad"]) <= Convert.ToDecimal(gvProductosRequeridos.SelectedDataKey["Disponibles"]))

                //Asignando Cantidad Inicial
                cantidad_inicial = Convert.ToDecimal(gvProductosRequeridos.SelectedDataKey["Cantidad"]);
            else
                //Asignando Cantidad Disponible en Inventario
                cantidad_inicial = Convert.ToDecimal(gvProductosRequeridos.SelectedDataKey["Disponibles"]);

            //Inicializando Bloque Transaccional
            using(TransactionScope trans = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciando Requisición
                using (Requisicion req = new Requisicion(Convert.ToInt32(Session["id_registro"])))
                //Instanciando Detalles
                using (RequisicionDetalle reqDet = new RequisicionDetalle(Convert.ToInt32(gvProductosRequeridos.SelectedDataKey["Id"])))
                {
                    //Validando que exista el Detalle y la Requisición
                    if (reqDet.Habilitar && req.habilitar)
                    {
                        //Validando que existan Registros
                        if (Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2")))
                        {
                            //Obteniendo Cantidad Total por 
                            cantidad_total = Convert.ToDecimal((((DataSet)Session["DS"]).Tables["Table2"].Compute("SUM(CantidadDeseada)", "")));

                            //Declarando Estatus
                            RequisicionDetalle.EstatusDetalle estatus;

                            //Obteniendo Estatus
                            estatus = cantidad_total == 0 ? (RequisicionDetalle.EstatusDetalle)reqDet.IdEstatus : cantidad_inicial == cantidad_total ? RequisicionDetalle.EstatusDetalle.Cerrado : RequisicionDetalle.EstatusDetalle.AbastecidoParcial;

                            //Actualizando Estatus
                            result = reqDet.EditaEstatus(estatus, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                            //Validando que Exista Cantidad por Surtir
                            if (cantidad_total > 0)
                            {
                                //Validando Operación Exitosa
                                if (result.OperacionExitosa)
                                {
                                    //Obteniendo Valor
                                    int idDetalleReq = result.IdRegistro;

                                    //Insertando Salida
                                    result = SAT_CL.Almacen.EntradaSalida.InsertarEntradaSalida(EntradaSalida.TipoOperacion.Salida, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, 140,
                                                        reqDet.IdDetalleRequisicion, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), 1, req.id_almacen, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                    //Validando Operación Exitosa
                                    if (result.OperacionExitosa)

                                        //Obteniendo Salida
                                        idSalida = result.IdRegistro;
                                }

                                //Validando que las Operaciones fuesen Exitosas
                                if (result.OperacionExitosa)
                                {
                                    //Recorriendo Registros
                                    foreach (DataRow dr in OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2").Rows)
                                    {
                                        //Validando que hay Cantidad por Abastecer
                                        if (Convert.ToDecimal(dr["CantidadDeseada"]) > 0)
                                        {
                                            //Instanciando Inventario
                                            using (Inventario inv = new Inventario(Convert.ToInt32(dr["Id"])))
                                            {
                                                //Validando que existe el Inventario
                                                if (inv.habilitar)
                                                {
                                                    //Extrayendo Contenido
                                                    result = inv.ReducirExistencias(Convert.ToDecimal(dr["CantidadDeseada"]), idSalida, 140, reqDet.IdDetalleRequisicion, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                                    //Si la Operación no fue Correcta
                                                    if (!result.OperacionExitosa)

                                                        //Terminando Ciclo
                                                        break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                                //Instanciando Excepción
                                result = new RetornoOperacion("No existe cantidad por Surtir");
                        }
                        else
                            //Instanciando Excepción
                            result = new RetornoOperacion("No existen registros de Inventario");                        

                        //Validando que las Operaciones fuesen Exitosas
                        if (result.OperacionExitosa)

                            //Completando Transacción
                            trans.Complete();
                    }
                    else
                        //Instanciando Excepción
                        result = new RetornoOperacion("No existe la Requisición");
                }
            }

            //Si la Operación fue Exitosa
            if (result.OperacionExitosa)
            {
                //Inicializando Valores
                inicializaValores();
                
                //Cargando Productos Requeridos
                cargaProductosRequeridos();
                
                //Cerrando ventana Modal
                gestionaVentanaModal(this, "SurtirProductoManual");
            }

            //Mostrando Resultado
            ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        #region Eventos GridView "Requisiciones Pendientes"

        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView "Requisiciones Pendientes"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamano_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando tamaño del GridView
            Controles.CambiaTamañoPaginaGridView(gvProductosRequeridos, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamano.SelectedValue));
        }
        /// <summary>
        /// Evento Producido al Exportar al Contenido del GridView "Requisiciones Pendientes"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportar_Click(object sender, EventArgs e)
        {
            //Exportando Contenido del GridView
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "Id-IdProducto");
        }
        /// <summary>
        /// Evento Producido al Cambiar la Expresión del Ordenamiento del GridView "Requisiciones Pendientes"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvProductosRequeridos_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión del Ordenamiento
            lblOrdenado.Text = Controles.CambiaSortExpressionGridView(gvProductosRequeridos, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression);
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Página del GridView "Requisiciones Pendientes"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvProductosRequeridos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            Controles.CambiaIndicePaginaGridView(gvProductosRequeridos, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbRelacionados_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if(gvProductosRequeridos.DataKeys.Count > 0)
            {
                //Obteniendo Control
                LinkButton lkb = (LinkButton)sender;

                //Seleccionando Fila
                Controles.SeleccionaFila(gvProductosRequeridos, sender, "lnk", false);

                //Cargando Productos
                cargaProductosEmpaquetados(Convert.ToInt32(gvProductosRequeridos.SelectedDataKey["IdProducto"]));

                //Mostrando Ventana Modal
                gestionaVentanaModal(lkb, "ProductoDesempaquetado");
            }
        }
        /// <summary>
        /// Evento Producido al Surtir el Producto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbSurtir_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvProductosRequeridos.DataKeys.Count > 0)
            {
                //Obteniendo Control
                LinkButton lkb = (LinkButton)sender;

                //Seleccionando Fila
                Controles.SeleccionaFila(gvProductosRequeridos, sender, "lnk", false);

                //Instanciando Detalle
                using (RequisicionDetalle rd = new RequisicionDetalle(Convert.ToInt32(gvProductosRequeridos.SelectedDataKey["Id"])))
                {
                    //Validando que existe el Detalle
                    if ((RequisicionDetalle.EstatusDetalle)rd.IdEstatus != RequisicionDetalle.EstatusDetalle.Cerrado &&
                        (RequisicionDetalle.EstatusDetalle)rd.IdEstatus != RequisicionDetalle.EstatusDetalle.Cancelado)
                    {
                        //Declarando variable Auxiliar
                        decimal cantidad = 0.00M;

                        //Validando que la Cantidad sea Menor o igual que la Cantidad Disponible en el Inventario
                        if (Convert.ToDecimal(gvProductosRequeridos.SelectedDataKey["Cantidad"]) <= Convert.ToDecimal(gvProductosRequeridos.SelectedDataKey["Disponibles"]))

                            //Asignando Cantidad Requerida
                            cantidad = Convert.ToDecimal(gvProductosRequeridos.SelectedDataKey["Cantidad"]);
                        else
                            //Asignando Cantidad Requerida
                            cantidad = Convert.ToDecimal(gvProductosRequeridos.SelectedDataKey["Disponibles"]);

                        //Validando Comando
                        switch (lkb.CommandName)
                        {
                            case "Automatico":
                                {
                                    //Asignando Cantidad Requerida
                                    txtCantidadReq.Text = cantidad.ToString();

                                    //Mostrando Ventana Modal
                                    gestionaVentanaModal(lkb, "CantidadProducto");
                                    break;
                                }
                            case "Manual":
                                {
                                    //Asignando Cantidad Inicial
                                    lblCantidadI.Text = cantidad.ToString();
                                    lblCantidadS.Text =
                                    lblCantidadR.Text = "0.00";

                                    //Cargando Productos del Inventario
                                    cargaProductosInventario(cargaProductosManual(Convert.ToInt32(gvProductosRequeridos.SelectedDataKey["IdProducto"])), 0.00M);

                                    //Mostrando Ventana Modal
                                    gestionaVentanaModal(lkb, "SurtirProductoManual");
                                    break;
                                }
                        }
                    }
                    else
                    {
                        //Declarando Mensaje de Excepción
                        string mensaje = "";
                        ScriptServer.NaturalezaNotificacion naturaleza = ScriptServer.NaturalezaNotificacion.Informacion;
                        
                        //Validando Estatus
                        switch((RequisicionDetalle.EstatusDetalle)rd.IdEstatus)
                        {
                            case RequisicionDetalle.EstatusDetalle.Cancelado:
                                    mensaje = "El Detalle ha sido Cancelado";
                                    naturaleza = ScriptServer.NaturalezaNotificacion.Error;
                                    break;
                            case RequisicionDetalle.EstatusDetalle.Cerrado:
                                    mensaje = "El Detalle ha sido Abastecido";
                                    naturaleza = ScriptServer.NaturalezaNotificacion.Informacion;
                                    break;
                        }

                        //Mostrando Mensaje
                        ScriptServer.MuestraNotificacion(this, mensaje, naturaleza, ScriptServer.PosicionNotificacion.AbajoDerecha);
                    }
                        
                }
            }
        }

        #endregion

        #region Eventos GridView "Desempaquetar Producto"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoProdDes_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Validando que existen Registros
            if (gvDesempaquetarProducto.DataKeys.Count > 0)
            {
                //Recorriendo Filas
                foreach(DataRow dr in OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1").Rows)
                {
                    //Asignando Cantidad en '0.00'
                    dr["CantidadDeseada"] = "0.00";
                }

                //Aceptando Cambios
                OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1").AcceptChanges();

                //Cambiando tamaño del GridView
                Controles.CambiaTamañoPaginaGridView(gvDesempaquetarProducto, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), Convert.ToInt32(ddlTamanoProdDes.SelectedValue));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportarProdDes_Click(object sender, EventArgs e)
        {
            //Exportando Contenido del GridView
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), "Id");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDesempaquetarProducto_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Validando que existen Registros
            if (gvDesempaquetarProducto.DataKeys.Count > 0)
            {
                //Recorriendo Filas
                foreach (DataRow dr in OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1").Rows)
                {
                    //Asignando Cantidad en '0.00'
                    dr["CantidadDeseada"] = "0.00";
                }

                //Aceptando Cambios
                OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1").AcceptChanges();

                //Cambiando Indice de Página
                Controles.CambiaIndicePaginaGridView(gvDesempaquetarProducto, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.NewPageIndex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDesempaquetarProducto_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Validando que existen Registros
            if (gvDesempaquetarProducto.DataKeys.Count > 0)
            {
                //Recorriendo Filas
                foreach (DataRow dr in OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1").Rows)
                
                    //Asignando Cantidad en '0.00'
                    dr["CantidadDeseada"] = "0.00";

                //Aceptando Cambios
                OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1").AcceptChanges();

                //Cambiando Expresión del Ordenamiento
                lblOrdenadoProdDes.Text = Controles.CambiaSortExpressionGridView(gvDesempaquetarProducto, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.SortExpression);
            }
        }
        /// <summary>
        /// Evento Producido al Dar Click en el Link "Desempaquetar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbDesempaquetar_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if(gvDesempaquetarProducto.DataKeys.Count > 0)
            {
                //Declarando Objeto de Retorno
                RetornoOperacion result = new RetornoOperacion();
                
                //Seleccionando Fila
                Controles.SeleccionaFila(gvDesempaquetarProducto, sender, "lnk", false);

                //Instanciando Inventario
                using(Inventario inv = new Inventario(Convert.ToInt32(Convert.ToInt32(gvDesempaquetarProducto.SelectedDataKey["Id"]))))
                {
                    //Validando que Exista el Inventario
                    if (inv.habilitar)
                    {
                        //Validando que existan Productos por Desempaquetar
                        if (Convert.ToDecimal(gvDesempaquetarProducto.SelectedDataKey["CantidadDeseada"]) > 0)
                        {
                            //Desempaquetando Producto
                            result = inv.ExtraerContenido(Convert.ToDecimal(gvDesempaquetarProducto.SelectedDataKey["CantidadDeseada"]), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                            //Validando Operación Exitosa
                            if (result.OperacionExitosa)
                            {
                                //Obteniendo Producto Empaquetado
                                int idProducto = Convert.ToInt32(gvProductosRequeridos.SelectedDataKey["IdProducto"]);

                                //Cargando Productos Empaquetados
                                cargaProductosEmpaquetados(idProducto);

                                //Inicializando Indices
                                Controles.InicializaIndices(gvDesempaquetarProducto);

                                //Cargando Productos Requeridos
                                cargaProductosRequeridos();

                                //Marcando Fila
                                Controles.MarcaFila(gvProductosRequeridos, idProducto.ToString(), "Id", "Id-IdProducto-Cantidad-Disponibles", OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), lblOrdenado.Text, Convert.ToInt32(ddlTamano.SelectedValue), true, 1);
                            }
                        }
                        else
                            //Instanciando Excepción
                            result = new RetornoOperacion("No Existen Productos por Abastecer");

                        //Mostrando Resultado
                        ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
                    }
                }
            }
        }
        /// <summary>
        /// Evento Producido al Dar Click en el Boton "Cambiar/Guardar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCambiarCantidadDesemp_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvDesempaquetarProducto.DataKeys.Count > 0)
            {
                //Obteniendo Boton
                LinkButton lkb = (LinkButton)sender;

                //Seleccionando Fila
                Controles.SeleccionaFila(gvDesempaquetarProducto, sender, "lnk", false);

                //Obteniendo Controles
                using (TextBox txt = (TextBox)gvDesempaquetarProducto.SelectedRow.FindControl("txtCantidadDeseada"))
                using (LinkButton lkbDes = (LinkButton)gvDesempaquetarProducto.SelectedRow.FindControl("lkbDesempaquetar"))
                {
                    //Validando Comando
                    switch (lkb.CommandName)
                    {
                        case "Cambiar":
                            {
                                //Validando que exista el Control
                                if (txt != null)
                                {
                                    //Habilitando el Control
                                    txt.Enabled = true;

                                    //Obteniendo Control
                                    LinkButton lnk = (LinkButton)sender;

                                    //Configurando Controles
                                    lkb.Text = lkb.CommandName = "Guardar";
                                    lkbDes.Enabled = false;
                                }

                                break;
                            }
                        case "Guardar":
                            {
                                //Validando que exista el Control
                                if (txt != null)
                                {
                                    //Declarando Objeto de Retorno
                                    RetornoOperacion result = new RetornoOperacion(0);

                                    //Recorriendo Registros
                                    foreach (DataRow dr in ((DataSet)Session["DS"]).Tables["Table1"].Select("Id = " + gvDesempaquetarProducto.SelectedDataKey["Id"].ToString()))
                                    {
                                        //Realizando Validación
                                        if (Convert.ToDecimal(dr["Cantidad"]) >= Convert.ToDecimal(txt.Text == "" ? "0" : txt.Text) ? true : false)

                                            //Actualizando Registro
                                            dr["CantidadDeseada"] = string.Format("{0:0.00}", txt.Text == "" ? "0" : txt.Text);
                                        else
                                        {
                                            //Mostrando Monto Maximo Permitido
                                            txt.Text = string.Format("{0:0.00}", dr["Cantidad"]);
                                            dr["CantidadDeseada"] = string.Format("{0:0.00}", dr["Cantidad"]);

                                            //Instanciando Excepción
                                            result = new RetornoOperacion(string.Format("La Cantidad excede el Monto de {0:0.00}", dr["Cantidad"]));
                                        }

                                        //Actualizando Cambios
                                        ((DataSet)Session["DS"]).Tables["Table1"].AcceptChanges();

                                        //Inicializando Indices
                                        Controles.InicializaIndices(gvDesempaquetarProducto);

                                        //Cargando Grid
                                        Controles.CargaGridView(gvDesempaquetarProducto, ((DataSet)Session["DS"]).Tables["Table1"], "Id-CantidadDeseada", lblOrdenado.Text, true, 1);

                                        //Obteniendo Control
                                        LinkButton lnk = (LinkButton)sender;

                                        //Deshabilitando el Control
                                        txt.Enabled = false;

                                        //Configurando Controles
                                        lnk.Text = lnk.CommandName = "Cambiar";
                                        lkbDes.Enabled = true;
                                    }

                                    //Validando que la Operación no fuese Exitosa
                                    if (!result.OperacionExitosa)

                                        //Mostrando Resultado
                                        ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
                                }

                                break;
                            }
                    }
                }
            }
        }

        #endregion

        #region Eventos GridView "Producto Inventario"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoProdInv_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Validando que existen Registros
            if (gvProductoInventario.DataKeys.Count > 0)
            {
                //Recorriendo Filas
                foreach (DataRow dr in OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2").Rows)

                    //Asignando Cantidad en '0.00'
                    dr["CantidadDeseada"] = "0.00";

                //Aceptando Cambios
                OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2").AcceptChanges();

                //Cambiando tamaño del GridView
                Controles.CambiaTamañoPaginaGridView(gvProductoInventario, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), Convert.ToInt32(ddlTamanoProdDes.SelectedValue));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportarProdInv_Click(object sender, EventArgs e)
        {
            //Exportando Contenido del GridView
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), "Id");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCambiar_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvProductoInventario.DataKeys.Count > 0)
            {
                //Obteniendo Boton
                LinkButton lkb = (LinkButton)sender;
                
                //Seleccionando Fila
                Controles.SeleccionaFila(gvProductoInventario, sender, "lnk", false);

                //Declarando Cantidades
                decimal cantidad_inicial = 0.00M, cantidad_utilizada = 0.00M, cantidad_permitida = 0.00M;

                //Si la cantidad en Inventario es mayor que la Cantidad Solicitada
                if (Convert.ToDecimal(gvProductosRequeridos.SelectedDataKey["Cantidad"]) <= Convert.ToDecimal(gvProductosRequeridos.SelectedDataKey["Disponibles"]))

                    //Asignando Cantidad Inicial
                    cantidad_inicial = Convert.ToDecimal(gvProductosRequeridos.SelectedDataKey["Cantidad"]);
                else
                    //Asignando Cantidad Disponible en Inventario
                    cantidad_inicial = Convert.ToDecimal(gvProductosRequeridos.SelectedDataKey["Disponibles"]);

                //Asignando Cantidad Utilizada
                cantidad_utilizada = Convert.ToDecimal((((DataSet)Session["DS"]).Tables["Table2"].Compute("SUM(CantidadDeseada)", "")));

                //Asignando Cantidad Permitida
                cantidad_permitida = cantidad_inicial - cantidad_utilizada;
                
                //Obteniendo Controles
                using (TextBox txt = (TextBox)gvProductoInventario.SelectedRow.FindControl("txtCantidadDeseada"))
                {
                    //Validando Comando
                    switch (lkb.CommandName)
                    {
                        case "Cambiar":
                            {
                                //Recorriendo Registros
                                foreach (DataRow dr in ((DataSet)Session["DS"]).Tables["Table2"].Select("Id = " + gvProductoInventario.SelectedDataKey["Id"].ToString()))
                                {
                                    //Añadiendo Cantidad Deseada en caso de Existir
                                    cantidad_permitida = cantidad_permitida + Convert.ToDecimal(dr["CantidadDeseada"]);
                                    
                                    //Si ya existe una cantidad Deseada
                                    if (Convert.ToDecimal(dr["CantidadDeseada"]) == 0)
                                    {
                                        //Validando que la Cantidad Deseada no exceda la Cantidad permitida
                                        if (Convert.ToDecimal(dr["Cantidad"]) <= cantidad_permitida)

                                            //Asignando Cantidad
                                            txt.Text = Convert.ToDecimal(dr["Cantidad"]).ToString();
                                        else
                                            //Asignando Cantidad
                                            txt.Text = cantidad_permitida.ToString();
                                    }
                                    else
                                        //Asignando Cantidad
                                        txt.Text = Convert.ToDecimal(dr["CantidadDeseada"]).ToString();
                                }

                                //Configurando Controles
                                txt.Enabled = true;
                                lkb.CommandName = lkb.Text = "Guardar";
                                break;
                            }
                        case "Guardar":
                            {
                                //Declarando Variable Auxiliar
                                decimal cantidad_x = 0.00M;

                                //Recorriendo Registros
                                foreach (DataRow dr in ((DataSet)Session["DS"]).Tables["Table2"].Select("Id = " + gvProductoInventario.SelectedDataKey["Id"].ToString()))
                                {
                                    //Añadiendo Cantidad Deseada en caso de Existir
                                    cantidad_permitida = cantidad_permitida + Convert.ToDecimal(dr["CantidadDeseada"]);
                                    
                                    //Validando que la Cantidad del Inventario sea menor a la Cantidad Permitida
                                    if (Convert.ToDecimal(dr["Cantidad"]) < cantidad_permitida)
                                    
                                        //Asignando Valor Permitido
                                        cantidad_x = Convert.ToDecimal(dr["Cantidad"]);

                                    //Si la Cantidad del Inventario es mayor o igual a la Cantidad Permitida
                                    else if (Convert.ToDecimal(dr["Cantidad"]) >= cantidad_permitida)
                                        
                                        //Asignando Valor Permitido
                                        cantidad_x = cantidad_permitida;

                                    //Validando que la Cantidad Ingresada no exceda la Cantidad Permitida
                                    if (Convert.ToDecimal(txt.Text == "" ? "0" : txt.Text) <= cantidad_x)
                                    
                                        //Asignando Cantidad Ingresada
                                        dr["CantidadDeseada"] = string.Format("{0:0.00}", txt.Text);
                                    else
                                    {
                                        //Asignando Cantidad Permitida
                                        txt.Text = cantidad_x.ToString();
                                        dr["CantidadDeseada"] = string.Format("{0:0.00}", cantidad_x);

                                        //Mostrando Mensaje
                                        ScriptServer.MuestraNotificacion(this, "La Cantidad excede el Monto permitido", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                                    }

                                }

                                //Aceptando Cambios
                                ((DataSet)Session["DS"]).Tables["Table2"].AcceptChanges();

                                //Inicializando Indices
                                Controles.InicializaIndices(gvProductoInventario);

                                //Cargando Grid
                                Controles.CargaGridView(gvProductoInventario, ((DataSet)Session["DS"]).Tables["Table2"], "Id-Cantidad-CantidadDeseada", lblOrdenado.Text, true, 1);
                                
                                //Configurando Controles
                                txt.Enabled = false;
                                lkb.CommandName = lkb.Text = "Cambiar";
                                break;
                            }
                    }
                }

                //Validando que existan Datos
                if (Validacion.ValidaOrigenDatos(((DataSet)Session["DS"]).Tables["Table2"]))
                {
                    //Asignando Valores
                    lblCantidadI.Text = cantidad_inicial.ToString();
                    lblCantidadS.Text = Convert.ToDecimal((((DataSet)Session["DS"]).Tables["Table2"].Compute("SUM(CantidadDeseada)", ""))).ToString();
                    lblCantidadR.Text = (cantidad_inicial - Convert.ToDecimal((((DataSet)Session["DS"]).Tables["Table2"].Compute("SUM(CantidadDeseada)", "")))).ToString();
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvProductoInventario_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Validando que existen Registros
            if (gvProductoInventario.DataKeys.Count > 0)
            {
                //Recorriendo Filas
                foreach (DataRow dr in OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2").Rows)

                    //Asignando Cantidad en '0.00'
                    dr["CantidadDeseada"] = "0.00";

                //Aceptando Cambios
                OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2").AcceptChanges();

                //Cambiando Indice de Página
                Controles.CambiaIndicePaginaGridView(gvProductoInventario, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), e.NewPageIndex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvProductoInventario_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Validando que existen Registros
            if (gvProductoInventario.DataKeys.Count > 0)
            {
                //Recorriendo Filas
                foreach (DataRow dr in OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2").Rows)

                    //Asignando Cantidad en '0.00'
                    dr["CantidadDeseada"] = "0.00";

                //Aceptando Cambios
                OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2").AcceptChanges();

                //Cambiando Expresión del Ordenamiento
                lblOrdenadoProdInv.Text = Controles.CambiaSortExpressionGridView(gvProductoInventario, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), e.SortExpression);
            }
        }

        #endregion

        #endregion

        #region Métodos

        /// <summary>
        /// Método encargado de Inicializar la Página
        /// </summary>
        private void inicializaPagina()
        {
            //Cargando Catalogos
            cargaCatalogos();

            //Inicializando Valores
            inicializaValores();
        }
        /// <summary>
        /// Método encargado de Cargar los Catalogos de la Página
        /// </summary>
        private void cargaCatalogos()
        {
            //Cargando Tamaño
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamano, "", 26);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoProdDes, "", 26);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoProdInv, "", 26);

            //Cargando Estatus
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlEstatus, "", 2126);

            //Cargando Tipo
            Controles.InicializaDropDownList(ddlTipo, "-- Seleccione un Tipo");
        }
        /// <summary>
        /// Método encargado de Inicializar los Valores
        /// </summary>
        private void inicializaValores()
        {
            //Validando que existe Sessión
            if(Convert.ToInt32(Session["id_registro"]) > 0)
            {
                //Instanciando Requisición
                using(SAT_CL.Almacen.Requisicion req = new SAT_CL.Almacen.Requisicion(Convert.ToInt32(Session["id_registro"])))
                {
                    //Validando que existe la Requisición
                    if (req.habilitar)
                    {
                        //Asignando Valores
                        lblNoRequisicion.Text = req.no_requisicion.ToString();
                        ddlEstatus.SelectedValue = req.id_estatus.ToString();
                        txtFechaEntrega.Text = req.fecha_entrega == DateTime.MinValue ? "" : req.fecha_entrega.ToString("dd/MM/yyyy HH:mm");
                        txtFechaEntReq.Text = req.fecha_entrega_requerida == DateTime.MinValue ? "" : req.fecha_entrega_requerida.ToString("dd/MM/yyyy HH:mm");
                        txtFechaSolicitud.Text = req.fecha_solitud == DateTime.MinValue ? "" : req.fecha_solitud.ToString("dd/MM/yyyy HH:mm");

                        //Instanciando Compania
                        using(SAT_CL.Global.CompaniaEmisorReceptor cer = new SAT_CL.Global.CompaniaEmisorReceptor(req.id_compania_emisora))
                        {
                            //Validando que exista la Compania
                            if (cer.habilitar)

                                //Asignando Compania
                                txtCompania.Text = cer.nombre + " ID:" + cer.id_compania_emisor_receptor.ToString();
                            else
                                //Limpiando Control
                                txtCompania.Text = "";
                        }

                        //Instanciando Tipo
                        using (SAT_CL.Global.TipoVencimiento tv = new SAT_CL.Global.TipoVencimiento(req.id_tipo))
                        {
                            //Validando que exista el Tipo
                            if (tv.habilitar)
                            {
                                //Cargando Tipo
                                SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipo, 48, "", tv.id_tipo_aplicacion, "", 0, "");

                                //Asignando Tipo
                                ddlTipo.SelectedValue = tv.id_tipo_vencimiento.ToString();
                            }
                        }

                        //Instanciando Almacen
                        using (SAT_CL.Almacen.Almacen al = new SAT_CL.Almacen.Almacen(req.id_almacen))
                        {
                            //Validando que exista la Compania
                            if (al.habilitar)

                                //Asignando Almacen
                                txtAlmacen.Text = al.descripcion + " ID:" + al.id_almacen.ToString();
                            else
                                //Limpiando Control
                                txtAlmacen.Text = "";
                        }

                        //Instanciando Solicitante
                        using (SAT_CL.Seguridad.Usuario user = new SAT_CL.Seguridad.Usuario(req.id_usuario_solicitante))
                        {
                            //Validando que exista el Solicitante
                            if (user.habilitar)

                                //Asignando Solicitante
                                txtSolicitante.Text = user.nombre + " ID:" + user.id_usuario.ToString();
                            else
                                //Limpiando Control
                                txtSolicitante.Text = "";
                        }

                        //Cargando Productos Requeridos
                        cargaProductosRequeridos();
                    }
                }
            }
            else
            {
                //Asignando Valores
                lblNoRequisicion.Text = "Por Asignar";
                txtFechaEntrega.Text = 
                txtFechaEntReq.Text = 
                txtFechaSolicitud.Text = "";

                //Inicializando Productos Requeridos
                Controles.InicializaGridview(gvProductosRequeridos);
            }
        }
        /// <summary>
        /// Método encargado de Cargar los Productos Requeridos
        /// </summary>
        private void cargaProductosRequeridos()
        {
            //Obteniendo Productos Requeridos
            using (DataTable dtProductosReq = SAT_CL.Almacen.RequisicionDetalle.ObtieneDetallesRequisicionSurtido(Convert.ToInt32(Session["id_registro"])))
            {
                //Validando que existan Registros
                if (Validacion.ValidaOrigenDatos(dtProductosReq))
                {
                    //Cargando GridView
                    gvProductosRequeridos.PageSize = Convert.ToInt32(ddlTamano.SelectedValue);
                    Controles.CargaGridView(gvProductosRequeridos, dtProductosReq, "Id-IdProducto-Cantidad-Disponibles", lblOrdenado.Text, true, 1);

                    //Añadiendo Tabla a Sesión
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtProductosReq, "Table");
                }
                else
                {
                    //Inicializando GridView
                    Controles.InicializaGridview(gvProductosRequeridos);

                    //Añadiendo Tabla a Sesión
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                }
            }
        }

        /// <summary>
        /// Método encargado de Cargar los Productos Empaquetados
        /// </summary>
        /// <param name="id_producto"></param>
        private void cargaProductosEmpaquetados(int id_producto)
        {
            //Obteniendo Productos Requeridos
            using (DataTable dtProductosEmp = SAT_CL.Almacen.Producto.ObtieneProductoContenidoDesempaquetar(id_producto))
            {
                //Validando que existan Registros
                if (Validacion.ValidaOrigenDatos(dtProductosEmp))
                {
                    //Cargando GridView
                    Controles.CargaGridView(gvDesempaquetarProducto, dtProductosEmp, "Id-CantidadDeseada", lblOrdenado.Text, true, 1);

                    //Añadiendo Tabla a Sesión
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtProductosEmp, "Table1");
                }
                else
                {
                    //Inicializando GridView
                    Controles.InicializaGridview(gvDesempaquetarProducto);

                    //Añadiendo Tabla a Sesión
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
                }
            }
        }
        /// <summary>
        /// Método encargado de Obtener los Productos del Inventario
        /// </summary>
        /// <param name="id_producto">Producto Deseado</param>
        private DataTable cargaProductosManual(int id_producto)
        {
            //Instanciando Productos
            return SAT_CL.Almacen.Inventario.ObtieneProductoInvetario(id_producto);
        }
        /// <summary>
        /// Método encargado de Obtener 
        /// </summary>
        /// <param name="dtProductosInventario"></param>
        /// <param name="cantidad_disponible"></param>
        private void cargaProductosInventario(DataTable dtProductosInventario, decimal cantidad_disponible)
        {
            //Validando que existan Productos
            if (Validacion.ValidaOrigenDatos(dtProductosInventario))
            {
                /*/Recorriendo Ciclo
                foreach (DataRow dr in dtProductosInventario.Rows)
                {
                    //Validando si la Cantidad es mayor a "0"
                    if (cantidad_disponible > 0)
                    {
                        //Validando que la Cantidad sea Mayor o Igual que la Cantidad Disponible
                        if (cantidad_disponible >= Convert.ToDecimal(dr["Cantidad"]))
                        {
                            //Asignando Cantidad Completa
                            dr["CantidadDeseada"] = dr["Cantidad"].ToString();

                            //Disminuyendo Cantidad
                            cantidad_disponible = cantidad_disponible - Convert.ToDecimal(dr["Cantidad"]);
                        }
                        else
                        {
                            //Asignando Cantidad Disponible
                            dr["CantidadDeseada"] = cantidad_disponible.ToString();

                            //Vaciando Cantidad
                            cantidad_disponible = 0.00M;
                        }
                    }
                    else
                        //Asignando Cantidad Completa
                        dr["CantidadDeseada"] = "0.00";
                }//*/

                //Cargando GridView
                Controles.CargaGridView(gvProductoInventario, dtProductosInventario, "Id-Cantidad-CantidadDeseada", lblOrdenado.Text, true, 1);

                //Añadiendo Resultado a Sesión
                Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtProductosInventario, "Table2");
            }
            else
            {
                //Inicializando GridView
                Controles.InicializaGridview(gvProductoInventario);

                //Eliminando Resultado de Sesión
                Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table2");
            }
        }
        /// <summary>
        /// Método Privado encargado de Gestionar las Ventanas Modales
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="comando"></param>
        private void gestionaVentanaModal(Control sender, string comando)
        {
            //Validando Comando
            switch(comando)
            {
                case "ProductoDesempaquetado":
                    {
                        //Mostrando Ventana Modal
                        ScriptServer.AlternarVentana(sender, "ProductoDesempaquetado", "contenedorVentanaProductoDesempaquetado", "ventanaProductoDesempaquetado");
                        break;
                    }
                case "SurtirProductoManual":
                    {
                        //Mostrando Ventana Modal
                        ScriptServer.AlternarVentana(sender, "SurtirProductoManual", "contenedorVentanaSurtirProductoManual", "ventanaSurtirProductoManual");
                        break;
                    }
                case "CantidadProducto":
                    {
                        //Mostrando Ventana Modal
                        ScriptServer.AlternarVentana(sender, "CantidadProducto", "contenedorVentanaCantidadProducto", "ventanaCantidadProducto");
                        break;
                    }
            }
        }

        #endregion
    }
}