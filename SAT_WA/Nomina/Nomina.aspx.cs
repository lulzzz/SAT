using SAT_CL.Global;
using System;
using System.Data;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;
using System.Collections.Generic;

namespace SAT.Nomina
{
    public partial class Nomina : System.Web.UI.Page
    {
        #region Eventos

        /// <summary>
        /// Evento Producido al Efectuarse una Recarga de Página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Validando que se Produjo un PostBack
            if (!Page.IsPostBack)

                //Invocando Método de Inicialización
                inicializaPagina();
        }
        /// <summary>
        /// Evento que permite seleccionar y ejecutar acciones del menú.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbElementoMenu_Click(object sender, EventArgs e)
        {
            //Creación del objeto botonMenu que obtiene las opciones de los menú desplegable .
            LinkButton botonMenu = (LinkButton)sender;
            //Permite ejecutar acciones determinadas por cada opción del menú
            switch (botonMenu.CommandName)
            {
                //Si la elección del menú es la opción Nuevo
                case "Nuevo":
                    {
                        //Asigna a la variable de session estatus el estado del formulario en nuevo
                        Session["estatus"] = Pagina.Estatus.Nuevo;
                        //Asigna a la variable de session id_registro el valor de 0.
                        Session["id_registro"] = 0;
                        //Invoca al método inicializaPagina
                        inicializaPagina();
                        //Limpia los mensajes de error del lblError
                        lblError.Text = "";
                        break;
                    }
                //Si la elección del menú es la opcion Abrir
                case "Abrir":
                    {
                        //Invoca al Método de Apertura
                        inicializaAperturaRegistro(158, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);
                        break;
                    }
                //Si la elección del menú es la opción Guardar
                case "Guardar":
                    {
                        //Invoca al método Guardado
                        guardaNomina();
                        break;
                    }
                //Si la elección del menú es la opción Editar
                case "Editar":
                    {
                        //Asigna a la variable session estaus el estado de la pagina nuevo
                        Session["estatus"] = Pagina.Estatus.Edicion;
                        //Invoca el método de Inicialización
                        inicializaPagina();
                        //Limpia los mensajes del lblError
                        lblError.Text = "";
                        break;
                    }
                //Si la elección del menú es la opción Eliminar
                case "Eliminar":
                    {
                        //Declarando Objeto de Retorno
                        RetornoOperacion result = new RetornoOperacion();
                        
                        //Instanciando Nomina
                        using (SAT_CL.Nomina.Nomina nom = new SAT_CL.Nomina.Nomina(Convert.ToInt32(Session["id_registro"])))
                        {
                            //Validando que exista la Nomina
                            if (nom.habilitar)
                            {
                                //Deshabilitando Nomina
                                result = nom.DeshabilitaNomina(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                //Validando Resultado Positivo
                                if(result.OperacionExitosa)
                                {
                                    //Asignando Sessión
                                    Session["id_registro"] = 0;
                                    Session["estatus"] = Pagina.Estatus.Nuevo;

                                    //Inicializando Página
                                    inicializaPagina();
                                }

                                //Mostrando Resultado
                                ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
                            }
                        }

                        break;
                    }
                //Si la elección del menú en la opción Bitacora
                case "Bitacora":
                    {
                        //Invoca al método bitacora
                        inicializaBitacora(Session["id_registro"].ToString(), "158", "Nomina");
                        break;
                    }
                //Si la elección del menú en la opcion Referencia
                case "Referencias":
                    {
                        //NO LLEVA REFERENCIAS
                        break;
                    }
                //Si la elección del menú en la opcion Archivo
                case "Archivo":
                    {
                        break;
                    }
                //Si la elección del menú en la opcion Timbrar Nómina
                case "TimbrarTodo":
                    {
                        //Ocultamos controles 
                        btnAceptarTimbradoEmpleado.Visible = false;
                        btnAceptarTimbradoNomina.Visible = true;

                        //Inicializamos Valores de Timbrado
                        inicializaValoresTimbradoNomina();

                        //Mostrando Ventana
                        gestionaVentanas(this, "Timbrar");
                        break;
                    }
                //Si la elección del menú en la opcion Acerca
                case "Acerca":
                    {
                        break;
                    }
                //Si la elección del menú en la opcion Ayuda
                case "Ayuda":
                    {
                        break;
                    }
            }
        }
        /// <summary>
        /// Evento Producido al Dar Click en el Boton "Aceptar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            //Invoca al método Guardado
            guardaNomina();
        }
        /// <summary>
        /// Evento Producido al Dar Click en el Boton "Cancelar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            //Evalua cada estatus de la página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //En caso de que el estado de la página este en modo Nuevo/Edición
                case Pagina.Estatus.Edicion:
                    {
                        //Asignando Estatus de Sesión
                        Session["estatus"] = Pagina.Estatus.Lectura;
                        break;
                    }
            }

            //Invocando Método de Inicialización
            inicializaPagina();
        }
        /// <summary>
        /// Evento Producido al Dar Click en el Boton "Agregar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAgregarEmp_Click(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Insertando Nomina de Empleado
            result = SAT_CL.Nomina.NominaEmpleado.InsertaNominaEmpleado(Convert.ToInt32(Session["id_registro"]), Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtEmpleado.Text, "ID:", 1)), 
                                                                        0, 0, 0, 0, 0, 0, 0, 0, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

            //Validando que la Operación fue Exitosa
            if (result.OperacionExitosa)
            {
                //Invocando Método de Carga de Nominas
                cargaNominaEmpleados();

                //Limpiando Control
                txtEmpleado.Text = "";
            }
            
            //Mostrando Mensaje de Confirmación
            ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Evento Producido al Aceptar el Timbrado de la Nómina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarTimbrado_Click(object sender, EventArgs e)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion res = new RetornoOperacion();            
            //Creación del objeto Button que obtiene las opciones de los menú desplegable .
            Button botonMenu = (Button)sender;
            //Permite ejecutar acciones determinadas por cada opción del menú
            switch (botonMenu.CommandName)
            {
                case"TimbrarEmpleado":
                    res = TimbraEmpleado();
                    break;
                case "TimbrarNomina":
                    res = TimbraNomina();
                    break;
            }

            //Ocultando Ventana
            gestionaVentanas(this, "Timbrar");

            //Mostrando Resultado Obtenido
            ScriptServer.MuestraNotificacion(this, res, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Evento Producido al Cancelar el Timbrado de la Nómina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarCancelarNomina_Click(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Instanciando Nómina Empleado
            using (SAT_CL.Nomina.NominaEmpleado ne = new SAT_CL.Nomina.NominaEmpleado(Convert.ToInt32(gvNominaEmpleado.SelectedDataKey["Id"])))
            {
                //Validando que exista el Registro
                if (ne.habilitar)

                    //Timbrando Nómina del Empleado
                    result = ne.ActualizaEstatus(SAT_CL.Nomina.NominaEmpleado.Estatus.Cancelado, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                else
                    //Instanciando Excepción
                    result = new RetornoOperacion("No Existe la Nómina del Empleado");

                //Validando Operación Exitosa
                if (result.OperacionExitosa)
                {
                    //Cargando Nomina de Empleados
                    cargaNominaEmpleados();

                    //Marcando Fila
                    Controles.MarcaFila(gvNominaEmpleado, ne.id_nomina_empleado.ToString(), "Id", "Id", OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), lblOrdenado.Text, Convert.ToInt32(ddlTamano.SelectedValue), true, 1);

                    //Sumando Totales
                    sumaTotalesNominaEmpleado();
                }
            }

            //Ocultando Ventana
            gestionaVentanas(this, "Cancelar");

            //Mostrando Resultado Obtenido
            ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Evento Producido al Dar Click en el Link "Exportar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportar_Click(object sender, EventArgs e)
        {
            //Obteniendo Control
            LinkButton lnk = (LinkButton)sender;

            //Validando el Comando
            switch(lnk.CommandName)
            {
                case "NominaEmpleados":
                    {
                        //Exportando Contenido
                        Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"));
                        break;
                    }
                case "DetalleNomina":
                    {
                        //Exportando Contenido
                        Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"));
                        break;
                    }
                case "NominaOtros":
                    {
                        //Exportando Contenido
                        Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"));
                        break;
                    }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtFecIniPago_TextChanged(object sender, EventArgs e)
        {
            //Invocando Método de Calculo
            calculaDiasPago();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtFecFinPago_TextChanged(object sender, EventArgs e)
        {
            //Invocando Método de Calculo
            calculaDiasPago();
        }
        /// <summary>
        /// Evento que permte imprimir el formato de impresión de Finiquito
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkFiniquito_Click(object sender, EventArgs e)
        {
            //Valida que existan datos en el gridview
            if(gvNominaEmpleado.DataKeys.Count != 0)
            {
                //Selecciona la fila del gridview
                Controles.SeleccionaFila(gvNominaEmpleado,sender,"lnk",false);
                //Obtiene la ruta y datos para el reporte de finiquito
                string urlReporte = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Nomina/Nomina.aspx", "~/RDLC/Reporte.aspx");
                //Instancia a nueva ventana de navegador para la apertura de registro
                TSDK.ASP.ScriptServer.AbreNuevaVentana(string.Format("{0}?idTipoReporte={1}&idRegistro={2}", urlReporte, "Finiquito", Convert.ToInt32(gvNominaEmpleado.SelectedDataKey["Id"])), "Finiquito", "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500", Page);
            }
        }
        
        #region Eventos GridView "Nomina Empleados"
        /// <summary>
        /// Evento Disparado al Cambiar el Tamaño del GridView "Nomina Empleados"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamano_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambio de Tamaño
            Controles.CambiaTamañoPaginaGridView(gvNominaEmpleado, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamano.SelectedValue), true, 1);

            //Mostrando Totales
            sumaTotalesNominaEmpleado();
        }
        /// <summary>
        /// Evento Disparado al Cambiar la Expresión de Ordenamiento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvNominaEmpleado_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión
            lblOrdenado.Text = Controles.CambiaSortExpressionGridView(gvNominaEmpleado, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression, true, 1);

            //Mostrando Totales
            sumaTotalesNominaEmpleado();
        }
        /// <summary>
        /// Evento Disparado al Cambiar el Indice de Página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvNominaEmpleado_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Página
            Controles.CambiaIndicePaginaGridView(gvNominaEmpleado, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, true, 1);

            //Mostrando Totales
            sumaTotalesNominaEmpleado();
        }
        /// <summary>
        /// Evento Producido al Dar Click al Link de Percepciones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkPercepcion_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvNominaEmpleado.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvNominaEmpleado, sender, "lnk", false);

                //Obteniendo Control
                LinkButton lnk = (LinkButton)sender;

                //Instanciando Detalle de Nomina
                using (SAT_CL.Nomina.NominaEmpleado ne = new SAT_CL.Nomina.NominaEmpleado(Convert.ToInt32(gvNominaEmpleado.SelectedDataKey["Id"])))
                {
                    //Validando que exista la nomina del Empleado
                    if (ne.habilitar)
                    {
                        //Mostrando Ventana
                        gestionaVentanas(this, "DetalleNomina");
                        
                        //Validando Comando
                        switch (lnk.CommandName)
                        {
                            case "Aguinaldo":
                                {
                                    //Inicializando Controles
                                    inicializaValoresDetalle(0, SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion.Bonificacion, 2);

                                    //Cargando de Detalles de Nomina
                                    cargaDetallesNomina(ne.id_nomina_empleado, 2, SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion.Percepcion);
                                    break;
                                }
                            case "Sueldo":
                                {
                                    //Inicializando Controles
                                    inicializaValoresDetalle(0, SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion.Percepcion, 1);

                                    //Cargando de Detalles de Nomina
                                    cargaDetallesNomina(ne.id_nomina_empleado, 1, SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion.Percepcion);
                                    break;
                                }
                            case "Otros":
                                {
                                    //Inicializando Controles
                                    inicializaValoresDetalle(0, SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion.Bonificacion, 0);

                                    //Cargando de Detalles de Nomina
                                    cargaDetallesNomina(ne.id_nomina_empleado, 0, SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion.Percepcion);
                                    break;
                                }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Evento Producido al Dar Click al Link de Deducciones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkDeduccion_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvNominaEmpleado.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvNominaEmpleado, sender, "lnk", false);

                //Obteniendo Control
                LinkButton lnk = (LinkButton)sender;

                //Instanciando Detalle de Nomina
                using (SAT_CL.Nomina.NominaEmpleado ne = new SAT_CL.Nomina.NominaEmpleado(Convert.ToInt32(gvNominaEmpleado.SelectedDataKey["Id"])))
                {
                    //Validando que exista la nomina del Empleado
                    if (ne.habilitar)
                    {
                        //Mostrando Ventana
                        gestionaVentanas(this, "DetalleNomina");

                        //Validando Comando
                        switch (lnk.CommandName)
                        {
                            case "IMSS":
                                {
                                    //Inicializando Controles
                                    inicializaValoresDetalle(0, SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion.Deduccion, 1);

                                    //Cargando de Detalles de Nomina
                                    cargaDetallesNomina(ne.id_nomina_empleado, 1, SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion.Deduccion);
                                    break;
                                }
                            case "ISPT":
                                {
                                    //Inicializando Controles
                                    inicializaValoresDetalle(0, SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion.Deduccion, 2);

                                    //Cargando de Detalles de Nomina
                                    cargaDetallesNomina(ne.id_nomina_empleado, 2, SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion.Deduccion);
                                    break;
                                }
                            case "Infonavit":
                                {
                                    //Inicializando Controles
                                    inicializaValoresDetalle(0, SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion.Deduccion, 9);

                                    //Cargando de Detalles de Nomina
                                    cargaDetallesNomina(ne.id_nomina_empleado, 9, SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion.Deduccion);
                                    break;
                                }
                            case "Otros":
                                {
                                    //Inicializando Controles
                                    inicializaValoresDetalle(0, SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion.Deduccion, 0);

                                    //Cargando de Detalles de Nomina
                                    cargaDetallesNomina(ne.id_nomina_empleado, 0, SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion.Deduccion);
                                    break;
                                }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Evento Producido al Dar Click en el Link de Hrs. Extra
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkHrsExtra_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvNominaEmpleado.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvNominaEmpleado, sender, "lnk", false);

                //Nomina Otros
                gestionaVentanas(this, "NominaOtros");

                //Habilitando Controles
                inicializaValoresNominaOtros(0, 2);

                //Cargando Nomina Otros
                cargaNominaOtros(Convert.ToInt32(gvNominaEmpleado.SelectedDataKey["Id"]), 2);
            }
        }
        /// <summary>
        /// Evento Producido al Dar Click en el Link de Incapacidad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkIncapacidad_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvNominaEmpleado.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvNominaEmpleado, sender, "lnk", false);

                //Nomina Otros
                gestionaVentanas(this, "NominaOtros");

                //Habilitando Controles
                inicializaValoresNominaOtros(0, 1);

                //Cargando Nomina Otros
                cargaNominaOtros(Convert.ToInt32(gvNominaEmpleado.SelectedDataKey["Id"]), 1);
            }
        }
        /// <summary>
        /// Evento Producido al Dar Click en el Link
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkActualizaNomina_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvNominaEmpleado.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvNominaEmpleado, sender, "lnk", false);

                //Declarando Objeto de Retorno
                RetornoOperacion result = new RetornoOperacion();

                //Instanciando Nomina de Empleado
                using (SAT_CL.Nomina.NominaEmpleado ne = new SAT_CL.Nomina.NominaEmpleado(Convert.ToInt32(gvNominaEmpleado.SelectedDataKey["Id"])))
                {
                    //Validando Registro
                    if (ne.habilitar)
                    {
                        //Obteniendo Control
                        LinkButton lnk = (LinkButton)sender;

                        //Validando Comando
                        switch (lnk.CommandName)
                        {
                            case "Eliminar":
                                {
                                    //Deshabilitando Nomina de Empleado
                                    result = ne.DeshabilitaNominaEmpleado(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                    //Operación Exitosa?
                                    if (result.OperacionExitosa)
                                    {
                                        //Inicializando Indices
                                        Controles.InicializaIndices(gvNominaEmpleado);

                                        //Recargando Nomina de Empleados
                                        cargaNominaEmpleados();
                                    }

                                    //Mostrando Excepción
                                    ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
                                    break;
                                }
                            case "Timbrar":
                                {
                                    //Validando que la Nomina este Registrada
                                    if (ne.estatus == SAT_CL.Nomina.NominaEmpleado.Estatus.Registrado)
                                    {
                                        //Ocultamos controles 
                                        btnAceptarTimbradoEmpleado.Visible = true;
                                        btnAceptarTimbradoNomina.Visible = false;
                                        //Abriendo Ventana de Confirmación
                                        gestionaVentanas(this, "Timbrar");

                                        //Inicializamos Valores de Timbrado
                                        inicializaValoresTimbradoNomina();
                                    }
                                    else
                                    {
                                        //Instanciando Excepción
                                        result = new RetornoOperacion(string.Format("La Nomina del Empleado se encuentra '{0}', Imposible su Timbrado", ne.estatus.ToString()));

                                        //Mostrando Excepción
                                        ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
                                    }
                                    break;
                                }
                            case "Cancelar":
                                {
                                    //Validando que la Nomina este Registrada
                                    if (ne.estatus == SAT_CL.Nomina.NominaEmpleado.Estatus.Timbrado)

                                        //Abriendo Ventana de Confirmación
                                        gestionaVentanas(this, "Cancelar");
                                    else
                                    {
                                        //Instanciando Excepción
                                        result = new RetornoOperacion(string.Format("La Nomina del Empleado se encuentra '{0}', Imposible su Cancelación", ne.estatus.ToString()));

                                        //Mostrando Excepción
                                        ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
                                    }
                                    break;
                                }
                        }
                    }
                    else
                    {
                        //Instanciando Excepción
                        result = new RetornoOperacion("No Existe la Nomina del Empleado");

                        //Mostrando Excepción
                        ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
                    }
                }
            }
        }
        /// <summary>
        /// Evento que permite imprimir el formato de comprobante de nomina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkImprimirNomina_Click(object sender, EventArgs e)
        {
            //Valida que existan registros en el grid view
            if (gvNominaEmpleado.DataKeys.Count != 0)
            {
                //Selecciona la fila del gridview
                Controles.SeleccionaFila(gvNominaEmpleado, sender, "lnk", false);

                //Instanciando Nómina Empleado
                using (SAT_CL.Nomina.NominaEmpleado ne = new SAT_CL.Nomina.NominaEmpleado(Convert.ToInt32(gvNominaEmpleado.SelectedDataKey["Id"])))
                {
                    //Obteniendo Control
                    LinkButton lnk = (LinkButton)sender;

                    //Validando Comando
                    switch (lnk.CommandName)
                    {
                        case "PDF":
                            {
                                //Obteniendo Ruta
                                string urlReporte = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Nomina/Nomina.aspx", "~/RDLC/Reporte.aspx");

                                //Instanciando nueva ventana de navegador para apertura de registro
                                TSDK.ASP.ScriptServer.AbreNuevaVentana(string.Format("{0}?idTipoReporte={1}&idRegistro={2}", urlReporte, "ComprobanteNomina", ne.id_nomina_empleado), "Comprobante", "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500", Page);
                                break;
                            }
                        case "XML":
                            {
                                //Invocando Método de Descarga
                                descargarXML(ne.id_comprobante);
                                break;
                            }
                    }
                }
            }
        }
        #endregion

        /// <summary>
        /// Evento Generado al cambiar Banco
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlBanco_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Catalogo de Cuentas
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlCuenta, 23, "Ninguno", Convert.ToInt32(ddlBanco.SelectedValue), "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "");

        }

        /// <summary>
        /// Evento Producido al Cerrar la Ventana Modal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkCerrar_Click(object sender, EventArgs e)
        {
            //Obteniendo Control
            LinkButton lnk = (LinkButton)sender;

            //Validando Comando
            switch(lnk.CommandName)
            {
                case "DetalleNomina":
                    {
                        //Invocando Método de Gestión de Ventanas
                        gestionaVentanas(this, "DetalleNomina");
                        break;
                    }
                case "NominaOtros":
                    {
                        //Invocando Método de Gestión de Ventanas
                        gestionaVentanas(this, "NominaOtros");
                        break;
                    }
                case "TimbradoNomina":
                    {
                        //Invocando Método de Gestión de Ventanas
                        gestionaVentanas(this, "Timbrar");
                        break;
                    }
                case "CanceladoNomina":
                    {
                        //Invocando Método de Gestión de Ventanas
                        gestionaVentanas(this, "Cancelar");
                        break;
                    }
            }
        }

        #region Eventos Modal "Detalle Nomina"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardarDet_Click(object sender, EventArgs e)
        {
            //Invocando Método de Guardado
            guardaDetalleNomina();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarDet_Click(object sender, EventArgs e)
        {
            //Invocando Método de Gestión de Ventanas
            gestionaVentanas(this, "DetalleNomina");
        }

        #endregion

        #region Eventos GridView "Detalle Nomina"

        /// <summary>
        /// Evento Disparado al Cambiar el Tamaño del GridView "Detalle Nomina"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoDet_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambio de Tamaño
            Controles.CambiaTamañoPaginaGridView(gvDetalleNomina, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), Convert.ToInt32(ddlTamanoDet.SelectedValue), true, 1);
        }
        /// <summary>
        /// Evento Disparado al Cambiar la Expresión de Ordenamiento del GridView "Detalle Nomina"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDetalleNomina_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión
            lblOrdenadoDet.Text = Controles.CambiaSortExpressionGridView(gvDetalleNomina, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.SortExpression, true, 1);
        }
        /// <summary>
        /// Evento Disparado al Cambiar el Indice de Página del GridView "Detalle Nomina"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDetalleNomina_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Página
            Controles.CambiaIndicePaginaGridView(gvDetalleNomina, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.NewPageIndex, true, 1);
        }
        /// <summary>
        /// Evento Producido al Editar el Detalle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEditar_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvDetalleNomina.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvDetalleNomina, sender, "lnk", false);

                //Declarando Objeto de Retorno
                RetornoOperacion result = new RetornoOperacion();
                
                //Instanciando Detalle
                using (SAT_CL.Nomina.DetalleNominaEmpleado dne = new SAT_CL.Nomina.DetalleNominaEmpleado(Convert.ToInt32(gvDetalleNomina.SelectedDataKey["Id"])))
                {
                    //Validando Habilitación
                    if (dne.habilitar)
                    {
                        //Asignando Valores
                        ddlConcepto.SelectedValue = dne.id_tipo_pago.ToString();
                        txtImporteGravado.Text = dne.importe_gravado.ToString();
                        txtImporteExento.Text = dne.importe_exento.ToString();
                    }
                }
            }
        }
        /// <summary>
        /// Evento Producido al Eliminar el Detalle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEliminar_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvDetalleNomina.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvDetalleNomina, sender, "lnk", false);

                //Declarando Objeto de Retorno
                RetornoOperacion result = new RetornoOperacion();

                //Instanciando Detalle
                using (SAT_CL.Nomina.DetalleNominaEmpleado dne = new SAT_CL.Nomina.DetalleNominaEmpleado(Convert.ToInt32(gvDetalleNomina.SelectedDataKey["Id"])))
                
                //Instanciando Tipo Cobro Recurrente
                using(SAT_CL.Liquidacion.TipoCobroRecurrente tcr = new SAT_CL.Liquidacion.TipoCobroRecurrente(dne.id_tipo_pago))
                {
                    //Validando Habilitación
                    if (dne.habilitar)
                    
                        //Deshabilitando Detalle
                        result = dne.DeshabilitarDetalleNominaEmpleado(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                    else
                        //Instanciando Excepción
                        result = new RetornoOperacion("No existe el Detalle de Nomina");

                    //Validando que la Operación fuese Exitosa
                    if (result.OperacionExitosa)
                    {
                        //Validando Percepción
                        if (((SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion)tcr.id_tipo_aplicacion == SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion.Percepcion ||
                            (SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion)tcr.id_tipo_aplicacion == SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion.Bonificacion) &&
                            (tcr.id_concepto_sat_nomina == 1 || tcr.id_concepto_sat_nomina == 2))
                        {
                            //Inicializando Valores del Detalle
                            inicializaValoresDetalle(0, (SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion)tcr.id_tipo_aplicacion, tcr.id_concepto_sat_nomina);

                            //Cargando Detalles de Nomina
                            cargaDetallesNomina(dne.id_nomina_empleado, tcr.id_concepto_sat_nomina, (SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion)tcr.id_tipo_aplicacion);
                        }
                        else if ((SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion)tcr.id_tipo_aplicacion == SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion.Deduccion &&
                                    (tcr.id_concepto_sat_nomina == 1 || tcr.id_concepto_sat_nomina == 2 || tcr.id_concepto_sat_nomina == 9))
                        {
                            //Inicializando Valores del Detalle
                            inicializaValoresDetalle(0, (SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion)tcr.id_tipo_aplicacion, tcr.id_concepto_sat_nomina);

                            //Cargando Detalles de Nomina
                            cargaDetallesNomina(dne.id_nomina_empleado, tcr.id_concepto_sat_nomina, (SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion)tcr.id_tipo_aplicacion);
                        }
                        else
                        {
                            //Inicializando Valores del Detalle
                            inicializaValoresDetalle(0, (SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion)tcr.id_tipo_aplicacion, 0);

                            //Cargando Detalles de Nomina
                            cargaDetallesNomina(dne.id_nomina_empleado, 0, (SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion)tcr.id_tipo_aplicacion);
                        }

                        //Inicializando Indices
                        Controles.InicializaIndices(gvDetalleNomina);

                        //Obteniendo Nomina de Empleado
                        int nomina_empleado = Convert.ToInt32(gvNominaEmpleado.SelectedDataKey["Id"]);

                        //Cargando Nomina de Empleados
                        cargaNominaEmpleados();

                        // Marcando Fila
                        Controles.MarcaFila(gvNominaEmpleado, nomina_empleado.ToString(), "Id", "Id", OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), lblOrdenado.Text, Convert.ToInt32(ddlTamano.SelectedValue), true, 1);
                        upgvNominaEmpleado.Update();

                        //Mostrando Totales
                        sumaTotalesNominaEmpleado();
                    }

                    //Mostrando Mensaje de Operación
                    ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
                }
            }
        }

        #endregion

        #region Eventos Nomina Otros

        /// <summary>
        /// Evento Producido el Presionar el Boton "Guardar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardarNO_Click(object sender, EventArgs e)
        {
            //Invocando Método de Guardado
            guardaNominaOtros();
        }
        /// <summary>
        /// Evento Producido el Presionar el Boton "Cancelar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarNO_Click(object sender, EventArgs e)
        {
            //Invocando Método de Gestión de Ventanas
            gestionaVentanas(this, "NominaOtros");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cargando Catalogo
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlSubTipo, "", 3150, Convert.ToInt32(ddlTipo.SelectedValue));

            //Validando Tipo de Nomina Otros
            if (Convert.ToInt32(ddlTipo.SelectedValue) == 2)

                //Viasualizando Control
                txtCantidad.Enabled = true;

            else if (Convert.ToInt32(ddlTipo.SelectedValue) == 1)
                //Viasualizando Control
                txtCantidad.Enabled = false;
        }

        #endregion

        #region Eventos GridView "Nomina Otros"

        /// <summary>
        /// Evento Disparado al Cambiar el Tamaño del GridView "Nomina Otros"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoNO_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambio de Tamaño
            Controles.CambiaTamañoPaginaGridView(gvNominaOtros, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), Convert.ToInt32(ddlTamanoNO.SelectedValue), true, 1);
        }
        /// <summary>
        /// Evento Disparado al Cambiar la Expresión de Ordenamiento del GridView "Nomina Otros"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvNominaOtros_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión
            lblOrdenadoNO.Text = Controles.CambiaSortExpressionGridView(gvNominaOtros, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), e.SortExpression, true, 1);
        }
        /// <summary>
        /// Evento Disparado al Cambiar el Indice de Página del GridView "Detalle Nomina"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvNominaOtros_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Página
            Controles.CambiaIndicePaginaGridView(gvNominaOtros, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), e.NewPageIndex, true, 1);
        }
        /// <summary>
        /// Evento Producido al Editar la Nomina Otros
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEditarNO_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if(gvNominaOtros.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvNominaOtros, sender, "lnk", false);

                //Instanciando Nomina Otros
                using(SAT_CL.Nomina.NominaOtros no = new SAT_CL.Nomina.NominaOtros(Convert.ToInt32(gvNominaOtros.SelectedDataKey["Id"])))
                {
                    //Validando que exista el Registro
                    if(no.habilitar)
                    
                        //Inicializando Valores
                        inicializaValoresNominaOtros(no.id_nomina_otros, no.id_tipo);
                }
            }
        }
        /// <summary>
        /// Evento Producido al Eliminar la Nomina Otros
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEliminarNO_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvNominaOtros.DataKeys.Count > 0)
            {
                //Declarando Objeto de Retorno
                RetornoOperacion result = new RetornoOperacion();
                
                //Seleccionando Fila
                Controles.SeleccionaFila(gvNominaOtros, sender, "lnk", false);

                //Instanciando Nomina Otros
                using (SAT_CL.Nomina.NominaOtros no = new SAT_CL.Nomina.NominaOtros(Convert.ToInt32(gvNominaOtros.SelectedDataKey["Id"])))
                {
                    //Validando que exista el Registro
                    if (no.habilitar)

                        //Deshabilitando Nomina de Otros
                        result = no.DeshabilitarNominaOtros(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                    //Validando Operación Exitosa
                    if (result.OperacionExitosa)
                    {
                        //Inicializando Valores
                        inicializaValoresNominaOtros(0, no.id_tipo);

                        //Obteniendo Nomina de Empleado
                        int nomina_empleado = Convert.ToInt32(gvNominaEmpleado.SelectedDataKey["Id"]);

                        //Actualizando Nomina Otros
                        cargaNominaOtros(nomina_empleado, no.id_tipo);

                        //Cargando Nomina de Empleados
                        cargaNominaEmpleados();

                        // Marcando Fila
                        Controles.MarcaFila(gvNominaEmpleado, nomina_empleado.ToString(), "Id", "Id", OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), lblOrdenado.Text, Convert.ToInt32(ddlTamano.SelectedValue), true, 1);
                        upgvNominaEmpleado.Update();

                        //Mostrando Totales
                        sumaTotalesNominaEmpleado();
                    }
                }

                //Mostrando Resultado
                ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
        }
        /// <summary>
        /// Click en botón email
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEmailEmpleado_Click(object sender, EventArgs e)
        {
            //Validando existencia de registros
            if (gvNominaEmpleado.DataKeys.Count > 0)
            {
                //Seleccionando Registro
                Controles.SeleccionaFila(gvNominaEmpleado, sender, "lnk", false);

                //Instanciando nomina empleado
                using (SAT_CL.Nomina.NominaEmpleado ne = new SAT_CL.Nomina.NominaEmpleado(Convert.ToInt32(gvNominaEmpleado.SelectedDataKey["Id"])))
                {                    
                    //Instanciando comprobante
                    using (SAT_CL.FacturacionElectronica.Comprobante rn = new SAT_CL.FacturacionElectronica.Comprobante(ne.id_comprobante))
                    {
                        //Si la nómina del empleado está timbrada
                        if (rn.id_comprobante > 0)
                        {
                            //Declarando auxiliar de email de entrega de correo
                            string email_entrega = "";

                            //Instanciando Empresa Emisora
                            using (SAT_CL.Global.CompaniaEmisorReceptor emisor = new CompaniaEmisorReceptor(rn.id_compania_emisor))
                            {
                                //cargando referencias de correo electronico de envío
                                using (DataTable mitReferencias = SAT_CL.Global.Referencia.CargaReferencias(ne.id_empleado, 76, SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(emisor.id_compania_emisor_receptor, 76, "Correo Electrónico", 0, "Recibo Nómina")))
                                    email_entrega = mitReferencias != null ? mitReferencias.Rows[0]["Valor"].ToString() : "";
                                {
                                    //Instanciando nómina
                                    using (SAT_CL.Nomina.Nomina nom = new SAT_CL.Nomina.Nomina(ne.id_nomina))
                                    {
                                        //Instanciando usuario
                                        using (SAT_CL.Seguridad.Usuario u = new SAT_CL.Seguridad.Usuario(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_usuario))
                                        {

                                            //Inicializando control de usuario de envío de email
                                            wucEmailCFDI.InicializaControl(u.email, string.Format("CFDI NOM. {0:dd/MM/yyyy} '{1}{2}' [{3}]", nom.fecha_fin_pago, rn.serie, rn.folio, emisor.rfc), email_entrega,
                                               "Los archivos se encuentran adjuntos en este mensaje. Ante cualquier aclaración contacte al remitente de este correo.", ne.id_comprobante);

                                            //Mostrando ventana modal correspondiente
                                            ScriptServer.AlternarVentana(this, "Email", "contenidoConfirmacionEmail", "confirmacionEmail");
                                        }
                                    }
                                }
                            }
                        }
                        //De lo contrario
                        else
                            //Mostrando error
                            ScriptServer.MuestraNotificacion(this, "El pago de nómina de este empleado no se ha timbrado.", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                    }
                }
            }
        }
        /// <summary>
        /// Click en botón enviar email
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnEnviarEmail_Click(object sender, EventArgs e)
        { 
            //Realizando el envío del correo
            RetornoOperacion resultado = wucEmailCFDI.EnviaEmail(true, true);

            //Si no hay errores
            if(resultado.OperacionExitosa)
                //Cerrando ventana modal
                ScriptServer.AlternarVentana(this, "Email", "contenidoConfirmacionEmail", "confirmacionEmail");

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Click en botón cerrar control de correo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LkbCerrarEmail_Click(object sender, EventArgs e)
        { 
            //Cerrando ventana modal
            ScriptServer.AlternarVentana(this, "Email", "contenidoConfirmacionEmail", "confirmacionEmail");
        }

        #endregion

        #endregion

        #region Métodos
        /// <summary>
        /// Método encargado de Inicializar la Página
        /// </summary>
        private void inicializaPagina()
        {
            //Obteniendo Comprobante
            string id_nomina = Request.QueryString["idRegistro"];

            //Validando que Exista
            if (id_nomina != null)
            {
                //Añadiendo Resultado a Session
                Session["id_registro"] = Convert.ToInt32(id_nomina);

                //Cambiando a Edición
                Session["estatus"] = Pagina.Estatus.Edicion;
            }
            
            //Invocando Método de Carga
            cargaCatalogos();
            //Invocando Método de Habilitación del Menú
            habilitaMenu();
            //Invocando Método de Habilitación del Controles
            habilitaControles();
            //Invocando Método de Inicialización de Valores
            inicializaValores();
            //Invocando Método de Carga de Nominas
            cargaNominaEmpleados();
        }
        /// <summary>
        /// Método encargado de Cargar los Catalogos
        /// </summary>
        private void cargaCatalogos()
        {
            //Cargando Tipos de Periodicidad de Pago
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlPeriodicidadPago, "", 3147);
            //Cargando Tipos de Método de Pago
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlMetodoPago, "", 80);
            //Cargando Catalogos de Tamaño
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamano, "", 26);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoDet, "", 26);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoNO, "", 26);
            //Catalogo Bancos
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlBanco, 22, "Ninguno");
            //Catalogo de Cuentas
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlCuenta, 23, "Ninguno", Convert.ToInt32(ddlBanco.SelectedValue), "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "");
        }
        /// <summary>
        /// Método encargado de habilitar las Opciones del Menú
        /// </summary>
        private void habilitaMenu()
        {
            //Evalua cada estatus de la página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //En caso de que el estado de la página este en modo Nuevo
                case Pagina.Estatus.Nuevo:
                    {
                        lkbNuevo.Enabled =
                        lkbAbrir.Enabled =
                        lkbGuardar.Enabled = true;
                        lkbEliminar.Enabled =
                        lkbEditar.Enabled =
                        lkbBitacora.Enabled =
                        lkbReferencias.Enabled =
                        lkbTimbrarTodo.Enabled =
                        lkbArchivos.Enabled = false;
                        lkbAcercaDe.Enabled =
                        lkbAyuda.Enabled = true;
                        break;
                    }
                //En caso de que el estado de la página este en modo de lectura
                case Pagina.Estatus.Lectura:
                    {
                        lkbNuevo.Enabled =
                        lkbAbrir.Enabled = true;
                        lkbGuardar.Enabled = false;
                        lkbEliminar.Enabled =
                        lkbEditar.Enabled =
                        lkbBitacora.Enabled =
                        lkbReferencias.Enabled =
                        lkbTimbrarTodo.Enabled = true;
                        lkbArchivos.Enabled = false;
                        lkbAcercaDe.Enabled =
                        lkbAyuda.Enabled = true;
                        break;
                    }
                //En caso de que el estado de la página este en modo edición
                case Pagina.Estatus.Edicion:
                    {
                        lkbNuevo.Enabled =
                        lkbAbrir.Enabled =
                        lkbGuardar.Enabled =
                        lkbEliminar.Enabled = true;
                        lkbEditar.Enabled = false;
                        lkbBitacora.Enabled =
                        lkbReferencias.Enabled = true;
                        lkbTimbrarTodo.Enabled = true;
                        lkbArchivos.Enabled = false;
                        lkbAcercaDe.Enabled =
                        lkbAyuda.Enabled = true;
                        break;
                    }
            }
        }
        /// <summary>
        /// Método encargado de Habilitar los Controles de la Forma
        /// </summary>
        private void habilitaControles()
        {
            //Evalua cada estatus de la página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //En caso de que el estado de la página este en modo Nuevo/Edición
                case Pagina.Estatus.Nuevo:
                    {
                        //Encabezado Nomina
                        txtFecIniPago.Enabled =
                        txtFecFinPago.Enabled =
                        txtDiasPago.Enabled =
                        txtFechaPago.Enabled =
                        txtFecNomina.Enabled =
                        txtSucursal.Enabled =
                        ddlPeriodicidadPago.Enabled =
                        ddlMetodoPago.Enabled =
                        btnAceptar.Enabled =
                        btnCancelar.Enabled = true;

                        //Nomina Empleado
                        txtEmpleado.Enabled =
                        btnAgregarEmp.Enabled =
                        gvNominaEmpleado.Enabled = false;
                        break;
                    }
                //En caso de que el estado de la página este en modo de lectura
                case Pagina.Estatus.Lectura:
                    {
                        //Encabezado Nomina
                        txtFecIniPago.Enabled =
                        txtFecFinPago.Enabled =
                        txtDiasPago.Enabled =
                        txtFechaPago.Enabled =
                        txtFecNomina.Enabled =
                        txtSucursal.Enabled =
                        ddlPeriodicidadPago.Enabled =
                        ddlMetodoPago.Enabled =
                        btnAceptar.Enabled =
                        btnCancelar.Enabled = 

                        //Nomina Empleado
                        txtEmpleado.Enabled =
                        btnAgregarEmp.Enabled =
                        gvNominaEmpleado.Enabled = false;
                        break;
                    }
                case Pagina.Estatus.Edicion:
                    {
                        //Encabezado Nomina
                        txtFecIniPago.Enabled =
                        txtFecFinPago.Enabled =
                        txtDiasPago.Enabled =
                        txtFechaPago.Enabled =
                        txtFecNomina.Enabled =
                        txtSucursal.Enabled =
                        ddlPeriodicidadPago.Enabled =
                        ddlMetodoPago.Enabled =
                        btnAceptar.Enabled =
                        btnCancelar.Enabled =

                        //Nomina Empleado
                        txtEmpleado.Enabled =
                        btnAgregarEmp.Enabled =
                        gvNominaEmpleado.Enabled = true;
                        break;
                    }
            }
        }
        /// <summary>
        /// Método encargado de Timbrar la Nómina del Empleado
        /// </summary>
        private RetornoOperacion TimbraEmpleado()
        {
            //Declarando Objeto Resultado
            RetornoOperacion result = new RetornoOperacion();
            //Instanciando Nómina Empleado
            using (SAT_CL.Nomina.NominaEmpleado ne = new SAT_CL.Nomina.NominaEmpleado(Convert.ToInt32(gvNominaEmpleado.SelectedDataKey["Id"])))
            {
                //Validando que exista el Registro
                if (ne.habilitar)
                {
                    //Timbrando Nómina del Empleado
                    result = ne.ImportaTimbraNominaEmpleadoComprobante_V3_2(txtSerie.Text, Convert.ToInt32(ddlCuenta.SelectedValue), HostingEnvironment.MapPath("~/XSLT/FacturacionElectronica/cadenaoriginal_3_2.xslt"),
                                                                         HostingEnvironment.MapPath("~/XSLT/FacturacionElectronica/cadenaoriginal_3_2_desconectado.xslt"), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);//*/
                }
                else
                    //Instanciando Excepción
                    result = new RetornoOperacion("No Existe la Nómina del Empleado");

                //Validando Operación Exitosa
                if (result.OperacionExitosa)
                {
                    //Cargando Nomina de Empleados
                    cargaNominaEmpleados();

                    //Marcando Fila
                    Controles.MarcaFila(gvNominaEmpleado, ne.id_nomina_empleado.ToString(), "Id", "Id", OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), lblOrdenado.Text, Convert.ToInt32(ddlTamano.SelectedValue), true, 1);

                    //Sumando Totales
                    sumaTotalesNominaEmpleado();
                }
            }
            //Delvolvemos Resultado
            return result;
        }
        /// <summary>
        /// Método encargado de Timbrar la Nómina 
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion TimbraNomina()
        {
            //Declarando Objeto Resultado
            RetornoOperacion result = new RetornoOperacion();
            //Instanciando Nomina
            using (SAT_CL.Nomina.Nomina nom = new SAT_CL.Nomina.Nomina(Convert.ToInt32(Session["id_registro"])))
            {

                //Validando que exista el Registro
                if (nom.habilitar)
                {
                    //Timbrando Nómina
                    result = nom.TimbraNomina_V3_2(txtSerie.Text, Convert.ToInt32(ddlCuenta.SelectedValue), HostingEnvironment.MapPath("~/XSLT/FacturacionElectronica/cadenaoriginal_3_2.xslt"),
                                                                         HostingEnvironment.MapPath("~/XSLT/FacturacionElectronica/cadenaoriginal_3_2_desconectado.xslt"), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                }
                else
                    //Instanciando Excepción
                    result = new RetornoOperacion("No Existe la Nómina.");

                //Validando Operación Exitosa
                if (result.OperacionExitosa)
                {
                    //Cargando Nomina de Empleados
                    cargaNominaEmpleados();

                    //Sumando Totales
                    sumaTotalesNominaEmpleado();
                }
            }
            //Delvolvemos Resultado
            return result;
        }
        /// <summary>
        /// Método encargado de Inicializar los Valores de la Página
        /// </summary>
        private void inicializaValores()
        {
            //Evalua cada estatus de la página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //En caso de que el estado de la página este en modo Nuevo/Edición
                case Pagina.Estatus.Nuevo:
                    {
                        //Obteniendo Fecha Base
                        DateTime fecha_base = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro();

                        //Asignando Valores
                        lblNoConsecutivo.Text = "Por Asignar";
                        txtDiasPago.Text = "0";
                        txtFecIniPago.Text = fecha_base.AddDays(-7).ToString("dd/MM/yyyy");
                        txtFecFinPago.Text = 
                        txtFechaPago.Text = 
                        txtFecNomina.Text = fecha_base.ToString("dd/MM/yyyy");
                        txtSucursal.Text = "Ninguna ID:0";
                        ddlMetodoPago.SelectedValue = "8";

                        //Instanciando Compania
                        using(SAT_CL.Global.CompaniaEmisorReceptor cer = new SAT_CL.Global.CompaniaEmisorReceptor(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                        {
                            //Validando que exista la Compania
                            if (cer.habilitar)

                                //Asignando Compania Emisora
                                txtCompania.Text = cer.nombre + " ID:" + cer.id_compania_emisor_receptor.ToString();
                            else
                                //Limpiando Control
                                txtCompania.Text = "";
                        }

                        //Invocando Método de Calculo
                        calculaDiasPago();
                        break;
                    }
                case Pagina.Estatus.Lectura:
                case Pagina.Estatus.Edicion:
                    {
                        //Instanciando Nomina
                        using(SAT_CL.Nomina.Nomina nom = new SAT_CL.Nomina.Nomina(Convert.ToInt32(Session["id_registro"])))
                        {
                            //Validando que exista la Nomina
                            if(nom.habilitar)
                            {
                                //Asignando Valores
                                lblNoConsecutivo.Text = nom.no_consecutivo.ToString();
                                txtDiasPago.Text = nom.dias_pago.ToString();
                                txtFecIniPago.Text = nom.fecha_inicio_pago == DateTime.MinValue ? "" : nom.fecha_inicio_pago.ToString("dd/MM/yyyy");
                                txtFecFinPago.Text = nom.fecha_fin_pago == DateTime.MinValue ? "" : nom.fecha_fin_pago.ToString("dd/MM/yyyy");
                                txtFechaPago.Text = nom.fecha_pago == DateTime.MinValue ? "" : nom.fecha_pago.ToString("dd/MM/yyyy");
                                txtFecNomina.Text = nom.fecha_nomina == DateTime.MinValue ? "" : nom.fecha_nomina.ToString("dd/MM/yyyy");
                                string id_metodo_pago = nom.id_metodo_pago.ToString();
                                //Validamos Método de Pago No Identificado
                                if (nom.id_metodo_pago.ToString() =="7")
                                {
                                    //OTROS
                                    id_metodo_pago = "17";
                                }
                                //Validamos Método de Pago Transferencia E
                                if (nom.id_metodo_pago.ToString() == "3")
                                {
                                    //Transferencia E
                                    id_metodo_pago = "8";
                                }
                                //Validamos Método de Pago Tarjeta Debito
                                if (nom.id_metodo_pago.ToString() == "5")
                                {
                                    //Tarjeta Debito
                                    id_metodo_pago = "15";
                                }
                                //Validamos Método de Pago Cheque
                                if (nom.id_metodo_pago.ToString() == "1")
                                {
                                    //Cheque
                                    id_metodo_pago = "10";
                                }
                                ddlMetodoPago.SelectedValue = id_metodo_pago;
                               
                                ddlPeriodicidadPago.SelectedValue = nom.id_periodicidad_pago.ToString();

                                //Instanciando Compania
                                using (SAT_CL.Global.CompaniaEmisorReceptor cer = new SAT_CL.Global.CompaniaEmisorReceptor(nom.id_compania_emisora))
                                {
                                    //Validando que exista la Compania
                                    if (cer.habilitar)

                                        //Asignando Compania Emisora
                                        txtCompania.Text = cer.nombre + " ID:" + cer.id_compania_emisor_receptor.ToString();
                                    else
                                        //Limpiando Control
                                        txtCompania.Text = "";
                                }

                                //Instanciando Sucursal
                                using (SAT_CL.Global.Sucursal suc = new SAT_CL.Global.Sucursal(nom.id_sucursal))
                                {
                                    //Validando que exista la Sucursal
                                    if (suc.habilitar)

                                        //Asignando Sucursal
                                        txtSucursal.Text = suc.nombre + " ID:" + suc.id_sucursal.ToString();
                                    else
                                        //Limpiando Control
                                        txtSucursal.Text = "Ninguna ID:0";
                                }
                            }
                        }
                        break;
                    }
            }
        }
        /// <summary>
        /// Método encargado de Calcular los Dias Pagados
        /// </summary>
        private void calculaDiasPago()
        {
            //Declarando Variables Auxiliares
            DateTime fecha_inicio, fecha_fin;

            //Obteniendo Fechas
            DateTime.TryParse(txtFecIniPago.Text, out fecha_inicio);
            DateTime.TryParse(txtFecFinPago.Text, out fecha_fin);

            //Validando Fechas
            if (fecha_inicio != DateTime.MinValue && fecha_fin != DateTime.MinValue)
            {
                //Calculando Dias de Pago
                TimeSpan dias_pago = fecha_fin - fecha_inicio;

                //Asignando Dias
                txtDiasPago.Text = (dias_pago.Days + 1).ToString();
            }
            else
                //Asignando Dias
                txtDiasPago.Text = "0";
        }
        /// <summary>
        /// Método encargado de Guardar la Nomina
        /// </summary>
        private void guardaNomina()
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Declarando Variables para Fecha
            DateTime fec_nomina, fec_ini_pago, fec_fin_pago, fec_pago;

            //Obteniendo Fechas
            DateTime.TryParse(txtFecNomina.Text, out fec_nomina);
            DateTime.TryParse(txtFecIniPago.Text, out fec_ini_pago);
            DateTime.TryParse(txtFecFinPago.Text, out fec_fin_pago);
            DateTime.TryParse(txtFechaPago.Text, out fec_pago);

            //Evalua cada estatus de la página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //En caso de que el estado de la página este en modo Nuevo
                case Pagina.Estatus.Nuevo:
                    {
                        //Insertando Nuevo
                        result = SAT_CL.Nomina.Nomina.InsertaNomina(Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:", 1)), 0, 0,
                                fec_pago, fec_ini_pago, fec_fin_pago, fec_nomina, Convert.ToInt32(txtDiasPago.Text == "" ? "0" : txtDiasPago.Text),
                                Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtSucursal.Text, "ID:", 1)), Convert.ToByte(ddlPeriodicidadPago.SelectedValue),
                                Convert.ToByte(ddlMetodoPago.SelectedValue), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                        break;
                    }
                case Pagina.Estatus.Edicion:
                    {
                        //Instanciando Nomina
                        using (SAT_CL.Nomina.Nomina nom = new SAT_CL.Nomina.Nomina(Convert.ToInt32(Session["id_registro"])))
                        {
                            //Validando que exista la Nomina
                            if (nom.habilitar)

                                //Insertando Nuevo
                                result = nom.EditaNomina(Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:", 1)), nom.no_consecutivo, nom.id_nomina_origen,
                                        fec_pago, fec_ini_pago, fec_fin_pago, fec_nomina, Convert.ToInt32(txtDiasPago.Text == "" ? "0" : txtDiasPago.Text),
                                        Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtSucursal.Text, "ID:", 1)), Convert.ToByte(ddlPeriodicidadPago.SelectedValue),
                                        Convert.ToByte(ddlMetodoPago.SelectedValue), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                            else
                                //Instanciando Error
                                result = new RetornoOperacion("No existe la Nomina");
                        }
                        break;
                    }
            }

            //Validando Operación Exitosa
            if (result.OperacionExitosa)
            {
                //Asignando Registro
                Session["id_registro"] = result.IdRegistro;
                //Cambiando Estatus de Sesión
                Session["estatus"] = Pagina.Estatus.Edicion;

                //Invocando Método de Inicialización
                inicializaPagina();
            }

            //Mostrando Mensaje de Operación
            ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Método encargado de Cargar la Nomina de los Empleados
        /// </summary>
        private void cargaNominaEmpleados()
        {
            //Validando estatus de Página
            switch((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {
                        //Cargando GridView
                        Controles.InicializaGridview(gvNominaEmpleado);

                        //Añadiendo Resultado a Sesión
                        Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                        break;
                    }
                case Pagina.Estatus.Lectura:
                case Pagina.Estatus.Edicion:
                    {
                        //Instanciando Nomina de Empleados
                        using (DataTable dtNominaEmpleados = SAT_CL.Nomina.NominaEmpleado.ObtieneNominasEmpleado(Convert.ToInt32(Session["id_registro"])))
                        {
                            //Validando que existan Registros
                            if (Validacion.ValidaOrigenDatos(dtNominaEmpleados))
                            {
                                //Cargando GridView
                                Controles.CargaGridView(gvNominaEmpleado, dtNominaEmpleados, "Id", lblOrdenado.Text, true, 1);

                                //Añadiendo Resultado a Sesión
                                Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtNominaEmpleados, "Table");
                            }
                            else
                            {
                                //Cargando GridView
                                Controles.InicializaGridview(gvNominaEmpleado);

                                //Añadiendo Resultado a Sesión
                                Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                            }
                        }
                        break;
                    }
            }

            //Mostrando Totales
            sumaTotalesNominaEmpleado();
        }
        /// <summary>
        /// Método encargado de Sumar los Totales del GridView de los Conceptos
        /// </summary>
        private void sumaTotalesNominaEmpleado()
        {
            //Validando Origend de Datos
            if (Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table")))
            {
                //Mostrando Totales
                gvNominaEmpleado.FooterRow.Cells[5].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Aguinaldo)", "")));
                gvNominaEmpleado.FooterRow.Cells[6].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Sueldo)", "")));
                gvNominaEmpleado.FooterRow.Cells[7].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(OtrasPercepciones)", "")));
                gvNominaEmpleado.FooterRow.Cells[8].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(IMSS)", "")));
                gvNominaEmpleado.FooterRow.Cells[9].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(ISPT)", "")));
                gvNominaEmpleado.FooterRow.Cells[10].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Infonavit)", "")));
                gvNominaEmpleado.FooterRow.Cells[11].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(OtrasDeducciones)", "")));
                gvNominaEmpleado.FooterRow.Cells[12].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(HrsExtra)", "")));
                gvNominaEmpleado.FooterRow.Cells[13].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Incapacidad)", "")));
                gvNominaEmpleado.FooterRow.Cells[14].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(TPercepciones)", "")));
                gvNominaEmpleado.FooterRow.Cells[15].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(TDeducciones)", "")));
                gvNominaEmpleado.FooterRow.Cells[16].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(TPagado)", "")));
            }
            else
            {
                //Mostrando Totales
                gvNominaEmpleado.FooterRow.Cells[5].Text = 
                gvNominaEmpleado.FooterRow.Cells[6].Text = 
                gvNominaEmpleado.FooterRow.Cells[7].Text = 
                gvNominaEmpleado.FooterRow.Cells[8].Text = 
                gvNominaEmpleado.FooterRow.Cells[9].Text = 
                gvNominaEmpleado.FooterRow.Cells[10].Text = 
                gvNominaEmpleado.FooterRow.Cells[11].Text = 
                gvNominaEmpleado.FooterRow.Cells[12].Text = 
                gvNominaEmpleado.FooterRow.Cells[13].Text = 
                gvNominaEmpleado.FooterRow.Cells[14].Text = 
                gvNominaEmpleado.FooterRow.Cells[15].Text = 
                gvNominaEmpleado.FooterRow.Cells[16].Text = string.Format("{0:C2}", 0);
            }
        }
        /// <summary>
        ///  Método que permie la consulta del historial de modificicaciones realizadas a un registro 
        /// </summary>
        /// <param name="idRegistro">ID que permite identificar un registro de TipoPago</param>
        /// <param name="idTabla">Identificador de la tabla TipoPago</param>
        /// <param name="Titulo">Encabezado de la ventana Bitacora</param>
        private void inicializaBitacora(string idRegistro, string idTabla, string Titulo)
        {
            //Creación de la variable url, que almacena una ruta de ubicación de la ventana, que contendra la bitacora de un registros de  TipoPago.
            string url = Cadena.RutaRelativaAAbsoluta("~/Nomina/Nomina.aspx", "~/Accesorios/BitacoraRegistro.aspx?idT=" + idTabla + "&idR=" + idRegistro + "&tB=" + Titulo);
            
            //Variable que almacena la resolucion de la ventana bitacora
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            
            //Invoca el método de la clase ScriptServer que abrira una nueva ventana acorde a una ubicacion, un titulo de la ventana, una configuracion de dimensiones.
            ScriptServer.AbreNuevaVentana(url, "Bitacora Tipo Pago", configuracion, Page);
        }
        /// <summary>
        /// Método que permite la busqueda de registros en la base de datos pertenecientes a la tabla TipoPago
        /// </summary>
        /// <param name="idTabla">Id que identifica a la tabla TipoPago registrada en la base de datos</param>
        /// <param name="idCompania"> Id que identifica a una compañia</param>
        private void inicializaAperturaRegistro(int idTabla, int idCompania)
        {
            //Asigna a la variable de session id_tabla el valor del parametro idTabla
            Session["id_tabla"] = idTabla;
            
            //Asigna valores a la variable url con la ruta de ubicacion de la ventana Abrir registros con los datos obtenidos de la tabla TipoPago
            string url = Cadena.RutaRelativaAAbsoluta("~/Nomina/Nomina.aspx", "~/Accesorios/AbrirRegistro.aspx?P1=" + idCompania.ToString());
            
            //Define las dimensiones de la ventana Abrir registros de TipoPago
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            
            //Invoca al método de la variable ScripServer que abrira una nueva ventana con los registros pertenecientes a la tabla TipoPago
            ScriptServer.AbreNuevaVentana(url, "Abrir Registro Tipo Pago", configuracion, Page);
        }
        /// <summary>
        /// Método encargado de Gestionar las Ventanas Modales
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="command"></param>
        private void gestionaVentanas(Control sender, string command)
        {
            //Validando Comando
            switch(command)
            {
                case "DetalleNomina":
                    {
                        //Alternando Ventana
                        ScriptServer.AlternarVentana(sender, "AltaDetallesNomina", "contenedorVentanaDetallesNomina", "ventanaDetallesNomina");
                        break;
                    }
                case "NominaOtros":
                    {
                        //Alternando Ventana
                        ScriptServer.AlternarVentana(sender, "DetallesNominaOtros", "contenedorVentanaNominaOtros", "ventanaNominaOtros");
                        break;
                    }
                case "Timbrar":
                    {
                        //Alternando Ventana
                        ScriptServer.AlternarVentana(sender, "TimbrarNomina", "contenedorVentanaTimbradoNomina", "ventanaTimbradoNomina");
                        break;
                    }
                case "Cancelar":
                    {
                        //Alternando Ventana
                        ScriptServer.AlternarVentana(sender, "CancelarNomina", "contenedorVentanaCanceladoNomina", "ventanaCanceladoNomina");
                        break;
                    }
            }
        }
        /// <summary>
        /// Realiza la descarga del XML del comprobante
        /// </summary>
        /// <param name="id_comprobante">Comprobante de Nómina</param>
        private void descargarXML(int id_comprobante)
        {
            //Instanciando registro en sesión
            using (SAT_CL.FacturacionElectronica.Comprobante c = new SAT_CL.FacturacionElectronica.Comprobante(id_comprobante))
            {
                //Si existe y está generado
                if (c.generado)
                {
                    //Obteniendo bytes del archivo XML
                    byte[] cfdi_xml = System.IO.File.ReadAllBytes(c.ruta_xml);

                    //Realizando descarga de archivo
                    if (cfdi_xml.Length > 0)
                    {
                        //Instanciando al emisor
                        using (CompaniaEmisorReceptor em = new CompaniaEmisorReceptor(c.id_compania_emisor))
                            Archivo.DescargaArchivo(cfdi_xml, string.Format("{0}_{1}{2}.xml", em.nombre_corto != "" ? em.nombre_corto : em.rfc, c.serie, c.folio), Archivo.ContentType.binary_octetStream);
                    }
                }
            }
        }

        #region Métodos Modal "Detalle Nomina"
        /// <summary>
        /// Método encargado de Inicializar Valores de Timbrado
        /// </summary>
        private void inicializaValoresTimbradoNomina()
        {
            ddlBanco.SelectedValue = "0";
            ddlCuenta.SelectedValue = "0";
            txtSerie.Text = "";
        }
        /// <summary>
        /// Inicializando Valores de los Controles
        /// </summary>
        /// <param name="id_detalle_nomina">Detalle de Nomina</param>
        /// <param name="tipo_aplicacion">Tipo de Aplicación</param>
        /// <param name="id_concepto_sat">Concepto de Nomina del SAT</param>
        private void inicializaValoresDetalle(int id_detalle_nomina, SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion tipo_aplicacion, int id_concepto_sat)
        {
            //Validando que exista un Concepto de Nomina
            if (id_concepto_sat > 0)
            {
                //Cargando Tipos de Pago (Conceptos Principales)
                using(DataTable dtConceptos = SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(72, "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, id_concepto_sat.ToString(), (int)tipo_aplicacion, ""))
                {
                    //Validando que existan Conceptos
                    if (Validacion.ValidaOrigenDatos(dtConceptos))

                        //Cargando Controles
                        Controles.CargaDropDownList(ddlConcepto, dtConceptos, "id", "descripcion");
                    else
                        //Inicializando Control
                        Controles.InicializaDropDownList(ddlConcepto, "-- Ninguno");
                }
            }
            else
            {   
                //Cargando Tipos de Pago (Conceptos Principales)
                using (DataTable dtConceptos = SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(73, "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", (int)tipo_aplicacion, ""))
                {
                    //Validando que existan Conceptos
                    if (Validacion.ValidaOrigenDatos(dtConceptos))

                        //Cargando Controles
                        Controles.CargaDropDownList(ddlConcepto, dtConceptos, "id", "descripcion");
                    else
                        //Inicializando Control
                        Controles.InicializaDropDownList(ddlConcepto, "-- Ninguno");
                }
            }

            //Validando que exista el Detalle de Nomina
            if(id_detalle_nomina > 0)
            {   
                //Instanciando Detalle de Nomina
                using (SAT_CL.Nomina.DetalleNominaEmpleado dne = new SAT_CL.Nomina.DetalleNominaEmpleado(id_detalle_nomina))
                {
                    //Validando que existe el Detalle
                    if (dne.habilitar)
                    {
                        //Asignando Valores
                        ddlConcepto.SelectedValue = dne.id_tipo_pago.ToString();
                        txtImporteGravado.Text = dne.importe_gravado.ToString();
                        txtImporteExento.Text = dne.importe_exento.ToString();
                    }
                }
            }
            else
            {
                //Asignando Valores
                txtImporteGravado.Text =
                txtImporteExento.Text = "0.00";
            }
        }
        /// <summary>
        /// Método encargado de Cargar los Detalles de Nomina
        /// </summary>
        /// <param name="id_nomina_empleado">Nomina del Empleado</param>
        /// <param name="id_concepto_sat">Concepto de Nomina del SAT</param>
        /// <param name="tipo_aplicacion"></param>
        private void cargaDetallesNomina(int id_nomina_empleado, int id_concepto_sat, SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion tipo_aplicacion)
        {
            //Instanciando Nomina de Empleados
            using (DataTable dtDetallesNominaEmpleados = SAT_CL.Nomina.DetalleNominaEmpleado.ObtieneDetalleNominaEmpleado(id_nomina_empleado, id_concepto_sat, tipo_aplicacion))
            {
                //Validando que existan Registros
                if (Validacion.ValidaOrigenDatos(dtDetallesNominaEmpleados))
                {
                    //Cargando GridView
                    Controles.CargaGridView(gvDetalleNomina, dtDetallesNominaEmpleados, "Id", lblOrdenadoDet.Text, true, 1);

                    //Añadiendo Resultado a Sesión
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtDetallesNominaEmpleados, "Table1");
                }
                else
                {
                    //Cargando GridView
                    Controles.InicializaGridview(gvDetalleNomina);

                    //Añadiendo Resultado a Sesión
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
                }
            }
        }
        /// <summary>
        /// Método encargado de Guardar el Detalle de Nomina
        /// </summary>
        /// <param name="tipo_aplicacion">Tipo de Aplicación</param>
        private void guardaDetalleNomina()
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Validando Concepto
            if (ddlConcepto.SelectedValue != "0")
            {
                //Validando si existe un Registro Seleccionado
                if (gvDetalleNomina.SelectedIndex != -1)
                {
                    //Instanciando Detalle de Nomina
                    using (SAT_CL.Nomina.DetalleNominaEmpleado dne = new SAT_CL.Nomina.DetalleNominaEmpleado(Convert.ToInt32(gvDetalleNomina.SelectedDataKey["Id"])))
                    {
                        //Validando que exista el Registro
                        if (dne.habilitar)

                            //Editando Detalle de Nomina
                            result = dne.EditarDetalleNominaEmpleado(Convert.ToInt32(gvNominaEmpleado.SelectedDataKey["Id"]), Convert.ToInt32(ddlConcepto.SelectedValue),
                                            Convert.ToDecimal(txtImporteGravado.Text), Convert.ToDecimal(txtImporteExento.Text), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                        else
                            //Instanciando Excepción
                            result = new RetornoOperacion("No Existe el Detalle");
                    }
                }
                else
                    //Insertando Detalle de Nomina
                    result = SAT_CL.Nomina.DetalleNominaEmpleado.InsertarDetalleNominaEmpleado(Convert.ToInt32(gvNominaEmpleado.SelectedDataKey["Id"]),
                                    Convert.ToInt32(ddlConcepto.SelectedValue), Convert.ToDecimal(txtImporteGravado.Text), Convert.ToDecimal(txtImporteExento.Text),
                                    ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
            }
            else
                //Instanciando Excepción
                result = new RetornoOperacion("No Existe el Concepto");

            //Validando Operación Exitosa
            if (result.OperacionExitosa)
            {
                //Instanciando Detalle de Nomina
                using (SAT_CL.Nomina.DetalleNominaEmpleado dne = new SAT_CL.Nomina.DetalleNominaEmpleado(result.IdRegistro))

                //Instanciando Tipo Recurrente
                using (SAT_CL.Liquidacion.TipoCobroRecurrente tcr = new SAT_CL.Liquidacion.TipoCobroRecurrente(dne.id_tipo_pago))
                {
                    //Validando Percepción
                    if (((SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion)tcr.id_tipo_aplicacion == SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion.Percepcion ||
                        (SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion)tcr.id_tipo_aplicacion == SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion.Bonificacion) &&
                        (tcr.id_concepto_sat_nomina == 1 || tcr.id_concepto_sat_nomina == 2))
                    {
                        //Inicializando Valores del Detalle
                        inicializaValoresDetalle(0, (SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion)tcr.id_tipo_aplicacion, tcr.id_concepto_sat_nomina);

                        //Cargando Detalles de Nomina
                        cargaDetallesNomina(dne.id_nomina_empleado, tcr.id_concepto_sat_nomina, (SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion)tcr.id_tipo_aplicacion);
                    }
                    else if ((SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion)tcr.id_tipo_aplicacion == SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion.Deduccion &&
                                (tcr.id_concepto_sat_nomina == 1 || tcr.id_concepto_sat_nomina == 2 || tcr.id_concepto_sat_nomina == 9))
                    {
                        //Inicializando Valores del Detalle
                        inicializaValoresDetalle(0, (SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion)tcr.id_tipo_aplicacion, tcr.id_concepto_sat_nomina);

                        //Cargando Detalles de Nomina
                        cargaDetallesNomina(dne.id_nomina_empleado, tcr.id_concepto_sat_nomina, (SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion)tcr.id_tipo_aplicacion);
                    }
                    else
                    {
                        //Inicializando Valores del Detalle
                        inicializaValoresDetalle(0, (SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion)tcr.id_tipo_aplicacion, 0);

                        //Cargando Detalles de Nomina
                        cargaDetallesNomina(dne.id_nomina_empleado, 0, (SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion)tcr.id_tipo_aplicacion);
                    }

                    //Inicializando Indices
                    Controles.InicializaIndices(gvDetalleNomina);

                    //Obteniendo Nomina de Empleado
                    int nomina_empleado = Convert.ToInt32(gvNominaEmpleado.SelectedDataKey["Id"]);

                    //Cargando Nomina de Empleados
                    cargaNominaEmpleados();

                    // Marcando Fila
                    Controles.MarcaFila(gvNominaEmpleado, nomina_empleado.ToString(), "Id", "Id", OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), lblOrdenado.Text, Convert.ToInt32(ddlTamano.SelectedValue), true, 1);
                    upgvNominaEmpleado.Update();

                    //Mostrando Totales
                    sumaTotalesNominaEmpleado();
                }
            }
            

            //Mostrando Mensaje de la Operación
            ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }


        #endregion

        #region Métodos Modal "Nomina Otros"

        /// <summary>
        /// Inicializando Valores de los Controles
        /// </summary>
        /// <param name="id_nomina_otros">Nomina Otros</param>
        private void inicializaValoresNominaOtros(int id_nomina_otros, byte id_tipo_nomina_otros)
        {
            //Validando que exista el Detalle de Nomina
            if (id_nomina_otros > 0)
            {
                //Instanciando Detalle de Nomina
                using (SAT_CL.Nomina.NominaOtros no = new SAT_CL.Nomina.NominaOtros(id_nomina_otros))
                {
                    //Validando que existe el Detalle
                    if (no.habilitar)
                    {
                        //Cargando Catalogo
                        SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipo, "", 3149);
                        //Asignando Tipo
                        ddlTipo.SelectedValue = no.id_tipo.ToString();
                        id_tipo_nomina_otros = no.id_tipo;
                        //Cargando Catalogo
                        SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlSubTipo, "", 3150, Convert.ToInt32(ddlTipo.SelectedValue));
                        //Asignando Valores
                        ddlSubTipo.SelectedValue = no.id_subtipo.ToString();
                        txtDias.Text = no.dias.ToString();
                        txtCantidad.Text = no.cantidad.ToString();
                        txtImporteGravadoNO.Text = no.importe_gravado.ToString();
                        txtImporteExentoNO.Text = no.importe_exento.ToString();
                    }
                }
            }
            else
            {
                //Cargando Catalogo
                SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipo, "", 3149);
                //Asignando Tipo
                ddlTipo.SelectedValue = id_tipo_nomina_otros.ToString();
                //Cargando Catalogo
                SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlSubTipo, "", 3150, Convert.ToInt32(ddlTipo.SelectedValue));
                //Asignando Valores
                txtDias.Text = "0";
                txtImporteGravadoNO.Text = 
                txtImporteExentoNO.Text = 
                txtCantidad.Text = "0.00";
            }

            //Validando Tipo de Nomina Otros
            if (id_tipo_nomina_otros == 2)

                //Viasualizando Control
                txtCantidad.Enabled = true;

            else if (id_tipo_nomina_otros == 1)
                //Viasualizando Control
                txtCantidad.Enabled = false;
        }
        /// <summary>
        /// Método encargado de Cargar la  Nomina de Otros del Empleado
        /// </summary>
        /// <param name="id_nomina_empleado">Nomina de Empleado</param>
        /// <param name="id_tipo_nomina_otros">Tipo de Nomina</param>
        private void cargaNominaOtros(int id_nomina_empleado, int id_tipo_nomina_otros)
        {
            //Instanciando Nomina de Empleados
            using (DataTable dtNominaOtrosEmpleados = SAT_CL.Nomina.NominaOtros.ObtieneNominaOtros(id_nomina_empleado, id_tipo_nomina_otros))
            {
                //Validando que existan Registros
                if (Validacion.ValidaOrigenDatos(dtNominaOtrosEmpleados))
                {
                    //Cargando GridView
                    Controles.CargaGridView(gvNominaOtros, dtNominaOtrosEmpleados, "Id", lblOrdenadoDet.Text, true, 1);

                    //Añadiendo Resultado a Sesión
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtNominaOtrosEmpleados, "Table2");
                }
                else
                {
                    //Cargando GridView
                    Controles.InicializaGridview(gvNominaOtros);

                    //Añadiendo Resultado a Sesión
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table2");
                }
            }
        }
        /// <summary>
        /// Método encargado de Guardar el Detalle de Nomina
        /// </summary>
        private void guardaNominaOtros()
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Validando si existe un Registro Seleccionado
            if (gvNominaOtros.SelectedIndex != -1)
            {
                //Instanciando Detalle de Nomina
                using (SAT_CL.Nomina.NominaOtros no = new SAT_CL.Nomina.NominaOtros(Convert.ToInt32(gvNominaOtros.SelectedDataKey["Id"])))
                {
                    //Validando que exista el Registro
                    if (no.habilitar)

                        //Editando Detalle de Nomina
                        result = no.EditarNominaOtros(Convert.ToInt32(gvNominaEmpleado.SelectedDataKey["Id"]),(SAT_CL.Nomina.NominaOtros.TipoNominaOtros) Convert.ToByte(ddlTipo.SelectedValue), Convert.ToInt32(txtDias.Text),
                                       (SAT_CL.Nomina.NominaOtros.SubTipo) Convert.ToByte(ddlSubTipo.SelectedValue), Convert.ToDecimal(txtImporteGravadoNO.Text), Convert.ToDecimal(txtImporteExentoNO.Text), 
                                        Convert.ToDecimal(txtCantidad.Text), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                    else
                        //Instanciando Excepción
                        result = new RetornoOperacion("No Existe la Nomina Otros");
                }
            }
            else
                //Insertando Detalle de Nomina
                result = SAT_CL.Nomina.NominaOtros.InsertarNominaOtros(Convert.ToInt32(gvNominaEmpleado.SelectedDataKey["Id"]), (SAT_CL.Nomina.NominaOtros.TipoNominaOtros)Convert.ToByte(ddlTipo.SelectedValue), Convert.ToInt32(txtDias.Text),
                                        (SAT_CL.Nomina.NominaOtros.SubTipo)Convert.ToByte(ddlSubTipo.SelectedValue), Convert.ToDecimal(txtImporteGravadoNO.Text), Convert.ToDecimal(txtImporteExentoNO.Text), 
                                        Convert.ToDecimal(txtCantidad.Text), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);


            //Validando Operación Exitosa
            if (result.OperacionExitosa)
            {
                //Instanciando Detalle de Nomina
                using (SAT_CL.Nomina.NominaOtros no = new SAT_CL.Nomina.NominaOtros(result.IdRegistro))
                {
                    //Inicializando Valores del Detalle
                    inicializaValoresNominaOtros(0, no.id_tipo);

                    //Cargando Nomina de Otros
                    cargaNominaOtros(no.id_nomina_empleado, no.id_tipo);

                    //Inicializando Indices
                    Controles.InicializaIndices(gvNominaOtros);

                    //Obteniendo Nomina de Empleado
                    int nomina_empleado = Convert.ToInt32(gvNominaEmpleado.SelectedDataKey["Id"]);

                    //Cargando Nomina de Empleados
                    cargaNominaEmpleados();

                    // Marcando Fila
                    Controles.MarcaFila(gvNominaEmpleado, nomina_empleado.ToString(), "Id", "Id", OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), lblOrdenado.Text, Convert.ToInt32(ddlTamano.SelectedValue), true, 1);
                    upgvNominaEmpleado.Update();

                    //Mostrando Totales
                    sumaTotalesNominaEmpleado();
                }
            }

            //Mostrando Mensaje de la Operación
            ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }


        #endregion
        #endregion
    }
}