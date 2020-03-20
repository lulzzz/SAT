using System;
using System.Data;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;

namespace SAT.SinLogin
{
    public partial class Calificacion : System.Web.UI.UserControl
    {
        #region Atributos
        /// <summary>
        /// Atributo que almacena las calificaciones asignadas por el evaluador
        /// </summary>
        private DataTable _mit_CalificacionDetalle;
        /// <summary>
        /// Atributo que almacena el identificador del contacto que realizo la evaluación
        /// </summary>
        private int _id_contacto;
        /// <summary>
        /// Atributo que almacena el identificador del usuario que realizo la evaluación
        /// </summary>
        private int _id_usuario;
        /// <summary>
        /// Atributo que almacena el identificador de la calificación general de la entidad
        /// </summary>
        private int _id_calificacion;
        /// <summary>
        /// Atributoq ue almacena la entidad a calificar (Cliente, Operador o Transportista)
        /// </summary>
        private int _id_tabla;
        /// <summary>
        /// Atributo que almacena el identificador de quien se va a calificar 
        /// </summary>
        private int _id_registro;
        /// <summary>
        /// Permite obtener el valor de tabulación de cada control.
        /// </summary>
        public short TabIndex
        {
            //Asigna a los controles del control de usuario el valor de values 
            set
            {
                ddlConcepto.TabIndex =
                imgbtnAgregarConcepto.TabIndex =
                ddlTamañoGridViewConceptosCalificacion.TabIndex =
                lkbExportarExcelConceptosCalificacion.TabIndex =
                btnGuardar.TabIndex =
                txtComenatrio.TabIndex = 
                gvConceptosCalificacion.TabIndex = value;                
            }
            //obtiene el primer valor del primer control de usuario
            get { return ddlConcepto.TabIndex; }
        }
        /// <summary>
        /// Permite obtener el valor de disponibilidad de los controles (true/false).
        /// </summary>
        public bool Enabled
        {
            //Asigna un valor a los controles del control de usuario
            set
            {
                ddlConcepto.Enabled =
                imgbtnAgregarConcepto.Enabled =
                ddlTamañoGridViewConceptosCalificacion.Enabled =
                lkbExportarExcelConceptosCalificacion.Enabled =
                btnGuardar.Enabled = 
                txtComenatrio.Enabled = 
                gvConceptosCalificacion.Enabled = value;
            }
            //Obtiene el valor del primer control de usuario
            get { return this.ddlConcepto.Enabled; }
        }
        #endregion

        #region Manejadores de Eventos
        /// <summary>
        /// Creación del evento ClickGuardarCalificacionGeneral
        /// </summary>
        public event EventHandler ClickGuardarCalificacionGeneral;
        /// <summary>
        /// Delegado del evento ClickGuardarCalificacionGeneral que permite que las clases derivadas invoquen al método OnClickGuardarCalificacionGeneral y generen el evento ClickGuardarCalificacionGeneral.
        /// </summary>
        /// <param name="e">Contiene datos del evento</param>
        public virtual void OnClickGuardarCalificacionGeneral(EventArgs e)
        {
            if (ClickGuardarCalificacionGeneral != null)
                ClickGuardarCalificacionGeneral(this, e);
        }
        #endregion

        #region Eventos
        /// <summary>
        /// Evento que permite determinar el inicio del control de usuario Calificacion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Valida si la página se a cargado por primera vez.
            if (!(Page.IsPostBack))
            {
                //Inicializa el grid view que alamcena  las calificaciones por concepto
                inicializaCalificacionDetalle();                
            }
                
            else
                //Invoca al método recuperaAtrubutos
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
        /// <summary>
        /// Evento que se ejecuta cuando se quiere agregar un detalle de calificación
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imgbtnAgregarConcepto_Click(object sender, EventArgs e)
        {
            //Invoca al método que guarda la califición por concepto 
            GuardaDetallesCalificacion();
        }

        /// <summary>
        /// Evento del gridView que modifica la presentación de la columna Calificación por una imagen acorde al valor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvConceptosCalificacion_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Valida los datos de 
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Almacena en la variable idcalificacion los datos que contenga la columna 3 (Calificacion asignada por concepto)
                string idCalificacion = ((DataRowView)e.Row.DataItem).Row.ItemArray[2].ToString();

