using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using System.Data;

namespace SAT.Externa
{
    public partial class wucHistorialCalificacion : System.Web.UI.UserControl
    {
        #region Atributos
        /// <summary>
        /// Atributo que alamcena el historial de calificaciones que ha tenido una entidad (Operador, Servicio,Compañia, etc.)
        /// </summary>
        private DataTable _mit_HistorialCalificaciones;
        /// <summary>
        /// Atributo que almacena la entidad  calificada (Cliente, Operador o Transportista)
        /// </summary>
        private int _id_tabla;
        /// <summary>
        /// Atributo que almacena el identificador de quien se califico
        /// </summary>
        private int _id_registro;
        #endregion

        #region Eventos
        /// <summary>
        /// Evento que permite determinar el inicio del control de usuario Historial Calificacion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Valida si la página se a cargado por primera vez.
            if (!(Page.IsPostBack))
            {
                //Inicializa el grid view que alamcena  las calificaciones por concepto
                inicializaHistorialCalificaciones();
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

        protected void gvHistorialCalificacion_RowDataBound(object sender, GridViewRowEventArgs e)
        {                        
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //Recupera origen de datos de la fila
                    DataRow row = ((DataRowView)e.Row.DataItem).Row;
                    //Valida la tabla
                    switch (this._id_tabla)
                    {
                        case 76:
                            {
                                //Oculta las columnas concepto por servicio
                                this.gvHistorialCalificacion.Columns[9].Visible = false;
                                this.gvHistorialCalificacion.Columns[10].Visible = false;
                                //Instancia a los controles de tipo imagen en el gridview
                                using (Image imageAsistencia = (Image)e.Row.FindControl("imgAsistencia"),
                                            imageDisciplina = (Image)e.Row.FindControl("imgDisciplina"),
                                            imagePresentacion = (Image)e.Row.FindControl("imgPresentacion"),
                                            imagePuntualidad = (Image)e.Row.FindControl("imgPuntualidad"),
                                            imageRendimiento = (Image)e.Row.FindControl("imgRendimiento"),
                                            imageSeguridad = (Image)e.Row.FindControl("imgSeguridad")) 
                                {
                                    //Recorre las filas del grid view y asigna las imagenes correspondientesa la columna ASistencia
                                    switch (row["Asistencia"].ToString())
                                    {
                                        case "1":
                                            imageAsistencia.ImageUrl = "~/Image/EstrellaHC1.png";
                                            break;
                                        case "2":
                                            imageAsistencia.ImageUrl = "~/Image/EstrellaHC2.png";
                                            break;
                                        case "3":
                                            imageAsistencia.ImageUrl = "~/Image/EstrellaHC3.png";
                                            break;
                                        case "4":
                                            imageAsistencia.ImageUrl = "~/Image/EstrellaHC4.png";
                                            break;
                                        case "5":
                                            imageAsistencia.ImageUrl = "~/Image/EstrellaHC5.png";
                                            break;
                                        default:
                                            imageAsistencia.ImageUrl = "~/Image/EstrellaHC.png";
                                            break;
                                            
                                    }
                                    //Recorre las filas del grid view y asigna las imagenes correspondientesa la columna Disciplina
                                    switch (row["Disciplina"].ToString())
                                    {
                                        case "1":
                                            imageDisciplina.ImageUrl = "~/Image/EstrellaHC1.png";
                                            break;
                                        case "2":
                                            imageDisciplina.ImageUrl = "~/Image/EstrellaHC2.png";
                                            break;
                                        case "3":
                                            imageDisciplina.ImageUrl = "~/Image/EstrellaHC3.png";
                                            break;
                                        case "4":
                                            imageDisciplina.ImageUrl = "~/Image/EstrellaHC4.png";
                                            break;
                                        case "5":
                                            imageDisciplina.ImageUrl = "~/Image/EstrellaHC5.png";
                                            break;
                                        default:
                                            imageDisciplina.ImageUrl = "~/Image/EstrellaHC.png";
                                            break;
                                    }
                                    //Recorre las filas del grid view y asigna las imagenes correspondientesa la columna Presentacion
                                    switch (row["Presentación"].ToString())
                                    {
                                        case "1":
                                            imagePresentacion.ImageUrl = "~/Image/EstrellaHC1.png";
                                            break;
                                        case "2":
                                            imagePresentacion.ImageUrl = "~/Image/EstrellaHC2.png";
                                            break;
                                        case "3":
                                            imagePresentacion.ImageUrl = "~/Image/EstrellaHC3.png";
                                            break;
                                        case "4":
                                            imagePresentacion.ImageUrl = "~/Image/EstrellaHC4.png";
                                            break;
                                        case "5":
                                            imagePresentacion.ImageUrl = "~/Image/EstrellaHC5.png";
                                            break;
                                        default:
                                            imagePresentacion.ImageUrl = "~/Image/EstrellaHC.png";
                                            break;
                                    }
                                    //Recorre las filas del grid view y asigna las imagenes correspondientesa la columna Puntualidad
                                    switch (row["Puntualidad"].ToString())
                                    {
                                        case "1":
                                            imagePuntualidad.ImageUrl = "~/Image/EstrellaHC1.png";
                                            break;
                                        case "2":
                                            imagePuntualidad.ImageUrl = "~/Image/EstrellaHC2.png";
                                            break;
                                        case "3":
                                            imagePuntualidad.ImageUrl = "~/Image/EstrellaHC3.png";
                                            break;
                                        case "4":
                                            imagePuntualidad.ImageUrl = "~/Image/EstrellaHC4.png";
                                            break;
                                        case "5":
                                            imagePuntualidad.ImageUrl = "~/Image/EstrellaHC5.png";
                                            break;
                                        default:
                                            imagePuntualidad.ImageUrl = "~/Image/EstrellaHC.png";
                                            break;
                                    }
                                    //Recorre las filas del grid view y asigna las imagenes correspondientesa la columna Rendimiento
                                    switch (row["Rendimiento"].ToString())
                                    {
                                        case "1":
                                            imageRendimiento.ImageUrl = "~/Image/EstrellaHC1.png";
                                            break;
                                        case "2":
                                            imageRendimiento.ImageUrl = "~/Image/EstrellaHC2.png";
                                            break;
                                        case "3":
                                            imageRendimiento.ImageUrl = "~/Image/EstrellaHC3.png";
                                            break;
                                        case "4":
                                            imageRendimiento.ImageUrl = "~/Image/EstrellaHC4.png";
                                            break;
                                        case "5":
                                            imageRendimiento.ImageUrl = "~/Image/EstrellaHC5.png";
                                            break;
                                        default:
                                            imageRendimiento.ImageUrl = "~/Image/EstrellaHC.png";
                                            break;
                                    }
                                    //Recorre las filas del grid view y asigna las imagenes correspondientesa la columna Seguridad
                                    switch (row["Seguridad"].ToString())
                                    {
                                        case "1":
                                            imageSeguridad.ImageUrl = "~/Image/EstrellaHC1.png";
                                            break;
                                        case "2":
                                            imageSeguridad.ImageUrl = "~/Image/EstrellaHC2.png";
                                            break;
                                        case "3":
                                            imageSeguridad.ImageUrl = "~/Image/EstrellaHC3.png";
                                            break;
                                        case "4":
                                            imageSeguridad.ImageUrl = "~/Image/EstrellaHC4.png";
                                            break;
                                        case "5":
                                            imageSeguridad.ImageUrl = "~/Image/EstrellaHC5.png";
                                            break;
                                        default:
                                            imageSeguridad.ImageUrl = "~/Image/EstrellaHC.png";
                                            break;
                                    }
                                
                                }
                                this.gvHistorialCalificacion.Columns[9].Visible = false;
                                this.gvHistorialCalificacion.Columns[10].Visible = false;
                                break;
                            }
                        case 1:
                            {
                                using (Image imageCitasTiempo = (Image)e.Row.FindControl("imgCitasTiempo"),
                                             imageCuidadoProducto = (Image)e.Row.FindControl("imgCuidadoProducto"))
                                {
                                    //Recorre las filas del grid view y asigna las imagenes correspondientesa la columna ASistencia
                                    switch (row["CitasTiempo"].ToString())
                                    {
                                        case "1":
                                            imageCitasTiempo.ImageUrl = "~/Image/EstrellaHC1.png";
                                            break;
                                        case "2":
                                            imageCitasTiempo.ImageUrl = "~/Image/EstrellaHC2.png";
                                            break;
                                        case "3":
                                            imageCitasTiempo.ImageUrl = "~/Image/EstrellaHC3.png";
                                            break;
                                        case "4":
                                            imageCitasTiempo.ImageUrl = "~/Image/EstrellaHC4.png";
                                            break;
                                        case "5":
                                            imageCitasTiempo.ImageUrl = "~/Image/EstrellaHC5.png";
                                            break;
                                        default:
                                            imageCitasTiempo.ImageUrl = "~/Image/EstrellaHC.png";
                                            break;

                                    }
                                    //Recorre las filas del grid view y asigna las imagenes correspondientesa la columna Disciplina
                                    switch (row["CuidadoProducto"].ToString())
                                    {
                                        case "1":
                                            imageCuidadoProducto.ImageUrl = "~/Image/EstrellaHC1.png";
                                            break;
                                        case "2":
                                            imageCuidadoProducto.ImageUrl = "~/Image/EstrellaHC2.png";
                                            break;
                                        case "3":
                                            imageCuidadoProducto.ImageUrl = "~/Image/EstrellaHC3.png";
                                            break;
                                        case "4":
                                            imageCuidadoProducto.ImageUrl = "~/Image/EstrellaHC4.png";
                                            break;
                                        case "5":
                                            imageCuidadoProducto.ImageUrl = "~/Image/EstrellaHC5.png";
                                            break;
                                        default:
                                            imageCuidadoProducto.ImageUrl = "~/Image/EstrellaHC.png";
                                            break;
                                    }
                                }
                                this.gvHistorialCalificacion.Columns[3].Visible = false;
                                this.gvHistorialCalificacion.Columns[4].Visible = false;
                                this.gvHistorialCalificacion.Columns[5].Visible = false;
                                this.gvHistorialCalificacion.Columns[6].Visible = false;
                                this.gvHistorialCalificacion.Columns[7].Visible = false;
                                this.gvHistorialCalificacion.Columns[8].Visible = false;
                                break;
                            }
                    }
                }                      
        }
        
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Método que almacena los valores de la página
        /// </summary>
        private void asignaAtributos()
        {
            //Almacena en la variable viewState  el valor del atributos de la clase
            ViewState["idTabla"] = this._id_tabla;
            ViewState["idRegistro"] = this._id_registro;
            ViewState["mitHistorialCalificacion"] = this._mit_HistorialCalificaciones;
        }
        /// <summary>
        /// Método que consulta o estrae los valores de la página. 
        /// </summary>   
        private void recuperaAtributos()
        {            
            //Valida el valor de la variable ViewState idTabla(que sea diferente de 0)
            if (Convert.ToInt32(ViewState["idTabla"]) != 0)
                //Si cumple la condición, asigna al atributo _id_tabla el valor de la variable ViewState idTabla
                this._id_tabla = Convert.ToInt32(ViewState["idTabla"]);
            //Valida el valor de la variable Viewstate idRegistro (que sea diferente a 0)
            if (Convert.ToInt32(ViewState["idRegistro"]) != 0)
                //Si cumple la condición, asigna al atributo _id_registro el valor de la variable ViewState idRegistro
                this._id_registro = Convert.ToInt32(ViewState["idRegistro"]);
            //Valida el valor de la variable Viewstate idConcepto (que sea diferente a 0)
            if ((DataTable)ViewState["mitHistorialCalificacion"] != null)
                //Si cumple la condición, asigna al atributo _mitConceptoCalificacion el valor de la variable ViewState mitConceptoCalificacion
                this._mit_HistorialCalificaciones = (DataTable)ViewState["mitHistorialCalificacion"];
            
        }

