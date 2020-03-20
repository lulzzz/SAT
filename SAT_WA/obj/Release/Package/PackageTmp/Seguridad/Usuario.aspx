<%@ Page Title="Usuario" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="Usuario.aspx.cs" Inherits="SAT.Seguridad.Usuario" %>
<%@ Register Src="~/UserControls/wucPerfilUsuarioAlta.ascx" TagName="wucPerfilUsuarioAlta" TagPrefix="tectos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Hojas de estilo-->
<link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
<link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
<!--Hoja de estilo validadores de controles-->
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" />
<!--Script que validan los datos ingresados en las cajas de texto del fomulario-->
<script src="../Scripts/jquery.validationEngine-es.js"></script>
<script src="../Scripts/jquery.validationEngine.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
    <!--Codigo que valida los controles del formulario-->
<script type="text/javascript">
//Obtiene la instancia actual de la página y amade un manejador de eventos
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Creación de la función que permite finalizar o validar los controles a partir de un error
function EndRequestHandler(sender, args) {
//Valida si el argumento de error no esta definido
if (args.get_error() == undefined) {
//Invoca a la función ConfiguraJqueryUsuario
ConfiguraJQueryUsuario();
}
}
//Declara la función qe valida los controles de la página
function ConfiguraJQueryUsuario() {
$(document).ready(function () {
//Creación y asignación de la función a la variable ValidaUsuario
var validaUsuario = function () {
//Creacion de las variables y asignación de los controles del formulario Usuario
var isValid1 = !$("#<%=txtNombre.ClientID%>").validationEngine('validate');
var isValid2 = !$("#<%=txtEmail.ClientID%>").validationEngine('validate');
var isValid3 = !$("#<%=txtContrasena.ClientID%>").validationEngine('validate');
var isValid4 = !$("#<%=txtRepitaContrasena.ClientID%>").validationEngine('validate');
var isValid5 = !$("#<%=txtPregunta.ClientID%>").validationEngine('validate');
var isValid6 = !$("#<%=txtRespuesta.ClientID%>").validationEngine('validate');
var isValid7 = !$("#<%=txtSesiones.ClientID%>").validationEngine('validate');
var isValid8 = !$("#<%=txtTiempo.ClientID%>").validationEngine('validate');
var isValid9 = !$("#<%=txtVigencia.ClientID%>").validationEngine('validate');
//Devuelve un valor a la función
return isValid1 && isValid2 && isValid3 && isValid4 && isValid5 && isValid6 && isValid7 && isValid8 && isValid9;
};
//Permite que los eventos de guardar activen la función de validación de controles
$("#<%=btnGuardar.ClientID%>").click(validaUsuario);
$("#<%=lkbGuardar.ClientID%>").click(validaUsuario);
});
}
//Invocando a la función
ConfiguraJQueryUsuario();
</script>
<!--Fin del script-->
<div id="encabezado_forma">
<img src="../Image/Cliente.jpg" />
<h1>Usuario</h1>
</div>
<!--Menú Principal-->
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
<asp:LinkButton ID="lkbBitacora" runat="server" Text="Bitácora" OnClick="lkbElementoMenu_Click" CommandName="Bitacora"/></li>
<li>
<asp:LinkButton ID="lkbReferencias" runat="server" Text="Referencias" OnClick="lkbElementoMenu_Click" CommandName="Referencias"/></li>
<li>
<asp:LinkButton ID="lkbArchivos" runat="server" Text="Archivos" OnClick="lkbElementoMenu_Click" CommandName="Archivo"/></li>
<li>
<asp:LinkButton ID="lkbSesiones" runat="server" Text="Sesiones Activas" OnClick="lkbElementoMenu_Click" CommandName="Sesiones"/></li>
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
<asp:PostBackTrigger ControlID="lkbArchivos" />
</Triggers>
</asp:UpdatePanel>
<!--Fin menú Principal-->
<div class="seccion_controles">
<div class="header_seccion">
<img src="../Image/User.jpg" />
<h2>Cuenta de usuario</h2>
</div>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label for="txtNombre">Nombre:</label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="uptxtNombre" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtNombre" CssClass="textbox2x  validate[required]" TabIndex="1" MaxLength="100"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtEmail">Email:</label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="uptxtEmail" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtEmail" CssClass="textbox2x  validate[required, custom[email]]" TabIndex="2" MaxLength="100"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtSesiones">No. Sesiones:</label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="uptxtSesiones" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtSesiones" CssClass="textbox_50px validate[required, custom[integer]]" TabIndex="3" MaxLength="8"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtTiempo">Tiempo Sesión:</label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="uptxtTiempo" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtTiempo" CssClass="textbox_50px validate[required, custom[integer]]" TabIndex="4" MaxLength="8"></asp:TextBox>
<label for="txtTiempo">minutos.</label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtVigencia">Vigencia cuenta:</label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="uptxtVigencia" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtVigencia" CssClass="textbox_50px validate[required, custom[integer]]" TabIndex="5" MaxLength="8"></asp:TextBox>
<label for="txtVigencia">diás.</label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtPregunta">Pregunta Secreta:</label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="uptxtPregunta" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtPregunta" CssClass="textbox2x validate[required]]" TabIndex="6" MaxLength="100"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtRespuesta">Respuesta Secreta:</label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="uptxtRespuesta" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtRespuesta" CssClass="textbox2x validate[required]" TabIndex="7" MaxLength="30"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtContrasena">Contraseña</label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="uptxtContrasena" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtContrasena" CssClass="textbox2x  validate[required]]" TabIndex="8" MaxLength="30" ></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtRepitaContrasena">Repita Contraseña:</label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="uptxtRepitaContrasena" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtRepitaContrasena" CssClass="textbox2x validate[required]" TabIndex="9" MaxLength="30"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtCompania">Compania:</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtCompania" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtCompania" runat="server" CssClass="textbox2x" TabIndex="10" Enabled="false"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlDepartamento">Departamento:</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlDepartamento" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlDepartamento" runat="server" CssClass="dropdown2x" TabIndex="11"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel runat="server" ID="upbtnCancelar" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button runat="server" ID="btnCancelar" Text="Cancelar" CssClass="boton_cancelar" OnClick="btnCancelar_Click" TabIndex="13" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel runat="server" ID="upbtnGuardar" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button runat="server" ID="btnGuardar" Text="Guardar" CssClass="boton" OnClick="btnGuardar_Click" TabIndex="12" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta_320px">
<asp:UpdatePanel runat="server" ID="uplblError" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label runat="server" ID="lblError" CssClass="label_error"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
<asp:UpdatePanel ID="upucPerfilUsuarioAlta" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <tectos:wucPerfilUsuarioAlta ID="ucPerfilUsuarioAlta" runat="server" TabIndex="14"
            OnClickGuardarPerfilUsuario="ucPerfilUsuarioAlta_ClickGuardarPerfilUsuario"
            OnClickEliminarPerfilUsuario="ucPerfilUsuarioAlta_ClickEliminarPerfilUsuario" />
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
    </Triggers>