                //Almacena en una variable el contenido actual del gridview 
                var image = e.Row.FindControl("imgCalificacion") as Image;
                //Si es diferente de nulo
                if (image != null)
                {
                    //Asigna una URL al control del gridView
                    switch(idCalificacion)
                    {
                        case "0":
                            {
                                image.ImageUrl = "~/Image/EstrellaH.png";
                                break;
                            }
                        case "1":
                            {

                                image.ImageUrl = "~/Image/EstrellaH1.png";
                                break;
                            }
                            
                        
                        case "2":
                            {
                                image.ImageUrl = "~/Image/EstrellaH2.png";
                                break;
                            }
                        case "3":
                            {
                                image.ImageUrl = "~/Image/EstrellaH3.png";
                                break;
                            }
                        case "4":
                            {
                                image.ImageUrl = "~/Image/EstrellaH4.png";
                                break;
                            }
                        case "5":
                            {
                                image.ImageUrl = "~/Image/EstrellaH5.png";
                                break;
                            }
                    }
                }
            }   
        }
        /// <summary>
        /// Evento que elimina calificaciones detalle de una entidad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEliminar_Click(object sender, EventArgs e)
        {
            //Creacion del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
                //Valida que esictan registros en el gridView
                if (gvConceptosCalificacion.DataKeys.Count > 0)
                {
                    //Selecciona el registro que se va a editar
                    Controles.SeleccionaFila(gvConceptosCalificacion, sender, "lnk", false);
                    //Instancia a la clase calificacion detalle
                    using (SAT_CL.Calificacion.CalificacionDetalle cdet = new SAT_CL.Calificacion.CalificacionDetalle(Convert.ToInt32(gvConceptosCalificacion.SelectedValue)))
                    {
                        //Elimina el concepto evaluado
                        retorno = cdet.DeshabilitarCalificacionDetalle(this._id_contacto, this._id_usuario);
                    }
                }    
 
            //Valida la acción de eliminar
            if (retorno.OperacionExitosa)

                //Invoca al método que inicializa el CU
                inicializaValores(this._id_tabla, this._id_registro, this._id_contacto, this._id_usuario,this._id_calificacion,true);
            //Envia mensaje 
            TSDK.ASP.ScriptServer.MuestraNotificacion(gvConceptosCalificacion, retorno, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Evento que actualiza la calificación general de la entidad acorde a los conceptos evaluados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (ClickGuardarCalificacionGeneral != null)
                OnClickGuardarCalificacionGeneral(new EventArgs());
        }
        
        #region Eventos GridView
        /// <summary>
        /// Evento producido al cambiar el indice de página del GV de Eventos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvConceptosCalificacion_OnpageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Permite cambiar de indice de pagina acorde al tamaño del gridview
            Controles.CambiaIndicePaginaGridView(gvConceptosCalificacion, this._mit_CalificacionDetalle, e.NewPageIndex, true, 2);
        }
        /// <summary>
        /// Evento producido al cambiar el tamaño de página del GV de Eventos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvConceptosCalificacion_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambia el rango de valores visibles en el gridview (25-50-75-100 registros por vista)
            Controles.CambiaTamañoPaginaGridView(gvConceptosCalificacion,this. _mit_CalificacionDetalle, Convert.ToInt32(ddlTamañoGridViewConceptosCalificacion.SelectedValue), true, 2);
        }
        /// <summary>
        /// Eventó producido al pulsar el botón de exportación de contenido de GV de Eventos a excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarExcelConceptosCalificacion_Onclick(object sender, EventArgs e)
        {
            //Invoca al metodoq ue permite exportar el gridview a formato de excel.
            Controles.ExportaContenidoGridView(this._mit_CalificacionDetalle, "Id","IdCalificacion");
        }
        /// <summary>
        /// Evento producido al cambiar el criterio de orden del GV de Eventos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvConceptosCalificacion_Onsorting(object sender, GridViewSortEventArgs e)
        {
            //Permite el ordenamiento de las columnas de manera ascendente o descendente
            lblCriterioGridViewConceptosCalificacion.Text = Controles.CambiaSortExpressionGridView(gvConceptosCalificacion, this._mit_CalificacionDetalle, e.SortExpression, true, 2);
        }
        #endregion

        #region Eventos Historial Calificaciones
        /// <summary>
        /// Abre la ventana modal de Historial calificaciones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCantidadComentarios_Click(object sender, EventArgs e)
        {
            //Inicializa el control Historial Clificación
            wucHistorial.InicializaControl(this._id_tabla, this._id_registro);
            //Abre la ventana Historial Calificación
            ScriptServer.AlternarVentana(lkbCantidadComentarios, "ventanaHistorialCalificacion", "contenedorHistorialCalificacion", "HistorialCalificacion");
        }
        /// <summary>
        /// Cierra la Ventana Modal de HistorialCalificaciones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkCerrarVentanaHistorialCalificacion_Click(object sender, EventArgs e)
        {
            //Cierra la ventana modal HistorialCalificacion
            ScriptServer.AlternarVentana(lnkCerrarVentanaHistorialCalificacion, lnkCerrarVentanaHistorialCalificacion.GetType(), "CerrarVetana", "contenedorHistorialCalificacion", "HistorialCalificacion");
        }
        #endregion
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Inicializa el gridView
        /// </summary>
        private void inicializaCalificacionDetalle()
        {
            //Inicializa el indice del gridView
            Controles.InicializaIndices(gvConceptosCalificacion);
            //Instancia al método que obtiene los conceptos evaluados a una entidad
            using (DataTable mit = SAT_CL.Calificacion.CalificacionDetalle.ObtieneCalificacionDetalle(this._id_calificacion,this._id_tabla,this._id_registro))
            {
                //Carga el gridView de valores
                Controles.CargaGridView(gvConceptosCalificacion, mit, "Id", "", true, 2);
                //ASigna al atributo los valores de la tabla
                this._mit_CalificacionDetalle = mit;
            }
        }

        /// <summary>
        /// Método que almacena los valores de la página(id_calificacion)
        /// </summary>
        private void asignaAtributos()
        {
            //Almacena en la variable viewState  el valor del atributos de la clase
            ViewState["idCalificacion"] = this._id_calificacion;
            ViewState["idTabla"] = this._id_tabla;
            ViewState["idRegistro"] = this._id_registro;
            ViewState["idContacto"] = this._id_contacto;
            ViewState["idUsuario"] = this._id_usuario;
            ViewState["mitConceptoCalificacion"] = this._mit_CalificacionDetalle;
        }
        /// <summary>
        /// Método que consulta o estrae los valores de la página. 
        /// </summary>   
        private void recuperaAtributos()
        {
            //Valida el valor de la variable ViewState idCalificacion(que sea diferente de 0)
            if (Convert.ToInt32(ViewState["idCalificacion"]) != 0)
                //Si cumple la condición, asigna al atributo _id_calificacion el valor de la variable ViewState idCalificacion
                this._id_calificacion = Convert.ToInt32(ViewState["idCalificacion"]);
            //Valida el valor de la variable ViewState idTabla(que sea diferente de 0)
            if (Convert.ToInt32(ViewState["idTabla"]) != 0)
                //Si cumple la condición, asigna al atributo _id_tabla el valor de la variable ViewState idTabla
                this._id_tabla = Convert.ToInt32(ViewState["idTabla"]);
            //Valida el valor de la variable Viewstate idRegistro (que sea diferente a 0)
            if (Convert.ToInt32(ViewState["idRegistro"]) != 0)
                //Si cumple la condición, asigna al atributo _id_registro el valor de la variable ViewState idRegistro
                this._id_registro = Convert.ToInt32(ViewState["idRegistro"]);
            //Valida el valor de la variable Viewstate idConcepto (que sea diferente a 0)
            if ((DataTable)ViewState["mitConceptoCalificacion"] != null)
                //Si cumple la condición, asigna al atributo _mitConceptoCalificacion el valor de la variable ViewState mitConceptoCalificacion
                this._mit_CalificacionDetalle = (DataTable)ViewState["mitConceptoCalificacion"];
            //Valida el valor de la variable Viewstate idConcepto (que sea diferente a 0)
            if (Convert.ToInt32(ViewState["idContacto"]) != 0)
                //Si cumple la condición, asigna al atributo _mitConceptoCalificacion el valor de la variable ViewState mitConceptoCalificacion
                this._id_contacto = (int)ViewState["idContacto"];
            //Valida el valor de la variable Viewstate idConcepto (que sea diferente a 0)
            if (Convert.ToInt32(ViewState["idUsuario"]) != 0)
                //Si cumple la condición, asigna al atributo _mitConceptoCalificacion el valor de la variable ViewState mitConceptoCalificacion
                this._id_usuario = (int)ViewState["idUsuario"];
        }
        /// <summary>
        /// Evento que inicializa los
        /// </summary>
        /// <param name="id_tabla"></param>
        /// <param name="id_registro"></param>
        /// <param name="id_contacto"></param>
        /// <param name="id_usuario"></param>
        private void inicializaValores(int id_tabla, int id_registro, int id_contacto, int id_usuario, int id_calificacion, bool controles)
        {
            //Asigna a los atributos los valores de los parametros 
            this._id_tabla = id_tabla;
            this._id_registro = id_registro;
            this._id_contacto = id_contacto;
            this._id_usuario = id_usuario;
            this._id_calificacion = id_calificacion;
            //ACORDE A LA TABLA OBTIENE LOS DATOS DE LA ENTIDAD A EVALUAR.
            switch (this._id_tabla)
            {
                //TABLA SERVICIO
                case 1:
                    {
                        using (SAT_CL.Documentacion.Servicio ser = new SAT_CL.Documentacion.Servicio(this._id_registro))
                        {
                            lblEntidad.Text ="N°. SERVICIO: "+ ser.no_servicio;
                        }
                        break;
                    }
                 // COMPAÑIA
                case 25:
                    {
                        using (SAT_CL.Global.CompaniaEmisorReceptor com = new SAT_CL.Global.CompaniaEmisorReceptor(this._id_registro))
                            lblEntidad.Text = com.nombre;
                        break;
                    }
                //OPERADOR
                case 76:
                    {
                        using (SAT_CL.Global.Operador op = new SAT_CL.Global.Operador(this._id_registro))
                            lblEntidad.Text = op.nombre;
                        break;
                    }
            }
            //Invoca al constructor de la clase unidad para obtener el id y el numero de la unidad
            byte Entidad = SAT_CL.Calificacion.Calificacion.ObtieneEntidad(this._id_tabla, this._id_registro);
            int CantidadComentarios = SAT_CL.Calificacion.Calificacion.ObtieneNumeroComentarios(this._id_tabla, this._id_registro);      
            //Valida el valor de la variable totalEstrellas y asigna una ubicación de una imagen a mostrar
            switch (Entidad)
                {
                    case 0:
                        {
                            imgHistorial.ImageUrl = "~/Image/Estrella.png";
                            lkbCantidadComentarios.Text = "0 / 5" + " ( 0 Opiniones  )";
                            break;
                        }
                        case 1:
                            {
                                imgHistorial.ImageUrl = "~/Image/Estrella1.png";
                                lkbCantidadComentarios.Text = Entidad + " / 5" + " ( " + CantidadComentarios + " opiniones  )";
                                break;
                            }                    
                            case 2 :
                        {
                            imgHistorial.ImageUrl = "~/Image/Estrella2.png";
                            lkbCantidadComentarios.Text = Entidad + " / 5" + " ( " + CantidadComentarios + " opiniones  )";
                            break;
                        }
                            case 3 :
                        {
                            imgHistorial.ImageUrl = "~/Image/Estrella3.png";
                            lkbCantidadComentarios.Text = Entidad + " / 5" + " ( " + CantidadComentarios + " opiniones  )";
                            break;
                        }
                            case 4:
                        {
                            imgHistorial.ImageUrl = "~/Image/Estrella4.png";
                            lkbCantidadComentarios.Text = Entidad + " / 5" + " ( " + CantidadComentarios + " opiniones  )";
                            break;
                        }
                            case 5:
                        {
                            imgHistorial.ImageUrl = "~/Image/Estrella5.png";
                            lkbCantidadComentarios.Text = Entidad + " / 5" + " ( " + CantidadComentarios + " opiniones  )";
                            break;
                        }
                }                                                
            //Inicializa los valores del gridview.
            inicializaCalificacionDetalle();
                habilitaControles(controles);
        }
        /// <summary>
        /// Método que habilita o deshabilita los controles del control de usuario
        /// </summary>
        /// <param name="enable"></param>
        private void habilitaControles( bool enable)
        {
            ddlConcepto.Enabled =
            imgbtnAgregarConcepto.Enabled =
            btnGuardar.Enabled =
            txtComenatrio.Enabled = 
            gvConceptosCalificacion.Enabled = enable;
        }
        
        #endregion

        #region Métodos Públicos
        /// <summary>
        /// Método que inicializa los valores de la forma
        /// </summary>
        /// <param name="id_tabla"></param>
        /// <param name="id_registro"></param>
        /// <param name="id_contacto"></param>
        /// <param name="id_usuario"></param>
        public void InicializaControl(int id_tabla, int id_registro,int id_contacto, int id_usuario, int id_calificacion, bool controles)
        {
            this._id_tabla = id_tabla;
            this._id_registro = id_registro;
            this._id_contacto = id_contacto;
            this._id_usuario = id_usuario;
            this._id_calificacion = id_calificacion;
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlConcepto,98, "", 0, "", this._id_tabla, "");
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamañoGridViewConceptosCalificacion, "", 26);
            inicializaValores(this._id_tabla, this._id_registro, this._id_contacto, this._id_usuario,this._id_calificacion,controles);
            txtComenatrio.Text = "";
        }

        /// <summary>
        /// Método que almacen los detalles de calificacion de una entidad
        /// </summary>
        public void GuardaDetallesCalificacion()
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion(0);
            //Inserta una calificación (bloque transaccional)
            using(TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Valida si existe un rgistro de calificación
                if (this._id_calificacion == 0)
                {
                    //En caso de ser un usuario (id_contacto es igual a cero)
                    if (this._id_contacto == 0)
                    {
                        //Instancia a la clase seguridad y obtienen los datos del usuario
                        using (SAT_CL.Seguridad.Usuario us = new SAT_CL.Seguridad.Usuario(this._id_usuario))
                        {
                            //Inserta una Calificación con los datos del usuario
                            retorno = SAT_CL.Calificacion.Calificacion.InsertarCalificacion(this._id_tabla, this._id_registro, 0, "", us.email, 0, this._id_usuario);
                        }
                    }
                    //En caso Contrario (id_usuario es igual a cero)
                    else
                    {
                        //Instancia a la clase Contacto y obtiene los datos necesarios del contacto
                        using (SAT_CL.Global.Contacto con = new SAT_CL.Global.Contacto(this._id_contacto))
                        {
                            //Inserta una calificación con los datos del contacto
                            retorno = SAT_CL.Calificacion.Calificacion.InsertarCalificacion(this._id_tabla, this._id_registro, 0, "", con.email, this._id_contacto, 0);
                        }
                    }
                    //Asigna al atributo calificación el idRegistro de BD
                    this._id_calificacion = retorno.IdRegistro;
                }                  
                //Valida que se realizara correctamente la primera inserció o en su defecto que deje insertar conceptos de la misma entidad
                if(retorno.OperacionExitosa)
                {
                    //Declara la variable estrellas
                    byte estrellas = 0;
                    //ASigna a la variable lo el valor que el usuario elija por concepto
                    estrellas = Convert.ToByte(Request.Form["estrellas"]);
                    //Si la estrella es mayor a cero
                    if(estrellas > 0)
                    {
                        //Instancia a la clase Califición 
                        using(SAT_CL.Calificacion.Calificacion cal = new SAT_CL.Calificacion.Calificacion(this._id_calificacion))
                        {
                            //Valida que exista una calificación
                            if (cal.id_calificacion != 0)
                                    //Inserta un detalle de calificación con los datos del contacto
                                    retorno = SAT_CL.Calificacion.CalificacionDetalle.InsertarCalificacionDetalle(this._id_calificacion, Convert.ToInt32(ddlConcepto.SelectedValue), estrellas, this._id_contacto,this._id_usuario);                                                                                              
                        }
                    }
                    //En caso contrario
                    else
                    {
                        //ASigna mensaje de que no puede agregar una calificación en cero
                        retorno = new RetornoOperacion("La calificacion mínima es una estrella");
                        this._id_calificacion = 0;
                    }                                  
                }
                //TErmina el bloque transaccional
                if (retorno.OperacionExitosa)
                {
                    //termina la transacción
                    trans.Complete();
                }
            }
            //Valida que la operación 
            if (retorno.OperacionExitosa)            
                //Inicializa el UC
                this.inicializaValores(this._id_tabla, this._id_registro,this._id_contacto,this._id_usuario,this._id_calificacion,true);                                               
            //Muestra el mensaje de error
            TSDK.ASP.ScriptServer.MuestraNotificacion(this.imgbtnAgregarConcepto, retorno, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Método que actualiza el promedio calculado a partir de los conceptos evaluados
        /// </summary>
        public RetornoOperacion GuardarCalificacionGeneral()
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();           
            //Instancia a la clase calificación
            using (SAT_CL.Calificacion.Calificacion cal = new SAT_CL.Calificacion.Calificacion(this._id_calificacion))
            {
                    //Realiza la actualización de la calificación en general con los datos del contacto
                    retorno = cal.ActualizaComentario(txtComenatrio.Text, this._id_contacto, this._id_usuario);
            }
            //Valida la operación (que se realizo correctamente)
            if (retorno.OperacionExitosa)
            {
                //Inicializa el CU
                inicializaValores(this._id_tabla, this._id_registro, this._id_contacto, this._id_usuario,this._id_calificacion,false);
            }
            //Muestra el mensaje de error
            TSDK.ASP.ScriptServer.MuestraNotificacion(this.btnGuardar, retorno, ScriptServer.PosicionNotificacion.AbajoDerecha);
            return retorno;
        }

        #endregion


    }
}