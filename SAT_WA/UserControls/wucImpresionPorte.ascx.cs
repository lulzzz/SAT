using SAT_CL;
using System;
using System.Web.UI;
using TSDK.ASP;
using TSDK.Base;

namespace SAT.UserControls
{
    public partial class wucImpresionPorte : System.Web.UI.UserControl
    {
        #region Enumeraciones

        /// <summary>
        /// Enumeración que expresa el Tipo de Impresión
        /// </summary>
        public enum TipoImpresion
        {
            CartaPorte = 1,

            BitacoraViaje
        }

        #endregion

        #region Atributos

        /// <summary>
        /// Atributo encargado de Almacenar el Identificador de la Factura Electronica
        /// </summary>
        private int _id_servicio;
        /// <summary>
        /// Atributo encargado de Almacenar el Tipo de Impresión del Servicio
        /// </summary>
        private TipoImpresion _tipo;

        #endregion

        #region Eventos

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Validando que se Produjo un PostBack
            if (!(Page.IsPostBack))
            {
                //Cargando Catalogos
                cargaCatalogo();
                //Invocando Método de Asignación
                asignaValores();
            }
            else
                //Recuperando Atributos
                recuperaValores();
        }
        /// <summary>
        /// Evento Producido antes de Efectuarse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            //Invocando Método de Asignación
            asignaValores();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imbImprimir_Click(object sender, ImageClickEventArgs e)
        {
            //Validando Evento
            if (ClickImprimirCartaPorte != null)
                //Invocando Método Virtual
                OnClickImprimirCartaPorte(new EventArgs());
        }

        #endregion

        #region Manejadores de Eventos

        /// <summary>
        /// Manejado de Evento para la Impresión de la Carta Porte
        /// </summary>
        public event EventHandler ClickImprimirCartaPorte;
        /// <summary>
        /// Método virtual encargado de Manipular el Manejador de Evento
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnClickImprimirCartaPorte(EventArgs e)
        {
            //Validando Existencia
            if (ClickImprimirCartaPorte != null)
                //Instanciando Evento
                ClickImprimirCartaPorte(this, e);
        }

        #endregion

        #region Metodos Privados

        /// <summary>
        /// 
        /// </summary>
        private void inicializaImpresionCartaPorte()
        {
            //Validando Existencia de la Nota
            if (this._id_servicio > 0)
            {
                //Instanciando Servicio
                using (SAT_CL.Documentacion.Servicio serv = new SAT_CL.Documentacion.Servicio(this._id_servicio))
                {
                    //Validando Servicio
                    if (serv.habilitar)
                    {
                        //Validando Tipo
                        switch (this._tipo)
                        {
                            case TipoImpresion.BitacoraViaje:
                                {
                                    //Asignando Texto
                                    lblEncabezado.Text = "Seleccione los recursos a mostrar en la Bitacora de Viaje";
                                    break;
                                }
                            case TipoImpresion.CartaPorte:
                                {
                                    //Asignando Texto
                                    lblEncabezado.Text = "Seleccione los recursos a mostrar en la Carta Porte";
                                    break;
                                }
                        }

                        //Cargando Catalogos
                        cargaCatalogo();
                    }
                    else
                        //Inicializando Catalogos
                        inicializaCatalogos();
                }
            }
            else
                //Inicializando Catalogos
                inicializaCatalogos();
        }
        /// <summary>
        /// Método encargado de Cargar los Catalogos
        /// </summary>
        private void cargaCatalogo()
        {
            //Operador
            CapaNegocio.m_capaNegocio.CargaCatalogo(ddlOperador, 188, "-- Seleccione un Operador", this._id_servicio, "", 0, "");
            //Unidades Motrices
            CapaNegocio.m_capaNegocio.CargaCatalogo(ddlUnidadMotriz, 189, "-- Seleccione una Unidad Motriz", this._id_servicio, "", 1, "");
            //Unidades de Arrastre 1
            CapaNegocio.m_capaNegocio.CargaCatalogo(ddlUnidadArrastre1, 189, "-- Seleccione la 1er Unidad de Arrastre", this._id_servicio, "", 2, "");
            //Unidades Motrices
            CapaNegocio.m_capaNegocio.CargaCatalogo(ddlUnidadArrastre2, 189, "-- Seleccione la 2da Unidad de Arrastre", this._id_servicio, "", 2, "");
        }

        private void inicializaCatalogos()
        {
            //Inicializando Catalogos
            Controles.InicializaDropDownList(ddlOperador, "Ninguno");
            Controles.InicializaDropDownList(ddlUnidadMotriz, "Ninguno");
            Controles.InicializaDropDownList(ddlUnidadArrastre1, "Ninguno");
            Controles.InicializaDropDownList(ddlUnidadArrastre2, "Ninguno");
        }
        /// <summary>
        /// 
        /// </summary>
        private void recuperaValores()
        {
            //Validando Datos
            if (Convert.ToInt32(ViewState["idServicio"]) > 0)
                //Asignando Valor
                this._id_servicio = Convert.ToInt32(ViewState["idServicio"]);
            //Validando Tipo
            if (Convert.ToInt32(ViewState["tipo"]) > 0)
                //Asignando Valor
                this._tipo = (TipoImpresion)Convert.ToInt32(ViewState["tipo"]);
        }
        /// <summary>
        /// 
        /// </summary>
        private void asignaValores()
        {
            //Asignando Valores
            ViewState["idServicio"] = this._id_servicio;
            ViewState["tipo"] = Convert.ToInt32(this._tipo);
        }

        #endregion

        #region Metodos Públicos

        /// <summary>
        /// Método encargado 
        /// </summary>
        /// <param name="id_servicio"></param>
        public void InicializaImpresionCartaPorte(int id_servicio, TipoImpresion tipo)
        {
            //Asignando Valores
            this._id_servicio = id_servicio;
            this._tipo = tipo;
            //Inicializando Control
            inicializaImpresionCartaPorte();
        }
        /// <summary>
        /// Método encargado de Imprimir la Carta Porte
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion ImprimeCartaPorte()
        {
            //Declarando Objeto de Retorno
            RetornoOperacion retorno = new RetornoOperacion();

            //Validando Servicio
            using (SAT_CL.Documentacion.Servicio serv = new SAT_CL.Documentacion.Servicio(this._id_servicio))
            {
                //Validando Servicio
                if (serv.habilitar)
                {
                    //Obtiene Ruta Absoluta
                    string urlReporte = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/UserControls/wucImpresionPorte.ascx", "~/RDLC/Reporte.aspx");

                    //Instanciando nueva ventana de navegador para apertura de registro
                    TSDK.ASP.ScriptServer.AbreNuevaVentana(string.Format("{0}?idTipoReporte={1}&idRegistro={2}&idRegistroB={3}&idRegistroC={4}&idRegistroD={5}", 
                            urlReporte, this._tipo == TipoImpresion.CartaPorte ? "Porte" : "BitacoraViaje", serv.id_servicio, ddlOperador.SelectedValue, ddlUnidadMotriz.SelectedValue, ddlUnidadArrastre1.SelectedValue, ddlUnidadArrastre2.SelectedValue), 
                            "Carta Porte", "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500", Page);
                }
                else
                    //Mostrando Excepción
                    retorno = new RetornoOperacion("No se puede recuperar el Servicio");
            }

            //Devolviendo Resultado Obtenido
            return retorno;
        }

        #endregion
    }
}