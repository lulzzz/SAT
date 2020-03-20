<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ArchivoRegistro.aspx.cs" Inherits="SAT.Accesorios.ArchivoRegistro" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <!-- Estilos de los Controles -->
    <link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
    <link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
    <link href="../CSS/ControlesUsuario.css" type="text/css" rel="stylesheet" />
    <!-- Estilos de Validación, DateTimePicker, MasketTextBox -->
    <link href="../CSS/jquery.validationEngine.css" type="text/css" rel="stylesheet" />
    <!-- Bibliotecas para Validación de formulario -->
    <script type="text/javascript" src="../Scripts/jquery-1.7.1.min.js" charset="utf-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
    <!-- JQueryScript de Validación -->
    <script type="text/javascript">
        //Función encargada de restablecer la Configuración de los Scripts de Validación
        $(document).ready(function () {
            //Validando que se cumplan las condiciones
            var validaControl = function () {
                var isValid1 = !$("#<%=txtReferencia.ClientID%>").validationEngine('validate');
                //Devolviendo Resultado
                return isValid1;
            }
            //Añadiendo Funcion al Evento Click del Boton "Guardar"
            $("#<%=btnGuardar.ClientID%>").click(validaControl);
        });
    </script>
</head>
<body>

    <form id="form1" runat="server">
        <asp:ScriptManager ID="sc" runat="server"></asp:ScriptManager>
        <div class="contenido_control_archivo">
            <div class="encabezado_control">
                <h2>Archivos Relacionados</h2>
            </div>
            <div class="contenido_resultado_archivo">
                <div class="renglon3x">
                    <div class="etiqueta">
                        <label for="ddlTamano">Mostrar:</label>
                    </div>
                    <div class="control">                        
                            <asp:DropDownList ID="ddlTamano" runat="server" TabIndex="6" CssClass="dropdown_100px"
                                OnSelectedIndexChanged="ddlTamano_SelectedIndexChanged">
                            </asp:DropDownList>                           
                    </div>
                    <div class="etiqueta">
                        <label>Ordenado:</label>
                    </div>
                    <div class="etiqueta">                       
                        <asp:Label ID="lblCriterio" runat="server"></asp:Label>                           
                    </div>
                    <div class="etiqueta">                        
                        <asp:LinkButton ID="lnkExportar" runat="server" TabIndex="7" Text="Exportar"
                            OnClick="lnkExportar_Click"></asp:LinkButton>                            
                    </div>
                </div>
                <div class="grid_archivo">
                    <asp:GridView ID="gvArchivos" runat="server" TabIndex="8" AllowPaging="true" AllowSorting="true"
                                OnSorting="gvArchivos_Sorting" OnPageIndexChanging="gvArchivos_PageIndexChanging" AutoGenerateColumns="false"
                                CssClass="gridview" Width="100%" PageSize="5">
                                <AlternatingRowStyle CssClass="gridviewrowalternate" />
                                <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                                <FooterStyle CssClass="gridviewfooter" />
                                <HeaderStyle CssClass="gridviewheader" />
                                <RowStyle CssClass="gridviewrow" />
                                <SelectedRowStyle CssClass="gridviewrowselected" />
                                <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                                <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                                <Columns>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Left" ItemStyle-Width="5%" Visible="false">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkDetallesTodos" Text="TODOS" runat="server"
                                                AutoPostBack="True" OnCheckedChanged="chkTodos_CheckedChanged"
                                                CssClass="LabelResalta" />
                                        </HeaderTemplate>
                                        <FooterStyle HorizontalAlign="Center" />
                                        <HeaderStyle Width="5%" HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkDetalle" runat="server" AutoPostBack="True"
                                                OnCheckedChanged="chkDetalle_CheckedChanged" />
                                        </ItemTemplate>
                                        <ItemStyle Width="5%" HorizontalAlign="Center" />
                                        <FooterTemplate>
                                            <asp:Label ID="lblContadorDetalles" runat="server" Text="0">
                                            </asp:Label><br />
                                            Seleccionado(s)
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Referencia" SortExpression="Referencia">
                                        <ItemTemplate>
                                            <asp:Label ID="lblReferenciaArchivo" runat="server"
                                                Text='<%# TSDK.Base.Cadena.TruncaCadena(Eval("Referencia").ToString(), 20, "...") %>' ToolTip='<%# Eval("Referencia") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Extension" HeaderText="Extensión" SortExpression="Extension" />
                                    <asp:TemplateField HeaderText="URL" SortExpression="URL">
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" ToolTip='<%# Eval("URL") %>' Text='<%# TSDK.Base.Cadena.TruncaCadena(Eval("URL").ToString(), 40, "...") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEliminar" runat="server" Text="Eliminar" TabIndex="9"
                                                OnClick="lnkEliminar_Click"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDescargar" runat="server" Text="Descargar" TabIndex="10"
                                                OnClick="lnkDescargar_Click"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                </div>
            </div>
            <div class="contenido_filtro_archivo"> 
                <div class="columna2x">
                    <div class="renglon2x">
                        <div class="etiqueta">
                            <label for="ddlTipo">Tipo</label>
                        </div>
                         <div class="control2x">
                            <asp:DropDownList ID="ddlTipo" runat="server" TabIndex="1" CssClass="dropdown2x"
                                OnSelectedIndexChanged="ddlTipo_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="renglon2x">
                        <div class="etiqueta">
                            <label for="txtReferencia">Referencia</label>
                        </div>
                        <div class="control2x">
                            <asp:TextBox ID="txtReferencia" runat="server" TabIndex="2" CssClass="textbox2x validate[required]" MaxLength="260"></asp:TextBox>
                        </div>
                    </div>
                    <div class="renglon2x">
                        <div class="etiqueta">
                            <label for="fuArchivo">Archivo</label>
                        </div>
                        <div class="control2x">
                            <asp:FileUpload ID="fuArchivo" runat="server" TabIndex="3" CssClass="textbox2x" ForeColor="Black" />
                        </div>
                    </div>
                    <div class="renglon2x"></div>
                     <div class="renglon2x">
                        <div class="controlBoton">
                            <asp:Button ID="btnGuardar" runat="server" TabIndex="4" OnClick="btnGuardar_Click"
                                Text="Guardar" CssClass="boton" />
                        </div>
                        <div class="controlBoton">
                            <asp:Button ID="btnCancelar" runat="server" TabIndex="5" OnClick="btnCancelar_Click"
                                Text="Cancelar" CssClass="boton_cancelar" />
                        </div>
                    </div>
                    <div class="renglon2x">
                        <div class="control2x">
                            <asp:Label ID="lblError" runat="server" CssClass="label_error"></asp:Label>
                            <asp:Label ID="lblErrorArchivos" runat="server" CssClass="label_error"></asp:Label>
                        </div>
                    </div>
                </div>

            </div>
            <div class="imagen_control">
                <img src="../Image/Archivo.png" />
            </div>
        </div>    
    </form>
</body>
</html>
