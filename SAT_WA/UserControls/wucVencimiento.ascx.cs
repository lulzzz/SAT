using System;
using System.Web.UI;
using TSDK.Base;

namespace SAT.UserControls
{
    public partial class wucVencimiento : System.Web.UI.UserControl
    {
        #region Atributos
        
        private int _id_vencimiento;
        private int _id_tabla;
        private int _id_registro;
        private bool _hab_terminar;
        /// <summary>
        /// Almacena el Nombre del Contenedor
        /// </summary>
        public string Contenedor;

        /// <summary>
        /// Propiedad que establece el Orden de la Tabulación de los Controles
        /// </summary>
        public short TabIndex
        {
            set
            {   //Asignando Orden
                ddlEstatus.TabIndex =
                txtRegistroEnt1.TabIndex =
                txtRegistroEnt2.TabIndex =
                txtRegistroEnt3.TabIndex =
                ddlTipoVencimiento.TabIndex =
                ddlPrioridad.TabIndex =
                txtDescripcion.TabIndex =
                txtFecIni.TabIndex =
                //txtFecFin.TabIndex =
                txtValorKm.TabIndex =
                btnCancelar.TabIndex =
                btnAceptar.TabIndex = value;
            }
            get { return ddlEstatus.TabIndex; }
        }
        /// <summary>
        /// Obtiene o establece el valor de habilitación de los controles contenidos
        /// </summary>
        public bool Enabled
        {
            set
            {
                //Asignando Habilitación
                txtRegistroEnt1.Enabled =
                txtRegistroEnt2.Enabled =
                //txtRegistroEnt3.Enabled =
                ddlTipoVencimiento.Enabled =
                //ddlPrioridad.Enabled =
                txtDescripcion.Enabled =
                txtFecIni.Enabled =
                //txtFecFin.Enabled =
                txtValorKm.Enabled =
                btnCancelar.Enabled = 
                btnAceptar.Enabled = value;
            }
            get { return ddlTipoVencimiento.Enabled; }
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Evento Producido al Efectuarse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Validando que se Produjo un PostBack
            if (!Page.IsPostBack)

                //Asignando Atributos
                asignaAtributos();
            else
                //Recuperando Atributos
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

        #region Eventos (Manejadores)

        /// <summary>
        /// Manejador de Evento "Guardar"
        /// </summary>
        public event EventHandler ClickGuardarVencimiento;
        /// <summary>
        /// Evento que Manipula el Manejador "Guardar"
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnClickGuardarVencimiento(EventArgs e)
        {   
            //Validando que exista el Evento
            if (ClickGuardarVencimiento != null)
                
                //Iniciando Evento
                ClickGuardarVencimiento(this, e);
        }
        /// <summary>
        /// Manejador de Evento "Terminar"
        /// </summary>
        public event EventHandler ClickTerminarVencimiento;
        /// <summary>
        /// Evento que Manipula el Manejador "Terminar"
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnClickTerminarVencimiento(EventArgs e)
        {
            //Validando que exista el Evento
            if (ClickTerminarVencimiento != null)

                //Iniciando Evento
                ClickTerminarVencimiento(this, e);
        }

        #endregion

        /// <summary>
        /// Evento Producido al Presionar el Boton "Aceptar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            //Validando que exista un Evento
            if (ClickGuardarVencimiento != null)
                
                //Iniciando Manejador
                OnClickGuardarVencimiento(e);
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Cancelar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            //Inicializando control
            inicializaControl();
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice del Tipo de Vencimiento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTipoVencimiento_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Instanciando Tipo de Vencimiento
            using (SAT_CL.Global.TipoVencimiento tv = new SAT_CL.Global.TipoVencimiento(Convert.ToInt32(ddlTipoVencimiento.SelectedValue)))
            {
                //Validando que Existe el Tipo
                if(tv.id_tipo_vencimiento > 0)
                    //Asignando Valor
                    ddlPrioridad.SelectedValue = tv.id_prioridad.ToString();
            }
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Terminar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnTerminar_Click(object sender, EventArgs e)
        {
            //Validando que exista un Evento
            if (ClickTerminarVencimiento != null)

                //Iniciando Manejador
                OnClickTerminarVencimiento(e);
        }

        #endregion

        #region Métodos  
     
        /// <summary>
        /// Método encargado de Inicializar los Valores del Control
        /// </summary>
        private void inicializaControl()
        {
            //Limpiando Controles de texto
            limpiaControles();

            //Cargando Catalogos
            cargaCatalogos();

            //Si no hay un vencimiento activo (captura de nuevo vencimiento)          
            if (this._id_tabla > 0)
            {
                //Asignando Valor de la Entidad
                ddlEntidad.SelectedValue = this._id_tabla.ToString();
                //Instanciando el registro en cuestión
                //Si es un operador
                if (this._id_tabla == 76)
                {
                    //Habilitando controles según sea requerido
                    txtRegistroEnt1.Visible = true;
                    txtRegistroEnt2.Visible =
                    txtRegistroEnt3.Visible =
                    txtValorKm.Enabled = false;
                    //Si el registro es mayor a 0
                    if (this._id_registro > 0)
                    {
                        //Mostrando registro de interés
                        using (SAT_CL.Global.Operador operador = new SAT_CL.Global.Operador(this._id_registro))
                            txtRegistroEnt1.Text = string.Format("{0} ID:{1}", operador.nombre, operador.id_operador);
                    }
                }
                //Si es unidad
                else if (this._id_tabla == 19)
                {
                    //Habilitando controles según sea requerido
                    txtRegistroEnt1.Visible =
                    txtRegistroEnt3.Visible = false;
                    txtRegistroEnt2.Visible = true;
                    txtValorKm.Enabled = true;
                    //Si el registro es mayor a 0
                    if (this._id_registro > 0)
                    {
                        //Mostrando registro de interés
                        using (SAT_CL.Global.Unidad unidad = new SAT_CL.Global.Unidad(this._id_registro))
                            txtRegistroEnt2.Text = string.Format("{0} ID:{1}", unidad.numero_unidad, unidad.id_unidad);
                    }
                }
                //Si es Transportista
                else if (this._id_tabla == 1)
                {
                    //Habilitando controles según sea requerido
                    txtRegistroEnt1.Visible =
                    txtRegistroEnt2.Visible =
                    txtValorKm.Enabled = false;
                    txtRegistroEnt3.Visible = true;
                    //Si el registro es mayor a 0
                    if (this._id_registro > 0)
                    {
                        //Mostrando registro de interés
                        using (SAT_CL.Documentacion.Servicio servicio = new SAT_CL.Documentacion.Servicio(this._id_registro))
                            txtRegistroEnt3.Text = string.Format("Servicio No.{0} ID:{1}", servicio.no_servicio, servicio.id_servicio);
                    }
                }

                //Tipo de Vencimiento correspondiente al tipo de entidad (Tabla: 19 -> 1 Unidad, Tabla: 76 -> 2 Operador)
                SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipoVencimiento, 48, "-- Seleccione un Tipo", Convert.ToInt32(ddlEntidad.SelectedValue == "76" ? "2" : ddlEntidad.SelectedValue == "19" ? "1" : "4"), "", 0, "");

                //Instanciando Tipo de Vencimiento
                using (SAT_CL.Global.TipoVencimiento tv = new SAT_CL.Global.TipoVencimiento(Convert.ToInt32(ddlTipoVencimiento.SelectedValue)))
                {
                    //Validando que Existe el Tipo
                    if (tv.id_tipo_vencimiento > 0)
                        //Asignando Valor
                        ddlPrioridad.SelectedValue = tv.id_prioridad.ToString();
                }
            }
            //De lo contrario (vencimiento activo en lectura o edición)
            else
            {
                //Instanciando vencimiento solicitado
                using (SAT_CL.Global.Vencimiento vencimiento = new SAT_CL.Global.Vencimiento(this._id_vencimiento))
                { 
                    //Asignando atributos de vencimiento sobre controles de lectura/edición
                    if (vencimiento.id_vencimiento > 0)
                    {
                        //Asignando Valores
                        lblId.Text = vencimiento.id_vencimiento.ToString();
                        ddlEstatus.SelectedValue = vencimiento.id_estatus.ToString();
                        ddlEntidad.SelectedValue = vencimiento.id_tabla.ToString();
                        //cargando catalogo de tipos de vencimiento
                        SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipoVencimiento, 48, "-- Seleccione un Tipo", Convert.ToInt32(ddlEntidad.SelectedValue == "76" ? "2" : ddlEntidad.SelectedValue == "19" ? "1" : "4"), "", 0, "");
                        //Asignando valor al catalogo
                        ddlTipoVencimiento.SelectedValue = vencimiento.id_tipo_vencimiento.ToString();
                        //Instanciando Tipo de Vencimiento
                        using (SAT_CL.Global.TipoVencimiento tv = new SAT_CL.Global.TipoVencimiento(vencimiento.id_tipo_vencimiento))
                        {
                            //Validando que Existe el Tipo
                            if (tv.id_tipo_vencimiento > 0)
                                //Asignando Valor
                                ddlPrioridad.SelectedValue = tv.id_prioridad.ToString();
                        }

                        ddlPrioridad.SelectedValue = vencimiento.id_prioridad.ToString();
                        ddlTipoVencimiento.SelectedValue = vencimiento.id_tipo_vencimiento.ToString();
                        txtDescripcion.Text = vencimiento.descripcion;
                        txtFecIni.Text = vencimiento.fecha_inicio.ToString("dd/MM/yyyy HH:mm");
                        txtFecFin.Text = vencimiento.fecha_fin == DateTime.MinValue ? "" : vencimiento.fecha_fin.ToString("dd/MM/yyyy HH:mm");
                        txtValorKm.Text = vencimiento.valor_km.ToString();

                        //Si es un operador
                        if (vencimiento.id_tabla == 76)
                        {
                            //Habilitando controles según sea requerido
                            txtRegistroEnt1.Visible = true;
                            txtRegistroEnt2.Visible =
                            txtRegistroEnt3.Visible =
                            txtValorKm.Enabled = false;

                            using (SAT_CL.Global.Operador operador = new SAT_CL.Global.Operador(vencimiento.id_registro))
                                txtRegistroEnt1.Text = string.Format("{0} ID:{1}", operador.nombre, operador.id_operador);                            
                        }
                        //Si es unidad
                        else if (vencimiento.id_tabla == 19)
                        {
                            //Habilitando controles según sea requerido
                            txtRegistroEnt1.Visible = false;
                            txtRegistroEnt2.Visible = true;
                            txtRegistroEnt3.Visible = false;
                            txtValorKm.Enabled = true;

                            using (SAT_CL.Global.Unidad unidad = new SAT_CL.Global.Unidad(vencimiento.id_registro))
                                txtRegistroEnt2.Text = string.Format("{0} ID:{1}", unidad.numero_unidad, unidad.id_unidad);                            
                        }
                        //Si es servicio
                        else if (vencimiento.id_tabla == 1)
                        {
                            //Habilitando controles según sea requerido
                            txtRegistroEnt1.Visible = 
                            txtRegistroEnt2.Visible = false;
                            txtRegistroEnt3.Visible = true;
                            txtValorKm.Enabled = false;

                            using (SAT_CL.Documentacion.Servicio servicio = new SAT_CL.Documentacion.Servicio(vencimiento.id_registro))
                                txtRegistroEnt3.Text = string.Format("Servicio No.{0} ID:{1}", servicio.no_servicio, servicio.id_servicio);
                        }
                    }
                }                
            }

