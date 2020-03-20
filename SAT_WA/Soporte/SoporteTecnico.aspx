<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="SoporteTecnico.aspx.cs" Inherits="SAT.Soporte.SoporteTecnico" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Estilos documentación de servicio -->
<link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
<link href="../CSS/ControlesUsuario.css" type="text/css" rel="stylesheet" />
<link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
<!-- Estilos Autocomplete, Mascara y Validadores JQuery -->
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
<link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
<link href="../CSS/GridviewScroll.css" rel="stylesheet" type="text/css" />
<!-- Bibliotecas para Validación de formulario --> 
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
<!-- Biblioteca para uso de datetime picker -->
<script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
    <!--Biblioteca para fijar encabeados GridView-->
<script type="text/javascript" src="../Scripts/gridviewScroll.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
    <script type="text/javascript">
        //Obteniendo instancia actual de la página y añadiendo manejador de evento
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        //Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
        function EndRequestHandler(sender, args) {
            if (args.get_error() == undefined) {
                ConfiguraJQuerySoporteTecnico();
            }
        }
        //Creando función para configuración de jquery en control de usuario
        function ConfiguraJQuerySoporteTecnico() {
            $(document).ready(function () {
                //Función de validación de campos
                var validacionSoporteTecnico = function (evt) {
                    //Validando Controles
                    var isValidP1 = !$("#<%=txtNoConsecutivoCompania.ClientID%>").validationEngine('validate');
                    var isValidP2 = !$("#<%=txtIdCompaniaEmisora.ClientID%>").validationEngine('validate');
                    var isValidP3 = !$("#<%=txtIdEstatus.ClientID%>").validationEngine('validate');
                    var isValidP4 = !$("#<%=txtUsuarioSolicitante.ClientID%>").validationEngine('validate');
                    var isValidP5 = !$("#<%=txtIdUsuarioAsistente.ClientID%>").validationEngine('validate');
                    var isValidP6 = !$("#<%=txtFechaInicioGeneral.ClientID%>").validationEngine('validate');
                    var isValidP7 = !$("#<%=txtFechaTerminoGeneral.ClientID%>").validationEngine('validate');
                    var isValidP8 = !$("#<%=txtIdSoporte.ClientID%>").validationEngine('validate');
                    var isValidP9 = !$("#<%=txtObservacion.ClientID%>").validationEngine('validate');
                    var isValidP10 = !$("#<%=txtFechaInicio.ClientID%>").validationEngine('validate'); 
                    var isValidP11 = !$("#<%=txtFechaTermino.ClientID%>").validationEngine('validate');
                    var isValidP12 = !$("#<%=txtFechaTerminoDetalle.ClientID%>").validationEngine('validate');
                    var isValidP13 = !$("#<%=txtIdSoporteE.ClientID%>").validationEngine('validate');
                    var isValidP14 = !$("#<%=txtObservacionE.ClientID%>").validationEngine('validate');
                    var isValidP15 = !$("#<%=txtFechaInicioE.ClientID%>").validationEngine('validate');
                    var isValidP16;
                    return isValidP1 && isValidP2 && isValidP3 && isValidP4 && isValidP5 && isValidP6 && isValidP7 && isValidP8, isValidP9,
                    isValidP10, isValidP11, isValidP12, isValidP13, isValidP14, isValidP15;
                };

                
           
            });

            //Fecha de inicio "soporte tecnico "
            $("#<%=txtFechaInicioGeneral.ClientID%>").datetimepicker({
                lang: 'es',
                format: 'd/m/Y H:i'
            });
            //Fecha de termino"soporte tecnico"
            $("#<%=txtFechaTerminoGeneral.ClientID%>").datetimepicker({
                lang: 'es',
                format: 'd/m/Y H:i'
            });
            //Fecha de inicio modal agregar soporte tecnico detalle
            $("#<%=txtFechaInicio.ClientID%>").datetimepicker({
                lang: 'es',
                format: 'd/m/Y H:i'
            });
            //Fecha de termino modal terminar soporte
            $("#<%=txtFechaTermino.ClientID%>").datetimepicker({
                lang: 'es',
                format: 'd/m/Y H:i'
            });
            //Fecha de termino modal terminar pendienetes 
            $("#<%=txtFechaTerminoDetalle.ClientID%>").datetimepicker({
                lang: 'es',
                format: 'd/m/Y H:i'
            });

            //Fecha de inicio modal editar soporte tecnico detalle
            $("#<%=txtFechaInicioE.ClientID%>").datetimepicker({
                lang: 'es',
                format: 'd/m/Y H:i'
            });

         



            $(document).keyup(function (e) {
                if (e.keyCode == 27) { // escape key maps to keycode `27`
                    //Ocultando Menu
                    OcultarMenu();
                }
            });
            $(document).click(function (e) {

                //Ocultando Menu
                OcultarMenu();
            });

           
        }
        ConfiguraJQuerySoporteTecnico();

        //Función encargada de Mostrar el Ménu
        function MostrarMenu(control, e) {
            //Ocultando en caso de estar Abierto
            OcultarMenu();

            //Obteniendo Coordenadas de las Forma
            var posx = e.pageX + 'px';
            var posy = e.pageY + 'px';

            //Si el Evento es de Tipo Click
            if (e.type == 'click')

                //Detener Propagación del Evento
                e.stopPropagation();

            //Asignando Posiciones al Documento
            document.getElementById(control).style.position = 'absolute';
            document.getElementById(control).style.left = posx;
            document.getElementById(control).style.top = posy;


            //Ejecutando 
            $(document).ready(function (evt) {

                //Mostrando DIV
                $('#' + control).slideDown(100);
            });
        }
        //Función encargada de Ocultar el Ménu
        function OcultarMenu() {
            $(document).ready(function () {
                //Ocultando DIV
                $('.menuContainer').slideUp(100);
            });
        }
  
    </script>

