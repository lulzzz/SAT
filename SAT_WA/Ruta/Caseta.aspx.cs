using Microsoft.SqlServer.Types;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.Ruta
{
    public partial class Caseta : System.Web.UI.Page
    {
        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            //Aplicando seguridad de la forma
            SAT_CL.Seguridad.Forma.AplicaSeguridadForma(this, "content1", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
            //Validando  PostBack
            if (!Page.IsPostBack)            
                //Inicializando  la forma
                inicializaForma();
        }
        /// <summary>
        /// Evento que ejecuta los estados  del menú
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbElementoMenu_Click(object sender, EventArgs e)
        {
            //Creación del objeto botonMenu que obtiene las opciones de los menú desplegable .
            LinkButton botonMenu = (LinkButton)sender;
            //Permite validar cada una de las opciones del ménu.
            switch (botonMenu.CommandName)
            {
                //en caso de seleccionar la opción de Nuevo en el menú.
                case "Nuevo":
                    {
                        //A la variable estatus de la sesion, se le asigna el valor de pagina nuevo
                        Session["estatus"] = Pagina.Estatus.Nuevo;
                        // La variable id_registro se le inicializa en cero.
                        Session["id_registro"] = 0;
                        //Se realiza un enfoque al primer control 
                        txtDescripcion.Focus();
                        //Inicializa el estado de la forma.
                        inicializaForma();
                        break;
                    }

                case "Abrir":
                    {
                        //Invoca al método inicializaApertura para inicializar la apertura de un registro de Caseta
                        inicializaAperturaRegistro(170, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);
                        break;
                    }

                case "Guardar":
                    {
                        guardarCaseta();
                        break;
                    }
                //En caso de seleccionar la opción Editar del menú.
                case "Editar":
                    {
                        //Se asigna a la sesion el estado de edición
                        Session["estatus"] = Pagina.Estatus.Edicion;
                        //Se realiza un enfoque al primer control 
                        txtDescripcion.Focus();
                        //Invoca al metodo inicializaFormana
                        inicializaForma();
                        break;
                    }
                //En caso de seleccionar la opción Eliminar del menú.
                case "Eliminar":
                    {
                        //Creación del objeto retorno
                        RetornoOperacion retorno = new RetornoOperacion();
                        //Invoca a la clase Caseta
                        using (SAT_CL.Ruta.Caseta cs = new SAT_CL.Ruta.Caseta((int)Session["id_registro"]))
                        {
                            //Valida que exista el registro
                            if (cs.id_caseta > 0)
                                //Asigna valores al objeto retorno
                                retorno = cs.DeshabilitarCaseta(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                            //Instancia a la clase requisición
                        }                       
                    //Valida si la inserción a la base de datos se realizo correctamente
                    if (retorno.OperacionExitosa)
                    {
                        //A la variable de sessión estatus le asigna el estado de la pagina en modo Nuevo
                        Session["estatus"] = Pagina.Estatus.Nuevo;
                        //A la variable de session id_registro le asigna el valor insertado en la tabla Caseta
                        Session["id_registro"] = 0;
                        //Invoca al método inicializa forma
                        inicializaForma();
                    }
                    //Mensaje de que la operación de deshabilitar registros se realizo correctamente.
                    TSDK.ASP.ScriptServer.MuestraNotificacion(this, retorno, ScriptServer.PosicionNotificacion.AbajoDerecha);
                    break;
                    }
                case "Bitacora":
                    {
                        //Invoca al método inicializaBitacora que muestra las modificaciones sobre un registro de Caseta
                        inicializaBitacora(Session["id_registro"].ToString(), "170", "Caseta");
                        break;
                    }

                case "Referencias":
                    {
                        //Invoca al método inicializaReferencia que muestra las observaciones hechas a un registro de Caseta
                        inicializaReferenciaRegistro(Session["id_registro"].ToString(), "170",
                                                    ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString());
                        break;
                    }

            }
        }
        /// <summary>
        /// Evento que se ejecuta cuando se da clic al botón guardar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            //Invoca al método que 
            guardarCaseta();
            
        }
        /// <summary>
        /// Evento que se ejecutara cuando se de clic en el boton cancela
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            //Válida cada estado de la pagina y ejecuta
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //En caso de que el estado de la pagina sea nuevo
                case Pagina.Estatus.Nuevo:
                    {
                        //A la variable id_registro se le asigna el valor de 0
                        Session["id_registro"] = 0;
                        break;
                    }
                //En caso de que el estado de la página sea edición
                case Pagina.Estatus.Edicion:
                    {
                        //A la variable estatus se le asigna el estado de lectura
                        Session["estatus"] = Pagina.Estatus.Lectura;
                        break;
                    }
            }
            //Invoca al método que inicializa los valores del formulario.
            inicializaForma();

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarCosto_Click(object sender, EventArgs e)
        {
            //Invoca al método que inicializa los valores de la forma
            inicializaCostoCaseta();
            txtNoEjes.Text = "";
            txtCosto.Text = "";
            txtFecha.Text = Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAgregarCosto_Click(object sender, EventArgs e)
        {
            guardarCostoCaseta();
        }
        /// <summary>
        /// Evento que Elimina el Puesto a una actividad asignada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEliminar_Click(object sender, EventArgs e)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Valida que esictan registros en el gridView
            if (gvCostoCaseta.DataKeys.Count > 0)
            {
                //Selecciona el registro que se va a editar
                Controles.SeleccionaFila(gvCostoCaseta, sender, "lnk", false);
                //Invoca a la clase Actividad Detalle
                using (SAT_CL.Ruta.CostoCaseta cs = new SAT_CL.Ruta.CostoCaseta((int)gvCostoCaseta.SelectedValue))
                {
                    //Asigna a retorno el resultado de invocar el método DeshabilitarActividadRegistro
                    retorno = cs.DeshabilitaCostoCaseta(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                }
                //Valida la acción de eliminar
                if (retorno.OperacionExitosa)
                {
                    //Invoca al método que inicializa la Forma
                    inicializaForma();
                    //Envia mensaje 
                    TSDK.ASP.ScriptServer.MuestraNotificacion(this, retorno, ScriptServer.PosicionNotificacion.AbajoDerecha);
                }
            }

        }
        /// <summary>
        /// Evento que Edita el Puesto a una actividad asignada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEditar_Click(object sender, EventArgs e)
        {
            //Valida que existan registros en el gridView
            if (gvCostoCaseta.DataKeys.Count > 0)
            {
                //Selecciona el registro que se va a editar
                Controles.SeleccionaFila(gvCostoCaseta, sender, "lnk", false);
                //Invoca a la clase Actividad Detalle
                using (SAT_CL.Ruta.CostoCaseta cs = new SAT_CL.Ruta.CostoCaseta((int)gvCostoCaseta.SelectedValue))
                {
                    //Carga los controles para su edición
                    ddlUnidadAutomotriz.SelectedValue = cs.id_tipo.ToString();
                    txtNoEjes.Text = cs.no_ejes.ToString();
                    txtCosto.Text = cs.costo_caseta.ToString();
                    ddlActualizacion.SelectedValue = cs.id_tipo_actualización.ToString();
                    txtFecha.Text = cs.fecha_actualizacion.ToString("dd/MM/yyyy HH:mm");
                    //Focaliza el cursor en ddlUnidadAutomotriz
                    ddlUnidadAutomotriz.Focus();

                }
            }
        }
        /// <summary>
        /// Evento producido al cambiar el indice de página del GV de Eventos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvCostoCaseta_OnpageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Permite cambiar de indice de pagina acorde al tamaño del gridview
            Controles.CambiaIndicePaginaGridView(gvCostoCaseta, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, true, 2);
        }
        /// <summary>
        /// Evento producido al cambiar el tamaño de página del GV de Eventos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvCostoCaseta_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambia el rango de valores visibles en el gridview (25-50-75-100 registros por vista)
            Controles.CambiaTamañoPaginaGridView(gvCostoCaseta, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamañoGridViewCostoCaseta.SelectedValue), true, 2);
        }
        /// <summary>
        /// Eventó producido al pulsar el botón de exportación de contenido de GV de Eventos a excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarExcelCostoCaseta_Onclick(object sender, EventArgs e)
        {
            //Invoca al metodoq ue permite exportar el gridview a formato de excel.
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "Id");
        }
        /// <summary>
        /// Evento producido al cambiar el criterio de orden del GV de Eventos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvCostoCaseta_Onsorting(object sender, GridViewSortEventArgs e)
        {
            //Permite el ordenamiento de las columnas de manera ascendente o descendente
            lblCriterioGridViewCostoCaseta.Text = Controles.CambiaSortExpressionGridView(gvCostoCaseta, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression, true, 2);
        }
               
        #endregion
        #region Métodos
        /// <summary>
        /// Método encargado de inicializar todos los aspectos de la página
        /// </summary>
        private void inicializaForma()
        {
            //Invoca al método cargaCatalogos();
            cargaCatalogos();
            //Invoca al método habilitaControles().
            habilitaControles();
            //Invoca al método habilitaMenu().
            habilitaMenu();
            //Invoca al método que inicializa los valores del gridview
            inicializaCostoCaseta();
            //Invoca al método inicializaValores().
            inicializaValores();

        }
        /// <summary>
        /// Método que inicializa los valores de los controles dropdown list
        /// </summary>
        private void cargaCatalogos()
        {
            //Carga los valores al dropdownlist TipoCaseta
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipoCaseta, "", 3164);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlRedCarretera, "", 3168);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlUnidadAutomotriz, 24, "", 0, "", 0, "");
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlActualizacion, "", 3166);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamañoGridViewCostoCaseta, "", 26);

        }
        /// <summary>
        /// Método que hablita los controles de acuerdo al estado de la págia
        /// </summary>
        private void habilitaControles()
        {
            //Valida el estatus de la página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //En casod e que la página este en Nuevo
                case Pagina.Estatus.Nuevo:
                    {

                        //Habilita los controles para su uso
                        txtDescripcion.Enabled =
                        txtAlias.Enabled =
                        ddlTipoCaseta.Enabled =
                        ddlRedCarretera.Enabled =
                        chkIAVE.Enabled =
                        btnGuardar.Enabled =
                        btnCancelar.Enabled = true;
                        //SecciónCostoCaseta
                        ddlUnidadAutomotriz.Enabled =
                        txtNoEjes.Enabled =
                        ddlActualizacion.Enabled =
                        txtFecha.Enabled =
                        btnAgregarCosto.Enabled =                        
                        gvCostoCaseta.Enabled =
                        btnAgregarCosto.Enabled =
                        btnCancelarCosto.Enabled = false;


                        break;
                    }
                case Pagina.Estatus.Edicion:
                    {
                        //Habilita los controles para su uso
                        txtDescripcion.Enabled =
                        txtAlias.Enabled =
                        ddlTipoCaseta.Enabled =
                        ddlRedCarretera.Enabled =
                        chkIAVE.Enabled =
                        btnGuardar.Enabled =
                        btnCancelar.Enabled =
                        //SecciónCostoCaseta
                        ddlUnidadAutomotriz.Enabled =
                        txtNoEjes.Enabled =
                        ddlActualizacion.Enabled =
                        txtFecha.Enabled =
                        btnAgregarCosto.Enabled =
                        gvCostoCaseta.Enabled =
                        btnAgregarCosto.Enabled =
                        btnCancelarCosto.Enabled = true;
                        break;
                    }
                //En caso de que la página este en Lectura
                case Pagina.Estatus.Lectura:
                    {
                        //Habilita los controles para su uso
                        txtDescripcion.Enabled =
                        txtAlias.Enabled =
                        ddlTipoCaseta.Enabled =
                        ddlRedCarretera.Enabled =
                        chkIAVE.Enabled =
                        btnGuardar.Enabled =
                        btnCancelar.Enabled = 
                        //SecciónCostoCaseta
                        ddlUnidadAutomotriz.Enabled =
                        txtNoEjes.Enabled =
                        ddlActualizacion.Enabled =
                        txtFecha.Enabled =
                        btnAgregarCosto.Enabled =
                        gvCostoCaseta.Enabled =
                        btnAgregarCosto.Enabled =
                        btnCancelarCosto.Enabled = false;
                        break;
                    }
            }

        }
        /// <summary>
        /// Método que habilita las opciones del ménu acorde al estatus de la página
        /// </summary>
        private void habilitaMenu()
        {
            //Valida cada estatus de la página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //Si se encuentra en modo nuevo
                case Pagina.Estatus.Nuevo:
                    {
                        //Habilita y Deshabilita las opciones del menú acorde a su funcionamiento
                        lkbNuevo.Enabled =
                        lkbAbrir.Enabled =
                        lkbGuardar.Enabled = true;
                        lkbEliminar.Enabled =
                        lkbEditar.Enabled =
                        lkbBitacora.Enabled =
                        lkbReferencias.Enabled =
                        btnAgregarCosto.Enabled =
                        btnCancelarCosto.Enabled = false;
                        btnGuardar.Enabled =
                        btnCancelar.Enabled = true;
                        break;
                    }
                //Si se encuentra en modo Edición
                case Pagina.Estatus.Edicion:
                    {
                        //Habilita y Deshabilita las opciones del menú acorde a su funcionamiento
                        lkbNuevo.Enabled =
                        lkbAbrir.Enabled =
                        lkbGuardar.Enabled =
                        lkbEliminar.Enabled = true;
                        lkbEditar.Enabled = false;
                        lkbBitacora.Enabled =
                        lkbReferencias.Enabled =
                        btnAgregarCosto.Enabled =
                        btnCancelarCosto.Enabled =
                        btnCancelar.Enabled =
                        btnGuardar.Enabled = true;
                        break;
                    }
                //Si se encuentra en modo Lectura
                case Pagina.Estatus.Lectura:
                    {
                        //Habilita y Deshabilita las opciones del menú acorde a su funcionamiento
                        lkbNuevo.Enabled =
                        lkbAbrir.Enabled = true;
                        lkbGuardar.Enabled = false;
                        lkbEliminar.Enabled =
                        lkbEditar.Enabled =
                        lkbBitacora.Enabled =
                        lkbReferencias.Enabled = true;
                        btnAgregarCosto.Enabled =
                        btnCancelarCosto.Enabled =
                        btnCancelar.Enabled =
                        btnGuardar.Enabled = false;
                        break;
                    }
            }

        }
        /// <summary>
        /// Método que inicializa los controles acorde al estado de la página
        /// </summary>
        private void inicializaValores()
        {
            //Valida cada estado de la página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //Si se encuentra en modo Nuevo
                case Pagina.Estatus.Nuevo:
                    {
                        //Limpia los controles.
                        //Habilita los controles para su uso
                        txtDescripcion.Text = "";
                        txtAlias.Text = "";
                        txtNoEjes.Text = "";
                        txtCosto.Text = "";
                        txtFecha.Text = Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");                        
                        break;
                    }
                //Si se encuentra en modo Edicion o Lectura
                case Pagina.Estatus.Edicion:
                case Pagina.Estatus.Lectura:
                    {
                        //Instancia a la calse Caseta
                        using (SAT_CL.Ruta.Caseta cs = new SAT_CL.Ruta.Caseta(Convert.ToInt32(Session["id_registro"])))
                        {
                            txtDescripcion.Text = cs.descripcion_caseta;
                            txtAlias.Text = cs.alias_caseta;
                            ddlTipoCaseta.SelectedValue = cs.id_tipo_caseta.ToString();
                            ddlRedCarretera.SelectedValue = cs.id_red_carretera.ToString();
                            chkIAVE.Checked = cs.bit_iave;
                        }

                        break;
                    }
            }
        }
        /// <summary>
        /// Método que permite almacenar las casetas por compañia
        /// </summary>
        private void guardarCaseta()
        {
            //Creación del objeto retorno
            RetornoOperacion result = new RetornoOperacion();
            SqlGeography geoubicacion = Session["geoubicacion"] != null ? (SqlGeography)Session["geoubicacion"] : SqlGeography.Point(0, 0, 4326);
            //Valida cada estatus de la página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //si el estado de la página es Nuevo
                case Pagina.Estatus.Nuevo:
                    {
                        //Instancia a la clase Caseta
                        result = SAT_CL.Ruta.Caseta.InsertarCaseta(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, txtDescripcion.Text, txtAlias.Text, geoubicacion, (SAT_CL.Ruta.Caseta.TipoCaseta)Convert.ToByte(ddlTipoCaseta.SelectedValue), (SAT_CL.Ruta.Caseta.RedCarretera)Convert.ToInt32(ddlRedCarretera.SelectedValue), chkIAVE.Checked, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                        break;
                    }
                //Si el estado de la página es Edición
                case Pagina.Estatus.Edicion:
                    {
                        //Instanciá a la clase Caseta
                        using (SAT_CL.Ruta.Caseta cs = new SAT_CL.Ruta.Caseta((int)Session["id_registro"]))
                        {
                            //Valida que exista el registro
                            if (cs.id_caseta > 0)
                            {
                                //Asigna valores al objeto retorno
                                result = cs.EditarCaseta(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, txtDescripcion.Text, txtAlias.Text, geoubicacion, (SAT_CL.Ruta.Caseta.TipoCaseta)Convert.ToByte(ddlTipoCaseta.SelectedValue), (SAT_CL.Ruta.Caseta.RedCarretera)Convert.ToInt32(ddlRedCarretera.SelectedValue), chkIAVE.Checked, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                            }
                        }
                        break;
                    }
            }            
            //Valida si la inserción a la base de datos se realizo correctamente
            if (result.OperacionExitosa)
            {
                //A la variable de sessión estatus le asigna el estado de la pagina en modo lectura
                Session["estatus"] = Pagina.Estatus.Edicion;
                //A la variable de session id_registro le asigna el valor insertado en la tabla Caseta
                Session["id_registro"] = result.IdRegistro;
                //Invoca al método inicializa forma
                inicializaForma();
            }
            //Muestra el mensaje de error
            TSDK.ASP.ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);

        }
        /// <summary>
        /// Método que almacena los costos ligados a una caseta
        /// </summary>
        private void guardarCostoCaseta()
        {
            //Creaón del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Instancia a la clase Caseta            
            using (SAT_CL.Ruta.Caseta cs = new SAT_CL.Ruta.Caseta((int)Session["id_registro"]))
            {
                //Valida que exista una actividad
                if (cs.id_caseta != 0)
                {
                    //Valida si hay registros en el gridvie CostoCaseta
                    if (gvCostoCaseta.SelectedIndex != -1)
                    {

                        //Invoca a la claseCostoCaseta
                        using (SAT_CL.Ruta.CostoCaseta ccs = new SAT_CL.Ruta.CostoCaseta(Convert.ToInt32(gvCostoCaseta.SelectedDataKey["Id"])))
                        {
                            //Asigna retorno a valor con los valores a editar.
                            retorno = ccs.EditarCostoCaseta(cs.id_caseta,(SAT_CL.Ruta.CostoCaseta.Tipo)Convert.ToByte(ddlUnidadAutomotriz.SelectedValue),Convert.ToInt32(txtNoEjes.Text),Convert.ToDecimal(txtCosto.Text),(SAT_CL.Ruta.CostoCaseta.TipoActualizacion)Convert.ToByte(ddlActualizacion.SelectedValue),Convert.ToDateTime(txtFecha.Text), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);                                
                        }
                    }
                    else
                    {
                        //Realiza una insercion
                        retorno = SAT_CL.Ruta.CostoCaseta.InsertarCostoCaseta(cs.id_caseta, (SAT_CL.Ruta.CostoCaseta.Tipo)Convert.ToByte(ddlUnidadAutomotriz.SelectedValue), Convert.ToInt32(txtNoEjes.Text), Convert.ToDecimal(txtCosto.Text), (SAT_CL.Ruta.CostoCaseta.TipoActualizacion)Convert.ToByte(ddlActualizacion.SelectedValue), Convert.ToDateTime(txtFecha.Text), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                    }
                }
            }
            
            //Valida si se realizo crectamente la accion
            if (retorno.OperacionExitosa)
            {
                //Limpia el contenido de los controles.
                txtNoEjes.Text = "";
                txtCosto.Text = "";
                txtFecha.Text = Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");
                //Invoca al método inicializaForma
                inicializaForma();
            }
            //Envia un ménsaje acorde a la acción.
            TSDK.ASP.ScriptServer.MuestraNotificacion(this, retorno, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Método que inicializa los valores del gridview
        /// </summary>
        private void inicializaCostoCaseta()
        {
            //Valida cada estatus de la página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //Si la pagina esta en Nuevo
                case Pagina.Estatus.Nuevo:
                    {
                        Controles.InicializaGridview(gvCostoCaseta);
                        break;
                    }
                //En caso de lectura y Edición
                case Pagina.Estatus.Edicion:
                case Pagina.Estatus.Lectura:
                    {
                        //Instancia a la clase actividad
                        using (SAT_CL.Ruta.Caseta cs = new SAT_CL.Ruta.Caseta((int)Session["id_registro"]))
                        {
                            //Valida que exista una actividad
                            if (cs.id_caseta != 0)
                            {
                                //Instancia a la clase actividad puesto
                                using (DataTable dtCostoCaseta = SAT_CL.Ruta.CostoCaseta.ObtieneCostoCaseta(cs.id_caseta))
                                {
                                    //Valida que existan datos en el datatable
                                    if (Validacion.ValidaOrigenDatos(dtCostoCaseta))
                                    {
                                        //Si existen registros, carga el gridview 
                                        Controles.CargaGridView(gvCostoCaseta, dtCostoCaseta, "Id", "", true, 2);
                                        //Asigna valores a la variable de session del DS
                                        Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtCostoCaseta, "Table");
                                    }
                                    //En caso contrario
                                    else
                                    {
                                        //Inicializa el gridView
                                        Controles.InicializaGridview(gvCostoCaseta);
                                        //Elimina los datos del dataset si se realizo una consulta anterior
                                        Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                                    }
                                }
                            }
                        }
                        break;
                    }
            }
            Controles.InicializaIndices(gvCostoCaseta);
        }

        /// <summary>
        ///  Método que permie la consulta del historial de modificicaciones realizadas a un registro 
        /// </summary>
        /// <param name="idRegistro">ID que permite identificar un registro de Caseta</param>
        /// <param name="idTabla">Identificador de la tabla Caseta</param>
        /// <param name="Titulo">Encabezado de la ventana Bitacora</param>
        private void inicializaBitacora(string idRegistro, string idTabla, string Titulo)
        {
            //Creación de la variable url, que almacena una ruta de ubicación de la ventana, que contendra la bitacora de un registros de  Caseta.
            string url = Cadena.RutaRelativaAAbsoluta("~/Ruta/Caseta.aspx",
                                                                "~/Accesorios/BitacoraRegistro.aspx?idT=" + idTabla + "&idR=" + idRegistro + "&tB=" + Titulo);
            //Variable que almacena la resolucion de la ventana bitacora
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Invoca el método de la clase ScriptServer que abrira una nueva ventana acorde a una ubicacion, un titulo de la ventana, una configuración de dimensiones.
            ScriptServer.AbreNuevaVentana(url, "Bitacora  Caseta", configuracion, Page);
        }
        /// <summary>
        /// Método que permite la busqueda de registros en la base de datos pertenecientes a la tabla Caseta
        /// </summary>
        /// <param name="idTabla">Id que identifica a la tabla Caseta registrada en la base de datos</param>
        /// <param name="idCompania"> Id que identifica a una compañia</param>
        private void inicializaAperturaRegistro(int idTabla, int idCompania)
        {
            //Asigna a la variable de session id_tabla el valor del parametro idTabla
            Session["id_tabla"] = idTabla;
            //Asigna valores a la variable url con la ruta de ubicacion de la ventana Abrir registros con los datos obtenidos de la tabla Caseta
            string url = Cadena.RutaRelativaAAbsoluta("~/Ruta/Caseta.aspx", "~/Accesorios/AbrirRegistro.aspx?P1=" + idCompania.ToString());
            //Define las dimensiones de la ventana Abrir registros de Caseta
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Invoca al método de la variable ScripServer que abrira una nueva ventana con los registros pertenecientes a la tabla Caseta
            ScriptServer.AbreNuevaVentana(url, "Abrir Registro Caseta", configuracion, Page);
        }
        /// <summary>
        /// Método encargado de inicializr la referencias a un registro
        /// </summary>
        /// <param name="id_registro">Id que permite identificar un registro de la tabla Caseta</param>
        /// <param name="id_tabla">Id que permite identificar a la tabla Caseta en la base de datos</param>
        /// <param name="id_compania">Id que permite identificar a que compania pertenece </param>
        private void inicializaReferenciaRegistro(string id_registro, string id_tabla, string id_compania)
        {
            //Asigna a la variable urlDestino una ruta de ubucación de la ventana de Referencia de la tabla Caseta
            string urlDestino = Cadena.RutaRelativaAAbsoluta("~/Ruta/Caseta.aspx", "~/Accesorios/ReferenciaRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idC=" + id_compania);
            //Invoca al método de la clase ScripServer que abrira una nueva ventana con los valores de Referencia de la tabla Caseta
            ScriptServer.AbreNuevaVentana(urlDestino, "Referencia de Registros Caseta", 800, 500, false, false, false, true, true, Page);
        }
        #endregion





    }
}