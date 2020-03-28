<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportesClientes.aspx.cs" Inherits="SAT.Reportes.ReportesClientes" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=14.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <!--Import Google Icon Font-->
    <link href="https://fonts.googleapis.com/icon?family=Material+Icons" rel="stylesheet"/>
    <!--Import materialize.css-->
    <link href="../Materialize/css/materialize.min.css" rel="stylesheet"/>
    <!--Import style.css-->
    <link href="../Materialize/css/style.css" rel="stylesheet" />
    <title>ReporteClientes</title>
</head>
<body>
    <form id="form1" runat="server">
        <!--Valida las inserciones de datos en los controles-->
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <%--Header--%>
        <nav class="headercolor" role="navigation">
            <div class="nav-wrapper">
                <a href="#!" class="brand-logo right"><i class="material-icons">date_range</i></a>
                <a href="#" data-activates="mobile-demo" class="button-collapse"><i class="material-icons">menu</i></a>
            </div>
        </nav>
        <div class="container">
            <!-- espacios con row -->
            <div class="row">
                <div class="col s12 m4">
                </div>
            </div>
            <div class="row">
                <div class="col s12  m4"></div>
            </div>
            <div class="content">
                <%--Subencabezado--%>
                <h5>Visor de Reportes e Indicadores<img src="../Image/TablaResultado.png" alt="" /></h5>
                <div class="divider" style="margin-top: 25px"></div>
                <%--Botones Principales--%>
                <%--Controles Entrada Salida--%>
                <%--Fila 1--%>
                <div class="row">
                    <%--Columna 1--%>
                    <div class="col s12 m4">
                        <label>Reporte</label>
                        <asp:UpdatePanel runat="server" ID="upddlReporte" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList runat="server" ID="ddlReporte" class="browser-default" TabIndex="1" AutoPostBack="true" OnSelectedIndexChanged="ddlReporte_SelectedIndexChanged"></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                 <%--Fila 2--%>
                <div class="row">

                    <div class="grid_seccion_completa_400px_altura">
                        <asp:UpdatePanel ID="uprvSSRS" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <rsweb:ReportViewer ID="rvSSRS" runat="server" Width="100%" Height="50%" Font-Names="Verdana" Font-Size="8pt" ProcessingMode="Remote"
                                    waitmessagefont-names="Verdana" waitmessagefont-size="14pt" AsyncRendering="true" ExportContentDisposition="OnlyHtmlInline"  Visible="false">
                                </rsweb:ReportViewer>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlReporte" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>

                    <asp:UpdatePanel ID="uprvPBI" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="rvPBI" runat="server" CssClass="label_negrita" Text="POWER BI" Visible="false" ></asp:Label > 
                        </ContentTemplate>
                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlReporte" />
                        </Triggers>
                    </asp:UpdatePanel>

                    <asp:UpdatePanel ID="uprvAND" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                           <asp:Label ID="rvAND" runat="server" CssClass="label_negrita" Text="ANALITYESDATA" Visible="false"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlReporte" />
                        </Triggers>
                    </asp:UpdatePanel>
                <%--Columna 1--%>
                    <div class="col s12 m12">                        

                    </div>
                </div>
            </div>
            <!-- espacios con row -->
            <div class="row">
                <div class="col s12 m4">
                </div>
            </div>
        </div>

        <%--Pie de pagina--%>
        <footer class="page-footer footercolor">
            <div class="container">
            </div>
            <div class="footer-copyright">
                <div class="container">
                    This site is powered by ARI TECTOS S.A. de C.V. | © 2017 TECTOS. All rights reserved
                </div>
            </div>
        </footer>
        <!--Import jQuery before materialize.js-->
        <script src="../Materialize/js/jquery.js"></script>
        <link href="../CSS/jquery.validationEngine.css" type="text/css" rel="stylesheet" />
        <!--Script que valida el contenido de los controles-->
        <script type="text/javascript" src="../Scripts/jquery.validationEngine.js"></script>
        <script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js"></script>
        <script src="../Materialize/js/materialize.min.js"></script>
        <!-- Notificaciones emergentes -->
        <!-- Notificaciones emergentes -->
        <script type="text/javascript" src='<%=ResolveUrl("~/Scripts/jquery.noty.packaged.js") %>'></script>
        <script type="text/javascript" src='<%=ResolveUrl("~/Scripts/jquery.noty.packaged.min.js") %>'></script>
        <script type="text/javascript" src='<%=ResolveUrl("~/Scripts/jquery.blockUI.js") %>'></script>
        <!-- Fechas -->
        <link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
        <!-- Biblioteca para uso de datetime picker -->
        <script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
        <!-- Animaciones de entrada y salida de elementos -->
        <link href="../CSS/animate.css" rel="stylesheet" type="text/css" />
        <!--Valida las inserciones de datos en los controles-->
        <script type="text/javascript">
            //Obtiene la instancia actual de la pagina y añade un manejador de eventos
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
            //Creación de la función que permite finalizar o validar los controles a partir de un error.
        </script>
    </form>
</body>
</html>
