<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Reporte.aspx.cs" Inherits="SAT.RDLC.Reporte" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=14.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../CSS/Forma.css" rel="stylesheet" />
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="smReporte">
    </asp:ScriptManager>
    <rsweb:ReportViewer ID="rvReporte" runat="server" Height="900px" Width="1000px" ShowPrintButton="true" ShowToolBar="true"></rsweb:ReportViewer>
    
    </form>
</body>
</html>
