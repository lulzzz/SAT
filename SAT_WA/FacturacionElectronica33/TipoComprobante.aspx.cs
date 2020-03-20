using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;

namespace SAT.FacturacionElectronica33
{
    public partial class TipoComprobante : System.Web.UI.Page
    {
        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            //Aplicando seguridad de la forma
            SAT_CL.Seguridad.Forma.AplicaSeguridadForma(this, "content1", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
            //Validando que se produjo un PostBack
            if (!(Page.IsPostBack))

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
            //guardaTipoComprobante();
            TSDK.ASP.ScriptServer.MuestraNotificacion(btnGuardar, guardaTipoComprobante(), ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

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
                        //Limpiando contenido de forma
                        inicializaForma();
                        break;
                    }
                case "Abrir":
                    {
                        //Inicializando Apertura de Registros
                        inicializaAperturaRegistro(212, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);

                        break;
                    }
                case "Guardar":
                    {   //Invocando Método de Guardado
                        //guardaTipoComprobante();
                        //RetornoOperacion resultado = new RetornoOperacion();
                        //resultado = new RetornoOperacion("Registro guardado exitosamente.");
                        TSDK.ASP.ScriptServer.MuestraNotificacion(lkbGuardar, guardaTipoComprobante(), ScriptServer.PosicionNotificacion.AbajoDerecha);
                        break;
                    }
                case "Editar":
                    {
                        //Asignando estatus nuevo
                        Session["estatus"] = Pagina.Estatus.Edicion;
                        //Limpiando contenido de forma
                        inicializaForma();
                        //RetornoOperacion resultado = new RetornoOperacion();
                        //resultado = new RetornoOperacion("Se edito registro exitosamente.");
                        //TSDK.ASP.ScriptServer.MuestraNotificacion(lkbEditar, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
                        break;
                        
                    }
                case "Eliminar":
                    {
                        //Instanciando Producto
                        using (SAT_CL.FacturacionElectronica33.TipoComprobante tc = new SAT_CL.FacturacionElectronica33.TipoComprobante(Convert.ToInt32(Session["id_registro"])))
                        {
                            //Validando que exista un Producto
                            if (tc.id_tipo_comprobante != 0)
                            {
                                //Declarando Objeto de Retorno
                                RetornoOperacion resultado = new RetornoOperacion();
                                //Deshabilitando Producto
                                resultado = tc.DeshabilitaTipoComprobante(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                //Validando que la Operación sea exitosa
                                if (resultado.OperacionExitosa)
                                {
                                    //Limpiando registro de Session
                                    Session["id_registro"] = 0;
                                    //Cambiando a Estatus "Nuevo"
                                    Session["estatus"] = Pagina.Estatus.Nuevo;
                                    //Inicializando Forma
                                    inicializaForma();
                                }

                                //Mostrando Mensaje de Operación
                                TSDK.ASP.ScriptServer.MuestraNotificacion(lkbEliminar, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);

                            }
                        }
                        break;
                    }
                case "Bitacora":
                    {   //Invocando Método de Inicializacion de Bitacora
                        inicializaBitacora(Session["id_registro"].ToString(), "212", "TipoComprobante");
                        break;
                    }
                case "Referencias":
                    {   //Invocando Método de Inicialización de Referencias

                        inicializaReferenciaRegistro(Session["id_registro"].ToString(), "212", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString());
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
                        txtClave.Enabled = 
                        txtDescripcion.Enabled = 
                        txtValorMaximo.Enabled = 
                        txtValorMaximoSueldos.Enabled = 
                        txtValorMaximoOtros.Enabled = 
                        btnGuardar.Enabled =
                        btnCancelar.Enabled = true;
                        break;
                    }

                  case Pagina.Estatus.Lectura:
                    {   //Deshabilitando Controles
                        txtClave.Enabled =
                        txtDescripcion.Enabled =
                        txtValorMaximo.Enabled =
                        txtValorMaximoSueldos.Enabled =
                        txtValorMaximoOtros.Enabled =
                        btnGuardar.Enabled =
                        btnCancelar.Enabled = false;
                        break;
                    }

                  case Pagina.Estatus.Edicion:
                    {   //Habilitando Controles
                        txtClave.Enabled =
                        txtDescripcion.Enabled =
                        txtValorMaximo.Enabled =
                        txtValorMaximoSueldos.Enabled =
                        txtValorMaximoOtros.Enabled =
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
        {   //Validando Estatus de Session
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {   //Asignando Valores
                        txtClave.Text =
                        txtDescripcion.Text =
                        txtValorMaximo.Text =
                        txtValorMaximoSueldos.Text =
                        txtValorMaximoOtros.Text = "";
                        break;
                    }

                case Pagina.Estatus.Edicion:
                case Pagina.Estatus.Lectura:
                    {   //Instanciando la Compania
                        using (SAT_CL.FacturacionElectronica33.TipoComprobante tc = new SAT_CL.FacturacionElectronica33.TipoComprobante(Convert.ToInt32(Session["id_registro"])))
                        {
                            //validando que el registro 
                            if (tc.habilitar)
                                //Asignando Valores
                                txtClave.Text = tc.clave;
                                txtDescripcion.Text = tc.descripcion;
                                txtValorMaximo.Text = tc.valor_maximo.ToString();
                                txtValorMaximoSueldos.Text = tc.valor_maximo_sueldos.ToString();
                                txtValorMaximoOtros.Text = tc.valor_maximo_otros.ToString();
                         break;
                        }
                    }
            }

        }

        /// <summary>
        /// Método encargado de guardar un nuevo registro
        /// </summary>
        private RetornoOperacion guardaTipoComprobante()
        {
            //Declarando objeto resultado
            RetornoOperacion resultado = new RetornoOperacion();
           
            //De acuerdo al estatus de la página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //Insertar Registro
                case Pagina.Estatus.Nuevo:
                    resultado = SAT_CL.FacturacionElectronica33.TipoComprobante.InsertaTipoComprobante(
                        txtClave.Text,
                        txtDescripcion.Text.ToUpper(),
                        Convert.ToDecimal(txtValorMaximo.Text),
                        Convert.ToDecimal(txtValorMaximoSueldos.Text),
                        Convert.ToDecimal(txtValorMaximoOtros.Text),
                        ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_usuario);
                    break;
                case Pagina.Estatus.Edicion:
                    //Instanciando al registro actual
                    using (SAT_CL.FacturacionElectronica33.TipoComprobante tc = new SAT_CL.FacturacionElectronica33.TipoComprobante(Convert.ToInt32(Session["id_registro"])))
                    {
                        //Si el registro existe/está disponible
                        if (tc.habilitar)
                        {

                            //Editando Producto
                            resultado = tc.EditaTipoComprobante(txtClave.Text,
                        txtDescripcion.Text,
                        Convert.ToDecimal(txtValorMaximo.Text),
                        Convert.ToDecimal(txtValorMaximoSueldos.Text),
                        Convert.ToDecimal(txtValorMaximoOtros.Text),
                        ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_usuario);
                                
                                //Convert.ToInt32(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_usuario.ToString()));
                        }
                        else
                            resultado = new RetornoOperacion("Registro no encontrado.");
                        
                    }
                    break;
            }
            //Validar si la insersion se hizo correctamente
            if (resultado.OperacionExitosa)
            {
                //El arrelgo session en su posicion estatus, se asigna el valor lectura
                Session["estatus"] = Pagina.Estatus.Lectura;
                //El arreglo session en su posicion id_registro se asigna el valor insertado
                Session["id_registro"] = resultado.IdRegistro;
                //Inicializar forma
                inicializaForma();
            }

            return resultado;
        }

        /// <summary>
        /// Método Privado encargado de Inicializar la Apertura de un Registro
        /// </summary>
        /// <param name="idTabla">Id de Tabla</param>
        private void inicializaAperturaRegistro(int idTabla, int idCompania)
        {   //Asignando Session
            Session["id_tabla"] = idTabla;
            //Construyendo URL 
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/FacturacionElectronica33/TipoComprobante.aspx", "~/Accesorios/AbrirRegistro.aspx?P1=" + idCompania.ToString());
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=650,height=600";
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
            string urlDestino = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/FacturacionElectronica33/TipoComprobante.aspx", "~/Accesorios/ReferenciaRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idC=" + id_compania);
            //Instanciando nueva ventana de navegador para apertura de referencias de registro
            TSDK.ASP.ScriptServer.AbreNuevaVentana(urlDestino, "Referencia", 800, 500, false, false, false, true, true, Page);
        }

        /// <summary>
        /// Método Privado encargado de Inicializar la Forma de Bitacora
        /// </summary>
        /// <param name="idRegistro">Id de Registro</param>
        /// <param name="idTabla">Id de Tabla</param>
        /// <param name="Titulo">Titulo</param>
        private void inicializaBitacora(string idRegistro, string idTabla, string Titulo)
        {   //Construyendo URL 
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/FacturacionElectronica33/TipoComprobante.aspx", "~/Accesorios/BitacoraRegistro.aspx?idT=" + idTabla + "&idR=" + idRegistro + "&tB=" + Titulo);
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Abriendo Nueva Ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "", configuracion, Page);
        }


        #endregion
    }
}