using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.Base;
using SAT_CL.Mantenimiento;
using System.Data;
using TSDK.Datos;

namespace SAT.UserControls
{
    public partial class wucAsignacionActividad : System.Web.UI.UserControl
    {
        #region Atributos

        private int _id_asignacion_actividad;
        private int _id_orden_actividad;
        private int _id_actividad;

        /// <summary>
        /// Propiedad encargada de Obtener o Establecer el Orden de Tabulación de los Controles
        /// </summary>
        public short TabIndex
        {
            set
            {
                //Asignando Tabulación
                ddlTipo.TabIndex =
                //ddlEstatus.TabIndex =
                txtEmpleado.TabIndex =
                txtProveedor.TabIndex =
                txtInicioAsignacion.TabIndex =
                txtFinAsignacion.TabIndex = 
                btnGuardar.TabIndex =
                btnIniciar.TabIndex =
                btnTerminar.TabIndex = value;
            }
            get { return ddlTipo.TabIndex; }
        }
        /// <summary>
        /// Propiedad encargada de Obtener o establecer el Valor de habilitación de los controles contenidos
        /// </summary>
        public bool Enabled
        {
            set
            {
                //Asignando Habilitación
                ddlTipo.Enabled =
                //ddlEstatus.Enabled =
                txtEmpleado.Enabled =
                txtProveedor.Enabled =
                txtInicioAsignacion.Enabled =
                txtFinAsignacion.Enabled =
                btnGuardar.Enabled =
                btnIniciar.Enabled =
                btnTerminar.Enabled = value;
            }
            get { return ddlTipo.Enabled; }
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            //COnfigurando Controles
            configuraTipo();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            //Validando que exista un Evento
            if (ClickGuardarAsignacion != null)
                
                //Iniciando Manejador
                OnClickGuardarAsignacion(e);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            //Validando que exista un Evento
            if (ClickEliminarAsignacion != null)

                //Iniciando Manejador
                OnClickEliminarAsignacion(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnIniciar_Click(object sender, EventArgs e)
        {
            //Validando que exista un Evento
            if (ClickIniciarAsignacion != null)

                //Iniciando Manejador
                OnClickIniciarAsignacion(e);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnTerminar_Click(object sender, EventArgs e)
        {
            //Validando que exista un Evento
            if (ClickTerminarAsignacion != null)

                //Iniciando Manejador
                OnClickTerminarAsignacion(e);
        }

        #region Eventos (Manejadores)

        /// <summary>
        /// Manejador de Evento "Guardar"
        /// </summary>
        public event EventHandler ClickGuardarAsignacion;
        /// <summary>
        /// Evento que Manipula el Manejador "Guardar"
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnClickGuardarAsignacion(EventArgs e)
        {   
            //Validando que exista el Evento
            if (ClickGuardarAsignacion != null)
                
                //Iniciando Evento
                ClickGuardarAsignacion(this, e);
        }
        /// <summary>
        /// Manejador de Evento "Eliminar"
        /// </summary>
        public event EventHandler ClickEliminarAsignacion;
        /// <summary>
        /// Evento que Manipula el Manejador "Eliminar"
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnClickEliminarAsignacion(EventArgs e)
        {
            //Validando que exista el Evento
            if (ClickEliminarAsignacion != null)

                //Iniciando Evento
                ClickEliminarAsignacion(this, e);
        }
        /// <summary>
        /// Manejador de Evento "Iniciar"
        /// </summary>
        public event EventHandler ClickIniciarAsignacion;
        /// <summary>
        /// Evento que Manipula el Manejador "Iniciar"
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnClickIniciarAsignacion(EventArgs e)
        {
            //Validando que exista el Evento
            if (ClickIniciarAsignacion != null)

                //Iniciando Evento
                ClickIniciarAsignacion(this, e);
        }
        /// <summary>
        /// Manejador de Evento "Terminar"
        /// </summary>
        public event EventHandler ClickTerminarAsignacion;
        /// <summary>
        /// Evento que Manipula el Manejador "Terminar"
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnClickTerminarAsignacion(EventArgs e)
        {
            //Validando que exista el Evento
            if (ClickTerminarAsignacion != null)

                //Iniciando Evento
                ClickTerminarAsignacion(this, e);
        }

        /// <summary>
        /// Evento generado al cambiar el Puesto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlPuesto_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cargamos Autocomplete
            cargaControlAutocompletado(ddlPuesto, ddlPuesto.GetType());
        }
        #endregion

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método encargado de Inicializar el Control de Usuario
        /// </summary>
        private void inicializaControlUsuario(byte id_puesto)
        {
            //Invocando Método
            cargaCatalogos();
            
           
            //Instanciando Asignación
            using(SAT_CL.Mantenimiento.ActividadAsignacion aa = new SAT_CL.Mantenimiento.ActividadAsignacion(this._id_asignacion_actividad))
            {
                //Validando que Exista la Asignación
                if(aa.id_asignacion_actividad > 0)
                {
                    //Asignando Valores
                    lblNoAsignacion.Text = aa.id_asignacion_actividad.ToString();
                    ddlTipo.SelectedValue = aa.id_tipo.ToString();
                    configuraTipo();
                    ddlEstatus.SelectedValue = aa.id_estatus.ToString();
                    txtInicioAsignacion.Text = aa.inicio_asignacion == DateTime.MinValue? "" : aa.inicio_asignacion.ToString("dd/MM/yyyy HH:mm");
                    txtFinAsignacion.Text = aa.fin_asignacion == DateTime.MinValue ? "" : aa.fin_asignacion.ToString("dd/MM/yyyy HH:mm");
                    if (aa.id_proveedor > 0)
                    {
                        //Instanciando Proveedor
                        using (SAT_CL.Global.CompaniaEmisorReceptor pro = new SAT_CL.Global.CompaniaEmisorReceptor(aa.id_proveedor))
                        {
                            //Validando que Exista el Proveedor
                            if (pro.id_compania_emisor_receptor > 0)

                                //Asignando Valor
                                txtProveedor.Text = pro.nombre + " ID:" + pro.id_compania_emisor_receptor.ToString();
                            else
                                //Limpiando Campo
                                txtProveedor.Text = "";
                        }
                    }
                    else if (aa.id_empleado > 0)
                    {
                        using (SAT_CL.Global.Operador op = new SAT_CL.Global.Operador(aa.id_empleado))
                        {
                            //Asignamos Valor de Puesto al Empleado
                            ddlPuesto.SelectedValue =  SAT_CL.Global.Catalogo.RegresaDescripcionValor(3158, SAT_CL.Global.Referencia.CargaReferencia("0", 76, op.id_operador, "General", "Tipo Mécanico"));
                            //Cargando Empleado
                            txtEmpleado.Text = op.nombre + " ID:" + aa.id_empleado.ToString();
                        }
                    }


                    if (aa.Estatus == ActividadAsignacion.EstatusAsignacionActividad.Iniciada)
                    {
                        //Cambiando Configuración
                        btnIniciar.CommandName =
                        btnIniciar.Text = "Pausar";

                    }
                    else
                    {
                        //Cambiando Configuración
                        btnIniciar.CommandName =
                        btnIniciar.Text = "Iniciar";

                    }
                    

                    //Cambiando Habilitación
                    btnIniciar.Enabled = 
                    btnTerminar.Enabled = true;
                }
                else
                {
                    //Limpiando Controles
                    lblNoAsignacion.Text = "Por Asignar";
                    txtEmpleado.Text =
                    txtProveedor.Text = "";
                    txtInicioAsignacion.Text = "";
                    txtFinAsignacion.Text = "";
                    ddlPuesto.SelectedValue = id_puesto.ToString();

                    //Cambiando Configuración
                    btnIniciar.CommandName =
                    btnIniciar.Text = "Pausar";
                    //Cambiando Habilitación
                    btnIniciar.Enabled =
                    btnTerminar.Enabled = true;
                    //Configurando Tipo
                    configuraTipo();
                }
            }
            //Método encargado de Cargar  el Autocomplete de Empleado de Acuerdo al Tipo de Mécanico
            cargaControlAutocompletado(this, this.GetType());
        }
        /// <summary>
        /// Método Privado encargado de Asignar los Atributos de la Forma
        /// </summary>
        private void asignaAtributos()
        {   
            //Asignando Atributos
            ViewState["idAsignacionActividad"] = this._id_asignacion_actividad;
            ViewState["idOrden"] = this._id_orden_actividad;
            ViewState["idActividad"] = this._id_actividad;
        }
        /// <summary>
        /// Método Privado encargado de Recuperar el Valor de los Atributos de la Forma
        /// </summary>
        private void recuperaAtributos()
        {   
            //Recuperando Atributos
            if (Convert.ToInt32(ViewState["idAsignacionActividad"]) != 0)
                this._id_asignacion_actividad = Convert.ToInt32(ViewState["idAsignacionActividad"]);
            if (Convert.ToInt32(ViewState["idOrden"]) != 0)
                this._id_orden_actividad = Convert.ToInt32(ViewState["idOrden"]);
            if (Convert.ToInt32(ViewState["idActividad"]) != 0)
                this._id_actividad= Convert.ToInt32(ViewState["idActividad"]);
        }
        /// <summary>
        /// Metodo encargado de Cargar los Catalogos
        /// </summary>
        private void cargaCatalogos()
        {
            //Cargando Catalogos
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlEstatus, "", 1126);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipo, "", 1125);
            //Carga los valores al dropdownlist puesto
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlPuesto, "", 3158);
        }
        /// <summary>
        /// Método encargado de Configurar los Controles según su Tipo (Interno/Externo)
        /// </summary>
        private void configuraTipo()
        {
            //Validando Tipo
            switch (ddlTipo.SelectedValue)
            {
                case "1":
                    {
                        //Configurando Controles
                        txtEmpleado.Text = "";
                        txtEmpleado.Enabled = true;
                        txtProveedor.Text = "";
                        txtProveedor.Enabled = false;
                        break;
                    }
                case "2":
                    {
                        //Configurando Control
                        txtEmpleado.Text = "";
                        txtEmpleado.Enabled = false;
                        txtProveedor.Text = "";
                        txtProveedor.Enabled = true;
                        break;
                    }
            }
        }

