<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/MasterPage/MasterPage.Master" CodeBehind="Notificacion.aspx.cs" Inherits="SAT.General.Notificacion" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<!-- Estilos documentación de servicio -->
<link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
<link href="../CSS/ControlesUsuario.css" type="text/css" rel="stylesheet" />
<link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
<!-- Estilos Autocomplete, Mascara y Validadores JQuery -->
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
<!-- Bibliotecas para Validación de formulario -->
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
<script type="text/javascript">
//Obteniendo instancia actual de la página y añadiendo manejador de evento
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
function EndRequestHandler(sender, args) {
if (args.get_error() == undefined) {
ConfiguraJQueryNotificacion();
}
}
//Función encargada de restablecer la Configuración de los Scripts de Validación
function ConfiguraJQueryNotificacion() {
$(document).ready(function () {
//Validando que se cumplan las condiciones
var validaNotificacion = function () {
var isValid1 = !$("#<%=txtContacto.ClientID%>").validationEngine('validate');
var isValid2 = !$("#<%=txtCliente.ClientID%>").validationEngine('validate');
var isValid3 = !$("#<%=txtUbicacion.ClientID%>").validationEngine('validate');
//Devolviendo Resultado
return isValid1 && isValid2 && isValid3;
}
//Validando que se cumplan las condiciones
var validaContacto = function () {
var isValid1 = !$("#<%=txtNombre.ClientID%>").validationEngine('validate');
var isValid2 = !$("#<%=txtTelefono.ClientID%>").validationEngine('validate');
var isValid3 = !$("#<%=txtEMail.ClientID%>").validationEngine('validate');
//Devolviendo Resultado
return isValid1 && isValid2 && isValid3;
}

//Añadiendo Funcion al Evento Click del Boton "Guardar"
$("#<%=btnGuardar.ClientID%>").click(validaNotificacion);
//Añadiendo Funcion al Evento Click del Menu "Guardar"
$("#<%=lkbGuardar.ClientID%>").click(validaNotificacion);
//Cargando Catalogo AutoCompleta
//Cargando Catalogo AutoCompleta Proveedor
$("#<%=txtContacto.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=56&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>' });
//Añadiendo Función de Autocompletado al Control de Cliente
$("#<%=txtCliente.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=55&param=<%= ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>' });
//Sugerencias de Ubicación
$("#<%= txtUbicacion.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=6&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>' });
//Añadiendo Funcion al Evento Click del Boton "GuardarContacto" 
$("#<%=btnAceptarContacto.ClientID%>").click(validaContacto);
});
}

//Invocando Funcion de Configuración
ConfiguraJQueryNotificacion();
</script>

<div id="encabezado_forma">
<img src="../Image/Producto.png" />
<h1>Notificación</h1>
</div>
<asp:UpdatePanel ID="upMenuPrincipal" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<nav id="menuForma">
<ul>
<li class="green">
<a href="#" class="fa fa-floppy-o"></a>
<ul>
<li>
<asp:LinkButton ID="lkbNuevo" runat="server" Text="Nuevo" OnClick="lkbElementoMenu_Click" CommandName="Nuevo" /></li>
<li>
<asp:LinkButton ID="lkbAbrir" runat="server" Text="Abrir" OnClick="lkbElementoMenu_Click" CommandName="Abrir" /></li>
<li>
<asp:LinkButton ID="lkbGuardar" runat="server" Text="Guardar" OnClick="lkbElementoMenu_Click" CommandName="Guardar" /></li>
<li>
<asp:LinkButton ID="lkbSalir" runat="server" Text="Salir" OnClick="lkbElementoMenu_Click" CommandName="Salir" /></li>
</ul>
</li>
<li class="red">
<a href="#" class="fa fa-pencil-square-o"></a>
<ul>
<li>
<asp:LinkButton ID="lkbEditar" runat="server" Text="Editar" OnClick="lkbElementoMenu_Click" CommandName="Editar" /></li>
<li>
<asp:LinkButton ID="lkbEliminar" runat="server" Text="Eliminar" OnClick="lkbElementoMenu_Click" CommandName="Eliminar" /></li>
</ul>
</li>
<li class="blue">
<a href="#" class="fa fa-cog"></a>
<ul>
<li>
<asp:LinkButton ID="lkbBitacora" runat="server" Text="Bitácora" OnClick="lkbElementoMenu_Click" CommandName="Bitacora" /></li>
<li>
<asp:LinkButton ID="lkbReferencias" runat="server" Text="Referencias" OnClick="lkbElementoMenu_Click" CommandName="Referencias" /></li>
<li>
<asp:LinkButton ID="lkbArchivos" runat="server" Text="Archivos" OnClick="lkbElementoMenu_Click" CommandName="Archivos" /></li>
</ul>
</li>
<li class="yellow">
<a href="#" class="fa fa-question-circle"></a>
<ul>
<li>
<asp:LinkButton ID="lkbAcercaDe" runat="server" Text="Acerca de" OnClick="lkbElementoMenu_Click" CommandName="Acerca" /></li>
<li>
<asp:LinkButton ID="lkbAyuda" runat="server" Text="Ayuda" OnClick="lkbElementoMenu_Click" CommandName="Ayuda" /></li>
</ul>
</li>
</ul>
</nav>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:PostBackTrigger ControlID="lkbBitacora" />
<asp:PostBackTrigger ControlID="lkbAbrir" />
<asp:PostBackTrigger ControlID="lkbReferencias" />
</Triggers>
</asp:UpdatePanel>
<div  class="seccion_controles">
<div class="columna2x">
<div class="header_seccion">
<img src="../Image/DatosProducto.png" />
<h2>Datos de la Notificación</h2>
</div>
<div class="columna">
<div class="renglon2x">
<div class="etiqueta">
<label for="txtContacto">Contacto</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtContacto" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtContacto" runat="server" TabIndex="1" CssClass="textbox2x validate[required, custom[IdCatalogo]]"  MaxLength="150"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarContacto" />
<asp:AsyncPostBackTrigger ControlID="btnEliminarContacto" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="validador">
<asp:UpdatePanel ID="uplnkVentana" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkVentana"   OnClick="lnkVentana_Click" runat="server" Text="Contacto" TabIndex="3"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
</Triggers>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtCliente">Cliente</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtCliente" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtCliente" runat="server" TabIndex="2" CssClass="textbox2x validate[required, custom[IdCatalogo]]"  MaxLength="150"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtUbicacion">Ubicación</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtUbicacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtUbicacion" runat="server" TabIndex="3" CssClass="textbox2x validate[required, custom[IdCatalogo]]" MaxLength="150"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnGuardar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnGuardar" runat="server" TabIndex="4"  OnClick="btnGuardar_Click" Text="Guardar" CssClass="boton" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCancelar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnCancelar" runat="server" TabIndex="5"  OnClick="btnCancelar_Click" Text="Cancelar" CssClass="boton_cancelar" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
            
</div>  </div>      
</div>
<div  class="contenedor_media_seccion">  
<div class="header_seccion">
<h2>Detalles</h2>
</div>
<div class="renglon3x">
<div class="etiqueta_50px">
<label for="ddlTamanoEvento">Mostrar:</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamanoEvento" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoEvento" runat="server" CssClass="dropdown" TabIndex="6"
OnSelectedIndexChanged="ddlTamanoEvento_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label>Ordenado Por:</label>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplblOrdenadoEvento" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblOrdenadoEvento" runat="server" ></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvEvento" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplkbExportarEvento" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbExportarEvento" runat="server" TabIndex="7" 
OnClick="lkbExportarEvento_Click" Text="Exportar"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbExportarEvento" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div  class="grid_seccion_completa_altura_variable">
<asp:UpdatePanel ID="upgvEvento" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvEvento" runat="server"  TabIndex="8" AutoGenerateColumns="False" OnSorting="gvEvento_Sorting" OnPageIndexChanging="gvEvento_PageIndexChanging" OnRowDataBound="gvEvento_RowDataBound"
ShowFooter="True" CssClass="gridview" Width="100%"  AllowPaging="True" AllowSorting="True"  PageSize="10" >
<Columns>
<asp:TemplateField HeaderText="Evento" SortExpression="Descripcion">
<ItemTemplate>
<asp:Label  ID="lblEvento" runat="server" Text='<%#Eval("Descripcion") %>' ></asp:Label>
</ItemTemplate>
<FooterTemplate>
<asp:DropDownList runat="server"  ID="ddlEvento" CssClass="dropdown2x" >
</asp:DropDownList>
</FooterTemplate>
</asp:TemplateField>
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lnkDeshabilitar" TabIndex="9" runat="server"  OnClick="lnkDeshabilitar_Click"  Text="Deshabilitar" CommandName="Deshabilitar" ></asp:LinkButton>
</ItemTemplate>
<FooterTemplate>
<asp:LinkButton ID="lnkInsertar" runat="server" TabIndex="10"  OnClick="lnkInsertar_Click" Text="Insertar"  CommandName="Insertar"  ></asp:LinkButton>
</FooterTemplate>
</asp:TemplateField>   
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lnkBitacora"  TabIndex="11" runat="server"   OnClick="lnkBitacora_Click" Text="Bitácora" CommandName="Bitacora" ></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>                                      
</Columns>
<AlternatingRowStyle CssClass="gridviewrowalternate" />
<FooterStyle CssClass="gridviewfooter" />
<HeaderStyle CssClass="gridviewheader" />
<RowStyle CssClass="gridviewrow" />
<SelectedRowStyle CssClass="gridviewrowselected" />
<SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
<SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlTamanoEvento" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel> 
</div>   
</div>
<div id="crearContacto" class="modal">
<div id="contenedorContacto" class="contenedor_modal_seccion_completa_arriba" style="width:700px">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarContacto" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarContacto" runat="server" Text="Cerrar" OnClick="lkbCerrarVentanaModal_Click" CommandName="Contacto">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div> 
<div class="seccion_controles">
<div class="header_seccion">
<img src="../Image/User.jpg" />
<h2>Contacto</h2>
</div>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label for="uptxtNombre">Nombre:</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtNombre" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtNombre" TabIndex="12" runat="server" MaxLength="200" CssClass="textbox2x validate[required]"></asp:TextBox> 
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lnkVentana" />
<asp:AsyncPostBackTrigger  ControlID="btnAceptarContacto"/>
</Triggers>
</asp:UpdatePanel>
</div>
   
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtTelefono">Telefono:</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtTelefono" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtTelefono" runat="server" MaxLength="30" TabIndex="13" CssClass="textbox2x validate[required]"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lnkVentana" />
<asp:AsyncPostBackTrigger  ControlID="btnAceptarContacto"/>
</Triggers>
</asp:UpdatePanel>
</div>
</div>          
<div class="renglon2x">
<div class="etiqueta">
<label for="txtEMail">E-Mail:</label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="uptxtTelefonoPS" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtEMail" MaxLength="100" TabIndex="14" CssClass="textbox2x validate[required, custom[email]]"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lnkVentana" />
<asp:AsyncPostBackTrigger  ControlID="btnAceptarContacto"/>
</Triggers>
</asp:UpdatePanel>
</div>
</div>  
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptarContaco" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptarContacto" TabIndex="15" runat="server" OnClick="btnAceptarContacto_Click"  CssClass="boton" Text="Aceptar"  />
</ContentTemplate>
<Triggers>          
</Triggers>
</asp:UpdatePanel>
</div> 
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnEliminarContacto" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnEliminarContacto" runat="server"  TabIndex="16" OnClick="btnEliminarContacto_Click"  CssClass="boton" Text="Eliminar"  />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>  
</div>      
</div>     
</div>  
</div></div>
</asp:Content>
