<%@ Page Language="C#" MasterPageFile="~/Presentacion/Principal.Master" AutoEventWireup="true" CodeBehind="Documentos.aspx.cs" Inherits="SICE_BS_WEB.Presentacion.Documentos"  %>

<%@ Register TagPrefix="dx" Namespace="DevExpress.Web" Assembly="DevExpress.Web.v17.1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">    
    
    <style>
        
        @media only screen and (max-width: 766px) {
            .collapsing, .in {background-color: #f7f7f7; color: #555!important;}
            .collapsing ul li a, .in ul li a {color: #555!important;}
            .collapsing ul li a:hover, .in ul li a:hover {color: #000!important; background-color: #E7E7E7!important;}


            /*Se usa para poder ver el limite inferior del dx:ASPxSplitter */
            /*.padding-bottom-sm {
                padding-bottom: 30%;
            }*/          
        }
                             
        .fondo_verde_letra_blanca{
            color:#FFF !important;            
            background-color: #73C000;
            border: 1px solid #E7E7E7;
            border-bottom: 1px solid #E7E7E7;
            border-top: 1px solid #E7E7E7;
        } 

        .combobox-width{
            width:40px;
        }

        .column_hidden_ed {
            display: none;
        }

        .column_hidden_ped {
            display: none;
        }


        /*******************************************************/
        /*Clases para cada columna del Grid*/
        .column_width_cg {
            width: 34px;
        }
        .column_width_np {
            width: 34px;
        }
        .column_width_np {
            width: 34px;
        }
        .column_width_ed {
            width: 34px;
        }
        .column_width_pa {
            width: 110px;
        }
        .column_width_encabezado {
            width: 100px;
        }
        .column_width_partida {
            width: 75px;
        }
        .column_width_rfc {
            width: 110px;
        }
        .column_width_pedimento {
            width: 160px;
        }
        .column_width_fecha {
            width: 120px;
        }
        .column_width_tipo {
            width: 130px;
        }
        .column_width_tipo_pedimento {
            width: 130px;
        }
        .column_width_clave_pedimento {
            width: 120px;
        }
        .column_width_clave_aduana {
            width: 100px;
        }
        .column_width_peso_bruto {
            width: 100px;
        }
        .column_width_tipo_cambio {
            width: 110px;
        }
        .column_width_valor_dolares {
            width: 110px;
        }
        .column_width_valor_aduanal {
            width: 110px;
        }
        .column_width_valor_comercial {
            width: 115px;
        }
        .column_width_fecha_pago_original {
            width: 125px;
        }
        .column_width_fecha_entrada_presentacion {
            width: 145px;
        }

        /*******************************************************/

    </style>
    <script type="text/javascript">
        function pageLoad(sender, args) {
            var pane1 = splitter.GetPaneByName('SplitterPane1');
            if (document.documentElement.clientWidth > 766) {
                pane1.collapsed = false;
            }
            else
                pane1.collapsed = true;

        }

        //Fija el modalED cuando se llame
        $(document).ready(function () {
            $('#modalED').modal({
                backdrop: 'static',
                keyboard: false,
                show: false
            });
        });

        function Closepopup() {
            $('#ModalED').modal('close');

        }

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

        var isDetailRowExpanded = new Array();
        function OnRowClick(s, e) {
            if (isDetailRowExpanded[e.visibleIndex] != true)
                s.ExpandDetailRow(e.visibleIndex);
            else
                s.CollapseDetailRow(e.visibleIndex);
        }
        function OnDetailRowExpanding(s, e) {
            isDetailRowExpanded[e.visibleIndex] = true;
        }
        function OnDetailRowCollapsing(s, e) {
            isDetailRowExpanded[e.visibleIndex] = false;
        }

        function gridIn_Init(s, e) {
            gridIn.Refresh();
        }

        /*función que abre botón btnPartidasCerrar_Click para ir al codebehind
          abre el loadingPanel, espera un tiempo y lo cierra*/
        function BtnCerrarPartidas() {
            document.getElementById('ContentSection_btnPartidasCerrar').click();
            loadingPanel.Show();
            setTimeout(function () { loadingPanel.Hide(); }, 3000);
        }

        //Se usa para palomear el checkbox seleccionado y despalomear los demás
        function chkConsultarClick(s, e, index) {
            var btnPdf = document.getElementById('ContentSection_btnPDF')

            if (!btnPdf.classList.contains('disabled'))
                btnPdf.classList.add('disabled');

            //document.getElementById('check');
            var cheked = s.GetChecked();
            var row = detailGridDocs.GetDataRow(index);


            var chkConsultar = eval("chkConsultar" + index);
            var valor = chkConsultar.GetChecked();
            chkConsultar.SetChecked(valor);

            for (var i = 0; i < detailGridDocs.GetVisibleRowsOnPage() ; i++) {
                if (i == index) {
                    chkConsultar = eval("chkConsultar" + i);
                    chkConsultar.SetChecked(valor);

                    if (valor == true)
                        btnPdf.classList.remove('disabled');
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

        /*función que abre evento DescargarZip en el codebehind para descargar archivo zipeado*/
        function lblDescargarZip_Click() {
            document.getElementById('ContentSection_DescargarZip').click();
            loadingPanel.Show();
            setTimeout(function () { loadingPanel.Hide(); }, 3000);
        }


        /* función que limpia los primeros filtros  */
        function btnLimpiar_Click() {
            document.getElementById('ContentSection_ASPxSplitter1_TxtPedimento').value = "";
            document.getElementById('ContentSection_ASPxSplitter1_cmbAduana').value = "";
            document.getElementById('ContentSection_ASPxSplitter1_cmbClave').value = "";
            document.getElementById('ContentSection_ASPxSplitter1_TxtTexto').value = "";
            document.getElementById('ContentSection_ASPxSplitter1_TxtComodines').value = "";
            CmbRango.SetValue(null);
            DESDE.SetText("");
            HASTA.SetText("");
        }


        /*Ajustar el tamaño del Splitter al 100% y colapsa - expande el pane1*/
        function OnInitSplitter(s, e) {
            AdjustSizeSplitter();
            document.getElementById("splitterContainer").style.visibility = "";
            AdjustarPane();
        }
        function OnControlsInitializedSplitter(s, e) {
            ASPxClientUtils.AttachEventToElement(window, "resize", function (evt) {
                AdjustSizeSplitter();
                AdjustarPane();
            });
        }
        function AdjustSizeSplitter() {
            var height = Math.max(0, document.documentElement.clientHeight * 0.88);
            splitter.SetHeight(height);
        }
        function AdjustarPane() {
            var pane1 = splitter.GetPaneByName('SplitterPane1');
            if (document.documentElement.clientWidth > 766) {
                pane1.collapsed = false;
            }
            else
                pane1.collapsed = true;
        }

        /*Ajustar el tamaño del grid principal al 100% usando scrollbarvertical */
        function OnInit(s, e) {
            AdjustSize();
            document.getElementById("gridContainer").style.visibility = "";

            /*
            OcultarColumnaED(cbxED.GetValue());
            OcultarColumnaPorcED(cbxPorcED.GetValue());


            $(".column_width_cg").width(34);
            $(".column_width_np").width(34);
            $(".column_width_pa").width(110);
            $(".column_width_encabezado").width(100);
            $(".column_width_partida").width(75);
            $(".column_width_rfc").width(110);
            $(".column_width_pedimento").width(160);
            $(".column_width_fecha").width(120);
            $(".column_width_tipo").width(130);
            $(".column_width_tipo_pedimento").width(130);
            $(".column_width_clave_pedimento").width(120);
            $(".column_width_clave_aduana").width(100);
            $(".column_width_peso_bruto").width(100);
            $(".column_width_tipo_cambio").width(110);
            $(".column_width_valor_dolares").width(110);
            $(".column_width_valor_aduanal").width(110);
            $(".column_width_valor_comercial").width(115);
            $(".column_width_fecha_pago_original").width(125);
            $(".column_width_fecha_entrada_presentacion").width(145);
            */

        }
        function OnEndCallback(s, e) {
            AdjustSize();
        }
        function OnControlsInitialized(s, e) {
            ASPxClientUtils.AttachEventToElement(window, "resize", function (evt) {
                AdjustSize();
            });
        }
        function AdjustSize() {
            //var height = Math.max(0, document.documentElement.clientHeight * 0.74);
            var height = Math.max(0, document.documentElement.clientHeight * 0.65);
            grid.SetHeight(height);
        }



        /* función que se usa en el Uploaded   */
        function onFilesUploadStart(s, e) {

        }

        /* función que se usa en el Uploaded   */
        function OnUploadComplete(arg) {
            lblFiles.SetText(arg.callbackData);
            document.getElementById('<%=btnQuitar.ClientID%>').style.display = "block";

            //Se usa para validar si se guarda o no
            document.cookie = "CookieValor=si_guarda;";
        }

        function QuitarFile() {
            lblFiles.SetText('');
            document.getElementById('<%=btnQuitar.ClientID%>').style.display = "none";

            //Se usa para validar si se guarda o no
            document.cookie = "CookieValor=no_guarda;";
        }

        /*Se usa para abrir los archivos en otra página o ventana*/
        //function openInNewTab() {
        //    window.document.forms[0].target = '_blank';
        //    setTimeout(function () { window.document.forms[0].target = ''; }, 0);
        //}

        //*********** Numero de Parte
        //funcion para cada registro del gridnp 

        function rbConsultarClick(s, e, index) {
            var cheked = s.GetChecked();

            var rbConsultar = eval("rbConsultar" + index);
            var valor = rbConsultar.GetChecked();

            for (var i = 0; i < gridnp.GetVisibleRowsOnPage() ; i++) {
                if (i == index) {
                    rbConsultar = eval("rbConsultar" + i);
                    rbConsultar.SetChecked(valor);
                }
                else {
                    try {
                        rbConsultar = eval("rbConsultar" + i);
                        rbConsultar.SetChecked(false);
                    }
                    catch (err) { }
                }
            }
        }

        function CargaLoading() {
            Callback3.PerformCallback();
            LoadingPanel3.Show();
        }

        /*Función realiza validaciones en campos númericos*/
        function ValidaValor(s) {
            var cantidad = se_Cantidad.GetValue();
            var dolar = se_ValorDolares.GetValue();

            if (isNaN(cantidad) || !cantidad || 0 === cantidad.length) {
                se_Cantidad.SetValue(0);
            }

            if (isNaN(dolar) || !dolar || 0 === dolar.length) {
                se_ValorDolares.SetValue(0);
            }
        }
        //***********


        var textSeparator = ";";
        function updateText() {
            var selectedItems = checkListBox.GetSelectedItems();
            checkComboBox.SetText(getSelectedItemsText(selectedItems));
        }
        function synchronizeListBoxValues(dropDown, args) {
            checkListBox.UnselectAll();
            var texts = dropDown.GetText().split(textSeparator);
            var values = getValuesByTexts(texts);
            checkListBox.SelectValues(values);
            updateText(); // for remove non-existing texts
        }
        function getSelectedItemsText(items) {
            var texts = [];
            for (var i = 0; i < items.length; i++)
                texts.push(items[i].text);
            return texts.join(textSeparator);
        }
        function getValuesByTexts(texts) {
            var actualValues = [];
            var item;
            for (var i = 0; i < texts.length; i++) {
                item = checkListBox.FindItemByText(texts[i]);
                if (item != null)
                    actualValues.push(item.value);
            }
            return actualValues;
        }


        //***********************************************************
        //Ocultar/Mostrar Columna ED
        var valorED = true;

        function OcultarColumnaED(visible) {
            valorED = visible;

            var disp = visible ? 'table-cell' : 'none';
            $('th.column_hidden_ed').css('display', disp);
            $('td.column_hidden_ed').css('display', disp);

            
            var index = $('td.column_hidden_ed').index();
            var dxtbl = $('td.column_hidden_ed').parent().prev().eq(0);
            dxtbl.children().eq(index).css('display', disp);

            /*
            var indexh = $('th.column_hidden_ed').index();
            var dxtblh = $('th.column_hidden_ed').parent().prev().eq(0);
            dxtblh.children().eq(indexh).css('display', disp);
            */
        }

        function cbxED_Init(s, e) {
            s.SetValue(valorED);
        }

        function cbxED_CheckedChanged(s, e) {
            OcultarColumnaED(s.GetValue());
        }
        //***********************************************************

        //***********************************************************
        //Ocultar/Mostrar Columna Porcentaje ED
        var valorPED = true;

        function OcultarColumnaPorcED(visible) {
            valorPED = visible;
            var disp = visible ? 'table-cell' : 'none';
            $('th.column_hidden_ped').css('display', disp);
            $('td.column_hidden_ped').css('display', disp);

            /*
            var index = $('td.column_hidden_ped').index();
            var dxtbl = $('td.column_hidden_ped').parent().prev().eq(0);
            dxtbl.children().eq(index).css('display', disp);

            var indexh = $('th.column_hidden_ped').index();
            var dxtblh = $('th.column_hidden_ped').parent().prev().eq(0);
            dxtblh.children().eq(indexh).css('display', disp);
            */

            /*var MasterTable = grid.get_masterTableView();
            MasterTable.getColumnByUniqueName("CG").get_element().width = "500px";*/           
            
        }

        function cbxPorcED_Init(s, e) {
            s.SetValue(valorPED);
        }

        function cbxPorcED_CheckedChanged(s, e) {
            OcultarColumnaPorcED(s.GetValue());
        }
        //***********************************************************

        function RefrescaGrid1y2() {
            __doPostBack('btnRefrescaGrid1y2', 'FilePedimento');
        }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentSection" runat="server">      
    <asp:MultiView ID="MultiView1" ActiveViewIndex="0" runat="server">
        <asp:View ID="View1" runat="server">
            <div class="container-fluid">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h1 id="h1_titulo" runat="server" class="panel-title small"></h1>
                    </div>
                    <div class="panel-body" > 
                        <asp:Panel ID="Panel1" runat="server">
                            <div class="row" style="padding-left:10px; padding-right:10px;">
                                <div id="splitterContainer" style="visibility: hidden">
                                <dx:ASPxSplitter ID="ASPxSplitter1" runat="server"  Width="100%" Height="100%" Theme="SoftOrange" EnableTheming="true" ResizingMode="Live" 
                                FullscreenMode="false" ClientInstanceName="splitter" > <%--Height="755px"--%>
                                
                                <ClientSideEvents Init="OnInitSplitter"/>
                                <Styles>                                                                        
                                    <Pane>
                                        <Paddings Padding="0px" />
                                    </Pane>
                                </Styles>
                                <Panes>                                    
                                   <dx:SplitterPane Name="SplitterPane1"  MinSize="155px" MaxSize="155px" Size="155px" ScrollBars="Vertical"
                                        ShowCollapseForwardButton="True" ShowCollapseBackwardButton="True" Collapsed="true" >
                                        <PaneStyle>
                                            <Paddings PaddingLeft="0px" PaddingTop="0px" PaddingRight="0px"/>
                                        </PaneStyle>                                     
                                        <ContentCollection>                                            
                                            <dx:SplitterContentControl ID="SplitterContentControl1" runat="server" SupportsDisabledAttribute="True">                                                
                                                <dx:ASPxGridView ID="Grid2" runat="server" EnableTheming="True" Theme="MetropolisBlue" Visible="true"
                                                    EnableCallBacks="false" ClientInstanceName="grid2" AutoGenerateColumns="False" Width="100%"
                                                    Settings-HorizontalScrollBarMode="Auto" KeyFieldName="YYMM" SettingsPager-PageSize="400" Font-Size="11px">                                   
                                                    <SettingsResizing ColumnResizeMode="Control" /> 
                                                    <SettingsPager PageSize="400">
                                                    </SettingsPager>
                                                    <Settings ShowFooter="False" ShowFilterRow="false" ShowFilterRowMenu="false"  />  
                                                    <SettingsBehavior AllowSort="false" AllowDragDrop="false" AllowGroup="false" />
                                                    <Styles>                                   
                                                        <Header BackColor="#F4F4F4" ForeColor="#000000" Font-Overline="false"  
                                                                 Font-Underline="false" Font-Bold="false" />
                                                             <SelectedRow BackColor="#9E9E9E" ForeColor="#FFFFFF" />
                                                             <Row  />
                                                             <AlternatingRow Enabled="True" />
                                                    </Styles>                                                   
                                                    <Columns>                                                                              
                                                        <dx:GridViewDataTextColumn Caption=" " FieldName="YYMM" ReadOnly="True" Visible="false" VisibleIndex="0" >
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataColumn Caption=" " FieldName="YYMM" VisibleIndex="1" Width="32%">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <CellStyle HorizontalAlign="Center"></CellStyle>
                                                            <DataItemTemplate>

                                                                    <dx:BootstrapButton ID="btnBuscarFecha" runat="server" Text="" OnCommand="btnBuscar_Command"
                                                                        CommandName='<%# Eval("YY") %>' CommandArgument='<%# Eval("MM") %>' ToolTip='<%# Eval("YY") + "-" + Eval("MM") %>'
                                                                        SettingsBootstrap-RenderOption="Primary" AutoPostBack="True" CssClasses-Control="btn btn-primary btn-xs" >
                                                                        <ClientSideEvents Click="function(s, e) { Callback.PerformCallback(); LoadingPanel1.Show(); }" 
                                                                                          Init="function(s, e) {LoadingPanel1.Hide();}" />
                                                                        <CssClasses Icon="glyphicon glyphicon-search" /> 
                                                                    </dx:BootstrapButton>
                                                                                                                  
                                                            </DataItemTemplate>
                                                        </dx:GridViewDataColumn>
                                                        <dx:GridViewDataTextColumn Caption="AÑO" FieldName="YY" ReadOnly="True" VisibleIndex="2" Width="36%">
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="11px" />
                                                            <CellStyle HorizontalAlign="Center" Font-Size="11px" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="MES" FieldName="MM" ReadOnly="True" VisibleIndex="3" Width="32%">
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="11px" />
                                                            <CellStyle HorizontalAlign="Center" Font-Size="11px" />
                                                        </dx:GridViewDataTextColumn>                                                                                                                                                         
                                                    </Columns>                    
                                                </dx:ASPxGridView>
                                                <%--<div id="divSlide" runat="server" class="slide-bar"></div>--%>                                              
                                            </dx:SplitterContentControl>
                                        </ContentCollection>
                                   </dx:SplitterPane>
                                   <dx:SplitterPane Name="SplitterPane2" ScrollBars="Vertical" >
                                    <ContentCollection>
                                        <dx:SplitterContentControl ID="SplitterContentControl2" runat="server" BackColor="#F4F4F4">                                            
                                            <div class="row" style="padding-left:10px; padding-top:10px; padding-right:10px; padding-bottom:0px;">                        
                                                <asp:Panel ID="Panelbtn" runat="server" DefaultButton="btnBuscar">
                                                <div class="col-md-12">
                                                <div class="col-sm-4 col-md-2">
                                                    <div runat="server" id="DivPedimento">
                                                        <div class="form-group" style="position: relative; width: 100%; float: left;" title="" data-toggle="tooltip">
                                                            <div class="input-group">
                                                                <span class="input-group-addon" id="basic-addon1">
                                                                    <span class="glyphicon glyphicon-list-alt" aria-hidden="true"></span>
                                                                </span>
                                                                <asp:TextBox ID="TxtPedimento" runat="server" CssClass="form-control input-sm" MaxLength="50" placeholder="Pedimento" Font-Size="11px"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-sm-4 col-md-2">
                                                    <div runat="server" id="DivAduana">
                                                        <div class="form-group" style="position: relative; width: 100%; float: left;" title="" data-toggle="tooltip">
                                                            <div class="input-group">
                                                                <span class="input-group-addon" id="basic-addon2">
                                                                    <span class="glyphicon glyphicon-eye-open" aria-hidden="true"></span>
                                                                </span>
                                                            
                                                                <dx:ASPxComboBox ID="cmbAduana" Caption="" runat="server" Height="30px" NullText="Aduana" DataSecurityMode="Default"  
                                                                    TextField="Aduana" ValueField="Aduana"  Width="100%" Font-Size="12px" Font-Names="Arial,Helvetica" CssClass="bordes_curvos_derecha" 
                                                                    Theme="MaterialCompact" ForeColor="#6B5555">
                                                                    <ClearButton DisplayMode="Never" />
                                                                </dx:ASPxComboBox>

                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-sm-4 col-md-2">
                                                    <div runat="server" id="DivClave">
                                                        <div class="form-group" style="position: relative; width: 100%; float: left;" title="" data-toggle="tooltip">
                                                            <div class="input-group">
                                                                <span class="input-group-addon" id="basic-addon3">
                                                                    <span class="glyphicon glyphicon-list-alt" aria-hidden="true"></span>
                                                                </span>
                                                                <dx:ASPxComboBox ID="cmbClave" Caption="" runat="server" Height="30px" NullText="Clave" DataSecurityMode="Default"  
                                                                   TextField="Clave" ValueField="Clave"  Width="100%" Font-Size="12px" Font-Names="Arial,Helvetica" CssClass="bordes_curvos_derecha" 
                                                                   Theme="MaterialCompact" ForeColor="#6B5555">
                                                                   <ClearButton DisplayMode="Never" />
                                                                </dx:ASPxComboBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-sm-4 col-md-2">
                                                    <div runat="server" id="DivTexto">
                                                        <div class="form-group" style="position: relative; width: 100%; float: left;" title="" data-toggle="tooltip">
                                                            <div class="input-group">
                                                                <span class="input-group-addon" id="basic-addon4">
                                                                    <span class="glyphicon glyphicon-font" aria-hidden="true"></span>
                                                                </span>
                                                                <asp:TextBox ID="TxtTexto" runat="server" CssClass="form-control input-sm" MaxLength="50" placeholder="Texto" Font-Size="11px"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-sm-4 col-md-1">
                                                    <dx:BootstrapButton ID="btnOcultarCG" runat="server" AutoPostBack="false" OnClick="btnOcultarCG_Click"  Text="Ocultar CG" CssClasses-Text="txt-sm" >
                                                        <%--<CssClasses Icon="glyphicon glyphicon-transfer" />--%>
                                                        <SettingsBootstrap RenderOption="Primary" Sizing="Small" />                                                                                
                                                        <ClientSideEvents Click="function(s, e) { Callback.PerformCallback(); LoadingPanel0.Show(); OcultarAparecerColumna(s); }" />
                                                    </dx:BootstrapButton>
                                                </div>
                                                <div class="col-sm-4 col-md-1">
                                                    <dx:BootstrapButton ID="btnOcultarNP" runat="server" AutoPostBack="false" OnClick="btnOcultarNP_Click"  Text="Ocultar NP" CssClasses-Text="txt-sm" >
                                                        <%--<CssClasses Icon="glyphicon glyphicon-transfer" />--%>
                                                        <SettingsBootstrap RenderOption="Primary" Sizing="Small" />                                                                                
                                                        <ClientSideEvents Click="function(s, e) { Callback.PerformCallback(); LoadingPanel0.Show(); OcultarAparecerColumna(s); }" />
                                                    </dx:BootstrapButton>
                                                </div>
                                                <div class="col-sm-4 col-md-2">
                                                    <div runat="server" id="DivComodines">
                                                        <%--<div class="form-group" style="position: relative; width: 100%; float: left;" title="" data-toggle="tooltip">
                                                            <div class="input-group">
                                                                <span class="input-group-addon" id="basic-addon5">
                                                                    <span class="glyphicon glyphicon-bookmark" aria-hidden="true"></span>
                                                                </span>
                                                       --%>                                                              
                                                                <dx:BootstrapButton ID="btnOcultarPorcED" runat="server" AutoPostBack="false" OnClick="btnOcultarPorcED_Click"  Text="Ocultar (%) ED" CssClasses-Text="txt-sm" >
                                                                    <%--<CssClasses Icon="glyphicon glyphicon-transfer" />--%>
                                                                    <SettingsBootstrap RenderOption="Primary" Sizing="Small" />                                                                                
                                                                    <ClientSideEvents Click="function(s, e) { Callback.PerformCallback(); LoadingPanel0.Show(); OcultarAparecerColumna(s); }" />
                                                                </dx:BootstrapButton>
                                                         <%--       
                                                                <table style="width:100%">
                                                                    <tr>
                                                                        <td>
                                                                            <dx:ASPxCheckBox ID="cbxCG" Text="CG" ClientInstanceName="cbxCG" runat="server">
                                                                                <ClientSideEvents Init="cbxCG_Init" CheckedChanged="cbxCG_CheckedChanged" />
                                                                            </dx:ASPxCheckBox>
                                                                        </td>
                                                                        <td>
                                                                            <dx:ASPxCheckBox ID="cbxPorcNP" Text="NP" ClientInstanceName="cbxNP" runat="server">
                                                                                <ClientSideEvents Init="cbxPN_Init" CheckedChanged="cbxNP_CheckedChanged" />
                                                                            </dx:ASPxCheckBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>--%>  
                                                            <%--</div>
                                                        </div>--%>
                                                    </div>
                                                </div>
                                               <%-- <div class="col-sm-4 col-md-2">--%>
                                                    
                                                 <%--<table style="width:100%">
                                                        <tr>
                                                            <td>
                                                                <dx:ASPxCheckBox ID="cbxED" Text="ED" ClientInstanceName="cbxED" runat="server">
                                                                    <ClientSideEvents Init="cbxED_Init" CheckedChanged="cbxED_CheckedChanged" />
                                                                </dx:ASPxCheckBox>
                                                            </td>
                                                            <td>
                                                                <dx:ASPxCheckBox ID="cbxPorcED" Text="(%) ED" ClientInstanceName="cbxPorcED" runat="server">
                                                                    <ClientSideEvents Init="cbxPorcED_Init" CheckedChanged="cbxPorcED_CheckedChanged" />
                                                                </dx:ASPxCheckBox>
                                                            </td>
                                                        </tr>
                                                    </table>--%>

                                                <%--</div>--%>
                                                </div>
                                                <div class="col-md-12">
                                                <div class="col-sm-4 col-md-2">
                                                    <div runat="server" id="DivRango">
                                                        <div class="form-group" style="position: relative; width: 100%; float: left;" title="" data-toggle="tooltip">
                                                            <div class="input-group">
                                                                <span class="input-group-addon" id="basic-addon6">
                                                                    <span class="glyphicon glyphicon-resize-horizontal" aria-hidden="true"></span>
                                                                </span>
                                                                <dx:ASPxComboBox ID="CmbRango" Caption="" runat="server" ClientInstanceName="CmbRango" Height="30px" NullText="Rango" DataSecurityMode="Default"
                                                                    Width="100%" Font-Size="11px" Font-Names="Arial,Helvetica" CssClass="bordes_curvos_derecha" Theme="MaterialCompact"
                                                                    OnSelectedIndexChanged="CmbRango_SelectedIndexChanged" AutoPostBack="true"  ForeColor="#6B5555">
                                                                    <ClientSideEvents SelectedIndexChanged="function(s, e) { Callback.PerformCallback(); LoadingPanel1.Show(); }" 
                                                                                Init="function(s, e) {LoadingPanel1.Hide(); }" />
                                                                    <Items>
                                                                        <dx:ListEditItem Text="Hoy" Value="Item1" />
                                                                        <dx:ListEditItem Text="Mes Actual" Value="Item2" />
                                                                        <dx:ListEditItem Text="Año Actual" Value="Item3" />
                                                                        <dx:ListEditItem Text="Año pasado" Value="Item4" />
                                                                        <dx:ListEditItem Text="5 Años" Value="Item5" />
                                                                    </Items>
                                                                    <ClearButton DisplayMode="Never" />
                                                                </dx:ASPxComboBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-sm-4 col-md-2">
                                                    <div runat="server" id="DivDesde">
                                                        <div class="form-group" style="position: relative; width: 100%; float: left;" runat="server" id="FgDesde" title="" data-toggle="tooltip">
                                                            <div class="input-group">
                                                                <span class="input-group-addon" id="basic-addon7">
                                                                    <span id="spanDesde" runat="server" class="glyphicon glyphicon-calendar" aria-hidden="true" ></span>
                                                                </span>
                                                                <dx:ASPxDateEdit ID="DESDE" runat="server" ClientInstanceName="DESDE" EditFormat="Custom" Date="" Width="100%" Theme="MaterialCompact"
                                                                    Font-Size="11px" CssClass="bordes_curvos_derecha"  NullText="Desde" DisplayFormatString="dd/MM/yyyy" >
                                                                    <CalendarProperties >
                                                                        <Style Font-Size="12px"></Style>                                                                                    
                                                                    </CalendarProperties>                                                                                
                                                                </dx:ASPxDateEdit>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-sm-4 col-md-2">
                                                    <div runat="server" id="DivHasta">
                                                        <div class="form-group" style="position: relative; width: 100%; float: left;" runat="server" id="FgHasta" title="" data-toggle="tooltip">
                                                            <div class="input-group">
                                                                <span class="input-group-addon" id="basic-addon8">
                                                                    <span id="spanHasta" runat="server" class="glyphicon glyphicon-calendar" aria-hidden="true" ></span>
                                                                </span>
                                                                 <dx:ASPxDateEdit ID="HASTA" runat="server" ClientInstanceName="HASTA" EditFormat="Custom" Date="" Width="100%" Theme="MaterialCompact" 
                                                                    Font-Size="11px" CssClass="bordes_curvos_derecha" NullText="Hasta" DisplayFormatString="dd/MM/yyyy">
                                                                    <CalendarProperties >
                                                                        <Style Font-Size="12px"></Style>
                                                                    </CalendarProperties>                                                                                
                                                                </dx:ASPxDateEdit>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-xs-4 col-md-1" >
                                                    <div runat="server" id="DivBuscar" >
                                                        <div class="form-group" style="position: relative; width: 50%; float: left;" title="" data-toggle="tooltip">
                                                            <div class="input-group">
                                                                <dx:BootstrapButton ID="btnBuscar" runat="server" AutoPostBack="false" OnClick="btnBuscar_OnClick" Text="Buscar" CssClasses-Text="txt-sm" >
                                                                    <CssClasses Icon="glyphicon glyphicon-search" />
                                                                    <SettingsBootstrap RenderOption="Primary" Sizing="Small" />                                                                                
                                                                    <ClientSideEvents Click="function(s, e) { Callback.PerformCallback(); LoadingPanel0.Show(); }" />
                                                                </dx:BootstrapButton>                                                                                                 
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-xs-4 col-md-1">
                                                    <div runat="server" id="DivLimpiar" >
                                                        <div class="form-group" style="position: relative; width: 50%; float: left;" title="" data-toggle="tooltip">
                                                            <div class="input-group">
                                                                <asp:LinkButton ID="btnLimpiar" runat="server" CssClass="btn btn-primary btn-sm" OnClientClick="btnLimpiar_Click(); return false;">
                                                                    <span class="glyphicon glyphicon-erase"></span>&nbsp;&nbsp;Limpiar
                                                                </asp:LinkButton>
                                                                &nbsp;                                                           
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                </div>       
                                                <%--<div class="col-sm-2 col-md-1" style="width: 100%;"></div>--%>
                                                <dx:ASPxCallback ID="ASPxCallback0" runat="server" ClientInstanceName="Callback" >
                                                    <ClientSideEvents CallbackComplete="function(s, e) { System.Threading.Thread.Sleep(3000); LoadingPanel0.Hide(); }" />
                                                </dx:ASPxCallback>
                                                <dx:ASPxLoadingPanel ID="LoadingPanel0" runat="server" ClientInstanceName="LoadingPanel0"
                                                    Modal="True" ViewStateMode="Enabled" Theme="MaterialCompact" >
                                                </dx:ASPxLoadingPanel>
                                                <%--                       
                                                                <dx:ASPxGridView ID="grid" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1" 
                                                                EnableTheming="True" Theme="MetropolisBlue" Width="200px" CssClass="table table-striped table-bordered table-hover" >
                                                                <SettingsAdaptivity AdaptivityMode="HideDataCells" >
                                                                </SettingsAdaptivity>
                                                                <Columns>
                                                                    <dx:GridViewDataTextColumn FieldName="RFC" ReadOnly="True" VisibleIndex="0" Width="14%" HeaderStyle-CssClass="visible-lg" CellStyle-CssClass ="visible-lg">
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataDateColumn FieldName="FECHA" ReadOnly="True" VisibleIndex="2" Width="10%">
                                                                    </dx:GridViewDataDateColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="TIPO" ReadOnly="True" VisibleIndex="3" Width="15%">
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="TIPO PEDIMENTO" ReadOnly="True" VisibleIndex="4" Width="16%">
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="CLAVE PEDIMENTO" ReadOnly="True" VisibleIndex="5" Width="15%">
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="CLAVE ADUANA" VisibleIndex="6" Caption="ADUANA" Width="10%">
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="PESO BRUTO" VisibleIndex="8" Width="10%">
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="TIPO DE CAMBIO" VisibleIndex="9" Width="12%">
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="VALOR DOLARES" VisibleIndex="10" ReadOnly="True" Width="12%">
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="VALOR ADUANAL" VisibleIndex="11" Width="12%">
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="VALOR COMERCIAL" VisibleIndex="12" Width="13%">
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataDateColumn FieldName="FECHA PAGO ORIGINAL" ReadOnly="True" VisibleIndex="13" Width="15%" >
                                                                    </dx:GridViewDataDateColumn>
                                                                    <dx:GridViewDataDateColumn FieldName="FECHA ENTRADA / PRESENTACION" ReadOnly="True" VisibleIndex="14" Width="20%" >
                                                                    </dx:GridViewDataDateColumn>
                                                                    <dx:GridViewDataHyperLinkColumn AdaptivePriority="1" AllowTextTruncationInAdaptiveMode="False" Caption="ADUANA-PATENTE-PEDIMENTO" FieldName="PEDIMENTOARMADO" VisibleIndex="1" Width="28%">
                                                                        <PropertiesHyperLinkEdit NavigateUrlFormatString="Documento.aspx?id={0}" Target="_blank" TextField="PEDIMENTOARMADO" >
                                                                        </PropertiesHyperLinkEdit>
                                                                    </dx:GridViewDataHyperLinkColumn>
                                                                </Columns>
                                                                <Settings HorizontalScrollBarMode="Visible" VerticalScrollBarMode="Auto"  />
                                                                </dx:ASPxGridView>
                                                                --%>         
                                                <%--<dx:BootstrapGridView ID="grid" runat="server" DataSourceID="SqlDataSource1">
                                                                <SettingsBehavior AllowSelectByRowClick="true" />
                                                                <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                                                                <SettingsPager ></SettingsPager>
                                                                <Columns>
                                                                    <dx:BootstrapGridViewTextColumn FieldName="RFC" ReadOnly="True" VisibleIndex="1" Width="110px">
                                                                    </dx:BootstrapGridViewTextColumn>
                                                                    <dx:BootstrapGridViewDateColumn FieldName="FECHA" ReadOnly="True" VisibleIndex="3" Width="100px">
                                                                    </dx:BootstrapGridViewDateColumn>
                                                                    <dx:BootstrapGridViewTextColumn FieldName="TIPO" ReadOnly="True" VisibleIndex="4" Width="130px">
                                                                    </dx:BootstrapGridViewTextColumn>
                                                                    <dx:BootstrapGridViewTextColumn FieldName="TIPO PEDIMENTO" ReadOnly="True" VisibleIndex="5" Width="130px">
                                                                    </dx:BootstrapGridViewTextColumn>
                                                                    <dx:BootstrapGridViewTextColumn FieldName="CLAVE PEDIMENTO" ReadOnly="True" VisibleIndex="6" Width="130px">
                                                                    </dx:BootstrapGridViewTextColumn>
                                                                    <dx:BootstrapGridViewTextColumn FieldName="CLAVE ADUANA" VisibleIndex="7" Caption="ADUANA" Width="80px">
                                                                    </dx:BootstrapGridViewTextColumn>
                                                                    <dx:BootstrapGridViewTextColumn FieldName="PESO BRUTO" VisibleIndex="8" Width="100px">
                                                                    </dx:BootstrapGridViewTextColumn>
                                                                    <dx:BootstrapGridViewTextColumn FieldName="TIPO DE CAMBIO" VisibleIndex="9" Width="120px">
                                                                    </dx:BootstrapGridViewTextColumn>
                                                                    <dx:BootstrapGridViewTextColumn FieldName="VALOR DOLARES" VisibleIndex="10" ReadOnly="True" Width="120px">
                                                                    </dx:BootstrapGridViewTextColumn>
                                                                    <dx:BootstrapGridViewTextColumn FieldName="VALOR ADUANAL" VisibleIndex="11" Width="120px">
                                                                    </dx:BootstrapGridViewTextColumn>
                                                                    <dx:BootstrapGridViewTextColumn FieldName="VALOR COMERCIAL" VisibleIndex="12" Width="120px">
                                                                    </dx:BootstrapGridViewTextColumn>
                                                                    <dx:BootstrapGridViewDateColumn FieldName="FECHA PAGO ORIGINAL" ReadOnly="True" VisibleIndex="13" Width="140px">
                                                                    </dx:BootstrapGridViewDateColumn>
                                                                    <dx:BootstrapGridViewDateColumn FieldName="FECHA ENTRADA / PRESENTACION" ReadOnly="True" VisibleIndex="14" Width="200px">
                                                                    </dx:BootstrapGridViewDateColumn>
                                                                    <dx:BootstrapGridViewHyperLinkColumn Caption="ADUANA-PATENTE-PEDIMENTO" FieldName="PEDIMENTOARMADO" VisibleIndex="2" Width="190px">
                                                                        <PropertiesHyperLinkEdit NavigateUrlFormatString="Documento.aspx?id={0}" Target="_blank" TextField="PEDIMENTOARMADO">
                                                                        </PropertiesHyperLinkEdit>
                                                                    </dx:BootstrapGridViewHyperLinkColumn>
                                                                </Columns>
                                                                </dx:BootstrapGridView>--%>
                                                </asp:Panel>                                                  
                                            </div>                                            
                                            <div style="margin-left:5px; margin-right:5px; margin-top:0px; margin-bottom:5px">
                                                <div id="gridContainer" style="visibility: hidden">
                                                <dx:ASPxGridView ID="Grid" runat="server" EnableTheming="True" Theme="SoftOrange" 
                                                    AutoGenerateColumns="False" Width="100%" Settings-HorizontalScrollBarMode="Auto" OnCustomCallback="Grid_CustomCallback"
                                                    ClientInstanceName="grid" Styles-DetailCell-Paddings-Paddingtop="3px" OnCustomButtonCallback="Grid_CustomButtonCallback"
                                                    KeyFieldName="PEDIMENTOARMADO" EnableCallBacks="false" SettingsPager-Position="TopAndBottom" 
                                                    OnCustomButtonInitialize="Grid_CustomButtonInitialize" Styles-Header-Font-Size="11px"
                                                    Styles-DetailCell-Paddings-PaddingBottom="3px"> 
                                                    <SettingsResizing ColumnResizeMode="Control" />
                                                    <SettingsPager Position="TopAndBottom">
                                                    </SettingsPager>
                                                    <Settings ShowFooter="True" ShowHeaderFilterButton="true" ShowFilterRowMenu="false" 
                                                        ShowGroupPanel="false" ShowVerticalScrollBar="true" VerticalScrollableHeight="0"  />
                                                    <%--<SettingsLoadingPanel Mode="Default" />
                                                                <ClientSideEvents BeginCallback="function(s, e) {
                                                                	loadingPanel1.Show();
                                                                }" EndCallback="function(s, e) {
                                                                	 loadingPanel1.Hide();
                                                                }" />--%>
                                                    <SettingsBehavior AllowSelectByRowClick="false" AllowSelectSingleRowOnly="true" FilterRowMode="OnClick"
                                                            ProcessSelectionChangedOnServer="false" EnableCustomizationWindow="true" />
                                                    <%--<SettingsPager Position="Bottom" ShowDisabledButtons="false" ShowNumericButtons="true" 
                                                            Summary-Visible="false" ShowSeparators="false" >
                                                            <PageSizeItemSettings Items="10, 20, 50" Position="Right" Visible="true"/>
                                                        </SettingsPager>--%> 
                                                    <Styles>                                                               
                                                        <SelectedRow BackColor="#9E9E9E" ForeColor="#FFFFFF"  />
                                                        <Row Font-Size="11px" />
                                                            <DetailCell>
                                                                <Paddings PaddingBottom="3px" PaddingTop="3px" />
                                                            </DetailCell>
                                                        <AlternatingRow Enabled="True" />
                                                        <PagerTopPanel Paddings-PaddingBottom="3px">
                                                            <Paddings PaddingBottom="3px" />
                                                        </PagerTopPanel>
                                                        <EmptyDataRow CssClass="myEmptyDataRow"></EmptyDataRow>
                                                    </Styles>
                                                    <SettingsPopup>
                                                        <HeaderFilter Height="200px" Width="195px"/>
                                                    </SettingsPopup>
                                                    <ClientSideEvents Init="OnInit" EndCallback="OnEndCallback" />
                                                    <%--<ClientSideEvents RowClick="OnRowClick" DetailRowExpanding="OnDetailRowExpanding"
                                                        DetailRowCollapsing="OnDetailRowCollapsing" />--%>
                                                    <SettingsDetail ShowDetailRow="true" AllowOnlyOneMasterRowExpanded="true" ShowDetailButtons="false" /> 
                                                    <Columns>
                                                        <%--<dx:GridViewCommandColumn Caption="ACCIONES" VisibleIndex="0">
                                                           <CustomButtons>
                                                               <dx:GridViewCommandColumnCustomButton ID="btnSeleccionar" Text="Ver Detalle" 
                                                                   Styles-Style-HoverStyle-ForeColor="#000000" Styles-Style-ForeColor="#000000"
                                                                   Styles-Style-HoverStyle-Font-Bold="false" Styles-Style-Font-Bold="false">                                        
                                                                   <Styles>
                                                                       <Style Font-Bold="False" ForeColor="Black">
                                                                           <HoverStyle Font-Bold="False" ForeColor="Black">
                                                                           </HoverStyle>
                                                                       </Style>
                                                                   </Styles>
                                                               </dx:GridViewCommandColumnCustomButton>
                                                           </CustomButtons>
                                                           <HeaderStyle HorizontalAlign="Center" ForeColor="#FFFFFF" />
                                                       </dx:GridViewCommandColumn>--%>
                                                        <%--<dx:GridViewDataTextColumn FieldName="CG" ReadOnly="True" VisibleIndex="0" Visible="False">
                                                        </dx:GridViewDataTextColumn>--%>
                                                        <dx:GridViewCommandColumn Caption="CG" VisibleIndex="0" ButtonRenderMode="Image" Width="34px" ToolTip="Cuenta Gastos">
                                                            <CustomButtons>
                                                                <dx:GridViewCommandColumnCustomButton ID="btnCG" Text="CG" Image-ToolTip="Cuenta Gastos" 
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
                                                            <%--<CustomButtons>
                                                                <dx:GridViewCommandColumnCustomButton ID="btnCG2" Text="CG" Image-ToolTip="Cuenta Gastos" 
                                                                    Styles-Style-HoverStyle-ForeColor="#000000" Styles-Style-ForeColor="#000000"
                                                                    Styles-Style-HoverStyle-Font-Bold="false" Styles-Style-Font-Bold="false">                                        
                                                                    <Image Url="../img/iconos/ico_cg1.png"></Image>
                                                                    <Styles>
                                                                        <Style Font-Bold="False" ForeColor="Black" Font-Size="11px">
                                                                            <HoverStyle Font-Bold="False" ForeColor="Black">
                                                                            </HoverStyle>
                                                                        </Style>
                                                                    </Styles>
                                                                </dx:GridViewCommandColumnCustomButton>
                                                            </CustomButtons>--%>
                                                            <HeaderStyle HorizontalAlign="Center" ForeColor="#FFFFFF" />
                                                        </dx:GridViewCommandColumn>
                                                        <dx:GridViewCommandColumn Caption="NP" VisibleIndex="0" ButtonRenderMode="Image" Width="34px" ToolTip="Número de Parte">
                                                            <CustomButtons>
                                                                <dx:GridViewCommandColumnCustomButton ID="btnNP" Text="NP" Image-ToolTip="Número de Parte"
                                                                    Styles-Style-HoverStyle-ForeColor="#000000" Styles-Style-ForeColor="#000000"
                                                                    Styles-Style-HoverStyle-Font-Bold="false" Styles-Style-Font-Bold="false">                                        
                                                                    <Image Url="../img/iconos/x_paloma.png"></Image>                                                                                
                                                                    <Styles>                                                                                    
                                                                        <Style Font-Bold="False" ForeColor="Black" Font-Size="11px">
                                                                            <HoverStyle Font-Bold="False" ForeColor="Black">
                                                                            </HoverStyle>
                                                                        </Style>
                                                                    </Styles>
                                                                </dx:GridViewCommandColumnCustomButton>
                                                            </CustomButtons>
                                                            <%--<Columns>
                                                                <dx:GridViewDataProgressBarColumn FieldName="PA" VisibleIndex="0" />
                                                            </Columns>--%>
                                                            <HeaderStyle HorizontalAlign="Center" ForeColor="#FFFFFF" />
                                                        </dx:GridViewCommandColumn>
                                                        <dx:GridViewDataColumn Caption="ED" VisibleIndex="0" Width="34px" ToolTip="Expediente Digital">
                                                            <EditFormSettings Visible="False" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <CellStyle HorizontalAlign="Center" />
                                                            <DataItemTemplate>
                                                                <%--<dx:ASPxLabel ID="lbl_Pedimentos" runat="server" Text='<%# Eval("PEDIMENTOARMADO") %>' ForeColor="#000" Font-Size="11px" />
                                                                <dx:ASPxButton ID="ASPxButtonDetailDoc" runat="server" AutoPostBack="false"   
                                                                    EnableTheming="false" RenderMode="Link" VerticalAlign="Top" ToolTip="Pedimento">
                                                                </dx:ASPxButton>--%>
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <dx:ASPxButton ID="ASPxButtonED" runat="server" AutoPostBack="false" OnClick="ASPxButtonED_Click"
                                                                                EnableTheming="false" RenderMode="Link" VerticalAlign="Top" ToolTip="Expediente Digital" OnInit="ASPxButtonED_Init">
                                                                            </dx:ASPxButton>
                                                                        </td>
                                                                    </tr>
                                                                 </table>
                                                            </DataItemTemplate>                                                            
                                                            <SettingsHeaderFilter Mode="CheckedList" />                                                                                                                        
                                                        </dx:GridViewDataColumn>                                                        
                                                        <dx:GridViewDataProgressBarColumn Caption="(%) ED" FieldName="PA" VisibleIndex="0"  Width="110px" HeaderStyle-HorizontalAlign="Center">
                                                            <DataItemTemplate>
                                                                <dx:ASPxProgressBar ID="ASPxProgressBar1" runat="server" Theme="iOS" CustomDisplayFormat="" Height="21px" Value='<%# Eval("PA") %>' Width="97%" Font-Size="11px">
                                                                </dx:ASPxProgressBar>
                                                            </DataItemTemplate>                                                        
                                                        </dx:GridViewDataProgressBarColumn>
                                                        <%--<dx:GridViewCommandColumn Caption="ED" VisibleIndex="0" ButtonRenderMode="Image" Width="120px" ToolTip="Expediente Digital">
                                                            <CustomButtons>
                                                                <dx:GridViewCommandColumnCustomButton ID="btnED" Text="ED" Image-ToolTip="Expediente Digital"
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
                                                        </dx:GridViewCommandColumn>--%>
                                                        <dx:GridViewCommandColumn Caption="ENCABEZADO" VisibleIndex="0">
                                                            <CustomButtons>                                                                            
                                                                <dx:GridViewCommandColumnCustomButton ID="btnVerEncabezado" Text="Ver Encabezado" 
                                                                    Styles-Style-HoverStyle-ForeColor="#FFF" Styles-Style-ForeColor="#FFF"
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
                                                        </dx:GridViewCommandColumn>
                                                        <dx:GridViewCommandColumn Caption="PARTIDA" VisibleIndex="0" Width="75px">
                                                            <CustomButtons>                                                                            
                                                                <dx:GridViewCommandColumnCustomButton ID="btnVerPartida" Text="Ver Partida" >
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
                                                        <%--<dx:GridViewDataColumn VisibleIndex="0" Width="70px">
                                                            <EditFormSettings Visible="False" />
                                                            <HeaderTemplate>
                                                                <dx:ASPxLabel ID="lbl_TitPartidas" runat="server" Text="PARTIDA" ForeColor="#FFFFFF" Font-Size="11px" />
                                                            </HeaderTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <DataItemTemplate>
                                                                <dx:ASPxButton ID="ASPxButtonDetail" runat="server" AutoPostBack="false" OnInit="ASPxButtonDetail_Init" 
                                                                   EnableTheming="false" RenderMode="Link" VerticalAlign="Top"> 
                                                                </dx:ASPxButton>
                                                            </DataItemTemplate>
                                                            <CellStyle HorizontalAlign="Center" />
                                                        </dx:GridViewDataColumn>--%>
                                                        <%--
                                                        <dx:GridViewCommandColumn Caption="Partidas" VisibleIndex="0">
                                                            <CustomButtons>
                                                                <dx:GridViewCommandColumnCustomButton ID="btnVerPartidas" Text="Ver Partidas" 
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
                                                        </dx:GridViewCommandColumn>
                                                        --%>
                                                        <dx:GridViewDataTextColumn FieldName="RFC" ReadOnly="True" VisibleIndex="1" Width="110px">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <SettingsHeaderFilter Mode="CheckedList" />
                                                            
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataColumn Caption="PEDIMENTO" FieldName="PEDIMENTOARMADO" VisibleIndex="2" Width="160px">
                                                            <EditFormSettings Visible="False" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <CellStyle HorizontalAlign="Center" />
                                                            <DataItemTemplate>
                                                                <%--<dx:ASPxLabel ID="lbl_Pedimentos" runat="server" Text='<%# Eval("PEDIMENTOARMADO") %>' ForeColor="#000" Font-Size="11px" />
                                                                <dx:ASPxButton ID="ASPxButtonDetailDoc" runat="server" AutoPostBack="false"   
                                                                    EnableTheming="false" RenderMode="Link" VerticalAlign="Top" ToolTip="Pedimento">
                                                                </dx:ASPxButton>--%>
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <dx:ASPxLabel ID="lbl_Pedimentos" runat="server" Text='<%# Eval("PEDIMENTOARMADO") %>' ForeColor="#000" Font-Size="11px" />
                                                                        </td>
                                                                        <td>&nbsp;&nbsp;
                                                                            <dx:ASPxButton ID="ASPxButtonDetailDoc" runat="server" AutoPostBack="false" OnClick="ASPxButtonDetailDoc_Click"
                                                                                EnableTheming="false" RenderMode="Link" VerticalAlign="Top" ToolTip="Pedimento" OnInit="ASPxButtonDetailDoc_Init">
                                                                              <%--   <ClientSideEvents Click="function(s,e) { 
                                                                                    window.document.forms[0].target = '_blank';
                                                                                    setTimeout(function () { window.document.forms[0].target = ''; }, 0);
                                                                                     }" /> --%>
                                                                            </dx:ASPxButton>
                                                                        </td>
                                                                    </tr>
                                                                 </table>
                                                            </DataItemTemplate>
                                                            
                                                            <SettingsHeaderFilter Mode="CheckedList" />
                                                        </dx:GridViewDataColumn>
                                                        <%--<dx:GridViewDataColumn Caption="PEDIMENTO" FieldName="PEDIMENTOARMADO" VisibleIndex="2" Width="170px">
                                                               
                                                             <HeaderTemplate>
                                                                <dx:ASPxLabel ID="lbl_TitPartidas" runat="server" Text="PEDIMENTO" ForeColor="#FFFFFF" Font-Size="11px" />
                                                            </HeaderTemplate>                                                                     
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <SettingsHeaderFilter Mode="CheckedList" />
                                                            <DataItemTemplate>
                                                                <dx:ASPxLabel ID="lbl_Pedimentos" runat="server" Text='<%# Eval("PEDIMENTOARMADO") %>' ForeColor="#000" Font-Size="11px" />
                                                                <dx:ASPxButton ID="ASPxButtonDetailDoc" runat="server" AutoPostBack="false" OnInit="ASPxButtonDetailDoc_Init" 
                                                                   EnableTheming="false" RenderMode="Link" VerticalAlign="Top"> 
                                                                </dx:ASPxButton>
                                                            </DataItemTemplate>
                                                            <CellStyle HorizontalAlign="Center" />
                                                        </dx:GridViewDataColumn>--%>
                                                        <%--<dx:GridViewDataTextColumn FieldName="PEDIMENTOARMADO" Caption="PEDIMENTO" VisibleIndex="2" Width="150px">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <SettingsHeaderFilter Mode="CheckedList" />
                                                                                 
                                                        </dx:GridViewDataTextColumn>--%>
                                                        <dx:GridViewDataDateColumn FieldName="FECHA" ReadOnly="True" VisibleIndex="3" Width="120px">
                                                            <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy"></PropertiesDateEdit>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <SettingsHeaderFilter Mode="CheckedList" />
                                                            <CellStyle HorizontalAlign="Center" />
                                                        </dx:GridViewDataDateColumn>
                                                        <dx:GridViewDataTextColumn FieldName="TIPO" ReadOnly="True" VisibleIndex="4" Width="130px">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <SettingsHeaderFilter Mode="CheckedList" />
                                                            
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="TIPO <br/> PEDIMENTO" FieldName="TIPO PEDIMENTO" ReadOnly="True" VisibleIndex="5" Width="130px">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <SettingsHeaderFilter Mode="CheckedList" />
                                                            
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="CLAVE <br/> PEDIMENTO" FieldName="CLAVE PEDIMENTO" ReadOnly="True" VisibleIndex="6" Width="120px">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <SettingsHeaderFilter Mode="CheckedList" />
                                                            
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="ADUANA" FieldName="CLAVE ADUANA" VisibleIndex="7" Width="100px">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <SettingsHeaderFilter Mode="CheckedList" />
                                                            
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="PESO <br/> BRUTO" FieldName="PESO BRUTO" VisibleIndex="8" Width="100px">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <CellStyle HorizontalAlign ="Right" />
                                                            <SettingsHeaderFilter Mode="CheckedList" />
                                                            <PropertiesTextEdit DisplayFormatString="{0:n3}" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="TIPO DE <br/> CAMBIO" FieldName="TIPO DE CAMBIO" VisibleIndex="9" Width="110px">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <CellStyle HorizontalAlign ="Right" />
                                                            <SettingsHeaderFilter Mode="CheckedList" />
                                                            <PropertiesTextEdit DisplayFormatString="{0:n5}" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="VALOR <br/> DOLARES" FieldName="VALOR DOLARES" VisibleIndex="10" ReadOnly="True" Width="110px">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <CellStyle HorizontalAlign ="Right" />
                                                            <SettingsHeaderFilter Mode="CheckedList" />
                                                            <PropertiesTextEdit DisplayFormatString="{0:n2}" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="VALOR <br/> ADUANAL" FieldName="VALOR ADUANAL" VisibleIndex="11" Width="110px">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <CellStyle HorizontalAlign ="Right" />
                                                            <SettingsHeaderFilter Mode="CheckedList" />
                                                            <PropertiesTextEdit DisplayFormatString="{0:n2}" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="VALOR <br/> COMERCIAL" FieldName="VALOR COMERCIAL" VisibleIndex="12" Width="115px">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <CellStyle HorizontalAlign ="Right" />
                                                            <SettingsHeaderFilter Mode="CheckedList" />
                                                            <PropertiesTextEdit DisplayFormatString="{0:n2}" />              
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataDateColumn Caption="FECHA PAGO <br/> ORIGINAL" FieldName="FECHA PAGO ORIGINAL" ReadOnly="True" VisibleIndex="13" Width="125px" PropertiesDateEdit-DisplayFormatString="" >
                                                            <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy"></PropertiesDateEdit>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <SettingsHeaderFilter Mode="CheckedList" />
                                                            
                                                        </dx:GridViewDataDateColumn>
                                                        <dx:GridViewDataDateColumn Caption="FECHA ENTRADA / <br/> PRESENTACIÓN" FieldName="FECHA ENTRADA / PRESENTACION" ReadOnly="True" VisibleIndex="14" Width="145px"  >
                                                            <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy"></PropertiesDateEdit>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <SettingsHeaderFilter Mode="CheckedList" />
                                                            
                                                        </dx:GridViewDataDateColumn>                                                                                            
                                                    </Columns>
                                                    <Toolbars>
                                                        <dx:GridViewToolbar Name="Toolbar1" ItemAlign="Left"  EnableAdaptivity="true">
                                                        <Items>
                                                            <dx:GridViewToolbarItem Name="Links">
                                                            <Template>                                
                                                                <%--<dx:BootstrapButton ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_OnClick" 
                                                                    SettingsBootstrap-RenderOption="Primary" AutoPostBack="false" SettingsBootstrap-Sizing="Small" >
                                                                    <ClientSideEvents Click="function(s, e) { Callback.PerformCallback(); LoadingPanel1.Show(); }" 
                                                                                      Init="function(s, e) {LoadingPanel1.Hide();}" />
                                                                    <CssClasses Icon="glyphicon glyphicon-search" /> 
                                                                </dx:BootstrapButton>--%>
                                                                <table border="0" style="width: 273px;">
                                                                    <tr>
                                                                        <td style="width:113px; text-align:left">
                                                                            <div style="position: relative; width: 50%; float: left;" title="" data-toggle="tooltip">
                                                                                <div class="input-group">
                                                                                    <dx:BootstrapButton ID="lkb_LimpiarFiltros" runat="server" Text="Limpiar Filtros" OnClick="lkb_LimpiarFiltros_Click" 
                                                                                        SettingsBootstrap-RenderOption="Primary" AutoPostBack="false" SettingsBootstrap-Sizing="Small" CssClasses-Text="txt-sm" >
                                                                                        <ClientSideEvents Click="function(s, e) { Callback.PerformCallback(); LoadingPanel1.Show(); }" 
                                                                                                          Init="function(s, e) {LoadingPanel1.Hide();}" />
                                                                                        <CssClasses Icon="glyphicon glyphicon-erase" /> 
                                                                                    </dx:BootstrapButton>
                                                                                </div>
                                                                            </div>
                                                                        </td>
                                                                        <td style="width:70px; text-align:left">
                                                                            <div style="position: relative; width: 50%; float: left;" title="" data-toggle="tooltip">
                                                                                <div class="input-group">
                                                                                    <asp:LinkButton ID="lkb_Excel" runat="server" OnClick="lkb_Excel_Click" CssClass="btn btn-primary btn-sm text-right txt-sm" ToolTip="">
                                                                                        <span class="glyphicon glyphicon-list"></span>&nbsp;&nbsp;Excel
                                                                                    </asp:LinkButton>
                                                                                </div>
                                                                            </div>
                                                                        </td>
                                                                        <td style="width:90px; text-align:left">
                                                                            <div style="position: relative; width: 50%; float: left;" title="" data-toggle="tooltip">
                                                                                <div class="input-group">
                                                                                    <dx:BootstrapButton ID="lkb_Actualizar" runat="server" Text="Actualizar" OnClick="lkb_Actualizar_Click" 
                                                                                        SettingsBootstrap-RenderOption="Primary" AutoPostBack="false" SettingsBootstrap-Sizing="Small" CssClasses-Text="txt-sm">
                                                                                        <ClientSideEvents Click="function(s, e) { Callback.PerformCallback(); LoadingPanel1.Show(); }" 
                                                                                                          Init="function(s, e) {LoadingPanel1.Hide();}" />
                                                                                        <CssClasses Icon="glyphicon glyphicon-refresh" /> 
                                                                                    </dx:BootstrapButton>
                                                                                </div>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>                                                              
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
                                                        <DetailRow>
                                                            <div style="margin-left:-25px; margin-right:5px; margin-top:-5px">
                                                            <div style="padding: 5px 5px 5px 30px">
                                                                <table style="width:100%">
                                                                    <tr>
                                                                        <td>                                                                                      
                                                                            <dx:ASPxLabel runat="server" Text='<%# "Pedimento: " + Eval("PEDIMENTOARMADO") %>' Font-Size="11px" />
                                                                        </td>
                                                                        <td>
                                                                            <%--Este botón redirecciona a una función que abre un método en el code behind
                                                                            <button id="BtnCerrar" runat="server" type="button" class="close" aria-label="Close" title="Cerrar Partida" onclick="BtnCerrarPartidas()" data-toggle="tooltip"  >
                                                                              <span aria-hidden="true">&times;</span>
                                                                            </button>
                                                                            <asp:Button ID="btnPartidasCerrar" runat="server" Text="" OnClick="btnPartidasCerrar_Click" CssClass="no-display" />
                                                                            <dx:ASPxLoadingPanel ID="loadingPanel" runat="server" ClientInstanceName="loadingPanel" Modal="True" ViewStateMode="Enabled" Theme="MaterialCompact"></dx:ASPxLoadingPanel>
                                                                            --%>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                <dx:ASPxPageControl runat="server" ID="ASPxPageControlPartidas" Height="60px" Width="100%" EnableCallBacks="true"  ContentStyle-Border-BorderWidth="3px"
                                                                    TabAlign="Justify" ActiveTabIndex="0" EnableTabScrolling="true" Theme="SoftOrange" Font-Size="12px" ContentStyle-VerticalAlign="Top">
                                                                    <TabStyle Paddings-PaddingLeft="50px" Paddings-PaddingRight="50px"  />                                                                                      
                                                                    <ContentStyle>
                                                                        <Paddings PaddingLeft="20px" PaddingRight="20px" PaddingTop="5px" />
                                                                    </ContentStyle>
                                                                    <TabPages>                                                                            
                                                                        <dx:TabPage Text="Partidas" >
                                                                            <ContentCollection>
                                                                                <dx:ContentControl ID="ContentControl18" runat="server">                                        
                                                                                    <div class="row">
                                                                                        <dx:ASPxGridView ID="GridDePartidas" runat="server" EnableTheming="True" Theme="SoftOrange" Width="100%" KeyFieldName="PARTIDA"
                                                                                            EnableCallBacks="false" AutoGenerateColumns="False" Settings-HorizontalScrollBarMode="Auto" Styles-DetailCell-Paddings-Paddingtop="3px" 
                                                                                            Styles-Header-Font-Size="11px" OnInit="GridDePartidas_Init" OnCustomButtonCallback="GridDePartidas_CustomButtonCallback" >
                                                                                            <SettingsResizing ColumnResizeMode="Control" />                                                                                                                      
                                                                                            <Styles>                                                               
                                                                                                <SelectedRow BackColor="#9E9E9E" ForeColor="#FFFFFF" />
                                                                                                <Row Font-Size="11px"  />
                                                                                                <AlternatingRow Enabled="True" />
                                                                                                <PagerTopPanel Paddings-PaddingBottom="3px">
                                                                                                    <Paddings PaddingBottom="3px" />
                                                                                                </PagerTopPanel>
                                                                                            </Styles>
                                                                                            <SettingsBehavior AllowSelectByRowClick="true" AllowSelectSingleRowOnly="true" />
                                                                                            <SettingsPager Position="TopAndBottom" PageSize="10">
                                                                                            </SettingsPager>
                                                                                            <%--<ClientSideEvents RowClick="OnRowClick" DetailRowExpanding="OnDetailRowExpanding"
                                                                                                DetailRowCollapsing="OnDetailRowCollapsing" />--%>
                                                                                            <SettingsDetail ShowDetailRow="true" AllowOnlyOneMasterRowExpanded="true" ShowDetailButtons="true" />                                                                       
                                                                                            <%--<SettingsPopup>
                                                                                                <HeaderFilter Height="200px" Width="195px"/>
                                                                                            </SettingsPopup>--%> 
                                                                                            <Columns>
                                                                                                <%--<dx:GridViewDataColumn VisibleIndex="0" Width="70px">
                                                                                                    <EditFormSettings Visible="False" />
                                                                                                    <DataItemTemplate>
                                                                                                        <dx:ASPxButton ID="ASPxButtonDetailPartidas" runat="server" AutoPostBack="false" OnInit="ASPxButtonDetailPartidas_Init" 
                                                                                                           EnableTheming="false" RenderMode="Link" VerticalAlign="Top"> 
                                                                                                        </dx:ASPxButton>
                                                                                                    </DataItemTemplate>
                                                                                                    <CellStyle HorizontalAlign="Center" />
                                                                                                </dx:GridViewDataColumn>--%>
                                                                                                <dx:GridViewCommandColumn Caption="ACCIONES" VisibleIndex="0"  Width="70px" AdaptivePriority="1">
                                                                                                    <CustomButtons>
                                                                                                        <dx:GridViewCommandColumnCustomButton ID="btnSeleccionarPartidas" Text="Ver Detalle" 
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
                                                                                                </dx:GridViewCommandColumn>
                                                                                                <dx:GridViewDataTextColumn Caption="PARTIDA" FieldName="PARTIDA" ReadOnly="True" VisibleIndex="1" Width="70px"  >
                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                    <CellStyle HorizontalAlign ="Center" />
                                                                                                    <SettingsHeaderFilter Mode="CheckedList" />
                                                                                                </dx:GridViewDataTextColumn>
                                                                                                <dx:GridViewDataTextColumn Caption="FRACCIÓN ARANCELARIA" FieldName="FRACCION ARANCELARIA" ReadOnly="True" VisibleIndex="2" Width="155px" >
                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                    <SettingsHeaderFilter Mode="CheckedList" />
                                                                                                </dx:GridViewDataTextColumn>
                                                                                                <dx:GridViewDataTextColumn Caption="SUBDIVISIÓN FRACCIÓN" FieldName="SUBDIVISION FRACCION" ReadOnly="True" VisibleIndex="3" Width="170px" >
                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                    <SettingsHeaderFilter Mode="CheckedList" />
                                                                                                </dx:GridViewDataTextColumn>
                                                                                                <dx:GridViewDataTextColumn Caption="DESCRIPCIÓN MERCANCIA" FieldName="DESCRIPCION MERCANCIA" ReadOnly="True" VisibleIndex="4" Width="600px" >
                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                    <SettingsHeaderFilter Mode="CheckedList" />
                                                                                                </dx:GridViewDataTextColumn>
                                                                                                <dx:GridViewDataTextColumn Caption="UNIDAD TARIFA" FieldName="UNIDAD TARIFA" ReadOnly="True" VisibleIndex="5" Width="110px" >
                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                    <SettingsHeaderFilter Mode="CheckedList" />
                                                                                                </dx:GridViewDataTextColumn>
                                                                                                <dx:GridViewDataTextColumn Caption="UNIDAD TARIFA DESCRIPCIÓN" FieldName="UNIDAD TARIFA DESCRIPCION" ReadOnly="True" VisibleIndex="6" Width="180px">
                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                    <SettingsHeaderFilter Mode="CheckedList" />
                                                                                                </dx:GridViewDataTextColumn>
                                                                                                <dx:GridViewDataTextColumn Caption="CANTIDAD TARIFA" FieldName="CANTIDAD TARIFA" ReadOnly="True" VisibleIndex="7" Width="120px" >
                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                    <CellStyle HorizontalAlign ="Right" />
                                                                                                    <SettingsHeaderFilter Mode="CheckedList" />
                                                                                                    <PropertiesTextEdit DisplayFormatString="{0:n5}" />
                                                                                                </dx:GridViewDataTextColumn>
                                                                                                <dx:GridViewDataTextColumn Caption="UNIDAD COMERCIAL" FieldName="UNIDAD COMERCIAL" ReadOnly="True" VisibleIndex="8" Width="120px" >
                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                    <SettingsHeaderFilter Mode="CheckedList" />
                                                                                                </dx:GridViewDataTextColumn>
                                                                                                <dx:GridViewDataTextColumn Caption="UNIDAD COMERCIAL DESCRIPCIÓN" FieldName="UNIDAD COMERCIAL DESCRIPCION" ReadOnly="True" VisibleIndex="9" Width="200px" >
                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                    <SettingsHeaderFilter Mode="CheckedList" />
                                                                                                </dx:GridViewDataTextColumn>
                                                                                                <dx:GridViewDataTextColumn Caption="CANTIDAD COMERCIAL" FieldName="CANTIDAD COMERCIAL" ReadOnly="True" VisibleIndex="10" Width="130px">
                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                    <CellStyle HorizontalAlign ="Right" />
                                                                                                    <SettingsHeaderFilter Mode="CheckedList" />
                                                                                                    <PropertiesTextEdit DisplayFormatString="{0:n3}" />
                                                                                                </dx:GridViewDataTextColumn>
                                                                                                <dx:GridViewDataTextColumn Caption="PRECIO UNITARIO" FieldName="PRECIO UNITARIO" ReadOnly="True" VisibleIndex="11" Width="120px" >
                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                    <CellStyle HorizontalAlign ="Right" />
                                                                                                    <SettingsHeaderFilter Mode="CheckedList" />
                                                                                                    <PropertiesTextEdit DisplayFormatString="{0:n5}" />
                                                                                                </dx:GridViewDataTextColumn>
                                                                                                <dx:GridViewDataTextColumn Caption="VALOR COMERCIAL" FieldName="VALOR COMERCIAL" ReadOnly="True" VisibleIndex="12" Width="120px" >
                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                    <CellStyle HorizontalAlign ="Right" />
                                                                                                    <SettingsHeaderFilter Mode="CheckedList" />
                                                                                                    <PropertiesTextEdit DisplayFormatString="{0:n2}" />
                                                                                                </dx:GridViewDataTextColumn>
                                                                                                <dx:GridViewDataTextColumn Caption="VALOR ADUANA" FieldName="VALOR ADUANA" ReadOnly="True" VisibleIndex="13" Width="120px" >
                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                    <CellStyle HorizontalAlign ="Right" />
                                                                                                    <SettingsHeaderFilter Mode="CheckedList" />
                                                                                                    <PropertiesTextEdit DisplayFormatString="{0:n2}" />
                                                                                                </dx:GridViewDataTextColumn>
                                                                                                <dx:GridViewDataTextColumn Caption="VALOR DOLARES" FieldName="VALOR DOLARES" ReadOnly="True" VisibleIndex="14" Width="120px" >
                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                    <CellStyle HorizontalAlign ="Right" />
                                                                                                    <SettingsHeaderFilter Mode="CheckedList" />
                                                                                                    <PropertiesTextEdit DisplayFormatString="{0:n2}" />
                                                                                                </dx:GridViewDataTextColumn>
                                                                                                <dx:GridViewDataTextColumn Caption="VALOR AGREGADO" FieldName="VALOR AGREGADO" ReadOnly="True" VisibleIndex="15" Width="120px" >
                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                    <CellStyle HorizontalAlign ="Right" />
                                                                                                    <SettingsHeaderFilter Mode="CheckedList" />
                                                                                                    <PropertiesTextEdit DisplayFormatString="{0:n2}" />
                                                                                                </dx:GridViewDataTextColumn>
                                                                                                <dx:GridViewDataTextColumn Caption="CÓDIGO PRODUCTO" FieldName="CODIGO PRODUCTO" ReadOnly="True" VisibleIndex="16" Width="120px" >
                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                    <SettingsHeaderFilter Mode="CheckedList" />
                                                                                                </dx:GridViewDataTextColumn>
                                                                                                <dx:GridViewDataTextColumn Caption="MARCA" FieldName="MARCA" ReadOnly="True" VisibleIndex="17" Width="260px">
                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                    <SettingsHeaderFilter Mode="CheckedList" />
                                                                                                </dx:GridViewDataTextColumn>
                                                                                                <dx:GridViewDataTextColumn Caption="MODELO" FieldName="MODELO" ReadOnly="True" VisibleIndex="18" Width="230px">
                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                    <SettingsHeaderFilter Mode="CheckedList" />
                                                                                                </dx:GridViewDataTextColumn>
                                                                                                <dx:GridViewDataTextColumn Caption="MÉTODO DE VALORACIÓN" FieldName="METODO DE VALORACION" ReadOnly="True" VisibleIndex="19" Width="280px" >
                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                    <SettingsHeaderFilter Mode="CheckedList" />
                                                                                                </dx:GridViewDataTextColumn>
                                                                                                <dx:GridViewDataTextColumn Caption="VINCULACIÓN" FieldName="VINCULACION" ReadOnly="True" VisibleIndex="20" Width="320px" >
                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                    <SettingsHeaderFilter Mode="CheckedList" />
                                                                                                </dx:GridViewDataTextColumn>
                                                                                                <dx:GridViewDataTextColumn Caption="PAÍS ORIGEN DESTINO" FieldName="PAIS ORIGEN DESTINO" ReadOnly="True" VisibleIndex="21" Width="140px">
                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                    <SettingsHeaderFilter Mode="CheckedList" />
                                                                                                </dx:GridViewDataTextColumn>
                                                                                                <dx:GridViewDataTextColumn Caption="PAÍS ORIGEN DESTINO NOMBRE" FieldName="PAIS ORIGEN DESTINO NOMBRE" ReadOnly="True" VisibleIndex="22" Width="270px" >
                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                    <SettingsHeaderFilter Mode="CheckedList" />
                                                                                                </dx:GridViewDataTextColumn>
                                                                                                <dx:GridViewDataTextColumn Caption="PAÍS COMPRADOR VENDEDOR" FieldName="PAIS COMPRADOR VENDEDOR" ReadOnly="True" VisibleIndex="23" Width="170px" >
                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                    <SettingsHeaderFilter Mode="CheckedList" />
                                                                                                </dx:GridViewDataTextColumn>
                                                                                                <dx:GridViewDataTextColumn Caption="PAÍS COMPRADOR VENDEDOR NOMBRE" FieldName="PAIS COMPRADOR VENDEDOR NOMBRE" ReadOnly="True" VisibleIndex="24" Width="270px">
                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                    <SettingsHeaderFilter Mode="CheckedList" />
                                                                                                </dx:GridViewDataTextColumn>
                                                                                            </Columns>
                                                                                            <%--<Toolbars>
                                                                                                <dx:GridViewToolbar Name="Toolbar1" ItemAlign="Left"  EnableAdaptivity="true">
                                                                                                    <Items>
                                                                                                        <dx:GridViewToolbarItem Name="Links">
                                                                                                         <Template>                                                        
                                                                                                             <asp:LinkButton ID="lkb_Excel_Partidas" runat="server" OnClick="lkb_Excel_Click" CssClass="btn btn-primary btn-sm" ToolTip="Exportar a Excel">
                                                                                                                 <span class="glyphicon glyphicon-list"></span>&nbsp;&nbsp;Excel
                                                                                                             </asp:LinkButton>
                                                                                                             <asp:LinkButton ID="lkb_Actualizar" runat="server" OnClick="lkb_Actualizar_Click" CssClass="btn btn-primary btn-sm" ToolTip="Actualizar">
                                                                                                                <span class="glyphicon glyphicon-refresh"></span>&nbsp;&nbsp;Actualizar
                                                                                                             </asp:LinkButton>
                                                                                                             <asp:LinkButton ID="lkb_LimpiarFiltros" runat="server" OnClick="lkb_LimpiarFiltros_Click" CssClass="btn btn-primary btn-sm" ToolTip="Limpiar Filtros">
                                                                                                                <span class="glyphicon glyphicon-erase"></span>&nbsp;&nbsp;Limpiar Filtros
                                                                                                             </asp:LinkButton>
                                                                                                         </Template>
                                                                                                        </dx:GridViewToolbarItem>            
                                                                                                    </Items>
                                                                                                </dx:GridViewToolbar>
                                                                                            </Toolbars>--%>
                                                                                            <Templates>
                                                                                                <DetailRow>
                                                                                                    <div  style="padding: 0px">
                                                                                                        <dx:ASPxGridView ID="GridPartidasDetail" runat="server" KeyFieldName="PARTIDA" Theme="SoftOrange" Styles-Header-Font-Size="11px" 
                                                                                                            Width="1020px" OnInit="GridPartidasDetail_Init">
                                                                                                            <Columns>                                                                                                    
                                                                                                                <dx:GridViewDataTextColumn Caption="CLAVE IDENTIFICADOR" FieldName="CLAVE IDENTIFICADOR" ReadOnly="True" VisibleIndex="1" Width="130px" >
                                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                                </dx:GridViewDataTextColumn>
                                                                                                                <dx:GridViewDataTextColumn Caption="CLAVE" FieldName="CLAVE" ReadOnly="True" VisibleIndex="2" Width="50px">
                                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                                </dx:GridViewDataTextColumn>
                                                                                                                <dx:GridViewDataTextColumn Caption="DESCRIPCIÓN" FieldName="DESCRIPCION" ReadOnly="True" VisibleIndex="3" Width="540px">
                                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                                </dx:GridViewDataTextColumn>
                                                                                                                <dx:GridViewDataTextColumn Caption="COMPLEMENTO 1" FieldName="COMPLEMENTO 1" ReadOnly="True" VisibleIndex="4" Width="100px">
                                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                                </dx:GridViewDataTextColumn>
                                                                                                                <dx:GridViewDataTextColumn Caption="COMPLEMENTO 2" FieldName="COMPLEMENTO 2" ReadOnly="True" VisibleIndex="5" Width="100px">
                                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                                </dx:GridViewDataTextColumn>
                                                                                                                <dx:GridViewDataTextColumn Caption="COMPLEMENTO 3" FieldName="COMPLEMENTO 3" ReadOnly="True" VisibleIndex="6" Width="100px">
                                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                                </dx:GridViewDataTextColumn>                                                                                                
                                                                                                            </Columns>
                                                                                                            <SettingsBehavior AllowSelectByRowClick="true" AllowSelectSingleRowOnly="true" />                                                                                                
                                                                                                            <Styles>                                                               
                                                                                                            <SelectedRow BackColor="#9E9E9E" ForeColor="#FFFFFF" />
                                                                                                            <Row Font-Size="11px" />
                                                                                                            <AlternatingRow Enabled="True" />
                                                                                                            <PagerTopPanel Paddings-PaddingBottom="3px">
                                                                                                                <Paddings PaddingBottom="3px" />
                                                                                                                </PagerTopPanel>
                                                                                                            </Styles>
                                                                                                            <SettingsPager Position="TopAndBottom" PageSize="10">
                                                                                                            </SettingsPager>                                                                                               
                                                                                                        </dx:ASPxGridView>                                                                                                                    
                                                                                                    </div>                                                                                                
                                                                                                </DetailRow>
                                                                                                <%--<PagerBar>
                                                                                                    <table runat="server" class="OptionsTable" style="width: 100%">
                                                                                                        <tr>
                                                                                                            <td style="padding-left: 8px;">
                                                                                                                <dx:ASPxButton runat="server" ID="FirstButton" Text="<<" Enabled="<%# Container.Grid.PageIndex > 0 %>"
                                                                                                                    AutoPostBack="false" UseSubmitBehavior="false" Font-Size="12px" Width="40px" CssClass="bordes_curvos">
                                                                                                                    <ClientSideEvents Click="function() { grid.GotoPage(0) }" />
                                                                                                                </dx:ASPxButton>
                                                                                                            </td>
                                                                                                            
                                                                                                            <td style="padding-left: 5px;">
                                                                                                                <dx:ASPxButton runat="server" ID="PrevButton" Text="<" Enabled="<%# Container.Grid.PageIndex > 0 %>"
                                                                                                                    AutoPostBack="false" UseSubmitBehavior="false" Font-Size="12px" Width="40px" CssClass="bordes_curvos">
                                                                                                                    <ClientSideEvents Click="function() { grid.PrevPage() }" />
                                                                                                                </dx:ASPxButton>
                                                                                                            </td>
                                                                                                            <td style="padding-left: 5px;">
                                                                                                                <dx:ASPxTextBox runat="server" ID="GotoBox" Width="30">
                                                                                                                    <ClientSideEvents Init="CustomPager.gotoBox_Init" ValueChanged="CustomPager.gotoBox_ValueChanged"
                                                                                                                        KeyPress="CustomPager.gotoBox_KeyPress" />
                                                                                                                </dx:ASPxTextBox>
                                                                                                            </td>
                                                                                                            <td style="padding-left: 5px;">
                                                                                                                <span style="white-space: nowrap">
                                                                                                                    de <%# Container.Grid.PageCount %>
                                                                                                                </span>
                                                                                                            </td>
                                                                                                            <td style="padding-left: 5px;">
                                                                                                                <dx:ASPxButton runat="server" ID="NextButton" Text=">" Enabled="<%# Container.Grid.PageIndex < Container.Grid.PageCount - 1 %>"
                                                                                                                    AutoPostBack="false" UseSubmitBehavior="false" Font-Size="12px" Width="40px" CssClass="bordes_curvos">
                                                                                                                    <ClientSideEvents Click="function() { grid.NextPage() }" />
                                                                                                                </dx:ASPxButton>
                                                                                                            </td>
                                                                                                            <td style="padding-left: 5px;">
                                                                                                                <dx:ASPxButton runat="server" ID="LastButton" Text=">>" Enabled="<%# Container.Grid.PageIndex < Container.Grid.PageCount - 1 %>"
                                                                                                                    AutoPostBack="false" UseSubmitBehavior="false" Font-Size="12px" Width="40px" CssClass="bordes_curvos">
                                                                                                                    <ClientSideEvents Click="function() { grid.GotoPage(grid.GetPageCount() - 1); }" />
                                                                                                                </dx:ASPxButton>
                                                                                                            </td>
                                                                                                            <td style="width: 100%"></td>
                                                                                                            <td style="white-space: nowrap">
                                                                                                                <span style="white-space: nowrap">
                                                                                                                    Filas:
                                                                                                                </span>
                                                                                                            </td>
                                                                                                            <td style="padding-right: 5px;">
                                                                                                                <dx:ASPxComboBox runat="server" ID="Combo" Width="70px" DropDownWidth="70px" ValueType="System.Int32"
                                                                                                                    OnLoad="PagerCombo_LoadPARTIDAS">
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
                                                                                                </PagerBar>--%>
                                                                                            </Templates>                                                                                               
                                                                                        </dx:ASPxGridView>   
                                                                                    </div>                                        
                                                                             </dx:ContentControl>
                                                                         </ContentCollection>
                                                                     </dx:TabPage>                                 
                                                                 </TabPages>
                                                                </dx:ASPxPageControl>
                                                            </div>
                                                            </div>
                                                        </DetailRow>
                                                        <PagerBar>                                                                        
                                                            <table runat="server" style="width: 100%">
                                                            <tr>
                                                                <td style="padding-left: 2px;">
                                                                    <dx:BootstrapButton ID="btnInicial" runat="server" Text="" AutoPostBack="false" UseSubmitBehavior="false"                                                                                
                                                                        SettingsBootstrap-RenderOption="Primary"  CssClasses-Control="btn btn-primary btn-xs" Enabled="<%# Container.Grid.PageIndex > 0 %>">
                                                                        <ClientSideEvents Click="function() { grid.GotoPage(0) }" />
                                                                        <CssClasses Icon="glyphicon glyphicon-backward" /> 
                                                                    </dx:BootstrapButton>
                                                                    <%--<dx:ASPxButton runat="server" ID="FirstButton" Text="<<" Enabled="<%# Container.Grid.PageIndex > 0 %>" 
                                                                        AutoPostBack="false" UseSubmitBehavior="false" Font-Size="12px" Width="40px" CssClass="bordes_curvos">
                                                                        <ClientSideEvents Click="function() { grid.GotoPage(0) }" />
                                                                    </dx:ASPxButton>--%>
                                                                </td>
                                                                
                                                                <td style="padding-left: 5px;">
                                                                    <dx:BootstrapButton ID="btnAtras" runat="server" Text="" AutoPostBack="false" UseSubmitBehavior="false"                                                                                
                                                                        SettingsBootstrap-RenderOption="Primary"  CssClasses-Control="btn btn-primary btn-xs" Enabled="<%# Container.Grid.PageIndex > 0 %>">
                                                                        <ClientSideEvents Click="function() { grid.PrevPage() }" />
                                                                        <CssClasses Icon="glyphicon glyphicon-triangle-left" /> 
                                                                    </dx:BootstrapButton>
                                                                    <%--<dx:ASPxButton runat="server" ID="PrevButton" Text="<" Enabled="<%# Container.Grid.PageIndex > 0 %>"
                                                                        AutoPostBack="false" UseSubmitBehavior="false" Font-Size="12px" Width="40px" CssClass="bordes_curvos">
                                                                        <ClientSideEvents Click="function() { grid.PrevPage() }" />
                                                                    </dx:ASPxButton>--%>
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
                                                                <td style="padding-left: 2px">
                                                                    <dx:BootstrapButton ID="btnSiuiente" runat="server" Text="" AutoPostBack="false" UseSubmitBehavior="false"                                                                                
                                                                        SettingsBootstrap-RenderOption="Primary"  CssClasses-Control="btn btn-primary btn-xs" Enabled="<%# Container.Grid.PageIndex < Container.Grid.PageCount - 1 %>">
                                                                        <ClientSideEvents Click="function() { grid.NextPage() }" />
                                                                        <CssClasses Icon="glyphicon glyphicon-triangle-right" /> 
                                                                    </dx:BootstrapButton>
                                                                    <%--<dx:ASPxButton runat="server" ID="NextButton" Text=">" Enabled="<%# Container.Grid.PageIndex < Container.Grid.PageCount - 1 %>"
                                                                        AutoPostBack="false" UseSubmitBehavior="false" Font-Size="12px" Width="40px" CssClass="bordes_curvos">
                                                                        <ClientSideEvents Click="function() { grid.NextPage() }" />
                                                                    </dx:ASPxButton>--%>
                                                                </td>
                                                                <td style="padding-left: 5px;">
                                                                    <dx:BootstrapButton ID="btnFinal" runat="server" Text="" AutoPostBack="false" UseSubmitBehavior="false"                                                                                
                                                                        SettingsBootstrap-RenderOption="Primary"  CssClasses-Control="btn btn-primary btn-xs" Enabled="<%# Container.Grid.PageIndex < Container.Grid.PageCount - 1 %>">
                                                                        <ClientSideEvents Click="function() { grid.GotoPage(grid.GetPageCount() - 1);}" />
                                                                        <CssClasses Icon="glyphicon glyphicon-forward" /> 
                                                                    </dx:BootstrapButton>
                                                                    <%--<dx:ASPxButton runat="server" ID="LastButton" Text=">>" Enabled="<%# Container.Grid.PageIndex < Container.Grid.PageCount - 1 %>"
                                                                        AutoPostBack="false" UseSubmitBehavior="false" Font-Size="12px" Width="40px" CssClass="bordes_curvos">
                                                                        <ClientSideEvents Click="function() { grid.GotoPage(grid.GetPageCount() - 1); }" />
                                                                    </dx:ASPxButton>--%>
                                                                </td>
                                                                <td style="width: 100%; padding-left:5px;font-size:11px">
                                                                    <span style="white-space: nowrap">
                                                                        (<%# Container.Grid.VisibleRowCount %>)
                                                                    </span>
                                                                </td>
                                                                <td style="padding-left: 5px; white-space: nowrap;font-size:11px">
                                                                    <span style="white-space: nowrap">
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
                                                <dx:ASPxGlobalEvents ID="ge" runat="server">
                                                    <ClientSideEvents ControlsInitialized="OnControlsInitialized" />
                                                </dx:ASPxGlobalEvents>                                                                                                            
                                                <dx:ASPxGridViewExporter ID="Exporter" GridViewID="Grid" runat="server" PaperKind="A5" Landscape="true" />
                                                <dx:ASPxCallback ID="ASPxCallback1" runat="server" ClientInstanceName="Callback" >
                                                    <ClientSideEvents CallbackComplete="function(s, e) { System.Threading.Thread.Sleep(3000); LoadingPanel1.Hide(); }" />
                                                </dx:ASPxCallback>
                                                <dx:ASPxLoadingPanel ID="LoadingPanel1" runat="server" ClientInstanceName="LoadingPanel1"
                                                    Modal="True" ViewStateMode="Enabled" Theme="MaterialCompact" >
                                                </dx:ASPxLoadingPanel>
                                                
                                                <button id="btnModalCG" type="button" data-toggle="modal" data-target="#ModalCG" data-whatever="Nuevo" onclick="return false;" style="display: none;"></button>
                                                <button id="btnModalED" type="button" data-toggle="modal" data-target="#ModalED" data-whatever="Nuevo" onclick="return false;" style="display: none;"></button>
                                                <button id="btnModalEncabezado" type="button" data-toggle="modal" data-target="#ModalEncabezado" data-whatever="Nuevo" onclick="return false;" style="display: none;"></button>
                                                <button id="btnPartidasM" type="button" data-toggle="modal" data-target="#ModalPartidaDetalle" data-whatever="Nuevo" onclick="return false;" style="display: none;"></button>
                                                <%--<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:BD %>" SelectCommand="SICEWEB_ENCABEZADO" 
                                                                    SelectCommandType="StoredProcedure" ValidateRequestMode="Enabled" ViewStateMode="Enabled" CancelSelectOnNullParameter="False" EnableCaching="true">
                                                                       <SelectParameters>
                                                                       <asp:ControlParameter ControlID="BootstrapFormLayout2$DESDE" Name="DESDE" PropertyName="Value" Type="DateTime" DefaultValue="01/01/1900" />
                                                                       <asp:ControlParameter ControlID="BootstrapFormLayout2$HASTA" Name="HASTA" PropertyName="Value" Type="DateTime" DefaultValue="01/01/1900" />
                                                                       <asp:ControlParameter ControlID="BootstrapFormLayout1$PEDIMENTO" Name="PEDIMENTO" PropertyName="Text" Type="String" DefaultValue="" />
                                                                       <asp:ControlParameter ControlID="BootstrapFormLayout1$CLAVE" Name="CLAVE_PEDIMENTO" PropertyName="Text" Type="String" DefaultValue="" />
                                                                       <asp:ControlParameter ControlID="BootstrapFormLayout1$ADUANA" Name="ADUANA" PropertyName="Text" Type="String" DefaultValue="" />
                                                                       <asp:ControlParameter ControlID="BootstrapFormLayout1$TEXTO" Name="TEXTO" PropertyName="Text" Type="String" DefaultValue="" />
                                                                       <asp:ControlParameter ControlID="BootstrapFormLayout1$COMODINES" Name="COMODIN" PropertyName="Text" Type="String" DefaultValue="" />
                                                                   </SelectParameters>
                                                                </asp:SqlDataSource>  --%>
                                            </div>
                                            
                                        </dx:SplitterContentControl>
                                    </ContentCollection>
                                   </dx:SplitterPane>                                            
                                </Panes>
                                </dx:ASPxSplitter>
                                </div>
                                <dx:ASPxGlobalEvents ID="geSplitter" runat="server">
                                    <ClientSideEvents ControlsInitialized="OnControlsInitializedSplitter" />
                                </dx:ASPxGlobalEvents>
                            </div>      
                        </asp:Panel>                                               
                        <dx:ASPxLabel ID="ASPx_Fecha_Desde" runat="server" Visible="false"></dx:ASPxLabel>
                        <dx:ASPxLabel ID="ASPx_Fecha_Hasta" runat="server" Visible="false"></dx:ASPxLabel>
                        <dx:ASPxLabel ID="ASPx_Pedimento" runat="server" Visible="false"></dx:ASPxLabel>
                        <dx:ASPxLabel ID="lblCadena" runat="server" Visible="false"/>
                        <dx:ASPxLabel ID="lblIdUsuario" runat="server" Visible="false"/>                               
                    </div>
                </div>
            </div>
        </asp:View>
        <asp:View ID="View2" runat="server">
            <div class="container-fluid">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <table style="width: 100%">
                            <tr>
                                <td style="text-align: left; width: 50%">
                                    <h1 id="h2_titulo" runat="server" class="panel-title small"></h1>
                                </td>
                                <td style="text-align: right; width: 50%">
                                    <asp:LinkButton ID="LinkButton1" runat="server" OnClick="lkb_Regresar_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button">
                                         <span class="glyphicon glyphicon-circle-arrow-left"></span>&nbsp;&nbsp;Regresar
                                    </asp:LinkButton>
                                </td>
                            </tr>
                        </table>                        
                    </div>
                    <div class="panel-body">
                        <asp:Panel ID="Panel2" runat="server">                                                       
                            <table style="width:100%;">
                                <tr>
                                    <td style="text-align:left; width:50%">
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <dx:ASPxLabel ID="Label1" runat="server" Text="Pedimento:" Font-Size="12px"/>&nbsp;
                                        <dx:ASPxLabel ID="lblPedimento" runat="server" Text="" Font-Size="12px"/>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:LinkButton ID="lkb_LimpiarFiltros" runat="server" OnClick="lkb_LimpiarFiltrosNP_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button">
                                          <%--<span class="glyphicon glyphicon-plus"></span>&nbsp;&nbsp;--%>Limpiar Filtros
                                        </asp:LinkButton>
                                        <%--<dx:BootstrapButton ID="lkb_LimpiarFiltros" runat="server" Text="Limpiar Filtros" OnClick="lkb_LimpiarFiltrosNP_Click" 
                                            SettingsBootstrap-RenderOption="Primary" AutoPostBack="false" SettingsBootstrap-Sizing="Small" CssClasses-Text="txt-sm" >
                                            <ClientSideEvents Click="function(s, e) { Callback3.PerformCallback(); LoadingPanel3.Show(); }" 
                                                              Init="function(s, e) {LoadingPanel3.Hide();}" />
                                            <CssClasses Icon="glyphicon glyphicon-erase" /> 
                                        </dx:BootstrapButton>--%>
                                    </td>
                                    <td style="text-align:right; width:28%">

                                    </td>
                                    <td style="text-align:right; width:5%;">
                                        <asp:LinkButton ID="lkb_Validar" runat="server" OnClick="lkb_Validar_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button">
                                          <%--<span class="glyphicon glyphicon-plus"></span>&nbsp;&nbsp;--%>Validar
                                        </asp:LinkButton>                                                          
                                        <%--<dx:BootstrapButton ID="bbValidar" runat="server" AutoPostBack="false" OnClick="bbValidar_Click"
                                            SettingsBootstrap-RenderOption="Primary" Text="Validar" CssClasses-Text="txt-sm">
                                            <SettingsBootstrap RenderOption="Primary" Sizing="Small" />
                                            <ClientSideEvents Click="function(s, e) { Callback3.PerformCallback(); LoadingPanel3.Show(); }"
                                                Init="function(s, e) {LoadingPanel3.Hide();}" />
                                        </dx:BootstrapButton>--%>
                                    </td>
                                    <td style="text-align:left; width:5%; padding-left:5px">
                                      <asp:Label ID="lblT_Descarga" runat="server" Font-Size="11px" Text="Descarga:" />
                                    </td>
                                    <td style="text-align:right; width:5%">
                                        <dx:ASPxComboBox ID="xcbDescarga" runat="server" DropDownStyle="DropDown" IncrementalFilteringMode="Contains"
                                            Width="90%" CssClass="bordes_curvos_derecha" ListBoxStyle-CssClass="combobox-width">
                                          <Items>
                                              <dx:ListEditItem Text="SI" Value="SI" Selected="true" />
                                              <dx:ListEditItem Text="NO" Value="NO" />
                                          </Items>                                                             
                                        </dx:ASPxComboBox>
                                    </td>
                                    <td style="text-align:right; width:7%; padding-right:16px">    
                                        <asp:LinkButton ID="lkb_Migrar" runat="server" OnClick="lkb_Migrar_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button">
                                          <%--<span class="glyphicon glyphicon-plus"></span>&nbsp;&nbsp;--%>Migrar A24
                                        </asp:LinkButton>
                                        <%--<dx:BootstrapButton ID="bbMigrar" runat="server" AutoPostBack="false" 
                                            SettingsBootstrap-RenderOption="Primary" Text="Migrar A24" CssClasses-Text="txt-sm">
                                            <SettingsBootstrap RenderOption="Primary" Sizing="Small" />
                                            <ClientSideEvents Click="function(s, e) { Callback3.PerformCallback(); LoadingPanel3.Show(); }" 
                                                Init="function(s, e) {LoadingPanel3.Hide();}" />
                                        </dx:BootstrapButton>--%>
                                    </td>
                                </tr>
                            </table>
                            <dx:ASPxGridView ID="GridNP" runat="server" EnableTheming="True" Theme="SoftOrange"
                              OnCustomCallback="GridNP_CustomCallback" EnableCallBacks="false" ClientInstanceName="gridnp"
                              AutoGenerateColumns="False" Width="100%" Settings-HorizontalScrollBarMode="Auto" KeyFieldName="PARTIDA_DETALLEKEY" SettingsPager-Position="TopAndBottom"
                              Styles-Header-ForeColor="#FFF" Styles-Header-Font-Size="11px" Styles-Cell-Font-Size="11px" Styles-Footer-Font-Size="11px">
                              <SettingsAdaptivity AdaptivityMode="HideDataCellsWindowLimit" HideDataCellsAtWindowInnerWidth="800" 
                                  AdaptiveDetailColumnCount="1" AllowOnlyOneAdaptiveDetailExpanded="True">
                                  <AdaptiveDetailLayoutProperties colcount="2">
                                      <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit"  SwitchToSingleColumnAtWindowInnerWidth="600" />
                                  </AdaptiveDetailLayoutProperties>
                              </SettingsAdaptivity> 
                              <SettingsResizing ColumnResizeMode="Control" />
                               <SettingsBehavior AllowSelectByRowClick="false" />
                              <Settings ShowFooter="true" ShowHeaderFilterButton="true" ShowFilterRowMenu="false"  />                                          
                              <Styles>                                                                                      
                                  <SelectedRow  />
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
                              <TotalSummary>
                                  <dx:ASPxSummaryItem FieldName="PARTIDA" SummaryType="Count" DisplayFormat=" {0:n0}" />
                                  <dx:ASPxSummaryItem FieldName="PARTIDA_DETALLE_VALORCOMERCIAL" SummaryType="Sum" DisplayFormat=" {0:n2}" />
                                  <dx:ASPxSummaryItem FieldName="PARTIDA_DETALLE_VALORDOLARES" SummaryType="Sum" DisplayFormat=" {0:n2}" />
                              </TotalSummary>                                            
                              <Columns>
                                  <dx:GridViewDataTextColumn FieldName="PARTIDA_DETALLEKEY" ReadOnly="True" VisibleIndex="0" Visible="false">
                                  </dx:GridViewDataTextColumn>
                                  <dx:GridViewDataColumn Caption="" VisibleIndex="0" Width="50px" Settings-AllowAutoFilter="False">
                                      <HeaderStyle HorizontalAlign="Center" />
                                      <CellStyle HorizontalAlign="Center"></CellStyle>
                                      <DataItemTemplate>
                                          <dx:ASPxRadioButton ID="rbConsultar" ClientInstanceName="rbConsultar" Enabled="true" runat="server"
                                              Theme="SoftOrange" GroupName="consultar" OnInit="rbConsultar_Init" AutoPostBack="true" OnCheckedChanged="rbConsultar_CheckedChanged">
                                          </dx:ASPxRadioButton>
                                      </DataItemTemplate>
                                      <EditFormCaptionStyle HorizontalAlign="Center" />
                                  </dx:GridViewDataColumn>
                                  <dx:GridViewDataTextColumn Caption="Secuencia" FieldName="PARTIDA" ReadOnly="True" VisibleIndex="1" Width="110px" >
                                      <HeaderStyle HorizontalAlign="Center" ForeColor="#FFF"  />
                                      <SettingsHeaderFilter Mode="CheckedList" />
                                      <CellStyle HorizontalAlign="Center" />
                                      <FooterCellStyle HorizontalAlign="Center" />
                                  </dx:GridViewDataTextColumn>
                                  <dx:GridViewDataTextColumn Caption="Fracción Arancelaria" FieldName="PARTIDA_DETALLE_FRACCIONARANCELARIA" ReadOnly="True" VisibleIndex="2" Width="170px">
                                      <HeaderStyle HorizontalAlign="Center" />
                                      <SettingsHeaderFilter Mode="CheckedList" />
                                      <CellStyle HorizontalAlign="Center" />
                                  </dx:GridViewDataTextColumn>
                                  <dx:GridViewDataTextColumn Caption="Cantidad Comercial" FieldName="PARTIDA_DETALLE_CANTIDADUNIDADMEDIDACOMERCIAL" ReadOnly="True" VisibleIndex="4" Width="170px">
                                      <HeaderStyle HorizontalAlign="Center" />
                                      <SettingsHeaderFilter Mode="CheckedList" />
                                      <CellStyle HorizontalAlign="Right" />
                                      <PropertiesTextEdit DisplayFormatString="{0:n2}" />
                                      <FooterCellStyle HorizontalAlign="Right" />
                                  </dx:GridViewDataTextColumn>
                                  <dx:GridViewDataTextColumn Caption="UMC" FieldName="PARTIDA_DETALLE_UNIDADMEDIDACOMERCIA_CLAVE" ReadOnly="True" VisibleIndex="5" Width="90px">
                                      <HeaderStyle HorizontalAlign="Center" />
                                      <SettingsHeaderFilter Mode="CheckedList" />
                                      <CellStyle HorizontalAlign="Center" />
                                  </dx:GridViewDataTextColumn>
                                  <dx:GridViewDataTextColumn Caption="Descripción" FieldName="PARTIDA_DETALLE_DESCRIPCIONMERCANCIA" ReadOnly="True" VisibleIndex="6" Width="549px">
                                      <HeaderStyle HorizontalAlign="Center" />
                                      <SettingsHeaderFilter Mode="CheckedList" />
                                      <CellStyle HorizontalAlign="Left" />
                                  </dx:GridViewDataTextColumn>
                                  <dx:GridViewDataTextColumn Caption="Valor Dólares" FieldName="PARTIDA_DETALLE_VALORDOLARES" ReadOnly="True" VisibleIndex="9" Width="135px">
                                      <HeaderStyle HorizontalAlign="Center" />
                                      <SettingsHeaderFilter Mode="CheckedList" />
                                      <CellStyle HorizontalAlign="Right" />
                                      <PropertiesTextEdit DisplayFormatString="{0:n2}" />
                                      <FooterCellStyle HorizontalAlign="Right" />
                                  </dx:GridViewDataTextColumn>                       
                              </Columns>
                              <Toolbars>
                                  <dx:GridViewToolbar Name="Toolbar1" EnableAdaptivity="true" ItemAlign="Left">
                                      <Items>
                                          <dx:GridViewToolbarItem Name="Links" ItemStyle-Width="100%">
                                          <Template>
                                              
                                              <%--<dx:BootstrapButton ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_OnClick" 
                                                  SettingsBootstrap-RenderOption="Primary" AutoPostBack="false" SettingsBootstrap-Sizing="Small" >
                                                  <ClientSideEvents Click="function(s, e) { Callback3.PerformCallback(); LoadingPanel3.Show(); }" 
                                                                    Init="function(s, e) {LoadingPanel3.Hide();}" />
                                                  <CssClasses Icon="glyphicon glyphicon-search" /> 
                                              </dx:BootstrapButton>
                                              <asp:LinkButton ID="lkb_Excel" runat="server" OnClick="lkb_Excel_Click" CssClass="btn btn-primary btn-sm text-right txt-sm" 
                                                  ToolTip="Exportar a Excel" data-toggle="tooltip">
                                                  <span class="glyphicon glyphicon-list"></span>&nbsp;&nbsp;Excel
                                              </asp:LinkButton>
                                              <dx:BootstrapButton ID="lkb_Actualizar" runat="server" Text="Actualizar" OnClick="lkb_Actualizar_Click" ToolTip="Actualizar"
                                                  SettingsBootstrap-RenderOption="Primary" AutoPostBack="false" SettingsBootstrap-Sizing="Small" data-toggle="tooltip" CssClasses-Text="txt-sm">
                                                  <ClientSideEvents Click="function(s, e) { Callback3.PerformCallback(); LoadingPanel3.Show(); }" 
                                                                    Init="function(s, e) {LoadingPanel3.Hide();}" />
                                                  <CssClasses Icon="glyphicon glyphicon-refresh" /> 
                                              </dx:BootstrapButton>
                                              <asp:LinkButton ID="lkb_LimpiarFiltros" runat="server" OnClick="lkb_LimpiarFiltros_Click" CssClass="btn btn-primary btn-sm" ToolTip="Limpiar Filtros" >
                                                  <span class="glyphicon glyphicon-erase"></span>&nbsp;&nbsp;Limpiar Filtros                                        
                                              </asp:LinkButton>                                    
                                              <asp:LinkButton ID="lkb_Actualizar" runat="server" OnClick="lkb_Actualizar_Click" CssClass="btn btn-primary btn-sm text-right" ToolTip="Actualizar">
                                                   <span class="glyphicon glyphicon-refresh"></span>&nbsp;&nbsp;Actualizar
                                              </asp:LinkButton>
                                              --%>                            
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
                           <dx:ASPxGridViewExporter ID="ASPxGridViewExporter1" GridViewID="GridNP" runat="server" PaperKind="A5" Landscape="true" />                          
                        </asp:Panel>
                           
                        <dx:ASPxPageControl runat="server" ID="ASPxPageControl1" Height="60px" Width="100%" EnableCallBacks="true"  ContentStyle-Border-BorderWidth="3px"
                           TabAlign="Justify" ActiveTabIndex="0" EnableTabScrolling="true" Theme="SoftOrange" Font-Size="12px" ContentStyle-VerticalAlign="Top">
                           <TabStyle Paddings-PaddingLeft="10px" Paddings-PaddingRight="10px"  />                                                                                      
                           <ContentStyle>
                               <Paddings PaddingLeft="20px" PaddingRight="20px" PaddingTop="5px" />
                           </ContentStyle>
                           <TabPages>                                                                            
                              <dx:TabPage Text="Número de Parte">
                                  <ContentCollection>
                                      <dx:ContentControl ID="ContentControl4" runat="server">
                                          
                                          <table style="width:100%">
                                              <tr>
                                                  <td style="text-align:left; width:45%">
                                                    <asp:LinkButton ID="lkb_Agregar" runat="server" OnClick="lkb_Agregar_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button">
                                                      <%--<span class="glyphicon glyphicon-plus"></span>&nbsp;&nbsp;--%>Agregar
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="lkb_Editar" runat="server" OnClick="lkb_Editar_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button">
                                                      <%--<span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp;--%>Editar
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="lkb_Borrar" runat="server" OnClick="lkb_Borrar_OnClick" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button">
                                                      <%--<span class="glyphicon glyphicon-remove"></span>&nbsp;&nbsp;--%>Borrar
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="lkb_BorrarTodos" runat="server" OnClick="lkb_BorrarTodos_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button">
                                                      <%--<span class="glyphicon glyphicon-remove"></span>&nbsp;&nbsp;--%>Borrar Todas las Partidas
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="lkb_BajarPartidas" runat="server" OnClick="lkb_BajarPartidas_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button">
                                                      <%--<span class="glyphicon glyphicon-remove"></span>&nbsp;&nbsp;--%>Bajar Partidas
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="lkb_ImportarCove" runat="server" OnClick="lkb_ImportarCove_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button">
                                                      <%--<span class="glyphicon glyphicon-remove"></span>&nbsp;&nbsp;--%>Importar Cove
                                                    </asp:LinkButton>
                                                 </td>
                                                 <td style="text-align:left; width:2%">
                                                    <asp:CheckBox ID="ckSincronizar" runat="server" AutoPostBack="true" Font-Size="11px" />
                                                 </td>
                                                 <td style="text-align:left; width:10%">
                                                     <asp:Label ID="lblT_SincronizarPartidas" runat="server" Font-Size="11px" Text="Sincronizar Partidas" />
                                                 </td>
                                                 <td style="text-align:left; width:23%">

                                                 </td>                                                                        
                                                 <td style="text-align:left; width:5%">
                                                    <asp:Label ID="lblT_Categorias" runat="server" Font-Size="11px" Text="Categoria:" />
                                                 </td>
                                                 <td style="text-align:left; width:15%; padding-right:16px">
                                                    <dx:ASPxComboBox ID="xcbCategoriaAll" runat="server" DropDownStyle="DropDown" IncrementalFilteringMode="Contains"
                                                        ValueField="CATEGORIA" TextField="CATEGORIA" ValueType="System.String" Width="100%" CssClass="bordes_curvos_derecha">
                                                        
                                                    </dx:ASPxComboBox>
                                                 </td>                                                                      
                                              </tr>
                                          </table>



                                          <dx:ASPxGridView ID="GridNP2" runat="server" EnableTheming="True" Theme="SoftOrange" AutoGenerateColumns="False"
                                              SettingsPager-Mode="ShowAllRecords" EnableCallBacks="false" ClientInstanceName="gridnp2" Width="100%" OnCustomCallback="GridNP2_CustomCallback"
                                              Settings-HorizontalScrollBarMode="Auto" KeyFieldName="DETALLENPKEY" SettingsPager-Position="TopAndBottom"
                                              Styles-Header-ForeColor="#FFF" Styles-Header-Font-Size="11px" Styles-Cell-Font-Size="11px" Styles-Footer-Font-Size="11px">
                                              <SettingsAdaptivity AdaptivityMode="HideDataCellsWindowLimit" HideDataCellsAtWindowInnerWidth="800" 
                                                  AdaptiveDetailColumnCount="1" AllowOnlyOneAdaptiveDetailExpanded="True">
                                                  <AdaptiveDetailLayoutProperties colcount="2">
                                                      <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit"  SwitchToSingleColumnAtWindowInnerWidth="600" />
                                                  </AdaptiveDetailLayoutProperties>
                                              </SettingsAdaptivity> 
                                              <SettingsResizing ColumnResizeMode="Control" />
                                               <SettingsBehavior AllowSelectByRowClick="true" />
                                              <Settings ShowFooter="true" ShowHeaderFilterButton="true" ShowFilterRowMenu="false"  />                                          
                                              <Styles>                                                                                      
                                                  <SelectedRow />
                                                  <Row  />
                                                  <AlternatingRow Enabled="True" />
                                                  <PagerTopPanel Paddings-PaddingBottom="3px"></PagerTopPanel>
                                              </Styles>
                                              <SettingsPopup>
                                                  <HeaderFilter Height="200px" Width="195px" />
                                              </SettingsPopup>                                                
                                              <TotalSummary>
                                                  <dx:ASPxSummaryItem FieldName="SECUENCIA_PEDIMENTO" SummaryType="Count" DisplayFormat=" {0:n0}" />
                                                  <dx:ASPxSummaryItem FieldName="CANTIDAD" SummaryType="Sum" DisplayFormat=" {0:n2}" />
                                                  <dx:ASPxSummaryItem FieldName="PRECIO_TOTAL" SummaryType="Sum" DisplayFormat=" {0:n2}" />
                                              </TotalSummary>
                                              <Columns>
                                                       <dx:GridViewDataTextColumn FieldName="DETALLENPKEY" ReadOnly="True" VisibleIndex="0" Visible="false">
                                                       </dx:GridViewDataTextColumn>
                                                       <dx:GridViewDataTextColumn Caption="Secuencia" FieldName="SECUENCIA_PEDIMENTO" ReadOnly="True" VisibleIndex="1" Width="110px" >
                                                           <HeaderStyle HorizontalAlign="Center" ForeColor="#FFF"  />
                                                           <SettingsHeaderFilter Mode="CheckedList" />
                                                           <CellStyle HorizontalAlign="Center" />
                                                           <FooterCellStyle HorizontalAlign="Center" />
                                                       </dx:GridViewDataTextColumn>
                                                       <dx:GridViewDataTextColumn Caption="COVE" FieldName="COVE" ReadOnly="True" VisibleIndex="2" Width="150px">
                                                           <HeaderStyle HorizontalAlign="Center" />
                                                           <SettingsHeaderFilter Mode="CheckedList" />
                                                           <CellStyle HorizontalAlign="Center" />
                                                       </dx:GridViewDataTextColumn>
                                                       <dx:GridViewDataTextColumn Caption="Factura" FieldName="FACTURA" ReadOnly="True" VisibleIndex="4" Width="140px">
                                                           <HeaderStyle HorizontalAlign="Center" />
                                                           <SettingsHeaderFilter Mode="CheckedList" />
                                                           <CellStyle HorizontalAlign="Center" />
                                                       </dx:GridViewDataTextColumn>
                                                       <dx:GridViewDataDateColumn Caption="Fecha Factura" FieldName="FECHA_FACTURA" ReadOnly="True" VisibleIndex="5" Width="150px">
                                                           <HeaderStyle HorizontalAlign="Center" />
                                                           <SettingsHeaderFilter Mode="CheckedList" />
                                                           <CellStyle HorizontalAlign="Center" />
                                                       </dx:GridViewDataDateColumn>
                                                       <dx:GridViewDataTextColumn Caption="Número de Parte" FieldName="CODIGO_PRODUCTO" ReadOnly="True" VisibleIndex="6" Width="150px">
                                                           <HeaderStyle HorizontalAlign="Center" />
                                                           <SettingsHeaderFilter Mode="CheckedList" />
                                                           <CellStyle HorizontalAlign="Center" />
                                                       </dx:GridViewDataTextColumn>
                                                       <dx:GridViewDataTextColumn Caption="Cantidad Comercial" FieldName="CANTIDAD" ReadOnly="True" VisibleIndex="7" Width="160px">
                                                           <HeaderStyle HorizontalAlign="Center" />
                                                           <SettingsHeaderFilter Mode="CheckedList" />
                                                           <CellStyle HorizontalAlign="Right" />
                                                           <PropertiesTextEdit DisplayFormatString="{0:n2}" />
                                                           <FooterCellStyle HorizontalAlign="Right" />
                                                       </dx:GridViewDataTextColumn>
                                                       <dx:GridViewDataTextColumn Caption="UMC" FieldName="UNIDAD_MEDIDA" ReadOnly="True" VisibleIndex="8" Width="120px">
                                                           <HeaderStyle HorizontalAlign="Center" />
                                                           <SettingsHeaderFilter Mode="CheckedList" />
                                                           <CellStyle HorizontalAlign="Center" />
                                                       </dx:GridViewDataTextColumn>
                                                       <dx:GridViewDataTextColumn Caption="Descripción" FieldName="DESCRIPCION_COMERCIAL" ReadOnly="True" VisibleIndex="9" Width="500px">
                                                           <HeaderStyle HorizontalAlign="Center" />
                                                           <SettingsHeaderFilter Mode="CheckedList" />
                                                           <CellStyle HorizontalAlign="Left" />
                                                       </dx:GridViewDataTextColumn>
                                                       <dx:GridViewDataTextColumn Caption="Valor Dólares" FieldName="PRECIO_TOTAL" ReadOnly="True" VisibleIndex="10" Width="140px">
                                                           <HeaderStyle HorizontalAlign="Center" />
                                                           <SettingsHeaderFilter Mode="CheckedList" />
                                                           <CellStyle HorizontalAlign="Right" />
                                                           <PropertiesTextEdit DisplayFormatString="{0:n2}" />
                                                           <FooterCellStyle HorizontalAlign="Right" />
                                                       </dx:GridViewDataTextColumn>
                                                       <dx:GridViewDataTextColumn Caption="Cve. Cliente/Proveedor" FieldName="CLIENTE_PROVEEDOR" ReadOnly="True" VisibleIndex="11" Width="160px">
                                                           <HeaderStyle HorizontalAlign="Center" />
                                                           <SettingsHeaderFilter Mode="CheckedList" />
                                                           <CellStyle HorizontalAlign="Center" />
                                                       </dx:GridViewDataTextColumn>
                                                       <dx:GridViewDataTextColumn Caption="PO" FieldName="PO" ReadOnly="True" VisibleIndex="12" Width="80px">
                                                           <HeaderStyle HorizontalAlign="Center" />
                                                           <SettingsHeaderFilter Mode="CheckedList" />
                                                           <CellStyle HorizontalAlign="Center" />
                                                       </dx:GridViewDataTextColumn>
                                                       <%--<dx:GridViewDataTextColumn Caption="Categoria" FieldName="CATEGORIA" ReadOnly="True" VisibleIndex="13" Width="160px">
                                                           <HeaderStyle HorizontalAlign="Center" />
                                                           <SettingsHeaderFilter Mode="CheckedList" />
                                                           <CellStyle HorizontalAlign="Center" />
                                                       </dx:GridViewDataTextColumn>--%>                       
                                                   </Columns>
                                              <%--<Toolbars>
                                                  <dx:GridViewToolbar Name="Toolbar1" EnableAdaptivity="true" ItemAlign="Left">
                                                      <Items>
                                                          <dx:GridViewToolbarItem Name="Links" ItemStyle-Width="100%">
                                                          <Template>--%>
                                                              
                                                              <%--<dx:BootstrapButton ID="bbAgregar" runat="server" AutoPostBack="false" ClientInstanceName="bbAgregar" Width="60px" OnClick="bbAgregar_OnClick"
                                                                  SettingsBootstrap-RenderOption="Primary" Text="Agregar" CssClasses-Text="txt-sm" >
                                                                  <SettingsBootstrap RenderOption="Primary" Sizing="Small" />
                                                                  <ClientSideEvents Click="function(s, e) { Callback3.PerformCallback(); LoadingPanel3.Show(); }" 
                                                                      Init="function(s, e) {LoadingPanel3.Hide();}" />
                                                              </dx:BootstrapButton>
                                                              <dx:BootstrapButton ID="bbEditar" runat="server" AutoPostBack="false" ClientInstanceName="bbEditar"  Width="70px" OnClick="bbEditar_OnClick"
                                                                   SettingsBootstrap-RenderOption="Primary" Text="Editar" CssClasses-Text="txt-sm" >
                                                                   <SettingsBootstrap RenderOption="Primary" Sizing="Small" />
                                                                   <ClientSideEvents Click="function(s, e) { Callback3.PerformCallback(); LoadingPanel3.Show(); }" 
                                                                       Init="function(s, e) {LoadingPanel3.Hide();}" />
                                                              </dx:BootstrapButton>
                                                              <dx:BootstrapButton ID="bbBorrar" runat="server" AutoPostBack="false" ClientInstanceName="bbBorrar"  Width="80px" OnClick="bbBorrar_OnClick"
                                                                  SettingsBootstrap-RenderOption="Primary" Text="Borrar" CssClasses-Text="txt-sm" >
                                                                  <SettingsBootstrap RenderOption="Primary" Sizing="Small" />
                                                                  <ClientSideEvents Click="function(s, e) { Callback3.PerformCallback(); LoadingPanel3.Show(); }" 
                                                                      Init="function(s, e) {LoadingPanel3.Hide();}" />
                                                              </dx:BootstrapButton>
                                                              <dx:BootstrapButton ID="bbBorrarTodas" runat="server" AutoPostBack="false" ClientInstanceName="bbBorrarTodas"  Width="130px" 
                                                                  SettingsBootstrap-RenderOption="Primary" Text="Borrar" CssClasses-Text="txt-sm" >
                                                                  <SettingsBootstrap RenderOption="Primary" Sizing="Small" />
                                                                  <ClientSideEvents Click="function(s, e) { Callback3.PerformCallback(); LoadingPanel3.Show(); }" 
                                                                      Init="function(s, e) {LoadingPanel3.Hide();}" />
                                                              </dx:BootstrapButton>--%>
                                                              
                                                              
                                                          <%--</Template>
                                                         </dx:GridViewToolbarItem>           
                                                      </Items>
                                                  </dx:GridViewToolbar>                                               
                                              </Toolbars>  --%>                                            
                                           </dx:ASPxGridView>

                                      </dx:ContentControl>    
                                  </ContentCollection>
                              </dx:TabPage>
                              <dx:TabPage Text="Errores">
                                  <ContentCollection>    
                                      <dx:ContentControl ID="ContentControl5" runat="server">
                                          
                                          <dx:ASPxGridView ID="GridNP3" runat="server" EnableTheming="True" Theme="SoftOrange" KeyFieldName="ERRORKEY"
                                              EnableCallBacks="false" ClientInstanceName="gridnp3" AutoGenerateColumns="False" Width="100%" 
                                              Settings-HorizontalScrollBarMode="Auto" SettingsPager-Position="TopAndBottom" SettingsPager-Mode="ShowAllRecords"
                                              Styles-Header-ForeColor="#FFF" Styles-Header-Font-Size="11px" Styles-Cell-Font-Size="11px">
                                              <SettingsAdaptivity AdaptivityMode="HideDataCellsWindowLimit" HideDataCellsAtWindowInnerWidth="800" 
                                                  AdaptiveDetailColumnCount="1" AllowOnlyOneAdaptiveDetailExpanded="True">
                                                  <AdaptiveDetailLayoutProperties colcount="2">
                                                      <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit"  SwitchToSingleColumnAtWindowInnerWidth="600" />
                                                  </AdaptiveDetailLayoutProperties>
                                              </SettingsAdaptivity> 
                                              <SettingsResizing ColumnResizeMode="Control" />
                                               <SettingsBehavior AllowSelectByRowClick="true" />
                                              <Settings ShowFooter="False" ShowHeaderFilterButton="true" ShowFilterRowMenu="false"  />                                          
                                              <Styles>                                                                                      
                                                  <SelectedRow BackColor="#9E9E9E" ForeColor="#FFFFFF" />
                                                  <Row  />
                                                  <AlternatingRow Enabled="True" />
                                                  <PagerTopPanel Paddings-PaddingBottom="3px"></PagerTopPanel>
                                              </Styles>
                                              <SettingsPopup>
                                                  <HeaderFilter Height="200px" Width="195px" />
                                              </SettingsPopup>                                                
                                              <Columns>
                                                  <dx:GridViewDataTextColumn FieldName="ERRORKEY" ReadOnly="True" VisibleIndex="0" Visible="false">
                                                  </dx:GridViewDataTextColumn>
                                                  <dx:GridViewDataTextColumn Caption="Secuencia" FieldName="SEC" ReadOnly="True" VisibleIndex="1" Width="160px">
                                                      <HeaderStyle HorizontalAlign="Center" />
                                                      <SettingsHeaderFilter Mode="CheckedList" />
                                                      <CellStyle HorizontalAlign="Center" />
                                                  </dx:GridViewDataTextColumn>
                                                  <dx:GridViewDataTextColumn Caption="Pedimento" FieldName="PEDIMENTO" ReadOnly="True" VisibleIndex="2" Width="170px">
                                                      <HeaderStyle HorizontalAlign="Center" />
                                                      <SettingsHeaderFilter Mode="CheckedList" />
                                                      <CellStyle HorizontalAlign="Center" />
                                                  </dx:GridViewDataTextColumn>
                                                  <dx:GridViewDataTextColumn Caption="Descripción" FieldName="DESCRIPCION" ReadOnly="True" VisibleIndex="3" Width="683px" >
                                                      <HeaderStyle HorizontalAlign="Center" ForeColor="#FFF"  />
                                                      <SettingsHeaderFilter Mode="CheckedList" />
                                                      <CellStyle HorizontalAlign="Center" />
                                                  </dx:GridViewDataTextColumn>
                                                  <dx:GridViewDataDateColumn Caption="Origen SVU" FieldName="ORIGENDSVU" ReadOnly="True" VisibleIndex="4" Width="200px">
                                                      <HeaderStyle HorizontalAlign="Center" />
                                                      <SettingsHeaderFilter Mode="CheckedList" />
                                                      <CellStyle HorizontalAlign="Center" />
                                                  </dx:GridViewDataDateColumn>                                                                      
                                              </Columns>                
                                           </dx:ASPxGridView>

                                      </dx:ContentControl>  
                                  </ContentCollection>
                              </dx:TabPage>
                           </TabPages>
                        </dx:ASPxPageControl>

                        <dx:ASPxCallback ID="ASPxCallback3" runat="server" ClientInstanceName="Callback3" >
                            <ClientSideEvents  CallbackComplete="function(s, e) { System.Threading.Thread.Sleep(3000); LoadingPanel3.Hide(); }" />
                        </dx:ASPxCallback>
                        <dx:ASPxLoadingPanel ID="ASPxLoadingPanel3" runat="server" ClientInstanceName="LoadingPanel3"
                            Modal="True" ViewStateMode="Enabled" Theme="MaterialCompact" >
                        </dx:ASPxLoadingPanel>
                    </div>
                </div>
            </div>
        </asp:View>
    </asp:MultiView>

    <button id="btnModal" type="button" data-toggle="modal" data-target="#Modal" data-whatever="Nuevo" onclick="return false;" style="display: none;"></button>
    <div class="modal fade" id="Modal" tabindex="-1" role="dialog" aria-labelledby="ModalTitulo" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-lg" role="document" >
            <div class="modal-content">
                <div class="modal-header">
                    <h4 id="ModalTitulo" class="modal-title" runat="server"></h4>                    
                </div>
                <asp:Panel ID="Panel3" runat="server">
                    <div class="modal-body">
                        <div class="row">

                            <div class="form-group col-md-12">
                                <div class="form-group col-md-6">
                                    &nbsp;<label id="Label13" runat="server" style="font-size:11px">Cove</label>
                                    <asp:TextBox ID="txt_cove" runat="server" CssClass="form-control input-sm" MaxLength="50" placeholder="Cove" Width="100%"></asp:TextBox>
                                </div>                                   
                                <div class="form-group col-md-3">
                                        &nbsp;<label id="Label14" runat="server" style="font-size:11px">Factura</label>                                                                                     
                                        <asp:TextBox ID="txt_Factura" runat="server" CssClass="form-control input-sm" MaxLength="50" placeholder="Factura" Width="100%"></asp:TextBox>
                                </div>
                                <div class="form-group col-md-3">
                                    &nbsp;<label id="Label2" runat="server" style="font-size:11px">Fecha Factura</label>
                                    <dx:ASPxDateEdit ID="dateEdit_Fecha" runat="server" EditFormat="Custom" Date="" Width="100%" Theme="MaterialCompact" 
                                        Font-Size="12px" CssClass="bordes_curvos" NullText="Fecha" DisplayFormatString="dd/MM/yyyy">
                                        <CalendarProperties >
                                            <Style Font-Size="12px"></Style>
                                        </CalendarProperties>                                                                                
                                    </dx:ASPxDateEdit>
                                </div>
                            </div>                                                             
                            <div class="form-group col-md-12">
                                <div class="form-group col-md-3">
                                    &nbsp;<label id="Label3" runat="server" style="font-size:11px">*Número de Parte</label>                                                                                     
                                    <asp:TextBox ID="txt_NumParte" runat="server" CssClass="form-control input-sm" MaxLength="50" placeholder="*Número de Parte" Width="100%"></asp:TextBox>
                                </div>
                                <div class="form-group col-md-3">
                                    &nbsp;<label id="Label4" runat="server" style="font-size:11px">*Cantidad Comercial</label>                                                                                     
                                    <dx:ASPxSpinEdit ID="se_Cantidad" ClientInstanceName="se_Cantidad" runat="server" NullText="*Cantidad Comercial" Width="100%" NumberType="Float" DecimalPlaces="4" DisplayFormatString="N4" 
                                       MinValue="0" Number="0" ValidateRequestMode="Disabled" ShowOutOfRangeWarning="false" SpinButtons-ShowIncrementButtons="false">
                                       <ClearButton DisplayMode="OnHover"></ClearButton>
                                        <%--<ClientSideEvents LostFocus = "function(s,e) { ValidaValor(s); }" />--%>                                      
                                    </dx:ASPxSpinEdit>
                                </div>
                                <div class="form-group col-md-3">
                                    &nbsp;<label id="Label5" runat="server" style="font-size:11px">*UMC</label>                                                                                     
                                    <asp:TextBox ID="txt_UMC" runat="server" CssClass="form-control input-sm" MaxLength="15" placeholder="*UMC" Width="100%"></asp:TextBox>
                                </div>
                                <div class="form-group col-md-3">
                                    &nbsp;<label id="Label6" runat="server" style="font-size:11px">*Valor Dólares</label>                                                                                                                                                                     
                                    <dx:ASPxSpinEdit ID="se_ValorDolares" ClientInstanceName="se_ValorDolares" runat="server" NullText="*Valor Dólares" Width="100%" NumberType="Float" DecimalPlaces="4" DisplayFormatString="N4" 
                                       MinValue="0" Number="0" ValidateRequestMode="Disabled" ShowOutOfRangeWarning="false" SpinButtons-ShowIncrementButtons="false">
                                       <ClearButton DisplayMode="OnHover"></ClearButton>
                                       <ClientSideEvents LostFocus = "function(s,e) { ValidaValor(s); }" />                                     
                                    </dx:ASPxSpinEdit>
                                </div>  
                            </div>
                            <div class="form-group col-md-12">
                                <div class="form-group col-md-6">
                                        &nbsp;<label id="Label7" runat="server" style="font-size:11px">Descripción</label>                                                                                     
                                        <asp:TextBox ID="txt_Descripcion" runat="server" CssClass="form-control input-sm" MaxLength="250" placeholder="Descripción" Width="100%"></asp:TextBox>
                                </div>
                                <div class="form-group col-md-3">
                                        &nbsp;<label id="Label8" runat="server" style="font-size:11px">*Cve. Cliente/Proveedor</label>
                                        <asp:TextBox ID="txt_ClienteProv" runat="server" CssClass="form-control input-sm" MaxLength="50" placeholder="*Cve. Cliente/Proveedor" Width="100%"></asp:TextBox>
                                </div>
                                <div class="form-group col-md-3">
                                        &nbsp;<label id="Label9" runat="server" style="font-size:11px">PO</label>                                                                                     
                                        <asp:TextBox ID="txt_PO" runat="server" CssClass="form-control input-sm" MaxLength="250" placeholder="PO" Width="100%"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group col-md-12">
                                <div class="form-group col-md-3">
                                        &nbsp;<label id="Label10" runat="server" style="font-size:11px">Categoria</label>                                                                                     
                                         <dx:ASPxComboBox ID="xcbCategoria" runat="server" DropDownStyle="DropDown" IncrementalFilteringMode="Contains"
                                             ValueField="IdCategoria" TextField="Categoria" ValueType="System.String" Width="100%" CssClass="bordes_curvos_derecha">                                             
                                         </dx:ASPxComboBox>
                                </div>
                                <div class="form-group col-md-3">
                                    &nbsp;<label id="Label11" runat="server" style="font-size:11px">Secuencia</label>
                                        <asp:TextBox ID="TextBox1" runat="server" CssClass="form-control input-sm" MaxLength="50" placeholder="Secuencia" Width="100%"></asp:TextBox>
                                </div>
                                <div class="form-group col-md-6">
                                
                                </div>                                
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
                            <asp:LinkButton ID="LinkButton2" runat="server" CssClass="btn btn-primary btn-sm" Text="Guardar" OnClick="btnGuardarNP_Click">
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
    <div class="modal fade" id="ModalCG" tabindex="-1" role="dialog" aria-labelledby="ModalCGTitulo" data-backdrop="static">
        <div class="modal-dialog modal-xl" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h6 id="ModalCGTitulo" class="modal-title" runat="server"></h6>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                      <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="row" style="margin-left:10px">
                        <dx:ASPxPageControl runat="server" ID="ASPxPageControlCG" Height="60px" Width="98%" EnableCallBacks="true"  ContentStyle-Border-BorderWidth="3px"
                             TabAlign="Justify" ActiveTabIndex="0" EnableTabScrolling="true" Theme="SoftOrange" Font-Size="12px" ContentStyle-VerticalAlign="Top">
                             <TabStyle Paddings-PaddingLeft="50px" Paddings-PaddingRight="50px"  />                                                                                      
                             <ContentStyle>
                                 <Paddings PaddingLeft="20px" PaddingRight="20px" PaddingTop="5px" />
                             </ContentStyle>
                             <TabPages>                                                                            
                                 <dx:TabPage Text="Cuenta de Gastos">
                                     <ContentCollection>
                                     <dx:ContentControl ID="ContentControl3" runat="server">
                                         <div class="row">
                                             <asp:LinkButton ID="lkb_Revisar" runat="server" CssClass="btn btn-primary btn-sm text-right txt-sm" data-toggle="tooltip" OnClick="lkb_Revisar_Click" >
                                                <span class="glyphicon glyphicon-list"></span>&nbsp;&nbsp;Revisado
                                             </asp:LinkButton>
                                             <asp:LinkButton ID="lkb_Autorizar" runat="server" CssClass="btn btn-primary btn-sm text-right txt-sm" data-toggle="tooltip" OnClick="lkb_Autorizar_Click">
                                                <span class="glyphicon glyphicon-list"></span>&nbsp;&nbsp;Autorizado
                                             </asp:LinkButton>
                                             <asp:LinkButton ID="lkb_Rechazar" runat="server" CssClass="btn btn-primary btn-sm text-right txt-sm" data-toggle="tooltip" OnClick="lkb_Rechazar_Click">
                                                <span class="glyphicon glyphicon-list"></span>&nbsp;&nbsp;Rechazado
                                             </asp:LinkButton>
                                             <asp:LinkButton ID="lkb_Asignar" runat="server" CssClass="btn btn-primary btn-sm text-right txt-sm" data-toggle="tooltip" OnClick="lkb_Asignar_Click" >
                                                <span class="glyphicon glyphicon-list"></span>&nbsp;&nbsp;Asignar Usuario
                                             </asp:LinkButton>
                                             &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                             <asp:Label ID="lblTit_AsignarUsuario" runat="server" Font-Size="11px" Text="Usuario asignado: " />
                                             &nbsp;&nbsp;
                                             <asp:Label ID="lbl_AsignarUsuario" runat="server" Font-Size="11px" />
                                             <asp:Label ID="lbl_IdAsignarUsuario" runat="server" Visible="false" />                                            
                                             <dx:ASPxGridView ID="detailGridCG" runat="server" KeyFieldName="PEDIMENTOARMADO" Theme="SoftOrange" Styles-Header-Font-Size="11px"  
                                                 Width="100%" OnCustomCallback="Grid_CustomCallbackCG" Settings-HorizontalScrollBarMode="Auto">
                                                 <Columns>
                                                     <dx:GridViewDataTextColumn FieldName="CGKEY" ReadOnly="True" VisibleIndex="0" Visible="false" >
                                                     </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataColumn Caption="SELECCIONAR" VisibleIndex="0" Width="90px" >
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <CellStyle HorizontalAlign="Center"></CellStyle>
                                                        <DataItemTemplate>
                                                            <dx:ASPxCheckBox ID="chkConsultarCU" ClientInstanceName="chkConsultarCU" Enabled="true" runat="server" OnInit="chkConsultarCU_Init">
                                                            </dx:ASPxCheckBox>
                                                        </DataItemTemplate>
                                                     </dx:GridViewDataColumn>
                                                     <dx:GridViewDataColumn Caption="EDITAR" VisibleIndex="0" Width="60px">
                                                         <EditFormSettings Visible="False" />
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                         <CellStyle HorizontalAlign="Center" />
                                                         <DataItemTemplate>
                                                             <dx:ASPxButton ID="btnEditarCU" runat="server" AutoPostBack="false" Text="Editar" OnClick="btnEditarCU_Click" Font-Size="11px"
                                                                 EnableTheming="false" RenderMode="Link" VerticalAlign="Top" >
                                                             </dx:ASPxButton>
                                                         </DataItemTemplate>                                                            
                                                         <SettingsHeaderFilter Mode="CheckedList" />
                                                     </dx:GridViewDataColumn>
                                                     <dx:GridViewDataColumn Caption="ESTATUS" VisibleIndex="1" Width="60px">
                                                         <EditFormSettings Visible="False" />
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                         <CellStyle HorizontalAlign="Center" />
                                                         <DataItemTemplate>
                                                            <table>
                                                               <tr>
                                                                   <td>                                                             
                                                                      <dx:ASPxButton ID="btnEstatusCG" runat="server" AutoPostBack="false"  
                                                                          EnableTheming="false" RenderMode="Link" VerticalAlign="Top" OnInit="btnEstatusCG_Init">
                                                                      </dx:ASPxButton>
                                                                  </td>
                                                               </tr>
                                                            </table>
                                                         </DataItemTemplate>                                                                                                                                                                                 
                                                     </dx:GridViewDataColumn>
                                                                                                                                              
                                                     <dx:GridViewDataTextColumn FieldName="STATUS" ReadOnly="True" VisibleIndex="1" Width="65px" Visible="false" >
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                         <CellStyle HorizontalAlign="Center" />
                                                     </dx:GridViewDataTextColumn>

                                                     <dx:GridViewDataTextColumn Caption=" NO PROVEEDOR <br/> SAP" FieldName="NO PROVEEDOR SAP" ReadOnly="True" VisibleIndex="2" Width="95px">
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                         <CellStyle HorizontalAlign="Center" />
                                                     </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataTextColumn Caption="NO CUENTA <br/> DE <BR/> GASTOS" FieldName="NO CUENTA DE GASTOS" ReadOnly="True" VisibleIndex="3" Width="95px">
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                         <CellStyle HorizontalAlign="Center" />
                                                     </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataDateColumn Caption="FECHA" FieldName="FECHA CUENTA DE GASTOS" ReadOnly="True" VisibleIndex="4" Width="95px">
                                                         <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy"></PropertiesDateEdit>
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                         <CellStyle HorizontalAlign="Center" />
                                                     </dx:GridViewDataDateColumn>
                                                     <dx:GridViewDataTextColumn Caption="CONCEPTO<br/>EXT" FieldName="CONCEPTO EXTRAORDINARIO" ReadOnly="True" VisibleIndex="5" Width="120px">
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                         <CellStyle HorizontalAlign="Center" />
                                                     </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataTextColumn Caption="GASTO<br/>EXT" FieldName="GASTO EXTRAORDINARIO" ReadOnly="True" VisibleIndex="6" Width="95px">
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                         <CellStyle HorizontalAlign="Center" />
                                                     </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataTextColumn Caption="CONTADOR<BR/>GASTO EXT" FieldName="CONTADOR" ReadOnly="True" VisibleIndex="7" Width="110px">
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                         <CellStyle HorizontalAlign="Center" />
                                                     </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataTextColumn Caption="GASTO EN<BR/>RANGO" FieldName="GASTO EN RANGO" ReadOnly="True" VisibleIndex="7" Width="110px">
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                         <CellStyle HorizontalAlign="Center" />
                                                     </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataTextColumn Caption="TOTAL <br/> BASE <BR/> IVA" FieldName="TOTAL BASE IVA" ReadOnly="True" VisibleIndex="8" Width="100px">
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                         <CellStyle HorizontalAlign="Right" />
                                                         <PropertiesTextEdit DisplayFormatString="{0:n4}" />
                                                     </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataTextColumn Caption="TOTAL <br/> IVA" FieldName="TOTAL IVA" ReadOnly="True" VisibleIndex="9" Width="100px">
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                         <CellStyle HorizontalAlign="Right" />
                                                         <PropertiesTextEdit DisplayFormatString="{0:n4}" />
                                                     </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataTextColumn Caption="TOTAL" FieldName="TOTAL CUENTA DE GASTOS" ReadOnly="True" VisibleIndex="10" Width="100px">
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                         <CellStyle HorizontalAlign="Right" />
                                                         <PropertiesTextEdit DisplayFormatString="{0:n4}" />
                                                     </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataTextColumn Caption="POLIZA <br/> ANTICIPO" FieldName="POLIZA ANTICIPO" ReadOnly="True" VisibleIndex="11" Width="90px" Visible="false">
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                         <CellStyle HorizontalAlign="Center" />
                                                     </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataTextColumn Caption="ANTICIPO" FieldName="ANTICIPO" ReadOnly="True" VisibleIndex="12" Width="95px" Visible="false">
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                         <CellStyle HorizontalAlign="Right" />
                                                         <PropertiesTextEdit DisplayFormatString="{0:n4}" />
                                                     </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataTextColumn Caption="CLAVE <br/> CONCEPTO" FieldName="CLAVE CONCEPTO" ReadOnly="True" VisibleIndex="13" Width="90px">
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                         <CellStyle HorizontalAlign="Center" />
                                                     </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataTextColumn Caption="CONCEPTO <BR/> CUENTAS <br/> DE GASTO" FieldName="CONCEPTO CUENTAS DE GASTO" ReadOnly="True" VisibleIndex="14" Width="170px">
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                     </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataTextColumn Caption="VALIDACIÓN <BR/> AUTOMÁTICA <br/>CONCEPTO" FieldName="CONCEPTO CUENTAS DE GASTO" ReadOnly="True" VisibleIndex="15" Width="170px">
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                     </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataTextColumn Caption="IMPORTE <br/> TOTAL A <BR/> PAGAR CONCEPTO" FieldName="IMPORTE TOTAL A PAGAR CONCEPTO" ReadOnly="True" VisibleIndex="16" Width="120px">
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                         <CellStyle HorizontalAlign="Right" />
                                                         <PropertiesTextEdit DisplayFormatString="{0:n4}" />
                                                     </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataTextColumn Caption="IMPORTE <br/> SIN <br/> IVA" FieldName="IMPORTE SIN IVA" ReadOnly="True" VisibleIndex="17" Width="100px">
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                         <CellStyle HorizontalAlign="Right" />
                                                         <PropertiesTextEdit DisplayFormatString="{0:n4}" />
                                                     </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataTextColumn Caption="RETENCIÓN <br/> IVA" FieldName="RETENCION IVA" ReadOnly="True" VisibleIndex="18" Width="100px">
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                         <CellStyle HorizontalAlign="Right" />
                                                         <PropertiesTextEdit DisplayFormatString="{0:n4}" />
                                                     </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataTextColumn Caption="IMPORTE <br/> IVA <br/> CONCEPTO" FieldName="IMPORTE IVA CONCEPTO" ReadOnly="True" VisibleIndex="19" Width="100px">
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                         <CellStyle HorizontalAlign="Right" />
                                                         <PropertiesTextEdit DisplayFormatString="{0:n4}" />
                                                     </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataTextColumn Caption="MONEDA" FieldName="MONEDA" ReadOnly="True" VisibleIndex="20" Width="85px">
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                         <CellStyle HorizontalAlign="Center" />
                                                     </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataTextColumn Caption="TIPO <br/> CUENTA" FieldName="TIPOCUENTA" ReadOnly="True" VisibleIndex="21" Width="90px">
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                         <CellStyle HorizontalAlign="Center" />
                                                     </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataTextColumn Caption="COMPROBADOS" FieldName="COMPROBADOS" ReadOnly="True" VisibleIndex="22" Width="95px">
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                         <CellStyle HorizontalAlign="Center" />
                                                     </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataTextColumn Caption="CANCELA A" FieldName="CANCELA A" ReadOnly="True" VisibleIndex="23" Width="87px">
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                         <CellStyle HorizontalAlign="Center" />
                                                     </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataTextColumn Caption="REFERENCIA <br/> DE <br/> TRÁFICO" FieldName="REFERENCIA DE TRAFICO" ReadOnly="True" VisibleIndex="24" Width="100px">
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                         <CellStyle HorizontalAlign="Center" />
                                                     </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataTextColumn Caption="NOMBRE <br/> PROVEEDOR" FieldName="NOMBRE PROVEEDOR" ReadOnly="True" VisibleIndex="25" Width="210px">
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                     </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataTextColumn Caption="RFC <br/> PROVEEDOR" FieldName="RFC PROVEEDOR" ReadOnly="True" VisibleIndex="26" Width="110px">
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                     </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataTextColumn Caption="RFC <br/> AGENCIA" FieldName="RFC AGENCIA" ReadOnly="True" VisibleIndex="27" Width="110px">
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                     </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataDateColumn Caption="FECHA <BR/> DE <BR/> CARGA" FieldName="FECHA DE CARGA" ReadOnly="True" VisibleIndex="28" Width="100px">
                                                         <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy"></PropertiesDateEdit>
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                         <CellStyle HorizontalAlign="Center" />
                                                     </dx:GridViewDataDateColumn>
                                                     <dx:GridViewDataTextColumn Caption="FOLIO <br/> COMPROBANTE <br/> DE TERCERO" FieldName="FOLIO COMPROBANTE DE TERCERO" ReadOnly="True" VisibleIndex="29" Width="120px">
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                         <CellStyle HorizontalAlign="Center" />
                                                     </dx:GridViewDataTextColumn>
                                                     <%--<dx:GridViewCommandColumn Caption="PDF" VisibleIndex="6" Width="80px">
                                                         <CustomButtons>
                                                             <dx:GridViewCommandColumnCustomButton ID="btnSeleccionarPDF" Text="PDF" 
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
                                                 </Columns>
                                                 <SettingsBehavior AllowSelectByRowClick="false" AllowSelectSingleRowOnly="true" />
                                                 <Settings ShowFooter="true" />
                                                 <TotalSummary>
                                                     <dx:ASPxSummaryItem FieldName="IMPORTE TOTAL A PAGAR CONCEPTO" SummaryType="Sum" DisplayFormat=" {0:n2}" />
                                                     <dx:ASPxSummaryItem FieldName="IMPORTE SIN IVA" SummaryType="Sum" DisplayFormat=" {0:n2}" />
                                                     <dx:ASPxSummaryItem FieldName="RETENCION IVA" SummaryType="Sum" DisplayFormat=" {0:n2}" />
                                                     <dx:ASPxSummaryItem FieldName="IMPORTE IVA CONCEPTO" SummaryType="Sum" DisplayFormat=" {0:n2}" />
                                                 </TotalSummary>                                                                                           
                                                 <Styles>                                                               
                                                 <SelectedRow BackColor="#9E9E9E" ForeColor="#FFFFFF" />
                                                 <Row Font-Size="11px" />
                                                 <AlternatingRow Enabled="True" />
                                                 <PagerTopPanel Paddings-PaddingBottom="3px">
                                                     <Paddings PaddingBottom="3px" />
                                                     </PagerTopPanel>
                                                 </Styles>
                                                 <SettingsPager Position="TopAndBottom" PageSize="10">
                                                 </SettingsPager>                                                                                               
                                             </dx:ASPxGridView>
                                         </div>
                                     </dx:ContentControl>
                                 </ContentCollection>
                                 </dx:TabPage>
                             </TabPages>
                         </dx:ASPxPageControl>
                    </div>                    
                </div>
                <%--<div class="modal-footer">--%>
                  <%--<button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>--%>
                <%--</div>--%>
            </div>
        </div>
    </div>
    <div class="modal fade" id="ModalED" tabindex="-1" role="dialog" aria-labelledby="ModalEDTitulo" data-backdrop="static">
        <div class="modal-dialog modal-xl" role="document">
            <div class="modal-content">
                <asp:Panel ID="PanelED" runat="server">
                    <div class="modal-header">
                        <h6 id="ModalEDTitulo" class="modal-title" runat="server"></h6>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                          <span aria-hidden="true">&times;</span>
                        </button>
                    </div>                
                    <div class="modal-body">                    
                        <div class="row" style="margin-left:2px">
                            <div class="row" style="margin-right:5px;margin-bottom:5px">
                            <div class="col-xs-12 col-sm-12 col-md-4">                            
                                <div class="form-group" style="position: relative; width: 100%; float: left;" runat="server" title="" data-toggle="tooltip">
                                    <div class="input-group">
                                        <span class="input-group-addon">
                                            <span id="span1" runat="server" class="glyphicon glyphicon-list-alt" aria-hidden="true" ></span>
                                        </span>
                                        <dx:ASPxComboBox ID="cbxTipoArchivo" Caption="" runat="server" Height="30px" NullText="TIPO DE ARCHIVO" DataSecurityMode="Default"
                                            Width="100%" Font-Size="11px" Font-Names="Arial,Helvetica" CssClass="bordes_curvos_derecha" Theme="MaterialCompact" ViewStateMode="Enabled"
                                            AutoPostBack="true" ForeColor="#6B5555" TextField="TIPOS DE ARCHIVO" ValueField="SUFIJO" OnSelectedIndexChanged="cbxTipoArchivo_SelectedIndexChanged">
                                            <ClientSideEvents SelectedIndexChanged="function(s, e) { Callback.PerformCallback(); LoadingPanel2.Show(); }" />
                                        </dx:ASPxComboBox>
                                    </div>
                                </div>                            
                            </div>
                            <div class="col-xs-12 col-sm-12 col-md-4" style="padding-bottom:3px">
                                <%--<dx:ASPxUploadControl ID="ASPxUploadControl1" runat="server" UploadMode="Auto" Width="100%" ShowProgressPanel="true" ShowUploadButton="true" 
                                     FileUploadMode="BeforePageLoad" OnFileUploadComplete="upc1_FileUploadComplete" NullText="Arrastre un archivo aquí" AutoStartUpload="true">
                                    <ClientSideEvents FileUploadComplete="OnUploadComplete" FilesUploadStart="onFilesUploadStart" />
                                    <AdvancedModeSettings EnableMultiSelect="false" EnableDragAndDrop="true"></AdvancedModeSettings>
                                    <ValidationSettings MaxFileSize="4194304" AllowedFileExtensions=".pdf,.doc,.docx,.jpg,.jpeg,.gif,.png,.bmp"></ValidationSettings>
                                    <BrowseButton Text="Archivo(s)" />--%> <%-- MaxFileSize="4194304" Maximum file size: 4 MB.--%>
                                <%--</dx:ASPxUploadControl>--%>
                                <dx:ASPxUploadControl ID="upc1" runat="server" UploadMode="Auto" Width="100%" ShowProgressPanel="true" ShowUploadButton="true" FileUploadMode="BeforePageLoad" 
                                    OnFileUploadComplete="upc1_FileUploadComplete" NullText="Arrastre un archivo aquí" AutoStartUpload="true" ShowClearFileSelectionButton="true" >
                                    <ClientSideEvents FileUploadComplete="function(s, e) { OnUploadComplete(e); }" FilesUploadStart="onFilesUploadStart" />
                                    <AdvancedModeSettings EnableMultiSelect="false" EnableFileList="false" EnableDragAndDrop="true"></AdvancedModeSettings>                                    
                                    <BrowseButton Text="Archivo(s)"  /> 
                                </dx:ASPxUploadControl>
                                <table style="width:100%">
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
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        
                                    </tr>
                                </table>
                            </div>
                            <div class="col-xs-12 col-sm-12 col-md-4" style="padding-bottom:3px">
                                <asp:LinkButton ID="btnGuardar" runat="server" CssClass="btn btn-primary btn-sm" OnClick="btnGuardar_Click" ToolTip="Guardar Archivo">
                                 <span class="glyphicon glyphicon-ok"></span>&nbsp;&nbsp;Guardar Archivo
                                </asp:LinkButton>
                            </div>    
                        </div>
                            <dx:ASPxPageControl runat="server" ID="ASPxPageControlED" Height="60px" Width="98%" EnableCallBacks="true"  ContentStyle-Border-BorderWidth="3px"
                            TabAlign="Justify" ActiveTabIndex="0" EnableTabScrolling="true" Theme="SoftOrange" Font-Size="12px" ContentStyle-VerticalAlign="Top">
                             <TabStyle Paddings-PaddingLeft="10px" Paddings-PaddingRight="10px"  />                                                                                      
                             <ContentStyle>
                                 <Paddings PaddingLeft="20px" PaddingRight="20px" PaddingTop="5px" />
                             </ContentStyle>
                             <TabPages>                                                                            
                                 <dx:TabPage Text="EXPEDIENTE DIGITAL">
                                     <ContentCollection>
                                     <dx:ContentControl ID="ContentControl1" runat="server">
                                         <div class="row">
                                             <dx:ASPxGridView ID="detailGridDocumentos" runat="server" KeyFieldName="PEDIMENTOARMADO" Theme="SoftOrange" Styles-Header-Font-Size="11px"  
                                                 Width="100%" OnCustomCallback="Grid_CustomCallbackDocumentos" OnCustomButtonCallback="detailGridDocumentos_CustomButtonCallback"
                                                 ClientInstanceName="detailGridDocs" SettingsPager-Position="TopAndBottom">
                                                 <Columns>
                                                     <dx:GridViewDataTextColumn FieldName="FILEKEY" ReadOnly="True" VisibleIndex="0" Visible="false" >
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                         <CellStyle HorizontalAlign="Center" />
                                                     </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataColumn Caption="DESCARGAR" VisibleIndex="1" Width="7%" >
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <CellStyle HorizontalAlign="Center"></CellStyle>
                                                        <DataItemTemplate>
                                                            <dx:ASPxCheckBox ID="chkConsultar" ClientInstanceName="chkConsultar" Enabled="true" runat="server" OnInit="chkConsultar_Init">
                                                            </dx:ASPxCheckBox>
                                                        </DataItemTemplate>
                                                     </dx:GridViewDataColumn>
                                                     <dx:GridViewDataColumn Caption="BORRAR" VisibleIndex="2" Width="6%">
                                                         <EditFormSettings Visible="False" />
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                         <CellStyle HorizontalAlign="Center" />
                                                         <DataItemTemplate>
                                                             <dx:ASPxButton ID="btnBorrarFile" runat="server" AutoPostBack="false" Text="Borrar" OnClick="btnBorrarFile_Click" Font-Size="11px"
                                                                 EnableTheming="false" RenderMode="Link" VerticalAlign="Top" Visible='<%# Eval("VISIBLEB") %>' >
                                                             </dx:ASPxButton>
                                                         </DataItemTemplate>                                                            
                                                         <SettingsHeaderFilter Mode="CheckedList" />
                                                     </dx:GridViewDataColumn>                                                                                                                                                     
                                                     <dx:GridViewDataTextColumn Caption="CLAVE" FieldName="CLAVE" ReadOnly="True" VisibleIndex="3" Width="6%" >
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                         <CellStyle HorizontalAlign="Center" />
                                                     </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataTextColumn Caption="COMPLEMENTO 1" FieldName="COMPLEMENTO 1" ReadOnly="True" VisibleIndex="4" Width="25%">
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                     </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataTextColumn Caption="DESCRIPCIÓN" FieldName="DESCRIPCION" ReadOnly="True" VisibleIndex="5" Width="35%">
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                     </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataTextColumn Caption="USUARIO" FieldName="USERNAME" ReadOnly="True" VisibleIndex="6" Width="15%" >
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                         <CellStyle HorizontalAlign="Left" />
                                                     </dx:GridViewDataTextColumn>                                                     
                                                     <dx:GridViewDataTextColumn Caption="STATUS" FieldName="STATUS" ReadOnly="True" VisibleIndex="7" Width="6%">
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                         <CellStyle HorizontalAlign="Center" />
                                                     </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataTextColumn Caption="ID" FieldName="ID" ReadOnly="True" VisibleIndex="8" Visible="false">
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                     </dx:GridViewDataTextColumn>
                                                     <%-- <dx:GridViewCommandColumn Caption="PDF" VisibleIndex="6" Width="80px">
                                                         <CustomButtons>
                                                             <dx:GridViewCommandColumnCustomButton ID="btnSeleccionarPDF" Text="PDF" 
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
                                                     <%--<dx:GridViewCommandColumn Caption="PDF" ShowSelectCheckbox="true" VisibleIndex="6">
                                                     </dx:GridViewCommandColumn>--%>                                                     
                                                     <%--<dx:GridViewCommandColumn Caption="PDF" VisibleIndex="7" ButtonRenderMode="Image" Width="60px" ToolTip="PDF">
                                                         <CustomButtons>
                                                             <dx:GridViewCommandColumnCustomButton ID="btnED_PDF" Text="ED" Image-ToolTip="PDF"
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
                                                     </dx:GridViewCommandColumn>--%>                                                                                                                                                                                                   
                                                 </Columns>
                                                 <SettingsBehavior AllowSelectByRowClick="false" AllowSelectSingleRowOnly="true" AllowSort="false" />
                                                 <SettingsAdaptivity AdaptivityMode="HideDataCellsWindowLimit" HideDataCellsAtWindowInnerWidth="800" 
                                                     AdaptiveDetailColumnCount="1" AllowOnlyOneAdaptiveDetailExpanded="True">
                                                     <AdaptiveDetailLayoutProperties colcount="2">
                                                         <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit"  SwitchToSingleColumnAtWindowInnerWidth="600" />
                                                     </AdaptiveDetailLayoutProperties>
                                                 </SettingsAdaptivity> 
                                                 <SettingsResizing ColumnResizeMode="Control" />                                                                                               
                                                 <Styles>                                                               
                                                    <Row Font-Size="11px" />
                                                    <AlternatingRow Enabled="True" />
                                                    <PagerTopPanel Paddings-PaddingBottom="3px">
                                                        <Paddings PaddingBottom="3px" />
                                                        </PagerTopPanel>
                                                 </Styles>
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
                                                 <SettingsPager Position="TopAndBottom" PageSize="400">
                                                 </SettingsPager>                                                                                               
                                             </dx:ASPxGridView>
                                         </div>
                                     </dx:ContentControl>
                                 </ContentCollection>
                                 </dx:TabPage>
                             </TabPages>
                         </dx:ASPxPageControl>
                        </div>
                        <dx:ASPxCallback ID="ASPxCallback2" runat="server" ClientInstanceName="Callback" >
                        <ClientSideEvents  CallbackComplete="function(s, e) { System.Threading.Thread.Sleep(3000); LoadingPanel2.Hide(); }" />
                        </dx:ASPxCallback>
                        <dx:ASPxLoadingPanel ID="ASPxLoadingPanel2" runat="server" ClientInstanceName="LoadingPanel2"
                            Modal="True" ViewStateMode="Enabled" Theme="MaterialCompact" >
                        </dx:ASPxLoadingPanel>
                    </div>                
                    <div class="modal-footer">
                        <div class="col-xs-12 col-sm-4 col-md-4 text-left" style="margin-bottom:5px">
                             <asp:LinkButton ID="btnPDF" runat="server" CssClass="btn btn-primary btn-sm disabled" OnClick="btnPDF_Click" ToolTip="Descargar archivo"
                                 OnClientClick="">
                                 <span class="glyphicon glyphicon-download-alt"></span>&nbsp;&nbsp;Descargar Archivo
                             </asp:LinkButton>
                             &nbsp;&nbsp;&nbsp;
                             <asp:LinkButton ID="btnZip" runat="server" CssClass="btn btn-primary btn-sm disabled" OnClick="btnZip_Click" ToolTip="Descargar archivo zip"
                                 OnClientClick="">
                                 <span class="glyphicon glyphicon-compressed"></span>&nbsp;&nbsp;Descargar Zip
                             </asp:LinkButton>
                         </div>
                         <div class="col-xs-12 col-sm-4 col-md-4 text-center">
                             <%--<asp:LinkButton ID="btnGuardar" runat="server" CssClass="btn btn-success btn-sm" OnClick="btnGuardar_Click" ToolTip="Guardar Archivo(s)">
                                 <span class="glyphicon glyphicon-ok"></span>&nbsp;&nbsp;Guardar Archivo(s)</asp:LinkButton>--%>                            
                         </div>
                         <div class="col-xs-12 col-sm-4 col-md-4">

                         </div>
                        <%--<div class="col-md-12 col-md-offset-0" style="text-align: center;">
                            <button type="button" class="btn btn-danger btn-sm" data-dismiss="modal">
                                    <span class="glyphicon glyphicon-remove"></span>&nbsp;&nbsp;Cancelar
                                </button>
                                <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                        </div>--%> 
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div> 
    <div class="modal fade" id="ModalEncabezado" tabindex="-1" role="dialog" aria-labelledby="ModalEncabezadoTitulo" data-backdrop="static">
        <div class="modal-dialog modal-xl" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h6 id="ModalEncabezadoTitulo" class="modal-title" runat="server"></h6>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                      <span aria-hidden="true">&times;</span>
                    </button>
                </div>
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
                                        <dx:ContentControl ID="CC_DatosGenerales" runat="server" >                                        
                                            <div class="row" style="overflow:auto; height:300px">
                                                <dx:ASPxFormLayout runat="server" ID="ASPxFormLayoutDG" Font-Size="11px">
                                                    <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit" SwitchToSingleColumnAtWindowInnerWidth="1200" />
                                                    <Items>
                                                        <dx:LayoutGroup Caption="" ColCount="2" GroupBoxDecoration="None" UseDefaultPaddings="false" BackColor="#E8E8E8">
                                                            <GroupBoxStyle>
                                                                <Caption Font-Bold="true" />
                                                            </GroupBoxStyle>
                                                            <ParentContainerStyle BackColor="#E8E8E8" CssClass="bordes_curvos">
                                                                <Paddings PaddingLeft="10px" PaddingRight="10px" PaddingTop="10px" />
                                                            </ParentContainerStyle>
                                                            <Items>                                                                
                                                                <dx:LayoutItem Caption="PEDIMENTO" >
                                                                    <CaptionStyle CssClass="background_texto_btn"/>
                                                                    <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                                    <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                                    <%--<ParentContainerStyle BackColor="#E8E8E8" Paddings-PaddingLeft="15px" Paddings-PaddingTop="5px" />--%>                                                                 
                                                                    <LayoutItemNestedControlCollection>
                                                                        <dx:LayoutItemNestedControlContainer>
                                                                            <dx:ASPxLabel ID="DG_PEDIMENTOARMADO" runat="server" Font-Size="11px" />                                                                            
                                                                        </dx:LayoutItemNestedControlContainer>
                                                                    </LayoutItemNestedControlCollection>
                                                                </dx:LayoutItem>
                                                                <dx:LayoutItem Caption="TIPO DE CAMBIO">                                                                    
                                                                    <CaptionStyle CssClass="background_texto_btn"/>
                                                                    <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                                    <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                                    <%--<ParentContainerStyle BackColor="#E8E8E8" Paddings-PaddingLeft="15px" Paddings-PaddingRight="15px" Paddings-PaddingTop="5px" />--%>
                                                                    <LayoutItemNestedControlCollection>
                                                                        <dx:LayoutItemNestedControlContainer>
                                                                            <dx:ASPxLabel ID="DG_TIPO_CAMBIO" runat="server" Font-Size="11px" />
                                                                        </dx:LayoutItemNestedControlContainer>
                                                                    </LayoutItemNestedControlCollection>
                                                                </dx:LayoutItem>
                                                                <dx:LayoutItem Caption="FECHA">                                                                    
                                                                    <CaptionStyle CssClass="background_texto_btn"/>
                                                                    <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                                    <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                                    <%--<ParentContainerStyle BackColor="#E8E8E8" Paddings-PaddingLeft="15px" Paddings-PaddingTop="5px"/>--%>
                                                                    <LayoutItemNestedControlCollection>
                                                                        <dx:LayoutItemNestedControlContainer>
                                                                            <dx:ASPxLabel ID="DG_FECHA" runat="server" Font-Size="11px" />
                                                                        </dx:LayoutItemNestedControlContainer>
                                                                    </LayoutItemNestedControlCollection>
                                                                </dx:LayoutItem>
                                                                <dx:LayoutItem Caption="MEDIO DE TRANSPORTE SALIDA">                                                                    
                                                                    <CaptionStyle CssClass="background_texto_btn"/>
                                                                    <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                                    <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                                    <%--<ParentContainerStyle BackColor="#E8E8E8" Paddings-PaddingLeft="15px" Paddings-PaddingRight="15px" Paddings-PaddingTop="5px"/>--%>
                                                                    <LayoutItemNestedControlCollection>
                                                                        <dx:LayoutItemNestedControlContainer>
                                                                            <dx:ASPxLabel ID="DG_MEDIO_TRANSPORTE_SALIDA" runat="server" Font-Size="11px" />
                                                                        </dx:LayoutItemNestedControlContainer>
                                                                    </LayoutItemNestedControlCollection>
                                                                </dx:LayoutItem>
                                                                <dx:LayoutItem Caption="TIPO DE OPERACIÓN">
                                                                    <CaptionStyle CssClass="background_texto_btn"/>
                                                                    <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                                    <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                                    <%--<ParentContainerStyle BackColor="#E8E8E8" Paddings-PaddingLeft="15px" />--%>
                                                                    <LayoutItemNestedControlCollection>
                                                                        <dx:LayoutItemNestedControlContainer>
                                                                            <dx:ASPxLabel ID="DG_TIPO_OPERACION" runat="server" Font-Size="11px" />
                                                                        </dx:LayoutItemNestedControlContainer>
                                                                    </LayoutItemNestedControlCollection>
                                                                </dx:LayoutItem>
                                                                <dx:LayoutItem Caption="MEDIO DE TRANSPORTE SALIDA DESC.">
                                                                    <CaptionStyle CssClass="background_texto_btn"/>
                                                                    <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                                    <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                                    <%--<ParentContainerStyle BackColor="#E8E8E8" Paddings-PaddingRight="15px" />--%>
                                                                    <LayoutItemNestedControlCollection>
                                                                        <dx:LayoutItemNestedControlContainer>
                                                                            <dx:ASPxLabel ID="DG_MEDIO_RANSPORTE_SALIDA_DESC" runat="server" Font-Size="11px" />
                                                                        </dx:LayoutItemNestedControlContainer>
                                                                    </LayoutItemNestedControlCollection>
                                                                </dx:LayoutItem>
                                                                <dx:LayoutItem Caption="TIPO DE PEDIMENTO">
                                                                    <CaptionStyle CssClass="background_texto_btn"/>
                                                                    <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                                    <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                                    <%--<ParentContainerStyle BackColor="#E8E8E8" Paddings-PaddingLeft="15px" />--%>
                                                                    <LayoutItemNestedControlCollection>
                                                                        <dx:LayoutItemNestedControlContainer>
                                                                            <dx:ASPxLabel ID="DG_TIPO_PEDIMENTO" runat="server" Font-Size="11px" />
                                                                        </dx:LayoutItemNestedControlContainer>
                                                                    </LayoutItemNestedControlCollection>
                                                                </dx:LayoutItem>
                                                                <dx:LayoutItem Caption="MEDIO DE TRANSPORTE ARRIBO">
                                                                    <CaptionStyle CssClass="background_texto_btn"/>
                                                                    <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                                    <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                                    <%--<ParentContainerStyle BackColor="#E8E8E8" Paddings-PaddingRight="15px" />--%>
                                                                    <LayoutItemNestedControlCollection>
                                                                        <dx:LayoutItemNestedControlContainer>
                                                                            <dx:ASPxLabel ID="DG_MEDIO_TRANSPORTE_ARRIBO" runat="server" Font-Size="11px" />
                                                                        </dx:LayoutItemNestedControlContainer>
                                                                    </LayoutItemNestedControlCollection>
                                                                </dx:LayoutItem>
                                                                <dx:LayoutItem Caption="DESCRIPCIÓN TIPO DE OPERACIÓN">
                                                                    <CaptionStyle CssClass="background_texto_btn"/>
                                                                    <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                                    <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                                    <%--<ParentContainerStyle BackColor="#E8E8E8" Paddings-PaddingLeft="15px" />--%>
                                                                    <LayoutItemNestedControlCollection>
                                                                        <dx:LayoutItemNestedControlContainer>
                                                                            <dx:ASPxLabel ID="DG_DESCRIPCION_TIPO_OPERACION" runat="server" Font-Size="11px" />
                                                                        </dx:LayoutItemNestedControlContainer>
                                                                    </LayoutItemNestedControlCollection>
                                                                </dx:LayoutItem>
                                                                <dx:LayoutItem Caption="MEDIO DE TRANSPORTE ARRIBO DESC.">
                                                                    <CaptionStyle CssClass="background_texto_btn"/>
                                                                    <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                                    <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                                    <%--<ParentContainerStyle BackColor="#E8E8E8" Paddings-PaddingRight="15px" />--%>
                                                                    <LayoutItemNestedControlCollection>
                                                                        <dx:LayoutItemNestedControlContainer>
                                                                            <dx:ASPxLabel ID="DG_MEDIO_TRANSPORTE_ARRIBO_DESC" runat="server" Font-Size="11px" />
                                                                        </dx:LayoutItemNestedControlContainer>
                                                                    </LayoutItemNestedControlCollection>
                                                                </dx:LayoutItem>
                                                                <dx:LayoutItem Caption="CLAVE PEDIMENTO">
                                                                    <CaptionStyle CssClass="background_texto_btn"/>
                                                                    <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                                    <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                                    <%--<ParentContainerStyle BackColor="#E8E8E8" Paddings-PaddingLeft="15px" />--%>
                                                                    <LayoutItemNestedControlCollection>
                                                                        <dx:LayoutItemNestedControlContainer>
                                                                            <dx:ASPxLabel ID="DG_CLAVE_PEDIMENTO" runat="server" Font-Size="11px" />
                                                                        </dx:LayoutItemNestedControlContainer>
                                                                    </LayoutItemNestedControlCollection>
                                                                </dx:LayoutItem>
                                                                <dx:LayoutItem Caption="MEDIO DE TRANSPORTE ENTRADA">
                                                                    <CaptionStyle CssClass="background_texto_btn"/>
                                                                    <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                                    <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                                    <%--<ParentContainerStyle BackColor="#E8E8E8" Paddings-PaddingRight="15px" />--%>
                                                                    <LayoutItemNestedControlCollection>
                                                                        <dx:LayoutItemNestedControlContainer>
                                                                            <dx:ASPxLabel ID="DG_MEDIO_TRANSPORTE_ENTRADA" runat="server" Font-Size="11px" />
                                                                        </dx:LayoutItemNestedControlContainer>
                                                                    </LayoutItemNestedControlCollection>
                                                                </dx:LayoutItem>
                                                                <dx:LayoutItem Caption="DESCRIPCIÓN DE LA CLAVE PEDIMENTO">
                                                                    <CaptionStyle CssClass="background_texto_btn"/>
                                                                    <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                                    <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                                    <%--<ParentContainerStyle BackColor="#E8E8E8" Paddings-PaddingLeft="15px" />--%>
                                                                    <LayoutItemNestedControlCollection>
                                                                        <dx:LayoutItemNestedControlContainer>
                                                                            <dx:ASPxLabel ID="DG_DESCRIPCION_CLAVE_PEDIMENTO" runat="server" Font-Size="11px" />
                                                                        </dx:LayoutItemNestedControlContainer>
                                                                    </LayoutItemNestedControlCollection>
                                                                </dx:LayoutItem>
                                                                <dx:LayoutItem Caption="MEDIO DE TRANSPORTE ENTRADA DESC.">
                                                                    <CaptionStyle CssClass="background_texto_btn"/>
                                                                    <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                                    <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                                    <%--<ParentContainerStyle BackColor="#E8E8E8" Paddings-PaddingRight="15px" />--%>
                                                                    <LayoutItemNestedControlCollection>
                                                                        <dx:LayoutItemNestedControlContainer>
                                                                            <dx:ASPxLabel ID="DG_MEDIO_TRANSPORTE_ENTRADA_DESC" runat="server" Font-Size="11px" />
                                                                        </dx:LayoutItemNestedControlContainer>
                                                                    </LayoutItemNestedControlCollection>
                                                                </dx:LayoutItem>
                                                                <dx:LayoutItem Caption="CLAVE PAÍS DESTINO">
                                                                    <CaptionStyle CssClass="background_texto_btn"/>
                                                                    <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                                    <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                                    <%--<ParentContainerStyle BackColor="#E8E8E8" Paddings-PaddingLeft="15px" />--%>
                                                                    <LayoutItemNestedControlCollection>
                                                                        <dx:LayoutItemNestedControlContainer>
                                                                            <dx:ASPxLabel ID="DG_CLAVE_PAIS_DESTINO" runat="server" Font-Size="11px" />
                                                                        </dx:LayoutItemNestedControlContainer>
                                                                    </LayoutItemNestedControlCollection>
                                                                </dx:LayoutItem>
                                                                <dx:LayoutItem Caption="CURP APODERADO MANDATARIO">
                                                                    <CaptionStyle CssClass="background_texto_btn"/>
                                                                    <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                                    <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                                    <%--<ParentContainerStyle BackColor="#E8E8E8" Paddings-PaddingRight="15px" />--%>
                                                                    <LayoutItemNestedControlCollection>
                                                                        <dx:LayoutItemNestedControlContainer>
                                                                            <dx:ASPxLabel ID="DG_CURP_APODERADO" runat="server" Font-Size="11px" />
                                                                        </dx:LayoutItemNestedControlContainer>
                                                                    </LayoutItemNestedControlCollection>
                                                                </dx:LayoutItem>
                                                                <dx:LayoutItem Caption="PAÍS DESTINO">
                                                                    <CaptionStyle CssClass="background_texto_btn"/>
                                                                    <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                                    <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                                    <%--<ParentContainerStyle BackColor="#E8E8E8" Paddings-PaddingLeft="15px" />--%>
                                                                    <LayoutItemNestedControlCollection>
                                                                        <dx:LayoutItemNestedControlContainer>
                                                                            <dx:ASPxLabel ID="DG_PAIS_DESTINO" runat="server" Font-Size="11px" />
                                                                        </dx:LayoutItemNestedControlContainer>
                                                                    </LayoutItemNestedControlCollection>
                                                                </dx:LayoutItem>
                                                                <dx:LayoutItem Caption="RFC AGENTE ADUANAL">
                                                                    <CaptionStyle CssClass="background_texto_btn"/>
                                                                    <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                                    <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                                    <%--<ParentContainerStyle BackColor="#E8E8E8" Paddings-PaddingRight="15px" />--%>
                                                                    <LayoutItemNestedControlCollection>
                                                                        <dx:LayoutItemNestedControlContainer>
                                                                            <dx:ASPxLabel ID="DG_RFC_AGENTE_ADUANAL" runat="server" Font-Size="11px" />
                                                                        </dx:LayoutItemNestedControlContainer>
                                                                    </LayoutItemNestedControlCollection>
                                                                </dx:LayoutItem>
                                                                <dx:LayoutItem Caption="CLAVE ADUANA">
                                                                    <CaptionStyle CssClass="background_texto_btn"/>
                                                                    <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                                    <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                                    <%--<ParentContainerStyle BackColor="#E8E8E8" Paddings-PaddingLeft="15px" />--%>
                                                                    <LayoutItemNestedControlCollection>
                                                                        <dx:LayoutItemNestedControlContainer>
                                                                            <dx:ASPxLabel ID="DG_CLAVE_ADUANA" runat="server" Font-Size="11px" />
                                                                        </dx:LayoutItemNestedControlContainer>
                                                                    </LayoutItemNestedControlCollection>
                                                                </dx:LayoutItem>
                                                                <dx:LayoutItem Caption="VALOR DOLARES">
                                                                    <CaptionStyle CssClass="background_texto_btn"/>
                                                                    <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                                    <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                                    <%--<ParentContainerStyle BackColor="#E8E8E8" Paddings-PaddingRight="15px" />--%>
                                                                    <LayoutItemNestedControlCollection>
                                                                        <dx:LayoutItemNestedControlContainer>
                                                                            <dx:ASPxLabel ID="DG_VALOR_DOLARES" runat="server" Font-Size="11px" />
                                                                        </dx:LayoutItemNestedControlContainer>
                                                                    </LayoutItemNestedControlCollection>
                                                                </dx:LayoutItem>
                                                                <dx:LayoutItem Caption="ADUANA">
                                                                    <CaptionStyle CssClass="background_texto_btn"/>
                                                                    <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                                    <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                                    <%--<ParentContainerStyle BackColor="#E8E8E8" Paddings-PaddingLeft="15px" />--%>
                                                                    <LayoutItemNestedControlCollection>
                                                                        <dx:LayoutItemNestedControlContainer>
                                                                            <dx:ASPxLabel ID="DG_ADUANA" runat="server" Font-Size="11px" />
                                                                        </dx:LayoutItemNestedControlContainer>
                                                                    </LayoutItemNestedControlCollection>
                                                                </dx:LayoutItem>
                                                                <dx:LayoutItem Caption="VALOR ADUANAL">
                                                                    <CaptionStyle CssClass="background_texto_btn"/>
                                                                    <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                                    <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                                    <%--<ParentContainerStyle BackColor="#E8E8E8" Paddings-PaddingRight="15px" />--%>
                                                                    <LayoutItemNestedControlCollection>
                                                                        <dx:LayoutItemNestedControlContainer>
                                                                            <dx:ASPxLabel ID="DG_VALOR_ADUANAL" runat="server" Font-Size="11px" />
                                                                        </dx:LayoutItemNestedControlContainer>
                                                                    </LayoutItemNestedControlCollection>
                                                                </dx:LayoutItem>
                                                                <dx:LayoutItem Caption="PESO BRUTO">
                                                                    <CaptionStyle CssClass="background_texto_btn"/>
                                                                    <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                                    <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                                    <ParentContainerStyle Paddings-PaddingBottom="10px" />
                                                                    <LayoutItemNestedControlCollection>
                                                                        <dx:LayoutItemNestedControlContainer>
                                                                            <dx:ASPxLabel ID="DG_PESO_BRUTO" runat="server" Font-Size="11px" />
                                                                        </dx:LayoutItemNestedControlContainer>
                                                                    </LayoutItemNestedControlCollection>
                                                                </dx:LayoutItem>
                                                                <dx:LayoutItem Caption="VALOR COMERCIAL">
                                                                    <CaptionStyle CssClass="background_texto_btn"/>
                                                                    <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                                    <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                                    <ParentContainerStyle Paddings-PaddingBottom="10px" />
                                                                    <LayoutItemNestedControlCollection>
                                                                        <dx:LayoutItemNestedControlContainer>
                                                                            <dx:ASPxLabel ID="DG_VALOR_COMERCIAL" runat="server" Font-Size="11px" />
                                                                        </dx:LayoutItemNestedControlContainer>
                                                                    </LayoutItemNestedControlCollection>
                                                                </dx:LayoutItem>                                                                                                                                                                                     
                                                            </Items>
                                                        </dx:LayoutGroup>
                                                    </Items>
                                                </dx:ASPxFormLayout>
                                                <%--<dx:ASPxVerticalGrid ID="VerticalGridDG" runat="server" KeyFieldName="PEDIMENTOARMADO" Width="100%"
                                                    Styles-Header-BackColor="#73C000" Styles-Header-ForeColor="#FFFFFF" Styles-Header-CssClass="fondo_vertical_grid" 
                                                    Theme="SoftOrange" Font-Size="10px" BorderTop-BorderWidth="1px">
                                                    <SettingsBehavior AllowSort="false" />
                                                    <Rows>
                                                    <dx:VerticalGridTextRow Caption="PEDIMENTOARMADO:" FieldName="PEDIMENTOARMADO" />            
                                                    <dx:VerticalGridTextRow Caption="FECHA:" FieldName="FECHA" />                                                                                                        
                                                    <dx:VerticalGridTextRow Caption="TIPO DE OPERACIÓN:" FieldName="TIPO DE OPERACION" />
                                                    <dx:VerticalGridTextRow Caption="TIPO DE PEDIMENTO:" FieldName="TIPO DE PEDIMENTO" />
                                                    <dx:VerticalGridDateRow Caption="DESCRIPCIÓN TIPO DE OPERACIÓN:" FieldName="DESCRIPCION TIPO DE OPERACION" />
                                                    <dx:VerticalGridTextRow Caption="CLAVE PEDIMENTO:" FieldName="CLAVE PEDIMENTO" />
                                                    <dx:VerticalGridTextRow Caption="DESCRIPCIÓN DE LA CLAVE PEDIMENTO:" FieldName="DESCRIPCION DE LA CLAVE PEDIMENTO" />
                                                    <dx:VerticalGridTextRow Caption="CLAVE PAÍS DESTINO:" FieldName="CLAVE PAIS DESTINO" />
                                                    <dx:VerticalGridTextRow Caption="PAÍS DESTINO:" FieldName="PAIS DESTINO" />
                                                    <dx:VerticalGridTextRow Caption="CLAVE ADUANA:" FieldName="CLAVE ADUANA" />
                                                    <dx:VerticalGridTextRow Caption="ADUANA:" FieldName="ADUANA" />
                                                    <dx:VerticalGridTextRow Caption="PESO BRUTO:" FieldName="PESO BRUTO" />
                                                    <dx:VerticalGridTextRow Caption="TIPO DE CAMBIO:" FieldName="TIPO DE CAMBIO" />
                                                    <dx:VerticalGridTextRow Caption="MEDIO DE TRANSPORTE SALIDA:" FieldName="MEDIO DE TRANSPORTE SALIDA" />
                                                    <dx:VerticalGridTextRow Caption="MEDIO DE TRANSPORTE SALIDA DESCRIPCIÓN:" FieldName="MEDIO DE TRANSPORTE SALIDA DESCRIPCION" />
                                                    <dx:VerticalGridTextRow Caption="MEDIO DE TRANSPORTE ARRIBO:" FieldName="MEDIO DE TRANSPORTE ARRIBO" />
                                                    <dx:VerticalGridTextRow Caption="MEDIO DE TRANSPORTE ARRIBO DESCRIPCIÓN:" FieldName="MEDIO DE TRANSPORTE ARRIBO DESCRIPCION" />
                                                    <dx:VerticalGridTextRow Caption="MEDIO DE TRANSPORTE ENTRADA:" FieldName="MEDIO DE TRANSPORTE ENTRADA" />
                                                    <dx:VerticalGridTextRow Caption="MEDIO DE TRANSPORTE ENTRADA DESCRIPCIÓN:" FieldName="MEDIO DE TRANSPORTE ENTRADA DESCRIPCION" />
                                                    <dx:VerticalGridTextRow Caption="CURP APODERADO MANDATARIO:" FieldName="CURP APODERADO MANDATARIO" />
                                                    <dx:VerticalGridTextRow Caption="RFC AGENTE ADUANAL:" FieldName="RFC AGENTE ADUANAL" />
                                                    <dx:VerticalGridTextRow Caption="VALOR DOLARES:" FieldName="VALOR DOLARES" />
                                                    <dx:VerticalGridTextRow Caption="VALOR ADUANAL:" FieldName="VALOR ADUANAL" />
                                                    <dx:VerticalGridTextRow Caption="VALOR COMERCIAL:" FieldName="VALOR COMERCIAL" />
                                                    
                                                </Rows>
                                                    <Settings RecordWidth="500" HeaderAreaWidth="270" HorizontalScrollBarMode="Auto" />
                                                    <SettingsPager PageSize="100" />                                                                                                
                                                    <Styles>
                                                        <Record HorizontalAlign="Left" />
                                                    </Styles>
                                                </dx:ASPxVerticalGrid>--%>                                                
                                            </div>
                                        </dx:ContentControl>
                                    </ContentCollection>
                                 </dx:TabPage>
                                 <dx:TabPage Text="Importador/Exportador">
                                    <ContentCollection>
                                        <dx:ContentControl ID="CC_Importador_Exportador" runat="server">
                                            <div class="row" style="overflow:auto; height:300px">
                                                <dx:ASPxFormLayout runat="server" ID="ASPxFormLayoutIE" Font-Size="11px">
                                                    <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit" SwitchToSingleColumnAtWindowInnerWidth="1200" />
                                                    <Items>
                                                        <dx:LayoutGroup Caption="" ColCount="2" GroupBoxDecoration="None" UseDefaultPaddings="false" BackColor="#E8E8E8" >
                                                            <GroupBoxStyle>
                                                                <Caption Font-Bold="true" />
                                                            </GroupBoxStyle>
                                                            <ParentContainerStyle BackColor="#E8E8E8" CssClass="bordes_curvos">
                                                                <Paddings PaddingLeft="10px" PaddingRight="10px" PaddingTop="10px" />
                                                            </ParentContainerStyle>
                                                            <Items>                                                                
                                                                <dx:LayoutItem Caption="IE RFC" >
                                                                    <CaptionStyle CssClass="background_texto_btn"  />
                                                                    <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                                    <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />                                                                
                                                                    <LayoutItemNestedControlCollection>
                                                                        <dx:LayoutItemNestedControlContainer>
                                                                            <dx:ASPxLabel ID="IE_RFC" runat="server" Font-Size="11px" />                                                                            
                                                                        </dx:LayoutItemNestedControlContainer>
                                                                    </LayoutItemNestedControlCollection>
                                                                </dx:LayoutItem>
                                                                <dx:LayoutItem Caption="EMBALAJES">                                                                    
                                                                    <CaptionStyle CssClass="background_texto_btn" />
                                                                    <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                                    <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                                    <LayoutItemNestedControlCollection>
                                                                        <dx:LayoutItemNestedControlContainer>
                                                                            <dx:ASPxLabel ID="IE_EMBALAJES" runat="server" Font-Size="11px" />
                                                                        </dx:LayoutItemNestedControlContainer>
                                                                    </LayoutItemNestedControlCollection>
                                                                </dx:LayoutItem>
                                                                <dx:LayoutItem Caption="IE CURP">                                                                    
                                                                    <CaptionStyle CssClass="background_texto_btn" />
                                                                    <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                                    <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                                    <LayoutItemNestedControlCollection>
                                                                        <dx:LayoutItemNestedControlContainer>
                                                                            <dx:ASPxLabel ID="IE_CURP" runat="server" Font-Size="11px" />
                                                                        </dx:LayoutItemNestedControlContainer>
                                                                    </LayoutItemNestedControlCollection>
                                                                </dx:LayoutItem>
                                                                <dx:LayoutItem Caption="INCREMENTABLES">                                                                    
                                                                    <CaptionStyle CssClass="background_texto_btn" />
                                                                    <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                                    <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                                    <LayoutItemNestedControlCollection>
                                                                        <dx:LayoutItemNestedControlContainer>
                                                                            <dx:ASPxLabel ID="IE_INCREMENTABLES" runat="server" Font-Size="11px" />
                                                                        </dx:LayoutItemNestedControlContainer>
                                                                    </LayoutItemNestedControlCollection>
                                                                </dx:LayoutItem>
                                                                <dx:LayoutItem Caption="IE RAZÓN SOCIAL">
                                                                    <CaptionStyle CssClass="background_texto_btn" />
                                                                    <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                                    <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                                    <LayoutItemNestedControlCollection>
                                                                        <dx:LayoutItemNestedControlContainer>
                                                                            <dx:ASPxLabel ID="IE_RAZON_SOCIAL" runat="server" Font-Size="11px" />
                                                                        </dx:LayoutItemNestedControlContainer>
                                                                    </LayoutItemNestedControlCollection>
                                                                </dx:LayoutItem>
                                                                <dx:LayoutItem Caption="ADUANA DESPACHO">
                                                                    <CaptionStyle CssClass="background_texto_btn" />
                                                                    <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                                    <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                                    <LayoutItemNestedControlCollection>
                                                                        <dx:LayoutItemNestedControlContainer>
                                                                            <dx:ASPxLabel ID="IE_ADUANA_DESPACHO" runat="server" Font-Size="11px" />
                                                                        </dx:LayoutItemNestedControlContainer>
                                                                    </LayoutItemNestedControlCollection>
                                                                </dx:LayoutItem>
                                                                <dx:LayoutItem Caption="IE CALLE">
                                                                    <CaptionStyle CssClass="background_texto_btn" />
                                                                    <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                                    <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                                    <LayoutItemNestedControlCollection>
                                                                        <dx:LayoutItemNestedControlContainer>
                                                                            <dx:ASPxLabel ID="IE_CALLE" runat="server" Font-Size="11px" />
                                                                        </dx:LayoutItemNestedControlContainer>
                                                                    </LayoutItemNestedControlCollection>
                                                                </dx:LayoutItem>
                                                                <dx:LayoutItem Caption="DESCRIPCIÓN ADUANA DESPACHO">
                                                                    <CaptionStyle CssClass="background_texto_btn" />
                                                                    <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                                    <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                                    <LayoutItemNestedControlCollection>
                                                                        <dx:LayoutItemNestedControlContainer>
                                                                            <dx:ASPxLabel ID="IE_DESCRIPCION_ADUANA_DESPACHO" runat="server" Font-Size="11px" />
                                                                        </dx:LayoutItemNestedControlContainer>
                                                                    </LayoutItemNestedControlCollection>
                                                                </dx:LayoutItem>
                                                                <dx:LayoutItem Caption="IE NÚMERO EXTERIOR">
                                                                    <CaptionStyle CssClass="background_texto_btn" />
                                                                    <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                                    <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                                    <%--<ParentContainerStyle BackColor="#E8E8E8" Paddings-PaddingLeft="15px" />--%>
                                                                    <LayoutItemNestedControlCollection>
                                                                        <dx:LayoutItemNestedControlContainer>
                                                                            <dx:ASPxLabel ID="IE_NUMERO_EXTERIOR" runat="server" Font-Size="11px" />
                                                                        </dx:LayoutItemNestedControlContainer>
                                                                    </LayoutItemNestedControlCollection>
                                                                </dx:LayoutItem>
                                                                <dx:LayoutItem Caption="BULTOS">
                                                                    <CaptionStyle CssClass="background_texto_btn" />
                                                                    <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                                    <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                                    <LayoutItemNestedControlCollection>
                                                                        <dx:LayoutItemNestedControlContainer>
                                                                            <dx:ASPxLabel ID="IE_BULTOS" runat="server" Font-Size="11px" />
                                                                        </dx:LayoutItemNestedControlContainer>
                                                                    </LayoutItemNestedControlCollection>
                                                                </dx:LayoutItem>
                                                                <dx:LayoutItem Caption="IE NÚMERO INTERIOR">
                                                                    <CaptionStyle CssClass="background_texto_btn" />
                                                                    <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                                    <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                                    <LayoutItemNestedControlCollection>
                                                                        <dx:LayoutItemNestedControlContainer>
                                                                            <dx:ASPxLabel ID="IE_NUMERO_INTERIOR" runat="server" Font-Size="11px" />
                                                                        </dx:LayoutItemNestedControlContainer>
                                                                    </LayoutItemNestedControlCollection>
                                                                </dx:LayoutItem>
                                                                <dx:LayoutItem Caption="EFECTIVO">
                                                                    <CaptionStyle CssClass="background_texto_btn" />
                                                                    <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                                    <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                                    <LayoutItemNestedControlCollection>
                                                                        <dx:LayoutItemNestedControlContainer>
                                                                            <dx:ASPxLabel ID="IE_EFECTIVO" runat="server" Font-Size="11px" />
                                                                        </dx:LayoutItemNestedControlContainer>
                                                                    </LayoutItemNestedControlCollection>
                                                                </dx:LayoutItem>
                                                                <dx:LayoutItem Caption="IE MUNICIPIO">
                                                                    <CaptionStyle CssClass="background_texto_btn" />
                                                                    <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                                    <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                                    <LayoutItemNestedControlCollection>
                                                                        <dx:LayoutItemNestedControlContainer>
                                                                            <dx:ASPxLabel ID="IE_MUNICIPIO" runat="server" Font-Size="11px" />
                                                                        </dx:LayoutItemNestedControlContainer>
                                                                    </LayoutItemNestedControlCollection>
                                                                </dx:LayoutItem>
                                                                <dx:LayoutItem Caption="OTROS">
                                                                    <CaptionStyle CssClass="background_texto_btn" />
                                                                    <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                                    <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                                    <LayoutItemNestedControlCollection>
                                                                        <dx:LayoutItemNestedControlContainer>
                                                                            <dx:ASPxLabel ID="IE_OTROS" runat="server" Font-Size="11px" />
                                                                        </dx:LayoutItemNestedControlContainer>
                                                                    </LayoutItemNestedControlCollection>
                                                                </dx:LayoutItem>
                                                                <dx:LayoutItem Caption="IE CÓDIGO POSTAL">
                                                                    <CaptionStyle CssClass="background_texto_btn" />
                                                                    <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                                    <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                                    <LayoutItemNestedControlCollection>
                                                                        <dx:LayoutItemNestedControlContainer>
                                                                            <dx:ASPxLabel ID="IE_CODIGO_POSTAL" runat="server" Font-Size="11px" />
                                                                        </dx:LayoutItemNestedControlContainer>
                                                                    </LayoutItemNestedControlCollection>
                                                                </dx:LayoutItem>
                                                                <dx:LayoutItem Caption="TOTAL">
                                                                    <CaptionStyle CssClass="background_texto_btn" />
                                                                    <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                                    <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                                    <LayoutItemNestedControlCollection>
                                                                        <dx:LayoutItemNestedControlContainer>
                                                                            <dx:ASPxLabel ID="IE_TOTAL" runat="server" Font-Size="11px" />
                                                                        </dx:LayoutItemNestedControlContainer>
                                                                    </LayoutItemNestedControlCollection>
                                                                </dx:LayoutItem>
                                                                <dx:LayoutItem Caption="IE PAÍS">
                                                                    <CaptionStyle CssClass="background_texto_btn" />
                                                                    <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                                    <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                                    <LayoutItemNestedControlCollection>
                                                                        <dx:LayoutItemNestedControlContainer>
                                                                            <dx:ASPxLabel ID="IE_PAIS" runat="server" Font-Size="11px" />
                                                                        </dx:LayoutItemNestedControlContainer>
                                                                    </LayoutItemNestedControlCollection>
                                                                </dx:LayoutItem>
                                                                <dx:LayoutItem Caption="CLAVE PAÍS">
                                                                    <CaptionStyle CssClass="background_texto_btn" />
                                                                    <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                                    <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                                    <LayoutItemNestedControlCollection>
                                                                        <dx:LayoutItemNestedControlContainer>
                                                                            <dx:ASPxLabel ID="IE_CLAVE_PAIS" runat="server" Font-Size="11px" />
                                                                        </dx:LayoutItemNestedControlContainer>
                                                                    </LayoutItemNestedControlCollection>
                                                                </dx:LayoutItem>
                                                                <dx:LayoutItem Caption="SEGUROS">
                                                                    <CaptionStyle CssClass="background_texto_btn" />
                                                                    <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                                    <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                                    <LayoutItemNestedControlCollection>
                                                                        <dx:LayoutItemNestedControlContainer>
                                                                            <dx:ASPxLabel ID="IE_SEGUROS" runat="server" Font-Size="11px" />
                                                                        </dx:LayoutItemNestedControlContainer>
                                                                    </LayoutItemNestedControlCollection>
                                                                </dx:LayoutItem>
                                                                <dx:LayoutItem Caption="DESCRIPCIÓN PAÍS">
                                                                    <CaptionStyle CssClass="background_texto_btn" />
                                                                    <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                                    <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                                    <LayoutItemNestedControlCollection>
                                                                        <dx:LayoutItemNestedControlContainer>
                                                                            <dx:ASPxLabel ID="IE_DESCRIPCIÓN_PAIS" runat="server" Font-Size="11px" />
                                                                        </dx:LayoutItemNestedControlContainer>
                                                                    </LayoutItemNestedControlCollection>
                                                                </dx:LayoutItem>
                                                                <dx:LayoutItem Caption="FLETES">
                                                                    <CaptionStyle CssClass="background_texto_btn" />
                                                                    <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                                    <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                                    <ParentContainerStyle BackColor="#E8E8E8" Paddings-PaddingBottom="10px" />
                                                                    <LayoutItemNestedControlCollection>
                                                                        <dx:LayoutItemNestedControlContainer>
                                                                            <dx:ASPxLabel ID="IE_FLETES" runat="server" Font-Size="11px" />
                                                                        </dx:LayoutItemNestedControlContainer>
                                                                    </LayoutItemNestedControlCollection>
                                                                </dx:LayoutItem>
                                                                <dx:LayoutItem Caption=" ">
                                                                    <CaptionStyle CssClass="background_texto_btn" />
                                                                    <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                                    <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                                    <ParentContainerStyle BackColor="#E8E8E8" Paddings-PaddingBottom="10px" />
                                                                    <LayoutItemNestedControlCollection>
                                                                        <dx:LayoutItemNestedControlContainer>
                                                                            <dx:ASPxLabel ID="ASPxLabel1" runat="server" Font-Size="11px" />
                                                                        </dx:LayoutItemNestedControlContainer>
                                                                    </LayoutItemNestedControlCollection>
                                                                </dx:LayoutItem>                                                                                                                                                                                
                                                            </Items>
                                                        </dx:LayoutGroup>
                                                    </Items>
                                                </dx:ASPxFormLayout>

                                                <%--<dx:ASPxVerticalGrid ID="VerticalGridIE" runat="server" KeyFieldName="PEDIMENTOARMADO" Width="100%" Styles-Table-BackColor="#E8E8E8"
                                                    Styles-Header-BackColor="#73C000" Styles-Header-ForeColor="#FFFFFF" Font-Size="10px" BorderTop-BorderWidth="1px"
                                                    Styles-Header-CssClass="fondo_vertical_grid" Theme="SoftOrange" >
                                                    <SettingsBehavior AllowSort="false" />
                                                    <Rows>
                                                        <dx:VerticalGridTextRow Caption="IE RFC:" FieldName="IE_RFC" />            
                                                        <dx:VerticalGridTextRow Caption="IE CURP:" FieldName="IE_CURP" />                                                                                                        
                                                        <dx:VerticalGridTextRow Caption="IE RAZÓN SOCIAL:" FieldName="IE_RAZON_SOCIAL" />
                                                        <dx:VerticalGridDateRow Caption="IE CALLE:" FieldName="IE_CALLE" />
                                                        <dx:VerticalGridTextRow Caption="IE NÚMERO EXTERIOR:" FieldName="IE_NUMERO_EXTERIOR" />
                                                        <dx:VerticalGridTextRow Caption="IE NÚMERO INTERIOR:" FieldName="IE_NUMERO_INTERIOR" />
                                                        <dx:VerticalGridTextRow Caption="IE MUNICIPIO:" FieldName="IE_MUNICIPIO" />
                                                        <dx:VerticalGridTextRow Caption="IE CÓDIGO POSTAL:" FieldName="IE_CODIGO_POSTAL" />
                                                        <dx:VerticalGridTextRow Caption="IE PAÍS:" FieldName="IE_PAIS" />
                                                        <dx:VerticalGridTextRow Caption="SEGUROS:" FieldName="SEGUROS" />
                                                        <dx:VerticalGridTextRow Caption="FLETES:" FieldName="FLETES" />
                                                        <dx:VerticalGridTextRow Caption="EMBALAJES:" FieldName="EMBALAJES" />
                                                        <dx:VerticalGridTextRow Caption="INCREMENTABLES:" FieldName="INCREMENTABLES" />
                                                        <dx:VerticalGridTextRow Caption="ADUANA DESPACHO:" FieldName="ADUANA_DESPACHO" />
                                                        <dx:VerticalGridTextRow Caption="DESCRIPCIÓN ADUANA DESPACHO:" FieldName="DESCRIPCION_ADUANA_DESPACHO" />
                                                        <dx:VerticalGridTextRow Caption="BULTOS:" FieldName="BULTOS" />
                                                        <dx:VerticalGridTextRow Caption="EFECTIVO:" FieldName="EFECTIVO" />
                                                        <dx:VerticalGridTextRow Caption="OTROS:" FieldName="OTROS" />
                                                        <dx:VerticalGridTextRow Caption="TOTAL:" FieldName="TOTAL" />
                                                        <dx:VerticalGridTextRow Caption="CLAVE PAÍS:" FieldName="CLAVE_PAIS" />
                                                        <dx:VerticalGridTextRow Caption="DESCRIPCIÓN PAÍS:" FieldName="DESCRIPCION_PAIS" />
                                                    </Rows>
                                                    <Settings RecordWidth="350" HeaderAreaWidth="200"  HorizontalScrollBarMode="Auto" />
                                                    <SettingsPager PageSize="100" />                                                                                                
                                                    <Styles>
                                                        <Record HorizontalAlign="Left" />
                                                    </Styles>
                                                </dx:ASPxVerticalGrid>     --%>                                           
                                            </div>
                                        </dx:ContentControl>
                                    </ContentCollection>
                                 </dx:TabPage>
                                 <dx:TabPage Text="Compradores/Vendedores">
                                    <ContentCollection>
                                        <dx:ContentControl ID="CC_Compradores_Vendedores" runat="server">
                                            <div class="row" style="overflow:auto; height:300px">
                                                <dx:ASPxVerticalGrid ID="VerticalGridCV" runat="server" KeyFieldName="PEDIMENTOARMADO" Width="100%" 
                                                    Styles-Header-CssClass="background_color_btn background_texto_btn"
                                                    Theme="SoftOrange" Font-Size="10px" BorderTop-BorderWidth="1px">
                                                    <SettingsBehavior AllowSort="false" />
                                                    <Rows>
                                                        <dx:VerticalGridTextRow Caption="ID FISCAL:" FieldName="ID_FISCAL" />            
                                                        <dx:VerticalGridTextRow Caption="NOMBRE:" FieldName="NOMBRE" />                                                                                                        
                                                        <dx:VerticalGridTextRow Caption="CALLE:" FieldName="CALLE" />
                                                        <dx:VerticalGridDateRow Caption="NÚMERO EXTERIOR:" FieldName="NUMERO_EXTERIOR" />
                                                        <dx:VerticalGridTextRow Caption="NÚMERO INTERIOR:" FieldName="NUMERO_INTERIOR" />
                                                        <dx:VerticalGridTextRow Caption="CIUDAD MUNICIPIO:" FieldName="CIUDAD_MUNICIPIO" />
                                                        <dx:VerticalGridTextRow Caption="CÓDIGO POSTAL:" FieldName="CODIGO_POSTAL" />
                                                        <dx:VerticalGridTextRow Caption="PAÍS:" FieldName="PAIS" />
                                                        <dx:VerticalGridTextRow Caption="CLAVE MONEDA:" FieldName="CLAVE_MONEDA" />
                                                        <dx:VerticalGridTextRow Caption="DESCRIPCIÓN<br/>MONEDA:" FieldName="DESCRIPCION_MONEDA" />
                                                        <dx:VerticalGridTextRow Caption="VALOR MONEDA<br/>EXTRANJERA:" FieldName="VALOR_MONEDA_EXTRANJERA" PropertiesTextEdit-DisplayFormatString="{0:n6}" />
                                                        <dx:VerticalGridTextRow Caption="VALOR DOLARES:" FieldName="VALOR_DOLARES" PropertiesTextEdit-DisplayFormatString="{0:n2}" />
                                                        <dx:VerticalGridTextRow Caption="PAÍS CLAVE:" FieldName="PAIS_CLAVE" />
                                                        <dx:VerticalGridTextRow Caption="PAÍS DESCRIPCIÓN:" FieldName="PAIS_DESCRIPCION" />
                                                        <dx:VerticalGridTextRow Caption="PEDIMENTO ORIGINAL:" FieldName="PEDIMENTO_ORIGINAL_(DESCARGOS)" />
                                                    </Rows>
                                                    <Settings RecordWidth="150" HeaderAreaWidth="130" HorizontalScrollBarMode="Auto" />
                                                    <SettingsPager PageSize="100" />                                                                                                
                                                    <Styles>
                                                        <Record HorizontalAlign="Left" />
                                                    </Styles>
                                                </dx:ASPxVerticalGrid>                                                
                                            </div>
                                        </dx:ContentControl>
                                 </ContentCollection>
                                 </dx:TabPage>
                                 <dx:TabPage Text="XML">
                                    <ContentCollection>
                                         <dx:ContentControl ID="CC_XML" runat="server">
                                            <div class="row">                                                                                                                    
                                                 <dx:ASPxFormLayout runat="server" ID="ASPxFormLayout1" Font-Size="11px" Width="100%">
                                                    <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit" SwitchToSingleColumnAtWindowInnerWidth="1200" />
                                                    <Items>
                                                        <dx:LayoutGroup Caption="" ColCount="1" GroupBoxDecoration="None" UseDefaultPaddings="false" SettingsItemCaptions-Location="Top">
                                                            <GroupBoxStyle>
                                                                <Caption Font-Bold="true" />
                                                            </GroupBoxStyle>
                                                            <ParentContainerStyle BackColor="#E8E8E8" CssClass="bordes_curvos" >
                                                                <Paddings Padding="10px" />
                                                            </ParentContainerStyle>                                                    
                                                            <Items>                                                               
                                                                <dx:LayoutItem Caption="&nbsp;&nbsp;&nbsp;&nbsp;XML"  >
                                                                    <ParentContainerStyle BackColor="#E8E8E8" />
                                                                    <CaptionStyle CssClass="background_texto_btn"  />
                                                                    <CaptionCellStyle CssClass="background_color_btn bordes_curvos_top" Paddings-PaddingTop="5px" Paddings-PaddingRight="0px"  />
                                                                    <NestedControlCellStyle Paddings-Padding="10px" BackColor="#F9F9F9" CssClass="bordes_curvos_down" />                                                                                                                                                                                                        
                                                                    <LayoutItemNestedControlCollection>
                                                                        <dx:LayoutItemNestedControlContainer>
                                                                            <div class="row col-xs-12" style="margin-left:3px; height:300px; width:100%;" >
                                                                                <dx:ASPxMemo ID="MemoXML" runat="server" Width="100%" Height="100%" ReadOnly="true" CssClass="bordes_curvos" ValidateRequestMode="Disabled" >                                                                                    
                                                                                </dx:ASPxMemo>
                                                                            </div>                                                                          
                                                                        </dx:LayoutItemNestedControlContainer>
                                                                    </LayoutItemNestedControlCollection>
                                                                    <CaptionSettings />
                                                                </dx:LayoutItem>                                                                                                                                                                                                                                                     
                                                            </Items>
                                                        </dx:LayoutGroup>
                                                    </Items>
                                                 </dx:ASPxFormLayout>
                                                 <%--<dx:BootstrapFormLayout ID="BootstrapFormLayout_XML" runat="server" LayoutType="Vertical" Width="100%" CssClasses-Control="bordes_curvos"  >
                                                 <Items>
                                                     <dx:BootstrapLayoutItem Caption="XML" ColSpanXs="12" ColSpanSm="12" ColSpanMd="12" ColSpanLg="12" >
                                                         <ContentCollection>
                                                             <dx:ContentControl> 
                                                                 <dx:ASPxLabel ID="ASPxLabelXML" runat="server" Width="100%" CssClass="label_radius" />                                           
                                                             </dx:ContentControl>
                                                         </ContentCollection>
                                                     </dx:BootstrapLayoutItem>                                                                                                   
                                                 </Items>
                                                 </dx:BootstrapFormLayout>--%>
                                             </div>
                                         </dx:ContentControl>
                                     </ContentCollection>
                                 </dx:TabPage>                            
                                 <dx:TabPage Text="Identificadores">
                                    <ContentCollection>
                                        <dx:ContentControl ID="CC_Identificadores" runat="server">
                                            <div class="row col-sx-12 col-sm-12 col-md-5 col-lg-5">
                                                <dx:ASPxGridView ID="detailGridIdentificadores" runat="server" KeyFieldName="PEDIMENTOARMADO" Theme="SoftOrange"  Width="100%" Styles-Header-Font-Size="11px">
                                                    <Columns>                                                                                                    
                                                        <dx:GridViewDataTextColumn Caption="CLAVE" FieldName="CLAVE" ReadOnly="True" VisibleIndex="1" Width="10%" >
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <CellStyle HorizontalAlign="Center" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="DESCRIPCIÓN" FieldName="DESCRIPCION" ReadOnly="True" VisibleIndex="2" Width="60%">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="COMPLEMENTO 1" FieldName="COMPLEMENTO_1" ReadOnly="True" VisibleIndex="3" Width="30%">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </dx:GridViewDataTextColumn>
                                                        <%--<dx:GridViewDataTextColumn Caption="STATUS" FieldName="" ReadOnly="True" VisibleIndex="4" Width="80px">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="PDF" FieldName="" ReadOnly="True" VisibleIndex="5" Width="200px">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </dx:GridViewDataTextColumn>--%>                                                                                                
                                                    </Columns>
                                                    <SettingsBehavior AllowSelectByRowClick="true" AllowSelectSingleRowOnly="true" />                                                                                                
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
                                                </dx:ASPxGridView>
                                            </div>
                                        </dx:ContentControl>
                                    </ContentCollection>
                                 </dx:TabPage>
                                 <dx:TabPage Text="Fletes">
                                    <ContentCollection>
                                        <dx:ContentControl ID="CC_Fletes" runat="server">
                                           <div class="row col-sx-12 col-sm-12 col-md-5 col-lg-5">
                                                <dx:ASPxGridView ID="detailGridFletes" runat="server" KeyFieldName="PEDIMENTOARMADO" Theme="SoftOrange" Width="100%" Styles-Header-Font-Size="11px">
                                                    <Columns>                                                                                                    
                                                        <dx:GridViewDataTextColumn Caption="PROVEEDOR" FieldName="PROVEEDOR" ReadOnly="True" VisibleIndex="1" Width="40%" >
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="FACTURA" FieldName="FACTURA" ReadOnly="True" VisibleIndex="2" Width="30%">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="IMPORTE" FieldName="IMPORTE" ReadOnly="True" VisibleIndex="3" Width="30%">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <CellStyle HorizontalAlign="Right" />
                                                            <PropertiesTextEdit DisplayFormatString="{0:n10}" />
                                                        </dx:GridViewDataTextColumn>                                                                                                                                                                                                   
                                                    </Columns>
                                                    <SettingsBehavior AllowSelectByRowClick="true" AllowSelectSingleRowOnly="true" />                                                                                                
                                                    <Styles>                                                               
                                                    <SelectedRow CssClass="background_color_btn background_texto_btn" />
                                                    <Row Font-Size="11px"  />
                                                    <AlternatingRow Enabled="True" />
                                                    <PagerTopPanel Paddings-PaddingBottom="3px">
                                                        <Paddings PaddingBottom="3px" />
                                                        </PagerTopPanel>
                                                    </Styles>
                                                    <SettingsPager Position="TopAndBottom" PageSize="10">
                                                    </SettingsPager>                                                                                               
                                                </dx:ASPxGridView>
                                            </div>
                                        </dx:ContentControl>
                                    </ContentCollection>
                                 </dx:TabPage>
                                 <dx:TabPage Text="Incrementables">
                                     <ContentCollection>
                                         <dx:ContentControl ID="ContentControl6" runat="server">
                                             <div class="row">
                                                 <dx:ASPxGridView ID="GridIn" runat="server" EnableTheming="True" Theme="SoftOrange"  Width="100%" Styles-Header-Font-Size="11px"
                                                     Settings-HorizontalScrollBarMode="Auto" KeyFieldName="(None)">
                                                     <Columns>                                                                                                    
                                                        <dx:GridViewDataTextColumn Caption="VALOR <BR/> SEGUROS" FieldName="VALOR_SEGUROS" ReadOnly="True" VisibleIndex="1" Width="100px">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <CellStyle HorizontalAlign="Right" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="FLETES" FieldName="FLETES" ReadOnly="True" VisibleIndex="2" Width="90px" AdaptivePriority="1">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <CellStyle HorizontalAlign="Right" />
                                                            <PropertiesTextEdit DisplayFormatString="{0:n2}" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="SEGUROS" FieldName="SEGUROS" ReadOnly="True" VisibleIndex="3" Width="90px" AdaptivePriority="1">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <CellStyle HorizontalAlign="Right" />
                                                            <PropertiesTextEdit DisplayFormatString="{0:n2}" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="EMBALAJES" FieldName="EMBALAJES" ReadOnly="True" VisibleIndex="4" Width="100px" AdaptivePriority="1">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <CellStyle HorizontalAlign="Right" />
                                                            <PropertiesTextEdit DisplayFormatString="{0:n2}" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="OTROS <BR/> INCREMENTABLES" FieldName="OTROS_INCREMENTABLES" ReadOnly="True" VisibleIndex="5" Width="120px" AdaptivePriority="1">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <CellStyle HorizontalAlign="Right" />
                                                            <PropertiesTextEdit DisplayFormatString="{0:n2}" />
                                                        </dx:GridViewDataTextColumn>                                                                                                
                                                    </Columns>
                                                    <SettingsBehavior AllowSelectByRowClick="true" AllowSelectSingleRowOnly="true" />
                                                    <%--<SettingsAdaptivity AdaptivityMode="HideDataCellsWindowLimit" HideDataCellsAtWindowInnerWidth="800" 
                                                        AdaptiveDetailColumnCount="1" AllowOnlyOneAdaptiveDetailExpanded="True">
                                                        <AdaptiveDetailLayoutProperties colcount="1">
                                                            <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit"  SwitchToSingleColumnAtWindowInnerWidth="600" />
                                                        </AdaptiveDetailLayoutProperties>
                                                    </SettingsAdaptivity> 
                                                    <SettingsResizing ColumnResizeMode="Control" />--%>
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
                                                 </dx:ASPxGridView>
                                             </div>
                                         </dx:ContentControl>
                                     </ContentCollection>
                                 </dx:TabPage>
                                 <dx:TabPage Text="Facturas">
                                     <ContentCollection>
                                         <dx:ContentControl ID="ContentControl7" runat="server">
                                         <div class="row">
                                             <dx:ASPxGridView ID="GridFacturas" runat="server" EnableTheming="True" Theme="SoftOrange" Styles-Header-Font-Size="11px" 
                                                 OnCustomCallback="Grid_CustomCallbackFACTURAS" Settings-HorizontalScrollBarMode="Auto" Width="100%" KeyFieldName="(None)" >
                                                <Columns>
                                                    <dx:GridViewDataTextColumn Caption="NÚMERO" FieldName="NUMERO" ReadOnly="True" VisibleIndex="1" Width="130px" >
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <CellStyle HorizontalAlign="Center" />
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataDateColumn Caption="FECHA" FieldName="FECHA" ReadOnly="True" VisibleIndex="2" Width="100px">
                                                        <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy"></PropertiesDateEdit>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <CellStyle HorizontalAlign="Center" />
                                                    </dx:GridViewDataDateColumn>
                                                    <dx:GridViewDataTextColumn Caption="TERMINO <br/> FACTURA" FieldName="TERMINO_FACTURA" ReadOnly="True" VisibleIndex="3" Width="90px">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <CellStyle HorizontalAlign="Center" />
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="TERMINO FACTURA <br/> DESCIRPCIÓN" FieldName="TERMINO_FACTURA_DESCIRPCION" ReadOnly="True" VisibleIndex="4" Width="400px">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="MONEDA" FieldName="MONEDA" ReadOnly="True" VisibleIndex="5" Width="90px">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="MONEDA <br/> NOMBRE" FieldName="MONEDA_NOMBRE" ReadOnly="True" VisibleIndex="6" Width="120px">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="VALOR <br/> DOLARES" FieldName="VALOR_DOLARES" ReadOnly="True" VisibleIndex="12" Width="100px">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <CellStyle HorizontalAlign="Right" />
                                                        <PropertiesTextEdit DisplayFormatString="{0:n2}" />
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="VALOR MONEDA <br/> EXTRANJERA" FieldName="VALOR_MONEDA_EXTRANJERA" ReadOnly="True" VisibleIndex="13" Width="120px">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <CellStyle HorizontalAlign="Right" />
                                                        <PropertiesTextEdit DisplayFormatString="{0:n4}" />
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="TAX ID <br/> FACTURA" FieldName="TAX_ID_FACTURA" ReadOnly="True" VisibleIndex="14" Width="120px">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <CellStyle HorizontalAlign="Center" />
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="PROVEEDOR <br/> COMPRADOR" FieldName="PROVEEDOR_COMPRADOR" ReadOnly="True" VisibleIndex="15" Width="290px">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        
                                                    </dx:GridViewDataTextColumn>
                                                </Columns>                                                                                              
                                                 <SettingsBehavior AllowSelectByRowClick="true" AllowSelectSingleRowOnly="true" />
                                                 <Styles>                                                               
                                                 <SelectedRow CssClass="background_color_btn background_texto_btn" />
                                                 <Row Font-Size="11px"  />
                                                 <AlternatingRow Enabled="True" />
                                                 <PagerTopPanel Paddings-PaddingBottom="3px">
                                                     <Paddings PaddingBottom="3px" />
                                                     </PagerTopPanel>
                                                 </Styles>                                                                             
                                                 <SettingsPager Position="TopAndBottom" PageSize="10">
                                                 </SettingsPager>                                                                                               
                                             </dx:ASPxGridView>                                                                        
                                             <%--<dx:ASPxGridViewExporter ID="ExporterFA" GridViewID="GridFacturas" runat="server" PaperKind="A5" Landscape="true" />--%>
                                         </div>
                                         </dx:ContentControl>
                                     </ContentCollection>
                                 </dx:TabPage>
                                 <dx:TabPage Text="Fechas">
                                     <ContentCollection>
                                         <dx:ContentControl ID="ContentControl8" runat="server">
                                             <div class="row">                                                                                                                              
                                                 <dx:ASPxGridView ID="GridFechas" runat="server" EnableTheming="True" Theme="SoftOrange" Settings-HorizontalScrollBarMode="Auto" 
                                                     OnCustomCallback="Grid_CustomCallbackFECHAS" Width="100%" Styles-Header-Font-Size="11px" KeyFieldName="(None)"> 
                                                     <Columns>
                                                         <dx:GridViewDataTextColumn Caption="DESCRIPCIÓN DE <br/> LA CLAVE" FieldName="DESCRIPCION" ReadOnly="True" VisibleIndex="1" Width="300px">
                                                             <HeaderStyle HorizontalAlign="Center" />
                                                         </dx:GridViewDataTextColumn>
                                                         <dx:GridViewDataDateColumn FieldName="FECHA" Caption="FECHA" ReadOnly="True" VisibleIndex="2" Width="110px">
                                                             <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy"></PropertiesDateEdit>
                                                             <HeaderStyle HorizontalAlign="Center" />
                                                             <CellStyle HorizontalAlign="Center" />
                                                         </dx:GridViewDataDateColumn>
                                                         <dx:GridViewDataTextColumn FieldName="CLAVE_FECHA" Caption="CLAVE <br/> FECHA" VisibleIndex="3" Width="100px">
                                                             <HeaderStyle HorizontalAlign="Center" />
                                                             <CellStyle HorizontalAlign="Center" />
                                                         </dx:GridViewDataTextColumn>
                                                     </Columns>
                                                     <SettingsBehavior AllowSelectByRowClick="true" AllowSelectSingleRowOnly="true" />
                                                     <%--<SettingsAdaptivity AdaptivityMode="HideDataCellsWindowLimit" HideDataCellsAtWindowInnerWidth="800" 
                                                         AdaptiveDetailColumnCount="1" AllowOnlyOneAdaptiveDetailExpanded="True">
                                                         <AdaptiveDetailLayoutProperties colcount="1">
                                                             <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit"  SwitchToSingleColumnAtWindowInnerWidth="600" />
                                                         </AdaptiveDetailLayoutProperties>
                                                     </SettingsAdaptivity> 
                                                     <SettingsResizing ColumnResizeMode="Control" />--%>
                                                     <Styles>                                                               
                                                         <SelectedRow CssClass="background_color_btn background_texto_btn" />
                                                         <Row Font-Size="11px"  />
                                                         <AlternatingRow Enabled="True" />
                                                         <PagerTopPanel Paddings-PaddingBottom="3px">
                                                             <Paddings PaddingBottom="3px" />
                                                         </PagerTopPanel>
                                                     </Styles>
                                                     <SettingsPager Position="TopAndBottom" PageSize="10">
                                                     </SettingsPager>
                                                 </dx:ASPxGridView>
                                                 <%--<dx:ASPxGridViewExporter ID="ExporterFecha" GridViewID="GridFechas" runat="server" PaperKind="A5" Landscape="true" />--%>                                                                                                                        
                                             </div>                                      
                                         </dx:ContentControl>
                                     </ContentCollection>
                                 </dx:TabPage>
                                 <dx:TabPage Text="Contribuciones Generales">
                                     <ContentCollection>
                                         <dx:ContentControl ID="ContentControl9" runat="server">
                                             <div class="row">
                                                 <dx:ASPxGridView ID="GridContriGrales" runat="server" EnableTheming="True" Theme="SoftOrange" Settings-HorizontalScrollBarMode="Auto"
                                                    OnCustomCallback="Grid_CustomCallbackCONTRIGRALES" Width="100%"  Styles-Header-Font-Size="11px" KeyFieldName="(None)" >
                                                    <Columns>
                                                        <dx:GridViewDataTextColumn Caption="CLAVE <br/> CONTRIBUCIÓN" FieldName="CLAVE CONTRIBUCION" ReadOnly="True" VisibleIndex="1" Width="100px" >
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <CellStyle HorizontalAlign="Center" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="DESCRIPCIÓN <br/> CONTRIBUCIÓN" FieldName="CONTRIBUCION DESCRIPCION" ReadOnly="True" VisibleIndex="2" Width="120px" >
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="TASA <br/> CLAVE" FieldName="TASA CLAVE" ReadOnly="True" VisibleIndex="3" Width="100px">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <CellStyle HorizontalAlign="Center" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="TASA <br/> DESCRIPCIÓN" FieldName="TASA DESCRIPCION" ReadOnly="True" VisibleIndex="4" Width="200px">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="TASA <br/> APLICABLE" FieldName="TASA APLICABLE" ReadOnly="True" VisibleIndex="5" Width="100px">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <CellStyle HorizontalAlign="Right" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="FORMA <br/> DE PAGO" FieldName="FROMA DE PAGO" ReadOnly="True" VisibleIndex="6" Width="100px">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <CellStyle HorizontalAlign="Center" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="DESCRIPCIÓN <br/> FORMA DE PAGO" FieldName="DESCRIPCION DE LA FORMA DE PAGO" ReadOnly="True" VisibleIndex="7" Width="140px" >
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="IMPORTE" FieldName="IMPORTE" ReadOnly="True" VisibleIndex="8" Width="100px">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <CellStyle HorizontalAlign="Right" />
                                                        </dx:GridViewDataTextColumn>
                                                    </Columns>
                                                    <SettingsBehavior AllowSelectByRowClick="true" AllowSelectSingleRowOnly="true" />
                                                    <%--<SettingsAdaptivity AdaptivityMode="HideDataCellsWindowLimit" HideDataCellsAtWindowInnerWidth="800" 
                                                        AdaptiveDetailColumnCount="1" AllowOnlyOneAdaptiveDetailExpanded="True">
                                                        <AdaptiveDetailLayoutProperties colcount="1">
                                                            <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit"  SwitchToSingleColumnAtWindowInnerWidth="600" />
                                                        </AdaptiveDetailLayoutProperties>
                                                    </SettingsAdaptivity> 
                                                    <SettingsResizing ColumnResizeMode="Control" />--%>
                                                    <Styles>                                                               
                                                        <SelectedRow CssClass="background_color_btn background_texto_btn" />
                                                        <Row Font-Size="11px"  />
                                                        <AlternatingRow Enabled="True" />
                                                        <PagerTopPanel Paddings-PaddingBottom="3px">
                                                            <Paddings PaddingBottom="3px" />
                                                        </PagerTopPanel>
                                                    </Styles>
                                                    <SettingsPager Position="TopAndBottom" PageSize="10">
                                                    </SettingsPager>                      
                                                </dx:ASPxGridView>
                                                 <%--<dx:ASPxGridViewExporter ID="ExporterContriGrales" GridViewID="GridContriGrales" runat="server" PaperKind="A5" Landscape="true" />    --%>
                                             </div>
                                         </dx:ContentControl>
                                     </ContentCollection>
                                 </dx:TabPage>
                                 <dx:TabPage Text="Impuestos">
                                     <ContentCollection>
                                         <dx:ContentControl ID="ContentControl10" runat="server">
                                             <div class="row">
                                                 <dx:ASPxGridView ID="GridImpuestos" runat="server" EnableTheming="True" Theme="SoftOrange" Settings-HorizontalScrollBarMode="Auto"
                                                     OnCustomCallback="Grid_CustomCallbackIMPUESTOS" Width="100%" Styles-Header-Font-Size="11px" KeyFieldName="(None)">                                                                        
                                                 <Columns>
                                                     <dx:GridViewDataTextColumn Caption="CONTRIBUCIÓN" FieldName="CONTRIBUCION" ReadOnly="True" VisibleIndex="1" Width="100px" >
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                         <CellStyle HorizontalAlign="Center" />
                                                     </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataTextColumn Caption="TASA" FieldName="TASA" ReadOnly="True" VisibleIndex="2" Width="100px">
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                         <CellStyle HorizontalAlign="Center" />
                                                     </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataTextColumn Caption="TIPO DE <BR/> TASA" FieldName="TIPO_DE_TASA" ReadOnly="True" VisibleIndex="3" Width="110px">
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                         <CellStyle HorizontalAlign="Center" />
                                                     </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataTextColumn Caption="FORMA DE <br/> PAGO" FieldName="FORMA_DE_PAGO" ReadOnly="True" VisibleIndex="4" Width="100px">
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                         <CellStyle HorizontalAlign="Center" />
                                                     </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataTextColumn Caption="IMPORTE" FieldName="IMPORTE" ReadOnly="True" VisibleIndex="5" Width="100px">
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                         <CellStyle HorizontalAlign="Right" />
                                                         <PropertiesTextEdit DisplayFormatString="{0:n4}" />
                                                     </dx:GridViewDataTextColumn>
                                                 </Columns>
                                                 <SettingsBehavior AllowSelectByRowClick="true" AllowSelectSingleRowOnly="true" />
                                                 <%--<SettingsAdaptivity AdaptivityMode="HideDataCellsWindowLimit" HideDataCellsAtWindowInnerWidth="800" 
                                                     AdaptiveDetailColumnCount="1" AllowOnlyOneAdaptiveDetailExpanded="True">
                                                     <AdaptiveDetailLayoutProperties colcount="2">
                                                         <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit"  SwitchToSingleColumnAtWindowInnerWidth="600" />
                                                     </AdaptiveDetailLayoutProperties>
                                                 </SettingsAdaptivity> 
                                                 <SettingsResizing ColumnResizeMode="Control" />--%>
                                                 <Styles>                                                               
                                                     <SelectedRow CssClass="background_color_btn background_texto_btn" />
                                                     <Row Font-Size="11px"  />
                                                     <AlternatingRow Enabled="True" />
                                                     <PagerTopPanel Paddings-PaddingBottom="3px">
                                                         <Paddings PaddingBottom="3px" />
                                                     </PagerTopPanel>
                                                 </Styles>
                                                 <SettingsPager Position="TopAndBottom" PageSize="10">
                                                 </SettingsPager>                    
                                                 </dx:ASPxGridView>
                                                 <%--<dx:ASPxGridViewExporter ID="ExporterImpuestos" GridViewID="GridImpuestos" runat="server" PaperKind="A5" Landscape="true" />--%>
                                             </div>
                                         </dx:ContentControl>
                                     </ContentCollection>
                                 </dx:TabPage>
                                 <dx:TabPage Text="Transporte">
                                     <ContentCollection>
                                         <dx:ContentControl ID="ContentControl11" runat="server">
                                             <div class="row">
                                                 <dx:ASPxGridView ID="GridTransporte" runat="server" EnableTheming="True" Theme="SoftOrange" Settings-HorizontalScrollBarMode="Auto"
                                                     OnCustomCallback="Grid_CustomCallbackTRANSPORTE" Width="100%" Styles-Header-Font-Size="11px" KeyFieldName="(None)">  
                                                     <Columns>
                                                        <dx:GridViewDataTextColumn Caption="IDENTIFICADOR" FieldName="IDENTIFICADOR" ReadOnly="True" VisibleIndex="1" Width="100px" >
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="PAÍS" FieldName="PAIS" ReadOnly="True" VisibleIndex="2" Width="80px">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="PAÍS <br/> DESCRIPCIÓN" FieldName="PAIS_DESCRIPCION" ReadOnly="True" VisibleIndex="3" Width="180px">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="NOMBRE" FieldName="NOMBRE" ReadOnly="True" VisibleIndex="4" Width="200px">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="RFC <br/> TRANSPORTISTA" FieldName="RFC_TRANSPORTISTA" ReadOnly="True" VisibleIndex="5" Width="140px">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="CURP <br/> TRANSPORTISTA" FieldName="CURP_TRANSPORTISTA" ReadOnly="True" VisibleIndex="6" Width="140px">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="DOMICILIO <br/> TRANSPORTISTA" FieldName="DOMICILIO_TRANSPORTISTA" ReadOnly="True" VisibleIndex="7" Width="400px">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="CANDADOS" FieldName="CANDADOS" ReadOnly="True" VisibleIndex="8" Width="100px">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </dx:GridViewDataTextColumn>
                                                     </Columns>
                                                     <SettingsBehavior AllowSelectByRowClick="true" AllowSelectSingleRowOnly="true" />
                                                     <%--<SettingsAdaptivity AdaptivityMode="HideDataCellsWindowLimit" HideDataCellsAtWindowInnerWidth="800" 
                                                        AdaptiveDetailColumnCount="1" AllowOnlyOneAdaptiveDetailExpanded="True">
                                                        <AdaptiveDetailLayoutProperties colcount="2">
                                                            <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit"  SwitchToSingleColumnAtWindowInnerWidth="600" />
                                                        </AdaptiveDetailLayoutProperties>
                                                     </SettingsAdaptivity> 
                                                     <SettingsResizing ColumnResizeMode="Control" />--%>
                                                     <Styles>                                                               
                                                         <SelectedRow CssClass="background_color_btn background_texto_btn" />
                                                         <Row Font-Size="11px"  />
                                                         <AlternatingRow Enabled="True" />
                                                         <PagerTopPanel Paddings-PaddingBottom="3px">
                                                             <Paddings PaddingBottom="3px" />
                                                         </PagerTopPanel>
                                                     </Styles>                                                                             
                                                     <SettingsPager Position="TopAndBottom" PageSize="10">
                                                     </SettingsPager>               
                                                 </dx:ASPxGridView>
                                                 <%--<dx:ASPxGridViewExporter ID="ExporterTransporte" GridViewID="GridTransporte" runat="server" PaperKind="A5" Landscape="true" />--%>
                                             </div>
                                         </dx:ContentControl>
                                     </ContentCollection>
                                 </dx:TabPage>
                                 <dx:TabPage Text="Guías">
                                     <ContentCollection>
                                         <dx:ContentControl ID="ContentControl12" runat="server">
                                             <div class="row col-sx-12 col-sm-12 col-md-4 col-lg-4">
                                                 <dx:ASPxGridView ID="GridGuias" runat="server" EnableTheming="True" Theme="SoftOrange" KeyFieldName="(None)"
                                                     OnCustomCallback="Grid_CustomCallbackGUIAS"  Width="100%" Styles-Header-Font-Size="11px" >                                                                                         
                                                     <Columns>
                                                         <dx:GridViewDataTextColumn Caption="GUÍA MANIFIESTO" FieldName="GUIA_GUIAMANIFIESTO" ReadOnly="True" VisibleIndex="1" Width="60%" >
                                                             <HeaderStyle HorizontalAlign="Center" />
                                                             <CellStyle HorizontalAlign="Center" />
                                                         </dx:GridViewDataTextColumn>
                                                         <dx:GridViewDataTextColumn Caption="TIPO GUÍA" FieldName="TIPO_GUIA" ReadOnly="True" VisibleIndex="2" Width="50%">
                                                             <HeaderStyle HorizontalAlign="Center" />
                                                             <CellStyle HorizontalAlign="Center" />
                                                         </dx:GridViewDataTextColumn>
                                                     </Columns>
                                                     <SettingsBehavior AllowSelectByRowClick="true" AllowSelectSingleRowOnly="true" />
                                                     <SettingsAdaptivity AdaptivityMode="HideDataCellsWindowLimit" HideDataCellsAtWindowInnerWidth="800" 
                                                         AdaptiveDetailColumnCount="1" AllowOnlyOneAdaptiveDetailExpanded="True">
                                                         <AdaptiveDetailLayoutProperties colcount="1">
                                                             <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit"  SwitchToSingleColumnAtWindowInnerWidth="600" />
                                                         </AdaptiveDetailLayoutProperties>
                                                     </SettingsAdaptivity> 
                                                     <SettingsResizing ColumnResizeMode="Control" />
                                                     <Styles>                                                               
                                                         <SelectedRow CssClass="background_color_btn background_texto_btn" />
                                                         <Row Font-Size="11px"  />
                                                         <AlternatingRow Enabled="True" />
                                                         <PagerTopPanel Paddings-PaddingBottom="3px">
                                                             <Paddings PaddingBottom="3px" />
                                                         </PagerTopPanel>
                                                     </Styles>
                                                     <SettingsPager Position="TopAndBottom" PageSize="10">
                                                     </SettingsPager>                    
                                                 </dx:ASPxGridView>
                                                 <%--<dx:ASPxGridViewExporter ID="ExporterGuias" GridViewID="GridGuias" runat="server" PaperKind="A5" Landscape="true" />--%>                                                                                               
                                             </div>
                                         </dx:ContentControl>
                                     </ContentCollection>
                                 </dx:TabPage>
                                 <dx:TabPage Text="Contenedores">
                                     <ContentCollection>
                                         <dx:ContentControl ID="ContentControl13" runat="server">
                                             <div class="row col-sx-12 col-sm-12 col-md-6 col-lg-6">
                                                 <dx:ASPxGridView ID="GridContenedores" runat="server" EnableTheming="True" Theme="SoftOrange" KeyFieldName="(None)"
                                                     OnCustomCallback="Grid_CustomCallbackCONTENEDORES" Width="100%" Styles-Header-Font-Size="11px" >    
                                                     <Columns>
                                                 <dx:GridViewDataTextColumn Caption="IDENTIFICADOR" FieldName="IDENTIFICADOR" ReadOnly="True" VisibleIndex="1" Width="24%" >
                                                     <HeaderStyle HorizontalAlign="Center" />
                                                     <CellStyle HorizontalAlign="Center" />
                                                 </dx:GridViewDataTextColumn>
                                                 <dx:GridViewDataTextColumn Caption="CLAVE" FieldName="CLAVE" ReadOnly="True" VisibleIndex="2" Width="20%">
                                                     <HeaderStyle HorizontalAlign="Center" />
                                                     <CellStyle HorizontalAlign="Center" />
                                                 </dx:GridViewDataTextColumn>
                                                 <dx:GridViewDataTextColumn Caption="DESCRIPCIÓN" FieldName="DESCRIPCION" ReadOnly="True" VisibleIndex="3" Width="56%">
                                                     <HeaderStyle HorizontalAlign="Center" />
                                                 </dx:GridViewDataTextColumn>
                                             </Columns>
                                                     <SettingsBehavior AllowSelectByRowClick="true" AllowSelectSingleRowOnly="true" />
                                                     <SettingsAdaptivity AdaptivityMode="HideDataCellsWindowLimit" HideDataCellsAtWindowInnerWidth="800" 
                                                 AdaptiveDetailColumnCount="1" AllowOnlyOneAdaptiveDetailExpanded="True">
                                                 <AdaptiveDetailLayoutProperties colcount="2">
                                                     <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit"  SwitchToSingleColumnAtWindowInnerWidth="600" />
                                                 </AdaptiveDetailLayoutProperties>
                                             </SettingsAdaptivity> 
                                                     <SettingsResizing ColumnResizeMode="Control" />
                                                     <Styles>                                                               
                                                 <SelectedRow CssClass="background_color_btn background_texto_btn" />
                                                 <Row Font-Size="11px"  />
                                                 <AlternatingRow Enabled="True" />
                                                 <PagerTopPanel Paddings-PaddingBottom="3px">
                                                     <Paddings PaddingBottom="3px" />
                                                 </PagerTopPanel>
                                             </Styles>
                                                     <SettingsPager Position="TopAndBottom" PageSize="10">
                                                     </SettingsPager>                     
                                                 </dx:ASPxGridView>
                                                 <%--<dx:ASPxGridViewExporter ID="ExporterContenedores" GridViewID="GridContenedores" runat="server" PaperKind="A5" Landscape="true" />--%>
                                             </div>
                                         </dx:ContentControl>
                                     </ContentCollection>
                                 </dx:TabPage>
                                 <dx:TabPage Text="Observaciones">
                                     <ContentCollection>
                                         <dx:ContentControl ID="ContentControl14" runat="server">
                                             <div class="row">
                                                 <dx:ASPxFormLayout runat="server" ID="ASPxFormLayout_Obs" Font-Size="11px" Width="100%">
                                                    <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit" SwitchToSingleColumnAtWindowInnerWidth="1200" />
                                                    <Items>
                                                        <dx:LayoutGroup Caption="" ColCount="1" GroupBoxDecoration="None" UseDefaultPaddings="false" SettingsItemCaptions-Location="Top">
                                                            <GroupBoxStyle>
                                                                <Caption Font-Bold="true" />
                                                            </GroupBoxStyle>
                                                            <ParentContainerStyle BackColor="#E8E8E8" CssClass="bordes_curvos" >
                                                                <Paddings Padding="10px" />
                                                            </ParentContainerStyle>                                                    
                                                            <Items>                                                                
                                                                <dx:LayoutItem Caption="&nbsp;&nbsp;&nbsp;&nbsp;OBSERVACIONES" Width="100%" >
                                                                    <ParentContainerStyle BackColor="#E8E8E8" />
                                                                    <CaptionStyle CssClass="background_texto_btn"  />
                                                                    <CaptionCellStyle CssClass="background_color_btn bordes_curvos_top" Paddings-PaddingTop="5px" Paddings-PaddingRight="0px"  />
                                                                    <NestedControlCellStyle Paddings-Padding="10px" BackColor="#F9F9F9" CssClass="bordes_curvos_down" />                                                                                                                                                                                                        
                                                                    <LayoutItemNestedControlCollection>
                                                                        <dx:LayoutItemNestedControlContainer>
                                                                            <dx:ASPxLabel ID="ASPx_OBSERVACIONES" runat="server" Font-Size="11px" Width="100%" />                                                                            
                                                                        </dx:LayoutItemNestedControlContainer>
                                                                    </LayoutItemNestedControlCollection>
                                                                    <CaptionSettings />
                                                                </dx:LayoutItem>                                                                                                                                                                                                                                                     
                                                            </Items>
                                                        </dx:LayoutGroup>
                                                    </Items>
                                                 </dx:ASPxFormLayout>                                                
                                             </div>
                                         </dx:ContentControl>
                                     </ContentCollection>
                                 </dx:TabPage>
                                 <dx:TabPage Text="Descargos">
                                     <ContentCollection>
                                         <dx:ContentControl ID="ContentControl15" runat="server">
                                             <div class="row">
                                                 <dx:ASPxGridView ID="GridDescargos" runat="server" EnableTheming="True" Theme="SoftOrange" Styles-Header-Font-Size="11px" 
                                                     OnCustomCallback="Grid_CustomCallbackDESCARGOS" Settings-HorizontalScrollBarMode="Auto" Width="100%" KeyFieldName="(None)">
                                                     <Columns>
                                                     <dx:GridViewDataTextColumn Caption="PATENTE <br/> ORIGINAL" FieldName="PATENTE_ORIGINAL" ReadOnly="True" VisibleIndex="1" Width="160px" >
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                     </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataTextColumn Caption="PEDIMENTO <br/> ORIGINAL" FieldName="PEDIMENTO_ORIGINAL" ReadOnly="True" VisibleIndex="2" Width="160px">
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                     </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataTextColumn Caption="ADUANA ORIGINAL <br/> CLAVE" FieldName="ADUANA_ORIGINAL_CLAVE" ReadOnly="True" VisibleIndex="3" Width="160px">
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                     </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataTextColumn Caption="ADUANA ORIGINAL <br/> DESCRIPCIÓN" FieldName="ADUANA_ORIGINAL_DESCRIPCION" ReadOnly="True" VisibleIndex="4" Width="260px">
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                     </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataTextColumn Caption="CLAVE DOCUMENTO <br/> ORIGINAL" FieldName="CLAVE_DOCUMENTO_ORIGINAL" ReadOnly="True" VisibleIndex="5" Width="160px">
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                     </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataTextColumn Caption="DESCRIPCIÓN <br/> CLAVE" FieldName="DESCRIPCION_CLAVE" ReadOnly="True" VisibleIndex="6" Width="230px">
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                     </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataDateColumn Caption="FECHA PAGO <br/> ORIGINAL" FieldName="FECHA_PAGO_ORIGINAL" ReadOnly="True" VisibleIndex="7" Width="120px">
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                     </dx:GridViewDataDateColumn>
                                                     <dx:GridViewDataTextColumn Caption="FRACCIÓN <br/> ORIGINAL" FieldName="FRACCION_ORIGINAL" ReadOnly="True" VisibleIndex="8" Width="120px">
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                     </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataTextColumn Caption="UNIDAD DE <br/> MEDIDA CLAVE" FieldName="UNIDAD_DE_MEDIDA_CLAVE" ReadOnly="True" VisibleIndex="9" Width="120px">
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                     </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataTextColumn Caption="UNIDAD <br/> DESCRIPCIÓN" FieldName="UNIDAD_DESCRIPCION" ReadOnly="True" VisibleIndex="10" Width="240px">
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                     </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataTextColumn Caption="CANTIDAD" FieldName="CANTIDAD" ReadOnly="True" VisibleIndex="11" Width="100px">
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                     </dx:GridViewDataTextColumn>
                                                 </Columns>
                                                     <SettingsBehavior AllowSelectByRowClick="true" AllowSelectSingleRowOnly="true" />
                                                     <%--<SettingsAdaptivity AdaptivityMode="HideDataCellsWindowLimit" HideDataCellsAtWindowInnerWidth="800" 
                                                        AdaptiveDetailColumnCount="1" AllowOnlyOneAdaptiveDetailExpanded="True">
                                                        <AdaptiveDetailLayoutProperties colcount="2">
                                                            <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit"  SwitchToSingleColumnAtWindowInnerWidth="600" />
                                                        </AdaptiveDetailLayoutProperties>
                                                     </SettingsAdaptivity> 
                                                     <SettingsResizing ColumnResizeMode="Control" />--%>
                                                     <Styles>                                                               
                                                         <SelectedRow CssClass="background_color_btn background_texto_btn" />
                                                         <Row Font-Size="11px"  />
                                                         <AlternatingRow Enabled="True" />
                                                         <PagerTopPanel Paddings-PaddingBottom="3px">
                                                             <Paddings PaddingBottom="3px" />
                                                         </PagerTopPanel>
                                                     </Styles>                                                                             
                                                     <SettingsPager Position="TopAndBottom" PageSize="10">
                                                     </SettingsPager>               
                                                 </dx:ASPxGridView>
                                                 <%--<dx:ASPxGridViewExporter ID="ExporterDescargos" GridViewID="GridDescargos" runat="server" PaperKind="A5" Landscape="true" />--%>
                                             </div>
                                         </dx:ContentControl>
                                     </ContentCollection>
                                 </dx:TabPage>
                                 <dx:TabPage Text="Rectificaciones">
                                     <ContentCollection>
                                         <dx:ContentControl ID="ContentControl2" runat="server">
                                             <div class="row">
                                                 <dx:ASPxGridView ID="GridRectificaciones" runat="server" EnableTheming="True" Theme="SoftOrange" Styles-Header-Font-Size="11px"
                                                     OnCustomCallback="Grid_CustomCallbackRECTIFICACIONES" Settings-HorizontalScrollBarMode="Auto" Width="100%" KeyFieldName="(None)">
                                                     <Columns>
                                                     <dx:GridViewDataTextColumn Caption="PEDIMENTO <br/> ARMADO" FieldName="PEDIMENTOARMADO" ReadOnly="True" VisibleIndex="1" Width="150px" >
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                     </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataTextColumn Caption="CLAVE <br/> RECTIFICACIÓN" FieldName="CLAVE RECTIFICACION" ReadOnly="True" VisibleIndex="2" Width="120px">
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                     </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataTextColumn Caption="FECHA DE PAGO <br/> RECTIFICACIÓN" FieldName="FECHA DE PAGO RECTIFICACION" ReadOnly="True" VisibleIndex="3" Width="130px">
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                     </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataTextColumn Caption="ADUANA <br/> ORIGINAL" FieldName="ADUANA ORIGINAL" ReadOnly="True" VisibleIndex="4" Width="110px">
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                     </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataTextColumn Caption="PATENTE <br/> ORIGINAL" FieldName="PATENTE ORIGINAL" ReadOnly="True" VisibleIndex="5" Width="120px">
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                     </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataTextColumn Caption="PEDIMENTO <br/> ORIGINAL" FieldName="PEDIMENTO ORIGINAL" ReadOnly="True" VisibleIndex="6" Width="130px">
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                     </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataDateColumn Caption="FECHA <br/> ORIGINAL" FieldName="FECHA ORIGINAL" ReadOnly="True" VisibleIndex="7" Width="110px">
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                     </dx:GridViewDataDateColumn>                                                     
                                                 </Columns>
                                                     <Styles>                                                               
                                                         <SelectedRow CssClass="background_color_btn background_texto_btn" />
                                                         <Row Font-Size="11px"  />
                                                         <AlternatingRow Enabled="True" />
                                                         <PagerTopPanel Paddings-PaddingBottom="3px">
                                                             <Paddings PaddingBottom="3px" />
                                                         </PagerTopPanel>
                                                     </Styles>                                                                             
                                                     <SettingsPager Position="TopAndBottom" PageSize="10">
                                                     </SettingsPager>                                                                                              
                                                 </dx:ASPxGridView>
                                                 <%--<dx:ASPxGridViewExporter ID="ExporterREctificaciones" GridViewID="GridRectificaciones" runat="server" PaperKind="A5" Landscape="true" />--%>
                                             </div>
                                         </dx:ContentControl>
                                     </ContentCollection>
                                 </dx:TabPage>
                             </TabPages>
                         </dx:ASPxPageControl>
                    </div>
                    <div class="modal-footer">
                    <div class="col-md-12 col-md-offset-0" style="text-align: center;">
                        <%--<asp:LinkButton ID="btnGuardar" runat="server" CssClass="btn btn-success btn-sm" Text="Guardar" OnClick="btnGuardar_Click">
                            <span class="glyphicon glyphicon-ok"></span>&nbsp;&nbsp;Guardar</asp:LinkButton>
                        <button type="button" class="btn btn-danger btn-sm" data-dismiss="modal">
                            <span class="glyphicon glyphicon-remove"></span>&nbsp;&nbsp;Cancelar
                        </button>--%>
                    </div>
                </div>
                </div>
                <div class="modal-footer">
                  <%--<button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>--%>                    
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="ModalPartidaDetalle" tabindex="-1" role="dialog" aria-labelledby="Modal1Titulo" data-backdrop="static">
        <div class="modal-dialog modal-xl" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h6 id="Modal1Titulo" class="modal-title" runat="server"></h6>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                      <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div id="DivPartidaDetalle" runat="server" class="modal-body">
                    <div class="row" style="margin-left:10px">
                        <div class="row">
                            <table style="width:190px; margin-left:15px; margin-bottom:5px">
                                <tr>
                                    <td style="width:55px">
                                        <dx:ASPxLabel ID="lblTitPartida" runat="server" Text="Partida:" />
                                    </td>
                                    <td style="width:80px">                          
                                        <dx:ASPxComboBox ID="cbxPartida" Caption="" runat="server" Height="30px" NullText="Partidas" DataSecurityMode="Default"
                                            Width="100%" Font-Size="11px" Font-Names="Arial,Helvetica" CssClass="bordes_curvos" Theme="MaterialCompact"
                                            OnSelectedIndexChanged="cbxPartida_SelectedIndexChanged" AutoPostBack="true"  ForeColor="#6B5555" TextField="PARTIDA" ValueField="PARTIDA">
                                            <ClientSideEvents SelectedIndexChanged="function(s, e) { Callback.PerformCallback(); LoadingPanel1.Show(); }" 
                                                        Init="function(s, e) {LoadingPanel1.Hide(); }" />                            
                                            <ClearButton DisplayMode="Never" />
                                        </dx:ASPxComboBox>                               
                                    </td>
                                    <td style="width:25px; text-align:right">
                                        <dx:ASPxLabel ID="lblTitDe" runat="server" Text="de" />
                                    </td>
                                    <td style="width:30px; text-align:right">
                                        <dx:ASPxLabel ID="lblTitDeTotal" runat="server" />
                                    </td>                                                              
                                </tr>
                            </table>
                        </div>                                                                                                           
                        <dx:ASPxPageControl runat="server" ID="pageControl" Height="60px" Width="98%" EnableCallBacks="true"  ContentStyle-Border-BorderWidth="3px"
                             TabAlign="Justify" ActiveTabIndex="0" EnableTabScrolling="true" Theme="SoftOrange" Font-Size="12px" ContentStyle-VerticalAlign="Top">
                             <TabStyle Paddings-PaddingLeft="50px" Paddings-PaddingRight="50px"  />                                                                                      
                             <ContentStyle>
                                 <Paddings PaddingLeft="20px" PaddingRight="20px" PaddingTop="5px" />
                             </ContentStyle>
                             <TabPages>                                                                            
                                 <dx:TabPage Text="Detalle">
                                     <ContentCollection>
                                     <dx:ContentControl ID="ContentControlP1" runat="server" >                                        
                                     <div class="row">
                                        <dx:ASPxFormLayout runat="server" ID="ASPxFormLayout_DetalleP" Font-Size="11px">
                                            <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit" SwitchToSingleColumnAtWindowInnerWidth="1200" />
                                            <Items>
                                                <dx:LayoutGroup Caption="" ColCount="2" GroupBoxDecoration="None" UseDefaultPaddings="false" BackColor="#E8E8E8">
                                                    <GroupBoxStyle>
                                                        <Caption Font-Bold="true" />
                                                    </GroupBoxStyle>
                                                    <ParentContainerStyle BackColor="#E8E8E8" CssClass="bordes_curvos">
                                                        <Paddings PaddingLeft="10px" PaddingRight="10px" PaddingTop="10px" />
                                                    </ParentContainerStyle>
                                                    <Items>                                                                
                                                        <dx:LayoutItem Caption="PARTIDA DETALLEKEY" >
                                                            <CaptionStyle CssClass="background_texto_btn"/>
                                                            <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                            <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />                                                                
                                                            <LayoutItemNestedControlCollection>
                                                                <dx:LayoutItemNestedControlContainer>
                                                                    <dx:ASPxLabel ID="lblP_PARTIDA_DETALLEKEY" runat="server" Font-Size="11px" />                                                                            
                                                                </dx:LayoutItemNestedControlContainer>
                                                            </LayoutItemNestedControlCollection>
                                                        </dx:LayoutItem>
                                                        <dx:LayoutItem Caption="VALOR COMERCIAL">                                                                    
                                                            <CaptionStyle CssClass="background_texto_btn"/>
                                                            <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                            <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                            <LayoutItemNestedControlCollection>
                                                                <dx:LayoutItemNestedControlContainer>
                                                                    <dx:ASPxLabel ID="lblP_VALOR_COMERCIAL" runat="server" Font-Size="11px" />
                                                                </dx:LayoutItemNestedControlContainer>
                                                            </LayoutItemNestedControlCollection>
                                                        </dx:LayoutItem>
                                                        <dx:LayoutItem Caption="PEDIMENTO">                                                                    
                                                            <CaptionStyle CssClass="background_texto_btn"/>
                                                            <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                            <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                            <LayoutItemNestedControlCollection>
                                                                <dx:LayoutItemNestedControlContainer>
                                                                    <dx:ASPxLabel ID="lblP_PEDIMENTO" runat="server" Font-Size="11px" />
                                                                </dx:LayoutItemNestedControlContainer>
                                                            </LayoutItemNestedControlCollection>
                                                        </dx:LayoutItem>
                                                        <dx:LayoutItem Caption="VALOR ADUANA">                                                                    
                                                            <CaptionStyle CssClass="background_texto_btn"/>
                                                            <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                            <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                            <LayoutItemNestedControlCollection>
                                                                <dx:LayoutItemNestedControlContainer>
                                                                    <dx:ASPxLabel ID="lblP_VALOR_ADUANA" runat="server" Font-Size="11px" />
                                                                </dx:LayoutItemNestedControlContainer>
                                                            </LayoutItemNestedControlCollection>
                                                        </dx:LayoutItem>
                                                        <dx:LayoutItem Caption="PARTIDA">
                                                            <CaptionStyle CssClass="background_texto_btn"/>
                                                            <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                            <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                            <LayoutItemNestedControlCollection>
                                                                <dx:LayoutItemNestedControlContainer>
                                                                    <dx:ASPxLabel ID="lblP_PARTIDA" runat="server" Font-Size="11px" />
                                                                </dx:LayoutItemNestedControlContainer>
                                                            </LayoutItemNestedControlCollection>
                                                        </dx:LayoutItem>
                                                        <dx:LayoutItem Caption="VALOR DOLARES">
                                                            <CaptionStyle CssClass="background_texto_btn"/>
                                                            <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                            <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                            <LayoutItemNestedControlCollection>
                                                                <dx:LayoutItemNestedControlContainer>
                                                                    <dx:ASPxLabel ID="lblP_VALOR_DOLARES" runat="server" Font-Size="11px" />
                                                                </dx:LayoutItemNestedControlContainer>
                                                            </LayoutItemNestedControlCollection>
                                                        </dx:LayoutItem>
                                                        <dx:LayoutItem Caption="NÚMERO PARTIDA">
                                                            <CaptionStyle CssClass="background_texto_btn"/>
                                                            <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                            <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                            <LayoutItemNestedControlCollection>
                                                                <dx:LayoutItemNestedControlContainer>
                                                                    <dx:ASPxLabel ID="lblP_NUMERO_PARTIDA" runat="server" Font-Size="11px" />
                                                                </dx:LayoutItemNestedControlContainer>
                                                            </LayoutItemNestedControlCollection>
                                                        </dx:LayoutItem>
                                                        <dx:LayoutItem Caption="VALOR AGREGADO">
                                                            <CaptionStyle CssClass="background_texto_btn"/>
                                                            <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                            <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                            <LayoutItemNestedControlCollection>
                                                                <dx:LayoutItemNestedControlContainer>
                                                                    <dx:ASPxLabel ID="lblP_VALOR_AGREGADO" runat="server" Font-Size="11px" />
                                                                </dx:LayoutItemNestedControlContainer>
                                                            </LayoutItemNestedControlCollection>
                                                        </dx:LayoutItem>
                                                        <dx:LayoutItem Caption="FRACCIÓN ARANCELARIA">
                                                            <CaptionStyle CssClass="background_texto_btn"/>
                                                            <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                            <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                            <%--<ParentContainerStyle BackColor="#E8E8E8" Paddings-PaddingLeft="15px" />--%>
                                                            <LayoutItemNestedControlCollection>
                                                                <dx:LayoutItemNestedControlContainer>
                                                                    <dx:ASPxLabel ID="lblP_FRACCION_ARANCELARIA" runat="server" Font-Size="11px" />
                                                                </dx:LayoutItemNestedControlContainer>
                                                            </LayoutItemNestedControlCollection>
                                                        </dx:LayoutItem>
                                                        <dx:LayoutItem Caption="CÓDIGO PRODUCTO">
                                                            <CaptionStyle CssClass="background_texto_btn"/>
                                                            <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                            <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                            <LayoutItemNestedControlCollection>
                                                                <dx:LayoutItemNestedControlContainer>
                                                                    <dx:ASPxLabel ID="lblP_CODIGO_PRODUCTO" runat="server" Font-Size="11px" />
                                                                </dx:LayoutItemNestedControlContainer>
                                                            </LayoutItemNestedControlCollection>
                                                        </dx:LayoutItem>
                                                        <dx:LayoutItem Caption="SUBDIVISIÓN FRACCIÓN">
                                                            <CaptionStyle CssClass="background_texto_btn"/>
                                                            <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                            <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                            <LayoutItemNestedControlCollection>
                                                                <dx:LayoutItemNestedControlContainer>
                                                                    <dx:ASPxLabel ID="lblP_SUBDIVISION_FRACCION" runat="server" Font-Size="11px" />
                                                                </dx:LayoutItemNestedControlContainer>
                                                            </LayoutItemNestedControlCollection>
                                                        </dx:LayoutItem>
                                                        <dx:LayoutItem Caption="MARCA">
                                                            <CaptionStyle CssClass="background_texto_btn"/>
                                                            <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                            <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                            <LayoutItemNestedControlCollection>
                                                                <dx:LayoutItemNestedControlContainer>
                                                                    <dx:ASPxLabel ID="lblP_MARCA" runat="server" Font-Size="11px" />
                                                                </dx:LayoutItemNestedControlContainer>
                                                            </LayoutItemNestedControlCollection>
                                                        </dx:LayoutItem>
                                                        <dx:LayoutItem Caption="DESCRIPCIÓN MERCANCIA">
                                                            <CaptionStyle CssClass="background_texto_btn"/>
                                                            <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                            <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                            <LayoutItemNestedControlCollection>
                                                                <dx:LayoutItemNestedControlContainer>
                                                                    <dx:ASPxLabel ID="lblP_DESCRIPCION_MERCANCIA" runat="server" Font-Size="11px" />
                                                                </dx:LayoutItemNestedControlContainer>
                                                            </LayoutItemNestedControlCollection>
                                                        </dx:LayoutItem>
                                                        <dx:LayoutItem Caption="MÓDELO">
                                                            <CaptionStyle CssClass="background_texto_btn"/>
                                                            <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                            <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                            <LayoutItemNestedControlCollection>
                                                                <dx:LayoutItemNestedControlContainer>
                                                                    <dx:ASPxLabel ID="lblP_MODELO" runat="server" Font-Size="11px" />
                                                                </dx:LayoutItemNestedControlContainer>
                                                            </LayoutItemNestedControlCollection>
                                                        </dx:LayoutItem>
                                                        <dx:LayoutItem Caption="UNIDAD MEDIDA TARIFA CLAVE">
                                                            <CaptionStyle CssClass="background_texto_btn" />
                                                            <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                            <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                            <LayoutItemNestedControlCollection>
                                                                <dx:LayoutItemNestedControlContainer>
                                                                    <dx:ASPxLabel ID="lblP_UNIDAD_MEDIDA_TARIFA_CLAVE" runat="server" Font-Size="11px" />
                                                                </dx:LayoutItemNestedControlContainer>
                                                            </LayoutItemNestedControlCollection>
                                                        </dx:LayoutItem>
                                                        <dx:LayoutItem Caption="MÉTODO VALORACIÓN">
                                                            <CaptionStyle CssClass="background_texto_btn"/>
                                                            <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                            <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                            <LayoutItemNestedControlCollection>
                                                                <dx:LayoutItemNestedControlContainer>
                                                                    <dx:ASPxLabel ID="lblP_METODO_VALORACION" runat="server" Font-Size="11px" />
                                                                </dx:LayoutItemNestedControlContainer>
                                                            </LayoutItemNestedControlCollection>
                                                        </dx:LayoutItem>
                                                        <dx:LayoutItem Caption="UNIDAD MEDIDA TARIFA DESCRIPCIÓN">
                                                            <CaptionStyle CssClass="background_texto_btn"/>
                                                            <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                            <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                            <LayoutItemNestedControlCollection>
                                                                <dx:LayoutItemNestedControlContainer>
                                                                    <dx:ASPxLabel ID="lblP_UNIDAD_MEDIDA_TARIFA_DESCRIPCION" runat="server" Font-Size="11px" />
                                                                </dx:LayoutItemNestedControlContainer>
                                                            </LayoutItemNestedControlCollection>
                                                        </dx:LayoutItem>
                                                        <dx:LayoutItem Caption="VINCULACIÓN">
                                                            <CaptionStyle CssClass="background_texto_btn"/>
                                                            <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                            <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                            <LayoutItemNestedControlCollection>
                                                                <dx:LayoutItemNestedControlContainer>
                                                                    <dx:ASPxLabel ID="lblP_VINCULACION" runat="server" Font-Size="11px" />
                                                                </dx:LayoutItemNestedControlContainer>
                                                            </LayoutItemNestedControlCollection>
                                                        </dx:LayoutItem>
                                                        <dx:LayoutItem Caption="CANTIDAD UNIDAD MEDIDA TARIFA">
                                                            <CaptionStyle CssClass="background_texto_btn"/>
                                                            <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                            <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                            <LayoutItemNestedControlCollection>
                                                                <dx:LayoutItemNestedControlContainer>
                                                                    <dx:ASPxLabel ID="lblP_CANTIDAD_UNIDAD_MEDIDA_TARIFA" runat="server" Font-Size="11px" />
                                                                </dx:LayoutItemNestedControlContainer>
                                                            </LayoutItemNestedControlCollection>
                                                        </dx:LayoutItem>
                                                        <dx:LayoutItem Caption="PAÍS ORIGEN DESTINO CLAVE">
                                                            <CaptionStyle CssClass="background_texto_btn"/>
                                                            <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                            <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                            <LayoutItemNestedControlCollection>
                                                                <dx:LayoutItemNestedControlContainer>
                                                                    <dx:ASPxLabel ID="lblP_PAIS_ORIGEN_DESTINO_CLAVE" runat="server" Font-Size="11px" />
                                                                </dx:LayoutItemNestedControlContainer>
                                                            </LayoutItemNestedControlCollection>
                                                        </dx:LayoutItem>
                                                        <dx:LayoutItem Caption="UNIDAD MEDIDA COMERCIAL CLAVE">
                                                            <CaptionStyle CssClass="background_texto_btn"/>
                                                            <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                            <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />                                                            
                                                            <LayoutItemNestedControlCollection>
                                                                <dx:LayoutItemNestedControlContainer>
                                                                    <dx:ASPxLabel ID="lblP_UNIDAD_MEDIDA_COMERCIAL_CLAVE" runat="server" Font-Size="11px" />
                                                                </dx:LayoutItemNestedControlContainer>
                                                            </LayoutItemNestedControlCollection>
                                                        </dx:LayoutItem>
                                                        <dx:LayoutItem Caption="PAÍS ORIGEN DESTINO NOMBRE">
                                                            <CaptionStyle CssClass="background_texto_btn"/>
                                                            <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                            <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                            <LayoutItemNestedControlCollection>
                                                                <dx:LayoutItemNestedControlContainer>
                                                                    <dx:ASPxLabel ID="lblP_PAIS_ORIGEN_DESTINO_NOMBRE" runat="server" Font-Size="11px" />
                                                                </dx:LayoutItemNestedControlContainer>
                                                            </LayoutItemNestedControlCollection>
                                                        </dx:LayoutItem>
                                                        <dx:LayoutItem Caption="UNIDAD MEDIDA COMERCIAL DESCRIPCIÓN">
                                                            <CaptionStyle CssClass="background_texto_btn"/>
                                                            <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                            <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                            <LayoutItemNestedControlCollection>
                                                                <dx:LayoutItemNestedControlContainer>
                                                                    <dx:ASPxLabel ID="lblP_UNIDAD_MEDIDA_COMERCIAL_CLAVE_DESCRIPCION" runat="server" Font-Size="11px" />
                                                                </dx:LayoutItemNestedControlContainer>
                                                            </LayoutItemNestedControlCollection>
                                                        </dx:LayoutItem>
                                                        <dx:LayoutItem Caption="PAÍS VENDEDOR COMPRADOR CLAVE">
                                                            <CaptionStyle CssClass="background_texto_btn"/>
                                                            <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                            <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                            <LayoutItemNestedControlCollection>
                                                                <dx:LayoutItemNestedControlContainer>
                                                                    <dx:ASPxLabel ID="lblP_PAIS_VENDEDOR_COMPRADOR_CLAVE" runat="server" Font-Size="11px" />
                                                                </dx:LayoutItemNestedControlContainer>
                                                            </LayoutItemNestedControlCollection>
                                                        </dx:LayoutItem>
                                                        <dx:LayoutItem Caption="CANTIDAD UNIDAD MEDIDA COMERCIAL">
                                                            <CaptionStyle CssClass="background_texto_btn"/>
                                                            <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                            <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                            <LayoutItemNestedControlCollection>
                                                                <dx:LayoutItemNestedControlContainer>
                                                                    <dx:ASPxLabel ID="lblP_CANTIDAD_UNIDAD_MEDIDA_COMERCIAL" runat="server" Font-Size="11px" />
                                                                </dx:LayoutItemNestedControlContainer>
                                                            </LayoutItemNestedControlCollection>
                                                        </dx:LayoutItem>
                                                        <dx:LayoutItem Caption="PAÍS VENDEDOR COMPRADOR NOMBRE">
                                                            <CaptionStyle CssClass="background_texto_btn"/>
                                                            <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                            <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                            <LayoutItemNestedControlCollection>
                                                                <dx:LayoutItemNestedControlContainer>
                                                                    <dx:ASPxLabel ID="lblP_PAIS_VENDEDOR_COMPRADOR_NOMBRE" runat="server" Font-Size="11px" />
                                                                </dx:LayoutItemNestedControlContainer>
                                                            </LayoutItemNestedControlCollection>
                                                        </dx:LayoutItem>                                                        
                                                        <dx:LayoutItem Caption="PRECIO UNITARIO">
                                                            <CaptionStyle CssClass="background_texto_btn"/>
                                                            <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                            <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                            <ParentContainerStyle BackColor="#E8E8E8" Paddings-PaddingBottom="10px" />
                                                            <LayoutItemNestedControlCollection>
                                                                <dx:LayoutItemNestedControlContainer>
                                                                    <dx:ASPxLabel ID="lblP_PRECIO_UNITARIO" runat="server" Font-Size="11px" />
                                                                </dx:LayoutItemNestedControlContainer>
                                                            </LayoutItemNestedControlCollection>
                                                        </dx:LayoutItem>
                                                        <dx:LayoutItem Caption=" ">
                                                            <CaptionStyle CssClass="background_texto_btn"/>
                                                            <CaptionCellStyle Paddings-Padding="5px" CssClass="background_color_btn bordes_curvos_izquierda" />
                                                            <NestedControlCellStyle Paddings-Padding="5px" BackColor="#F9F9F9" CssClass="bordes_curvos_derecha" />
                                                            <ParentContainerStyle BackColor="#E8E8E8" Paddings-PaddingBottom="10px" />
                                                            <LayoutItemNestedControlCollection>
                                                                <dx:LayoutItemNestedControlContainer>
                                                                    <dx:ASPxLabel ID="lblP_" runat="server" Font-Size="11px" />
                                                                </dx:LayoutItemNestedControlContainer>
                                                            </LayoutItemNestedControlCollection>
                                                        </dx:LayoutItem>                                                                                                                                                                             
                                                    </Items>
                                                </dx:LayoutGroup>
                                            </Items>
                                        </dx:ASPxFormLayout>
                                        <%-- <dx:ASPxVerticalGrid ID="VerticalGridPartidaDetalle" runat="server" KeyFieldName="PARTIDA" Width="100%" 
                                                 Font-Size="10px" Styles-Header-CssClass="background_color_btn background_texto_btn fondo_vertical_grid" 
                                                 Theme="SoftOrange" BorderTop-BorderWidth="1px">
                                                 <SettingsBehavior AllowSort="false" />
                                                 <Rows>                                                     
                                                     <dx:VerticalGridTextRow Caption="PARTIDA DETALLEKEY:" FieldName="PARTIDA_DETALLEKEY" />            
                                                     <dx:VerticalGridTextRow Caption="PEDIMENTOARMADO:" FieldName="PEDIMENTOARMADO" />                                                                                                        
                                                     <dx:VerticalGridTextRow Caption="PARTIDA:" FieldName="PARTIDA" />
                                                     <dx:VerticalGridDateRow Caption="NÚMERO PARTIDA:" FieldName="PARTIDA_DETALLE_NUMEROPARTIDA" />
                                                     <dx:VerticalGridTextRow Caption="FRACCIÓN ARANCELARIA:" FieldName="PARTIDA_DETALLE_FRACCIONARANCELARIA" />
                                                     <dx:VerticalGridTextRow Caption="SUBDIVISIÓN FRACCIÓN:" FieldName="PARTIDA_DETALLE_SUBDIVISIONFRACCION" />
                                                     <dx:VerticalGridTextRow Caption="DESCRIPCIÓN MERCANCIA:" FieldName="PARTIDA_DETALLE_DESCRIPCIONMERCANCIA" />
                                                     <dx:VerticalGridTextRow Caption="UNIDAD MEDIDA TARIFA CLAVE:" FieldName="PARTIDA_DETALLE_UNIDADMEDIDATARIFA_CLAVE" />
                                                     <dx:VerticalGridTextRow Caption="UNIDAD MEDIDA TARIFA DESCRIPCIÓN:" FieldName="PARTIDA_DETALLE_UNIDADMEDIDATARIFA_DESCRIPCION" />
                                                     <dx:VerticalGridTextRow Caption="CANTIDAD UNIDAD MEDIDA TARIFA:" FieldName="PARTIDA_DETALLE_CANTIDADUNIDADMEDIDATARIFA" />
                                                     <dx:VerticalGridTextRow Caption="UNIDAD MEDIDA COMERCIA CLAVE:" FieldName="PARTIDA_DETALLE_UNIDADMEDIDACOMERCIA_CLAVE" />
                                                     <dx:VerticalGridTextRow Caption="UNIDAD MEDIDA COMERCIAL DESCRIPCIÓN:" FieldName="PARTIDA_DETALLE_UNIDADMEDIDACOMERCIAL_DESCRIPCION" />
                                                     <dx:VerticalGridTextRow Caption="CANTIDAD UNIDAD MEDIDA COMERCIAL:" FieldName="PARTIDA_DETALLE_CANTIDADUNIDADMEDIDACOMERCIAL" />
                                                     <dx:VerticalGridTextRow Caption="PRECIO UNITARIO:" FieldName="PARTIDA_DETALLE_PRECIOUNITARIO" />
                                                     <dx:VerticalGridTextRow Caption="VALOR COMERCIAL:" FieldName="PARTIDA_DETALLE_VALORCOMERCIAL" />
                                                     <dx:VerticalGridTextRow Caption="VALOR ADUANA:" FieldName="PARTIDA_DETALLE_VALORADUANA" />
                                                     <dx:VerticalGridTextRow Caption="VALOR DOLARES:" FieldName="PARTIDA_DETALLE_VALORDOLARES" />
                                                     <dx:VerticalGridTextRow Caption="VALOR AGREGADO:" FieldName="PARTIDA_DETALLE_VALORAGREGADO" />
                                                     <dx:VerticalGridTextRow Caption="CÓDIGO PRODUCTO:" FieldName="PARTIDA_DETALLE_CODIPRODUCTO" />
                                                     <dx:VerticalGridTextRow Caption="MARCA:" FieldName="PARTIDA_DETALLE_MARCA" />
                                                     <dx:VerticalGridTextRow Caption="MÓDELO:" FieldName="PARTIDA_DETALLE_MODELO" />
                                                     <dx:VerticalGridTextRow Caption="MÉTODO VALORACIÓN:" FieldName="PARTIDA_DETALLE_METODOVALORACION" />
                                                     <dx:VerticalGridTextRow Caption="VINCULACIÓN:" FieldName="PARTIDA_DETALLE_VINCULACION" />
                                                     <dx:VerticalGridTextRow Caption="PAÍS ORIGEN DESTINO_CLAVE:" FieldName="PARTIDA_DETALLE_PAISORIGENDESTINO_CLAVE" />
                                                     <dx:VerticalGridTextRow Caption="PAÍS ORIGEN DESTINO NOMBRE:" FieldName="PARTIDA_DETALLE_PAISORIGENDESTINO_DESCRIPCION" />
                                                     <dx:VerticalGridTextRow Caption="PAÍS VENDEDOR COMPRADOR CLAVE:" FieldName="PARTIDA_DETALLE_PAISVENDEDORCOMPRADOR_CLAVE" />
                                                     <dx:VerticalGridTextRow Caption="PAÍS VENDEDOR COMPRADOR NOMBRE:" FieldName="PARTIDA_DETALLE_PAISVENDEDORCOMPRADOR_DESCRIPCION" />
                                                 </Rows>
                                                 <Settings RecordWidth="856" HeaderAreaWidth="270" HorizontalScrollBarMode="Auto" />
                                                 <SettingsPager PageSize="100" />                                                                                                
                                                 <Styles>
                                                     <Record HorizontalAlign="Left" />
                                                 </Styles>
                                        </dx:ASPxVerticalGrid>--%>
                                     </div>
                                     </dx:ContentControl>
                                 </ContentCollection>
                                 </dx:TabPage>
                                 <dx:TabPage Text="Observaciones">
                                     <ContentCollection>
                                     <dx:ContentControl ID="ContentControlP2" runat="server">
                                         <div class="row">
                                             <dx:ASPxFormLayout runat="server" ID="ASPxFormLayout_ObsP" Font-Size="11px" Width="100%">
                                                <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit" SwitchToSingleColumnAtWindowInnerWidth="1200" />
                                                <Items>
                                                    <dx:LayoutGroup Caption="" ColCount="1" GroupBoxDecoration="None" UseDefaultPaddings="false" SettingsItemCaptions-Location="Top">
                                                        <GroupBoxStyle>
                                                            <Caption Font-Bold="true" />
                                                        </GroupBoxStyle>
                                                        <ParentContainerStyle BackColor="#E8E8E8" CssClass="bordes_curvos" >
                                                            <Paddings Padding="10px" />
                                                        </ParentContainerStyle>                                                    
                                                        <Items>                                                                
                                                            <dx:LayoutItem Caption="&nbsp;&nbsp;&nbsp;&nbsp;OBSERVACIONES" Width="100%" >
                                                                <ParentContainerStyle BackColor="#E8E8E8" />
                                                                <CaptionStyle CssClass="background_texto_btn"  />
                                                                <CaptionCellStyle CssClass="background_color_btn bordes_curvos_top" Paddings-PaddingTop="5px" Paddings-PaddingRight="0px"  />
                                                                <NestedControlCellStyle Paddings-Padding="10px" BackColor="#F9F9F9" CssClass="bordes_curvos_down" />                                                                                                                                                                                                        
                                                                <LayoutItemNestedControlCollection>
                                                                    <dx:LayoutItemNestedControlContainer>
                                                                        <dx:ASPxLabel ID="ASPx_PartidaObservaciones" runat="server" Font-Size="11px" Width="100%" />                                                                            
                                                                    </dx:LayoutItemNestedControlContainer>
                                                                </LayoutItemNestedControlCollection>
                                                                <CaptionSettings />
                                                            </dx:LayoutItem>                                                                                                                                                                                                                                                     
                                                        </Items>
                                                    </dx:LayoutGroup>
                                                </Items>
                                             </dx:ASPxFormLayout>                                             
                                         </div>
                                     </dx:ContentControl>
                                 </ContentCollection>
                                 </dx:TabPage>
                                 <dx:TabPage Text="Impuestos">
                                     <ContentCollection>
                                     <dx:ContentControl ID="ContentControlP3" runat="server">
                                         <div class="row">
                                            <dx:ASPxGridView ID="GridPartidaGravamen" runat="server" EnableTheming="True" KeyFieldName="PARTIDA" Theme="SoftOrange" Width="100%" 
                                                StylesToolbar-Item-BackColor="#E7E7E7" StylesToolbar-Item-ForeColor="#5E5E5E" StylesToolbar-Item-Width="100%"
                                                Styles-Header-Font-Size="11px" Settings-HorizontalScrollBarMode="Auto">
                                                <Columns>                                                                                                    
                                                     <dx:GridViewDataTextColumn Caption="PARTIDA" FieldName="PARTIDA" ReadOnly="True" VisibleIndex="1" Width="100px" >
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                         <CellStyle HorizontalAlign="Center" />
                                                     </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataTextColumn Caption="CLAVE <br/> GRAVAMEN" FieldName="CLAVE GRAVAMEN" ReadOnly="True" VisibleIndex="2" Width="100px">
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                         <CellStyle HorizontalAlign="Center" />
                                                     </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataTextColumn Caption="GRAVAMEN <br/> DESCRIPCIÓN" FieldName="GRAVAMEN DESCRIPCION" ReadOnly="True" VisibleIndex="3" Width="100px">
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                         <CellStyle HorizontalAlign="Center" />                                                        
                                                     </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataTextColumn Caption="CLAVE <br/> TASA" FieldName="CLAVE TASA" ReadOnly="True" VisibleIndex="4" Width="230px">
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                         <CellStyle HorizontalAlign="Center" />
                                                     </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataTextColumn Caption="DESCRIPCIÓN <br/> TASA" FieldName="DESCRIPCION TASA" ReadOnly="True" VisibleIndex="5" Width="100px">
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                         <CellStyle HorizontalAlign="Center" />
                                                     </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataTextColumn Caption="TASA <br/> APLICABLE" FieldName="TASA APLICABLE" ReadOnly="True" VisibleIndex="6" Width="160px">
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                         <CellStyle HorizontalAlign="Right" />
                                                     </dx:GridViewDataTextColumn>                                                                                                
                                                 </Columns>
                                                <SettingsBehavior AllowSelectByRowClick="true" AllowSelectSingleRowOnly="true" />                                                                                                
                                                <%--<SettingsAdaptivity AdaptivityMode="HideDataCellsWindowLimit" HideDataCellsAtWindowInnerWidth="800" 
                                                    AdaptiveDetailColumnCount="1" AllowOnlyOneAdaptiveDetailExpanded="True">
                                                    <AdaptiveDetailLayoutProperties colcount="1">
                                                        <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit"  SwitchToSingleColumnAtWindowInnerWidth="600" />
                                                    </AdaptiveDetailLayoutProperties>
                                                </SettingsAdaptivity> 
                                                <SettingsResizing ColumnResizeMode="Control" />--%>
                                                <Styles>                                                               
                                                 <SelectedRow  CssClass="background_texto_btn background_color_btn"/>
                                                 <Row Font-Size="11px" />
                                                 <AlternatingRow Enabled="True" />
                                                 <PagerTopPanel Paddings-PaddingBottom="3px">
                                                     <Paddings PaddingBottom="3px" />
                                                     </PagerTopPanel>
                                                 </Styles>
                                                <SettingsPager Position="TopAndBottom" PageSize="10">
                                                </SettingsPager>
                                                <SettingsPager Visible="true"></SettingsPager>                                                
                                                <Toolbars>
                                                    <dx:GridViewToolbar Name="Toolbar1" ItemAlign="Left"  EnableAdaptivity="true" >
                                                    <Items>
                                                        <dx:GridViewToolbarItem Name="Links">
                                                        <Template>
                                                            <div style="padding-left:5px; background-color:#E7E7E7; color:#5E5E5E; width:100%">
                                                                <dx:ASPxLabel ID="LblTit_Gravamen" runat="server" Text="GRAVAMEN" Font-Size="11px" />
                                                            </div>
                                                        </Template>
                                                        </dx:GridViewToolbarItem>                                                                         
                                                    </Items>
                                                    </dx:GridViewToolbar>
                                                </Toolbars>                                                                                                                                              
                                             </dx:ASPxGridView>
                                             <br />
                                             <dx:ASPxGridView ID="GridPartidaTasas" runat="server" EnableTheming="True" KeyFieldName="PARTIDA" Theme="SoftOrange" Width="100%" 
                                                StylesToolbar-Item-BackColor="#E7E7E7" StylesToolbar-Item-ForeColor="#5E5E5E" StylesToolbar-Item-Width="100%"
                                                Styles-Header-Font-Size="11px" Settings-HorizontalScrollBarMode="Auto">
                                                <Columns>                                                                                                    
                                                     <dx:GridViewDataTextColumn Caption="PARTIDA" FieldName="PARTIDA" ReadOnly="True" VisibleIndex="1" Width="100px" >
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                         <CellStyle HorizontalAlign="Center" />
                                                     </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="IMPORTE" FieldName="IMPORTE" ReadOnly="True" VisibleIndex="2" Width="100px">
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                        <CellStyle HorizontalAlign="Right" />
                                                        <PropertiesTextEdit DisplayFormatString="{0:n2}" />
                                                     </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataTextColumn Caption="CLAVE <br/> GRAVAMEN" FieldName="CLAVE GRAVAMEN" ReadOnly="True" VisibleIndex="3" Width="100px">
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                         <CellStyle HorizontalAlign="Center" />
                                                     </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataTextColumn Caption="DESCRIPCIÓN <br/> GRAVAMEN" FieldName="DESCRIPCION GRAVAMEN" ReadOnly="True" VisibleIndex="4" Width="230px">
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                         <CellStyle HorizontalAlign="Center" />
                                                     </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataTextColumn Caption="CLAVE FORMA <br/> DE PAGO" FieldName="CLAVE FORMA DE PAGO" ReadOnly="True" VisibleIndex="5" Width="100px">
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                         <CellStyle HorizontalAlign="Center" />
                                                     </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataTextColumn Caption="DESCRIPCIÓN <br/> FORMA DE PAGO" FieldName="DESCRIPCION FORMA DE PAGO" ReadOnly="True" VisibleIndex="6" Width="160px">
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                         <CellStyle HorizontalAlign="Center" />
                                                     </dx:GridViewDataTextColumn>                                                    
                                                 </Columns>
                                                 <SettingsBehavior AllowSelectByRowClick="true" AllowSelectSingleRowOnly="true" />                                                                                                
                                                 <%--<SettingsAdaptivity AdaptivityMode="HideDataCellsWindowLimit" HideDataCellsAtWindowInnerWidth="800" 
                                                    AdaptiveDetailColumnCount="1" AllowOnlyOneAdaptiveDetailExpanded="True">
                                                    <AdaptiveDetailLayoutProperties colcount="1">
                                                        <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit"  SwitchToSingleColumnAtWindowInnerWidth="600" />
                                                    </AdaptiveDetailLayoutProperties>
                                                 </SettingsAdaptivity>                                                 
                                                 <SettingsResizing ColumnResizeMode="Control" />--%>
                                                 <Styles>                                                               
                                                 <SelectedRow CssClass="background_texto_btn background_color_btn" />
                                                 <Row Font-Size="11px" />
                                                 <AlternatingRow Enabled="True" />
                                                 <PagerTopPanel Paddings-PaddingBottom="3px">
                                                     <Paddings PaddingBottom="3px" />
                                                     </PagerTopPanel>
                                                 </Styles>
                                                <SettingsPager Position="TopAndBottom" PageSize="10">
                                                </SettingsPager>
                                                <SettingsPager Visible="true"></SettingsPager>                                                
                                                <Toolbars>
                                                    <dx:GridViewToolbar Name="Toolbar1" ItemAlign="Left"  EnableAdaptivity="true" >
                                                    <Items>
                                                        <dx:GridViewToolbarItem Name="Links">
                                                        <Template>
                                                            <div style="padding-left:5px; background-color:#E7E7E7; color:#5E5E5E; width:100%">
                                                                <dx:ASPxLabel ID="LblTit_Tasas" runat="server" Text="TASAS" Font-Size="11px" />
                                                            </div>
                                                        </Template>
                                                        </dx:GridViewToolbarItem>                                                                         
                                                    </Items>
                                                    </dx:GridViewToolbar>
                                                </Toolbars>                                                                                                                                              
                                             </dx:ASPxGridView>   
                                         </div>
                                     </dx:ContentControl>
                                 </ContentCollection>
                                 </dx:TabPage>
                                 <dx:TabPage Text="Identificadores">
                                     <ContentCollection>
                                     <dx:ContentControl ID="ContentControlP4" runat="server">
                                         <div class="row">
                                             <dx:ASPxGridView ID="GridPartidaIdentificadores" runat="server" KeyFieldName="PARTIDA" Theme="SoftOrange" Width="100%" 
                                                StylesToolbar-Item-BackColor="#E7E7E7" StylesToolbar-Item-ForeColor="#5E5E5E" StylesToolbar-Item-Width="100%"
                                                Styles-Header-Font-Size="11px" Settings-HorizontalScrollBarMode="Auto" EnableTheming="True"
                                                 >
                                                <Columns>                                                                                                    
                                                     <dx:GridViewDataTextColumn Caption="CLAVE <BR/> IDENTIFICADOR" FieldName="CLAVE IDENTIFICADOR" ReadOnly="True" VisibleIndex="1" Width="120px" >
                                                         <HeaderStyle HorizontalAlign="Center" Font-Size="11px" />
                                                         <CellStyle HorizontalAlign="Center" />
                                                     </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataTextColumn Caption="DESCRIPCIÓN" FieldName="DESCRIPCION" ReadOnly="True" VisibleIndex="2" Width="450px">
                                                         <HeaderStyle HorizontalAlign="Center" Font-Size="11px"/>
                                                     </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataTextColumn Caption="COMPLEMENTO 1" FieldName="COMPLEMENTO 1" ReadOnly="True" VisibleIndex="3" Width="110px">
                                                         <HeaderStyle HorizontalAlign="Center" Font-Size="11px" />
                                                     </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataTextColumn Caption="COMPLEMENTO 2" FieldName="COMPLEMENTO 2" ReadOnly="True" VisibleIndex="4" Width="110px">
                                                         <HeaderStyle HorizontalAlign="Center" Font-Size="11px" />
                                                     </dx:GridViewDataTextColumn>
                                                     <dx:GridViewDataTextColumn Caption="COMPLEMENTO 3" FieldName="COMPLEMENTO 3" ReadOnly="True" VisibleIndex="5" Width="110px">
                                                         <HeaderStyle HorizontalAlign="Center" Font-Size="11px" />
                                                     </dx:GridViewDataTextColumn>
                                                 </Columns>
                                                <SettingsBehavior AllowSelectByRowClick="true" AllowSelectSingleRowOnly="true" />                                                                                                
                                                <Styles>                                                               
                                                 <SelectedRow CssClass="background_texto_btn background_color_btn" />
                                                 <Row Font-Size="11px" />
                                                 <AlternatingRow Enabled="True" />
                                                 <PagerTopPanel Paddings-PaddingBottom="3px">
                                                     <Paddings PaddingBottom="3px" />
                                                     </PagerTopPanel>
                                                 </Styles>
                                                <SettingsPager Position="TopAndBottom" PageSize="10">
                                                </SettingsPager>
                                                <SettingsPager Visible="true"></SettingsPager>                                                                                                                                                                                         
                                             </dx:ASPxGridView>
                                         </div>
                                     </dx:ContentControl>
                                 </ContentCollection>
                                 </dx:TabPage>
                                 <dx:TabPage Text="Regulaciones (RRNA)">
                                     <ContentCollection>
                                     <dx:ContentControl ID="ContentControlP5" runat="server">
                                        <div class="row">
                                             <dx:ASPxGridView ID="GridPartidaRegulaciones" runat="server" KeyFieldName="PARTIDA" Theme="SoftOrange" Width="100%" 
                                                StylesToolbar-Item-BackColor="#E7E7E7" StylesToolbar-Item-ForeColor="#5E5E5E" StylesToolbar-Item-Width="100%"
                                                Styles-Header-Font-Size="11px"  Settings-HorizontalScrollBarMode="Auto" EnableTheming="True">
                                                <Columns>                                                                                                    
                                                    <dx:GridViewDataTextColumn Caption="CLAVE" FieldName="CLAVE" ReadOnly="True" VisibleIndex="1" Width="100px" >
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="DESCRIPCIÓN" FieldName="DESCRIPCION" ReadOnly="True" VisibleIndex="2" Width="430px">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="NÚMERO <BR/> PERMISO" FieldName="NUMERO PERMISO" ReadOnly="True" VisibleIndex="3" Width="140px">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="FIRMA <BR/> DESCARGO" FieldName="FIRMA DESCARGO" ReadOnly="True" VisibleIndex="4" Width="140px">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="VALOR COMERCIAL <BR/> DOLARES" FieldName="VALOR COMERCIAL DOLARES" ReadOnly="True" VisibleIndex="5" Width="160px">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="CANTIDAD <BR/> UMC" FieldName="CANTIDAD UMC" ReadOnly="True" VisibleIndex="6" Width="140px">
                                                         <HeaderStyle HorizontalAlign="Center" />
                                                    </dx:GridViewDataTextColumn>
                                                 </Columns>
                                                <SettingsBehavior AllowSelectByRowClick="true" AllowSelectSingleRowOnly="true" />                                                                                                
                                                <Styles>                                                               
                                                 <SelectedRow CssClass="background_texto_btn background_color_btn" />
                                                 <Row Font-Size="11px" />
                                                 <AlternatingRow Enabled="True" />
                                                 <PagerTopPanel Paddings-PaddingBottom="3px">
                                                     <Paddings PaddingBottom="3px" />
                                                     </PagerTopPanel>
                                                 </Styles>
                                                <SettingsPager Position="TopAndBottom" PageSize="10">
                                                </SettingsPager>
                                                <SettingsPager Visible="true"></SettingsPager>                                                                                                                                                                                         
                                             </dx:ASPxGridView>
                                         </div>
                                     </dx:ContentControl>
                                 </ContentCollection>
                                 </dx:TabPage>                                 
                                 <dx:TabPage Text="XML">
                                     <ContentCollection>
                                     <dx:ContentControl ID="ContentControlP7" runat="server">
                                        <div class="row">
                                            <dx:ASPxFormLayout runat="server" ID="ASPxFormLayout_XMLPartida" Font-Size="11px" Width="100%">
                                                <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit" SwitchToSingleColumnAtWindowInnerWidth="1200" />
                                                <Items>
                                                    <dx:LayoutGroup Caption="" ColCount="1" GroupBoxDecoration="None" UseDefaultPaddings="false" SettingsItemCaptions-Location="Top">
                                                        <GroupBoxStyle>
                                                            <Caption Font-Bold="true" />
                                                        </GroupBoxStyle>
                                                        <ParentContainerStyle BackColor="#E8E8E8" CssClass="bordes_curvos" >
                                                            <Paddings Padding="10px" />
                                                        </ParentContainerStyle>                                                    
                                                        <Items>                                                                
                                                            <dx:LayoutItem Caption="&nbsp;&nbsp;&nbsp;&nbsp;XML" Width="100%" >
                                                                <ParentContainerStyle CssClass="background_texto_btn" />
                                                                <CaptionStyle ForeColor="#FFFFFF"  />
                                                                <CaptionCellStyle CssClass="background_color_btn bordes_curvos_top" Paddings-PaddingTop="5px" Paddings-PaddingRight="0px"  />
                                                                <NestedControlCellStyle Paddings-Padding="10px" BackColor="#F9F9F9" CssClass="bordes_curvos_down" />                                                                                                                                                                                                        
                                                                <LayoutItemNestedControlCollection>
                                                                    <dx:LayoutItemNestedControlContainer>
                                                                        <div class="row col-xs-12" style="margin-left:3px; height:300px; width:100%;" >
                                                                            <dx:ASPxMemo ID="MemoPartidaXML" runat="server" Width="100%" Height="100%" ReadOnly="true" CssClass="bordes_curvos" ValidateRequestMode="Disabled" >                                                                                    
                                                                            </dx:ASPxMemo>
                                                                        </div>                                                                         
                                                                    </dx:LayoutItemNestedControlContainer>
                                                                </LayoutItemNestedControlCollection>
                                                                <CaptionSettings />
                                                            </dx:LayoutItem>                                                                                                                                                                                                                                                     
                                                        </Items>
                                                    </dx:LayoutGroup>
                                                </Items>
                                            </dx:ASPxFormLayout>                                             
                                         </div>
                                     </dx:ContentControl>
                                 </ContentCollection>
                                 </dx:TabPage>
                                 <dx:TabPage Text="Códigos">
                                     <ContentCollection>
                                     <dx:ContentControl ID="ContentControlP8" runat="server">
                                        <div class="row">
                                            <dx:ASPxGridView ID="GridPartidaCodigos" runat="server" KeyFieldName="PARTIDA" EnableTheming="True" Theme="SoftOrange" Width="100%" 
                                                StylesToolbar-Item-BackColor="#E7E7E7" StylesToolbar-Item-ForeColor="#5E5E5E" StylesToolbar-Item-Width="100%"
                                                Styles-Header-Font-Size="11px" Settings-HorizontalScrollBarMode="Auto">
                                                <Columns>                                                                                                    
                                                    <dx:GridViewDataTextColumn Caption="FACTURA" FieldName="FACTURA" ReadOnly="True" VisibleIndex="1" Width="140px" >
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="NÚMERO DE <BR/> PARTE" FieldName="NUMERO DE PARTE" ReadOnly="True" VisibleIndex="2" Width="140px">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="ORDEN DE <BR/> COMPRA" FieldName="ORDEN DE COMPRA" ReadOnly="True" VisibleIndex="3" Width="140px">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="LÍNEA DE <BR/> FACTURA" FieldName="LINEA DE FACTURA" ReadOnly="True" VisibleIndex="4" Width="140px">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </dx:GridViewDataTextColumn>                                                    
                                                 </Columns>
                                                <SettingsBehavior AllowSelectByRowClick="true" AllowSelectSingleRowOnly="true" />                                                                                                
                                                <Styles>                                                               
                                                 <SelectedRow CssClass="background_texto_btn background_color_btn" />
                                                 <Row Font-Size="11px" />
                                                 <AlternatingRow Enabled="True" />
                                                 <PagerTopPanel Paddings-PaddingBottom="3px">
                                                     <Paddings PaddingBottom="3px" />
                                                     </PagerTopPanel>
                                                 </Styles>
                                                <SettingsPager Position="TopAndBottom" PageSize="10">
                                                </SettingsPager>
                                                <SettingsPager Visible="true"></SettingsPager>                                                                                                                                                                                         
                                             </dx:ASPxGridView>
                                        </div>
                                     </dx:ContentControl>
                                 </ContentCollection>
                                 </dx:TabPage>
                             </TabPages>
                         </dx:ASPxPageControl>
                    </div>
                    <div class="modal-footer">
                    <div class="col-md-12 col-md-offset-0" style="text-align: center;">
                        <%--<asp:LinkButton ID="btnGuardar" runat="server" CssClass="btn btn-success btn-sm" Text="Guardar" OnClick="btnGuardar_Click">
                            <span class="glyphicon glyphicon-ok"></span>&nbsp;&nbsp;Guardar</asp:LinkButton>
                        <button type="button" class="btn btn-danger btn-sm" data-dismiss="modal">
                            <span class="glyphicon glyphicon-remove"></span>&nbsp;&nbsp;Cancelar
                        </button>--%>
                    </div>
                </div>
                </div>
                <div class="modal-footer">
                  <%--<button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>--%>
                </div>
            </div>
        </div>
    </div>
    <button id="btnModalCU" type="button" data-toggle="modal" data-target="#ModalCU" data-whatever="Nuevo" onclick="return false;" style="display: none;"></button>
    <div class="modal fade" id="ModalCU" tabindex="-1" role="dialog" aria-labelledby="ModalTituloCU" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-sm" role="document" >
            <div class="modal-content">
                <div class="modal-header">
                    <h4 id="ModalTituloCU" class="modal-title" runat="server"></h4>                    
                </div>
                <asp:Panel ID="Panel4" runat="server">
                    <div class="modal-body">
                        <div class="row">
                            <div class="form-group col-md-12">
                                <%--&nbsp;<label id="Label24" runat="server" style="font-size:11px">Busca Usuario</label>--%>
                                <table style="width:100%">
                                    <tr>
                                        <td style="width:5%">
                                            <asp:Label Text="*" Font-Italic="true" runat="server" CssClass="asp-label" ForeColor="Red" />
                                        </td>
                                        <td style="width:95%">
                                            <dx:ASPxComboBox ID="cbxUsuarios" runat="server" Width="100%" DropDownWidth="550"
                                                DropDownStyle="DropDownList" ValueField="IdUsuario" TextField="Nombre"
                                                ValueType="System.String" TextFormatString="{0}" EnableCallbackMode="true" IncrementalFilteringMode="StartsWith"
                                                CallbackPageSize="30">
                                                <Columns>
                                                    <dx:ListBoxColumn Caption="Usuario" FieldName="Nombre" Width="230px" />
                                                    <dx:ListBoxColumn Caption="Perfil" FieldName="NombrePerfil" Width="110px" />
                                                </Columns>
                                            </dx:ASPxComboBox>
                                        </td>                                        
                                    </tr>
                                </table>
                                
                                
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
                            <asp:LinkButton ID="lkb_AceptarCU" runat="server" CssClass="btn btn-primary btn-sm" Text="Aceptar" OnClick="lkb_AceptarCU_Click">
                                <span class="glyphicon glyphicon-ok"></span>&nbsp;&nbsp;Aceptar</asp:LinkButton>
                            <button type="button" class="btn btn-primary btn-sm" data-dismiss="modal">
                                <span class="glyphicon glyphicon-remove"></span>&nbsp;&nbsp;Cancelar
                            </button>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div> 
    <button id="btnModalCE" type="button" data-toggle="modal" data-target="#ModalCE" data-whatever="Nuevo" onclick="return false;" style="display: none;"></button>
    <div class="modal fade" id="ModalCE" tabindex="-1" role="dialog" aria-labelledby="ModalTituloCE" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-sm" role="document" >
            <div class="modal-content">
                <div class="modal-header">
                    <h4 id="ModalTituloCE" class="modal-title" runat="server"></h4>                    
                </div>
                <asp:Panel ID="Panel5" runat="server">
                    <div class="modal-body">
                        <div class="row">
                            <div class="form-group col-md-12">
                                <%--&nbsp;<label id="Label24" runat="server" style="font-size:11px">Busca Usuario</label>--%>
                                <table style="width:100%">
                                    <tr>
                                        <td style="width:5%">
                                            <asp:Label Text="*" Font-Italic="true" runat="server" CssClass="asp-label" ForeColor="Red" />
                                        </td>
                                        <td style="width:95%">
                                            <dx:ASPxComboBox ID="cbxCE" runat="server" Width="90%" DropDownWidth="205"
                                                DropDownStyle="DropDownList" ValueField="GEKEY" TextField="CONCEPTO"
                                                ValueType="System.String" TextFormatString="{0}" EnableCallbackMode="true" IncrementalFilteringMode="StartsWith"
                                                CallbackPageSize="30">
                                                <Columns>
                                                    <dx:ListBoxColumn Caption="CONCEPTO" FieldName="CONCEPTO" Width="110px" />
                                                    <%--<dx:ListBoxColumn Caption="CLAVE" FieldName="CLAVE" Width="60px" />--%>
                                                </Columns>
                                            </dx:ASPxComboBox>
                                        </td>                                        
                                    </tr>
                                </table>
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
                            <asp:LinkButton ID="lkb_AceptarCE" runat="server" CssClass="btn btn-primary btn-sm" Text="Aceptar" OnClick="lkb_AceptarCE_Click">
                                <span class="glyphicon glyphicon-ok"></span>&nbsp;&nbsp;Aceptar</asp:LinkButton>
                            <button type="button" class="btn btn-primary btn-sm" data-dismiss="modal">
                                <span class="glyphicon glyphicon-remove"></span>&nbsp;&nbsp;Cancelar
                            </button>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div>
    <button id="btnModalRE" type="button" data-toggle="modal" data-target="#ModalRE" data-whatever="Nuevo" onclick="return false;" style="display: none;"></button>
    <div class="modal fade" id="ModalRE" tabindex="-1" role="dialog" aria-labelledby="ModalTituloRE" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-sm" role="document" >
            <div class="modal-content">
                <div class="modal-header">
                    <h4 id="ModalTituloRE" class="modal-title" runat="server"></h4>                    
                </div>
                <asp:Panel ID="Panel6" runat="server">
                    <div class="modal-body">
                        <div class="row">
                            <div class="form-group col-md-12">
                                <label id="lblTCuentaRechazo" runat="server" style="font-size:11px"></label>
                            </div>
                            <div class="form-group col-md-12">
                                <dx:ASPxMemo ID="xMemo_Rechazo" runat="server" ClientInstanceName="xMemo_Rechazo" 
                                    CssClass="form-control input-sm" Native="true" MaxLength="1000" placeholder="" Font-Size="11px" Height="60px" ></dx:ASPxMemo>
                                <%--&nbsp;<label id="Label24" runat="server" style="font-size:11px">Busca Usuario</label>
                                <table style="width:100%">

                                    <tr>
                                        <td style="width:5%">
                                            <asp:Label Text="*" Font-Italic="true" runat="server" CssClass="asp-label" ForeColor="Red" />
                                        </td>
                                        <td style="width:95%">
                                            
                                        </td>                                        
                                    </tr>
                                </table>
                                --%>
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
                            <asp:LinkButton ID="lkb_EnviarRechazo" runat="server" CssClass="btn btn-primary btn-sm" Text="Enviar" >
                                <span class="glyphicon glyphicon-envelope"></span>&nbsp;&nbsp;Enviar</asp:LinkButton>
                            <button type="button" class="btn btn-primary btn-sm" data-dismiss="modal">
                                <span class="glyphicon glyphicon-remove"></span>&nbsp;&nbsp;Cancelar
                            </button>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div>
    <button id="btnRefrescaGrid1y2" type="button" onclick="RefrescaGrid1y2(); return false;" style="display: none;"></button>

    <%--
    <%--<asp:Button ID="btnBuscar1" runat="server" Text="Buscar" OnClientClick="return OnClientClickEventHandler();" />
    <dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="lp" Modal="True" Theme="PlasticBlue"  >
    </dx:ASPxLoadingPanel>
    <dx:ASPxCallback ID="callback" runat="server" ClientInstanceName="cb"  OnCallback="callback_Callback" >
        <ClientSideEvents CallbackComplete="OnCallbackComplete" />
    </dx:ASPxCallback>
    --%>   
    <%--<dx:ASPxLabel ID="NotesLabel" runat="server" Text='<%# Eval("Notes") %>'> </dx:ASPxLabel>--%>

    <button id="btnError" type="button" data-toggle="modal" data-target="#AlertError" style="display: none;"></button>
    <div class="modal fade" id="AlertError" tabindex="-1" role="dialog">
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
    <button id="btnQuestionR" type="button" data-toggle="modal" data-target="#AlertQuestionR" style="display: none;"></button>
    <div class="modal fade" id="AlertQuestionR" tabindex="-1" role="dialog" style="top: 25%; outline: none;">
        <div class="modal-dialog modal-sm" role="document">
            <div class="alert alert-info text-center" style="-webkit-box-shadow: 0 5px 15px rgba(0, 0, 0, .5); box-shadow: 0 5px 15px rgba(0, 0, 0, .5);">
                <span class="glyphicon glyphicon-question-sign ico"></span>
                <br />
                <br />
                <p id="pModalQuestionR" runat="server" class="alert-title">
                </p>
                <hr />
                <asp:Button ID="btnOkR" runat="server" CssClass="btn btn-info" OnClick="btnOkR_Click" Text="Aceptar"></asp:Button>
                <button id="btnCancelR" runat="server" class="btn btn-info" data-dismiss="modal">Cancelar</button>
            </div>
        </div>
    </div>
    <button id="btnQuestionED" type="button" data-toggle="modal" data-target="#AlertQuestionED" style="display: none;"></button>
    <div class="modal fade" id="AlertQuestionED" tabindex="-1" role="dialog" style="top: 25%; outline: none;">
        <div class="modal-dialog modal-sm" role="document">
            <div class="alert alert-info text-center" style="-webkit-box-shadow: 0 5px 15px rgba(0, 0, 0, .5); box-shadow: 0 5px 15px rgba(0, 0, 0, .5);">
                <span class="glyphicon glyphicon-question-sign ico"></span>
                <br />
                <br />
                <p id="pModalQuestionED" runat="server" class="alert-title">
                </p>
                <p id="pModalQuestionED2" runat="server" class="alert-title">
                </p>
                <hr />
                <asp:Button ID="btnOkED" runat="server" CssClass="btn btn-info" OnClick="btnOkED_Click" Text="Aceptar"></asp:Button>
                <button id="btnCancelED" runat="server" class="btn btn-info" data-dismiss="modal">Cancelar</button>
            </div>
        </div>
    </div>
    <button id="btnQuestionCU" type="button" data-toggle="modal" data-target="#AlertQuestionCU" style="display: none;"></button>
    <div class="modal fade" id="AlertQuestionCU" tabindex="-1" role="dialog" style="top: 25%; outline: none;">
        <div class="modal-dialog modal-sm" role="document">
            <div class="alert alert-info text-center" style="-webkit-box-shadow: 0 5px 15px rgba(0, 0, 0, .5); box-shadow: 0 5px 15px rgba(0, 0, 0, .5);">
                <span class="glyphicon glyphicon-question-sign ico"></span>
                <br />
                <br />
                <p id="pModalQuestionCU" runat="server" class="alert-title">
                </p>
                <hr />
                <asp:Button ID="btnOkCU" runat="server" CssClass="btn btn-info" Text="Aceptar" OnClick="btnOkCU_Click"></asp:Button>
                <button id="btnCancelCU" runat="server" class="btn btn-info" data-dismiss="modal">Cancelar</button>
            </div>
        </div>
    </div>
    <button id="btnQuestionAU" type="button" data-toggle="modal" data-target="#AlertQuestionAU" style="display: none;"></button>
    <div class="modal fade" id="AlertQuestionAU" tabindex="-1" role="dialog" style="top: 25%; outline: none;">
        <div class="modal-dialog modal-sm" role="document">
            <div class="alert alert-info text-center" style="-webkit-box-shadow: 0 5px 15px rgba(0, 0, 0, .5); box-shadow: 0 5px 15px rgba(0, 0, 0, .5);">
                <span class="glyphicon glyphicon-question-sign ico"></span>
                <br />
                <br />
                <p id="pModalQuestionAU" runat="server" class="alert-title">
                </p>
                <hr />
                <asp:Button ID="btnOkAU" runat="server" CssClass="btn btn-info" Text="Aceptar" OnClick="btnOkAU_Click"></asp:Button>
                <button id="btnCancelAU" runat="server" class="btn btn-info" data-dismiss="modal">Cancelar</button>
            </div>
        </div>
    </div>
    <button id="btnQuestionRE" type="button" data-toggle="modal" data-target="#AlertQuestionRE" style="display: none;"></button>
    <div class="modal fade" id="AlertQuestionRE" tabindex="-1" role="dialog" style="top: 25%; outline: none;">
        <div class="modal-dialog modal-sm" role="document">
            <div class="alert alert-info text-center" style="-webkit-box-shadow: 0 5px 15px rgba(0, 0, 0, .5); box-shadow: 0 5px 15px rgba(0, 0, 0, .5);">
                <span class="glyphicon glyphicon-question-sign ico"></span>
                <br />
                <br />
                <p id="pModalQuestionRE" runat="server" class="alert-title">
                </p>
                <hr />
                <asp:Button ID="btnOkRE" runat="server" CssClass="btn btn-info" Text="Aceptar" OnClick="btnOkRE_Click"></asp:Button>
                <button id="btnCancelRE" runat="server" class="btn btn-info" data-dismiss="modal">Cancelar</button>
            </div>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptSection" runat="server">
</asp:Content>
