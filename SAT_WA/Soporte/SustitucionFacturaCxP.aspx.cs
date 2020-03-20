using SAT_CL;
using SAT_CL.FacturacionElectronica;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.Soporte
{
    public partial class SustitucionFacturaCxP : System.Web.UI.Page
    {

        #region Atributos
        int TipoServicio;
        int tipo;
        #endregion

        #region Eventos

        /// <summary>
        /// Evento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Aplicando seguridad de la forma
            SAT_CL.Seguridad.Forma.AplicaSeguridadForma(this, "content1", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

            //Validando si se Produjo un PostBack
            if (!Page.IsPostBack)

                //Invocando Método de Inicialización
                inicializaPagina();
        }
        /// <summary>
        /// Evento Buscar Facturas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            //Invocando Método de Busqueda
            buscaFacturas();
        }
        /// <summary>
        /// Evento Buscar Facturas Modal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscarModal_Click(object sender, EventArgs e)
        {
            //Invocando Método de Busqueda
            buscaFacturasModal();
        }     
        /// <summary>
        /// Evento Guarda Soporte Tecnico Uwu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucSoporteTecnico_ClickAceptarSoporte(object sender, EventArgs e)
        {
            RetornoOperacion resultado = new RetornoOperacion();
            if (Convert.ToInt32(ViewState["idTipo"]) == 1)
            {
                //RetornoOperacion resultado = RegresaDeposito();//ucSoporte.GuardaSoporte();
                resultado = SustitucionFacturaCXPExistente();
                //Si no hay errores
                if (resultado.OperacionExitosa)
                {
                    //Actualizando lista de vales de diesel
                    resultado = ucSoporte.GuardaSoporte();
                }
                //Cerrando ventana modal
                ScriptServer.AlternarVentana(this, "Soporte", "contenidoSoporteTecnicoModal", "contenidoSoporteTecnico");
                ScriptServer.AlternarVentana(this, "SustitucionExistente", "contenedorVentanaSustitucionExistente", "VentanaSustitucionExistente");
                buscaFacturas();
                Controles.InicializaIndices(gvFacturas);
                Controles.InicializaIndices(gvAnticiposProveedor);
                ViewState["idTipo"] = this.tipo = 0;
            }
            else
            {
                //RetornoOperacion resultado = RegresaDeposito();//ucSoporte.GuardaSoporte();
                resultado = SustitucionFacturaCxPNuevo();
                //Si no hay errores
                if (resultado.OperacionExitosa)
                {
                    //Actualizando lista de vales de diesel
                    resultado = ucSoporte.GuardaSoporte();
                }
                //Cerrando ventana modal
                ScriptServer.AlternarVentana(this, "Soporte", "contenidoSoporteTecnicoModal", "contenidoSoporteTecnico");
                ScriptServer.AlternarVentana(this, "SustitucionNueva", "contenedorVentanaSustitucionNueva", "VentanaSustitucionNueva");
                Controles.InicializaIndices(gvFacturas);
                Controles.InicializaIndices(gvAnticiposProveedor);
                buscaFacturas();
            }
        }
        protected void wucSoporteTecnico_ClickCancelarSoporte(object sender, EventArgs e)
        {
            ScriptServer.AlternarVentana(this, "Soporte", "contenidoSoporteTecnicoModal", "contenidoSoporteTecnico");
            Controles.InicializaIndices(gvFacturas);
            Controles.InicializaIndices(gvAnticiposProveedor);
        }
        #region Eventos Validación SAT
        /// <summary>
        /// Evento click en ventana modal de confirmación de validación de factura
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnValidacionSAT_Click(object sender, EventArgs e)
        {
            //Determinando respuesta del usuario
            switch (((Button)sender).CommandName)
            {
                case "Descartar":
                    //Ocultando ventana modal
                    ScriptServer.AlternarVentana(btnCanelarValidacion, "DatosValidacionSAT", "contenidoResultadoConsultaSATModal", "contenidoResultadoConsultaSAT");
                    break;
                case "Continuar":
                    //Realizando proceso de guardado de factura de proveedor
                    guardaXML();

                    //Ocultando ventana modal
                    ScriptServer.AlternarVentana(btnAceptarValidacion, "DatosValidacionSAT", "contenidoResultadoConsultaSATModal", "contenidoResultadoConsultaSAT");
                    break;
            }
        }

        #endregion


        #endregion

        #region Métodos
        /// <summary>
        /// Método Privado encargado de Inicializar la Forma
        /// </summary>
        private void inicializaPagina()
        {
            //Cargando Catalogos
            cargaCatalogos();
            //Inicializando GridViews
            Controles.InicializaGridview(gvFacturas);
            Controles.InicializaGridview(gvAnticiposProveedor);
        }
        /// <summary>
        /// Método Privado encargado de Cargar los Catalogos de la Forma
        /// </summary>
        private void cargaCatalogos()
        {
            //Cargando Catalogos de Tamaño de GridView
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoFacturas, "", 26);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoFI, "", 26);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(lbxEstatusClasificacion, "", 58);
        }
        /// <summary>
        /// Método Privado encargado de Inicializar los Valores
        /// </summary>
        private void inicializaValores()
        {
            //Instanciando Factura
            using (SAT_CL.CXP.FacturadoProveedor fp = new SAT_CL.CXP.FacturadoProveedor(Convert.ToInt32(Session["id_registro"])))
            {
                //Validando que exista la Factura
                if (fp.id_factura != 0)
                {
                    /** Encabezado de la Factura **/
                    lblId.Text = fp.id_factura.ToString();
                    txtTotal.Text = string.Format("{0:#,###,###,###.00}", fp.total_factura);
                }
            }
        }
        /// <summary>
        /// <summary>
        /// Método Privado encargado de Buscar las Facturas
        /// </summary>
        private void buscaFacturas()
        {
            //Obteniendo Valor
            using (DataTable dtFacturas = SAT_CL.CXP.Reportes.ObtieneFacturasPorPagar(
            Convert.ToInt32(Cadena.RegresaElementoNoVacio(Cadena.RegresaCadenaSeparada(txtCliente.Text, "ID:", 1, ""))),
            Cadena.RegresaElementoNoVacio(txtUUID.Text, ""),
            Convert.ToString(Cadena.RegresaElementoNoVacio(txtFolio.Text, "0")),
            Convert.ToString(Cadena.RegresaElementoNoVacio(txtSerie.Text, "0")), Cadena.VerificaCadenaVacia(lbxEstatusClasificacion.Text, "0")))
            {
                //Validando que existan Registros
                if (Validacion.ValidaOrigenDatos(dtFacturas))
                {
                    //Cargando GridView
                    Controles.CargaGridView(gvFacturas, dtFacturas, "Id-MontoTotal-MontoAplicado", "", true, 2);

                    //Añadiendo Tabla a Session
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtFacturas, "Table");
                }
                else
                {
                    //Inicializando GridView
                    Controles.InicializaGridview(gvFacturas);

                    //Eliminando Tabla de Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                }
            }

        }
        /// <summary>
        /// <summary>
        /// Método Privado encargado de Buscar las Facturas Modal
        /// </summary>
        private void buscaFacturasModal() {
            using (SAT_CL.CXP.FacturadoProveedor Facturas = new SAT_CL.CXP.FacturadoProveedor(Convert.ToInt32(gvFacturas.SelectedDataKey["Id"])))
            {

                //Obteniendo Valor
                using (DataTable dtFacturas = SAT_CL.CXP.Reportes.ObtieneFacturasPorPagar(
                Facturas.id_compania_proveedor,
                Cadena.RegresaElementoNoVacio(txtUUIDModal.Text, ""),
                Convert.ToString(Cadena.RegresaElementoNoVacio(txtFolioModal.Text, "0")),
                Convert.ToString(Cadena.RegresaElementoNoVacio(txtSerieModal.Text, "0")), Convert.ToString(2)))
                {
                    //Validando que existan Registros
                    if (Validacion.ValidaOrigenDatos(dtFacturas))
                    {
                        Controles.InicializaGridview(gvAnticiposProveedor);
                        //Cargando GridView
                        Controles.CargaGridView(gvAnticiposProveedor, dtFacturas, "Id-MontoTotal-MontoAplicado", "", true, 2);

                        //Añadiendo Tabla a Session
                        Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtFacturas, "Table");
                    }
                    else
                    {
                        //Inicializando GridView
                        Controles.InicializaGridview(gvAnticiposProveedor);

                        //Eliminando Tabla de Session
                        Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                    }


                }
            }
        }
        /// <summary>
        /// <summary>
        /// Método Privado encargado de abrir ventana modal soporte tecnico para validacion sat
        /// </summary>
        private void InicializaVentanaModal(){
            //Declarando objeto de resultado
            RetornoOperacion retorno = new RetornoOperacion();
            string Mensaje = "";
            //Validando que existen Registros en el gridView
            if (gvFacturas.DataKeys.Count > 0)
            {
   
                using (SAT_CL.CXP.FacturadoProveedor FacturasAnt = new SAT_CL.CXP.FacturadoProveedor(Convert.ToInt32(gvFacturas.SelectedDataKey["Id"])))
                {
                    using (SAT_CL.CXP.FacturadoProveedor FacturasAho = new SAT_CL.CXP.FacturadoProveedor(Convert.ToInt32(Session["id_registro"])))
                    {
                        if (string.IsNullOrEmpty(FacturasAnt.folio))
                        {
                            Mensaje = "La Factura con Uuid: " + FacturasAnt.uuid + ",fue sustituida por: " + FacturasAho.uuid;
                        }
                        else
                            Mensaje = "La Factura con folio: " + FacturasAnt.folio + ",fue sustituida por: " + FacturasAho.folio;

                    }
                }
                ucSoporte.InicializaControlUsuario(Mensaje, 4, "");
                //Mostrando ventana modal correspondiente      
                alternaVentanaModal("Soporte", this.Page);
            }
            else
            {
                retorno = new RetornoOperacion("Seleccione una factura");
                //Mostrando Mensaje
                TSDK.ASP.ScriptServer.MuestraNotificacion(this, retorno, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
        }
        /// <summary>
        /// <summary>
        /// Método encargado sustitucion Factura Agregar nueva Factura
        /// </summary>
        public RetornoOperacion SustitucionFacturaCxPNuevo()
        {
            //Declarando objeto de resultado
            RetornoOperacion retorno = new RetornoOperacion();

            //Instanciando Para editar la factura a estatus 7
            using (SAT_CL.CXP.FacturadoProveedor facturadoP = new SAT_CL.CXP.FacturadoProveedor(Convert.ToInt32(gvFacturas.SelectedDataKey["Id"])))
            {
                if (facturadoP.id_factura > 0)
                {
                    using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                    {
                        //Asigna al objeto retorno los datos ingresados en los controles invocando el método de edición de la clase ordenCompra
                        int Factura1 = Convert.ToInt32(gvFacturas.SelectedDataKey["MontoTotal"]);
                        decimal Factura2 = Convert.ToDecimal(txtTotal.Text);
                        int MontoAplicado = Convert.ToInt32(gvFacturas.SelectedDataKey["MontoAplicado"]);
                        if (Factura2 >= Factura1)
                        {
                            retorno = facturadoP.EditaFacturadoProveedor(7,
                                                   ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                        }
                        else if (Factura2 >= MontoAplicado)
                        {
                            retorno = facturadoP.EditaFacturadoProveedor(7,
                                                   ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                        }         
                        if (retorno.OperacionExitosa)
                        {
                            //Instanciando Para editar la fichas por el nuevo id de factura
                            using (DataTable dtFichasFacturas = SAT_CL.CXC.FichaIngresoAplicacion.ObtieneAplicacionesFacturas(72, Convert.ToInt32(gvFacturas.SelectedDataKey["Id"]), 0))
                            {
                                if (Validacion.ValidaOrigenDatos(dtFichasFacturas))
                                {
                                    List<DataRow> SustitucionFacturasFichas = (from DataRow dep in dtFichasFacturas.AsEnumerable()
                                                                               select dep).ToList();
                                    if (SustitucionFacturasFichas.Count > 0)
                                    {
                                        foreach (DataRow row in SustitucionFacturasFichas)
                                        {
                                            using (SAT_CL.CXC.FichaIngresoAplicacion Ficha = new SAT_CL.CXC.FichaIngresoAplicacion(Convert.ToInt32(row["Id"])))
                                            {
                                                retorno = Ficha.EditarFichaIngresoAplicacion(Convert.ToInt32(lblId.Text),
                                                  ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                if (retorno.OperacionExitosa)
                                                {

                                                    //Instanciando Para editar la relacion por el nuevo id de factura
                                                    using (DataTable dtRelaciones = SAT_CL.CXP.FacturadoProveedorRelacion.ObtieneRelacionesFactura(Convert.ToInt32(gvFacturas.SelectedDataKey["Id"])))
                                                    {
                                                        if (Validacion.ValidaOrigenDatos(dtRelaciones))
                                                        {
                                                            List<DataRow> SustitucionFacturasRelacion = (from DataRow dep in dtRelaciones.AsEnumerable()
                                                                                                         select dep).ToList();
                                                            if (SustitucionFacturasRelacion.Count > 0)
                                                            {
                                                                foreach (DataRow rows in SustitucionFacturasRelacion)
                                                                {
                                                                    using (SAT_CL.CXP.FacturadoProveedorRelacion Relacion = new SAT_CL.CXP.FacturadoProveedorRelacion(Convert.ToInt32(rows["Id"])))
                                                                    {
                                                                        retorno = Relacion.EditarFacturadoProveedorRelacion(Convert.ToInt32(lblId.Text),
                                                                          ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                                                        if (retorno.OperacionExitosa)
                                                                        {

                                                                            //Instanciando Para editar la factura actual dependiendo estaus 
                                                                            using (SAT_CL.CXP.FacturadoProveedor facturadoPr = new SAT_CL.CXP.FacturadoProveedor(Convert.ToInt32(lblId.Text)))
                                                                            {

                                                                                if (facturadoPr.id_factura > 0)
                                                                                {
                                                                                
                                                                                    int Estatus;

                                                                                    if (Factura2 > MontoAplicado)
                                                                                    {
                                                                                        Estatus = 4;
                                                                                        //Asigna al objeto retorno los datos ingresados en los controles invocando el método de edición de la clase ordenCompra
                                                                                        retorno = facturadoPr.EditaFacturadoProveedor(Convert.ToByte(Estatus),
                                                                                         ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                                                    }
                                                                                    else if (Factura2 == MontoAplicado)
                                                                                    {
                                                                                        Estatus = 5;
                                                                                        //Asigna al objeto retorno los datos ingresados en los controles invocando el método de edición de la clase ordenCompra
                                                                                        retorno = facturadoPr.EditaFacturadoProveedor(Convert.ToByte(Estatus),
                                                                                         ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                                                    }
                                                                                }

                                                                            }
                                                                        }
                                                                     }
                                                                }

                                                            }

                                                        }
                                                    }
                                                }
                                             }
                                        }

                                    }

                                }
                                else
                                {
                                    retorno = new RetornoOperacion("No se puede sustituir la factura ya que el monto de la nueva factura es menor y no cuenta con valor para cubrir montos aplicados");
                                }
                            }

                        }
                        if (retorno.OperacionExitosa)
                        {
                            //Creando liga entre facturas
                            retorno = SAT_CL.Global.Referencia.InsertaReferencia(Convert.ToInt32(lblId.Text), 72, SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(0, 72, "Refacturacion", 0, "Anticipos Proveedor"), gvFacturas.SelectedDataKey["Id"].ToString(), Fecha.ObtieneFechaEstandarMexicoCentro(), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                            //Validando Operación Exitosa
                            if (retorno.OperacionExitosa)
                            {
                                //Completando Transacción
                                trans.Complete();
                            }
                        }
                    }
                }
                else
                {
                    retorno = new RetornoOperacion("No se puede sustituir Factura");

                }
                //Mostrando Mensaje
                TSDK.ASP.ScriptServer.MuestraNotificacion(this, retorno, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
            //Devolviendo Resultado Obtenido
            return retorno;
        }
        /// <summary>
        /// <summary>
        /// Método encargado sustitucion Factura Existente
        /// </summary>
        public RetornoOperacion SustitucionFacturaCXPExistente()
        {
            //Declarando objeto de resultado
            RetornoOperacion retorno = new RetornoOperacion();
            //Validando que existen Registros en el gridView
            if (gvAnticiposProveedor.DataKeys.Count > 0)
            {
                //Seleccionando Fila del grid view para editar los valores
                using (SAT_CL.CXP.FacturadoProveedor FacturasE = new SAT_CL.CXP.FacturadoProveedor(Convert.ToInt32(gvAnticiposProveedor.SelectedDataKey["Id"])))
                {
                    using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                    {
                        //Instanciando Para editar la factura a estatus 7
                        using (SAT_CL.CXP.FacturadoProveedor facturadoP = new SAT_CL.CXP.FacturadoProveedor(Convert.ToInt32(gvFacturas.SelectedDataKey["Id"])))
                        {
                            if (facturadoP.id_factura > 0)
                            {
                                int Factura1 = Convert.ToInt32(gvFacturas.SelectedDataKey["MontoTotal"]);
                                int Factura2 = Convert.ToInt32(gvAnticiposProveedor.SelectedDataKey["MontoTotal"]);
                                int MontoAplicado = Convert.ToInt32(gvFacturas.SelectedDataKey["MontoAplicado"]);

                                if (Factura2 >= Factura1)
                                {

                                    //Asigna al objeto retorno los datos ingresados en los controles invocando el método de edición de la clase ordenCompra
                                    retorno = facturadoP.EditaFacturadoProveedor(7, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                }
                                else if (Factura2 >= MontoAplicado)
                                {
                                    //Asigna al objeto retorno los datos ingresados en los controles invocando el método de edición de la clase ordenCompra
                                    retorno = facturadoP.EditaFacturadoProveedor(7, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                }
                                if (retorno.OperacionExitosa)
                                {
                                    //Instanciando Para editar la fichas por el nuevo id de factura
                                    using (DataTable dtFichasFacturas = SAT_CL.CXC.FichaIngresoAplicacion.ObtieneAplicacionesFacturas(72, Convert.ToInt32(gvFacturas.SelectedDataKey["Id"]), 0))
                                    {
                                        if (Validacion.ValidaOrigenDatos(dtFichasFacturas))
                                        {
                                            List<DataRow> SustitucionFacturasFichas = (from DataRow dep in dtFichasFacturas.AsEnumerable()
                                                                                       select dep).ToList();
                                            if (SustitucionFacturasFichas.Count > 0)
                                            {
                                                foreach (DataRow row in SustitucionFacturasFichas)
                                                {
                                                    using (SAT_CL.CXC.FichaIngresoAplicacion Ficha = new SAT_CL.CXC.FichaIngresoAplicacion(Convert.ToInt32(row["Id"])))
                                                    {
                                                        retorno = Ficha.EditarFichaIngresoAplicacion(Convert.ToInt32(gvAnticiposProveedor.SelectedDataKey["Id"]),
                                                          ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                        if (retorno.OperacionExitosa)
                                                        {
                                                            //Instanciando Para editar la relacion por el nuevo id de factura
                                                            using (DataTable dtRelaciones = SAT_CL.CXP.FacturadoProveedorRelacion.ObtieneRelacionesFactura(Convert.ToInt32(gvFacturas.SelectedDataKey["Id"])))
                                                            {
                                                                if (Validacion.ValidaOrigenDatos(dtRelaciones))
                                                                {
                                                                    List<DataRow> SustitucionFacturasRelacion = (from DataRow dep in dtRelaciones.AsEnumerable()
                                                                                                                 select dep).ToList();
                                                                    if (SustitucionFacturasRelacion.Count > 0)
                                                                    {
                                                                        foreach (DataRow rows in SustitucionFacturasRelacion)
                                                                        {
                                                                            using (SAT_CL.CXP.FacturadoProveedorRelacion Relacion = new SAT_CL.CXP.FacturadoProveedorRelacion(Convert.ToInt32(rows["Id"])))
                                                                            {
                                                                                retorno = Relacion.EditarFacturadoProveedorRelacion(Convert.ToInt32(gvAnticiposProveedor.SelectedDataKey["Id"]),
                                                                                  ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                                                if (retorno.OperacionExitosa)
                                                                                {

                                                                                    //Instanciando Para editar la factura actual dependiendo estaus 
                                                                                    using (SAT_CL.CXP.FacturadoProveedor facturadoF = new SAT_CL.CXP.FacturadoProveedor(Convert.ToInt32(gvAnticiposProveedor.SelectedDataKey["Id"])))
                                                                                    {
                                                                                        if (facturadoF.id_factura > 0)
                                                                                        {
                                                                                           
                                                                                            int Estatus;

                                                                                            if (Factura2 > MontoAplicado)
                                                                                            {
                                                                                                Estatus = 4;
                                                                                                //Asigna al objeto retorno los datos ingresados en los controles invocando el método de edición de la clase ordenCompra
                                                                                                retorno = facturadoF.EditaFacturadoProveedor(Convert.ToByte(Estatus),
                                                                                                ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                                                            }
                                                                                            else if (Factura2 == MontoAplicado)
                                                                                            {
                                                                                                Estatus = 5;
                                                                                                //Asigna al objeto retorno los datos ingresados en los controles invocando el método de edición de la clase ordenCompra
                                                                                                retorno = facturadoF.EditaFacturadoProveedor(Convert.ToByte(Estatus),
                                                                                                ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                                                            }

                                                                                        }

                                                                                    }


                                                                                }
                                                                            }
                                                                        }

                                                                    }

                                                                }
                                                            }
                                                        }
                                                    }
                                                }

                                            }

                                        }
                                    }
                                }
                                else
                                {
                                    retorno = new RetornoOperacion("No se puede sustituir la factura ya que el monto de la nueva factura es menor y cuenta con valor para cubrir montos aplicados");
                                }
                            }
                        }
                        if (retorno.OperacionExitosa)
                        {
                            //Creando liga entre facturas
                            retorno = SAT_CL.Global.Referencia.InsertaReferencia(Convert.ToInt32(gvAnticiposProveedor.SelectedDataKey["Id"]), 72, SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(0, 72, "Refacturacion", 0, "Anticipos Proveedor"), gvFacturas.SelectedDataKey["Id"].ToString(), Fecha.ObtieneFechaEstandarMexicoCentro(), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                            //Validando Operación Exitosa
                            if (retorno.OperacionExitosa)
                            {
                                //Completando Transacción
                                trans.Complete();
                            }
                        }
                    }

                }


            }
            else
            {
                retorno = new RetornoOperacion("No se puede sustituir Factura");

            }

            //Mostrando Mensaje
            TSDK.ASP.ScriptServer.MuestraNotificacion(this, retorno, ScriptServer.PosicionNotificacion.AbajoDerecha);


            //Devolviendo Resultado Obtenido
            return retorno;

        }

        #region Métodos Importación Factura

        /// <summary>
        /// Método Público Web encargado de Obtener los Archivos del Lado del Cliente
        /// </summary>
        /// <param name="context"></param>
        /// <param name="name"></param>
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static void ArchivoSesion(object context, string file_name)
        {
            //Obteniendo Archivo Codificado en UTF8
            UTF8Encoding utf8 = new UTF8Encoding();
            byte[] archivoXML = utf8.GetBytes(context.ToString());

            //Declarando Documento XML
            XmlDocument doc = new XmlDocument();

            //Obteniendo XML en cadena
            using (MemoryStream ms = new MemoryStream(archivoXML))

                //Cargando Documento XML
                doc.Load(ms);

            //Guardando en sesión el objeto creado
            System.Web.HttpContext.Current.Session["XML"] = doc;
            System.Web.HttpContext.Current.Session["XMLFileName"] = file_name;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="archivoBase64"></param>
        /// <param name="nombreArchivo"></param>
        /// <param name="mimeType"></param>
        /// <returns></returns>
        [WebMethod]
        public static string LecturaArchivo(string archivoBase64, string nombreArchivo, string mimeType)
        {
            //Definiendo objeto de retorno
            string resultado = "";

            //Si hay elementos
            if (!string.IsNullOrEmpty(archivoBase64))
            {
                //Validando tipo de archivo (mime type), debe ser .xml
                if (mimeType == "text/xml")
                {
                    try
                    {
                        //Convietiendo archivo a bytes
                        byte[] responseData = Convert.FromBase64String(TSDK.Base.Cadena.RegresaCadenaSeparada(archivoBase64, "base64,", 1));

                        //Declarando Documento XML
                        XmlDocument doc = new XmlDocument();
                        XDocument responseXml = new XDocument();

                        //Obteniendo XML en cadena
                        using (MemoryStream ms = new MemoryStream(responseData))
                            //Cargando Documento XML
                            doc.Load(ms);

                        //Convirtiendo XML
                        responseXml = TSDK.Base.Xml.ConvierteXmlDocumentAXDocument(doc);
                        //Almacenando en variables de sesión
                        HttpContext.Current.Session["XML"] = responseXml;
                        HttpContext.Current.Session["XMLFileName"] = nombreArchivo;
                        //Instanciando Resultado Positivo
                        resultado = string.Format("Archivo '{0}' cargado correctamente!!!", nombreArchivo);
                    }
                    catch (Exception ex)
                    {
                        //Limpiando en variables de sesión
                        HttpContext.Current.Session["XML"] = null;
                        HttpContext.Current.Session["XMLFileName"] = "";
                        //Instanciando Excepción
                        resultado = string.Format("Error al Cargar el Archivo: '{0}'", ex.Message);
                    }
                }
                else
                    //Si el tipo de archivo no es válido
                    resultado = "El archivo seleccionado no tiene un formato válido. Formatos permitidos '.xls' / '.xlsx'.";
            }
            else
                //Instanciando Excepción
                resultado = "No se encontró contenido en el archivo.";

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Realiza la validación del estatus de publicación del CFDI en servidores del SAT
        /// </summary>
        private void validaEstatusPublicacionSAT(System.Web.UI.Control control)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Declarando auxiliares
            string rfc_emisor, rfc_receptor, UUID;
            decimal monto; DateTime fecha_expedicion;

            //Obteniendo Documento XML
            XDocument xDocument = (XDocument)Session["XML"];
            XNamespace ns = xDocument.Root.GetNamespaceOfPrefix("cfdi");
            XNamespace tfd = CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Namespace TimbreFiscalDigital");

            //Convirtiendo Documento
            XmlDocument xmlDocument = new XmlDocument();
            using (var xmlReader = xDocument.CreateReader())
            {
                //Cargando XML Document
                xmlDocument.Load(xmlReader);
            }

            //Validando versión
            switch (xDocument.Root.Attribute("version") != null ? xDocument.Root.Attribute("version").Value : xDocument.Root.Attribute("Version").Value)
            {
                case "3.2":
                case "3.3":
                    {
                        //Realizando validación de estatus en SAT
                        result = Comprobante.ValidaEstatusPublicacionSAT(xmlDocument, out rfc_emisor, out rfc_receptor, out monto, out UUID, out fecha_expedicion);

                        //Colocando resultado sobre controles
                        imgValidacionSAT.Src = result.OperacionExitosa ? "../Image/Exclamacion.png" : "../Image/ExclamacionRoja.png";
                        headerValidacionSAT.InnerText = result.Mensaje;
                        lblRFCEmisor.Text = rfc_emisor;
                        lblRFCReceptor.Text = rfc_receptor;
                        lblUUID.Text = UUID;
                        lblTotalFactura.Text = monto.ToString("C");
                        lblFechaExpedicion.Text = fecha_expedicion.ToString("dd/MM/yyyy HH:mm");
                        break;
                    }
            }

            //Mostrando resultado de consulta en SAT (ventana modal)
            ScriptServer.AlternarVentana(control, "DatosValidacionSAT", "contenidoResultadoConsultaSATModal", "contenidoResultadoConsultaSAT");
        }
        /// <summary>
        /// Método encargado de Importar el Archvio XML
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion guardaXML()
        {
            //Declarando Objeto de Operación
            RetornoOperacion result = new RetornoOperacion();

            //Declarando Variable Auxiliar
            int idFacturaProveedor = 0;

            //Validando Archivo XML
            if (Session["XML"] != null)
            {
                //Obteniendo Archivo XML
                XDocument documento = (XDocument)Session["XML"];

                //Validando estatus de la Forma
                switch ((Pagina.Estatus)Session["estatus"])
                {
                    case Pagina.Estatus.Copia:
                        {
                            //Inicializando Bloque Transaccional
                            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                            {
                                //Validando versión
                                switch (documento.Root.Attribute("version") != null ? documento.Root.Attribute("version").Value : documento.Root.Attribute("Version").Value)
                                {
                                    case "3.2":
                                        {
                                            //Insertando CFDI 3.2
                                            result = SAT_CL.CXP.FacturadoProveedor.ImportaComprobanteVersion32(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                                      Convert.ToInt32(TipoServicio), Session["XMLFileName"].ToString(), documento, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                            break;
                                        }
                                    case "3.3":
                                        {
                                            //Insertando CFDI 3.3
                                            result = SAT_CL.CXP.FacturadoProveedor.ImportaComprobanteVersion33(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                                      Convert.ToInt32(TipoServicio), Session["XMLFileName"].ToString(), documento, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                            break;
                                        }
                                }

                                //Validando Operación Exitosa
                                if (result.OperacionExitosa)
                                {
                                    //Guardando Factura Nueva
                                    idFacturaProveedor = result.IdRegistro;

                                    //Refacturando
                                    result = SAT_CL.CXP.FacturadoProveedor.RefacturacionCXP(Convert.ToInt32(Session["id_registro"]), idFacturaProveedor, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                    //Validando Operación Exitosa
                                    if (result.OperacionExitosa)
                                    {
                                        //Instanciando Resultado Positivo
                                        result = new RetornoOperacion(idFacturaProveedor);

                                        //Completando Transacción
                                        scope.Complete();
                                    }
                                }
                            }
                            break;
                        }
                    default:
                        {
                            //Validando versión
                            switch (documento.Root.Attribute("version") != null ? documento.Root.Attribute("version").Value : documento.Root.Attribute("Version").Value)
                            {
                                case "3.2":
                                    {
                                        //Insertando CFDI 3.2
                                        result = SAT_CL.CXP.FacturadoProveedor.ImportaComprobanteVersion32(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                         // Convert.ToInt32(ddlTipoServicio.SelectedValue), Session["XMLFileName"].ToString(), documento, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                         Convert.ToInt32(TipoServicio), Session["XMLFileName"].ToString(), documento, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                        break;
                                    }
                                case "3.3":
                                    {
                                        //Insertando CFDI 3.3
                                        result = SAT_CL.CXP.FacturadoProveedor.ImportaComprobanteVersion33(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                                  Convert.ToInt32(TipoServicio), Session["XMLFileName"].ToString(), documento, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                        break;
                                    }
                            }

                            //Validando Operación Exitosa
                            if (result.OperacionExitosa)
                            { 
                            //Guardando Factura Nueva
                            idFacturaProveedor = result.IdRegistro;       
                            }
                            break;
                        }
                }
            }
            else
                //Instanciando Excepción
                result = new RetornoOperacion("Cargue su archivo XML para importar");

            //Validando que exista
            if (result.OperacionExitosa)
            {
                //Reasignando Id de registro
                result = new RetornoOperacion(idFacturaProveedor);
                //Establecemos el id del registro
                Session["id_registro"] = result.IdRegistro;
                //Eliminando Contenido en Sessión del XML
                Session["XML"] =
                Session["XMLFileName"] = null;
                //Inicializando Valores
                inicializaValores();
                InicializaVentanaModal();
            }

            //Actualizamos la etiqueta de errores
            ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);

            //Devolviendo resultado Obtenido
            return result;
        }
        
        #endregion

        #endregion

        #region Eventos GridView "Facturas"

        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView "Facturas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoFacturas_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando tamaño del GridView
            Controles.CambiaTamañoPaginaGridView(gvFacturas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamanoFacturas.SelectedValue), true, 2);
          //  Controles.CambiaTamañoPaginaGridView(gvSoporteTecnico, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamano.SelectedValue), true, 2);
        }
        /// <summary>
        /// Evento Producido al Exportar al Contenido del GridView "Facturas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportarFacturas_Click(object sender, EventArgs e)
        {
            //Exportando Contenido del GridView
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "Id");
        }
        /// <summary>
        /// Evento Producido al Cambiar la Expresión del Ordenamiento del GridView "Facturas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFacturas_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión del Ordenamiento
            lblOrdenadoFacturas.Text = Controles.CambiaSortExpressionGridView(gvFacturas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression, true, 2);

        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Página del GridView "Facturas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFacturas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice del GridView
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvFacturas, ((DataSet)Session["DS"]).Tables["Table"], e.NewPageIndex, true, 1);
        }

        protected void gvFacturas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Cargando Menu Contextual
            Controles.CreaMenuContextual(e, "menuContext", "menuOptions", "MostrarMenu", true, true);
        }
        /// <summary>
        /// Evento que permite cargar los valores a los controles de la modal solo si se desea realizar una edición.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkFacturaNueva_Click(object sender, EventArgs e)
        {
            //Declarando objeto de resultado
            RetornoOperacion retorno = new RetornoOperacion();
            //Validando que existen Registros en el gridView
            if (gvFacturas.DataKeys.Count > 0)
            {
                //Seleccionando Fila del grid view para editar los valores
                TSDK.ASP.Controles.SeleccionaFila(gvFacturas, sender, "lnk", false);
                using (SAT_CL.CXP.FacturadoProveedor Facturas = new SAT_CL.CXP.FacturadoProveedor(Convert.ToInt32(gvFacturas.SelectedDataKey["Id"])))
                {
                    //Obteniendo Valor
                    TipoServicio = Convert.ToInt32(Facturas.id_tipo_servicio);
                    if (Convert.ToInt32(Facturas.id_estatus_factura) == 2)
                    {
                    retorno = new RetornoOperacion("No puedes realizar la sustitucion de la factura por que se encuentra en un estatus aceptada");
                    //Mostrando Mensaje
                    TSDK.ASP.ScriptServer.MuestraNotificacion(this, retorno, ScriptServer.PosicionNotificacion.AbajoDerecha);
                    }
                    else
                    ScriptServer.AlternarVentana(this.Page, "SustitucionNueva", "contenedorVentanaSustitucionNueva", "VentanaSustitucionNueva");
                }
            }
            
        }

        /// <summary>
        /// Evento Producido al Cambiar el Monto de un Registro del GridView "Facturas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkFacturaExistente_Click(object sender, EventArgs e)
        {
            //Declarando objeto de resultado
            RetornoOperacion retorno = new RetornoOperacion();
            //Validando que existen Registros en el gridView
            if (gvFacturas.DataKeys.Count > 0)
            {
                //Seleccionando Fila del grid view para editar los valores
                TSDK.ASP.Controles.SeleccionaFila(gvFacturas, sender, "lnk", false);
                using (SAT_CL.CXP.FacturadoProveedor Facturas = new SAT_CL.CXP.FacturadoProveedor(Convert.ToInt32(gvFacturas.SelectedDataKey["Id"])))
                {
                    //Obteniendo Valor
                    TipoServicio = Convert.ToInt32(Facturas.id_tipo_servicio);
                    if (Convert.ToInt32(Facturas.id_estatus_factura) == 2)
                    {
                        retorno = new RetornoOperacion("No puedes realizar la sustitucion de la factura por que se encuentra en un estatus aceptada");
                        //Mostrando Mensaje
                        TSDK.ASP.ScriptServer.MuestraNotificacion(this, retorno, ScriptServer.PosicionNotificacion.AbajoDerecha);
                    }
                    else    
                      //Mostrando Ventana de Confirmación
                     ScriptServer.AlternarVentana(this.Page, "SustitucionExistente", "contenedorVentanaSustitucionExistente", "VentanaSustitucionExistente");
                }                      
            }           
        }
        /// <summary>
        /// Evento control de imaButton "Facturas "
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imbsustitucionExistente_Click(object sender, ImageClickEventArgs e)
        {
            //Declarando objeto de resultado
            RetornoOperacion retorno = new RetornoOperacion();
            Controles.SeleccionaFila(gvFacturas, sender, "imb", false);

            ImageButton imb = (ImageButton)sender;
            switch (imb.CommandName)
            {
                case "Existente":
                    //Crea objeto de previaje
                    if (Convert.ToInt32(gvFacturas.SelectedDataKey["Id"]) > 0)
                    {
                        

                        using (SAT_CL.CXP.FacturadoProveedor Facturas = new SAT_CL.CXP.FacturadoProveedor(Convert.ToInt32(gvFacturas.SelectedDataKey["Id"])))
                        {
                            //Obteniendo Valor
                            TipoServicio = Convert.ToInt32(Facturas.id_tipo_servicio);
                            if (Convert.ToInt32(Facturas.id_estatus_factura) == 2)
                            {
                                retorno = new RetornoOperacion("No puedes realizar la sustitucion de la factura por que se encuentra en un estatus aceptada");
                                //Mostrando Mensaje
                                TSDK.ASP.ScriptServer.MuestraNotificacion(this, retorno, ScriptServer.PosicionNotificacion.AbajoDerecha);
                            }
                            else
                            { 
                                //Mostrando Ventana de Confirmación
                                buscaFacturasModal();
                                ScriptServer.AlternarVentana(this.Page, "SustitucionExistente", "contenedorVentanaSustitucionExistente", "VentanaSustitucionExistente");
                            }
                        }
                    }
                    break;
                case "Nueva":
                    //Validando que existen Registros en el gridView
                    if (gvFacturas.DataKeys.Count > 0)
                    {


                        using (SAT_CL.CXP.FacturadoProveedor Facturas = new SAT_CL.CXP.FacturadoProveedor(Convert.ToInt32(gvFacturas.SelectedDataKey["Id"])))
                        {
                            //Obteniendo Valor
                            TipoServicio = Convert.ToInt32(Facturas.id_tipo_servicio);
                            if (Convert.ToInt32(Facturas.id_estatus_factura) == 2)
                            {
                                retorno = new RetornoOperacion("No puedes realizar la sustitucion de la factura por que se encuentra en un estatus aceptada");
                                //Mostrando Mensaje
                                TSDK.ASP.ScriptServer.MuestraNotificacion(this, retorno, ScriptServer.PosicionNotificacion.AbajoDerecha);
                            }
                            else
                                //Mostrando Ventana de Confirmación
                                ScriptServer.AlternarVentana(this.Page, "SustitucionNueva", "contenedorVentanaSustitucionNueva", "VentanaSustitucionNueva");

                        }
                    }
                    break;
            }
        }
        #endregion

        #region Eventos GridView "Facturas Existentes"
        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView "Facturas Existentesr"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoFI_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando tamaño del GridView
            Controles.CambiaTamañoPaginaGridView(gvAnticiposProveedor, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), Convert.ToInt32(ddlTamanoFI.SelectedValue), true, 7);
        }
        /// <summary>
        /// Evento Producido al Exportar al Contenido del GridView "Facturas Existentes"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportar_Click(object sender, EventArgs e)
        {
            //Exportando Contenido del GridView
          //  Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), "Id");
        }
        /// <summary>
        /// Evento Producido al Cambiar la Expresión del Ordenamiento del GridView "Facturas Existentes"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvAnticiposProveedor_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión del Ordenamiento
            Controles.CambiaSortExpressionGridView(gvAnticiposProveedor, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.SortExpression, true, 7);
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Página del GridView "Facturas Existentes"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvAnticiposProveedor_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            Controles.CambiaIndicePaginaGridView(gvAnticiposProveedor, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.NewPageIndex, true, 7);
        }
        /// <summary>
        /// Evento control de imaButton "Facturas Existentes"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imbsustitucion_Click(object sender, ImageClickEventArgs e)
        {
            //Declarando objeto de resultado
            RetornoOperacion retorno = new RetornoOperacion();
            Controles.SeleccionaFila(gvAnticiposProveedor, sender, "imb", false);
            ImageButton imb = (ImageButton)sender;
            switch (imb.CommandName)
            {   
                case "Cambio":
                    string Mensaje = "";
                    //Validando que existen Registros en el gridView
                    using (SAT_CL.CXP.FacturadoProveedor FacturasAnt = new SAT_CL.CXP.FacturadoProveedor(Convert.ToInt32(gvFacturas.SelectedDataKey["Id"])))
                    {
                        using (SAT_CL.CXP.FacturadoProveedor FacturasAho = new SAT_CL.CXP.FacturadoProveedor(Convert.ToInt32(gvAnticiposProveedor.SelectedDataKey["Id"])))
                        {
                            if (string.IsNullOrEmpty(FacturasAnt.folio))
                            {
                                Mensaje = "La Factura con Uuid: " + FacturasAnt.uuid + ",fue sustituida por: " + FacturasAho.uuid;
                            }
                            else
                                Mensaje = "La Factura con folio: " + FacturasAnt.folio + ",fue sustituida por: " + FacturasAho.folio;
                        }
                    }
                    ucSoporte.InicializaControlUsuario(Mensaje, 4, "");
                    //Mostrando ventana modal correspondiente      
                    alternaVentanaModal("Soporte", this.Page);
                    ViewState["idTipo"] = this.tipo = 1;
                    break;
            }

        }

        #endregion

        #region Eventos Importación Factura

        /// <summary>
        /// Evento producido al Presionar el Boton "Importar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnImportar_Click(object sender, EventArgs e)
        {
            //Validando que existe el Archivo
            if (Session["XML"] != null)
            {
                //Obteniendo Documento
                XDocument doc = (XDocument)Session["XML"];
                XNamespace ns = doc.Root.GetNamespaceOfPrefix("cfdi");

                try
                {
                    //Declarando variables Auxiliares
                    string rfc = "", nombre = "";

                    //Validando Version
                    switch (doc.Root.Attribute("Version") != null ? doc.Root.Attribute("Version").Value : doc.Root.Attribute("version").Value)
                    {
                        case "3.2":
                            {
                                //Asignando valores
                                rfc = doc.Root.Element(ns + "Emisor").Attribute("rfc").Value.ToUpper();
                                nombre = doc.Root.Element(ns + "Emisor").Attribute("nombre").Value.ToUpper();
                                break;
                            }
                        case "3.3":
                            {
                                //Asignando valores
                                rfc = doc.Root.Element(ns + "Emisor").Attribute("Rfc").Value.ToUpper();
                                nombre = doc.Root.Element(ns + "Emisor").Attribute("Nombre").Value.ToUpper();
                                break;
                            }
                    }

                    //Instanciando Compania Emisora (Proveedor)
                    using (SAT_CL.Global.CompaniaEmisorReceptor emi = SAT_CL.Global.CompaniaEmisorReceptor.ObtieneInstanciaCompaniaRFC(rfc, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                    {
                        //Validando que Exista el Proveedor
                        if (emi.id_compania_emisor_receptor > 0)
                        {
                            validaEstatusPublicacionSAT(btnImportar);
                        }
                        else
                        {
                            TSDK.ASP.ScriptServer.AlternarVentana(upbtnImportar, "Ventana Confirmación", "contenedorVentanaConfirmacion", "ventanaConfirmacion");
                        }

                    }
                }
                catch (Exception ex)
                {
                    //Obteniendo Mensaje
                    string excepcion = ex.Message;

                    //Mostrando Notificación
                    ScriptServer.MuestraNotificacion(btnImportar, excepcion, ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                }
            }
        }

        #endregion

        #region Modales
        /// <summary>
        /// Método abre ventanas modales de manera dinamica
        /// </summary>
        /// <param name="nombre_ventana"></param>
        /// <param name="control"></param>
        private void alternaVentanaModal(string nombre_ventana, Control control)
        {
            //Determina que modal abrira
            switch (nombre_ventana)
            {
                case "Nueva":
                    //ScriptServer.AlternarVentana(control, nombre_ventana, "ventanaConsultaMedica", "contenedorVentanaConsultaMedica");
                    ScriptServer.AlternarVentana(control, nombre_ventana, "SustitucionNueva", "contenedorVentanaSustitucionNueva", "VentanaSustitucionNueva");             
                    break;
                case "Existente":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "SustitucionExistente", "contenedorVentanaSustitucionExistente", "VentanaSustitucionExistente");                   
                    break;
                case "Soporte":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "Soporte", "contenidoSoporteTecnicoModal", "contenidoSoporteTecnico");
                    break;
            }
        }

        /// <summary>
        /// Evento producido al dar click en boton cerrar ventana
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarVentanaModal_Click(object sender, EventArgs e)
        {
            //Validando que LinkButton se presiono
            switch (((LinkButton)sender).CommandName)
            {
                case "Nueva":
                    alternaVentanaModal("Nueva", (LinkButton)sender);
                    break;
                case "Existente":
                    alternaVentanaModal("Existente", (LinkButton)sender);
                    break;
                case "Soporte":
                    alternaVentanaModal("Soporte", (LinkButton)sender);
                    break;
            }
        }
        #endregion
    }
}