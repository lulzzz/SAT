<%@ Page Title="Asignación de Vales" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="DieselFactura.aspx.cs" Inherits="SAT.EgresoServicio.DieselFactura" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<!-- Estilos documentación de servicio -->
<link href="../CSS/DocumentacionServicio.css" rel="stylesheet" />
<link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
<link href="../CSS/ControlesUsuario.css" type="text/css" rel="stylesheet" />
<link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
<!-- Estilos Autocomplete, Mascara y Validadores JQuery -->
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
<link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
<!-- Bibliotecas para Validación de formulario -->
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
<script type="text/javascript">
//Obteniendo instancia actual de la página y añadiendo manejador de evento
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
function EndRequestHandler(sender, args) {
if (args.get_error() == undefined) {
ConfiguraDieselFactura();
}
}
//Declarando Función de Configuración
function ConfiguraDieselFactura()
{   //Inicializando Contenido
$(document).ready(function () {
//Declarando Función de Validación
var validaBusquedaFactura = function () {
    var isValid1 = !$("#<%=txtFolio.ClientID%>").validationEngine('validate');
    var isValid2 = !$("#<%=txtSerie.ClientID%>").validationEngine('validate');
//Devolviendo Resultado
    return isValid1 && isValid2;
}
    //Declarando Función de Validación
    var validaBusquedaVale = function () {
        var isValid1 = !$("#<%=txtFechaCarga.ClientID%>").validationEngine('validate');
        var isValid2 = !$("#<%=txtNoVale.ClientID%>").validationEngine('validate');
        //Devolviendo Resultado
        return isValid1 && isValid2;
    }
//Añadiendo Validación al Método Click Buscar Vale
    $("#<%=btnBuscarVale.ClientID%>").click(validaBusquedaVale);
    //Añadiendo Validación al Método Click Buscar Vale
    $("#<%=btnBuscarFactura.ClientID%>").click(validaBusquedaFactura);
    // *** Fecha de inicio, fin de Registro (Idioma: Español, Formato: 'dd:MM:aaaa HH:mm') *** //
    $("#<%=txtFechaCarga.ClientID%>").datetimepicker({
        lang: 'es',
        format: 'd/m/Y',
        timepicker: false
    });
    // *** Fecha de Carga (Idioma: Español, Formato: 'dd:MM:aaaa HH:mm') *** //
    $('.scriptFechaE').datetimepicker({
        lang: 'es',
        format: 'd/m/Y H:i'
    });
    //Validación de controles de Edición de Vales
    var validacionEdicionVales= function () {
        var isValidP1 = !$('.scriptLitrosE').validationEngine('validate');
        var isValidP2 = !$('.scriptFechaE').validationEngine('validate');
        return isValidP1 && isValidP2;

    };
    //Validación de campos requeridos
    $('.scriptGuardarValeE').click(validacionEdicionVales);
  
});
}
//Ejecutando Función
ConfiguraDieselFactura();
</script>
<div id="encabezado_forma">
<h1>Asignación de Vales de Diesel</h1>
</div>
<div class="seccion_controles">
<div class="columna3x">
<div class="header_seccion">
<img src="../Image/FacturacionCargos.png" />
<h2>Facturas</h2>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtProveedor">Proveedor</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtProveedor" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlProveedor" runat="server" CssClass="dropdown2x" TabIndex="1" AutoPostBack="true"
OnSelectedIndexChanged="ddlProveedor_SelectedIndexChanged"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtSerie">Serie</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtSerie" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtSerie" runat="server" CssClass="textbox" TabIndex="2"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtFolio">Folio</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtFolio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFolio" runat="server" CssClass="textbox validate[custom[positiveNumber]]" TabIndex="3"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
</div>
<div class="control2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnBuscarFactura" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnBuscarFactura" runat="server" Text="Buscar Factura" TabIndex="7"
CssClass="boton" OnClick="btnBuscarFactura_Click" />
</ContentTemplate>
</asp:UpdatePanel>
</div></div>
    </div>
