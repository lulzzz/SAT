using Microsoft.SqlServer.Types;
using System;
using TSDK.Base;
using TSDK.ASP;
using System.Data;
using TSDK.Datos;
using System.Web.UI;

namespace SAT.UserControls
{
    public partial class wucBitacoraMonitoreo : System.Web.UI.UserControl
    {
        #region Atributos
        /// <summary>
        /// Id Bitacora Monitoreo
        /// </summary>
        private int _id_bitacora_monitoreo;
        /// <summary>
        /// Id Servicio
        /// </summary>
        private int _id_servicio;
        /// <summary>
        /// Id Servicio
        /// </summary>
        private int _id_parada;
        /// <summary>
        /// Id Servicio
        /// </summary>
        private int _id_evento;
        /// <summary>
        /// Id Movimiento
        /// </summary>
        private int _id_movimiento;
        /// <summary>
        /// Id Tabla
        /// </summary>
        private int _id_tabla;
        /// <summary>
        /// Id Registro
        /// </summary>
        private int _id_registro;
        /// Declaración de Evento ClickGuardar
        /// </summary>
        public event EventHandler ClickRegistrar;
        /// <summary>
        /// Declaración de Evento ClickEliminar
        /// </summary>
        public event EventHandler ClickEliminar;
        /// <summary>
        /// Declaración del evento ClickAceptar
        /// </summary>
        public event EventHandler ClickAceptar;
        /// <summary>
        /// Propiedad que establece el Orden de la Tabulación de los Controles
        /// </summary>
        public short TabIndex
        {
            set
            {   //Asignando Tab
                ddlTipo.TabIndex =
                txtFechaBitacora.TabIndex =
                txtComentario.TabIndex =
                txtUbicacion.TabIndex =
                btnEliminar.TabIndex =
                btnRegistrar.TabIndex = value;
            }
            get { return ddlTipo.TabIndex; }
        }

        #endregion

        #region Eventos
        /// <summary>
        /// Evento generado al cargar la Página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Validando  PostBack
            if (!this.IsPostBack)
            {
                //Carga catalogos
                //cargaCatalogos();
            }
            else
            {
                //Si es PostaBack
                //Recupera Atributos
                recuperaAtributos();
            }