        private void inicializaValores(int id_tabla, int id_registro)
        {            
            //ACORDE A LA TABLA OBTIENE LOS DATOS DE LA ENTIDAD A EVALUAR.
            switch (id_tabla)
            {
                //TABLA SERVICIO
                case 1:
                    {
                        using (SAT_CL.Documentacion.Servicio ser = new SAT_CL.Documentacion.Servicio(this._id_registro))
                        {
                            lblEntidad.Text = "N°. SERVICIO: " + ser.no_servicio;
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
            //Invoca el método que cargara las calificaciones dadas a una entidad
            inicializaHistorialCalificaciones();
        }

        private void inicializaHistorialCalificaciones()
        {
            //Inicializa el indice del gridView
            Controles.InicializaIndices(gvHistorialCalificacion);
            //Instancia al método que obtiene los conceptos evaluados a una entidad
            using (DataTable mit = SAT_CL.Calificacion.Calificacion.ObtieneHistorialCalificacionEntidad(this._id_tabla, this._id_registro))
            {
                //Carga el gridView de valores
                Controles.CargaGridView(gvHistorialCalificacion, mit, "Id", "", true, 0);
                //ASigna al atributo los valores de la tabla
                this._mit_HistorialCalificaciones = mit;
            }
        }

        #endregion

        #region Métodos Públicos

        public void InicializaControl(int id_tabla, int id_registro)
        {
            this._id_tabla = id_tabla;
            this._id_registro = id_registro;
            inicializaValores(this._id_tabla, this._id_registro);
        }
        #endregion


    }
}