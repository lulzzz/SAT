using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;
namespace SAT.General
{
    public partial class Sucursal : System.Web.UI.Page
    {
        #region Eventos

        /// <summary>
        /// Evento producido al efectuarse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Validando que se haya producido un PostBack
            if (!(Page.IsPostBack))

                //Inicializando Forma
                inicializaForma();
        }
        /// <summary>
        /// Evento generado al cambiar la Vista
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnVista_OnClick(object sender, EventArgs e)
        {
            //Determinando el tipo de acción a realizar
            switch (((Button)sender).CommandName)
            {
                case "Sucursales":
                    mtvSucursales.ActiveViewIndex = 0;
                    btnSucursales.CssClass = "boton_pestana_activo";
                    btnSucursal.CssClass = "boton_pestana";
                    //Carga Sucursales
                    cargaSucursales();
                    lblErrorSucursales.Text = "";
                    break;
                case "Sucursal":
                    mtvSucursales.ActiveViewIndex = 1;
                    btnSucursales.CssClass = "boton_pestana";
                    btnSucursal.CssClass = "boton_pestana_activo";
                    //Inicializamos Grid
                    TSDK.ASP.Controles.InicializaIndices(gvSucursales);
                    //Inicializa Valores
                    inicializaValores();
                    break;
            }
        }
        /// <summary>
        /// Evento generado al editar una Sucursal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbEditar_Click(object sender, EventArgs e)
        {
            //Validando si existen Registros
            if (gvSucursales.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                TSDK.ASP.Controles.SeleccionaFila(gvSucursales, sender, "lnk", false);
                //Modificamos Vista
                mtvSucursales.ActiveViewIndex = 1;
                btnSucursales.CssClass = "boton_pestana";
                btnSucursal.CssClass = "boton_pestana_activo";
                //Inicializamos Valores
                inicializaValores();
            }
        }

        /// <summary>
        /// Evento generado al eliminar una Sucursal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbEliminar_Click(object sender, EventArgs e)
        {
            //Validando si existen Registros
            if (gvSucursales.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                TSDK.ASP.Controles.SeleccionaFila(gvSucursales, sender, "lnk", false);

                //Deshabilitamos Sucursal
                deshabilitaSucursal();
            }
        }