            //Validando Configuración de Termino
            if(this._hab_terminar)
                //Asignando Valor
                txtFecFin.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");

            //Aplicando habilitación sobre botón terminar
            btnTerminar.Enabled =
            txtFecFin.Enabled = this._hab_terminar;
        }        
        /// <summary>
        /// Método encargado de Cargar los Catalogos
        /// </summary>
        private void cargaCatalogos()
        {
            //Catalogo de Entidades
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlEntidad, 47, "");
            //Cargando Prioridad
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlPrioridad, "", 1103);
            //Catálogo de estatus
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlEstatus, "", 1104);
            //Catálogo de Tipo de Vencimientos
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipoVencimiento, 48, "-- Seleccione un Tipo",  Convert.ToInt32(ddlEntidad.SelectedValue == "76" ? "2" : "1"), "", 0, "");

            //Instanciando Tipo de Vencimiento
            using (SAT_CL.Global.TipoVencimiento tv = new SAT_CL.Global.TipoVencimiento(Convert.ToInt32(ddlTipoVencimiento.SelectedValue)))
            {
                //Validando que Existe el Tipo
                if (tv.id_tipo_vencimiento > 0)
                    //Asignando Valor
                    ddlPrioridad.SelectedValue = tv.id_prioridad.ToString();
            }
        }
        /// <summary>
        /// Método encargado de Asignar los Atributos 
        /// </summary>
        private void asignaAtributos()
        {
            //Asignando Atributos
            ViewState["IdVencimiento"] = this._id_vencimiento;
            ViewState["IdTabla"] = this._id_tabla;
            ViewState["IdRegistro"] = this._id_registro;
            ViewState["HabTerminar"] = this._hab_terminar;
        }
        /// <summary>
        /// Método encargado de Recuperar los Atributos
        /// </summary>
        private void recuperaAtributos()
        {
            //Validando que Exista un Vencimiento
            if (ViewState["IdVencimiento"] != null)
                //Asignando Vencimiento
                this._id_vencimiento = Convert.ToInt32(ViewState["IdVencimiento"]);

            //Validando que Exista Tabla de Origen para entidad
            if (ViewState["IdTabla"] != null)
                //Asignando Vencimiento
                this._id_tabla = Convert.ToInt32(ViewState["IdTabla"]);

            //Validando que Exista una entidad
            if (ViewState["IdRegistro"] != null)
                //Asignando Vencimiento
                this._id_tabla = Convert.ToInt32(ViewState["IdRegistro"]);

            //Validando habilitación de botón terminar
            if (ViewState["HabTerminar"] != null)
                this._hab_terminar = Convert.ToBoolean(ViewState["HabTerminar"]);
        }
        /// <summary>
        /// Método encargado de Limpiar los Controles
        /// </summary>
        private void limpiaControles()
        {
            //Limpiando Controles
            lblId.Text = "Por Asignar";
            txtDescripcion.Text =
            txtRegistroEnt1.Text =
            txtRegistroEnt2.Text =
            txtRegistroEnt3.Text =
            txtValorKm.Text = "";
            txtFecIni.Text = Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");
            txtFecFin.Text = "";
            lblError.Text = "";
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método Público encargado de Inicializar el Control
        /// </summary>
        /// <param name="id_vencimiento">Vencimiento</param>
        /// <param name="hab_terminar">True para habilitar la opción de término de vencimiento, de lo contrario False</param>
        public void InicializaControl(int id_vencimiento, bool hab_terminar)
        {   
            //Asignando criterio de habilitación de opción Terminar
            this._hab_terminar = hab_terminar;
            //Asignando Id de Vencimiento, Tabla y Registro
            this._id_vencimiento = id_vencimiento;
            this._id_tabla = 0;
            this._id_registro = 0;

            //Inicializando Control
            inicializaControl();
        }
        /// <summary>
        /// Método Público encargado de Inicializar el Control
        /// </summary>
        /// <param name="id_tabla">Entidad</param>
        /// <param name="id_registro">Id de Registro</param>
        public void InicializaControl(int id_tabla, int id_registro)
        {
            //Asignando criterio de habilitación de opción Terminar
            this._hab_terminar = false;
            //Asignando Id de Vencimiento, Tabla y Registro
            this._id_vencimiento = 0;
            this._id_tabla = id_tabla;
            this._id_registro = id_registro;

            //Inicializando Control
            inicializaControl();
        }
        /// <summary>
        /// Método encargado de Guardar un Vencimiento
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion GuardaVencimiento()
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Obteniendo Fechas
            DateTime fecha_inicio = DateTime.MinValue;
            DateTime.TryParse(txtFecIni.Text, out fecha_inicio);

            //Obteniendo Registro
            int idRegistro = Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada((txtRegistroEnt1.Visible ? txtRegistroEnt1.Text : (txtRegistroEnt2.Visible ? txtRegistroEnt2.Text : txtRegistroEnt3.Text)), "ID:", 1, "0"));

            //Validando que Exista el Tipo de Vencimiento
            if (ddlTipoVencimiento.SelectedValue != "0")
            {
                //Validando si existe un Vencimiento
                if (this._id_vencimiento > 0)
                {
                    //Instanciando Vencimiento
                    using (SAT_CL.Global.Vencimiento ven = new SAT_CL.Global.Vencimiento(this._id_vencimiento))
                    {
                        //Validando que Exista el Vencimiento
                        if (ven.id_vencimiento > 0)

                            //Edita Vencimiento
                            result = ven.EditaVencimiento(ven.estatus, Convert.ToInt32(ddlEntidad.SelectedValue), idRegistro,
                                            Convert.ToInt32(ddlPrioridad.SelectedValue), Convert.ToInt32(ddlTipoVencimiento.SelectedValue), txtDescripcion.Text.ToUpper(), fecha_inicio, ven.fecha_fin,
                                            Convert.ToDecimal(txtValorKm.Text == "" ? "0" : txtValorKm.Text), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                    }
                }
                else
                    //Edita Vencimiento
                    result = SAT_CL.Global.Vencimiento.InsertaVencimiento(Convert.ToInt32(ddlEntidad.SelectedValue), idRegistro,
                                    Convert.ToInt32(ddlPrioridad.SelectedValue), Convert.ToInt32(ddlTipoVencimiento.SelectedValue), txtDescripcion.Text.ToUpper(), fecha_inicio, DateTime.MinValue,
                                    Convert.ToDecimal(txtValorKm.Text == "" ? "0" : txtValorKm.Text), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
            }
            else
                //Instanciando Excepción
                result = new RetornoOperacion("* Seleccione el Tipo del Vencimiento");

            //Validando que la Operación fuese Exitosa
            if (result.OperacionExitosa)
            {
                //Asignando Id de Registro actualizado
                this._id_vencimiento = result.IdRegistro;
                //Limpiando auxiliares de tabla y registro
                this._id_tabla = this._id_registro = 0;

                //Invocando Método de Inicialización
                inicializaControl();
            }

            //Mostrando Mensaje de la Operación
            lblError.Text = result.Mensaje;

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de Terminar el Vencimiento
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion TerminaVencimiento()
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Obteniendo Fecha de Fin
            DateTime fec_fin;
            DateTime.TryParse(txtFecFin.Text, out fec_fin);

            //Validando que Exista el Vencimiento
            if (this._id_vencimiento > 0)
            {
                //Instanciando Vencimiento
                using (SAT_CL.Global.Vencimiento ven = new SAT_CL.Global.Vencimiento(this._id_vencimiento))
                {
                    //Validando que exista el Vencimiento
                    if (ven.id_vencimiento > 0)
                        //Terminando Vencimiento
                        result = ven.TerminaVencimiento(fec_fin, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                    else
                        //Instanciando Excepción
                        result = new RetornoOperacion("El vencimiento no fue encontrado para su edición.");
                }
            }
            else
                //Instanciando Excepción
                result = new RetornoOperacion("No existe un vencimiento que terminar.");

            //Validando que la Operación fue Exitosa
            if (result.OperacionExitosa)
            {
                //Guardando Id de registro actualizado
                this._id_vencimiento = result.IdRegistro;
                this._id_tabla = this._id_registro = 0;

                //Invocando Método de Inicialización
                inicializaControl();
            }

            //Mostrando Mensaje de Operación
            lblError.Text = result.Mensaje;

            //Devolviendo Objeto de Retorno
            return result;
        }

        #endregion
    }
}