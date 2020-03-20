using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
namespace SAT.Almacen
{
    public partial class Producto : System.Web.UI.Page
    {
        #region Eventos

        /// <summary>
        /// Evento que determina como inicia la página Producto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Aplicando seguridad de la forma
            SAT_CL.Seguridad.Forma.AplicaSeguridadForma(this, "content1", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
            //Valida si la página se esta cargando por primera vez.
            if (!Page.IsPostBack)
                //Invoca al método inicializaForma
                inicializaForma();


            //Invocando Método de Carga
            cargaControlAutocompletado();
            //COLOCA VALOR DEFAULT AL BOTÓN CANCELAR
            this.Form.DefaultButton = this.btnGuardar.UniqueID;
        }
        /// <summary>
        /// Evento que permite seleccionar y ejecutar acciones del menú.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbElementoMenu_Click(object sender, EventArgs e)
        {
            //Creación del objeto botonMenu que obtiene las opciones de los menú desplegable .
            LinkButton botonMenu = (LinkButton)sender;
            //Permite ejecutar acciones determinadas por cada opción del menú
            switch (botonMenu.CommandName)
            {
                //Si la elección del menú es la opción Nuevo
                case "Nuevo":
                    {
                        //Asigna a la variable de session estatus el estado del formulario en nuevo
                        Session["estatus"] = Pagina.Estatus.Nuevo;
                        //Asigna a la variable de session id_registro el valor de 0.
                        Session["id_registro"] = 0;
                        //Invoca al método inicializaForma
                        inicializaForma();
                        //Se realiza un enfoque al primer control 
                        txtSKU.Focus();
                        break;
                    }
                //Si la elección del menú es la opcion Abrir
                case "Abrir":
                    {
                        //Invoca al método inicializaAperturaRegistro();
                        inicializaAperturaRegistro(163, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);
                        break;
                    }

                //Si la elección del menú es la opción Guardar
                case "Guardar":
                    {
                        //Invoca al método guardaProducto();
                        guardarProducto();
                        break;
                    }
                //Si la elección del menú es la opción Editar
                case "Editar":
                    {
                        using (SAT_CL.Almacen.Producto oc = new SAT_CL.Almacen.Producto((int)Session["id_registro"]))
                        {
                                //Asigna a la variable session estaus el estado de la pagina nuevo
                                Session["estatus"] = Pagina.Estatus.Edicion;
                                //Invoca el método inicializaForma();
                                inicializaForma();
                                //Se realiza un enfoque al primer control 
                                txtSKU.Focus();
                            
                        }
                        break;
                    }
                //Si la elección del menú es la opción Eliminar
                case "Eliminar":
                    {
                        //Creación del objeto retorno
                        RetornoOperacion retorno = new RetornoOperacion();
                        //Invoca al constructor de la clase Producto para poder instanciar sus  métodos 
                        using (SAT_CL.Almacen.Producto producto = new SAT_CL.Almacen.Producto((int)Session["id_registro"]))
                        {
                            //Valida que exista el registro
                            if (producto.id_producto > 0)
                            {
                                //Asigna los valores al objeto retorno invocando el método DeshabilitarProducto.
                                retorno = producto.DeshabilitaProducto(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                            }
                        }
                        //Valida si la inserción a la base de datos se realizo correctamente
                        if (retorno.OperacionExitosa)
                        {
                            //A la variable de sessión estatus le asigna el estado de la pagina en modo Nuevo
                            Session["estatus"] = Pagina.Estatus.Nuevo;
                            //A la variable de session id_registro le asigna el valor insertado en la tabla Producto
                            Session["id_registro"] = 0;
                            //Invoca al método inicializa forma
                            inicializaForma();
                        }
                        //Mensaje de que la operación de deshabilitar registros se realizo correctamente.
                        TSDK.ASP.ScriptServer.MuestraNotificacion(this, retorno, ScriptServer.PosicionNotificacion.AbajoDerecha);
                        break;
                    }
                //Si la elección del menú es la opción Bitacora
                case "Bitacora":
                    {
                        //Invoca al método bitacora
                        inicializaBitacora(Session["id_registro"].ToString(), "163", "Producto");
                        break;
                    }
                //Si la elección del menú es la opcion Referencia
                case "Referencias":
                    {
                        //Invoca al método inicializaReferencia que muestra las observaciones hechas a un registro de Producto
                        inicializaReferenciaRegistro(Session["id_registro"].ToString(), "163",
                                                    ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString());
                        break;
                    }
            }
        }
        /// <summary>
        /// Evento que carga los valores del dropdownlist unidad medida acorde a la opción seleccionada dropdownlist TipoMedida
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTipoMedida_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Carga los valores al DropDownList ddlUnidadMedida
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlUnidadMedida, 44, "", Convert.ToInt32(ddlTipoMedida.SelectedValue), "", 0, "");

        }
        /// <summary>
        /// Habilita los controles de producto contenido
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkProductoContenido_CheckedChanged(object sender, EventArgs e)
        {
            //Valida el estado del check box (true)
            if (chkProductoContenido.Checked == true)
            {
                //Habilita los controles txtProductoContenido y txtCantidadContenido
                txtProductoContenido.Enabled = true;
                txtCantidadContenido.Enabled = true;
                //Invoca al constructor de la clase Producto y asigna como paramétro el valor de la variable de session id_registro
                using (SAT_CL.Almacen.Producto producto = new SAT_CL.Almacen.Producto((int)Session["id_registro"]))
                {
                    //Valida que exista el producto contenido
                    if (producto.id_producto_contenido > 0)
                    {
                        //Invoca al constructor de la clase Producto y obtiene el nombre del producto contenido.
                        using (SAT_CL.Almacen.Producto contenido = new SAT_CL.Almacen.Producto(producto.id_producto_contenido))
                        {
                            //Asigna a la caja de texto el valor correspondiente al producto contenido
                            txtProductoContenido.Text = contenido.descripcion + " ID:" + contenido.id_producto.ToString();
                        }
                        txtCantidadContenido.Text = producto.cantidad_contenido.ToString();
                    }
                    //Si no tiene producto contenido
                    else
                    {
                        //Limpia las cajas de texto
                        txtProductoContenido.Text = "";
                        txtCantidadContenido.Text = "";
                    }
                }
            }
            //en caso contrario de que no sea verdadero el estado del checkbox chkProductoContenido
            else
            {
                //Valida los estatus de la pagina
                switch ((Pagina.Estatus)Session["estatus"])
                {
                    //En caso de que el estado de la página sea nuevo y edicion
                    case Pagina.Estatus.Nuevo:
                    case Pagina.Estatus.Edicion:
                        {
                            //Deshabilita los controles
                            txtProductoContenido.Enabled =
                            txtCantidadContenido.Enabled = false;
                            //Asigna a la caja de texto el valor correspondiente al producto contenido
                            txtProductoContenido.Text = "SIN PRODUCTO ID: 0";
                            txtCantidadContenido.Text = "0.0";                                
                            
                            break;
                        }
                    //En caso de que el estado de  la página sea Lectura.
                    case Pagina.Estatus.Lectura:
                        {
                            //Deshabilita los controles
                            txtProductoContenido.Enabled =
                            txtCantidadContenido.Enabled = false;
                            //Invoca al constructor de la clase Producto y asigna como paramétro el valor de la variable de session id_registro
                            using (SAT_CL.Almacen.Producto producto = new SAT_CL.Almacen.Producto((int)Session["id_registro"]))
                            {
                                //Valida que exista el producto contenido
                                if (producto.id_producto_contenido > 0)
                                {
                                    //Invoca al constructor de la clase Producto y obtiene el nombre del producto contenido.
                                    using (SAT_CL.Almacen.Producto contenido = new SAT_CL.Almacen.Producto(producto.id_producto_contenido))
                                    {
                                        //Asigna a la caja de texto el valor correspondiente al producto contenido
                                        txtProductoContenido.Text = contenido.descripcion + " ID:" + contenido.id_producto.ToString();
                                    }
                                    txtCantidadContenido.Text = producto.cantidad_contenido.ToString();
                                }
                                //En caso de que no exista el producto contenido
                                else
                                {
                                    //Asigna a la caja de texto el valor correspondiente al producto contenido
                                    txtProductoContenido.Text = "SIN PRODUCTO ID: 0";
                                    txtCantidadContenido.Text = producto.cantidad_contenido.ToString();
                                }
                            }
                            break;
                        }
                }
            }                                
        }
        /// <summary>
        /// Evento que controla el almacenamiento del registro a la base de datos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            //Invoca al método guardarProducto
            guardarProducto();
        }
        /// <summary>
        /// Evento que anula acciones realizadas sobre el formulario (Inserción y edición de datos)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            //Acorde al estatus de la página valida:
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //En caso de que el estado de la pagina sea Nuevo
                case Pagina.Estatus.Nuevo:
                    {
                        //Asigna a la variable de sessión id_registro el valor de 0.
                        Session["id_registro"] = 0;
                        break;
                    }
                //En caso de que el estado de la página sea de edición
                case Pagina.Estatus.Edicion:
                    {
                        //Asigna a la variable de session estatus el valor de Lectura
                        Session["estatus"] = Pagina.Estatus.Lectura;
                        break;
                    }
            }
            //Invoca al método inicializaForma().
            inicializaForma();
        }
        /// <summary>
        /// Evento generado al Cambiar la Categoria
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Limpiamos Control de Producto
            txtProductoContenido.Text = "SIN PRODUCTO ID:0";
            //Cargamos Autocomplete
            cargaControlAutocompletado();
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método que permite inicializar el aspecto del formulario
        /// </summary>
        private void inicializaForma()
        {
            //Invoca al método habilitaControles.
            habilitaControles();
            //Invoca al método habilitaMenu.
            habilitaMenu();
            //Invoca al método cargaCatalogo.
            cargaCatalogo();
            //Invoca al método inicializaValores.
            inicializaValores();
        }
        /// <summary>
        /// Método que permite habilitar los controles en base a cada estado de la página
        /// </summary>
        private void habilitaControles()
        {
            //Valida cada estado de la página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //En caso de que el estado sea Nuevo o Edicio
                case Pagina.Estatus.Nuevo:
                    {
                        //Habilitara los controles a exepción de txtCompaniaEmisor
                        txtCompaniaEmisor.Enabled = false;
                        txtSKU.Enabled =
                        txtDescripcion.Enabled =
                        ddlTipoMedida.Enabled =
                        ddlUnidadMedida.Enabled =
                        ddlCategoria.Enabled =
                        txtFabricante.Enabled =
                        txtGarantia.Enabled = true;
                        ddlEstatus.Enabled = false;
                        txtPrecioEntrada.Enabled =
                        ddlMonedaEntrada.Enabled =
                        txtPrecioSalida.Enabled =
                        ddlMonedaSalida.Enabled =
                        txtCantidadMinima.Enabled =
                        txtCantidadMaxima.Enabled =
                        chkProductoContenido.Enabled =
                        chkSinInventario.Enabled =
                        txtCantidadMayoreo.Enabled =
                        txtPrecioMayoreo.Enabled = true;
                        txtProductoContenido.Enabled =
                        txtCantidadContenido.Enabled = false;
                        btnCancelar.Enabled =
                        btnGuardar.Enabled = true;
                        break;
                    }
                case Pagina.Estatus.Edicion:
                    {
                        //Habilitara los controles a exepción de txtCompaniaEmisor
                        txtCompaniaEmisor.Enabled = false;
                        txtSKU.Enabled =
                        txtDescripcion.Enabled =
                        ddlTipoMedida.Enabled =
                        ddlUnidadMedida.Enabled =
                        ddlCategoria.Enabled =
                        txtFabricante.Enabled =
                        txtGarantia.Enabled = true;
                        ddlEstatus.Enabled = false;
                        txtPrecioEntrada.Enabled =
                        ddlMonedaEntrada.Enabled =
                        txtPrecioSalida.Enabled =
                        ddlMonedaSalida.Enabled =
                        txtCantidadMinima.Enabled =
                        txtCantidadMaxima.Enabled =
                        chkProductoContenido.Enabled =
                        chkSinInventario.Enabled = 
                        txtCantidadMayoreo.Enabled = 
                        txtPrecioMayoreo.Enabled = true;
                        btnCancelar.Enabled =
                        btnGuardar.Enabled = true;
                        break;
                    }

                //En caso de que el estado sea Lectura
                case Pagina.Estatus.Lectura:
                    {
                        //Deshabilitara los controles 
                        txtCompaniaEmisor.Enabled = 
                        txtSKU.Enabled =
                        txtDescripcion.Enabled =
                        ddlTipoMedida.Enabled =
                        ddlUnidadMedida.Enabled = 
                        ddlCategoria.Enabled =
                        txtFabricante.Enabled =
                        txtGarantia.Enabled =
                        ddlEstatus.Enabled =
                        txtPrecioEntrada.Enabled =
                        ddlMonedaEntrada.Enabled =
                        txtPrecioSalida.Enabled =
                        ddlMonedaSalida.Enabled =
                        txtCantidadMayoreo.Enabled =
                        txtPrecioMayoreo.Enabled =
                        txtCantidadMinima.Enabled =
                        chkProductoContenido.Enabled =
                        chkSinInventario.Enabled =
                        txtCantidadMaxima.Enabled = 
                        txtProductoContenido.Enabled =
                        btnCancelar.Enabled =
                        btnGuardar.Enabled = 
                        txtCantidadContenido.Enabled = false;
                        break;
                    }
            }
        }
        /// <summary>
        /// Método que permite habilitar las opciones del ménu principal en base al estado de la página
        /// </summary>
        private void habilitaMenu()
        {
            //Valida cada estado de la página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //En caso de que el estado de la página sea Nuevo
                case Pagina.Estatus.Nuevo:
                    {
                        //Habilita y Deshabilita las opciones del ménu principal de la página
                        lkbNuevo.Enabled =
                        lkbAbrir.Enabled =
                        lkbGuardar.Enabled = true;
                        lkbEliminar.Enabled =
                        lkbEditar.Enabled =
                        lkbBitacora.Enabled =
                        lkbReferencias.Enabled = false;
                        break;
                    }
                case Pagina.Estatus.Edicion:
                    {
                        //Habilita y Deshabilita las opciones del ménu principal de la página
                        lkbNuevo.Enabled =
                        lkbAbrir.Enabled =
                        lkbGuardar.Enabled =
                        lkbEliminar.Enabled = true;
                        lkbEditar.Enabled =
                        lkbBitacora.Enabled =
                        lkbReferencias.Enabled = true;

                        break;
                    }
                case Pagina.Estatus.Lectura:
                    {
                        //Habilita y Deshabilita las opciones del ménu principal de la página
                        lkbNuevo.Enabled =
                        lkbAbrir.Enabled = true;
                        lkbGuardar.Enabled = false;
                        lkbEliminar.Enabled =
                        lkbEditar.Enabled =
                        lkbBitacora.Enabled =
                        lkbReferencias.Enabled = true;
                        break;
                    }
            }
        }
        /// <summary>
        /// Método que permite la carga de catalogos a los controles DrodDownList
        /// </summary>
        private void cargaCatalogo()
        {
            //Carga los valores al DropDownList Estatus
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlEstatus, "", 3154);
            //Carga los valores al DropDownList ddlMonedaEntrada
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlMonedaEntrada, "", 11);
            //Carga los valores al DropDownList ddlMonedaSalida
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlMonedaSalida, "", 11);
            //Carga los valores al DropDownList ddlTipoMedida
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipoMedida, 75, "", 4, "", 0, "");
            //Carga los valores al DropDownList ddlUnidadMedida
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlUnidadMedida, 44,"", Convert.ToInt32(ddlTipoMedida.SelectedValue),"",0,"");
            //CArga los valores al DropDownList ddlCategoria
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlCategoria, "", 3156);
        }
        /// <summary>
        /// Método que permite inicializar los valores de los controles en base a cada estado de la página
        /// </summary>
        private void inicializaValores()
        {
            //Valida cada estado de la página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //En caso de que el estado de la pagina sea nuevo.
                case Pagina.Estatus.Nuevo:
                    {
                        //Limpia los controles del formulario.
                        lblIdProducto.Text = "Por Asignar";
                        txtSKU.Text = "";
                        txtDescripcion.Text = "";
                        txtFabricante.Text = "";
                        txtGarantia.Text = "";
                        txtPrecioEntrada.Text = "";
                        txtPrecioSalida.Text = "";
                        txtCantidadMayoreo.Text = "";
                        txtPrecioMayoreo.Text = "";
                        txtCantidadMinima.Text = "";
                        txtCantidadMaxima.Text = "";
                        txtProductoContenido.Text = "SIN PRODUCTO ID: 0";
                        txtCantidadContenido.Text = "0.0";
                        chkProductoContenido.Checked = false;
                        chkSinInventario.Checked = false;
                        //Invoca al constructor de la clase CompaniaEmisorReceptor para obtener el nombre de la compañia
                        using (SAT_CL.Global.CompaniaEmisorReceptor emisor = new SAT_CL.Global.CompaniaEmisorReceptor(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                        {
                            //Asigina al txtCompania el nombre de la compañia del usuario.
                            txtCompaniaEmisor.Text = emisor.nombre + " ID:" + emisor.id_compania_emisor_receptor.ToString();
                        }
                        break;
                    }
                //En caso de que el estado de la página sea edición o Lectura
                case Pagina.Estatus.Edicion:                    
                case Pagina.Estatus.Lectura:
                    {
                        //Invoca al constructor de la clase Producto y asigna como paramétro el valor de la variable de session id_registro
                        using (SAT_CL.Almacen.Producto producto = new SAT_CL.Almacen.Producto((int)Session["id_registro"]))
                        {
                            //Valida que exista el registro
                            if (producto.id_producto > 0)
                            {
                                //Asigna al label Id el identificador del producto.
                                lblIdProducto.Text = producto.id_producto.ToString();
                                //Invoca al constructor de la clase CompaniaEmisor para obtener el nombre de las compañias.
                                using (SAT_CL.Global.CompaniaEmisorReceptor emisor = new SAT_CL.Global.CompaniaEmisorReceptor(producto.id_compania_emisor))
                                {
                                    //Valida que el registro exista
                                    if (emisor.id_compania_emisor_receptor > 0)
                                        //Asigna al control los nombres y id de cada compañia
                                        txtCompaniaEmisor.Text = string.Format("{0}   ID:{1}", emisor.nombre, emisor.id_compania_emisor_receptor);
                                }
                                txtSKU.Text = producto.sku;
                                txtDescripcion.Text = producto.descripcion;
                                txtFabricante.Text = producto.fabricante;
                                txtGarantia.Text = producto.garantia.ToString();
                                txtPrecioEntrada.Text = producto.precio_entrada.ToString();
                                txtPrecioSalida.Text = producto.precio_salida.ToString();
                                txtCantidadMayoreo.Text= producto.cantidad_mayoreo.ToString();
                                txtPrecioMayoreo.Text = producto.precio_salida_mayoreo.ToString();
                                txtCantidadMinima.Text = producto.cantidad_minima.ToString();
                                txtCantidadMaxima.Text = producto.cantidad_maxima.ToString();
                                ddlEstatus.SelectedValue = producto.id_estatus.ToString();
                                ddlMonedaEntrada.SelectedValue = producto.id_moneda_entrada.ToString();
                                ddlMonedaSalida.SelectedValue = producto.id_moneda_salida.ToString();
                                ddlCategoria.SelectedValue = producto.categoria.ToString();
                                chkSinInventario.Checked = producto.bit_sin_inventario;
                                //Invoca a la clase unidadMedida
                                using (SAT_CL.Global.UnidadMedida unidad = new SAT_CL.Global.UnidadMedida(producto.id_unidad))
                                {
                                    //ASigna valores al dropdownlist TipoMedida
                                    ddlTipoMedida.SelectedValue = unidad.id_tipo_unidad_medida.ToString();
                                    //Carga los valores al DropDownList ddlUnidadMedida
                                    SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlUnidadMedida, 44, "", Convert.ToInt32(ddlTipoMedida.SelectedValue), "", 0, "");
                                    ddlUnidadMedida.SelectedValue = producto.id_unidad.ToString();                                                                           
                                }
                                //Valida que exista el producto contenido
                                if (producto.id_producto_contenido > 0)
                                {
                                    //Invoca al método que valida el check box chkProductocontenido
                                    validaCheck();
                                    //Invoca al constructor de la clase Producto y obtiene el nombre del producto contenido.
                                    using (SAT_CL.Almacen.Producto contenido = new SAT_CL.Almacen.Producto(producto.id_producto_contenido))
                                    {
                                        //Asigna a la caja de texto el valor correspondiente al producto contenido
                                        txtProductoContenido.Text = contenido.descripcion + " ID:" + contenido.id_producto.ToString();
                                    }
                                    txtCantidadContenido.Text = producto.cantidad_contenido.ToString();
                                }
                                //En caso contrario
                                else
                                {
                                    //Invoca al método que valida el check box chkProductocontenido
                                    validaCheck();
                                    //Asigna a la caja de texto el valor correspondiente al producto contenido
                                    txtProductoContenido.Text ="SIN PRODUCTO ID: 0";
                                    txtCantidadContenido.Text = producto.cantidad_contenido.ToString();
                                }
                            }
                        }

                        break;
                    }
            }

        }
        /// <summary>
        /// Método que permite almacenar los datos que se ingresaron en los controles del formulario
        /// </summary>
        private void guardarProducto()
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creació del objeto resultado1
            RetornoOperacion resultado1= minimosMaximos();
            //Creación del objeto resultado2
            RetornoOperacion resultado2 = productoContenido();
            
            if (resultado1.OperacionExitosa && resultado2.OperacionExitosa)
            {
                //Valida cada estado de la página
                switch ((Pagina.Estatus)Session["estatus"])
                {
                    //En caso de que el estado de la página sea nuevo
                    case Pagina.Estatus.Nuevo:
                        {
                            //Asigna al objeto retorno los datos ingresados invocando al método inserción de la clase Producto
                            retorno = SAT_CL.Almacen.Producto.InsertaProducto(
                                ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,Cadena.VerificaCadenaVacia(txtSKU.Text.ToUpper(),""), txtDescripcion.Text.ToUpper(),
                                Convert.ToInt32(ddlUnidadMedida.SelectedValue), Convert.ToInt32(ddlCategoria.SelectedValue), txtFabricante.Text.ToUpper(),
                                Convert.ToInt32(Cadena.VerificaCadenaVacia(txtGarantia.Text, "0")), SAT_CL.Almacen.Producto.Estatus.Activo,
                                Convert.ToByte(ddlMonedaEntrada.SelectedValue), Convert.ToDecimal(Cadena.VerificaCadenaVacia(txtPrecioEntrada.Text, "0.0")), Convert.ToByte(ddlMonedaSalida.SelectedValue),
                                Convert.ToDecimal(Cadena.VerificaCadenaVacia(txtPrecioSalida.Text, "0.0")), Convert.ToDecimal(Cadena.VerificaCadenaVacia(txtCantidadMayoreo.Text, "0.0")),
                                Convert.ToDecimal(Cadena.VerificaCadenaVacia(txtPrecioMayoreo.Text, "0.0")),
                                Convert.ToDecimal(Cadena.VerificaCadenaVacia(txtCantidadMinima.Text, "0.0")), Convert.ToDecimal(Cadena.VerificaCadenaVacia(txtCantidadMaxima.Text, "0.0")),
                                Convert.ToInt32(Cadena.VerificaCadenaVacia(Cadena.RegresaCadenaSeparada(txtProductoContenido.Text, "ID:", 1), "0")),
                                Convert.ToDecimal(Cadena.VerificaCadenaVacia(txtCantidadContenido.Text, "0")),
                                Convert.ToBoolean(chkSinInventario.Checked), 
                                ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                            break;
                        }
                    //En caso de que el estado de la página sea edicion
                    case Pagina.Estatus.Edicion:
                        {
                            //Invoca al constructor de la clase Producto para poder instanciar sus  métodos 
                            using (SAT_CL.Almacen.Producto producto = new SAT_CL.Almacen.Producto((int)Session["id_registro"]))
                            {
                                if (producto.id_producto > 0)
                                {
                                    //Asigna al objeto retorno los datos ingresados en los controles invocando el método de edición de la clase Producto
                                    retorno = producto.EditaProducto(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,Cadena.VerificaCadenaVacia(txtSKU.Text.ToUpper(),""), txtDescripcion.Text.ToUpper(),
                                Convert.ToInt32(ddlUnidadMedida.SelectedValue), Convert.ToInt32(ddlCategoria.SelectedValue), txtFabricante.Text.ToUpper(),
                                Convert.ToInt32(Cadena.VerificaCadenaVacia(txtGarantia.Text, "0")), SAT_CL.Almacen.Producto.Estatus.Activo,
                                Convert.ToByte(ddlMonedaEntrada.SelectedValue), Convert.ToDecimal(Cadena.VerificaCadenaVacia(txtPrecioEntrada.Text, "0.0")), Convert.ToByte(ddlMonedaSalida.SelectedValue),
                                Convert.ToDecimal(Cadena.VerificaCadenaVacia(txtPrecioSalida.Text, "0.0")), Convert.ToDecimal(Cadena.VerificaCadenaVacia(txtCantidadMayoreo.Text, "0.0")),
                                Convert.ToDecimal(Cadena.VerificaCadenaVacia(txtPrecioMayoreo.Text, "0.0")),
                                Convert.ToDecimal(Cadena.VerificaCadenaVacia(txtCantidadMinima.Text, "0.0")), Convert.ToDecimal(Cadena.VerificaCadenaVacia(txtCantidadMaxima.Text, "0.0")),
                                Convert.ToInt32(Cadena.VerificaCadenaVacia(Cadena.RegresaCadenaSeparada(txtProductoContenido.Text, "ID:", 1), "0")),
                                Convert.ToDecimal(Cadena.VerificaCadenaVacia(txtCantidadContenido.Text, "0")),
                                Convert.ToBoolean(chkSinInventario.Checked), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);


                                }
                            }
                            break;
                        }
                }
                //Valida si la inserción a la base de datos se realizo correctamente
                if (retorno.OperacionExitosa)
                {
                    //A la variable de sessión estatus le asigna el estado de la pagina en modo lectura
                    Session["estatus"] = Pagina.Estatus.Lectura;
                    //A la variable de session id_registro le asigna el valor insertado en la tabla Producto
                    Session["id_registro"] = retorno.IdRegistro;
                    //Invoca al método inicializa forma
                    inicializaForma();
                }
                //Muestra el mensaje de error
                TSDK.ASP.ScriptServer.MuestraNotificacion(this, retorno, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
            //Valida la operación exitosa  del objeto resultado1
            if (!resultado1.OperacionExitosa)
            {
                //Muestra el mensaje de error.
                TSDK.ASP.ScriptServer.MuestraNotificacion(this, resultado1, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
            //Valida la operación exitosa del objeto resultado2
            else if(!resultado2.OperacionExitosa)
            {
            //Muestra el mnesaje de error.
            TSDK.ASP.ScriptServer.MuestraNotificacion(this, resultado2, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
        }
        /// <summary>
        ///  Método que permie la consulta del historial de modificicaciones realizadas a un registro 
        /// </summary>
        /// <param name="idRegistro">ID que permite identificar un registro de Producto</param>
        /// <param name="idTabla">Identificador de la tabla Producto</param>
        /// <param name="Titulo">Encabezado de la ventana Bitacora</param>
        private void inicializaBitacora(string idRegistro, string idTabla, string Titulo)
        {
            //Creación de la variable url, que almacena una ruta de ubicación de la ventana, que contendra la bitacora de un registros de  Producto.
            string url = Cadena.RutaRelativaAAbsoluta("~/Almacen/Producto.aspx",
                "~/Accesorios/BitacoraRegistro.aspx?idT=" + idTabla + "&idR=" + idRegistro + "&tB=" + Titulo);
            //Variable que almacena la resolucion de la ventana bitacora
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Invoca el método de la clase ScriptServer que abrira una nueva ventana acorde a una ubicacion, un titulo de la ventana, una configuracion de dimensiones.
            ScriptServer.AbreNuevaVentana(url, "Bitacora  Producto", configuracion, Page);
        }
        /// <summary>
        /// Método que valida las cantidades Mínimas y Máximas
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion minimosMaximos()
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion(1);
            //Compara la cantidad mínima : si es vacia asigna un cero y la compara con la cantidad máxima
            if (Convert.ToDecimal(Cadena.VerificaCadenaVacia(txtCantidadMinima.Text,"0.0")).CompareTo(Convert.ToDecimal(Cadena.VerificaCadenaVacia(txtCantidadMaxima.Text,"0.0")))>0)
            {
                //Asigna al objeto retorno el mensaje de error si no se cumple la validación de las cantidades mínimas y máximas
                retorno = new RetornoOperacion("La cantidad máxima de stock debe ser mayor o igual a la cantidad mínima");
            }
            //Retorna el objeto retorno al método.
            return retorno;
        }
        /// <summary>
        /// Método que valida el producto contenido. (Que la cantidad sea mayor a cero siempre y cuando exista el producto contenido)
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion productoContenido()
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion(1);
            //Valida solo cuando el check box este activado
            if (chkProductoContenido.Checked == true)
            {
                //Valida que el campo de cantidad producto contenido (si es igual a cero y tiene prodcuto contenido)
                if ((Convert.ToDecimal(Cadena.VerificaCadenaVacia(txtCantidadContenido.Text, "0.0")) == 0) && (Convert.ToInt32(Cadena.VerificaCadenaVacia(Cadena.RegresaCadenaSeparada(txtProductoContenido.Text, "ID:", 1), "0")) != 0))
                {
                    //ASigna al objeto retorno el mensaje de error.
                    retorno = new RetornoOperacion("El campo de cantidad no puede ser cero.");
                }
                //Valida si la cantidad es mayor a cero pero no tiene producto contenido 
                else if ((Convert.ToDecimal(Cadena.VerificaCadenaVacia(txtCantidadContenido.Text, "0.0")) != 0) && (Convert.ToInt32(Cadena.VerificaCadenaVacia(Cadena.RegresaCadenaSeparada(txtProductoContenido.Text, "ID:", 1), "0")) == 0))
                {
                    //Asigna al objeto retorno el mensaje de error
                    retorno = new RetornoOperacion("El campo de producto contenido no puede estar vacio.");
                }
            }

            return retorno;
        }
        /// <summary>
        /// Metodo encargado de Cargar el Catalogo de Autocompletado
        /// </summary>
        private void cargaControlAutocompletado()
        {
            //Generamos Sript
            string script =
            @"<script type='text/javascript'>
              $('#" + this.txtProductoContenido.ClientID + @"').autocomplete({ source:'../WebHandlers/AutoCompleta.ashx?id=" + 39 + @" &param=" + ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString() + @"&param2="+ddlCategoria.SelectedValue +@"'});
              </script>";

            //Registrando el script sólo para los paneles que producirán actualización del mismo
            System.Web.UI.ScriptManager.RegisterStartupScript(this.Page, this.GetType(),"AutocompleteRecursos", script, false);
        }
        /// <summary>
        /// Método que valida el estatus del CheckBox ProductoContenido
        /// </summary>
        private void validaCheck()
        {
            //Valida cada estado de la página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //En caso de que el estado de la pagina sea nuevo.
                case Pagina.Estatus.Edicion:
                    {
                        //Invoca al constructor de la clase Producto y asigna como paramétro el valor de la variable de session id_registro
                        using (SAT_CL.Almacen.Producto producto = new SAT_CL.Almacen.Producto((int)Session["id_registro"]))
                        {
                            //Valida, si el id_producto_contenido es igual a cero(no tiene producto contenido).
                            if (producto.id_producto_contenido == 0)
                            {
                                //Deshabilita los controles y el check box quedara sin seleccionar
                                chkProductoContenido.Checked = false;
                                txtProductoContenido.Enabled = false;
                                txtCantidadContenido.Enabled = false;
                            }
                            //En caso contrario (tiene producto contenido)
                            else
                            {
                                //Habilita los controles y el check box quedara  seleccionado
                                chkProductoContenido.Checked = true;
                                txtProductoContenido.Enabled = true;
                                txtCantidadContenido.Enabled = true;
                            }
                        }
                        break;
                    }
                //Si la pagina se encuentra en modo lectura
                case Pagina.Estatus.Lectura:
                    {
                        //Invoca al constructor de la clase Producto y asigna como paramétro el valor de la variable de session id_registro
                        using (SAT_CL.Almacen.Producto producto = new SAT_CL.Almacen.Producto((int)Session["id_registro"]))
                        {
                            //Valida, si el id_producto_contenido es igual a cero(no tiene producto contenido).
                            if (producto.id_producto_contenido == 0)                            
                                //Check box quedara sin seleccionar
                                chkProductoContenido.Checked = false;                            
                            //En caso contrario
                            else                            
                                //Check box quedara seleccionado
                                chkProductoContenido.Checked = true;                            
                        }
                        break;
                    }
            }
        }
        /// <summary>
        /// Método que permite la busqueda de registros en la base de datos pertenecientes a la tabla Producto
        /// </summary>
        /// <param name="idTabla">Id que identifica a la tabla Producto registrada en la base de datos</param>
        /// <param name="idCompania"> Id que identifica a una compañia</param>
        private void inicializaAperturaRegistro(int idTabla, int idCompania)
        {
            //Asigna a la variable de session id_tabla el valor del parametro idTabla
            Session["id_tabla"] = idTabla;
            //Asigna valores a la variable url con la ruta de ubicacion de la ventana Abrir registros con los datos obtenidos de la tabla Producto
            string url = Cadena.RutaRelativaAAbsoluta("~/Almacen/Producto.aspx", "~/Accesorios/AbrirRegistro.aspx?P1=" + idCompania.ToString());
            //Define las dimensiones de la ventana Abrir registros de Producto
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Invoca al método de la variable ScripServer que abrira una nueva ventana con los registros pertenecientes a la tabla Producto
            ScriptServer.AbreNuevaVentana(url, "Abrir Registro Producto", configuracion, Page);
        }
        /// <summary>
        /// Método encargado de inicializr la referencias a un registro
        /// </summary>
        /// <param name="id_registro">Id que permite identificar un registro de la tabla Producto</param>
        /// <param name="id_tabla">Id que permite identificar a la tabla Producto en la base de datos</param>
        /// <param name="id_compania">Id que permite identificar a que compania pertenece </param>
        private void inicializaReferenciaRegistro(string id_registro, string id_tabla, string id_compania)
        {
            //Asigna a la variable urlDestino una ruta de ubucación de la ventana de Referencia de la tabla Producto
            string urlDestino = Cadena.RutaRelativaAAbsoluta("~/Almacen/Producto.aspx", "~/Accesorios/ReferenciaRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idC=" + id_compania);
            //Invoca al método de la clase ScripServer que abrira una nueva ventana con los valores de Referencia de la tabla Producto
            ScriptServer.AbreNuevaVentana(urlDestino, "Referencia de Registros Producto", 800, 500, false, false, false, true, true, Page);
        }
        
        #endregion
    }
}