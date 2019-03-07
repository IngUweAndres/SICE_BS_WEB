﻿<%@ Page Language="C#" MasterPageFile="~/Presentacion/Principal.Master" AutoEventWireup="true" CodeBehind="Cat_RepresentanteLegal.aspx.cs" Inherits="SICE_BS_WEB.Presentacion.Cat_RepresentanteLegal" %>

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
             
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentSection" runat="server">
           
    <div class="container-fluid">
         <div class="panel panel-primary">
             <div class="panel-heading">
                 <h1 id="h1_titulo" runat="server" class="panel-title small"></h1>
             </div>
             <div class="panel-body">
                 <asp:Panel ID="Panel1" runat="server">
                 <%--<div class="row" style="padding-left:10px; padding-top:10px; padding-right:10px;">   --%>                                     
                    <dx:ASPxGridView ID="Grid" runat="server" EnableTheming="True" Theme="SoftOrange" OnToolbarItemClick="Grid_ToolbarItemClick"
                        OnRowCommand="Grid_RowCommand" OnCustomCallback="Grid_CustomCallback" EnableCallBacks="false" ClientInstanceName="grid"
                        AutoGenerateColumns="False" Width="100%" Settings-HorizontalScrollBarMode="Auto" KeyFieldName="REPRESENTANTEKEY" SettingsPager-Position="TopAndBottom"
                        Styles-Header-Font-Size="11px" Styles-Cell-Font-Size="11px" >
                        <SettingsAdaptivity AdaptivityMode="HideDataCellsWindowLimit" HideDataCellsAtWindowInnerWidth="800" 
                            AdaptiveDetailColumnCount="1" AllowOnlyOneAdaptiveDetailExpanded="True">
                            <AdaptiveDetailLayoutProperties colcount="2">
                                <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit"  SwitchToSingleColumnAtWindowInnerWidth="600" />
                            </AdaptiveDetailLayoutProperties>
                        </SettingsAdaptivity> 
                        <SettingsResizing ColumnResizeMode="Control" />
                         <SettingsBehavior AllowSelectByRowClick="true" AllowSelectSingleRowOnly="true" />
                        <Settings ShowFooter="False" ShowHeaderFilterButton="true" ShowFilterRowMenu="false"  />                                          
                        <Styles>                                                                                      
                            <SelectedRow />
                            <Row  />
                            <AlternatingRow Enabled="True" />
                            <PagerTopPanel Paddings-PaddingBottom="3px"></PagerTopPanel>
                        </Styles>
                        <SettingsPopup>
                            <HeaderFilter Height="200px" Width="195px" />
                        </SettingsPopup> 
                        <%--<SettingsPager Position="Bottom" ShowDisabledButtons="false" ShowNumericButtons="true" 
                                 <SettingsLoadingPanel Mode="Default" Text="Actualizando" />
                                 <SettingsBehavior AllowSelectByRowClick="true" AllowSelectSingleRowOnly="true" FilterRowMode="Auto"
                                         ProcessSelectionChangedOnServer="true"/>
                                     Summary-Visible="false" ShowSeparators="false" >
                                     <PageSizeItemSettings Items="10, 20, 50" Position="Right" Visible="true"/>
                                 </SettingsPager>--%>                                             
                        <Columns>
                            <dx:GridViewDataTextColumn FieldName="REPRESENTANTEKEY" ReadOnly="True"  Visible="false" >                                
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="NOMBRE" FieldName="NOMBRE" ReadOnly="True" VisibleIndex="1" Width="180px" >
                                <HeaderStyle HorizontalAlign="Center" />
                                <SettingsHeaderFilter Mode="CheckedList" />
                                <CellStyle HorizontalAlign="Center"></CellStyle>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="APELLIDO PATERNO" FieldName="APELLIDOPATERNO" ReadOnly="True" VisibleIndex="2" Width="190px" >
                                <HeaderStyle HorizontalAlign="Center" />
                                <SettingsHeaderFilter Mode="CheckedList" />
                                <CellStyle HorizontalAlign="Center"></CellStyle>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="APELLIDO MATERNO" FieldName="APELLIDOMATERNO" ReadOnly="True" VisibleIndex="3" Width="190px" >
                                <HeaderStyle HorizontalAlign="Center" />
                                <SettingsHeaderFilter Mode="CheckedList" />
                                <CellStyle HorizontalAlign="Center"></CellStyle>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="RFC" FieldName="RFC" ReadOnly="True" VisibleIndex="4" Width="180px" >
                                <HeaderStyle HorizontalAlign="Center" />
                                <SettingsHeaderFilter Mode="CheckedList" />
                                <CellStyle HorizontalAlign="Center"></CellStyle>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="CORREO ELECTRÓNICO" FieldName="CORREOELECTRONICO" ReadOnly="True" VisibleIndex="5" Width="320px" >
                                <HeaderStyle HorizontalAlign="Center" />
                                <SettingsHeaderFilter Mode="CheckedList" />
                                <CellStyle HorizontalAlign="Center"></CellStyle>
                            </dx:GridViewDataTextColumn>  
                            <dx:GridViewDataTextColumn Caption="TELÉFONO" FieldName="TELEFONO" ReadOnly="True" VisibleIndex="6" Width="160px" >
                                <HeaderStyle HorizontalAlign="Center" />
                                <SettingsHeaderFilter Mode="CheckedList" />
                                <CellStyle HorizontalAlign="Center"></CellStyle>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataDateColumn Caption="APARTIR DE" FieldName="APARTIRDE" ReadOnly="True" VisibleIndex="7" Width="120px" >
                                <HeaderStyle HorizontalAlign="Center" />
                                <SettingsHeaderFilter Mode="CheckedList" />
                                <CellStyle HorizontalAlign="Center"></CellStyle>
                            </dx:GridViewDataDateColumn>
                            <dx:GridViewDataDateColumn Caption="HASTA" FieldName="HASTA" ReadOnly="True" VisibleIndex="8" Width="120px" >
                                <HeaderStyle HorizontalAlign="Center" />
                                <SettingsHeaderFilter Mode="CheckedList" />
                                <CellStyle HorizontalAlign="Center"></CellStyle>
                            </dx:GridViewDataDateColumn>
                            <%--<dx:GridViewDataTextColumn Caption="FIRMA" FieldName="FIRMA" ReadOnly="True" VisibleIndex="9" Width="100px" >
                                <HeaderStyle HorizontalAlign="Center" />
                                <SettingsHeaderFilter Mode="CheckedList" />
                                <CellStyle HorizontalAlign="Center"></CellStyle>
                            </dx:GridViewDataTextColumn>--%>
                            <dx:GridViewDataBinaryImageColumn Caption="FIRMA" FieldName="FIRMA" VisibleIndex="9" Width="200px" PropertiesBinaryImage-ImageHeight="50px">
                                <HeaderStyle HorizontalAlign="Center" />
                                <PropertiesBinaryImage ImageWidth="180" ImageHeight="50" EnableServerResize="True">
                                    <EditingSettings Enabled="false" />
                                </PropertiesBinaryImage>
                            </dx:GridViewDataBinaryImageColumn>
                            <%--<dx:GridViewDataTextColumn Caption="ORIGENDSVU" FieldName="ORIGENDSVU" ReadOnly="True" VisibleIndex="10" Width="140px" >
                                <HeaderStyle HorizontalAlign="Left" ForeColor="#FFF"  />
                                <SettingsHeaderFilter Mode="CheckedList" />
                                <CellStyle HorizontalAlign="Left"></CellStyle>
                            </dx:GridViewDataTextColumn>--%>
                        </Columns>
                        <Toolbars>
                        <dx:GridViewToolbar Name="Toolbar1" EnableAdaptivity="true" ItemAlign="Left">
                            <Items>
                                <dx:GridViewToolbarItem Name="Links">
                                <Template>
                                    <asp:LinkButton ID="lkb_Nuevo" runat="server" OnClick="lkb_Nuevo_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button" ToolTip="Nuevo Representate Legal" data-toggle="tooltip">
                                        <span class="glyphicon glyphicon-plus"></span>&nbsp;&nbsp;Nuevo
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="lkb_Editar" runat="server" OnClick="lkb_Editar_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button" ToolTip="Editar Representate Legal" data-toggle="tooltip">
                                        <span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp;Editar
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="lkb_Eliminar" runat="server" OnClick="lkb_Eliminar_Click" OnClientClick="alertQuestion('¿Desea eliminar este representante legal?', this); return false;" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button" ToolTip="Eliminar Representate Legal" data-toggle="tooltip">
                                        <span class="glyphicon glyphicon-remove"></span>&nbsp;&nbsp;Eliminar
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="lkb_Excel" runat="server" OnClick="lkb_Excel_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button" ToolTip="Exportar a Excel" data-toggle="tooltip">
                                        <span class="glyphicon glyphicon-list"></span>&nbsp;&nbsp;Excel
                                    </asp:LinkButton>
                                    <%--<asp:LinkButton ID="lkb_LimpiarFiltros" runat="server" OnClick="lkb_LimpiarFiltros_Click" CssClass="btn btn-primary btn-sm txt-sm" ToolTip="Limpiar Filtro" data-toggle="tooltip">
                                        <span class="glyphicon glyphicon-erase"></span>&nbsp;&nbsp;Limpiar Filtros
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="lkb_Actualizar" runat="server" OnClick="lkb_Actualizar_Click" CssClass="btn btn-primary btn-sm txt-sm" ToolTip="Actualizar" data-toggle="tooltip">
                                             <span class="glyphicon glyphicon-refresh"></span>&nbsp;&nbsp;Actualizar
                                    </asp:LinkButton>--%>                           
                                </Template>
                               </dx:GridViewToolbarItem>           
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
                    </asp:Panel>                       
                    <dx:ASPxCallback ID="ASPxCallback1" runat="server" ClientInstanceName="Callback" >
                        <ClientSideEvents  CallbackComplete="function(s, e) { System.Threading.Thread.Sleep(3000); LoadingPanel1.Hide(); }" />
                    </dx:ASPxCallback>
                    <dx:ASPxLoadingPanel ID="LoadingPanel1" runat="server" ClientInstanceName="LoadingPanel1"
                        Modal="True" ViewStateMode="Enabled" Theme="MaterialCompact" >
                    </dx:ASPxLoadingPanel>
                    <dx:ASPxLabel ID="ASPx_Mensaje" runat="server" Text="" Visible="false"></dx:ASPxLabel>
                    <dx:ASPxLabel ID="lblCadena" runat="server" Visible="false"/>
                 <%--</div>--%>                                        
             </div>
         </div>
    </div>
    <br /><br />   
    <button id="btnModal" type="button" data-toggle="modal" data-target="#Modal" data-whatever="Nuevo" onclick="return false;" style="display: none;"></button>
    <div class="modal fade" id="Modal" tabindex="-1" role="dialog" aria-labelledby="ModalTitulo" data-backdrop="static" >
        <div class="modal-dialog modal-lg" role="document" >
            <div class="modal-content">
                <div class="modal-header">
                    <h4 id="ModalTitulo" class="modal-title" runat="server"></h4>                    
                </div>
                <asp:Panel ID="Panel2" runat="server">
                    <div class="modal-body">
                        <div class="row">
                            <div class="form-group col-md-12">                                
                                <div class="col-md-3">
                                    &nbsp;<label id="Label1" runat="server" style="font-size:11px">* Nombre</label>
                                    <asp:TextBox ID="txt_Nombre" runat="server" CssClass="form-control input-sm" MaxLength="30" placeholder="* Nombre" Font-Size="11px" Width="100%"></asp:TextBox>
                                </div>
                                <div class="col-md-3">
                                    &nbsp;<label id="Label2" runat="server" style="font-size:11px">* Apellido Paterno</label>
                                    <asp:TextBox ID="txt_Paterno" runat="server" CssClass="form-control input-sm" MaxLength="20" placeholder="* Apellido Paterno" Font-Size="11px" Width="100%"></asp:TextBox>    
                                </div>
                                <div class="col-md-3">
                                   &nbsp;<label id="Label3" runat="server" style="font-size:11px">* Apellido Materno</label>
                                   <asp:TextBox ID="txt_Materno" runat="server" CssClass="form-control input-sm" MaxLength="20" placeholder="* Apellido Materno" Font-Size="11px" Width="100%"></asp:TextBox>    
                                </div>
                                <div class="col-md-3">
                                   &nbsp;<label id="Label4" runat="server" style="font-size:11px">* RFC</label>
                                   <asp:TextBox ID="txt_rfc" runat="server" CssClass="form-control input-sm" MaxLength="20" placeholder="* RFC" Font-Size="11px" Width="100%"></asp:TextBox>    
                                </div>                                
                            </div>
                            <div class="form-group col-md-12">                                
                                <div class="col-md-3">
                                    &nbsp;<label id="Label5" runat="server" style="font-size:11px">* Apartir de</label>
                                    <dx:ASPxDateEdit ID="ApartirDe" runat="server" EditFormat="Custom" Date="" Width="120px" Height="30px" Theme="MaterialCompact" 
                                        Font-Size="11px" CssClass="bordes_curvos" NullText="* A partir de" DisplayFormatString="dd/MM/yyyy">
                                        <CalendarProperties EnableMonthNavigation="True" EnableYearNavigation="True" ShowClearButton="False" ShowDayHeaders="True" ShowTodayButton="False" ShowWeekNumbers="False">
                                             <MonthGridPaddings Padding="0px" />
                                             <Style Font-Size="10px"></Style>
                                         </CalendarProperties>
                                    </dx:ASPxDateEdit>
                                </div>
                                <div class="col-md-3">
                                    &nbsp;<label id="Label6" runat="server" style="font-size:11px">* Hasta</label>
                                    <dx:ASPxDateEdit ID="Hasta" runat="server" EditFormat="Custom" Date="" Width="120px" Height="30px" Theme="MaterialCompact" 
                                        Font-Size="11px" CssClass="bordes_curvos" NullText="* Hasta" DisplayFormatString="dd/MM/yyyy">
                                        <CalendarProperties EnableMonthNavigation="True" EnableYearNavigation="True" ShowClearButton="False" ShowDayHeaders="True" ShowTodayButton="False" ShowWeekNumbers="False">
                                             <MonthGridPaddings Padding="0px" />
                                             <Style Font-Size="10px"></Style>
                                         </CalendarProperties>
                                    </dx:ASPxDateEdit>
                                </div>
                                <div class="col-md-3">
                                   &nbsp;<label id="Label7" runat="server" style="font-size:11px">Correo</label>
                                   <asp:TextBox ID="txt_correo" runat="server" CssClass="form-control input-sm" MaxLength="50" placeholder="Correo" Font-Size="11px" Width="100%"></asp:TextBox>    
                                </div>
                                <div class="col-md-3">
                                   &nbsp;<label id="Label8" runat="server" style="font-size:11px">Teléfono</label>
                                   <asp:TextBox ID="txt_tel" runat="server" CssClass="form-control input-sm" MaxLength="15" placeholder="Teléfono" Font-Size="11px" Width="100%"></asp:TextBox>    
                                </div>                                
                            </div>
                            <div class="form-group col-md-12">
                                <div class="col-md-3">
                                    &nbsp;<label id="Label9" runat="server" style="font-size:11px">Firma</label>
                                    <dx:ASPxBinaryImage ID="BinaryImage" ClientInstanceName="ClientBinaryImage" Width="200" Height="80"  
                                    ShowLoadingImage="true" LoadingImageUrl="~/Content/Loading.gif" runat="server" EditingSettings-EmptyValueText="firma" Font-Size="10px">
                                    <EditingSettings Enabled="true">
                                        <UploadSettings >
                                            <UploadValidationSettings AllowedFileExtensions=".bmp"></UploadValidationSettings>
                                        </UploadSettings>
                                    </EditingSettings>
                                </dx:ASPxBinaryImage>
                                </div>
                                <div class="col-md-9"></div>                                
                            </div>                            
                            <div class="col-md-12">
                                <div class="form-group text-right" style="position: relative; width: 100%; float: left;">
                                    <asp:Label Text="* Campo obligatorio" Font-Italic="true" runat="server" CssClass="asp-label" ForeColor="Red" />
                                </div>
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