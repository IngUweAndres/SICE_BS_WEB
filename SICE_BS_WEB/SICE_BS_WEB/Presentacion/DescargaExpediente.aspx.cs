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
using SICE_BS_WEB.WebReference;
using System.IO;
using System.Configuration;

namespace SICE_BS_WEB.Presentacion
{
    public partial class DescargaExpediente : System.Web.UI.Page
    {
        BoDescargaExpediente exp = new BoDescargaExpediente();
        Catalogos catalogo = new Catalogos();
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

                    Session["GridED"] = null;
                    TituloPanel(string.Empty);
                    RANGO.Text = DESDE.Text = HASTA.Text = string.Empty;

                    DataTable dtGrid = new DataTable();
                    GridED.DataSource = dtGrid;
                    GridED.DataBind();
                    GridED.Settings.VerticalScrollableHeight = 280;
                    GridED.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;

                    InhabilitarBotonDescarga();

                    string mensaje = "";
                    DataTable dta = new DataTable();
                    dta = catalogo.TraerAduanas(lblCadena.Text, ref mensaje);
                    cmbADUANA.DataSource = dta;
                    cmbADUANA.DataBind();

                    //Trae en una sesion la tabla de datastage
                    mensaje = string.Empty;
                    Session["DS"] = exp.Consulta_DATA_STAGE(lblCadena.Text, ref mensaje);
                    GridDS.DataSource = Session["DS"];
                    GridDS.DataBind();
                    GridDS.Settings.VerticalScrollableHeight = 280;
                    GridDS.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;

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
            InhabilitarBotonDescarga();

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
            InhabilitarBotonDescarga();

            LoadingPanel1.ContainerElementID = "Panel1";

            //System.Threading.Thread.Sleep(3000);
            
            string mensaje = "";
            //InformesVentanilla informes = new InformesVentanilla();
            DataTable dt = new DataTable();

            //Obtener valores de fechas
            DateTime? dateDesde = string.IsNullOrEmpty(DESDE.Text) ? (DateTime?)null : DateTime.Parse(DESDE.Text);
            DateTime? dateHasta = string.IsNullOrEmpty(HASTA.Text) ? (DateTime?)null : DateTime.Parse(HASTA.Text);

            //Validar fechas que existan
            if ((dateDesde == null || dateHasta == null))
            {
                AlertError("Debe seleccionar un rango de fechas validas");
                GridED.DataSource = dt;
                GridED.DataBind();
                GridED.Settings.VerticalScrollableHeight = 280;
                GridED.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                return;
            }

            //Validar fechas rangos correctos
            if ((dateDesde != null && dateHasta != null) && dateDesde > dateHasta)
            {                
                //(this.Master as Principal).AlertError("La fecha desde es mayor a la fecha hasta");
                AlertError("La fecha Desde no puede ser mayor a la fecha Hasta");
                GridED.DataSource = dt;
                GridED.DataBind();
                GridED.Settings.VerticalScrollableHeight = 280;
                GridED.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                return;
            }

            dt = exp.ConsultaExpediente(dateDesde, dateHasta, PEDIMENTO.Text, PATENTES.Text, cmbADUANA.Text, lblCadena.Text, ref mensaje);
            if (dt != null && dt.Rows.Count > 0)
            {
                GridED.DataSource = Session["GridED"] = dt;
                GridED.DataBind();
            }
            else
            {
                GridED.DataSource = Session["GridED"] = dt;
                GridED.DataBind();
                AlertError("No hay información o intentelo de nuevo");
            }

            GridED.Settings.VerticalScrollableHeight = 280;
            GridED.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
        }

        #region GridPageSize
        protected int GridPageSize
        {
            get
            {
                if (Session[PageSizeSessionKey] == null)
                    return GridED.SettingsPager.PageSize;
                return (int)Session[PageSizeSessionKey];
            }
            set { Session[PageSizeSessionKey] = value; }
        }
        #endregion

        #region GridPageSizeDS
        protected int GridPageSizeDS
        {
            get
            {
                if (Session[PageSizeSessionKey] == null)
                    return GridDS.SettingsPager.PageSize;
                return (int)Session[PageSizeSessionKey];
            }
            set { Session[PageSizeSessionKey] = value; }
        }
        #endregion

        protected void Page_Init(object sender, EventArgs e)
        {
            GridED.SettingsPager.PageSize = GridPageSize;

            #region Grid Principal

            //Cuando se quiera filtrar el Grid entra en el if
            if (Session["GridED"] != null)
            {
                GridED.DataSource = Session["GridED"];
                GridED.DataBind();
                GridED.SettingsPager.PageSize = GridPageSize;
            }

            #endregion

            #region Cuenta Gastos

            //Cuando se quiera filtrar el GridDS entra en el if
            if (Session["DS"] != null)
            {
                GridDS.DataSource = Session["DS"];
                GridDS.DataBind();
                GridDS.SettingsPager.PageSize = GridPageSizeDS;
            }

            #endregion

        }

