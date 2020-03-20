<%@ Page Title="Actividad" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="Actividad.aspx.cs" Inherits="SAT.Mantenimiento.Actividad" %>
<%@ Register Src="~/UserControls/wucRequisicion.ascx" TagName="wucRequiquisicion" TagPrefix="wuc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!--Estilos para la forma-->
<link href ="../CSS/Controles.css" rel="stylesheet" />
<link href="../CSS/Forma.css" rel="stylesheet" />
<link href="../CSS/GridviewScroll.css" rel="stylesheet" type="text/css" />
<!--Estilo del query-->
<link href="../CSS/jquery.validationEngine.css"  rel="stylesheet"/>
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js"></script>
<script type="text/javascript" src="../Scripts/gridviewScroll.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
    <!--Valida las inserciones de datos en los controles-->
<script type ="text/javascript">
//Obtiene la instancia actual de la pagina y añade un manejador de eventos
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Creación de la función que permite finalizar o validar los controles a partir de un error.
function EndRequestHandler(sender, args) {
//Valida si el argumento de error no esta definido
if (args.get_error() == undefined) {
//Invoca a la Funcion ConfiguraJqueryActividad
ConfiguraJQueryActividad();
}
}

//Declara la función que valida los controles de la pagina
function ConfiguraJQueryActividad() {
    $(document).ready(function () {
        //Creación  y asignación de la funcion a la variable validaActividad
        var validaActividad = function () {
            //Creación de las variables y asignacion de los controles de la pagina Actividad
            var isValid1 = !$("#<%=txtDescripcion.ClientID%>").validationEngine('validate');
            //Devuelve un valor a la funcion
            return isValid1;
        }
        //Permite que los eventos de guardar activen la funcion de validación de controles.
        $("#<%=btnGuardar.ClientID%>").click(validaActividad);
        $("#<%=lkbGuardar.ClientID%>").click(validaActividad);
        //Creación  y asignación de la funcion a la variable validaActividad
        var validaActividadPuesto = function () {
            //Creación de las variables y asignacion de los controles de la pagina Actividad
            var isValid1 = !$("#<%=txtTiempoActividad.ClientID%>").validationEngine('validate');
            //Devuelve un valor a la funcion
            return isValid1;
        }
        $("#<%=btnGuardaActividadPuesto.ClientID%>").click(validaActividadPuesto);

        //Añadiendo Encabezado Fijo
        $("#<%=gvProducto.ClientID%>").gridviewScroll({
            width: document.getElementById("contenedorProdctoActividad").offsetWidth - 15,
            height: 400,
            //freezesize: 4
        });
    });
}

//Invocando Función de Configuración
ConfiguraJQueryActividad();
</script>
<!--Fin validación de datos-->
<div id="encabezado_forma">
<img src="../Image/llave-inglesa.png" />
<h1>Actividad</h1>
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
</ul>
</li>
<li class="red">
<a href="#" class="fa fa-pencil-square-o"></a>
<ul>
<li>
<asp:LinkButton ID="lkbEditar" runat="server" Text="Editar" OnClick="lkbElementoMenu_Click" CommandName="Editar" /></li>
<li>
<asp:LinkButton ID="lkbEliminar" runat="server" Text="Eliminar" OnClick="lkbElementoMenu_Click" CommandName="Eliminar" /></li>
<li>
<asp:LinkButton ID="lkbProducto" runat="server" Text="Producto" OnClick="lkbElementoMenu_Click" CommandName="Producto" /></li>
</ul>
</li>
<li class="blue">
<a href="#" class="fa fa-cog"></a>
<ul>
<li>
<asp:LinkButton ID="lkbBitacora" runat="server" Text="Bitácora" OnClick="lkbElementoMenu_Click" CommandName="Bitacora" /></li>
<li>
<asp:LinkButton ID="lkbReferencias" runat="server" Text="Referencias" OnClick="lkbElementoMenu_Click" CommandName="Referencias" /></li>
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
<div class="contenedor_controles">
<div class="columna3x">
<div class="header_seccion">
<h2>Descripción de Actividades</h2>
</div>       
<div class="renglon2x">
<div class="etiqueta">
<label for ="txtDescripcion">Descripción</label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="uptxtDescripcion" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtDescripcion" CssClass="textbox2x validate[required]" TabIndex="1" MaxLength="150"></asp:TextBox> 
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID ="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID ="lkbEliminar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlFamilia">Familia</label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="upddlFamilia" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList runat="server" ID="ddlFamilia" CssClass="dropdown" OnSelectedIndexChanged="ddlFamilia_SelectedIndexChanged" AutoPostBack="true" TabIndex="2" ></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID ="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID ="lkbEliminar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlSubFamilia">SubFamilia</label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="upddlSubFamilia" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList runat="server" ID="ddlSubFamilia" CssClass="dropdown" TabIndex="3"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID ="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID ="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="ddlFamilia" EventName="SelectedIndexChanged" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlTipoUnidad">Tipo Unidad</label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="upddlTipoUnidad" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList runat="server" ID="ddlTipoUnidad"  CssClass="dropdown"  TabIndex="4" OnSelectedIndexChanged="ddlTipoUnidad_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID ="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID ="lkbEliminar" EventName="Click" />                            
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlSubTipoUnidad">Sub tipo Unidad</label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="upddlSubTipoUnidad" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList runat="server" ID="ddlSubTipoUnidad" CssClass="dropdown" TabIndex="5"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID ="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID ="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="ddlTipoUnidad" EventName="SelectedIndexChanged" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel runat="server" ID="upbtnCancelar" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button runat="server" ID="btnCancelar" cssClass="boton_cancelar" Text="Cancelar" OnClick="btnCancelar_Click" TabIndex="7"/>
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
<div class="controlBoton">
<asp:UpdatePanel runat="server" ID="upbtnGuardar" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button runat="server" ID="btnGuardar" CssClass="boton" Text="Guardar" OnClick="btnGuardar_Click" TabIndex="6" />
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
  
