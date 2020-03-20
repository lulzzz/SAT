using SAT_CL.Global;
using SAT_CL.Nomina;
using System;
using System.Data;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;
namespace SAT.Nomina
{
	public partial class Nomina1233 : System.Web.UI.Page
    {
		
        #region Eventos
		/// <summary>
		/// Evento Producido al Efectuarse una Recarga de Página
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void Page_Load(object sender, EventArgs e)
		{
			//Validando que se Produjo un PostBack
			if (!Page.IsPostBack)
				//Invocando Método de Inicialización
				InicializaPagina();
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
					//Invoca al método inicializaPagina
					InicializaPagina();
					//Limpia los mensajes de error del lblError
					lblError.Text = "";
					break;
				}
				//Si la elección del menú es la opcion Abrir
				case "Abrir":
				{
					//Invoca al Método de Apertura
					InicializaAperturaRegistro(200, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);
					break;
				}
				//Si la elección del menú es la opción Guardar
				case "Guardar":
				{
					//Invoca al método Guardado
					GuardaNomina();
					break;
				}
				//Si la elección del menú es la opción Editar
				case "Editar":
				{
					//Asigna a la variable session estaus el estado de la pagina nuevo
					Session["estatus"] = Pagina.Estatus.Edicion;
					//Invoca el método de Inicialización
					InicializaPagina();
					//Limpia los mensajes del lblError
					lblError.Text = "";
					break;
				}
				//Si la elección del menú es la opción Eliminar
				case "Eliminar":
				{
					//Declarando Objeto de Retorno
					RetornoOperacion result = new RetornoOperacion();
					//Instanciando Nomina
					using (SAT_CL.Nomina.Nomina12 nom = new SAT_CL.Nomina.Nomina12(Convert.ToInt32(Session["id_registro"])))
					{
						//Validando que exista la Nomina
						if (nom.habilitar)
						{
							//Deshabilitando Nomina
							result = nom.DeshabilitaNomina(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
							//Validando Resultado Positivo
							if (result.OperacionExitosa)
							{
								//Asignando Sessión
								Session["id_registro"] = 0;
								Session["estatus"] = Pagina.Estatus.Nuevo;
								//Inicializando Página
								InicializaPagina();
							}
							//Mostrando Resultado
							ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
						}
					}
					break;
				}
				//Si la elección del menú en la opción Bitacora
				case "Bitacora":
				{
					//Invoca al método bitacora
					InicializaBitacora(Session["id_registro"].ToString(), "200", "Nomina");
					break;
				}
				//Si la elección del menú en la opcion Referencia
				case "Referencias":
				{
					//NO LLEVA REFERENCIAS
					break;
				}
				//Si la elección del menú en la opcion Archivo
				case "Archivo":
				{
					break;
				}
				//Si la elección del menú en la opcion Timbrar Nómina
				case "TimbrarTodo":
				{
					//Ocultamos controles 
					btnAceptarTimbradoEmpleado.Visible = false;
					btnAceptarTimbradoNomina.Visible = true;
					//Inicializamos Valores de Timbrado
					InicializaValoresTimbradoNomina();
					//Mostrando Ventana
					GestionaVentanas(this, "Timbrar");
					break;
				}
				//Si la elección del menú en la opcion Acerca
				case "Acerca":
				{
					break;
				}
				//Si la elección del menú en la opcion Ayuda
				case "Ayuda":
				{
					break;
				}
			}
		}
		/// <summary>
		/// Evento Producido al Dar Click en el Boton "Aceptar"
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btnAceptar_Click(object sender, EventArgs e)
		{
			//Invoca al método Guardado
			GuardaNomina();
		}
		/// <summary>
		/// Evento Producido al Dar Click en el Boton "Cancelar"
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btnCancelar_Click(object sender, EventArgs e)
		{
			//Evalua cada estatus de la página
			switch ((Pagina.Estatus)Session["estatus"])
			{
				//En caso de que el estado de la página este en modo Nuevo/Edición
				case Pagina.Estatus.Edicion:
				{
					//Asignando Estatus de Sesión
					Session["estatus"] = Pagina.Estatus.Lectura;
					break;
				}
			}
			//Invocando Método de Inicialización
			InicializaPagina();
		}
		/// <summary>
		/// Evento Producido al Dar Click en el Boton "Agregar"
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btnAgregarEmp_Click(object sender, EventArgs e)
		{
			//Declarando Objeto de Retorno
			RetornoOperacion result = new RetornoOperacion();
			//Insertando Nomina de Empleado
			result = NomEmpleado.InsertaNominaEmpleado(Convert.ToInt32(Session["id_registro"]), Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtEmpleado.Text, "ID:", 1)), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
			//Validando que la Operación fue Exitosa
			if (result.OperacionExitosa)
			{
				//Invocando Método de Carga de Nominas
				CargaNominaEmpleados();
				//Limpiando Control
				txtEmpleado.Text = "";
			}
			//Mostrando Mensaje de Confirmación
			ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
		}
		/// <summary>
		/// Evento Producido al Aceptar el Timbrado de la Nómina
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btnAceptarTimbrado_Click(object sender, EventArgs e)
		{
			//Declaramos Objeto Resultado
			RetornoOperacion res = new RetornoOperacion();
			//Creación del objeto Button que obtiene las opciones de los menú desplegable .
			Button botonMenu = (Button)sender;
			//Permite ejecutar acciones determinadas por cada opción del menú
			switch (botonMenu.CommandName)
			{
				case "TimbrarEmpleado":
					res = TimbraEmpleado();
					break;
				case "TimbrarNomina":
					res = TimbraNomina();
					break;
			}
			//Ocultando Ventana
			GestionaVentanas(this, "Timbrar");
			//Mostrando Resultado Obtenido
			ScriptServer.MuestraNotificacion(this, res, ScriptServer.PosicionNotificacion.AbajoDerecha);
		}
		/// <summary>
		/// Evento Producido al Cancelar el Timbrado de la Nómina
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btnAceptarCancelarNomina_Click(object sender, EventArgs e)
		{
			//Declarando Objeto de Retorno
			RetornoOperacion result = new RetornoOperacion();
			//Instanciando Nómina Empleado
			using (NomEmpleado ne = new NomEmpleado(Convert.ToInt32(gvNominaEmpleado.SelectedDataKey["Id"])))
			{
				//Validando que exista el Registro
				if (ne.habilitar)
					//Timbrando Nómina del Empleado
					result = ne.ActualizaEstatus(NomEmpleado.Estatus.Cancelado, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
				else
					//Instanciando Excepción
					result = new RetornoOperacion("No Existe la Nómina del Empleado");
				//Validando Operación Exitosa
				if (result.OperacionExitosa)
				{
					//Cargando Nomina de Empleados
					CargaNominaEmpleados();
					//Marcando Fila
					Controles.MarcaFila(gvNominaEmpleado, ne.id_nomina_empleado.ToString(), "Id", "Id", OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), lblOrdenado.Text, Convert.ToInt32(ddlTamano.SelectedValue), true, 1);
					//Sumando Totales
					SumaTotalesNominaEmpleado();
				}
			}
			//Ocultando Ventana
			GestionaVentanas(this, "Cancelar");
			//Mostrando Resultado Obtenido
			ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
		}
		/// <summary>
		/// Evento Producido al Dar Click en el Link "Exportar"
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void lnkExportar_Click(object sender, EventArgs e)
		{
			//Obteniendo Control
			LinkButton lnk = (LinkButton)sender;
			//Validando el Comando
			switch (lnk.CommandName)
			{
				case "NominaEmpleados":
				{
					//Exportando Contenido
					Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"));
					break;
				}
				case "DetalleNomina":
				{
					//Exportando Contenido
					Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"));
					break;
				}
				case "NominaOtros":
				{
					//Exportando Contenido
					Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"));
					break;
				}
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void txtFecIniPago_TextChanged(object sender, EventArgs e)
		{
			//Invocando Método de Calculo
			CalculaDiasPago();
		}
		/// <summary>
		/// Evento generado al cambiar el Tipo de Nómina
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void ddlTipoNomina_SelectedIndexChanged(object sender, EventArgs e)
		{
			//Cargando Tipos de Periodicidad de Pago
			SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlPeriodicidadPago, "", 3186, Convert.ToInt32(ddlTipoNomina.SelectedValue));
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void txtFecFinPago_TextChanged(object sender, EventArgs e)
		{
			//Invocando Método de Calculo
			CalculaDiasPago();
		}
		/// <summary>
		/// Evento que permte imprimir el formato de impresión de Finiquito
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void lnkFiniquito_Click(object sender, EventArgs e)
		{
			//Valida que existan datos en el gridview
			if (gvNominaEmpleado.DataKeys.Count != 0)
			{
				//Selecciona la fila del gridview
				Controles.SeleccionaFila(gvNominaEmpleado, sender, "lnk", false);
				//Obtiene la ruta y datos para el reporte de finiquito
				string urlReporte = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Nomina/Nomina12.aspx", "~/RDLC/Reporte.aspx");
				//Instancia a nueva ventana de navegador para la apertura de registro
				TSDK.ASP.ScriptServer.AbreNuevaVentana(string.Format("{0}?idTipoReporte={1}&idRegistro={2}", urlReporte, "Finiquito12", Convert.ToInt32(gvNominaEmpleado.SelectedDataKey["Id"])), "Finiquito", "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500", Page);
			}
		}
		#region Eventos GridView "Nomina Empleados"
		/// <summary>
		/// Evento Disparado al Cambiar el Tamaño del GridView "Nomina Empleados"
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void ddlTamano_SelectedIndexChanged(object sender, EventArgs e)
		{
			//Cambio de Tamaño
			Controles.CambiaTamañoPaginaGridView(gvNominaEmpleado, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamano.SelectedValue), true, 1);
			//Mostrando Totales
			SumaTotalesNominaEmpleado();
		}
		/// <summary>
		/// Evento Disparado al Cambiar la Expresión de Ordenamiento
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void gvNominaEmpleado_Sorting(object sender, GridViewSortEventArgs e)
		{
			//Cambiando Expresión
			lblOrdenado.Text = Controles.CambiaSortExpressionGridView(gvNominaEmpleado, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression, true, 1);
			//Mostrando Totales
			SumaTotalesNominaEmpleado();
		}
		/// <summary>
		/// Evento Disparado al Cambiar el Indice de Página
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void gvNominaEmpleado_PageIndexChanging(object sender, GridViewPageEventArgs e)
		{
			//Cambiando Página
			Controles.CambiaIndicePaginaGridView(gvNominaEmpleado, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, true, 1);
			//Mostrando Totales
			SumaTotalesNominaEmpleado();
		}
		/// <summary>
		/// Evento Producido al Dar Click al Link de Percepciones
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void lnkPercepcion_Click(object sender, EventArgs e)
		{
			//Validando que existan Registros
			if (gvNominaEmpleado.DataKeys.Count > 0)
			{
				//Seleccionando Fila
				Controles.SeleccionaFila(gvNominaEmpleado, sender, "lnk", false);
				//Obteniendo Control
				LinkButton lnk = (LinkButton)sender;
				//Instanciando Detalle de Nomina
				using (NomEmpleado ne = new NomEmpleado(Convert.ToInt32(gvNominaEmpleado.SelectedDataKey["Id"])))
				{
					//Validando que exista la nomina del Empleado
					if (ne.habilitar)
					{
						//Mostrando Ventana
						GestionaVentanas(this, "DetalleNomina");
						//Validando Comando
						switch (lnk.CommandName)
						{
							case "Aguinaldo":
							{
								//Inicializando Controles
								InicializaValoresDetalle(0, SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion.Bonificacion, 2);
								//Cargando de Detalles de Nomina
								CargaDetallesNomina(ne.id_nomina_empleado, "002", (byte)SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion.Percepcion);
								break;
							}
							case "Sueldo":
							{
								//Inicializando Controles
								InicializaValoresDetalle(0, SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion.Percepcion, 1);
								//Cargando de Detalles de Nomina
								CargaDetallesNomina(ne.id_nomina_empleado, "001", (byte)SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion.Percepcion);
								break;
							}
							case "Otros":
							{
								//Inicializando Controles
								InicializaValoresDetalle(0, SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion.Bonificacion, 0);
								//Cargando de Detalles de Nomina
								CargaDetallesNomina(ne.id_nomina_empleado, "0", (byte)SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion.Percepcion);
								break;
							}
						}
					}
				}
			}
		}
        /// <summary>
        /// Evento Producido al Dar Click al Link de Otros Pagos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkOtrosPagos_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvNominaEmpleado.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvNominaEmpleado, sender, "lnk", false);
                //Obteniendo Control
                LinkButton lnk = (LinkButton)sender;
                //Instanciando Detalle de Nomina
                using (NomEmpleado ne = new NomEmpleado(Convert.ToInt32(gvNominaEmpleado.SelectedDataKey["Id"])))
                {
                    //Validando que exista la nomina del Empleado
                    if (ne.habilitar)
                    {

                        //Validando Comando
                        switch (lnk.CommandName)
                        {
                            case "Otros":
                                {
                                    //Mostrando Ventana
                                    GestionaVentanas(this, "DetalleNomina");
                                    //Inicializando Controles
                                    InicializaValoresDetalle(0, SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion.Otros, 0);
                                    //Cargando de Detalles de Nomina
                                    CargaDetallesNomina(ne.id_nomina_empleado, "0", (byte)SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion.Otros);
                                    break;
                                }
                        }
                    }
                }
            }
        }
		/// <summary>
		/// Evento Producido al Dar Click al Link de Deducciones
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void lnkDeduccion_Click(object sender, EventArgs e)
		{
			//Validando que existan Registros
			if (gvNominaEmpleado.DataKeys.Count > 0)
			{
				//Seleccionando Fila
				Controles.SeleccionaFila(gvNominaEmpleado, sender, "lnk", false);
				//Obteniendo Control
				LinkButton lnk = (LinkButton)sender;
				//Instanciando Detalle de Nomina
				using (NomEmpleado ne = new NomEmpleado(Convert.ToInt32(gvNominaEmpleado.SelectedDataKey["Id"])))
				{
					//Validando que exista la nomina del Empleado
					if (ne.habilitar)
					{
						//Mostrando Ventana
						GestionaVentanas(this, "DetalleNomina");
						//Validando Comando
						switch (lnk.CommandName)
						{
							case "IMSS":
							{
								//Inicializando Controles
								InicializaValoresDetalle(0, SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion.Deduccion, 1);
								//Cargando de Detalles de Nomina
								CargaDetallesNomina(ne.id_nomina_empleado, "001", (byte)SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion.Deduccion);
								break;
							}
							case "ISPT":
							{
								//Inicializando Controles
								InicializaValoresDetalle(0, SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion.Deduccion, 2);
								//Cargando de Detalles de Nomina
								CargaDetallesNomina(ne.id_nomina_empleado, "002", (byte)SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion.Deduccion);
								break;
							}
							case "Infonavit":
							{
								//Inicializando Controles
								InicializaValoresDetalle(0, SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion.Deduccion, 9);
								//Cargando de Detalles de Nomina
								CargaDetallesNomina(ne.id_nomina_empleado, "009", (byte)SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion.Deduccion);
								break;
							}
							case "Otros":
							{
								//Inicializando Controles
								InicializaValoresDetalle(0, SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion.Deduccion, 0);
								//Cargando de Detalles de Nomina
								CargaDetallesNomina(ne.id_nomina_empleado, "0", (byte)SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion.Deduccion);
								break;
							}
						}
					}
				}
			}
		}
		/// <summary>
		/// Evento Producido al Dar Click en el Link de Hrs. Extra
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void lnkHrsExtra_Click(object sender, EventArgs e)
		{
			//Validando que existan Registros
			if (gvNominaEmpleado.DataKeys.Count > 0)
			{
				//Seleccionando Fila
				Controles.SeleccionaFila(gvNominaEmpleado, sender, "lnk", false);
				//Inicializamos Indices
				Controles.InicializaIndices(gvNominaOtros);
				//Nomina Otros
				GestionaVentanas(this, "NominaOtros");
				//Inicializando Valores del Detalle
				InicializaValoresNominaOtros(Convert.ToInt32(gvNominaEmpleado.SelectedDataKey["Id"]), 0, 2);
				//Cargando Nomina Otros
				CargaNominaOtros(Convert.ToInt32(gvNominaEmpleado.SelectedDataKey["Id"]), 2);
			}
		}
		/// <summary>
		/// Evento Producido al Dar Click en el Link de Incapacidad
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void lnkIncapacidad_Click(object sender, EventArgs e)
		{
			//Validando que existan Registros
			if (gvNominaEmpleado.DataKeys.Count > 0)
			{
				//Seleccionando Fila
				Controles.SeleccionaFila(gvNominaEmpleado, sender, "lnk", false);
				//Inicializamos Indices
				Controles.InicializaIndices(gvNominaOtros);
				//Nomina Otros
				GestionaVentanas(this, "NominaOtros");
				//Inicializando Valores del Detalle
				InicializaValoresNominaOtros(Convert.ToInt32(gvNominaEmpleado.SelectedDataKey["Id"]), 0, 1);
				//Cargando Nomina Otros
				CargaNominaOtros(Convert.ToInt32(gvNominaEmpleado.SelectedDataKey["Id"]), 1);
			}
		}
		/// <summary>
		/// Evento Producido al Dar Click en el Link
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void lnkActualizaNomina_Click(object sender, EventArgs e)
		{
			//Validando que existan Registros
			if (gvNominaEmpleado.DataKeys.Count > 0)
			{
				//Seleccionando Fila
				Controles.SeleccionaFila(gvNominaEmpleado, sender, "lnk", false);
				//Declarando Objeto de Retorno
				RetornoOperacion result = new RetornoOperacion();
				//Instanciando Nomina de Empleado
				using (NomEmpleado ne = new NomEmpleado(Convert.ToInt32(gvNominaEmpleado.SelectedDataKey["Id"])))
				{
					//Validando Registro
					if (ne.habilitar)
					{
						//Obteniendo Control
						LinkButton lnk = (LinkButton)sender;
						//Validando Comando
						switch (lnk.CommandName)
						{
							case "Eliminar":
							{
								//Deshabilitando Nomina de Empleado
								result = ne.DeshabilitaNomEmpleado(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
								//Operación Exitosa?
								if (result.OperacionExitosa)
								{
									//Inicializando Indices
									Controles.InicializaIndices(gvNominaEmpleado);
									//Recargando Nomina de Empleados
									CargaNominaEmpleados();
								}
								//Mostrando Excepción
								ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
								break;
							}
							case "Timbrar":
							{
								//Validando que la Nomina este Registrada
								if (ne.estatus == NomEmpleado.Estatus.Registrado)
								{
									//Ocultamos controles 
									btnAceptarTimbradoEmpleado.Visible = true;
									btnAceptarTimbradoNomina.Visible = false;
									//Abriendo Ventana de Confirmación
									GestionaVentanas(this, "Timbrar");
									//Inicializamos Valores de Timbrado
									InicializaValoresTimbradoNomina();
								}
								else
								{
									//Instanciando Excepción
									result = new RetornoOperacion(string.Format("La Nomina del Empleado se encuentra '{0}', Imposible su Timbrado", ne.estatus.ToString()));
									//Mostrando Excepción
									ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
								}
								break;
							}
							case "Cancelar":
							{
								//Validando que la Nomina este Registrada
								if (ne.estatus == NomEmpleado.Estatus.Timbrado)
									//Abriendo Ventana de Confirmación
									GestionaVentanas(this, "Cancelar");
								else
								{
									//Instanciando Excepción
									result = new RetornoOperacion(string.Format("La Nomina del Empleado se encuentra '{0}', Imposible su Cancelación", ne.estatus.ToString()));
									//Mostrando Excepción
									ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
								}
								break;
							}
						}
					}
					else
					{
						//Instanciando Excepción
						result = new RetornoOperacion("No Existe la Nomina del Empleado");
						//Mostrando Excepción
						ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
					}
				}
			}
		}
		/// <summary>
		/// Evento que permite imprimir el formato de comprobante de nomina
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void lnkImprimirNomina_Click(object sender, EventArgs e)
		{
			//Valida que existan registros en el grid view
			if (gvNominaEmpleado.DataKeys.Count != 0)
			{
				//Selecciona la fila del gridview
				Controles.SeleccionaFila(gvNominaEmpleado, sender, "lnk", false);
				//Instanciando Nómina Empleado
				using (NomEmpleado ne = new NomEmpleado(Convert.ToInt32(gvNominaEmpleado.SelectedDataKey["Id"])))
				{
					//Validando Nómina Empleado
					if (ne.habilitar)
					{
						//Obteniendo Control
						LinkButton lnk = (LinkButton)sender;
						//Validando Comando
						switch (lnk.CommandName)
						{
							case "PDF":
							{
								//Validando el Comprobante
								if (ne.id_comprobante33 > 0)
								{
									//Obteniendo Ruta
									string urlReporte = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Nomina/Nomina1233.aspx", "~/RDLC/Reporte.aspx");
									//Instanciando nueva ventana de navegador para apertura de registro
									TSDK.ASP.ScriptServer.AbreNuevaVentana(string.Format("{0}?idTipoReporte={1}&idRegistro={2}", urlReporte, "ComprobanteNominaN12v33", ne.id_nomina_empleado), "Comprobante", "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500", Page);
								}
								else if (ne.id_comprobante > 0)
								{
									//Obteniendo Ruta
									string urlReporte = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Nomina/Nomina1233.aspx", "~/RDLC/Reporte.aspx");
									//Instanciando nueva ventana de navegador para apertura de registro
									TSDK.ASP.ScriptServer.AbreNuevaVentana(string.Format("{0}?idTipoReporte={1}&idRegistro={2}", urlReporte, "ComprobanteNominaN12", ne.id_nomina_empleado), "Comprobante", "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500", Page);
								}
								else
									//Instanciando Excepción
									ScriptServer.MuestraNotificacion(this.Page, new RetornoOperacion("No se puede recuperar el Recibo de Nómina del Empleado"), ScriptServer.PosicionNotificacion.AbajoDerecha);
								break;
							}
							case "XML":
							{
								//Validando el Comprobante
								if (ne.id_comprobante33 > 0)
									//Invocando Método de Descarga
									DescargarXMLv33(ne.id_comprobante33);
								else if (ne.id_comprobante > 0)
									//Invocando Método de Descarga
									DescargarXML(ne.id_comprobante);
								break;
							}
						}
					}
					else
						//Instanciando Excepción
						ScriptServer.MuestraNotificacion(this.Page, new RetornoOperacion("No se puede recuperar el Recibo de Nómina del Empleado"), ScriptServer.PosicionNotificacion.AbajoDerecha);
				}
			}
		}
		#endregion
		/// <summary>
		/// Evento Producido al Cerrar la Ventana Modal
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void lnkCerrar_Click(object sender, EventArgs e)
		{
			//Obteniendo Control
			LinkButton lnk = (LinkButton)sender;
			//Validando Comando
			switch (lnk.CommandName)
			{
				case "CanceladoNomina":
				{
					//Invocando Método de Gestión de Ventanas
					GestionaVentanas(this, "Cancelar");
					break;
				}
				case "DetalleNomina":
				{
					//Invocando Método de Gestión de Ventanas
					GestionaVentanas(this, "DetalleNomina");
					break;
				}
				case "NominaOtros":
				{
					//Invocando Método de Gestión de Ventanas
					GestionaVentanas(this, "NominaOtros");
					break;
				}
				case "SeparacionIndemnizacion":
				{
					GestionaVentanas(this, "SeparacionIndemnizacion");
					CargaDetallesNominaEmpleado();
					break;
				}
				case "TimbradoNomina":
				{
					//Invocando Método de Gestión de Ventanas
					GestionaVentanas(this, "Timbrar");
					break;
				}
			}
		}
		#region Eventos Modal "Detalle Nomina"
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btnGuardarDet_Click(object sender, EventArgs e)
		{
			//Invocando Método de Guardado
			GuardaDetalleNomina();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btnCancelarDet_Click(object sender, EventArgs e)
		{
			//Invocando Método de Gestión de Ventanas
			GestionaVentanas(this, "DetalleNomina");
		}
		#endregion
		#region Eventos GridView "Detalle Nomina"
		/// <summary>
		/// Evento Disparado al Cambiar el Tamaño del GridView "Detalle Nomina"
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void ddlTamanoDet_SelectedIndexChanged(object sender, EventArgs e)
		{
			//Cambio de Tamaño
			Controles.CambiaTamañoPaginaGridView(gvDetalleNomina, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), Convert.ToInt32(ddlTamanoDet.SelectedValue), true, 1);
		}
		/// <summary>
		/// Evento Disparado al Cambiar la Expresión de Ordenamiento del GridView "Detalle Nomina"
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void gvDetalleNomina_Sorting(object sender, GridViewSortEventArgs e)
		{
			//Cambiando Expresión
			lblOrdenadoDet.Text = Controles.CambiaSortExpressionGridView(gvDetalleNomina, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.SortExpression, true, 1);
		}
		/// <summary>
		/// Evento Disparado al Cambiar el Indice de Página del GridView "Detalle Nomina"
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void gvDetalleNomina_PageIndexChanging(object sender, GridViewPageEventArgs e)
		{
			//Cambiando Página
			Controles.CambiaIndicePaginaGridView(gvDetalleNomina, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.NewPageIndex, true, 1);
		}
		/// <summary>
		/// Evento Producido al Editar el Detalle
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void lnkEditar_Click(object sender, EventArgs e)
		{
			//Validando que existan Registros
			if (gvDetalleNomina.DataKeys.Count > 0)
			{
				//Seleccionando Fila
				Controles.SeleccionaFila(gvDetalleNomina, sender, "lnk", false);
				//Declarando Objeto de Retorno
				RetornoOperacion result = new RetornoOperacion();
				//Instanciamos Registro Agrupador
				using (SAT_CL.Nomina.EsquemaRegistro objEsquemRegistro = new SAT_CL.Nomina.EsquemaRegistro(Convert.ToInt32(gvDetalleNomina.SelectedValue)))
				{
					//De acuerdo al Tipo Detalle
					//Percepción
					if (objEsquemRegistro.id_esquema == 41)
					{
						//Obtenemos Valores
						int id_concepto = 0;
						decimal importe_gravado = 00M;
						decimal importe_exento = 00M;
						//Obtenemos Valores de las Percepciones
						SAT_CL.Nomina.EsquemaRegistro.ObtieneDetalleNominaPercepcion(Convert.ToInt32(gvNominaEmpleado.SelectedDataKey["Id"]), Convert.ToInt32(gvDetalleNomina.SelectedValue), out id_concepto, out importe_gravado, out importe_exento);
						//Asignando Valores
						ddlConcepto.SelectedValue = id_concepto.ToString();
						txtImporteGravado.Text = importe_gravado.ToString();
						txtImporteExento.Text = importe_exento.ToString();
					}
					else if (objEsquemRegistro.id_esquema == 74)
					{
						//Obtenemos Valores
						int id_concepto = 0;
						decimal importe = 00M;
						//Obtenemos Valores de las Percepciones
						SAT_CL.Nomina.EsquemaRegistro.ObtieneDetalleNominaDeduccion(Convert.ToInt32(gvNominaEmpleado.SelectedDataKey["Id"]), Convert.ToInt32(gvDetalleNomina.SelectedValue), out id_concepto, out importe);
						//Asignando Valores
						ddlConcepto.SelectedValue = id_concepto.ToString();
						txtImporte.Text = importe.ToString();
					}
					else if (objEsquemRegistro.id_esquema == 82)
					{
						//Obtenemos Valores
						int id_concepto = 0;
						decimal importe = 00M;
						decimal importe_subsidio_causado = 00M;
						bool valor_subsidio_causado = false;
						//Obtenemos Valores de las Percepciones
						SAT_CL.Nomina.EsquemaRegistro.ObtieneDetalleNominaOtros(Convert.ToInt32(gvNominaEmpleado.SelectedDataKey["Id"]), Convert.ToInt32(gvDetalleNomina.SelectedValue), out id_concepto, out importe, out importe_subsidio_causado, out valor_subsidio_causado);
						//Asignando Valores
						ddlConcepto.SelectedValue = id_concepto.ToString();
						txtImporte.Text = importe.ToString();
						txtSubsidioCausado.Text = importe_subsidio_causado.ToString();
						txtSubsidioCausado.Enabled = chkSubsidioCausado.Checked = valor_subsidio_causado;
					}
				}
			}
		}
		/// <summary>
		/// Evento Producido al Eliminar el Detalle
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void lnkEliminar_Click(object sender, EventArgs e)
		{
			//Validando que existan Registros
			if (gvDetalleNomina.DataKeys.Count > 0)
			{
				//Seleccionando Fila
				Controles.SeleccionaFila(gvDetalleNomina, sender, "lnk", false);
				//Declarando Objeto de Retorno
				RetornoOperacion result = new RetornoOperacion();
				//Instanciando Detalle
				using (SAT_CL.Nomina.EsquemaRegistro esquemaR = new SAT_CL.Nomina.EsquemaRegistro(Convert.ToInt32(gvDetalleNomina.SelectedDataKey["Id"])))
				{
					//Obtenemos Valores
					int id_concepto = 0;
					decimal importe_gravado = 00M;
					decimal importe_exento = 00M;
					decimal importe = 00M;
					decimal importe_subsidio_causado = 00M;
					bool valor_subsidio_causado = false;
					//De acuerdo al Esquema
					if (esquemaR.id_esquema == 41)
					{
						SAT_CL.Nomina.EsquemaRegistro.ObtieneDetalleNominaPercepcion(Convert.ToInt32(gvNominaEmpleado.SelectedDataKey["Id"]), esquemaR.id_esquema_registro, out id_concepto, out importe_gravado, out importe_exento);
					}
					else if (esquemaR.id_esquema == 41)
					{
						//Obtenemos Valores de las Percepciones
						SAT_CL.Nomina.EsquemaRegistro.ObtieneDetalleNominaDeduccion(Convert.ToInt32(gvNominaEmpleado.SelectedDataKey["Id"]), esquemaR.id_esquema_registro, out id_concepto, out importe);
					}
					else if (esquemaR.id_esquema == 82)
					{
						//Obtenemos Valores de las Percepciones
						SAT_CL.Nomina.EsquemaRegistro.ObtieneDetalleNominaOtros(Convert.ToInt32(gvNominaEmpleado.SelectedDataKey["Id"]), esquemaR.id_esquema_registro, out id_concepto, out importe, out importe_subsidio_causado, out valor_subsidio_causado);
					}
					//Instanciando Tipo Cobro Recurrente
					using (SAT_CL.Liquidacion.TipoCobroRecurrente tcr = new SAT_CL.Liquidacion.TipoCobroRecurrente(id_concepto))
					{
						//Validando Habilitación
						if (esquemaR.habilitar)
						{
							//Deshabilitando Detalle
							result = esquemaR.DeshabilitaEsquemaRegistroSuperior(txtVersion.Text, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
						}
						else
							//Instanciando Excepción
							result = new RetornoOperacion("No existe el Detalle de Nomina");
						//Validando que la Operación fuese Exitosa
						if (result.OperacionExitosa)
						{
							//Validando Percepción
							if (((SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion)tcr.id_tipo_aplicacion == SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion.Percepcion || (SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion)tcr.id_tipo_aplicacion == SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion.Bonificacion) && (tcr.id_concepto_sat_nomina == 1 || tcr.id_concepto_sat_nomina == 2))
							{
								//Inicializando Valores del Detalle
								InicializaValoresDetalle(0, (SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion)tcr.id_tipo_aplicacion, tcr.id_concepto_sat_nomina);
								//Cargando Detalles de Nomina
								CargaDetallesNomina(esquemaR.id_nomina_empleado, Catalogo.RegresaDescripcioValorCadena(92, tcr.id_concepto_sat_nomina), tcr.id_tipo_aplicacion);
							}
							else if ((SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion)tcr.id_tipo_aplicacion == SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion.Deduccion && (tcr.id_concepto_sat_nomina == 1 || tcr.id_concepto_sat_nomina == 2 || tcr.id_concepto_sat_nomina == 9))
							{
								//Inicializando Valores del Detalle
								InicializaValoresDetalle(0, (SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion)tcr.id_tipo_aplicacion, tcr.id_concepto_sat_nomina);
								//Cargando Detalles de Nomina
								CargaDetallesNomina(esquemaR.id_nomina_empleado, Catalogo.RegresaDescripcioValorCadena(91, tcr.id_concepto_sat_nomina), tcr.id_tipo_aplicacion);
							}
							else
							{
								//Inicializando Valores del Detalle
								InicializaValoresDetalle(0, (SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion)tcr.id_tipo_aplicacion, 0);
								//Cargando Detalles de Nomina
								CargaDetallesNomina(esquemaR.id_nomina_empleado, "0", tcr.id_tipo_aplicacion);
							}
							//Inicializando Indices
							Controles.InicializaIndices(gvDetalleNomina);
							//Obteniendo Nomina de Empleado
							int nomina_empleado = Convert.ToInt32(gvNominaEmpleado.SelectedDataKey["Id"]);
							//Cargando Nomina de Empleados
							CargaNominaEmpleados();
							// Marcando Fila
							Controles.MarcaFila(gvNominaEmpleado, nomina_empleado.ToString(), "Id", "Id", OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), lblOrdenado.Text, Convert.ToInt32(ddlTamano.SelectedValue), true, 1);
							upgvNominaEmpleado.Update();
							//Mostrando Totales
							SumaTotalesNominaEmpleado();
						}
						//Mostrando Mensaje de Operación
						ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
					}
				}
			}
		}
		/// <summary>
		/// Evento generado al cambiar el cke Box
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void chkSubsidioCausado_CheckedChanged(object sender, EventArgs e)
		{
			//De acuerdo al cambio de Check Box
			if (chkSubsidioCausado.Checked)
			{
				//Habilitamos Control
				txtSubsidioCausado.Enabled = true;
			}
			else
			{
				//Habilitamos Control
				txtSubsidioCausado.Enabled = false;
			}
		}
		#endregion
		#region Eventos Nomina Otros
		/// <summary>
		/// Evento Producido el Presionar el Boton "Guardar"
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btnGuardarNO_Click(object sender, EventArgs e)
		{
			//Invocando Método de Guardado
			GuardaNominaOtros();
		}
		/// <summary>
		/// Evento Producido el Presionar el Boton "Cancelar"
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btnCancelarNO_Click(object sender, EventArgs e)
		{
			//Invocando Método de Gestión de Ventanas
			GestionaVentanas(this, "NominaOtros");
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void ddlTipo_SelectedIndexChanged(object sender, EventArgs e)
		{
			//Cargando Catalogo
			SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlSubTipo, "", 3150, Convert.ToInt32(ddlTipo.SelectedValue));
			//Validando Tipo de Nomina Otros
			if (Convert.ToInt32(ddlTipo.SelectedValue) == 2)
				//Viasualizando Control
				txtCantidad.Enabled = true;
			else if (Convert.ToInt32(ddlTipo.SelectedValue) == 1)
				//Viasualizando Control
				txtCantidad.Enabled = false;
		}
		#endregion
		#region Eventos GridView "Nomina Otros"
		/// <summary>
		/// Evento Disparado al Cambiar el Tamaño del GridView "Nomina Otros"
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void ddlTamanoNO_SelectedIndexChanged(object sender, EventArgs e)
		{
			//Cambio de Tamaño
			Controles.CambiaTamañoPaginaGridView(gvNominaOtros, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), Convert.ToInt32(ddlTamanoNO.SelectedValue), true, 1);
		}
		/// <summary>
		/// Evento Disparado al Cambiar la Expresión de Ordenamiento del GridView "Nomina Otros"
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void gvNominaOtros_Sorting(object sender, GridViewSortEventArgs e)
		{
			//Cambiando Expresión
			lblOrdenadoNO.Text = Controles.CambiaSortExpressionGridView(gvNominaOtros, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), e.SortExpression, true, 1);
		}
		/// <summary>
		/// Evento Disparado al Cambiar el Indice de Página del GridView "Detalle Nomina"
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void gvNominaOtros_PageIndexChanging(object sender, GridViewPageEventArgs e)
		{
			//Cambiando Página
			Controles.CambiaIndicePaginaGridView(gvNominaOtros, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), e.NewPageIndex, true, 1);
		}
		/// <summary>
		/// Evento Producido al Editar la Nomina Otros
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void lnkEditarNO_Click(object sender, EventArgs e)
		{
			//Validando que existan Registros
			if (gvNominaOtros.DataKeys.Count > 0)
			{
				//Seleccionando Fila
				Controles.SeleccionaFila(gvNominaOtros, sender, "lnk", false);
				//Inicializando Valores del Detalle
				InicializaValoresNominaOtros(Convert.ToInt32(gvNominaEmpleado.SelectedDataKey["Id"]), Convert.ToInt32(gvNominaOtros.SelectedValue), Convert.ToByte(ddlTipo.SelectedValue));
			}
		}
		/// <summary>
		/// Evento Producido al Eliminar la Nomina Otros
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void lnkEliminarNO_Click(object sender, EventArgs e)
		{
			//Validando que existan Registros
			if (gvNominaOtros.DataKeys.Count > 0)
			{
				//Declarando Objeto de Retorno
				RetornoOperacion result = new RetornoOperacion();
				//Seleccionando Fila
				Controles.SeleccionaFila(gvNominaOtros, sender, "lnk", false);
				//Instanciando Nomina Otros
				using (SAT_CL.Nomina.EsquemaRegistro no = new SAT_CL.Nomina.EsquemaRegistro(Convert.ToInt32(gvNominaOtros.SelectedDataKey["Id"])))
				{
					//Validando que exista el Registro
					if (no.habilitar)
						//Deshabilitando Nomina de Otros
						result = no.DeshabilitaEsquemaRegistroSuperior(txtVersion.Text, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
					//Validando Operación Exitosa
					if (result.OperacionExitosa)
					{
						//Inicializando Valores del Detalle
						InicializaValoresNominaOtros(Convert.ToInt32(gvNominaEmpleado.SelectedDataKey["Id"]), 0, Convert.ToByte(ddlTipo.SelectedValue));
						//Obteniendo Nomina de Empleado
						int nomina_empleado = Convert.ToInt32(gvNominaEmpleado.SelectedDataKey["Id"]);
						//Actualizando Nomina Otros
						CargaNominaOtros(nomina_empleado, Convert.ToByte(ddlTipo.SelectedValue));
						//Cargando Nomina de Empleados
						CargaNominaEmpleados();
						// Marcando Fila
						Controles.MarcaFila(gvNominaEmpleado, nomina_empleado.ToString(), "Id", "Id", OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), lblOrdenado.Text, Convert.ToInt32(ddlTamano.SelectedValue), true, 1);
						upgvNominaEmpleado.Update();
						//Mostrando Totales
						SumaTotalesNominaEmpleado();
					}
				}
				//Mostrando Resultado
				ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
			}
		}
		/// <summary>
		/// Click en botón email
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void lnkEmailEmpleado_Click(object sender, EventArgs e)
		{
			//Validando existencia de registros
			if (gvNominaEmpleado.DataKeys.Count > 0)
			{
				//Seleccionando Registro
				Controles.SeleccionaFila(gvNominaEmpleado, sender, "lnk", false);
				//Instanciando nomina empleado
				using (NomEmpleado ne = new NomEmpleado(Convert.ToInt32(gvNominaEmpleado.SelectedDataKey["Id"])))
				{
					//Validando Nómina del Empleado
					if (ne.habilitar)
					{
						//Validando Comprobante v3.3
						if (ne.id_comprobante33 > 0)
						{
							//Instanciando comprobante
							using (SAT_CL.FacturacionElectronica33.Comprobante rn = new SAT_CL.FacturacionElectronica33.Comprobante(ne.id_comprobante33))
							{
								//Validando Comprobante
								if (rn.habilitar)
								{
									//Declarando auxiliar de email de entrega de correo
									string email_entrega = "";
									//Instanciando Empresa Emisora
									using (SAT_CL.Global.CompaniaEmisorReceptor emisor = new CompaniaEmisorReceptor(rn.id_compania_emisor))
									{
										//cargando referencias de correo electronico de envío
										using (DataTable mitReferencias = SAT_CL.Global.Referencia.CargaReferencias(ne.id_empleado, 76, SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(emisor.id_compania_emisor_receptor, 76, "Correo Electrónico", 0, "Recibo Nómina")))
											email_entrega = mitReferencias != null ? mitReferencias.Rows[0]["Valor"].ToString() : "";
										{
											//Instanciando nómina
											using (SAT_CL.Nomina.Nomina nom = new SAT_CL.Nomina.Nomina(ne.id_nomina))
											{
												//Instanciando usuario
												using (SAT_CL.Seguridad.Usuario u = new SAT_CL.Seguridad.Usuario(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_usuario))
												{
													//Inicializando control de usuario de envío de email
													wucEmailCFDIv3_3.InicializaControl(u.email, string.Format("CFDI NOM. {0:dd/MM/yyyy} '{1}{2}' [{3}]", nom.fecha_fin_pago, rn.serie, rn.folio, emisor.rfc), email_entrega, "Los archivos se encuentran adjuntos en este mensaje. Ante cualquier aclaración contacte al remitente de este correo.", ne.id_comprobante33);
													//Mostrando ventana modal correspondiente
													ScriptServer.AlternarVentana(this, "Emailv3_3", "contenedorVentanaEmailV33", "ventanaEmailV33");
												}
											}
										}
									}
								}
								else
									//Mostrando error
									ScriptServer.MuestraNotificacion(this, "El pago de nómina de este empleado no se ha timbrado.", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
							}
						}
						else if (ne.id_comprobante > 0)
						{
							//Instanciando comprobante
							using (SAT_CL.FacturacionElectronica.Comprobante rn = new SAT_CL.FacturacionElectronica.Comprobante(ne.id_comprobante))
							{
								//Validando Comprobante
								if (rn.habilitar)
								{
									//Declarando auxiliar de email de entrega de correo
									string email_entrega = "";
									//Instanciando Empresa Emisora
									using (SAT_CL.Global.CompaniaEmisorReceptor emisor = new CompaniaEmisorReceptor(rn.id_compania_emisor))
									{
										//cargando referencias de correo electronico de envío
										using (DataTable mitReferencias = SAT_CL.Global.Referencia.CargaReferencias(ne.id_empleado, 76, SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(emisor.id_compania_emisor_receptor, 76, "Correo Electrónico", 0, "Recibo Nómina")))
											email_entrega = mitReferencias != null ? mitReferencias.Rows[0]["Valor"].ToString() : "";
										{
											//Instanciando nómina
											using (SAT_CL.Nomina.Nomina nom = new SAT_CL.Nomina.Nomina(ne.id_nomina))
											{
												//Instanciando usuario
												using (SAT_CL.Seguridad.Usuario u = new SAT_CL.Seguridad.Usuario(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_usuario))
												{
													//Inicializando control de usuario de envío de email
													wucEmailCFDI.InicializaControl(u.email, string.Format("CFDI NOM. {0:dd/MM/yyyy} '{1}{2}' [{3}]", nom.fecha_fin_pago, rn.serie, rn.folio, emisor.rfc), email_entrega, "Los archivos se encuentran adjuntos en este mensaje. Ante cualquier aclaración contacte al remitente de este correo.", ne.id_comprobante);
													//Mostrando ventana modal correspondiente
													ScriptServer.AlternarVentana(this, "Email", "contenidoConfirmacionEmail", "confirmacionEmail");
												}
											}
										}
									}
								}
								else
									//Mostrando error
									ScriptServer.MuestraNotificacion(this, "El pago de nómina de este empleado no se ha timbrado.", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
							}
						}
					}
					else
						//Mostrando error
						ScriptServer.MuestraNotificacion(this, new RetornoOperacion("No se puede recuperar la Nómina del Empleado"), ScriptServer.PosicionNotificacion.AbajoDerecha);
				}
			}
		}
		/// <summary>
		/// Click en botón enviar email
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void BtnEnviarEmail_Click(object sender, EventArgs e)
		{
			//Realizando el envío del correo
			RetornoOperacion resultado = wucEmailCFDI.EnviaEmail(true, true);
			//Si no hay errores
			if (resultado.OperacionExitosa)
				//Cerrando ventana modal
				ScriptServer.AlternarVentana(this, "Email", "contenidoConfirmacionEmail", "confirmacionEmail");
			//Mostrando resultado
			ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
		}
		/// <summary>
		/// Click en botón cerrar control de correo
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void LkbCerrarEmail_Click(object sender, EventArgs e)
		{
			//Cerrando ventana modal
			ScriptServer.AlternarVentana(this, "Email", "contenidoConfirmacionEmail", "confirmacionEmail");
		}
		#endregion
		#region Eventos
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void wucEmailCFDIv3_3_BtnEnviarEmail_Click(object sender, EventArgs e)
		{
			//Realizando el envío del correo
			RetornoOperacion resultado = wucEmailCFDIv3_3.EnviaEmail(true, true);
			//Si no hay errores
			if (resultado.OperacionExitosa)
				//Cerrando ventana modal
				ScriptServer.AlternarVentana(this, "Emailv3_3", "contenedorVentanaEmailV33", "ventanaEmailV33");
			//Mostrando resultado
			ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void wucEmailCFDIv3_3_LkbCerrarEmail_Click(object sender, EventArgs e)
		{
			//Cerrando ventana modal
			ScriptServer.AlternarVentana(this, "Emailv3_3", "contenedorVentanaEmailV33", "ventanaEmailV33");
		}
		#endregion
		#endregion

		#region Métodos
		/// <summary>
		/// Método encargado de Inicializar la Página
		/// </summary>
		private void InicializaPagina()
		{
			//Obteniendo Comprobante
			string id_nomina = Request.QueryString["idRegistro"];
			//Validando que Exista
			if (id_nomina != null)
			{
				//Añadiendo Resultado a Session
				Session["id_registro"] = Convert.ToInt32(id_nomina);
				//Cambiando a Edición
				Session["estatus"] = Pagina.Estatus.Edicion;
			}
			//Invocando Método de Carga
			CargaCatalogos();
			//Invocando Método de Habilitación del Menú
			HabilitaMenu();
			//Invocando Método de Habilitación del Controles
			HabilitaControles();
			//Invocando Método de Inicialización de Valores
			InicializaValores();
			//Invocando Método de Carga de Nominas
			CargaNominaEmpleados();
		}
		/// <summary>
		/// Método encargado de Cargar los Catalogos
		/// </summary>
		/// 
		private void CargaCatalogos()
		{
			//Cargando Tipos de Nómina
			SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipoNomina, "", 3185);
			//Cargando Tipos de Periodicidad de Pago
			SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlPeriodicidadPago, "", 3186, Convert.ToInt32(ddlTipoNomina.SelectedValue));
			//Cargando Tipos de Forma de Pago
			SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlFormaPago, 185, "");
			//Cargando Catalogos de Tamaño
			SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamano, "", 26);
			SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoDet, "", 26);
			SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoNO, "", 26);
			//Cargando Tipos de Nómina
			SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipoNomina, "", 3185);
		}
		/// <summary>
		/// Método encargado de habilitar las Opciones del Menú
		/// </summary>
		private void HabilitaMenu()
		{
			//Evalua cada estatus de la página
			switch ((Pagina.Estatus)Session["estatus"])
			{
				//En caso de que el estado de la página este en modo Nuevo
				case Pagina.Estatus.Nuevo:
				{
					lkbNuevo.Enabled =
					lkbAbrir.Enabled =
					lkbGuardar.Enabled = true;
					lkbEliminar.Enabled =
					lkbEditar.Enabled =
					lkbBitacora.Enabled =
					lkbReferencias.Enabled =
					lkbTimbrarTodo.Enabled =
					lkbArchivos.Enabled = false;
					lkbAcercaDe.Enabled =
					lkbAyuda.Enabled = true;
					break;
				}
				//En caso de que el estado de la página este en modo de lectura
				case Pagina.Estatus.Lectura:
				{
					lkbNuevo.Enabled =
					lkbAbrir.Enabled = true;
					lkbGuardar.Enabled = false;
					lkbEliminar.Enabled =
					lkbEditar.Enabled =
					lkbBitacora.Enabled =
					lkbReferencias.Enabled =
					lkbTimbrarTodo.Enabled = true;
					lkbArchivos.Enabled = false;
					lkbAcercaDe.Enabled =
					lkbAyuda.Enabled = true;
					break;
				}
				//En caso de que el estado de la página este en modo edición
				case Pagina.Estatus.Edicion:
				{
					lkbNuevo.Enabled =
					lkbAbrir.Enabled =
					lkbGuardar.Enabled =
					lkbEliminar.Enabled = true;
					lkbEditar.Enabled = false;
					lkbBitacora.Enabled =
					lkbReferencias.Enabled = true;
					lkbTimbrarTodo.Enabled = true;
					lkbArchivos.Enabled = false;
					lkbAcercaDe.Enabled =
					lkbAyuda.Enabled = true;
					break;
				}
			}
		}
		/// <summary>
		/// Método encargado de Habilitar los Controles de la Forma
		/// </summary>
		private void HabilitaControles()
		{
			//Evalua cada estatus de la página
			switch ((Pagina.Estatus)Session["estatus"])
			{
				//En caso de que el estado de la página este en modo Nuevo/Edición
				case Pagina.Estatus.Nuevo:
				{
					//Encabezado Nomina
					txtFecIniPago.Enabled =
					txtFecFinPago.Enabled =
					txtDiasPago.Enabled =
					txtFechaPago.Enabled =
					txtFecNomina.Enabled =
					txtSucursal.Enabled =
					ddlPeriodicidadPago.Enabled =
					ddlFormaPago.Enabled =
					btnAceptar.Enabled =
					btnCancelar.Enabled = true;
					//Nomina Empleado
					txtEmpleado.Enabled =
					btnAgregarEmp.Enabled =
					gvNominaEmpleado.Enabled = false;
					break;
				}
				//En caso de que el estado de la página este en modo de lectura
				case Pagina.Estatus.Lectura:
				{
					//Encabezado Nomina
					txtFecIniPago.Enabled =
					txtFecFinPago.Enabled =
					txtDiasPago.Enabled =
					txtFechaPago.Enabled =
					txtFecNomina.Enabled =
					txtSucursal.Enabled =
					ddlPeriodicidadPago.Enabled =
					ddlFormaPago.Enabled =
					btnAceptar.Enabled =
					btnCancelar.Enabled =
					//Nomina Empleado
					txtEmpleado.Enabled =
					btnAgregarEmp.Enabled =
					gvNominaEmpleado.Enabled = false;
					break;
				}
				case Pagina.Estatus.Edicion:
				{
					//Encabezado Nomina
					txtFecIniPago.Enabled =
					txtFecFinPago.Enabled =
					txtDiasPago.Enabled =
					txtFechaPago.Enabled =
					txtFecNomina.Enabled =
					txtSucursal.Enabled =
					ddlPeriodicidadPago.Enabled =
					ddlFormaPago.Enabled =
					btnAceptar.Enabled =
					btnCancelar.Enabled =
					//Nomina Empleado
					txtEmpleado.Enabled =
					btnAgregarEmp.Enabled =
					gvNominaEmpleado.Enabled = true;
					break;
				}
			}
		}
		/// <summary>
		/// Método encargado de Timbrar la Nómina del Empleado
		/// </summary>
		private RetornoOperacion TimbraEmpleado()
		{
			//Declarando Objeto Resultado
			RetornoOperacion result = new RetornoOperacion();
			//Instanciando Nómina Empleado
			using (NomEmpleado ne = new NomEmpleado(Convert.ToInt32(gvNominaEmpleado.SelectedDataKey["Id"])))
			{
				//Validando que exista el Registro
				if (ne.habilitar)
				{
					//Timbrando Nómina del Empleado
					result = ne.ImportaTimbraNominaEmpleadoComprobante_V3_3(txtVersion.Text, txtSerie.Text, HostingEnvironment.MapPath("~/XSLT/FacturacionElectronica3_3/cadenaoriginal_3_3.xslt"), HostingEnvironment.MapPath("~/XSLT/FacturacionElectronica3_3/cadenaoriginal_3_3_desconectado.xslt"), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);//*/
				}
				else
					//Instanciando Excepción
					result = new RetornoOperacion("No Existe la Nómina del Empleado");
				//Validando Operación Exitosa
				if (result.OperacionExitosa)
				{
					//Cargando Nomina de Empleados
					CargaNominaEmpleados();
					//Marcando Fila
					Controles.MarcaFila(gvNominaEmpleado, ne.id_nomina_empleado.ToString(), "Id", "Id", OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), lblOrdenado.Text, Convert.ToInt32(ddlTamano.SelectedValue), true, 1);
					//Sumando Totales
					SumaTotalesNominaEmpleado();
				}
			}
			//Delvolvemos Resultado
			return result;
		}
		/// <summary>
		/// Método encargado de Timbrar la Nómina 
		/// </summary>
		/// <returns></returns>
		private RetornoOperacion TimbraNomina()
		{
			//Declarando Objeto Resultado
			RetornoOperacion result = new RetornoOperacion();
			//Instanciando Nomina
			using (SAT_CL.Nomina.Nomina12 nom = new SAT_CL.Nomina.Nomina12(Convert.ToInt32(Session["id_registro"])))
			{
				//Validando que exista el Registro
				if (nom.habilitar)
				{
					//Timbrando Nómina
					result = nom.TimbraNomina_V3_3(txtSerie.Text, HostingEnvironment.MapPath("~/XSLT/FacturacionElectronica3_3/cadenaoriginal_3_3.xslt"), HostingEnvironment.MapPath("~/XSLT/FacturacionElectronica3_3/cadenaoriginal_3_3_desconectado.xslt"), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
				}
				else
					//Instanciando Excepción
					result = new RetornoOperacion("No Existe la Nómina.");
				//Validando Operación Exitosa
				if (result.OperacionExitosa)
				{
					//Cargando Nomina de Empleados
					CargaNominaEmpleados();
					//Sumando Totales
					SumaTotalesNominaEmpleado();
				}
			}
			//Delvolvemos Resultado
			return result;
		}
		/// <summary>
		/// Método encargado de Inicializar los Valores de la Página
		/// </summary>
		private void InicializaValores()
		{
			//Evalua cada estatus de la página
			switch ((Pagina.Estatus)Session["estatus"])
			{
				//En caso de que el estado de la página este en modo Nuevo/Edición
				case Pagina.Estatus.Nuevo:
				{
					//Obteniendo Fecha Base
					DateTime fecha_base = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro();
					//Asignando Valores
					lblNoConsecutivo.Text = "Por Asignar";
					ddlTipoNomina.SelectedValue = "1";
					txtDiasPago.Text = "0.00";
					txtFecIniPago.Text = fecha_base.AddDays(-7).ToString("dd/MM/yyyy");
					txtFecFinPago.Text =
					txtFechaPago.Text =
					txtFecNomina.Text = fecha_base.ToString("dd/MM/yyyy");
					txtSucursal.Text = "Ninguna ID:0";
					ddlFormaPago.SelectedValue = "8";
					//Instanciando Compania
					using (SAT_CL.Global.CompaniaEmisorReceptor cer = new SAT_CL.Global.CompaniaEmisorReceptor(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
					{
						//Validando que exista la Compania
						if (cer.habilitar)
							//Asignando Compania Emisora
							txtCompania.Text = cer.nombre + " ID:" + cer.id_compania_emisor_receptor.ToString();
						else
							//Limpiando Control
							txtCompania.Text = "";
					}
					//Invocando Método de Calculo
					CalculaDiasPago();
					break;
				}
				case Pagina.Estatus.Lectura:
				case Pagina.Estatus.Edicion:
				{
					//Instanciando Nomina
					using (SAT_CL.Nomina.Nomina12 nom = new SAT_CL.Nomina.Nomina12(Convert.ToInt32(Session["id_registro"])))
					{
						//Validando que exista la Nomina
						if (nom.habilitar)
						{
							//Asignando Valores
							lblNoConsecutivo.Text = nom.no_consecutivo.ToString();
							txtVersion.Text = nom.version;
							ddlTipoNomina.SelectedValue = nom.id_tipo_nomina.ToString();
							txtDiasPago.Text = nom.dias_pago.ToString();
							txtFecIniPago.Text = nom.fecha_inicial_pago == DateTime.MinValue ? "" : nom.fecha_inicial_pago.ToString("dd/MM/yyyy");
							txtFecFinPago.Text = nom.fecha_final_pago == DateTime.MinValue ? "" : nom.fecha_final_pago.ToString("dd/MM/yyyy");
							txtFechaPago.Text = nom.fecha_pago == DateTime.MinValue ? "" : nom.fecha_pago.ToString("dd/MM/yyyy");
							txtFecNomina.Text = nom.fecha_nomina == DateTime.MinValue ? "" : nom.fecha_nomina.ToString("dd/MM/yyyy");
							ddlFormaPago.SelectedValue = nom.id_metodo_pago.ToString();
							//Cargando Tipos de Periodicidad de Pago
							SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlPeriodicidadPago, "", 3186, Convert.ToInt32(ddlTipoNomina.SelectedValue));
							ddlPeriodicidadPago.SelectedValue = nom.id_periodicidad_pago.ToString();
							//Instanciando Compania
							using (SAT_CL.Global.CompaniaEmisorReceptor cer = new SAT_CL.Global.CompaniaEmisorReceptor(nom.id_compania_emisor))
							{
								//Validando que exista la Compania
								if (cer.habilitar)
									//Asignando Compania Emisora
									txtCompania.Text = cer.nombre + " ID:" + cer.id_compania_emisor_receptor.ToString();
								else
									//Limpiando Control
									txtCompania.Text = "";
							}
							//Instanciando Sucursal
							using (SAT_CL.Global.Sucursal suc = new SAT_CL.Global.Sucursal(nom.id_sucursal))
							{
								//Validando que exista la Sucursal
								if (suc.habilitar)
									//Asignando Sucursal
									txtSucursal.Text = suc.nombre + " ID:" + suc.id_sucursal.ToString();
								else
									//Limpiando Control
									txtSucursal.Text = "Ninguna ID:0";
							}
						}
					}
					break;
				}
			}
		}
		/// <summary>
		/// Método encargado de Calcular los Dias Pagados
		/// </summary>
		private void CalculaDiasPago()
		{
			//Declarando Variables Auxiliares
			DateTime fecha_inicio, fecha_fin;
			//Obteniendo Fechas
			DateTime.TryParse(txtFecIniPago.Text, out fecha_inicio);
			DateTime.TryParse(txtFecFinPago.Text, out fecha_fin);
			//Validando Fechas
			if (fecha_inicio != DateTime.MinValue && fecha_fin != DateTime.MinValue)
			{
				//Calculando Dias de Pago
				TimeSpan dias_pago = fecha_fin - fecha_inicio;
				//Asignando Dias
				txtDiasPago.Text = (dias_pago.Days + 1).ToString() + ".000";
			}
			else
				//Asignando Dias
				txtDiasPago.Text = "0.000";
		}
		/// <summary>
		/// Método encargado de Guardar la Nomina
		/// </summary>
		private void GuardaNomina()
		{
			//Declarando Objeto de Retorno
			RetornoOperacion result = new RetornoOperacion();
			//Declarando Variables para Fecha
			DateTime fec_nomina, fec_ini_pago, fec_fin_pago, fec_pago;
			//Obteniendo Fechas
			DateTime.TryParse(txtFecNomina.Text, out fec_nomina);
			DateTime.TryParse(txtFecIniPago.Text, out fec_ini_pago);
			DateTime.TryParse(txtFecFinPago.Text, out fec_fin_pago);
			DateTime.TryParse(txtFechaPago.Text, out fec_pago);
			//Evalua cada estatus de la página
			switch ((Pagina.Estatus)Session["estatus"])
			{
				//En caso de que el estado de la página este en modo Nuevo
				case Pagina.Estatus.Nuevo:
				{
					//Insertando Nuevo
					result = SAT_CL.Nomina.Nomina12.InsertaNomina(Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:", 1)), 0, 0, txtVersion.Text, Convert.ToByte(ddlTipoNomina.SelectedValue), fec_pago, fec_ini_pago, fec_fin_pago, fec_nomina, Convert.ToDecimal(txtDiasPago.Text == "" ? "0" : txtDiasPago.Text), Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtSucursal.Text, "ID:", 1)), Convert.ToByte(ddlPeriodicidadPago.SelectedValue), Convert.ToByte(ddlFormaPago.SelectedValue), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
					break;
				}
				case Pagina.Estatus.Edicion:
				{
					//Instanciando Nomina
					using (SAT_CL.Nomina.Nomina12 nom = new SAT_CL.Nomina.Nomina12(Convert.ToInt32(Session["id_registro"])))
					{
						//Validando que exista la Nomina
						if (nom.habilitar)
							//Insertando Nuevo
							result = nom.EditaNomina(Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:", 1)), nom.no_consecutivo, nom.id_nomina_origen, nom.version, Convert.ToByte(ddlTipoNomina.SelectedValue), fec_pago, fec_ini_pago, fec_fin_pago, fec_nomina, Convert.ToDecimal(txtDiasPago.Text == "" ? "0" : txtDiasPago.Text), Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtSucursal.Text, "ID:", 1)), Convert.ToByte(ddlPeriodicidadPago.SelectedValue), Convert.ToByte(ddlFormaPago.SelectedValue), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
						else
							//Instanciando Error
							result = new RetornoOperacion("No existe la Nomina");
					}
					break;
				}
			}
			//Validando Operación Exitosa
			if (result.OperacionExitosa)
			{
				//Asignando Registro
				Session["id_registro"] = result.IdRegistro;
				//Cambiando Estatus de Sesión
				Session["estatus"] = Pagina.Estatus.Edicion;
				//Invocando Método de Inicialización
				InicializaPagina();
			}
			//Mostrando Mensaje de Operación
			ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
		}
		/// <summary>
		/// Método encargado de Cargar la Nomina de los Empleados
		/// </summary>
		private void CargaNominaEmpleados()
		{
			//Validando estatus de Página
			switch ((Pagina.Estatus)Session["estatus"])
			{
				case Pagina.Estatus.Nuevo:
				{
					//Cargando GridView
					Controles.InicializaGridview(gvNominaEmpleado);
					//Añadiendo Resultado a Sesión
					Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
					break;
				}
				case Pagina.Estatus.Lectura:
				case Pagina.Estatus.Edicion:
				{
					CargaDetallesNominaEmpleado();
					break;
				}
			}
			//Mostrando Totales
			SumaTotalesNominaEmpleado();
		}
		/// <summary>
		/// Método encargado de realizar la consulta en la BD y con ellos llenar el gvNominaEmpleado
		/// </summary>
		private void CargaDetallesNominaEmpleado()
		{
			//Instanciando Nomina de Empleados
			using (DataTable dtNominaEmpleados = NomEmpleado.ObtieneNominasEmpleado(Convert.ToInt32(Session["id_registro"])))
			{
				//Validando que existan Registros
				if (Validacion.ValidaOrigenDatos(dtNominaEmpleados))
				{
					//Cargando GridView
					Controles.CargaGridView(gvNominaEmpleado, dtNominaEmpleados, "Id", lblOrdenado.Text, true, 1);
					//Añadiendo Resultado a Sesión
					Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtNominaEmpleados, "Table");
				}
				else
				{
					//Cargando GridView
					Controles.InicializaGridview(gvNominaEmpleado);
					//Añadiendo Resultado a Sesión
					Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
				}
			}
		}
		/// <summary>
		/// Método encargado de Sumar los Totales del GridView de los Conceptos
		/// </summary>
		private void SumaTotalesNominaEmpleado()
		{
			//Validando Origend de Datos
			if (Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table")))
			{
				//Mostrando Totales
				gvNominaEmpleado.FooterRow.Cells[5].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Aguinaldo)", "")));
				gvNominaEmpleado.FooterRow.Cells[6].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Sueldo)", "")));
				gvNominaEmpleado.FooterRow.Cells[7].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(OtrasPercepciones)", "")));
				gvNominaEmpleado.FooterRow.Cells[8].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(SeparacionIndemnizacion)", "")));
				gvNominaEmpleado.FooterRow.Cells[9].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(IMSS)", "")));
				gvNominaEmpleado.FooterRow.Cells[10].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(ISPT)", "")));
				gvNominaEmpleado.FooterRow.Cells[11].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Infonavit)", "")));
				gvNominaEmpleado.FooterRow.Cells[12].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(OtrasDeducciones)", "")));
				gvNominaEmpleado.FooterRow.Cells[13].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(HrsExtra)", "")));
				gvNominaEmpleado.FooterRow.Cells[14].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Incapacidad)", "")));
				gvNominaEmpleado.FooterRow.Cells[15].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(OtrosPagos)", "")));
				gvNominaEmpleado.FooterRow.Cells[16].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(TPercepciones)", "")));
				gvNominaEmpleado.FooterRow.Cells[17].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(TDeducciones)", "")));
				gvNominaEmpleado.FooterRow.Cells[18].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(TPagado)", "")));
			}
			else
			{
				//Mostrando Totales
				gvNominaEmpleado.FooterRow.Cells[5].Text =
				gvNominaEmpleado.FooterRow.Cells[6].Text =
				gvNominaEmpleado.FooterRow.Cells[7].Text =
				gvNominaEmpleado.FooterRow.Cells[8].Text =
				gvNominaEmpleado.FooterRow.Cells[9].Text =
				gvNominaEmpleado.FooterRow.Cells[10].Text =
				gvNominaEmpleado.FooterRow.Cells[11].Text =
				gvNominaEmpleado.FooterRow.Cells[12].Text =
				gvNominaEmpleado.FooterRow.Cells[13].Text =
				gvNominaEmpleado.FooterRow.Cells[14].Text =
				gvNominaEmpleado.FooterRow.Cells[15].Text =
				gvNominaEmpleado.FooterRow.Cells[16].Text =
				gvNominaEmpleado.FooterRow.Cells[17].Text =
				gvNominaEmpleado.FooterRow.Cells[18].Text = string.Format("{0:C2}", 0);
			}
		}
		/// <summary>
		///  Método que permie la consulta del historial de modificicaciones realizadas a un registro 
		/// </summary>
		/// <param name="idRegistro">ID que permite identificar un registro de TipoPago</param>
		/// <param name="idTabla">Identificador de la tabla TipoPago</param>
		/// <param name="Titulo">Encabezado de la ventana Bitacora</param>
		private void InicializaBitacora(string idRegistro, string idTabla, string Titulo)
		{
			//Creación de la variable url, que almacena una ruta de ubicación de la ventana, que contendra la bitacora de un registros de  TipoPago.
			string url = Cadena.RutaRelativaAAbsoluta("~/Nomina/Nomina12.aspx", "~/Accesorios/BitacoraRegistro.aspx?idT=" + idTabla + "&idR=" + idRegistro + "&tB=" + Titulo);
			//Variable que almacena la resolucion de la ventana bitacora
			string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
			//Invoca el método de la clase ScriptServer que abrira una nueva ventana acorde a una ubicacion, un titulo de la ventana, una configuracion de dimensiones.
			ScriptServer.AbreNuevaVentana(url, "Bitacora Tipo Pago", configuracion, Page);
		}
		/// <summary>
		/// Método que permite la busqueda de registros en la base de datos pertenecientes a la tabla TipoPago
		/// </summary>
		/// <param name="idTabla">Id que identifica a la tabla TipoPago registrada en la base de datos</param>
		/// <param name="idCompania"> Id que identifica a una compañia</param>
		private void InicializaAperturaRegistro(int idTabla, int idCompania)
		{
			//Asigna a la variable de session id_tabla el valor del parametro idTabla
			Session["id_tabla"] = idTabla;
			//Asigna valores a la variable url con la ruta de ubicacion de la ventana Abrir registros con los datos obtenidos de la tabla TipoPago
			string url = Cadena.RutaRelativaAAbsoluta("~/Nomina/Nomina12.aspx", "~/Accesorios/AbrirRegistro.aspx?P1=" + idCompania.ToString());
			//Define las dimensiones de la ventana Abrir registros de TipoPago
			string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
			//Invoca al método de la variable ScripServer que abrira una nueva ventana con los registros pertenecientes a la tabla TipoPago
			ScriptServer.AbreNuevaVentana(url, "Abrir Registro Tipo Pago", configuracion, Page);
		}
		/// <summary>
		/// Método encargado de Gestionar las Ventanas Modales
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="command"></param>
		private void GestionaVentanas(Control sender, string command)
		{
			//Validando Comando
			switch (command)
			{
				case "Cancelar":
				{
					//Alternando Ventana
					ScriptServer.AlternarVentana(sender, "CancelarNomina", "contenedorVentanaCanceladoNomina", "ventanaCanceladoNomina");
					break;
				}
				case "DetalleNomina":
				{
					//Alternando Ventana
					ScriptServer.AlternarVentana(sender, "AltaDetallesNomina", "contenedorVentanaDetallesNomina", "ventanaDetallesNomina");
					break;
				}
				case "NominaOtros":
				{
					//Alternando Ventana
					ScriptServer.AlternarVentana(sender, "DetallesNominaOtros", "contenedorVentanaNominaOtros", "ventanaNominaOtros");
					break;
				}
				case "SeparacionIndemnizacion":
				{
					//Alternar ventana
					ScriptServer.AlternarVentana(sender, "SeparacionIndemnizacion", "contenedorVentanaSeparacionIndemnizacion", "ventanaSeparacionIndemnizacion");
					break;
				}
				case "Timbrar":
				{
					//Alternando Ventana
					ScriptServer.AlternarVentana(sender, "TimbrarNomina", "contenedorVentanaTimbradoNomina", "ventanaTimbradoNomina");
					break;
				}
			}
		}
		/// <summary>
		/// Realiza la descarga del XML del comprobante
		/// </summary>
		/// <param name="id_comprobante33">Comprobante de Nómina</param>
		private void DescargarXMLv33(int id_comprobante33)
		{
			//Instanciando registro en sesión
			using (SAT_CL.FacturacionElectronica33.Comprobante c = new SAT_CL.FacturacionElectronica33.Comprobante(id_comprobante33))
			{
				//Si existe y está generado
				if (c.bit_generado)
				{
					//Obteniendo bytes del archivo XML
					byte[] cfdi_xml = System.IO.File.ReadAllBytes(c.ruta_xml);
					//Realizando descarga de archivo
					if (cfdi_xml.Length > 0)
					{
						//Instanciando al emisor
						using (CompaniaEmisorReceptor em = new CompaniaEmisorReceptor(c.id_compania_emisor))
							Archivo.DescargaArchivo(cfdi_xml, string.Format("{0}_{1}{2}.xml", em.nombre_corto != "" ? em.nombre_corto : em.rfc, c.serie, c.folio), Archivo.ContentType.binary_octetStream);
					}
				}
			}
		}
		/// <summary>
		/// Realiza la descarga del XML del comprobante
		/// </summary>
		/// <param name="id_comprobante">Comprobante de Nómina</param>
		private void DescargarXML(int id_comprobante)
		{
			//Instanciando registro en sesión
			using (SAT_CL.FacturacionElectronica.Comprobante c = new SAT_CL.FacturacionElectronica.Comprobante(id_comprobante))
			{
				//Si existe y está generado
				if (c.generado)
				{
					//Obteniendo bytes del archivo XML
					byte[] cfdi_xml = System.IO.File.ReadAllBytes(c.ruta_xml);
					//Realizando descarga de archivo
					if (cfdi_xml.Length > 0)
					{
						//Instanciando al emisor
						using (CompaniaEmisorReceptor em = new CompaniaEmisorReceptor(c.id_compania_emisor))
							Archivo.DescargaArchivo(cfdi_xml, string.Format("{0}_{1}{2}.xml", em.nombre_corto != "" ? em.nombre_corto : em.rfc, c.serie, c.folio.ToString()), Archivo.ContentType.binary_octetStream);
					}
				}
			}
		}
		#region Métodos Modal "Detalle Nomina"
		/// <summary>
		/// Método encargado de Inicializar Valores de Timbrado
		/// </summary>
		private void InicializaValoresTimbradoNomina()
		{
			txtSerie.Text = "";
		}
		/// <summary>
		/// Inicializando Valores de los Controles
		/// </summary>
		/// <param name="id_detalle_nomina">Detalle de Nomina</param>
		/// <param name="tipo_aplicacion">Tipo de Aplicación</param>
		/// <param name="id_concepto_sat">Concepto de Nomina del SAT</param>
		private void InicializaValoresDetalle(int id_detalle_nomina, SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion tipo_aplicacion, int id_concepto_sat)
		{
			//Validando que exista un Concepto de Nomina
			if (id_concepto_sat > 0)
			{
				//Cargando Tipos de Pago (Conceptos Principales)
				using (DataTable dtConceptos = SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(72, "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, id_concepto_sat.ToString(), (int)tipo_aplicacion, ""))
				{
					//Validando que existan Conceptos
					if (Validacion.ValidaOrigenDatos(dtConceptos))
						//Cargando Controles
						Controles.CargaDropDownList(ddlConcepto, dtConceptos, "id", "descripcion");
					else
						//Inicializando Control
						Controles.InicializaDropDownList(ddlConcepto, "-- Ninguno");
				}
			}
			else
			{
				//Cargando Tipos de Pago (Conceptos Principales)
				using (DataTable dtConceptos = SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(73, "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", (int)tipo_aplicacion, ""))
				{
					//Validando que existan Conceptos
					if (Validacion.ValidaOrigenDatos(dtConceptos))
						//Cargando Controles
						Controles.CargaDropDownList(ddlConcepto, dtConceptos, "id", "descripcion");
					else
						//Inicializando Control
						Controles.InicializaDropDownList(ddlConcepto, "-- Ninguno");
				}
			}
			//Validando que exista el Detalle de Nomina
			if (id_detalle_nomina > 0)
			{
				//Validamos Tipo de Pagos 
				if (tipo_aplicacion == SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion.Bonificacion ||
				tipo_aplicacion == SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion.Percepcion)
				{
					//Obtenemos Valores
					int id_concepto = 0;
					decimal importe_gravado = 00M;
					decimal importe_exento = 00M;
					//Obtenemos Valores de las Percepciones
					SAT_CL.Nomina.EsquemaRegistro.ObtieneDetalleNominaPercepcion(Convert.ToInt32(gvNominaEmpleado.SelectedDataKey["Id"]), Convert.ToInt32(gvDetalleNomina.SelectedValue), out id_concepto, out importe_gravado, out importe_exento);
					//Asignando Valores
					ddlConcepto.SelectedValue = id_concepto.ToString();
					txtImporteGravado.Text = importe_gravado.ToString();
					txtImporteExento.Text = importe_exento.ToString();
				}
				else if (tipo_aplicacion == SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion.Deduccion ||
				tipo_aplicacion == SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion.Descuento)
				{
					//Obtenemos Valores
					int id_concepto = 0;
					decimal importe = 00M;
					//Obtenemos Valores de las Percepciones
					SAT_CL.Nomina.EsquemaRegistro.ObtieneDetalleNominaDeduccion(Convert.ToInt32(gvNominaEmpleado.SelectedDataKey["Id"]), Convert.ToInt32(gvDetalleNomina.SelectedValue), out id_concepto, out importe);
					//Asignando Valores
					ddlConcepto.SelectedValue = id_concepto.ToString();
					txtImporte.Text = importe.ToString();
				}
				else if (tipo_aplicacion == SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion.Otros)
				{
					//Obtenemos Valores
					int id_concepto = 0;
					decimal importe = 00M;
					decimal importe_subsidio_causado = 00M;
					bool valor_subsidio_causado = false;
					//Obtenemos Valores de las Percepciones
					SAT_CL.Nomina.EsquemaRegistro.ObtieneDetalleNominaOtros(Convert.ToInt32(gvNominaEmpleado.SelectedDataKey["Id"]), Convert.ToInt32(gvDetalleNomina.SelectedValue), out id_concepto, out importe, out importe_subsidio_causado, out valor_subsidio_causado);
					//Asignando Valores
					ddlConcepto.SelectedValue = id_concepto.ToString();
					txtImporte.Text = importe.ToString();
					txtSubsidioCausado.Text = importe_subsidio_causado.ToString();
					txtSubsidioCausado.Enabled = chkSubsidioCausado.Checked = valor_subsidio_causado;
				}
			}
			else
			{
				//Asignando Valores
				txtImporteGravado.Text =
				txtImporteExento.Text = "0.00";
				txtImporte.Text = "0.00";
				chkSubsidioCausado.Checked = false;
				txtSubsidioCausado.Text = "0.00";
			}
			//Validamos Tipo de Pagos 
			if (tipo_aplicacion == SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion.Bonificacion ||
			tipo_aplicacion == SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion.Percepcion)
			{
				//Deahabilitamos Control de Importes de Percepciones
				lblImporteGravado.Text = "ImporteGravado";
				lblImporteExento.Visible =
				txtImporteExento.Visible = true;
				txtImporteGravado.Visible = true;
				txtImporte.Visible =
				lblSubsidioCausado.Visible =
				chkSubsidioCausado.Visible =
				txtSubsidioCausado.Visible = false;
			}
			else if (tipo_aplicacion == SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion.Deduccion ||
			tipo_aplicacion == SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion.Descuento)
			{
				//Deahabilitamos Control de Importes de Deducciones
				lblImporteExento.Visible =
				txtImporteExento.Visible = false;
				txtImporteGravado.Visible = false;
				txtImporte.Visible = true;
				lblImporteGravado.Text = "Importe";
				lblSubsidioCausado.Visible =
				chkSubsidioCausado.Visible =
				txtSubsidioCausado.Visible = false;
			}
			else if (tipo_aplicacion == SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion.Otros)
			{
				//Deahabilitamos Control de Importes de Deducciones
				lblImporteExento.Visible =
				txtImporteExento.Visible = false;
				txtImporteGravado.Visible = false;
				txtImporte.Visible = true;
				lblImporteGravado.Text = "Importe";
				lblSubsidioCausado.Visible =
				chkSubsidioCausado.Visible =
				txtSubsidioCausado.Visible = true;
			}
		}
		/// <summary>
		/// Método encargado de Cargar los Detalles de Nomina
		/// </summary>
		/// <param name="id_nomina_empleado">Nomina del Empleado</param>
		///<param name="clave_concepto_sat">Clave del Concepto de Nómina</param>
		/// <param name="tipo_aplicacion"></param>
		private void CargaDetallesNomina(int id_nomina_empleado, string clave_concepto_sat, int id_tipo_aplicacion)
		{
			//Instanciando Nomina de Empleados
			using (DataTable dtDetallesNominaEmpleados = SAT_CL.Nomina.EsquemaRegistro.ObtieneDetalleNominaEmpleado(id_nomina_empleado, clave_concepto_sat, id_tipo_aplicacion))
			{
				//Validando que existan Registros
				if (Validacion.ValidaOrigenDatos(dtDetallesNominaEmpleados))
				{
					//Cargando GridView
					Controles.CargaGridView(gvDetalleNomina, dtDetallesNominaEmpleados, "Id-IdEsquemaSubsidio", lblOrdenadoDet.Text, true, 1);
					//Añadiendo Resultado a Sesión
					Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtDetallesNominaEmpleados, "Table1");
				}
				else
				{
					//Cargando GridView
					Controles.InicializaGridview(gvDetalleNomina);
					//Añadiendo Resultado a Sesión
					Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
				}
			}
		}
		/// <summary>
		/// Método encargado de Guardar el Detalle de Nomina
		/// </summary>
		/// <param name="tipo_aplicacion">Tipo de Aplicación</param>
		private void GuardaDetalleNomina()
		{
			//Declarando Objeto de Retorno
			RetornoOperacion result = new RetornoOperacion();
			//Validando Concepto
			if (ddlConcepto.SelectedValue != "0")
			{
				//Validando si existe un Registro Seleccionado
				if (gvDetalleNomina.SelectedIndex != -1)
				{
					//Instanciando Detalle de Nomina
					using (SAT_CL.Nomina.EsquemaRegistro objEsquemaRegistro = new SAT_CL.Nomina.EsquemaRegistro(Convert.ToInt32(gvDetalleNomina.SelectedDataKey["Id"])))
					{
						//Validando que exista el Registro
						if (objEsquemaRegistro.habilitar)
							//Insertando Detalle de Nomina
							result = SAT_CL.Nomina.EsquemaRegistro.ActualizaDetalleNomina(Convert.ToInt32(gvDetalleNomina.SelectedValue), Convert.ToInt32(gvDetalleNomina.SelectedDataKey["IdEsquemaSubsidio"]), txtVersion.Text, Convert.ToInt32(gvNominaEmpleado.SelectedDataKey["Id"]),
							Convert.ToInt32(ddlConcepto.SelectedValue), Convert.ToDecimal(txtImporteGravado.Text), Convert.ToDecimal(txtImporteExento.Text), Convert.ToDecimal(txtImporte.Text), chkSubsidioCausado.Checked,
							Convert.ToDecimal(txtSubsidioCausado.Text),
							((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
						else
							//Instanciando Excepción
							result = new RetornoOperacion("No Existe el Detalle");
					}
				}
				else
					//Insertando Detalle de Nomina
					result = SAT_CL.Nomina.EsquemaRegistro.ActualizaDetalleNomina(0, 0, txtVersion.Text, Convert.ToInt32(gvNominaEmpleado.SelectedDataKey["Id"]), Convert.ToInt32(ddlConcepto.SelectedValue), Convert.ToDecimal(txtImporteGravado.Text), Convert.ToDecimal(txtImporteExento.Text), Convert.ToDecimal(txtImporte.Text), chkSubsidioCausado.Checked, Convert.ToDecimal(Convert.ToDecimal(txtSubsidioCausado.Text)), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
			}
			else
				//Instanciando Excepción
				result = new RetornoOperacion("No Existe el Concepto");
			//Validando Operación Exitosa
			if (result.OperacionExitosa)
			{
				//Instanciando Registro del Elemnto Agrupado
				using (SAT_CL.Nomina.EsquemaRegistro objEsquemaRegistro = new SAT_CL.Nomina.EsquemaRegistro(result.IdRegistro))
				{
					//Obtenemos Valores
					int id_concepto = 0;
					decimal importe_gravado = 00M;
					decimal importe_exento = 00M;
					decimal importe = 00M;
					decimal importe_subsidio_causado = 00M;
					bool valor_subsidio_causado = false;
					//De acuerdo al Esquema
					if (objEsquemaRegistro.id_esquema == 41)
					{
						SAT_CL.Nomina.EsquemaRegistro.ObtieneDetalleNominaPercepcion(Convert.ToInt32(gvNominaEmpleado.SelectedDataKey["Id"]), objEsquemaRegistro.id_esquema_registro, out id_concepto, out importe_gravado, out importe_exento);
					}
					else if (objEsquemaRegistro.id_esquema == 74)
					{
						//Obtenemos Valores de las Percepciones
						SAT_CL.Nomina.EsquemaRegistro.ObtieneDetalleNominaDeduccion(Convert.ToInt32(gvNominaEmpleado.SelectedDataKey["Id"]), objEsquemaRegistro.id_esquema_registro, out id_concepto, out importe);
					}
					else if (objEsquemaRegistro.id_esquema == 82)
					{
						//Obtenemos Valores de las Percepciones
						SAT_CL.Nomina.EsquemaRegistro.ObtieneDetalleNominaOtros(Convert.ToInt32(gvNominaEmpleado.SelectedDataKey["Id"]), objEsquemaRegistro.id_esquema_registro, out id_concepto, out importe, out importe_subsidio_causado, out valor_subsidio_causado);
					}
					//Instanciando Tipo Recurrente
					using (SAT_CL.Liquidacion.TipoCobroRecurrente tcr = new SAT_CL.Liquidacion.TipoCobroRecurrente(id_concepto))
					{
						//Validando Percepción
						if (((SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion)tcr.id_tipo_aplicacion == SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion.Percepcion || (SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion)tcr.id_tipo_aplicacion == SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion.Bonificacion) && (tcr.id_concepto_sat_nomina == 1 || tcr.id_concepto_sat_nomina == 2))
						{
							//Inicializando Valores del Detalle
							InicializaValoresDetalle(0, (SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion)tcr.id_tipo_aplicacion, tcr.id_concepto_sat_nomina);
							//Cargando Detalles de Nomina
							CargaDetallesNomina(Convert.ToInt32(gvNominaEmpleado.SelectedDataKey["Id"]), Catalogo.RegresaDescripcioValorCadena(92, tcr.id_concepto_sat_nomina), tcr.id_tipo_aplicacion);
						}
						else if ((SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion)tcr.id_tipo_aplicacion == SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion.Deduccion && (tcr.id_concepto_sat_nomina == 1 || tcr.id_concepto_sat_nomina == 2 || tcr.id_concepto_sat_nomina == 9))
						{
							//Inicializando Valores del Detalle
							InicializaValoresDetalle(0, (SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion)tcr.id_tipo_aplicacion, tcr.id_concepto_sat_nomina);
							//Cargando Detalles de Nomina
							CargaDetallesNomina(Convert.ToInt32(gvNominaEmpleado.SelectedDataKey["Id"]), Catalogo.RegresaDescripcioValorCadena(91, tcr.id_concepto_sat_nomina), tcr.id_tipo_aplicacion);
						}
						else
						{
							//Inicializando Valores del Detalle
							InicializaValoresDetalle(0, (SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion)tcr.id_tipo_aplicacion, 0);
							//Cargando Detalles de Nomina
							CargaDetallesNomina(Convert.ToInt32(gvNominaEmpleado.SelectedDataKey["Id"]), "0", tcr.id_tipo_aplicacion);
						}
						//Inicializando Indices
						Controles.InicializaIndices(gvDetalleNomina);
						//Obteniendo Nomina de Empleado
						int nomina_empleado = Convert.ToInt32(gvNominaEmpleado.SelectedDataKey["Id"]);
						//Cargando Nomina de Empleados
						CargaNominaEmpleados();
						// Marcando Fila
						Controles.MarcaFila(gvNominaEmpleado, nomina_empleado.ToString(), "Id", "Id", OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), lblOrdenado.Text, Convert.ToInt32(ddlTamano.SelectedValue), true, 1);
						upgvNominaEmpleado.Update();
						//Mostrando Totales
						SumaTotalesNominaEmpleado();
					}
				}
			}
			//Mostrando Mensaje de la Operación
			ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
		}
		#endregion
		#region Métodos Modal "Nomina Otros"
		/// <summary>
		/// Inicializa Controles de Nómina Otros
		/// </summary>
		/// <param name="id_nomina_empleado"></param>
		/// <param name="id_esquema_registro"></param>
		/// <param name="id_tipo_nomina_otros"></param>
		private void InicializaValoresNominaOtros(int id_nomina_empleado, int id_esquema_registro, byte id_tipo_nomina_otros)
		{
			//Cargando Catalogo
			SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipo, "", 3149);
			//Asignando Tipo
			ddlTipo.SelectedValue = id_tipo_nomina_otros.ToString();
			//Cargando Catalogo
			SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlSubTipo, "", 3150, Convert.ToInt32(ddlTipo.SelectedValue));
			//Validando que exista el Detalle de Nomina
			if (id_esquema_registro > 0)
			{
				//Instanciamos Esquema Registro
				using (SAT_CL.Nomina.EsquemaRegistro objEsquemaRegistro = new SAT_CL.Nomina.EsquemaRegistro(id_esquema_registro))
				{
					//Validando que existe el Detalle
					if (objEsquemaRegistro.habilitar)
					{
						//Si el Tipo es Horas Extras
						if (id_tipo_nomina_otros == 2)
						{
							//Declaramos variable
							int dias = 0;
							byte id_tipo_horas = 0;
							int horas_extras = 0;
							decimal importe_pagado = 00M;
							//Obtenemos Valores de las Horas Extras
							SAT_CL.Nomina.EsquemaRegistro.ObtieneDetalleHorasExtras(id_nomina_empleado, objEsquemaRegistro.id_esquema_registro,
							out dias, out id_tipo_horas, out horas_extras, out importe_pagado);
							//Asignando Valores
							ddlSubTipo.SelectedValue = id_tipo_horas.ToString();
							txtDias.Text = dias.ToString();
							txtCantidad.Text = horas_extras.ToString();
							txtImportePagado.Text = importe_pagado.ToString();
						}
						//Incapacidades
						else if (id_tipo_nomina_otros == 1)
						{
							//Declaramos variable
							int dias = 0;
							byte id_tipo = 0;
							decimal importe_pagado = 00M;
							//Obtenemos Valores de las Horas Extras
							SAT_CL.Nomina.EsquemaRegistro.ObtieneDetalleIncapacidades(id_nomina_empleado, objEsquemaRegistro.id_esquema_registro, out dias, out id_tipo, out importe_pagado);
							//Asignando Valores
							ddlSubTipo.SelectedValue = id_tipo.ToString();
							txtDias.Text = dias.ToString();
							txtCantidad.Text = "0";
							txtImportePagado.Text = importe_pagado.ToString();
						}
					}
				}
			}
			else
			{
				//Asignando Valores
				txtDias.Text = "0";
				txtImportePagado.Text = "0.00";
				txtCantidad.Text = "0";
			}
			//Validando Tipo de Nomina Otros
			if (id_tipo_nomina_otros == 2)
				//Viasualizando Control
				txtCantidad.Enabled = true;
			else if (id_tipo_nomina_otros == 1)
				//Viasualizando Control
				txtCantidad.Enabled = false;
		}
		/// <summary>
		/// Método encargado de Cargar la  Nomina de Otros del Empleado
		/// </summary>
		/// <param name="id_nomina_empleado">Nomina de Empleado</param>
		/// <param name="id_tipo_nomina_otros">Tipo de Nomina</param>
		private void CargaNominaOtros(int id_nomina_empleado, int id_tipo_nomina_otros)
		{
			//Instanciando Nomina de Empleados
			using (DataTable dtNominaOtrosEmpleados = SAT_CL.Nomina.EsquemaRegistro.CargaHorasExtraIncapacidad(id_nomina_empleado, id_tipo_nomina_otros))
			{
				//Validando que existan Registros
				if (Validacion.ValidaOrigenDatos(dtNominaOtrosEmpleados))
				{
					//Cargando GridView
					Controles.CargaGridView(gvNominaOtros, dtNominaOtrosEmpleados, "Id", lblOrdenadoDet.Text, true, 1);
					//Añadiendo Resultado a Sesión
					Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtNominaOtrosEmpleados, "Table2");
				}
				else
				{
					//Cargando GridView
					Controles.InicializaGridview(gvNominaOtros);
					//Añadiendo Resultado a Sesión
					Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table2");
				}
			}
		}
		/// <summary>
		/// Método encargado de Guardar el Detalle de Nomina
		/// </summary>
		private void GuardaNominaOtros()
		{
			//Declarando Objeto de Retorno
			RetornoOperacion result = new RetornoOperacion();
			//Validando si existe un Registro Seleccionado
			if (gvNominaOtros.SelectedIndex != -1)
			{
				//Instanciando Detalle de Nomina
				using (SAT_CL.Nomina.EsquemaRegistro objeEsquemaRegistro = new SAT_CL.Nomina.EsquemaRegistro(Convert.ToInt32(gvNominaOtros.SelectedDataKey["Id"])))
				{
					//Validando que exista el Registro
					if (objeEsquemaRegistro.habilitar)
					{
						////Editando Horas Extras e Incapacidades
						result = EditaHrExtraIncapacidad();
					}
					else
						//Instanciando Excepción
						result = new RetornoOperacion("No Existe la Nomina Otros");
				}
			}
			else
				//Insertando Hrs Extra o Incapacidades
				result = InsertaHrExtraIncapacidad();
			//Validando Operación Exitosa
			if (result.OperacionExitosa)
			{
				//Instanciando Detalle de Nomina
				using (SAT_CL.Nomina.EsquemaRegistro objEsquemaRegistro = new SAT_CL.Nomina.EsquemaRegistro(result.IdRegistro))
				{
					//Inicializando Valores del Detalle
					InicializaValoresNominaOtros(Convert.ToInt32(gvNominaEmpleado.SelectedDataKey["Id"]), 0, Convert.ToByte(ddlTipo.SelectedValue));
					//Cargando Nomina de Otros
					CargaNominaOtros(Convert.ToInt32(gvNominaEmpleado.SelectedDataKey["Id"]), Convert.ToByte(ddlTipo.SelectedValue));
					//Inicializando Indices
					Controles.InicializaIndices(gvNominaOtros);
					//Obteniendo Nomina de Empleado
					int nomina_empleado = Convert.ToInt32(gvNominaEmpleado.SelectedDataKey["Id"]);
					//Cargando Nomina de Empleados
					CargaNominaEmpleados();
					// Marcando Fila
					Controles.MarcaFila(gvNominaEmpleado, nomina_empleado.ToString(), "Id", "Id", OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), lblOrdenado.Text, Convert.ToInt32(ddlTamano.SelectedValue), true, 1);
					upgvNominaEmpleado.Update();
					//Mostrando Totales
					SumaTotalesNominaEmpleado();
				}
			}
			//Mostrando Mensaje de la Operación
			ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
		}
		#endregion
		/// <summary>
		/// Enlace a datos de filas de GV
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void gvDetalleNomina_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			//Si existen registros
			if (gvDetalleNomina.DataKeys.Count > 0)
			{
				//Para columnas de datos
				if (e.Row.RowType == DataControlRowType.DataRow)
				{
					//Recuperando origen de datos de la fila
					DataRow row = ((DataRowView)e.Row.DataItem).Row;
					//Encontrando controles de interés
					using (Label lblImporteGravado = (Label)e.Row.FindControl("lblImporteGravado"), lblImporteExento = (Label)e.Row.FindControl("lblImporteExento"), lblImporte = (Label)e.Row.FindControl("lblImporte"), lblId = (Label)e.Row.FindControl("lblId"))
					{
						//Instanciamos Registro
						using (SAT_CL.Nomina.EsquemaRegistro objEsquemaRegistro = new SAT_CL.Nomina.EsquemaRegistro(Convert.ToInt32(lblId.Text)))
						{
							/**** APLICANDO CONFIGURACIÓN DE ASIGNACIÓN ****/
							//De acuerdo al Tipo Detalle
							//Percepción
							if (objEsquemaRegistro.id_esquema == 41)
							{
								//Ocultamos Controles
								lblImporteGravado.Visible =
								lblImporteExento.Visible = true;
								lblImporte.Visible = false;
							}
							else
							{
								//Ocultamos Controles
								lblImporteGravado.Visible =
								lblImporteExento.Visible = false;
								lblImporte.Visible = true;
							}
						}
					}
				}
			}
		}
		/// <summary>
		/// Método encargado de Insertar las Horas Extras  e Incapacidades
		/// </summary>
		/// <returns></returns>
		private RetornoOperacion InsertaHrExtraIncapacidad()
		{
			//Declaramos Objeto Resultado
			RetornoOperacion resultado = new RetornoOperacion();
			//De acuerdo al Tipo de Otros
			//Horas Extras
			if (ddlTipo.SelectedValue == "2")
			{
				//Insertamos Horas Extras
				resultado = SAT_CL.Nomina.EsquemaRegistro.ActualizaHorasExtras(0, Convert.ToInt32(gvNominaEmpleado.SelectedDataKey["Id"]), txtVersion.Text, Convert.ToInt32(txtDias.Text), Convert.ToByte(ddlSubTipo.SelectedValue), Convert.ToInt32(txtCantidad.Text), Convert.ToDecimal(txtImportePagado.Text), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
			}
			//Incapacidad
			else if (ddlTipo.SelectedValue == "1")
			{
				//Insertamos Incapacidad
				resultado = SAT_CL.Nomina.EsquemaRegistro.ActualizaIncapacidades(0, Convert.ToInt32(gvNominaEmpleado.SelectedDataKey["Id"]), txtVersion.Text, Convert.ToInt32(txtDias.Text), Convert.ToByte(ddlSubTipo.SelectedValue), Convert.ToDecimal(txtImportePagado.Text), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
			}
			//Devolvemos Resultado
			return resultado;
		}
		/// <summary>
		/// Método encargado de Editar las Horas Extras  e Incapacidades
		/// </summary>
		/// <returns></returns>
		private RetornoOperacion EditaHrExtraIncapacidad()
		{
			//Declaramos Objeto Resultado
			RetornoOperacion resultado = new RetornoOperacion();
			//De acuerdo al Tipo de Otros
			//Horas Extras
			if (ddlTipo.SelectedValue == "2")
			{
				//Insertamos Horas Extras
				resultado = SAT_CL.Nomina.EsquemaRegistro.ActualizaHorasExtras(Convert.ToInt32(gvNominaOtros.SelectedValue), Convert.ToInt32(gvNominaEmpleado.SelectedDataKey["Id"]), txtVersion.Text, Convert.ToInt32(txtDias.Text), Convert.ToByte(ddlSubTipo.SelectedValue), Convert.ToInt32(txtCantidad.Text), Convert.ToDecimal(txtImportePagado.Text), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
			}
			//Incapacidad
			else if (ddlTipo.SelectedValue == "1")
			{
				//Insertamos Incapacidad
				resultado = SAT_CL.Nomina.EsquemaRegistro.ActualizaIncapacidades(Convert.ToInt32(gvNominaOtros.SelectedValue), Convert.ToInt32(gvNominaEmpleado.SelectedDataKey["Id"]), txtVersion.Text, Convert.ToInt32(txtDias.Text), Convert.ToByte(ddlSubTipo.SelectedValue), Convert.ToDecimal(txtImportePagado.Text), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
			}
			//Devolvemos Resultado
			return resultado;
		}
		#endregion

		#region Separacion e Indemnizacion
		/// <summary>
		/// Al dar click en el enlace de la columna "Separacion Indemnizacion"
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void lnkSeparacionIndemnizacion_Click(object sender, EventArgs e)
		{
			//Validando que existan Registros
			if (gvNominaEmpleado.DataKeys.Count > 0)
			{
				//Seleccionando Fila
				Controles.SeleccionaFila(gvNominaEmpleado, sender, "lnk", false);
				//Obteniendo Control
				LinkButton lnk = (LinkButton)sender;
				//Instanciando Detalle de Nomina
				using (NomEmpleado NomEmp = new NomEmpleado(Convert.ToInt32(gvNominaEmpleado.SelectedDataKey["Id"])))
				{
					//Validando que exista la nomina del Empleado
					if (NomEmp.habilitar)
					{
						//Mostrando Ventana
						GestionaVentanas(this, "SeparacionIndemnizacion");
						//Validando Comando
						switch (lnk.CommandName)
						{
							case "SeparacionIndemnizacion":
							{
								//Cargar informacion del GV
								CargaDetallesSeparacionIndemnizacion(NomEmp.id_nomina_empleado);
								//Inicializar controles
								#region ddlConcepto
								InicializaValoresSeparacionIndemnizacion(0, SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion.Percepcion, 25);
								#endregion
								#region txtAnosServicio
								using (Operador operador = new Operador(NomEmp.id_empleado))
								{
									//Si el operador ya fue dado de baja
									if (operador.fecha_baja != DateTime.MinValue)
									{
										txtSIAnosServicio.Text = Fecha.ObtieneAnosDiferenciaRedondeado(operador.fecha_ingreso, operador.fecha_baja).ToString();
										txtSIAnosServicio.ToolTip = $"Fecha de ingreso: {operador.fecha_ingreso.ToString("dddd, dd MMMM yyyy")} \nFecha de baja: {operador.fecha_baja.ToString("dddd, dd MMMM yyyy")}";
									}
									//Si el operador aun no es dado de baja, calcular sobre la fecha de la nomina
									else
									{
										using (SAT_CL.Nomina.Nomina12 nomina12 = new SAT_CL.Nomina.Nomina12(NomEmp.id_nomina))
										{
											txtSIAnosServicio.Text = Fecha.ObtieneAnosDiferenciaRedondeado(operador.fecha_ingreso, nomina12.fecha_nomina).ToString();
											txtSIAnosServicio.ToolTip = $"Fecha de ingreso: {operador.fecha_ingreso.ToString("dddd, dd MMMM yyyy")} \nFecha de nomina: {nomina12.fecha_nomina.ToString("dddd, dd MMMM yyyy")}";
										}
									}
									ImprimirAcumulables(NomEmp.id_nomina_empleado, NomEmp.id_empleado);
								}
								#endregion
								#region txtUltimoSueldo
								txtSIUltimoSueldoO.Text = NomEmpleado.ObtieneSueldoNominaAnterior(NomEmp.id_nomina_empleado, NomEmp.id_empleado).ToString();
								#endregion
								#region Ingresos No/Acumulables 
								txtSITotalPagado.Text = EsquemaRegistro.ObtieneTotalSeparacionIndemnizacion(NomEmp.id_nomina_empleado).ToString();
								//Tooltips explicativos
								txtSIIngresoNoAcumulable.ToolTip = $"La diferencia entre el Total Pagado: ${txtSITotalPagado.Text}\nMenos el último sueldo ordinario: ${txtSIUltimoSueldoO.Text}";
								txtSIIngresoAcumulable.ToolTip = $"El menor entre el Total Pagado: ${txtSITotalPagado.Text}\nY el último sueldo ordinario: ${txtSIUltimoSueldoO.Text}";
								#endregion
								//ddlTamano
								SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlSITamano, "", 26);
								break;
							}
						}
					}
				}
			}
		}
		#region Eventos
		/// <summary>
		/// Al presionar el boton Cancelar en la ventana Separacion e Indemnizacion
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btnSICancelar_Click(object sender, EventArgs e)
		{
			GestionaVentanas(this, "SeparacionIndemnizacion");
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btnSIGuardar_Click(object sender, EventArgs e)
		{
			GuardarSeparacionIndemnizacion();
		}
		/// <summary>
		/// Al cambiar el elemento seleccionado paracambiar de tamaño
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void ddlSITamano_SelectedIndexChanged(object sender, EventArgs e)
		{
			//Cambio de Tamaño
			Controles.CambiaTamañoPaginaGridView(gvDetalleNominaSI, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3"), Convert.ToInt32(ddlSITamano.SelectedValue), true, 1);
		}
		/// <summary>
		/// Evento activado al enlazar la informacion de un dataTable con el gvDetalleNominaSI
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void gvDetalleNominaSI_RowDataBound(object sender, GridViewRowEventArgs e)
		{
		}
		/// <summary>
		/// Evento activado al cambiar el orden de muestra en el gridview
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void gvDetalleNominaSI_Sorting(object sender, GridViewSortEventArgs e)
		{
			//Cambiando Expresión
			lblSIOrdenado.Text = Controles.CambiaSortExpressionGridView(gvDetalleNominaSI, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3"), e.SortExpression, true, 1);
		}
		/// <summary>
		/// Evento activado al cambiar la pagina mostrada del gridview
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void gvDetalleNominaSI_PageIndexChanging(object sender, GridViewPageEventArgs e)
		{
		}
		/// <summary>
		/// Al presionar el link Editar en el gv de la ventana Separacion Indemnizacion
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void lnkEditarSI_Click(object sender, EventArgs e)
		{
			//Validando que existan Registros
			if (gvDetalleNominaSI.DataKeys.Count > 0)
			{
				//Seleccionando Fila
				Controles.SeleccionaFila(gvDetalleNominaSI, sender, "lnk", false);
				int idEsquemaRegistro = //[49]ImporteGravado(Esquema)
				EsquemaRegistro.ObtieneIdEsquemaRegistro(49, Convert.ToInt32(gvDetalleNominaSI.SelectedDataKey["IdNominaEmpleadoTER"]), Convert.ToInt32(gvDetalleNominaSI.SelectedDataKey["IdEsquemaSuperiorTER"]));
				//Instanciamos Registro Agrupador
				using (EsquemaRegistro objEsquemRegistro = new EsquemaRegistro(idEsquemaRegistro))
				{
					txtSIIngresoGravado.Text = objEsquemRegistro.valor;
					if (objEsquemRegistro.id_esquema == 49)
					{
						//Obtenemos Valores
						int id_concepto = 0;
						decimal importe_gravado = 00M;
						decimal importe_exento = 00M;
						//Obtenemos Valores de las Percepciones
						EsquemaRegistro.ObtieneDetalleNominaPercepcion(Convert.ToInt32(gvNominaEmpleado.SelectedDataKey["Id"]), Convert.ToInt32(gvDetalleNominaSI.SelectedDataKey["IdEsquemaSuperiorTER"]), out id_concepto, out importe_gravado, out importe_exento);
						//Asignando Valores
						ddlSIConcepto.SelectedValue = id_concepto.ToString();
					}
				}
			}
		}
		/// <summary>
		/// Al presionar el link Eliminar en el gv de la ventana SeparacionIndemnizacion
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void lnkEliminarSI_Click(object sender, EventArgs e)
		{
			//Validar que existen Registros
			if (gvDetalleNominaSI.DataKeys.Count > 0)
			{
				//Crear objeto retorno
				RetornoOperacion resultado = new RetornoOperacion();
				//Seleccionar fila
				Controles.SeleccionaFila(gvDetalleNominaSI, sender, "lnk", false);
				//Almacena el id que agrupará los atributos de un nodo
				int idEsquemaSuperior = 0;
				//Obtener y guardar los idNominaEmpleado, idUsuario para agilizar la lectura y ejecución del código
				//Obtener valores que se repiten durante la ejecucion
				int idNomEmpleado = Convert.ToInt32(gvNominaEmpleado.SelectedDataKey["Id"]);
				int idValor = Convert.ToInt32(ddlSIConcepto.SelectedValue);
				int idUsuario = ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario;
				int idCompania = ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor;
				int idEmpleado; using (NomEmpleado nE = new NomEmpleado(idNomEmpleado)) idEmpleado = nE.id_empleado;
				decimal totalSeparacionIndemnizacion = EsquemaRegistro.ObtieneTotalSeparacionIndemnizacion(idNomEmpleado);
				decimal ultimoSueldo = NomEmpleado.ObtieneSueldoNominaAnterior(idNomEmpleado, idEmpleado);
				bool existenciasSI;
				//Instanciar nomina empleado para revisar el estatus en el que se encuentra
				using (NomEmpleado nominaE = new NomEmpleado(idNomEmpleado))
				{
					//Si aun no tiene ningun comprobante, continuar insercion o edicion de atributos/esquemas. Si tiene comprobante, significa que se encuentra timbrada o cancelada
					if (nominaE.id_comprobante == 0 && nominaE.id_comprobante33 == 0)
					{
						//Validar que se seleccióno un registro mediante un LNK para ELIMINAR
						if (gvDetalleNominaSI.SelectedIndex != -1)
						{
							//Buscar el id que tenga el esquema [47]Clave, para tener acceso al agrupador que comparte con el resto de esquemas de interés
							using (EsquemaRegistro eClave = new EsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistrado(idNomEmpleado, 47, gvDetalleNominaSI.SelectedDataKey["Clave"].ToString())))
							{
								//Si se logra instanciar el registro con esquema [47]
								if (eClave.habilitar)
								{
									idEsquemaSuperior = eClave.id_esquema_superior;
									//Eliminar los esquemas [46]TipoPercepcion [47]PrimaAntiguedad [48]Concepto [49]ImporteGravado mediante transaccion. Esquema [50]ImporteExento no interesa eliminarse
									using (System.Transactions.TransactionScope trans = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
									{
										//Eliminar el esquema [40]Percepcion
										using (EsquemaRegistro eP = new EsquemaRegistro(idEsquemaSuperior))
										{
											if (eP.habilitar) resultado = eP.DeshabilitaEsquemaRegistro(idUsuario);
											else resultado = new RetornoOperacion("Imposible eliminar el nodo Percepcion");
										}
										if (resultado.OperacionExitosa)
										{
											//Eliminar el esquema [46]TipoPercepcion
											using (EsquemaRegistro eTP = new EsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistrado(idNomEmpleado, 46, "", idEsquemaSuperior)))
											{
												//Validar que se puede instanciar para eliminar
												if (eTP.habilitar) resultado = eTP.DeshabilitaEsquemaRegistro(idUsuario);
												else resultado = new RetornoOperacion("Imposible eliminar el atributo Tipo Percepción.");
											}
											//Si el esquema 46 se eliminó correctamente
											if (resultado.OperacionExitosa)
											{
												//Eliminar el esquema [47]Clave
												using (EsquemaRegistro eClave2 = new EsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistrado(idNomEmpleado, 47, "", idEsquemaSuperior)))
												{
													//Validar que se puede instanciar para eliminar
													if (eClave2.habilitar) resultado = eClave2.DeshabilitaEsquemaRegistro(idUsuario);
													else resultado = new RetornoOperacion("Imposible eliminar el atributo Clave.");
												}
												//Si el esquema 47 se eliminó correctamente
												if (resultado.OperacionExitosa)
												{
													//Eliminar el esquema [48]Concepto
													using (EsquemaRegistro eCpt = new EsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistrado(idNomEmpleado, 48, "", idEsquemaSuperior)))
													{
														//Validar que se puede instanciar para eliminar
														if (eCpt.habilitar) resultado = eCpt.DeshabilitaEsquemaRegistro(idUsuario);
														else resultado = new RetornoOperacion("Imposible eliminar el atributo Concepto.");
													}
													//Si el esquema 48 se eliminó correctamente
													if (resultado.OperacionExitosa)
													{
														//Eliminar el esquema [49]ImporteGravado
														using (EsquemaRegistro eImporteG = new EsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistrado(idNomEmpleado, 49, "", idEsquemaSuperior)))
														{
															//Validar que se puede instanciar para eliminar
															if (eImporteG.habilitar) resultado = eImporteG.DeshabilitaEsquemaRegistro(idUsuario);
															else resultado = new RetornoOperacion("Imposible eliminar el atributo Importe Gravado.");
														}
														//Si el esquema [49] se eliminó correctamente
														if (resultado.OperacionExitosa)
														{
															//Eliminar esquema [50]ImporteExento
															using (EsquemaRegistro eImporteE = new EsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistrado(idNomEmpleado, 50, "", idEsquemaSuperior)))
															{
																if (eImporteE.habilitar) resultado = eImporteE.DeshabilitaEsquemaRegistro(idUsuario);
																else resultado = new RetornoOperacion("Imposible eliminar atributo Importe Exento");
															}
															/*Hasta aqui temina la edicion de los atributos para el nodo Percepcion*/
															//Si el esquema 50 se eliminó correctamente
															if (resultado.OperacionExitosa)
															{
																/**/
																/*Aqui comienza la edicion de los atributos para el nodo SeparacionIndemnizacion*/
																/**/
																//Actualizar el total pagado
																totalSeparacionIndemnizacion = EsquemaRegistro.ObtieneTotalSeparacionIndemnizacion(idNomEmpleado);
																//Buscar si aun quedan otros nodos Percepcion referente a SI. Luego de deshabilitar
																existenciasSI = EsquemaRegistro.ValidaExistenciasSeparacionIndmnizacion(idNomEmpleado, "0", 4);

																//Buscar el id que tenga el esquema [61]SeparacionIndemnizacion, para tener acceso al agrupador que comparte con el resto de esquemas de interés
																using (EsquemaRegistro eSI = new EsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistrado(idNomEmpleado, 61)))
																{
																	//Actualizar el agrupador
																	idEsquemaSuperior = eSI.id_esquema_registro;


																	//Si se encuentra, y aun hay informacion de otras Percepciones SI. Entonces actualizar todos los atributos
																	if (eSI.habilitar && existenciasSI)
																	{
																		/*No se requiere actualizar el valor del esquema [61]SeparacionIndemnizacion*/
																		//Instanciar el esquema [62]TotalPagado
																		using (EsquemaRegistro eTotalP = new EsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistrado(idNomEmpleado, 62, "", idEsquemaSuperior)))
																		{
																			//Validar que se puede instanciar para actualizar
																			if (eTotalP.habilitar) resultado = eTotalP.ActualizaValor(totalSeparacionIndemnizacion.ToString(), idUsuario);
																			else resultado = new RetornoOperacion("Imposible actualizar el atributo Total Pagado");
																		}
																		//Si el esquema 62 se actualizó correctamente
																		if (resultado.OperacionExitosa)
																		{
																			/*El esquema [63]NumAñosServicio y [64]UltimoSueldoMesOrd no interesan eliminar*/
																			//Instanciar el esquema [65]IngresoAcumulable
																			using (EsquemaRegistro eIngresoA = new EsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistrado(idNomEmpleado, 65, "", idEsquemaSuperior)))
																			{
																				//Validar que se puede instanciar para eliminar
																				if (eIngresoA.habilitar) resultado = eIngresoA.ActualizaValor((totalSeparacionIndemnizacion < ultimoSueldo ? totalSeparacionIndemnizacion : ultimoSueldo).ToString(), idUsuario);
																				else resultado = new RetornoOperacion("Imposible actualizar el atributo Ingreso Acumulable");
																			}
																			//Si el esquema 65 se eliminó correctamente
																			if (resultado.OperacionExitosa)
																			{
																				//Instanciar el esquema [66]IngresoNoAcumulable
																				using (EsquemaRegistro eIngresoNA = new EsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistrado(idNomEmpleado, 66, "", idEsquemaSuperior)))
																				{
																					//Validar que se puede instanciar para eliminar
																					if (eIngresoNA.habilitar) resultado = eIngresoNA.ActualizaValor((totalSeparacionIndemnizacion < ultimoSueldo ? totalSeparacionIndemnizacion : ultimoSueldo).ToString(), idUsuario);
																					else resultado = new RetornoOperacion("Imposible actualizar el atributo Ingreso No Acumulable");
																				}
																				//Si el esquema 66 se eliminó correctamente
																				if (resultado.OperacionExitosa)
																				{
																					//Instanciar el esquema [68]TotalSeparacionIndemnizacion
																					using (EsquemaRegistro eTotalSI = new EsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistrado(idNomEmpleado, 68)))
																					{
																						//Validar que se puede instanciar para eliminar
																						if (eTotalSI.habilitar) resultado = eTotalSI.ActualizaValor(totalSeparacionIndemnizacion.ToString(), idUsuario);
																						else resultado = new RetornoOperacion("Imposible actualizar el atributo Total Separacion Indemnizacion");
																					}
																					//Si el esquema [68] se actualiza correctametne, al ser el ultimo esquema por actualizar. Terminar la transaccion
																					if (resultado.OperacionExitosa)
																					{
																						using (SAT_CL.Nomina.Nomina12 nom = new SAT_CL.Nomina.Nomina12(Convert.ToInt32(Session["id_registro"])))
																						{
																							resultado = EsquemaRegistro.ActualizaTotalesNomina(nom.version, idNomEmpleado, idUsuario);
																							if (resultado.OperacionExitosa)
																							{
																								resultado = EsquemaRegistro.ActualizaTotalesPercepciones(nom.version, idNomEmpleado, idUsuario);
																							}
																						}
																					}
																				}
																			}
																		}
																	}


																	//Si se encuentra, y  no hay informacion de otras Percepciones SI. Entonces eliminar todos los atributos
																	else if (eSI.habilitar && !existenciasSI)
																	{
																		//Deshabilitar el esquema [61]SeparacionIndemnizacion
																		resultado = eSI.DeshabilitaEsquemaRegistro(idUsuario);
																		if (resultado.OperacionExitosa)
																		{
																			//Instanciar el esquema [62]TotalPagado
																			using (EsquemaRegistro eTotalP = new EsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistrado(idNomEmpleado, 62, "", idEsquemaSuperior)))
																			{
																				//Validar que se puede instanciar para actualizar
																				if (eTotalP.habilitar) resultado = eTotalP.DeshabilitaEsquemaRegistro(idUsuario);
																				else resultado = new RetornoOperacion("Imposible eliminar el atributo Total Pagado");
																			}
																			//Si el esquema 62 se actualizó correctamente
																			if (resultado.OperacionExitosa)
																			{
																				/*El esquema [63]NumAñosServicio y [64]UltimoSueldoMesOrd no interesan eliminar*/
																				using (EsquemaRegistro eNumAS = new EsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistrado(idNomEmpleado, 63, "", idEsquemaSuperior)))
																				{
																					if (eNumAS.habilitar) resultado = eNumAS.DeshabilitaEsquemaRegistro(idUsuario);
																					else resultado = new RetornoOperacion("Imposible eliminar el atributo NumAnosServicio");
																				}
																				if (resultado.OperacionExitosa)
																				{
																					using (EsquemaRegistro eUltSueldo = new EsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistrado(idNomEmpleado, 64, "", idEsquemaSuperior)))
																					{
																						if (eUltSueldo.habilitar) resultado = eUltSueldo.DeshabilitaEsquemaRegistro(idUsuario);
																						else resultado = new RetornoOperacion("Imposible eliminar el atributo UltimoSueldoMesOrdinario");
																					}
																					if (resultado.OperacionExitosa)
																					{
																						//Instanciar el esquema [65]IngresoAcumulable
																						using (EsquemaRegistro eIngresoA = new EsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistrado(idNomEmpleado, 65, "", idEsquemaSuperior)))
																						{
																							//Validar que se puede instanciar para eliminar
																							if (eIngresoA.habilitar) resultado = eIngresoA.DeshabilitaEsquemaRegistro(idUsuario);
																							else resultado = new RetornoOperacion("Imposible eliminar el atributo IngresoAcumulable");
																						}
																						//Si el esquema 65 se eliminó correctamente
																						if (resultado.OperacionExitosa)
																						{
																							//Instanciar el esquema [66]IngresoNoAcumulable
																							using (EsquemaRegistro eIngresoNA = new EsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistrado(idNomEmpleado, 66, "", idEsquemaSuperior)))
																							{
																								//Validar que se puede instanciar para eliminar
																								if (eIngresoNA.habilitar) resultado = eIngresoNA.DeshabilitaEsquemaRegistro(idUsuario);
																								else resultado = new RetornoOperacion("Imposible eliminar el atributo IngresoNoAcumulable");
																							}
																							//Si el esquema 66 se eliminó correctamente
																							if (resultado.OperacionExitosa)
																							{
																								//Instanciar el esquema [68]TotalSeparacionIndemnizacion
																								using (EsquemaRegistro eTotalSI = new EsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistrado(idNomEmpleado, 68)))
																								{
																									//Validar que se puede instanciar para eliminar
																									if (eTotalSI.habilitar) resultado = eTotalSI.DeshabilitaEsquemaRegistro(idUsuario);
																									else resultado = new RetornoOperacion("Imposible eliminar el atributo TotalSeparacionIndemnizacion");
																								}
																								//Si el esquema [68] se actualiza correctametne, al ser el ultimo esquema por actualizar. Terminar la transaccion
																								if (resultado.OperacionExitosa)
																								{
																									using (SAT_CL.Nomina.Nomina12 nom = new SAT_CL.Nomina.Nomina12(Convert.ToInt32(Session["id_registro"])))
																									{
																										resultado = EsquemaRegistro.ActualizaTotalesNomina(nom.version, idNomEmpleado, idUsuario);
																										if (resultado.OperacionExitosa)
																										{
																											resultado = EsquemaRegistro.ActualizaTotalesPercepciones(nom.version, idNomEmpleado, idUsuario);

																											/*TO DO: Si ya no hay más percepciones de tipo SI, eliminar el nodo SeparacionIndemnizacion por completo*/
																											if (!EsquemaRegistro.ValidaExistenciasSeparacionIndmnizacion(idNomEmpleado, "0", 4))
																											{

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
																		resultado = new RetornoOperacion("Imposible eliminar el Nodo SeparacionIndemnizacion.");
																	}
																}
															}
														}
													}
												}
											}
											if (resultado.OperacionExitosa) trans.Complete();
										}
									}
								}
								//Si no se instancia el esquema [47], aun no hay un registro para SeparacionIncemnizacion
								else resultado = new RetornoOperacion("Imposible obtener el Registro.");
							}
						}
					}
					else resultado = new RetornoOperacion("El estatus de la Nomina del empleado, no permite su edición");
				}
				//Si se logran insertar todos los esquemas
				if (resultado.OperacionExitosa)
				{
					//Actualizar gridview
					CargaDetallesSeparacionIndemnizacion(idNomEmpleado);
					CargaDetallesNominaEmpleado();
					txtSIIngresoGravado.Text = "0";
					txtSITotalPagado.Text = EsquemaRegistro.ObtieneTotalSeparacionIndemnizacion(idNomEmpleado).ToString();
					using (NomEmpleado nE = new NomEmpleado(idNomEmpleado)) ImprimirAcumulables(idNomEmpleado, nE.id_empleado);
				}
				//Avisar al usuario cualquier resultado obtenido, Bueno o Malo
				ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
			}
		}
		/// <summary>
		/// Evento activado al cambiar el texto del Ingreso Gravado, y quitar el foco en la caja de texto
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void txtSIIngresoGravado_TextChanged(object sender, EventArgs e)
		{
		}
		#endregion
		#region Metodos
		/// <summary>
		/// Método encargado de llenar el gridview de la ventana modal de SeparacionIndemnizacion
		/// </summary>
		/// <param name="idNomEmpleado"></param>
		private void CargaDetallesSeparacionIndemnizacion(int idNomEmpleado)
		{
			//Consultar Separacion Indemnizacion
			using (DataTable dtSeparacionIndemnizacion = EsquemaRegistro.ObtieneDetallesSeparacionIndemnizacion(idNomEmpleado, "0", 4))
			{
				//Validando que existan Registros
				if (Validacion.ValidaOrigenDatos(dtSeparacionIndemnizacion))
				{
					//Cargando GridView
					Controles.CargaGridView(gvDetalleNominaSI, dtSeparacionIndemnizacion, "IdNominaEmpleadoTER-IdEsquemaSuperiorTER-Clave", lblSIOrdenado.Text, true, 1);
					//Añadiendo Resultado a Sesión
					Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtSeparacionIndemnizacion, "Table3");
				}
				else
				{
					//Cargando GridView
					Controles.InicializaGridview(gvDetalleNominaSI);
					//Añadiendo Resultado a Sesión
					Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table3");
				}
			}
		}
		/// <summary>
		/// Método encargado de guardar la informacion de los esquemasRegistro 46-50 para esa nomina de empleado
		/// </summary>
		private void GuardarSeparacionIndemnizacion()
		{
			//Declarar objeto retorno
			RetornoOperacion resultado = new RetornoOperacion();
			//Almacena el id que agrupará los atributos de un nodo
			int idEsquemaSuperior = 0;
			//Obtener valores que se repiten durante la ejecucion
			int idNomEmpleado = Convert.ToInt32(gvNominaEmpleado.SelectedDataKey["Id"]);
			int idValor = Convert.ToInt32(ddlSIConcepto.SelectedValue);
			int idUsuario = ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario;
			int idCompania = ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor;
			int idEmpleado; using (NomEmpleado nE = new NomEmpleado(idNomEmpleado)) idEmpleado = nE.id_empleado;
			decimal totalSeparacionIndemnizacion = EsquemaRegistro.ObtieneTotalSeparacionIndemnizacion(idNomEmpleado);
			decimal ultimoSueldo = NomEmpleado.ObtieneSueldoNominaAnterior(idNomEmpleado, idEmpleado);
			//Instanciar nomina empleado para revisar el estatus en el que se encuentra
			using (NomEmpleado nominaE = new NomEmpleado(idNomEmpleado))
			{
				//Si aun no tiene ningun comprobante, continuar insercion o edicion de atributos/esquemas. Si tiene comprobante, significa que se encuentra timbrada o cancelada
				if (nominaE.id_comprobante == 0 && nominaE.id_comprobante33 == 0)
				{
					//Validar que haya una cantidad en la caja de texto
					if (txtSIIngresoGravado.Text != "")
					{
						//Validar que se seleccióno un registro mediante un LNK para EDITAR
						if (gvDetalleNominaSI.SelectedIndex != -1)
						{
							//Buscar el id que tenga el esquema [47]Clave, para tener acceso al agrupador que comparte con el resto de esquemas de interés
							using (EsquemaRegistro eClave = new EsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistrado(idNomEmpleado, 47, gvDetalleNominaSI.SelectedDataKey["Clave"].ToString())))
							{
								//Si se logra instanciar el registro con esquema [47]
								if (eClave.habilitar)
								{
									idEsquemaSuperior = eClave.id_esquema_superior;
									//Editar los esquemas [46]TipoPercepcion [47]PrimaAntiguedad [48]Concepto [49]ImporteGravado mediante transaccion. Esquema [50]ImporteExento no interesa editarse
									using (System.Transactions.TransactionScope trans = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
									{
										#region Insercion
										//Instanciar el TipoCobroRecurrente seleccionado actualmente en el ddlSIConcepto
										using (SAT_CL.Liquidacion.TipoCobroRecurrente tipoCR = new SAT_CL.Liquidacion.TipoCobroRecurrente(idValor))
										{
											//Si se logra instanciar el tipoCobroRecurrente
											if (tipoCR.habilitar)
											{
												//Editar el esquema [46]TipoPercepcion
												using (EsquemaRegistro eTP = new EsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistrado(idNomEmpleado, 46, "", idEsquemaSuperior)))
												{
													//Validar que se puede instanciar para editar
													if (eTP.habilitar) resultado = eTP.ActualizaValor(tipoCR.clave_nomina, idUsuario);
													else resultado = new RetornoOperacion("Imposible editar el atributo Tipo Percepción.");
												}
												//Si el esquema 46 se editó correctamente
												if (resultado.OperacionExitosa)
												{
													//Editar el esquema [47]Clave
													using (EsquemaRegistro eClave2 = new EsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistrado(idNomEmpleado, 47, "", idEsquemaSuperior)))
													{
														//Validar que se puede instanciar para editar
														if (eClave2.habilitar) resultado = eClave2.ActualizaValor(tipoCR.clave_nomina, idUsuario);
														else resultado = new RetornoOperacion("Imposible editar el atributo Clave.");
													}
													//Si el esquema 47 se editó correctamente
													if (resultado.OperacionExitosa)
													{
														//Editar el esquema [48]Concepto
														using (EsquemaRegistro eCpt = new EsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistrado(idNomEmpleado, 48, "", idEsquemaSuperior)))
														{
															//Validar que se puede instanciar para editar
															if (eCpt.habilitar) resultado = eCpt.ActualizaValor(tipoCR.descripcion, idUsuario);
															else resultado = new RetornoOperacion("Imposible editar el atributo Concepto.");
														}
														//Si el esquema 48 se editó correctamente
														if (resultado.OperacionExitosa)
														{
															//Editar el esquema [49]ImporteGravado
															using (EsquemaRegistro eImporteG = new EsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistrado(idNomEmpleado, 49, "", idEsquemaSuperior)))
															{
																//Validar que se puede instanciar para editar
																if (eImporteG.habilitar) resultado = eImporteG.ActualizaValor(txtSIIngresoGravado.Text, idUsuario);
																else resultado = new RetornoOperacion("Imposible editar el atributo Importe Gravado.");
															}
															/*Hasta aqui temina la edicion de los atributos para el nodo Percepcion*/
															//Buscar el id que tenga el esquema [61]SeparacionIndemnizacion, para tener acceso al agrupador que comparte con el resto de esquemas de interés
															using (EsquemaRegistro eSI = new EsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistrado(idNomEmpleado, 61)))
															{
																//Si se encuentra
																if (eSI.habilitar)
																{
																	//Actualizar el agrupador
																	idEsquemaSuperior = eSI.id_esquema_registro;
																	//Si el esquema 49 se editó correctamente
																	if (resultado.OperacionExitosa)
																	{
																		/*Aqui comienza la edicion de los atributos para el nodo SeparacionIndemnizacion*/
																		//Ya que la Percepcion fue actualizada. Actualizar el total pagado
																		totalSeparacionIndemnizacion = EsquemaRegistro.ObtieneTotalSeparacionIndemnizacion(idNomEmpleado);
																		//Instanciar el esquema [62]TotalPagado
																		using (EsquemaRegistro eTotalP = new EsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistrado(idNomEmpleado, 62, "", idEsquemaSuperior)))
																		{
																			//Validar que se puede instanciar para editar
																			if (eTotalP.habilitar) resultado = eTotalP.ActualizaValor(totalSeparacionIndemnizacion.ToString(), idUsuario);
																			else resultado = new RetornoOperacion("Imposible editar el atributo Total Pagado");
																		}
																		//Si el esquema 62 se edito correctamente
																		if (resultado.OperacionExitosa)
																		{
																			/*El esquema [63]NumAñosServicio y [64]UltimoSueldoMesOrd no interesan editar*/
																			//Instanciar el esquema [65]IngresoAcumulable
																			using (EsquemaRegistro eIngresoA = new EsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistrado(idNomEmpleado, 65, "", idEsquemaSuperior)))
																			{
																				//Validar que se puede instanciar para editar
																				if (eIngresoA.habilitar) resultado = eIngresoA.ActualizaValor((totalSeparacionIndemnizacion < ultimoSueldo ? totalSeparacionIndemnizacion : ultimoSueldo).ToString(), idUsuario);
																				else resultado = new RetornoOperacion("Imposible editar el atributo Ingreso Acumulable");
																			}
																			//Si el esquema 65 se editó correctamente
																			if (resultado.OperacionExitosa)
																			{
																				//Instanciar el esquema [66]IngresoNoAcumulable
																				using (EsquemaRegistro eIngresoNA = new EsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistrado(idNomEmpleado, 66, "", idEsquemaSuperior)))
																				{
																					//Validar que se puede instanciar para editar
																					if (eIngresoNA.habilitar) resultado = eIngresoNA.ActualizaValor((totalSeparacionIndemnizacion - ultimoSueldo).ToString(), idUsuario);
																					else resultado = new RetornoOperacion("Imposible editar el atributo Total Pagao");
																				}
																				//Si el esquema 66 se editó correctamente
																				if (resultado.OperacionExitosa)
																				{
																					//Instanciar el esquema [68]TotalSeparacionIndemnizacion
																					using (EsquemaRegistro eTotalSI = new EsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistrado(idNomEmpleado, 68)))
																					{
																						//Validar que se puede instanciar para editar
																						if (eTotalSI.habilitar) resultado = eTotalSI.ActualizaValor(totalSeparacionIndemnizacion.ToString(), idUsuario);
																						else resultado = new RetornoOperacion("Imposible editar el atributo Total Pagao");
																					}
																					//Si el esquema [68] se actualiza correctametne, al ser el ultimo esquema por actualizar. Terminar la transaccion
																					if (resultado.OperacionExitosa)
																					{
																						using (SAT_CL.Nomina.Nomina12 nom = new SAT_CL.Nomina.Nomina12(Convert.ToInt32(Session["id_registro"])))
																						{
																							resultado = EsquemaRegistro.ActualizaTotalesNomina(nom.version, idNomEmpleado, idUsuario);
																							if (resultado.OperacionExitosa)
																							{
																								resultado = EsquemaRegistro.ActualizaTotalesPercepciones(nom.version, idNomEmpleado, idUsuario);
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
																	resultado = new RetornoOperacion("Imposible acceder al nodo SeparacionIndemnizacion");
																}
															}
														}
													}
												}
											}
											//Si no se instancia el tipoCobroRecurrente Interrumpir el resto de operaciones
											else resultado = new RetornoOperacion("Imposible obtener el Cobro Recurrente");
										}
										#endregion
										if (resultado.OperacionExitosa) trans.Complete();
									}
								}
								//Si no se instancia el esquema [47], aun no hay un registro para SeparacionIncemnizacion
								else resultado = new RetornoOperacion("Imposible obtener el Registro.");
							}
						}
						//Si no hay ninguno seleccionado
						else
						{
							//Insertar un nuevo registro de dichos esquemas, validado por una transaccion
							using (System.Transactions.TransactionScope transaccionGlobal = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
							{
								resultado = EsquemaRegistro.InsertaSeparacionIndemnizacion(nominaE.id_nomina, idNomEmpleado, idValor, Convert.ToDecimal(txtSIIngresoGravado.Text), idEmpleado, idUsuario);
								#region Insercion Vieja
								////Insertar nodo para el esquema [41]Percepcion, que será nuestro agrupador
								//resultado = EsquemaRegistro.InsertaEsquemaRegistro(41, 0, idNomEmpleado, "", 0, 0, 0, idUsuario);
								////Si se insertó el esquema [41]
								//if (resultado.OperacionExitosa)
								//{
								//	//Almacenar el id obtenido para usarlo como agrupador
								//	idEsquemaSuperior = resultado.IdRegistro;
								//	//Englobar las inserciones del Nodo Percepcion
								//	resultado = InsertaNodoPercepcion(idEsquemaSuperior, idNomEmpleado, idValor, idUsuario);
								//	//Si la informacion para el Nodo Percepcion se inserta, con el esquema [50]ImporteExento como ultimo
								//	if (resultado.OperacionExitosa)
								//	{
								//		totalSeparacionIndemnizacion = EsquemaRegistro.ObtieneTotalSeparacionIndemnizacion(idNomEmpleado);
								//		/*Comienza la insercion de la informacion para el Nodo SeparacionIndemnizacion*/
								//		resultado = InsertaNodoSeparacion(idNomEmpleado, idEsquemaSuperior, idUsuario, totalSeparacionIndemnizacion, ultimoSueldo, resultado.IdRegistro);
								//		//
								//		if (resultado.OperacionExitosa)
								//		{
								//			using (SAT_CL.Nomina.Nomina12 nom = new SAT_CL.Nomina.Nomina12(Convert.ToInt32(Session["id_registro"])))
								//			{
								//				resultado = EsquemaRegistro.ActualizaTotalesNomina(nom.version, idNomEmpleado, idUsuario);
								//				if (resultado.OperacionExitosa)
								//				{
								//					resultado = EsquemaRegistro.ActualizaTotalesPercepciones(nom.version, idNomEmpleado, idUsuario);
								//				}
								//			}
								//		}
								//	}
								//}
								#endregion
								//Si ambos nodos se insertan correctamente, terminar transacciones
								if (resultado.OperacionExitosa)
								{
									transaccionGlobal.Complete();
								}
							}
						}
					}
					else
						resultado = new RetornoOperacion("Indique el Importe Gravado.");
				}
				else resultado = new RetornoOperacion("El estatus de la Nomina del empleado, no permite su edición");
			}
			//Si se logran insertar todos los esquemas
			if (resultado.OperacionExitosa)
			{
				//Actualizar gridview
				CargaDetallesSeparacionIndemnizacion(idNomEmpleado);
				CargaDetallesNominaEmpleado();
				txtSIIngresoGravado.Text = "0";
				txtSITotalPagado.Text = EsquemaRegistro.ObtieneTotalSeparacionIndemnizacion(idNomEmpleado).ToString();
				using (NomEmpleado nE = new NomEmpleado(idNomEmpleado)) ImprimirAcumulables(idNomEmpleado, nE.id_empleado);
			}
			//Avisar al usuario cualquier resultado obtenido, Bueno o Malo
			ScriptServer.MuestraNotificacion(btnSIGuardar, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
		}
		///// <summary>
		///// Insercion de atributos para el Nodo SeparacionIndemnizacion
		///// </summary>
		///// <param name="idNomEmpleado"></param>
		///// <param name="idEsquemaSuperior"></param>
		///// <param name="idUsuario"></param>
		///// <param name="totalSeparacionIndemnizacion"></param>
		///// <param name="ultimoSueldo"></param>
		///// <returns></returns>
		//private RetornoOperacion InsertaNodoSeparacion(int idNomEmpleado, int idEsquemaSuperior, int idUsuario, decimal totalSeparacionIndemnizacion, decimal ultimoSueldo, int idRegistro)
		//{
		//	RetornoOperacion resultado = new RetornoOperacion(idRegistro, "", true);
		//	/*Pueden existir hasta 3 nodos de Percepcion, relacionadas uno a cada clave de Separacion Indemnizacion [022], [023], [025]*/
		//	/*Por otro lado, el nodo SeparacionIndemnizacion debe englobar la informacion que puedan traer los nodos Percepcion. Es decir, no debe repetirse*/
		//	//Revisar si ya existe el registro para el esquema [61]SeparacionIndemnizacion. Si existe, edita; si no, inserta
		//	using (EsquemaRegistro esq61 = new EsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistrado(idNomEmpleado, 61)))
		//	{
		//		if (esq61.habilitar) { /*No se requiere actualizar el valor del esquema 61, solo tomar el agrupador de éste*/ idEsquemaSuperior = esq61.id_esquema_registro; }
		//		//Insertar el registro para el esquema [61]SeparacionIndemnizacion
		//		else { resultado = EsquemaRegistro.InsertaEsquemaRegistro(61, 0, idNomEmpleado, "", 0, 0, 0, idUsuario); idEsquemaSuperior = resultado.IdRegistro; }
		//	}
		//	//Si se insertó el esquema [61]
		//	if (resultado.OperacionExitosa)
		//	{
		//		//Revisar si ya existe el registro para el esquema 62. Si existe, edita; si no, inserta
		//		using (EsquemaRegistro esq62 = new EsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistrado(idNomEmpleado, 62, "", idEsquemaSuperior)))
		//		{
		//			if (esq62.habilitar) esq62.ActualizaValor(totalSeparacionIndemnizacion.ToString(), idUsuario);
		//			//Insertar el esquema [62]TotalPagado
		//			else resultado = EsquemaRegistro.InsertaEsquemaRegistro(62, idEsquemaSuperior, idNomEmpleado, totalSeparacionIndemnizacion.ToString(), 0, 0, 0, idUsuario);
		//		}
		//		//Si se insertó el esquema [62]
		//		if (resultado.OperacionExitosa)
		//		{
		//			using (EsquemaRegistro esq63 = new EsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistrado(idNomEmpleado, 63, "", idEsquemaSuperior)))
		//			{
		//				if (esq63.habilitar) {/*No se requiere actualizar el esquema 63*/ }
		//				//Insertar el esquema [63]NumAñosServicio
		//				else resultado = EsquemaRegistro.InsertaEsquemaRegistro(63, idEsquemaSuperior, idNomEmpleado, txtSIAnosServicio.Text, 0, 0, 0, idUsuario);
		//			}
		//			//Si se insertó el esquema [63]
		//			if (resultado.OperacionExitosa)
		//			{
		//				using (EsquemaRegistro esq64 = new EsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistrado(idNomEmpleado, 64, "", idEsquemaSuperior)))
		//				{
		//					if (esq64.habilitar) { /*No se requiere editar el esquema 64*/ }
		//					//Insertar el esquema [64]UltimoSueldoMesord
		//					else resultado = EsquemaRegistro.InsertaEsquemaRegistro(64, idEsquemaSuperior, idNomEmpleado, ultimoSueldo.ToString(), 0, 0, 0, idUsuario);
		//				}
		//				//Si se insertó el esquema [64]
		//				if (resultado.OperacionExitosa)
		//				{
		//					using (EsquemaRegistro esq65 = new EsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistrado(idNomEmpleado, 65, "", idEsquemaSuperior)))
		//					{
		//						if (esq65.habilitar) resultado = esq65.ActualizaValor((totalSeparacionIndemnizacion < ultimoSueldo ? totalSeparacionIndemnizacion : ultimoSueldo).ToString(), idUsuario);
		//						//Insertar el esquema [65]IngresoAcumulable
		//						else resultado = EsquemaRegistro.InsertaEsquemaRegistro(65, idEsquemaSuperior, idNomEmpleado, (totalSeparacionIndemnizacion < ultimoSueldo ? totalSeparacionIndemnizacion : ultimoSueldo).ToString(), 0, 0, 0, idUsuario);
		//					}
		//					//Si se insertó el esquema [65]
		//					if (resultado.OperacionExitosa)
		//					{
		//						using (EsquemaRegistro esq66 = new EsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistrado(idNomEmpleado, 66, "", idEsquemaSuperior)))
		//						{
		//							if (esq66.habilitar) resultado = esq66.ActualizaValor((totalSeparacionIndemnizacion - ultimoSueldo).ToString(), idUsuario);
		//							//Insertar el esquema [66]IngresoNoAcumulable
		//							else resultado = EsquemaRegistro.InsertaEsquemaRegistro(66, idEsquemaSuperior, idNomEmpleado, (totalSeparacionIndemnizacion - ultimoSueldo).ToString(), 0, 0, 0, idUsuario);
		//						}
		//						/*Hasta aquí termina la inserción de la información del Nodo SeparacionIndemnizacion*/
		//						//Si se insertó el esquema [66]
		//						if (resultado.OperacionExitosa)
		//						{
		//							//Buscar si el esquema 68 ya fue insertado, e instanciarlo
		//							using (EsquemaRegistro esq68 = new EsquemaRegistro(EsquemaRegistro.ObtieneIdEsquemaRegistrado(idNomEmpleado, 68)))
		//							{
		//								//Si ya existe y se obtiene el registro, actualizar solo su atributo Valor.
		//								if (esq68.habilitar)
		//									resultado = esq68.ActualizaValor(totalSeparacionIndemnizacion.ToString(), idUsuario);
		//								//Si no existe, insertarlo con el valor actualizado
		//								else
		//									resultado = EsquemaRegistro.InsertaEsquemaRegistro(68, 0, idNomEmpleado, totalSeparacionIndemnizacion.ToString(), 0, 0, 0, idUsuario);
		//							}
		//						}
		//					}
		//				}
		//			}
		//		}
		//	}
		//	return resultado;
		//}
		///// <summary>
		///// Insercion de atributos parael Nodo Percepcion
		///// </summary>
		///// <param name="idEsquemaSuperior"></param>
		///// <param name="idNomEmpleado"></param>
		///// <param name="idValor"></param>
		///// <param name="idUsuario"></param>
		///// <returns></returns>
		//private RetornoOperacion InsertaNodoPercepcion(int idEsquemaSuperior, int idNomEmpleado, int idValor, int idUsuario)
		//{
		//	RetornoOperacion resultado;
		//	//Instanciar el TipoCobroRecurrente para obtener la clave del SAT. Ejs: "022", "023", "025". Dado el elemento escogido del ddlSIConcepto, que puede devolver [82]PrimaAntiguedad, [83]Indemnizacion y []PagosPorSeparacion
		//	using (SAT_CL.Liquidacion.TipoCobroRecurrente tipoCobroR = new SAT_CL.Liquidacion.TipoCobroRecurrente(idValor))
		//	{
		//		//Validando que se instanció correctamente
		//		if (tipoCobroR.habilitar)
		//		{
		//			//Tabla[78]:TipoCobroRecurrente->IdValor:[82]PrimaAntigüedad/[83]Indemnizaciones/[PorRegistrarID]PagosPorIndemnizacion
		//			//Insertar el registro para el esquema [46]TipoPercepcion
		//			resultado = EsquemaRegistro.InsertaEsquemaRegistro(46, idEsquemaSuperior, idNomEmpleado, tipoCobroR.clave_nomina, 78, 0, idValor, idUsuario);
		//			//Si se insertó el esquema [46]
		//			if (resultado.OperacionExitosa)
		//			{
		//				//Insertar el registro para el esquema [47]Clave
		//				resultado = EsquemaRegistro.InsertaEsquemaRegistro(47, idEsquemaSuperior, idNomEmpleado, tipoCobroR.clave_nomina, 78, 0, idValor, idUsuario);
		//				//Si se insertó el esquema [47]
		//				if (resultado.OperacionExitosa)
		//				{
		//					//Insertar el registro para el esquema [48]Concepto
		//					resultado = EsquemaRegistro.InsertaEsquemaRegistro(48, idEsquemaSuperior, idNomEmpleado, tipoCobroR.descripcion, 78, 0, idValor, idUsuario);
		//					//Si se insertó el esquema [48]
		//					if (resultado.OperacionExitosa)
		//					{
		//						//Insertar el registro para el esquema [49]ImporteGravado
		//						resultado = EsquemaRegistro.InsertaEsquemaRegistro(49, idEsquemaSuperior, idNomEmpleado, txtSIIngresoGravado.Text, 0, 0, 0, idUsuario);
		//						//Si se insertó el esquema [49]
		//						if (resultado.OperacionExitosa)
		//						{
		//							//Insertar el registro para el esquema [50]ImporteExento. Al tratarse de informacion sobre SeparacionIndemnizacion, SIEMPRE ENVIAR CERO EN DECIMAL
		//							resultado = EsquemaRegistro.InsertaEsquemaRegistro(50, idEsquemaSuperior, idNomEmpleado, "0.00", 0, 0, 0, idUsuario);
		//							/*Hasta aqui termina la inserción de la informacion para el Nodo Percepcion*/
		//						}
		//					}
		//				}
		//			}
		//		}
		//		//Si no se logra instanciar el TipoCobroRecurrente, impedir que siga la transaccion
		//		else
		//		{
		//			resultado = new RetornoOperacion("Imposible obtener el Cobro Recurrente");
		//		}
		//	}
		//	return resultado;
		//}

		/// <summary>
		/// Método encargado de cargar el valor de los controles izquierdos de la ventana modal de Separacion Indemnizacion
		/// </summary>
		/// <param name="idDetalleNomina"></param>
		/// <param name="tipoAplicacion"></param>
		/// <param name="idConceptoSAT"></param>
		private void InicializaValoresSeparacionIndemnizacion(int idDetalleNomina, SAT_CL.Liquidacion.TipoCobroRecurrente.TipoAplicacion tipoAplicacion, int idConceptoSAT)
		{
			//Si recibe una clave de SAT
			if (idConceptoSAT > 0)
			{
				//Consultar los catalogos Tipo de Pago
				using (DataTable dtConceptos = SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(73, "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "SeparacionIndemnizacion", (int)tipoAplicacion, ""))
				{
					//Si obtiene catalogos
					if (Validacion.ValidaOrigenDatos(dtConceptos))
						//Vaciar informacion
						Controles.CargaDropDownList(ddlSIConcepto, dtConceptos, "id", "descripcion");
					else
						Controles.InicializaDropDownList(ddlSIConcepto, "--Ninguno");
				}
			}
			else
				Controles.InicializaDropDownList(ddlSIConcepto, "--Ninguno");
		}
		/// <summary>
		/// Método llamado tras el evento de cambio de texto en txtSIIngresoGravado
		/// </summary>
		private void ImprimirAcumulables(int idNomEmpleado, int idEmpleado)
		{
			//Imprimir el menor entre el TotalPagado y el Ultimo Sueldo
			txtSIIngresoAcumulable.Text = (EsquemaRegistro.ObtieneTotalSeparacionIndemnizacion(idNomEmpleado) < NomEmpleado.ObtieneSueldoNominaAnterior(idNomEmpleado, idEmpleado) ? EsquemaRegistro.ObtieneTotalSeparacionIndemnizacion(idNomEmpleado) : NomEmpleado.ObtieneSueldoNominaAnterior(idNomEmpleado, idEmpleado)).ToString();
			//Imprimir la diferencia entre el Total Pagado y el Ultimo Sueldo
			txtSIIngresoNoAcumulable.Text = (EsquemaRegistro.ObtieneTotalSeparacionIndemnizacion(idNomEmpleado) - NomEmpleado.ObtieneSueldoNominaAnterior(idNomEmpleado, idEmpleado)).ToString();
			//Tooltips explicativos
			txtSIIngresoNoAcumulable.ToolTip = $"La diferencia entre el Total Pagado: ${txtSITotalPagado.Text}\nMenos el último sueldo ordinario: ${txtSIUltimoSueldoO.Text}";
			txtSIIngresoAcumulable.ToolTip = $"El menor entre el Total Pagado: ${txtSITotalPagado.Text}\nY el último sueldo ordinario: ${txtSIUltimoSueldoO.Text}";
		}
		#endregion
		#endregion
	}
}