    // <summary>
    /// Metodo encargado de Cargar el Catalogo de Autocompletado
    /// </summary>
    private void cargaControlAutocompletado(Control control, Type tipo)
    { 
        //Declaramos variable tipo de autocomplete
        string id = "46";
         
        //Generamos Sript
        string script =
        @"<script type='text/javascript'>
          $('#" + this.txtEmpleado.ClientID + @"').autocomplete({ source:'../WebHandlers/AutoCompleta.ashx?id=" + id + @"&param=" + ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor + @"&param2=" + ddlPuesto.SelectedValue + @"',
         appendTo: '" + this.Contenedor + @"'});
          </script>";

        //Registrando el script sólo para los paneles que producirán actualización del mismo
        System.Web.UI.ScriptManager.RegisterStartupScript(control, tipo, "AutocompleteRecursos", script, false);
    }
        #endregion

        #region Métodos Públicos

        /// <summary>
       /// Método encargado de Inicializar el Control de Usuario
        /// </summary>
        /// <param name="id_asignacion_actividad"></param>
        /// <param name="id_orden_actividad"></param>
        /// <param name="id_actividad"></param>
        /// <param name="id_puesto"></param>
        public void InicializaAsignacionActividad(int id_asignacion_actividad, int id_orden_actividad, int id_actividad, byte id_puesto)
        {
            //Asignando Atributos
            this._id_asignacion_actividad = id_asignacion_actividad;
            this._id_orden_actividad = id_orden_actividad;
            this._id_actividad = id_actividad;
            //Inicializando Control
            inicializaControlUsuario(id_puesto);
        }
        /// <summary>
        /// Método encargado de Guardar la Asignación de la Actividad
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion GuardaAsignacionActividad()
        {
            //Declarando Objeto de retorno
            RetornoOperacion result = new RetornoOperacion();

            //Obteniendo Fechas
            DateTime fecha_inicio = DateTime.MinValue, fecha_fin  = DateTime.MinValue;
            DateTime.TryParse(txtInicioAsignacion.Text, out fecha_inicio);
            DateTime.TryParse(txtFinAsignacion.Text, out fecha_fin);

            //Instanciando Asignación
            using (ActividadAsignacion aa = new ActividadAsignacion(this._id_asignacion_actividad))
            {
                //Validando que Exista el Registro
                if (aa.id_asignacion_actividad > 0)
                {
                    //Editando Asignación
                    result = aa.EditaRegistroActividadAsignacion(this._id_orden_actividad, (ActividadAsignacion.TipoAsignacionActividad)Convert.ToInt32(ddlTipo.SelectedValue),
                                (ActividadAsignacion.EstatusAsignacionActividad)Convert.ToInt32(ddlEstatus.SelectedValue), Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtEmpleado.Text, "ID:", 1, "0")),
                                Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtProveedor.Text, "ID:", 1, "0")), fecha_inicio, fecha_fin,
                                ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                }
                else
                {
                    //Insertando Asignación
                    result = ActividadAsignacion.InsertaActividadAsignacion(this._id_orden_actividad, (ActividadAsignacion.TipoAsignacionActividad)Convert.ToInt32(ddlTipo.SelectedValue),
                                ActividadAsignacion.EstatusAsignacionActividad.Asignada, Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtEmpleado.Text, "ID:", 1, "0")),
                                Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtProveedor.Text, "ID:", 1,"0")), DateTime.MinValue, DateTime.MinValue,
                                ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                }
            }

            //Validando que la Operación fuese Exitosa
            if (result.OperacionExitosa)
            
                //Asignando Variables
                this._id_asignacion_actividad = result.IdRegistro;

            //Inicializando Control
            inicializaControlUsuario(1);

            //Devolviendo Resultado Obtenido
            return result;
        }

        /// <summary>
        /// Método encargado de Guardar la Asignación de la Actividad
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion IniciarAsignacionActividad(DateTime fecha)
        {
            //Declarando Objeto de retorno
            RetornoOperacion result = new RetornoOperacion();

            //Instanciando Asignación
            using (ActividadAsignacion aa = new ActividadAsignacion(this._id_asignacion_actividad))
            {
               
                    //Editando Asignación
                    result = aa.IniciaAsignacion(fecha,((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
            }

            //Validando que la Operación fuese Exitosa
            if (result.OperacionExitosa)

                //Asignando Variables
                this._id_asignacion_actividad = result.IdRegistro;

            //Inicializando Control
            inicializaControlUsuario(1);

            //Asignando Mensaje de Operación
            TSDK.ASP.ScriptServer.MuestraNotificacion(this, result, TSDK.ASP.ScriptServer.PosicionNotificacion.AbajoDerecha);

            //Devolviendo Resultado Obtenido
            return result;
        }

        /// <summary>
        /// Método encargado de Pausar la Asignación
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion PausarAsignacionActividad(DateTime fecha)
        {
            //Declarando Objeto de retorno
            RetornoOperacion result = new RetornoOperacion();

            //Instanciando Asignación
            using (ActividadAsignacion aa = new ActividadAsignacion(this._id_asignacion_actividad))
            {

                //Editando Asignación
                result = aa.PausaAsignacion(fecha, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
            }

            //Validando que la Operación fuese Exitosa
            if (result.OperacionExitosa)

                //Asignando Variables
                this._id_asignacion_actividad = result.IdRegistro;

            //Inicializando Control
            inicializaControlUsuario(1);

            //Asignando Mensaje de Operación
            TSDK.ASP.ScriptServer.MuestraNotificacion(this, result, TSDK.ASP.ScriptServer.PosicionNotificacion.AbajoDerecha);
            //Devolviendo Resultado Obtenido
            return result;
        }

        /// <summary>
        /// Método encargado de Eliminar la Asignación de la Actividad
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion EliminaAsignacionActividad()
        {
            //Declarando Objeto de retorno
            RetornoOperacion result = new RetornoOperacion();

            //Instanciando Asignación
            using (ActividadAsignacion aa = new ActividadAsignacion(this._id_asignacion_actividad))
            {
                //Validando que Exista el Registro
                if (aa.id_asignacion_actividad > 0)

                    //Deshabilitando Actividad
                    result = aa.DeshabilitaActividadAsignacion(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                else
                    //Instanciando Excepción
                    result = new RetornoOperacion("No se encontro la Asignación deseada");
            }

            //Validando que la Operación fuese Exitosa
            if (result.OperacionExitosa)

                //Asignando Variables
                this._id_asignacion_actividad = 0;

            //Inicializando Control
            inicializaControlUsuario(1);

            //Asignando Mensaje de Operación
            TSDK.ASP.ScriptServer.MuestraNotificacion(this, result, TSDK.ASP.ScriptServer.PosicionNotificacion.AbajoDerecha);

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Iniciar la Asignación
        /// </summary>
        /// <param name="fecha">Fecha Inicio/Pausa</param>
        /// <returns></returns>
        public RetornoOperacion IniciarAsignacion(DateTime fecha)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Validando Estatus de la Asignación
            switch(btnIniciar.CommandName)
            {
                case "Iniciar":
                    {
                        //Iniciamos Asignación
                       result = IniciarAsignacionActividad(fecha);
                        break;
                    }
                case "Pausar":
                    {
                       result = PausarAsignacionActividad(fecha);
                        break;
                    }
            }

            //Devolviendo Resultado Obtenido
            return result;
        }
        

        /// <summary>
        ///  Método encargado de Terminar la Asignación
        /// </summary>
        /// <param name="fecha">fecha</param>
        /// <returns></returns>
        public RetornoOperacion TerminarAsignacion(DateTime fecha)
        {
            //Declarando Objeto de retorno
            RetornoOperacion result = new RetornoOperacion();

            //Instanciando Asignación
            using (ActividadAsignacion aa = new ActividadAsignacion(this._id_asignacion_actividad))
            {

                //Editando Asignación
                result = aa.TerminaAsignacion(fecha, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
            }

            //Validando que la Operación fuese Exitosa
            if (result.OperacionExitosa)

                //Asignando Variables
                this._id_asignacion_actividad = result.IdRegistro;

            //Inicializando Control
            inicializaControlUsuario(1);

            //Asignando Mensaje de Operación
            TSDK.ASP.ScriptServer.MuestraNotificacion(this, result, TSDK.ASP.ScriptServer.PosicionNotificacion.AbajoDerecha);

            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion

        
    }
}