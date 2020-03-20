using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;


namespace SAT.Accesorios
{
    public partial class DatosOperador : System.Web.UI.Page
    {
        #region Eventos
        /// <summary>
        /// Inicialia los valores de la página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Valida si la página se ha cargado por primera vez
            if (!Page.IsPostBack)
                inicializaForma();
        }
        /// <summary>
        /// Evento que abre el control de usuario Calificacion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imgbCalificacion_Click(object sender, ImageClickEventArgs e)
        {
            int idOperador = Convert.ToInt32(Request.QueryString.Get("idOp"));
            wucCalificacion.InicializaControl(76, idOperador, 0, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario, 0, true);
            TSDK.ASP.ScriptServer.AlternarVentana(imgbCalificacion, "ventanaCalificacion", "contenedorVentanaCalificacion", "ventanaCalificacion");             
        }
        /// <summary>
        /// Evento que cierra ventana modal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarCalificacion_Click(object sender, EventArgs e)
        {
            //Obtiene el control linkButom para definir que accion se realizara
            LinkButton lnk = (LinkButton)sender;
            //Valida el linkbutton y determina la acción a ejecutar
            switch (lnk.CommandName)
            {
                //Cierra la ventana modal de Calificación
                case "CierraCalificacion":
                    {
                        //Inicializa forma y cierra la ventan modal del Control de usuario CAlificación
                        inicializaForma();
                        TSDK.ASP.ScriptServer.AlternarVentana(this, this.GetType(), "CerrarVentana", "contenedorVentanaCalificacion", "ventanaCalificacion");            
                        break;
                    }
                //Cierra la ventana modal de Historial Calificación
                case "CierraHistorialCalificacion":
                    {
                        TSDK.ASP.ScriptServer.AlternarVentana(this, this.GetType(), "CerrarVentana", "contenedorVentanaHistorialCalificacion", "ventanaHistorialCalificacion");            
                        break;
                    }
            }
            
        }
        /// <summary>
        /// Evento que alamcena la calificación general calificada
        /// </summary>
        protected void wucCalificacion_ClickGuardarCalificacionGeneral(object sender, EventArgs e)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Asigna al objeto retorno el resultado del método del control de usuario que guardar calificación general clificada
            retorno= wucCalificacion.GuardarCalificacionGeneral();
            //Valida la operación de almacenamiento de la operación
            if (retorno.OperacionExitosa)
            {
                //Inicializa la forma y cierra la vetana modal.
                inicializaForma();
                TSDK.ASP.ScriptServer.AlternarVentana(this, this.GetType(), "CerrarVentana", "contenedorVentanaCalificacion", "ventanaCalificacion");
            }
        }
        /// <summary>
        /// Evento que invoca al control de usuario historial Calificación
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbComentarios_Click(object sender, EventArgs e)
        {
            //Crea variable que almacena al operador
            int idOperador = Convert.ToInt32(Request.QueryString.Get("idOp"));
            //Inicializa el control de usuario Calificación Historial
            wucHistorialCalificacion.InicializaControl(76, idOperador);
            //Abre la ventana modal de historial Calificación
            TSDK.ASP.ScriptServer.AlternarVentana(this, this.GetType(), "HistorialCalificacion", "contenedorVentanaHistorialCalificacion", "ventanaHistorialCalificacion");
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Inicializa los valores de la página
        /// </summary>
        private void inicializaForma()
        {
            //Obtiene el identificador del Operador del Query String
            int idOperador = Convert.ToInt32(Request.QueryString.Get("idOp"));
            //Instancia a la clase operador para obtener los datos del oeprador
            using (SAT_CL.Global.Operador op = new SAT_CL.Global.Operador(idOperador))
            {
                //Asigna valores a los controles
                lblNombre.Text = op.nombre;
                lblTipoLicencia.Text = SAT_CL.Global.Catalogo.RegresaDescripcionCatalogo(1105, op.id_tipo_licencia);
                lblNoLicencia.Text = op.numero_licencia;
                lblCelular.Text = op.telefono;
                lblTelefonoCasa.Text = op.telefono_casa;
                lblRControl.Text =op.r_control;
                lblNSS.Text = op.nss;
                lblRFC.Text = op.rfc;
                lblCURP.Text = op.curp;
                lblCodAutorizacion.Text = op.cod_authenticacion;
                //Obtiene la dirección del operador
                using (SAT_CL.Global.Direccion dir = new SAT_CL.Global.Direccion(op.id_direccion))
                {
                    lblDireccion.Text = dir.ObtieneDireccionCompleta();
                }
                //Crea la variable que almacenara la ubicación actual del operador
                string ubicacionActual = "";
                //Acorde al estatus del operador obtiene la ubicación actual
                switch (op.estatus)
                {
                    case SAT_CL.Global.Operador.Estatus.Disponible:
                    case SAT_CL.Global.Operador.Estatus.Ocupado:
                        //Instanciando Parada actual
                        using (SAT_CL.Despacho.Parada p = new SAT_CL.Despacho.Parada(op.id_parada))
                            ubicacionActual = p.descripcion;
                        break;
                    case SAT_CL.Global.Operador.Estatus.Transito:
                        //Instanciando movimiento
                        using (SAT_CL.Despacho.Movimiento m = new SAT_CL.Despacho.Movimiento(op.id_movimiento))
                            ubicacionActual = m.descripcion;
                        break;
                    default:
                        ubicacionActual = "No Disponible";
                        break;
                }
                //Asigna al label de ubicación Actual el valor de la variable ubicacionActual.
                lblUbicacionActual.Text = ubicacionActual;
                //Creación de la variable que almacena la vigencia de una licencia.
                bool vigencia= SAT_CL.Global.Vencimiento.ValidaLicenciaVigente(idOperador);
                if (!vigencia)                   
                    lblVencimiento.Text = "Vencida desde:";                                                                                
                else                
                    lblVencimiento.Text = "Vigente hasta:";
                //Muestra la fecha de inicio del vencimiento
                using (DataTable dtFechaVEncimiento = SAT_CL.Global.Vencimiento.CargaVencimientosRecurso(SAT_CL.Global.TipoVencimiento.TipoAplicacion.Operador, idOperador, 1, 4, 1, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue))
                {
                    //Valida los datos del datatable
                    if (Validacion.ValidaOrigenDatos(dtFechaVEncimiento))
                    {
                        //RECORRE EL DATATABLE
                        foreach (DataRow r in dtFechaVEncimiento.Rows)
                        {
                            DateTime fecha = Convert.ToDateTime(r["FechaInicio"]);
                            lblFechaVencimiento.Text = fecha.ToString("yyyy-MM-dd");
                        }
                    }
                }                    
                //Creación de la variable Calificación 
                byte Calificacion = 0;
                int CantidadComentarios = 0;
                //Obtiene el promedio de calificación del operador
                Calificacion = SAT_CL.Calificacion.Calificacion.ObtieneEntidad(76, idOperador);
                CantidadComentarios = SAT_CL.Calificacion.Calificacion.ObtieneNumeroComentarios(76, idOperador);
                    //Acorde al promedio colocara el promedio
                    switch(Calificacion)
                    {
                        case 1:
                            imgbCalificacion.ImageUrl = "~/Image/Estrella1.png";
                            lkbComentarios.Text = Calificacion + " / 5" + " ( " + CantidadComentarios + " opiniones  )"; 
                            break;
                        case 2:
                            imgbCalificacion.ImageUrl = "~/Image/Estrella2.png";
                            lkbComentarios.Text = Calificacion + " / 5" + " ( " + CantidadComentarios + " opiniones  )"; 
                            break;
                        case 3:
                            imgbCalificacion.ImageUrl = "~/Image/Estrella3.png";
                            lkbComentarios.Text = Calificacion + " / 5" + " ( " + CantidadComentarios + " opiniones  )"; 
                            break;
                        case 4:
                            imgbCalificacion.ImageUrl = "~/Image/Estrella4.png";
                            lkbComentarios.Text = Calificacion + " / 5" + " ( " + CantidadComentarios + " opiniones  )"; 
                            break;
                        case 5:
                            imgbCalificacion.ImageUrl = "~/Image/Estrella5.png";
                            lkbComentarios.Text = Calificacion + " / 5" + " ( " + CantidadComentarios + " opiniones  )"; 
                            break;
                        default:
                            imgbCalificacion.ImageUrl = "~/Image/Estrella.png";
                            lkbComentarios.Text = "0 / 5" + " ( 0 Opiniones  )"; 
                            break;
                    }
                //Busca la foto del operador                
                using (DataTable dtFotoOperador = SAT_CL.Global.ArchivoRegistro.CargaArchivoRegistro(76, idOperador, 21, 0))
                {
                    //Valida los datos del dataset
                    if (Validacion.ValidaOrigenDatos(dtFotoOperador))
                    {
                        //Recorre el dataset y la ruta de la foto del operador lo asigna al control de image Foto Operador
                        foreach (DataRow r in dtFotoOperador.Rows)
                            //Asigna la ubicación  de la foto del operador al control de imagen
                            imgOperador.ImageUrl = String.Format("../Accesorios/VisorImagenID.aspx?t_carga=archivo&t_escala=pixcel&alto=256&ancho=256&url={0}", Convert.ToString(r["URL"]));
                    }
                }
            }
        }
        
        #endregion



        
    }
}