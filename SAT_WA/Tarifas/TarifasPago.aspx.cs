using SAT_CL.Global;
using System;
using System.Collections.Generic;
using System.Data;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;
using System.Linq;

namespace SAT.Tarifas
{
    public partial class TarifasPago : System.Web.UI.Page
    {
        #region Eventos

        /// <summary>
        /// Evento Producido al efectuarse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {   
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
        {   
            //Validando estatus de Página
            switch (((LinkButton)sender).CommandName)
            {
                case "Nuevo":
                    {   
                        //Asignando estatus nuevo
                        Session["estatus"] = Pagina.Estatus.Nuevo;
                        //Limpiando Id de sesión
                        Session["id_registro"] = 0;
                        //Eliminando DataSet de Session
                        Session["DS"] = null;
                        //Limpiando Mensaje de Error
                        lblError.Text = "";
                        //Limpiando contenido de forma
                        inicializaForma();
                        break;
                    }
                case "Abrir":
                    {   
                        //Inicializando Apertura de Registros
                        inicializaAperturaRegistro(108, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);
                        break;
                    }
                case "Guardar":
                    {   
                        //Invocando Método de Guardado
                        guardaTarifa();
                        break;
                    }
                case "Editar":
                    {   
                        //Asignando estatus nuevo
                        Session["estatus"] = Pagina.Estatus.Edicion;
                        //Limpiando contenido de forma
                        inicializaForma();
                        break;
                    }
                case "Eliminar":
                    {   
                        //Instanciando Producto
                        using (SAT_CL.TarifasPago.Tarifa tar = new SAT_CL.TarifasPago.Tarifa(Convert.ToInt32(Session["id_registro"])))
                        {   
                            //Validando que exista un Producto
                            if (tar.id_tarifa != 0)
                            {   
                                //Declarando Objeto de Retorno
                                RetornoOperacion result = new RetornoOperacion();
                                //Declarando Objeto de Retorno
                                RetornoOperacion resultDetalle = new RetornoOperacion();
                                //Declarando Objeto de Retorno
                                RetornoOperacion resultPerfil = new RetornoOperacion();
                                //Inicializando Transacción
                                using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                                {   
                                    //Declarando Objetos de Ciclo
                                    bool resCargoCiclo, resDetalleCiclo;
                                    resCargoCiclo = resDetalleCiclo = true;
                                    //Declarando las Filas
                                    DataRow drDetalle;
                                    //Obteniendo Detalles
                                    using (DataTable dtDetalles = SAT_CL.TarifasPago.TarifaMatriz.ObtieneMatrizTarifa(Convert.ToInt32(Session["id_registro"])))
                                    {   
                                        //Validando que Existan Registros
                                        if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtDetalles))
                                        {
                                            //Inicializando Contador
                                            int contadorDetalle = 0;
                                            //Inicializando Ciclo
                                            while (resDetalleCiclo)
                                            {
                                                //Obteniendo Fila
                                                drDetalle = dtDetalles.Rows[contadorDetalle];
                                                //Instanciando Cargo Recurrente
                                                using (SAT_CL.TarifasPago.TarifaMatriz tm = new SAT_CL.TarifasPago.TarifaMatriz(Convert.ToInt32(drDetalle["Id"])))
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
                                                    else//Terminando Ciclo
                                                        resDetalleCiclo = false;
                                                }
                                            }
                                        }
                                        else
                                            //Instanciando Excepción
                                            resultDetalle = new RetornoOperacion(0, "No existen Detalles", true);
                                    }
                                   
                                    //Obteniendo Perfiles de la Tarifa
                                    using(DataTable dtPerfiles = SAT_CL.TarifasPago.TarifaDetallePerfilPago.ObtienePerfilesTarifa(Convert.ToInt32(Session["id_registro"])))
                                    {
                                        //Validando que Existen Perfiles
                                        if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtPerfiles))
                                        {
                                            //Recorriendo Registros
                                            foreach(DataRow dr in dtPerfiles.Rows)
                                            {
                                                //Instanciando Perfil
                                                using(SAT_CL.TarifasPago.TarifaDetallePerfilPago tdp = new SAT_CL.TarifasPago.TarifaDetallePerfilPago(Convert.ToInt32(dr["Id"])))
                                                {
                                                    //Validando que Exista el Perfil
                                                    if(tdp.id_tarifa_detalle_perfil_pago > 0)
                                                    {
                                                        //Deshabilitando Perfil
                                                        resultPerfil = tdp.DeshabilitaTarifaDetallePerfilPago(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                                        //Validando que la Operación fuese Incorrecta
                                                        if (!resultPerfil.OperacionExitosa)

                                                            //Terminando Ciclo
                                                            break;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                            //Instanciando Positivo el Retorno
                                            resultPerfil = new RetornoOperacion(1);
                                    }
                                    
                                    //Validando que ambas Operaciones hayan sido Exitosas
                                    if (resultDetalle.OperacionExitosa && resultPerfil.OperacionExitosa)
                                    {   
                                        //Deshabilitando Tarifa
                                        result = tar.DeshabilitaTarifa(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                        
                                        //Validando que la Deshabilitación haya sido exitosa
                                        if (result.OperacionExitosa)
                                            
                                            //Completando Transaccion
                                            trans.Complete();
                                    }
                                }
                                
                                //Validando que la Operación sea exitosa
                                if (result.OperacionExitosa)
                                {   
                                    //Limpiando registro de Session
                                    Session["id_registro"] = 0;
                                    //Cambiando a Estatus "Nuevo"
                                    Session["estatus"] = Pagina.Estatus.Nuevo;
                                    //Inicializando Forma
                                    inicializaForma();
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
                    {   
                        //Inicializando Bitacora
                        inicializaBitacora(Session["id_registro"].ToString(), "108", "Tarifas de Pago");
                        break;
                    }
                case "Referencias":
                    {   
                        //Invocando Método de Inicialización de Referencias
                        inicializaReferenciaRegistro(Session["id_registro"].ToString(), "108", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString());
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
        /// <summary>
        /// Evento que evalúa el nivel de aplicación de una tarifa de pago
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlNivelAplicacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Valida si la opcion del control ddlNivelAplicacion es un movimiento en vacío.
            if (Convert.ToInt32(ddlNivelAplicacion.SelectedValue) == 3)
            {
                //Asigna el texto TODOS ID=0 al control txtCliente.
                txtCliente.Text = "TODOS ID:0";
                //Deja en modo lectura al control txtCliente.
                txtCliente.Enabled = false;
            }
            //Si el nivel de aplicación de una tarifa de pago no es un movimiento en vacío.
            else
            {
                //Limpia el control txt.Cliente.
                txtCliente.Text = "";
                //Deja en modo edición al control txtCliente.
                txtCliente.Enabled = true;
            }
        }

        #region Eventos Clasificación

        /// <summary>
        /// Evento Producido al Presionar el Boton "Guardar" de la Clasificación
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucClasificacion_ClickGuardar(object sender, EventArgs e)
        {   
            //Guardando Cambios
            ucClasificacion.GuardaCambiosClasificacion();
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Cancelar" de la Clasificación
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucClasificacion_ClickCancelar(object sender, EventArgs e)
        {   
            //Cancelando Cambios
            ucClasificacion.CancelaCambiosClasificacion();
        }

        #endregion

       

        #region Eventos Perfil Tarifa

        /// <summary>
        /// Evento Producido al Cambiar el Indice del Control "Tipo Columna Filtro"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTipoColumnaFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            //
            cargaValoresPerfil();
        }
        /// <summary>
        /// Método encargado de Agregar el Perfil de Pago de la Tarifa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkAgregaPerfilPago_Click(object sender, EventArgs e)
        {
            //Validando el Tipo de Columna Filtro
            switch (ddlPerfilPago.SelectedValue)
            {
                //Unidad
                case "1":
                    {
                        //Configurando Unidad
                        SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipoColumnaFiltro, 54, "", 19, "", 0, "");
                        cargaValoresPerfil();
                        break;
                    }
                //Operador
                case "2":
                    {
                        //Configurando Operador
                        SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipoColumnaFiltro, 54, "", 76, "", 0, "");
                        cargaValoresPerfil();
                        break;
                    }
                //Transportista
                case "3":
                    {
                        //Configurando Transportista
                        SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipoColumnaFiltro, 54, "", 25, "", 0, "");
                        cargaValoresPerfil();
                        break;
                    }
            }

            //Evento que Carga los valores de los Perfiles
            cargaPerfiles();

            //Limpiando Control
            lblErrorperfil.Text = "";

            //Alternando Ventana
            TSDK.ASP.ScriptServer.AlternarVentana(uplnkAgregaPerfilPago, uplnkAgregaPerfilPago.GetType(), "PerfilesTarifa", "contenedorVentanaPerfilPago", "ventanaPerfilPago");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportarPerfil_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoPerfil_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño de Página
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvPerfilTarifa, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3"), Convert.ToInt32(ddlTamanoPerfil.SelectedValue), true, 0);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvPerfilTarifa_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión de Ordenamiento
            lblOrdenadoPerfil.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvPerfilTarifa, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3"), e.SortExpression);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvPerfilTarifa_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvPerfilTarifa, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3"), e.NewPageIndex);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardarPerfil_Click(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Validando Estatus
            switch((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {
                        //Validando que Existen Registros
                        if(TSDK.Datos.Validacion.ValidaOrigenDatos((DataSet)Session["DS"], "Table3"))
                        {
                            //Obteniendo Perfil Coincidente
                            string[] perfiles = (from perfilTarifa in ((DataSet)Session["DS"]).Tables["Table3"].AsEnumerable()
                                                 where perfilTarifa.Field<string>("IdTipo") == ddlTipoColumnaFiltro.SelectedValue
                                                 select perfilTarifa.Field<string>("IdTipo").First().ToString()).ToArray();
                            
                            //Validando que Existen Registros
                            if (perfiles.Length <= 0)
                            {
                                //Añadiendo Perfil
                                ((DataSet)Session["DS"]).Tables["Table3"].Rows.Add(((DataSet)Session["DS"]).Tables["Table3"].Rows.Count + 1, 
                                                    ddlTipoColumnaFiltro.SelectedValue, ddlTipoColumnaFiltro.SelectedItem.Text,
                                                    ddlValor.Visible ? ddlValor.SelectedValue : "0", ddlValor.Visible ? ddlValor.SelectedItem.Text : txtValor.Text);

                                //Aceptando Cambios
                                ((DataSet)Session["DS"]).Tables["Table3"].AcceptChanges();

                                //Asignando registro Positivo
                                result = new RetornoOperacion(1);
                            }
                            else
                                //Instanciando Excepción
                                result = new RetornoOperacion("No puede agregar un Perfil con este Tipo, ya existe uno previo");
                        }
                        else
                        {
                            //Declarando Tabla temporal
                            DataTable dtPerfiles = new DataTable();

                            //Añadiendo Columnas
                            dtPerfiles.Columns.Add("Id", typeof(string));
                            dtPerfiles.Columns.Add("IdTipo", typeof(string));
                            dtPerfiles.Columns.Add("Tipo", typeof(string));
                            dtPerfiles.Columns.Add("IdValor", typeof(string));
                            dtPerfiles.Columns.Add("Valor", typeof(string));

                            //Añadiendo Perfil
                            dtPerfiles.Rows.Add(1, ddlTipoColumnaFiltro.SelectedValue, ddlTipoColumnaFiltro.SelectedItem.Text, 
                                                    ddlValor.Visible ? ddlValor.SelectedValue : "0",  ddlValor.Visible ? ddlValor.SelectedItem.Text : txtValor.Text);

                            //Añadiendo Tabla a DataSet
                            Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtPerfiles, "Table3");

                            //Asignando registro Positivo
                            result = new RetornoOperacion(1);
                        }


                        break;
                    }
                case Pagina.Estatus.Edicion:
                    {
                        //Insertando Perfil de Tarifa
                        result = SAT_CL.TarifasPago.TarifaDetallePerfilPago.InsertaTarifaDetallePerfilPago(Convert.ToInt32(Session["id_registro"]),
                                    Convert.ToInt32(ddlTipoColumnaFiltro.SelectedValue), ddlValor.Visible ? ddlValor.SelectedValue : txtValor.Text, 
                                    ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                        break;
                    }
            }

            //Validando que la Operación fuese Exitosa
            if(result.OperacionExitosa)
                
                //Cargando Perfiles en BD
                cargaPerfiles();

            //Mostrando Mensaje de la Operación
            lblErrorperfil.Text = result.Mensaje;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarPerfil_Click(object sender, EventArgs e)
        {
            //Cargando Catalogo
            cargaValoresPerfil();

            //Carga los Perfiles
            cargaPerfiles();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkCerrarPerfil_Click(object sender, EventArgs e)
        {
            //Alternando Ventana
            TSDK.ASP.ScriptServer.AlternarVentana(uplnkCerrarPerfil, uplnkCerrarPerfil.GetType(), "PerfilesTarifa", "contenedorVentanaPerfilPago", "ventanaPerfilPago");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEliminarPerfil_Click(object sender, EventArgs e)
        {
            //Validando que Existan Registros
            if (gvPerfilTarifa.DataKeys.Count > 0)
            {
                //Declarando Objeto de Retorno
                RetornoOperacion result = new RetornoOperacion();

                //Seleccionando Fila
                TSDK.ASP.Controles.SeleccionaFila(gvPerfilTarifa, sender, "lnk", false);
                
                //Validando Estatus
                switch ((Pagina.Estatus)Session["estatus"])
                {
                    case Pagina.Estatus.Nuevo:
                        {
                            //Recorriendo Filas
                            foreach(DataRow dr in TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3").Rows)
                            {
                                //Validando 
                                if (Convert.ToInt32(dr["Id"]) == Convert.ToInt32(gvPerfilTarifa.SelectedDataKey["Id"]))
                                {
                                    //Removiendo Filas
                                    ((DataSet)Session["DS"]).Tables["Table3"].Rows.Remove(dr);

                                    //Terminando Ciclo
                                    break;
                                }
                            }

                            //Aceptando Cambios
                            ((DataSet)Session["DS"]).Tables["Table3"].AcceptChanges();

                            //Instanciando Retorno
                            result = new RetornoOperacion(1);

                            break;
                        }
                    case Pagina.Estatus.Edicion:
                        {
                            //Obteniendo Perfil
                            using (SAT_CL.TarifasPago.TarifaDetallePerfilPago tdp = new SAT_CL.TarifasPago.TarifaDetallePerfilPago(Convert.ToInt32(gvPerfilTarifa.SelectedDataKey["Id"])))
                            {
                                //Validando que Exista el Perfil
                                if(tdp.id_tarifa_detalle_perfil_pago > 0)
                                
                                    //Deshabilitando Perfil
                                    result = tdp.DeshabilitaTarifaDetallePerfilPago(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                            }

                            break;
                        }
                }

                //Validando que la Operación fuese Exitosa
                if(result.OperacionExitosa)
                
                    //Cargando Perfiles
                    cargaPerfiles();

                //Mostrando Mensaje
                lblErrorperfil.Text = result.Mensaje;
            }
        }

        #endregion

        /// <summary>
        /// Evento Producido al Presionar el Boton "Guardar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {   
            //Invocando Método de Guardado
            guardaTarifa();
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Cancelar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {   
            //Validando el Estatus de la Página
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
        {
            //Invocando Método de Configuración
            configuraControlesMatriz();
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice del Control "Filas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlFilas_SelectedIndexChanged(object sender, EventArgs e)
        {   
            //Invocando Método de Configuración
            configuraControlesMatriz();
        }

        #region Eventos Tarifa Matriz (Detalles)

        #region Eventos GridView "Tarifa Matriz (Detalles)"

        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoReqDisp_SelectedIndexChanged(object sender, EventArgs e)
        {   
            //Cambiando Tamaño de Página
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvTarifamatriz, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamanoReqDisp.SelectedValue), true, 0);
        }
        /// <summary>
        /// Evento Producido al Exportar el Contenido del GridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportar_Click(object sender, EventArgs e)
        {   
            //Exportando Contenido
            TSDK.ASP.Controles.ExportaContenidoGridView(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"));
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Paginación
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvTarifamatriz_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {   
            //Cambiando Indice de Página
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvTarifamatriz, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex);
        }
        /// <summary>
        /// Evento Producido al Cambiar el Ordenamiento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvTarifamatriz_Sorting(object sender, GridViewSortEventArgs e)
        {   
            //Cambiando Expresión de Ordenamiento
            lblOrdenar.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvTarifamatriz, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression);
        }
        /// <summary>
        /// Evento Producido al Dar Click al LinkButton "Editar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEditarMatriz_Click(object sender, EventArgs e)
        {   
            //Validando que existan Registros
            if (gvTarifamatriz.DataKeys.Count > 0)
            {   
                //Seleccionando Fila
                TSDK.ASP.Controles.SeleccionaFila(gvTarifamatriz, sender, "lnk", false);
                
                //Instanciando Detalle de Tarifa
                using (SAT_CL.TarifasPago.TarifaMatriz tm = new SAT_CL.TarifasPago.TarifaMatriz(Convert.ToInt32(gvTarifamatriz.SelectedDataKey["Id"])))
                {   
                    //Validando que exista el Registro
                    if (tm.id_tarifa_matriz != 0)
                    {   
                        //Validando Columna Filtro
                        switch ((SAT_CL.TarifasPago.Tarifa.CriterioMatrizTarifa)Convert.ToInt32(ddlColumnas.SelectedValue))
                        {   
                            //Opciones de Catalogo
                            case SAT_CL.TarifasPago.Tarifa.CriterioMatrizTarifa.Producto:
                            case SAT_CL.TarifasPago.Tarifa.CriterioMatrizTarifa.Ciudad:
                            case SAT_CL.TarifasPago.Tarifa.CriterioMatrizTarifa.Ubicacion:
                            case SAT_CL.TarifasPago.Tarifa.CriterioMatrizTarifa.NoAplica:
                                {   
                                    //Mostrando Control de Catalogo
                                    txtCatCol.Text = tm.valor_desc_col + " ID:" + tm.valor_filtro_col;
                                    txtCatCol.Visible = true;
                                    txtCatCol.Enabled =
                                    txtRotCol.Visible = false;
                                    break;
                                }
                            
                            //Opciones de Captura
                            case SAT_CL.TarifasPago.Tarifa.CriterioMatrizTarifa.Paradas:
                            case SAT_CL.TarifasPago.Tarifa.CriterioMatrizTarifa.Distancia:
                            case SAT_CL.TarifasPago.Tarifa.CriterioMatrizTarifa.Peso:
                            case SAT_CL.TarifasPago.Tarifa.CriterioMatrizTarifa.Volumen:
                            case SAT_CL.TarifasPago.Tarifa.CriterioMatrizTarifa.Cantidad:
                                {   
                                    //Mostrando Control de Catalogo
                                    txtRotCol.Text = tm.valor_desc_col;
                                    txtRotCol.Enabled =
                                    txtCatCol.Visible =
                                    txtRotCol.Visible = true;
                                    break;
                                }
                        }
                        
                        //Validando Columna Filtro
                        switch ((SAT_CL.TarifasPago.Tarifa.CriterioMatrizTarifa)Convert.ToInt32(ddlFilas.SelectedValue))
                        {   
                            //Opciones de Catalogo
                            case SAT_CL.TarifasPago.Tarifa.CriterioMatrizTarifa.Producto:
                            case SAT_CL.TarifasPago.Tarifa.CriterioMatrizTarifa.Ciudad:
                            case SAT_CL.TarifasPago.Tarifa.CriterioMatrizTarifa.Ubicacion:
                            case SAT_CL.TarifasPago.Tarifa.CriterioMatrizTarifa.NoAplica:
                                {   
                                    //Mostrando Control de Catalogo
                                    txtCatFila.Text = tm.valor_desc_row + " ID:" + tm.valor_filtro_row;
                                    txtCatFila.Visible = true;
                                    txtCatFila.Enabled =
                                    txtRotFila.Visible = false;
                                    break;
                                }
                            //Opciones de Captura
                            case SAT_CL.TarifasPago.Tarifa.CriterioMatrizTarifa.Paradas:
                            case SAT_CL.TarifasPago.Tarifa.CriterioMatrizTarifa.Distancia:
                            case SAT_CL.TarifasPago.Tarifa.CriterioMatrizTarifa.Peso:
                            case SAT_CL.TarifasPago.Tarifa.CriterioMatrizTarifa.Volumen:
                            case SAT_CL.TarifasPago.Tarifa.CriterioMatrizTarifa.Cantidad:
                                {   
                                    //Mostrando Control de Catalogo
                                    txtRotFila.Text = tm.valor_desc_row;
                                    txtRotFila.Enabled =
                                    txtCatFila.Visible = false;
                                    txtRotFila.Visible = true;
                                    break;
                                }
                        }
                        
                        txtTarifaCargado.Text = tm.tarifa_cargado.ToString();
                        txtTarifaVacio.Text = tm.tarifa_vacio.ToString();
                        txtTarifaTronco.Text = tm.tarifa_tronco.ToString();
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
            {   
                //Declarando Variables Contenedoras
                string val_desc_col, val_desc_row, val_id_col, val_id_row = "";
                //Invocando Método de Obtencion
                obtieneValoresColumnaFila(out val_desc_col, out val_desc_row, out val_id_col, out val_id_row);
                //Declarando Objeto de Retorno
                RetornoOperacion result = new RetornoOperacion();
                //Validando que exista una Fila Seleccionada
                if (gvTarifamatriz.SelectedIndex != -1)
                {   
                    //Instanciando Detalle de Tarifa
                    using (SAT_CL.TarifasPago.TarifaMatriz tm = new SAT_CL.TarifasPago.TarifaMatriz(Convert.ToInt32(gvTarifamatriz.SelectedDataKey["Id"])))
                    {   
                        //Validando que exista el Registro
                        if (tm.id_tarifa_matriz != 0)
                        {   
                            //Editando Matriz
                            result = tm.EditaTarifaMatriz(Convert.ToInt32(Session["id_registro"]), tm.valor_filtro_col, tm.valor_filtro_row, tm.valor_desc_col,
                                                          tm.valor_desc_row, 0, 0, Convert.ToDecimal(txtTarifaCargado.Text), Convert.ToDecimal(txtTarifaVacio.Text),
                                                          Convert.ToDecimal(txtTarifaTronco.Text), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                        }
                    }
                }
                else
                {   
                    //Insertar Matriz
                    result = SAT_CL.TarifasPago.TarifaMatriz.InsertaTarifaMatriz(Convert.ToInt32(Session["id_registro"]), val_id_col, val_id_row, val_desc_col,
                                                                             val_desc_row, 0, 0, Convert.ToDecimal(txtTarifaCargado.Text), Convert.ToDecimal(txtTarifaVacio.Text),
                                                                             Convert.ToDecimal(txtTarifaTronco.Text), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                }
                
                //Validando que la Operación fuera Exitosa
                if (result.OperacionExitosa)
                {   
                    //Limpiando Controles
                    limpiaControlesTarifaMatriz();
                    //Habilitando Controles
                    habilitarControlesTarifaMatriz(true);
                    //Cargando Detalles
                    cargaDetallesTarifaMatriz();
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
        {   
            //Limpiando Controles
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
                    using (SAT_CL.TarifasPago.TarifaMatriz tm = new SAT_CL.TarifasPago.TarifaMatriz(Convert.ToInt32(gvTarifamatriz.SelectedDataKey["Id"])))
                    {
                        //Inicializando Transacción
                        using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                        {
                            
                            //Validando que exista el Registro
                            if (tm.id_tarifa_matriz != 0)
                            {
                                //Editando Matriz
                                result = tm.DeshabilitaTarifaMatriz(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                //Validando que se hayan Realizado Ambas Deshabilitaciones
                                if (result.OperacionExitosa && resultCargo.OperacionExitosa)
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
        /// Evento encargado de Mostrar 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkVerMatriz_Click(object sender, EventArgs e)
        {
            //Cargando Detalles de la Tarifa
            cargaMatrizDetallesTarifa();

            //Alternando Ventana
            TSDK.ASP.ScriptServer.AlternarVentana(uplnkVerMatriz, uplnkVerMatriz.GetType(), "VentanaMatriz", "contenedorVentanaMatriz", "ventanaMatriz");
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
            switch (((Button)sender).CommandName)
            {
                case "Fila":
                    {
                        //Obteniendo Detalles Ligados a una Tarifa y el Valor de la Columna
                        using (DataTable dt = SAT_CL.TarifasPago.TarifaMatriz.ObtieneDetallesTarifaValorColumna(Convert.ToInt32(Session["id_registro"]), "", txtRotFila.Visible == true ? txtRotFila.Text : TSDK.Base.Cadena.RegresaCadenaSeparada(txtCatFila.Text, "ID:", 1)))
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
                                        using (SAT_CL.TarifasPago.TarifaMatriz tm = new SAT_CL.TarifasPago.TarifaMatriz(Convert.ToInt32(dr["Id"])))
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
                        using (DataTable dt = SAT_CL.TarifasPago.TarifaMatriz.ObtieneDetallesTarifaValorColumna(Convert.ToInt32(Session["id_registro"]), val_id_col, ""))
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
                                        using (SAT_CL.TarifasPago.TarifaMatriz tm = new SAT_CL.TarifasPago.TarifaMatriz(Convert.ToInt32(dr["Id"])))
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
        {   
            //Cargando Catalogos
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
        {   
            //Cargando Catalogos
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTarifaBase, 15, "");
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlNivelAplicacion, "", 89);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlPerfilPago, "", 90);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlColumnas, 16, "Ninguna");
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlFilas, 16, "Ninguna");
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoReqDisp, "", 18);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoPerfil, "", 18);
        }
        /// <summary>
        /// Método Privado encargado de Habilitar el Menu
        /// </summary>
        private void habilitaMenu()
        {   
            //Validando Estatus de Session
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {   
                        //Archivo
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
                    {   
                        //Archivo
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
                    {   
                        //Archivo
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
        {   
            //Validando Estatus de Session
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {   
                        //Habilitando Controles
                        txtDescripcion.Enabled =
                        ddlTarifaBase.Enabled =
                        ddlNivelAplicacion.Enabled =
                        ddlPerfilPago.Enabled =
                        ddlColumnas.Enabled =
                        ddlFilas.Enabled =
                        txtCliente.Enabled =
                        txtValorUCargado.Enabled =
                        txtValorUVacio.Enabled =
                        txtValorTronco.Enabled =
                        txtFecIni.Enabled =
                        txtFecFin.Enabled =
                        btnGuardar.Enabled =
                        btnCancelar.Enabled = true;
                        //Habilitando Controles
                        habilitarControlesTarifaMatriz(false);
                        ucClasificacion.Enabled =
                       
                        lnkAgregaPerfilPago.Enabled = true;
                        break;
                    }
                case Pagina.Estatus.Edicion:
                    {   
                        //Habilitando Controles
                        txtDescripcion.Enabled =
                        ddlTarifaBase.Enabled =
                        ddlNivelAplicacion.Enabled =
                        txtCliente.Enabled =
                        txtValorUCargado.Enabled =
                        txtValorUVacio.Enabled =
                        txtValorTronco.Enabled =
                        txtFecIni.Enabled =
                        txtFecFin.Enabled =
                        btnGuardar.Enabled =
                        btnCancelar.Enabled = true;
                        //Habilitando Controles
                        habilitarControlesTarifaMatriz(true);
                        ucClasificacion.Enabled = true;
                        
                        //Deshabilitando Columna, Fila y Perfil de Pago
                        ddlPerfilPago.Enabled =
                        ddlColumnas.Enabled =
                        ddlFilas.Enabled = false;
                        lnkAgregaPerfilPago.Enabled = true;
                        break;
                    }
                case Pagina.Estatus.Lectura:
                    {   
                        //Deshabilitando Controles
                        txtDescripcion.Enabled =
                        ddlTarifaBase.Enabled =
                        ddlNivelAplicacion.Enabled =
                        ddlPerfilPago.Enabled =
                        ddlColumnas.Enabled =
                        ddlFilas.Enabled =
                        txtCliente.Enabled =
                        txtValorUCargado.Enabled =
                        txtValorUVacio.Enabled =
                        txtValorTronco.Enabled =
                        txtFecIni.Enabled =
                        txtFecFin.Enabled =
                        btnGuardar.Enabled =
                        btnCancelar.Enabled = false;
                        //Habilitando Controles
                        habilitarControlesTarifaMatriz(false);
                        ucClasificacion.Enabled =
                        
                        lnkAgregaPerfilPago.Enabled = false;
                        break;
                    }
            }
        }
        /// <summary>
        /// Método Privado encargado de Inicializar los Valores
        /// </summary>
        private void inicializaValores()
        {   
            //Validando Estatus de Session
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {   
                        //Asignando Valores
                        lblId.Text = "Por Asignar";
                        txtDescripcion.Text =
                        txtCliente.Text = "";
                        txtValorUCargado.Text = 
                        txtValorUVacio.Text = 
                        txtValorTronco.Text = "0.00";
                        txtFecIni.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy");
                        txtFecFin.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().AddYears(1).ToString("dd/MM/yyyy");
                        //Instanciando Compania
                        using (SAT_CL.Global.CompaniaEmisorReceptor cer = new SAT_CL.Global.CompaniaEmisorReceptor(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                        {   
                            //Validando si existe la Compania
                            if (cer.id_compania_emisor_receptor != 0)
                                
                                //Asignando Descripción de la Compania
                                txtCompania.Text = cer.nombre + " ID:" + cer.id_compania_emisor_receptor.ToString();
                            else
                                //Limpiando Valor
                                txtCompania.Text = "";
                        }

                        //Inicializando Control
                        ucClasificacion.InicializaControl(108, 0, Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:", 1)));

                        //Invocando Método de Limpieza
                        limpiaControlesTarifaMatriz();
                        TSDK.ASP.Controles.InicializaGridview(gvTarifamatriz);
                        configuraControlesMatriz();
                        break;
                    }
                case Pagina.Estatus.Lectura:
                case Pagina.Estatus.Edicion:
                    {   
                        //Instanciando Tarifa
                        using (SAT_CL.TarifasPago.Tarifa tar = new SAT_CL.TarifasPago.Tarifa(Convert.ToInt32(Session["id_registro"])))
                        {   
                            //Validando que exista la Tarifa
                            if (tar.id_tarifa != 0)
                            {   
                                //Asignando Valores
                                lblId.Text = tar.id_tarifa.ToString();
                                txtDescripcion.Text = tar.descripcion;
                                
                                //Instanciando Compania
                                using (SAT_CL.Global.CompaniaEmisorReceptor cer = new SAT_CL.Global.CompaniaEmisorReceptor(tar.id_compania_emisor))
                                {   
                                    //Validando si existe la Compania
                                    if (cer.id_compania_emisor_receptor != 0)
                                        
                                        //Asignando Descripción de la Compania
                                        txtCompania.Text = cer.nombre + " ID:" + cer.id_compania_emisor_receptor.ToString();
                                    else
                                        //Limpiando Valor
                                        txtCompania.Text = "";
                                }
                                //Instanciando Cliente
                                using (SAT_CL.Global.CompaniaEmisorReceptor cli = new SAT_CL.Global.CompaniaEmisorReceptor(tar.id_cliente_receptor))
                                {   
                                    //Validando que exista el Cliente
                                    if (cli.id_compania_emisor_receptor != 0)
                                        
                                        //Asignando Descripción
                                        txtCliente.Text = cli.nombre + " ID:" + cli.id_compania_emisor_receptor.ToString();
                                    else
                                        //Limpiando Valor
                                        txtCliente.Text = "";
                                }

                                //Valores de Texto
                                txtValorUCargado.Text = tar.valor_unitario.ToString();
                                txtValorUVacio.Text = tar.valor_unitario_tronco.ToString();
                                txtValorTronco.Text = tar.valor_unitario_vacio.ToString();
                                txtFecIni.Text = tar.fecha_inicio.ToString("dd/MM/yyyy");
                                txtFecFin.Text = tar.fecha_fin.ToString("dd/MM/yyyy");
                                
                                //Catalogos
                                ddlTarifaBase.SelectedValue = tar.id_base_tarifa.ToString();
                                ddlNivelAplicacion.SelectedValue = tar.id_nivel_pago.ToString();
                                ddlPerfilPago.SelectedValue = tar.id_perfil_pago.ToString();
                                ddlColumnas.SelectedValue = tar.id_columna_filtro_col.ToString();
                                ddlFilas.SelectedValue = tar.id_columna_filtro_row.ToString();
                                
                                //Inicializando Control
                                ucClasificacion.InicializaControl(108, tar.id_tarifa, Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:", 1)));
                                
                                //Cargando Detalles
                                configuraControlesMatriz();
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
        {   
            //Declarando Objeto de Retorno de Operación
            RetornoOperacion result = new RetornoOperacion();
            //Declarando Variable de Fecha
            DateTime FecIni, FecFin;
            //Obteniendo Fechas
            DateTime.TryParse(txtFecIni.Text + " 00:00", out FecIni);
            DateTime.TryParse(txtFecFin.Text + " 23:59", out FecFin);
            
            //Validando Fechas
            if (FecIni.CompareTo(FecFin) < 0)
            {      
                //Validando Estatus de Session
                switch ((Pagina.Estatus)Session["estatus"])
                {
                    case Pagina.Estatus.Nuevo:
                        {   
                            //Inicializando Transacción
                            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                            {   
                                //Insertando Tarifa
                                result = SAT_CL.TarifasPago.Tarifa.InsertaTarifa(txtDescripcion.Text.ToUpper(), Convert.ToInt32(ddlTarifaBase.SelectedValue), 
                                                (SAT_CL.TarifasPago.Tarifa.NivelPago)Convert.ToInt32(ddlNivelAplicacion.SelectedValue), 
                                                (SAT_CL.TarifasPago.Tarifa.PerfilPago)Convert.ToInt32(ddlPerfilPago.SelectedValue),
                                                Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:", 1)),
                                                Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCliente.Text, "ID:", 1)),
                                                Convert.ToDecimal(txtValorUCargado.Text), Convert.ToDecimal(txtValorUVacio.Text),
                                                Convert.ToDecimal(txtValorTronco.Text), Convert.ToInt32(ddlColumnas.SelectedValue), 
                                                Convert.ToInt32(ddlFilas.SelectedValue), FecIni, FecFin, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                
                                //Validando que se Inserto la tarifa
                                if (result.OperacionExitosa)
                                {   
                                    //Declarando Objeto de Retorno de Clasificacion
                                    RetornoOperacion resultClasificacion = new RetornoOperacion();
                                    
                                    //Insertando Clasificación
                                    resultClasificacion = Clasificacion.InsertaClasificacion(108, result.IdRegistro, 0, 0, 0, 0, 0, 0, 0, 0, 0, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                    
                                    //Validando que se Inserto la Clasificación
                                    if (resultClasificacion.OperacionExitosa)
                                        //Finalizando Transaccion
                                        trans.Complete();
                                }
                            }
                            break;
                        }
                    case Pagina.Estatus.Edicion:
                        {   
                            //Instanciando Registro
                            using (SAT_CL.TarifasPago.Tarifa tar = new SAT_CL.TarifasPago.Tarifa(Convert.ToInt32(Session["id_registro"])))
                            {   
                                //Validando que exista la Tarifa
                                if (tar.id_tarifa != 0)
                                {   
                                    //Editando la Tarifa
                                    result = tar.EditaTarifa(txtDescripcion.Text.ToUpper(), Convert.ToInt32(ddlTarifaBase.SelectedValue),
                                                        (SAT_CL.TarifasPago.Tarifa.NivelPago)Convert.ToInt32(ddlNivelAplicacion.SelectedValue),
                                                        (SAT_CL.TarifasPago.Tarifa.PerfilPago)Convert.ToInt32(ddlPerfilPago.SelectedValue),
                                                        Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:", 1)),
                                                        Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCliente.Text, "ID:", 1)),
                                                        Convert.ToDecimal(txtValorUCargado.Text), Convert.ToDecimal(txtValorUVacio.Text),
                                                        Convert.ToDecimal(txtValorTronco.Text), Convert.ToInt32(ddlColumnas.SelectedValue),
                                                        Convert.ToInt32(ddlFilas.SelectedValue), FecIni, FecFin, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                }
                            }

                            break;
                        }
                }
                
                //Validando que la Operacion haya sido exitosa
                if (result.OperacionExitosa)
                {   
                    //Asignando Valores
                    Session["id_registro"] = result.IdRegistro;
                    
                    //Validando el Estatus de la Página
                    if ((Pagina.Estatus)Session["estatus"] == Pagina.Estatus.Nuevo)
                        
                        //Asignando Estatus de Edición
                        Session["estatus"] = Pagina.Estatus.Edicion;
                    else
                        //Asignando Estatus de Lectura
                        Session["estatus"] = Pagina.Estatus.Lectura;

                    //Guardando Perfiles
                    guardaPerfiles();
                    
                    //Inicializando Forma
                    inicializaForma();
                }
                
                //Mostrando Mensaje
                lblError.Text = result.Mensaje;
            }
            else
                //Mostrando Error
                lblError.Text = "La Fecha de Inicio es posterior a la Fecha de Fin";
        }
        /// <summary>
        /// Método Privado encargado de Inicializar la Apertura de un Registro
        /// </summary>
        /// <param name="idTabla">Id de Tabla</param>
        private void inicializaAperturaRegistro(int idTabla, int idCompania)
        {   
            //Asignando Session
            Session["id_tabla"] = idTabla;
            //Construyendo URL 
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Tarifas/Tarifas.aspx", "~/Accesorios/AbrirRegistro.aspx?P1=" + idCompania.ToString());
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
        {   
            //Construyendo URL 
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Tarifas/TarifasPago.aspx", "~/Accesorios/BitacoraRegistro.aspx?idT=" + idTabla + "&idR=" + idRegistro + "&tB=" + Titulo);
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
        {   
            //Declarando variable para armado de URL
            string urlDestino = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Tarifas/TarifasPago.aspx", "~/Accesorios/ReferenciaRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idC=" + id_compania);
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
        {   
            //Inicializando valores
            val_desc_col = val_desc_row = val_id_col = val_id_row = "";
            
            //Validando el Tipo de Columna
            switch ((SAT_CL.TarifasPago.Tarifa.CriterioMatrizTarifa)Convert.ToInt32(ddlColumnas.SelectedValue))
            {   
                //Opciones de Catalogo
                case SAT_CL.TarifasPago.Tarifa.CriterioMatrizTarifa.Producto:
                case SAT_CL.TarifasPago.Tarifa.CriterioMatrizTarifa.Ciudad:
                case SAT_CL.TarifasPago.Tarifa.CriterioMatrizTarifa.Ubicacion:
                case SAT_CL.TarifasPago.Tarifa.CriterioMatrizTarifa.NoAplica:
                    {   
                        //Asignando Valores
                        val_desc_col = TSDK.Base.Cadena.RegresaCadenaSeparada(txtCatCol.Text, " ID:", 0);
                        val_id_col = TSDK.Base.Cadena.RegresaCadenaSeparada(txtCatCol.Text, " ID:", 1);
                        break;
                    }
                
                //Opciones de Captura
                case SAT_CL.TarifasPago.Tarifa.CriterioMatrizTarifa.Paradas:
                case SAT_CL.TarifasPago.Tarifa.CriterioMatrizTarifa.Distancia:
                case SAT_CL.TarifasPago.Tarifa.CriterioMatrizTarifa.Peso:
                case SAT_CL.TarifasPago.Tarifa.CriterioMatrizTarifa.Volumen:
                case SAT_CL.TarifasPago.Tarifa.CriterioMatrizTarifa.Cantidad:
                    {   
                        //Asignando Valores
                        val_desc_col = txtRotCol.Text;
                        val_id_col = txtRotCol.Text;
                        break;
                    }
            }
            
            //Validando el Tipo de Fila
            switch ((SAT_CL.TarifasPago.Tarifa.CriterioMatrizTarifa)Convert.ToInt32(ddlFilas.SelectedValue))
            {   
                //Opciones de Catalogo
                case SAT_CL.TarifasPago.Tarifa.CriterioMatrizTarifa.Producto:
                case SAT_CL.TarifasPago.Tarifa.CriterioMatrizTarifa.Ciudad:
                case SAT_CL.TarifasPago.Tarifa.CriterioMatrizTarifa.Ubicacion:
                case SAT_CL.TarifasPago.Tarifa.CriterioMatrizTarifa.NoAplica:
                    {   
                        //Asignando Valores
                        val_desc_row = TSDK.Base.Cadena.RegresaCadenaSeparada(txtCatFila.Text, " ID:", 0);
                        val_id_row = TSDK.Base.Cadena.RegresaCadenaSeparada(txtCatFila.Text, " ID:", 1);
                        break;
                    }
                
                //Opciones de Captura
                case SAT_CL.TarifasPago.Tarifa.CriterioMatrizTarifa.Paradas:
                case SAT_CL.TarifasPago.Tarifa.CriterioMatrizTarifa.Distancia:
                case SAT_CL.TarifasPago.Tarifa.CriterioMatrizTarifa.Peso:
                case SAT_CL.TarifasPago.Tarifa.CriterioMatrizTarifa.Volumen:
                case SAT_CL.TarifasPago.Tarifa.CriterioMatrizTarifa.Cantidad:
                    {   
                        //Asignando Valores
                        val_desc_row = txtRotFila.Text;
                        val_id_row = txtRotFila.Text;
                        break;
                    }
            }
        }

        #region Métodos Tarifa Matriz (Detalles)

        /// <summary>
        /// Método encargado de Configurar los Controles de la Matriz
        /// </summary>
        private void configuraControlesMatriz()
        {
            //Validando el Tipo de Columna
            switch ((SAT_CL.TarifasPago.Tarifa.CriterioMatrizTarifa)Convert.ToInt32(ddlColumnas.SelectedValue))
            {
                //Opciones de Catalogo
                case SAT_CL.TarifasPago.Tarifa.CriterioMatrizTarifa.Producto:
                case SAT_CL.TarifasPago.Tarifa.CriterioMatrizTarifa.Ciudad:
                case SAT_CL.TarifasPago.Tarifa.CriterioMatrizTarifa.Ubicacion:
                    {   //Asignando Valores
                        txtCatCol.Visible = true;
                        txtRotCol.Visible = false;
                        break;
                    }

                //Opciones de Captura
                case SAT_CL.TarifasPago.Tarifa.CriterioMatrizTarifa.Paradas:
                case SAT_CL.TarifasPago.Tarifa.CriterioMatrizTarifa.Distancia:
                case SAT_CL.TarifasPago.Tarifa.CriterioMatrizTarifa.Peso:
                case SAT_CL.TarifasPago.Tarifa.CriterioMatrizTarifa.Volumen:
                case SAT_CL.TarifasPago.Tarifa.CriterioMatrizTarifa.Cantidad:
                    {   //Asignando Valores
                        txtCatCol.Visible = false;
                        txtRotCol.Visible = true;
                        break;
                    }
            }

            //Validando el Tipo de Columna
            switch ((SAT_CL.TarifasPago.Tarifa.CriterioMatrizTarifa)Convert.ToInt32(ddlFilas.SelectedValue))
            {
                //Opciones de Catalogo
                case SAT_CL.TarifasPago.Tarifa.CriterioMatrizTarifa.Producto:
                case SAT_CL.TarifasPago.Tarifa.CriterioMatrizTarifa.Ciudad:
                case SAT_CL.TarifasPago.Tarifa.CriterioMatrizTarifa.Ubicacion:
                case SAT_CL.TarifasPago.Tarifa.CriterioMatrizTarifa.NoAplica:
                    {
                        //Asignando Valores
                        txtCatFila.Visible = true;
                        txtRotFila.Visible = false;
                        break;
                    }

                //Opciones de Captura
                case SAT_CL.TarifasPago.Tarifa.CriterioMatrizTarifa.Paradas:
                case SAT_CL.TarifasPago.Tarifa.CriterioMatrizTarifa.Distancia:
                case SAT_CL.TarifasPago.Tarifa.CriterioMatrizTarifa.Peso:
                case SAT_CL.TarifasPago.Tarifa.CriterioMatrizTarifa.Volumen:
                case SAT_CL.TarifasPago.Tarifa.CriterioMatrizTarifa.Cantidad:
                    {
                        //Asignando Valores
                        txtCatFila.Visible = false;
                        txtRotFila.Visible = true;
                        break;
                    }
            }
        }

        /// <summary>
        /// Método Privado encargado de Cargar los Detalles de las Tarifas
        /// </summary>
        private void cargaDetallesTarifaMatriz()
        {   
            //Obteniendo Detalles de las Tarifas
            using (DataTable dtDetallesTarifa = SAT_CL.TarifasPago.TarifaMatriz.ObtieneMatrizTarifa(Convert.ToInt32(Session["id_registro"])))
            {   
                //Validando que existan registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtDetallesTarifa))
                {   
                    //Cargando GridView
                    TSDK.ASP.Controles.CargaGridView(gvTarifamatriz, dtDetallesTarifa, "Id", "");
                    //Añadiendo Tabla a DataSet de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtDetallesTarifa, "Table");
                }
                else
                {   
                    //Inicializando GridView
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
            txtTarifaVacio.Text = 
            txtTarifaTronco.Text = "";
            //Quitando Selección del GridView
            gvTarifamatriz.SelectedIndex = -1;
            
        }
        /// <summary>
        /// Método Privado que Habilita los Controles de los Detalles
        /// </summary>
        /// <param name="enable">Habilitacion de Controles</param>
        private void habilitarControlesTarifaMatriz(bool enable)
        {   
            //Asignando Habilitación de Controles
            txtRotCol.Enabled =
            txtRotFila.Enabled =
            txtCatCol.Enabled =
            txtCatFila.Enabled =
            imgbtnQuitarColumna.Enabled =
            imgbtnQuitarFila.Enabled =
            txtTarifaCargado.Enabled =
            txtTarifaVacio.Enabled =
            txtTarifaTronco.Enabled =
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
            using (DataSet dsDetalles = SAT_CL.TarifasPago.TarifaMatriz.ObtieneDetallesMatrizTarifa(Convert.ToInt32(Session["id_registro"])))
            {
                //Validando que exista contenido en las Tablas
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dsDetalles, "Table") && TSDK.Datos.Validacion.ValidaOrigenDatos(dsDetalles, "Table1"))
                {
                    //Obteniendo Matriz en Blanco
                    DataTable dtDetallesMatrizCargado = creaTabla(dsDetalles.Tables["Table"], dsDetalles.Tables["Table1"]);
                    DataTable dtDetallesMatrizVacio = dtDetallesMatrizCargado.Copy();
                    DataTable dtDetallesMatrizTronco = dtDetallesMatrizCargado.Copy();

                    //Validando que exista una Matriz
                    if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtDetallesMatrizCargado) && TSDK.Datos.Validacion.ValidaOrigenDatos(dsDetalles, "Table2"))
                    {
                        //Recorriendo cada Fila
                        foreach (DataRow dr in dsDetalles.Tables["Table2"].Rows)
                        {
                            //Editando Celdas
                            dtDetallesMatrizCargado.Rows[Convert.ToInt32(dr["PosX"])][Convert.ToInt32(dr["PosY"])] = dr["TarCargado"].ToString();
                            dtDetallesMatrizVacio.Rows[Convert.ToInt32(dr["PosX"])][Convert.ToInt32(dr["PosY"])] = dr["TarVacio"].ToString();
                            dtDetallesMatrizTronco.Rows[Convert.ToInt32(dr["PosX"])][Convert.ToInt32(dr["PosY"])] = dr["TarTronco"].ToString();
                        }

                        //Eliminando la Primera Fila --Opcional
                        dtDetallesMatrizCargado.Rows[0].Delete();
                        dtDetallesMatrizVacio.Rows[0].Delete();
                        dtDetallesMatrizTronco.Rows[0].Delete();

                        //Cargando GridView
                        TSDK.ASP.Controles.CargaGridView(gvMatrizCargada, dtDetallesMatrizCargado, "", "");
                        TSDK.ASP.Controles.CargaGridView(gvMatrizVacia, dtDetallesMatrizVacio, "", "");
                        TSDK.ASP.Controles.CargaGridView(gvMatrizTronco, dtDetallesMatrizTronco, "", "");
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
                foreach (DataRow dr in Columnas.Rows)
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

        #region Métodos Perfil Tarifa

        /// <summary>
        /// Método encargado de Cargar los Valores del Perfil
        /// </summary>
        private void cargaValoresPerfil()
        {
            //Instanciando Columna Filtro
            using (TablaColumnaFiltro tcf = new TablaColumnaFiltro(Convert.ToInt32(ddlTipoColumnaFiltro.SelectedValue)))
            {
                //Validando que exista el Registro
                if (tcf.id_columna_filtro != 0)
                {
                    //Validando que sea de Tipo catalogo
                    if (tcf.id_tipo_catalogo != 0 || tcf.id_tabla_catalogo != 0)
                    {
                        //Habilitando Controles
                        ddlValor.Visible = true;
                        txtValor.Visible = false;
                        //Cargando Catalogo
                        SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlValor, 7, "", Convert.ToInt32(ddlTipoColumnaFiltro.SelectedValue), "", 0, "");
                    }
                    else
                    {
                        //Habilitando Controles
                        ddlValor.Visible = false;
                        txtValor.Visible = true;
                        //Inicializando Control
                        Controles.InicializaDropDownList(ddlValor, "Ninguna");
                    }

                    //Limpiando Control
                    txtValor.Text = "";
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void cargaPerfiles()
        {
            //Validando Estatus
            switch((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {
                        //Obteniendo Origen de Datos
                        if (TSDK.Datos.Validacion.ValidaOrigenDatos(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3")))
                        {
                            //Cargando GridView
                            TSDK.ASP.Controles.CargaGridView(gvPerfilTarifa, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3"), "Id-IdTipo-IdValor", "");
                        }
                        else
                            //Inicializando GridView
                            TSDK.ASP.Controles.InicializaGridview(gvPerfilTarifa);

                        break;
                    }
                case Pagina.Estatus.Edicion:
                    {
                        //Obteniendo Perfiles
                        using (DataTable dtPerfilesTarifa = SAT_CL.TarifasPago.TarifaDetallePerfilPago.ObtienePerfilesTarifa(Convert.ToInt32(Session["id_registro"])))
                        {
                            //Validando que Existan Perfiles
                            if(TSDK.Datos.Validacion.ValidaOrigenDatos(dtPerfilesTarifa))
                            {
                                //Cargando GridView
                                TSDK.ASP.Controles.CargaGridView(gvPerfilTarifa, dtPerfilesTarifa, "Id-IdTipo-IdValor", "");

                                //Añadiendo a Session
                                Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtPerfilesTarifa, "Table3");
                            }
                            else
                            {
                                //Inicializando GridView
                                TSDK.ASP.Controles.InicializaGridview(gvPerfilTarifa);

                                //Eliminando Tabla de Session
                                Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table3");
                            }
                        }

                        break;
                    }
            }

            //Inicializando Indices del GridView
            TSDK.ASP.Controles.InicializaIndices(gvPerfilTarifa);
        }
        /// <summary>
        /// 
        /// </summary>
        private void guardaPerfiles()
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            
            //Inicilaizando Bloque Transaccional
            using(TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Validando que existen Perfiles Temporales
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3")))
                {
                    //Recorriendo Perfiles
                    foreach(DataRow dr in TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3").Rows)
                    {
                        //Insertando Perfil de Tarifa
                        result = SAT_CL.TarifasPago.TarifaDetallePerfilPago.InsertaTarifaDetallePerfilPago(Convert.ToInt32(Session["id_registro"]), Convert.ToInt32(dr["IdTipo"]),
                                    dr["IdValor"].ToString() == "0" ? dr["Valor"].ToString() : dr["IdValor"].ToString(), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                        //Validando que la Operación no fuese Exitosa
                        if (!result.OperacionExitosa)
                            
                            //Terminando Ciclo
                            break;
                    }
                }
                else
                    //Instanciando Positivo el Retorno
                    result = new RetornoOperacion(1);

                //Validando que la Operación fue Correcta
                if (result.OperacionExitosa)
                {
                    //Eliminando Tabla de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table3");
                    
                    //Cargando Perfiles
                    cargaPerfiles();

                    //Completando Transacción
                    trans.Complete();
                }
                else if(result.IdRegistro == -1)
                
                    //Instanciando Excepción
                    result = new RetornoOperacion("No puede agregar un Perfil con este Tipo, ya existe uno previo");

                //Mostrando Mensaje de la Operación
                lblErrorperfil.Text = result.Mensaje;
            }
        }

        #endregion



        #endregion
    }
}