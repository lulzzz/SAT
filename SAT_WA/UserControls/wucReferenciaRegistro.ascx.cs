using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SAT.UserControls
{
    public partial class wucReferenciaRegistro : System.Web.UI.UserControl
    {
        #region Propiedades del control

        /// <summary>
        /// Almacena el nombre la bd a la que pertenece la tabla
        /// </summary>
        private string _db;

        private int _id_tabla;
        /// <summary>
        /// Define la tabla a la cual pertenece el registro del que se mostraran las referencias.
        /// </summary>
        public int Tabla
        {   //Obteniendo Valor
            get { return _id_tabla; }
            //Asignando Valor
            set { _id_tabla = value; }
        }

        private int _id_registro;
        /// <summary>
        /// Define el registro al cual pertenecen las referencias.
        /// </summary>
        public int Registro
        {   //Obteniendo Valor
            get { return _id_registro; }
            //Asignando Valor
            set { _id_registro = value; }
        }

        private int _id_compania;
        /// <summary>
        /// Define la compania a la cual pertenecen las referencias.
        /// </summary>
        public int Compania
        {   //Obteniendo Valor
            get { return _id_compania; }
            //Asignando Valor
            set { _id_compania = value; }
        }

        #endregion

        #region Eventos del control

        /// <summary>
        /// Metodo ejecutado al cargar la pagina donde se encuentra contenido el control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {   //Si es una recarga de página
            if (Page.IsPostBack)
                //Leyendo viewstate
                leeViewState();
        }
        /// <summary>
        /// Evento disparado antes de guardar el Viewstate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {   //Asignando el viewstate
            asignaViewState();
        }
        /// <summary>
        /// Metodo disparado al seleccionar un nodo determinado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void trvReferencias_SelectedNodeChanged(object sender, EventArgs e)
        {   //Validando que exista un Nodo Seleccionado
            if (trvReferencias.SelectedNode != null)
            {   //limpiamos los indices del gridview
                TSDK.ASP.Controles.InicializaIndices(gvReferencias);
                //Seleccionando el Nodo
                SAT_CL.Global.Referencia.NodoSeleccionado(trvReferencias.SelectedNode.Depth, Convert.ToInt32(trvReferencias.SelectedValue), _id_tabla, _id_registro, _id_compania, gvReferencias, false);
            }
        }
        /// <summary>
        /// Inserta una nueva referencia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAgregar_Click(object sender, EventArgs e)
        {   //Si existe un tipo de referencia seleccionado en el arbol de estructura
            if (trvReferencias.SelectedNode != null && trvReferencias.SelectedNode.Depth == 2)
            {   //Declarando Objeto de Retorno
                SAT_CL.Global.Referencia.ErrorReferencia result;
                //Agregando registro dependiendo la BD
                result = SAT_CL.Global.Referencia.Guarda(SAT_CL.Global.Referencia.AccionReferencia.Registrar, Convert.ToInt32(trvReferencias.SelectedValue), 0, txtReferencia.Text, _id_registro, _id_tabla, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                //Mostrando Error
                SAT_CL.Global.Referencia.Error(result, lblError);
                //Recargamos el grid view
                if (result == SAT_CL.Global.Referencia.ErrorReferencia.SinError)
                    inicializaGridView();
            }
            else//Mostramos mensaje de nodo no valido
                SAT_CL.Global.Referencia.Error(SAT_CL.Global.Referencia.ErrorReferencia.NodoTipoNoValido, lblError);
        }
        /// <summary>
        /// Evento disparado al dar clic en el boton editar elemento del grid view 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEditar_Click(object sender, EventArgs e)
        {   //Seleccionamos la fila y la ponemos en modo editable
            TSDK.ASP.Controles.SeleccionaFila(gvReferencias, sender, "lnk", true);
            //Editando registro dependiendo la BD
            SAT_CL.Global.Referencia.NodoSeleccionado(trvReferencias.SelectedNode.Depth, Convert.ToInt32(trvReferencias.SelectedValue), _id_tabla, _id_registro, _id_compania, gvReferencias, true);
        }
        /// <summary>
        /// Evento disparado al dar un clic sobre el boton Aceptar edicion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkAceptar_Click(object sender, EventArgs e)
        {   //Obtenemos la referencia al boton que se dio click
            using (TextBox valor_referencia = (TextBox)gvReferencias.SelectedRow.FindControl("txtValorReferencia"))
            {   //Validando el Contenido del Control
                if (valor_referencia.Text != "")
                {   //Declarando Objeto de Operación
                    SAT_CL.Global.Referencia.ErrorReferencia result;
                    //Ejecutando Operación
                    result = SAT_CL.Global.Referencia.Guarda(SAT_CL.Global.Referencia.AccionReferencia.Editar, 0, Convert.ToInt32(gvReferencias.SelectedDataKey.Value), valor_referencia.Text, _id_registro, _id_tabla, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                    //Recargamos el grid view
                    if (result == SAT_CL.Global.Referencia.ErrorReferencia.SinError)
                    {   //Inicializando Control
                        inicializaGridView();
                    }
                    else
                    {   //Mostrando Error
                        lblError.Text = result == SAT_CL.Global.Referencia.ErrorReferencia.Editable ? "No se puede Editar este tipo de Referencia" : "";
                    }
                }
                else//Mostrando Error
                    lblError.Text = "La Referencia es Requerida";
            }
        }
        /// <summary>
        /// Evento disparado al dar un clic sobre el boton eliminar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEliminar_Click(object sender, EventArgs e)
        {   //Declarando Objeto de Operación
            SAT_CL.Global.Referencia.ErrorReferencia result;
            //Registramos en BD CENTRAL
            result = SAT_CL.Global.Referencia.Guarda(SAT_CL.Global.Referencia.AccionReferencia.Eliminar, 0, Convert.ToInt32(gvReferencias.SelectedDataKey.Value), "", _id_registro, _id_tabla, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
            //Gestionamos el resultado
            SAT_CL.Global.Referencia.Error(result, lblError);
        }
        /// <summary>
        /// Evento disparado al dar clic en el boton cancelar edicion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkCancelar_Click(object sender, EventArgs e)
        {   //Quitamos la fila de modo de edicion 
            TSDK.ASP.Controles.InicializaIndices(gvReferencias);
            //Seleccionando Nodo
            SAT_CL.Global.Referencia.NodoSeleccionado(trvReferencias.SelectedNode.Depth, Convert.ToInt32(trvReferencias.SelectedValue), _id_tabla, _id_registro, _id_compania, gvReferencias, false);
            //Limpiando Mensaje
            lblError.Text = "";
        }

        #endregion

        #region Metodos del control

        /// <summary>
        /// Metodo encargado de inicializar el control de usuario
        /// </summary>
        /// <param name="registro"></param>
        /// <param name="tabla"></param>
        /// <param name="compania"></param>
        public void InicializaControl(int registro, int tabla, int compania)
        {   //Estableciendo registro y tabla
            _id_registro = registro;
            _id_tabla = tabla;
            _id_compania = compania;
            _db = "";
            //Inicializamos la estructura del arbol
            inicializaArbol();
            //Inicializamos el gridView
            using(DataTable dt = SAT_CL.Global.Referencia.CargaReferenciasGeneral(registro, tabla, compania))
            {   //Validando que existan registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dt))
                {   //Cargando GridView
                    TSDK.ASP.Controles.CargaGridView(gvReferencias, dt, "", "");
                    //Deshabilitando Columna Editar
                    gvReferencias.Columns[4].Visible = false;
                }
                else//Inicializando GridView
                    TSDK.ASP.Controles.InicializaGridview(gvReferencias);
            }
            //Asignando viewstate
            asignaViewState();
        }
        /// <summary>
        /// Método encargado de asignar las variabkes viewstate
        /// </summary>
        private void asignaViewState()
        {   //Asignando los Valores
            ViewState["id_registro"] = _id_registro;
            ViewState["id_tabla"] = _id_tabla;
            ViewState["id_compania"] = _id_compania;
            ViewState["db"] = _db;
        }
        /// <summary>
        /// Método encargado de leer las variables viewstate
        /// </summary>
        private void leeViewState()
        {   //Recuperando los Valores
            _id_registro = Convert.ToInt32(ViewState["id_registro"]);
            _id_tabla = Convert.ToInt32(ViewState["id_tabla"]);
            _id_compania = Convert.ToInt32(ViewState["id_compania"]);
            _db = ViewState["db"].ToString();
        }
        /// <summary>
        /// Metodo encargado de inicializar la estructura del arbol en razon al numero de tabla asignado
        /// </summary>
        private void inicializaArbol()
        {   //Obteniendo Estructura del Arbol
            using (DataSet ds = SAT_CL.Global.Referencia.CargaEstructuraReferencia(SAT_CL.Global.Referencia.EstructuraArbolReferencia.Arbol, _id_tabla, _id_registro, _id_compania, 0, 0))
            {   //Inicializando Arbol
                TSDK.ASP.Controles.CreaArbolTresNiveles(trvReferencias, ds, "Table", "alias_tabla", "id_tabla", "Table1", "id_tabla", "nombre_grupo", "id_grupo", "Table2", "id_grupo", "nombre_tipo", "id_tipo");
            }
        }
        /// <summary>
        /// Inicializa el GridView
        /// </summary>
        private void inicializaGridView()
        {   //Limpiamos el textbox
            txtReferencia.Text = lblError.Text = "";
            //Ponemos al grid en modo de lectura
            TSDK.ASP.Controles.InicializaIndices(gvReferencias);
            //Creando objeto que almacena el DS
            using (DataSet ds = SAT_CL.Global.Referencia.CargaEstructuraReferencia(SAT_CL.Global.Referencia.EstructuraArbolReferencia.Arbol, _id_tabla, _id_registro, _id_compania, 0, 0))
            {   //Seleccionando Nodo
                SAT_CL.Global.Referencia.NodoSeleccionado(trvReferencias.SelectedNode.Depth, Convert.ToInt32(trvReferencias.SelectedValue), _id_tabla, _id_registro, _id_compania, gvReferencias, false);
            }
        }

        #endregion 
    }
}