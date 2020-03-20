using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;

namespace SAT.ControlPatio
{
    public partial class ZonaPatio : System.Web.UI.Page
    {
        #region Eventos

        /// <summary>
        /// Evento Producido al Efectuarse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {   
            //Aplicando seguridad de la forma
            SAT_CL.Seguridad.Forma.AplicaSeguridadForma(this, "content1", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
          
            //Validando si se Produjo un PostBack
            if(!(Page.IsPostBack))
                //Inicializando Página
                inicializaPagina();
        }
        /// <summary>
        /// Evento producido al Presionar el Menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbElementoMenu_Click(object sender, EventArgs e)
        {   //Validando estatus de Página
            switch (((LinkButton)sender).CommandName)
            {
                case "Nuevo":
                    {   //Asignando estatus nuevo
                        Session["estatus"] = Pagina.Estatus.Nuevo;
                        //Limpiando Id de sesión
                        Session["id_registro"] = 0;
                        //Limpiando Mensaje de Error
                        lblError.Text = "";
                        //Limpiando contenido de forma
                        inicializaPagina();
                        break;
                    }
                case "Abrir":
                    {   //Inicializando Apertura de Registros
                        inicializaAperturaRegistro(87, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);
                        break;
                    }
                case "Guardar":
                    {   //Invocando Método de Guardado
                        guardaZonaPatio();
                        break;
                    }
                case "Editar":
                    {   //Asignando estatus nuevo
                        Session["estatus"] = Pagina.Estatus.Edicion;
                        //Limpiando contenido de forma
                        inicializaPagina();
                        break;
                    }
                case "Eliminar":
                    {   //Instanciando Producto
                        using (SAT_CL.ControlPatio.ZonaPatio zp = new SAT_CL.ControlPatio.ZonaPatio(Convert.ToInt32(Session["id_registro"])))
                        {   //Validando que exista un Producto
                            if (zp.id_zona_patio > 0)
                            {   //Declarando Objeto de Retorno
                                RetornoOperacion result = new RetornoOperacion();
                                //Deshabilitando Zona
                                result = zp.DeshabilitaZonaPatio(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                //Validando que la Operación sea exitosa
                                if (result.OperacionExitosa)
                                {   //Limpiando registro de Session
                                    Session["id_registro"] = 0;
                                    //Cambiando a Estatus "Nuevo"
                                    Session["estatus"] = Pagina.Estatus.Nuevo;
                                    //Inicializando Forma
                                    inicializaPagina();
                                }//Mostrando Mensaje de Operación
                                lblError.Text = result.Mensaje;
                            }
                        }
                        break;
                    }
                case "Bitacora":
                    {   //Inicializando Bitacora
                        inicializaBitacora(Session["id_registro"].ToString(), "87", "Compania");
                        break;
                    }
                case "Referencias":
                    {   //Invocando Método de Inicialización de Referencias
                        inicializaReferenciaRegistro(Session["id_registro"].ToString(), "87", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString());
                        break;
                    }
                case "Archivos":
                    {   //TODO: Implementar uso de archivos ligados a registro
                        inicializaImagenes(Convert.ToInt32(Session["id_registro"]), 87, "Zona Patio", 9);
                        break;
                    }
                case "Acerca":
                    //TODO: Implementar uso de acerca de
                    break;
                case "Ayuda":
                    //TODO: Implementar uso de ayuda
                    break;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlPatio_SelectedIndexChanged(object sender, EventArgs e)
        {   //Cargando Zonas de Patio
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlZonaSuperior, 37, "Ninguno", Convert.ToInt32(ddlPatio.SelectedValue), "", 0, "");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {   //Invocando Método de Guardado
            guardaZonaPatio();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {   //Validando Estatus de Página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Edicion:
                    {   //Asignando Estatus a Lectura
                        Session["estatus"] = Pagina.Estatus.Lectura;
                        break;
                    }
            }
            //Invocando Método de Inicialización
            inicializaPagina();
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Método Privado encargado de Inicializar los Controles de la Página
        /// </summary>
        private void inicializaPagina()
        {   //Cargando Catalogos
            cargaCatalogos();
            //Habilitando Menu
            habilitaMenu();
            //Habilitando Controles
            habilitaControles();
            //Inicializando Valores
            inicializaValores();
        }
        /// <summary>
        /// Método Privado encargado de Cargar los Catalogos de la Forma
        /// </summary>
        private void cargaCatalogos()
        {   //Cargando los Patios por Compania
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlPatio, 34, "Ninguno", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario, "", 0, "");
            //Cargando Zonas de Patio
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlZonaSuperior, 37, "Ninguno", Convert.ToInt32(ddlPatio.SelectedValue), "", 0, "");
            //Cargando Tipos de Zona
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipoZona, "", 70);
        }
        /// <summary>
        /// Método Privado encargado de Habilitar el Menu
        /// </summary>
        private void habilitaMenu()
        {   //Validando Estatus de Session
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {   //Archivo
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
                    {   //Archivo
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
                    {   //Archivo
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
        /// Método Privado encargado de Habilitar los Controles
        /// </summary>
        private void habilitaControles()
        {   //Validando Estatus de Página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Lectura:
                    {   //Deshabilitando Valores
                        txtDescripcion.Enabled =
                        txtColor.Enabled =
                        ddlPatio.Enabled =
                        ddlZonaSuperior.Enabled =
                        ddlTipoZona.Enabled =
                        btnGuardar.Enabled =
                        btnCancelar.Enabled = false;
                        break;
                    }
                case Pagina.Estatus.Nuevo:

                case Pagina.Estatus.Edicion:
                    {   //Deshabilitando Valores
                        txtDescripcion.Enabled =
                        txtColor.Enabled =
                        ddlPatio.Enabled =
                        ddlZonaSuperior.Enabled =
                        ddlTipoZona.Enabled =
                        btnGuardar.Enabled =
                        btnCancelar.Enabled = true;
                        break;
                    }
            }
        }
        /// <summary>
        /// Método Privado encargado de Inicializar los Valores de los Controles
        /// </summary>
        private void inicializaValores()
        {   //Validando Estatus de Página
            switch((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {   //Limpiando Controles
                        lblId.Text = "Por Asignar";
                        txtDescripcion.Text =
                        txtColor.Text = "";
                        break;
                    }
                case Pagina.Estatus.Lectura:
                    
                case Pagina.Estatus.Edicion:
                    {   //Instanciando Zona de Patio
                        using (SAT_CL.ControlPatio.ZonaPatio zp = new SAT_CL.ControlPatio.ZonaPatio(Convert.ToInt32(Session["id_registro"])))
                        {   //Validando que exista el registro
                            if (zp.id_zona_patio > 0)
                            {   //Asignando Valores
                                lblId.Text = zp.id_zona_patio.ToString();
                                txtDescripcion.Text = zp.descripcion;
                                txtColor.Text = zp.color_hxd;
                                ddlPatio.SelectedValue = zp.id_ubicacion_patio.ToString();
                                ddlZonaSuperior.SelectedValue = zp.id_zona_superior.ToString();
                                ddlTipoZona.SelectedValue = zp.id_tipo_zona.ToString();
                            }
                        }
                        break;
                    }
            }
        }
        /// <summary>
        /// Método Privado encargado de Guardar los Cambios a las Zonas de Patio
        /// </summary>
        private void guardaZonaPatio()
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Validando Estatus de Página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {   //Insertando Zona de Patio
                        result = SAT_CL.ControlPatio.ZonaPatio.InsertaZonaPatio(Convert.ToInt32(ddlPatio.SelectedValue),
                                    Convert.ToInt32(ddlZonaSuperior.SelectedValue), txtDescripcion.Text,
                                    (SAT_CL.ControlPatio.ZonaPatio.TipoZona)Convert.ToInt32(ddlTipoZona.SelectedValue), new Microsoft.SqlServer.Types.SqlGeography(),
                                    txtColor.Text, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                        break;
                    }
                case Pagina.Estatus.Edicion:
                    {   //Instanciando Zona de Patio
                        using (SAT_CL.ControlPatio.ZonaPatio zp = new SAT_CL.ControlPatio.ZonaPatio(Convert.ToInt32(Session["id_registro"])))
                        {   //Validando que exista el registro
                            if (zp.id_zona_patio > 0)
                            {   //Editando Zona de Patio
                                result = zp.EditaZonaPatio(Convert.ToInt32(ddlPatio.SelectedValue),
                                                Convert.ToInt32(ddlZonaSuperior.SelectedValue), txtDescripcion.Text,
                                                (SAT_CL.ControlPatio.ZonaPatio.TipoZona)Convert.ToInt32(ddlTipoZona.SelectedValue), new Microsoft.SqlServer.Types.SqlGeography(),
                                                txtColor.Text, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                            }
                        }

                        break;
                    }
            }
            //Validando si la Operación fue exitosa
            if(result.OperacionExitosa)
            {   //Asignando variable de Session
                Session["id_registro"] = result.IdRegistro;
                Session["estatus"] = Pagina.Estatus.Lectura;
                //Inicializando Página
                inicializaPagina();
            }
            //Mostrando Mensaje de Operación
            lblError.Text = result.Mensaje;
        }
        /// <summary>
        /// Método Privado encargado de Inicializar la Apertura de un Registro
        /// </summary>
        /// <param name="idTabla">Id de Tabla</param>
        private void inicializaAperturaRegistro(int idTabla, int idCompania)
        {   //Asignando Session
            Session["id_tabla"] = idTabla;
            //Construyendo URL 
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/ControlPatio/ZonaPatio.aspx", "~/Accesorios/AbrirRegistro.aspx?P1=" + idCompania.ToString());
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=600";
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
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/ControlPatio/ZonaPatio.aspx", "~/Accesorios/BitacoraRegistro.aspx?idT=" + idTabla + "&idR=" + idRegistro + "&tB=" + Titulo);
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=600";
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
            string urlDestino = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/ControlPatio/ZonaPatio.aspx", "~/Accesorios/ReferenciaRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idC=" + id_compania);
            //Instanciando nueva ventana de navegador para apertura de referencias de registro
            TSDK.ASP.ScriptServer.AbreNuevaVentana(urlDestino, "Referencia", 800, 650, false, false, false, true, true, Page);
        }
        /// <summary>
        /// Configura y muestra ventana de referencias de registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="nombre_tabla">Nombre de la Tabla</param>
        /// <param name="id_configuracion_tipo_archivo">Id de Configuración de tipo de archivo s seleccionar</param>
        private void inicializaImagenes(int id_registro, int id_tabla, string nombre_tabla, int id_configuracion_tipo_archivo)
        {
            //Construyendo URL 
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/ControlPatio/ZonaPatio.aspx", "~/Accesorios/ArchivoRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idC=" + ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString() + "&idTV=" + id_configuracion_tipo_archivo + "&tB=" + nombre_tabla);
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=600";
            //Abriendo Nueva Ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "Imagenes", configuracion, Page);
        }

        #endregion
    }
}