<div id="encabezado_forma">
<h1>Soporte Técnico</h1>
</div>
<!--Menu Principal-->
    <asp:UpdatePanel ID="upMenuPrincipal" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <nav id="menuForma">
                <ul>
                    <li class="green">
                        <a href="#" class="fa fa-floppy-o"></a>
                        <ul>
                            <li>
                                <asp:LinkButton ID="lkbNuevo" runat="server" Text="Nuevo" OnClick="lkbElementoMenu_Click" CommandName= "Nuevo" /></li>
                            <li>
                                <asp:LinkButton ID="lkbAbrir" runat="server" Text="Abrir" OnClick="lkbElementoMenu_Click" CommandName="Abrir" /></li>
                            <li>
                                <asp:LinkButton ID="lkbGuardar" runat="server" Text="Guardar" OnClick="lkbElementoMenu_Click" CommandName="Guardar" /></li>
                            <li>
                                <asp:LinkButton ID="lkbTerminar" runat="server" Text="Terminar" OnClick="lkbElementoMenu_Click" CommandName="Terminar" /></li>
                        </ul>
                    </li>
                    <li class="red">
                        <a href="#" class="fa fa-pencil-square-o"></a>
                        <ul>
                            <li>
                                <asp:LinkButton ID="lkbEditar" runat="server" Text="Editar" OnClick="lkbElementoMenu_Click" CommandName="Editar" /></li>
                            <li>
                                <asp:LinkButton ID="lkbEliminar" runat="server" Text="Eliminar" OnClick="lkbElementoMenu_Click" CommandName="Eliminar" /></li>
                        </ul>
                    </li>
                    <li class="blue">
                        <a href="#" class="fa fa-cog"></a>
                        <ul>
                            <li>
                                <asp:LinkButton ID="lkbBitacora" runat="server" Text="Bitácora" OnClick="lkbElementoMenu_Click" CommandName="Bitacora" /></li>
                            <li>
                                <asp:LinkButton ID="lkbReferencias" runat="server" Text="Referencias" OnClick="lkbElementoMenu_Click" CommandName="Referencias" /></li>
                        </ul>
                    </li>
                </ul>
            </nav>
        </ContentTemplate>
        <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
        <asp:AsyncPostBackTrigger ControlID="btnCancelarSoporteEdita"/>
        <asp:AsyncPostBackTrigger ControlID="btnCancelarSoporte"/>
        <asp:AsyncPostBackTrigger ControlID="btnGuardarSoporte"/>
        <asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteTermino"/>
        <asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteEdita" />
        <asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteFecha" />
        <asp:PostBackTrigger ControlID="lkbBitacora" />
        <asp:PostBackTrigger ControlID="lkbReferencias" />
      
        </Triggers>
    </asp:UpdatePanel>
