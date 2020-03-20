using SAT_CL.Despacho;
using SAT_CL.Global;
using SAT_CL.Seguridad;
using System;
using System.Collections.Generic;
using TSDK.ASP;
using TSDK.Base;

namespace SAT.UserControls
{
    public partial class MovimientoVacioSinOrden : System.Web.UI.UserControl
    {
        #region Atributos

        /// <summary>
        /// Id de Ubicación de origen del movimiento (ubicación actual de las unidades a mover)
        /// </summary>
        private int _id_ubicacion_origen;
        /// <summary>
        /// Id Recurso
        /// </summary>
        private int _id_recurso;
        /// <summary>
        /// Id Compañia Emisor
        /// </summary>
        private int _id_compania_emisor;
        /// <summary>
        /// Fecha de Llegada de la Unidad A mover
        /// </summary>
        private DateTime _fecha_llegada;
        /// <summary>
        /// Id Tipo Actualizacón Fin de la Estancia
        /// </summary>
        private EstanciaUnidad.TipoActualizacionFin _id_tipo_actualizacion_fin;
        /// <summary>
        /// Declaración de Evento ClickRegistrar
        /// </summary>
        public event EventHandler ClickRegistrar;
        /// <summary>
        /// Declaración de Evento ClickCancelar
        /// </summary>
        public event EventHandler ClickCancelar;
        /// <summary>
        /// Almacena el Nombre del Contenedor
        /// </summary>
        public string Contenedor;

        //TODO: Implementar lógica de devolución de Id para Movimiento Instanciado, de momento solo aplica para nuevo registro (captura)

