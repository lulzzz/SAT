<%@ Page Title="Tarifas" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="Tarifas.aspx.cs" Inherits="SAT.Tarifas.Tarifas" %>

<%@ Register Src="~/UserControls/wucClasificacion.ascx" TagName="WucClasificacion" TagPrefix="tectos" %>
<%@ Register Src="~/UserControls/wucCargoRecurrente.ascx" TagName="WucCargoRecurrente" TagPrefix="tectos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Estilos documentación de servicio -->
    <link href="../CSS/DocumentacionServicio.css" rel="stylesheet" />
    <link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
    <link href="../CSS/ControlesUsuario.css" type="text/css" rel="stylesheet" />
    <link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
    <!-- Estilos Autocomplete, Mascara y Validadores JQuery -->
    <link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
    <!-- Bibliotecas para Validación de formulario -->
    <script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
    <script type="text/javascript">
        //Obteniendo instancia actual de la página y añadiendo manejador de evento
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        //Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
        function EndRequestHandler(sender, args) {
            if (args.get_error() == undefined) {
                ConfiguraJQueryTarifa();
            }
        }
        //Creando Función para Configuración de jquery en la Forma Web
        function ConfiguraJQueryTarifa() {
            $(document).ready(function () {
                //Función de Validación
                var validacionTarifa = function (evt) {
                    var isValid1 = !$("#<%=txtDescripcion.ClientID%>").validationEngine('validate');
                    var isValid2 = !$("#<%=txtCliente.ClientID%>").validationEngine('validate');
                    var isValid3 = !$("#<%=txtTransportista.ClientID%>").validationEngine('validate');
                    var isValid4 = !$("#<%=txtValorUCargado.ClientID%>").validationEngine('validate');
                    var isValid5 = !$("#<%=txtValorUVacio.ClientID%>").validationEngine('validate');
                    var isValid6 = !$("#<%=txtFecIni.ClientID%>").validationEngine('validate');
                    var isValid7 = !$("#<%=txtFecFin.ClientID%>").validationEngine('validate');
                    //Devolviendo Resultados
                    return isValid1 && isValid2 && isValid3 && isValid4 && isValid5 && isValid6 && isValid7;
                }
                //Asignando Función de Validación al Evento Click de los Botones
                $("#<%=btnGuardar.ClientID%>").click(validacionTarifa);
                $("#<%=lkbGuardar.ClientID%>").click(validacionTarifa);
                //Función de Validación
                var validacionTarifaMatriz = function (evt) {
                    var isValid1 = !$("#<%=txtTarifaCargado.ClientID%>").validationEngine('validate');
        var isValid2 = !$("#<%=txtTarifaVacio.ClientID%>").validationEngine('validate');
        /**** Columna ****/
        var isValid3;
        var columna = $("#<%=ddlColumnas.ClientID%>").val();
        //Validando si es de Tipo Catalogo
    if (columna == 57 || columna == 60 || columna == 1069) {
        isValid3 = !$("#<%=txtCatCol.ClientID%>").validationEngine('validate');
}
    //Si es de otro tipo
else {
    isValid3 = !$("#<%=txtRotCol.ClientID%>").validationEngine('validate');
}
        /**** Fila ****/
        var isValid4;
        var fila = $("#<%=ddlFilas.ClientID%>").val();
        //Validando si es de Tipo Catalogo
    if (fila == 57 || fila == 60 || fila == 1069 || fila == 0) {
        isValid4 = !$("#<%=txtCatFila.ClientID%>").validationEngine('validate');
}
    //Si es de otro tipo
else {
    isValid4 = !$("#<%=txtRotFila.ClientID%>").validationEngine('validate');
}
        //Devolviendo Resultados
        return isValid1 && isValid2 && isValid3 && isValid4;
    }
                //Asignando Función de Validación al Evento Click de los Botones
                $("#<%=btnGuardarMatriz.ClientID%>").click(validacionTarifaMatriz);

                var validaColumna = function (evt) {
                    /**** Columna ****/
                    var isValid;
                    var columna = $("#<%=ddlColumnas.ClientID%>").val();
        //Validando si es de Tipo Catalogo
        if (columna == 57 || columna == 60 || columna == 1069 || columna == 0)
            isValid = !$("#<%=txtCatCol.ClientID%>").validationEngine('validate');
    //Si es de otro tipo
else
    isValid = !$("#<%=txtRotCol.ClientID%>").validationEngine('validate');
        return isValid;
    }
                //Validando que exista la Columna
                $("#<%=imgbtnQuitarColumna.ClientID%>").click(validaColumna);

                var validaFila = function (evt) {
                    /**** Fila ****/
                    var isValidFila;
                    var fila = $("#<%=ddlFilas.ClientID%>").val();
        //Validando si es de Tipo Catalogo
        if (fila == 57 || fila == 60 || fila == 1069 || fila == 0)
            isValidFila = !$("#<%=txtCatFila.ClientID%>").validationEngine('validate');
    //Si es de otro tipo
else
    isValidFila = !$("#<%=txtRotFila.ClientID%>").validationEngine('validate');
        return isValidFila;
    }
                //Validando que exista la Fila
                $("#<%=imgbtnQuitarFila.ClientID%>").click(validaFila);

                //Cargando Catalogo AutoCompleta
                $("#<%=txtCliente.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=15&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>' });
                //Cargando Controles de Fechas
                $("#<%=txtFecIni.ClientID%>").datetimepicker({
                    lang: 'es',
                    format: 'd/m/Y'
                });
                $("#<%=txtFecFin.ClientID%>").datetimepicker({
                    lang: 'es',
                    format: 'd/m/Y'
                });
                //Validando el Cambio del Indice del Control "Columnas"
                $("#<%=ddlColumnas.ClientID%>").change(function () {
                    //Ubicaciones
                    if (this.value == 57)
                        //Cargando Catalogo de Ubicaciones
                        $("#<%=txtCatCol.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=2&param=<%=TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:",1)%>'});
            //Producto
        else if (this.value == 60)
            //Cargando Catalogo de Producto
            $("#<%=txtCatCol.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=5&param=<%=TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:",1)%>'});
    //Ciudades
else if (this.value == 1069)
    //Cargando Catalogo de Ubicaciones
    $("#<%=txtCatCol.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=9&param=<%=TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:",1)%>' });
    //Ninguno
else if (this.value == 0)
    $("#<%=txtCatCol.ClientID%>").val("Ninguno ID:0");
    });
                //Validando el Cambio del Indice del Control "Filas"
                $("#<%=ddlFilas.ClientID%>").change(function () {
                    //Producto
                    if (this.value == 60)
                        //Cargando Catalogo de Producto
                        $("#<%=txtCatFila.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=5&param=<%=TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:",1)%>' });
            //Ubicaciones
        else if (this.value == 57)
            //Cargando Catalogo de Ubicaciones
            $("#<%=txtCatFila.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=2&param=<%=TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:",1)%>' });
    //Ciudades
else if (this.value == 1069)
    //Cargando Catalogo de Ubicaciones
    $("#<%=txtCatFila.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=9&param=<%=TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:",1)%>' });
    //Ninguno
else if (this.value == 0)
    $("#<%=txtCatFila.ClientID%>").val("Ninguno ID:0");
    });

                function CargaAutoComplete() {
                    var columna = $("#<%=ddlColumnas.ClientID%>").val();
        var fila = $("#<%=ddlFilas.ClientID%>").val();

        /** Cargando Valor de Columna **/
        //Producto
        if (columna == 60)
            //Cargando Catalogo de Producto
            $("#<%=txtCatCol.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=5&param=<%=TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:",1)%>' });
        //Ubicaciones
    else if (columna == 57)
        //Cargando Catalogo de Ubicaciones
        $("#<%=txtCatCol.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=2&param=<%=TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:",1)%>' });
    //Ciudades
else if (columna == 1069)
    //Cargando Catalogo de Ubicaciones
    $("#<%=txtCatCol.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=9&param=<%=TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:",1)%>'});
    //Ninguno
else if (columna == 0)
    $("#<%=txtCatCol.ClientID%>").val("Ninguno ID:0");

        //Producto
    if (fila == 60)
        //Cargando Catalogo de Producto
        $("#<%=txtCatFila.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=5&param=<%=TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:",1)%>'});
        //Ubicaciones
    else if (fila == 57)
        //Cargando Catalogo de Ubicaciones
        $("#<%=txtCatFila.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=2&param=<%=TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:",1)%>'});
    //Ciudades
else if (fila == 1069)
    //Cargando Catalogo de Ubicaciones
    $("#<%=txtCatFila.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=9&param=<%=TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:",1)%>'});
    //Ninguno
else if (fila == 0)
    $("#<%=txtCatFila.ClientID%>").val("Ninguno ID:0");
}
                //Invocando Funcion de Carga
                CargaAutoComplete();
            });
}
//Función de Configuración
ConfiguraJQueryTarifa();
    </script>
    <div id="encabezado_forma">
        <img src="../Image/tipocobro.png" />
        <h1>Tarifas de Cobro</h1>
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
            <asp:PostBackTrigger ControlID="lkbReferencias" />
        </Triggers>
    </asp:UpdatePanel>
    <div class="contenedor_controles">
        <div class="header_seccion">            
            <h2>Datos Principales</h2>
        </div>
        <div class="columna2x">
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtCompania">Compañia</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtCompania" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtCompania" runat="server" CssClass="textbox2x" Enabled="false"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon">
                <div class="etiqueta">
                    <label for="lblId">No Tarifa</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uplblId" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblId" runat="server"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
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
                <div class="etiqueta">
                    <label for="txtDescripcion">Nombre Tarifa</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtDescripcion" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtDescripcion" runat="server" TabIndex="1" CssClass="textbox2x validate[required]"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon">
                <div class="etiqueta">
                    <label for="ddlTarifa">Base de Cobro</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="upddlTarifaBase" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlTarifaBase" runat="server" TabIndex="2" CssClass="dropdown"></asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
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
                <div class="etiqueta">
                    <label for="txtCliente">Cliente</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtCliente" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtCliente" runat="server" TabIndex="3" CssClass="textbox2x validate[required, custom[IdCatalogo]]"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
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
                <div class="etiqueta">
                    <label for="ddlFilas">Valor Filas</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="upddlFilas" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlFilas" runat="server" TabIndex="4" CssClass="dropdown2x" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlFilas_SelectedIndexChanged">
                            </asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
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
                <div class="etiqueta">
                    <label for="ddlColumnas">Valor Columnas</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="upddlColumnas" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlColumnas" runat="server" TabIndex="5" CssClass="dropdown2x" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlColumnas_SelectedIndexChanged">
                            </asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
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
        <div class="columna2x">
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtTransportista">Transportista</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtTransportista" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtTransportista" runat="server" TabIndex="6" CssClass="textbox2x validate[required, custom[IdCatalogo]]" Enabled="false"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
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
                <div class="etiqueta">
                    <label for="txtValorUCargado">Monto Fijo</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtValorUCargado" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtValorUCargado" runat="server" TabIndex="7" CssClass="textbox2x validate[required, custom[positiveNumber]]"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
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
                <div class="etiqueta">
                    <label for="txtValorUVacio">Monto Fijo Sec</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtValorUVacio" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtValorUVacio" runat="server" TabIndex="8" CssClass="textbox2x validate[required, custom[positiveNumber]]"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
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
                <div class="etiqueta">
                    <label for="txtFecIni">Inicio Tarifa</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtFecIni" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtFecIni" runat="server" TabIndex="9" CssClass="textbox validate[required, custom[date]]" MaxLength="10"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
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
                <div class="etiqueta">
                    <label for="txtFecFin">Fin Tarifa</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtFecFin" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtFecFin" runat="server" TabIndex="10" CssClass="textbox validate[required, custom[date]]" MaxLength="10"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon">
                <div class="controlBoton">
                    <asp:UpdatePanel ID="upbtnGuardar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnGuardar" runat="server" TabIndex="11" Text="Guardar" CssClass="boton"
                                OnClick="btnGuardar_Click" />
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
                            <asp:Button ID="btnCancelar" runat="server" TabIndex="12" Text="Cancelar" CssClass="boton_cancelar"
                                OnClick="btnCancelar_Click" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon">
                <div class="control">
                    <asp:UpdatePanel ID="uplblError" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblError" runat="server" CssClass="label_error"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
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
    <section class="acordion">
        <div class="contenedor_controles">
            <input type="radio" name="secciones" id="rdbClasificacion" />
            <label for="rdbClasificacion">
                <img src="../Image/Clasificacion.png" />Clasificación de Tarifa</label>
            <article>
                <asp:UpdatePanel ID="upucClasificacion" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <tectos:WucClasificacion ID="ucClasificacion" runat="server" TabIndex="14" OnClickGuardar="ucClasificacion_ClickGuardar"
                            OnClickCancelar="ucClasificacion_ClickCancelar" Enabled="false" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                        <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                        <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                        <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                        <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                    </Triggers>
                </asp:UpdatePanel>
            </article>
        </div>
        <div class="contenedor_controles">
            <input type="radio" name="secciones" id="rdbDetalles" />
            <label for="rdbDetalles">
                <img src="../Image/Tabla.png" />Matriz de Tarifa</label>
            <article>
                <div class="columna2x">
                    <div class="renglon2x"></div>
                    <div class="renglon2x">
                        <div class="etiqueta">
                            <label for="ddlRotFila">Valor Fila</label>
                        </div>
                        <div class="control2x">
                            <asp:UpdatePanel ID="upddlRotFila" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtCatFila" runat="server" TabIndex="15" CssClass="textbox2x validate[required, custom[IdCatalogo]]" Visible="true"></asp:TextBox>
                                    <asp:TextBox ID="txtRotFila" runat="server" TabIndex="15" CssClass="textbox2x validate[required, custom[positiveNumber]]" Visible="false"></asp:TextBox>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnConfirmarEliminacion" />
                                    <asp:AsyncPostBackTrigger ControlID="btnGuardarMatriz" />
                                    <asp:AsyncPostBackTrigger ControlID="btnCancelarMatriz" />
                                    <asp:AsyncPostBackTrigger ControlID="btnEliminarMatriz" />
                                    <asp:AsyncPostBackTrigger ControlID="ddlFilas" />
                                    <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                    <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                    <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                                    <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                    <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                                    <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                                    <asp:AsyncPostBackTrigger ControlID="gvTarifamatriz" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                        <div class="validador">
                            <asp:UpdatePanel ID="upbtnQuitarFila" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:ImageButton ID="imgbtnQuitarFila" runat="server" OnClick="imgbtnQuitarFila_Click"
                                        TabIndex="16" ImageUrl="~/Image/minus.png" ToolTip="Quitar Todas las Filas" />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
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
                        <div class="etiqueta">
                            <label for="ddlColumnaMatriz">Valor Columna</label>
                        </div>
                        <div class="control2x">
                            <asp:UpdatePanel ID="upddlRotCol" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtCatCol" runat="server" TabIndex="17" CssClass="textbox2x validate[required, custom[IdCatalogo]]" Visible="true"></asp:TextBox>
                                    <asp:TextBox ID="txtRotCol" runat="server" TabIndex="17" CssClass="textbox2x validate[required, custom[positiveNumber]]" Visible="false"></asp:TextBox>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnConfirmarEliminacion" />
                                    <asp:AsyncPostBackTrigger ControlID="btnGuardarMatriz" />
                                    <asp:AsyncPostBackTrigger ControlID="btnCancelarMatriz" />
                                    <asp:AsyncPostBackTrigger ControlID="btnEliminarMatriz" />
                                    <asp:AsyncPostBackTrigger ControlID="ddlColumnas" />
                                    <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                    <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                    <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                                    <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                    <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                                    <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                                    <asp:AsyncPostBackTrigger ControlID="gvTarifamatriz" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                        <div class="validador">
                            <asp:UpdatePanel ID="upbtnQuitarColumna" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:ImageButton ID="imgbtnQuitarColumna" runat="server" OnClick="imgbtnQuitarColumna_Click"
                                        TabIndex="18" ImageUrl="~/Image/minus.png" ToolTip="Quitar Todas las Columnas" />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                    <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                    <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                                    <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                    <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                                    <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div class="renglon">
                        <div class="etiqueta">
                            <label for="txtTarifaCargado">Monto Cargado</label>
                        </div>
                        <div class="control">
                            <asp:UpdatePanel ID="uptxtTarifaCargado" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtTarifaCargado" runat="server" TabIndex="19" CssClass="textbox validate[required, custom[positiveNumber]]"></asp:TextBox>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnConfirmarEliminacion" />
                                    <asp:AsyncPostBackTrigger ControlID="btnGuardarMatriz" />
                                    <asp:AsyncPostBackTrigger ControlID="btnCancelarMatriz" />
                                    <asp:AsyncPostBackTrigger ControlID="btnEliminarMatriz" />
                                    <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                    <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                    <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                                    <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                    <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                                    <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                                    <asp:AsyncPostBackTrigger ControlID="gvTarifamatriz" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div class="renglon">
                        <div class="etiqueta">
                            <label for="txtTarifaVacio">Monto Vacio</label>
                        </div>
                        <div class="control">
                            <asp:UpdatePanel ID="uptxtTarifaVacio" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtTarifaVacio" runat="server" TabIndex="20" CssClass="textbox validate[required, custom[positiveNumber]]"></asp:TextBox>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnConfirmarEliminacion" />
                                    <asp:AsyncPostBackTrigger ControlID="btnGuardarMatriz" />
                                    <asp:AsyncPostBackTrigger ControlID="btnCancelarMatriz" />
                                    <asp:AsyncPostBackTrigger ControlID="btnEliminarMatriz" />
                                    <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                    <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                    <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                                    <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                    <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                                    <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                                    <asp:AsyncPostBackTrigger ControlID="gvTarifamatriz" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div class="renglon">
                        <asp:UpdatePanel ID="uplblErrorMatriz" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="lblErrorMatriz" runat="server" CssClass="label_error"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnConfirmarEliminacion" />
                                <asp:AsyncPostBackTrigger ControlID="btnGuardarMatriz" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelarMatriz" />
                                <asp:AsyncPostBackTrigger ControlID="btnEliminarMatriz" />
                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                                <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                                <asp:AsyncPostBackTrigger ControlID="gvTarifamatriz" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="renglon">
                        <div class="controlBoton">
                            <asp:UpdatePanel ID="upbtnGuardarMatriz" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Button ID="btnGuardarMatriz" runat="server" TabIndex="21" Text="Guardar" CssClass="boton"
                                        OnClick="btnGuardarMatriz_Click" />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                    <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                    <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                                    <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                    <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                                    <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                        <div class="controlBoton">
                            <asp:UpdatePanel ID="upbtnCancelarMatriz" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Button ID="btnCancelarMatriz" runat="server" TabIndex="22" Text="Cancelar" CssClass="boton_cancelar"
                                        OnClick="btnCancelarMatriz_Click" />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                    <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                    <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                                    <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                    <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                                    <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                        <div class="controlBoton">
                            <asp:UpdatePanel ID="upbtnEliminarMatriz" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Button ID="btnEliminarMatriz" runat="server" TabIndex="23" Text="Eliminar" CssClass="boton"
                                        OnClick="btnEliminarMatriz_Click" />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                    <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                    <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                                    <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                    <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                                    <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div class="renglon"></div>
                </div>
                <div class="columna2x">
                    <div class="renglon2x">
                        <div class="controlr">
                            <asp:UpdatePanel ID="uplnkVerMatriz" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:LinkButton ID="lnkVerMatriz" runat="server" Text="Ver Matriz" OnClick="lnkVerMatriz_Click"></asp:LinkButton>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div class="renglon2x">
                        <div class="control">
                            <asp:UpdatePanel ID="upddlTamanoReqDisp" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <label for="ddlTamanoReqDisp"></label>
                                    <asp:DropDownList ID="ddlTamanoReqDisp" runat="server" CssClass="dropdown" TabIndex="24" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlTamanoReqDisp_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                    <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                    <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                                    <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                    <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                                    <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                        <div class="etiqueta">
                            <label>Ordenado Por:</label>
                        </div>
                        <div class="etiqueta">
                            <asp:UpdatePanel ID="uplblOrdenar" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Label ID="lblOrdenar" runat="server"></asp:Label>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="gvTarifamatriz" EventName="Sorting" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                        <div class="etiqueta">
                            <asp:UpdatePanel ID="uplnkExportar" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:LinkButton ID="lnkExportar" runat="server" OnClick="lnkExportar_Click" Text="Exportar Excel" TabIndex="25"></asp:LinkButton>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="lnkExportar" />
                                    <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                    <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                    <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                                    <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                    <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                                    <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div>
                        <asp:UpdatePanel ID="upgvTarifamatriz" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:GridView ID="gvTarifamatriz" runat="server" TabIndex="26" AllowPaging="true" AllowSorting="true"
                                    OnPageIndexChanging="gvTarifamatriz_PageIndexChanging" OnSorting="gvTarifamatriz_Sorting"
                                    CssClass="gridview" AutoGenerateColumns="false" Width="97%" PageSize="5">
                                    <AlternatingRowStyle CssClass="gridviewrowalternate" />
                                    <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                                    <FooterStyle CssClass="gridviewfooter" />
                                    <HeaderStyle CssClass="gridviewheader" />
                                    <RowStyle CssClass="gridviewrow" />
                                    <SelectedRowStyle CssClass="gridviewrowselected" />
                                    <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                                    <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                                    <Columns>
                                        <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="false" />
                                        <asp:BoundField DataField="Fila" HeaderText="Fila" SortExpression="Fila" />
                                        <asp:BoundField DataField="Columna" HeaderText="Columna" SortExpression="Columna" />
                                        <asp:BoundField DataField="TarifaC" HeaderText="Tarifa Cargado" SortExpression="TarifaC" />
                                        <asp:BoundField DataField="TarifaV" HeaderText="Tarifa Vacio" SortExpression="TarifaV" />
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkEditarMatriz" runat="server" Text="Editar" OnClick="lnkEditarMatriz_Click"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnConfirmarEliminacion" />
                                <asp:AsyncPostBackTrigger ControlID="btnGuardarMatriz" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelarMatriz" />
                                <asp:AsyncPostBackTrigger ControlID="btnEliminarMatriz" />
                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                                <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                                <asp:AsyncPostBackTrigger ControlID="ddlTamanoReqDisp" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <asp:UpdatePanel ID="uppnlMatriz" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel ID="pnlMatriz" runat="server" Visible="false">
                            <div class="contenedor_controles">
                                <div id="reporteMatrizCargada" class="columna600px">
                                    <label>Tarifas Cargadas</label>
                                    <asp:UpdatePanel ID="upgvMatriz" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:GridView ID="gvMatrizCargada" runat="server" TabIndex="27" AllowPaging="false" AllowSorting="false"
                                                CssClass="gridview" AutoGenerateColumns="true" Width="97%">
                                                <AlternatingRowStyle CssClass="gridviewrowalternate" />
                                                <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                                                <FooterStyle CssClass="gridviewfooter" />
                                                <HeaderStyle CssClass="gridviewheader" />
                                                <RowStyle CssClass="gridviewrow" />
                                                <SelectedRowStyle CssClass="gridviewrowselected" />
                                                <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                                                <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                                                <Columns>
                                                </Columns>
                                            </asp:GridView>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="lnkVerMatriz" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                                <div id="reporteMatrizVacia" class="columna600px">
                                    <label>Tarifas Vacias</label>
                                    <asp:UpdatePanel ID="upgvMatrizVacia" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:GridView ID="gvMatrizVacia" runat="server" TabIndex="28" AllowPaging="false" AllowSorting="false"
                                                CssClass="gridview" AutoGenerateColumns="true" Width="97%">
                                                <AlternatingRowStyle CssClass="gridviewrowalternate" />
                                                <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                                                <FooterStyle CssClass="gridviewfooter" />
                                                <HeaderStyle CssClass="gridviewheader" />
                                                <RowStyle CssClass="gridviewrow" />
                                                <SelectedRowStyle CssClass="gridviewrowselected" />
                                                <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                                                <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                                                <Columns>
                                                </Columns>
                                            </asp:GridView>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="lnkVerMatriz" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="lnkVerMatriz" />
                        <asp:AsyncPostBackTrigger ControlID="btnGuardarMatriz" />
                    </Triggers>
                </asp:UpdatePanel>
            </article>
        </div>
    </section>
    <div class="seccion_controles">
        <div class="header_seccion">
            <img src="../Image/SignoPesos.png" />
            <h2>Cargos Recurrente</h2>
            <asp:UpdatePanel ID="uplblTipoCargoRecurrente" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <h2><asp:Label ID="lblTipoCargoRecurrente" runat="server"></asp:Label></h2>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnCancelarMatriz" />
                    <asp:AsyncPostBackTrigger ControlID="gvTarifamatriz" />
                    <asp:AsyncPostBackTrigger ControlID="btnEliminarMatriz" />
                    <asp:AsyncPostBackTrigger ControlID="btnConfirmarEliminacion" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div>
            <asp:UpdatePanel ID="upucCargoRecurrente" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <tectos:WucCargoRecurrente ID="ucCargoRecurrente" runat="server" TabIndex="29" Enabled="false"
                        OnClickGuardarCargoRecurrente="ucCargoRecurrente_ClickGuardarCargoRecurrente"
                        OnClickEliminarCargoRecurrente="ucCargoRecurrente_ClickEliminarCargoRecurrente" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnConfirmarEliminacion" />
                    <asp:AsyncPostBackTrigger ControlID="btnGuardarMatriz" />
                    <asp:AsyncPostBackTrigger ControlID="btnCancelarMatriz" />
                    <asp:AsyncPostBackTrigger ControlID="btnEliminarMatriz" />
                    <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                    <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                    <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                    <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                    <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                    <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                    <asp:AsyncPostBackTrigger ControlID="gvTarifamatriz" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <div id="contenedorVentanaConfirmacion" class="modal">
        <div id="ventanaConfirmacion" class="contenedor_ventana_confirmacion">
            <div class="header_seccion">
                <asp:UpdatePanel ID="uplblValor" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <img src="../Image/Exclamacion.png" />
                        <h2>Al eliminar el valor de la 
                        <asp:Label ID="lblValor" runat="server"></asp:Label> se quitaran todos los registros ligados a este.</h2><br />
                        <h2>¿Desea continuar?</h2>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="imgbtnQuitarFila" />
                        <asp:AsyncPostBackTrigger ControlID="imgbtnQuitarColumna" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="columna2x">
                <div class="renglon2x">
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnConfirmarEliminacion" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnConfirmarEliminacion" runat="server" OnClick="btnConfirmarEliminacion_Click" 
                                    CssClass="boton" Text="Aceptar" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="imgbtnQuitarFila" />
                                <asp:AsyncPostBackTrigger ControlID="imgbtnQuitarColumna" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnCancelarEliminacion" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnCancelarEliminacion" runat="server" OnClick="btnCancelarEliminacion_Click"
                                    CssClass="boton_cancelar" Text="Cancelar" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="imgbtnQuitarFila" />
                                <asp:AsyncPostBackTrigger ControlID="imgbtnQuitarColumna" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
