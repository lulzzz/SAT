using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.Base;

namespace SAT.UserControls
{
    public partial class wucKilometraje : System.Web.UI.UserControl
    {
        #region Atributos

        private int _id_kilometraje;
        private int _id_compania;
        private int _id_ubicacion_origen;
        private int _id_ubicacion_destino;

        /// <summary>
        /// Propiedad que establece el Orden de la Tabulación de los Controles
        /// </summary>
        public short TabIndex
        {
            set
            {   //Asignando Orden
                txtUbicacionOrigen.TabIndex =
                txtUbicacionDestino.TabIndex =
                ddlRuta.TabIndex =
                txtKms.TabIndex =
                txtTiempo.TabIndex =
                txtKmsMaps.TabIndex =
                txtTiempoMaps.TabIndex =
                chkIndicador.TabIndex =
                txtKmsInv.TabIndex =
                txtTiempoInv.TabIndex =
                btnAceptar.TabIndex = value;
            }
            get { return txtUbicacionOrigen.TabIndex; }
        }
        /// <summary>
        /// Almacena el Nombre del Contenedor
        /// </summary>
        public string Contenedor;

        #endregion

        #region Eventos

        /// <summary>
        /// Evento Producido al Efectuarse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Validando que se produzca un PostBack
            if (!(Page.IsPostBack))
                
                //Asignando Atributos
                asignaAtributos();
            else
                //Recuperando Atributos
                recuperaAtributos();
        }
        /// <summary>
        /// Evento desencadenado antes de Producirse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {   
            //Invocando Método de Asignación
            asignaAtributos();
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Aceptar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            //Validando que exista un Evento
            if (ClickGuardar != null)
                
