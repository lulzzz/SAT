<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportesClientes.aspx.cs" Inherits="SAT.Reportes.ReportesClientes" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="../CSS/PaginaMaestra.css" rel="stylesheet" />
    <link href="../CSS/MenuPrincipal.css" rel="stylesheet" />
    <link href="../CSS/MenuUsuario.css" rel="stylesheet" />
    <link href="../CSS/Forma.css" rel="stylesheet" />
    <link href="../CSS/Controles.css" rel="stylesheet" />
    <!-- Estilos de Scripts -->
    <link href="../CSS/jquery-ui.css" rel="stylesheet" />
    <link href="../CSS/jquery-ui.min.css" rel="stylesheet" />
    <link href="../CSS/jquery-ui.structure.css" rel="stylesheet" />
    <link href="../CSS/jquery-ui.structure.min.css" rel="stylesheet" />
    <link href="../CSS/jquery-ui.theme.css" rel="stylesheet" />
    <link href="../CSS/jquery-ui.theme.min.css" rel="stylesheet" />

    <link href="https://fonts.googleapis.com/css?family=Lato|Montserrat|Open+Sans|Quicksand|Roboto" rel="stylesheet" />

    <!-- Animaciones de entrada y salida de elementos -->
    <link href="../CSS/animate.css" rel="stylesheet" type="text/css" />
    <link href="//maxcdn.bootstrapcdn.com/font-awesome/4.2.0/css/font-awesome.min.css" rel="stylesheet" />
    <!-- Habilitación para uso de jquery en formas ligadas a esta master page -->
    <script src='<%=ResolveUrl("~/Scripts/jquery-1.7.1.js") %>' type="text/javascript"></script>
    <script src='<%=ResolveUrl("~/Scripts/jquery-1.7.1.min.js") %>' type="text/javascript"></script>

    <title>Reportes de Clientes</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
        <div>
        </div>
    </form>
</body>
</html>
