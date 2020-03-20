<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucControlDiesel.ascx.cs" Inherits="SAT.UserControls.wucControlDiesel" %>
<!-- hoja de estilo que da formato al control de usuario-->
<link href="../CSS/Forma.css" rel="stylesheet" />
<link href="../CSS/Controles.css" rel="stylesheet" />
<link href="../CSS/ControlesUsuario.css" rel="stylesheet" />
<link href="../CSS/ControlPatio.css" rel="stylesheet" />
<!-- Estilo de validación de los controles-->
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" />
<!--Invoca al estilo encargado de dar formato a las cajas de texto que almacenen datos datatime -->
<link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
<!--Librerias para la validacion de los controles-->
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js"></script>
<div class="columna4x">
<div class="header_seccion">
<img src="../Image/ControlDiesel.png" />
<h2>Control Diesel Sistema vs Lecturas</h2>
</div>    
<div class="columna" style="width: 400px">
<div class="header_seccion">
<h2>Control Diesel Sistema</h2>
</div>
<div class="renglon" style="width:350px">
<div class="etiqueta_50px">
<label for="lblKilometros">Kilometros</label>
</div>
<div class="etiqueta_80px">
<asp:Label runat="server" ID="lblKilometros" CssClass="label_negrita"></asp:Label>
</div>
<div class="etiqueta_50px">
<label for="lblLitros">Litros</label>
</div>
<div class="etiqueta_80px">
<asp:Label runat="server" ID="lblLitros" CssClass="label_negrita"></asp:Label>
</div>
</div>
<div class="renglon">
    <div class="etiqueta">
        <label for="">Rendimiento </label>
    </div>
    <div class="etiqueta_200px" style="font-size:14px;">
    <asp:Label runat="server" ID="lblRendimientoSistema" CssClass="label_negrita"></asp:Label>
    </div>
</div>
<div class="renglon" style="width: 350px">       
<div class="etiqueta">
<label for="lblCriterioGridViewControlDieselSistema">Ordenado Por:</label>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplblCriterioGridViewControlDieselSistema" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblCriterioGridViewControlDieselSistema" TabIndex="15" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvControlDieselSistema" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>  
<div class="etiqueta_50px">
<asp:UpdatePanel runat="server" ID="uplkbExportarExcelControlDieselSistema" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbExportarExcelControlDieselSistema" runat="server" Text="Exportar" TabIndex="16" OnClick="lkbExportarExcelControlDieselSistema_Onclick"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbExportarExcelControlDieselSistema" />
</Triggers>
</asp:UpdatePanel>
</div>                      
</div>
<div class="grid_resumen_visor"  style="min-height: 150px; padding:30px;">
<asp:UpdatePanel ID="upgvControlDieselSistema" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvControlDieselSistema" CssClass="gridview" OnPageIndexChanging="gvControlDieselSistema_OnpageIndexChanging" OnSorting="gvControlDieselSistema_Onsorting" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="false"
ShowFooter="True" TabIndex="17" 
PageSize="25" Width="100%">
<Columns>
<asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="false" />
<asp:BoundField DataField="Unidad" HeaderText="Unidad" SortExpression="Unidad" />
<asp:BoundField DataField="Kms" HeaderText="Kms" SortExpression="Kms" ItemStyle-HorizontalAlign="Right" />
<asp:BoundField DataField="Litros" HeaderText="Litros" SortExpression="Litros" ItemStyle-HorizontalAlign="Right"/>                                                    
</Columns>
<AlternatingRowStyle CssClass="gridviewrowalternate" />
<EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
<FooterStyle CssClass="gridviewfooter" />
<HeaderStyle CssClass="gridviewheader" />
<RowStyle CssClass="gridviewrow" />
<SelectedRowStyle CssClass="gridviewrowselected" />
<SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
<SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvServiciosLiquidacion" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="columna" style="width: 350px">
<div class="header_seccion">
<h2>Control Diesel Lectura</h2>
</div>
<div class="renglon">
<div class="etiqueta_50px">
<label for="lblKilometrosLectura">Kilometros</label>
</div>
<div class="etiqueta_80px">
<asp:Label runat="server" ID="lblKilometrosLectura" CssClass="label_negrita"></asp:Label>
</div>
<div class="etiqueta_50px">
<label for="lblLitrosLectura">Litros</label>
</div>
<div class="etiqueta_80px">
<asp:Label runat="server" ID="lblLitrosLectura" CssClass="label_negrita"></asp:Label>
</div>
</div>
<div class="renglon">
    <div class="etiqueta">
        <label for="lblRendientolectura">Rendimiento: </label>
    </div>
    <div class="etiqueta_200px" style="font-size:14px;">
    <asp:Label runat="server" ID="lblRendientolectura" CssClass="label_negrita"></asp:Label>
    </div>
</div>
<div class="renglon" style="width: 350px">
<div class="etiqueta">
<label for="lblCriterioGridViewControlDieselLectura">Ordenado Por:</label>
</div>
<div class="etiqueta_50px">
<asp:UpdatePanel ID="uplblCriterioGridViewControlDieselLectura" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblCriterioGridViewControlDieselLectura" TabIndex="15" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvControlDieselLectura" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<asp:UpdatePanel runat="server" ID="uplkbExportarExcelControlDieselLectura" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbExportarExcelControlDieselLectura" runat="server" Text="Exportar" TabIndex="16" OnClick="lkbExportarExcelControlDieselLectura_Onclick"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbExportarExcelControlDieselLectura" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_resumen_visor" style="min-height: 150px;padding:30px;">
<asp:UpdatePanel ID="upgvControlDieselLectura" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvControlDieselLectura" CssClass="gridview" OnPageIndexChanging="gvControlDieselLectura_OnpageIndexChanging" OnSorting="gvControlDieselLectura_Onsorting" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="false"
ShowFooter="True" TabIndex="17" PageSize="25" Width="100%">
<Columns>
<asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="false" />
<asp:BoundField DataField="UnidadLectura" HeaderText="Unidad" SortExpression="UnidadLectura" />
<asp:BoundField DataField="KmsLectura" HeaderText="Kms" SortExpression="KmsLectura" ItemStyle-HorizontalAlign="Right" />
<asp:BoundField DataField="LitrosLectura" HeaderText="Litros" SortExpression="LitrosLectura" ItemStyle-HorizontalAlign="Right"/>                                                    
</Columns>
<AlternatingRowStyle CssClass="gridviewrowalternate" />
<EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
<FooterStyle CssClass="gridviewfooter" />
<HeaderStyle CssClass="gridviewheader" />
<RowStyle CssClass="gridviewrow" />
<SelectedRowStyle CssClass="gridviewrowselected" />
<SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
<SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
</asp:GridView>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>