            /*/Declarando Script
            string script = @"<script type='text/javascript'>
                                //Script de Elevación de Imagen
                                $('.myImageElevate').elevateZoom({ easing: true });
                              </script>";

            //Recuperando update panel de imagen con script jqzoom y registrando script de efectos
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "Elevate Zoom", script, false);//*/
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
        /// Manipula Evento ClickEliminar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void OnClickEliminar(EventArgs e)
        {
            //Validando que exista un Evento
            if (ClickEliminar != null)
                ClickEliminar(this, e);
        }
        /// Evento disparado al presionar el boton Eliminar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnClick_btnEliminar(object sender, EventArgs e)
        {
            //Validando que exista un Evento
            if (ClickEliminar != null)
                OnClickEliminar(new EventArgs());
            //Invoca al método que abre la ventana modal.
            ScriptServer.AlternarVentana(btnEliminar, upbtnEliminar.GetType(), "AbrirVentana", "contenidoBitacoraMonitoreo", "bitacoraMonitoreo");
        }
        /// <summary>
        /// Manipula Evento ClickAceptar
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnClickAceptar(EventArgs e)
        {
            //Validando que exista un Evento
            if (ClickAceptar != null)
                ClickAceptar(this, e);
        }
        /// <summary>
        /// Método que elimina el registro.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnClick_btnAceptar(object sender, EventArgs e)
        {
            //Validando que exista un Evento
            if (ClickAceptar != null)
                OnClickAceptar(new EventArgs());
            //Invoca al método que cierra la ventana modal
            ScriptServer.AlternarVentana(btnCancelar, upbtnCancelar.GetType(), "CerrarVentana", "contenidoBitacoraMonitoreo", "bitacoraMonitoreo");
        }
        /// <summary>
        /// Evento que cierra la ventana modal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnClick_btnCancelar(object sender, EventArgs e)
        {
            //Invoca al método que cierra la ventana modal. 
            ScriptServer.AlternarVentana(btnCancelar, upbtnCancelar.GetType(), "CerrarVentana", "contenidoBitacoraMonitoreo", "bitacoraMonitoreo");
        }
        /// <summary>
        /// Evento Producido al Dar Click en el Link 'Archivos'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbArchivos_Click(object sender, EventArgs e)
        {
            //Validando que exista el Registro
            if (this._id_bitacora_monitoreo > 0)
            {
                //Declarando variable para armado de URL
                string urlDestino = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/UserControls/wucBitacoraMonitoreo.aspx", "~/Accesorios/ArchivoRegistro.aspx?idT=150&idR=" + this._id_bitacora_monitoreo.ToString() + "&idTV=20");
                //Instanciando nueva ventana de navegador para apertura de referencias de registro
                TSDK.ASP.ScriptServer.AbreNuevaVentana(urlDestino, "Archivos", 800, 480, false, false, false, true, true, Page);
            }
            else
                //Instanciando Mensaje
                TSDK.ASP.ScriptServer.MuestraNotificacion(lkbArchivos, "No existe el registro para añadir las Evidencias", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        #endregion

        #region Métodos Privados
        /// <summary>
        /// Recupera Atributos
        /// </summary>
        private void recuperaAtributos()
        {
            //Recuperando Atributos
            if (Convert.ToInt32(ViewState["IdBitacoraMonitoreo"]) != 0)
                this._id_bitacora_monitoreo = Convert.ToInt32(ViewState["IdBitacoraMonitoreo"]);
            //Recuperando Atributos
            if (Convert.ToInt32(ViewState["IdServicio"]) != 0)
                this._id_servicio = Convert.ToInt32(ViewState["IdServicio"]);
            //Recuperando Atributos
            if (Convert.ToInt32(ViewState["IdParada"]) != 0)
                this._id_parada = Convert.ToInt32(ViewState["IdParada"]);
            //Recuperando Atributos
            if (Convert.ToInt32(ViewState["IdEvento"]) != 0)
                this._id_evento = Convert.ToInt32(ViewState["IdEvento"]);
            //Recuperando Atributos
            if (Convert.ToInt32(ViewState["IdMovimiento"]) != 0)
                this._id_movimiento = Convert.ToInt32(ViewState["IdMovimiento"]);
            //Recuperando Atributos
            if (Convert.ToInt32(ViewState["IdTabla"]) != 0)
                this._id_tabla = Convert.ToInt32(ViewState["IdTabla"]);
            //Recuperando Atributos
            if (Convert.ToInt32(ViewState["IdRegistro"]) != 0)
                this._id_registro = Convert.ToInt32(ViewState["IdRegistro"]);
        }

        /// <summary>
        /// Asigna Atributos
        /// </summary>
        private void asignaAtributos()
        {
            //Asignando Atributos
            ViewState["IdBitacoraMonitoreo"] = this._id_bitacora_monitoreo;
            ViewState["IdServicio"] = this._id_servicio;
            ViewState["IdParada"] = this._id_parada;
            ViewState["IdEvento"] = this._id_evento;
            ViewState["IdMovimiento"] = this._id_movimiento;
            ViewState["IdTabla"] = this._id_tabla;
            ViewState["IdRegistro"] = this._id_registro;
        }

        /// <summary>
        /// Método encargado de cargar los catalogos
        /// </summary>
        private void cargaCatalogos()
        {
            //Cargando Catalogos 
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipo, "", 3138);
        }

        /// <summary>
        /// Inicializamos el control de Valor de Acuerdo a la tabla
        /// </summary>
        /// <param name="id_tabla"></param>
        private void inicializaValor(int id_tabla)
        {
            switch (id_tabla)
            {
                //Unidad
                case 19:
                    //Instanciamos Unidad
                    using (SAT_CL.Global.Unidad objUnidad = new SAT_CL.Global.Unidad(this._id_registro))
                    {
                        //Asignamos Valores
                        lblDescripcionValor.Text = "Unidad";
                        lblValor.Text = objUnidad.numero_unidad;
                    }
                    break;
                //Operador
                case 76:
                    //Instanciamos Operador
                    using (SAT_CL.Global.Operador objOperador = new SAT_CL.Global.Operador(this._id_registro))
                    {
                        //Asignamos Valores
                        lblDescripcionValor.Text = "Operador";
                        lblValor.Text = objOperador.nombre;
                    }
                    break;
                //Transportista
                case 25:
                    //Instanciamos Transportista
                    using (SAT_CL.Global.CompaniaEmisorReceptor objTercero = new SAT_CL.Global.CompaniaEmisorReceptor(this._id_registro))
                    {
                        //Asignamos Valores
                        lblDescripcionValor.Text = "Transportista";
                        lblValor.Text = objTercero.nombre;
                    }
                    break;
                default:
                    //Asignamos Valores
                    lblDescripcionValor.Text = "Desconocido";
                    lblValor.Text = "----";
                    break;
            }
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Inicializa Valores de los Controles
        /// </summary>
        private void inicializaValores()
        {
            //Verificamos Id Bitácora Monitoreo
            if (this._id_bitacora_monitoreo == 0)
            {
                //Cargando Catalogos de Usuario
                SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipo, "", 3138, 0);
                
                //Inicializamos Valores
                ddlTipo.SelectedValue = "1";
                ddlTipo.Enabled = true;
                //Validamos que exista el Servicio
                if (this._id_servicio > 0)
                {
                    //Instanciamos Servicio
                    using (SAT_CL.Documentacion.Servicio objServicio = new SAT_CL.Documentacion.Servicio(this._id_servicio))
                    {
                        lblServicio.Text = objServicio.no_servicio.ToString();
                    }
                }
                else
                {
                    lblServicio.Text = "Sin Asignar";
                }
                //Intanciamos Movimiento
                using (SAT_CL.Despacho.Movimiento objMovimiento = new SAT_CL.Despacho.Movimiento(this._id_movimiento))
                {
                    //Instanciamos Parad Origen
                    using (SAT_CL.Despacho.Parada objParadaOrigen = new SAT_CL.Despacho.Parada(objMovimiento.id_parada_origen), 
                                                    objParadaDestino = new SAT_CL.Despacho.Parada(objMovimiento.id_parada_destino))
                    {
                        lblMovimiento.Text = objParadaOrigen.descripcion + " - " + objParadaDestino.descripcion;
                    }
                }

                //Instanciando Parada
                using (SAT_CL.Despacho.Parada stop = new SAT_CL.Despacho.Parada(this._id_parada))
                {
                    //Validando que exista la Parada
                    if (stop.habilitar)

                        //Asignando Valor
                        lblParada.Text = stop.descripcion;
                    else
                        //Asignando Valor
                        lblParada.Text = "----";
                }

                //Instanciando Evento
                using (SAT_CL.Despacho.ParadaEvento evento = new SAT_CL.Despacho.ParadaEvento(this._id_evento))
                {
                    //Validando que exista el Evento
                    if (evento.habilitar)

                        //Mostrando descripción del Evento
                        lblEvento.Text = evento.descripcion_tipo_evento;
                    else
                        //Mostrando descripción del Evento
                        lblEvento.Text = "----";
                }

                txtFechaBitacora.Text = Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");
                inicializaValor(this._id_tabla);
                txtComentario.Text = "";
                txtUbicacion.Text = "";
                txtGeoUbicacion.Text = "";
                lblError.Text = "";

                //Deshabilitando botón eliminar
                btnEliminar.Enabled = false;
            }
            else
            {
                //Instanciamos Deposito
                using (SAT_CL.Monitoreo.BitacoraMonitoreo objBitacoraMonitoreo = new SAT_CL.Monitoreo.BitacoraMonitoreo(this._id_bitacora_monitoreo))
                {
                    //Cargando Catalogos de Usuario
                    SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipo, "", 3138);
                    
                    //Inicializamos Valores
                    ddlTipo.SelectedValue = objBitacoraMonitoreo.id_tipo_bitacora_monitoreo.ToString();
                    ddlTipo.Enabled = false;

                    //Validamos que exista el Servicio
                    if (objBitacoraMonitoreo.id_servicio > 0)
                    {
                        //Instanciamos Servicio
                        using (SAT_CL.Documentacion.Servicio objServicio = new SAT_CL.Documentacion.Servicio(objBitacoraMonitoreo.id_servicio))
                        {

                            lblServicio.Text = objServicio.no_servicio.ToString();
                        }
                    }
                    //Intanciamos Movimiento
                    using (SAT_CL.Despacho.Movimiento objMovimiento = new SAT_CL.Despacho.Movimiento(objBitacoraMonitoreo.id_movimiento))
                    {
                        //Instanciamos Parad Origen
                        using (SAT_CL.Despacho.Parada objParadaOrigen = new SAT_CL.Despacho.Parada(objMovimiento.id_parada_origen), objParadaDestino = new SAT_CL.Despacho.Parada(objMovimiento.id_parada_destino))
                        {
                            lblMovimiento.Text = objParadaOrigen.descripcion + " - " + objParadaDestino.descripcion;
                        }
                    }
                    txtFechaBitacora.Text = objBitacoraMonitoreo.fecha_bitacora.ToString("dd/MM/yyyy HH:mm");
                    this._id_registro = objBitacoraMonitoreo.id_registro;
                    inicializaValor(objBitacoraMonitoreo.id_tabla);
                    txtComentario.Text = objBitacoraMonitoreo.comentario; ;
                    txtUbicacion.Text = objBitacoraMonitoreo.nombre_ubicacion;
                    txtGeoUbicacion.Text = string.Format("{0}, {1}", objBitacoraMonitoreo.latitud, objBitacoraMonitoreo.longitud);
                    lblError.Text = "";
                }
            }

            //Invocando Método de Carga
            cargaImagenDocumentos();
        }
        /// <summary>
        /// Método encargado de Cargar las Imagenes de los Documentos
        /// </summary>
        private void cargaImagenDocumentos()
        {
            //Obteniendo Incidencias
            using (DataTable dtIncidencias = SAT_CL.Monitoreo.BitacoraMonitoreo.CargaImagenesBitacoraMonitoreo(this._id_bitacora_monitoreo))
            {
                //Validando que existen Registros
                if (Validacion.ValidaOrigenDatos(dtIncidencias))
                
                    //Cargando Lista de Incidencias
                    Controles.CargaDataList(dtlImagenDocumentos, dtIncidencias, "URL", "", "");
                else
                    //Cargando lista vacía
                    Controles.CargaDataList(dtlImagenDocumentos, null, "URL", "", "");
            }
        }

