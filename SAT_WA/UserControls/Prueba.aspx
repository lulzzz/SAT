<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="true" CodeBehind="Prueba.aspx.cs" MasterPageFile="~/MasterPage/MasterPage.Master" Inherits="SAT.UserControls.Prueba" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Estilos documentación de servicio -->
<link href="../CSS/DocumentacionServicio.css" rel="stylesheet" />
<link href="../CSS/ControlesUsuario.css" rel="stylesheet" />
<!-- Estilos Autocomplete, Mascara y Validadores JQuery -->
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
<link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
<!-- Bibliotecas para uso de autocomplete en controles de búsqueda filtrada -->
<!-- Bibliotecas para Validación de formulario -->
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
<!-- Biblioteca para uso de datetime picker -->
<script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
<!-- Biblioteca para ventana modal  -->
<script type="text/javascript" src="../Scripts/jquery.plainmodal.min.js" charset="utf-8"></script>
<link href="../CSS/jquery.jqzoom.css" rel="stylesheet" type="text/css" />
<script src="../Scripts/jquery.jqzoom-core.js" type="text/javascript"></script>
<!-- Estilos -->
    <%@ Register   Src="~/UserControls/wucRuta.ascx" TagPrefix="uc1" TagName="wuPublicacion" %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
<uc1:wuPublicacion ID="wucPublicacionUnidad" Contenedor="#ContenedorPublicacion"  runat="server" />

</asp:Content>