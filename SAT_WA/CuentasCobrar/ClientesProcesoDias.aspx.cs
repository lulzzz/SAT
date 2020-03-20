using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;
using System.Data;


namespace SAT.CuentasCobrar
{
    public partial class ClientesProcesoDias : System.Web.UI.Page
    {

        #region Atributos

        // Atributo que guarda la verificacion echa para saber si se esta trabajando con dias de la semana o dias del mes 
        private int verificar = 0;

        #endregion 

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            //Aplicando Seguridad a la Forma
            SAT_CL.Seguridad.Forma.AplicaSeguridadForma(this, "content1", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

            //Verificando que no sea un PostBack
            if (!Page.IsPostBack)
            {
                inicializaForma();
            }
           
        }

        /// <summary>
        /// Evento que ejecuta cuando se selecciona un elemento del menu principal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbEventosMenu_Click(object sender, EventArgs e)
        {
            LinkButton botonMenu = (LinkButton)sender;

            //Obtenemos cual fue el control que disparo el evento
            switch (botonMenu.CommandName)
            {
                //Si fue el control Nuevo
                case "Nuevo":
                    {
                        //Asignamos el estatus Nuevo al estatus de la pagina
                        Session["estatus"] = Pagina.Estatus.Nuevo;
                        //Asignamos un 0 a la variable id_registro de la sesion
                        Session["id_registro"] = 0;
                        //Invocamos al metodo inicializa forma
                        inicializaForma();
                        break;
                    }
                //Si fue el control Abrir
                case "Abrir":
                    {
                        //Invocamos al metodo inicializaAperturaRegistro
                        inicializaAperturaRegistro(205, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);
                        //Invocamos al metodo inicializa forma
                        inicializaForma();
                        break;
                    }
                //Si fue el control Guardar
                case "Guardar":
                    {
                        //Invocamos al metodo guardaClienteProceso
                        guardaClienteProceso();
                        break;
                    }
                //Si fue el control Editar
                case "Editar":
                    {
                         //Asignamos el estatus Edicion al estatus de la pagina
                        Session["estatus"] = Pagina.Estatus.Edicion;
                        //Invocamos al metodo inicializaForma
                        inicializaForma();
                        break;
                    }
                //Si fue el control Eliminar
                case "Eliminar":
                    {
                        //Invocamos al metodo deshabilitaClienteProceso
                        deshabilitaClienteProceso();
                        break;
                    }
                //Si fue el control Bitacora
                case "Bitacora":
                    {
                        //Invocamos al metodo inicializaBitacora
                        inicicalizaBitacora(Session["id_registro"].ToString(), "205", "Clientes Procesos");
                        break;
                    }
                //Si fue el control Referencia
                case "Referencias":
                    {
                        //Invocamos al metodo inicializaReferenciaRegistro
                        inicializaReferenciaRegistro(Session["id_registro"].ToString(), "205",
                                                  ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString());

                        break;
                    }
                //Si fue el control Archivos
                case "Archivos":
                    {
                        break;
                    }


            }
        }

        /// <summary>
        /// Evento que ejecuta cuando se da clic al boton guardar del apartado Clientes Procesos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            //Invocamos al metodo guardaClienteProceso
            guardaClienteProceso();
        }

