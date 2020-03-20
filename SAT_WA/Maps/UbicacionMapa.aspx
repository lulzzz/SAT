<%@ Page Title="Mapa de Ubicación" Language="C#" AutoEventWireup="true" CodeBehind="UbicacionMapa.aspx.cs" Inherits="SAT.Maps.UbicacionMapa" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Mapa</title>
    <style>
        html, body, form, #mapa {
            height: 100%;
            margin: 0px;
            padding: 0px;
        }
    </style>
    <script src="https://maps.googleapis.com/maps/api/js?v=3.exp&key=AIzaSyBH8LH8WQEO7Y9BqT62mCWWV0WPYKQnaCY"></script>
    

</head>
<body>
    <form id="Form1">
        <asp:Literal ID="ltr" runat="server" />
        <div id="mapa">
        </div>
    </form>
</body>
</html>
