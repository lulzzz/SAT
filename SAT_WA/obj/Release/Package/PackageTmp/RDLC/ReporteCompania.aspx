<%@ Page Title="Reportes e Indicadores" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="ReporteCompania.aspx.cs" Inherits="SAT.RDLC.ReporteCompania" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=14.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<!-- Estilos documentación de servicio -->
<link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
<link href="../CSS/ControlesUsuario.css" type="text/css" rel="stylesheet" />
<link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
<div class="seccion_controles">
<div class="header_seccion">
<img src="../Image/TablaResultado.png" />
<h2>Visor de Reportes e Indicadores</h2>
</div>
<div class="columna2x">
<div class="renglon2x">
</div>
<div class="renglon2x">
<div class="etiqueta_50px">
<label for="ddlReporte">Reporte</label>
</div>
<div class="control2x">
<asp:DropDownList ID="ddlReporte" runat="server" CssClass="dropdown2x" AutoPostBack="true" OnSelectedIndexChanged="ddlReporte_SelectedIndexChanged"></asp:DropDownList>
</div>
</div>
</div>
</div>
<div class="grid_seccion_completa_400px_altura">
<asp:UpdatePanel ID="uprvSSRS" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<rsweb:ReportViewer ID="rvSSRS" runat="server" Width="100%" Font-Names="Verdana" Font-Size="8pt" ProcessingMode="Remote" 
WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" AsyncRendering="true" ExportContentDisposition="OnlyHtmlInline">
</rsweb:ReportViewer>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlReporte" />
</Triggers>
</asp:UpdatePanel>
</div>
</asp:Content>
