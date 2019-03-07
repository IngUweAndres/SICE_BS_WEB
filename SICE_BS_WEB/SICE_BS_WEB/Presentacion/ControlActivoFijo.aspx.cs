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
using System.Text;
using System.Timers;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Linq;
using System.Diagnostics;
using SICE_BS_WEB.WebReference;

namespace SICE_BS_WEB.Presentacion
{
    public partial class ControlActivoFijo : System.Web.UI.Page
    {
        Catalogos catalogo = new Catalogos();
        Inicio inicio = new Inicio();
        Perfiles perfiles = new Perfiles();
        ControlExcepciones excepcion = new ControlExcepciones();
        static string nombreArchivo = string.Empty;
        static string tituloPagina = string.Empty;
        protected static string tituloPanel = string.Empty;
        static bool permisoConsultar = false;
        static bool permisoAgregar = false;
        static bool permisoEditar = false;
        static bool permisoEliminar = false; 
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
                        permisoAgregar = Convert.ToBoolean(Convert.ToInt32(dt.Rows[0]["Agregar"].ToString()));
                        permisoEditar = Convert.ToBoolean(Convert.ToInt32(dt.Rows[0]["Editar"].ToString()));
                        permisoEliminar = Convert.ToBoolean(Convert.ToInt32(dt.Rows[0]["Eliminar"].ToString()));
                        permisoExportar = Convert.ToBoolean(Convert.ToInt32(dt.Rows[0]["Exportar"].ToString()));
                        Page.Title = tituloPagina;
                    }

                    lblRFC.Text = Session["RFC"].ToString().Trim();

                    //Grid Principal
                    Session["Grid"] = null;
                    TituloPanel(string.Empty);

                    DataTable dtc = new DataTable();                    
                    string mensaje = string.Empty;
                    dtc = catalogo.TraerActivosFijos(PEDIMENTO.Text, FOLIO.Text, lblCadena.Text, ref mensaje);
                    if (dtc != null && dtc.Rows.Count > 0)
                    {
                        Grid.DataSource = Session["Grid"] = dtc;
                        Grid.DataBind();
                        Grid.SettingsPager.PageSize = 20;

                        //Selecccionar el primer registro del grid
                        if (Session["Grid"] != null)
                            Grid.Selection.SelectRow(0);
                    }
                    else
                    {
                        Grid.DataSource = Session["Grid"] = dtc;
                        Grid.DataBind();
                        //AlertError("No hay información por el momento");
                    }

                    Grid.Settings.VerticalScrollableHeight = 330;
                    Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
            

                    //Combo Tipo Activo Fijo
                    mensaje = string.Empty;
                    dtc = new DataTable();
                    dtc = catalogo.TraerTiposActivoFijo(lblCadena.Text, ref mensaje);
                    CmbTipoActivo.DataSource = dtc == null ? new DataTable() : dtc;
                    CmbTipoActivo.DataBind();


                    //Combo Tipo Documento
                    mensaje = string.Empty;
                    dtc = new DataTable();
                    dtc = catalogo.TraerTiposDocumentos(lblCadena.Text, ref mensaje);
                    cmbTipoDocumento.DataSource = dtc == null ? new DataTable() : dtc;
                    cmbTipoDocumento.DataBind();

                    //Se selecciona el primer registro
                    if (dtc != null)
                    {
                        if (dtc.Rows.Count > 0)
                            cmbTipoDocumento.SelectedIndex = 0;
                        else
                            cmbTipoDocumento.SelectedIndex = -1;
                    }

                    //Combo Pedimento
                    Session["Pedimentos"] = null;
                    mensaje = string.Empty;
                    dtc = new DataTable();
                    dtc = catalogo.TraerPedimentosTodos(lblCadena.Text, ref mensaje);
                    
                    if (dtc != null && dtc.Rows.Count > 0)
                        Session["Pedimentos"] = dtc;
                    
                    //cmbPedimento.DataSource = dtc == null ? new DataTable() : dtc;
                    //cmbPedimento.DataBind();
                    //cmbPedimento.DataSource = new DataTable();
                    //cmbPedimento.DataBind();                   

                    //cmbPedimento.SelectedIndex = -1;

                    ////Se selecciona el primer registro
                    //if (dtc != null)
                    //{
                    //    if (dtc.Rows.Count > 0)
                    //        cmbPedimento.SelectedIndex = 0;
                    //    else
                    //        cmbPedimento.SelectedIndex = -1;
                    //}

                    //Validamos los permisos
                    var lnk_Agregar = (LinkButton)Grid.Toolbars[0].Items.FindByName("Links").FindControl("lkb_Nuevo");
                    var lnk_Editar = (LinkButton)Grid.Toolbars[0].Items.FindByName("Links").FindControl("lkb_Editar");
                    var lnk_Eliminar = (LinkButton)Grid.Toolbars[0].Items.FindByName("Links").FindControl("lkb_Eliminar");
                    var lkb_Excel = (LinkButton)Grid.Toolbars[0].Items.FindByName("Links").FindControl("lkb_Excel");

                    Session["permisoAgregar"] = permisoAgregar;
                    Session["permisoEditar"] = permisoEditar;
                    Session["permisoEliminar"] = permisoEliminar;
                    Session["permisoExportar"] = permisoExportar;

                    lnk_Agregar.Visible = permisoAgregar;
                    lnk_Editar.Visible = permisoEditar;
                    lnk_Eliminar.Visible = permisoEliminar;
                    lkb_Excel.Visible = permisoExportar;

