<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReferenciaRegistro.aspx.cs" Inherits="SAT.Accesorios.ReferenciaRegistro" %>
<%@ Register Src="~/UserControls/wucReferenciaRegistro.ascx" TagName="WucReferencia" TagPrefix="tectos" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
<!-- Estilos de los Controles -->
<link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
<link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
<link href="../CSS/ControlesUsuario.css" type="text/css" rel="stylesheet" />
<!-- Estilos de Validación, DateTimePicker, MasketTextBox -->
<link href="../CSS/jquery.validationEngine.css" type="text/css" rel="stylesheet" />
<!-- Libreiras de Validación, DateTimePicker, MasketTextBox -->
<script src="../Scripts/jquery-1.7.1.min.js" type="text/javascript" charset="utf-8"></script>
<script src="../Scripts/jquery.validationEngine.js" type="text/javascript" charset="utf-8"></script>
<script src="../Scripts/jquery.validationEngine-es.js" type="text/javascript" charset="utf-8"></script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="sc" runat="server"></asp:ScriptManager>
    <div>
    <tectos:WucReferencia ID="ucReferencias" runat="server"></tectos:WucReferencia>
    </div>
    </form>
</body>
</html>
