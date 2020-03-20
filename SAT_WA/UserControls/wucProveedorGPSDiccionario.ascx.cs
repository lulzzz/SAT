using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.UserControls
{
    public partial class wucProveedorGPSDiccionario : System.Web.UI.UserControl
    {
        #region Atributos

        /// <summary>
        /// Id de Bitácora seleccionado
        /// </summary>
        private int _id_proveedorWsDiccionario;
        /// <summary>
        /// Id de tabla de la bitácora 
        /// </summary>
        private int _id_tabla;
        /// <summary>
        /// Id de registo de la bitácora 
        /// </summary>
        private int _id_registro;
        /// <summary>
        /// Tabla con los bitácoras encontradas
        /// </summary>
        private DataTable _DS;

        #endregion

        #region Métodos

        /// <summary>
        /// Método que permite asignar los valores a los controles DropDownList.
        /// </summary>
        private void cargaCatalogos()
        {
            //Cargando catálogo
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamano, "", 18);
            //SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlProveedor, "", 3208);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipoDato, "---SELECIOONAR TIPO----", 3208);
            SAT_CL.Seguridad.Forma.AplicaSeguridadControlusuarioWeb(this, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);    
        }

        /// <summary>
        /// Método encargado de Asignar los Atributos 
        /// </summary>
        private void asignaAtributos()
        {
            //Asignando Atributos
            ViewState["IdTabla"] = this._id_tabla;
            ViewState["IdRegistro"] = this._id_registro;
            ViewState["IdProveedor"] = this._id_proveedorWsDiccionario;
            ViewState["DS"] = this._DS;

        }
        /// <summary>
        /// Método encargado de Recuperar los Atributos
        /// </summary>
        private void recuperaAtributos()
        {
            //Validando que Existan atributos antes de reasignar
            if (ViewState["IdTabla"] != null && ViewState["IdRegistro"] != null)
            {
                this._id_tabla = Convert.ToInt32(ViewState["IdTabla"]);
                this._id_proveedorWsDiccionario = Convert.ToInt32(ViewState["IdProveedor"]);
                if (ViewState["DS"] != null)
                this._DS = (DataTable)ViewState["DS"];
            }

        }

        /// <summary>
        /// Inicializa control de Usuario
        /// </summary>
        /// <param name="id_tabla">Tabla de la bitácora (Unidad =19)</param>
        /// <param name="id_registro">Id Registro</param>
        /// <param name="id_proveedor">Habilitación de la Edición</param>
        public void InicializaControl(int id_tabla, int id_registro)
        {
            //Asignando a atributos privados
            this._id_tabla = id_tabla;
            this._id_registro = id_registro;
            //Cargando catálogo
            cargaCatalogos();
            //inicializa controles
            CargaContenidoControles();

        }
        /// <summary>
        /// Carga la Bitácora Proveedor GPS Diccionario
        /// </summary>
        private void cargaHistorialDiccionario()
        {
            //Obteniendo detalles de viaje
            using (DataTable dtProveedor = SAT_CL.Monitoreo.ProveedorWSDiccionario.ObtieneRegistrosDiccionarioWS(this._id_tabla, this._id_registro))
            {
                //Valida si existen los datos del datase
                if (Validacion.ValidaOrigenDatos(dtProveedor))
                {
                    //Si existen, carga los valores del datatable al gridview
                    Controles.CargaGridView(gvProveedorGPS, dtProveedor, "Id", "");
                    //Asigna a la variable de sesion los datos del dataset invocando al método AñadeTablaDataSet
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtProveedor, "Table");
                }
                //Si no existen
                else
                {
                    //Inicializa el gridView 
                    Controles.InicializaGridview(gvProveedorGPS);
                    //Elimina los datos del dataset si se realizo una consulta anterior
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                }
            }
            ////Inicializa grid
            //Controles.InicializaIndices(gvProveedorGPS);
            //Controles.InicializaGridview(gvProveedorGPS);
        }

        /// <summary>
        /// Inserta y edita los registro
        /// </summary>
        private void InsertaProveedorWSDiccionario()
        {
            //Creacion del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            if (this._id_proveedorWsDiccionario == 0)
            {
                using (SAT_CL.Monitoreo.ProveedorWSDiccionario Inserta = new SAT_CL.Monitoreo.ProveedorWSDiccionario(this._id_proveedorWsDiccionario))
                {
                    retorno = Inserta.InsertarProveedorWSDiccionario(Convert.ToInt32(ddlProveedor.Text), this._id_tabla, Convert.ToInt32(lblIdRegistro.Text), 1,
                            Convert.ToString(txtValor.Text.ToUpper()), Convert.ToInt32(ddlTipoDato.Text), Cadena.VerificaCadenaVacia(txtAlias.Text.ToUpper(), ""), Cadena.VerificaCadenaVacia(txtSerie.Text.ToUpper(), ""),
                           ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                    if (retorno.OperacionExitosa)
                    {
                        //Asigna a la variable el registro insertado
                        this._id_proveedorWsDiccionario = retorno.IdRegistro;
                    }
                }
                //Muestra mensaje de error o correto la transaccion
                ScriptServer.MuestraNotificacion(this.Page, retorno, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }

            if (this._id_proveedorWsDiccionario != 0)
            {
                using (SAT_CL.Monitoreo.ProveedorWSDiccionario Edita = new SAT_CL.Monitoreo.ProveedorWSDiccionario(this._id_proveedorWsDiccionario))
                {
                    retorno = Edita.EditarProveedorWSDiccionario(Convert.ToInt32(ddlProveedor.Text), this._id_tabla, Convert.ToInt32(lblIdRegistro.Text), 1,
                            Convert.ToString(txtValor.Text.ToUpper()), Convert.ToInt32(ddlTipoDato.Text), Cadena.VerificaCadenaVacia(txtAlias.Text.ToUpper(), ""), Cadena.VerificaCadenaVacia(txtSerie.Text.ToUpper(), ""),
                           ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                }
                if (retorno.OperacionExitosa)
                {
                    //Asigna a la variable el registro insertado
                    this._id_proveedorWsDiccionario = retorno.IdRegistro;
                    this._id_registro = Convert.ToInt32(lblIdRegistro.Text);
                    cargaHistorialDiccionario();
                }
                //Muestra mensaje de error o correto la transaccion
                ScriptServer.MuestraNotificacion(this.Page, retorno, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }


        }



        /// <summary>
        /// Método que configura la vista de los controles del view
        /// </summary>
        private void CargaContenidoControles()
        {
            //Valida si existe un registro de Evaluacion Aplicacion
            if (this._id_proveedorWsDiccionario > 0)
            {
                //Instancia a la Evaluacion Aplicacion
                using (SAT_CL.Monitoreo.ProveedorWSDiccionario ea = new SAT_CL.Monitoreo.ProveedorWSDiccionario(this._id_proveedorWsDiccionario))
                {
                    //Valida que este activa
                    if (ea.habilitar)
                    {
                        //tabla Unidad
                        if (_id_tabla == 19)
                        {                           
                            lblIdEntidad.Text = "Unidad:";
                            //Instanciando Unidad
                            using (SAT_CL.Global.Unidad unidad = new SAT_CL.Global.Unidad(this._id_registro))
                            {
                                lblNombre.Text = unidad.numero_unidad;
                            }
                            lblIdRegistro.Text = Convert.ToString(ea.id_registro);
                            ddlProveedor.Text = Convert.ToString( ea.id_proveedor_ws);
                            ddlTipoDato.Text = Convert.ToString(ea.tipo_identificador);
                            txtValor.Text = Convert.ToString(ea.identificador);
                            txtSerie.Text = Convert.ToString(ea.serie);
                            txtAlias.Text = Convert.ToString(ea.alias);
                            //Instanciando Unidad
                            using (SAT_CL.Global.Unidad unidad = new SAT_CL.Global.Unidad(Convert.ToInt32(Session["id_registro"])))
                            {
                                //Validando que exista la Unidad
                                if (unidad.habilitar)

                                    //Cargando Servicios GPS
                                    SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlProveedor, 187, "", unidad.id_compania_emisor, "", 0, "");
                                else
                                    //Inicializando Control
                                    Controles.InicializaDropDownList(ddlProveedor, "");
                            }
                        }
                        //Tabla Compañia
                        if (_id_tabla == 25)
                        {
                            //Instanciando Compañia
                            using (SAT_CL.Global.CompaniaEmisorReceptor Com = new SAT_CL.Global.CompaniaEmisorReceptor(this._id_registro))
                            {
                                lblNombre.Text = Com.nombre;
                            }
                            lblIdEntidad.Text = "Compañia";
                            lblIdRegistro.Text = Convert.ToString(ea.id_registro);
                            ddlProveedor.Text = Convert.ToString(ea.id_proveedor_ws);
                            ddlTipoDato.Text = Convert.ToString(ea.tipo_identificador);
                            txtValor.Text = Convert.ToString(ea.identificador);
                            txtSerie.Text = Convert.ToString(ea.serie);
                            txtAlias.Text = Convert.ToString(ea.alias);
                            txtSerie.Enabled = false;
                            //Instanciando Compañia
                            using (SAT_CL.Global.CompaniaEmisorReceptor Com = new SAT_CL.Global.CompaniaEmisorReceptor(Convert.ToInt32(Session["id_registro"])))
                            {
                                //Validando que exista la Unidad
                                if (Com.habilitar)

                                    //Cargando Servicios GPS
                                    SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlProveedor, 187, "", Com.id_compania_uso, "", 0, "");
                                else
                                    //Inicializando Control
                                    Controles.InicializaDropDownList(ddlProveedor, "");
                            }
                        }
                        //Tabla empleados
                        if (_id_tabla == 76)
                        {
                            lblIdEntidad.Text = "Operador";
                            //Instanciando Operador
                            using (SAT_CL.Global.Operador Ope = new SAT_CL.Global.Operador(this._id_registro))
                            {
                                lblNombre.Text = Ope.nombre;
                            }
                            lblIdRegistro.Text = Convert.ToString(ea.id_registro);
                            ddlProveedor.Text = Convert.ToString(ea.id_proveedor_ws);
                            ddlTipoDato.Text = Convert.ToString(ea.tipo_identificador);
                            txtValor.Text = Convert.ToString(ea.identificador);
                            txtSerie.Text = Convert.ToString(ea.serie);
                            txtAlias.Text = Convert.ToString(ea.alias);
                            txtSerie.Enabled = false;
                            //Instanciando Unidad
                            using (SAT_CL.Global.Operador Ope = new SAT_CL.Global.Operador(Convert.ToInt32(Session["id_registro"])))
                            {
                                //Validando que exista la Unidad
                                if (Ope.habilitar)
                                    //Cargando Servicios GPS
                                    SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlProveedor, 187, "", Ope.id_compania_emisor, "", 0, "");
                                else
                                    //Inicializando Control
                                    Controles.InicializaDropDownList(ddlProveedor, "");
                            }
                        }         
                    }
                }
                Controles.InicializaIndices(gvProveedorGPS);
            }
            //Si no existe
            else
            {
            //tabla Unidad
            if (_id_tabla == 19)
            {
                lblIdEntidad.Text = "Unidad:";
                           
                //Instanciando Unidad
                using (SAT_CL.Global.Unidad unidad = new SAT_CL.Global.Unidad(this._id_registro))
                {
                    lblNombre.Text = unidad.numero_unidad;
                    lblIdRegistro.Text = Convert.ToString(unidad.id_unidad);
                }
                //Instanciando Unidad
                using (SAT_CL.Global.Unidad unidad = new SAT_CL.Global.Unidad(Convert.ToInt32(Session["id_registro"])))
                {
                    //Validando que exista la Unidad
                    if (unidad.habilitar)

                        //Cargando Servicios GPS
                        SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlProveedor, 187, "", unidad.id_compania_emisor, "", 0, "");
                    else
                        //Inicializando Control
                        Controles.InicializaDropDownList(ddlProveedor, "");
                }
             }
                    
                
            //tabla candidatos
            if (_id_tabla == 25)
            {
                lblIdEntidad.Text = "Compañia";
                //Instanciando Compañia
                using (SAT_CL.Global.CompaniaEmisorReceptor Com = new SAT_CL.Global.CompaniaEmisorReceptor(this._id_registro))
                {
                    lblNombre.Text = Com.nombre;
                    lblIdRegistro.Text = Convert.ToString(Com.id_compania_emisor_receptor);
                }
                txtSerie.Enabled = false;
                //Instanciando Compañia
                using (SAT_CL.Global.CompaniaEmisorReceptor Com = new SAT_CL.Global.CompaniaEmisorReceptor(Convert.ToInt32(Session["id_registro"])))
                {
                    //Validando que exista la Unidad
                    if (Com.habilitar)

                        //Cargando Servicios GPS
                        SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlProveedor, 187, "", Com.id_compania_uso, "", 0, "");
                    else
                        //Inicializando Control
                        Controles.InicializaDropDownList(ddlProveedor, "");
                }
            }
            //tabla administrativo
            if (_id_tabla == 76)
            {
                lblIdEntidad.Text = "Operador";
                //Instanciando Unidad
                using (SAT_CL.Global.Operador Ope = new SAT_CL.Global.Operador(this._id_registro))
                {
                    lblNombre.Text = Ope.nombre;
                    lblIdRegistro.Text = Convert.ToString(Ope.id_operador);
                }                         
                txtSerie.Enabled = false;
                //Instanciando Unidad
                using (SAT_CL.Global.Operador Ope = new SAT_CL.Global.Operador(Convert.ToInt32(Session["id_registro"])))
                {
                    //Validando que exista la Unidad
                    if (Ope.habilitar)
                        //Cargando Servicios GPS
                        SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlProveedor, 187, "", Ope.id_compania_emisor, "", 0, "");
                    else
                        //Inicializando Control
                        Controles.InicializaDropDownList(ddlProveedor, "");
                }
             }                                  
             limpiaControles();
            }
            cargaHistorialDiccionario();
        }
        /// <summary>
        /// Metodo Limpia Controles
        /// </summary>
        private void limpiaControles()
        {
            txtAlias.Text =
            txtSerie.Text =
            txtValor.Text = "";             
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Evento producido al  cargar el control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                asignaAtributos();

                cargaCatalogos();
                //inicializa controles
                CargaContenidoControles();
            }
            else
                recuperaAtributos();
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
        /// Evento click en botón nuevo Bitácora Monitoreo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            InsertaProveedorWSDiccionario();
        }
        /// <summary>
        /// Evento que permite la busqueda de bitacora monitoreo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            ////Invoca al método cargaHistorialBitacora
            //cargaHistorialBitacora();
            this._id_proveedorWsDiccionario = 0;
            InicializaControl(this._id_tabla,this._id_registro);
            
        }
        /// <summary>
        /// Evento Producido al Cambiar el Concepto de Cobro del Detalle de Factura
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTipoDato_SelectedIndexChanged(object sender, EventArgs e)
        {           
            ////Devolviendo foco a control
            txtValor.Focus();
            txtValor.Text = "";
        }
        #endregion

        #region Gridview

        protected void gvProveedorGPS_RowDataBound(object sender, GridViewRowEventArgs e)
        {
    
        }


        /// <summary>
        /// Evento Producido al Cambiar la Expresión del Ordenamiento del GridView 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvProveedorGPS_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión del Ordenamiento
            lblOrdenado.Text = Controles.CambiaSortExpressionGridView(gvProveedorGPS, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression, true, 2);
        }

        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView "Soporte Tecnico"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamano_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando tamaño del GridView
            Controles.CambiaTamañoPaginaGridView(gvProveedorGPS, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamano.SelectedValue), true, 2);
        }

        /// <summary>
        /// Evento Producido al Cambiar el Indice de Página del GridView "Soporte Tecnico"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvProveedorGPS_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            Controles.CambiaIndicePaginaGridView(gvProveedorGPS, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, true, 2);
        }

        /// <summary>
        /// Evento Producido al Exportar al Contenido del GridView "Soporte Tecnico Detalles"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportar_Click(object sender, EventArgs e)
        {
            //Exportando Contenido del GridView
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "Id");
        }

        /// <summary>
        /// Eventos de los botones deltro del gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imbProveedorWSDiccionario_Click(object sender, ImageClickEventArgs e)
        {
            if (gvProveedorGPS.DataKeys.Count > 0)
            {
                Controles.SeleccionaFila(gvProveedorGPS, sender, "imb", true);
                ImageButton imb = (ImageButton)sender;
                this._id_proveedorWsDiccionario = Convert.ToInt32(gvProveedorGPS.SelectedDataKey["Id"]);
                CargaContenidoControles();
            }   
        }







        #endregion
    }
}