                    SetColores();
                }
            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "ControlActivoFijo-Page_Load", ex, lblCadena.Text, ref mensaje);
                if (mensaje.Length == 0)
                    mensaje = "Error: " + excepcion.SerializarExMessage(lblCadena.Text, ex);
                //Response.Redirect("Login.aspx");
            }
        }

        private void TituloPanel(string descripcion)
        {
            h1_titulo.InnerText = tituloPanel = tituloPagina + descripcion;
        }

        #region Grid
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

        #region GridDocs
        protected int GridPageSizeDocumentos
        {
            get
            {
                if (Session[PageSizeSessionKey] == null)
                    return GridDocs.SettingsPager.PageSize;
                return (int)Session[PageSizeSessionKey];
            }
            set { Session[PageSizeSessionKey] = value; }
        }
        #endregion

        #region GridPed
        protected int GridPageSizePedimentos
        {
            get
            {
                if (Session[PageSizeSessionKey] == null)
                    return GridPed.SettingsPager.PageSize;
                return (int)Session[PageSizeSessionKey];
            }
            set { Session[PageSizeSessionKey] = value; }
        }
        #endregion

        //Metodo inicial de pantalla
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Session["Grid"] = null;
            }

            Grid.SettingsPager.PageSize = GridPageSize;

            //Cuando se quiera filtrar el Grid entra en el if
            if (Session["Grid"] != null)
            {
                Grid.DataSource = Session["Grid"];
                Grid.DataBind();
                Grid.SettingsPager.PageSize = GridPageSize;
            }

            //Cuando se quiera filtrar el GridDocs entra en el if
            if (Session["GridDocs"] != null)
            {
                GridDocs.DataSource = Session["GridDocs"];
                GridDocs.DataBind();
                GridDocs.SettingsPager.PageSize = GridPageSizeDocumentos;
            }

            //Cuando se quiera filtrar el GridPed entra en el if
            if (Session["GridPed"] != null)
            {
                GridPed.DataSource = Session["GridPed"];
                GridPed.DataBind();
                GridPed.SettingsPager.PageSize = GridPageSizePedimentos;
            }

        }

        //Metodo que llama al Callback para actualizar el PageSize y el Grid
        protected void Grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            GridPageSize = int.Parse(e.Parameters);
            Grid.SettingsPager.PageSize = GridPageSize;
            Grid.DataBind();
        }

        //Metodo que llama al Callback para actualizar el PageSize y el Grid
        protected void GridDocs_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            GridPageSize = int.Parse(e.Parameters);
            GridDocs.SettingsPager.PageSize = GridPageSize;
            GridDocs.DataBind();
        }

        //Metodo que llama al Callback para actualizar el PageSize y el Grid
        protected void GridPed_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            GridPageSize = int.Parse(e.Parameters);
            GridPed.SettingsPager.PageSize = GridPageSize;
            GridPed.DataBind();
        }

        //Metodo que llama al combo box al seleccionar la cantidad de registros en el page
        protected void PagerCombo_Load(object sender, EventArgs e)
        {
            (sender as ASPxComboBox).Value = Grid.SettingsPager.PageSize;
        }

        protected void Grid_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        { }

        protected void GridDocs_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        { }

        protected void GridPed_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        { }

        private void ConfigureExport(ASPxGridView grid)
        {
            PrintingSystemBase ps = new PrintingSystemBase();
            ps.ExportOptions.Xlsx.SheetName = "Datos Generales";

            ps.XlSheetCreated += ps_XlSheetCreated;
            PrintableComponentLinkBase link1 = new PrintableComponentLinkBase(ps);
            link1.PaperName = "DatosGenerales";
            link1.Component = Exporter;

            CompositeLinkBase compositeLink = new CompositeLinkBase(ps);
            compositeLink.Links.AddRange(new object[] { link1 });
            compositeLink.CreatePageForEachLink();

            ps.Dispose();
            grid.Settings.ShowColumnHeaders = true;
        }

        void ps_XlSheetCreated(object sender, XlSheetCreatedEventArgs e)
        {
            if (e.Index == 0) e.SheetName = "Control Activo Fijo";
            if (e.Index == 1) e.SheetName = "Control Activo Fijo";
        }

        protected void lkb_Excel_Click(object sender, EventArgs e)
        {
            if (Grid.VisibleRowCount > 0)
            {
                Exporter.WriteXlsToResponse("Control Activo Fijo", new XlsExportOptionsEx() { SheetName = "Control Activo Fijo" });
            }
            else
                AlertError("No hay información por exportar");

            //Actualiza los permisos de los botones en grid
            PermisosUsuario();

            //Actualiza los permisos de los botones en grid
            PermisosUsuario();
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
                        Exporter.WritePdfToResponse("Control Activo Fijo");
                        break;
                    case "ExportToXLS":
                        Exporter.WriteXlsToResponse("Control Activo Fijo", new XlsExportOptionsEx() { SheetName = "Control Activo Fijo" });
                        break;
                    case "ExportToXLSX":
                        Exporter.WriteXlsxToResponse("Control Activo Fijo", new XlsxExportOptionsEx() { SheetName = "Control Activo Fijo" });
                        break;
                    default:
                        break;
                }
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

            //Actualiza los permisos de los botones en grid
            PermisosUsuario();
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

        //Metodo que muestra ventana de alerta
        public void AlertError(string mensaje)
        {
            pModal.InnerText = mensaje;
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "", "<script> document.getElementById('btnError').setAttribute('data-whatever', '" + mensaje + "'); document.getElementById('btnError').click(); </script> ", false);
        }

        //Metodo que muestra ventana de satisfactorio
        public void AlertSuccess(string mensaje)
        {
            pModalSucces.InnerText = mensaje;
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "", "<script> document.getElementById('btnSuccess').setAttribute('data-whatever', '" + mensaje + "'); document.getElementById('btnSuccess').click(); </script> ", false);
        }

        //Metodo que muestra ventana de pregunta
        public void AlertQuestion(string mensaje, string btnUniqueId)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "",
                "<script> document.getElementById('btnOk').setAttribute('onclick', \"javascript:__doPostBack('" + btnUniqueId + "', '')\"); document.getElementById('btnQuestion').setAttribute('data-whatever', '" + mensaje + "'); document.getElementById('btnQuestion').click(); </script> ", false);
        }

        //Metodo que muestra el modal
        private void MostrarModal()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "MostrarModal", "<script> document.getElementById('btnModal').click(); </script> ", false);
        }

        //Metodo que muestra el modal de Documentos
        private void ModalDocumento()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "ModalDocumento", "<script> document.getElementById('btnModalDocumento').click(); </script> ", false);
        }

        //Metodo que muestra el modal de Pedimentos
        private void ModalPedimentos()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "ModalPedimento", "<script> document.getElementById('btnModalPedimento').click(); </script> ", false);
        }

        //Metodo que muestra el modal
        private void MostrarModalPartida()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "MostrarModalPartida", "<script> document.getElementById('btnModalPartida').click(); </script> ", false);
        }

        //Metodo botón Buscar con los filtros el reporte 
        protected void btnBuscar_OnClick(object sender, EventArgs e)
        {
            //LoadingPanel1.ContainerElementID = "Panel1";
            //System.Threading.Thread.Sleep(3000);

            //Valida Pedimento y Folio
            string mensaje = ""; 
            DataTable dt = new DataTable();

            dt = catalogo.TraerActivosFijos(PEDIMENTO.Text, FOLIO.Text, lblCadena.Text, ref mensaje);
            if (dt != null && dt.Rows.Count > 0)
            {
                Grid.DataSource = Session["Grid"] = dt;
                Grid.DataBind();
                Grid.SettingsPager.PageSize = 20;

                //Selecccionar el primer registro del grid
                if (Session["Grid"] != null)
                    Grid.Selection.SelectRow(0);
            }
            else
            {
                Grid.DataSource = Session["Grid"] = dt;
                Grid.DataBind();
                AlertError("No hay información o intentelo de nuevo");
            }

            Grid.Settings.VerticalScrollableHeight = 330;
            Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;

            //Actualiza los permisos de los botones en grid
            PermisosUsuario();
        }

        protected void lkb_Nuevo_Click(object sender, EventArgs e)
        {
            //Abre Modal
            MostrarModal();

            //Titulo del Modal
            ModalTitulo.InnerText = "Nuevo Activo Fijo";
            DataBind();
                       
            //Limpiar controles
            TxtFolio.Text = string.Empty;
            TxtMarca.Text = string.Empty;
            TxtModelo.Text = string.Empty;
            TxtSerie.Text = string.Empty;
            TxtFraccion.Text = string.Empty;
            TxtDesc.Text = string.Empty;
            txtPaisOrigen.Text = string.Empty;
            CmbTipoActivo.SelectedIndex = -1;
            TxtValorAduana.Text = string.Empty;
            TxtValorComercial.Text = string.Empty;
            TxtDescripcionTecnica.Text = string.Empty;
            TxtPO.Text = string.Empty;
            TxtValorME.Text = string.Empty;
            TxtME.Text = string.Empty;

            //Actualiza los permisos de los botones en grid
            PermisosUsuario();
        }

        protected void lkb_Editar_Click(object sender, EventArgs e)
        {
            string mensaje = string.Empty;
            DataTable dt = new DataTable();
            int valida_select = 0;

            for (int i = 0; i < Grid.VisibleRowCount; i++)
            {
                if (Grid.Selection.IsRowSelected(i))
                {
                    //Abre Modal
                    MostrarModal();

                    //Titulo del Modal
                    ModalTitulo.InnerText = "Editar Activo Fijo";
                    DataBind();

                    //Asignar valor al campo txt
                    Session["ACTIVOKEY"] = Grid.GetSelectedFieldValues("ACTIVOFIJOKEY")[0].ToString().Trim();
                    string tipo = Grid.GetSelectedFieldValues("TIPOACTIVOFIJO")[0].ToString().Trim();
                    string valortipo = string.Empty;
                    foreach (ListEditItem item in CmbTipoActivo.Items)
                    {
                        if(item.Text.Trim().Contains(tipo))
                        {
                            CmbTipoActivo.Value = item.Value;
                            break;
                        }
                    }              
                    TxtFolio.Text = Grid.GetSelectedFieldValues("FOLIOINTERNO")[0].ToString().Trim();
                    TxtMarca.Text = Grid.GetSelectedFieldValues("MARCA")[0].ToString().Trim();
                    TxtModelo.Text = Grid.GetSelectedFieldValues("MODELO")[0].ToString().Trim();
                    TxtSerie.Text = Grid.GetSelectedFieldValues("SERIE")[0].ToString().Trim();
                    TxtFraccion.Text = Grid.GetSelectedFieldValues("FRACCION")[0].ToString().Trim();
                    TxtDesc.Text = Grid.GetSelectedFieldValues("DESCRIPCION")[0].ToString().Trim();
                    txtPaisOrigen.Text = Grid.GetSelectedFieldValues("CLAVEPAISORIGEN")[0].ToString().Trim();
                    TxtValorAduana.Text = Grid.GetSelectedFieldValues("VALORADUANA")[0].ToString().Trim();
                    TxtValorComercial.Text = Grid.GetSelectedFieldValues("VALORCOMERCIAL")[0].ToString().Trim();
                    TxtDescripcionTecnica.Text = Grid.GetSelectedFieldValues("DESCRIPCIONTECNICA")[0].ToString().Trim();
                    TxtPO.Text = Grid.GetSelectedFieldValues("PO")[0].ToString().Trim();
                    TxtValorME.Text = Grid.GetSelectedFieldValues("VALORME")[0].ToString().Trim();
                    TxtME.Text = Grid.GetSelectedFieldValues("ME")[0].ToString().Trim();

                    valida_select = 1;
                }
            }

            if (valida_select == 0)
                AlertError("Debe seleccionar un activo fijo para poder editar");

            //Actualiza los permisos de los botones en grid
            PermisosUsuario();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                //Actualiza los permisos de los botones en grid
                PermisosUsuario();
         
                //Validar que solo exista el Folio y tipo de Activo Fijo
                string valida = string.Empty;
                if (TxtFolio.Text.Trim() == string.Empty && CmbTipoActivo.Text.Trim() == string.Empty)
                {
                    valida = "Para poder guardar escriba un folio y seleccione un tipo de activo";
                }
                else if (TxtFolio.Text.Trim() == string.Empty)
                {
                    valida = "Para poder guardar escriba un folio";
                }
                else if (CmbTipoActivo.Text.Trim() == string.Empty)
                {
                    valida = "Para poder guardar seleccione un tipo de activo";
                }

                //Valida si no se repite el folio
                if (Session["Grid"] != null && ((DataTable)(Session["Grid"])).Rows.Count > 0)
                {
                    foreach (DataRow fila in ((DataTable)(Session["Grid"])).Rows)
                    {
                        //Al guardar
                        if (ModalTitulo.InnerText.Contains("Nuevo") && fila["FOLIOINTERNO"].ToString().Trim().ToUpper() == TxtFolio.Text.Trim().ToUpper())
                        {
                            valida = "El folio ya existe: " + TxtFolio.Text.Trim();
                            break;
                        }

                        //Al editar
                        if (ModalTitulo.InnerText.Contains("Editar") && Session["ACTIVOKEY"] != null &&
                            fila["FOLIOINTERNO"].ToString().Trim().ToUpper() == TxtFolio.Text.Trim().ToUpper() &&
                            fila["ACTIVOFIJOKEY"].ToString().Trim() != Session["ACTIVOKEY"].ToString().Trim())
                        {
                            valida = "El folio ya existe: " + TxtFolio.Text.Trim();
                            break;
                        }
                    }
                }


                if (valida.Length > 0)
                {
                    //Mantiene el modal
                    MostrarModal();

                    //Titulo del Modal
                    if (ModalTitulo.InnerText.Contains("Nuevo"))
                        ModalTitulo.InnerText = "Nuevo Activo Fijo";
                    else
                        ModalTitulo.InnerText = "Editar Activo Fijo";
                    DataBind();

                    AlertError(valida);
                    return;
                }

                
                //Guardar

                if (TxtValorAduana.Text.Trim().Equals(string.Empty))
                    TxtValorAduana.Text = "0";

                if (TxtValorComercial.Text.Trim().Equals(string.Empty))
                    TxtValorComercial.Text = "0";
                
                string mensaje = "";
                DataTable dt = new DataTable();

                if (ModalTitulo.InnerText.Contains("Nuevo"))
                    dt = catalogo.GuardarActivoFijo(TxtFolio.Text.Trim(), TxtMarca.Text.Trim(), TxtModelo.Text.Trim(), TxtSerie.Text.Trim(), TxtFraccion.Text.Trim(), TxtDesc.Text.Trim(),
                                                    CmbTipoActivo.Text, decimal.Parse(TxtValorAduana.Text.Trim()), decimal.Parse(TxtValorComercial.Text.Trim()), 
                                                    int.Parse(Session["IdUsuario"].ToString()), lblCadena.Text, txtPaisOrigen.Text.Trim(), TxtDescripcionTecnica.Text.Trim(), 
                                                    TxtPO.Text.Trim() , decimal.Parse(TxtValorME.Text.Trim()), TxtME.Text.Trim(), ref mensaje);
                else if (ModalTitulo.InnerText.Contains("Editar"))
                    dt = catalogo.EditarActivoFijo(Int64.Parse(Session["ACTIVOKEY"].ToString()), TxtFolio.Text.Trim(), TxtMarca.Text.Trim(), TxtModelo.Text.Trim(), TxtSerie.Text.Trim(), TxtFraccion.Text.Trim(),
                                                   TxtDesc.Text.Trim(), CmbTipoActivo.Text, decimal.Parse(TxtValorAduana.Text.Trim()), decimal.Parse(TxtValorComercial.Text.Trim()),
                                                   int.Parse(Session["IdUsuario"].ToString()), lblCadena.Text, txtPaisOrigen.Text.Trim(), TxtDescripcionTecnica.Text.Trim(),
                                                    TxtPO.Text.Trim(), decimal.Parse(TxtValorME.Text.Trim()), TxtME.Text.Trim(), ref mensaje);


                if (dt != null && dt.Rows.Count > 0)
                {
                    
                    Grid.DataSource = Session["Grid"] = dt;
                    Grid.DataBind();

                    //Selecccionar el primer registro del grid
                    if (Session["Grid"] != null)
                        Grid.Selection.SelectRow(0);
                }
                else
                {
                    Grid.DataSource = Session["Grid"];
                    Grid.DataBind();
                }

                Grid.Settings.VerticalScrollableHeight = 330;
                Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;

                //Actualiza los permisos de los botones en grid
                PermisosUsuario();
            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "ControlActivoFijo-btnGuardar_Click", ex, lblCadena.Text, ref mensaje);
            }

        }

        protected void lkb_Eliminar_Click(object sender, EventArgs e)
        {
            int valida_select = 0;

            for (int i = 0; i < Grid.VisibleRowCount; i++)
            {
                if (Grid.Selection.IsRowSelected(i))
                {
                    string mensaje = string.Empty;
                    DataTable dt = new DataTable();
                    Int64 id = Int64.Parse(Grid.GetSelectedFieldValues("ACTIVOFIJOKEY")[0].ToString().Trim());
                    dt = catalogo.EliminarActivoFijo(id, int.Parse(Session["IdUsuario"].ToString()), lblCadena.Text, ref mensaje);

                    Grid.DataSource = Session["Grid"] = dt;
                    Grid.DataBind();
                    Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Hidden;
                    Grid.SettingsPager.PageSize = 20;

                    //Selecccionar el primer registro del grid
                    if (Session["Grid"] != null)
                        Grid.Selection.SelectRow(0);

                    AlertSuccess("El activo fijo se eliminó con éxito.");
                    valida_select = 1;

                    Grid.Settings.VerticalScrollableHeight = 330;
                    Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                    break;
                }
            }

            if (valida_select == 0)
                AlertError("Debe seleccionar un activo fijo para poder eliminar");

            //Actualiza los permisos de los botones en grid
            PermisosUsuario();
        }

        protected void PermisosUsuario()
        {
            if (Session["permisoAgregar"] != null)
            {
                //Validamos los permisos
                var lnk_Agregar = (LinkButton)Grid.Toolbars[0].Items.FindByName("Links").FindControl("lkb_Nuevo");
                var lnk_Editar = (LinkButton)Grid.Toolbars[0].Items.FindByName("Links").FindControl("lkb_Editar");
                var lnk_Eliminar = (LinkButton)Grid.Toolbars[0].Items.FindByName("Links").FindControl("lkb_Eliminar");
                var lkb_Excel = (LinkButton)Grid.Toolbars[0].Items.FindByName("Links").FindControl("lkb_Excel");

                lnk_Agregar.Visible = bool.Parse(Session["permisoAgregar"].ToString());
                lnk_Editar.Visible = bool.Parse(Session["permisoEditar"].ToString());
                lnk_Eliminar.Visible = bool.Parse(Session["permisoEliminar"].ToString());
                lkb_Excel.Visible = bool.Parse(Session["permisoExportar"].ToString());
            }
        }

        //Evento del Grid para abrir los botones
        protected void Grid_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            try
            {
                //Se obtienen variables del renglón seleccionado
                Session["ACTIVOFIJOKEY"] = Grid.GetRowValues(e.VisibleIndex, "ACTIVOFIJOKEY").ToString();
                Session["TIPOACTIVOFIJO"] = Grid.GetRowValues(e.VisibleIndex, "TIPOACTIVOFIJO").ToString();

                DataTable dt = new DataTable();
                string mensaje = string.Empty;

                if (e.ButtonID == "btnDocs")
                {
                    //Se abre modal Documentos
                    ModalDocumento();

                    //Titulo del Modal Documentos
                    ModalTituloDocumento.InnerText = "Tipo Activo: " + Session["TIPOACTIVOFIJO"].ToString();
                    DataBind();

                    dt = catalogo.TraerDocumentosPorActivo(Int64.Parse(Session["ACTIVOFIJOKEY"].ToString()), lblCadena.Text, ref mensaje);

                    //Se actualiza el grid GridDocs
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        GridDocs.DataSource = Session["GridDocs"] = dt;
                        GridDocs.DataBind();
                        GridDocs.SettingsPager.PageSize = 20;

                        //Selecccionar el primer registro del grid
                        if (Session["GridDocs"] != null)
                            GridDocs.Selection.SelectRow(0);
                    }
                    else
                    {
                        GridDocs.DataSource = Session["GridDocs"] = dt;
                        GridDocs.DataBind();
                    }

                    GridDocs.Settings.VerticalScrollableHeight = 180;
                    GridDocs.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                }
                else if (e.ButtonID == "btnPed")
                {
                    //Se abre modal Pedimentos
                    ModalPedimentos();

                    //Titulo del Modal Documentos
                    ModalTituloPedimento.InnerText = "Tipo Activo: " + Session["TIPOACTIVOFIJO"].ToString();
                    DataBind();

                    //Limpiar campos
                    cmbPedimento.Text = string.Empty;
                    txtPartida.Text = string.Empty;

                    dt = catalogo.TraerPedimentosPorActivo(Int64.Parse(Session["ACTIVOFIJOKEY"].ToString()), lblCadena.Text, ref mensaje);

                    //Se actualiza el grid GridPed
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        GridPed.DataSource = Session["GridPed"] = dt;
                        GridPed.DataBind();
                        GridPed.SettingsPager.PageSize = 20;

                        //Selecccionar el primer registro del grid
                        if (Session["GridPed"] != null)
                            GridPed.Selection.SelectRow(0);
                    }
                    else
                    {
                        GridPed.DataSource = Session["GridPed"] = dt;
                        GridPed.DataBind();
                    }

                    GridPed.Settings.VerticalScrollableHeight = 180;
                    GridPed.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                    GridPed.SettingsEditing.BatchEditSettings.EditMode = GridViewBatchEditMode.Cell;
                    GridPed.SettingsEditing.BatchEditSettings.StartEditAction = GridViewBatchStartEditAction.Click;
                }

            }
            catch (Exception ex)
            {
                int idusuario = 0;
                string mensaje = "";
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "ControlActivoFijo-Grid_CustomButtonCallback", ex, lblCadena.Text, ref mensaje);
            }
        }

        //Este evento se genera cuando se da clic en botón subir archivo en el FileUpload
        protected void upc_Imagen_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            try
            {
                string fileNameComplete = e.UploadedFile.FileName;
                //string name = System.IO.Path.GetFileNameWithoutExtension(fileNameComplete);
                string extension = System.IO.Path.GetExtension(fileNameComplete);
                Session["extensionFile"] = extension;              
                Session["filebyte"] = e.UploadedFile.FileBytes;

                //Se limpia la variable Session["Files"]
                Session["Files"] = null;
                
                DataTable dt = new DataTable();
                dt.Columns.Add("ACTIVOFIJOKEY", typeof(string));
                dt.Columns.Add("nombre_completo", typeof(string));
                dt.Columns.Add("extension", typeof(string));
                dt.Columns.Add("file", typeof(byte[]));
                DataRow row;
                row = dt.NewRow();
                row["ACTIVOFIJOKEY"] = Session["ACTIVOFIJOKEY"].ToString();
                row["nombre_completo"] = fileNameComplete;
                row["extension"] = extension;
                row["file"] = e.UploadedFile.FileBytes;
                dt.Rows.Add(row);
                Session["Files"] = dt;

                //e.CallbackData = fileNameComplete;

                //string valida = string.Empty;
                //foreach (DataRow fila in ((DataTable)(Session["GridDocs"])).Rows)
                //{
                //    //Al guardar
                //    if (fila["ACTIVOFIJOKEY"].ToString().Trim().ToUpper() == Session["ACTIVOFIJOKEY"].ToString().Trim().ToUpper() &&
                //        fila["IMAGEN"].ToString().Trim().ToUpper() == cadenaFile.Trim().ToUpper())
                //    {
                //        valida = "Ya existe el archivo: " + fileNameComplete.Trim();
                //        break;
                //    }
                //}

                //if (valida.Length > 0)
                //{
                //    //Mantiene el modal
                //    ModalDocumento();

                //    //Titulo del Modal
                //    ModalTituloDocumento.InnerText = "Tipo Activo: " + Session["TIPOACTIVOFIJO"].ToString();
                //    DataBind();

                //    AlertError(valida);

                //    MantenerGrid();
                //    return;
                //}
                
            }
            catch (Exception ex)
            {
                int idusuario = 0;
                string mensaje = "";
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "ControlActivoFijo-upc_Imagen_FileUploadComplete", ex, lblCadena.Text, ref mensaje);
            }
        }

        //protected void btnQuitarFile_Click(object sender, EventArgs e)
        //{
        //    System.Threading.Thread.Sleep(1000);
        //    Session["Files"] = null;

        //    ModalDocumento();
        //}

        //Agrega Archivos en Documentos
        protected void btnAgregarDoc_Click(object sender, EventArgs e)
        {
            try
            {
                ModalDocumento();

                if (Session["Files"] != null)
                {
                    foreach (DataRow fila in ((DataTable)(Session["Files"])).Rows)
                    {
                        string mensaje = string.Empty;
                        string activofijokey = fila["ACTIVOFIJOKEY"].ToString();
                        string extension = fila["extension"].ToString();
                        string nombre_completo = fila["nombre_completo"].ToString();
                        byte[] filebyte = (byte[])fila["file"];

                        //Se puede usar para el archivo
                        //(byte[])Session["filebyte"] o filebyte

                        DataTable dt = new DataTable();
                        dt = catalogo.GuardarDocumentosPorActivo(Int64.Parse(activofijokey), filebyte, cmbTipoDocumento.Text, nombre_completo, int.Parse(Session["IdUsuario"].ToString()), lblCadena.Text, ref mensaje);
                        ModalDocumento();

                        //Obtiene datos actualizados
                        if (dt != null && dt.Rows.Count > 0)
                        {                            
                            GridDocs.DataSource = Session["GridDocs"] = dt;
                            GridDocs.DataBind();
                            GridDocs.Settings.VerticalScrollBarMode = ScrollBarMode.Hidden;
                            GridDocs.SettingsPager.PageSize = 20;

                            //Selecccionar el primer registro del grid
                            if (Session["GridDocs"] != null)
                                GridDocs.Selection.SelectRow(0);
                        }
                        else
                        {
                            GridDocs.DataSource = Session["GridDocs"];
                            GridDocs.DataBind();
                        }

                        GridDocs.Settings.VerticalScrollableHeight = 180;
                        GridDocs.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                        DataBind();
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(String), "btnFiles", "<script> document.getElementById('btnFiles').click(); </script> ", false);
                    }
                }
                
            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "ControlActivoFijo-btnGuardar_Click", ex, lblCadena.Text, ref mensaje);
            }

        }        

        //Evento del Grid para el botón de abrir Imagenes
        protected void GridDocs_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            try
            {
                ModalDocumento();

                Session["ACTIVOFIJOKEY"] = GridDocs.GetRowValues(e.VisibleIndex, "ACTIVOFIJOKEY").ToString();
                Session["IMAGENESKEY"] = GridDocs.GetRowValues(e.VisibleIndex, "IMAGENESKEY").ToString();

                //Titulo del Modal Documentos
                ModalTituloDocumento.InnerText = "Tipo Activo: " + Session["TIPOACTIVOFIJO"].ToString();
                DataBind();                

                if (e.ButtonID == "btnOpenImage")
                {
                    string nombre = GridDocs.GetRowValues(e.VisibleIndex, "NOMBREARCHIVO").ToString();
                    string extension = nombre.Trim().Substring(nombre.LastIndexOf("."), (nombre.Length - nombre.LastIndexOf(".")));


                    //Evento del webservices para traer documento y mostrar el archivo

                    TExpediente res;
                    IExpedienteComercioservice v1 = new IExpedienteComercioservice();

                    res = v1.GetPedimento(lblRFC.Text, Session["IMAGENESKEY"].ToString(), "DIAF");
                    string base64BinaryStr = res.Archivo.Trim();

                    //Valida si viene vacio el archivo del wb 
                    if (base64BinaryStr.Trim() == string.Empty)
                    {
                        AlertError("Error al descargar el documento, intentelo más tarde");
                        return;
                    }

                    //Si existe entonces lo abre
                    base64BinaryStr = base64BinaryStr.Replace("*", "+").Replace("-", "/");

                    if (string.IsNullOrEmpty(base64BinaryStr) || base64BinaryStr.Length % 4 != 0
                        || base64BinaryStr.Contains(" ") || base64BinaryStr.Contains("\t") || base64BinaryStr.Contains("\r") || base64BinaryStr.Contains("\n"))
                    { }
                    else
                    {
                        byte[] sPDFDecoded = Convert.FromBase64String(base64BinaryStr);

                        Response.Clear();
                        MemoryStream ms = new MemoryStream(sPDFDecoded);
                        Response.ContentType = "application/" + extension;
                        Response.AddHeader("content-disposition", "attachment;filename=" + nombre);
                        Response.Buffer = true;
                        ms.WriteTo(Response.OutputStream);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                        //Response.End();
                    }



                    ////Abre archivos en su programa correspondiente
                    //string file = @"C:\Users\DESARROLLO2\Desktop\docs mario\alta en IMSSS gruposac\IDSE_Y564656410_Lote_198457714.pdf"; 
                    //ProcessStartInfo pi = new ProcessStartInfo(file);
                    //pi.Arguments = Path.GetFileName(file);
                    //pi.UseShellExecute = true;
                    //pi.WorkingDirectory = Path.GetDirectoryName(file);
                    //pi.FileName = file;
                    //pi.Verb = "OPEN";
                    //Process.Start(pi);
                }
                //else if (e.ButtonID == "btnBorrarDocs")
                //{}
            }
            catch (Exception ex)
            {
                int idusuario = 0;
                string mensaje = "";
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "ControlActivoFijo-GridDocs_CustomButtonCallback", ex, lblCadena.Text, ref mensaje);
            }
        }

        //Elimina Archivos en Documentos
        protected void lkb_EliminarDocs_Click(object sender, EventArgs e)
        {
            ModalDocumento();

            int valida_select = 0;

            for (int i = 0; i < GridDocs.VisibleRowCount; i++)
            {
                if (GridDocs.Selection.IsRowSelected(i))
                {
                    valida_select = 1;
                    string mensaje = string.Empty;
                    DataTable dt = new DataTable();
                    Session["ACTIVOFIJOKEY"] = GridDocs.GetSelectedFieldValues("ACTIVOFIJOKEY")[0].ToString().Trim();
                    Session["IMAGENESKEY"] = GridDocs.GetSelectedFieldValues("IMAGENESKEY")[0].ToString().Trim();

                    Guid newGuid = Guid.Parse(Session["IMAGENESKEY"].ToString());

                    dt = catalogo.EliminarDocumentosPorActivo(Int64.Parse(Session["ACTIVOFIJOKEY"].ToString()), newGuid, int.Parse(Session["IdUsuario"].ToString()), lblCadena.Text, ref mensaje);

                    GridDocs.DataSource = Session["GridDocs"] = dt;
                    GridDocs.DataBind();
                    GridDocs.Settings.VerticalScrollBarMode = ScrollBarMode.Hidden;
                    GridDocs.SettingsPager.PageSize = 20;

                    //Selecccionar el primer registro del grid
                    if (Session["GridDocs"] != null)
                        GridDocs.Selection.SelectRow(0);

                    AlertSuccess("El archivo se eliminó con éxito.");

                    GridDocs.Settings.VerticalScrollableHeight = 180;
                    GridDocs.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                }
            }

            if (valida_select == 0)
                AlertError("Debe seleccionar un archivo para poder eliminar");

        }


        protected void MantenerGrid()
        {
            try
            {
                if (Session["Grid"] != null && ((DataTable)Session["Grid"]).Rows.Count > 0)
                {
                    Grid.DataSource = Session["Grid"];
                    Grid.DataBind();
                    Grid.SettingsPager.PageSize = 20;

                    //Selecccionar el primer registro del grid
                    if (Session["Grid"] != null)
                        Grid.Selection.SelectRow(0);
                }
                else
                {
                    DataTable dt = new DataTable();
                    Grid.DataSource = dt;
                    Grid.DataBind();
                }

                Grid.Settings.VerticalScrollableHeight = 180;
                Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
            }
            catch (Exception ex)
            {
                int idusuario = 0;
                string mensaje = "";
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "ControlActivoFijo-MantenerGrid", ex, lblCadena.Text, ref mensaje);
            }

        }

        //Se usa para refrescar la cantidad de datos en el combo de pedimentos
        protected void ASPxComboBox_OnItemsRequestedByFilterCondition_SQL(object source, ListEditItemsRequestedByFilterConditionEventArgs e)
        {
            DataTable dt = new DataTable();
            ASPxComboBox comboBox = (ASPxComboBox)source;

            if (e.Filter.Trim().Equals(string.Empty))
                Session["ValidaPed"] = "no";
            else
                Session["ValidaPed"] = "si";


            if (Session["Pedimentos"] != null)
            {
                DataView dv = new DataView(((DataTable)(Session["Pedimentos"])));

                if (!e.Filter.Trim().Equals(string.Empty))
                {
                    dv.RowFilter = "PEDIMENTOARMADO like '%" + e.Filter + "%'";   //dv.RowFilter = "PEDIMENTOARMADO like '%240-3071-7003189%'";
                    dt = dv.ToTable();
                    comboBox.DataSource = dt == null ? new DataTable() : dt;
                }
                else
                {
                    dt = dv.ToTable();
                    DataTable dtn = new DataTable();
                    dtn.Columns.Add("PEDIMENTOARMADO", typeof(string));
                    DataRow row;
                    int tope = dt.Rows.Count - 1;

                    for (int i = e.BeginIndex; i < tope; i++)
                    {
                        //if ((i < (10) && (10) < tope) || (i <= tope && (10) > tope))
                        if (i < tope)
                        {
                            try
                            {
                                string pedi = dt.Rows[i]["PEDIMENTOARMADO"].ToString();
                                row = dtn.NewRow();
                                row["PEDIMENTOARMADO"] = pedi;
                                dtn.Rows.Add(row);
                            }
                            catch {
                                break;
                            }
                        }
                        else
                            break;
                    }
                    comboBox.DataSource = dtn == null ? new DataTable() : dtn;
                }
            }
            else
                comboBox.DataSource = dt == null ? new DataTable() : dt;

            comboBox.DataBind();

            //cmbPedimento.DataSource = dt == null ? new DataTable() : dt;
            //cmbPedimento.DataBind();
        }

        //Se usa para validar la cantidad de datos en el combo de pedimentos
        protected void ASPxComboBox_OnItemRequestedByValue_SQL(object source, ListEditItemRequestedByValueEventArgs e)
        {
            DataTable dt = new DataTable();
            ASPxComboBox comboBox = (ASPxComboBox)source;

            if (e.Text.Trim().Contains("Pedimento") || e.Text.Trim().Equals(string.Empty))
                Session["ValidaPed"] = "no";
            else
                Session["ValidaPed"] = "si";

            if (Session["Pedimentos"] != null)
            {
                DataView dv = new DataView(((DataTable)(Session["Pedimentos"])));

                if (!e.Text.Trim().Contains("Pedimento") && !e.Text.Trim().Equals(string.Empty))
                {
                    dv.RowFilter = "PEDIMENTOARMADO like '%" + e.Text + "%'"; //dv.RowFilter = "PEDIMENTOARMADO like '%240-3071-7003189%'";
                    dt = dv.ToTable();
                    comboBox.DataSource = dt == null ? new DataTable() : dt;

                }
                else if (e.Text.Trim().Contains("Pedimento") || e.Text.Trim().Equals(string.Empty))
                {
                    dt = dv.ToTable();
                    DataTable dtn = new DataTable();
                    dtn.Columns.Add("PEDIMENTOARMADO", typeof(string));
                    DataRow row;
                    int tope = dt.Rows.Count - 1;

                    for (int i = 0; i < 10; i++)
                    {
                        if ((i < 10 && 10 < tope) || (i <= tope && 10 > tope))
                        {
                            row = dtn.NewRow();
                            row["PEDIMENTOARMADO"] = dt.Rows[i]["PEDIMENTOARMADO"].ToString();
                            dtn.Rows.Add(row);
                        }
                        else
                            break;
                    }
                    comboBox.DataSource = dtn == null ? new DataTable() : dtn;
                }
            }
            else
                comboBox.DataSource = dt == null ? new DataTable() : dt;

            comboBox.DataBind();
        }

        //Agrega Pedimentos
        protected void btnAgregarPed_Click(object sender, EventArgs e)
        {
            try
            {
                ModalPedimentos();

                //Validar que seleccione un pedimento
                string valida = string.Empty;
                if (cmbPedimento.Text.Trim().Equals(string.Empty))
                {
                    valida = "Para poder guardar seleccione un pedimento";
                }
                else
                {
                    //Valida que no se repita un pedimento siempre y cuando existan registros en el grid
                    if (Session["GridPed"]!= null && ((DataTable)(Session["GridPed"])).Rows.Count > 0)
                    {
                        foreach (DataRow fila in ((DataTable)(Session["GridPed"])).Rows)
                        {
                            if (fila["PEDIMENTO"].ToString().Trim().ToUpper() == cmbPedimento.Text.Trim().ToUpper())
                            {
                                valida = "El pedimento ya existe: " + cmbPedimento.Text.Trim();
                                break;
                            }
                        }
                    }
                }

                if (valida.Length > 0)
                {
                    AlertError(valida);
                    return;
                }


                //Guardar
                string mensaje = "";
                DataTable dt = new DataTable();
                Int64 partida = 0;
                if (txtPartida.Text.Trim().Length > 0)
                    partida = Int64.Parse(txtPartida.Text.Trim());

                dt = catalogo.GuardarPedimento(Int64.Parse(Session["ACTIVOFIJOKEY"].ToString()), cmbPedimento.Text.Trim(), partida, int.Parse(Session["IdUsuario"].ToString()), lblCadena.Text, ref mensaje);

                if (dt != null && dt.Rows.Count > 0)
                {
                    GridPed.DataSource = Session["GridPed"] = dt;
                    GridPed.DataBind();

                    //Selecccionar el primer registro del grid
                    if (Session["GridPed"] != null)
                        GridPed.Selection.SelectRow(0);
                }
                else
                {
                    GridPed.DataSource = Session["GridPed"];
                    GridPed.DataBind();
                }

                GridPed.Settings.VerticalScrollableHeight = 180;
                GridPed.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;

            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "ControlActivoFijo-btnGuardar_Click", ex, lblCadena.Text, ref mensaje);
            }
        }

        //Editar Pedimentos
        protected void lkb_EditarPed_Click(object sender, EventArgs e)
        {
            string mensaje = string.Empty;
            DataTable dt = new DataTable();
            int valida_select = 0;
            ModalPedimentos();

            for (int i = 0; i < GridPed.VisibleRowCount; i++)
            {
                if (GridPed.Selection.IsRowSelected(i))
                {
                    //Abre Modal                    
                    MostrarModalPartida();

                    //Titulo del Modal
                    ModalTituloPartida.InnerText = "Editar Pedimento";
                    DataBind();

                    //Asignar valor al campo txt
                    Session["RELACIONPEDIMENTOKEY"] = GridPed.GetSelectedFieldValues("RELACIONPEDIMENTOKEY")[0].ToString().Trim();
                    Session["ACTIVOFIJOKEY"] = GridPed.GetSelectedFieldValues("ACTIVOFIJOKEY")[0].ToString().Trim();
                    txt_PartidaEditar.Text = GridPed.GetSelectedFieldValues("PARTIDA")[0].ToString().Trim();

                    valida_select = 1;
                    break;
                }
            }

            if (valida_select == 0)
                AlertError("Debe seleccionar un pedimento para poder editar");

            //Actualiza los permisos de los botones en grid
            PermisosUsuario();
        }

        protected void btnGuardarPed_Click(object sender, EventArgs e)
        {
            try
            {
                //Actualiza los permisos de los botones en grid
                PermisosUsuario();
                string valida = string.Empty;
                ModalPedimentos();

                //Valida si no se repite la partida
                if (Session["GridPed"] != null && ((DataTable)(Session["GridPed"])).Rows.Count > 0)
                {
                    foreach (DataRow fila in ((DataTable)(Session["GridPed"])).Rows)
                    {
                        if (fila["PARTIDA"].ToString().Trim().Length > 0 && (fila["PARTIDA"].ToString().Trim().ToUpper() == txt_PartidaEditar.Text.Trim().ToUpper()))
                        {
                            valida = "La partida existe: " + txt_PartidaEditar.Text.Trim();
                            break;
                        }                        
                    }
                }


                if (valida.Length > 0)
                {
                    //Mantiene el modal
                    MostrarModalPartida();

                    AlertError(valida);
                    return;
                }

                Int64 partida = 0;
                if(txt_PartidaEditar.Text.Trim().Length > 0)
                    partida = Int64.Parse(txt_PartidaEditar.Text.Trim());

                //Guardar
                string mensaje = "";
                DataTable dt = new DataTable();
                dt = catalogo.EditarPedimento(Int64.Parse(Session["RELACIONPEDIMENTOKEY"].ToString()), Int64.Parse(Session["ACTIVOFIJOKEY"].ToString()),
                                              partida, int.Parse(Session["IdUsuario"].ToString()), lblCadena.Text, ref mensaje);


                if (dt != null && dt.Rows.Count > 0)
                {

                    GridPed.DataSource = Session["GridPed"] = dt;
                    GridPed.DataBind();

                    //Selecccionar el primer registro del grid
                    if (Session["GridPed"] != null)
                        GridPed.Selection.SelectRow(0);
                }
                else
                {
                    GridPed.DataSource = Session["GridPed"];
                    GridPed.DataBind();
                }

                GridPed.Settings.VerticalScrollableHeight = 180;
                GridPed.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;

                //Actualiza los permisos de los botones en grid
                PermisosUsuario();
            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "ControlActivoFijo-btnGuardarPed_Click", ex, lblCadena.Text, ref mensaje);
            }

        }

        //Elimina Pedimentos
        protected void lkb_EliminarPed_Click(object sender, EventArgs e)
        {
            ModalPedimentos();

            int valida_select = 0;

            for (int i = 0; i < GridPed.VisibleRowCount; i++)
            {
                if (GridPed.Selection.IsRowSelected(i))
                {
                    valida_select = 1;
                    string mensaje = string.Empty;
                    DataTable dt = new DataTable();
                    Session["RELACIONPEDIMENTOKEY"] = GridPed.GetSelectedFieldValues("RELACIONPEDIMENTOKEY")[0].ToString().Trim();
                    Session["ACTIVOFIJOKEY"] = GridPed.GetSelectedFieldValues("ACTIVOFIJOKEY")[0].ToString().Trim();

                    dt = catalogo.EliminarPedimento(Int64.Parse(Session["RELACIONPEDIMENTOKEY"].ToString()), Int64.Parse(Session["ACTIVOFIJOKEY"].ToString()), int.Parse(Session["IdUsuario"].ToString()), lblCadena.Text, ref mensaje);

                    GridPed.DataSource = Session["GridPed"] = dt;
                    GridPed.DataBind();
                    GridPed.Settings.VerticalScrollBarMode = ScrollBarMode.Hidden;
                    GridPed.SettingsPager.PageSize = 20;

                    //Selecccionar el primer registro del grid
                    if (Session["GridPed"] != null)
                        GridPed.Selection.SelectRow(0);

                    AlertSuccess("El pedimento se eliminó con éxito.");

                    GridPed.Settings.VerticalScrollableHeight = 180;
                    GridPed.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                }
            }

            if (valida_select == 0)
                AlertError("Debe seleccionar un pedimento para poder eliminar");

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

    }
}