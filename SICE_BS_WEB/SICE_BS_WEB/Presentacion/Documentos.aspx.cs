using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;
using SICE_BS_WEB.Negocios;
using DevExpress.Web;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrintingLinks;
using DevExpress.Data.Filtering;
using System.IO;
using System.Drawing.Imaging;
using SICE_BS_WEB.WebReference;
using System.Text;
using System.Timers;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Linq;
using DevExpress.Web.Bootstrap;


namespace SICE_BS_WEB.Presentacion
{
    public partial class Documentos : System.Web.UI.Page
    {
        Inicio inicio = new Inicio();
        Perfiles perfiles = new Perfiles();
        Catalogos catalogo = new Catalogos();
        ControlExcepciones excepcion = new ControlExcepciones();
        static string nombreArchivo = string.Empty;
        static string tituloPagina = string.Empty;
        protected static string tituloPanel = string.Empty;
        static bool permisoConsultar = false;
        static bool permisoAgregar = false;
        static bool permisoExportar = false;        
        const string PageSizeSessionKey = "ed5e843d-cff7-47a7-815e-832923f7fb09";
        BoDocumentos docs = new BoDocumentos();
        BoNumeroParte np = new BoNumeroParte();

        //SE CAMBIA EL COLOR DEL TEMA DEFINIDO EN EL WEB CONFIG
        protected void Page_PreInit(object sender, EventArgs e)
        {
            (this.Master as Principal).ColoresMenuFooter();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["Cadena"] == null)
                {
                    //string alerta = "<script>alert('La sesión ha expirado, debe volver a iniciar sesión');window.location.href='Login.aspx'</script>";                    
                    //Response.Write(alerta);
                    Session["Tab"] = "Salir";
                    Response.Redirect("Login.aspx", false);
                    return;
                }
                else
                {
                    lblIdUsuario.Text = Session["IdUsuario"].ToString();
                    lblCadena.Text = Session["Cadena"].ToString();
                    Session["Tab"] = "Inicio";
                }


                if (!Page.IsPostBack)
                {
                    nombreArchivo = Request.Path.Substring(Request.Path.LastIndexOf("/") + 1);
                    if (Session["Permisos"] != null)
                    {
                        DataTable dt = ((DataTable)Session["Permisos"]).Select("Archivo =  '" + nombreArchivo + "'").CopyToDataTable();
                        tituloPagina = dt.Rows[0]["NombreModulo"].ToString();
                        permisoConsultar = Convert.ToBoolean(Convert.ToInt32(dt.Rows[0]["Consultar"].ToString()));
                        if (!permisoConsultar)
                            Response.Redirect("Default.aspx");
                        permisoExportar = Convert.ToBoolean(Convert.ToInt32(dt.Rows[0]["Exportar"].ToString()));
                        permisoAgregar = Convert.ToBoolean(Convert.ToInt32(dt.Rows[0]["Agregar"].ToString()));
                        Page.Title = tituloPagina;
                    }
                    TituloPanel(string.Empty);


                    //Consulta de entrada la fecha DESDE con el primer día del mes actual y el último día a la fecha HASTA
                    int v_mes = int.Parse(DateTime.Now.Month.ToString());
                    DateTime fecha1 = new DateTime(DateTime.Now.Year, v_mes, 1);
                    DateTime fecha2;
                    if (v_mes != 12)
                        fecha2 = new DateTime(DateTime.Now.Year, v_mes + 1, 1).AddDays(-1);
                    else
                        fecha2 = new DateTime(DateTime.Now.Year, v_mes, 31);

                    Session["DESDE"] = ASPx_Fecha_Desde.Text = DESDE.Text = fecha1.ToShortDateString();
                    Session["HASTA"] = ASPx_Fecha_Hasta.Text = HASTA.Text = fecha2.ToShortDateString();
                    CmbRango.Value = "Item2";

                    string mensaje = "";
                    DataTable dta = new DataTable();
                    dta = catalogo.TraerAduanas(lblCadena.Text, ref mensaje);
                    cmbAduana.DataSource = dta;
                    cmbAduana.DataBind();


                    DataTable dtcve = new DataTable();
                    dtcve = catalogo.TraerClaves(lblCadena.Text, ref mensaje);
                    cmbClave.DataSource = dtcve;
                    cmbClave.DataBind();

                    ////Trae información del mes actual                    
                    DataTable dtGrid = new DataTable();
                    //string mensaje = "";
                    //Session["GridD"] = null;
                    //dtGrid = docs.ConsultarDocumentos(fecha1, fecha2, "", "", "", "", "", lblCadena.Text, ref mensaje);
                    //if (dtGrid != null && dtGrid.Rows.Count > 0)
                    //{
                    //    Grid.DataSource = Session["GridD"] = dtGrid;
                    //    Grid.DataBind();
                    //    Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Hidden;
                    //    Grid.SettingsPager.PageSize = 20;
                    //}
                    //else
                    //{
                    Grid.DataSource = Session["GridD"] = dtGrid;
                    Grid.DataBind();
                    //Grid.Settings.VerticalScrollableHeight = 260;
                    //Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;    
                    //    AlertError("No hay información o intentelo de nuevo");
                    //}

                    //Traer todas las fechas de operación aduaneras
                    string mensaje2 = "";
                    Session["FECHAS"] = docs.ConsultarFechasOperacion(lblCadena.Text, ref mensaje2);
                    if (Session["FECHAS"] != null)
                    {
                        CreaSlideBar();
                    }

                    //Limpia grids del splitter inferior
                    LimpiarGridsSplitter();

                    //El foco en el control TxtPedimento
                    TxtPedimento.Focus();

                    //Validamos los permisos
                    var lkb_Excel = (LinkButton)Grid.Toolbars[0].Items.FindByName("Links").FindControl("lkb_Excel");

                    Session["permisoAgregar"] = permisoAgregar;
                    Session["permisoExportar"] = permisoExportar;

                    lkb_Excel.Visible = permisoExportar;
                    upc1.Enabled = permisoAgregar;
                    btnGuardar.Enabled = permisoAgregar;

                    SetColores();



                    //Llenar los combobox de Categorias
                    mensaje = "";
                    DataTable dtcateg = new DataTable();
                    //dtcateg = np.Trae_Categorias(Session["Cadena"].ToString(), ref mensaje);

                    xcbCategoria.DataSource = xcbCategoriaAll.DataSource = dtcateg;
                    xcbCategoria.DataBind();
                    xcbCategoriaAll.DataBind();
                }

