using SAT_CL;
using SAT_CL.Despacho;
using SAT_CL.Global;
using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;
using System.Configuration;
namespace SAT.UserControls
{
    public partial class wucAsignacionRecurso : System.Web.UI.UserControl
    {
        #region Atributos

        private int _id_movimiento;
        private int _id_origen;
        private int _id_ubicacion;
        private int _id_compania;
        private int _id_recurso;
        private byte _id_tipo_asignacion;
        private DataSet _ds;
        /// <summary>
        /// Atributo encargado de Almacenar el Recurso Seleccionado
        /// </summary>
        public int idRecurso { get { return this._id_recurso; } }
        /// <summary>
        /// Atributo encargado de Almacenar el Tipo de Asignación
        /// </summary>
        public byte idTipoAsignacion { get { return this._id_tipo_asignacion; } }

        /// <summary>
        /// Almacena el Nombre del Contenedor del control de usuario
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
            {
                //Asignando Atributos
                asignaAtributos();

                //Cargando Catalogos
                cargaCatalogos();
            }
            else
                //Recuperando Atributos
                recuperaAtributos();

            //Invocando Método de Carga
            //cargaControlAutocompletado();

            //Validando que Existen Registros
            if (TSDK.Datos.Validacion.ValidaOrigenDatos(this._ds, "Table"))

                //Cargando GridView
                TSDK.ASP.Controles.CargaGridView(gvRecursosDisponibles, this._ds.Tables["Table"], "Id", "", false, 0);
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

        #region Eventos (Manejadores)

        /// <summary>
        /// Manejador de Evento "Agregar"
        /// </summary>
        public event EventHandler ClickAgregarRecurso;
        /// <summary>
        /// Manejador de Evento "Liberar Recurso"
        /// </summary>
        public event EventHandler ClickLiberarRecurso;
        /// <summary>
        /// Manejador de Evento "Quitar"
        /// </summary>
        public event EventHandler ClickQuitarRecurso;
        /// <summary>
        /// Manejador de Evento "Reubicar"
        /// </summary>
        public event EventHandler ClickReubicarRecurso;
        /// <summary>
        /// Evento que Manipula el Manejador "Liberear Recurso"
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnClickLiberarRecurso(EventArgs e)
        {
            //Validando que exista el Evento
            if (ClickLiberarRecurso != null)

                //Iniciando Evento
                ClickLiberarRecurso(this, e);
        }

        /// <summary>
        /// Evento que Manipula el Manejador "Agregar"
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnClickAgregarRecurso(EventArgs e)
        {
            //Validando que exista el Evento
            if (ClickAgregarRecurso != null)

                //Iniciando Evento
                ClickAgregarRecurso(this, e);
        }
        /// <summary>
        /// Evento que Manipula el Manejador "Quitar"
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnClickQuitarRecurso(EventArgs e)
        {
            //Validando que exista el Evento
            if (ClickQuitarRecurso != null)

                //Iniciando Evento
                ClickQuitarRecurso(this, e);
        }
        /// <summary>
        /// Evento que Manipula el Manejador "Reubicar"
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnClickReubicarRecurso(EventArgs e)
        {
            //Validando que exista el Evento
            if (ClickReubicarRecurso != null)

                //Iniciando Evento
                ClickReubicarRecurso(this, e);
        }

        #endregion

        /// <summary>
        /// Evento producido al cambiar el tipo de asignación
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTipoAsignacion_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            //Invocando Método de Carga
            //cargaControlAutocompletado();

            //Inicialiamos Grid View
            Controles.InicializaGridview(gvRecursosDisponibles);
        }
        /// <summary>
        /// Evento producido al buscar los Recursos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscarRecursos_OnClick(object sender, EventArgs e)
        {
            //Limpiando control de ordenamiento
            lblOrdenarRecursosDisponibles.Text = "";
            uplblOrdenarRecursosDisponibles.Update();

            //Validamos Tipo de Asignación para mostrar Recursos
            //Unidad
            if (ddlTipoAsignacion.SelectedValue == "1")

                //Cargamos Unidades
                cargaUnidades();
            else
            {
                //Operador
                if (ddlTipoAsignacion.SelectedValue == "2")

                    //Cargamos Operadores
                    cargaOperadores();
                else
                    //Cargamos Terceros
                    cargaTerceros();
            }

            //Inicializamos Incides
            Controles.InicializaIndices(gvRecursosAsignados);
            Controles.InicializaIndices(gvRecursosDisponibles);

            //Limpiamos Error
            lblErrorRecursosAsignados.Text = "";
        }
        /// <summary>
        /// Evento Producido al Marcar o Desmarcar 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkUbicacionActual_CheckedChanged(object sender, EventArgs e)
        {
            //Limpiando control de ordenamiento
            lblOrdenarRecursosDisponibles.Text = "";
            uplblOrdenarRecursosDisponibles.Update();

            //Validamos Tipo de Asignación para mostrar Recursos
            //Unidad
            if (ddlTipoAsignacion.SelectedValue == "1")

                //Cargamos Unidades
                cargaUnidades();
            else
            {
                //Operador
                if (ddlTipoAsignacion.SelectedValue == "2")

                    //Cargamos Operadores
                    cargaOperadores();
                else
                    //Cargamos Terceros
                    cargaTerceros();
            }

            //Inicializamos Incides
            Controles.InicializaIndices(gvRecursosAsignados);
            Controles.InicializaIndices(gvRecursosDisponibles);

            //Limpiamos Error
            lblErrorRecursosAsignados.Text = "";
        }
        /// <summary>
        /// Evento producido al Cancelar la Eliminación del Recurso
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarEliminacionRecurso_OnClick(object sender, EventArgs e)
        {
            //Mostramos Ventana Modal
            ScriptServer.AlternarVentana(upbtnCancelarEliminacionRecurso, upbtnCancelarEliminacionRecurso.GetType(), "CerrarVentanaModal", "contenidoConfirmacionQuitarRecursos", "confirmacionQuitarRecursos");
        }
        /// <summary>
        /// Evento producido al Eliminar un Recurso
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarEliminacionRecurso_OnClick(object sender, EventArgs e)
        {
            //Declaramos Variable Auxiliares
            MovimientoAsignacionRecurso.Tipo tipo = MovimientoAsignacionRecurso.Tipo.Operador;

            //Obtenemos el Tipo de Unidad
            obtieneTipoRecursoAsignado(out tipo);

            //Asignando Recurso
            this._id_recurso = Convert.ToInt32(gvRecursosAsignados.SelectedValue);
            this._id_tipo_asignacion = Convert.ToByte(tipo);

            //Validando que exista un Evento
            if (ClickQuitarRecurso != null)
                //Iniciando Manejador
                OnClickQuitarRecurso(e);

            //Cerramos Ventana Modal
            ScriptServer.AlternarVentana(upbtnAceptarEliminacionRecurso, upbtnAceptarEliminacionRecurso.GetType(), "CerrarVentanaModal", "contenidoConfirmacionQuitarRecursos", "confirmacionQuitarRecursos");
        }
        /// <summary>
        /// Evento producido al Cancelar la Asignación de Recurso
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarAsignadoAlrecurso_OnClick(object sender, EventArgs e)
        {
            //Mostramos Ventana Modal
            ScriptServer.AlternarVentana(upbtnCancelarAsignadoAlrecurso, upbtnCancelarAsignadoAlrecurso.GetType(), "CierreVentanaModal", "contenidoConfirmacionAsignadoAlRecurso", "confirmacionAsignadoAlRecurso");
        }
        /// <summary>
        /// Evento Produciado Al Aceptar asignación de recursos vinculados a recurso por agregar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarAsignadoAlrecurso_OnClick(object sender, EventArgs e)
        {
            //Declaramos Objeto de Retorno
            RetornoOperacion resultado = new RetornoOperacion();

            //Declaramos Variable Auxiliares
            MovimientoAsignacionRecurso.Tipo tipo = MovimientoAsignacionRecurso.Tipo.Operador;
            int tipo_unidad = 0;

            //Obtenemos el Tipo de Unidad
            obtieneTipoRecurso(out tipo, out tipo_unidad);

            //Asignando Recurso
            this._id_recurso = Convert.ToInt32(gvRecursosDisponibles.SelectedValue);
            this._id_tipo_asignacion = Convert.ToByte(tipo);

            //Validamos Asignación Estancias del Recurso
            resultado = MovimientoAsignacionRecurso.ValidaEstanciaAsignacionRecurso(tipo, Convert.ToInt32(gvRecursosDisponibles.SelectedValue));

            //Validamos resultado
            if (resultado.OperacionExitosa)
            {
                //Ocultando confirmación para agregar recurso vinculado en Ventana Modal
                ScriptServer.AlternarVentana(upbtnAceptarAsignadoAlrecurso, upbtnAceptarAsignadoAlrecurso.GetType(), "CierreVentanaModal", "contenidoConfirmacionAsignadoAlRecurso", "confirmacionAsignadoAlRecurso");

                //Validando que exista un Evento
                if (ClickAgregarRecurso != null)
                    //Iniciando Manejador
                    OnClickAgregarRecurso(e);
            }
        }

        #region Eventos GridView "Recursos Asignados"

