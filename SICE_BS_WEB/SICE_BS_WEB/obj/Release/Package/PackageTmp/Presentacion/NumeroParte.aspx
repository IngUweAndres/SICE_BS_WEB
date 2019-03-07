<%@ Page Language="C#" MasterPageFile="~/Presentacion/Principal.Master" AutoEventWireup="true" CodeBehind="NumeroParte.aspx.cs" Inherits="SICE_BS_WEB.Presentacion.NumeroParte" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        body {
            background-color: #FFFFFF;
        }

        .navbar {
            border: none;
            margin-bottom: 10px;
            border-radius: 0 !important;
            border-bottom-left-radius: 6px !important;
            border-bottom-right-radius: 6px !important;
        }
        

        .header {
            color: #000;
            background-color: #F8F8F8; /*color gris claro*/
            border-bottom: 1px solid #eee;
        }

        .header .navbar-nav > li > a {
            color: #5E5E5E;
        }

        .header .navbar-nav > a {
            color: #5E5E5E; /*color gris fuerte*/
        }

        /*Hover sobre menú principal*/
        .header .navbar-nav > li:hover > a {
            color: #000!important; 
            background-color: #E7E7E7; /*color gris medio*/
        }

        .header .navbar-nav > li > a:focus {
            color: #5E5E5E; /*color gris fuerte*/
            background-color: #E7E7E7; /*color gris medio*/
        }

        
        .header .navbar-nav > li:hover .dropdown-InformesGenerales {
            color: #000!Important;
            display:block;
            height:400px;
            width:420px;
            overflow:auto;
            background-color: #F8F8F8;
            padding-left:0px;
            padding-top:5px;
            padding-bottom:5px;
            list-style:none;
             z-index:1000;      
        }

        ul.dropdown-InformesGenerales  {
            display:block;
            color: #000!Important;            
            height:400px;
            width:420px;
            overflow:auto;
            background-color: #F8F8F8;
            padding-left:0px;
            padding-top:5px;
            padding-bottom:5px;
            list-style:none;
             z-index:1000;      
        }

        .header .navbar-nav > li .dropdown-InformesGenerales{
            display:none;
            position:absolute;
            box-shadow: 5px 5px 20px rgba(0,0,0,0.3);
            -moz-box-shadow: 5px 5px 20px rgba(0,0,0,0.3);
            -webkit-box-shadow: 5px 5px 20px rgba(0,0,0,0.3);       
        }

        .header .navbar-nav > li .dropdown-InformesGenerales li{
            display:block;
            padding-top:5px;
            padding-bottom:5px;
            padding-left:20px;
            text-align:left;
        }

        .header .navbar-nav > li .dropdown-InformesGenerales li:hover{
            overflow:hidden;
            color: #000!important; 
            background-color: #E7E7E7; /*color gris medio*/
        }
       
        .header .navbar-nav > li .dropdown-InformesGenerales li > a{
            display:block;
            color: #000!important;
            text-decoration:none;            
            
        }   

        @media only screen and (max-width: 766px) {
            .collapsing, .in {background-color: #f7f7f7; color: #555!important;}
            .collapsing ul li a, .in ul li a {color: #555!important;}
            .collapsing ul li a:hover, .in ul li a:hover {color: #000!important; background-color: #E7E7E7!important;}

            .dropdown-menu {
                background-color: #F8F8F8 !important; /*gris muy claro*/                
            }
            
            .dropdown-menu > li > a:hover {
                color: #000 !important;
                background-color: #E7E7E7; /*gris claro*/
            }

            .dropdown-menu > li > a:focus {
                color: #000 !important;
                background-color: #E7E7E7;
            }

            .dropdown-menu > li > a > img {
                display: inline-block !important;
            }

            .dropdown-menu > li > a {
                color: #000 !important;
            }

            .header .navbar-nav > li .dropdown-InformesGenerales {
                position:relative; 
                width:100%;
                display:compact;
                height:190px;
                overflow:auto;
                z-index:1000;
                padding-top:0px;
                padding-left:0px;
                text-align:left;
            }

            .header .navbar-nav > li:focus .dropdown-InformesGenerales {
                color: #000 !important;
                background-color: #E7E7E7;
                padding-top:0px;
                padding-left:0px;
                text-align:left;
            }

            .header .navbar-nav > li:hover .dropdown-InformesGenerales {
                display:none;
                position:relative; 
                width:100%;
                display:block;
                height:190px;
                overflow:auto;
                z-index:1000;
                padding-top:0px;
                padding-left:0px;
                text-align:left;
            }


            .dropdown-InformesGenerales {
                display:none;
                background-color: #F8F8F8 !important; /*gris muy claro*/
                border-top: 1px solid #1186B8;
                border-bottom: 1px solid #1186B8;
                position:relative;
            }
            
            .dropdown-InformesGenerales > li > a:hover {
                color: #000 !important;
                background-color: #E7E7E7; /*gris claro*/
            }

            .dropdown-InformesGenerales > li > a:focus {
                color: #000 !important;
                background-color: #E7E7E7;
            }

            .dropdown-InformesGenerales > li > a > img {
                display: inline-block !important;
            }

            .dropdown-InformesGenerales > li > a {
                color: #000 !important;

            }         
        }

        .panel-title {
            font-size: 14px !important;
        }

        .panel-primary {
            border-color: #F8F8F8;
            overflow: hidden;
        }

        .panel-primary > .panel-heading {
            color: #FFFFFF;
            background-color: #73C000 !important;
            border-color: #62A000 !important;
            border-width:1px;
        }

        .btn-primary {
            background-color: #73C000 !important;
            border-color: #73C000 !important;
            color: #fff !important;
        }

        .btn-primary:hover {
            background-color: #62A000 !important;
            border-color: #62A000 !important;
            color: #fff !important;
        }
        
        .btn-primary:active:focus {
            background-color: #62A000 !important;
            border-color: #62A000 !important;
            color: #fff !important;
        }

        .color_invisible{
            color: transparent !important;
        }

        .bordes_curvos{
            border-radius: 0px 5px 5px 0px;
             -moz-border-radius: 5px 5px 5px 5px;
            -khtml-border-radius: 5px 5px 5px 5px;
            -webkit-border-radius: 5px 5px 5px 5px;
            overflow: hidden !important;
            overflow-x: hidden !important;
            overflow-y: hidden !important;                 
        }

        .bordes_curvos_derecha{
            border-radius: 0px 5px 5px 0px;
             -moz-border-radius: 0px 5px 5px 0px;
            -khtml-border-radius: 0px 5px 5px 0px;
            -webkit-border-radius: 0px 5px 5px 0px;
            overflow: hidden !important;
            overflow-x: hidden !important;
            overflow-y: hidden !important;         
        }

        /*Hover del filtro del grid*/
        .dxeListBoxItemHover_SoftOrange {
            background-color: #73C000; /*#ff9460;*/
            color: #FFFFFF;
        }

        /*Botón deshabilitado del filtro del grid*/
        .dxbDisabled_SoftOrange, a.dxbButton_SoftOrange.dxbDisabled_SoftOrange {
            color: #9d9d9d;
            cursor: default;
            text-decoration: none;
        }

        .dxbDisabled_SoftOrange {
            background: #cacaca url(/DXR.axd?r=0_5024-owDhf) repeat-x top;
            border-color: #a2a2a2;
        }

        /*Botón habilitado del filtro del grid*/
        .dxbButton_SoftOrange {
            color: #FFFFFF;
            font: 12px Tahoma, Geneva, sans-serif;
            border: 1px solid #95D732; /*#cb4b31;*/
            background: /*#EF643C*/ #73C000 repeat-x top;
            padding: 1px;
        }

        .dxpcLoadingPanel_SoftOrange, .dxlpLoadingPanel_SoftOrange, .dxlpLoadingPanelWithContent_SoftOrange {
            font: 12px Tahoma, Geneva, sans-serif;
            color: #73C000;
            background-color: White;
            border: 1px solid #73C000;
        }

        /*.dxGridView_gvHeaderFilterActive_SoftOrange{
            background-image: url('filtro_verde.png'); 
            width: 21px;
            height: 21px; 
            background-position: 0px 0px;   
            background-position-x: 0px;
            background-position-y: 0px;
        }
        .dxGridView_gvHeaderFilter_SoftOrange{
            background-image: url("filtro_gris.png");
            background-repeat: no-repeat;
            background-color: transparent;
            background-position: 0px 0px;
        }

        .dx-acc-r .dxGridView_gvHeaderFilterActive_SoftOrange:before {
            left: 0px;
            top: 0px;
        }

        .dx-acc-r .dxGridView_CTClearFilter_SoftOrange:before, .dx-acc-r .dxGridView_CTClearFilterDisabled_SoftOrange:before, .dx-acc-r .dxGridView_CTClearGrouping_SoftOrange:before, .dx-acc-r .dxGridView_CTClearGroupingDisabled_SoftOrange:before, .dx-acc-r .dxGridView_CTCollapseDetailRow_SoftOrange:before, .dx-acc-r .dxGridView_CTCollapseDetailRowDisabled_SoftOrange:before, .dx-acc-r .dxGridView_CTCollapseRow_SoftOrange:before, .dx-acc-r .dxGridView_CTCollapseRowDisabled_SoftOrange:before, .dx-acc-r .dxGridView_CTDeleteRow_SoftOrange:before, .dx-acc-r .dxGridView_CTDeleteRowDisabled_SoftOrange:before, .dx-acc-r .dxGridView_CTEditRow_SoftOrange:before, .dx-acc-r .dxGridView_CTEditRowDisabled_SoftOrange:before, .dx-acc-r .dxGridView_CTExpandDetailRow_SoftOrange:before, .dx-acc-r .dxGridView_CTExpandDetailRowDisabled_SoftOrange:before, .dx-acc-r .dxGridView_CTExpandRow_SoftOrange:before, .dx-acc-r .dxGridView_CTExpandRowDisabled_SoftOrange:before, .dx-acc-r .dxGridView_CTFullCollapse_SoftOrange:before, .dx-acc-r .dxGridView_CTFullCollapseDisabled_SoftOrange:before, .dx-acc-r .dxGridView_CTFullExpand_SoftOrange:before, .dx-acc-r .dxGridView_CTFullExpandDisabled_SoftOrange:before, .dx-acc-r .dxGridView_CTNewRow_SoftOrange:before, .dx-acc-r .dxGridView_CTNewRowDisabled_SoftOrange:before, .dx-acc-r .dxGridView_CTRefresh_SoftOrange:before, .dx-acc-r .dxGridView_CTRefreshDisabled_SoftOrange:before, .dx-acc-r .dxGridView_CTShowCustDialog_SoftOrange:before, .dx-acc-r .dxGridView_CTShowCustDialogDisabled_SoftOrange:before, .dx-acc-r .dxGridView_CTShowCustomizationWindow_SoftOrange:before, .dx-acc-r .dxGridView_CTShowCustomizationWindowDisabled_SoftOrange:before, .dx-acc-r .dxGridView_CTShowFilterEditor_SoftOrange:before, .dx-acc-r .dxGridView_CTShowFilterEditorDisabled_SoftOrange:before, .dx-acc-r .dxGridView_CTShowGroupPanel_SoftOrange:before, .dx-acc-r .dxGridView_CTShowGroupPanelDisabled_SoftOrange:before, .dx-acc-r .dxGridView_CTShowSearchPanel_SoftOrange:before, .dx-acc-r .dxGridView_CTShowSearchPanelDisabled_SoftOrange:before, .dx-acc-r .dxGridView_gvCellError_SoftOrange:before, .dx-acc-r .dxGridView_gvCMClearFilter_SoftOrange:before, .dx-acc-r .dxGridView_gvCMClearFilterDisabled_SoftOrange:before, .dx-acc-r .dxGridView_gvCMClearGrouping_SoftOrange:before, .dx-acc-r .dxGridView_gvCMClearGroupingDisabled_SoftOrange:before, .dx-acc-r .dxGridView_gvCMCollapseDetailRow_SoftOrange:before, .dx-acc-r .dxGridView_gvCMCollapseDetailRowDisabled_SoftOrange:before, .dx-acc-r .dxGridView_gvCMCollapseRow_SoftOrange:before, .dx-acc-r .dxGridView_gvCMCollapseRowDisabled_SoftOrange:before, .dx-acc-r .dxGridView_gvCMDeleteRow_SoftOrange:before, .dx-acc-r .dxGridView_gvCMDeleteRowDisabled_SoftOrange:before, .dx-acc-r .dxGridView_gvCMEditRow_SoftOrange:before, .dx-acc-r .dxGridView_gvCMEditRowDisabled_SoftOrange:before, .dx-acc-r .dxGridView_gvCMExpandDetailRow_SoftOrange:before, .dx-acc-r .dxGridView_gvCMExpandDetailRowDisabled_SoftOrange:before, .dx-acc-r .dxGridView_gvCMExpandRow_SoftOrange:before, .dx-acc-r .dxGridView_gvCMExpandRowDisabled_SoftOrange:before, .dx-acc-r .dxGridView_gvCMFullCollapse_SoftOrange:before, .dx-acc-r .dxGridView_gvCMFullCollapseDisabled_SoftOrange:before, .dx-acc-r .dxGridView_gvCMFullExpand_SoftOrange:before, .dx-acc-r .dxGridView_gvCMFullExpandDisabled_SoftOrange:before, .dx-acc-r .dxGridView_gvCMGroupByColumn_SoftOrange:before, .dx-acc-r .dxGridView_gvCMGroupByColumnDisabled_SoftOrange:before, .dx-acc-r .dxGridView_gvCMNewRow_SoftOrange:before, .dx-acc-r .dxGridView_gvCMNewRowDisabled_SoftOrange:before, .dx-acc-r .dxGridView_gvCMRefresh_SoftOrange:before, .dx-acc-r .dxGridView_gvCMRefreshDisabled_SoftOrange:before, .dx-acc-r .dxGridView_gvCMSearchPanel_SoftOrange:before, .dx-acc-r .dxGridView_gvCMSearchPanelDisabled_SoftOrange:before, .dx-acc-r .dxGridView_gvCMShowCustDialog_SoftOrange:before, .dx-acc-r .dxGridView_gvCMShowCustDialogDisabled_SoftOrange:before, .dx-acc-r .dxGridView_gvCMShowCustDialogHover_SoftOrange:before, .dx-acc-r .dxGridView_gvCMShowCustomizationWindow_SoftOrange:before, .dx-acc-r .dxGridView_gvCMShowCustomizationWindowDisabled_SoftOrange:before, .dx-acc-r .dxGridView_gvCMShowFilterEditor_SoftOrange:before, .dx-acc-r .dxGridView_gvCMShowFilterEditorDisabled_SoftOrange:before, .dx-acc-r .dxGridView_gvCMShowGroupPanel_SoftOrange:before, .dx-acc-r .dxGridView_gvCMShowGroupPanelDisabled_SoftOrange:before, .dx-acc-r .dxGridView_gvCMShowSearchPanel_SoftOrange:before, .dx-acc-r .dxGridView_gvCMShowSearchPanelDisabled_SoftOrange:before, .dx-acc-r .dxGridView_gvCMSortAscending_SoftOrange:before, .dx-acc-r .dxGridView_gvCMSortAscendingDisabled_SoftOrange:before, .dx-acc-r .dxGridView_gvCMSortDescending_SoftOrange:before, .dx-acc-r .dxGridView_gvCMSortDescendingDisabled_SoftOrange:before, .dx-acc-r .dxGridView_gvCMSummaryAverage_SoftOrange:before, .dx-acc-r .dxGridView_gvCMSummaryAverageDisabled_SoftOrange:before, .dx-acc-r .dxGridView_gvCMSummaryCount_SoftOrange:before, .dx-acc-r .dxGridView_gvCMSummaryCountDisabled_SoftOrange:before, .dx-acc-r .dxGridView_gvCMSummaryMax_SoftOrange:before, .dx-acc-r .dxGridView_gvCMSummaryMaxDisabled_SoftOrange:before, .dx-acc-r .dxGridView_gvCMSummaryMin_SoftOrange:before, .dx-acc-r .dxGridView_gvCMSummaryMinDisabled_SoftOrange:before, .dx-acc-r .dxGridView_gvCMSummarySum_SoftOrange:before, .dx-acc-r .dxGridView_gvCMSummarySumDisabled_SoftOrange:before, .dx-acc-r .dxGridView_gvCMUngroupColumn_SoftOrange:before, .dx-acc-r .dxGridView_gvCOApply_SoftOrange:before, .dx-acc-r .dxGridView_gvCOApplyDisabled_SoftOrange:before, .dx-acc-r .dxGridView_gvCOClearFilter_SoftOrange:before, .dx-acc-r .dxGridView_gvCOClearFilterDisabled_SoftOrange:before, .dx-acc-r .dxGridView_gvCOClose_SoftOrange:before, .dx-acc-r .dxGridView_gvCOColumnDrag_SoftOrange:before, .dx-acc-r .dxGridView_gvCOColumnDragDisabled_SoftOrange:before, .dx-acc-r .dxGridView_gvCOColumnGroup_SoftOrange:before, .dx-acc-r .dxGridView_gvCOColumnGroupDisabled_SoftOrange:before, .dx-acc-r .dxGridView_gvCOColumnHide_SoftOrange:before, .dx-acc-r .dxGridView_gvCOColumnHideDisabled_SoftOrange:before, .dx-acc-r .dxGridView_gvCOColumnRemove_SoftOrange:before, .dx-acc-r .dxGridView_gvCOColumnRemoveDisabled_SoftOrange:before, .dx-acc-r .dxGridView_gvCOColumnShow_SoftOrange:before, .dx-acc-r .dxGridView_gvCOColumnShowDisabled_SoftOrange:before, .dx-acc-r .dxGridView_gvCOColumnSort_SoftOrange:before, .dx-acc-r .dxGridView_gvCOColumnSortDisabled_SoftOrange:before, .dx-acc-r .dxGridView_gvCOColumnSortDown_SoftOrange:before, .dx-acc-r .dxGridView_gvCOColumnSortDownDisabled_SoftOrange:before, .dx-acc-r .dxGridView_gvCOColumnSortUp_SoftOrange:before, .dx-acc-r .dxGridView_gvCOColumnSortUpDisabled_SoftOrange:before, .dx-acc-r .dxGridView_gvCOColumnUngroup_SoftOrange:before, .dx-acc-r .dxGridView_gvCOColumnUngroupDisabled_SoftOrange:before, .dx-acc-r .dxGridView_gvCOColumnUnsort_SoftOrange:before, .dx-acc-r .dxGridView_gvCOColumnUnsortDisabled_SoftOrange:before, .dx-acc-r .dxGridView_gvCODragAreaCollapse_SoftOrange:before, .dx-acc-r .dxGridView_gvCODragAreaExpand_SoftOrange:before, .dx-acc-r .dxGridView_gvCOFilterCollapse_SoftOrange:before, .dx-acc-r .dxGridView_gvCOFilterExpand_SoftOrange:before, .dx-acc-r .dxGridView_gvCollapsedButton_SoftOrange:before, .dx-acc-r .dxGridView_gvCollapsedButtonRtl_SoftOrange:before, .dx-acc-r .dxGridView_gvDetailCollapsedButton_SoftOrange:before, .dx-acc-r .dxGridView_gvDetailCollapsedButtonRtl_SoftOrange:before, .dx-acc-r .dxGridView_gvDetailExpandedButton_SoftOrange:before, .dx-acc-r .dxGridView_gvDetailExpandedButtonRtl_SoftOrange:before, .dx-acc-r .dxGridView_gvDragAndDropArrowDown_SoftOrange:before, .dx-acc-r .dxGridView_gvDragAndDropArrowLeft_SoftOrange:before, .dx-acc-r .dxGridView_gvDragAndDropArrowRight_SoftOrange:before, .dx-acc-r .dxGridView_gvDragAndDropArrowUp_SoftOrange:before, .dx-acc-r .dxGridView_gvDragAndDropHideColumn_SoftOrange:before, .dx-acc-r .dxGridView_gvExpandedButton_SoftOrange:before, .dx-acc-r .dxGridView_gvExpandedButtonRtl_SoftOrange:before, .dx-acc-r .dxGridView_gvFilterRowButton_SoftOrange:before, .dx-acc-r .dxGridView_gvFixedGroupRow_SoftOrange:before, .dx-acc-r .dxGridView_gvHeaderFilter_SoftOrange:before, .dx-acc-r .dxGridView_gvHeaderFilterActive_SoftOrange:before, .dx-acc-r .dxGridView_gvHeaderSortDown_SoftOrange:before, .dx-acc-r .dxGridView_gvHeaderSortUp_SoftOrange:before, .dx-acc-r .dxGridView_gvHideAdaptiveDetailButton_SoftOrange:before, .dx-acc-r .dxGridView_gvParentGroupRows_SoftOrange:before, .dx-acc-r .dxGridView_gvShowAdaptiveDetailButton_SoftOrange:before, .dx-acc-r .dxGridView_WindowResizer_SoftOrange:before, .dx-acc-r .dxGridView_WindowResizerRtl_SoftOrange:before, .dx-acc-r .dxgvFocusedRow_SoftOrange .dxGridView_gvShowAdaptiveDetailButton_SoftOrange:before {
            content: url('filtro_verde.png');
            background-repeat: no-repeat;
            background-color: transparent;
            background-position: 0px 0px;
        }*/

        
        /*Fuente pequeña*/
        .txt-sm{
            font-size:11px;
        }

        /*Botón de flecha regresar*/
        .flecha_regresar {
            float: right;
            font-size: 19px;
            font-weight: normal;
            line-height: 1;
            color: #FFF;
            text-shadow: 0 1px 0 #FFF;
            filter: alpha(opacity=10);
            opacity: .9;
        }

            .flecha_regresar:hover,
            .flecha_regresar:focus {
                color: #FFF;
                text-decoration: none;
                cursor: pointer;
                text-decoration-color: #fff;
                filter: alpha(opacity=50);
                opacity: .5;
            }

        button.flecha_regresar {
            -webkit-appearance: none;
            padding: 0;
            cursor: pointer;
            background: transparent;
            border: 0;
        }

    </style>
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

        //funcion para cada registro del grid 
        function rbConsultarClick(s, e, index) {
            var cheked = s.GetChecked();

            var rbConsultar = eval("rbConsultar" + index);
            var valor = rbConsultar.GetChecked();

            for (var i = 0; i < grid.GetVisibleRowsOnPage() ; i++) {
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

        //funcion para cada registro del grid 
        function rbConsultar2Click(s, e, index) {
            var cheked = s.GetChecked();

            var rbConsultar2 = eval("rbConsultar2" + index);
            var valor = rbConsultar2.GetChecked();

            for (var i = 0; i < grid2.GetVisibleRowsOnPage() ; i++) {
                if (i == index) {
                    rbConsultar2 = eval("rbConsultar2" + i);
                    rbConsultar2.SetChecked(valor);
                }
                else {
                    try {
                        rbConsultar2 = eval("rbConsultar2" + i);
                        rbConsultar2.SetChecked(false);
                    }
                    catch (err) { }
                }
            }
        }

        function CargaLoading() {
            Callback.PerformCallback();
            LoadingPanel1.Show();
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
             
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentSection" runat="server">
           
    <div class="container-fluid">
         <div class="panel panel-primary">
             <div class="panel-heading">
                 <table style="width: 100%">
                    <tr>
                        <td style="text-align: left; width: 50%">
                            <h1 id="h1_titulo" runat="server" class="panel-title small"></h1>
                        </td>
                        <td style="text-align: right; width: 50%">
                            <asp:LinkButton ID="lkb_Regresar" runat="server" OnClick="lkb_Regresar_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button">
                                 <span class="glyphicon glyphicon-circle-arrow-left"></span>&nbsp;&nbsp;Regresar
                            </asp:LinkButton>
                        </td>
                    </tr>
                </table>
                 
             </div>
             <div class="panel-body" >
                 <asp:Panel ID="Panel1" runat="server">                                                       
                    <dx:ASPxGridView ID="Grid" runat="server" EnableTheming="True" Theme="SoftOrange"
                       OnCustomCallback="Grid_CustomCallback" EnableCallBacks="false" ClientInstanceName="grid"
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
                           <dx:GridViewDataColumn Caption="" VisibleIndex="0" Width="40px" Settings-AllowAutoFilter="False">
                               <HeaderStyle HorizontalAlign="Center" />
                               <CellStyle HorizontalAlign="Center"></CellStyle>
                               <DataItemTemplate>
                                   <dx:ASPxRadioButton ID="rbConsultar" ClientInstanceName="rbConsultar" Enabled="true" runat="server"
                                       Theme="SoftOrange" GroupName="consultar" OnInit="rbConsultar_Init" AutoPostBack="true" OnCheckedChanged="rbConsultar_CheckedChanged">
                                   </dx:ASPxRadioButton>
                               </DataItemTemplate>
                           </dx:GridViewDataColumn>
                           <dx:GridViewDataTextColumn Caption="Secuencia" FieldName="PARTIDA" ReadOnly="True" VisibleIndex="1" Width="120px" >
                               <HeaderStyle HorizontalAlign="Center" ForeColor="#FFF"  />
                               <SettingsHeaderFilter Mode="CheckedList" />
                               <CellStyle HorizontalAlign="Center" />
                               <FooterCellStyle HorizontalAlign="Center" />
                           </dx:GridViewDataTextColumn>
                           <dx:GridViewDataTextColumn Caption="Fracción Arancelaría" FieldName="PARTIDA_DETALLE_FRACCIONARANCELARIA" ReadOnly="True" VisibleIndex="2" Width="170px">
                               <HeaderStyle HorizontalAlign="Center" />
                               <SettingsHeaderFilter Mode="CheckedList" />
                               <CellStyle HorizontalAlign="Center" />
                           </dx:GridViewDataTextColumn>
                           <dx:GridViewDataTextColumn Caption="Cantidad Comercial" FieldName="PARTIDA_DETALLE_VALORCOMERCIAL" ReadOnly="True" VisibleIndex="4" Width="170px">
                               <HeaderStyle HorizontalAlign="Center" />
                               <SettingsHeaderFilter Mode="CheckedList" />
                               <CellStyle HorizontalAlign="Right" />
                               <PropertiesTextEdit DisplayFormatString="{0:n2}" />
                               <FooterCellStyle HorizontalAlign="Right" />
                           </dx:GridViewDataTextColumn>
                           <dx:GridViewDataTextColumn Caption="UMC" FieldName="PARTIDA_DETALLE_UNIDADMEDIDACOMERCIA_CLAVE" ReadOnly="True" VisibleIndex="5" Width="150px">
                               <HeaderStyle HorizontalAlign="Center" />
                               <SettingsHeaderFilter Mode="CheckedList" />
                               <CellStyle HorizontalAlign="Center" />
                           </dx:GridViewDataTextColumn>
                           <dx:GridViewDataTextColumn Caption="Descripción" FieldName="PARTIDA_DETALLE_DESCRIPCIONMERCANCIA" ReadOnly="True" VisibleIndex="6" Width="465px">
                               <HeaderStyle HorizontalAlign="Center" />
                               <SettingsHeaderFilter Mode="CheckedList" />
                               <CellStyle HorizontalAlign="Left" />
                           </dx:GridViewDataTextColumn>
                           <dx:GridViewDataTextColumn Caption="Valor Dólares" FieldName="PARTIDA_DETALLE_VALORDOLARES" ReadOnly="True" VisibleIndex="8" Width="145px">
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
                                       <table style="width:100%;">
                                           <tr>
                                               <td style="text-align:left; ">
                                                   &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                   <dx:ASPxLabel ID="Label1" runat="server" Text="Pedimento:" Font-Size="12px"/>&nbsp;
                                                   <dx:ASPxLabel ID="lblPedimento" runat="server" Text="" Font-Size="12px"/>
                                                   &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                   <dx:BootstrapButton ID="lkb_LimpiarFiltros" runat="server" Text="Limpiar Filtros" OnClick="lkb_LimpiarFiltros_Click" 
                                                       SettingsBootstrap-RenderOption="Primary" AutoPostBack="false" SettingsBootstrap-Sizing="Small" CssClasses-Text="txt-sm" >
                                                       <ClientSideEvents Click="function(s, e) { Callback.PerformCallback(); LoadingPanel1.Show(); }" 
                                                                         Init="function(s, e) {LoadingPanel1.Hide();}" />
                                                       <CssClasses Icon="glyphicon glyphicon-erase" /> 
                                                   </dx:BootstrapButton>
                                               </td>
                                               <td style="text-align:right; padding-right:5px ">
                                                   <dx:BootstrapButton ID="bbValidar" runat="server" AutoPostBack="false" OnClick="bbValidar_Click"
                                                       SettingsBootstrap-RenderOption="Primary" Text="Validar" CssClasses-Text="txt-sm">
                                                       <SettingsBootstrap RenderOption="Primary" Sizing="Small" />
                                                       <ClientSideEvents Click="function(s, e) { Callback.PerformCallback(); LoadingPanel1.Show(); }"
                                                           Init="function(s, e) {LoadingPanel1.Hide();}" />
                                                   </dx:BootstrapButton>
                                                   &nbsp;&nbsp;
                                                   <dx:BootstrapButton ID="bbMigrar" runat="server" AutoPostBack="false" 
                                                       SettingsBootstrap-RenderOption="Primary" Text="Migrar A24" CssClasses-Text="txt-sm">
                                                       <SettingsBootstrap RenderOption="Primary" Sizing="Small" />
                                                       <ClientSideEvents Click="function(s, e) { Callback.PerformCallback(); LoadingPanel1.Show(); }" 
                                                           Init="function(s, e) {LoadingPanel1.Hide();}" />
                                                   </dx:BootstrapButton>
                                                   &nbsp;&nbsp;
                                               </td>
                                           </tr>
                                       </table>
                                       <%--<dx:BootstrapButton ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_OnClick" 
                                           SettingsBootstrap-RenderOption="Primary" AutoPostBack="false" SettingsBootstrap-Sizing="Small" >
                                           <ClientSideEvents Click="function(s, e) { Callback.PerformCallback(); LoadingPanel1.Show(); }" 
                                                             Init="function(s, e) {LoadingPanel1.Hide();}" />
                                           <CssClasses Icon="glyphicon glyphicon-search" /> 
                                       </dx:BootstrapButton>
                                       <asp:LinkButton ID="lkb_Excel" runat="server" OnClick="lkb_Excel_Click" CssClass="btn btn-primary btn-sm text-right txt-sm" 
                                           ToolTip="Exportar a Excel" data-toggle="tooltip">
                                           <span class="glyphicon glyphicon-list"></span>&nbsp;&nbsp;Excel
                                       </asp:LinkButton>
                                       <dx:BootstrapButton ID="lkb_Actualizar" runat="server" Text="Actualizar" OnClick="lkb_Actualizar_Click" ToolTip="Actualizar"
                                           SettingsBootstrap-RenderOption="Primary" AutoPostBack="false" SettingsBootstrap-Sizing="Small" data-toggle="tooltip" CssClasses-Text="txt-sm">
                                           <ClientSideEvents Click="function(s, e) { Callback.PerformCallback(); LoadingPanel1.Show(); }" 
                                                             Init="function(s, e) {LoadingPanel1.Hide();}" />
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
               
                 <dx:ASPxPageControl runat="server" ID="ASPxPageControl1" Height="60px" Width="100%" EnableCallBacks="true"  ContentStyle-Border-BorderWidth="3px"
                            TabAlign="Justify" ActiveTabIndex="0" EnableTabScrolling="true" Theme="SoftOrange" Font-Size="12px" ContentStyle-VerticalAlign="Top">
                             <TabStyle Paddings-PaddingLeft="10px" Paddings-PaddingRight="10px"  />                                                                                      
                             <ContentStyle>
                                 <Paddings PaddingLeft="20px" PaddingRight="20px" PaddingTop="5px" />
                             </ContentStyle>
                             <TabPages>                                                                            
                                <dx:TabPage Text="Número de Parte">
                                    <ContentCollection>
                                        <dx:ContentControl ID="ContentControl1" runat="server">
                                        
                                            <dx:ASPxGridView ID="Grid2" runat="server" EnableTheming="True" Theme="SoftOrange" AutoGenerateColumns="False" SettingsPager-Mode="ShowAllRecords" 
                                                OnCustomCallback="Grid2_CustomCallback" EnableCallBacks="false" ClientInstanceName="grid2" Width="100%"
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
                                                    <dx:ASPxSummaryItem FieldName="CANTIDAD" SummaryType="Sum" DisplayFormat=" {0:n4}" />
                                                    <dx:ASPxSummaryItem FieldName="PRECIO_TOTAL" SummaryType="Sum" DisplayFormat=" {0:n4}" />
                                                </TotalSummary>
                                                <Columns>
                                                    <dx:GridViewDataTextColumn FieldName="DETALLENPKEY" ReadOnly="True" VisibleIndex="0" Visible="false">
                                                    </dx:GridViewDataTextColumn>
                                                    <%--<dx:GridViewDataColumn Caption=" " VisibleIndex="0" Width="40px" Settings-AllowAutoFilter="False">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <CellStyle HorizontalAlign="Center"></CellStyle>
                                                        <DataItemTemplate>
                                                            <dx:ASPxRadioButton ID="rbConsultar2" ClientInstanceName="rbConsultar2" Enabled="true" runat="server"
                                                                Theme="SoftOrange" GroupName="consultar" OnInit="rbConsultar2_Init" AutoPostBack="true" OnCheckedChanged="rbConsultar2_CheckedChanged">
                                                            </dx:ASPxRadioButton>
                                                        </DataItemTemplate>
                                                    </dx:GridViewDataColumn>--%>
                                                    <dx:GridViewDataTextColumn Caption="Secuencia" FieldName="SECUENCIA_PEDIMENTO" ReadOnly="True" VisibleIndex="1" Width="110px" >
                                                        <HeaderStyle HorizontalAlign="Center" ForeColor="#FFF"  />
                                                        <SettingsHeaderFilter Mode="CheckedList" />
                                                        <CellStyle HorizontalAlign="Center" />
                                                        <FooterCellStyle HorizontalAlign="Center" />
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="Cove" FieldName="COVE" ReadOnly="True" VisibleIndex="2" Width="150px">
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
                                                </Columns>
                                                <Toolbars>
                                                    <dx:GridViewToolbar Name="Toolbar1" EnableAdaptivity="true" ItemAlign="Left">
                                                        <Items>
                                                            <dx:GridViewToolbarItem Name="Links" ItemStyle-Width="100%">
                                                            <Template>
                                                                <dx:BootstrapButton ID="bbAgregar" runat="server" AutoPostBack="false"  Width="60px" OnClick="bbAgregar_OnClick"
                                                                    SettingsBootstrap-RenderOption="Primary" Text="Agregar" CssClasses-Text="txt-sm">
                                                                    <SettingsBootstrap RenderOption="Primary" Sizing="Small" />
                                                                    <ClientSideEvents Click="function(s, e) { Callback.PerformCallback(); LoadingPanel1.Show(); }" 
                                                                        Init="function(s, e) {LoadingPanel1.Hide();}" />
                                                                </dx:BootstrapButton>
                                                                <dx:BootstrapButton ID="bbEditar" runat="server" AutoPostBack="false"  Width="70px" OnClick="bbEditar_OnClick"
                                                                     SettingsBootstrap-RenderOption="Primary" Text="Editar" CssClasses-Text="txt-sm">
                                                                     <SettingsBootstrap RenderOption="Primary" Sizing="Small" />
                                                                     <ClientSideEvents Click="function(s, e) { Callback.PerformCallback(); LoadingPanel1.Show(); }" 
                                                                         Init="function(s, e) {LoadingPanel1.Hide();}" />
                                                                 </dx:BootstrapButton>
                                                                <dx:BootstrapButton ID="bbBorrar" runat="server" AutoPostBack="false"  Width="80px" OnClick="bbBorrar_OnClick"
                                                                    SettingsBootstrap-RenderOption="Primary" Text="Borrar" CssClasses-Text="txt-sm">
                                                                    <SettingsBootstrap RenderOption="Primary" Sizing="Small" />
                                                                    <ClientSideEvents Click="function(s, e) { Callback.PerformCallback(); LoadingPanel1.Show(); }" 
                                                                        Init="function(s, e) {LoadingPanel1.Hide();}" />
                                                                </dx:BootstrapButton>

                                                                <%--<dx:BootstrapButton ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_OnClick" 
                                                                    SettingsBootstrap-RenderOption="Primary" AutoPostBack="false" SettingsBootstrap-Sizing="Small" >
                                                                    <ClientSideEvents Click="function(s, e) { Callback.PerformCallback(); LoadingPanel1.Show(); }" 
                                                                                      Init="function(s, e) {LoadingPanel1.Hide();}" />
                                                                    <CssClasses Icon="glyphicon glyphicon-search" /> 
                                                                </dx:BootstrapButton>
                                                                <asp:LinkButton ID="lkb_Excel" runat="server" OnClick="lkb_Excel_Click" CssClass="btn btn-primary btn-sm text-right txt-sm" 
                                                                    ToolTip="Exportar a Excel" data-toggle="tooltip">
                                                                    <span class="glyphicon glyphicon-list"></span>&nbsp;&nbsp;Excel
                                                                </asp:LinkButton>
                                                                <dx:BootstrapButton ID="lkb_Actualizar" runat="server" Text="Actualizar" OnClick="lkb_Actualizar_Click" ToolTip="Actualizar"
                                                                    SettingsBootstrap-RenderOption="Primary" AutoPostBack="false" SettingsBootstrap-Sizing="Small" data-toggle="tooltip" CssClasses-Text="txt-sm">
                                                                    <ClientSideEvents Click="function(s, e) { Callback.PerformCallback(); LoadingPanel1.Show(); }" 
                                                                                      Init="function(s, e) {LoadingPanel1.Hide();}" />
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
                                                <%--<Templates>
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
                                                </Templates>     --%>                 
                                             </dx:ASPxGridView>
                                            <dx:ASPxGridViewExporter ID="ASPxGridViewExporter1" GridViewID="Grid" runat="server" PaperKind="A5" Landscape="true" />

                                        </dx:ContentControl>    
                                    </ContentCollection>
                                </dx:TabPage>
                                <dx:TabPage Text="Errores">
                                    <ContentCollection>    
                                        <dx:ContentControl ID="ContentControl2" runat="server">
                                            
                                            <dx:ASPxGridView ID="Grid3" runat="server" EnableTheming="True" Theme="SoftOrange" KeyFieldName="ERRORKEY"
                                                EnableCallBacks="false" ClientInstanceName="grid3" AutoGenerateColumns="False" Width="100%" 
                                                Settings-HorizontalScrollBarMode="Auto" SettingsPager-Position="TopAndBottom"
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
                                                    <dx:GridViewDataTextColumn Caption="Descripción" FieldName="DESCRIPCION" ReadOnly="True" VisibleIndex="3" Width="530px" >
                                                        <HeaderStyle HorizontalAlign="Center" ForeColor="#FFF"  />
                                                        <SettingsHeaderFilter Mode="CheckedList" />
                                                        <CellStyle HorizontalAlign="Center" />
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataDateColumn Caption="Origen SVU" FieldName="ORIGENDSVU" ReadOnly="True" VisibleIndex="4" Width="170px">
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
    
    <button id="btnModal" type="button" data-toggle="modal" data-target="#Modal" data-whatever="Nuevo" onclick="return false;" style="display: none;"></button>
    <div class="modal fade" id="Modal" tabindex="-1" role="dialog" aria-labelledby="ModalTitulo" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-lg" role="document" >
            <div class="modal-content">
                <div class="modal-header">
                    <h4 id="ModalTitulo" class="modal-title" runat="server"></h4>                    
                </div>
                <asp:Panel ID="Panel2" runat="server">
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
                                        <ClientSideEvents LostFocus = "function(s,e) { ValidaValor(s); }" />                                      
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
                                <div class="form-group col-md-4">
                                        &nbsp;<label id="Label7" runat="server" style="font-size:11px">Descripción</label>                                                                                     
                                        <asp:TextBox ID="txt_Descripcion" runat="server" CssClass="form-control input-sm" MaxLength="250" placeholder="Descripción" Width="100%"></asp:TextBox>
                                </div>
                                <div class="form-group col-md-4">
                                        &nbsp;<label id="Label8" runat="server" style="font-size:11px">*Cve. Cliente/Proveedor</label>                                                                                     
                                        <asp:TextBox ID="txt_ClienteProv" runat="server" CssClass="form-control input-sm" MaxLength="50" placeholder="*Cve. Cliente/Proveedor" Width="100%"></asp:TextBox>
                                </div>
                                <div class="form-group col-md-4">
                                        &nbsp;<label id="Label9" runat="server" style="font-size:11px">PO</label>                                                                                     
                                        <asp:TextBox ID="txt_PO" runat="server" CssClass="form-control input-sm" MaxLength="250" placeholder="PO" Width="100%"></asp:TextBox>
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
                            <asp:LinkButton ID="btnGuardar" runat="server" CssClass="btn btn-success btn-sm" Text="Guardar" OnClick="btnGuardar_Click">
                                <span class="glyphicon glyphicon-ok"></span>&nbsp;&nbsp;Guardar</asp:LinkButton>
                            <button type="button" class="btn btn-danger btn-sm" data-dismiss="modal">
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
