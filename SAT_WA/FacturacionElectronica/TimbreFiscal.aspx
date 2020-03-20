<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TimbreFiscal.aspx.cs" Inherits="SAT.FacturacionElectronica.TimbreFiscal" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
  <title></title>
<link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
<link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
<link href="../CSS/ControlesUsuario.css" type="text/css" rel="stylesheet" />
</head>
<body>
<form id="form1"  runat="server">
<asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
<div id="encabezado_forma">
<h1>Timbre Fiscal</h1>
</div>
<div class="seccion_controles">
<div class="header_seccion">
<h2>Detalles del Timbre Fiscal</h2>
</div>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta_155px">
<label class="Label" for="lblID">
Comprobante: 
</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uplblID" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblID" runat="server" TabIndex="3"  CssClass="label" AutoPostBack="true">ID</asp:Label>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta_155px">
<label class="Label" for="txtVersion">
Versión
</label>
</div>
<div class="control2">
<asp:UpdatePanel ID="uptxtVersion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtVersion" runat="server" TabIndex="3"  CssClass="textbox" Enabled="false" AutoPostBack="true">
</asp:TextBox>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta_155px">
<label class="Label" for="txtFecTim">
Fecha de Timbrado
</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtFecTim" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFecTim" runat="server" TabIndex="3" CssClass="textbox2x" Enabled="false" AutoPostBack="true">
</asp:TextBox>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta_155px">
<label class="Label" for="txtUUID">
Folio UUID
</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtUUID" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtUUID" runat="server" TabIndex="3"  CssClass="textbox2x" Enabled="false" AutoPostBack="true">
</asp:TextBox>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta_155px">
<label class="Label" for="txtSelloCFD">
Sello CFD
</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtSelloCFD" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtSelloCFD" runat="server" TabIndex="3" 
CssClass="textbox2x" Enabled="false" AutoPostBack="true" 
TextMode="MultiLine"></asp:TextBox>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x"></div>
<div class="renglon2x">
<div class="etiqueta_155px">
<label class="Label" for="txtNoCer">
No. Certificado
</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtNoCer" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtNoCer" runat="server" TabIndex="3"  CssClass="textbox2x" Enabled="false" AutoPostBack="true">
</asp:TextBox>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x"></div>
<div class="renglon2x">
<div class="etiqueta_155px">
<label class="Label" for="txtSelloSAT">
Sello SAT
</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtSelloSAT" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtSelloSAT" runat="server" TabIndex="3" 
CssClass="textbox2x" Enabled="false" AutoPostBack="true" 
TextMode="MultiLine"></asp:TextBox>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div  class="control2x">
<asp:UpdatePanel ID="uplblError" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label runat="server" ID="lblError"  CssClass="label_error"></asp:Label></ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
</form>
</body>
</html>
