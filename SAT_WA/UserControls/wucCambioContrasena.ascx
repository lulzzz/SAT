<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucCambioContrasena.ascx.cs" Inherits="SAT.UserControls.wucCambioContrasena" %>
<link href="../CSS/Forma.css" rel="stylesheet" />
<link href="../CSS/Controles.css" rel="stylesheet" />
<link href="../CSS/ControlesUsuario.css" type="text/css" rel="stylesheet" />
<!-- Estilos Autocomplete, Mascara y Validadores JQuery -->
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
<!-- Bibliotecas para Validación de formulario -->
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
<script type="text/javascript">
    //Obteniendo instancia actual de la página y añadiendo manejador de evento
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    //Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
    function EndRequestHandler(sender, args) {
        if (args.get_error() == undefined) {
            ConfiguraCambioContrasena();
        }
    }

    //Declarando Función de COnfiguración
    function ConfiguraCambioContrasena() {
        $(document).ready(function () {

            //Validación del Boton Aceptar
            $("#<%=btnAceptar.ClientID%>").click(function () {

                //Añadiendo Validación de Controles
                var isValid1 = !$("#<%=txtClave.ClientID%>").validationEngine('validate');
                var isValid2 = !$("#<%=txtClaveNueva.ClientID%>").validationEngine('validate');
                var isValid3 = !$("#<%=txtClaveNueva2.ClientID%>").validationEngine('validate');

                //Devolviendo Resultado Obtenido
                return isValid1 && isValid2 && isValid3;
            });

            //Password Strength
            $("#<%=txtClaveNueva.ClientID%>").on('keyup', function () {
                
                //Declarando Variables Auxiliares
                var cont = 0;
                var password = $("#<%=txtClaveNueva.ClientID%>");
                var estatus, colorFondo;

                //Validando que sean mas de 6 Caracteres
                if (password.val().length > 6) {
                    //Incrementando Contador
                    cont++;
                }
                //Validando que sean mas de 8 Caracteres
                if (password.val().length > 8) {
                    //Incrementando Contador
                    cont++;
                }

                //Validando que Contenga Mayusculas y Minusculas
                if (/(?=.*[A-Z])(?=.*[a-z])/.test(password.val())) {
                    cont++;
                }

                //Validando que Contenga Digitos
                if (/(?=.*[0-9])/.test(password.val())) {
                    cont++;
                }

                //Validando que Contenga Caracteres Especiales
                if (/@|\$|\!|&|\^/.test(password.val())) {
                    cont++;
                }

                //Calculando Tamaño y Porcentaje del Indicador
                var tamano = (100 - ((cont * 2) * 10));
                var porcentaje = (100 - tamano);

                //Validando Estilos y Nivel de Fortaleza
                switch (cont) {
                    case 0:
                    case 1:
                        estatus = "Debil";
                        colorFondo = "#8B4513";
                        break;
                    case 2:
                    case 3:
                        estatus = "Media";
                        colorFondo = "#FFA500";
                        break;
                    case 4:
                        estatus = "Fuerte";
                        colorFondo = "#0000FF";
                        break;
                    case 5:
                        estatus = "Excelente";
                        colorFondo = "#008000";
                        break;
                }

                //Asignando Estilos a los Indicadores
                $('.password-strength .inner').css('right', tamano + "%").css('background-color', colorFondo);
                $('.password-strength .text').html(estatus + " (" + porcentaje + "%)");
            });

        });
    }

    //Invocando Función de Configuración
    ConfiguraCambioContrasena();
</script>
<div class="contenedor_media_seccion">
    <div class="header_seccion">
        <img src="../Image/Seguridad.png" />
        <h2>Actualización de Contraseña</h2>
    </div>
    <div class="columna2x">
        <div class="renglon2x">
            <div class="etiqueta">
                <label for="txtClave">Contraseña</label>
            </div>
            <div class="control2x">
                <asp:UpdatePanel ID="uptxtClave" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TextBox ID="txtClave" runat="server" CssClass="textbox validate[required]" TextMode="Password" MaxLength="30"></asp:TextBox>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon2x">
            <div class="etiqueta">
                <label for="txtClaveNueva">Nueva Contraseña</label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="uptxtClaveNueva" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TextBox ID="txtClaveNueva" runat="server" CssClass="textbox validate[required, equals[ctl00_ucCambioContrasena_txtClaveNueva2]]" TextMode="Password" MaxLength="30"></asp:TextBox>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta">
                <div class="password-strength">
                    <div class="inner"></div>
                    <div class="text"></div>
                </div>
            </div>
        </div>
        <div class="renglon2x">
            <div class="etiqueta">
                <label for="txtClaveNueva2">Confirmar</label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="uptxtClaveNueva2" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TextBox ID="txtClaveNueva2" runat="server" CssClass="textbox validate[required, equals[ctl00_ucCambioContrasena_txtClaveNueva]]" TextMode="Password" MaxLength="30"></asp:TextBox>
                        
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon2x">
            <div class="controlBoton">
                <asp:UpdatePanel ID="upbtnAceptar" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" OnClick="btnAceptar_Click" CssClass="boton" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="controlBoton">
                <asp:UpdatePanel ID="upbtnCancelar" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click" CssClass="boton_cancelar" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon2x">
            <asp:UpdatePanel ID="uplblError" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Label ID="lblError" runat="server" CssClass="label_error"></asp:Label>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</div>