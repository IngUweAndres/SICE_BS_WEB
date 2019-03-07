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

namespace SICE_BS_WEB.Presentacion
{
    public partial class IA_HojaCalculo : System.Web.UI.Page
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



                    Session["Grid"] = null;
                    TituloPanel(string.Empty);

                    string mensaje = "";                    
                    DataTable dth = new DataTable();

                    //Traer
                    dth = informes.ConsultarHojaCalculo(Session["Cadena"].ToString(), ref mensaje);

                    //Cargar de nuevo Hoja de Cálculo
                    Grid.DataSource = Session["Grid"] = dth;
                    Grid.DataBind();
                    Grid.Settings.VerticalScrollableHeight = 230;
                    Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                    Grid.SettingsPager.PageSize = 20;

                    Session["ASPxBinaryImageRL"] = null;
                }
            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "IA_HojaCalculo-Page_Load", ex, lblCadena.Text, ref mensaje);
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
        }

        //Cuando el grid principal trae datos entra aquí para pintar imagen en botón Detalle
        protected void ASPxButtonDetailDocD_Init(object sender, EventArgs e)
        {
            ASPxButton btn = (ASPxButton)sender;
            btn.ImageUrl = "~/img/iconos/ico_doc1.png";

            GridViewDataItemTemplateContainer container = btn.NamingContainer as GridViewDataItemTemplateContainer;

            if (Grid.GetRowValues(container.VisibleIndex, "valida").ToString() == "2") // 2, significa que en tabla detallehc se tiene más de un registro
                btn.Visible = true;
            else
                btn.Visible = false;

            if(Session["ASPxBinaryImageRL"] != null)
                ASPxBinaryImageRL.Value = Session["ASPxBinaryImageRL"];
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
                    AlertError("Si va a crear hoja de cálculo por pedimento, asegurese de limpiar las fechas o viseversa.");
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

                string v_mensaje = string.Empty;
                if (PEDIMENTO.Text.Trim().Length > 0)
                    dt = informes.ConsultarPorPedimentoEnHojaCalculo(PEDIMENTO.Text, lblCadena.Text, ref mensaje, out v_mensaje);
                else
                    dt = informes.ConsultarPorFechasEnHojaCalculo(dateDesde, dateHasta, lblCadena.Text, ref mensaje);



                if (dt != null && dt.Rows.Count > 0)
                {
                    Grid.DataSource = Session["Grid"] = dt;
                    Grid.DataBind();
                    Grid.Settings.VerticalScrollableHeight = 230;
                    Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                    Grid.SettingsPager.PageSize = 20;

                    Session["MENSAJE"] = v_mensaje;
                }
                else
                {
                    Grid.DataSource = Session["Grid"] = dt;
                    Grid.DataBind();
                    AlertError("No hay información o intentelo de nuevo");
                }
            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "IA_HojaCalculo-ASPxButtonDetailDoc_Click", ex, lblCadena.Text, ref mensaje);
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
                string v_hckey = Grid.GetRowValues(container.VisibleIndex, "hckey").ToString();
                string v_pedimento = Grid.GetRowValues(container.VisibleIndex, "pedimentoarmado" ).ToString();


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
                XtraReport1 reporte = new XtraReport1();
                //string mensaje = string.Empty;
                //Guid keyguid = Guid.Parse(v_hckey);
                //reporte.DataSource = reportes.Traer_Anexo(keyguid, Session["Cadena"].ToString(), ref mensaje);
                reporte.Parameters[0].Value = v_hckey;
                reporte.Parameters[0].Visible = false;
                //reporte.ExportToPdf(FilePath + "\\report_" + Convert.ToString(numeroSinCotaArbitraria) + ".pdf");                    
                //reportePath = FilePath + "\\report_" + Convert.ToString(numeroSinCotaArbitraria) + ".pdf";
                
                //frmPDF1.Attributes.Add("src", "report_" + Convert.ToString(numeroSinCotaArbitraria) + ".pdf");
                reporte.ExportToPdf(FilePath + "\\" + Convert.ToString(v_pedimento) + "-HC.pdf");
                reportePath = FilePath + "\\" + Convert.ToString(v_pedimento) + "-HC.pdf";

                frmPDF1.Attributes.Add("src", Convert.ToString(v_pedimento) + "-HC.pdf");
                MostrarModal();
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message.ToString());
                string mensaje = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                //excepcion.RegistrarExcepcion(idusuario, "IA_HojaCalculo-ASPxButtonDetailDoc_Click", ex, lblCadena.Text, ref mensaje);
            }
        }

        //Botón en columna de Pedimento, abre archivo PDF
        protected void ASPxButtonDetailDocD_Click(object sender, EventArgs e)
        {
            try
            {
                ASPxButton btn = (ASPxButton)sender;

                btn.ImageUrl = "~/img/iconos/ico_doc1.png";

                GridViewDataItemTemplateContainer container = (GridViewDataItemTemplateContainer)btn.NamingContainer;
                string v_hckey = Grid.GetRowValues(container.VisibleIndex, "hckey").ToString();
                string v_pedimento = Grid.GetRowValues(container.VisibleIndex, "pedimentoarmado").ToString();

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
                ANEXOYFACTURAS reporte = new ANEXOYFACTURAS();
                //string mensaje = string.Empty;
                //Guid keyguid = Guid.Parse(v_hckey);
                //reporte.DataSource = reportes.Traer_Detalle_Anexo(keyguid, Session["Cadena"].ToString(), ref mensaje);
                reporte.Parameters[0].Value = v_hckey;
                reporte.Parameters[0].Visible = false;
                //reporte.ExportToPdf(FilePath + "\\report_" + Convert.ToString(numeroSinCotaArbitraria) + ".pdf");
                //reportePath = FilePath + "\\report_" + Convert.ToString(numeroSinCotaArbitraria) + ".pdf";
                //frmPDF1.Attributes.Add("src", "report_" + Convert.ToString(numeroSinCotaArbitraria) + ".pdf");
                reporte.ExportToPdf(FilePath + "\\" + Convert.ToString(v_pedimento) + "-ANEXOHC.pdf");
                reportePath = FilePath + "\\" + Convert.ToString(v_pedimento) + "-ANEXOHC.pdf";
                frmPDF1.Attributes.Add("src", Convert.ToString(v_pedimento) + "-ANEXOHC.pdf");

                MostrarModal();
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message.ToString());
                string mensaje = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                //excepcion.RegistrarExcepcion(idusuario, "IA_HojaCalculo-ASPxButtonDetailDocD_Click", ex, lblCadena.Text, ref mensaje);
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
                //excepcion.RegistrarExcepcion(idusuario, "Page_Load", ex, Session["Cadena"].ToString(), ref mensaje);
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

        //Propiedad GridPageSize2
        protected int GridPageSize2
        {
            get
            {
                if (Session[PageSizeSessionKey] == null)
                    return Grid2.SettingsPager.PageSize;
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
                    return Grid3.SettingsPager.PageSize;
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


            //Cuando se quiera filtrar el Grid2 entra en el if
            if (Session["Grid2"] != null)
            {
                Grid2.DataSource = Session["Grid2"];
                Grid2.DataBind();
                Grid2.SettingsPager.PageSize = GridPageSize2;
            }

            //Cuando se quiera filtrar el Grid3 entra en el if
            if (Session["Grid3"] != null)
            {
                Grid3.DataSource = Session["Grid3"];
                Grid3.DataBind();
                Grid3.SettingsPager.PageSize = GridPageSize3;
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
            Grid.SettingsPager.PageSize = GridPageSize;
            Grid.DataBind();
        }

        //Checkbox All - Grid
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
                excepcion.RegistrarExcepcion(idusuario, "IA_HojaCalculo-chkConsultarAll_Init", ex, lblCadena.Text, ref mensaje);
            }
        }

        //Checkbox1 - Grid
        protected void chkConsultar1_Init(object sender, EventArgs e)
        {
            try
            {
                ASPxCheckBox chb = sender as ASPxCheckBox;
                GridViewDataItemTemplateContainer container = chb.NamingContainer as GridViewDataItemTemplateContainer;

                chb.ClientInstanceName = String.Format("chkConsultar1{0}", container.VisibleIndex);
                chb.ClientSideEvents.CheckedChanged = String.Format("function (s, e) {{ chkConsultar1Click(s, e, {0}); }}", container.VisibleIndex);

                lblT_TotalRenglones.Text = "Total registros (" + Grid.VisibleRowCount + ")";

            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "IA_HojaCalculo-chkConsultar1_Init", ex, lblCadena.Text, ref mensaje);
            }
        }

        ////Metodo que llama al Callback para actualizar el PageSize y el Grid
        //protected void Grid2_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        //{
        //    GridPageSize2 = int.Parse(e.Parameters);
        //    Grid2.SettingsPager.PageSize = GridPageSize2;
        //    Grid2.DataBind();
        //}

        ////Metodo que llama al Callback para actualizar el PageSize y el Grid
        //protected void Grid3_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        //{
        //    GridPageSize3 = int.Parse(e.Parameters);
        //    Grid3.SettingsPager.PageSize = GridPageSize3;
        //    Grid3.DataBind();
        //}

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

        protected void lkb_Excel_Click(object sender, EventArgs e)
        {
            if (!permisoExportar)
            {
                AlertError("No tiene permiso para exportar");
                return;
            }

            if (Grid.VisibleRowCount > 0)
            {
                Exporter.WriteXlsToResponse("Hoja Cálculo", new XlsExportOptionsEx() { SheetName = "Hoja Cálculo" });
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
            dt = informes.ConsultarHojaCalculo(Session["Cadena"].ToString(), ref mensaje);

            lkb_LimpiarFiltros_Click(null, null);

            //Cargar de nuevo Hoja de Cálculo
            Grid.DataSource = Session["Grid"] = dt;
            Grid.DataBind();
            Grid.Settings.VerticalScrollableHeight = 230;
            Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
            Grid.SettingsPager.PageSize = 20;
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
                FilePath = Server.MapPath(FolderPath) + "\\Temp\\HC";
                ZipPath = Server.MapPath(FolderPath) + "\\Temp\\archivo_hc.zip";

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
                for (int i = 0; i < Grid.VisibleRowCount; i++)
                {
                    ASPxCheckBox chkConsultar = Grid.FindRowCellTemplateControl(i, (GridViewDataColumn)Grid.Columns["Seleccionar"], "chkConsultar1") as ASPxCheckBox;

                    if (chkConsultar.Checked)
                    {
                        string v_hckey = Grid.GetRowValues(i, "hckey").ToString().Trim();
                        string v_pedimento = Grid.GetRowValues(i, "pedimentoarmado").ToString().Trim();
                        string v_valida = Grid.GetRowValues(i, "valida").ToString().Trim();
                        bandera = 1;

                        //CREA REPORTES HC                                    
                        //Crea el reporte en  pdf
                        XtraReport1 reporte = new XtraReport1();
                        reporte.Parameters[0].Value = v_hckey;
                        reporte.Parameters[0].Visible = false;

                        //Valida, sí existe el archivo del reporte con el mismo pedimento agregarle un + 1 al nombre
                        int cont1 = 1;
                        while (System.IO.File.Exists(FilePath + "\\" + v_pedimento + "-MV.pdf"))
                        {
                            v_pedimento = v_pedimento + "(" + cont1.ToString() + ")";
                            cont1 += 1;
                        }   

                        reporte.ExportToPdf(FilePath + "\\" + Convert.ToString(v_pedimento) + "-HC.pdf");

                        if (v_valida.Contains("2")) // el valor de "valida" que viene del Grid nos indica si tiene detalle anexo.
                        {
                            //CREA REPORTES ANEXOS DETALLE
                            ANEXOYFACTURAS reporteAnexo = new ANEXOYFACTURAS();
                            reporte.Parameters[0].Value = v_hckey;
                            reporte.Parameters[0].Visible = false;
                            reporte.ExportToPdf(FilePath + "\\" + Convert.ToString(v_pedimento) + "-ANEXOHC.pdf");
                        }
                                                
                    }
                }

                if (bandera.Equals(1))
                {
                //Ejecuta botón btnDescargarZip que a su vez ejecuta su acción de cliente para ir a la clase FileDownloadHandlerMV
                ScriptManager.RegisterStartupScript(this.Page, typeof(String), "", "<script> document.getElementById('btnZip').click(); </script> ", false);
                }
                else
                    AlertError("Debe seleccionar una hoja de cálculo para descarga en zip");
            }
            catch (Exception ex)
            {
                AlertError(ex.Message);
                string mensaje = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "IA_HojaCalculo-lkb_DescargarZip_Click", ex, lblCadena.Text, ref mensaje);
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
            Session["hckey"] = Grid.GetRowValues(e.VisibleIndex, "hckey").ToString();

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
                    Session["Grid2"] = null;
                    Session["Grid3"] = null;

                    //Abre el segundo view 
                    MultiView1.ActiveViewIndex = 1;
                    lblTitPedimento.InnerText = "Pedimento: " + Grid.GetRowValues(e.VisibleIndex, "pedimentoarmado").ToString();
                    ASPxPageControl1.ActiveTabIndex = 0;

                    //Traer valores a los campos de hojad e calculo y si tiene detalle también
                    string mensaje = "";
                    DataTable dt = new DataTable();
                    dt = informes.ConsultarDatosEdicion(Session["hckey"].ToString(), Session["Cadena"].ToString(), ref mensaje);

                    #region Anexo Facturas
                    //Se usa para traer de anexos solo las facturas
                    string mensaje2 = "";
                    DataTable dtf = new DataTable();
                    dtf = informes.ConsultarDatosEdicionFactura(Session["hckey"].ToString(), Session["Cadena"].ToString(), ref mensaje2);

                    //Anexo Facturas
                    Grid3.DataSource = Session["Grid3"] = dtf;
                    Grid3.DataBind();
                    Grid3.Settings.VerticalScrollableHeight = 168;
                    Grid3.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                    Grid3.SettingsPager.PageSize = 20;
                    #endregion


                    if (dt != null && dt.Rows.Count > 0)
                    {
                        txt_Referencia.Text = dt.Rows[0]["Referencia"].ToString();

                        //01 Datos del Importador
                        txt01_Nombre.Text = dt.Rows[0]["01_nombreimportador"].ToString();
                        txt01_RFC.Text = dt.Rows[0]["01_rfcimportador"].ToString();
                        txt01_Calle.Text = dt.Rows[0]["01_importadorcalle"].ToString();
                        txt01_NoExtInt.Text = dt.Rows[0]["01_importadornumero"].ToString();
                        txt01_CodigoPostal.Text = dt.Rows[0]["01_importadorcodigop"].ToString();
                        txt01_Colonia.Text = dt.Rows[0]["01_importadorcolonia"].ToString();

                        //02 Datos del Vendedor
                        txt02_Nombre.Text = dt.Rows[0]["02_nombrevendedor"].ToString();
                        txt02_TaxNumber.Text = dt.Rows[0]["02_rfcvendedor"].ToString();
                        txt02_Calle.Text = dt.Rows[0]["02_vendedorcalle"].ToString();
                        txt02_NoExtInt.Text = dt.Rows[0]["02_vendedornumero"].ToString();
                        txt02_Ciudad.Text = dt.Rows[0]["02_vendedorciudad"].ToString();
                        txt02_Pais.Text = dt.Rows[0]["02_vendedorpais"].ToString();

                        //Datos de la Mercancia
                        Grid2.DataSource = Session["Grid2"] = dt;
                        Grid2.DataBind();
                        Grid2.Settings.VerticalScrollableHeight = 168;
                        Grid2.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                        Grid2.SettingsPager.PageSize = 20;

                        //05 Determinación Método
                        if (dt.Rows[0]["04_determinacionpago_01"].ToString() != null && dt.Rows[0]["04_determinacionpago_01"].ToString() == "SI")
                        {
                            chk41S.Checked = true;
                            chk41N.Checked = false;
                        }
                        else
                        {
                            chk41S.Checked = false;
                            chk41N.Checked = true;
                        }
                        if (dt.Rows[0]["04_determinacionpago_02"].ToString() != null && dt.Rows[0]["04_determinacionpago_02"].ToString() == "SI")
                        {
                            chk42S.Checked = true;
                            chk42N.Checked = false;
                        }
                        else
                        {
                            chk42S.Checked = false;
                            chk42N.Checked = true;
                        }
                        if (dt.Rows[0]["04_determinacionpago_03"].ToString() != null && dt.Rows[0]["04_determinacionpago_03"].ToString() == "SI")
                        {
                            chk43S.Checked = true;
                            chk43N.Checked = false;
                        }
                        else
                        {
                            chk43S.Checked = false;
                            chk43N.Checked = true;
                        }
                        if (dt.Rows[0]["04_determinacionpago_04"].ToString() != null && dt.Rows[0]["04_determinacionpago_04"].ToString() == "SI")
                        {
                            chk44S.Checked = true;
                            chk44N.Checked = false;
                        }
                        else
                        {
                            chk44S.Checked = false;
                            chk44N.Checked = true;
                        }
                        if (dt.Rows[0]["04_determinacionpago_05"].ToString() != null && dt.Rows[0]["04_determinacionpago_05"].ToString() == "SI")
                        {
                            chk45S.Checked = true;
                            chk45N.Checked = false;
                        }
                        else
                        {
                            chk45S.Checked = false;
                            chk45N.Checked = true;
                        }

                        //Precio Pagado o por Pagar
                        txt5_MonedaPago.Text = dt.Rows[0]["05_monedapagodirecto"].ToString();
                        se5_PagoDirecto.Text = (dt.Rows[0]["05_pagodirecto"].ToString().Trim() == string.Empty) ? "0" : dt.Rows[0]["05_pagodirecto"].ToString();
                        se5_Contraprestacion.Text = (dt.Rows[0]["05_contraprestacionpagodirecto"].ToString().Trim() == string.Empty) ? "0" : dt.Rows[0]["05_contraprestacionpagodirecto"].ToString();

                        decimal v_pagodirecto = 0;
                        decimal v_contraprestacion = 0;
                        v_pagodirecto = (se5_PagoDirecto.Text != null && se5_PagoDirecto.Text.Length > 0) ? decimal.Parse(se5_PagoDirecto.Text) : 0;
                        v_contraprestacion = (se5_Contraprestacion.Text != null && se5_Contraprestacion.Text.Length > 0) ? decimal.Parse(se5_Contraprestacion.Text) : 0;
                        lbl05Total.InnerText = (v_pagodirecto + v_contraprestacion).ToString();
                        //se5_Total.Text = dt.Rows[0]["05_totalpreciopagado"].ToString();

                        //06 Ajuste AutoIncrementable
                        txt6_Moneda.Text = dt.Rows[0]["06_incrementablemoneda"].ToString();
                        se6_Comision.Text = (dt.Rows[0]["06_incrementablescomision"].ToString().Trim() == string.Empty) ? "0" : dt.Rows[0]["06_incrementablescomision"].ToString();
                        se6_FleteSeguro.Text = (dt.Rows[0]["06_incremetablesfletes"].ToString().Trim() == string.Empty) ? "0" : dt.Rows[0]["06_incremetablesfletes"].ToString();
                        se6_CargaDescarga.Text = (dt.Rows[0]["06_incremetablesfletescarga"].ToString().Trim() == string.Empty) ? "0" : dt.Rows[0]["06_incremetablesfletescarga"].ToString();
                        se6_MaterialAportado.Text = (dt.Rows[0]["06_incremetablesmateriales"].ToString().Trim() == string.Empty) ? "0" : dt.Rows[0]["06_incremetablesmateriales"].ToString();
                        se6_TecnoAportada.Text = (dt.Rows[0]["06_incremetablestecnologias"].ToString().Trim() == string.Empty) ? "0" : dt.Rows[0]["06_incremetablestecnologias"].ToString();
                        se6_Regalias.Text = (dt.Rows[0]["06_incremetablesregalias"].ToString().Trim() == string.Empty) ? "0" : dt.Rows[0]["06_incremetablesregalias"].ToString();
                        se6_Reversiones.Text = (dt.Rows[0]["06_incremetablesreversiones"].ToString().Trim() == string.Empty) ? "0" : dt.Rows[0]["06_incremetablesreversiones"].ToString();

                        decimal v_comision = 0;
                        decimal v_flete = 0;
                        decimal v_carga = 0;
                        decimal v_material = 0;
                        decimal v_tecno = 0;
                        decimal v_regal = 0;
                        decimal v_rever = 0;

                        v_comision = (se6_Comision.Text != null && se6_Comision.Text.Length > 0) ? decimal.Parse(se6_Comision.Text) : 0;
                        v_flete = (se6_FleteSeguro.Text != null && se6_FleteSeguro.Text.Length > 0) ? decimal.Parse(se6_FleteSeguro.Text) : 0;
                        v_carga = (se6_CargaDescarga.Text != null && se6_CargaDescarga.Text.Length > 0) ? decimal.Parse(se6_CargaDescarga.Text) : 0;
                        v_material = (se6_MaterialAportado.Text != null && se6_MaterialAportado.Text.Length > 0) ? decimal.Parse(se6_MaterialAportado.Text) : 0;
                        v_tecno = (se6_TecnoAportada.Text != null && se6_TecnoAportada.Text.Length > 0) ? decimal.Parse(se6_TecnoAportada.Text) : 0;
                        v_regal = (se6_Regalias.Text != null && se6_Regalias.Text.Length > 0) ? decimal.Parse(se6_Regalias.Text) : 0;
                        v_rever = (se6_Reversiones.Text != null && se6_Reversiones.Text.Length > 0) ? decimal.Parse(se6_Reversiones.Text) : 0;
                        lbl06Total.InnerText = (v_comision + v_flete + v_carga + v_material + v_tecno + v_regal + v_rever).ToString();

                        //07 No Incrementable
                        txt7_Moneda.Text = dt.Rows[0]["07_noincrementablesmodena"].ToString();
                        se7_GastoNoRelacionado.Text = (dt.Rows[0]["07_noincremetablesrelacionados"].ToString().Trim() == string.Empty) ? "0" : dt.Rows[0]["07_noincremetablesrelacionados"].ToString();
                        se7_FleteSeguro.Text = (dt.Rows[0]["07_noincremetablesfletes"].ToString().Trim() == string.Empty) ? "0" : dt.Rows[0]["07_noincremetablesfletes"].ToString();
                        se7_GastosConstruccion.Text = (dt.Rows[0]["07_noincremetablescontribucion"].ToString().Trim() == string.Empty) ? "0" : dt.Rows[0]["07_noincremetablescontribucion"].ToString();
                        se7_Inst.Text = (dt.Rows[0]["07_noincremetablesotros"].ToString().Trim() == string.Empty) ? "0" : dt.Rows[0]["07_noincremetablesotros"].ToString();
                        se7_Contribuciones.Text = (dt.Rows[0]["07_noincremetablescontribuciones"].ToString().Trim() == string.Empty) ? "0" : dt.Rows[0]["07_noincremetablescontribuciones"].ToString();
                        se7_Dividendos.Text = (dt.Rows[0]["07_noincremetablesdividendos"].ToString().Trim() == string.Empty) ? "0" : dt.Rows[0]["07_noincremetablesdividendos"].ToString();

                        decimal v7_norela = 0;
                        decimal v7_flete = 0;
                        decimal v7_const = 0;
                        decimal v7_inst = 0;
                        decimal v7_contr = 0;
                        decimal v7_divi = 0;

                        v7_norela = (se7_GastoNoRelacionado.Text != null && se7_GastoNoRelacionado.Text.Length > 0) ? decimal.Parse(se7_GastoNoRelacionado.Text) : 0;
                        v7_flete = (se7_FleteSeguro.Text != null && se7_FleteSeguro.Text.Length > 0) ? decimal.Parse(se7_FleteSeguro.Text) : 0;
                        v7_const = (se7_GastosConstruccion.Text != null && se7_GastosConstruccion.Text.Length > 0) ? decimal.Parse(se7_GastosConstruccion.Text) : 0;
                        v7_inst = (se7_Inst.Text != null && se7_Inst.Text.Length > 0) ? decimal.Parse(se7_Inst.Text) : 0;
                        v7_contr = (se7_Contribuciones.Text != null && se7_Contribuciones.Text.Length > 0) ? decimal.Parse(se7_Contribuciones.Text) : 0;
                        v7_divi = (se7_Dividendos.Text != null && se7_Dividendos.Text.Length > 0) ? decimal.Parse(se7_Dividendos.Text) : 0;
                        lbl07Total.InnerText = (v7_norela + v7_flete + v7_const + v7_inst + v7_contr + v7_divi).ToString();

                        //08 Valor Aduana Conforme al método de valor de transacción
                        se8_Pagar.Text = (dt.Rows[0]["08_preciopagadoporpagar"].ToString().Trim() == string.Empty) ? "0" : dt.Rows[0]["08_preciopagadoporpagar"].ToString();
                        txt8_Moneda1.Text = dt.Rows[0]["08_monedapreciopagadoporpagar"].ToString();
                        se8_Ajuste.Text = (dt.Rows[0]["08_ajusteincrementables"].ToString().Trim() == string.Empty) ? "0" : dt.Rows[0]["08_ajusteincrementables"].ToString();
                        txt8_Moneda2.Text = dt.Rows[0]["08_monedaajusteincrementables"].ToString();
                        se8_Aduana.Text = (dt.Rows[0]["08_valorenaduana"].ToString().Trim() == string.Empty) ? "0" : dt.Rows[0]["08_valorenaduana"].ToString();
                        txt8_Moneda3.Text = dt.Rows[0]["08_monedavalorenaduana"].ToString();                        

                        //09 La Presente Determinación del Valor es Valida para
                        txt9_PediNum.Text = dt.Rows[0]["09_pedimentonumero"].ToString();
                        if (dt.Rows[0]["09_fechapedimento_dmy"].ToString().Trim().Length > 10)
                            date9_FechaPedimento.Text = dt.Rows[0]["09_fechapedimento_dmy"].ToString().Substring(0, 10);
                        else
                            date9_FechaPedimento.Text = dt.Rows[0]["09_fechapedimento_dmy"].ToString();                        
                        txt9_FacturaNumero.Text = dt.Rows[0]["09_facturanumero"].ToString();
                        if (dt.Rows[0]["09_fechafactura_dmy"].ToString().Trim().Length > 10)
                            date9_FechaFactura.Text = dt.Rows[0]["09_fechafactura_dmy"].ToString().Substring(0, 10);
                        else
                            date9_FechaFactura.Text = dt.Rows[0]["09_fechafactura_dmy"].ToString();
                        if (dt.Rows[0]["09_masdeunpedimento"].ToString().Trim().Length > 0)
                            chk9_MasPedimento.Checked = true;
                        else
                            chk9_MasPedimento.Checked = false;
                        txt9_LugarFactura.Text = dt.Rows[0]["09_lugaremisionfactura"].ToString();
                        if (dt.Rows[0]["09_tipofacturaunico"].ToString().Trim().Length > 0)
                            chk9_FacturaUnica.Checked = true;
                        else
                            chk9_FacturaUnica.Checked = false;
                        if (dt.Rows[0]["09_subdivision"].ToString().Trim().Length > 0)
                            chk9_Subdivion.Checked = true;
                        else
                            chk9_Subdivion.Checked = false;

                        //10 Valor Aduana Según Determinado por Otros Métodos
                        se_ValorAduana.Text = (dt.Rows[0]["10_valoraduanadeterminado"].ToString().Trim() == string.Empty) ? "0" : dt.Rows[0]["10_valoraduanadeterminado"].ToString();

                        //12 Método para la Determinación del Valor en Aduana
                        if (dt.Rows[0]["12_valordetransaccionesidenticas"].ToString().Trim().Length > 0)
                            chk12_Identicas.Checked = true;
                        else
                            chk12_Identicas.Checked = false;
                        if (dt.Rows[0]["12_valordetransaccionessimilares"].ToString().Trim().Length > 0)
                            chk12_Similares.Checked = true;
                        else
                            chk12_Similares.Checked = false;
                        if (dt.Rows[0]["12_valorpreciounitario"].ToString().Trim().Length > 0)
                            chk12_Venta.Checked = true;
                        else
                            chk12_Venta.Checked = false;
                        if (dt.Rows[0]["12_valorreconstruido"].ToString().Trim().Length > 0)
                            chk12_Reconstruido.Checked = true;
                        else
                            chk12_Reconstruido.Checked = false;
                        if (dt.Rows[0]["12_valordeterminado"].ToString().Trim().Length > 0)
                            chk12_Articulo.Checked = true;
                        else
                            chk12_Articulo.Checked = false;

                        //13 Datos Representante Legal
                        Session["REPRESENTANTEKEY"] = null;
                        if (dt.Rows[0]["REPRESENTANTEKEY"].ToString().Trim().Length > 0)
                        {
                            Session["REPRESENTANTEKEY"] = int.Parse(dt.Rows[0]["REPRESENTANTEKEY"].ToString().Trim());
                            if (dt.Rows[0]["MuestraFirma"].ToString().Trim().Length > 0 && dt.Rows[0]["MuestraFirma"].ToString().Trim().ToUpper().Contains("TRUE"))
                                cbx_MostrarFirma.Checked = true;
                            else
                                cbx_MostrarFirma.Checked = false;
                        }
                        else
                            cbx_MostrarFirma.Checked = false;

                        Session["nombrerepresentntelegal"] = dt.Rows[0]["nombrerepresentntelegal"].ToString();
                        txtRL_Nombre.Text = dt.Rows[0]["NOMBRE"].ToString().Trim();
                        txtRL_Paterno.Text = dt.Rows[0]["APELLIDOPATERNO"].ToString().Trim();
                        txtRL_Materno.Text = dt.Rows[0]["APELLIDOMATERNO"].ToString().Trim();
                        //txtRL_RFC.Text = dt.Rows[0]["rfcrepresentntelegal"].ToString().Trim();
                        txtRL_RFC.Text = dt.Rows[0]["RFC"].ToString().Trim();
                        txtRL_ApartirDe.Text = dt.Rows[0]["APARTIRDE"].ToString().Trim();
                        if (txtRL_ApartirDe.Text.Trim().Length > 10)
                            txtRL_ApartirDe.Text = txtRL_ApartirDe.Text.Trim().Substring(0, 10);
                        txtRL_Hasta.Text = dt.Rows[0]["HASTA"].ToString().Trim();
                        if (txtRL_Hasta.Text.Trim().Length > 10)
                            txtRL_Hasta.Text = txtRL_Hasta.Text.Trim().Substring(0, 10);
                        txtRL_Correo.Text = dt.Rows[0]["CORREOELECTRONICO"].ToString().Trim();
                        txtRL_Tel.Text = dt.Rows[0]["TELEFONO"].ToString().Trim();
                        if (dt.Rows[0]["FIRMAIMAGE0"].ToString().Trim().Length > 0)
                            ASPxBinaryImageRL.Value = (byte[])dt.Rows[0]["FIRMAIMAGE0"];
                        else
                            ASPxBinaryImageRL.Value = null;

                        Session["ASPxBinaryImageRL"] = ASPxBinaryImageRL.Value;

                        txt14_Patente.Text = dt.Rows[0]["patente"].ToString();
                      
                    }
                    else
                    {
                        MultiView1.ActiveViewIndex = 0;
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
                if (Session["Cadena"] == null || Session["hckey"] == null)
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
                dt = informes.EliminaRegistroEnHojaCalculo(Session["hckey"].ToString(), Session["Cadena"].ToString(), ref mensaje);

                lkb_LimpiarFiltros_Click(null, null);

                //Cargar de nuevo Hoja de Cálculo
                
                Grid.DataSource = Session["Grid"] = dt;
                Grid.DataBind();
                Grid.Settings.VerticalScrollableHeight = 230;
                Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                Grid.SettingsPager.PageSize = 20;

            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                //excepcion.RegistrarExcepcion(idusuario, "IA_HojaCalculo-btnOk_Click", ex, Session["Cadena"].ToString(), ref mensaje);
            }
        }

        //Editar Grid2 detallehc
        protected void lkb_EditarDetalle_Click(object sender, EventArgs e)
        {
            int valida_select = 0;

            for (int i = 0; i < Grid2.VisibleRowCount; i++)
            {
                if (Grid2.Selection.IsRowSelected(i))
                {
                    //Abre Modal
                    MostrarModal2();

                    //Titulo del Modal
                    Modal2Titulo.InnerText = "Orden: " + Grid2.GetSelectedFieldValues("orden")[0].ToString().Trim();                    

                    //Asignar valor al campo txt
                    Session["detallehckey"] = Grid2.GetSelectedFieldValues("detallehckey")[0].ToString().Trim();
                    txt_fraccion.Text = Grid2.GetSelectedFieldValues("fraccion")[0].ToString().Trim();
                    txt_Descripcion.Text = Grid2.GetSelectedFieldValues("descripcion")[0].ToString().Trim();
                    txt_cantidad.Text = Grid2.GetSelectedFieldValues("cantidad")[0].ToString().Trim();
                    txt_paisprod.Text = Grid2.GetSelectedFieldValues("paisproduccion")[0].ToString().Trim();
                    txt_paisproc.Text = Grid2.GetSelectedFieldValues("paisprocedencia")[0].ToString().Trim();                    

                    valida_select = 1;
                }
            }

            if (valida_select == 0)
                AlertError("Debe seleccionar una fila para poder editar");
        }

        //Botón guardar la sesion del grid2
        protected void btnGuardarSessionDetalle_Click(object sender, EventArgs e)
        {
            if (Session["Grid2"] != null)
            {
                foreach (DataRow fila in ((DataTable)Session["Grid2"]).Rows)
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

                Grid2.DataSource = Session["Grid2"];
                Grid2.DataBind();
                Grid2.Settings.VerticalScrollableHeight = 168;
                Grid2.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                Grid2.SettingsPager.PageSize = 20;

            }
        }


        //Editar Grid3 faturahc
        protected void lkb_EditarFactura_Click(object sender, EventArgs e)
        {
            int valida_select = 0;

            for (int i = 0; i < Grid3.VisibleRowCount; i++)
            {
                if (Grid3.Selection.IsRowSelected(i))
                {
                    //Abre Modal
                    MostrarModal3();

                    //Titulo del Modal
                    ModalTituloF.InnerText = "Factura: " + Grid3.GetSelectedFieldValues("Factura")[0].ToString().Trim();

                    //Asignar valor
                    Session["facturahckey"] = Grid3.GetSelectedFieldValues("facturahckey")[0].ToString().Trim();
                    txtF_Factura.Text = Grid3.GetSelectedFieldValues("Factura")[0].ToString().Trim();
                    txtF_LugarEmision.Text = Grid3.GetSelectedFieldValues("lugaremision")[0].ToString().Trim();
                    txtF_Subdivision.Text = Grid3.GetSelectedFieldValues("subdivision")[0].ToString().Trim();
                    txtF_Proveedor.Text = Grid3.GetSelectedFieldValues("proveedor")[0].ToString().Trim();
                    txtF_TaxNumero.Text = Grid3.GetSelectedFieldValues("taxnumero")[0].ToString().Trim();
                    txtF_Calle.Text = Grid3.GetSelectedFieldValues("facturacalle")[0].ToString().Trim();
                    txtF_INTEXT.Text = Grid3.GetSelectedFieldValues("facturanumero")[0].ToString().Trim();
                    txtF_Ciudad.Text = Grid3.GetSelectedFieldValues("facturaciudad")[0].ToString().Trim();
                    txtF_Pais.Text = Grid3.GetSelectedFieldValues("facturapais")[0].ToString().Trim();

                    valida_select = 1;
                }
            }

            if (valida_select == 0)
                AlertError("Debe seleccionar una fila para poder editar");
        }

        //Botón guardar la sesion del grid3
        protected void btnGuardarSessionFactura_Click(object sender, EventArgs e)
        {
            if (Session["Grid3"] != null)
            {
                foreach (DataRow fila in ((DataTable)Session["Grid3"]).Rows)
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

                Grid3.DataSource = Session["Grid3"];
                Grid3.DataBind();
                Grid3.Settings.VerticalScrollableHeight = 168;
                Grid3.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                Grid3.SettingsPager.PageSize = 20;

            }
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
                int opcion = 1;  //1:actualiza tabla hojacalculo y detallehc, otro número: actualiza solo la tabla detallehc

                //Valida puntos 4
                string pago41 = string.Empty;
                string pago42 = string.Empty;
                string pago43 = string.Empty;
                string pago44 = string.Empty;
                string pago45 = string.Empty;

                if (chk41S.Checked) pago41 = "SI"; else pago41 = "NO";
                if (chk42S.Checked) pago42 = "SI"; else pago42 = "NO";
                if (chk43S.Checked) pago43 = "SI"; else pago43 = "NO";
                if (chk44S.Checked) pago44 = "SI"; else pago44 = "NO";
                if (chk45S.Checked) pago45 = "SI"; else pago45 = "NO";


                //Valida punto 10
                decimal valor_aduana = 0;

                if (se_ValorAduana.Text.Length > 0)
                    valor_aduana = decimal.Parse(se_ValorAduana.Text);
                
                DateTime? v_date9_FechaPedimento = string.IsNullOrEmpty(date9_FechaPedimento.Text) ? (DateTime?)null : DateTime.Parse(date9_FechaPedimento.Text);
                
                DateTime? v_date9_FechaFactura = string.IsNullOrEmpty(date9_FechaFactura.Text) ? (DateTime?)null : DateTime.Parse(date9_FechaFactura.Text);



                //Valida puntos 9
                string v_chk9_MasPedimento = string.Empty;
                if (chk9_MasPedimento.Checked) v_chk9_MasPedimento = "SI"; else v_chk9_MasPedimento = "NO";

                string v_chk9_FacturaUnica = string.Empty;
                if (chk9_FacturaUnica.Checked) v_chk9_FacturaUnica = "SI"; else v_chk9_FacturaUnica = "NO";
                
                string v_chk9_Subdivion = string.Empty;
                if (chk9_Subdivion.Checked) v_chk9_Subdivion = "SI"; else v_chk9_Subdivion = "NO";
                
                    string v_chk12_Identicas = string.Empty;
                if (chk12_Identicas.Checked) v_chk12_Identicas = "SI"; else v_chk12_Identicas = "NO";
                
                    string v_chk12_Similares = string.Empty;
                if (chk12_Similares.Checked) v_chk12_Similares = "SI"; else v_chk12_Similares = "NO";
                
                 string v_chk12_Venta = string.Empty;
                if (chk12_Venta.Checked) v_chk12_Venta = "SI"; else v_chk12_Venta = "NO";
                
                string v_chk12_Reconstruido = string.Empty;
                if (chk12_Reconstruido.Checked) v_chk12_Reconstruido = "SI"; else v_chk12_Reconstruido = "NO";
                
                string v_chk12_Articulo = string.Empty;
                 if (chk12_Articulo.Checked) v_chk12_Articulo = "SI"; else v_chk12_Articulo = "NO";
                

                //Valida punto 13 Representante Legal
                string v_nombrecompleto = txtRL_Paterno.Text.Trim() + " " + txtRL_Materno.Text.Trim() + " " + txtRL_Nombre.Text.Trim();

                //Mostrar Firma
                int muestraFirma = 1;

                if (!cbx_MostrarFirma.Checked)
                    muestraFirma = 0;

                if (Session["Grid2"] != null)
                {

                    //Obtener valor de Representantekey
                    int repkey = 0;
                    if(Session["REPRESENTANTEKEY"] != null && Session["REPRESENTANTEKEY"].ToString().Trim().Length > 0)
                        repkey = int.Parse(Session["REPRESENTANTEKEY"].ToString().Trim());


                    foreach (DataRow fila in ((DataTable)Session["Grid2"]).Rows)
                    {

                        dt = informes.ActualizaHojaCalculoYDetalle(fila["hckey"].ToString(), txt01_Nombre.Text.Trim(), txt01_RFC.Text.Trim(), txt01_Calle.Text,
                        txt01_NoExtInt.Text.Trim(), txt01_CodigoPostal.Text.Trim(), txt01_Colonia.Text.Trim(), txt02_Nombre.Text.Trim(), txt02_TaxNumber.Text.Trim(),
                        txt02_Calle.Text.Trim(), txt02_NoExtInt.Text.Trim(), txt02_Ciudad.Text.Trim(), txt02_Pais.Text.Trim(), pago41, pago42, pago43, pago44, pago45,
                        txt5_MonedaPago.Text.Trim(), decimal.Parse(se5_PagoDirecto.Text.Trim()), decimal.Parse(se5_Contraprestacion.Text.Trim()),
                        decimal.Parse(lbl05Total.InnerText.Trim()), txt6_Moneda.Text.Trim(), decimal.Parse(se6_Comision.Text.Trim()), decimal.Parse(se6_FleteSeguro.Text.Trim()),
                        decimal.Parse(se6_CargaDescarga.Text.Trim()), decimal.Parse(se6_MaterialAportado.Text.Trim()), decimal.Parse(se6_TecnoAportada.Text.Trim()), decimal.Parse(se6_Regalias.Text.Trim()),
                        decimal.Parse(se6_Reversiones.Text.Trim()), decimal.Parse(lbl06Total.InnerText.Trim()),txt7_Moneda.Text.Trim(),decimal.Parse(se7_GastoNoRelacionado.Text.Trim()),
                        decimal.Parse(se7_FleteSeguro.Text.Trim()), decimal.Parse(se7_GastosConstruccion.Text.Trim()), decimal.Parse(se7_Inst.Text.Trim()), decimal.Parse(se7_Contribuciones.Text.Trim()),
                        decimal.Parse(se7_Dividendos.Text.Trim()), decimal.Parse(lbl07Total.InnerText.Trim()),decimal.Parse(se8_Pagar.Text.Trim()),txt8_Moneda1.Text.Trim(),
                        decimal.Parse(se8_Ajuste.Text.Trim()), txt8_Moneda2.Text.Trim(), decimal.Parse(se8_Aduana.Text.Trim()),txt8_Moneda3.Text.Trim(),txt9_PediNum.Text.Trim(),
                        v_date9_FechaPedimento,txt9_FacturaNumero.Text.Trim(), v_date9_FechaFactura, v_chk9_MasPedimento,txt9_LugarFactura.Text.Trim(),v_chk9_FacturaUnica,
                        v_chk9_Subdivion,decimal.Parse(se_ValorAduana.Text.Trim()),v_chk12_Identicas,v_chk12_Similares,v_chk12_Venta,v_chk12_Reconstruido,v_chk12_Articulo,
                        valor_aduana, Int64.Parse(fila["detallehckey"].ToString()),fila["fraccion"].ToString(), fila["descripcion"].ToString(),
                        decimal.Parse(fila["cantidad"].ToString()), fila["paisproduccion"].ToString(), fila["paisprocedencia"].ToString(), v_nombrecompleto,
                        txtRL_RFC.Text.Trim(), txt14_Patente.Text.Trim(), txt_Referencia.Text.Trim(), opcion, repkey, muestraFirma, Session["Cadena"].ToString(), ref mensaje);

                        opcion += 1;
                    }
                }

                if (Session["Grid3"] != null)
                {
                    DataTable dtF = new DataTable();
                    foreach (DataRow fila in ((DataTable)Session["Grid3"]).Rows)
                    {                        
                        mensaje = string.Empty;
                        dtF = informes.ActualizaFacturas(Int64.Parse(fila["facturahckey"].ToString()), fila["Factura"].ToString(), fila["lugaremision"].ToString(),
                            fila["subdivision"].ToString(), fila["proveedor"].ToString(), fila["taxnumero"].ToString(), fila["facturacalle"].ToString(),
                            fila["facturanumero"].ToString(), fila["facturaciudad"].ToString(), fila["facturapais"].ToString(), Session["Cadena"].ToString(), ref mensaje);
                    }
                }


                lkb_LimpiarFiltros_Click(null, null);

                //Cargar de nuevo Hoja de Cálculo
                Grid.DataSource = Session["Grid"] = dt;
                Grid.DataBind();
                Grid.Settings.VerticalScrollableHeight = 230;
                Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                Grid.SettingsPager.PageSize = 20;



                //Regresa a la pantalla anterior
                MultiView1.ActiveViewIndex = 0;

            }
            catch (Exception ex)
            {
                AlertError(ex.Message);
            }
        }


        #region CANCELADO - Métodos del DROPDOWN CON GRID INTERNO

        //protected void GridView_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
        //{

        //    DevExpress.Web.ASPxGridView grid = (DevExpress.Web.ASPxGridView)DropDownEdit.FindControl("GridView");
        //    object[] employeeNames = new object[grid.VisibleRowCount];
        //    object[] keyValues = new object[grid.VisibleRowCount];


        //    for (int i = 0; i < grid.VisibleRowCount; i++)
        //    {
        //        employeeNames[i] = "(ProductName)" + grid.GetRowValues(i, "ProductName") + ", (SupplierID)" + grid.GetRowValues(i, "SupplierID") + ", (UnitPrice)" +
        //                           grid.GetRowValues(i, "UnitPrice");//grid.GetRowValues(i, "FirstName") + " " + grid.GetRowValues(i, "LastName");
        //        keyValues[i] = grid.GetRowValues(i, grid.KeyFieldName);
        //    }

        //    e.Properties["cpEmployeeNames"] = employeeNames;
        //    e.Properties["cpKeyValues"] = keyValues;
        //}


        //protected void GridView_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        //{
        //    DevExpress.Web.ASPxGridView grid = sender as DevExpress.Web.ASPxGridView;
        //    //e.NewValues["ID"] = EmployeeSessionProvider.GenerateNewID();
        //}


        //protected void GridView_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        //{
        //    SynchronizeFocusedRow();
        //}


        //protected void SynchronizeFocusedRow()
        //{
        //    DevExpress.Web.ASPxGridView grid = (DevExpress.Web.ASPxGridView)DropDownEdit.FindControl("GridView");
        //    object lookupKeyValue = DropDownEdit.KeyValue;
        //    grid.FocusedRowIndex = grid.FindVisibleIndexByKeyValue(lookupKeyValue);
        //}


        //protected void GridView_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        //{
        //    DevExpress.Web.ASPxGridView grid = sender as DevExpress.Web.ASPxGridView;
        //    grid.ScrollToVisibleIndexOnClient = 0;
        //}

        #endregion


        #region REPRESENTANTE LEGAL

        //Metodo que llama al Callback para actualizar el PageSize y el Grid
        protected void GridRL_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            GridPageSizeRL = int.Parse(e.Parameters);
            GridRL.SettingsPager.PageSize = GridPageSizeRL;
            GridRL.DataBind();
        }

        //Botón que abre el modal para ver el grid de representantes legales
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


                //Seleccionar el checkbox por los valores de los campos en reprentante legal
                for (int i = 0; i < GridRL.VisibleRowCount; i++)
                {
                    string v_apartirDe = GridRL.GetRowValues(i, "APARTIRDE").ToString().ToUpper().Trim();
                    if (v_apartirDe.Length > 10)
                        v_apartirDe = v_apartirDe.Substring(0, 10);

                    string v_hasta = GridRL.GetRowValues(i, "HASTA").ToString().ToUpper().Trim();
                    if (v_hasta.Length > 10)
                        v_hasta = v_hasta.Substring(0, 10);

                    ASPxCheckBox chkConsultar = GridRL.FindRowCellTemplateControl(i, (GridViewDataColumn)GridRL.Columns["Seleccionar"], "chkConsultar") as ASPxCheckBox;
                    if(txtRL_Nombre.Text.ToUpper().Trim() + txtRL_Paterno.Text.ToUpper().Trim() + txtRL_Materno.Text.ToUpper().Trim() + txtRL_RFC.Text.ToUpper().Trim() +
                       txtRL_ApartirDe.Text.Trim() + txtRL_Hasta.Text.Trim() == 
                       GridRL.GetRowValues(i, "NOMBRE").ToString().ToUpper().Trim() + GridRL.GetRowValues(i, "APELLIDOPATERNO").ToString().ToUpper().Trim() +
                       GridRL.GetRowValues(i, "APELLIDOMATERNO").ToString().ToUpper().Trim() + GridRL.GetRowValues(i, "RFC").ToString().ToUpper().Trim() +
                       v_apartirDe + v_hasta)
                    {
                        chkConsultar.Checked = true;

                        //Habilitar botón de editar
                        string cledit = lkb_EditarRL.CssClass;
                        if (cledit.Contains("disabled"))
                        {
                            cledit = cledit.Replace("disabled","");
                            lkb_EditarRL.CssClass = cledit;
                        }

                        //Habilitar botón de eliminar
                        string cldelete = lkb_EliminarRL.CssClass;
                        if (cldelete.Contains("disabled"))
                        {
                            cldelete = cldelete.Replace("disabled", "");
                            lkb_EliminarRL.CssClass = cldelete;
                        }

                        ////Habilitar botón de Aceptar
                        //string cldacept = btnRegresarRL.CssClass;
                        //if (cldacept.Contains("disabled"))
                        //{
                        //    cldacept = cldacept.Replace("disabled", "");
                        //    btnRegresarRL.CssClass = cldacept;
                        //}                        
                    }
                    else
                        chkConsultar.Checked = false;
                }


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

                    ////Deshabilitar botón de Aceptar
                    //string cldacept = btnRegresarRL.CssClass;
                    //if (!cldacept.Contains("disabled"))
                    //{
                    //    cldacept = cldacept + " " + "disabled";
                    //    btnRegresarRL.CssClass = cldacept;
                    //}

                    txtRL_Nombre.Text = string.Empty;
                    txtRL_Paterno.Text = string.Empty;
                    txtRL_Materno.Text = string.Empty;
                    txtRL_RFC.Text = string.Empty;
                    txtRL_ApartirDe.Text = string.Empty;
                    txtRL_Hasta.Text = string.Empty;
                    txtRL_Correo.Text = string.Empty;
                    txtRL_Tel.Text = string.Empty;
                    ASPxBinaryImageRL.Value = null;
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
                excepcion.RegistrarExcepcion(idusuario, "IA_HojaCalculo-lkb_RL_Click", ex, lblCadena.Text, ref mensaje);
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
                        txtRL_Nombre.Text = GridRL.GetRowValues(i, "NOMBRE").ToString().Trim();
                        txtRL_Paterno.Text = GridRL.GetRowValues(i, "APELLIDOPATERNO").ToString().Trim();
                        txtRL_Materno.Text = GridRL.GetRowValues(i, "APELLIDOMATERNO").ToString().Trim();
                        txtRL_RFC.Text = GridRL.GetRowValues(i, "RFC").ToString().Trim();
                        txtRL_ApartirDe.Text = GridRL.GetRowValues(i, "APARTIRDE").ToString().Trim();
                        if (txtRL_ApartirDe.Text.Trim().Length > 10)
                            txtRL_ApartirDe.Text = txtRL_ApartirDe.Text.Trim().Substring(0, 10);
                        txtRL_Hasta.Text = GridRL.GetRowValues(i, "HASTA").ToString().Trim();
                        if (txtRL_Hasta.Text.Trim().Length > 10)
                            txtRL_Hasta.Text = txtRL_Hasta.Text.Trim().Substring(0, 10);
                        txtRL_Correo.Text = GridRL.GetRowValues(i, "CORREOELECTRONICO").ToString().Trim();
                        txtRL_Tel.Text = GridRL.GetRowValues(i, "TELEFONO").ToString().Trim();
                        if (GridRL.GetRowValues(i, "FIRMA").ToString().Trim().Length > 0)                   
                            ASPxBinaryImageRL.Value = (byte[])GridRL.GetRowValues(i, "FIRMA");                
                        else
                            ASPxBinaryImageRL.Value = null;


                        Session["REPRESENTANTEKEY"] = GridRL.GetRowValues(i, "REPRESENTANTEKEY").ToString().Trim();
                        cbx_MostrarFirma.Checked = true;
                        
                        break;
                    }
                }

                //Limpia los controles
                if(valida.Equals(0))
                {
                    txtRL_Nombre.Text = string.Empty;
                    txtRL_Paterno.Text = string.Empty;
                    txtRL_Materno.Text = string.Empty;
                    txtRL_RFC.Text = string.Empty;
                    txtRL_ApartirDe.Text = string.Empty;
                    txtRL_Hasta.Text = string.Empty;
                    txtRL_Correo.Text = string.Empty;
                    txtRL_Tel.Text = string.Empty;
                    ASPxBinaryImageRL.Value = null;
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
                excepcion.RegistrarExcepcion(idusuario, "IA_HojaCalculo-lkb_RL_Click", ex, lblCadena.Text, ref mensaje);
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
                excepcion.RegistrarExcepcion(idusuario, "IA_HojaCalculo-chkConsultar_Init", ex, lblCadena.Text, ref mensaje);
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
                //excepcion.RegistrarExcepcion(idusuario, "IA_HojaCalculo-btnOkR_Click", ex, Session["Cadena"].ToString(), ref mensaje);
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