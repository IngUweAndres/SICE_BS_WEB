<%@ Page Language="C#" MasterPageFile="~/Presentacion/Principal.Master" AutoEventWireup="true" CodeBehind="IA_ManifestacionValor.aspx.cs" Inherits="SICE_BS_WEB.Presentacion.IA_ManifestacionValor" %>

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

        /*******************************************************************************/
        /*funciones para para validar los checkboxes en Agente o Apoderado Aduanal */
        function Get_chkb1S(s, e) {
            var cheked = s.GetChecked();

            if (cheked){
                chkb1N.SetChecked(false);
            }            
        }
        function Get_chkb1N(s, e) {
            var cheked = s.GetChecked();

            if (cheked) {
                chkb1S.SetChecked(false);
            }
        }


        function Get_chkb2S(s, e) {
            var cheked = s.GetChecked();

            if (cheked) {
                chkb2N.SetChecked(false);
            }
        }
        function Get_chkb2N(s, e) {
            var cheked = s.GetChecked();

            if (cheked) {
                chkb2S.SetChecked(false);
            }
        }
        /********************************************************************************************/

        /********************************************************************************************/
        /*funciones para para validar los checkboxes al dar clic en botón editar del grid principal */

        function Get_chkfS() {

            /*Método de Valoración*/
            var chk1 = chkf1S.GetChecked();
            var chk2 = chkf2S.GetChecked();

            if (chk1) {

                chkf2S.SetChecked(false);
            }
            else if (chk2) {

                chkf1S.SetChecked(false);
            }

            var chk3 = chkf3S.GetChecked();
            if (chk3) {

                txt_f_descripciondetransaccionmercancias.SetEnabled(true);
            }
            else {

                txt_f_descripciondetransaccionmercancias.SetEnabled(false);
            }

            var chk4 = chkf4S.GetChecked();
            if (chk4) {

                txt_f_descripciondetransaccionmercanciasidenticas.SetEnabled(true);
            }
            else {

                txt_f_descripciondetransaccionmercanciasidenticas.SetEnabled(false);
            }

            var chk5 = chkf5S.GetChecked();
            if (chk5) {

                txt_f_descripciondetransaccionmercanciassimilares.SetEnabled(true);
            }
            else {

                txt_f_descripciondetransaccionmercanciassimilares.SetEnabled(false);
            }

            var chk6 = chkf6S.GetChecked();
            if (chk6) {

                txt_f_descripcionvalordepreciounitariodeventa.SetEnabled(true);
            }
            else {

                txt_f_descripcionvalordepreciounitariodeventa.SetEnabled(false);
            }

            var chk7 = chkf7S.GetChecked();
            if (chk7) {

                txt_f_descripcionvalorreconstruido.SetEnabled(true);
            }
            else {

                txt_f_descripcionvalorreconstruido.SetEnabled(false);
            }

            var chk8 = chkf8S.GetChecked();
            if (chk8) {

                txt_f_descripcionvalorconformaley.SetEnabled(true);
            }
            else {

                txt_f_descripcionvalorconformaley.SetEnabled(false);
            }


            /*Anexo*/
            var chkg = chkg1S.GetChecked();

            if (chkg) {
                chkg1S.SetChecked(true);
                txt_g_anexadocumentacionnumeroyletra.SetEnabled(true);
            }
            else {
                chkg1S.SetChecked(false);
                txt_g_anexadocumentacionnumeroyletra.SetEnabled(false);
            }
        }

        /********************************************************************************************/


        /********************************************************************************************/
        /*funcion para para validar los checkboxes en Método de Valoración */
        function Get_chkf1S(s, e) {
            var cheked = s.GetChecked();
            var chk2 = chkf2S.GetChecked();

            if (cheked) {

                chkf3S.SetEnabled(true);
                chkf4S.SetEnabled(true);
                chkf5S.SetEnabled(true);
                chkf6S.SetEnabled(true);
                chkf7S.SetEnabled(true);
                chkf8S.SetEnabled(true);
            }
            else {
                if (!chk2) {

                    chkf3S.SetEnabled(false);
                    chkf4S.SetEnabled(false);
                    chkf5S.SetEnabled(false);
                    chkf6S.SetEnabled(false);
                    chkf7S.SetEnabled(false);
                    chkf8S.SetEnabled(false);                   
                }
            }

            txt_f_descripciondetransaccionmercancias.SetEnabled(false);
            txt_f_descripciondetransaccionmercanciasidenticas.SetEnabled(false);
            txt_f_descripciondetransaccionmercanciassimilares.SetEnabled(false);
            txt_f_descripcionvalordepreciounitariodeventa.SetEnabled(false);
            txt_f_descripcionvalorreconstruido.SetEnabled(false);
            txt_f_descripcionvalorconformaley.SetEnabled(false);

            chkf2S.SetChecked(false);
            chkf3S.SetChecked(false);
            chkf4S.SetChecked(false);
            chkf5S.SetChecked(false);
            chkf6S.SetChecked(false);
            chkf7S.SetChecked(false);
            chkf8S.SetChecked(false);

            txt_f_descripciondetransaccionmercancias.SetText('');
            txt_f_descripciondetransaccionmercanciasidenticas.SetText('');
            txt_f_descripciondetransaccionmercanciassimilares.SetText('');
            txt_f_descripcionvalordepreciounitariodeventa.SetText('');
            txt_f_descripcionvalorreconstruido.SetText('');
            txt_f_descripcionvalorconformaley.SetText('');
        }

        function Get_chkf2S(s, e) {
            var cheked = s.GetChecked();
            var chk1 = chkf1S.GetChecked();

            if (cheked) {

                chkf3S.SetEnabled(true);
                chkf4S.SetEnabled(true);
                chkf5S.SetEnabled(true);
                chkf6S.SetEnabled(true);
                chkf7S.SetEnabled(true);
                chkf8S.SetEnabled(true);
            }
            else {
                if (!chk1) {

                    chkf3S.SetEnabled(false);
                    chkf4S.SetEnabled(false);
                    chkf5S.SetEnabled(false);
                    chkf6S.SetEnabled(false);
                    chkf7S.SetEnabled(false);
                    chkf8S.SetEnabled(false);
                }
            }

            txt_f_descripciondetransaccionmercancias.SetEnabled(false);
            txt_f_descripciondetransaccionmercanciasidenticas.SetEnabled(false);
            txt_f_descripciondetransaccionmercanciassimilares.SetEnabled(false);
            txt_f_descripcionvalordepreciounitariodeventa.SetEnabled(false);
            txt_f_descripcionvalorreconstruido.SetEnabled(false);
            txt_f_descripcionvalorconformaley.SetEnabled(false);

            chkf1S.SetChecked(false);
            chkf3S.SetChecked(false);
            chkf4S.SetChecked(false);
            chkf5S.SetChecked(false);
            chkf6S.SetChecked(false);
            chkf7S.SetChecked(false);
            chkf8S.SetChecked(false);

            txt_f_descripciondetransaccionmercancias.SetText('');
            txt_f_descripciondetransaccionmercanciasidenticas.SetText('');
            txt_f_descripciondetransaccionmercanciassimilares.SetText('');
            txt_f_descripcionvalordepreciounitariodeventa.SetText('');
            txt_f_descripcionvalorreconstruido.SetText('');
            txt_f_descripcionvalorconformaley.SetText('');
        }


        function Get_chkf3S(s, e) {
            var cheked = s.GetChecked();
            var chk = chkf1S.GetChecked();

            if (chk){

                if (cheked) {
                    chkf3S.SetChecked(true);
                txt_f_descripciondetransaccionmercancias.SetEnabled(true);
                
                }
                else if (!cheked) {
                    chkf3S.SetChecked(false);
                    txt_f_descripciondetransaccionmercancias.SetEnabled(false);
                }

                chkf4S.SetChecked(false);
                chkf5S.SetChecked(false);
                chkf6S.SetChecked(false);
                chkf7S.SetChecked(false);
                chkf8S.SetChecked(false);

                txt_f_descripciondetransaccionmercanciasidenticas.SetEnabled(false);
                txt_f_descripciondetransaccionmercanciassimilares.SetEnabled(false);
                txt_f_descripcionvalordepreciounitariodeventa.SetEnabled(false);
                txt_f_descripcionvalorreconstruido.SetEnabled(false);
                txt_f_descripcionvalorconformaley.SetEnabled(false);

                txt_f_descripciondetransaccionmercancias.SetText('');
                txt_f_descripciondetransaccionmercanciasidenticas.SetText('');
                txt_f_descripciondetransaccionmercanciassimilares.SetText('');
                txt_f_descripcionvalordepreciounitariodeventa.SetText('');
                txt_f_descripcionvalorreconstruido.SetText('');
                txt_f_descripcionvalorconformaley.SetText('');
            }
            else {

                if (cheked) {
                    chkf3S.SetChecked(true);
                    txt_f_descripciondetransaccionmercancias.SetEnabled(true);

                }
                else if (!cheked) {
                    chkf3S.SetChecked(false);
                    txt_f_descripciondetransaccionmercancias.SetEnabled(false);
                }

                txt_f_descripciondetransaccionmercancias.SetText('');
            }         
        }

        function Get_chkf4S(s, e) {
            var cheked = s.GetChecked();
            var chk = chkf1S.GetChecked();

            if (chk) {

                if (cheked) {
                    chkf4S.SetChecked(true);
                    txt_f_descripciondetransaccionmercanciasidenticas.SetEnabled(true);

                }
                else if (!cheked) {
                    chkf4S.SetChecked(false);
                    txt_f_descripciondetransaccionmercanciasidenticas.SetEnabled(false);
                }

                chkf3S.SetChecked(false);
                chkf5S.SetChecked(false);
                chkf6S.SetChecked(false);
                chkf7S.SetChecked(false);
                chkf8S.SetChecked(false);

                txt_f_descripciondetransaccionmercancias.SetEnabled(false);
                txt_f_descripciondetransaccionmercanciassimilares.SetEnabled(false);
                txt_f_descripcionvalordepreciounitariodeventa.SetEnabled(false);
                txt_f_descripcionvalorreconstruido.SetEnabled(false);
                txt_f_descripcionvalorconformaley.SetEnabled(false);

                txt_f_descripciondetransaccionmercancias.SetText('');
                txt_f_descripciondetransaccionmercanciasidenticas.SetText('');
                txt_f_descripciondetransaccionmercanciassimilares.SetText('');
                txt_f_descripcionvalordepreciounitariodeventa.SetText('');
                txt_f_descripcionvalorreconstruido.SetText('');
                txt_f_descripcionvalorconformaley.SetText('');
            }
            else {

                if (cheked) {
                    chkf4S.SetChecked(true);
                    txt_f_descripciondetransaccionmercanciasidenticas.SetEnabled(true);

                }
                else if (!cheked) {
                    chkf4S.SetChecked(false);
                    txt_f_descripciondetransaccionmercanciasidenticas.SetEnabled(false);
                }

                txt_f_descripciondetransaccionmercanciasidenticas.SetText('');
            }           
        }

        function Get_chkf5S(s, e) {
            var cheked = s.GetChecked();
            var chk = chkf1S.GetChecked();

            if (chk) {

                if (cheked) {
                    chkf5S.SetChecked(true);
                    txt_f_descripciondetransaccionmercanciassimilares.SetEnabled(true);

                }
                else if (!cheked) {
                    chkf5S.SetChecked(false);
                    txt_f_descripciondetransaccionmercanciassimilares.SetEnabled(false);
                }

                chkf3S.SetChecked(false);
                chkf4S.SetChecked(false);
                chkf6S.SetChecked(false);
                chkf7S.SetChecked(false);
                chkf8S.SetChecked(false);

                txt_f_descripciondetransaccionmercancias.SetEnabled(false);
                txt_f_descripciondetransaccionmercanciasidenticas.SetEnabled(false);
                txt_f_descripcionvalordepreciounitariodeventa.SetEnabled(false);
                txt_f_descripcionvalorreconstruido.SetEnabled(false);
                txt_f_descripcionvalorconformaley.SetEnabled(false);

                txt_f_descripciondetransaccionmercancias.SetText('');
                txt_f_descripciondetransaccionmercanciasidenticas.SetText('');
                txt_f_descripciondetransaccionmercanciassimilares.SetText('');
                txt_f_descripcionvalordepreciounitariodeventa.SetText('');
                txt_f_descripcionvalorreconstruido.SetText('');
                txt_f_descripcionvalorconformaley.SetText('');
            }
            else {

                if (cheked) {
                    chkf5S.SetChecked(true);
                    txt_f_descripciondetransaccionmercanciassimilares.SetEnabled(true);

                }
                else if (!cheked) {
                    chkf5S.SetChecked(false);
                    txt_f_descripciondetransaccionmercanciassimilares.SetEnabled(false);
                }

                txt_f_descripciondetransaccionmercanciassimilares.SetText('');
            }
        }

        function Get_chkf6S(s, e) {
            var cheked = s.GetChecked();
            var chk = chkf1S.GetChecked();

            if (chk) {

                if (cheked) {
                    chkf6S.SetChecked(true);
                    txt_f_descripcionvalordepreciounitariodeventa.SetEnabled(true);

                }
                else if (!cheked) {
                    chkf6S.SetChecked(false);
                    txt_f_descripcionvalordepreciounitariodeventa.SetEnabled(false);
                }

                chkf3S.SetChecked(false);
                chkf4S.SetChecked(false);
                chkf5S.SetChecked(false);
                chkf7S.SetChecked(false);
                chkf8S.SetChecked(false);

                txt_f_descripciondetransaccionmercancias.SetEnabled(false);
                txt_f_descripciondetransaccionmercanciasidenticas.SetEnabled(false);
                txt_f_descripciondetransaccionmercanciassimilares.SetEnabled(false);
                txt_f_descripcionvalorreconstruido.SetEnabled(false);
                txt_f_descripcionvalorconformaley.SetEnabled(false);

                txt_f_descripciondetransaccionmercancias.SetText('');
                txt_f_descripciondetransaccionmercanciasidenticas.SetText('');
                txt_f_descripciondetransaccionmercanciassimilares.SetText('');
                txt_f_descripcionvalordepreciounitariodeventa.SetText('');
                txt_f_descripcionvalorreconstruido.SetText('');
                txt_f_descripcionvalorconformaley.SetText('');
            }
            else {

                if (cheked) {
                    chkf6S.SetChecked(true);
                    txt_f_descripcionvalordepreciounitariodeventa.SetEnabled(true);

                }
                else if (!cheked) {
                    chkf6S.SetChecked(false);
                    txt_f_descripcionvalordepreciounitariodeventa.SetEnabled(false);
                }

                txt_f_descripcionvalordepreciounitariodeventa.SetText('');
            }
        }

        function Get_chkf7S(s, e) {
            var cheked = s.GetChecked();
            var chk = chkf1S.GetChecked();

            if (chk) {

                if (cheked) {
                    chkf7S.SetChecked(true);
                    txt_f_descripcionvalorreconstruido.SetEnabled(true);

                }
                else if (!cheked) {
                    chkf7S.SetChecked(false);
                    txt_f_descripcionvalorreconstruido.SetEnabled(false);
                }

                chkf3S.SetChecked(false);
                chkf4S.SetChecked(false);
                chkf5S.SetChecked(false);
                chkf6S.SetChecked(false);
                chkf8S.SetChecked(false);

                txt_f_descripciondetransaccionmercancias.SetEnabled(false);
                txt_f_descripciondetransaccionmercanciasidenticas.SetEnabled(false);
                txt_f_descripciondetransaccionmercanciassimilares.SetEnabled(false);
                txt_f_descripcionvalordepreciounitariodeventa.SetEnabled(false);
                txt_f_descripcionvalorconformaley.SetEnabled(false);

                txt_f_descripciondetransaccionmercancias.SetText('');
                txt_f_descripciondetransaccionmercanciasidenticas.SetText('');
                txt_f_descripciondetransaccionmercanciassimilares.SetText('');
                txt_f_descripcionvalordepreciounitariodeventa.SetText('');
                txt_f_descripcionvalorreconstruido.SetText('');
                txt_f_descripcionvalorconformaley.SetText('');
            }
            else {

                if (cheked) {
                    chkf7S.SetChecked(true);
                    txt_f_descripcionvalorreconstruido.SetEnabled(true);

                }
                else if (!cheked) {
                    chkf7S.SetChecked(false);
                    txt_f_descripcionvalorreconstruido.SetEnabled(false);
                }

                txt_f_descripcionvalorreconstruido.SetText('');
            }
        }

        function Get_chkf8S(s, e) {
            var cheked = s.GetChecked();
            var chk = chkf1S.GetChecked();

            if (chk) {

                if (cheked) {
                    chkf8S.SetChecked(true);
                    txt_f_descripcionvalorconformaley.SetEnabled(true);

                }
                else if (!cheked) {
                    chkf8S.SetChecked(false);
                    txt_f_descripcionvalorconformaley.SetEnabled(false);
                }

                chkf3S.SetChecked(false);
                chkf4S.SetChecked(false);
                chkf5S.SetChecked(false);
                chkf6S.SetChecked(false);
                chkf7S.SetChecked(false);

                txt_f_descripciondetransaccionmercancias.SetEnabled(false);
                txt_f_descripciondetransaccionmercanciasidenticas.SetEnabled(false);
                txt_f_descripciondetransaccionmercanciassimilares.SetEnabled(false);
                txt_f_descripcionvalordepreciounitariodeventa.SetEnabled(false);
                txt_f_descripcionvalorreconstruido.SetEnabled(false);

                txt_f_descripciondetransaccionmercancias.SetText('');
                txt_f_descripciondetransaccionmercanciasidenticas.SetText('');
                txt_f_descripciondetransaccionmercanciassimilares.SetText('');
                txt_f_descripcionvalordepreciounitariodeventa.SetText('');
                txt_f_descripcionvalorreconstruido.SetText('');
                txt_f_descripcionvalorconformaley.SetText('');
            }
            else {

                if (cheked) {
                    chkf8S.SetChecked(true);
                    txt_f_descripcionvalorconformaley.SetEnabled(true);

                }
                else if (!cheked) {
                    chkf8S.SetChecked(false);
                    txt_f_descripcionvalorconformaley.SetEnabled(false);
                }

                txt_f_descripcionvalorconformaley.SetText('');
            }
        }
        
        /*******************************************************************************/

        /*******************************************************************************/
        /*funcion para para validar el checkbox en Anexos */
        function Get_chkg1S(s, e) {
            var cheked = s.GetChecked();

            if (cheked) {
                chkg1S.SetChecked(true);
                txt_g_anexadocumentacionnumeroyletra.SetEnabled(true);
            }
            else {

                chkg1S.SetChecked(false);
                txt_g_anexadocumentacionnumeroyletra.SetEnabled(false);
            }

            txt_g_anexadocumentacionnumeroyletra.SetText('');
        }
        /*******************************************************************************/


        /*******************************************************************************/
        /*funciones para validar en VTM */
        function Valor_VTM(s) {
            var valor = se_ga_preciofacturanumero.GetValue();
            //var _preciofacturaletra = txt_ga_preciofacturaletra.GetValue();
            //var _anexadocumentacionmodena = txt_g_anexadocumentacionmodena.GetValue();

            if (isNaN(valor) || !valor || 0 === valor.length) {
                se_ga_preciofacturanumero.SetValue(0);
                valor = 0;
            }

            <%--
            var txt_ga_preciofacturaletra = myTrim(document.getElementById('<%=txt_ga_preciofacturaletra.ClientID%>'));
            var txt_g_anexadocumentacionmodena = myTrim(document.getElementById('<%=txt_g_anexadocumentacionmodena.ClientID%>'));

                        
            var lkb_OrdenAnexo65_Agregar = document.getElementById('<%=lkb_OrdenAnexo65_Agregar.ClientID%>');
            var lkb_ManifestacionConcepto65_Agregar = document.getElementById('<%=lkb_ManifestacionConcepto65_Agregar.ClientID%>');
            

            var bandera = 0;

            if (valor > 0) {
                bandera = 1;
            }
            if (bandera == 0) {
                if (_preciofacturaletra != null && ASPxClientUtils.Trim(_preciofacturaletra).length > 0) {
                    bandera = 1;
                }
                if (bandera == 0) {
                    if (_anexadocumentacionmodena != null && ASPxClientUtils.Trim(_anexadocumentacionmodena).value.length > 0) {
                        bandera = 1;
                    }
                    if (bandera == 0) {
                        if (chkvtm_b1S.GetChecked())
                            bandera = 1;
                        if (bandera == 0) {
                            if (chkvtm_b2S.GetChecked())
                                bandera = 1;
                            if (bandera == 0) {
                                if (chkvtm_b3S.GetChecked())
                                    bandera = 1;
                                if (bandera == 0) {
                                    if (chkvtm_b4S.GetChecked())
                                        bandera = 1;
                                    if (bandera == 0) {
                                        if (grid3.GetVisibleRowsOnPage() > 0)
                                            bandera = 1;
                                        if (bandera == 0) {
                                            if (chkvtm_c1S.GetChecked())
                                                bandera = 1;
                                            if (bandera == 0) {
                                                if (grid4.GetVisibleRowsOnPage() > 0)
                                                    bandera = 1;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }


            if (bandera == 1) {
                chk65_1S.SetEnabled(false);
                chk65_1N.SetEnabled(false);
                chk65_2S.SetEnabled(false);
                chk65_2N.SetEnabled(false);
                chk65_3S.SetEnabled(false);
                chk65_3N.SetEnabled(false);
                $("#ContentSection_ASPxPageControl1_lkb_OrdenAnexo65_Agregar").attr("disabled", "disabled");
                $("#ContentSection_ASPxPageControl1_lkb_OrdenAnexo65_Agregar").bind('click', false);
                $("#ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto65_Agregar").attr("disabled", "disabled");
                $("#ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto65_Agregar").bind('click', false);


                //$("#ContentSection_ASPxPageControl1_lkb_OrdenAnexo65_Agregar").attr('disabled', true);
                //$('[id$=ContentSection_ASPxPageControl1_lkb_OrdenAnexo65_Agregar]').removeAttr('href');
                //$("#ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto65_Agregar").attr('disabled', true);
                //$('[id$=ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto65_Agregar]').removeAttr('href');

                //lkb_OrdenAnexo65_Agregar.style.display = "none";  //'inline'
                //lkb_OrdenAnexo65_Agregar.style.visibility = 'hidden';  //'visible'
                //lkb_ManifestacionConcepto65_Agregar.style.display = "none";  //'inline'
                //lkb_ManifestacionConcepto65_Agregar.style.visibility = 'hidden';  //'visible'
            }
            else {
                chk65_1S.SetEnabled(true);
                chk65_1N.SetEnabled(true);
                chk65_2S.SetEnabled(true);
                chk65_2N.SetEnabled(true);
                chk65_3S.SetEnabled(true);
                chk65_3N.SetEnabled(true);
                $("#ContentSection_ASPxPageControl1_lkb_OrdenAnexo65_Agregar").removeAttr("disabled");
                $("#ContentSection_ASPxPageControl1_lkb_OrdenAnexo65_Agregar").unbind('click', false);
                $("#ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto65_Agregar").removeAttr("disabled");
                $("#ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto65_Agregar").unbind('click', false);

                //$("#ContentSection_ASPxPageControl1_lkb_OrdenAnexo65_Agregar").attr('disabled', false);
                //$('[id$=ContentSection_ASPxPageControl1_lkb_OrdenAnexo65_Agregar]').attr('href',);
                //$("#ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto65_Agregar").attr('disabled', false);
                //$('[id$=ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto65_Agregar]').attr('href');

            }

            chk65_1S.SetChecked(false);
            chk65_1N.SetChecked(false);
            chk65_2S.SetChecked(false);
            chk65_2N.SetChecked(false);
            chk65_3S.SetChecked(false);
            chk65_3N.SetChecked(false);

            //El __doPostBack l oque hace es que en el Page_Load con Request["__EVENTARGUMENT"] trae valor 'VTM', 
            //si lo trae ejecuta el metódo btnVTM_Click() del botón para que limpie Grid8_MV y Grid5_MV
            __doPostBack('btnVTM', 'VTM');
            --%>
        }

        /*******************************************************************************/

        /*******************************************************************************/
        /*funcion que valida si la pestaña de Articulo 65 de la ley se habilita o deshabilita */

        //Pequeña función para cadenas borrar los espacios en blanco tipo trim()
        function myTrim(x) {
            return x.value.replace(/^\s+|\s+$/gm, '');
        }

        function ValorTransaccionMercancias() {

            <%--
            var valor = se_ga_preciofacturanumero.GetValue();
            
            var _preciofacturaletra = txt_ga_preciofacturaletra.GetValue();
            var _anexadocumentacionmodena = txt_g_anexadocumentacionmodena.GetValue();

            
            var txt_ga_preciofacturaletra = myTrim(document.getElementById('<%=txt_ga_preciofacturaletra.ClientID%>'));
            var txt_g_anexadocumentacionmodena = myTrim(document.getElementById('<%=txt_g_anexadocumentacionmodena.ClientID%>'));
            --%>

            var bandera = 0;

            //if (valor > 0) {
            //    bandera = 1;
            //}            
            if (bandera == 0) {
                //if (_preciofacturaletra != null && ASPxClientUtils.Trim(_preciofacturaletra).length > 0) {
                //    bandera = 1;
                //}
                //if (bandera == 0) {
                //    if (_anexadocumentacionmodena != null && ASPxClientUtils.Trim(_anexadocumentacionmodena).value.length > 0) {
                //        bandera = 1;
                //    }
                //    if (bandera == 0) {
                        if (chkvtm_b1S.GetChecked())
                            bandera = 1;
                        if (bandera == 0) {
                            if (chkvtm_b2S.GetChecked())
                                bandera = 1;
                            if (bandera == 0) {
                                if (chkvtm_b3S.GetChecked())
                                    bandera = 1;
                                if (bandera == 0) {
                                    if (chkvtm_b4S.GetChecked())
                                        bandera = 1;
                                    if (bandera == 0) {
                                        if (grid3.GetVisibleRowsOnPage() > 0)
                                            bandera = 1;
                                        if (bandera == 0) {
                                            if (chkvtm_c1S.GetChecked())
                                                bandera = 1;
                                            if (bandera == 0) {
                                                if (grid4.GetVisibleRowsOnPage() > 0)
                                                    bandera = 1;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    //}
                //}
            }


            if(bandera == 1){
                chk65_1S.SetEnabled(false);
                chk65_1N.SetEnabled(false);
                chk65_2S.SetEnabled(false);
                chk65_2N.SetEnabled(false);
                chk65_3S.SetEnabled(false);
                chk65_3N.SetEnabled(false);
                $("#ContentSection_ASPxPageControl1_lkb_OrdenAnexo65_Agregar").attr("disabled", "disabled");
                $("#ContentSection_ASPxPageControl1_lkb_OrdenAnexo65_Agregar").bind('click', false);
                $("#ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto65_Agregar").attr("disabled", "disabled");
                $("#ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto65_Agregar").bind('click', false);
            }
            else {
                chk65_1S.SetEnabled(true);
                chk65_1N.SetEnabled(true);
                chk65_2S.SetEnabled(true);
                chk65_2N.SetEnabled(true);
                chk65_3S.SetEnabled(true);
                chk65_3N.SetEnabled(true);
                $("#ContentSection_ASPxPageControl1_lkb_OrdenAnexo65_Agregar").removeAttr("disabled");
                $("#ContentSection_ASPxPageControl1_lkb_OrdenAnexo65_Agregar").unbind('click', false);
                $("#ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto65_Agregar").removeAttr("disabled");
                $("#ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto65_Agregar").unbind('click', false);
            }

            chk65_1S.SetChecked(false);
            chk65_1N.SetChecked(false);
            chk65_2S.SetChecked(false);
            chk65_2N.SetChecked(false);
            chk65_3S.SetChecked(false);
            chk65_3N.SetChecked(false);
        }

        function Get_chkvtm_b1S(s, e) {

            //var valor = se_ga_preciofacturanumero.GetValue();            
            var cheked = s.GetChecked();

            var bandera = 0;

            //if (valor > 0) {
            //    bandera = 1;
            //}
            if (bandera == 0) {                                
                if (cheked)
                    bandera = 1;
                if (bandera == 0) {
                    if (chkvtm_b2S.GetChecked())
                        bandera = 1;
                    if (bandera == 0) {
                        if (chkvtm_b3S.GetChecked())
                            bandera = 1;
                        if (bandera == 0) {
                            if (chkvtm_b4S.GetChecked())
                                bandera = 1;
                            if (bandera == 0) {
                                if (grid3.GetVisibleRowsOnPage() > 0)
                                    bandera = 1;
                                if (bandera == 0) {
                                    if (chkvtm_c1S.GetChecked())
                                        bandera = 1;
                                    if (bandera == 0) {
                                        if (grid4.GetVisibleRowsOnPage() > 0)
                                            bandera = 1;
                                    }
                                }
                            }
                        }
                    }
                }                        
            }


            if (bandera == 1) {
                chk65_1S.SetEnabled(false);
                chk65_1N.SetEnabled(false);
                chk65_2S.SetEnabled(false);
                chk65_2N.SetEnabled(false);
                chk65_3S.SetEnabled(false);
                chk65_3N.SetEnabled(false);
                $("#ContentSection_ASPxPageControl1_lkb_OrdenAnexo65_Agregar").attr("disabled", "disabled");
                $("#ContentSection_ASPxPageControl1_lkb_OrdenAnexo65_Agregar").bind('click', false);
                $("#ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto65_Agregar").attr("disabled", "disabled");
                $("#ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto65_Agregar").bind('click', false);
            }
            else {
                chk65_1S.SetEnabled(true);
                chk65_1N.SetEnabled(true);
                chk65_2S.SetEnabled(true);
                chk65_2N.SetEnabled(true);
                chk65_3S.SetEnabled(true);
                chk65_3N.SetEnabled(true);
                $("#ContentSection_ASPxPageControl1_lkb_OrdenAnexo65_Agregar").removeAttr("disabled");
                $("#ContentSection_ASPxPageControl1_lkb_OrdenAnexo65_Agregar").unbind('click', false);
                $("#ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto65_Agregar").removeAttr("disabled");
                $("#ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto65_Agregar").unbind('click', false);
            }

            chk65_1S.SetChecked(false);
            chk65_1N.SetChecked(false);
            chk65_2S.SetChecked(false);
            chk65_2N.SetChecked(false);
            chk65_3S.SetChecked(false);
            chk65_3N.SetChecked(false);

            //El __doPostBack l oque hace es que en el Page_Load con Request["__EVENTARGUMENT"] trae valor 'VTM', 
            //si lo trae ejecuta el metódo btnVTM_Click() del botón para que limpie Grid8_MV y Grid5_MV
            __doPostBack('btnVTM', 'VTM');
        }

        function Get_chkvtm_b2S(s, e) {
            
            var cheked = s.GetChecked();
            var bandera = 0;

            if (bandera == 0) {
                
                if (chkvtm_b1S.GetChecked())
                    bandera = 1;
                if (bandera == 0) {
                    if (cheked)
                        bandera = 1;
                    if (bandera == 0) {
                        if (chkvtm_b3S.GetChecked())
                            bandera = 1;
                        if (bandera == 0) {
                            if (chkvtm_b4S.GetChecked())
                                bandera = 1;
                            if (bandera == 0) {
                                if (grid3.GetVisibleRowsOnPage() > 0)
                                    bandera = 1;
                                if (bandera == 0) {
                                    if (chkvtm_c1S.GetChecked())
                                        bandera = 1;
                                    if (bandera == 0) {
                                        if (grid4.GetVisibleRowsOnPage() > 0)
                                            bandera = 1;
                                    }
                                }
                            }
                        }
                    }
                }                             
            }


            if (bandera == 1) {
                chk65_1S.SetEnabled(false);
                chk65_1N.SetEnabled(false);
                chk65_2S.SetEnabled(false);
                chk65_2N.SetEnabled(false);
                chk65_3S.SetEnabled(false);
                chk65_3N.SetEnabled(false);
                $("#ContentSection_ASPxPageControl1_lkb_OrdenAnexo65_Agregar").attr("disabled", "disabled");
                $("#ContentSection_ASPxPageControl1_lkb_OrdenAnexo65_Agregar").bind('click', false);
                $("#ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto65_Agregar").attr("disabled", "disabled");
                $("#ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto65_Agregar").bind('click', false);
            }
            else {
                chk65_1S.SetEnabled(true);
                chk65_1N.SetEnabled(true);
                chk65_2S.SetEnabled(true);
                chk65_2N.SetEnabled(true);
                chk65_3S.SetEnabled(true);
                chk65_3N.SetEnabled(true);
                $("#ContentSection_ASPxPageControl1_lkb_OrdenAnexo65_Agregar").removeAttr("disabled");
                $("#ContentSection_ASPxPageControl1_lkb_OrdenAnexo65_Agregar").unbind('click', false);
                $("#ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto65_Agregar").removeAttr("disabled");
                $("#ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto65_Agregar").unbind('click', false);
            }

            chk65_1S.SetChecked(false);
            chk65_1N.SetChecked(false);
            chk65_2S.SetChecked(false);
            chk65_2N.SetChecked(false);
            chk65_3S.SetChecked(false);
            chk65_3N.SetChecked(false);

            //El __doPostBack l oque hace es que en el Page_Load con Request["__EVENTARGUMENT"] trae valor 'VTM', 
            //si lo trae ejecuta el metódo btnVTM_Click() del botón para que limpie Grid8_MV y Grid5_MV
            __doPostBack('btnVTM', 'VTM');
        }

        function Get_chkvtm_b3S(s, e) {
         
            var cheked = s.GetChecked();
            var bandera = 0;

            if (bandera == 0) {
                
                if (chkvtm_b1S.GetChecked())
                    bandera = 1;
                if (bandera == 0) {
                    if (chkvtm_b2S.GetChecked())
                        bandera = 1;
                    if (bandera == 0) {
                        if (cheked)
                            bandera = 1;
                        if (bandera == 0) {
                            if (chkvtm_b4S.GetChecked())
                                bandera = 1;
                            if (bandera == 0) {
                                if (grid3.GetVisibleRowsOnPage() > 0)
                                    bandera = 1;
                                if (bandera == 0) {
                                    if (chkvtm_c1S.GetChecked())
                                        bandera = 1;
                                    if (bandera == 0) {
                                        if (grid4.GetVisibleRowsOnPage() > 0)
                                            bandera = 1;
                                    }
                                }
                            }
                        }
                    }
                }
            }


            if (bandera == 1) {
                chk65_1S.SetEnabled(false);
                chk65_1N.SetEnabled(false);
                chk65_2S.SetEnabled(false);
                chk65_2N.SetEnabled(false);
                chk65_3S.SetEnabled(false);
                chk65_3N.SetEnabled(false);
                $("#ContentSection_ASPxPageControl1_lkb_OrdenAnexo65_Agregar").attr("disabled", "disabled");
                $("#ContentSection_ASPxPageControl1_lkb_OrdenAnexo65_Agregar").bind('click', false);
                $("#ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto65_Agregar").attr("disabled", "disabled");
                $("#ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto65_Agregar").bind('click', false);
            }
            else {
                chk65_1S.SetEnabled(true);
                chk65_1N.SetEnabled(true);
                chk65_2S.SetEnabled(true);
                chk65_2N.SetEnabled(true);
                chk65_3S.SetEnabled(true);
                chk65_3N.SetEnabled(true);
                $("#ContentSection_ASPxPageControl1_lkb_OrdenAnexo65_Agregar").removeAttr("disabled");
                $("#ContentSection_ASPxPageControl1_lkb_OrdenAnexo65_Agregar").unbind('click', false);
                $("#ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto65_Agregar").removeAttr("disabled");
                $("#ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto65_Agregar").unbind('click', false);
            }

            chk65_1S.SetChecked(false);
            chk65_1N.SetChecked(false);
            chk65_2S.SetChecked(false);
            chk65_2N.SetChecked(false);
            chk65_3S.SetChecked(false);
            chk65_3N.SetChecked(false);

            //El __doPostBack l oque hace es que en el Page_Load con Request["__EVENTARGUMENT"] trae valor 'VTM', 
            //si lo trae ejecuta el metódo btnVTM_Click() del botón para que limpie Grid8_MV y Grid5_MV
            __doPostBack('btnVTM', 'VTM');
        }

        function Get_chkvtm_b4S(s, e) {
        
            var cheked = s.GetChecked();
            var bandera = 0;

            if (bandera == 0) {
                
                if (chkvtm_b1S.GetChecked())
                    bandera = 1;
                if (bandera == 0) {
                    if (chkvtm_b2S.GetChecked())
                        bandera = 1;
                    if (bandera == 0) {
                        if (chkvtm_b3S.GetChecked())
                            bandera = 1;
                        if (bandera == 0) {
                            if (cheked)
                                bandera = 1;
                            if (bandera == 0) {
                                if (grid3.GetVisibleRowsOnPage() > 0)
                                    bandera = 1;
                                if (bandera == 0) {
                                    if (chkvtm_c1S.GetChecked())
                                        bandera = 1;
                                    if (bandera == 0) {
                                        if (grid4.GetVisibleRowsOnPage() > 0)
                                            bandera = 1;
                                    }
                                }
                            }
                        }
                    }
                }
            }


            if (bandera == 1) {
                chk65_1S.SetEnabled(false);
                chk65_1N.SetEnabled(false);
                chk65_2S.SetEnabled(false);
                chk65_2N.SetEnabled(false);
                chk65_3S.SetEnabled(false);
                chk65_3N.SetEnabled(false);
                $("#ContentSection_ASPxPageControl1_lkb_OrdenAnexo65_Agregar").attr("disabled", "disabled");
                $("#ContentSection_ASPxPageControl1_lkb_OrdenAnexo65_Agregar").bind('click', false);
                $("#ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto65_Agregar").attr("disabled", "disabled");
                $("#ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto65_Agregar").bind('click', false);
            }
            else {
                chk65_1S.SetEnabled(true);
                chk65_1N.SetEnabled(true);
                chk65_2S.SetEnabled(true);
                chk65_2N.SetEnabled(true);
                chk65_3S.SetEnabled(true);
                chk65_3N.SetEnabled(true);
                $("#ContentSection_ASPxPageControl1_lkb_OrdenAnexo65_Agregar").removeAttr("disabled");
                $("#ContentSection_ASPxPageControl1_lkb_OrdenAnexo65_Agregar").unbind('click', false);
                $("#ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto65_Agregar").removeAttr("disabled");
                $("#ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto65_Agregar").unbind('click', false);
            }

            chk65_1S.SetChecked(false);
            chk65_1N.SetChecked(false);
            chk65_2S.SetChecked(false);
            chk65_2N.SetChecked(false);
            chk65_3S.SetChecked(false);
            chk65_3N.SetChecked(false);

            //El __doPostBack l oque hace es que en el Page_Load con Request["__EVENTARGUMENT"] trae valor 'VTM', 
            //si lo trae ejecuta el metódo btnVTM_Click() del botón para que limpie Grid8_MV y Grid5_MV
            __doPostBack('btnVTM', 'VTM');
        }

        function Get_chkvtm_c1S(s, e) {
       
            var cheked = s.GetChecked();
            var bandera = 0;

            if (bandera == 0) {
                
                if (chkvtm_b1S.GetChecked())
                    bandera = 1;
                if (bandera == 0) {
                    if (chkvtm_b2S.GetChecked())
                        bandera = 1;
                    if (bandera == 0) {
                        if (chkvtm_b3S.GetChecked())
                            bandera = 1;
                        if (bandera == 0) {
                            if (chkvtm_b4S.GetChecked())
                                bandera = 1;
                            if (bandera == 0) {
                                if (grid3.GetVisibleRowsOnPage() > 0)
                                    bandera = 1;
                                if (bandera == 0) {
                                    if (cheked)
                                        bandera = 1;
                                    if (bandera == 0) {
                                        if (grid4.GetVisibleRowsOnPage() > 0)
                                            bandera = 1;
                                    }
                                }
                            }
                        }
                    }
                }
            }


            if (bandera == 1) {
                chk65_1S.SetEnabled(false);
                chk65_1N.SetEnabled(false);
                chk65_2S.SetEnabled(false);
                chk65_2N.SetEnabled(false);
                chk65_3S.SetEnabled(false);
                chk65_3N.SetEnabled(false);
                $("#ContentSection_ASPxPageControl1_lkb_OrdenAnexo65_Agregar").attr("disabled", "disabled");
                $("#ContentSection_ASPxPageControl1_lkb_OrdenAnexo65_Agregar").bind('click', false);
                $("#ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto65_Agregar").attr("disabled", "disabled");
                $("#ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto65_Agregar").bind('click', false);
            }
            else {
                chk65_1S.SetEnabled(true);
                chk65_1N.SetEnabled(true);
                chk65_2S.SetEnabled(true);
                chk65_2N.SetEnabled(true);
                chk65_3S.SetEnabled(true);
                chk65_3N.SetEnabled(true);
                $("#ContentSection_ASPxPageControl1_lkb_OrdenAnexo65_Agregar").removeAttr("disabled");
                $("#ContentSection_ASPxPageControl1_lkb_OrdenAnexo65_Agregar").unbind('click', false);
                $("#ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto65_Agregar").removeAttr("disabled");
                $("#ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto65_Agregar").unbind('click', false);
            }

            chk65_1S.SetChecked(false);
            chk65_1N.SetChecked(false);
            chk65_2S.SetChecked(false);
            chk65_2N.SetChecked(false);
            chk65_3S.SetChecked(false);
            chk65_3N.SetChecked(false);

            //El __doPostBack l oque hace es que en el Page_Load con Request["__EVENTARGUMENT"] trae valor 'VTM', 
            //si lo trae ejecuta el metódo btnVTM_Click() del botón para que limpie Grid8_MV y Grid5_MV
            __doPostBack('btnVTM', 'VTM');
        }

        /*******************************************************************************/
        /*funciones para para validar en Articulo 65 Ley */
        function Articulo65() {

            <%--          
            var txt_ga_preciofacturaletra = myTrim(document.getElementById('<%=txt_ga_preciofacturaletra.ClientID%>'));
            var txt_g_anexadocumentacionmodena = myTrim(document.getElementById('<%=txt_g_anexadocumentacionmodena.ClientID%>'));
            --%>

            //Valida los controles de Articulo 65 de la Ley

            var bandera = 0;

            if (chk65_1S.GetChecked())
                bandera = 1;
            if (bandera == 0) {
                if (chk65_1N.GetChecked())
                    bandera = 1;
                if (bandera == 0) {
                    if (chk65_2S.GetChecked())
                        bandera = 1;
                    if (bandera == 0) {
                        if (chk65_2N.GetChecked())
                            bandera = 1;
                        if (bandera == 0) {
                            if (chk65_3S.GetChecked())
                                bandera = 1;
                            if (bandera == 0) {
                                if (grid8.GetVisibleRowsOnPage() > 0)
                                    bandera = 1;
                                if (bandera == 0) {
                                    if (chk65_3N.GetChecked())
                                        bandera = 1;
                                    if (bandera == 0) {
                                        if (grid5.GetVisibleRowsOnPage() > 0)
                                            bandera = 1;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (bandera == 1) {

                //se_ga_preciofacturanumero.SetEnabled(false);
                //txt_ga_preciofacturaletra.SetEnabled(false);
                //txt_g_anexadocumentacionmodena.SetEnabled(false);
                //$("#ContentSection_ASPxPageControl1_txt_ga_preciofacturaletra").disabled = true;
                //$("#ContentSection_ASPxPageControl1_txt_g_anexadocumentacionmodena").disabled = true;
                chkvtm_b1S.SetEnabled(false);
                chkvtm_b2S.SetEnabled(false);
                chkvtm_b3S.SetEnabled(false);
                chkvtm_b4S.SetEnabled(false);
                chkvtm_c1S.SetEnabled(false);

                $("#ContentSection_ASPxPageControl1_lkb_Anexo_Agregar").attr("disabled", "disabled");
                $("#ContentSection_ASPxPageControl1_lkb_Anexo_Agregar").bind('click', false);
                $("#ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto66_Agregar").attr("disabled", "disabled");
                $("#ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto66_Agregar").bind('click', false);

            }
            else {

                //se_ga_preciofacturanumero.SetEnabled(true);
                //txt_ga_preciofacturaletra.SetEnabled(true);
                //txt_g_anexadocumentacionmodena.SetEnabled(true);
                //$("#ContentSection_ASPxPageControl1_txt_ga_preciofacturaletra").disabled = false;
                //$("#ContentSection_ASPxPageControl1_txt_g_anexadocumentacionmodena").disabled = false;
                chkvtm_b1S.SetEnabled(true);
                chkvtm_b2S.SetEnabled(true);
                chkvtm_b3S.SetEnabled(true);
                chkvtm_b4S.SetEnabled(true);
                chkvtm_c1S.SetEnabled(true);

                $("#ContentSection_ASPxPageControl1_lkb_Anexo_Agregar").removeAttr("disabled");
                $("#ContentSection_ASPxPageControl1_lkb_Anexo_Agregar").unbind('click', false);
                $("#ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto66_Agregar").removeAttr("disabled");
                $("#ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto66_Agregar").unbind('click', false);
            }
            
            //txt_ga_preciofacturaletra.SetValue('');
            //txt_g_anexadocumentacionmodena.SetValue('');
            chkvtm_b1S.SetChecked(false);
            chkvtm_b2S.SetChecked(false);
            chkvtm_b3S.SetChecked(false);
            chkvtm_b4S.SetChecked(false);
            chkvtm_c1S.SetChecked(false);
            //se_ga_preciofacturanumero.SetValue(0);
        }


        function Get_chk65_1S(s, e) {
            var cheked = s.GetChecked();

            <%--          
            var txt_ga_preciofacturaletra = myTrim(document.getElementById('<%=txt_ga_preciofacturaletra.ClientID%>'));
            var txt_g_anexadocumentacionmodena = myTrim(document.getElementById('<%=txt_g_anexadocumentacionmodena.ClientID%>'));
            --%>

            if (cheked) {
                chk65_1N.SetChecked(false);

                //se_ga_preciofacturanumero.SetEnabled(false);
                //txt_ga_preciofacturaletra.SetEnabled(false);
                //txt_g_anexadocumentacionmodena.SetEnabled(false);
                //$("#ContentSection_ASPxPageControl1_txt_ga_preciofacturaletra").disabled = true;
                //$("#ContentSection_ASPxPageControl1_txt_g_anexadocumentacionmodena").disabled = true;
                chkvtm_b1S.SetEnabled(false);
                chkvtm_b2S.SetEnabled(false);
                chkvtm_b3S.SetEnabled(false);
                chkvtm_b4S.SetEnabled(false);
                chkvtm_c1S.SetEnabled(false);                

                $("#ContentSection_ASPxPageControl1_lkb_Anexo_Agregar").attr("disabled", "disabled");
                $("#ContentSection_ASPxPageControl1_lkb_Anexo_Agregar").bind('click', false);
                $("#ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto66_Agregar").attr("disabled", "disabled");
                $("#ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto66_Agregar").bind('click', false);

                //El __doPostBack lo que hace es que en el Page_Load con Request["__EVENTARGUMENT"] trae valor 'A65', 
                //si lo trae ejecuta el metódo btnA65_Click() del botón para que limpie Grid3_MV y Grid4_MV
                __doPostBack('btnR65', 'R65');
            }
            else {

                //Valida los demás controles de Articulo 65 de la Ley

                var bandera = 0;

                if (chk65_1N.GetChecked())
                    bandera = 1;
                if (bandera == 0) {
                    if (chk65_2S.GetChecked())
                        bandera = 1;
                    if (bandera == 0) {
                        if (chk65_2N.GetChecked())
                            bandera = 1;
                        if (bandera == 0) {
                            if (chk65_3S.GetChecked())
                                bandera = 1;
                            if (bandera == 0) {
                                if (grid8.GetVisibleRowsOnPage() > 0)
                                    bandera = 1;
                                if (bandera == 0) {
                                    if (chk65_3N.GetChecked())
                                        bandera = 1;
                                    if (bandera == 0) {
                                        if (grid5.GetVisibleRowsOnPage() > 0)
                                            bandera = 1;
                                    }
                                }
                            }
                        }
                    }
                }

                if (bandera == 1) {

                    //se_ga_preciofacturanumero.SetEnabled(false);
                    //txt_ga_preciofacturaletra.SetEnabled(false);
                    //txt_g_anexadocumentacionmodena.SetEnabled(false);
                    //$("#ContentSection_ASPxPageControl1_txt_ga_preciofacturaletra").disabled = true;
                    //$("#ContentSection_ASPxPageControl1_txt_g_anexadocumentacionmodena").disabled = true;
                    chkvtm_b1S.SetEnabled(false);
                    chkvtm_b2S.SetEnabled(false);
                    chkvtm_b3S.SetEnabled(false);
                    chkvtm_b4S.SetEnabled(false);
                    chkvtm_c1S.SetEnabled(false);

                    $("#ContentSection_ASPxPageControl1_lkb_Anexo_Agregar").attr("disabled", "disabled");
                    $("#ContentSection_ASPxPageControl1_lkb_Anexo_Agregar").bind('click', false);
                    $("#ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto66_Agregar").attr("disabled", "disabled");
                    $("#ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto66_Agregar").bind('click', false);

                    //El __doPostBack lo que hace es que en el Page_Load con Request["__EVENTARGUMENT"] trae valor 'A65', 
                    //si lo trae ejecuta el metódo btnA65_Click() del botón para que limpie Grid3_MV y Grid4_MV
                    __doPostBack('btnR65', 'R65');
                }
                else {

                    //se_ga_preciofacturanumero.SetEnabled(true);
                    //txt_ga_preciofacturaletra.SetEnabled(true);
                    //txt_g_anexadocumentacionmodena.SetEnabled(true);
                    //$("#ContentSection_ASPxPageControl1_txt_ga_preciofacturaletra").disabled = false;
                    //$("#ContentSection_ASPxPageControl1_txt_g_anexadocumentacionmodena").disabled = false;
                    chkvtm_b1S.SetEnabled(true);
                    chkvtm_b2S.SetEnabled(true);
                    chkvtm_b3S.SetEnabled(true);
                    chkvtm_b4S.SetEnabled(true);
                    chkvtm_c1S.SetEnabled(true);

                    $("#ContentSection_ASPxPageControl1_lkb_Anexo_Agregar").removeAttr("disabled");
                    $("#ContentSection_ASPxPageControl1_lkb_Anexo_Agregar").unbind('click', false);
                    $("#ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto66_Agregar").removeAttr("disabled");
                    $("#ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto66_Agregar").unbind('click', false);
                }

            }

            //se_ga_preciofacturanumero.SetValue(0);
            //txt_ga_preciofacturaletra.SetValue('');
            //txt_g_anexadocumentacionmodena.SetValue('');
            chkvtm_b1S.SetChecked(false);
            chkvtm_b2S.SetChecked(false);
            chkvtm_b3S.SetChecked(false);
            chkvtm_b4S.SetChecked(false);
            chkvtm_c1S.SetChecked(false);
        }

        function Get_chk65_1N(s, e) {
            var cheked = s.GetChecked();

            if (cheked) {
                chk65_1S.SetChecked(false);

                chkvtm_b1S.SetEnabled(false);
                chkvtm_b2S.SetEnabled(false);
                chkvtm_b3S.SetEnabled(false);
                chkvtm_b4S.SetEnabled(false);
                chkvtm_c1S.SetEnabled(false);

                $("#ContentSection_ASPxPageControl1_lkb_Anexo_Agregar").attr("disabled", "disabled");
                $("#ContentSection_ASPxPageControl1_lkb_Anexo_Agregar").bind('click', false);
                $("#ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto66_Agregar").attr("disabled", "disabled");
                $("#ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto66_Agregar").bind('click', false);

                //El __doPostBack lo que hace es que en el Page_Load con Request["__EVENTARGUMENT"] trae valor 'A65', 
                //si lo trae ejecuta el metódo btnA65_Click() del botón para que limpie Grid3_MV y Grid4_MV
                __doPostBack('btnR65', 'R65');
            }
            else {

                //Valida los demás controles de Articulo 65 de la Ley

                var bandera = 0;

                if (chk65_1S.GetChecked())
                    bandera = 1;
                if (bandera == 0) {
                    if (chk65_2S.GetChecked())
                        bandera = 1;
                    if (bandera == 0) {
                        if (chk65_2N.GetChecked())
                            bandera = 1;
                        if (bandera == 0) {
                            if (chk65_3S.GetChecked())
                                bandera = 1;
                            if (bandera == 0) {
                                if (grid8.GetVisibleRowsOnPage() > 0)
                                    bandera = 1;
                                if (bandera == 0) {
                                    if (chk65_3N.GetChecked())
                                        bandera = 1;
                                    if (bandera == 0) {
                                        if (grid5.GetVisibleRowsOnPage() > 0)
                                            bandera = 1;
                                    }
                                }
                            }
                        }
                    }
                }

                if (bandera == 1) {

                    chkvtm_b1S.SetEnabled(false);
                    chkvtm_b2S.SetEnabled(false);
                    chkvtm_b3S.SetEnabled(false);
                    chkvtm_b4S.SetEnabled(false);
                    chkvtm_c1S.SetEnabled(false);

                    $("#ContentSection_ASPxPageControl1_lkb_Anexo_Agregar").attr("disabled", "disabled");
                    $("#ContentSection_ASPxPageControl1_lkb_Anexo_Agregar").bind('click', false);
                    $("#ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto66_Agregar").attr("disabled", "disabled");
                    $("#ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto66_Agregar").bind('click', false);

                    //El __doPostBack lo que hace es que en el Page_Load con Request["__EVENTARGUMENT"] trae valor 'A65', 
                    //si lo trae ejecuta el metódo btnA65_Click() del botón para que limpie Grid3_MV y Grid4_MV
                    __doPostBack('btnR65', 'R65');
                }
                else {

                    chkvtm_b1S.SetEnabled(true);
                    chkvtm_b2S.SetEnabled(true);
                    chkvtm_b3S.SetEnabled(true);
                    chkvtm_b4S.SetEnabled(true);
                    chkvtm_c1S.SetEnabled(true);

                    $("#ContentSection_ASPxPageControl1_lkb_Anexo_Agregar").removeAttr("disabled");
                    $("#ContentSection_ASPxPageControl1_lkb_Anexo_Agregar").unbind('click', false);
                    $("#ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto66_Agregar").removeAttr("disabled");
                    $("#ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto66_Agregar").unbind('click', false);
                }

            }

            chkvtm_b1S.SetChecked(false);
            chkvtm_b2S.SetChecked(false);
            chkvtm_b3S.SetChecked(false);
            chkvtm_b4S.SetChecked(false);
            chkvtm_c1S.SetChecked(false);
        }

        function Get_chk65_2S(s, e) {
            var cheked = s.GetChecked();

            if (cheked) {
                chk65_2N.SetChecked(false);

                chkvtm_b1S.SetEnabled(false);
                chkvtm_b2S.SetEnabled(false);
                chkvtm_b3S.SetEnabled(false);
                chkvtm_b4S.SetEnabled(false);
                chkvtm_c1S.SetEnabled(false);

                $("#ContentSection_ASPxPageControl1_lkb_Anexo_Agregar").attr("disabled", "disabled");
                $("#ContentSection_ASPxPageControl1_lkb_Anexo_Agregar").bind('click', false);
                $("#ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto66_Agregar").attr("disabled", "disabled");
                $("#ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto66_Agregar").bind('click', false);

                //El __doPostBack lo que hace es que en el Page_Load con Request["__EVENTARGUMENT"] trae valor 'A65', 
                //si lo trae ejecuta el metódo btnA65_Click() del botón para que limpie Grid3_MV y Grid4_MV
                __doPostBack('btnR65', 'R65');
            }
            else {

                //Valida los demás controles de Articulo 65 de la Ley

                var bandera = 0;

                if (chk65_1S.GetChecked())
                    bandera = 1;
                if (bandera == 0) {
                    if (chk65_1N.GetChecked())
                        bandera = 1;
                    if (bandera == 0) {
                        if (chk65_2N.GetChecked())
                            bandera = 1;
                        if (bandera == 0) {
                            if (chk65_3S.GetChecked())
                                bandera = 1;
                            if (bandera == 0) {
                                if (grid8.GetVisibleRowsOnPage() > 0)
                                    bandera = 1;
                                if (bandera == 0) {
                                    if (chk65_3N.GetChecked())
                                        bandera = 1;
                                    if (bandera == 0) {
                                        if (grid5.GetVisibleRowsOnPage() > 0)
                                            bandera = 1;
                                    }
                                }
                            }
                        }
                    }
                }

                if (bandera == 1) {

                    chkvtm_b1S.SetEnabled(false);
                    chkvtm_b2S.SetEnabled(false);
                    chkvtm_b3S.SetEnabled(false);
                    chkvtm_b4S.SetEnabled(false);
                    chkvtm_c1S.SetEnabled(false);

                    $("#ContentSection_ASPxPageControl1_lkb_Anexo_Agregar").attr("disabled", "disabled");
                    $("#ContentSection_ASPxPageControl1_lkb_Anexo_Agregar").bind('click', false);
                    $("#ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto66_Agregar").attr("disabled", "disabled");
                    $("#ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto66_Agregar").bind('click', false);

                    //El __doPostBack lo que hace es que en el Page_Load con Request["__EVENTARGUMENT"] trae valor 'A65', 
                    //si lo trae ejecuta el metódo btnA65_Click() del botón para que limpie Grid3_MV y Grid4_MV
                    __doPostBack('btnR65', 'R65');
                }
                else {

                    chkvtm_b1S.SetEnabled(true);
                    chkvtm_b2S.SetEnabled(true);
                    chkvtm_b3S.SetEnabled(true);
                    chkvtm_b4S.SetEnabled(true);
                    chkvtm_c1S.SetEnabled(true);

                    $("#ContentSection_ASPxPageControl1_lkb_Anexo_Agregar").removeAttr("disabled");
                    $("#ContentSection_ASPxPageControl1_lkb_Anexo_Agregar").unbind('click', false);
                    $("#ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto66_Agregar").removeAttr("disabled");
                    $("#ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto66_Agregar").unbind('click', false);
                }

            }

            chkvtm_b1S.SetChecked(false);
            chkvtm_b2S.SetChecked(false);
            chkvtm_b3S.SetChecked(false);
            chkvtm_b4S.SetChecked(false);
            chkvtm_c1S.SetChecked(false);
        }

        function Get_chk65_2N(s, e) {
            var cheked = s.GetChecked();

            if (cheked) {
                chk65_2S.SetChecked(false);
            
                chkvtm_b1S.SetEnabled(false);
                chkvtm_b2S.SetEnabled(false);
                chkvtm_b3S.SetEnabled(false);
                chkvtm_b4S.SetEnabled(false);
                chkvtm_c1S.SetEnabled(false);

                $("#ContentSection_ASPxPageControl1_lkb_Anexo_Agregar").attr("disabled", "disabled");
                $("#ContentSection_ASPxPageControl1_lkb_Anexo_Agregar").bind('click', false);
                $("#ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto66_Agregar").attr("disabled", "disabled");
                $("#ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto66_Agregar").bind('click', false);

                //El __doPostBack lo que hace es que en el Page_Load con Request["__EVENTARGUMENT"] trae valor 'A65', 
                //si lo trae ejecuta el metódo btnA65_Click() del botón para que limpie Grid3_MV y Grid4_MV
                __doPostBack('btnR65', 'R65');
            }
            else {

                //Valida los demás controles de Articulo 65 de la Ley

                var bandera = 0;

                if (chk65_1S.GetChecked())
                    bandera = 1;
                if (bandera == 0) {
                    if (chk65_1N.GetChecked())
                        bandera = 1;
                    if (bandera == 0) {
                        if (chk65_2S.GetChecked())
                            bandera = 1;
                        if (bandera == 0) {
                            if (chk65_3S.GetChecked())
                                bandera = 1;
                            if (bandera == 0) {
                                if (grid8.GetVisibleRowsOnPage() > 0)
                                    bandera = 1;
                                if (bandera == 0) {
                                    if (chk65_3N.GetChecked())
                                        bandera = 1;
                                    if (bandera == 0) {
                                        if (grid5.GetVisibleRowsOnPage() > 0)
                                            bandera = 1;
                                    }
                                }
                            }
                        }
                    }
                }

                if (bandera == 1) {

                    chkvtm_b1S.SetEnabled(false);
                    chkvtm_b2S.SetEnabled(false);
                    chkvtm_b3S.SetEnabled(false);
                    chkvtm_b4S.SetEnabled(false);
                    chkvtm_c1S.SetEnabled(false);

                    $("#ContentSection_ASPxPageControl1_lkb_Anexo_Agregar").attr("disabled", "disabled");
                    $("#ContentSection_ASPxPageControl1_lkb_Anexo_Agregar").bind('click', false);
                    $("#ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto66_Agregar").attr("disabled", "disabled");
                    $("#ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto66_Agregar").bind('click', false);

                    //El __doPostBack lo que hace es que en el Page_Load con Request["__EVENTARGUMENT"] trae valor 'A65', 
                    //si lo trae ejecuta el metódo btnA65_Click() del botón para que limpie Grid3_MV y Grid4_MV
                    __doPostBack('btnR65', 'R65');
                }
                else {

                    chkvtm_b1S.SetEnabled(true);
                    chkvtm_b2S.SetEnabled(true);
                    chkvtm_b3S.SetEnabled(true);
                    chkvtm_b4S.SetEnabled(true);
                    chkvtm_c1S.SetEnabled(true);

                    $("#ContentSection_ASPxPageControl1_lkb_Anexo_Agregar").removeAttr("disabled");
                    $("#ContentSection_ASPxPageControl1_lkb_Anexo_Agregar").unbind('click', false);
                    $("#ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto66_Agregar").removeAttr("disabled");
                    $("#ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto66_Agregar").unbind('click', false);
                }

            }

            chkvtm_b1S.SetChecked(false);
            chkvtm_b2S.SetChecked(false);
            chkvtm_b3S.SetChecked(false);
            chkvtm_b4S.SetChecked(false);
            chkvtm_c1S.SetChecked(false);
        }

        function Get_chk65_3S(s, e) {
            var cheked = s.GetChecked();

            if (cheked) {
                chk65_3N.SetChecked(false);
            
                chkvtm_b1S.SetEnabled(false);
                chkvtm_b2S.SetEnabled(false);
                chkvtm_b3S.SetEnabled(false);
                chkvtm_b4S.SetEnabled(false);
                chkvtm_c1S.SetEnabled(false);

                $("#ContentSection_ASPxPageControl1_lkb_Anexo_Agregar").attr("disabled", "disabled");
                $("#ContentSection_ASPxPageControl1_lkb_Anexo_Agregar").bind('click', false);
                $("#ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto66_Agregar").attr("disabled", "disabled");
                $("#ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto66_Agregar").bind('click', false);

                //El __doPostBack lo que hace es que en el Page_Load con Request["__EVENTARGUMENT"] trae valor 'A65', 
                //si lo trae ejecuta el metódo btnA65_Click() del botón para que limpie Grid3_MV y Grid4_MV
                __doPostBack('btnR65', 'R65');
            }
            else {

                //Valida los demás controles de Articulo 65 de la Ley

                var bandera = 0;

                if (chk65_1S.GetChecked())
                    bandera = 1;
                if (bandera == 0) {
                    if (chk65_1N.GetChecked())
                        bandera = 1;
                    if (bandera == 0) {
                        if (chk65_2S.GetChecked())
                            bandera = 1;
                        if (bandera == 0) {
                            if (chk65_2N.GetChecked())
                                bandera = 1;
                            if (bandera == 0) {
                                if (grid8.GetVisibleRowsOnPage() > 0)
                                    bandera = 1;
                                if (bandera == 0) {
                                    if (chk65_3N.GetChecked())
                                        bandera = 1;
                                    if (bandera == 0) {
                                        if (grid5.GetVisibleRowsOnPage() > 0)
                                            bandera = 1;
                                    }
                                }
                            }
                        }
                    }
                }

                if (bandera == 1) {

                    chkvtm_b1S.SetEnabled(false);
                    chkvtm_b2S.SetEnabled(false);
                    chkvtm_b3S.SetEnabled(false);
                    chkvtm_b4S.SetEnabled(false);
                    chkvtm_c1S.SetEnabled(false);

                    $("#ContentSection_ASPxPageControl1_lkb_Anexo_Agregar").attr("disabled", "disabled");
                    $("#ContentSection_ASPxPageControl1_lkb_Anexo_Agregar").bind('click', false);
                    $("#ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto66_Agregar").attr("disabled", "disabled");
                    $("#ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto66_Agregar").bind('click', false);

                    //El __doPostBack lo que hace es que en el Page_Load con Request["__EVENTARGUMENT"] trae valor 'A65', 
                    //si lo trae ejecuta el metódo btnA65_Click() del botón para que limpie Grid3_MV y Grid4_MV
                    __doPostBack('btnR65', 'R65');
                }
                else {

                    chkvtm_b1S.SetEnabled(true);
                    chkvtm_b2S.SetEnabled(true);
                    chkvtm_b3S.SetEnabled(true);
                    chkvtm_b4S.SetEnabled(true);
                    chkvtm_c1S.SetEnabled(true);

                    $("#ContentSection_ASPxPageControl1_lkb_Anexo_Agregar").removeAttr("disabled");
                    $("#ContentSection_ASPxPageControl1_lkb_Anexo_Agregar").unbind('click', false);
                    $("#ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto66_Agregar").removeAttr("disabled");
                    $("#ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto66_Agregar").unbind('click', false);
                }

            }

            chkvtm_b1S.SetChecked(false);
            chkvtm_b2S.SetChecked(false);
            chkvtm_b3S.SetChecked(false);
            chkvtm_b4S.SetChecked(false);
            chkvtm_c1S.SetChecked(false);
        }

        function Get_chk65_3N(s, e) {
            var cheked = s.GetChecked();

            if (cheked) {
                chk65_3S.SetChecked(false);
            
                chkvtm_b1S.SetEnabled(false);
                chkvtm_b2S.SetEnabled(false);
                chkvtm_b3S.SetEnabled(false);
                chkvtm_b4S.SetEnabled(false);
                chkvtm_c1S.SetEnabled(false);

                $("#ContentSection_ASPxPageControl1_lkb_Anexo_Agregar").attr("disabled", "disabled");
                $("#ContentSection_ASPxPageControl1_lkb_Anexo_Agregar").bind('click', false);
                $("#ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto66_Agregar").attr("disabled", "disabled");
                $("#ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto66_Agregar").bind('click', false);

                //El __doPostBack lo que hace es que en el Page_Load con Request["__EVENTARGUMENT"] trae valor 'A65', 
                //si lo trae ejecuta el metódo btnA65_Click() del botón para que limpie Grid3_MV y Grid4_MV
                __doPostBack('btnR65', 'R65');
            }
            else {

                //Valida los demás controles de Articulo 65 de la Ley

                var bandera = 0;

                if (chk65_1S.GetChecked())
                    bandera = 1;
                if (bandera == 0) {
                    if (chk65_1N.GetChecked())
                        bandera = 1;
                    if (bandera == 0) {
                        if (chk65_2S.GetChecked())
                            bandera = 1;
                        if (bandera == 0) {
                            if (chk65_2N.GetChecked())
                                bandera = 1;
                            if (bandera == 0) {
                                if (grid8.GetVisibleRowsOnPage() > 0)
                                    bandera = 1;
                                if (bandera == 0) {
                                    if (chk65_3S.GetChecked())
                                        bandera = 1;
                                    if (bandera == 0) {
                                        if (grid5.GetVisibleRowsOnPage() > 0)
                                            bandera = 1;
                                    }
                                }
                            }
                        }
                    }
                }

                if (bandera == 1) {

                    chkvtm_b1S.SetEnabled(false);
                    chkvtm_b2S.SetEnabled(false);
                    chkvtm_b3S.SetEnabled(false);
                    chkvtm_b4S.SetEnabled(false);
                    chkvtm_c1S.SetEnabled(false);

                    $("#ContentSection_ASPxPageControl1_lkb_Anexo_Agregar").attr("disabled", "disabled");
                    $("#ContentSection_ASPxPageControl1_lkb_Anexo_Agregar").bind('click', false);
                    $("#ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto66_Agregar").attr("disabled", "disabled");
                    $("#ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto66_Agregar").bind('click', false);

                    //El __doPostBack lo que hace es que en el Page_Load con Request["__EVENTARGUMENT"] trae valor 'A65', 
                    //si lo trae ejecuta el metódo btnA65_Click() del botón para que limpie Grid3_MV y Grid4_MV
                    __doPostBack('btnR65', 'R65');
                }
                else {

                    chkvtm_b1S.SetEnabled(true);
                    chkvtm_b2S.SetEnabled(true);
                    chkvtm_b3S.SetEnabled(true);
                    chkvtm_b4S.SetEnabled(true);
                    chkvtm_c1S.SetEnabled(true);

                    $("#ContentSection_ASPxPageControl1_lkb_Anexo_Agregar").removeAttr("disabled");
                    $("#ContentSection_ASPxPageControl1_lkb_Anexo_Agregar").unbind('click', false);
                    $("#ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto66_Agregar").removeAttr("disabled");
                    $("#ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto66_Agregar").unbind('click', false);
                }

            }

            chkvtm_b1S.SetChecked(false);
            chkvtm_b2S.SetChecked(false);
            chkvtm_b3S.SetChecked(false);
            chkvtm_b4S.SetChecked(false);
            chkvtm_c1S.SetChecked(false);
        }
        /*******************************************************************************/

        /*******************************************************************************/
        /*funciones para validar los checkboxes en Otros Metodos  */
        function Get_chkOtros_1S(s, e) {
            var cheked = s.GetChecked();

            if (cheked) {
                chkOtros_1N.SetChecked(false);
            }
        }

        function Get_chkOtros_1N(s, e) {
            var cheked = s.GetChecked();

            if (cheked) {
                chkOtros_1S.SetChecked(false);
            }
        }

        function Get_chkOtros_2S(s, e) {
            var cheked = s.GetChecked();

            if (cheked) {
                chkOtros_2N.SetChecked(false);
            }
        }

        function Get_chkOtros_2N(s, e) {
            var cheked = s.GetChecked();

            if (cheked) {
                chkOtros_2S.SetChecked(false);
            }
        }
        
        /*******************************************************************************/

        /*******************************************************************************/
        /*funciones para para validar los checkboxes en Importaciones Temporal  */
        function Get_chkIT_1S(s, e) {
            var cheked = s.GetChecked();

            if (cheked) {
                chkIT_1N.SetChecked(false);
            }
        }

        function Get_chkIT_1N(s, e) {
            var cheked = s.GetChecked();

            if (cheked) {
                chkIT_1S.SetChecked(false);
            }
        }

        function Get_chkIT_2S(s, e) {
            var cheked = s.GetChecked();

            if (cheked) {
                chkIT_2N.SetChecked(false);
            }
        }

        function Get_chkIT_2N(s, e) {
            var cheked = s.GetChecked();

            if (cheked) {
                chkIT_2S.SetChecked(false);
            }
        }

        /*******************************************************************************/

        /*******************************************************************************/
        /*funciones para para validar en Periocidad de la Manifestación de Valor */
        function Get_chkPMV_1S(s, e) {
            var cheked = s.GetChecked();

            if (cheked) {
                chkPMV_1N.SetChecked(false);
            }
        }

        function Get_chkPMV_1N(s, e) {
            var cheked = s.GetChecked();

            if (cheked) {
                chkPMV_1S.SetChecked(false);
            }
        }





        /*Función realiza la suma del punto 05*/
        <%--function ValorNuevoTotal05(s) {
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
        }--%>

        /*Función realiza la suma del punto 06*/
        <%--function ValorNuevoTotal06(s) {
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
        }--%>

        /*Función realiza la suma del punto 07*/
        <%--function ValorNuevoTotal07(s) {
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
        }--%>


        /*Función realiza punto 08*/
        //function Valida08(s) {
        //    var pagar = se8_Pagar.GetValue();
        //    var ajuste = se8_Ajuste.GetValue();
        //    var aduana = se8_Aduana.GetValue();
          

        //    if (isNaN(pagar) || !pagar || 0 === pagar.length) {
        //        se8_Pagar.SetValue(0);
        //    }

        //    if (isNaN(ajuste) || !ajuste || 0 === ajuste.length) {
        //        se8_Ajuste.SetValue(0);
        //    }

        //    if (isNaN(aduana) || !aduana || 0 === aduana.length) {
        //        se8_Aduana.SetValue(0);
        //    }

        //}

        
        

        ///*Función le agrega comas a un número mayor a 999, funciona con decimales */
        //function formatNumber(num) {
        //    return num.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,')
        //}


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
        
        //Grid_MV
        //Se usa para palomear o despalomear el checkbox all del grid Grid_MV
        function chkMarcarTodoClick(s, e) {
            
            var cheked = s.GetChecked();

            for (var i = 0; i < grid.GetVisibleRowsOnPage() ; i++) {
                var chkConsultar1 = eval("chkConsultar1" + i);
                chkConsultar1.SetChecked(cheked);
            }
        }

        //Grid_MV
        //Se usa para palomear o despalomear los checkbox del grid Grid_MV
        function chkConsultar1_Init(s, e, index) {
        }


        //Se usa para palomear el checkbox seleccionado y despalomear los demás en Grid de Representante Legal
        function chkConsultarFClick(s, e, index) {
            var vlkb_Editar = document.getElementById('ContentSection_ASPxPageControl1_lkb_Factura_Editar')
            var vlkb_Eliminar = document.getElementById('ContentSection_ASPxPageControl1_lkb_Factura_Eliminar')


            if (!vlkb_Editar.classList.contains('disabled'))
                vlkb_Editar.classList.add('disabled');

            if (!vlkb_Eliminar.classList.contains('disabled'))
                vlkb_Eliminar.classList.add('disabled');

            var cheked = s.GetChecked();
            var row = grid2.GetDataRow(index);


            var chkConsultar = eval("chkConsultar" + index);
            var valor = chkConsultar.GetChecked();
            chkConsultar.SetChecked(valor);

            for (var i = 0; i < grid2.GetVisibleRowsOnPage() ; i++) {
                if (i == index) {
                    chkConsultar = eval("chkConsultar" + i);
                    chkConsultar.SetChecked(valor);

                    if (valor == true) {

                        vlkb_Editar.classList.remove('disabled');
                        vlkb_Eliminar.classList.remove('disabled');
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

        //Se usa para palomear el checkbox seleccionado y despalomear los demás en Grid3_MV
        function chkConsultarTMClick(s, e, index) {
            var vlkb_Editar = document.getElementById('ContentSection_ASPxPageControl1_lkb_Anexo_Editar')
            var vlkb_Eliminar = document.getElementById('ContentSection_ASPxPageControl1_lkb_Anexo_Eliminar')


            if (!vlkb_Editar.classList.contains('disabled'))
                vlkb_Editar.classList.add('disabled');

            if (!vlkb_Eliminar.classList.contains('disabled'))
                vlkb_Eliminar.classList.add('disabled');

            var cheked = s.GetChecked();
            var row = grid3.GetDataRow(index);


            var chkConsultar = eval("chkConsultarTM" + index);
            var valor = chkConsultar.GetChecked();
            chkConsultar.SetChecked(valor);

            for (var i = 0; i < grid3.GetVisibleRowsOnPage() ; i++) {
                if (i == index) {
                    chkConsultar = eval("chkConsultarTM" + i);
                    chkConsultar.SetChecked(valor);

                    if (valor == true) {

                        vlkb_Editar.classList.remove('disabled');
                        vlkb_Eliminar.classList.remove('disabled');
                    }
                }
                else {
                    try {
                        chkConsultar = eval("chkConsultarTM" + i);
                        chkConsultar.SetChecked(false);
                    }
                    catch (err) { }
                }
            }
        }

        //Se usa para palomear el checkbox seleccionado y despalomear los demás en Grid4_MV
        function chkConsultarTM2Click(s, e, index) {
            var vlkb_Editar = document.getElementById('ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto66_Editar')
            var vlkb_Eliminar = document.getElementById('ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto66_Eliminar')


            if (!vlkb_Editar.classList.contains('disabled'))
                vlkb_Editar.classList.add('disabled');

            if (!vlkb_Eliminar.classList.contains('disabled'))
                vlkb_Eliminar.classList.add('disabled');

            var cheked = s.GetChecked();
            var row = grid4.GetDataRow(index);


            var chkConsultar = eval("chkConsultarTM2" + index);
            var valor = chkConsultar.GetChecked();
            chkConsultar.SetChecked(valor);

            for (var i = 0; i < grid4.GetVisibleRowsOnPage() ; i++) {
                if (i == index) {
                    chkConsultar = eval("chkConsultarTM2" + i);
                    chkConsultar.SetChecked(valor);

                    if (valor == true) {

                        vlkb_Editar.classList.remove('disabled');
                        vlkb_Eliminar.classList.remove('disabled');
                    }
                }
                else {
                    try {
                        chkConsultar = eval("chkConsultarTM2" + i);
                        chkConsultar.SetChecked(false);
                    }
                    catch (err) { }
                }
            }
        }


        //Se usa para palomear el checkbox seleccionado y despalomear los demás en Grid8_MV
        function chkConsultarS65Click(s, e, index) {
            var vlkb_Editar = document.getElementById('ContentSection_ASPxPageControl1_lkb_OrdenAnexo65_Editar')
            var vlkb_Eliminar = document.getElementById('ContentSection_ASPxPageControl1_lkb_OrdenAnexo65_Eliminar')


            if (!vlkb_Editar.classList.contains('disabled'))
                vlkb_Editar.classList.add('disabled');

            if (!vlkb_Eliminar.classList.contains('disabled'))
                vlkb_Eliminar.classList.add('disabled');

            var cheked = s.GetChecked();
            var row = grid8.GetDataRow(index);


            var chkConsultar = eval("chkConsultarS65" + index);
            var valor = chkConsultar.GetChecked();
            chkConsultar.SetChecked(valor);

            for (var i = 0; i < grid8.GetVisibleRowsOnPage() ; i++) {
                if (i == index) {
                    chkConsultar = eval("chkConsultarS65" + i);
                    chkConsultar.SetChecked(valor);

                    if (valor == true) {

                        vlkb_Editar.classList.remove('disabled');
                        vlkb_Eliminar.classList.remove('disabled');
                    }
                }
                else {
                    try {
                        chkConsultar = eval("chkConsultarS65" + i);
                        chkConsultar.SetChecked(false);
                    }
                    catch (err) { }
                }
            }
        }

        //Se usa para palomear el checkbox seleccionado y despalomear los demás en Grid5_MV
        function chkConsultarN65Click(s, e, index) {
            var vlkb_Editar = document.getElementById('ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto65_Editar')
            var vlkb_Eliminar = document.getElementById('ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto65_Eliminar')


            if (!vlkb_Editar.classList.contains('disabled'))
                vlkb_Editar.classList.add('disabled');

            if (!vlkb_Eliminar.classList.contains('disabled'))
                vlkb_Eliminar.classList.add('disabled');

            var cheked = s.GetChecked();
            var row = grid5.GetDataRow(index);


            var chkConsultar = eval("chkConsultarN65" + index);
            var valor = chkConsultar.GetChecked();
            chkConsultar.SetChecked(valor);

            for (var i = 0; i < grid5.GetVisibleRowsOnPage() ; i++) {
                if (i == index) {
                    chkConsultar = eval("chkConsultarN65" + i);
                    chkConsultar.SetChecked(valor);

                    if (valor == true) {

                        vlkb_Editar.classList.remove('disabled');
                        vlkb_Eliminar.classList.remove('disabled');
                    }
                }
                else {
                    try {
                        chkConsultar = eval("chkConsultarN65" + i);
                        chkConsultar.SetChecked(false);
                    }
                    catch (err) { }
                }
            }
        }


        //Se usa para palomear el checkbox seleccionado y despalomear los demás en Grid6_MV
        function chkConsultarOTClick(s, e, index) {
            var vlkb_Editar = document.getElementById('ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto67_Editar')
            var vlkb_Eliminar = document.getElementById('ContentSection_ASPxPageControl1_lkb_ManifestacionConcepto67_Eliminar')


            if (!vlkb_Editar.classList.contains('disabled'))
                vlkb_Editar.classList.add('disabled');

            if (!vlkb_Eliminar.classList.contains('disabled'))
                vlkb_Eliminar.classList.add('disabled');

            var cheked = s.GetChecked();
            var row = grid6.GetDataRow(index);


            var chkConsultar = eval("chkConsultarOT" + index);
            var valor = chkConsultar.GetChecked();
            chkConsultar.SetChecked(valor);

            for (var i = 0; i < grid6.GetVisibleRowsOnPage() ; i++) {
                if (i == index) {
                    chkConsultar = eval("chkConsultarOT" + i);
                    chkConsultar.SetChecked(valor);

                    if (valor == true) {

                        vlkb_Editar.classList.remove('disabled');
                        vlkb_Eliminar.classList.remove('disabled');
                    }
                }
                else {
                    try {
                        chkConsultar = eval("chkConsultarOT" + i);
                        chkConsultar.SetChecked(false);
                    }
                    catch (err) { }
                }
            }
        }


        //Se usa para palomear el checkbox seleccionado y despalomear los demás en Grid7_MV
        function chkConsultarOT2Click(s, e, index) {
            var vlkb_Editar = document.getElementById('ContentSection_ASPxPageControl1_lkb_ManifestacionValorAduana_Editar')
            var vlkb_Eliminar = document.getElementById('ContentSection_ASPxPageControl1_lkb_ManifestacionValorAduana_Eliminar')


            if (!vlkb_Editar.classList.contains('disabled'))
                vlkb_Editar.classList.add('disabled');

            if (!vlkb_Eliminar.classList.contains('disabled'))
                vlkb_Eliminar.classList.add('disabled');

            var cheked = s.GetChecked();
            var row = grid7.GetDataRow(index);


            var chkConsultar = eval("chkConsultarOT2" + index);
            var valor = chkConsultar.GetChecked();
            chkConsultar.SetChecked(valor);

            for (var i = 0; i < grid7.GetVisibleRowsOnPage() ; i++) {
                if (i == index) {
                    chkConsultar = eval("chkConsultarOT2" + i);
                    chkConsultar.SetChecked(valor);

                    if (valor == true) {

                        vlkb_Editar.classList.remove('disabled');
                        vlkb_Eliminar.classList.remove('disabled');
                    }
                }
                else {
                    try {
                        chkConsultar = eval("chkConsultarOT2" + i);
                        chkConsultar.SetChecked(false);
                    }
                    catch (err) { }
                }
            }
        }

        //Se usa para palomear el checkbox seleccionado y despalomear los demás en Grid9_MV
        function chkConsultarITClick(s, e, index) {
            var vlkb_Editar = document.getElementById('ContentSection_ASPxPageControl1_lkb_ManifestacionMercanciaProvisional_Editar')
            var vlkb_Eliminar = document.getElementById('ContentSection_ASPxPageControl1_lkb_ManifestacionMercanciaProvisional_Eliminar')


            if (!vlkb_Editar.classList.contains('disabled'))
                vlkb_Editar.classList.add('disabled');

            if (!vlkb_Eliminar.classList.contains('disabled'))
                vlkb_Eliminar.classList.add('disabled');

            var cheked = s.GetChecked();
            var row = grid9.GetDataRow(index);


            var chkConsultar = eval("chkConsultarIT" + index);
            var valor = chkConsultar.GetChecked();
            chkConsultar.SetChecked(valor);

            for (var i = 0; i < grid9.GetVisibleRowsOnPage() ; i++) {
                if (i == index) {
                    chkConsultar = eval("chkConsultarIT" + i);
                    chkConsultar.SetChecked(valor);

                    if (valor == true) {

                        vlkb_Editar.classList.remove('disabled');
                        vlkb_Eliminar.classList.remove('disabled');
                    }
                }
                else {
                    try {
                        chkConsultar = eval("chkConsultarIT" + i);
                        chkConsultar.SetChecked(false);
                    }
                    catch (err) { }
                }
            }
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
                     <div class="row" style="padding-top:10px">                        
                        <div class="form-group col-sm-6 col-md-3">
                           <div runat="server" id="DivPedimento">
                               <div class="form-group" style="position: relative; width: 100%; float: left;">
                                   <div class="input-group">
                                       <span class="input-group-addon" id="basic-addon1">
                                           <span class="glyphicon glyphicon-list-alt" aria-hidden="true"></span>
                                       </span>
                                       <asp:TextBox ID="PEDIMENTO" runat="server" CssClass="form-control input-sm" MaxLength="50" placeholder="Pedimento" Width="100%"></asp:TextBox>
                                   </div>
                               </div>
                           </div>
                        </div>
                        <div class="form-group col-sm-6 col-md-1"></div>                        
                        <div class="form-group col-sm-4 col-md-4">
                            <table style="width:100%">
                                <tr>
                                    <td style="width:50%">
                                        <dx:ASPxDateEdit ID="DESDE" runat="server" ClientInstanceName="DESDE" EditFormat="Custom" Date="" Width="120px" Height="30px" Theme="MaterialCompact" 
                                             Font-Size="11px" CssClass="bordes_curvos" NullText="Desde" DisplayFormatString="dd/MM/yyyy">
                                             <CalendarProperties EnableMonthNavigation="True" EnableYearNavigation="True" ShowClearButton="False" ShowDayHeaders="True" ShowTodayButton="False" ShowWeekNumbers="False">
                                                 <MonthGridPaddings Padding="0px" />                                                                 
                                                 <Style Font-Size="10px"></Style>
                                             </CalendarProperties>
                                        </dx:ASPxDateEdit>
                                    </td>
                                    <td style="width:50%">
                                        <dx:ASPxDateEdit ID="HASTA" runat="server" ClientInstanceName="HASTA" EditFormat="Custom" Date="" Width="120px" Height="30px" Theme="MaterialCompact" 
                                             Font-Size="11px" CssClass="bordes_curvos" NullText="Hasta" DisplayFormatString="dd/MM/yyyy">
                                             <CalendarProperties EnableMonthNavigation="True" EnableYearNavigation="True" ShowClearButton="False" ShowDayHeaders="True" ShowTodayButton="False" ShowWeekNumbers="False">
                                                 <MonthGridPaddings Padding="0px" />                                                                 
                                                 <Style Font-Size="10px"></Style>
                                             </CalendarProperties>
                                        </dx:ASPxDateEdit>
                                    </td>
                                </tr>
                            </table>
                        </div>                   
                        <div class="form-group col-sm-6 col-md-2">
                            <div runat="server" id="DivBuscar">
                                <div class="form-group" style="position: relative; width: 50%; float: left;" >
                                    <div class="input-group">
                                        <dx:BootstrapButton ID="btnBuscar" runat="server" AutoPostBack="false" OnClick="btnBuscar_OnClick" 
                                            SettingsBootstrap-RenderOption="Primary" Text="&nbsp;&nbsp;&nbsp; Crear Manifestación de Valor &nbsp;&nbsp;&nbsp;" CssClasses-Text="txt-sm">
                                            <SettingsBootstrap RenderOption="Primary" Sizing="Small" />
                                            <ClientSideEvents Click="function(s, e) { Callback.PerformCallback(); LoadingPanel1.Show(); }" />
                                        </dx:BootstrapButton>
                                    </div>
                                </div>
                            </div>                                
                        </div>
                        <div class="form-group col-sm-6 col-md-2"></div>                        
                     </div>
                     <div style="padding-bottom:5px">
                        <dx:BootstrapButton ID="lkb_LimpiarFiltros" runat="server" AutoPostBack="false" CssClasses-Text="txt-sm" data-toggle="tooltip" OnClick="lkb_LimpiarFiltros_Click" SettingsBootstrap-RenderOption="Primary" SettingsBootstrap-Sizing="Small" Text="Limpiar Filtros">
                         <ClientSideEvents Click="function(s, e) { Callback.PerformCallback(); LoadingPanel1.Show(); }" Init="function(s, e) {LoadingPanel1.Hide();}" />
                         <CssClasses Icon="glyphicon glyphicon-erase" />
                        </dx:BootstrapButton>
                        <asp:LinkButton ID="lkb_Excel" runat="server" CssClass="btn btn-primary btn-sm text-right txt-sm" data-toggle="tooltip" OnClick="lkb_Excel_Click" >
                           <span class="glyphicon glyphicon-list"></span>&nbsp;&nbsp;Excel
                        </asp:LinkButton>
                        <dx:BootstrapButton ID="lkb_Actualizar" runat="server" AutoPostBack="false" CssClasses-Text="txt-sm" data-toggle="tooltip" OnClick="lkb_Actualizar_Click" SettingsBootstrap-RenderOption="Primary" SettingsBootstrap-Sizing="Small" Text="Actualizar" >
                         <ClientSideEvents Click="function(s, e) { Callback.PerformCallback(); LoadingPanel1.Show(); }" Init="function(s, e) {LoadingPanel1.Hide();}" />
                         <CssClasses Icon="glyphicon glyphicon-refresh" />
                        </dx:BootstrapButton>
                     </div>
                     
                     
                     <dx:ASPxGridView ID="Grid_MV" runat="server" AutoGenerateColumns="False" ClientInstanceName="grid" EnableCallBacks="false" 
                        EnableTheming="True" KeyFieldName="mvkeymanifestacion" OnCustomCallback="Grid_CustomCallback" OnToolbarItemClick="Grid_ToolbarItemClick" 
                        Settings-HorizontalScrollBarMode="Auto" Styles-Cell-Font-Size="11px" Styles-Header-Font-Size="11px" SettingsPager-Mode="ShowAllRecords" 
                        Theme="SoftOrange" Width="100%" OnCustomButtonCallback="Grid_CustomButtonCallback">
                        <SettingsAdaptivity AdaptiveDetailColumnCount="1" AdaptivityMode="HideDataCellsWindowLimit" AllowOnlyOneAdaptiveDetailExpanded="True" HideDataCellsAtWindowInnerWidth="800">
                            <AdaptiveDetailLayoutProperties colcount="2">
                                <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit" SwitchToSingleColumnAtWindowInnerWidth="600" />
                            </AdaptiveDetailLayoutProperties>
                        </SettingsAdaptivity>
                        <SettingsResizing ColumnResizeMode="Control" />
                        <SettingsBehavior AllowSelectByRowClick="false" AllowSelectSingleRowOnly="true" />
                        <Settings ShowFooter="False" ShowHeaderFilterButton="true" ShowFilterRowMenu="false" />
                        <Styles>                                                                                      
                            <SelectedRow CssClass="background_color_btn background_texto_btn" />
                            <Row  />
                            <AlternatingRow Enabled="True" />
                        </Styles>
                        <SettingsPopup>
                            <HeaderFilter Height="200px" Width="195px" />
                        </SettingsPopup>                        
                        <Columns>
                            <dx:GridViewDataTextColumn FieldName="mvkeymanifestacion" ReadOnly="True" Visible="false" VisibleIndex="0">
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
                                   <dx:GridViewCommandColumnCustomButton ID="btnEditar" Text="Editar">
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
                                   <dx:GridViewCommandColumnCustomButton ID="btnEliminar" Text="Eliminar">
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
                                            <td style="width:40%">&nbsp;&nbsp;
                                                <dx:ASPxButton ID="ASPxButtonDetailDoc" runat="server" AutoPostBack="false" OnClick="ASPxButtonDetailDoc_Click"
                                                    EnableTheming="false" RenderMode="Link" VerticalAlign="Top" ToolTip="Hoja Cálculo" OnInit="ASPxButtonDetailDoc_Init">
                                                </dx:ASPxButton>
                                            </td>                                           
                                        </tr>
                                     </table>
                                </DataItemTemplate>                                 
                                <SettingsHeaderFilter Mode="CheckedList" />
                            </dx:GridViewDataColumn>                          
                            <dx:GridViewDataDateColumn Caption="FECHA PEDIMENTO" FieldName="MV_FECHAPEDIMENTO" ReadOnly="True" VisibleIndex="4" Width="170px">
                                <HeaderStyle HorizontalAlign="Center" />
                                <SettingsHeaderFilter Mode="CheckedList" />
                                <CellStyle HorizontalAlign="Center" />
                            </dx:GridViewDataDateColumn>
                            <dx:GridViewDataDateColumn Caption="FECHA ELABORACIÓN" FieldName="FECHAELABORACION" ReadOnly="True" VisibleIndex="5" Width="170px">
                                <HeaderStyle HorizontalAlign="Center" />
                                <SettingsHeaderFilter Mode="CheckedList" />
                                <CellStyle HorizontalAlign="Center" />
                            </dx:GridViewDataDateColumn>
                            <dx:GridViewDataTextColumn Caption="NOMBRE EXPORTADOR" FieldName="a_nombre" VisibleIndex="6" Width="444px">
                                <HeaderStyle HorizontalAlign="Center" />
                                <CellStyle HorizontalAlign ="Left" />
                                <SettingsHeaderFilter Mode="CheckedList" />
                            </dx:GridViewDataTextColumn>                           
                        </Columns>                        
                     </dx:ASPxGridView>
                     <div style="padding-bottom:5px; padding-top:5px">
                         <button id="btnZip" type="button" style="display: none;" onclick="return window.location = 'FileDownloadHandlerMV.ashx?id=All';">
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
                     <table style="width: 100%">
                         <tr>
                             <td style="text-align: left; width: 50%">
                                 <h1 id="h2_titulo" runat="server" class="panel-title small"></h1>
                             </td>
                             <td style="text-align: right; width: 50%">
                                 <asp:LinkButton ID="LinkButton9" runat="server" OnClick="lkb_Regresar_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button">
                                      <span class="glyphicon glyphicon-circle-arrow-left"></span>&nbsp;&nbsp;Regresar
                                 </asp:LinkButton>
                             </td>
                         </tr>
                     </table>
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
                    <div class="col-sm-4 col-md-5"></div>
                    
                    <dx:ASPxPageControl runat="server" ID="ASPxPageControl1" Height="60px" Width="98%" EnableCallBacks="false"  ContentStyle-Border-BorderWidth="3px"
                            TabAlign="Justify" ActiveTabIndex="0" EnableTabScrolling="true" Theme="SoftOrange" Font-Size="12px" ContentStyle-VerticalAlign="Top"
                            EnableViewState="False" EnableHierarchyRecreation="True">
                             <TabStyle Paddings-PaddingLeft="10px" Paddings-PaddingRight="10px"  />                                                                                      
                             <ContentStyle>
                                 <Paddings PaddingLeft="20px" PaddingRight="20px" PaddingTop="5px" />
                             </ContentStyle>
                            <ClientSideEvents ActiveTabChanged="Get_chkfS()" />
                             <TabPages>                                                                            
                                <dx:TabPage Text="Información General">
                                    <ContentCollection>
                                        <dx:ContentControl ID="ContentControl1" runat="server" >
                                            <div style="height: 260px; overflow: auto">
                                                <div style="padding-top:10px">
                                                    <div class="form-group col-sm-12 col-md-6">
                                                        &nbsp;<label id="Label1" runat="server" style="font-size:11px">Nombre o Denominación Razón Social</label>
                                                        <asp:TextBox ID="txt_a_Nombre" runat="server" CssClass="form-control input-sm" MaxLength="250" placeholder="Nombre o Denominación Razón Social" Font-Size="11px"></asp:TextBox>
                                                    </div>
                                                    <div class="form-group col-sm-12 col-md-6">
                                                    &nbsp;<label id="Label3" runat="server" style="font-size:11px">Calle</label>
                                                    <asp:TextBox ID="txt_a_Calle" runat="server" CssClass="form-control input-sm" MaxLength="100" placeholder="Calle" Font-Size="11px"></asp:TextBox>
                                                </div>                                                    
                                                </div>
                                                
                                                <div class="form-group col-sm-12 col-md-2">
                                                        &nbsp;<label id="Label2" runat="server" style="font-size:11px">No. Exterior</label>
                                                        <asp:TextBox ID="txt_a_NoExt" runat="server" CssClass="form-control input-sm" MaxLength="50" placeholder="No. Exterior" Font-Size="11px"></asp:TextBox>
                                                    </div>
                                                <div class="form-group col-sm-12 col-md-2">
                                                    &nbsp;<label id="Label4" runat="server" style="font-size:11px">No. Interior</label>
                                                    <asp:TextBox ID="txt_a_NoInt" runat="server" CssClass="form-control input-sm" MaxLength="5" placeholder="No. Interior" Font-Size="11px"></asp:TextBox>
                                                </div>
                                                <div class="form-group col-sm-12 col-md-2">
                                                    &nbsp;<label id="Label103" runat="server" style="font-size:11px">Ciudad</label>
                                                    <asp:TextBox ID="txt_a_ciudad" runat="server" CssClass="form-control input-sm" MaxLength="50" placeholder="Ciudad" Font-Size="11px"></asp:TextBox>
                                                </div>
                                                <div class="form-group col-sm-12 col-md-2">
                                                    &nbsp;<label id="Label5" runat="server" style="font-size:11px">Código Postal</label>
                                                    <asp:TextBox ID="txt_a_codigo_postal" runat="server" CssClass="form-control input-sm" MaxLength="5" placeholder="Código Postal" Font-Size="11px"></asp:TextBox>
                                                </div>
                                                <div class="form-group col-sm-12 col-md-2">
                                                    &nbsp;<label id="Label6" runat="server" style="font-size:11px">Estado</label>
                                                    <asp:TextBox ID="txt_a_Estado" runat="server" CssClass="form-control input-sm" MaxLength="50" placeholder="Estado" Font-Size="11px"></asp:TextBox>
                                                </div>
                                                <div class="form-group col-sm-12 col-md-2">
                                                    &nbsp;<label id="Label104" runat="server" style="font-size:11px">País</label>
                                                    <asp:TextBox ID="txt_a_pais" runat="server" CssClass="form-control input-sm" MaxLength="50" placeholder="País" Font-Size="11px"></asp:TextBox>
                                                </div>
                                                <div class="form-group col-sm-12 col-md-2">
                                                    &nbsp;<label id="Label105" runat="server" style="font-size:11px">Teléfono</label>
                                                    <asp:TextBox ID="txt_a_telefono" runat="server" CssClass="form-control input-sm" MaxLength="50" placeholder="Teléfono" Font-Size="11px"></asp:TextBox>
                                                </div>
                                                <div class="form-group col-sm-12 col-md-4">
                                                    &nbsp;<label id="Label106" runat="server" style="font-size:11px">Correo</label>
                                                    <asp:TextBox ID="txt_a_correo" runat="server" CssClass="form-control input-sm" MaxLength="50" placeholder="Correo" Font-Size="11px"></asp:TextBox>
                                                </div>
                                            </div> 
                                        </dx:ContentControl>
                                    </ContentCollection>
                                </dx:TabPage>
                                <dx:TabPage Text="Vinculación">
                                    <ContentCollection>
                                        <dx:ContentControl ID="ContentControl2" runat="server">
                                            <div style="height: 260px; overflow: auto">
                                                <div style="padding-top:10px">
                                                    <div class="form-group col-sm-12 col-md-7">
                                                        <label id="Label107" runat="server" style="font-size:11px">1.¿Existe vinculación entre importador y vendedor?</label>                                                    
                                                    </div>
                                                    <div class="form-group col-sm-12 col-md-5">
                                                        <label id="Label108" runat="server" style="font-size:11px">Si</label>
                                                        <dx:ASPxCheckBox ID="chkb1S" ClientInstanceName="chkb1S" Enabled="true" runat="server" >
                                                            <ClientSideEvents CheckedChanged="function(s, e) {Get_chkb1S(s, e); }" />                                                              
                                                        </dx:ASPxCheckBox>
                                                        &nbsp;&nbsp;
                                                        <label id="Label109" runat="server" style="font-size:11px">No</label>
                                                        <dx:ASPxCheckBox ID="chkb1N" ClientInstanceName="chkb1N" Enabled="true" runat="server" >
                                                            <ClientSideEvents CheckedChanged="function(s, e) {Get_chkb1N(s, e); }" />                                                               
                                                        </dx:ASPxCheckBox>   
                                                    </div>
                                                </div>
                                                <div class="form-group col-sm-12 col-md-7">
                                                    <label id="Label110" runat="server" style="font-size:11px">2.¿Influyó en el valor de transacción?</label>
                                                </div>
                                                <div class="form-group col-sm-12 col-md-5">
                                                    <label id="Label111" runat="server" style="font-size:11px">Si</label>
                                                    <dx:ASPxCheckBox ID="chkb2S" ClientInstanceName="chkb2S" Enabled="true" runat="server" >                                                                
                                                        <ClientSideEvents CheckedChanged="function(s, e) {Get_chkb2S(s, e); }" />
                                                    </dx:ASPxCheckBox>
                                                    &nbsp;&nbsp;
                                                    <label id="Label112" runat="server" style="font-size:11px">No</label>
                                                    <dx:ASPxCheckBox ID="chkb2N" ClientInstanceName="chkb2N" Enabled="true" runat="server" >                                                                
                                                        <ClientSideEvents CheckedChanged="function(s, e) {Get_chkb2N(s, e); }" />
                                                    </dx:ASPxCheckBox>
                                                </div>                                                
                                            </div>
                                        </dx:ContentControl>
                                    </ContentCollection>
                                </dx:TabPage>
                                <dx:TabPage Text="Datos del Importador">
                                    <ContentCollection>
                                         <dx:ContentControl ID="ContentControl12" runat="server" >
                                            <div style="height: 260px; overflow: hidden">
                                                <div style="padding-top:10px">
                                                    <div class="form-group col-sm-12 col-md-4">
                                                        &nbsp;<label id="Label7" runat="server" style="font-size:11px">Nombre o Denominación Social</label>
                                                        <asp:TextBox ID="txt_c_nombre_importador" runat="server" CssClass="form-control input-sm" MaxLength="250" placeholder="Nombre o Denominación Social" Font-Size="11px"></asp:TextBox>
                                                    </div>
                                                    <div class="form-group col-sm-12 col-md-4">
                                                        &nbsp;<label id="Label8" runat="server" style="font-size:11px">Apellido Paterno</label>
                                                        <asp:TextBox ID="txt_c_apellidopaternoimportador" runat="server" CssClass="form-control input-sm" MaxLength="100" placeholder="Apellido Paterno" Font-Size="11px"></asp:TextBox>
                                                    </div>
                                                    <div class="form-group col-sm-12 col-md-4">
                                                        &nbsp;<label id="Label75" runat="server" style="font-size:11px">Apellido Materno</label>
                                                        <asp:TextBox ID="txt_c_apellidomaternoimportador" runat="server" CssClass="form-control input-sm" MaxLength="100" placeholder="Apellido Materno" Font-Size="11px"></asp:TextBox>
                                                    </div>                                                                                                     
                                                </div>
                                                <div class="form-group col-sm-12 col-md-4">
                                                        &nbsp;<label id="Label83" runat="server" style="font-size:11px">Nombre</label>
                                                        <asp:TextBox ID="txt_c_nombreimportador" runat="server" CssClass="form-control input-sm" MaxLength="100" placeholder="Nombre" Font-Size="11px"></asp:TextBox>
                                                    </div>
                                                <div class="form-group col-sm-12 col-md-3">
                                                    &nbsp;<label id="Label85" runat="server" style="font-size:11px">RFC</label>
                                                    <asp:TextBox ID="txt_c_rfcimportador" runat="server" CssClass="form-control input-sm" MaxLength="100" placeholder="RFC" Font-Size="11px"></asp:TextBox>
                                                </div>
                                                <div class="form-group col-sm-12 col-md-5">
                                                    &nbsp;<label id="Label84" runat="server" style="font-size:11px">Calle</label>
                                                    <asp:TextBox ID="txt_c_calleimportador" runat="server" CssClass="form-control input-sm" MaxLength="100" placeholder="Calle" Font-Size="11px"></asp:TextBox>
                                                </div>

                                                <div class="form-group col-sm-12 col-md-2">
                                                        &nbsp;<label id="Label9" runat="server" style="font-size:11px">No. Exterior</label>
                                                        <asp:TextBox ID="txt_c_numeroexteriorimportador" runat="server" CssClass="form-control input-sm" MaxLength="50" placeholder="No. Exterior" Font-Size="11px"></asp:TextBox>
                                                    </div>
                                                <div class="form-group col-sm-12 col-md-2">
                                                    &nbsp;<label id="Label10" runat="server" style="font-size:11px">No. Interior</label>
                                                    <asp:TextBox ID="txt_c_numerointeriorimportador" runat="server" CssClass="form-control input-sm" MaxLength="5" placeholder="No. Interior" Font-Size="11px"></asp:TextBox>
                                                </div>
                                                <div class="form-group col-sm-12 col-md-2">
                                                    &nbsp;<label id="Label11" runat="server" style="font-size:11px">Ciudad</label>
                                                    <asp:TextBox ID="txt_c_ciudadimportador" runat="server" CssClass="form-control input-sm" MaxLength="5" placeholder="Ciudad" Font-Size="11px"></asp:TextBox>
                                                </div>
                                                <div class="form-group col-sm-12 col-md-2">
                                                    &nbsp;<label id="Label12" runat="server" style="font-size:11px">Código Postal</label>
                                                    <asp:TextBox ID="txt_c_codigopostalimportador" runat="server" CssClass="form-control input-sm" MaxLength="5" placeholder="Código Postal" Font-Size="11px"></asp:TextBox>
                                                </div>
                                                <div class="form-group col-sm-12 col-md-2">
                                                    &nbsp;<label id="Label71" runat="server" style="font-size:11px">Estado</label>
                                                    <asp:TextBox ID="txt_c_estadoimportador" runat="server" CssClass="form-control input-sm" MaxLength="50" placeholder="Estado" Font-Size="11px"></asp:TextBox>
                                                </div>
                                                <div class="form-group col-sm-12 col-md-2">
                                                    &nbsp;<label id="Label72" runat="server" style="font-size:11px">País</label>
                                                    <asp:TextBox ID="txt_c_paisimportado" runat="server" CssClass="form-control input-sm" MaxLength="50" placeholder="País" Font-Size="11px"></asp:TextBox>
                                                </div>
                                                <div class="form-group col-sm-12 col-md-2">
                                                    &nbsp;<label id="Label73" runat="server" style="font-size:11px">Teléfono</label>
                                                    <asp:TextBox ID="txt_c_telefonoimportador" runat="server" CssClass="form-control input-sm" MaxLength="50" placeholder="Teléfono" Font-Size="11px"></asp:TextBox>
                                                </div>
                                                <div class="form-group col-sm-12 col-md-4">
                                                    &nbsp;<label id="Label74" runat="server" style="font-size:11px">Correo</label>
                                                    <asp:TextBox ID="txt_c_correoimportador" runat="server" CssClass="form-control input-sm" MaxLength="50" placeholder="Correo" Font-Size="11px"></asp:TextBox>
                                                </div>
                                            </div> 
                                        </dx:ContentControl>
                                    </ContentCollection>
                                </dx:TabPage>
                                 <dx:TabPage Text="Agente o Apoderado Aduanal">
                                    <ContentCollection>
                                        <dx:ContentControl ID="ContentControl11" runat="server">
                                            <div style="height: 260px; overflow: auto">
                                                <div style="padding-top:10px">
                                                    <div class="form-group col-sm-12 col-md-4">
                                                        &nbsp;<label id="Label87" runat="server" style="font-size:11px">Apellido Paterno</label>
                                                        <asp:TextBox ID="txt_d_apellidopaternoagente" runat="server" CssClass="form-control input-sm" MaxLength="100" placeholder="Apellido Paterno" Font-Size="11px"></asp:TextBox>
                                                    </div>
                                                    <div class="form-group col-sm-12 col-md-4">
                                                        &nbsp;<label id="Label88" runat="server" style="font-size:11px">Apellido Materno</label>
                                                        <asp:TextBox ID="txt_d_apellidomaternoagente" runat="server" CssClass="form-control input-sm" MaxLength="100" placeholder="Apellido Materno" Font-Size="11px"></asp:TextBox>
                                                    </div>
                                                    <div class="form-group col-sm-12 col-md-4">
                                                        &nbsp;<label id="Label86" runat="server" style="font-size:11px">Nombre</label>
                                                        <asp:TextBox ID="txt_d_nombreagente" runat="server" CssClass="form-control input-sm" MaxLength="100" placeholder="Nombre" Font-Size="11px"></asp:TextBox>
                                                    </div>                                                                                                    
                                                </div>
                                                <div class="form-group col-sm-12 col-md-4">
                                                    &nbsp;<label id="Label89" runat="server" style="font-size:11px">No. Patente u Autorización</label>
                                                    <asp:TextBox ID="txt_d_patenteagente" runat="server" CssClass="form-control input-sm" MaxLength="50" placeholder="No. Patente u Autorización" Font-Size="11px"></asp:TextBox>
                                                </div>
                                            </div>  
                                        </dx:ContentControl>
                                    </ContentCollection>
                                </dx:TabPage>
                                <dx:TabPage Text="Datos Facturas">
                                    <ContentCollection>
                                        <dx:ContentControl ID="ContentControl3" runat="server">
                                            <div style="height: 260px; overflow: auto">
                                                
                                                <asp:LinkButton ID="lkb_Factura_Agregar" runat="server" OnClick="lkb_Factura_Agregar_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button">
                                                    <span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp;Agregar
                                                </asp:LinkButton>
                                                <asp:LinkButton ID="lkb_Factura_Editar" runat="server" OnClick="lkb_Factura_Editar_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button disabled">
                                                    <span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp;Editar
                                                </asp:LinkButton>
                                                <asp:LinkButton ID="lkb_Factura_Eliminar" runat="server" OnClick="lkb_Factura_Eliminar_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button disabled">
                                                    <span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp;Eliminar
                                                </asp:LinkButton>
                                                
                                                <dx:ASPxGridView ID="Grid2_MV" runat="server" EnableTheming="True" Theme="SoftOrange" OnCustomCallback="Grid2_MV_CustomCallback" 
                                                    EnableCallBacks="false" ClientInstanceName="grid2" AutoGenerateColumns="False" Width="410px" Settings-HorizontalScrollBarMode="Auto" 
                                                    KeyFieldName="mvkeyfacura" SettingsPager-Position="TopAndBottom" Styles-Header-ForeColor="#FFF" Styles-Header-Font-Size="11px" 
                                                    Styles-Cell-Font-Size="11px" >
                                                    <SettingsAdaptivity AdaptivityMode="HideDataCellsWindowLimit" HideDataCellsAtWindowInnerWidth="800" 
                                                        AdaptiveDetailColumnCount="1" AllowOnlyOneAdaptiveDetailExpanded="True">
                                                        <AdaptiveDetailLayoutProperties colcount="1">
                                                            <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit"  SwitchToSingleColumnAtWindowInnerWidth="600" />
                                                        </AdaptiveDetailLayoutProperties>
                                                    </SettingsAdaptivity> 
                                                    <SettingsResizing ColumnResizeMode="Control" />
                                                     <SettingsBehavior AllowSelectByRowClick="false" AllowSelectSingleRowOnly="true" />
                                                    <Settings ShowFooter="False" ShowHeaderFilterButton="true" ShowFilterRowMenu="false"  />
                                                    <Styles>                                                                                      
                                                        <SelectedRow CssClass="background_color_btn background_texto_btn" />
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
                                                        <dx:GridViewDataTextColumn FieldName="mvkeyfacura" ReadOnly="True" Visible="false">
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="mvkeymanifestacion" ReadOnly="True"  Visible="false" >
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataColumn Caption="Seleccionar" VisibleIndex="0" Width="70px">
                                                           <HeaderStyle HorizontalAlign="Center" ForeColor="White" />
                                                           <CellStyle HorizontalAlign="Center"></CellStyle>
                                                           <DataItemTemplate>
                                                               <dx:ASPxCheckBox ID="chkConsultar" ClientInstanceName="chkConsultar" Enabled="true" runat="server" OnInit="chkConsultarF_Init">
                                                               </dx:ASPxCheckBox>
                                                           </DataItemTemplate>
                                                        </dx:GridViewDataColumn>
                                                        <dx:GridViewDataTextColumn Caption="Factura" FieldName="Factura" ReadOnly="True" VisibleIndex="1" Width="142px" >
                                                            <HeaderStyle HorizontalAlign="Center"   />
                                                            <SettingsHeaderFilter Mode="CheckedList" />
                                                            <CellStyle HorizontalAlign="Center"></CellStyle>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataDateColumn Caption="Fecha Factura" FieldName="FechaFactura" ReadOnly="True" VisibleIndex="4" Width="170px">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <SettingsHeaderFilter Mode="CheckedList" />
                                                            <CellStyle HorizontalAlign="Center" />
                                                        </dx:GridViewDataDateColumn>
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
                                        </dx:ContentControl>
                                    </ContentCollection>
                                </dx:TabPage>                                
                                <dx:TabPage Text="Método de Valoración">
                                    <ContentCollection>
                                        <dx:ContentControl ID="ContentControl4" runat="server">
                                            <div style="height: 260px; overflow: auto">
                                                <div style="padding-top:10px">
                                                    <div class="form-group col-sm-12 col-md-5">
                                                        <label id="Label20" runat="server" style="font-size:11px">¿Se utilizó un método de valoración?</label>                                                    
                                                    </div>
                                                    <div class="form-group col-sm-12 col-md-1">
                                                        <dx:ASPxCheckBox ID="chkf1S" ClientInstanceName="chkf1S" Enabled="true" runat="server" >
                                                            <ClientSideEvents CheckedChanged="function(s, e) {Get_chkf1S(s, e); }" />                                                              
                                                        </dx:ASPxCheckBox>                                                        
                                                    </div>
                                                    <div class="form-group col-sm-12 col-md-5">
                                                        <label id="Label21" runat="server" style="font-size:11px">¿Se utilizó más de un método de valoración?</label>                                                    
                                                    </div>
                                                    <div class="form-group col-sm-12 col-md-1">
                                                        <dx:ASPxCheckBox ID="chkf2S" ClientInstanceName="chkf2S" Enabled="true" runat="server" >
                                                            <ClientSideEvents CheckedChanged="function(s, e) {Get_chkf2S(s, e); }" />                                                              
                                                        </dx:ASPxCheckBox>                                                        
                                                    </div>
                                                </div>
                                                <div class="form-group col-sm-12 col-md-5">
                                                    <label id="Label23" runat="server" style="font-size:11px">Valor de transacción de las mercancías</label>
                                                </div>
                                                <div class="form-group col-sm-12 col-md-1">
                                                    <dx:ASPxCheckBox ID="chkf3S" ClientInstanceName="chkf3S" Enabled="true" runat="server" >                                                                
                                                        <ClientSideEvents CheckedChanged="function(s, e) {Get_chkf3S(s, e); }" />
                                                    </dx:ASPxCheckBox>                                                
                                                </div>
                                                <div class="form-group col-sm-12 col-md-6">
                                                    <dx:ASPxMemo ID="txt_f_descripciondetransaccionmercancias" runat="server" ClientInstanceName="txt_f_descripciondetransaccionmercancias" CssClass="form-control input-sm" Native="true" MaxLength="250" placeholder="" Font-Size="11px" Height="60px" ></dx:ASPxMemo>
                                                </div>
                                                <div class="form-group col-sm-12 col-md-5">
                                                    <label id="Label22" runat="server" style="font-size:11px">Valor de transacción de las mercancías idénticas</label>
                                                </div>
                                                <div class="form-group col-sm-12 col-md-1">
                                                    <dx:ASPxCheckBox ID="chkf4S" ClientInstanceName="chkf4S" Enabled="true" runat="server" >                                                                
                                                        <ClientSideEvents CheckedChanged="function(s, e) {Get_chkf4S(s, e); }" />
                                                    </dx:ASPxCheckBox>                                                
                                                </div>
                                                <div class="form-group col-sm-12 col-md-6">
                                                    <dx:ASPxMemo ID="txt_f_descripciondetransaccionmercanciasidenticas" runat="server" ClientInstanceName="txt_f_descripciondetransaccionmercanciasidenticas" CssClass="form-control input-sm" Native="true" MaxLength="250" placeholder="" Font-Size="11px" Height="60px"></dx:ASPxMemo>
                                                </div>
                                                <div class="form-group col-sm-12 col-md-5">
                                                    <label id="Label24" runat="server" style="font-size:11px">Valor de transacción de las similares</label>
                                                </div>
                                                <div class="form-group col-sm-12 col-md-1">
                                                    <dx:ASPxCheckBox ID="chkf5S" ClientInstanceName="chkf5S" Enabled="true" runat="server" >                                                                
                                                        <ClientSideEvents CheckedChanged="function(s, e) {Get_chkf5S(s, e); }" />
                                                    </dx:ASPxCheckBox>                                                
                                                </div>
                                                <div class="form-group col-sm-12 col-md-6">
                                                    <dx:ASPxMemo ID="txt_f_descripciondetransaccionmercanciassimilares" runat="server" ClientInstanceName="txt_f_descripciondetransaccionmercanciassimilares" CssClass="form-control input-sm" Native="true" MaxLength="250" placeholder="" Font-Size="11px" Height="60px"></dx:ASPxMemo>
                                                </div>
                                                <div class="form-group col-sm-12 col-md-5">
                                                    <label id="Label25" runat="server" style="font-size:11px">Valor de precio unitaro de venta</label>
                                                </div>
                                                <div class="form-group col-sm-12 col-md-1">
                                                    <dx:ASPxCheckBox ID="chkf6S" ClientInstanceName="chkf6S" Enabled="true" runat="server" >                                                                
                                                        <ClientSideEvents CheckedChanged="function(s, e) {Get_chkf6S(s, e); }" />
                                                    </dx:ASPxCheckBox>                                                
                                                </div>
                                                <div class="form-group col-sm-12 col-md-6">
                                                    <dx:ASPxMemo ID="txt_f_descripcionvalordepreciounitariodeventa" runat="server" ClientInstanceName="txt_f_descripcionvalordepreciounitariodeventa" CssClass="form-control input-sm" Native="true" MaxLength="250" placeholder="" Font-Size="11px" Height="60px"></dx:ASPxMemo>
                                                </div>
                                                <div class="form-group col-sm-12 col-md-5">
                                                    <label id="Label26" runat="server" style="font-size:11px">Valor reconstruido</label>
                                                </div>
                                                <div class="form-group col-sm-12 col-md-1">
                                                    <dx:ASPxCheckBox ID="chkf7S" ClientInstanceName="chkf7S" Enabled="true" runat="server" >                                                                
                                                        <ClientSideEvents CheckedChanged="function(s, e) {Get_chkf7S(s, e); }" />
                                                    </dx:ASPxCheckBox>                                                
                                                </div>
                                                <div class="form-group col-sm-12 col-md-6">
                                                    <dx:ASPxMemo ID="txt_f_descripcionvalorreconstruido" runat="server" ClientInstanceName="txt_f_descripcionvalorreconstruido" CssClass="form-control input-sm" Native="true" MaxLength="250" placeholder="" Font-Size="11px" Height="60px"></dx:ASPxMemo>
                                                </div>
                                                <div class="form-group col-sm-12 col-md-5">
                                                    <label id="Label27" runat="server" style="font-size:11px">Valor conforme al artículo 78 de la Ley</label>
                                                </div>
                                                <div class="form-group col-sm-12 col-md-1">
                                                    <dx:ASPxCheckBox ID="chkf8S" ClientInstanceName="chkf8S" Enabled="true" runat="server" >                                                                
                                                        <ClientSideEvents CheckedChanged="function(s, e) {Get_chkf8S(s, e); }" />
                                                    </dx:ASPxCheckBox>                                                
                                                </div>
                                                <div class="form-group col-sm-12 col-md-6">
                                                    <dx:ASPxMemo ID="txt_f_descripcionvalorconformaley" runat="server" ClientInstanceName="txt_f_descripcionvalorconformaley" CssClass="form-control input-sm" Native="true" MaxLength="250" placeholder="" Font-Size="11px" Height="60px"></dx:ASPxMemo>
                                                </div>
                                            </div>
                                        </dx:ContentControl>
                                    </ContentCollection>
                                </dx:TabPage>
                                <dx:TabPage Text="Anexos">
                                    <ContentCollection>
                                        <dx:ContentControl ID="ContentControl6" runat="server">
                                            <div style="height: 260px; overflow: auto">
                                                <div style="height: 240px; overflow: auto">
                                                    <div style="padding-top:10px">
                                                        <div class="form-group col-sm-12 col-md-12">
                                                            <label id="Label28" runat="server" style="font-size:11px">Señale la opción en caso de presentar anexos, los cuales deberá numerarlos y foliarlos, señalando el número total de hojas anexas con númeo y letra</label>                                                    
                                                        </div>
                                                        <div class="form-group col-sm-12 col-md-1">
                                                            <dx:ASPxCheckBox ID="chkg1S" ClientInstanceName="chkg1S" Enabled="true" runat="server" >
                                                                <ClientSideEvents CheckedChanged="function(s, e) {Get_chkg1S(s, e); }" />                                                              
                                                            </dx:ASPxCheckBox>                                                        
                                                        </div>
                                                        <div class="form-group col-sm-12 col-md-11"></div>
                                                    </div>
                                                    <div class="form-group col-sm-12 col-md-12">
                                                        <label id="Label29" runat="server" style="font-size:11px">Escriba con número y letra:</label>                                                    
                                                    </div>
                                                    <div class="form-group col-sm-12 col-md-12">
                                                        <dx:ASPxMemo ID="txt_g_anexadocumentacionnumeroyletra" runat="server" ClientInstanceName="txt_g_anexadocumentacionnumeroyletra" CssClass="form-control input-sm" Native="true" MaxLength="100" placeholder="" Font-Size="11px" Height="40px"></dx:ASPxMemo>
                                                    </div>
                                                    <%--<div class="form-group col-sm-12 col-md-12">
                                                                                                            
                                                    </div>
                                                    <div class="form-group col-sm-12 col-md-12">
                                                        
                                                    </div>--%>
                                                </div>
                                            </div>
                                        </dx:ContentControl>
                                    </ContentCollection>
                                </dx:TabPage>
                                <dx:TabPage Text="Valor de Transacción de las Mercancias">
                                   <ContentCollection>
                                    <dx:ContentControl ID="ContentControl7" runat="server">
                                           <div id="divVTM" runat="server" style="height: 260px; overflow: auto">
                                               <div style="padding-top:10px">
                                                   <div class="form-group col-sm-12 col-md-12">
                                                       <label id="Label40" runat="server" style="font-size:11px">a) Precio pagado en moneda de facturación, con número y letra</label>                                                    
                                                   </div>
                                                   <div class="form-group col-sm-12 col-md-3">
                                                       <dx:ASPxSpinEdit ID="se_ga_preciofacturanumero" ClientInstanceName="se_ga_preciofacturanumero" runat="server" NullText="" Width="100%" NumberType="Float" DecimalPlaces="4" DisplayFormatString="N4" 
                                                          MinValue="0" Number="0" ValidateRequestMode="Disabled" ShowOutOfRangeWarning="false" SpinButtons-ShowIncrementButtons="false">
                                                          <ClearButton DisplayMode="OnHover"></ClearButton>
                                                          <ClientSideEvents LostFocus = "function(s,e) { Valor_VTM(s); }" />
                                                       </dx:ASPxSpinEdit>
                                                       <%--<dx:ASPxSpinEdit ID="se_ga_preciofacturanumero" ClientInstanceName="se_ga_preciofacturanumero" runat="server" NullText="" Width="100%" NumberType="Float" DecimalPlaces="4" DisplayFormatString="N4" 
                                                          MinValue="0" Number="0" ValidateRequestMode="Disabled" ShowOutOfRangeWarning="false" SpinButtons-ShowIncrementButtons="false">
                                                          <ClearButton DisplayMode="OnHover"></ClearButton>
                                                          <ClientSideEvents LostFocus = "function(s,e) { Valor_VTM(s); }" />
                                                       </dx:ASPxSpinEdit>--%>
                                                   </div>
                                                   <div class="form-group col-sm-12 col-md-7">
                                                       <dx:ASPxTextBox ID="txt_ga_preciofacturaletra" runat="server" ClientInstanceName="txt_ga_preciofacturaletra" CssClass="form-control input-sm" MaxLength="250" NullText="Escriba el número con letra" Font-Size="11px" Width="100%">
                                                       </dx:ASPxTextBox>                                                       
                                                       <%--<dx:ASPxTextBox ID="txt_ga_preciofacturaletra" runat="server" ClientInstanceName="txt_ga_preciofacturaletra" CssClass="form-control input-sm" MaxLength="250" NullText="Escriba el número con letra" Font-Size="11px" Width="100%">
                                                           <ClientSideEvents TextChanged="function(s,e) { ValorTransaccionMercancias(); }" />
                                                       </dx:ASPxTextBox>--%>
                                                   </div>
                                                   <div class="form-group col-sm-12 col-md-2">
                                                       <dx:ASPxTextBox ID="txt_g_anexadocumentacionmodena" runat="server" ClientInstanceName="txt_g_anexadocumentacionmodena" CssClass="form-control input-sm" MaxLength="5" NullText="Moneda" Font-Size="11px" Width="100%">
                                                       </dx:ASPxTextBox>                                                       
                                                       <%--<dx:ASPxTextBox ID="txt_g_anexadocumentacionmodena" runat="server" ClientInstanceName="txt_g_anexadocumentacionmodena" CssClass="form-control input-sm" MaxLength="5" NullText="Moneda" Font-Size="11px" Width="100%">
                                                           <ClientSideEvents TextChanged="function(s,e) { ValorTransaccionMercancias(); }" />
                                                       </dx:ASPxTextBox>--%>
                                                   </div>
                                               </div>
                                               <div class="form-group col-sm-12 col-md-12">
                                                   <label id="Label41" runat="server" style="font-size:11px">b) Información conforme al artículo 66 de la Ley (conceptos que no integran el valor de transacción)</label>
                                               </div>
                                               <div class="form-group col-sm-12 col-md-12">
                                                   <label id="Label30" runat="server" style="font-size:11px">Señale los siguientes conceptos que se ajusten a su caso particular</label>
                                               </div>
                                               <div class="form-group col-sm-12 col-md-12">
                                                    <div class="form-group col-sm-12 col-md-1">
                                                        <dx:ASPxCheckBox ID="chkvtm_b1S" ClientInstanceName="chkvtm_b1S" Enabled="true" runat="server" >
                                                            <ClientSideEvents CheckedChanged="function(s, e) {Get_chkvtm_b1S(s, e); }" />
                                                         </dx:ASPxCheckBox>
                                                    </div>
                                                    <div class="form-group col-sm-12 col-md-11">
                                                        <label id="Label31" runat="server" style="font-size:11px">Es el precio previsto en la factura.</label>
                                                    </div>
                                               </div>
                                               <div class="form-group col-sm-12 col-md-12">
                                                    <div class="form-group col-sm-12 col-md-1">
                                                        <dx:ASPxCheckBox ID="chkvtm_b2S" ClientInstanceName="chkvtm_b2S" Enabled="true" runat="server" >
                                                            <ClientSideEvents CheckedChanged="function(s, e) {Get_chkvtm_b2S(s, e); }" />
                                                         </dx:ASPxCheckBox>
                                                    </div>
                                                    <div class="form-group col-sm-12 col-md-11">
                                                        <label id="Label32" runat="server" style="font-size:11px">Es el precio de otros documentos que se anexan a la manifestación.</label>
                                                    </div>
                                               </div>
                                               <div class="form-group col-sm-12 col-md-12">
                                                    <div class="form-group col-sm-12 col-md-1">
                                                        <dx:ASPxCheckBox ID="chkvtm_b3S" ClientInstanceName="chkvtm_b3S" Enabled="true" runat="server" >
                                                            <ClientSideEvents CheckedChanged="function(s, e) {Get_chkvtm_b3S(s, e); }" />
                                                         </dx:ASPxCheckBox>
                                                    </div>
                                                    <div class="form-group col-sm-12 col-md-11">
                                                        <label id="Label33" runat="server" style="font-size:11px">Si existen los conceptos señalados en el artículo 66 de la Ley (conceptos que no integran el valor de transacción).</label>
                                                    </div>
                                               </div>
                                               <div class="form-group col-sm-12 col-md-12">
                                                    <div class="form-group col-sm-12 col-md-1">
                                                        <dx:ASPxCheckBox ID="chkvtm_b4S" ClientInstanceName="chkvtm_b4S" Enabled="true" runat="server" >
                                                            <ClientSideEvents CheckedChanged="function(s, e) {Get_chkvtm_b4S(s, e); }" />
                                                         </dx:ASPxCheckBox>
                                                    </div>
                                                    <div class="form-group col-sm-12 col-md-11">
                                                        <label id="Label34" runat="server" style="font-size:11px">Los conceptos del artículo 66 de la Ley aparecen desglosados o especificados en la factura comercial.</label>
                                                    </div>
                                               </div>
                                               <div class="form-group col-sm-12 col-md-12">
                                                   <label id="Label35" runat="server" style="font-size:11px">c)Indicar en caso de ANEXAR documentación a la manifestación de valor.</label>
                                               </div>
                                               <div class="form-group col-sm-12 col-md-12">
                                                   <label id="Label36" runat="server" style="font-size:11px">Nota: Sólo se relacionarán los documentos que se anexen, correspondientes a los conceptos previstos en el artículo 66 de la Ley.</label>                                                   
                                               </div>
                                               <div class="form-group col-sm-12 col-md-12">
                                                   <asp:LinkButton ID="lkb_Anexo_Agregar" runat="server" OnClick="lkb_Anexo_Agregar_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button" >
                                                       <span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp;Agregar
                                                   </asp:LinkButton>
                                                   <asp:LinkButton ID="lkb_Anexo_Editar" runat="server" OnClick="lkb_Anexo_Editar_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button disabled" >
                                                       <span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp;Editar
                                                   </asp:LinkButton>
                                                   <asp:LinkButton ID="lkb_Anexo_Eliminar" runat="server" OnClick="lkb_Anexo_Eliminar_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button disabled" >
                                                       <span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp;Eliminar
                                                   </asp:LinkButton> 
                                                   <dx:ASPxGridView ID="Grid3_MV" runat="server" EnableTheming="True" Theme="SoftOrange" OnCustomCallback="Grid3_MV_CustomCallback"
                                                       EnableCallBacks="false" ClientInstanceName="grid3" AutoGenerateColumns="False" Width="100%" 
                                                       Settings-HorizontalScrollBarMode="Auto" KeyFieldName="mvkeyanexomanifestacion" SettingsPager-Position="TopAndBottom"
                                                       Styles-Header-ForeColor="#FFF" Styles-Header-Font-Size="11px" Styles-Cell-Font-Size="11px" >
                                                       <SettingsAdaptivity AdaptivityMode="HideDataCellsWindowLimit" HideDataCellsAtWindowInnerWidth="800" 
                                                        AdaptiveDetailColumnCount="1" AllowOnlyOneAdaptiveDetailExpanded="True">
                                                        <AdaptiveDetailLayoutProperties colcount="1">
                                                            <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit"  SwitchToSingleColumnAtWindowInnerWidth="600" />
                                                        </AdaptiveDetailLayoutProperties>
                                                        </SettingsAdaptivity> 
                                                        <SettingsResizing ColumnResizeMode="Control" />
                                                        <SettingsBehavior AllowSelectByRowClick="false" AllowSelectSingleRowOnly="true" />
                                                        <Settings ShowFooter="False" ShowHeaderFilterButton="true" ShowFilterRowMenu="false"  />
                                                        <Styles>                                                                                      
                                                            <SelectedRow CssClass="background_color_btn background_texto_btn" />
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
                                                           <dx:GridViewDataTextColumn FieldName="mvkeyanexomanifestacion" ReadOnly="True"  Visible="false" >
                                                           </dx:GridViewDataTextColumn>
                                                           <dx:GridViewDataColumn Caption="Seleccionar" VisibleIndex="0" Width="70px">
                                                               <HeaderStyle HorizontalAlign="Center" ForeColor="White" />
                                                               <CellStyle HorizontalAlign="Center"></CellStyle>
                                                               <DataItemTemplate>
                                                                   <dx:ASPxCheckBox ID="chkConsultarTM" ClientInstanceName="chkConsultar" Enabled="true" runat="server" OnInit="chkConsultarTM_Init">                                                                
                                                                   </dx:ASPxCheckBox>
                                                               </DataItemTemplate>
                                                            </dx:GridViewDataColumn>
                                                           <dx:GridViewDataDateColumn Caption="Numerar anexos y <br/>relacionarlos" FieldName="ordenanexo" ReadOnly="True" VisibleIndex="3" Width="150px">
                                                               <HeaderStyle HorizontalAlign="Center" />
                                                               <SettingsHeaderFilter Mode="CheckedList" />
                                                               <CellStyle HorizontalAlign="Center" />
                                                           </dx:GridViewDataDateColumn>
                                                           <dx:GridViewDataTextColumn Caption="Conceptos previstos en el artículo 66 de la Ley. Anote cada factura o documento comercial <br/> que anexa de acuerdo al número asignado" FieldName="Conceptoanexo" ReadOnly="True" VisibleIndex="4" Width="497px" >
                                                               <HeaderStyle HorizontalAlign="Center"   />
                                                               <SettingsHeaderFilter Mode="CheckedList" />
                                                               <CellStyle HorizontalAlign="Center"></CellStyle>
                                                           </dx:GridViewDataTextColumn>
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
                                               <div class="form-group col-sm-12 col-md-12">
                                                    <div class="form-group col-sm-12 col-md-1">
                                                        <dx:ASPxCheckBox ID="chkvtm_c1S" ClientInstanceName="chkvtm_c1S" Enabled="true" runat="server" >
                                                            <ClientSideEvents CheckedChanged="function(s, e) {Get_chkvtm_c1S(s, e); }" />
                                                         </dx:ASPxCheckBox>
                                                    </div>
                                                    <div class="form-group col-sm-12 col-md-11">
                                                        <label id="Label37" runat="server" style="font-size:11px">d) Indique en caso de NO ANEXAR documentación y sólo describirán los conceptos previstos en el artículo 66 de la Ley.</label>
                                                    </div>
                                               </div>
                                               <div class="form-group col-sm-12 col-md-12">
                                                   <label id="Label38" runat="server" style="font-size:11px">Describa la mercancía, los conceptos señalados en el artículo 66 de la Ley y el precio pagado respecto de cada uno, es decir, los conceptos que no integran el valor de transacción. Sólo cuando estos no aparezcan desglosados o especificados en la factura o documentación comercial.</label>
                                               </div>
                                               <div class="form-group col-sm-12 col-md-12">
                                                   <asp:LinkButton ID="lkb_ManifestacionConcepto66_Agregar" runat="server" OnClick="lkb_ManifestacionConcepto66_Agregar_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button" >
                                                       <span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp;Agregar
                                                   </asp:LinkButton>
                                                   <asp:LinkButton ID="lkb_ManifestacionConcepto66_Editar" runat="server" OnClick="lkb_ManifestacionConcepto66_Editar_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button disabled" >
                                                       <span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp;Editar
                                                   </asp:LinkButton>
                                                   <asp:LinkButton ID="lkb_ManifestacionConcepto66_Eliminar" runat="server" OnClick="lkb_ManifestacionConcepto66_Eliminar_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button disabled" >
                                                       <span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp;Eliminar
                                                   </asp:LinkButton>
                                                   <dx:ASPxGridView ID="Grid4_MV" runat="server" EnableTheming="True" Theme="SoftOrange" OnCustomCallback="Grid4_MV_CustomCallback"
                                                       EnableCallBacks="false" ClientInstanceName="grid4" AutoGenerateColumns="False" Width="100%" 
                                                       Settings-HorizontalScrollBarMode="Auto" KeyFieldName="mvkeyconcepto66" SettingsPager-Position="TopAndBottom"
                                                       Styles-Header-ForeColor="#FFF" Styles-Header-Font-Size="11px" Styles-Cell-Font-Size="11px" >
                                                       <SettingsAdaptivity AdaptivityMode="HideDataCellsWindowLimit" HideDataCellsAtWindowInnerWidth="800" 
                                                           AdaptiveDetailColumnCount="1" AllowOnlyOneAdaptiveDetailExpanded="True">
                                                           <AdaptiveDetailLayoutProperties colcount="2">
                                                               <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit"  SwitchToSingleColumnAtWindowInnerWidth="600" />
                                                           </AdaptiveDetailLayoutProperties>
                                                       </SettingsAdaptivity> 
                                                       <SettingsResizing ColumnResizeMode="Control" />
                                                       <SettingsBehavior AllowSelectByRowClick="false" AllowSelectSingleRowOnly="true" />
                                                       <Settings ShowFooter="False" ShowHeaderFilterButton="true" ShowFilterRowMenu="false"  />                                         
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
                                                           <dx:GridViewDataTextColumn FieldName="mvkeyconcepto66" ReadOnly="True"  Visible="false" >
                                                           </dx:GridViewDataTextColumn>
                                                           <dx:GridViewDataColumn Caption="Seleccionar" VisibleIndex="0" Width="70px">
                                                               <HeaderStyle HorizontalAlign="Center" ForeColor="White" />
                                                               <CellStyle HorizontalAlign="Center"></CellStyle>
                                                               <DataItemTemplate>
                                                                   <dx:ASPxCheckBox ID="chkConsultarTM2" ClientInstanceName="chkConsultar" Enabled="true" runat="server" OnInit="chkConsultarTM2_Init">                                                                
                                                                   </dx:ASPxCheckBox>
                                                               </DataItemTemplate>
                                                            </dx:GridViewDataColumn>
                                                           <dx:GridViewDataDateColumn Caption="No." FieldName="ordenconcepto" ReadOnly="True" Width="50px">
                                                               <HeaderStyle HorizontalAlign="Center" />
                                                               <SettingsHeaderFilter Mode="CheckedList" />
                                                               <CellStyle HorizontalAlign="Center" />
                                                           </dx:GridViewDataDateColumn>
                                                           <dx:GridViewBandColumn Caption="Conceptos previstos en el artículo 66 de la Ley" HeaderStyle-ForeColor="White">
                                                               <Columns>
                                                                   <dx:GridViewDataTextColumn Caption="Mercancía" FieldName="mercancia" ReadOnly="True" Width="150px">
                                                                       <HeaderStyle HorizontalAlign="Center" />
                                                                       <SettingsHeaderFilter Mode="CheckedList" />
                                                                       <CellStyle HorizontalAlign="Center"></CellStyle>
                                                                   </dx:GridViewDataTextColumn>
                                                                   <dx:GridViewDataTextColumn Caption="Factura o documentos <br/> comerciales" FieldName="factura" ReadOnly="True" Width="150px">
                                                                       <HeaderStyle HorizontalAlign="Center" />
                                                                       <SettingsHeaderFilter Mode="CheckedList" />
                                                                       <CellStyle HorizontalAlign="Center"></CellStyle>
                                                                   </dx:GridViewDataTextColumn>
                                                                   <dx:GridViewDataTextColumn Caption="Importe y moneda <br/>de facturación" FieldName="importeconcepto" ReadOnly="True" Width="150px">
                                                                       <HeaderStyle HorizontalAlign="Center" />
                                                                       <SettingsHeaderFilter Mode="CheckedList" />
                                                                       <CellStyle HorizontalAlign="Center"></CellStyle>
                                                                   </dx:GridViewDataTextColumn>
                                                                   <dx:GridViewDataTextColumn Caption="Concepto del cargo" FieldName="conceptodelcargo" ReadOnly="True" Width="150px">
                                                                       <HeaderStyle HorizontalAlign="Center" />
                                                                       <SettingsHeaderFilter Mode="CheckedList" />
                                                                       <CellStyle HorizontalAlign="Center"></CellStyle>
                                                                   </dx:GridViewDataTextColumn>
                                                               </Columns>
                                                           </dx:GridViewBandColumn>
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
                                               <div class="form-group col-sm-12 col-md-12">
                                                   <label id="Label39" runat="server" style="font-size:11px">Nota:Puede optar por no rellenar en rubro de "conceptos del cargo" si estos aparecen desglosados o especificados en la factura, en caso de que no aparezcan desglosados deben ser descritos.</label>
                                               </div>                                               
                                           </div>
                                       </dx:ContentControl>
                                   </ContentCollection>
                                </dx:TabPage>
                                <dx:TabPage Text="Artículo 65 de la Ley">
                                   <ContentCollection>
                                       <dx:ContentControl ID="ContentControl8" runat="server">
                                           <div style="height: 260px; overflow: auto">
                                               <div style="padding-top:10px">
                                                    <div class="form-group col-sm-12 col-md-12">
                                                        <label id="Label42" runat="server" style="font-size:11px">Información conforme al artículo 65 de la Ley (conceptos que integran el valor de transacción).</label>
                                                    </div>  
                                               </div>
                                               <div class="form-group col-sm-12 col-md-12">
                                                   <label id="Label43" runat="server" style="font-size:11px">El importador debe señalar si existen cargos conforme al artículo 65 de la Ley. Señale si el precio pagado por las mercancías importadas comprende el importe de los conceptos señalados en el artículo 65 de la Ley.</label>
                                               </div>
                                               <div class="form-group col-sm-12 col-md-12">
                                                   <div class="form-group col-sm-12 col-md-2">
                                                        <dx:ASPxCheckBox ID="chk65_1S" ClientInstanceName="chk65_1S" Enabled="true" runat="server" >
                                                            <ClientSideEvents CheckedChanged="function(s, e) {Get_chk65_1S(s, e); }" />                                                              
                                                        </dx:ASPxCheckBox>
                                                        &nbsp;&nbsp;
                                                        <label id="Label44" runat="server" style="font-size:11px">Si</label>
                                                   </div>
                                                   <div class="form-group col-sm-12 col-md-2">
                                                        <dx:ASPxCheckBox ID="chk65_1N" ClientInstanceName="chk65_1N" Enabled="true" runat="server" >
                                                            <ClientSideEvents CheckedChanged="function(s, e) {Get_chk65_1N(s, e); }" />                                                              
                                                        </dx:ASPxCheckBox>
                                                        &nbsp;&nbsp;
                                                        <label id="Label45" runat="server" style="font-size:11px">No</label>
                                                   </div>
                                                   <div class="form-group col-sm-12 col-md-8"></div>
                                               </div>
                                               <div class="form-group col-sm-12 col-md-12">
                                                    <label id="Label46" runat="server" style="font-size:11px">En su caso, señale si el importador opta por acompañar o NO las facturas y otros documentos a su manifestación de valor.</label>
                                               </div>
                                               <div class="form-group col-sm-12 col-md-12">
                                                   <div class="form-group col-sm-12 col-md-2">
                                                        <dx:ASPxCheckBox ID="chk65_2S" ClientInstanceName="chk65_2S" Enabled="true" runat="server" >
                                                            <ClientSideEvents CheckedChanged="function(s, e) {Get_chk65_2S(s, e); }" />                                                              
                                                        </dx:ASPxCheckBox>
                                                        &nbsp;&nbsp;
                                                        <label id="Label47" runat="server" style="font-size:11px">Si</label>
                                                   </div>                                                   
                                                   <div class="form-group col-sm-12 col-md-2">
                                                        <dx:ASPxCheckBox ID="chk65_2N" ClientInstanceName="chk65_2N" Enabled="true" runat="server" >
                                                            <ClientSideEvents CheckedChanged="function(s, e) {Get_chk65_2N(s, e); }" />                                                              
                                                        </dx:ASPxCheckBox>
                                                        &nbsp;&nbsp;
                                                        <label id="Label48" runat="server" style="font-size:11px">No</label>
                                                   </div>
                                                   <div class="form-group col-sm-12 col-md-8"></div>
                                               </div>
                                               <div class="form-group col-sm-12 col-md-12">
                                                   <div class="form-group col-sm-12 col-md-12">
                                                        <dx:ASPxCheckBox ID="chk65_3S" ClientInstanceName="chk65_3S" Enabled="true" runat="server" >
                                                            <ClientSideEvents CheckedChanged="function(s, e) {Get_chk65_3S(s, e); }" />  
                                                        </dx:ASPxCheckBox>
                                                        &nbsp;&nbsp;                                                    
                                                        <label id="Label49" runat="server" style="font-size:11px">Indicar si ANEXA documentación</label>
                                                   </div>                                                    
                                               </div>
                                               <div class="form-group col-sm-12 col-md-12">
                                                    <label id="Label68" runat="server" style="font-size:11px">En caso de anexar documentación, señale lo siguiente:</label>
                                               </div>
                                               <div class="form-group col-sm-12 col-md-12">
                                                   <asp:LinkButton ID="lkb_OrdenAnexo65_Agregar" runat="server" OnClick="lkb_OrdenAnexo65_Agregar_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button" >
                                                       <span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp;Agregar
                                                   </asp:LinkButton>
                                                   <asp:LinkButton ID="lkb_OrdenAnexo65_Editar" runat="server" OnClick="lkb_OrdenAnexo65_Editar_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button disabled" >
                                                       <span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp;Editar
                                                   </asp:LinkButton>
                                                   <asp:LinkButton ID="lkb_OrdenAnexo65_Eliminar" runat="server" OnClick="lkb_OrdenAnexo65_Eliminar_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button disabled" >
                                                       <span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp;Eliminar
                                                   </asp:LinkButton>
                                                   <dx:ASPxGridView ID="Grid8_MV" runat="server" EnableTheming="True" Theme="SoftOrange" OnCustomCallback="Grid8_MV_CustomCallback"
                                                       EnableCallBacks="false" ClientInstanceName="grid8" AutoGenerateColumns="False" Width="100%" 
                                                       Settings-HorizontalScrollBarMode="Auto" KeyFieldName="mvkeyanexomanifestacion65" SettingsPager-Position="TopAndBottom"
                                                       Styles-Header-ForeColor="#FFF" Styles-Header-Font-Size="11px" Styles-Cell-Font-Size="11px" >
                                                       <SettingsAdaptivity AdaptivityMode="HideDataCellsWindowLimit" HideDataCellsAtWindowInnerWidth="800" 
                                                           AdaptiveDetailColumnCount="1" AllowOnlyOneAdaptiveDetailExpanded="True">
                                                           <AdaptiveDetailLayoutProperties colcount="2">
                                                               <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit"  SwitchToSingleColumnAtWindowInnerWidth="600" />
                                                           </AdaptiveDetailLayoutProperties>
                                                       </SettingsAdaptivity> 
                                                       <SettingsResizing ColumnResizeMode="Control" />
                                                       <SettingsBehavior AllowSelectByRowClick="false" AllowSelectSingleRowOnly="true" />
                                                       <Settings ShowFooter="False" ShowHeaderFilterButton="true" ShowFilterRowMenu="false"  />                                          
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
                                                           <dx:GridViewDataTextColumn FieldName="mvkeyanexomanifestacion65" ReadOnly="True"  Visible="false" >
                                                           </dx:GridViewDataTextColumn>
                                                           <dx:GridViewDataColumn Caption="Seleccionar" VisibleIndex="0" Width="70px">
                                                               <HeaderStyle HorizontalAlign="Center" ForeColor="White" />
                                                               <CellStyle HorizontalAlign="Center"></CellStyle>
                                                               <DataItemTemplate>
                                                                   <dx:ASPxCheckBox ID="chkConsultarS65" ClientInstanceName="chkConsultar" Enabled="true" runat="server" OnInit="chkConsultarS65_Init">                                                                
                                                                   </dx:ASPxCheckBox>
                                                               </DataItemTemplate>
                                                            </dx:GridViewDataColumn>
                                                           <dx:GridViewDataDateColumn Caption="Numerar anexos y <br/> relacionarlos" FieldName="ordenanexo65" ReadOnly="True" Width="138px">
                                                               <HeaderStyle HorizontalAlign="Center" />
                                                               <SettingsHeaderFilter Mode="CheckedList" />
                                                               <CellStyle HorizontalAlign="Center" />
                                                           </dx:GridViewDataDateColumn>
                                                           <dx:GridViewBandColumn Caption="Conceptos previstos en el artículo 65 de la Ley" HeaderStyle-ForeColor="White">
                                                               <HeaderStyle HorizontalAlign="Center" />
                                                               <Columns>
                                                                   <dx:GridViewDataTextColumn Caption="Anote cada factura o documento comercial que anexa de acuerdo al número asignado" FieldName="Conceptoanexo65" ReadOnly="True" Width="510px">
                                                                       <HeaderStyle HorizontalAlign="Left" />
                                                                       <SettingsHeaderFilter Mode="CheckedList" />
                                                                       <CellStyle HorizontalAlign="Center"></CellStyle>
                                                                   </dx:GridViewDataTextColumn>                                                                   
                                                               </Columns>
                                                           </dx:GridViewBandColumn>
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
                                               <div class="form-group col-sm-12 col-md-12">
                                                   <div class="form-group col-sm-12 col-md-12">
                                                        <dx:ASPxCheckBox ID="chk65_3N" ClientInstanceName="chk65_3N" Enabled="true" runat="server" >
                                                             <ClientSideEvents CheckedChanged="function(s, e) {Get_chk65_3N(s, e); }" /> 
                                                        </dx:ASPxCheckBox>
                                                        &nbsp;&nbsp;                                                    
                                                        <label id="Label69" runat="server" style="font-size:11px">Indicar si NO ANEXA documentación</label>
                                                   </div>                                                    
                                               </div>
                                               <div class="form-group col-sm-12 col-md-12">
                                                    <label id="Label50" runat="server" style="font-size:11px">En caso de NO anexar documentación, deberá señalar el importe de cada uno de ellos e indicará el número que asgne a cada uno de los anexos a que se refiere este párrafo, relacionado el número del anexo (s) en que conste los cargos de referecnia, con la mercancía (s) a cuyo precio pagado deben incrementarse los cargos multicitados.</label>
                                               </div>
                                               <div class="form-group col-sm-12 col-md-12">
                                                   <asp:LinkButton ID="lkb_ManifestacionConcepto65_Agregar" runat="server" OnClick="lkb_ManifestacionConcepto65_Agregar_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button">
                                                       <span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp;Agregar
                                                   </asp:LinkButton>
                                                   <asp:LinkButton ID="lkb_ManifestacionConcepto65_Editar" runat="server" OnClick="lkb_ManifestacionConcepto65_Editar_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button disabled">
                                                       <span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp;Editar
                                                   </asp:LinkButton>
                                                   <asp:LinkButton ID="lkb_ManifestacionConcepto65_Eliminar" runat="server" OnClick="lkb_ManifestacionConcepto65_Eliminar_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button disabled">
                                                       <span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp;Eliminar
                                                   </asp:LinkButton>
                                                    <dx:ASPxGridView ID="Grid5_MV" runat="server" EnableTheming="True" Theme="SoftOrange" OnCustomCallback="Grid5_MV_CustomCallback"
                                                        EnableCallBacks="false" ClientInstanceName="grid5" AutoGenerateColumns="False" Width="100%" 
                                                        Settings-HorizontalScrollBarMode="Auto" KeyFieldName="mvkeyconcepto65" SettingsPager-Position="TopAndBottom"
                                                        Styles-Header-ForeColor="#FFF" Styles-Header-Font-Size="11px" Styles-Cell-Font-Size="11px" >
                                                        <SettingsAdaptivity AdaptivityMode="HideDataCellsWindowLimit" HideDataCellsAtWindowInnerWidth="800" 
                                                            AdaptiveDetailColumnCount="1" AllowOnlyOneAdaptiveDetailExpanded="True">
                                                            <AdaptiveDetailLayoutProperties colcount="2">
                                                                <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit"  SwitchToSingleColumnAtWindowInnerWidth="600" />
                                                            </AdaptiveDetailLayoutProperties>
                                                        </SettingsAdaptivity> 
                                                        <SettingsResizing ColumnResizeMode="Control" />
                                                        <SettingsBehavior AllowSelectByRowClick="false" AllowSelectSingleRowOnly="true" />
                                                        <Settings ShowFooter="False" ShowHeaderFilterButton="true" ShowFilterRowMenu="false"  />                                          
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
                                                            <dx:GridViewDataTextColumn FieldName="mvkeyconcepto65" ReadOnly="True"  Visible="false" >
                                                            </dx:GridViewDataTextColumn>                                                            
                                                            <dx:GridViewDataColumn Caption="Seleccionar" VisibleIndex="0" Width="70px">
                                                               <HeaderStyle HorizontalAlign="Center" ForeColor="White" />
                                                               <CellStyle HorizontalAlign="Center"></CellStyle>
                                                               <DataItemTemplate>
                                                                   <dx:ASPxCheckBox ID="chkConsultarN65" ClientInstanceName="chkConsultar" Enabled="true" runat="server" OnInit="chkConsultarN65_Init">                                                                
                                                                   </dx:ASPxCheckBox>
                                                               </DataItemTemplate>
                                                            </dx:GridViewDataColumn>
                                                            <dx:GridViewDataTextColumn Caption="No." FieldName="ordenconcepto65" ReadOnly="True" VisibleIndex="1" Width="60px">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <SettingsHeaderFilter Mode="CheckedList" />
                                                                <CellStyle HorizontalAlign="Center" />
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Mercancía o<br/>proveedor" FieldName="mercancia65" ReadOnly="True" VisibleIndex="2" Width="118px">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <SettingsHeaderFilter Mode="CheckedList" />
                                                                <CellStyle HorizontalAlign="Center" />
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Factura o<br/>documento" FieldName="factura65" ReadOnly="True" VisibleIndex="3" Width="118px">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <SettingsHeaderFilter Mode="CheckedList" />
                                                                <CellStyle HorizontalAlign="Center" />
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Importe<br/>Facturación" FieldName="importeconcepto65" ReadOnly="True" VisibleIndex="4" Width="118px">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <SettingsHeaderFilter Mode="CheckedList" />
                                                                <CellStyle HorizontalAlign="Center" />
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Moneda<br/>Facturación" FieldName="monedaconcepto65" ReadOnly="True" VisibleIndex="5" Width="118px">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <SettingsHeaderFilter Mode="CheckedList" />
                                                                <CellStyle HorizontalAlign="Center" />
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Concepto<br/>del cargo" FieldName="conceptodelcargo65" ReadOnly="True" VisibleIndex="6" Width="118px">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <SettingsHeaderFilter Mode="CheckedList" />
                                                                <CellStyle HorizontalAlign="Center" />
                                                            </dx:GridViewDataTextColumn>
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
                                       </dx:ContentControl>
                                   </ContentCollection>
                                </dx:TabPage>
                                <dx:TabPage Text="Otros Artículos conforme a la Ley">
                                    <ContentCollection>
                                        <dx:ContentControl ID="ContentControl9" runat="server">
                                            <div style="height: 260px; overflow: auto">
                                               <div style="padding-top:10px">
                                                   <div class="form-group col-sm-12 col-md-12">
                                                       <label id="Label56" runat="server" style="font-size:11px">Para los acsos de donde se utilice cualquier método distinto al de "valor de transacción de las mercancías" debe indicar por tipo de marcancía, la razón por la cual en los términos de los artículos 67 y 71 de la Ley, no utilizó el método de "valor de transacción de las mercancías"</label>                                                    
                                                   </div>                                                   
                                               </div>
                                               <div class="form-group col-sm-12 col-md-12">
                                                    <label id="Label58" runat="server" style="font-size:11px">a) La base gravable deriva de una compraventa para la exportación con destino a territorio nacional.</label>
                                               </div>
                                               <div class="form-group col-sm-12 col-md-12">
                                                   <div class="form-group col-sm-12 col-md-2">
                                                        <dx:ASPxCheckBox ID="chkOtros_1S" ClientInstanceName="chkOtros_1S" Enabled="true" runat="server" >
                                                            <ClientSideEvents CheckedChanged="function(s, e) {Get_chkOtros_1S(s, e); }" />                                                              
                                                        </dx:ASPxCheckBox>
                                                        &nbsp;&nbsp;
                                                        <label id="Label51" runat="server" style="font-size:11px">Si</label>
                                                   </div>                                                   
                                                   <div class="form-group col-sm-12 col-md-2">
                                                        <dx:ASPxCheckBox ID="chkOtros_1N" ClientInstanceName="chkOtros_1N" Enabled="true" runat="server" >
                                                            <ClientSideEvents CheckedChanged="function(s, e) {Get_chkOtros_1N(s, e); }" />                                                              
                                                        </dx:ASPxCheckBox>
                                                        &nbsp;&nbsp;
                                                        <label id="Label52" runat="server" style="font-size:11px">No</label>
                                                   </div>
                                                   <div class="form-group col-sm-12 col-md-8"></div>
                                               </div>
                                               <div class="form-group col-sm-12 col-md-12">
                                                    <label id="Label53" runat="server" style="font-size:11px">b) Si existe alguna circunstancia distinta de las previstas en los artículos 67 y 71 de la Ley que impida utilizar el valor de transacción, lo señalará a continuación :</label>
                                               </div>
                                               <div class="form-group col-sm-12 col-md-12">
                                                    <asp:LinkButton ID="lkb_ManifestacionConcepto67_Agregar" runat="server" OnClick="lkb_ManifestacionConcepto67_Agregar_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button">
                                                        <span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp;Agregar
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="lkb_ManifestacionConcepto67_Editar" runat="server" OnClick="lkb_ManifestacionConcepto67_Editar_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button disabled">
                                                        <span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp;Editar
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="lkb_ManifestacionConcepto67_Eliminar" runat="server" OnClick="lkb_ManifestacionConcepto67_Eliminar_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button disabled">
                                                        <span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp;Eliminar
                                                    </asp:LinkButton>
                                                    <dx:ASPxGridView ID="Grid6_MV" runat="server" EnableTheming="True" Theme="SoftOrange"  OnCustomCallback="Grid6_MV_CustomCallback"
                                                        EnableCallBacks="false" ClientInstanceName="grid6" AutoGenerateColumns="False" Width="100%" 
                                                        Settings-HorizontalScrollBarMode="Auto" KeyFieldName="mvkeyconcepto67" SettingsPager-Position="TopAndBottom"
                                                        Styles-Header-ForeColor="#FFF" Styles-Header-Font-Size="11px" Styles-Cell-Font-Size="11px" >
                                                        <SettingsAdaptivity AdaptivityMode="HideDataCellsWindowLimit" HideDataCellsAtWindowInnerWidth="800" 
                                                            AdaptiveDetailColumnCount="1" AllowOnlyOneAdaptiveDetailExpanded="True">
                                                            <AdaptiveDetailLayoutProperties colcount="2">
                                                                <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit"  SwitchToSingleColumnAtWindowInnerWidth="600" />
                                                            </AdaptiveDetailLayoutProperties>
                                                        </SettingsAdaptivity> 
                                                        <SettingsResizing ColumnResizeMode="Control" />
                                                        <SettingsBehavior AllowSelectByRowClick="false" AllowSelectSingleRowOnly="true" />
                                                        <Settings ShowFooter="False" ShowHeaderFilterButton="true" ShowFilterRowMenu="false"  />
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
                                                            <dx:GridViewDataTextColumn FieldName="mvkeyconcepto67" ReadOnly="True"  Visible="false" >
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataColumn Caption="Seleccionar" VisibleIndex="0" Width="70px">
                                                               <HeaderStyle HorizontalAlign="Center" ForeColor="White" />
                                                               <CellStyle HorizontalAlign="Center"></CellStyle>
                                                               <DataItemTemplate>
                                                                   <dx:ASPxCheckBox ID="chkConsultarOT" ClientInstanceName="chkConsultar" Enabled="true" runat="server" OnInit="chkConsultarOT_Init">                                                                
                                                                   </dx:ASPxCheckBox>
                                                               </DataItemTemplate>
                                                            </dx:GridViewDataColumn>
                                                            <dx:GridViewBandColumn Caption="Numerar anexos y <br/> relacionarlos" HeaderStyle-ForeColor="White">
                                                               <Columns>
                                                                    <dx:GridViewDataTextColumn Caption=" " FieldName="ordenconcepto" ReadOnly="True" Width="110px">
                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                        <SettingsHeaderFilter Mode="CheckedList" />
                                                                        <CellStyle HorizontalAlign="Center" />
                                                                    </dx:GridViewDataTextColumn>
                                                               </Columns>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                            </dx:GridViewBandColumn>
                                                            <dx:GridViewBandColumn Caption="Anote por cada tipo de mercancía, el valor determinado conforme a los artículos 72 a 78 de la Ley,<br/> según el método de valoración utilizado, o bien podrá optar por acompañar los documentos en los<br/> que, en su caso, conste dicho valor en aduana." HeaderStyle-ForeColor="White">
                                                               <Columns>                                                       
                                                                    <dx:GridViewDataTextColumn Caption="Mercancia(s)" FieldName="mercancia" ReadOnly="True" Width="135px">
                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                        <SettingsHeaderFilter Mode="CheckedList" />
                                                                        <CellStyle HorizontalAlign="Center" />
                                                                    </dx:GridViewDataTextColumn>
                                                                   <dx:GridViewDataTextColumn Caption="Valor<br/>determinado" FieldName="valordeterminado" ReadOnly="True" Width="135px">
                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                        <SettingsHeaderFilter Mode="CheckedList" />
                                                                        <CellStyle HorizontalAlign="Center" />
                                                                    </dx:GridViewDataTextColumn>
                                                                   <dx:GridViewDataTextColumn Caption="Método de valor<br/>utilizado" FieldName="metodovalorutilizado" ReadOnly="True" Width="135px">
                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                        <SettingsHeaderFilter Mode="CheckedList" />
                                                                        <CellStyle HorizontalAlign="Center" />
                                                                    </dx:GridViewDataTextColumn>
                                                                   <dx:GridViewDataTextColumn Caption="Motivo o hecho por el cual<br/>utilizó otro método" FieldName="motivodemetodo" ReadOnly="True" Width="135px">
                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                        <SettingsHeaderFilter Mode="CheckedList" />
                                                                        <CellStyle HorizontalAlign="Center" />
                                                                    </dx:GridViewDataTextColumn>
                                                               </Columns>
                                                            </dx:GridViewBandColumn>                                                            
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
                                               <div class="form-group col-sm-12 col-md-12">
                                                    <label id="Label54" runat="server" style="font-size:11px">Señale si optará por acompañar los documentos en los que conste dicho valor en aduana.</label>
                                               </div>
                                               <div class="form-group col-sm-12 col-md-12">
                                                   <div class="form-group col-sm-12 col-md-2">
                                                        <dx:ASPxCheckBox ID="chkOtros_2S" ClientInstanceName="chkOtros_2S" Enabled="true" runat="server" >
                                                            <ClientSideEvents CheckedChanged="function(s, e) {Get_chkOtros_2S(s, e); }" />                                                              
                                                        </dx:ASPxCheckBox>
                                                        &nbsp;&nbsp;
                                                        <label id="Label55" runat="server" style="font-size:11px">Si</label>
                                                   </div>                                                   
                                                   <div class="form-group col-sm-12 col-md-2">
                                                        <dx:ASPxCheckBox ID="chkOtros_2N" ClientInstanceName="chkOtros_2N" Enabled="true" runat="server" >
                                                            <ClientSideEvents CheckedChanged="function(s, e) {Get_chkOtros_2N(s, e); }" />                                                              
                                                        </dx:ASPxCheckBox>
                                                        &nbsp;&nbsp;
                                                        <label id="Label57" runat="server" style="font-size:11px">No</label>
                                                   </div>
                                                   <div class="form-group col-sm-12 col-md-8"></div>
                                               </div>
                                               <div class="form-group col-sm-12 col-md-12">
                                                    <label id="Label59" runat="server" style="font-size:11px">En caso de anexar documentos, indicar el número que asigne a cada uno de sus anexos y relacionarlos con claridad con la mercancía a que corresponda el valor aduana respectivo.</label>
                                               </div>
                                               <div class="form-group col-sm-12 col-md-12">
                                                   <asp:LinkButton ID="lkb_ManifestacionValorAduana_Agregar" runat="server" OnClick="lkb_ManifestacionValorAduana_Agregar_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button">
                                                        <span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp;Agregar
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="lkb_ManifestacionValorAduana_Editar" runat="server" OnClick="lkb_ManifestacionValorAduana_Editar_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button disabled">
                                                        <span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp;Editar
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="lkb_ManifestacionValorAduana_Eliminar" runat="server" OnClick="lkb_ManifestacionValorAduana_Eliminar_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button disabled">
                                                        <span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp;Eliminar
                                                    </asp:LinkButton>
                                                    <dx:ASPxGridView ID="Grid7_MV" runat="server" EnableTheming="True" Theme="SoftOrange" OnCustomCallback="Grid7_MV_CustomCallback"
                                                        EnableCallBacks="false" ClientInstanceName="grid7" AutoGenerateColumns="False" Width="100%" 
                                                        Settings-HorizontalScrollBarMode="Auto" KeyFieldName="mvkeyanexovaloraduana" SettingsPager-Position="TopAndBottom"
                                                        Styles-Header-ForeColor="#FFF" Styles-Header-Font-Size="11px" Styles-Cell-Font-Size="11px" >
                                                        <SettingsAdaptivity AdaptivityMode="HideDataCellsWindowLimit" HideDataCellsAtWindowInnerWidth="800" 
                                                            AdaptiveDetailColumnCount="1" AllowOnlyOneAdaptiveDetailExpanded="True">
                                                            <AdaptiveDetailLayoutProperties colcount="2">
                                                                <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit"  SwitchToSingleColumnAtWindowInnerWidth="600" />
                                                            </AdaptiveDetailLayoutProperties>
                                                        </SettingsAdaptivity> 
                                                        <SettingsResizing ColumnResizeMode="Control" />
                                                        <SettingsBehavior AllowSelectByRowClick="false" AllowSelectSingleRowOnly="true" />
                                                        <Settings ShowFooter="False" ShowHeaderFilterButton="true" ShowFilterRowMenu="false"  />
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
                                                            <dx:GridViewDataTextColumn FieldName="mvkeyanexovaloraduana" ReadOnly="True"  Visible="false" >
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataColumn Caption="Seleccionar" VisibleIndex="0" Width="70px">
                                                               <HeaderStyle HorizontalAlign="Center" ForeColor="White" />
                                                               <CellStyle HorizontalAlign="Center"></CellStyle>
                                                               <DataItemTemplate>
                                                                   <dx:ASPxCheckBox ID="chkConsultarOT2" ClientInstanceName="chkConsultar" Enabled="true" runat="server" OnInit="chkConsultarOT2_Init">                                                                
                                                                   </dx:ASPxCheckBox>
                                                               </DataItemTemplate>
                                                            </dx:GridViewDataColumn>                                                           
                                                            <dx:GridViewDataTextColumn Caption="No. Designado al documento anexado" FieldName="numerodocumentoasignado" ReadOnly="True" Width="325px">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <SettingsHeaderFilter Mode="CheckedList" />
                                                                <CellStyle HorizontalAlign="Center" />
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Mercancía con la que se relaciona" FieldName="mercanciarelacion" ReadOnly="True" Width="325px">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <SettingsHeaderFilter Mode="CheckedList" />
                                                                <CellStyle HorizontalAlign="Center" />
                                                            </dx:GridViewDataTextColumn>                                                            
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
                                        </dx:ContentControl>
                                    </ContentCollection>
                                </dx:TabPage>
                                <dx:TabPage Text="Importación Temporal">
                                    <ContentCollection>
                                        <dx:ContentControl ID="ContentControl10" runat="server">                                            
                                            <div style="height: 260px; overflow: auto">
                                               <div style="padding-top:10px">
                                                   <div class="form-group col-sm-12 col-md-12">
                                                       <label id="Label60" runat="server" style="font-size:11px">En caso de importaciones temporales señale lo siguiente:</label>
                                                   </div>                                                   
                                               </div>
                                               <div class="form-group col-sm-12 col-md-12">
                                                    <label id="Label61" runat="server" style="font-size:11px">El valor determinado por las mercancías es provisional.</label>
                                               </div>
                                               <div class="form-group col-sm-12 col-md-12">
                                                   <div class="form-group col-sm-12 col-md-2">
                                                        <dx:ASPxCheckBox ID="chkIT_1S" ClientInstanceName="chkIT_1S" Enabled="true" runat="server" >
                                                            <ClientSideEvents CheckedChanged="function(s, e) {Get_chkIT_1S(s, e); }" />                                                              
                                                        </dx:ASPxCheckBox>
                                                        &nbsp;&nbsp;
                                                        <label id="Label62" runat="server" style="font-size:11px">Si</label>
                                                   </div>                                                   
                                                   <div class="form-group col-sm-12 col-md-2">
                                                        <dx:ASPxCheckBox ID="chkIT_1N" ClientInstanceName="chkIT_1N" Enabled="true" runat="server" >
                                                            <ClientSideEvents CheckedChanged="function(s, e) {Get_chkIT_1N(s, e); }" />                                                              
                                                        </dx:ASPxCheckBox>
                                                        &nbsp;&nbsp;
                                                        <label id="Label63" runat="server" style="font-size:11px">No</label>
                                                   </div>
                                                   <div class="form-group col-sm-12 col-md-8"></div>
                                               </div>
                                               <div class="form-group col-sm-12 col-md-12">
                                                    <label id="Label64" runat="server" style="font-size:11px">Se anexa la documentación en la que consta el valor de la mercancía.</label>
                                               </div>
                                               <div class="form-group col-sm-12 col-md-12">
                                                   <div class="form-group col-sm-12 col-md-2">
                                                        <dx:ASPxCheckBox ID="chkIT_2S" ClientInstanceName="chkIT_2S" Enabled="true" runat="server" >
                                                            <ClientSideEvents CheckedChanged="function(s, e) {Get_chkIT_2S(s, e); }" />                                                              
                                                        </dx:ASPxCheckBox>
                                                        &nbsp;&nbsp;
                                                        <label id="Label66" runat="server" style="font-size:11px">Si</label>
                                                   </div>                                                   
                                                   <div class="form-group col-sm-12 col-md-2">
                                                        <dx:ASPxCheckBox ID="chkIT_2N" ClientInstanceName="chkIT_2N" Enabled="true" runat="server" >
                                                            <ClientSideEvents CheckedChanged="function(s, e) {Get_chkIT_2N(s, e); }" />                                                              
                                                        </dx:ASPxCheckBox>
                                                        &nbsp;&nbsp;
                                                        <label id="Label67" runat="server" style="font-size:11px">No</label>
                                                   </div>
                                                   <div class="form-group col-sm-12 col-md-8"></div>
                                               </div>
                                               <div class="form-group col-sm-12 col-md-12">
                                                    <label id="Label65" runat="server" style="font-size:11px">En caso de que no anexe la documentación en la que cobste el valor de la mercancía(s) a importar temporalmente, debe indicar el valor provisional de la misma señalando los siguientes datos :</label>
                                               </div>
                                               <div class="form-group col-sm-12 col-md-12">
                                                    <asp:LinkButton ID="lkb_ManifestacionMercanciaProvisional_Agregar" runat="server" OnClick="lkb_ManifestacionMercanciaProvisional_Agregar_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button">
                                                        <span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp;Agregar
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="lkb_ManifestacionMercanciaProvisional_Editar" runat="server" OnClick="lkb_ManifestacionMercanciaProvisional_Editar_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button disabled">
                                                        <span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp;Editar
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="lkb_ManifestacionMercanciaProvisional_Eliminar" runat="server" OnClick="lkb_ManifestacionMercanciaProvisional_Eliminar_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button disabled">
                                                        <span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp;Eliminar
                                                    </asp:LinkButton>
                                                    <dx:ASPxGridView ID="Grid9_MV" runat="server" EnableTheming="True" Theme="SoftOrange" OnCustomCallback="Grid9_MV_CustomCallback"
                                                        EnableCallBacks="false" ClientInstanceName="grid9" AutoGenerateColumns="False" Width="100%" 
                                                        Settings-HorizontalScrollBarMode="Auto" KeyFieldName="mvkeymercanciaprovisional" SettingsPager-Position="TopAndBottom"
                                                        Styles-Header-ForeColor="#FFF" Styles-Header-Font-Size="11px" Styles-Cell-Font-Size="11px" >
                                                        <SettingsAdaptivity AdaptivityMode="HideDataCellsWindowLimit" HideDataCellsAtWindowInnerWidth="800" 
                                                            AdaptiveDetailColumnCount="1" AllowOnlyOneAdaptiveDetailExpanded="True">
                                                            <AdaptiveDetailLayoutProperties colcount="2">
                                                                <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit"  SwitchToSingleColumnAtWindowInnerWidth="600" />
                                                            </AdaptiveDetailLayoutProperties>
                                                        </SettingsAdaptivity> 
                                                        <SettingsResizing ColumnResizeMode="Control" />
                                                        <SettingsBehavior AllowSelectByRowClick="false" AllowSelectSingleRowOnly="true" />
                                                        <Settings ShowFooter="False" ShowHeaderFilterButton="true" ShowFilterRowMenu="false"  />                                          
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
                                                            <dx:GridViewDataTextColumn FieldName="mvkeymercanciaprovisional" ReadOnly="True"  Visible="false" >
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataColumn Caption="Seleccionar" VisibleIndex="0" Width="70px">
                                                               <HeaderStyle HorizontalAlign="Center" ForeColor="White" />
                                                               <CellStyle HorizontalAlign="Center"></CellStyle>
                                                               <DataItemTemplate>
                                                                   <dx:ASPxCheckBox ID="chkConsultarIT" ClientInstanceName="chkConsultar" Enabled="true" runat="server" OnInit="chkConsultarIT_Init">                                                                
                                                                   </dx:ASPxCheckBox>
                                                               </DataItemTemplate>
                                                            </dx:GridViewDataColumn>                                                           
                                                            <dx:GridViewDataTextColumn Caption="Tipo de mercancia" FieldName="tipomercancia" ReadOnly="True" Width="325px">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <SettingsHeaderFilter Mode="CheckedList" />
                                                                <CellStyle HorizontalAlign="Center" />
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Valor provisional" FieldName="valorprovisional" ReadOnly="True" Width="325px">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <SettingsHeaderFilter Mode="CheckedList" />
                                                                <CellStyle HorizontalAlign="Center" />
                                                            </dx:GridViewDataTextColumn>                                                            
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
                                        </dx:ContentControl>
                                    </ContentCollection>
                                </dx:TabPage>
                                <dx:TabPage Text="Periocidad de la Manifestación de Valor">
                                    <ContentCollection>
                                        <dx:ContentControl ID="ContentControl5" runat="server">
                                            <div style="height: 360px; overflow: auto;">
                                                <div class="form-group col-sm-12 col-md-12">
                                                     <label id="Label92" runat="server" style="font-size:11px">Bajo protesta de decir verdad, manifiesto que los datos asentados en el presente documento son ciertos.</label>
                                                </div>
                                                <div class="col-sm-12 col-md-12">
                                                    <div class="form-group col-sm-12 col-md-4">
                                                        <table style="width:100%;">
                                                            <tr>
                                                                <td style="width:30%;">
                                                                    <label id="Label93" runat="server" style="font-size:11px">Fecha:</label>
                                                                </td>
                                                                <td style="width:70%">
                                                                    <dx:ASPxDateEdit ID="date_MV_FECHAPEDIMENTO" runat="server" EditFormat="Custom" Date="" Width="120px" Height="30px" Theme="MaterialCompact" 
                                                                         Font-Size="11px" CssClass="bordes_curvos" NullText="Fecha" DisplayFormatString="dd/MM/yyyy">
                                                                         <CalendarProperties EnableMonthNavigation="True" EnableYearNavigation="True" ShowClearButton="False" ShowDayHeaders="True" ShowTodayButton="False" ShowWeekNumbers="False">
                                                                             <MonthGridPaddings Padding="0px" />                                                                 
                                                                             <Style Font-Size="10px"></Style>
                                                                         </CalendarProperties>
                                                                    </dx:ASPxDateEdit>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                         
                                                    </div>
                                                    <div class="form-group col-sm-12 col-md-1"></div>
                                                    <div class="form-group col-sm-12 col-md-7">
                                                        <table style="width:100%;">
                                                            <tr>
                                                                <td style="width:15%">
                                                                    <label id="Label114" runat="server" style="font-size:11px">RFC:</label>
                                                                </td>
                                                                <td style="width:85%">
                                                                    <asp:TextBox ID="txt_gdb_rfcmanifestacion" runat="server" CssClass="form-control input-sm" MaxLength="50" placeholder="" Width="100%"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>                                                    
                                                </div>                                                
                                                <div style="padding-top:10px">
                                                    <div class="form-group col-sm-12 col-md-12">
                                                         <label id="Label70" runat="server" style="font-size:11px">Señale si es el importador presenta la manifestación de valor por operación o por periodo de seis meses</label>
                                                    </div>    
                                                </div>
                                                <div class="form-group col-sm-12 col-md-12">
                                                   <div class="form-group col-sm-12 col-md-3">
                                                        <dx:ASPxCheckBox ID="chkPMV_1S" ClientInstanceName="chkPMV_1S" Enabled="true" runat="server" >
                                                            <ClientSideEvents CheckedChanged="function(s, e) {Get_chkPMV_1S(s, e); }" />                                                              
                                                        </dx:ASPxCheckBox>
                                                        &nbsp;&nbsp;
                                                        <label id="Label90" runat="server" style="font-size:11px">Por operación</label>
                                                   </div>                                                   
                                                   <div class="form-group col-sm-12 col-md-4">
                                                        <dx:ASPxCheckBox ID="chkPMV_1N" ClientInstanceName="chkPMV_1N" Enabled="true" runat="server" >
                                                            <ClientSideEvents CheckedChanged="function(s, e) {Get_chkPMV_1N(s, e); }" />                                                              
                                                        </dx:ASPxCheckBox>
                                                        &nbsp;&nbsp;
                                                        <label id="Label91" runat="server" style="font-size:11px">Por período de seis meses</label>
                                                   </div>
                                                   <div class="form-group col-sm-12 col-md-5"></div>
                                                </div>
                                                <div class="col-sm-12 col-md-12">
                                                    <div class="form-group col-sm-12 col-md-12">
                                                        <label id="Label113" runat="server" style="font-size:11px">Nombre del importador o de su representante legal:</label>
                                                        &nbsp;&nbsp;        
                                                        <asp:TextBox ID="txt_gdb_nombremanifestacion" runat="server" CssClass="form-control input-sm" MaxLength="100" placeholder="" Width="100%"></asp:TextBox>     
                                                    </div>
                                                </div>
                                                <div class="col-sm-12 col-md-12">
                                                    <div class="col-md-4">
                                                        <asp:LinkButton ID="lkb_RL" runat="server" OnClick="lkb_RL_Click" CssClass="btn btn-primary btn-sm txt-sm margin-top-bottom-button">
                                                             <span class="glyphicon glyphicon-search"></span>&nbsp;&nbsp;Seleccione un Representante Legal
                                                        </asp:LinkButton>
                                                    </div>                                                    
                                                    <div class="col-md-4">
                                                        &nbsp;<label id="Label115" runat="server" style="font-size:11px">Firma</label>
                                                        <dx:ASPxBinaryImage ID="ASPxBinaryImage" ClientInstanceName="ClientBinaryImage" Width="200" Height="80"  
                                                        ShowLoadingImage="true" LoadingImageUrl="~/Content/Loading.gif" runat="server" EditingSettings-EmptyValueText="firma" Font-Size="10px" >
                                                        <EditingSettings Enabled="false">
                                                        </EditingSettings>
                                                    </dx:ASPxBinaryImage>
                                                    </div>
                                                    <div class="col-md-2">
                                                        &nbsp;<label id="Label116" runat="server" style="font-size:11px">Mostrar</label>
                                                        <dx:ASPxCheckBox ID="cbx_MostrarFirma" runat="server"  Checked="false" Width="100%">
                                                        </dx:ASPxCheckBox>
                                                    </div>
                                                    <div class="col-md-2"></div>
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
                     <%--<asp:LinkButton ID="btnCancelar" runat="server" CssClass="btn btn-primary btn-sm" Text="Cancelar" OnClick="btnCancelar_Click">
                        <span class="glyphicon glyphicon-remove"></span>&nbsp;&nbsp;Cancelar</asp:LinkButton>--%>
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
                                                <HeaderStyle HorizontalAlign="Left"   />
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

    <button id="btnModalDF" type="button" data-toggle="modal" data-target="#ModalDF" data-whatever="Nuevo" onclick="return false;" style="display: none;"></button>
    <div class="modal fade" id="ModalDF" tabindex="-1" role="dialog" aria-labelledby="ModalTituloDF" data-backdrop="static" data-keyboard="false" >
        <div class="modal-dialog modal-sm" role="document" >
            <div class="modal-content">
                <div class="modal-header">
                    <h4 id="ModalTituloDF" class="modal-title" runat="server"></h4>                    
                </div>
                <asp:Panel ID="Panel7" runat="server">
                    <div class="modal-body">
                        <div class="row">
                            <div class="form-group col-md-6">                                
                                &nbsp;<label id="Label118" runat="server" style="font-size:11px">* Factura</label>
                                <asp:TextBox ID="txt_DF_Factura" runat="server" CssClass="form-control input-sm" MaxLength="30" placeholder="" Font-Size="11px" Width="100%"></asp:TextBox>                                                            
                            </div>
                            <div class="form-group col-md-12">                        
                                &nbsp;<label id="lblT_DF_Fecha" runat="server" style="font-size:11px">* Fecha</label>
                                <dx:ASPxDateEdit ID="date_DF_Fecha" runat="server" EditFormat="Custom" Date="" Width="120px" Height="30px" Theme="MaterialCompact" 
                                    Font-Size="11px" CssClass="bordes_curvos" NullText="" DisplayFormatString="dd/MM/yyyy">
                                    <CalendarProperties EnableMonthNavigation="True" EnableYearNavigation="True" ShowClearButton="False" ShowDayHeaders="True" ShowTodayButton="False" ShowWeekNumbers="False">
                                         <MonthGridPaddings Padding="0px" />
                                         <Style Font-Size="10px"></Style>
                                     </CalendarProperties>
                                </dx:ASPxDateEdit>                                                           
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
                            <asp:LinkButton ID="btnGuardarDF" runat="server" CssClass="btn btn-primary btn-sm" Text="Guardar" OnClick="btnGuardarDF_Click">
                                <span class="glyphicon glyphicon-ok"></span>&nbsp;&nbsp;Guardar
                            </asp:LinkButton>
                             <asp:LinkButton ID="btnCancelarDF" runat="server" CssClass="btn btn-primary btn-sm" Text="Cancelar" OnClick="btnCancelarDF_Click">
                                <span class="glyphicon glyphicon-remove"></span>&nbsp;&nbsp;Cancelar
                             </asp:LinkButton>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div>

    <button id="btnModalTM" type="button" data-toggle="modal" data-target="#ModalTM" data-whatever="Nuevo" onclick="return false;" style="display: none;"></button>
    <div class="modal fade" id="ModalTM" tabindex="-1" role="dialog" aria-labelledby="ModalTituloTM" data-backdrop="static" data-keyboard="false" >
        <div class="modal-dialog modal-sm" role="document" >
            <div class="modal-content">
                <div class="modal-header">
                    <h4 id="ModalTituloTM" class="modal-title" runat="server"></h4>                    
                </div>
                <asp:Panel ID="Panel8" runat="server">
                    <div class="modal-body">
                        <div class="row">
                            <div class="form-group col-md-6">                                
                                &nbsp;<label id="Label119" runat="server" style="font-size:11px">* Numerar Anexos:</label>
                                <asp:TextBox ID="txt_ordenanexo" runat="server" CssClass="form-control input-sm" MaxLength="10" placeholder="" Font-Size="11px" onkeypress="return onlyDotsAndNumbers(this,event);" Width="100%"></asp:TextBox>
                            </div>
                            <div class="form-group col-md-12">                        
                                &nbsp;<label id="Label120" runat="server" style="font-size:11px">* Conceptos previstos en el artículo 66 de la Ley:</label>                                
                                <dx:ASPxMemo ID="Memo_Conceptoanexo" runat="server" ClientInstanceName="Memo_Conceptoanexo" CssClass="form-control input-sm" Native="true" MaxLength="50" placeholder="" Font-Size="11px" Height="60px" ></dx:ASPxMemo>
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
                            <asp:LinkButton ID="btnGuardarTM" runat="server" CssClass="btn btn-primary btn-sm" Text="Guardar" OnClick="btnGuardarTM_Click">
                                <span class="glyphicon glyphicon-ok"></span>&nbsp;&nbsp;Guardar
                            </asp:LinkButton>
                             <asp:LinkButton ID="btnCancelarTM" runat="server" CssClass="btn btn-primary btn-sm" Text="Cancelar" OnClick="btnCancelarTM_Click">
                                <span class="glyphicon glyphicon-remove"></span>&nbsp;&nbsp;Cancelar
                             </asp:LinkButton>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div>

    <button id="btnModalTM2" type="button" data-toggle="modal" data-target="#ModalTM2" data-whatever="Nuevo" onclick="return false;" style="display: none;"></button>
    <div class="modal fade" id="ModalTM2" tabindex="-1" role="dialog" aria-labelledby="ModalTituloTM2" data-backdrop="static" data-keyboard="false" >
        <div class="modal-dialog modal-lg" role="document" >
            <div class="modal-content">
                <div class="modal-header">
                    <h4 id="ModalTituloTM2" class="modal-title" runat="server"></h4>                    
                </div>
                <asp:Panel ID="Panel9" runat="server">
                    <div class="modal-body">
                        <div class="row">
                            <div class="form-group col-md-12">
                                <div class="form-group col-md-2">                                
                                    &nbsp;<label id="Label121" runat="server" style="font-size:11px">* No. Concepto:</label>
                                    <asp:TextBox ID="txt_TM2_ordenconcepto" runat="server" CssClass="form-control input-sm" MaxLength="10" placeholder="" Font-Size="11px" onkeypress="return onlyDotsAndNumbers(this,event);" Width="100%"></asp:TextBox>
                                </div>
                                <div class="form-group col-md-4">                        
                                    &nbsp;<label id="Label122" runat="server" style="font-size:11px">* Mercancía:</label>                                
                                    <asp:TextBox ID="txt_TM2_mercancia" runat="server" CssClass="form-control input-sm" MaxLength="250" placeholder="" Font-Size="11px" Width="100%"></asp:TextBox>
                                </div>
                                <div class="form-group col-md-4">                        
                                    &nbsp;<label id="Label123" runat="server" style="font-size:11px">* Factura:</label>                                
                                    <asp:TextBox ID="txt_TM2_factura" runat="server" CssClass="form-control input-sm" MaxLength="50" placeholder="" Font-Size="11px" Width="100%"></asp:TextBox>
                                </div>
                                <div class="form-group col-md-2">                        
                                    &nbsp;<label id="Label124" runat="server" style="font-size:11px">* Importe Concepto:</label>                                
                                    <asp:TextBox ID="txt_TM2_importe" runat="server" CssClass="form-control input-sm" MaxLength="20" placeholder="" Font-Size="11px" onkeypress="return onlyDotsAndNumbers(this,event);" Width="100%"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-12">
                                <div class="form-group col-md-2">                        
                                    &nbsp;<label id="Label125" runat="server" style="font-size:11px">* Moneda:</label>                                
                                    <asp:TextBox ID="txt_TM2_moneda" runat="server" CssClass="form-control input-sm" MaxLength="5" placeholder="" Font-Size="11px" Width="100%"></asp:TextBox>
                                </div>
                                <div class="form-group col-md-4">                        
                                    &nbsp;<label id="Label126" runat="server" style="font-size:11px">* Concepto del Cargo:</label>                                
                                    <asp:TextBox ID="txt_TM2_concepto_cargo" runat="server" CssClass="form-control input-sm" MaxLength="250" placeholder="" Font-Size="11px" Width="100%"></asp:TextBox>
                                </div>
                                <div class="form-group col-md-6"></div>
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
                            <asp:LinkButton ID="LinkButton5" runat="server" CssClass="btn btn-primary btn-sm" Text="Guardar" OnClick="btnGuardarTM2_Click">
                                <span class="glyphicon glyphicon-ok"></span>&nbsp;&nbsp;Guardar
                            </asp:LinkButton>
                             <asp:LinkButton ID="LinkButton6" runat="server" CssClass="btn btn-primary btn-sm" Text="Cancelar" OnClick="btnCancelarTM2_Click">
                                <span class="glyphicon glyphicon-remove"></span>&nbsp;&nbsp;Cancelar
                             </asp:LinkButton>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div>

    <button id="btnModalS65" type="button" data-toggle="modal" data-target="#ModalS65" data-whatever="Nuevo" onclick="return false;" style="display: none;"></button>
    <div class="modal fade" id="ModalS65" tabindex="-1" role="dialog" aria-labelledby="ModalTituloS65" data-backdrop="static" data-keyboard="false" >
        <div class="modal-dialog modal-sm" role="document" >
            <div class="modal-content">
                <div class="modal-header">
                    <h4 id="ModalTituloS65" class="modal-title" runat="server"></h4>                    
                </div>
                <asp:Panel ID="Panel10" runat="server">
                    <div class="modal-body">
                        <div class="row">
                            <div class="form-group col-md-6">                                
                                &nbsp;<label id="Label127" runat="server" style="font-size:11px">* Numerar Anexos:</label>
                                <asp:TextBox ID="txt_S65_ordenanexo65" runat="server" CssClass="form-control input-sm" MaxLength="10" placeholder="" Font-Size="11px" onkeypress="return onlyDotsAndNumbers(this,event);" Width="100%"></asp:TextBox>
                            </div>
                            <div class="form-group col-md-12">                        
                                &nbsp;<label id="Label128" runat="server" style="font-size:11px">* Factura o Documento Comercial:</label>                                
                                <dx:ASPxMemo ID="Memo_S65_conceptoanexo65" runat="server" ClientInstanceName="ASPxMemo_S65_conceptoanexo65" CssClass="form-control input-sm" Native="true" MaxLength="150" placeholder="" Font-Size="11px" Height="60px" ></dx:ASPxMemo>
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
                            <asp:LinkButton ID="LinkButton7" runat="server" CssClass="btn btn-primary btn-sm" Text="Guardar" OnClick="btnGuardarS65_Click">
                                <span class="glyphicon glyphicon-ok"></span>&nbsp;&nbsp;Guardar
                            </asp:LinkButton>
                             <asp:LinkButton ID="LinkButton8" runat="server" CssClass="btn btn-primary btn-sm" Text="Cancelar" OnClick="btnCancelarS65_Click">
                                <span class="glyphicon glyphicon-remove"></span>&nbsp;&nbsp;Cancelar
                             </asp:LinkButton>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div>

    <button id="btnModalN65" type="button" data-toggle="modal" data-target="#ModalN65" data-whatever="Nuevo" onclick="return false;" style="display: none;"></button>
    <div class="modal fade" id="ModalN65" tabindex="-1" role="dialog" aria-labelledby="ModalTituloN65" data-backdrop="static" data-keyboard="false" >
        <div class="modal-dialog modal-lg" role="document" >
            <div class="modal-content">
                <div class="modal-header">
                    <h4 id="ModalTituloN65" class="modal-title" runat="server"></h4>                    
                </div>
                <asp:Panel ID="Panel11" runat="server">
                    <div class="modal-body">
                        <div class="row">
                            <div class="form-group col-md-12">
                                <div class="form-group col-md-2">                                
                                    &nbsp;<label id="Label129" runat="server" style="font-size:11px">* Numerar Anexos:</label>
                                    <asp:TextBox ID="txt_N65_ordenconcepto" runat="server" CssClass="form-control input-sm" MaxLength="10" placeholder="" Font-Size="11px" onkeypress="return onlyDotsAndNumbers(this,event);" Width="100%"></asp:TextBox>
                                </div>
                                <div class="form-group col-md-4">                        
                                    &nbsp;<label id="Label130" runat="server" style="font-size:11px">* Mercancía o Proveedor:</label>                                
                                    <asp:TextBox ID="txt_N65_mercancia" runat="server" CssClass="form-control input-sm" MaxLength="250" placeholder="" Font-Size="11px" Width="100%"></asp:TextBox>
                                </div>
                                <div class="form-group col-md-4">                        
                                    &nbsp;<label id="Label131" runat="server" style="font-size:11px">* Factura:</label>                                
                                    <asp:TextBox ID="txt_N65_factura" runat="server" CssClass="form-control input-sm" MaxLength="250" placeholder="" Font-Size="11px" Width="100%"></asp:TextBox>
                                </div>
                                <div class="form-group col-md-2">                        
                                    &nbsp;<label id="Label132" runat="server" style="font-size:11px">* Importe Concepto:</label>                                
                                    <asp:TextBox ID="txt_N65_importe" runat="server" CssClass="form-control input-sm" MaxLength="250" placeholder="" onkeypress="return onlyDotsAndNumbers(this,event);" Font-Size="11px" Width="100%"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-12">
                                <div class="form-group col-md-2">                        
                                    &nbsp;<label id="Label133" runat="server" style="font-size:11px">* Moneda:</label>                                
                                    <asp:TextBox ID="txt_N65_moneda" runat="server" CssClass="form-control input-sm" MaxLength="5" placeholder="" Font-Size="11px" Width="100%"></asp:TextBox>                                
                                </div>
                                <div class="form-group col-md-4">                        
                                    &nbsp;<label id="Label134" runat="server" style="font-size:11px">* Concepto del Cargo:</label>                                
                                    <asp:TextBox ID="txt_N65_conceptocargo" runat="server" CssClass="form-control input-sm" MaxLength="250" placeholder="" Font-Size="11px" Width="100%"></asp:TextBox>
                                </div>
                                <div class="form-group col-md-6"></div>
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
                            <asp:LinkButton ID="btnGuardarN65" runat="server" CssClass="btn btn-primary btn-sm" Text="Guardar" OnClick="btnGuardarN65_Click">
                                <span class="glyphicon glyphicon-ok"></span>&nbsp;&nbsp;Guardar
                            </asp:LinkButton>
                             <asp:LinkButton ID="btnCancelarN65" runat="server" CssClass="btn btn-primary btn-sm" Text="Cancelar" OnClick="btnCancelarN65_Click">
                                <span class="glyphicon glyphicon-remove"></span>&nbsp;&nbsp;Cancelar
                             </asp:LinkButton>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div>

    <button id="btnModalOT" type="button" data-toggle="modal" data-target="#ModalOT" data-whatever="Nuevo" onclick="return false;" style="display: none;"></button>
    <div class="modal fade" id="ModalOT" tabindex="-1" role="dialog" aria-labelledby="ModalTituloOT" data-backdrop="static" data-keyboard="false" >
        <div class="modal-dialog modal-lg" role="document" >
            <div class="modal-content">
                <div class="modal-header">
                    <h4 id="ModalTituloOT" class="modal-title" runat="server"></h4>                    
                </div>
                <asp:Panel ID="Panel12" runat="server">
                    <div class="modal-body">
                        <div class="row">
                            <div class="form-group col-md-12">
                                <div class="form-group col-md-2">                                
                                    &nbsp;<label id="Label135" runat="server" style="font-size:11px">* Numerar Concepto:</label>
                                    <asp:TextBox ID="txt_OT_ordenconcepto" runat="server" CssClass="form-control input-sm" MaxLength="10" placeholder="" Font-Size="11px" onkeypress="return onlyDotsAndNumbers(this,event);" Width="100%"></asp:TextBox>
                                </div>
                                <div class="form-group col-md-4">                        
                                    &nbsp;<label id="Label136" runat="server" style="font-size:11px">* Mercancía(s):</label>                                
                                    <asp:TextBox ID="txt_OT_mercancia" runat="server" CssClass="form-control input-sm" MaxLength="250" placeholder="" Font-Size="11px" Width="100%"></asp:TextBox>
                                </div>
                                <div class="form-group col-md-4">                        
                                    &nbsp;<label id="Label137" runat="server" style="font-size:11px">* Valor determinado:</label>                                
                                    <asp:TextBox ID="txt_OT_valordeterminado" runat="server" CssClass="form-control input-sm" MaxLength="20" placeholder="" Font-Size="11px" onkeypress="return onlyDotsAndNumbers(this,event);" Width="100%"></asp:TextBox>
                                </div>
                                <div class="form-group col-md-2"></div>
                            </div>
                            <div class="col-md-12">
                                <div class="form-group col-md-4">                        
                                    &nbsp;<label id="Label138" runat="server" style="font-size:11px">* Método de valor utilizado:</label>                                
                                    <asp:TextBox ID="txt_OT_metodovalorutilizado" runat="server" CssClass="form-control input-sm" MaxLength="50" placeholder="" Font-Size="11px" Width="100%"></asp:TextBox>
                                </div>
                                <div class="form-group col-md-4">                        
                                    &nbsp;<label id="Label139" runat="server" style="font-size:11px">* Motivo o hecho de otro método:</label>                                
                                    <asp:TextBox ID="txt_OT_motivodemetodo" runat="server" CssClass="form-control input-sm" MaxLength="50" placeholder="" Font-Size="11px" Width="100%"></asp:TextBox>
                                </div>
                                <div class="form-group col-md-4"></div>                           
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
                            <asp:LinkButton ID="btnGuardarOT" runat="server" CssClass="btn btn-primary btn-sm" Text="Guardar" OnClick="btnGuardarOT_Click">
                                <span class="glyphicon glyphicon-ok"></span>&nbsp;&nbsp;Guardar
                            </asp:LinkButton>
                             <asp:LinkButton ID="btnCancelarOT" runat="server" CssClass="btn btn-primary btn-sm" Text="Cancelar" OnClick="btnCancelarOT_Click">
                                <span class="glyphicon glyphicon-remove"></span>&nbsp;&nbsp;Cancelar
                             </asp:LinkButton>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div>

    <button id="btnModalOT2" type="button" data-toggle="modal" data-target="#ModalOT2" data-whatever="Nuevo" onclick="return false;" style="display: none;"></button>
    <div class="modal fade" id="ModalOT2" tabindex="-1" role="dialog" aria-labelledby="ModalTituloOT2" data-backdrop="static" data-keyboard="false" >
        <div class="modal-dialog modal-sm" role="document" >
            <div class="modal-content">
                <div class="modal-header">
                    <h4 id="ModalTituloOT2" class="modal-title" runat="server"></h4>                    
                </div>
                <asp:Panel ID="Panel13" runat="server">
                    <div class="modal-body">
                        <div class="row">
                            <div class="form-group col-md-12">                                
                                &nbsp;<label id="Label140" runat="server" style="font-size:11px">* No. Designado al documento anexado:</label>
                                <asp:TextBox ID="txt_OT2_numerodocumentoasignado" runat="server" CssClass="form-control input-sm" MaxLength="50" placeholder="" Font-Size="11px" onkeypress="return onlyDotsAndNumbers(this,event);" Width="100%"></asp:TextBox>
                            </div>
                            <div class="form-group col-md-12">                        
                                &nbsp;<label id="Label141" runat="server" style="font-size:11px">* Mercancía con la que se relaciona:</label>                                
                                <asp:TextBox ID="txt_OT2_mercanciarelacion" runat="server" CssClass="form-control input-sm" MaxLength="50" placeholder="" Font-Size="11px" Width="100%"></asp:TextBox>
                            </div>                                
                        </div>
                    </div>                
                    <div class="modal-footer">
                        <div class="col-md-12 col-md-offset-0" style="text-align: center;">
                            <asp:LinkButton ID="btnGuardarOT2" runat="server" CssClass="btn btn-primary btn-sm" Text="Guardar" OnClick="btnGuardarOT2_Click">
                                <span class="glyphicon glyphicon-ok"></span>&nbsp;&nbsp;Guardar
                            </asp:LinkButton>
                             <asp:LinkButton ID="btnCancelarOT2" runat="server" CssClass="btn btn-primary btn-sm" Text="Cancelar" OnClick="btnCancelarOT2_Click">
                                <span class="glyphicon glyphicon-remove"></span>&nbsp;&nbsp;Cancelar
                             </asp:LinkButton>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div>

    <button id="btnModalIT" type="button" data-toggle="modal" data-target="#ModalIT" data-whatever="Nuevo" onclick="return false;" style="display: none;"></button>
    <div class="modal fade" id="ModalIT" tabindex="-1" role="dialog" aria-labelledby="ModalTituloIT" data-backdrop="static" data-keyboard="false" >
        <div class="modal-dialog modal-sm" role="document" >
            <div class="modal-content">
                <div class="modal-header">
                    <h4 id="ModalTituloIT" class="modal-title" runat="server"></h4>                    
                </div>
                <asp:Panel ID="Panel14" runat="server">
                    <div class="modal-body">
                        <div class="row">
                            <div class="form-group col-md-12">                                
                                &nbsp;<label id="Label142" runat="server" style="font-size:11px">* Tipo de mercancia:</label>
                                <asp:TextBox ID="txt_IT_tipomercancia" runat="server" CssClass="form-control input-sm" MaxLength="50" placeholder="" Font-Size="11px" Width="100%"></asp:TextBox>
                            </div>
                            <div class="form-group col-md-12">                        
                                &nbsp;<label id="Label143" runat="server" style="font-size:11px">* Valor provisional:</label>                                
                                <asp:TextBox ID="txt_IT_valorprovisional" runat="server" CssClass="form-control input-sm" MaxLength="50" placeholder="" Font-Size="11px" onkeypress="return onlyDotsAndNumbers(this,event);" Width="100%"></asp:TextBox>
                            </div>                                
                        </div>
                    </div>                
                    <div class="modal-footer">
                        <div class="col-md-12 col-md-offset-0" style="text-align: center;">
                            <asp:LinkButton ID="btnGuardarIT" runat="server" CssClass="btn btn-primary btn-sm" Text="Guardar" OnClick="btnGuardarIT_Click">
                                <span class="glyphicon glyphicon-ok"></span>&nbsp;&nbsp;Guardar
                            </asp:LinkButton>
                             <asp:LinkButton ID="btnCancelarIT" runat="server" CssClass="btn btn-primary btn-sm" Text="Cancelar" OnClick="btnCancelarIT_Click">
                                <span class="glyphicon glyphicon-remove"></span>&nbsp;&nbsp;Cancelar
                             </asp:LinkButton>
                        </div>
                    </div>
                </asp:Panel>
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

    <button id="btnQuestionDF" type="button" data-toggle="modal" data-target="#AlertQuestionDF" style="display: none;"></button>
    <div class="modal fade" id="AlertQuestionDF" tabindex="-1" role="dialog" style="top: 25%; outline: none;">
        <div class="modal-dialog modal-sm" role="document">
            <div class="alert alert-info text-center" style="-webkit-box-shadow: 0 5px 15px rgba(0, 0, 0, .5); box-shadow: 0 5px 15px rgba(0, 0, 0, .5);">
                <span class="glyphicon glyphicon-question-sign ico"></span>
                <br />
                <br />
                <p id="pModalQuestionDF" runat="server" class="alert-title">
                </p>
                <hr />
                <asp:Button ID="btnOkDF" runat="server" CssClass="btn btn-info" OnClick="btnOkDF_Click" Text="Aceptar"></asp:Button>
                <button id="btnDFCancelar" runat="server" class="btn btn-info" data-dismiss="modal">Cancelar</button>
            </div>
        </div>
    </div>

    <button id="btnQuestionTM" type="button" data-toggle="modal" data-target="#AlertQuestionTM" style="display: none;"></button>
    <div class="modal fade" id="AlertQuestionTM" tabindex="-1" role="dialog" style="top: 25%; outline: none;">
        <div class="modal-dialog modal-sm" role="document">
            <div class="alert alert-info text-center" style="-webkit-box-shadow: 0 5px 15px rgba(0, 0, 0, .5); box-shadow: 0 5px 15px rgba(0, 0, 0, .5);">
                <span class="glyphicon glyphicon-question-sign ico"></span>
                <br />
                <br />
                <p id="pModalQuestionTM" runat="server" class="alert-title">
                </p>
                <hr />
                <asp:Button ID="btnOkTM" runat="server" CssClass="btn btn-info" OnClick="btnOkTM_Click" Text="Aceptar"></asp:Button>
                <button id="btnTMCancelar" runat="server" class="btn btn-info" data-dismiss="modal">Cancelar</button>
            </div>
        </div>
    </div>

    <button id="btnQuestionTM2" type="button" data-toggle="modal" data-target="#AlertQuestionTM2" style="display: none;"></button>
    <div class="modal fade" id="AlertQuestionTM2" tabindex="-1" role="dialog" style="top: 25%; outline: none;">
        <div class="modal-dialog modal-sm" role="document">
            <div class="alert alert-info text-center" style="-webkit-box-shadow: 0 5px 15px rgba(0, 0, 0, .5); box-shadow: 0 5px 15px rgba(0, 0, 0, .5);">
                <span class="glyphicon glyphicon-question-sign ico"></span>
                <br />
                <br />
                <p id="pModalQuestionTM2" runat="server" class="alert-title">
                </p>
                <hr />
                <asp:Button ID="btnOkTM2" runat="server" CssClass="btn btn-info" OnClick="btnOkTM2_Click" Text="Aceptar"></asp:Button>
                <button id="btnTM2Cancelar" runat="server" class="btn btn-info" data-dismiss="modal">Cancelar</button>
            </div>
        </div>
    </div>

    <button id="btnQuestionS65" type="button" data-toggle="modal" data-target="#AlertQuestionS65" style="display: none;"></button>
    <div class="modal fade" id="AlertQuestionS65" tabindex="-1" role="dialog" style="top: 25%; outline: none;">
        <div class="modal-dialog modal-sm" role="document">
            <div class="alert alert-info text-center" style="-webkit-box-shadow: 0 5px 15px rgba(0, 0, 0, .5); box-shadow: 0 5px 15px rgba(0, 0, 0, .5);">
                <span class="glyphicon glyphicon-question-sign ico"></span>
                <br />
                <br />
                <p id="pModalQuestionS65" runat="server" class="alert-title">
                </p>
                <hr />
                <asp:Button ID="btnOkS65" runat="server" CssClass="btn btn-info" OnClick="btnOkS65_Click" Text="Aceptar"></asp:Button>
                <button id="btnS65Cancelar" runat="server" class="btn btn-info" data-dismiss="modal">Cancelar</button>
            </div>
        </div>
    </div>

    <button id="btnQuestionN65" type="button" data-toggle="modal" data-target="#AlertQuestionN65" style="display: none;"></button>
    <div class="modal fade" id="AlertQuestionN65" tabindex="-1" role="dialog" style="top: 25%; outline: none;">
        <div class="modal-dialog modal-sm" role="document">
            <div class="alert alert-info text-center" style="-webkit-box-shadow: 0 5px 15px rgba(0, 0, 0, .5); box-shadow: 0 5px 15px rgba(0, 0, 0, .5);">
                <span class="glyphicon glyphicon-question-sign ico"></span>
                <br />
                <br />
                <p id="pModalQuestionN65" runat="server" class="alert-title">
                </p>
                <hr />
                <asp:Button ID="btnOkN65" runat="server" CssClass="btn btn-info" OnClick="btnOkN65_Click" Text="Aceptar"></asp:Button>
                <button id="btnN65Cancelar" runat="server" class="btn btn-info" data-dismiss="modal">Cancelar</button>
            </div>
        </div>
    </div>

    <button id="btnQuestionOT" type="button" data-toggle="modal" data-target="#AlertQuestionOT" style="display: none;"></button>
    <div class="modal fade" id="AlertQuestionOT" tabindex="-1" role="dialog" style="top: 25%; outline: none;">
        <div class="modal-dialog modal-sm" role="document">
            <div class="alert alert-info text-center" style="-webkit-box-shadow: 0 5px 15px rgba(0, 0, 0, .5); box-shadow: 0 5px 15px rgba(0, 0, 0, .5);">
                <span class="glyphicon glyphicon-question-sign ico"></span>
                <br />
                <br />
                <p id="pModalQuestionOT" runat="server" class="alert-title">
                </p>
                <hr />
                <asp:Button ID="btnOkOT" runat="server" CssClass="btn btn-info" OnClick="btnOkOT_Click" Text="Aceptar"></asp:Button>
                <button id="btnOTCancelar" runat="server" class="btn btn-info" data-dismiss="modal">Cancelar</button>
            </div>
        </div>
    </div>

    <button id="btnQuestionOT2" type="button" data-toggle="modal" data-target="#AlertQuestionOT2" style="display: none;"></button>
    <div class="modal fade" id="AlertQuestionOT2" tabindex="-1" role="dialog" style="top: 25%; outline: none;">
        <div class="modal-dialog modal-sm" role="document">
            <div class="alert alert-info text-center" style="-webkit-box-shadow: 0 5px 15px rgba(0, 0, 0, .5); box-shadow: 0 5px 15px rgba(0, 0, 0, .5);">
                <span class="glyphicon glyphicon-question-sign ico"></span>
                <br />
                <br />
                <p id="pModalQuestionOT2" runat="server" class="alert-title">
                </p>
                <hr />
                <asp:Button ID="btnOkOT2" runat="server" CssClass="btn btn-info" OnClick="btnOkOT2_Click" Text="Aceptar"></asp:Button>
                <button id="btnOT2Cancelar" runat="server" class="btn btn-info" data-dismiss="modal">Cancelar</button>
            </div>
        </div>
    </div>

    <button id="btnQuestionIT" type="button" data-toggle="modal" data-target="#AlertQuestionIT" style="display: none;"></button>
    <div class="modal fade" id="AlertQuestionIT" tabindex="-1" role="dialog" style="top: 25%; outline: none;">
        <div class="modal-dialog modal-sm" role="document">
            <div class="alert alert-info text-center" style="-webkit-box-shadow: 0 5px 15px rgba(0, 0, 0, .5); box-shadow: 0 5px 15px rgba(0, 0, 0, .5);">
                <span class="glyphicon glyphicon-question-sign ico"></span>
                <br />
                <br />
                <p id="pModalQuestionIT" runat="server" class="alert-title">
                </p>
                <hr />
                <asp:Button ID="btnOkIT" runat="server" CssClass="btn btn-info" OnClick="btnOkIT_Click" Text="Aceptar"></asp:Button>
                <button id="btnITCancelar" runat="server" class="btn btn-info" data-dismiss="modal">Cancelar</button>
            </div>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptSection" runat="server">
</asp:Content>