</div>
<div class="columna3x">
<div class="header_seccion">
<h2>Puesto Actividad</h2>
</div>
<div class="renglon3x">
<div class="etiqueta_50px">
<label for="ddlPuesto">Puesto</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlPuesto" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList runat="server" ID="ddlPuesto" CssClass="dropdown" TabIndex="9"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardaActividadPuesto" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarActividadPuesto" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="gvActividadPuesto" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<label for="txtTiempoActividad">Duración</label>
</div>
<div class="control_60px">
<asp:UpdatePanel runat="server" ID="uptxtTiempoActividad" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtTiempoActividad" CssClass="textbox_50px validate[required, custom[positiveNumber]]" TabIndex="10" MaxLength="9"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardaActividadPuesto" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarActividadPuesto" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="gvActividadPuesto" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<label>minutos</label>
</div>    
<div class="control_100px">
<asp:UpdatePanel runat="server" ID="upbtnGuardaActividadPuesto" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button runat="server" ID="btnGuardaActividadPuesto" CssClass="boton" Text="Guardar" OnClick="btnGuardaActividadPuesto_Click" TabIndex="11" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardaActividadPuesto" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="gvActividadPuesto" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control_100px">
<asp:UpdatePanel runat="server" ID="upbtnActividadPuesto" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button runat="server" ID="btnCancelarActividadPuesto" OnClick="btnCancelarActividadPuesto_Click" CssClass="boton_cancelar" Text="Cancelar" TabIndex="12" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardaActividadPuesto" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="gvActividadPuesto" />
</Triggers>
</asp:UpdatePanel>
</div>  
</div>
<div class="renglon3x">
<div class="etiqueta_50px">
<label for="ddlTamano">Mostrar:</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="upddlTamano" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamano" runat="server" CssClass="dropdown_100px" AutoPostBack="true"
OnSelectedIndexChanged="ddlTamano_SelectedIndexChanged" TabIndex="13">
</asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardaActividadPuesto" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarActividadPuesto" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblOrdenar">Ordenado Por:</label>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplblOrdenar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b><asp:Label ID="lblOrdenar" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvActividadPuesto" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<asp:UpdatePanel ID="uplnkExportar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportar" runat="server" OnClick="lnkExportar_Click" Text="Exportar" TabIndex="15"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardaActividadPuesto" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarActividadPuesto" EventName="Click" />
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
<!--GridView-->
<div class="grid_seccion_completa_100px_altura">
<asp:UpdatePanel ID="upgvActividadPuesto" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvActividadPuesto" runat="server" AllowPaging="True" AllowSorting="True" TabIndex="16"
OnPageIndexChanging="gvActividadPuesto_PageIndexChanging" OnSorting="gvActividadPuesto_Sorting"
PageSize="5" CssClass="gridview" ShowFooter="True" Width="100%" AutoGenerateColumns="False">
<AlternatingRowStyle CssClass="gridviewrowalternate" />
<EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
<FooterStyle CssClass="gridviewfooter" />
<HeaderStyle CssClass="gridviewheader" />
<RowStyle CssClass="gridviewrow" />
<SelectedRowStyle CssClass="gridviewrowselected" />
<SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
<SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
<Columns>
<asp:BoundField DataField="Id" HeaderText="ID" SortExpression="Id" Visible="false" />
<asp:BoundField DataField="Puesto" HeaderText="Puesto" SortExpression="Puesto" ItemStyle-HorizontalAlign="Left" />
<asp:TemplateField HeaderText="Duracion" SortExpression="Duracion">
    <ItemTemplate>
        <asp:Label ID="lblDuracion" runat="server" Text='<%# TSDK.Base.Cadena.ConvierteMinutosACadena(Convert.ToInt32(TSDK.Base.Cadena.VerificaCadenaVacia(Eval("Duracion").ToString(), "0"))) %>'></asp:Label>
    </ItemTemplate>
    <ItemStyle HorizontalAlign="Right" />
