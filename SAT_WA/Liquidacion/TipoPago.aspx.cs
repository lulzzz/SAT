using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;


namespace SAT.Liquidacion
{
    /// <summary>
    /// Clase del Formulario TipoPago que administra el comportamiento de los controles en base a eventos
    /// </summary>
    public partial class TipoPago : System.Web.UI.Page
    {
        #region Eventos
      
        /// <summary>
        /// Evento que determina como inicializa la pagina web TipoPago
        /// </summary>
        /// <param name="sender">Almacena la referencia de quien inicio un evento</param>
        /// <param name="e">Contiene información de la clase EventArgs</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Aplicando seguridad de la forma
            SAT_CL.Seguridad.Forma.AplicaSeguridadForma(this, "content1", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
            //Valida si la pagina es cargada por primera vez.
            if (!Page.IsPostBack)
            {
                //Invocal al método inicializaForma
                inicializaForma();
            }

        }

        /// <summary>
        /// Evento que permite almacenar los datos obtenidos de los controles del formulario a la base de datos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            //Invoca al método guardarTipoPago().
            guardarTipoPago();
        }
     
        /// <summary>
        /// Evento que me anula acciones realizadas sobre el formulario (Ingreso, edicion de datos, etc.)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            //Valida cada estado del formulario y en base al estado se ejecutaran instrucciones sobre el formulario.
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //Si el estado de la pagina se encuentra en modo  Nuevo
                case Pagina.Estatus.Nuevo:
                    {
                        //Asigna a la variable session estatus el estado de nuevo.
                        Session["estatus"] = Pagina.Estatus.Nuevo;
                        //Asigna a la variable session id_registro el valor de 0.
                        Session["id_registro"] = 0;
                        //Invoca al método inicializaForma();
                        inicializaForma();
                        break;
                    }
                //Si el estado de la pagina se encuentra en modo de Edición
                case Pagina.Estatus.Edicion:
                    {
                        //Asigna a la variable session estatus el valor actual de la pagina.
                        Session["estatus"] = Pagina.Estatus.Lectura;
                        //Invoca al método inicializaForma()
                        inicializaForma();
                        break;
                    }
            }
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
                        //Invoca al método inicializaForma
                        inicializaForma();
                        //Limpia los mensajes de error del lblError
                        lblError.Text = "";
                        break;
                    }
                //Si la elección del menú es la opcion Abrir
                case "Abrir":
                    {
                        //Invoca al método inicializaAperturaRegistro();
                        inicializaAperturaRegistro(81, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);

                        break;
                    }

                //Si la elección del menú es la opción Guardar
                case "Guardar":
                    {
                        //Invoca al método guardaTipoPago();
                        guardarTipoPago();
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
                        //Invoca al método deshabilitaTipoPago();
                        deshabilitarTipoPago();
                        break;
                    }
                //Si la elección del menú en la opción Bitacora
                case "Bitacora":
                    {
                        //Invoca al método bitacora
                        inicializaBitacora(Session["id_registro"].ToString(), "81", "Tipo Pago");
                        break;
                    }
                //Si la elección del menú en la opcion Referencia
                case "Referencias":
                    {
                        //Invoca al método inicializaReferencia que muestra las observaciones hechas a un registro de TipoPago
                        inicializaReferenciaRegistro(Session["id_registro"].ToString(), "81",
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
        /// Evento que carga los valores del DropDownList UnidadMedidad dependiendo de dropdowList TipoUnidad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTipoUnidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Carga el catalogo de unidad de medida
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlUnidadMedida, 44, "", Convert.ToInt32(ddlTipoUnidad.SelectedValue), "", 0, "");
        }
        
        /// <summary>
        /// Evento que carga los valores del DropDownList TarifaBase dependiendo del DropDownList UnidadMedida
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlUnidadMedida_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Carga catalogo tarifa
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTarifa, 45, "Ninguno", Convert.ToInt32(ddlUnidadMedida.SelectedValue), "", 0, "");
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Método que inicializa los aspectos de la página.
        /// </summary>
        private void inicializaForma()
        {
            //Evalua si la variable session estatus es nulo.
            if (Session["estatus"] == null)
            {
            }

            cargaCatalogos();
            //En caso de que el estado de la pagina no sea nulo (Estatus en modo edicion, lectura, etc.).
            //Invoca al método habilitaControles();
            habilitaControles();
            //Invoca al método habilitaMenú();
            habilitaMenu();
            //Invoca al método inicializaValores();
            inicializaValores();
        }

        /// <summary>
        /// Método que permite la carga de catalosgos en los controles dropdowlist.
        /// </summary>
        private void cargaCatalogos()
        {
            //Invoca al metodo cargaCatalogo para que inicilice los dropdowlist con los catalogos de la base de datos.
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipoUnidad, "", 4);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlAplicacion, "", 96);            
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlUnidadMedida, 44, "Ninguno", Convert.ToInt32(ddlTipoUnidad.SelectedValue), "", 0, "");
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTarifa, 45, "Ninguno", Convert.ToInt32(ddlUnidadMedida.SelectedValue), "", 0, "");
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlConceptoSat, "", 92);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlMoneda, "", 11);
        }

        /// <summary>
        /// Método que permite cambiar el estado (habilitado / deshabiliatado) de los controles acorde al estado de la página.
        /// </summary>
        private void habilitaControles()
        {
            //Evalua cada estatus de la pagina y habilita los controles
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //Si estado de la pagina esta en modo Nuevo o Edicion habilitara los controles
                case Pagina.Estatus.Nuevo:
                case Pagina.Estatus.Edicion:
                    {
                        txtDescripcion.Enabled =
                        ddlAplicacion.Enabled =
                        ddlTipoUnidad.Enabled=
                        ddlUnidadMedida.Enabled =
                        ddlTarifa.Enabled =
                        ddlConceptoSat.Enabled =
                        chkGravado.Enabled =
                        ddlMoneda.Enabled =
                        txtImpuestoTrasladado.Enabled =
                        txtImpuestoRetenido.Enabled =
                        txtCargoAdicional1.Enabled =
                        txtCargoAdicional2.Enabled =
                        btnCancelar.Enabled =
                        btnGuardar.Enabled = true;
                        break;
                    }
                //Si en estado de la pagina esta en modo lectura
                case Pagina.Estatus.Lectura:
                    {
                        txtDescripcion.Enabled =
                        ddlAplicacion.Enabled =
                        ddlTipoUnidad.Enabled=
                        ddlUnidadMedida.Enabled =                        
                        ddlTarifa.Enabled =
                        ddlConceptoSat.Enabled =
                        chkGravado.Enabled =
                        ddlMoneda.Enabled =
                        txtImpuestoTrasladado.Enabled =
                        txtImpuestoRetenido.Enabled =
                        txtCargoAdicional1.Enabled =
                        txtCargoAdicional2.Enabled =
                        btnGuardar.Enabled =
                        btnCancelar.Enabled= false;
                        break;
                    }
            }
        }
       
        /// <summary>
        /// Método que habilita las opciones del menú en base a los estados de la página.
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
                        lkbArchivos.Enabled = false;
                        lkbAcercaDe.Enabled =
                        lkbAyuda.Enabled =
                        btnGuardar.Enabled = true;

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
                        lkbReferencias.Enabled = true;
                        lkbArchivos.Enabled = false;
                        lkbAcercaDe.Enabled = 
                        lkbAyuda.Enabled = true;
                        btnGuardar.Enabled = false;
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
                        lkbArchivos.Enabled = false;
                        lkbAcercaDe.Enabled =
                        lkbAyuda.Enabled = 
                        btnGuardar.Enabled = true;
                        break;
                    }
            }
        }

        /// <summary>
        /// Método que inicializa los valores de los controles acorde a cada estatus de la página.
        /// </summary>
        private void inicializaValores()
        {
            //Evalua cada estatus de la página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //En caso de que el estado de la pagina sea nuevo, los controles no deberan tener texto
                case Pagina.Estatus.Nuevo:
                    {
                        txtDescripcion.Text = "";
                        chkGravado.Checked = false;
                        txtImpuestoTrasladado.Text = "";
                        txtImpuestoRetenido.Text = "";
                        txtCargoAdicional1.Text = "";
                        txtCargoAdicional2.Text = "";
                        lblError.Text = ""; 
                        break;
                    }
                //En caso de que el estado de la página este en  modo de lectura o edición
                case Pagina.Estatus.Lectura:
                case Pagina.Estatus.Edicion:
                    {
                        //Invoca a la clase TipoPago y asigna como parametro al cosntructor la variable de session id_registro
                        using (SAT_CL.Liquidacion.TipoPago tp = new SAT_CL.Liquidacion.TipoPago((int)Session["id_registro"]))
                        {
                            
                            txtDescripcion.Text = tp.descripcion;
                            ddlAplicacion.SelectedValue = tp.id_nivel_aplicacion.ToString();                            
                            //Invoca al constructor de la clase UnidadMedida para cargar los valores a los dropdownlist unidadMedida, y tarifa
                            using (SAT_CL.Global.UnidadMedida um = new SAT_CL.Global.UnidadMedida(tp.id_unidad_medida))
                            {
                               //Valida que el identificador de unidad de medida sea mayor a 0.
                                if (tp.id_unidad_medida > 0)
                                {
                                 //Si cumple con la validación:
                                //Asigna el valor de tipounidadMedida consultado de la base de datos al dropdownlist
                                ddlTipoUnidad.SelectedValue = um.id_tipo_unidad_medida.ToString();
                                //Invoca al catálogo unidad de medida
                                SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlUnidadMedida, 44, "", Convert.ToInt32(ddlTipoUnidad.SelectedValue), "", 0, "");
                                //Carga el valor de catálogo al dropdownlist con el valor almacenado en la base de datos
                                ddlUnidadMedida.SelectedValue = tp.id_unidad_medida.ToString();                                                                 
                                }                               
                            }
                            //Carga los controles con los valores almacenados en la base de datos
                            txtImpuestoTrasladado.Text = tp.tasa_impuesto_trasladado.ToString();
                            txtImpuestoRetenido.Text = tp.tasa_impuesto_retenido.ToString();
                            txtCargoAdicional1.Text = tp.tasa_impuesto1.ToString();
                            txtCargoAdicional2.Text = tp.tasa_impuesto2.ToString();
                            ddlConceptoSat.SelectedValue = tp.id_concepto_sat_nomina.ToString();
                            ddlMoneda.SelectedValue = tp.id_moneda.ToString();
                            chkGravado.Checked = tp.gravado;
                        }
                        break;
                    }
            }
        }

        /// <summary>
        /// Método que almacena los datos obtenidos del formulario tipoPago a la base de datos.
        /// </summary>
        private void guardarTipoPago()
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Valida cada estado del formulario y ejecutara acciones.
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //Si el estado del formualrio es Nuevo
                case Pagina.Estatus.Nuevo:
                    {
                        //Asignación de valores obtenidos de los controles de la pagina al objeto retorno, para su insercion en la base de datos.
                        retorno = SAT_CL.Liquidacion.TipoPago.InsertaTipoPago(txtDescripcion.Text, Convert.ToInt32(ddlUnidadMedida.SelectedValue),
                                                                             Convert.ToDecimal(Cadena.VerificaCadenaVacia(txtImpuestoTrasladado.Text, "0")),
                                                                             Convert.ToDecimal(Cadena.VerificaCadenaVacia(txtImpuestoRetenido.Text, "0")),
                                                                             Convert.ToByte(ddlAplicacion.SelectedValue), Convert.ToByte(ddlMoneda.SelectedValue),
                                                                             ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                                                             Convert.ToDecimal(Cadena.VerificaCadenaVacia(txtCargoAdicional1.Text, "0")),
                                                                             Convert.ToDecimal(Cadena.VerificaCadenaVacia(txtCargoAdicional2.Text, "0")),
                                                                             Convert.ToInt32(ddlTarifa.SelectedValue), Convert.ToByte(ddlConceptoSat.SelectedValue),
                                                                             chkGravado.Checked, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                        break;
                    }
                //Si el estado del formulario es Edición
                case Pagina.Estatus.Edicion:
                    {
                         //Invoca al constructor de la clase TipoPago con el valor de la variable de session id_registro.
                        using (SAT_CL.Liquidacion.TipoPago tp = new SAT_CL.Liquidacion.TipoPago((int)Session["id_registro"]))
                        {
                            //Asignación de valores al objeto retorno, con los datos obtenidos de los controles del formulario TipoPago.
                            retorno = tp.EditaTipoPago(txtDescripcion.Text, Convert.ToInt32(ddlUnidadMedida.SelectedValue), Convert.ToDecimal(txtImpuestoTrasladado.Text.ToString()),
                                          Convert.ToDecimal(txtImpuestoRetenido.Text.ToString()), Convert.ToByte(ddlAplicacion.SelectedValue), Convert.ToByte(ddlMoneda.SelectedValue),
                                          ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, Convert.ToDecimal(txtCargoAdicional1.Text.ToString()),
                                          Convert.ToDecimal(txtCargoAdicional2.Text.ToString()), Convert.ToInt32(ddlTarifa.SelectedValue), Convert.ToByte(ddlConceptoSat.SelectedValue),
                                          chkGravado.Checked, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                        }
                         break;
                    }
            }
            //Valida que la operacion de inserción se realizo correctamente
            if (retorno.OperacionExitosa)
            {
                //Asigna el valor de estatus session en modo lectura.
                Session["estatus"] = Pagina.Estatus.Lectura;
                //Asigna a la variable de session id_registro el valor generado en la base de datos(id).
                Session["id_registro"] = retorno.IdRegistro;
                //Invoca al método inicializaForma.
                inicializaForma();
            }
            //Muestra un mensaje acorde a la validación de la operación.
            lblError.Text = retorno.Mensaje;
        }   

        /// <summary>
        /// Método que modifica el estado de un registro (Habilita/Deshabilita)de la base de datos. 
        /// </summary>
        private void deshabilitarTipoPago()
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Invoca al constructor de la clase TipoPago con el valor de la variable de session id_registro.
            using(SAT_CL.Liquidacion.TipoPago tp = new SAT_CL.Liquidacion.TipoPago((int)Session["id_registro"]))
            {
                //Asignación de valores al objeto retorno con los datos del usuario.
                retorno = tp.DeshabilitaTipoPago(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
            }
            //Valida si la operación se realizo correctamente.
            if(retorno.OperacionExitosa){
                //Asigna el valor de estado lectura a la variable de session estatus 
                Session["estatus"] = Pagina.Estatus.Nuevo;
                //Asigna el valor 0 a la variable de session id_registro
                Session["id_registro"] = 0;
                //invoca al método inicializaForma().
                inicializaForma();
            }
            //Muestra un mensaje acorde a la validación de la operación
            lblError.Text = retorno.Mensaje;
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
            string url = Cadena.RutaRelativaAAbsoluta("~/Liquidacion/TipoPago.aspx",
                                                                "~/Accesorios/BitacoraRegistro.aspx?idT=" + idTabla + "&idR=" + idRegistro + "&tB=" + Titulo);
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
            string url = Cadena.RutaRelativaAAbsoluta("~/Liquidacion/TipoPago.aspx", "~/Accesorios/AbrirRegistro.aspx?P1=" + idCompania.ToString());
            //Define las dimensiones de la ventana Abrir registros de TipoPago
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Invoca al método de la variable ScripServer que abrira una nueva ventana con los registros pertenecientes a la tabla TipoPago
            ScriptServer.AbreNuevaVentana(url, "Abrir Registro Tipo Pago", configuracion, Page);
        }

        /// <summary>
        /// Método encargado de inicializr la referencias a un registro
        /// </summary>
        /// <param name="id_registro">Id que permite identificar un registro de la tabla tipoPago</param>
        /// <param name="id_tabla">Id que permite identificar a la tabla tipoPago en la base de datos</param>
        /// <param name="id_compania">Id que permite identificar a que compania pertenece </param>
        private void inicializaReferenciaRegistro(string id_registro, string id_tabla, string id_compania)
        {
            //Asigna a la variable urlDestino una ruta de ubucación de la ventana de Referencia de la tabla TipoPago
            string urlDestino = Cadena.RutaRelativaAAbsoluta("~/Liquidacion/TipoPago.aspx", "~/Accesorios/ReferenciaRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idC=" + id_compania);
            //Invoca al método de la clase ScripServer que abrira una nueva ventana con los valores de Referencia de la tabla TipoPago
            ScriptServer.AbreNuevaVentana(urlDestino, "Referencia de Registros Tipo Pago", 800, 500, false, false, false, true, true, Page);
        }
        
        #endregion





    }
}