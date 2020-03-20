using SAT_CL.Despacho;
using SAT_CL.Global;
using SAT_CL.Seguridad;
using System;
using System.Collections.Generic;
using TSDK.Base;


namespace SAT.UserControls
{
    public partial class wucMovimientoVacio : System.Web.UI.UserControl
    {
        #region Atributos

        /// <summary>
        /// Id de Ubicación Origen del Movimiento (ubicación actual de las unidades a mover)
        /// </summary>
        private int _id_ubicacion_origen;
        /// <summary>
        /// Id Servicio
        /// </summary>
        private int _id_servicio;
        /// <summary>
        /// Id Parada
        /// </summary>
        private int _id_parada;
        /// <summary>
        /// Id Compania Emisor
        /// </summary>
        private int _id_compania_emisor;
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
        }
        #endregion

        #region Métodos

        /// <summary>
        /// Cargamos Catalogos
        /// </summary>
        private void cargaCatalogos(int id_ubicacion)
        {
            //Cargando Catalogos Unidad
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlUnidad, 32, "Sin Asignar", ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "1", id_ubicacion, "");

            //Cargando Catalogos Arrastre 1
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlRemolque1, 32, "Sin Asignar", ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "2", id_ubicacion, "");

            //Cargando Catalogos Arrastre 2
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlRemolque2, 32, "Sin Asignar", ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "2", id_ubicacion, "");

            //Cargando Catalogos Arrastre 2
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlDolly, 32, "Sin Asignar", ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "3", id_ubicacion, "");
        }

        /// <summary>
        /// Método  encargado de Inicializar controles para Unidad Motriz
        /// </summary>
        private void inicializaValores()
        {
            txtFechaSalidaUnidades.Text = "";
            chkTercero.Checked = false;
            txtNombreTercero.Text = "";
            ddlUnidad.SelectedValue = "0";
            ddlRemolque1.SelectedValue = "0";
            ddlRemolque2.SelectedValue = "0";
            ddlDolly.SelectedValue = "0";
            lblError.Text =
            txtRemolque13ero.Text =
            txtRemolque23ero.Text = "";
        }

        /// <summary>
        /// Método  encargado de Inicializar controles para Unidad Motriz
        /// </summary>
        private void inicializaValoresUnidad(int id_Unidad)
        {
            txtFechaSalidaUnidades.Text = "";
            chkTercero.Checked = false;
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
            txtFechaSalidaUnidades.Text = "";
            chkTercero.Checked = false;
            txtNombreTercero.Text = "";
            ddlUnidad.SelectedValue = "0";
            ddlRemolque1.SelectedValue = id_Unidad.ToString();
            ddlRemolque2.SelectedValue = "0";
            ddlDolly.SelectedValue = "0";
            txtOperador.Text = "NO DISPONIBLE ID:0";
            lblError.Text =
            txtRemolque13ero.Text =
            txtRemolque23ero.Text = "";
        }

        /// <summary>
        /// Método  encargado de habilitar controles 
        /// </summary>
        private void habilitarControles()
        {
            txtFechaSalidaUnidades.Enabled = true;
            chkTercero.Enabled =
            txtNombreTercero.Enabled = true;
            ddlUnidad.Enabled =
            ddlRemolque1.Enabled =
            ddlRemolque2.Enabled =
            ddlDolly.Enabled =
            txtRemolque13ero.Enabled =
            txtRemolque23ero.Enabled =
            btnAceptarMovimientoVacio.Enabled =
            btnCancelarMovimientoVacio.Enabled = false;
        }

        /// <summary>
        /// Método  encargado de habilitar controles para Unidad Motriz
        /// </summary>
        private void habilitarControlesUnidad()
        {
            txtFechaSalidaUnidades.Enabled = true;
            chkTercero.Enabled =
            txtNombreTercero.Enabled =
            ddlUnidad.Enabled = false;
            ddlRemolque1.Enabled =
            ddlRemolque2.Enabled =
            txtRemolque13ero.Enabled =
            txtRemolque23ero.Enabled =
            btnAceptarMovimientoVacio.Enabled =
            btnCancelarMovimientoVacio.Enabled =
            ddlDolly.Enabled = true;
        }

        /// <summary>
        /// Método  encargado de habilitar controles para Remolque
        /// </summary>
        private void habilitaControlesRemolque()
        {
            txtFechaSalidaUnidades.Enabled = true;
            chkTercero.Enabled = true;
            txtNombreTercero.Enabled = false;
            ddlUnidad.Enabled = true;
            ddlRemolque1.Enabled = false;
            ddlRemolque2.Enabled = true;
            txtRemolque13ero.Enabled =
            txtRemolque23ero.Enabled = false;
            btnAceptarMovimientoVacio.Enabled =
           btnCancelarMovimientoVacio.Enabled =
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
            if (Convert.ToInt32(ViewState["IdServicio"]) != 0)
                this._id_servicio = Convert.ToInt32(ViewState["IdServicio"]);
            //Recuperando Atributos
            if (Convert.ToInt32(ViewState["IdParada"]) != 0)
                this._id_parada = Convert.ToInt32(ViewState["IdParada"]);
            //Recuperando Atributos
            if (Convert.ToInt32(ViewState["IdCompania"]) != 0)
                this._id_compania_emisor = Convert.ToInt32(ViewState["IdCompania"]);
            //Recuperando Atributos
            if (Convert.ToInt32(ViewState["IdTipoActualizacionFin"]) != 0)
                this._id_tipo_actualizacion_fin = (EstanciaUnidad.TipoActualizacionFin)Convert.ToByte(ViewState["IdTipoActualizacionFin"]);
        }

        /// <summary>
        /// Asigna Atributos
        /// </summary>
        private void asignaAtributos()
        {
            //Asignando Atributos
            ViewState["IdServicio"] = this._id_servicio;
            ViewState["IdParada"] = this._id_parada;
            ViewState["IdCompania"] = this._id_compania_emisor;
            ViewState["IdTipoActualizacionFin"] = this._id_tipo_actualizacion_fin;
            ViewState["IdUbicacionOrigen"] = this._id_ubicacion_origen;
        }

        /// <summary>
        /// Inicializamos Controles Movimiento en Vacio
        /// </summary>
        /// <param name="idCompania">Id Compania del Servicio</param>
        /// <param name="idParadaActual">Id Parada Actual</param>
        /// <param name="tipoActualizaionFin">Tipo de Actualización Fin de la Estancia (Manual, GPS, APP)</param>
        public void InicializaControl(int idCompania, int idParadaActual, Parada.TipoActualizacionLlegada tipoActualizacionLlegada, Parada.TipoActualizacionSalida tipoActualizacionSalida, EstanciaUnidad.TipoActualizacionInicio tipoActuailizacionInicio,
                             EstanciaUnidad.TipoActualizacionFin tipoActualizaionFin)
        {
            //Asignamo Atributos
            this._id_compania_emisor = idCompania;
            this._id_tipo_actualizacion_fin = tipoActualizaionFin;
            this._id_parada = idParadaActual;
            this._id_ubicacion_origen = 0;  //Se actualiza con el Id de Ubicación de la unidad principal del movimiento

            //Instanciamos Parada
            using (Parada objParada = new Parada(idParadaActual))
            {
                //Declaramos Variables para la Unidad por modificar ubicación
                int id_unidad = 0;

                //Asignando Atributos
                this._id_servicio = objParada.id_servicio;

                //Cargamos la Unidade que se asignarón a la Parada pero se encuentran en una ubicación distinta
                MovimientoAsignacionRecurso.CargaRecursoAsignadoAParadaDifUbicacion(idParadaActual, objParada.id_ubicacion, out id_unidad, out this._id_ubicacion_origen);

                //Instanciamos Unidad
                using (SAT_CL.Global.Unidad objUnidad = new SAT_CL.Global.Unidad(id_unidad))
                {
                    //Si existe unidad a mover
                    if (objUnidad.id_unidad > 0)
                    {

                        //Instanciamos Tipo de Unidad
                        using (SAT_CL.Global.UnidadTipo objUnidadTipo = new SAT_CL.Global.UnidadTipo(objUnidad.id_tipo_unidad))
                        {
                            //Cargamos Catalogos
                            cargaCatalogos(this._id_ubicacion_origen);

                            //Validamos Tipo de Unidad para Inicializar y habilitar Controles
                            if (objUnidadTipo.bit_motriz == true)
                            {
                                //Inicializamos Controles
                                inicializaValoresUnidad(objUnidad.id_unidad);
                                //Habilitamos Controles
                                habilitarControlesUnidad();
                            }
                            else
                            {
                                //Inicializamos Controles
                                inicializaValoresRemolque(objUnidad.id_unidad);
                                //Habilitamos Controles
                                habilitaControlesRemolque();
                            }
                        }
                    }
                    else
                    {
                        //Inicializamos Controles
                        inicializaValores();
                        //Habilitamos Controles
                        habilitarControles();
                    }
                }
            }
        }

        /// <summary>
        /// Registramos Movimiento en Vacio
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion RegistraMovimientoVacio()
        {
            //Declaramos objeto Resultante
            RetornoOperacion resultado = new RetornoOperacion();

            //Validamos que los Remolque sean Diferentes
            if (ddlRemolque1.SelectedValue != ddlRemolque2.SelectedValue || (ddlRemolque1.SelectedValue == "0" && ddlRemolque2.SelectedValue == "0"))
            {

                //Creando lista de unidades de arrastre involucradas
                List<int> arrastre = new List<int>();
                arrastre.Add(Convert.ToInt32(ddlRemolque1.SelectedValue));
                arrastre.Add(Convert.ToInt32(ddlRemolque2.SelectedValue));
                arrastre.Add(Convert.ToInt32(ddlDolly.SelectedValue));
                arrastre.RemoveAll(item => item == 0);

                //Insertamos movimiento en Vacio
                resultado = Movimiento.InsertaMovimientoVacio(this._id_ubicacion_origen, this._id_servicio, this._id_parada, this._id_compania_emisor,
                                        Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtOperador.Text, ':', 1)), Convert.ToInt32(ddlUnidad.SelectedValue),
                                        Convert.ToInt32(Cadena.VerificaCadenaVacia(Cadena.RegresaCadenaSeparada(txtNombreTercero.Text, ':', 1), "0")), arrastre,
                                        Convert.ToDateTime(txtFechaSalidaUnidades.Text), this._id_tipo_actualizacion_fin,
                                        txtRemolque13ero.Text, txtRemolque23ero.Text, ((Usuario)Session["usuario"]).id_usuario);


                //Validamos Resultado
                if (resultado.OperacionExitosa)
                {
                    //Inicializamos Controles
                    InicializaControl(this._id_compania_emisor, this._id_parada, Parada.TipoActualizacionLlegada.Manual, Parada.TipoActualizacionSalida.Manual,
                        EstanciaUnidad.TipoActualizacionInicio.Manual, this._id_tipo_actualizacion_fin);
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

        #endregion
    }
}


