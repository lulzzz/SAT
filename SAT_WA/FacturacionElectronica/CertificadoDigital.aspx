<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/MasterPage/MasterPage.Master" CodeBehind="CertificadoDigital.aspx.cs" Inherits="SAT.FacturacionElectronica.CertificadoDigital" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Estilos dcontroles -->
    <link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
    <link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
    <!-- Estilos Autocomplete, Mascara y Validadores JQuery -->
    <link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
    <!-- Bibliotecas para Validación de formulario -->
    <script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
    <script type="text/javascript">
        //Obteniendo instancia actual de la página y añadiendo manejador de evento
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        //Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
        function EndRequestHandler(sender, args) {
            if (args.get_error() == undefined) {
                ConfiguraCertificadoDigital();
            }
        }
        //Declarando Función de COnfiguración
        function ConfiguraCertificadoDigital() {
            $(document).ready(function () {    
                //Validación del Boton Aceptar
                $("#<%=btnAceptarContrasenaRevocacion.ClientID%>").click(function () {    
                    //Añadiendo Validación de Controles
                    var isValid1 = !$("#<%=txtContrasenaRevocacion.ClientID%>").validationEngine('validate');
                    var isValid2 = !$("#<%=txtConfirmarContrasenaRevocacion.ClientID%>").validationEngine('validate');    
                    //Devolviendo Resultado Obtenido
                    return isValid1 && isValid2
                });    
                //Validando que se cumplan las condiciones
                var validaCertificado = function () {    
                    //Removiendo Clases de los Controles
                    $("#<%=txtNuevaContrasena.ClientID%>").removeClass();
                    $("#<%=txtConfirmarNuevaContrasena.ClientID%>").removeClass();
                    $("#<%=txtNuevaContrasenaRevocacion.ClientID%>").removeClass();
                    $("#<%=txtConfirmarNuevaContrasenaRevocacion.ClientID%>").removeClass();
    
                    //Asignando Estilos
                    $("#<%=txtNuevaContrasena.ClientID%>").addClass('textbox validate[required, equals[<%=txtConfirmarNuevaContrasena.ClientID%>]]');
                    $("#<%=txtConfirmarNuevaContrasena.ClientID%>").addClass('textbox validate[required, equals[<%=txtNuevaContrasena.ClientID%>]]');
                    $("#<%=txtNuevaContrasenaRevocacion.ClientID%>").addClass('textbox validate[required, equals[<%=txtConfirmarNuevaContrasenaRevocacion.ClientID%>]]');
                $("#<%=txtConfirmarNuevaContrasenaRevocacion.ClientID%>").addClass('textbox validate[required, equals[<%=txtNuevaContrasenaRevocacion.ClientID%>]]');

                //Añadiendo Validación de Controles
                var isValid1 = !$("#<%=txtNuevaContrasena.ClientID%>").validationEngine('validate');
                var isValid2 = !$("#<%=txtConfirmarNuevaContrasena.ClientID%>").validationEngine('validate');
                var isValid3 = !$("#<%=txtNuevaContrasenaRevocacion.ClientID%>").validationEngine('validate');
                var isValid4 = !$("#<%=txtConfirmarNuevaContrasenaRevocacion.ClientID%>").validationEngine('validate');

                //Devolviendo Resultado Obtenido
                return isValid1 && isValid2 && isValid3 && isValid4
            }

            //Añadiendo Funcion al Evento Click del Boton "Aceptar"
            $("#<%=btnAceptar.ClientID%>").click(validaCertificado);
            //Añadiendo Funcion al Evento Click del Menu "Guardar"
            $("#<%=lkbGuardar.ClientID%>").click(validaCertificado);
        });
    }

    //Invocando Función de Configuración
    ConfiguraCertificadoDigital();
</script>
<div id="encabezado_forma">
        <h1>Certificado Digital</h1>
    </div>
