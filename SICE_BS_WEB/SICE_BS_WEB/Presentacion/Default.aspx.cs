using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SICE_BS_WEB.Negocios;
using DevExpress.XtraCharts;
using DevExpress.XtraCharts.Web;
using System.Data;
using DevExpress.Web;
using DevExpress.Web.Bootstrap;
using System.Drawing;
using System.Configuration;

using System.IO;

namespace SICE_BS_WEB.Presentacion
{
    public partial class Default : System.Web.UI.Page
    {
        ControlExcepciones excepcion = new ControlExcepciones();
        Defaul def = new Defaul();

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
                    Session["Tab"] = "Salir";
                    //Response.Write(alerta);
                    Response.Redirect("Login.aspx", false);
                    return;
                }
                else
                {
                    lblCadena.Text = Session["Cadena"].ToString();
                    Session["Tab"] = "Inicio";
                }

                if (!Page.IsPostBack)
                {

                    //string connStr = ConfigurationSettings.AppSettings("myConnectionString");
                    //string connStr2 = ConfigurationManager.ConnectionStrings["SICE_WEB_Connection"].ConnectionString;

                    //var setts = ConfigurationManager.ConnectionStrings["SICE_WEB_Connection"];
                    //var fi = typeof(ConfigurationElement).GetField("_bReadOnly", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                    //fi.SetValue(setts, false);
                    ////setts.ConnectionString = "Data Source=198.72.112.86,1910;Initial Catalog=vicrila_pvu_fs;User ID=sa;Password=Lmorante01;";

                    //string source = string.Empty;
                    //string bd = string.Empty;
                    //string id = string.Empty;
                    //string pwd = string.Empty;

                    //source = "198.72.112.86,1910,1226";
                    //bd = "vicrila_pvu_fs";
                    //id = "sa";
                    //pwd = "Lmorante01";

                    //string conectionstring = string.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3};", source, bd, id, pwd);


                    //setts.ConnectionString = conectionstring;
                    //string connStr3 = ConfigurationManager.ConnectionStrings["SICE_WEB_Connection"].ConnectionString;


                    //string source = string.Empty;
                    //string bd = string.Empty;
                    //string id = string.Empty;
                    //string pwd = string.Empty;

                    //source = "198.72.112.86,1910,1226";
                    //bd = "vicrila_pvu_fs";
                    //id="sa";
                    //pwd = "Lmorante01";

                    //string conectionstring = string.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3};", source, bd, id, pwd);
                    //AppSetting setting = new AppSetting();
                    //setting.SaveConnectionString("SICE_WEB_Connection", conectionstring);

                    //string newCnnStr = "Data Source=198.72.112.86,1910,1226;Initial Catalog=vicrila_pvu_fs;Persist Security Info=True;User ID=sa;Password=Lmorante01";
                    //Properties.Settings.Default["SICE_WEB_Connection"] = newCnnStr;
                    
                    //var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    //var connectionStringsSection = (ConnectionStringsSection)config.GetSection("SICE_WEB_Connection");
                    //connectionStringsSection.ConnectionStrings["Blah"].ConnectionString = "Data Source=198.72.112.86,1910,1226;Initial Catalog=vicrila_pvu_fs;UID=sa;password=Lmorante01";
                    //config.Save();
                    //ConfigurationManager.RefreshSection("SICE_WEB_Connection");

                    //ConfigurationManager.RefreshSection("SICE_WEB_Connection");
                    //string connStr6 = ConfigurationManager.ConnectionStrings["SICE_WEB_Connection"].ConnectionString;

                    //Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    //config.ConnectionStrings.ConnectionStrings[_connectionStringName].ConnectionString = textBox1.Text;
                    //config.Save(ConfigurationSaveMode.Modified, true);
                    //ConfigurationManager.RefreshSection("connectionStrings");





                    

                    //Invisible las fechas
                    InvisibleFechas();
                    
                    //Por default se selecciona la opción "los últimos 30 días"
                    PERIODO.SelectedIndex = 0;
                    DateTime desde = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(-30);
                    DateTime hasta = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

                    Session["desde"] = DESDE.Text = desde.ToShortDateString();
                    Session["hasta"] = HASTA.Text = hasta.ToShortDateString();
                    //Se obtienen valores y se pinta la gráfica de Dona
                    MostrarTotales(desde, hasta);

                    //Pinta la gráfica de Total de Coves
                    //PintaGraficaTotalCoves();

                    #region Muestra la primera gráfica diseño anterior
                    //if (Session["dtCoves"] != null)
                    //{
                    //    DataTable table = new DataTable();
                    //    table.Columns.Add("Fecha", typeof(string));
                    //    table.Columns.Add("TOTAL COVES", typeof(int));
                    //    DataRow row;
                    //    foreach (DataRow fila in ((DataTable)(Session["dtCoves"])).Rows)
                    //    {
                    //        row = table.NewRow();
                    //        row["Fecha"] = fila["Fecha"].ToString().Substring(0, 10);
                    //        row["TOTAL COVES"] = int.Parse(fila["TOTAL COVES"].ToString());
                    //        table.Rows.Add(row);
                    //    }                        

                    //    ChartArea.DataSource = table;
                    //    ChartArea.SettingsCommonSeries.ArgumentField = "Fecha";
                    //    ChartArea.SeriesCollection.Add(new BootstrapChartLineSeries() { ValueField = "TOTAL COVES", Name = "TOTAL COVES : " + sCoves.InnerText });
                    //    ChartArea.ValueAxisCollection.Add(new BootstrapChartValueAxis() { TickInterval = 1});
                    //    ChartArea.DataBind();

                    //}
                    #endregion

                    #region Muestra la gráfica de Dona diseños anteriores
                    //if (Session["dtDonaCoves"] != null)
                    //{
                        #region WebChartControlDonut
                        //DataTable dtD = new DataTable();
                        //dtD.Columns.Add("STATUS", typeof(string));
                        //dtD.Columns.Add("TOTAL", typeof(int));
                        //DataRow row;
                        //foreach (DataRow fila in ((DataTable)(Session["dtDonaCoves"])).Rows)
                        //{
                        //    row = dtD.NewRow();
                        //    row["STATUS"] = fila["STATUS"].ToString();
                        //    row["TOTAL"] = int.Parse(fila["TOTAL"].ToString());
                        //    dtD.Rows.Add(row);
                        //}

                        //row = dtD.NewRow();
                        //row["STATUS"] = "s1";
                        //row["TOTAL"] = "5";
                        //dtD.Rows.Add(row);
                        //row = dtD.NewRow();
                        //row["STATUS"] = "s2";
                        //row["TOTAL"] = "10";
                        //dtD.Rows.Add(row);
                        //row = dtD.NewRow();
                        //row["STATUS"] = "s3";
                        //row["TOTAL"] = "15";
                        //dtD.Rows.Add(row);
                        //row = dtD.NewRow();
                        //row["STATUS"] = "s4";
                        //row["TOTAL"] = "25";
                        //dtD.Rows.Add(row);

                        //WebChartControlDonut.SeriesTemplate.ChangeView(ViewType.Doughnut);
                        //WebChartControlDonut.SeriesDataMember = "STATUS";
                        //WebChartControlDonut.SeriesSelectionMode = SeriesSelectionMode.Series;
                        //WebChartControlDonut.SeriesTemplate.ArgumentDataMember = "STATUS";
                        //WebChartControlDonut.SeriesTemplate.ValueDataMembers.AddRange("TOTAL");

                        //// Create a doughnut series.
                        //Series series1 = new Series("Series 1", ViewType.Doughnut);

                        //// Populate the series with points.
                        //foreach (DataRow fila in ((DataTable)(Session["dtDonaCoves"])).Rows)
                        //{
                        //    series1.Points.Add(new SeriesPoint(fila["STATUS"].ToString(), int.Parse(fila["TOTAL"].ToString())));
                        //}

                        //// Add the series to the chart.
                        //WebChartControlDonut.Series.Add(series1);

                        //// Specify the text pattern of series labels.
                        //series1.Label.TextPattern = "{A}: {VP:P0}";

                        //// Specify how series points are sorted.
                        //series1.SeriesPointsSorting = SortingMode.Ascending;
                        //series1.SeriesPointsSortingKey = SeriesPointKey.Argument;

                        //// Specify the behavior of series labels.
                        //((DoughnutSeriesLabel)series1.Label).Position = PieSeriesLabelPosition.TwoColumns;
                        //((DoughnutSeriesLabel)series1.Label).ResolveOverlappingMode = ResolveOverlappingMode.Default;
                        //((DoughnutSeriesLabel)series1.Label).ResolveOverlappingMinIndent = 5;

                        ////((DoughnutSeriesView)series1.View).ExplodedPoints.Add(series1.Points[0]);
                        ////((DoughnutSeriesView)series1.View).ExplodedDistancePercentage = 10;

                        //// Access the diagram's options.
                        ////((SimpleDiagram)WebChartControlDonut.Diagram).Dimension = 2;

                        //// Add a title to the chart and hide the legend.
                        ////ChartTitle chartTitle1 = new ChartTitle();
                        ////chartTitle1.Text = "Status Covesss";
                        ////WebChartControlDonut.Titles.Add(chartTitle1);
                        ////WebChartControlDonut.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;

                        //// Add the chart to the form.
                        //this.Controls.Add(WebChartControlDonut);
                        //WebChartControlDonut.DataBind();
                        #endregion

                        //BootstrapPieChart1.SeriesCollection.Add(new BootstrapPieChartSeries() { ValueField = "TOTAL" });
                        //BootstrapPieChart1.SettingsCommonSeries.ArgumentField = "STATUS";
                        //BootstrapPieChart1.DataSource = ((DataTable)(Session["dtDonaCoves"]));
                        //BootstrapPieChart1.DataBind();

                        //ChartDonut.SeriesCollection.Add(new BootstrapPieChartSeries() { ValueField = "TOTAL" });
                        //ChartDonut.SettingsCommonSeries.ArgumentField = "STATUS";
                        //ChartDonut.DataSource = ((DataTable)(Session["dtDonaCoves"]));
                        ////ChartDonut.SettingsCommonSeries.ValueField = "TOTAL";                        
                        ////ChartDonut.SettingsToolTip.Enabled = true;
                        ////ChartDonut.SettingsToolTip.OnClientCustomizeTooltip = "customizeTooltipDona";
                        ////ChartDonut.SettingsToolTip.ArrowLength = 20;
                        ////ChartDonut.SettingsToolTip.ArgumentFormat.Currency = "";
                        //ChartDonut.DataBind();

                    //}
                    #endregion
                }