<div class="renglon2x">
<div class="etiqueta_50px">
<label for="ddlTamanoFac">Mostrar:</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="upddlTamanoFac" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoFac" runat="server" CssClass="dropdown_100px" AutoPostBack="true"
TabIndex="9" OnSelectedIndexChanged="ddlTamanoFac_SelectedIndexChanged"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<label for="lblOrdenadoFactura">Ordenado:</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uplblOrdenadoFactura" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblOrdenadoFactura" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvFacturas" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<asp:UpdatePanel ID="uplnkExportarFactura" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportarFactura" runat="server" Text="Exportar" TabIndex="10"
OnClick="lnkExportarFactura_Click"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportarFactura" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_media_altura">
<asp:UpdatePanel ID="upgvFacturas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvFacturas" runat="server" AllowSorting="true" AllowPaging="true"
CssClass="gridview" OnSorting="gvFacturas_Sorting" OnPageIndexChanging="gvFacturas_PageIndexChanging"
AutoGenerateColumns="false" PageSize="25" ShowFooter="true" Width="90%" OnRowDataBound="gvFacturas_RowDataBound">
<AlternatingRowStyle CssClass="gridviewrowalternate" />
<EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
<FooterStyle CssClass="gridviewfooter" />
<HeaderStyle CssClass="gridviewheader" />
<RowStyle CssClass="gridviewrow" />
<SelectedRowStyle CssClass="gridviewrowselected" />
<SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
<SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
<Columns>
<asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="false" />
<asp:BoundField DataField="SerieFolio" HeaderText="Serie/Folio" SortExpression="SerieFolio" />
<asp:TemplateField SortExpression="UUID" HeaderText="UUID">
<ItemTemplate>
<asp:Label ID="lblUUIDComp" runat="server" Text='<%#TSDK.Base.Cadena.InvierteCadena(TSDK.Base.Cadena.TruncaCadena(TSDK.Base.Cadena.InvierteCadena(Eval("UUID").ToString()), 12, "...")) %>'
ToolTip='<%#Eval("UUID")%>'></asp:Label>
</ItemTemplate>
<ItemStyle HorizontalAlign="Right" />
</asp:TemplateField>
<asp:BoundField DataField="FecFac" HeaderText="Fecha Factura" SortExpression="FecFac" DataFormatString="{0:dd/MM/yyyy }" />
<asp:BoundField DataField="Monto" HeaderText="Monto" SortExpression="Monto" DataFormatString="{0:c}" />
<asp:BoundField DataField="Asignado" HeaderText="Asignado" SortExpression="Asignado" DataFormatString="{0:c}"/>
<asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lkbSeleccionar" runat="server" OnClick="lkbSeleccionar_Click"
Text="Seleccionar"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnBuscarFactura" />
<asp:AsyncPostBackTrigger ControlID="btnAsignarVales" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoFac" />
<asp:AsyncPostBackTrigger ControlID="gvValesAsignados" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="columna3x">
<div class="header_seccion">
<img src="../Image/Evidencia.png" />
<h2>Vales de Diesel</h2>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlEstacionCombiustible">Estación</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlEstacionCombiustible" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlEstacionCombustible" runat="server" CssClass="dropdown2x" TabIndex="5"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlProveedor" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtNoVale">No. Vale</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtNoVale" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtNoVale" runat="server" CssClass="textbox validate[custom[onlyNumberSp]]" TabIndex="6"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtFechaCarga">Fecha Carga</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtFechaCarga" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFechaCarga" runat="server" CssClass="textbox validate[required, custom[date]]" TabIndex="4"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
  <div class="etiqueta">
                    <asp:UpdatePanel ID="upchkIncluir" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:CheckBox ID="chkIncluir" runat="server" Text="¿Incluir?" TabIndex="2" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
