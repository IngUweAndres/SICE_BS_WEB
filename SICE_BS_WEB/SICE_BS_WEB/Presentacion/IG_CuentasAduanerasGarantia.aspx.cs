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

namespace SICE_BS_WEB.Presentacion
{
    public partial class IG_CuentasAduanerasGarantia : System.Web.UI.Page
    {
        Inicio inicio = new Inicio();
        Perfiles perfiles = new Perfiles();
        ControlExcepciones excepcion = new ControlExcepciones();
        static string nombreArchivo = string.Empty;
        static string tituloPagina = string.Empty;
        protected static string tituloPanel = string.Empty;
        static bool permisoConsultar = false;        
        static bool permisoExportar = false;        
        bool isExport = false;
        const string PageSizeSessionKey = "ed5e843d-cff7-47a7-815e-832923f7fb09";

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
                    lblCadena.Text = Session["Cadena"].ToString();
                    Session["Tab"] = "Inicio";
                }

                if (!Page.IsPostBack)
                {
                    

                    nombreArchivo = Request.Path.Substring(Request.Path.LastIndexOf("/") + 1);
                    if (Session["Permisos"] != null)
                    {
                        DataTable dt = ((DataTable)Session["Permisos"]).Select("Archivo like '%" + nombreArchivo + "%'").CopyToDataTable();
                        tituloPagina = dt.Rows[0]["NombreModulo"].ToString();
                        permisoConsultar = Convert.ToBoolean(Convert.ToInt32(dt.Rows[0]["Consultar"].ToString()));
                        if (!permisoConsultar)
                            Response.Redirect("Default.aspx");
                        permisoExportar = Convert.ToBoolean(Convert.ToInt32(dt.Rows[0]["Exportar"].ToString()));
                        Page.Title = tituloPagina;
                    }

                    Session["Grid"] = null;
                    TituloPanel(string.Empty);
                    RANGO.Text = DESDE.Text = HASTA.Text = string.Empty;

                    DataTable dtGrid = new DataTable();
                    Grid.DataSource = dtGrid;
                    Grid.DataBind();
                    Grid.Settings.VerticalScrollableHeight = 230;
                    Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                }
            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "Page_Load", ex, lblCadena.Text, ref mensaje);
                if (mensaje.Length == 0)
                    mensaje = "Error: " + excepcion.SerializarExMessage(lblCadena.Text, ex);
                //Response.Redirect("Login.aspx");
            }
        }

        private void TituloPanel(string descripcion)
        {
            h1_titulo.InnerText = tituloPanel = tituloPagina + descripcion;
        }

        //Metodo para seleccionar rango de fechas (Hoy, Mes Actual, Año Actual, Año pasado, 5 Años)
        protected void RANGO_SelectedIndexChanged(object sender, EventArgs e)
        {
            DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            if (RANGO.SelectedIndex != -1)
            {
                switch (RANGO.SelectedItem.Value.ToString())
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
   
        //Metodo botón Buscar con los filtros el reporte 
        protected void btnBuscar_OnClick(object sender, EventArgs e)
        {
            LoadingPanel1.ContainerElementID = "Panel1";

            //System.Threading.Thread.Sleep(3000);
            
            string mensaje = "";
            InformesGenerales infogral = new InformesGenerales();
            DataTable dt = new DataTable();

            //Obtener valores de fechas
            DateTime? dateDesde = string.IsNullOrEmpty(DESDE.Text) ? (DateTime?)null : DateTime.Parse(DESDE.Text);
            DateTime? dateHasta = string.IsNullOrEmpty(HASTA.Text) ? (DateTime?)null : DateTime.Parse(HASTA.Text);

            //Validar fechas que existan
            if ((dateDesde == null || dateHasta == null))
            {
                AlertError("Debe seleccionar un rango de fechas validas");
                Grid.DataSource = dt;
                Grid.DataBind();
                return;
            }

            //Validar fechas rangos correctos
            if ((dateDesde != null && dateHasta != null) && dateDesde > dateHasta)
            {
                //(this.Master as Principal).AlertError("La fecha desde es mayor a la fecha hasta");
                AlertError("La fecha Desde no puede ser mayor a la fecha Hasta");
                Grid.DataSource = dt;
                Grid.DataBind();
                return;
            }

            //dt = infogral.ConsultarContenedores("TransporteMercancia", PEDIMENTO.Text, dateDesde, dateHasta, ref mensaje);
            dt = infogral.ConsultarCuentasAduanerasGarantia(PEDIMENTO.Text, dateDesde, dateHasta, PATENTES.Text, lblCadena.Text, ref mensaje);
            if (dt != null && dt.Rows.Count > 0)
            {    
                Grid.DataSource = Session["Grid"] = dt;
                Grid.DataBind();
                Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Hidden;
                Grid.SettingsPager.PageSize = 20;
            }
            else
            {
                Grid.DataSource = Session["Grid"] = dt;
                Grid.DataBind();
                AlertError("No hay información o intentelo de nuevo");
            }
        }

        //Propiedad GridPageSize
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

        //Metodo inicial de pantalla
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Session["Grid"] = null;
            }

            Grid.SettingsPager.PageSize = GridPageSize;
            
            //Cuando se quiera filtrar el Grid entra en el if
            if(Session["Grid"] != null)
            {
               Grid.DataSource = Session["Grid"];
               Grid.DataBind();
               Grid.SettingsPager.PageSize = GridPageSize;
            }

        }

        //Metodo que llama al Callback para actualizar el PageSize y el Grid
        protected void Grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            GridPageSize = int.Parse(e.Parameters);
            Grid.SettingsPager.PageSize = GridPageSize;
            Grid.DataBind();
        }

        //Metodo que llama al combo box al seleccionar la cantidad de registros en el page
        protected void PagerCombo_Load(object sender, EventArgs e)
        {
            (sender as ASPxComboBox).Value = Grid.SettingsPager.PageSize;
        }

        //Metodo que llama al toolbar para exportar datos
        protected void Grid_ToolbarItemClick(object source, ASPxGridToolbarItemClickEventArgs e)
        {
            if (Grid.VisibleRowCount > 0)
            {
                //ConfigureExport(Grid);

                switch (e.Item.Name)
                {
                    case "ExportToPDF":
                        Exporter.WritePdfToResponse("Cuentas Aduaneras de Garantía");
                        break;
                    case "ExportToXLS":
                        Exporter.WriteXlsToResponse("Cuentas Aduaneras de Garantía", new XlsExportOptionsEx() { SheetName = "Cuentas Aduaneras de Garantía" });
                        break;
                    case "ExportToXLSX":
                        Exporter.WriteXlsxToResponse("Cuentas Aduaneras de Garantía", new XlsxExportOptionsEx() { SheetName = "Cuentas Aduaneras de Garantía" });
                        break;
                    default:
                        break;
                }
            }
            else
                AlertError("No hay información por exportar");
        }

        //Metodo que muestra ventana de alerta
        public void AlertError(string mensaje)
        {
            pModal.InnerText = mensaje;
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "", "<script> document.getElementById('btnError').setAttribute('data-whatever', '" + mensaje + "'); document.getElementById('btnError').click(); </script> ", false);
        }

        protected void lkb_Excel_Click(object sender, EventArgs e)
        {
            if (Grid.VisibleRowCount > 0)
            {
                //LoadingPanel1.Cli
                Exporter.WriteXlsToResponse("Cuentas Aduaneras de Garantía", new XlsExportOptionsEx() { SheetName = "Cuentas Aduaneras de Garantía" });
            }
            else
                AlertError("No hay información por exportar");

        }

        protected void lkb_Actualizar_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < Grid.VisibleRowCount; i++)
            {
                Grid.DataSource = Session["Grid"];
                Grid.DataBind();
                Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Hidden;
                Grid.SettingsPager.PageSize = 20;
            }
        }

        //Método que limpia los filtros del grid
        protected void lkb_LimpiarFiltros_Click(object sender, EventArgs e)
        {
            foreach (GridViewColumn column in Grid.Columns)
            {
                if (column is GridViewDataColumn)
                {
                    ((GridViewDataColumn)column).Settings.AutoFilterCondition = AutoFilterCondition.Default;
                    Grid.AutoFilterByColumn(column, "");
                    Grid.FilterExpression = String.Empty;
                    Grid.ClearSort();
                }
            }
        }

    }
}