<!--Barra de navegacion-->
<!--Controles-->
<div class="seccion_controles">
<div class="header_seccion">
<img src="../Image/Soporte1.png" />
<h2>Descripción del Soporte:</h2>
</div>
<div class="columna2x">
<div class="renglon2x" >
<div class="etiqueta">
<label for="txtNoConsecutivoCompania">Consecutivo:</label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="UptxtNoConsecutivoCompania" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="txtNoConsecutivoCompania" runat="server" CssClass="label_negrita" Text="Por Asignar"></asp:Label>  
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarSoporteEdita"/>
<asp:AsyncPostBackTrigger ControlID="btnCancelarSoporte"/>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbTerminar" />    
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporte"/>
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteTermino"/>
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteEdita" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteFecha" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>

<div class="renglon2x">
<div class="etiqueta">
<label for="txtIdCompaniaEmisora">Compañia:</label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="uptxtIdCompaniaEmisora" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtIdCompaniaEmisora" CssClass="textbox2x" TabIndex="1" MaxLength="75"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarSoporteEdita"/>
<asp:AsyncPostBackTrigger ControlID="btnCancelarSoporte"/>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbTerminar" /> 
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporte" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteTermino"/>
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteEdita" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteFecha" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>

<div class="renglon2x">
<div class="etiqueta">
<label for="txtIdEstatus">Estatus:</label>
</div>
<div class="control">
<asp:UpdatePanel runat="server" ID="uptxtIdEstatus" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList runat="server" ID="txtIdEstatus" CssClass="dropdown_100px " TabIndex="2"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarSoporteEdita"/>
<asp:AsyncPostBackTrigger ControlID="btnCancelarSoporte"/>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbTerminar" /> 
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporte" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteTermino"/>
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteEdita" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteFecha" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>

<div class="renglon2x">
<div class="etiqueta">
<label for="txtUsuarioSolicitante">Solicitante:</label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="UptxtUsuarioSolicitante" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtUsuarioSolicitante" CssClass="textbox2x" TabIndex="3"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarSoporteEdita"/>
<asp:AsyncPostBackTrigger ControlID="btnCancelarSoporte"/>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbTerminar" /> 
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporte" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteTermino"/>
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteEdita" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteFecha" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>

<div class="renglon2x">
<div class="etiqueta">
<label for="txtIdUsuarioAsistente">Asistente:</label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="UptxtIdUsuarioAsistente" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtIdUsuarioAsistente" CssClass="textbox" TabIndex="4" MaxLength="9"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarSoporteEdita"/>
<asp:AsyncPostBackTrigger ControlID="btnCancelarSoporte"/>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbTerminar" /> 
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporte" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteTermino"/>
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteEdita" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteFecha" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>

<div class="renglon2x">
<div class="etiqueta">
<label for="txtFechaInicioGeneral">Inicio</label>
</div>
<div class="control">
<asp:UpdatePanel runat="server" ID="UptxtFechaInicioGeneral" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtFechaInicioGeneral" CssClass="textbox validate[required, custom[dateTime24]]" TabIndex="5" MaxLength="20"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarSoporteEdita"/>
<asp:AsyncPostBackTrigger ControlID="btnCancelarSoporte"/>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbTerminar" /> 
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporte" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteTermino"/>
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteEdita" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteFecha" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>

