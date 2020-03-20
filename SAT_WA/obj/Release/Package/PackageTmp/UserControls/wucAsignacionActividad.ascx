<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucAsignacionActividad.ascx.cs" Inherits="SAT.UserControls.wucAsignacionActividad" %>
<!-- Estilos documentación de servicio -->
<link href="../CSS/Forma.css" rel="stylesheet" />
<link href="../CSS/Controles.css" rel="stylesheet" />
<link href="../CSS/DocumentacionServicio.css" rel="stylesheet" />
<link href="../CSS/ControlesUsuario.css" rel="stylesheet" />
<!-- Estilos Autocomplete, Mascara y Validadores JQuery -->
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
<link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
<!-- Bibliotecas para Validación de formulario -->
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
<!-- Biblioteca para uso de datetime picker -->
<script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
<script type="text/javascript">
    //Obteniendo instancia actual de la página y añadiendo manejador de evento
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    //Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
    function EndRequestHandler(sender, args) {
        if (args.get_error() == undefined) {
            ConfiguraJQueryWucAsignacionActividad();
        }
    }

    //Declarando Función de Configuración
    function ConfiguraJQueryWucAsignacionActividad() {
        $(document).ready(function () {
            //Declarando Función de Validación
            var validacionAsignacion = function () {
                var isValidP1;
                var isValidP2;

                //Obteniendo Tipo
                var tipo = $("#<%=ddlTipo.ClientID%>").val();

                //Validando el Tipo
                if (tipo == 1) {
                    isValidP1 = !$('#<%=txtEmpleado.ClientID%>').validationEngine('validate');
                    isValidP2 = true;
                }
                else {
                    isValidP1 = true;
                    isValidP2 = !$('#<%=txtProveedor.ClientID%>').validationEngine('validate');
                }

                //Devolviendo Resultado Obtenido
                return isValidP1 && isValidP2
            };

            //Validación de campos requeridos
            $('#<%=btnGuardar.ClientID%>').click(validacionAsignacion);

            //Cargando Controles de Fechas
            $("#<%=txtInicioAsignacion.ClientID%>").datetimepicker({
                lang: 'es',
                format: 'd/m/Y H:i'
            });
            $("#<%=txtFinAsignacion.ClientID%>").datetimepicker({
                lang: 'es',
                format: 'd/m/Y H:i'
            });

            //Cargando Catalogo de Proveedores
            $("#<%=txtProveedor.ClientID%>").autocomplete({
                source: '../WebHandlers/AutoCompleta.ashx?id=48&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>',
                appendTo: "<%=this.Contenedor%>"
            });
        });
    }
    
    //Invocando Función de Configutración
    ConfiguraJQueryWucAsignacionActividad();