        /// <summary>
        /// Obtiene el Id de Recurso Transportista seleccionado
        /// </summary>
        public int id_transportista { get { return Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtNombreTercero.Text, "ID:", 1)); } }
        /// <summary>
        /// Obtiene el Id de Recurso Unidad Motriz
        /// </summary>
        public int id_unidad { get { return Convert.ToInt32(ddlUnidad.SelectedValue); } }
        /// <summary>
        /// Obtiene el Id de Recurso Remolque 1
        /// </summary>
        public int id_remolque1 { get { return Convert.ToInt32(ddlRemolque1.SelectedValue); } }
        /// <summary>
        /// Obtiene el Id de Recurso Remolque 2
        /// </summary>
        public int id_remolque2 { get { return Convert.ToInt32(ddlRemolque2.SelectedValue); } }
        /// <summary>
        /// Obtiene el Id de Recurso Dolly
        /// </summary>
        public int id_dolly { get { return Convert.ToInt32(ddlDolly.SelectedValue); } }
        /// <summary>
        /// Obtiene la fecha de inicio del movimiento
        /// </summary>
        public DateTime fecha_inicio
        {
            get
            {
                DateTime fecha = DateTime.MinValue;
                DateTime.TryParse(txtFechaSalidaUnidades.Text, out fecha);
                return fecha;
            }
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Evento generado al Cargar la Página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Validando  PostBack
            if (!this.IsPostBack)
            {

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
        /// Manipula Evento ClickRegistrar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void OnClickRegistrar(EventArgs e)
        {
            if (ClickRegistrar != null)
                ClickRegistrar(this, e);

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
        /// Evento disparado al presionar el boton Registrar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnClick_btnRegistrar(object sender, EventArgs e)
        {
            //Validando que exista un Evento
            if (ClickRegistrar != null)
                OnClickRegistrar(new EventArgs());
        }
        /// <summary>
        /// Evento disparado al presionar el boton Cancelar
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
        /// Evento generado al cambiar chkTercero
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkTercero_OnCheckedChanged(object sender, EventArgs e)
        {
            //Validamos si ésta marcado Tercero
            if (chkTercero.Checked)
            {
                //Habilitamos control Tercero 
                txtNombreTercero.Enabled = true;
                //Deshabilitamos Unidad
                ddlUnidad.Enabled = false;
                ddlUnidad.SelectedValue = "0";
                txtOperador.Text = "NO DISPONIBLE ID:0";
            }
            else
            {
                //Limpiamos control Tercero
                txtNombreTercero.Text = "";
                //Deshabilitamos  Controles Tercero
                txtNombreTercero.Enabled = false;
                //Habilitamos Unidad
                ddlUnidad.Enabled = true;
                ddlUnidad.SelectedValue = "0";

            }
            //Cambiamos Fecha de LLegada por la 
            lblFechaLlegada.Text = this._fecha_llegada.ToString("dd/MM/yyyy HH:mm");
        }

        /// <summary>
        /// Evento generado al cambiar la selección de la Unidad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlUnidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Instanciamos Unidad
            using (Unidad objUnidad = new Unidad(Convert.ToInt32(ddlUnidad.SelectedValue)))
            {
                //Instanciamos Operador
                using (Operador objOperador = new Operador(objUnidad.id_operador))
                {
                    //Validamos que exista Operador
                    if (objOperador.id_operador > 0)
                    {
                        //Inicializamos Valor
                        txtOperador.Text = objOperador.nombre + " ID:" + objOperador.id_operador.ToString();
                    }
                    else
                    {
                        //Inicializamos Control
                        txtOperador.Text = "NO DISPONIBLE ID:0";
                    }
                }
            }

            //Inicicializa Control Fecha de Llegada
            inicializaControlFechaLlegada(Convert.ToInt32(ddlUnidad.SelectedValue));
        }

        /// <summary>
        /// Evento generado al Cambiar la Seleccion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlRemolque1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Inicializa Control de Fecha de Llegada
            inicializaControlFechaLlegada(Convert.ToInt32(ddlRemolque1.SelectedValue));
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Cargamos Catalogos
        /// </summary>
        private void cargaCatalogos(int id_ubicacion)
        {
            //Cargando Catalogos Unidad
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlUnidad, 66, "Sin Asignar", ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", id_ubicacion, "");

            //Cargando Catalogos Arrastre 1
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlRemolque1, 55, "Sin Asignar", ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", id_ubicacion, "");

            //Cargando Catalogos Arrastre 2
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlRemolque2, 55, "Sin Asignar", ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", id_ubicacion, "");

            //Cargando Catalogos Arrastre 2
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlDolly, 32, "Sin Asignar", ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "3", id_ubicacion, "");
        }

        /// <summary>
        /// Método  encargado de Inicializar controles para Unidad Motriz
        /// </summary>
        private void inicializaValoresUnidad(int id_Unidad)
        {
            txtFechaSalidaUnidades.Text = "";
            chkTercero.Checked = false;
            txtUbicacion.Text = "";
            txtNombreTercero.Text = "";
            ddlUnidad.SelectedValue = id_Unidad.ToString();
            ddlRemolque1.SelectedValue = "0";
            ddlRemolque2.SelectedValue = "0";
            ddlDolly.SelectedValue = "0";
            //Instanciamos Unidad
            using (Unidad objUnidad = new Unidad(id_Unidad))
            {
                //Instanciamos Operador
                using (Operador objOperador = new Operador(objUnidad.id_operador))
                {
                    //Si existe Operador
                    if (objOperador.id_operador > 0)
                    {
                        //Iniciamizamos Control de Operador
                        txtOperador.Text = objOperador.nombre + "ID:" + objOperador.id_operador.ToString();
                    }
                    else
                    {
                        //Iniciamizamos Control de Operador
                        txtOperador.Text = "NO DISPONIBLE ID:0";
                    }
                }
            }
            lblError.Text =
            txtRemolque13ero.Text =
            txtRemolque23ero.Text = "";
        }

        /// <summary>
        /// Método  encargado de Inicializar controles para Remolque
        /// </summary>
        private void inicializaValoresRemolque(int id_Unidad)
        {
            lblFechaLlegada.Text = "";
            txtFechaSalidaUnidades.Text = "";
            chkTercero.Checked = false;
            txtUbicacion.Text = "";
            txtOperador.Text = "NO DIPONIBLE ID: 0";
            txtNombreTercero.Text = "";
            ddlUnidad.SelectedValue = "0";
            ddlRemolque1.SelectedValue = id_Unidad.ToString();
            ddlRemolque2.SelectedValue = "0";
            ddlDolly.SelectedValue = "0";
            lblError.Text =
            txtRemolque13ero.Text =
            txtRemolque23ero.Text = "";
        }

        /// <summary>
        /// Método  encargado de habilitar controles para Unidad Motriz
        /// </summary>
        /// <param name="permite_arrastre">True si la unidad permitirá unidades de arrastre</param>
        private void habilitarControlesUnidad(bool permite_arrastre)
        {
            lblFechaLlegada.Text = "";
            txtFechaSalidaUnidades.Enabled = true;
            txtUbicacion.Enabled = true;
            chkTercero.Enabled =
            txtNombreTercero.Enabled =
            txtOperador.Enabled =
            ddlUnidad.Enabled = false;

            ddlRemolque1.Enabled =
            ddlRemolque2.Enabled =
            ddlDolly.Enabled = permite_arrastre;
            txtRemolque13ero.Enabled =
            txtRemolque23ero.Enabled =
            btnAceptarMovimientoVacioSinOrden.Enabled =
            btnCancelarMovimientoVacioSinOrden.Enabled = true;
        }

        /// <summary>
        /// Método  encargado de habilitar controles para Remolque
        /// </summary>
        private void habilitaControlesRemolque()
        {
            txtFechaSalidaUnidades.Enabled = true;
            txtUbicacion.Enabled = true;
            chkTercero.Enabled = true;
            txtNombreTercero.Enabled = false;
            txtOperador.Enabled = false;
            ddlUnidad.Enabled = true;
            ddlRemolque1.Enabled = false;
            ddlRemolque2.Enabled = true;
            btnAceptarMovimientoVacioSinOrden.Enabled =
            btnCancelarMovimientoVacioSinOrden.Enabled =
            txtRemolque13ero.Enabled =
            txtRemolque23ero.Enabled =
            ddlDolly.Enabled = true;
        }

        /// <summary>
        /// Recupera Atributos
        /// </summary>
        private void recuperaAtributos()
        {
            //Recuperando Atributos
            if (Convert.ToInt32(ViewState["IdUbicacionOrigen"]) != 0)
                this._id_ubicacion_origen = Convert.ToInt32(ViewState["IdUbicacionOrigen"]);
            //Recuperando Atributos
            if (Convert.ToInt32(ViewState["IdCompaniaEmisor"]) != 0)
                this._id_compania_emisor = Convert.ToInt32(ViewState["IdCompaniaEmisor"]);
            //Recuperando Atributos
            if (Convert.ToInt32(ViewState["IdRecurso"]) != 0)
                this._id_recurso = Convert.ToInt32(ViewState["IdRecurso"]);
            //Recuperando Atributos
            if (Convert.ToInt32(ViewState["IdTipoActualizacionFin"]) != 0)
                this._id_tipo_actualizacion_fin = (EstanciaUnidad.TipoActualizacionFin)Convert.ToByte(ViewState["IdTipoActualizacionFin"]);
        }
        /// <summary>
        /// Asigna Atributos
        /// </summary>
        private void asignaAtributos()
        {
            ViewState["IdUbicacionOrigen"] = this._id_ubicacion_origen;
            ViewState["IdRecurso"] = this._id_recurso;
            ViewState["IdCompaniaEmisor"] = this._id_compania_emisor;
            ViewState["IdTipoActualizacionFin"] = this._id_tipo_actualizacion_fin;
        }
        /// <summary>
        /// Inicializamos Controles Movimiento en Vacio Sin Orden
        /// </summary>
        /// <param name="id_recurso">Id Recurso</param>
        /// <param name="id_compania_emisor">Id Compañia Emisor</param>
        /// <param name="tipoActualizaionFin">Tipo de Actualización Fin de la Estancia (Manual, GPS, APP)</param>
        public RetornoOperacion InicializaControl(int id_recurso, int id_compania_emisor, Parada.TipoActualizacionLlegada tipoActualizacionLlegada, Parada.TipoActualizacionSalida tipoActualizacionSalida, EstanciaUnidad.TipoActualizacionInicio tipoActuailizacionInicio,
            EstanciaUnidad.TipoActualizacionFin tipoActualizaionFin)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();
            //Asignamo Atributos
            this._id_tipo_actualizacion_fin = tipoActualizaionFin;
            this._id_recurso = id_recurso;
            this._id_compania_emisor = id_compania_emisor;
            this._id_ubicacion_origen = 0;  //Se actualiza con la ubicación de la unidad seleccionada
            chkFechaActualMov.Checked = false;

            //Obtenemos Estancia Actual de la Unidad
            using (EstanciaUnidad objEstanciaUnidad = new EstanciaUnidad(EstanciaUnidad.ObtieneEstanciaUnidadIniciada(id_recurso)))
            {
                //Validamos Estancia de la Unidad
                if (objEstanciaUnidad.id_estancia_unidad > 0)
                {
                    this._fecha_llegada = objEstanciaUnidad.inicio_estancia;
                    //Instanciamos Parada
                    using (Parada objParada = new Parada(objEstanciaUnidad.id_parada))
                    {
                        //Instanciamos Unidad
                        using (SAT_CL.Global.Unidad objUnidad = new SAT_CL.Global.Unidad(id_recurso))
                        {
                            //Si existe unidad a mover
                            if (objUnidad.id_unidad > 0 && objUnidad.EstatusUnidad == Unidad.Estatus.ParadaDisponible)
                            {
                                //Instanciamos Tipo de Unidad
                                using (SAT_CL.Global.UnidadTipo objUnidadTipo = new SAT_CL.Global.UnidadTipo(objUnidad.id_tipo_unidad))
                                {
                                    //Guardando ubicación de origen del movimiento
                                    this._id_ubicacion_origen = objParada.id_ubicacion;

                                    //Cargamos Catalogos
                                    cargaCatalogos(objParada.id_ubicacion);

                                    //Validamos Tipo de Unidad para Inicializar y habilitar Controles
                                    if (objUnidadTipo.bit_motriz == true)
                                    {
                                        //Inicializamos Controles
                                        inicializaValoresUnidad(objUnidad.id_unidad);
                                        //Habilitamos Controles
                                        habilitarControlesUnidad(objUnidadTipo.bit_permite_arrastre);
                                        //sI LA 
                                    }
                                    else
                                    {
                                        //Inicializamos Controles
                                        inicializaValoresRemolque(objUnidad.id_unidad);
                                        //Habilitamos Controles
                                        habilitaControlesRemolque();
                                    }
                                }
                                //Establecemos Resultado Exitoso
                                resultado = new RetornoOperacion(0);
                            }
                        }

                        //Inicializando Control de Lugar de Origen
                        lblLugarOrigen.Text = objParada.descripcion;
                        //Inicializamos Control Fecha de Llegada al Origen
                        lblFechaLlegada.Text = objEstanciaUnidad.inicio_estancia.ToString("dd/MM/yyyy HH:mm").ToString();
                    }
                }
            }
            //Devolvemos Resultado
            return resultado;
        }
        /// <summary>
        /// Registramos Movimiento en Vacio
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion RegistraMovimientoVacioSinOrden()
        {
            //Declaramos objeto Resultante
            RetornoOperacion resultado = new RetornoOperacion();

            //Validamos que los Remolque sean Diferentes
            if (ddlRemolque1.SelectedValue != ddlRemolque2.SelectedValue || (ddlRemolque1.SelectedValue == "0" && ddlRemolque2.SelectedValue == "0"))
            {
                //Creando Lista de unidades de arrastre
                List<int> arrastre = new List<int>();
                arrastre.Add(Convert.ToInt32(ddlRemolque1.SelectedValue));
                arrastre.Add(Convert.ToInt32(ddlRemolque2.SelectedValue));
                arrastre.Add(Convert.ToInt32(ddlDolly.SelectedValue));
                arrastre.RemoveAll(item => item == 0);

                //Insertamos movimiento en Vacio
                resultado = Movimiento.InsertaMovimientoVacio(this._id_ubicacion_origen, Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtUbicacion.Text, ':', 1)), this._id_compania_emisor,
                                                            Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtOperador.Text, ':', 1)), Convert.ToInt32(ddlUnidad.SelectedValue),
                                                            Convert.ToInt32(Cadena.VerificaCadenaVacia(Cadena.RegresaCadenaSeparada(txtNombreTercero.Text, ':', 1), "0")), arrastre,
                                                            Convert.ToDateTime(txtFechaSalidaUnidades.Text), this._id_tipo_actualizacion_fin, txtRemolque13ero.Text, txtRemolque23ero.Text,
                                                            ((Usuario)Session["usuario"]).id_usuario);


                //Validamos Resultado
                if (resultado.OperacionExitosa)
                {
                    //Inicializamos Controles
                    InicializaControl(this._id_recurso, this._id_compania_emisor, Parada.TipoActualizacionLlegada.Manual, Parada.TipoActualizacionSalida.Manual, EstanciaUnidad.TipoActualizacionInicio.Manual, this._id_tipo_actualizacion_fin);
                }
                else
                {
                    //Mostrando Mensaje Error
                    lblError.Text = resultado.Mensaje;
                }
            }
            else
            {
                //Mostramos Mensaje Error
                lblError.Text = "No puedes seleccionar remolques iguales.";
            }

            return resultado;
        }
        /// <summary>
        /// Cancelamos Movimiento en Vacio
        /// </summary>
        /// <param name="nombreScript"></param>
        /// <param name="nombreVentanas"></param>
        public void CancelaModalMovimientoVacioSinOrden(string nombreScript, params string[] nombreVentanas)
        {
            //Mostramos Ventana Modal
            ScriptServer.AlternarVentana(btnCancelarMovimientoVacioSinOrden, btnCancelarMovimientoVacioSinOrden.GetType(), nombreScript, nombreVentanas);
        }
        /// <summary>
        /// Ocultamos Ventana Modal que contiene el Control tras Registrar
        /// </summary>
        /// <param name="nombreScript"></param>
        /// <param name="nombreVentanas"></param>
        public void OcultaVentanaRegistrar(string nombreScript, params string[] nombreVentanas)
        {
            //Mostramos Ventana Modal
            ScriptServer.AlternarVentana(btnAceptarMovimientoVacioSinOrden, btnAceptarMovimientoVacioSinOrden.GetType(), nombreScript, nombreVentanas);
        }
        /// <summary> 
        /// Método encargado de Inicializa el Control de la Fecha de Llegada
        /// </summary>
        /// <param name="id_recurso"></param>
        private void inicializaControlFechaLlegada(int id_recurso)
        {
            //Obtenemos Estancia Actual de la Unidad
            using (EstanciaUnidad objEstanciaUnidad = new EstanciaUnidad(EstanciaUnidad.ObtieneEstanciaUnidadIniciada(id_recurso)))
            {
                //Validamos Estancia de la Unidad
                if (objEstanciaUnidad.id_estancia_unidad > 0)
                {
                    //En caso de que la Fecha de Inicio de la Estancia sea mayor a la establecida por la Unidad que se desea mover
                    if (objEstanciaUnidad.inicio_estancia > this._fecha_llegada)
                    {
                        // Cambiamos Valor del Control de fecha de llegada 
                        lblFechaLlegada.Text = objEstanciaUnidad.inicio_estancia.ToString("dd/MM/yyyy HH:mm").ToString();
                    }
                }
            }
        }

        #endregion

        /// <summary>
        /// Evento generado al Cambiar de Selección
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlDolly_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Inicializa Control de Fecha de Llegada
            inicializaControlFechaLlegada(Convert.ToInt32(ddlRemolque1.SelectedValue));
        }

        /// <summary>
        /// //Evento generado al Cambiar de Selección
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlRemolque2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Inicializa Control de Fecha de Llegada
            inicializaControlFechaLlegada(Convert.ToInt32(ddlRemolque1.SelectedValue));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkFechaActualMov_CheckedChanged(object sender, EventArgs e)
        {
            //Asignando fecha actual al control de fecha de salida del movimiento (inicio)
            txtFechaSalidaUnidades.Text = chkFechaActualMov.Checked ? Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm") : "";
        }


    }
}