        #endregion

        #region Método Publicos

        

        /// <summary>
        /// Inicializamos Controles
        /// </summary>
        /// <param name="id_bitacora_monitoreo"></param>
        /// <param name="idServicio"></param>
        /// <param name="idParada"></param>
        /// <param name="idEvento"></param>
        /// <param name="idMovimiento"></param>
        /// <param name="idTabla"></param>
        /// <param name="idRegistro"></param>
        public void InicializaControl(int id_bitacora_monitoreo, int idServicio, int idParada, int idEvento, int idMovimiento, int idTabla, int idRegistro)
        {
            //Asignando Atributos
            this._id_bitacora_monitoreo = id_bitacora_monitoreo;
            this._id_servicio = idServicio;
            this._id_parada = idParada;
            this._id_evento = idEvento;
            this._id_movimiento = idMovimiento;
            this._id_tabla = idTabla;
            this._id_registro = idRegistro;

            //Inicializa Valores
            inicializaValores();
        }

        /// <summary>
        /// Método que permite inicializar el atributo.
        /// </summary>
        /// <param name="id_bitacora_monitoreo"></param>
        public void InicializaControl(int id_bitacora_monitoreo)
        {
            //Invoca al método inicializa valores.
            this.InicializaControl(id_bitacora_monitoreo, 0, 0, 0, 0, 0, 0);
        }