                //Iniciando Manejador
                OnClickGuardar(e);
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Cancelar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            //Limpiando Controles
            limpiaControles();
        }
        /// <summary>
        /// Evento Producido al Presionar 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkCalcular_Click(object sender, EventArgs e)
        {
            //Asignando Kilometraje
            txtKmsMaps.Text = txtKms.Text;
        }
        /// <summary>
        /// Evento Producido al Marcar el Control "Indicador"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkIndicador_CheckedChanged(object sender, EventArgs e)
        {
            //Invocando Método de Configuración
            configuraControlesIndicador(chkIndicador.Checked);
        }

        #region Eventos "Kilometraje de Pago y Cobro"

        /// <summary>
        /// Evento Producido al Marcar o Desmarcar el Control CheckBox "chkKmsPC" o "chkKmsInvPC"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkKmsPC_CheckedChanged(object sender, EventArgs e)
        {
            //Obteniendo Control
            CheckBox chk = (CheckBox)sender;
            
            //Validando Control
            switch (chk.ID)
            {
                case "chkKmsPC":
                    {
                        //Validando Kilometraje Real
                        if (chk.Checked)
                        {
                            //Asignando Valores
                            txtKmsPago.Text =
                            txtKmsCobro.Text = txtKms.Text;

                            //Deshabilitando Controles
                            txtKmsPago.Enabled =
                            txtKmsCobro.Enabled = false;
                        }
                        else
                        {
                            //Habilitando Controles
                            txtKmsPago.Enabled =
                            txtKmsCobro.Enabled = true;
                        }

                        break;
                    }
                case "chkKmsInvPC":
                    {
                        //Validando Kilometraje Real
                        if (chk.Checked)
                        {
                            //Asignando Valores
                            txtKmsInvPago.Text =
                            txtKmsInvCobro.Text = txtKms.Text;

                            //Deshabilitando Controles
                            txtKmsInvPago.Enabled =
                            txtKmsInvCobro.Enabled = false;
                        }
                        else
                        {
                            //Habilitando Controles
                            txtKmsInvPago.Enabled =
                            txtKmsInvCobro.Enabled = true;
                        }

                        break;
                    }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnConfirmarKms_Click(object sender, EventArgs e)
        {
            //Obteniendo Control
            Button btn = (Button)sender;
            
            //Validando Control
            switch(btn.ID)
            {
                case "btnConfirmarKms":
                    {
                        //Ocultando Ventana Modal
                        TSDK.ASP.ScriptServer.AlternarVentana(upbtnConfirmarKms, upbtnConfirmarKms.GetType(), "VentanaKms", "contenidoVentanaKmsPagoCobro", "ventanaKmsPagoCobro");
                        break;
                    }
                case "btnConfirmarKmsInv":
                    {
                        //Ocultando Ventana Modal
                        TSDK.ASP.ScriptServer.AlternarVentana(upbtnConfirmarKmsInv, upbtnConfirmarKmsInv.GetType(), "VentanaKmsInv", "contenidoVentanaKmsInvPagoCobro", "ventanaKmsInvPagoCobro");
                        break;
                    }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkOtrosKms_Click(object sender, EventArgs e)
        {
            //Obteniendo Control
            LinkButton lnk = (LinkButton)sender;

            //Validando Control
            switch(lnk.ID)
            {
                case "lnkOtrosKmsReal":
                    {
                        //Asignando Control Positivo
                        chkKmsPC.Checked = true;
                        
                        //Configurando Controles
                        configuraKilometrajesPC();

                        //Ocultando Ventana Modal
                        TSDK.ASP.ScriptServer.AlternarVentana(uplnkOtrosKmsReal, uplnkOtrosKmsReal.GetType(), "VentanaKms", "contenidoVentanaKmsPagoCobro", "ventanaKmsPagoCobro");
                        break;
                    }
                case "lnkOtrosKmsInv":
                    {
                        //Asignando Control Positivo
                        chkKmsInvPC.Checked = true;

                        //Obteniendo Valor
                        txtKmsInv.Text = txtKmsInv.Text == "" ? txtKms.Text : txtKmsInv.Text;
                        
                        //Configurando Controles
                        configuraKilometrajesInvPC();

                        //Ocultando Ventana Modal
                        TSDK.ASP.ScriptServer.AlternarVentana(uplnkOtrosKmsInv, uplnkOtrosKmsInv.GetType(), "VentanaKmsInv", "contenidoVentanaKmsInvPagoCobro", "ventanaKmsInvPagoCobro");
                        break;
                    }
            }
        }

        #endregion

        #endregion

        #region Manejadores de Eventos

        /// <summary>
        /// Manejador de Evento "Guardar"
        /// </summary>
        public event EventHandler ClickGuardar;
        /// <summary>
        /// Evento que Manipula el Manejador "Guardar"
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnClickGuardar(EventArgs e)
        {
            //Validando que exista el Evento
            if (ClickGuardar != null)
                //Iniciando Evento
                ClickGuardar(this, e);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método encargado de Inicializar el Control
        /// </summary>
        /// <param name="id_kilometraje">Kilometraje</param>
        /// <param name="id_compania">Compania Emisora</param>
        /// <param name="id_ubicacion_ori">Ubicación de Origen</param>
        /// <param name="id_ubicacion_dest">Ubicación de Destino</param>
        private void inicializaControlUsuario(int id_kilometraje, int id_compania, int id_ubicacion_ori, int id_ubicacion_dest)
        {
            //Invocando Carga de Catalogos
            cargaCatalogos();

            //Asignando Atributos
            this._id_kilometraje = id_kilometraje;
            this._id_compania = id_compania;

            //Limpiando Control
            lblError.Text = "";

            //Validando que Exista el Kilometraje
            if (this._id_kilometraje > 0)
            {
                //Instanciando Kilometraje
                using (SAT_CL.Global.Kilometraje km = new SAT_CL.Global.Kilometraje(id_kilometraje))
                {
                    //Validando que Exista el Kilometraje
                    if (km.id_kilometraje > 0)
                    {
                        //Asignando Valores
                        txtUbicacionOrigen.Text = obtieneDescripcionUbicacion(km.id_ubicacion_origen);
                        txtUbicacionDestino.Text = obtieneDescripcionUbicacion(km.id_ubicacion_destino);
                        txtKms.Text = km.kms_reales.ToString();
                        txtTiempo.Text = km.tiempo_real.ToString();
                        txtKmsMaps.Text = km.kms_maps.ToString();
                        txtTiempoMaps.Text = km.tiempo_maps.ToString();

                        //Kilometraje de Pago y Cobro
                        txtKmsPago.Text = km.kms_pago.ToString();
                        txtKmsCobro.Text = km.kms_cobro.ToString();

                        //Asignando Ubicaciones OyD
                        this._id_ubicacion_origen = km.id_ubicacion_origen;
                        this._id_ubicacion_destino = km.id_ubicacion_destino;

                        //Instanciando Kilometraje Inverso
                        using (SAT_CL.Global.Kilometraje kmsInv = new SAT_CL.Global.Kilometraje(km.id_ubicacion_destino, km.id_ubicacion_origen, km.id_ruta, km.id_compania_emisor))
                        {
                            //Validando que exista el Kilometraje
                            if(kmsInv.id_kilometraje > 0)
                            {
                                //Kilometraje y Tiempo Real Inverso
                                txtKmsInv.Text = kmsInv.kms_reales.ToString();
                                txtTiempoInv.Text = kmsInv.tiempo_real.ToString();
                                //Kilometraje de Pago y Cobro del Kms Inverso
                                txtKmsInvPago.Text = kmsInv.kms_pago.ToString();
                                txtKmsInvCobro.Text = kmsInv.kms_cobro.ToString();
                            }
                            else
                            {
                                //Kilometraje y Tiempo Real Inverso
                                txtKmsInv.Text = 
                                txtTiempoInv.Text =
                                //Kilometraje de Pago y Cobro del Kms Inverso
                                txtKmsInvPago.Text =
                                txtKmsInvCobro.Text = "0.00";
                            }
                        }
                    }
                }
            }
            else
            {
                //Invocando Método de Limpieza
                limpiaControles();
                
                //Asignando Valores
                txtUbicacionOrigen.Text = obtieneDescripcionUbicacion(id_ubicacion_ori);
                txtUbicacionDestino.Text = obtieneDescripcionUbicacion(id_ubicacion_dest);

                //Validando que Exista una Ubicación
                if(txtUbicacionOrigen.Text != "")
                    //Asignando Ubicación Origen
                    this._id_ubicacion_origen = id_ubicacion_ori;
                else
                    //Quitando Ubicación Origen
                    this._id_ubicacion_origen = 0;

                //Validando que Exista una Ubicación
                if (txtUbicacionDestino.Text != "")
                    //Asignando Ubicación Destino
                    this._id_ubicacion_destino = id_ubicacion_dest;
                else
                    //Quitando Ubicación Origen
                    this._id_ubicacion_destino = 0;
            }

            //Configurando Controles
            chkIndicador.Checked = 
            chkKmsPC.Checked = 
            chkKmsInvPC.Checked = true;

            //Invocando Métodos de Configuración
            configuraControlesIndicador(true);
            configuraKilometrajesPC();
            configuraKilometrajesInvPC();
        }
        /// <summary>
        /// Método encargado de Asignar el Valor de los Atributos
        /// </summary>
        private void asignaAtributos()
        {
            //Asignando Atributos
            ViewState["idKilometraje"] = this._id_kilometraje;
            ViewState["idCompania"] = this._id_compania;
            ViewState["idUbicacionOrigen"] = this._id_ubicacion_origen;
            ViewState["idUbicacionDestino"] = this._id_ubicacion_destino;
        }
        /// <summary>
        /// Método encargado de Recuperar el Valor de los Atributos
        /// </summary>
        private void recuperaAtributos()
        {
            //Validando si Existe el Kilometraje
            if (Convert.ToInt32(ViewState["idKilometraje"]) != 0)
                //Asignando Kilometraje
                this._id_kilometraje = Convert.ToInt32(ViewState["idKilometraje"]);
            //Validando si Existe el Kilometraje
            if (Convert.ToInt32(ViewState["idCompania"]) != 0)
                //Asignando Kilometraje
                this._id_compania = Convert.ToInt32(ViewState["idCompania"]);
            //Validando si Existe el Kilometraje
            if (Convert.ToInt32(ViewState["idUbicacionOrigen"]) != 0)
                //Asignando Kilometraje
                this._id_ubicacion_origen = Convert.ToInt32(ViewState["idUbicacionOrigen"]);
            //Validando si Existe el Kilometraje
            if (Convert.ToInt32(ViewState["idUbicacionDestino"]) != 0)
                //Asignando Kilometraje
                this._id_ubicacion_destino = Convert.ToInt32(ViewState["idUbicacionDestino"]);
        }
        /// <summary>
        /// Método encargado de Cargar los Catalogos
        /// </summary>
        private void cargaCatalogos()
        {
            //Inicializando GridView
            TSDK.ASP.Controles.InicializaDropDownList(ddlRuta, "-- Seleccione una Ruta");
        }
        /// <summary>
        /// Método encargado de Limpiar los Controles
        /// </summary>
        private void limpiaControles()
        {
            //Limpiando Controles
            txtUbicacionDestino.Text =
            txtUbicacionOrigen.Text = "";
            txtKms.Text =
            txtKmsMaps.Text = 
            txtKmsInv.Text = "0";
            txtTiempo.Text =
            txtTiempoMaps.Text = 
            txtTiempoInv.Text = "0";
        }
        /// <summary>
        /// Método encargado de Obtener las Ubicaciónes
        /// </summary>
        /// <param name="id_ubicacion">Ubicación</param>
        /// <returns></returns>
        private string obtieneDescripcionUbicacion(int id_ubicacion)
        {
            //Declarando Objeto de Retorno
            string descripcion_ubicacion = "";

            //Instanciando la Ubicación
            using(SAT_CL.Global.Ubicacion ubi = new SAT_CL.Global.Ubicacion(id_ubicacion))
            {
                //Validando que Exista la Ubicación
                if (ubi.id_ubicacion > 0)

                    //Asignando la Descripción de la Ubicación
                    descripcion_ubicacion = string.Format("{0} ID:{1}", ubi.descripcion, ubi.id_ubicacion);
                else
                    //Limpiando Control
                    descripcion_ubicacion = "";
            }

            //Devolviendo Resultado Obtenido
            return descripcion_ubicacion;
        }
        /// <summary>
        /// Método encargado de Configurar los Controles según el Indicador
        /// </summary>
        /// <param name="indicador">Indicador de los Controles</param>
        private void configuraControlesIndicador(bool indicador)
        {
            //Configurando Controles
            txtKmsInv.Enabled = txtTiempoInv.Enabled = lnkOtrosKmsInv.Enabled = !indicador;
        }
        /// <summary>
        /// Método encargado de Configurar los Controles de Kilometrajes de Pago y Cobro
        /// </summary>
        private void configuraKilometrajesPC()
        {
            //Instanciando Kilometraje
            using (SAT_CL.Global.Kilometraje kms = new SAT_CL.Global.Kilometraje(this._id_kilometraje))
            {
                //Validando que Existe un Kilometraje
                if (this._id_kilometraje > 0)
                {
                    //Validando que el Kilometraje de Pago y Cobro sea distinto al Real
                    if (!txtKms.Text.Equals(txtKmsPago.Text) || !txtKms.Text.Equals(txtKmsCobro.Text))
                    {                        
                        //Desmarcando Indicador
                        chkKmsPC.Checked = false;
                        //Habilitando Controles
                        txtKmsPago.Enabled =
                        txtKmsCobro.Enabled = true;
                    }
                    else
                    {
                        //Desmarcando Indicador
                        chkKmsPC.Checked = true;
                        //Habilitando Controles
                        txtKmsPago.Enabled =
                        txtKmsCobro.Enabled = false;
                        
                        //Asignando Valores
                        txtKmsPago.Text =
                        txtKmsCobro.Text = txtKms.Text;
                    }
                }
                else
                {
                    //Validando Kilometraje Real
                    if (chkKmsPC.Checked)
                    {
                        //Asignando Valores
                        txtKmsPago.Text = 
                        txtKmsCobro.Text = txtKms.Text;

                        //Deshabilitando Controles
                        txtKmsPago.Enabled =
                        txtKmsCobro.Enabled = false;
                    }
                    else
                    {
                        //Habilitando Controles
                        txtKmsPago.Enabled =
                        txtKmsCobro.Enabled = true;
                    }
                }
            }            
        }
        /// <summary>
        /// Método encargado de Configurar los Controles de Kilometrajes de Pago y Cobro (Inverso)
        /// </summary>
        private void configuraKilometrajesInvPC()
        {
            //Instanciando Kilometraje Inverso
            using (SAT_CL.Global.Kilometraje kms = SAT_CL.Global.Kilometraje.ObtieneKilometrajeInverso(this._id_kilometraje))
            {
                //Validando que Existe un Kilometraje
                if (this._id_kilometraje > 0)
                {
                    //Validando que el Kilometraje de Pago y Cobro sea distinto al Real
                    if (!kms.kms_reales.ToString().Equals(txtKmsInvPago.Text) || !kms.kms_reales.ToString().Equals(txtKmsInvCobro.Text))
                    {
                        //Desmarcando Indicador
                        chkKmsInvPC.Checked = false;
                        //Habilitando Controles
                        txtKmsInvPago.Enabled =
                        txtKmsInvCobro.Enabled = true;
                    }
                    else
                    {
                        //Desmarcando Indicador
                        chkKmsInvPC.Checked = true;
                        //Habilitando Controles
                        txtKmsInvPago.Enabled =
                        txtKmsInvCobro.Enabled = false;
                        //Asignando Valores
                        txtKmsInvPago.Text =
                        txtKmsInvCobro.Text = kms.kms_reales.ToString();
                    }
                }
                else
                {
                    //Validando Kilometraje Real
                    if (chkKmsInvPC.Checked)
                    {
                        //Asignando Valores
                        txtKmsInvPago.Text =
                        txtKmsInvCobro.Text = txtKmsInv.Text;

                        //Deshabilitando Controles
                        txtKmsInvPago.Enabled =
                        txtKmsInvCobro.Enabled = false;
                    }
                    else
                    {
                        //Habilitando Controles
                        txtKmsInvPago.Enabled =
                        txtKmsInvCobro.Enabled = true;
                    }
                }
            }
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método encargado de Inicializar el Control
        /// </summary>
        /// <param name="id_kilometraje">Kilometraje</param>
        /// <param name="id_compania">Compania Emisora</param>
        public void InicializaControlKilometraje(int id_kilometraje, int id_compania)
        {
            //Invocando Método de Inicialización
            inicializaControlUsuario(id_kilometraje, id_compania, 0, 0);
        }
        /// <summary>
        /// Método encargado de Inicializar el Control de Kilometraje dada una Ubicación de Origen y una de Destino
        /// </summary>
        /// <param name="id_kilometraje">Kilometraje</param>
        /// <param name="id_compania">Compania Emisora</param>
        /// <param name="id_ubicacion_ori">Ubicación de Origen</param>
        /// <param name="id_ubicacion_dest">Ubicación de Destino</param>
        public void InicializaControlKilometraje(int id_kilometraje, int id_compania, int id_ubicacion_ori, int id_ubicacion_dest)
        {
            //Invocando Método de Inicialización
            inicializaControlUsuario(id_kilometraje, id_compania, id_ubicacion_ori, id_ubicacion_dest);
        }
        /// <summary>
        /// Método encargado de Guardar el Kilometraje
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion GuardaKilometraje()
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Inicializando el Bloque Transaccional 
            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Declarando Variable Auxiliar
                int idKilometraje = 0;
                
                //Validando que Existe el Kilometraje
                if (this._id_kilometraje > 0)
                {
                    //Instanciando Kilometraje
                    using (SAT_CL.Global.Kilometraje km = new SAT_CL.Global.Kilometraje(this._id_kilometraje))
                    {
                        //Validando que exista el Kilometraje
                        if (km.id_kilometraje > 0)
                        {
                            //Editando Kilometraje
                            result = km.EditaKilometraje(Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtUbicacionOrigen.Text, "ID:", 1)),
                                        Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtUbicacionDestino.Text, "ID:", 1)), km.geo_ubicacion_origen,
                                        km.geo_ubicacion_destino, Convert.ToDecimal(txtKms.Text), km.kms_maps, Convert.ToDecimal(txtTiempo.Text), km.tiempo_maps,
                                        Convert.ToInt32(ddlRuta.SelectedValue), Convert.ToDecimal(txtKmsPago.Text), Convert.ToDecimal(txtKmsCobro.Text), 
                                        this._id_compania, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                            //Validando que la Operación fuese Exitosa
                            if (result.OperacionExitosa)
                            {
                                //Obteniendo Registro
                                idKilometraje = result.IdRegistro;
                                
                                //Instanciando Kilometraje Inverso
                                using(SAT_CL.Global.Kilometraje kmsInv = SAT_CL.Global.Kilometraje.ObtieneKilometrajeInverso(result.IdRegistro))
                                {
                                    //Validando que Exista el Kilometraje
                                    if (kmsInv.id_kilometraje > 0)
                                    {
                                        //Editando Kilometraje
                                        result = kmsInv.EditaKilometraje(kmsInv.id_ubicacion_origen, kmsInv.id_ubicacion_destino, km.geo_ubicacion_origen,
                                                    km.geo_ubicacion_destino, chkIndicador.Checked ? Convert.ToDecimal(txtKms.Text) : Convert.ToDecimal(txtKmsInv.Text),
                                                    km.kms_maps, chkIndicador.Checked ? Convert.ToDecimal(txtTiempo.Text) : Convert.ToDecimal(txtTiempoInv.Text), km.tiempo_maps,
                                                    Convert.ToInt32(ddlRuta.SelectedValue), Convert.ToDecimal(txtKmsInvPago.Text), Convert.ToDecimal(txtKmsInvCobro.Text),
                                                    this._id_compania, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                        //Validando que la Operación fuese Exitosa
                                        if (result.OperacionExitosa)
                                        {
                                            //Instanciando 1er Kilometraje
                                            result = new RetornoOperacion(idKilometraje);

                                            //Completando Transacción
                                            trans.Complete();
                                        }
                                    }
                                }
                            }
                        }
                        else
                            //Instanciando Excepción
                            result = new RetornoOperacion("No se puede Acceder al Kilometraje");
                    }
                }
                else
                {
                    //Insertando Kilometraje
                    result = SAT_CL.Global.Kilometraje.InsertaKilometraje(Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtUbicacionOrigen.Text, "ID:", 1)),
                                    Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtUbicacionDestino.Text, "ID:", 1)), SqlGeography.Null, SqlGeography.Null,
                                    Convert.ToDecimal(txtKms.Text), 0.00M, Convert.ToDecimal(txtTiempo.Text), 0.00M, Convert.ToInt32(ddlRuta.SelectedValue),
                                    Convert.ToDecimal(chkKmsPC.Checked ? txtKms.Text : txtKmsPago.Text), Convert.ToDecimal(chkKmsPC.Checked ? txtKms.Text : txtKmsCobro.Text), 
                                    this._id_compania, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                    //Insertando Kilometraje
                    if (result.OperacionExitosa)
                    {
                        //Guardando 1er kilometraje
                        idKilometraje = result.IdRegistro;

                        //Insertando 2do Kilometraje
                        result = SAT_CL.Global.Kilometraje.InsertaKilometraje(Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtUbicacionDestino.Text, "ID:", 1)),
                                        Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtUbicacionOrigen.Text, "ID:", 1)), SqlGeography.Null, SqlGeography.Null,
                                        chkIndicador.Checked ? Convert.ToDecimal(txtKms.Text) : Convert.ToDecimal(txtKmsInv.Text), 0.00M,
                                        chkIndicador.Checked ? Convert.ToDecimal(txtTiempo.Text) : Convert.ToDecimal(txtTiempoInv.Text), 0.00M, Convert.ToInt32(ddlRuta.SelectedValue),
                                        Convert.ToDecimal(chkKmsInvPC.Checked ? chkIndicador.Checked ? txtKms.Text : txtKmsInv.Text : txtKmsInvPago.Text),
                                        Convert.ToDecimal(chkKmsInvPC.Checked ? chkIndicador.Checked ? txtKms.Text : txtKmsInv.Text : txtKmsInvCobro.Text), this._id_compania,
                                        ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                        //Validando que la Operación fuese Exitosa
                        if (result.OperacionExitosa)
                        {
                            //Instanciando 1er Kilometraje
                            result = new RetornoOperacion(idKilometraje);

                            //Completando Transacción
                            trans.Complete();
                        }
                    }
                }
            }

            //Validando que la Operación fuese Exitosa
            if (result.OperacionExitosa)
            {
                //Obteniendo Kilometraje
                this._id_kilometraje = result.IdRegistro;

                //Inicializando Control de Usuario
                inicializaControlUsuario(this._id_kilometraje, this._id_compania, this._id_ubicacion_origen, this._id_ubicacion_destino);
            }
            
            //Mostrando Mensaje de la Operación
            lblError.Text = result.Mensaje;
            
            //Devolviendo Objeto de Retorno
            return result;
        }

        #endregion
    }
}