<div class="renglon2x">
<div class="etiqueta">
<label for="txtFechaTerminoGeneral">Termino</label>
</div>
<div class="control">
<asp:UpdatePanel runat="server" ID="uptxtFechaTerminoGeneral" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtFechaTerminoGeneral" CssClass="textbox validate[required, custom[dateTime24]]" TabIndex="6" MaxLength="20"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarSoporteEdita"/>
<asp:AsyncPostBackTrigger ControlID="btnCancelarSoporte"/>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbTerminar" /> 
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporte" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteTermino"/>
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteEdita" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteFecha" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>

<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="UpbtnAgregar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAgregar" runat="server" CssClass="boton" Text="Agregar"  TabIndex="7" OnClick="btnAgregar_Click" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarSoporteEdita"/>
<asp:AsyncPostBackTrigger ControlID="btnCancelarSoporte"/>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbTerminar" /> 
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporte" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteTermino"/>
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteEdita" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteFecha" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCancelar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnCancelar" runat="server" CssClass="boton_cancelar" TabIndex="8"  Text="Cancelar" OnClick="btnCancelar_Click" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarSoporteEdita"/>
<asp:AsyncPostBackTrigger ControlID="btnCancelarSoporte"/>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbTerminar" /> 
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporte" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteTermino"/>
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteEdita" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteFecha" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnGuardar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnGuardar" runat="server" CssClass="boton" Text="Guardar"  TabIndex="7" OnClick="btnGuardar_Click" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarSoporteEdita"/>
<asp:AsyncPostBackTrigger ControlID="btnCancelarSoporte"/>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbTerminar" /> 
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporte" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteTermino"/>
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteEdita" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteFecha" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>

<div class="renglon2x">
<div class="control2x">
<asp:UpdatePanel ID="uplblId" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblId" runat="server" Visible="false"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarSoporteEdita"/>
<asp:AsyncPostBackTrigger ControlID="btnCancelarSoporte"/>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbTerminar" /> 
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporte" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteTermino"/>
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteEdita" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteFecha" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>

<div class="renglon2x">
<div class="control2x">
<asp:UpdatePanel ID="uplblError" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblError" runat="server" CssClass="label_error"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarSoporteEdita"/>
<asp:AsyncPostBackTrigger ControlID="btnCancelarSoporte"/>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbTerminar" /> 
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporte" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteTermino"/>
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteEdita" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteFecha" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>  
</div>