</script>
<div class="contenedor_media_seccion">
    <div class="header_seccion">
        <h2>Asignación Actividad</h2>
    </div>
    <div class="columna550px">
        <div class="renglon2x">
            <div class="etiqueta">
                <label for="lblNoAsignacion">No. Asignación</label>
            </div>
            <div class="etiqueta_155px">
                <asp:UpdatePanel ID="uplblNoAsignacion" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <b><asp:Label ID="lblNoAsignacion" runat="server" Text="Por Asignar"></asp:Label></b>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                        <asp:AsyncPostBackTrigger ControlID="btnIniciar" />
                        <asp:AsyncPostBackTrigger ControlID="btnTerminar" />
                        <asp:AsyncPostBackTrigger ControlID="btnEliminar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon2x">
            <div class="etiqueta">
                <label for="ddlTipo">Tipo</label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="upddlTipo" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlTipo" runat="server" CssClass="dropdown" AutoPostBack="true" 
                            OnSelectedIndexChanged="ddlTipo_SelectedIndexChanged"></asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                        <asp:AsyncPostBackTrigger ControlID="btnIniciar" />
                        <asp:AsyncPostBackTrigger ControlID="btnTerminar" />
                        <asp:AsyncPostBackTrigger ControlID="btnEliminar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon2x">
            <div class="etiqueta">
                <label for="ddlEstatus">Estatus</label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="upddlEstatus" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlEstatus" runat="server" CssClass="dropdown" Enabled="false"></asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                        <asp:AsyncPostBackTrigger ControlID="btnIniciar" />
                        <asp:AsyncPostBackTrigger ControlID="btnTerminar" />
                        <asp:AsyncPostBackTrigger ControlID="btnEliminar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
           <div class="renglon2x">
            <div class="etiqueta">
                <label for="ddlPuesto">Puesto</label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="upddlPuesto" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlPuesto" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPuesto_SelectedIndexChanged" CssClass="dropdown" ></asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon2x">
            <div class="etiqueta">
                <label for="txtEmpleado">Empleado</label>
            </div>
            <div class="control2x">
                <asp:UpdatePanel ID="uptxtEmpleado" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TextBox ID="txtEmpleado" runat="server" CssClass="textbox2x validate[required, custom[IdCatalogo]]"></asp:TextBox>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                        <asp:AsyncPostBackTrigger ControlID="btnIniciar" />
                        <asp:AsyncPostBackTrigger ControlID="btnTerminar" />
                        <asp:AsyncPostBackTrigger ControlID="ddlTipo" />
                        <asp:AsyncPostBackTrigger ControlID="btnEliminar" />
                        <asp:AsyncPostBackTrigger ControlID="ddlPuesto" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon2x">
            <div class="etiqueta">
                <label for="txtProveedor">Proveedor</label>
            </div>
            <div class="control2x">
                <asp:UpdatePanel ID="uptxtProveedor" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TextBox ID="txtProveedor" runat="server" CssClass="textbox2x validate[required, custom[IdCatalogo]]"></asp:TextBox>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                        <asp:AsyncPostBackTrigger ControlID="btnIniciar" />
                        <asp:AsyncPostBackTrigger ControlID="btnTerminar" />
                        <asp:AsyncPostBackTrigger ControlID="ddlTipo" />
                        <asp:AsyncPostBackTrigger ControlID="btnEliminar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon2x">
            <div class="etiqueta">
                <label for="txtInicioAsignacion">Inicio</label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="uptxtInicioAsignacion" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TextBox ID="txtInicioAsignacion" Enabled="false" runat="server" CssClass="textbox validate[required, custom[dateTime24]]" MaxLength="16"></asp:TextBox>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                        <asp:AsyncPostBackTrigger ControlID="btnIniciar" />
                        <asp:AsyncPostBackTrigger ControlID="btnTerminar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon2x">
            <div class="etiqueta">
                <label for="txtInicioAsignacion">Fin</label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="uptxtFinAsignacion" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TextBox ID="txtFinAsignacion"  Enabled="false" runat="server" CssClass="textbox validate[required, custom[dateTime24]]" MaxLength="16"></asp:TextBox>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                        <asp:AsyncPostBackTrigger ControlID="btnIniciar" />
                        <asp:AsyncPostBackTrigger ControlID="btnTerminar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon3x">
            <div class="control_100px">
                <asp:UpdatePanel ID="upbtnGuardar" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Button ID="btnGuardar" runat="server" Text="Guardar" OnClick="btnGuardar_Click" CssClass="boton" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnIniciar" />
                        <asp:AsyncPostBackTrigger ControlID="btnTerminar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>            
            <div class="control_100px">
                <asp:UpdatePanel ID="upbtnEliminar" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" OnClick="btnEliminar_Click" CssClass="boton" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnIniciar" />
                        <asp:AsyncPostBackTrigger ControlID="btnTerminar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="control_100px">
                <asp:UpdatePanel ID="upbtnIniciar" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Button ID="btnIniciar" runat="server" CssClass="boton" Text="Iniciar" OnClick="btnIniciar_Click" CommandName="Iniciar" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                        <asp:AsyncPostBackTrigger ControlID="btnTerminar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="control_100px">
                <asp:UpdatePanel ID="upbtnTerminar" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Button ID="btnTerminar" runat="server" CssClass="boton_cancelar" Text="Terminar" OnClick="btnTerminar_Click" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                        <asp:AsyncPostBackTrigger ControlID="btnIniciar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon2x">

        </div>
    </div>
</div>