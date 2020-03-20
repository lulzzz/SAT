using SAT_CL.EgresoServicio;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;

namespace SAT.EgresoServicio
{
    /// <summary>
    /// Clase de la pagina web PrecioCombustible, que administraa el comportamiento de los controles en base a eventos
    /// </summary>
    public partial class PrecioCombustible : System.Web.UI.Page
    {
        #region Eventos

        /// <summary>
        /// Evento que determina como inicializa la pagina web PrecioCombustible
        /// </summary>
        /// <param name="sender">Almacena la referencia de quien inicio un evento</param>
        /// <param name="e">Contiene información de la clase EventArgs</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Aplicando seguridad de la forma
            SAT_CL.Seguridad.Forma.AplicaSeguridadForma(this, "content1", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
            
            //Valida si la pagina es cargada por primera vez.
            if (!Page.IsPostBack)            
                //Invoca al método inicializa forma
                inicializaForma();        
   
            //Invocando Método de Autocompletado
            cargaCatalogoAutocompleta();
        }
        /// <summary>
        /// Evento que se ejecutara cuando se da clic en el botón Cancelar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancela_Click(object sender, EventArgs e)
        {
            //Valida cada estado de la página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //Válida cada estatus de la pagina
                case Pagina.Estatus.Nuevo:
                    {
                        //Se le asigna a la variable session estatus el estado de la pagina en modo de nuevo
                        Session["estatus"] = Pagina.Estatus.Nuevo;
                        //A la variable de session id_registro se le asigna el valor de 0
                        Session["id_registro"] = 0;
                        break;
                    }
                case Pagina.Estatus.Edicion:
                    {
                        //Se le asigna a la variable session estatus el estado de la pagina en modo lectura
                        Session["estatus"] = Pagina.Estatus.Lectura;
                        break;
                    }
            }
            //Invoca al método incializaForma
            inicializaForma();
        }