<!--Grid-->
<div class="contenedor_seccion_completa">
<div class="header_seccion">
<img src="../Image/SoporteTecnicoDetalle.png" />
<h2>Detalle de Soporte</h2>
</div>
<div class="renglon3x">
<div class="etiqueta">
<label for="ddlTamano">Mostrar</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamano" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamano" runat="server" CssClass="dropdown" AutoPostBack="true"
OnSelectedIndexChanged="ddlTamano_SelectedIndexChanged" TabIndex="11"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblOrdenado">Ordenado</label>
</div>
<div class="etiqueta_155px">
<asp:UpdatePanel ID="uplblOrdenado" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblOrdenado" runat="server" CssClass="label_negrita"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvSoporteTecnico" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50pxr">
<asp:UpdatePanel ID="uplkbExportar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbExportar" runat="server" Text="Exportar" TabIndex="12" 
OnClick="lnkExportar_Click" CommandName="Requisicion"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbExportar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_400px_altura" oncontextmenu="return false">
<asp:UpdatePanel ID="upgvSoporteTecnico" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvSoporteTecnico" runat="server" PageSize="25" Width="100%" TabIndex="13"
AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" OnSorting="gvSoporteTecnico_Sorting"
OnRowDataBound="gvSoporteTecnico_RowDataBound" OnPageIndexChanging="gvSoporteTecnico_PageIndexChanging" 
CssClass="gridview" ShowFooter="True">
<AlternatingRowStyle CssClass="gridviewrowalternate" />
<EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
<Columns>
<asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="false" />
<asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
<asp:BoundField DataField="Soporte" HeaderText="Soporte" SortExpression="Soporte" />
<asp:BoundField DataField="Fecha Inicio" HeaderText="Fecha Inicio" SortExpression="Fecha Inicio" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
<asp:BoundField DataField="Fecha Termino" HeaderText="Fecha Termino" SortExpression="Fecha Termino" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
<asp:BoundField DataField="Observacion" HeaderText="Observación" SortExpression="Observacion" />
<asp:TemplateField>
<ItemTemplate>
<div id="menuContext" runat="server">
<img src="../Image/menu_context2.png" />
</div>
<div id="menuOptions" runat="server" class="MenuContext menuContainer" style="display:none;">
<div class="ContextItem">
<asp:LinkButton ID="lnkEditar" runat="server" Text="Editar" OnClick="lnkEditar_Click"></asp:LinkButton>
</div>
<div class="ContextItem">
<asp:LinkButton ID="lnkEliminar" runat="server" Text="Eliminar" OnClick="lnkEliminar_Click"></asp:LinkButton>
</div>
<div class="ContextItem">
<asp:LinkButton ID="lnkTerminarSoporte" runat="server" Text="Terminar Soporte" OnClick="lnkTerminarSoporte_Click"></asp:LinkButton>
</div>
</div>
</ItemTemplate>
</asp:TemplateField>
</Columns>
<FooterStyle CssClass="gridviewfooter" />
<HeaderStyle CssClass="gridviewheader" />
<RowStyle CssClass="gridviewrow" />
<SelectedRowStyle CssClass="gridviewrowselected" />
<SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
<SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarSoporteEdita"/>
<asp:AsyncPostBackTrigger ControlID="btnCancelarSoporte"/>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbTerminar" /> 
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporte" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteTermino"/>
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteFecha" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteEdita" />
<asp:AsyncPostBackTrigger ControlID="ddlTamano" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteEdita" />

</Triggers>
</asp:UpdatePanel>
</div>
</div>

<!-- VENTANA MODAL DE SOPORTE TECNICO AGREGAR DETALLE -->
<div id="soporteTecnicoModalAgregar" class="modal">
<div id="soporteTecnicoAgregar" class="contenedor_ventana_confirmacion_arriba" style="min-width:500px;padding-bottom:20px;" >
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarSoporteDetalle" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarSoporteDetalle" runat="server" OnClick="lkbCerrarVentanaModal_Click" CommandName="Agregar" >
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<asp:UpdatePanel ID="upucSoporte" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<!--Formulario de ventana modal-->
    <div class="seccion_controles_modal">
<div class="columna2x">
    <!--Controles-->
    <div class="header_seccion">
    <img src="../Image/Soportes.png" width="32" height="32"/>
    <h2>Agregar Soporte</h2>
    </div>

<div class="renglon2x" >
<div class="etiqueta">
<label for="txtIdSoporte">Soporte:</label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="UptxtIdSoporte" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList runat="server" ID="txtIdSoporte" CssClass="dropdown_100px" TabIndex="2"  Width="200"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarSoporteEdita"/>
<asp:AsyncPostBackTrigger ControlID="btnCancelarSoporte"/>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbTerminar" /> 
<asp:AsyncPostBackTrigger ControlID="btnAgregar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteFecha" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteEdita" />
<asp:AsyncPostBackTrigger ControlID="gvSoporteTecnico" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
           
