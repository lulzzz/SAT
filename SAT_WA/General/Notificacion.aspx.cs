using SAT_CL;
using System;
using System.Data;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;
using SAT_CL.Global;
namespace SAT.General
{
    public partial class Notificacion : System.Web.UI.Page
    {
        /// <summary>
        /// Evento generado al cargar la Página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Si no es una recraga de página
            if (!this.IsPostBack)
                inicializaForma();
        }

        #region Eventos Notificación
        /// <summary>
        /// Evento click sobre algún elemento del menú de la página
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
                        Session["estatus"] = TSDK.ASP.Pagina.Estatus.Nuevo;
                        //Limpiando Id de sesión
                        Session["id_registro"] = 0;
                        //Limpiando contenido de forma
                        inicializaForma();
                        break;
                    }
                case "Abrir":
                    {
                        //Inicializando Apertura de Registros
                        inicializaAperturaRegistro(177, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);
                        break;
                    }
                case "Guardar":
                    {
                        //Si ya existe un registro activo
                        if (Convert.ToInt32(Session["id_registro"]) > 0)
                            //Guarda Notificación
                            guardaNotificacion(lkbGuardar);
                        break;
                    }
                case "Editar":
                    {
                        //Asignando estatus nuevo
                        Session["estatus"] = TSDK.ASP.Pagina.Estatus.Edicion;
                        //Limpiando contenido de forma
                        inicializaForma();
                        break;
                    }
                case "Bitacora":
                    {   //Inicializando Bitacora
                        inicializaBitacora(Session["id_registro"].ToString(), "177", "Notificacion");
                        break;
                    }
                case "Referencias":
                    {   //Invocando Método de Inicialización de Referencias
                        inicializaReferenciaRegistro(Session["id_registro"].ToString(), "177", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString());
                        break;
                    }
                case "Eliminar":
                    {   //Deshabilitamos Notificación
                        deshabilitaNotificacion();
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
        /// Evento generado al Guardar la Notificación
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            //Guarda Notificación
            guardaNotificacion(btnGuardar);
        }

        /// <summary>
        /// Evento generado al Cancelar la Notificación
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            //Si el estatus actual de la página es edición
            if ((TSDK.ASP.Pagina.Estatus)Session["estatus"] == TSDK.ASP.Pagina.Estatus.Edicion)
                //Actualizando estatus a lectura
                Session["estatus"] = TSDK.ASP.Pagina.Estatus.Lectura;
            else if ((TSDK.ASP.Pagina.Estatus)Session["estatus"] == TSDK.ASP.Pagina.Estatus.Nuevo)
                //Actualizando estatus a lectura
                Session["estatus"] = TSDK.ASP.Pagina.Estatus.Nuevo;

            //Inicializando contenido de forma
            inicializaForma();
        }
        #endregion

        #region Métodos Notificación
        /// <summary>
        /// Inicializa el contenido de la forma (cargando catalogos, contenido y asignando habilitación de controles)
        /// </summary>
        private void inicializaForma()
        {
            //Cargando catálogos
            cargaCatalogosEvento();
            //Cargando contenido de controles
            inicializaContenidoControles();
            //Habilitando controles
            habilitaControles();
            //Habilitando elementos del menú
            habilitaMenu();
            //Carga Eventos
            cargaEventos();
        }

        /// <summary>
        /// Inicializa el contenido de los controles (en blanco o predeterminado) con los datos de un registro
        /// </summary>
        private void inicializaContenidoControles()
        {
            //Determinando el estatus de la página
            switch ((TSDK.ASP.Pagina.Estatus)Session["estatus"])
            {
                case TSDK.ASP.Pagina.Estatus.Nuevo:
                    //Borrando el contenido 
                    txtContacto.Text = "";
                    txtCliente.Text = "";
                    txtUbicacion.Text = "";
                    break;
                case TSDK.ASP.Pagina.Estatus.Lectura:
                case TSDK.ASP.Pagina.Estatus.Edicion:
                    //Intsnaciamos Notificación
                    using (SAT_CL.Notificacion.Notificacion objNotificacion = new SAT_CL.Notificacion.Notificacion(Convert.ToInt32(Session["id_registro"])))
                    {
                        //Instanciamos Contacto
                        using (SAT_CL.Global.Contacto objContacto = new SAT_CL.Global.Contacto(objNotificacion.id_contacto))
                        {
                            txtContacto.Text = objContacto.nombre + " ID:" + objContacto.id_contacto.ToString();
                            //Instanciamos Cliente
                            using (SAT_CL.Global.CompaniaEmisorReceptor objCompania = new SAT_CL.Global.CompaniaEmisorReceptor(objNotificacion.id_compania_cliente))
                            {
                                //Si la Notificación es Para Todos los Clientes
                                if (objNotificacion.id_compania_cliente == 0)
                                    txtCliente.Text = "TODOS" + " ID:0";
                                else
                                    txtCliente.Text = objCompania.nombre + " ID:" + objCompania.id_compania_emisor_receptor.ToString();

                            }
                            //Validamos Tabla 
                            if (objNotificacion.id_tabla == 15)
                            {
                                //Si la Notificación es Para Todos los Clientes
                                if (objNotificacion.id_registro == 0)
                                    txtUbicacion.Text = "TODOS" + " ID:0";
                                else
                                    //Si la Tabla es Ubicación
                                    using (SAT_CL.Global.Ubicacion objUbicacion = new SAT_CL.Global.Ubicacion(objNotificacion.id_registro))
                                    {
                                        txtUbicacion.Text = objUbicacion.descripcion + " ID:" + objUbicacion.id_ubicacion.ToString();
                                    }
                            }
                        }
                    }
                    break;
            }

        }

        /// <summary>
        /// Habilita o deshabilita loc controles de la forma en base a su estatus
        /// </summary>
        private void habilitaControles()
        {
            //En base al estatus
            switch ((TSDK.ASP.Pagina.Estatus)Session["estatus"])
            {
                case TSDK.ASP.Pagina.Estatus.Lectura:
                    txtContacto.Enabled =
                    txtCliente.Enabled =
                    txtUbicacion.Enabled =
                    btnGuardar.Enabled =
                    btnCancelar.Enabled = false;
                    break;
                case TSDK.ASP.Pagina.Estatus.Nuevo:
                case TSDK.ASP.Pagina.Estatus.Edicion:
                    txtContacto.Enabled =
                     txtCliente.Enabled =
                     txtUbicacion.Enabled =
                     btnGuardar.Enabled =
                     btnCancelar.Enabled = true;
                    break;
            }
        }

        /// <summary>
        /// Método Privado encargado de Habilitar el Menu
        /// </summary>
        private void habilitaMenu()
        {   //Validando Estatus de Session
            switch ((TSDK.ASP.Pagina.Estatus)Session["estatus"])
            {
                case TSDK.ASP.Pagina.Estatus.Nuevo:
                    {   //Archivo
                        lkbNuevo.Enabled =
                        lkbAbrir.Enabled =
                        lkbGuardar.Enabled =
                        lkbSalir.Enabled = true;
                        //Edicion
                        lkbEditar.Enabled =
                        lkbBitacora.Enabled =
                        lkbReferencias.Enabled =
                        lkbArchivos.Enabled = false;

                        break;
                    }
                case TSDK.ASP.Pagina.Estatus.Lectura:
                    {   //Archivo
                        lkbNuevo.Enabled =
                        lkbAbrir.Enabled =
                        lkbSalir.Enabled = true;
                        lkbGuardar.Enabled = false;
                        //Edicion
                        lkbEditar.Enabled =
                        lkbBitacora.Enabled =
                        lkbReferencias.Enabled =
                        lkbArchivos.Enabled = true;

                        break;
                    }
                case TSDK.ASP.Pagina.Estatus.Edicion:
                    {   //Archivo
                        lkbNuevo.Enabled =
                        lkbAbrir.Enabled =
                        lkbGuardar.Enabled =
                        lkbSalir.Enabled =
                        //Edicion
                        lkbEditar.Enabled =
                        lkbBitacora.Enabled =
                        lkbReferencias.Enabled =
                        lkbArchivos.Enabled = true;

                        break;
                    }
            }
        }

        /// <summary>
        /// Método Privado encargado de Inicializar la Apertura de un Registro
        /// </summary>
        /// <param name="idTabla">Id de Tabla</param>
        private void inicializaAperturaRegistro(int idTabla, int idCompania)
        {   //Asignando Session
            Session["id_tabla"] = idTabla;
            //Construyendo URL 
            string url = string.Format("{0}?P1={1}", Cadena.RutaRelativaAAbsoluta("~/General/Notificacion.aspx", "~/Accesorios/AbrirRegistro.aspx"), ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);
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
            string url = Cadena.RutaRelativaAAbsoluta("~/General/Notificacion.aspx", "~/Accesorios/BitacoraRegistro.aspx?idT=" + idTabla + "&idR=" + idRegistro + "&tB=" + Titulo);
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
            string urlDestino = Cadena.RutaRelativaAAbsoluta("~/General/Notificacion.aspx", "~/Accesorios/ReferenciaRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idC=" + id_compania);
            //Instanciando nueva ventana de navegador para apertura de referencias de registro
            TSDK.ASP.ScriptServer.AbreNuevaVentana(urlDestino, "Referencia", 800, 650, false, false, false, true, true, Page);
        }

        /// <summary>
        /// Evento generado al Guardar una Notificación
        /// </summary>
        /// <param name="control">Control que se Dispara</param>
        private void guardaNotificacion(System.Web.UI.Control control)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();
            //Validando el Estatus de la Página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {
                        //Insertando Notificación
                        resultado = SAT_CL.Notificacion.Notificacion.InsertaNotificacion(Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtContacto.Text, ":", 1)),
                                                                                        ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                                                                        Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtCliente.Text, ":", 1)), 15,
                                                                                        Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtUbicacion.Text, ':', 1)), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                        break;
                    }
                case Pagina.Estatus.Edicion:
                    {
                        //Instanciando Notificación
                        using (SAT_CL.Notificacion.Notificacion objNotificacion = new SAT_CL.Notificacion.Notificacion(Convert.ToInt32(Session["id_registro"])))
                        {

                            //Editando Notificación
                            resultado = objNotificacion.EditaNotificacion(Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtContacto.Text, ":", 1)),
                                                                                    ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                                                                    Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtCliente.Text, ":", 1)), 15,
                                                                                    Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtUbicacion.Text, ':', 1)), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);


                        }
                        break;
                    }
            }
            //Validando Operación Exitosa
            if (resultado.OperacionExitosa)
            {
                //Asignando Sessiones
                Session["id_registro"] = resultado.IdRegistro;
                //Si el estatus actual de la página es edición
                if ((TSDK.ASP.Pagina.Estatus)Session["estatus"] == TSDK.ASP.Pagina.Estatus.Edicion)
                    //Actualizando estatus a lectura
                    Session["estatus"] = TSDK.ASP.Pagina.Estatus.Lectura;
                else if ((TSDK.ASP.Pagina.Estatus)Session["estatus"] == TSDK.ASP.Pagina.Estatus.Nuevo)
                    //Actualizando estatus a lectura
                    Session["estatus"] = TSDK.ASP.Pagina.Estatus.Edicion;

                //Inicializando Página
                inicializaForma();
            }
            //Muestra el mensaje de error
            TSDK.ASP.ScriptServer.MuestraNotificacion(control, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        /// <summary>
        /// Método encargado de guardar de Deshabilitar una Notificación
        /// </summary>
        /// <param name="control">Control que dispara la acción</param>
        private void deshabilitaNotificacion(System.Web.UI.Control control)
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Instanciamos Notificación
            using (SAT_CL.Notificacion.Notificacion objNotificacion = new SAT_CL.Notificacion.Notificacion(Convert.ToInt32(Session["id_registro"])))
            {
                //Deshabilitamos Notificación
                resultado = objNotificacion.DeshabilitaNotificacion(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

            }
            //SI no existen errores
            if (resultado.OperacionExitosa)
            {
                //Asignando estatus de lectura
                Session["estatus"] = Pagina.Estatus.Nuevo;
                //Asignamos Id de Registro
                Session["id_registro"] = 0;
                //Inicialzaindo contenido de forma
                inicializaForma();
            }
            //Muestra el mensaje de error
            TSDK.ASP.ScriptServer.MuestraNotificacion(control, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        /// <summary>
        /// Muestra u oculta la ventana modal solicitada
        /// </summary>
        /// <param name="nombre_ventana">Nombre de ventana por ocultar</param>
        /// <param name="control">Control que solicita la acción</param>
        private void alternaVentanaModal(string nombre_ventana, System.Web.UI.Control control)
        {
            //Determinando la ventana por alternar
            switch (nombre_ventana)
            {
                case "contacto":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "crearContacto", "contenedorContacto");
                    break;
            }
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

                case "Contacto":
                    //Cerramos Ventana Modal
                    alternaVentanaModal("contacto", lkbCerrar);
                    break;
            }
        }

        #endregion

        #region Métodos Eventos

        /// <summary>
        /// Método encargado de cargar los Eventos
        /// </summary>
        private void cargaEventos()
        {
            //Obtenemos Depósito
            using (DataTable mit = SAT_CL.Notificacion.DetalleNotificacion.CargaDetalleNotificacion(Convert.ToInt32(Session["id_registro"])))
            {

                //Validando que el DataSet contenga las tablas
                if (Validacion.ValidaOrigenDatos(mit))
                {
                    //Cargando los GridView
                    Controles.CargaGridView(gvEvento, mit, "Id", "", false, 1);
                    //Añadiendo Tablas al DataSet de Session
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], mit, "Table");

                }
                else
                {   //Inicializando GridViews
                    Controles.InicializaGridview(gvEvento);


                    //Eliminando Tablas del DataSet de Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                }
            }
            //Inicializamos Indices
            Controles.InicializaIndices(gvEvento);
        }

        /// <summary>
        /// Método encargado de Deshabilitar el Evento
        /// </summary>
        private void deshabilitaEvento()
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Innstanciamos Vales
            using (SAT_CL.Notificacion.DetalleNotificacion objDetalle = new SAT_CL.Notificacion.DetalleNotificacion(Convert.ToInt32(gvEvento.SelectedValue)))
            {
                //Deshabilitamos Evento
                resultado = objDetalle.DeshabilitarDetallleNotificacion(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

            }
            //SI no existen errores
            if (resultado.OperacionExitosa)
            {
                //Cargamos Eventos
                cargaEventos();
                //Inicializamos Indices
                Controles.InicializaIndices(gvEvento);
            }
            //Muestra el mensaje de error
            TSDK.ASP.ScriptServer.MuestraNotificacion(gvEvento, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        /// <summary>
        /// Método encargado de guardar el Evento
        /// </summary>
        private void guardaEvento()
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Recuperando controles 
            using (DropDownList ddlEvento = (DropDownList)gvEvento.FooterRow.FindControl("ddlEvento"))
            {
                //Insertamos Evento
                resultado = SAT_CL.Notificacion.DetalleNotificacion.InsertarDetalleNotificacion(Convert.ToInt32(Session["id_registro"]), Convert.ToInt32(ddlEvento.SelectedValue),
                               ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

            }


            //SI no existen errores
            if (resultado.OperacionExitosa)
            {
                //Cargamos Eventos
                cargaEventos();
                //Inicializamos Indices
                Controles.InicializaIndices(gvEvento);
            }
            //Muestra el mensaje de error
            TSDK.ASP.ScriptServer.MuestraNotificacion(gvEvento, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        /// <summary>
        /// Realiza la carga de los controles de selección que lo requieran
        /// </summary>
        private void cargaCatalogosEvento()
        {
            //Tamaño de la Caseta
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoEvento, "", 56);

        }

        /// <summary>
        /// Método encargado de guardar de Deshabilitar una Notificacion
        /// </summary>
        private void deshabilitaNotificacion()
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Instanciamos Ruta
            using (SAT_CL.Notificacion.Notificacion objNotificacion = new SAT_CL.Notificacion.Notificacion(Convert.ToInt32(Session["id_registro"])))
            {
                //Deshabilitamos Tipo Unidad
                resultado = objNotificacion.DeshabilitaNotificacion(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

            }
            //SI no existen errores
            if (resultado.OperacionExitosa)
            {
                //Asignando estatus de lectura
                Session["estatus"] = Pagina.Estatus.Nuevo;
                //Asignamos Id de Registro
                Session["id_registro"] = 0;
                //Inicialzaindo contenido de forma
                inicializaForma();
            }
            //Muestra el mensaje de error
            TSDK.ASP.ScriptServer.MuestraNotificacion(lkbEliminar, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        #endregion

        #region Eventos 

        /// <summary>
        /// Evento generado al Insertar un Detalles
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkInsertar_Click(object sender, EventArgs e)
        {
            //Guardamos Evento
            guardaEvento();
        }

        /// <summary>
        /// Evento generado al DFeshabilitar un Evento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkDeshabilitar_Click(object sender, EventArgs e)
        {
            //Si hay registros
            if (gvEvento.DataKeys.Count > 0)
            {
                //Seleccionando la fila correspondiente
                Controles.SeleccionaFila(gvEvento, sender, "lnk", false);
                //Deshabilita  eVENTO
                deshabilitaEvento();
            }
        }

        /// <summary>
        /// Evento generado a Mostrar la Bitácora
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkBitacora_Click(object sender, EventArgs e)
        {
            //Si hay registros
            if (gvEvento.DataKeys.Count > 0)
            {
                //Seleccionando la fila correspondiente
                Controles.SeleccionaFila(gvEvento, sender, "lnk", false);
                //Invocando Método de Inicializacion de Bitacora
                inicializaBitacora(gvEvento.SelectedValue.ToString(), "178", "Detalle");
            }
        }

        /// <summary>
        /// Evento corting de gridview de Evento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvEvento_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Validando que existan Llaves
            if (gvEvento.DataKeys.Count > 0)
            {   //Asignando Criterio de Ordenacion
                OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table").DefaultView.Sort = lblOrdenadoEvento.Text;
                //Cambiando Ordenamiento
                lblOrdenadoEvento.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvEvento, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression, false, 1);
            }
        }

        /// <summary>
        /// Evento cambio de página en gridview de Evento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvEvento_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvEvento, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, false, 1);
        }
        /// <summary>
        /// Evento cambio de tamaño de página de gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoEvento_SelectedIndexChanged(object sender, EventArgs e)
        {
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvEvento, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamanoEvento.SelectedValue), false, 1);
        }
        /// <summary>
        /// Evento click en botón de exportación de Evento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarEvento_Click(object sender, EventArgs e)
        {
            TSDK.ASP.Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "");
        }

        /// <summary>
        /// Inicializa los Controles del GV Evento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvEvento_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Obtenemos la fila actual
            GridViewRow fila = e.Row;

            //Verificamos si la fila es para los elementos de datos o es la fila del footer
            switch (fila.RowType)
            {
                //Fila tipo Datos, para obtener objetos LinkButton correspondientes
                case DataControlRowType.DataRow:
                    //Creamos instancias del tipo LinkButton correspondientes a Deshabilitar y Guardar
                    using (LinkButton lnkBitacora = (LinkButton)fila.FindControl("lnkBitacora"),
                      lnkDeshabilitar = (LinkButton)fila.FindControl("lnkDeshabilitar"))
                    {
                        switch ((Pagina.Estatus)Session["estatus"])
                        {
                            //Habilitamos campos de acuerdo al estatus de la pagina
                            case Pagina.Estatus.Nuevo:
                            case Pagina.Estatus.Copia:
                                {
                                    // Deshabilitamos LinkButton´s
                                    lnkBitacora.Enabled =
                                    lnkDeshabilitar.Enabled = false;
                                }
                                break;
                            case Pagina.Estatus.Lectura:
                                {
                                    // Deshabilitamos LinkButton´s
                                    lnkBitacora.Enabled =
                                    lnkDeshabilitar.Enabled = false;
                                    break;
                                }
                            case Pagina.Estatus.Edicion:
                                {
                                    //Si la fila no esta en modo de edicion
                                    if (gvEvento.EditIndex == -1)
                                    {
                                        lnkDeshabilitar.Enabled =
                                        lnkBitacora.Enabled = true;
                                    }
                                    break;
                                }
                        }
                    }
                    break;
                //Fila Tipo Footer para Obtener los datos
                case DataControlRowType.Footer:
                    {
                        //Creamos Instancias de Tipo TextBox y DropDownList

                        using (DropDownList ddlEvento = (DropDownList)fila.FindControl("ddlEvento"))
                        {
                            using (LinkButton lnkInsertar = (LinkButton)fila.FindControl("lnkInsertar"))
                            {
                                //Validando Estatus de la Pagina
                                switch ((Pagina.Estatus)Session["estatus"])
                                {
                                    case Pagina.Estatus.Nuevo:
                                        //case Pagina.Estatus.Copiar:
                                        {
                                            //Deshabilitamos controles
                                            lnkInsertar.Enabled =
                                            ddlEvento.Enabled = false;
                                        }
                                        break;
                                    case Pagina.Estatus.Lectura:
                                        {
                                            //Deshabilitamos controles
                                            lnkInsertar.Enabled =
                                          ddlEvento.Enabled = false;

                                        }
                                        break;
                                    case Pagina.Estatus.Edicion:
                                        {
                                            //Habilitamos controles 
                                            lnkInsertar.Enabled =
                                            ddlEvento.Enabled = true;

                                        }
                                        break;
                                }
                                //Validamos que exista Notificación
                                if (Convert.ToInt32(Session["id_registro"]) > 0)
                                {
                                    //Intsnaciamos Notificación
                                    using (SAT_CL.Notificacion.Notificacion objNotificacion = new SAT_CL.Notificacion.Notificacion(Convert.ToInt32(Session["id_registro"])))
                                    {
                                        //cargando catalogo Tipo Operacion
                                        SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlEvento, 95, "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", objNotificacion.id_tabla, "");
                                    }
                                }
                                else
                                {
                                    //cargando catalogo Tipo Operacion
                                    SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlEvento, 95, "Sin Asignar", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, "");
                                }

                            }
                        }
                    }
                    break;
            }
        }



        #endregion

        #region Métodos

        /// <summary>
        /// Inicializa el contenido de los controles (en blanco o predeterminado) con los datos de un registro
        /// </summary>
        private void inicializaContenidoControlesContacto()
        {
            //Si ya existe un registro activo de Contacto
            if (Convert.ToInt32(Session["id_registro_b"]) != 0)
            {
                //Instanciamos Contacto
                using (SAT_CL.Global.Contacto objContacto = new SAT_CL.Global.Contacto(Convert.ToInt32(Session["id_registro_b"])))
                {
                    txtNombre.Text = objContacto.nombre;
                    txtTelefono.Text = objContacto.telefono;
                    txtEMail.Text = objContacto.email;
                }
            }
            else
            {
                txtNombre.Text = "";
                txtTelefono.Text = "";
                txtEMail.Text = "";
            }

        }

        /// <summary>
        /// Método encargado de Guardar un Contacto
        /// </summary>
        private void guardaContacto()
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();
            //Si ya existe un registro activo de Contacto
            if (Convert.ToInt32(Session["id_registro_b"]) == 0)
            {
                //Insertando Contacto
                resultado = SAT_CL.Global.Contacto.InsertaContacto(txtNombre.Text, txtTelefono.Text, txtEMail.Text,
                            ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, 0,
                            ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
            }
            else
            {
                //Instanciando  Contacto
                using (SAT_CL.Global.Contacto objContacto = new SAT_CL.Global.Contacto(Convert.ToInt32(Session["id_registro_b"])))
                {

                    //Editando Contacto
                    resultado = objContacto.EditaContacto(txtNombre.Text, txtTelefono.Text, txtEMail.Text, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);


                }
            }

            //Validando Operación Exitosa
            if (resultado.OperacionExitosa)
            {
                //Establecemos Registro
                Session["id_registro_b"] = resultado.IdRegistro;

                //Inicializa Contenido Controles
                inicializaContenidoControlesContacto();

            }
            //Muestra el mensaje de error
            TSDK.ASP.ScriptServer.MuestraNotificacion(btnAceptarContacto, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        /// <summary>
        /// Método encargado Deshabilitar un Contacto
        /// </summary>
        private void deshabilitaContacto()
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();
            //Si ya existe un registro activo de Contacto
            if (Convert.ToInt32(Session["id_registro_b"]) > 0)
            {
                //Instanciando  Contacto
                using (SAT_CL.Global.Contacto objContacto = new SAT_CL.Global.Contacto(Convert.ToInt32(Session["id_registro_b"])))
                {
                    resultado = objContacto.DeshabilitaContacto(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                }
            }

            //Validando Operación Exitosa
            if (resultado.OperacionExitosa)
            {
                //Inicializa Contenido Controles
                inicializaContenidoControlesContacto();

                //Cerramos Ventana Modal
                alternaVentanaModal("contacto", btnEliminarContacto);

                //Limpiamos Control de Contacto
                txtContacto.Text = "";
            }
            //Muestra el mensaje de error
            TSDK.ASP.ScriptServer.MuestraNotificacion(btnEliminarContacto, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        #endregion
        #region Eventos Contacto

        /// <summary>
        /// Eventio generado al Mostrar la Ventana de Cobntacto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkVentana_Click(object sender, EventArgs e)
        {
            //Si ya existe un registro activo de Contacto
            Session["id_registro_b"] = Cadena.RegresaCadenaSeparada(txtContacto.Text, ':', 1);

            //Inicializamos Controles
            inicializaContenidoControlesContacto();

            //Mostramos Modal
            alternaVentanaModal("contacto", lnkVentana);
        }

        /// <summary>
        /// Evento generado al Aceptar el Contacto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarContacto_Click(object sender, EventArgs e)
        {
            //Guarda Contacto
            guardaContacto();
        }

        #endregion

        /// <summary>
        /// Evento generado al Eliminar un Contacto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEliminarContacto_Click(object sender, EventArgs e)
        {
            //Deshabilitamos Contacto
            deshabilitaContacto();
        }








    }
}