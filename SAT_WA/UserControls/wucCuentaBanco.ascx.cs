using SAT_CL;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
namespace SAT.UserControls
{
    public partial class wucCuentaBanco : System.Web.UI.UserControl
    {
        #region Atributos

        /// <summary>
        /// IdTabla
        /// </summary>
        private int _id_tabla;
        /// <summary>
        /// Id Registro
       /// </summary>
        private int _id_registro;
        /// <summary>
        /// Tabla con las Cuentas encontradas
        /// </summary>
        private DataTable _mitCuentas;

     
        #endregion

        #region Métodos
        /// <summary>
        /// Método encargado de Recuperar los Atributos
        /// </summary>
        private void recuperaAtributos()
        {
            //Validando que Existan atributos antes de reasignar
            if (ViewState["IdRegistro"] != null
               )
            {
                this._id_tabla= Convert.ToInt32(ViewState["IdTabla"]);
                this._id_registro = Convert.ToInt32(ViewState["IdRegistro"]);
                if (ViewState["mitCuentas"] != null)
                    this._mitCuentas = (DataTable)ViewState["mitCuentas"];
            }

        }

        /// <summary>
        /// Método encargado de Asignar los Atributos 
        /// </summary>
        private void asignaAtributos()
        {
            //Asignando Atributos
            ViewState["IdRegistro"] = this._id_registro;
            ViewState["IdTabla"] = this._id_tabla;
            ViewState["mitCuentas"] = this._mitCuentas;
        }

        /// <summary>
        /// Evento Producido antes de Efectuarse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            //Invocando Método de Asignación
            asignaAtributos();
        }

        /// <summary>
        /// Inicializa el Control de Cuentas
        /// </summary>
        /// <param name="id_tabla"></param>
        /// <param name="id_registro"></param>
        /// <param name="hab_consultar"></param>
        public void InicializaControl(int id_entidad, int id_registro  )
        {
            //Asignando a atributos privados
            this._id_tabla = id_entidad;
            this._id_registro = id_registro;

            chkActivas.Checked = true;
            //Deacuerdo al Tipo de Tabla
            //Operador
            if (id_entidad == 76)
            {
                //Instanciamos Operador
                using(SAT_CL.Global.Operador objOperador = new SAT_CL.Global.Operador(id_registro))
                {
                    //Inicializamos Nombre de la Entidad
                    lblEntidad.Text = objOperador.nombre;
                }
            }
                //Compañia
            else if(id_entidad ==25)
            {
                //Instanciamos Operador
                using (SAT_CL.Global.CompaniaEmisorReceptor objCompania = new SAT_CL.Global.CompaniaEmisorReceptor(id_registro))
                {
                    //Inicializamos Nombre de la Entidad
                    lblEntidad.Text = objCompania.nombre;
                }
            }
            

            //Carga Cuentas ligados a una Entidad
            cargaCuentas(this._id_tabla, id_registro);
        }

        /// <summary>
        /// Método encargado de Cargar Cuentas ligadas a una Entidad
        /// </summary>
        private void cargaCuentas(int id_tabla, int id_registro)
        {
            byte activa = 1;
            if(!chkActivas.Checked)
            {
                activa = 0;
            }
               //Cargando Cuentas
            this._mitCuentas = SAT_CL.Bancos.CuentaBancos.CargaCuentasBanco(id_tabla, id_registro, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, activa);
                //Si no hay registros
                if (this._mitCuentas == null)
                    TSDK.ASP.Controles.InicializaGridview(gvCuentas);
                else
                    //Mostrandolos en gridview
                    TSDK.ASP.Controles.CargaGridView(gvCuentas, this._mitCuentas, "Id", lblOrdenadoCuentas.Text, false, 1);

            
        }