        /// <summary>
        /// Evenyo generado al Guardar la Sucursal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            //Invocando Método de Guardar
            guardaSucursal();
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
                    //Cambiando Session a Lectura
                    Session["estatus"] = Pagina.Estatus.Lectura;
                    break;
            }

            //Inicializando Forma
            inicializaForma();
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Dirección"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkVentana_Click(object sender, EventArgs e)
        {
            //Habilitando el Control
            ucDireccion.Enable = true;

            //Validando que exista un registro previo
            if (Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtDireccion.Text, "ID:", 1)) != 0)

                //Inicializando Control con el Registro Previo
                ucDireccion.InicializaControl(Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtDireccion.Text, "ID:", 1)));
            else
                //Inicializando Control por Defecto
                ucDireccion.InicializaControl();
        }

        /// <summary>
        /// Evento producido al cambiar el tamaño de gridview de detalles
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamañoGridViewSucursales_SelectedIndexChanged(object sender, EventArgs e)
        {
            Controles.CambiaTamañoPaginaGridView(gvSucursales, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamañoGridViewSucursales.SelectedValue), true, 1);
        }
        /// <summary>
        /// Evento producido al pulsar el botón de exportación a excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExcel_Click(object sender, EventArgs e)
        {
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"));
        }
        /// <summary>
        /// Evento prodicido al cambiar el indice de página del Gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvSucursales_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Controles.CambiaIndicePaginaGridView(gvSucursales, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, true, 1);
        }
        /// <summary>
        /// Evento producido al cambiar el orden del Gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvSucursales_Sorting(object sender, GridViewSortEventArgs e)
        {
            Controles.CambiaSortExpressionGridView(gvSucursales, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression, true, 1);
        }
        #region Eventos UserControl "Dirección"

        /// <summary>
        /// Evento Producido al Guardar la Dirección
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucDireccion_ClickGuardarDireccion(object sender, EventArgs e)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Obteniendo resultado de la Operación
            result = ucDireccion.GuardaDireccion();
            //Instanciando Direccion
            using (SAT_CL.Global.Direccion dir = new SAT_CL.Global.Direccion(result.IdRegistro))
            {   //Validando que exista
                if (dir.id_direccion != 0)
                {   //Mostrando Descripcion
                    txtDireccion.Text = dir.calle + " ID:" + dir.id_direccion.ToString();
                    //Deshabilitando el Control
                    ucDireccion.Enable = false;
                }
                else//Limpiando Control
                    txtDireccion.Text = "";
            }
        }
        /// <summary>
        /// Evento Producido al Eliminar la Dirección
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucDireccion_ClickEliminarDireccion(object sender, EventArgs e)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Eliminando Dirección
            result = ucDireccion.EliminaDireccion();
            //Validando si la Operación fue Exitosa
            if (result.OperacionExitosa)
            {   //Validando si el registro eliminado es igual al Seleccionado
                if (Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtDireccion.Text, "ID:", 1)) == result.IdRegistro)
                    //Limpiando Control
                    txtDireccion.Text = "";
            }
        }
        /// <summary>
        /// Evento Producido al Seleccionar la Dirección
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucDireccion_ClickSeleccionarDireccion(object sender, EventArgs e)
        {   //Instanciando Dirección
            using (SAT_CL.Global.Direccion dir = new SAT_CL.Global.Direccion(ucDireccion.SeleccionaDireccion()))
            {   //Validando que exista una Dirección
                if (dir.id_direccion != 0)
                {   //Mostrando Descripción
                    txtDireccion.Text = dir.calle + " ID:" + dir.id_direccion.ToString();
                    //Deshabilitando el Control
                    ucDireccion.Enable = false;
                }
                else//Limpiando Control
                    txtDireccion.Text = "";
            }
        }

        #endregion


        #endregion

        #region Métodos

        /// <summary>
        /// Método Privado encargado de Inicializar la Forma
        /// </summary>
        private void inicializaForma()
        {
            //Carga catalogos
            cargaCatalogos();
            //Habilita Controles
            habilitarControles();
            //Inicializando Control
            ucDireccion.InicializaControl();
            //Mostrando Enfoque al Primer Control
            txtNombre.Focus();
            //Carga Sucursales
            cargaSucursales();
        }

        /// <summary>
        /// Método Privado encargado de Cargar los Catalogos de la Forma
        /// </summary>
        private void cargaCatalogos()
        {

            //Cargando Catalogos 
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamañoGridViewSucursales, "", 18);

        }
        /// <summary>
        /// Método Prvado encargado de Habilitar los Controles
        /// </summary>
        private void habilitarControles()
        {
            //Habilita Controles
            txtNombre.Enabled = true;

        }
        /// <summary>
        /// Método Privado encargado de Inicializar los Valores
        /// </summary>
        private void inicializaValores()
        {
            //Validando Estatus Selección de Sucursal
            if (gvSucursales.SelectedIndex == -1)
            {
                //Asignando Valores
                txtNombre.Text =
                txtDireccion.Text = "";
                lblError.Text = "";
                lblErrorSucursales.Text = "";
            }
            else
            {
                //Instanciando la Compania
                using (SAT_CL.Global.Sucursal su = new SAT_CL.Global.Sucursal(Convert.ToInt32(gvSucursales.SelectedValue)))
                {   //Asignando Valores
                    txtNombre.Text = su.nombre;
                    //Instanciando Direccion
                    using (SAT_CL.Global.Direccion dir = new SAT_CL.Global.Direccion(su.id_direccion))
                    {   //Validando que exista la Direccion
                        if (dir.id_direccion != 0)
                            //Asignando Valor
                            txtDireccion.Text = dir.calle + " ID:" + dir.id_direccion.ToString();
                        else//Limpiando Control
                            txtDireccion.Text = "";
                    }
                }
                lblError.Text = "";
            }
        }
        /// <summary>
        /// Método Privado encargado de Guardar los Cambios de la Sucursal
        /// </summary>
        private void guardaSucursal()
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Validando Estatus de Session
            if (gvSucursales.SelectedIndex == -1)
            {

                //Realizando Inserción
                result = SAT_CL.Global.Sucursal.InsertarSucursal(Convert.ToInt32(Request.QueryString["id_compania"]), txtNombre.Text, Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtDireccion.Text, "ID:", 1)),
                          ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

            }
            else
            {
                using (SAT_CL.Global.Sucursal su = new SAT_CL.Global.Sucursal(Convert.ToInt32(gvSucursales.SelectedValue)))
                {   //Validando que sea un Registro Valido
                    if (su.id_sucursal != 0)
                        //Realizando Edición
                        result = su.EditarSucursal(Convert.ToInt32(Request.QueryString["id_compania"]), txtNombre.Text, Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtDireccion.Text, "ID:", 1)),
                                          ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                }
            }
            //Inicializamos Valores
            inicializaValores();
            //Mostrando Mensaje de Error
            lblError.Text = result.Mensaje;
        }

        /// <summary>
        /// Método Privado encargado de Guardar los Cambios de la Sucursal
        /// </summary>
        private void deshabilitaSucursal()
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Validando Estatus de Session
            if (Convert.ToInt32(gvSucursales.SelectedValue) > 0)
            {
                using (SAT_CL.Global.Sucursal su = new SAT_CL.Global.Sucursal(Convert.ToInt32(gvSucursales.SelectedValue)))
                {   //Validando que sea un Registro Valido
                    if (su.id_sucursal != 0)
                        //Realizando Edición
                        result = su.DeshabilitarSucursal(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                }
            }
            //Cargamos Scursales
            cargaSucursales();
            lblErrorSucursales.Text = result.Mensaje;
        }
        /// <summary>
        /// Método Privado encargado de Inicializar la Apertura de un Registro
        /// </summary>
        /// <param name="idTabla">Id de Tabla</param>
        private void inicializaAperturaRegistro(int idTabla, int idCompania)
        {   //Asignando Session
            Session["id_tabla"] = idTabla;
            //Construyendo URL 
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/General/Producto.aspx", "~/Accesorios/AbrirRegistro.aspx");
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=650,height=600";
            //Abriendo Nueva Ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "", configuracion, Page);
        }
        /// <summary>
        /// Método Privado encargado de Inicializar la Forma de Bitacora
        /// </summary>
        /// <param name="idRegistro">Id de Registro</param>
        /// <param name="idTabla">Id de Tabla</param>
        /// <param name="Titulo">Titulo</param>
        private void inicializaBitacora(string idRegistro, string idTabla, string Titulo)
        {   //Construyendo URL 
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/General/CompaniaEmisorReceptor.aspx", "~/Accesorios/BitacoraRegistro.aspx?idT=" + idTabla + "&idR=" + idRegistro + "&tB=" + Titulo);
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=650,height=700";
            //Abriendo Nueva Ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "", configuracion, Page);
        }
        /// <summary>
        /// Método encargado de Referenciar el Registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="id_compania">Id de Compania</param>
        private void inicializaReferenciaRegistro(string id_registro, string id_tabla, string id_compania)
        {   //Declarando variable para armado de URL
            string urlDestino = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/General/CompaniaEmisorReceptor.aspx", "~/Accesorios/ReferenciaRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idC=" + id_compania);
            //Instanciando nueva ventana de navegador para apertura de referencias de registro
            TSDK.ASP.ScriptServer.AbreNuevaVentana(urlDestino, "Referencia", 800, 650, false, false, false, true, true, Page);
        }

        /// <summary>
        /// Carga Paradas
        /// </summary>
        private void cargaSucursales()
        {
            //Obtenemos Paradas ligados al Id de Servicio
            using (DataTable mit = SAT_CL.Global.Sucursal.CargaSucursales(Convert.ToInt32(Request.QueryString["id_compania"])))
            {
                //Cargamos Grid View
                Controles.CargaGridView(gvSucursales, mit, "Id", "", true, 1);

                //Valida Origen de Datos
                if (mit != null)
                {
                    //Añadiendo Tabla a DataSet de Session
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], mit, "Table");
                }
                else
                {
                    //Eliminamos Tabla de la Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                }
            }
        }

        #endregion
    }

}