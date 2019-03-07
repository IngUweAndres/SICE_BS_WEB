<%@ Page Language="C#" MasterPageFile="~/Presentacion/Principal.Master" AutoEventWireup="true" CodeBehind="Cat_Perfiles.aspx.cs" Inherits="SICE_BS_WEB.Presentacion.Cat_Perfiles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        //Declara funciones jQuery una vez cargado el documento
        $(document).ready(function () {
            $('#Modal1').modal({
                backdrop: 'static',
                keyboard: false,
                show: false
            });            
        });

        function onRequestStart(sender, args) {
            if (args.get_eventTarget().indexOf("btnExcel") >= 0)
                args.set_enableAjax(false);
            else if (args.get_eventTarget().indexOf("btnPdf") >= 0)
                args.set_enableAjax(false);
        }

        function pageLoad(sender, args) {            
            //En cada postback los elementos HTML pierden su estilo y se ejecuta pageLoad; 
            // con los siguientes metodos se vuelve a asignar la clase correspondiente 
            // de acuerdo a las condiciones de cada uno
            validaNombre();

            $(function () {
                $('[data-toggle="popover"]').popover();
                $('[data-toggle="tooltip"]').tooltip({
                    delay: 500,
                    trigger: 'hover'
                });
            });            
        }

        function todos1Click(s, e, index) {
            var cheked = s.GetChecked();
            var row = grid2.GetDataRow(index);

            var chkTodo = eval("chkTodo" + index);
            chkTodo.SetChecked(cheked);

            var chkConsultar = eval("chkConsultar" + index);
            chkConsultar.SetChecked(cheked);

            try {
                var chkAgregar = eval("chkAgregar" + index);
                chkAgregar.SetChecked(cheked);
            }
            catch (err) { }
            try {
                var chkEditar = eval("chkEditar" + index);
                chkEditar.SetChecked(cheked);
            }
            catch (err) { }
            try {
                var chkEliminar = eval("chkEliminar" + index);
                chkEliminar.SetChecked(cheked);
            }
            catch (err) { }
            try {
                var chkExportar = eval("chkExportar" + index);
                chkExportar.SetChecked(cheked);
            }
            catch (err) { }
        }

        function permisos1Click(s, e, index) {
            var cheked = s.GetChecked();
            var row = grid2.GetDataRow(index);

            var checkbox = eval("chkConsultar" + index);
            var checkbox1 = checkbox.GetChecked();

            try {
                checkbox = eval("chkAgregar" + index);
                var checkbox2 = checkbox.GetChecked();
            }
            catch (err) { }
            try {
                checkbox = eval("chkEditar" + index);
                var checkbox3 = checkbox.GetChecked();
            }
            catch (err) { }
            try {
                checkbox = eval("chkEliminar" + index);
                var checkbox4 = checkbox.GetChecked();
            }
            catch (err) { }
            try {
                checkbox = eval("chkExportar" + index);
                var checkbox5 = checkbox.GetChecked();
            }
            catch (err) { }

            var chkTodo = eval("chkTodo" + index);
            chkTodo.SetChecked((checkbox1 != null ? checkbox1 : true) &&
            (checkbox2 != null ? checkbox2 : true) &&
            (checkbox3 != null ? checkbox3 : true) &&
            (checkbox4 != null ? checkbox4 : true) &&
            (checkbox5 != null ? checkbox5 : true));
        }

        function btnMarcarTodo1Click(sender, event) {
            var cheked = sender.checked;
            try {
                for (var i = 0; i < grid2.GetVisibleRowsOnPage() ; i++) {
                    var chkTodo = eval("chkTodo" + i);
                    chkTodo.SetChecked(cheked);

                    var chkConsultar = eval("chkConsultar" + i);
                    chkConsultar.SetChecked(cheked);

                    try {
                        var chkAgregar = eval("chkAgregar" + i);
                        chkAgregar.SetChecked(cheked);
                    }
                    catch (err) { }
                    try {
                        var chkEditar = eval("chkEditar" + i);
                        chkEditar.SetChecked(cheked);
                    }
                    catch (err) { }
                    try {
                        var chkEliminar = eval("chkEliminar" + i);
                        chkEliminar.SetChecked(cheked);
                    }
                    catch (err) { }
                    try {
                        var chkExportar = eval("chkExportar" + i);
                        chkExportar.SetChecked(cheked);
                    }
                    catch (err) { }
                }
            }
            catch (err) { }
            //Busca el icono (etiqueta span que contiene el glyphicon)
            var ico = document.getElementById("<%= IcoMarcarTodo.ClientID %>");
            if (ico != null)
                ico.className = cheked ? "glyphicon glyphicon-check" : "glyphicon glyphicon-unchecked";
        }
        
        function validaNombre() {
            var div = document.getElementById("<%= DivNombre.ClientID %>");
            var txt = document.getElementById("<%= TxtNombre.ClientID %>");
            var i = document.getElementById("<%= ITxtNombre.ClientID %>");
            if (txt.value.length > 0)
                asignarClases(div, i, false);
            else
                limpiarClases(div, i);
        }

        //
        //Grid-PageBar
        //
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

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentSection" runat="server">
    <asp:MultiView ID="MultiView1" ActiveViewIndex="0" runat="server">
        <asp:View runat="server">
            <div class="container">
                <div class="panel panel-primary">
                    <div id="divHeader" runat="server" class="panel-heading">
                        <h1 id="h1_titulo" runat="server" class="panel-title"></h1>
                    </div>
                    <div class="panel-body">                        
                        <asp:Panel ID="Panel1" runat="server">
                            <dx:ASPxGridView ID="Grid" runat="server" EnableTheming="True" Theme="SoftOrange" AutoGenerateColumns="False"
                               OnCustomCallback="Grid_CustomCallback" EnableCallBacks="false" ClientInstanceName="grid" Width="100%"
                               Settings-HorizontalScrollBarMode="Auto" KeyFieldName="IdPerfil" OnSelectionChanged="Grid_SelectionChanged"
                               Styles-Header-Font-Size="11px" Styles-Cell-Font-Size="11px" >
                               <SettingsAdaptivity AdaptivityMode="HideDataCellsWindowLimit" HideDataCellsAtWindowInnerWidth="800" 
                                AdaptiveDetailColumnCount="1" AllowOnlyOneAdaptiveDetailExpanded="True"  >
                                <AdaptiveDetailLayoutProperties colcount="2">
                                    <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit"  SwitchToSingleColumnAtWindowInnerWidth="600" />
                                </AdaptiveDetailLayoutProperties>
                               </SettingsAdaptivity> 
                               <SettingsResizing ColumnResizeMode="Control" /> 
                               <Settings ShowFooter="False" ShowHeaderFilterButton="true" ShowFilterRowMenu="false" />
                               <SettingsBehavior AllowSelectByRowClick="true" AllowSelectSingleRowOnly="true" FilterRowMode="OnClick"  
                                   ProcessSelectionChangedOnServer="false" ProcessColumnMoveOnClient="true" ColumnMoveMode="AmongSiblings"  />
                               <Styles>                                   
                                   <Row  />
                                   <AlternatingRow Enabled="True" />
                                   <PagerTopPanel Paddings-PaddingBottom="3px"></PagerTopPanel>
                               </Styles>
                               <SettingsPopup>
                                    <HeaderFilter Height="200px" Width="195px"/>
                               </SettingsPopup> 
                               <%--<SettingsPager Position="Bottom" ShowDisabledButtons="false" ShowNumericButtons="true" 
                                 <SettingsLoadingPanel Mode="Default" Text="Actualizando" />
                                 <SettingsBehavior AllowSelectByRowClick="true" AllowSelectSingleRowOnly="true" FilterRowMode="Auto"
                                         ProcessSelectionChangedOnServer="true"/>
                                     Summary-Visible="false" ShowSeparators="false" >
                                     <PageSizeItemSettings Items="10, 20, 50" Position="Right" Visible="true"/>
                                 </SettingsPager>--%>                                                           
                               <Columns>
                                    <dx:GridViewDataTextColumn Caption="" FieldName="IdPerfil" ReadOnly="True" Visible="false">
                                        <SettingsHeaderFilter Mode="CheckedList" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Nombre" FieldName="Nombre" ReadOnly="True" VisibleIndex="2" Width="50%">
                                        <SettingsHeaderFilter Mode="CheckedList" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Clave" FieldName="Clave" ReadOnly="True" VisibleIndex="3" Width="50%">
                                        <SettingsHeaderFilter Mode="CheckedList" />
                                    </dx:GridViewDataTextColumn>                     
                                </Columns>
                               <Toolbars>
                                   <dx:GridViewToolbar Name="Toolbar1" ItemAlign="Left"  EnableAdaptivity="true">
                                   <Items>
                                       <dx:GridViewToolbarItem Name="Links">
                                        <Template>
                                            <asp:LinkButton ID="lkb_Nuevo" runat="server" OnClick="lkb_Nuevo_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button" ToolTip="Nuevo Usuario">
                                                <span class="glyphicon glyphicon-plus"></span>&nbsp;&nbsp;Nuevo
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="lkb_Editar" runat="server" OnClick="lkb_Editar_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button" ToolTip="Editar Usuario" >
                                                <span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp;Editar
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="lkb_Eliminar" runat="server" OnClick="lkb_Eliminar_Click" OnClientClick="alertQuestion('¿Desea eliminar este perfil?', this); return false;" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button" ToolTip="Eliminar Usuario">
                                                <span class="glyphicon glyphicon-remove"></span>&nbsp;&nbsp;Eliminar
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="lkb_Excel" runat="server" OnClick="lkb_Excel_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button" ToolTip="Exportar a Excel">
                                                <span class="glyphicon glyphicon-list"></span>&nbsp;&nbsp;Excel
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="lkb_LimpiarFiltros" runat="server" OnClick="lkb_LimpiarFiltros_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button" ToolTip="Limpiar Filtros">
                                                <span class="glyphicon glyphicon-erase"></span>&nbsp;&nbsp;Limpiar Filtros
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="lkb_Actualizar" runat="server" OnClick="lkb_Actualizar_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button" ToolTip="Actualizar">
                                                <span class="glyphicon glyphicon-refresh"></span>&nbsp;&nbsp;Actualizar
                                            </asp:LinkButton>                                            
                                        </Template>
                                       </dx:GridViewToolbarItem>

                                        <%--
                                        <dx:GridViewToolbarItem Text="Exportar a" Image-IconID="actions_download_16x16office2013" BeginGroup="true">
                                           <Items>
                                               <dx:GridViewToolbarItem Name="ExportToPDF" Text="PDF" Image-IconID="export_exporttopdf_16x16office2013" />
                                               <dx:GridViewToolbarItem Name="ExportToXLSX" Text="XLSX" Image-IconID="export_exporttoxlsx_16x16office2013" />
                                               <dx:GridViewToolbarItem Name="ExportToXLS" Text="XLS" Image-IconID="export_exporttoxls_16x16office2013" />
                                           </Items>
                                       </dx:GridViewToolbarItem>
                                        --%>              
                                   </Items>
                                </dx:GridViewToolbar>
                                </Toolbars>
                               <Templates>
                                   <PagerBar>
                                      <table runat="server" class="OptionsTable" style="width: 100%">
                               <tr>
                                   <td style="padding-left: 8px;">
                                       <dx:BootstrapButton ID="btnInicial" runat="server" Text="" AutoPostBack="false" UseSubmitBehavior="false"                                                                                
                                           SettingsBootstrap-RenderOption="Primary"  CssClasses-Control="btn btn-primary btn-xs" Enabled="<%# Container.Grid.PageIndex > 0 %>">
                                           <ClientSideEvents Click="function() { grid.GotoPage(0) }" />
                                           <CssClasses Icon="glyphicon glyphicon-backward" /> 
                                       </dx:BootstrapButton>
                                   </td>
                                   
                                   <td style="padding-left: 5px;">
                                       <dx:BootstrapButton ID="btnAtras" runat="server" Text="" AutoPostBack="false" UseSubmitBehavior="false"                                                                                
                                           SettingsBootstrap-RenderOption="Primary"  CssClasses-Control="btn btn-primary btn-xs" Enabled="<%# Container.Grid.PageIndex > 0 %>">
                                           <ClientSideEvents Click="function() { grid.PrevPage() }" />
                                           <CssClasses Icon="glyphicon glyphicon-triangle-left" /> 
                                       </dx:BootstrapButton>
                                   </td>
                                   <td style="padding-left: 5px;">
                                       <dx:ASPxTextBox runat="server" ID="GotoBox" Width="30" Font-Size="11px">
                                           <ClientSideEvents Init="CustomPager.gotoBox_Init" ValueChanged="CustomPager.gotoBox_ValueChanged"
                                               KeyPress="CustomPager.gotoBox_KeyPress" />
                                       </dx:ASPxTextBox>
                                   </td>
                                   <td style="padding-left: 5px;font-size:11px">
                                       <span style="white-space: nowrap">
                                           de <%# Container.Grid.PageCount %>
                                       </span>
                                   </td>
                                   <td style="padding-left: 5px;">
                                       <dx:BootstrapButton ID="btnSiuiente" runat="server" Text="" AutoPostBack="false" UseSubmitBehavior="false"                                                                                
                                           SettingsBootstrap-RenderOption="Primary"  CssClasses-Control="btn btn-primary btn-xs" Enabled="<%# Container.Grid.PageIndex < Container.Grid.PageCount - 1 %>">
                                           <ClientSideEvents Click="function() { grid.NextPage() }" />
                                           <CssClasses Icon="glyphicon glyphicon-triangle-right" /> 
                                       </dx:BootstrapButton>
                                   </td>
                                   <td style="padding-left: 5px;">
                                       <dx:BootstrapButton ID="btnFinal" runat="server" Text="" AutoPostBack="false" UseSubmitBehavior="false"                                                                                
                                           SettingsBootstrap-RenderOption="Primary"  CssClasses-Control="btn btn-primary btn-xs" Enabled="<%# Container.Grid.PageIndex < Container.Grid.PageCount - 1 %>">
                                           <ClientSideEvents Click="function() { grid.GotoPage(grid.GetPageCount() - 1);}" />
                                           <CssClasses Icon="glyphicon glyphicon-forward" /> 
                                       </dx:BootstrapButton>
                                   </td>
                                   <td style="width: 100%; padding-left:5px;font-size:11px">
                                       <span style="white-space: nowrap">
                                           (<%# Container.Grid.VisibleRowCount %>)
                                       </span>
                                   </td>
                                   <td style="white-space: nowrap">
                                       <span style="white-space: nowrap;font-size:11px">
                                           Filas:
                                       </span>
                                   </td>
                                   <td style="padding-right: 5px;">
                                       <dx:ASPxComboBox runat="server" ID="Combo" Width="70px" DropDownWidth="70px" ValueType="System.Int32"
                                           OnLoad="PagerCombo_Load" Font-Size="11px">
                                           <Items>
                                               <dx:ListEditItem Value="20" />
                                               <dx:ListEditItem Value="30" />
                                               <dx:ListEditItem Value="50" />
                                           </Items>
                                           <ClientSideEvents SelectedIndexChanged="CustomPager.combo_SelectedIndexChanged" />
                                       </dx:ASPxComboBox>
                                   </td>
                               </tr>
                           </table>
                                  </PagerBar>
                                </Templates>                      
                            </dx:ASPxGridView>
                            <dx:ASPxGridViewExporter ID="Exporter" GridViewID="Grid" runat="server" PaperKind="A5" Landscape="true" />
                            <dx:ASPxCallback ID="ASPxCallback1" runat="server" ClientInstanceName="Callback" >
                                <ClientSideEvents  CallbackComplete="function(s, e) { System.Threading.Thread.Sleep(3000); LoadingPanel1.Hide(); }" />
                            </dx:ASPxCallback>
                            <dx:ASPxLoadingPanel ID="LoadingPanel1" runat="server" ClientInstanceName="LoadingPanel1"
                                Modal="True" ViewStateMode="Enabled" Theme="MaterialCompact" >
                            </dx:ASPxLoadingPanel>
                            <button id="btnNuevoH" type="button" data-toggle="modal" data-target="#Modal1" data-whatever="Nuevo" onclick="return false;" style="display: none;"></button>
                            <button id="btnEditarH" type="button" data-toggle="modal" data-target="#Modal1" data-whatever="Editar" onclick="return false;" style="display: none;"></button>
                        </asp:Panel>
                        <asp:HiddenField ID="HfIdPerfil" runat="server" />
                        <dx:ASPxLabel ID="lblCadena" runat="server" Visible="false"/>
                    </div>
                </div>
            </div>
        </asp:View>
    </asp:MultiView>
    <div class="modal fade" id="Modal1" tabindex="-1" role="dialog" aria-labelledby="Modal1Titulo" data-backdrop="static">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 id="Modal1Titulo" runat="server"></h4>
                </div>
                <asp:Panel ID="Panel2" runat="server">
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-sm-4 col-md-3">
                                <div runat="server" id="DivNombre">
                                    <div class="form-group" style="position: relative; width: 100%; float: left;" title="Nombre" data-toggle="tooltip" >
                                        <div class="input-group">
                                            <span class="input-group-addon" id="basic-addon1">
                                                <span class="glyphicon glyphicon-eye-open" aria-hidden="true"></span>
                                            </span>
                                            <asp:TextBox ID="TxtNombre" runat="server" CssClass="form-control input-sm" MaxLength="15" placeholder="* Nombre" onblur="validaNombre()"></asp:TextBox>
                                        </div>
                                        <i runat="server" id="ITxtNombre"></i>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-4 col-md-3">
                                <div runat="server" id="DivClave">
                                    <div class="form-group" style="position: relative; width: 100%; float: left;" title="Clave" data-toggle="tooltip">
                                        <div class="input-group">
                                            <span class="input-group-addon" id="basic-addon2">
                                                <span class="glyphicon glyphicon-tag" aria-hidden="true"></span>
                                            </span>
                                            <asp:TextBox ID="TxtClave" runat="server" CssClass="form-control input-sm" MaxLength="15" placeholder="Clave"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12 form-group">
                                <dx:ASPxGridView ID="Grid2" runat="server" EnableTheming="True" Theme="MetropolisBlue"
                                    EnableCallBacks="false" ClientInstanceName="grid2" AutoGenerateColumns="False" Width="100%"
                                    Settings-HorizontalScrollBarMode="Auto" KeyFieldName="" SettingsPager-PageSize="400">                                    
                                    <SettingsResizing ColumnResizeMode="Control" /> 
                                    <Settings ShowFooter="False" ShowFilterRow="false" ShowFilterRowMenu="false" VerticalScrollBarMode="Auto" VerticalScrollableHeight="224" />                                                                                            
                                    <SettingsBehavior AllowSort="false" AllowDragDrop="false" AllowGroup="false" />
                                    <Styles>                                   
                                        <Header BackColor="#F4F4F4" ForeColor="#000000" Font-Overline="false"  
                                                 Font-Underline="false" Font-Bold="false" Font-Size="12px" />
                                             <SelectedRow BackColor="#9E9E9E" ForeColor="#FFFFFF" />
                                             <Row  />
                                             <AlternatingRow Enabled="True" />
                                    </Styles>
                                    <Columns>                                                                              
                                        <dx:GridViewDataTextColumn Caption="" FieldName="IdModulo" ReadOnly="True" Visible="false" VisibleIndex="0" >
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Nombre" FieldName="Nombre" ReadOnly="True" VisibleIndex="1" Width="446px">
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </dx:GridViewDataTextColumn>
                                        <%--
                                        <dx:GridViewDataTextColumn Caption="Tipo" FieldName="TipoModulo" ReadOnly="True" VisibleIndex="2" Width="70px">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <CellStyle HorizontalAlign="Center"></CellStyle>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewCommandColumn ShowSelectCheckbox="True" Caption="" Name="Todo" VisibleIndex="3" >
                                        </dx:GridViewCommandColumn>
                                        --%>
                                        <dx:GridViewDataColumn Caption="Control <br /> Total" FieldName="Todo" VisibleIndex="3" Width="70px">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <CellStyle HorizontalAlign="Center"></CellStyle>
                                            <DataItemTemplate>
                                                <dx:ASPxCheckBox ID="chkTodo" ClientInstanceName="chkTodo" runat="server" Enabled="false" ToolTip="Seleccionar todos los permisos" OnInit="chkTodo_Init">                                                    
                                                </dx:ASPxCheckBox>
                                            </DataItemTemplate>
                                        </dx:GridViewDataColumn>
                                        <dx:GridViewDataColumn Caption="Consultar" FieldName="Consultar" VisibleIndex="4" Width="70px" >
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <CellStyle HorizontalAlign="Center"></CellStyle>
                                            <DataItemTemplate>
                                                <dx:ASPxCheckBox ID="chkConsultar" ClientInstanceName="chkConsultar" runat="server" Enabled="false" OnInit="chkConsultar_Init" ToolTip="Ver"  />
                                            </DataItemTemplate>
                                        </dx:GridViewDataColumn>
                                        <dx:GridViewDataColumn Caption="Agregar" FieldName="Agregar" VisibleIndex="5" Width="70px" >
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <CellStyle HorizontalAlign="Center"></CellStyle>
                                            <DataItemTemplate>
                                                <dx:ASPxCheckBox ID="chkAgregar" ClientInstanceName="chkAgregar" runat="server" Enabled="false" OnInit="chkAgregar_Init" ToolTip="Nuevo" />
                                            </DataItemTemplate>
                                        </dx:GridViewDataColumn>
                                        <dx:GridViewDataColumn Caption="Editar" FieldName="Editar" VisibleIndex="6" Width="70px" >
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <CellStyle HorizontalAlign="Center"></CellStyle>
                                            <DataItemTemplate>
                                                <dx:ASPxCheckBox ID="chkEditar" ClientInstanceName="chkEditar" runat="server" Enabled="false" OnInit="chkEditar_Init" ToolTip="Editar" />
                                            </DataItemTemplate>
                                        </dx:GridViewDataColumn>
                                        <dx:GridViewDataColumn Caption="Eliminar" FieldName="Eliminar" VisibleIndex="7" Width="70px" >
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <CellStyle HorizontalAlign="Center"></CellStyle>
                                            <DataItemTemplate>
                                                <dx:ASPxCheckBox ID="chkEliminar" ClientInstanceName="chkEliminar" runat="server" Enabled="false" OnInit="chkEliminar_Init" ToolTip="Borrar" />
                                            </DataItemTemplate>
                                        </dx:GridViewDataColumn>
                                        <dx:GridViewDataColumn Caption="Exportar" FieldName="Exportar" VisibleIndex="8" Width="70px" >
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <CellStyle HorizontalAlign="Center"></CellStyle>
                                            <DataItemTemplate>
                                                <dx:ASPxCheckBox ID="chkExportar" ClientInstanceName="chkExportar" runat="server" Enabled="false" OnInit="chkExportar_Init" ToolTip="Exportar" />
                                            </DataItemTemplate>
                                        </dx:GridViewDataColumn>                                                                                                 
                                   </Columns>                    
                                </dx:ASPxGridView>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-6 col-sm-6 col-md-6 text-left">
                                <label runat="server" id="LblMarcarTodo" class="btn btn-primary btn-sm">
                                    <asp:CheckBox runat="server" ID="CbxMarcarTodo" Checked="false" Style="display: none;" onclick="btnMarcarTodo1Click(this, event);" />
                                    <span class="glyphicon glyphicon-unchecked" id="IcoMarcarTodo" runat="server"></span>&nbsp;&nbsp;Marcar todo
                                </label>
                            </div>
                            <div class="col-xs-6 col-sm-6 col-md-6 text-right">
                                <asp:Label Text="* Campos obligatorios" Font-Italic="true" runat="server" CssClass="asp-label" ForeColor="Red" />
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <div class="col-md-12 col-md-offset-0" style="text-align: center;">
                            <asp:LinkButton ID="btnGuardar" runat="server" CssClass="btn btn-primary btn-sm" Text="Guardar" OnClick="btnGuardar_Click">
                                <span class="glyphicon glyphicon-ok"></span>&nbsp;&nbsp;Guardar</asp:LinkButton>
                            <button type="button" class="btn btn-primary btn-sm" data-dismiss="modal">
                                <span class="glyphicon glyphicon-remove"></span>&nbsp;&nbsp;Cancelar
                            </button>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div>
    
    <button id="btnQuestion" type="button" data-toggle="modal" data-target="#AlertQuestion" style="display: none;"></button>
    <div class="modal fade" id="AlertQuestion" tabindex="-1" role="dialog" style="top: 25%; outline: none;" >
        <div class="modal-dialog modal-sm" role="document">
            <div class="alert alert-info text-center" style="-webkit-box-shadow: 0 5px 15px rgba(0, 0, 0, .5); box-shadow: 0 5px 15px rgba(0, 0, 0, .5);">
                <span class="glyphicon glyphicon-question-sign ico"></span>
                <br />
                <br />
                <p id="pModalQuestion" runat="server" class="alert-title">
                </p>
                <hr />
                <a id="btnOk" class="btn btn-info" data-dismiss="modal">Aceptar</a>
                <a id="btnCancel" class="btn btn-info" data-dismiss="modal">Cancelar</a>
            </div>
        </div>
    </div>
    <button id="btnError" type="button" data-toggle="modal" data-target="#AlertError" style="display: none;"></button>
    <div class="modal fade" id="AlertError" tabindex="-1" role="dialog" data-html="true">
        <div class="modal-dialog modal-sm" role="document" style="top: 25%; outline: none;">
            <div class="alert alert-danger text-center" style="-webkit-box-shadow: 0 5px 15px rgba(0, 0, 0, .5); box-shadow: 0 5px 15px rgba(0, 0, 0, .5);">
                <span class="glyphicon glyphicon-alert ico"></span>
                <br />
                <br />
                <p id="pModalError" runat="server" class="alert-title">
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