        /// <summary>
        /// Registra Bitácora Monitoreo
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion RegistraBitacoraMonitoreo()
        {
            //Declaramos Mensaje
            RetornoOperacion resultado = new RetornoOperacion();

            //Validamos Existencia de la Bitácora Monitoreo
            if (this._id_bitacora_monitoreo == 0)
            {
                //Validando Tabla y registro
                if (this._id_tabla != 0 && this._id_registro != 0)

                    //Registramos Bitácora Monitoreo
                    resultado = SAT_CL.Monitoreo.BitacoraMonitoreo.InsertaBitacoraMonitoreo(SAT_CL.Monitoreo.BitacoraMonitoreo.OrigenBitacoraMonitoreo.Escritorio,
                               Convert.ToByte(ddlTipo.SelectedValue), this._id_servicio, this._id_parada, this._id_evento, this._id_movimiento, this._id_tabla, this._id_registro, SqlGeography.Point(0, 0, 4326), txtUbicacion.Text,
                               txtComentario.Text, Convert.ToDateTime(txtFechaBitacora.Text), 0.00M, false, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                else
                    //Instanciando Excepción
                    resultado = new RetornoOperacion("");
            }
            else
            {
                //Instanciamos Bitácora Monitpreo
                using (SAT_CL.Monitoreo.BitacoraMonitoreo objBitacoraMonitoreo = new SAT_CL.Monitoreo.BitacoraMonitoreo(this._id_bitacora_monitoreo))
                {
                    //Validando Existencia
                    if (objBitacoraMonitoreo.habilitar)
                    
                    //Editamos Bitácora Monitoreo
                    resultado = objBitacoraMonitoreo.EditaBitacoraMonitoreo(Convert.ToByte(ddlTipo.SelectedValue), objBitacoraMonitoreo.id_servicio,
                                objBitacoraMonitoreo.id_parada, objBitacoraMonitoreo.id_evento, objBitacoraMonitoreo.id_movimiento, objBitacoraMonitoreo.id_tabla, objBitacoraMonitoreo.id_registro, SqlGeography.Point(0, 0, 4326), txtUbicacion.Text,
                                txtComentario.Text, Convert.ToDateTime(txtFechaBitacora.Text), objBitacoraMonitoreo.velocidad, objBitacoraMonitoreo.bit_encendido, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                    else
                        //Instanciando Excepción
                        resultado = new RetornoOperacion("");
                }
            }

            //Validamos Resultado
            if (resultado.OperacionExitosa)
            {
                //Inicializamos Control
                InicializaControl(resultado.IdRegistro, this._id_servicio, this._id_parada, this._id_evento, this._id_movimiento, this._id_tabla, this._id_registro);
            }

            //Mostramos Mensaje Error
            lblError.Text = resultado.Mensaje;
            
            //Devolvemos Valor
            return resultado;
        }

        /// <summary>
        /// Deshabilitar Bitacora Monitoreo 
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaBitacoraMonitoreo()
        {
            //Declaramos Mensaje
            RetornoOperacion resultado = new RetornoOperacion();

            //Validamos Existencia de Bitácora Monitoreo
            if (this._id_bitacora_monitoreo > 0)
            {

                //Instanciamos Bitácora Monitoreo
                using (SAT_CL.Monitoreo.BitacoraMonitoreo objBitacoraMonitoreo = new SAT_CL.Monitoreo.BitacoraMonitoreo(this._id_bitacora_monitoreo))
                {
                    //Solicitamos Depósito
                    resultado = objBitacoraMonitoreo.DeshabilitaBitacoraMonitoreo(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                }
            }
            //Mostramos Mensaje Error
            lblError.Text = resultado.Mensaje;

            //Devolvemos Valor
            return resultado;
        }
        /// <summary>
        /// Método encargado de Eliminar el Zoom
        /// </summary>
        public void RemueveImagenElevatedZoom()
        {
            //Declarando Script de Configuración
            string script = @"<script type='text/javascript'>
                                $('.myImageElevate').remove();
                              </script>";

            //Registrando Script
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "DestruyeElevatedZoom", script, false);
        }

        #endregion        
    }
}