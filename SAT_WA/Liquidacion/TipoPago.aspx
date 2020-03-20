<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="TipoPago.aspx.cs" Inherits="SAT.Liquidacion.TipoPago" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<!-- Estilos de la pagina-->
<link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
<link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
<!-- Estilo de validación de los controles-->
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" />
<!--Librerias para la validacion de los controles-->
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
    <!--Script que valida la insercion de datos en los controles-->
    <script type ="text/javascript">
        //Obtiene la instancia actual de la pagina y añade un manejador de eventos
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        //Creación de la función que permite finalizar o validar los controles a partir de un error.
        function EndRequestHandler(sender, args) {
            //Valida si el argumento de error no esta definido
            if (args.get_error() == undefined) {
                //Invoca a la Funcion ConfiguraJqueryTipoPago
                ConfiguraJQueryTipoPago();
            }
        }

        //Declara la función que valida los controles de la pagina
        function ConfiguraJQueryTipoPago()
        {
            $(document).ready(function () {
                //Creación  y asignación de la funcion a la variable ValidaTipoPago
                var validaTipoPago = function () {
                    //Creación de las variables y asignacion de los controles de la pagina TipoPago
                    var isValid1 = !$("#<%=txtDescripcion.ClientID%>").validationEngine('validate');
                    var isValid2 = !$("#<%=txtImpuestoTrasladado.ClientID%>").validationEngine('validate');
                    var isValid3 = !$("#<%=txtImpuestoRetenido.ClientID%>").validationEngine('validate')
                    var isValid4 = !$("#<%=txtCargoAdicional1.ClientID%>").validationEngine('validate');
                    var isValid5 = !$("#<%=txtCargoAdicional2.ClientID%>").validationEngine('validate');
                    //Devuelve un valor a la funcion
                    return isValid1 && isValid2 && isValid3 && isValid4 && isValid5;
                };
                //Permite que los eventos de guardar activen la funcion de validación de controles.
                $("#<%=btnGuardar.ClientID%>").click(validaTipoPago);
                $("#<%=lkbGuardar.ClientID%>").click(validaTipoPago);
            });
        }
        ConfiguraJQueryTipoPago();
    </script>
    <!--Fin Script-->

    <div id="encabezado_forma">
        <img src="../Image/Compania.png" />
        <h1>Tipo Pago</h1>
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
                                <asp:LinkButton ID="lkbNuevo" runat="server" Text="Nuevo" OnClick="lkbElementoMenu_Click" CommandName="Nuevo" /></li>
                            <li>
                                <asp:LinkButton ID="lkbAbrir" runat="server" Text="Abrir" OnClick="lkbElementoMenu_Click" CommandName="Abrir" /></li>
                            <li>
                                <asp:LinkButton ID="lkbGuardar" runat="server" Text="Guardar" OnClick="lkbElementoMenu_Click" CommandName="Guardar" /></li>
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
            <asp:PostBackTrigger ControlID="lkbAbrir" />
            <asp:PostBackTrigger ControlID="lkbReferencias" />
        </Triggers>
    </asp:UpdatePanel>
    <!--Fin menú principal-->

    <div class="seccion_controles">
        <div class="header_seccion">
            <img src="../Image/Depositos.png" />
            <h2>Descripcion Tipo de Pago</h2>
        </div>

        <div class="columna2x">
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtDescripcion">Descripción:</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel runat="server" ID="uptxtDescripcion" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox runat="server" ID="txtDescripcion" CssClass="textbox2x validate[required, custom[onlyLetterSp]]" MaxLength="150" TabIndex="1"></asp:TextBox>
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
                    <label for="ddlAplicacion">Aplicación:</label>
                </div>
                <div class ="control2x">
                    <asp:UpdatePanel runat="server" ID="upddlAplicacion" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList runat="server" ID="ddlAplicacion" CssClass="dropdown" TabIndex="2"></asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>

            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="ddlTipoUnidad">Tipo Unidad:</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel runat="server" ID="upddlTipoUnidad" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList runat="server" ID="ddlTipoUnidad" CssClass="dropdown" AutoPostBack="true" OnSelectedIndexChanged="ddlTipoUnidad_SelectedIndexChanged" TabIndex="3"></asp:DropDownList> 
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
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
                            <asp:DropDownList runat="server" ID="ddlUnidadMedida" CssClass="dropdown" AutoPostBack="true" OnSelectedIndexChanged="ddlUnidadMedida_SelectedIndexChanged" TabIndex="4"></asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="ddlTipoUnidad" EventName="SelectedIndexChanged" />                            
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>

            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="ddlTarifa">Tarifa Base:</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel runat="server" ID="upddlTarifa" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList runat="server" ID="ddlTarifa" CssClass="dropdown" AutoPostBack="true" TabIndex="5"></asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnCAncelar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
                             <asp:AsyncPostBackTrigger ControlID="ddlUnidadMedida" EventName="SelectedIndexChanged" />                          
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>

            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="ddlMoneda">Moneda:</label>
                </div>
                <div class ="control2x">
                    <asp:UpdatePanel runat="server" ID="upddlMoneda" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList runat="server" ID="ddlMoneda" CssClass="dropdown" TabIndex="6" ></asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>

            <div class="renglon3x">
                <div class="etiqueta">
                    <label for="txtImpuestoTrasladado">Impuesto Trasladado:</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel runat="server" ID="uptxtImpuestoTrasladado" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox runat="server" ID="txtImpuestoTrasladado" CssClass="textbox_50px validate[custom[positiveNumber]]" MaxLength="18" TabIndex="7"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>

            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtImpuestoRetenido">Impuesto Retenido:</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel runat="server" ID="uptxtImpuestoRetenido" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox runat="server" ID="txtImpuestoRetenido" CssClass="textbox_50px validate[custom[positiveNumber]]" MaxLength="18"  TabIndex="8"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>

            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtCargoAdicional1">Cargo Adicional 1:</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel runat="server" ID="uptxtCargoAdicional1" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox runat="server" ID="txtCargoAdicional1" CssClass="textbox_50px validate[custom[positiveNumber]]" MaxLength="18"  TabIndex="9"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>

            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtCargoAdicional2">Cargo Adicional 2:</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel runat="server" ID="uptxtCargoAdicional2" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox runat="server" ID="txtCargoAdicional2" CssClass="textbox_50px validate[custom[positiveNumber]]" MaxLength="18" TabIndex="10"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>

            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="ddlConceptoSat">Concepto Nómina Sat:</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel runat="server" ID="upddlConceptoSat" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList runat="server" ID="ddlConceptoSat" CssClass="dropdown2x" TabIndex="11" ></asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>

            <div class=" renglon2x">
                <div class="etiqueta"></div>
                <div class="control2x">
                    <asp:UpdatePanel runat="server" ID="upchkGravado" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:CheckBox runat="server" ID="chkGravado" text="Gravado" TabIndex="12" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>

           <div class="renglon2x">
               <div class="controlBoton">
                   <asp:UpdatePanel runat="server" ID="upbtnCancelar" UpdateMode="Conditional">
                       <ContentTemplate>
                           <asp:Button runat="server" ID="btnCancelar" Text="Cancelar" CssClass="boton_cancelar" OnClick="btnCancelar_Click" TabIndex="14"/>
                       </ContentTemplate>
                       <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
                       </Triggers>
                   </asp:UpdatePanel>
               </div>

               <div class="controlBoton">
                   <asp:UpdatePanel runat="server" ID="upbtnGuardar" UpdateMode="Conditional">
                       <ContentTemplate>
                           <asp:Button runat="server" ID="btnGuardar" Text="Guardar" CssClass="boton" OnClick="btnGuardar_Click" TabIndex="13" />
                       </ContentTemplate>
                       <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
                       </Triggers>
                   </asp:UpdatePanel>
               </div>
           </div>

            <div class="renglon2x">
                <div class="etiqueta_320px">
                    <asp:UpdatePanel runat="server" ID="uplblError" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label runat="server" ID="lblError" CssClass="label_error"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            
        </div>
    </div>
</asp:Content>