</div>
<div class="renglon2x">
<div class="etiqueta">
</div>
<div class="etiqueta">
</div>
<div class="control2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnBuscarVale" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnBuscarVale" runat="server" Text="Buscar Vale" TabIndex="8"
CssClass="boton" OnClick="btnBuscarVale_Click" />
</ContentTemplate>
</asp:UpdatePanel>
</div></div>
</div>
<div class="renglon2x">
<div class="etiqueta_50px">
<label for="ddlTamanoVale">Mostrar:</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="upddlTamanoVale" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoVale" runat="server" CssClass="dropdown_100px"   AutoPostBack="true"
TabIndex="11" OnSelectedIndexChanged="ddlTamanoVale_SelectedIndexChanged"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<label for="lblOrdenadoVale">Ordenado:</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uplblOrdenadoVale" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblOrdenadoVale" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvVales" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<asp:UpdatePanel ID="uplnkExportarVale" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportarVale" runat="server" Text="Exportar" TabIndex="12"
OnClick="lnkExportarVale_Click"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportarVale" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_media_altura">
<asp:UpdatePanel ID="upgvVales" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvVales" runat="server" AllowSorting="true" AllowPaging="true"
CssClass="gridview" OnSorting="gvVales_Sorting" OnPageIndexChanging="gvVales_PageIndexChanging"
AutoGenerateColumns="false" PageSize="25" ShowFooter="true" Width="90%" >
<AlternatingRowStyle CssClass="gridviewrowalternate" />
<EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
<FooterStyle CssClass="gridviewfooter" />
<HeaderStyle CssClass="gridviewheader" />
<RowStyle CssClass="gridviewrow" />
<SelectedRowStyle CssClass="gridviewrowselected" />
<SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
<SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
<Columns>
<asp:TemplateField>
<HeaderStyle HorizontalAlign="Center" />
<HeaderTemplate>
<asp:CheckBox ID="chkTodos" runat="server" OnCheckedChanged="chkTodos_CheckedChanged" AutoPostBack="true" />
</HeaderTemplate>
<ItemStyle HorizontalAlign="Center" />
<ItemTemplate>
<asp:CheckBox ID="chkVarios" runat="server" OnCheckedChanged="chkTodos_CheckedChanged" AutoPostBack="true" />
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="NoVale" SortExpression="NoVale">
<ItemTemplate>
<asp:Label  ID="lblNoVale" runat="server" Text='<%#Eval("NoVale") %>' ></asp:Label>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="NoServicio" SortExpression="NoServicio">
<ItemTemplate>
<asp:Label  ID="lblNoServicio" runat="server" Text='<%#Eval("NoServicio") %>' ></asp:Label>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Referencia" SortExpression="Referencia">
<ItemTemplate>
<asp:Label  ID="lblReferencia" runat="server" Text='<%#Eval("Referencia") %>' ></asp:Label>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Tercero" SortExpression="Tercero">
<ItemTemplate>
<asp:Label  ID="lblTercero" runat="server" Text='<%#Eval("Tercero") %>' ></asp:Label>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Asignación" SortExpression="Asignacion">
<ItemTemplate>
<asp:Label  ID="lblAsignacion" runat="server" Text='<%#Eval("Asignacion") %>' ></asp:Label>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Unidad" SortExpression="Unidad">
<ItemTemplate>
<asp:Label  ID="lblUnidad" runat="server" Text='<%#Eval("Unidad") %>' ></asp:Label>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Fecha" SortExpression="Fecha">
<EditItemTemplate>
<asp:TextBox ID="txtFechaE" MaxLength="18" runat="server"  CssClass="textbox_100px scriptFechaE validate[required,custom[dateTime24]]" >
</asp:TextBox>
</EditItemTemplate>
<ItemTemplate>
<asp:Label  ID="lblFecha" runat="server" Text= '<%# Eval("Fecha","{0:dd/MM/yyyy HH:mm}") %>' ></asp:Label>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Litros" SortExpression="Litros">
<EditItemTemplate>
<asp:TextBox ID="txtLitrosE" MaxLength="18" runat="server"  CssClass="textbox_50px scriptLitrosE validate[required,custom[positiveNumber]]" >
</asp:TextBox>
</EditItemTemplate>
<ItemTemplate>
<asp:Label  ID="lbllitros" runat="server" Text='<%#Eval("Litros") %>' ></asp:Label>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Costo Comb." SortExpression="CostoCombustible">
<EditItemTemplate>
<asp:TextBox ID="txtCCombustibleE" MaxLength="18" runat="server"  CssClass="textbox_50px scriptLitrosE validate[required,custom[positiveNumber]]" >
</asp:TextBox>
</EditItemTemplate>
<ItemTemplate>
<asp:Label  ID="lblCCombustible" runat="server" Text='<%#Eval("CostoCombustible") %>' ></asp:Label>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Monto" SortExpression="Monto">
<ItemTemplate>
<asp:Label  ID="lblMonto" runat="server" Text='<%#Eval("Monto", "{0:c}") %>' ></asp:Label>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField>
<EditItemTemplate>
<asp:LinkButton ID="lnkGuardarE" runat="server" OnClick="lnkGuardarE_Click"  CssClass="textbox scriptGuardarValeE"  CommandName="GuardarE" Text="Guardar" ></asp:LinkButton>
</EditItemTemplate>
<ItemTemplate> 
<asp:LinkButton ID="lnkEditar" runat="server"  CommandName="Editar"  OnClick="lnkEditarE_Click" Text="Editar" ></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField> 
<asp:TemplateField>
<EditItemTemplate>
<asp:LinkButton ID="lnkCancelarE" runat="server" OnClick="lnkCancelarE_Click"  CssClass="textbox"  CommandName="GuardarE" Text="Cancelar" ></asp:LinkButton>
</EditItemTemplate>
</asp:TemplateField> 
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnBuscarVale" />
<asp:AsyncPostBackTrigger ControlID="btnAsignarVales" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoVale" />
<asp:AsyncPostBackTrigger ControlID="gvValesAsignados" />
<asp:AsyncPostBackTrigger ControlID="btnBuscarFactura" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAsignarVales" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAsignarVales" runat="server" Text="Asignar Vales" CssClass="boton" TabIndex="13"
OnClick="btnAsignarVales_Click" />
</ContentTemplate>
<Triggers></Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
<div class="contenedor_seccion_completa">
<div class="header_seccion">
  <img src="../Image/Documento.png" />
