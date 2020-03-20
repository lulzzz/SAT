<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="Producto.aspx.cs" Inherits="SAT.Almacen.Producto" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!--Hoja de estilo que da formato a la página-->
    <link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
    <link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
    <!--Hoja de estlo para la validación de los controles-->
    <link href="../CSS/jquery.validationEngine.css" type="text/css" rel="stylesheet" />
    <!--Script que valida el contenido de los controles-->
    <script type="text/javascript" src="../Scripts/jquery.validationEngine.js"></script>
    <script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server"> 
    <!--Valida las inserciones de datos en los controles-->
    <script type ="text/javascript">
        //Obtiene la instancia actual de la pagina y añade un manejador de eventos
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        //Creación de la función que permite finalizar o validar los controles a partir de un error.
        function EndRequestHandler(sender, args) {
            //Valida si el argumento de error no esta definido
                if (args.get_error() == undefined) {
                    //Invoca a la Funcion ConfiguraJqueryProductoAlmacen
                    ConfiguraJQueryProductoAlmacen();
                }
        }
        //Declara la función que valida los controles de la pagina
        function ConfiguraJQueryProductoAlmacen() {
            $(document).ready(function () {
                //Creación  y asignación de la funcion a la variable ValidaProductoAlmacen
                var validaProductoAlmacen = function () {
                    //Creación de las variables y asignacion de los controles de la pagina ProductoAlmacen
                    var isValid1 = !$("#<%=txtDescripcion.ClientID%>").validationEngine('validate');
                    var isValid2 = !$("#<%=txtSKU.ClientID%>").validationEngine('validate');
                    var isValid3 = !$("#<%=txtGarantia.ClientID%>").validationEngine('validate');
                    var isValid4 = !$("#<%=txtPrecioEntrada.ClientID%>").validationEngine('validate');
                    var isValid5 = !$("#<%=txtPrecioSalida.ClientID%>").validationEngine('validate');
                    var isValid6 = !$("#<%=txtPrecioMayoreo.ClientID%>").validationEngine('validate');
                    var isValid6 = !$("#<%=txtPrecioMayoreo.ClientID%>").validationEngine('validate');
                    var isValid6 = !$("#<%=txtPrecioMayoreo.ClientID%>").validationEngine('validate');
                    var isValid7 = !$("#<%=txtCantidadMayoreo.ClientID%>").validationEngine('validate');
                    var isValid6 = !$("#<%=txtPrecioMayoreo.ClientID%>").validationEngine('validate');
                    var isValid8 = !$("#<%=txtCantidadMinima.ClientID%>").validationEngine('validate');
                    var isValid6 = !$("#<%=txtPrecioMayoreo.ClientID%>").validationEngine('validate');
                    var isValid9 = !$("#<%=txtCantidadMaxima.ClientID%>").validationEngine('validate');
                    var isValid6 = !$("#<%=txtPrecioMayoreo.ClientID%>").validationEngine('validate');
                    var isValid10 = !$("#<%=txtCantidadContenido.ClientID%>").validationEngine('validate');
                    //Devuelve un valor a la funcion
                    var isValid6 = !$("#<%=txtPrecioMayoreo.ClientID%>").validationEngine('validate');
                    return isValid1 && isValid2 && isValid3 && isValid4 && isValid5 && isValid6 && isValid7 && isValid8 && isValid9 && isValid10;
                };
                //Permite que los eventos de guardar activen la funcion de validación de controles.
                $("#<%=btnGuardar.ClientID%>").click(validaProductoAlmacen);
                $("#<%=lkbGuardar.ClientID%>").click(validaProductoAlmacen);    
                $("#<%=txtFabricante.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=42&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>' });
            });
        }
        ConfiguraJQueryProductoAlmacen();
    </script>
    <!--Fin validación de datos-->
    <div id="encabezado_forma">
<h1>Producto</h1>
</div>
    <!--Menú Principal-->
    <asp:UpdatePanel ID="upMenuPrincipal" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <nav id="menuForma">
                <ul>
                    <li class="green"><a href="#" class="fa fa-floppy-o"></a>
                        <ul>
                            <li>
                                <asp:LinkButton ID="lkbNuevo" runat="server" Text="Nuevo" OnClick="lkbElementoMenu_Click" CommandName="Nuevo" />
                            </li>
                            <li>
                                <asp:LinkButton ID="lkbAbrir" runat="server" Text="Abrir" OnClick="lkbElementoMenu_Click" CommandName="Abrir" />
                            </li>
                            <li>
                                <asp:LinkButton ID="lkbGuardar" runat="server" Text="Guardar" OnClick="lkbElementoMenu_Click" CommandName="Guardar" />

                            </li>
                        </ul>
                    </li>
                    <li class="red"><a href="#" class="fa fa-pencil-square-o"></a>
                        <ul>
                            <li>
                                <asp:LinkButton ID="lkbEditar" runat="server" Text="Editar" OnClick="lkbElementoMenu_Click" CommandName="Editar" />
                            </li>
                            <li>
                                <asp:LinkButton ID="lkbEliminar" runat="server" Text="Eliminar" OnClick="lkbElementoMenu_Click" CommandName="Eliminar" />
                            </li>
                        </ul>
                    </li>
                    <li class="blue"><a href="#" class="fa fa-cog"></a>
                        <ul>
                            <li>
                                <asp:LinkButton ID="lkbBitacora" runat="server" Text="Bitácora" OnClick="lkbElementoMenu_Click" CommandName="Bitacora" />
                            </li>
                            <li>
                                <asp:LinkButton ID="lkbReferencias" runat="server" Text="Referencias" OnClick="lkbElementoMenu_Click" CommandName="Referencias" />
                            </li>
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
    <!--Fin menú Principal-->
    <div class="contenedor_controles">
        <div class="header_seccion">
            <img src="../Image/Producto.png" /><h2>Descripción Producto</h2>
        </div>
        <div class="columna3x">
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="lblIdProducto"></label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel runat="server" ID="uplblIdProducto" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label runat="server" ID="lblIdProducto" CssClass="label_negrita" TabIndex="1" Visible="false"></asp:Label>
                        </ContentTemplate>
                        <Triggers>                        
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" /> 
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>

            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtCompaniaEmisor">Compañía:</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel runat="server" ID="uptxtCompaniaEmisor" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox runat="server" ID="txtCompaniaEmisor" CssClass="textbox2x" ></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>                        
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" /> 
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>

            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtSKU">SKU:</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel runat="server" ID="uptxtSKU" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox runat="server" ID="txtSKU" CssClass="textbox2x" TabIndex="1" MaxLength="75"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>                        
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />    
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>

            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtNoOrdenCompra">Descripción:</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel runat="server" ID="uptxtDescripcion" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox runat="server" ID="txtDescripcion" CssClass="textbox2x validate[required]" MaxLength="100" TabIndex="2"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>                        
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />   
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>

            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txFabricante">Fabricante:</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel runat="server" ID="uptxtFabricante" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox runat="server" ID="txtFabricante" CssClass="textbox  validate[custom[IdCatalogo]]" TabIndex="3" MaxLength="100"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>                        
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />    
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>

            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtGarantia">Garantía (Meses):</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel runat="server" ID="uptxtGarantia" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox runat="server" ID="txtGarantia" CssClass="textbox_50px validate[custom[integer]]" TabIndex="4" MaxLength="9"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>                        
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />   
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>

            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="ddlEstatus">Estatus:</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel runat="server" ID="upddlEstatus" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList runat="server" ID="ddlEstatus" CssClass="dropdown_100px " TabIndex="5"></asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>                        
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" /> 
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>

            <div class="renglon2x">
<div class="etiqueta">
<label for="ddlTipoMedida">Tipo Medida:</label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="upddlTipoMedida" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList runat="server" ID="ddlTipoMedida" CssClass="dropdown_100px" TabIndex="6" OnSelectedIndexChanged="ddlTipoMedida_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
</ContentTemplate>
<Triggers>                        
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />    
</Triggers>
</asp:UpdatePanel>
</div>
</div>

            <div class="renglon2x">
<div class="etiqueta">
<label for="ddlUnidadMedida">Unidad Medida:</label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="upddlUnidadMedida" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList runat="server" ID="ddlUnidadMedida" CssClass="dropdown_100px" TabIndex="7"></asp:DropDownList>
</ContentTemplate>
<Triggers>                        
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="ddlTipoMedida" EventName="SelectedIndexChanged" />        
</Triggers>
</asp:UpdatePanel>
</div>
</div>

            <div class="renglon2x">
<div class="etiqueta">
<label for="ddlCategoria">Categoría: </label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="upddlCategoria" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList runat="server" ID="ddlCategoria" OnSelectedIndexChanged="ddlCategoria_SelectedIndexChanged" CssClass="dropdown"  AutoPostBack="true" TabIndex="8"></asp:DropDownList>
</ContentTemplate>
<Triggers>                        
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />     
</Triggers>
</asp:UpdatePanel>
</div>
</div>

        </div>

        <div class="columna3x">
            <div class="renglon2x">
                <div class="etiqueta_320px">
                    <label class="label_negrita">Precios Producto</label>
                </div>
            </div>
            <div class="renglon3x">
                <div class="etiqueta">
                    <label for="txtPrecioEntrada">Precio Entrada:</label>
                </div>
                <div class="control_80px">
                    <asp:UpdatePanel runat="server" ID="uptxtPrecioEntrada" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox runat="server" ID="txtPrecioEntrada" CssClass="textbox_50px validate[custom[positiveNumber]]" TabIndex="9" MaxLength="9"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>                        
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" /> 
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="etiqueta_50px">
                    <label for="ddlMonedaEntrada">Moneda:</label>
                </div>
                <div class="control_60px">
                    <asp:UpdatePanel runat="server" ID="upddlMonedaEntrada" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList runat="server" ID="ddlMonedaEntrada" CssClass="dropdown_100px" TabIndex="10"></asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>                        
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>

<div class="renglon3x">
<div class="etiqueta">
<label for="txtPrecioSalida">Precio Salida:</label>
</div>
<div class="control_80px">
<asp:UpdatePanel runat="server" ID="uptxtPrecioSalida" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtPrecioSalida" CssClass="textbox_50px validate[custom[positiveNumber]]" TabIndex="11" MaxLength="9"></asp:TextBox>
</ContentTemplate>
<Triggers>                        
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" /> 
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<label for="ddlMonedaSalida">Moneda:</label>
</div>
<div class="control_60px">
<asp:UpdatePanel runat="server" ID="upddlMonedaSalida" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList runat="server" ID="ddlMonedaSalida" CssClass="dropdown_100px" TabIndex="12"></asp:DropDownList>
</ContentTemplate>
<Triggers>                        
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta_320px">
<label class="label_negrita">Producto por Mayoreo</label>
</div>
</div>
<div class="renglon3x">
<div class="etiqueta">
<label for="txtCantidadMayoreo">Cantidad:</label>
</div>
<div class="control_80px">
<asp:UpdatePanel runat="server" ID="uptxtCantidadMayoreo" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtCantidadMayoreo" CssClass="textbox_50px validate[custom[positiveNumber]]" MaxLength="9" TabIndex="13"></asp:TextBox>
</ContentTemplate>
<Triggers>                        
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" /> 
</Triggers>
</asp:UpdatePanel>
</div>

<div class="etiqueta_50px">
<label for="txtPrecioSalidaMayoreo">Precio:</label>
</div>
<div class="control_60px">
<asp:UpdatePanel runat="server" ID="uptxtPrecioMayoreo" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtPrecioMayoreo" CssClass="textbox_50px validate[custom[positiveNumber]]" MaxLength="9" TabIndex="14"></asp:TextBox>
</ContentTemplate>
<Triggers>                        
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" /> 
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta_320px">
<label class="label_negrita">Stock en almacen</label>
</div>
</div>
<div class=" renglon3x">
<div class="etiqueta">
<label for="txtCantidadMinima"> Mínimo:</label>
</div>
<div class="control_80px">
<asp:UpdatePanel runat="server" ID="uptxtCantidadMinima" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtCantidadMinima" CssClass="textbox_50px validate[custom[positiveNumber]]" MaxLength="9" TabIndex="15"></asp:TextBox>
</ContentTemplate>
<Triggers>                        
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" /> 
</Triggers>
</asp:UpdatePanel>
</div>

<div class="etiqueta_50px">
<label for="txtCantidadMaxima">Máximo:</label>
</div>
<div class="control_60px">
<asp:UpdatePanel runat="server" ID="uptxtCantidadMaxima" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtCantidadMaxima" CssClass="textbox_50px validate[custom[positiveNumber]]" MaxLength="9" TabIndex="16"></asp:TextBox>
</ContentTemplate>
<Triggers>                        
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" /> 
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<asp:UpdatePanel runat="server" ID="upchkProductoContenido">
<ContentTemplate>
<asp:CheckBox runat="server" ID="chkProductoContenido" Checked="false" OnCheckedChanged="chkProductoContenido_CheckedChanged" Text="¿Contiene Producto?" TabIndex="17" AutoPostBack="true"/>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" /> 
</Triggers>
</asp:UpdatePanel> 
</div>
<div class="renglon3x">
<div class="etiqueta">
<label for="txtProductoContenido">Producto:</label>
</div>
<div class="control_100px">
<asp:UpdatePanel runat="server" ID="uptxtProductoContenido" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtProductoContenido" CssClass="textbox2x validate[custom[IdCatalogo]] " TabIndex="18"></asp:TextBox>
</ContentTemplate>
<Triggers>                        
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" /> 
<asp:AsyncPostBackTrigger ControlID="chkProductoContenido" />
<asp:AsyncPostBackTrigger ControlID="ddlCategoria" />
</Triggers>
</asp:UpdatePanel>
</div>

</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtCantidadContenido">Cantidad:</label>
</div>
<div class="control_60px">
<asp:UpdatePanel runat="server" ID="uptxtCantidadContenido" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtCantidadContenido" CssClass="textbox_50px validate[custom[positiveNumber]]" MaxLength="9" TabIndex="19"></asp:TextBox>
</ContentTemplate>
<Triggers>                        
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" /> 
<asp:AsyncPostBackTrigger ControlID="chkProductoContenido" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>

<div class="renglon2x">
<asp:UpdatePanel runat="server" ID="upchkSinInventario">
<ContentTemplate>
<asp:CheckBox runat="server" ID="chkSinInventario" Checked="false" Text="¿Sin control de inventario?" TabIndex="20"/>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" /> 
</Triggers>
</asp:UpdatePanel> 
</div>

<div class="renglon2x">
    <div class="controlBoton">
        <asp:UpdatePanel runat="server" ID="upbtnCancelar" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Button runat="server" ID="btnCancelar" cssClass="boton_cancelar" Text="Cancelar" OnClick="btnCancelar_Click" TabIndex="21"/>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    
    <div class="controlBoton">
        <asp:UpdatePanel runat="server" ID="upbtnGuardar" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Button runat="server" ID="btnGuardar" CssClass="boton" Text="Guardar" OnClick="btnGuardar_Click" TabIndex="22" />
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</div>
</div>
    </div>
</asp:Content>
