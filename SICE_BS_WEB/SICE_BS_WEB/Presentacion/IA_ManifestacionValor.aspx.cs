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
using System.Configuration;
using System.IO;
using System.Data.SqlClient;


namespace SICE_BS_WEB.Presentacion
{
    public partial class IA_ManifestacionValor : System.Web.UI.Page
    {
        Inicio inicio = new Inicio();
        InformesAnalisis informes = new InformesAnalisis();
        ControlExcepciones excepcion = new ControlExcepciones();
        static string nombreArchivo = string.Empty;
        static string tituloPagina = string.Empty;
        protected static string tituloPanel = string.Empty;
        static bool permisoConsultar = false;
        static bool permisoAgregar = false;
        static bool permisoEditar = false;
        static bool permisoEliminar = false;
        static bool permisoExportar = false;       
        const string PageSizeSessionKey = "ed5e843d-cff7-47a7-815e-832923f7fb09";
        static string reportePath = string.Empty;
        Catalogos catalogo = new Catalogos();

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
                        permisoAgregar = Convert.ToBoolean(Convert.ToInt32(dt.Rows[0]["Agregar"].ToString()));
                        permisoEditar = Convert.ToBoolean(Convert.ToInt32(dt.Rows[0]["Editar"].ToString()));
                        permisoEliminar = Convert.ToBoolean(Convert.ToInt32(dt.Rows[0]["Eliminar"].ToString()));
                        permisoExportar = Convert.ToBoolean(Convert.ToInt32(dt.Rows[0]["Exportar"].ToString()));
                        Page.Title = tituloPagina;
                    }

                    Session["Grid_MV"] = null;
                    TituloPanel(string.Empty);

                    string mensaje = "";                    
                    DataTable dth = new DataTable();

                    //Traer
                    dth = informes.ConsultarMV(Session["Cadena"].ToString(), ref mensaje);

                    //Cargar de nuevo Hoja de Cálculo
                    Grid_MV.DataSource = Session["Grid_MV"] = dth;
                    Grid_MV.DataBind();
                    Grid_MV.Settings.VerticalScrollableHeight = 260;
                    Grid_MV.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                    Grid_MV.SettingsPager.PageSize = 20;

