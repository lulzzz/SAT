using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Datos;
namespace SAT.UserControls
{
    public partial class wucControlDiesel : System.Web.UI.UserControl
    {
        #region Atributos
        /// <summary>
        /// Atributo que almacena los datos de control de diesel de las lectura de una unidad
        /// </summary>
        private DataTable _mit_ControlDieselLectura;
        /// <summary>
        /// Atributo que almacena los datos de control de diesel genrados por el sistema
        /// </summary>
        private DataTable _mit_ControlDieselSistema;
        /// <summary>
        /// Atributo que almacena el identificador de un operador
        /// </summary>
        private int _id_operador;
        /// <summary>
        /// Atributo que almacena el identificador de una liquidacion
        /// </summary>
        private int _id_liquidacion;
        /// <summary>
        /// Atributo que almacena la fecha de liquidación
        /// </summary>
        private DateTime _fecha_liquidacion;
        #endregion

        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            //Valida si se ha generado una carga previa del control
            if (!(Page.IsPostBack))
                //Invoca al método que inicializa valores del control de usuario
                cargaControlDiesel();
            else
                //Recupera los valores de los atributos
                recuperaAtributos();
        }
        /// <summary>
        /// Evento que realiza una carga previa del control de usuario.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            //Invoca al método Asignacion
            asignaAtributos();
        }

        #region Eventos GridView
        /// <summary>
        /// Evento producido al cambiar el indice de página del GV de Eventos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvControlDieselSistema_OnpageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Permite cambiar de indice de pagina acorde al tamaño del gridview
            Controles.CambiaIndicePaginaGridView(gvControlDieselSistema, this._mit_ControlDieselSistema, e.NewPageIndex, true, 2);
        }

        /// <summary>
        /// Eventó producido al pulsar el botón de exportación de contenido de GV de Eventos a excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarExcelControlDieselSistema_Onclick(object sender, EventArgs e)
        {
            //Invoca al metodoq ue permite exportar el gridview a formato de excel.
            Controles.ExportaContenidoGridView(this._mit_ControlDieselSistema, "Id");
        }
        /// <summary>
        /// Evento producido al cambiar el criterio de orden del GV de Eventos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvControlDieselSistema_Onsorting(object sender, GridViewSortEventArgs e)
        {
            //Permite el ordenamiento de las columnas de manera ascendente o descendente
            lblCriterioGridViewControlDieselSistema.Text = Controles.CambiaSortExpressionGridView(gvControlDieselSistema, this._mit_ControlDieselSistema, e.SortExpression, true, 1);
        }

        /// <summary>
        /// Evento producido al cambiar el indice de página del GV de Eventos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvControlDieselLectura_OnpageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Permite cambiar de indice de pagina acorde al tamaño del gridview
            Controles.CambiaIndicePaginaGridView(gvControlDieselLectura, this._mit_ControlDieselLectura, e.NewPageIndex, true, 1);
        }
        /// <summary>
        /// Eventó producido al pulsar el botón de exportación de contenido de GV de Eventos a excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarExcelControlDieselLectura_Onclick(object sender, EventArgs e)
        {
            //Invoca al metodoq ue permite exportar el gridview a formato de excel.
            Controles.ExportaContenidoGridView(this._mit_ControlDieselLectura, "Id");
        }
        /// <summary>
        /// Evento producido al cambiar el criterio de orden del GV de Eventos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvControlDieselLectura_Onsorting(object sender, GridViewSortEventArgs e)
        {
            //Permite el ordenamiento de las columnas de manera ascendente o descendente
            lblCriterioGridViewControlDieselLectura.Text = Controles.CambiaSortExpressionGridView(gvControlDieselLectura, this._mit_ControlDieselLectura, e.SortExpression, true, 1);
        }

        #endregion
        #endregion

        #region Métodos
        /// <summary>
        /// Método que inicializa los controles
        /// </summary>
        /// <param name="id_liquidacion">Identificador de la liquidacion</param>
        /// <param name="id_operador">Identificador del Operador</param>
        /// <param name="fecha_liquidacion">Fecha de la liquidación</param>
        public void InicializaControl(int id_liquidacion, int id_operador, DateTime fecha_liquidacion)
        {
            //Invoca al método encargado de inicializar valores del WUC Control diesel
            this.inicializaValores(id_liquidacion, id_operador, fecha_liquidacion);         
        }
        /// <summary>
        /// Método que inicializa los controles del WUC ControlDiesel
        /// </summary>
        /// <param name="id_liquidacion">Identificador de la liquidacion</param>
        /// <param name="id_operador">Identificador del Operador</param>
        /// <param name="fecha_liquidacion">Fecha de la liquidación</param>
        private void inicializaValores(int id_liquidacion, int id_operador, DateTime fecha_liquidacion)
        {
            this._id_liquidacion = id_liquidacion;
            this._id_operador = id_operador;
            this._fecha_liquidacion = fecha_liquidacion;
            cargaControlDiesel();
        }
        /// <summary>
        /// Método que consulta o estrae los valores de la página. 
        /// </summary> 
        private void recuperaAtributos()
        {
            //Valida el valor de la variable ViewState
            if (Convert.ToInt32(ViewState["idLiquidacion"]) != 0)
                //Si cumple la condición, asigna al atributo
                this._id_liquidacion = Convert.ToInt32(ViewState["idLiquidacion"]);
            //Valida el valor de la variable ViewState
            if (Convert.ToInt32(ViewState["idOperador"]) != 0)
                //Si cumple la condición, asigna al atributo
                this._id_operador = Convert.ToInt32(ViewState["idOperador"]);
            //Valida el valor de la variable ViewState
            if (Convert.ToDateTime(ViewState["fechaLiquidacion"]) != null)
                //Si cumple la condición, asigna al atributo
                this._fecha_liquidacion = Convert.ToDateTime(ViewState["fechaLiquidacion"]);
            //Valida el valor de la variable Viewstate
            if ((DataTable)ViewState["mitControlDieselLectura"] != null)
                //Si cumple la condición, asigna al atributo _mit_ControlDieselLectura el valor de la variable ViewState mitControlDieselLectura
                this._mit_ControlDieselLectura = (DataTable)ViewState["mitControlDieselLectura"];
            //Valida el valor de la variable Viewstate
            if ((DataTable)ViewState["mitControlDieselSistema"] != null)
                //Si cumple la condición, asigna al atributo _mit_ControlDieselLectura el valor de la variable ViewState mitControlDieselLectura
                this._mit_ControlDieselSistema = (DataTable)ViewState["mitControlDieselSistema"];
        }
        /// <summary>
        /// Método que alamcena los valores de la página
        /// </summary>
        private void asignaAtributos()
        {
            //Almacena en la variable viewState  el valor del atributos de la clase
            ViewState["idLiquidacion"] = this._id_liquidacion;
            ViewState["idOperador"] = this._id_operador;
            ViewState["fechaLiquidacion"] = this._fecha_liquidacion;
            ViewState["mitControlDieselLectura"] = this._mit_ControlDieselLectura;
            ViewState["mitControlDieselSistema"] = this._mit_ControlDieselSistema;
        }
        /// <summary>
        /// Método que carga los valores de los gridview
        /// </summary>
        private void cargaControlDiesel()
        {
            //Inicializa los indices de los gridView
            Controles.InicializaIndices(gvControlDieselSistema);
            Controles.InicializaIndices(gvControlDieselLectura);
            //Obtiene las lecturas de una unidad
            using (DataSet DS = SAT_CL.Liquidacion.Reportes.ControlDiesel(this._id_liquidacion, this._id_operador, this._fecha_liquidacion))
            {

                //Valida los datos del dataset
                if (Validacion.ValidaOrigenDatos(DS,"Table"))
                {
                    //Carga los controles
                    Controles.CargaGridView(gvControlDieselSistema, DS.Tables["Table"],"Id","",true,1);                                        
                    this._mit_ControlDieselSistema = DS.Tables["Table"];
                }
                //En caso contrario
                else
                {
                    Controles.InicializaGridview(gvControlDieselSistema);
                    //Eliminando Tablas del DataSet de Session
                    this._mit_ControlDieselSistema = null;
                }
                if (Validacion.ValidaOrigenDatos(DS, "Table1"))
                {
                    Controles.CargaGridView(gvControlDieselLectura, DS.Tables["Table1"], "Id", "", true, 1);
                    this._mit_ControlDieselLectura = DS.Tables["Table1"];
                }
                //En caso contrario
                else
                {
                    Controles.InicializaGridview(gvControlDieselLectura);
                    //Eliminando Tablas del DataSet de Session
                    this._mit_ControlDieselLectura = null;
                }
            }
            calculaKilometraje();
        }
        /// <summary>
        /// Método que calcula los totales de Kilometraje y Diesel
        /// </summary>
        private void calculaKilometraje()
        {
            //Valida los datos de las tablas 
            if (Validacion.ValidaOrigenDatos(this._mit_ControlDieselSistema))
            {
                decimal kilometros = Convert.ToDecimal(string.Format("{0:0.00}", Convert.ToDecimal(this._mit_ControlDieselSistema.Compute("SUM(Kms)", ""))));
                decimal litros = Convert.ToDecimal(string.Format("{0:0.00}", Convert.ToDecimal(this._mit_ControlDieselSistema.Compute("SUM(Litros)", ""))));  
                //Realiza al calculo y lo muestra el los label
                lblKilometros.Text = kilometros.ToString();
                lblLitros.Text = litros.ToString();

                if (kilometros > 0 && litros > 0)
                    lblRendimientoSistema.Text = string.Format("{0:0.000}",(kilometros / litros)) + "  Kms / l";
                else
                    lblRendimientoSistema.Text = "0.00";
            }
            else
            {
                //Muestra en cero
                lblKilometros.Text = "0.00";
                lblLitros.Text = "0.00";
                lblRendimientoSistema.Text = "0.00";
            }
            //Valida los datos de las tablas 
            if (Validacion.ValidaOrigenDatos(this._mit_ControlDieselLectura))
            {
                decimal kilometrosLectura = Convert.ToDecimal(string.Format("{0:0.00}", Convert.ToDecimal(this._mit_ControlDieselLectura.Compute("SUM(KmsLectura)", ""))));
                decimal litrosLectura = Convert.ToDecimal(string.Format("{0:0.00}", Convert.ToDecimal(this._mit_ControlDieselLectura.Compute("SUM(LitrosLectura)", ""))));
                lblKilometrosLectura.Text = kilometrosLectura.ToString();
                lblLitrosLectura.Text = litrosLectura.ToString();
                if (kilometrosLectura > 0 && litrosLectura > 0)
                    lblRendientolectura.Text = string.Format("{0.000}",(kilometrosLectura / litrosLectura)) + "  Kms / l";
                else
                    lblRendientolectura.Text = "0.00";
            }
            //En caso Contrario
            else
            {
                //Muestra en cero
                lblKilometrosLectura.Text = "0.00";
                lblLitrosLectura.Text = "0.00";
                lblRendientolectura.Text = "0.00";
            }           
        }
        #endregion
    }
}