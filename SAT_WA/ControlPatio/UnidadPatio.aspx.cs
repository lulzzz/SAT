using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.ControlPatio
{
    public partial class UnidadPatio : System.Web.UI.Page
    {
        #region Eventos

        /// <summary>
        /// Carga de la página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Aplicando seguridad de la forma
            SAT_CL.Seguridad.Forma.AplicaSeguridadForma(this, "content1", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

            //Si no es una recarga de página
            if (!Page.IsPostBack)
                inicializaForma();
        }
        /// <summary>
        /// Click en botón guardar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            guardaUnidadPatio();
        }
        /// <summary>
        /// Click en botón cancelar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            //Valida cada estatus de la Pagina
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //Si el estado de la pagina se encuentra en modo  Nuevo
                case Pagina.Estatus.Nuevo:
                    {
                        //Asigna a la variable session id_registro el valor de 0.
                        Session["id_registro"] = 0;
                        break;
                    }
                //Si el estado de la pagina se encuentra en modo de Edición
                case Pagina.Estatus.Edicion:
                    {
                        //Asigna a la variable session estatus el valor actual de la pagina.
                        Session["estatus"] = Pagina.Estatus.Lectura;
                        break;
                    }
            }
            //Invoca al método inicializaForma();
            inicializaForma();
        }

        protected void lkbElementoMenu_Click(object sender, EventArgs e)
        {
            //Validando estatus de Página
            switch (((LinkButton)sender).CommandName)
            {
                case "Nuevo":
                    {
                        //Asigna a la variable de sesión de estatus el estado del formulario en nuevo
                        Session["estatus"] = Pagina.Estatus.Nuevo;
                        //Asigna a la variable de sesión id_registro el valor de 0
                        Session["id_registro"] = 0;
                        //Invoca al método inicializaForma
                        inicializaForma();
                        //Limpiando mensaje de error del lblError
                        lblError.Text = "";
                        //Hace un enfoque en el primer control
                        txtEconomico.Focus();
                        break;
                    }
                case "Abrir":
                    {
                        //Inicializando Apertura de Registros
                        inicializaAperturaRegistro(244);
                        break;
                    }
                case "Guardar":
                    {
                        //Invocando Método de Guardado
                        guardaUnidadPatio();
                        break;
                    }
                case "Editar":
                    {
                        //Asigna a la variable de sesión el estatus de edición
                        Session["estatus"] = Pagina.Estatus.Edicion;
                        //Invoca el método inicializaForma()
                        inicializaForma();
                        //Limpia los mensajes del lblError
                        lblError.Text = "";
                        //Hace enfoque en el primer control
                        txtEconomico.Focus();
                        break;
                    }
                case "Eliminar":
                    {
                        //Invocanto al método de Eliminación
                        bajaUnidadPatio();
                        break;
                    }
                //Si la elección del menú es la opción Bitacora
                case "Bitacora":
                    {
                        //Invoca al método bitacora
                        inicializaBitacora(Session["id_registro"].ToString(), "244", "Unidad Patio");
                        break;
                    }
                //Si la eleccion del menu es la opcion Imprimir 
                case "Imprimir":
                    {
                        string urlReporte = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/ControlPatio/UnidadPatio.aspx", "~/RDLC/Reporte.aspx");

                        //Instanciando nueva ventana de navegador para apertura de registro
                        TSDK.ASP.ScriptServer.AbreNuevaVentana(string.Format("{0}?idTipoReporte={1}&IdUnidad={2}", urlReporte, "QRUnidad", Convert.ToInt32(Session["id_registro"])), "QRUnidad", "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500", Page);
                        break;
                    }
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Inicializa el contenido de la forma
        /// </summary>
        private void inicializaForma()
        {
            //Cargando catalogos
            //cargaCatalogos();
            //Habilitando controles
            habilitaControles();
            //Habilitando menú
            habilitaMenu();
            //Cargando contenido de controles
            cargaContenidoControles();
        }

        private void cargaContenidoControles()
        {
            //Determinando el estatus de la página
            switch ((TSDK.ASP.Pagina.Estatus)Session["estatus"])
            {
                case TSDK.ASP.Pagina.Estatus.Nuevo:
                    //Borrando el contenido 
                    lblId.Text = "Por Asignar";
                    txtEconomico.Text =
                    txtPlacas.Text =
                    txtColor.Text =
                    txtTransportista.Text = "";
                    break;
                case TSDK.ASP.Pagina.Estatus.Lectura:
                case TSDK.ASP.Pagina.Estatus.Edicion:
                    //Instanciando registro de contacto
                    using (SAT_CL.ControlPatio.UnidadPatio up = new SAT_CL.ControlPatio.UnidadPatio(Convert.ToInt32(Session["id_registro"])))
                    {
                        //Si el registro existe
                        if (up.habilitar)
                        {
                            //Borrando el contenido 
                            lblId.Text = up.id_unidad_patio.ToString();
                            txtEconomico.Text = up.no_economico.ToString();
                            txtPlacas.Text = up.placas.ToString();
                            txtColor.Text = up.color.ToString();
                            using (SAT_CL.ControlPatio.PatioTransportista t = new SAT_CL.ControlPatio.PatioTransportista(up.id_transportista))
                            {
                                //Si el registro existe
                                if (t.habilitar)
                                {
                                    //txtUsuarioSistema.Text = u.nombre.ToString() + " ID:" + c.id_usuario_sistema.ToString();
                                    txtTransportista.Text = String.Format("{0} ID:{1}", t.nombre, up.id_transportista);
                                }
                            }

                        }
                    }
                    break;
            }

            //Limpiando errores
            lblError.Text = "";
        }

        /// <summary>
        /// Habilita o deshabilita los controles de la forma con base al estatus
        /// </summary>
        private void habilitaControles()
        {
            //Con base al estatus
            switch ((TSDK.ASP.Pagina.Estatus)Session["estatus"])
            {
                case TSDK.ASP.Pagina.Estatus.Lectura:
                    txtEconomico.Enabled =
                    txtPlacas.Enabled =
                    txtColor.Enabled =
                    txtTransportista.Enabled =
                    btnGuardar.Enabled =
                    btnCancelar.Enabled = false;
                    break;
                case TSDK.ASP.Pagina.Estatus.Nuevo:
                case TSDK.ASP.Pagina.Estatus.Edicion:
                    txtEconomico.Enabled =
                    txtPlacas.Enabled =
                    txtColor.Enabled =
                    txtTransportista.Enabled =
                    btnGuardar.Enabled =
                    btnCancelar.Enabled = true;
                    break;
            }
        }

        /// <summary>
        /// Método Privado encargado de Habilitar el Menú
        /// </summary>
        private void habilitaMenu()
        {
            //Validando estatus de Session
            switch ((TSDK.ASP.Pagina.Estatus)Session["estatus"])
            {
                case TSDK.ASP.Pagina.Estatus.Nuevo:
                    {   //Archivo
                        lkbNuevo.Enabled =
                        lkbAbrir.Enabled =
                        lkbGuardar.Enabled = true;
                        lkbImprimir.Enabled = false;
                        //Edicion
                        lkbEditar.Enabled =
                        lkbBajaEliminar.Enabled = false;
                        //Herramientas
                        lkbBitacora.Enabled = false;
                        break;
                    }
                case TSDK.ASP.Pagina.Estatus.Lectura:
                    {
                        //Archivo
                        lkbNuevo.Enabled =
                        lkbAbrir.Enabled = true;
                        btnGuardar.Enabled = false;
                        lkbImprimir.Enabled = true;
                        //Edicion
                        lkbEditar.Enabled =
                        lkbBajaEliminar.Enabled = true;
                        //Herramientas
                        lkbBitacora.Enabled = true;
                        break;
                    }
                case TSDK.ASP.Pagina.Estatus.Edicion:
                    {
                        //Archivo
                        lkbNuevo.Enabled =
                        lkbAbrir.Enabled =
                        lkbGuardar.Enabled = true;
                        lkbImprimir.Enabled = false;
                        //Edicion
                        lkbEditar.Enabled =
                        lkbBajaEliminar.Enabled = true;
                        //Herramientas
                        lkbBitacora.Enabled = true;
                        break;
                    }

            }
        }

        /// <summary>
        /// Inserta o actualiza los valores del registro
        /// </summary>
        private void guardaUnidadPatio()
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //En base al estatus
            switch ((TSDK.ASP.Pagina.Estatus)Session["estatus"])
            {
                case TSDK.ASP.Pagina.Estatus.Nuevo:
                    resultado = SAT_CL.ControlPatio.UnidadPatio.InsertaUnidadPatio(
                        txtEconomico.Text.ToUpper(),
                        txtPlacas.Text.ToUpper(),
                        txtColor.Text.ToUpper(),
                        Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtTransportista.Text, "ID:", 1)),
                        ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario,
                        true
                    );
                    break;
                case TSDK.ASP.Pagina.Estatus.Edicion:
                    //Instanciando contacto actual
                    using (SAT_CL.ControlPatio.UnidadPatio up = new SAT_CL.ControlPatio.UnidadPatio(Convert.ToInt32(Session["id_registro"])))
                    {
                        //Si el Contacto existe
                        if (up.habilitar)
                        {
                            resultado = up.EditaUnidadPatio(//Error en el nombre del método
                                txtEconomico.Text.ToUpper(),
                                txtPlacas.Text.ToUpper(),
                                txtColor.Text.ToUpper(),
                                Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtTransportista.Text, "ID:", 1)),
                                ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario
                            );
                        }
                        else
                            resultado = new RetornoOperacion("El registro solicitado no fue encontrado.");
                    }
                    break;
            }

            //Si no hay errores de guardado
            if (resultado.OperacionExitosa)
            {
                //Guardando datos de registro para carga de estatus de lectura
                Session["id_registro"] = resultado.IdRegistro;
                Session["estatus"] = Pagina.Estatus.Lectura;
                inicializaForma();
            }

            //Mostrando resultado.
            TSDK.ASP.ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        private void bajaUnidadPatio()
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();

            //Inicializando Bloque Transaccional
            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Invoca al constructor de la clase y asigna el valor de la variable de session id_registro.
                using (SAT_CL.ControlPatio.UnidadPatio up = new SAT_CL.ControlPatio.UnidadPatio((int)Session["id_registro"]))
                {
                    //Valida si el registro existe.
                    if (up.id_unidad_patio > 0)
                    {
                        //Asigna al objeto retorno los datos del usuario que realizo el cambio de estado del registro (Deshabilitó)                            
                        retorno = up.DeshabilitaUnidadPatio(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                        //Validando Operación Exitosa
                        if (retorno.OperacionExitosa)
                        {
                            //Completando Transacción
                            trans.Complete();

                            //Mensaje de que la operación de deshabilitar registros se realizo correctamente.
                            TSDK.ASP.ScriptServer.MuestraNotificacion(this, retorno, ScriptServer.PosicionNotificacion.AbajoDerecha);
                        }
                    }
                }
            }

            //Valida si la operación se realizo correctamente.
            if (retorno.OperacionExitosa)
            {
                //Asigna el valor de estado lectura a la variable de session estatus 
                Session["estatus"] = Pagina.Estatus.Nuevo;
                //Asigna el valor 0 a la variable de session id_registro
                Session["id_registro"] = 0;
                //invoca al método inicializaForma().
                inicializaForma();
            }
        }

        private void inicializaAperturaRegistro(int idTabla)
        {   //Asignando sesión
            Session["id_tabla"] = idTabla;
            //Construyendo URL
            string url = Cadena.RutaRelativaAAbsoluta("~/ControlPatio/UnidadPatio.aspx", "~/Accesorios/AbrirRegistro.aspx");
            //Definiendo configuración de la ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=650,height=600";
            //Abriendo Nueva Ventana
            ScriptServer.AbreNuevaVentana(url, "Abrir registro de Unidad Patio", configuracion, Page);
        }

        private void inicializaBitacora(string idRegistro, string idTabla, string Titulo)
        {
            //Creación de la variable url, que almacena una ruta de ubicación de la ventana, que contendrá la bitácora de un registros de Unidad Patio.
            string url = Cadena.RutaRelativaAAbsoluta("~/ControlPatio/UnidadPatio.aspx",
                "~/Accesorios/BitacoraRegistro.aspx?idT=" + idTabla + "&idR=" + idRegistro + "&tB=" + Titulo);
            //Variable que almacena la resolucion de la ventana bitacora
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Invoca el método de la clase ScriptServer que abrira una nueva ventana acorde a una ubicacion, un titulo de la ventana, una configuracion de dimensiones.
            ScriptServer.AbreNuevaVentana(url, "Bitacora  Unidad Patio", configuracion, Page);
        }

        #endregion
    }
}