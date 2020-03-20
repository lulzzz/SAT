using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;


namespace SAT.Liquidacion
{
    /// <summary>
    /// Clase del Formulario TipoCobroRecurrente que administra el comportamiento de los controles en base a eventos
    /// </summary>
    public partial class TipoCobroRecurrente : System.Web.UI.Page
    {
        #region Eventos

        /// <summary>
        /// Evento que determina como inicializa la pagina web TipoCobroRecurrente
        /// </summary>
        /// <param name="sender">Almacena la referencia de quien inicio un evento</param>
        /// <param name="e">Contiene información de la clase EventArgs</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Aplicando seguridad de la forma
            SAT_CL.Seguridad.Forma.AplicaSeguridadForma(this, "content1", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
            //Valida si la pagina es cargada por primera vez.
            if (!Page.IsPostBack)
                //Invocal al método inicializaForma
                inicializaForma();
        }

        /// <summary>
        /// Evento que se dispara acorde al estatus de la pagina e invocara a un metodo (almacenar los datos obtenidos de los controles del formulario a la base de datos).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            //Invoca al método guardarTipoCobroRecurrente().
            guardarTipoCobroRecurrente();
        }

        /// <summary>
        /// Evento que se dispara acorde al estado de la página e invoca a un metodo (anula acciones realizadas sobre el formulario (Ingreso, edicion de datos, etc.))
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            //Evalua cada estado de la página.
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //En caso de que la página este en modo nuevo.
                case Pagina.Estatus.Nuevo:
                    {
                        Session["id_registro"] = 0;
                        break;
                    }
                case Pagina.Estatus.Edicion:
                    {
                        //Asigna a la variable de session estatus el valor de Lectura.
                        Session["estatus"] = Pagina.Estatus.Lectura;
                        break;
                    }
            }
            //Invoca al método inicializaForma.
            inicializaForma();
        }

        /// <summary>
        /// Evento que se dispara acorde a la elección de una opcion del ménu principal e invoca a un método
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbElementoMenu_Click(object sender, EventArgs e)
        { 
            //Creación del objeto botonMenú
            LinkButton botonMenu = (LinkButton)sender;
            //Evalúa cada opcion del menú Principal.
            switch (botonMenu.CommandName)
            {
                //En caso de que la opcion del menú sea nuevo
                case "Nuevo":
                    {
                        //Asigna la variable session estatus el valor Nuevo.
                        Session["estatus"] = Pagina.Estatus.Nuevo;
                        //Asigna a la variable de session id_registro el valor de 0.
                        Session["id_registro"] = 0;
                        //Invoca al método inicializaForma.
                        inicializaForma();
                        //Limpia los mensajes del lblError
                        lblError.Text = "";
                        break;
                    }
                //Si la elección del menú es la opcion Abrir
                case "Abrir":
                    {
                        //Invoca al método inicializaAperturaRegistro();
                        inicializaAperturaRegistro(78, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);

                        break;
                    }

                //Si la elección del menú es la opción Guardar
                case "Guardar":
                    {
                        //Invoca al método guardaTipoCobroRecurrente();
                        guardarTipoCobroRecurrente();
                        break;
                    }
                //Si la elección del menú es la opción Editar
                case "Editar":
                    {
                        //Asigna a la variable session estaus el estado de la pagina nuevo
                        Session["estatus"] = Pagina.Estatus.Edicion;
                        //Invoca el método inicializaForma();
                        inicializaForma();
                        //Limpia los mensajes del lblError
                        lblError.Text = "";
                        break;
                    }
                //Si la elección del menú es la opción Eliminar
                case "Eliminar":
                    {
                        //Creación del objeto retorno.
                        RetornoOperacion retorno = new RetornoOperacion();
                        //Invoca al constructor de la clase TipoCobroRecurrente con el valor de la variable de session id_registro.
                        using (SAT_CL.Liquidacion.TipoCobroRecurrente tcr = new SAT_CL.Liquidacion.TipoCobroRecurrente((int)Session["id_registro"]))
                        {
                            //Valida que exista el registro en la base de datos
                            if(tcr.id_tipo_cobro_recurrente > 0)
                            //Asigna a la variable retorno los datos del usuario que realizo el cambio de estado al registro.
                            retorno = tcr.DeshabilitaTipoCobroRecurrente(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                        }
                        //Valida si se realizo correctamente la operación.
                        if (retorno.OperacionExitosa)
                        {
                            //Asigna a la variable sessión estatus el estado de "Nuevo".
                            Session["estatus"] = Pagina.Estatus.Nuevo;
                            //Asigna a la variable session id_registro el valor de 0.
                            Session["id_registro"] = 0;
                            //Invoca al método inicializaForma.
                            inicializaForma();
                        }
                        //Muestra un mensaje acorde a la validación de la operación
                        lblError.Text = retorno.Mensaje;
                        break;
                    }
                //Si la elección del menú en la opción Bitacora
                case "Bitacora":
                    {
                        //Invoca al método bitacora
                        inicializaBitacora(Session["id_registro"].ToString(), "78", "Tipo Cobro Recurrente");
                        break;
                    }
                //Si la elección del menú en la opcion Referencia
                case "Referencias":
                    {
                        //Invoca al método inicializaReferencia que muestra las observaciones hechas a un registro de TipoCobroRecurrente
                        inicializaReferenciaRegistro(Session["id_registro"].ToString(), "78",
                                                    ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString());
                        break;
                    }
                //Si la elección del menú en la opcion Archivo
                case "Archivo":
                    {
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
        /// Evento que carga el catálogo al dropdownlist de ConceptoSat, dependiendo de la opción seleccionada del dropdownlist TipoAplicación 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTipoAplicacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Si la opción del dropdwnlist TipoAplicación es 1 o 2, cargara el catalogo de deduccion
            if (ddlTipoAplicacion.SelectedValue == "1" || ddlTipoAplicacion.SelectedValue == "2")
            {
                //Carga el catalogo de deduccion al dropdownlist Concepto Sat
                SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlConceptoSat, "", 91,Convert.ToInt32(ddlTipoAplicacion.SelectedValue));
                //desmarca la opción del chekcbox Positivo
                chkPositivo.Checked = false;
            }
            //Si la opción del dropdownlist TipoAaplicación es 3 o 4, carga el catalogo de percepción
            if (ddlTipoAplicacion.SelectedValue == "3" || ddlTipoAplicacion.SelectedValue == "4")
            {
                //Carga el catálogo de percepciones al dropdownlist Concepto Sat.
                SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlConceptoSat, "", 92,Convert.ToInt32(ddlTipoAplicacion.SelectedValue));
                //Marca la opción del checkbox Positivo
                chkPositivo.Checked = true;
            }
            //Si la opción del dropdownlist TipoAaplicación es 5 es Otros Pagos
            if (ddlTipoAplicacion.SelectedValue == "5")
            {
                //Carga el catálogo de percepciones al dropdownlist Concepto Sat.
                SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlConceptoSat, "", 3188, Convert.ToInt32(ddlTipoAplicacion.SelectedValue));
                //Marca la opción del checkbox Positivo
                chkPositivo.Checked = true;
            }  
        }
       
        #endregion

        #region Métodos

        /// <summary>
        /// Método que inicializa los aspectos de la página.
        /// </summary>
        private void inicializaForma()
        {
            //Cargar los catalogos de los dropdownList
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipoAplicacion, "", 1098);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlConceptoSat, "", 91, Convert.ToInt32(ddlTipoAplicacion.SelectedValue));
            //Invoca al método habilitaControles();
            habilitaControles();
            //Invoca al método HabilitaMenu();
            habilitaMenu();
            //Invoca al método inicializaValores();
            inicializaValores();
        }

        /// <summary>
        /// Evalúa cada estado de la página y en base al estatus habilita o deshabilita los controles.
        /// </summary>
        private void habilitaControles()
        {
            //Evalua cada estado de la pagina
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //Valida el estado de Nuevo y Edicion; los controles estaran habilitados 
                case Pagina.Estatus.Nuevo:
                case Pagina.Estatus.Edicion:
                    {
                        ddlTipoAplicacion.Enabled =
                        txtDescripcion.Enabled =
                        txtClaveContabilidad.Enabled =
                        ddlConceptoSat.Enabled =
                        chkGravado.Enabled =
                        chkSinTermino.Enabled =
                        btnCancelar.Enabled =
                        btnGuardar.Enabled = true;
                        chkPositivo.Enabled = false;
                        break;
                    }
                //Valida el estado de Lectura; los controles estaran deshabilitados
                case Pagina.Estatus.Lectura:
                    {
                        ddlTipoAplicacion.Enabled =
                        txtDescripcion.Enabled =
                        txtClaveContabilidad.Enabled =
                        ddlConceptoSat.Enabled =
                        chkGravado.Enabled =
                        chkPositivo.Enabled =
                        chkSinTermino.Enabled =
                        btnCancelar.Enabled =
                        btnGuardar.Enabled = false;                        
                        break;
                    }

            }

        }

        /// <summary>
        /// Método que evalúa  cada estado de la página y en base al estatus habilita o deshabilita las opciones del Menu contextual.
        /// </summary>
        private void habilitaMenu()
        {
            //Valida cada estatus de la página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //En caso de que el estado de la página este en modo Nuevo
                case Pagina.Estatus.Nuevo:
                    {
                        lkbAbrir.Enabled =
                        lkbNuevo.Enabled =
                        lkbGuardar.Enabled =true;
                        lkbEditar.Enabled =
                        lkbEliminar.Enabled =
                        lkbReferencias.Enabled =
                        lkbBitacora.Enabled =
                        lkbArchivos.Enabled = false;
                        lkbAcercaDe.Enabled =
                        lkbAyuda.Enabled =
                        btnGuardar.Enabled = true;
                        break;
                    }
                //En caso de que el estado de la página este en modo Lectura
                case Pagina.Estatus.Lectura:
                    {
                        lkbAbrir.Enabled =
                        lkbNuevo.Enabled = true;
                        lkbGuardar.Enabled = false;
                        lkbEditar.Enabled =
                        lkbEliminar.Enabled =
                        lkbReferencias.Enabled =
                        lkbBitacora.Enabled = true;
                        lkbArchivos.Enabled = false;
                        lkbAcercaDe.Enabled = 
                        lkbAyuda.Enabled = true;
                        btnGuardar.Enabled = false;
                        break;
                    }
                //En caso de que el estado de la página este en modo Edición
                case Pagina.Estatus.Edicion:
                    {
                        lkbAbrir.Enabled =
                        lkbNuevo.Enabled = 
                        lkbGuardar.Enabled = true;
                        lkbEditar.Enabled = false;
                        lkbEliminar.Enabled =
                        lkbReferencias.Enabled =
                        lkbBitacora.Enabled = true;
                        lkbArchivos.Enabled = false;
                        lkbAcercaDe.Enabled =
                        lkbAyuda.Enabled = true;
                        btnGuardar.Enabled = true;
                        break;
                    }

            }
        }

        /// <summary>
        /// Método que inicializa los valores de los controles de la página acorde a su estatus.
        /// </summary>
        private void inicializaValores()
        {
            //Evalúa cada estatus de la página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //En caso de que el estado de la página este en modo Nuevo
                case Pagina.Estatus.Nuevo:
                    {
                        //Limpia los controles 
                        txtDescripcion.Text = "";
                        txtClaveContabilidad.Text = "";
                        chkGravado.Checked = false;
                        chkPositivo.Checked = true;
                        chkSinTermino.Checked = false;
                        break;
                    }
                //En caso de que el estado de la página este en modo Lectura o Edición
                case Pagina.Estatus.Lectura:
                case Pagina.Estatus.Edicion:
                    {
                        //Invoca a la clase TipoCobroRecurrente y asigna como parametro al cosntructor la variable de session id_registro
                        using (SAT_CL.Liquidacion.TipoCobroRecurrente tcr = new SAT_CL.Liquidacion.TipoCobroRecurrente((int)Session["id_registro"]))
                        {
                            //Valida que exista el registro en la base de datos
                            if (tcr.id_tipo_cobro_recurrente > 0)
                            {
                                //Carga los controles del formulario con el registro
                                ddlTipoAplicacion.SelectedValue = tcr.id_tipo_aplicacion.ToString();  
                                //Valida la elección del dropdownlist TipoAplicación.              
                                if (ddlTipoAplicacion.SelectedValue == "1" || ddlTipoAplicacion.SelectedValue == "2")
                                {
                                    //Carga el catálogo ConceptoSat Tipo deduccion al dropdownlist
                                    SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlConceptoSat, "", 91, Convert.ToInt32(ddlTipoAplicacion.SelectedValue));
                                    //Seleccióna la opción del dropdownlist ConceptoSat dependiendo del registro obtenido para su edición.
                                    ddlConceptoSat.SelectedValue = tcr.id_concepto_sat_nomina.ToString();
                                }
                                //Valida la elección del dropdownlist TipoAplicacion.
                                if (ddlTipoAplicacion.SelectedValue == "3" || ddlTipoAplicacion.SelectedValue == "4")
                                {
                                    //Carga el catálogo CatalogoSat Tipo persepcion al dropdownlist
                                    SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlConceptoSat, "", 92,Convert.ToInt32(ddlTipoAplicacion.SelectedValue));
                                    //Seleccióna la opción del dropdownlist ConceptoSat dependiendo del registro obtenido para su edición.
                                    ddlConceptoSat.SelectedValue = tcr.id_concepto_sat_nomina.ToString();
                                }
                                //Si la opción del dropdownlist TipoAaplicación es 5 es Otros Pagos
                                if (ddlTipoAplicacion.SelectedValue == "5")
                                {
                                    //Carga el catálogo de percepciones al dropdownlist Concepto Sat.
                                    SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlConceptoSat, "", 3188, Convert.ToInt32(ddlTipoAplicacion.SelectedValue));
                                    //Seleccióna la opción del dropdownlist ConceptoSat dependiendo del registro obtenido para su edición.
                                    ddlConceptoSat.SelectedValue = tcr.id_concepto_sat_nomina.ToString();
                                } 
                                //Carga los controles acorde a los datos obtenidos de la consulta.
                                txtDescripcion.Text = tcr.descripcion;
                                txtClaveContabilidad.Text = tcr.clave_nomina;
                                chkGravado.Checked = tcr.gravado;
                                chkPositivo.Checked = tcr.bit_positivo;
                                chkSinTermino.Checked = tcr.bit_sin_termino;
                            }
                       }
                        break;
                    }
            }
        }

        /// <summary>
        /// Método que permite la inserción de valores de los controles del formulario en la base de datos.
        /// </summary>
        private void guardarTipoCobroRecurrente()
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Valida cada estatus de la pagina.
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //En caso de que la pagina este en modo Nuevo.
                case Pagina.Estatus.Nuevo:
                    {
                        retorno = SAT_CL.Liquidacion.TipoCobroRecurrente.InsertaTipoCobroRecurrente(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                                                                      Convert.ToByte(ddlTipoAplicacion.SelectedValue), txtDescripcion.Text, chkPositivo.Checked,
                                                                                      chkSinTermino.Checked, txtClaveContabilidad.Text, Convert.ToByte(ddlConceptoSat.SelectedValue), chkGravado.Checked,
                                                                                      ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                        break;
                    }
                //En caso de que la página este en modo Edición.
                case Pagina.Estatus.Edicion:
                    {
                        //Invoca al constructor de la Clase TipoCobroRecurrente con el valor de la variable de session id_registro.
                        using (SAT_CL.Liquidacion.TipoCobroRecurrente tcr = new SAT_CL.Liquidacion.TipoCobroRecurrente((int)Session["id_registro"]))
                        {
                            //Valida i existe el registro en la base de datos
                            if (tcr.id_tipo_cobro_recurrente > 0)
                            {
                                retorno = tcr.EditaTipoCobroRecurrente(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                                                        Convert.ToByte(ddlTipoAplicacion.SelectedValue),
                                                                        txtDescripcion.Text, chkPositivo.Checked, chkSinTermino.Checked, txtClaveContabilidad.Text, Convert.ToByte(ddlConceptoSat.SelectedValue),
                                                                        chkGravado.Checked, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                            }
                        }
                        break;
                    }     
            }
            //Valida si la operacion de edicion de registro se realizo correctamente
            if (retorno.OperacionExitosa)
            {
                //Asigna a la variable session estatus el valor de Lectura
                Session["estatus"] = Pagina.Estatus.Lectura;
                //Asigna a la variable de session id_registro el valor generado por la base de datos.
                Session["id_registro"] = retorno.IdRegistro;
                //Invoca al método inicializaForma;
                inicializaForma();
            }
            //Muestra un mensaje acorde a la validación de la operación
            lblError.Text = retorno.Mensaje;          
        }

        /// <summary>
        ///  Método que permie la consulta del historial de modificicaciones realizadas a un registro 
        /// </summary>
        /// <param name="idRegistro">ID que permite identificar un registro de TipoCobroRecurrente</param>
        /// <param name="idTabla">Identificador de la tabla TipoCobroRecurrente</param>
        /// <param name="Titulo">Encabezado de la ventana Bitacora</param>
        private void inicializaBitacora(string idRegistro, string idTabla, string Titulo)
        {
            //Creación de la variable url, que almacena una ruta de ubicación de la ventana, que contendra la bitacora de un registros de  TipoCobroRecurrente.
            string url = Cadena.RutaRelativaAAbsoluta("~/Liquidacion/TipoCobroRecurrente.aspx",
                                                                "~/Accesorios/BitacoraRegistro.aspx?idT=" + idTabla + "&idR=" + idRegistro + "&tB=" + Titulo);
            //Variable que almacena la resolucion de la ventana bitacora
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Invoca el método de la clase ScriptServer que abrira una nueva ventana acorde a una ubicacion, un titulo de la ventana, una configuracion de dimensiones.
            ScriptServer.AbreNuevaVentana(url, "Bitacora Tipo Pago", configuracion, Page);
        }

        /// <summary>
        /// Método que permite la busqueda de registros en la base de datos pertenecientes a la tabla TipoCobroRecurrente
        /// </summary>
        /// <param name="idTabla">Id que identifica a la tabla TipoCobroRecurrente registrada en la base de datos</param>
        /// <param name="idCompania"> Id que identifica a una compañia</param>
        private void inicializaAperturaRegistro(int idTabla, int idCompania)
        {
            //Asigna a la variable de session id_tabla el valor del parametro idTabla
            Session["id_tabla"] = idTabla;
            //Asigna valores a la variable url con la ruta de ubicacion de la ventana Abrir registros con los datos obtenidos de la tabla TipoCobroRecurrente
            string url = Cadena.RutaRelativaAAbsoluta("~/Liquidacion/TipoCobroRecurrente.aspx", "~/Accesorios/AbrirRegistro.aspx?P1=" + idCompania.ToString());
            //Define las dimensiones de la ventana Abrir registros de TipoCobroRecurrente
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Invoca al método de la variable ScripServer que abrira una nueva ventana con los registros pertenecientes a la tabla TipoCobroRecurrente
            ScriptServer.AbreNuevaVentana(url, "Abrir Registro Tipo Cobro Recurrente", configuracion, Page);
        }

        /// <summary>
        /// Método encargado de inicializr la referencias a un registro
        /// </summary>
        /// <param name="id_registro">Id que permite identificar un registro de la tabla TipoCobroRecurrente</param>
        /// <param name="id_tabla">Id que permite identificar a la tabla TipoCobroRecurrente en la base de datos</param>
        /// <param name="id_compania">Id que permite identificar a que compania pertenece </param>
        private void inicializaReferenciaRegistro(string id_registro, string id_tabla, string id_compania)
        {
            //Asigna a la variable urlDestino una ruta de ubucación de la ventana de Referencia de la tabla TipoCobroRecurrente
            string urlDestino = Cadena.RutaRelativaAAbsoluta("~/Liquidacion/TipoCobroRecurernte.aspx", "~/Accesorios/ReferenciaRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idC=" + id_compania);
            //Invoca al método de la clase ScripServer que abrira una nueva ventana con los valores de Referencia de la tabla TipoCobroRecurrente
            ScriptServer.AbreNuevaVentana(urlDestino, "Referencia de Registros Tipo Cobro Recurrente", 800, 500, false, false, false, true, true, Page);
        }


        #endregion

    }
}