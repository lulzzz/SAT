<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Ruta.aspx.cs" MasterPageFile="~/MasterPage/MasterPage.Master" Inherits="SAT.Ruta.Ruta" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Estilos documentación de servicio -->
    <link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
    <link href="../CSS/ControlesUsuario.css" type="text/css" rel="stylesheet" />
    <link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
    <!-- Estilos Autocomplete, Mascara y Validadores JQuery -->
    <link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
    <!-- Bibliotecas para Validación de formulario -->
    <script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
    <script type="text/javascript">
        //Obteniendo instancia actual de la página y añadiendo manejador de evento
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        //Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
        function EndRequestHandler(sender, args) {
            if (args.get_error() == undefined) {
                ConfiguraJQueryRuta();
            }
        }
        //Función encargada de restablecer la Configuración de los Scripts de Validación
        function ConfiguraJQueryRuta() {
            $(document).ready(function () {
                //Validando que se cumplan las condiciones para Insercció de Ruta
                var validaRuta = function () {
                    var isValid1 = !$("#<%=txtKilometros.ClientID%>").validationEngine('validate');
    var isValid2 = !$("#<%=txtOrigen.ClientID%>").validationEngine('validate');
    var isValid3 = !$("#<%=txtDestino.ClientID%>").validationEngine('validate');
    var isValid4 = !$("#<%=txtCliente.ClientID%>").validationEngine('validate');
    var isValid5 = !$("#<%=txtDescripcion.ClientID%>").validationEngine('validate');
        //Devolviendo Resultado
        return isValid1 && isValid2 && isValid3 && isValid4 && isValid5;
    }

    //Añadiendo Funcion al Evento Click del Boton "Guardar"
    $("#<%=btnGuardar.ClientID%>").click(validaRuta);
    //Validando que se cumplan las condiciones para Insercción de Casetas
    var validaCaseta = function () {
        var isValid1 = !$("#<%=txtCaseta.ClientID%>").validationEngine('validate');
        //Devolviendo Resultado
        return isValid1;
    }

    //Añadiendo Funcion al Evento Click del Boton "Guardar"
    $("#<%=imbAgregarCaseta.ClientID%>").click(validaCaseta);
    //Validando que se cumplan las condiciones para Insercción de Tipos de Unidad
    var validaTipoUnidadDiesel = function () {
        var isValid1 = !$("#<%=txtRendimiento.ClientID%>").validationEngine('validate');
        //Devolviendo Resultado
        return isValid1;
    }

    //Añadiendo Funcion al Evento Click del Boton "Guardar"
    $("#<%=btnAceptarTipoUnidad.ClientID%>").click(validaTipoUnidadDiesel);
    //Validación de controles de Inserción de Ciudades
    var validacionInserccionVales = function () {
        var isValidP1 = !$('.scriptLitros').validationEngine('validate');
        return isValidP1;
    };
    //Validación de campos requeridos
    $('.scriptGuardarVale').click(validacionInserccionVales);
    //Validación de controles de Inserción de Ciudades
    var validacionEditarVales = function () {
        var isValidP1 = !$('.scriptLitrosE').validationEngine('validate');
        return isValidP1;

    };
    //Validación de campos requeridos
    $('.scriptGuardarValeE').click(validacionEditarVales);

    //Validación de controles de Edición de Concepto
    var validacionEditaConcepto = function () {
        var isValidP1 = !$('.scriptMontoE').validationEngine('validate');
        return isValidP1;

    };
    //Validación de campos requeridos
    $('.scriptGuardarConceptoE').click(validacionEditaConcepto);
    //Validación de controles de Inserción de Concepto
    var validacionInsertaConcepto = function () {
        var isValidP1 = !$('.scriptMonto').validationEngine('validate');
        return isValidP1;
    };
    //Validación de campos requeridos
    $('.scriptGuardarVale').click(validacionInserccionVales);
    //Validación de controles de Inserción de Ciudades
    var validacionEditarVales = function () {
        var isValidP1 = !$('.scriptLitrosE').validationEngine('validate');
        return isValidP1;

    };
    //Cargando Función de Validación de Funciones del Control
        $("#<%=gvVale.ClientID%>").click(function (sender) {
            //Declarando Variable de Retorno
            var isValid = true;
            //Validando el Control que produjo el Evento
            if (sender.target.id.indexOf('lnkInserta') != -1) {
                //Validando si el Control "Anden" esta Visible
                if ($("*[id$=gvVale] input[id$=txtEstaciones]")[0] != undefined) {
                    //Carga Validacion
                    isValid = !$("*[id$=gvVale] input[id$=txtEstaciones]").validationEngine('validate');
                    //alert("Anden");
                }//Validando si el Control "Cajon" esta Visible
                else if ($("*[id$=gvVale] input[id$=txtProveedores]")[0] != undefined) {
                    //Carga Validacion
                    isValid = !$("*[id$=gvVale] input[id$=txtProveedores]").validationEngine('validate');
                    //alert("Cajon");
                }
            }
            //Devolviendo Resultado de validación
            return isValid;
        });
         //Cargando Función de Validación de Funciones del Control
        $("#<%=gvVale.ClientID%>").click(function (sender) {
            //Declarando Variable de Retorno
            var isValid = true;
            //Validando el Control que produjo el Evento
            if (sender.target.id.indexOf('lnkGuardaE') != -1) {
                //Validando si el Control "Anden" esta Visible
                if ($("*[id$=gvVale] input[id$=txtEstacionesE]")[0] != undefined) {
                    //Carga Validacion
                    isValid = !$("*[id$=gvVale] input[id$=txtEstacionesE]").validationEngine('validate');
                    //alert("Anden");
                }//Validando si el Control "Cajon" esta Visible
                else if ($("*[id$=gvVale] input[id$=txtProveedoresE]")[0] != undefined) {
                    //Carga Validacion
                    isValid = !$("*[id$=gvVale] input[id$=txtProveedoresE]").validationEngine('validate');
                    //alert("Cajon");
                }
            }
            //Devolviendo Resultado de validación
            return isValid;
        });
        //Validación de campos requeridos
        $('.scriptGuardarConcepto').click(validacionInsertaConcepto);
        //Añadiendo Función de Autocompletado al Control de Cliente
        $("#<%=txtCliente.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=55&param=<%= ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>' });
        //Añadiendo Función de Autocompletado al Control de Casetas
        $("#<%=txtCaseta.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=54&param=<%= ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>' });
        });
        }

        //Invocando Funcion de Configuración
        ConfiguraJQueryRuta();
    </script>
    <div id="encabezado_forma">
        <img src="../Image/Paradas.png" />
        <h1>Ruta</h1>
    </div>
    <asp:UpdatePanel ID="upMenuPrincipal" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <nav id="menuForma">
                <ul>
                    <li class="green">
                        <a href="#" class="fa fa-floppy-o"></a>
                        <ul>
                            <li>
                                <asp:LinkButton ID="lkbNuevo" runat="server" Text="Nuevo" OnClick="lkbElementoMenu_Click" CommandName="Nuevo" /></li>
                            <li>
                                <asp:LinkButton ID="lkbAbrir" runat="server" Text="Abrir" OnClick="lkbElementoMenu_Click" CommandName="Abrir" /></li>
                            <li>
                                <asp:LinkButton ID="lkbGuardar" runat="server" Text="Guardar" OnClick="lkbElementoMenu_Click" CommandName="Guardar" /></li>
                            <li>
                                <asp:LinkButton ID="lkbSalir" runat="server" Text="Salir" OnClick="lkbElementoMenu_Click" CommandName="Salir" /></li>
                        </ul>
                    </li>
                    <li class="red">
                        <a href="#" class="fa fa-pencil-square-o"></a>
                        <ul>
                            <li>
                                <asp:LinkButton ID="lkbEditar" runat="server" Text="Editar" OnClick="lkbElementoMenu_Click" CommandName="Editar" /></li>
                            <li>
                                <asp:LinkButton ID="lkbEliminar" runat="server" Text="Eliminar" OnClick="lkbElementoMenu_Click" CommandName="Eliminar" /></li>
                            <li>
                                <asp:LinkButton ID="lkbCopiar" runat="server" Text="Copiar" OnClick="lkbElementoMenu_Click" CommandName="Copiar" /></li>
                        </ul>
                    </li>
                    <li class="blue">
                        <a href="#" class="fa fa-cog"></a>
                        <ul>
                            <li>
                                <asp:LinkButton ID="lkbBitacora" runat="server" Text="Bitácora" OnClick="lkbElementoMenu_Click" CommandName="Bitacora" /></li>
                            <li>
                                <asp:LinkButton ID="lkbReferencias" runat="server" Text="Referencias" OnClick="lkbElementoMenu_Click" CommandName="Referencias" /></li>
                            <li>
                                <asp:LinkButton ID="lkbArchivos" runat="server" Text="Archivos" OnClick="lkbElementoMenu_Click" CommandName="Archivos" /></li>
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
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
            <asp:PostBackTrigger ControlID="lkbBitacora" />
            <asp:PostBackTrigger ControlID="lkbAbrir" />
            <asp:PostBackTrigger ControlID="lkbReferencias" />
        </Triggers>
    </asp:UpdatePanel>
    <div class="contenedor_controles">
        <div class="columna3x">
            <div class="renglon3x">
                <div class="etiqueta">
                    <label for="txtDescripcion">Descripción</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtDescripcion" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtDescripcion" runat="server" TabIndex="1" CssClass="textbox2x validate[required]" MaxLength="400"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbCopiar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="etiqueta" style="width: 12px">
                    <asp:UpdatePanel ID="uplblCopia" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label runat="server" ID="lblCopia" Text="" CssClass="label_error"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbCopiar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtAliasRuta">Alias</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtAliasRuta" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtAliasRuta" runat="server" TabIndex="2" CssClass="textbox validate[required]" MaxLength="50"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbCopiar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="ddlTipoAplicacion">Tipo Aplicación</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="upddlTipoAplicacion" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlTipoAplicacion" runat="server" TabIndex="3" CssClass="dropdown"></asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbCopiar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtCliente">Cliente</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtCliente" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtCliente" runat="server" TabIndex="4" CssClass="textbox2x validate[required, custom[IdCatalogo]]" MaxLength="100"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbCopiar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="ddlTipoOrigen">Tipo Origen</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="upddlTipoOrigen" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlTipoOrigen" AutoPostBack="true" runat="server" TabIndex="5" OnSelectedIndexChanged="ddlTipoOrigen_SelectedIndexChanged" CssClass="dropdown"></asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbCopiar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>           
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtOrigen">Origen</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtOrigen" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtOrigen" runat="server" TabIndex="6" CssClass="textbox2x validate[required, custom[IdCatalogo]]" MaxLength="100"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                            <asp:AsyncPostBackTrigger ControlID="ddlTipoOrigen" />
                            <asp:AsyncPostBackTrigger ControlID="lkbCopiar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>

            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="ddlTipoDestino">Tipo Destino</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="upddlTipoDestino" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlTipoDestino" AutoPostBack="true" runat="server" TabIndex="7" OnSelectedIndexChanged="ddlTipoDestino_SelectedIndexChanged" CssClass="dropdown"></asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbCopiar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtDestino">Destino</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtDestino" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtDestino" runat="server" TabIndex="8" CssClass="textbox2x validate[required, custom[IdCatalogo]]" MaxLength="100"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                            <asp:AsyncPostBackTrigger ControlID="ddlTipoDestino" />
                            <asp:AsyncPostBackTrigger ControlID="lkbCopiar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="Kilometros">Kilometros</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="upKilometros" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtKilometros" runat="server" TabIndex="9" CssClass="textbox validate[required, custom[positiveNumber]]" MaxLength="18"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbCopiar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>

            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="chkPermisionario">Permisionario</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="upchkPermisionario" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:CheckBox ID="chkPermisionario" TabIndex="10" runat="server" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbCopiar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="controlBoton">
                    <asp:UpdatePanel ID="upbtnGuardar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnGuardar" runat="server" TabIndex="11" Text="Guardar" OnClick="btnGuardar_Click" CssClass="boton" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbCopiar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="controlBoton">
                    <asp:UpdatePanel ID="upbtnCancelar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnCancelar" OnClick="btnCancelar_Click" runat="server" TabIndex="12" Text="Cancelar" CssClass="boton_cancelar" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbCopiar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>

        </div>
    </div>
    <div class="contenedor_botones_pestaña">
        <div class="control_boton_pestana">
            <asp:UpdatePanel ID="upbtnPestanaCasetas" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Button ID="btnPestanaCasetas" runat="server" TabIndex="13" Text="Casetas" CssClass="boton_pestana_activo" CommandName="Casetas" OnClick="btnPestana_Click" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnPestanaDiesel" />
                    <asp:AsyncPostBackTrigger ControlID="btnPestanaConcepto" />
                    <asp:AsyncPostBackTrigger ControlID="btnPestanaDiesel1" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div class="control_boton_pestana">
            <asp:UpdatePanel ID="upbtnPestanaDiesel" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Button ID="btnPestanaDiesel" runat="server" Text="Diesel" TabIndex="14" CssClass="boton_pestana" CommandName="Diesel" OnClick="btnPestana_Click" Visible="false" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnPestanaCasetas" />
                    <asp:AsyncPostBackTrigger ControlID="btnPestanaConcepto" />
                    <asp:AsyncPostBackTrigger ControlID="btnPestanaDiesel1" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div class="control_boton_pestana">
            <asp:UpdatePanel ID="upbtnPestanaConcepto" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Button ID="btnPestanaConcepto" runat="server" Text="Concepto" TabIndex="14" CssClass="boton_pestana" CommandName="Concepto" OnClick="btnPestana_Click" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnPestanaCasetas" />
                    <asp:AsyncPostBackTrigger ControlID="btnPestanaDiesel" />
                    <asp:AsyncPostBackTrigger ControlID="btnPestanaDiesel1" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div class="control_boton_pestana">
            <asp:UpdatePanel ID="upbtnPestanaDiesel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Button ID="btnPestanaDiesel1" runat="server" Text="Diesel" TabIndex="14" CssClass="boton_pestana" CommandName="Diesel1" OnClick="btnPestana_Click" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnPestanaCasetas" />
                    <asp:AsyncPostBackTrigger ControlID="btnPestanaConcepto" />
                    <asp:AsyncPostBackTrigger ControlID="btnPestanaDiesel" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <div class="contenido_tabs">
        <asp:UpdatePanel ID="upmtvRuta" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:MultiView ID="mtvRuta" runat="server" ActiveViewIndex="0">
                    <asp:View ID="vwCasetas" runat="server">
                        <div class="columna3x">
                            <div class="header_seccion">
                                <img src="../Image/Buscar.png" />
                                <h2>Filtros de Búsqueda</h2>
                            </div>
                            <div class="renglon2x">
                                <div class="etiqueta">
                                    <label for="txtCaseta">Caseta</label>
                                </div>
                                <div class="control2x">
                                    <asp:UpdatePanel ID="uptxtCaseta" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtCaseta" runat="server" MaxLength="250" CssClass="textbox2x validate[required, custom[IdCatalogo]]" TabIndex="15"></asp:TextBox>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="imbAgregarCaseta" />
                                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                            <asp:AsyncPostBackTrigger ControlID="imbAgregarCaseta" />
                                            <asp:AsyncPostBackTrigger ControlID="lkbCopiar" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                            <div style="width: auto; padding: 5px">
                                <asp:UpdatePanel ID="upimbAgregarCaseta" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:LinkButton ID="imbAgregarCaseta" TabIndex="16" runat="server" OnClick="imbAgregarCaseta_Click">
<img alt="" src="../Image/Agregar.png" width="25" height="25" />
                                        </asp:LinkButton>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                                        <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                        <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                                        <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                        <asp:AsyncPostBackTrigger ControlID="lkbCopiar" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </div>

                         <div class="renglon2x">
                <div class="etiqueta">
                    <label for="ddlTipoDeposito">Tipo Deposito</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="upddlTipoDeposito" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlTipoDeposito" runat="server" TabIndex="17" CssClass="dropdown2x"
                                   AutoPostBack="true"></asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbCopiar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>

                        <div class="header_seccion">
                            <img src="../Image/TablaResultado.png" />
                            <h2>Casetas</h2>
                        </div>
                        <div class="renglon3x">
                            <div class="etiqueta" style="width: auto">
                                <label for="ddlTamanoCasetas">
                                    Mostrar:
                                </label>
                            </div>
                            <div class="control">
                                <asp:DropDownList ID="ddlTamanoCasetas" OnSelectedIndexChanged="ddlTamanoCasetas_SelectedIndexChanged" runat="server" TabIndex="18" AutoPostBack="true" CssClass="dropdown" />
                            </div>
                            <div class="etiqueta">
                                <label for="lblOrdenadoCasetas">Ordenado Por:</label>
                            </div>
                            <div class="etiqueta" style="width: auto">
                                <asp:UpdatePanel ID="uplblOrdenadoCasetas" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblOrdenadoCasetas" runat="server"></asp:Label>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="gvCasetas" EventName="Sorting" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                            <div class="etiqueta" style="width: auto;">
                                <asp:UpdatePanel ID="uplkbExportarCasetas" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:LinkButton ID="lkbExportarCasetas" runat="server" Text="Exportar Excel" TabIndex="17"></asp:LinkButton>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="lkbExportarCasetas" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <div class="grid_seccion_completa_altura_variable">
                            <asp:UpdatePanel ID="upgvCasetas" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:GridView ID="gvCasetas" OnPageIndexChanging="gvCasetas_PageIndexChanging" ShowFooter="True" OnSorting="gvCasetas_Sorting" runat="server" AutoGenerateColumns="False" AllowPaging="True" TabIndex="18"
                                        ShowHeaderWhenEmpty="True" PageSize="10" AllowSorting="True"
                                        CssClass="gridview" Width="100%">
                                        <AlternatingRowStyle CssClass="gridviewrowalternate" />
                                        <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                                        <Columns>
                                            <asp:BoundField DataField="Descripcion" HeaderText="Descripción" SortExpression="Descripcion" />
                                            <asp:BoundField DataField="TipoCaseta" HeaderText="Tipo Caseta" SortExpression="TipoCaseta" />
                                            <asp:BoundField DataField="RedCarretera" HeaderText="Red Carretera" SortExpression="RedCarretera" />
                                            <asp:BoundField DataField="IAVE" HeaderText="IAVE" SortExpression="IAVE" />
                                            <asp:BoundField DataField="C1" HeaderText="C1" SortExpression="C1" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" />
                                            <asp:BoundField DataField="C2" HeaderText="C2" SortExpression="C2" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" />
                                            <asp:BoundField DataField="C3" HeaderText="C3" SortExpression="C3" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" />
                                            <asp:BoundField DataField="C4" HeaderText="C4" SortExpression="C4" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" />
                                            <asp:BoundField DataField="C5" HeaderText="C5" SortExpression="C5" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" />
                                            <asp:BoundField DataField="C6" HeaderText="C6" SortExpression="C6" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" />
                                            <asp:BoundField DataField="C7" HeaderText="C7" SortExpression="C7" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" />
                                            <asp:BoundField DataField="C8" HeaderText="C8" SortExpression="C8" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" />
                                            <asp:BoundField DataField="C9" HeaderText="C9" SortExpression="C9" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" />
                                            <asp:BoundField DataField="TipoDeposito" HeaderText="Tipo Deposito" SortExpression="TipoDeposito" />
                                            <asp:TemplateField HeaderText="" SortExpression="Bitacora">
                                                <ItemTemplate>
                                                    <asp:UpdatePanel ID="uplnkBitacoraCaseta" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:LinkButton ID="lnkBitacoraCaseta" runat="server" TabIndex="27" OnClick="lnkBitacoraCaseta_Click" Text="Bitácora" CommandName="Bitacora"></asp:LinkButton>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:PostBackTrigger ControlID="lnkBitacoraCaseta" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" SortExpression="Deshabilitar">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkDeshabilitar" runat="server" OnClick="lnkDeshabilitar_Click" TabIndex="19" Text="Deshabilitar"></asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <FooterStyle CssClass="gridviewfooter" />
                                        <HeaderStyle CssClass="gridviewheader" />
                                        <RowStyle CssClass="gridviewrow" />
                                        <SelectedRowStyle CssClass="gridviewrowselected" />
                                    </asp:GridView>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ddlTamanoCasetas" />
                                    <asp:AsyncPostBackTrigger ControlID="imbAgregarCaseta" />
                                    <asp:AsyncPostBackTrigger ControlID="lkbCopiar" />
                                    <asp:AsyncPostBackTrigger ControlID="btnPestanaCasetas" />
                                    <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                    <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                                    <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                    <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                    <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </asp:View>
                    <asp:View ID="vwDiesel" runat="server">
                        <div class="seccion_controles">
                            <div class="header_seccion">
                                <img src="../Image/Requerimiento.png" />
                                <h2>Datos del Tipo de Unidad</h2>
                            </div>
                            <div class="columna2x">
                                <div class="renglon2x">
                                    <div class="etiqueta">
                                        <label for="ddlTipoUnidad">Tipo Unidad</label>
                                    </div>
                                    <div class="control2x">
                                        <asp:UpdatePanel ID="upddlTipoUnidad" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddlTipoUnidad" AutoPostBack="true" runat="server" TabIndex="10" CssClass="dropdown"></asp:DropDownList>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                                <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                                                <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                                <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                                                <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                                                <asp:AsyncPostBackTrigger ControlID="btnAceptarTipoUnidad" />
                                                <asp:AsyncPostBackTrigger ControlID="lkbCopiar" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                                <div class="renglon2x">
                                    <div class="etiqueta">
                                        <label for="ddlConfiguracion">Configuración</label>
                                    </div>
                                    <div class="control2x">
                                        <asp:UpdatePanel ID="upddlConfiguracion" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddlConfiguracion" AutoPostBack="true" runat="server" TabIndex="21" CssClass="dropdown"></asp:DropDownList>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                                <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                                                <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                                <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                                                <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                                                <asp:AsyncPostBackTrigger ControlID="btnAceptarTipoUnidad" />
                                                <asp:AsyncPostBackTrigger ControlID="lkbCopiar" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                                <div class="renglon2x">
                                    <div class="etiqueta">
                                        <label for="txtRendiemiento">Rendimiento</label>
                                    </div>
                                    <div class="control2x">
                                        <asp:UpdatePanel runat="server" ID="uptxtRendimiento" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:TextBox runat="server" ID="txtRendimiento" MaxLength="50" TabIndex="23" CssClass="textbox validate[required,custom[positiveNumber]]"></asp:TextBox>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                                <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                                                <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                                <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                                                <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                                                <asp:AsyncPostBackTrigger ControlID="btnAceptarTipoUnidad" />
                                                <asp:AsyncPostBackTrigger ControlID="lkbCopiar" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                                <div class="renglon2x">
                                    <div class="controlBoton">
                                        <asp:UpdatePanel ID="upbtnAceptarTipoUnidad" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Button ID="btnAceptarTipoUnidad" TabIndex="24" runat="server" OnClick="btnAceptarTipoUnidad_Click" CssClass="boton" Text="Aceptar" />
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                                <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                                                <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                                <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                                                <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                                                <asp:AsyncPostBackTrigger ControlID="lkbCopiar" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                            <div class="columna2x">
                                <div class="grid_seccion_completa_200px_altura">
                                    <asp:UpdatePanel ID="upgvTipoUnidad" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:GridView ID="gvTipoUnidad" runat="server" PageSize="10" OnPageIndexChanging="gvTipoUnidad_PageIndexChanging" AutoGenerateColumns="False" OnRowDataBound="gvTipoUnidad_RowDataBound"
                                                ShowFooter="True" CssClass="gridview" Width="100%" AllowPaging="True" AllowSorting="True">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Tipo Unidad" SortExpression="TipoUnidad">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkSeleccionar" runat="server" TabIndex="25" OnClick="lnkAccionTipoUnidad_Click" Text='<%#Eval("TipoUnidad") %>' CommandName="Seleccionar"></asp:LinkButton>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="Configuracion" HeaderText="Configuración" SortExpression="Configuracion" />
                                                    <asp:BoundField DataField="Rendimiento" HeaderText="Rendimiento" SortExpression="Rendimiento" />
                                                    <asp:TemplateField HeaderText="" SortExpression="Deshabilitar">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkDeshabilitar" runat="server" TabIndex="26" OnClick="lnkAccionTipoUnidad_Click" Text="Deshabilitar" CommandName="Deshabilitar"></asp:LinkButton>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="" SortExpression="Bitacora">
                                                        <ItemTemplate>
                                                            <asp:UpdatePanel ID="uplnkBitacora" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:LinkButton ID="lnkBitacora" runat="server" TabIndex="27" OnClick="lnkAccionTipoUnidad_Click" Text="Bitácora" CommandName="Bitacora"></asp:LinkButton>
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:PostBackTrigger ControlID="lnkBitacora" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <AlternatingRowStyle CssClass="gridviewrowalternate" />
                                                <FooterStyle CssClass="gridviewfooter" />
                                                <HeaderStyle CssClass="gridviewheader" />
                                                <RowStyle CssClass="gridviewrow" />
                                                <SelectedRowStyle CssClass="gridviewrowselected" />
                                                <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                                                <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                                            </asp:GridView>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnAceptarTipoUnidad" />
                                            <asp:AsyncPostBackTrigger ControlID="lkbCopiar" />
                                            <asp:AsyncPostBackTrigger ControlID="btnPestanaDiesel" />
                                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                            <div class="header_seccion">
                                <h2>Vales</h2>
                            </div>
                            <div class="renglon3x">
                                <div class="etiqueta_50px">
                                    <label for="ddlTamanoVales">Mostrar:</label>
                                </div>
                                <div class="control">
                                    <asp:UpdatePanel ID="upddlTamanoVales" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="ddlTamanoVales" runat="server" CssClass="dropdown" TabIndex="28"
                                                OnSelectedIndexChanged="ddlTamanoVales_SelectedIndexChanged" AutoPostBack="true">
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <div class="etiqueta">
                                    <label>Ordenado Por:</label>
                                </div>
                                <div class="etiqueta">
                                    <asp:UpdatePanel ID="uplblOrdenadoVales" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="lblOrdenadoVales" runat="server"></asp:Label>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="gvVales" EventName="Sorting" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                                <div class="etiqueta">
                                    <asp:UpdatePanel ID="uplnkExportar" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:LinkButton ID="lkbExportarVales" runat="server" TabIndex="29"
                                                OnClick="lkbExportarVales_Click" Text="Exportar"></asp:LinkButton>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="lkbExportarVales" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                            <div class="grid_seccion_completa_altura_variable">
                                <asp:UpdatePanel ID="upgvVales" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView ID="gvVales" runat="server" TabIndex="30" AutoGenerateColumns="False" OnSorting="gvVales_Sorting" OnPageIndexChanging="gvVales_PageIndexChanging" OnRowDataBound="gvVales_RowDataBound"
                                            ShowFooter="True" CssClass="gridview" Width="100%" AllowPaging="True" AllowSorting="True" PageSize="10">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Litros" SortExpression="Litros">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtLitrosE" MaxLength="18" runat="server" CssClass="textbox scriptLitrosE validate[required,custom[positiveNumber]]">
                                                        </asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbllitros" runat="server" Text='<%#Eval("Litros") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtlitros" MaxLength="18" runat="server" CssClass="textbox scriptLitros validate[required,custom[positiveNumber]]">
                                                        </asp:TextBox>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Tipo Operación" SortExpression="TipoOperacion">
                                                    <EditItemTemplate>
                                                        <asp:DropDownList runat="server" ID="ddlTipoOperacionE" CssClass="dropdown">
                                                        </asp:DropDownList>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTipoOperacion" runat="server" Text='<%#Eval("TipoOperacion") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:DropDownList runat="server" ID="ddlTipoOperacion" CssClass="dropdown">
                                                        </asp:DropDownList>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Estación de Combustible" SortExpression="UbicacionEstacion">
                                                    <EditItemTemplate>
                                                        <asp:DropDownList runat="server" ID="ddlEstacionE" CssClass="dropdown2x">
                                                        </asp:DropDownList>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEstacion" runat="server" Text='<%#Eval("UbicacionEstacion") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:UpdatePanel ID="upddlEstacion" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:DropDownList runat="server" ID="ddlEstacion" CssClass="dropdown2x"></asp:DropDownList>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                            </Triggers>
                                                        </asp:UpdatePanel>

                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <EditItemTemplate>
                                                        <asp:LinkButton ID="lnkGuardarE" runat="server" OnClick="lkbAccionVale_Click" CssClass="textbox scriptGuardarValeE" CommandName="GuardarE" Text="Guardar"></asp:LinkButton>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkEditar" runat="server" OnClick="lnkGuardarE_Click" CommandName="Editar" Text="Editar"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:LinkButton ID="lnkInsertar" runat="server" TabIndex="31" CssClass="textbox scriptGuardarVale" Text="Insertar" CommandName="Insertar" OnClick="lnkInsertar_Click"></asp:LinkButton>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <EditItemTemplate>
                                                        <asp:LinkButton ID="lnkCancelarE" runat="server" OnClick="lkbAccionVale_Click" CssClass="textbox" CommandName="CancelarE" Text="Cancelar"></asp:LinkButton>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkDeshabilitar" TabIndex="32" runat="server" OnClick="lkbAccionVale_Click" Text="Deshabilitar" CommandName="Deshabilitar"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkBitacora" TabIndex="33" runat="server" OnClick="lkbAccionVale_Click" Text="Bitácora" CommandName="Bitacora"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <AlternatingRowStyle CssClass="gridviewrowalternate" />
                                            <FooterStyle CssClass="gridviewfooter" />
                                            <HeaderStyle CssClass="gridviewheader" />
                                            <RowStyle CssClass="gridviewrow" />
                                            <SelectedRowStyle CssClass="gridviewrowselected" />
                                            <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                                            <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                                        </asp:GridView>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddlTamanoVales" />
                                        <asp:AsyncPostBackTrigger ControlID="gvTipoUnidad" />
                                        <asp:AsyncPostBackTrigger ControlID="btnAceptarTipoUnidad" />
                                        <asp:AsyncPostBackTrigger ControlID="lkbCopiar" />
                                        <asp:AsyncPostBackTrigger ControlID="btnPestanaDiesel" />
                                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                        <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                        <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                        <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </asp:View>
                    <asp:View ID="vwDiesel1" runat="server">
                        <div class="seccion_controles">
                            <div class="header_seccion">
                                <img src="../Image/Requerimiento.png" />
                                <h2>Datos del Tipo de Unidad</h2>
                            </div>
                            <div class="columna2x">
                                <div class="renglon2x">
                                    <div class="etiqueta">
                                        <label for="ddlTiposUnidad">Tipo Unidad</label>
                                    </div>
                                    <div class="control2x">
                                        <asp:UpdatePanel ID="upddlTiposUnidad" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddlTiposUnidad" AutoPostBack="true" runat="server" TabIndex="10" CssClass="dropdown"></asp:DropDownList>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                                <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                                                <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                                <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                                                <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                                                <asp:AsyncPostBackTrigger ControlID="btnAceptarTipoUnidad" />
                                                <asp:AsyncPostBackTrigger ControlID="lkbCopiar" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                                <div class="renglon2x">
                                    <%--<div class="etiqueta" >
                                        <label for="ddlConfiguraciones">Configuración</label>
                                    </div>--%>
                                    <div class="control2x">
                                        <asp:UpdatePanel ID="upddlConfiguraciones" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddlConfiguraciones" AutoPostBack="true" Visible="false"  runat="server" TabIndex="21" CssClass="dropdown"></asp:DropDownList>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                                <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                                                <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                                <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                                                <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                                                <asp:AsyncPostBackTrigger ControlID="btnAceptarTipoUnidad" />
                                                <asp:AsyncPostBackTrigger ControlID="lkbCopiar" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                                <%--<div class="renglon2x">
                                    <div class="etiqueta">
                                        <label for="txtRendiemiento">Rendimiento</label>
                                    </div>
                                    <div class="control2x">
                                        <asp:UpdatePanel runat="server" ID="UpdatePanel3" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:TextBox runat="server" ID="TextBox1" MaxLength="50" TabIndex="23" CssClass="textbox validate[required,custom[positiveNumber]]"></asp:TextBox>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                                <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                                                <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                                <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                                                <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                                                <asp:AsyncPostBackTrigger ControlID="btnAceptarTipoUnidad" />
                                                <asp:AsyncPostBackTrigger ControlID="lkbCopiar" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>--%>
                                <div class="renglon2x">
                                    <div class="controlBoton">
                                        <asp:UpdatePanel ID="upbtnAceptarTiposUnidad" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Button ID="btnAceptarTiposUnidad" TabIndex="24" runat="server" OnClick="btnAceptarTiposUnidad_Click" CssClass="boton" Text="Aceptar" />
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                                <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                                                <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                                <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                                                <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                                                <asp:AsyncPostBackTrigger ControlID="lkbCopiar" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                            <div class="columna2x">
                                <div class="grid_seccion_completa_200px_altura">
                                    <asp:UpdatePanel ID="upgvTiposUnidad" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:GridView ID="gvTiposUnidad" runat="server" PageSize="10" OnPageIndexChanging="gvTiposUnidad_PageIndexChanging" AutoGenerateColumns="False" OnRowDataBound="gvTiposUnidad_RowDataBound"
                                                ShowFooter="True" CssClass="gridview" Width="100%" AllowPaging="True" AllowSorting="True">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Tipo Unidad" SortExpression="TipoUnidad">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkSelecciona" runat="server" TabIndex="25" OnClick="lnkAccionTiposUnidad_Click" Text='<%#Eval("TipoUnidad") %>' CommandName="Seleccionar"></asp:LinkButton>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <%--<asp:BoundField DataField="Configuracion" HeaderText="Configuración" SortExpression="Configuracion" />--%>
                                                    <%--<asp:BoundField DataField="Rendimiento" HeaderText="Rendimiento" SortExpression="Rendimiento" />--%>
                                                    <asp:TemplateField HeaderText="" SortExpression="Deshabilitar">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkDeshabilita" runat="server" TabIndex="26" OnClick="lnkAccionTiposUnidad_Click" Text="Deshabilitar" CommandName="Deshabilitar"></asp:LinkButton>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="" SortExpression="Bitacora">
                                                        <ItemTemplate>
                                                            <asp:UpdatePanel ID="uplnkBitacoras" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:LinkButton ID="lnkBitacoras" runat="server" TabIndex="27" OnClick="lnkAccionTiposUnidad_Click" Text="Bitácora" CommandName="Bitacora"></asp:LinkButton>
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:PostBackTrigger ControlID="lnkBitacoras" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <AlternatingRowStyle CssClass="gridviewrowalternate" />
                                                <FooterStyle CssClass="gridviewfooter" />
                                                <HeaderStyle CssClass="gridviewheader" />
                                                <RowStyle CssClass="gridviewrow" />
                                                <SelectedRowStyle CssClass="gridviewrowselected" />
                                                <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                                                <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                                            </asp:GridView>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnAceptarTiposUnidad" />
                                            <asp:AsyncPostBackTrigger ControlID="lkbCopiar" />
                                            <asp:AsyncPostBackTrigger ControlID="btnPestanaDiesel1" />
                                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                            <div class="header_seccion">
                                <h2>Vales</h2>
                            </div>
                            <div class="renglon3x">
                                <div class="etiqueta_50px">
                                    <label for="ddlTamanoVales">Mostrar:</label>
                                </div>
                                <div class="control">
                                    <asp:UpdatePanel ID="upddlTamanoVale" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="ddlTamanoVale" runat="server" CssClass="dropdown" TabIndex="28"
                                                OnSelectedIndexChanged="ddlTamanoVale_SelectedIndexChanged" AutoPostBack="true">
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <div class="etiqueta">
                                    <label>Ordenado Por:</label>
                                </div>
                                <div class="etiqueta">
                                    <asp:UpdatePanel ID="uplblOrdenadoVale" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="lblOrdenadoVale" runat="server"></asp:Label>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="gvVale" EventName="Sorting" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                                <div class="etiqueta">
                                    <asp:UpdatePanel ID="uplkbExportaVales" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:LinkButton ID="lkbExportaVales" runat="server" TabIndex="29"
                                                OnClick="lkbExportaVales_Click" Text="Exportar"></asp:LinkButton>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="lkbExportaVales" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                            <div class="grid_seccion_completa_altura_variable">
                                <asp:UpdatePanel ID="upgvVale" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView ID="gvVale" runat="server" TabIndex="30" AutoGenerateColumns="False" OnSorting="gvVale_Sorting" OnPageIndexChanging="gvVale_PageIndexChanging" OnRowDataBound="gvVale_RowDataBound"
                                            ShowFooter="True" CssClass="gridview" Width="100%" AllowPaging="True" AllowSorting="True" PageSize="10">
                                            <Columns>
                                                <%--<asp:TemplateField HeaderText="Litros" SortExpression="Litros">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="TextBox2" MaxLength="18" runat="server" CssClass="textbox scriptLitrosE validate[required,custom[positiveNumber]]">
                                                        </asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label2" runat="server" Text='<%#Eval("Litros") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="TextBox3" MaxLength="18" runat="server" CssClass="textbox scriptLitros validate[required,custom[positiveNumber]]">
                                                        </asp:TextBox>
                                                    </FooterTemplate>
                                                </asp:TemplateField>--%>
                                                <asp:TemplateField HeaderText="Tipo Operación" SortExpression="TipoOperacion">
                                                    <EditItemTemplate>
                                                        <asp:DropDownList runat="server" ID="ddlTiposOperacionE" AutoPostBack="true" OnSelectedIndexChanged="ddlTiposOperacionE_SelectedIndexChanged" CssClass="dropdown">
                                                        </asp:DropDownList>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTiposOperacion" runat="server" Text='<%#Eval("TipoOperacion") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:DropDownList runat="server" ID="ddlTiposOperacion" AutoPostBack="true" OnSelectedIndexChanged="ddlTiposOperacion_SelectedIndexChanged" CssClass="dropdown">
                                                        </asp:DropDownList>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Estación de Combustible" SortExpression="UbicacionEstacion">
                                                    <EditItemTemplate>
                                                        <asp:TextBox runat="server" ID="txtEstacionesE" CssClass="textbox2x validate[required, custom[IdCatalogo]]"></asp:TextBox>
                                                        <asp:TextBox runat="server" ID="txtProveedoresE" CssClass="textbox2x validate[required, custom[IdCatalogo]]"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEstaciones" runat="server" Text='<%#Eval("UbicacionEstacion") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox runat="server" ID="txtEstaciones" CssClass="textbox2x validate[required, custom[IdCatalogo]]"></asp:TextBox>
                                                        <asp:TextBox runat="server" ID="txtProveedores" CssClass="textbox2x validate[required, custom[IdCatalogo]]"></asp:TextBox>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <EditItemTemplate>
                                                        <asp:LinkButton ID="lnkGuardaE" runat="server" OnClick="lkbAccionVales_Click" CssClass="textbox" CommandName="GuardarE" Text="Guardar"></asp:LinkButton>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkEdita" runat="server" OnClick="lnkGuardaE_Click" CommandName="Editar" Text="Editar"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:LinkButton ID="lnkInserta" runat="server" TabIndex="31" CssClass="textbox" Text="Insertar" CommandName="Insertar" OnClick="lnkInserta_Click"></asp:LinkButton>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <EditItemTemplate>
                                                        <asp:LinkButton ID="lnkCancelaE" runat="server" OnClick="lkbAccionVales_Click" CssClass="textbox" CommandName="CancelarE" Text="Cancelar"></asp:LinkButton>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkDeshabilit" TabIndex="32" runat="server" OnClick="lkbAccionVales_Click" Text="Deshabilitar" CommandName="Deshabilitar"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkBitacor" TabIndex="33" runat="server" OnClick="lkbAccionVales_Click" Text="Bitácora" CommandName="Bitacora"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <AlternatingRowStyle CssClass="gridviewrowalternate" />
                                            <FooterStyle CssClass="gridviewfooter" />
                                            <HeaderStyle CssClass="gridviewheader" />
                                            <RowStyle CssClass="gridviewrow" />
                                            <SelectedRowStyle CssClass="gridviewrowselected" />
                                            <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                                            <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                                        </asp:GridView>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddlTamanoVale" />
                                        <asp:AsyncPostBackTrigger ControlID="gvTiposUnidad" />
                                        <asp:AsyncPostBackTrigger ControlID="btnAceptarTiposUnidad" />
                                        <asp:AsyncPostBackTrigger ControlID="lkbCopiar" />
                                        <asp:AsyncPostBackTrigger ControlID="btnPestanaDiesel1" />
                                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                        <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                        <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                        <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                                        <%--<asp:AsyncPostBackTrigger ControlID="gvVale" />--%>
                                        <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </asp:View>
                    <asp:View ID="vwConcepto" runat="server">
                        <div class="header_seccion">
                            <img src="../Image/Requerimiento.png" />
                            <h2>Datos del  Concepto</h2>
                        </div>
                        <div class="renglon3x">
                            <div class="etiqueta_50px">
                                <label for="ddlTamanoVales">Mostrar:</label>
                            </div>
                            <div class="control">
                                <asp:UpdatePanel ID="upddlTamanoConcepto" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlTamanoConcepto" runat="server" CssClass="dropdown" TabIndex="28"
                                            OnSelectedIndexChanged="ddlTamanoConcepto_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="etiqueta">
                                <label>Ordenado Por:</label>
                            </div>
                            <div class="etiqueta">
                                <asp:UpdatePanel ID="uplblOrdenadoConcepto" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblOrdenadoConcepto" runat="server"></asp:Label>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="gvConcepto" EventName="Sorting" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                            <div class="etiqueta">
                                <asp:UpdatePanel ID="uplkbExportarConcepto" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:LinkButton ID="lkbExportarConcepto" runat="server" TabIndex="29"
                                            OnClick="lkbExportarConcepto_Click" Text="Exportar"></asp:LinkButton>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="lkbExportarConcepto" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <div class="grid_seccion_completa_altura_variable">
                            <asp:UpdatePanel ID="upgvConcepto" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:GridView ID="gvConcepto" runat="server" TabIndex="30" AutoGenerateColumns="False" OnSorting="gvConcepto_Sorting" OnPageIndexChanging="gvConcepto_PageIndexChanging" OnRowDataBound="gvConcepto_RowDataBound"
                                        ShowFooter="True" CssClass="gridview" Width="100%" AllowPaging="True" AllowSorting="True" PageSize="10">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Tipo Monto" SortExpression="TipoMonto">
                                                <EditItemTemplate>
                                                    <asp:DropDownList runat="server" ID="ddlTipoMontoE" CssClass="dropdown">
                                                    </asp:DropDownList>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTipoMonto" runat="server" Text='<%#Eval("TipoMonto") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:DropDownList runat="server" ID="ddlTipoMonto" CssClass="dropdown">
                                                    </asp:DropDownList>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Concepto" SortExpression="Concepto">
                                                <EditItemTemplate>
                                                    <asp:DropDownList runat="server" ID="ddlConceptoE" CssClass="dropdown2x">
                                                    </asp:DropDownList>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblConcepto" runat="server" Text='<%#Eval("Concepto") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:DropDownList runat="server" ID="ddlConcepto" CssClass="dropdown2x">
                                                    </asp:DropDownList>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Monto" SortExpression="Monto">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtMontoE" MaxLength="18" runat="server" CssClass="textbox scriptMontoE validate[required,custom[positiveNumber]]">
                                                    </asp:TextBox>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMonto" runat="server" Text='<%#Eval("Monto") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="txtMonto" MaxLength="18" runat="server" CssClass="textbox scriptMonto validate[required,custom[positiveNumber]]">
                                                    </asp:TextBox>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="¿Comprobación?" SortExpression="BitComprobacion">
                                                <EditItemTemplate>
                                                    <asp:CheckBox ID="chkComprobacionE" runat="server"></asp:CheckBox>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblComprobacion" runat="server" Text='<%#Eval("BitComprobacion") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:CheckBox ID="chkComprobacion" runat="server"></asp:CheckBox>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <EditItemTemplate>
                                                    <asp:LinkButton ID="lnkGuardarE" runat="server" OnClick="lkbAccionConcepto_Click" CssClass="textbox scriptGuardarConceptoE" CommandName="GuardarE" Text="Guardar"></asp:LinkButton>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkEditar" runat="server" OnClick="lnkGuardarConceptoE_Click" CommandName="Editar" Text="Editar"></asp:LinkButton>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:LinkButton ID="lnkInsertar" runat="server" TabIndex="31" CssClass="textbox scriptGuardarConcepto" Text="Insertar" CommandName="Insertar" OnClick="lnkInsertarConcepto_Click"></asp:LinkButton>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <EditItemTemplate>
                                                    <asp:LinkButton ID="lnkCancelarE" runat="server" OnClick="lkbAccionConcepto_Click" CssClass="textbox" CommandName="CancelarE" Text="Cancelar"></asp:LinkButton>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkDeshabilitar" TabIndex="32" runat="server" OnClick="lkbAccionConcepto_Click" Text="Deshabilitar" CommandName="Deshabilitar"></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkBitacora" TabIndex="33" runat="server" OnClick="lkbAccionConcepto_Click" Text="Bitácora" CommandName="Bitacora"></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <AlternatingRowStyle CssClass="gridviewrowalternate" />
                                        <FooterStyle CssClass="gridviewfooter" />
                                        <HeaderStyle CssClass="gridviewheader" />
                                        <RowStyle CssClass="gridviewrow" />
                                        <SelectedRowStyle CssClass="gridviewrowselected" />
                                        <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                                        <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                                    </asp:GridView>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ddlTamanoConcepto" />
                                    <asp:AsyncPostBackTrigger ControlID="lkbCopiar" />
                                    <asp:AsyncPostBackTrigger ControlID="btnPestanaConcepto" />
                                    <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                    <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                    <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                                    <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                                    <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                                    <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </asp:View>                    
                </asp:MultiView>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnPestanaCasetas" />
                <asp:AsyncPostBackTrigger ControlID="btnPestanaDiesel" />
                <asp:AsyncPostBackTrigger ControlID="btnPestanaDiesel1" />
                <asp:AsyncPostBackTrigger ControlID="btnPestanaConcepto" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