<div class="renglon2x">
<div class="etiqueta">
<label for="txtObservacion">Observación:</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtObservacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtObservacion" runat="server" CssClass="textbox2x" TabIndex="3"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarSoporteEdita"/>
<asp:AsyncPostBackTrigger ControlID="btnCancelarSoporte"/>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbTerminar" /> 
<asp:AsyncPostBackTrigger ControlID="btnAgregar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteFecha" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteEdita" />
<asp:AsyncPostBackTrigger ControlID="gvSoporteTecnico" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>

<div class="renglon2x">
<div class="etiqueta">
<label for="txtFechaInicio">Inicio</label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="UptxtFechaInicio" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtFechaInicio" CssClass="textbox validate[required, custom[dateTime24]]" TabIndex="4" MaxLength="20"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarSoporteEdita"/>
<asp:AsyncPostBackTrigger ControlID="btnCancelarSoporte"/>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbTerminar" /> 
<asp:AsyncPostBackTrigger ControlID="btnAgregar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteFecha" />
<asp:AsyncPostBackTrigger ControlID="gvSoporteTecnico" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
          
  
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCancelarSoporte" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnCancelarSoporte" runat="server" CssClass="boton_cancelar" Text="Cancelar" OnClick="btnCancelarSoporte_Click" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarSoporteEdita"/>
<asp:AsyncPostBackTrigger ControlID="btnCancelarSoporte"/>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbTerminar" /> 
<asp:AsyncPostBackTrigger ControlID="btnAgregar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteFecha" />
<asp:AsyncPostBackTrigger ControlID="gvSoporteTecnico" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnGuardarSoporte" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnGuardarSoporte" runat="server" CssClass="boton" Text="Guardar" OnClick="btnGuardarSoporte_Click"/>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarSoporteEdita"/>
<asp:AsyncPostBackTrigger ControlID="btnCancelarSoporte"/>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbTerminar" />
<asp:AsyncPostBackTrigger ControlID="btnAgregar" />
<asp:AsyncPostBackTrigger ControlID="lkbCerrarSoporteDetalle"/>
<asp:AsyncPostBackTrigger ControlID="gvSoporteTecnico" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporte" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteFecha" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteTermino"/>
</Triggers>
</asp:UpdatePanel>
</div> 

 </div>

    

     </div>
          </div>
<!--fin de ventana modal -->


</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarSoporteEdita"/>
<asp:AsyncPostBackTrigger ControlID="btnCancelarSoporte"/>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbTerminar" /> 
<asp:AsyncPostBackTrigger ControlID="btnAgregar" />
<asp:AsyncPostBackTrigger ControlID="gvSoporteTecnico" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporte" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteFecha" />
<asp:AsyncPostBackTrigger ControlID="gvSoporteTecnico" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
 
<!-- VENTANA MODAL DE SOPORTE TECNICO TERMINAR FECHA -->
<div id="soporteTecnicoModalTerminar" class="modal">
<div id="soporteTecnicoTerminar" class="contenedor_ventana_confirmacion_arriba" style="min-width:500px;padding-bottom:20px;" >
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="UplkbCerrarSoporteFecha" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarSoporteFecha" runat="server" OnClick="lkbCerrarVentanaModal_Click" CommandName="Terminar" >
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<asp:UpdatePanel ID="upucSoporte1" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<!--Formulario de ventana modal-->
    <div class="seccion_controles_modal">
<div class="columna2x">
    <!--Controles-->
    <div class="header_seccion">
    <img src="../Image/Soportes.png" width="32" height="32"/>
    <h2>Terminar Soporte</h2>
    </div>

          
 <div class="renglon2x" ">