        /// <summary>
        /// Evento que ejecuta cuando se da clic al boton cancelar del apartado Clientes Procesos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            //Verificamos el estado de la pagina
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //Si se encuentra en un estatus de Nuevo
                case Pagina.Estatus.Nuevo:
                    {
                        //Asignamos el estatus de nuevo a la pagina
                        Session["estatus"] = Pagina.Estatus.Nuevo;
                        //Asignamos un 0 a la variable id_registro de sesion
                        Session["id_registro"] = 0;
                        break;
                    }
                //Si se encuentra en un estatus de Edicion
                case Pagina.Estatus.Edicion:
                    {
                        //Asignamos el estatus de Lectura a la pagina
                        Session["estatus"] = Pagina.Estatus.Lectura;
                        break;
                    }
            }
            //Invocamos al metodo inicializaForma
            inicializaForma();
        }

        /// <summary>
        /// Evento que ejecuta cuando se cambia el estado de  los Radio Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rdbDias_CheckedChanged(object sender, EventArgs e)
        {
            //If el radio con el id rdbDiasSemana esta checado
            if (rdbDiasSemana.Checked == true)
            {
                //Al dropDownList que contiene los dias de la semana se habilita
                ddlDiasSemana.Enabled = true;
                //Al textBox en donde ingresas dias de mes se deshabilita
                txtDiaMes.Enabled = false;
            }
            //If el radio con el id rdbDiasMes esta checado
            if (rdbDiasMes.Checked == true)
            {
                //Al textBox en donde ingresas dias de mes se Habilita
                txtDiaMes.Enabled = true;
                //Al dropDownList que contiene los dias de la semana se deshabilita
                ddlDiasSemana.Enabled = false;
            }
        }

        /// <summary>
        /// Evento que ejecuta cuando se da clic al boton guardar del apartado Dias Revision
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardarDiasRevision_Click(object sender, EventArgs e)
        {
            RetornoOperacion retorno = new RetornoOperacion();

            //Verificamos el estado de la pagina
            switch ((Pagina.Estatus)Session["estatus"])
            {
              //Si se encuentra en un estatus de Lectura
                case Pagina.Estatus.Edicion:
                    {
                        //Si el radio button rdbDiasSemana se encuentra en un estado de checado
                        if (rdbDiasSemana.Checked == true)
                        {
                            //Insertamos registro a la base de datos con un valor para los dias del mes con 0
                            retorno = SAT_CL.Facturacion.ProcesoDias.InsertaProcesoDias(Convert.ToInt32(Session["id_registro"]), Convert.ToInt32(ddlDiasSemana.SelectedValue), Convert.ToInt32(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario), TimeSpan.Parse(txtHoraInicio.Text), TimeSpan.Parse(txtHoraTermino.Text), 0);
                        }
                        //Si el radio button rdbDiasMes se encuentra en un estado de checado
                        if (rdbDiasMes.Checked == true)
                        {
                            //Insertamos registro a la base de datos con un valor para los dias de la semana con 0
                            retorno = SAT_CL.Facturacion.ProcesoDias.InsertaProcesoDias(Convert.ToInt32(Session["id_registro"]), 0, Convert.ToInt32(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario), TimeSpan.Parse(txtHoraInicio.Text), TimeSpan.Parse(txtHoraTermino.Text), Convert.ToInt32(txtDiaMes.Text));
                        }

                        break;
                    }
                //Si se encuentra en un estatus de Edicion
                case Pagina.Estatus.Lectura:
                    {
                        //Inicializamos la clase Proceso Dias con un Id de registro
                        using (SAT_CL.Facturacion.ProcesoDias pd = new SAT_CL.Facturacion.ProcesoDias((int)gvDiasRevision.SelectedValue))
                        {
                            //Si el radio button rdbDiasSemana se encuentra en un estado de checado
                            if (rdbDiasSemana.Checked == true)
                            {
                                //Editamos registro seleccionado con un valor para los dias del mes con 0
                                retorno = pd.ActualizaRegistroBD(Convert.ToInt32(Session["id_registro"]), Convert.ToInt32(ddlDiasSemana.SelectedValue), Convert.ToInt32(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario), TimeSpan.Parse(txtHoraInicio.Text), TimeSpan.Parse(txtHoraTermino.Text), 0);
                            }

                            if (rdbDiasMes.Checked == true)
                            {
                                //Editamos registro seleccionado con un valor para los dias de la semana con 0
                                retorno = pd.ActualizaRegistroBD(Convert.ToInt32(Session["id_registro"]), 0, Convert.ToInt32(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario), TimeSpan.Parse(txtHoraInicio.Text), TimeSpan.Parse(txtHoraTermino.Text), Convert.ToInt32(txtDiaMes.Text));
                            }
                            break;
                        }
                    }   
            }
            //Si la operacion fue exitosa
            if (retorno.OperacionExitosa)
            {
                //Asigna a la variable estatus el estado de lectura
                Session["estatus"] = Pagina.Estatus.Edicion;
                //Invocamos al metodo inicializa forma
                inicializaForma();
            }

            //Mostramos mensaje con el valor del retorno de la operacion
            TSDK.ASP.ScriptServer.MuestraNotificacion(this, retorno, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        /// <summary>
        /// Evento que ejecuta cuando se da clic al boton cancelar del apartado Dias Revision
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarDiasRevision_Click(object sender, EventArgs e)
        {
            //Verificamos el estado de la pagina
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //Si se encuentra en un estatus de Lectura
                case Pagina.Estatus.Lectura:
                    {
                        //Invocamos al metodo verificaDiaSemanaODiaMes
                       // verificaDiaSemanaODiaMes();
                        //Limpiamos controles del apartado Dias Revision
                        //txtDiaMes.Text = "";
                        //txtHoraInicio.Text = "";
                        //txtHoraTermino.Text = "";
                        Session["estatus"] = Pagina.Estatus.Edicion;
                        break;
                    }
                //Si se encuentra en un estatus de Edicion
                case Pagina.Estatus.Edicion:{
                    //Asignamos a la variable esatus de la sesion el estado de lectura
                    Session["estatus"] = Pagina.Estatus.Edicion;
                        break;
                    }
            }
            //Invocamos al metodo inicializa forma
            inicializaForma();
        }

        #endregion

        #region Eventos Del Grid

        /// <summary>
        /// Evento que ejecuta cuando se da clic al LinkButton editar del Grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEditar_Click(object sender, EventArgs e)
        {
            //Valida que existan registros en el gridView
            if (gvDiasRevision.DataKeys.Count > 0)
            {
                //Selecciona el registro que se va a editar
                Controles.SeleccionaFila(gvDiasRevision, sender, "lnk", false);
                //Inicializamos la clase Proceso Dias con el id del registro que se va a editar
                using (SAT_CL.Facturacion.ProcesoDias pd = new SAT_CL.Facturacion.ProcesoDias((int)gvDiasRevision.SelectedValue))
                {
                    //Carga los controles para su edición
                    ddlDiasSemana.SelectedValue = Convert.ToString(pd.id_dia);
                    //Si el radio Button rdbDiasMes esta en un estado de checado
                    if (rdbDiasMes.Checked == true)
                    {
                        //Cargamos el valor al text box
                        txtDiaMes.Text = Convert.ToString(pd.numero_dias);
                    }

                    txtHoraInicio.Text = Convert.ToString(pd.hora_inicio).Remove(5);
                    txtHoraTermino.Text = Convert.ToString(pd.hora_termino).Remove(5);

                    //A la variable estatus de sesion de se aligana el estado de edicion.
                    Session["estatus"] = Pagina.Estatus.Lectura;

                }
            }
        }

        /// <summary>
        /// Evento que ejecuta cuando se da clic al LinkButton Eliminar del Grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEliminar_Click(object sender, EventArgs e)
        {
            RetornoOperacion retorno = new RetornoOperacion();
            //Valida que existan registros en el gridView
            if (gvDiasRevision.DataKeys.Count > 0)
            {
                //Selecciona el registro que se va a eliminar
                Controles.SeleccionaFila(gvDiasRevision, sender, "lnk", false);

                //Inicializamos la clase Proceso Dias con el id del registro que se va a eliminar
                using (SAT_CL.Facturacion.ProcesoDias pd = new SAT_CL.Facturacion.ProcesoDias((int)gvDiasRevision.SelectedValue))
                {
                    //Eliminamos registro
                    retorno = pd.DeshabilitaProcesoDias(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                }
            }

            //Si la operacion fue exitosa
            if (retorno.OperacionExitosa)
            {
                //A la variable esatus de la sesion se le asigna el estado de Lectura
                Session["estatus"] = Pagina.Estatus.Edicion;
                //Invocamos al metodo inicializaForma
                inicializaForma();
            }

            //Mostramos el mensaje obtenido de la consulata
            TSDK.ASP.ScriptServer.MuestraNotificacion(this, retorno, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        /// <summary>
        /// Evento que ejecuta cuando se selecciona un elemento del dropDowList ddlTamano del grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamano_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando tamaño del GridView
            Controles.CambiaTamañoPaginaGridView(gvDiasRevision, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamano.SelectedValue), true, 1);
        }

        /// <summary>
        /// Evento que ejecuta cuando se selecciona un elemento de Paginacion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDiasRevision_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Muestra los registros que corresponden al indice la pagina seleccionado
            Controles.CambiaIndicePaginaGridView(gvDiasRevision, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, true, 1);
        }

        /// <summary>
        /// Evento que ejecuta cuando se selecciona el encabezado de una columna para ordenamiento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDiasRevision_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Ordena los registros dentro del grid view
            lblOrdenado.Text = Controles.CambiaSortExpressionGridView(gvDiasRevision, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression, true, 1);
        }

        /// <summary>
        /// Evento que ejecuta cuando se da clic el LinkButton exportar del grid 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportar_Click(object sender, EventArgs e)
        {
            //Exporta los datos del grid a una hoja de excel
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "IdProcesoDias");
        }

        #endregion

        #region Metodos Privados

        /// <summary>
        /// Metodo que se encarga de inicializar la forma
        /// </summary>
        private void inicializaForma()
        {
            if (Session["estatus"] == null)
            {
                //A la variable esatus se le asigna el estado de nuevo
                Session["estatus"] = Pagina.Estatus.Nuevo;
            }

            //Asignamos a variable estatus el valor del estatus con el que viene la pagina
            Session["estatus"] = Session["estatus"];

            //Invocamos al metodo carga catalogos
            cargaCatalogos();

            //Invocamos al metodo habilita controles
            habilitaControles();

            //Invocamos al metodo habilita menu
            habilitaMenu();

            //Invocamos al metodo inicializa valores
            inicializaValores();

            //Poner el cursor en el primer control
            txtCliente.Focus();
        }

        /// <summary>
        /// Metodo que se encarga de cargar los catalogos
        /// </summary>
        private void cargaCatalogos()
        {
            //Carga dropDownList con los distintos tipos de procesos
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipoProceso, "", 3140);
            //Carga dropDownList con los dias de la semana
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlDiasSemana, "SELECCIONE UN DIA", 3179);
            //Carga dropDownList con el numero de registros para mostrar
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamano, "", 18);
            //Inicializa Grid View
            TSDK.ASP.Controles.InicializaGridview(gvDiasRevision);
        }

        /// <summary>
        /// Metodo que se encarga de habilitar controles
        /// </summary>
        private void habilitaControles()
        {
            //Obtiene el estado de la pagina
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //Si se encuentra en un estado de nuevo
                case Pagina.Estatus.Nuevo:
                    {
                        txtCliente.Enabled = true;
                        ddlTipoProceso.Enabled = true;
                        txtSecuencia.Enabled = true;
                        txtDescripcion.Enabled = true;
                        txtContacto.Enabled = true;
                        btnGuardar.Enabled = true;
                        btnGuardar.Enabled = true;

                        rdbDiasSemana.Enabled = false;
                        rdbDiasMes.Enabled = false;
                        ddlDiasSemana.Enabled = false;
                        txtDiaMes.Enabled = false;
                        txtHoraInicio.Enabled = false;
                        txtHoraTermino.Enabled = false;
                        btnGuardarDiasRevision.Enabled = false;
                        btnCancelarDiasRevision.Enabled = false;
                        gvDiasRevision.Enabled = false;

                        break;
                    }
                //Si se encuentra en un estado de lectura
                case Pagina.Estatus.Lectura:
                    {
                        txtCliente.Enabled = false;
                        ddlTipoProceso.Enabled = false;
                        txtSecuencia.Enabled = false;
                        txtDescripcion.Enabled = false;
                        txtContacto.Enabled = false;
                        btnGuardar.Enabled = false;
                        btnCancelar.Enabled = false;

                        txtHoraInicio.Enabled = false;
                        txtHoraTermino.Enabled = false;
                        btnGuardarDiasRevision.Enabled = false;
                        btnCancelarDiasRevision.Enabled = false;
                        gvDiasRevision.Enabled = false;
                        rdbDiasSemana.Enabled = false;
                        rdbDiasMes.Enabled = false;
                        ddlDiasSemana.Enabled = false;
                        txtDiaMes.Enabled = false;
                        gvDiasRevision.Enabled = false;
                        break;
                    }
                //Si se encuentra en un estado de edicion
                case Pagina.Estatus.Edicion:
                    {
                        txtCliente.Enabled = true;
                        ddlTipoProceso.Enabled = true;
                        txtSecuencia.Enabled = true;
                        txtDescripcion.Enabled = true;
                        txtContacto.Enabled = true;
                        btnGuardar.Enabled = true;
                        btnCancelar.Enabled = true;

                        rdbDiasSemana.Enabled = true;
                        rdbDiasMes.Enabled = true;
                        ddlDiasSemana.Enabled = true;
                        txtDiaMes.Enabled = true;
                        txtHoraInicio.Enabled = true;
                        txtHoraTermino.Enabled = true;
                        btnGuardarDiasRevision.Enabled = true;
                        btnCancelarDiasRevision.Enabled = true;
                        gvDiasRevision.Enabled = true;

                        break;
                    }
            }
        }

        /// <summary>
        /// Metodo que se encarga de habilitar el menu
        /// </summary>
        private void habilitaMenu()
        {
            //Permite validar cada uno de estatus de una sesión
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //En el caso de que el estatus de la página sea nuevo, habilita las opciones del menú(abrir,guardar,nuevo).
                case Pagina.Estatus.Nuevo:
                    {
                        lkbNuevo.Enabled = true;
                        lkbAbrir.Enabled = true;
                        lkbGuardar.Enabled = true;
                        lkbEliminar.Enabled = false;
                        lkbEditar.Enabled = false;
                        lkbBitacora.Enabled = false;
                        lkbReferencias.Enabled = false;
                        lkbArchivos.Enabled = false;
                        break;
                    }
                //En el caso de que el estatus de la pagina sea lectura, habilita las opciones del ménu(nuevo,abrir,eliminar,editar,bitacora,referencia,Archivo).
                case Pagina.Estatus.Lectura:
                    {
                        lkbNuevo.Enabled = true;
                        lkbAbrir.Enabled = true;
                        lkbGuardar.Enabled = false;
                        lkbEliminar.Enabled = true;
                        lkbEditar.Enabled = true;
                        lkbBitacora.Enabled = true;
                        lkbReferencias.Enabled = true;
                        lkbArchivos.Enabled = true;
                        break;
                    }
                //en el caso de que el estatus de la pagina sea de edicion, habilita las opciones del menú (nuevo,abrir,guardar,eliminar,bitacora,referencia)
                case Pagina.Estatus.Edicion:
                    {
                        lkbNuevo.Enabled = true;
                        lkbAbrir.Enabled = true;
                        lkbGuardar.Enabled = true;
                        lkbEliminar.Enabled = true;
                        lkbEditar.Enabled = false;
                        lkbBitacora.Enabled = true;
                        lkbReferencias.Enabled = true;
                        lkbArchivos.Enabled = false;
                        break;
                    }
            }
        }

        /// <summary>
        /// Metodo que se encarga de inicializar valores de la forma
        /// </summary>
        private void inicializaValores()
        {
            //Obtiene el estado de la pagina
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //Si el estado es de nuevo
                case Pagina.Estatus.Nuevo:
                    {
                        txtCliente.Text = "";
                        txtSecuencia.Text = "";
                        txtDescripcion.Text = "";
                        txtContacto.Text = "";

                        rdbDiasSemana.Checked = true;
                        ddlDiasSemana.SelectedValue = "1";
                        txtDiaMes.Text = "";
                        txtHoraInicio.Text = "";
                        txtHoraTermino.Text = "";
                   
                        cargaDiasRevision();
                        break;
                    }
                //Si el estado es de lectura y edicion
                case Pagina.Estatus.Lectura:
                case Pagina.Estatus.Edicion:
                    {
                        using(SAT_CL.Facturacion.ClienteProceso cp = new SAT_CL.Facturacion.ClienteProceso((int)Session["id_registro"])){
                            txtCliente.Text = cp.nombreCliente.ToString();
                            ddlTipoProceso.SelectedValue = cp.id_tipo_proceso.ToString();
                            txtSecuencia.Text = cp.secuencia.ToString();
                            txtDescripcion.Text = cp.descripcion.ToString();
                            txtContacto.Text = cp.contacto.ToString();
                        }

                        txtDiaMes.Text = "";
                        txtHoraInicio.Text = "";
                        txtHoraTermino.Text = "";
                        if (  Convert.ToInt32(Session["estatus"]) == 3)
                        {
                            verificaDiaSemanaODiaMes();
                           
                        }
                        
                        cargaDiasRevision();

                        break;
                    }
            }
        }

        /// <summary>
        /// Metodo que se encarga de guardar un cliente casignandole un proceso
        /// </summary>
        private void guardaClienteProceso()
        {
            RetornoOperacion retorno = new RetornoOperacion();
            //Obtenemos el estado de la pagina
            switch ((Pagina.Estatus)Session["estatus"])
            {
                    //Si el estado es de nuevo 
                case Pagina.Estatus.Nuevo:
                    {
                        //Insertamos un registro
                        retorno = SAT_CL.Facturacion.ClienteProceso.InsertaClienteProceso(Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCliente.Text, "ID:", 1)), Convert.ToInt32(ddlTipoProceso.SelectedValue), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario, Convert.ToByte(txtSecuencia.Text), txtDescripcion.Text, txtContacto.Text);
                        break;
                    }
                    //Si el estado es de edicion
                case Pagina.Estatus.Edicion:
                    {
                        //Instanciamos clase con el id de registro que se va a editar
                        using (SAT_CL.Facturacion.ClienteProceso cp = new SAT_CL.Facturacion.ClienteProceso((int)Session["id_registro"]))
                        {
                            //Editamos registro
                           retorno =  cp.ActualizaRegistroBD(Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCliente.Text, "ID:", 1)), Convert.ToInt32(ddlTipoProceso.SelectedValue), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario, Convert.ToByte(txtSecuencia.Text), txtDescripcion.Text, txtContacto.Text);
                        }
                        break;
                    }


            }

            if (retorno.OperacionExitosa)
            {
                //Asigna a la variable estatus el estado de lectura
                Session["estatus"] = Pagina.Estatus.Lectura;
                //La variable id_registro se le asigna el id generado en la base de datos
                Session["id_registro"] = retorno.IdRegistro;
                //Invoca al método inicializaForma
                inicializaForma();
            }

            TSDK.ASP.ScriptServer.MuestraNotificacion(this, retorno, ScriptServer.PosicionNotificacion.AbajoDerecha);

        }

        /// <summary>
        /// Metodo que deshabilitar un cliente
        /// </summary>
        private void deshabilitaClienteProceso()
        {
            RetornoOperacion retorno = new RetornoOperacion();
            //Instanciamos la clase con el id del registro que vamos a deshabilitar
            using (SAT_CL.Facturacion.ClienteProceso cp = new SAT_CL.Facturacion.ClienteProceso((int)Session["id_registro"]))
            {
                //Deshabilitampos el registro
                retorno = cp.DeshabilitaClienteProceso(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
            }

            //Si la operacion fue exitosa
            if (retorno.OperacionExitosa)
            {
                //Deshabilitamos registros que contienen los dias de revision ligados al registro del cliente deshabilitado
                SAT_CL.Facturacion.ProcesoDias.DeshabilitaRegistrosLigadosAUnCliente((int)Session["id_registro"], ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                //Asignamos el estado de nuevo a la variable estatus de sesion
                Session["estatus"] = Pagina.Estatus.Nuevo;
                //A la variable de id registro le asignamos el valor de 0
                Session["id_registro"] = 0;
                //Invocamos al metodo inicializa forma
                inicializaForma();
            }

            TSDK.ASP.ScriptServer.MuestraNotificacion(this, retorno, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        /// <summary>
        /// Metodo que inicializa la apertura de un registro
        /// </summary>
        private void inicializaAperturaRegistro(int idTabla, int idCompania)
        {
            //Asigna a la variable de session id_tabla el valor del parametro idTabla
            Session["id_tabla"] = idTabla;
            //Asigna valores a la variable url con la ruta de ubicacion de la ventana Abrir registros con los datos obtenidos de la tabla clientes procesos
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/CuentasCobrar/ClientesProcesoDias.aspx", "~/Accesorios/AbrirRegistro.aspx?P1=" + idCompania.ToString());
            //Define las dimensiones de la ventana Abrir registros de banco
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Invoca al método de la variable ScripServer que abrira una nueva ventana con los registros pertenecientes a la tabla clientes procesos
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "Abrir Registro Clientes Proceso", configuracion, Page);
        }

        /// <summary>
        /// Metodo que inicializa la apertura de un registro
        /// </summary>
        private void inicicalizaBitacora(string idRegistro, string idTabla, string Titulo)
        {
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/CuentasCobrar/ClientesProcesoDias.aspx",
                                                                "~/Accesorios/BitacoraRegistro.aspx?idT=" + idTabla + "&idR=" + idRegistro + "&tB=" + Titulo);
           
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
         
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "Bitacora Clientes Procesos", configuracion, Page);
        }

        /// <summary>
        /// Metodo que inicializa la referencia de un registro
        /// </summary>
        private void inicializaReferenciaRegistro(string id_registro, string id_tabla, string id_compania)
        {
            string urlDestino = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/CuentasCobrar/ClientesProcesoDias.aspx", "~/Accesorios/ReferenciaRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idC=" + id_compania);
          
            TSDK.ASP.ScriptServer.AbreNuevaVentana(urlDestino, "Referencia de Registros Clientes Procesos", 800, 500, false, false, false, true, true, Page);
        }

        /// <summary>
        /// Metodo encargado de verificar de que forma se guardo los dias de revision de un  proceso ya sea por dias de la semana o dias del mes
        /// </summary>
        private void verificaDiaSemanaODiaMes()
        {
            //Invocamos al metodo VerificaDiaSemana
            verificar = SAT_CL.Facturacion.ProcesoDias.VerificaDiaSemanaODiaMes((int)Session["id_registro"]);

            //Si verificar es igual
            if (verificar == 0)
            {
                rdbDiasSemana.Checked = true;
                rdbDiasMes.Checked = false;
                ddlDiasSemana.SelectedValue = "1";
                ddlDiasSemana.Enabled = true;
                txtDiaMes.Enabled = false;

                rdbDiasSemana.Enabled = true;
                rdbDiasMes.Enabled = true;
            }

            if (verificar == 1)
            {

                rdbDiasSemana.Enabled = false;
                rdbDiasMes.Enabled = false;

                rdbDiasMes.Checked = true;
                txtDiaMes.Enabled = true;
                ddlDiasSemana.SelectedValue = "0";

                rdbDiasSemana.Checked = false;
                ddlDiasSemana.Enabled = false;

            }

            if (verificar == 2)
            {
                rdbDiasSemana.Enabled = false;
                rdbDiasMes.Enabled = false;

                rdbDiasMes.Checked = false;
                txtDiaMes.Enabled = false;
                ddlDiasSemana.SelectedValue = "1";

                rdbDiasSemana.Checked = true;
                ddlDiasSemana.Enabled = true;
            }
        }

        /// <summary>
        /// Metodo encargado de cargar al grid registros
        /// </summary>
        private void cargaDiasRevision()
        {
            using (DataTable mit = SAT_CL.Facturacion.ProcesoDias.ObtieneProcesoDias((int)Session["id_registro"]))
            {

                //Validando Origen de Datos
                if (Validacion.ValidaOrigenDatos(mit))
                {
                    //Cargando GridView con Datos
                    Controles.CargaGridView(gvDiasRevision, mit, "IdProcesoDias", "", true, 0);
                    //Guardando Tabla en Session
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], mit, "Table");

                }
                else
                {
                    //Eliminando Tablas del DataSet de Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                    //Cargando GridView Vacio
                    Controles.InicializaGridview(gvDiasRevision);
                }

            }
          

        }

        #endregion

    }
}