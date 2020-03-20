using SAT_CL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;

namespace SAT.Administrativo
{
    public partial class TipoCambio : System.Web.UI.Page
    {
        #region Eventos

        /// <summary>
        /// Evento Producido al Actualizar la Forma
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Aplicando seguridad de la forma
            SAT_CL.Seguridad.Forma.AplicaSeguridadForma(this, "content1", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

            //Evento Producido al Efectuarse un PostBack
            if (!Page.IsPostBack)

                //Inicializando Página
                inicializaPagina();
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
                        inicializaPagina();
                        //Se realiza un enfoque al primer control 
                        ddlMoneda.Focus();
                        break;
                    }
                //Si la elección del menú es la opcion Abrir
                case "Abrir":
                    {
                        //Invoca al método inicializaAperturaRegistro();
                        inicializaAperturaRegistro(102, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);

                        break;
                    }

                //Si la elección del menú es la opción Guardar
                case "Guardar":
                    {
                        //Declarando Objeto de retorno
                        RetornoOperacion result = new RetornoOperacion();

                        //Guardando Tipo de Cambio
                        result = guardarTipoCambio();

                        //Mostrando Mensaje de Operación
                        ScriptServer.MuestraNotificacion(botonMenu, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
                        break;
                    }
                //Si la elección del menú es la opción Editar
                case "Editar":
                    {
                        //Asigna a la variable session estaus el estado de la pagina nuevo
                        Session["estatus"] = Pagina.Estatus.Edicion;
                        //Invoca el método inicializaForma();
                        inicializaPagina();
                        break;
                    }
                //Si la elección del menú es la opción Eliminar
                case "Eliminar":
                    {
                        //Creación del objeto retorno
                        RetornoOperacion retorno = new RetornoOperacion();
                        //Invoca al constructor de la clase CuentaBancos con el valor de la variable de sessión Id_registro.
                        using (SAT_CL.Bancos.TipoCambio tc = new SAT_CL.Bancos.TipoCambio((int)Session["id_registro"]))
                        {
                            //Valida que exista el registro
                            if (tc.habilitar)
                                //Asigna al objeto retorno los datos del usuario que elimino el registro, invocando al método Deshabilitar de la clase CuentaBancos
                                retorno = tc.DeshabilitarTipoCambio(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                        }
                        //Valida so i la operación de deshabilitar registro se realizo correctamente
                        if (retorno.OperacionExitosa)
                        {
                            //Asigna a la variable de sessión estatus el estado de la página Nuevo.
                            Session["estatus"] = Pagina.Estatus.Nuevo;
                            //Asigna a la variable de session id_registro el valor de 0.
                            Session["id_registro"] = 0;
                            //Invoca al método inicializaForma().
                            inicializaPagina();
                        }
                        //Mensaje de que la operación de deshabilitar registros se realizo correctamente.
                        ScriptServer.MuestraNotificacion(botonMenu, retorno, ScriptServer.PosicionNotificacion.AbajoDerecha);
                        break;
                    }
                //Si la elección del menú es la opción Bitacora
                case "Bitacora":
                    {
                        //Invoca al método bitacora
                        inicializaBitacora(Session["id_registro"].ToString(), "102", "Tipo de Cambio");
                        break;
                    }
                //Si la elección del menú es la opcion Referencia
                case "Referencias":
                    {
                        //Invoca al método inicializaReferencia que muestra las observaciones hechas a un registro de CuentaBancos
                        inicializaReferenciaRegistro(Session["id_registro"].ToString(), "102",
                                                    ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString());
                        break;
                    }
                //Si la elección del menú es la opcion Archivo
                case "Archivo":
                    {
                        break;
                    }
                //Si la elección del menú es la opcion Acerca
                case "Acerca":
                    {
                        break;
                    }
                //Si la elección del menú es la opcion Ayuda
                case "Ayuda":
                    {
                        break;
                    }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            //Declarando Objeto de retorno
            RetornoOperacion result = new RetornoOperacion();

            //Guardando Tipo de Cambio
            result = guardarTipoCambio();

            //Mostrando Mensaje de Operación
            ScriptServer.MuestraNotificacion(btnGuardar, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            //Acorde al estatus de la página valida:
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //En caso de que el estado de la pagina sea Nuevo
                case Pagina.Estatus.Nuevo:
                    {
                        //Asigna a la variable de sessión id_registro el valor de 0.
                        Session["id_registro"] = 0;
                        break;
                    }
                //En caso de que el estado de la página sea de edición
                case Pagina.Estatus.Edicion:
                    {
                        //Asigna a la variable de session estatus el valor de Lectura
                        Session["estatus"] = Pagina.Estatus.Lectura;
                        break;
                    }
            }
            //Invoca al método inicializaForma().
            inicializaPagina();
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Método encargado de Inicializar la Página
        /// </summary>
        private void inicializaPagina()
        {
            //Cargando Catalogos
            cargaCatalogos();
            //Habilitando Menu
            habilitaMenu();
            //Habilitando Controles
            habilitaControles();
            //Inicializando Valores
            inicializaValores();
        }
        /// <summary>
        /// Método encargado de Cargar los Catalogos
        /// </summary>
        private void cargaCatalogos()
        {
            //Cargando Moneda
            CapaNegocio.m_capaNegocio.CargaCatalogo(ddlMoneda, 97, "", 0, "", 0, "");
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipoOperacion, "Todos", 82);
        }
        /// <summary>
        /// Método que permite habilitar o deshabilitar los controles en base al estado de la página
        /// </summary>
        private void habilitaControles()
        {
            //Valida cada estado de la página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //En caso de que el estado de la página sea Nuevo o edición
                case Pagina.Estatus.Nuevo:
                    {
                        ddlMoneda.Enabled =
                        ddlTipoOperacion.Enabled =
                        txtFecha.Enabled =
                        txtValor.Enabled =
                        btnGuardar.Enabled =
                        btnCancelar.Enabled = true;
                        break;
                    }
                case Pagina.Estatus.Edicion:
                    {
                        //Habilita los controles del formularios
                        ddlMoneda.Enabled =
                        ddlTipoOperacion.Enabled =
                        txtFecha.Enabled =
                        txtValor.Enabled =
                        btnGuardar.Enabled =
                        btnCancelar.Enabled = true;
                        break;
                    }
                //En caso de que el estado de la página sea lectura
                case Pagina.Estatus.Lectura:
                    {
                        //Deshabilita los controles del formulario
                        ddlMoneda.Enabled =
                        ddlTipoOperacion.Enabled =
                        txtFecha.Enabled =
                        txtValor.Enabled =
                        btnGuardar.Enabled =
                        btnCancelar.Enabled = false;
                        break;
                    }
            }
        }
        /// <summary>
        /// Método que habilita las opciones del menú principal en base al estado de la página
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
                        btnGuardar.Enabled = true;
                        break;
                    }
            }
        }
        /// <summary>
        /// Método que asigna valores a los controles en base al estado de la página 
        /// </summary>
        private void inicializaValores()
        {
            //Valida cada estado de la página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //En caso de que el estado de la página sea nuevo
                case Pagina.Estatus.Nuevo:
                    {
                        //Limpia el contenido de los controles
                        txtValor.Text = "";
                        txtFecha.Text = Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy");
                        break;
                    }
                //En caso de que el estado de la pagina sea edición o lectura
                case Pagina.Estatus.Lectura:
                case Pagina.Estatus.Edicion:
                    {
                        //Invoca al constructor de la clase CuentaBancos y asigna como paramétro el valor de la variable de session id_registro
                        using (SAT_CL.Bancos.TipoCambio tc = new SAT_CL.Bancos.TipoCambio((int)Session["id_registro"]))
                        {
                            //Valida que exista el registro de cuenta bancos
                            if (tc.habilitar)
                            {
                                //Asignando Valores
                                txtValor.Text = tc.valor_tipo_cambio.ToString("0.000000");
                                txtFecha.Text = tc.fecha.ToString("dd/MM/yyyy");
                                ddlMoneda.SelectedValue = tc.id_moneda.ToString();
                                ddlTipoOperacion.SelectedValue = tc.id_operacion_uso.ToString();
                            }
                        }
                        break;
                    }
            }

        }
        /// <summary>
        /// Método que almacena los datos de los controles validando si es un nuevo registro o una edición
        /// </summary>
        private RetornoOperacion guardarTipoCambio()
        {
            //Creación del objeto retorno
            RetornoOperacion result = new RetornoOperacion();

            //Obteniendo Fecha
            DateTime fecha_tc;
            DateTime.TryParse(txtFecha.Text, out fecha_tc);
            
            //Valida cada estado de la página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //En caso de que el estado de la página sea nuevo
                case Pagina.Estatus.Nuevo:
                    {
                        //Asigna al objeto retorno los valores de los controles invocando al método de inserción de la clase CuentaBancos
                        result = SAT_CL.Bancos.TipoCambio.InsertarTipoCambio(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, 
                                                                Convert.ToDecimal(txtValor.Text), Convert.ToByte(ddlMoneda.SelectedValue),
                                                                fecha_tc, (SAT_CL.Bancos.TipoCambio.OperacionUso)Convert.ToByte(ddlTipoOperacion.SelectedValue),
                                                                ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                        break;
                    }
                case Pagina.Estatus.Edicion:
                    {
                        //Invoca al constructor de la clase cuentaBancos y asigna como parámetro el valor de la variable session id_registro
                        using (SAT_CL.Bancos.TipoCambio tc = new SAT_CL.Bancos.TipoCambio((int)Session["id_registro"]))
                        {
                            //Valida si existe el registro cuenta banco
                            if (tc.habilitar)
                            {
                                //Asigna al objeto retorno el valor de los controles invocando al método de edición de la clase CuentaBancos
                                result = tc.EditarTipoCambio(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                                        Convert.ToDecimal(txtValor.Text), Convert.ToByte(ddlMoneda.SelectedValue),
                                                        fecha_tc, (SAT_CL.Bancos.TipoCambio.OperacionUso)Convert.ToByte(ddlTipoOperacion.SelectedValue),
                                                        ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                            }
                        }
                        break;
                    }
            }

            //Valida que la operacion de insercion y edición se reañizaron corectamente.
            if (result.OperacionExitosa)
            {
                //Asigna a la variable de session estatus el valor de Lectura.
                Session["estatus"] = Pagina.Estatus.Lectura;
                //Asigna a la variable de session id_registro el valor generado en la base de datos(id)
                Session["id_registro"] = result.IdRegistro;
                //Invoca al método inicializaForma().
                inicializaPagina();
            }

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        ///  Método que permie la consulta del historial de modificicaciones realizadas a un registro 
        /// </summary>
        /// <param name="idRegistro">ID que permite identificar un registro de CuentaBancos</param>
        /// <param name="idTabla">Identificador de la tabla CuentaBancos</param>
        /// <param name="Titulo">Encabezado de la ventana Bitacora</param>
        private void inicializaBitacora(string idRegistro, string idTabla, string Titulo)
        {
            //Creación de la variable url, que almacena una ruta de ubicación de la ventana, que contendra la bitacora de un registros de  CuentaBancos.
            string url = Cadena.RutaRelativaAAbsoluta("~/Administrativo/TipoCambio.aspx",
                                                                "~/Accesorios/BitacoraRegistro.aspx?idT=" + idTabla + "&idR=" + idRegistro + "&tB=" + Titulo);
            //Variable que almacena la resolucion de la ventana bitacora
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Invoca el método de la clase ScriptServer que abrira una nueva ventana acorde a una ubicacion, un titulo de la ventana, una configuracion de dimensiones.
            ScriptServer.AbreNuevaVentana(url, "Bitacora Tipo de Cambio", configuracion, Page);
        }

        /// <summary>
        /// Método que permite la busqueda de registros en la base de datos pertenecientes a la tabla CuentaBancos
        /// </summary>
        /// <param name="idTabla">Id que identifica a la tabla CuentaBancos registrada en la base de datos</param>
        /// <param name="idCompania"> Id que identifica a una compañia</param>
        private void inicializaAperturaRegistro(int idTabla, int idCompania)
        {
            //Asigna a la variable de session id_tabla el valor del parametro idTabla
            Session["id_tabla"] = idTabla;
            //Asigna valores a la variable url con la ruta de ubicacion de la ventana Abrir registros con los datos obtenidos de la tabla CuentaBancos
            string url = Cadena.RutaRelativaAAbsoluta("~/Administrativo/TipoCambio.aspx", "~/Accesorios/AbrirRegistro.aspx?P1=" + idCompania.ToString());
            //Define las dimensiones de la ventana Abrir registros de CuentaBancos
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Invoca al método de la variable ScripServer que abrira una nueva ventana con los registros pertenecientes a la tabla CuentaBancos
            ScriptServer.AbreNuevaVentana(url, "Abrir Tipo de Cambio", configuracion, Page);
        }

        /// <summary>
        /// Método encargado de inicializr la referencias a un registro
        /// </summary>
        /// <param name="id_registro">Id que permite identificar un registro de la tabla CuentaBancos</param>
        /// <param name="id_tabla">Id que permite identificar a la tabla CuentaBancos en la base de datos</param>
        /// <param name="id_compania">Id que permite identificar a que compania pertenece </param>
        private void inicializaReferenciaRegistro(string id_registro, string id_tabla, string id_compania)
        {
            //Asigna a la variable urlDestino una ruta de ubucación de la ventana de Referencia de la tabla CuentaBancos
            string urlDestino = Cadena.RutaRelativaAAbsoluta("~/Administrativo/TipoCambio.aspx", "~/Accesorios/ReferenciaRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idC=" + id_compania);
            //Invoca al método de la clase ScripServer que abrira una nueva ventana con los valores de Referencia de la tabla CuentaBancos
            ScriptServer.AbreNuevaVentana(urlDestino, "Referencia de Tipo de Cambio", 800, 500, false, false, false, true, true, Page);
        }

        #endregion
    }
}