<div class="etiqueta">
<label for="txtFechaTermino">Termino</label >
</div>
<div class="control">
<asp:UpdatePanel runat="server" ID="UptxtFechaTermino" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtFechaTermino" CssClass="textbox validate[required, custom[dateTime24]]" TabIndex="5" MaxLength="20" visible="true"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarSoporteEdita"/>
<asp:AsyncPostBackTrigger ControlID="btnCancelarSoporte"/>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbTerminar" /> 
<asp:AsyncPostBackTrigger ControlID="btnAgregar" />
<asp:AsyncPostBackTrigger ControlID="gvSoporteTecnico" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporte" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteEdita" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteFecha" />
<asp:AsyncPostBackTrigger ControlID="gvSoporteTecnico" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>



 <div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnGuardarSoporteFecha" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnGuardarSoporteFecha" runat="server" CssClass="boton" Text="Guardar" OnClick="btnGuardarSoporteFecha_Click"/>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvSoporteTecnico" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteTermino"/>
</Triggers>
</asp:UpdatePanel>
</div>  
</div>  
    </div>   
        </div>      
<!--fin de ventana modal -->
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarSoporteEdita"/>
<asp:AsyncPostBackTrigger ControlID="btnCancelarSoporte"/>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnAgregar" />
<asp:AsyncPostBackTrigger ControlID="lkbTerminar" /> 
<asp:AsyncPostBackTrigger ControlID="gvSoporteTecnico" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporte" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteEdita" />
<asp:AsyncPostBackTrigger ControlID="gvSoporteTecnico" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>

<!-- VENTANA MODAL DE SOPORTE TECNICO TERMINAR PENDIENTES -->
<div id="soporteTecnicoModalTerminoDetalle" class="modal">
<div id="soporteTecnicoTerminoDetalle" class="contenedor_ventana_confirmacion_arriba" style="min-width:500px;padding-bottom:20px;" >
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarSoporteTermino" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarVentanaModal" runat="server" OnClick="lkbCerrarVentanaModal_Click" CommandName="TerminaDetalle" >
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<asp:UpdatePanel ID="upucSoporte2" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<!--Formulario de ventana modal-->
    <div class="seccion_controles_modal">
<div class="columna2x">
    <!--Controles-->
    <div class="header_seccion">
    <img src="../Image/Soportes.png" width="32" height="32"/>
    <h2>Terminar Soportes Restantes</h2>
    </div>        
 <div class="renglon2x">
<div class="etiqueta">
<label for="txtFechaTerminoDetalle">Termino</label >
</div>
<div class="control">
<asp:UpdatePanel runat="server" ID="UptxtFechaTerminoDetalle" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtFechaTerminoDetalle" CssClass="textbox validate[required, custom[dateTime24]]" TabIndex="5" MaxLength="20" visible="true"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarSoporteEdita"/>
<asp:AsyncPostBackTrigger ControlID="btnCancelarSoporte"/>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbTerminar" /> 
<asp:AsyncPostBackTrigger ControlID="btnAgregar" />
<asp:AsyncPostBackTrigger ControlID="gvSoporteTecnico" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporte" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteEdita" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteFecha" />
<asp:AsyncPostBackTrigger ControlID="gvSoporteTecnico" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>

 <div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="UpbtnGuardarTermino" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnGuardarSoporteTermino" runat="server" CssClass="boton" Text="Terminar" OnClick="btnGuardarSoporteTermino_Click"/>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvSoporteTecnico" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteTermino"/>
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteTermino"/>
</Triggers>
</asp:UpdatePanel>
</div>  
</div>  
    </div>   
        </div>      
<!--fin de ventana modal -->
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarSoporteEdita"/>
<asp:AsyncPostBackTrigger ControlID="btnCancelarSoporte"/>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnAgregar" />
<asp:AsyncPostBackTrigger ControlID="lkbTerminar" /> 
<asp:AsyncPostBackTrigger ControlID="gvSoporteTecnico" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporte" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteEdita" />
<asp:AsyncPostBackTrigger ControlID="gvSoporteTecnico" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>

