using System;
using System.Data;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.Nomina
{
    public partial class ReporteNomina : System.Web.UI.Page
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
        /// Evento Producido 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            //Invocando Método de Busqueda
            buscaNominaEncabezado();
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
            switch (lnk.CommandName)
            {
                case "NominaEncabezado":
                    {
                        //Exportando Contenido
                        Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"));
                        break;
                    }
            }
        }
        /// <summary>
        /// Evento que permite imprimir el fromato de comprobante de nomina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkImprimirNomina_Click(object sender, EventArgs e)
        {
            //Valida que existan registros en el grid view
            if (gvNominaEncabezado.DataKeys.Count != 0)
            {
                //Selecciona la fila del gridview
                Controles.SeleccionaFila(gvNominaEncabezado, sender, "lnk", false);
                //Obteniendo Ruta            
                string urlReporte = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Nomina/ReporteNomina.aspx", "~/RDLC/Reporte.aspx");
                //Instanciando nueva ventana de navegador para apertura de registro
                TSDK.ASP.ScriptServer.AbreNuevaVentana(string.Format("{0}?idTipoReporte={1}&idRegistro={2}", urlReporte, "ComprobanteNomina", Convert.ToInt32(gvNominaEncabezado.SelectedDataKey["Id"])), "ComprobanteNomina", "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500", Page);

            }

        }
        #region Eventos GridView "Nomina Encabezado"

        /// <summary>
        /// Evento Disparado al Cambiar el Tamaño del GridView "Nomina Empleados"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamano_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambio de Tamaño
            Controles.CambiaTamañoPaginaGridView(gvNominaEncabezado, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamano.SelectedValue), true, 1);
        }
        /// <summary>
        /// Evento Disparado al Cambiar la Expresión de Ordenamiento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvNominaEncabezado_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión
            lblOrdenado.Text = Controles.CambiaSortExpressionGridView(gvNominaEncabezado, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression, true, 1);
        }
        /// <summary>
        /// Evento Disparado al Cambiar el Indice de Página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvNominaEncabezado_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Página
            Controles.CambiaIndicePaginaGridView(gvNominaEncabezado, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, true, 1);
        }
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
        /// Evento generado al Timbrar la Nómina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarTimbradoNomina_Click(object sender, EventArgs e)
        {
            
            //Declaramos Objeto Resultado
            RetornoOperacion res = new RetornoOperacion();
           
            //Timbramos Nómina
            res = TimbraNomina();

            //Ocultando Ventana
            gestionaVentanas(this, "Timbrar");

            //Mostrando Resultado Obtenido
            ScriptServer.MuestraNotificacion(this, res, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        /// <summary>
        /// Evento Producido al Dar Click en el Link
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkActualizaNomina_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if(gvNominaEncabezado.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvNominaEncabezado, sender, "lnk", false);

                //Obteniendo Control
                LinkButton lnk = (LinkButton)sender;

                //Validando Comando
                switch(lnk.CommandName)
                {
                    case "VerNomina":
                        {
                            //Creación de la variable url, que almacena una ruta de ubicación de la ventana, que contendra la bitacora de un registros de  TipoPago.
                            string url = Cadena.RutaRelativaAAbsoluta("~/Nomina/ReporteNomina.aspx", "~/Nomina/Nomina.aspx?idRegistro=" + gvNominaEncabezado.SelectedDataKey["Id"].ToString());

                            //Redireccionando a la Forma de Nomina
                            Response.Redirect(url);
                            break;
                        }
                    case "Timbrar":
                        {
                            //Inicializamos Valores de Timbrado
                            inicializaValoresTimbradoNomina();
                            //Mostramos Ventana Modal Timbrar
                            gestionaVentanas(this, "Timbrar");
                            break;
                        }
                    case "CopiarNomina":
                        {
                            //Declarando Objeto de Retorno
                            RetornoOperacion result = new RetornoOperacion();
                            
                            //Instanciando Nomina
                            using (SAT_CL.Nomina.Nomina nomina = new SAT_CL.Nomina.Nomina(Convert.ToInt32(gvNominaEncabezado.SelectedDataKey["Id"])))
                            {
                                //Validando que existe el Registro
                                if (nomina.habilitar)
                                
                                    //Copiando Nomina
                                    result = nomina.CopiaNomina(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                else
                                    //Instanciando Excepción
                                    result = new RetornoOperacion("No se puede Acceder a la Nómina");

                                //Validando Operación Exitosa
                                if(result.OperacionExitosa)
                                {
                                    //Instanciamos Nomina
                                    using (SAT_CL.Nomina.Nomina objNominaNueva = new SAT_CL.Nomina.Nomina(result.IdRegistro))
                                    {
                                        //Personalizando Mensaje Correcto
                                        result = new RetornoOperacion(result.IdRegistro, string.Format("La  Nómina No. '{0}' se Creó Exitosamente", objNominaNueva.no_consecutivo.ToString()), true);
                                    }
                                    //Buscando Nomina Actualizada
                                    buscaNominaEncabezado();

                                    //Inicializando Indices
                                    Controles.InicializaIndices(gvNominaEncabezado);
                                }
                            }

                            //Mostrando Mensaje de Operación
                            ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
                            break;
                        }
                }
            }
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
            switch (lnk.CommandName)
            {

                case "TimbradoNomina":
                    {
                        //Invocando Método de Gestión de Ventanas
                        gestionaVentanas(this, "Timbrar");
                        break;
                    }
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
            //Invocando Método de Carga
            cargaCatalogos();

            //Inicializando Control
            Controles.InicializaGridview(gvNominaEncabezado);

            //Instanciando Compania
            using (SAT_CL.Global.CompaniaEmisorReceptor cer = new SAT_CL.Global.CompaniaEmisorReceptor(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
            {
                //Validando que exista la Compania
                if (cer.habilitar)

                    //Asignando Compania Emisora
                    txtCompania.Text = cer.nombre + " ID:" + cer.id_compania_emisor_receptor.ToString();
                else
                    //Limpiando Control
                    txtCompania.Text = "";
            }

            //Configurando Fechas
            txtFecFinPago.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");
            txtFecIniPago.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().AddDays(-7).ToString("dd/MM/yyyy HH:mm");
        }
        /// <summary>
        /// Método encargado de Cargar los Catalogos
        /// </summary>
        private void cargaCatalogos()
        {
            //Cargando Catalogos de Tamaño
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamano, "", 26);
            //Catalogo Bancos
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlBanco, 22, "Ninguno");
            //Catalogo de Cuentas
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlCuenta, 23, "Ninguno", Convert.ToInt32(ddlBanco.SelectedValue), "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "");
        
        }
        /// <summary>
        /// Método encargado de Buscar la Nomina
        /// </summary>
        private void buscaNominaEncabezado()
        {
            //Declarando Variables de Fecha
            DateTime fec_ini_pago, fec_fin_pago, fec_ini_nomina, fec_fin_nomina;
            fec_ini_pago = fec_fin_pago = fec_ini_nomina = fec_fin_nomina = DateTime.MinValue;

            //Validando Fechas Solicitadas
            if (chkFechaPago.Checked)
            {
                //Obteniendo Fecha
                DateTime.TryParse(txtFecIniPago.Text, out fec_ini_pago);
                DateTime.TryParse(txtFecFinPago.Text, out fec_fin_pago);
            }
            if (chkFechaNomina.Checked)
            {
                //Obteniendo Fecha
                DateTime.TryParse(txtFecIniPago.Text, out fec_ini_nomina);
                DateTime.TryParse(txtFecFinPago.Text, out fec_fin_nomina);
            }

            //Obteniendo Reporte
            using(DataTable dtNominaEncabezado = SAT_CL.Nomina.Reporte.ObtieneNominaEncabezado(Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:", 1, "0")),
                                                    Convert.ToInt32(txtNoNomina.Text == "" ? "0" : txtNoNomina.Text), fec_ini_pago, fec_fin_pago, fec_ini_nomina, fec_fin_nomina))
            {
                //Validando que exista el Reporte
                if(TSDK.Datos.Validacion.ValidaOrigenDatos(dtNominaEncabezado))
                {
                    //Cargando GridView
                    Controles.CargaGridView(gvNominaEncabezado, dtNominaEncabezado, "Id", lblOrdenado.Text, true, 1);

                    //Añadiendo Resultado a Sesión
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtNominaEncabezado, "Table");
                }
                else
                {
                    //Inicializando GridView
                    Controles.InicializaGridview(gvNominaEncabezado);

                    //Eliminando Resultado de Sesión
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                }
            }
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
            using (SAT_CL.Nomina.Nomina nom = new SAT_CL.Nomina.Nomina(Convert.ToInt32(gvNominaEncabezado.SelectedDataKey["Id"])))
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

                    //Mostrando Mensaje de Operación
                    ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
                }
            }
            //Delvolvemos Resultado
            return result;
        }

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
        /// Método encargado de Gestionar las Ventanas Modales
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="command"></param>
        private void gestionaVentanas(Control sender, string command)
        {
            //Validando Comando
            switch (command)
            {
                case "Timbrar":
                    {
                        //Alternando Ventana
                        ScriptServer.AlternarVentana(sender, "TimbrarNomina", "contenedorVentanaTimbradoNomina", "ventanaTimbradoNomina");
                        break;
                    }
            }
        }
        #endregion


    }
}