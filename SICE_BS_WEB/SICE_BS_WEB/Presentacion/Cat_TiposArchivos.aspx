<%@ Page Language="C#" MasterPageFile="~/Presentacion/Principal.Master" AutoEventWireup="true" CodeBehind="Cat_TiposArchivos.aspx.cs" Inherits="SICE_BS_WEB.Presentacion.Cat_TiposArchivos" %>

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

        //Se usa para palomear el checkbox seleccionado y despalomear los demás en GridCP
        function chkConsultarClick(s, e, index) {
            var btnEdit = document.getElementById('ContentSection_lkb_EditaCP')
            var btnDelete = document.getElementById('ContentSection_lkb_EliminarCP')

            if (!btnEdit.classList.contains('disabled'))
                btnEdit.classList.add('disabled');

            if (!btnDelete.classList.contains('disabled'))
                btnDelete.classList.add('disabled');

            //document.getElementById('check');
            var cheked = s.GetChecked();
            var row = GridCP.GetDataRow(index);


            var chkConsultar = eval("chkConsultar" + index);
            var valor = chkConsultar.GetChecked();
            chkConsultar.SetChecked(valor);

            for (var i = 0; i < GridCP.GetVisibleRowsOnPage() ; i++) {
                if (i == index) {
                    chkConsultar = eval("chkConsultar" + i);
                    chkConsultar.SetChecked(valor);

                    if (valor == true) {
                        btnEdit.classList.remove('disabled');
                        btnDelete.classList.remove('disabled');
                    }
                }
                else {
                    try {
                        chkConsultar = eval("chkConsultar" + i);
                        chkConsultar.SetChecked(false);
                    }
                    catch (err) { }
                }
            }
        }

        //Se usa para palomear el checkbox seleccionado y despalomear los demás en GridIG
        function chkConsultarIGClick(s, e, index) {
            var btnEdit = document.getElementById('ContentSection_lkb_EditaIG')
            var btnDelete = document.getElementById('ContentSection_lkb_EliminarIG')

            if (!btnEdit.classList.contains('disabled'))
                btnEdit.classList.add('disabled');

            if (!btnDelete.classList.contains('disabled'))
                btnDelete.classList.add('disabled');

            //document.getElementById('check');
            var cheked = s.GetChecked();
            var row = GridIG.GetDataRow(index);


            var chkConsultarIG = eval("chkConsultarIG" + index);
            var valor = chkConsultarIG.GetChecked();
            chkConsultarIG.SetChecked(valor);

            for (var i = 0; i < GridIG.GetVisibleRowsOnPage() ; i++) {
                if (i == index) {
                    chkConsultarIG = eval("chkConsultarIG" + i);
                    chkConsultarIG.SetChecked(valor);

                    if (valor == true) {
                        btnEdit.classList.remove('disabled');
                        btnDelete.classList.remove('disabled');
                    }
                }
                else {
                    try {
                        chkConsultarIG = eval("chkConsultarIG" + i);
                        chkConsultarIG.SetChecked(false);
                    }
                    catch (err) { }
                }
            }
        }

        //Se usa para palomear el checkbox seleccionado y despalomear los demás en GridIP
        function chkConsultarIPClick(s, e, index) {
            var btnEdit = document.getElementById('ContentSection_lkb_EditaIP')
            var btnDelete = document.getElementById('ContentSection_lkb_EliminarIP')

            if (!btnEdit.classList.contains('disabled'))
                btnEdit.classList.add('disabled');

            if (!btnDelete.classList.contains('disabled'))
                btnDelete.classList.add('disabled');

            //document.getElementById('check');
            var cheked = s.GetChecked();
            var row = GridIP.GetDataRow(index);


            var chkConsultarIP = eval("chkConsultarIP" + index);
            var valor = chkConsultarIP.GetChecked();
            chkConsultarIP.SetChecked(valor);

            for (var i = 0; i < GridIP.GetVisibleRowsOnPage() ; i++) {
                if (i == index) {
                    chkConsultarIP = eval("chkConsultarIP" + i);
                    chkConsultarIP.SetChecked(valor);

                    if (valor == true) {
                        btnEdit.classList.remove('disabled');
                        btnDelete.classList.remove('disabled');
                    }
                }
                else {
                    try {
                        chkConsultarIP = eval("chkConsultarIP" + i);
                        chkConsultarIP.SetChecked(false);
                    }
                    catch (err) { }
                }
            }
        }

        //Se usa para palomear el checkbox seleccionado y despalomear los demás en GridIN
        function chkConsultarINClick(s, e, index) {
            var btnEdit = document.getElementById('ContentSection_lkb_EditaIN')
            var btnDelete = document.getElementById('ContentSection_lkb_EliminarIN')

            if (!btnEdit.classList.contains('disabled'))
                btnEdit.classList.add('disabled');

            if (!btnDelete.classList.contains('disabled'))
                btnDelete.classList.add('disabled');

            //document.getElementById('check');
            var cheked = s.GetChecked();
            var row = GridIN.GetDataRow(index);


            var chkConsultarIN = eval("chkConsultarIN" + index);
            var valor = chkConsultarIN.GetChecked();
            chkConsultarIN.SetChecked(valor);

            for (var i = 0; i < GridIN.GetVisibleRowsOnPage() ; i++) {
                if (i == index) {
                    chkConsultarIN = eval("chkConsultarIN" + i);
                    chkConsultarIN.SetChecked(valor);

                    if (valor == true) {
                        btnEdit.classList.remove('disabled');
                        btnDelete.classList.remove('disabled');
                    }
                }
                else {
                    try {
                        chkConsultarIN = eval("chkConsultarIN" + i);
                        chkConsultarIN.SetChecked(false);
                    }
                    catch (err) { }
                }
            }
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
                        AutoGenerateColumns="False" Width="100%" Settings-HorizontalScrollBarMode="Auto" KeyFieldName="TIPOARCHIVOKEY" SettingsPager-Position="TopAndBottom"
                        Styles-Header-Font-Size="11px" Styles-Cell-Font-Size="11px" >
                        <SettingsAdaptivity AdaptivityMode="HideDataCellsWindowLimit" HideDataCellsAtWindowInnerWidth="800" 
                            AdaptiveDetailColumnCount="1" AllowOnlyOneAdaptiveDetailExpanded="True">
                            <AdaptiveDetailLayoutProperties colcount="1">
                                <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit"  SwitchToSingleColumnAtWindowInnerWidth="600" />
                            </AdaptiveDetailLayoutProperties>
                        </SettingsAdaptivity> 
                        <SettingsResizing ColumnResizeMode="Control" />
                         <SettingsBehavior AllowSelectByRowClick="true" AllowSelectSingleRowOnly="true" />
                        <Settings ShowFooter="False" ShowHeaderFilterButton="true" ShowFilterRowMenu="false"  />                                          
                        <Styles>                                                                                      
                            <SelectedRow BackColor="#73C000" ForeColor="#FFFFFF" />
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
                            <dx:GridViewDataTextColumn FieldName="TIPOARCHIVOKEY" ReadOnly="True"  Visible="false" >                                
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="TIPO DE ARCHIVO" FieldName="TIPOS DE ARCHIVO" ReadOnly="True" VisibleIndex="1" Width="250px" >
                                <HeaderStyle HorizontalAlign="Left"   />
                                <SettingsHeaderFilter Mode="CheckedList" />
                                <CellStyle HorizontalAlign="Left"></CellStyle>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="SUFIJO" FieldName="SUFIJO" ReadOnly="True" VisibleIndex="2" Width="80px" >
                                <HeaderStyle HorizontalAlign="Left"   />
                                <SettingsHeaderFilter Mode="CheckedList" />
                                <CellStyle HorizontalAlign="Left"></CellStyle>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="TIPO DE</br>OPERACIÓN" FieldName="TIPO DE OPERACION" ReadOnly="True" VisibleIndex="3" Width="115px" >
                                <HeaderStyle HorizontalAlign="Left"   />
                                <SettingsHeaderFilter Mode="CheckedList" />
                                <CellStyle HorizontalAlign="Left"></CellStyle>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="OBLIGATORIO</br>AA" FieldName="OBLIGATORIO" ReadOnly="True" VisibleIndex="4" Width="130px" >
                                <HeaderStyle HorizontalAlign="Center"   />
                                <SettingsHeaderFilter Mode="CheckedList" />
                                <CellStyle HorizontalAlign="Center"></CellStyle>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="OBLIGATORIO</br>COMPAÑÍA" FieldName="OBLIGATORIOC" ReadOnly="True" VisibleIndex="5" Width="130px" >
                                <HeaderStyle HorizontalAlign="Center"   />
                                <SettingsHeaderFilter Mode="CheckedList" />
                                <CellStyle HorizontalAlign="Center"></CellStyle>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="CLAVE</br>PEDIMENTO" FieldName="CLAVEPEDIMENTO" ReadOnly="True" VisibleIndex="6" Width="150px" >
                                <HeaderStyle HorizontalAlign="Center"   />
                                <SettingsHeaderFilter Mode="CheckedList" />
                                <CellStyle HorizontalAlign="Center"></CellStyle>
                            </dx:GridViewDataTextColumn>                            
                            <dx:GridViewDataTextColumn Caption="IDENTIFICADOR</br>GENERAL" FieldName="IDENTIFICADOR" ReadOnly="True" VisibleIndex="7" Width="155px" >
                                <HeaderStyle HorizontalAlign="Center"   />
                                <SettingsHeaderFilter Mode="CheckedList" />
                                <CellStyle HorizontalAlign="Center"></CellStyle>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="IDENTIFICADOR</br>PARTIDA" FieldName="IDENTIFICADORP" ReadOnly="True" VisibleIndex="8" Width="155px" >
                                <HeaderStyle HorizontalAlign="Center"   />
                                <SettingsHeaderFilter Mode="CheckedList" />
                                <CellStyle HorizontalAlign="Center"></CellStyle>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="INCOTERM" FieldName="INCOTERM" ReadOnly="True" VisibleIndex="9" Width="110px" >
                                <HeaderStyle HorizontalAlign="Center"   />
                                <SettingsHeaderFilter Mode="CheckedList" />
                                <CellStyle HorizontalAlign="Center"></CellStyle>
                            </dx:GridViewDataTextColumn>               
                        </Columns>
                        <Toolbars>
                        <dx:GridViewToolbar Name="Toolbar1" EnableAdaptivity="true" ItemAlign="Left">
                            <Items>
                                <dx:GridViewToolbarItem Name="Links">
                                <Template>
                                    <asp:LinkButton ID="lkb_Nuevo" runat="server" OnClick="lkb_Nuevo_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button" ToolTip="Nuevo Tipo de Archivo" data-toggle="tooltip">
                                        <span class="glyphicon glyphicon-plus"></span>&nbsp;&nbsp;Nuevo
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="lkb_Editar" runat="server" OnClick="lkb_Editar_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button" ToolTip="Editar Tipo de Archivo" data-toggle="tooltip">
                                        <span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp;Editar
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="lkb_Eliminar" runat="server" OnClick="lkb_Eliminar_Click" OnClientClick="alertQuestion1('¿Desea eliminar este tipo de archivo?', this); return false;" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button" ToolTip="Eliminar Tipo de Archivo" data-toggle="tooltip">
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
                    <asp:Label ID="lblTP" runat="server" Visible="false" />
                    <asp:HiddenField ID="HfCPBorrar" runat="server" />
                    <asp:HiddenField ID="HfIGBorrar" runat="server" />
                    <asp:HiddenField ID="HfIPBorrar" runat="server" />
                    <asp:HiddenField ID="HfINBorrar" runat="server" />
                    
                 <%--</div>--%>                                        
             </div>
         </div>
    </div>
    <br /><br />   
    <button id="btnModal" type="button" data-toggle="modal" data-target="#Modal" data-whatever="Nuevo" onclick="return false;" style="display: none;"></button>
    <div class="modal fade" id="Modal" tabindex="-1" role="dialog" aria-labelledby="ModalTitulo" data-backdrop="static">
        <div class="modal-dialog modal-lg" role="document" >
            <div class="modal-content">
                <div class="modal-header">
                    <h4 id="ModalTitulo" class="modal-title" runat="server"></h4>                    
                </div>
                <asp:Panel ID="Panel2" runat="server">
                    <div class="modal-body">
                        <div class="row">
                            <div class="form-group col-md-12">
                                <div class="form-group col-md-12">
                                    &nbsp;<label id="Label13" runat="server" style="font-size:11px">* Tipo de Archivo</label>
                                    <asp:TextBox ID="txt_tipoArchivo" runat="server" CssClass="form-control input-sm" MaxLength="150" placeholder="* Tipo de Archivo" Width="100%"></asp:TextBox>
                                </div>                                
                            </div>
                            <div class="form-group col-md-12">
                                <div class="form-group col-md-3">
                                    &nbsp;<label id="Label1" runat="server" style="font-size:11px">* Sufijo</label>
                                    <asp:TextBox ID="txt_sufijo" runat="server" CssClass="form-control input-sm" MaxLength="10" placeholder="* Sufijo" Width="100%"></asp:TextBox>
                                </div>                              
                                <div class="form-group col-md-3">
                                    &nbsp;<label id="Label2" runat="server" style="font-size:11px">* Tipo Operación</label>
                                    <dx:ASPxComboBox ID="cmbTipoOperacion" Caption="" runat="server" Height="30px" NullText="* Tipo Operación" DataSecurityMode="Default"
                                        Width="100%" Font-Size="12px" Font-Names="Arial,Helvetica" CssClass="bordes_curvos_derecha" Theme="MaterialCompact" ForeColor="#6B5555">                                                
                                        <Items>
                                            <dx:ListEditItem Text="Ambas" Value="1" />
                                            <dx:ListEditItem Text="Exportación" Value="2" />
                                            <dx:ListEditItem Text="Importación" Value="3" />                                                   
                                        </Items>
                                        <ClearButton DisplayMode="Never" />
                                    </dx:ASPxComboBox>
                                </div>
                                <div class="form-group col-md-3">
                                    &nbsp;<label id="Label3" runat="server" style="font-size:11px">* Obligatorio AA</label>
                                    <dx:ASPxComboBox ID="cmbObligatorio" Caption="" runat="server" Height="30px" NullText="* Obligatorio AA" DataSecurityMode="Default"
                                        Width="100%" Font-Size="12px" Font-Names="Arial,Helvetica" CssClass="bordes_curvos_derecha" Theme="MaterialCompact" ForeColor="#6B5555">                                                
                                        <Items>
                                            <dx:ListEditItem Text="Si" Value="1" />
                                            <dx:ListEditItem Text="No" Value="2" />                                                    
                                        </Items>
                                        <ClearButton DisplayMode="Never" />
                                    </dx:ASPxComboBox>
                                </div>
                                <div class="form-group col-md-3">
                                    &nbsp;<label id="Label4" runat="server" style="font-size:11px">Obligatorio Compañía</label>
                                    <dx:ASPxComboBox ID="cmbObligatorioC" Caption="" runat="server" Height="30px" NullText=" Obligatorio Compañía" DataSecurityMode="Default"
                                        Width="100%" Font-Size="12px" Font-Names="Arial,Helvetica" CssClass="bordes_curvos_derecha" Theme="MaterialCompact" ForeColor="#6B5555">                                                
                                        <Items>
                                            <dx:ListEditItem Text="Si" Value="1" />
                                            <dx:ListEditItem Text="No" Value="2" />                                                    
                                        </Items>
                                        <ClearButton DisplayMode="Never" />
                                    </dx:ASPxComboBox>
                                </div>
                            </div>
                            <div class="form-group col-md-12">
                                <div class="form-group col-md-3">
                                    &nbsp;<label id="Label5" runat="server" style="font-size:11px">Clave Pedimento</label>
                                    <table style="width:100%">
                                        <tr>
                                            <td style="width:88%">
                                                <asp:TextBox ID="txt_ClavePedimento" runat="server" CssClass="form-control input-sm" placeholder="Clave Pedimento" MaxLength="5" Width="100%" Enabled="false"></asp:TextBox>
                                            </td>
                                            <td style="width:2%"></td>
                                            <td style="width:10%">
                                                <asp:LinkButton ID="lkb_ClavePedimento" runat="server" OnClick="lkb_ClavePedimento_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button" Width="30px">
                                                    <span class="glyphicon glyphicon-option-horizontal"></span>&nbsp;&nbsp;
                                                </asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </div>                              
                                <div class="form-group col-md-3">
                                    &nbsp;<label id="Label6" runat="server" style="font-size:11px">Identificador General</label>
                                    <table style="width:100%">
                                        <tr>
                                            <td style="width:88%">
                                                <asp:TextBox ID="txt_IG" runat="server" CssClass="form-control input-sm" placeholder="Identificador General" Width="100%" Enabled="false"></asp:TextBox>
                                            </td>
                                            <td style="width:2%"></td>
                                            <td style="width:5%">
                                                <asp:LinkButton ID="lkb_IdentificadorGeneral" runat="server" OnClick="lkb_IdentificadorGeneral_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button" Width="30px">
                                                    <span class="glyphicon glyphicon-option-horizontal"></span>&nbsp;
                                                </asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="form-group col-md-3">
                                    &nbsp;<label id="Label7" runat="server" style="font-size:11px">Identificador Partida</label>
                                    <table style="width:100%">
                                        <tr>
                                            <td style="width:88%">
                                                <asp:TextBox ID="txt_IP" runat="server" CssClass="form-control input-sm" placeholder="Identificador Partida" Width="100%" Enabled="false"></asp:TextBox>
                                            </td>
                                            <td style="width:2%"></td>
                                            <td style="width:5%">
                                                <asp:LinkButton ID="lkb_IdentificadorPartida" runat="server" OnClick="lkb_IdentificadorPartida_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button" Width="30px">
                                                    <span class="glyphicon glyphicon-option-horizontal"></span>
                                                </asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="form-group col-md-3">
                                    &nbsp;<label id="Label8" runat="server" style="font-size:11px">INCOTERM</label>
                                    <table style="width:100%">
                                        <tr>
                                            <td style="width:88%">
                                                <asp:TextBox ID="txt_Incoterm" runat="server" CssClass="form-control input-sm" placeholder="INCOTERM" Width="100%" Enabled="false"></asp:TextBox>
                                            </td>
                                            <td style="width:2%"></td>
                                            <td style="width:5%">
                                                <asp:LinkButton ID="lkb_INCOTERM" runat="server" OnClick="lkb_INCOTERM_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button" Width="30px">
                                                    <span class="glyphicon glyphicon-option-horizontal"></span>
                                                </asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div class="form-group col-md-12">
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
    
    <button id="btnModalCP" type="button" data-toggle="modal" data-target="#ModalCP" data-whatever="Nuevo" onclick="return false;" style="display: none;"></button>
    <div class="modal fade" id="ModalCP" tabindex="-1" role="dialog" aria-labelledby="ModalTitulo" data-backdrop="static">
        <div class="modal-dialog modal-md" role="document" >
            <div class="modal-content">
                <div class="modal-header">
                    <h4 id="ModalTituloCP" class="modal-title" runat="server"></h4>                    
                </div>
                <asp:Panel ID="Panel3" runat="server">
                    <div class="modal-body">
                        <div class="row" style="padding-left:29px;padding-right:29px">
                            
                            <asp:LinkButton ID="lkb_NuevoCP" runat="server" OnClick="lkb_NuevoCP_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button">
                                <span class="glyphicon glyphicon-plus"></span>&nbsp;&nbsp;Nuevo
                            </asp:LinkButton>
                            <asp:LinkButton ID="lkb_EditaCP" runat="server" OnClick="lkb_EditaCP_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button">
                                <span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp;Editar
                            </asp:LinkButton>
                            <asp:LinkButton ID="lkb_EliminarCP" runat="server" OnClick="lkb_EliminarCP_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button" >
                                <span class="glyphicon glyphicon-remove"></span>&nbsp;&nbsp;Eliminar
                            </asp:LinkButton>
                            <dx:ASPxGridView ID="GridCP" runat="server" EnableTheming="True" Theme="SoftOrange" Settings-HorizontalScrollBarMode="Auto"
                                OnCustomCallback="GridCP_CustomCallback" EnableCallBacks="false" ClientInstanceName="GridCP"
                                KeyFieldName="TCPKEY" AutoGenerateColumns="False" Width="100%" 
                                OnCustomButtonCallback="GridCP_CustomButtonCallback">
                                <SettingsAdaptivity AdaptivityMode="HideDataCellsWindowLimit" HideDataCellsAtWindowInnerWidth="800" 
                                    AdaptiveDetailColumnCount="1" AllowOnlyOneAdaptiveDetailExpanded="True">
                                    <AdaptiveDetailLayoutProperties colcount="2">
                                        <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit"  SwitchToSingleColumnAtWindowInnerWidth="600" />
                                    </AdaptiveDetailLayoutProperties>
                                </SettingsAdaptivity>
                                <SettingsResizing ColumnResizeMode="Control" />
                                <%--<SettingsBehavior AllowSelectByRowClick="true" AllowSelectSingleRowOnly="true" />--%>
                                <Styles>
                                <SelectedRow CssClass="background_color_btn background_texto_btn" />
                                <Row Font-Size="11px" />
                                <AlternatingRow Enabled="True" />
                                <PagerTopPanel Paddings-PaddingBottom="3px">
                                    <Paddings PaddingBottom="3px" />
                                    </PagerTopPanel>
                                </Styles>
                                <SettingsPager Position="TopAndBottom" PageSize="10">
                                </SettingsPager>
                                <Columns>
                                    <dx:GridViewDataMemoColumn FieldName="TCPKEY" Visible="False"></dx:GridViewDataMemoColumn>
                                    <%--<dx:GridViewCommandColumn Caption="#" VisibleIndex="0" ButtonRenderMode="Link" Width="50px">
                                        <CustomButtons>
                                            <dx:GridViewCommandColumnCustomButton ID="btnEditar" Text="Editar">                                                                                                                                                                       
                                                <Styles>                                                                                    
                                                    <Style Font-Bold="False" ForeColor="Black" Font-Size="11px">
                                                        <HoverStyle Font-Bold="False" ForeColor="Black">
                                                        </HoverStyle>
                                                    </Style>
                                                </Styles>
                                            </dx:GridViewCommandColumnCustomButton>
                                        </CustomButtons>                                        
                                        <HeaderStyle HorizontalAlign="Center"/>
                                        <CellStyle HorizontalAlign="Center" />
                                    </dx:GridViewCommandColumn>
                                    <dx:GridViewCommandColumn Caption="#" VisibleIndex="1" ButtonRenderMode="Link" Width="65px">                                        
                                        <CustomButtons>
                                            <dx:GridViewCommandColumnCustomButton ID="btnEliminar" Text="Eliminar">                                                                                                                                                                       
                                                <Styles>                                                                                    
                                                    <Style Font-Bold="False" ForeColor="Black" Font-Size="11px">
                                                        <HoverStyle Font-Bold="False" ForeColor="Black">
                                                        </HoverStyle>
                                                    </Style>
                                                </Styles>
                                            </dx:GridViewCommandColumnCustomButton>
                                        </CustomButtons>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <CellStyle HorizontalAlign="Center" />
                                    </dx:GridViewCommandColumn>--%>
                                    <dx:GridViewDataColumn Caption="Seleccione" VisibleIndex="1" Width="75px" >
                                       <HeaderStyle HorizontalAlign="Center" />
                                       <CellStyle HorizontalAlign="Center"></CellStyle>
                                       <DataItemTemplate>
                                           <dx:ASPxCheckBox ID="chkConsultar" ClientInstanceName="chkConsultar" Enabled="true" runat="server" OnInit="chkConsultar_Init">                                                                
                                           </dx:ASPxCheckBox>
                                       </DataItemTemplate>
                                    </dx:GridViewDataColumn>
                                    <dx:GridViewDataTextColumn Caption="Clave</br>Pedimento" FieldName="CLAVEPEDIMENTO" VisibleIndex="2" Width="109px">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <CellStyle HorizontalAlign="Center" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataColumn Caption="Identificador</br>General" FieldName="IDENTIFICADOR" VisibleIndex="3" Width="109px">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <CellStyle HorizontalAlign="Center" />
                                    </dx:GridViewDataColumn>
                                    <dx:GridViewDataMemoColumn Caption="Identificador</br>Partida" FieldName="IDENTIFICADORP" VisibleIndex="4" Width="109px">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <CellStyle HorizontalAlign="Center" />
                                    </dx:GridViewDataMemoColumn>
                                    <dx:GridViewDataColumn Caption="INCOTERM" FieldName="INCOTERM" VisibleIndex="5" Width="109px">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <CellStyle HorizontalAlign="Center" />
                                    </dx:GridViewDataColumn>
                                </Columns>
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
                            <%--<asp:ObjectDataSource ID="ObjectDataSourceCP" runat="server" OldValuesParameterFormatString="original_{0}"
                                SelectMethod="TraeClavesPedimentos" InsertMethod="GuardaClavePedimento" UpdateMethod="ActualizaClavePedimento" 
                                TypeName="SICE_BS_WEB.Negocios.Catalogos">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="lblTP" Name="TPKEY" Type="Int64" />
                                </SelectParameters>
                                <InsertParameters>
                                    <asp:ControlParameter ControlID="lblTP" Name="TPKEY" Type="Int64" />
                                    <asp:ControlParameter ControlID="GridCP" Name="CLAVEPEDIMENTO" Type="String" />
                                    <asp:ControlParameter ControlID="GridCP" Name="IDENTIFICADOR" Type="String" />
                                    <asp:ControlParameter ControlID="GridCP" Name="IDENTIFICADORP" Type="String" />
                                    <asp:ControlParameter ControlID="GridCP" Name="INCOTERM" Type="String" />
                                </InsertParameters>
                                <UpdateParameters>
                                    <asp:ControlParameter ControlID="lblTP" Name="TPKEY" Type="Int64" />
                                    <asp:ControlParameter ControlID="GridCP" Name="CLAVEPEDIMENTO" Type="String" />
                                    <asp:ControlParameter ControlID="GridCP" Name="IDENTIFICADOR" Type="String" />
                                    <asp:ControlParameter ControlID="GridCP" Name="IDENTIFICADORP" Type="String" />
                                    <asp:ControlParameter ControlID="GridCP" Name="INCOTERM" Type="String" />
                                </UpdateParameters>
                            </asp:ObjectDataSource>--%>
                                   
                        </div>
                    </div>                
                    <div class="modal-footer">
                        <div class="col-md-12 col-md-offset-0" style="text-align: center;">
                            <asp:LinkButton ID="btnGuardarCP" runat="server" CssClass="btn btn-primary btn-sm" Text="Guardar" OnClick="btnGuardarCP_Click">
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
    
    <button id="btnModalIG" type="button" data-toggle="modal" data-target="#ModalIG" data-whatever="Nuevo" onclick="return false;" style="display: none;"></button>
    <div class="modal fade" id="ModalIG" tabindex="-1" role="dialog" aria-labelledby="ModalTitulo" data-backdrop="static">
        <div class="modal-dialog modal-sm" role="document" >
            <div class="modal-content">
                <div class="modal-header">
                    <h4 id="ModalTituloIG" class="modal-title" runat="server"></h4>                    
                </div>
                <asp:Panel ID="Panel5" runat="server">
                    <div class="modal-body">
                        <div class="row" style="padding-left:35px;padding-right:35px">
                            
                            <asp:LinkButton ID="lkb_NuevoIG" runat="server" OnClick="lkb_NuevoIG_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button">
                                <span class="glyphicon glyphicon-plus"></span>&nbsp;&nbsp;Nuevo
                            </asp:LinkButton>
                            <asp:LinkButton ID="lkb_EditaIG" runat="server" OnClick="lkb_EditaIG_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button">
                                <span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp;Editar
                            </asp:LinkButton>
                            <asp:LinkButton ID="lkb_EliminarIG" runat="server" OnClick="lkb_EliminarIG_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button" >
                                <span class="glyphicon glyphicon-remove"></span>&nbsp;&nbsp;Eliminar
                            </asp:LinkButton>
                            <dx:ASPxGridView ID="GridIG" runat="server" EnableTheming="True" Theme="SoftOrange" Settings-HorizontalScrollBarMode="Auto"
                                OnCustomCallback="GridIG_CustomCallback" KeyFieldName="IGKEY" AutoGenerateColumns="False" Width="100%" 
                                OnCustomButtonCallback="GridIG_CustomButtonCallback" ClientInstanceName="GridIG">
                                <SettingsAdaptivity AdaptivityMode="HideDataCellsWindowLimit" HideDataCellsAtWindowInnerWidth="800" 
                                    AdaptiveDetailColumnCount="1" AllowOnlyOneAdaptiveDetailExpanded="True">
                                    <AdaptiveDetailLayoutProperties colcount="2">
                                        <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit"  SwitchToSingleColumnAtWindowInnerWidth="600" />
                                    </AdaptiveDetailLayoutProperties>
                                </SettingsAdaptivity>
                                <SettingsResizing ColumnResizeMode="Control" />
                                <Styles>
                                <SelectedRow CssClass="background_color_btn background_texto_btn" />
                                <Row Font-Size="11px" />
                                <AlternatingRow Enabled="True" />
                                <PagerTopPanel Paddings-PaddingBottom="3px">
                                    <Paddings PaddingBottom="3px" />
                                    </PagerTopPanel>
                                </Styles>
                                <SettingsPager Position="TopAndBottom" PageSize="10">
                                </SettingsPager>
                                <Columns>
                                    <dx:GridViewDataMemoColumn FieldName="IGKEY" Visible="False"></dx:GridViewDataMemoColumn>  
                                    <dx:GridViewDataColumn Caption="Seleccione" VisibleIndex="1" Width="80px" >
                                      <HeaderStyle HorizontalAlign="Center" />
                                      <CellStyle HorizontalAlign="Center"></CellStyle>
                                      <DataItemTemplate>
                                          <dx:ASPxCheckBox ID="chkConsultarIG" ClientInstanceName="chkConsultarIG" Enabled="true" runat="server" OnInit="chkConsultarIG_Init">                                                                
                                          </dx:ASPxCheckBox>
                                      </DataItemTemplate>
                                    </dx:GridViewDataColumn>                                                                     
                                    <dx:GridViewDataColumn Caption="Identificador</br>General" FieldName="IDENTIFICADOR" VisibleIndex="1" Width="120px">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <CellStyle HorizontalAlign="Center" />
                                    </dx:GridViewDataColumn>                                    
                                </Columns>
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
                                   
                        </div>
                    </div>                
                    <div class="modal-footer">
                        <div class="col-md-12 col-md-offset-0" style="text-align: center;">
                            <asp:LinkButton ID="LinkButton2" runat="server" CssClass="btn btn-primary btn-sm" Text="Guardar" OnClick="btnGuardarIG_Click">
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

    <button id="btnModalIG2" type="button" data-toggle="modal" data-target="#ModalIG2" data-whatever="Nuevo" onclick="return false;" style="display: none;"></button>
    <div class="modal fade" id="ModalIG2" tabindex="-1" role="dialog" aria-labelledby="ModalTitulo" data-backdrop="static">
        <div class="modal-dialog modal-sm" role="document" >
            <div class="modal-content">
                <div class="modal-header">
                    <h4 id="ModalTituloIG2" class="modal-title" runat="server"></h4>                    
                </div>
                <asp:Panel ID="Panel9" runat="server">
                    <div class="modal-body">
                        <div class="row">                            
                            <div id="div1" runat="server" class="form-group col-md-12">                                
                                &nbsp;<label id="Label9" runat="server" style="font-size:11px">Identificador General</label>
                                <asp:TextBox ID="txtIG2_IdentificadorGeneral" runat="server" CssClass="form-control input-sm" placeholder="Identificador General" Width="100%" MaxLength="10"></asp:TextBox>
                            </div>
                                   
                        </div>
                    </div>                
                    <div class="modal-footer">
                        <div class="col-md-12 col-md-offset-0" style="text-align: center;">
                            <asp:LinkButton ID="btnGuardarIG2" runat="server" CssClass="btn btn-primary btn-sm" Text="Guardar" OnClick="btnGuardarIG2_Click">
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

    <button id="btnModalIP" type="button" data-toggle="modal" data-target="#ModalIP" data-whatever="Nuevo" onclick="return false;" style="display: none;"></button>
    <div class="modal fade" id="ModalIP" tabindex="-1" role="dialog" aria-labelledby="ModalTitulo" data-backdrop="static">
        <div class="modal-dialog modal-sm" role="document" >
            <div class="modal-content">
                <div class="modal-header">
                    <h4 id="ModalTituloIP" class="modal-title" runat="server"></h4>                    
                </div>
                <asp:Panel ID="Panel6" runat="server">
                    <div class="modal-body">
                        <div class="row" style="padding-left:35px;padding-right:35px">
                            
                            <asp:LinkButton ID="lkb_NuevoIP" runat="server" OnClick="lkb_NuevoIP_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button">
                                <span class="glyphicon glyphicon-plus"></span>&nbsp;&nbsp;Nuevo
                            </asp:LinkButton>
                            <asp:LinkButton ID="lkb_EditaIP" runat="server" OnClick="lkb_EditaIP_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button">
                                <span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp;Editar
                            </asp:LinkButton>
                            <asp:LinkButton ID="lkb_EliminarIP" runat="server" OnClick="lkb_EliminarIP_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button" >
                                <span class="glyphicon glyphicon-remove"></span>&nbsp;&nbsp;Eliminar
                            </asp:LinkButton>
                            <dx:ASPxGridView ID="GridIP" runat="server" EnableTheming="True" Theme="SoftOrange" Settings-HorizontalScrollBarMode="Auto"
                                OnCustomCallback="GridIP_CustomCallback" KeyFieldName="IPKEY" AutoGenerateColumns="False" Width="100%" 
                                OnCustomButtonCallback="GridIP_CustomButtonCallback" ClientInstanceName="GridIP">
                                <SettingsAdaptivity AdaptivityMode="HideDataCellsWindowLimit" HideDataCellsAtWindowInnerWidth="800" 
                                    AdaptiveDetailColumnCount="1" AllowOnlyOneAdaptiveDetailExpanded="True">
                                    <AdaptiveDetailLayoutProperties colcount="2">
                                        <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit"  SwitchToSingleColumnAtWindowInnerWidth="600" />
                                    </AdaptiveDetailLayoutProperties>
                                </SettingsAdaptivity>
                                <SettingsResizing ColumnResizeMode="Control" />
                                <Styles>
                                <SelectedRow CssClass="background_color_btn background_texto_btn" />
                                <Row Font-Size="11px" />
                                <AlternatingRow Enabled="True" />
                                <PagerTopPanel Paddings-PaddingBottom="3px">
                                    <Paddings PaddingBottom="3px" />
                                    </PagerTopPanel>
                                </Styles>
                                <SettingsPager Position="TopAndBottom" PageSize="10">
                                </SettingsPager>
                                <Columns>
                                    <dx:GridViewDataMemoColumn FieldName="IPKEY" Visible="False"></dx:GridViewDataMemoColumn>  
                                    <dx:GridViewDataColumn Caption="Seleccione" VisibleIndex="1" Width="80px" >
                                      <HeaderStyle HorizontalAlign="Center" />
                                      <CellStyle HorizontalAlign="Center"></CellStyle>
                                      <DataItemTemplate>
                                          <dx:ASPxCheckBox ID="chkConsultarIP" ClientInstanceName="chkConsultarIP" Enabled="true" runat="server" OnInit="chkConsultarIP_Init">                                                                
                                          </dx:ASPxCheckBox>
                                      </DataItemTemplate>
                                    </dx:GridViewDataColumn>                                                                     
                                    <dx:GridViewDataColumn Caption="IDENTIFICADOR</br>PARTIDA" FieldName="IDENTIFICADORP" VisibleIndex="1" Width="120px">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <CellStyle HorizontalAlign="Center" />
                                    </dx:GridViewDataColumn>
                                </Columns>
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
                                   
                        </div>
                    </div>                
                    <div class="modal-footer">
                        <div class="col-md-12 col-md-offset-0" style="text-align: center;">
                            <asp:LinkButton ID="LinkButton3" runat="server" CssClass="btn btn-primary btn-sm" Text="Guardar" OnClick="btnGuardarIP_Click">
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

    <button id="btnModalIP2" type="button" data-toggle="modal" data-target="#ModalIP2" data-whatever="Nuevo" onclick="return false;" style="display: none;"></button>
    <div class="modal fade" id="ModalIP2" tabindex="-1" role="dialog" aria-labelledby="ModalTitulo" data-backdrop="static">
        <div class="modal-dialog modal-sm" role="document" >
            <div class="modal-content">
                <div class="modal-header">
                    <h4 id="ModalTituloIP2" class="modal-title" runat="server"></h4>                    
                </div>
                <asp:Panel ID="Panel10" runat="server">
                    <div class="modal-body">
                        <div class="row">                            
                            <div id="div2" runat="server" class="form-group col-md-12">                                
                                &nbsp;<label id="Label10" runat="server" style="font-size:11px">Identificador Partida</label>
                                <asp:TextBox ID="txtIP2_IdentificadorPartida" runat="server" CssClass="form-control input-sm" placeholder="Identificador Partida" Width="100%" MaxLength="10"></asp:TextBox>
                            </div>
                                   
                        </div>
                    </div>                
                    <div class="modal-footer">
                        <div class="col-md-12 col-md-offset-0" style="text-align: center;">
                            <asp:LinkButton ID="btnGuardarIP2" runat="server" CssClass="btn btn-primary btn-sm" Text="Guardar" OnClick="btnGuardarIP2_Click">
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

    <button id="btnModalCP2" type="button" data-toggle="modal" data-target="#ModalCP2" data-whatever="Nuevo" onclick="return false;" style="display: none;"></button>
    <div class="modal fade" id="ModalCP2" tabindex="-1" role="dialog" aria-labelledby="ModalTitulo" data-backdrop="static">
        <div class="modal-dialog modal-sm" role="document" >
            <div class="modal-content">
                <div class="modal-header">
                    <h4 id="ModalTituloCP2" class="modal-title" runat="server"></h4>                    
                </div>
                <asp:Panel ID="Panel4" runat="server">
                    <div class="modal-body">
                        <div class="row">
                            <div id="divCP" runat="server" class="form-group col-md-12">                               
                                &nbsp;<label id="lblT_CP" runat="server" style="font-size:11px">*Clave Pedimento</label>
                                <asp:TextBox ID="txtCP2_ClavePedimento" runat="server" CssClass="form-control input-sm" MaxLength="5" placeholder="* Clave Pedimento" Width="100%"></asp:TextBox>                               
                            </div>
                            <div id="divIG" runat="server" class="form-group col-md-12">
                                &nbsp;<label id="lblT_IG" runat="server" style="font-size:11px">Identificador General</label>
                                <asp:TextBox ID="txtCP2_IG" runat="server" CssClass="form-control input-sm" MaxLength="150" placeholder="Identificador General" Width="100%"></asp:TextBox>                                                                                             
                                
                            </div>
                            <div id="divIP" runat="server" class="form-group col-md-12">                                
                                &nbsp;<label id="lblT_IP" runat="server" style="font-size:11px">Identificador Partida</label>
                                <asp:TextBox ID="txtCP2_IP" runat="server" CssClass="form-control input-sm" placeholder="Identificador Partida" MaxLength="150" Width="100%"></asp:TextBox>
                            </div>
                            <div id="divIN" runat="server" class="form-group col-md-12">
                                &nbsp;<label id="lblT_IN" runat="server" style="font-size:11px">INCOTERM</label>
                                <table style="width:100%">
                                    <tr>
                                        <td style="width:88%">
                                            <asp:TextBox ID="txtCP2_Incoterm" runat="server" CssClass="form-control input-sm" placeholder="INCOTERM" Width="100%" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td style="width:2%"></td>
                                        <td style="width:5%">
                                            <asp:LinkButton ID="lkb_INCOTERMCP2" runat="server" OnClick="lkb_INCOTERMCP2_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button" Width="30px">
                                                <span class="glyphicon glyphicon-option-horizontal"></span>
                                            </asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>


                            </div>    
                        </div>
                    </div>                
                    <div class="modal-footer">
                        <div class="col-md-12 col-md-offset-0" style="text-align: center;">
                            <asp:LinkButton ID="btnGuardarCP2" runat="server" CssClass="btn btn-primary btn-sm" Text="Guardar" OnClick="btnGuardarCP2_Click">
                                <span class="glyphicon glyphicon-ok"></span>&nbsp;&nbsp;Guardar</asp:LinkButton>
                            <%--<asp:LinkButton ID="btnCancelarCP2" runat="server" CssClass="btn btn-primary btn-sm" Text="Cancelar" OnClick="btnCancelarCP2_Click">
                                <span class="glyphicon glyphicon-remove"></span>&nbsp;&nbsp;Cancelar</asp:LinkButton>--%>
                            <button type="button" class="btn btn-primary btn-sm" data-dismiss="modal">
                                <span class="glyphicon glyphicon-remove"></span>&nbsp;&nbsp;Cancelar
                            </button>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div>
    
    <button id="btnModalIN" type="button" data-toggle="modal" data-target="#ModalIN" data-whatever="Nuevo" onclick="return false;" style="display: none;"></button>
    <div class="modal fade" id="ModalIN" tabindex="-1" role="dialog" aria-labelledby="ModalTitulo" data-backdrop="static">
        <div class="modal-dialog modal-sm" role="document" >
            <div class="modal-content">
                <div class="modal-header">
                    <h4 id="ModalTituloIN" class="modal-title" runat="server"></h4>                    
                </div>
                <asp:Panel ID="Panel7" runat="server">
                    <div class="modal-body">
                        <div class="row" style="padding-left:35px;padding-right:35px">
                            
                            <asp:LinkButton ID="lkb_NuevoIN" runat="server" OnClick="lkb_NuevoIN_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button">
                                <span class="glyphicon glyphicon-plus"></span>&nbsp;&nbsp;Nuevo
                            </asp:LinkButton>
                            <asp:LinkButton ID="lkb_EditaIN" runat="server" OnClick="lkb_EditaIN_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button">
                                <span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp;Editar
                            </asp:LinkButton>
                            <asp:LinkButton ID="lkb_EliminarIN" runat="server" OnClick="lkb_EliminarIN_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button" >
                                <span class="glyphicon glyphicon-remove"></span>&nbsp;&nbsp;Eliminar
                            </asp:LinkButton>
                            <dx:ASPxGridView ID="GridIN" runat="server" EnableTheming="True" Theme="SoftOrange" Settings-HorizontalScrollBarMode="Auto"
                                OnCustomCallback="GridIN_CustomCallback" KeyFieldName="IKEY" AutoGenerateColumns="False" Width="100%" 
                                OnCustomButtonCallback="GridIN_CustomButtonCallback" ClientInstanceName="GridIN">
                                <SettingsAdaptivity AdaptivityMode="HideDataCellsWindowLimit" HideDataCellsAtWindowInnerWidth="800" 
                                    AdaptiveDetailColumnCount="1" AllowOnlyOneAdaptiveDetailExpanded="True">
                                    <AdaptiveDetailLayoutProperties colcount="2">
                                        <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit"  SwitchToSingleColumnAtWindowInnerWidth="600" />
                                    </AdaptiveDetailLayoutProperties>
                                </SettingsAdaptivity>
                                <SettingsResizing ColumnResizeMode="Control" />
                                <Styles>
                                <SelectedRow CssClass="background_color_btn background_texto_btn" />
                                <Row Font-Size="11px" />
                                <AlternatingRow Enabled="True" />
                                <PagerTopPanel Paddings-PaddingBottom="3px">
                                    <Paddings PaddingBottom="3px" />
                                    </PagerTopPanel>
                                </Styles>
                                <SettingsPager Position="TopAndBottom" PageSize="10">
                                </SettingsPager>
                                <Columns>
                                    <dx:GridViewDataMemoColumn FieldName="IKEY" Visible="False"></dx:GridViewDataMemoColumn>
                                    <%--<dx:GridViewCommandColumn Caption="#" VisibleIndex="0" ButtonRenderMode="Link" Width="50px">
                                        <CustomButtons>
                                            <dx:GridViewCommandColumnCustomButton ID="btnEditarIN" Text="Editar">                                                                                                                                                                       
                                                <Styles>                                                                                    
                                                    <Style Font-Bold="False" ForeColor="Black" Font-Size="11px">
                                                        <HoverStyle Font-Bold="False" ForeColor="Black">
                                                        </HoverStyle>
                                                    </Style>
                                                </Styles>
                                            </dx:GridViewCommandColumnCustomButton>
                                        </CustomButtons>                                        
                                        <HeaderStyle HorizontalAlign="Center"/>
                                        <CellStyle HorizontalAlign="Center" />
                                    </dx:GridViewCommandColumn>
                                    <dx:GridViewCommandColumn Caption="#" VisibleIndex="1" ButtonRenderMode="Link" Width="65px">                                        
                                        <CustomButtons>
                                            <dx:GridViewCommandColumnCustomButton ID="btnEliminarIN" Text="Eliminar">                                                                                                                                                                       
                                                <Styles>                                                                                    
                                                    <Style Font-Bold="False" ForeColor="Black" Font-Size="11px">
                                                        <HoverStyle Font-Bold="False" ForeColor="Black">
                                                        </HoverStyle>
                                                    </Style>
                                                </Styles>
                                            </dx:GridViewCommandColumnCustomButton>
                                        </CustomButtons>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <CellStyle HorizontalAlign="Center" />
                                    </dx:GridViewCommandColumn>--%>
                                    <dx:GridViewDataColumn Caption="Seleccione" VisibleIndex="1" Width="80px" >
                                      <HeaderStyle HorizontalAlign="Center" />
                                      <CellStyle HorizontalAlign="Center"></CellStyle>
                                      <DataItemTemplate>
                                          <dx:ASPxCheckBox ID="chkConsultarIN" ClientInstanceName="chkConsultarIN" Enabled="true" runat="server" OnInit="chkConsultarIN_Init">                                                                
                                          </dx:ASPxCheckBox>
                                      </DataItemTemplate>
                                    </dx:GridViewDataColumn>                                                                    
                                    <dx:GridViewDataColumn Caption="INCOTERM" FieldName="INCOTERM" VisibleIndex="2" Width="120px">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <CellStyle HorizontalAlign="Center" />
                                    </dx:GridViewDataColumn>
                                </Columns>
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
                                   
                        </div>
                    </div>                
                    <div class="modal-footer">
                        <div class="col-md-12 col-md-offset-0" style="text-align: center;">
                            <asp:LinkButton ID="LinkButton4" runat="server" CssClass="btn btn-primary btn-sm" Text="Guardar" OnClick="btnGuardarIN_Click">
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

    <button id="btnModalIN2" type="button" data-toggle="modal" data-target="#ModalIN2" data-whatever="Nuevo" onclick="return false;" style="display: none;"></button>
    <div class="modal fade" id="ModalIN2" tabindex="-1" role="dialog" aria-labelledby="ModalTitulo" data-backdrop="static">
        <div class="modal-dialog modal-sm" role="document" >
            <div class="modal-content">
                <div class="modal-header">
                    <h4 id="ModalTituloIN2" class="modal-title" runat="server"></h4>                    
                </div>
                <asp:Panel ID="Panel8" runat="server">
                    <div class="modal-body">
                        <div class="row">                            
                            <div id="divIN2" runat="server" class="form-group col-md-12">                                
                                &nbsp;<label id="lblT_IN2" runat="server" style="font-size:11px">INCOTERM</label>
                                <asp:TextBox ID="txtIN2_Incoterm" runat="server" CssClass="form-control input-sm" placeholder="INCOTERM" Width="100%" MaxLength="10"></asp:TextBox>
                            </div>
                                   
                        </div>
                    </div>                
                    <div class="modal-footer">
                        <div class="col-md-12 col-md-offset-0" style="text-align: center;">
                            <asp:LinkButton ID="btnGuardarIN2" runat="server" CssClass="btn btn-primary btn-sm" Text="Guardar" OnClick="btnGuardarIN2_Click">
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

    <button id="btnQuestion1" type="button" data-toggle="modal" data-target="#AlertQuestion1" style="display: none;"></button>
    <div class="modal fade" id="AlertQuestion1" tabindex="-1" role="dialog" style="top: 25%; outline: none;">
        <div class="modal-dialog modal-sm" role="document">
            <div class="alert alert-info text-center" style="-webkit-box-shadow: 0 5px 15px rgba(0, 0, 0, .5); box-shadow: 0 5px 15px rgba(0, 0, 0, .5);">
                <span class="glyphicon glyphicon-question-sign ico"></span>
                <br />
                <br />
                <p id="pModalQuestion1" runat="server" class="alert-title">
                </p>
                <hr />
                <asp:Button ID="btnOk1" runat="server" CssClass="btn btn-info" OnClick="btnOk1_Click" Text="Aceptar"></asp:Button>
                <button id="btnCancel1" runat="server" class="btn btn-info" data-dismiss="modal">Cancelar</button>
            </div>
        </div>
    </div>

    <button id="btnQuestion" type="button" data-toggle="modal" data-target="#AlertQuestion" style="display: none;"></button>
    <div class="modal fade" id="AlertQuestion" tabindex="-1" role="dialog" style="top: 25%; outline: none;">
        <div class="modal-dialog modal-sm" role="document">
            <div class="alert alert-info text-center" style="-webkit-box-shadow: 0 5px 15px rgba(0, 0, 0, .5); box-shadow: 0 5px 15px rgba(0, 0, 0, .5);">
                <span class="glyphicon glyphicon-question-sign ico"></span>
                <br />
                <br />
                <p id="pModalQuestion" runat="server" class="alert-title">
                </p>
                <hr />
                <asp:Button ID="btnOk" runat="server" CssClass="btn btn-info" OnClick="btnOk_Click" Text="Aceptar"></asp:Button>
                <button id="btnCancel" runat="server" class="btn btn-info" data-dismiss="modal">Cancelar</button>
            </div>
        </div>
    </div>

    <button id="btnQuestionIG" type="button" data-toggle="modal" data-target="#AlertQuestionIG" style="display: none;"></button>
    <div class="modal fade" id="AlertQuestionIG" tabindex="-1" role="dialog" style="top: 25%; outline: none;">
        <div class="modal-dialog modal-sm" role="document">
            <div class="alert alert-info text-center" style="-webkit-box-shadow: 0 5px 15px rgba(0, 0, 0, .5); box-shadow: 0 5px 15px rgba(0, 0, 0, .5);">
                <span class="glyphicon glyphicon-question-sign ico"></span>
                <br />
                <br />
                <p id="pModalQuestionIG" runat="server" class="alert-title">
                </p>
                <hr />
                <asp:Button ID="btnOkIG" runat="server" CssClass="btn btn-info" OnClick="btnOkIG_Click" Text="Aceptar"></asp:Button>
                <button id="btnCancelIG" runat="server" class="btn btn-info" data-dismiss="modal">Cancelar</button>
            </div>
        </div>
    </div>

    <button id="btnQuestionIP" type="button" data-toggle="modal" data-target="#AlertQuestionIP" style="display: none;"></button>
    <div class="modal fade" id="AlertQuestionIP" tabindex="-1" role="dialog" style="top: 25%; outline: none;">
        <div class="modal-dialog modal-sm" role="document">
            <div class="alert alert-info text-center" style="-webkit-box-shadow: 0 5px 15px rgba(0, 0, 0, .5); box-shadow: 0 5px 15px rgba(0, 0, 0, .5);">
                <span class="glyphicon glyphicon-question-sign ico"></span>
                <br />
                <br />
                <p id="pModalQuestionIP" runat="server" class="alert-title">
                </p>
                <hr />
                <asp:Button ID="btnOkIP" runat="server" CssClass="btn btn-info" OnClick="btnOkIP_Click" Text="Aceptar"></asp:Button>
                <button id="btnCancelIP" runat="server" class="btn btn-info" data-dismiss="modal">Cancelar</button>
            </div>
        </div>
    </div>

    <button id="btnQuestionIN" type="button" data-toggle="modal" data-target="#AlertQuestionIN" style="display: none;"></button>
    <div class="modal fade" id="AlertQuestionIN" tabindex="-1" role="dialog" style="top: 25%; outline: none;">
        <div class="modal-dialog modal-sm" role="document">
            <div class="alert alert-info text-center" style="-webkit-box-shadow: 0 5px 15px rgba(0, 0, 0, .5); box-shadow: 0 5px 15px rgba(0, 0, 0, .5);">
                <span class="glyphicon glyphicon-question-sign ico"></span>
                <br />
                <br />
                <p id="pModalQuestionIN" runat="server" class="alert-title">
                </p>
                <hr />
                <asp:Button ID="btnOkIN" runat="server" CssClass="btn btn-info" OnClick="btnOkIN_Click" Text="Aceptar"></asp:Button>
                <button id="btnCancelIN" runat="server" class="btn btn-info" data-dismiss="modal">Cancelar</button>
            </div>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptSection" runat="server">
</asp:Content>
