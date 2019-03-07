<%@ Page Language="C#" MasterPageFile="~/Presentacion/Principal.Master" AutoEventWireup="true" CodeBehind="ControlActivoFijo.aspx.cs" Inherits="SICE_BS_WEB.Presentacion.ControlActivoFijo" %>

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

        
        /* función que se usa en el Uploaded */
        function onFilesUploadStart() {
                        
        }

        /* función que se usa en el Uploaded */
        function OnUploadComplete() {
            document.getElementById('<%=btnAgregarDoc.ClientID%>').click();
            LoadingPanel2.Show();
            setTimeout(function () { LoadingPanel2.Hide(); }, 6000);
        }


        /*función para traer solo números */
        function onlyDotsAndNumbers(txt, event) {
            var charCode = (event.which) ? event.which : event.keyCode
            if (charCode == 46) {
                if (txt.value.indexOf(".") < 0)
                    return true;
                else
                    return false;
            }

            if (txt.value.indexOf(".") > 0) {
                var txtlen = txt.value.length;
                var dotpos = txt.value.indexOf(".");
                //Change the number here to allow more decimal points than 2
                if ((txtlen - dotpos) > 3)
                    return false;
            }

            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            else
            {
                charCode = this.optional(event) || /^-?(?:\d+|\d{1,3}(?:,\d{3})+)?(?:\,\d+)?$/.test(charCode);
            }

            return true;
        }

        
        /*función para agregar comas*/
        function addCommas(clientID) {

            var nStr = document.getElementById(clientID.id).value;

            nStr += '';
            x = nStr.split('.');
            if (!x[0]) {
                x[0] = "0";

            }
            x1 = x[0];
            if (!x[1]) {
                x[1] = "000";
            }
            x2 = x.length > 1 ? '.' + x[1] : '';
            var rgx = /(\d+)(\d{3})/;
            while (rgx.test(x1)) {
                x1 = x1.replace(rgx, '$1' + ',' + '$2');
            }

            document.getElementById(clientID.id).value = x1 + x2;
            return true;
        }

        /*función para quitar comas*/
        function removeCommas(clientID) {
            var nStr = document.getElementById(clientID.id).value;
            nStr = nStr.replace(/,/g, '');
            document.getElementById(clientID.id).value = nStr;

            $(clientID).select();

            return true;
        }


        function OnBatchStartEdit(s, e) {
            if (e.focusedColumn.fieldName == "PEDIMENTOS") {
                var editor = s.GetEditor(e.focusedColumn.fieldName);
                if (e.visibleIndex >= 0)
                    editor.SetEnabled(false);
                else
                    editor.SetEnabled(true);
            }
        }
             
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentSection" runat="server">
           
    <div class="container">
         <div class="panel panel-primary">
             <div class="panel-heading">
                 <h1 id="h1_titulo" runat="server" class="panel-title small"></h1>
             </div>
             <div class="panel-body"> 
                 <asp:Panel ID="Panel1" runat="server">
                    <div class="row" style="padding-left:10px; padding-top:10px; padding-right:10px;">
                        <div class="col-sm-4 col-md-3">
                            <div runat="server" id="DivPedimento">
                                <div class="form-group" style="position: relative; width: 100%; float: left;">
                                    <div class="input-group">
                                        <span class="input-group-addon" id="basic-addon1">
                                            <span class="glyphicon glyphicon-list-alt" aria-hidden="true"></span>
                                        </span>
                                        <asp:TextBox ID="PEDIMENTO" runat="server" CssClass="form-control input-sm" MaxLength="50" placeholder="Pedimento"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>     
                        <div class="col-sm-4 col-md-3">
                            <div runat="server" id="DivFolio">
                                <div class="form-group" style="position: relative; width: 100%; float: left;">
                                    <div class="input-group">
                                        <span class="input-group-addon" id="basic-addon2">
                                            <span class="glyphicon glyphicon-option-horizontal" aria-hidden="true"></span>
                                        </span>
                                        <asp:TextBox ID="FOLIO" runat="server" CssClass="form-control input-sm" MaxLength="50" placeholder="Folio"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div> 
                        <div class="col-sm-4 col-md-2">
                            <div runat="server" id="DivBuscar">
                                <div class="form-group" style="position: relative; width: 50%; float: left;">
                                    <div class="input-group">
                                        <dx:BootstrapButton ID="btnBuscar" runat="server" AutoPostBack="false" OnClick="btnBuscar_OnClick" 
                                            SettingsBootstrap-RenderOption="Primary" Text="Buscar" CssClasses-Text="txt-sm">
                                            <SettingsBootstrap RenderOption="Primary" Sizing="Small" />
                                            <ClientSideEvents Click="function(s, e) { Callback.PerformCallback(); LoadingPanel1.Show(); }" />
                                        </dx:BootstrapButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>  
                    <dx:ASPxGridView ID="Grid" runat="server" EnableTheming="True" Theme="SoftOrange" OnToolbarItemClick="Grid_ToolbarItemClick"
                        OnRowCommand="Grid_RowCommand" OnCustomCallback="Grid_CustomCallback" EnableCallBacks="false" ClientInstanceName="grid" OnCustomButtonCallback="Grid_CustomButtonCallback"
                        AutoGenerateColumns="False" Width="100%" Settings-HorizontalScrollBarMode="Auto" KeyFieldName="ACTIVOFIJOKEY" SettingsPager-Position="TopAndBottom"
                        Styles-Header-ForeColor="#FFF" Styles-Header-Font-Size="11px" Styles-Cell-Font-Size="11px" >
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
                            <dx:GridViewDataTextColumn FieldName="ACTIVOFIJOKEY" ReadOnly="True"  Visible="false" >
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="TIPO ACTIVO" FieldName="TIPOACTIVOFIJO" ReadOnly="True" VisibleIndex="1" Width="240px" >
                                <HeaderStyle HorizontalAlign="Center" ForeColor="#FFF"  />
                                <SettingsHeaderFilter Mode="CheckedList" />
                                <CellStyle HorizontalAlign="Left"></CellStyle>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewCommandColumn Caption="DOCUMENTOS" VisibleIndex="2" ButtonRenderMode="Image" Width="90px" ToolTip="Documentos">
                                <CustomButtons>
                                    <dx:GridViewCommandColumnCustomButton ID="btnDocs" Text="Imagenes" Image-ToolTip="Documentos"
                                        Styles-Style-HoverStyle-ForeColor="#000000" Styles-Style-ForeColor="#000000"
                                        Styles-Style-HoverStyle-Font-Bold="false" Styles-Style-Font-Bold="false">                                        
                                        <Image Url="../img/iconos/ico_exdi.png"></Image>                                                                                
                                        <Styles>                                                                                    
                                            <Style Font-Bold="False" ForeColor="Black" Font-Size="11px">
                                                <HoverStyle Font-Bold="False" ForeColor="Black">
                                                </HoverStyle>
                                            </Style>
                                        </Styles>
                                    </dx:GridViewCommandColumnCustomButton>
                                </CustomButtons>
                                <HeaderStyle HorizontalAlign="Center" ForeColor="#FFFFFF" />
                            </dx:GridViewCommandColumn>
                            <dx:GridViewCommandColumn Caption="PEDIMENTOS" VisibleIndex="3" ButtonRenderMode="Image" Width="90px" ToolTip="Pedimentos">
                                <CustomButtons>
                                    <dx:GridViewCommandColumnCustomButton ID="btnPed" Text="Pedimentos" Image-ToolTip="Pedimentos"
                                        Styles-Style-HoverStyle-ForeColor="#000000" Styles-Style-ForeColor="#000000"
                                        Styles-Style-HoverStyle-Font-Bold="false" Styles-Style-Font-Bold="false">                                        
                                        <Image Url="../img/iconos/ico_doc1.png"></Image>                                                                                
                                        <Styles>                                                                                    
                                            <Style Font-Bold="False" ForeColor="Black" Font-Size="11px">
                                                <HoverStyle Font-Bold="False" ForeColor="Black">
                                                </HoverStyle>
                                            </Style>
                                        </Styles>
                                    </dx:GridViewCommandColumnCustomButton>
                                </CustomButtons>
                                <HeaderStyle HorizontalAlign="Center" ForeColor="#FFFFFF" />
                            </dx:GridViewCommandColumn>
                            <dx:GridViewDataTextColumn Caption="FOLIO" FieldName="FOLIOINTERNO" ReadOnly="True" VisibleIndex="4" Width="150px" >
                                <HeaderStyle HorizontalAlign="Center" ForeColor="#FFF"  />
                                <SettingsHeaderFilter Mode="CheckedList" />
                                <CellStyle HorizontalAlign="Left"></CellStyle>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="MARCA" FieldName="MARCA" ReadOnly="True" VisibleIndex="5" Width="150px" >
                                <HeaderStyle HorizontalAlign="Center" ForeColor="#FFF"  />
                                <SettingsHeaderFilter Mode="CheckedList" />
                                <CellStyle HorizontalAlign="Left"></CellStyle>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="MODELO" FieldName="MODELO" ReadOnly="True" VisibleIndex="6" Width="140px" >
                                <HeaderStyle HorizontalAlign="Center" ForeColor="#FFF"  />
                                <SettingsHeaderFilter Mode="CheckedList" />
                                <CellStyle HorizontalAlign="Left"></CellStyle>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="SERIE" FieldName="SERIE" ReadOnly="True" VisibleIndex="7" Width="151px" >
                                <HeaderStyle HorizontalAlign="Center" ForeColor="#FFF"  />
                                <SettingsHeaderFilter Mode="CheckedList" />
                                <CellStyle HorizontalAlign="Left"></CellStyle>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="CLAVE PAÍS ORIGEN" FieldName="CLAVEPAISORIGEN" ReadOnly="True" VisibleIndex="7" Width="160px" >
                                <HeaderStyle HorizontalAlign="Center" ForeColor="#FFF"  />
                                <SettingsHeaderFilter Mode="CheckedList" />
                                <CellStyle HorizontalAlign="Left"></CellStyle>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="VALOR ADUANA" FieldName="VALORADUANA" ReadOnly="True" VisibleIndex="8" Width="145px">
                                <HeaderStyle HorizontalAlign="Center" />
                                <SettingsHeaderFilter Mode="CheckedList" />
                                <CellStyle HorizontalAlign="Right" />
                                <PropertiesTextEdit DisplayFormatString="{0:n3}" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="VALOR COMERCIAL" FieldName="VALORCOMERCIAL" ReadOnly="True" VisibleIndex="9" Width="150px">
                                <HeaderStyle HorizontalAlign="Center" />
                                <SettingsHeaderFilter Mode="CheckedList" />
                                <CellStyle HorizontalAlign="Right" />
                                <PropertiesTextEdit DisplayFormatString="{0:n3}" />
                            </dx:GridViewDataTextColumn>  
                            <dx:GridViewDataTextColumn FieldName="FRACCION" ReadOnly="True" Visible="false" >
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="DESCRIPCION" ReadOnly="True" Visible="false" >
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="DESCRIPCIONTECNICA" ReadOnly="True" Visible="false" >
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="PO" ReadOnly="True" Visible="false" >
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="VALORME" ReadOnly="True" Visible="false" >
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="ME" ReadOnly="True" Visible="false" >
                            </dx:GridViewDataTextColumn>                                           
                        </Columns>
                        <Toolbars>
                        <dx:GridViewToolbar Name="Toolbar1" EnableAdaptivity="true" ItemAlign="Left">
                            <Items>
                                <dx:GridViewToolbarItem Name="Links">
                                <Template>
                                    <asp:LinkButton ID="lkb_Nuevo" runat="server" OnClick="lkb_Nuevo_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button" >
                                        <span class="glyphicon glyphicon-plus"></span>&nbsp;&nbsp;Nuevo
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="lkb_Editar" runat="server" OnClick="lkb_Editar_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button" >
                                        <span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp;Editar
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="lkb_Eliminar" runat="server" OnClick="lkb_Eliminar_Click" OnClientClick="alertQuestion('¿Desea eliminar este activo fijo?', this); return false;" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button" >
                                        <span class="glyphicon glyphicon-remove"></span>&nbsp;&nbsp;Eliminar
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="lkb_Excel" runat="server" OnClick="lkb_Excel_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button" >
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
                 <dx:ASPxLabel ID="lblRFC" runat="server" Text="" Visible="false"></dx:ASPxLabel>
                 <dx:ASPxLabel ID="lblCadena" runat="server" Visible="false"/>                               
             </div>
         </div>
    </div>
   
    <button id="btnModal" type="button" data-toggle="modal" data-target="#Modal" data-whatever="Nuevo" onclick="return false;" style="display: none;"></button>
    <div class="modal fade" id="Modal" tabindex="-1" role="dialog" aria-labelledby="ModalTitulo" data-backdrop="static" >
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h6 id="ModalTitulo" class="modal-title" runat="server"></h6>                                        
                </div>
                <asp:Panel ID="Panel2" runat="server">
                    <div class="modal-body">
                        <div class="row" style="padding-left:10px">
                            <dx:ASPxPageControl runat="server" ID="ASPxPageControlEncabezado" Height="60px" Width="98%" EnableCallBacks="true"  ContentStyle-Border-BorderWidth="3px"
                            TabAlign="Justify" ActiveTabIndex="0" EnableTabScrolling="true" Theme="SoftOrange" Font-Size="12px" ContentStyle-VerticalAlign="Top">
                                <TabStyle Paddings-PaddingLeft="10px" Paddings-PaddingRight="10px"  />                                                                                      
                                <ContentStyle>
                                    <Paddings PaddingLeft="20px" PaddingRight="20px" PaddingTop="5px" />
                                </ContentStyle>
                                <TabPages>
                                    <dx:TabPage Text="Datos Generales">
                                        <ContentCollection>
                                            <dx:ContentControl ID="AF_DatosGenerales" runat="server" >
                                                <div class="row" style="margin-top:5px">
                                                    <div class="col-xs-12 col-sm-6">
                                                        <div class="col-xs-12">
                                                            <div class="form-group" style="position: relative; width: 100%; float: left;" title="" data-toggle="tooltip">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon" id="basic-addon3">
                                                                        <span class="glyphicon glyphicon-folder-open" aria-hidden="true"></span>
                                                                    </span>
                                                                    <asp:TextBox ID="TxtFolio" runat="server" CssClass="form-control input-sm" MaxLength="50" placeholder="* Folio" Font-Size="11px"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-xs-12">
                                                            <div class="form-group" style="position: relative; width: 100%; float: left;" title="" data-toggle="tooltip">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon" id="basic-addon4">
                                                                        <span class="glyphicon glyphicon-bookmark" aria-hidden="true"></span>
                                                                    </span>
                                                                    <asp:TextBox ID="TxtMarca" runat="server" CssClass="form-control input-sm" MaxLength="50" placeholder="Marca" Font-Size="11px"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-xs-12">
                                                            <div class="form-group" style="position: relative; width: 100%; float: left;" title="" data-toggle="tooltip">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon" id="basic-addon5">
                                                                        <span class="glyphicon glyphicon-menu-hamburger" aria-hidden="true"></span>
                                                                    </span>
                                                                    <asp:TextBox ID="TxtModelo" runat="server" CssClass="form-control input-sm" MaxLength="50" placeholder="Modelo" Font-Size="11px"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-xs-12">
                                                            <div class="form-group" style="position: relative; width: 100%; float: left;" title="" data-toggle="tooltip">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon" id="basic-addon6">
                                                                        <span class="glyphicon glyphicon-align-justify" aria-hidden="true"></span>
                                                                    </span>
                                                                    <asp:TextBox ID="TxtSerie" runat="server" CssClass="form-control input-sm" MaxLength="50" placeholder="Serie" Font-Size="11px"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-xs-12">
                                                            <div class="form-group" style="position: relative; width: 100%; float: left;" title="" data-toggle="tooltip">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon" id="basic-addon7">
                                                                        <span class="glyphicon glyphicon-list" aria-hidden="true"></span>
                                                                    </span>
                                                                    <asp:TextBox ID="TxtFraccion" runat="server" CssClass="form-control input-sm" MaxLength="50" placeholder="Fracción" Font-Size="11px"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                                                                                <div class="col-xs-12">
                                                            <div class="form-group" style="position: relative; width: 100%; float: left;" title="" data-toggle="tooltip">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon" id="basic-addon8">
                                                                        <span class="glyphicon glyphicon-list-alt" aria-hidden="true"></span>
                                                                    </span>
                                                                    <asp:TextBox ID="TxtDesc" runat="server" CssClass="form-control input-sm" placeholder="Descripción"  Font-Size="11px"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-xs-12">
                                                            <div class="form-group" style="position: relative; width: 100%; float: left;" title="" data-toggle="tooltip">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon" id="basic-addon9">
                                                                        <span class="glyphicon glyphicon-globe" aria-hidden="true"></span>
                                                                    </span>
                                                                    <asp:TextBox ID="txtPaisOrigen" runat="server" CssClass="form-control input-sm" placeholder="Clave País Origen" Font-Size="11px" MaxLength="5"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>

                                                    </div>
                                                    <div class="col-xs-12 col-sm-6">
                                                        <div class="col-xs-12">
                                                            <div class="form-group" style="position: relative; width: 100%; float: left;" title="" data-toggle="tooltip">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon" id="basic-addon10">
                                                                        <span class="glyphicon glyphicon-briefcase" aria-hidden="true"></span>
                                                                    </span>
                                                                    <dx:ASPxComboBox ID="CmbTipoActivo" Caption="" runat="server" TextField="TipoActivoFijo" ValueField="TipoActivoKey" 
                                                                        NullText="* Tipo Activo" DataSecurityMode="Default" Width="100%"  Font-Size="11px" DropDownHeight="110px"
                                                                        Font-Names="Arial,Helvetica" CssClass="bordes_curvos_derecha" Theme="MaterialCompact" ForeColor="#6B5555"  >
                                                                        <ClearButton DisplayMode="Never"  />
                                                                    </dx:ASPxComboBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-xs-12">
                                                            <div class="form-group" style="position: relative; width: 100%; float: left;" title="" data-toggle="tooltip">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon" id="basic-addon11">
                                                                        <span class="glyphicon glyphicon-list" aria-hidden="true"></span>
                                                                    </span>
                                                                    <asp:TextBox runat="server" ID="TxtValorAduana" CssClass="form-control input-sm" MaxLength="50" 
                                                                        placeholder="Valor Aduana" Font-Size="11px" onkeypress="return onlyDotsAndNumbers(this,event);"
                                                                        onfocus="removeCommas(this)" OnBlur="addCommas(this)" />                                                                    
                                                                </div>
                                                            </div>                                                            
                                                        </div>
                                                        <div class="col-xs-12">
                                                            <div class="form-group" style="position: relative; width: 100%; float: left;" title="" data-toggle="tooltip">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon" id="basic-addon14">
                                                                        <span class="glyphicon glyphicon-list" aria-hidden="true"></span>
                                                                    </span>
                                                                    <div class="desktopCell">
                                                                        <asp:TextBox runat="server" ID="TxtValorComercial" CssClass="form-control input-sm" MaxLength="50" 
                                                                        placeholder="Valor Comercial" Font-Size="11px" onkeypress="return onlyDotsAndNumbers(this,event);"
                                                                        onfocus="removeCommas(this)" OnBlur="addCommas(this)" />                                                                        
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                          <div class="col-xs-12">
                                                            <div class="form-group" style="position: relative; width: 100%; float: left;" title="" data-toggle="tooltip">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon" id="basic-addon15">
                                                                        <span class="glyphicon glyphicon-list-alt" aria-hidden="true"></span>
                                                                    </span>
                                                                    <asp:TextBox ID="TxtDescripcionTecnica" runat="server" CssClass="form-control input-sm" placeholder="Descripción Técnica"  Font-Size="11px"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-xs-12">
                                                            <div class="form-group" style="position: relative; width: 100%; float: left;" title="" data-toggle="tooltip">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon" id="basic-addon16">
                                                                        <span class="glyphicon glyphicon-list-alt" aria-hidden="true"></span>
                                                                    </span>
                                                                    <asp:TextBox ID="TxtPO" runat="server" CssClass="form-control input-sm" placeholder="PO"  Font-Size="11px"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    <div class="col-xs-12">
                                                            <div class="form-group" style="position: relative; width: 100%; float: left;" title="" data-toggle="tooltip">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon" id="basic-addon17">
                                                                        <span class="glyphicon glyphicon-list" aria-hidden="true"></span>
                                                                    </span>
                                                                    <asp:TextBox runat="server" ID="TxtValorME" CssClass="form-control input-sm" MaxLength="50" 
                                                                        placeholder="Valor Moneda Extranjera" Font-Size="11px" onkeypress="return onlyDotsAndNumbers(this,event);"
                                                                        onfocus="removeCommas(this)" OnBlur="addCommas(this)" />                                                                    
                                                                </div>
                                                            </div>                                                            
                                                        </div>
                                                    <div class="col-xs-12">
                                                            <div class="form-group" style="position: relative; width: 100%; float: left;" title="" data-toggle="tooltip">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon" id="basic-addon18">
                                                                        <span class="glyphicon glyphicon-list" aria-hidden="true"></span>
                                                                    </span>
                                                                    <asp:TextBox ID="TxtME" runat="server" CssClass="form-control input-sm" placeholder="Moneda Extrenjera"  Font-Size="11px"></asp:TextBox>                                                                  
                                                                </div>
                                                            </div>                                                            
                                                        </div>
                                                    </div>
                                                
                                                    </div>
                                            </dx:ContentControl>
                                        </ContentCollection>
                                    </dx:TabPage>
                                </TabPages>
                            </dx:ASPxPageControl>
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

    <button id="btnModalDocumento" type="button" data-toggle="modal" data-target="#ModalDocumento" data-whatever="Nuevo" onclick="return false;" style="display: none;"></button>
    <div class="modal fade" id="ModalDocumento" tabindex="-1" role="dialog" aria-labelledby="ModalTituloDocumento" data-backdrop="static" >
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <table style="width:100%">
                        <tr>
                            <td style="text-align:left; width:50%">
                                <h6 id="ModalTituloDocumento" class="modal-title" runat="server"></h6>
                            </td>
                            <td style="text-align:right; width:50%">
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                            </td>
                        </tr>
                    </table>                                       
                </div>
                <asp:Panel ID="Panel3" runat="server">
                    <div class="modal-body">
                        <div class="row" style="padding-left:10px">
                            <dx:ASPxPageControl runat="server" ID="ASPxPageControlDocumento" Height="300px" Width="98%" EnableCallBacks="true" ContentStyle-Border-BorderWidth="3px"
                            TabAlign="Justify" ActiveTabIndex="0" EnableTabScrolling="true" Theme="SoftOrange" Font-Size="12px" ContentStyle-VerticalAlign="Top">
                                <TabStyle Paddings-PaddingLeft="10px" Paddings-PaddingRight="10px"  />                                                                                      
                                <ContentStyle>
                                    <Paddings PaddingLeft="20px" PaddingRight="20px" PaddingTop="5px" />
                                </ContentStyle>
                                <TabPages>
                                    <dx:TabPage Text="DOCUMENTOS">
                                        <ContentCollection>
                                            <dx:ContentControl ID="ContentControl1" runat="server" >
                                                <div class="row" style="margin-top:5px">
                                                    <div class="col-xs-12 col-sm-5">
                                                        <div class="form-group" style="position: relative; width: 100%; float: left;" title="" data-toggle="tooltip">
                                                            <div class="input-group">
                                                                <span class="input-group-addon" id="basic-addon12">
                                                                    <span class="glyphicon glyphicon-folder-open" aria-hidden="true"></span>
                                                                </span>
                                                                <dx:ASPxComboBox ID="cmbTipoDocumento" Caption="" runat="server" TextField="TIPOIMAGEN" ValueField="TIPOIMAGEKEY" 
                                                                    NullText="* Tipo Documento" DataSecurityMode="Default" Width="100%"  Font-Size="12px" DropDownHeight="120px"
                                                                    Font-Names="Arial,Helvetica" CssClass="bordes_curvos_derecha" Theme="MaterialCompact" ForeColor="#6B5555"  >
                                                                    <ClearButton DisplayMode="Never"  />
                                                                </dx:ASPxComboBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-xs-12 col-sm-5" style="margin-bottom:5px">
                                                        <dx:ASPxUploadControl ID="upc_Imagen" runat="server" UploadMode="Auto" Width="100%" ShowProgressPanel="true" ShowUploadButton="true" FileUploadMode="OnPageLoad" 
                                                            OnFileUploadComplete="upc_Imagen_FileUploadComplete" NullText="Arrastre un archivo o de clic aquí" AutoStartUpload="true" ShowClearFileSelectionButton="true" >
                                                            <ClientSideEvents FileUploadComplete="function(s, e) { OnUploadComplete(); }" FilesUploadStart="onFilesUploadStart" />
                                                            <AdvancedModeSettings EnableMultiSelect="false" EnableFileList="false" EnableDragAndDrop="true"></AdvancedModeSettings>                                    
                                                            <BrowseButton Text="Archivo(s)"  /> 
                                                        </dx:ASPxUploadControl>
                                                        <%--<table style="width:100%">
                                                            <tr>
                                                                <td style="text-align:left;">
                                                                    <table>
                                                                        <tr>
                                                                            <td style="text-align:left;">
                                                                                <dx:ASPxLabel ID="lblTitRespuestaFileUpload" runat="server" Font-Size="11px" ClientInstanceName="lblFiles"/>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    <table>
                                                                        <tr>
                                                                            <td style="text-align:left;">
                                                                                <asp:LinkButton ID="btnQuitar" runat="server"  OnClientClick="QuitarFile(); return false" Font-Size="11px" 
                                                                                    ForeColor="Black" CssClass="no-display" ClientIDMode="AutoID">quitar
                                                                                </asp:LinkButton>
                                                                                <asp:Button ID="btnQuitarFile" runat="server" OnClick="btnQuitarFile_Click" CssClass="no-display" ClientIDMode="AutoID" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                                
                                                            </tr>
                                                        </table>--%>
                                                    </div>
                                                    <div class="col-xs-12 col-sm-2">
                                                        <asp:LinkButton ID="btnAgregarDoc" CommandName="btnAgregarDoc" runat="server" CssClass="no-display" OnClick="btnAgregarDoc_Click">
                                                        </asp:LinkButton>
                                                        <asp:LinkButton ID="lkb_EliminarDocs" runat="server" OnClick="lkb_EliminarDocs_Click" OnClientClick="alertQuestion('¿Desea eliminar este archivo?', this); return false;" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button" >
                                                            <span class="glyphicon glyphicon-remove"></span>&nbsp;&nbsp;Eliminar
                                                        </asp:LinkButton>
                                                    </div>
                                                </div>
                                                <div class="row" style="margin-top:5px;">
                                                    <dx:ASPxGridView ID="GridDocs" runat="server" EnableTheming="True" Theme="SoftOrange" EnableCallBacks="false" OnCustomButtonCallback="GridDocs_CustomButtonCallback"
                                                        AutoGenerateColumns="False" Width="100%" Settings-HorizontalScrollBarMode="Auto" KeyFieldName="IMAGENESKEY" SettingsPager-Position="TopAndBottom"
                                                        Styles-Header-ForeColor="#FFF" Styles-Header-Font-Size="11px" Styles-Cell-Font-Size="11px" >                                                                                                                
                                                        <SettingsAdaptivity AdaptivityMode="HideDataCellsWindowLimit" HideDataCellsAtWindowInnerWidth="800" 
                                                            AdaptiveDetailColumnCount="1" AllowOnlyOneAdaptiveDetailExpanded="True">
                                                            <AdaptiveDetailLayoutProperties colcount="1">
                                                                <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit"  SwitchToSingleColumnAtWindowInnerWidth="600" />
                                                            </AdaptiveDetailLayoutProperties>
                                                        </SettingsAdaptivity> 
                                                        <SettingsResizing ColumnResizeMode="Control" />
                                                        <SettingsBehavior AllowSelectByRowClick="true" AllowSelectSingleRowOnly="true" />
                                                        <Settings ShowFooter="False" ShowHeaderFilterButton="false" ShowFilterRowMenu="false"  />                                          
                                                        <Styles>                                                                                      
                                                            <SelectedRow BackColor="#73C000" ForeColor="#FFFFFF" />
                                                            <Row  />
                                                            <AlternatingRow Enabled="True" />
                                                            <PagerTopPanel Paddings-PaddingBottom="3px"></PagerTopPanel>
                                                        </Styles>                                                                                                    
                                                        <Columns>
                                                            <dx:GridViewDataTextColumn FieldName="IMAGENESKEY" ReadOnly="True" Visible="false" >
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="ACTIVOFIJOKEY" ReadOnly="True" Visible="false" >
                                                            </dx:GridViewDataTextColumn>
                                                            <%--<dx:GridViewCommandColumn Caption="Borrar" VisibleIndex="1" Width="60px">
                                                                <CustomButtons>                                                                            
                                                                    <dx:GridViewCommandColumnCustomButton ID="btnBorrarDocs" Text="Borrar" 
                                                                        Styles-Style-HoverStyle-ForeColor="#000000" Styles-Style-ForeColor="#000000"
                                                                        Styles-Style-HoverStyle-Font-Bold="false" Styles-Style-Font-Bold="false">                                     
                                                                        <Styles>
                                                                            <Style Font-Bold="False" ForeColor="Black" Font-Size="11px">
                                                                                <HoverStyle Font-Bold="False" ForeColor="Black">
                                                                                </HoverStyle>
                                                                            </Style>
                                                                        </Styles>
                                                                    </dx:GridViewCommandColumnCustomButton>
                                                                </CustomButtons>
                                                                <HeaderStyle HorizontalAlign="Center" ForeColor="#FFFFFF" />
                                                            </dx:GridViewCommandColumn>--%>
                                                            <dx:GridViewDataTextColumn Caption="TIPO IMAGEN" FieldName="TIPOIMAGEN" ReadOnly="True" VisibleIndex="2" Width="306px" >
                                                                <HeaderStyle HorizontalAlign="Center" ForeColor="#FFF"  />
                                                                <SettingsHeaderFilter Mode="CheckedList" />
                                                                <CellStyle HorizontalAlign="Left"></CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewCommandColumn Caption="ARCHIVO" VisibleIndex="3" ButtonRenderMode="Image" Width="70px">
                                                                <CustomButtons>
                                                                    <dx:GridViewCommandColumnCustomButton ID="btnOpenImage" Text="Imagenes" Image-ToolTip="Imagen"
                                                                        Styles-Style-HoverStyle-ForeColor="#000000" Styles-Style-ForeColor="#000000"
                                                                        Styles-Style-HoverStyle-Font-Bold="false" Styles-Style-Font-Bold="false">                                        
                                                                        <Image Url="../img/iconos/ico_exdi.png"></Image>                                                                                
                                                                        <Styles>                                                                                    
                                                                            <Style Font-Bold="False" ForeColor="Black" Font-Size="11px">
                                                                                <HoverStyle Font-Bold="False" ForeColor="Black">
                                                                                </HoverStyle>
                                                                            </Style>
                                                                        </Styles>
                                                                    </dx:GridViewCommandColumnCustomButton>
                                                                </CustomButtons>
                                                                <HeaderStyle HorizontalAlign="Center" ForeColor="#FFFFFF" />
                                                            </dx:GridViewCommandColumn>                                                           
                                                            <dx:GridViewDataTextColumn Caption="NOMBRE ARCHIVO" FieldName="NOMBREARCHIVO" ReadOnly="True" VisibleIndex="4" Width="450px" >
                                                                <HeaderStyle HorizontalAlign="Center" ForeColor="#FFF"  />
                                                                <SettingsHeaderFilter Mode="CheckedList" />
                                                                <CellStyle HorizontalAlign="Left"></CellStyle>
                                                            </dx:GridViewDataTextColumn>                                                                                                 
                                                        </Columns>                                                                                                                              
                                                    </dx:ASPxGridView>
                                                </div>
                                            </dx:ContentControl>
                                        </ContentCollection>
                                    </dx:TabPage>
                                </TabPages>
                            </dx:ASPxPageControl>
                            <div class="col-md-12">                                
                                <div class="form-group text-right" style="position: relative; width: 100%; float: left;">
                                    <asp:Label Text="* Campo obligatorio" Font-Italic="true" runat="server" CssClass="asp-label" ForeColor="Red" />
                                </div>
                            </div>    
                        </div>
                        <dx:ASPxLoadingPanel ID="ASPxLoadingPanel2" runat="server" ClientInstanceName="LoadingPanel2"
                            Modal="True" ViewStateMode="Enabled" Theme="MaterialCompact" >
                        </dx:ASPxLoadingPanel>
                    </div>                
                    <div class="modal-footer">
                        <div class="col-md-12 col-md-offset-0" style="text-align: center;">
                            <button type="button" class="btn btn-primary btn-sm" data-dismiss="modal">
                                <span class="glyphicon glyphicon-remove"></span>&nbsp;&nbsp;Cancelar
                            </button>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div>

    <button id="btnModalPedimento" type="button" data-toggle="modal" data-target="#ModalPedimento" data-whatever="Nuevo" onclick="return false;" style="display: none;"></button>
    <div class="modal fade" id="ModalPedimento" tabindex="-1" role="dialog" aria-labelledby="ModalTituloPedimento" data-backdrop="static" >
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">                    
                    <table style="width:100%">
                        <tr>
                            <td style="text-align:left; width:50%">
                                <h6 id="ModalTituloPedimento" class="modal-title" runat="server"></h6>
                            </td>
                            <td style="text-align:right; width:50%">
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                            </td>
                        </tr>
                    </table>                                       
                </div>
                <asp:Panel ID="Panel4" runat="server">
                    <div class="modal-body">
                        <div class="row" style="padding-left:10px">
                            <dx:ASPxPageControl runat="server" ID="ASPxPageControlPedimento" Height="300px" Width="98%" EnableCallBacks="true"  ContentStyle-Border-BorderWidth="3px"
                            TabAlign="Justify" ActiveTabIndex="0" EnableTabScrolling="true" Theme="SoftOrange" Font-Size="12px" ContentStyle-VerticalAlign="Top">
                                <TabStyle Paddings-PaddingLeft="10px" Paddings-PaddingRight="10px"  />                                                                                      
                                <ContentStyle>
                                    <Paddings PaddingLeft="20px" PaddingRight="20px" PaddingTop="5px" />
                                </ContentStyle>
                                <TabPages>
                                    <dx:TabPage Text="PEDIMENTOS">
                                        <ContentCollection>
                                            <dx:ContentControl ID="ContentControl2" runat="server" >
                                                <div class="row" style="margin-top:5px">
                                                    <div class="col-xs-12 col-sm-5">
                                                        <div class="form-group" style="position: relative; width: 100%; float: left;" title="" data-toggle="tooltip">
                                                            <div class="input-group">
                                                                <span class="input-group-addon" id="basic-addon13">
                                                                    <span class="glyphicon glyphicon-folder-open" aria-hidden="true"></span>
                                                                </span>
                                                                <%--<dx:ASPxComboBox ID="cmbPedimento" Caption="" runat="server" TextField="PEDIMENTOARMADO" ValueField="PEDIMENTOARMADO" 
                                                                    NullText="* Pedimento" DataSecurityMode="Default" Width="100%"  Font-Size="12px" DropDownHeight="110px"
                                                                    Font-Names="Arial,Helvetica" CssClass="bordes_curvos_derecha" Theme="MaterialCompact" ForeColor="#6B5555">
                                                                    <ClearButton DisplayMode="Never"  />
                                                                </dx:ASPxComboBox>--%>
                                                                <dx:ASPxComboBox ID="cmbPedimento" Caption="" runat="server" TextField="PEDIMENTOARMADO" ValueType="System.String" ValueField="PEDIMENTOARMADO" 
                                                                    NullText="* Pedimento" DataSecurityMode="Default" Width="100%"  Font-Size="12px" DropDownHeight="236px" DropDownWidth="250px"
                                                                    Font-Names="Arial,Helvetica" CssClass="bordes_curvos_derecha" Theme="MaterialCompact" ForeColor="#6B5555"
                                                                    FilterMinLength="0" DropDownStyle="DropDown" EnableCallbackMode="true" CallbackPageSize="10" 
                                                                    OnItemsRequestedByFilterCondition="ASPxComboBox_OnItemsRequestedByFilterCondition_SQL" EnableViewState="false"
                                                                    OnItemRequestedByValue="ASPxComboBox_OnItemRequestedByValue_SQL" TextFormatString="{0}" >
                                                                    <ClearButton DisplayMode="Never"  />
                                                                </dx:ASPxComboBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-xs-12 col-sm-3">
                                                        <div runat="server" id="DivPartida">
                                                            <div class="form-group" style="position: relative; width: 100%; float: left;">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon" id="basic-addon30">
                                                                        <span class="glyphicon glyphicon-list-alt" aria-hidden="true"></span>
                                                                    </span>
                                                                    <asp:TextBox ID="txtPartida" runat="server" CssClass="form-control input-sm" MaxLength="15" placeholder="Partida"
                                                                        onkeypress="return onlyDotsAndNumbers(this,event);"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-xs-12 col-sm-2" style="margin-bottom:10px">
                                                        <asp:LinkButton ID="btnAgregarPed" runat="server" CssClass="btn btn-primary btn-sm" OnClick="btnAgregarPed_Click">
                                                            <span class="glyphicon glyphicon-ok"></span>&nbsp;&nbsp;Agregar
                                                        </asp:LinkButton>                                                       
                                                    </div> 
                                                    <div class="col-xs-12 col-sm-2">
                                                        <asp:LinkButton ID="lkb_EliminarPed" runat="server" OnClick="lkb_EliminarPed_Click" OnClientClick="alertQuestion('¿Desea eliminar este pedimento?', this); return false;" CssClass="btn btn-primary btn-sm" >
                                                            <span class="glyphicon glyphicon-remove"></span>&nbsp;&nbsp;Eliminar
                                                        </asp:LinkButton>
                                                    </div>
                                                     
                                                </div>
                                                <div class="row" style="margin-top:5px;">
                                                    <dx:ASPxGridView ID="GridPed" runat="server" EnableTheming="True" Theme="SoftOrange" OnCustomButtonCallback="GridDocs_CustomButtonCallback"
                                                        Width="100%" Settings-HorizontalScrollBarMode="Auto" KeyFieldName="PEDIMENTO" SettingsPager-Position="TopAndBottom"
                                                        Styles-Header-ForeColor="#FFF" Styles-Header-Font-Size="11px" Styles-Cell-Font-Size="11px" >                                                        
                                                        <SettingsBehavior AllowSelectByRowClick="true" AllowSelectSingleRowOnly="true" />
                                                        <Settings ShowFooter="False" ShowHeaderFilterButton="false" ShowFilterRowMenu="false"  />                                          
                                                        <ClientSideEvents BatchEditStartEditing="OnBatchStartEdit" />
                                                        <Styles>                                                                                      
                                                            <SelectedRow BackColor="#73C000" ForeColor="#FFFFFF" />
                                                            <Row  />
                                                            <AlternatingRow Enabled="True" />
                                                            <PagerTopPanel Paddings-PaddingBottom="3px"></PagerTopPanel>
                                                        </Styles>                                                                                                    
                                                        <Columns>
                                                            <dx:GridViewDataTextColumn FieldName="RELACIONPEDIMENTOKEY" ReadOnly="True" Visible="false" >
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="ACTIVOFIJOKEY" ReadOnly="True" Visible="false" >
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="PEDIMENTOS" FieldName="PEDIMENTO" ReadOnly="True" VisibleIndex="1" Width="50%">
                                                                <HeaderStyle HorizontalAlign="Left" ForeColor="#FFF"  />
                                                                <SettingsHeaderFilter Mode="CheckedList" />
                                                                <CellStyle HorizontalAlign="Left"></CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="PARTIDA" VisibleIndex="2"  Width="50%">
                                                                <HeaderStyle HorizontalAlign="Left" ForeColor="#FFF"  />
                                                                <SettingsHeaderFilter Mode="CheckedList" />
                                                                <CellStyle HorizontalAlign="Left"></CellStyle>
                                                            </dx:GridViewDataTextColumn>                                                                                               
                                                        </Columns>
                                                        <Toolbars>
                                                        <dx:GridViewToolbar Name="Toolbar1" EnableAdaptivity="true" ItemAlign="Left">
                                                            <Items>
                                                                <dx:GridViewToolbarItem Name="Links">
                                                                <Template>                                                                    
                                                                    <asp:LinkButton ID="lkb_EditarPed" runat="server" OnClick="lkb_EditarPed_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button" >
                                                                        <span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp;Editar
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
                                                                   <td style="width: 100%"></td>
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
                                            </dx:ContentControl>
                                        </ContentCollection>
                                    </dx:TabPage>
                                </TabPages>
                            </dx:ASPxPageControl>
                            <div class="col-md-12">                                
                                <div class="form-group text-right" style="position: relative; width: 100%; float: left;">
                                    <asp:Label Text="* Campo obligatorio" Font-Italic="true" runat="server" CssClass="asp-label" ForeColor="Red" />
                                </div>
                            </div>    
                        </div>
                    </div>                
                    <div class="modal-footer">
                        <div class="col-md-12 col-md-offset-0" style="text-align: center;">
                            <button type="button" class="btn btn-primary btn-sm" data-dismiss="modal">
                                <span class="glyphicon glyphicon-remove"></span>&nbsp;&nbsp;Cancelar
                            </button>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div>

    <button id="btnModalPartida" type="button" data-toggle="modal" data-target="#ModalPartida" data-whatever="Nuevo" onclick="return false;" style="display: none;"></button>
    <div class="modal fade" id="ModalPartida" tabindex="-1" role="dialog" aria-labelledby="ModalTituloPartida" data-backdrop="static" >
        <div class="modal-dialog modal-sm" role="document">
            <div class="modal-content">
                <div class="modal-header">                    
                    <table style="width:100%">
                        <tr>
                            <td style="text-align:left; width:50%">
                                <h6 id="ModalTituloPartida" class="modal-title" runat="server"></h6>
                            </td>
                            <td style="text-align:right; width:50%">
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                            </td>
                        </tr>
                    </table>                                       
                </div>
                <asp:Panel ID="Panel5" runat="server">
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-md-12">
                                <div runat="server" id="Div2">
                                    <div class="form-group" style="position: relative; width: 100%; float: left;">
                                        <div class="input-group">
                                            <span class="input-group-addon" id="basic-addon31">
                                                <span class="glyphicon glyphicon-list-alt" aria-hidden="true"></span>
                                            </span>
                                            <asp:TextBox ID="txt_PartidaEditar" runat="server" CssClass="form-control input-sm" MaxLength="15" placeholder="Partida"
                                                onkeypress="return onlyDotsAndNumbers(this,event);"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>                                                             
                                <div class="form-group text-right" style="position: relative; width: 100%; float: left;">
                                    <asp:Label Text="* Campo obligatorio" Font-Italic="true" runat="server" CssClass="asp-label" ForeColor="Red" />
                                </div>
                            </div>    
                        </div>
                    </div>                
                    <div class="modal-footer">
                        <div class="col-md-12 col-md-offset-0" style="text-align: center;">
                            <asp:LinkButton ID="LinkButton1" runat="server" CssClass="btn btn-primary btn-sm" Text="Guardar" OnClick="btnGuardarPed_Click">
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
    <div class="modal fade" id="AlertQuestion" tabindex="-1" role="dialog" style="top: 25%; outline: none;">
        <div class="modal-dialog modal-sm" role="document">
            <div class="alert alert-info text-center" style="-webkit-box-shadow: 0 5px 15px rgba(0, 0, 0, .5); box-shadow: 0 5px 15px rgba(0, 0, 0, .5);">
                <span class="glyphicon glyphicon-question-sign ico"></span>
                <br />
                <br />
                <p class="alert-title">
                </p>
                <hr />
                <a id="btnOk" class="btn btn-info" data-dismiss="modal">Aceptar</a>
                <a id="btnCancel" class="btn btn-info" data-dismiss="modal">Cancelar</a>
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
