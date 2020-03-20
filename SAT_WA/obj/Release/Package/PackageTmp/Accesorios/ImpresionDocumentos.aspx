<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImpresionDocumentos.aspx.cs" Inherits="SAT.Accesorios.ImpresionDocumentos" %>
<%@ Register Src="~/UserControls/wucImpresionDocumentos.ascx" TagName="wucImpresionDocumentos" TagPrefix="tectos" %>
<!DOCTYPE html> 

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
<!-- Estilos de los Controles -->
<link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
<link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
<link href="../CSS/ControlesUsuario.css" type="text/css" rel="stylesheet" />
<link href="../CSS/MenuPrincipal.css" rel="stylesheet" />
<link href="../CSS/MenuUsuario.css" rel="stylesheet" />
<!-- Estilos de Validación, DateTimePicker, MasketTextBox -->
<link href="../CSS/jquery.validationEngine.css" type="text/css" rel="stylesheet" />
<link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
<link href="../CSS/animate.css" rel="stylesheet" type="text/css" />
<link href="../CSS/jquery-ui.css" rel="stylesheet" type="text/css" />
<link href="../CSS/jquery.multiselect.css" rel="stylesheet" type="text/css" />
<!-- Libreiras de Validación, DateTimePicker, MasketTextBox -->
<script src="../Scripts/jquery-1.7.1.min.js" type="text/javascript" charset="utf-8"></script>
<script src="../Scripts/jquery-ui.js" type="text/javascript" charset="utf-8"></script>
<script src="../Scripts/jquery-ui.min.js" type="text/javascript" charset="utf-8"></script>
<script src="../Scripts/jquery.validationEngine.js" type="text/javascript" charset="utf-8"></script>
<script src="../Scripts/jquery.validationEngine-es.js" type="text/javascript" charset="utf-8"></script>
<script src="../Scripts/jquery.datetimepicker.js" type="text/javascript" charset="utf-8"></script>
<script src="../Scripts/jquery.noty.packaged.js" type="text/javascript" charset="utf-8"></script>
<script src="../Scripts/jquery.noty.packaged.min.js" type="text/javascript" charset="utf-8"></script>
<script src="../Scripts/gridviewScroll.min.js" type="text/javascript" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.multiselect.js"></script>
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
            <asp:UpdatePanel ID="upucImpresionDocumentos" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <tectos:wucImpresionDocumentos ID="ucImpresionDocumentos" runat="server" Enabled="true" TabIndex="1" />
                </ContentTemplate>
                <Triggers>
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>