<nav id="menuForma">
    <ul>
        <li class="green">
            <a href="#" class="fa fa-floppy-o"></a>
            <ul>
                <li>
                    <asp:LinkButton ID="lkbNuevo" runat="server" Text="Nuevo" OnClick="lkbElementoMenu_Click" CommandName="Nuevo" />
                </li>
                <li>
                    <asp:LinkButton ID="lkbAbrir" runat="server" Text="Abrir" OnClick="lkbElementoMenu_Click" CommandName="Abrir" />
                </li>
                <li> 
                    <asp:UpdatePanel ID="uplkbGuardar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:LinkButton ID="lkbGuardar" runat="server" Text="Guardar" OnClick="lkbElementoMenu_Click" CommandName="Guardar" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnAceptar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </li>
            </ul>
        </li>


        <li class="red">
            <a href="#" class="fa fa-pencil-square-o"></a>
            <ul>
                <li>
                    <asp:UpdatePanel ID="uplkbEliminar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:LinkButton ID="lkbEliminar" runat="server" Text="Eliminar" OnClick="lkbElementoMenu_Click" CommandName="Eliminar" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnAceptar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </li>
                <li>
                    <asp:UpdatePanel ID="uplkbRevocar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:LinkButton ID="lkbRevocar" runat="server" Text="Revocar" OnClick="lkbElementoMenu_Click" CommandName="Revocar" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnAceptar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </li>
            </ul>
        </li>
        <li class="blue">
            <a href="#" class="fa fa-cog"></a>
            <ul>
                <li>
                    <asp:UpdatePanel ID="uplbkBitacora" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:LinkButton ID="lkbBitacora" runat="server" Text="Bitácora" OnClick="lkbElementoMenu_Click" CommandName="Bitacora" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="lkbBitacora" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </li>
                <li>
                    <asp:UpdatePanel ID="uplkbReferencia" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:LinkButton ID="lkbReferencias" runat="server" Text="Referencias" OnClick="lkbElementoMenu_Click" CommandName="Referencia" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="lkbReferencias" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </li>
            </ul>
        </li>
<li class="yellow">
<a href="#" class="fa fa-question-circle"></a>
<ul>
<li>
<asp:LinkButton ID="lkbAcercaDe" runat="server" Text="Acerca de" OnClick="lkbElementoMenu_Click" CommandName="Acerca" /></li>
<li>
<asp:LinkButton ID="lkbAyuda" runat="server" Text="Ayuda" OnClick="lkbElementoMenu_Click" CommandName="Ayuda" /></li>
</ul>
</li>
</ul>
</nav>




<div class="seccion_controles">
<div class="header_seccion">
<h2>Datos de Certificado Digital</h2>
</div>
<div class="columna2x">
<div class="renglo2x">
<div class="etiqueta">
<label class="Label" for="lblID">ID</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uplblID" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label runat="server" ID="lblID" CssClass="label">ID</asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarContrasenaRevocacion" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="ddlEstatus">
Estatus
</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlEstatus" runat="server">
<ContentTemplate>
<asp:DropDownList ID="ddlEstatus" runat="server"  TabIndex ="1" CssClass="dropdown2x"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarContrasenaRevocacion" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
</Triggers>
</asp:UpdatePanel></div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="ddlEmisor">
Emisor
</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtIdAlterno" runat="server">
<ContentTemplate>
<asp:DropDownList ID="ddlEmisor" runat="server"  CssClass="dropdown2x"  TabIndex="2"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarContrasenaRevocacion" />
</Triggers>
</asp:UpdatePanel></div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="ddlSucursal">
Sucursal
</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlSucursal" runat="server">
<ContentTemplate>
<asp:DropDownList ID="ddlSucursal" runat="server" TabIndex="3"  CssClass="dropdown2x"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarContrasenaRevocacion" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
</Triggers>
</asp:UpdatePanel></div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="ddlTipo">
Tipo
</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upTipo" runat="server">
<ContentTemplate>
<asp:DropDownList ID="ddlTipo" runat="server"  TabIndex="4" CssClass="dropdown2x"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarContrasenaRevocacion" />
</Triggers>
</asp:UpdatePanel></div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="ArchivoCer">
Archivo .cer
</label></div>
<div class="control2x">
<asp:FileUpload ID="fuArchivoCer" runat="server"    TabIndex="3" CssClass="textbox2x"  ForeColor="Black" />
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="ArchivoCer">
Archivo .key
</label></div>
<div class="control2x">
<asp:FileUpload ID="fuArchivoKey"   runat="server" TabIndex="3" CssClass="textbox2x" ForeColor="Black" />
</div>
</div>
<div class="renglon2x">
<div class="control2x">
<asp:Button ID="btnCargarArchivos"  OnClick="btnCargarArchivos_Click" runat="server"  Text="Cargar Archivos" />
</div>
</div>
</div>
<div class  ="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="txtNuevaContrasena">
 Contraseña