        /// <summary>
        /// Evento Prodicido al Cambiar el Indice de página del Gridview "RecursosAsignados"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvRecursosAsignados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Validando que Existan Registros
            if (gvRecursosAsignados.DataKeys.Count > 0)
            {
                //Cambiando el indice del GridView
                Controles.CambiaIndicePaginaGridView(gvRecursosAsignados, OrigenDatos.RecuperaDataTableDataSet(this._ds, "Table1"), e.NewPageIndex, false, 1);
            }
        }
        /// Evento Producido al pulsar el botón "Exportar RecursosAsignados"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarRecursosAsignados_OnClick(object sender, EventArgs e)
        {
            //Validando que Existan Registros
            if (gvRecursosAsignados.DataKeys.Count > 0)
            {
                //Exportando de Servicios
                Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet(this._ds, "Table1"));
            }
        }
        /// <summary>
        /// Evento producido al pulsar el link Recursos Asignados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbRecursosAsignados_Click(object sender, EventArgs e)
        {
            //Recuperando el botón pulsado
            LinkButton b = (LinkButton)sender;

            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Declaramos variables
            int id_unidad = 0; int id_operador = 0; int id_tercero = 0;

            //Si existen registros
            if (gvRecursosAsignados.DataKeys.Count > 0)
            {
                //Seleccionando la fila actual
                Controles.SeleccionaFila(gvRecursosAsignados, sender, "lnk", false);

                //En base al comando definido para el botón
                switch (b.CommandName)
                {
                    case "Quitar":
                        {
                            //Asignando Recurso
                            this._id_recurso = Convert.ToInt32(gvRecursosAsignados.SelectedValue);
                            this._id_tipo_asignacion = Convert.ToByte(ddlTipoAsignacion.SelectedValue);

                            //Instanciamos Asignación
                            using (MovimientoAsignacionRecurso objMovimientoAsignacionRecurso = new MovimientoAsignacionRecurso
                                                                                              (Convert.ToInt32(gvRecursosAsignados.SelectedDataKey.Value)))
                            {
                                //Validamos Deshabilitación de Recursos
                                resultado = objMovimientoAsignacionRecurso.ValidaDeshabilitacionRecursos();
                            }
                            //Si existe asignación ligada
                            if (!resultado.OperacionExitosa)
                            {

                                //Asignamos Mensaje
                                lblMensaje.Text = resultado.Mensaje;

                                //Mostramos Ventana Modal
                                ScriptServer.AlternarVentana(upgvRecursosAsignados, upgvRecursosAsignados.GetType(), "AbreVentanaModal", "contenidoConfirmacionQuitarRecursos", "confirmacionQuitarRecursos");
                            }
                            else
                            {
                                //Validando que exista un Evento
                                if (ClickQuitarRecurso != null)

                                    //Iniciando Manejador
                                    OnClickQuitarRecurso(e);              
                            }
                            break;
                        }
                    case "Anticipos":
                        {
                            //Mostramos Vista
                            mtvAnticipos.ActiveViewIndex = 2;

                            //Validamos si el Tipo de Asignación actual es  el correcto y obtenemos asignaciòn
                            resultado = SAT_CL.Despacho.MovimientoAsignacionRecurso.ObtieneTipoAsignacion(out id_unidad, out id_operador, out id_tercero, Convert.ToInt32(gvRecursosAsignados.SelectedDataKey["Id"]),
                                                   Convert.ToInt32(gvRecursosAsignados.SelectedDataKey["IdRecurso"]));

                            //Validamos si el Tipo de Asignación actual es  el correcto
                            if (resultado.OperacionExitosa)
                            {
                                //Inicializando estatus de filtrado de anticipos (otro proveedor)
                                inicializaAsignacionOtroProveedor(id_operador, id_unidad, id_tercero);

                                //Craga Anticipos
                                cargaAnticiposEntidad(id_operador, id_unidad, id_tercero);

                                //Cambiando Vista
                                mvwPrincipal.ActiveViewIndex = 1;
                            }
                            else
                                //Establecemos Mensaje
                                lblMensajeAsignadoAlRecurso.Text = resultado.Mensaje;

                            break;
                        }
                }
            }
        }

        #endregion

        #region Eventos GridView "Recursos Disponibles"

        /// <summary>
        /// Evento Producido  al enlazar los datos del Grid View Recursos Disponibles
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvRecursosDisponibles_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Si el Tipo de Asignación Actual es Unidad
            if (ddlTipoAsignacion.SelectedValue == "1")
            {
                //Evaluamos Tipo de búsqueda de la Ubicación para Insercción de  la celda Reubicación
                if (chkUbicacionActual.Checked)

                    //Insertamos Celda de Agregar
                    insertaCeldaAgregar(e.Row);
                else
                    //Insertamos Celda de Reubicar
                    insertaCeldaReubicacion(e.Row);
            }
            else
                //Insertamos Celda de Agregar
                insertaCeldaAgregar(e.Row);

            //Si el Tipo de Asignación no es Tercero
            if (ddlTipoAsignacion.SelectedValue != "3")
            {
                //Insertamos Columna Para Liberar Recurso
                insertaCeldaLiberar(e.Row);
            }

        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice del DropDownList "Tamaño RecursosDisponibles"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoRecursosDisponibles_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            //Validando que Existan Registros
            if (gvRecursosDisponibles.DataKeys.Count > 0)
            {
                //Asignando Expresión de Ordenamiento
                OrigenDatos.RecuperaDataTableDataSet(this._ds, "Table").DefaultView.Sort = lblOrdenarRecursosDisponibles.Text;

                //Cambia Tamaño del GridView
                Controles.CambiaTamañoPaginaGridView(gvRecursosDisponibles, OrigenDatos.RecuperaDataTableDataSet(this._ds, "Table"),
                                            Convert.ToInt32(ddlTamanoRecursosDisponibles.SelectedValue), false, 1);
            }
        }
        /// <summary>
        /// Evento Producido al pulsar el botón "Exportar RecursosDisponibles"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarRecursosDisponibles_OnClick(object sender, EventArgs e)
        {
            //Validando que Existan Registros
            if (gvRecursosDisponibles.DataKeys.Count > 0)
            {
                //Exportando de Servicios
                Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet(this._ds, "Table"));
            }
        }
        /// <summary>
        /// Evento Producido al Cambiar el orden del Gridview "Recursos Disponibles"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvRecursosDisponibles_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Validando que Existan Registros
            if (gvRecursosDisponibles.DataKeys.Count > 0)
            {
                //Asignando Expresión de Ordenamiento
                OrigenDatos.RecuperaDataTableDataSet(this._ds, "Table").DefaultView.Sort = lblOrdenarRecursosDisponibles.Text;

                //Ordenando el GridView
                lblOrdenarRecursosDisponibles.Text = Controles.CambiaSortExpressionGridView(gvRecursosDisponibles, OrigenDatos.RecuperaDataTableDataSet(this._ds, "Table"), e.SortExpression, false, 1);
            }
        }
        /// <summary>
        /// Evento Prodicido al Cambiar el Indice de página del Gridview "RecursosDisponibles"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvRecursosDisponibles_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Validando que Existan Registros
            if (gvRecursosDisponibles.DataKeys.Count > 0)
            {
                //Asignando Expresión de Ordenamiento
                OrigenDatos.RecuperaDataTableDataSet(this._ds, "Table").DefaultView.Sort = lblOrdenarRecursosDisponibles.Text;

                //Cambiando el indice del GridView
                Controles.CambiaIndicePaginaGridView(gvRecursosDisponibles, OrigenDatos.RecuperaDataTableDataSet(this._ds, "Table"), e.NewPageIndex, false, 1);
            }
        }
        /// <summary>
        /// Evento producido al pulsar el link Agregar Recurso en el grid de recurso disponible
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbAgregarRecurso_Click(object sender, EventArgs e)
        {
            //Validamos Seleccion el Servicio
            if (gvRecursosDisponibles.DataKeys.Count > 0)
            {
                //Declaramos Objeto Resultado
                RetornoOperacion resultado = new RetornoOperacion();

                //Seleccionando la fila actual
                Controles.SeleccionaFila(gvRecursosDisponibles, sender, "lnk", false);

                //Declaramos variables para almacenar el tipo de recurso
                MovimientoAsignacionRecurso.Tipo tipo;
                int tipo_unidad;

                //Obtenemos Tipo de recurso  para su asignación
                obtieneTipoRecurso(out tipo, out tipo_unidad);

                //Asignando Recurso
                this._id_recurso = Convert.ToInt32(gvRecursosDisponibles.SelectedValue);
                this._id_tipo_asignacion = Convert.ToByte(tipo);

                //Validamos Asignación ligadas al Recurso 
                resultado = MovimientoAsignacionRecurso.ValidaAsignacionesLigadasAlRecurso(tipo, Convert.ToInt32(gvRecursosDisponibles.SelectedValue));

                //Si existe asignación ligada
                if (!resultado.OperacionExitosa)
                {
                    //Asignamos Mensaje
                    lblMensajeAsignadoAlRecurso.Text = resultado.Mensaje;

                    //Mostramos Ventana Modal
                    ScriptServer.AlternarVentana(upgvRecursosAsignados, upgvRecursosAsignados.GetType(), "AbreVentanaModal", "contenidoConfirmacionAsignadoAlRecurso", "confirmacionAsignadoAlRecurso");
                }
                else
                {
                    //Validamos Asignación Estancias del Recurso
                    resultado = MovimientoAsignacionRecurso.ValidaEstanciaAsignacionRecurso(tipo, Convert.ToInt32(gvRecursosDisponibles.SelectedValue));

                    //Validamos reusultado
                    if (resultado.OperacionExitosa)
                    {

                        //Validando que exista un Evento
                        if (ClickAgregarRecurso != null)

                            //Iniciando Manejador
                            OnClickAgregarRecurso(e);
                    }
                }
            }
        }

        /// <summary>
        /// Evento generado de Cancelar la Liberación de las Unidades ou Operadores
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarLiberacionRecurso_Click(object sender, EventArgs e)
        {
            //Cerramos Ventana Modal
            ScriptServer.AlternarVentana(upbtnCancelarLiberacionRecurso, upbtnCancelarLiberacionRecurso.GetType(), "CerrarLiberacionUnidad", "contenidoConfirmacionLiberacionRecurso", "confirmacionLiberacionRecurso");

        }

        /// <summary>
        /// Evento producido al pulsar el link Agregar Recurso en el grid de recurso disponible
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbLiberarRecurso_Click(object sender, EventArgs e)
        {
            //Validamos Seleccion el Servicio
            if (gvRecursosDisponibles.DataKeys.Count > 0)
            {
                //Declaramos Objeto Resultado
                RetornoOperacion resultado = new RetornoOperacion();

                //Obtenemos link
                LinkButton lnk = (LinkButton)sender;

                //Seleccionando la fila actual
                Controles.SeleccionaFila(gvRecursosDisponibles, sender, "lnk", false);

                //Declaramos variables para almacenar el tipo de recurso
                MovimientoAsignacionRecurso.Tipo tipo;
                int tipo_unidad;

                //Obtenemos Tipo de recurso  para su asignación
                obtieneTipoRecurso(out tipo, out tipo_unidad);

                //Asignando Recurso
                this._id_recurso = Convert.ToInt32(gvRecursosDisponibles.SelectedValue);
                this._id_tipo_asignacion = Convert.ToByte(tipo);

                //Validamos Asignación ligadas al Recurso 
                resultado = MovimientoAsignacionRecurso.ValidaLiberacionRecurso(tipo, Convert.ToInt32(gvRecursosDisponibles.SelectedValue));

                //Si existe asignación ligada
                if (!resultado.OperacionExitosa)
                {
                    //Asignamos Mensaje
                    lblMensajeLiberacionRecurso.Text = resultado.Mensaje;
                    //Mostramos Ventana Modal
                    ScriptServer.AlternarVentana(lnk, lnk.GetType(), "MostrarLiberacionUnidad", "contenidoConfirmacionLiberacionRecurso", "confirmacionLiberacionRecurso");
                }
                else
                {
                    //Validando que exista un Evento
                    if (ClickLiberarRecurso != null)

                        //Iniciando Manejador
                        OnClickLiberarRecurso(e);

                    //Cerramos Ventana Modal
                    ScriptServer.AlternarVentana(upgvRecursosDisponibles, upgvRecursosDisponibles.GetType(), "CerrarLiberacionUnidad", "contenidoConfirmacionLiberacionRecurso", "confirmacionLiberacionRecurso");

                }
            }
        }

        /// <summary>
        /// Evento generado al liberar los Recursos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarLiberacionRecurso_Click(object sender, EventArgs e)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();
            //Declaramos variables para almacenar el tipo de recurso
            MovimientoAsignacionRecurso.Tipo tipo;
            int tipo_unidad;

            //Obtenemos Tipo de recurso  para su asignación
            obtieneTipoRecurso(out tipo, out tipo_unidad);

            //Asignando Recurso
            this._id_recurso = Convert.ToInt32(gvRecursosDisponibles.SelectedValue);
            this._id_tipo_asignacion = Convert.ToByte(tipo);


            //Validando que exista un Evento
            if (ClickLiberarRecurso != null)

                //Iniciando Manejador
                OnClickLiberarRecurso(e);

            //Cerramos Ventana Modal
            ScriptServer.AlternarVentana(upbtnAceptarLiberacionRecurso, upbtnAceptarLiberacionRecurso.GetType(), "CerrarLiberacionUnidad", "contenidoConfirmacionLiberacionRecurso", "confirmacionLiberacionRecurso");

        }

        /// <summary>
        /// Evento producido al pulsar el link Reubicar el recurso
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbReubicarRecurso_Click(object sender, EventArgs e)
        {
            //Validamos que existan Unidades
            if (gvRecursosDisponibles.DataKeys.Count > 0)
            {
                //Seleccionando la fila actual
                Controles.SeleccionaFila(gvRecursosDisponibles, sender, "lnk", false);

                //Declaramos variables para almacenar el tipo de recurso
                MovimientoAsignacionRecurso.Tipo tipo;
                int tipo_unidad;

                //Obtenemos Tipo de recurso  para su asignación
                obtieneTipoRecurso(out tipo, out tipo_unidad);

                //Asignando Recurso
                this._id_recurso = Convert.ToInt32(gvRecursosDisponibles.SelectedValue);
                this._id_tipo_asignacion = Convert.ToByte(tipo);

                //Obteniendo Control
                LinkButton lnkReu = (LinkButton)sender;

                //Validando que exista un Evento
                if (ClickReubicarRecurso != null)

                    //Iniciando Manejador
                    OnClickReubicarRecurso(e);
            }
        }

        #endregion

        #region Eventos Anticipos

        /// <summary>
        /// Evento Generado al dar click en el nuevo depósito
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNuevoDeposito_Click(object sender, EventArgs e)
        {
            //Declaramos objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Declaramos variables
            int id_unidad = 0; int id_operador = 0; int id_tercero = 0;
            //Indicando que no es requerido mostrar solo concepto de anticipo a proveedor
            ucDepositos.SoloAnticipoProveedor = false;

            //Validamos si el Tipo de Asignación actual es  el correcto, obtenemos recurso por asignar
            resultado = SAT_CL.Despacho.MovimientoAsignacionRecurso.ObtieneTipoAsignacion(out id_unidad, out id_operador, out id_tercero, Convert.ToInt32(gvRecursosAsignados.SelectedDataKey["Id"]),
                                   Convert.ToInt32(gvRecursosAsignados.SelectedDataKey["IdRecurso"]));
            //Validamos Resultado
            if (resultado.OperacionExitosa)
            {
                //Mostramos Vista Depòsito
                mtvAnticipos.ActiveViewIndex = 0;

                //Recuperando Id de otro proveedor
                int id_otro_proveedor = Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtOtroProveedor.Text, "ID:", 1));

                //Si se hace petición de otro proveedor
                if (id_otro_proveedor > 0)
                {
                    //Sobreescribiendo Id de Proveedor e inicializando unidad y operador
                    id_tercero = id_otro_proveedor;
                    id_unidad = id_operador = 0;
                    //Asignando atributo a control de depósitos
                    ucDepositos.SoloAnticipoProveedor = true;

                }

                //Instanciando Movimiento
                using (Movimiento mov = new Movimiento(this._id_movimiento))
                {
                    //Instanciando Servicio
                    using (SAT_CL.Documentacion.Servicio ser = new SAT_CL.Documentacion.Servicio(mov.id_servicio))
                    {
                        ucDepositos.HabilitaConcepto = true;
                        ucDepositos.MuestraSolicitar = false;
                        //Inicializamos Control Depòsito en Edición
                        ucDepositos.InicializaControl(0, id_unidad, id_operador, id_tercero, ser.id_servicio, mov.id_movimiento,
                                  Convert.ToInt32(gvRecursosAsignados.SelectedDataKey["Id"]), ser.id_cliente_receptor);

                    }
                }
            }

            //Sólo si hay error
            if (!resultado.OperacionExitosa)
                //Notificando que no es posible realizar esta acción para otro proveedor
                ScriptServer.MuestraNotificacion(btnNuevoDeposito, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Evento Generado al dar click en el nuevo depósito programado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNuevoDepositoProgramado_Click(object sender, EventArgs e)
        {
            //Instanciando Movimiento
            using (Movimiento mov = new Movimiento(this._id_movimiento))
            {
                //Instanciando Servicio
                using (SAT_CL.Documentacion.Servicio ser = new SAT_CL.Documentacion.Servicio(mov.id_servicio))
                {
                    //Crea variable que almacena la ruta de apertura de Documentacion por servicio
                    //string urlDestino = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~Operacion/Despacho.aspx", "Documentacion/AnticiposProgramados.aspx?idServicio=" + ser.id_servicio);
                    string url = Cadena.RutaRelativaAAbsoluta("~/Operacion/Despacho.aspx", "~/Documentacion/AnticiposProgramados.aspx?idReg1=" + ser.no_servicio);
                    Response.Redirect(url);
                }
            }
        }
        /// <summary>
        /// Evento generado al dar click en nuevo vale de Diesel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNuevoVale_Click(object sender, EventArgs e)
        {
            //Declaramos objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Recuperando Id de otro proveedor
            int id_otro_proveedor = Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtOtroProveedor.Text, "ID:", 1));

            //Si se solicitó otro proveedor
            if (id_otro_proveedor > 0)
            {
                //Asignando resultado
                resultado = new RetornoOperacion("No es posible asignar Diesel a este Proveedor.");
            }
            else
            {
                //Declaramos variables
                int id_unidad = 0; int id_operador = 0; int id_tercero = 0;

                //Validamos si el Tipo de Asignación actual es  el correcto, obtenemos recurso por asignar
                resultado = SAT_CL.Despacho.MovimientoAsignacionRecurso.ObtieneTipoAsignacion(out id_unidad, out id_operador, out id_tercero, Convert.ToInt32(gvRecursosAsignados.SelectedDataKey["Id"]),
                                                    Convert.ToInt32(gvRecursosAsignados.SelectedDataKey["IdRecurso"]));
                //Validamos Resultado
                if (resultado.OperacionExitosa)
                {
                    //Mostramos Vista Vale
                    mtvAnticipos.ActiveViewIndex = 1;

                    //Instanciando Movimiento
                    using (Movimiento mov = new Movimiento(this._id_movimiento))

                    //Instanciando Servicio
                    using (SAT_CL.Documentacion.Servicio ser = new SAT_CL.Documentacion.Servicio(mov.id_servicio))
                    {
                        //Inicializamos Control Movimiento en vacio
                        ucAsignacionDiesel.InicializaControlUsuario(0, id_unidad, id_operador, id_tercero, ser.id_servicio, this._id_movimiento,
                                                                    Convert.ToInt32(gvRecursosAsignados.SelectedDataKey["Id"]));
                    }
                }
            }

            //Sólo si hay error
            if (!resultado.OperacionExitosa)
                //Notificando que no es posible realizar esta acción para otro proveedor
                ScriptServer.MuestraNotificacion(btnNuevoVale, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        /// <summary>
        /// Mostramos Vista de Calculo de Ruta
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCalcularRuta_Click(object sender, EventArgs e)
        {
            //Declaramos objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();
            //Mostramos Ruta
            mtvAnticipos.ActiveViewIndex = 3;

            //Instanciamos Movimiento
            using (SAT_CL.Despacho.Movimiento objMovimiento = new Movimiento(this._id_movimiento))
            {
                if (objMovimiento.habilitar)
                {

                    int id_unidad = 0; int id_operador = 0; int id_tercero = 0;

                    //Validamos si el Tipo de Asignación actual es  el correcto, obtenemos recurso por asignar
                    resultado = SAT_CL.Despacho.MovimientoAsignacionRecurso.ObtieneTipoAsignacion(out id_unidad, out id_operador, out id_tercero, Convert.ToInt32(gvRecursosAsignados.SelectedDataKey["Id"]),
                                                        Convert.ToInt32(gvRecursosAsignados.SelectedDataKey["IdRecurso"]));
                    //Inicializando contenido de control para el Calculo de Ruta
                    wucCalcularRuta.InicializaControl(objMovimiento.id_servicio, Convert.ToInt32(gvRecursosAsignados.SelectedDataKey["Id"]), id_unidad, id_operador, id_tercero);
                }
            }
        }

        /// <summary>
        /// Evento generado al Cerrar la Ventana de Anticipos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarAnticiposR_Click(object sender, EventArgs e)
        {
            //Regresando a Vista Principal
            mvwPrincipal.ActiveViewIndex = 0;

            //Inicializando Indices
            TSDK.ASP.Controles.InicializaIndices(gvRecursosAsignados);
        }

        #region Eventos UserControl "Depositos"

        /// <summary>
        /// Eventó Generado al Registrar un  Depósito
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucDepositos_ClickRegistrar(object sender, EventArgs e)
        {
            //Declaramos objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Declaramos variable para almacenar Recurso
            int id_unidad = 0;
            int id_operador = 0;
            int id_tercero = 0;

            //Validamos si el Tipo de Asignación actual es  el correcto y obtenemos asignaciòn
            SAT_CL.Despacho.MovimientoAsignacionRecurso.ObtieneTipoAsignacion(out id_unidad, out id_operador, out id_tercero, Convert.ToInt32(gvRecursosAsignados.SelectedDataKey["Id"]),
                                     Convert.ToInt32(gvRecursosAsignados.SelectedDataKey["IdRecurso"]));
            //Insertamos Depósitos
            resultado = ucDepositos.RegistraDeposito();

            //Validamos Inserrciòn de Depósito
            if (resultado.OperacionExitosa)
            {
                //Instanciando Deposito
                using (SAT_CL.EgresoServicio.Deposito dep = new SAT_CL.EgresoServicio.Deposito(resultado.IdRegistro))

                //Instanciando Concepto de Deposito
                using (SAT_CL.EgresoServicio.ConceptoDeposito cd = new SAT_CL.EgresoServicio.ConceptoDeposito(dep.id_concepto))
                {
                    //Validando que existan los Registros
                    if (dep.habilitar && cd.habilitar)
                    {
                        //Validando que 
                        if (!cd.descripcion.Equals("Anticipo Proveedor"))
                        {
                            //Validamos Estatus del Depósito diferente de Registrado
                            if (dep.id_estatus != 1)
                            {
                                //Mostramos Reporte Anticipos
                                mtvAnticipos.ActiveViewIndex = 2;
                                //cargamos Anticipos
                                cargaAnticiposEntidad(id_operador, id_unidad, id_tercero);
                            }
                        }
                    }

                }
            }
        }

        /// <summary>
        /// Evento generado al Solicitar un Depósito
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucDepositos_ClickSolicitar(object sender, EventArgs e)
        {
            //Declaramos objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Declaramos variable para almacenar Recurso
            int id_unidad = 0;
            int id_operador = 0;
            int id_tercero = 0;

            //Validamos si el Tipo de Asignación actual es  el correcto y obtenemos asignaciòn
            SAT_CL.Despacho.MovimientoAsignacionRecurso.ObtieneTipoAsignacion(out id_unidad, out id_operador, out id_tercero, Convert.ToInt32(gvRecursosAsignados.SelectedDataKey["Id"]),
                                     Convert.ToInt32(gvRecursosAsignados.SelectedDataKey["IdRecurso"]));

            //Declaramos Objeto Resultado
            resultado = ucDepositos.SolicitaDeposito();
            //Instanciando Deposito
            using (SAT_CL.EgresoServicio.Deposito dep = new SAT_CL.EgresoServicio.Deposito(resultado.IdRegistro))
            {
                //Validamos Resultado
                if (resultado.OperacionExitosa)
                {
                    //Validamos que el Deppósito se se encuentre Registrado
                    if (dep.id_estatus != 1)
                    {
                        //Mostramos Reporte Anticipos
                        mtvAnticipos.ActiveViewIndex = 2;
                        //cargamos Anticipos
                        cargaAnticiposEntidad(id_operador, id_unidad, id_tercero);
                    }
                }
            }

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this.Page, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Eventó Generado al Cancelar un Depósito
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucDepositos_ClickCancelar(object sender, EventArgs e)
        {
            //Declaramos variable para almacenar Recurso
            int id_unidad = 0;
            int id_operador = 0;
            int id_tercero = 0;

            //Validamos si el Tipo de Asignación actual es  el correcto y obtenemos asignaciòn
            SAT_CL.Despacho.MovimientoAsignacionRecurso.ObtieneTipoAsignacion(out id_unidad, out id_operador, out id_tercero, Convert.ToInt32(gvRecursosAsignados.SelectedDataKey["Id"]),
                                     Convert.ToInt32(gvRecursosAsignados.SelectedDataKey["IdRecurso"]));
            //Mostramos Reporte Anticipos
            mtvAnticipos.ActiveViewIndex = 2;
            //cargamos Anticipos
            cargaAnticiposEntidad(id_operador, id_unidad, id_tercero);
        }
        /// <summary>
        /// Eventó Generado al Eliminar un Depósito
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucDepositos_ClickEliminar(object sender, EventArgs e)
        {
            //Declaramos objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Declaramos variable para almacenar Recurso
            int id_unidad = 0;
            int id_operador = 0;
            int id_tercero = 0;

            //Validamos si el Tipo de Asignación actual es  el correcto y obtenemos asignaciòn
            SAT_CL.Despacho.MovimientoAsignacionRecurso.ObtieneTipoAsignacion(out id_unidad, out id_operador, out id_tercero, Convert.ToInt32(gvRecursosAsignados.SelectedDataKey["Id"]),
                                     Convert.ToInt32(gvRecursosAsignados.SelectedDataKey["IdRecurso"]));
            //Insertamos Depósitos
            //Eliminamos Depósito
            resultado = ucDepositos.DeshabilitaDeposito();

            //Validamos Inserrciòn de Depósito
            if (resultado.OperacionExitosa)
            {
                //Mostramos Reporte Anticipos
                mtvAnticipos.ActiveViewIndex = 2;
                //cargamos Anticipos
                cargaAnticiposEntidad(id_operador, id_unidad, id_tercero);
            }

            //Mostrando Mensaje
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        #endregion

        #region Eventos UserControl "Diesel"

        /// <summary>
        /// Eventó Generado al Cancelar un Vale de Diesel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucAsignacionDiesel_ClickCancelarAsignacion(object sender, EventArgs e)
        {
            //Mostramos Reporte Anticipos
            mtvAnticipos.ActiveViewIndex = 2;
        }
        /// <summary>
        /// Evento generado al Guardar un Vale de Diesel
        /// </summary>
        protected void ucAsignacionDiesel_ClickGuardarAsignacion(object sender, EventArgs e)
        {
            //Declaramos objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Declaramos variable para almacenar Recurso
            int id_unidad = 0;
            int id_operador = 0;
            int id_tercero = 0;

            //Validamos si el Tipo de Asignación actual es  el correcto y obtenemos asignaciòn
            SAT_CL.Despacho.MovimientoAsignacionRecurso.ObtieneTipoAsignacion(out id_unidad, out id_operador, out id_tercero, Convert.ToInt32(gvRecursosAsignados.SelectedDataKey["Id"]),
                                     Convert.ToInt32(gvRecursosAsignados.SelectedDataKey["IdRecurso"]));
            //Insertamos Diesel
            resultado = ucAsignacionDiesel.GuardaDiesel();

            //Validamos Onsercciòn de Diesel
            if (resultado.OperacionExitosa)
            {
                //Mostramos Reporte Anticipos
                mtvAnticipos.ActiveViewIndex = 2;

                //cargamos Anticipos
                cargaAnticiposEntidad(id_operador, id_unidad, id_tercero);
            }
        }

        /// <summary>
        /// Evento Generado al Calculado Diesel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucAsignacionDiesel_ClickCalculado1(object sender, EventArgs e)
        {
            InicializaInformacionDieselKms(ucAsignacionDiesel);
            //Mostramos Ventana Modal
            TSDK.ASP.ScriptServer.AlternarVentana(ucAsignacionDiesel, "Calculado", "contenedorVentanaConfirmacionInformacionCalculado", "ventanaConfirmacionInformacionCalculado");
        }

        /// <summary>
        /// Evento generado al dar click en Cerrar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkCerrarVentanaInformacionCalculado_Click(object sender, EventArgs e)
        {
            //Mostramos Ventana Modal
            TSDK.ASP.ScriptServer.AlternarVentana(lnkCerrarVentanaInformacionCalculado, "Calculado", "contenedorVentanaConfirmacionInformacionCalculado", "ventanaConfirmacionInformacionCalculado");

        }
        /// <summary>
        /// Inicializamos Información de Diesel y Kms
        /// <
        /// </summary>
        private void InicializaInformacionDieselKms(System.Web.UI.Control control)
        {
            //Declaramos Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);
            //Declarando Variables Auxiliares cap_unidad = 0;
            decimal cap_unidad = 0;
            int id_unidad = 0;
            DateTime fecha_carga = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro();
            //Instanciamos Diesel
            using (SAT_CL.EgresoServicio.AsignacionDiesel objAsignacionDiesel = new SAT_CL.EgresoServicio.AsignacionDiesel(ucAsignacionDiesel.idAsignacionDiesel))
            {
                //Validamos Vale
                if (objAsignacionDiesel.id_asignacion_diesel > 0)
                {
                    //Establecemos Fecha Carga
                    fecha_carga = objAsignacionDiesel.fecha_carga;
                }
            }

            //Instanciando Unidad
            using (SAT_CL.Global.Unidad uni = new SAT_CL.Global.Unidad(ucAsignacionDiesel.idUnidadDiesel))
            {
                //Validando que Exista la Unidad
                if (uni.id_unidad > 0)

                    //Obteniendo Capacidad de Combustible
                    cap_unidad = uni.capacidad_combustible;
                id_unidad = uni.id_unidad;


                //Obtenemos rendimiento
                decimal rendimiento = Convert.ToDecimal(Cadena.VerificaCadenaVacia(SAT_CL.Global.Referencia.CargaReferencia("0", 19, id_unidad, "Rendimiento Unidad", "Rendimiento"), "0"));
                //Si el Rendimiento No existe
                if (rendimiento == 0)
                {
                    //Si el Kilometraje es Diferente de O
                    if (uni.kilometraje_asignado != 0)
                    {
                        //Siel Combustible Asignado es Diferente de 0
                        if (uni.combustible_asignado != 0)
                        {
                            //Calculamos rendimiento
                            rendimiento = uni.kilometraje_asignado / uni.combustible_asignado;
                        }
                    }
                }

                //Inicializamos Valores
                lblCapacidadTanque.Text = cap_unidad.ToString() + "lts";
                lblRendimiento.Text = Cadena.TruncaCadena(rendimiento.ToString(), 5, "") + "kms/lts";
                lblFechaUltimaCarga.Text = SAT_CL.EgresoServicio.DetalleLiquidacion.ObtieneUltimaFechaCargaDiesel(id_unidad, ucAsignacionDiesel.idAsignacionDiesel).ToString("dd/MM/yy HH:mm");
                lblKmsUltimaCarga.Text = SAT_CL.Global.Unidad.ObtieneKmsTotalesUltimacarga(id_unidad, SAT_CL.EgresoServicio.DetalleLiquidacion.ObtieneUltimaFechaCargaDiesel(id_unidad, ucAsignacionDiesel.idAsignacionDiesel), fecha_carga).ToString() + "kms";
                //Validamos que sea diferente de 0 para la Division
                if (rendimiento > 0)
                {
                    lblCalculado.Text = Cadena.TruncaCadena((SAT_CL.Global.Unidad.ObtieneKmsTotalesUltimacarga(id_unidad, SAT_CL.EgresoServicio.DetalleLiquidacion.ObtieneUltimaFechaCargaDiesel(id_unidad, ucAsignacionDiesel.idAsignacionDiesel), fecha_carga) / Convert.ToDecimal(rendimiento)).ToString(), 5, "") + "lts";
                    lblSobrante.Text = Cadena.TruncaCadena((Convert.ToDecimal(cap_unidad) - (SAT_CL.Global.Unidad.ObtieneKmsTotalesUltimacarga(id_unidad, SAT_CL.EgresoServicio.DetalleLiquidacion.ObtieneUltimaFechaCargaDiesel(id_unidad, ucAsignacionDiesel.idAsignacionDiesel), fecha_carga) / Convert.ToDecimal(rendimiento))).ToString(), 5, "") + "lts";
                    lblAlcanceKms.Text = Cadena.TruncaCadena(((Convert.ToDecimal(cap_unidad) - (SAT_CL.Global.Unidad.ObtieneKmsTotalesUltimacarga(id_unidad, SAT_CL.EgresoServicio.DetalleLiquidacion.ObtieneUltimaFechaCargaDiesel(id_unidad, ucAsignacionDiesel.idAsignacionDiesel), fecha_carga) / Convert.ToDecimal(rendimiento))) * rendimiento).ToString(), 5, "") + "kms.";
                }
                else
                {
                    //Mostramos Resultado
                    resultado = new RetornoOperacion("El rendimiento debe ser Mayor a 0");
                }
            }
            //Mostrando resultado
            ScriptServer.MuestraNotificacion(control, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);

        }

        #endregion

        #region Eventos GridView "Anticipos"

        /// <summary>
        /// Cambio de marcado en opción Otro proveedor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkOtroProveedor_CheckedChanged(object sender, EventArgs e)
        {
            //Limpiando caja de texto de busqueda de otro proveedor
            txtOtroProveedor.Text = "";

            //SI se ha marcado la opción
            if (chkOtroProveedor.Checked)
            {
                //Cambiando indice activo
                mtvAsignacionActivaDeposito.SetActiveView(vwOtroProveedor);
                //Cargando gv de anticipos en blanco
                Controles.InicializaGridview(gvAnticipos);
                //Borrando de sesión los viajes cargados anteriormente
                this._ds = OrigenDatos.EliminaTablaDataSet(this._ds, "Table3");
                //Definiendo foco en control de busqueda
                txtOtroProveedor.Focus();
            }
            //De lo contrario
            else
            {
                //Cambiando indice activo
                mtvAsignacionActivaDeposito.SetActiveView(vwAsignacionMovimiento);

                //Declaramos variables
                int id_unidad = 0; int id_operador = 0; int id_tercero = 0;
                //Validamos si el Tipo de Asignación actual es  el correcto y obtenemos asignaciòn
                if (SAT_CL.Despacho.MovimientoAsignacionRecurso.ObtieneTipoAsignacion(out id_unidad, out id_operador, out id_tercero, Convert.ToInt32(gvRecursosAsignados.SelectedDataKey["Id"]),
                                       Convert.ToInt32(gvRecursosAsignados.SelectedDataKey["IdRecurso"])).OperacionExitosa)
                {
                    //Cargando anticipos del recurso actual
                    cargaAnticiposEntidad(id_operador, id_unidad, id_tercero);
                }
                else
                {
                    //Cargando gv de anticipos en blanco
                    Controles.InicializaGridview(gvAnticipos);
                    //Borrando de sesión los viajes cargados anteriormente
                    this._ds = OrigenDatos.EliminaTablaDataSet(this._ds, "Table3");
                }

            }
        }
        /// <summary>
        /// Cambio de selección de proveedor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtOtroProveedor_TextChanged(object sender, EventArgs e)
        {
            //Obteniendo Id de otro proveedor
            int id_proveedor = Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtOtroProveedor.Text, "ID:", 1));

            //Cargando listado de anticipos del proveedor especificado ( en caso de ser solicitado)
            if (id_proveedor > 0)
            {
                cargaAnticiposEntidad(0, 0, id_proveedor);
            }
            else
            {
                //Cargando gv de anticipos en blanco
                Controles.InicializaGridview(gvAnticipos);
                //Borrando de sesión los viajes cargados anteriormente
                this._ds = OrigenDatos.EliminaTablaDataSet(this._ds, "Table3");
            }
        }
        /// <summary>
        /// Evento producido al pulsar el link Anticipos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbAnticipos_OnClick(object sender, EventArgs e)
        {
            //Recuperando el botón pulsado
            LinkButton b = (LinkButton)sender;

            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Declaramos variables
            int id_unidad = 0; int id_operador = 0; int id_tercero = 0;

            //Instanciando Movimiento
            using (Movimiento mov = new Movimiento(this._id_movimiento))

            //Instanciando Servicio
            using (SAT_CL.Documentacion.Servicio serv = new SAT_CL.Documentacion.Servicio(mov.id_servicio))
            {
                //Si existen registros
                if (gvAnticipos.DataKeys.Count > 0)
                {
                    //Seleccionando la fila actual
                    Controles.SeleccionaFila(gvAnticipos, sender, "lnk", false);

                    //Validamos si el Tipo de Asignación actual es  el correcto, obtenemos recurso por asignar
                    resultado = SAT_CL.Despacho.MovimientoAsignacionRecurso.ObtieneTipoAsignacion(out id_unidad, out id_operador, out id_tercero, Convert.ToInt32(gvRecursosAsignados.SelectedDataKey["Id"]),
                                           Convert.ToInt32(gvRecursosAsignados.SelectedDataKey["IdRecurso"]));

                    //Si la Asignaciòn es correcta
                    if (resultado.OperacionExitosa)
                    {
                        //En base al comando definido para el botón
                        switch (b.CommandName)
                        {
                            case "Editar":
                                {
                                    //Si el tipo de operaciòn es Depòsito
                                    if (gvAnticipos.SelectedDataKey["Tipo"].ToString() == "Deposito")
                                    {
                                        if (Convert.ToInt32(gvAnticipos.SelectedDataKey["Programado"]) == 0)
                                        {
                                            //Mostramos Vista Depòsito
                                            mtvAnticipos.ActiveViewIndex = 0;
                                            ucDepositos.HabilitaConcepto = false;
                                            ucDepositos.MuestraSolicitar = true;
                                            //Inicializamos Control Depòsito en Ediciòn
                                            ucDepositos.InicializaControl(Convert.ToInt32(gvAnticipos.SelectedValue), id_unidad, id_operador, id_tercero,
                                                      serv.id_servicio, this._id_movimiento, Convert.ToInt32(gvRecursosAsignados.SelectedDataKey["Id"]), serv.id_cliente_receptor);          
                                        }
                                        else
                                            inicializaAnticiposProgramados(serv.id_servicio, Convert.ToInt32(gvAnticipos.SelectedValue));
                                    }
                                    else if(gvAnticipos.SelectedDataKey["Tipo"].ToString() == "ValeDiesel")
                                    {
                                        //Mostramos Vista Vale
                                        mtvAnticipos.ActiveViewIndex = 1;
                                        //Inicializamos Control Movimiento en vacio
                                        ucAsignacionDiesel.InicializaControlUsuario(Convert.ToInt32(gvAnticipos.SelectedValue), id_unidad, id_operador, id_tercero,
                                                  serv.id_servicio, this._id_movimiento, Convert.ToInt32(gvRecursosAsignados.SelectedDataKey["Id"]));
                                    }                                    
                                    break;
                                }
                            case "Factura":
                                {
                                    //Si el tipo de operaciòn es Depòsito
                                    if (gvAnticipos.SelectedDataKey["Tipo"].ToString() == "Deposito")
                                    {
                                        //Asigna valores a la variable url con la ruta de ubicacion de la ventana Abrir registros con los datos obtenidos de la tabla Usuario
                                        string url = Cadena.RutaRelativaAAbsoluta("~/UserControls/wucAsignacionRecurso.ascx", "~/Accesorios/ServicioFacturas.aspx?idRegistro=51&idRegistroB=" + Convert.ToInt32(gvAnticipos.SelectedValue).ToString() + "&idRegistroC=" + serv.id_servicio.ToString());
                                        //Define las dimensiones de la ventana Abrir registros de Usuario
                                        string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=1150,height=400";
                                        //Invoca al método de la variable ScripServer que abrira una nueva ventana con los registros pertenecientes a la tabla Usuario
                                        ScriptServer.AbreNuevaVentana(url, "Abrir Registro Usuario", configuracion, Page);
                                    }
                                    break;
                                }
                        }
                    }
                    else
                        //Establecemos Mensaje Error
                        lblOrdenarAnticipos.Text = resultado.Mensaje;
                }
            }
        }
        /// <summary>
        /// Evento generado al exportar el contenido de gvAnticipos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarAnticipos_Click(object sender, EventArgs e)
        {
            //Exportando eventos
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet(this._ds, "Table3"));
        }
        /// <summary>
        /// Evento generado al cambiar el Tamaño de gvAnticipos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoAnticipos_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Validando que Existan Registros
            if (gvAnticipos.DataKeys.Count > 0)
            {
                //Asignando Expresión anterior
                this._ds.Tables["Table3"].DefaultView.Sort = lblOrdenarAnticipos.Text;

                //Cambia Tamaño del GridView
                Controles.CambiaTamañoPaginaGridView(gvAnticipos, OrigenDatos.RecuperaDataTableDataSet(this._ds, "Table3"),
                                            Convert.ToInt32(ddlTamanoAnticipos.SelectedValue), true, 9);
            }
        }
        /// <summary>
        /// Evento Producido al Cambiar el orden del Gridview "Anticipos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvAnticipos_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Validando que Existan Registros
            if (gvAnticipos.DataKeys.Count > 0)
            {
                //Asignando Expresión anterior
                this._ds.Tables["Table3"].DefaultView.Sort = lblOrdenarAnticipos.Text;

                //Ordenando el GridView
                lblOrdenarAnticipos.Text = Controles.CambiaSortExpressionGridView(gvAnticipos, OrigenDatos.RecuperaDataTableDataSet(this._ds, "Table3"), e.SortExpression, true, 9);
            }
        }
        /// <summary>
        /// Evento Prodicido al Cambiar el Indice de página del Gridview "Anticipos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvAnticipos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Validando que Existan Registros
            if (gvAnticipos.DataKeys.Count > 0)
            {
                //Asignando Expresión anterior
                this._ds.Tables["Table3"].DefaultView.Sort = lblOrdenarAnticipos.Text;

                //Cambiando el indice del GridView
                Controles.CambiaIndicePaginaGridView(gvAnticipos, OrigenDatos.RecuperaDataTableDataSet(this._ds, "Table3"), e.NewPageIndex, true, 9);
            }
        }
        /// <summary>
        /// Evento Producido  al enlazar los datos del Grid View Anticipos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvAnticipos_RowDataBound(object sender, GridViewRowEventArgs e)
        {            
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lkbEditar = (LinkButton)e.Row.FindControl("lkbEditar");
                if (((DataRowView)e.Row.DataItem).Row.ItemArray[14].ToString().Contains("CasetasIAVE"))
                {
                    lkbEditar.Visible = false;
                }
            }
        }

        #endregion

        #endregion

        #endregion

        #region Métodos

        #region Métodos Privados

        /// <summary>
        /// Método encargado de Asignar los Atributos de la Forma
        /// </summary>
        private void asignaAtributos()
        {
            //Asignando Atributos
            ViewState["AsignacionRecurso"] = this._ds;
            ViewState["idMovimiento"] = this._id_movimiento;
            ViewState["idOrigen"] = this._id_origen;
            ViewState["idUbicacion"] = this._id_ubicacion;
            ViewState["idCompania"] = this._id_compania;
            ViewState["idRecurso"] = this.idRecurso;
            ViewState["idTipoAsignacion"] = this.idTipoAsignacion;
        }
        /// <summary>
        /// Método encargado de Recuperar el Valor de los Atributos de la Forma
        /// </summary>
        private void recuperaAtributos()
        {
            //Recuperando Atributos
            if (TSDK.Datos.Validacion.ValidaOrigenDatos((DataSet)ViewState["AsignacionRecurso"]))

                //Asignando Servicios Maestros
                this._ds = (DataSet)ViewState["AsignacionRecurso"];
            //Movimiento
            if (Convert.ToInt32(ViewState["idMovimiento"]) > 0)
                this._id_movimiento = Convert.ToInt32(ViewState["idMovimiento"]);
            //Origen
            if (Convert.ToInt32(ViewState["idOrigen"]) > 0)
                this._id_origen = Convert.ToInt32(ViewState["idOrigen"]);
            //Ubicación
            if (Convert.ToInt32(ViewState["idUbicacion"]) > 0)
                this._id_ubicacion = Convert.ToInt32(ViewState["idUbicacion"]);
            //Compania
            if (Convert.ToInt32(ViewState["idCompania"]) > 0)
                this._id_compania = Convert.ToInt32(ViewState["idCompania"]);
            //Compania
            if (Convert.ToInt32(ViewState["idRecurso"]) > 0)
                this._id_recurso = Convert.ToInt32(ViewState["idRecurso"]);
            //Compania
            if (Convert.ToByte(ViewState["idTipoAsignacion"]) > 0)
                this._id_tipo_asignacion = Convert.ToByte(ViewState["idTipoAsignacion"]);
        }
        /// <summary>
        /// Método encargado de Inicializar la Configuración del Control
        /// </summary>
        private void inicializaControl()
        {
            //Instanciando Parada
            using (Parada stop = new Parada(this._id_origen))
            {
                //Validando que existe la Parada
                if (stop.id_parada > 0)

                    //Asignando Ubicación
                    this._id_ubicacion = stop.id_ubicacion;
            }

            //Limpiando Controles de Busqueda
            txtValor.Text =
            lblErrorRecursosAsignados.Text = "";

            //Inicializando Indices
            Controles.InicializaIndices(gvRecursosAsignados);

            //Invocando Método de Carga
            cargaCatalogos();

            //Mostrando Vista Principal
            mvwPrincipal.ActiveViewIndex = 0;

            //Invocando Método de Carga
            cargaRecursosAsignados();

            //Inicialiamos Grid View
            Controles.InicializaGridview(gvRecursosDisponibles);
        }
        
        /// <summary>
        /// Método encargado de Cargar los Catalogos del Control de Usuario
        /// </summary>
        private void cargaCatalogos()
        {
            //Recursos Disponibles
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoRecursosDisponibles, "", 18);

            //Anticipos
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoAnticipos, "", 18);
            //Tipo Asignacion
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipoAsignacion, "", 46);
        }
        /// <summary>   
        /// Método encarga de obtener el tipo de recurso (Unidad, Operador, Tercero)
        /// </summary>
        /// <param name="tipo"> Tipo recurso (Unidad, Operador, Tercero)</param>
        /// <param name="tipo_unidad">Tipo Tractor (Remolque, Dolly,Tractor)</param>
        private void obtieneTipoRecurso(out MovimientoAsignacionRecurso.Tipo tipo, out int tipo_unidad)
        {
            //Declaramos variables
            tipo = MovimientoAsignacionRecurso.Tipo.Operador;
            tipo_unidad = 0;

            //Obtenemos Tipo de Recurso
            switch (ddlTipoAsignacion.SelectedValue)
            {
                case "1":
                    {
                        tipo = MovimientoAsignacionRecurso.Tipo.Unidad;
                        //Instanciamos Unidad
                        using (SAT_CL.Global.Unidad objUnidad = new SAT_CL.Global.Unidad(Convert.ToInt32(gvRecursosDisponibles.SelectedValue)))
                        {
                            //Establecemos valor del Tipo de Unidad
                            tipo_unidad = objUnidad.id_tipo_unidad;
                        }
                        break;
                    }
                case "2":
                    {
                        tipo = MovimientoAsignacionRecurso.Tipo.Operador;
                        break;
                    }
                case "3":
                    {
                        tipo = MovimientoAsignacionRecurso.Tipo.Tercero;
                        break;
                    }
            }
        }
        /// <summary>
        /// Método encarga de obtener el tipo de recurso Asignado (Unidad, Operador, Tercero)
        /// </summary>
        /// <param name="tipo"> Tipo recurso (Unidad, Operador, Tercero)</param>
        private void obtieneTipoRecursoAsignado(out MovimientoAsignacionRecurso.Tipo tipo)
        {
            //Declaramos variables
            tipo = MovimientoAsignacionRecurso.Tipo.Operador;

            //Obtenemos Tipo de Recurso
            switch (ddlTipoAsignacion.SelectedValue)
            {
                case "1":
                    {
                        tipo = MovimientoAsignacionRecurso.Tipo.Unidad;
                        break;
                    }
                case "2":
                    {
                        tipo = MovimientoAsignacionRecurso.Tipo.Operador;
                        break;
                    }
                case "3":
                    {
                        tipo = MovimientoAsignacionRecurso.Tipo.Tercero;
                        break;
                    }
            }
        }

        #region Métodos Recursos Disponibles

        /// <summary>
        /// Método Privado encargado de Insertar el link Agregar en las filas
        /// </summary>
        /// <param name="fila"></param>
        private void insertaCeldaAgregar(GridViewRow fila)
        {   //Declarando variables a utilizar
            TableCell celda = new TableCell();
            ///////Determinando el tipo de celda///////
            //Si es celda de datos
            if (fila.RowType == DataControlRowType.DataRow)
            {
                //Instanciando nuevo LinkButton
                LinkButton lkbAgregar = new LinkButton();
                //Asignando Id de control
                lkbAgregar.ID = "lkbAgregar";
                //Estableciendo texto
                lkbAgregar.Text = "Agregar";
                //Estableciendo estilo
                lkbAgregar.CssClass = "LinkButton";
                //Estableciendo evento click
                lkbAgregar.Click += lkbAgregarRecurso_Click;
                //Quitando validacion
                lkbAgregar.CausesValidation = false;

                //Instanciando nuevo update panel
                UpdatePanel up = new UpdatePanel();
                up.UpdateMode = UpdatePanelUpdateMode.Conditional;

                up.ContentTemplateContainer.Controls.Add(lkbAgregar);
                //Definiendo trigger sincrono para el link contenido en el panel
                AsyncPostBackTrigger trigger = new AsyncPostBackTrigger();
                trigger.ControlID = gvRecursosDisponibles.ID;
                up.Triggers.Add(trigger);
                //Añadiendo controles (UpdatePanel y LinkButton) a la celda
                celda.Controls.Add(up);
                //Añadiendo celda al grid
                fila.Cells.Add(celda);
            }
            //Si es celda encabezado
            else if (fila.RowType == DataControlRowType.Header)
            {
                try
                {   //Estableciendo estilo de celda
                    celda.CssClass = "EncabezadoGridViewCSS";
                }
                catch (NullReferenceException)
                {   //Si el estilo no fue definido no se establece
                }
                //Añadiendo celda al grid
                fila.Cells.Add(celda);
            }
            //Si es celda pie
            else
            {
                try
                {   //Estableciendo estilo de celda
                    celda.CssClass = "PieGridViewCSS";
                }
                catch (NullReferenceException)
                {   //Si el estilo no fue definido no se establece
                }
                //Añadiendo celda al grid
                fila.Cells.Add(celda);
            }
        }

        /// <summary>
        /// Método Privado encargado de Insertar el link Liberar en las filas
        /// </summary>
        /// <param name="fila"></param>
        private void insertaCeldaLiberar(GridViewRow fila)
        {   //Declarando variables a utilizar
            TableCell celda = new TableCell();
            ///////Determinando el tipo de celda///////
            //Si es celda de datos
            if (fila.RowType == DataControlRowType.DataRow)
            {
                //Instanciando nuevo LinkButton
                LinkButton lkbLiberar = new LinkButton();
                //Asignando Id de control
                lkbLiberar.ID = "lkbLiberar";
                //Estableciendo texto
                lkbLiberar.Text = "Liberar";
                //Estableciendo estilo
                lkbLiberar.CssClass = "LinkButton";
                //Estableciendo evento click
                lkbLiberar.Click += lkbLiberarRecurso_Click;
                //Quitando validacion
                lkbLiberar.CausesValidation = false;

                //Instanciando nuevo update panel
                UpdatePanel up = new UpdatePanel();
                up.UpdateMode = UpdatePanelUpdateMode.Conditional;

                up.ContentTemplateContainer.Controls.Add(lkbLiberar);
                //Definiendo trigger sincrono para el link contenido en el panel
                AsyncPostBackTrigger trigger = new AsyncPostBackTrigger();
                trigger.ControlID = gvRecursosDisponibles.ID;
                up.Triggers.Add(trigger);
                //Añadiendo controles (UpdatePanel y LinkButton) a la celda
                celda.Controls.Add(up);
                //Añadiendo celda al grid
                fila.Cells.Add(celda);
            }
            //Si es celda encabezado
            else if (fila.RowType == DataControlRowType.Header)
            {
                try
                {   //Estableciendo estilo de celda
                    celda.CssClass = "EncabezadoGridViewCSS";
                }
                catch (NullReferenceException)
                {   //Si el estilo no fue definido no se establece
                }
                //Añadiendo celda al grid
                fila.Cells.Add(celda);
            }
            //Si es celda pie
            else
            {
                try
                {   //Estableciendo estilo de celda
                    celda.CssClass = "PieGridViewCSS";
                }
                catch (NullReferenceException)
                {   //Si el estilo no fue definido no se establece
                }
                //Añadiendo celda al grid
                fila.Cells.Add(celda);
            }
        }
        /// <summary>
        /// Método Privado encargado de Insertar el link Reubicacion en el Grid View Recursos Disponibles
        /// </summary>
        /// <param name="fila"></param>
        private void insertaCeldaReubicacion(GridViewRow fila)
        {   //Declarando variables a utilizar
            TableCell celda = new TableCell();
            ///////Determinando el tipo de celda///////
            //Si es celda de datos
            if (fila.RowType == DataControlRowType.DataRow)
            {
                //Instanciando nuevo LinkButton
                LinkButton lkbReubicacion = new LinkButton();
                //Asignando Id de control
                lkbReubicacion.ID = "lkbReubicacion";
                //Estableciendo texto
                lkbReubicacion.Text = "Reubicación";
                //Estableciendo estilo
                lkbReubicacion.CssClass = "LinkButton";
                //Estableciendo evento Click
                lkbReubicacion.Click += lkbReubicarRecurso_Click;
                //Quitando validacion
                lkbReubicacion.CausesValidation = false;

                //Instanciando nuevo update panel
                UpdatePanel up = new UpdatePanel();
                up.UpdateMode = UpdatePanelUpdateMode.Conditional;
                up.ContentTemplateContainer.Controls.Add(lkbReubicacion);
                //Definiendo trigger sincrono para el link contenido en el panel
                AsyncPostBackTrigger trigger = new AsyncPostBackTrigger();
                trigger.ControlID = gvRecursosAsignados.ID;
                up.Triggers.Add(trigger);
                //Añadiendo controles (UpdatePanel y LinkButton) a la celda
                celda.Controls.Add(up);
                //Añadiendo celda al grid
                fila.Cells.Add(celda);
            }
            //Si es celda encabezado
            else if (fila.RowType == DataControlRowType.Header)
            {
                try
                {   //Estableciendo estilo de celda
                    celda.CssClass = "gridviewheader";
                }
                catch (NullReferenceException)
                {   //Si el estilo no fue definido no se establece
                }
                //Añadiendo celda al grid
                fila.Cells.Add(celda);
            }
            //Si es celda pie
            else
            {
                try
                {   //Estableciendo estilo de celda
                    celda.CssClass = "gridviewfooter";
                }
                catch (NullReferenceException)
                {   //Si el estilo no fue definido no se establece
                }
                //Añadiendo celda al grid
                fila.Cells.Add(celda);
            }
        }
        /// <summary>
        /// Carga Operadores
        /// </summary>
        private void cargaOperadores()
        {
            //Declaramos variable para consultar la unidades de acuerdo al control chkUbicacionActual
            int id_ubicacion_actual = 0;
            int id_ubicacion_diferente = 0;

            //En caso de mostrar unidades con la misma ubicación
            if (chkUbicacionActual.Checked)
            {
                //Asignamos Id de la Ubicación Actual
                id_ubicacion_actual = this._id_ubicacion;
            }
            else
            {
                //Asignamos Id de la Ubicación Diferente a la ctual
                id_ubicacion_diferente = this._id_ubicacion;
            }

            //Obtenemos Operadores
            using (DataTable mit = Operador.CargaOperadoresParaAsignacionEnDespacho(this._id_compania, txtValor.Text, id_ubicacion_actual, (byte)id_ubicacion_diferente))
            {
                //Cargamos Grid View
                TSDK.ASP.Controles.CargaGridView(gvRecursosDisponibles, mit, "Id", "", false, 0);

                //Valida Origen de Datos
                if (mit != null)
                {
                    //Añadiendo Tabla a DataSet de Session
                    this._ds = TSDK.Datos.OrigenDatos.AñadeTablaDataSet(this._ds, mit, "Table");
                }
                else
                {
                    //Eliminamos Tabla de la Session
                    this._ds = OrigenDatos.EliminaTablaDataSet(this._ds, "Table");
                }
            }
        }
        /// <summary>
        /// Carga Terceros
        /// </summary>
        private void cargaTerceros()
        {
            //Obtenemos Terceros
            using (DataTable mit = SAT_CL.Global.CompaniaEmisorReceptor.CargaTercerosParaAsignacionEnDespacho(this._id_compania, txtValor.Text))
            {
                //Cargamos Grid View
                Controles.CargaGridView(gvRecursosDisponibles, mit, "Id", "", false, 0);
                //Valida Origen de Datos
                if (mit != null)
                {
                    //Añadiendo Tabla a DataSet de Session
                    this._ds = TSDK.Datos.OrigenDatos.AñadeTablaDataSet(this._ds, mit, "Table");
                }
                else
                {
                    //Eliminamos Tabla de la Session
                    this._ds = OrigenDatos.EliminaTablaDataSet(this._ds, "Table");
                }
            }
        }
        /// <summary>
        /// Carga Unidades
        /// </summary>
        private void cargaUnidades()
        {
            //Declaramos variable para consultar la unidades de acuerdo al control chkUbicacionActual
            int id_ubicacion_actual = 0;
            int id_ubicacion_diferente = 0;

            //En caso de mostrar unidades con la misma ubicación
            if (chkUbicacionActual.Checked)
            {
                //Asignamos Id de la Ubicación Actual
                id_ubicacion_actual = this._id_ubicacion;
            }
            else
            {
                //Asignamos Id de la Ubicación Diferente a la ctual
                id_ubicacion_diferente = this._id_ubicacion;
            }

            //Obtenemos Unidades
            using (DataTable mit = Unidad.CargaUnidadesParaAsignacionEnDespacho(this._id_compania, txtValor.Text,
               id_ubicacion_actual, id_ubicacion_diferente))
            {
                //Cargamos Grid View
                Controles.CargaGridView(gvRecursosDisponibles, mit, "Id", "", false, 0);
                //Valida Origen de Datos
                if (mit != null)
                {
                    //Añadiendo Tabla a DataSet de Session
                    this._ds = TSDK.Datos.OrigenDatos.AñadeTablaDataSet(this._ds, mit, "Table");
                }
                else
                {
                    //Eliminamos Tabla de la Session
                    this._ds = OrigenDatos.EliminaTablaDataSet(this._ds, "Table");
                }
            }
        }
        /// <summary>
        /// Carga Recursos Disponibles
        /// </summary>
        private void cargaRecursosDiponibles()
        {
            //Limpiando control de ordenamiento
            lblOrdenarRecursosDisponibles.Text = "";
            uplblOrdenarRecursosDisponibles.Update();

            //De acuerdo al Tipo de Asignación
            switch (ddlTipoAsignacion.SelectedValue)
            {
                //Unidad
                case "1":
                    cargaUnidades();
                    break;
                //Operador
                case "2":
                    cargaOperadores();
                    break;
                //Tercero
                case "3":
                    cargaTerceros();
                    break;
            }
        }
        private void inicializaAnticiposProgramados(int id_servicio, int id_deposito)
        {
            //Construyendo URL 
            string url = Cadena.RutaRelativaAAbsoluta("~/Operacion/Despacho.aspx", "~/Documentacion/AnticiposProgramados.aspx?idReg1=" + id_servicio + "&idReg2=" + id_deposito);           
            //Abriendo Nueva Ventana
            Response.Redirect(url);
        }

        #endregion

        #region Métodos Recursos Asignados

        /// <summary>
        /// Realiza la carga de los Recursos Asignados 
        /// </summary>
        private void cargaRecursosAsignados()
        {
            //Obteniendo los Terceros
            using (DataTable dt = SAT_CL.Despacho.MovimientoAsignacionRecurso.CargaMovimientosAsignacionParaVisualizacion(this._id_movimiento))
            {
                //Cargando GridView 
                Controles.CargaGridView(gvRecursosAsignados, dt, "Id-IdRecurso", "", false, 0);
                //Validando que la Tabla Contenga Registros
                if (dt != null)
                {
                    //Añadiendo Tabla a DataSet de Session
                    this._ds = TSDK.Datos.OrigenDatos.AñadeTablaDataSet(this._ds, dt, "Table1");
                }
                else
                {
                    //Borrando de sesión los viajes cargados anteriormente
                    this._ds = OrigenDatos.EliminaTablaDataSet(this._ds, "Table1");
                }
            }
        }
        /// <summary>
        /// Método encargado de asignar los recursos
        /// </summary>
        /// <param name="tipo">Tipo Asignación (Unidad, Operador, Tercero)</param>
        /// <param name="tipo_unidad">Tipo Unidad (Tractor, Remolque, Dolly)</param>
        private RetornoOperacion asignaRecurso(MovimientoAsignacionRecurso.Tipo tipo, int tipo_unidad)
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Realizando la asignación del Recuros 
            resultado = MovimientoAsignacionRecurso.InsertaMovimientoAsignacionRecursoParaDespacho(this._id_compania,
                       this._id_movimiento, this._id_origen, EstanciaUnidad.Tipo.Operativa, EstanciaUnidad.TipoActualizacionInicio.Manual, tipo, tipo_unidad, Convert.ToInt32(gvRecursosDisponibles.SelectedValue),
                       ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

            //Si no existe ningún error
            if (resultado.OperacionExitosa)
            {
                //Cargamos Recursos Asignados
                cargaRecursosAsignados();

                //Carga Recursos Disponible
                cargaRecursosDiponibles();

                //Inicializamos Grid View 
                Controles.InicializaIndices(gvRecursosDisponibles);

            }

            //Mostramos Mensaje Error
            lblErrorRecursosAsignados.Text = resultado.Mensaje;

            //Devolviendo Resultado Obtenido
            return resultado;
        }

        #endregion

        #region Métodos Depositos

        /// <summary>
        /// Muestra el valor de la asignación de recurso activa en el movimiento
        /// </summary>
        /// <param name="id_operador"></param>
        /// <param name="id_unidad"></param>
        /// <param name="id_tercero"></param>
        private void inicializaAsignacionOtroProveedor(int id_operador, int id_unidad, int id_tercero)
        {
            //Quitando marca de otro proveedor
            chkOtroProveedor.Checked = false;
            //Limpiando caja de texto
            txtOtroProveedor.Text = "";

            //Cargando asignación activa en etiqueta
            cargaDatosAsignacionMovimiento(id_operador, id_unidad, id_tercero);

            //Asignando vista predeterminada
            mtvAsignacionActivaDeposito.SetActiveView(vwAsignacionMovimiento);
        }
        /// <summary>
        /// Realiza la carga de la información principal de la asignación de recursos de movimiento seleccionada para añadir anticipos o diesel
        /// </summary>
        /// <param name="id_unidad"></param>
        /// <param name="id_operador"></param>
        /// <param name="id_proveedor_compania"></param>
        private void cargaDatosAsignacionMovimiento(int id_unidad, int id_operador, int id_proveedor_compania)
        {
            //Determinando el tipo de asignación
            if (id_unidad > 0)
            {
                //Instanciando recurso
                using (Unidad u = new Unidad(id_unidad))
                    lblAsignacionMovimiento.Text = string.Format("Unidad '{0}'", u.numero_unidad);
            }
            else if (id_operador > 0)
            {
                //Instanciando recurso
                using (Operador o = new Operador(id_operador))
                    lblAsignacionMovimiento.Text = string.Format("Operador '{0}'", o.nombre);
            }
            else
            {
                //Instanciando recurso
                using (CompaniaEmisorReceptor c = new CompaniaEmisorReceptor(id_proveedor_compania))
                    lblAsignacionMovimiento.Text = string.Format("Proveedor '{0}'", c.nombre);
            }
        }

        /// <summary>
        /// Carga Anticipos (Vales de Diesel y Depósitos)
        /// </summary>
        /// <param name="id_operador">Id Operador</param>
        /// <param name="id_unidad">Id Unidad</param>
        /// <param name="id_proveedor_compania">Id Proveedor Compania</param>
        private void cargaAnticiposEntidad(int id_operador, int id_unidad, int id_proveedor_compania)
        {
            //Cargando asignación predeterminada se recurso
            cargaDatosAsignacionMovimiento(id_unidad, id_operador, id_proveedor_compania);

            //Instanciando movimiento asociado
            using (Movimiento mov = new Movimiento(this._id_movimiento))
            {
                //Obteniendo depósitos y vales del viaje o movimiento vacío 
                using (DataTable dt = mov.id_servicio > 0 ? SAT_CL.EgresoServicio.Reportes.CargaAnticiposRecursoServicio(id_operador, id_unidad, id_proveedor_compania, mov.id_servicio)
                                                            : SAT_CL.EgresoServicio.Reportes.CargaAnticiposRecursoMovimiento(id_operador, id_unidad, id_proveedor_compania, mov.id_movimiento))
                {
                    //Inicializando indices gridView
                    Controles.InicializaIndices(gvAnticipos);
                    //Cargando GridView de Viajes
                    Controles.CargaGridView(gvAnticipos, dt, "Id-Tipo-Programado", lblOrdenarAnticipos.Text, true, 9);
                    //Validando que el datatable no sea null
                    if (dt != null)
                    {
                        //Añadiendo Tabla a DataSet de Session
                        this._ds = TSDK.Datos.OrigenDatos.AñadeTablaDataSet(this._ds, dt, "Table3");
                    }
                    else
                    {
                        //Borrando de sesión los viajes cargados anteriormente
                        this._ds = OrigenDatos.EliminaTablaDataSet(this._ds, "Table3");
                    }
                }
            }
        }

        #endregion

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método encargado de Inicializar las Asignaciones de los Recursos
        /// </summary>
        /// <param name="idMovimiento">Movimiento</param>
        /// <param name="idParada">Parada</param>
        /// <param name="idCompania">Compania Emisora</param>
        public void InicializaAsignacionRecurso(int idMovimiento, int idParada, int idCompania)
        {
            //Asignando Atributos
            this._id_movimiento = idMovimiento;
            this._id_compania = idCompania;
            this._id_origen = idParada;

            //Inicializando Control
            inicializaControl();
        }
        /// <summary>
        /// Deshabilita Recurso
        /// </summary>
        public RetornoOperacion DeshabilitaRecurso()
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Instanciando al registro activo en sesión
            using (MovimientoAsignacionRecurso r = new MovimientoAsignacionRecurso(Convert.ToInt32(gvRecursosAsignados.SelectedValue)))
            {
                //Realizando la deshabilitación
                resultado = r.CancelaMovimientoAsignacionRecursoParaDespacho(EstanciaUnidad.TipoActualizacionFin.Manual, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_usuario);
            }

            //Si no existe ningún error
            if (resultado.OperacionExitosa)
            {
                //Cargamos Recursos Asignados
                cargaRecursosAsignados();

                //Carga Recursos Disponible
                cargaRecursosDiponibles();

                //Actualizando lista de unidades 
                upgvRecursosAsignados.Update();
                upgvRecursosDisponibles.Update();

                //Inicializamos Indices
                Controles.InicializaIndices(gvRecursosAsignados);
            }

            //Mostramos Mensaje Error
            lblErrorRecursosAsignados.Text = resultado.Mensaje;

            //Devolviendo Resultado Obtenido
            return resultado;
        }
        /// <summary>
        /// Método encargado de Agregar el Recurso al Movimiento
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion AgregaAsignacionRecurso()
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Declaramos Variable
            MovimientoAsignacionRecurso.Tipo tipo = MovimientoAsignacionRecurso.Tipo.Operador;
            int tipo_unidad = 0;

            //Obtenemos el Tipo de Unidad
            obtieneTipoRecurso(out tipo, out tipo_unidad);

            //Asignando recurso
            result = asignaRecurso(tipo, tipo_unidad);
            //Actualizando lista de unidades 
            upgvRecursosAsignados.Update();
            upgvRecursosDisponibles.Update();

            //Devolviendo Objeto de Retorno
            return result;
        }

        /// <summary>
        /// Método encargado de la Liberación de las Unidades y Operadores
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion LiberarRecurso()
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();
            //De acuerdo al Tipo de Recurso
            switch ((MovimientoAsignacionRecurso.Tipo)this._id_tipo_asignacion)
            {
                //Unidad
                case MovimientoAsignacionRecurso.Tipo.Unidad:
                    //Instanciamos Unidad
                    using (Unidad objUnidad = new Unidad(Convert.ToInt32(gvRecursosDisponibles.SelectedValue)))
                    {
                        //Liberamos Unidad
                        resultado = objUnidad.LiberarUnidad(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                    }
                    break;
                //Operador
                case MovimientoAsignacionRecurso.Tipo.Operador:
                    //Instanciamos Operador
                    using (Operador objOperador = new Operador(Convert.ToInt32(gvRecursosDisponibles.SelectedValue)))
                    {
                        //Liberamos Operador
                        resultado = objOperador.LiberarOperador(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                    }
                    break;

            }
            //Validamos Resultado
            if (resultado.OperacionExitosa)
            {

                //Inicializa Indices
                Controles.InicializaIndices(gvRecursosDisponibles);
                //Cargamos Recursos Disponibles
                cargaRecursosDiponibles();
                //Carga Asignaciones
                cargaRecursosAsignados();
                //Actualizando lista de unidades 
                upgvRecursosAsignados.Update();
                upgvRecursosDisponibles.Update();
            }
            //Mostramos Mensaje
            lblErrorRecursosAsignados.Text = resultado.Mensaje;

            //Devolvemos Objeto retorno
            return resultado;
        }

        #endregion

        #endregion

        
    }
}