                //Evento que dispara al hacer clic en botones (Coves, Coves Descargados, Edocs, Edocs Descargados)
                var arg = Request.Form["__EVENTTARGET"];
                if (arg != null && arg.ToString().Trim() != string.Empty && !arg.Contains("PERIODO"))
                {
                   string divID = (string)arg;
                   Clic_Botones(divID);
                }

            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "Default-Page_Load", ex, lblCadena.Text, ref mensaje);
                if (mensaje.Length == 0)
                    mensaje = "Error: " + excepcion.SerializarExMessage(lblCadena.Text, ex);
                Response.Redirect("Login.aspx");
            }
        }

        protected void PERIODO_SelectedIndexChanged(object sender, EventArgs e)
        {
            DateTime hasta = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            DateTime desde;

            Session["hasta"] = hasta.ToShortDateString();

            if (PERIODO.SelectedIndex != -1)
            {
                switch (PERIODO.SelectedItem.Value.ToString())
                {
                    case "1": //Últimos 30 días
                        desde = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(-30);
                        Session["desde"] = desde.ToShortDateString();
                        InvisibleFechas();
                        MostrarTotales(desde, hasta);
                        break;
                    case "2": //Últimos 7 días
                        desde = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(-7);
                        Session["desde"] = desde.ToShortDateString();
                        InvisibleFechas();
                        MostrarTotales(desde, hasta);
                        break;
                    //case "3": //Año pasado
                    //    desde = new DateTime(DateTime.Now.Year - 1, 1, 1);
                    //    hasta = new DateTime(DateTime.Now.Year - 1, 12, 31);
                    //    InvisibleFechas();
                    //    MostrarTotales(desde, hasta);
                    //    break;
                    case "3": //Personalizado
                        DESDE.Text = Session["desde"].ToString();
                        HASTA.Text = Session["hasta"].ToString();
                        MostrarTotales(Convert.ToDateTime(DESDE.Text), hasta);
                        VisibleFechas();
                        break;
                }

                

                //Pinta la gráfica de Total de Coves para cuando se selecciona periodo de 30 o 7 días con las nuevas fechas
                //if (PERIODO.SelectedItem.Value.ToString().Contains("1") || PERIODO.SelectedItem.Value.ToString().Contains("2"))
                //{
                //    //Pinta la gráfica de Total de Coves
                //    PintaGraficaTotalCoves();
                //}
            }
        }

        protected void InvisibleFechas()
        {
            FgDesde.Visible = FgHasta.Visible = false;
            DESDE.Text = HASTA.Text = string.Empty;
            DivActualizar.Visible = false;
        }

        protected void VisibleFechas()
        {
            FgDesde.Visible = FgHasta.Visible = true;
            DivActualizar.Visible = true;
        }

        //Trae de BD la información por los parámetros de fecha, pinta gráfica Barras(Tipo Operaciones), pinta la gráfica de Dona(Estatus Coves)
        protected void MostrarTotales(DateTime desde, DateTime hasta)
        {
            ////Se obtienen totales
            //int total_coves = 0;
            //int total_coves_descargados = 0;
            //int total_edocs = 0;
            //int total_edocs_descargados = 0;
            string mensaje = "";
            
            try
            {
                //Session["dtCoves"] = def.TraerTotalCoves(desde, hasta, ref mensaje);
                //mensaje = "";
                //Session["dtCovesD"] = def.TraerTotalCovesDescargados(desde, hasta, ref mensaje);
                //mensaje = "";
                //Session["dtEdocs"] = def.TraerTotalEdocs(desde, hasta, ref mensaje);
                //mensaje = "";
                //Session["dtEdocsD"] = def.TraerTotalEdocsDescargados(desde, hasta, ref mensaje);
                //mensaje = "";
                //Session["dtDonaCoves"] = def.TraerCoveGrafica(desde, hasta, ref mensaje);

                //if (Session["dtCoves"] != null && ((DataTable)(Session["dtCoves"])).Rows.Count > 0)
                //{
                //    foreach (DataRow fila in ((DataTable)(Session["dtCoves"])).Rows)
                //        total_coves += int.Parse(fila["TOTAL COVES"].ToString());
                //    sCoves.InnerText = total_coves.ToString();
                //}
                //else
                //    sCoves.InnerText = total_coves.ToString();
                //if (Session["dtCovesD"] != null && ((DataTable)(Session["dtCovesD"])).Rows.Count > 0)
                //{
                //    foreach (DataRow fila in ((DataTable)(Session["dtCovesD"])).Rows)
                //        total_coves_descargados += int.Parse(fila["TOTAL COVES"].ToString());
                //    sCovesD.InnerText = total_coves_descargados.ToString();
                //}
                //else
                //    sCovesD.InnerText = total_coves_descargados.ToString();
                //if (Session["dtEdocs"] != null && ((DataTable)(Session["dtEdocs"])).Rows.Count > 0)
                //{
                //    foreach (DataRow fila in ((DataTable)(Session["dtEdocs"])).Rows)
                //        total_edocs += int.Parse(fila["TOTAL"].ToString());
                //    sEdocs.InnerText = total_edocs.ToString();
                //}
                //else
                //    sEdocs.InnerText = total_edocs.ToString();
                //if (Session["dtEdocsD"] != null && ((DataTable)(Session["dtEdocsD"])).Rows.Count > 0)
                //{
                //    foreach (DataRow fila in ((DataTable)(Session["dtEdocsD"])).Rows)
                //        total_edocs_descargados += int.Parse(fila["EDOCS DESCARGADOS"].ToString());
                //    sEdocsD.InnerText = total_edocs_descargados.ToString();
                //}
                //else
                //    sEdocsD.InnerText = total_edocs_descargados.ToString();

                //Trae valores de fechas a los campos de las gráficas
                lbl1_FechaIni.Text = lbl2_FechaIni.Text = lbl5_FechaIni.Text = lbl6_FechaIni.Text = desde.ToString().Substring(0, 10);
                lbl1_FechaFin.Text = lbl2_FechaFin.Text = lbl5_FechaFin.Text = lbl6_FechaFin.Text = hasta.ToString().Substring(0, 10);

                //Pinta la gráfica barras(Tipo Operaciones)
                mensaje = "";
                DataTable dtTO = def.TraerTipoOperacion(desde, hasta, lblCadena.Text, ref mensaje);
                PintaGraficaTO(dtTO);

                //Pinta la gráfica barras(Valor Operaciones)
                mensaje = "";
                DataTable dtVO = def.TraerValorOperacion(desde, hasta, lblCadena.Text, ref mensaje);
                PintaGraficaVO(dtVO);

                //Pinta la gráfica de Dona(Estatus Coves)
                mensaje = "";
                Session["dtDonaCoves"] = def.TraerCoveGrafica(desde, hasta, lblCadena.Text, ref mensaje);
                PintaGraficaDona();

                //Pinta la gráfica de Dona2(Estatus Edocuments)
                mensaje = "";
                Session["dtDonaEd"] = def.TraerEdGrafica(desde, hasta, lblCadena.Text, ref mensaje);
                PintaGraficaDona2();

            }
            catch (Exception ex) {
                string mjs = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "Default-MostrarTotales", ex, lblCadena.Text, ref mjs);
                if (mjs.Length == 0)
                    mjs = "Error: " + excepcion.SerializarExMessage(lblCadena.Text, ex);
            }
        }

        //Muestra gráficas según el Botón elegido (Coves, Coves Descargados, Edocs, Edocs Descargados)
        protected void Clic_Botones(string valor)
        {
            try
            {
                //ChartArea.SeriesCollection.Clear();

                //DataTable dt;
                //if (valor.Contains("divCoves"))
                //{
                //    dt = new DataTable();
                //    dt.Columns.Add("Fecha", typeof(string));
                //    dt.Columns.Add("TOTAL COVES", typeof(int));
                //    DataRow row;
                //    if (Session["dtCoves"] != null && ((DataTable)(Session["dtCoves"])).Rows.Count > 0)
                //    {
                //        foreach (DataRow fila in ((DataTable)(Session["dtCoves"])).Rows)
                //        {
                //            row = dt.NewRow();
                //            row["Fecha"] = fila["Fecha"].ToString().Substring(0, 10);
                //            row["TOTAL COVES"] = int.Parse(fila["TOTAL COVES"].ToString());
                //            dt.Rows.Add(row);
                //        }
                //    }

                //    ChartArea.DataSource = dt;
                //    ChartArea.SettingsCommonSeries.ArgumentField = "Fecha";
                //    ChartArea.SeriesCollection.Add(new BootstrapChartLineSeries() { ValueField = "TOTAL COVES", Name = "TOTAL COVES : " + sCoves.InnerText });
                //    ChartArea.DataBind();
                //}
                //else if (valor.Contains("divDesCoves"))
                //{
                //    dt = new DataTable();
                //    dt.Columns.Add("Fecha", typeof(string));
                //    dt.Columns.Add("TOTAL COVES", typeof(int));
                //    DataRow row;
                //    if (Session["dtCovesD"] != null && ((DataTable)(Session["dtCovesD"])).Rows.Count > 0)
                //    {
                //        foreach (DataRow fila in ((DataTable)(Session["dtCovesD"])).Rows)
                //        {
                //            row = dt.NewRow();
                //            row["Fecha"] = fila["Fecha"].ToString().Substring(0, 10);
                //            row["TOTAL COVES"] = int.Parse(fila["TOTAL COVES"].ToString());
                //            dt.Rows.Add(row);
                //        }
                //    }

                //    ChartArea.DataSource = dt;
                //    ChartArea.SettingsCommonSeries.ArgumentField = "Fecha";
                //    ChartArea.SeriesCollection.Add(new BootstrapChartLineSeries() { ValueField = "TOTAL COVES", Name = "TOTAL COVES DESCARGADOS : " + sCovesD.InnerText });
                //    ChartArea.DataBind();
                //}
                //else if (valor.Contains("divEdocs"))
                //{
                //    dt = new DataTable();
                //    dt.Columns.Add("Fecha", typeof(string));
                //    dt.Columns.Add("TOTAL", typeof(int));
                //    DataRow row;
                //    if (Session["dtEdocs"] != null && ((DataTable)(Session["dtEdocs"])).Rows.Count > 0)
                //    {
                //        foreach (DataRow fila in ((DataTable)(Session["dtEdocs"])).Rows)
                //        {
                //            row = dt.NewRow();
                //            row["Fecha"] = fila["Fecha"].ToString().Substring(0, 10);
                //            row["TOTAL"] = int.Parse(fila["TOTAL"].ToString());
                //            dt.Rows.Add(row);
                //        }
                //    }
                //    ChartArea.DataSource = dt;
                //    ChartArea.SettingsCommonSeries.ArgumentField = "Fecha";
                //    ChartArea.SeriesCollection.Add(new BootstrapChartLineSeries() { ValueField = "TOTAL", Name = "TOTAL EDOCS : " + sEdocs.InnerText });
                //    ChartArea.DataBind();
                //}
                //else if (valor.Contains("divDesEdocs"))
                //{
                //    dt = new DataTable();
                //    dt.Columns.Add("Fecha", typeof(string));
                //    dt.Columns.Add("TOTAL", typeof(int));
                //    DataRow row;
                //    if (Session["dtEdocsD"] != null && ((DataTable)(Session["dtEdocsD"])).Rows.Count > 0)
                //    {
                //        foreach (DataRow fila in ((DataTable)(Session["dtEdocsD"])).Rows)
                //        {
                //            row = dt.NewRow();
                //            row["Fecha"] = fila["Fecha"].ToString().Substring(0, 10);
                //            row["TOTAL"] = int.Parse(fila["EDOCS DESCARGADOS"].ToString());
                //            dt.Rows.Add(row);
                //        }
                //    }
                //    ChartArea.DataSource = dt;
                //    ChartArea.SettingsCommonSeries.ArgumentField = "Fecha";
                //    ChartArea.SeriesCollection.Add(new BootstrapChartLineSeries() { ValueField = "TOTAL", Name = "TOTAL EDOCS DESCARGADOS : " + sEdocsD.InnerText });
                //    ChartArea.DataBind();
                //}

                //Pinta la gráfica de dona
                //PintaGraficaDona();
            }
            catch (Exception ex) {
                string mjs = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "Default-Clic_Botones", ex, lblCadena.Text, ref mjs);
                if (mjs.Length == 0)
                    mjs = "Error: " + excepcion.SerializarExMessage(lblCadena.Text, ex);
            }
        }

        //Botón Actualizar
        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            //Validar que las fechas cumplan un rango de 90 días            
            DateTime hasta = new DateTime(int.Parse(HASTA.Text.Substring(6,4)), int.Parse(HASTA.Text.Substring(3,2)), int.Parse(HASTA.Text.Substring(0,2)));
            DateTime desde = new DateTime(int.Parse(DESDE.Text.Substring(6, 4)), int.Parse(DESDE.Text.Substring(3, 2)), int.Parse(DESDE.Text.Substring(0, 2)));
            double dias = (hasta - desde).TotalDays;

            /*if (dias > 90)
            {
                AlertError("El periodo personalizado puede abarcar hasta 90 días");
                hasta = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddYears(4);
                desde = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddYears(4);
            }*/

            MostrarTotales(desde, hasta);
            
            //Pinta la gráfica de Total de Coves
            //PintaGraficaTotalCoves();            
        }

        //Metodo que muestra ventana de alerta
        public void AlertError(string mensaje)
        {
            pModalError.InnerText = mensaje;
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "", "<script> document.getElementById('btnError').setAttribute('data-whatever', '" + mensaje + "'); document.getElementById('btnError').click(); </script> ", false);
        }

        public void PintaGraficaTotalCoves()
        {
            ////Pinta la gráfica de Total de Coves
            //ChartArea.SeriesCollection.Clear();
            //DataTable dt = new DataTable();
            //dt.Columns.Add("Fecha", typeof(string));
            //dt.Columns.Add("TOTAL COVES", typeof(int));
            //DataRow row;
            //if (Session["dtCoves"] != null && ((DataTable)(Session["dtCoves"])).Rows.Count > 0)
            //{
            //    foreach (DataRow fila in ((DataTable)(Session["dtCoves"])).Rows)
            //    {
            //        row = dt.NewRow();
            //        row["Fecha"] = fila["Fecha"].ToString().Substring(0, 10);
            //        row["TOTAL COVES"] = int.Parse(fila["TOTAL COVES"].ToString());
            //        dt.Rows.Add(row);
            //    }
            //}

            //ChartArea.DataSource = dt;
            //ChartArea.SettingsCommonSeries.ArgumentField = "Fecha";
            //ChartArea.SeriesCollection.Add(new BootstrapChartLineSeries () { ValueField = "TOTAL COVES", Name = "TOTAL COVES : " + sCoves.InnerText });
            //ChartArea.DataBind();

        }
       
        public void PintaGraficaTO(DataTable dt)
        {
            BootstrapChart1.DataSource = dt;
            BootstrapChart1.DataBind();
        }

        public void PintaGraficaVO(DataTable dtVO)
        {
            //var currentCulture = System.Globalization.CultureInfo.InstalledUICulture;
            //var numberFormat = (System.Globalization.NumberFormatInfo)currentCulture.NumberFormat.Clone();

            //if (dtVO != null && dtVO.Rows.Count > 0)
            //{
            //    DataTable dt = new DataTable();
            //    dt.Columns.Add("VALOR", typeof(double));
            //    dt.Columns.Add("IMPORTACION", typeof(string));
            //    dt.Columns.Add("EXPORTACION", typeof(string));
            //    DataRow row;
            //    foreach (DataRow fila in dtVO.Rows)
            //    {
            //        row = dt.NewRow();
            //        row["VALOR"] = double.Parse(string.Format("{0:n}", fila["VALOR"], numberFormat));
            //        //row["VALOR"] = Convert.ToDouble(string.Format("{0:n}", fila["VALOR"]));
            //        //row["VALOR"] = Convert.ToDecimal(fila["VALOR"]).ToString("#,##0.00");
            //        row["IMPORTACION"] = fila["IMPORTACION"].ToString();
            //        row["EXPORTACION"] = fila["EXPORTACION"].ToString();
            //        dt.Rows.Add(row);
            //    }

            //    BootstrapChart2.DataSource = dt;
            //}
            //else
            //    BootstrapChart2.DataSource = dtVO;

            BootstrapChart2.DataSource = dtVO;
            BootstrapChart2.DataBind();
        }

        public void PintaGraficaDona()
        {
            if (Session["dtDonaCoves"] != null && ((DataTable)(Session["dtDonaCoves"])).Rows.Count > 0)
            {
                //Pinta la gráfica de dona coves
                WebChartControlDona.DataSource = (DataTable)Session["dtDonaCoves"];
                WebChartControlDona.DataBind();

                #region Edición de Leyenda
                //Legend legend = WebChartControlDona.Legend;

                //// Display the chart control's legend.
                //legend.Visibility = DevExpress.Utils.DefaultBoolean.True;

                //// Define its margins and alignment relative to the diagram.
                //legend.Margins.All = 8;
                //legend.AlignmentHorizontal = LegendAlignmentHorizontal.Center;
                //legend.AlignmentVertical = LegendAlignmentVertical.BottomOutside;

                //// Define the layout of items within the legend.
                //legend.Direction = LegendDirection.LeftToRight;
                //legend.EquallySpacedItems = true;
                //legend.HorizontalIndent = 8;
                //legend.VerticalIndent = 8;
                //legend.TextVisible = true;
                //legend.TextOffset = 8;
                //legend.MarkerVisible = true;
                //legend.MarkerSize = new Size(20, 20);
                //legend.Padding.All = 4;

                //// Define the limits for the legend to occupy the chart's space.
                //legend.MaxHorizontalPercentage = 50;
                //legend.MaxVerticalPercentage = 50;

                //// Customize the legend appearance.
                //legend.BackColor = Color.Beige;
                //legend.FillStyle.FillMode = FillMode.Gradient;
                //((RectangleGradientFillOptions)legend.FillStyle.Options).Color2 = Color.Bisque;

                //legend.Border.Visibility = DevExpress.Utils.DefaultBoolean.True;
                //legend.Border.Color = Color.DarkBlue;
                //legend.Border.Thickness = 2;

                //legend.Shadow.Visible = true;
                //legend.Shadow.Color = Color.LightGray;
                //legend.Shadow.Size = 2;

                //// Customize the legend text properties.
                //legend.EnableAntialiasing = DevExpress.Utils.DefaultBoolean.False;
                //legend.Font = new Font("Arial", 9, FontStyle.Regular);
                //legend.TextColor = Color.DarkBlue;
                #endregion

                #region Borra la información anterior de las donas
                //BootstrapPieChart1.SeriesCollection.Clear();
                //BootstrapPieChart2.SeriesCollection.Clear();
                #endregion

                #region Pinta donas anteriores
                //BootstrapPieChart1.DataSource = (DataTable)Session["dtDonaCoves"];
                //BootstrapPieChart1.SettingsCommonSeries.ArgumentField = "STATUS";
                //BootstrapPieChart1.SeriesCollection.Add(new BootstrapPieChartSeries() { ValueField = "TOTAL", Name = "STATUS" });
                //BootstrapPieChart1.DataBind();
                //BootstrapPieChart2.DataSource = (DataTable)Session["dtDonaCoves"];
                //BootstrapPieChart2.SettingsCommonSeries.ArgumentField = "STATUS";
                //BootstrapPieChart2.SeriesCollection.Add(new BootstrapPieChartSeries() { ValueField = "TOTAL", Name = "STATUS" });
                //BootstrapPieChart2.DataBind();
                #endregion

            }
        }

        public void PintaGraficaDona2()
        {
            if (Session["dtDonaEd"] != null && ((DataTable)(Session["dtDonaEd"])).Rows.Count > 0)
            {
                //Pinta la gráfica de dona eds
                WebChartControlDona2.DataSource = (DataTable)Session["dtDonaEd"];
                WebChartControlDona2.DataBind();
            }
        }

    }
}