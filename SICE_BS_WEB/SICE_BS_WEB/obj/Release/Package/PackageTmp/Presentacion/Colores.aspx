<%@ Page Language="C#" MasterPageFile="~/Presentacion/Principal.Master" AutoEventWireup="true" CodeBehind="Colores.aspx.cs" Inherits="SICE_BS_WEB.Presentacion.Colores" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        //Inicializa todos los tooltip
        $(function () {
            $('[data-toggle="popover"]').popover();
            $('[data-toggle="tooltip"]').tooltip({
                delay: 500,
                trigger: 'hover'
            });
        });

        var CustomPager = {
            gotoBox_Init: function (s, e) {
                s.SetText(1 + grid.GetPageIndex());
            },
            gotoBox_KeyPress: function (s, e) {
                if (e.htmlEvent.keyCode != 13)
                    return;
                ASPxClientUtils.PreventEventAndBubble(e.htmlEvent);
                CustomPager.applyGotoBoxValue(s);
            },
            gotoBox_ValueChanged: function (s, e) {
                CustomPager.applyGotoBoxValue(s);
            },
            applyGotoBoxValue: function (textBox) {
                if (grid.InCallback())
                    return;
                var pageIndex = parseInt(textBox.GetText()) - 1;
                if (pageIndex < 0)
                    pageIndex = 0;
                grid.GotoPage(pageIndex);
            },
            combo_SelectedIndexChanged: function (s, e) {
                grid.PerformCallback(s.GetSelectedItem().text);
            }
        };

        function OnToolbarItemClick(s, e) {
            if (IsExportToolbarCommand(e.item.name)) {
                e.processOnServer = true;
                e.usePostBack = true;
            }
        }
        function IsExportToolbarCommand(command) {
            return command == "ExportToPDF" || command == "ExportToXLSX" || command == "ExportToXLS";
        }

        function ColorChangedHandler(s, e) {
            CallbackPanel.PerformCallback();
        }


     </script>
    <style>
        p {
            text-align: justify;
            text-justify: inter-word;            
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentSection" runat="server">
    <dx:ASPxLabel ID="lblCadena" runat="server" Visible="false"/>                  
    <dx:ASPxCallback ID="ASPxCallback1" runat="server" ClientInstanceName="Callback" >
        <ClientSideEvents  CallbackComplete="function(s, e) { System.Threading.Thread.Sleep(3000); LoadingPanel1.Hide(); }" />
    </dx:ASPxCallback>
    <dx:ASPxLoadingPanel ID="LoadingPanel1" runat="server" ClientInstanceName="LoadingPanel1"
        Modal="True" ViewStateMode="Enabled" Theme="MaterialCompact" >
    </dx:ASPxLoadingPanel>
 
 <button id="btnModal" type="button" data-toggle="modal" data-target="#Modal" data-whatever="Nuevo" onclick="return false;" style="display: none;"></button>
 <div class="modal fade" id="Modal" tabindex="-1" role="dialog" aria-labelledby="ModalTitulo" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-md" role="document" >
            <div class="modal-content">
                <div class="modal-header">
                    <h4 id="ModalTitulo" runat="server"></h4>
                </div>
                <asp:Panel ID="Panel2" runat="server">
                    <div class="modal-body">
                        <div class="row">
                            <div class="form-group col-md-12">
                                <dx:ASPxFormLayout ID="formLayout1" runat="server" AlignItemCaptionsInAllGroups="True" UseDefaultPaddings="false" Width="100%">
                                    <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit" SwitchToSingleColumnAtWindowInnerWidth="600" />
                                    <Items>                                        
                                        <dx:LayoutGroup Caption="Menú y Pie de Página" ColCount="3" SettingsItemCaptions-Location="Top">
                                            <Items>
                                                <dx:LayoutItem Caption="Fondo">
                                                    <LayoutItemNestedControlCollection>
                                                        <dx:LayoutItemNestedControlContainer runat="server" SupportsDisabledAttribute="True">
                                                            <dx:ASPxColorEdit runat="server" ID="ce_MenuBackColor" Color="#dedede" Width="100%" ClearButton-DisplayMode="Never" EnableCustomColors="true" ColumnCount="10" CssClass="bordes_curvos_derecha">                                    
                                                                <%--<ClientSideEvents ColorChanged="ColorChangedHandler" />--%>
                                                            </dx:ASPxColorEdit>
                                                        </dx:LayoutItemNestedControlContainer>
                                                    </LayoutItemNestedControlCollection>
                                                </dx:LayoutItem>
                                                <dx:LayoutItem Caption="Texto">
                                                    <LayoutItemNestedControlCollection>
                                                        <dx:LayoutItemNestedControlContainer runat="server" SupportsDisabledAttribute="True">
                                                            <dx:ASPxColorEdit runat="server" ID="ce_MenuFontColor" Color="#dedede" Width="100%" ClearButton-DisplayMode="Never" EnableCustomColors="true" ColumnCount="10" CssClass="bordes_curvos_derecha" >
                                                                <%--<ClientSideEvents ColorChanged="ColorChangedHandler" />--%>
                                                            </dx:ASPxColorEdit>
                                                        </dx:LayoutItemNestedControlContainer>
                                                    </LayoutItemNestedControlCollection>
                                                </dx:LayoutItem>
                                                <dx:LayoutItem Caption="Botón Seleccionado">
                                                    <LayoutItemNestedControlCollection>
                                                        <dx:LayoutItemNestedControlContainer runat="server" SupportsDisabledAttribute="True">
                                                            <dx:ASPxColorEdit runat="server" ID="ce_MenuBackSelectedColor" Color="#dedede" Width="100%" ClearButton-DisplayMode="Never" EnableCustomColors="true" ColumnCount="10" CssClass="bordes_curvos_derecha" >
                                                                <%--<ClientSideEvents ColorChanged="ColorChangedHandler" />--%>
                                                            </dx:ASPxColorEdit>
                                                        </dx:LayoutItemNestedControlContainer>
                                                    </LayoutItemNestedControlCollection>
                                                </dx:LayoutItem>
                                            </Items>
                                        </dx:LayoutGroup>                                                                               
                                    </Items>
                                </dx:ASPxFormLayout>
                            </div>
                            <div class="form-group col-md-12">
                                <dx:ASPxFormLayout ID="ASPxFormLayout2" runat="server" AlignItemCaptionsInAllGroups="True" UseDefaultPaddings="false" Width="100%">
                                    <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit" SwitchToSingleColumnAtWindowInnerWidth="600" />
                                    <Items>                                        
                                        <dx:LayoutGroup Caption="Encabezados y Botones" ColCount="3" SettingsItemCaptions-Location="Top">
                                            <Items>
                                                <dx:LayoutItem Caption="Fondo">
                                                    <LayoutItemNestedControlCollection>
                                                        <dx:LayoutItemNestedControlContainer runat="server" SupportsDisabledAttribute="True">
                                                            <dx:ASPxColorEdit runat="server" ID="ce_btnBackColor" Color="#dedede" Width="100%" ClearButton-DisplayMode="Never" EnableCustomColors="true" ColumnCount="10" CssClass="bordes_curvos_derecha">                                    
                                                                <%--<ClientSideEvents ColorChanged="ColorChangedHandler" />--%>
                                                            </dx:ASPxColorEdit>
                                                        </dx:LayoutItemNestedControlContainer>
                                                    </LayoutItemNestedControlCollection>
                                                </dx:LayoutItem>
                                                <dx:LayoutItem Caption="Texto">
                                                    <LayoutItemNestedControlCollection>
                                                        <dx:LayoutItemNestedControlContainer runat="server" SupportsDisabledAttribute="True">
                                                            <dx:ASPxColorEdit runat="server" ID="ce_btnFontColor" Color="#dedede" Width="100%" ClearButton-DisplayMode="Never" EnableCustomColors="true" ColumnCount="10" CssClass="bordes_curvos_derecha" >
                                                                <%--<ClientSideEvents ColorChanged="ColorChangedHandler" />--%>
                                                            </dx:ASPxColorEdit>
                                                        </dx:LayoutItemNestedControlContainer>
                                                    </LayoutItemNestedControlCollection>
                                                </dx:LayoutItem>
                                                <dx:LayoutItem Caption="Botón Seleccionado">
                                                    <LayoutItemNestedControlCollection>
                                                        <dx:LayoutItemNestedControlContainer runat="server" SupportsDisabledAttribute="True">
                                                            <dx:ASPxColorEdit runat="server" ID="ce_btnBackSelectedColor" Color="#dedede" Width="100%" ClearButton-DisplayMode="Never" EnableCustomColors="true" ColumnCount="10" CssClass="bordes_curvos_derecha" >
                                                                <%--<ClientSideEvents ColorChanged="ColorChangedHandler" />--%>
                                                            </dx:ASPxColorEdit>
                                                        </dx:LayoutItemNestedControlContainer>
                                                    </LayoutItemNestedControlCollection>
                                                </dx:LayoutItem>
                                            </Items>
                                        </dx:LayoutGroup>
                                        <%--<dx:EmptyLayoutItem />  --%>                                                                                                            
                                    </Items>
                                </dx:ASPxFormLayout>
                            </div>
                            <div class="form-group col-md-12">
                                <dx:ASPxFormLayout ID="ASPxFormLayout1" runat="server" AlignItemCaptionsInAllGroups="True" UseDefaultPaddings="false" Width="100%">
                                <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit" SwitchToSingleColumnAtWindowInnerWidth="600" />
                                <Items>                                        
                                    <dx:LayoutGroup Caption="Información Importante" ColCount="1" SettingsItemCaptions-Location="Top">
                                        <Items>
                                            <dx:LayoutItem ShowCaption="False" HorizontalAlign="Left">
                                                <LayoutItemNestedControlCollection>
                                                    <dx:LayoutItemNestedControlContainer runat="server" SupportsDisabledAttribute="True">
                                                        <asp:Panel runat="server" HorizontalAlign="Justify">
                                                            <p><asp:Label Text="* Si después de guardar no ve los cambios reflejados, ir a:" Font-Italic="false" runat="server" ForeColor="#999999" Font-Size="11px" /></p>
                                                            <p><asp:Label Text="<b>- IE</b>: Seleccionar Opciones de Internet, después seleccionar la pestaña General y dar clic en botón de Eliminar, seleccionae la casilla de Archivos temporales de Internet y archivos de sitio web y seleccionar botón Eliminar" 
                                                                Font-Italic="false" runat="server" ForeColor="#999999" Font-Size="11px" /></p>
                                                            <p><asp:Label Text="<b>- Mozilla FireFox</b>: Seleccionar Opciones, después seleccionar del menú izquierdo la opción de Privacidad y Seguridad, ir a la parte donde dice Cookies y datos del sitio, dar clic en botón Limpiar datos..., dentro seleccionar la casilla Contenido web en caché y dar clic en botón Limpiar" 
                                                                Font-Italic="false" runat="server" ForeColor="#999999" Font-Size="11px" /></p>
                                                            <p><asp:Label Text="<b>- Google Chrome</b>: Seleccionar Más herramientas, después seleccionar Borrar datos de navegación, en la pestaña Configuración avanzada seleccionar la casilla de Archivos e imágenes almacenados en caché, después dar clic en botón Borrar los datos" 
                                                                Font-Italic="false" runat="server" ForeColor="#999999" Font-Size="11px" /></p>
                                                            </asp:Panel>                                                        
                                                        </dx:LayoutItemNestedControlContainer>
                                                </LayoutItemNestedControlCollection>
                                            </dx:LayoutItem>
                                        </Items>
                                    </dx:LayoutGroup>
                                    <%--<dx:EmptyLayoutItem />  --%>                                                                                                            
                                </Items>
                            </dx:ASPxFormLayout>
                            </div>                            
                        </div>
                            
                    </div>
                    <div class="modal-footer">
                        <div class="col-md-12 col-md-offset-0" style="text-align: center;">
                            <asp:LinkButton ID="btnGuardar" runat="server" CssClass="btn btn-primary btn-sm" Text="Guardar" OnClick="btnGuardar_Click">
                                <span class="glyphicon glyphicon-ok"></span>&nbsp;&nbsp;Guardar</asp:LinkButton>
                            <asp:LinkButton ID="btnCancelar" runat="server" CssClass="btn btn-primary btn-sm" Text="Guardar" OnClick="btnCancelar_Click">
                                <span class="glyphicon glyphicon-remove"></span>&nbsp;&nbsp;Cancelar</asp:LinkButton>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div>
 <button id="btnError" type="button" data-toggle="modal" data-target="#AlertError" style="display: none;"></button>
 <div class="modal fade" id="AlertError" tabindex="-1" role="dialog">
     <div class="modal-dialog modal-sm" role="document" style="top: 25%; outline: none;">
         <div class="alert alert-danger text-center" style="-webkit-box-shadow: 0 5px 15px rgba(0, 0, 0, .5); box-shadow: 0 5px 15px rgba(0, 0, 0, .5);">
             <span class="glyphicon glyphicon-alert ico"></span>
             <br />
             <br />
             <p id="pModal" runat="server" class="alert-title">
             </p>                                
             <hr/>
             <button type="button" class="btn btn-danger" data-dismiss="modal">Aceptar</button>
         </div>
     </div>
 </div>

 <button id="btnSuccess" type="button" data-toggle="modal" data-target="#AlertSuccess" style="display: none;"></button>
 <div class="modal fade" id="AlertSuccess" tabindex="-1" role="dialog" style="top: 25%; outline: none;">
     <div class="modal-dialog modal-sm" role="document">
         <div class="alert alert-success text-center" style="-webkit-box-shadow: 0 5px 15px rgba(0, 0, 0, .5); box-shadow: 0 5px 15px rgba(0, 0, 0, .5);">
             <span class="glyphicon glyphicon-thumbs-up ico"></span>
             <br />
             <br />
             <p id="pModalSucces" runat="server" class="alert-title">
             </p>
             <hr />
             <button type="button" class="btn btn-success" data-dismiss="modal">Aceptar</button>
         </div>
     </div>
 </div>
   

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptSection" runat="server">
</asp:Content>