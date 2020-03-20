using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAT_CL.Despacho;
using SAT_CL.Documentacion;
using System.Data;
using TSDK.ASP;

namespace SAT.Externa
{
    public partial class Calificacion : System.Web.UI.Page
    {
        #region Atributos
        
        /// <summary>
        /// Declaramos Tabla para Paradas
        /// </summary>
        private DataTable _table_Paradas;
        #endregion
        /// <summary>
        /// Método generado al Cargar la Página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Validando que se Produzca un PostBack
            if (!(Page.IsPostBack))
                //Invocando Método de Inicialización
                inicializaForma();
        }

       #region Eventos
        /// <summary>
        /// Evento Generado al Cambiar el Tamaño
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamañoGridViewParadas_SelectedIndexChanged(object sender, EventArgs e)
        {

            //Cambiando Tamaño de Registros
            Controles.CambiaTamañoPaginaGridView(gvParadas, this._table_Paradas, Convert.ToInt32(ddlTamañoGridViewParadas.SelectedValue));

        }

        /// <summary>
        /// Evento generado al exportar Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarParadas_Click(object sender, EventArgs e)
        {
            //Exportando Excel
            Controles.ExportaContenidoGridView(this._table_Paradas);
        }

        /// <summary>
        /// Cerrar alguna ventana modal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarVentanaModal_Click(object sender, EventArgs e)
        {
            //Determinando botón pulsado
            LinkButton lkbCerrar = (LinkButton)sender;

            //De acuerdo al comando del botón
            switch (lkbCerrar.CommandName)
            {

                case "Calificacion":
                    //Cerramos Ventana Modal
                    alternaVentanaModal("calificacion", lkbCerrar);
                    //Inicializamos Promedio Servicio
                      inicializaPromedioServicio(Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(lblTablaServicio.Text, ':', 0)), Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(lblTablaServicio.Text, ':', 1)));
                    //Inicializamos Promedio Operador
                     inicializaPromedioOperador(Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(lblTablaOperador.Text, ':', 0)), Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(lblTablaOperador.Text, ':', 1)));
           
                    break;
            }
        }

        /// <summary>
        /// Evento generado al Calificar un Servicio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imbCalificarServicio_Click(object sender, EventArgs e)
        {
            //Mostramos Ventana de Calificación e Servicio
            alternaVentanaModal("calificacion", imbCalificarServicio);
             //Declarando variables 
            int idServicio, idContacto;
            //Validamos 
            if (validaDatos(out idServicio, out idContacto))
            {
               
            //Inicializamos Control de Usuario
                wucCalificacion.InicializaControl(1, idServicio,
                                                idContacto, 0,0,true);
            }
        }

        /// <summary>
        /// Evento generado al Calificar un Operador
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imbCalificarOperador_Click(object sender, EventArgs e)
        {
            //Mostramos Ventana de Calificación de Operador
            alternaVentanaModal("calificacion", imbCalificarOperador);

            //Declarando variables 
            int idServicio, idContacto;
            //Validamos 
            if (validaDatos(out idServicio, out idContacto))
            {
               
            //Inicializamos Control de Usuario
                wucCalificacion.InicializaControl(76, SAT_CL.Global.Operador.ObtieneIdUltimoOperador(idServicio),
                                                idContacto, 0,0,true);
            }
        }

        /// <summary>
        /// Evento Genrado al Guardar la Calificación
        /// </summary>
        protected void wucCalificacion_ClickGuardarCalificacionGeneral(object sender, EventArgs e)
        {
            wucCalificacion.GuardarCalificacionGeneral();

             //Declarando variables 
            int idServicio, idContacto;
            //Validamos 
            if (validaDatos(out idServicio, out idContacto))
            {
                //Inicializamos Promedio Servicio
                inicializaPromedioServicio(Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(lblTablaServicio.Text, ':', 0)), Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(lblTablaServicio.Text, ':', 1)));
                //Inicializamos Promedio Operador
                inicializaPromedioOperador(Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(lblTablaOperador.Text, ':', 0)), Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(lblTablaOperador.Text, ':', 1)));
            }

        }
       #endregion

       #region Métodos
        /// <summary>
        /// Método Privado encargado de Inicializar los Controles de la Forma
        /// </summary>
        private void inicializaForma()
        {  
            //Declarando variables 
            int idServicio, idContacto;
            //Validamos 
            if (validaDatos(out idServicio, out idContacto))
            {
                //Inicializamos Contenido
                inicializaContenidoControles(idServicio);
                //Cargamos Paradas
                cargaParadas(idServicio);
                lblTablaOperador.Text = "76 :" + SAT_CL.Global.Operador.ObtieneIdUltimoOperador(idServicio);
                lblTablaServicio.Text = "1 :" + idServicio.ToString();

                inicializaPromedioServicio(Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(lblTablaServicio.Text, ':', 0)), Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(lblTablaServicio.Text, ':', 1)));
                //Inicializamos Promedio Operador
                inicializaPromedioOperador(Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(lblTablaOperador.Text, ':', 0)), Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(lblTablaOperador.Text, ':', 1)));
            
            }
            //Cargamos Catalogo
            cargaCatalogos();
        }

        /// <summary>
        /// Método Privado encargado de Inicializar los Controles de la Forma
        /// </summary>
        private void inicializaContenidoControles(int id_servicio)
        {
            //Instanciamos Servicio
            using(Servicio objServicio = new Servicio(id_servicio))
            {
                //Instanciamos Cliente
                using (SAT_CL.Global.CompaniaEmisorReceptor objCliente = new SAT_CL.Global.CompaniaEmisorReceptor(objServicio.id_cliente_receptor))
                {
                    //Instanciamos Parada Origen
                    using(SAT_CL.Global.Ubicacion objUbicacionOrigen = new SAT_CL.Global.Ubicacion (objServicio.id_ubicacion_carga))
                    {
                        //Instanciamos Parada Destino
                        using (SAT_CL.Global.Ubicacion objUbicacionDestino = new SAT_CL.Global.Ubicacion(objServicio.id_ubicacion_descarga))
                        {
                            //Intsanciamos Despacho
                            using (SAT_CL.Despacho.ServicioDespacho objServicioDespacho = new ServicioDespacho(ServicioDespacho.TipoCarga.IdServicio, objServicio.id_servicio))
                            {
                                lblServicio.Text = "Servicio " + objServicio.no_servicio;
                                lblCliente.Text = objCliente.nombre_corto;
                                lblOrigen.Text = objUbicacionOrigen.descripcion;
                                lblDestino.Text = objUbicacionDestino.descripcion;
                                lblEstatus.Text = SAT_CL.Global.Catalogo.RegresaDescripcionCatalogo(6, Convert.ToInt32(objServicio.estatus));
                                lblNoViaje.Text = SAT_CL.Global.Referencia.CargaReferencia(objServicio.id_compania_emisor.ToString(), 1, objServicio.id_servicio, "Referencia de Viaje", "No. Viaje");
                                lblPorte.Text = objServicio.porte;
                                //Instanciamos Operador
                                using (SAT_CL.Global.Operador objOperador = new SAT_CL.Global.Operador(SAT_CL.Global.Operador.ObtieneIdUltimoOperador(id_servicio)))
                                {
                                    lblOperador.Text = objOperador.nombre;
                                }
                            }
                        }
                    }
                }
            }
                //Cargamos Parada
                cargaParadas(id_servicio);
             
        }

        /// <summary>
        /// Método Privado encargado de Cargar los Catalogos de la Forma
        /// </summary>
        private void cargaCatalogos()
        {

            //Cargando Catalogos 
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamañoGridViewParadas, "", 18);

        }

        /// <summary>
        /// Método encargado de Validar Datos Requeridos.
        /// </summary>
        /// <param name="id_servicio">Id Servicio</param>
        /// <param name="id_contacto">Id Contacto</param>
        /// <returns></returns>
        private bool validaDatos(out int id_servicio, out int id_contacto)
        {   //Declarando objeto de retorno
            bool resultado = false;
            //Inicializnado valores de salida
            id_servicio = id_contacto  = 0;
            //Id de Registro a consultar
            if (Request.QueryString.Get("idS") != "" &&
                Request.QueryString.Get("idC") != "" )
            {
                //Id de Servicio
                id_servicio = Convert.ToInt32(Request.QueryString.Get("idS"));
                //Id dContacto
                id_contacto = Convert.ToInt32(Request.QueryString.Get("idC"));
                //Devolviendo Resultado Positivo
                resultado = true;
            }
            //Devolviendo valor de resultado
            return resultado;
        }

        /// <summary>
        /// Método encargada de cargar las Paradas
        /// </summary>
        private void cargaParadas(int id_servicio)
        {
            //Inicializamos Indices del grid View Paradas
            TSDK.ASP.Controles.InicializaIndices(gvParadas);

            //Validando que Exista un Servicio
            if (id_servicio != 0)
            {
                //Obtenemos Paradas
                using (DataTable mit = SAT_CL.Despacho.Parada.CargaParadasParaVisualizacion(id_servicio))
                {
                    //Valida Origen de Datos
                    if (TSDK.Datos.Validacion.ValidaOrigenDatos(mit))
                    {
                        //Cargamos Grid View
                        TSDK.ASP.Controles.CargaGridView(gvParadas, mit, "Id-Secuencia-IdMovimiento", "", false, 0);

                        //Añadiendo Tabla 
                        this._table_Paradas = mit;
                    }
                    else
                    {
                        //Inicializando GridView
                        TSDK.ASP.Controles.InicializaGridview(gvParadas);

                        //Eliminando Tabla 
                        this._table_Paradas = null;
                    }
                }
            }
            else
            {
                //Inicializando GridView
                TSDK.ASP.Controles.InicializaGridview(gvParadas);

                //Eliminando Tabla 
                this._table_Paradas = null;
            }
        }

        /// <summary>
        /// Muestra u oculta la ventana modal solicitada
        /// </summary>
        /// <param name="nombre_ventana">Nombre de ventana por ocultar</param>
        /// <param name="control">Control que solicita la acción</param>
        private void alternaVentanaModal(string nombre_ventana, System.Web.UI.Control control)
        {
            //Determinando la ventana por alternar
            switch (nombre_ventana)
            {
                case "calificacion":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "crearCalificar", "contenedorCalificar");
                    break;
            }
        }

        /// <summary>
        /// Inicializmamos Promedio
        /// </summary>
        private void inicializaPromedioServicio(int id_tabla, int id_registro)
        {
            //Obtenemos Promedio
            byte promedio = SAT_CL.Calificacion.Calificacion.ObtieneEntidad(id_tabla, id_registro);


            //Valida el valor de la variable totalEstrellas y asigna una ubicación de una imagen a mostrar
            switch (promedio)
            {
                case 0:
                    {
                        imgCalificarServicio.ImageUrl = "~/Image/Estrella.png";
                        break;
                    }
                case 1:
                    {
                        imgCalificarServicio.ImageUrl = "~/Image/EstrellaH1.png";
                        break;
                    }
                case 2:
                    {
                        imgCalificarServicio.ImageUrl = "~/Image/EstrellaH2.png";
                        break;
                    }
                case 3:
                    {
                        imgCalificarServicio.ImageUrl = "~/Image/EstrellaH3.png";
                        break;
                    }
                case 4:
                    {
                        imgCalificarServicio.ImageUrl = "~/Image/EstrellaH4.png";
                        break;
                    }
                case 5:
                    {
                        imgCalificarServicio.ImageUrl = "~/Image/EstrellaH5.png";
                        break;
                    }
            }
        }
            
             

        /// <summary>
        /// Inicializmamos Promedio
        /// </summary>
        private void inicializaPromedioOperador(int id_tabla, int id_registro)
        {
            //Obtenemos Promedio
            byte promedio = SAT_CL.Calificacion.Calificacion.ObtieneEntidad(id_tabla, id_registro);

            
                //Valida el valor de la variable totalEstrellas y asigna una ubicación de una imagen a mostrar
                switch (promedio)
                {
                    case 0:
                        {
                            imgCalificarOperador.ImageUrl = "~/Image/Estrella.png";
                            break;
                        }
                    case 1:
                        {
                            imgCalificarOperador.ImageUrl = "~/Image/EstrellaH1.png";
                            break;
                        }
                    case 2:
                        {
                            imgCalificarOperador.ImageUrl = "~/Image/EstrellaH2.png";
                            break;
                        }
                    case 3:
                        {
                            imgCalificarOperador.ImageUrl = "~/Image/EstrellaH3.png";
                            break;
                        }
                    case 4:
                        {
                            imgCalificarOperador.ImageUrl = "~/Image/EstrellaH4.png";
                            break;
                        }
                    case 5:
                        {
                            imgCalificarOperador.ImageUrl = "~/Image/EstrellaH5.png";
                            break;
                        }
                }
            
        }
       #endregion

       

       
    }
}