                    Session["ASPxBinaryImage"] = null;
                }

                string v_javascript = Request["__EVENTARGUMENT"];
                if (v_javascript != null)
                {
                    if(v_javascript.Contains("VTM"))
                        btnVTM_Click(this, new EventArgs());
                    else if (v_javascript.Contains("R65"))
                        btnA65_Click(this, new EventArgs());
                }

                //Se ejecuta lo sig para validar siempre los valores de las pestañas de Valor de Transacción de las Mercancias y Articulo 65 de la Ley
                if (Session["pestaña"] != null)
                {
                    if (Session["pestaña"].ToString().Contains("VTM"))
                    {
                        //Ejecuta funcion script para validar si habilita o no pestaña de Articulo 65 de la Ley
                        ScriptManager.RegisterStartupScript(this.Page, typeof(String), "ValorTransaccionMercancias", "<script> ValorTransaccionMercancias(); </script> ", false);

                        //Ejecuta limpieza de Grid8_MV y Grid5_MV
                        btnVTM_Click(this, new EventArgs());
                    }
                    else
                    {

                        //Ejecuta funcion script para validar si habilita o no pestaña de Valor de TRansacción de las Mercancias
                        ScriptManager.RegisterStartupScript(this.Page, typeof(String), "Articulo65", "<script> Articulo65(); </script> ", false);

                        //Ejecuta limpieza de Grid3_MV y Grid4_MV
                        btnA65_Click(this, new EventArgs());
                    }
                }

            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "IA_ManifestacionValor", ex, lblCadena.Text, ref mensaje);
                if (mensaje.Length == 0)
                    mensaje = "Error: " + excepcion.SerializarExMessage(lblCadena.Text, ex);
                //Response.Redirect("Login.aspx");
            }
        }


        private void TituloPanel(string descripcion)
        {
            h2_titulo.InnerText = h1_titulo.InnerText = tituloPanel = tituloPagina + descripcion;
        }


        //Cuando el grid principal trae datos entra aquí para pintar imagen en botón de Hoja de Cálculo
        protected void ASPxButtonDetailDoc_Init(object sender, EventArgs e)
        {
            ASPxButton btn = (ASPxButton)sender;
            btn.ImageUrl = "~/img/iconos/ico_doc1.png";

            if (Session["ASPxBinaryImage"] != null)
                ASPxBinaryImage.Value = Session["ASPxBinaryImage"];
        }
   

        //Metodo botón Buscar con los filtros el reporte 
        protected void btnBuscar_OnClick(object sender, EventArgs e)
        {
            try
            {
                DateTime? dateDesde = null;
                DateTime? dateHasta = null;
                //Validar
                if (PEDIMENTO.Text.Trim().Length > 0 && (DESDE.Text.Trim().Length > 0 || HASTA.Text.Trim().Length > 0))
                {
                    AlertError("Si va a crear manifestación de valor por pedimento, asegurese de limpiar las fechas o viseversa.");
                    return;
                }

                if (PEDIMENTO.Text.Trim().Length == 0 && (DESDE.Text.Trim().Length > 0 || HASTA.Text.Trim().Length > 0))
                {
                    //Obtener valores de fechas
                    dateDesde = string.IsNullOrEmpty(DESDE.Text) ? (DateTime?)null : DateTime.Parse(DESDE.Text);
                    dateHasta = string.IsNullOrEmpty(HASTA.Text) ? (DateTime?)null : DateTime.Parse(HASTA.Text);

                    //Validar fechas rangos correctos
                    if ((dateDesde != null && dateHasta != null) && dateDesde > dateHasta)
                    {
                        AlertError("La fecha Desde no puede ser mayor a la fecha Hasta");
                        return;
                    }
                }

                LoadingPanel1.ContainerElementID = "Panel1";

                //System.Threading.Thread.Sleep(3000);

                string mensaje = "";
                DataTable dt = new DataTable();

                if (PEDIMENTO.Text.Trim().Length > 0)
                    dt = informes.ConsultarPorPedimentoEnMV(PEDIMENTO.Text.Trim(), lblCadena.Text, ref mensaje);
                else
                    dt = informes.ConsultarPorFechasEnMV(dateDesde, dateHasta, lblCadena.Text, ref mensaje);

                Grid_MV.DataSource = Session["Grid_MV"] = dt;
                Grid_MV.DataBind();
                Grid_MV.Settings.VerticalScrollableHeight = 260;
                Grid_MV.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                Grid_MV.SettingsPager.PageSize = 20;

            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "IA_ManifestacionValor-btnBuscar_OnClick", ex, lblCadena.Text, ref mensaje);
            }
        }


        //Botón en columna de Pedimento, abre archivo PDF
        protected void ASPxButtonDetailDoc_Click(object sender, EventArgs e)
        {
            try
            {
                ASPxButton btn = (ASPxButton)sender;

                btn.ImageUrl = "~/img/iconos/ico_doc1.png";

                GridViewDataItemTemplateContainer container = (GridViewDataItemTemplateContainer)btn.NamingContainer;
                string v_mvkeymanifestacion = Grid_MV.GetRowValues(container.VisibleIndex, "mvkeymanifestacion").ToString();
                string v_pedimento = Grid_MV.GetRowValues(container.VisibleIndex, "pedimentoarmado").ToString();


                //MOSTRAR REPORTE
                string FilePath = string.Empty;
                int numeroSinCotaArbitraria = 0;

                string FolderPath = ConfigurationManager.AppSettings["FolderPath"];
                FilePath = Server.MapPath(FolderPath);

                //Se borrarán si existen reportes dejados mayor a 4 reportes
                string filesToDelete = @"report_*.pdf";   // Only delete PDF files containing "report_" in their filenames
                string[] fileList = System.IO.Directory.GetFiles(FilePath, filesToDelete);

                if (fileList != null && fileList.Count() > 4)
                {
                    foreach (string file in fileList)
                    {
                        //System.Diagnostics.Debug.WriteLine(file + "will be deleted");
                        System.IO.File.Delete(file);
                    }
                }

                Random rnd = new Random();
                // Obtiene un número natural (incluye el 0) aleatorio entre 0 e int.MaxValue
                numeroSinCotaArbitraria = rnd.Next();

                //Reportes reportes = new Reportes();
                MValor reporte = new MValor();
                //string mensaje = string.Empty;
                //Guid keyguid = Guid.Parse(v_mvkeymanifestacion);
                //reporte.DataSource = reportes.Traer_Anexo(keyguid, Session["Cadena"].ToString(), ref mensaje);
                reporte.Parameters[0].Value = v_pedimento;
                reporte.Parameters[0].Visible = false;
                reporte.Parameters[1].Value = Int64.Parse(v_mvkeymanifestacion);
                reporte.Parameters[1].Visible = false;
                reporte.Parameters[2].Value = 0;
                reporte.Parameters[2].Visible = false;
                //reporte.ExportToPdf(FilePath + "\\report_" + Convert.ToString(numeroSinCotaArbitraria) + ".pdf");
                //reportePath = FilePath + "\\report_" + Convert.ToString(numeroSinCotaArbitraria) + ".pdf";
                
                //frmPDF1.Attributes.Add("src", "report_" + Convert.ToString(numeroSinCotaArbitraria) + ".pdf");
                reporte.ExportToPdf(FilePath + "\\" + v_pedimento + "-MV.pdf");
                reportePath = FilePath + "\\" + v_pedimento + "-MV.pdf";

                frmPDF1.Attributes.Add("src", v_pedimento + "-MV.pdf");
                
                
                MostrarModal();
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message.ToString());
                string mensaje = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                //excepcion.RegistrarExcepcion(idusuario, "IA_ManifestacionValor-ASPxButtonDetailDoc_Click", ex, lblCadena.Text, ref mensaje);
            }
        }


        //Botón que borra el archivo generado pdf
        protected void BtnUpL1_Click(object sender, EventArgs e)
        {
            try
            {
                if (System.IO.File.Exists(reportePath))
                {
                    System.IO.File.Delete(reportePath);
                }

                string FolderPath = ConfigurationManager.AppSettings["FolderPath"];
                string FilePath = Server.MapPath(FolderPath);

                string files = FilePath + "\\report_";

                string[] filePaths = Directory.GetFiles(FilePath);

                foreach (string filePath in filePaths)
                {
                    if (filePath.Contains("report_"))
                        File.Delete(filePath);

                }
            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                //excepcion.RegistrarExcepcion(idusuario, "IA_ManifestacionValor", ex, Session["Cadena"].ToString(), ref mensaje);
                //if (mensaje.Length == 0)
                //    mensaje = "Error: " + excepcion.SerializarExMessage(Session["Cadena"].ToString(), ex);
                AlertError(ex.Message);
            }
        }

        private void MostrarModal()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "MostrarModal", "<script> document.getElementById('btnNuevoRpt').click(); </script> ", false);

        }

        private void MostrarModal2()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "MostrarModal2", "<script> document.getElementById('btnModal').click(); </script> ", false);

        }

        private void MostrarModal3()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "MostrarModal3", "<script> document.getElementById('btnModalF').click(); </script> ", false);

        }

        private void MostrarModalRL()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "MostrarModalRL", "<script> document.getElementById('btnModalRL').click(); </script> ", false);

        }

        private void MostrarModalRL2()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "MostrarModalRL2", "<script> document.getElementById('btnModalRL2').click(); </script> ", false);

        }

        private void MostrarModalDF()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "MostrarModalDF", "<script> document.getElementById('btnModalDF').click(); </script> ", false);

        }

        private void MostrarModalTM()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "MostrarModalTM", "<script> document.getElementById('btnModalTM').click(); </script> ", false);

        }

        private void MostrarModalTM2()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "MostrarModalTM2", "<script> document.getElementById('btnModalTM2').click(); </script> ", false);

        }

        private void MostrarModalS65()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "MostrarModalS65", "<script> document.getElementById('btnModalS65').click(); </script> ", false);

        }

        private void MostrarModalN65()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "MostrarModalN65", "<script> document.getElementById('btnModalN65').click(); </script> ", false);

        }

        private void MostrarModalOT()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "MostrarModalOT", "<script> document.getElementById('btnModalOT').click(); </script> ", false);

        }

        private void MostrarModalOT2()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "MostrarModalOT2", "<script> document.getElementById('btnModalOT2').click(); </script> ", false);

        }

        private void MostrarModalIT()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "MostrarModalIT", "<script> document.getElementById('btnModalIT').click(); </script> ", false);

        }

        //Propiedad GridPageSize
        protected int GridPageSize
        {
            get
            {
                if (Session[PageSizeSessionKey] == null)
                    return Grid_MV.SettingsPager.PageSize;
                return (int)Session[PageSizeSessionKey];
            }
            set { Session[PageSizeSessionKey] = value; }
        }

        //Propiedad GridPageSize2
        protected int GridPageSize2
        {
            get
            {
                if (Session[PageSizeSessionKey] == null)
                    return Grid2_MV.SettingsPager.PageSize;
                return (int)Session[PageSizeSessionKey];
            }
            set { Session[PageSizeSessionKey] = value; }
        }

        //Propiedad GridPageSize3
        protected int GridPageSize3
        {
            get
            {
                if (Session[PageSizeSessionKey] == null)
                    return Grid3_MV.SettingsPager.PageSize;
                return (int)Session[PageSizeSessionKey];
            }
            set { Session[PageSizeSessionKey] = value; }
        }

        //Propiedad GridPageSize4
        protected int GridPageSize4
        {
            get
            {
                if (Session[PageSizeSessionKey] == null)
                    return Grid4_MV.SettingsPager.PageSize;
                return (int)Session[PageSizeSessionKey];
            }
            set { Session[PageSizeSessionKey] = value; }
        }

        //Propiedad GridPageSize5
        protected int GridPageSize5
        {
            get
            {
                if (Session[PageSizeSessionKey] == null)
                    return Grid5_MV.SettingsPager.PageSize;
                return (int)Session[PageSizeSessionKey];
            }
            set { Session[PageSizeSessionKey] = value; }
        }

        //Propiedad GridPageSize6
        protected int GridPageSize6
        {
            get
            {
                if (Session[PageSizeSessionKey] == null)
                    return Grid6_MV.SettingsPager.PageSize;
                return (int)Session[PageSizeSessionKey];
            }
            set { Session[PageSizeSessionKey] = value; }
        }

        //Propiedad GridPageSize7
        protected int GridPageSize7
        {
            get
            {
                if (Session[PageSizeSessionKey] == null)
                    return Grid7_MV.SettingsPager.PageSize;
                return (int)Session[PageSizeSessionKey];
            }
            set { Session[PageSizeSessionKey] = value; }
        }

        //Propiedad GridPageSize8
        protected int GridPageSize8
        {
            get
            {
                if (Session[PageSizeSessionKey] == null)
                    return Grid8_MV.SettingsPager.PageSize;
                return (int)Session[PageSizeSessionKey];
            }
            set { Session[PageSizeSessionKey] = value; }
        }

        //Propiedad GridPageSize9
        protected int GridPageSize9
        {
            get
            {
                if (Session[PageSizeSessionKey] == null)
                    return Grid9_MV.SettingsPager.PageSize;
                return (int)Session[PageSizeSessionKey];
            }
            set { Session[PageSizeSessionKey] = value; }
        }

        //Propiedad GridPageSizeRL
        protected int GridPageSizeRL
        {
            get
            {
                if (Session[PageSizeSessionKey] == null)
                    return GridRL.SettingsPager.PageSize;
                return (int)Session[PageSizeSessionKey];
            }
            set { Session[PageSizeSessionKey] = value; }
        }

        //Metodo inicial de pantalla
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Session["Grid_MV"] = null;
            }

            Grid_MV.SettingsPager.PageSize = GridPageSize;
            
            //Cuando se quiera filtrar el Grid entra en el if
            if (Session["Grid_MV"] != null)
            {
                Grid_MV.DataSource = Session["Grid_MV"];
                Grid_MV.DataBind();
                Grid_MV.SettingsPager.PageSize = GridPageSize;
            }


            //Cuando se quiera filtrar el Grid2_MV entra en el if
            if (Session["Grid2_MV"] != null)
            {
                Grid2_MV.DataSource = Session["Grid2_MV"];
                Grid2_MV.DataBind();
                Grid2_MV.SettingsPager.PageSize = GridPageSize2;
            }

            //Cuando se quiera filtrar el Grid3_MV entra en el if
            if (Session["Grid3_MV"] != null)
            {
                Grid3_MV.DataSource = Session["Grid3_MV"];
                Grid3_MV.DataBind();
                Grid3_MV.SettingsPager.PageSize = GridPageSize3;
            }

            //Cuando se quiera filtrar el Grid4_MV entra en el if
            if (Session["Grid4_MV"] != null)
            {
                Grid4_MV.DataSource = Session["Grid4_MV"];
                Grid4_MV.DataBind();
                Grid4_MV.SettingsPager.PageSize = GridPageSize4;
            }

            //Cuando se quiera filtrar el Grid5_MV entra en el if
            if (Session["Grid5_MV"] != null)
            {
                Grid5_MV.DataSource = Session["Grid5_MV"];
                Grid5_MV.DataBind();
                Grid5_MV.SettingsPager.PageSize = GridPageSize5;
            }

            //Cuando se quiera filtrar el Grid6_MV entra en el if
            if (Session["Grid6_MV"] != null)
            {
                Grid6_MV.DataSource = Session["Grid6_MV"];
                Grid6_MV.DataBind();
                Grid6_MV.SettingsPager.PageSize = GridPageSize6;
            }

            //Cuando se quiera filtrar el Grid7_MV entra en el if
            if (Session["Grid7_MV"] != null)
            {
                Grid7_MV.DataSource = Session["Grid7_MV"];
                Grid7_MV.DataBind();
                Grid7_MV.SettingsPager.PageSize = GridPageSize7;
            }

            //Cuando se quiera filtrar el Grid8_MV entra en el if
            if (Session["Grid8_MV"] != null)
            {
                Grid8_MV.DataSource = Session["Grid8_MV"];
                Grid8_MV.DataBind();
                Grid8_MV.SettingsPager.PageSize = GridPageSize8;
            }

            //Cuando se quiera filtrar el Grid9_MV entra en el if
            if (Session["Grid9_MV"] != null)
            {
                Grid9_MV.DataSource = Session["Grid9_MV"];
                Grid9_MV.DataBind();
                Grid9_MV.SettingsPager.PageSize = GridPageSize9;
            }

            //Cuando se quiera filtrar el GridRL entra en el if
            if (Session["GridRL"] != null)
            {
                GridRL.DataSource = Session["GridRL"];
                GridRL.DataBind();
                GridRL.SettingsPager.PageSize = GridPageSizeRL;
            }

        }

        //Metodo que llama al Callback para actualizar el PageSize y el Grid
        protected void Grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            GridPageSize = int.Parse(e.Parameters);
            Grid_MV.SettingsPager.PageSize = GridPageSize;
            Grid_MV.DataBind();
        }

        //Checkbox All - Grid_MV
        protected void chkMarcarTodo_Init(object sender, EventArgs e)
        {
            try
            {
                ASPxCheckBox chb = sender as ASPxCheckBox;
                chb.ClientSideEvents.CheckedChanged = String.Format("function (s, e) {{ chkMarcarTodoClick(s, e, {0}); }}", 0);                
            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "IA_ManifestacionValor-chkConsultarAll_Init", ex, lblCadena.Text, ref mensaje);
            }
        }

        //Checkbox1 - Grid_MV
        protected void chkConsultar1_Init(object sender, EventArgs e)
        {
            try
            {
                ASPxCheckBox chb = sender as ASPxCheckBox;
                GridViewDataItemTemplateContainer container = chb.NamingContainer as GridViewDataItemTemplateContainer;

                chb.ClientInstanceName = String.Format("chkConsultar1{0}", container.VisibleIndex);
                chb.ClientSideEvents.CheckedChanged = String.Format("function (s, e) {{ chkConsultar1Click(s, e, {0}); }}", container.VisibleIndex);

                lblT_TotalRenglones.Text = "Total registros (" + Grid_MV.VisibleRowCount + ")";

            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "IA_ManifestacionValor-chkConsultar1_Init", ex, lblCadena.Text, ref mensaje);
            }
        }

        ////Metodo que llama al Callback para actualizar el PageSize y el Grid
        //protected void Grid2_MV_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        //{
        //    GridPageSize2 = int.Parse(e.Parameters);
        //    Grid2_MV.SettingsPager.PageSize = GridPageSize2;
        //    Grid2_MV.DataBind();
        //}

        ////Metodo que llama al Callback para actualizar el PageSize y el Grid
        //protected void Grid3_MV_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        //{
        //    GridPageSize3 = int.Parse(e.Parameters);
        //    Grid3_MV.SettingsPager.PageSize = GridPageSize3;
        //    Grid3_MV.DataBind();
        //}

        //Metodo que llama al combo box al seleccionar la cantidad de registros en el page
        protected void PagerCombo_Load(object sender, EventArgs e)
        {
            (sender as ASPxComboBox).Value = Grid_MV.SettingsPager.PageSize;
        }

        //Metodo que llama al toolbar para exportar datos
        protected void Grid_ToolbarItemClick(object source, ASPxGridToolbarItemClickEventArgs e)
        {
            if (Grid_MV.VisibleRowCount > 0)
            {
                //ConfigureExport(Grid);

                switch (e.Item.Name)
                {
                    case "ExportToPDF":
                        Exporter.WritePdfToResponse("Reporte anual de operaciones COMEXT (IMMEX)");
                        break;
                    case "ExportToXLS":
                        Exporter.WriteXlsToResponse("Reporte anual de operaciones COMEXT (IMMEX)", new XlsExportOptionsEx() { SheetName = "Reporte anual de operaciones COMEXT (IMMEX)" });
                        break;
                    case "ExportToXLSX":
                        Exporter.WriteXlsxToResponse("Reporte anual de operaciones COMEXT (IMMEX)", new XlsxExportOptionsEx() { SheetName = "Reporte anual de operaciones COMEXT (IMMEX)" });
                        break;
                    default:
                        break;
                }
            }
            else
                AlertError("No hay información por exportar");
        }

        //Metodo que muestra ventana de satisfactorio
        public void AlertSuccess(string mensaje)
        {
            pModalSucces.InnerText = mensaje;
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "", "<script> document.getElementById('btnSuccess').setAttribute('data-whatever', '" + mensaje + "'); document.getElementById('btnSuccess').click(); </script> ", false);
        }

        //Metodo que muestra ventana de alerta
        public void AlertError(string mensaje)
        {
            pModal.InnerText = mensaje;
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "", "<script> document.getElementById('btnError').setAttribute('data-whatever', '" + mensaje + "'); document.getElementById('btnError').click(); </script> ", false);
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

        //Metodo que muestra ventana de QuestionDF
        public void AlertQuestionDF(string mensaje)
        {
            pModalQuestionDF.InnerText = mensaje;
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "", "<script> document.getElementById('btnQuestionDF').setAttribute('data-whatever', '" + mensaje + "'); document.getElementById('btnQuestionDF').click(); </script> ", false);
        }

        //Metodo que muestra ventana de QuestionTM
        public void AlertQuestionTM(string mensaje)
        {
            pModalQuestionTM.InnerText = mensaje;
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "", "<script> document.getElementById('btnQuestionTM').setAttribute('data-whatever', '" + mensaje + "'); document.getElementById('btnQuestionTM').click(); </script> ", false);
        }

        //Metodo que muestra ventana de QuestionTM2
        public void AlertQuestionTM2(string mensaje)
        {
            pModalQuestionTM2.InnerText = mensaje;
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "", "<script> document.getElementById('btnQuestionTM2').setAttribute('data-whatever', '" + mensaje + "'); document.getElementById('btnQuestionTM2').click(); </script> ", false);
        }

        //Metodo que muestra ventana de QuestionS65
        public void AlertQuestionS65(string mensaje)
        {
            pModalQuestionS65.InnerText = mensaje;
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "", "<script> document.getElementById('btnQuestionS65').setAttribute('data-whatever', '" + mensaje + "'); document.getElementById('btnQuestionS65').click(); </script> ", false);
        }

        //Metodo que muestra ventana de QuestionS65
        public void AlertQuestionN65(string mensaje)
        {
            pModalQuestionS65.InnerText = mensaje;
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "", "<script> document.getElementById('btnQuestionN65').setAttribute('data-whatever', '" + mensaje + "'); document.getElementById('btnQuestionN65').click(); </script> ", false);
        }

        //Metodo que muestra ventana de QuestionOT
        public void AlertQuestionOT(string mensaje)
        {
            pModalQuestionOT.InnerText = mensaje;
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "", "<script> document.getElementById('btnQuestionOT').setAttribute('data-whatever', '" + mensaje + "'); document.getElementById('btnQuestionOT').click(); </script> ", false);
        }

        //Metodo que muestra ventana de QuestionOT2
        public void AlertQuestionOT2(string mensaje)
        {
            pModalQuestionOT2.InnerText = mensaje;
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "", "<script> document.getElementById('btnQuestionOT2').setAttribute('data-whatever', '" + mensaje + "'); document.getElementById('btnQuestionOT2').click(); </script> ", false);
        }

        //Metodo que muestra ventana de QuestionIT
        public void AlertQuestionIT(string mensaje)
        {
            pModalQuestionIT.InnerText = mensaje;
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "", "<script> document.getElementById('btnQuestionIT').setAttribute('data-whatever', '" + mensaje + "'); document.getElementById('btnQuestionIT').click(); </script> ", false);
        }

        protected void lkb_Excel_Click(object sender, EventArgs e)
        {
            if (!permisoExportar)
            {
                AlertError("No tiene permiso para exportar");
                return;
            }

            if (Grid_MV.VisibleRowCount > 0)
            {
                Exporter.WriteXlsToResponse("Manifestación Valor", new XlsExportOptionsEx() { SheetName = "Manifestación Valor" });
            }
            else
                AlertError("No hay información por exportar");

        }

        protected void lkb_Actualizar_Click(object sender, EventArgs e)
        {
            if (Session["Cadena"] == null)
            {
                string alerta = "<script>alert('La sesión ha expirado, debe volver a iniciar sesión');window.location.href='Login.aspx'</script>";
                Session["Tab"] = "Salir";
                Response.Write(alerta);
                Response.Redirect("Login.aspx", false);
                return;
            }

            string mensaje = "";
            DataTable dt = new DataTable();

            //Traer
            dt = informes.ConsultarPorPedimentoEnMV(PEDIMENTO.Text, lblCadena.Text, ref mensaje);

            lkb_LimpiarFiltros_Click(null, null);

            //Cargar de nuevo Hoja de Cálculo
            Grid_MV.DataSource = Session["Grid_MV"] = dt;
            Grid_MV.DataBind();
            Grid_MV.Settings.VerticalScrollableHeight = 260;
            Grid_MV.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
            Grid_MV.SettingsPager.PageSize = 20;
        }

        //Descargar Zip
        protected void lkb_DescargarZip_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["Cadena"] == null)
                {
                    string alerta = "<script>alert('La sesión ha expirado, debe volver a iniciar sesión');window.location.href='Login.aspx'</script>";
                    Session["Tab"] = "Salir";
                    Response.Write(alerta);
                    Response.Redirect("Login.aspx", false);
                    return;
                }

                //Declaración de variables
                string FilePath = string.Empty;
                string ZipPath = string.Empty;
                string NegocioPath = string.Empty;
                string FolderPath = ConfigurationManager.AppSettings["FolderPath"];


                //Obtiene la ruta del directorio donde se depositaran los archivos
                NegocioPath = Server.MapPath(FolderPath).Replace("Presentacion", "");
                NegocioPath = NegocioPath + "Negocio";
                FilePath = Server.MapPath(FolderPath) + "\\Temp\\MV";
                ZipPath = Server.MapPath(FolderPath) + "\\Temp\\archivo_mv.zip";

                //Valida, si existe el directorio lo borra
                if (System.IO.Directory.Exists(FilePath))
                {
                    try
                    {
                        //Borra el dirctorio con info si es que existe
                        System.IO.Directory.Delete(FilePath, true);
                    }
                    catch (System.IO.IOException ioex) { }
                }

                //Crea el directorio
                System.IO.Directory.CreateDirectory(FilePath);


                //Valida si existe el file archivo.zip que lo borre
                if (System.IO.File.Exists(ZipPath))
                {
                    System.IO.File.Delete(ZipPath);
                }

                int bandera = 0;
                for (int i = 0; i < Grid_MV.VisibleRowCount; i++)
                {
                    ASPxCheckBox chkConsultar = Grid_MV.FindRowCellTemplateControl(i, (GridViewDataColumn)Grid_MV.Columns["Seleccionar"], "chkConsultar1") as ASPxCheckBox;

                    if (chkConsultar.Checked)
                    {
                        string v_mvkeymanifestacion = Grid_MV.GetRowValues(i, "mvkeymanifestacion").ToString().Trim();
                        string v_pedimento = Grid_MV.GetRowValues(i, "pedimentoarmado").ToString().Trim();
                        bandera = 1;

                        //CREA REPORTES                                     
                        //Crea el reporte en  pdf
                        MValor reporte = new MValor();
                        reporte.Parameters[0].Value = v_pedimento;
                        reporte.Parameters[0].Visible = false;
                        reporte.Parameters[1].Value = Int64.Parse(v_mvkeymanifestacion);
                        reporte.Parameters[1].Visible = false;
                        reporte.Parameters[2].Value = 0;
                        reporte.Parameters[2].Visible = false;

                        //Valida, sí existe el archivo del reporte con el mismo pedimento agregarle un + 1 al nombre
                        int contador = 1;                        
                        while (System.IO.File.Exists(FilePath + "\\" + v_pedimento + "-MV.pdf"))
                        {
                            v_pedimento = v_pedimento + "(" + contador.ToString() + ")";
                            contador += 1;
                        }  
                      
                        reporte.ExportToPdf(FilePath + "\\" + v_pedimento + "-MV.pdf");
                    }
                }


                if (bandera.Equals(1))
                {
                    //Ejecuta botón btnDescargarZip que a su vez ejecuta su acción de cliente para ir a la clase FileDownloadHandlerMV
                    ScriptManager.RegisterStartupScript(this.Page, typeof(String), "", "<script> document.getElementById('btnZip').click(); </script> ", false);
                }
                else
                    AlertError("Debe seleccionar una manifestación de valor para descarga en zip");
            }
            catch(Exception ex)
            {
                AlertError(ex.Message);
                string mensaje = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "IA_ManifestacionValor-lkb_DescargarZip_Click", ex, lblCadena.Text, ref mensaje);
            }
        }

        //Método que limpia los filtros del grid
        protected void lkb_LimpiarFiltros_Click(object sender, EventArgs e)
        {
            foreach (GridViewColumn column in Grid_MV.Columns)
            {
                if (column is GridViewDataColumn)
                {
                    ((GridViewDataColumn)column).Settings.AutoFilterCondition = AutoFilterCondition.Default;
                    Grid_MV.AutoFilterByColumn(column, "");
                    Grid_MV.FilterExpression = String.Empty;
                    Grid_MV.ClearSort();
                }
            }
        }

        //Editar o eliminar pedimentos 
        protected void Grid_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            if (Session["Cadena"] == null)
            {
                string alerta = "<script>alert('La sesión ha expirado, debe volver a iniciar sesión');window.location.href='Login.aspx'</script>";
                Session["Tab"] = "Salir";
                Response.Write(alerta);
                Response.Redirect("Login.aspx", false);
                return;
            }

            //Llave de fila seleccionada
            Session["mvkeymanifestacion"] = Grid_MV.GetRowValues(e.VisibleIndex, "mvkeymanifestacion").ToString();

            try
            {
                if (!permisoEditar)
                {
                    AlertError("No tiene permiso para editar");
                    return;
                }

                if (!permisoEliminar)
                {
                    AlertError("No tiene permiso para eliminar");
                    return;
                }

                if (e.ButtonID == "btnEditar")
                {
                    //Limpiar sessiones
                    Session["Grid2_MV"] = null;
                    Session["Grid3_MV"] = null;
                    Session["Grid4_MV"] = null;
                    Session["Grid5_MV"] = null;
                    Session["Grid6_MV"] = null;
                    Session["Grid7_MV"] = null;
                    Session["Grid8_MV"] = null;
                    Session["Grid9_MV"] = null;
                    Session["pestaña"] = null;

                    //Abre el segundo view 
                    MultiView1.ActiveViewIndex = 1;
                    string v_pedimento = Grid_MV.GetRowValues(e.VisibleIndex, "pedimentoarmado").ToString();
                    lblTitPedimento.InnerText = "Pedimento: " + v_pedimento;
                    ASPxPageControl1.ActiveTabIndex = 0;

                    //Traer valores 
                    string mensaje = "";
                    DataTable dt = new DataTable();
                    // Opcion = 0, trae toda la manifestacion de valor relacionada con las 9 tablas
                    dt = informes.TraerMVParaEditar(v_pedimento, Int64.Parse(Session["mvkeymanifestacion"].ToString()), 0, Session["Cadena"].ToString(), ref mensaje);                    
                    
                    DataTable dts = new DataTable();

                    txt_Referencia.Text = dt != null ? dt.Rows[0]["Referencia"].ToString() : string.Empty;


                    //a Datos Generales
                    txt_a_Nombre.Text = dt != null ? dt.Rows[0]["a_nombre"].ToString() : string.Empty;

                    txt_a_NoExt.Text = dt != null ? dt.Rows[0]["a_numeroexterior"].ToString() : string.Empty;
                    txt_a_Calle.Text = dt != null ? dt.Rows[0]["a_calle"].ToString() : string.Empty;
                    txt_a_NoInt.Text = dt != null ? dt.Rows[0]["a_numerointerior"].ToString() : string.Empty;
                    txt_a_ciudad.Text = dt != null ? dt.Rows[0]["a_ciudad"].ToString() : string.Empty;
                    txt_a_codigo_postal.Text = dt != null ? dt.Rows[0]["a_codigopostal"].ToString() : string.Empty;
                    txt_a_Estado.Text = dt!= null ? dt.Rows[0]["a_estado"].ToString() : string.Empty;
                    txt_a_pais.Text = dt != null ? dt.Rows[0]["a_pais"].ToString() : string.Empty;
                    txt_a_telefono.Text = dt != null ? dt.Rows[0]["a_telefono"].ToString() : string.Empty;
                    txt_a_correo.Text = dt != null ? dt.Rows[0]["a_correo"].ToString() : string.Empty;



                    //b Agente o Apoderado Aduanal
                    if (dt != null && dt.Rows[0]["b_SIexistevinculacionimportadorvendedor"].ToString().ToUpper().Trim() == "X")
                    {
                        chkb1S.Checked = true;
                        chkb1N.Checked = false;
                    }
                    else
                    {
                        chkb1S.Checked = false;
                        chkb1N.Checked = true;
                    }
                    if (dt != null && dt.Rows[0]["b_SIinfluyovalordetransaccion"].ToString().ToUpper().Trim() == "X")
                    {
                        chkb2S.Checked = true;
                        chkb2N.Checked = false;
                    }
                    else
                    {
                        chkb2S.Checked = false;
                        chkb2N.Checked = true;
                    }

                    //c Datos del Importador
                    txt_c_nombre_importador.Text = dt != null ? dt.Rows[0]["c_nombreimportador"].ToString() : string.Empty;
                    txt_c_apellidopaternoimportador.Text = dt != null ?dt.Rows[0]["c_apellidopaternoimportador"].ToString() : string.Empty;
                    txt_c_apellidomaternoimportador.Text = dt != null ?dt.Rows[0]["c_apellidomaternoimportador"].ToString() : string.Empty;
                    txt_c_nombreimportador.Text = dt != null ? dt.Rows[0]["c_nombreimportador"].ToString() : string.Empty;
                    txt_c_rfcimportador.Text = dt != null ? dt.Rows[0]["c_rfcimportador"].ToString() : string.Empty;
                    txt_c_calleimportador.Text = dt != null ? dt.Rows[0]["c_calleimportador"].ToString() : string.Empty;
                    txt_c_numeroexteriorimportador.Text = dt != null ? dt.Rows[0]["c_numeroexteriorimportador"].ToString() : string.Empty;
                    txt_c_numerointeriorimportador.Text = dt != null ? dt.Rows[0]["c_numerointeriorimportador"].ToString() : string.Empty;
                    txt_c_ciudadimportador.Text = dt != null ? dt.Rows[0]["c_ciudadimportador"].ToString() : string.Empty;
                    txt_c_codigopostalimportador.Text = dt != null ? dt.Rows[0]["c_codigopostalimportador"].ToString() : string.Empty;
                    txt_c_estadoimportador.Text = dt != null ? dt.Rows[0]["c_estadoimportador"].ToString() : string.Empty;
                    txt_c_paisimportado.Text = dt != null ? dt.Rows[0]["c_paisimportador"].ToString() : string.Empty;
                    txt_c_telefonoimportador.Text = dt != null ? dt.Rows[0]["c_telefonoimportador"].ToString() : string.Empty;
                    txt_c_correoimportador.Text = dt != null ? dt.Rows[0]["c_correoimportador"].ToString() : string.Empty;

                    //d Agente o Apoderado Aduanal
                    txt_d_apellidopaternoagente.Text = dt != null ? dt.Rows[0]["d_apellidopaternoagente"].ToString() : string.Empty;
                    txt_d_apellidomaternoagente.Text = dt != null ? dt.Rows[0]["d_apellidomaternoagente"].ToString() : string.Empty;
                    txt_d_nombreagente.Text = dt != null ? dt.Rows[0]["d_nombreagente"].ToString() : string.Empty;
                    txt_d_patenteagente.Text = dt != null ? dt.Rows[0]["d_patenteagente"].ToString() : string.Empty;


                    // Facturas
                    mensaje = "";
                    //Opcion 1, trae la tabla [ManifestacionFactura]
                    dts = informes.TraerMVParaEditar(v_pedimento, Int64.Parse(Session["mvkeymanifestacion"].ToString()), 1, Session["Cadena"].ToString(), ref mensaje);
                    Grid2_MV.DataSource = Session["Grid2_MV"] = dts;
                    Grid2_MV.DataBind();
                    Grid2_MV.Settings.VerticalScrollableHeight = 168;
                    Grid2_MV.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                    //Grid2_MV.SettingsPager.PageSize = 20;

                    //Por default los botones de editar y eliminar están desactivados, si hay registros entra en el if para activarlos.
                    //if (Grid2_MV.VisibleRowCount > 0)
                    //{
                    //    string cledit = lkb_Factura_Editar.CssClass;

                    //    if (cledit.Contains("disabled"))
                    //    {
                    //        cledit = cledit.Replace("disabled", "");
                    //        lkb_Factura_Editar.CssClass = cledit;
                    //    }

                    //    //Habilitar botón de eliminar
                    //    string cldelete = lkb_Factura_Eliminar.CssClass;
                    //    if (cldelete.Contains("disabled"))
                    //    {
                    //        cldelete = cldelete.Replace("disabled", "");
                    //        lkb_Factura_Eliminar.CssClass = cldelete;
                    //    }
                    //}
                    

                    //f Método de Valoración
                    if (dt != null && dt.Rows[0]["f_unmetododevaloracion"].ToString().Trim().Length > 0)
                    {
                        chkf1S.Checked = true;
                        chkf2S.Checked = false;
                    }
                    else
                        chkf1S.Checked = false;

                    if (dt != null && dt.Rows[0]["f_masdeunmetododevaloracion"].ToString().Trim().Length > 0)
                    {
                        chkf1S.Checked = false;
                        chkf2S.Checked = true;
                    }
                    else
                        chkf2S.Checked = false;
                    

                    if (dt != null && dt.Rows[0]["f_valordetransaccionmercancias"].ToString().Trim().Length > 0)
                    {
                        chkf3S.Checked = true;
                    }
                    else
                        chkf3S.Checked = false;

                    if (dt != null && dt.Rows[0]["f_descripciondetransaccionmercancias"].ToString().Trim().Length > 0)
                    {
                        txt_f_descripciondetransaccionmercancias.Text = dt.Rows[0]["f_descripciondetransaccionmercancias"].ToString();
                    }
                    else
                        txt_f_descripciondetransaccionmercancias.Text = string.Empty;

                    if (dt != null && dt.Rows[0]["f_valordetransaccionmercanciasidenticas"].ToString().Trim().Length > 0)
                    {
                        chkf4S.Checked = true;
                    }
                    else
                        chkf4S.Checked = false;

                    if (dt != null && dt.Rows[0]["f_descripciondetransaccionmercanciasidenticas"].ToString().Trim().Length > 0)
                    {
                        txt_f_descripciondetransaccionmercanciasidenticas.Text = dt.Rows[0]["f_descripciondetransaccionmercanciasidenticas"].ToString();
                    }
                    else
                        txt_f_descripciondetransaccionmercanciasidenticas.Text = string.Empty;

                    if (dt != null && dt.Rows[0]["f_valordetransaccionmercanciassimilares"].ToString().Trim().Length > 0)
                    {
                        chkf5S.Checked = true;
                    }
                    else
                        chkf5S.Checked = false;

                    if (dt != null && dt.Rows[0]["f_descripciondetransaccionmercanciassimilares"].ToString().Trim().Length > 0)
                    {
                        txt_f_descripciondetransaccionmercanciassimilares.Text = dt.Rows[0]["f_descripciondetransaccionmercanciassimilares"].ToString();
                    }
                    else
                        txt_f_descripciondetransaccionmercanciassimilares.Text = string.Empty;


                    if (dt != null && dt.Rows[0]["f_valordepreciounitariodeventa"].ToString().Trim().Length > 0)
                    {
                        chkf6S.Checked = true;
                    }
                    else
                        chkf6S.Checked = false;

                    if (dt != null && dt.Rows[0]["f_descripcionvalordepreciounitariodeventa"].ToString().Trim().Length > 0)
                    {
                        txt_f_descripcionvalordepreciounitariodeventa.Text = dt.Rows[0]["f_descripcionvalordepreciounitariodeventa"].ToString();
                    }
                    else
                        txt_f_descripcionvalordepreciounitariodeventa.Text = string.Empty;


                    if (dt != null && dt.Rows[0]["f_valorreconstruido"].ToString().Trim().Length > 0)
                    {
                        chkf7S.Checked = true;
                    }
                    else
                        chkf7S.Checked = false;

                    if (dt != null && dt.Rows[0]["f_descripcionvalorreconstruido"].ToString().Trim().Length > 0)
                    {
                        txt_f_descripcionvalorreconstruido.Text = dt.Rows[0]["f_descripcionvalorreconstruido"].ToString();
                    }
                    else
                        txt_f_descripcionvalorreconstruido.Text = string.Empty;


                    if (dt != null && dt.Rows[0]["f_valorconformaley"].ToString().Trim().Length > 0)
                    {
                        chkf8S.Checked = true;
                    }
                    else
                        chkf8S.Checked = false;

                    if (dt != null && dt.Rows[0]["f_descripcionvalorconformaley"].ToString().Trim().Length > 0)
                    {
                        txt_f_descripcionvalorconformaley.Text = dt.Rows[0]["f_descripcionvalorconformaley"].ToString();
                    }
                    else
                        txt_f_descripcionvalorconformaley.Text = string.Empty;


                    //Manda a llamar funcion Get_chkf1S
                    ScriptManager.RegisterStartupScript(this.Page, typeof(String), "", "<script> Get_chkfS(); </script> ", false);


                    //g Anexos
                    if (dt != null && dt.Rows[0]["g_anexadocumentacion"].ToString().Trim().Length > 0)
                    {
                        chkg1S.Checked = true;
                    }
                    else
                    {
                        chkg1S.Checked = false;
                    }

                    if (dt != null && dt.Rows[0]["g_anexadocumentacionnumeroyletra"].ToString().Trim().Length > 0)
                    {
                        txt_g_anexadocumentacionnumeroyletra.Text = dt.Rows[0]["g_anexadocumentacionnumeroyletra"].ToString().Trim();
                    }
                    else
                        txt_g_anexadocumentacionnumeroyletra.Text = string.Empty;

                    if (dt != null && dt.Rows[0]["g_anexadocumentacionmodena"].ToString().Trim().Length > 0)
                    {
                        txt_g_anexadocumentacionmodena.Text = dt.Rows[0]["g_anexadocumentacionmodena"].ToString().Trim();
                    }
                    else
                        txt_g_anexadocumentacionmodena.Text = string.Empty;
                    


                    //VALOR DE TRANSACCION DE LAS MERCANCIAS

                    //ga a) En caso de utilizar el valor de transacción de las mercancias indicar lo siguiente
                    if (dt != null && dt.Rows[0]["ga_preciofacturanumero"].ToString().Trim().Length > 0)
                    {
                        se_ga_preciofacturanumero.Text = dt.Rows[0]["ga_preciofacturanumero"].ToString().Trim();                       
                    }
                    else
                        se_ga_preciofacturanumero.Value = 0;


                    if (dt != null && dt.Rows[0]["ga_preciofacturaletra"].ToString().Trim().Length > 0)
                    {
                        txt_ga_preciofacturaletra.Text = dt.Rows[0]["ga_preciofacturaletra"].ToString().Trim();
                    }
                    else
                        txt_ga_preciofacturaletra.Text = string.Empty;

                    //b) Información conforme al artículo 66 de la Ley (conceptos que no integran el valor de transacción)
                    if (dt != null && dt.Rows[0]["gb_precioprevistofactura"].ToString().Trim().Length > 0)
                    {
                        chkvtm_b1S.Checked = true;
                    }
                    else
                    {
                        chkvtm_b1S.Checked = false;
                    }

                    if (dt != null && dt.Rows[0]["gb_otrosdocumentosanexados"].ToString().Trim().Length > 0)
                    {
                        chkvtm_b2S.Checked = true;
                    }
                    else
                    {
                        chkvtm_b2S.Checked = false;
                    }

                    if (dt != null && dt.Rows[0]["gb_conceptosenlaley"].ToString().Trim().Length > 0)
                    {
                        chkvtm_b3S.Checked = true;
                    }
                    else
                    {
                        chkvtm_b3S.Checked = false;
                    }

                    if (dt != null && dt.Rows[0]["gb_conceptosenlaleydesglozadosenfactura"].ToString().Trim().Length > 0)
                    {
                        chkvtm_b4S.Checked = true;
                    }
                    else
                    {
                        chkvtm_b4S.Checked = false;
                    }

                    // [ManifestacionAnexo]
                    mensaje = "";
                    dts = new DataTable();
                    //Opcion 2, trae la tabla [ManifestacionAnexo]
                    dts = informes.TraerMVParaEditar(v_pedimento, Int64.Parse(Session["mvkeymanifestacion"].ToString()), 2, Session["Cadena"].ToString(), ref mensaje);
                    Grid3_MV.DataSource = Session["Grid3_MV"] = dts;
                    Grid3_MV.DataBind();
                    Grid3_MV.Settings.VerticalScrollableHeight = 168;
                    Grid3_MV.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                    //Grid3_MV.SettingsPager.PageSize = 20;

                    //d) Indicar con la " X" en caso de NO ANEXAR documentación y sólo describirán los conceptos previstos en el artículo 66 de la Ley.
                    if (dt != null && dt.Rows[0]["gd_NOanexadocumentacion"].ToString().Trim().Length > 0)
                    {
                        chkvtm_c1S.Checked = true;
                    }
                    else
                    {
                        chkvtm_c1S.Checked = false;
                    }

                    // [ManifestacionConcepto66]
                    mensaje = "";
                    dts = new DataTable();
                    //Opcion 3, trae la tabla [ManifestacionConcepto66]
                    dts = informes.TraerMVParaEditar(v_pedimento, Int64.Parse(Session["mvkeymanifestacion"].ToString()), 3, Session["Cadena"].ToString(), ref mensaje);
                    Grid4_MV.DataSource = Session["Grid4_MV"] = dts;
                    Grid4_MV.DataBind();
                    Grid4_MV.Settings.VerticalScrollableHeight = 168;
                    Grid4_MV.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                    //Grid4_MV.SettingsPager.PageSize = 20;

                    //El importador debe señalar si existen cargos conforme al artículo 65 de la Ley. Señale con una "X " si el precio pagado por las mercancías importadas comprende el importe de los conceptos señalados en el artículo 65 de la Ley.
                    if (dt != null && dt.Rows[0]["gd_SIimportedeacuerdoalaley"].ToString().Trim().Length > 0)
                    {
                        chk65_1S.Checked = true;
                    }
                    else
                    {
                        chk65_1S.Checked = false;
                    }

                    if (dt != null && dt.Rows[0]["gd_NOimportedeacuerdoalaley"].ToString().Trim().Length > 0)
                    {
                        chk65_1N.Checked = true;
                    }
                    else
                    {
                        chk65_1N.Checked = false;
                    }

                    //En su caso, señale si el importador opta por acompañar o NO las facturas y otros documentos a su manifestación de valor.
                    if (dt != null && dt.Rows[0]["gd_SIotrosdocumentosimportedeacuerdoalaley"].ToString().Trim().Length > 0)
                    {               
                        chk65_2S.Checked = true;
                    }
                    else
                    {
                        chk65_2S.Checked = false;
                    }

                    if (dt != null && dt.Rows[0]["gd_NOotrosdocumentosimportedeacuerdoalaley"].ToString().Trim().Length > 0)
                    {
                        chk65_2N.Checked = true;
                    }
                    else
                    {
                        chk65_2N.Checked = false;
                    }

                    //Indicar si ANEXA documentación
                    if (dt != null && dt.Rows[0]["gda_anexadocumentacion"].ToString().Trim().Length > 0)
                    {
                        chk65_3S.Checked = true;
                    }
                    else
                    {
                        chk65_3S.Checked = false;
                    }


                    // [ManifestacionAnexo65]
                    mensaje = "";
                    dts = new DataTable();
                    //Opcion 7, trae la tabla [ManifestacionAnexo65]
                    dts = informes.TraerMVParaEditar(v_pedimento, Int64.Parse(Session["mvkeymanifestacion"].ToString()), 7, Session["Cadena"].ToString(), ref mensaje);
                    Grid8_MV.DataSource = Session["Grid8_MV"] = dts;
                    Grid8_MV.DataBind();
                    Grid8_MV.Settings.VerticalScrollableHeight = 168;
                    Grid8_MV.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                    Grid8_MV.SettingsPager.PageSize = 20;


                    if (dt != null && dt.Rows[0]["gd65_NOanexadocumentacion"].ToString().Trim().Length > 0)
                    {
                        chk65_3N.Checked = true;
                    }
                    else
                    {
                        chk65_3N.Checked = false;
                    }


                    // [ManifestacionConcepto65]
                    mensaje = "";
                    dts = new DataTable();
                    //Opcion 4, trae la tabla [ManifestacionConcepto65]
                    dts = informes.TraerMVParaEditar(v_pedimento, Int64.Parse(Session["mvkeymanifestacion"].ToString()), 4, Session["Cadena"].ToString(), ref mensaje);
                    Grid5_MV.DataSource = Session["Grid5_MV"] = dts;
                    Grid5_MV.DataBind();
                    Grid5_MV.Settings.VerticalScrollableHeight = 168;
                    Grid5_MV.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                    Grid5_MV.SettingsPager.PageSize = 20;

                    //Otros Artículos conforme a la Ley
                    //a) La base gravable deriva de una compraventa para la exportación con destino a territorio nacional.
                    if (dt != null && dt.Rows[0]["gda_SIderivadecompraventa"].ToString().Trim().Length > 0)
                    {
                        chkOtros_1S.Checked = true;
                    }
                    else
                    {
                        chkOtros_1S.Checked = false;
                    }

                    if (dt != null && dt.Rows[0]["gda_NOderivadecompraventa"].ToString().Trim().Length > 0)
                    {
                        chkOtros_1N.Checked = true;
                    }
                    else
                    {
                        chkOtros_1N.Checked = false;
                    }

                    // [ManifestacionConcepto67]
                    mensaje = "";
                    dts = new DataTable();
                    //Opcion 5, trae la tabla [ManifestacionConcepto67]
                    dts = informes.TraerMVParaEditar(v_pedimento, Int64.Parse(Session["mvkeymanifestacion"].ToString()), 5, Session["Cadena"].ToString(), ref mensaje);
                    Grid6_MV.DataSource = Session["Grid6_MV"] = dts;
                    Grid6_MV.DataBind();
                    Grid6_MV.Settings.VerticalScrollableHeight = 168;
                    Grid6_MV.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                    Grid6_MV.SettingsPager.PageSize = 20;

                    //b) Si existe alguna circunstancia distinta de las previstas en los artículos 67 y 71 de la Ley que impida utilizar el valor de transacción, lo señalará a continuación :
                    //Señale si optará por acompañar los documentos en los que conste dicho valor en aduana.
                    if (dt != null && dt.Rows[0]["gda_SIanexadocumentovaloraduana"].ToString().Trim().Length > 0)
                    {               
                        chkOtros_2S.Checked = true;
                    }
                    else
                    {
                        chkOtros_2S.Checked = false;
                    }

                    if (dt != null && dt.Rows[0]["gda_NOanexadocumentovaloraduana"].ToString().Trim().Length > 0)
                    {
                        chkOtros_2N.Checked = true;
                    }
                    else
                    {
                        chkOtros_2N.Checked = false;
                    }

                    // [ManifestacionValorAduana]
                    mensaje = "";
                    dts = new DataTable();
                    //Opcion 6, trae la tabla [ManifestacionValorAduana]
                    dts = informes.TraerMVParaEditar(v_pedimento, Int64.Parse(Session["mvkeymanifestacion"].ToString()), 6, Session["Cadena"].ToString(), ref mensaje);
                    Grid7_MV.DataSource = Session["Grid7_MV"] = dts;
                    Grid7_MV.DataBind();
                    Grid7_MV.Settings.VerticalScrollableHeight = 168;
                    Grid7_MV.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                    Grid7_MV.SettingsPager.PageSize = 20;
                    

                    //Importación Temporal
                    //El valor determinado por las mercancías es provisional.
                    if (dt != null && dt.Rows[0]["gdb_SIvalordeterminadopormercaciaprovicional"].ToString().Trim().Length > 0)
                    {
                        chkIT_1S.Checked = true;
                    }
                    else
                    {
                        chkIT_1S.Checked = false;
                    }

                    if (dt != null && dt.Rows[0]["gdb_NOvalordeterminadopormercaciaprovicional"].ToString().Trim().Length > 0)
                    {
                        chkIT_1N.Checked = true;
                    }
                    else
                    {
                        chkIT_1N.Checked = false;
                    }

                    if (dt != null && dt.Rows[0]["gdb_SIdocumentacionvalormercancia"].ToString().Trim().Length > 0)
                    {
                        chkIT_2S.Checked = true;
                    }
                    else
                    {
                        chkIT_2S.Checked = false;
                    }

                    if (dt != null && dt.Rows[0]["gdb_NOdocumentacionvalormercancia"].ToString().Trim().Length > 0)
                    {
                        chkIT_2N.Checked = true;
                    }
                    else
                    {
                        chkIT_2N.Checked = false;
                    }

                    // [ManifestacionMercanciaProvisional]
                    mensaje = "";
                    dts = new DataTable();
                    //Opcion 8, trae la tabla [ManifestacionMercanciaProvisional]
                    dts = informes.TraerMVParaEditar(v_pedimento, Int64.Parse(Session["mvkeymanifestacion"].ToString()), 8, Session["Cadena"].ToString(), ref mensaje);
                    Grid9_MV.DataSource = Session["Grid9_MV"] = dts;
                    Grid9_MV.DataBind();
                    Grid9_MV.Settings.VerticalScrollableHeight = 168;
                    Grid9_MV.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                    Grid9_MV.SettingsPager.PageSize = 20;


                    // Periocidad de la Manifestación de Valor
                    if (dt != null && dt.Rows[0]["gdb_poroperacionimportaciontemporal"].ToString().Trim().Length > 0)
                    {
                        chkPMV_1S.Checked = true;
                    }
                    else
                    {
                        chkPMV_1S.Checked = false;
                    }

                    if (dt != null && dt.Rows[0]["gdb_poroperiodoimportaciontemporal"].ToString().Trim().Length > 0)
                    {
                        chkPMV_1N.Checked = true;
                    }
                    else
                    {
                        chkPMV_1N.Checked = false;
                    }

                    if (dt != null && dt.Rows[0]["gdb_rfcmanifestacion"].ToString().Trim().Length > 0)
                    {
                        txt_gdb_rfcmanifestacion.Text = dt.Rows[0]["gdb_rfcmanifestacion"].ToString().Trim();
                    }
                    else
                    {
                        txt_gdb_rfcmanifestacion.Text = string.Empty;
                    }

                    if (dt != null && dt.Rows[0]["MV_FECHAPEDIMENTO"].ToString().Trim().Length > 0)
                    {
                        if (dt.Rows[0]["MV_FECHAPEDIMENTO"].ToString().Trim().Length > 10)
                            date_MV_FECHAPEDIMENTO.Text = dt.Rows[0]["MV_FECHAPEDIMENTO"].ToString().Substring(0, 10);
                        else
                            date_MV_FECHAPEDIMENTO.Text = dt.Rows[0]["MV_FECHAPEDIMENTO"].ToString();
                    }
                    else
                    {
                        date_MV_FECHAPEDIMENTO.Text = string.Empty;
                    }

                    if (dt != null && dt.Rows[0]["gdb_nombremanifestacion"].ToString().Trim().Length > 0)
                    {
                        txt_gdb_nombremanifestacion.Text = dt.Rows[0]["gdb_nombremanifestacion"].ToString().Trim();
                    }
                    else
                    {
                        txt_gdb_nombremanifestacion.Text = string.Empty;
                    }

                    Session["REPRESENTANTEKEY"] = null;
                    if (dt != null && dt.Rows[0]["REPRESENTANTEKEY"].ToString().Trim().Length > 0)
                    {
                        Session["REPRESENTANTEKEY"] = int.Parse(dt.Rows[0]["REPRESENTANTEKEY"].ToString().Trim());
                        if (dt.Rows[0]["MuestraFirma"].ToString().Trim().Length > 0 && dt.Rows[0]["MuestraFirma"].ToString().Trim().ToUpper().Contains("TRUE"))
                            cbx_MostrarFirma.Checked = true;
                        else
                            cbx_MostrarFirma.Checked = false;
                    }
                    else
                        cbx_MostrarFirma.Checked = false;

                    if (dt != null && dt.Rows[0]["FIRMAIMAGE"].ToString().Trim().Length > 0)
                        ASPxBinaryImage.Value = (byte[])dt.Rows[0]["FIRMAIMAGE"];
                    else
                        ASPxBinaryImage.Value = null;




                    ////////////////////////////////////////////////////////////////////////////////////////////////
                    //Validar si se habilita o no la pestaña de Articulo 65 Ley

                    int bandera = 0;

                    //VALOR DE TRANSACCION DE LAS MERCANCIAS

                    if (decimal.Parse(se_ga_preciofacturanumero.Text) > 0)
                        bandera = 1;

                    if(txt_ga_preciofacturaletra.Text.Trim().Length > 0)
                        bandera = 1;

                    if (txt_g_anexadocumentacionmodena.Text.Trim().Length > 0)
                        bandera = 1;                    

                    if (chkvtm_b1S.Checked)
                        bandera = 1;

                    if (chkvtm_b2S.Checked)
                        bandera = 1;

                    if (chkvtm_b3S.Checked)
                        bandera = 1;

                    if (chkvtm_b4S.Checked)
                        bandera = 1;

                    if (Grid3_MV.VisibleRowCount > 0)
                        bandera = 1;

                    if (chkvtm_c1S.Checked)
                        bandera = 1;

                    if (Grid4_MV.VisibleRowCount > 0)
                        bandera = 1;

                    
                    //Valida bandera
                    if(bandera.Equals(1))
                    {
                        chk65_1S.Enabled = false;
                        chk65_1N.Enabled = false;
                        chk65_2S.Enabled = false;
                        chk65_2N.Enabled = false;
                        chk65_3S.Enabled = false;
                        lkb_OrdenAnexo65_Agregar.Enabled = false;
                        chk65_3N.Enabled = false;
                        lkb_ManifestacionConcepto65_Agregar.Enabled = false;
                    }
                    else
                    {
                        chk65_1S.Enabled = true;
                        chk65_1N.Enabled = true;
                        chk65_2S.Enabled = true;
                        chk65_2N.Enabled = true;
                        chk65_3S.Enabled = true;
                        lkb_OrdenAnexo65_Agregar.Enabled = true;
                        chk65_3N.Enabled = true;
                        lkb_ManifestacionConcepto65_Agregar.Enabled = true;
                    }

                }
                else if (e.ButtonID == "btnEliminar")
                {
                    //Modal Question
                    string valida = "¿Desea eliminar este registro?";                    
                    AlertQuestion(valida);
                }
            }
            catch (Exception ex)
            {
                AlertError(ex.Message);
            }
        }

        //Elimina registro en Grid
        protected void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["Cadena"] == null || Session["mvkeymanifestacion"] == null)
                {
                    string alerta = "<script>alert('La sesión ha expirado, debe volver a iniciar sesión');window.location.href='Login.aspx'</script>";
                    Session["Tab"] = "Salir";
                    Response.Write(alerta);
                    Response.Redirect("Login.aspx", false);
                    return;
                }

                string mensaje = "";
                DataTable dt = new DataTable();

                //Eliminar
                dt = informes.EliminaMV(Session["mvkeymanifestacion"].ToString(), Session["Cadena"].ToString(), ref mensaje);

                lkb_LimpiarFiltros_Click(null, null);

                //Cargar de nuevo MV

                Grid_MV.DataSource = Session["Grid_MV"] = dt;
                Grid_MV.DataBind();
                Grid_MV.Settings.VerticalScrollableHeight = 260;
                Grid_MV.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                Grid_MV.SettingsPager.PageSize = 20;

            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                //excepcion.RegistrarExcepcion(idusuario, "IA_ManifestacionValor-btnOk_Click", ex, Session["Cadena"].ToString(), ref mensaje);
            }
        }

        //Botón Regresar
        protected void lkb_Regresar_Click(object sender, EventArgs e)
        {
            Session["pestaña"] = null;
            MultiView1.ActiveViewIndex = 0;
        }

        //Botón Cancelar
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 0;
        }

        //Botón Guardar al Editar
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["Cadena"] == null || Session["mvkeymanifestacion"] == null)
                {
                    string alerta = "<script>alert('La sesión ha expirado, debe volver a iniciar sesión');window.location.href='Login.aspx'</script>";
                    Session["Tab"] = "Salir";
                    Response.Write(alerta);
                    Response.Redirect("Login.aspx", false);
                    return;
                }

                string mensaje = "";
                DataTable dt = new DataTable();
               

                //Declaración de variables para validar
                string v_chkb1S = string.Empty;
                string v_chkb1N = string.Empty;
                string v_chkb2S = string.Empty;
                string v_chkb2N = string.Empty;

                string v_chkf1S = string.Empty;
                string v_chkf2S = string.Empty;
                string v_chkf3S = string.Empty;
                string v_chkf4S = string.Empty;
                string v_chkf5S = string.Empty;
                string v_chkf6S = string.Empty;
                string v_chkf7S = string.Empty;
                string v_chkf8S = string.Empty;

                string v_chkg1S = string.Empty;
                
                decimal v_se_ga_preciofacturanumero = 0;

                string v_chkvtm_b1S = string.Empty;
                string v_chkvtm_b2S = string.Empty;
                string v_chkvtm_b3S = string.Empty;
                string v_chkvtm_b4S = string.Empty;

                string v_chkvtm_c1S = string.Empty;

                string v_chk65_1S = string.Empty;
                string v_chk65_1N = string.Empty;
                string v_chk65_2S = string.Empty;
                string v_chk65_2N = string.Empty;
                string v_chk65_3S = string.Empty;
                string v_chk65_3N = string.Empty;

                string v_chkOtros_1S = string.Empty;
                string v_chkOtros_1N = string.Empty;
                string v_chkOtros_2S = string.Empty;
                string v_chkOtros_2N = string.Empty;

                string v_chkIT_1S = string.Empty;
                string v_chkIT_1N = string.Empty;
                string v_chkIT_2S = string.Empty;
                string v_chkIT_2N = string.Empty;

                string v_chkPMV_1S = string.Empty;
                string v_chkPMV_1N = string.Empty;


                //Validaciones
                if (chkb1S.Checked) v_chkb1S = "X"; else v_chkb1S = string.Empty;
                if (chkb1N.Checked) v_chkb1N = "X"; else v_chkb1N = string.Empty;
                if (chkb2S.Checked) v_chkb2S = "X"; else v_chkb2S = string.Empty;
                if (chkb2N.Checked) v_chkb2N = "X"; else v_chkb2N = string.Empty;

                if (chkf1S.Checked) v_chkf1S = "X"; else v_chkf1S = string.Empty;
                if (chkf2S.Checked) v_chkf2S = "X"; else v_chkf2S = string.Empty;
                if (chkf3S.Checked) v_chkf3S = "X"; else v_chkf3S = string.Empty;
                if (chkf4S.Checked) v_chkf4S = "X"; else v_chkf4S = string.Empty;
                if (chkf5S.Checked) v_chkf5S = "X"; else v_chkf5S = string.Empty;
                if (chkf6S.Checked) v_chkf6S = "X"; else v_chkf6S = string.Empty;
                if (chkf7S.Checked) v_chkf7S = "X"; else v_chkf7S = string.Empty;
                if (chkf8S.Checked) v_chkf8S = "X"; else v_chkf8S = string.Empty;

                if (chkg1S.Checked) v_chkg1S = "X"; else v_chkg1S = string.Empty;

                v_se_ga_preciofacturanumero = (se_ga_preciofacturanumero.Text != null && se_ga_preciofacturanumero.Text.Length > 0) ? decimal.Parse(se_ga_preciofacturanumero.Text) : 0;

                if (chkvtm_b1S.Checked) v_chkvtm_b1S = "X"; else v_chkvtm_b1S = string.Empty;
                if (chkvtm_b2S.Checked) v_chkvtm_b2S = "X"; else v_chkvtm_b2S = string.Empty;
                if (chkvtm_b3S.Checked) v_chkvtm_b3S = "X"; else v_chkvtm_b3S = string.Empty;
                if (chkvtm_b4S.Checked) v_chkvtm_b4S = "X"; else v_chkvtm_b4S = string.Empty;

                if (chkvtm_c1S.Checked) v_chkvtm_c1S = "X"; else v_chkvtm_c1S = string.Empty;

                if (chk65_1S.Checked) v_chk65_1S = "X"; else v_chk65_1S = string.Empty;
                if (chk65_1N.Checked) v_chk65_1N = "X"; else v_chk65_1N = string.Empty;
                if (chk65_2S.Checked) v_chk65_2S = "X"; else v_chk65_2S = string.Empty;
                if (chk65_2N.Checked) v_chk65_2N = "X"; else v_chk65_2N = string.Empty;
                if (chk65_3S.Checked) v_chk65_3S = "X"; else v_chk65_3S = string.Empty;
                if (chk65_3N.Checked) v_chk65_3N = "X"; else v_chk65_3N = string.Empty;

                if (chkOtros_1S.Checked) v_chkOtros_1S = "X"; else v_chkOtros_1S = string.Empty;
                if (chkOtros_1N.Checked) v_chkOtros_1N = "X"; else v_chkOtros_1N = string.Empty;
                if (chkOtros_2S.Checked) v_chkOtros_2S = "X"; else v_chkOtros_2S = string.Empty;
                if (chkOtros_2N.Checked) v_chkOtros_2N = "X"; else v_chkOtros_2N = string.Empty;

                if (chkIT_1S.Checked) v_chkIT_1S = "X"; else v_chkIT_1S = string.Empty;
                if (chkIT_1N.Checked) v_chkIT_1N = "X"; else v_chkIT_1N = string.Empty;
                if (chkIT_2S.Checked) v_chkIT_2S = "X"; else v_chkIT_2S = string.Empty;
                if (chkIT_2N.Checked) v_chkIT_2N = "X"; else v_chkIT_2N = string.Empty;

                DateTime? v_date_MV_FECHAPEDIMENTO = string.IsNullOrEmpty(date_MV_FECHAPEDIMENTO.Text) ? (DateTime?)null : DateTime.Parse(date_MV_FECHAPEDIMENTO.Text);

                if (chkPMV_1S.Checked) v_chkPMV_1S = "X"; else v_chkPMV_1S = string.Empty;
                if (chkPMV_1N.Checked) v_chkPMV_1N = "X"; else v_chkPMV_1N = string.Empty;


                //Mostrar Firma
                int muestraFirma = 1;

                if (!cbx_MostrarFirma.Checked)
                    muestraFirma = 0;

                //Obtener valor de Representantekey
                int repkey = 0;
                if (Session["REPRESENTANTEKEY"] != null && Session["REPRESENTANTEKEY"].ToString().Trim().Length > 0)
                    repkey = int.Parse(Session["REPRESENTANTEKEY"].ToString().Trim());

                //
                //GUARDAR
                //

                //ManifestacionDeValor
                dt = informes.Actualiza_ManifestacionDeValor( Int64.Parse(Session["mvkeymanifestacion"].ToString()), txt_Referencia.Text.Trim(), txt_a_Nombre.Text.Trim(),
                    txt_a_Calle.Text.Trim(), txt_a_NoExt.Text.Trim(), txt_a_NoInt.Text.Trim(), txt_a_ciudad.Text.Trim(), txt_a_codigo_postal.Text.Trim(), txt_a_Estado.Text.Trim(),
                    txt_a_pais.Text.Trim(),txt_a_telefono.Text.Trim(), txt_a_correo.Text.Trim(), v_chkb1S,v_chkb1N, v_chkb2S, v_chkb2N, txt_c_nombreimportador.Text.Trim(),
                    txt_c_apellidopaternoimportador.Text.Trim(), txt_c_apellidomaternoimportador.Text.Trim(), txt_c_rfcimportador.Text.Trim(), txt_c_numeroexteriorimportador.Text.Trim(),
                    txt_c_numerointeriorimportador.Text.Trim(), txt_c_ciudadimportador.Text.Trim(), txt_c_codigopostalimportador.Text.Trim(), txt_c_estadoimportador.Text.Trim(),
                    txt_c_paisimportado.Text.Trim(), txt_c_telefonoimportador.Text.Trim(), txt_c_correoimportador.Text.Trim(), txt_d_apellidopaternoagente.Text.Trim(),
                    txt_d_apellidomaternoagente.Text.Trim(), txt_d_nombreagente.Text.Trim(), txt_d_patenteagente.Text.Trim(), v_chkf1S, v_chkf2S, v_chkf3S, txt_f_descripciondetransaccionmercancias.Text.Trim(),
                    v_chkf4S, txt_f_descripciondetransaccionmercanciasidenticas.Text.Trim(), v_chkf5S, txt_f_descripciondetransaccionmercanciassimilares.Text.Trim(), v_chkf6S,
                    txt_f_descripcionvalordepreciounitariodeventa.Text.Trim(), v_chkf7S, txt_f_descripcionvalorreconstruido.Text.Trim(), v_chkf8S, txt_f_descripcionvalorconformaley.Text.Trim(),
                    v_chkg1S, txt_g_anexadocumentacionnumeroyletra.Text.Trim(), txt_g_anexadocumentacionmodena.Text.Trim(), v_se_ga_preciofacturanumero, txt_ga_preciofacturaletra.Text.Trim(), v_chkvtm_b1S, v_chkvtm_b2S, v_chkvtm_b3S, v_chkvtm_b4S,
                    v_chkvtm_c1S, v_chk65_1S, v_chk65_1N, v_chk65_2S, v_chk65_2N, v_chk65_3S, v_chk65_3N, v_chkOtros_1S, v_chkOtros_1N, v_chkOtros_2S, v_chkOtros_2N, v_chkIT_1S, v_chkIT_1N, v_chkIT_2S,
                    v_chkIT_2N, v_chkPMV_1S, v_chkPMV_1N, txt_gdb_rfcmanifestacion.Text.Trim(), v_date_MV_FECHAPEDIMENTO, txt_gdb_nombremanifestacion.Text.Trim(), repkey, muestraFirma,
                    Session["Cadena"].ToString(), ref mensaje);                                                                                   

                lkb_LimpiarFiltros_Click(null, null);

                //Cargar 
                Grid_MV.DataSource = Session["Grid_MV"] = dt;
                Grid_MV.DataBind();
                Grid_MV.Settings.VerticalScrollableHeight = 260;
                Grid_MV.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                Grid_MV.SettingsPager.PageSize = 20;



                //Se crea una variable para que obtenga la cadena de conexión
                string connString = Session["Cadena"].ToString();
                string cmdString = string.Empty;



                //Datos Factura

                if (Session["Grid2_MV"] != null)
                {
                    //se limpia tabla ManifestacionFactura para agregar información cambiada
                    dt = new DataTable();

                    cmdString = "DELETE FROM ManifestacionFactura WHERE mvkeymanifestacion = " + Session["mvkeymanifestacion"].ToString();

                    try
                    {
                        using (SqlConnection conn = new SqlConnection(connString))
                        {
                            conn.Open();
                            using (SqlCommand comm = new SqlCommand())
                            {
                                comm.Connection = conn;
                                comm.CommandText = cmdString;
                                using (SqlDataReader reader = comm.ExecuteReader())
                                {
                                    if (!reader.HasRows)
                                        dt = null;
                                    else
                                        dt.Load(reader);
                                }
                            }
                            conn.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        AlertError(ex.Message + " Error en BD en tabla ManifestacionFactura");
                    }

                    //Hacemos un bulk a la tabla ManifestacionFactura                                
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(Session["Cadena"].ToString()))
                    {

                        foreach (DataColumn col in ((DataTable)Session["Grid2_MV"]).Columns)
                        {
                            bulkCopy.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                        }


                        bulkCopy.DestinationTableName = "dbo.ManifestacionFactura";

                        try
                        {
                            bulkCopy.WriteToServer(((DataTable)Session["Grid2_MV"]));
                        }
                        catch (Exception ex)
                        {
                            AlertError(ex.Message);
                        }
                    }
                }

                //Valor de Transacción de las Mercancias

                if (Session["Grid3_MV"] != null)
                {
                    //TM
                    //se limpia tabla ManifestacionAnexo para agregar información cambiada
                    dt = new DataTable();

                    cmdString = "DELETE FROM ManifestacionAnexo WHERE mvkeymanifestacion = " + Session["mvkeymanifestacion"].ToString();

                    try
                    {
                        using (SqlConnection conn = new SqlConnection(connString))
                        {
                            conn.Open();
                            using (SqlCommand comm = new SqlCommand())
                            {
                                comm.Connection = conn;
                                comm.CommandText = cmdString;
                                using (SqlDataReader reader = comm.ExecuteReader())
                                {
                                    if (!reader.HasRows)
                                        dt = null;
                                    else
                                        dt.Load(reader);
                                }
                            }
                            conn.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        AlertError(ex.Message + " Error en BD en tabla ManifestacionAnexo");
                    }

                    //Hacemos un bulk a la tabla ManifestacionAnexo                                
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(Session["Cadena"].ToString()))
                    {
                        foreach (DataColumn col in ((DataTable)Session["Grid3_MV"]).Columns)
                        {
                            bulkCopy.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                        }


                        bulkCopy.DestinationTableName = "dbo.ManifestacionAnexo";

                        try
                        {
                            bulkCopy.WriteToServer(((DataTable)Session["Grid3_MV"]));
                        }
                        catch (Exception ex)
                        {
                            AlertError(ex.Message);
                        }
                    }
                }


                //TM2
                //se limpia tabla ManifestacionConcepto66 para agregar información cambiada
                if (Session["Grid4_MV"] != null)
                {
                    dt = new DataTable();

                    cmdString = "DELETE FROM ManifestacionConcepto66 WHERE mvkeymanifestacion = " + Session["mvkeymanifestacion"].ToString();

                    try
                    {
                        using (SqlConnection conn = new SqlConnection(connString))
                        {
                            conn.Open();
                            using (SqlCommand comm = new SqlCommand())
                            {
                                comm.Connection = conn;
                                comm.CommandText = cmdString;
                                using (SqlDataReader reader = comm.ExecuteReader())
                                {
                                    if (!reader.HasRows)
                                        dt = null;
                                    else
                                        dt.Load(reader);
                                }
                            }
                            conn.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        AlertError(ex.Message + " Error en BD en tabla ManifestacionConcepto66");
                    }

                    //Hacemos un bulk a la tabla ManifestacionConcepto66                                
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(Session["Cadena"].ToString()))
                    {
                        foreach (DataColumn col in ((DataTable)Session["Grid4_MV"]).Columns)
                        {
                            bulkCopy.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                        }


                        bulkCopy.DestinationTableName = "dbo.ManifestacionConcepto66";

                        try
                        {
                            bulkCopy.WriteToServer(((DataTable)Session["Grid4_MV"]));
                        }
                        catch (Exception ex)
                        {
                            AlertError(ex.Message);
                        }
                    }
                }


                //Articulo 65 de la ley
                if (Session["Grid8_MV"] != null)
                {
                    //Si Anexo
                    //se limpia tabla ManifestacionAnexo65 para agregar información cambiada
                    dt = new DataTable();

                    cmdString = "DELETE FROM ManifestacionAnexo65 WHERE mvkeymanifestacion = " + Session["mvkeymanifestacion"].ToString();

                    try
                    {
                        using (SqlConnection conn = new SqlConnection(connString))
                        {
                            conn.Open();
                            using (SqlCommand comm = new SqlCommand())
                            {
                                comm.Connection = conn;
                                comm.CommandText = cmdString;
                                using (SqlDataReader reader = comm.ExecuteReader())
                                {
                                    if (!reader.HasRows)
                                        dt = null;
                                    else
                                        dt.Load(reader);
                                }
                            }
                            conn.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        AlertError(ex.Message + " Error en BD en tabla ManifestacionAnexo65");
                    }

                    //Hacemos un bulk a la tabla ManifestacionAnexo65                                
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(Session["Cadena"].ToString()))
                    {
                        foreach (DataColumn col in ((DataTable)Session["Grid8_MV"]).Columns)
                        {
                            bulkCopy.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                        }


                        bulkCopy.DestinationTableName = "dbo.ManifestacionAnexo65";

                        try
                        {
                            bulkCopy.WriteToServer(((DataTable)Session["Grid8_MV"]));
                        }
                        catch (Exception ex)
                        {
                            AlertError(ex.Message);
                        }
                    }
                }

                //No Anexo
                if (Session["Grid5_MV"] != null)
                {

                    //se limpia tabla ManifestacionConcepto65 para agregar información cambiada
                    dt = new DataTable();

                    cmdString = "DELETE FROM ManifestacionConcepto65 WHERE mvkeymanifestacion = " + Session["mvkeymanifestacion"].ToString();

                    try
                    {
                        using (SqlConnection conn = new SqlConnection(connString))
                        {
                            conn.Open();
                            using (SqlCommand comm = new SqlCommand())
                            {
                                comm.Connection = conn;
                                comm.CommandText = cmdString;
                                using (SqlDataReader reader = comm.ExecuteReader())
                                {
                                    if (!reader.HasRows)
                                        dt = null;
                                    else
                                        dt.Load(reader);
                                }
                            }
                            conn.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        AlertError(ex.Message + " Error en BD en tabla ManifestacionConcepto65");
                    }

                    //Hacemos un bulk a la tabla ManifestacionConcepto65                                
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(Session["Cadena"].ToString()))
                    {
                        foreach (DataColumn col in ((DataTable)Session["Grid5_MV"]).Columns)
                        {
                            bulkCopy.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                        }


                        bulkCopy.DestinationTableName = "dbo.ManifestacionConcepto65";

                        try
                        {
                            bulkCopy.WriteToServer(((DataTable)Session["Grid5_MV"]));
                        }
                        catch (Exception ex)
                        {
                            AlertError(ex.Message);
                        }
                    }
                }



                //Otros Articulos Conforme A La Ley
                if (Session["Grid6_MV"] != null)
                {
                    //se limpia tabla ManifestacionConcepto67 para agregar información cambiada
                    dt = new DataTable();

                    cmdString = "DELETE FROM ManifestacionConcepto67 WHERE mvkeymanifestacion = " + Session["mvkeymanifestacion"].ToString();

                    try
                    {
                        using (SqlConnection conn = new SqlConnection(connString))
                        {
                            conn.Open();
                            using (SqlCommand comm = new SqlCommand())
                            {
                                comm.Connection = conn;
                                comm.CommandText = cmdString;
                                using (SqlDataReader reader = comm.ExecuteReader())
                                {
                                    if (!reader.HasRows)
                                        dt = null;
                                    else
                                        dt.Load(reader);
                                }
                            }
                            conn.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        AlertError(ex.Message + " Error en BD en tabla ManifestacionConcepto67");
                    }


                    //Hacemos un bulk a la tabla ManifestacionConcepto67                                
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(Session["Cadena"].ToString()))
                    {
                        foreach (DataColumn col in ((DataTable)Session["Grid6_MV"]).Columns)
                        {
                            bulkCopy.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                        }


                        bulkCopy.DestinationTableName = "dbo.ManifestacionConcepto67";

                        try
                        {
                            bulkCopy.WriteToServer(((DataTable)Session["Grid6_MV"]));
                        }
                        catch (Exception ex)
                        {
                            AlertError(ex.Message);
                        }
                    }
                }

                //se limpia tabla ManifestacionValorAduana para agregar información cambiada

                if (Session["Grid7_MV"] != null)
                {
                    dt = new DataTable();

                    cmdString = "DELETE FROM ManifestacionValorAduana WHERE mvkeymanifestacion = " + Session["mvkeymanifestacion"].ToString();

                    try
                    {
                        using (SqlConnection conn = new SqlConnection(connString))
                        {
                            conn.Open();
                            using (SqlCommand comm = new SqlCommand())
                            {
                                comm.Connection = conn;
                                comm.CommandText = cmdString;
                                using (SqlDataReader reader = comm.ExecuteReader())
                                {
                                    if (!reader.HasRows)
                                        dt = null;
                                    else
                                        dt.Load(reader);
                                }
                            }
                            conn.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        AlertError(ex.Message + " Error en BD en tabla ManifestacionValorAduana");
                    }

                    //Hacemos un bulk a la tabla ManifestacionValorAduana                                
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(Session["Cadena"].ToString()))
                    {
                        foreach (DataColumn col in ((DataTable)Session["Grid7_MV"]).Columns)
                        {
                            bulkCopy.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                        }


                        bulkCopy.DestinationTableName = "dbo.ManifestacionValorAduana";

                        try
                        {
                            bulkCopy.WriteToServer(((DataTable)Session["Grid7_MV"]));
                        }
                        catch (Exception ex)
                        {
                            AlertError(ex.Message);
                        }
                    }
                }


                // Importación Temporal
                //se limpia tabla ManifestacionMercanciaProvisional para agregar información cambiada

                if (Session["Grid9_MV"] != null)
                {
                    dt = new DataTable();

                    cmdString = "DELETE FROM ManifestacionMercanciaProvisional WHERE mvkeymanifestacion = " + Session["mvkeymanifestacion"].ToString();

                    try
                    {
                        using (SqlConnection conn = new SqlConnection(connString))
                        {
                            conn.Open();
                            using (SqlCommand comm = new SqlCommand())
                            {
                                comm.Connection = conn;
                                comm.CommandText = cmdString;
                                using (SqlDataReader reader = comm.ExecuteReader())
                                {
                                    if (!reader.HasRows)
                                        dt = null;
                                    else
                                        dt.Load(reader);
                                }
                            }
                            conn.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        AlertError(ex.Message + " Error en BD en tabla ManifestacionMercanciaProvisional");
                    }

                    //Hacemos un bulk a la tabla ManifestacionMercanciaProvisional                                
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(Session["Cadena"].ToString()))
                    {
                        foreach (DataColumn col in ((DataTable)Session["Grid9_MV"]).Columns)
                        {
                            bulkCopy.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                        }


                        bulkCopy.DestinationTableName = "dbo.ManifestacionMercanciaProvisional";

                        try
                        {
                            bulkCopy.WriteToServer(((DataTable)Session["Grid9_MV"]));
                        }
                        catch (Exception ex)
                        {
                            AlertError(ex.Message);
                        }
                    }
                }

                //

                AlertSuccess("Cambios guardados con éxito");
                //Se mantiene en la misma pantalla
                MultiView1.ActiveViewIndex = 1;

            }
            catch (Exception ex)
            {
                AlertError(ex.Message);
            }
        }


        //Editar Grid2_MV detallehc
        protected void lkb_EditarDetalle_Click(object sender, EventArgs e)
        {
            int valida_select = 0;

            for (int i = 0; i < Grid2_MV.VisibleRowCount; i++)
            {
                if (Grid2_MV.Selection.IsRowSelected(i))
                {
                    //Abre Modal
                    MostrarModal2();

                    //Titulo del Modal
                    Modal2Titulo.InnerText = "Orden: " + Grid2_MV.GetSelectedFieldValues("orden")[0].ToString().Trim();                    

                    //Asignar valor al campo txt
                    Session["detallehckey"] = Grid2_MV.GetSelectedFieldValues("detallehckey")[0].ToString().Trim();
                    txt_fraccion.Text = Grid2_MV.GetSelectedFieldValues("fraccion")[0].ToString().Trim();
                    txt_Descripcion.Text = Grid2_MV.GetSelectedFieldValues("descripcion")[0].ToString().Trim();
                    txt_cantidad.Text = Grid2_MV.GetSelectedFieldValues("cantidad")[0].ToString().Trim();
                    txt_paisprod.Text = Grid2_MV.GetSelectedFieldValues("paisproduccion")[0].ToString().Trim();
                    txt_paisproc.Text = Grid2_MV.GetSelectedFieldValues("paisprocedencia")[0].ToString().Trim();                    

                    valida_select = 1;
                }
            }

            if (valida_select == 0)
                AlertError("Debe seleccionar una fila para poder editar");
        }

        //Botón guardar la sesion del Grid2_MV
        protected void btnGuardarSessionDetalle_Click(object sender, EventArgs e)
        {
            if (Session["Grid2_MV"] != null)
            {
                foreach (DataRow fila in ((DataTable)Session["Grid2_MV"]).Rows)
                {
                    if (Session["detallehckey"].ToString().Trim() == fila["detallehckey"].ToString().Trim())
                    {
                        fila["fraccion"] = txt_fraccion.Text.Trim();
                        fila["descripcion"] = txt_Descripcion.Text.Trim();
                        fila["cantidad"] = txt_cantidad.Text.Trim();
                        fila["paisproduccion"] = txt_paisprod.Text.Trim();
                        fila["paisprocedencia"] = txt_paisproc.Text.Trim();
                        break;
                    }
                }

                Grid2_MV.DataSource = Session["Grid2_MV"];
                Grid2_MV.DataBind();
                Grid2_MV.Settings.VerticalScrollableHeight = 168;
                Grid2_MV.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                //Grid2_MV.SettingsPager.PageSize = 20;

            }
        }


        //Editar Grid3_MV faturahc
        protected void lkb_EditarFactura_Click(object sender, EventArgs e)
        {
            int valida_select = 0;

            for (int i = 0; i < Grid3_MV.VisibleRowCount; i++)
            {
                if (Grid3_MV.Selection.IsRowSelected(i))
                {
                    //Abre Modal
                    MostrarModal3();

                    //Titulo del Modal
                    ModalTituloF.InnerText = "Factura: " + Grid3_MV.GetSelectedFieldValues("Factura")[0].ToString().Trim();

                    //Asignar valor
                    Session["facturahckey"] = Grid3_MV.GetSelectedFieldValues("facturahckey")[0].ToString().Trim();
                    txtF_Factura.Text = Grid3_MV.GetSelectedFieldValues("Factura")[0].ToString().Trim();
                    txtF_LugarEmision.Text = Grid3_MV.GetSelectedFieldValues("lugaremision")[0].ToString().Trim();
                    txtF_Subdivision.Text = Grid3_MV.GetSelectedFieldValues("subdivision")[0].ToString().Trim();
                    txtF_Proveedor.Text = Grid3_MV.GetSelectedFieldValues("proveedor")[0].ToString().Trim();
                    txtF_TaxNumero.Text = Grid3_MV.GetSelectedFieldValues("taxnumero")[0].ToString().Trim();
                    txtF_Calle.Text = Grid3_MV.GetSelectedFieldValues("facturacalle")[0].ToString().Trim();
                    txtF_INTEXT.Text = Grid3_MV.GetSelectedFieldValues("facturanumero")[0].ToString().Trim();
                    txtF_Ciudad.Text = Grid3_MV.GetSelectedFieldValues("facturaciudad")[0].ToString().Trim();
                    txtF_Pais.Text = Grid3_MV.GetSelectedFieldValues("facturapais")[0].ToString().Trim();

                    valida_select = 1;
                }
            }

            if (valida_select == 0)
                AlertError("Debe seleccionar una fila para poder editar");
        }

        //Botón guardar la sesion del Grid3_MV
        protected void btnGuardarSessionFactura_Click(object sender, EventArgs e)
        {
            if (Session["Grid3_MV"] != null)
            {
                foreach (DataRow fila in ((DataTable)Session["Grid3_MV"]).Rows)
                {
                    if (Session["facturahckey"].ToString().Trim() == fila["facturahckey"].ToString().Trim())
                    {
                        fila["Factura"] = txtF_Factura.Text.Trim();
                        fila["lugaremision"] = txtF_LugarEmision.Text.Trim();
                        fila["subdivision"] = txtF_Subdivision.Text.Trim();
                        fila["proveedor"] = txtF_Proveedor.Text.Trim();
                        fila["taxnumero"] = txtF_TaxNumero.Text.Trim();
                        fila["facturacalle"] = txtF_Calle.Text.Trim();
                        fila["facturanumero"] = txtF_INTEXT.Text.Trim();
                        fila["facturaciudad"] = txtF_Ciudad.Text.Trim();
                        fila["facturapais"] = txtF_Pais.Text.Trim();
                        break;
                    }
                }

                Grid3_MV.DataSource = Session["Grid3_MV"];
                Grid3_MV.DataBind();
                Grid3_MV.Settings.VerticalScrollableHeight = 168;
                Grid3_MV.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                Grid3_MV.SettingsPager.PageSize = 20;

            }
        }




        #region DATOS FACTURA

        protected void Grid2_MV_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            GridPageSize2 = int.Parse(e.Parameters);
            Grid2_MV.SettingsPager.PageSize = GridPageSize2;
            Grid2_MV.DataBind();
        }

        protected void chkConsultarF_Init(object sender, EventArgs e)
        {
            try
            {
                ASPxCheckBox chb = sender as ASPxCheckBox;
                GridViewDataItemTemplateContainer container = chb.NamingContainer as GridViewDataItemTemplateContainer;

                chb.ClientInstanceName = String.Format("chkConsultar{0}", container.VisibleIndex);
                chb.ClientSideEvents.CheckedChanged = String.Format("function (s, e) {{ chkConsultarFClick(s, e, {0}); }}", container.VisibleIndex);
            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "IA_ManifestacionValor-chkConsultarF_Init", ex, lblCadena.Text, ref mensaje);
            }
        }

        //Botón Agregar 
        protected void lkb_Factura_Agregar_Click(object sender, EventArgs e)
        {
            try
            {
                MostrarModalDF();

                ModalTituloDF.InnerHtml = "Agregar Factura";

                //Limpiar controles
                txt_DF_Factura.Text = string.Empty;
                date_DF_Fecha.Text = DateTime.Now.ToShortDateString();

                Grid2_MV.Settings.VerticalScrollableHeight = 168;
                Grid2_MV.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
            }
            catch(Exception ex)
            {
                AlertError(ex.Message);
            }
        }

        //Botón Editar 
        protected void lkb_Factura_Editar_Click(object sender, EventArgs e)
        {
            try
            {
                int valida_select = 0;

                if (Grid2_MV.VisibleRowCount > 0)
                {
                    for (int i = 0; i < Grid2_MV.VisibleRowCount; i++)
                    {
                        ASPxCheckBox chkConsultar = Grid2_MV.FindRowCellTemplateControl(i, (GridViewDataColumn)Grid2_MV.Columns["Seleccionar"], "chkConsultar") as ASPxCheckBox;

                        if (chkConsultar.Checked)
                        {
                            //Abre Modal 
                            MostrarModalDF();

                            //Titulo del Modal
                            ModalTituloDF.InnerText = "Editar Factura";

                            Session["mvkeyfacura"] = Grid2_MV.GetRowValues(i, "mvkeyfacura").ToString().Trim();
                            txt_DF_Factura.Text = Grid2_MV.GetRowValues(i, "Factura").ToString().Trim();
                            date_DF_Fecha.Text = Grid2_MV.GetRowValues(i, "FechaFactura").ToString().Trim();

                            valida_select = 1;
                            break;
                        }
                    }

                    if (valida_select == 0)
                        AlertError("Debe seleccionar un registro para poder editar");

                    
                    //string cledit = lkb_Factura_Editar.CssClass;

                    //if (cledit.Contains("disabled"))
                    //{
                    //    cledit = cledit.Replace("disabled", "");
                    //    lkb_Factura_Editar.CssClass = cledit;
                    //}

                    ////Habilitar botón de eliminar
                    //string cldelete = lkb_Factura_Eliminar.CssClass;
                    //if (cldelete.Contains("disabled"))
                    //{
                    //    cldelete = cldelete.Replace("disabled", "");
                    //    lkb_Factura_Eliminar.CssClass = cldelete;
                    //}

                    Grid2_MV.Settings.VerticalScrollableHeight = 168;
                    Grid2_MV.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                }
            }
            catch (Exception ex)
            {
                AlertError(ex.Message);
            }
        }

        //Botón Eliminar 
        protected void lkb_Factura_Eliminar_Click(object sender, EventArgs e)
        {
            try
            {
                int valida_select = 0;

                if (Grid2_MV.VisibleRowCount > 0)
                {
                    for (int i = 0; i < Grid2_MV.VisibleRowCount; i++)
                    {
                        ASPxCheckBox chkConsultar = Grid2_MV.FindRowCellTemplateControl(i, (GridViewDataColumn)Grid2_MV.Columns["Seleccionar"], "chkConsultar") as ASPxCheckBox;

                        if (chkConsultar.Checked)
                        {
                            
                            //Asignar valor al campo txt
                            Session["mvkeyfacura"] = Grid2_MV.GetRowValues(i, "mvkeyfacura").ToString().Trim();
                            Session["mvkeymanifestacion"] = Grid2_MV.GetRowValues(i, "mvkeymanifestacion").ToString().Trim();

                            valida_select = 1;

                            //Modal Question
                            string valida = "¿Desea eliminar este registro?";
                            AlertQuestionDF(valida);

                            break;
                        }
                    }

                    if (valida_select == 0)
                        AlertError("Debe seleccionar un registro para poder eliminar");

                }
            }
            catch (Exception ex)
            {
                AlertError(ex.Message);
            }
        }

        //Elimina registro en Grid2_MV
        protected void btnOkDF_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["Cadena"] == null || Session["mvkeyfacura"] == null || Session["Grid2_MV"] == null)
                {
                    string alerta = "<script>alert('La sesión ha expirado, debe volver a iniciar sesión');window.location.href='Login.aspx'</script>";
                    Session["Tab"] = "Salir";
                    Response.Write(alerta);
                    Response.Redirect("Login.aspx", false);
                    return;
                }

                
                DataTable dt = new DataTable();
                dt.Columns.Add("mvkeyfacura", typeof(Int64));
                dt.Columns.Add("Factura", typeof(string));
                dt.Columns.Add("mvkeymanifestacion", typeof(Int64));
                dt.Columns.Add("FechaFactura", typeof(string));

                dt = ((DataTable)Session["Grid2_MV"]);

                foreach(DataRow fila in dt.Rows)
                {
                    if (!fila.RowState.ToString().Contains("Del"))
                    {
                        if (fila["mvkeyfacura"].ToString() == Session["mvkeyfacura"].ToString())
                        {
                            fila.Delete();
                            break;
                        }
                    }
                }

                //Cargar de nuevo Grid2_MV
                Grid2_MV.DataSource = Session["Grid2_MV"] = dt;
                Grid2_MV.DataBind();
                Grid2_MV.Settings.VerticalScrollableHeight = 168;
                Grid2_MV.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                //Grid2_MV.SettingsPager.PageSize = 20;

            }
            catch (Exception ex)
            {
                AlertError(ex.Message);
            }
        }


        //Botón Guardar 
        protected void btnGuardarDF_Click(object sender, EventArgs e)
        {
            try
            {

                Grid2_MV.Settings.VerticalScrollableHeight = 168;
                Grid2_MV.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;

                #region Valida campos

                if (txt_DF_Factura.Text.Trim().Equals(string.Empty) || date_DF_Fecha.Text.Trim().Equals(string.Empty))
                {
                    if (txt_DF_Factura.Text.Trim().Equals(string.Empty))
                        AlertError("Debe escribir una Fatura");
                    if (date_DF_Fecha.Text.Trim().Equals(string.Empty))
                        AlertError("Debe seleccionar una Fecha");

                    return;
                }


                for (int i = 0; i < Grid2_MV.VisibleRowCount; i++)
                {
                    if (txt_DF_Factura.Text.Trim().ToUpper() == Grid2_MV.GetRowValues(i, "Factura").ToString().Trim() &&
                        date_DF_Fecha.Text.Trim() == Grid2_MV.GetRowValues(i, "FechaFactura").ToString().Trim() && !ModalTituloDF.InnerHtml.Contains("Edit"))
                    {

                        AlertError("Ya existe la Factura y la Fecha seleccionada");

                        return;
                    }

                    if (txt_DF_Factura.Text.Trim().ToUpper() == Grid2_MV.GetRowValues(i, "Factura").ToString().Trim() &&
                        date_DF_Fecha.Text.Trim() == Grid2_MV.GetRowValues(i, "FechaFactura").ToString().Trim() && ModalTituloDF.InnerHtml.Contains("Edit") &&
                        Session["mvkeyfacura"].ToString() != Grid2_MV.GetRowValues(i, "mvkeyfacura").ToString().Trim())
                    {

                        AlertError("Ya existe la Factura y la Fecha seleccionada");

                        return;
                    }
                }

                #endregion



                DataTable dt = new DataTable();
                dt.Columns.Add("mvkeyfacura", typeof(Int64));
                dt.Columns.Add("Factura", typeof(string));
                dt.Columns.Add("mvkeymanifestacion", typeof(Int64));                
                dt.Columns.Add("FechaFactura", typeof(string));

                if (Session["Grid2_MV"] != null)
                    dt = ((DataTable)Session["Grid2_MV"]);

                //GUARDAR
                Int64 valortop = 0;
                if (!ModalTituloDF.InnerHtml.Contains("Edit"))
                {
                    if (dt != null)
                    {
                        //Se obtiene el valor maximo de mvkeyfacura + 1
                        foreach (DataRow fila in dt.Rows)
                        {
                            if (!fila.RowState.ToString().Contains("Del"))
                            {
                                if (Int64.Parse(fila["mvkeyfacura"].ToString()) > valortop)
                                {
                                    valortop = Int64.Parse(fila["mvkeyfacura"].ToString());
                                }
                            }
                        }
                    }

                    valortop += 1;

                    //Se agrega nuevo registro al dt
                    DataRow dr = dt.NewRow();
                    dr["mvkeyfacura"] = valortop;
                    dr["Factura"] = txt_DF_Factura.Text.Trim();
                    dr["mvkeymanifestacion"] = Int64.Parse(Session["mvkeymanifestacion"].ToString());                    
                    dr["FechaFactura"] = date_DF_Fecha.Text.Trim();
                    dt.Rows.Add(dr);
                }
                
                //Editar
                else if (ModalTituloDF.InnerHtml.Contains("Edit"))
                {
                    if (dt != null)
                    {
                        foreach (DataRow fila in dt.Rows)
                        {
                            if (!fila.RowState.ToString().Contains("Del"))
                            {
                                if (fila["mvkeyfacura"].ToString() == Session["mvkeyfacura"].ToString())
                                {
                                    fila["Factura"] = txt_DF_Factura.Text.Trim();
                                    fila["FechaFactura"] = date_DF_Fecha.Text.Trim();
                                }
                            }
                        }
                    }
                }


                Grid2_MV.DataSource = Session["Grid2_MV"] = dt;
                Grid2_MV.DataBind();
                Grid2_MV.Settings.VerticalScrollableHeight = 168;
                Grid2_MV.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "IA_ManifestacionValor-btnGuardarDF_Click", ex, lblCadena.Text, ref mensaje);
                AlertError(ex.Message);
            }
        }

        //Botón Cancelar
        protected void btnCancelarDF_Click(object sender, EventArgs e)
        {
            try
            {
                Grid2_MV.Settings.VerticalScrollableHeight = 168;
                Grid2_MV.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "IA_ManifestacionValor-btnCancelarDF_Click", ex, lblCadena.Text, ref mensaje);
            }
        }

        #endregion



        #region VALOR DE TRANSACCIÓN DE LAS MERCANCIAS

        //TM
        protected void Grid3_MV_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            GridPageSize3 = int.Parse(e.Parameters);
            Grid3_MV.SettingsPager.PageSize = GridPageSize3;
            Grid3_MV.DataBind();
        }

        protected void chkConsultarTM_Init(object sender, EventArgs e)
        {
            try
            {
                ASPxCheckBox chb = sender as ASPxCheckBox;
                GridViewDataItemTemplateContainer container = chb.NamingContainer as GridViewDataItemTemplateContainer;

                chb.ClientInstanceName = String.Format("chkConsultarTM{0}", container.VisibleIndex);
                chb.ClientSideEvents.CheckedChanged = String.Format("function (s, e) {{ chkConsultarTMClick(s, e, {0}); }}", container.VisibleIndex);
            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "IA_ManifestacionValor-chkConsultarTM_Init", ex, lblCadena.Text, ref mensaje);
            }
        }

        //Botón Agregar
        protected void lkb_Anexo_Agregar_Click(object sender, EventArgs e)
        {
            try
            {
                MostrarModalTM();

                ModalTituloTM.InnerHtml = "Agregar factura o documento comercial";

                //Limpiar controles
                txt_ordenanexo.Text = string.Empty;
                Memo_Conceptoanexo.Text = string.Empty;

                Grid3_MV.Settings.VerticalScrollableHeight = 168;
                Grid3_MV.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;


                lkb_Anexo_Agregar.Focus();
                

            }
            catch (Exception ex)
            {
                AlertError(ex.Message);
            }
        }

        //Botón Editar
        protected void lkb_Anexo_Editar_Click(object sender, EventArgs e)
        {
            try
            {
                int valida_select = 0;

                if (Grid3_MV.VisibleRowCount > 0)
                {
                    for (int i = 0; i < Grid3_MV.VisibleRowCount; i++)
                    {
                        ASPxCheckBox chkConsultar = Grid3_MV.FindRowCellTemplateControl(i, (GridViewDataColumn)Grid3_MV.Columns["Seleccionar"], "chkConsultarTM") as ASPxCheckBox;

                        if (chkConsultar.Checked)
                        {
                            //Abre Modal 
                            MostrarModalTM();

                            //Titulo del Modal
                            ModalTituloTM.InnerText = "Editar factura o documento comercial";

                            Session["mvkeyanexomanifestacion"] = Grid3_MV.GetRowValues(i, "mvkeyanexomanifestacion").ToString().Trim();
                            Session["mvkeymanifestacion"] = Grid3_MV.GetRowValues(i, "mvkeymanifestacion").ToString().Trim();
                            txt_ordenanexo.Text = Grid3_MV.GetRowValues(i, "ordenanexo").ToString().Trim();
                            Memo_Conceptoanexo.Text = Grid3_MV.GetRowValues(i, "Conceptoanexo").ToString().Trim();

                            valida_select = 1;
                            break;
                        }
                    }

                    if (valida_select == 0)
                        AlertError("Debe seleccionar un registro para poder editar");

                    Grid3_MV.Settings.VerticalScrollableHeight = 168;
                    Grid3_MV.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                    lkb_Anexo_Agregar.Focus();
                }
            }
            catch (Exception ex)
            {
                AlertError(ex.Message);
            }
        }

        //Botón Eliminar
        protected void lkb_Anexo_Eliminar_Click(object sender, EventArgs e)
        {
            try
            {
                int valida_select = 0;
                lkb_Anexo_Agregar.Focus();

                if (Grid3_MV.VisibleRowCount > 0)
                {
                    for (int i = 0; i < Grid3_MV.VisibleRowCount; i++)
                    {
                        ASPxCheckBox chkConsultar = Grid3_MV.FindRowCellTemplateControl(i, (GridViewDataColumn)Grid3_MV.Columns["Seleccionar"], "chkConsultarTM") as ASPxCheckBox;

                        if (chkConsultar.Checked)
                        {

                            //Asignar valor al campo txt
                            Session["mvkeyanexomanifestacion"] = Grid3_MV.GetRowValues(i, "mvkeyanexomanifestacion").ToString().Trim();
                            Session["mvkeymanifestacion"] = Grid3_MV.GetRowValues(i, "mvkeymanifestacion").ToString().Trim();

                            valida_select = 1;

                            //Modal Question
                            string valida = "¿Desea eliminar este registro?";
                            AlertQuestionTM(valida);

                            break;
                        }
                    }

                    if (valida_select == 0)
                        AlertError("Debe seleccionar un registro para poder eliminar");

                }
            }
            catch (Exception ex)
            {
                AlertError(ex.Message);
            }
        }

        //Elimina registro en Grid3_MV
        protected void btnOkTM_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["Cadena"] == null || Session["mvkeyanexomanifestacion"] == null || Session["Grid3_MV"] == null)
                {
                    string alerta = "<script>alert('La sesión ha expirado, debe volver a iniciar sesión');window.location.href='Login.aspx'</script>";
                    Session["Tab"] = "Salir";
                    Response.Write(alerta);
                    Response.Redirect("Login.aspx", false);
                    return;
                }


                DataTable dt = new DataTable();
                dt.Columns.Add("mvkeyanexomanifestacion", typeof(Int64));
                dt.Columns.Add("ordenanexo", typeof(int));
                dt.Columns.Add("mvkeymanifestacion", typeof(Int64));
                dt.Columns.Add("Conceptoanexo", typeof(string));

                dt = ((DataTable)Session["Grid3_MV"]);

                foreach (DataRow fila in dt.Rows)
                {
                    if (!fila.RowState.ToString().Contains("Del"))
                    {
                        if (fila["mvkeyanexomanifestacion"].ToString() == Session["mvkeyanexomanifestacion"].ToString())
                        {
                            fila.Delete();
                            break;
                        }
                    }
                }

                //Cargar de nuevo Grid3_MV
                Grid3_MV.DataSource = Session["Grid3_MV"] = dt;
                Grid3_MV.DataBind();
                Grid3_MV.Settings.VerticalScrollableHeight = 168;
                Grid3_MV.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                //Grid3_MV.SettingsPager.PageSize = 20;

                lkb_Anexo_Agregar.Focus();


                //Ejecuta funcion script para validar si habilita o no pestaña de Articulo 65 de la Ley
                ScriptManager.RegisterStartupScript(this.Page, typeof(String), "ValorTransaccionMercancias", "<script> ValorTransaccionMercancias(); </script> ", false);

                //Ejecuta limpieza de Grid8_MV y Grid5_MV
                btnVTM_Click(this, new EventArgs());
            }
            catch (Exception ex)
            {
                AlertError(ex.Message);
            }
        }

        //Botón Guardar
        protected void btnGuardarTM_Click(object sender, EventArgs e)
        {
            try
            {

                Grid3_MV.Settings.VerticalScrollableHeight = 168;
                Grid3_MV.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                lkb_Anexo_Agregar.Focus();

                #region Valida campos

                if (txt_ordenanexo.Text.Trim().Equals(string.Empty) || Memo_Conceptoanexo.Text.Trim().Equals(string.Empty))
                {
                    MostrarModalTM();
                    

                    if (txt_ordenanexo.Text.Trim().Equals(string.Empty) && Memo_Conceptoanexo.Text.Trim().Equals(string.Empty))
                        AlertError("Enumere un anexo y escriba una factura o documento comercial");
                    else if (txt_ordenanexo.Text.Trim().Equals(string.Empty) && !Memo_Conceptoanexo.Text.Trim().Equals(string.Empty))
                        AlertError("Enumere un anexo");
                    else if (!txt_ordenanexo.Text.Trim().Equals(string.Empty) && Memo_Conceptoanexo.Text.Trim().Equals(string.Empty))
                        AlertError("Escriba una factura o documento comercial");

                    return;
                }


                for (int i = 0; i < Grid3_MV.VisibleRowCount; i++)
                {
                    if (txt_ordenanexo.Text.Trim().ToUpper() == Grid3_MV.GetRowValues(i, "ordenanexo").ToString().Trim() &&
                        Memo_Conceptoanexo.Text.Trim() == Grid3_MV.GetRowValues(i, "Conceptoanexo").ToString().Trim() && !ModalTituloTM.InnerHtml.Contains("Edit"))
                    {
                        MostrarModalTM();
                        AlertError("Ya existe ese valor de transacción de las mecancías");

                        return;
                    }

                    if (txt_ordenanexo.Text.Trim().ToUpper() == Grid3_MV.GetRowValues(i, "ordenanexo").ToString().Trim() &&
                        Memo_Conceptoanexo.Text.Trim() == Grid2_MV.GetRowValues(i, "Conceptoanexo").ToString().Trim() && ModalTituloTM.InnerHtml.Contains("Edit") &&
                        Session["mvkeyanexomanifestacion"].ToString() != Grid3_MV.GetRowValues(i, "mvkeyanexomanifestacion").ToString().Trim())
                    {
                        MostrarModalTM();
                        AlertError("Ya existe ese valor de transacción de las mecancías");

                        return;
                    }
                }

                #endregion



                DataTable dt = new DataTable();
                dt.Columns.Add("mvkeyanexomanifestacion", typeof(Int64));
                dt.Columns.Add("ordenanexo", typeof(int));
                dt.Columns.Add("mvkeymanifestacion", typeof(Int64));
                dt.Columns.Add("Conceptoanexo", typeof(string));

                if (Session["Grid3_MV"] != null)
                    dt = ((DataTable)Session["Grid3_MV"]);

                //GUARDAR
                Int64 valortop = 0;
                if (!ModalTituloTM.InnerHtml.Contains("Edit"))
                {
                    if (dt != null)
                    {
                        //Se obtiene el valor maximo de mvkeyanexomanifestacion + 1
                        foreach (DataRow fila in dt.Rows)
                        {
                            if (!fila.RowState.ToString().Contains("Del"))
                            {
                                if (Int64.Parse(fila["mvkeyanexomanifestacion"].ToString()) > valortop)
                                {
                                    valortop = Int64.Parse(fila["mvkeyanexomanifestacion"].ToString());
                                }
                            }
                        }
                    }

                    valortop += 1;

                    //Se agrega nuevo registro al dt
                    DataRow dr = dt.NewRow();
                    dr["mvkeyanexomanifestacion"] = valortop;
                    dr["ordenanexo"] = txt_ordenanexo.Text.Trim();
                    dr["mvkeymanifestacion"] = Int64.Parse(Session["mvkeymanifestacion"].ToString());
                    dr["Conceptoanexo"] = Memo_Conceptoanexo.Text.Trim();
                    dt.Rows.Add(dr);
                }

                //EDITAR
                else if (ModalTituloTM.InnerHtml.Contains("Edit"))
                {
                    foreach (DataRow fila in dt.Rows)
                    {
                        if (!fila.RowState.ToString().Contains("Del"))
                        {
                            if (fila["mvkeyanexomanifestacion"].ToString() == Session["mvkeyanexomanifestacion"].ToString())
                            {
                                fila["ordenanexo"] = txt_ordenanexo.Text.Trim();
                                fila["Conceptoanexo"] = Memo_Conceptoanexo.Text.Trim();
                            }
                        }
                    }
                }


                Grid3_MV.DataSource = Session["Grid3_MV"] = dt;
                Grid3_MV.DataBind();
                Grid3_MV.Settings.VerticalScrollableHeight = 168;
                Grid3_MV.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;

                //Ejecuta funcion script para validar si habilita o no pestaña de Articulo 65 de la Ley
                ScriptManager.RegisterStartupScript(this.Page, typeof(String), "ValorTransaccionMercancias", "<script> ValorTransaccionMercancias(); </script> ", false);

                //Ejecuta limpieza de Grid8_MV y Grid5_MV
                btnVTM_Click(this, new EventArgs());
            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "IA_ManifestacionValor-btnGuardarTM_Click", ex, lblCadena.Text, ref mensaje);
                AlertError(ex.Message);
            }
        }

        //Botón Cancelar Factura
        protected void btnCancelarTM_Click(object sender, EventArgs e)
        {
            try
            {
                Grid3_MV.Settings.VerticalScrollableHeight = 168;
                Grid3_MV.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;

                lkb_Anexo_Agregar.Focus();
            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "IA_ManifestacionValor-btnCancelarTM_Click", ex, lblCadena.Text, ref mensaje);
            }
        }



        //TM2
        protected void Grid4_MV_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            GridPageSize4 = int.Parse(e.Parameters);
            Grid4_MV.SettingsPager.PageSize = GridPageSize4;
            Grid4_MV.DataBind();
        }

        protected void chkConsultarTM2_Init(object sender, EventArgs e)
        {
            try
            {
                ASPxCheckBox chb = sender as ASPxCheckBox;
                GridViewDataItemTemplateContainer container = chb.NamingContainer as GridViewDataItemTemplateContainer;

                chb.ClientInstanceName = String.Format("chkConsultarTM2{0}", container.VisibleIndex);
                chb.ClientSideEvents.CheckedChanged = String.Format("function (s, e) {{ chkConsultarTM2Click(s, e, {0}); }}", container.VisibleIndex);
            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "IA_ManifestacionValor-chkConsultarTM2_Init", ex, lblCadena.Text, ref mensaje);
            }
        }

        //Botón Agregar
        protected void lkb_ManifestacionConcepto66_Agregar_Click(object sender, EventArgs e)
        {
            try
            {
                MostrarModalTM2();

                ModalTituloTM2.InnerHtml = "Agregar mercancia";

                //Limpiar controles
                txt_TM2_ordenconcepto.Text = string.Empty;
                txt_TM2_mercancia.Text = string.Empty;
                txt_TM2_factura.Text = string.Empty;
                txt_TM2_importe.Text = string.Empty;
                txt_TM2_moneda.Text = string.Empty;
                txt_TM2_concepto_cargo.Text = string.Empty;

                Grid4_MV.Settings.VerticalScrollableHeight = 168;
                Grid4_MV.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;


                lkb_ManifestacionConcepto66_Agregar.Focus();


            }
            catch (Exception ex)
            {
                AlertError(ex.Message);
            }
        }

        //Botón Editar
        protected void lkb_ManifestacionConcepto66_Editar_Click(object sender, EventArgs e)
        {
            try
            {
                int valida_select = 0;

                if (Grid4_MV.VisibleRowCount > 0)
                {
                    for (int i = 0; i < Grid4_MV.VisibleRowCount; i++)
                    {
                        ASPxCheckBox chkConsultar = Grid4_MV.FindRowCellTemplateControl(i, (GridViewDataColumn)Grid4_MV.Columns["Seleccionar"], "chkConsultarTM2") as ASPxCheckBox;

                        if (chkConsultar.Checked)
                        {
                            //Abre Modal 
                            MostrarModalTM2();

                            //Titulo del Modal
                            ModalTituloTM2.InnerText = "Editar mercancia";

                            Session["mvkeyconcepto66"] = Grid4_MV.GetRowValues(i, "mvkeyconcepto66").ToString().Trim();
                            Session["mvkeymanifestacion"] = Grid4_MV.GetRowValues(i, "mvkeymanifestacion").ToString().Trim();
                            txt_TM2_ordenconcepto.Text = Grid4_MV.GetRowValues(i, "ordenconcepto").ToString().Trim();
                            txt_TM2_mercancia.Text = Grid4_MV.GetRowValues(i, "mercancia").ToString().Trim();
                            txt_TM2_factura.Text = Grid4_MV.GetRowValues(i, "factura").ToString().Trim();
                            txt_TM2_importe.Text = Grid4_MV.GetRowValues(i, "importeconcepto").ToString().Trim();
                            txt_TM2_moneda.Text = Grid4_MV.GetRowValues(i, "monedaconcepto").ToString().Trim();
                            txt_TM2_concepto_cargo.Text = Grid4_MV.GetRowValues(i, "conceptodelcargo").ToString().Trim();

                            valida_select = 1;
                            break;
                        }
                    }

                    if (valida_select == 0)
                        AlertError("Debe seleccionar un registro para poder editar");

                    Grid4_MV.Settings.VerticalScrollableHeight = 168;
                    Grid4_MV.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                    lkb_ManifestacionConcepto66_Agregar.Focus();
                }
            }
            catch (Exception ex)
            {
                AlertError(ex.Message);
            }
        }

        //Botón Eliminar
        protected void lkb_ManifestacionConcepto66_Eliminar_Click(object sender, EventArgs e)
        {
            try
            {
                int valida_select = 0;
                lkb_ManifestacionConcepto66_Agregar.Focus();

                if (Grid4_MV.VisibleRowCount > 0)
                {
                    for (int i = 0; i < Grid4_MV.VisibleRowCount; i++)
                    {
                        ASPxCheckBox chkConsultar = Grid4_MV.FindRowCellTemplateControl(i, (GridViewDataColumn)Grid4_MV.Columns["Seleccionar"], "chkConsultarTM2") as ASPxCheckBox;

                        if (chkConsultar.Checked)
                        {

                            //Asignar valor al campo txt
                            Session["mvkeyconcepto66"] = Grid4_MV.GetRowValues(i, "mvkeyconcepto66").ToString().Trim();
                            Session["mvkeymanifestacion"] = Grid4_MV.GetRowValues(i, "mvkeymanifestacion").ToString().Trim();

                            valida_select = 1;

                            //Modal Question
                            string valida = "¿Desea eliminar este registro?";
                            AlertQuestionTM2(valida);

                            break;
                        }
                    }

                    if (valida_select == 0)
                        AlertError("Debe seleccionar un registro para poder eliminar");

                }
            }
            catch (Exception ex)
            {
                AlertError(ex.Message);
            }
        }

        //Elimina registro en Grid4_MV
        protected void btnOkTM2_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["Cadena"] == null || Session["mvkeyconcepto66"] == null || Session["Grid4_MV"] == null)
                {
                    string alerta = "<script>alert('La sesión ha expirado, debe volver a iniciar sesión');window.location.href='Login.aspx'</script>";
                    Session["Tab"] = "Salir";
                    Response.Write(alerta);
                    Response.Redirect("Login.aspx", false);
                    return;
                }


                DataTable dt = new DataTable();
                dt.Columns.Add("mvkeyconcepto66", typeof(Int64));
                dt.Columns.Add("ordenconcepto", typeof(int));
                dt.Columns.Add("mvkeymanifestacion", typeof(Int64));
                dt.Columns.Add("mercancia", typeof(string));
                dt.Columns.Add("factura", typeof(string));
                dt.Columns.Add("importeconcepto", typeof(string));
                dt.Columns.Add("monedaconcepto", typeof(string));
                dt.Columns.Add("conceptodelcargo", typeof(string));

                dt = ((DataTable)Session["Grid4_MV"]);

                foreach (DataRow fila in dt.Rows)
                {
                    if (!fila.RowState.ToString().Contains("Del"))
                    {
                        if (fila["mvkeyconcepto66"].ToString() == Session["mvkeyconcepto66"].ToString())
                        {
                            fila.Delete();
                            break;
                        }
                    }
                }

                //Cargar de nuevo Grid4_MV
                Grid4_MV.DataSource = Session["Grid4_MV"] = dt;
                Grid4_MV.DataBind();
                Grid4_MV.Settings.VerticalScrollableHeight = 168;
                Grid4_MV.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                //Grid4_MV.SettingsPager.PageSize = 20;

                lkb_ManifestacionConcepto66_Agregar.Focus();

                //Ejecuta funcion script para validar si habilita o no pestaña de Articulo 65 de la Ley
                ScriptManager.RegisterStartupScript(this.Page, typeof(String), "ValorTransaccionMercancias", "<script> ValorTransaccionMercancias(); </script> ", false);

                //Ejecuta limpieza de Grid8_MV y Grid5_MV
                btnVTM_Click(this, new EventArgs());
            }
            catch (Exception ex)
            {
                AlertError(ex.Message);
            }
        }

        //Botón Guardar
        protected void btnGuardarTM2_Click(object sender, EventArgs e)
        {
            try
            {

                Grid4_MV.Settings.VerticalScrollableHeight = 168;
                Grid4_MV.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                lkb_ManifestacionConcepto66_Agregar.Focus();


                #region Valida campos

                if (txt_TM2_ordenconcepto.Text.Trim().Equals(string.Empty) || txt_TM2_mercancia.Text.Trim().Equals(string.Empty) ||
                    txt_TM2_factura.Text.Trim().Equals(string.Empty) || txt_TM2_importe.Text.Trim().Equals(string.Empty) ||
                    txt_TM2_moneda.Text.Trim().Equals(string.Empty) || txt_TM2_concepto_cargo.Text.Trim().Equals(string.Empty))
                {
                    MostrarModalTM2();

                    if (txt_TM2_ordenconcepto.Text.Trim().Equals(string.Empty))
                        AlertError("Enumere un concepto de orden");
                    else if (txt_TM2_mercancia.Text.Trim().Equals(string.Empty))
                        AlertError("Escriba una mercancia");
                    else if (txt_TM2_factura.Text.Trim().Equals(string.Empty))
                        AlertError("Escriba una factura");
                    else if (txt_TM2_importe.Text.Trim().Equals(string.Empty))
                        AlertError("Escriba un importe");
                    else if (txt_TM2_moneda.Text.Trim().Equals(string.Empty))
                        AlertError("Escriba una moneda");
                    else if (txt_TM2_concepto_cargo.Text.Trim().Equals(string.Empty))
                        AlertError("Escriba un concepto de cargo");

                    return;
                }


                for (int i = 0; i < Grid4_MV.VisibleRowCount; i++)
                {
                    if (txt_TM2_ordenconcepto.Text.Trim().ToUpper() == Grid4_MV.GetRowValues(i, "ordenconcepto").ToString().Trim() &&
                        txt_TM2_mercancia.Text.Trim() == Grid4_MV.GetRowValues(i, "mercancia").ToString().Trim() &&
                        txt_TM2_factura.Text.Trim() == Grid4_MV.GetRowValues(i, "factura").ToString().Trim() &&
                        txt_TM2_concepto_cargo.Text.Trim() == Grid4_MV.GetRowValues(i, "conceptodelcargo").ToString().Trim() && !ModalTituloTM2.InnerHtml.Contains("Edit"))
                    {
                        MostrarModalTM2();
                        AlertError("Ya existe esa mercancia");

                        return;
                    }

                    if (txt_TM2_ordenconcepto.Text.Trim().ToUpper() == Grid4_MV.GetRowValues(i, "ordenconcepto").ToString().Trim() &&
                        txt_TM2_mercancia.Text.Trim() == Grid4_MV.GetRowValues(i, "mercancia").ToString().Trim() &&
                        txt_TM2_factura.Text.Trim() == Grid4_MV.GetRowValues(i, "factura").ToString().Trim() &&
                        txt_TM2_concepto_cargo.Text.Trim() == Grid4_MV.GetRowValues(i, "conceptodelcargo").ToString().Trim() && ModalTituloTM2.InnerHtml.Contains("Edit") &&
                        Session["mvkeyconcepto66"].ToString() != Grid4_MV.GetRowValues(i, "mvkeyconcepto66").ToString().Trim())
                    {
                        MostrarModalTM2();
                        AlertError("Ya existe esa mercancia");

                        return;
                    }
                }

                #endregion



                DataTable dt = new DataTable();
                dt.Columns.Add("mvkeyconcepto66", typeof(Int64));
                dt.Columns.Add("ordenconcepto", typeof(int));
                dt.Columns.Add("mvkeymanifestacion", typeof(Int64));
                dt.Columns.Add("mercancia", typeof(string));
                dt.Columns.Add("factura", typeof(string));
                dt.Columns.Add("importeconcepto", typeof(string));
                dt.Columns.Add("monedaconcepto", typeof(string));
                dt.Columns.Add("conceptodelcargo", typeof(string));

                if (Session["Grid4_MV"] != null)
                    dt = ((DataTable)Session["Grid4_MV"]);

                //GUARDAR
                Int64 valortop = 0;
                if (!ModalTituloTM2.InnerHtml.Contains("Edit"))
                {
                    if (dt != null)
                    {
                        //Se obtiene el valor maximo de mvkeyanexomanifestacion + 1
                        foreach (DataRow fila in dt.Rows)
                        {
                            if (!fila.RowState.ToString().Contains("Del"))
                            {
                                if (Int64.Parse(fila["mvkeyconcepto66"].ToString()) > valortop)
                                {
                                    valortop = Int64.Parse(fila["mvkeyconcepto66"].ToString());
                                }
                            }
                        }
                    }

                    valortop += 1;

                    //Se agrega nuevo registro al dt
                    DataRow dr = dt.NewRow();
                    dr["mvkeyconcepto66"] = valortop;
                    dr["ordenconcepto"] = txt_TM2_ordenconcepto.Text.Trim();
                    dr["mvkeymanifestacion"] = Int64.Parse(Session["mvkeymanifestacion"].ToString());
                    dr["mercancia"] = txt_TM2_mercancia.Text.Trim();
                    dr["factura"] = txt_TM2_factura.Text.Trim();
                    dr["importeconcepto"] = txt_TM2_importe.Text.Trim();
                    dr["monedaconcepto"] = txt_TM2_moneda.Text.Trim();
                    dr["conceptodelcargo"] = txt_TM2_concepto_cargo.Text.Trim();
                    dt.Rows.Add(dr);
                }

                //EDITAR
                else if (ModalTituloTM2.InnerHtml.Contains("Edit"))
                {
                    foreach (DataRow fila in dt.Rows)
                    {
                        if (!fila.RowState.ToString().Contains("Del"))
                        {
                            if (fila["mvkeyconcepto66"].ToString() == Session["mvkeyconcepto66"].ToString())
                            {
                                fila["ordenconcepto"] = txt_TM2_ordenconcepto.Text.Trim();
                                fila["mercancia"] = txt_TM2_mercancia.Text.Trim();
                                fila["factura"] = txt_TM2_factura.Text.Trim();
                                fila["importeconcepto"] = txt_TM2_importe.Text.Trim();
                                fila["monedaconcepto"] = txt_TM2_moneda.Text.Trim();
                                fila["conceptodelcargo"] = txt_TM2_concepto_cargo.Text.Trim();
                            }
                        }
                    }
                }


                Grid4_MV.DataSource = Session["Grid4_MV"] = dt;
                Grid4_MV.DataBind();
                Grid4_MV.Settings.VerticalScrollableHeight = 168;
                Grid4_MV.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;

                //Ejecuta funcion script para validar si habilita o no pestaña de Articulo 65 de la Ley
                ScriptManager.RegisterStartupScript(this.Page, typeof(String), "ValorTransaccionMercancias", "<script> ValorTransaccionMercancias(); </script> ", false);

                //Ejecuta limpieza de Grid8_MV y Grid5_MV
                btnVTM_Click(this, new EventArgs());
            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "IA_ManifestacionValor-btnGuardarTM2_Click", ex, lblCadena.Text, ref mensaje);
                AlertError(ex.Message);
            }
        }

        //Botón Cancelar Factura
        protected void btnCancelarTM2_Click(object sender, EventArgs e)
        {
            try
            {
                Grid4_MV.Settings.VerticalScrollableHeight = 168;
                Grid4_MV.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;

                lkb_ManifestacionConcepto66_Agregar.Focus();
            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "IA_ManifestacionValor-btnCancelarTM2_Click", ex, lblCadena.Text, ref mensaje);
            }
        }




        //Se ejecuta por función en javascript para borrar si tienen datos el Grid8_MV y el Grid5_MV
        protected void btnVTM_Click(object sender, EventArgs e)
        {
            Grid8_MV.DataSource = Session["Grid8_MV"] = null;
            Grid8_MV.DataBind();
            Grid8_MV.Settings.VerticalScrollableHeight = 168;
            Grid8_MV.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
            Grid8_MV.SettingsPager.PageSize = 20;

            Grid5_MV.DataSource = Session["Grid5_MV"] = null;
            Grid5_MV.DataBind();
            Grid5_MV.Settings.VerticalScrollableHeight = 168;
            Grid5_MV.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;

            //Ejecuta funcion script para validar si habilita o no pestaña de Articulo 65 de la Ley
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "ValorTransaccionMercancias", "<script> ValorTransaccionMercancias(); </script> ", false);

            Session["pestaña"] = "VTM";
        }

        #endregion



        #region ARTICULO 65 DE LA LEY

        //Si Anexo
        protected void Grid8_MV_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            GridPageSize8 = int.Parse(e.Parameters);
            Grid8_MV.SettingsPager.PageSize = GridPageSize8;
            Grid8_MV.DataBind();
        }

        protected void chkConsultarS65_Init(object sender, EventArgs e)
        {
            try
            {
                ASPxCheckBox chb = sender as ASPxCheckBox;
                GridViewDataItemTemplateContainer container = chb.NamingContainer as GridViewDataItemTemplateContainer;

                chb.ClientInstanceName = String.Format("chkConsultarS65{0}", container.VisibleIndex);
                chb.ClientSideEvents.CheckedChanged = String.Format("function (s, e) {{ chkConsultarS65Click(s, e, {0}); }}", container.VisibleIndex);
            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "IA_ManifestacionValor-chkConsultarS65_Init", ex, lblCadena.Text, ref mensaje);
            }
        }

        //Botón Agregar
        protected void lkb_OrdenAnexo65_Agregar_Click(object sender, EventArgs e)
        {
            try
            {
                MostrarModalS65();

                ModalTituloS65.InnerHtml = "Agregar conceptos previstos en el articulo 65 de la ley";

                //Limpiar controles
                txt_S65_ordenanexo65.Text = string.Empty;
                Memo_S65_conceptoanexo65.Text = string.Empty;

                Grid8_MV.Settings.VerticalScrollableHeight = 168;
                Grid8_MV.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;


                lkb_OrdenAnexo65_Agregar.Focus();


            }
            catch (Exception ex)
            {
                AlertError(ex.Message);
            }
        }

        //Botón Editar
        protected void lkb_OrdenAnexo65_Editar_Click(object sender, EventArgs e)
        {
            try
            {
                int valida_select = 0;

                if (Grid8_MV.VisibleRowCount > 0)
                {
                    for (int i = 0; i < Grid8_MV.VisibleRowCount; i++)
                    {
                        ASPxCheckBox chkConsultar = Grid8_MV.FindRowCellTemplateControl(i, (GridViewDataColumn)Grid8_MV.Columns["Seleccionar"], "chkConsultarS65") as ASPxCheckBox;

                        if (chkConsultar.Checked)
                        {
                            //Abre Modal 
                            MostrarModalS65();

                            //Titulo del Modal
                            ModalTituloS65.InnerText = "Editar conceptos previstos en el articulo 65 de la ley";

                            Session["mvkeyanexomanifestacion65"] = Grid8_MV.GetRowValues(i, "mvkeyanexomanifestacion65").ToString().Trim();
                            Session["mvkeymanifestacion"] = Grid8_MV.GetRowValues(i, "mvkeymanifestacion").ToString().Trim();
                            txt_S65_ordenanexo65.Text = Grid8_MV.GetRowValues(i, "ordenanexo65").ToString().Trim();
                            Memo_S65_conceptoanexo65.Text = Grid8_MV.GetRowValues(i, "Conceptoanexo65").ToString().Trim();


                            valida_select = 1;
                            break;
                        }
                    }

                    if (valida_select == 0)
                        AlertError("Debe seleccionar un registro para poder editar");

                    Grid8_MV.Settings.VerticalScrollableHeight = 168;
                    Grid8_MV.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                    lkb_OrdenAnexo65_Agregar.Focus();
                }
            }
            catch (Exception ex)
            {
                AlertError(ex.Message);
            }
        }

        //Botón Eliminar
        protected void lkb_OrdenAnexo65_Eliminar_Click(object sender, EventArgs e)
        {
            try
            {
                int valida_select = 0;
                lkb_OrdenAnexo65_Agregar.Focus();

                if (Grid8_MV.VisibleRowCount > 0)
                {
                    for (int i = 0; i < Grid8_MV.VisibleRowCount; i++)
                    {
                        ASPxCheckBox chkConsultar = Grid8_MV.FindRowCellTemplateControl(i, (GridViewDataColumn)Grid8_MV.Columns["Seleccionar"], "chkConsultarS65") as ASPxCheckBox;

                        if (chkConsultar.Checked)
                        {

                            //Asignar valor al campo txt
                            Session["mvkeyanexomanifestacion65"] = Grid8_MV.GetRowValues(i, "mvkeyanexomanifestacion65").ToString().Trim();
                            Session["mvkeymanifestacion"] = Grid8_MV.GetRowValues(i, "mvkeymanifestacion").ToString().Trim();

                            valida_select = 1;

                            //Modal Question
                            string valida = "¿Desea eliminar este registro?";
                            AlertQuestionS65(valida);

                            break;
                        }
                    }

                    if (valida_select == 0)
                        AlertError("Debe seleccionar un registro para poder eliminar");

                }
            }
            catch (Exception ex)
            {
                AlertError(ex.Message);
            }
        }

        //Elimina registro en Grid8_MV
        protected void btnOkS65_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["Cadena"] == null || Session["mvkeyanexomanifestacion65"] == null || Session["Grid8_MV"] == null)
                {
                    string alerta = "<script>alert('La sesión ha expirado, debe volver a iniciar sesión');window.location.href='Login.aspx'</script>";
                    Session["Tab"] = "Salir";
                    Response.Write(alerta);
                    Response.Redirect("Login.aspx", false);
                    return;
                }


                DataTable dt = new DataTable();
                dt.Columns.Add("mvkeyanexomanifestacion65", typeof(Int64));
                dt.Columns.Add("ordenanexo65", typeof(int));
                dt.Columns.Add("mvkeymanifestacion", typeof(Int64));
                dt.Columns.Add("Conceptoanexo65", typeof(string));


                dt = ((DataTable)Session["Grid8_MV"]);

                foreach (DataRow fila in dt.Rows)
                {
                    if (!fila.RowState.ToString().Contains("Del"))
                    {
                        if (fila["mvkeyanexomanifestacion65"].ToString() == Session["mvkeyanexomanifestacion65"].ToString())
                        {
                            fila.Delete();
                            break;
                        }
                    }
                }

                //Cargar de nuevo Grid8_MV
                Grid8_MV.DataSource = Session["Grid8_MV"] = dt;
                Grid8_MV.DataBind();
                Grid8_MV.Settings.VerticalScrollableHeight = 168;
                Grid8_MV.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                //Grid4_MV.SettingsPager.PageSize = 20;

                lkb_OrdenAnexo65_Agregar.Focus();

                //Ejecuta funcion script para validar si habilita o no pestaña de Valor de TRansacción de las Mercancias
                ScriptManager.RegisterStartupScript(this.Page, typeof(String), "Articulo65", "<script> Articulo65(); </script> ", false);

                //Ejecuta limpieza de Grid3_MV y Grid4_MV
                btnA65_Click(this, new EventArgs());
            }
            catch (Exception ex)
            {
                AlertError(ex.Message);
            }
        }

        //Botón Guardar
        protected void btnGuardarS65_Click(object sender, EventArgs e)
        {
            try
            {

                Grid8_MV.Settings.VerticalScrollableHeight = 168;
                Grid8_MV.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                lkb_OrdenAnexo65_Agregar.Focus();


                #region Valida campos

                if (txt_S65_ordenanexo65.Text.Trim().Equals(string.Empty) || Memo_S65_conceptoanexo65.Text.Trim().Equals(string.Empty))
                {
                    MostrarModalS65();

                    if (txt_S65_ordenanexo65.Text.Trim().Equals(string.Empty))
                        AlertError("Enumere un anexo");
                    else if (Memo_S65_conceptoanexo65.Text.Trim().Equals(string.Empty))
                        AlertError("Escriba una factura o documento comercial");                    

                    return;
                }


                for (int i = 0; i < Grid8_MV.VisibleRowCount; i++)
                {
                    if (txt_S65_ordenanexo65.Text.Trim().ToUpper() == Grid8_MV.GetRowValues(i, "ordenanexo65").ToString().Trim() &&
                        Memo_S65_conceptoanexo65.Text.Trim() == Grid8_MV.GetRowValues(i, "Conceptoanexo65").ToString().Trim() && !ModalTituloS65.InnerHtml.Contains("Edit"))
                    {
                        MostrarModalS65();
                        AlertError("Ya existe ese concepto");

                        return;
                    }

                    if (txt_S65_ordenanexo65.Text.Trim().ToUpper() == Grid8_MV.GetRowValues(i, "ordenanexo65").ToString().Trim() &&
                        Memo_S65_conceptoanexo65.Text.Trim() == Grid8_MV.GetRowValues(i, "Conceptoanexo65").ToString().Trim() && ModalTituloS65.InnerHtml.Contains("Edit") &&
                        Session["mvkeyanexomanifestacion65"].ToString() != Grid8_MV.GetRowValues(i, "mvkeyanexomanifestacion65").ToString().Trim())
                    {
                        MostrarModalS65();
                        AlertError("Ya existe ese concepto");

                        return;
                    }
                }

                #endregion



                DataTable dt = new DataTable();
                dt.Columns.Add("mvkeyanexomanifestacion65", typeof(Int64));
                dt.Columns.Add("ordenanexo65", typeof(int));
                dt.Columns.Add("mvkeymanifestacion", typeof(Int64));
                dt.Columns.Add("Conceptoanexo65", typeof(string));

                if (Session["Grid8_MV"] != null)
                    dt = ((DataTable)Session["Grid8_MV"]);

                //GUARDAR
                Int64 valortop = 0;
                if (!ModalTituloS65.InnerHtml.Contains("Edit"))
                {
                    if (dt != null)
                    {
                        //Se obtiene el valor maximo de mvkeyanexomanifestacion + 1
                        foreach (DataRow fila in dt.Rows)
                        {
                            if (!fila.RowState.ToString().Contains("Del"))
                            {
                                if (Int64.Parse(fila["mvkeyanexomanifestacion65"].ToString()) > valortop)
                                {
                                    valortop = Int64.Parse(fila["mvkeyanexomanifestacion65"].ToString());
                                }
                            }
                        }
                    }

                    valortop += 1;

                    //Se agrega nuevo registro al dt
                    DataRow dr = dt.NewRow();
                    dr["mvkeyanexomanifestacion65"] = valortop;
                    dr["ordenanexo65"] = txt_S65_ordenanexo65.Text.Trim();
                    dr["mvkeymanifestacion"] = Int64.Parse(Session["mvkeymanifestacion"].ToString());
                    dr["Conceptoanexo65"] = Memo_S65_conceptoanexo65.Text.Trim();                    
                    dt.Rows.Add(dr);
                }

                //EDITAR
                else if (ModalTituloS65.InnerHtml.Contains("Edit"))
                {
                    foreach (DataRow fila in dt.Rows)
                    {
                        if (!fila.RowState.ToString().Contains("Del"))
                        {
                            if (fila["mvkeyanexomanifestacion65"].ToString() == Session["mvkeyanexomanifestacion65"].ToString())
                            {
                                fila["ordenanexo65"] = txt_S65_ordenanexo65.Text.Trim();
                                fila["Conceptoanexo65"] = Memo_S65_conceptoanexo65.Text.Trim();
                            }
                        }
                    }
                }


                Grid8_MV.DataSource = Session["Grid8_MV"] = dt;
                Grid8_MV.DataBind();
                Grid8_MV.Settings.VerticalScrollableHeight = 168;
                Grid8_MV.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;

                //Ejecuta funcion script para validar si habilita o no pestaña de Valor de TRansacción de las Mercancias
                ScriptManager.RegisterStartupScript(this.Page, typeof(String), "Articulo65", "<script> Articulo65(); </script> ", false);

                //Ejecuta limpieza de Grid3_MV y Grid4_MV
                btnA65_Click(this, new EventArgs());
            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "IA_ManifestacionValor-btnGuardarS65_Click", ex, lblCadena.Text, ref mensaje);
                AlertError(ex.Message);
            }
        }

        //Botón Cancelar Factura
        protected void btnCancelarS65_Click(object sender, EventArgs e)
        {
            try
            {
                Grid8_MV.Settings.VerticalScrollableHeight = 168;
                Grid8_MV.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;

                lkb_OrdenAnexo65_Agregar.Focus();
            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "IA_ManifestacionValor-btnCancelarS65_Click", ex, lblCadena.Text, ref mensaje);
            }
        }



        //No Anexo
        protected void Grid5_MV_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            GridPageSize5 = int.Parse(e.Parameters);
            Grid5_MV.SettingsPager.PageSize = GridPageSize5;
            Grid5_MV.DataBind();
        }

        protected void chkConsultarN65_Init(object sender, EventArgs e)
        {
            try
            {
                ASPxCheckBox chb = sender as ASPxCheckBox;
                GridViewDataItemTemplateContainer container = chb.NamingContainer as GridViewDataItemTemplateContainer;

                chb.ClientInstanceName = String.Format("chkConsultarN65{0}", container.VisibleIndex);
                chb.ClientSideEvents.CheckedChanged = String.Format("function (s, e) {{ chkConsultarN65Click(s, e, {0}); }}", container.VisibleIndex);
            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "IA_ManifestacionValor-chkConsultarN65_Init", ex, lblCadena.Text, ref mensaje);
            }
        }

        //Botón Agregar
        protected void lkb_ManifestacionConcepto65_Agregar_Click(object sender, EventArgs e)
        {
            try
            {
                MostrarModalN65();

                ModalTituloN65.InnerHtml = "Agregar factura o documento, mercancia o proveedor";

                //Limpiar controles
                txt_N65_ordenconcepto.Text = string.Empty;
                txt_N65_mercancia.Text = string.Empty;
                txt_N65_factura.Text = string.Empty;
                txt_N65_importe.Text = string.Empty;
                txt_N65_moneda.Text = string.Empty;
                txt_N65_conceptocargo.Text = string.Empty;

                Grid5_MV.Settings.VerticalScrollableHeight = 168;
                Grid5_MV.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;


                lkb_ManifestacionConcepto65_Agregar.Focus();


            }
            catch (Exception ex)
            {
                AlertError(ex.Message);
            }
        }

        //Botón Editar
        protected void lkb_ManifestacionConcepto65_Editar_Click(object sender, EventArgs e)
        {
            try
            {
                int valida_select = 0;

                if (Grid5_MV.VisibleRowCount > 0)
                {
                    for (int i = 0; i < Grid5_MV.VisibleRowCount; i++)
                    {
                        ASPxCheckBox chkConsultar = Grid5_MV.FindRowCellTemplateControl(i, (GridViewDataColumn)Grid5_MV.Columns["Seleccionar"], "chkConsultarN65") as ASPxCheckBox;

                        if (chkConsultar.Checked)
                        {
                            //Abre Modal 
                            MostrarModalN65();

                            //Titulo del Modal
                            ModalTituloN65.InnerText = "Editar factura o documento, mercancia o proveedor";

                            Session["mvkeyconcepto65"] = Grid5_MV.GetRowValues(i, "mvkeyconcepto65").ToString().Trim();
                            Session["mvkeymanifestacion"] = Grid5_MV.GetRowValues(i, "mvkeymanifestacion").ToString().Trim();
                            txt_N65_ordenconcepto.Text = Grid5_MV.GetRowValues(i, "ordenconcepto65").ToString().Trim();
                            txt_N65_mercancia.Text = Grid5_MV.GetRowValues(i, "mercancia65").ToString().Trim();
                            txt_N65_factura.Text = Grid5_MV.GetRowValues(i, "factura65").ToString().Trim();
                            txt_N65_importe.Text = Grid5_MV.GetRowValues(i, "importeconcepto65").ToString().Trim();
                            txt_N65_moneda.Text = Grid5_MV.GetRowValues(i, "monedaconcepto65").ToString().Trim();
                            txt_N65_conceptocargo.Text = Grid5_MV.GetRowValues(i, "conceptodelcargo65").ToString().Trim();


                            valida_select = 1;
                            break;
                        }
                    }

                    if (valida_select == 0)
                        AlertError("Debe seleccionar un registro para poder editar");

                    Grid5_MV.Settings.VerticalScrollableHeight = 168;
                    Grid5_MV.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                    lkb_ManifestacionConcepto65_Agregar.Focus();
                }
            }
            catch (Exception ex)
            {
                AlertError(ex.Message);
            }
        }

        //Botón Eliminar
        protected void lkb_ManifestacionConcepto65_Eliminar_Click(object sender, EventArgs e)
        {
            try
            {
                int valida_select = 0;
                lkb_ManifestacionConcepto65_Agregar.Focus();

                if (Grid5_MV.VisibleRowCount > 0)
                {
                    for (int i = 0; i < Grid5_MV.VisibleRowCount; i++)
                    {
                        ASPxCheckBox chkConsultar = Grid5_MV.FindRowCellTemplateControl(i, (GridViewDataColumn)Grid5_MV.Columns["Seleccionar"], "chkConsultarN65") as ASPxCheckBox;

                        if (chkConsultar.Checked)
                        {

                            //Asignar valor al campo txt
                            Session["mvkeyconcepto65"] = Grid5_MV.GetRowValues(i, "mvkeyconcepto65").ToString().Trim();
                            Session["mvkeymanifestacion"] = Grid5_MV.GetRowValues(i, "mvkeymanifestacion").ToString().Trim();

                            valida_select = 1;

                            //Modal Question
                            string valida = "¿Desea eliminar este registro?";
                            AlertQuestionN65(valida);

                            break;
                        }
                    }

                    if (valida_select == 0)
                        AlertError("Debe seleccionar un registro para poder eliminar");

                }
            }
            catch (Exception ex)
            {
                AlertError(ex.Message);
            }
        }

        //Elimina registro en Grid8_MV
        protected void btnOkN65_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["Cadena"] == null || Session["mvkeyconcepto65"] == null || Session["Grid5_MV"] == null)
                {
                    string alerta = "<script>alert('La sesión ha expirado, debe volver a iniciar sesión');window.location.href='Login.aspx'</script>";
                    Session["Tab"] = "Salir";
                    Response.Write(alerta);
                    Response.Redirect("Login.aspx", false);
                    return;
                }


                DataTable dt = new DataTable();
                dt.Columns.Add("mvkeyconcepto65", typeof(Int64));
                dt.Columns.Add("ordenconcepto65", typeof(int));
                dt.Columns.Add("mvkeymanifestacion", typeof(Int64));
                dt.Columns.Add("mercancia65", typeof(string));
                dt.Columns.Add("factura65", typeof(string));
                dt.Columns.Add("importeconcepto65", typeof(string));
                dt.Columns.Add("monedaconcepto65", typeof(string));
                dt.Columns.Add("conceptodelcargo65", typeof(string));


                dt = ((DataTable)Session["Grid5_MV"]);

                foreach (DataRow fila in dt.Rows)
                {
                    if (!fila.RowState.ToString().Contains("Del"))
                    {
                        if (fila["mvkeyconcepto65"].ToString() == Session["mvkeyconcepto65"].ToString())
                        {
                            fila.Delete();
                            break;
                        }
                    }
                }

                //Cargar de nuevo Grid5_MV
                Grid5_MV.DataSource = Session["Grid5_MV"] = dt;
                Grid5_MV.DataBind();
                Grid5_MV.Settings.VerticalScrollableHeight = 168;
                Grid5_MV.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                //Grid4_MV.SettingsPager.PageSize = 20;

                lkb_ManifestacionConcepto65_Agregar.Focus();

                //Ejecuta funcion script para validar si habilita o no pestaña de Valor de TRansacción de las Mercancias
                ScriptManager.RegisterStartupScript(this.Page, typeof(String), "Articulo65", "<script> Articulo65(); </script> ", false);

                //Ejecuta limpieza de Grid3_MV y Grid4_MV
                btnA65_Click(this, new EventArgs());
            }
            catch (Exception ex)
            {
                AlertError(ex.Message);
            }
        }

        //Botón Guardar
        protected void btnGuardarN65_Click(object sender, EventArgs e)
        {
            try
            {

                Grid5_MV.Settings.VerticalScrollableHeight = 168;
                Grid5_MV.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                lkb_ManifestacionConcepto65_Agregar.Focus();


                #region Valida campos

                if (txt_N65_ordenconcepto.Text.Trim().Equals(string.Empty) || txt_N65_mercancia.Text.Trim().Equals(string.Empty) ||
                    txt_N65_factura.Text.Trim().Equals(string.Empty) || txt_N65_importe.Text.Trim().Equals(string.Empty) ||
                    txt_N65_moneda.Text.Trim().Equals(string.Empty) || txt_N65_conceptocargo.Text.Trim().Equals(string.Empty))
                {
                    MostrarModalN65();

                    if (txt_N65_ordenconcepto.Text.Trim().Equals(string.Empty))
                        AlertError("Enumere un concepto");
                    else if (txt_N65_mercancia.Text.Trim().Equals(string.Empty))
                        AlertError("Escriba una mercancia");
                    else if (txt_N65_factura.Text.Trim().Equals(string.Empty))
                        AlertError("Escriba una factura o documento comercial");
                    else if (txt_N65_importe.Text.Trim().Equals(string.Empty))
                        AlertError("Escriba un importe");
                    else if (txt_N65_moneda.Text.Trim().Equals(string.Empty))
                        AlertError("Escriba una moneda");
                    else if (txt_N65_conceptocargo.Text.Trim().Equals(string.Empty))
                        AlertError("Escriba un concepto de cargo");

                    return;
                }


                for (int i = 0; i < Grid5_MV.VisibleRowCount; i++)
                {
                    if (txt_N65_ordenconcepto.Text.Trim().ToUpper() == Grid5_MV.GetRowValues(i, "ordenconcepto65").ToString().Trim() &&
                        txt_N65_mercancia.Text.Trim() == Grid5_MV.GetRowValues(i, "mercancia65").ToString().Trim() &&
                        txt_N65_factura.Text.Trim() == Grid5_MV.GetRowValues(i, "factura65").ToString().Trim() &&
                        txt_N65_importe.Text.Trim() == Grid5_MV.GetRowValues(i, "importeconcepto65").ToString().Trim() &&
                        txt_N65_moneda.Text.Trim() == Grid5_MV.GetRowValues(i, "monedaconcepto65").ToString().Trim() &&
                        txt_N65_conceptocargo.Text.Trim() == Grid5_MV.GetRowValues(i, "conceptodelcargo65").ToString().Trim() && !ModalTituloN65.InnerHtml.Contains("Edit"))
                    {
                        MostrarModalS65();
                        AlertError("Ya existe ese concepto");

                        return;
                    }

                    if (txt_N65_ordenconcepto.Text.Trim().ToUpper() == Grid5_MV.GetRowValues(i, "ordenconcepto65").ToString().Trim() &&
                        txt_N65_mercancia.Text.Trim() == Grid5_MV.GetRowValues(i, "mercancia65").ToString().Trim() &&
                        txt_N65_factura.Text.Trim() == Grid5_MV.GetRowValues(i, "factura65").ToString().Trim() &&
                        txt_N65_importe.Text.Trim() == Grid5_MV.GetRowValues(i, "importeconcepto65").ToString().Trim() &&
                        txt_N65_moneda.Text.Trim() == Grid5_MV.GetRowValues(i, "monedaconcepto65").ToString().Trim() &&
                        txt_N65_conceptocargo.Text.Trim() == Grid5_MV.GetRowValues(i, "conceptodelcargo65").ToString().Trim() && ModalTituloN65.InnerHtml.Contains("Edit") &&
                        Session["mvkeyconcepto65"].ToString() != Grid5_MV.GetRowValues(i, "mvkeyconcepto65").ToString().Trim())
                    {
                        MostrarModalN65();
                        AlertError("Ya existe ese concepto");

                        return;
                    }
                }

                #endregion



                DataTable dt = new DataTable();
                dt.Columns.Add("mvkeyconcepto65", typeof(Int64));
                dt.Columns.Add("ordenconcepto65", typeof(int));
                dt.Columns.Add("mvkeymanifestacion", typeof(Int64));
                dt.Columns.Add("mercancia65", typeof(string));
                dt.Columns.Add("factura65", typeof(string));
                dt.Columns.Add("importeconcepto65", typeof(string));
                dt.Columns.Add("monedaconcepto65", typeof(string));
                dt.Columns.Add("conceptodelcargo65", typeof(string));

                if (Session["Grid5_MV"] != null)
                    dt = ((DataTable)Session["Grid5_MV"]);

                //GUARDAR
                Int64 valortop = 0;
                if (!ModalTituloN65.InnerHtml.Contains("Edit"))
                {
                    if (dt != null)
                    {
                        //Se obtiene el valor maximo de mvkeyconcepto65 + 1
                        foreach (DataRow fila in dt.Rows)
                        {
                            if (!fila.RowState.ToString().Contains("Del"))
                            {
                                if (Int64.Parse(fila["mvkeyconcepto65"].ToString()) > valortop)
                                {
                                    valortop = Int64.Parse(fila["mvkeyconcepto65"].ToString());
                                }
                            }
                        }
                    }

                    valortop += 1;

                    //Se agrega nuevo registro al dt
                    DataRow dr = dt.NewRow();
                    dr["mvkeyconcepto65"] = valortop;
                    dr["ordenconcepto65"] = txt_N65_ordenconcepto.Text.Trim();
                    dr["mvkeymanifestacion"] = Int64.Parse(Session["mvkeymanifestacion"].ToString());
                    dr["mercancia65"] = txt_N65_mercancia.Text.Trim();
                    dr["factura65"] = txt_N65_factura.Text.Trim();
                    dr["importeconcepto65"] = txt_N65_importe.Text.Trim();
                    dr["monedaconcepto65"] = txt_N65_moneda.Text.Trim();
                    dr["conceptodelcargo65"] = txt_N65_conceptocargo.Text.Trim();
                    dt.Rows.Add(dr);
                }

                //EDITAR
                else if (ModalTituloN65.InnerHtml.Contains("Edit"))
                {
                    foreach (DataRow fila in dt.Rows)
                    {
                        if (!fila.RowState.ToString().Contains("Del"))
                        {
                            if (fila["mvkeyconcepto65"].ToString() == Session["mvkeyconcepto65"].ToString())
                            {
                                fila["ordenconcepto65"] = txt_N65_ordenconcepto.Text.Trim();
                                fila["mercancia65"] = txt_N65_mercancia.Text.Trim();
                                fila["factura65"] = txt_N65_factura.Text.Trim();
                                fila["importeconcepto65"] = txt_N65_importe.Text.Trim();
                                fila["monedaconcepto65"] = txt_N65_moneda.Text.Trim();
                                fila["conceptodelcargo65"] = txt_N65_conceptocargo.Text.Trim();
                            }
                        }
                    }
                }


                Grid5_MV.DataSource = Session["Grid5_MV"] = dt;
                Grid5_MV.DataBind();
                Grid5_MV.Settings.VerticalScrollableHeight = 168;
                Grid5_MV.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;

                //Ejecuta funcion script para validar si habilita o no pestaña de Valor de TRansacción de las Mercancias
                ScriptManager.RegisterStartupScript(this.Page, typeof(String), "Articulo65", "<script> Articulo65(); </script> ", false);

                //Ejecuta limpieza de Grid3_MV y Grid4_MV
                btnA65_Click(this, new EventArgs());
            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "IA_ManifestacionValor-btnGuardarN65_Click", ex, lblCadena.Text, ref mensaje);
                AlertError(ex.Message);
            }
        }

        //Botón Cancelar Factura
        protected void btnCancelarN65_Click(object sender, EventArgs e)
        {
            try
            {
                Grid5_MV.Settings.VerticalScrollableHeight = 168;
                Grid5_MV.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;

                lkb_ManifestacionConcepto65_Agregar.Focus();
            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "IA_ManifestacionValor-btnCancelarN65_Click", ex, lblCadena.Text, ref mensaje);
            }
        }


        //Se ejecuta por función en javascript para borrar si tienen datos el Grid3_MV y el Grid4_MV
        protected void btnA65_Click(object sender, EventArgs e)
        {
            Grid3_MV.DataSource = Session["Grid3_MV"] = null;
            Grid3_MV.DataBind();
            Grid3_MV.Settings.VerticalScrollableHeight = 168;
            Grid3_MV.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
            Grid3_MV.SettingsPager.PageSize = 20;

            Grid4_MV.DataSource = Session["Grid4_MV"] = null;
            Grid4_MV.DataBind();
            Grid4_MV.Settings.VerticalScrollableHeight = 168;
            Grid4_MV.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;

            //Ejecuta funcion script para validar si habilita o no pestaña de Valor de TRansacción de las Mercancias
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "Articulo65", "<script> Articulo65(); </script> ", false);

            Session["pestaña"] = "A65";
        }
        #endregion



        #region OTROS ARTICULOS CONFORME A LA LEY

        //OT
        protected void Grid6_MV_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            GridPageSize6 = int.Parse(e.Parameters);
            Grid6_MV.SettingsPager.PageSize = GridPageSize6;
            Grid6_MV.DataBind();
        }

        protected void chkConsultarOT_Init(object sender, EventArgs e)
        {
            try
            {
                ASPxCheckBox chb = sender as ASPxCheckBox;
                GridViewDataItemTemplateContainer container = chb.NamingContainer as GridViewDataItemTemplateContainer;

                chb.ClientInstanceName = String.Format("chkConsultarOT{0}", container.VisibleIndex);
                chb.ClientSideEvents.CheckedChanged = String.Format("function (s, e) {{ chkConsultarOTClick(s, e, {0}); }}", container.VisibleIndex);
            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "IA_ManifestacionValor-chkConsultarOT_Init", ex, lblCadena.Text, ref mensaje);
            }
        }

        //Botón Agregar
        protected void lkb_ManifestacionConcepto67_Agregar_Click(object sender, EventArgs e)
        {
            try
            {
                MostrarModalOT();

                ModalTituloOT.InnerHtml = "Agregar valor conforme a los artículos 72 a 78 de la ley";

                //Limpiar controles
                txt_OT_ordenconcepto.Text = string.Empty;
                txt_OT_mercancia.Text = string.Empty;
                txt_OT_valordeterminado.Text = string.Empty;
                txt_OT_metodovalorutilizado.Text = string.Empty;
                txt_OT_motivodemetodo.Text = string.Empty;

                Grid6_MV.Settings.VerticalScrollableHeight = 168;
                Grid6_MV.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;


                lkb_ManifestacionConcepto67_Agregar.Focus();


            }
            catch (Exception ex)
            {
                AlertError(ex.Message);
            }
        }

        //Botón Editar
        protected void lkb_ManifestacionConcepto67_Editar_Click(object sender, EventArgs e)
        {
            try
            {
                int valida_select = 0;

                if (Grid6_MV.VisibleRowCount > 0)
                {
                    for (int i = 0; i < Grid6_MV.VisibleRowCount; i++)
                    {
                        ASPxCheckBox chkConsultar = Grid6_MV.FindRowCellTemplateControl(i, (GridViewDataColumn)Grid6_MV.Columns["Seleccionar"], "chkConsultarOT") as ASPxCheckBox;

                        if (chkConsultar.Checked)
                        {
                            //Abre Modal 
                            MostrarModalOT();

                            //Titulo del Modal
                            ModalTituloOT.InnerText = "Editar valor conforme a los artículos 72 a 78 de la ley";

                            Session["mvkeyconcepto67"] = Grid6_MV.GetRowValues(i, "mvkeyconcepto67").ToString().Trim();
                            Session["mvkeymanifestacion"] = Grid6_MV.GetRowValues(i, "mvkeymanifestacion").ToString().Trim();
                            txt_OT_ordenconcepto.Text = Grid6_MV.GetRowValues(i, "ordenconcepto").ToString().Trim();
                            txt_OT_mercancia.Text = Grid6_MV.GetRowValues(i, "mercancia").ToString().Trim();
                            txt_OT_valordeterminado.Text = Grid6_MV.GetRowValues(i, "valordeterminado").ToString().Trim();
                            txt_OT_metodovalorutilizado.Text = Grid6_MV.GetRowValues(i, "metodovalorutilizado").ToString().Trim();
                            txt_OT_motivodemetodo.Text = Grid6_MV.GetRowValues(i, "motivodemetodo").ToString().Trim();

                            valida_select = 1;
                            break;
                        }
                    }

                    if (valida_select == 0)
                        AlertError("Debe seleccionar un registro para poder editar");

                    Grid6_MV.Settings.VerticalScrollableHeight = 168;
                    Grid6_MV.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                    lkb_ManifestacionConcepto67_Agregar.Focus();
                }
            }
            catch (Exception ex)
            {
                AlertError(ex.Message);
            }
        }

        //Botón Eliminar
        protected void lkb_ManifestacionConcepto67_Eliminar_Click(object sender, EventArgs e)
        {
            try
            {
                int valida_select = 0;
                lkb_ManifestacionConcepto67_Agregar.Focus();

                if (Grid6_MV.VisibleRowCount > 0)
                {
                    for (int i = 0; i < Grid6_MV.VisibleRowCount; i++)
                    {
                        ASPxCheckBox chkConsultar = Grid6_MV.FindRowCellTemplateControl(i, (GridViewDataColumn)Grid6_MV.Columns["Seleccionar"], "chkConsultarOT") as ASPxCheckBox;

                        if (chkConsultar.Checked)
                        {

                            //Asignar valor al campo txt
                            Session["mvkeyconcepto67"] = Grid6_MV.GetRowValues(i, "mvkeyconcepto67").ToString().Trim();
                            Session["mvkeymanifestacion"] = Grid6_MV.GetRowValues(i, "mvkeymanifestacion").ToString().Trim();

                            valida_select = 1;

                            //Modal Question
                            string valida = "¿Desea eliminar este registro?";
                            AlertQuestionOT(valida);

                            break;
                        }
                    }

                    if (valida_select == 0)
                        AlertError("Debe seleccionar un registro para poder eliminar");

                }
            }
            catch (Exception ex)
            {
                AlertError(ex.Message);
            }
        }

        //Elimina registro en Grid8_MV
        protected void btnOkOT_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["Cadena"] == null || Session["mvkeyconcepto67"] == null || Session["Grid6_MV"] == null)
                {
                    string alerta = "<script>alert('La sesión ha expirado, debe volver a iniciar sesión');window.location.href='Login.aspx'</script>";
                    Session["Tab"] = "Salir";
                    Response.Write(alerta);
                    Response.Redirect("Login.aspx", false);
                    return;
                }


                DataTable dt = new DataTable();
                dt.Columns.Add("mvkeyconcepto67", typeof(Int64));
                dt.Columns.Add("ordenconcepto", typeof(int));
                dt.Columns.Add("mvkeymanifestacion", typeof(Int64));
                dt.Columns.Add("mercancia", typeof(string));
                dt.Columns.Add("valordeterminado", typeof(string));
                dt.Columns.Add("metodovalorutilizado", typeof(string));
                dt.Columns.Add("motivodemetodo", typeof(string));


                dt = ((DataTable)Session["Grid6_MV"]);

                foreach (DataRow fila in dt.Rows)
                {
                    if (!fila.RowState.ToString().Contains("Del"))
                    {
                        if (fila["mvkeyconcepto67"].ToString() == Session["mvkeyconcepto67"].ToString())
                        {
                            fila.Delete();
                            break;
                        }
                    }
                }

                //Cargar de nuevo Grid5_MV
                Grid6_MV.DataSource = Session["Grid6_MV"] = dt;
                Grid6_MV.DataBind();
                Grid6_MV.Settings.VerticalScrollableHeight = 168;
                Grid6_MV.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                //Grid4_MV.SettingsPager.PageSize = 20;

                lkb_ManifestacionConcepto67_Agregar.Focus();
            }
            catch (Exception ex)
            {
                AlertError(ex.Message);
            }
        }

        //Botón Guardar
        protected void btnGuardarOT_Click(object sender, EventArgs e)
        {
            try
            {

                Grid6_MV.Settings.VerticalScrollableHeight = 168;
                Grid6_MV.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                lkb_ManifestacionConcepto67_Agregar.Focus();


                #region Valida campos

                if (txt_OT_ordenconcepto.Text.Trim().Equals(string.Empty) || txt_OT_mercancia.Text.Trim().Equals(string.Empty) ||
                    txt_OT_valordeterminado.Text.Trim().Equals(string.Empty) || txt_OT_metodovalorutilizado.Text.Trim().Equals(string.Empty) || 
                    txt_OT_motivodemetodo.Text.Trim().Equals(string.Empty))
                {
                    MostrarModalOT();

                    if (txt_OT_ordenconcepto.Text.Trim().Equals(string.Empty))
                        AlertError("Enumere una orden concepto");
                    else if (txt_OT_mercancia.Text.Trim().Equals(string.Empty))
                        AlertError("Escriba una mercancia");
                    else if (txt_OT_valordeterminado.Text.Trim().Equals(string.Empty))
                        AlertError("Escriba un valor determinado");
                    else if (txt_OT_metodovalorutilizado.Text.Trim().Equals(string.Empty))
                        AlertError("Escriba un método utilizado");
                    else if (txt_OT_motivodemetodo.Text.Trim().Equals(string.Empty))
                        AlertError("Escriba una motivo del método");
                    
                    return;
                }


                for (int i = 0; i < Grid6_MV.VisibleRowCount; i++)
                {
                    if (txt_OT_ordenconcepto.Text.Trim().ToUpper() == Grid6_MV.GetRowValues(i, "ordenconcepto").ToString().Trim() &&
                        txt_OT_mercancia.Text.Trim() == Grid6_MV.GetRowValues(i, "mercancia").ToString().Trim() &&
                        txt_OT_valordeterminado.Text.Trim() == Grid6_MV.GetRowValues(i, "valordeterminado").ToString().Trim() &&
                        txt_OT_metodovalorutilizado.Text.Trim() == Grid6_MV.GetRowValues(i, "metodovalorutilizado").ToString().Trim() &&
                        txt_OT_motivodemetodo.Text.Trim() == Grid6_MV.GetRowValues(i, "motivodemetodo").ToString().Trim() && !ModalTituloOT.InnerHtml.Contains("Edit"))
                    {
                        MostrarModalOT();
                        AlertError("Ya existe ese concepto");

                        return;
                    }

                    if (txt_OT_ordenconcepto.Text.Trim().ToUpper() == Grid6_MV.GetRowValues(i, "ordenconcepto").ToString().Trim() &&
                        txt_OT_mercancia.Text.Trim() == Grid6_MV.GetRowValues(i, "mercancia").ToString().Trim() &&
                        txt_OT_valordeterminado.Text.Trim() == Grid6_MV.GetRowValues(i, "valordeterminado").ToString().Trim() &&
                        txt_OT_metodovalorutilizado.Text.Trim() == Grid6_MV.GetRowValues(i, "metodovalorutilizado").ToString().Trim() &&
                        txt_OT_motivodemetodo.Text.Trim() == Grid6_MV.GetRowValues(i, "motivodemetodo").ToString().Trim() && ModalTituloOT.InnerHtml.Contains("Edit") &&
                        Session["mvkeyconcepto67"].ToString() != Grid5_MV.GetRowValues(i, "mvkeyconcepto67").ToString().Trim())
                    {
                        MostrarModalOT();
                        AlertError("Ya existe ese concepto");

                        return;
                    }
                }

                #endregion



                DataTable dt = new DataTable();
                dt.Columns.Add("mvkeyconcepto67", typeof(Int64));
                dt.Columns.Add("ordenconcepto", typeof(int));
                dt.Columns.Add("mvkeymanifestacion", typeof(Int64));
                dt.Columns.Add("mercancia", typeof(string));
                dt.Columns.Add("valordeterminado", typeof(string));
                dt.Columns.Add("metodovalorutilizado", typeof(string));
                dt.Columns.Add("motivodemetodo", typeof(string));

                if (Session["Grid6_MV"] != null)
                    dt = ((DataTable)Session["Grid6_MV"]);

                //GUARDAR
                Int64 valortop = 0;
                if (!ModalTituloOT.InnerHtml.Contains("Edit"))
                {
                    if (dt != null)
                    {
                        //Se obtiene el valor maximo de mvkeyconcepto67 + 1
                        foreach (DataRow fila in dt.Rows)
                        {
                            if (!fila.RowState.ToString().Contains("Del"))
                            {
                                if (Int64.Parse(fila["mvkeyconcepto67"].ToString()) > valortop)
                                {
                                    valortop = Int64.Parse(fila["mvkeyconcepto67"].ToString());
                                }
                            }
                        }
                    }

                    valortop += 1;

                    //Se agrega nuevo registro al dt
                    DataRow dr = dt.NewRow();
                    dr["mvkeyconcepto67"] = valortop;
                    dr["ordenconcepto"] = txt_OT_ordenconcepto.Text.Trim();
                    dr["mvkeymanifestacion"] = Int64.Parse(Session["mvkeymanifestacion"].ToString());
                    dr["mercancia"] = txt_OT_mercancia.Text.Trim();
                    dr["valordeterminado"] = txt_OT_valordeterminado.Text.Trim();
                    dr["metodovalorutilizado"] = txt_OT_metodovalorutilizado.Text.Trim();
                    dr["motivodemetodo"] = txt_OT_motivodemetodo.Text.Trim();
                    dt.Rows.Add(dr);
                }

                //EDITAR
                else if (ModalTituloOT.InnerHtml.Contains("Edit"))
                {
                    foreach (DataRow fila in dt.Rows)
                    {
                        if (!fila.RowState.ToString().Contains("Del"))
                        {
                            if (fila["mvkeyconcepto67"].ToString() == Session["mvkeyconcepto67"].ToString())
                            {
                                fila["ordenconcepto"] = txt_OT_ordenconcepto.Text.Trim();
                                fila["mercancia"] = txt_OT_mercancia.Text.Trim();
                                fila["valordeterminado"] = txt_OT_valordeterminado.Text.Trim();
                                fila["metodovalorutilizado"] = txt_OT_metodovalorutilizado.Text.Trim();
                                fila["motivodemetodo"] = txt_OT_motivodemetodo.Text.Trim();
                            }
                        }
                    }
                }


                Grid6_MV.DataSource = Session["Grid6_MV"] = dt;
                Grid6_MV.DataBind();
                Grid6_MV.Settings.VerticalScrollableHeight = 168;
                Grid6_MV.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "IA_ManifestacionValor-btnGuardarOT_Click", ex, lblCadena.Text, ref mensaje);
                AlertError(ex.Message);
            }
        }

        //Botón Cancelar Factura
        protected void btnCancelarOT_Click(object sender, EventArgs e)
        {
            try
            {
                Grid6_MV.Settings.VerticalScrollableHeight = 168;
                Grid6_MV.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;

                lkb_ManifestacionConcepto67_Agregar.Focus();
            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "IA_ManifestacionValor-btnCancelarOT_Click", ex, lblCadena.Text, ref mensaje);
            }
        }

        //OT2
        protected void Grid7_MV_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            GridPageSize7 = int.Parse(e.Parameters);
            Grid7_MV.SettingsPager.PageSize = GridPageSize7;
            Grid7_MV.DataBind();
        }

        protected void chkConsultarOT2_Init(object sender, EventArgs e)
        {
            try
            {
                ASPxCheckBox chb = sender as ASPxCheckBox;
                GridViewDataItemTemplateContainer container = chb.NamingContainer as GridViewDataItemTemplateContainer;

                chb.ClientInstanceName = String.Format("chkConsultarOT2{0}", container.VisibleIndex);
                chb.ClientSideEvents.CheckedChanged = String.Format("function (s, e) {{ chkConsultarOT2Click(s, e, {0}); }}", container.VisibleIndex);
            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "IA_ManifestacionValor-chkConsultarOT2_Init", ex, lblCadena.Text, ref mensaje);
            }
        }

        //Botón Agregar
        protected void lkb_ManifestacionValorAduana_Agregar_Click(object sender, EventArgs e)
        {
            try
            {
                MostrarModalOT2();

                ModalTituloOT2.InnerHtml = "Agregar número y mercancia correspondiente al valor aduana respectivo";

                //Limpiar controles
                txt_OT2_numerodocumentoasignado.Text = string.Empty;
                txt_OT2_mercanciarelacion.Text = string.Empty;

                Grid7_MV.Settings.VerticalScrollableHeight = 168;
                Grid7_MV.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;


                lkb_ManifestacionValorAduana_Agregar.Focus();


            }
            catch (Exception ex)
            {
                AlertError(ex.Message);
            }
        }

        //Botón Editar
        protected void lkb_ManifestacionValorAduana_Editar_Click(object sender, EventArgs e)
        {
            try
            {
                int valida_select = 0;

                if (Grid7_MV.VisibleRowCount > 0)
                {
                    for (int i = 0; i < Grid7_MV.VisibleRowCount; i++)
                    {
                        ASPxCheckBox chkConsultar = Grid7_MV.FindRowCellTemplateControl(i, (GridViewDataColumn)Grid7_MV.Columns["Seleccionar"], "chkConsultarOT2") as ASPxCheckBox;

                        if (chkConsultar.Checked)
                        {
                            //Abre Modal 
                            MostrarModalOT2();

                            //Titulo del Modal
                            ModalTituloOT2.InnerText = "Editar número y mercancia correspondiente al valor aduana respectivo";

                            Session["mvkeyanexovaloraduana"] = Grid7_MV.GetRowValues(i, "mvkeyanexovaloraduana").ToString().Trim();
                            Session["mvkeymanifestacion"] = Grid7_MV.GetRowValues(i, "mvkeymanifestacion").ToString().Trim();
                            txt_OT2_numerodocumentoasignado.Text = Grid7_MV.GetRowValues(i, "numerodocumentoasignado").ToString().Trim();
                            txt_OT2_mercanciarelacion.Text = Grid7_MV.GetRowValues(i, "mercanciarelacion").ToString().Trim();

                            valida_select = 1;
                            break;
                        }
                    }

                    if (valida_select == 0)
                        AlertError("Debe seleccionar un registro para poder editar");

                    Grid7_MV.Settings.VerticalScrollableHeight = 168;
                    Grid7_MV.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                    lkb_ManifestacionValorAduana_Agregar.Focus();
                }
            }
            catch (Exception ex)
            {
                AlertError(ex.Message);
            }
        }

        //Botón Eliminar
        protected void lkb_ManifestacionValorAduana_Eliminar_Click(object sender, EventArgs e)
        {
            try
            {
                int valida_select = 0;
                lkb_ManifestacionValorAduana_Agregar.Focus();

                if (Grid7_MV.VisibleRowCount > 0)
                {
                    for (int i = 0; i < Grid7_MV.VisibleRowCount; i++)
                    {
                        ASPxCheckBox chkConsultar = Grid7_MV.FindRowCellTemplateControl(i, (GridViewDataColumn)Grid7_MV.Columns["Seleccionar"], "chkConsultarOT2") as ASPxCheckBox;

                        if (chkConsultar.Checked)
                        {

                            //Asignar valor al campo txt
                            Session["mvkeyanexovaloraduana"] = Grid7_MV.GetRowValues(i, "mvkeyanexovaloraduana").ToString().Trim();
                            Session["mvkeymanifestacion"] = Grid7_MV.GetRowValues(i, "mvkeymanifestacion").ToString().Trim();

                            valida_select = 1;

                            //Modal Question
                            string valida = "¿Desea eliminar este registro?";
                            AlertQuestionOT2(valida);

                            break;
                        }
                    }

                    if (valida_select == 0)
                        AlertError("Debe seleccionar un registro para poder eliminar");

                }
            }
            catch (Exception ex)
            {
                AlertError(ex.Message);
            }
        }

        //Elimina registro en Grid7_MV
        protected void btnOkOT2_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["Cadena"] == null || Session["mvkeyanexovaloraduana"] == null || Session["Grid7_MV"] == null)
                {
                    string alerta = "<script>alert('La sesión ha expirado, debe volver a iniciar sesión');window.location.href='Login.aspx'</script>";
                    Session["Tab"] = "Salir";
                    Response.Write(alerta);
                    Response.Redirect("Login.aspx", false);
                    return;
                }


                DataTable dt = new DataTable();
                dt.Columns.Add("mvkeyanexovaloraduana", typeof(Int64));
                dt.Columns.Add("numerodocumentoasignado", typeof(string));
                dt.Columns.Add("mvkeymanifestacion", typeof(Int64));
                dt.Columns.Add("mercanciarelacion", typeof(string));


                dt = ((DataTable)Session["Grid7_MV"]);

                foreach (DataRow fila in dt.Rows)
                {
                    if (!fila.RowState.ToString().Contains("Del"))
                    {
                        if (fila["mvkeyanexovaloraduana"].ToString() == Session["mvkeyanexovaloraduana"].ToString())
                        {
                            fila.Delete();
                            break;
                        }
                    }
                }

                //Cargar de nuevo Grid7_MV
                Grid7_MV.DataSource = Session["Grid7_MV"] = dt;
                Grid7_MV.DataBind();
                Grid7_MV.Settings.VerticalScrollableHeight = 168;
                Grid7_MV.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                //Grid4_MV.SettingsPager.PageSize = 20;

                lkb_ManifestacionValorAduana_Agregar.Focus();
            }
            catch (Exception ex)
            {
                AlertError(ex.Message);
            }
        }

        //Botón Guardar
        protected void btnGuardarOT2_Click(object sender, EventArgs e)
        {
            try
            {

                Grid7_MV.Settings.VerticalScrollableHeight = 168;
                Grid7_MV.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                lkb_ManifestacionValorAduana_Agregar.Focus();


                #region Valida campos

                if (txt_OT2_numerodocumentoasignado.Text.Trim().Equals(string.Empty) || txt_OT2_mercanciarelacion.Text.Trim().Equals(string.Empty))
                {
                    MostrarModalOT();

                    if (txt_OT2_numerodocumentoasignado.Text.Trim().Equals(string.Empty))
                        AlertError("Escriba un no. designado al documento anexado");
                    else if (txt_OT2_mercanciarelacion.Text.Trim().Equals(string.Empty))
                        AlertError("Escriba una mercancia con la que se relaciona");                    

                    return;
                }


                for (int i = 0; i < Grid7_MV.VisibleRowCount; i++)
                {
                    if (txt_OT2_numerodocumentoasignado.Text.Trim().ToUpper() == Grid7_MV.GetRowValues(i, "numerodocumentoasignado").ToString().Trim() &&
                        txt_OT2_mercanciarelacion.Text.Trim() == Grid7_MV.GetRowValues(i, "mercanciarelacion").ToString().Trim() && !ModalTituloOT2.InnerHtml.Contains("Edit"))
                    {
                        MostrarModalOT2();
                        AlertError("Ya existe esa mercancia");

                        return;
                    }

                    if (txt_OT2_numerodocumentoasignado.Text.Trim().ToUpper() == Grid7_MV.GetRowValues(i, "numerodocumentoasignado").ToString().Trim() &&
                        txt_OT2_mercanciarelacion.Text.Trim() == Grid7_MV.GetRowValues(i, "mercanciarelacion").ToString().Trim() && ModalTituloOT2.InnerHtml.Contains("Edit") &&
                        Session["mvkeyanexovaloraduana"].ToString() != Grid7_MV.GetRowValues(i, "mvkeyanexovaloraduana").ToString().Trim())
                    {
                        MostrarModalOT2();
                        AlertError("Ya existe esa mercancia");

                        return;
                    }
                }

                #endregion



                DataTable dt = new DataTable();
                dt.Columns.Add("mvkeyanexovaloraduana", typeof(Int64));
                dt.Columns.Add("numerodocumentoasignado", typeof(string));
                dt.Columns.Add("mvkeymanifestacion", typeof(Int64));
                dt.Columns.Add("mercanciarelacion", typeof(string));


                if (Session["Grid7_MV"] != null)
                    dt = ((DataTable)Session["Grid7_MV"]);

                //GUARDAR
                Int64 valortop = 0;
                if (!ModalTituloOT2.InnerHtml.Contains("Edit"))
                {
                    if (dt != null)
                    {
                        //Se obtiene el valor maximo de mvkeyanexovaloraduana + 1
                        foreach (DataRow fila in dt.Rows)
                        {
                            if (!fila.RowState.ToString().Contains("Del"))
                            {
                                if (Int64.Parse(fila["mvkeyanexovaloraduana"].ToString()) > valortop)
                                {
                                    valortop = Int64.Parse(fila["mvkeyanexovaloraduana"].ToString());
                                }
                            }
                        }
                    }

                    valortop += 1;

                    //Se agrega nuevo registro al dt
                    DataRow dr = dt.NewRow();
                    dr["mvkeyanexovaloraduana"] = valortop;
                    dr["numerodocumentoasignado"] = txt_OT2_numerodocumentoasignado.Text.Trim();
                    dr["mvkeymanifestacion"] = Int64.Parse(Session["mvkeymanifestacion"].ToString());
                    dr["mercanciarelacion"] = txt_OT2_mercanciarelacion.Text.Trim();                    
                    dt.Rows.Add(dr);
                }

                //EDITAR
                else if (ModalTituloOT2.InnerHtml.Contains("Edit"))
                {
                    foreach (DataRow fila in dt.Rows)
                    {
                        if (!fila.RowState.ToString().Contains("Del"))
                        {
                            if (fila["mvkeyanexovaloraduana"].ToString() == Session["mvkeyanexovaloraduana"].ToString())
                            {
                                fila["numerodocumentoasignado"] = txt_OT2_numerodocumentoasignado.Text.Trim();
                                fila["mercanciarelacion"] = txt_OT2_mercanciarelacion.Text.Trim();                                
                            }
                        }
                    }
                }


                Grid7_MV.DataSource = Session["Grid7_MV"] = dt;
                Grid7_MV.DataBind();
                Grid7_MV.Settings.VerticalScrollableHeight = 168;
                Grid7_MV.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "IA_ManifestacionValor-btnGuardarOT2_Click", ex, lblCadena.Text, ref mensaje);
                AlertError(ex.Message);
            }
        }

        //Botón Cancelar Factura
        protected void btnCancelarOT2_Click(object sender, EventArgs e)
        {
            try
            {
                Grid7_MV.Settings.VerticalScrollableHeight = 168;
                Grid7_MV.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;

                lkb_ManifestacionValorAduana_Agregar.Focus();
            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "IA_ManifestacionValor-btnCancelarOT2_Click", ex, lblCadena.Text, ref mensaje);
            }
        }


        #endregion



        #region IMPORTACION TEMPORAL

        protected void Grid9_MV_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            GridPageSize9 = int.Parse(e.Parameters);
            Grid9_MV.SettingsPager.PageSize = GridPageSize9;
            Grid9_MV.DataBind();
        }

        protected void chkConsultarIT_Init(object sender, EventArgs e)
        {
            try
            {
                ASPxCheckBox chb = sender as ASPxCheckBox;
                GridViewDataItemTemplateContainer container = chb.NamingContainer as GridViewDataItemTemplateContainer;

                chb.ClientInstanceName = String.Format("chkConsultarIT{0}", container.VisibleIndex);
                chb.ClientSideEvents.CheckedChanged = String.Format("function (s, e) {{ chkConsultarITClick(s, e, {0}); }}", container.VisibleIndex);
            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "IA_ManifestacionValor-chkConsultarIT_Init", ex, lblCadena.Text, ref mensaje);
            }
        }

        //Botón Agregar
        protected void lkb_ManifestacionMercanciaProvisional_Agregar_Click(object sender, EventArgs e)
        {
            try
            {
                MostrarModalIT();

                ModalTituloIT.InnerHtml = "Agregar tipo mercancia y valor provisional";

                //Limpiar controles
                txt_IT_tipomercancia.Text = string.Empty;
                txt_IT_valorprovisional.Text = string.Empty;

                Grid9_MV.Settings.VerticalScrollableHeight = 168;
                Grid9_MV.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;


                lkb_ManifestacionMercanciaProvisional_Agregar.Focus();

            }
            catch (Exception ex)
            {
                AlertError(ex.Message);
            }
        }

        //Botón Editar
        protected void lkb_ManifestacionMercanciaProvisional_Editar_Click(object sender, EventArgs e)
        {
            try
            {
                int valida_select = 0;

                if (Grid9_MV.VisibleRowCount > 0)
                {
                    for (int i = 0; i < Grid9_MV.VisibleRowCount; i++)
                    {
                        ASPxCheckBox chkConsultar = Grid9_MV.FindRowCellTemplateControl(i, (GridViewDataColumn)Grid9_MV.Columns["Seleccionar"], "chkConsultarIT") as ASPxCheckBox;

                        if (chkConsultar.Checked)
                        {
                            //Abre Modal 
                            MostrarModalIT();

                            //Titulo del Modal
                            ModalTituloIT.InnerText = "Editar tipo mercancia y valor provisional";

                            Session["mvkeymercanciaprovisional"] = Grid9_MV.GetRowValues(i, "mvkeymercanciaprovisional").ToString().Trim();
                            Session["mvkeymanifestacion"] = Grid9_MV.GetRowValues(i, "mvkeymanifestacion").ToString().Trim();
                            txt_IT_tipomercancia.Text = Grid9_MV.GetRowValues(i, "tipomercancia").ToString().Trim();
                            txt_IT_valorprovisional.Text = Grid9_MV.GetRowValues(i, "valorprovisional").ToString().Trim();

                            valida_select = 1;
                            break;
                        }
                    }

                    if (valida_select == 0)
                        AlertError("Debe seleccionar un registro para poder editar");

                    Grid9_MV.Settings.VerticalScrollableHeight = 168;
                    Grid9_MV.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                    lkb_ManifestacionMercanciaProvisional_Agregar.Focus();
                }
            }
            catch (Exception ex)
            {
                AlertError(ex.Message);
            }
        }

        //Botón Eliminar
        protected void lkb_ManifestacionMercanciaProvisional_Eliminar_Click(object sender, EventArgs e)
        {
            try
            {
                int valida_select = 0;
                lkb_ManifestacionMercanciaProvisional_Agregar.Focus();

                if (Grid9_MV.VisibleRowCount > 0)
                {
                    for (int i = 0; i < Grid9_MV.VisibleRowCount; i++)
                    {
                        ASPxCheckBox chkConsultar = Grid9_MV.FindRowCellTemplateControl(i, (GridViewDataColumn)Grid9_MV.Columns["Seleccionar"], "chkConsultarIT") as ASPxCheckBox;

                        if (chkConsultar.Checked)
                        {

                            //Asignar valor al campo txt
                            Session["mvkeymercanciaprovisional"] = Grid9_MV.GetRowValues(i, "mvkeymercanciaprovisional").ToString().Trim();
                            Session["mvkeymanifestacion"] = Grid9_MV.GetRowValues(i, "mvkeymanifestacion").ToString().Trim();

                            valida_select = 1;

                            //Modal Question
                            string valida = "¿Desea eliminar este registro?";
                            AlertQuestionIT(valida);

                            break;
                        }
                    }

                    if (valida_select == 0)
                        AlertError("Debe seleccionar un registro para poder eliminar");

                }
            }
            catch (Exception ex)
            {
                AlertError(ex.Message);
            }
        }

        //Elimina registro en Grid7_MV
        protected void btnOkIT_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["Cadena"] == null || Session["mvkeymercanciaprovisional"] == null || Session["Grid9_MV"] == null)
                {
                    string alerta = "<script>alert('La sesión ha expirado, debe volver a iniciar sesión');window.location.href='Login.aspx'</script>";
                    Session["Tab"] = "Salir";
                    Response.Write(alerta);
                    Response.Redirect("Login.aspx", false);
                    return;
                }


                DataTable dt = new DataTable();
                dt.Columns.Add("mvkeymercanciaprovisional", typeof(Int64));
                dt.Columns.Add("tipomercancia", typeof(string));
                dt.Columns.Add("mvkeymanifestacion", typeof(Int64));
                dt.Columns.Add("valorprovisional", typeof(string));


                dt = ((DataTable)Session["Grid9_MV"]);

                foreach (DataRow fila in dt.Rows)
                {
                    if (!fila.RowState.ToString().Contains("Del"))
                    {
                        if (fila["mvkeymercanciaprovisional"].ToString() == Session["mvkeymercanciaprovisional"].ToString())
                        {
                            fila.Delete();
                            break;
                        }
                    }
                }

                //Cargar de nuevo Grid9_MV
                Grid9_MV.DataSource = Session["Grid9_MV"] = dt;
                Grid9_MV.DataBind();
                Grid9_MV.Settings.VerticalScrollableHeight = 168;
                Grid9_MV.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                //Grid9_MV.SettingsPager.PageSize = 20;

                lkb_ManifestacionMercanciaProvisional_Agregar.Focus();
            }
            catch (Exception ex)
            {
                AlertError(ex.Message);
            }
        }

        //Botón Guardar
        protected void btnGuardarIT_Click(object sender, EventArgs e)
        {
            try
            {

                Grid9_MV.Settings.VerticalScrollableHeight = 168;
                Grid9_MV.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                lkb_ManifestacionMercanciaProvisional_Agregar.Focus();


                #region Valida campos

                if (txt_IT_tipomercancia.Text.Trim().Equals(string.Empty) || txt_IT_valorprovisional.Text.Trim().Equals(string.Empty))
                {
                    MostrarModalIT();

                    if (txt_IT_tipomercancia.Text.Trim().Equals(string.Empty))
                        AlertError("Escriba un tipo de mercancia");
                    else if (txt_IT_valorprovisional.Text.Trim().Equals(string.Empty))
                        AlertError("Escriba un valor provisional");

                    return;
                }


                for (int i = 0; i < Grid9_MV.VisibleRowCount; i++)
                {
                    if (txt_IT_tipomercancia.Text.Trim().ToUpper() == Grid9_MV.GetRowValues(i, "tipomercancia").ToString().Trim() &&
                        txt_IT_valorprovisional.Text.Trim() == Grid9_MV.GetRowValues(i, "valorprovisional").ToString().Trim() && !ModalTituloIT.InnerHtml.Contains("Edit"))
                    {
                        MostrarModalOT2();
                        AlertError("Ya existe ese tipo de mercancia y valor provisional");

                        return;
                    }

                    if (txt_IT_tipomercancia.Text.Trim().ToUpper() == Grid9_MV.GetRowValues(i, "tipomercancia").ToString().Trim() &&
                        txt_IT_valorprovisional.Text.Trim() == Grid9_MV.GetRowValues(i, "valorprovisional").ToString().Trim() && ModalTituloIT.InnerHtml.Contains("Edit") &&
                        Session["mvkeymercanciaprovisional"].ToString() != Grid9_MV.GetRowValues(i, "mvkeymercanciaprovisional").ToString().Trim())
                    {
                        MostrarModalIT();
                        AlertError("Ya existe ese tipo de mercancia y valor provisional");

                        return;
                    }
                }

                #endregion



                DataTable dt = new DataTable();
                dt.Columns.Add("mvkeymercanciaprovisional", typeof(Int64));
                dt.Columns.Add("tipomercancia", typeof(string));
                dt.Columns.Add("mvkeymanifestacion", typeof(Int64));
                dt.Columns.Add("valorprovisional", typeof(string));


                if (Session["Grid9_MV"] != null)
                    dt = ((DataTable)Session["Grid9_MV"]);

                //GUARDAR
                Int64 valortop = 0;
                if (!ModalTituloIT.InnerHtml.Contains("Edit"))
                {
                    if (dt != null)
                    {
                        //Se obtiene el valor maximo de mvkeyanexovaloraduana + 1
                        foreach (DataRow fila in dt.Rows)
                        {
                            if (!fila.RowState.ToString().Contains("Del"))
                            {
                                if (Int64.Parse(fila["mvkeymercanciaprovisional"].ToString()) > valortop)
                                {
                                    valortop = Int64.Parse(fila["mvkeymercanciaprovisional"].ToString());
                                }
                            }
                        }
                    }

                    valortop += 1;

                    //Se agrega nuevo registro al dt
                    DataRow dr = dt.NewRow();
                    dr["mvkeymercanciaprovisional"] = valortop;
                    dr["tipomercancia"] = txt_IT_tipomercancia.Text.Trim();
                    dr["mvkeymanifestacion"] = Int64.Parse(Session["mvkeymanifestacion"].ToString());
                    dr["valorprovisional"] = txt_IT_valorprovisional.Text.Trim();
                    dt.Rows.Add(dr);
                }

                //EDITAR
                else if (ModalTituloIT.InnerHtml.Contains("Edit"))
                {
                    foreach (DataRow fila in dt.Rows)
                    {
                        if (!fila.RowState.ToString().Contains("Del"))
                        {
                            if (fila["mvkeymercanciaprovisional"].ToString() == Session["mvkeymercanciaprovisional"].ToString())
                            {
                                fila["tipomercancia"] = txt_IT_tipomercancia.Text.Trim();
                                fila["valorprovisional"] = txt_IT_valorprovisional.Text.Trim();
                            }
                        }
                    }
                }


                Grid9_MV.DataSource = Session["Grid9_MV"] = dt;
                Grid9_MV.DataBind();
                Grid9_MV.Settings.VerticalScrollableHeight = 168;
                Grid9_MV.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "IA_ManifestacionValor-btnGuardarIT_Click", ex, lblCadena.Text, ref mensaje);
                AlertError(ex.Message);
            }
        }

        //Botón Cancelar Factura
        protected void btnCancelarIT_Click(object sender, EventArgs e)
        {
            try
            {
                Grid9_MV.Settings.VerticalScrollableHeight = 168;
                Grid9_MV.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;

                lkb_ManifestacionMercanciaProvisional_Agregar.Focus();
            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "IA_ManifestacionValor-btnCancelarIT_Click", ex, lblCadena.Text, ref mensaje);
            }
        }

        #endregion



        #region REPRESENTANTE LEGAL

        //Metodo que llama al Callback para actualizar el PageSize y el Grid
        protected void GridRL_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            GridPageSizeRL = int.Parse(e.Parameters);
            GridRL.SettingsPager.PageSize = GridPageSizeRL;
            GridRL.DataBind();
        }

        //Botón abre modal para ver el grid de representantes legales
        protected void lkb_RL_Click(object sender, EventArgs e)
        {
            try
            {
                //abrir modal
                MostrarModalRL();


                //cargar información al grid GridRL
                string mensaje = "";
                DataTable dtc = new DataTable();

                dtc = catalogo.Traer_Representantes_Legales(lblCadena.Text, ref mensaje);
                if (dtc != null && dtc.Rows.Count > 0)
                {
                    GridRL.DataSource = Session["GridRL"] = dtc;
                    GridRL.DataBind();
                    GridRL.SettingsPager.PageSize = 20;

                }
                else
                {
                    GridRL.DataSource = Session["Grid"] = dtc;
                    GridRL.DataBind();
                    //AlertError("No hay información o intentelo de nuevo");
                }

                GridRL.Settings.VerticalScrollableHeight = 330;
                GridRL.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;

                if (GridRL.VisibleRowCount > 0)
                {
                    //Seleccionar el checkbox por los valores de los campos en reprentante legal
                    for (int i = 0; i < GridRL.VisibleRowCount; i++)
                    {
                        ASPxCheckBox chkConsultar = GridRL.FindRowCellTemplateControl(i, (GridViewDataColumn)GridRL.Columns["Seleccionar"], "chkConsultar") as ASPxCheckBox;

                        if( Session["REPRESENTANTEKEY"] != null && (Session["REPRESENTANTEKEY"].ToString().Trim() == GridRL.GetRowValues(i, "REPRESENTANTEKEY").ToString().Trim()))
                        {
                            chkConsultar.Checked = true;

                            //Habilitar botón de editar
                            string cledit = lkb_EditarRL.CssClass;
                            if (cledit.Contains("disabled"))
                            {
                                cledit = cledit.Replace("disabled", "");
                                lkb_EditarRL.CssClass = cledit;
                            }

                            //Habilitar botón de eliminar
                            string cldelete = lkb_EliminarRL.CssClass;
                            if (cldelete.Contains("disabled"))
                            {
                                cldelete = cldelete.Replace("disabled", "");
                                lkb_EliminarRL.CssClass = cldelete;
                            }
                        }
                        else
                            chkConsultar.Checked = false;
                    }                    
                }
                else
                
                    //Si no existe información deshabilita botones de editar eliminar y aceptar, como limpiar controles
                if (GridRL.VisibleRowCount == 0)
                {
                    //Deshabilitar botón de editar
                    string cledit = lkb_EditarRL.CssClass;
                    if (!cledit.Contains("disabled"))
                    {
                        cledit = cledit + " " + "disabled";
                        lkb_EditarRL.CssClass = cledit;
                    }

                    //Deshabilitar botón de eliminar
                    string cldelete = lkb_EliminarRL.CssClass;
                    if (!cldelete.Contains("disabled"))
                    {
                        cldelete = cldelete + " " + "disabled";
                        lkb_EliminarRL.CssClass = cldelete;
                    }                   
                    
                    ASPxBinaryImage.Value = null;
                    Session["REPRESENTANTEKEY"] = null;
                    cbx_MostrarFirma.Checked = false;
                }

            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "IA_ManifestacionValor-lkb_RL_Click", ex, lblCadena.Text, ref mensaje);
            }
        }

        //Abre el modal para ver el grid de representantes legales
        protected void btnRegresarRL_Click(object sender, EventArgs e)
        {
            try
            {
                //Obtener los datos del registro seleccionado por el checkbox 

                int valida = 0;

                for (int i = 0; i < GridRL.VisibleRowCount; i++)
                {
                    ASPxCheckBox chkConsultar = GridRL.FindRowCellTemplateControl(i, (GridViewDataColumn)GridRL.Columns["Seleccionar"], "chkConsultar") as ASPxCheckBox;

                    if (chkConsultar != null && chkConsultar.Checked)
                    {
                        valida = 1;
                        
                        if (GridRL.GetRowValues(i, "FIRMA").ToString().Trim().Length > 0)
                            ASPxBinaryImage.Value = (byte[])GridRL.GetRowValues(i, "FIRMA");
                        else
                            ASPxBinaryImage.Value = null;


                        Session["REPRESENTANTEKEY"] = GridRL.GetRowValues(i, "REPRESENTANTEKEY").ToString().Trim();
                        cbx_MostrarFirma.Checked = true;

                        break;
                    }
                }

                //Limpia los controles
                if (valida.Equals(0))
                {
                    ASPxBinaryImage.Value = null;
                    Session["REPRESENTANTEKEY"] = null;
                    cbx_MostrarFirma.Checked = false;
                }
            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "IA_ManifestacionValor-btnRegresarRL_Click", ex, lblCadena.Text, ref mensaje);
            }
        }

        //Este método lo llama el checkbox "chkConsultar" del grid "GridRL" y este método llama a una función "chkConsultarClick" 
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
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "IA_ManifestacionValor-chkConsultar_Init", ex, lblCadena.Text, ref mensaje);
            }
        }

        protected void lkb_NuevoRL_Click(object sender, EventArgs e)
        {
            //Abre Modales
            MostrarModalRL();
            MostrarModalRL2();

            //Titulo del Modal
            ModalTituloRL2.InnerText = "Nuevo Representate Legal";
            DataBind();

            //Limpiar Campos
            txtRL2_Nombre.Text = string.Empty;
            txtRL2_Paterno.Text = string.Empty;
            txtRL2_Materno.Text = string.Empty;
            txtRL2_rfc.Text = string.Empty;
            txtRL2_tel.Text = string.Empty;
            txtRL2_correo.Text = string.Empty;
            dateRL2_ApartirDe.Text = DateTime.Now.ToShortDateString();
            dateRL2_Hasta.Text = DateTime.Now.ToShortDateString();
            BinaryImageRL2.Value = null;

            ////Actualiza los permisos de los botones en grid
            //PermisosUsuario();
        }

        protected void lkb_EditarRL_Click(object sender, EventArgs e)
        {
            string mensaje = string.Empty;
            DataTable dt = new DataTable();
            int valida_select = 0;


            for (int i = 0; i < GridRL.VisibleRowCount; i++)
            {
                ASPxCheckBox chkConsultar = GridRL.FindRowCellTemplateControl(i, (GridViewDataColumn)GridRL.Columns["Seleccionar"], "chkConsultar") as ASPxCheckBox;

                if (chkConsultar.Checked)
                {
                    //Abre Modales
                    MostrarModalRL();
                    MostrarModalRL2();

                    //Titulo del Modal
                    ModalTituloRL2.InnerText = "Editar Representate Legal";
                    DataBind();

                    //Asignar valor al campo txt
                    Session["REPRESENTANTEKEY"] = GridRL.GetRowValues(i, "REPRESENTANTEKEY").ToString().Trim();

                    txtRL2_Nombre.Text = GridRL.GetRowValues(i, "NOMBRE").ToString().Trim();
                    txtRL2_Paterno.Text = GridRL.GetRowValues(i, "APELLIDOPATERNO").ToString().Trim(); 
                    txtRL2_Materno.Text = GridRL.GetRowValues(i, "APELLIDOMATERNO").ToString().Trim(); 
                    txtRL2_rfc.Text = GridRL.GetRowValues(i, "RFC").ToString().Trim(); 
                    txtRL2_tel.Text = GridRL.GetRowValues(i, "TELEFONO").ToString().Trim(); 
                    txtRL2_correo.Text = GridRL.GetRowValues(i, "CORREOELECTRONICO").ToString().Trim(); 
                    string v_apartir = GridRL.GetRowValues(i, "APARTIRDE").ToString().Trim(); 
                    dateRL2_ApartirDe.Text = v_apartir.Substring(0, 10);
                    string v_hasta = GridRL.GetRowValues(i, "HASTA").ToString().Trim(); 
                    dateRL2_Hasta.Text = v_hasta.Substring(0, 10);
                    if (GridRL.GetRowValues(i, "FIRMA").ToString().Trim().Length > 0)
                        BinaryImageRL2.Value = (byte[])GridRL.GetRowValues(i, "FIRMA");
                    else
                        BinaryImageRL2.Value = null;
                    valida_select = 1;
                }
            }

            if (valida_select == 0)
            {
                AlertError("Debe seleccionar un representante legal para poder editar");

                //Mostrar Modal
                MostrarModalRL();
            }
            ////Actualiza los permisos de los botones en grid
            //PermisosUsuario();
        }

        protected void lkb_EliminarRL_Click(object sender, EventArgs e)
        {

            //Modal Question
            string valida = "¿Desea eliminar este registro?";
            AlertQuestionR(valida);

            //Mostrar Modal
            MostrarModalRL();

        }

        //Elimina registro en GridRL
        protected void btnOkR_Click(object sender, EventArgs e)
        {
            try
            {
                int valida_select = 0;

                for (int i = 0; i < GridRL.VisibleRowCount; i++)
                {
                    ASPxCheckBox chkConsultar = GridRL.FindRowCellTemplateControl(i, (GridViewDataColumn)GridRL.Columns["Seleccionar"], "chkConsultar") as ASPxCheckBox;

                    if (chkConsultar.Checked)
                    {
                        string mensaje = string.Empty;
                        DataTable dt = new DataTable();
                        int id = int.Parse(GridRL.GetRowValues(i, "REPRESENTANTEKEY").ToString().Trim());
                        dt = catalogo.Eliminar_Representante_Legal(id, lblCadena.Text, ref mensaje);

                        GridRL.DataSource = Session["GridRL"] = dt;
                        GridRL.DataBind();
                        GridRL.SettingsPager.PageSize = 20;


                        //AlertSuccess("El representnte legal se eliminó con éxito.");
                        valida_select = 1;

                        GridRL.Settings.VerticalScrollableHeight = 330;
                        GridRL.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;

                        break;
                    }
                }

                if (valida_select == 0)
                    AlertError("Debe seleccionar un representante legal para poder eliminar");


                //Carga de nuevo GridRL
                lkb_RL_Click(null, null);

                ////Actualiza los permisos de los botones en grid
                //PermisosUsuario();

            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                //excepcion.RegistrarExcepcion(idusuario, "IA_ManifestacionValor-btnOkR_Click", ex, Session["Cadena"].ToString(), ref mensaje);
            }
        }

        //Botón Cancelar dentro de Nuevo o Editar
        protected void btnCancelarRL_Click(object sender, EventArgs e)
        {
            //Carga de nuevo GridRL
            lkb_RL_Click(null, null);
        }

        protected void btnGuardarRL_Click(object sender, EventArgs e)
        {
            if (Session["Cadena"] == null || (Session["GridRL"] == null && GridRL.VisibleRowCount > 0))
            {
                string alerta = "<script>alert('La sesión ha expirado, debe volver a iniciar sesión');window.location.href='Login.aspx'</script>";
                Session["Tab"] = "Salir";
                Response.Write(alerta);
                Response.Redirect("Login.aspx", false);
                return;
            }

            //Actualiza los permisos de los botones en grid
            //PermisosUsuario();

            int total_representantes = 0;


            if (Session["GridRL"] != null && ((DataTable)Session["GridRL"]).Rows.Count > 0)
                total_representantes = ((DataTable)Session["GridRL"]).Rows.Count;


            //Obtener valores de fechas
            DateTime? dateDesde = string.IsNullOrEmpty(dateRL2_ApartirDe.Text) ? (DateTime?)null : DateTime.Parse(dateRL2_ApartirDe.Text);
            DateTime? dateHasta = string.IsNullOrEmpty(dateRL2_Hasta.Text) ? (DateTime?)null : DateTime.Parse(dateRL2_Hasta.Text);


            string valida = string.Empty;

            //Validar que el dato a guardar o modificar no exista 
            if (txtRL2_Nombre.Text.Trim() == string.Empty)
            {
                valida = "Para poder guardar escriba un nombre";
            }

            else if (txtRL2_Paterno.Text.Trim() == string.Empty)
            {
                valida = "Para poder guardar escriba un apellido paterno";
            }

            else if (txtRL2_Materno.Text.Trim() == string.Empty)
            {
                valida = "Para poder guardar escriba un apellido materno";
            }

            else if (txtRL2_rfc.Text.Trim() == string.Empty)
            {
                valida = "Para poder guardar escriba un RFC valido";
            }

            //Validar fechas que existan
            else if ((dateDesde == null || dateHasta == null))
            {
                valida = "Debe seleccionar un rango de fechas validas";
            }

            //Validar fechas rangos correctos
            else if ((dateDesde != null && dateHasta != null) && dateDesde > dateHasta)
            {
                valida = "La fecha a partir de, no puede ser mayor a la fecha hasta";
            }

            else if (Session["GridRL"] != null && ((DataTable)(Session["GridRL"])).Rows.Count > 0)
            {
                foreach (DataRow fila in ((DataTable)(Session["GridRL"])).Rows)
                {
                    DateTime? dateDesde_f = string.IsNullOrEmpty(fila["APARTIRDE"].ToString().Trim()) ? (DateTime?)null : DateTime.Parse(fila["APARTIRDE"].ToString().Trim());
                    DateTime? dateHasta_f = string.IsNullOrEmpty(fila["HASTA"].ToString().Trim()) ? (DateTime?)null : DateTime.Parse(fila["HASTA"].ToString().Trim());

                    //Nuevo
                    if (ModalTituloRL2.InnerText.Contains("Nuevo") &&
                        fila["NOMBRE"].ToString().ToUpper().Trim() == txtRL2_Nombre.Text.ToUpper().Trim() &&
                        fila["APELLIDOPATERNO"].ToString().ToUpper().Trim() == txtRL2_Paterno.Text.ToUpper().Trim() &&
                        fila["APELLIDOMATERNO"].ToString().ToUpper().Trim() == txtRL2_Materno.Text.ToUpper().Trim() &&
                        fila["RFC"].ToString().ToUpper().Trim() == txtRL2_rfc.Text.ToUpper().Trim() &&
                        dateDesde_f < dateDesde && dateHasta_f > dateDesde)
                    {
                        valida = "Ya existe este representante legal en un rango de fechas a las seleccionadas: Nombre(" + txtRL2_Nombre.Text.Trim() + "), Ap Paterno(" + txtRL2_Paterno.Text.Trim() +
                            "), Ap Materno(" + txtRL2_Materno.Text.Trim() + "), RFC(" + txtRL2_rfc.Text.Trim() + "), Fecha apartir de(" + fila["APARTIRDE"].ToString().Trim() + "), Fecha hasta(" + fila["HASTA"].ToString().Trim() + ") ";
                        break;
                    }


                    //Editar
                    if (ModalTituloRL2.InnerText.Contains("Editar") &&
                        fila["REPRESENTANTEKEY"].ToString().ToUpper().Trim() != Session["REPRESENTANTEKEY"].ToString().Trim().ToUpper() &&
                        fila["NOMBRE"].ToString().ToUpper().Trim() == txtRL2_Nombre.Text.ToUpper().Trim() &&
                        fila["APELLIDOPATERNO"].ToString().ToUpper().Trim() == txtRL2_Paterno.Text.ToUpper().Trim() &&
                        fila["APELLIDOMATERNO"].ToString().ToUpper().Trim() == txtRL2_Materno.Text.ToUpper().Trim() &&
                        fila["RFC"].ToString().ToUpper().Trim() == txtRL2_rfc.Text.ToUpper().Trim() &&
                        dateDesde_f < dateDesde && dateHasta_f > dateDesde)
                    {
                        valida = "Ya existe este representante legal en un rango de fechas a las seleccionadas: Nombre(" + txtRL2_Nombre.Text.Trim() + "), Ap Paterno(" + txtRL2_Paterno.Text.Trim() +
                            "), Ap Materno(" + txtRL2_Materno.Text.Trim() + "), RFC(" + txtRL2_rfc.Text.Trim() + "), Fecha apartir de(" + fila["APARTIRDE"].ToString().Trim() + "), Fecha hasta(" + fila["HASTA"].ToString().Trim() + ") ";
                        break;
                    }

                }
            }


            if (valida.Length > 0)
            {
                //Mantiene los modales
                MostrarModalRL();
                MostrarModalRL2();

                AlertError(valida);

                //Carga de nuevo GridRL
                lkb_RL_Click(null, null);

                return;
            }

            var imagen = BinaryImageRL2.Value;

            //Guardar
            string mensaje = "";
            DataTable dt = new DataTable();

            if (ModalTituloRL2.InnerText.Contains("Nuevo"))
                dt = catalogo.Guardar_Representante_Legal(txtRL2_Nombre.Text.ToUpper().Trim(), txtRL2_Paterno.Text.ToUpper().Trim(), txtRL2_Materno.Text.ToUpper().Trim(),
                                                          txtRL2_rfc.Text.ToUpper().Trim(), txtRL2_correo.Text.Trim(), txtRL2_tel.Text.Trim(), dateDesde, dateHasta,
                                                          imagen, lblCadena.Text, ref mensaje);
            else if (ModalTituloRL2.InnerText.Contains("Editar"))
                dt = catalogo.Editar_Representante_Legal(int.Parse(Session["REPRESENTANTEKEY"].ToString()), txtRL2_Nombre.Text.ToUpper().Trim(), txtRL2_Paterno.Text.ToUpper().Trim(),
                                                         txtRL2_Materno.Text.ToUpper().Trim(), txtRL2_rfc.Text.ToUpper().Trim(), txtRL2_correo.Text.Trim(), txtRL2_tel.Text.Trim(), dateDesde,
                                                         dateHasta, imagen, lblCadena.Text, ref mensaje);

            if (dt != null && dt.Rows.Count > 0)
            {
                GridRL.DataSource = Session["GridRL"] = dt;
                GridRL.DataBind();
                GridRL.Settings.VerticalScrollableHeight = 330;
                GridRL.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                GridRL.SettingsPager.PageSize = 20;

                //AlertSuccess("El representante legal se " + (ModalTitulo.InnerText.Contains("Editar") ? "actualizó" : "agregó") + ".");
            }
            else
            {
                GridRL.DataSource = Session["GridRL"];
                GridRL.DataBind();
            }

            GridRL.Settings.VerticalScrollableHeight = 330;
            GridRL.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;


            //Carga de nuevo GridRL
            lkb_RL_Click(null, null);

            ////Actualiza los permisos de los botones en grid
            //PermisosUsuario();
        }


        #endregion

 
    }
}