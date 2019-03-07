<%@ Page Language="C#" MasterPageFile="~/Presentacion/Principal.Master" AutoEventWireup="true" CodeBehind="IA_HojaCalculo.aspx.cs" Inherits="SICE_BS_WEB.Presentacion.IA_HojaCalculo" %>

<%@ Register assembly="DevExpress.XtraReports.v17.1.Web, Version=17.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraReports.Web" tagprefix="dx" %>

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


        function CancelVistaPrevia() {
            document.getElementById('ContentSection_BtnUpL1').click();
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
            else {
                charCode = this.optional(event) || /^-?(?:\d+|\d{1,3}(?:,\d{3})+)?(?:\,\d+)?$/.test(charCode);
            }

            return true;
        }

        //Grid
        //Se usa para palomear o despalomear el checkbox all del grid Grid
        function chkMarcarTodoClick(s, e) {

            var cheked = s.GetChecked();

            for (var i = 0; i < grid.GetVisibleRowsOnPage() ; i++) {
                var chkConsultar1 = eval("chkConsultar1" + i);
                chkConsultar1.SetChecked(cheked);
            }
        }

        //Grid
        //Se usa para palomear o despalomear los checkbox del grid Grid_MV
        function chkConsultar1_Init(s, e, index) {            
        }

        /*funciones para para validar los checkboxes en Determinación del Método */
        function Get_chk41S(s, e) {
            var cheked = s.GetChecked();

            if (cheked){
                chk41N.SetChecked(false);
            }            
        }
        function Get_chk41N(s, e) {
            var cheked = s.GetChecked();

            if (cheked) {
                chk41S.SetChecked(false);
            }
        }


        function Get_chk42S(s, e) {
            var cheked = s.GetChecked();

            if (cheked) {
                chk42N.SetChecked(false);
            }
        }
        function Get_chk42N(s, e) {
            var cheked = s.GetChecked();

            if (cheked) {
                chk42S.SetChecked(false);
            }
        }


        function Get_chk43S(s, e) {
            var cheked = s.GetChecked();

            if (cheked) {
                chk43N.SetChecked(false);
            }
        }
        function Get_chk43N(s, e) {
            var cheked = s.GetChecked();

            if (cheked) {
                chk43S.SetChecked(false);
            }
        }

        function Get_chk44S(s, e) {
            var cheked = s.GetChecked();

            if (cheked) {
                chk44N.SetChecked(false);
            }
        }
        function Get_chk44N(s, e) {
            var cheked = s.GetChecked();

            if (cheked) {
                chk44S.SetChecked(false);
            }
        }
        
        function Get_chk45S(s, e) {
            var cheked = s.GetChecked();

            if (cheked) {
                chk45N.SetChecked(false);
            }
        }
        function Get_chk45N(s, e) {
            var cheked = s.GetChecked();

            if (cheked) {
                chk45S.SetChecked(false);
            }
        }


        /*Función realiza la suma del punto 05*/
        function ValorNuevoTotal05(s) {
            var pago = se5_PagoDirecto.GetValue();
            var contra = se5_Contraprestacion.GetValue();

            if (isNaN(pago) || !pago || 0 === pago.length) {
                se5_PagoDirecto.SetValue(0);
                pago = 0;
            }

            if (isNaN(contra) || !contra || 0 === contra.length) {
                se5_Contraprestacion.SetValue(0);
                contra = 0;
            }

            var total = parseFloat(pago) + parseFloat(contra);
            document.getElementById("<%= lbl05Total.ClientID %>").innerHTML = formatNumber(total);
        }

        /*Función realiza la suma del punto 06*/
        function ValorNuevoTotal06(s) {
            var comision = se6_Comision.GetValue();
            var flete = se6_FleteSeguro.GetValue();
            var carga = se6_CargaDescarga.GetValue();
            var material = se6_MaterialAportado.GetValue();
            var tecno = se6_TecnoAportada.GetValue();
            var regal = se6_Regalias.GetValue();
            var rever = se6_Reversiones.GetValue();

            if (isNaN(comision) || !comision || 0 === comision.length) {
                se6_Comision.SetValue(0);
                comision = 0;
            }

            if (isNaN(flete) || !flete || 0 === flete.length) {
                se6_FleteSeguro.SetValue(0);
                flete = 0;
            }

            if (isNaN(carga) || !carga || 0 === carga.length) {
                se6_CargaDescarga.SetValue(0);
                carga = 0;
            }

            if (isNaN(material) || !material || 0 === material.length) {
                se6_MaterialAportado.SetValue(0);
                material = 0;
            }

            if (isNaN(tecno) || !tecno || 0 === tecno.length) {
                se6_TecnoAportada.SetValue(0);
                tecno = 0;
            }

            if (isNaN(regal) || !regal || 0 === regal.length) {
                se6_Regalias.SetValue(0);
                regal = 0;
            }

            if (isNaN(rever) || !rever || 0 === rever.length) {
                se6_Reversiones.SetValue(0);
                rever = 0;
            }

            var total = parseFloat(comision) + parseFloat(flete) + parseFloat(carga) + parseFloat(material) + parseFloat(tecno) + parseFloat(regal) + parseFloat(rever);
            document.getElementById("<%= lbl06Total.ClientID %>").innerHTML = formatNumber(total);
        }

        /*Función realiza la suma del punto 07*/
        function ValorNuevoTotal07(s) {
            var norel = se7_GastoNoRelacionado.GetValue();
            var flete = se7_FleteSeguro.GetValue();
            var construc = se7_GastosConstruccion.GetValue();
            var inst = se7_Inst.GetValue();
            var contri = se7_Contribuciones.GetValue();
            var divi = se7_Dividendos.GetValue();

            if (isNaN(norel) || !norel || 0 === norel.length) {
                se7_GastoNoRelacionado.SetValue(0);
                norel = 0;
            }

            if (isNaN(flete) || !flete || 0 === flete.length) {
                se7_FleteSeguro.SetValue(0);
                flete = 0;
            }

            if (isNaN(construc) || !construc || 0 === construc.length) {
                se7_GastosConstruccion.SetValue(0);
                construc = 0;
            }

            if (isNaN(inst) || !inst || 0 === inst.length) {
                se7_Inst.SetValue(0);
                inst = 0;
            }

            if (isNaN(contri) || !contri || 0 === contri.length) {
                se7_Contribuciones.SetValue(0);
                contri = 0;
            }

            if (isNaN(divi) || !divi || 0 === divi.length) {
                se7_Dividendos.SetValue(0);
                divi = 0;
            }

            var total = parseFloat(norel) + parseFloat(flete) + parseFloat(construc) + parseFloat(inst) + parseFloat(contri) + parseFloat(divi);
            document.getElementById("<%= lbl07Total.ClientID %>").innerHTML = formatNumber(total);
        }


        /*Función realiza punto 08*/
        function Valida08(s) {
            var pagar = se8_Pagar.GetValue();
            var ajuste = se8_Ajuste.GetValue();
            var aduana = se8_Aduana.GetValue();
          

            if (isNaN(pagar) || !pagar || 0 === pagar.length) {
                se8_Pagar.SetValue(0);
            }

            if (isNaN(ajuste) || !ajuste || 0 === ajuste.length) {
                se8_Ajuste.SetValue(0);
            }

            if (isNaN(aduana) || !aduana || 0 === aduana.length) {
                se8_Aduana.SetValue(0);
            }

        }

        
        

        /*Función le agrega comas a un número mayor a 999, funciona con decimales */
        function formatNumber(num) {
            return num.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,')
        }


        //Se usa para palomear el checkbox seleccionado y despalomear los demás en Grid de Representante Legal
        function chkConsultarClick(s, e, index) {
            /*var btnRegresarRL = document.getElementById('ContentSection_btnRegresarRL')*/
            var vlkb_EditarRL = document.getElementById('ContentSection_lkb_EditarRL')
            var vlkb_EliminarRL = document.getElementById('ContentSection_lkb_EliminarRL')
              

            /*if (!btnRegresarRL.classList.contains('disabled'))
                btnRegresarRL.classList.add('disabled');*/

            if (!vlkb_EditarRL.classList.contains('disabled'))
                vlkb_EditarRL.classList.add('disabled');

            if (!vlkb_EliminarRL.classList.contains('disabled'))
                vlkb_EliminarRL.classList.add('disabled');

            //document.getElementById('check');
            var cheked = s.GetChecked();
            var row = GridRL.GetDataRow(index);


            var chkConsultar = eval("chkConsultar" + index);
            var valor = chkConsultar.GetChecked();
            chkConsultar.SetChecked(valor);

            for (var i = 0; i < GridRL.GetVisibleRowsOnPage() ; i++) {
                if (i == index) {
                    chkConsultar = eval("chkConsultar" + i);
                    chkConsultar.SetChecked(valor);

                    if (valor == true) {
                        /*btnRegresarRL.classList.remove('disabled');*/
                        vlkb_EditarRL.classList.remove('disabled');
                        vlkb_EliminarRL.classList.remove('disabled');
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

        /*SE CANCELA SU USO*/
        /*Funciones para el dropdown con el grid interno */
        //var GridViewAdjustRequired = true;

        //function DropDownHandler(s, e) {
        //    SynchronizeFocusedRow();

        //    if (GridViewAdjustRequired)
        //        GridView.AdjustControl();

        //     GridViewAdjustRequired = false;
        //}


        //function GridViewInitHandler(s, e) {
        //    SynchronizeFocusedRow();
        //}


        //function RowClickHandler(s, e) {
        //    DropDownEdit.SetKeyValue(GridView.cpKeyValues[e.visibleIndex]);
        //    DropDownEdit.SetText(GridView.cpEmployeeNames[e.visibleIndex]);
        //    //var v=GridView.GetSelectedFieldValues();
        //    //DropDownEdit.HideDropDown();
        //}


        //function EndCallbackHandler(s, e) {
        //    DropDownEdit.AdjustDropDownWindow();
        //    UpdateEditBox();
        //}


        //function SynchronizeFocusedRow() {
        //    var keyValue = DropDownEdit.GetKeyValue();
        //    var index = -1;

        //    if (keyValue != null)
        //        index = ASPxClientUtils.ArrayIndexOf(GridView.cpKeyValues, keyValue);

        //    GridView.SetFocusedRowIndex(index);
        //    GridView.MakeRowVisible(index);

        //    UpdateEditBox();
        //}

        //function UpdateEditBox() {
        //    var rowIndex = GridView.GetFocusedRowIndex();
        //    var focusedEmployeeName = rowIndex == -1 ? "" : GridView.cpEmployeeNames[rowIndex];
        //    var employeeNameInEditBox = DropDownEdit.GetText();

        //    if (employeeNameInEditBox != focusedEmployeeName)
        //        DropDownEdit.SetText(focusedEmployeeName);
        //}

        /************************************************/

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentSection" runat="server">    
<asp:MultiView ID="MultiView1" ActiveViewIndex="0" runat="server">
    <asp:View ID="View1" runat="server">
        <div class="container">
             <div class="panel panel-primary">
                 <div class="panel-heading">
                     <h1 id="h1_titulo" runat="server" class="panel-title small"></h1>
                 </div>
                 <div class="panel-body" >
                     <asp:Panel ID="Panel1" runat="server">
                     <div class="row" style="padding-left:10px; padding-top:10px; padding-right:10px;">                    
                        <div class="col-sm-6 col-md-3">
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
                        <div class="col-sm-6 col-md-1"></div>                        
                        <div class="col-sm-4 col-md-2">
                            <dx:ASPxDateEdit ID="DESDE" runat="server" ClientInstanceName="DESDE" EditFormat="Custom" Date="" Width="120px" Height="30px" Theme="MaterialCompact" 
                                 Font-Size="11px" CssClass="bordes_curvos" NullText="Desde" DisplayFormatString="dd/MM/yyyy">
                                 <CalendarProperties EnableMonthNavigation="True" EnableYearNavigation="True" ShowClearButton="False" ShowDayHeaders="True" ShowTodayButton="False" ShowWeekNumbers="False">
                                     <MonthGridPaddings Padding="0px" />                                                                 
                                     <Style Font-Size="10px"></Style>
                                 </CalendarProperties>
                            </dx:ASPxDateEdit>
                        </div>
                        <div class="col-sm-4 col-md-2">
                            <dx:ASPxDateEdit ID="HASTA" runat="server" ClientInstanceName="HASTA" EditFormat="Custom" Date="" Width="120px" Height="30px" Theme="MaterialCompact" 
                                 Font-Size="11px" CssClass="bordes_curvos" NullText="Hasta" DisplayFormatString="dd/MM/yyyy">
                                 <CalendarProperties EnableMonthNavigation="True" EnableYearNavigation="True" ShowClearButton="False" ShowDayHeaders="True" ShowTodayButton="False" ShowWeekNumbers="False">
                                     <MonthGridPaddings Padding="0px" />                                                                 
                                     <Style Font-Size="10px"></Style>
                                 </CalendarProperties>
                            </dx:ASPxDateEdit>
                        </div>                    
                        <div class="col-sm-6 col-md-2">
                            <div runat="server" id="DivBuscar">
                                <div class="form-group" style="position: relative; width: 50%; float: left;" >
                                    <div class="input-group">
                                        <dx:BootstrapButton ID="btnBuscar" runat="server" AutoPostBack="false" OnClick="btnBuscar_OnClick" 
                                            SettingsBootstrap-RenderOption="Primary" Text="Crear Hoja de Cálculo" CssClasses-Text="txt-sm">
                                            <SettingsBootstrap RenderOption="Primary" Sizing="Small" />
                                            <ClientSideEvents Click="function(s, e) { Callback.PerformCallback(); LoadingPanel1.Show(); }" />
                                        </dx:BootstrapButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-6 col-md-2"></div>
                     </div>
                     <%--<div class="col-sm-12">&nbsp;</div>--%>                                        
                        <dx:ASPxGridView ID="Grid" runat="server" AutoGenerateColumns="False" ClientInstanceName="grid" EnableCallBacks="false" 
                         EnableTheming="True" KeyFieldName="hckey" OnCustomCallback="Grid_CustomCallback" OnToolbarItemClick="Grid_ToolbarItemClick" 
                         Settings-HorizontalScrollBarMode="Auto" Styles-Cell-Font-Size="11px" Styles-Header-Font-Size="11px" SettingsPager-Mode="ShowAllRecords"
                         Theme="SoftOrange" Width="100%" OnCustomButtonCallback="Grid_CustomButtonCallback">
                         <SettingsAdaptivity AdaptiveDetailColumnCount="1" AdaptivityMode="HideDataCellsWindowLimit" AllowOnlyOneAdaptiveDetailExpanded="True" HideDataCellsAtWindowInnerWidth="800">
                             <AdaptiveDetailLayoutProperties colcount="2">
                                 <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit" SwitchToSingleColumnAtWindowInnerWidth="600" />
                             </AdaptiveDetailLayoutProperties>
                         </SettingsAdaptivity>
                         <SettingsResizing ColumnResizeMode="Control" />
                         <SettingsBehavior AllowSelectByRowClick="false" AllowSelectSingleRowOnly="true" />
                        <Settings ShowFooter="False" ShowHeaderFilterButton="true" ShowFilterRowMenu="false"  />
                         <Styles>                                                                                      
                             <SelectedRow CssClass="background_color_btn background_texto_btn" />
                             <Row  />
                             <AlternatingRow Enabled="True" />
                         </Styles>
                         <SettingsPopup>
                             <HeaderFilter Height="200px" Width="195px" />
                         </SettingsPopup>
                         <Columns>
                             <dx:GridViewDataTextColumn FieldName="hckey" ReadOnly="True" Visible="false" VisibleIndex="0">
                             </dx:GridViewDataTextColumn>
                             <dx:GridViewDataColumn Caption="Seleccionar" VisibleIndex="0" Width="70px">
                               <HeaderStyle HorizontalAlign="Center" ForeColor="White" />
                               <HeaderTemplate>
                                   <dx:ASPxCheckBox ID="chkMarcarTodo" ClientInstanceName="chkMarcarTodo" runat="server" OnInit="chkMarcarTodo_Init">
                                    </dx:ASPxCheckBox>
                               </HeaderTemplate>
                               <CellStyle HorizontalAlign="Center"></CellStyle>
                               <DataItemTemplate>
                                   <dx:ASPxCheckBox ID="chkConsultar1" ClientInstanceName="chkConsultar1" runat="server" OnInit="chkConsultar1_Init">                                                                
                                   </dx:ASPxCheckBox>
                               </DataItemTemplate>
                            </dx:GridViewDataColumn>
                             <dx:GridViewCommandColumn Caption=" " VisibleIndex="1" Width="95px">
                                <HeaderStyle HorizontalAlign="Center" />
                                <CustomButtons>
                                    <dx:GridViewCommandColumnCustomButton ID="btnEditar" Text="Editar"
                                        Styles-Style-HoverStyle-ForeColor="#000000" Styles-Style-ForeColor="#000000"
                                        Styles-Style-HoverStyle-Font-Bold="false" Styles-Style-Font-Bold="false">
                                        <Styles>
                                            <Style Font-Bold="False" ForeColor="Black" Font-Size="11px">                                                
                                            </Style>
                                        </Styles>
                                    </dx:GridViewCommandColumnCustomButton>
                                </CustomButtons>
                            </dx:GridViewCommandColumn>
                             <dx:GridViewCommandColumn Caption=" " VisibleIndex="2" Width="95px">
                                <HeaderStyle HorizontalAlign="Center" />
                                <CustomButtons>
                                    <dx:GridViewCommandColumnCustomButton ID="btnEliminar" Text="Eliminar"
                                        Styles-Style-HoverStyle-ForeColor="#000000" Styles-Style-ForeColor="#000000"
                                        Styles-Style-HoverStyle-Font-Bold="false" Styles-Style-Font-Bold="false">
                                        <Styles>
                                            <Style Font-Bold="False" ForeColor="Black" Font-Size="11px">
                                            </Style>
                                        </Styles>
                                    </dx:GridViewCommandColumnCustomButton>
                                </CustomButtons>
                             </dx:GridViewCommandColumn>
                             <dx:GridViewDataColumn Caption="PEDIMENTO ARMADO" FieldName="pedimentoarmado" VisibleIndex="3" Width="230px">
                                 <EditFormSettings Visible="False" />
                                 <HeaderStyle HorizontalAlign="Center" />
                                 <CellStyle HorizontalAlign="Center" />
                                 <DataItemTemplate>
                                     <table>
                                         <tr>
                                             <td style="width:60%">
                                                 <dx:ASPxLabel ID="lbl_Pedimentos" runat="server" Text='<%# Eval("pedimentoarmado") %>' ForeColor="#000" Font-Size="11px" />
                                             </td>
                                             <td style="width:20%">&nbsp;&nbsp;
                                                 <dx:ASPxButton ID="ASPxButtonDetailDoc" runat="server" AutoPostBack="false" OnClick="ASPxButtonDetailDoc_Click"
                                                     EnableTheming="false" RenderMode="Link" VerticalAlign="Top" ToolTip="Hoja Cálculo" OnInit="ASPxButtonDetailDoc_Init">
                                                 </dx:ASPxButton>
                                             </td>
                                             <td style="width:20%">&nbsp;&nbsp;
                                                 <dx:ASPxButton ID="ASPxButtonDetailDocD" runat="server" AutoPostBack="false" OnClick="ASPxButtonDetailDocD_Click"
                                                     EnableTheming="false" RenderMode="Link" VerticalAlign="Top" ToolTip="Detalle" OnInit="ASPxButtonDetailDocD_Init">
                                                 </dx:ASPxButton>
                                             </td>
                                         </tr>
                                      </table>
                                 </DataItemTemplate>                                 
                                 <SettingsHeaderFilter Mode="CheckedList" />
                             </dx:GridViewDataColumn>
                             <%--<dx:GridViewDataTextColumn Caption="NÚMERO DE FACTURA" FieldName="09_facturanumero" ReadOnly="True" VisibleIndex="4" Width="290px">
                                 <HeaderStyle HorizontalAlign="Center" />
                                 <SettingsHeaderFilter Mode="CheckedList" />
                                 <CellStyle HorizontalAlign="Center" />
                             </dx:GridViewDataTextColumn>
                             <dx:GridViewDataDateColumn Caption="RFC" FieldName="01_rfcimportador" ReadOnly="True" VisibleIndex="4" Width="350px">
                                 <HeaderStyle HorizontalAlign="Center" />
                                 <SettingsHeaderFilter Mode="CheckedList" />
                                 <CellStyle HorizontalAlign="Center" />
                             </dx:GridViewDataDateColumn>--%>
                             <dx:GridViewDataDateColumn Caption="FECHA PEDIMENTO" FieldName="09_fechapedimento" ReadOnly="True" VisibleIndex="4" Width="170px">
                                 <HeaderStyle HorizontalAlign="Center" />
                                 <SettingsHeaderFilter Mode="CheckedList" />
                                 <CellStyle HorizontalAlign="Center" />
                             </dx:GridViewDataDateColumn>
                             <dx:GridViewDataDateColumn Caption="FECHA ELABORACIÓN" FieldName="fechaelaboracion" ReadOnly="True" VisibleIndex="5" Width="170px">
                                 <HeaderStyle HorizontalAlign="Center" />
                                 <SettingsHeaderFilter Mode="CheckedList" />
                                 <CellStyle HorizontalAlign="Center" />
                             </dx:GridViewDataDateColumn>
                         </Columns>
                         <Toolbars>
                             <dx:GridViewToolbar EnableAdaptivity="true" ItemAlign="Left" Name="Toolbar1">
                                 <Items>
                                     <dx:GridViewToolbarItem Name="Links">
                                         <Template>
                                             <%--<dx:BootstrapButton ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_OnClick" 
                                        SettingsBootstrap-RenderOption="Primary" AutoPostBack="false" SettingsBootstrap-Sizing="Small" >
                                        <ClientSideEvents Click="function(s, e) { Callback.PerformCallback(); LoadingPanel1.Show(); }" 
                                                          Init="function(s, e) {LoadingPanel1.Hide();}" />
                                        <CssClasses Icon="glyphicon glyphicon-search" /> 
                                    </dx:BootstrapButton>--%>
                                             <dx:BootstrapButton ID="lkb_LimpiarFiltros" runat="server" AutoPostBack="false" CssClasses-Text="txt-sm" data-toggle="tooltip" OnClick="lkb_LimpiarFiltros_Click" SettingsBootstrap-RenderOption="Primary" SettingsBootstrap-Sizing="Small" Text="Limpiar Filtros" ToolTip="Limpiar Filtros">
                                                 <ClientSideEvents Click="function(s, e) { Callback.PerformCallback(); LoadingPanel1.Show(); }" Init="function(s, e) {LoadingPanel1.Hide();}" />
                                                 <CssClasses Icon="glyphicon glyphicon-erase" />
                                             </dx:BootstrapButton>
                                             <asp:LinkButton ID="lkb_Excel" runat="server" CssClass="btn btn-primary btn-sm text-right txt-sm" data-toggle="tooltip" OnClick="lkb_Excel_Click" ToolTip="Exportar a Excel">
                                                <span class="glyphicon glyphicon-list"></span>&nbsp;&nbsp;Excel
                                             </asp:LinkButton>
                                             <dx:BootstrapButton ID="lkb_Actualizar" runat="server" AutoPostBack="false" CssClasses-Text="txt-sm" data-toggle="tooltip" OnClick="lkb_Actualizar_Click" SettingsBootstrap-RenderOption="Primary" SettingsBootstrap-Sizing="Small" Text="Actualizar" ToolTip="Actualizar">
                                                 <ClientSideEvents Click="function(s, e) { Callback.PerformCallback(); LoadingPanel1.Show(); }" Init="function(s, e) {LoadingPanel1.Hide();}" />
                                                 <CssClasses Icon="glyphicon glyphicon-refresh" />
                                             </dx:BootstrapButton>
                                             <%--
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
                     </dx:ASPxGridView>
                     <div style="padding-bottom:5px; padding-top:5px">
                         <button id="btnZip" type="button" style="display: none;" onclick="return window.location = 'FileDownloadHandlerHC.ashx?id=All';">
                         </button>                         
                         <dx:BootstrapButton ID="lkb_DescargarZip" runat="server" AutoPostBack="false" CssClasses-Text="txt-sm" data-toggle="tooltip" OnClick="lkb_DescargarZip_Click" SettingsBootstrap-RenderOption="Primary" SettingsBootstrap-Sizing="Small" Text="Descargar Zip" >
                         <ClientSideEvents Click="function(s, e) { Callback.PerformCallback(); LoadingPanel1.Show(); }" Init="function(s, e) {LoadingPanel1.Hide();}" />
                         <CssClasses Icon="glyphicon glyphicon-save-file" />
                     </dx:BootstrapButton>
                     <asp:Label ID="lblT_TotalRenglones" runat="server" Font-Size="11px" />
                     </div>
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
                 </div>
             </div>
        </div>
    </asp:View>
    <asp:View ID="View2" runat="server">
        <div class="container">
             <div class="panel panel-primary">
                 <div class="panel-heading">
                     <h1 id="h2_titulo" runat="server" class="panel-title small"></h1>
                 </div>
                 <div class="panel-body" >
                    <div class="col-sm-4 col-md-3">
                        <label id="lblTitPedimento" runat="server" style="font-size:11px"></label>
                    </div>
                    <div class="col-sm-4 col-md-4">
                        <table>
                            <tr>
                                <td style="width:30%">
                                    <label id="lblTitReferencia" runat="server" style="font-size:11px">Referencia:</label>
                                </td>
                                <td style="width:70%">
                                    <asp:TextBox ID="txt_Referencia" runat="server" CssClass="form-control input-sm" MaxLength="15" placeholder="Referencia" Font-Size="11px" Width="75%"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        
                        
                    </div>
                    <div class="col-sm-4 col-md-5">

                    </div>
                    
                    <dx:ASPxPageControl runat="server" ID="ASPxPageControl1" Height="60px" Width="98%" EnableCallBacks="true"  ContentStyle-Border-BorderWidth="3px"
                            TabAlign="Justify" ActiveTabIndex="0" EnableTabScrolling="true" Theme="SoftOrange" Font-Size="12px" ContentStyle-VerticalAlign="Top">
                             <TabStyle Paddings-PaddingLeft="10px" Paddings-PaddingRight="10px"  />                                                                                      
                             <ContentStyle>
                                 <Paddings PaddingLeft="20px" PaddingRight="20px" PaddingTop="5px" />
                             </ContentStyle>
                             <TabPages>                                                                            
                                <dx:TabPage Text="Datos del Importador">
                                    <ContentCollection>
                                        <dx:ContentControl ID="ContentControl1" runat="server" >
                                            <div style="height: 240px; overflow: auto">
                                                <div style="padding-top:10px">
                                                    <div class="form-group col-sm-12 col-md-8">
                                                        &nbsp;<label id="Label1" runat="server" style="font-size:11px">Apellido Paterno, Materno, Nombre(s), Denominación o Razón Social</label>
                                                        <asp:TextBox ID="txt01_Nombre" runat="server" CssClass="form-control input-sm" MaxLength="250" placeholder="Apellido Paterno, Materno, Nombre(s), Denominación o Razón Social" Font-Size="11px"></asp:TextBox>
                                                    </div>
                                                    <div class="form-group col-sm-12 col-md-4">
                                                        &nbsp;<label id="Label2" runat="server" style="font-size:11px">RFC</label>
                                                        <asp:TextBox ID="txt01_RFC" runat="server" CssClass="form-control input-sm" MaxLength="50" placeholder="RFC" Font-Size="11px"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group col-sm-12 col-md-10">
                                                    &nbsp;<label id="Label3" runat="server" style="font-size:11px">Calle</label>
                                                    <asp:TextBox ID="txt01_Calle" runat="server" CssClass="form-control input-sm" MaxLength="100" placeholder="Calle" Font-Size="11px"></asp:TextBox>
                                                </div>
                                                <div class="form-group col-sm-12 col-md-2">
                                                    &nbsp;<label id="Label4" runat="server" style="font-size:11px">No. Ext/Int</label>
                                                    <asp:TextBox ID="txt01_NoExtInt" runat="server" CssClass="form-control input-sm" MaxLength="5" placeholder="No. Ext/Int" Font-Size="11px"></asp:TextBox>
                                                </div>
                                                <div class="form-group col-sm-12 col-md-2">
                                                    &nbsp;<label id="Label5" runat="server" style="font-size:11px">Código Postal</label>
                                                    <asp:TextBox ID="txt01_CodigoPostal" runat="server" CssClass="form-control input-sm" MaxLength="5" placeholder="Código Postal" Font-Size="11px"></asp:TextBox>
                                                </div>
                                                <div class="form-group col-sm-12 col-md-10">
                                                    &nbsp;<label id="Label6" runat="server" style="font-size:11px">Entidad o Municipio</label>
                                                    <asp:TextBox ID="txt01_Colonia" runat="server" CssClass="form-control input-sm" MaxLength="50" placeholder="Entidad o Municipio" Font-Size="11px"></asp:TextBox>
                                                </div>
                                            </div> 
                                        </dx:ContentControl>
                                    </ContentCollection>
                                </dx:TabPage>
                                <dx:TabPage Text="Datos del Vendedor">
                                    <ContentCollection>
                                        <dx:ContentControl ID="ContentControl2" runat="server">
                                            <div style="height: 240px; overflow: auto">
                                                <div style="padding-top:10px">
                                                    <div class="form-group col-sm-12 col-md-8">
                                                        &nbsp;<label id="Label7" runat="server" style="font-size:11px">Apellido Paterno, Materno, Nombre(s), Denominación o Razón Social</label>
                                                        <asp:TextBox ID="txt02_Nombre" runat="server" CssClass="form-control input-sm" MaxLength="250" placeholder="Apellido Paterno, Materno, Nombre(s), Denominación o Razón Social" Font-Size="11px"></asp:TextBox>
                                                    </div>
                                                    <div class="form-group col-sm-12 col-md-4">
                                                        &nbsp;<label id="Label8" runat="server" style="font-size:11px">Tax Number</label>
                                                        <asp:TextBox ID="txt02_TaxNumber" runat="server" CssClass="form-control input-sm" MaxLength="150" placeholder="Tax Number" Font-Size="11px"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group col-sm-12 col-md-10">
                                                    &nbsp;<label id="Label9" runat="server" style="font-size:11px">Calle</label>
                                                    <asp:TextBox ID="txt02_Calle" runat="server" CssClass="form-control input-sm" MaxLength="150" placeholder="Calle" Font-Size="11px"></asp:TextBox>
                                                </div>
                                                <div class="form-group col-sm-12 col-md-2">
                                                    &nbsp;<label id="Label10" runat="server" style="font-size:11px">No. Ext/Int</label>
                                                    <asp:TextBox ID="txt02_NoExtInt" runat="server" CssClass="form-control input-sm" MaxLength="50" placeholder="No. Ext/Int" Font-Size="11px"></asp:TextBox>
                                                </div>
                                                <div class="form-group col-sm-12 col-md-6">
                                                    &nbsp;<label id="Label11" runat="server" style="font-size:11px">Ciudad</label>
                                                    <asp:TextBox ID="txt02_Ciudad" runat="server" CssClass="form-control input-sm" MaxLength="150" placeholder="Ciudad" Font-Size="11px"></asp:TextBox>
                                                </div>
                                                <div class="form-group col-sm-12 col-md-6">
                                                    &nbsp;<label id="Label12" runat="server" style="font-size:11px">País</label>
                                                    <asp:TextBox ID="txt02_Pais" runat="server" CssClass="form-control input-sm" MaxLength="150" placeholder="País" Font-Size="11px"></asp:TextBox>
                                                </div>
                                            </div>
                                        </dx:ContentControl>
                                    </ContentCollection>
                                </dx:TabPage>
                                <dx:TabPage Text="Datos de la Mercancia">
                                    <ContentCollection>
                                        <dx:ContentControl ID="ContentControl3" runat="server">
                                            <div style="height: 260px; overflow: auto">    
                                                <dx:ASPxGridView ID="Grid2" runat="server" EnableTheming="True" Theme="SoftOrange"
                                                    EnableCallBacks="false" ClientInstanceName="grid2" AutoGenerateColumns="False" Width="100%" 
                                                    Settings-HorizontalScrollBarMode="Auto" KeyFieldName="detallehckey" SettingsPager-Position="TopAndBottom"
                                                    Styles-Header-ForeColor="#FFF" Styles-Header-Font-Size="11px" Styles-Cell-Font-Size="11px" >
                                                    <SettingsAdaptivity AdaptivityMode="HideDataCellsWindowLimit" HideDataCellsAtWindowInnerWidth="800" 
                                                        AdaptiveDetailColumnCount="1" AllowOnlyOneAdaptiveDetailExpanded="True">
                                                        <AdaptiveDetailLayoutProperties colcount="2">
                                                            <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit"  SwitchToSingleColumnAtWindowInnerWidth="600" />
                                                        </AdaptiveDetailLayoutProperties>
                                                    </SettingsAdaptivity> 
                                                    <SettingsResizing ColumnResizeMode="Control" />
                                                     <SettingsBehavior AllowSelectByRowClick="true" AllowSelectSingleRowOnly="true" />
                                                    <Settings ShowFooter="False" ShowHeaderFilterButton="false" ShowFilterRowMenu="false"  />                                          
                                                    <Styles>                                                                                      
                                                        <SelectedRow CssClass="background_color_btn background_texto_btn" />
                                                        <Row  />
                                                        <AlternatingRow Enabled="True" />
                                                        <PagerTopPanel Paddings-PaddingBottom="3px"></PagerTopPanel>
                                                    </Styles>
                                                    <SettingsPopup>
                                                        <HeaderFilter Height="200px" Width="195px" />
                                                    </SettingsPopup>
                                                    <Columns>
                                                        <dx:GridViewDataTextColumn FieldName="detallehckey" ReadOnly="True"  Visible="false" >
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="Orden" FieldName="orden" ReadOnly="True" VisibleIndex="1" Width="70px" >
                                                            <HeaderStyle HorizontalAlign="Center" ForeColor="#FFF"  />
                                                            <SettingsHeaderFilter Mode="CheckedList" />
                                                            <CellStyle HorizontalAlign="Center"></CellStyle>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="Fracción" FieldName="fraccion" ReadOnly="True" VisibleIndex="2" Width="100px" >
                                                            <HeaderStyle HorizontalAlign="Center" ForeColor="#FFF"  />
                                                            <SettingsHeaderFilter Mode="CheckedList" />
                                                            <CellStyle HorizontalAlign="Center"></CellStyle>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="Descripcion" FieldName="descripcion" ReadOnly="True" VisibleIndex="3" Width="240px" >
                                                            <HeaderStyle HorizontalAlign="Center" ForeColor="#FFF"  />
                                                            <SettingsHeaderFilter Mode="CheckedList" />
                                                            <CellStyle HorizontalAlign="Left"></CellStyle>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="Cantidad" FieldName="cantidad" ReadOnly="True" VisibleIndex="4" Width="90px" >
                                                            <HeaderStyle HorizontalAlign="Center" ForeColor="#FFF"  />
                                                            <SettingsHeaderFilter Mode="CheckedList" />
                                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                                            <PropertiesTextEdit DisplayFormatString="{0:n2}" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="País Producción" FieldName="paisproduccion" ReadOnly="True" VisibleIndex="5" Width="134px" >
                                                            <HeaderStyle HorizontalAlign="Center" ForeColor="#FFF"  />
                                                            <SettingsHeaderFilter Mode="CheckedList" />
                                                            <CellStyle HorizontalAlign="Center"></CellStyle>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="País Procedencia" FieldName="paisprocedencia" ReadOnly="True" VisibleIndex="6" Width="133px" >
                                                            <HeaderStyle HorizontalAlign="Center" ForeColor="#FFF"  />
                                                            <SettingsHeaderFilter Mode="CheckedList" />
                                                            <CellStyle HorizontalAlign="Center"></CellStyle>
                                                        </dx:GridViewDataTextColumn>
                                                    </Columns>
                                                    <Toolbars>
                                                    <dx:GridViewToolbar Name="Toolbar1" EnableAdaptivity="true" ItemAlign="Left">
                                                        <Items>
                                                            <dx:GridViewToolbarItem Name="Links">
                                                            <Template>                                                            
                                                                <asp:LinkButton ID="lkb_Editar" runat="server" OnClick="lkb_EditarDetalle_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button" ToolTip="Editar Mercancía" data-toggle="tooltip">
                                                                    <span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp;Editar
                                                                </asp:LinkButton>                                                                                     
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
                                            </div>  
                                        </dx:ContentControl>
                                    </ContentCollection>
                                </dx:TabPage>

                                <dx:TabPage Text="Anexo Facturas">
                                    <ContentCollection>
                                        <dx:ContentControl ID="ContentControl12" runat="server">
                                            <div style="height: 260px; overflow: auto">    
                                                <dx:ASPxGridView ID="Grid3" runat="server" EnableTheming="True" Theme="SoftOrange"
                                                    EnableCallBacks="false" ClientInstanceName="grid2" AutoGenerateColumns="False" Width="100%" 
                                                    Settings-HorizontalScrollBarMode="Auto" KeyFieldName="facturahckey" SettingsPager-Position="TopAndBottom"
                                                    Styles-Header-ForeColor="#FFF" Styles-Header-Font-Size="11px" Styles-Cell-Font-Size="11px" >
                                                    <SettingsAdaptivity AdaptivityMode="HideDataCellsWindowLimit" HideDataCellsAtWindowInnerWidth="800" 
                                                        AdaptiveDetailColumnCount="1" AllowOnlyOneAdaptiveDetailExpanded="True">
                                                        <AdaptiveDetailLayoutProperties colcount="2">
                                                            <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit"  SwitchToSingleColumnAtWindowInnerWidth="600" />
                                                        </AdaptiveDetailLayoutProperties>
                                                    </SettingsAdaptivity> 
                                                    <SettingsResizing ColumnResizeMode="Control" />
                                                     <SettingsBehavior AllowSelectByRowClick="true" AllowSelectSingleRowOnly="true" />
                                                    <Settings ShowFooter="False" ShowHeaderFilterButton="false" ShowFilterRowMenu="false"  />                                          
                                                    <Styles>                                                                                      
                                                        <SelectedRow CssClass="background_color_btn background_texto_btn" />
                                                        <Row  />
                                                        <AlternatingRow Enabled="True" />
                                                        <PagerTopPanel Paddings-PaddingBottom="3px"></PagerTopPanel>
                                                    </Styles>
                                                    <SettingsPopup>
                                                        <HeaderFilter Height="200px" Width="195px" />
                                                    </SettingsPopup>
                                                    <Columns>
                                                        <dx:GridViewDataTextColumn FieldName="facturahckey" ReadOnly="True"  Visible="false" >
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="Factura" FieldName="Factura" ReadOnly="True" VisibleIndex="1" Width="110px" >
                                                            <HeaderStyle HorizontalAlign="Center" ForeColor="#FFF"  />
                                                            <SettingsHeaderFilter Mode="CheckedList" />
                                                            <CellStyle HorizontalAlign="Center"></CellStyle>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="Lugar Emisión" FieldName="lugaremision" ReadOnly="True" VisibleIndex="2" Width="110px" >
                                                            <HeaderStyle HorizontalAlign="Center" ForeColor="#FFF"  />
                                                            <SettingsHeaderFilter Mode="CheckedList" />
                                                            <CellStyle HorizontalAlign="Center"></CellStyle>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="Subdivisión" FieldName="subdivision" ReadOnly="True" VisibleIndex="3" Width="90px" >
                                                            <HeaderStyle HorizontalAlign="Center" ForeColor="#FFF"  />
                                                            <SettingsHeaderFilter Mode="CheckedList" />
                                                            <CellStyle HorizontalAlign="Center"></CellStyle>                                                            
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="Proveedor" FieldName="proveedor" ReadOnly="True" VisibleIndex="4" Width="330px" >
                                                            <HeaderStyle HorizontalAlign="Center" ForeColor="#FFF"  />
                                                            <SettingsHeaderFilter Mode="CheckedList" />
                                                            <CellStyle HorizontalAlign="Center"></CellStyle>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="Tax Número" FieldName="taxnumero" ReadOnly="True" VisibleIndex="5" Width="110px" >
                                                            <HeaderStyle HorizontalAlign="Center" ForeColor="#FFF"  />
                                                            <SettingsHeaderFilter Mode="CheckedList" />
                                                            <CellStyle HorizontalAlign="Center"></CellStyle>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="Factura Calle" FieldName="facturacalle" ReadOnly="True" VisibleIndex="6" Width="180px" >
                                                            <HeaderStyle HorizontalAlign="Center" ForeColor="#FFF"  />
                                                            <SettingsHeaderFilter Mode="CheckedList" />
                                                            <CellStyle HorizontalAlign="Center"></CellStyle>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="No. Ext/Int" FieldName="facturanumero" ReadOnly="True" VisibleIndex="7" Width="110px" >
                                                            <HeaderStyle HorizontalAlign="Center" ForeColor="#FFF"  />
                                                            <SettingsHeaderFilter Mode="CheckedList" />
                                                            <CellStyle HorizontalAlign="Center"></CellStyle>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="Ciudad" FieldName="facturaciudad" ReadOnly="True" VisibleIndex="8" Width="130px" >
                                                            <HeaderStyle HorizontalAlign="Center" ForeColor="#FFF"  />
                                                            <SettingsHeaderFilter Mode="CheckedList" />
                                                            <CellStyle HorizontalAlign="Center"></CellStyle>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="País" FieldName="facturapais" ReadOnly="True" VisibleIndex="9" Width="110px" >
                                                            <HeaderStyle HorizontalAlign="Center" ForeColor="#FFF"  />
                                                            <SettingsHeaderFilter Mode="CheckedList" />
                                                            <CellStyle HorizontalAlign="Center"></CellStyle>
                                                        </dx:GridViewDataTextColumn>
                                                    </Columns>
                                                    <Toolbars>
                                                    <dx:GridViewToolbar Name="Toolbar1" EnableAdaptivity="true" ItemAlign="Left">
                                                        <Items>
                                                            <dx:GridViewToolbarItem Name="Links">
                                                            <Template>                                                            
                                                                <asp:LinkButton ID="lkb_EditarFactura" runat="server" OnClick="lkb_EditarFactura_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button" ToolTip="Editar Factura" data-toggle="tooltip">
                                                                    <span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp;Editar
                                                                </asp:LinkButton>                                                                                     
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
                                            </div>  
                                        </dx:ContentControl>
                                    </ContentCollection>
                                </dx:TabPage>

                                <dx:TabPage Text="Determinación del Método">
                                    <ContentCollection>
                                        <dx:ContentControl ID="ContentControl4" runat="server">
                                            <div style="height: 240px; overflow: auto">
                                                <div style="padding-top:10px">
                                                    <div class="form-group col-sm-12 col-md-7">
                                                        <label id="Label20" runat="server" style="font-size:11px">1.¿Es compraventa para importación a territorio nacional?</label>                                                    
                                                    </div>
                                                    <div class="form-group col-sm-12 col-md-5">
                                                        <label id="Label21" runat="server" style="font-size:11px">Si</label>
                                                        <dx:ASPxCheckBox ID="chk41S" ClientInstanceName="chk41S" Enabled="true" runat="server" >
                                                            <ClientSideEvents CheckedChanged="function(s, e) {Get_chk41S(s, e); }" />                                                              
                                                        </dx:ASPxCheckBox>
                                                        &nbsp;&nbsp;
                                                        <label id="Label22" runat="server" style="font-size:11px">No</label>
                                                        <dx:ASPxCheckBox ID="chk41N" ClientInstanceName="chk41N" Enabled="true" runat="server" >
                                                            <ClientSideEvents CheckedChanged="function(s, e) {Get_chk41N(s, e); }" />                                                               
                                                        </dx:ASPxCheckBox>   
                                                    </div>
                                                </div>
                                                <div class="form-group col-sm-12 col-md-7">
                                                    <label id="Label23" runat="server" style="font-size:11px">2. Únicamente personas vinculadas ¿La vinculación afecta al precio?</label>
                                                </div>
                                                <div class="form-group col-sm-12 col-md-5">
                                                    <label id="Label24" runat="server" style="font-size:11px">Si</label>
                                                    <dx:ASPxCheckBox ID="chk42S" ClientInstanceName="chk42S" Enabled="true" runat="server" >                                                                
                                                        <ClientSideEvents CheckedChanged="function(s, e) {Get_chk42S(s, e); }" />
                                                    </dx:ASPxCheckBox>
                                                    &nbsp;&nbsp;
                                                    <label id="Label25" runat="server" style="font-size:11px">No</label>
                                                    <dx:ASPxCheckBox ID="chk42N" ClientInstanceName="chk42N" Enabled="true" runat="server" >                                                                
                                                        <ClientSideEvents CheckedChanged="function(s, e) {Get_chk42N(s, e); }" />
                                                    </dx:ASPxCheckBox>
                                                </div>
                                                <div class="form-group col-sm-12 col-md-7">
                                                    <label id="Label26" runat="server" style="font-size:11px">3. ¿Existen restricciones?</label>
                                                </div>
                                                <div class="form-group col-sm-12 col-md-5">
                                                    <label id="Label27" runat="server" style="font-size:11px">Si</label>
                                                    <dx:ASPxCheckBox ID="chk43S" ClientInstanceName="chk43S" Enabled="true" runat="server" >                                                                
                                                        <ClientSideEvents CheckedChanged="function(s, e) {Get_chk43S(s, e); }" />
                                                    </dx:ASPxCheckBox>
                                                    &nbsp;&nbsp;
                                                    <label id="Label28" runat="server" style="font-size:11px">No</label>
                                                    <dx:ASPxCheckBox ID="chk43N" ClientInstanceName="chk43N" Enabled="true" runat="server" >
                                                        <ClientSideEvents CheckedChanged="function(s, e) {Get_chk43N(s, e); }" />                                                              
                                                    </dx:ASPxCheckBox>
                                                </div>
                                                <div class="form-group col-sm-12 col-md-7">
                                                    <label id="Label29" runat="server" style="font-size:11px">4. ¿Existen contraprestaciones?</label>
                                                </div>
                                                <div class="form-group col-sm-12 col-md-5">
                                                    <label id="Label30" runat="server" style="font-size:11px">Si</label>
                                                    <dx:ASPxCheckBox ID="chk44S" ClientInstanceName="chk44S" Enabled="true" runat="server" >
                                                        <ClientSideEvents CheckedChanged="function(s, e) {Get_chk44S(s, e); }" />
                                                    </dx:ASPxCheckBox>
                                                    &nbsp;&nbsp;
                                                    <label id="Label31" runat="server" style="font-size:11px">No</label>
                                                    <dx:ASPxCheckBox ID="chk44N" ClientInstanceName="chk44N" Enabled="true" runat="server" >
                                                        <ClientSideEvents CheckedChanged="function(s, e) {Get_chk44N(s, e); }" />                                                            
                                                    </dx:ASPxCheckBox>
                                                </div>
                                                <div class="form-group col-sm-12 col-md-7">
                                                    <label id="Label32" runat="server" style="font-size:11px">5. ¿Existen regalias o reversiones?</label>
                                                </div>
                                                <div class="form-group col-sm-12 col-md-5">
                                                    <label id="Label33" runat="server" style="font-size:11px">Si</label>
                                                    <dx:ASPxCheckBox ID="chk45S" ClientInstanceName="chk45S" Enabled="true" runat="server" >
                                                        <ClientSideEvents CheckedChanged="function(s, e) {Get_chk45S(s, e); }" />                                                                
                                                    </dx:ASPxCheckBox>
                                                    &nbsp;&nbsp;
                                                    <label id="Label34" runat="server" style="font-size:11px">No</label>
                                                    <dx:ASPxCheckBox ID="chk45N" ClientInstanceName="chk45N" Enabled="true" runat="server" >
                                                        <ClientSideEvents CheckedChanged="function(s, e) {Get_chk45N(s, e); }" />                                                                
                                                    </dx:ASPxCheckBox>
                                                </div>
                                            </div>
                                        </dx:ContentControl>
                                    </ContentCollection>
                                </dx:TabPage>
                                <dx:TabPage Text="Precio Pagado o por Pagar">
                                    <ContentCollection>
                                        <dx:ContentControl ID="ContentControl6" runat="server">
                                            <div style="height: 240px; overflow: auto">
                                                <div style="padding-top:10px">
                                                    <div class="form-group col-sm-12 col-md-4">
                                                        <label id="Label36" runat="server" style="font-size:11px">Moneda en</label>                                                    
                                                    </div>
                                                    <div class="form-group col-sm-12 col-md-8">
                                                        <asp:TextBox ID="txt5_MonedaPago" runat="server" CssClass="form-control input-sm" MaxLength="5" placeholder="Moneda" Font-Size="11px" Width="60px"></asp:TextBox>   
                                                    </div>
                                                </div>
                                                <div class="form-group col-sm-12 col-md-4">
                                                    <label id="Label37" runat="server" style="font-size:11px">Pagos Directos</label>
                                                </div>
                                                <div class="form-group col-sm-12 col-md-8">
                                                    <dx:ASPxSpinEdit ID="se5_PagoDirecto" ClientInstanceName="se5_PagoDirecto" runat="server" NullText="" Width="140px" NumberType="Float" DecimalPlaces="4" DisplayFormatString="N4" 
                                                       MinValue="0" Number="0" ValidateRequestMode="Disabled" ShowOutOfRangeWarning="false" SpinButtons-ShowIncrementButtons="false">
                                                       <ClearButton DisplayMode="OnHover"></ClearButton>
                                                       <ClientSideEvents LostFocus = "function(s,e) { ValorNuevoTotal05(s); }" />
                                                    </dx:ASPxSpinEdit>
                                                </div>
                                                <div class="form-group col-sm-12 col-md-4">
                                                    <label id="Label38" runat="server" style="font-size:11px">Contraprestaciones o Pagos Directos</label>
                                                </div>
                                                <div class="form-group col-sm-12 col-md-8">
                                                    <dx:ASPxSpinEdit ID="se5_Contraprestacion" ClientInstanceName="se5_Contraprestacion" runat="server" NullText="" Width="140px" NumberType="Float" DecimalPlaces="4" DisplayFormatString="N4" 
                                                       MinValue="0" Number="0" ValidateRequestMode="Disabled" ShowOutOfRangeWarning="false" SpinButtons-ShowIncrementButtons="false">
                                                       <ClearButton DisplayMode="OnHover"></ClearButton>
                                                        <ClientSideEvents LostFocus = "function(s,e) { ValorNuevoTotal05(s); }" />
                                                    </dx:ASPxSpinEdit>
                                                </div>
                                                <div class="form-group col-sm-12 col-md-4">
                                                    <label id="Label39" runat="server" style="font-size:11px">Total</label>
                                                </div>
                                                <div class="form-group col-sm-12 col-md-8">
                                                    <label id="lbl05Total" runat="server" style="font-size:11px"></label>
                                                    <%--<dx:ASPxSpinEdit ID="se5_Total" runat="server" NullText="" Width="140px" NumberType="Float" DecimalPlaces="4" DisplayFormatString="N4" 
                                                       MinValue="0" ValidateRequestMode="Disabled" ShowOutOfRangeWarning="false" SpinButtons-ShowIncrementButtons="false">
                                                       <ClearButton DisplayMode="OnHover"></ClearButton>
                                                    </dx:ASPxSpinEdit>--%>
                                                </div>                                                
                                            </div>
                                        </dx:ContentControl>
                                    </ContentCollection>
                                </dx:TabPage>
                                <dx:TabPage Text="Ajustes Auto Incrementables">
                                   <ContentCollection>
                                       <dx:ContentControl ID="ContentControl7" runat="server">
                                           <div style="height: 240px; overflow: auto">
                                               <div style="padding-top:10px">
                                                   <div class="form-group col-sm-12 col-md-3">
                                                       <label id="Label40" runat="server" style="font-size:11px">Moneda en</label>                                                    
                                                   </div>
                                                   <div class="form-group col-sm-12 col-md-9">
                                                       <asp:TextBox ID="txt6_Moneda" runat="server" CssClass="form-control input-sm" MaxLength="5" placeholder="Moneda" Font-Size="11px" Width="60px"></asp:TextBox>   
                                                   </div>
                                               </div>
                                               <div class="form-group col-sm-12 col-md-3">
                                                   <label id="Label41" runat="server" style="font-size:11px">Comisiones</label>
                                               </div>
                                               <div class="form-group col-sm-12 col-md-3">
                                                   <dx:ASPxSpinEdit ID="se6_Comision" ClientInstanceName="se6_Comision" runat="server" NullText="" Width="140px" NumberType="Float" DecimalPlaces="4" DisplayFormatString="N4" 
                                                      MinValue="0" Number="0" ValidateRequestMode="Disabled" ShowOutOfRangeWarning="false" SpinButtons-ShowIncrementButtons="false">
                                                      <ClearButton DisplayMode="OnHover"></ClearButton>
                                                      <ClientSideEvents LostFocus = "function(s,e) { ValorNuevoTotal06(s); }" />
                                                   </dx:ASPxSpinEdit>
                                               </div>
                                               <div class="form-group col-sm-12 col-md-3">
                                                   <label id="Label42" runat="server" style="font-size:11px">Fletes y Seguros</label>
                                               </div>
                                               <div class="form-group col-sm-12 col-md-3">
                                                   <dx:ASPxSpinEdit ID="se6_FleteSeguro" ClientInstanceName="se6_FleteSeguro" runat="server" NullText="" Width="140px" NumberType="Float" DecimalPlaces="4" DisplayFormatString="N4" 
                                                      MinValue="0" Number="0" ValidateRequestMode="Disabled" ShowOutOfRangeWarning="false" SpinButtons-ShowIncrementButtons="false">
                                                      <ClearButton DisplayMode="OnHover"></ClearButton>
                                                       <ClientSideEvents LostFocus = "function(s,e) { ValorNuevoTotal06(s); }" />
                                                   </dx:ASPxSpinEdit>
                                               </div>
                                               <div class="form-group col-sm-12 col-md-3">
                                                   <label id="Label45" runat="server" style="font-size:11px">Carga y Descarga</label>
                                               </div>
                                               <div class="form-group col-sm-12 col-md-3">
                                                   <dx:ASPxSpinEdit ID="se6_CargaDescarga" ClientInstanceName="se6_CargaDescarga" runat="server" NullText="" Width="140px" NumberType="Float" DecimalPlaces="4" DisplayFormatString="N4" 
                                                      MinValue="0" Number="0" ValidateRequestMode="Disabled" ShowOutOfRangeWarning="false" SpinButtons-ShowIncrementButtons="false">
                                                      <ClearButton DisplayMode="OnHover"></ClearButton>
                                                       <ClientSideEvents LostFocus = "function(s,e) { ValorNuevoTotal06(s); }" />
                                                   </dx:ASPxSpinEdit>
                                               </div>
                                               <div class="form-group col-sm-12 col-md-3">
                                                   <label id="Label46" runat="server" style="font-size:11px">Materiales Aportados</label>
                                               </div>
                                               <div class="form-group col-sm-12 col-md-3">
                                                   <dx:ASPxSpinEdit ID="se6_MaterialAportado" ClientInstanceName="se6_MaterialAportado" runat="server" NullText="" Width="140px" NumberType="Float" DecimalPlaces="4" DisplayFormatString="N4" 
                                                      MinValue="0" Number="0" ValidateRequestMode="Disabled" ShowOutOfRangeWarning="false" SpinButtons-ShowIncrementButtons="false">
                                                      <ClearButton DisplayMode="OnHover"></ClearButton>
                                                       <ClientSideEvents LostFocus = "function(s,e) { ValorNuevoTotal06(s); }" />
                                                   </dx:ASPxSpinEdit>
                                               </div>
                                               <div class="form-group col-sm-12 col-md-3">
                                                   <label id="Label47" runat="server" style="font-size:11px">Tecnología Aportada</label>
                                               </div>
                                               <div class="form-group col-sm-12 col-md-3">
                                                   <dx:ASPxSpinEdit ID="se6_TecnoAportada" ClientInstanceName="se6_TecnoAportada" runat="server" NullText="" Width="140px" NumberType="Float" DecimalPlaces="4" DisplayFormatString="N4" 
                                                      MinValue="0" Number="0" ValidateRequestMode="Disabled" ShowOutOfRangeWarning="false" SpinButtons-ShowIncrementButtons="false">
                                                      <ClearButton DisplayMode="OnHover"></ClearButton>
                                                       <ClientSideEvents LostFocus = "function(s,e) { ValorNuevoTotal06(s); }" />
                                                   </dx:ASPxSpinEdit>
                                               </div>
                                               <div class="form-group col-sm-12 col-md-3">
                                                   <label id="Label48" runat="server" style="font-size:11px">Regalias</label>
                                               </div>
                                               <div class="form-group col-sm-12 col-md-3">
                                                   <dx:ASPxSpinEdit ID="se6_Regalias" ClientInstanceName="se6_Regalias" runat="server" NullText="" Width="140px" NumberType="Float" DecimalPlaces="4" DisplayFormatString="N4" 
                                                      MinValue="0" Number="0" ValidateRequestMode="Disabled" ShowOutOfRangeWarning="false" SpinButtons-ShowIncrementButtons="false">
                                                      <ClearButton DisplayMode="OnHover"></ClearButton>
                                                       <ClientSideEvents LostFocus = "function(s,e) { ValorNuevoTotal06(s); }" />
                                                   </dx:ASPxSpinEdit>
                                               </div>
                                               <div class="form-group col-sm-12 col-md-3">
                                                   <label id="Label49" runat="server" style="font-size:11px">Reversiones</label>
                                               </div>
                                               <div class="form-group col-sm-12 col-md-3">
                                                   <dx:ASPxSpinEdit ID="se6_Reversiones" ClientInstanceName="se6_Reversiones" runat="server" NullText="" Width="140px" NumberType="Float" DecimalPlaces="4" DisplayFormatString="N4" 
                                                      MinValue="0" Number="0" ValidateRequestMode="Disabled" ShowOutOfRangeWarning="false" SpinButtons-ShowIncrementButtons="false">
                                                      <ClearButton DisplayMode="OnHover"></ClearButton>
                                                       <ClientSideEvents LostFocus = "function(s,e) { ValorNuevoTotal06(s); }" />
                                                   </dx:ASPxSpinEdit>
                                               </div>
                                               <div class="form-group col-sm-12 col-md-3">
                                                   <label id="Label43" runat="server" style="font-size:11px">Total</label>
                                               </div>
                                               <div class="form-group col-sm-12 col-md-3">
                                                   <label id="lbl06Total" runat="server" style="font-size:11px"></label>
                                               </div>                                                
                                           </div>
                                       </dx:ContentControl>
                                   </ContentCollection>
                                </dx:TabPage>
                                <dx:TabPage Text="No Incrementables">
                                   <ContentCollection>
                                       <dx:ContentControl ID="ContentControl8" runat="server">
                                           <div style="height: 240px; overflow: auto">
                                               <div style="padding-top:10px">
                                                   <div class="form-group col-sm-12 col-md-3">
                                                       <label id="Label44" runat="server" style="font-size:11px">Moneda en</label>                                                    
                                                   </div>
                                                   <div class="form-group col-sm-12 col-md-9">
                                                       <asp:TextBox ID="txt7_Moneda" runat="server" CssClass="form-control input-sm" MaxLength="5" placeholder="Moneda" Font-Size="11px" Width="60px"></asp:TextBox>   
                                                   </div>
                                               </div>
                                               <div class="form-group col-sm-12 col-md-3">
                                                   <label id="Label50" runat="server" style="font-size:11px">Gasto No Relacionado</label>
                                               </div>
                                               <div class="form-group col-sm-12 col-md-3">
                                                   <dx:ASPxSpinEdit ID="se7_GastoNoRelacionado" ClientInstanceName="se7_GastoNoRelacionado" runat="server" NullText="" Width="140px" NumberType="Float" DecimalPlaces="4" DisplayFormatString="N4" 
                                                      MinValue="0" Number="0" ValidateRequestMode="Disabled" ShowOutOfRangeWarning="false" SpinButtons-ShowIncrementButtons="false">
                                                      <ClearButton DisplayMode="OnHover"></ClearButton>
                                                      <ClientSideEvents LostFocus = "function(s,e) { ValorNuevoTotal07(s); }" />
                                                   </dx:ASPxSpinEdit>
                                               </div>
                                               <div class="form-group col-sm-12 col-md-3">
                                                   <label id="Label51" runat="server" style="font-size:11px">Fletes y Seguros</label>
                                               </div>
                                               <div class="form-group col-sm-12 col-md-3">
                                                   <dx:ASPxSpinEdit ID="se7_FleteSeguro" ClientInstanceName="se7_FleteSeguro" runat="server" NullText="" Width="140px" NumberType="Float" DecimalPlaces="4" DisplayFormatString="N4" 
                                                      MinValue="0" Number="0" ValidateRequestMode="Disabled" ShowOutOfRangeWarning="false" SpinButtons-ShowIncrementButtons="false">
                                                      <ClearButton DisplayMode="OnHover"></ClearButton>
                                                       <ClientSideEvents LostFocus = "function(s,e) { ValorNuevoTotal07(s); }" />
                                                   </dx:ASPxSpinEdit>
                                               </div>
                                               <div class="form-group col-sm-12 col-md-3">
                                                   <label id="Label52" runat="server" style="font-size:11px">Gastos Construcción</label>
                                               </div>
                                               <div class="form-group col-sm-12 col-md-3">
                                                   <dx:ASPxSpinEdit ID="se7_GastosConstruccion" ClientInstanceName="se7_GastosConstruccion" runat="server" NullText="" Width="140px" NumberType="Float" DecimalPlaces="4" DisplayFormatString="N4" 
                                                      MinValue="0" Number="0" ValidateRequestMode="Disabled" ShowOutOfRangeWarning="false" SpinButtons-ShowIncrementButtons="false">
                                                      <ClearButton DisplayMode="OnHover"></ClearButton>
                                                       <ClientSideEvents LostFocus = "function(s,e) { ValorNuevoTotal07(s); }" />
                                                   </dx:ASPxSpinEdit>
                                               </div>
                                               <div class="form-group col-sm-12 col-md-3">
                                                   <label id="Label53" runat="server" style="font-size:11px">Inst., Armado, Etc.</label>
                                               </div>
                                               <div class="form-group col-sm-12 col-md-3">
                                                   <dx:ASPxSpinEdit ID="se7_Inst" ClientInstanceName="se7_Inst" runat="server" NullText="" Width="140px" NumberType="Float" DecimalPlaces="4" DisplayFormatString="N4" 
                                                      MinValue="0" Number="0" ValidateRequestMode="Disabled" ShowOutOfRangeWarning="false" SpinButtons-ShowIncrementButtons="false">
                                                      <ClearButton DisplayMode="OnHover"></ClearButton>
                                                       <ClientSideEvents LostFocus = "function(s,e) { ValorNuevoTotal07(s); }" />
                                                   </dx:ASPxSpinEdit>
                                               </div>
                                               <div class="form-group col-sm-12 col-md-3">
                                                   <label id="Label54" runat="server" style="font-size:11px">Constribuciones</label>
                                               </div>
                                               <div class="form-group col-sm-12 col-md-3">
                                                   <dx:ASPxSpinEdit ID="se7_Contribuciones" ClientInstanceName="se7_Contribuciones" runat="server" NullText="" Width="140px" NumberType="Float" DecimalPlaces="4" DisplayFormatString="N4" 
                                                      MinValue="0" Number="0" ValidateRequestMode="Disabled" ShowOutOfRangeWarning="false" SpinButtons-ShowIncrementButtons="false">
                                                      <ClearButton DisplayMode="OnHover"></ClearButton>
                                                       <ClientSideEvents LostFocus = "function(s,e) { ValorNuevoTotal07(s); }" />
                                                   </dx:ASPxSpinEdit>
                                               </div>
                                               <div class="form-group col-sm-12 col-md-3">
                                                   <label id="Label55" runat="server" style="font-size:11px">Dividendos</label>
                                               </div>
                                               <div class="form-group col-sm-12 col-md-3">
                                                   <dx:ASPxSpinEdit ID="se7_Dividendos" ClientInstanceName="se7_Dividendos" runat="server" NullText="" Width="140px" NumberType="Float" DecimalPlaces="4" DisplayFormatString="N4" 
                                                      MinValue="0" Number="0" ValidateRequestMode="Disabled" ShowOutOfRangeWarning="false" SpinButtons-ShowIncrementButtons="false">
                                                      <ClearButton DisplayMode="OnHover"></ClearButton>
                                                       <ClientSideEvents LostFocus = "function(s,e) { ValorNuevoTotal07(s); }" />
                                                   </dx:ASPxSpinEdit>
                                               </div>                                               
                                               <div class="form-group col-sm-12 col-md-3">
                                                   <label id="Label57" runat="server" style="font-size:11px">Total</label>
                                               </div>
                                               <div class="form-group col-sm-12 col-md-3">
                                                   <label id="lbl07Total" runat="server" style="font-size:11px"></label>
                                               </div>                                                
                                           </div>
                                       </dx:ContentControl>
                                   </ContentCollection>
                                </dx:TabPage>
                                <dx:TabPage Text="Valor en Aduana Conforme al Método de Valor de Transacción">
                                    <ContentCollection>
                                        <dx:ContentControl ID="ContentControl9" runat="server">
                                            <div style="height: 240px; overflow: auto">
                                                <div style="padding-top:10px">
                                                   <div class="form-group col-sm-12 col-md-3">
                                                       <label id="Label56" runat="server" style="font-size:11px">Precio Pagado o Por Pagar</label>                                                    
                                                   </div>
                                                   <div class="form-group col-sm-12 col-md-3">
                                                       <dx:ASPxSpinEdit ID="se8_Pagar" ClientInstanceName="se8_Pagar" runat="server" NullText="" Width="140px" NumberType="Float" DecimalPlaces="4" DisplayFormatString="N4" 
                                                           MinValue="0" Number="0" ValidateRequestMode="Disabled" ShowOutOfRangeWarning="false" SpinButtons-ShowIncrementButtons="false">
                                                           <ClearButton DisplayMode="OnHover"></ClearButton>
                                                           <ClientSideEvents LostFocus = "function(s,e) { Valida08(s); }" />
                                                        </dx:ASPxSpinEdit>   
                                                   </div>
                                                   <div class="form-group col-sm-12 col-md-2">
                                                       <label id="Label59" runat="server" style="font-size:11px">Moneda en</label>                                                    
                                                   </div>
                                                   <div class="form-group col-sm-12 col-md-4">
                                                       <asp:TextBox ID="txt8_Moneda1" runat="server" CssClass="form-control input-sm" MaxLength="5" placeholder="Moneda" Font-Size="11px" Width="60px"></asp:TextBox>   
                                                   </div>
                                               </div>
                                               <div class="form-group col-sm-12 col-md-3">
                                                       <label id="Label58" runat="server" style="font-size:11px">Ajustes Incrementables</label>                                                    
                                               </div>
                                               <div class="form-group col-sm-12 col-md-3">
                                                   <dx:ASPxSpinEdit ID="se8_Ajuste" ClientInstanceName="se8_Ajuste" runat="server" NullText="" Width="140px" NumberType="Float" DecimalPlaces="4" DisplayFormatString="N4" 
                                                       MinValue="0" Number="0" ValidateRequestMode="Disabled" ShowOutOfRangeWarning="false" SpinButtons-ShowIncrementButtons="false">
                                                       <ClearButton DisplayMode="OnHover"></ClearButton>
                                                       <ClientSideEvents LostFocus = "function(s,e) { Valida08(s); }" />
                                                    </dx:ASPxSpinEdit>   
                                               </div>
                                               <div class="form-group col-sm-12 col-md-2">
                                                   <label id="Label60" runat="server" style="font-size:11px">Moneda en</label>                                                    
                                               </div>
                                               <div class="form-group col-sm-12 col-md-4">
                                                   <asp:TextBox ID="txt8_Moneda2" runat="server" CssClass="form-control input-sm" MaxLength="5" placeholder="Moneda" Font-Size="11px" Width="60px"></asp:TextBox>   
                                               </div>
                                               <div class="form-group col-sm-12 col-md-3">
                                                       <label id="Label61" runat="server" style="font-size:11px">Valor en Aduana</label>                                                    
                                               </div>
                                               <div class="form-group col-sm-12 col-md-3">
                                                   <dx:ASPxSpinEdit ID="se8_Aduana" ClientInstanceName="se8_Aduana" runat="server" NullText="" Width="140px" NumberType="Float" DecimalPlaces="4" DisplayFormatString="N4" 
                                                       MinValue="0" Number="0" ValidateRequestMode="Disabled" ShowOutOfRangeWarning="false" SpinButtons-ShowIncrementButtons="false">
                                                       <ClearButton DisplayMode="OnHover"></ClearButton>
                                                       <ClientSideEvents LostFocus = "function(s,e) { Valida08(s); }" />
                                                    </dx:ASPxSpinEdit>   
                                               </div>
                                               <div class="form-group col-sm-12 col-md-2">
                                                   <label id="Label62" runat="server" style="font-size:11px">Moneda en</label>                                                    
                                               </div>
                                               <div class="form-group col-sm-12 col-md-4">
                                                   <asp:TextBox ID="txt8_Moneda3" runat="server" CssClass="form-control input-sm" MaxLength="5" placeholder="Moneda" Font-Size="11px" Width="60px"></asp:TextBox>   
                                               </div>
                                            </div>
                                        </dx:ContentControl>
                                    </ContentCollection>
                                </dx:TabPage>
                                <dx:TabPage Text="La Presente Determinación Del Valor es Valida Para">
                                    <ContentCollection>
                                        <dx:ContentControl ID="ContentControl10" runat="server">
                                            
                                            <div class="row"> 

                                                <div class="form-group col-md-12" style="padding-top:10px;">
                                                    <div class="form-group col-sm-12 col-md-3">
                                                        <label id="Label64" runat="server" style="font-size:11px">Fecha Pedimento</label>
                                                    </div>                                                    
                                                    <div class="form-group col-sm-12 col-md-3" >
                                                        <dx:ASPxDateEdit ID="date9_FechaPedimento" runat="server" EditFormat="Custom" Date="" Width="120px" Height="30px" Theme="MaterialCompact" 
                                                             Font-Size="11px" CssClass="bordes_curvos" NullText="Fecha" DisplayFormatString="dd/MM/yyyy">
                                                             <CalendarProperties EnableMonthNavigation="True" EnableYearNavigation="True" ShowClearButton="False" ShowDayHeaders="True" ShowTodayButton="False" ShowWeekNumbers="False">
                                                                 <MonthGridPaddings Padding="0px" />                                                                 
                                                                 <Style Font-Size="10px"></Style>
                                                             </CalendarProperties>
                                                        </dx:ASPxDateEdit>                                                        
                                                    </div>
                                                    <div class="form-group col-sm-12 col-md-3">
                                                        <label id="Label63" runat="server" style="font-size:11px">Pedimento Número</label>
                                                    </div>
                                                    <div class="form-group col-sm-12 col-md-3">
                                                        <asp:TextBox ID="txt9_PediNum" runat="server" CssClass="form-control input-sm" MaxLength="25" placeholder="Pedimento Número" Font-Size="11px" Width="100%"></asp:TextBox>
                                                    </div>                                                                                                        
                                                </div>

                                                <div class="form-group col-md-12">
                                                    <div class="form-group col-sm-12 col-md-3">
                                                        <label id="Label66" runat="server" style="font-size:11px">Fecha Factura</label>                                                    
                                                    </div>
                                                    <div class="form-group col-sm-12 col-md-3">
                                                        <dx:ASPxDateEdit ID="date9_FechaFactura" runat="server" EditFormat="Custom" Date="" Width="120px" Height="30px" Theme="MaterialCompact" 
                                                            Font-Size="11px" CssClass="bordes_curvos" NullText="Fecha" DisplayFormatString="dd/MM/yyyy">
                                                            <CalendarProperties EnableMonthNavigation="True" EnableYearNavigation="True" ShowClearButton="False" ShowDayHeaders="True" ShowTodayButton="False" ShowWeekNumbers="False">
                                                                 <MonthGridPaddings Padding="0px" />                                                                 
                                                                 <Style Font-Size="10px"></Style>
                                                             </CalendarProperties>                                                                                
                                                        </dx:ASPxDateEdit>
                                                    </div>
                                                    <div class="form-group col-sm-12 col-md-3">
                                                        <label id="Label65" runat="server" style="font-size:11px">Factura Número</label>                                                    
                                                    </div>
                                                    <div class="form-group col-sm-12 col-md-3">
                                                        <asp:TextBox ID="txt9_FacturaNumero" runat="server" CssClass="form-control input-sm" MaxLength="50" placeholder="Factura Número" Font-Size="11px" Width="100%"></asp:TextBox>      
                                                    </div>                                                    
                                                </div>
                                                    
                                                <div class="form-group col-md-12">
                                                    <div class="form-group col-sm-12 col-md-3">
                                                        <label id="Label67" runat="server" style="font-size:11px">Cuenta con más de un Pedimento</label>                                                    
                                                    </div>
                                                    <div class="form-group col-sm-12 col-md-3">
                                                        <dx:ASPxCheckBox ID="chk9_MasPedimento" ClientInstanceName="chk9_MasPedimento" Enabled="true" runat="server" >
                                                        </dx:ASPxCheckBox>
                                                    </div>
                                                    <div class="form-group col-sm-12 col-md-3">
                                                        <label id="Label68" runat="server" style="font-size:11px">Lugar de la Emisión de la Factura</label>
                                                    </div>
                                                    <div class="form-group col-sm-12 col-md-3">
                                                        <asp:TextBox ID="txt9_LugarFactura" runat="server" CssClass="form-control input-sm" MaxLength="100" placeholder="Moneda" Font-Size="11px" Width="60px"></asp:TextBox>   
                                                    </div>
                                                </div>
                                                

                                                <div class="form-group col-md-12">
                                                    <div class="form-group col-sm-12 col-md-3">
                                                        <label id="Label69" runat="server" style="font-size:11px">Tipo de Factura Única</label>                                                    
                                                    </div>
                                                    <div class="form-group col-sm-12 col-md-3">
                                                        <dx:ASPxCheckBox ID="chk9_FacturaUnica" ClientInstanceName="chk9_FacturaUnica" Enabled="true" runat="server" >
                                                        </dx:ASPxCheckBox>
                                                    </div>
                                                    <div class="form-group col-sm-12 col-md-3">
                                                        <label id="Label70" runat="server" style="font-size:11px">Subdivisiones</label>
                                                    </div>
                                                    <div class="form-group col-sm-12 col-md-3">
                                                        <dx:ASPxCheckBox ID="chk9_Subdivion" ClientInstanceName="chk9_Subdivion" Enabled="true" runat="server" >
                                                        </dx:ASPxCheckBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <br/>
                                            <br/>
                                            <br/>
                                            <br/>
                                            <br/>
                                            <br/>
                                            <br/>
                                            <br/>
                                            <br/>
                                            <br/>
                                        </dx:ContentControl>
                                    </ContentCollection>
                                </dx:TabPage>
                                <dx:TabPage Text="Valor en Aduana Determinado Según Otros Métodos">
                                    <ContentCollection>
                                        <dx:ContentControl ID="ContentControl5" runat="server">
                                            <div style="height: 240px; overflow: auto">
                                                <div style="padding-top:10px">
                                                    <div class="form-group col-sm-12">
                                                        &nbsp;<label id="Label35" runat="server" style="font-size:11px">Valor en aduana determinado según otros métodos</label>                                               
                                                    </div>
                                                </div>
                                                <div class="form-group col-sm-12">
                                                     <dx:ASPxSpinEdit ID="se_ValorAduana" runat="server" NullText="0% to 100%" Width="80px" NumberType="Integer" DisplayFormatString="{0}%" 
                                                        MinValue="0" MaxValue="100" ValidateRequestMode="Disabled" ShowOutOfRangeWarning="false" SpinButtons-ShowIncrementButtons="false">
                                                        <ClearButton DisplayMode="OnHover"></ClearButton>                                                         
                                                     </dx:ASPxSpinEdit>
                                                    <%--<dx:ASPxTextBox ID="txt_ValorAduana" runat="server" Width="70px" MaxLength="3" Font-Size="11px">
                                                        <MaskSettings Mask="<0..100g>%" IncludeLiterals="None" />
                                                    </dx:ASPxTextBox>--%>
                                                </div>
                                            </div>
                                        </dx:ContentControl>
                                    </ContentCollection>
                                </dx:TabPage>
                                <dx:TabPage Text="Método para la Determinación del Valor en Aduana">
                                    <ContentCollection>
                                        <dx:ContentControl ID="ContentControl11" runat="server">
                                            <div style="height: 240px; overflow: auto">
                                                <div class="col-md-12">
                                                    <div style="padding-top:10px">
                                                        <div class="form-group col-sm-12 col-md-5">
                                                            <label id="Label71" runat="server" style="font-size:11px">Valor de Transición de Mercancías Identicas</label>
                                                        </div>
                                                        <div class="form-group col-sm-12 col-md-1">
                                                            <dx:ASPxCheckBox ID="chk12_Identicas" ClientInstanceName="chk12_Identicas" Enabled="true" runat="server" >
                                                            </dx:ASPxCheckBox>
                                                        </div>
                                                        <div class="form-group col-sm-12 col-md-6">&nbsp;</div>                                                    
                                                    </div>
                                                </div>
                                                <div class="col-md-12">
                                                    <div class="form-group col-sm-12 col-md-5">
                                                        <label id="Label72" runat="server" style="font-size:11px">Valor de Transición de Mercancías Similares</label>
                                                    </div>
                                                    <div class="form-group col-sm-12 col-md-1">
                                                        <dx:ASPxCheckBox ID="chk12_Similares" ClientInstanceName="chk12_Similares" Enabled="true" runat="server" >
                                                        </dx:ASPxCheckBox>
                                                    </div>
                                                    <div class="form-group col-sm-12 col-md-6">&nbsp;</div>
                                                </div>
                                                <div class="col-md-12">
                                                    <div class="form-group col-sm-12 col-md-5">
                                                        <label id="Label73" runat="server" style="font-size:11px">Valor Precio Unitario de Venta</label>
                                                    </div>
                                                    <div class="form-group col-sm-12 col-md-1">
                                                        <dx:ASPxCheckBox ID="chk12_Venta" ClientInstanceName="chk12_Venta" Enabled="true" runat="server" >
                                                        </dx:ASPxCheckBox>
                                                    </div>
                                                    <div class="form-group col-sm-12 col-md-6">&nbsp;</div>
                                                </div>
                                                <div class="col-md-12">
                                                    <div class="form-group col-sm-12 col-md-5">
                                                        <label id="Label74" runat="server" style="font-size:11px">Valor Reconstruido</label>
                                                    </div>
                                                    <div class="form-group col-sm-12 col-md-1">
                                                        <dx:ASPxCheckBox ID="chk12_Reconstruido" ClientInstanceName="chk12_Reconstruido" Enabled="true" runat="server" >
                                                        </dx:ASPxCheckBox>
                                                    </div>
                                                    <div class="form-group col-sm-12 col-md-6">&nbsp;</div>
                                                </div>
                                                <div class="col-md-12">
                                                    <div class="form-group col-sm-12 col-md-5">
                                                        <label id="Label75" runat="server" style="font-size:11px">Valor Determinado Conforme al Artículo 78 de la Ley</label>
                                                    </div>
                                                    <div class="form-group col-sm-12 col-md-1">
                                                        <dx:ASPxCheckBox ID="chk12_Articulo" ClientInstanceName="chk12_Articulo" Enabled="true" runat="server" >
                                                        </dx:ASPxCheckBox>
                                                    </div>
                                                    <div class="form-group col-sm-12 col-md-6">&nbsp;</div>
                                                </div>
                                            </div>
                                        </dx:ContentControl>
                                    </ContentCollection>
                                </dx:TabPage>
                                <dx:TabPage Text="Datos del Representante Legal">
                                    <ContentCollection>
                                        <dx:ContentControl ID="ContentControl13" runat="server">
                                            <div style="height: 305px; overflow: auto">
                                                <div style="padding-top:10px">
                                                    <div class="form-group col-sm-12">
                                                        <asp:LinkButton ID="lkb_RL" runat="server" OnClick="lkb_RL_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button">
                                                             <span class="glyphicon glyphicon-search"></span>&nbsp;&nbsp;Seleccione un Representante Legal
                                                        </asp:LinkButton>
                                                        <%--<asp:TextBox ID="txtRL_Nombre" runat="server" CssClass="form-control input-sm" MaxLength="80" placeholder="Apellido Paterno, Materno, Nombre(s) del Representante Legal" Font-Size="11px" Visible="false"></asp:TextBox>--%>
                                                        <%--<asp:TextBox ID="txtRL_RFC" runat="server" CssClass="form-control input-sm" MaxLength="50" placeholder="RFC" Font-Size="11px" Width="260px" Visible="false"></asp:TextBox>--%>
                                                    </div>

                                                    <div class="form-group col-md-12">                                
                                                        <div class="col-md-3">
                                                            &nbsp;<label id="Label83" runat="server" style="font-size:11px">Nombre</label>
                                                            <asp:TextBox ID="txtRL_Nombre" runat="server" CssClass="form-control input-sm" MaxLength="30" Font-Size="11px" Width="100%" Enabled="false"></asp:TextBox>
                                                        </div>
                                                        <div class="col-md-3">
                                                            &nbsp;<label id="Label84" runat="server" style="font-size:11px">Apellido Paterno</label>
                                                            <asp:TextBox ID="txtRL_Paterno" runat="server" CssClass="form-control input-sm" MaxLength="20"  Font-Size="11px" Width="100%" Enabled="false"></asp:TextBox>    
                                                        </div>
                                                        <div class="col-md-3">
                                                           &nbsp;<label id="Label85" runat="server" style="font-size:11px">Apellido Materno</label>
                                                           <asp:TextBox ID="txtRL_Materno" runat="server" CssClass="form-control input-sm" MaxLength="20"  Font-Size="11px" Width="100%" Enabled="false"></asp:TextBox>    
                                                        </div>
                                                        <div class="col-md-3">
                                                           &nbsp;<label id="Label86" runat="server" style="font-size:11px">RFC</label>
                                                           <asp:TextBox ID="txtRL_RFC" runat="server" CssClass="form-control input-sm" MaxLength="20"  Font-Size="11px" Width="100%" Enabled="false"></asp:TextBox>    
                                                        </div>                                
                                                    </div>
                                                    <div class="form-group col-md-12">                                
                                                        <div class="col-md-3">
                                                            &nbsp;<label id="Label87" runat="server" style="font-size:11px">Apartir de</label>
                                                            <asp:TextBox ID="txtRL_ApartirDe" runat="server" CssClass="form-control input-sm" MaxLength="50"  Font-Size="11px" Width="120px" Enabled="false"></asp:TextBox>                                                            
                                                        </div>
                                                        <div class="col-md-3">
                                                            &nbsp;<label id="Label88" runat="server" style="font-size:11px">Hasta</label>
                                                            <asp:TextBox ID="txtRL_Hasta" runat="server" CssClass="form-control input-sm" MaxLength="50"  Font-Size="11px" Width="120px" Enabled="false"></asp:TextBox>                                                            
                                                        </div>
                                                        <div class="col-md-3">
                                                           &nbsp;<label id="Label89" runat="server" style="font-size:11px">Correo</label>
                                                           <asp:TextBox ID="txtRL_Correo" runat="server" CssClass="form-control input-sm" MaxLength="50"  Font-Size="11px" Width="100%" Enabled="false"></asp:TextBox>    
                                                        </div>
                                                        <div class="col-md-3">
                                                           &nbsp;<label id="Label90" runat="server" style="font-size:11px">Teléfono</label>
                                                           <asp:TextBox ID="txtRL_Tel" runat="server" CssClass="form-control input-sm" MaxLength="15" Font-Size="11px" Width="100%" Enabled="false"></asp:TextBox>    
                                                        </div>                                
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-md-4">
                                                            &nbsp;<label id="Label91" runat="server" style="font-size:11px">Firma</label>
                                                            <dx:ASPxBinaryImage ID="ASPxBinaryImageRL" ClientInstanceName="ClientBinaryImage" Width="200" Height="80"  
                                                            ShowLoadingImage="true" LoadingImageUrl="~/Content/Loading.gif" runat="server" EditingSettings-EmptyValueText="firma" Font-Size="10px" >
                                                            <EditingSettings Enabled="false">
                                                            </EditingSettings>
                                                        </dx:ASPxBinaryImage>
                                                        </div>
                                                        <div class="col-md-2">
                                                            &nbsp;<label id="Label92" runat="server" style="font-size:11px">Mostrar</label>
                                                            <dx:ASPxCheckBox ID="cbx_MostrarFirma" runat="server"  Checked="false" Width="100%">
                                                            </dx:ASPxCheckBox>
                                                        </div>
                                                        <div class="col-md-3">
                                                            &nbsp;<label id="Label93" runat="server" style="font-size:11px">Patente</label>
                                                            <asp:TextBox ID="txt14_Patente" runat="server" CssClass="form-control input-sm" MaxLength="10" placeholder="Patente" Font-Size="11px" Width="100%" ></asp:TextBox>
                                                        </div>
                                                        <div class="col-md-3"></div>
                                                    </div>

                                                </div>                                                  
                                            </div>
                                        </dx:ContentControl>
                                    </ContentCollection>
                                </dx:TabPage>
                             </TabPages>
                    </dx:ASPxPageControl>
                 </div>
                 <div class="panel-footer" align="Center">
                     <asp:LinkButton ID="btnGuardar" runat="server" CssClass="btn btn-primary btn-sm" Text="Guardar" OnClick="btnGuardar_Click" >
                        <span class="glyphicon glyphicon-ok"></span>&nbsp;&nbsp;Guardar</asp:LinkButton>
                     <asp:LinkButton ID="btnCancelar" runat="server" CssClass="btn btn-primary btn-sm" Text="Cancelar" OnClick="btnCancelar_Click">
                        <span class="glyphicon glyphicon-remove"></span>&nbsp;&nbsp;Cancelar</asp:LinkButton>
                 </div>
            </div>
        </div>
    </asp:View>
</asp:MultiView>

    <asp:Button ID="BtnUpL1" runat="server" Style="display: none" OnClick="BtnUpL1_Click" />
    <button id="btnNuevoRpt" type="button" data-toggle="modal" data-target="#Modal1" data-whatever="Nuevo" onclick="return false;" style="display: none;"></button>
    <div class="modal" id="Modal1" tabindex="-1" role="dialog" aria-labelledby="Modal1Titulo" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-lg" role="document" data-backdrop="static">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 id="Modal1Titulo" runat="server"></h4>
                </div>
                <asp:Panel ID="Panel5" runat="server">
                    <div class="modal-body">
                        <iframe id="frmPDF1" style="border: 1px solid #666CCC" title="PDF in an i-Frame" runat="server" frameborder="1" scrolling="auto" height="1100" width="850"></iframe>
                    </div>
                    <div class="modal-footer">
                        <div class="col-md-12 col-md-offset-0" style="text-align: center;">
                            <button type="button" class="btn btn-primary btn-sm" data-dismiss="modal" onclick="CancelVistaPrevia();">
                                <span class="glyphicon glyphicon-remove"></span>&nbsp;&nbsp;Cancelar
                            </button>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div>

    <button id="btnModal" type="button" data-toggle="modal" data-target="#Modal" data-whatever="Nuevo" onclick="return false;" style="display: none;"></button>
    <div class="modal fade" id="Modal" tabindex="-1" role="dialog" aria-labelledby="ModalTitulo" data-backdrop="static" >
        <div class="modal-dialog modal-sm" role="document" data-backdrop="static" >
            <div class="modal-content">
                <div class="modal-header">
                    <h6 id="Modal2Titulo" class="modal-title" runat="server"></h6>                    
                </div>
                <asp:Panel ID="Panel2" runat="server">
                    <div class="modal-body">
                        <div class="row">
                                                          
                                <div class="form-group col-md-12">
                                    &nbsp;<label id="Label13" runat="server" style="font-size:11px">* Fracción</label>
                                    <asp:TextBox ID="txt_fraccion" runat="server" CssClass="form-control input-sm" MaxLength="10" placeholder="* Fracción" Width="100%"></asp:TextBox>
                                </div>                                   
                                <div class="form-group col-md-12">
                                        &nbsp;<label id="Label14" runat="server" style="font-size:11px">Descripción</label>                                                                                     
                                        <asp:TextBox ID="txt_Descripcion" runat="server" CssClass="form-control input-sm" MaxLength="250" placeholder="Descripción" Width="100%"></asp:TextBox>
                                </div>
                                <div class="form-group col-md-12">
                                        &nbsp;<label id="Label15" runat="server" style="font-size:11px">Cantidad</label>                                                                                     
                                        <asp:TextBox ID="txt_cantidad" runat="server" CssClass="form-control input-sm" MaxLength="18" placeholder="Cantidad" 
                                            onkeypress="return onlyDotsAndNumbers(this,event);" Width="100%"></asp:TextBox>
                                </div>
                                <div class="form-group col-md-12">
                                        &nbsp;<label id="Label16" runat="server" style="font-size:11px">País Producción</label>                                                                                     
                                        <asp:TextBox ID="txt_paisprod" runat="server" CssClass="form-control input-sm" MaxLength="100" placeholder="País Producción" Width="100%"></asp:TextBox>
                                </div>
                                <div class="form-group col-md-12">
                                        &nbsp;<label id="Label17" runat="server" style="font-size:11px">País Procedencia</label>                                                                                     
                                        <asp:TextBox ID="txt_paisproc" runat="server" CssClass="form-control input-sm" MaxLength="100" placeholder="País Procedencia" Width="100%"></asp:TextBox>
                                </div>                                
                                <div class="form-group text-right" style="position: relative; width: 95%; float: left;">
                                    <asp:Label Text="* Campo obligatorio" Font-Italic="true" runat="server" CssClass="asp-label" ForeColor="Red" />
                                </div>
                            </div>    
                    </div>                
                    <div class="modal-footer">
                        <div class="col-md-12 col-md-offset-0" style="text-align: center;">
                            <asp:LinkButton ID="LinkButton1" runat="server" CssClass="btn btn-primary btn-sm" Text="Guardar" OnClick="btnGuardarSessionDetalle_Click">
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

    <button id="btnModalF" type="button" data-toggle="modal" data-target="#ModalF" data-whatever="Nuevo" onclick="return false;" style="display: none;"></button>
    <div class="modal fade" id="ModalF" tabindex="-1" role="dialog" aria-labelledby="ModalTitulo" data-backdrop="static" >
        <div class="modal-dialog modal-md" role="document" data-backdrop="static" >
            <div class="modal-content">
                <div class="modal-header">
                    <h6 id="ModalTituloF" class="modal-title" runat="server"></h6>                    
                </div>
                <asp:Panel ID="Panel6" runat="server">
                    <div class="modal-body">
                        <div class="row">                                                          
                                <div class="form-group col-md-6">
                                    &nbsp;<label id="Label94" runat="server" style="font-size:11px">Factura</label>
                                    <asp:TextBox ID="txtF_Factura" runat="server" CssClass="form-control input-sm" MaxLength="50" placeholder="Factura" Width="100%"></asp:TextBox>
                                </div>                                   
                                <div class="form-group col-md-6">
                                        &nbsp;<label id="Label95" runat="server" style="font-size:11px">Lugar Emisión</label>                                                                                     
                                        <asp:TextBox ID="txtF_LugarEmision" runat="server" CssClass="form-control input-sm" MaxLength="150" placeholder="Lugar Emisión" Width="100%"></asp:TextBox>
                                </div>
                                <div class="form-group col-md-6">
                                        &nbsp;<label id="Label96" runat="server" style="font-size:11px">Subdivisión</label>                                                                                     
                                        <asp:TextBox ID="txtF_Subdivision" runat="server" CssClass="form-control input-sm" MaxLength="2" placeholder="Subdivisión" Width="100%"></asp:TextBox>
                                </div>
                                <div class="form-group col-md-6">
                                        &nbsp;<label id="Label97" runat="server" style="font-size:11px">Proveedor</label>                                                                                     
                                        <asp:TextBox ID="txtF_Proveedor" runat="server" CssClass="form-control input-sm" MaxLength="150" placeholder="Proveedor" Width="100%"></asp:TextBox>
                                </div>
                                <div class="form-group col-md-6">
                                        &nbsp;<label id="Label98" runat="server" style="font-size:11px">TAX Número</label>                                                                                     
                                        <asp:TextBox ID="txtF_TaxNumero" runat="server" CssClass="form-control input-sm" MaxLength="20" placeholder="TAX Número" Width="100%"></asp:TextBox>
                                </div>
                                <div class="form-group col-md-6">
                                        &nbsp;<label id="Label99" runat="server" style="font-size:11px">Factura Calle</label>                                                                                     
                                        <asp:TextBox ID="txtF_Calle" runat="server" CssClass="form-control input-sm" MaxLength="150" placeholder="Factura Calle" Width="100%"></asp:TextBox>
                                </div>
                                <div class="form-group col-md-6">
                                        &nbsp;<label id="Label100" runat="server" style="font-size:11px">No. INT/EXT</label>                                                                                     
                                        <asp:TextBox ID="txtF_INTEXT" runat="server" CssClass="form-control input-sm" MaxLength="50" placeholder="No. INT/EXT" Width="100%"></asp:TextBox>
                                </div>
                                <div class="form-group col-md-6">
                                        &nbsp;<label id="Label101" runat="server" style="font-size:11px">Ciudad</label>                                                                                     
                                        <asp:TextBox ID="txtF_Ciudad" runat="server" CssClass="form-control input-sm" MaxLength="150" placeholder="Ciudad" Width="100%"></asp:TextBox>
                                </div>
                                <div class="form-group col-md-6">
                                        &nbsp;<label id="Label102" runat="server" style="font-size:11px">País</label>                                                                                     
                                        <asp:TextBox ID="txtF_Pais" runat="server" CssClass="form-control input-sm" MaxLength="100" placeholder="País" Width="100%"></asp:TextBox>
                                </div>                              
                            </div>    
                    </div>                
                    <div class="modal-footer">
                        <div class="col-md-12 col-md-offset-0" style="text-align: center;">
                            <asp:LinkButton ID="LinkButton4" runat="server" CssClass="btn btn-primary btn-sm" Text="Guardar" OnClick="btnGuardarSessionFactura_Click">
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

    <button id="btnModalRL" type="button" data-toggle="modal" data-target="#ModalRL" data-whatever="Nuevo" onclick="return false;" style="display: none;"></button>
    <div class="modal fade" id="ModalRL" tabindex="-1" role="dialog" aria-labelledby="ModalTituloRL" data-backdrop="static" data-keyboard="false" >
        <div class="modal-dialog modal-xl" role="document" data-backdrop="static" >
            <div class="modal-content">
                <div class="modal-header">
                    <h4 id="ModalTituloRL" class="modal-title" runat="server">Seleccione un Representante Legal</h4>                    
                </div>
                <asp:Panel ID="Panel3" runat="server">
                    <div class="modal-body">
                        <div class="row">
                                
                                <div class="form-group col-sm-12">
                                    <asp:LinkButton ID="lkb_Nuevo" runat="server" OnClick="lkb_NuevoRL_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button" ToolTip="Nuevo Representate Legal" data-toggle="tooltip">
                                        <span class="glyphicon glyphicon-plus"></span>&nbsp;&nbsp;Nuevo
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="lkb_EditarRL" runat="server" OnClick="lkb_EditarRL_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button disabled" ToolTip="Editar Representate Legal" data-toggle="tooltip">
                                        <span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp;Editar
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="lkb_EliminarRL" runat="server" OnClick="lkb_EliminarRL_Click" OnClientClick="alertQuestionR('¿Desea eliminar este representante legal?', this); return false;" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button disabled" ToolTip="Eliminar Representate Legal" data-toggle="tooltip">
                                        <span class="glyphicon glyphicon-remove"></span>&nbsp;&nbsp;Eliminar
                                    </asp:LinkButton>                                  
                                    <dx:ASPxGridView ID="GridRL" runat="server" EnableTheming="True" Theme="SoftOrange" OnToolbarItemClick="Grid_ToolbarItemClick"
                                        OnCustomCallback="GridRL_CustomCallback" EnableCallBacks="false" ClientInstanceName="GridRL"
                                        AutoGenerateColumns="False" Width="100%" Settings-HorizontalScrollBarMode="Auto" KeyFieldName="REPRESENTANTEKEY" SettingsPager-Position="TopAndBottom"
                                        Styles-Header-Font-Size="11px" Styles-Cell-Font-Size="11px" >
                                        <SettingsAdaptivity AdaptivityMode="HideDataCellsWindowLimit" HideDataCellsAtWindowInnerWidth="800" 
                                            AdaptiveDetailColumnCount="1" AllowOnlyOneAdaptiveDetailExpanded="True">
                                            <AdaptiveDetailLayoutProperties colcount="2">
                                                <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit"  SwitchToSingleColumnAtWindowInnerWidth="600" />
                                            </AdaptiveDetailLayoutProperties>
                                        </SettingsAdaptivity> 
                                        <SettingsResizing ColumnResizeMode="Control" />
                                         <SettingsBehavior AllowSelectByRowClick="false" AllowSelectSingleRowOnly="false" />
                                        <Settings ShowFooter="False" ShowHeaderFilterButton="true" ShowFilterRowMenu="false"  />                                          
                                        <Styles>                                                                                      
                                            <SelectedRow  CssClass="background_color_btn background_texto_btn"/>
                                            <Row  />
                                            <AlternatingRow Enabled="True" />
                                            <PagerTopPanel Paddings-PaddingBottom="3px"></PagerTopPanel>
                                        </Styles>
                                        <SettingsPopup>
                                            <HeaderFilter Height="200px" Width="195px" />
                                        </SettingsPopup> 
                                        <%--
                                        <SettingsPager Position="Bottom" ShowDisabledButtons="false" ShowNumericButtons="true" 
                                        <SettingsLoadingPanel Mode="Default" Text="Actualizando" />
                                        <SettingsBehavior AllowSelectByRowClick="true" AllowSelectSingleRowOnly="true" FilterRowMode="Auto"
                                                ProcessSelectionChangedOnServer="true"/>
                                            Summary-Visible="false" ShowSeparators="false" >
                                            <PageSizeItemSettings Items="10, 20, 50" Position="Right" Visible="true"/>
                                        </SettingsPager>
                                        --%>                                             
                                        <Columns>
                                            <dx:GridViewDataTextColumn FieldName="REPRESENTANTEKEY" ReadOnly="True"  Visible="false" >                                
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataColumn Caption="Seleccionar " VisibleIndex="0" Width="70px" >
                                               <HeaderStyle HorizontalAlign="Center" />
                                               <CellStyle HorizontalAlign="Center"></CellStyle>
                                               <DataItemTemplate>
                                                   <dx:ASPxCheckBox ID="chkConsultar" ClientInstanceName="chkConsultar" Enabled="true" runat="server" OnInit="chkConsultar_Init">                                                                
                                                   </dx:ASPxCheckBox>
                                               </DataItemTemplate>
                                            </dx:GridViewDataColumn>                                            
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
                                                                     
                                    <%-- 
                                    <dx:ASPxDropDownEdit ID="DropDownEdit" runat="server" ClientInstanceName="DropDownEdit" Width="100%" Height="60%" AllowUserInput="True" AnimationType="None" CssClass="bordes_curvos_derecha" >
                                        <DropDownWindowStyle HorizontalAlign="Left" VerticalAlign="Top"></DropDownWindowStyle>
                                        <DropDownWindowTemplate>
                                            <dx:ASPxGridView ID="GridView" runat="server" AutoGenerateColumns="False" ClientInstanceName="GridView" 
                                                DataSourceID="AccessDataSource1" KeyFieldName="ProductID" OnRowInserting="GridView_RowInserting" SettingsPager-Position="TopAndBottom"
                                                OnInitNewRow="GridView_InitNewRow" OnCustomJSProperties="GridView_CustomJSProperties" Styles-Header-Font-Size="11px" Styles-Cell-Font-Size="11px"
                                                OnAfterPerformCallback="GridView_AfterPerformCallback" Width="100%">
                                                <SettingsAdaptivity AdaptivityMode="HideDataCellsWindowLimit" HideDataCellsAtWindowInnerWidth="800" 
                                                    AdaptiveDetailColumnCount="1" AllowOnlyOneAdaptiveDetailExpanded="True">
                                                    <AdaptiveDetailLayoutProperties colcount="2">
                                                        <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit"  SwitchToSingleColumnAtWindowInnerWidth="600" />
                                                    </AdaptiveDetailLayoutProperties>
                                                </SettingsAdaptivity> 
                                                <SettingsResizing ColumnResizeMode="Control" />
                                                <SettingsBehavior AllowFocusedRow="True" />
                                                <SettingsPager Mode="ShowPager" />
                                                <Settings ShowFilterRow="true" VerticalScrollableHeight="50" />
                                                <Styles>                                                                                      
                                                    <SelectedRow CssClass="background_color_btn background_texto_btn" />
                                                        <Row  />
                                                        <AlternatingRow Enabled="True" />
                                                        <PagerTopPanel Paddings-PaddingBottom="3px"></PagerTopPanel>
                                                </Styles>
                                                <ClientSideEvents Init="GridViewInitHandler" RowClick="RowClickHandler" EndCallback="EndCallbackHandler" 
                                                    SelectionChanged="function(s, e) {if (e.isSelected) {var key = s.GetRowKey(e.visibleIndex);alert('Last Key = ' + key);}}" />
                                                <Columns>
                                                        <dx:GridViewCommandColumn ShowEditButton="True" VisibleIndex="0"></dx:GridViewCommandColumn>
                                                        <dx:GridViewDataTextColumn FieldName="ProductID" ReadOnly="True" VisibleIndex="1">
                                                            <EditFormSettings Visible="False"></EditFormSettings>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="ProductName" VisibleIndex="2" Width="160px">
                                                            <Settings AllowAutoFilter="true" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="SupplierID" VisibleIndex="3">
                                                            <Settings AllowAutoFilter="true" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="CategoryID" VisibleIndex="4">
                                                            <Settings AllowAutoFilter="true" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="UnitPrice" VisibleIndex="5">
                                                            <Settings AllowAutoFilter="true" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="UnitsInStock" VisibleIndex="6">
                                                            <Settings AllowAutoFilter="true" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="UnitsOnOrder" VisibleIndex="7">
                                                            <Settings AllowAutoFilter="true" />
                                                        </dx:GridViewDataTextColumn>
                                                </Columns>                                                                    
                                            </dx:ASPxGridView>
                                        </DropDownWindowTemplate>
                                    </dx:ASPxDropDownEdit>
                                    <asp:AccessDataSource ID="AccessDataSource1" runat="server" DataFile="~/App_Data/nwind.mdb"
                                        SelectCommand="SELECT [ProductID], [ProductName], [SupplierID], [CategoryID], [UnitPrice], [UnitsInStock], [UnitsOnOrder] FROM [Products]">
                                    </asp:AccessDataSource>--%>
                                </div>   
                        </div>    
                    </div>                
                    <div class="modal-footer">
                        <div class="col-md-12 col-md-offset-0" style="text-align: center;">
                            <asp:LinkButton ID="btnRegresarRL" runat="server" CssClass="btn btn-primary btn-sm" OnClick="btnRegresarRL_Click" >
                                 <span class="glyphicon glyphicon-ok"></span>&nbsp;&nbsp;Aceptar
                             </asp:LinkButton>                           
                            <button type="button" class="btn btn-primary btn-sm" data-dismiss="modal">
                                <span class="glyphicon glyphicon-remove"></span>&nbsp;&nbsp;Cancelar
                            </button>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div>
    
    <button id="btnModalRL2" type="button" data-toggle="modal" data-target="#ModalRL2" data-whatever="Nuevo" onclick="return false;" style="display: none;"></button>
    <div class="modal fade" id="ModalRL2" tabindex="-1" role="dialog" aria-labelledby="ModalTituloRL2" data-backdrop="static" data-keyboard="false" >
        <div class="modal-dialog modal-lg" role="document" >
            <div class="modal-content">
                <div class="modal-header">
                    <h4 id="ModalTituloRL2" class="modal-title" runat="server"></h4>                    
                </div>
                <asp:Panel ID="Panel4" runat="server">
                    <div class="modal-body">
                        <div class="row">
                            <div class="form-group col-md-12">                                
                                <div class="col-md-3">
                                    &nbsp;<label id="Label18" runat="server" style="font-size:11px">* Nombre</label>
                                    <asp:TextBox ID="txtRL2_Nombre" runat="server" CssClass="form-control input-sm" MaxLength="30" placeholder="* Nombre" Font-Size="11px" Width="100%"></asp:TextBox>
                                </div>
                                <div class="col-md-3">
                                    &nbsp;<label id="Label19" runat="server" style="font-size:11px">* Apellido Paterno</label>
                                    <asp:TextBox ID="txtRL2_Paterno" runat="server" CssClass="form-control input-sm" MaxLength="20" placeholder="* Apellido Paterno" Font-Size="11px" Width="100%"></asp:TextBox>    
                                </div>
                                <div class="col-md-3">
                                   &nbsp;<label id="Label76" runat="server" style="font-size:11px">* Apellido Materno</label>
                                   <asp:TextBox ID="txtRL2_Materno" runat="server" CssClass="form-control input-sm" MaxLength="20" placeholder="* Apellido Materno" Font-Size="11px" Width="100%"></asp:TextBox>    
                                </div>
                                <div class="col-md-3">
                                   &nbsp;<label id="Label77" runat="server" style="font-size:11px">* RFC</label>
                                   <asp:TextBox ID="txtRL2_rfc" runat="server" CssClass="form-control input-sm" MaxLength="20" placeholder="* RFC" Font-Size="11px" Width="100%"></asp:TextBox>    
                                </div>                                
                            </div>
                            <div class="form-group col-md-12">                                
                                <div class="col-md-3">
                                    &nbsp;<label id="Label78" runat="server" style="font-size:11px">* Apartir de</label>
                                    <dx:ASPxDateEdit ID="dateRL2_ApartirDe" runat="server" EditFormat="Custom" Date="" Width="120px" Height="30px" Theme="MaterialCompact" 
                                        Font-Size="11px" CssClass="bordes_curvos" NullText="* A partir de" DisplayFormatString="dd/MM/yyyy">
                                        <CalendarProperties EnableMonthNavigation="True" EnableYearNavigation="True" ShowClearButton="False" ShowDayHeaders="True" ShowTodayButton="False" ShowWeekNumbers="False">
                                             <MonthGridPaddings Padding="0px" />
                                             <Style Font-Size="10px"></Style>
                                         </CalendarProperties>
                                    </dx:ASPxDateEdit>
                                </div>
                                <div class="col-md-3">
                                    &nbsp;<label id="Label79" runat="server" style="font-size:11px">* Hasta</label>
                                    <dx:ASPxDateEdit ID="dateRL2_Hasta" runat="server" EditFormat="Custom" Date="" Width="120px" Height="30px" Theme="MaterialCompact" 
                                        Font-Size="11px" CssClass="bordes_curvos" NullText="* Hasta" DisplayFormatString="dd/MM/yyyy">
                                        <CalendarProperties EnableMonthNavigation="True" EnableYearNavigation="True" ShowClearButton="False" ShowDayHeaders="True" ShowTodayButton="False" ShowWeekNumbers="False">
                                             <MonthGridPaddings Padding="0px" />
                                             <Style Font-Size="10px"></Style>
                                         </CalendarProperties>
                                    </dx:ASPxDateEdit>
                                </div>
                                <div class="col-md-3">
                                   &nbsp;<label id="Label80" runat="server" style="font-size:11px">Correo</label>
                                   <asp:TextBox ID="txtRL2_correo" runat="server" CssClass="form-control input-sm" MaxLength="50" placeholder="Correo" Font-Size="11px" Width="100%"></asp:TextBox>    
                                </div>
                                <div class="col-md-3">
                                   &nbsp;<label id="Label81" runat="server" style="font-size:11px">Teléfono</label>
                                   <asp:TextBox ID="txtRL2_tel" runat="server" CssClass="form-control input-sm" MaxLength="15" placeholder="Teléfono" Font-Size="11px" Width="100%"></asp:TextBox>    
                                </div>                                
                            </div>
                            <div class="form-group col-md-12">
                                <div class="col-md-3">
                                    &nbsp;<label id="Label82" runat="server" style="font-size:11px">Firma</label>
                                    <dx:ASPxBinaryImage ID="BinaryImageRL2" ClientInstanceName="ClientBinaryImage" Width="200" Height="80"  
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
                            <asp:LinkButton ID="LinkButton2" runat="server" CssClass="btn btn-primary btn-sm" Text="Guardar" OnClick="btnGuardarRL_Click">
                                <span class="glyphicon glyphicon-ok"></span>&nbsp;&nbsp;Guardar</asp:LinkButton>
                             <asp:LinkButton ID="LinkButton3" runat="server" CssClass="btn btn-primary btn-sm" Text="Cancelar" OnClick="btnCancelarRL_Click">
                                <span class="glyphicon glyphicon-remove"></span>&nbsp;&nbsp;Cancelar</asp:LinkButton>
                            <%--<button type="button" class="btn btn-primary btn-sm" data-dismiss="modal">
                                <span class="glyphicon glyphicon-remove"></span>&nbsp;&nbsp;Cancelar
                            </button>--%>
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

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptSection" runat="server">
</asp:Content>