</asp:TemplateField>                          
<asp:BoundField DataField="CostoHora" HeaderText="Costo x Hora" SortExpression="CostoHora" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right"/>
<asp:BoundField DataField="ManoObra" HeaderText="Mano Obra" SortExpression="ManoObra" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right"/>
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lnkEditar" runat="server" Text="Editar" OnClick="lnkEditar_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lnkEliminar" runat="server" Text="Eliminar" OnClick="lnkEliminar_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlTamano" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarActividadPuesto" />
<asp:AsyncPostBackTrigger ControlID="btnGuardaActividadPuesto" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="renglon3x"></div>
<!--GridView-->
</div>
</div>   
<div class="contenedor_controles">
<div class="header_seccion">
<h2>Producto</h2>
</div>
<!--GRIDVIEW PRODUCTO-->
<div class="renglon3x">
<div class="etiqueta_50px">
<label for="ddlTamanProducto">Mostrar:</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="upddlTamanoProducto" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoProducto" runat="server" CssClass="dropdown_100px" AutoPostBack="true"
OnSelectedIndexChanged="ddlTamanoProducto_SelectedIndexChanged" TabIndex="16">
</asp:DropDownList>
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
<div class="etiqueta">
<label for="lblOrdenarProducto">Ordenado Por:</label>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplblOrdenarProducto" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b><asp:Label ID="lblOrdenarProducto" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvProducto" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<asp:UpdatePanel ID="uplnkExportarProducto" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportarProducto" runat="server" OnClick="lnkExportarProducto_Click" Text="Exportar" TabIndex="17"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportarProducto" />
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
<div class="grid_seccion_completa_altura_variable" id="contenedorProdctoActividad">
<asp:UpdatePanel ID="upgvProducto" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvProducto" runat="server" AllowPaging="True" AllowSorting="True" TabIndex="16"
OnPageIndexChanging="gvProducto_PageIndexChanging" OnSorting="gvProducto_Sorting"
PageSize="5" CssClass="gridview" ShowFooter="True" Width="100%" AutoGenerateColumns="False">
<AlternatingRowStyle CssClass="gridviewrowalternate" />
<EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
<FooterStyle CssClass="gridviewfooter" />
<HeaderStyle CssClass="gridviewheader" />
<RowStyle CssClass="gridviewrow" />
<SelectedRowStyle CssClass="gridviewrowselected" />
<SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
<SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
<Columns>
<asp:BoundField DataField="IdRequisicion" HeaderText="IdRequisicion" SortExpression="IdRequisicion" Visible="false" />
<asp:BoundField DataField="Categoria" HeaderText="Categoria" SortExpression="Categoria" ItemStyle-HorizontalAlign="Left" />
<asp:BoundField DataField="CodProducto" HeaderText="Código Producto" SortExpression="CodProducto" ItemStyle-HorizontalAlign="Right" />
<asp:BoundField DataField="Producto" HeaderText="Producto" SortExpression="Producto" ItemStyle-HorizontalAlign="Left" />                           
<asp:BoundField DataField="UnidadMedida" HeaderText="Unidad Medida" SortExpression="UnidadMedida" ItemStyle-HorizontalAlign="Left" /> 
<asp:BoundField DataField="Precio" HeaderText="Precio Unitario" SortExpression="Precio" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" />
<asp:BoundField DataField="Cantidad" HeaderText="Cantidad" SortExpression="Cantidad" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right"/> 
<asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" FooterStyle-HorizontalAlign="Right"/> 
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlTamanoProducto" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbProducto" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarImagen" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
<!--FIN GRIDVIEW PRODUCTO-->
</div>
<!--Ventana modal Requisicion-->
<div id="RequisicionModal" class="modal">
<div id="Requisicion" class="contenedor_modal_seccion_completa_arriba">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplnkCerrarImagen" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrarImagen" runat="server" OnClick="lnkCerrarImagen_Click" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="seccion_controles_modal">
<div class="header_seccion">
<h1>Producto</h1>
</div>
<asp:UpdatePanel ID="upwucRequisicion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<wuc:wucRequiquisicion runat="server" ID="wucRequisicion"  OnClickGuardarRequisicion="wucRequisicion_ClickGuardarRequisicion" OnClickSolicitarRequisicion="wucRequisicion_ClickSolicitarRequisicion" Contenedor="#Requisicion"/>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbProducto" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>  
<!--Fin ventana modal Requisición-->                
</asp:Content>