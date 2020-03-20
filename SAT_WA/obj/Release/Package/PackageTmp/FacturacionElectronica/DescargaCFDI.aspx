<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DescargaCFDI.aspx.cs" Inherits="SAT.FacturacionElectronica.DescargaCFDI" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Descarga</title>
    <link href="../../HojasEstilo/CSS.css" rel="stylesheet" type="text/css" />
    <script type="text/JavaScript">
        function IsPopupBlocker() {
            var strNewURL = "Dummy.htm"
            var Strfeature = "";
            var WindowOpen = window.open
(strNewURL, "MainWindow", Strfeature);
            try {
                var obj = WindowOpen.name;
                WindowOpen.close();
            }
            catch (e) {
                alert("Su navegador No acepta Pop-Up. Por favor inactive su bloqueador de Pop-Up para poder ver esta página ");
            }
        }
        IsPopupBlocker();
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="forma">
            <div class="contenido_forma">
                <!-- Este panel aparece cuando el vinculo es correcto -->
                <asp:Panel ID="pnlVinculoCorrecto" runat="server" Visible="false">
                    <div>
                        <br />
                        <h1>GRACIAS POR USAR ESTE PORTAL DE DESCARGA.</h1>
                        <br />
                        Tus descargas restantes con este vinculo son:
                    <asp:Label ID="lblDescRest" runat="server" Text="Label" CssClass="Label"></asp:Label>
                        <br />
                        <br />
                        <br />
                    </div>
                    <asp:LinkButton ID="link_descarga" Text="Descarga tu archivo" runat="server" CssClass="LinkButton" OnClick="link_descarga_Click" Visible="True"></asp:LinkButton>
                </asp:Panel>
                <!-- Este panel aparecera en caso de un viculo invalido -->
                <asp:Panel ID="pnlVinculoIncorrecto" runat="server" Visible="False">
                    <div>
                        <h1>VINCULO INCORRECTO</h1>
                        <br />
                        <asp:Label ID="lblAviso" runat="server"></asp:Label>
                    </div>
                </asp:Panel>
            </div>
        </div>
    </form>
</body>
</html>