<!-- VENTANA MODAL EDITAR SOPORTE TECNICO AGREGAR DETALLE -->
<div id="soporteTecnicoModalEdicion" class="modal">
<div id="soporteTecnicoEdicion" class="contenedor_ventana_confirmacion_arriba" style="min-width:500px;padding-bottom:20px;" >
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarSoporteTerminoEdita" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarSoporteTerminoEdita" runat="server" OnClick="lkbCerrarVentanaModal_Click" CommandName="Edicion" >
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<asp:UpdatePanel ID="upucSoporte3" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<!--Formulario de ventana modal-->
    <div class="seccion_controles_modal">
<div class="columna2x">
    <!--Controles-->
    <div class="header_seccion">
    <img src="../Image/Soportes.png" width="32" height="32"/>
    <h2>Edita Soporte</h2>
    </div>

<div class="renglon2x">
<div class="etiqueta">
<label for="txtIdSoporteE">Soporte:</label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="UptxtIdSoporteE" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList runat="server" ID="txtIdSoporteE" CssClass="dropdown_100px " TabIndex="2" Width="200"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarSoporteEdita"/>
<asp:AsyncPostBackTrigger ControlID="btnCancelarSoporte"/>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnAgregar" />
<asp:AsyncPostBackTrigger ControlID="lkbTerminar" /> 
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteFecha" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteEdita" />
<asp:AsyncPostBackTrigger ControlID="gvSoporteTecnico" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
           
<div class="renglon2x">
<div class="etiqueta">
<label for="txtObservacionE">Observación:</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="UptxtObservacionE" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtObservacionE" runat="server" CssClass="textbox2x" TabIndex="3"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarSoporteEdita"/>
<asp:AsyncPostBackTrigger ControlID="btnCancelarSoporte"/>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbTerminar" /> 
<asp:AsyncPostBackTrigger ControlID="btnAgregar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteFecha" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteEdita" />
<asp:AsyncPostBackTrigger ControlID="gvSoporteTecnico" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>

<div class="renglon2x">
<div class="etiqueta">
<label for="txtFechaInicioE">Inicio</label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="UptxtFechaInicioE" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtFechaInicioE" CssClass="textbox validate[required, custom[dateTime24]]" TabIndex="4" MaxLength="20"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarSoporteEdita"/>
<asp:AsyncPostBackTrigger ControlID="btnCancelarSoporte"/>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbTerminar" /> 
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnAgregar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteEdita" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteFecha" />
<asp:AsyncPostBackTrigger ControlID="gvSoporteTecnico" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>

          
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="UpbtnCancelarSoporteEdita" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnCancelarSoporteEdita" runat="server" CssClass="boton_cancelar" Text="Cancelar" OnClick="btnCancelarSoporteEdita_Click" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarSoporteEdita"/>
<asp:AsyncPostBackTrigger ControlID="btnCancelarSoporte"/>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbTerminar" /> 
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnAgregar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteFecha" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteEdita" />
<asp:AsyncPostBackTrigger ControlID="gvSoporteTecnico" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="UpbtnGuardarSoporteEdita" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnGuardarSoporteEdita" runat="server" CssClass="boton" Text="Guardar" OnClick="btnGuardarSoporteEdita_Click"/>
</ContentTemplate>
<Triggers>
    <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarSoporteEdita"/>
<asp:AsyncPostBackTrigger ControlID="btnCancelarSoporte"/>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbTerminar" /> 
<asp:AsyncPostBackTrigger ControlID="btnAgregar" />
<asp:AsyncPostBackTrigger ControlID="gvSoporteTecnico" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporte" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteEdita" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteFecha" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteTermino"/>
</Triggers>
</asp:UpdatePanel>
</div> 
 </div>
     </div>
          </div>
<!--fin de ventana modal -->
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarSoporteEdita"/>
<asp:AsyncPostBackTrigger ControlID="btnCancelarSoporte"/>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbTerminar" /> 
<asp:AsyncPostBackTrigger ControlID="btnAgregar" />
<asp:AsyncPostBackTrigger ControlID="gvSoporteTecnico" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporte" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteEdita" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarSoporteFecha" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>

</asp:Content>