        /// <summary>
        /// Evento que se ejecutara cuando se da clic en el botón Guardar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            //Valida cada estatus de la pagina
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //En caso de que la pagina este en modo de nuevo
                case Pagina.Estatus.Nuevo:
                    {
                        //Invoca al método guardarPrecioCombustible
                        guardarPrecioCombustible();
                        break;
                    }
                //En caso de que el estado de la página este en modo edición
                case Pagina.Estatus.Edicion:
                    {
                        //invoca al método editarPrecioCombustible
                        editarPrecioCombustible();
                        break;
                    }
            }
        }
        /// <summary>
        /// Método que permite seleccionar una opción del ménu principal y en vase a ello se realizaran acciones. 
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
                        //Inicializa el estado de la forma.
                        inicializaForma();
                        //El label de error se mostrara sin caracteres ya que el estado es nuevo
                        lblError.Text = "";
                        break;
                    }

                case "Abrir":
                    {
                        //Invoca al método inicializaApertura para inicializar la apertura de un registro de Precio Combustible
                        inicializaAperturaRegistro(58, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);
                        break;
                    }

                case "Guardar":
                    {
                        guardarPrecioCombustible();
                        break;
                    }
                //En caso de seleccionar la opción Editar del menú.
                case "Editar":
                    {
                        //Se asigna a la sesion el estado de edición
                        Session["estatus"] = Pagina.Estatus.Edicion;
                        //Invoca al metodo inicializaFormana
                        inicializaForma();
                        //El lbel cualdo este en modo de edicion debera limpiarse
                        lblError.Text = "";
                        break;
                    }
                //En caso de seleccionar la opción Eliminar del menú.
                case "Eliminar":
                    {
                        //Creación del objeto retorno
                        RetornoOperacion retorno = new RetornoOperacion();
                        //Invoca a la clase Costo combustible para obtener el registro insertado en la base de datos.
                        using (CostoCombustible cc = new CostoCombustible((int)Session["id_registro"]))
                        {
                            if (cc.id_tipo_combustible > 0)
                            //Asignación de valores al objeto retorno, con los datos del usuario que realizo el cambio de estatus de un registro (habilitado/deshabilitado) de CostoCombustible
                            retorno = cc.DeshabilitaCostoCombustible(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                        }
                        //Validación de que el cambio de estado del registro (habilitado/deshabilitado) de CostoCombustible se realizo o no
                        if (retorno.OperacionExitosa)
                        {
                            //Asignacion a la variable de session estatus el valor del estado de la pagina en modo de nuevo
                            Session["estatus"] = Pagina.Estatus.Nuevo;
                            //Asignacion a la variable de session id_registro el valor de 0
                            Session["id_registro"] = 0;
                            //Invoca al método inicializaForma
                            inicializaForma();
                        }
                        //Muestra un mensaje de validacion de la operación
                        lblError.Text = retorno.Mensaje;
                        break;
                    }
                case "Bitacora":
                    {
                        //Invoca al método inicializaBitacora que muestra las modificaciones hechas sobre un registro de Precio Combustible
                        inicializaBitacora(Session["id_registro"].ToString(), "58", "CostoCombustible");
                        break;
                    }

                case "Referencias":
                    {
                        //Invoca al método inicializaReferencia que muestra las observaciones hechas a un registro de Precio Combustible
                        inicializaReferenciaRegistro(Session["id_registro"].ToString(), "58",
                                                    ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString());
                        break;
                    }
                case "Archivo":
                    {
                        break;
                    }
                case "Acerca":
                    {
                        break;
                    }
                case "Ayuda":
                    {
                        break;
                    }
            }
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método que inicializa los aspectos de la página
        /// </summary>
        private void inicializaForma()
        {
            //Asigna a la variable estatus el valor actual de la pagina
            Session["estatus"] = Session["estatus"];
            //Invoca al método cargaCatalogo
            cargaCatalogo();
            //Invoca al metodo habilitaControles
            habilitaControles();
            //Invoca al método habilitaMenu
            habilitaMenu();
            //Invoca al método inicializaValores
            inicializaValores();
        }
        /// <summary>
        /// Método encargado de Cargar los Catalogos de la Forma
        /// </summary>
        private void cargaCatalogo()
        {
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipoCombustible, "", 49);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTabla, 105, "");
        }
        /// <summary>
        /// Método que permite cambiar el estado (habilitado / deshabiliatado) de los controles acorde al estado de la página
        /// </summary>
        private void habilitaControles()
        {
            //Válida cada estatus de la página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //Si el estado de la página es nuevo o en edicion habilita los controles
                case Pagina.Estatus.Nuevo:
                case Pagina.Estatus.Edicion:
                    {
                        txtFechaInicio.Enabled =
                        txtFechaFin.Enabled =
                        ddlTabla.Enabled =
                        txtRegistro.Enabled =
                        txtPrecioCombustible.Enabled =
                        txtReferencia.Enabled =
                        btnCancelar.Enabled=
                        btnGuardar.Enabled=
                        ddlTipoCombustible.Enabled = true;
                        break;
                    }
                //En caso de que el estado de la pagina sea de lectura deshabilita los controles
                case Pagina.Estatus.Lectura:
                    {
                        txtFechaInicio.Enabled =
                        txtFechaFin.Enabled =
                        ddlTabla.Enabled =
                        txtRegistro.Enabled =
                        txtPrecioCombustible.Enabled =
                        txtReferencia.Enabled =
                        btnCancelar.Enabled=
                        btnGuardar.Enabled=
                        ddlTipoCombustible.Enabled = false;
                        break;
                    }
            }
        }

        /// <summary>
        /// Método que permite habilitar las opciones del menu contextual acorde al estado de la página
        /// </summary>
        private void habilitaMenu()
        {
            //Valida cada estatus ende la pagina y acorde a ello hibilitara las opciones del menú
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //Cuando el estatus de la página es nuevo,habilita las opciones del menú nuevo,abrir,guardar,acercade,ayuda y el boton guarda.
                case Pagina.Estatus.Nuevo:
                    {
                        lkbNuevo.Enabled = 
                        lkbAbrir.Enabled = 
                        lkbGuardar.Enabled = true;
                        lkbEliminar.Enabled =
                        lkbEditar.Enabled =
                        lkbBitacora.Enabled =
                        lkbReferencias.Enabled =
                        lkbArchivos.Enabled = false;
                        lkbAcercaDe.Enabled =
                        lkbAyuda.Enabled =
                        btnGuardar.Enabled = true;
                        break;
                    }
                //Cuando el estatus de la página esta en modo lectura, habilita las opciones del menú nuevo,abrir,eliminar,editar,bitacora,referencia,acercade y ayuda
                case Pagina.Estatus.Lectura:
                    {
                        lkbNuevo.Enabled =
                        lkbAbrir.Enabled = true;
                        lkbGuardar.Enabled = false;
                        lkbEliminar.Enabled =
                        lkbEditar.Enabled =
                        lkbBitacora.Enabled =
                        lkbReferencias.Enabled = 
                        lkbArchivos.Enabled = 
                        lkbAcercaDe.Enabled =
                        lkbAyuda.Enabled = true;
                        btnGuardar.Enabled = false;
                        break;
                    }
                //Cuando el estatus de la página esta en modo edicion
                case Pagina.Estatus.Edicion:
                    {
                        lkbNuevo.Enabled =
                        lkbAbrir.Enabled =
                        lkbGuardar.Enabled =
                        lkbEliminar.Enabled = true;
                        lkbEditar.Enabled = false;
                        lkbBitacora.Enabled =
                        lkbReferencias.Enabled = true;
                        lkbArchivos.Enabled = false;
                        lkbAcercaDe.Enabled =
                        lkbAyuda.Enabled =
                        btnGuardar.Enabled = true;
                        break;
                    }
            }
        }
        /// <summary>
        /// Método que inicializa los valores de los controles acorde a cada estatus de la página
        /// </summary>
        private void inicializaValores()
        {
            //Evalua cada estatus de la pagina 
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //En caso de que la página este en modo de nuevo
                case Pagina.Estatus.Nuevo:
                    {
                        txtFechaInicio.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm"); 
                        txtFechaFin.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");
                        txtRegistro.Text = "";
                        txtPrecioCombustible.Text = "";
                        txtReferencia.Text = "";
                        lblError.Text = "";
                        break;
                    }
                 //En caso de que la página este en modo de lectura o edicion
                case Pagina.Estatus.Lectura:
                case Pagina.Estatus.Edicion:
                    {
                        //Creación del objeto que invoca a la clase Precio Combustible para obtener el registro insertado en la base de datos
                        using(CostoCombustible cc = new CostoCombustible((int)Session["id_registro"]))
                        {
                            if(cc.id_tipo_combustible > 0)
                            {
                                //Validando tabla
                                switch (cc.id_tabla)
                                {
                                    case 15:
                                        {
                                            using (SAT_CL.Global.Ubicacion ubi = new SAT_CL.Global.Ubicacion(cc.id_registro))
                                            {
                                                txtRegistro.Text = string.Format("{0} ID:{1}", ubi.descripcion, ubi.id_ubicacion);

                                            }
                                          break;
                                        }
                                }


                                txtFechaInicio.Text = cc.fecha_inicio.ToString("dd/MM/yyyy HH:mm");
                                txtFechaFin.Text = cc.fecha_fin.ToString("dd/MM/yyyy HH:mm");
                                ddlTabla.SelectedValue = cc.id_tabla.ToString();
                                txtPrecioCombustible.Text =cc.costo_combustible.ToString();
                                txtReferencia.Text = cc.referencia;
                                ddlTipoCombustible.SelectedValue = cc.id_tipo_combustible.ToString();
                            }
                        }
                        break;
                    }
            }
        }
        /// <summary>
        /// Método encargado de Cargar el Catalogo Autocompleta
        /// </summary>
        private void cargaCatalogoAutocompleta()
        {
            //Obteniendo Compania
            string idCompania = ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString();

            //Declarando Script de Carga
            string script = @"<script>
                                //Serializando Control
                                $('#" + this.ddlTabla.ClientID + @"').serialize();

                                //Obteniendo Tipo de Entidad
                                var tipoEntidad = " + this.ddlTabla.SelectedValue + @";

                                //Validando Tipo de Entidad
                                switch (tipoEntidad) {
                                    case 15:
                                        {   
                                            //Cargando Catalogo AutoCompleta
                                            $('#" + txtRegistro.ClientID + @"').autocomplete({
                                                source:'../WebHandlers/AutoCompleta.ashx?id=2&param=" + idCompania + @"'
                                            });
                                            
                                            break;
                                        }
                                }
                                
                              </script>";

            //Registrando Script
            ScriptManager.RegisterStartupScript(this, this.GetType(), "CargaAutocompletaEntidad", script, false);
        }
        /// <summary>
        /// Método que permite la insercion de los valores a la tabla CostoCombustible obtenidos de los controles de la pagina PrecioCombustible
        /// </summary>
        private void guardarPrecioCombustible()
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Invoca al método validaFecha  y asigna el resultado del método al objeto retorno.
            retorno = validaFechas();
            //Valida si el resultado del método se realizo correctamente (La validación de las Fechas)
            if (retorno.OperacionExitosa)
            {
                //Asignación de valores al objeto retorno, con los valores obtenidos del formaulario de la pagina PrecioCombustible
                retorno = CostoCombustible.InsertaCostoCombustible(Convert.ToByte(ddlTipoCombustible.SelectedValue), Convert.ToInt32(ddlTabla.SelectedValue), 
                                                                   Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtRegistro.Text, "ID:", 1)),
                                                                  Convert.ToDateTime(txtFechaInicio.Text), Convert.ToDateTime(txtFechaFin.Text),
                                                                  Convert.ToDecimal(txtPrecioCombustible.Text), txtReferencia.Text.ToUpper(), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                //Valida si la inserción a la base de datos se realizo correctamente
                if (retorno.OperacionExitosa)
                {
                    //A la variable de sessión estatus le asigna el estado de la pagina en modo lectura
                    Session["estatus"] = Pagina.Estatus.Lectura;
                    //A la variable de session id_registro le asigna el valor insertado en la tabla CostoCombustible
                    Session["id_registro"] = retorno.IdRegistro;
                    //Invoca al método inicializa forma
                    inicializaForma();
                }
            }
            //Manda un mensaje dependiendo de la validación de la operación
            lblError.Text = retorno.Mensaje;  
        }

        /// <summary>
        /// Método que permite la actualizacion de los compos de la tabla Costo combustible, con los datos obtenidos del formulario PrecioCombustible
        /// </summary>
        private void editarPrecioCombustible()
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Invoca a la clase CostoCombustible para inicializar la busqueda de registros y poderlos editar
            using (CostoCombustible cc = new CostoCombustible((int)Session["id_registro"]))
            {
                if (cc.id_costo_combustible > 0)
                {
                    //Asignación de valores obtenidos del fromulario PrecioCombustible al objeto retorno, para la actualizacion del registro de la tabla CostoCombustible
                    retorno = cc.EditaCostoCombustible(Convert.ToByte(ddlTipoCombustible.SelectedValue), Convert.ToInt32(ddlTabla.SelectedValue), 
                                                       Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtRegistro.Text, "ID:", 1)), Convert.ToDateTime(txtFechaInicio.Text), 
                                                       Convert.ToDateTime(txtFechaFin.Text), Convert.ToDecimal(txtPrecioCombustible.Text), txtReferencia.Text.ToUpper(), 
                                                       ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                }
            }
            //Validación de la actualizacion de datos sobre el registro
            if (retorno.OperacionExitosa)
            {
                //Asignación a la variable de session estatus del valor del estado de la pagina en modo lectura.
                Session["estatus"] = Pagina.Estatus.Lectura;
                //Asignación a la variable de session id_registro  el valor del identificador generado en la base de datos, en la tabla CostoCombustible;
                Session["id_registro"] = retorno.IdRegistro;
                //Invoca al método inicializaForma
                inicializaForma();
            }
            //Muestra un mensaje validando si se realizo la operación correctamente o no.
            lblError.Text = retorno.Mensaje;
        }

        /// <summary>
        /// Método que valida la fecha de inicio sea menor a la Fecha Fin
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion validaFechas()
        {
            //Creación del objeto retorno con valor 1 al constructor de la clase
            RetornoOperacion retorno = new RetornoOperacion(1);
            //Compara los datos encontrados en los controles de fecha inicio y fecha fin(si la fechaInicio es menor a fechaFin y el resultado de la comparacion es a 0)
            if(Convert.ToDateTime(txtFechaInicio.Text).CompareTo(Convert.ToDateTime(txtFechaFin.Text))>0)
            {
                //  Al objeto retorno se le asigna un mensaje de error en la validación de las fechas.
                retorno = new RetornoOperacion(" Fecha Inicio debe ser MENOR que Fecha Fin.");
            }
            //Retorna el resultado al método 
            return retorno;
        }


        /// <summary>
        ///  Método que permie la consulta del historial de modificicaciones realizadas a un registro 
        /// </summary>
        /// <param name="idRegistro">ID que permite identificar un registro de CostoCombustible</param>
        /// <param name="idTabla">Identificador de la tabla CostoCombustible</param>
        /// <param name="Titulo">Encabezado de la ventana Bitacora</param>
        private void inicializaBitacora(string idRegistro, string idTabla, string Titulo)
        {
            //Creación de la variable url, que almacena una ruta de ubicación de la ventana, que contendra la bitacora de un registros de  Precio Combustible.
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/EgresoServicio/PrecioCombustible.aspx",
                                                                "~/Accesorios/BitacoraRegistro.aspx?idT=" + idTabla + "&idR=" + idRegistro + "&tB=" + Titulo);
           //Variable que almacena la resolucion de la ventana bitacora
           string configuracion= "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Invoca el método de la clase ScriptServer que abrira una nueva ventana acorde a una ubicacion, un titulo de la ventana, una configuracion de dimenciones.
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url,"Bitacora Precio Combustible",configuracion,Page);
        }

        /// <summary>
        /// Método que permite la busqueda de registros en la base de datos pertenecientes a la tabla Precio Combustible 
        /// </summary>
        /// <param name="idTabla">Id que identifica a la tabla Precio Combustible registrada en la base de datos</param>
        /// <param name="idCompania"> Id que identifica a una compañia</param>
        private void inicializaAperturaRegistro(int idTabla, int idCompania)
        {
            //Asigna a la variable de session id_tabla el valor del parametro idTabla
            Session["id_tabla"] = idTabla;
            //Asigna valores a la variable url con la ruta de ubicacion de la ventana Abrir registros con los datos obtenidos de la tabla Precio Combustible
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/EgresoServicio/PrecioCombustible.aspx", "~/Accesorios/AbrirRegistro.aspx?P1=" + idCompania.ToString());
            //Define las dimensiones de la ventana Abrir registros de Precio Combustible
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Invoca al método de la variable ScripServer que abrira una nueva ventana con los registros pertenecientes a la tabla Precio Combustible
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "Abrir Registro Precio Combustible", configuracion, Page);
        }

        /// <summary>
        /// Método encargado de inicializr la referencias a un registro
        /// </summary>
        /// <param name="id_registro">Id que permite identificar un registro de la tabla Precio Combustible</param>
        /// <param name="id_tabla">Id que permite identificar a la tabla Precio Combustible en la base de datos</param>
        /// <param name="id_compania">Id que permite identificar a que compania pertenece </param>
        private void inicializaReferenciaRegistro(string id_registro, string id_tabla, string id_compania)
        {
            //Asigna a la variable urlDestino una ruta de ubucación de la ventana de Referencia de la tabla Precio Combustible
            string urlDestino = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/EgresoServicio/PrecioCombustible.aspx", "~/Accesorios/ReferenciaRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idC=" + id_compania);
            //Invoca al método de la clase ScripServer que abrira una nueva ventana con los valores de Referencia de la tabla Precio Combustible
            TSDK.ASP.ScriptServer.AbreNuevaVentana(urlDestino, "Referencia de Registros Precio Combustible", 800, 500, false, false, false, true, true, Page);
        }
        #endregion



    }
}