                //Cada vez que se abra un pedimento por webservices entra aquí.
                string v_javascript = Request["__EVENTARGUMENT"];
                if (v_javascript != null && v_javascript.Contains("FilePedimento"))
                {
                    lkb_Actualizar_Click(null, null);
                    ActualizarGrid2();
                    v_javascript = null;
                }
            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (lblIdUsuario.Text.Length > 0)
                    idusuario = int.Parse(lblIdUsuario.Text);
                excepcion.RegistrarExcepcion(idusuario, "Documentos-Page_Load", ex, lblCadena.Text, ref mensaje);
                if (mensaje.Length == 0)
                    mensaje = "Error: " + excepcion.SerializarExMessage(lblCadena.Text, ex);
            }
        }

        protected void PermisosUsuario()
        {
            if (Session["permisoAgregar"] != null)
            {
                //Validamos los permisos                
                var lkb_Excel = (LinkButton)Grid.Toolbars[0].Items.FindByName("Links").FindControl("lkb_Excel");

                lkb_Excel.Visible = bool.Parse(Session["permisoExportar"].ToString());
                upc1.Enabled = bool.Parse(Session["permisoAgregar"].ToString());
                btnGuardar.Enabled = bool.Parse(Session["permisoAgregar"].ToString());
            }
        }

        private void TituloPanel(string descripcion)
        {
            h1_titulo.InnerText = tituloPanel = tituloPagina + descripcion;            
        }
        
        protected void BootstrapPageControl1_ActiveTabChanged(object source, DevExpress.Web.Bootstrap.BootstrapPageControlEventArgs e)
        {

        }

        //Metodo para seleccionar rango de fechas (Hoy, Mes Actual, Año Actual, Año pasado, 5 Años)
        protected void CmbRango_SelectedIndexChanged(object sender, EventArgs e)
        {
            DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            if (CmbRango.SelectedIndex != -1)
            {
                switch (CmbRango.SelectedItem.Value.ToString())
                {
                    case "Item1": //Hoy
                        //FechaActual para fecha inicial y final
                        DESDE.Text = fechaActual.ToShortDateString();
                        HASTA.Text = fechaActual.ToShortDateString();
                        break;
                    case "Item2": //Mes Actual
                        //Consulta el primer día del mes actual con el último día del mes actual
                        int v_mes = int.Parse(DateTime.Now.Month.ToString());
                        DateTime fecha1 = new DateTime(DateTime.Now.Year, v_mes, 1);
                        DateTime fecha2;
                        if (v_mes != 12)
                            fecha2 = new DateTime(DateTime.Now.Year, v_mes + 1, 1).AddDays(-1);
                        else
                            fecha2 = new DateTime(DateTime.Now.Year, v_mes, 31);
                        DESDE.Text = fecha1.ToShortDateString();
                        HASTA.Text = fecha2.ToShortDateString();
                        break;
                    case "Item3": //Año Actual
                        //Consulta fecha inicial de año actual y fecha final de año actual
                        DateTime fechaIniAnio = new DateTime(DateTime.Now.Year, 1, 1);
                        DateTime fechaFinAnio = new DateTime(DateTime.Now.Year, 12, 31);
                        DESDE.Text = fechaIniAnio.ToShortDateString();
                        HASTA.Text = fechaFinAnio.ToShortDateString();
                        break;
                    case "Item4"://Año pasado
                        //Consulta fecha inicial de año pasado y fecha final de año pasado
                        DateTime fechaIniAnioPasado = new DateTime(DateTime.Now.Year - 1, 1, 1);
                        DateTime fechaFinAnioPasado = new DateTime(DateTime.Now.Year - 1, 12, 31);
                        DESDE.Text = fechaIniAnioPasado.ToShortDateString();
                        HASTA.Text = fechaFinAnioPasado.ToShortDateString();
                        break;
                    case "Item5"://5 Años
                        //Fecha inicial día actual hace 5 años y fecha final es fecha actual
                        DateTime fechaIniAnioPasadoCinco = new DateTime(DateTime.Now.Year - 5, DateTime.Now.Month, DateTime.Now.Day);
                        DESDE.Text = fechaIniAnioPasadoCinco.ToShortDateString();
                        HASTA.Text = fechaActual.ToShortDateString();
                        break;
                }
            }
        }        

        private void CreaSlideBar()
        {
            try
            {

                //Valida si la session tiene información, sino salir de session
                if (Session["FECHAS"] == null)
                {
                    Response.Redirect("Login.aspx");
                    return;
                }

                //Carga el Grid2 del datatable de Session["FECHAS"]
                Grid2.DataSource = ((DataTable)Session["FECHAS"]);
                Grid2.DataBind();

                ////Carga dinamicamente el div divSlide
                //string mensaje = string.Empty;
                //LiteralControl literal = new LiteralControl();
                //int contador = 1;                
                //if (Session["FECHAS"] != null)
                //{
                //    const string quote = "\"";
                //    literal.Text += @"<ul>";
                //    foreach (DataRow renglon in ((DataTable) Session["FECHAS"]).Rows)
                //    {
                //        string mes = renglon["MES"].ToString();
                //        mes = mes.Length == 1 ? "0" + mes : mes;
                //        literal.Text += @"<li><a href=" + quote + " " + quote + " class=" + quote + "glyphicon glyphicon-search" + quote + "></a>" + 
                //                               renglon["ANIO"].ToString() + " - " + mes + " </li>";
                //        //literal.Text += @"<li><dx:BootstrapButton ID=" + quote + "btnFechas" + contador + "" + quote + " runat=" + quote + "server" + quote + " OnClick=" + quote + "btnFechas_OnClick" + quote + " " +
                //        //                 "SettingsBootstrap-RenderOption=" + quote + "Primary" + quote + " SettingsBootstrap-Sizing=" + quote + "Small" + quote + " >   " +
                //        //                 "<ClientSideEvents Click=" + quote + "function(s, e) { Callback.PerformCallback(); LoadingPanel1.Show(); }" + quote + " " +
                //        //                 "                  Init=" + quote + "function(s, e) {LoadingPanel1.Hide();}" + quote + " /> " +
                //        //                 "<CssClasses Icon=" + quote + "glyphicon glyphicon-search" + quote + " /> " +
                //        //                 "</dx:BootstrapButton>&nbsp;&nbsp;" + renglon["ANIO"].ToString() + " - " + mes + " </li>";
                //        contador++;
                //    }
                //    literal.Text += "</ul>";
                //    divSlide.Controls.Add(new LiteralControl(literal.Text));
                //}
            }
            catch (Exception ex){
                string mensaje = string.Empty;
                int idusuario = 0;
                if (lblIdUsuario.Text.Length > 0)
                    idusuario = int.Parse(lblIdUsuario.Text);
                excepcion.RegistrarExcepcion(idusuario, "Documentos-CreaSlideBar", ex, lblCadena.Text, ref mensaje);
            }
        }

        //Botones Buscar en Grid2 por Año y Mes 
        protected void btnBuscar_Command(object sender, CommandEventArgs e)
        {
            //Busca los rangos de fecha
            int v_anio = int.Parse(e.CommandName.ToString());
            int v_mes = int.Parse(e.CommandArgument.ToString());

            DateTime fecha1 = new DateTime(v_anio, v_mes, 1);
            DateTime fecha2;
            if (v_mes != 12)
                fecha2 = new DateTime(v_anio, v_mes + 1, 1).AddDays(-1);
            else
                fecha2 = new DateTime(v_anio, v_mes, 31);

            Session["DESDE"] = ASPx_Fecha_Desde.Text = DESDE.Text = fecha1.ToShortDateString();
            Session["HASTA"] = ASPx_Fecha_Hasta.Text = HASTA.Text = fecha2.ToShortDateString();
            CmbRango.Value = null;//"Item2";

            //Se limpian los demás registros
            TxtPedimento.Text = cmbClave.Text = cmbAduana.Text = TxtTexto.Text = string.Empty;
            //TxtComodines.Text = string.Empty;

            //Ejecuta buscar información de lo rangos de fecha dados anteriormente 
            btnBuscar_OnClick(null, null);
            //Limpiar filtros
            lkb_LimpiarFiltros_Click(null, null);

            //Solo un registro se puede expandir y se cierran todas
            Grid.SettingsDetail.AllowOnlyOneMasterRowExpanded = true;
            if (Grid.SettingsDetail.AllowOnlyOneMasterRowExpanded)
            {
                Grid.DetailRows.CollapseAllRows();
            }
        }

        //Botón Buscar
        protected void btnBuscar_OnClick(object sender, EventArgs e)
        {
            try
            {
                LoadingPanel1.ContainerElementID = "Panel1";

                Session["DESDE"] = ASPx_Fecha_Desde.Text = DESDE.Text;
                Session["HASTA"] = ASPx_Fecha_Hasta.Text = HASTA.Text;

                //DataTable dt = new DataTable();

                ////Obtener valores de fechas
                //DateTime? dateDesde = string.IsNullOrEmpty(DESDE.Text) ? (DateTime?)null : DateTime.Parse(DESDE.Text);
                //DateTime? dateHasta = string.IsNullOrEmpty(HASTA.Text) ? (DateTime?)null : DateTime.Parse(HASTA.Text);

                ////Validar fechas que existan
                //if ((dateDesde == null || dateHasta == null))
                //{
                //    AlertError("Debe seleccionar un rango de fechas validas");
                //    return;
                //}

                ////Validar fechas rangos correctos
                //if ((dateDesde != null && dateHasta != null) && dateDesde > dateHasta)
                //{
                //    AlertError("La fecha Desde no puede ser mayor a la fecha Hasta");
                //    return;
                //}

                //System.Threading.Thread.Sleep(3000);

                string mensaje = "";
                BoDocumentos docs = new BoDocumentos();
                DataTable dt = new DataTable();
                DateTime? dateDesde = string.IsNullOrEmpty(DESDE.Text) ? (DateTime?)null : DateTime.Parse(DESDE.Text);
                DateTime? dateHasta = string.IsNullOrEmpty(HASTA.Text) ? (DateTime?)null : DateTime.Parse(HASTA.Text);
                
                //El parametro después de TxtTexto.Text es TxtComodines.Text que ya no se usa
                dt = docs.ConsultarDocumentos(dateDesde, dateHasta, TxtPedimento.Text.Trim(), cmbClave.Text.Trim(),
                                              cmbAduana.Text.Trim(), TxtTexto.Text.Trim(), "", lblCadena.Text, ref mensaje);
                if (dt != null && dt.Rows.Count > 0)
                {
                    Grid.DataSource = Session["GridD"] = dt;
                    Grid.DataBind();
                    //Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Hidden;
                    //Grid.Settings.VerticalScrollableHeight = 260;
                    //Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                    Grid.SettingsPager.PageSize = 20;
                    Grid.DetailRows.ExpandRow(1);

                    //Solo un registro se puede expandir y se cierran todas
                    Grid.SettingsDetail.AllowOnlyOneMasterRowExpanded = true;
                    if (Grid.SettingsDetail.AllowOnlyOneMasterRowExpanded)
                    {
                        Grid.DetailRows.CollapseAllRows();
                    }
                }
                else
                {
                    Grid.DataSource = Session["GridD"] = dt;
                    Grid.DataBind();
                    AlertError("No hay información o intentelo de nuevo");
                }                

                //Limpia grids del splitter inferior
                LimpiarGridsSplitter();

                //Actualiza los permisos
                PermisosUsuario();
            }
            catch (Exception ex){
                string mensaje = string.Empty;
                int idusuario = 0;
                if (lblIdUsuario.Text.Length > 0)
                    idusuario = int.Parse(lblIdUsuario.Text);
                excepcion.RegistrarExcepcion(idusuario, "Documentos-btnBuscar_OnClick", ex, lblCadena.Text, ref mensaje);
            }
        }

        //Botón Limpiar
        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            try
            {
                TxtPedimento.Text = string.Empty;
                cmbAduana.SelectedIndex = -1;
                cmbClave.SelectedIndex = -1;
                TxtTexto.Text = string.Empty;
                //TxtComodines.Text = string.Empty;
                CmbRango.SelectedIndex = -1;
                DESDE.Text = string.Empty;
                HASTA.Text = string.Empty;
            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (lblIdUsuario.Text.Length > 0)
                    idusuario = int.Parse(lblIdUsuario.Text);
                excepcion.RegistrarExcepcion(idusuario, "Documentos-btnLimpiar_Click", ex, lblCadena.Text, ref mensaje);
            }
        }


        //Evento que entra antes de mostrar los datos en Grid,
        //muestra diferentes imagenes para el botón de "btnCG" ver si se habilita o no.
        protected void Grid_CustomButtonInitialize(object sender, ASPxGridViewCustomButtonEventArgs e)
        {            
            int renglon = e.VisibleIndex;
            ASPxGridView grid = (ASPxGridView)sender;
            if (e.ButtonID == "btnCG")
            {
                e.Image.Url = "../img/iconos/ico_cug.png";
                e.Image.ToolTip = "Cuenta de Gastos";

                if (Session["GridD"] != null)
                {
                    if (((DataTable)Session["GridD"]).Rows[e.VisibleIndex]["CG"].ToString().Equals("0"))
                    {
                        e.Image.Url = "../img/iconos/x_reloj.png";
                        e.Image.ToolTip = "Sin Cuenta de Gastos";
                    }

                    else if (((DataTable)Session["GridD"]).Rows[e.VisibleIndex]["CG"].ToString().Equals("1"))
                    {
                        e.Image.Url = "../img/iconos/x_guion.png";
                        e.Image.ToolTip = "Con Cuenta de Gastos";
                    }

                    else if (((DataTable)Session["GridD"]).Rows[e.VisibleIndex]["CG"].ToString().Equals("2"))
                    {
                        e.Image.Url = "../img/iconos/x_edit.png";
                        e.Image.ToolTip = "Revisado";
                    }

                    else if (((DataTable)Session["GridD"]).Rows[e.VisibleIndex]["CG"].ToString().Equals("3"))
                    {
                        e.Image.Url = "../img/iconos/x_paloma.png";
                        e.Image.ToolTip = "Autorizado";
                    }

                    else if (((DataTable)Session["GridD"]).Rows[e.VisibleIndex]["CG"].ToString().Equals("4"))
                    {
                        e.Image.Url = "../img/iconos/x_exis.png";
                        e.Image.ToolTip = "Rechazado";
                    }

                    //SE BORRA ES PARA VER SOLO LOS COLORES DEL BOTÓN 
                    //if (((DataTable)Session["GridD"]).Rows[e.VisibleIndex]["PEDIMENTOARMADO"].ToString().Equals("470-1714-8003066"))
                    //{
                    //    e.Image.Url = "../img/iconos/ico_cu_azul.png";
                    //    e.Image.ToolTip = "No tiene";
                    //    //e.Column.CellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#6facf7");
                    //}
                    //else if (((DataTable)Session["GridD"]).Rows[e.VisibleIndex]["PEDIMENTOARMADO"].ToString().Equals("470-1714-8002579"))
                    //{
                    //    e.Image.Url = "../img/iconos/ico_cu_rojo.png";
                    //    e.Image.ToolTip = "Si tiene con error";
                    //    //e.Column.CellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#ec1717");
                    //}
                    //else if (((DataTable)Session["GridD"]).Rows[e.VisibleIndex]["PEDIMENTOARMADO"].ToString().Equals("400-3890-8001385"))
                    //{
                    //    e.Image.Url = "../img/iconos/ico_cu_amarillo.png";
                    //    e.Image.ToolTip = "Si tiene sin validar";
                    //    //e.Column.CellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#f6f904");
                    //}
                    //else if (((DataTable)Session["GridD"]).Rows[e.VisibleIndex]["PEDIMENTOARMADO"].ToString().Equals("400-3890-8001384"))
                    //{
                    //    e.Image.Url = "../img/iconos/ico_cu_verde.png";
                    //    e.Image.ToolTip = "Ok";
                    //    //e.Column.CellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#74fa08");
                    //}


                    //else if (((DataTable)Session["GridD"]).Rows[e.VisibleIndex]["PEDIMENTOARMADO"].ToString().Equals("400-3890-8001383"))
                    //{
                    //    e.Image.Url = "../img/iconos/ico_cug_azul.png";
                    //    e.Image.ToolTip = "No tiene";
                    //    //e.Column.CellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#6facf7");
                    //}
                    //else if (((DataTable)Session["GridD"]).Rows[e.VisibleIndex]["PEDIMENTOARMADO"].ToString().Equals("400-3890-8001381"))
                    //{
                    //    e.Image.Url = "../img/iconos/ico_cug_rojo.png";
                    //    e.Image.ToolTip = "Si tiene con error";
                    //    //e.Column.CellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#ec1717");
                    //}
                    //else if (((DataTable)Session["GridD"]).Rows[e.VisibleIndex]["PEDIMENTOARMADO"].ToString().Equals("400-3890-8001382"))
                    //{
                    //    e.Image.Url = "../img/iconos/ico_cug_amarillo.png";
                    //    e.Image.ToolTip = "Si tiene sin validar";
                    //    //e.Column.CellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#f6f904");
                    //}
                    //else if (((DataTable)Session["GridD"]).Rows[e.VisibleIndex]["PEDIMENTOARMADO"].ToString().Equals("400-3890-8001380"))
                    //{
                    //    e.Image.Url = "../img/iconos/ico_cug_verde.png";
                    //    e.Image.ToolTip = "Ok";
                    //    //e.Column.CellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#74fa08");
                    //}


                    //else if (((DataTable)Session["GridD"]).Rows[e.VisibleIndex]["PEDIMENTOARMADO"].ToString().Equals("400-3890-8001378"))
                    //{
                    //    e.Image.Url = "../img/iconos/ico_cug_azul1.png";
                    //    e.Image.ToolTip = "No tiene";
                    //    //e.Column.CellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#6facf7");
                    //}
                    //else if (((DataTable)Session["GridD"]).Rows[e.VisibleIndex]["PEDIMENTOARMADO"].ToString().Equals("240-3897-8501968"))
                    //{
                    //    e.Image.Url = "../img/iconos/ico_cug_rojo1.png";
                    //    e.Image.ToolTip = "Si tiene con error";
                    //    //e.Column.CellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#ec1717");
                    //}
                    //else if (((DataTable)Session["GridD"]).Rows[e.VisibleIndex]["PEDIMENTOARMADO"].ToString().Equals("240-3897-8501963"))
                    //{
                    //    e.Image.Url = "../img/iconos/ico_cug_amarillo1.png";
                    //    e.Image.ToolTip = "Si tiene sin validar";
                    //    //e.Column.CellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#f6f904");
                    //}
                    //else if (((DataTable)Session["GridD"]).Rows[e.VisibleIndex]["PEDIMENTOARMADO"].ToString().Equals("240-3897-8501919"))
                    //{
                    //    e.Image.Url = "../img/iconos/ico_cug_verde1.png";
                    //    e.Image.ToolTip = "Ok";
                    //    //e.Column.CellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#74fa08");
                    //}
                }

                //else
                //{
                //    e.Image.Url = "../img/iconos/ico_cug.png";
                //    e.Image.ToolTip = "Cuenta de Gastos";
                //    //e.Column.CellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFF"); 
                //}

                    //if (((DataTable)Session["GridD"]).Rows[e.VisibleIndex]["Prueba"].ToString().Contains("1"))
                    //{
                    //  e.Visible = DevExpress.Utils.DefaultBoolean.False;
                    //e.Image.Url = "../img/iconos/ico_cug.png";

                //}
                //else
                //{
                //  e.Image.Url = "../img/iconos/ico_cg1.png";
                //}
            }

            //Actualiza los permisos
            PermisosUsuario();
        }

        #region GridPageSize

        #region Grid Principal
        protected int GridPageSize
        {
            get
            {
                if (Session[PageSizeSessionKey] == null)
                    return Grid.SettingsPager.PageSize;
                return (int)Session[PageSizeSessionKey];
            }
            set { Session[PageSizeSessionKey] = value; }
        }
        #endregion

        #region Expediente Digital
        protected int GridPageSizeDocumentos
        {
            get
            {
                if (Session[PageSizeSessionKey] == null)
                    return detailGridDocumentos.SettingsPager.PageSize;
                return (int)Session[PageSizeSessionKey];
            }
            set { Session[PageSizeSessionKey] = value; }
        }
        #endregion

        #region Cuenta de Gastos
        protected int GridPageSizeCuentaGastos
        {
            get
            {
                if (Session[PageSizeSessionKey] == null)
                    return detailGridCG.SettingsPager.PageSize;
                return (int)Session[PageSizeSessionKey];
            }
            set { Session[PageSizeSessionKey] = value; }
        }
        #endregion

        #region Ver Encabezado

        #region Identificadores
        protected int GridPageSizeGridIdentificadores
        {
            get
            {
                if (Session[PageSizeSessionKey] == null)
                    return detailGridIdentificadores.SettingsPager.PageSize;
                return (int)Session[PageSizeSessionKey];
            }
            set { Session[PageSizeSessionKey] = value; }
        }
        #endregion
        
        #region Fletes
        protected int GridPageSizeGridFletes
        {
            get
            {
                if (Session[PageSizeSessionKey] == null)
                    return detailGridFletes.SettingsPager.PageSize;
                return (int)Session[PageSizeSessionKey];
            }
            set { Session[PageSizeSessionKey] = value; }
        }
        #endregion

        #region Incrementables
        protected int GridPageSizeIn
        {
            get
            {
                if (Session[PageSizeSessionKey] == null)
                    return GridIn.SettingsPager.PageSize;
                return (int)Session[PageSizeSessionKey];
            }
            set { Session[PageSizeSessionKey] = value; }
        }
        #endregion

        #region Facturas
        protected int GridPageSizeFacturas
        {
            get
            {
                if (Session[PageSizeSessionKey] == null)
                    return GridFacturas.SettingsPager.PageSize;
                return (int)Session[PageSizeSessionKey];
            }
            set { Session[PageSizeSessionKey] = value; }
        }
        #endregion

        #region Fechas
        protected int GridPageSizeFechas
        {
            get
            {
                if (Session[PageSizeSessionKey] == null)
                    return GridFechas.SettingsPager.PageSize;
                return (int)Session[PageSizeSessionKey];
            }
            set { Session[PageSizeSessionKey] = value; }
        }
        #endregion

        #region Contribuciones Generales
        protected int GridPageSizeContriGrales
        {
            get
            {
                if (Session[PageSizeSessionKey] == null)
                    return GridContriGrales.SettingsPager.PageSize;
                return (int)Session[PageSizeSessionKey];
            }
            set { Session[PageSizeSessionKey] = value; }
        }
        #endregion

        #region Impuestos
        protected int GridPageSizeImpuestos
        {
            get
            {
                if (Session[PageSizeSessionKey] == null)
                    return GridImpuestos.SettingsPager.PageSize;
                return (int)Session[PageSizeSessionKey];
            }
            set { Session[PageSizeSessionKey] = value; }
        }
        #endregion

        #region Transporte
        protected int GridPageSizeTransporte
        {
            get
            {
                if (Session[PageSizeSessionKey] == null)
                    return GridTransporte.SettingsPager.PageSize;
                return (int)Session[PageSizeSessionKey];
            }
            set { Session[PageSizeSessionKey] = value; }
        }
        #endregion

        #region Guias
        protected int GridPageSizeGuias
        {
            get
            {
                if (Session[PageSizeSessionKey] == null)
                    return GridGuias.SettingsPager.PageSize;
                return (int)Session[PageSizeSessionKey];
            }
            set { Session[PageSizeSessionKey] = value; }
        }
        #endregion

        #region Contenedores
        protected int GridPageSizeContenedores
        {
            get
            {
                if (Session[PageSizeSessionKey] == null)
                    return GridContenedores.SettingsPager.PageSize;
                return (int)Session[PageSizeSessionKey];
            }
            set { Session[PageSizeSessionKey] = value; }
        }
        #endregion

        #region Descargos
        protected int GridPageSizeDescargos
        {
            get
            {
                if (Session[PageSizeSessionKey] == null)
                    return GridDescargos.SettingsPager.PageSize;
                return (int)Session[PageSizeSessionKey];
            }
            set { Session[PageSizeSessionKey] = value; }
        }
        #endregion

        #region Rectificaciones
        protected int GridPageSizeRectificaciones
        {
            get
            {
                if (Session[PageSizeSessionKey] == null)
                    return GridRectificaciones.SettingsPager.PageSize;
                return (int)Session[PageSizeSessionKey];
            }
            set { Session[PageSizeSessionKey] = value; }
        }
        #endregion

        #endregion

        #region Partidas Ver Detalle

        protected int GridPageSizeGridPartidaGravamen
        {
            get
            {
                if (Session[PageSizeSessionKey] == null)
                    return GridPartidaGravamen.SettingsPager.PageSize;
                return (int)Session[PageSizeSessionKey];
            }
            set { Session[PageSizeSessionKey] = value; }
        }

        protected int GridPageSizeGridPartidaTasas
        {
            get
            {
                if (Session[PageSizeSessionKey] == null)
                    return GridPartidaTasas.SettingsPager.PageSize;
                return (int)Session[PageSizeSessionKey];
            }
            set { Session[PageSizeSessionKey] = value; }
        }

        protected int GridPageSizeGridPartidaIdentificadores
        {
            get
            {
                if (Session[PageSizeSessionKey] == null)
                    return GridPartidaIdentificadores.SettingsPager.PageSize;
                return (int)Session[PageSizeSessionKey];
            }
            set { Session[PageSizeSessionKey] = value; }
        }

        protected int GridPageSizeGridPartidaRegulaciones
        {
            get
            {
                if (Session[PageSizeSessionKey] == null)
                    return GridPartidaRegulaciones.SettingsPager.PageSize;
                return (int)Session[PageSizeSessionKey];
            }
            set { Session[PageSizeSessionKey] = value; }
        }

        protected int GridPageSizeGridPartidaCodigos
        {
            get
            {
                if (Session[PageSizeSessionKey] == null)
                    return GridPartidaCodigos.SettingsPager.PageSize;
                return (int)Session[PageSizeSessionKey];
            }
            set { Session[PageSizeSessionKey] = value; }
        }

        #endregion

        #region Numero de Parte

        protected int GridPageSizeNP
        {
            get
            {
                if (Session[PageSizeSessionKey] == null)
                    return GridNP.SettingsPager.PageSize;
                return (int)Session[PageSizeSessionKey];
            }
            set { Session[PageSizeSessionKey] = value; }
        }

        protected int GridPageSizeNP2
        {
            get
            {
                if (Session[PageSizeSessionKey] == null)
                    return GridNP2.SettingsPager.PageSize;
                return (int)Session[PageSizeSessionKey];
            }
            set { Session[PageSizeSessionKey] = value; }
        }

        protected int GridPageSizeNP3
        {
            get
            {
                if (Session[PageSizeSessionKey] == null)
                    return GridNP3.SettingsPager.PageSize;
                return (int)Session[PageSizeSessionKey];
            }
            set { Session[PageSizeSessionKey] = value; }
        }

        #endregion

        #endregion

        protected void Page_Init(object sender, EventArgs e)
        {
            Grid.SettingsPager.PageSize = GridPageSize;

            #region Grid Principal

            //Cuando se quiera filtrar el Grid entra en el if
            if (Session["GridD"] != null)
            {
                Grid.DataSource = Session["GridD"];
                Grid.DataBind();
                Grid.SettingsPager.PageSize = GridPageSize;
            }

            #endregion

            #region Cuenta Gastos

            //Cuando se quiera filtrar el detailGridCG entra en el if
            if (Session["GridCuentaGastos"] != null)
            {
                detailGridCG.DataSource = Session["GridCuentaGastos"];
                detailGridCG.DataBind();
                detailGridCG.SettingsPager.PageSize = GridPageSizeCuentaGastos;
            }

            #endregion

            #region Expediente Digital

            //Cuando se quiera filtrar el detailGridDocumentos entra en el if
            if (Session["GridDocumentos"] != null)
            {
                detailGridDocumentos.DataSource = Session["GridDocumentos"];
                detailGridDocumentos.DataBind();              
                detailGridDocumentos.SettingsPager.PageSize = GridPageSizeDocumentos;                
            }

            #endregion

            #region Ver Encabezado

            //Cuando se quiera filtrar el GridIdentificadores entra en el if
            if (Session["GridIdentificadores"] != null)
            {
                detailGridIdentificadores.DataSource = Session["GridIdentificadores"];
                detailGridIdentificadores.DataBind();
                detailGridIdentificadores.SettingsPager.PageSize = GridPageSizeGridIdentificadores;
            }

            //Cuando se quiera filtrar el GridFletes entra en el if
            if (Session["GridFletes"] != null)
            {
                detailGridFletes.DataSource = Session["GridFletes"];
                detailGridFletes.DataBind();
                detailGridFletes.SettingsPager.PageSize = GridPageSizeGridFletes;
            }

            //Cuando se quiera filtrar el GridIn entra en el if
            if (Session["GridIn"] != null)
            {
                GridIn.DataSource = Session["GridIn"];
                GridIn.DataBind();
                GridIn.SettingsPager.PageSize = GridPageSizeIn;
            }

            //Cuando se quiera filtrar el GridFacturas entra en el if
            if (Session["GridFacturas"] != null)
            {
                GridFacturas.DataSource = Session["GridFacturas"];
                GridFacturas.DataBind();
                GridFacturas.SettingsPager.PageSize = GridPageSizeFacturas;
            }

            //Cuando se quiera filtrar el GridFechas entra en el if
            if (Session["GridFechas"] != null)
            {
                GridFechas.DataSource = Session["GridFechas"];
                GridFechas.DataBind();
                GridFechas.SettingsPager.PageSize = GridPageSizeFechas;
            }

            //Cuando se quiera filtrar el GridFechas entra en el if
            if (Session["GridContriGrales"] != null)
            {
                GridContriGrales.DataSource = Session["GridContriGrales"];
                GridContriGrales.DataBind();
                GridContriGrales.SettingsPager.PageSize = GridPageSizeContriGrales;
            }

            //Cuando se quiera filtrar el GridImpuestos entra en el if
            if (Session["GridImpuestos"] != null)
            {
                GridImpuestos.DataSource = Session["GridImpuestos"];
                GridImpuestos.DataBind();
                GridImpuestos.SettingsPager.PageSize = GridPageSizeImpuestos;
            }

            //Cuando se quiera filtrar el GridTransporte entra en el if
            if (Session["GridTransporte"] != null)
            {
                GridTransporte.DataSource = Session["GridTransporte"];
                GridTransporte.DataBind();
                GridTransporte.SettingsPager.PageSize = GridPageSizeTransporte;
            }

            //Cuando se quiera filtrar el GridGuias entra en el if
            if (Session["GridGuias"] != null)
            {
                GridGuias.DataSource = Session["GridGuias"];
                GridGuias.DataBind();
                GridGuias.SettingsPager.PageSize = GridPageSizeGuias;
            }

            //Cuando se quiera filtrar el GridContenedores entra en el if
            if (Session["GridContenedores"] != null)
            {
                GridContenedores.DataSource = Session["GridContenedores"];
                GridContenedores.DataBind();
                GridContenedores.SettingsPager.PageSize = GridPageSizeContenedores;
            }

            //Cuando se quiera filtrar el GridDescargos entra en el if
            if (Session["GridDescargos"] != null)
            {
                GridDescargos.DataSource = Session["GridDescargos"];
                GridDescargos.DataBind();
                GridDescargos.SettingsPager.PageSize = GridPageSizeDescargos;
            }

            //Cuando se quiera filtrar el GridRectificaciones entra en el if
            if (Session["GridRectificaciones"] != null)
            {
                GridRectificaciones.DataSource = Session["GridRectificaciones"];
                GridRectificaciones.DataBind();
                GridRectificaciones.SettingsPager.PageSize = GridPageSizeRectificaciones;
            }

            #endregion

            #region Paritdas Ver Detalle

            //Cuando se quiera filtrar el GridPartidaGravamen entra en el if
            if (Session["GridPartidaGravamen"] != null)
            {
                GridPartidaGravamen.DataSource = Session["GridPartidaGravamen"];
                GridPartidaGravamen.DataBind();
                GridPartidaGravamen.SettingsPager.PageSize = GridPageSizeGridPartidaGravamen;
            }

            //Cuando se quiera filtrar el GridPartidaTasas entra en el if
            if (Session["GridPartidaTasas"] != null)
            {
                GridPartidaTasas.DataSource = Session["GridPartidaTasas"];
                GridPartidaTasas.DataBind();
                GridPartidaTasas.SettingsPager.PageSize = GridPageSizeGridPartidaTasas;
            }

            //Cuando se quiera filtrar el GridPartidaIdentificadores entra en el if
            if (Session["GridPartidaIdentificadores"] != null)
            {
                GridPartidaIdentificadores.DataSource = Session["GridPartidaIdentificadores"];
                GridPartidaIdentificadores.DataBind();
                GridPartidaIdentificadores.SettingsPager.PageSize = GridPageSizeGridPartidaIdentificadores;
            }

            //Cuando se quiera filtrar el GridPartidaRegulaciones entra en el if
            if (Session["GridPartidaRegulaciones"] != null)
            {
                GridPartidaRegulaciones.DataSource = Session["GridPartidaRegulaciones"];
                GridPartidaRegulaciones.DataBind();
                GridPartidaRegulaciones.SettingsPager.PageSize = GridPageSizeGridPartidaRegulaciones;
            }

            //Cuando se quiera filtrar el GridPartidaCodigos entra en el if
            if (Session["GridPartidaCodigos"] != null)
            {
                GridPartidaCodigos.DataSource = Session["GridPartidaCodigos"];
                GridPartidaCodigos.DataBind();
                GridPartidaCodigos.SettingsPager.PageSize = GridPageSizeGridPartidaCodigos;
            }

            #endregion

            //Para cada columna en el Grid se define modo de filtro CheckedList 
            //foreach (GridViewDataColumn column in Grid.Columns)
            //    column.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;

            #region Número de Parte

            //Cuando se quiera filtrar el GridNP entra en el if
            if (Session["GridNP"] != null)
            {
                GridNP.DataSource = Session["GridNP"];
                GridNP.DataBind();
                GridNP.SettingsPager.PageSize = GridPageSizeNP;
            }

            //Cuando se quiera filtrar el GridNP2 entra en el if
            if (Session["GridNP2"] != null)
            {
                GridNP2.DataSource = Session["GridNP2"];
                GridNP2.DataBind();
                GridNP2.SettingsPager.PageSize = GridPageSizeNP2;
            }

            //Cuando se quiera filtrar el GridNP3 entra en el if
            if (Session["GridNP3"] != null)
            {
                GridNP3.DataSource = Session["GridNP3"];
                GridNP3.DataBind();
                GridNP3.SettingsPager.PageSize = GridPageSizeNP3;
            }

            #endregion
        }

        protected void callback_Callback(object source, CallbackEventArgs e)
        {           
            System.Threading.Thread.Sleep(1000);
            btnBuscar_OnClick(null,null);
        }

        //Se usaba para traer en la session de pedimento su valor 
        //protected void VerticalGrid_BeforePerformDataSelect(object sender, EventArgs e)
        //{
        //    ASPxVerticalGrid vg = sender as ASPxVerticalGrid;
        //    Session["PEDIMENTOARMADO"] = ASPxGridView.GetDetailRowKeyValue(vg).ToString();
        //    ASPx_Pedimento.Text = Session["PEDIMENTOARMADO"].ToString(); 
        //}

        //Se usaba para cuando se expandia el Grid
        //protected void Grid_DetailRowExpandedChanged(object sender, ASPxGridViewDetailRowEventArgs e)
        //{
        //    if (e.Expanded)
        //    {
        //        dynamic keyValue = Grid.GetRowValues(e.VisibleIndex, Grid.KeyFieldName);
        //        string v_pedimento = keyValue;

        //        BoDocumentos documentos = new BoDocumentos();
        //        DataTable dt = new DataTable();
        //        string mensaje = "";

        //        #region PARTIDAS

        //        dt = new DataTable();
        //        dt = documentos.ConsultarPartidas(v_pedimento, lblCadena.Text, ref mensaje);
        //        GridPartidas.DataSource = Session["GridPartidas"] = dt;
        //        GridPartidas.DataBind();

        //        #endregion

        //        #region INCREMENTABLES

        //        dt = new DataTable();
        //        dt = documentos.ConsultarIncrementables(v_pedimento, lblCadena.Text, ref mensaje);
        //        GridIn.DataSource = Session["GridIn"] = dt;
        //        GridIn.DataBind();

        //        #endregion

        //        #region FACTURAS

        //        dt = new DataTable();
        //        dt = documentos.ConsultarFacturas(v_pedimento, lblCadena.Text, ref mensaje);
        //        GridFacturas.DataSource = Session["GridFacturas"] = dt;
        //        GridFacturas.DataBind();

        //        //Para cada columna en el GridFacturas se define modo de filtro CheckedList 
        //        foreach (GridViewDataColumn column in GridFacturas.Columns)
        //            column.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;

        //        #endregion

        //        #region FECHAS

        //        dt = new DataTable();
        //        dt = documentos.ConsultarFechas(v_pedimento, lblCadena.Text, ref mensaje);
        //        GridFechas.DataSource = Session["GridFechas"] = dt;
        //        GridFechas.DataBind();

        //        //Para cada columna en el GridFechas se define modo de filtro CheckedList 
        //        foreach (GridViewDataColumn column in GridFechas.Columns)
        //            column.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;

        //        #endregion

        //        #region CONTRIBUCIONES GENERALES

        //        dt = new DataTable();
        //        dt = documentos.ConsultarContribucionesGenerales(v_pedimento, lblCadena.Text, ref mensaje);
        //        GridContriGrales.DataSource = Session["GridContenedores"] = dt;
        //        GridContriGrales.DataBind();

        //        //Para cada columna en el GridContriGrales se define modo de filtro CheckedList 
        //        foreach (GridViewDataColumn column in GridContriGrales.Columns)
        //            column.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;

        //        #endregion

        //        #region IMPUESTOS

        //        dt = new DataTable();
        //        dt = documentos.ConsultarImpuestos(v_pedimento, lblCadena.Text, ref mensaje);
        //        GridImpuestos.DataSource = Session["GridImpuestos"] = dt;
        //        GridImpuestos.DataBind();

        //        //Para cada columna en el GridImpuestos se define modo de filtro CheckedList 
        //        foreach (GridViewDataColumn column in GridImpuestos.Columns)
        //            column.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;

        //        #endregion

        //        #region TRANSPORTE

        //        dt = new DataTable();
        //        dt = documentos.ConsultarTransporte(v_pedimento, lblCadena.Text, ref mensaje);
        //        GridTransporte.DataSource = Session["GridTransporte"] = dt;
        //        GridTransporte.DataBind();

        //        //Para cada columna en el GridTransporte se define modo de filtro CheckedList 
        //        foreach (GridViewDataColumn column in GridTransporte.Columns)
        //            column.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;

        //        #endregion

        //        #region GUIAS

        //        dt = new DataTable();
        //        dt = documentos.ConsultarGuias(v_pedimento, lblCadena.Text, ref mensaje);
        //        GridGuias.DataSource = Session["GridGuias"] = dt;
        //        GridGuias.DataBind();

        //        //Para cada columna en el GridGuias se define modo de filtro CheckedList 
        //        foreach (GridViewDataColumn column in GridGuias.Columns)
        //            column.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;

        //        #endregion

        //        #region CONTENEDORES

        //        dt = new DataTable();
        //        dt = documentos.ConsultarContenedores(v_pedimento, lblCadena.Text, ref mensaje);
        //        GridContenedores.DataSource = Session["GridContenedores"] = dt;
        //        GridContenedores.DataBind();

        //        //Para cada columna en el GridContenedores se define modo de filtro CheckedList 
        //        foreach (GridViewDataColumn column in GridContenedores.Columns)
        //            column.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;


        //        #endregion

        //        #region OBSERVACIONES

        //        dt = new DataTable();
        //        dt = documentos.ConsultarObservaciones(v_pedimento, lblCadena.Text, ref mensaje);

        //        ASPx_OBSERVACIONES.Text = dt.Rows[0]["OBSERVACIONES"].ToString().Trim() != string.Empty ? dt.Rows[0]["OBSERVACIONES"].ToString().Trim() : "---";

        //        #endregion

        //        #region DESCARGOS

        //        dt = new DataTable();
        //        dt = documentos.ConsultarDescargos(v_pedimento, lblCadena.Text, ref mensaje);
        //        GridDescargos.DataSource = Session["GridDescargos"] = dt;
        //        GridDescargos.DataBind();

        //        //Para cada columna en el GridDescargos se define modo de filtro CheckedList 
        //        foreach (GridViewDataColumn column in GridDescargos.Columns)
        //            column.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;

        //        #endregion

        //    }
        //    else
        //        //Limpia grids del splitter inferior
        //        LimpiarGridsSplitter();
        //}

        protected void btnPartidasCerrar_Click(object sender, EventArgs e)
        {
            System.Threading.Thread.Sleep(1000);
            LimpiarGridsSplitter();
        }

        protected void LimpiarGridsSplitter()
        {
            DataTable dt = new DataTable();

            #region CUENTA DE GASTOS

            detailGridCG.DataSource = Session["GridCuentaGastos"] = dt;
            detailGridCG.DataBind();

            #endregion

            #region EXPEDIENTE DIGITAL

            detailGridDocumentos.DataSource = Session["GridDocumentos"] = dt;
            detailGridDocumentos.DataBind();

            #endregion

            #region IDENTIFICADORES
            detailGridIdentificadores.DataSource = Session["GridIdentificadores"] = dt;
            detailGridIdentificadores.DataBind();

            #endregion

            #region FLETES
            detailGridFletes.DataSource = Session["GridFletes"] = dt;
            detailGridFletes.DataBind();

            #endregion

            #region INCREMENTABLES
            GridIn.DataSource = Session["GridIn"] = dt;
            GridIn.DataBind();
            
            #endregion

            #region FACTURAS
            GridFacturas.DataSource = Session["GridFacturas"] = dt;
            GridFacturas.DataBind();

            #endregion

            #region FECHAS
            GridFechas.DataSource = Session["GridFechas"] = dt;
            GridFechas.DataBind();

            #endregion

            #region CONTRIBUCIONES GENERALES
            GridContriGrales.DataSource = Session["GridContriGrales"] = dt;
            GridContriGrales.DataBind();

            #endregion

            #region IMPUESTOS
            GridImpuestos.DataSource = Session["GridImpuestos"] = dt;
            GridImpuestos.DataBind();

            #endregion

            #region TRANSPORTE
            GridTransporte.DataSource = Session["GridTransporte"] = dt;
            GridTransporte.DataBind();

            #endregion

            #region GUIAS
            GridGuias.DataSource = Session["GridGuias"] = dt;
            GridGuias.DataBind();

            #endregion

            #region CONTENEDORES
            GridContenedores.DataSource = Session["GridContenedores"] = dt;
            GridContenedores.DataBind();

            #endregion

            #region OBSERVACIONES

            ASPx_OBSERVACIONES.Text = "---";
            
            #endregion

            #region DESCARGOS
            GridDescargos.DataSource = Session["GridDescargos"] = dt;
            GridDescargos.DataBind();

            #endregion

            #region RECTIFICACIONES
            GridRectificaciones.DataSource = Session["GridRectificaciones"] = dt;
            GridRectificaciones.DataBind();

            #endregion

            #region PARTIDA GRAVAMEN

            GridPartidaGravamen.DataSource = Session["GridPartidaGravamen"] = dt;
            GridPartidaGravamen.DataBind();

            #endregion

            #region PARTIDA TASAS

            GridPartidaTasas.DataSource = Session["GridPartidaTasas"] = dt;
            GridPartidaTasas.DataBind();

            #endregion

            #region PARTIDA IDENTIFICADORES

            GridPartidaIdentificadores.DataSource = Session["GridPartidaIdentificadores"] = dt;
            GridPartidaIdentificadores.DataBind();

            #endregion

            #region PARTIDA REGULACIONES

            GridPartidaRegulaciones.DataSource = Session["GridPartidaRegulaciones"] = dt;
            GridPartidaRegulaciones.DataBind();

            #endregion

            #region PARTIDA CODIGOS

            GridPartidaCodigos.DataSource = Session["GridPartidaCodigos"] = dt;
            GridPartidaCodigos.DataBind();

            #endregion

        }

        //Evento del Grid para cada uno de los botones(CG,VerEncabezado,VerPartidas)
        protected void Grid_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            try
            {
                BoDocumentos documentos = new BoDocumentos();
                DataTable dt = new DataTable();
                string mensaje = "";
                string v_pedimento = null;
                Session["PEDIMENTOARMADO"] = ASPx_Pedimento.Text = v_pedimento = Grid.GetRowValues(e.VisibleIndex, "PEDIMENTOARMADO").ToString();
                Session["RFC"] = Grid.GetRowValues(e.VisibleIndex, "RFC").ToString();
                Session["TIPO"] = Grid.GetRowValues(e.VisibleIndex, "TIPO").ToString();

                if (e.ButtonID == "btnCG")
                {

                    MostrarModalCG();

                    //Titulo del Modal
                    ModalCGTitulo.InnerText = "Pedimento: " + v_pedimento;
                    DataBind();

                    //Trae información al grid de docuemntos
                    mensaje = "";
                    dt = documentos.ConsultarCuentaDeGastos(v_pedimento, lblCadena.Text, ref mensaje);
                    detailGridCG.DataSource = Session["GridCuentaGastos"] = dt;
                    detailGridCG.DataBind();
                    detailGridCG.Settings.VerticalScrollableHeight = 310;
                    detailGridCG.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;

                    //Se crea un datatable CG para llevar los conceptos extraordinarios por cgkey
                    Session["dtCG"] = null;

                    DataTable dtCG = new DataTable();
                    dtCG.Columns.Add("CGKEY", typeof(string));
                    dtCG.Columns.Add("NO CUENTA DE GASTOS", typeof(string));
                    dtCG.Columns.Add("CONCEPTO EXTRAORDINARIO", typeof(string));
                    Session["dtCG"] = dtCG;

                    //Para cada columna en el GridFacturas se define modo de filtro CheckedList 
                    //foreach (GridViewDataColumn column in detailGridCG.Columns)
                    //    column.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;

                }
                else if (e.ButtonID == "btnED")
                {
                    //Muestra el Modal para ED y también ejecuta un script para dar un valor al cookie "CookieValor"
                    MostrarModalED();

                    //Se limpian sesiones que obtienen el nombre y el sufijo del tipo de archivo
                    Session["cbxTipoArchivo"] = null;
                    Session["cbxSufijoArchivo"] = null;

                    //Titulo del Modal
                    ModalEDTitulo.InnerText = "Pedimento: " + v_pedimento;
                    DataBind();

                    ////Limpiar texto de resultado al guardar archivo(s)
                    lblTitRespuestaFileUpload.Text = string.Empty;
                    //Session["FileUploadForeColor"] = null;


                    //Traer el catálogo de Tipos de Archivos al combo cbxTipoArchivo
                    Catalogos catalogo = new Catalogos();
                    dt = catalogo.TraerTiposArchivo(lblCadena.Text, ref mensaje);
                    cbxTipoArchivo.DataSource = dt == null ? new DataTable() : dt;
                    cbxTipoArchivo.DataBind();

                    //Se selecciona el primer tipo de archivo y se fijan valores de variables de session, siempre que exista al menos uno
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        cbxTipoArchivo.SelectedIndex = 0;
                        Session["cbxTipoArchivo"] = cbxTipoArchivo.SelectedItem.Text.ToString();
                        Session["cbxSufijoArchivo"] = cbxTipoArchivo.SelectedItem.Value.ToString();
                        if (Session["cbxSufijoArchivo"].ToString().IndexOf("*") != -1)
                            Session["cbxSufijoArchivo"] = Session["cbxSufijoArchivo"].ToString().Replace("*", "");

                        btnGuardar.Enabled = true;
                    }
                    else
                    {
                        AlertError("No podrá guardar archivos porque no existen tipos de archivos");
                        btnGuardar.Enabled = false;
                    }

                    //Se limpia la variable de Session["Files"] que guarda los archivos y nombres a subir
                    Session["Files"] = null;


                    //Trae información al grid de docuemntos
                    mensaje = "";
                    dt = new DataTable();
                    dt = documentos.ConsultarExpedienteDigital(v_pedimento, int.Parse(lblIdUsuario.Text), lblCadena.Text, ref mensaje);
                    detailGridDocumentos.DataSource = Session["GridDocumentos"] = dt;
                    detailGridDocumentos.DataBind();
                    detailGridDocumentos.Settings.VerticalScrollableHeight = 300;
                    detailGridDocumentos.Settings.VerticalScrollBarMode = ScrollBarMode.Auto;
                    //detailGridDocumentos.SettingsPager.PageSize = 20;
                    
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        ////Para cada columna en el detailGridDocumentos se define modo de filtro CheckedList 
                        //foreach (GridViewDataColumn column in detailGridDocumentos.Columns)
                        //{
                        //    column.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                        //    if (column.ToString().Contains("ID"))
                        //        break;
                        //}

                        //Se obtiene los nombres de los css del botón btnZip si no tiene el nombre "disabled" se le agrega
                        string cl = btnZip.CssClass;
                        if (!cl.Contains("disabled"))
                        {
                            cl = cl + " " + "disabled";
                            btnZip.CssClass = cl;
                        }

                        //Para cada renglon validar si se hace visible el checkbox y si existe algún checkbox habilitar el botón de descargar en zip
                        for (int i = 0; i < detailGridDocumentos.VisibleRowCount; i++)
                        {
                            ASPxCheckBox chkConsultar = detailGridDocumentos.FindRowCellTemplateControl(i, (GridViewDataColumn)detailGridDocumentos.Columns["PDF"], "chkConsultar") as ASPxCheckBox;

                            if (chkConsultar != null)
                            {
                                if (!detailGridDocumentos.GetRowValues(i, "STATUS").ToString().Equals("OK"))
                                    chkConsultar.Visible = false;
                                else
                                {
                                    chkConsultar.Visible = true;

                                    string cls = btnZip.CssClass;
                                    string newCls = cls.Replace("disabled", "");
                                    btnZip.CssClass = newCls;
                                }
                            }
                        }
                    }
                }
                else if (e.ButtonID == "btnNP")
                {
                    MultiView1.ActiveViewIndex = 1;
                    ASPxPageControl1.ActiveTabIndex = 0;
                    Session["Secuencia"] = null;

                    //Titulo pantalla
                    h2_titulo.InnerText = "Número de Parte";
                    
                    DataTable dtnp2 = new DataTable();
                    DataTable dtnp3 = new DataTable();

                    Session["GridNP"] = null;
                    mensaje = string.Empty;

                    //Llena GridNP 
                    DataTable dtnp = new DataTable();
                    dtnp = np.Consultar_NP(Session["PEDIMENTOARMADO"].ToString(), Session["Cadena"].ToString(), ref mensaje);
                    GridNP.DataSource = Session["GridNP"] = dtnp;
                    GridNP.DataBind();
                    GridNP.Settings.VerticalScrollableHeight = 130;
                    GridNP.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;

                    ////Si existe al menos un registro, entra aquí
                    //if (dtnp != null && dtnp.Rows.Count > 0)
                    //{
                    //    //se selecciona el primer registro
                    //    GridNP.Selection.SelectRow(0);

                    //    //Trae información para grid de detalle y grid de errores
                    //    for (int i = 0; i < GridNP.VisibleRowCount; i++)
                    //    {
                    //        if (GridNP.Selection.IsRowSelected(i))
                    //        {
                    //            mensaje = string.Empty;
                    //            dtnp2 = np.Consultar_NP_Detalle(GridNP.GetSelectedFieldValues("PARTIDA")[0].ToString().Trim(), Session["PEDIMENTOARMADO"].ToString(), Session["Cadena"].ToString(), ref mensaje);
                    //            dtnp3 = np.Errores_Detalle_NP(Session["PEDIMENTOARMADO"].ToString(), Session["Cadena"].ToString(), ref mensaje);
                    //        }
                    //    }

                    //    bbAgregar.Enabled = true;
                    //    bbEditar.Enabled = true;
                    //    bbBorrar.Enabled = true;
                    //}
                    //else
                    //{
                    //    bbAgregar.Enabled = false;
                    //    bbEditar.Enabled = false;
                    //    bbBorrar.Enabled = false;
                    //}


                    //Detalle de NP
                    GridNP2.DataSource = Session["GridNP2"] = dtnp2;
                    GridNP2.DataBind();
                    GridNP2.Settings.VerticalScrollableHeight = 200;
                    GridNP2.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;


                    //GridNP3 Errores
                    GridNP3.DataSource = Session["GridNP3"] = dtnp3;
                    GridNP3.DataBind();
                    GridNP3.Settings.VerticalScrollableHeight = 200;
                    GridNP3.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;

                    ////Si existe algún error mostrar el GridNP3
                    //if (dtnp3 != null && dtnp3.Rows.Count > 0)
                    //    ASPxPageControl1.ActiveTabIndex = 1;
                    //else
                    //    ASPxPageControl1.ActiveTabIndex = 0;

                }
                else if (e.ButtonID == "btnVerEncabezado")
                {
                    MostrarModalEncabezado();

                    //Titulo del Modal
                    ModalEncabezadoTitulo.InnerText = "Pedimento: " + v_pedimento;
                    DataBind();

                    //Abre siempre la pestaña de Datos Generales
                    ASPxPageControlEncabezado.ActiveTabPage = ASPxPageControlEncabezado.TabPages.FindByText("Datos Generales");

                    #region Datos Generales

                    dt = new DataTable();
                    mensaje = "";
                    dt = documentos.ConsultarDatosGenerales(v_pedimento, lblCadena.Text, ref mensaje);
                    //VerticalGridDG.DataSource = dt;
                    //VerticalGridDG.DataBind();
                    if(dt != null && dt.Rows.Count > 0)
                    {
                        DG_PEDIMENTOARMADO.Text = dt.Rows[0]["PEDIMENTOARMADO"].ToString();
                        DG_FECHA.Text = dt.Rows[0]["FECHA"].ToString().Substring(0,10);
                        DG_TIPO_OPERACION.Text = dt.Rows[0]["TIPO DE OPERACION"].ToString();
                        DG_TIPO_PEDIMENTO.Text = dt.Rows[0]["TIPO DE PEDIMENTO"].ToString();
                        DG_DESCRIPCION_TIPO_OPERACION.Text = dt.Rows[0]["DESCRIPCION TIPO DE OPERACION"].ToString();
                        DG_CLAVE_PEDIMENTO.Text = dt.Rows[0]["CLAVE PEDIMENTO"].ToString();
                        DG_DESCRIPCION_CLAVE_PEDIMENTO.Text = dt.Rows[0]["DESCRIPCION DE LA CLAVE PEDIMENTO"].ToString();
                        DG_CLAVE_PAIS_DESTINO.Text = dt.Rows[0]["CLAVE PAIS DESTINO"].ToString();
                        DG_PAIS_DESTINO.Text = dt.Rows[0]["PAIS DESTINO"].ToString();
                        DG_CLAVE_ADUANA.Text = dt.Rows[0]["CLAVE ADUANA"].ToString();
                        DG_ADUANA.Text = dt.Rows[0]["ADUANA"].ToString();

                        decimal peso_bruto = 0;
                        try { peso_bruto = decimal.Parse(dt.Rows[0]["PESO BRUTO"].ToString()); }
                        catch { }
                        DG_PESO_BRUTO.Text = string.Format("{0:n3}", peso_bruto);

                        decimal tipo_cambio = 0;
                        try { tipo_cambio = decimal.Parse(dt.Rows[0]["TIPO DE CAMBIO"].ToString()); }
                        catch { }
                        DG_TIPO_CAMBIO.Text = string.Format("{0:n5}", tipo_cambio);

                        DG_MEDIO_TRANSPORTE_SALIDA.Text = dt.Rows[0]["MEDIO DE TRANSPORTE SALIDA"].ToString();
                        DG_MEDIO_RANSPORTE_SALIDA_DESC.Text = dt.Rows[0]["MEDIO DE TRANSPORTE SALIDA DESCRIPCION"].ToString();
                        DG_MEDIO_TRANSPORTE_ARRIBO.Text = dt.Rows[0]["MEDIO DE TRANSPORTE ARRIBO"].ToString();
                        DG_MEDIO_TRANSPORTE_ARRIBO_DESC.Text = dt.Rows[0]["MEDIO DE TRANSPORTE ARRIBO DESCRIPCION"].ToString();
                        DG_MEDIO_TRANSPORTE_ENTRADA.Text = dt.Rows[0]["MEDIO DE TRANSPORTE ENTRADA"].ToString();
                        DG_MEDIO_TRANSPORTE_ENTRADA_DESC.Text = dt.Rows[0]["MEDIO DE TRANSPORTE ENTRADA DESCRIPCION"].ToString();
                        DG_CURP_APODERADO.Text = dt.Rows[0]["CURP APODERADO MANDATARIO"].ToString();
                        DG_RFC_AGENTE_ADUANAL.Text = dt.Rows[0]["RFC AGENTE ADUANAL"].ToString();

                        decimal valor_dolar = 0;
                        try { valor_dolar = decimal.Parse(dt.Rows[0]["VALOR DOLARES"].ToString()); }
                        catch { }
                        DG_VALOR_DOLARES.Text = string.Format("{0:n2}", valor_dolar);

                        decimal valor_aduanal = 0;
                        try { valor_aduanal = decimal.Parse(dt.Rows[0]["VALOR ADUANAL"].ToString()); }
                        catch { }
                        DG_VALOR_ADUANAL.Text = string.Format("{0:n2}", valor_aduanal);

                        decimal valor_comercial = 0;
                        try { valor_comercial = decimal.Parse(dt.Rows[0]["VALOR COMERCIAL"].ToString()); }
                        catch { }
                        DG_VALOR_COMERCIAL.Text = string.Format("{0:n2}", valor_comercial);
                    }
                    #endregion

                    #region Importador/Exportador

                    dt = new DataTable();
                    mensaje = "";
                    dt = documentos.ConsultarIE(v_pedimento, lblCadena.Text, ref mensaje);
                    //VerticalGridIE.DataSource = dt;
                    //VerticalGridIE.DataBind();
                    if(dt != null && dt.Rows.Count > 0)
                    {
                        IE_RFC.Text = dt.Rows[0]["IE_RFC"].ToString();
                        IE_CURP.Text = dt.Rows[0]["IE_CURP"].ToString();
                        IE_RAZON_SOCIAL.Text = dt.Rows[0]["IE_RAZON_SOCIAL"].ToString();
                        IE_CALLE.Text = dt.Rows[0]["IE_CALLE"].ToString();
                        IE_NUMERO_EXTERIOR.Text = dt.Rows[0]["IE_NUMERO_EXTERIOR"].ToString();
                        IE_NUMERO_INTERIOR.Text = dt.Rows[0]["IE_NUMERO_INTERIOR"].ToString();
                        IE_MUNICIPIO.Text = dt.Rows[0]["IE_MUNICIPIO"].ToString();
                        IE_CODIGO_POSTAL.Text = dt.Rows[0]["IE_CODIGO_POSTAL"].ToString();
                        IE_PAIS.Text = dt.Rows[0]["IE_PAIS"].ToString();
                        
                        decimal seguros = 0;
                        try { seguros = decimal.Parse(dt.Rows[0]["SEGUROS"].ToString()); }
                        catch { }
                        IE_SEGUROS.Text = string.Format("{0:n2}", seguros);

                        decimal fletes = 0;
                        try { fletes = decimal.Parse(dt.Rows[0]["FLETES"].ToString()); }
                        catch { }
                        IE_FLETES.Text = string.Format("{0:n2}", fletes);

                        decimal embalajes = 0;
                        try { embalajes = decimal.Parse(dt.Rows[0]["EMBALAJES"].ToString()); }
                        catch { }
                        IE_EMBALAJES.Text = string.Format("{0:n2}", embalajes);

                        decimal incrementable = 0;
                        try { incrementable = decimal.Parse(dt.Rows[0]["INCREMENTABLES"].ToString()); }
                        catch { }
                        IE_INCREMENTABLES.Text = string.Format("{0:n2}", incrementable);

                        IE_ADUANA_DESPACHO.Text = dt.Rows[0]["ADUANA_DESPACHO"].ToString();
                        IE_DESCRIPCION_ADUANA_DESPACHO.Text = dt.Rows[0]["DESCRIPCION_ADUANA_DESPACHO"].ToString();
                        IE_BULTOS.Text = dt.Rows[0]["BULTOS"].ToString();

                        decimal efectivo = 0;
                        try { efectivo = decimal.Parse(dt.Rows[0]["EFECTIVO"].ToString()); }
                        catch { }
                        IE_EFECTIVO.Text = string.Format("{0:n2}", efectivo);


                        IE_OTROS.Text = dt.Rows[0]["OTROS"].ToString();

                        decimal total = 0;
                        try { total = decimal.Parse(dt.Rows[0]["TOTAL"].ToString()); }
                        catch { }
                        IE_TOTAL.Text = string.Format("{0:n2}", total);

                        IE_CLAVE_PAIS.Text = dt.Rows[0]["CLAVE_PAIS"].ToString();
                        IE_DESCRIPCIÓN_PAIS.Text = dt.Rows[0]["DESCRIPCION_PAIS"].ToString();
                    }

                    #endregion

                    #region Compradores/Vendedores

                    dt = new DataTable();
                    mensaje = "";
                    dt = documentos.ConsultarCV(v_pedimento, lblCadena.Text, ref mensaje);
                    VerticalGridCV.DataSource = dt;
                    VerticalGridCV.DataBind();

                    #endregion

                    #region XML

                    dt = new DataTable();
                    mensaje = "";
                    dt = documentos.ConsultarXML(v_pedimento, lblCadena.Text, ref mensaje);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        string cadena = dt.Rows[0]["XML"].ToString().Trim();

                        string formattedXml = XElement.Parse(cadena).ToString().Replace("<", "< ").Replace(">", " >");
                        //string formattedXml2 = System.Xml.Linq.XDocument.Parse(cadena).ToString();

                        MemoXML.Text = formattedXml;
                    }

                    //System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                    //doc.LoadXml(cadena);
                    //System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    //System.Xml.XmlWriter xw = System.Xml.XmlTextWriter.Create(sb, new System.Xml.XmlWriterSettings() { Indent = true });
                    //doc.WriteTo(xw);
                    //xw.Flush();
                    //formattedXml = sb.ToString();

                    //MemoX.Text = formattedXml;


                    //XPathDocument document = new XPathDocument(cadena);
                    //XPathNavigator navigator = document.CreateNavigator();

                    //XPathNodeIterator nodes = navigator.Select("/S:Envelope/S:Header/wsse:Security/wsu:Timestamp/S:Body/ns2:consultarPedimentoCompletoRespuesta/ns3:tieneError/ns2:numeroOperacion/ns2:pedimento/ns2:encabezado/ns2:tipoOperacion/ns2:descripcion/ns2:clave/ns2:claveDocumento/ns2:destino");
                    //nodes.MoveNext();
                    //XPathNavigator nodesNavigator = nodes.Current;

                    //XPathNodeIterator nodesText = nodesNavigator.SelectDescendants(XPathNodeType.Text, false);

                    //while (nodesText.MoveNext())
                    //    Console.WriteLine(nodesText.Current.Value);

                    


                    //XmlDocument xml = new XmlDocument();
                    //xml.LoadXml(dt.Rows[0]["XML"].ToString());
                    //xmlE.Document = xml;



                    //ASPxLabelXML1.Text = dt.Rows[0]["XML"].ToString().Trim() != string.Empty ? dt.Rows[0]["XML"].ToString().Trim() : "---";
                    //ASPxLabelXML.Text = dt.Rows[0]["XML"].ToString().Trim() != string.Empty ? dt.Rows[0]["XML"].ToString().Trim() : "---";

                    #endregion

                    #region Identificadores

                    dt = new DataTable();
                    mensaje = "";
                    dt = documentos.ConsultarIG(v_pedimento, lblCadena.Text, ref mensaje);
                    detailGridIdentificadores.DataSource = Session["GridIdentificadores"] = dt;
                    detailGridIdentificadores.DataBind();

                    #endregion

                    #region Fletes

                    dt = new DataTable();
                    mensaje = "";
                    dt = documentos.ConsultarFletes(v_pedimento, lblCadena.Text, ref mensaje);
                    detailGridFletes.DataSource = Session["GridFletes"] = dt;
                    detailGridFletes.DataBind();

                    #endregion

                    #region INCREMENTABLES

                    mensaje = "";
                    dt = new DataTable();
                    dt = documentos.ConsultarIncrementables(v_pedimento, lblCadena.Text, ref mensaje);
                    GridIn.DataSource = Session["GridIn"] = dt;
                    GridIn.DataBind();

                    #endregion

                    #region FACTURAS

                    mensaje = "";
                    dt = new DataTable();
                    dt = documentos.ConsultarFacturas(v_pedimento, lblCadena.Text, ref mensaje);
                    GridFacturas.DataSource = Session["GridFacturas"] = dt;
                    GridFacturas.DataBind();

                    ////Para cada columna en el GridFacturas se define modo de filtro CheckedList 
                    //foreach (GridViewDataColumn column in GridFacturas.Columns)
                    //    column.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;

                    #endregion

                    #region FECHAS

                    mensaje = "";
                    dt = new DataTable();
                    dt = documentos.ConsultarFechas(v_pedimento, lblCadena.Text, ref mensaje);
                    GridFechas.DataSource = Session["GridFechas"] = dt;
                    GridFechas.DataBind();

                    //Para cada columna en el GridFechas se define modo de filtro CheckedList 
                    foreach (GridViewDataColumn column in GridFechas.Columns)
                        column.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;

                    #endregion

                    #region CONTRIBUCIONES GENERALES

                    mensaje = "";
                    dt = new DataTable();
                    dt = documentos.ConsultarContribucionesGenerales(v_pedimento, lblCadena.Text, ref mensaje);
                    GridContriGrales.DataSource = Session["GridContriGrales"] = dt;
                    GridContriGrales.DataBind();

                    //Para cada columna en el GridContriGrales se define modo de filtro CheckedList 
                    foreach (GridViewDataColumn column in GridContriGrales.Columns)
                        column.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;

                    #endregion

                    #region IMPUESTOS

                    mensaje = "";
                    dt = new DataTable();
                    dt = documentos.ConsultarImpuestos(v_pedimento, lblCadena.Text, ref mensaje);
                    GridImpuestos.DataSource = Session["GridImpuestos"] = dt;
                    GridImpuestos.DataBind();

                    //Para cada columna en el GridImpuestos se define modo de filtro CheckedList 
                    foreach (GridViewDataColumn column in GridImpuestos.Columns)
                        column.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;

                    #endregion

                    #region TRANSPORTE

                    mensaje = "";
                    dt = new DataTable();
                    dt = documentos.ConsultarTransporte(v_pedimento, lblCadena.Text, ref mensaje);
                    GridTransporte.DataSource = Session["GridTransporte"] = dt;
                    GridTransporte.DataBind();

                    //Para cada columna en el GridTransporte se define modo de filtro CheckedList 
                    foreach (GridViewDataColumn column in GridTransporte.Columns)
                        column.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;

                    #endregion

                    #region GUIAS

                    mensaje = "";
                    dt = new DataTable();
                    dt = documentos.ConsultarGuias(v_pedimento, lblCadena.Text, ref mensaje);
                    GridGuias.DataSource = Session["GridGuias"] = dt;
                    GridGuias.DataBind();

                    //Para cada columna en el GridGuias se define modo de filtro CheckedList 
                    foreach (GridViewDataColumn column in GridGuias.Columns)
                        column.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;

                    #endregion

                    #region CONTENEDORES

                    mensaje = "";
                    dt = new DataTable();
                    dt = documentos.ConsultarContenedores(v_pedimento, lblCadena.Text, ref mensaje);
                    GridContenedores.DataSource = Session["GridContenedores"] = dt;
                    GridContenedores.DataBind();

                    //Para cada columna en el GridContenedores se define modo de filtro CheckedList 
                    foreach (GridViewDataColumn column in GridContenedores.Columns)
                        column.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;


                    #endregion

                    #region OBSERVACIONES

                    dt = new DataTable();
                    dt = documentos.ConsultarObservaciones(v_pedimento, lblCadena.Text, ref mensaje);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        ASPx_OBSERVACIONES.Text = dt.Rows[0]["OBSERVACIONES"].ToString().Trim() != string.Empty ? dt.Rows[0]["OBSERVACIONES"].ToString().Trim() : "---";
                    }

                    #endregion

                    #region DESCARGOS

                    mensaje = "";
                    dt = new DataTable();
                    dt = documentos.ConsultarDescargos(v_pedimento, lblCadena.Text, ref mensaje);
                    GridDescargos.DataSource = Session["GridDescargos"] = dt;
                    GridDescargos.DataBind();

                    ////Para cada columna en el GridDescargos se define modo de filtro CheckedList 
                    //foreach (GridViewDataColumn column in GridDescargos.Columns)
                    //    column.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;

                    #endregion

                    #region RECTIFICACIONES

                    mensaje = "";
                    dt = new DataTable();
                    dt = documentos.ConsultarRectificaciones(v_pedimento, lblCadena.Text, ref mensaje);
                    GridRectificaciones.DataSource = Session["GridRectificaciones"] = dt;
                    GridRectificaciones.DataBind();

                    ////Para cada columna en el GridRectificaciones se define modo de filtro CheckedList 
                    //foreach (GridViewDataColumn column in GridRectificaciones.Columns)
                    //    column.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;

                    #endregion

                }

                else if (e.ButtonID == "btnVerPartida")
                {
                    //Se obtienen las partidas en un Datatable
                    mensaje = "";
                    dt = documentos.ConsultarPartidas(v_pedimento, lblCadena.Text, ref mensaje);

                    //Valida si existen Partidas
                    if (dt == null)
                        return;


                    MostrarModal();

                    //Titulo del Modal
                    Modal1Titulo.InnerText = "Pedimento: " + v_pedimento;
                    DataBind();


                    //Abre siempre la pestaña de Detalle
                    pageControl.ActiveTabPage = pageControl.TabPages.FindByText("Detalle");


                    //Se ordenan por cantidad ascendente y se guarda en otro Datatable
                    DataView dv = dt.DefaultView;
                    dv.Sort = "PARTIDA ASC";
                    DataTable sortedDt = dv.ToTable();


                    //Se recorre la tabla para obtener el último registro
                    string total = string.Empty;
                    foreach(DataRow fila in sortedDt.Rows)
                        total = fila["PARTIDA"].ToString();


                    //Se agrega al parámetro el valor total de partidas
                    lblTitDeTotal.Text = total;


                    //Llenar combo de Partidas
                    cbxPartida.DataSource = dt == null ? new DataTable() : sortedDt;
                    cbxPartida.DataBind();


                    //Se selecciona la primer Partida
                    cbxPartida.SelectedIndex = 0;
                    long v_partida = long.Parse(cbxPartida.Text.Trim());


                    #region DETALLE
                    //mensaje = "";
                    dt = new DataTable();
                    dt = documentos.ConsultarPartida_Detalle(v_pedimento, v_partida, lblCadena.Text, ref mensaje);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        //VerticalGridPartidaDetalle.DataSource = dt;
                        //VerticalGridPartidaDetalle.DataBind();

                        lblP_PARTIDA_DETALLEKEY.Text = dt.Rows[0]["PARTIDA_DETALLEKEY"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLEKEY"].ToString().Trim() : "---";
                        lblP_PEDIMENTO.Text = dt.Rows[0]["PEDIMENTOARMADO"].ToString().Trim() != string.Empty ? dt.Rows[0]["PEDIMENTOARMADO"].ToString().Trim() : "---";
                        lblP_PARTIDA.Text = dt.Rows[0]["PARTIDA"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA"].ToString().Trim() : "---";
                        lblP_NUMERO_PARTIDA.Text = dt.Rows[0]["PARTIDA_DETALLE_NUMEROPARTIDA"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_NUMEROPARTIDA"].ToString().Trim() : "---";
                        lblP_FRACCION_ARANCELARIA.Text = dt.Rows[0]["PARTIDA_DETALLE_FRACCIONARANCELARIA"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_FRACCIONARANCELARIA"].ToString().Trim() : "---";
                        lblP_SUBDIVISION_FRACCION.Text = dt.Rows[0]["PARTIDA_DETALLE_SUBDIVISIONFRACCION"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_SUBDIVISIONFRACCION"].ToString().Trim() : "---";
                        lblP_DESCRIPCION_MERCANCIA.Text = dt.Rows[0]["PARTIDA_DETALLE_DESCRIPCIONMERCANCIA"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_DESCRIPCIONMERCANCIA"].ToString().Trim() : "---";
                        lblP_UNIDAD_MEDIDA_TARIFA_CLAVE.Text = dt.Rows[0]["PARTIDA_DETALLE_UNIDADMEDIDATARIFA_CLAVE"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_UNIDADMEDIDATARIFA_CLAVE"].ToString().Trim() : "---";
                        lblP_UNIDAD_MEDIDA_TARIFA_DESCRIPCION.Text = dt.Rows[0]["PARTIDA_DETALLE_UNIDADMEDIDATARIFA_DESCRIPCION"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_UNIDADMEDIDATARIFA_DESCRIPCION"].ToString().Trim() : "---";
                        lblP_CANTIDAD_UNIDAD_MEDIDA_TARIFA.Text = dt.Rows[0]["PARTIDA_DETALLE_CANTIDADUNIDADMEDIDATARIFA"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_CANTIDADUNIDADMEDIDATARIFA"].ToString().Trim() : "---";
                        lblP_UNIDAD_MEDIDA_COMERCIAL_CLAVE.Text = dt.Rows[0]["PARTIDA_DETALLE_UNIDADMEDIDACOMERCIA_CLAVE"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_UNIDADMEDIDACOMERCIA_CLAVE"].ToString().Trim() : "---";
                        lblP_UNIDAD_MEDIDA_COMERCIAL_CLAVE_DESCRIPCION.Text = dt.Rows[0]["PARTIDA_DETALLE_UNIDADMEDIDACOMERCIAL_DESCRIPCION"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_UNIDADMEDIDACOMERCIAL_DESCRIPCION"].ToString().Trim() : "---";
                        lblP_CANTIDAD_UNIDAD_MEDIDA_COMERCIAL.Text = dt.Rows[0]["PARTIDA_DETALLE_CANTIDADUNIDADMEDIDACOMERCIAL"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_CANTIDADUNIDADMEDIDACOMERCIAL"].ToString().Trim() : "---";
                        lblP_PRECIO_UNITARIO.Text = dt.Rows[0]["PARTIDA_DETALLE_PRECIOUNITARIO"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_PRECIOUNITARIO"].ToString().Trim() : "---";
                        lblP_VALOR_COMERCIAL.Text = dt.Rows[0]["PARTIDA_DETALLE_VALORCOMERCIAL"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_VALORCOMERCIAL"].ToString().Trim() : "---";
                        lblP_VALOR_ADUANA.Text = dt.Rows[0]["PARTIDA_DETALLE_VALORADUANA"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_VALORADUANA"].ToString().Trim() : "---";
                        lblP_VALOR_DOLARES.Text = dt.Rows[0]["PARTIDA_DETALLE_VALORDOLARES"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_VALORDOLARES"].ToString().Trim() : "---";
                        lblP_VALOR_AGREGADO.Text = dt.Rows[0]["PARTIDA_DETALLE_VALORAGREGADO"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_VALORAGREGADO"].ToString().Trim() : "---";
                        lblP_CODIGO_PRODUCTO.Text = dt.Rows[0]["PARTIDA_DETALLE_CODIPRODUCTO"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_CODIPRODUCTO"].ToString().Trim() : "---";
                        lblP_MARCA.Text = dt.Rows[0]["PARTIDA_DETALLE_MARCA"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_MARCA"].ToString().Trim() : "---";
                        lblP_MODELO.Text = dt.Rows[0]["PARTIDA_DETALLE_MODELO"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_MODELO"].ToString().Trim() : "---";
                        lblP_METODO_VALORACION.Text = dt.Rows[0]["PARTIDA_DETALLE_METODOVALORACION"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_METODOVALORACION"].ToString().Trim() : "---";
                        lblP_VINCULACION.Text = dt.Rows[0]["PARTIDA_DETALLE_VINCULACION"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_VINCULACION"].ToString().Trim() : "---";
                        lblP_PAIS_ORIGEN_DESTINO_CLAVE.Text = dt.Rows[0]["PARTIDA_DETALLE_PAISORIGENDESTINO_CLAVE"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_PAISORIGENDESTINO_CLAVE"].ToString().Trim() : "---";
                        lblP_PAIS_ORIGEN_DESTINO_NOMBRE.Text = dt.Rows[0]["PARTIDA_DETALLE_PAISORIGENDESTINO_DESCRIPCION"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_PAISORIGENDESTINO_DESCRIPCION"].ToString().Trim() : "---";
                        lblP_PAIS_VENDEDOR_COMPRADOR_CLAVE.Text = dt.Rows[0]["PARTIDA_DETALLE_PAISVENDEDORCOMPRADOR_CLAVE"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_PAISVENDEDORCOMPRADOR_CLAVE"].ToString().Trim() : "---";
                        lblP_PAIS_VENDEDOR_COMPRADOR_NOMBRE.Text = dt.Rows[0]["PARTIDA_DETALLE_PAISVENDEDORCOMPRADOR_DESCRIPCION"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_PAISVENDEDORCOMPRADOR_DESCRIPCION"].ToString().Trim() : "---";


                    }

                    #endregion

                    #region OBSERVACIONES

                    dt = new DataTable();
                    dt = documentos.ConsultarPartida_Observaciones(v_pedimento, v_partida, lblCadena.Text, ref mensaje);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        ASPx_PartidaObservaciones.Text = dt.Rows[0]["OBSERVACIONES"].ToString().Trim() != string.Empty ? dt.Rows[0]["OBSERVACIONES"].ToString().Trim() : "---";

                    }
                    #endregion

                    #region IMPUESTOS GRAVAMEN E IMPUESTOS TASAS

                    //GRAVAMEN
                    dt = new DataTable();
                    dt = documentos.ConsultarPartida_Gravamen(v_pedimento, v_partida, lblCadena.Text, ref mensaje);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        GridPartidaGravamen.DataSource = Session["GridPartidaGravamen"] = dt;
                        GridPartidaGravamen.DataBind();
                    }

                    //TASAS
                    dt = new DataTable();
                    dt = documentos.ConsultarPartida_Tasas(v_pedimento, v_partida, lblCadena.Text, ref mensaje);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        GridPartidaTasas.DataSource = Session["GridPartidaTasas"] = dt;
                        GridPartidaTasas.DataBind();
                    }
                    #endregion

                    #region IDENTIFICADORES

                    dt = new DataTable();
                    dt = documentos.ConsultarPartida_Identificadores(v_pedimento, v_partida, lblCadena.Text, ref mensaje);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        GridPartidaIdentificadores.DataSource = Session["GridPartidaIdentificadores"] = dt;
                        GridPartidaIdentificadores.DataBind();
                    }

                    #endregion

                    #region REGULACIONES

                    dt = new DataTable();
                    dt = documentos.ConsultarPartida_Regulaciones(v_pedimento, v_partida, lblCadena.Text, ref mensaje);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        GridPartidaRegulaciones.DataSource = Session["GridPartidaRegulaciones"] = dt;
                        GridPartidaRegulaciones.DataBind();
                    }

                    #endregion

                    #region XML
                    
                    dt = new DataTable();
                    MemoPartidaXML.Text = string.Empty;
                    dt = documentos.ConsultarPartida_XML(v_pedimento, v_partida, lblCadena.Text, ref mensaje);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        string cadena = dt.Rows[0]["XML"].ToString().Trim() != string.Empty ? dt.Rows[0]["XML"].ToString().Trim() : "---";
                        if (!cadena.Equals("---"))
                        {
                            string formattedXml = XElement.Parse(cadena).ToString().Replace("<", "< ").Replace(">", " >");
                            MemoPartidaXML.Text = formattedXml;
                        }
                        else
                            MemoPartidaXML.Text = cadena;
                    }

                    #endregion

                    #region CÓDIGOS

                    dt = new DataTable();
                    dt = documentos.ConsultarPartida_Codigos(v_pedimento, v_partida, lblCadena.Text, ref mensaje);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        GridPartidaCodigos.DataSource = Session["GridPartidaCodigos"] = dt;
                        GridPartidaCodigos.DataBind();
                    }

                    #endregion

                }

                //Se requiere cargar los datos en Grid2 que son las Fechas(Año-Mes)
                if (Grid2.VisibleRowCount == 0)
                {
                    if (Session["FECHAS"] == null)
                        Session["FECHAS"] = docs.ConsultarFechasOperacion(lblCadena.Text, ref mensaje);

                    CreaSlideBar();
                }
            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (lblIdUsuario.Text.Length > 0)
                    idusuario = int.Parse(lblIdUsuario.Text);
                excepcion.RegistrarExcepcion(idusuario, "Documentos-Grid_CustomButtonCallback", ex, lblCadena.Text, ref mensaje);
            }

            //Actualiza los permisos
            PermisosUsuario();
        }

        protected void ASPxButtonED_Click(object sender, EventArgs e)
        {
            try
            {
                ASPxButton btn = (ASPxButton)sender;

                btn.ImageUrl = "~/img/iconos/ico_exdi.png";

                GridViewDataItemTemplateContainer container = (GridViewDataItemTemplateContainer)btn.NamingContainer;

                string v_pedimento = null;
                Session["PEDIMENTOARMADO"] = ASPx_Pedimento.Text = v_pedimento = Grid.GetRowValues(container.VisibleIndex, "PEDIMENTOARMADO").ToString();
                Session["RFC"] = Grid.GetRowValues(container.VisibleIndex, "RFC").ToString();
                Session["TIPO"] = Grid.GetRowValues(container.VisibleIndex, "TIPO").ToString();

                //Muestra el Modal para ED y también ejecuta un script para dar un valor al cookie "CookieValor"
                MostrarModalED();

                //Se limpian sesiones que obtienen el nombre y el sufijo del tipo de archivo
                Session["cbxTipoArchivo"] = null;
                Session["cbxSufijoArchivo"] = null;

                //Titulo del Modal
                ModalEDTitulo.InnerText = "Pedimento: " + v_pedimento;
                DataBind();

                ////Limpiar texto de resultado al guardar archivo(s)
                lblTitRespuestaFileUpload.Text = string.Empty;
                //Session["FileUploadForeColor"] = null;


                //Traer el catálogo de Tipos de Archivos al combo cbxTipoArchivo
                Catalogos catalogo = new Catalogos();
                DataTable dt = new DataTable();
                string mensaje = string.Empty;
                dt = catalogo.TraerTiposArchivo(lblCadena.Text, ref mensaje);
                cbxTipoArchivo.DataSource = dt == null ? new DataTable() : dt;
                cbxTipoArchivo.DataBind();

                //Se selecciona el primer tipo de archivo y se fijan valores de variables de session, siempre que exista al menos uno
                if (dt != null && dt.Rows.Count > 0)
                {
                    cbxTipoArchivo.SelectedIndex = 0;
                    Session["cbxTipoArchivo"] = cbxTipoArchivo.SelectedItem.Text.ToString();
                    Session["cbxSufijoArchivo"] = cbxTipoArchivo.SelectedItem.Value.ToString();
                    if (Session["cbxSufijoArchivo"].ToString().IndexOf("*") != -1)
                        Session["cbxSufijoArchivo"] = Session["cbxSufijoArchivo"].ToString().Replace("*", "");

                    btnGuardar.Enabled = true;
                }
                else
                {
                    AlertError("No podrá guardar archivos porque no existen tipos de archivos");
                    btnGuardar.Enabled = false;
                }

                //Se limpia la variable de Session["Files"] que guarda los archivos y nombres a subir
                Session["Files"] = null;


                //Trae información al grid de docuemntos
                BoDocumentos documentos = new BoDocumentos();
                mensaje = "";
                dt = new DataTable();
                dt = documentos.ConsultarExpedienteDigital(v_pedimento, int.Parse(lblIdUsuario.Text), lblCadena.Text, ref mensaje);
                detailGridDocumentos.DataSource = Session["GridDocumentos"] = dt;
                detailGridDocumentos.DataBind();
                detailGridDocumentos.Settings.VerticalScrollableHeight = 300;
                detailGridDocumentos.Settings.VerticalScrollBarMode = ScrollBarMode.Auto;
                //detailGridDocumentos.SettingsPager.PageSize = 20;

                if (dt != null && dt.Rows.Count > 0)
                {
                    ////Para cada columna en el detailGridDocumentos se define modo de filtro CheckedList 
                    //foreach (GridViewDataColumn column in detailGridDocumentos.Columns)
                    //{
                    //    column.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                    //    if (column.ToString().Contains("ID"))
                    //        break;
                    //}

                    //Se obtiene los nombres de los css del botón btnZip si no tiene el nombre "disabled" se le agrega
                    string cl = btnZip.CssClass;
                    if (!cl.Contains("disabled"))
                    {
                        cl = cl + " " + "disabled";
                        btnZip.CssClass = cl;
                    }

                    //Para cada renglon validar si se hace visible el checkbox y si existe algún checkbox habilitar el botón de descargar en zip
                    for (int i = 0; i < detailGridDocumentos.VisibleRowCount; i++)
                    {
                        ASPxCheckBox chkConsultar = detailGridDocumentos.FindRowCellTemplateControl(i, (GridViewDataColumn)detailGridDocumentos.Columns["PDF"], "chkConsultar") as ASPxCheckBox;

                        if (chkConsultar != null)
                        {
                            if (!detailGridDocumentos.GetRowValues(i, "STATUS").ToString().Equals("OK"))
                                chkConsultar.Visible = false;
                            else
                            {
                                chkConsultar.Visible = true;

                                string cls = btnZip.CssClass;
                                string newCls = cls.Replace("disabled", "");
                                btnZip.CssClass = newCls;
                            }
                        }
                    }
                }

                //Se requiere cargar los datos en Grid2 que son las Fechas(Año-Mes)
                if (Grid2.VisibleRowCount == 0)
                {
                    if (Session["FECHAS"] == null)
                        Session["FECHAS"] = docs.ConsultarFechasOperacion(lblCadena.Text, ref mensaje);

                    CreaSlideBar();
                }
            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (lblIdUsuario.Text.Length > 0)
                    idusuario = int.Parse(lblIdUsuario.Text);
                excepcion.RegistrarExcepcion(idusuario, "Documentos-ASPxButtonED_Click", ex, lblCadena.Text, ref mensaje);
            }            
        }

        //Cuando el grid principal trae datos entra aquí para pintar imagen en botones de columna ED
        protected void ASPxButtonED_Init(object sender, EventArgs e)
        {
            ASPxButton btn = (ASPxButton)sender;
            btn.ImageUrl = "~/img/iconos/ico_exdi.png";
        }


        protected void ASPxProgressBar1_Init(object sender, EventArgs e)
        {

            
        }

        //Al seleccionar una partida entra en este evento para traer información relacionada a esta partida
        protected void cbxPartida_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            { 
                MostrarModal();

                //Abre siempre la pestaña de Detalle
                pageControl.ActiveTabPage = pageControl.TabPages.FindByText("Detalle");


                BoDocumentos documentos = new BoDocumentos();
                DataTable dt = new DataTable();
                string mensaje = "";

                string v_pedimento = Session["PEDIMENTOARMADO"].ToString();
                long v_partida = long.Parse(cbxPartida.Text.Trim());
                

                #region DETALLE
                //mensaje = "";
                dt = new DataTable();
                dt = documentos.ConsultarPartida_Detalle(v_pedimento, v_partida, lblCadena.Text, ref mensaje);
                if (dt != null && dt.Rows.Count > 0)
                {
                    //VerticalGridPartidaDetalle.DataSource = dt;
                    //VerticalGridPartidaDetalle.DataBind();

                    lblP_PARTIDA_DETALLEKEY.Text = dt.Rows[0]["PARTIDA_DETALLEKEY"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLEKEY"].ToString().Trim() : "---";
                    lblP_PEDIMENTO.Text = dt.Rows[0]["PEDIMENTOARMADO"].ToString().Trim() != string.Empty ? dt.Rows[0]["PEDIMENTOARMADO"].ToString().Trim() : "---";
                    lblP_PARTIDA.Text = dt.Rows[0]["PARTIDA"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA"].ToString().Trim() : "---";
                    lblP_NUMERO_PARTIDA.Text = dt.Rows[0]["PARTIDA_DETALLE_NUMEROPARTIDA"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_NUMEROPARTIDA"].ToString().Trim() : "---";
                    lblP_FRACCION_ARANCELARIA.Text = dt.Rows[0]["PARTIDA_DETALLE_FRACCIONARANCELARIA"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_FRACCIONARANCELARIA"].ToString().Trim() : "---";
                    lblP_SUBDIVISION_FRACCION.Text = dt.Rows[0]["PARTIDA_DETALLE_SUBDIVISIONFRACCION"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_SUBDIVISIONFRACCION"].ToString().Trim() : "---";
                    lblP_DESCRIPCION_MERCANCIA.Text = dt.Rows[0]["PARTIDA_DETALLE_DESCRIPCIONMERCANCIA"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_DESCRIPCIONMERCANCIA"].ToString().Trim() : "---";
                    lblP_UNIDAD_MEDIDA_TARIFA_CLAVE.Text = dt.Rows[0]["PARTIDA_DETALLE_UNIDADMEDIDATARIFA_CLAVE"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_UNIDADMEDIDATARIFA_CLAVE"].ToString().Trim() : "---";
                    lblP_UNIDAD_MEDIDA_TARIFA_DESCRIPCION.Text = dt.Rows[0]["PARTIDA_DETALLE_UNIDADMEDIDATARIFA_DESCRIPCION"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_UNIDADMEDIDATARIFA_DESCRIPCION"].ToString().Trim() : "---";
                    lblP_CANTIDAD_UNIDAD_MEDIDA_TARIFA.Text = dt.Rows[0]["PARTIDA_DETALLE_CANTIDADUNIDADMEDIDATARIFA"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_CANTIDADUNIDADMEDIDATARIFA"].ToString().Trim() : "---";
                    lblP_UNIDAD_MEDIDA_COMERCIAL_CLAVE.Text = dt.Rows[0]["PARTIDA_DETALLE_UNIDADMEDIDACOMERCIA_CLAVE"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_UNIDADMEDIDACOMERCIA_CLAVE"].ToString().Trim() : "---";
                    lblP_UNIDAD_MEDIDA_COMERCIAL_CLAVE_DESCRIPCION.Text = dt.Rows[0]["PARTIDA_DETALLE_UNIDADMEDIDACOMERCIAL_DESCRIPCION"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_UNIDADMEDIDACOMERCIAL_DESCRIPCION"].ToString().Trim() : "---";
                    lblP_CANTIDAD_UNIDAD_MEDIDA_COMERCIAL.Text = dt.Rows[0]["PARTIDA_DETALLE_CANTIDADUNIDADMEDIDACOMERCIAL"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_CANTIDADUNIDADMEDIDACOMERCIAL"].ToString().Trim() : "---";
                    lblP_PRECIO_UNITARIO.Text = dt.Rows[0]["PARTIDA_DETALLE_PRECIOUNITARIO"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_PRECIOUNITARIO"].ToString().Trim() : "---";
                    lblP_VALOR_COMERCIAL.Text = dt.Rows[0]["PARTIDA_DETALLE_VALORCOMERCIAL"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_VALORCOMERCIAL"].ToString().Trim() : "---";
                    lblP_VALOR_ADUANA.Text = dt.Rows[0]["PARTIDA_DETALLE_VALORADUANA"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_VALORADUANA"].ToString().Trim() : "---";
                    lblP_VALOR_DOLARES.Text = dt.Rows[0]["PARTIDA_DETALLE_VALORDOLARES"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_VALORDOLARES"].ToString().Trim() : "---";
                    lblP_VALOR_AGREGADO.Text = dt.Rows[0]["PARTIDA_DETALLE_VALORAGREGADO"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_VALORAGREGADO"].ToString().Trim() : "---";
                    lblP_CODIGO_PRODUCTO.Text = dt.Rows[0]["PARTIDA_DETALLE_CODIPRODUCTO"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_CODIPRODUCTO"].ToString().Trim() : "---";
                    lblP_MARCA.Text = dt.Rows[0]["PARTIDA_DETALLE_MARCA"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_MARCA"].ToString().Trim() : "---";
                    lblP_MODELO.Text = dt.Rows[0]["PARTIDA_DETALLE_MODELO"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_MODELO"].ToString().Trim() : "---";
                    lblP_METODO_VALORACION.Text = dt.Rows[0]["PARTIDA_DETALLE_METODOVALORACION"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_METODOVALORACION"].ToString().Trim() : "---";
                    lblP_VINCULACION.Text = dt.Rows[0]["PARTIDA_DETALLE_VINCULACION"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_VINCULACION"].ToString().Trim() : "---";
                    lblP_PAIS_ORIGEN_DESTINO_CLAVE.Text = dt.Rows[0]["PARTIDA_DETALLE_PAISORIGENDESTINO_CLAVE"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_PAISORIGENDESTINO_CLAVE"].ToString().Trim() : "---";
                    lblP_PAIS_ORIGEN_DESTINO_NOMBRE.Text = dt.Rows[0]["PARTIDA_DETALLE_PAISORIGENDESTINO_DESCRIPCION"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_PAISORIGENDESTINO_DESCRIPCION"].ToString().Trim() : "---";
                    lblP_PAIS_VENDEDOR_COMPRADOR_CLAVE.Text = dt.Rows[0]["PARTIDA_DETALLE_PAISVENDEDORCOMPRADOR_CLAVE"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_PAISVENDEDORCOMPRADOR_CLAVE"].ToString().Trim() : "---";
                    lblP_PAIS_VENDEDOR_COMPRADOR_NOMBRE.Text = dt.Rows[0]["PARTIDA_DETALLE_PAISVENDEDORCOMPRADOR_DESCRIPCION"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_PAISVENDEDORCOMPRADOR_DESCRIPCION"].ToString().Trim() : "---";


                }

                #endregion

                #region OBSERVACIONES

                dt = new DataTable();
                dt = documentos.ConsultarPartida_Observaciones(v_pedimento, v_partida, lblCadena.Text, ref mensaje);
                if (dt != null && dt.Rows.Count > 0)
                {
                    ASPx_PartidaObservaciones.Text = dt.Rows[0]["OBSERVACIONES"].ToString().Trim() != string.Empty ? dt.Rows[0]["OBSERVACIONES"].ToString().Trim() : "---";

                }
                #endregion

                #region IMPUESTOS GRAVAMEN E IMPUESTOS TASAS

                //GRAVAMEN
                dt = new DataTable();
                dt = documentos.ConsultarPartida_Gravamen(v_pedimento, v_partida, lblCadena.Text, ref mensaje);
                if (dt != null && dt.Rows.Count > 0)
                {
                    GridPartidaGravamen.DataSource = Session["GridPartidaGravamen"] = dt;
                    GridPartidaGravamen.DataBind();
                }

                //TASAS
                dt = new DataTable();
                dt = documentos.ConsultarPartida_Tasas(v_pedimento, v_partida, lblCadena.Text, ref mensaje);
                if (dt != null && dt.Rows.Count > 0)
                {
                    GridPartidaTasas.DataSource = Session["GridPartidaTasas"] = dt;
                    GridPartidaTasas.DataBind();
                }
                #endregion

                #region IDENTIFICADORES

                dt = new DataTable();
                dt = documentos.ConsultarPartida_Identificadores(v_pedimento, v_partida, lblCadena.Text, ref mensaje);
                if (dt != null && dt.Rows.Count > 0)
                {
                    GridPartidaIdentificadores.DataSource = Session["GridPartidaIdentificadores"] = dt;
                    GridPartidaIdentificadores.DataBind();
                }

                #endregion

                #region REGULACIONES

                dt = new DataTable();
                dt = documentos.ConsultarPartida_Regulaciones(v_pedimento, v_partida, lblCadena.Text, ref mensaje);
                if (dt != null && dt.Rows.Count > 0)
                {
                    GridPartidaRegulaciones.DataSource = Session["GridPartidaRegulaciones"] = dt;
                    GridPartidaRegulaciones.DataBind();
                }

                #endregion

                #region XML

                dt = new DataTable();
                MemoPartidaXML.Text = string.Empty;
                dt = documentos.ConsultarPartida_XML(v_pedimento, v_partida, lblCadena.Text, ref mensaje);
                if (dt != null && dt.Rows.Count > 0)
                {                    
                    string cadena = dt.Rows[0]["XML"].ToString().Trim() != string.Empty ? dt.Rows[0]["XML"].ToString().Trim() : "---";
                    if (!cadena.Equals("---"))
                    {
                        string formattedXml = XElement.Parse(cadena).ToString().Replace("<", "< ").Replace(">", " >");
                        MemoPartidaXML.Text = formattedXml;
                    }
                    else
                        MemoPartidaXML.Text = cadena;
                }
                #endregion

                #region CÓDIGOS

                dt = new DataTable();
                dt = documentos.ConsultarPartida_Codigos(v_pedimento, v_partida, lblCadena.Text, ref mensaje);
                if (dt != null && dt.Rows.Count > 0)
                {
                    GridPartidaCodigos.DataSource = Session["GridPartidaCodigos"] = dt;
                    GridPartidaCodigos.DataBind();
                }

                #endregion


                //Se requiere cargar los datos en Grid2 que son las Fechas(Año-Mes)
                if (Grid2.VisibleRowCount == 0)
                {
                    if (Session["FECHAS"] == null)
                        Session["FECHAS"] = docs.ConsultarFechasOperacion(lblCadena.Text, ref mensaje);

                    CreaSlideBar();
                }
            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (lblIdUsuario.Text.Length > 0)
                    idusuario = int.Parse(lblIdUsuario.Text);
                excepcion.RegistrarExcepcion(idusuario, "Documentos-cbxPartida_SelectedIndexChanged", ex, lblCadena.Text, ref mensaje);
            }

        }




        //Versión anterior del evento Grid_CustomButtonCallback
        //protected void Grid_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        //{
        //    string v_pedimento = "";
        //    string v_clave_pedimento = "";
        //    string v_tipo_operacion = "";
        //    DateTime v_DateTime;
        //    string v_fecha = "";


        //    DateTime? date_desde = new DateTime();
        //    DateTime? date_hasta = new DateTime();

        //    dynamic keyValue = Grid.GetRowValues(e.VisibleIndex, Grid.KeyFieldName);
        //    v_pedimento = keyValue;
        //    keyValue = Grid.GetRowValues(e.VisibleIndex, "CLAVE PEDIMENTO");
        //    v_clave_pedimento = keyValue;
        //    keyValue = Grid.GetRowValues(e.VisibleIndex, "TIPO PEDIMENTO");
        //    v_tipo_operacion = keyValue;
        //    keyValue = Grid.GetRowValues(e.VisibleIndex, "FECHA");
        //    v_DateTime = keyValue;
        //    v_fecha = v_DateTime.ToString("dd/MM/yyyy", CultureInfo.CurrentCulture);

        //    if (ASPx_Fecha_Desde.Text.Trim().Length > 0)
        //        date_desde = DateTime.Parse(ASPx_Fecha_Desde.Text);
        //    else
        //        date_desde = null;
        //    if (ASPx_Fecha_Hasta.Text.Trim().Length > 0)
        //        date_hasta = DateTime.Parse(ASPx_Fecha_Hasta.Text);
        //    else
        //        date_hasta = null;

        //    if (v_pedimento.Length > 0)
        //    {
        //        MultiView1.ActiveViewIndex = 1;

        //        //TituloPanel("Pedimento <b>(" + v_pedimento + ")</b>");
        //        tituloPanel = v_pedimento + " - " + v_clave_pedimento + " - " + v_tipo_operacion + " - " + v_fecha;
        //        DataBind();

        //        BoDocumentos documentos = new BoDocumentos();
        //        DataTable dt = new DataTable();
        //        string mensaje = "";

        //        #region PARTIDAS
        //        dt = new DataTable();
        //        dt = documentos.ConsultarPartidas(v_pedimento, lblCadena.Text, ref mensaje);
        //        if (dt != null && dt.Rows.Count > 0)
        //        {
        //            GridPartidas.DataSource = Session["GridPartidas"] = dt;
        //            GridPartidas.DataBind();

        //            ////Para cada columna en el GridPartidas se define modo de filtro CheckedList 
        //            //foreach (GridViewDataColumn column in GridPartidas.Columns)
        //            //    column.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
        //        }

        //        #endregion

        //        #region DATOS GENERALES
                
        //        dt = new DataTable();
        //        dt = documentos.ConsultarDatosGenerales(v_pedimento, lblCadena.Text, ref mensaje);

        //        if (dt != null && dt.Rows.Count > 0)
        //        {
        //            ASPx_PEDIMENTOARMADO.Text = dt.Rows[0]["PEDIMENTOARMADO"].ToString().Trim() != string.Empty ? dt.Rows[0]["PEDIMENTOARMADO"].ToString().Trim() : "---";
        //            ASPx_FECHA.Text = dt.Rows[0]["FECHA"].ToString().Trim() != string.Empty ? dt.Rows[0]["FECHA"].ToString().Trim() : "---";
        //            ASPx_TIPO_OPERACION.Text = dt.Rows[0]["TIPO DE OPERACION"].ToString().Trim() != string.Empty ? dt.Rows[0]["TIPO DE OPERACION"].ToString().Trim() : "---";
        //            ASPx_DESC_OPERACION.Text = dt.Rows[0]["DESCRIPCION TIPO DE OPERACION"].ToString().Trim() != string.Empty ? dt.Rows[0]["DESCRIPCION TIPO DE OPERACION"].ToString().Trim() : "---";
        //            ASPx_CLAVE_PEDIMENTO.Text = dt.Rows[0]["CLAVE PEDIMENTO"].ToString().Trim() != string.Empty ? dt.Rows[0]["CLAVE PEDIMENTO"].ToString().Trim() : "---";
        //            ASPx_DES_CLAVE_PED.Text = dt.Rows[0]["DESCRIPCION DE LA CLAVE PEDIMENTO"].ToString().Trim() != string.Empty ? dt.Rows[0]["DESCRIPCION DE LA CLAVE PEDIMENTO"].ToString().Trim() : "---";
        //            ASPx_CLAVE_PAIS_DESTINO.Text = dt.Rows[0]["CLAVE PAIS DESTINO"].ToString().Trim() != string.Empty ? dt.Rows[0]["CLAVE PAIS DESTINO"].ToString().Trim() : "---";
        //            ASPx_PAIS_DESTINO.Text = dt.Rows[0]["PAIS DESTINO"].ToString().Trim() != string.Empty ? dt.Rows[0]["PAIS DESTINO"].ToString().Trim() : "---";
        //            ASPx_CLAVE_ADUANA.Text = dt.Rows[0]["CLAVE ADUANA"].ToString().Trim() != string.Empty ? dt.Rows[0]["CLAVE ADUANA"].ToString().Trim() : "---";
        //            ASPx_ADUANA.Text = dt.Rows[0]["ADUANA"].ToString().Trim() != string.Empty ? dt.Rows[0]["ADUANA"].ToString().Trim() : "---";
        //            ASPx_PESO_BRUTO.Text = dt.Rows[0]["PESO BRUTO"].ToString().Trim() != string.Empty ? dt.Rows[0]["PESO BRUTO"].ToString().Trim() : "---";
        //            ASPx_TIPO_CAMBIO.Text = dt.Rows[0]["TIPO DE CAMBIO"].ToString().Trim() != string.Empty ? dt.Rows[0]["TIPO DE CAMBIO"].ToString().Trim() : "---";
        //            ASPx_MEDIO_TRANS_SALIDA.Text = dt.Rows[0]["MEDIO DE TRANSPORTE SALIDA"].ToString().Trim() != string.Empty ? dt.Rows[0]["MEDIO DE TRANSPORTE SALIDA"].ToString().Trim() : "---";
        //            ASPx_MEDIO_TRANS_SALIDA_DESC.Text = dt.Rows[0]["MEDIO DE TRANSPORTE SALIDA DESCRIPCION"].ToString().Trim() != string.Empty ? dt.Rows[0]["MEDIO DE TRANSPORTE SALIDA DESCRIPCION"].ToString().Trim() : "---";
        //            ASPx_MEDIO_TRANS_ARRIBO.Text = dt.Rows[0]["MEDIO DE TRANSPORTE ARRIBO"].ToString().Trim() != string.Empty ? dt.Rows[0]["MEDIO DE TRANSPORTE ARRIBO"].ToString().Trim() : "---";
        //            ASPx_MEDIO_TRANS_ARRIBO_DESC.Text = dt.Rows[0]["MEDIO DE TRANSPORTE ARRIBO DESCRIPCION"].ToString().Trim() != string.Empty ? dt.Rows[0]["MEDIO DE TRANSPORTE ARRIBO DESCRIPCION"].ToString().Trim() : "---";
        //            ASPx_MEDIO_TRANS_ENTRADA.Text = dt.Rows[0]["MEDIO DE TRANSPORTE ENTRADA"].ToString().Trim() != string.Empty ? dt.Rows[0]["MEDIO DE TRANSPORTE ENTRADA"].ToString().Trim() : "---";
        //            ASPx_MEDIO_TRANS_ENTRADA_DESC.Text = dt.Rows[0]["MEDIO DE TRANSPORTE ENTRADA DESCRIPCION"].ToString().Trim() != string.Empty ? dt.Rows[0]["MEDIO DE TRANSPORTE ENTRADA DESCRIPCION"].ToString().Trim() : "---";
        //            ASPx_CURO_APODERADO.Text = dt.Rows[0]["CURP APODERADO MANDATARIO"].ToString().Trim() != string.Empty ? dt.Rows[0]["CURP APODERADO MANDATARIO"].ToString().Trim() : "---";
        //            ASPx_RFC_AGENTE_ADUANAL.Text = dt.Rows[0]["RFC AGENTE ADUANAL"].ToString().Trim() != string.Empty ? dt.Rows[0]["RFC AGENTE ADUANAL"].ToString().Trim() : "---";
        //            ASPx_VALOR_DOLARES.Text = dt.Rows[0]["VALOR DOLARES"].ToString().Trim() != string.Empty ? dt.Rows[0]["VALOR DOLARES"].ToString().Trim() : "---";
        //            ASPx_VALOR_ADUANAL.Text = dt.Rows[0]["VALOR ADUANAL"].ToString().Trim() != string.Empty ? dt.Rows[0]["VALOR ADUANAL"].ToString().Trim() : "---";
        //            ASPx_VALOR_COMERCIAL.Text = dt.Rows[0]["VALOR COMERCIAL"].ToString().Trim() != string.Empty ? dt.Rows[0]["VALOR COMERCIAL"].ToString().Trim() : "---";
        //        }
        //        else
        //        {
        //            ASPx_PEDIMENTOARMADO.Text = "---";
        //            ASPx_FECHA.Text = "---";
        //            ASPx_TIPO_OPERACION.Text = "---";
        //            ASPx_DESC_OPERACION.Text = "---";
        //            ASPx_CLAVE_PEDIMENTO.Text = "---";
        //            ASPx_DES_CLAVE_PED.Text = "---";
        //            ASPx_CLAVE_PAIS_DESTINO.Text = "---";
        //            ASPx_PAIS_DESTINO.Text = "---";
        //            ASPx_CLAVE_ADUANA.Text = "---";
        //            ASPx_ADUANA.Text = "---";
        //            ASPx_PESO_BRUTO.Text = "---";
        //            ASPx_TIPO_CAMBIO.Text = "---";
        //            ASPx_MEDIO_TRANS_SALIDA.Text = "---";
        //            ASPx_MEDIO_TRANS_SALIDA_DESC.Text = "---";
        //            ASPx_MEDIO_TRANS_ARRIBO.Text = "---";
        //            ASPx_MEDIO_TRANS_ARRIBO_DESC.Text = "---";
        //            ASPx_MEDIO_TRANS_ENTRADA.Text = "---";
        //            ASPx_MEDIO_TRANS_ENTRADA_DESC.Text = "---";
        //            ASPx_CURO_APODERADO.Text = "---";
        //            ASPx_RFC_AGENTE_ADUANAL.Text = "---";
        //            ASPx_VALOR_DOLARES.Text = "---";
        //            ASPx_VALOR_ADUANAL.Text = "---";
        //            ASPx_VALOR_COMERCIAL.Text = "---";
        //        }

        //        #endregion

        //        #region IMPORTADOR/EXPORTADOR

        //        dt = new DataTable();
        //        dt = documentos.ConsultarIE(v_pedimento, lblCadena.Text, ref mensaje);

        //        if (dt != null && dt.Rows.Count > 0)
        //        {
        //            ASPx_IE_RFC.Text = dt.Rows[0]["IE_RFC"].ToString().Trim() != string.Empty ? dt.Rows[0]["IE_RFC"].ToString().Trim() : "---";
        //            ASPx_IE_CURP.Text = dt.Rows[0]["IE_CURP"].ToString().Trim() != string.Empty ? dt.Rows[0]["IE_CURP"].ToString().Trim() : "---";
        //            ASPx_IE_RAZON_SOCIAL.Text = dt.Rows[0]["IE_RAZON_SOCIAL"].ToString().Trim() != string.Empty ? dt.Rows[0]["IE_RAZON_SOCIAL"].ToString().Trim() : "---";
        //            ASPx_IE_CALLE.Text = dt.Rows[0]["IE_CALLE"].ToString().Trim() != string.Empty ? dt.Rows[0]["IE_CALLE"].ToString().Trim() : "---";
        //            ASPx_IE_NUMERO_EXTERIOR.Text = dt.Rows[0]["IE_NUMERO_EXTERIOR"].ToString().Trim() != string.Empty ? dt.Rows[0]["IE_NUMERO_EXTERIOR"].ToString().Trim() : "---";
        //            ASPx_IE_NUMERO_INTERIOR.Text = dt.Rows[0]["IE_NUMERO_INTERIOR"].ToString().Trim() != string.Empty ? dt.Rows[0]["IE_NUMERO_INTERIOR"].ToString().Trim() : "---";
        //            ASPx_IE_MUNICIPIO.Text = dt.Rows[0]["IE_MUNICIPIO"].ToString().Trim() != string.Empty ? dt.Rows[0]["IE_MUNICIPIO"].ToString().Trim() : "---";
        //            ASPx_IE_CODIGO_POSTAL.Text = dt.Rows[0]["IE_CODIGO_POSTAL"].ToString().Trim() != string.Empty ? dt.Rows[0]["IE_CODIGO_POSTAL"].ToString().Trim() : "---";
        //            ASPx_IE_PAIS.Text = dt.Rows[0]["IE_PAIS"].ToString().Trim() != string.Empty ? dt.Rows[0]["IE_PAIS"].ToString().Trim() : "---";
        //            ASPx_SEGUROS.Text = dt.Rows[0]["SEGUROS"].ToString().Trim() != string.Empty ? dt.Rows[0]["SEGUROS"].ToString().Trim() : "---";
        //            ASPx_FLETES.Text = dt.Rows[0]["FLETES"].ToString().Trim() != string.Empty ? dt.Rows[0]["FLETES"].ToString().Trim() : "---";
        //            ASPx_EMBALAJES.Text = dt.Rows[0]["EMBALAJES"].ToString().Trim() != string.Empty ? dt.Rows[0]["EMBALAJES"].ToString().Trim() : "---";
        //            ASPx_INCREMENTABLES.Text = dt.Rows[0]["INCREMENTABLES"].ToString().Trim() != string.Empty ? dt.Rows[0]["INCREMENTABLES"].ToString().Trim() : "---";
        //            ASPx_ADUANA_DESPACHO.Text = dt.Rows[0]["ADUANA_DESPACHO"].ToString().Trim() != string.Empty ? dt.Rows[0]["ADUANA_DESPACHO"].ToString().Trim() : "---";
        //            ASPx_DESCRIPCION_ADUANA_DESPACHO.Text = dt.Rows[0]["DESCRIPCION_ADUANA_DESPACHO"].ToString().Trim() != string.Empty ? dt.Rows[0]["DESCRIPCION_ADUANA_DESPACHO"].ToString().Trim() : "---";
        //            ASPx_BULTOS.Text = dt.Rows[0]["BULTOS"].ToString().Trim() != string.Empty ? dt.Rows[0]["BULTOS"].ToString().Trim() : "---";
        //            ASPx_EFECTIVO.Text = dt.Rows[0]["EFECTIVO"].ToString().Trim() != string.Empty ? dt.Rows[0]["EFECTIVO"].ToString().Trim() : "---";
        //            ASPx_OTROS.Text = dt.Rows[0]["OTROS"].ToString().Trim() != string.Empty ? dt.Rows[0]["OTROS"].ToString().Trim() : "---";
        //            ASPx_TOTAL.Text = dt.Rows[0]["TOTAL"].ToString().Trim() != string.Empty ? dt.Rows[0]["TOTAL"].ToString().Trim() : "---";
        //            ASPx_CLAVE_PAIS.Text = dt.Rows[0]["CLAVE_PAIS"].ToString().Trim() != string.Empty ? dt.Rows[0]["CLAVE_PAIS"].ToString().Trim() : "---";
        //            ASPx_DESCRIPCION_PAIS.Text = dt.Rows[0]["DESCRIPCION_PAIS"].ToString().Trim() != string.Empty ? dt.Rows[0]["DESCRIPCION_PAIS"].ToString().Trim() : "---";
        //        }
        //        else
        //        {
        //            ASPx_IE_RFC.Text = "---";
        //            ASPx_IE_CURP.Text = "---";
        //            ASPx_IE_RAZON_SOCIAL.Text = "---";
        //            ASPx_IE_CALLE.Text = "---";
        //            ASPx_IE_NUMERO_EXTERIOR.Text = "---";
        //            ASPx_IE_NUMERO_INTERIOR.Text = "---";
        //            ASPx_IE_MUNICIPIO.Text = "---";
        //            ASPx_IE_CODIGO_POSTAL.Text = "---";
        //            ASPx_IE_PAIS.Text = "---";
        //            ASPx_SEGUROS.Text = "---";
        //            ASPx_FLETES.Text = "---";
        //            ASPx_EMBALAJES.Text = "---";
        //            ASPx_INCREMENTABLES.Text = "---";
        //            ASPx_ADUANA_DESPACHO.Text = "---";
        //            ASPx_DESCRIPCION_ADUANA_DESPACHO.Text = "---";
        //            ASPx_BULTOS.Text = "---";
        //            ASPx_EFECTIVO.Text = "---";
        //            ASPx_OTROS.Text = "---";
        //            ASPx_TOTAL.Text = "---";
        //            ASPx_CLAVE_PAIS.Text = "---";
        //            ASPx_DESCRIPCION_PAIS.Text = "---";
        //        }

        //        #endregion

        //        #region COMPRADORES/VENDEDORES

        //        dt = new DataTable();
        //        dt = documentos.ConsultarCV(v_pedimento, lblCadena.Text, ref mensaje);
        //        if (dt != null && dt.Rows.Count > 0)
        //        {
        //            GridCV.DataSource = Session["GridCV"] = dt;
        //            GridCV.DataBind();

        //            //Para cada columna en el GridCV se define modo de filtro CheckedList 
        //            foreach (GridViewDataColumn column in GridCV.Columns)
        //                column.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
        //        }

        //        #endregion

        //        #region IDENTIFICADORES GENERALES

        //        dt = new DataTable();
        //        dt = documentos.ConsultarIG(v_pedimento, lblCadena.Text, ref mensaje);
        //        if (dt.Rows.Count > 0)
        //        {
        //            Session["GridIG"] = dt;
        //            GridIG.DataSource = dt;
        //            GridIG.DataBind();

        //            //Para cada columna en el GridIG se define modo de filtro CheckedList 
        //            foreach (GridViewDataColumn column in GridIG.Columns)
        //                column.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
        //        }

        //        #endregion

        //        #region FLETES

        //        dt = new DataTable();
        //        dt = documentos.ConsultarFletes(v_pedimento, lblCadena.Text, ref mensaje);

        //        if (dt != null && dt.Rows.Count > 0)
        //        {
        //            ASPx_PROVEEDOR.Text = dt.Rows[0]["PROVEEDOR"].ToString().Trim() != string.Empty ? dt.Rows[0]["PROVEEDOR"].ToString().Trim() : "---";
        //            ASPx_FACTURA.Text = dt.Rows[0]["FACTURA"].ToString().Trim() != string.Empty ? dt.Rows[0]["FACTURA"].ToString().Trim() : "---";
        //            ASPx_IMPORTE.Text = dt.Rows[0]["IMPORTE"].ToString().Trim() != string.Empty ? dt.Rows[0]["IMPORTE"].ToString().Trim() : "---";
        //        }
        //        else
        //        {
        //            ASPx_PROVEEDOR.Text = "---";
        //            ASPx_FACTURA.Text = "---";
        //            ASPx_IMPORTE.Text = "---";

        //        }

        //        #endregion

        //        #region INCREMENTABLES

        //        //dt = new DataTable();
        //        //dt = documentos.ConsultarIncrementables(v_pedimento, lblCadena.Text, ref mensaje);

        //        //if (dt != null && dt.Rows.Count > 0)
        //        //{
        //        //    ASPx_INC_VALOR_SEGUROS.Text = dt.Rows[0]["VALOR_SEGUROS"].ToString().Trim() != string.Empty ? dt.Rows[0]["VALOR_SEGUROS"].ToString().Trim() : "---";
        //        //    ASPx_INC_FLETES.Text = dt.Rows[0]["FLETES"].ToString().Trim() != string.Empty ? dt.Rows[0]["FLETES"].ToString().Trim() : "---";
        //        //    ASPx_INC_SEGUROS.Text = dt.Rows[0]["SEGUROS"].ToString().Trim() != string.Empty ? dt.Rows[0]["SEGUROS"].ToString().Trim() : "---";
        //        //    ASPx_INC_EMBALAJES.Text = dt.Rows[0]["EMBALAJES"].ToString().Trim() != string.Empty ? dt.Rows[0]["EMBALAJES"].ToString().Trim() : "---";
        //        //    ASPx_INC_OTROS_INCREMENTABLES.Text = dt.Rows[0]["OTROS_INCREMENTABLES"].ToString().Trim() != string.Empty ? dt.Rows[0]["OTROS_INCREMENTABLES"].ToString().Trim() : "---";
        //        //}
        //        //else
        //        //{
        //        //    ASPx_INC_VALOR_SEGUROS.Text = "---";
        //        //    ASPx_INC_FLETES.Text = "---";
        //        //    ASPx_INC_SEGUROS.Text = "---";
        //        //    ASPx_INC_EMBALAJES.Text = "---";
        //        //    ASPx_INC_OTROS_INCREMENTABLES.Text = "---";
        //        //}

        //        #endregion

        //        #region FACTURAS

        //        dt = new DataTable();
        //        dt = documentos.ConsultarFacturas(v_pedimento, lblCadena.Text, ref mensaje);
        //        if (dt != null && dt.Rows.Count > 0)
        //        {
        //            GridFacturas.DataSource = Session["GridFacturas"] = dt;
        //            GridFacturas.DataBind();

        //            //Para cada columna en el GridFacturas se define modo de filtro CheckedList 
        //            foreach (GridViewDataColumn column in GridFacturas.Columns)
        //                column.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
        //        }

        //        #endregion

        //        #region FECHAS

        //        dt = new DataTable();
        //        dt = documentos.ConsultarFechas(v_pedimento, lblCadena.Text, ref mensaje);
        //        if (dt != null && dt.Rows.Count > 0)
        //        {
        //            GridFechas.DataSource = Session["GridFechas"] = dt;
        //            GridFechas.DataBind();

        //            //Para cada columna en el GridFechas se define modo de filtro CheckedList 
        //            foreach (GridViewDataColumn column in GridFechas.Columns)
        //                column.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
        //        }

        //        #endregion

        //        #region CONTRIBUCIONES GENERALES

        //        dt = new DataTable();
        //        dt = documentos.ConsultarContribucionesGenerales(v_pedimento, lblCadena.Text, ref mensaje);
        //        if (dt != null && dt.Rows.Count > 0)
        //        {
        //            GridContriGrales.DataSource = Session["GridContenedores"] = dt;
        //            GridContriGrales.DataBind();

        //            //Para cada columna en el GridContriGrales se define modo de filtro CheckedList 
        //            foreach (GridViewDataColumn column in GridContriGrales.Columns)
        //                column.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
        //        }

        //        #endregion

        //        #region IMPUESTOS

        //        dt = new DataTable();
        //        dt = documentos.ConsultarImpuestos(v_pedimento, lblCadena.Text, ref mensaje);
        //        if (dt != null && dt.Rows.Count > 0)
        //        {
        //            GridImpuestos.DataSource = Session["GridImpuestos"] = dt;
        //            GridImpuestos.DataBind();

        //            //Para cada columna en el GridImpuestos se define modo de filtro CheckedList 
        //            foreach (GridViewDataColumn column in GridImpuestos.Columns)
        //                column.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
        //        }

        //        #endregion

        //        #region TRANSPORTE

        //        dt = new DataTable();
        //        dt = documentos.ConsultarTransporte(v_pedimento, lblCadena.Text, ref mensaje);
        //        if (dt != null && dt.Rows.Count > 0)
        //        {
        //            GridTransporte.DataSource = Session["GridTransporte"] = dt;
        //            GridTransporte.DataBind();

        //            //Para cada columna en el GridTransporte se define modo de filtro CheckedList 
        //            foreach (GridViewDataColumn column in GridTransporte.Columns)
        //                column.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
        //        }

        //        #endregion

        //        #region GUIAS

        //        dt = new DataTable();
        //        dt = documentos.ConsultarGuias(v_pedimento, lblCadena.Text, ref mensaje);
        //        if (dt != null && dt.Rows.Count > 0)
        //        {
        //            GridGuias.DataSource = Session["GridGuias"] = dt;
        //            GridGuias.DataBind();

        //            //Para cada columna en el GridGuias se define modo de filtro CheckedList 
        //            foreach (GridViewDataColumn column in GridGuias.Columns)
        //                column.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
        //        }

        //        #endregion

        //        #region CONTENEDORES

        //        dt = new DataTable();
        //        dt = documentos.ConsultarContenedores(v_pedimento, lblCadena.Text, ref mensaje);
        //        if (dt != null && dt.Rows.Count > 0)
        //        {
        //            GridContenedores.DataSource = Session["GridContenedores"] = dt;
        //            GridContenedores.DataBind();

        //            //Para cada columna en el GridContenedores se define modo de filtro CheckedList 
        //            foreach (GridViewDataColumn column in GridContenedores.Columns)
        //                column.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
        //        }

        //        #endregion

        //        #region OBSERVACIONES

        //        dt = new DataTable();
        //        dt = documentos.ConsultarObservaciones(v_pedimento, lblCadena.Text, ref mensaje);

        //        if (dt != null && dt.Rows.Count > 0)
        //        {
        //            ASPx_OBSERVACIONES.Text = dt.Rows[0]["OBSERVACIONES"].ToString().Trim() != string.Empty ? dt.Rows[0]["OBSERVACIONES"].ToString().Trim() : "---";
        //        }

        //        #endregion

        //        #region DESCARGOS

        //        dt = new DataTable();
        //        dt = documentos.ConsultarDescargos(v_pedimento, lblCadena.Text, ref mensaje);
        //        if (dt != null && dt.Rows.Count > 0)
        //        {
        //            GridDescargos.DataSource = Session["GridDescargos"] = dt;
        //            GridDescargos.DataBind();

        //            //Para cada columna en el GridDescargos se define modo de filtro CheckedList 
        //            foreach (GridViewDataColumn column in GridDescargos.Columns)
        //                column.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
        //        }

        //        #endregion

        //    }
        //}

        //protected void Grid_SelectionChanged(object sender, EventArgs e)
        //{
        //    string v_pedimento = "";
        //    DateTime? date_desde = new DateTime();
        //    DateTime? date_hasta = new DateTime();

        //    v_pedimento = Grid.GetSelectedFieldValues("PEDIMENTOARMADO")[0].ToString();

        //    if (ASPx_Fecha_Desde.Text.Trim().Length > 0)
        //        date_desde = DateTime.Parse(ASPx_Fecha_Desde.Text);
        //    else
        //        date_desde = null;
        //    if (ASPx_Fecha_Hasta.Text.Trim().Length > 0)
        //        date_hasta = DateTime.Parse(ASPx_Fecha_Hasta.Text);
        //    else
        //        date_hasta = null;

        //    if(v_pedimento.Length > 0)
        //    {
        //        MultiView1.ActiveViewIndex = 1;

        //        //TituloPanel("Pedimento <b>(" + v_pedimento + ")</b>");
        //        tituloPanel = "Pedimento <b>(" + v_pedimento + ")</b>";
        //        DataBind();

        //        BoDocumentos documentos = new BoDocumentos();

        //        #region DATOS GENERALES
        //        DataTable dt = new DataTable();
        //        string mensaje = "";
        //        dt = documentos.ConsultarDatosGenerales(v_pedimento, lblCadena.Text, ref mensaje);

        //        if (dt != null && dt.Rows.Count > 0)
        //        {                    
        //            ASPx_PEDIMENTOARMADO.Text = dt.Rows[0]["PEDIMENTOARMADO"].ToString().Trim() != string.Empty ? dt.Rows[0]["PEDIMENTOARMADO"].ToString().Trim() : "---";
        //            ASPx_FECHA.Text = dt.Rows[0]["FECHA"].ToString().Trim() != string.Empty ? dt.Rows[0]["FECHA"].ToString().Trim() : "---";
        //            ASPx_TIPO_OPERACION.Text = dt.Rows[0]["TIPO DE OPERACION"].ToString().Trim() != string.Empty ? dt.Rows[0]["TIPO DE OPERACION"].ToString().Trim() : "---";
        //            ASPx_DESC_OPERACION.Text = dt.Rows[0]["DESCRIPCION TIPO DE OPERACION"].ToString().Trim() != string.Empty ? dt.Rows[0]["DESCRIPCION TIPO DE OPERACION"].ToString().Trim() : "---";
        //            ASPx_CLAVE_PEDIMENTO.Text = dt.Rows[0]["CLAVE PEDIMENTO"].ToString().Trim() != string.Empty ? dt.Rows[0]["CLAVE PEDIMENTO"].ToString().Trim() : "---";
        //            ASPx_DES_CLAVE_PED.Text = dt.Rows[0]["DESCRIPCION DE LA CLAVE PEDIMENTO"].ToString().Trim() != string.Empty ? dt.Rows[0]["DESCRIPCION DE LA CLAVE PEDIMENTO"].ToString().Trim() : "---";
        //            ASPx_CLAVE_PAIS_DESTINO.Text = dt.Rows[0]["CLAVE PAIS DESTINO"].ToString().Trim() != string.Empty ? dt.Rows[0]["CLAVE PAIS DESTINO"].ToString().Trim() : "---";
        //            ASPx_PAIS_DESTINO.Text = dt.Rows[0]["PAIS DESTINO"].ToString().Trim() != string.Empty ? dt.Rows[0]["PAIS DESTINO"].ToString().Trim() : "---";
        //            ASPx_CLAVE_ADUANA.Text = dt.Rows[0]["CLAVE ADUANA"].ToString().Trim() != string.Empty ? dt.Rows[0]["CLAVE ADUANA"].ToString().Trim() : "---";
        //            ASPx_ADUANA.Text = dt.Rows[0]["ADUANA"].ToString().Trim() != string.Empty ? dt.Rows[0]["ADUANA"].ToString().Trim() : "---";
        //            ASPx_PESO_BRUTO.Text = dt.Rows[0]["PESO BRUTO"].ToString().Trim() != string.Empty ? dt.Rows[0]["PESO BRUTO"].ToString().Trim() : "---";
        //            ASPx_TIPO_CAMBIO.Text = dt.Rows[0]["TIPO DE CAMBIO"].ToString().Trim() != string.Empty ? dt.Rows[0]["TIPO DE CAMBIO"].ToString().Trim() : "---";
        //            ASPx_MEDIO_TRANS_SALIDA.Text = dt.Rows[0]["MEDIO DE TRANSPORTE SALIDA"].ToString().Trim() != string.Empty ? dt.Rows[0]["MEDIO DE TRANSPORTE SALIDA"].ToString().Trim() : "---";
        //            ASPx_MEDIO_TRANS_SALIDA_DESC.Text = dt.Rows[0]["MEDIO DE TRANSPORTE SALIDA DESCRIPCION"].ToString().Trim() != string.Empty ? dt.Rows[0]["MEDIO DE TRANSPORTE SALIDA DESCRIPCION"].ToString().Trim() : "---";
        //            ASPx_MEDIO_TRANS_ARRIBO.Text = dt.Rows[0]["MEDIO DE TRANSPORTE ARRIBO"].ToString().Trim() != string.Empty ? dt.Rows[0]["MEDIO DE TRANSPORTE ARRIBO"].ToString().Trim() : "---";
        //            ASPx_MEDIO_TRANS_ARRIBO_DESC.Text = dt.Rows[0]["MEDIO DE TRANSPORTE ARRIBO DESCRIPCION"].ToString().Trim() != string.Empty ? dt.Rows[0]["MEDIO DE TRANSPORTE ARRIBO DESCRIPCION"].ToString().Trim() : "---";
        //            ASPx_MEDIO_TRANS_ENTRADA.Text = dt.Rows[0]["MEDIO DE TRANSPORTE ENTRADA"].ToString().Trim() != string.Empty ? dt.Rows[0]["MEDIO DE TRANSPORTE ENTRADA"].ToString().Trim() : "---";
        //            ASPx_MEDIO_TRANS_ENTRADA_DESC.Text = dt.Rows[0]["MEDIO DE TRANSPORTE ENTRADA DESCRIPCION"].ToString().Trim() != string.Empty ? dt.Rows[0]["MEDIO DE TRANSPORTE ENTRADA DESCRIPCION"].ToString().Trim() : "---";
        //            ASPx_CURO_APODERADO.Text = dt.Rows[0]["CURP APODERADO MANDATARIO"].ToString().Trim() != string.Empty ? dt.Rows[0]["CURP APODERADO MANDATARIO"].ToString().Trim() : "---";
        //            ASPx_RFC_AGENTE_ADUANAL.Text = dt.Rows[0]["RFC AGENTE ADUANAL"].ToString().Trim() != string.Empty ? dt.Rows[0]["RFC AGENTE ADUANAL"].ToString().Trim() : "---";
        //            ASPx_VALOR_DOLARES.Text = dt.Rows[0]["VALOR DOLARES"].ToString().Trim() != string.Empty ? dt.Rows[0]["VALOR DOLARES"].ToString().Trim() : "---";
        //            ASPx_VALOR_ADUANAL.Text = dt.Rows[0]["VALOR ADUANAL"].ToString().Trim() != string.Empty ? dt.Rows[0]["VALOR ADUANAL"].ToString().Trim() : "---";
        //            ASPx_VALOR_COMERCIAL.Text = dt.Rows[0]["VALOR COMERCIAL"].ToString().Trim() != string.Empty ? dt.Rows[0]["VALOR COMERCIAL"].ToString().Trim() : "---";
        //        }
        //        else
        //        {
        //            ASPx_PEDIMENTOARMADO.Text = "---";
        //            ASPx_FECHA.Text = "---";
        //            ASPx_TIPO_OPERACION.Text = "---";
        //            ASPx_DESC_OPERACION.Text = "---";
        //            ASPx_CLAVE_PEDIMENTO.Text = "---";
        //            ASPx_DES_CLAVE_PED.Text = "---";
        //            ASPx_CLAVE_PAIS_DESTINO.Text = "---";
        //            ASPx_PAIS_DESTINO.Text = "---";
        //            ASPx_CLAVE_ADUANA.Text = "---";
        //            ASPx_ADUANA.Text = "---";
        //            ASPx_PESO_BRUTO.Text = "---";
        //            ASPx_TIPO_CAMBIO.Text = "---";
        //            ASPx_MEDIO_TRANS_SALIDA.Text = "---";
        //            ASPx_MEDIO_TRANS_SALIDA_DESC.Text = "---";
        //            ASPx_MEDIO_TRANS_ARRIBO.Text = "---";
        //            ASPx_MEDIO_TRANS_ARRIBO_DESC.Text = "---";
        //            ASPx_MEDIO_TRANS_ENTRADA.Text = "---";
        //            ASPx_MEDIO_TRANS_ENTRADA_DESC.Text = "---";
        //            ASPx_CURO_APODERADO.Text = "---";
        //            ASPx_RFC_AGENTE_ADUANAL.Text = "---";
        //            ASPx_VALOR_DOLARES.Text = "---";
        //            ASPx_VALOR_ADUANAL.Text = "---";
        //            ASPx_VALOR_COMERCIAL.Text = "---";
        //        }

        //        #endregion

        //        #region IMPORTADOR/EXPORTADOR

        //        dt = new DataTable();
        //        dt = documentos.ConsultarIE(v_pedimento, lblCadena.Text, ref mensaje);

        //        if (dt != null && dt.Rows.Count > 0)
        //        {
        //            ASPx_IE_RFC.Text = dt.Rows[0]["IE_RFC"].ToString().Trim() != string.Empty ? dt.Rows[0]["IE_RFC"].ToString().Trim() : "---";
        //            ASPx_IE_CURP.Text = dt.Rows[0]["IE_CURP"].ToString().Trim() != string.Empty ? dt.Rows[0]["IE_CURP"].ToString().Trim() : "---";
        //            ASPx_IE_RAZON_SOCIAL.Text = dt.Rows[0]["IE_RAZON_SOCIAL"].ToString().Trim() != string.Empty ? dt.Rows[0]["IE_RAZON_SOCIAL"].ToString().Trim() : "---";
        //            ASPx_IE_CALLE.Text = dt.Rows[0]["IE_CALLE"].ToString().Trim() != string.Empty ? dt.Rows[0]["IE_CALLE"].ToString().Trim() : "---";
        //            ASPx_IE_NUMERO_EXTERIOR.Text = dt.Rows[0]["IE_NUMERO_EXTERIOR"].ToString().Trim() != string.Empty ? dt.Rows[0]["IE_NUMERO_EXTERIOR"].ToString().Trim() : "---";
        //            ASPx_IE_NUMERO_INTERIOR.Text = dt.Rows[0]["IE_NUMERO_INTERIOR"].ToString().Trim() != string.Empty ? dt.Rows[0]["IE_NUMERO_INTERIOR"].ToString().Trim() : "---";
        //            ASPx_IE_MUNICIPIO.Text = dt.Rows[0]["IE_MUNICIPIO"].ToString().Trim() != string.Empty ? dt.Rows[0]["IE_MUNICIPIO"].ToString().Trim() : "---";
        //            ASPx_IE_CODIGO_POSTAL.Text = dt.Rows[0]["IE_CODIGO_POSTAL"].ToString().Trim() != string.Empty ? dt.Rows[0]["IE_CODIGO_POSTAL"].ToString().Trim() : "---";
        //            ASPx_IE_PAIS.Text = dt.Rows[0]["IE_PAIS"].ToString().Trim() != string.Empty ? dt.Rows[0]["IE_PAIS"].ToString().Trim() : "---";
        //            ASPx_SEGUROS.Text = dt.Rows[0]["SEGUROS"].ToString().Trim() != string.Empty ? dt.Rows[0]["SEGUROS"].ToString().Trim() : "---";
        //            ASPx_FLETES.Text = dt.Rows[0]["FLETES"].ToString().Trim() != string.Empty ? dt.Rows[0]["FLETES"].ToString().Trim() : "---";
        //            ASPx_EMBALAJES.Text = dt.Rows[0]["EMBALAJES"].ToString().Trim() != string.Empty ? dt.Rows[0]["EMBALAJES"].ToString().Trim() : "---";
        //            ASPx_INCREMENTABLES.Text = dt.Rows[0]["INCREMENTABLES"].ToString().Trim() != string.Empty ? dt.Rows[0]["INCREMENTABLES"].ToString().Trim() : "---";
        //            ASPx_ADUANA_DESPACHO.Text = dt.Rows[0]["ADUANA_DESPACHO"].ToString().Trim() != string.Empty ? dt.Rows[0]["ADUANA_DESPACHO"].ToString().Trim() : "---";
        //            ASPx_DESCRIPCION_ADUANA_DESPACHO.Text = dt.Rows[0]["DESCRIPCION_ADUANA_DESPACHO"].ToString().Trim() != string.Empty ? dt.Rows[0]["DESCRIPCION_ADUANA_DESPACHO"].ToString().Trim() : "---";
        //            ASPx_BULTOS.Text = dt.Rows[0]["BULTOS"].ToString().Trim() != string.Empty ? dt.Rows[0]["BULTOS"].ToString().Trim() : "---";
        //            ASPx_EFECTIVO.Text = dt.Rows[0]["EFECTIVO"].ToString().Trim() != string.Empty ? dt.Rows[0]["EFECTIVO"].ToString().Trim() : "---";
        //            ASPx_OTROS.Text = dt.Rows[0]["OTROS"].ToString().Trim() != string.Empty ? dt.Rows[0]["OTROS"].ToString().Trim() : "---";
        //            ASPx_TOTAL.Text = dt.Rows[0]["TOTAL"].ToString().Trim() != string.Empty ? dt.Rows[0]["TOTAL"].ToString().Trim() : "---";
        //            ASPx_CLAVE_PAIS.Text = dt.Rows[0]["CLAVE_PAIS"].ToString().Trim() != string.Empty ? dt.Rows[0]["CLAVE_PAIS"].ToString().Trim() : "---";
        //            ASPx_DESCRIPCION_PAIS.Text = dt.Rows[0]["DESCRIPCION_PAIS"].ToString().Trim() != string.Empty ? dt.Rows[0]["DESCRIPCION_PAIS"].ToString().Trim() : "---";
        //        }
        //        else
        //        {
        //            ASPx_IE_RFC.Text ="---";
        //            ASPx_IE_CURP.Text = "---";
        //            ASPx_IE_RAZON_SOCIAL.Text = "---";
        //            ASPx_IE_CALLE.Text = "---";
        //            ASPx_IE_NUMERO_EXTERIOR.Text = "---";
        //            ASPx_IE_NUMERO_INTERIOR.Text = "---";
        //            ASPx_IE_MUNICIPIO.Text = "---";
        //            ASPx_IE_CODIGO_POSTAL.Text = "---";
        //            ASPx_IE_PAIS.Text = "---";
        //            ASPx_SEGUROS.Text = "---";
        //            ASPx_FLETES.Text = "---";
        //            ASPx_EMBALAJES.Text = "---";
        //            ASPx_INCREMENTABLES.Text = "---";
        //            ASPx_ADUANA_DESPACHO.Text = "---";
        //            ASPx_DESCRIPCION_ADUANA_DESPACHO.Text = "---";
        //            ASPx_BULTOS.Text = "---";
        //            ASPx_EFECTIVO.Text = "---";
        //            ASPx_OTROS.Text ="---";
        //            ASPx_TOTAL.Text = "---";
        //            ASPx_CLAVE_PAIS.Text = "---";
        //            ASPx_DESCRIPCION_PAIS.Text = "---";
        //        }

        //        #endregion

        //        #region COMPRADORES/VENDEDORES

        //        dt = new DataTable();
        //        dt = documentos.ConsultarCV(v_pedimento, lblCadena.Text, ref mensaje);
        //        if (dt != null && dt.Rows.Count > 0)
        //        {
        //            GridCV.DataSource = Session["GridCV"] = dt;
        //            GridCV.DataBind();
        //        }

        //        #endregion

        //        #region IDENTIFICADORES GENERALES

        //        dt = new DataTable();
        //        dt = documentos.ConsultarIG(v_pedimento, lblCadena.Text, ref mensaje);
        //        if (dt.Rows.Count > 0)
        //        {
        //            Session["GridIG"] = dt;
        //            GridIG.DataSource = dt;
        //            GridIG.DataBind();
        //        }

        //        #endregion

        //        #region FLETES

        //        dt = new DataTable();
        //        dt = documentos.ConsultarFletes(v_pedimento, lblCadena.Text, ref mensaje);

        //        if (dt != null && dt.Rows.Count > 0)
        //        {
        //            ASPx_PROVEEDOR.Text = dt.Rows[0]["PROVEEDOR"].ToString().Trim() != string.Empty ? dt.Rows[0]["PROVEEDOR"].ToString().Trim() : "---";
        //            ASPx_FACTURA.Text = dt.Rows[0]["FACTURA"].ToString().Trim() != string.Empty ? dt.Rows[0]["FACTURA"].ToString().Trim() : "---";
        //            ASPx_IMPORTE.Text = dt.Rows[0]["IMPORTE"].ToString().Trim() != string.Empty ? dt.Rows[0]["IMPORTE"].ToString().Trim() : "---";
        //        }
        //        else
        //        {
        //            ASPx_PROVEEDOR.Text = "---";
        //            ASPx_FACTURA.Text = "---";
        //            ASPx_IMPORTE.Text = "---";

        //        }

        //        #endregion

        //        #region INCREMENTABLES

        //        //dt = new DataTable();
        //        //dt = documentos.ConsultarIncrementables(v_pedimento, lblCadena.Text, ref mensaje);

        //        //if (dt != null && dt.Rows.Count > 0)
        //        //{
        //        //    ASPx_INC_VALOR_SEGUROS.Text = dt.Rows[0]["VALOR_SEGUROS"].ToString().Trim() != string.Empty ? dt.Rows[0]["VALOR_SEGUROS"].ToString().Trim() : "---";
        //        //    ASPx_INC_FLETES.Text = dt.Rows[0]["FLETES"].ToString().Trim() != string.Empty ? dt.Rows[0]["FLETES"].ToString().Trim() : "---";
        //        //    ASPx_INC_SEGUROS.Text = dt.Rows[0]["SEGUROS"].ToString().Trim() != string.Empty ? dt.Rows[0]["SEGUROS"].ToString().Trim() : "---";
        //        //    ASPx_INC_EMBALAJES.Text = dt.Rows[0]["EMBALAJES"].ToString().Trim() != string.Empty ? dt.Rows[0]["EMBALAJES"].ToString().Trim() : "---";
        //        //    ASPx_INC_OTROS_INCREMENTABLES.Text = dt.Rows[0]["OTROS_INCREMENTABLES"].ToString().Trim() != string.Empty ? dt.Rows[0]["OTROS_INCREMENTABLES"].ToString().Trim() : "---";
        //        //}
        //        //else
        //        //{
        //        //    ASPx_INC_VALOR_SEGUROS.Text = "---";
        //        //    ASPx_INC_FLETES.Text = "---";
        //        //    ASPx_INC_SEGUROS.Text = "---";
        //        //    ASPx_INC_EMBALAJES.Text = "---";
        //        //    ASPx_INC_OTROS_INCREMENTABLES.Text = "---";
        //        //}

        //        #endregion

        //        #region FACTURAS

        //        dt = new DataTable();
        //        dt = documentos.ConsultarFacturas(v_pedimento,lblCadena.Text,  ref mensaje);
        //        if (dt != null && dt.Rows.Count > 0)
        //        {
        //            GridFacturas.DataSource = Session["GridFacturas"] = dt;
        //            GridFacturas.DataBind();
        //        }

        //        #endregion

        //        #region FECHAS

        //        dt = new DataTable();
        //        dt = documentos.ConsultarFechas(v_pedimento, lblCadena.Text, ref mensaje);
        //        if (dt != null && dt.Rows.Count > 0)
        //        {
        //            GridFechas.DataSource = Session["GridFechas"] = dt;
        //            GridFechas.DataBind();
        //        }

        //        #endregion

        //        #region IMPUESTOS

        //        dt = new DataTable();
        //        dt = documentos.ConsultarImpuestos(v_pedimento, lblCadena.Text, ref mensaje);
        //        if (dt != null && dt.Rows.Count > 0)
        //        {
        //            GridImpuestos.DataSource = Session["GridImpuestos"] = dt;
        //            GridImpuestos.DataBind();
        //        }

        //        #endregion

        //        #region TRANSPORTE

        //        dt = new DataTable();
        //        dt = documentos.ConsultarTransporte(v_pedimento, lblCadena.Text, ref mensaje);
        //        if (dt != null && dt.Rows.Count > 0)
        //        {
        //            GridTransporte.DataSource = Session["GridTransporte"] = dt;
        //            GridTransporte.DataBind();
        //        }

        //        #endregion

        //        #region GUIAS

        //        dt = new DataTable();
        //        dt = documentos.ConsultarGuias(v_pedimento, lblCadena.Text, ref mensaje);
        //        if (dt != null && dt.Rows.Count > 0)
        //        {
        //            GridGuias.DataSource = Session["GridGuias"] = dt;
        //            GridGuias.DataBind();
        //        }

        //        #endregion

        //        #region CONTENEDORES

        //        dt = new DataTable();
        //        dt = documentos.ConsultarContenedores(v_pedimento, lblCadena.Text, ref mensaje);
        //        if (dt != null && dt.Rows.Count > 0)
        //        {
        //            GridContenedores.DataSource = Session["GridContenedores"] = dt;
        //            GridContenedores.DataBind();
        //        }

        //        #endregion

        //        #region OBSERVACIONES

        //        dt = new DataTable();
        //        dt = documentos.ConsultarObservaciones(v_pedimento, lblCadena.Text, ref mensaje);

        //        if (dt != null && dt.Rows.Count > 0)
        //        {
        //            ASPx_OBSERVACIONES.Text = dt.Rows[0]["OBSERVACIONES"].ToString().Trim() != string.Empty ? dt.Rows[0]["OBSERVACIONES"].ToString().Trim() : "---";
        //        }

        //        #endregion

        //        #region DESCARGOS

        //        dt = new DataTable();
        //        dt = documentos.ConsultarDescargos(v_pedimento, lblCadena.Text, ref mensaje);
        //        if (dt != null && dt.Rows.Count > 0)
        //        {
        //            GridDescargos.DataSource = Session["GridDescargos"] = dt;
        //            GridDescargos.DataBind();
        //        }

        //        #endregion

        //    }
        //}

        #region DOCUMENTOS

        protected void Grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            GridPageSize = int.Parse(e.Parameters);
            Grid.SettingsPager.PageSize = GridPageSize;
            Grid.DataBind();
        }

        protected void PagerCombo_Load(object sender, EventArgs e)
        {
            (sender as ASPxComboBox).Value = Grid.SettingsPager.PageSize;
        }

                
        //Evento se ejecuta al dar click en la fila para abrir un archivo PDF en detailGridDocumentos
        //Se cambio por el botón PDF
        protected void detailGridDocumentos_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        {
        //    try
        //    {
        //        string tipo, folio, descripcion, status = null;
        //        ASPxGridView detailGridDocumentos = sender as ASPxGridView;
        //        tipo = detailGridDocumentos.GetRowValues(e.VisibleIndex, "CLAVE").ToString();
        //        status = detailGridDocumentos.GetRowValues(e.VisibleIndex, "STATUS").ToString();

        //        //Cove
        //        descripcion = detailGridDocumentos.GetRowValues(e.VisibleIndex, "DESCRIPCION").ToString();
        //        //ED
        //        folio = detailGridDocumentos.GetRowValues(e.VisibleIndex, "COMPLEMENTO 1").ToString().Trim() == string.Empty ? null : detailGridDocumentos.GetRowValues(e.VisibleIndex, "COMPLEMENTO 1").ToString();

        //        if (status.Equals("OK"))
        //        {
        //            string rfc = string.Empty;
        //            string pedimento = string.Empty;

        //            rfc = Session["RFC"].ToString();

        //            if (tipo.Equals("COVE"))
        //            {
        //                pedimento = descripcion;
        //                tipo = "COVE_SICE";
        //            }
        //            if (tipo.Equals("DIGI"))
        //            {
        //                if (descripcion.IndexOf(".") > -1)
        //                    pedimento = descripcion.Substring(0, descripcion.IndexOf("."));
        //                else
        //                    pedimento = descripcion;
        //            }
        //            else if (tipo.Equals("ED"))
        //            {
        //                pedimento = folio;
        //            }


        //            TExpediente res;

        //            IExpedienteComercioservice v1 = new IExpedienteComercioservice();

        //            res = v1.GetPedimento(rfc, pedimento, tipo);
        //            //res = v1.GetPedimento("MPE8309215YA", "731-3897-8000002", "PSSICE");
        //            //res = v1.GetPedimento("MPE8309215YA", "0433180408YS2", "ED");
        //            string base64BinaryStr = res.Archivo.Trim();

        //            //Valida si viene vacio el archivo del wb 
        //            if (base64BinaryStr.Trim() == string.Empty)
        //            {
        //                AlertError("Error al abrir un " + Session["Clave"].ToString() + " intentelo más tarde");
        //                return;
        //            }

        //            base64BinaryStr = base64BinaryStr.Replace("*", "+").Replace("-", "/");

        //            if (string.IsNullOrEmpty(base64BinaryStr) || base64BinaryStr.Length % 4 != 0
        //            || base64BinaryStr.Contains(" ") || base64BinaryStr.Contains("\t") || base64BinaryStr.Contains("\r") || base64BinaryStr.Contains("\n"))
        //            { }
        //            else
        //            {
        //                byte[] sPDFDecoded = Convert.FromBase64String(base64BinaryStr);

        //                Response.Clear();
        //                MemoryStream ms = new MemoryStream(sPDFDecoded);
        //                Response.ContentType = "application/pdf";
        //                Response.AddHeader("content-disposition", "attachment;filename=" + pedimento + "_SICE.pdf");
        //                Response.Buffer = true;
        //                ms.WriteTo(Response.OutputStream);
        //                HttpContext.Current.ApplicationInstance.CompleteRequest();
        //                //Response.End();
        //            }
        //        }
        //        else
        //            AlertError("El status debe estar en OK para poder abrir el archivo");
        //    }

        //    catch (System.Threading.ThreadAbortException ex)
        //    {
        //        string mensaje = string.Empty;
        //        int idusuario = 0;
        //        if (Session["IdUsuario"] != null)
        //            idusuario = int.Parse(Session["IdUsuario"].ToString());
            //        excepcion.RegistrarExcepcion(idusuario, "Documentos-detailGridDocumentos_CustomButtonCallback", ex, lblCadena.Text, ref mensaje);
        //    }

        #region Anterior Base64Byte
        //    //BoDocumentos documentos = new BoDocumentos();            
        //    //byte[] Base64Byte;
        //    //string mensaje = "";
        //    //string valor = string.Empty;
            //    //valor = documentos.ConsultarDocumentosString(v_tipo, v_folio, v_id, lblCadena.Text, ref mensaje);
            //    ////Base64Byte = documentos.ConsultarDocumentosBynary(v_tipo, v_folio, v_id, lblCadena.Text, ref mensaje);

        //    //if (valor != string.Empty)
        //    //{
        //    //    try
        //    //    {
        //    //        valor = valor.Replace("\r\n","");
        //    //        Base64Byte = Convert.FromBase64String(valor);

        //    //        //1a forma (ver si salva el file en alguna parte)
        //    //        Response.ClearContent();
        //    //        Response.ContentType = "application/octetstream";
        //    //        Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", "test.pdf"));
        //    //        Response.AddHeader("Content-Length", Base64Byte.Length.ToString());
        //    //        Response.BinaryWrite(Base64Byte);
        //    //        Response.Flush();
        //    //        Response.Close();

        //    //        //2da forma (crea un archivo y el file en una ruta designada)
        //    //        FileStream obj = File.Create("C:\\FilePDF\\test.pdf");
        //    //        obj.Write(Base64Byte, 0, Base64Byte.Length);
        //    //        obj.Flush();
        //    //        obj.Close();
        //    //        System.Diagnostics.Process.Start("C:\\FilePDF\\test.pdf");

        //    //        OcultarModalED();
        //    //    }
        //    //    catch (Exception ex)
        //    //    {
        //    //        AlertError("El archivo no se pudo descargar");
        //    //    }
        //    //}
        //    //else
        //    //{
        //    //    AlertError("Por el momento no hay archivo pdf para este registro");
        //    //}
        #endregion

        #region pruebas anteriores

        //    ////if (valor != null && valor.Length > 0)
        //    ////{
        //    //    //string fileName = System.IO.Path.GetFileName(valor);
        //    //    //System.IO.FileInfo fileInfo = new System.IO.FileInfo(valor);

        //    //System.IO.FileStream stream =
        //    //new FileStream(@"C:\PROYECTOS\VISUAL STUDIO\1. Codigo\SICE_BS_WEB2\InactionWMS_Web_Indar\img\file.pdf", FileMode.CreateNew);
        //    //System.IO.BinaryWriter writer = new BinaryWriter(stream);
        //    //writer.Write(binaryData, 0, binaryData.Length);
        //    //writer.Close();

        //    //string name = "test.pdf";
        //    //Response.ClearContent();
        //    //Response.ContentType = @"application\octetstream";
        //    //Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", name));
        //    //Response.AddHeader("Content-Length", binaryData.Length.ToString());
        //    //Response.BinaryWrite(binaryData);
        //    //Response.Flush();
        //    //Response.Close();  


        //    //    //Response.AddHeader("Content-type", "application/pdf");
        //    //    //Response.AddHeader("Content-Length", binaryData.Length.ToString());
        //    //    //Response.AddHeader("Content-Disposition", "attachment; filename=test.pdf");
        //    //    //Response.OutputStream.Write(binaryData, 0, binaryData.Length);
        //    //    //Response.Flush();
        //    //    //Response.End();   

        //    //    //Crear un archivo pdf
        //    //    //FileStream fs = new FileStream(@"C:\PROYECTOS\VISUAL STUDIO\1. Codigo\SICE_BS_WEB2\InactionWMS_Web_Indar\img\filepdf.png", FileMode.CreateNew, FileAccess.Write);
        //    //    //fs.Write(binaryData, 0, binaryData.Length);
        //    //    //fs.Flush();
        //    //    //fs.Close();
                
        //    //    //Leer un archivo
        //    //    //FileStream fs2 = new FileStream(@"C:\PROYECTOS\VISUAL STUDIO\1. Codigo\SICE_BS_WEB2\InactionWMS_Web_Indar\img\filepdf.png", FileMode.Open, FileAccess.Read);
        //    //    //BinaryReader br = new BinaryReader(fs2);
        //    //    //Byte[] bytes = br.ReadBytes((Int32)fs2.Length);
        //    //    //br.Close();
        //    //    //fs2.Close();

        //    //    #region bueno
        //    //    string filename = @"C:\PROYECTOS\VISUAL STUDIO\1. Codigo\SICE_BS_WEB2\InactionWMS_Web_Indar\img\file.pdf";
                
        //    //    ////Abre el archivo en página web    
        //    //    //System.Diagnostics.Process.Start(filename);
                
        //    //    //Abre el archivo pdf en una aplicación para pdf
        //    //    System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo(filename);
        //    //    System.Diagnostics.Process.Start(startInfo);
        //    //    #endregion

        //    //    #region pruebas
        //    //    //string strPdf = System.Text.Encoding.ASCII.GetString(binaryData);
        //    //    //char[] chrPdf = strPdf.ToCharArray();
        //    //    //Response.Write(chrPdf, 0, chrPdf.Length);
        //    //    //strPdf = File.ReadAllText(strPdf);
        //    //    //Response.Write(strPdf.ToCharArray(), 0, strPdf.Length);

        //    //    //Response.ClearHeaders();
        //    //    //Response.Clear();
        //    //    //Response.AddHeader("Content-Type", "application/pdf");
        //    //    //Response.AddHeader("Content-Length", binaryData.Length.ToString());
        //    //    //Response.AddHeader("Content-Disposition", "inline; filename=Nombre.pdf");
        //    //    //Response.BinaryWrite(binaryData);
        //    //    //Response.Flush();
        //    //    //Response.End();

        //    //    //Response.ContentType = @"application\pdf";
        //    //    //Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName + ".pdf");
        //    //    //Response.WriteFile(valor);
        //    //    //Response.Flush();

        //    //    //Response.AddHeader("Content-Length", fileInfo.Length.ToString());
        //    //    //Response.End();
        //    //    #endregion

        //    ////}
        //    ////else
        //    ////    AlertError("No hay documentos a mostrar.");

        #endregion
        }

        
        #endregion

        #region PARTIDAS
      
        //Botón en columna de Pedimento, abre archivos
        protected void ASPxButtonDetailDoc_Click(object sender, EventArgs e)
        {
            try
            {
                ASPxButton btn = (ASPxButton)sender;

                btn.ImageUrl = "~/img/iconos/ico_doc1.png";

                GridViewDataItemTemplateContainer container = (GridViewDataItemTemplateContainer)btn.NamingContainer;
                object[] values = (object[])container.Grid.GetRowValues(container.VisibleIndex, new string[] { "RFC", "PEDIMENTOARMADO" });

                TExpediente res;

                string rfc = values[0].ToString();
                string pedimento = values[1].ToString();

                IExpedienteComercioservice v1 = new IExpedienteComercioservice();
                
                res = v1.GetPedimento(rfc, pedimento, "PSSICE");
                string base64BinaryStr = res.Archivo.Trim();
                Response.Write("<script>window.open('" + base64BinaryStr + "','_blank') </script>");
                HttpContext.Current.ApplicationInstance.CompleteRequest();

                //Va a ejecutar un botón que a su vez ejecuta un método, es decir vuelve a entrar al codebehind
                IrRefrescaGrid1y2();


               /*base64BinaryStr = base64BinaryStr.Replace("*", "+").Replace("-", "/");

                if (string.IsNullOrEmpty(base64BinaryStr) || base64BinaryStr.Length % 4 != 0
               || base64BinaryStr.Contains(" ") || base64BinaryStr.Contains("\t") || base64BinaryStr.Contains("\r") || base64BinaryStr.Contains("\n"))
                { }
                else
                {
                    byte[] sPDFDecoded = Convert.FromBase64String(base64BinaryStr);

                    Response.Clear();
                    MemoryStream ms = new MemoryStream(sPDFDecoded);
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=" + pedimento + "_SICE.pdf");
                    Response.Buffer = true;
                    ms.WriteTo(Response.OutputStream);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    //Response.Redirect(url, false);
                    //Response.End();
                }*/
            }
            catch (Exception ex)
            {
                IrRefrescaGrid1y2();
                string mensaje = string.Empty;
                int idusuario = 0;
                if (lblIdUsuario.Text.Length > 0)
                    idusuario = int.Parse(lblIdUsuario.Text);
                excepcion.RegistrarExcepcion(idusuario, "Documentos-ASPxButtonDetailDoc_Click", ex, lblCadena.Text, ref mensaje);
                AlertError(ex.Message);
            }
            
        }

        //Cuando el grid principal trae datos entra aquí para pintar imagen en botones de comluna Pedimento
        protected void ASPxButtonDetailDoc_Init(object sender, EventArgs e)
        {
            ASPxButton btn = (ASPxButton)sender;
            btn.ImageUrl = "~/img/iconos/ico_doc1.png";
        }

        //Cuando el grid principal trae datos entra aquí para pintar la imagen de los signos (+-) en los botones en columna de Partidas
        //Entra cada vez que se seleccione el botón en Partidas (+)(-), 
        protected void ASPxButtonDetail_Init(object sender, EventArgs e)
        {
            ASPxButton btn = (ASPxButton)sender;
            GridViewDataItemTemplateContainer templateContainer = (GridViewDataItemTemplateContainer)btn.NamingContainer;

            if (templateContainer.Grid.DetailRows.IsVisible(templateContainer.VisibleIndex))
            {
                btn.ClientSideEvents.Click = string.Format("function(s, e) {{ {0}.CollapseDetailRow({1}); }}", templateContainer.Grid.ClientInstanceName, templateContainer.VisibleIndex);          
                btn.ImageUrl = "~/img/iconos/sign_menos.png";
            }
            else
            {
                btn.ClientSideEvents.Click = string.Format("function(s, e) {{ {0}.ExpandDetailRow({1}); }}", templateContainer.Grid.ClientInstanceName, templateContainer.VisibleIndex);
                btn.ImageUrl = "~/img/iconos/sign_mas.png";
            }
        }        

        //Entra en este evento después de a ver entrado al evento ASPxButtonDetail_Init para llenar el grid de partidas
        protected void GridDePartidas_Init(object sender, EventArgs e)
        {
            ASPxGridView grd = (ASPxGridView)sender;
            Session["Pedimento"] = (sender as ASPxGridView).GetMasterRowKeyValue();

            BoDocumentos documentos = new BoDocumentos();
            DataTable dt = new DataTable();

            string mensaje = string.Empty;
            dt = documentos.ConsultarPartidas(Session["Pedimento"].ToString(), lblCadena.Text, ref mensaje);
            if (dt != null && dt.Rows.Count > 0)
            {
                grd.DataSource = Session["GridPartidas"] =  dt;
                grd.DataBind();
            }
        }

        //Evento que se desencadena en GridDePartidas al dar clic en Ver Detalle columna ACCIONES 
        protected void GridDePartidas_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            long v_partida = 0;
            ASPxGridView grdD = (ASPxGridView)sender;

            dynamic keyValue = grdD.GetRowValues(e.VisibleIndex, grdD.KeyFieldName);
            //dynamic keyValue = grdD.GetRowValues(e.VisibleIndex, "PARTIDA");
            v_partida = keyValue;            

            MostrarModal();

            //Titulo del Modal
            Modal1Titulo.InnerText = "Partida: " + v_partida.ToString();
            DataBind();

            //Se requiere cargar los datos en Grid2 que son las Fechas(Año-Mes)
            string mensaje = "";
            if (Grid2.VisibleRowCount == 0)
            {
                if (Session["FECHAS"] == null)
                    Session["FECHAS"] = docs.ConsultarFechasOperacion(lblCadena.Text, ref mensaje);

                CreaSlideBar();
            }

            BoDocumentos documentos = new BoDocumentos();
            DataTable dt = new DataTable();

            #region DETALLE
            dt = new DataTable();
            dt = documentos.ConsultarPartida_Detalle(Session["Pedimento"].ToString(), v_partida, lblCadena.Text, ref mensaje);
            if (dt != null && dt.Rows.Count > 0)
            {
                //VerticalGridPartidaDetalle.DataSource = dt;
                //VerticalGridPartidaDetalle.DataBind();

                lblP_PARTIDA_DETALLEKEY.Text = dt.Rows[0]["PARTIDA_DETALLEKEY"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLEKEY"].ToString().Trim() : "---";
                lblP_PEDIMENTO.Text = dt.Rows[0]["PEDIMENTOARMADO"].ToString().Trim() != string.Empty ? dt.Rows[0]["PEDIMENTOARMADO"].ToString().Trim() : "---";
                lblP_PARTIDA.Text = dt.Rows[0]["PARTIDA"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA"].ToString().Trim() : "---";
                lblP_NUMERO_PARTIDA.Text = dt.Rows[0]["PARTIDA_DETALLE_NUMEROPARTIDA"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_NUMEROPARTIDA"].ToString().Trim() : "---";
                lblP_FRACCION_ARANCELARIA.Text = dt.Rows[0]["PARTIDA_DETALLE_FRACCIONARANCELARIA"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_FRACCIONARANCELARIA"].ToString().Trim() : "---";
                lblP_SUBDIVISION_FRACCION.Text = dt.Rows[0]["PARTIDA_DETALLE_SUBDIVISIONFRACCION"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_SUBDIVISIONFRACCION"].ToString().Trim() : "---";
                lblP_DESCRIPCION_MERCANCIA.Text = dt.Rows[0]["PARTIDA_DETALLE_DESCRIPCIONMERCANCIA"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_DESCRIPCIONMERCANCIA"].ToString().Trim() : "---";
                lblP_UNIDAD_MEDIDA_TARIFA_CLAVE.Text = dt.Rows[0]["PARTIDA_DETALLE_UNIDADMEDIDATARIFA_CLAVE"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_UNIDADMEDIDATARIFA_CLAVE"].ToString().Trim() : "---";
                lblP_UNIDAD_MEDIDA_TARIFA_DESCRIPCION.Text = dt.Rows[0]["PARTIDA_DETALLE_UNIDADMEDIDATARIFA_DESCRIPCION"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_UNIDADMEDIDATARIFA_DESCRIPCION"].ToString().Trim() : "---";
                lblP_CANTIDAD_UNIDAD_MEDIDA_TARIFA.Text = dt.Rows[0]["PARTIDA_DETALLE_CANTIDADUNIDADMEDIDATARIFA"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_CANTIDADUNIDADMEDIDATARIFA"].ToString().Trim() : "---";
                lblP_UNIDAD_MEDIDA_COMERCIAL_CLAVE.Text = dt.Rows[0]["PARTIDA_DETALLE_UNIDADMEDIDACOMERCIA_CLAVE"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_UNIDADMEDIDACOMERCIA_CLAVE"].ToString().Trim() : "---";
                lblP_UNIDAD_MEDIDA_COMERCIAL_CLAVE_DESCRIPCION.Text = dt.Rows[0]["PARTIDA_DETALLE_UNIDADMEDIDACOMERCIAL_DESCRIPCION"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_UNIDADMEDIDACOMERCIAL_DESCRIPCION"].ToString().Trim() : "---";
                lblP_CANTIDAD_UNIDAD_MEDIDA_COMERCIAL.Text = dt.Rows[0]["PARTIDA_DETALLE_CANTIDADUNIDADMEDIDACOMERCIAL"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_CANTIDADUNIDADMEDIDACOMERCIAL"].ToString().Trim() : "---";
                lblP_PRECIO_UNITARIO.Text = dt.Rows[0]["PARTIDA_DETALLE_PRECIOUNITARIO"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_PRECIOUNITARIO"].ToString().Trim() : "---";
                lblP_VALOR_COMERCIAL.Text = dt.Rows[0]["PARTIDA_DETALLE_VALORCOMERCIAL"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_VALORCOMERCIAL"].ToString().Trim() : "---";
                lblP_VALOR_ADUANA.Text = dt.Rows[0]["PARTIDA_DETALLE_VALORADUANA"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_VALORADUANA"].ToString().Trim() : "---";
                lblP_VALOR_DOLARES.Text = dt.Rows[0]["PARTIDA_DETALLE_VALORDOLARES"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_VALORDOLARES"].ToString().Trim() : "---";
                lblP_VALOR_AGREGADO.Text = dt.Rows[0]["PARTIDA_DETALLE_VALORAGREGADO"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_VALORAGREGADO"].ToString().Trim() : "---";
                lblP_CODIGO_PRODUCTO.Text = dt.Rows[0]["PARTIDA_DETALLE_CODIPRODUCTO"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_CODIPRODUCTO"].ToString().Trim() : "---";
                lblP_MARCA.Text = dt.Rows[0]["PARTIDA_DETALLE_MARCA"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_MARCA"].ToString().Trim() : "---";
                lblP_MODELO.Text = dt.Rows[0]["PARTIDA_DETALLE_MODELO"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_MODELO"].ToString().Trim() : "---";
                lblP_METODO_VALORACION.Text = dt.Rows[0]["PARTIDA_DETALLE_METODOVALORACION"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_METODOVALORACION"].ToString().Trim() : "---";
                lblP_VINCULACION.Text = dt.Rows[0]["PARTIDA_DETALLE_VINCULACION"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_VINCULACION"].ToString().Trim() : "---";
                lblP_PAIS_ORIGEN_DESTINO_CLAVE.Text = dt.Rows[0]["PARTIDA_DETALLE_PAISORIGENDESTINO_CLAVE"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_PAISORIGENDESTINO_CLAVE"].ToString().Trim() : "---";
                lblP_PAIS_ORIGEN_DESTINO_NOMBRE.Text = dt.Rows[0]["PARTIDA_DETALLE_PAISORIGENDESTINO_DESCRIPCION"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_PAISORIGENDESTINO_DESCRIPCION"].ToString().Trim() : "---";
                lblP_PAIS_VENDEDOR_COMPRADOR_CLAVE.Text = dt.Rows[0]["PARTIDA_DETALLE_PAISVENDEDORCOMPRADOR_CLAVE"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_PAISVENDEDORCOMPRADOR_CLAVE"].ToString().Trim() : "---";
                lblP_PAIS_VENDEDOR_COMPRADOR_NOMBRE.Text = dt.Rows[0]["PARTIDA_DETALLE_PAISVENDEDORCOMPRADOR_DESCRIPCION"].ToString().Trim() != string.Empty ? dt.Rows[0]["PARTIDA_DETALLE_PAISVENDEDORCOMPRADOR_DESCRIPCION"].ToString().Trim() : "---";


            }

            #endregion
            
            #region OBSERVACIONES
                
            dt = new DataTable();
            dt = documentos.ConsultarPartida_Observaciones(Session["Pedimento"].ToString(), v_partida, lblCadena.Text, ref mensaje);
            if (dt != null && dt.Rows.Count > 0)
            {
                ASPx_PartidaObservaciones.Text = dt.Rows[0]["OBSERVACIONES"].ToString().Trim() != string.Empty ? dt.Rows[0]["OBSERVACIONES"].ToString().Trim() : "---";

            }
            #endregion

            #region IMPUESTOS GRAVAMEN E IMPUESTOS TASAS

            //GRAVAMEN
            dt = new DataTable();
            dt = documentos.ConsultarPartida_Gravamen(Session["Pedimento"].ToString(), v_partida, lblCadena.Text, ref mensaje);
            if (dt != null && dt.Rows.Count > 0)
            {
                GridPartidaGravamen.DataSource = Session["GridPartidaGravamen"] = dt;
                GridPartidaGravamen.DataBind();
            }

            //TASAS
            dt = new DataTable();
            dt = documentos.ConsultarPartida_Tasas(Session["Pedimento"].ToString(), v_partida, lblCadena.Text, ref mensaje);
            if (dt != null && dt.Rows.Count > 0)
            {
                GridPartidaTasas.DataSource = Session["GridPartidaTasas"] = dt;
                GridPartidaTasas.DataBind();
            }
            #endregion

            #region IDENTIFICADORES

            dt = new DataTable();
            dt = documentos.ConsultarPartida_Identificadores(Session["Pedimento"].ToString(), v_partida, lblCadena.Text, ref mensaje);
            if (dt != null && dt.Rows.Count > 0)
            {
                GridPartidaIdentificadores.DataSource = Session["GridPartidaIdentificadores"] = dt;
                GridPartidaIdentificadores.DataBind();
            }

            #endregion

            #region REGULACIONES

            dt = new DataTable();
            dt = documentos.ConsultarPartida_Regulaciones(Session["Pedimento"].ToString(), v_partida, lblCadena.Text, ref mensaje);
            if (dt != null && dt.Rows.Count > 0)
            {
                GridPartidaRegulaciones.DataSource = Session["GridPartidaRegulaciones"] = dt;
                GridPartidaRegulaciones.DataBind();
            }

            #endregion

            #region XML

            dt = new DataTable();
            dt = documentos.ConsultarPartida_XML(Session["Pedimento"].ToString(), v_partida, lblCadena.Text, ref mensaje);
            if (dt != null && dt.Rows.Count > 0)
            {
                string cadena = dt.Rows[0]["XML"].ToString().Trim() != string.Empty ? dt.Rows[0]["XML"].ToString().Trim() : "---";
                if (!cadena.Equals("---"))
                {
                    string formattedXml = XElement.Parse(cadena).ToString().Replace("<", "< ").Replace(">", " >");
                    MemoPartidaXML.Text = formattedXml;
                }
                else
                    MemoPartidaXML.Text = cadena;
            }
            #endregion

            #region CÓDIGOS

            dt = new DataTable();
            dt = documentos.ConsultarPartida_Codigos(Session["Pedimento"].ToString(), v_partida, lblCadena.Text, ref mensaje);
            if (dt != null && dt.Rows.Count > 0)
            {
                GridPartidaCodigos.DataSource = Session["GridPartidaCodigos"] = dt;
                GridPartidaCodigos.DataBind();
            }

            #endregion

        }    

        protected void GridPartidasDetail_Init(object sender, EventArgs e)
        {
            try
            {
                ASPxGridView grdD = (ASPxGridView)sender;
                GridViewDetailRowTemplateContainer RowTemplateContaine = (GridViewDetailRowTemplateContainer)grdD.NamingContainer;

                int count = 0;
                foreach (DataRow fila in ((DataTable)(Session["GridPartidas"])).Rows)
                {
                    if (count.Equals(RowTemplateContaine.VisibleIndex))
                    {
                        Session["Partidas"] = fila["PARTIDA"].ToString();
                        break;
                    }

                    count = count + 1;
                }

                BoDocumentos documentos = new BoDocumentos();
                DataTable dt = new DataTable();

                string mensaje = string.Empty;
                dt = documentos.ConsultarPartida_IdentificadoresPartida(Session["Pedimento"].ToString(), int.Parse(Session["Partidas"].ToString()), lblCadena.Text, ref mensaje);
                if (dt != null && dt.Rows.Count > 0)
                {
                    grdD.DataSource = dt;
                    grdD.DataBind();
                }
            }
            catch(Exception ex)
            {                
                string mensaje = string.Empty;
                int idusuario = 0;
                if (lblIdUsuario.Text.Length > 0)
                    idusuario = int.Parse(lblIdUsuario.Text);
                excepcion.RegistrarExcepcion(idusuario, "Documentos-GridPartidasDetail_Init", ex, lblCadena.Text, ref mensaje);
            }
        }

        #endregion

        #region Mostrar y Ocultar Modals

        private void MostrarModal()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "MostrarModal", "<script> document.getElementById('btnPartidasM').click(); </script> ", false);
        }

        private void MostrarModalCG()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "MostrarModalCG", "<script> document.getElementById('btnModalCG').click(); </script> ", false);
        }

        private void MostrarModalED()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "MostrarModalED", "<script> document.getElementById('btnModalED').click(); </script> ", false);
            
            //Se ejecuta en un script para colocar un valor en el cookie "CookieValor"
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "CookieValor", "<script> document.cookie='CookieValor=no_guarda;'; </script> ", false);
        }

        private void OcultarModalED()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "OcultarModalED", "<script> $('#ModalED').modal('hide');</script>", false);
        }

        private void MostrarModalEncabezado()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "MostrarModalEncabezado", "<script> document.getElementById('btnModalEncabezado').click(); </script> ", false);
        }

        private void MostrarModalCU()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "MostrarModalCU", "<script> document.getElementById('btnModalCU').click(); </script> ", false);
        }

        private void MostrarModalCE()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "MostrarModalCE", "<script> document.getElementById('btnModalCE').click(); </script> ", false);
        }

        private void MostrarModalRE()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "MostrarModalRE", "<script> document.getElementById('btnModalRE').click(); </script> ", false);
        }

        private void IrRefrescaGrid1y2()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "IrRefrescaGrid1y2", "<script> document.getElementById('btnRefrescaGrid1y2').click(); </script> ", false);
        }
        
        #endregion

        #region COMPRADORES/VENDEDORES

        //protected void Grid_CustomCallbackCV(object sender, ASPxGridViewCustomCallbackEventArgs e)
        //{
        //    GridPageSizeCV = int.Parse(e.Parameters);
        //    GridCV.SettingsPager.PageSize = GridPageSizeCV;
        //    GridCV.DataBind();
        //}

        //protected void PagerCombo_LoadCV(object sender, EventArgs e)
        //{
        //    (sender as ASPxComboBox).Value = GridCV.SettingsPager.PageSize;
        //}

        //protected void GridCV_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        //{

        //}

        #endregion

        #region IDENTIFICADORES GENERALES

        //protected void Grid_CustomCallbackIG(object sender, ASPxGridViewCustomCallbackEventArgs e)
        //{
        //    GridPageSizeIG = int.Parse(e.Parameters);
        //    GridIG.SettingsPager.PageSize = GridPageSizeIG;
        //    GridIG.DataBind();
        //}

        //protected void PagerCombo_LoadIG(object sender, EventArgs e)
        //{
        //    (sender as ASPxComboBox).Value = GridIG.SettingsPager.PageSize;
        //}

        //protected void GridIG_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        //{

        //}

        #endregion

        #region FACTURAS

        protected void Grid_CustomCallbackFACTURAS(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            GridPageSizeFacturas = int.Parse(e.Parameters);
            GridFacturas.SettingsPager.PageSize = GridPageSizeFacturas;
            GridFacturas.DataBind();
        }

        protected void PagerCombo_LoadFACTURAS(object sender, EventArgs e)
        {
            (sender as ASPxComboBox).Value = GridFacturas.SettingsPager.PageSize;
        }


        #endregion

        #region FECHAS

        protected void Grid_CustomCallbackFECHAS(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            GridPageSizeFechas = int.Parse(e.Parameters);
            GridFechas.SettingsPager.PageSize = GridPageSizeFechas;
            GridFechas.DataBind();
        }

        protected void PagerCombo_LoadFechas(object sender, EventArgs e)
        {
            (sender as ASPxComboBox).Value = GridFechas.SettingsPager.PageSize;
        }

        protected void GridFechas_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        {

        }

        #endregion

        #region CONTRIBUCIONES GENERALES

        protected void Grid_CustomCallbackCONTRIGRALES(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            GridPageSizeContriGrales = int.Parse(e.Parameters);
            GridContriGrales.SettingsPager.PageSize = GridPageSizeContriGrales;
            GridContriGrales.DataBind();
        }

        protected void PagerCombo_LoadCONTRIGRALES(object sender, EventArgs e)
        {
            (sender as ASPxComboBox).Value = GridContriGrales.SettingsPager.PageSize;
        }

        protected void GridContriGrales_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        {

        }

        #endregion

        #region IMPUESTOS

        protected void Grid_CustomCallbackIMPUESTOS(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            GridPageSizeImpuestos = int.Parse(e.Parameters);
            GridImpuestos.SettingsPager.PageSize = GridPageSizeImpuestos;
            GridImpuestos.DataBind();
        }

        protected void PagerCombo_LoadIMPUESTOS(object sender, EventArgs e)
        {
            (sender as ASPxComboBox).Value = GridImpuestos.SettingsPager.PageSize;
        }

        protected void GridImpuestos_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        {

        }

        #endregion

        #region TRANSPORTE

        protected void Grid_CustomCallbackTRANSPORTE(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            GridPageSizeTransporte = int.Parse(e.Parameters);
            GridTransporte.SettingsPager.PageSize = GridPageSizeTransporte;
            GridTransporte.DataBind();
        }

        protected void PagerCombo_LoadTRANSPORTE(object sender, EventArgs e)
        {
            (sender as ASPxComboBox).Value = GridTransporte.SettingsPager.PageSize;
        }

        protected void GridTransporte_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        {

        }

        #endregion

        #region GUIAS

        protected void Grid_CustomCallbackGUIAS(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            GridPageSizeGuias = int.Parse(e.Parameters);
            GridGuias.SettingsPager.PageSize = GridPageSizeGuias;
            GridGuias.DataBind();
        }

        protected void PagerCombo_LoadGUIAS(object sender, EventArgs e)
        {
            (sender as ASPxComboBox).Value = GridGuias.SettingsPager.PageSize;
        }

        protected void GridGuias_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        {

        }

        #endregion

        #region CONTENEDORES

        protected void Grid_CustomCallbackCONTENEDORES(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            GridPageSizeContenedores = int.Parse(e.Parameters);
            GridContenedores.SettingsPager.PageSize = GridPageSizeContenedores;
            GridContenedores.DataBind();
        }

        protected void PagerCombo_LoadCONTENEDORES(object sender, EventArgs e)
        {
            (sender as ASPxComboBox).Value = GridContenedores.SettingsPager.PageSize;
        }

        protected void GridContenedores_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        {

        }

        #endregion

        #region DESCARGOS

        protected void Grid_CustomCallbackDESCARGOS(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            GridPageSizeDescargos = int.Parse(e.Parameters);
            GridDescargos.SettingsPager.PageSize = GridPageSizeDescargos;
            GridDescargos.DataBind();
        }

        protected void PagerCombo_LoadDESCARGOS(object sender, EventArgs e)
        {
            (sender as ASPxComboBox).Value = GridDescargos.SettingsPager.PageSize;
        }

        protected void GridDescargos_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        {

        }

        #endregion

        #region RECTIFICACIONES

        protected void Grid_CustomCallbackRECTIFICACIONES(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            GridPageSizeRectificaciones = int.Parse(e.Parameters);
            GridRectificaciones.SettingsPager.PageSize = GridPageSizeRectificaciones;
            GridRectificaciones.DataBind();
        }

        #endregion

        #region DOCUMENTOS O EXPEDIENTE DIGITAL

        protected void Grid_CustomCallbackDocumentos(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            GridPageSizeDocumentos = int.Parse(e.Parameters);
            detailGridDocumentos.SettingsPager.PageSize = GridPageSizeDocumentos;
            detailGridDocumentos.DataBind();
        }

        #endregion

        #region CUENTA DE GASTOS

        protected void Grid_CustomCallbackCG(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            GridPageSizeCuentaGastos = int.Parse(e.Parameters);
            detailGridCG.SettingsPager.PageSize = GridPageSizeCuentaGastos;
            detailGridCG.DataBind();
        }

        #endregion

        #region EXPORTAR EXCEL
        protected void lkb_Excel_Click(object sender, EventArgs e)
        {
            string valor = ((System.Web.UI.Control)(sender)).ID;
            switch (valor)
            {
                case "lkb_Excel":
                    if (Grid.VisibleRowCount > 0)
                    {
                        Exporter.WriteXlsToResponse("Documentos", new XlsExportOptionsEx() { SheetName = "Documentos" });
                    }
                    else
                        AlertError("No hay información por exportar");
                    break;         
                case "lkb_Excel_FA":
                    if (GridFacturas.VisibleRowCount > 0)
                    {
                        //ExporterFA.WriteXlsToResponse("Facturas", new XlsExportOptionsEx() { SheetName = "Facturas" });
                    }
                    else
                        AlertError("No hay información por exportar");
                    break;
                case "lkb_Excel_Fechas":
                    if (GridFechas.VisibleRowCount > 0)
                    {
                        //ExporterFecha.WriteXlsToResponse("Fechas", new XlsExportOptionsEx() { SheetName = "Fechas" });
                    }
                    else
                        AlertError("No hay información por exportar");
                    break;
                case "lkb_Excel_ContriGrales":
                    if (GridContriGrales.VisibleRowCount > 0)
                    {
                        //ExporterContriGrales.WriteXlsToResponse("Contribuciones Generales", new XlsExportOptionsEx() { SheetName = "Contribuciones Generales" });
                    }
                    else
                        AlertError("No hay información por exportar");
                    break;
                case "lkb_Excel_Impuestos":
                    if (GridImpuestos.VisibleRowCount > 0)
                    {
                        //ExporterImpuestos.WriteXlsToResponse("Impuestos", new XlsExportOptionsEx() { SheetName = "Impuestos" });
                    }
                    else
                        AlertError("No hay información por exportar");
                    break;
                case "lkb_Excel_Transporte":
                    if (GridTransporte.VisibleRowCount > 0)
                    {
                        //ExporterTransporte.WriteXlsToResponse("Transporte", new XlsExportOptionsEx() { SheetName = "Transporte" });
                    }
                    else
                        AlertError("No hay información por exportar");
                    break;
                case "lkb_Excel_Guias":
                    if (GridGuias.VisibleRowCount > 0)
                    {
                        //ExporterGuias.WriteXlsToResponse("Guías", new XlsExportOptionsEx() { SheetName = "Guías" });
                    }
                    else
                        AlertError("No hay información por exportar");
                    break;
                case "lkb_Excel_Contenedores":
                    if (GridContenedores.VisibleRowCount > 0)
                    {
                        //ExporterContenedores.WriteXlsToResponse("Contenedores", new XlsExportOptionsEx() { SheetName = "Contenedores" });
                    }
                    else
                        AlertError("No hay información por exportar");
                    break;
                case "lkb_Excel_Descargos":
                    if (GridDescargos.VisibleRowCount > 0)
                    {
                        //ExporterDescargos.WriteXlsToResponse("Descargos", new XlsExportOptionsEx() { SheetName = "Descargos" });
                    }
                    else
                        AlertError("No hay información por exportar");
                    break;
            }

            //Actualiza los permisos
            PermisosUsuario();
            
        }

        #endregion

        //Método que refresca los datos del grid
        protected void lkb_Actualizar_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < Grid.VisibleRowCount; i++)
            {
                Grid.DataSource = Session["GridD"];
                Grid.DataBind();                
                //Grid.Settings.VerticalScrollableHeight = 260;
                //Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                //Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Hidden;
                Grid.SettingsPager.PageSize = 20;
            }

            //Actualiza los permisos 
            PermisosUsuario();
        }

        //Método que refresca los datos del grid2
        protected void ActualizarGrid2()
        {
            string mensaje = string.Empty;

            //Se requiere cargar los datos en Grid2 que son las Fechas(Año-Mes)
            if (Grid2.VisibleRowCount == 0)
            {
                if (Session["FECHAS"] == null)
                    Session["FECHAS"] = docs.ConsultarFechasOperacion(lblCadena.Text, ref mensaje);
            }

            CreaSlideBar();
        }

        //Método que filtra los datos del grid
        protected void lkb_Filtrar_Click(object sender, EventArgs e)
        {
            foreach (GridViewColumn column in Grid.Columns)
            {   
                if (((DevExpress.Web.GridViewDataColumn)(column)).FieldName == "PEDIMENTOARMADO")
                {
                    ((GridViewDataColumn)column).Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                    Grid.SettingsFilterControl.ViewMode = FilterControlViewMode.VisualAndText;
                    //  (column, Session["filterValue"].ToString());
                }
                
                //valor = ((GridViewDataColumn)column).FilterExpression.ToString();
                //Grid.AutoFilterByColumn(column, valor);
            }
           
            //GridViewDataColumn col2 = Grid.Columns["PEDIMENTOARMADO"] as GridViewDataColumn;
            //GridViewDataColumn col5 = Grid.Columns["TIPO PEDIMENTO"] as GridViewDataColumn;
            //string filter2 = col2.FilterExpression;
            //string filterValue = string.Empty;
            //CriteriaOperator criteria = CriteriaOperator.Parse(filter2);
            //// You can also check for other criteria operator types
            //if (criteria is BinaryOperator)
            //    filterValue = ((criteria as BinaryOperator).RightOperand as OperandValue).Value.ToString();
            //Grid.DataBind();

            //Actualiza los permisos 
            PermisosUsuario();
            
        }

        protected void Grid_ProcessColumnAutoFilter(object sender, ASPxGridViewAutoFilterEventArgs e)
        {
            Session["filterValue"] = e.Value;
        }

        //Método que limpia los filtros del grid
        protected void lkb_LimpiarFiltros_Click(object sender, EventArgs e)
        {
            foreach (GridViewColumn column in Grid.Columns)
            {
                if (column is GridViewDataColumn)
                {
                    ((GridViewDataColumn)column).Settings.AutoFilterCondition = AutoFilterCondition.Default;
                    Grid.AutoFilterByColumn(column,"");
                    Grid.FilterExpression = String.Empty;
                    Grid.ClearSort();
                }
            }            

            foreach (GridViewColumn column in GridFacturas.Columns)
            {
                if (column is GridViewDataColumn)
                {
                    ((GridViewDataColumn)column).Settings.AutoFilterCondition = AutoFilterCondition.Default;
                    GridFacturas.AutoFilterByColumn(column, "");
                    GridFacturas.FilterExpression = String.Empty;
                    GridFacturas.ClearSort();
                }
            }

            foreach (GridViewColumn column in GridFechas.Columns)
            {
                if (column is GridViewDataColumn)
                {
                    ((GridViewDataColumn)column).Settings.AutoFilterCondition = AutoFilterCondition.Default;
                    GridFechas.AutoFilterByColumn(column, "");
                    GridFechas.FilterExpression = String.Empty;
                    GridFechas.ClearSort();
                }
            }

            foreach (GridViewColumn column in GridImpuestos.Columns)
            {
                if (column is GridViewDataColumn)
                {
                    ((GridViewDataColumn)column).Settings.AutoFilterCondition = AutoFilterCondition.Default;
                    GridImpuestos.AutoFilterByColumn(column, "");
                    GridImpuestos.FilterExpression = String.Empty;
                    GridImpuestos.ClearSort();
                }
            }

            foreach (GridViewColumn column in GridTransporte.Columns)
            {
                if (column is GridViewDataColumn)
                {
                    ((GridViewDataColumn)column).Settings.AutoFilterCondition = AutoFilterCondition.Default;
                    GridTransporte.AutoFilterByColumn(column, "");
                    GridTransporte.FilterExpression = String.Empty;
                    GridTransporte.ClearSort();
                }
            }

            foreach (GridViewColumn column in GridGuias.Columns)
            {
                if (column is GridViewDataColumn)
                {
                    ((GridViewDataColumn)column).Settings.AutoFilterCondition = AutoFilterCondition.Default;
                    GridGuias.AutoFilterByColumn(column, "");
                    GridGuias.FilterExpression = String.Empty;
                    GridGuias.ClearSort();
                }
            }

            foreach (GridViewColumn column in GridContenedores.Columns)
            {
                if (column is GridViewDataColumn)
                {
                    ((GridViewDataColumn)column).Settings.AutoFilterCondition = AutoFilterCondition.Default;
                    GridContenedores.AutoFilterByColumn(column, "");
                    GridContenedores.FilterExpression = String.Empty;
                    GridContenedores.ClearSort();
                }
            }

            foreach (GridViewColumn column in GridDescargos.Columns)
            {
                if (column is GridViewDataColumn)
                {
                    ((GridViewDataColumn)column).Settings.AutoFilterCondition = AutoFilterCondition.Default;
                    GridDescargos.AutoFilterByColumn(column, "");
                    GridDescargos.FilterExpression = String.Empty;
                    GridDescargos.ClearSort();
                }
            }
        }

        //Metodo que muestra ventana de alerta
        public void AlertError(string mensaje)
        {
            pModalError.InnerText = mensaje;
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "", "<script> document.getElementById('btnError').setAttribute('data-whatever', '" + mensaje + "'); document.getElementById('btnError').click(); </script> ", false);
        }

        //Metodo que muestra ventana de satisfactorio
        public void AlertSuccess(string mensaje)
        {
            pModalSucces.InnerText = mensaje;
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "", "<script> document.getElementById('btnSuccess').setAttribute('data-whatever', '" + mensaje + "'); document.getElementById('btnSuccess').click(); </script> ", false);
        }

        //Metodo que muestra ventana de Question
        public void AlertQuestion(string mensaje)
        {
            pModalQuestion.InnerText = mensaje;
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "", "<script> document.getElementById('btnQuestion').setAttribute('data-whatever', '" + mensaje + "'); document.getElementById('btnQuestion').click(); </script> ", false);
        }

        //Metodo que muestra ventana de QuestionR
        public void AlertQuestionR(string mensaje)
        {
            pModalQuestionR.InnerText = mensaje;
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "", "<script> document.getElementById('btnQuestionR').setAttribute('data-whatever', '" + mensaje + "'); document.getElementById('btnQuestionR').click(); </script> ", false);
        }

        //Metodo que muestra ventana de QuestionED
        public void AlertQuestionED(string mensaje, string mensaje2)
        {
            pModalQuestionED.InnerText = mensaje;
            pModalQuestionED2.InnerText = mensaje2;
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "", "<script> document.getElementById('btnQuestionED').setAttribute('data-whatever', '" + mensaje + "'); document.getElementById('btnQuestionED').click(); </script> ", false);
        }

        //Metodo que muestra ventana de QuestionCU
        public void AlertQuestionCU(string mensaje)
        {
            pModalQuestionCU.InnerText = mensaje;
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "", "<script> document.getElementById('btnQuestionCU').setAttribute('data-whatever', '" + mensaje + "'); document.getElementById('btnQuestionCU').click(); </script> ", false);
        }

        //Metodo que muestra ventana de QuestionAU
        public void AlertQuestionAU(string mensaje)
        {
            pModalQuestionAU.InnerText = mensaje;
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "", "<script> document.getElementById('btnQuestionAU').setAttribute('data-whatever', '" + mensaje + "'); document.getElementById('btnQuestionAU').click(); </script> ", false);
        }

        //Metodo que muestra ventana de QuestionRE
        public void AlertQuestionRE(string mensaje)
        {
            pModalQuestionRE.InnerText = mensaje;
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "", "<script> document.getElementById('btnQuestionRE').setAttribute('data-whatever', '" + mensaje + "'); document.getElementById('btnQuestionRE').click(); </script> ", false);
        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 0;
            
            string mensaje2 = "";            
            if (Grid2.VisibleRowCount == 0)
            {
                if (Session["FECHAS"] == null)
                    Session["FECHAS"] = docs.ConsultarFechasOperacion(lblCadena.Text, ref mensaje2);

                CreaSlideBar();
            }
        }

        //Botón btnPDF, se usa para descargar los Coves y los ED´s
        protected void btnPDF_Click(object sender, EventArgs e)
        {
            try
            {
                string rfc = null;
                string pedimento = null;
                string tipo = null;
                string status = null;
                string complemento = null;
                string descripcion = null;
                string clave = null;
                string vFilekey = null;
                int valida = 0;

                //Recorre detailGridDocumentos
                for (int i = 0; i < detailGridDocumentos.VisibleRowCount; i++)
                {
                    ASPxCheckBox chkConsultar = detailGridDocumentos.FindRowCellTemplateControl(i, (GridViewDataColumn)detailGridDocumentos.Columns["PDF"], "chkConsultar") as ASPxCheckBox;

                    //if (detailGridDocumentos.Selection.IsRowSelected(i))
                    if (chkConsultar.Checked)
                    {
                        valida = 1;
                        var rowValues = detailGridDocumentos.GetRowValues(i, new string[] { "FILEKEY", "CLAVE", "DESCRIPCION", "COMPLEMENTO 1", "STATUS" }) as object[];
                        
                        vFilekey = rowValues[0].ToString();
                        Session["Clave"] = clave = rowValues[1].ToString();
                        descripcion = rowValues[2].ToString();
                        complemento = rowValues[3].ToString();
                        status = rowValues[4].ToString();
                        //clave = detailGridDocumentos.GetSelectedFieldValues("CLAVE")[0].ToString().Trim();
                        //descripcion = detailGridDocumentos.GetSelectedFieldValues("DESCRIPCION")[0].ToString().Trim();
                        //complemento = detailGridDocumentos.GetSelectedFieldValues("COMPLEMENTO 1")[0].ToString().Trim();
                        //status = detailGridDocumentos.GetSelectedFieldValues("STATUS")[0].ToString().Trim(); 
                        break;
                    }
                }

                //Validaciones
                if (valida.Equals(0))
                {
                    MantenerModalED();
                    AlertError("Debe seleccionar un registro para abrir el archivo pdf");
                    return;
                }

                if (status == null || !status.Equals("OK"))
                {
                    MantenerModalED();
                    AlertError("Para abrir el archivo el status debe ser OK");
                    return;
                }

                if (Session["RFC"].ToString() == null)
                    return;

                rfc = Session["RFC"].ToString();


                if (clave.Equals("COVE"))
                {
                    pedimento = descripcion;
                    tipo = "COVE_SICE";
                }
                if (clave.Equals("DIGI"))
                {
                    //Valida si en la columna de descripción, esta el nombre del archivo sino está en la columna de complemento
                    if (descripcion.IndexOf(".") != -1)
                        pedimento = descripcion;
                    else
                        pedimento = complemento;

                    tipo = clave;
                }
                else if (clave.Equals("ED"))
                {
                    pedimento = complemento;
                    tipo = clave;
                    descripcion = descripcion + ".pdf";
                }

                try
                {
                    if (tipo.Contains("DIGI") || tipo.Contains("ED"))
                    {
                        Guid Filekey = Guid.Parse(vFilekey);
                        string mensaje = string.Empty;
                        DataTable dt = new DataTable();
                        Byte[] vByte;
                        vByte = docs.Obtener_Archivo_Digi(Filekey, tipo, lblCadena.Text, ref mensaje);

                        //Response.Write("<script>window.open('" + vByte + "','_blank') </script>");
                        //HttpContext.Current.ApplicationInstance.CompleteRequest();
                        Response.Clear();
                        MemoryStream ms = new MemoryStream(vByte);
                        //Response.ContentType = "application/zip";
                        Response.AddHeader("content-disposition", "attachment;filename=" + descripcion);
                        Response.Buffer = true;
                        ms.WriteTo(Response.OutputStream);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                        Response.End();
                    }
                    else
                    {
                        TExpediente res;
                        IExpedienteComercioservice v1 = new IExpedienteComercioservice();

                        res = v1.GetPedimento(rfc, pedimento, tipo);
                        string base64BinaryStr = res.Archivo.Trim();

                        //Valida si viene vacio el archivo del wb 
                        if (base64BinaryStr.Trim() == string.Empty)
                        {
                            MantenerModalED();
                            AlertError("Error al abrir un " + Session["Clave"].ToString() + " intentelo más tarde");
                            return;
                        }

                        Response.Write("<script>window.open('" + base64BinaryStr + "','_blank') </script>");
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }

                    MostrarModalED();




                    //Si existe entonces lo abre
                   /* base64BinaryStr = base64BinaryStr.Replace("*", "+").Replace("-", "/");

                    if (string.IsNullOrEmpty(base64BinaryStr) || base64BinaryStr.Length % 4 != 0
                        || base64BinaryStr.Contains(" ") || base64BinaryStr.Contains("\t") || base64BinaryStr.Contains("\r") || base64BinaryStr.Contains("\n"))
                    { }
                    else
                    {                        
                        byte[] sPDFDecoded = Convert.FromBase64String(base64BinaryStr);

                        Response.Clear();
                        MemoryStream ms = new MemoryStream(sPDFDecoded);

                        if (clave.Equals("COVE") || clave.Equals("ED"))
                        {
                            Response.ContentType = "application/pdf";
                            Response.AddHeader("content-disposition", "attachment;filename=" + pedimento + "_SICE.pdf");
                        }
                        else
                        {
                            string extension = pedimento.Trim().Substring(pedimento.LastIndexOf("."), (pedimento.Length - pedimento.LastIndexOf(".")));
                            Response.ContentType = "application/" + extension;
                            Response.AddHeader("content-disposition", "attachment;filename=" + pedimento);
                        }

                        Response.Buffer = true;
                        ms.WriteTo(Response.OutputStream);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                        //Response.End();
                    }*/
                }
                catch (Exception exc) 
                { 
                    Session["exc"] = exc.Message;

                    int idusuario = 0;
                    string mensaje = "";
                    if (lblIdUsuario.Text.Length > 0)
                        idusuario = int.Parse(lblIdUsuario.Text);
                    excepcion.RegistrarExcepcion(idusuario, "Documentos-btnPDF_Click", exc, lblCadena.Text, ref mensaje);
                    MantenerModalED();
                    AlertError("Error al abrir un " + Session["Clave"].ToString() + " intentelo más tarde");
                }
                finally
                {
                    string mensaje = string.Empty;

                    //Se requiere cargar los datos en Grid2 que son las Fechas(Año-Mes)
                    if (Grid2.VisibleRowCount == 0)
                    {
                        if (Session["FECHAS"] == null)
                            Session["FECHAS"] = docs.ConsultarFechasOperacion(lblCadena.Text, ref mensaje);

                        CreaSlideBar();
                    }
                }
                
            }
            catch (Exception ex)
            {
                    int idusuario = 0;
                    string mensaje = "";
                    if (lblIdUsuario.Text.Length > 0)
                        idusuario = int.Parse(lblIdUsuario.Text);
                    excepcion.RegistrarExcepcion(idusuario, "Documentos-btnPDF_Click", ex, lblCadena.Text, ref mensaje);

                    MantenerModalED();

                    AlertError("Error al abrir un " + Session["Clave"].ToString() + " intentelo más tarde");
            }
            finally
            {
                string mensaje = string.Empty;

                //Se requiere cargar los datos en Grid2 que son las Fechas(Año-Mes)
                if (Grid2.VisibleRowCount == 0)
                {
                    if (Session["FECHAS"] == null)
                        Session["FECHAS"] = docs.ConsultarFechasOperacion(lblCadena.Text, ref mensaje);

                    CreaSlideBar();
                }
            }
        }

        //Botón btnZip, se usa para descargar todos los archivos del pedimento
        protected void btnZip_Click(object sender, EventArgs e)
        {
           try
           {
               string rfc = null;
               string pedimento = null;

               rfc = Session["RFC"].ToString().Trim();
               pedimento = Session["PEDIMENTOARMADO"].ToString().Trim();

               TExpediente res;
               IExpedienteComercioservice v1 = new IExpedienteComercioservice();

               res = v1.Getzip(rfc, pedimento);
               string base64BinaryStr = res.Archivo.Trim();
               
               Response.Write("<script>window.open('" + base64BinaryStr + "','_blank') </script>");
               HttpContext.Current.ApplicationInstance.CompleteRequest();
               //Response.End();

               MantenerModalED();
           }
           catch (Exception ex)
           {
               int idusuario = 0;
               string mensaje = "";
               if (lblIdUsuario.Text.Length > 0)
                   idusuario = int.Parse(lblIdUsuario.Text);
               excepcion.RegistrarExcepcion(idusuario, "Documentos-DescargarZip", ex, lblCadena.Text, ref mensaje);

               MantenerModalED();

               AlertError("Error al descargar el archivo zipeado, intentelo más tarde");
           }
        }

        protected void MantenerModalED()
        {
            BoDocumentos documentos = new BoDocumentos();
            DataTable dt = new DataTable();
            string mensaje = "";

            MostrarModalED();

            //Titulo del Modal
            ModalEDTitulo.InnerText = "Pedimento: " + Session["PEDIMENTOARMADO"].ToString();
            DataBind();

            //Trae información al grid de docuemntos
            mensaje = "";
            dt = documentos.ConsultarExpedienteDigital(Session["PEDIMENTOARMADO"].ToString(), int.Parse(lblIdUsuario.Text), lblCadena.Text, ref mensaje);
            detailGridDocumentos.DataSource = Session["GridDocumentos"] = dt;
            detailGridDocumentos.DataBind();
            detailGridDocumentos.Settings.VerticalScrollableHeight = 300;
            detailGridDocumentos.Settings.VerticalScrollBarMode = ScrollBarMode.Auto;

            //Para cada columna en el detailGridDocumentos se define modo de filtro CheckedList 
            if (dt.Rows.Count > 0)
            {
                foreach (GridViewDataColumn column in detailGridDocumentos.Columns)
                {
                    column.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                    if (column.ToString().Contains("ID"))
                        break;
                }

                for (int i = 0; i < detailGridDocumentos.VisibleRowCount; i++)
                {
                    ASPxCheckBox chkConsultar = detailGridDocumentos.FindRowCellTemplateControl(i, (GridViewDataColumn)detailGridDocumentos.Columns["PDF"], "chkConsultar") as ASPxCheckBox;

                    if (!detailGridDocumentos.GetRowValues(i, "STATUS").ToString().Equals("OK"))
                        chkConsultar.Visible = false;
                    else
                        chkConsultar.Visible = true;

                }
            }


            //Se requiere cargar los datos en Grid2 que son las Fechas(Año-Mes)
            if (Grid2.VisibleRowCount == 0)
            {
                if (Session["FECHAS"] == null)
                    Session["FECHAS"] = docs.ConsultarFechasOperacion(lblCadena.Text, ref mensaje);

                CreaSlideBar();
            }
        }

        //Este método lo llama el checkbox "chkConsultar" del grid "detailGridDocumentos" y este método llama a una función "chkConsultarClick" 
        //para hacer que se seleccione solo un checkbox en el grid.
        protected void chkConsultar_Init(object sender, EventArgs e)
        {
            try
            {
                ASPxCheckBox chb = sender as ASPxCheckBox;
                GridViewDataItemTemplateContainer container = chb.NamingContainer as GridViewDataItemTemplateContainer;

                chb.ClientInstanceName = String.Format("chkConsultar{0}", container.VisibleIndex);
                chb.ClientSideEvents.CheckedChanged = String.Format("function (s, e) {{ chkConsultarClick(s, e, {0}); }}", container.VisibleIndex);
            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (lblIdUsuario.Text.Length > 0)
                    idusuario = int.Parse(lblIdUsuario.Text);
                excepcion.RegistrarExcepcion(idusuario, "Documentos-chkConsultar_Init", ex, lblCadena.Text, ref mensaje);
            }
        }

        //Botón Borrar archivo por renglon en detailGridDocumentos(borrar archivo mientras coincida el nombre del usuario logeado con el que guardo)
        protected void btnBorrarFile_Click(object sender, EventArgs e)
        {
            try
            {
                ASPxButton btn = (ASPxButton)sender;

                GridViewDataItemTemplateContainer container = (GridViewDataItemTemplateContainer)btn.NamingContainer;
                string vFILEKEY = detailGridDocumentos.GetRowValues(container.VisibleIndex, "FILEKEY").ToString();
                string vUSERNAME = detailGridDocumentos.GetRowValues(container.VisibleIndex, "USERNAME").ToString();
                string vDESCRIPCION = detailGridDocumentos.GetRowValues(container.VisibleIndex, "DESCRIPCION").ToString();

                string valida = "¿Desea borrar el registro";
                string valida2 = vDESCRIPCION + "?";
                AlertQuestionED(valida, valida2);

                Session["vFILEKEY"] = vFILEKEY;

                MostrarModalED();
            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (lblIdUsuario.Text.Length > 0)
                    idusuario = int.Parse(lblIdUsuario.Text);
                excepcion.RegistrarExcepcion(idusuario, "Documentos-btnBorrarFile_Click", ex, lblCadena.Text, ref mensaje);
            }            
        }

        protected void btnOkED_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["vFILEKEY"] == null)
                {
                    string alerta = "<script>alert('La sesión ha expirado, debe volver a iniciar sesión');window.location.href='Login.aspx'</script>";
                    Session["Tab"] = "Salir";
                    Response.Write(alerta);
                    return;
                }
                
                Guid guidkey = Guid.Parse(Session["vFILEKEY"].ToString());

                string mensaje = string.Empty;
                DataTable dt = new DataTable();
                dt = docs.BorrarFileExpedienteDigital(guidkey, int.Parse(lblIdUsuario.Text), lblCadena.Text, ref mensaje);

                MantenerModalED();
            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (lblIdUsuario.Text.Length > 0)
                    idusuario = int.Parse(lblIdUsuario.Text);
                //excepcion.RegistrarExcepcion(idusuario, "Documentos-btnOkED_Click", ex, Session["Cadena"].ToString(), ref mensaje);
            }
        }

        //Se selecciona un valor del combobox de Tipo de Archivos
        protected void cbxTipoArchivo_SelectedIndexChanged(object sender, EventArgs e)
        {
            MostrarModalED();

            string mensaje = "";

            if (cbxTipoArchivo.SelectedIndex != -1)
            {
                Session["cbxTipoArchivo"] = cbxTipoArchivo.SelectedItem.Text.ToString();
                Session["cbxSufijoArchivo"] = cbxTipoArchivo.SelectedItem.Value.ToString();
                if (Session["cbxSufijoArchivo"].ToString().IndexOf("*") != -1)
                    Session["cbxSufijoArchivo"] = Session["cbxSufijoArchivo"].ToString().Replace("*", "");
            }
            

            //Se requiere cargar los datos en Grid2 que son las Fechas(Año-Mes)
            if (Grid2.VisibleRowCount == 0)
            {
                if (Session["FECHAS"] == null)
                    Session["FECHAS"] = docs.ConsultarFechasOperacion(lblCadena.Text, ref mensaje);

                CreaSlideBar();
            }

        }

        //Este evento se genera cuando se da clic en botón subir archivo en el FileUpload
        protected void upc1_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            try
            {
                BoDocumentos documentos = new BoDocumentos();

                string fileNameComplete = e.UploadedFile.FileName;
                string name = System.IO.Path.GetFileNameWithoutExtension(fileNameComplete);
                string extension = System.IO.Path.GetExtension(fileNameComplete);
                Session["extensionFile"] = extension;                                
                Session["filebyte"] = e.UploadedFile.FileBytes;

                //byte[] filebyte = e.UploadedFile.FileBytes;
                //string cadenaFile = Encoding.ASCII.GetString(filebyte);

                //Se limpia la sesion file y se le agrega nueva información en un datatable
                Session["Files"] = null;
                DataTable dt = new DataTable();
                dt.Columns.Add("nombre_completo", typeof(string));
                dt.Columns.Add("nombre", typeof(string));
                dt.Columns.Add("file", typeof(byte[]));
                DataRow row;
                row = dt.NewRow();
                row["nombre_completo"] = fileNameComplete;
                row["nombre"] = name;
                row["file"] = e.UploadedFile.FileBytes;
                dt.Rows.Add(row);
                Session["Files"] = dt;


                ////Si existe uno o más archivos se guardarán en una variable de Session tipo DataTable                                
                //if(Session["Files"]!=null && ((DataTable)Session["Files"]).Rows.Count > 0)
                //{
                //    DataRow row;
                //    row = ((DataTable)Session["Files"]).NewRow();
                //    row["nombre_completo"] = fileNameComplete;
                //    row["nombre"] = name;
                //    row["file"] = e.UploadedFile.FileBytes;
                //    ((DataTable)Session["Files"]).Rows.Add(row);
                //}
                //else
                //{
                //    DataTable dt = new DataTable();
                //    dt.Columns.Add("nombre_completo", typeof(string));
                //    dt.Columns.Add("nombre", typeof(string));
                //    dt.Columns.Add("file", typeof(byte[]));
                //    DataRow row;
                //    row = dt.NewRow();
                //    row["nombre_completo"] = fileNameComplete;
                //    row["nombre"] = name;
                //    row["file"] = e.UploadedFile.FileBytes;
                //    dt.Rows.Add(row);
                //    Session["Files"] = dt;
                //}

                //Se agrega el nombre del archivo en el evento para agregarlo por javascript al control lblTitRespuestaFileUpload
                e.CallbackData = fileNameComplete;
                

                //Por si necesito guardarlo en una ruta temporal
                //using (var stream = new FileStream(varFilePath, FileMode.Open, FileAccess.Read))
                //{
                //    using (var reader = new BinaryReader(stream))
                //    {
                //        filebyte = reader.ReadBytes((int)stream.Length);
                //    }
                //}
            }
            catch (Exception ex)
            {
                int idusuario = 0;
                string mensaje = "";
                if (lblIdUsuario.Text.Length > 0)
                    idusuario = int.Parse(lblIdUsuario.Text);
                excepcion.RegistrarExcepcion(idusuario, "Documentos-upc1_FileUploadComplete", ex, lblCadena.Text, ref mensaje);
            }
        }
        
        //Evento que guardará en BD los archivos seleccionados con su Tipo de Archivo, Pedimento
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                //Valida si se usa sessiones si no tienen información volver a tenerlas
                if (Session["PEDIMENTOARMADO"] == null)
                {
                    Response.Redirect("Login.aspx");
                    return;
                }

                if (Session["cbxSufijoArchivo"] == null || Session["extensionFile"] == null)
                {
                    MostrarModalED();
                    AlertError("Debe agregar un archivo para poder guardar");
                    return;
                }                

                BoDocumentos documentos = new BoDocumentos();
                string valor = string.Empty;
                string nombre = string.Empty;
                string nombre_completo = string.Empty;
                string name_sufijo = Session["PEDIMENTOARMADO"].ToString() + Session["cbxSufijoArchivo"].ToString() + Session["extensionFile"].ToString();
                MostrarModalED();

                //Titulo del Modal
                ModalEDTitulo.InnerText = "Pedimento: " + Session["PEDIMENTOARMADO"].ToString();
                DataBind();

                //Se puede también de está forma
                //HttpCookie httpCookieValor = Request.Cookies["CookieValor"];
                //string cookie = httpCookieValor.Value;
                string cookieValor = Request.Cookies["CookieValor"].Value;

                //if (Session["Files"] != null && ((DataTable)Session["Files"]).Rows.Count > 0)
                if (!cookieValor.Equals("no_guarda") && Session["Files"] != null)
                {
                    foreach (DataRow fila in ((DataTable)(Session["Files"])).Rows)
                    {
                        string mensaje = "";
                        nombre_completo = fila["nombre_completo"].ToString();
                        nombre = fila["nombre"].ToString();
                        byte[] bytes = (byte[])fila["file"];

                        //Se puede usar para el archivo
                        //(byte[])Session["filebyte"] o filebyte

                        valor = documentos.GuardarArchivo_ExpedienteDigital(bytes, Session["PEDIMENTOARMADO"].ToString(), Session["cbxTipoArchivo"].ToString(), name_sufijo, int.Parse(lblIdUsuario.Text), lblCadena.Text, ref mensaje);

                        if (!valor.Equals("Ok"))
                            break;
                    }

                    //Valida resultado
                    if (valor.Equals("Ok"))
                    {
                        if (((DataTable)Session["Files"]).Rows.Count == 1)
                            AlertSuccess("Archivo guardado con éxito");
                        //lblTitRespuestaFileUpload.Text = "Archivo guardado con éxito";
                        else if (((DataTable)Session["Files"]).Rows.Count > 1)
                            AlertSuccess("Archivos guardados con éxito");
                        //lblTitRespuestaFileUpload.Text = "Archivos guardados con éxito";

                        //Session["FileUploadForeColor"] = "#1ee008";
                    }
                    else
                    {
                        AlertError("No se pudo guardar el archivo " + nombre_completo + ", intentelo de nuevo");
                        //lblTitRespuestaFileUpload.Text = "No se pudo guardar el archivo " + nombre + ", intentelo de nuevo";
                        //Session["FileUploadForeColor"] = "#ff0000";
                    }
                    //lblTitRespuestaFileUpload.ForeColor = System.Drawing.ColorTranslator.FromHtml(Session["FileUploadForeColor"].ToString());
                }
                else
                {
                    AlertError("No hay archivos por guardar");
                    //lblTitRespuestaFileUpload.Text = "No hay archivos por guardar";
                    //Session["FileUploadForeColor"] = "#ff0000";
                    //lblTitRespuestaFileUpload.ForeColor = System.Drawing.ColorTranslator.FromHtml(Session["FileUploadForeColor"].ToString());

                }

                MantenerED();
            }
            catch (Exception ex)
            {
                int idusuario = 0;
                string mensaje = "";
                if (lblIdUsuario.Text.Length > 0)
                    idusuario = int.Parse(lblIdUsuario.Text);
                excepcion.RegistrarExcepcion(idusuario, "Documentos-btnGuardar_Click", ex, lblCadena.Text, ref mensaje);
            }
        }

        protected void MantenerED()
        {
            BoDocumentos documentos = new BoDocumentos();

            //Se obtiene los nombres de los css del botón btnZip si no tiene el nombre "disabled" se le agrega
            string cl = btnZip.CssClass;
            if (!cl.Contains("disabled"))
            {
                cl = cl + " " + "disabled";
                btnZip.CssClass = cl;
            }


            //Se actualiza información del grid de docuemntos
            string mensaje2 = "";
            DataTable dt = new DataTable();
            dt = documentos.ConsultarExpedienteDigital(Session["PEDIMENTOARMADO"].ToString(), int.Parse(lblIdUsuario.Text), lblCadena.Text, ref mensaje2);
            detailGridDocumentos.DataSource = Session["GridDocumentos"] = dt;
            detailGridDocumentos.DataBind();
            detailGridDocumentos.Settings.VerticalScrollableHeight = 300;
            detailGridDocumentos.Settings.VerticalScrollBarMode = ScrollBarMode.Auto;


            //Para cada renglon validar si se hace visible el checkbox y si existe algún checkbox habilitar el botón de descargar en zip
            for (int i = 0; i < detailGridDocumentos.VisibleRowCount; i++)
            {
                ASPxCheckBox chkConsultar = detailGridDocumentos.FindRowCellTemplateControl(i, (GridViewDataColumn)detailGridDocumentos.Columns["PDF"], "chkConsultar") as ASPxCheckBox;

                if (chkConsultar != null)
                {
                    if (!detailGridDocumentos.GetRowValues(i, "STATUS").ToString().Equals("OK"))
                        chkConsultar.Visible = false;
                    else
                    {
                        chkConsultar.Visible = true;

                        string cls = btnZip.CssClass;
                        string newCls = cls.Replace("disabled", "");
                        btnZip.CssClass = newCls;
                    }
                }
            }

            //Se requiere cargar los datos en Grid2 que son las Fechas(Año-Mes)
            if (Grid2.VisibleRowCount == 0)
            {
                string mensaje = "";
                if (Session["FECHAS"] == null)
                    Session["FECHAS"] = documentos.ConsultarFechasOperacion(lblCadena.Text, ref mensaje);

                CreaSlideBar();
            }

        }

        //***************
        //NÚMERO DE PARTE
        //***************


        //Metodo que llama al Callback para actualizar el PageSize y el GridNP
        protected void GridNP_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            GridPageSizeNP = int.Parse(e.Parameters);
            GridNP.SettingsPager.PageSize = GridPageSizeNP;
            GridNP.DataBind();
        }


        //Metodo que llama al Callback para actualizar el PageSize y el GridNP2
        protected void GridNP2_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            GridPageSizeNP2 = int.Parse(e.Parameters);
            GridNP2.SettingsPager.PageSize = GridPageSizeNP2;
            GridNP2.DataBind();
        }

        //Botón Regresar
        protected void lkb_Regresar_Click(object sender, EventArgs e)
        {
            Session["Secuencia"] = null;
            MultiView1.ActiveViewIndex = 0;
            return;
        }


        //Método que limpia los filtros del gridnp
        protected void lkb_LimpiarFiltrosNP_Click(object sender, EventArgs e)
        {
            foreach (GridViewColumn column in GridNP.Columns)
            {
                if (column is GridViewDataColumn)
                {
                    ((GridViewDataColumn)column).Settings.AutoFilterCondition = AutoFilterCondition.Default;
                    GridNP.AutoFilterByColumn(column, "");
                    GridNP.FilterExpression = String.Empty;
                    GridNP.ClearSort();
                }
            }
        }

        //Botón Validar
        protected void lkb_Validar_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["Cadena"] == null || Session["PEDIMENTOARMADO"] == null)
                {
                    string alerta = "<script>alert('La sesión ha expirado, debe volver a iniciar sesión');window.location.href='Login.aspx'</script>";
                    Session["Tab"] = "Salir";
                    Response.Write(alerta);
                    return;
                }

                string mensaje = "";
                DataTable dt = new DataTable();
                dt = np.Validar_NP(Session["PEDIMENTOARMADO"].ToString(), Session["Cadena"].ToString(), ref mensaje);


                mensaje = "";
                dt = new DataTable();
                dt = np.Errores_Detalle_NP(Session["PEDIMENTOARMADO"].ToString(), Session["Cadena"].ToString(), ref mensaje);

                if (dt != null && dt.Rows.Count > 0)
                {
                    //if (dt.Rows.Count == 1)
                    //    AlertError("Existe un error al validar el pedimento: " + Session["PEDIMENTOARMADO"].ToString());
                    //else if (dt.Rows.Count > 0)
                    //    AlertError("Existen errores al validar el pedimento: " + Session["PEDIMENTOARMADO"].ToString());

                    ASPxPageControl1.ActiveTabIndex = 1;
                }
                else
                {
                    ASPxPageControl1.ActiveTabIndex = 0;
                    //AlertSuccess("Validación exitosa");
                }

                mensaje = string.Empty;
                GridNP3.DataSource = Session["GridNP3"] = dt;
                GridNP3.DataBind();
                GridNP3.Settings.VerticalScrollableHeight = 200;
                GridNP3.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
            }
            catch (Exception ex)
            {
                AlertError(ex.Message);
            }
        }

        //Botón Migrar A24
        protected void lkb_Migrar_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["Cadena"] == null || Session["PEDIMENTOARMADO"] == null || Session["TIPO"] == null)
                {
                    string alerta = "<script>alert('La sesión ha expirado, debe volver a iniciar sesión');window.location.href='Login.aspx'</script>";
                    Session["Tab"] = "Salir";
                    Response.Write(alerta);
                    return;
                }
                
                string v_tipo = string.Empty;
                if (Session["TIPO"].ToString().Contains("IMPORTACION"))
                    v_tipo = "1";
                else
                    v_tipo = "2";


                string mensaje = "";
                DataTable dt = new DataTable();
                dt = np.MigrarA24(Session["PEDIMENTOARMADO"].ToString(), xcbDescarga.Text, v_tipo, Session["Cadena"].ToString(), ref mensaje);

                if(dt != null && dt.Rows[0][0].ToString() == "1")
                {
                    AlertError("El pedimento ya esta en la base de datos");
                }
                else if (dt != null && dt.Rows[0][0].ToString() == "2")
                {
                    AlertError("El pedimento tiene errores");

                    mensaje = "";
                    dt = new DataTable();
                    dt = np.Errores_Detalle_NP(Session["PEDIMENTOARMADO"].ToString(), Session["Cadena"].ToString(), ref mensaje);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        ASPxPageControl1.ActiveTabIndex = 1;
                    }
                    else
                    {
                        ASPxPageControl1.ActiveTabIndex = 0;
                    }

                    mensaje = string.Empty;
                    GridNP3.DataSource = Session["GridNP3"] = dt;
                    GridNP3.DataBind();
                    GridNP3.Settings.VerticalScrollableHeight = 200;
                    GridNP3.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;

                }

            }
            catch (Exception ex)
            {
                AlertError(ex.Message);
            }
        }


        #region RadioButton

        //Metodo del radiobuton para seleccionar por fila en GridNP
        protected void rbConsultar_Init(object sender, EventArgs e)
        {
            try
            {
                ASPxRadioButton rb = sender as ASPxRadioButton;
                GridViewDataItemTemplateContainer container = rb.NamingContainer as GridViewDataItemTemplateContainer;

                rb.ClientInstanceName = String.Format("rbConsultar{0}", container.VisibleIndex);
                rb.ClientSideEvents.CheckedChanged = String.Format("function (s, e) {{ rbConsultarClick(s, e, {0}); }}", container.VisibleIndex);
                Session["GridNP_IndexChecked"] = container.VisibleIndex;

                var lblPedimento = (ASPxLabel)GridNP.Toolbars[0].Items.FindByName("Links").FindControl("lblPedimento");
                lblPedimento.Text = Session["PEDIMENTOARMADO"].ToString();
            }
            catch (Exception ex)
            {
                AlertError(ex.Message);
            }
        }


        //Evento del radio button en GridNP
        protected void rbConsultar_CheckedChanged(object sender, EventArgs e)
        {
            try
            {

                if (Session["Cadena"] == null || Session["PEDIMENTOARMADO"] == null)
                {
                    string alerta = "<script>alert('La sesión ha expirado, debe volver a iniciar sesión');window.location.href='Login.aspx'</script>";
                    Session["Tab"] = "Salir";
                    Response.Write(alerta);
                    return;
                }

                ASPxRadioButton rbBox = sender as ASPxRadioButton;

                if (rbBox.Checked)
                {
                    //Valor del campo ITEM_KEY del Grid
                    string key = ((GridViewDataItemTemplateContainer)rbBox.NamingContainer).KeyValue.ToString();

                    //Obtener la secuencia seleccioanda
                    string v_secuencia = string.Empty;


                    foreach (DataRow fila in ((DataTable)Session["GridNP"]).Rows)
                    {
                        if (fila["PARTIDA_DETALLEKEY"].ToString().Trim() == key)
                        {
                            Session["Secuencia"] = v_secuencia = fila["Partida"].ToString();
                            break;
                        }
                    }


                    //Obtener datos en Grid2
                    string mensaje = string.Empty;
                    DataTable dt = new DataTable();

                    //Grid Descarga
                    dt = np.Consultar_NP_Detalle(v_secuencia, Session["PEDIMENTOARMADO"].ToString(), Session["Cadena"].ToString(), ref mensaje);

                    GridNP2.DataSource = Session["GridNP2"] = dt;
                    GridNP2.DataBind();
                    GridNP2.Settings.VerticalScrollableHeight = 200;
                    GridNP2.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;

                    //Selecccionar el primer registro del gridnp2
                    if (Session["GridNP2"] != null)
                        GridNP2.Selection.SelectRow(0);

                    var lblPedimento = (ASPxLabel)GridNP.Toolbars[0].Items.FindByName("Links").FindControl("lblPedimento");
                    lblPedimento.Text = Session["PEDIMENTOARMADO"].ToString();

                    ////Habilitar los botones de agregar, editar y borrar
                    //LinkButton lkb_Agregar = GridNP2.Toolbars.FindByName("Toolbar1").Items.FindByName("Links").FindControl("bbAgregar") as LinkButton;
                    //BootstrapButton bbEditar = GridNP2.Toolbars.FindByName("Toolbar1").Items.FindByName("Links").FindControl("bbEditar") as BootstrapButton;
                    //BootstrapButton bbBorrar = GridNP2.Toolbars.FindByName("Toolbar1").Items.FindByName("Links").FindControl("bbBorrar") as BootstrapButton;

                    //lkb_Agregar.Enabled = true;
                    //bbEditar.Enabled = true;
                    //bbBorrar.Enabled = true;
                }

            }
            catch (Exception ex)
            {
                AlertError(ex.Message);
            }
        }

        #endregion


        //Metodo que muestra el modal
        private void MostrarModalNP()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "MostrarModalNP", "<script> document.getElementById('btnModal').click(); </script> ", false);
        }


        //Agregar en Grid2
        protected void lkb_Agregar_Click(object sender, EventArgs e)
        {
            //Valida
            if (Session["Secuencia"] == null)
            {
                AlertError("Debe seleccionar una secuencia");
                return;
            }

            //Abre Modal
            MostrarModalNP();

            //Titulo del Modal
            ModalTitulo.InnerText = "Agregar";

            //Limpiar variables
            txt_cove.Text = string.Empty;
            dateEdit_Fecha.Text = string.Empty;
            txt_Factura.Text = string.Empty;
            txt_NumParte.Text = string.Empty;
            se_Cantidad.Text = "0";
            txt_UMC.Text = string.Empty;
            se_ValorDolares.Text = "0";
            txt_Descripcion.Text = string.Empty;
            txt_ClienteProv.Text = string.Empty;
            txt_PO.Text = string.Empty;


        }


        //Editar en GridNP2
        protected void lkb_Editar_Click(object sender, EventArgs e)
        {
            //Valida
            if (Session["Secuencia"] == null)
            {
                AlertError("Debe seleccionar una secuencia");
                return;
            }

            string mensaje = string.Empty;
            DataTable dt = new DataTable();
            int valida_select = 0;

            for (int i = 0; i < GridNP2.VisibleRowCount; i++)
            {
                if (GridNP2.Selection.IsRowSelected(i))
                {
                    //Abre Modal
                    MostrarModalNP();

                    //Titulo del Modal
                    ModalTitulo.InnerText = "Editar";

                    //Asignar valores
                    Session["key"] = GridNP2.GetSelectedFieldValues("DETALLENPKEY")[0].ToString().Trim();
                    txt_cove.Text = GridNP2.GetSelectedFieldValues("COVE")[0].ToString().Trim();
                    txt_Factura.Text = GridNP2.GetSelectedFieldValues("FACTURA")[0].ToString().Trim();
                    string date = GridNP2.GetSelectedFieldValues("FECHA_FACTURA")[0].ToString().Trim();
                    if (date == null || date.Trim() == string.Empty)
                        dateEdit_Fecha = null;
                    else
                        dateEdit_Fecha.Date = DateTime.Parse(date);
                    txt_NumParte.Text = GridNP2.GetSelectedFieldValues("CODIGO_PRODUCTO")[0].ToString().Trim();
                    se_Cantidad.Text = GridNP2.GetSelectedFieldValues("CANTIDAD")[0].ToString().Trim();
                    txt_UMC.Text = GridNP2.GetSelectedFieldValues("UNIDAD_MEDIDA")[0].ToString().Trim();
                    txt_Descripcion.Text = GridNP2.GetSelectedFieldValues("DESCRIPCION_COMERCIAL")[0].ToString().Trim();
                    se_ValorDolares.Text = GridNP2.GetSelectedFieldValues("PRECIO_TOTAL")[0].ToString().Trim();
                    txt_ClienteProv.Text = GridNP2.GetSelectedFieldValues("CLIENTE_PROVEEDOR")[0].ToString().Trim();
                    txt_PO.Text = GridNP2.GetSelectedFieldValues("PO")[0].ToString().Trim();
                    valida_select = 1;
                }
            }

            if (valida_select == 0)
            {
                AlertError("Debe seleccionar un registro para poder editar");
            }

            var lblPedimento = (ASPxLabel)GridNP.Toolbars[0].Items.FindByName("Links").FindControl("lblPedimento");
            lblPedimento.Text = Session["PEDIMENTOARMADO"].ToString();
        }


        //Eliminar en GridNP2
        protected void lkb_Borrar_OnClick(object sender, EventArgs e)
        {
            //Valida
            if (Session["Secuencia"] == null)
            {
                AlertError("Debe seleccionar una secuencia");
                return;
            }

            bool bandera = false;
            for (int i = 0; i < GridNP2.VisibleRowCount; i++)
            {
                if (GridNP2.Selection.IsRowSelected(i))
                {
                    bandera = true;
                    break;
                }
            }

            if (bandera)
            {
                //Modal Question
                string valida = "¿Desea eliminar este registro?";
                AlertQuestion(valida);
            }
            else
                AlertError("Debe seleccionar un registro para poderlo borrar");
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["Cadena"] == null || Session["PEDIMENTOARMADO"] == null || Session["Secuencia"] == null)
                {
                    string alerta = "<script>alert('La sesión ha expirado, debe volver a iniciar sesión');window.location.href='Login.aspx'</script>";
                    Session["Tab"] = "Salir";
                    Response.Write(alerta);
                    return;
                }

                int valida_select = 0;

                for (int i = 0; i < GridNP2.VisibleRowCount; i++)
                {
                    if (GridNP2.Selection.IsRowSelected(i))
                    {
                        string mensaje = string.Empty;
                        DataTable dt = new DataTable();
                        Int64 key = Int64.Parse(GridNP2.GetSelectedFieldValues("DETALLENPKEY")[0].ToString().Trim());

                        dt = np.Eliminar_Detalle_NP(key, Session["Secuencia"].ToString(), Session["PEDIMENTOARMADO"].ToString(), Session["Cadena"].ToString(), ref mensaje);

                        GridNP2.DataSource = Session["GridNP2"] = dt;
                        GridNP2.DataBind();
                        GridNP2.Settings.VerticalScrollableHeight = 200;
                        GridNP2.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;

                        //Selecccionar el primer registro del grid2
                        if (Session["GridNP2"] != null)
                            GridNP2.Selection.SelectRow(0);

                        AlertSuccess("El registro se eliminó con éxito.");
                        valida_select = 1;

                    }
                }

                if (valida_select == 0)
                {
                    AlertError("Debe seleccionar un registro para poder eliminar");
                }

                var lblPedimento = (ASPxLabel)GridNP.Toolbars[0].Items.FindByName("Links").FindControl("lblPedimento");
                lblPedimento.Text = Session["PEDIMENTOARMADO"].ToString();

            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (lblIdUsuario.Text.Length > 0)
                    idusuario = int.Parse(lblIdUsuario.Text);
                //excepcion.RegistrarExcepcion(idusuario, "Documentos-btnOk_Click", ex, Session["Cadena"].ToString(), ref mensaje);
            }
        }

        //Eliminar en GridNP2 todos
        protected void lkb_BorrarTodos_Click(object sender, EventArgs e)
        {
            //Valida
            if (Session["Secuencia"] == null)
            {
                AlertError("Debe seleccionar una secuencia");
                return;
            }

            //Modal Question
            string valida = "¿Desea eliminar todos los registros?";
            AlertQuestionR(valida);  
        }

        protected void btnOkR_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    if (Session["Cadena"] == null || Session["PEDIMENTOARMADO"] == null || Session["Secuencia"] == null)
            //    {
            //        string alerta = "<script>alert('La sesión ha expirado, debe volver a iniciar sesión');window.location.href='Login.aspx'</script>";
            //        Session["Tab"] = "Salir";
            //        Response.Write(alerta);
            //        return;
            //    }

            //    int valida_select = 0;

            //    for (int i = 0; i < GridNP2.VisibleRowCount; i++)
            //    {
            //        if (GridNP2.Selection.IsRowSelected(i))
            //        {
            //            string mensaje = string.Empty;
            //            DataTable dt = new DataTable();
            //            Int64 key = Int64.Parse(GridNP2.GetSelectedFieldValues("DETALLENPKEY")[0].ToString().Trim());

            //            dt = np.Eliminar_Detalle_NP(key, Session["Secuencia"].ToString(), Session["PEDIMENTOARMADO"].ToString(), Session["Cadena"].ToString(), ref mensaje);

            //            GridNP2.DataSource = Session["GridNP2"] = dt;
            //            GridNP2.DataBind();
            //            GridNP2.Settings.VerticalScrollableHeight = 200;
            //            GridNP2.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;

            //            //Selecccionar el primer registro del grid2
            //            if (Session["GridNP2"] != null)
            //                GridNP2.Selection.SelectRow(0);

            //            AlertSuccess("El registro se eliminó con éxito.");
            //            valida_select = 1;

            //        }
            //    }

            //    if (valida_select == 0)
            //    {
            //        AlertError("Debe seleccionar un registro para poder eliminar");
            //    }

            //    var lblPedimento = (ASPxLabel)GridNP.Toolbars[0].Items.FindByName("Links").FindControl("lblPedimento");
            //    lblPedimento.Text = Session["PEDIMENTOARMADO"].ToString();

            //}
            //catch (Exception ex)
            //{
            //    string mensaje = string.Empty;
            //    int idusuario = 0;
            //    if (Session["IdUsuario"] != null)
            //        idusuario = int.Parse(Session["IdUsuario"].ToString());
            //    //excepcion.RegistrarExcepcion(idusuario, "Documentos-btnOk_Click", ex, Session["Cadena"].ToString(), ref mensaje);
            //}
        }

        protected void lkb_BajarPartidas_Click(object sender, EventArgs e)
        {
            try
            {
                //Valida
                if (Session["Secuencia"] == null)
                {
                    AlertError("Debe seleccionar una secuencia");
                    return;
                }
            }
            catch (Exception ex)
            { }
        }

        protected void lkb_ImportarCove_Click(object sender, EventArgs e)
        {
            try
            {
                //Valida
                if (Session["Secuencia"] == null)
                {
                    AlertError("Debe seleccionar una secuencia");
                    return;
                }
            }
            catch (Exception ex)
            { }
        }

        //Guardar Cambios de Grid2
        protected void btnGuardarNP_Click(object sender, EventArgs e)
        {
            try
            {
                //Valida Sesiones
                if (ModalTitulo.InnerText.Contains("Agregar") && (Session["Cadena"] == null || Session["PEDIMENTOARMADO"] == null) || Session["Secuencia"] == null)
                {
                    string alerta = "<script>alert('La sesión ha expirado, debe volver a iniciar sesión');window.location.href='Login.aspx'</script>";
                    Session["Tab"] = "Salir";
                    Response.Write(alerta);
                    return;
                }

                if (ModalTitulo.InnerText.Contains("Editar") && (Session["key"] == null || Session["Cadena"] == null || Session["PEDIMENTOARMADO"] == null || Session["Secuencia"] == null))
                {
                    string alerta = "<script>alert('La sesión ha expirado, debe volver a iniciar sesión');window.location.href='Login.aspx'</script>";
                    Session["Tab"] = "Salir";
                    Response.Write(alerta);
                    return;
                }


                //Valida campos en modal
                DateTime? dateFecha = string.IsNullOrEmpty(dateEdit_Fecha.Text) ? (DateTime?)null : DateTime.Parse(dateEdit_Fecha.Text);
                decimal cantidad = string.IsNullOrEmpty(se_Cantidad.Text.Trim()) ? 0 : decimal.Parse(se_Cantidad.Text.Trim());
                decimal dolar = string.IsNullOrEmpty(se_ValorDolares.Text.Trim()) ? 0 : decimal.Parse(se_ValorDolares.Text.Trim());
                string valida = string.Empty;

                if (string.IsNullOrEmpty(txt_NumParte.Text.Trim()))
                {
                    MostrarModalNP();
                    valida = "Debe agregar un número de parte para poder " + (ModalTitulo.InnerText.Contains("Agregar") ? "guardar" : "editar");
                    AlertError(valida);

                    var lblPedimento = (ASPxLabel)GridNP.Toolbars[0].Items.FindByName("Links").FindControl("lblPedimento");
                    lblPedimento.Text = Session["PEDIMENTOARMADO"].ToString();
                    return;
                }
                if (cantidad == 0)
                {
                    MostrarModalNP();
                    valida = "Debe agregar una cantidad comercial para poder " + (ModalTitulo.InnerText.Contains("Agregar") ? "guardar" : "editar");
                    AlertError(valida);

                    var lblPedimento = (ASPxLabel)GridNP.Toolbars[0].Items.FindByName("Links").FindControl("lblPedimento");
                    lblPedimento.Text = Session["PEDIMENTOARMADO"].ToString();
                    return;
                }
                if (string.IsNullOrEmpty(txt_UMC.Text.Trim()))
                {
                    MostrarModalNP();
                    valida = "Debe agregar un UMC para poder " + (ModalTitulo.InnerText.Contains("Agregar") ? "guardar" : "editar");
                    AlertError(valida);

                    var lblPedimento = (ASPxLabel)GridNP.Toolbars[0].Items.FindByName("Links").FindControl("lblPedimento");
                    lblPedimento.Text = Session["PEDIMENTOARMADO"].ToString();
                    return;
                }
                if (dolar == 0)
                {
                    MostrarModalNP();
                    valida = "Debe agregar un valor en doláres para poder " + (ModalTitulo.InnerText.Contains("Agregar") ? "guardar" : "editar");
                    AlertError(valida);

                    var lblPedimento = (ASPxLabel)GridNP.Toolbars[0].Items.FindByName("Links").FindControl("lblPedimento");
                    lblPedimento.Text = Session["PEDIMENTOARMADO"].ToString();
                    return;
                }
                if (string.IsNullOrEmpty(txt_ClienteProv.Text.Trim()))
                {
                    MostrarModalNP();
                    valida = "Debe agregar una clave cliente/proveedor para poder " + (ModalTitulo.InnerText.Contains("Agregar") ? "guardar" : "editar");
                    AlertError(valida);

                    var lblPedimento = (ASPxLabel)GridNP.Toolbars[0].Items.FindByName("Links").FindControl("lblPedimento");
                    lblPedimento.Text = Session["PEDIMENTOARMADO"].ToString();
                    return;
                }


                //Guardar
                string mensaje = "";
                DataTable dt = new DataTable();

                if (ModalTitulo.InnerText.Contains("Agregar"))
                    dt = np.Agregar_Detalle_NP(txt_cove.Text.Trim(), txt_Factura.Text.Trim(), dateFecha, txt_NumParte.Text.Trim(), cantidad, txt_UMC.Text.Trim(), txt_Descripcion.Text.Trim(), dolar,
                                               txt_ClienteProv.Text.Trim(), txt_PO.Text.Trim(), Session["Secuencia"].ToString(), Session["PEDIMENTOARMADO"].ToString(), Session["Cadena"].ToString(), ref mensaje);
                else if (ModalTitulo.InnerText.Contains("Editar"))
                    dt = np.Editar_Detalle_NP(Int64.Parse(Session["key"].ToString().Trim()), txt_cove.Text.Trim(), txt_Factura.Text.Trim(), dateFecha, txt_NumParte.Text.Trim(), cantidad, txt_UMC.Text.Trim(), txt_Descripcion.Text.Trim(),
                                              dolar, txt_ClienteProv.Text.Trim(), txt_PO.Text.Trim(), Session["Secuencia"].ToString(), Session["PEDIMENTOARMADO"].ToString(), Session["Cadena"].ToString(), ref mensaje);


                GridNP2.DataSource = Session["GridNP2"] = dt;
                GridNP2.DataBind();
                GridNP2.Settings.VerticalScrollableHeight = 200;
                GridNP2.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;

                //Selecccionar el primer registro del grid2
                if (Session["GridNP2"] != null)
                    GridNP2.Selection.SelectRow(0);

                AlertSuccess("El registro se " + (ModalTitulo.InnerText.Contains("Editar") ? "actualizó" : "agregó") + " con éxito.");
            }
            catch (Exception ex)
            {
                AlertError(ex.Message);
            }

            var lblPedimento1 = (ASPxLabel)GridNP.Toolbars[0].Items.FindByName("Links").FindControl("lblPedimento");
            lblPedimento1.Text = Session["PEDIMENTOARMADO"].ToString();
        }

        protected void SetColores()
        {
            try
            {
                if (Session["ButtonBackColor"] != null)
                {
                    //Grid.Styles.Header.BackColor = System.Drawing.ColorTranslator.FromHtml(Session["ButtonBackColor"].ToString());
                    //Grid.Styles.Header.ForeColor = System.Drawing.ColorTranslator.FromHtml(Session["ButtonFontColor"].ToString());
                    Grid.Styles.SelectedRow.BackColor = System.Drawing.ColorTranslator.FromHtml(Session["ButtonBackColor"].ToString());
                    Grid.Styles.SelectedRow.ForeColor = System.Drawing.ColorTranslator.FromHtml(Session["ButtonFontColor"].ToString());
                }
            }
            catch (Exception ex)
            { }
        }




        //Método que oculta la columna CG
        protected void btnOcultarCG_Click(object sender, EventArgs e)
        {
            if (Grid.Columns["CG"].Visible)
            {
                Grid.Columns["CG"].Visible = false;
                btnOcultarCG.Text = "Mostrar CG";
            }
            else
            {
                Grid.Columns["CG"].Visible = true;
                btnOcultarCG.Text = "Ocultar CG";
            }

            //Se requiere cargar los datos en Grid2 que son las Fechas(Año-Mes)
            if (Grid2.VisibleRowCount == 0)
            {
                string mensaje = string.Empty;
                if (Session["FECHAS"] == null)
                    Session["FECHAS"] = docs.ConsultarFechasOperacion(lblCadena.Text, ref mensaje);

                CreaSlideBar();
            }
        }

        //Método que oculta la columna NP
        protected void btnOcultarNP_Click(object sender, EventArgs e)
        {
            if (Grid.Columns["NP"].Visible)
            {
                Grid.Columns["NP"].Visible = false;
                btnOcultarNP.Text = "Mostrar NP";
            }
            else
            {
                Grid.Columns["NP"].Visible = true;
                btnOcultarNP.Text = "Ocultar NP";
            }

            //Se requiere cargar los datos en Grid2 que son las Fechas(Año-Mes)
            if (Grid2.VisibleRowCount == 0)
            {
                string mensaje = string.Empty;
                if (Session["FECHAS"] == null)
                    Session["FECHAS"] = docs.ConsultarFechasOperacion(lblCadena.Text, ref mensaje);

                CreaSlideBar();
            }
        }

        //Método que oculta la columna (%) ED
        protected void btnOcultarPorcED_Click(object sender, EventArgs e)
        {
            if (Grid.Columns["PA"].Visible)
            {
                Grid.Columns["PA"].Visible = false;
                btnOcultarPorcED.Text = "Mostrar (%) ED";
            }
            else
            {
                Grid.Columns["PA"].Visible = true;
                btnOcultarPorcED.Text = "Ocultar (%) ED";
            }

            //Se requiere cargar los datos en Grid2 que son las Fechas(Año-Mes)
            if (Grid2.VisibleRowCount == 0)
            {
                string mensaje = string.Empty;
                if (Session["FECHAS"] == null)
                    Session["FECHAS"] = docs.ConsultarFechasOperacion(lblCadena.Text, ref mensaje);

                CreaSlideBar();
            }
        }


        #region ASIGNAR USUARIO
        //Botón en CG, abre modal para asignar usuario
        protected void lkb_Asignar_Click(object sender, EventArgs e)
        {
            try
            {
                MostrarModalCU();
                ModalTituloCU.InnerHtml = "Asignar Usuario";

                Usuarios user = new Usuarios();
                DataTable dt = new DataTable();
                string mensaje = string.Empty;
                dt = docs.Consultar_CG_SiguienteUsuario(int.Parse(lblIdUsuario.Text), lblCadena.Text, ref mensaje);

                cbxUsuarios.DataSource = dt;
                cbxUsuarios.DataBind();

                if (lbl_AsignarUsuario.Text.Trim().Length == 0)
                    cbxUsuarios.SelectedIndex = -1;

                MostrarModalCG();

                //Se requiere cargar los datos en Grid2 que son las Fechas(Año-Mes)
                if (Grid2.VisibleRowCount == 0)
                {
                    if (Session["FECHAS"] == null)
                        Session["FECHAS"] = docs.ConsultarFechasOperacion(lblCadena.Text, ref mensaje);

                    CreaSlideBar();
                }
            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (lblIdUsuario.Text.Length > 0)
                    idusuario = int.Parse(lblIdUsuario.Text);
                excepcion.RegistrarExcepcion(idusuario, "Documentos-lkb_Asignar_Click", ex, lblCadena.Text, ref mensaje);
            }
        }

        //Asignar nombre de usuario al label
        protected void lkb_AceptarCU_Click(object sender, EventArgs e)
        {
            lbl_AsignarUsuario.Text = cbxUsuarios.Text.Trim();
            lbl_IdAsignarUsuario.Text = cbxUsuarios.Value.ToString();
            MostrarModalCG();

            //Se requiere cargar los datos en Grid2 que son las Fechas(Año-Mes)
            if (Grid2.VisibleRowCount == 0)
            {
                string mensaje = string.Empty;
                if (Session["FECHAS"] == null)
                    Session["FECHAS"] = docs.ConsultarFechasOperacion(lblCadena.Text, ref mensaje);

                CreaSlideBar();
            }
        }
        #endregion


        #region EDITAR, SELECCIONAR CONCEPTO EXTRAORDINARIO
        //Botón en CG Editar, muestra los gastos extraordinarios
        protected void btnEditarCU_Click(object sender, EventArgs e)
        {
            try
            {
                ASPxButton btn = (ASPxButton)sender;

                GridViewDataItemTemplateContainer container = (GridViewDataItemTemplateContainer)btn.NamingContainer;
                Session["CGKEY"] = detailGridCG.GetRowValues(container.VisibleIndex, "CGKEY").ToString();
                Session["NO_CUENTA_DE_GASTOS"] = detailGridCG.GetRowValues(container.VisibleIndex, "NO CUENTA DE GASTOS").ToString();
                Session["CONCEPTO_EXT"] = detailGridCG.GetRowValues(container.VisibleIndex, "CONCEPTO EXTRAORDINARIO").ToString();
                
               MostrarModalCE();
               ModalTituloCE.InnerText = "Gastos Extraordinarios";

               DataTable dt = new DataTable();
               string mensaje = "";
               dt = docs.Consultar_CG_GastosExtraordinarios(lblCadena.Text, ref mensaje);

               cbxCE.DataSource = dt;
               cbxCE.DataBind();

               cbxCE.SelectedIndex = -1;
               

                MostrarModalCG();

                //Se requiere cargar los datos en Grid2 que son las Fechas(Año-Mes)
                if (Grid2.VisibleRowCount == 0)
                {
                    mensaje = string.Empty;
                    if (Session["FECHAS"] == null)
                        Session["FECHAS"] = docs.ConsultarFechasOperacion(lblCadena.Text, ref mensaje);

                    CreaSlideBar();
                }
            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (lblIdUsuario.Text.Length > 0)
                    idusuario = int.Parse(lblIdUsuario.Text);
                excepcion.RegistrarExcepcion(idusuario, "Documentos-cbxPartida_SelectedIndexChanged", ex, lblCadena.Text, ref mensaje);
            }
        }

        //Botón Aceptar Gastos Extraordinarios
        protected void lkb_AceptarCE_Click(object sender, EventArgs e)
        {
            if (Session["CGKEY"] == null)
            {
                Session["Tab"] = "Salir";
                Response.Redirect("Login.aspx", false);
                return;
            }


            MostrarModalCG();

            if (cbxCE.Text.Trim() != string.Empty)
            {
                int valida = 0;

                //Valida tabla Session["dtCG"]
                if (Session["dtCG"] != null)
                {
                    foreach (DataRow fila in ((DataTable)Session["dtCG"]).Rows)
                    {
                        if (fila["CGKEY"].ToString().Trim() == Session["CGKEY"].ToString().Trim())
                        {
                            valida = 1;

                            if(fila["CONCEPTO EXTRAORDINARIO"].ToString().Trim() == cbxCE.Text.Trim())
                            {
                                AlertError("La cuenta de gastos " + fila["NO CUENTA DE GASTOS"].ToString() + " ya tiene el concepto " + cbxCE.Text.Trim());
                                return;
                            }
                            else
                            {
                                //Edita el DataTable Session["dtCG"]
                                fila["CONCEPTO EXTRAORDINARIO"] = cbxCE.Text.Trim();
                                break;
                            }
                        }
                    }
                }




                //Guardamos en Session["dtCG"] un nuevo registro
                if (valida.Equals(0))
                {
                    DataTable dt = new DataTable();

                    if (Session["dtCG"] != null)
                        dt = ((DataTable)Session["dtCG"]);

                    DataRow row;
                    row = dt.NewRow();
                    row["CGKEY"] = Session["CGKEY"].ToString();
                    row["NO CUENTA DE GASTOS"] = Session["NO_CUENTA_DE_GASTOS"].ToString();
                    row["CONCEPTO EXTRAORDINARIO"] = cbxCE.Text.Trim();
                    dt.Rows.Add(row);
                    Session["dtCG"] = dt;
                }
            }
            else
                AlertError("Debe seleccionar una opción");
        }
        #endregion



        //Cuando el grid detailGridCG trae datos entra aquí para pintar imagen en columna ESTATUS
        protected void btnEstatusCG_Init(object sender, EventArgs e)
        {
            ASPxButton btn = (ASPxButton)sender;
            btn.ImageUrl = "~/img/iconos/ico_exdi.png";
            string valor = string.Empty;

            for (int i = 0; i < detailGridCG.VisibleRowCount; i++)
            {
                valor = detailGridCG.GetRowValues(i, "STATUS").ToString().Trim();
                if(valor.Contains("0"))
                    btn.ImageUrl = "~/img/iconos/x_guion.png";
                else if (valor.Contains("1"))
                    btn.ImageUrl = "~/img/iconos/x_paloma.png";
                else if (valor.Contains("2"))
                    btn.ImageUrl = "~/img/iconos/x_exis.png";
                else if (valor.Contains("3"))
                    btn.ImageUrl = "~/img/iconos/x_edit.png";
            }

        }

        //Método del chckbox en detailGridCG
        protected void chkConsultarCU_Init(object sender, EventArgs e)
        {
            try
            {
                ASPxCheckBox chb = sender as ASPxCheckBox;
                GridViewDataItemTemplateContainer container = chb.NamingContainer as GridViewDataItemTemplateContainer;
            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (lblIdUsuario.Text.Length > 0)
                    idusuario = int.Parse(lblIdUsuario.Text);
                excepcion.RegistrarExcepcion(idusuario, "Documentos-chkConsultarCU_Init", ex, lblCadena.Text, ref mensaje);
            }
        }


        #region REVISADO, AUTORIZADO, RECHAZADO
        //Botón de Revisado en CG, abre el modal de pregunta
        protected void lkb_Revisar_Click(object sender, EventArgs e)
        {
            //Se requiere cargar los datos en Grid2 que son las Fechas(Año-Mes)
            if (Grid2.VisibleRowCount == 0)
            {
                string mensaje = string.Empty;
                if (Session["FECHAS"] == null)
                    Session["FECHAS"] = docs.ConsultarFechasOperacion(lblCadena.Text, ref mensaje);

                CreaSlideBar();
            }

            MostrarModalCG();


            if (lbl_AsignarUsuario.Text.Trim() == string.Empty)
            {
                AlertError("Debe asignar un usuario");
                return;
            }

            AlertQuestionCU("¿Desea pasar al estatus de revisado?");
        }

        //Botón de Revisado en CG, acepta cambios
        protected void btnOkCU_Click(object sender, EventArgs e)
        {
            try
            {
                MostrarModalCG();
                int valida = 0;
                string respuesta = string.Empty;
                string mensaje = string.Empty;
                DataTable dt = new DataTable();
                string v_CGKEY = string.Empty;
                string v_CG = string.Empty;
                string v_CE = string.Empty;

                //Se requiere cargar los datos en Grid2 que son las Fechas(Año-Mes)
                if (Grid2.VisibleRowCount == 0)
                {
                    if (Session["FECHAS"] == null)
                        Session["FECHAS"] = docs.ConsultarFechasOperacion(lblCadena.Text, ref mensaje);

                    CreaSlideBar();
                }

                //Recorre detailGridDocumentos
                for (int i = 0; i < detailGridCG.VisibleRowCount; i++)
                {
                    ASPxCheckBox chkConsultarCU = detailGridCG.FindRowCellTemplateControl(i, (GridViewDataColumn)detailGridCG.Columns["SELECCIONAR"], "chkConsultarCU") as ASPxCheckBox;

                    //if (detailGridDocumentos.Selection.IsRowSelected(i))
                    if (chkConsultarCU.Checked)
                    {
                        valida = 1;
                        break;
                    }
                }

                //Validaciones
                if (valida.Equals(0))
                {
                    AlertError("Debe seleccionar un registro para revisar");
                    return;
                }

                //Recorre detailGridDocumentos
                for (int i = 0; i < detailGridCG.VisibleRowCount; i++)
                {
                    ASPxCheckBox chkConsultarCU = detailGridCG.FindRowCellTemplateControl(i, (GridViewDataColumn)detailGridCG.Columns["SELECCIONAR"], "chkConsultarCU") as ASPxCheckBox;

                    //if (detailGridDocumentos.Selection.IsRowSelected(i))
                    if (chkConsultarCU.Checked)
                    {
                        valida = 1;
                        var rowValues = detailGridCG.GetRowValues(i, new string[] { "CGKEY", "NO CUENTA DE GASTOS", "CONCEPTO EXTRAORDINARIO" }) as object[];

                        Session["CGKEY"] = rowValues[0].ToString();
                        Session["NO CUENTA DE GASTOS"] = rowValues[1].ToString();
                        Session["CONCEPTO EXTRAORDINARIO"] = rowValues[2].ToString();
                        v_CGKEY = detailGridCG.GetRowValues(i, "CGKEY").ToString().Trim();
                        v_CG = detailGridCG.GetRowValues(i, "NO CUENTA DE GASTOS").ToString().Trim();
                        v_CE = detailGridCG.GetRowValues(i, "CONCEPTO EXTRAORDINARIO").ToString().Trim();


                        if (Session["dtCG"] != null)
                        {
                            foreach (DataRow fila in ((DataTable)Session["dtCG"]).Rows)
                            {
                                if (fila["CGKEY"].ToString().Trim() == Session["CGKEY"].ToString().Trim())
                                {
                                    string v_concepto = string.Empty;
                                    if (fila["CONCEPTO EXTRAORDINARIO"].ToString().Trim() == string.Empty)
                                        v_concepto = v_CE;
                                    else
                                        v_concepto = fila["CONCEPTO EXTRAORDINARIO"].ToString();

                                    dt = docs.CG_RevisadoAutorizadoRechazado(1, Int64.Parse(fila["CGKEY"].ToString()), int.Parse(lblIdUsuario.Text),
                                         int.Parse(lbl_IdAsignarUsuario.Text), fila["NO CUENTA DE GASTOS"].ToString(), v_concepto,
                                         lblCadena.Text, ref mensaje, out respuesta);

                                    break;
                                }
                            }
                        }
                    }
                }


                //Actualiza grid cg
                mensaje = "";
                dt = docs.ConsultarCuentaDeGastos(ASPx_Pedimento.Text, lblCadena.Text, ref mensaje);
                detailGridCG.DataSource = Session["GridCuentaGastos"] = dt;
                detailGridCG.DataBind();
                detailGridCG.Settings.VerticalScrollableHeight = 310;
                detailGridCG.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;

                //if (respuesta.Contains("ACCIONES GUARDADAS"))
                //    AlertSuccess("Cambio exitoso a estatus revisado");
                
                    
                
            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (lblIdUsuario.Text.Length > 0)
                    idusuario = int.Parse(lblIdUsuario.Text);
                excepcion.RegistrarExcepcion(idusuario, "Documentos-btnOkCU_Click", ex, lblCadena.Text, ref mensaje);
            }
        }

        //Botón de Autorizar en CG, , abre el modal de pregunta
        protected void lkb_Autorizar_Click(object sender, EventArgs e)
        {
            //Se requiere cargar los datos en Grid2 que son las Fechas(Año-Mes)
            if (Grid2.VisibleRowCount == 0)
            {
                string mensaje = string.Empty;
                if (Session["FECHAS"] == null)
                    Session["FECHAS"] = docs.ConsultarFechasOperacion(lblCadena.Text, ref mensaje);

                CreaSlideBar();
            }

            MostrarModalCG();


            if (lbl_AsignarUsuario.Text.Trim() == string.Empty)
            {
                AlertError("Debe asignar un usuario");
                return;
            }

            AlertQuestionAU("¿Desea pasar al estatus de autorizado?");
        }

        //Botón de Revisado en CG, acepta cambios
        protected void btnOkAU_Click(object sender, EventArgs e)
        {
            try
            {
                MostrarModalCG();
                int valida = 0;
                string respuesta = string.Empty;
                string mensaje = string.Empty;
                DataTable dt = new DataTable();
                string v_CGKEY = string.Empty;
                string v_CG = string.Empty;
                string v_CE = string.Empty;

                //Se requiere cargar los datos en Grid2 que son las Fechas(Año-Mes)
                if (Grid2.VisibleRowCount == 0)
                {
                    if (Session["FECHAS"] == null)
                        Session["FECHAS"] = docs.ConsultarFechasOperacion(lblCadena.Text, ref mensaje);

                    CreaSlideBar();
                }

                //Recorre detailGridDocumentos
                for (int i = 0; i < detailGridCG.VisibleRowCount; i++)
                {
                    ASPxCheckBox chkConsultarCU = detailGridCG.FindRowCellTemplateControl(i, (GridViewDataColumn)detailGridCG.Columns["SELECCIONAR"], "chkConsultarCU") as ASPxCheckBox;

                    //if (detailGridDocumentos.Selection.IsRowSelected(i))
                    if (chkConsultarCU.Checked)
                    {
                        valida = 1;
                        break;
                    }
                }

                //Validaciones
                if (valida.Equals(0))
                {
                    AlertError("Debe seleccionar un registro para autorizar");
                    return;
                }

                //Recorre detailGridDocumentos
                for (int i = 0; i < detailGridCG.VisibleRowCount; i++)
                {
                    ASPxCheckBox chkConsultarCU = detailGridCG.FindRowCellTemplateControl(i, (GridViewDataColumn)detailGridCG.Columns["SELECCIONAR"], "chkConsultarCU") as ASPxCheckBox;

                    //if (detailGridDocumentos.Selection.IsRowSelected(i))
                    if (chkConsultarCU.Checked)
                    {
                        valida = 1;
                        var rowValues = detailGridCG.GetRowValues(i, new string[] { "CGKEY", "NO CUENTA DE GASTOS", "CONCEPTO EXTRAORDINARIO" }) as object[];

                        Session["CGKEY"] = rowValues[0].ToString();
                        Session["NO CUENTA DE GASTOS"] = rowValues[1].ToString();
                        Session["CONCEPTO EXTRAORDINARIO"] = rowValues[2].ToString();
                        v_CGKEY = detailGridCG.GetRowValues(i, "CGKEY").ToString().Trim();
                        v_CG = detailGridCG.GetRowValues(i, "NO CUENTA DE GASTOS").ToString().Trim();
                        v_CE = detailGridCG.GetRowValues(i, "CONCEPTO EXTRAORDINARIO").ToString().Trim();
                        


                        if (Session["dtCG"] != null)
                        {
                            foreach (DataRow fila in ((DataTable)Session["dtCG"]).Rows)
                            {
                                if (fila["CGKEY"].ToString().Trim() == Session["CGKEY"].ToString().Trim())
                                {
                                    string v_concepto = string.Empty;
                                    if (fila["CONCEPTO EXTRAORDINARIO"].ToString().Trim() == string.Empty)
                                        v_concepto = v_CE;
                                    else
                                        v_concepto = fila["CONCEPTO EXTRAORDINARIO"].ToString();

                                    dt = docs.CG_RevisadoAutorizadoRechazado(2, Int64.Parse(fila["CGKEY"].ToString()), int.Parse(lblIdUsuario.Text),
                                         int.Parse(lbl_IdAsignarUsuario.Text), fila["NO CUENTA DE GASTOS"].ToString(), v_concepto,
                                         lblCadena.Text, ref mensaje, out respuesta);

                                    break;
                                }
                            }
                        }
                    }
                }


                //Actualiza grid cg
                mensaje = "";
                dt = docs.ConsultarCuentaDeGastos(ASPx_Pedimento.Text, lblCadena.Text, ref mensaje);
                detailGridCG.DataSource = Session["GridCuentaGastos"] = dt;
                detailGridCG.DataBind();
                detailGridCG.Settings.VerticalScrollableHeight = 310;
                detailGridCG.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;

                //if (respuesta.Contains("ACCIONES GUARDADAS"))
                //    AlertSuccess("Cambio exitoso a estatus revisado");

            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (lblIdUsuario.Text.Length > 0)
                    idusuario = int.Parse(lblIdUsuario.Text);
                excepcion.RegistrarExcepcion(idusuario, "Documentos-btnOkAU_Click", ex, lblCadena.Text, ref mensaje);
            }
        }

        //Botón de Rechazar en CG, , abre el modal de pregunta
        protected void lkb_Rechazar_Click(object sender, EventArgs e)
        {
            //Se requiere cargar los datos en Grid2 que son las Fechas(Año-Mes)
            if (Grid2.VisibleRowCount == 0)
            {
                string mensaje = string.Empty;
                if (Session["FECHAS"] == null)
                    Session["FECHAS"] = docs.ConsultarFechasOperacion(lblCadena.Text, ref mensaje);

                CreaSlideBar();
            }

            MostrarModalCG();


            if (lbl_AsignarUsuario.Text.Trim() == string.Empty)
            {
                AlertError("Debe asignar un usuario");
                return;
            }

            AlertQuestionRE("¿Desea pasar al estatus de rechazado?");
        }

        //Botón de Revisado en CG, acepta cambios
        protected void btnOkRE_Click(object sender, EventArgs e)
        {
            try
            {
                MostrarModalCG();
                //string respuesta = string.Empty;
                string mensaje = string.Empty;
                DataTable dt = new DataTable();
                int valida = 0;
                string v_cg = string.Empty;

                for (int i = 0; i < detailGridCG.VisibleRowCount; i++)
                {
                    ASPxCheckBox chkConsultarCU = detailGridCG.FindRowCellTemplateControl(i, (GridViewDataColumn)detailGridCG.Columns["SELECCIONAR"], "chkConsultarCU") as ASPxCheckBox;

                    //if (detailGridDocumentos.Selection.IsRowSelected(i))
                    if (chkConsultarCU.Checked)
                    {
                        valida = 1;
                        v_cg = detailGridCG.GetRowValues(i, "NO CUENTA DE GASTOS").ToString().Trim();
                        break;
                    }
                }

                if(valida == 0)
                {
                    AlertError("Debe seleccionar un registro");
                    return;
                }

                MostrarModalRE();
                ModalTituloRE.InnerHtml = "Rechazar No. de Cuenta";
                lblTCuentaRechazo.InnerHtml = "Indicar el motivo de rechazo de la cuenta: " + v_cg;

                //Actualiza grid cg
                mensaje = "";
                dt = docs.ConsultarCuentaDeGastos(ASPx_Pedimento.Text, lblCadena.Text, ref mensaje);
                detailGridCG.DataSource = Session["GridCuentaGastos"] = dt;
                detailGridCG.DataBind();
                detailGridCG.Settings.VerticalScrollableHeight = 310;
                detailGridCG.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;

                //if (respuesta.Contains("ACCIONES GUARDADAS"))
                //    AlertSuccess("Cambio exitoso a estatus revisado");

            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (lblIdUsuario.Text.Length > 0)
                    idusuario = int.Parse(lblIdUsuario.Text);
                excepcion.RegistrarExcepcion(idusuario, "Documentos-btnOkRE_Click", ex, lblCadena.Text, ref mensaje);
            }
        }
        #endregion

    }
}