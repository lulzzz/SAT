<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UbicacionExternaMapa.aspx.cs" Inherits="SAT.Maps.UbicacionExternaMapa" %>
<%@ Register Src="../UserControls/wucMapaUbicacion.ascx" TagName="wucMapaUbicacion" TagPrefix="tectos" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Mapa</title>
</head>
<body>
    <form id="form1" runat="server">
        <tectos:wucMapaUbicacion ID="ucMapaUbicacionExterna" runat="server" />
    </form>
</body>
</html>
