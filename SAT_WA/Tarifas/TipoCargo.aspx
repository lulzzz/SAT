<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="TipoCargo.aspx.cs" Inherits="SAT.Tarifas.TipoCargo" %>
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
                ConfiguraJQueryTipoCargo();
            }
        }
        //Creando Función para Configuración de jquery en la Forma Web
        function ConfiguraJQueryTipoCargo() {
            $(document).ready(function () {
                //Cargando autocompleta Id Clave SAT
                $("#<%=txtIdClaveSAT.ClientID%>").autocomplete({
                    source: '../WebHandlers/AutoCompleta.ashx?id=59&param=<%=txtIdClaveSAT.Text.ToString()%>'
                });
                //Cargando autocompleta ventana modal
                $("#<%=txtClave.ClientID%>").autocomplete({
                    source: '../WebHandlers/AutoCompleta.ashx?id=60&param=<%=txtClave.Text.ToString()%>',
                    appendTo: '#ventanaAltaClavePS'
                });
                //Función de Validación
                var validacionTipocargo = function (evt) {
                    var isValid1 = !$("#<%=txtDescripcion.ClientID%>").validationEngine('validate');
                    var isValid2 = !$("#<%=txtTasaIT.ClientID%>").validationEngine('validate');
                    var isValid3 = !$("#<%=txtTasaIR.ClientID%>").validationEngine('validate');
                    var isValid4 = !$("#<%=txtTasaImp1.ClientID%>").validationEngine('validate');
                    var isValid5 = !$("#<%=txtTasaImp2.ClientID%>").validationEngine('validate');
                    var isValid6 = !$("#<%=txtCtaContable.ClientID%>").validationEngine('validate');
                    var isValid7 = !$("#<%=txtIdClaveSAT.ClientID%>").validationEngine('validate');
    //Devolviendo Resultados
    return isValid1 && isValid2 && isValid3 && isValid4 && isValid5 && isValid6 && isValid7;
}
    //Asignando Función de Validación al Evento Click de los Botones
    $("#<%=btnAceptar.ClientID%>").click(validacionTipocargo);
    $("#<%=lkbGuardar.ClientID%>").click(validacionTipocargo);
});
}
//Función de Configuración
ConfiguraJQueryTipoCargo();
    </script>
    <div id="encabezado_forma">
        <img src="../Image/Facturacion.png" />
        <h1>Conceptos de Cobro</h1>
    </div>
    <asp:UpdatePanel ID="upMenuPrincipal" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <nav id="menuForma">
                <ul>
                    <li class="green">
                        <a href="#" class="fa fa-floppy-o"></a>
                        <ul>
                            <li><asp:LinkButton ID="lkbNuevo" runat="server" Text="Nuevo" OnClick="lkbElementoMenu_Click" CommandName="Nuevo" /></li>
                            <li><asp:LinkButton ID="lkbAbrir" runat="server" Text="Abrir" OnClick="lkbElementoMenu_Click" CommandName="Abrir" /></li>
                            <li><asp:LinkButton ID="lkbGuardar" runat="server" Text="Guardar" OnClick="lkbElementoMenu_Click" CommandName="Guardar" /></li>
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
            <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
            <asp:PostBackTrigger ControlID="lkbBitacora" />
            <asp:PostBackTrigger ControlID="lkbReferencias" />
        </Triggers>
    </asp:UpdatePanel>
    <div class="seccion_controles">
        <div class="header_seccion">
            <h2>Concepto de Cobro</h2>
        </div>
        <!--Controles de la columna izq.-->
        <div class="columna2x">
            <!--Descripción-->
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtDescripcion">Descripción</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtDescripcion" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtDescripcion" runat="server" TabIndex="1" CssClass="textbox2x validate[required]"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <!--Unidad de medida-->
            <div class="renglon">
                <div class="etiqueta">
                    <label for="ddlUnidadMedida">Unidad Medida</label>
                </div>
                <div class="control_100px">
                    <asp:UpdatePanel ID="upddlUnidadMedida" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlUnidadMedida" runat="server" CssClass="dropdown_100px" TabIndex="2"></asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <!--Tasas e importes-->
            <div class="renglon2x">
                <!--I. Tras.-->
                <div class="etiqueta">
                    <label for="ddlTipoImpTras">T.Imp.Trasladado</label>
                </div>
                <div class="control_100px">
                    <asp:UpdatePanel ID="upddlTipoImpTras" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlTipoImpTras" runat="server" CssClass="dropdown_100px" TabIndex="3"></asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <!--Tasa Tras.-->
                <div class="etiqueta">
                    <label for="txtTasaIT">Tasa Trasladado</label>
                </div>
                <div class="control_100px">
                    <asp:UpdatePanel ID="uptxtTasaIT" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtTasaIT" runat="server" TabIndex="4" CssClass="textbox_100px validate[required, custom[positiveNumber]]"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <!--I. Ret.-->
                <div class="etiqueta">
                    <label for="ddlTipoImpRet">T.Imp.Retenido</label>
                </div>
                <div class="control_100px">
                    <asp:UpdatePanel ID="upddlTipoImpRet" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlTipoImpRet" CssClass="dropdown_100px" runat="server" TabIndex="5"></asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <!--Tasa Ret.-->
                <div class="etiqueta">
                    <label for="txtTasaIR">Tasa Retención</label>
                </div>
                <div class="control_100px">
                    <asp:UpdatePanel ID="uptxtTasaIR" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtTasaIR" runat="server" TabIndex="6" CssClass="textbox_100px validate[required, custom[positiveNumber]]"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <!--Moneda-->
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="ddlMoneda">Moneda</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="upddlMoneda" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlMoneda" runat="server" CssClass="dropdown2x" TabIndex="7"></asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <!--Clave SAT-->
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtIdClaveSat">Clave SAT</label>
                </div>
                <div class="control_100px" style="width:60%">
                    <asp:UpdatePanel ID="uptxtIdClaveSAT" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtIdClaveSAT" runat="server" TabIndex="8" CssClass="textbox validate[required,custom[IdCatalogo]]" Width="100%"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>                
                <!--Link a ventana modal-->
                <div class="etiqueta" style="width:17%">
                    <asp:UpdatePanel ID="uplnkAgregarClave" runat ="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:LinkButton ID="lnkAgregarClave" runat="server" Text="Agregar Clave" CommandName ="DetalleVenta" OnClick="lnkAgregarClave_Click" TabIndex="20"></asp:LinkButton>
                        </ContentTemplate>
                        <Triggers>
                        </Triggers>
                    </asp:UpdatePanel>
                </div>         
            </div>
        </div>
        <!--Controles de la columna der.-->
        <div class="columna2x">
            <!--Cuenta contable-->
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtCtaContable">Cuenta Contable</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtCtaContable" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtCtaContable" TabIndex="9" runat="server" CssClass="textbox2x validate[required]"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <!--Tipo de Cargo-->
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="ddlTipoCargo">Tipo de Cargo</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="upddlTipoCargo" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlTipoCargo" runat="server" CssClass="dropdown2x" TabIndex="10"></asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <!--Tarifa base-->
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="ddlTarifaBase">Tarifa Base</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="upddlTarifaBase" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlTarifaBase" runat="server" CssClass="dropdown2x" TabIndex="11"></asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <!--Otro impuesto 1-->
            <div class="renglon">
                <div class="etiqueta">
                    <label for="txtTasaImp1">Otro Impuesto</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtTasaImp1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtTasaImp1" runat="server" TabIndex="12" CssClass="textbox_100px validate[custom[positiveNumber]]"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <!--Otro impuesto 2-->
            <div class="renglon">
                <div class="etiqueta">
                    <label for="txtTasaImp2">Otro Impuesto 2</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtTasaImp2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtTasaImp2" runat="server" TabIndex="13" CssClass="textbox_100px validate[custom[positiveNumber]]"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <!--Botones-->
            <div class="renglon_boton">
                <div class="controlBoton">
                    <asp:UpdatePanel ID="upbtnAceptar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnAceptar" runat="server" CssClass="boton" Text="Aceptar"
                                TabIndex="14" OnClick="btnAceptar_Click" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="controlBoton">
                    <asp:UpdatePanel ID="upbtnCancelar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnCancelar" runat="server" CssClass="boton_cancelar" Text="Cancelar"
                                TabIndex="15" OnClick="btnCancelar_Click" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>      
        </div>
    </div>
    <!--Ventana Agregar Clave Producto Servicio-->
    <!--1 Modal-->
    <div id="contenedorVentanaAltaClavePS" class="modal">
        <!--2 Contenedor-->
        <div id="ventanaAltaClavePS" class="contenedor_ventana_confirmacion_arriba">
            <!--Boton Cerrar-->
            <div class="boton_cerrar_modal">
                <asp:UpdatePanel ID="uplnkCerrar" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lnkCerrar" runat="server" Text="" OnClick="lnkCerrar_Click">
                            <img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="columna3x">
                <!--Contenido-->
                <div class="header_seccion">
                    <h2>Claves de Producto o Servicio</h2>
                </div>
                <div class="renglon3x" style="float:left;">
                    <!--Etiqueta "Clave o Descripción-->
                    <div class="etiqueta_50px">
                        <label for="txtClave">Clave: </label>
                    </div>
                    <!--TextBox "Clave o Descripción-->
                    <div class="control" style="width:254px">
                        <asp:UpdatePanel ID="uptxtClave" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtClave" TabIndex="16" runat="server" CssClass="textbox validate[required,custom[IdCatalogo]]"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnAgregarClave" />
                                <asp:AsyncPostBackTrigger ControlID="gvClaveSP" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <!--Botón Agregar-->
                    <div class="control" style="width:auto">
                        <asp:UpdatePanel ID="upbtnAgregarClave" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnAgregarClave" TabIndex="17" runat="server" CssClass="boton" Text="Agregar" OnClick="btnAgregarClave_Click" />
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <!--Label Ordenado Por-->
                    <div class="etiqueta" style="width:auto">
                        <label for="lblOrdenado">Orden:</label>
                    </div>
                    <!--Label que muestra el orden-->
                    <div class="etiqueta" style="width:100px">
                        <asp:UpdatePanel ID="uplblOrden" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <b><asp:Label ID="lblOrden" runat="server" Width="120px"></asp:Label></b>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvClaveSP" EventName="Sorting" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <!--Link Exportar-->
                    <div class="etiqueta_50px">
                        <asp:UpdatePanel ID="uplnkExportar" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:LinkButton ID="lnkExportar" TabIndex="18" runat="server" Text="Exportar" CommandName="ExportarClaveSP" OnClick="lnkExportar_Click"></asp:LinkButton>
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="lnkExportar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <!--Renglón de GridView-->
                <div class="grid_seccion_completa_200px_altura">
                    <asp:UpdatePanel ID="upgvClaveSP" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:GridView ID="gvClaveSP" runat="server" AllowSorting="true" PageSize="10" TabIndex="19"
                                CssClass="gridview" ShowFooter="true" OnSorting="gvClaveSP_Sorting"
                                AutoGenerateColumns="false" Width="100%">
                                <AlternatingRowStyle CssClass="gridviewrowalternate" />
                                <EmptyDataRowStyle BackColor="#fffff" ForeColor="#ff0000"/>
                                <FooterStyle CssClass="gridviewfooter" />
                                <HeaderStyle CssClass="gridviewheader" />
                                <RowStyle CssClass="gridviewrow" />
                                <SelectedRowStyle CssClass="gridviewrowselected" />
                                <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                                <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                                <Columns>
                                    <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="false" />
                                    <asp:BoundField DataField="IdValorCadena" HeaderText="Clave" SortExpression="IdValorCadena" />
                                    <asp:BoundField DataField="Descripcion" HeaderText="Descripción" SortExpression="Descripcion" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEditar" runat="server" Text="Editar" CommandName="Editar" OnClick="lnkActualizaClave_Click"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEliminar" runat="server" Text="Eliminar" CommandName="Eliminar" OnClick="lnkActualizaClave_Click"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="txtClave" />
                            <asp:AsyncPostBackTrigger ControlID="btnAgregarClave" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>    
</asp:Content>
