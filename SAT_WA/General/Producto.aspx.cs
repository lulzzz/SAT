using SAT_CL;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;

namespace SAT.General
{
    public partial class Producto : System.Web.UI.Page
    {
        #region Eventos

        /// <summary>
        /// Evento disparado al producirse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Aplicando seguridad de la forma
            SAT_CL.Seguridad.Forma.AplicaSeguridadForma(this, "content1", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
            //Validando que se produjo un PostBack
            if(!(Page.IsPostBack))

                //Inicializando Página
                inicializaForma();
        }
        /// <summary>
        /// Evento disparado al Presionar el Boton "Guardar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {   
            //Invocando Método de Guardado
            guardaProducto();
        }
        /// <summary>
        /// Evento disparado al Presionar el Boton "Cancelar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {   
            //Validando Estatus de pagina
            switch((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Edicion:
                    //Cambiando estatus de Página a Lectura
                    Session["estatus"] = Pagina.Estatus.Lectura;
                        break;
            }
            
            //Inicializando Forma
            inicializaForma();
        }
        /// <summary>
        /// Evento disparado al Presionar el Menu
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
                        Session["estatus"] = Pagina.Estatus.Nuevo;
                        //Limpiando Id de sesión
                        Session["id_registro"] = 0;
                        //Limpiando Mensaje de Error
                        lblError.Text = "";
                        //Limpiando contenido de forma
                        inicializaForma();
                        break;
                    }
                case "Abrir":
                    {   
                        //Inicializando Apertura de Registros
                        inicializaAperturaRegistro(6, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);
                        
                        break;
                    }
                case "Guardar":
                    {   //Invocando Método de Guardado
                        guardaProducto();
                        break;
                    }
                case "Editar":
                    {   
                        //Asignando estatus nuevo
                        Session["estatus"] = Pagina.Estatus.Edicion;
                        //Limpiando contenido de forma
                        inicializaForma();
                        break;
                    }
                case "Eliminar":
                    {   
                        //Instanciando Producto
                        using(SAT_CL.Global.Producto pro = new SAT_CL.Global.Producto(Convert.ToInt32(Session["id_registro"])))
                        {   
                            //Validando que exista un Producto
                            if(pro.id_producto != 0)
                            {   
                                //Declarando Objeto de Retorno
                                RetornoOperacion result = new RetornoOperacion();
                                //Deshabilitando Producto
                                result = pro.DeshabilitaProducto(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                //Validando que la Operación sea exitosa
                                if(result.OperacionExitosa)
                                {   
                                    //Limpiando registro de Session
                                    Session["id_registro"] = 0;
                                    //Cambiando a Estatus "Nuevo"
                                    Session["estatus"] = Pagina.Estatus.Nuevo;
                                    //Inicializando Forma
                                    inicializaForma();
                                }
                                
                                //Mostrando Mensaje de Operación
                                lblError.Text = result.Mensaje;
                            }
                        }
                        break;
                    }
                case "Bitacora":
                    {   //Invocando Método de Inicializacion de Bitacora
                        inicializaBitacora(Session["id_registro"].ToString(), "6", "Producto");
                        break;
                    }
                case "Referencias":
                    {   //Invocando Método de Inicialización de Referencias
                        
                        inicializaReferenciaRegistro(Session["id_registro"].ToString(), "6", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString());
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

        #endregion

        #region Métodos

        /// <summary>
        /// Método Privado encargado de Inicializar la Página
        /// </summary>
        private void inicializaForma()
        {   //Validando que exista un estatus de Página
            Session["estatus"] = Session["estatus"] == null ? Pagina.Estatus.Nuevo : Session["estatus"];
            //Cargando Catalogos
            CapaNegocio.m_capaNegocio.CargaCatalogo(ddlUnidadTemp, 2, "", 6, "", 0, "");
            //Habilitando Controles
            habilitaControles();
            //Habilitando Menu
            habilitaMenu();
            //Inicializando Valores
            inicializaValores();
        }
        /// <summary>
        /// Método Privado encargado de Habilitar los Controles de la Página
        /// </summary>
        private void habilitaControles()
        {   //Validando Estatus de Pagina
            switch((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {   //Habilitando Controles
                        txtCompania.Enabled = 
                        txtDescripcion.Enabled =
                        chkPeligroso.Enabled =
                        chkFlamable.Enabled =
                        chkPerecedero.Enabled =
                        chkFluido.Enabled =
                        txtTempMin.Enabled =
                        txtTempMax.Enabled =
                        ddlUnidadTemp.Enabled =
                        txtInfoAdd1.Enabled =
                        txtInfoAdd2.Enabled =
                        btnGuardar.Enabled =
                        btnCancelar.Enabled = true;
                        
                        break;
                    }
                case Pagina.Estatus.Lectura:
                    {   //Deshabilitando Controles
                        txtCompania.Enabled = 
                        txtDescripcion.Enabled =
                        chkPeligroso.Enabled =
                        chkFlamable.Enabled =
                        chkPerecedero.Enabled =
                        chkFluido.Enabled =
                        txtTempMin.Enabled =
                        txtTempMax.Enabled =
                        ddlUnidadTemp.Enabled =
                        txtInfoAdd1.Enabled =
                        txtInfoAdd2.Enabled =
                        btnGuardar.Enabled =
                        btnCancelar.Enabled = false;

                        break;
                    }
                case Pagina.Estatus.Edicion:
                    {   //Habilitando Controles
                        txtCompania.Enabled = 
                        txtDescripcion.Enabled =
                        chkPeligroso.Enabled =
                        chkFlamable.Enabled =
                        chkPerecedero.Enabled =
                        chkFluido.Enabled =
                        txtTempMin.Enabled =
                        txtTempMax.Enabled =
                        ddlUnidadTemp.Enabled =
                        txtInfoAdd1.Enabled =
                        txtInfoAdd2.Enabled =
                        btnGuardar.Enabled =
                        btnCancelar.Enabled = true;

                        break;
                    }
            }
        }
        /// <summary>
        /// Método Privado encargado de Habilitar el Menu
        /// </summary>
        private void habilitaMenu()
        {   //Validando Estatus de Pagina
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
        /// Método Privado encargado de Inicializar los Valores
        /// </summary>
        private void inicializaValores()
        {   //Validando Estatus de Pagina
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {   //Inicializando Valores
                        lblId.Text = "Por Asignar";

                        //Instanciando Compania
                        using (SAT_CL.Global.CompaniaEmisorReceptor cer = new SAT_CL.Global.CompaniaEmisorReceptor(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                        {   
                            //validando que exista la Compania
                            if (cer.id_compania_emisor_receptor != 0)

                                //Añadiendo Descripción
                                txtCompania.Text = cer.nombre + " ID:" + cer.id_compania_emisor_receptor.ToString();
                            else
                                //Limpiando Control
                                txtCompania.Text = "";
                        }
                        txtDescripcion.Text = 
                        txtTempMin.Text =
                        txtTempMax.Text =
                        txtInfoAdd1.Text =
                        txtInfoAdd2.Text = "";
                        chkPeligroso.Checked =
                        chkFlamable.Checked =
                        chkPerecedero.Checked =
                        chkFluido.Checked = false;

                        break;
                    }
                case Pagina.Estatus.Lectura:
                    
                case Pagina.Estatus.Edicion:
                    {   //Instanciando Producto
                        using (SAT_CL.Global.Producto pro = new SAT_CL.Global.Producto(Convert.ToInt32(Session["id_registro"])))
                        {   //Validando que exista un Id Valido
                            if (pro.id_producto != 0)
                            {   //Inicializando Valores
                                lblId.Text = pro.id_producto.ToString();
                                //Cargando catalogos
                                CapaNegocio.m_capaNegocio.CargaCatalogo(ddlUnidadTemp, 2, "Ninguno", 6, "", 0, "");
                                ddlUnidadTemp.SelectedValue = pro.id_unidad_temperatura.ToString();
                                using(SAT_CL.Global.CompaniaEmisorReceptor cer = new SAT_CL.Global.CompaniaEmisorReceptor(pro.id_compania_emisor))
                                {   //validando que exista la Compania
                                    if (cer.id_compania_emisor_receptor != 0)
                                        //Añadiendo Descripción
                                        txtCompania.Text = cer.nombre + " ID:" + cer.id_compania_emisor_receptor.ToString();
                                }
                                //txtCompania.Text = 
                                txtDescripcion.Text = pro.descripcion;
                                txtTempMin.Text = pro.minima_temperatura.ToString();
                                txtTempMax.Text = pro.maxima_temperatura.ToString();
                                txtInfoAdd1.Text = pro.informacion_adicional1;
                                txtInfoAdd2.Text = pro.informacion_adicional2;
                                chkPeligroso.Checked = pro.bit_peligroso;
                                chkFlamable.Checked = pro.bit_flamable;
                                chkPerecedero.Checked = pro.bit_perecedero;
                                chkFluido.Checked = pro.bit_fluido;
                            }
                        }
                        break;
                    }
            }
        }
        /// <summary>
        /// Método Privado encargado de Guardar los Registros
        /// </summary>
        private void guardaProducto()
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Validando Estatus de Página
            switch((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {   
                        //Insertando Producto
                        result = SAT_CL.Global.Producto.InsertaProducto(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, txtDescripcion.Text, chkPeligroso.Checked, chkFlamable.Checked,
                                                            chkPerecedero.Checked, chkFluido.Checked, Convert.ToInt32(txtTempMin.Text == "" ? "0" : txtTempMin.Text), Convert.ToInt32(txtTempMax.Text == "" ? "0" : txtTempMax.Text),
                                                            Convert.ToInt32(ddlUnidadTemp.SelectedValue), txtInfoAdd1.Text, txtInfoAdd2.Text, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                        break;
                    }
                case Pagina.Estatus.Edicion:
                    {   
                        //Instanciando Producto
                        using (SAT_CL.Global.Producto pro = new SAT_CL.Global.Producto(Convert.ToInt32(Session["id_registro"])))
                        {   
                            //Validando que existe el Producto
                            if (pro.id_producto > 0)

                                //Editando Producto
                                result = pro.EditaProducto(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, txtDescripcion.Text, chkPeligroso.Checked, chkFlamable.Checked,
                                                                chkPerecedero.Checked, chkFluido.Checked, Convert.ToInt32(txtTempMin.Text == "" ? "0" : txtTempMin.Text),
                                                                Convert.ToInt32(txtTempMax.Text == "" ? "0" : txtTempMax.Text), Convert.ToInt32(ddlUnidadTemp.SelectedValue),
                                                                txtInfoAdd1.Text, txtInfoAdd2.Text, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                            else
                                //Instanciando la Excepción
                                result = new RetornoOperacion("No se puede Acceder al Producto");
                        }
                        break;
                    }
            }
            
            //Validando que la Operación haya sido exitosa
            if(result.OperacionExitosa)
            {   
                //Asignando Valor a las Sessiones
                Session["estatus"] = Pagina.Estatus.Lectura;
                Session["id_registro"] = result.IdRegistro;
                
                //Invocando Método de Inicialización
                inicializaForma();
            }

            //Mostrando Mensaje
            lblError.Text = result.Mensaje;
        }
        /// <summary>
        /// Método Privado encargado de Inicializar la Forma de Bitacora
        /// </summary>
        /// <param name="idRegistro">Id de Registro</param>
        /// <param name="idTabla">Id de Tabla</param>
        /// <param name="Titulo">Titulo</param>
        private void inicializaBitacora(string idRegistro, string idTabla, string Titulo)
        {   //Construyendo URL 
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/General/Producto.aspx", "~/Accesorios/BitacoraRegistro.aspx?idT=" + idTabla + "&idR=" + idRegistro + "&tB=" + Titulo);
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Abriendo Nueva Ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "", configuracion, Page);
        }
        /// <summary>
        /// Método Privado encargado de Inicializar la Apertura de un Registro
        /// </summary>
        /// <param name="idTabla">Id de Tabla</param>
        private void inicializaAperturaRegistro(int idTabla, int idCompania)
        {   //Asignando Session
            Session["id_tabla"] = idTabla;
            //Construyendo URL 
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/General/Producto.aspx", "~/Accesorios/AbrirRegistro.aspx?P1=" + idCompania.ToString());
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
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
            string urlDestino = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/General/Producto.aspx", "~/Accesorios/ReferenciaRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idC=" + id_compania);
            //Instanciando nueva ventana de navegador para apertura de referencias de registro
            TSDK.ASP.ScriptServer.AbreNuevaVentana(urlDestino, "Referencia", 800, 500, false, false, false, true, true, Page);
        }

        #endregion
    }
}