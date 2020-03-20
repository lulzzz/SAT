using SAT_CL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;

namespace SAT.UserControls
{
    public partial class wucBitacora : System.Web.UI.UserControl
    {
        #region Atributos

        /// <summary>
        /// Id de Tabla a consultar
        /// </summary>
        private int _idTabla;
        /// <summary>
        /// Id de Registro a consultar
        /// </summary>
        private int _idRegistro;
        /// <summary>
        /// DataSet con los registros a mostrar
        /// </summary>
        private DataSet _ds;
        /// <summary>
        /// Titulo de bitácora
        /// </summary>
        private string _tituloBitacora;

        #endregion
        
        #region Eventos

        /// <summary>
        /// Evento Producido al efectuarse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {   //Validando si se Produjo un PostBack
            if (!(Page.IsPostBack))
                //Invocando  Método de Asignación
                asignaAtributos();
            else//Invocando  Método de Recuperación
                recuperaAtributos();
        }
        /// <summary>
        /// Evento desencadenado antes de Producirse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {   //Invocando Método de Asignación
            asignaAtributos();
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Buscar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {   
            //Validando que las fechas sean Validas
            if (Convert.ToDateTime(txtFecIni.Text).CompareTo(Convert.ToDateTime(txtFecFin.Text)) < 1)
                
                //Obteniendo Registros
                this._ds = SAT_CL.Global.Bitacora.CargaBitacoraControl(this._idTabla, this._idRegistro, Convert.ToDateTime(txtFecIni.Text),
                            Convert.ToDateTime(txtFecFin.Text), Convert.ToInt32(ddlTipo.SelectedValue), gvResultado, "Table", "Id", lblCriterio.Text);
            else
                //Mostrando Mensaje
                lblError.Text = "La Fecha de Inicio es mayor a la Fecha de Fin";
        }

        #region Eventos GridView "Resultados"

        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamano_SelectedIndexChanged(object sender, EventArgs e)
        {   //Validando si existen Registros
            if(gvResultado.DataKeys.Count > 0)
            {   //Asignando Expresion de Ordenamiento
                this._ds.Tables["Table"].DefaultView.Sort = lblCriterio.Text;
                //Cambiando Tamaño del GridView
                Controles.CambiaTamañoPaginaGridView(gvResultado, this._ds.Tables["Table"], Convert.ToInt32(ddlTamano.SelectedValue), true, 1);
            }
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Página del GridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvResultado_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {   //Validando si existen Registros
            if (gvResultado.DataKeys.Count > 0)
            {   //Asignando Expresion de Ordenamiento
                this._ds.Tables["Table"].DefaultView.Sort = lblCriterio.Text;
                //Cambiando Indice de Página del GridView
                Controles.CambiaIndicePaginaGridView(gvResultado, this._ds.Tables["Table"], e.NewPageIndex, true, 1);
            }
        }
        /// <summary>
        /// Evento Producido al Cambiar la Expresión de Ordenamiento del GridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvResultado_Sorting(object sender, GridViewSortEventArgs e)
        {   //Validando si existen Registros
            if (gvResultado.DataKeys.Count > 0)
            {   //Asignando Expresion de Ordenamiento
                this._ds.Tables["Table"].DefaultView.Sort = lblCriterio.Text;
                //Cambiando Indice de Página del GridView
                lblCriterio.Text = Controles.CambiaSortExpressionGridView(gvResultado, this._ds.Tables["Table"], e.SortExpression, true, 1);
            }
        }
        /// <summary>
        /// Evento Producido a Presionar el LinkButton "Exportar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportar_Click(object sender, EventArgs e)
        {   //Validando si existen Registros
            if (gvResultado.DataKeys.Count > 0)
                //Exportando Contenido del GridView
                Controles.ExportaContenidoGridView(this._ds.Tables["Table"], "Id","IdRegistro");
        }

        #endregion

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Privado encargado de Inicializar los Controles
        /// </summary>
        private void inicializaControl()
        {   
            //Cargando Catalogos
            CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipo, 12, "Todos", _idTabla, "", 0, "");
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamano, "", 18);
            //Asignando Fechas
            txtFecIni.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().AddDays(-7).ToString("dd/MM/yyyy HH:mm");
            txtFecFin.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().AddMinutes(3).ToString("dd/MM/yyyy HH:mm");
            //Obteniendo Registros
            this._ds = SAT_CL.Global.Bitacora.CargaBitacoraControl(this._idTabla, this._idRegistro, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().AddDays(-7), TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().AddMinutes(3),
                        Convert.ToInt32(ddlTipo.SelectedValue), gvResultado, "Table", "Id", lblCriterio.Text);
        }
        /// <summary>
        /// Método Privado encargado de Asignar los Atributos
        /// </summary>
        private void asignaAtributos()
        {   //Asignando Valores
            ViewState["idTabla"] = this._idTabla;
            ViewState["idRegistro"] = this._idRegistro;
            ViewState["DS"] = this._ds;
        }
        /// <summary>
        /// Método Privado encargado de Recuperar los Atributos
        /// </summary>
        private void recuperaAtributos()
        {   //Recuperando Valores
            if (Convert.ToInt32(ViewState["idTabla"]) != 0)
                this._idTabla = Convert.ToInt32(ViewState["idTabla"]);
            if (Convert.ToInt32(ViewState["idRegistro"]) != 0)
                this._idRegistro = Convert.ToInt32(ViewState["idRegistro"]);
            if (TSDK.Datos.Validacion.ValidaOrigenDatos((DataSet)ViewState["DS"], "Table"))
                this._ds = (DataSet)ViewState["DS"];
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método Público encargado de Inicializar el Control
        /// </summary>
        /// <param name="idRegistro"></param>
        /// <param name="id_tabla"></param>
        /// <param name="nombre_tabla"></param>
        public void InicializaControlUsuario(int idRegistro, int id_tabla, string nombre_tabla)
        {   //Asignando Id de registro
            this._idRegistro = idRegistro;
            this._idTabla = id_tabla;
            this._tituloBitacora = nombre_tabla;
            //Inicializamos el control
            inicializaControl();
        }


        #endregion
    }
}