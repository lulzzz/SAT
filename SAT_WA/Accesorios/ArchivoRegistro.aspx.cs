using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;

namespace SAT.Accesorios
{
    public partial class ArchivoRegistro : System.Web.UI.Page
    {
        #region Eventos

        /// <summary>
        /// Evento Producido al efectuarse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {   //Validando que se Produzca un PostBack
            if (!(Page.IsPostBack))
                //Invocando Método de Inicialización
                inicializaForma();
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Selección del Control "ddlTipo"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTipo_SelectedIndexChanged(object sender, EventArgs e)
        {   //Declarando variables 
            int idRegistro, idTabla, idTipCatalogoValido, idCompania;
            //Validamos Seleccion
            if (ddlTipo.SelectedValue != "0")
            {   //Validamos  Datos
                if (validaDatos(out idTabla, out idRegistro, out idTipCatalogoValido, out idCompania))
                    //Carga Archivos
                    cargaArchivos(idTabla, idRegistro, Convert.ToInt32(ddlTipo.SelectedValue), idCompania);
            }
        }
        /// <summary>
        /// Evento Producido al Presionar el boton "Guardar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {   //Invocando Método de Guardado
            guardaArchivo();
        }
        /// <summary>
        /// Evento Producido al Presionar el boton "Cancelar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {   //Determinando petición de cierre de ventana (con o sin recarga de padre)
            if (Request.QueryString["actualizaPadre"] != null ? Convert.ToBoolean(Request.QueryString["actualizaPadre"]) : true)
                //cerrando ventana actual y actualizando contenido de ventana padre
                TSDK.ASP.ScriptServer.ActualizaVentanaPadre(this, true);
            else//Sólo cerrando ventana hija
                TSDK.ASP.ScriptServer.CierraVentana(this);
        }

        #region Eventos GridView

        /// <summary>
        /// Evento Producido el Cambiar el Indice del Control "ddlTamano"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamano_SelectedIndexChanged(object sender, EventArgs e)
        {   //Validando que Existan Registros
            if(gvArchivos.DataKeys.Count > 0)
                //Cambiando Tamaño del GridView
                Controles.CambiaTamañoPaginaGridView(gvArchivos, ((DataSet)Session["DS"]).Tables["ArchivosRegistro"], Convert.ToInt32(ddlTamano.SelectedValue));
        }
        /// <summary>
        /// Evento Producido al Presionar el LinkButton "Exportar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportar_Click(object sender, EventArgs e)
        {   //Validando que Existan Registros
            if (gvArchivos.DataKeys.Count > 0)
                //Exportando Contenido del GridView
                Controles.ExportaContenidoGridView(((DataSet)Session["DS"]).Tables["ArchivosRegistro"]);
        }
        /// <summary>
        /// Evento Producido al Cambiar la Expresión de Ordenamiento del GridView "Archivos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvArchivos_Sorting(object sender, GridViewSortEventArgs e)
        {   //Validando que Existan Registros
            if (gvArchivos.DataKeys.Count > 0)
                //Cambiando Expresión de Ordenamiento del GridView
                lblCriterio.Text = Controles.CambiaSortExpressionGridView(gvArchivos, ((DataSet)Session["DS"]).Tables["ArchivosRegistro"], e.SortExpression);
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Paginación del GridView "Archivos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvArchivos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {   //Validando que Existan Registros
            if (gvArchivos.DataKeys.Count > 0)
                //Cambiando Indice de Página del GridView
                Controles.CambiaIndicePaginaGridView(gvArchivos, ((DataSet)Session["DS"]).Tables["ArchivosRegistro"], e.NewPageIndex);
        }

        #region Eventos Controles GridView

        /// <summary>
        /// Evento Producido al Presionar el CheckBox "Todos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkTodos_CheckedChanged(object sender, EventArgs e)
        {   //Si existen registros activos
            if (gvArchivos.DataKeys.Count > 0)
            {   //Referenciando al control que produjo el evento
                CheckBox todos = (CheckBox)sender;
                //Referenciando a la etiqueta contador del pie
                using (Label c = (Label)gvArchivos.FooterRow.FindControl("lblContadorDetalles"))
                    //Realizando la selección de todos los elementos del GridView
                    c.Text = TSDK.ASP.Controles.SeleccionaFilasTodas(gvArchivos, "chkDetalle", todos.Checked).ToString();
            }
        }
        /// <summary>
        /// Evento Producido al Presionar el CheckBox "Detalle"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkDetalle_CheckedChanged(object sender, EventArgs e)
        {   //Si existen registros activos
            if (gvArchivos.DataKeys.Count > 0)
            {   //Referenciando al control que produjo el evento
                CheckBox todos = (CheckBox)sender;
                //Sumando Totales en Footer
                TSDK.ASP.Controles.SumaSeleccionadosFooter(todos.Checked, gvArchivos, "lblContadorDetalles");
            }
        }
        /// <summary>
        /// Evento Producido al Presionar el LinkButton "lnkEliminar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEliminar_Click(object sender, EventArgs e)
        {   //Validando que Existan Registros
            if (gvArchivos.DataKeys.Count > 0)
            {   //Seleccionando Fila a Descargar
                Controles.SeleccionaFila(gvArchivos, sender, "lnk", false);
                //Invocando Método de Eliminación
                eliminaArchivo();
            }
        }
        /// <summary>
        /// Evento Producido al Presionar el LinkButton "lnkDescargar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkDescargar_Click(object sender, EventArgs e)
        {   //Validando que Existan Registros
            if (gvArchivos.DataKeys.Count > 0)
            {   //Seleccionando Fila a Descargar
                Controles.SeleccionaFila(gvArchivos, sender, "lnk", false);
                //Invocando Método de Descarga
                descargaArchivo();
            }
        }

        #endregion

        #endregion

        #endregion

        #region Métodos

        /// <summary>
        /// Método Privado encargado de Inicializar los Controles de la Forma
        /// </summary>
        private void inicializaForma()
        {   //Declarando variables 
            int idRegistro, idTabla, idTipCatalogoValido, IdCompania;
            //Validamos 
            if (validaDatos(out idTabla, out idRegistro, out idTipCatalogoValido, out IdCompania))
            {   //Carga catalogos
                SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamano, "", 18);
                SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipo, 14, "", idTabla, "", 0, "");
                //Inicializa Grid View
                cargaArchivos(idTabla, idRegistro, Convert.ToInt32(ddlTipo.SelectedValue), IdCompania);
            }
            //Limpiamos Errores
            lblError.Text = "";
            lblErrorArchivos.Text = "";
            txtReferencia.Text = "";
        }
        /// <summary>
        /// Método Privado encargado de Validar los Datos
        /// </summary>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="id_registro">Id de Registro</param>
        /// <param name="id_tipo_catalogo_valido">Id de Tipo de Catalogo Valido</param>
        /// <param name="id_compania">Id Compañia</param>
        /// <returns></returns>
        private bool validaDatos(out int id_tabla, out int id_registro, out int id_tipo_catalogo_valido, out int id_compania)
        {   //Declarando objeto de retorno
            bool resultado = false;
            //Inicializnado valores de salida
            id_registro = id_tabla = id_tipo_catalogo_valido = id_compania = 0;
            //Id de Registro a consultar
            if (Request.QueryString.Get("idR") != "" &&
                Request.QueryString.Get("idT") != "" &&
                Request.QueryString.Get("idTV") != "" &&
                Request.QueryString.Get("idC") != "")
            {   //Id de Registro
                id_registro = Convert.ToInt32(Request.QueryString.Get("idR"));
                //Id de Tabla
                id_tabla = Convert.ToInt32(Request.QueryString.Get("idT"));
                //Id Tipo Catalogo Valido
                id_tipo_catalogo_valido = Convert.ToInt32(Request.QueryString.Get("idTV"));
                //Id Compañia
                id_compania = Convert.ToInt32(Request.QueryString.Get("idC"));
                //Devolviendo Resultado Positivo
                resultado = true;
            }
            //Devolviendo valor de resultado
            return resultado;
        }
        /// <summary>
        /// Método Privado que Realiza la Búsqueda y Carga de archivos de un tipo solicitado para el registro de la tabla indicada
        /// </summary>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="id_registro">Id de Registro</param>
        /// <param name="id_archivo_tipo_configuracion">Id de Tipo de Archivo de Configuración</param>
        /// <param name="id_compania">Id Compania</param>
        private void cargaArchivos(int id_tabla, int id_registro, int id_archivo_tipo_configuracion, int id_compania)
        {   //Definiendo tabla para almacenar datos devueltos
            using (DataTable mit = SAT_CL.Global.ArchivoRegistro.CargaArchivoRegistro(id_tabla, id_registro, id_archivo_tipo_configuracion, id_compania))
            {   //Validando origen de datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(mit))
                {   //Inicializando indices de selección
                    Controles.InicializaIndices(gvArchivos);
                    //Guardando Datos en sesión
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], mit, "ArchivosRegistro");
                    //Carga Grid View 
                    Controles.CargaGridView(gvArchivos, mit, "Id", "", true, 1);
                }
                else
                {   //Inicializamos Grid View 
                    Controles.InicializaGridview(gvArchivos);
                    //Elimina Tabla Dataset
                    TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "ArchivosRegistro");
                }
            }
        }
        /// <summary>
        /// Método Privado que Guarda los Archivos
        /// </summary>
        private void guardaArchivo()
        {   //Definiendo objeto de resultado
            RetornoOperacion result = new RetornoOperacion();
            //Id de Registro
            int idRegistro = Convert.ToInt32(Request.QueryString.Get("idR"));
            //Id de Tabla
            int idTabla = Convert.ToInt32(Request.QueryString.Get("idT"));
            //Id Tipo Catalogo Valido
            int id_tipo_catalogo_valido = Convert.ToInt32(Request.QueryString.Get("idTV"));
            //Id Compañia
            int id_compania = Convert.ToInt32(Request.QueryString.Get("idC"));
            //Validamos que el Id tipo de catalogo seleccionado se igual al definido 
            if (Convert.ToInt32(ddlTipo.SelectedValue) == id_tipo_catalogo_valido || id_tipo_catalogo_valido == 0)
            {   //Validando si existe un Archivo
                if (fuArchivo.HasFile)
                {   //Declaramos variable para almacenar ruta
                    string ruta = "";
                    //Armando ruta de guardado físico de archivo
                    //Compuesta de un path genérico para esta ventana, el Id de Tabla e Id de Registro, más el nombre asignado al archivo (incluyendo extensión)
                    //ruta = string.Format(@"{0}{1}\{2}-{3}{4}", SAT_CL.CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Directorio Raíz Gestor Archivos SAT"), idTabla.ToString("0000"), idRegistro.ToString("0000000"), txtReferencia.Text.ToUpper().Replace(" ", "_"), "." + TSDK.Base.Cadena.RegresaCadenaSeparada(fuArchivo.FileName, '.', 1));
                    ruta = string.Format(@"{0}{1}\{2}-{3}{4}", SAT_CL.CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Directorio Raíz Gestor Archivos SAT"), idTabla.ToString("0000"), idRegistro.ToString("0000000"), txtReferencia.Text.ToUpper().Replace(" ", "_"), "." + TSDK.Base.Cadena.RegresaCadenaSeparada(fuArchivo.FileName, '.', 1));
                    //Insertamos Registro
                    result = SAT_CL.Global.ArchivoRegistro.InsertaArchivoRegistro(idTabla, idRegistro, Convert.ToInt32(ddlTipo.SelectedValue), txtReferencia.Text.ToUpper(), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario, fuArchivo.FileBytes, ruta);
                }
                else//Instanciando Exception
                    result = new RetornoOperacion("No hay un archivo seleccionado");
            }
            else//Instanciando Exception
                result = new RetornoOperacion("El Tipo seleccionado no es Valido");
            //Si no existe error
            if (result.OperacionExitosa)
            {   //Cargamos Archivos Ingresados           
                cargaArchivos(idTabla, idRegistro, Convert.ToInt32(ddlTipo.SelectedValue), id_compania);
                //Limpiando controles de carga
                txtReferencia.Text = "";
            }
            //Mostrando resultado
            lblError.Text = result.Mensaje;
            lblErrorArchivos.Text = "";
        }
        /// <summary>
        /// Método Privado que Exporta los Archivos
        /// </summary>
        private void descargaArchivo()
        {   //Validando que este seleccionado el registro
            if (gvArchivos.SelectedIndex != -1)
            {   //Instanciando Registro del Archivo
                using (SAT_CL.Global.ArchivoRegistro ar = new SAT_CL.Global.ArchivoRegistro(Convert.ToInt32(gvArchivos.SelectedDataKey["Id"])))
                {   //Instanciando Tipo de Configuración del Archivo
                    using (SAT_CL.Global.ArchivoTipoConfiguracion atc = new SAT_CL.Global.ArchivoTipoConfiguracion(Convert.ToInt32(ddlTipo.SelectedValue)))
                    {   //Instanciando Tipo de Archivo
                        using(SAT_CL.Global.ArchivoTipo at = new SAT_CL.Global.ArchivoTipo(atc.id_archivo_tipo))
                            //Descargando el Archivo deseado
                            TSDK.Base.Archivo.DescargaArchivo(File.ReadAllBytes(ar.url), ar.referencia + at.extension, TSDK.Base.Archivo.ContentType.binary_octetStream);
                    }
                }

            }
        }
        /// <summary>
        /// Método Privado que Elimina los Archivos
        /// </summary>
        private void eliminaArchivo()
        {   //Validando que este seleccionado el registro
            if (gvArchivos.SelectedIndex != -1)
            {   //Id Tipo Catalogo Valido
                int id_tipo_catalogo_valido = Convert.ToInt32(Request.QueryString.Get("idTV"));
                //Id de Registro
                int idRegistro = Convert.ToInt32(Request.QueryString.Get("idR"));
                //Id de Tabla
                int idTabla = Convert.ToInt32(Request.QueryString.Get("idT"));
                //Id Compañia
                int id_compania = Convert.ToInt32(Request.QueryString.Get("idC"));
                //Declarando Retorno de Operación
                RetornoOperacion result = new RetornoOperacion();
                //Instanciando Registro del Archivo
                using (SAT_CL.Global.ArchivoRegistro ar = new SAT_CL.Global.ArchivoRegistro(Convert.ToInt32(gvArchivos.SelectedDataKey["Id"])))
                {   //Validamos  el Tipo de Archivo
                    if (id_tipo_catalogo_valido == ar.id_archivo_tipo_configuracion)
                    {   //Validando que exista un Registro
                        if (ar.id_archivo_registro != 0)
                            //Deshabilitando Registro
                            result = ar.DeshabilitaArchivoRegistro(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                        else//Instanciando Exception
                            result = new RetornoOperacion("Archivo no localizado en BD");
                    }
                    else//Instanciando Exception
                        result = new RetornoOperacion("El Tipo no es válido para su eliminación");
                }
                //Valdiando que la Operación haya sido exitosa
                if (result.OperacionExitosa)
                    //Cargando Archivos Ingresados
                    cargaArchivos(idTabla, idRegistro, Convert.ToInt32(ddlTipo.SelectedValue), id_compania);
                //Mostrando Mensajes
                lblError.Text = "";
                lblErrorArchivos.Text = result.Mensaje;
            }
        }

        #endregion
    }
}