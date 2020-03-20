using SAT_CL;
using SAT_CL.Despacho;
using SAT_CL.Global;
using SAT_CL.Seguridad;
using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.UserControls
{
    public partial class wucPublicacionUnidad : System.Web.UI.UserControl
    {
        #region Atributos

        /// <summary>
        /// Id Publicacion
        /// </summary>
        private int _id_publicacion;
        /// <summary>
        /// Id Unidad
        /// </summary>
        private int _id_unidad;
        /// <summary>
        ///  Id Operador
        /// </summary>
        private int _id_operador;
        public event EventHandler ClickPublicar;
        /// <summary>
        /// Declaración de Evento ClickCancelar
        /// </summary>
        public event EventHandler ClickCancelar;
        /// <summary>
        /// Almacena el Nombre del Contenedor
        /// </summary>
        public string Contenedor;
        /// <summary>
        /// Tabla con las Ciudades Deseadas
        /// </summary>
        private DataTable _mitCiudadesDeseadas;

        /// <summary>
        /// Propiedad que establece el Orden de la Tabulación de los Controles
        /// </summary>
        public short TabIndex
        {
            set
            {   //Asignando Orden
                ddlTipoUnidad.TabIndex =
                txtUnidad.TabIndex =
                txtInformacionUnidad.TabIndex =
                chkLicenciaVigente.TabIndex =
                ddlEstadoDisponible.TabIndex =
                txtCiudadDisponible.TabIndex =
                txtFechaDisponible.TabIndex =
                txtLimiteDisponibilidad.TabIndex =
                txtObservacion.TabIndex = 
                ddlTamanoCiudadesDeseadas.TabIndex =
                lkbExportarCiudadesDeseadas.TabIndex =
                gvCiudadesDeseadas.TabIndex=  value;
            }
            get { return ddlTipoUnidad.TabIndex; }
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Evento generado al cargar la pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Validando  PostBack
            if (!this.IsPostBack)
            {
                //Carga catalogos
                cargaCatalogos();
            }
            else
            {
                //Recupera Atributos
                recuperaAtributos();
            }
        }

        /// <summary>
        /// Evento producido previo a la carga
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            //Almacenando valores viewstate
            asignaAtributos();
        }

        /// <summary>
        /// Manipula Evento ClickPublicar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void OnClickPublicar(EventArgs e)
        {
            if (ClickPublicar != null)
                ClickPublicar(this, e);

        }

        /// Evento disparado al presionar el boton Publicar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnClick_btnPublicar(object sender, EventArgs e)
        {
            //Validando que exista un Evento
            if (ClickPublicar != null)
                OnClickPublicar(e);
        }
        /// <summary>
        /// Manipula Evento ClickCancelar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void OnClickCancelar(EventArgs e)
        {
            if (ClickCancelar != null)
                ClickCancelar(this, e);

        }
        /// <summary>
        /// Evento disparado al presionar el boton cancelar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnClick_btnCancelar(object sender, EventArgs e)
        {
            //Validando que exista un Evento
            if (ClickCancelar != null)
                OnClickCancelar(new EventArgs());
        }

        /// <summary>
        /// Evento generado al Cambiar el Tipo de Unidad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTipoUnidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Validamos  Tipo de Unidad sea Motriz
            using (UnidadTipo objUnidadTipo = new UnidadTipo(Convert.ToInt32(ddlTipoUnidad.SelectedValue)))
            {
                if (!objUnidadTipo.bit_motriz)
                {
                    //Inicializamos Control
                    txtOperador.Text = "Ninguno ID:0";
                    txtOperador.Enabled = false;
                    txtInformacionOperador.Text = "";
                }
                else
                {
                    txtOperador.Enabled = true;
                    txtOperador.Text = "";
                    txtInformacionOperador.Text = "";
                }
            }
            //Inicializamos Validador
            inicializaValidador();
            //Carga Autocomplete
            inicializaAutocompleteUnidad(ddlTipoUnidad);
            //Limpiamos Controles de Unidad
            txtUnidad.Text = "";
            txtInformacionUnidad.Text = "";
        }

        /// Evento sorting de gridview 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvCiudadesDeseadas_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Validando que existan Llaves
            if (gvCiudadesDeseadas.DataKeys.Count > 0)
            {   //Asignando Criterio de Ordenacion
                this._mitCiudadesDeseadas.DefaultView.Sort = lblOrdenadoCiudadesDeseadas.Text;
                //Cambiando Ordenamiento
                lblOrdenadoCiudadesDeseadas.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvCiudadesDeseadas, this._mitCiudadesDeseadas, e.SortExpression, false, 1);
            }
        }
        /// <summary>
        /// Evento cambio de página en gridview 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvCiudadesDeseadas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvCiudadesDeseadas, this._mitCiudadesDeseadas, e.NewPageIndex, false, 1);
        }
        /// <summary>
        /// Evento cambio de tamaño de página de gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoCiudadesDeseadas_SelectedIndexChanged(object sender, EventArgs e)
        {
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvCiudadesDeseadas, this._mitCiudadesDeseadas, Convert.ToInt32(ddlTamanoCiudadesDeseadas.SelectedValue), false, 1);
        }
        /// <summary>
        /// Evento click en botón de exportación 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarCiudadesDeseadas_Click(object sender, EventArgs e)
        {
            TSDK.ASP.Controles.ExportaContenidoGridView(this._mitCiudadesDeseadas, "");
        }

        /// <summary>
        /// Evento Generado al Insertar una Ciudad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbInsertar_Click(object sender, EventArgs e)
        {
            //Insertamos Ciudad
            insertaCiudadTemporal();
        }

        /// <summary>
        /// Evento Generado al Eliminar una Ciudad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbEliminar_Click(object sender, EventArgs e)
        {
            //Validamos Registros
            if (gvCiudadesDeseadas.DataKeys.Count > 0)
            {
                //Seleccionando la fila correspondiente
                Controles.SeleccionaFila(gvCiudadesDeseadas, sender, "lnk", false);

                //Eliminamos Ciudad
                eliminaCiudadTemporal();
            }
        }

        /// <summary>
        /// Evento generado al cambiar el Estado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlEstadoDisponible_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cargamos Autocomplete
            inicializaAutocompleteCiudad();
        }

        /// <summary>
        /// Evento Generado al Cambiar la Unidad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtUnidad_TextChanged(object sender, EventArgs e)
        {
            //Asignamos Nuevo Id de Unidad
            this._id_unidad = Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtUnidad.Text, ':', 1));
            //Instanciamos Unidad
            using (SAT_CL.Global.Unidad _objUnidad = new Unidad(this._id_unidad))
            {
                //Instanciamos Operador de la Unidad
                using (SAT_CL.Global.Operador _objOperador = new Operador(_objUnidad.id_operador))
                {

                    txtInformacionUnidad.Text = "Sub-Tipo: " + SAT_CL.Global.Catalogo.RegresaDescripcionCatalogo(1109, _objUnidad.id_sub_tipo_unidad) +
                                 ", Modelo: " + _objUnidad.modelo + ", Marca: " + SAT_CL.Global.Catalogo.RegresaDescripcionCatalogo(55, _objUnidad.id_marca)
                    + ", Dimensiones: " + SAT_CL.Global.Catalogo.RegresaDescripcionCatalogo(1108, _objUnidad.id_dimension)
                    + ", Placas: " + _objUnidad.placas
                    + ", Antena GPS: " + _objUnidad.antena_gps_principal;

                    //Validamos Tipo de Unidad
                    using (UnidadTipo objUnidadTipo = new UnidadTipo(_objUnidad.id_tipo_unidad))
                    {
                        //Validamos que la Unidad sea Motriz
                        if (objUnidadTipo.bit_motriz)
                        {
                            //Inicializamos Valores del Operador
                            txtOperador.Text = _objOperador.nombre + " ID:" + _objOperador.id_operador.ToString();
                            txtInformacionOperador.Text = "Edad: " + (TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().AddTicks(-_objOperador.fecha_nacimiento.Ticks).Year - 1).ToString() +
                                                          ", R-Control: " + _objOperador.r_control;
                            chkLicenciaVigente.Checked = Vencimiento.ValidaLicenciaVigente(this._id_operador);
                        }
                        else
                        {
                            //Inicializamos Valores del Operador
                            txtOperador.Text = "";
                            txtInformacionOperador.Text = "";
                            chkLicenciaVigente.Checked = false;
                        }
                    }
                }
            }
            //Inicializamos AutoComplete de Unidad
            inicializaAutocompleteUnidad(txtUnidad);

        }
        #endregion

        #region Métodos

        /// <summary>
        /// Construye el esquema de tablas temporales
        /// </summary>
        private void creaEsquemaTablasTemporales()
        {
            //Ciudades
            this._mitCiudadesDeseadas = new DataTable();
            this._mitCiudadesDeseadas.Columns.Add("Id", typeof(Int32));
            this._mitCiudadesDeseadas.Columns.Add("Ciudad", typeof(String));
            this._mitCiudadesDeseadas.Columns.Add("Tarifa", typeof(Decimal));
            this._mitCiudadesDeseadas.Columns.Add("Anticipo", typeof(Decimal));
        }
        /// <summary>
        /// Recupera Atributos
        /// </summary>
        private void recuperaAtributos()
        {
            //Recuperando Atributos
            if (Convert.ToInt32(ViewState["IdPublicacion"]) != 0)
                this._id_publicacion = Convert.ToInt32(ViewState["IdPublicacion"]);
            //Recuperando Atributos
            if (Convert.ToInt32(ViewState["IdUnidad"]) != 0)
                this._id_unidad = Convert.ToInt32(ViewState["ObjUnidad"]);
            //Recuperando Atributos
            if (Convert.ToInt32(ViewState["IdOperador"]) != 0)
                this._id_operador = Convert.ToInt32(ViewState["IdOperador"]);
            if (ViewState["mitCiudades"] != null)
                this._mitCiudadesDeseadas = (DataTable)ViewState["mitCiudades"];


        }

        /// <summary>
        /// Asigna Atributos
        /// </summary>
        private void asignaAtributos()
        {
            //Asignando Atributos
            ViewState["IdPublicacion"] = this._id_publicacion;
            ViewState["IdUnidad"] = this._id_unidad;
            ViewState["IdOperador"] = this._id_operador;
            ViewState["mitCiudades"] = this._mitCiudadesDeseadas;

        }

        /// <summary>
        /// Inicializa Valores
        /// </summary>
        private void inicializaValores()
        {
            //Validamos Existencia de Unidad
            if (this._id_unidad > 0 )
            {
                //Instanciamos Unidad
                using (SAT_CL.Global.Unidad _objUnidad = new Unidad(this._id_unidad))
                {
                    //Instanciamos Operador de la Unidad
                    using (SAT_CL.Global.Operador _objOperador = new Operador(_objUnidad.id_operador))
                    {
                        txtUnidad.Text = _objUnidad.numero_unidad.ToString() + " ID:" + _objUnidad.id_unidad.ToString();

                        txtInformacionUnidad.Text = "Sub-Tipo: " + SAT_CL.Global.Catalogo.RegresaDescripcionCatalogo(1109, _objUnidad.id_sub_tipo_unidad) +
                                     ", Modelo: " + _objUnidad.modelo + ", Marca: " + SAT_CL.Global.Catalogo.RegresaDescripcionCatalogo(55, _objUnidad.id_marca)
                        + ", Dimensiones: " + SAT_CL.Global.Catalogo.RegresaDescripcionCatalogo(1108, _objUnidad.id_dimension)
                        + ", Placas: " + _objUnidad.placas
                        + ", Antena GPS: " + _objUnidad.antena_gps_principal;

                        //Validamos Tipo de Unidad
                        using (UnidadTipo objUnidadTipo = new UnidadTipo(_objUnidad.id_tipo_unidad))
                        {
                            //Inicializamos Control Tipo de Unidad
                            ddlTipoUnidad.SelectedValue = objUnidadTipo.id_tipo_unidad.ToString(); ;
                            //Validamos que la Unidad sea Motriz
                            if (objUnidadTipo.bit_motriz)
                            {
                                //Inicializamos Valores del Operador
                                txtOperador.Text = _objOperador.nombre + " ID:" + _objOperador.id_operador.ToString();
                                txtInformacionOperador.Text = "Edad: " + (TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().AddTicks(-_objOperador.fecha_nacimiento.Ticks).Year - 1).ToString() +
                                                              ", R-Control: " + _objOperador.r_control;
                                chkLicenciaVigente.Checked = Vencimiento.ValidaLicenciaVigente(this._id_operador);
                            }
                            else
                            {
                                //Inicializamos Valores del Operador
                                txtOperador.Text = "";
                                txtInformacionOperador.Text = "";
                                chkLicenciaVigente.Checked = false;
                            }
                            //Inicializamos Controles de la Ciudad Disponible de acuerdo al Estatus de la Unidad (Disponible)
                            if ((SAT_CL.Global.Unidad.Estatus)_objUnidad.id_estatus_unidad == Unidad.Estatus.ParadaDisponible)
                            {
                                //Instanciamos Estancia de la Unidad
                                using (EstanciaUnidad objEstancia = new EstanciaUnidad(_objUnidad.id_estancia))
                                {
                                    //Instanciamos Parada
                                    using (Parada objParada = new Parada(objEstancia.id_parada))
                                    {
                                        //Instanciamos Ubicacion
                                        using (Ubicacion objUbicacion = new Ubicacion(objParada.id_ubicacion))
                                        {
                                            //Instanciamos Ciudad
                                            using (Ciudad objCiudad = new Ciudad(objUbicacion.id_ciudad))
                                            {
                                                //Asignamos Ciudad Default
                                                ddlEstadoDisponible.SelectedValue = objCiudad.id_estado.ToString();
                                                inicializaAutocompleteCiudad();
                                                txtCiudadDisponible.Text = objCiudad.descripcion + " ID:" + objCiudad.id_ciudad.ToString();

                                            }
                                        }
                                    }
                                }
                            }
                            //Inicializamos Controles de la Ciudad Disponible de acuerdo al Estatus de la Unidad (Transito)
                            if ((SAT_CL.Global.Unidad.Estatus)_objUnidad.id_estatus_unidad == Unidad.Estatus.Transito)
                            {
                                //Instanciamos Movimiento
                                using (Movimiento objMovimiento = new Movimiento(_objUnidad.id_movimiento))
                                {
                                    //Instanciamos Parada
                                    using (Parada objParada = new Parada(objMovimiento.id_parada_destino))
                                    {
                                        //Instanciamos Ubicacion
                                        using (Ubicacion objUbicacion = new Ubicacion(objParada.id_ubicacion))
                                        {
                                            //Instanciamos Ciudad
                                            using (Ciudad objCiudad = new Ciudad(objUbicacion.id_ciudad))
                                            {
                                                //Asignamos Ciudad Default
                                                ddlEstadoDisponible.SelectedValue = objCiudad.id_estado.ToString();
                                                inicializaAutocompleteCiudad();
                                                txtCiudadDisponible.Text = objCiudad.descripcion + " ID:" + objCiudad.id_ciudad.ToString();
                                            }
                                        }
                                    }
                                }
                            }
                            //Inicializamos Controles de la Ciudad Disponible de acuerdo al Estatus de la Unidad (Ocupado)
                            if ((SAT_CL.Global.Unidad.Estatus)_objUnidad.id_estatus_unidad == Unidad.Estatus.ParadaOcupado)
                            {
                                //Instanciamos Estancia de la Unidad
                                using (EstanciaUnidad objEstancia = new EstanciaUnidad(_objUnidad.id_estancia))
                                {
                                    //Instanciamos Parada
                                    using (Parada objParada = new Parada(objEstancia.id_parada))
                                    {
                                        //Instanciamos Ubicacion
                                        using (Ubicacion objUbicacion = new Ubicacion(objParada.id_ubicacion))
                                        {
                                            //Instanciamos Ciudad
                                            using (Ciudad objCiudad = new Ciudad(objUbicacion.id_ciudad))
                                            {
                                                //Asignamos Ciudad Default
                                                ddlEstadoDisponible.SelectedValue = objCiudad.id_estado.ToString();
                                                inicializaAutocompleteCiudad();
                                                txtCiudadDisponible.Text = objCiudad.descripcion + " ID:" + objCiudad.id_ciudad.ToString();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                //Inicializamos Valores
                txtFechaDisponible.Text = "";
                txtLimiteDisponibilidad.Text = "";
                txtObservacion.Text = "";
            }
            else 
            {
                //Obtenemos Datos de la Publicación Activa de la Unidad
                DataTable mitPublicacion = consumoInicializaPublicacion();

               //Validando que el contenga registros
                if (Validacion.ValidaOrigenDatos(mitPublicacion))
                {
                    //Obtenemos La Referencias del Concepto Origen
                    DataRow re = (from DataRow r in mitPublicacion.Rows
                                  select r).FirstOrDefault();
                    //Validamos que exista elementos
                    if (re != null )
                    {
                        //Inicializamos Valores
                        ddlTipoUnidad.SelectedValue = UnidadTipo.RegresaIdUnidadTipo(re.Field<string>("TipoUnidad")).ToString();
                        txtUnidad.Text = re.Field<string>("UnidadOrigen");

                        txtInformacionUnidad.Text = "Sub-Tipo: " + re.Field<string>("SubTipoUnidad") +
                                     ", Modelo: " + re.Field<string>("Modelo")
                        + ", Dimensiones: " + re.Field<string>("Dimensiones")
                        + ", Placas: " + re.Field<string>("Placas")
                        + ", Antena GPS: " + re.Field<string>("AntenaGPS");
                        txtOperador.Text = re.Field<string>("Operador");
                       //Instanciamos Tipo de Unidad
                        using (SAT_CL.Global.UnidadTipo objUnidadTipo = new UnidadTipo(Convert.ToInt32(ddlTipoUnidad.SelectedValue)))
                        {
                            //Validamos Tipo de Unidad
                            if (objUnidadTipo.bit_motriz)
                            {
                                txtInformacionOperador.Text = "Edad: " + Convert.ToInt32(re["Edad"]).ToString() +
                                                                     ", R-Control: " + re.Field<string>("RControl");
                                chkLicenciaVigente.Checked = re.Field<bool>("Licencia");
                            }
                            else
                            {
                                //Limpiamos Controles
                                txtInformacionOperador.Text = "";
                                chkLicenciaVigente.Checked = false;
                            }

                        }
                        //Carga catalogo de Estados
                        CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlEstadoDisponible, "", 16);
                        ddlEstadoDisponible.SelectedValue = Catalogo.RegresaValorCadenaValor(16,re.Field<string>("EstadoDisponibilidad"));
                        txtCiudadDisponible.Text = re.Field<string>("CiudadDisponibilidad");
                        txtFechaDisponible.Text = re.Field<DateTime>("InicioDisponibilidad").ToString("dd/MM/yyyy HH:mm");
                        txtLimiteDisponibilidad.Text = re.Field<DateTime>("InicioDisponibilidad").AddMinutes(Convert.ToInt32(re["LimiteDisponibilidad"])).ToString("dd/MM/yyyy HH:mm");
                        txtObservacion.Text = re.Field<string>("Observacion");
                    }
                }

            }
        }

        /// <summary>
        /// Método encargado de Cargar los Catalogos del Control de Usuario
        /// </summary>
        private void cargaCatalogos()
        {
            //Tipos de Unidad
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipoUnidad, 24, "", 0, "", 0, "");
            //Cargando catalogo de tamaño  
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoCiudadesDeseadas, "", 56);
            //Carga catalogo de Estados
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlEstadoDisponible, "", 16);
        }

        /// <summary>
        /// Habilita Controles
        /// </summary>
        private void habilitaControles()
        {
            //Validamos Si existe el Id de Unidad
            if (this._id_unidad > 0)
            {
                //Instanciamos Unidad
                using (Unidad objUnidad = new Unidad(this._id_unidad))
                {
                    //Validamos Tipo de Unidad
                    using (UnidadTipo objUnidadTipo = new UnidadTipo(objUnidad.id_tipo_unidad))
                    {
                        //Validamos que la Unidad sea Motriz
                        if (objUnidadTipo.bit_motriz)
                        {
                            txtOperador.Enabled = true;

                        }
                        else
                        {
                            txtOperador.Enabled = false;
                        }
                    }
                }
                //Habilitamos Controles
                ddlTipoUnidad.Enabled =
                txtUnidad.Enabled =
                txtInformacionUnidad.Enabled =
                chkLicenciaVigente.Enabled =
                ddlEstadoDisponible.Enabled =
                txtCiudadDisponible.Enabled =
                txtFechaDisponible.Enabled =
                txtLimiteDisponibilidad.Enabled =
                txtObservacion.Enabled = 
                gvCiudadesDeseadas.Enabled = 
                btnPublicar.Enabled = true;
            }
            else
            {
                //Deshabilitamos Controles
                ddlTipoUnidad.Enabled =
                txtUnidad.Enabled =
                txtOperador.Enabled =
                txtInformacionUnidad.Enabled =
                chkLicenciaVigente.Enabled =
                ddlEstadoDisponible.Enabled =
                txtCiudadDisponible.Enabled =
                txtFechaDisponible.Enabled =
                txtLimiteDisponibilidad.Enabled =
                txtObservacion.Enabled = 
                btnPublicar.Enabled =
                gvCiudadesDeseadas.Enabled = false;
            }
        }

        /// <summary>
        /// Método encargado de Inicializar los validadores requeridos
        /// </summary>
        private void inicializaValidador()
        {
            string script = "";
            //Validamos  Tipo de Unidad sea Motriz
            using (UnidadTipo objUnidadTipo = new UnidadTipo(Convert.ToInt32(ddlTipoUnidad.SelectedValue)))
            {
                if (objUnidadTipo.bit_motriz)
                {
                    //Generamos script para validación de Publicación de la Unidad
                    script =
                   @"<script type='text/javascript'>
                
                    var validacionPublicacionUnidad = function (evt) {
                        //Validando sólo contenido de controles de interés (por separado para visualizar todos los mensajes de error a la vez)
                        var isValid1 = !$('#" + txtUnidad.ClientID + @"').validationEngine('validate');
                        var isValid2 = !$('#" + txtCiudadDisponible.ClientID + @"').validationEngine('validate');
                        var isValid3 = !$('#" + txtFechaDisponible.ClientID + @"').validationEngine('validate');
                        var isValid4 = !$('#" + txtLimiteDisponibilidad.ClientID + @"').validationEngine('validate'); 
                        var isValid5 = !$('#" + txtOperador.ClientID + @"').validationEngine('validate'); 
                          return isValid1 && isValid2 && isValid3 && isValid4 && isValid5
                    }; 
                //Botón Buscar
                $('#" + btnPublicar.ClientID + @"').click(validacionPublicacionUnidad);
                                    </script>";

                }
                else
                {
                    //Generamos script para validación de Fechas
                    script =
                       @"<script type='text/javascript'>
                
                    var validacionPublicacionUnidad1 = function (evt) {
                        var isValid1 = !$('#" + txtUnidad.ClientID + @"').validationEngine('validate');
                        var isValid2 = !$('#" + txtCiudadDisponible.ClientID + @"').validationEngine('validate');
                        var isValid3 = !$('#" + txtFechaDisponible.ClientID + @"').validationEngine('validate');
                        var isValid4 = !$('#" + txtLimiteDisponibilidad.ClientID + @"').validationEngine('validate'); 
                        return isValid1 && isValid2 && isValid3 && isValid4 
                    }; 
                //Botón Buscar
                $('#" + btnPublicar.ClientID + @"').click(validacionPublicacionUnidad1);
                                    </script>";

                }
            }
            //Registrando el script sólo para los paneles que producirán actualización del mismo
            System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Publicacion", script, false);
        }

        /// <summary>
        /// Inicializamos Autocomplete Unidad
        /// </summary>
        /// <param name="control"></param>
        private void inicializaAutocompleteUnidad(System.Web.UI.Control control)
        {
            //Generamos Sript
            string script =
            @"<script type='text/javascript'>
              $('#" + this.txtUnidad.ClientID + @"').autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=50&param=" + ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString() + @"&param3=" + ddlTipoUnidad.SelectedValue + @"', appendTo: '" + this.Contenedor + @"'});
              </script>";

            //Registrando el script sólo para los paneles que producirán actualización del mismo
            System.Web.UI.ScriptManager.RegisterStartupScript(control, control.GetType(), "AutocompleteEstado", script, false);
        }

        /// <summary>
        /// Inicializamos Autocomplete de Ciudad
        /// </summary>
        private void inicializaAutocompleteCiudad()
        {
            //Generamos Sript
            string script =
            @"<script type='text/javascript'>
              $('#" + this.txtCiudadDisponible.ClientID + @"').autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=51&param=" + ddlEstadoDisponible.SelectedValue + @"', appendTo: '" + this.Contenedor + @"'});
              </script>";

            //Registrando el script sólo para los paneles que producirán actualización del mismo
            System.Web.UI.ScriptManager.RegisterStartupScript(ddlEstadoDisponible, ddlEstadoDisponible.GetType(), "AutocompleteCiudad", script, false);
        }

        /// <summary>
        /// Inicializamos Control de Publicacion de Unidad
        /// </summary>
        /// <param name="id_unidad">Id de Unidad en caso de no contar con la Publicacion</param>
        /// <param name="id_publicacion">Id de Publicación si Ya existe</param>
        public void InicializaControl(int id_unidad, int id_publicacion)
        {
            //Instaciamos Unidad
            using (Unidad objUnidad = new Unidad(id_unidad))
            {
                //Asignando Atributos
                this._id_unidad = id_unidad;
                this._id_operador = objUnidad.id_operador;
                this._id_publicacion = id_publicacion;
                //Inicializa Valores
                inicializaValores();
                //Habilita Controles
                habilitaControles();
                //Inicializamos Validador
                inicializaValidador();
                //Inicializa Autocomplete
                inicializaAutocompleteUnidad(this);
                //Cargamos Autocomplete
                inicializaAutocompleteCiudad();
                //Limpiamos Tabla
                if (this._mitCiudadesDeseadas != null)
                {
                    this._mitCiudadesDeseadas.Clear();
                }
                //Crga Ciudades Destino
                cargaCiudadesDestino();
                //Carga Esquema de Tabla Temporal
                creaEsquemaTablasTemporales();
            }
        }
        /// <summary>
        /// Carga las Ciudades deseadas a los criterios de búsqueda solicitados
        /// </summary>
        private void cargaCiudadesDestino()
        {
            //Validamos Publicacion
            if (this._id_publicacion == 0)
            {
                //Validamos Origen de Datos
                if (this._mitCiudadesDeseadas == null || !TSDK.Datos.Validacion.ValidaOrigenDatos(this._mitCiudadesDeseadas))
                    TSDK.ASP.Controles.InicializaGridview(gvCiudadesDeseadas);
                else
                    //Mostrandolos en gridview
                    TSDK.ASP.Controles.CargaGridView(gvCiudadesDeseadas, this._mitCiudadesDeseadas, "Id", lblOrdenadoCiudadesDeseadas.Text, false, 1);
            }
            else
            {
                //Cargamos Ciudades Destinos Deseadas 
                this._mitCiudadesDeseadas = consumoInicializaCiudadesDeseadas();  
                //Mostrandolo en el Grid View
               TSDK.ASP.Controles.CargaGridView(gvCiudadesDeseadas, this._mitCiudadesDeseadas, "Id", lblOrdenadoCiudadesDeseadas.Text, false, 1);
             
            }
        }

        /// <summary>
        /// Inserta una Ciudad Deseada en la tabla temporal
        /// </summary>
        private void insertaCiudadTemporal()
        {
            //Obteniendo el Nuevo Id
            decimal Id = (from DataRow r in this._mitCiudadesDeseadas.Rows
                          select Convert.ToDecimal(r["Id"])).DefaultIfEmpty(0).Max() + 1;

            //Declarando objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion(Id);

            try
            {
                //Recuperando controles 
                using (TextBox txtCiudadD = (TextBox)gvCiudadesDeseadas.FooterRow.FindControl("txtCiudadDeseada"),
                        txtTarifaD = (TextBox)gvCiudadesDeseadas.FooterRow.FindControl("txtTarifaDeseada"),
                        txtAnticipoD = (TextBox)gvCiudadesDeseadas.FooterRow.FindControl("txtAnticipoDeseado"))
                {


                    //Creando nueva fila de tabla 
                    DataRow nr = this._mitCiudadesDeseadas.NewRow();
                    //Añadiendo atributos de evento
                    nr.SetField("Id", Id);
                    nr.SetField("Ciudad", Cadena.RegresaCadenaSeparada(txtCiudadD.Text, ',', 0));
                    nr.SetField("Tarifa", Convert.ToDecimal(Cadena.VerificaCadenaVacia(txtTarifaD.Text, "0")));
                    nr.SetField("Anticipo", Convert.ToDecimal(Cadena.VerificaCadenaVacia(txtAnticipoD.Text, "0")));

                    //Insertando Ciudad DestinosDeseados
                    this._mitCiudadesDeseadas.Rows.Add(nr);
                }

            }
            catch (Exception ex) { resultado = new RetornoOperacion(string.Format("Error al insertar la Ciudad Desino Deseada: {0}", ex.Message)); }


            //Mostrando resultado
            ScriptServer.MuestraNotificacion(gvCiudadesDeseadas, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);

            //Carga Ciudades Deseadas
            cargaCiudadesDestino();

        }

        /// <summary>
        /// Realiza la eliminación de la Ciudad Deseada
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion eliminaCiudadTemporal()
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Guardamos Id
            int Id = Convert.ToInt32(gvCiudadesDeseadas.SelectedValue);
            //Obteniendo la fila en el origen de datos del producto seleccionado
            DataRow r = (from DataRow p in this._mitCiudadesDeseadas.Rows
                         where Convert.ToDecimal(p["Id"]) == Convert.ToInt32(gvCiudadesDeseadas.SelectedValue)
                         select p).FirstOrDefault();

            //Eliminando Ciudad temporal
            this._mitCiudadesDeseadas.Rows.Remove(r);

            resultado = new RetornoOperacion(Id);

            //Validamos Resultado
            if (resultado.OperacionExitosa)
            {
                cargaCiudadesDestino();
                //Iniciamizamos Indices
                Controles.InicializaIndices(gvCiudadesDeseadas);
            }

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(gvCiudadesDeseadas, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);

            //Devolviendo resultado
            return resultado;
        }


        #endregion

        #region Métodos de Consumo
        /// <summary>
        /// Publica Unidad
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion publicaUnidad()
        {
            //Establecemos variable resultado
            RetornoOperacion resultado = null;

            //Consumimos Web Service
            using (DespachoCentral.DespachoClient despacho = new DespachoCentral.DespachoClient())
            {
                //Instanciamos Compañia
                using (SAT_CL.Global.CompaniaEmisorReceptor objCompania = new CompaniaEmisorReceptor(((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                {
                    //Obtenemos Tiempo Limite de Disponibilidad de la Unidad
                    TimeSpan tiempoLimite = Convert.ToDateTime(txtLimiteDisponibilidad.Text).Subtract(Convert.ToDateTime(txtFechaDisponible.Text));

                    string resultado_web_service = despacho.PublicaUnidad(((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, objCompania.nombre,
                                       Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtUnidad.Text, ':', 1)), Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtOperador.Text, ':', 1)),
                                      Cadena.RegresaCadenaSeparada(txtCiudadDisponible.Text, "ID:", 0), Catalogo.RegresaDescripcioValorCadena(16, Convert.ToInt32(ddlEstadoDisponible.SelectedValue)), Fecha.ConvierteDateTimeString(Convert.ToDateTime(txtFechaDisponible.Text), "yyyy-MM-dd HH:mm"), Convert.ToInt32(tiempoLimite.TotalMinutes), txtObservacion.Text, SAT_CL.Despacho.ConsumoSOAPCentral.ObtieneCiudadesDeseadas(this._mitCiudadesDeseadas),
                                         CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Direccion del Servidor", 0), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Nombre de BD", 0),
                                       TSDK.Base.Cadena.VerificaCadenaVacia(SAT_CL.Global.Referencia.CargaReferencia("0", 25, objCompania.id_compania_emisor_receptor, "Consumo Web Service", "Contraseña"), ""),
                                       ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).nombre);


                    //Obtenemos Documento generado
                    XDocument xDoc = XDocument.Parse(resultado_web_service);

                    //Validamos que exista Respuesta
                    if (xDoc != null)
                    {
                        //Traduciendo resultado
                        resultado = new RetornoOperacion(Convert.ToInt32(xDoc.Descendants("idRegistro").FirstOrDefault().Value), xDoc.Descendants("mensaje").FirstOrDefault().Value.ToString(), Convert.ToBoolean(xDoc.Descendants("operacionExitosa").FirstOrDefault().Value));

                        //Validamos Resultado
                        if (resultado.OperacionExitosa)
                        {
                            //Personalizamos Mensaje
                            resultado = new RetornoOperacion("La Unidad ha sido Publicada", true);
                        }

                    }
                    else
                    {
                        //Establecmos Mensaje Resultado
                        resultado = new RetornoOperacion("No es posible obtener la respuesta de WS");
                    }
                }
                //Cerramos Conexion
                despacho.Close();
            }
            return resultado;
        }

        /// <summary>
        /// Insertamos Operador
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion insertaOperador()
        {
            //Establecemos variable resultado
            RetornoOperacion resultado = null;

            //Consumimos Web Service
            using (GlobalCentral.GlobalClient global = new GlobalCentral.GlobalClient())
            {
                //Instanciamos Operador
                using (SAT_CL.Global.Operador objOperador = new Operador(Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtOperador.Text, ':', 1))))
                {
                    string resultado_web_service = global.InsertaOperadorCentral(objOperador.id_operador, objOperador.nombre, Convert.ToByte((TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().AddTicks(-objOperador.fecha_nacimiento.Ticks).Year - 1)),
                                                   objOperador.r_control, chkLicenciaVigente.Checked, CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Direccion del Servidor", 0), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Nombre de BD", 0),
                                   TSDK.Base.Cadena.VerificaCadenaVacia(SAT_CL.Global.Referencia.CargaReferencia("0", 25, ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "Consumo Web Service", "Contraseña"), ""),
                                                 ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).nombre);

                    //Obtenemos Documento generado
                    XDocument xDoc = XDocument.Parse(resultado_web_service);

                    //Validamos que exista Respuesta
                    if (xDoc != null)
                    {
                        //Traduciendo resultado
                        resultado = new RetornoOperacion(Convert.ToInt32(xDoc.Descendants("idRegistro").FirstOrDefault().Value), xDoc.Descendants("mensaje").FirstOrDefault().Value.ToString(), Convert.ToBoolean(xDoc.Descendants("operacionExitosa").FirstOrDefault().Value));

                        //Validamos Resultado
                        if (resultado.OperacionExitosa)
                        {
                            //Personalizamos Mensaje
                            resultado = new RetornoOperacion("El operador ha sido registrado", true);
                        }

                    }
                    else
                    {
                        //Establecmos Mensaje Resultado
                        resultado = new RetornoOperacion("No es posible obtener la respuesta de WS");
                    }
                }
                //Cerramos Conexión
                global.Close();
            }
            return resultado;
        }

        /// <summary>
        /// Insertamos Unidad
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion insertaUnidad()
        {
            //Establecemos variable resultado
            RetornoOperacion resultado = null;

            //Consumimos Web Service
            using (GlobalCentral.GlobalClient global = new GlobalCentral.GlobalClient())
            {
                //Instanciamos Unidad
                using (SAT_CL.Global.Unidad objUnidad = new Unidad(Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtUnidad.Text, ':', 1))))
                {
                    //Declaramos Variable paara validar existencia de Antena
                    bool antena = false;
                    //Validamos que Tenga Antena Principa
                    if (objUnidad.antena_gps_principal != "")
                    {
                        antena = true;
                    }
                    //Instanciamos Tipo de Unidad
                    using (SAT_CL.Global.UnidadTipo objUnidadTipo = new UnidadTipo(objUnidad.id_tipo_unidad))
                    {
                        string resultado_web_service = global.InsertaUnidadCentral(objUnidad.id_unidad, objUnidad.numero_unidad, objUnidadTipo.descripcion_unidad,
                                                                                  Catalogo.RegresaDescripcionCatalogo(1109, objUnidad.id_sub_tipo_unidad), objUnidad.modelo,
                                                                                  objUnidad.ano, objUnidad.placas, antena, Catalogo.RegresaDescripcionCatalogo(1108, objUnidad.id_dimension),
                                                                                   CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Direccion del Servidor", 0), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Nombre de BD", 0),
                                   TSDK.Base.Cadena.VerificaCadenaVacia(SAT_CL.Global.Referencia.CargaReferencia("0", 25, ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "Consumo Web Service", "Contraseña"), ""),
                                                                                  ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).nombre);



                        //Obtenemos Documento generado
                        XDocument xDoc = XDocument.Parse(resultado_web_service);

                        //Validamos que exista Respuesta
                        if (xDoc != null)
                        {
                            //Traduciendo resultado
                            resultado = new RetornoOperacion(Convert.ToInt32(xDoc.Descendants("idRegistro").FirstOrDefault().Value), xDoc.Descendants("mensaje").FirstOrDefault().Value.ToString(), Convert.ToBoolean(xDoc.Descendants("operacionExitosa").FirstOrDefault().Value));

                            //Validamos Resultado
                            if (resultado.OperacionExitosa)
                            {
                                //Personalizamos Mensaje
                                resultado = new RetornoOperacion("La Unidad ha sido registrada", true);
                            }
                        }
                        else
                        {
                            //Establecmos Mensaje Resultado
                            resultado = new RetornoOperacion("No es posible obtener la respuesta de WS");
                        }
                    }
                }
                //Cerramos conexión
                global.Close();
            }
            return resultado;
        }

        /// <summary>
        /// Método encargado de Publicar la Unidad
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion PublicaUnidad()
        {
            //Declaramos Objeto Retorno
            RetornoOperacion resultado = new RetornoOperacion();

            //Publicamos Unidad
            resultado = publicaUnidad();

            //Si no Existe Unidad U Operador
            if (resultado.IdRegistro == -3 || resultado.IdRegistro == -4)
            {
                //Si no existe Unidad
                if (resultado.IdRegistro == -3)
                {
                    //Insertamos Unidad
                    resultado = insertaUnidad();

                    //Validamos Resultado
                    if (resultado.OperacionExitosa)
                    {
                        //Publicamos Unidad
                        resultado = publicaUnidad();

                        //Validamos resultado
                        if (resultado.OperacionExitosa)
                        {
                            //Actualziamos datos de la Unidad
                            consumoActualizaUnidad();
                            //Actualiamos datos del Operador
                            consumoActualizaOperador();
                        }
                    }
                }
                //Si no existe Operador
                if (resultado.IdRegistro == -4)
                {
                    //Insertamos Operador
                    resultado = insertaOperador();

                    //Validamos Resultado
                    if (resultado.OperacionExitosa)
                    {
                        //Publicamos Unidad
                        resultado = publicaUnidad();

                        //Validamos resultado
                        if (resultado.OperacionExitosa)
                        {
                            //Actualziamos datos de la Unidad
                            consumoActualizaUnidad();
                            //Actualiamos datos del Operador
                            consumoActualizaOperador();
                        }
                    }
                }
            }

            //Devolvemos Valor
            return resultado;
        }

        /// <summary>
        /// Obtenemos Información de la Publicación
        /// </summary>
        /// <returns></returns>
        private DataTable consumoInicializaPublicacion()
        {
            //Establecemos variable resultado
            DataSet ds = new DataSet();
            DataTable mit = null;
            RetornoOperacion resultado = new RetornoOperacion();

            //Consumimos Web Service
            using (DespachoCentral.DespachoClient despacho = new DespachoCentral.DespachoClient())
            {

                string resultado_web_service = despacho.ObtieneDatosPublicacion(this._id_publicacion, CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Direccion del Servidor", 0), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Nombre de BD", 0),
                                   TSDK.Base.Cadena.VerificaCadenaVacia(SAT_CL.Global.Referencia.CargaReferencia("0", 25, ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "Consumo Web Service", "Contraseña"), ""));

                //Obtenemos Documento generado
                XDocument xDoc = XDocument.Parse(resultado_web_service);

                //Validamos que exista Respuesta
                if (xDoc.Descendants("idRegistro").FirstOrDefault().Value != null)
                {
                    //Traduciendo resultado
                    resultado = new RetornoOperacion(Convert.ToInt32(xDoc.Descendants("idRegistro").FirstOrDefault().Value), xDoc.Descendants("mensaje").FirstOrDefault().Value.ToString(), Convert.ToBoolean(xDoc.Descendants("operacionExitosa").FirstOrDefault().Value));

                }
                //Validamos Resultado
                if (resultado.OperacionExitosa)
                {
                    //Obtenemos DataSet
                    ds.ReadXml(xDoc.Document.Element("Publicacion").Element("NewDataSet").CreateReader());
                    //Asignamos tabla
                    mit = ds.Tables["Table"];
                }
                else
                {
                    //Establecmos Mensaje Resultado
                    resultado = new RetornoOperacion("No es posible obtener la respuesta de WS");
                }
                //Si Existe Error
                if (!resultado.OperacionExitosa)
                {
                    //Mostrando Mensaje de Operación
                    TSDK.ASP.ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
                }

                despacho.Close();
            }
            return mit;
        }

        /// <summary>
        /// Obtenemos Información de las Ciudades Deseadas
        /// </summary>
        /// <returns></returns>
        private DataTable consumoInicializaCiudadesDeseadas()
        {
            //Establecemos variable resultado
            DataSet ds = new DataSet();
            DataTable mit = null;
            RetornoOperacion resultado = new RetornoOperacion();

            //Consumimos Web Service
            using (DespachoCentral.DespachoClient despacho = new DespachoCentral.DespachoClient())
            {

                string resultado_web_service = despacho.ObtieneCiudadesPublicacion(this._id_publicacion, CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Direccion del Servidor", 0), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Nombre de BD", 0),
                                   TSDK.Base.Cadena.VerificaCadenaVacia(SAT_CL.Global.Referencia.CargaReferencia("0", 25, ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "Consumo Web Service", "Contraseña"), ""));

                //Obtenemos Documento generado
                XDocument xDoc = XDocument.Parse(resultado_web_service);

                //Validamos que exista Respuesta
                if (xDoc.Descendants("idRegistro").FirstOrDefault().Value != null)
                {
                    //Traduciendo resultado
                    resultado = new RetornoOperacion(Convert.ToInt32(xDoc.Descendants("idRegistro").FirstOrDefault().Value), xDoc.Descendants("mensaje").FirstOrDefault().Value.ToString(), Convert.ToBoolean(xDoc.Descendants("operacionExitosa").FirstOrDefault().Value));

                }
                //Validamos Resultado
                if (resultado.OperacionExitosa)
                {
                    //Obtenemos DataSet
                    ds.ReadXml(xDoc.Document.Element("CiudadesPublicacion").Element("NewDataSet").CreateReader());
                    //Asignamos tabla
                    mit = ds.Tables["Table"];
                }
                else
                {
                    //Establecmos Mensaje Resultado
                    resultado = new RetornoOperacion("No es posible obtener la respuesta de WS");
                }
                //Si Existe Error
                if (!resultado.OperacionExitosa)
                {
                    //Mostrando Mensaje de Operación
                    TSDK.ASP.ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
                }

                despacho.Close();
            }
            return mit;
        }

        /// <summary>
        /// Actualiza los Datos del Operador de Hecosistema
        /// </summary>
        /// <returns></returns>
        private void consumoActualizaOperador()
        {
            //Establecemos variable resultado
            RetornoOperacion resultado = null;

            //Consumimos Web Service
            using (GlobalCentral.GlobalClient global = new GlobalCentral.GlobalClient())
            {
                //Instanciamos Unidad
                using (SAT_CL.Global.Operador objOperador = new Operador(Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtOperador.Text, ':', 1))))
                {
                    string resultado_web_service = global.InsertaOperadorCentral(objOperador.id_operador, objOperador.nombre, Convert.ToByte((TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().AddTicks(-objOperador.fecha_nacimiento.Ticks).Year - 1)),
                                                   objOperador.r_control, chkLicenciaVigente.Checked, CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Direccion del Servidor", 0), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Nombre de BD", 0),
                                   TSDK.Base.Cadena.VerificaCadenaVacia(SAT_CL.Global.Referencia.CargaReferencia("0", 25, ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "Consumo Web Service", "Contraseña"), ""),
                                                 ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).nombre);



                    //Obtenemos Documento generado
                    XDocument xDoc = XDocument.Parse(resultado_web_service);

                    //Validamos que exista Respuesta
                    if (xDoc != null)
                    {
                        //Traduciendo resultado
                        resultado = new RetornoOperacion(Convert.ToInt32(xDoc.Descendants("idRegistro").FirstOrDefault().Value), xDoc.Descendants("mensaje").FirstOrDefault().Value.ToString(), Convert.ToBoolean(xDoc.Descendants("operacionExitosa").FirstOrDefault().Value));

                        //Validamos Resultado
                        if (resultado.OperacionExitosa)
                        {
                            //Personalizamos Mensaje
                            resultado = new RetornoOperacion("El Operador ha sido actualizado", true);
                        }

                    }
                    else
                    {
                        //Establecmos Mensaje Resultado
                        resultado = new RetornoOperacion("No es posible obtener la respuesta de WS");
                    }
                }
                //Cerramos Web Service
                global.Close();
            }
        }

        /// <summary>
        /// Actualiza los Datos de la Unidad de Hecosistema
        /// </summary>
        /// <returns></returns>
        private void consumoActualizaUnidad()
        {
            //Establecemos variable resultado
            RetornoOperacion resultado = null;

            //Consumimos Web Service
            using (GlobalCentral.GlobalClient global = new GlobalCentral.GlobalClient())
            {
                 //Instanciamos Unidad
                using (SAT_CL.Global.Unidad objUnidad = new Unidad(Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtUnidad.Text, ':', 1))))
                {
                    //Declaramos Variable paara validar existencia de Antena
                    bool antena = false;
                    //Validamos que Tenga Antena Principa
                    if (objUnidad.antena_gps_principal != "")
                    {
                        antena = true;
                    }
                    //Instanciamos Tipo de Unidad
                    using (SAT_CL.Global.UnidadTipo objUnidadTipo = new UnidadTipo(objUnidad.id_tipo_unidad))
                    {
                        string resultado_web_service = global.InsertaUnidadCentral(objUnidad.id_unidad, objUnidad.numero_unidad, objUnidadTipo.descripcion_unidad,
                                                                                  Catalogo.RegresaDescripcionCatalogo(1109, objUnidad.id_sub_tipo_unidad), objUnidad.modelo,
                                                                                  objUnidad.ano, objUnidad.placas, antena, Catalogo.RegresaDescripcionCatalogo(1108, objUnidad.id_dimension),
                                                                                   CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Direccion del Servidor", 0), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Nombre de BD", 0),
                                   TSDK.Base.Cadena.VerificaCadenaVacia(SAT_CL.Global.Referencia.CargaReferencia("0", 25, ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "Consumo Web Service", "Contraseña"), ""),
                                                                                  ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).nombre);



                        //Obtenemos Documento generado
                        XDocument xDoc = XDocument.Parse(resultado_web_service);

                        //Validamos que exista Respuesta
                        if (xDoc != null)
                        {
                            //Traduciendo resultado
                            resultado = new RetornoOperacion(Convert.ToInt32(xDoc.Descendants("idRegistro").FirstOrDefault().Value), xDoc.Descendants("mensaje").FirstOrDefault().Value.ToString(), Convert.ToBoolean(xDoc.Descendants("operacionExitosa").FirstOrDefault().Value));

                            //Validamos Resultado
                            if (resultado.OperacionExitosa)
                            {
                                //Personalizamos Mensaje
                                resultado = new RetornoOperacion("La Unidad ha sido actualizada", true);
                            }

                        }
                        else
                        {
                            //Establecmos Mensaje Resultado
                            resultado = new RetornoOperacion("No es posible obtener la respuesta de WS");
                        }
                    }
                }
                //Cerramos Web Service
                global.Close();
            }
        }

        #endregion
    }
}