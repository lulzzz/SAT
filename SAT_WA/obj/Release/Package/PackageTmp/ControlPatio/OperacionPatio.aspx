<%@ Page Title="Operación de Patio" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="OperacionPatio.aspx.cs" Inherits="SAT.ControlPatio.OperacionPatio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../CSS/ControlPatio.css" rel="stylesheet" />
    <link href="../CSS/ControlEvidencias.css" rel="stylesheet" />
    <!-- Estilos Autocomplete, Mascara y Validadores JQuery -->
    <link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/jquery.jqzoom.css" rel="stylesheet" type="text/css" />
    <!-- Bibliotecas para Validación de formulario -->
    <script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
    <script src="../Scripts/jquery.jqzoom-core.js" type="text/javascript"></script>

    <style>
        .newb {
            margin: 1px;
            margin-top: 1px;
            padding: 0px;
            width: 55px;
            height: 45px;
            float: left;
            border-right: 2px solid #DDD;
        }

        .leyenda_indicauni {
            margin: 0px;
            /*margin-top:2px;*/
            margin-left: 5px;
            padding: 0px;
            width: 95%;
            height: 25px;
            float: left;
            font-family: 'Lucida Sans', 'Lucida Sans Regular', 'Lucida Grande', 'Lucida Sans Unicode', Geneva, Verdana, sans-serif;
            font-size: 15px;
            /*text-align:center;*/
            text-decoration: none;
            color: #AAA;
            -webkit-transition: all 300ms ease-in-out;
            -moz-transition: all 300ms ease-in-out;
            -o-transition: all 300ms ease-in-out;
            -ms-transition: all 300ms ease-in-out;
            transition: all 300ms ease-in-out;
        }

        .indicador:hover .leyenda_indicauni {
            color: #000;
            font-size: 13px;
            font-weight: bold;
        }

        .indicadorLnew:hover .leyenda_indicauni {
            color: #000;
            font-size: 13px;
            font-weight: bold;
        }

        .indicador_texto:hover .leyenda_indicauni {
            color: #000;
            font-size: 11px;
            font-weight: bold;
        }

        .leyenda_indicaunitwo {
            margin: 0px;
            /*margin-top:2px;*/
            margin-left: 35px;
            padding: 0px;
            width: 95%;
            height: 15px;
            float: left;
            font-family: 'Lucida Sans', 'Lucida Sans Regular', 'Lucida Grande', 'Lucida Sans Unicode', Geneva, Verdana, sans-serif;
            font-size: 15px;
            /*text-align:center;*/
            text-decoration: none;
            color: #AAA;
            -webkit-transition: all 300ms ease-in-out;
            -moz-transition: all 300ms ease-in-out;
            -o-transition: all 300ms ease-in-out;
            -ms-transition: all 300ms ease-in-out;
            transition: all 300ms ease-in-out;
        }

        .indicador:hover .leyenda_indicaunitwo {
            color: #000;
            font-size: 13px;
            font-weight: bold;
        }

        .indicadorLnew:hover .leyenda_indicaunitwo {
            color: #000;
            font-size: 13px;
            font-weight: bold;
        }

        .indicador_texto:hover .leyenda_indicaunitwo {
            color: #000;
            font-size: 13px;
            font-weight: bold;
        }

        .numero_indicaforan {
            margin: 0px;
            margin-left: 5px;
            padding: 0px;
            width: 120px;
            height: 50px;
            float: left;
            font-family: Impact;
            font-size: 40px;
            text-align: center;
            color: #000;
            -webkit-transition: all 300ms ease-in-out;
            -moz-transition: all 300ms ease-in-out;
            -o-transition: all 300ms ease-in-out;
            -ms-transition: all 300ms ease-in-out;
            transition: all 300ms ease-in-out;
        }

        .indicador:hover .numero_indicaforan {
            color: red;
            font-size: 35px;
        }

        .indicadorLnew:hover .numero_indicaforan {
            color: red;
            font-size: 35px;
        }

        .indicadorLnew {
            margin: 1px;
            margin-top: 2px;
            padding: 0px;
            width: 190px;
            height: 85px;
            float: left;
            border-right: 1px solid #DDD;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
    <script type="text/javascript">
        //Obteniendo instancia actual de la página y añadiendo manejador de evento
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        //Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
        function EndRequestHandler(sender, args) {
            if (args.get_error() == undefined) {
                ConfiguraOperacionPatio();
            }
        }
        //Función de Configuración
        function ConfiguraOperacionPatio() {   //Inicia Confugración
            $(document).ready(function () {
                //Cargando Control DateTimePicker
                $("#<%=txtFecha.ClientID%>").datetimepicker({
                    lang: 'es',
                    format: 'd/m/Y H:i'
                });

                //Cargando Función de Validación de Funciones del Control
                $("#<%=gvEntidades.ClientID%>").click(function (sender) {
                    //Declarando Variable de Retorno
                    var isValid = true;
                    //Validando el Control que produjo el Evento
                    if (sender.target.id.indexOf('lnkGuardar') != -1) {
                        //Validando si el Control "Anden" esta Visible
                        if ($("*[id$=gvEntidades] input[id$=txtAnden]")[0] != undefined) {
                            //Carga Validacion
                            isValid = !$("*[id$=gvEntidades] input[id$=txtAnden]").validationEngine('validate');
                            //alert("Anden");
                        }//Validando si el Control "Cajon" esta Visible
                        else if ($("*[id$=gvEntidades] input[id$=txtCajon]")[0] != undefined) {
                            //Carga Validacion
                            isValid = !$("*[id$=gvEntidades] input[id$=txtCajon]").validationEngine('validate');
                            //alert("Cajon");
                        }
                    }
                    //Devolviendo Resultado de validación
                    return isValid;
                });

                //Carga Catalogo Autocompleta 'Andenes'
                $('*[id$=gvEntidades] input[id$=txtAnden]').autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=22&param=' + $("#<%=ddlPatio.ClientID%>").val() });
                //Carga Catalogo Autocompleta 'Cajones'
                $('*[id$=gvEntidades] input[id$=txtCajon]').autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=23&param=' + $("#<%=ddlPatio.ClientID%>").val() });
            });
        }
        
                //Creando función para configuración de jquery en control de usuario
        function ConfiguraJQueryEventos() {
            $(document).ready(function () {
                //Función de validación de campos
                var validacionContacto = function (evt) {

                    //Validando Controles
                    var isValidP2 = !$("#<%=txtAndenCajon.ClientID%>").validationEngine('validate');
                    return isValidP2;
                };
                //Botón Guardar
                $("#<%=btnGuardarEvento.ClientID%>").click(validacionContacto);
            });
        }
        //Invocación Inicial de método de configuración JQuery
        ConfiguraJQueryEventos();
        //Invocando Función de Configuración
        ConfiguraOperacionPatio();

    </script>
    <div id="encabezado_forma">
        <img src="../Image/OperacionPatio.png" />
        <h1>Operación Patio</h1>
    </div>
    <div class="encabezado_acceso">
        <div class="renglon_encabezado_acceso">
            <div class="etiqueta">
                <label for="ddlPatio">Patio</label>
            </div>
            <div class="control2x">
                <asp:UpdatePanel ID="upddlPatio" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlPatio" runat="server" CssClass="dropdown2x"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlPatio_SelectedIndexChanged">
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="controlr">
                <label for="txtFecha">Fecha</label>
            </div>
        </div>
        <div class="control_fecha_acceso">
            <asp:UpdatePanel ID="uptxtFecha" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:TextBox ID="txtFecha" runat="server" CssClass="txtFecha_acceso validate[required, custom[dateTime24]]"
                        MaxLength="16"></asp:TextBox>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                    <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                    <asp:AsyncPostBackTrigger ControlID="gvEntidades" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <section class="fila_indicador">
        <a href="UnidadesDentro.aspx" class="indicador">
            <div class="numero_indicador">
                <asp:UpdatePanel runat="server" ID="upplblUnidades" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label runat="server" ID="lblUnidades"></asp:Label>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger EventName="SelectedIndexChanged" ControlID="ddlPatio" />
                        <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                        <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                        <asp:AsyncPostBackTrigger ControlID="gvEntidades" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="imagen_indicador">
                <img src="../Image/IndicadorUnidadesPatio.png" />
            </div>
            <div class="leyenda_indicador">
                Unidades en patio
            </div>
        </a>
        <a href="ReporteAndenesAhora.aspx" class="indicador">
            <div class="numero_indicador">
                <asp:UpdatePanel runat="server" ID="uplblCargando" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label runat="server" ID="lblCargando"></asp:Label>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger EventName="SelectedIndexChanged" ControlID="ddlPatio" />
                        <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                        <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                        <asp:AsyncPostBackTrigger ControlID="gvEntidades" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="imagen_indicador">
                <img src="../Image/IndicadorCajaCarga.png" />
            </div>
            <div class="leyenda_indicador">
                Unidades Cargando
            </div>
        </a>
        <a href="ReporteAndenesAhora.aspx" class="indicadorL">
            <div class="numero_indicador">
                <asp:UpdatePanel runat="server" ID="upplblDescargando" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label runat="server" ID="lblDescargando"></asp:Label>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger EventName="SelectedIndexChanged" ControlID="ddlPatio" />
                        <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                        <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                        <asp:AsyncPostBackTrigger ControlID="gvEntidades" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="imagen_indicador">
                <img src="../Image/IndicadorCajaDescarga.png" />
            </div>
            <div class="leyenda_indicador">
                Unidades Descargando
            </div>
        </a>
        <a href="ReporteAndenesAhora.aspx" class="indicadorLnew">
            <div class="leyenda_indicaunitwo">
                Andenes
            </div>
            <div class="numero_indicaforan">
                <asp:UpdatePanel runat="server" ID="uplblAnden" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label runat="server" ID="lblTotalAnden"></asp:Label>
                        <div class="newb">
                            <asp:Label runat="server" ID="lblAnden"></asp:Label>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger EventName="SelectedIndexChanged" ControlID="ddlPatio" />
                        <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                        <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                        <asp:AsyncPostBackTrigger ControlID="gvEntidades" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="imagen_indicador">
                <img src="../Image/AndenLogistico.png" />
            </div>
            <div class="leyenda_indicauni">
                Disponibles Totales
            </div>
        </a>

        <a href="ReporteCajones.aspx" class="indicadorL">
            <div class="numero_indicador">
                <asp:UpdatePanel runat="server" ID="uplblEstacionada" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label runat="server" ID="lblEstacionada"></asp:Label>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger EventName="SelectedIndexChanged" ControlID="ddlPatio" />
                        <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                        <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                        <asp:AsyncPostBackTrigger ControlID="gvEntidades" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="imagen_indicador">
                <img src="../Image/IndicadorCajaEstaciona.png" />
            </div>
            <div class="leyenda_indicador">
                Unidades Estacionadas
            </div>
        </a>
        <a href="ReporteCajones.aspx" class="indicadorLnew">
            <div class="leyenda_indicaunitwo">
                Cajones
            </div>
            <div class="numero_indicaforan">
                <asp:UpdatePanel runat="server" ID="uplblCajon" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label runat="server" ID="lblTotalCajon"></asp:Label>
                        <div class="newb">
                            <asp:Label runat="server" ID="lblCajon"></asp:Label>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger EventName="SelectedIndexChanged" ControlID="ddlPatio" />
                        <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                        <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                        <asp:AsyncPostBackTrigger ControlID="gvEntidades" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="imagen_indicador">
                <img src="../Image/IndicadorCajones.png" />
            </div>
            <div class="leyenda_indicauni">
                Disponibles Totales
            </div>
        </a>
    </section>
    <div class="seccion_controles">
        <div class="header_seccion">
            <img src="../Image/OperacionPatio2.png" />
            <h2>Operación de Patio</h2>
            <div class="controlr">
                <asp:UpdatePanel ID="uplnkMapa" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton runat="server" ID="lnkMapa" Text="Mostrar Mapa" OnClick="lnkMapa_Click">
<img src="../Image/ImagenPatio.png" />
Mostrar Patio
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon3x">
            <asp:Panel ID="pnlBuscar" runat="server" DefaultButton="btnBuscar">
                <div class="etiqueta">
                    <label for="txtNoUnidad">No. Unidad</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtNoUnidad" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtNoUnidad" runat="server" TabIndex="1" CssClass="textbox_100px"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers></Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="etiqueta">
                    <label for="txtPlacas">Placas</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtPlacas" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtPlacas" runat="server" TabIndex="2" CssClass="textbox_100px"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers></Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="controlBoton">
                    <asp:UpdatePanel ID="upbtnBuscar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="boton" TabIndex="3"
                                OnClick="btnBuscar_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </asp:Panel>
        </div>
        <div class="grid_operacion_patio">
            <div class="renglon3x">
                <div class="etiqueta">
                    <label for="ddlTamano">Mostrar:</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="upddlTamano" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlTamano" runat="server" CssClass="dropdown" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlTamano_SelectedIndexChanged">
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="etiqueta">
                    <label for="lblOrdenado">Ordenado:</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uplblOrdenado" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <b>
                                <asp:Label ID="lblOrdenado" runat="server"></asp:Label></b>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="gvEntidades" EventName="Sorting" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>

                <asp:UpdatePanel ID="uplnkExportar" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lnkExportar" runat="server" Text="Exportar"
                            OnClick="lnkExportar_Click"></asp:LinkButton>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="lnkExportar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <asp:UpdatePanel ID="upgvEntidades" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="gvEntidades" runat="server" AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true"
                        OnSorting="gvEntidades_Sorting" Sorted="*Estancia ASC" OnPageIndexChanging="gvEntidades_PageIndexChanging"
                        OnRowDataBound="gvEntidades_RowDataBound"
                        PageSize="250" Width="100%" CssClass="gridview">
                        <AlternatingRowStyle CssClass="gridviewrowalternate" />
                        <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                        <FooterStyle CssClass="gridviewfooter" />
                        <HeaderStyle CssClass="gridviewheader" />
                        <RowStyle CssClass="gridviewrow" />
                        <SelectedRowStyle CssClass="gridviewrowselected" />
                        <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                        <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                        <Columns>
                            <asp:TemplateField HeaderText="Estatus" SortExpression="*Estancia1">
                                <ItemTemplate>
                                    <asp:Image ID="imgEstatus" runat="server" Height="20px" ImageAlign="AbsMiddle" Width="20px"
                                        ImageUrl="~/Image/semaforo_verde.png" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="false" />
                            <asp:TemplateField HeaderText="Tipo" SortExpression="Tipo">
                                <ItemTemplate>
                                    <asp:Label ID="lblTipo" runat="server" Text='<%# Eval("Tipo") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblTipoEdit" runat="server" Text='<%# Eval("Tipo") %>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="No. Unidad" SortExpression="NoUnidad">
                                <ItemTemplate>
                                    <asp:Label ID="lblNoUnidad" runat="server" Text='<%# Eval("NoUnidad") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblNoUnidadEdit" runat="server" Text='<%# Eval("NoUnidad") %>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Estancia en Patio" SortExpression="*Estancia">
                                <ItemTemplate>
                                    <asp:Label ID="lblEstancia" runat="server" Text='<%# Eval("Estancia") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblEstanciaEdit" runat="server" Text='<%# Eval("Estancia") %>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Estado" SortExpression="Estado">
                                <ItemTemplate>
                                    <asp:Label ID="lblOperacion" runat="server" Text='<%# Eval("Estado") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlOperacion" runat="server" CssClass="dropdown" OnSelectedIndexChanged="ddlOperacion_SelectedIndexChanged"
                                        AutoPostBack="true" TabIndex="6">
                                    </asp:DropDownList>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="IdEvt" HeaderText="IdEvt" SortExpression="IdEvt" Visible="false" />
                            <asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" Visible="false" />
                            <asp:TemplateField HeaderText="Anden/Cajon" SortExpression="AndenCajon">
                                <ItemTemplate>
                                    <asp:Label ID="lblAndenCajon" runat="server" Text='<%# Eval("AndenCajon") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtAnden" runat="server" TabIndex="7" CssClass="textbox_100px txtAndenes validate[required, custom[IdCatalogo]]" Text='<%# Eval("AndenCajon") %>'></asp:TextBox>
                                    <asp:TextBox ID="txtCajon" runat="server" TabIndex="7" CssClass="textbox_100px txtCajones validate[required, custom[IdCatalogo]]" Text='<%# Eval("AndenCajon") %>'></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tiempo" SortExpression="*TiempoEvt1">
                                <ItemTemplate>
                                    <asp:Label ID="lblTiempoEvt" runat="server" CssClass="label_negrita" Text='<%# Eval("TiempoEvt") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblTiempoEvtEdit" runat="server" CssClass="label_negrita" Text='<%# Eval("TiempoEvt") %>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Operador Asignado" SortExpression="OpeIni">
                                <ItemTemplate>
                                    <asp:Label ID="lblOperador" runat="server" CssClass="label_negrita" Text='<%# Eval("OpeIni") %>' Visible="false"></asp:Label>
                                    <asp:Image ID="imgOperador" runat="server" Height="20px" Visible="false" ImageAlign="AbsMiddle" Width="20px"
                                        ImageUrl="~/Image/operador2.png" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Confirmación Operador" SortExpression="BitConfirmacionIni">
                                <ItemTemplate>
                                    <asp:Label ID="lblConfirmacionIni" runat="server" CssClass="label_negrita" Text='<%# Eval("BitConfirmacionIni") %>' Visible="false"></asp:Label>
                                    <asp:Image ID="imgConfirmacionIni" runat="server" Height="20px" Visible="false" ImageAlign="AbsMiddle" Width="20px" ImageUrl="~/Image/TripPending.png" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <%--<asp:BoundField SortExpression="FechaAsignacion" DataField="FechaAsignacion" HeaderText="Fecha Asignación" DataFormatString="{0:dd/MM/yyyy HH:mm}" />--%>
                            <asp:TemplateField HeaderText="FechaAsignacion" SortExpression="FechaAsignacion" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblFechaAsignacion" runat="server" CssClass="label_negrita" Text='<%# Eval("FechaAsignacion","{0:dd/MM/yyyy HH:mm}") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="FechaConfirmacion" SortExpression="FechaConfirmacion" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblFechaConfirmacion" runat="server" CssClass="label_negrita" Text='<%# Eval("FechaConfirmacion","{0:dd/MM/yyyy HH:mm}") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Inicio Estado Anden" SortExpression="InicioAnden">
                                <ItemTemplate>
                                    <asp:Label ID="lblInicioAnden" runat="server" CssClass="label_negrita" Text='<%# Eval("InicioAnden") %>' Visible="false"></asp:Label>
                                    <asp:Image ID="imgInicioAnden" runat="server" Height="20px" Visible="false" ImageAlign="AbsMiddle" Width="20px"
                                        ImageUrl="~/Image/TripPending.png" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Fin Estado Anden" SortExpression="FinAnden">
                                <ItemTemplate>
                                    <asp:Label ID="lblFinAnden" runat="server" CssClass="label_negrita" Text='<%# Eval("FinAnden") %>' Visible="false"></asp:Label>
                                    <asp:Image ID="imgFinAnden" runat="server" Height="20px" Visible="false" ImageAlign="AbsMiddle" Width="20px"
                                        ImageUrl="~/Image/TripPending.png" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:BoundField SortExpression="OpeFin" DataField="OpeFin" HeaderText="Operador Fin" Visible="false" />
                            <asp:TemplateField HeaderText="Confirmación Fin" SortExpression="BitConfirmacionFin" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblConfirmacionFin" runat="server" CssClass="label_negrita" Text='<%# Eval("BitConfirmacionFin") %>' Visible="false"></asp:Label>
                                    <asp:Image ID="imgConfirmacionFin" runat="server" Height="20px" Visible="false" ImageAlign="AbsMiddle" Width="20px"
                                        ImageUrl="~/Image/TripPending.png" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="FechaIniAnden" SortExpression="FechaIniAnden" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblFechaIniAnden" runat="server" CssClass="label_negrita" Text='<%# Eval("FechaIniAnden","{0:dd/MM/yyyy HH:mm}") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="FechaFinAnden" SortExpression="FechaFinAnden" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblFechaFinAnden" runat="server" CssClass="label_negrita" Text='<%# Eval("FechaFinAnden","{0:dd/MM/yyyy HH:mm}") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="" SortExpression="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="35px" ItemStyle-Width="35px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imbContinuar" runat="server" ToolTip="Iniciar Evento" Height="20px" ImageUrl="~/Image/Iniciar.png" CommandName="Continuar" OnClick="lnkContinuar_Click" TabIndex="4" Visible="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:ImageButton ID="imbEvento" runat="server" ToolTip="Terminar Evento Actual" Height="20px" ImageUrl="~/Image/Terminar.png" OnClick="lnkEvento_Click" TabIndex="5" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton ID="lnkGuardar" runat="server" Text="Guardar" CommandName="Guardar" OnClick="lnkAccionGridView_Click" TabIndex="8"></asp:LinkButton><br />
                                    <asp:LinkButton ID="lnkCancelar" runat="server" Text="Cancelar" CommandName="Cancelar" OnClick="lnkAccionGridView_Click" TabIndex="8"></asp:LinkButton>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkEvidencias" runat="server" OnClick="lnkEvidencias_Click" Visible="false">
<img src="../Image/ImagenEvidencia.png" width="20" height="20" />
                                    </asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton ID="ibEvidenciasEdit" runat="server" ToolTip="Ver Evidencias" ImageUrl="~/Image/ImagenEvidencia.png"
                                        Enabled="false" />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField SortExpression="Estancia" DataField="Estancia" HeaderText="Estancia" Visible="false" />
                            <asp:BoundField SortExpression="TiempoEvt" DataField="TiempoEvt" HeaderText="TiempoEvt" Visible="false" />
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ddlPatio" />
                    <asp:AsyncPostBackTrigger ControlID="ddlTamano" />
                    <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                    <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                    <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                    <asp:AsyncPostBackTrigger ControlID="lnkCerrarImagen" />
                    <asp:AsyncPostBackTrigger ControlID="btnGuardarEvento" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <div id="contenidoVentanaConfirmacion" class="modal">
        <div id="ventanaConfirmacion" class="contenedor_ventana_confirmacion">
            <div class="header_seccion">
                <img src="../Image/Exclamacion.png" />
                <h2>¿Desea terminar el Evento con esta Fecha?</h2>
                <br />
                <br />
                <br />
                <asp:UpdatePanel ID="uplblFecha" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <b>
                            <asp:Label ID="lblFecha" runat="server" Text=""></asp:Label></b>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvEntidades" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="columna2x">
                <div class="renglon2x">
                    <div class="control">
                        <asp:UpdatePanel ID="uplblError" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="lblError" runat="server" CssClass="label_error"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                <asp:AsyncPostBackTrigger ControlID="gvEntidades" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnAceptar" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" CssClass="boton"
                                    OnClick="btnAceptar_Click" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvEntidades" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnCancelar" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="boton_cancelar"
                                    OnClick="btnCancelar_Click" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvEntidades" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="mapa_patio" class="modal_mapa_patio">
        <div id="visualizacion_mapa_patio" class="contenedor_mapa_patio">
            <div class="cerrar_mapa">
                <asp:UpdatePanel runat="server" ID="uplnkCerrar" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lnkCerrar" runat="server" OnClick="lnkCerrar_Click" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="informacion_mapa">
                <div class="header_seccion">
                    <img src="../Image/ImagenPatio.png" />
                    <h2>Mapa Patio</h2>
                </div>
                <div class="etiqueta">
                    <label for="">Zona de Patio</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="upddlZonaPatio" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlZonaPatio" runat="server" CssClass="dropdown" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlZonaPatio_SelectedIndexChanged">
                            </asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlPatio" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="header_info_mapa">
                    <h3>Indicadores Patio</h3>
                </div>
                <div class="renglon_info_mapa">
                    <div class="indicador_info_mapa">
                        <asp:UpdatePanel runat="server" ID="uplblInfoMapa2" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label runat="server" ID="lblInfoMapa2"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="imgLayout" />
                                <asp:AsyncPostBackTrigger ControlID="lnkMapa" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="indicador_leyenda_info_mapa">Cargando</div>
                </div>
                <div class="renglon_info_mapa">
                    <div class="indicador_info_mapa">
                        <asp:UpdatePanel runat="server" ID="uplblInfoMapa3" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label runat="server" ID="lblInfoMapa3"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="imgLayout" />
                                <asp:AsyncPostBackTrigger ControlID="lnkMapa" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="indicador_leyenda_info_mapa">Descargando</div>
                </div>
                <div class="renglon_info_mapa">
                    <div class="indicador_info_mapa">
                        <asp:UpdatePanel runat="server" ID="uplblInfoMapa4" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label runat="server" ID="lblInfoMapa4"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="imgLayout" />
                                <asp:AsyncPostBackTrigger ControlID="lnkMapa" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="indicador_leyenda_info_mapa">Cargadas x Descarga</div>
                </div>
                <div class="renglon_info_mapa">
                    <div class="indicador_info_mapa">
                        <asp:UpdatePanel runat="server" ID="uplblInfoMapa5" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label runat="server" ID="lblInfoMapa5"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="imgLayout" />
                                <asp:AsyncPostBackTrigger ControlID="lnkMapa" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="indicador_leyenda_info_mapa">Cargadas x Salir</div>
                </div>
                <div class="renglon_info_mapa">
                    <div class="indicador_info_mapa">
                        <asp:UpdatePanel runat="server" ID="uplblInfoMapa6" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label runat="server" ID="lblInfoMapa6"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="imgLayout" />
                                <asp:AsyncPostBackTrigger ControlID="lnkMapa" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="indicador_leyenda_info_mapa">Vacias x Cargar</div>
                </div>
                <div class="renglon_info_mapa_total">
                    <div class="indicador_info_mapa">
                        <asp:UpdatePanel runat="server" ID="uplblInfoMapa7" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label runat="server" ID="lblInfoMapa7"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="imgLayout" />
                                <asp:AsyncPostBackTrigger ControlID="lnkMapa" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="indicador_leyenda_info_mapa">Vacias x Salir</div>
                </div>
                <div class="renglon_info_mapa">
                    <div class="indicador_info_mapa">
                        <asp:UpdatePanel runat="server" ID="uplblInfoMapa1" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label runat="server" ID="lblInfoMapa1"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="imgLayout" />
                                <asp:AsyncPostBackTrigger ControlID="lnkMapa" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="indicador_leyenda_info_mapa">Unidades en Patio</div>
                </div>
                <div class="header_info_mapa">
                    <h3>Datos Entidad</h3>
                </div>
                <div class="renglon_info_mapa">
                    <div class="entidad_info_mapa">
                        <asp:UpdatePanel runat="server" ID="uplblEntidad" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label runat="server" ID="lblEntidad"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="imgLayout" />
                                <asp:AsyncPostBackTrigger ControlID="lnkMapa" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon_info_mapa">
                    <div class="entidad_mas_info_mapa">
                        <asp:UpdatePanel runat="server" ID="uplblTiempo" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label runat="server" ID="lblTiempo"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="imgLayout" />
                                <asp:AsyncPostBackTrigger ControlID="lnkMapa" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon_info_mapa">
                    <div class="entidad_mas_info_mapa">
                        <asp:UpdatePanel runat="server" ID="uplblUnidad" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label runat="server" ID="lblUnidad"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="imgLayout" />
                                <asp:AsyncPostBackTrigger ControlID="lnkMapa" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon_info_mapa">
                    <div class="entidad_mas_info_mapa">
                        <asp:UpdatePanel runat="server" ID="uplblNoOperaciones" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label runat="server" ID="lblNoOperaciones"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="imgLayout" />
                                <asp:AsyncPostBackTrigger ControlID="lnkMapa" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon_info_mapa">
                    <div class="entidad_mas_info_mapa">
                        <asp:UpdatePanel runat="server" ID="uplblTiempoPromedio" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label runat="server" ID="lblTiempoPromedio"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="imgLayout" />
                                <asp:AsyncPostBackTrigger ControlID="lnkMapa" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon_info_mapa">
                    <div class="entidad_mas_info_mapa">
                        <asp:UpdatePanel runat="server" ID="uplblUtilizacion" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label runat="server" ID="lblUtilizacion"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="imgLayout" />
                                <asp:AsyncPostBackTrigger ControlID="lnkMapa" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>

            </div>
            <div class="contenedor_mapa">
                <asp:UpdatePanel ID="upimgLayout" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <asp:ImageButton ID="imgLayout" runat="server" ImageAlign="Middle" OnClick="imgLayout_Click" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlZonaPatio" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="ddlPatio" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                        <asp:AsyncPostBackTrigger ControlID="gvEntidades" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>

        </div>
    </div>
    <div id="contenidoVentanaEvidencias" class="modal">
        <div id="ventanaEvidencias" class="contenedor_modal_imagenes">
            <div class="cerrar_mapa">
                <asp:UpdatePanel runat="server" ID="uplnkCerrarImagen" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lnkCerrarImagen" runat="server" OnClick="lnkCerrarImagen_Click" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="contenedor_imagenes_patio">
                <div class="header_seccion">
                    <img src="../Image/Imagenes.png" />
                    <h2>Evidencias en Patio</h2>
                </div>
                <div class="visor_imagen">
                    <asp:UpdatePanel ID="uphplImagenZoom" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:HyperLink ID="hplImagenZoom" runat="server" CssClass="MYCLASS" NavigateUrl="~/Image/noDisponible.jpg" Height="150" Width="200">
                                <asp:Image ID="imgImagenZoom" runat="server" Height="150px" Width="200px" ImageUrl="~/Image/noDisponible.jpg" BorderWidth="1" BorderStyle="Dotted" BorderColor="Gray" />
                            </asp:HyperLink>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="imagenes">
                    <asp:UpdatePanel ID="updtlImagenImagenes" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DataList ID="dtlImagenImagenes" runat="server" RepeatDirection="Horizontal">
                                <ItemTemplate>
                                    <asp:UpdatePanel ID="uplkbThumbnailDoc" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:LinkButton ID="lkbThumbnailDoc" runat="server" CommandName='<%# Eval("URL") %>' OnClick="lkbThumbnailDoc_Click">
<img alt='<%# "ID: " + Eval("Id")  %>' src='<%# String.Format("../Accesorios/VisorImagenID.aspx?t_carga=archivo&t_escala=pixcel&alto=73&ancho=95&url={0}", Eval("URL")) %>' width="95" height="73" />
                                            </asp:LinkButton>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </ItemTemplate>
                            </asp:DataList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <!-- Ventana Modal Autorizar -->
    <div id="contenedorVentanaEvento" class="modal">
        <div id="ventanaEvento" class="contenedor_ventana_confirmacion">
            <div class="header_seccion">
                <img src="../Image/Exclamacion.png" />
                <h2>Continuar Evento</h2>
            </div>
            <div class="renglon2x">
                <div class="etiqueta_80px">
                    <label for="ddlTipo">Evento</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel runat="server" ID="upddlTipo" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList runat="server" ID="ddlTipo" CssClass="dropdown_200px" OnSelectedIndexChanged="ddlTipo_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="gvEntidades" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="control_80px">
                    <asp:UpdatePanel runat="server" ID="upchkAndenActual" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:CheckBox ID="chkAndenActual" runat="server" Text="¿Anden Actual?"
                                Checked="false" TabIndex="9" AutoPostBack="true" OnCheckedChanged="chkAndenActual_CheckedChanged"/>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="gvEntidades" />
                            <asp:AsyncPostBackTrigger ControlID="ddlTipo" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="btnGuardarEvento" />
                            <asp:AsyncPostBackTrigger ControlID="chkAndenActual" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>

            <div class="renglon2x" style="float: left">
                <div class="etiqueta_80px">
                    <label for="txtAndenCajon">Anden/Cajon </label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel runat="server" ID="uptxtAndenCajon" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox runat="server" ID="txtAndenCajon" CssClass="textbox2x validate[required, custom[IdCatalogo]]"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="gvEntidades" />
                            <asp:AsyncPostBackTrigger ControlID="ddlTipo" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="btnGuardarEvento" />
                            <asp:AsyncPostBackTrigger ControlID="chkAndenActual" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="controlBoton">
                    <asp:UpdatePanel runat="server" ID="upbtnGuardarEvento" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button runat="server" ID="btnGuardarEvento" Text="Guardar" CssClass="boton" OnClick="btnGuardarEvento_Click" />
                        </ContentTemplate>
                        <Triggers>
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="controlBoton">
                    <asp:UpdatePanel runat="server" ID="upbtnCancelarEvento" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button runat="server" ID="btnCancelarEvento" Text="Cancelar" CssClass="boton_cancelar" OnClick="btnCancelarEvento_Click" />
                        </ContentTemplate>
                        <Triggers>
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