</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtNuevaContrasena" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtNuevaContrasena" runat="server"  CssClass="textbox validate[required]"
TextMode="Password" MaxLength="200" TabIndex="7"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarContrasenaRevocacion" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="txtConfirmarNuevaContrasena">
Confirmar Contraseña
</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtConfirmarNuevaContrasena" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtConfirmarNuevaContrasena" runat="server" 
 TextMode="Password" MaxLength="200" CssClass="textbox validate[required]" TabIndex="8"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarContrasenaRevocacion" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="txtNuevaContrasenaRevocacion">
 Contraseña Revocación
</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtNuevaContrasenaRevocacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtNuevaContrasenaRevocacion" runat="server"   CssClass="textbox validate[required]"
TextMode="Password" MaxLength="200" TabIndex="9"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptarContrasenaRevocacion" />
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="txtConfirmarNuevaContrasenaRevocacion">
Confirmar Contraseña Revocación
</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtConfirmarNuevaContrasenaRevocacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtConfirmarNuevaContrasenaRevocacion" runat="server"  CssClass="textbox validate[required]"
 TextMode="Password" MaxLength="200" TabIndex="10"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarContrasenaRevocacion" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button runat="server" ID="btnAceptar" CssClass="boton" Text="Aceptar" OnClick="btnAceptar_Click"
TabIndex="11" ValidationGroup="Contrasena" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarContrasenaRevocacion" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo"  />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<asp:UpdatePanel ID="uplblErrorP" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label runat="server" ID="lblError"  CssClass="label_error"></asp:Label></ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
<div id="contenidoConfirmacionContrasenaRevocacion" class="modal">
<div id="confirmacionContrasenaRevocacion"" class="contenedor_ventana_confirmacion"> 
<div  style="text-align:right">
<asp:UpdatePanel runat="server" ID="uplkbCerrarContrasenaRevocacion" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarContrasenaRevocacion" runat="server" Text="Cerrar" OnClick="lkbCerrarContrasenaRevocacion_Click" >
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>               
<h3>Revocación Certificado Digital</h3>
<div class="columna"> 
<div class="renglon2x"></div>
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="txtContrasenaRevocacion">
Contraseña
</label>
</div>
<div class="control" style="width: auto">
<asp:UpdatePanel ID="uptxtContrasenaRevocacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtContrasenaRevocacion" CssClass="textbox validate[required, equals[ctl00_content1_txtConfirmarContrasenaRevocacion]]" runat="server" TextMode="Password"  TabIndex="2"> </asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbCerrarContrasenaRevocacion" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarContrasenaRevocacion" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="txtContrasenaRevocacion">
Confirmar Contraseña
</label>
</div>
<div class="control" style="width: auto">
<asp:UpdatePanel ID="uptxtConfirmarContrasenaRevocacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtConfirmarContrasenaRevocacion"   CssClass="textbox validate[required, equals[ctl00_content1_txtContrasenaRevocacion]]" TextMode="Password" runat="server"  TabIndex="2" ></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbCerrarContrasenaRevocacion" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarContrasenaRevocacion" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptarContrasenaRevocacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptarContrasenaRevocacion" runat="server" OnClick="btnAceptarContrasenaRevocacion_Click"    CssClass ="boton"  Text="Aceptar"  />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>  
<div class="renglon2x">
<div class="control" style="width: auto">
<asp:UpdatePanel ID="uplblErrorContrasenaRevocacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblErrorContrasenaRevocacion" runat="server"  CssClass="label_error"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbCerrarContrasenaRevocacion" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarContrasenaRevocacion" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>            
</div>
</div>
</asp:Content>