        /// <summary>
        /// Método encargado de Deshabilitar la Cuenta
        /// </summary>
        private void deshabilitaCuenta()
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Invoca al constructor de la clase CuentaBancos con el valor de la variable de sessión Id_registro.
            using (SAT_CL.Bancos.CuentaBancos cb = new SAT_CL.Bancos.CuentaBancos(Convert.ToInt32(gvCuentas.SelectedValue)))
            {
                //Valida que exista el registro
                if (cb.id_cuenta_bancos > 0)
                //Deshabilitamos Vale
                    resultado = cb.DeshabilitarCuentaBancos(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

            }
            //SI no existen errores
            if (resultado.OperacionExitosa)
            {
                //Cargamos Cuentas
                cargaCuentas(this._id_tabla, this._id_registro);
                //Inicializamos Indices
                Controles.InicializaIndices(gvCuentas);
            }
            //Muestra el mensaje de error
            TSDK.ASP.ScriptServer.MuestraNotificacion(gvCuentas, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Carga los catalogos en los dropdownlist
        /// </summary>
        private void cargaCatalogos()
        {
            //Eventos
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoCuentas, "", 18);
        }
        /// <summary>
        /// Método Privado encargado de Inicializar la Forma de Bitacora
        /// </summary>
        /// <param name="idRegistro">Id de Registro</param>
        /// <param name="idTabla">Id de Tabla</param>
        /// <param name="Titulo">Titulo</param>
        private void inicializaBitacora(string idRegistro, string idTabla, string Titulo)
        {   //Construyendo URL 
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Ruta/Ruta.aspx", "~/Accesorios/BitacoraRegistro.aspx?idT=" + idTabla + "&idR=" + idRegistro + "&tB=" + Titulo);
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Abriendo Nueva Ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "", configuracion, Page);
        }
        /// <summary>
        /// Método encargado de guardar la Cuenta
        /// </summary>
        private void guardaCuenta()
        { 
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

                //Recuperando controles 
            using (TextBox txtNumCuenta = (TextBox)gvCuentas.FooterRow.FindControl("txtNumCuenta"))
                {
                    using (DropDownList ddlTipoCuenta = (DropDownList)gvCuentas.FooterRow.FindControl("ddlTipoCuenta"),
                           ddlBanco = (DropDownList)gvCuentas.FooterRow.FindControl("ddlBanco"))
                    {

                       
                            //Insertamos Cuenta
                            resultado = SAT_CL.Bancos.CuentaBancos.InsertarCuentaBancos(Convert.ToInt32(ddlBanco.SelectedValue), this._id_tabla, this._id_registro,
                                txtNumCuenta.Text,(SAT_CL.Bancos.CuentaBancos.TipoCuenta)Convert.ToByte(ddlTipoCuenta.SelectedValue),((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                        }
                    }
                

            //SI no existen errores
            if (resultado.OperacionExitosa)
            {
                //Cargamos Cuentas
                cargaCuentas(this._id_tabla, this._id_registro);
                //Inicializamos Indices
                Controles.InicializaIndices(gvCuentas);
            }
            //Muestra el mensaje de error
            TSDK.ASP.ScriptServer.MuestraNotificacion(gvCuentas, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        #endregion
        #region Eventos

        /// <summary>
        /// Evento generado al Cargar las Cuentas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkActivas_CheckedChanged(object sender, EventArgs e)
        {
            //Cargamos las Cuentas
            cargaCuentas(this._id_tabla, this._id_registro);
        }

        /// <summary>
        /// Evento producido al  cargar el control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Validando que se Produjo un PostBack
            if (!(Page.IsPostBack))
            {
                //Carga los Catalogos
                cargaCatalogos();
            }
            else
                //Recuperando Atributos
                recuperaAtributos();
        }

        /// <summary>
        /// Evento corting de gridview de Lectura
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvCuentas_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Validando que existan Llaves
            if (gvCuentas.DataKeys.Count > 0)
            {   //Asignando Criterio de Ordenacion
                this._mitCuentas.DefaultView.Sort = lblOrdenadoCuentas.Text;
                //Cambiando Ordenamiento
                lblOrdenadoCuentas.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvCuentas, this._mitCuentas, e.SortExpression, false, 1);
            }
        }
        /// <summary>
        /// Evento cambio de página en gridview de Bitácora Lectura
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvCuentas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvCuentas, this._mitCuentas, e.NewPageIndex, false, 1);
        }
        /// <summary>
        /// Evento cambio de tamaño de página de gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoCuentas_SelectedIndexChanged(object sender, EventArgs e)
        {
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvCuentas, this._mitCuentas, Convert.ToInt32(ddlTamanoCuentas.SelectedValue), false, 1);
        }
        /// <summary>
        /// Evento click en botón de exportación de Bitácora Lectura
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarCuentas_Click(object sender, EventArgs e)
        {
            TSDK.ASP.Controles.ExportaContenidoGridView(this._mitCuentas, "");
        }

        /// <summary>
        /// Click en algún Link de GV de Cuentas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbAccionCuentas_Click(object sender, EventArgs e)
        {            
                //Seleccionando la fila correspondiente
                Controles.SeleccionaFila(gvCuentas, sender, "lnk", false);
                //Convirtiendo objeto a linkbutton
                LinkButton lkb = (LinkButton)sender;

                //Determinando el comando por ejecutar
                switch (lkb.CommandName)
                {
                    case "Deshabilitar":
                     //Si hay registros
                    if (gvCuentas.DataKeys.Count > 0)
                    {
                    //Deshabilitamos Cuenta
                    deshabilitaCuenta();
                    }
                                break;
                    case "Bitacora":
                        //Si hay registros
                        if (gvCuentas.DataKeys.Count > 0)
                        {
                        //Invocando Método de Inicializacion de Bitacora
                        inicializaBitacora(gvCuentas.SelectedValue.ToString(), "99", "Cuentas");
                        }
                        break;
                    case "Insertar":
                        //Insertamos Cuenta
                        guardaCuenta();
                        break;

                }
            }
    
        /// <summary>
        /// Inicializa los Controles del GV 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvCuentas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Obtenemos la fila actual
            GridViewRow fila = e.Row;

            //Verificamos si la fila es para los elementos de datos o es la fila del footer
            switch (fila.RowType)
            {
                //Fila tipo Datos, para obtener objetos LinkButton correspondientes
                case DataControlRowType.DataRow:
                //Fila Tipo Footer para Obtener los datos
                case DataControlRowType.Footer:
                    {
                        using (DropDownList ddlBanco = (DropDownList)fila.FindControl("ddlBanco"),
                                                ddlTipoCuenta = (DropDownList)fila.FindControl("ddlTipoCuenta"))
                        {
                            if (ddlBanco != null)
                            {
                                //Carga el catalogo Banco al dropdownlist Bancos
                                SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlBanco, 22, "", 0, "", 0, "");
                                //Carga el catalogo TipoCuenta al dropdownlist tipocuenta
                                SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipoCuenta, "", 77);
                                //Inicializamos Tipo de Cuenta
                                ddlTipoCuenta.SelectedValue = "1";
                            }


                        }

                    }
                    break;
            }
        }
        #endregion


    }
}