</asp:UpdatePanel>

<!-- Ventana que Muestra las Sesiones Activas por Usuario -->
<div id="contenedorVentanaSesionesActivas" class="modal">
<div id="ventanaSesionesActivas" class="contenedor_ventana_confirmacion_arriba" style="width:630px">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrar" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrar" runat="server" OnClick="lkbCerrar_Click" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="columna3x">
<div class="header_seccion">
    <img src="../Image/Seguridad.png" />
    <h2>Sesiones Activas</h2>
</div>
<div class="renglon3x">
<div class="etiqueta_50px">
    <label for="ddlTamanoSA">Mostrar:</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="upddlTamanoSA" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:DropDownList ID="ddlTamanoSA" runat="server" OnSelectedIndexChanged="ddlTamanoSA_SelectedIndexChanged" 
            CssClass="dropdown_100px" AutoPostBack="true"></asp:DropDownList>
    </ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
    <label for="lblOrdenadoSA">Ordenado Por:</label>
</div>
<div class="etiqueta">
    <asp:UpdatePanel ID="uplblOrdenadoSA" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Label ID="lblOrdenadoSA" runat="server" CssClass="label_negrita"></asp:Label>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="gvSesionesActivas" EventName="Sorting" />
        </Triggers>
    </asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
    <asp:UpdatePanel ID="uplkbExportarSA" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:LinkButton ID="lkbExportarSA" runat="server" Text="Exportar" OnClick="lkbExportarSA_Click"></asp:LinkButton>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="lkbExportarSA" />
        </Triggers>
    </asp:UpdatePanel>
</div>
<div class="controlBoton">
    <asp:UpdatePanel ID="upbtnCerrarSesiones" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Button ID="btnCerrarSesiones" Text="Cerrar Sesiones" runat="server" CssClass="boton"
                OnClick="btnCerrarSesiones_Click" />
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="lkbSesiones" />
            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
        </Triggers>
    </asp:UpdatePanel>
    </div>
</div>
<div class="grid_seccion_completa_150px_altura">
<asp:UpdatePanel ID="upgvSesionesActivas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvSesionesActivas" runat="server"  AutoGenerateColumns="False" PageSize="5"
ShowFooter="True" CssClass="gridview"  Width="100%" OnPageIndexChanging="gvSesionesActivas_PageIndexChanging" 
OnSorting="gvSesionesActivas_Sorting" AllowPaging="True" AllowSorting="True">
<AlternatingRowStyle CssClass="gridviewrowalternate" />
<FooterStyle CssClass="gridviewfooter" />
<HeaderStyle CssClass="gridviewheader" />
<RowStyle CssClass="gridviewrow" />
<SelectedRowStyle CssClass="gridviewrowselected" />
<SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
<SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
<Columns>
<asp:BoundField DataField="Dispositivo" HeaderText="Dispositivo" SortExpression="Dispositivo" />
<asp:BoundField DataField="Nombre" HeaderText="Nombre" SortExpression="Nombre" />
<asp:BoundField DataField="InicioAct" HeaderText="Inicio de Actividad" SortExpression="InicioAct" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
<asp:BoundField DataField="FinAct" HeaderText="Fin de Actividad" SortExpression="FinAct" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
<asp:BoundField DataField="DireccionMAC" HeaderText="Dirección MAC" SortExpression="DireccionMAC" />
<asp:TemplateField>
    <ItemTemplate>
        <asp:LinkButton ID="lkbTerminarSesion" runat="server" Text="Terminar Sesión" OnClick="lkbTerminarSesion_Click"></asp:LinkButton>
    </ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlTamanoSA" />
<asp:AsyncPostBackTrigger ControlID="lkbSesiones" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnCerrarSesiones" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>

</div>
</div>
</asp:Content>
