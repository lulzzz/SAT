using SAT_CL;
using SAT_CL.Seguridad;
using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.Seguridad
{
    public partial class SeguridadPerfilUsuario : System.Web.UI.Page
    {
        #region Eventos
        /// <summary>
        /// Evento generado al cargar la Página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Aplicando seguridad de la forma
            SAT_CL.Seguridad.Forma.AplicaSeguridadForma(this, "content1", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
            //Si no es postback se inicializa la forma
            if (!Page.IsPostBack)
                inicializaForma();

        }

        /// <summary>
        /// Evento generado al cambiar el Nivel de Seguridad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlNivelDeSeguridad_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Carga Catalogo 
            cargaCatalogosPerfilUsuario();

            //Inicializa Grid View
            Controles.InicializaGridview(gvAcciones);
        }

        /// <summary>
        /// Evento producido al cambiar el indice de página del GV de Acciones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvAcciones_OnpageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Controles.CambiaIndicePaginaGridView(gvAcciones, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, true, 0);
        }
        /// <summary>
        /// Evento producido al cambiar el tamaño de página del GV de Eventos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvAcciones_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            Controles.CambiaTamañoPaginaGridView(gvAcciones, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamañoGridViewAcciones.SelectedValue), true, 0);
        }
        /// <summary>
        /// Eventó producido al pulsar el botón de exportación de contenido de GV de Acciones a excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarExcelAcciones_Onclick(object sender, EventArgs e)
        {
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "");

        }
        /// <summary>
        /// Evento producido al cambiar el criterio de orden del GV de Acciones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvAcciones_Onsorting(object sender, GridViewSortEventArgs e)
        {
            lblCriterioGridViewAcciones.Text = Controles.CambiaSortExpressionGridView(gvAcciones, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression, true, 0);
        }

        /// <summary>
        /// Evento generado al Cargar las Acciones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnConfigurar_Click(object sender, EventArgs e)
        {
            //cargando Lista de controles negados por perfil/usuario
            cargaControlesNegadosPerfilUsuario();
            //Carga Lista de Acciones
            cargaAcciones();
        }

        /// <summary>
        /// Evento generado al Cambiar el Perfil de Usuario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlPerfilUsuario_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Inicializa Grid View
            Controles.InicializaGridview(gvAcciones);
        }

        /// <summary>
        /// Evento generado al enlazar el gv Acciones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvAcciones_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Obtenemos la fila actual
            GridViewRow fila = e.Row;

            //Validamos Existan registros
            if (gvAcciones.DataKeys.Count > 0)
            {
                //Cargamos Acciones Negados
                using (DataTable mit = OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"))
                {
                    //Validamos Origen de Datos
                    if (mit != null)
                    {
                        //Verificamos el tipo de fila actual
                        switch (fila.RowType)
                        {
                            //Si es una fila de datos
                            case DataControlRowType.DataRow:
                                //Recuperando ChekBox         
                                using (CheckBox chkNegado = (CheckBox)fila.FindControl("chkNegado"))
                                {
                                    //Declaramos Variable
                                    int negado = (from DataRow r in mit.Rows
                                                  where Convert.ToInt32(r["IdAccion"]) == Convert.ToInt32(((DataRowView)e.Row.DataItem).Row["IdAccion"])
                                                  select Convert.ToInt32(r["IdAccion"])).FirstOrDefault();
                                    //Si existe la Negación del Control
                                    if (negado > 0)
                                        //Marcamos el control
                                        chkNegado.Checked = true;
                                }
                                break;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Click en cambiar permiso de seguridad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCambiarPermiso_Click(object sender, EventArgs e)
        {
            //Si existen registros
            if (gvAcciones.DataKeys.Count > 0)
            {
                //Seleccionando fila
                Controles.SeleccionaFila(gvAcciones, sender, "lnk", false);

                //Recuperando checkbox de acción
                CheckBox chk = (CheckBox)gvAcciones.SelectedRow.FindControl("chkNegado");
                //Validamos que  no sea Null
                if (chk != null)
                {
                    //Autorización del Control (Marcado = Negado), Si está negado se Pide Habilitar = 1
                    actualizaSeguridad(Convert.ToByte(gvAcciones.SelectedValue), chk.Checked ? 1: -1);
                }
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Método encargado de inicializar los Acciones generales de la forma
        /// </summary>
        private void inicializaForma()
        {
            //Cargando los catalogos
            cargaCatalogos();
            //Inicializa Grid View
            Controles.InicializaGridview(gvAcciones);
        }
        /// <summary>
        /// Carga Catalogo Perfil/Seguridad
        /// </summary>
        private void cargaCatalogosPerfilUsuario()
        {
            //Carga Catalogo de acuerdo al Tipo
            switch ((SAT_CL.Seguridad.ControlPerfilUsuario.Tipo)Convert.ToByte(ddlNivelDeSeguridad.SelectedValue))
            {
                //Perfiles
                case SAT_CL.Seguridad.ControlPerfilUsuario.Tipo.Perfil:
                    lblPerfilUsuario.Text = "Perfil";
                    CapaNegocio.m_capaNegocio.CargaCatalogo(ddlPerfilUsuario, 61, "", ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, "");
                    break;
                //Usuarios
                case SAT_CL.Seguridad.ControlPerfilUsuario.Tipo.Usuario:
                    lblPerfilUsuario.Text = "Usuario";
                    CapaNegocio.m_capaNegocio.CargaCatalogo(ddlPerfilUsuario, 62, "", ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, "");
                    break;
            }
        }
        /// <summary>
        /// Carga Catalogo Perfil/Seguridad
        /// </summary>
        private void cargaControlesNegadosPerfilUsuario()
        {
            //Declaramos Tabla
            DataTable mit = null;
            //Carga Catalogo de acuerdo al Tipo
            switch ((SAT_CL.Seguridad.ControlPerfilUsuario.Tipo)Convert.ToByte(ddlNivelDeSeguridad.SelectedValue))
            {
                //Perfiles
                case SAT_CL.Seguridad.ControlPerfilUsuario.Tipo.Perfil:
                    //Cargamos Controles Negados
                    mit = ControlPerfilUsuario.CargaControlesNegadosPerfil(Convert.ToInt32(ddlPerfilUsuario.SelectedValue));

                    break;
                //Usuarios
                case SAT_CL.Seguridad.ControlPerfilUsuario.Tipo.Usuario:
                    //Cargamos Controles Negados
                    mit = ControlPerfilUsuario.CargaControlesNegadosUsuario(Convert.ToInt32(ddlPerfilUsuario.SelectedValue), 0);
                    break;
            }

            //Si hay resultados, se guardan en sesión
            if (mit != null)
                Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], mit, "Table1");
            else
                //De lo contrario eliminando tabla previa
                Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
        }
        /// <summary>
        /// Método encargado de actualizar la seguridad
        /// </summary>
        /// <param name="id_accion"></param>
        /// <param name="valor"></param>
        private void actualizaSeguridad(byte id_accion, decimal valor)
        {
            //Declarando variable para verificar resultado
            RetornoOperacion resultado = new RetornoOperacion();

            switch ((SAT_CL.Seguridad.ControlPerfilUsuario.Tipo)Convert.ToByte(ddlNivelDeSeguridad.SelectedValue))
            {
                //Perfil
                case SAT_CL.Seguridad.ControlPerfilUsuario.Tipo.Perfil:

                    resultado = ControlPerfilUsuario.AutorizaControlPerfil(id_accion, Convert.ToInt32(ddlPerfilUsuario.SelectedValue),
                                              valor, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                    break;
                //Usuario
                case SAT_CL.Seguridad.ControlPerfilUsuario.Tipo.Usuario:

                    ControlPerfilUsuario.AutorizaControlUsuario(id_accion, Convert.ToInt32(ddlPerfilUsuario.SelectedValue),
                                              valor, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                    break;

            }

            //Personalizando resultado por acción
            resultado = new RetornoOperacion(resultado.IdRegistro, string.Format("{0}: {1}", SAT_CL.Global.Catalogo.RegresaDescripcionCatalogo(3137, id_accion), resultado.Mensaje), resultado.OperacionExitosa);

            //Si no hay errores, se actualiza información desde bd
            if (resultado.OperacionExitosa)
            {
                //Cargando lista de controles negados
                cargaControlesNegadosPerfilUsuario();
                //Cargando lista de acciones
                cargaAcciones();
            }

            //Mostramos Resultado
            TSDK.ASP.ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Carga Catalogoa
        /// </summary>
        private void cargaCatalogos()
        {
            //Tipo
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlNivelDeSeguridad, "", 3136);

            //Tamaño
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamañoGridViewAcciones, "", 26);

            //Carga Perfil Usuario
            cargaCatalogosPerfilUsuario();
        }
        /// <summary>
        /// Realiza la carga de los Acciones 
        /// </summary>
        private void cargaAcciones()
        {
            //Inicializando indices
            Controles.InicializaIndices(gvAcciones);

            //Carga Acciones
            using (DataTable dt = SAT_CL.Seguridad.Control.CargaAcciones())
            {
                //Cargando GridView 
                Controles.CargaGridView(gvAcciones, dt, "IdAccion", lblCriterioGridViewAcciones.Text, true, 0);
                //Validando que la Tabla no sea null
                if (dt != null)
                    //Añadiendo Tabla a DataSet de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dt, "Table");
                else
                    //Eliminamos Tabla  del DataSet Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
            }
        }

        #endregion

    }
}