        //Metodo que llama al Callback para actualizar el PageSize y el Grid
        protected void GridED_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            GridPageSize = int.Parse(e.Parameters);
            GridED.SettingsPager.PageSize = GridPageSize;
            GridED.DataBind();
            GridED.Settings.VerticalScrollableHeight = 280;
            GridED.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
        }

        //Metodo que llama al Callback para actualizar el GridPageSizeDS y el GridDS
        protected void GridDS_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            GridPageSizeDS = int.Parse(e.Parameters);
            GridDS.SettingsPager.PageSize = GridPageSizeDS;
            GridDS.DataBind();
            GridDS.Settings.VerticalScrollableHeight = 280;
            GridDS.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
        }

        //Metodo que llama al combo box al seleccionar la cantidad de registros en el page
        protected void PagerCombo_Load(object sender, EventArgs e)
        {
            //(sender as ASPxComboBox).Value = Grid.SettingsPager.PageSize;
        }

        
        //Metodo que muestra ventana de alerta
        public void AlertError(string mensaje)
        {
            pModal.InnerText = mensaje;
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "", "<script> document.getElementById('btnError').setAttribute('data-whatever', '" + mensaje + "'); document.getElementById('btnError').click(); </script> ", false);
        }

        protected void lkb_Excel_Click(object sender, EventArgs e)
        {
            InhabilitarBotonDescarga();

            if (GridED.VisibleRowCount > 0)
            {
                //LoadingPanel1.Cli
                Exporter.WriteXlsToResponse("DescargaExpediente", new XlsExportOptionsEx() { SheetName = "DescargaExpediente" });
            }
            else
                AlertError("No hay información por exportar");

        }

        protected void lkb_Actualizar_Click(object sender, EventArgs e)
        {
            InhabilitarBotonDescarga();
            btnBuscar_OnClick(null, null);
        }

        //Método que limpia los filtros del grid
        protected void lkb_LimpiarFiltros_Click(object sender, EventArgs e)
        {
            InhabilitarBotonDescarga();

            foreach (GridViewColumn column in GridED.Columns)
            {
                if (column is GridViewDataColumn)
                {
                    ((GridViewDataColumn)column).Settings.AutoFilterCondition = AutoFilterCondition.Default;
                    GridED.AutoFilterByColumn(column, "");
                    GridED.FilterExpression = String.Empty;
                    GridED.ClearSort();
                }
            }
        }

        //Metodo del checkbox del header
        protected void chkAll_Init(object sender, EventArgs e)
        {
            try
            {
                ASPxCheckBox chb = sender as ASPxCheckBox;
                chb.ClientSideEvents.CheckedChanged = String.Format("function (s, e) {{ chkallClick(s, e, {0}); }}", 0);                
            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "DescargaExpediente-chkAll_Init", ex, lblCadena.Text, ref mensaje);
            }
        }


        //Metodo del checkbox por fila
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
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "DescargaExpediente-chkConsultar_Init", ex, lblCadena.Text, ref mensaje);
            }
        }

        //Metodo que se ejecuta al darle clic en botón Descargar Expediente 
        protected void btnDescargar_OnClick(object sender, EventArgs e)
        {
            try
            {
                string rfc = null;
                string pedimento = null;

                if (Session["RFC"] == null)
                {
                    Response.Redirect("Login.aspx");
                    return;
                }

                rfc = Session["RFC"].ToString().Trim();

                TExpediente res;
                IExpedienteComercioservice v1 = new IExpedienteComercioservice();

                //Recorre el grid para ver si hay registros seleccionados, y si los hay abrir en zip cada registro
                //Buscar la manera de descargar más de un archivo zip
                for (int i = 0; i < GridED.VisibleRowCount; i++)
                {
                    ASPxCheckBox chkConsultar = GridED.FindRowCellTemplateControl(i, (GridViewDataColumn)GridED.Columns["TODO"], "chkConsultar") as ASPxCheckBox;

                    if (chkConsultar.Checked)
                    {
                        pedimento = GridED.GetRowValues(i, "PEDIMENTO ARMADO").ToString();
                        
                        res = v1.Getzip(rfc, pedimento);
                        string base64BinaryStr = res.Archivo.Trim();
                        
                        try
                        {
                            Response.Write("<script>window.open('" + base64BinaryStr + "','_blank') </script>");
                            HttpContext.Current.ApplicationInstance.CompleteRequest();
                            //Response.End();
                        }
                        catch (Exception ex) { string msg = ex.Message; }
                    }
                }
            }
            catch (Exception ex)
            {
                int idusuario = 0;
                string mensaje = "";
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "DescargaExpediente-DescargarZip", ex, lblCadena.Text, ref mensaje);

                AlertError("Error al descargar el archivo zipeado, intentelo más tarde");
            }
        }

        //Inhabilita botón de descarga por script
        protected void InhabilitarBotonDescarga()
        {
            //Se agrega por script que el botón de descarga este inhabilitado
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "", "<script> var btnDescargar = document.getElementById('ContentSection_btnDescargar'); btnDescargar.classList.add('disabled'); </script> ", false);
        }

        

        
        //Metodo que se ejecuta el MasterView
        protected void btnDescargarDS_OnClick(object sender, EventArgs e)
        {
            try
            {
                MultiView1.ActiveViewIndex = 1;
                h2_titulo.InnerHtml = "Descarga Expediente -> Descarga Data Stage";

                if (Session["DS"] == null)
                {
                    string mensaje = string.Empty;
                    DataTable dt2 = new DataTable();
                    dt2 = exp.Consulta_DATA_STAGE(lblCadena.Text, ref mensaje);
                    Session["DS"] = dt2;
                }

                GridDS.DataSource = Session["DS"];
                GridDS.DataBind();
                GridDS.Settings.VerticalScrollableHeight = 280;
                GridDS.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;                
            }
            catch (Exception ex)
            {
                int idusuario = 0;
                string mensaje = "";
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "DescargaExpediente-btnDescargarDS_OnClick", ex, lblCadena.Text, ref mensaje);
            }
        }

        //Botón Regresar
        protected void lkb_Regresar_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 0;
            return;
        }

        //Cuando el GridDS trae datos entra aquí para pintar imagen en botones de comluna Pedimento
        protected void ASPxButtonDoc_Init(object sender, EventArgs e)
        {
            ASPxButton btn = (ASPxButton)sender;
            btn.ImageUrl = "~/img/iconos/ico_doc1.png";
        }

        //Botón en columna Nombre, abre archivo
        protected void ASPxButtonDoc_Click(object sender, EventArgs e)
        {
            try
            {
                ASPxButton btn = (ASPxButton)sender;

                btn.ImageUrl = "~/img/iconos/ico_doc1.png";

                GridViewDataItemTemplateContainer container = (GridViewDataItemTemplateContainer)btn.NamingContainer;
                string v_FILESDSKEY = GridDS.GetRowValues(container.VisibleIndex, "FILESDSKEY").ToString().Trim();
                string v_NOMBRE = GridDS.GetRowValues(container.VisibleIndex, "DSNAME").ToString().Trim();

                Guid guidkey = Guid.Parse(v_FILESDSKEY);
                
                string mensaje = string.Empty;
                DataTable dt = new DataTable();
                byte[] vByte;
                vByte = exp.Trae_Archivo_DATA_STAGE(guidkey, lblCadena.Text, ref mensaje);

                Response.Clear();
                MemoryStream ms = new MemoryStream(vByte);
                Response.ContentType = "application/zip";
                Response.AddHeader("content-disposition", "attachment;filename=" + v_NOMBRE);
                Response.Buffer = true;
                ms.WriteTo(Response.OutputStream);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                Response.End();

            }
            catch (Exception ex) {}            
        }

        protected void lkb_ExcelDS_Click(object sender, EventArgs e)
        {
            if (GridDS.VisibleRowCount > 0)
            {
                Exporter.WriteXlsToResponse("Descarga DataStage", new XlsExportOptionsEx() { SheetName = "Descarga DataStage" });
            }
            else
                AlertError("No hay información por exportar");

        }

        protected void lkb_ActualizarDS_Click(object sender, EventArgs e)
        {
            try
            {
                string mensaje = string.Empty;
                Session["DS"] = exp.Consulta_DATA_STAGE(lblCadena.Text, ref mensaje);

                GridDS.DataSource = Session["DS"];
                GridDS.DataBind();
                GridDS.Settings.VerticalScrollableHeight = 280;
                GridDS.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "DescargaExpediente-lkb_ActualizarDS_Click", ex, lblCadena.Text, ref mensaje);
            }
        }

        //Método que limpia los filtros del GridDS
        protected void lkb_LimpiarFiltrosDS_Click(object sender, EventArgs e)
        {
            foreach (GridViewColumn column in GridDS.Columns)
            {
                if (column is GridViewDataColumn)
                {
                    ((GridViewDataColumn)column).Settings.AutoFilterCondition = AutoFilterCondition.Default;
                    GridDS.AutoFilterByColumn(column, "");
                    GridDS.FilterExpression = String.Empty;
                    GridDS.ClearSort();
                }
            }
        }


    }
}