<h2>Vales Asignados</h2>
</div>
<div class="renglon2x">
<div class="etiqueta_50px">
<label for="ddlTamanoValesAsig">Mostrar:</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="upddlTamanoValesAsig" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoValesAsig" runat="server" CssClass="dropdown_100px" AutoPostBack="true"
TabIndex="13" OnSelectedIndexChanged="ddlTamanoValesAsig_SelectedIndexChanged"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<label for="lblOrdenadoValesAsig">Ordenado:</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uplblOrdenadoValesAsig" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblOrdenadoValesAsig" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvValesAsignados" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<asp:UpdatePanel ID="uplnkExportarValesAsig" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportarValesAsig" runat="server" Text="Exportar" TabIndex="14"
OnClick="lnkExportarValesAsig_Click"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportarValesAsig" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa">
<asp:UpdatePanel ID="upgvValesAsignados" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvValesAsignados" runat="server" AllowSorting="true" AllowPaging="true"
CssClass="gridview" OnSorting="gvValesAsignados_Sorting" OnPageIndexChanging="gvValesAsignados_PageIndexChanging"
AutoGenerateColumns="false" PageSize="25" ShowFooter="true" Width="90%">
<AlternatingRowStyle CssClass="gridviewrowalternate" />
<EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
<FooterStyle CssClass="gridviewfooter" />
<HeaderStyle CssClass="gridviewheader" />
<RowStyle CssClass="gridviewrow" />
<SelectedRowStyle CssClass="gridviewrowselected" />
<SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
<SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
<Columns>
<asp:BoundField DataField="NoVale" HeaderText="No. Vale" SortExpression="NoVale" />
<asp:BoundField DataField="Referencia" HeaderText="Referencia" SortExpression="Referencia" />
<asp:BoundField DataField="Tercero" HeaderText="Tercero" SortExpression="Tercero" />
<asp:BoundField DataField="Asignacion" HeaderText="Asignación" SortExpression="Asignacion" />
<asp:BoundField DataField="Unidad" HeaderText="Unidad" SortExpression="Unidad" />
<asp:BoundField DataField="Fecha" HeaderText="Fecha" SortExpression="Fecha" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
<asp:BoundField DataField="Litros" HeaderText="Litros" SortExpression="Litros" />
<asp:BoundField DataField="Monto" HeaderText="Monto" SortExpression="Monto" DataFormatString="{0:c}" />
<asp:TemplateField>
<ItemTemplate> 
<asp:LinkButton ID="lnkQuitar" runat="server"  OnClick="lnkQuitar_Click"  Text="Quitar" ></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField> 
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvFacturas" />
<asp:AsyncPostBackTrigger ControlID="btnBuscarFactura" />
<asp:AsyncPostBackTrigger ControlID="btnAsignarVales" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoValesAsig" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</asp:Content>
