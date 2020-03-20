<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucCuentaBanco.ascx.cs" Inherits="SAT.UserControls.wucCuentaBanco" %>
<!--hoja de estilos que dan formato al control de usuario-->
<link href="../CSS/Forma.css" rel="stylesheet" />
<link href="../CSS/Controles.css" rel="stylesheet" />
<link href="../CSS/ControlesUsuario.css" rel="stylesheet" />
<!-- Estilo de validación de los controles-->
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" />
<!--Invoca al estilo encargado de dar formato a las cajas de texto que almacenen datos datatime -->
<link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
<!--Librerias para la validacion de los controles-->
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js"></script>
<!--Invoca a los script que que validan los datos de Fecha-->
<script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
<script type="text/javascript">
//Obtiene la instancia actual de la pagina y añade un manejador de eventos
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Creación de la función que permite finalizar o validar los controles a partir de un error.
function EndRequestHandler(sender, args) {
//Valida si el argumento de error no esta definido
if (args.get_error() == undefined) {
//Invoca a la Funcion ConfiguraJQueryCuentas
ConfiguraJQueryCuentas();
}
}
//Declara la función que valida los controles de la pagina
function ConfiguraJQueryCuentas() {
$(document).ready(function () {
    //Validación de controles de Inserción de Ciudades
    var validacionGuardarCuenta = function () {
        var isValidP1 = !$('.scriptCuenta').validationEngine('validate');
        return isValidP1;

    };
    //Validación de campos requeridos
    $('.scriptGuardarCuenta').click(validacionGuardarCuenta);
});
}
ConfiguraJQueryCuentas();
</script>
<div class ="seccion_controles">
<div class="header_seccion">
    <img src="../Image/Depositos.png" />
<h2>Alta de Cuentas  [
<asp:Label ID="lblEntidad" runat="server">Entidad</asp:Label>]</h2>
</div>
        <div class="columna3x">
 <div class="renglon2x">
                <div class="etiqueta">
                    <label for="chkActivas">Activas</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="upchkActivas" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:CheckBox ID="chkActivas" Checked="true" runat="server" AutoPostBack="true"  OnCheckedChanged="chkActivas_CheckedChanged" TabIndex="2" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            </div> <div class="contenedor_controles">
<div class="renglon3x">
<div class="control">
<asp:UpdatePanel ID="upddlTamanoCuentas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoCuentas" runat="server" CssClass="dropdown" TabIndex="28"
OnSelectedIndexChanged="ddlTamanoCuentas_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label>Ordenado Por:</label>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplblOrdenadoCuentas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblOrdenadoCuentas" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvCuentas" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplnkExportar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbExportarCuentas" runat="server" TabIndex="29" 
OnClick="lkbExportarCuentas_Click" Text="Exportar"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbExportarCuentas" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div  class="grid_seccion_completa_200px_altura">
<asp:UpdatePanel ID="upgvCuentas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvCuentas" runat="server"  TabIndex="30" AutoGenerateColumns="False" OnSorting="gvCuentas_Sorting" OnPageIndexChanging="gvCuentas_PageIndexChanging" OnRowDataBound="gvCuentas_RowDataBound"
ShowFooter="True" CssClass="gridview2" Width="100%"  AllowPaging="True" AllowSorting="True"  PageSize="10" >
<Columns>
<asp:TemplateField HeaderText="Banco" SortExpression="Banco">
<ItemTemplate>
<asp:Label  ID="lblBanco" runat="server" Text='<%#Eval("Banco") %>' ></asp:Label>
</ItemTemplate>
<FooterTemplate>
<asp:DropDownList runat="server"  ID="ddlBanco" CssClass="dropdown" >
</asp:DropDownList>
</FooterTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Num Cuenta" SortExpression="NumCuenta">
<ItemTemplate>
<asp:Label  ID="lbltxtNumCuenta" runat="server" Text='<%#Eval("NumCuenta") %>' ></asp:Label>
</ItemTemplate>
<FooterTemplate>
<asp:TextBox ID="txtNumCuenta" MaxLength="50" runat="server" CssClass="textbox scriptCuenta validate[required]" >
</asp:TextBox>
</FooterTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Tipo Cuenta" SortExpression="TipoCuenta">
<ItemTemplate>
<asp:Label  ID="lblTipoCuenta" runat="server" Text='<%#Eval("TipoCuenta") %>' ></asp:Label>
</ItemTemplate>
<FooterTemplate>
<asp:DropDownList runat="server"  ID="ddlTipoCuenta" CssClass="dropdown" >
</asp:DropDownList>
</FooterTemplate>
</asp:TemplateField>  
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lnkBitacora"  TabIndex="33" runat="server"  OnClick="lkbAccionCuentas_Click"  Text="Bitácora" CommandName="Bitacora" ></asp:LinkButton>
</ItemTemplate>
<FooterTemplate>
<asp:LinkButton ID="lnkInsertar" runat="server" TabIndex="31" OnClick="lkbAccionCuentas_Click"  CssClass="textbox scriptGuardarCuenta" Text="Insertar"  CommandName="Insertar"  ></asp:LinkButton>
</FooterTemplate>
</asp:TemplateField> 
 <asp:TemplateField SortExpression="Deshabilitar">
<ItemTemplate>
<asp:LinkButton ID="lnkDeshabilitar" TabIndex="32" runat="server"  OnClick="lkbAccionCuentas_Click"  Text='<%#Eval("Deshabilitar") %>'  CommandName='<%#Eval("Deshabilitar") %>' ></asp:LinkButton>
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
<asp:AsyncPostBackTrigger ControlID="ddlTamanoCuentas" />
<asp:AsyncPostBackTrigger ControlID="chkActivas" />
</Triggers>
</asp:UpdatePanel> 
</div>
</div>
</div>
 