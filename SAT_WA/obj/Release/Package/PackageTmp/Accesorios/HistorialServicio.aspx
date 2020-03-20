<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HistorialServicio.aspx.cs" Inherits="SAT.Accesorios.HistorialServicio" %>
<%@ Register Src="~/UserControls/wucHistorialViajes.ascx" TagName="wucHistorialViajes" TagPrefix="tectos" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
<title></title>
<!-- Estilos -->
<link href="../CSS/Forma.css" rel="stylesheet" />
<link href="../CSS/Controles.css" rel="stylesheet" type="text/css" />
<link href="../CSS/ControlesUsuario.css" rel="stylesheet" type="text/css" />
<link href="../CSS/Operacion.css" rel="stylesheet" />
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
<link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
<link href="../CSS/jquery-ui.css" rel="stylesheet" type="text/css" />
<!-- Libreiras de Validación, DateTimePicker, MasketTextBox -->
<script src="../Scripts/jquery-1.7.1.min.js" type="text/javascript" charset="utf-8"></script>
<script src="../Scripts/jquery-ui.js" type="text/javascript" charset="utf-8"></script>
<script src="../Scripts/jquery-ui.min.js" type="text/javascript" charset="utf-8"></script>
<script src="../Scripts/jquery.validationEngine.js" type="text/javascript" charset="utf-8"></script>
<script src="../Scripts/jquery.validationEngine-es.js" type="text/javascript" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
<script src='<%=ResolveUrl("~/Scripts/jquery.blockUI.js") %>' type="text/javascript"></script>
</head>
<body>
<form id="form1" runat="server">
<asp:ScriptManager ID="sc" runat="server"></asp:ScriptManager>
<script>
Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(Loading);
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(Loaded);

function Loading() {
$.blockUI({ message: '<h2><img src="../Image/loading2.gif" /> Espere por favor...</h2>', fadeIn: 200 });
}
function Loaded() {
$.unblockUI({ fadeOut: 200 });
}

</script>
<div>
<asp:UpdatePanel ID="upwucHistorialViajes" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<tectos:wucHistorialViajes ID="wucHistorialViajes" runat="server" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</form>
</body>
</html>
