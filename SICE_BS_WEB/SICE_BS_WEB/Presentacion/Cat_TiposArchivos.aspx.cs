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
    public partial class Cat_TiposArchivos : System.Web.UI.Page
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

                    Session["Grid"] = null;
                    TituloPanel(string.Empty);

                    string mensaje = "";
                    DataTable dtc = new DataTable();

                    dtc = catalogo.TraerTiposArchivo(lblCadena.Text, ref mensaje);
                    if (dtc != null && dtc.Rows.Count > 0)
                    {
                        Grid.DataSource = Session["Grid"] = dtc;
                        Grid.DataBind();
                        Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Hidden;
                        Grid.SettingsPager.PageSize = 20;

                        //Selecccionar el primer registro del grid
                        if (Session["Grid"] != null)
                            Grid.Selection.SelectRow(0);
                    }
                    else
                    {
                        Grid.DataSource = Session["Grid"] = dtc;
                        Grid.DataBind();
                        AlertError("No hay información o intentelo de nuevo");
                    }

                    Grid.Settings.VerticalScrollableHeight = 330;
                    Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;

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

        //Propiedad GridPageSizeCP
        protected int GridPageSizeCP
        {
            get
            {
                if (Session[PageSizeSessionKey] == null)
                    return GridCP.SettingsPager.PageSize;
                return (int)Session[PageSizeSessionKey];
            }
            set { Session[PageSizeSessionKey] = value; }
        }

        //Propiedad GridPageSizeIG
        protected int GridPageSizeIG
        {
            get
            {
                if (Session[PageSizeSessionKey] == null)
                    return GridIG.SettingsPager.PageSize;
                return (int)Session[PageSizeSessionKey];
            }
            set { Session[PageSizeSessionKey] = value; }
        }

        //Propiedad GridPageSizeIP
        protected int GridPageSizeIP
        {
            get
            {
                if (Session[PageSizeSessionKey] == null)
                    return GridIP.SettingsPager.PageSize;
                return (int)Session[PageSizeSessionKey];
            }
            set { Session[PageSizeSessionKey] = value; }
        }

        //Propiedad GridPageSizeIN
        protected int GridPageSizeIN
        {
            get
            {
                if (Session[PageSizeSessionKey] == null)
                    return GridIN.SettingsPager.PageSize;
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


            //Cuando se quiera filtrar el GridCP entra en el if
            if (Session["GridCP"] != null)
            {
                GridCP.DataSource = Session["GridCP"];
                GridCP.DataBind();
                GridCP.SettingsPager.PageSize = GridPageSizeCP;
            }

            //Cuando se quiera filtrar el GridIG entra en el if
            if (Session["GridIG"] != null)
            {
                GridIG.DataSource = Session["GridIG"];
                GridIG.DataBind();
                GridIG.SettingsPager.PageSize = GridPageSizeIG;
            }

            //Cuando se quiera filtrar el GridIP entra en el if
            if (Session["GridIP"] != null)
            {
                GridIP.DataSource = Session["GridIP"];
                GridIP.DataBind();
                GridIP.SettingsPager.PageSize = GridPageSizeIP;
            }

            //Cuando se quiera filtrar el GridIN entra en el if
            if (Session["GridIN"] != null)
            {
                GridIN.DataSource = Session["GridIN"];
                GridIN.DataBind();
                GridIN.SettingsPager.PageSize = GridPageSizeIN;
            }

        }

        //Metodo que llama al Callback para actualizar el PageSize y el Grid
        protected void Grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            GridPageSize = int.Parse(e.Parameters);
            Grid.SettingsPager.PageSize = GridPageSize;
            Grid.DataBind();
        }

        //Metodo que llama al Callback para actualizar el PageSizeCP y el GridCP
        protected void GridCP_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            GridPageSizeCP = int.Parse(e.Parameters);
            GridCP.SettingsPager.PageSize = GridPageSizeCP;
            GridCP.DataBind();
        }

        //Metodo que llama al Callback para actualizar el PageSizeIG y el GridIG
        protected void GridIG_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            GridPageSizeIG = int.Parse(e.Parameters);
            GridIG.SettingsPager.PageSize = GridPageSizeIG;
            GridIG.DataBind();
        }

        //Metodo que llama al Callback para actualizar el PageSizeIP y el GridIP
        protected void GridIP_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            GridPageSizeIP = int.Parse(e.Parameters);
            GridIP.SettingsPager.PageSize = GridPageSizeIP;
            GridIP.DataBind();
        }

        //Metodo que llama al Callback para actualizar el PageSizeIN y el GridIN
        protected void GridIN_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            GridPageSizeIN = int.Parse(e.Parameters);
            GridIN.SettingsPager.PageSize = GridPageSizeIN;
            GridIN.DataBind();
        }

        //Metodo que llama al combo box al seleccionar la cantidad de registros en el page
        protected void PagerCombo_Load(object sender, EventArgs e)
        {
            (sender as ASPxComboBox).Value = Grid.SettingsPager.PageSize;
        }

        protected void Grid_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        {
            string commandName = e.CommandArgs.CommandName;
           
            if (commandName.Equals("Nuevo"))
            {

            }
        }

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
            if (e.Index == 0) e.SheetName = "DatosGenerales";
            if (e.Index == 1) e.SheetName = "DatosGenerales";
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
                        Exporter.WritePdfToResponse("Catálogo Tipos de Archivo");
                        break;
                    case "ExportToXLS":
                        Exporter.WriteXlsToResponse("Catálogo Tipos de Archivo", new XlsExportOptionsEx() { SheetName = "Catálogo Tipos de Archivo" });
                        break;
                    case "ExportToXLSX":
                        Exporter.WriteXlsxToResponse("Catálogo Tipos de Archivo", new XlsxExportOptionsEx() { SheetName = "Catálogo Tipos de Archivo" });
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

        //Metodo que muestra ventana de satisfactorio
        public void AlertSuccess(string mensaje)
        {
            pModalSucces.InnerText = mensaje;
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "", "<script> document.getElementById('btnSuccess').setAttribute('data-whatever', '" + mensaje + "'); document.getElementById('btnSuccess').click(); </script> ", false);
        }

        //Metodo que muestra ventana de Question
        public void AlertQuestion1(string mensaje)
        {
            pModalQuestion1.InnerText = mensaje;
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "", "<script> document.getElementById('btnQuestion1').setAttribute('data-whatever', '" + mensaje + "'); document.getElementById('btnQuestion1').click(); </script> ", false);
        }

        //Metodo que muestra ventana de Question
        public void AlertQuestion(string mensaje)
        {
            pModalQuestion.InnerText = mensaje;
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "", "<script> document.getElementById('btnQuestion').setAttribute('data-whatever', '" + mensaje + "'); document.getElementById('btnQuestion').click(); </script> ", false);
        }

        //Metodo que muestra ventana de QuestionIG
        public void AlertQuestionIG(string mensaje)
        {
            pModalQuestionIG.InnerText = mensaje;
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "", "<script> document.getElementById('btnQuestionIG').setAttribute('data-whatever', '" + mensaje + "'); document.getElementById('btnQuestionIG').click(); </script> ", false);
        }

        //Metodo que muestra ventana de QuestionIP
        public void AlertQuestionIP(string mensaje)
        {
            pModalQuestionIP.InnerText = mensaje;
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "", "<script> document.getElementById('btnQuestionIP').setAttribute('data-whatever', '" + mensaje + "'); document.getElementById('btnQuestionIP').click(); </script> ", false);
        }

        //Metodo que muestra ventana de QuestionIN
        public void AlertQuestionIN(string mensaje)
        {
            pModalQuestionIN.InnerText = mensaje;
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "", "<script> document.getElementById('btnQuestionIN').setAttribute('data-whatever', '" + mensaje + "'); document.getElementById('btnQuestionIN').click(); </script> ", false);
        }

        //Metodo que muestra el modal
        private void MostrarModal()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "MostrarModal", "<script> document.getElementById('btnModal').click(); </script> ", false);
        }

        //Metodo que muestra el modal Clave Pedimento
        private void MostrarModalCP()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "MostrarModalCP", "<script> document.getElementById('btnModalCP').click(); </script> ", false);
        }

        //Metodo que muestra el modal Clave Pedimento CP2
        private void MostrarModalCP2()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "MostrarModalCP2", "<script> document.getElementById('btnModalCP2').click(); </script> ", false);
        }

        //Metodo que muestra el modal IG
        private void MostrarModalIG()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "MostrarModalIG", "<script> document.getElementById('btnModalIG').click(); </script> ", false);
        }

        //Metodo que muestra el modal Clave Pedimento IG2
        private void MostrarModalIG2()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "MostrarModalIG2", "<script> document.getElementById('btnModalIG2').click(); </script> ", false);
        }

        //Metodo que muestra el modal IP
        private void MostrarModalIP()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "MostrarModalIP", "<script> document.getElementById('btnModalIP').click(); </script> ", false);
        }

        //Metodo que muestra el modal Clave Pedimento IP2
        private void MostrarModalIP2()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "MostrarModalIP2", "<script> document.getElementById('btnModalIP2').click(); </script> ", false);
        }

        //Metodo que muestra el modal IN
        private void MostrarModalIN()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "MostrarModalIN", "<script> document.getElementById('btnModalIN').click(); </script> ", false);
        }

        //Metodo que muestra el modal IN2
        private void MostrarModalIN2()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(String), "MostrarModalIN2", "<script> document.getElementById('btnModalIN2').click(); </script> ", false);
        }

        protected void lkb_Nuevo_Click(object sender, EventArgs e)
        {
            //Abre Modal
            MostrarModal();

            //Titulo del Modal
            ModalTitulo.InnerText = "Nuevo Tipo de Archivo";
            DataBind();

            //Limpiar Campo
            txt_tipoArchivo.Text = string.Empty;
            txt_sufijo.Text = string.Empty;
            cmbTipoOperacion.Text = string.Empty;
            cmbObligatorio.Text = string.Empty;
            cmbObligatorioC.Text = string.Empty;
            txt_ClavePedimento.Text = string.Empty;
            txt_IG.Text = string.Empty;
            txt_IP.Text = string.Empty;
            txt_Incoterm.Text = string.Empty;
            lblTP.Text = "0";

            //Actualiza los permisos de los botones en grid
            PermisosUsuario();

            //Limpiar sesiones para edición o eliminación en cp, ig, ip e in
            //Session["TCPKEY"] = null;
            Session["CLAVEPEDIMENTO"] = null;
            Session["IDENTIFICADOR"] = null;
            Session["IDENTIFICADORP"] = null;
            Session["INCOTERM"] = null;

            Session["GridCP"] = null;
            Session["GridIG"] = null;
            Session["GridIP"] = null;
            Session["GridIN"] = null;

            Session["Mantener_Info_CP"] = null;
            Session["Mantener_Info_IG"] = null;
            Session["Mantener_Info_IP"] = null;
            Session["Mantener_Info_IN"] = null;

            HfCPBorrar.Value = string.Empty;
            HfIGBorrar.Value = string.Empty;
            HfIPBorrar.Value = string.Empty;
            HfINBorrar.Value = string.Empty;

            lkb_ClavePedimento.Enabled = true;
            lkb_IdentificadorGeneral.Enabled = true;
            lkb_IdentificadorPartida.Enabled = true;
            lkb_INCOTERM.Enabled = true;
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
                    ModalTitulo.InnerText = "Editar Tipo de Archivo";
                    DataBind();

                    //Asignar valor al campo txt
                    Session["TIPOARCHIVOKEY"] = lblTP.Text = Grid.GetSelectedFieldValues("TIPOARCHIVOKEY")[0].ToString().Trim();
                    txt_tipoArchivo.Text = Grid.GetSelectedFieldValues("TIPOS DE ARCHIVO")[0].ToString().Trim();
                    txt_sufijo.Text = Grid.GetSelectedFieldValues("SUFIJO")[0].ToString().Trim();

                    if (Grid.GetSelectedFieldValues("TIPO DE OPERACION")[0].ToString().Trim().ToUpper().Contains("AMBAS"))
                        cmbTipoOperacion.SelectedIndex = 0;
                    else if (Grid.GetSelectedFieldValues("TIPO DE OPERACION")[0].ToString().Trim().ToUpper().Contains("EXPORT"))
                        cmbTipoOperacion.SelectedIndex = 1;
                    else if (Grid.GetSelectedFieldValues("TIPO DE OPERACION")[0].ToString().Trim().ToUpper().Contains("IMPORT"))
                        cmbTipoOperacion.SelectedIndex = 2;
                    else
                        cmbTipoOperacion.Text = "";
                    
                    if (Grid.GetSelectedFieldValues("OBLIGATORIO")[0].ToString().Trim().ToUpper().Contains("SI"))
                        cmbObligatorio.SelectedIndex = 0;
                    else if (Grid.GetSelectedFieldValues("OBLIGATORIO")[0].ToString().Trim().ToUpper().Contains("NO"))
                        cmbObligatorio.SelectedIndex = 1;
                    else
                        cmbObligatorio.Text = "";

                    if (Grid.GetSelectedFieldValues("OBLIGATORIOC")[0].ToString().Trim().ToUpper().Contains("SI"))
                        cmbObligatorioC.SelectedIndex = 0;
                    else if (Grid.GetSelectedFieldValues("OBLIGATORIOC")[0].ToString().Trim().ToUpper().Contains("NO"))
                        cmbObligatorioC.SelectedIndex = 1;
                    else
                        cmbObligatorioC.Text = "";

                    txt_ClavePedimento.Text = Grid.GetSelectedFieldValues("CLAVEPEDIMENTO")[0].ToString().Trim().ToUpper();
                    txt_IG.Text = Grid.GetSelectedFieldValues("IDENTIFICADOR")[0].ToString().Trim().ToUpper();
                    txt_IP.Text = Grid.GetSelectedFieldValues("IDENTIFICADORP")[0].ToString().Trim().ToUpper();
                    txt_Incoterm.Text = Grid.GetSelectedFieldValues("INCOTERM")[0].ToString().Trim().ToUpper();


                    //Si hay datos en clave pedimento se bloquean los botones IG,IP e IN
                    if (txt_ClavePedimento.Text.Trim().Length > 0)
                    {
                        lkb_ClavePedimento.Enabled = true;
                        lkb_IdentificadorGeneral.Enabled = false;
                        lkb_IdentificadorPartida.Enabled = false;
                        lkb_INCOTERM.Enabled = false;
                    }
                    else
                    {
                        lkb_IdentificadorGeneral.Enabled = true;
                        lkb_IdentificadorPartida.Enabled = true;
                        lkb_INCOTERM.Enabled = true;
                    }

                    if (txt_ClavePedimento.Text.Trim().Length.Equals(0) && (txt_IG.Text.Trim().Length > 0 || txt_IP.Text.Trim().Length > 0 || txt_Incoterm.Text.Trim().Length > 0))
                    {
                        lkb_ClavePedimento.Enabled = false;
                    }
                    else if (txt_ClavePedimento.Text.Trim().Length.Equals(0) && txt_IG.Text.Trim().Length.Equals(0) && txt_IP.Text.Trim().Length.Equals(0) && txt_Incoterm.Text.Trim().Length.Equals(0))
                    {
                        lkb_ClavePedimento.Enabled = true;
                        lkb_IdentificadorGeneral.Enabled = true;
                        lkb_IdentificadorPartida.Enabled = true;
                        lkb_INCOTERM.Enabled = true;
                    }
                    
                    Session["GridCP"] = null;
                    Session["GridIG"] = null;
                    Session["GridIP"] = null;
                    Session["GridIN"] = null;

                    Session["Mantener_Info_CP"] = null;
                    Session["Mantener_Info_IG"] = null;
                    Session["Mantener_Info_IP"] = null;
                    Session["Mantener_Info_IN"] = null;

                    HfCPBorrar.Value = string.Empty;
                    HfIGBorrar.Value = string.Empty;
                    HfIPBorrar.Value = string.Empty;
                    HfINBorrar.Value = string.Empty;
                    
                    valida_select = 1;
                    break;
                }
            }

            if(valida_select == 0)
                AlertError("Debe seleccionar un tipo de archivo para poder editar");

            //Actualiza los permisos de los botones en grid
            PermisosUsuario();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (Session["Grid"] == null)
            {
                string alerta = "<script>alert('La sesión ha expirado, debe volver a iniciar sesión');window.location.href='Login.aspx'</script>";
                Session["Tab"] = "Salir";
                Response.Write(alerta);
                return;
            }

            //Actualiza los permisos de los botones en grid
            PermisosUsuario();


            string valida = string.Empty;

            //Validar que el dato a guardar o modificar no exista 
            if (txt_tipoArchivo.Text.Trim() == string.Empty)
            {
                valida = "Para poder guardar escriba un tipo de archivo";
            }

            else if (txt_sufijo.Text.Trim() == string.Empty)
            {
                valida = "Para poder guardar escriba un sufijo";
            }

            else if (cmbTipoOperacion.Text.Trim() == string.Empty)
            {
                valida = "Para poder guardar seleccione un tipo de operación";
            }

            else if (cmbObligatorio.Text.Trim() == string.Empty)
            {
                valida = "Para poder guardar seleccione una opción obligatorio";
            }

            else if (Session["Grid"] != null && ((DataTable)(Session["Grid"])).Rows.Count > 0)
            {
                foreach (DataRow fila in ((DataTable)(Session["Grid"])).Rows)
                {
                    //Al Guardar
                    if (ModalTitulo.InnerText.Contains("Nuevo") && 
                        fila["TIPOS DE ARCHIVO"].ToString().ToUpper().Trim() == txt_tipoArchivo.Text.ToUpper().Trim())
                    {
                        valida = "Ya existe el tipo de archivo: " + txt_tipoArchivo.Text.Trim();
                        break;
                    }
                    else if (ModalTitulo.InnerText.Contains("Nuevo") && 
                        fila["SUFIJO"].ToString().ToUpper().Trim() == txt_sufijo.Text.ToUpper().Trim())
                    {
                        valida = "Ya existe el sufijo: " + txt_sufijo.Text.Trim();
                        break;
                    }

                    //Al Editar
                    if (ModalTitulo.InnerText.Contains("Editar") &&
                        fila["TIPOS DE ARCHIVO"].ToString().ToUpper().Trim() == txt_tipoArchivo.Text.ToUpper().Trim() &&
                        fila["TIPOARCHIVOKEY"].ToString().ToUpper().Trim() != Session["TIPOARCHIVOKEY"].ToString().Trim().ToUpper())
                    {
                        valida = "Ya existe el tipo de archivo: " + txt_tipoArchivo.Text.Trim();
                        break;
                    }
                    else if (ModalTitulo.InnerText.Contains("Editar") &&
                        fila["SUFIJO"].ToString().ToUpper().Trim() == txt_sufijo.Text.ToUpper().Trim() &&
                        fila["TIPOARCHIVOKEY"].ToString().ToUpper().Trim() != Session["TIPOARCHIVOKEY"].ToString().Trim().ToUpper())
                    {
                        valida = "Ya existe el sufijo: " + txt_sufijo.Text.Trim();
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
                    ModalTitulo.InnerText = "Nuevo Tipo de Archivo";
                else
                    ModalTitulo.InnerText = "Editar Tipo de Archivo";
                DataBind();

                AlertError(valida);
                return;
            }
            

            //Guardar o Editar en TIPOARCHIVO
            string mensaje = "";
            DataTable dt = new DataTable();

            //Valida si la session sig es null son nuevos registros, sino es editar  
            if (ModalTitulo.InnerText.Contains("Nuevo"))
            {
                dt = catalogo.GuardarTipoArchivo(txt_tipoArchivo.Text.ToUpper().Trim(), txt_sufijo.Text.ToUpper().Trim(), cmbTipoOperacion.Text.ToUpper().Trim(),
                                                 cmbObligatorio.Text.ToUpper().Trim(), cmbObligatorioC.Text.ToUpper().Trim(), int.Parse(Session["IdUsuario"].ToString()), lblCadena.Text, ref mensaje);

                //Obtener TPKEY de dt
                lblTP.Text = dt.Rows[0]["TPKEY"].ToString();
            }
            else
            {
                dt = catalogo.EditarTipoArchivo(Int64.Parse(Session["TIPOARCHIVOKEY"].ToString()), txt_tipoArchivo.Text.ToUpper().Trim(), txt_sufijo.Text.ToUpper().Trim(),
                                                cmbTipoOperacion.Text.ToUpper().Trim(), cmbObligatorio.Text.ToUpper().Trim(), cmbObligatorioC.Text.ToUpper().Trim(), 
                                                int.Parse(Session["IdUsuario"].ToString()), lblCadena.Text, ref mensaje);
            }




            //Guardar o Editar en CP, IG,IP e IN dependiendo de cada renglón
            if (Session["GridCP"] != null)
            {
                foreach (DataRow fila in ((DataTable)(Session["GridCP"])).Rows)
                {
                    //Edita
                    if (fila["ANTERIOR_CLAVEPEDIMENTO"].ToString().Length > 0 || fila["ANTERIOR_IDENTIFICADOR"].ToString().Length > 0 ||
                        fila["ANTERIOR_IDENTIFICADORP"].ToString().Length > 0 || fila["ANTERIOR_INCOTERM"].ToString().Length > 0)
                    {
                        dt = catalogo.ActualizaClavePedimento(Int64.Parse(lblTP.Text), fila["ANTERIOR_CLAVEPEDIMENTO"].ToString().ToUpper().Trim(), fila["CLAVEPEDIMENTO"].ToString().ToUpper().Trim(),
                             fila["ANTERIOR_IDENTIFICADOR"].ToString().ToUpper().Trim(), fila["IDENTIFICADOR"].ToString().ToUpper().Trim(), fila["IDENTIFICADORP"].ToString().ToUpper().Trim(),
                             fila["ANTERIOR_IDENTIFICADORP"].ToString().ToUpper().Trim(), fila["ANTERIOR_INCOTERM"].ToString().ToUpper().Trim(), fila["INCOTERM"].ToString().ToUpper().Trim(), lblCadena.Text, ref mensaje);
                    }
                    //Nuevo
                    else if (fila["ANTERIOR_CLAVEPEDIMENTO"].ToString().Length.Equals(0) && fila["ANTERIOR_IDENTIFICADOR"].ToString().Length.Equals(0) &&
                        fila["ANTERIOR_IDENTIFICADORP"].ToString().Length.Equals(0) && fila["ANTERIOR_INCOTERM"].ToString().Length.Equals(0))
                    {
                        dt = catalogo.GuardaClavePedimento(Int64.Parse(lblTP.Text), fila["CLAVEPEDIMENTO"].ToString().ToUpper().Trim(), fila["IDENTIFICADOR"].ToString().ToUpper().Trim(),
                             fila["IDENTIFICADORP"].ToString().ToUpper().Trim(), fila["INCOTERM"].ToString().ToUpper().Trim(), lblCadena.Text, ref mensaje);
                    }
                }
            }
            else if (Session["GridCP"] == null && ModalTitulo.InnerText.Contains("Nuevo"))
            {
                mensaje = "";
                dt = new DataTable();
                dt = catalogo.TraerTiposArchivo(lblCadena.Text, ref mensaje);
            }


            //Borra CP si existe datos en variable HfCPBorrar
            if (HfCPBorrar.Value.Length > 0)
            {
                mensaje = string.Empty;
                dt = new DataTable();
                HfCPBorrar.Value = HfCPBorrar.Value.Substring(1, HfCPBorrar.Value.Length - 1);
                dt = catalogo.EliminaClavePedimento(Int64.Parse(lblTP.Text), HfCPBorrar.Value, lblCadena.Text, ref mensaje);

                HfCPBorrar.Value = string.Empty;
            }



            //Guardar o Editar IG
            if (Session["GridIG"] != null)
            {
                foreach (DataRow fila in ((DataTable)(Session["GridIG"])).Rows)
                {
                    //Edita
                    if (fila["ANTERIOR_IDENTIFICADOR"].ToString().Length > 0)
                    {
                        dt = catalogo.ActualizaIG(Int64.Parse(lblTP.Text), fila["ANTERIOR_IDENTIFICADOR"].ToString().ToUpper().Trim(), fila["IDENTIFICADOR"].ToString().ToUpper().Trim(), lblCadena.Text, ref mensaje);
                    }
                    //Nuevo
                    else if (fila["ANTERIOR_IDENTIFICADOR"].ToString().Length.Equals(0))
                    {
                        dt = catalogo.GuardaIG(Int64.Parse(lblTP.Text), fila["IDENTIFICADOR"].ToString().ToUpper().Trim(), lblCadena.Text, ref mensaje);
                    }
                }
            }

            //Borra IG si existe datos en variable HfIGBorrar
            if (HfIGBorrar.Value.Length > 0)
            {
                mensaje = string.Empty;
                dt = new DataTable();
                HfIGBorrar.Value = HfIGBorrar.Value.Substring(1, HfIGBorrar.Value.Length - 1);
                dt = catalogo.EliminaIG(Int64.Parse(lblTP.Text), HfIGBorrar.Value, lblCadena.Text, ref mensaje);

                HfIGBorrar.Value = string.Empty;
            }



            //Guardar o Editar IP
            if (Session["GridIP"] != null)
            {
                foreach (DataRow fila in ((DataTable)(Session["GridIP"])).Rows)
                {
                    //Edita
                    if (fila["ANTERIOR_IDENTIFICADORP"].ToString().Length > 0)
                    {
                        dt = catalogo.ActualizaIP(Int64.Parse(lblTP.Text), fila["ANTERIOR_IDENTIFICADORP"].ToString().ToUpper().Trim(), fila["IDENTIFICADORP"].ToString().ToUpper().Trim(), lblCadena.Text, ref mensaje);
                    }
                    //Nuevo
                    else if (fila["ANTERIOR_IDENTIFICADORP"].ToString().Length.Equals(0))
                    {
                        dt = catalogo.GuardaIP(Int64.Parse(lblTP.Text), fila["IDENTIFICADORP"].ToString().ToUpper().Trim(), lblCadena.Text, ref mensaje);
                    }
                }
            }

            //Borra IP si existe datos en variable HfIPBorrar
            if (HfIPBorrar.Value.Length > 0)
            {
                mensaje = string.Empty;
                dt = new DataTable();
                HfIPBorrar.Value = HfIPBorrar.Value.Substring(1, HfIPBorrar.Value.Length - 1);
                dt = catalogo.EliminaIP(Int64.Parse(lblTP.Text), HfIPBorrar.Value, lblCadena.Text, ref mensaje);

                HfIPBorrar.Value = string.Empty;
            }

            //Guardar o Editar IN
            if (Session["GridIN"] != null)
            {
                foreach (DataRow fila in ((DataTable)(Session["GridIN"])).Rows)
                {
                    //Edita
                    if (fila["ANTERIOR_INCOTERM"].ToString().Length > 0)
                    {
                        dt = catalogo.ActualizaIN(Int64.Parse(lblTP.Text), fila["ANTERIOR_INCOTERM"].ToString().ToUpper().Trim(), fila["INCOTERM"].ToString().ToUpper().Trim(), lblCadena.Text, ref mensaje);
                    }
                    //Nuevo
                    else if (fila["ANTERIOR_INCOTERM"].ToString().Length.Equals(0))
                    {
                        dt = catalogo.GuardaIN(Int64.Parse(lblTP.Text), fila["INCOTERM"].ToString().ToUpper().Trim(), lblCadena.Text, ref mensaje);
                    }
                }
            }

            //Borra IN si existe datos en variable HfINBorrar
            if (HfINBorrar.Value.Length > 0)
            {
                mensaje = string.Empty;
                dt = new DataTable();
                HfINBorrar.Value = HfINBorrar.Value.Substring(1, HfINBorrar.Value.Length - 1);
                dt = catalogo.EliminaIncoterm(Int64.Parse(lblTP.Text), HfINBorrar.Value, lblCadena.Text, ref mensaje);

                HfINBorrar.Value = string.Empty;
            }



            if (dt != null && dt.Rows.Count > 0)
            {
                Grid.DataSource = Session["Grid"] = dt;
                Grid.DataBind();
                Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Hidden;
                Grid.SettingsPager.PageSize = 20;

                //Selecccionar el primer registro del grid
                if (Session["Grid"] != null)
                    Grid.Selection.SelectRow(0);

                AlertSuccess("El Tipo de Archivo se " + (ModalTitulo.InnerText.Contains("Edit") ? "actualizó" : "agregó") + ".");
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

            //Limpiar sesiones para edición o eliminación
            //Session["TCPKEY"] = null;
            Session["CLAVEPEDIMENTO"] = null;
            Session["IDENTIFICADOR"] = null;
            Session["IDENTIFICADORP"] = null;
            Session["INCOTERM"] = null;
            Session["Mantener_Info_CP"] = null;
            Session["Mantener_Info_IG"] = null;
            Session["Mantener_Info_IP"] = null;
            Session["Mantener_Info_IN"] = null;
            HfCPBorrar.Value = string.Empty;


            if (txt_ClavePedimento.Text.Trim().Length > 0)
            {
                lkb_IdentificadorGeneral.Enabled = false;
                lkb_IdentificadorPartida.Enabled = false;
                lkb_INCOTERM.Enabled = false;
            }
            else
            {
                lkb_IdentificadorGeneral.Enabled = true;
                lkb_IdentificadorPartida.Enabled = true;
                lkb_INCOTERM.Enabled = true;
            }
            

            if(txt_ClavePedimento.Text.Trim().Length.Equals(0) && (txt_IG.Text.Trim().Length > 0 || txt_IP.Text.Trim().Length > 0 || txt_Incoterm.Text.Trim().Length > 0))
            {
                lkb_ClavePedimento.Enabled = false;
            }
            else if (txt_ClavePedimento.Text.Trim().Length.Equals(0) && txt_IG.Text.Trim().Length.Equals(0) && txt_IP.Text.Trim().Length.Equals(0) && txt_Incoterm.Text.Trim().Length.Equals(0))
            {
                lkb_ClavePedimento.Enabled = true;
                lkb_IdentificadorGeneral.Enabled = true;
                lkb_IdentificadorPartida.Enabled = true;
                lkb_INCOTERM.Enabled = true;
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
                    Session["TIPOARCHIVOKEY"] = Grid.GetSelectedFieldValues("TIPOARCHIVOKEY")[0].ToString().Trim();

                    //Modal Question
                    string valida = "¿Desea eliminar este registro?";
                    AlertQuestion1(valida);
                    valida_select = 1;
                    break;
                }
            }

            if (valida_select == 0)
                AlertError("Debe seleccionar un Tipo de Archivo para poder eliminar");

            //Actualiza los permisos de los botones en grid
            PermisosUsuario();
        }

        protected void btnOk1_Click(object sender, EventArgs e)
        {

            string mensaje = string.Empty;
            DataTable dt = new DataTable();
            Int64 id = Int64.Parse(Session["TIPOARCHIVOKEY"].ToString().Trim());
            dt = catalogo.EliminarTipoArchivo(id, int.Parse(Session["IdUsuario"].ToString()), lblCadena.Text, ref mensaje);

            Grid.DataSource = Session["Grid"] = dt;
            Grid.DataBind();
            Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Hidden;
            Grid.SettingsPager.PageSize = 20;

            //Selecccionar el primer registro del grid
            if (Session["Grid"] != null)
                Grid.Selection.SelectRow(0);

            //AlertSuccess("El Tipo de archivo se eliminó con éxito.");

            Grid.Settings.VerticalScrollableHeight = 330;
            Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;

            //Actualiza los permisos de los botones en grid
            PermisosUsuario();
        }

        protected void lkb_Excel_Click(object sender, EventArgs e)
        {
            if (Grid.VisibleRowCount > 0)
            {
                Exporter.WriteXlsToResponse("Catálogo Tipos de Archivo", new XlsExportOptionsEx() { SheetName = "Catálogo Tipos de Archivo" });
            }
            else
                AlertError("No hay información por exportar");

            //Actualiza los permisos de los botones en grid
            PermisosUsuario();
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




        #region CLAVE PEDIMENTO
        
        protected void lkb_ClavePedimento_Click(object sender, EventArgs e)
        {
            MostrarModal();
            MostrarModalCP();
            ModalTituloCP.InnerText = "Clave Pedimento";

            string mensaje = "";
            DataTable dt = new DataTable();

            //Carga gridCP
            if (txt_ClavePedimento.Text.Trim().Length > 0 || txt_IG.Text.Trim().Length > 0 || txt_IP.Text.Trim().Length > 0 || txt_Incoterm.Text.Trim().Length > 0)
            {
                if (Session["Mantener_Info_CP"] == null)
                {
                    dt = catalogo.TraeClavesPedimentos(Int64.Parse(lblTP.Text.Trim()), lblCadena.Text, ref mensaje);
                    Session["GridCP"] = dt;
                }
            }
            else
                Session["GridCP"] = null;

            //Se limpia gridcp
            DataTable dtL = new DataTable();
            GridCP.DataSource = dtL;
            GridCP.DataBind();

            GridCP.DataSourceID = null;
            GridCP.DataBind();

            GridCP.DataSource = Session["GridCP"];
            GridCP.DataBind();
            GridCP.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
            GridCP.SettingsPager.PageSize = 20;            
            GridCP.Settings.VerticalScrollableHeight = 210;

            if (Session["GridCP"] != null)
                GridCP.Selection.UnselectAll();
        }

        protected void btnGuardarCP_Click(object sender, EventArgs e)
        {
            if (Session["GridCP"] == null)
            {
                MostrarModal();
                MostrarModalCP();
                AlertError("Debe agregar un nuevo registro para poderlo guardar");                
                return;
            }

            MostrarModal();

            //colocar valores del gridCP en las cajas de texto del primer modal
            string v_cp = string.Empty;
            string v_ig = string.Empty;
            string v_ip = string.Empty;
            string v_in = string.Empty;
            
            if (Session["GridCP"] != null)
            {
                foreach (DataRow fila in ((DataTable)(Session["GridCP"])).Rows)
                {
                    if (fila["CLAVEPEDIMENTO"].ToString().Trim().Length > 0)
                        v_cp += fila["CLAVEPEDIMENTO"].ToString().ToUpper().Trim() + ",";
                    if (fila["IDENTIFICADOR"].ToString().Trim().Length > 0)
                        v_ig += fila["IDENTIFICADOR"].ToString().ToUpper().Trim() + ",";
                    if (fila["IDENTIFICADORP"].ToString().Trim().Length > 0)
                    v_ip += fila["IDENTIFICADORP"].ToString().ToUpper().Trim() + ",";
                    if (fila["INCOTERM"].ToString().Trim().Length > 0)
                        v_in += fila["INCOTERM"].ToString().ToUpper().Trim() + ",";
                }
            }

            if (v_cp.Length > 0)
                v_cp = v_cp.Substring(0, v_cp.Length - 1);
            if (v_ig.Length > 0)
                v_ig = v_ig.Substring(0, v_ig.Length - 1);
            if (v_ip.Length > 0)
                v_ip = v_ip.Substring(0, v_ip.Length - 1);
            if (v_in.Length > 0)
                v_in = v_in.Substring(0, v_in.Length - 1);



            txt_ClavePedimento.Text = v_cp;
            txt_IG.Text = v_ig;
            txt_IP.Text = v_ip;
            txt_Incoterm.Text = v_in;

            Session["Mantener_Info_CP"] = "Si";

            //Si hay datos en clave pedimento se bloquean los botones IG,IP e IN
            if (txt_ClavePedimento.Text.Trim().Length > 0)
            {
                lkb_IdentificadorGeneral.Enabled = false;
                lkb_IdentificadorPartida.Enabled = false;
                lkb_INCOTERM.Enabled = false;
            }
            else
            {
                lkb_IdentificadorGeneral.Enabled = true;
                lkb_IdentificadorPartida.Enabled = true;
                lkb_INCOTERM.Enabled = true;
            }

        }

        protected void lkb_NuevoCP_Click(object sender, EventArgs e)
        {
            MostrarModal();
            MostrarModalCP();
            ModalTituloCP.InnerText = "Clave Pedimento";
            MostrarModalCP2();
            ModalTituloCP2.InnerText = "Nueva Clave Pedimento";


            //Limpiar Campos de CP2
            txtCP2_ClavePedimento.Text = string.Empty;
            txtCP2_IG.Text = string.Empty;
            txtCP2_IP.Text = string.Empty;
            txtCP2_Incoterm.Text = string.Empty;

            txtCP2_ClavePedimento.Visible = true;
            txtCP2_IG.Visible = true;
            txtCP2_IP.Visible = true;
            txtCP2_Incoterm.Visible = true;

            lblT_CP.Visible = true;
            lblT_IG.Visible = true;
            lblT_IP.Visible = true;
            lblT_IN.Visible = true;

            divCP.Visible = true;
            divIG.Visible = true;
            divIP.Visible = true;
            divIN.Visible = true;

            //Actualiza los permisos de los botones en grid
            PermisosUsuario();

            if (Session["GridCP"] != null)
                GridCP.Selection.UnselectAll();

            Session["NuevoCP"] = "Si";
            Session["CLAVEPEDIMENTO"] = null;
            Session["IDENTIFICADOR"] = null;
            Session["IDENTIFICADORP"] = null;
            Session["INCOTERM"] = null;
        }

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
                excepcion.RegistrarExcepcion(idusuario, "Cat_TiposArchivo-chkConsultar_Init", ex, lblCadena.Text, ref mensaje);
            }
        }

        protected void lkb_EditaCP_Click(object sender, EventArgs e)
        {            
            int valida_select = 0;

            for (int i = 0; i < GridCP.VisibleRowCount; i++)
            {
                ASPxCheckBox chkConsultar = GridCP.FindRowCellTemplateControl(i, (GridViewDataColumn)GridCP.Columns["Seleccione"], "chkConsultar") as ASPxCheckBox;

                //if(GridCP.GetSelectedFieldValues("CLAVEPEDIMENTO").Count > 0)
                //if (GridCP.Selection.IsRowSelected(i))
                if (chkConsultar.Checked)
                {
                    MostrarModal();
                    MostrarModalCP();
                    MostrarModalCP2();
                    ModalTituloCP2.InnerText = "Editar Clave Pedimento";

                    var rowValues = GridCP.GetRowValues(i, new string[] { "CLAVEPEDIMENTO", "IDENTIFICADOR", "IDENTIFICADORP", "INCOTERM" }) as object[];

                    Session["RENGLON"] = i;
                    //Session["CLAVEPEDIMENTO"] = txtCP2_ClavePedimento.Text = GridCP.GetSelectedFieldValues("CLAVEPEDIMENTO")[0].ToString().Trim();
                    //Session["IDENTIFICADOR"] = txtCP2_IG.Text = GridCP.GetSelectedFieldValues("IDENTIFICADOR")[0].ToString().Trim();
                    //Session["IDENTIFICADORP"] = txtCP2_IP.Text = GridCP.GetSelectedFieldValues("IDENTIFICADORP")[0].ToString().Trim();
                    //Session["INCOTERM"] = txtCP2_Incoterm.Text = GridCP.GetSelectedFieldValues("INCOTERM")[0].ToString().Trim();

                    Session["CLAVEPEDIMENTO"] = txtCP2_ClavePedimento.Text = rowValues[0].ToString();
                    Session["IDENTIFICADOR"] = txtCP2_IG.Text = rowValues[1].ToString();
                    Session["IDENTIFICADORP"] = txtCP2_IP.Text = rowValues[2].ToString();
                    Session["INCOTERM"] = txtCP2_Incoterm.Text = rowValues[3].ToString();
                    Session["NuevoCP"] = null;
                    valida_select = 1;
                }
            }
            

            if (valida_select == 0)
            {
                MostrarModal();
                MostrarModalCP();
                AlertError("Debe seleccionar un registro para poder editar");
            }

            //Actualiza los permisos de los botones en grid
            PermisosUsuario();

        }

        protected void lkb_EliminarCP_Click(object sender, EventArgs e)
        {
            int valida_select = 0;
            MostrarModal();
            MostrarModalCP();

            for (int i = 0; i < GridCP.VisibleRowCount; i++)
            {
                ASPxCheckBox chkConsultar = GridCP.FindRowCellTemplateControl(i, (GridViewDataColumn)GridCP.Columns["Seleccione"], "chkConsultar") as ASPxCheckBox;

                if (chkConsultar.Checked)
                {
                    Session["RENGLON"] = i;

                    //Modal Question
                    string valida = "¿Desea eliminar este registro?";
                    AlertQuestion(valida);
                    valida_select = 1;
                }
            }
            
            if (valida_select == 0)
            {                
                AlertError("Debe seleccionar un registro para poder eliminar");
            }            
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            MostrarModal();
            MostrarModalCP();

            //Recorro el gridCP y elimino el renglon seleccionado
            DataTable dt = new DataTable();

            dt.Columns.Add("TCPKEY", typeof(Int64));
            dt.Columns.Add("ANTERIOR_CLAVEPEDIMENTO", typeof(string));
            dt.Columns.Add("CLAVEPEDIMENTO", typeof(string));
            dt.Columns.Add("ANTERIOR_IDENTIFICADOR", typeof(string));
            dt.Columns.Add("IDENTIFICADOR", typeof(string));
            dt.Columns.Add("ANTERIOR_IDENTIFICADORP", typeof(string));
            dt.Columns.Add("IDENTIFICADORP", typeof(string));
            dt.Columns.Add("ANTERIOR_INCOTERM", typeof(string));
            dt.Columns.Add("INCOTERM", typeof(string));

            if (Session["GridCP"] != null)
            {
                int renglon = 0;
                foreach (DataRow fila in ((DataTable)(Session["GridCP"])).Rows)
                {
                    if (renglon != int.Parse(Session["RENGLON"].ToString()))
                        dt.Rows.Add(0, fila["ANTERIOR_CLAVEPEDIMENTO"].ToString().Trim().ToUpper(), fila["CLAVEPEDIMENTO"].ToString().Trim().ToUpper(),
                                       fila["ANTERIOR_IDENTIFICADOR"].ToString().Trim().ToUpper(), fila["IDENTIFICADOR"].ToString().Trim().ToUpper(),
                                       fila["ANTERIOR_IDENTIFICADORP"].ToString().Trim().ToUpper(), fila["IDENTIFICADORP"].ToString().Trim().ToUpper(),
                                       fila["ANTERIOR_INCOTERM"].ToString().Trim().ToUpper(), fila["INCOTERM"].ToString().Trim().ToUpper());
                    else
                    {
                        HfCPBorrar.Value += "," + fila["CLAVEPEDIMENTO"].ToString().Trim().ToUpper();
                    }

                    renglon += 1;
                }
            }

            GridCP.DataSource = Session["GridCP"] = dt;
            GridCP.DataBind();
            GridCP.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
            GridCP.SettingsPager.PageSize = 20;
            GridCP.Settings.VerticalScrollableHeight = 210;
            GridCP.DataBind();

            if (Session["GridCP"] != null)
                GridCP.Selection.UnselectAll();

            //Actualiza los permisos de los botones en grid
            PermisosUsuario();
        }

        protected void lkb_INCOTERMCP2_Click(object sender, EventArgs e)
        {
            MostrarModal();
            MostrarModalCP();
            MostrarModalCP2();
            MostrarModalIN();
            ModalTituloIN.InnerText = "INCOTERM";
            Session["lkb_INCOTERM"] = "viene de CP";
 
            //Carga gridIN
            string mensaje = "";
            DataTable dt = new DataTable();

            //Carga GridIN
            if (txtCP2_Incoterm.Text.Trim().Length > 0)
            {
                if (Session["GridIN"] == null)
                {
                    dt = catalogo.TraeIncoterm(Int64.Parse(lblTP.Text.Trim()), lblCadena.Text, ref mensaje);
                    Session["GridIN"] = dt;
                }
            }
            else
                Session["GridIN"] = null;

            //Se limpia gridcp
            DataTable dtL = new DataTable();
            GridIN.DataSource = dtL;
            GridIN.DataBind();

            GridIN.DataSourceID = null;
            GridIN.DataBind();

            GridIN.DataSource = Session["GridIN"];
            GridIN.DataBind();
            GridIN.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
            GridIN.SettingsPager.PageSize = 20;
            GridIN.Settings.VerticalScrollableHeight = 210;

            if (Session["GridIN"] != null)
                GridIN.Selection.UnselectAll();           
        }

        protected void GridCP_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        {            
            //Session["TCPKEY"] = GridCP.GetRowValues(e.VisibleIndex, "TCPKEY").ToString();

            MostrarModal();
            MostrarModalCP();

            if (e.ButtonID == "btnEditar")
            {
                MostrarModalCP2();
                ModalTituloCP2.InnerText = "Editar Clave Pedimento";

                Session["RENGLON"] = e.VisibleIndex;
                Session["CLAVEPEDIMENTO"] = txtCP2_ClavePedimento.Text = GridCP.GetRowValues(e.VisibleIndex, "CLAVEPEDIMENTO").ToString();
                Session["IDENTIFICADOR"] = txtCP2_IG.Text = GridCP.GetRowValues(e.VisibleIndex, "IDENTIFICADOR").ToString();
                Session["IDENTIFICADORP"] = txtCP2_IP.Text = GridCP.GetRowValues(e.VisibleIndex, "IDENTIFICADORP").ToString();
                Session["INCOTERM"] = txtCP2_Incoterm.Text = GridCP.GetRowValues(e.VisibleIndex, "INCOTERM").ToString();

            }
            else if (e.ButtonID == "btnEliminar")
            {

            }
        }

        protected void btnGuardarCP2_Click(object sender, EventArgs e)
        {

            MostrarModal();
            MostrarModalCP();
            ModalTituloCP.InnerText = "Clave Pedimento";

            //Valida Clave Pedimento
            if(txtCP2_ClavePedimento.Text.Trim().Count() == 0)
            {                
                MostrarModalCP2();

                if (Session["CLAVEPEDIMENTO"] == null)
                    ModalTituloCP2.InnerText = "Nuevo Clave Pedimento";
                else
                    ModalTituloCP2.InnerText = "Editar Clave Pedimento";

                AlertError("Debe escribir una Clave Pedimento");
                return;
            }

            //Valida si existe el nuevo IG
            if (Session["GridCP"] != null)
            {
                foreach (DataRow fila in ((DataTable)(Session["GridCP"])).Rows)
                {
                    if (fila["CLAVEPEDIMENTO"].ToString().Trim().ToUpper() == txtCP2_ClavePedimento.Text.Trim().ToUpper())
                    {
                        MostrarModalIG2();
                        AlertError("Ya existe la Clave Pedimento: " + txtCP2_ClavePedimento.Text.Trim().ToUpper());
                        return;
                    }
                }
            }

            DataTable dt = new DataTable();

            dt.Columns.Add("TCPKEY", typeof(Int64));
            dt.Columns.Add("ANTERIOR_CLAVEPEDIMENTO", typeof(string));
            dt.Columns.Add("CLAVEPEDIMENTO", typeof(string));
            dt.Columns.Add("ANTERIOR_IDENTIFICADOR", typeof(string));
            dt.Columns.Add("IDENTIFICADOR", typeof(string));
            dt.Columns.Add("ANTERIOR_IDENTIFICADORP", typeof(string));
            dt.Columns.Add("IDENTIFICADORP", typeof(string));
            dt.Columns.Add("ANTERIOR_INCOTERM", typeof(string));
            dt.Columns.Add("INCOTERM", typeof(string));

            if (Session["GridCP"] != null)
            {
                int contador = 0;
                foreach (DataRow fila in ((DataTable)(Session["GridCP"])).Rows)
                {
                    //Valida si existe la session sig., si existe estas editando
                    //Session["CLAVEPEDIMENTO"] = txtCP2_ClavePedimento.Text = GridCP.GetRowValues(e.VisibleIndex, "CLAVEPEDIMENTO").ToString();
                    //Session["IDENTIFICADOR"] = txtCP2_IG.Text = GridCP.GetRowValues(e.VisibleIndex, "IDENTIFICADOR").ToString();
                    //Session["IDENTIFICADORP"] = txtCP2_IP.Text = GridCP.GetRowValues(e.VisibleIndex, "IDENTIFICADORP").ToString();
                    //Session["INCOTERM"] = txtCP2_Incoterm.Text = GridCP.GetRowValues(e.VisibleIndex, "INCOTERM").ToString();
                    if (Session["CLAVEPEDIMENTO"] != null && int.Parse(Session["RENGLON"].ToString()).Equals(contador))
                    {
                        if (fila["CLAVEPEDIMENTO"].ToString().ToUpper().Trim() == Session["CLAVEPEDIMENTO"].ToString().ToUpper().Trim() &&
                            fila["IDENTIFICADOR"].ToString().ToUpper().Trim() == Session["IDENTIFICADOR"].ToString().ToUpper().Trim() &&
                            fila["IDENTIFICADORP"].ToString().ToUpper().Trim() == Session["IDENTIFICADORP"].ToString().ToUpper().Trim() &&
                            fila["INCOTERM"].ToString().ToUpper().Trim() == Session["INCOTERM"].ToString().ToUpper().Trim())
                        {
                            dt.Rows.Add(0, fila["CLAVEPEDIMENTO"].ToString().ToUpper().Trim(), txtCP2_ClavePedimento.Text.ToUpper().Trim(), fila["IDENTIFICADOR"].ToString().ToUpper().Trim(), txtCP2_IG.Text.ToUpper().Trim(), 
                                fila["IDENTIFICADORP"].ToString().ToUpper().Trim(), txtCP2_IP.Text.ToUpper().Trim(), fila["INCOTERM"].ToString().ToUpper().Trim(), txtCP2_Incoterm.Text.ToUpper().Trim());

                            Session["CLAVEPEDIMENTO"] = null;
                            Session["IDENTIFICADOR"] = null;
                            Session["IDENTIFICADORP"] = null;
                            Session["INCOTERM"] = null;
                        }

                    }
                    else
                        dt.Rows.Add(0, string.Empty, fila["CLAVEPEDIMENTO"].ToString().Trim().ToUpper(), string.Empty, fila["IDENTIFICADOR"].ToString().Trim().ToUpper(),
                             string.Empty, fila["IDENTIFICADORP"].ToString().Trim().ToUpper(), string.Empty, fila["INCOTERM"].ToString().Trim().ToUpper());

                    contador += 1;
                }
            }

            //Valida si existe la session sig., si no existe estas agregando uno nuevo
            if (Session["NuevoCP"] != null && Session["NuevoCP"].ToString().Trim().Contains("Si"))
                dt.Rows.Add(0, string.Empty, txtCP2_ClavePedimento.Text.ToUpper().Trim(), string.Empty, txtCP2_IG.Text.ToUpper().Trim(), string.Empty, txtCP2_IP.Text.ToUpper().Trim(), string.Empty, txtCP2_Incoterm.Text.ToUpper().Trim());

            //Se limpia session
            Session["NuevoCP"] = null;


            //if(ModalTituloCP2.InnerText.Contains("Nue"))
            //{
            //    dt = catalogo.GuardaClavePedimento(Int64.Parse(lblTP.Text.ToUpper().Trim()), txtCP2_ClavePedimento.Text.ToUpper().Trim(), txtCP2_IG.Text.ToUpper().Trim(), txtCP2_IP.Text.ToUpper().Trim(), txtCP2_Incoterm.Text.ToUpper().Trim(), lblCadena.Text, ref mensaje);
            //}
            //else if (ModalTituloCP2.InnerText.Contains("Edit"))
            //{
            //    dt = catalogo.ActualizaClavePedimento(Int64.Parse(lblTP.Text.ToUpper().Trim()), txtCP2_ClavePedimento.Text.ToUpper().Trim(), txtCP2_IG.Text.ToUpper().Trim(), txtCP2_IP.Text.ToUpper().Trim(), txtCP2_Incoterm.Text.ToUpper().Trim(), lblCadena.Text, ref mensaje);
            //}


            GridCP.DataSource = Session["GridCP"] = dt;
            GridCP.DataBind();
            GridCP.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
            GridCP.SettingsPager.PageSize = 20;
            GridCP.Settings.VerticalScrollableHeight = 210;
            GridCP.DataBind();

            if (Session["GridCP"] != null)
                GridCP.Selection.UnselectAll();

            MostrarModal();
            MostrarModalCP();
            ModalTituloCP.InnerText = "Clave Pedimento";

        }

        //protected void btnCancelarCP2_Click(object sender, EventArgs e)
        //{
        //    MostrarModal();
        //    MostrarModalCP();
        //    ModalTituloCP.InnerText = "Clave Pedimento";

        //    ////Carga gridCP
        //    //string mensaje = "";
        //    //DataTable dt = new DataTable();

        //    //dt = catalogo.TraeClavesPedimentos(Int64.Parse(lblTP.Text.Trim()), lblCadena.Text, ref mensaje);

        //    //GridCP.DataSource = Session["GridCP"] = dt;
        //    //GridCP.DataBind();
        //    //GridCP.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
        //    //GridCP.SettingsPager.PageSize = 20;
        //    //GridCP.Settings.VerticalScrollableHeight = 330;
        //    //GridCP.DataBind();
        //}

        #endregion

        #region IG

        protected void lkb_IdentificadorGeneral_Click(object sender, EventArgs e)
        {
            MostrarModal();
            MostrarModalIG();
            ModalTituloIG.InnerText = "Identificador General";

            //Carga gridIG
            string mensaje = "";
            DataTable dt = new DataTable();

            if (txt_IG.Text.Trim().Length > 0)
            {
                if (Session["Mantener_Info_IG"] == null)
                {
                    dt = catalogo.TraeIG(Int64.Parse(lblTP.Text.Trim()), lblCadena.Text, ref mensaje);
                    Session["GridIG"] = dt;
                }
            }
            else
                Session["GridIG"] = null;
            

            GridIG.DataSource = Session["GridIG"];
            GridIG.DataBind();
            GridIG.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
            GridIG.SettingsPager.PageSize = 20;
            GridIG.Settings.VerticalScrollableHeight = 210;
            GridIG.DataBind();
        }

        protected void btnGuardarIG_Click(object sender, EventArgs e)
        {
            MostrarModal();            

            //colocar valores del Session["GridIN"] en las cajas de texto
            string v_ig = string.Empty;

            if (Session["GridIG"] != null)
            {
                foreach (DataRow fila in ((DataTable)(Session["GridIG"])).Rows)
                {
                    v_ig += fila["IDENTIFICADOR"].ToString().ToUpper().Trim() + ",";
                }
            }

            if (v_ig.Length > 0)
                v_ig = v_ig.Substring(0, v_ig.Length - 1);

            txt_IG.Text = v_ig;

            Session["Mantener_Info_IG"] = "Si";

            //Si hay datos en clave pedimento se bloquean los botones IG,IP e IN
            if ((txt_IG.Text.Trim().Length > 0 || txt_IP.Text.Trim().Length > 0 || txt_Incoterm.Text.Trim().Length > 0) && txt_ClavePedimento.Text.Trim().Length.Equals(0))
            {
                lkb_ClavePedimento.Enabled = false;
            }
            else
            {
                lkb_ClavePedimento.Enabled = true;
            }


        }

        protected void lkb_NuevoIG_Click(object sender, EventArgs e)
        {
            MostrarModal();
            MostrarModalIG();
            MostrarModalIG2();

            //Titulo del Modal IG2
            ModalTituloIG2.InnerText = "Nuevo Identificador General";


            //Limpiar Campo de IG2
            txtIG2_IdentificadorGeneral.Text = string.Empty;

            Session["NuevoIG"] = "Si";
            Session["IDENTIFICADOR"] = null;

            //Actualiza los permisos de los botones en grid
            PermisosUsuario();
        }

        protected void chkConsultarIG_Init(object sender, EventArgs e)
        {
            try
            {
                ASPxCheckBox chb = sender as ASPxCheckBox;
                GridViewDataItemTemplateContainer container = chb.NamingContainer as GridViewDataItemTemplateContainer;

                chb.ClientInstanceName = String.Format("chkConsultarIG{0}", container.VisibleIndex);
                chb.ClientSideEvents.CheckedChanged = String.Format("function (s, e) {{ chkConsultarIGClick(s, e, {0}); }}", container.VisibleIndex);
            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "Cat_TiposArchivo-chkConsultarIG_Init", ex, lblCadena.Text, ref mensaje);
            }
        }

        protected void lkb_EditaIG_Click(object sender, EventArgs e)
        {
            int valida_select = 0;


            for (int i = 0; i < GridIG.VisibleRowCount; i++)
            {
                ASPxCheckBox chkConsultarIG = GridIG.FindRowCellTemplateControl(i, (GridViewDataColumn)GridIG.Columns["Seleccione"], "chkConsultarIG") as ASPxCheckBox;

                if (chkConsultarIG.Checked)
                {
                    MostrarModal();
                    MostrarModalIG();
                    MostrarModalIG2();
                    ModalTituloIG2.InnerText = "Editar Identificador General";

                    var rowValues = GridIG.GetRowValues(i, new string[] { "IGKEY", "IDENTIFICADOR" }) as object[];

                    Session["RENGLON_IG"] = i;
                    Session["IGKEY"] = rowValues[0].ToString();
                    Session["IDENTIFICADOR"] = txtIG2_IdentificadorGeneral.Text = rowValues[1].ToString();
                    Session["NuevoIG"] = null;
                    valida_select = 1;
                    break;
                }
            }


            if (valida_select == 0)
            {
                MostrarModal();                
                MostrarModalIG();
                AlertError("Debe seleccionar un registro para poder editar");
            }

            //Actualiza los permisos de los botones en grid
            PermisosUsuario();

        }

        protected void lkb_EliminarIG_Click(object sender, EventArgs e)
        {
            int valida_select = 0;
            MostrarModal();            
            MostrarModalIG();

            for (int i = 0; i < GridIG.VisibleRowCount; i++)
            {
                ASPxCheckBox chkConsultarIG = GridIG.FindRowCellTemplateControl(i, (GridViewDataColumn)GridIG.Columns["Seleccione"], "chkConsultarIG") as ASPxCheckBox;

                if (chkConsultarIG.Checked)
                {
                    Session["RENGLON_IG"] = i;

                    //Modal Question
                    string valida = "¿Desea eliminar este registro?";
                    AlertQuestionIG(valida);
                    valida_select = 1;
                    break;
                }
            }

            if (valida_select == 0)
            {
                AlertError("Debe seleccionar un registro para poder eliminar");
            }
        }

        protected void btnOkIG_Click(object sender, EventArgs e)
        {
            MostrarModal();
            MostrarModalIG();

            //Recorro el gridIG y elimino el renglon seleccionado
            DataTable dt = new DataTable();

            dt.Columns.Add("IGKEY", typeof(Int64));
            dt.Columns.Add("ANTERIOR_IDENTIFICADOR", typeof(string));
            dt.Columns.Add("IDENTIFICADOR", typeof(string));

            if (Session["GridIG"] != null)
            {
                int renglon = 0;
                foreach (DataRow fila in ((DataTable)(Session["GridIG"])).Rows)
                {
                    if (renglon != int.Parse(Session["RENGLON_IG"].ToString()))
                        dt.Rows.Add(fila["IGKEY"].ToString(), fila["ANTERIOR_IDENTIFICADOR"].ToString().Trim().ToUpper(), fila["IDENTIFICADOR"].ToString().Trim().ToUpper());
                    else
                    {
                        HfIGBorrar.Value += "," + fila["IDENTIFICADOR"].ToString().Trim().ToUpper();
                    }


                    renglon += 1;
                }
            }

            GridIG.DataSource = Session["GridIG"] = dt;
            GridIG.DataBind();
            GridIG.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
            GridIG.SettingsPager.PageSize = 20;
            GridIG.Settings.VerticalScrollableHeight = 210;
            GridIG.DataBind();

            if (Session["GridIG"] != null)
                GridIG.Selection.UnselectAll();

            //Actualiza los permisos de los botones en grid
            PermisosUsuario();
        }

        protected void GridIG_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            Int64 tcpkey = 0;
            Int64 tpkey = 0;
            tcpkey = Int64.Parse(GridIG.GetRowValues(e.VisibleIndex, "TCPKEY").ToString());
            tpkey = Int64.Parse(GridIG.GetRowValues(e.VisibleIndex, "TPKEY").ToString());

            if (e.ButtonID == "btnEditar")
            {

            }
            else if (e.ButtonID == "btnEliminar")
            {

            }
        }

        protected void btnGuardarIG2_Click(object sender, EventArgs e)
        {
            MostrarModal();
            MostrarModalIG();


            //Valida Clave Pedimento
            if (txtIG2_IdentificadorGeneral.Text.Trim().Count() == 0)
            {
                MostrarModalIG2();
                AlertError("Debe escribir un Identificador General");
                return;
            }

            //Valida si existe el nuevo IG
            if (Session["GridIG"] != null)
            {
                foreach (DataRow fila in ((DataTable)(Session["GridIG"])).Rows)
                {
                    if (fila["IDENTIFICADOR"].ToString().Trim().ToUpper() == txtIG2_IdentificadorGeneral.Text.Trim().ToUpper())
                    {
                        MostrarModalIG2();
                        AlertError("Ya existe el Identificador General: " + txtIG2_IdentificadorGeneral.Text.Trim().ToUpper());
                        return;
                    }
                }
            }


            DataTable dt = new DataTable();
            dt.Columns.Add("IGKEY", typeof(Int64));
            dt.Columns.Add("ANTERIOR_IDENTIFICADOR", typeof(string));
            dt.Columns.Add("IDENTIFICADOR", typeof(string));

            Int64 renglon = 0;
            if (Session["GridIG"] != null)
            {
                foreach (DataRow fila in ((DataTable)(Session["GridIG"])).Rows)
                {
                    if (Session["RENGLON_IG"] != null && Session["RENGLON_IG"].ToString() == renglon.ToString() && Session["IGKEY"] != null && Session["IDENTIFICADOR"] != null)
                    {
                        dt.Rows.Add(Int64.Parse(Session["IGKEY"].ToString()), Session["IDENTIFICADOR"].ToString().Trim().ToUpper(), txtIG2_IdentificadorGeneral.Text.Trim().ToUpper());
                        Session["IGKEY"] = null;
                        Session["RENGLON_IG"] = null;
                        Session["IDENTIFICADOR"] = null;
                    }
                    else
                    {
                        dt.Rows.Add(Int64.Parse(fila["IGKEY"].ToString()), string.Empty, fila["IDENTIFICADOR"].ToString().Trim().ToUpper());
                    }
                    renglon += 1;
                }
            }

            if (Session["NuevoIG"] != null && Session["NuevoIG"].ToString().Contains("Si"))
            {
                dt.Rows.Add(renglon, string.Empty, txtIG2_IdentificadorGeneral.Text.ToUpper().Trim());
                Session["NuevoIG"] = null;
            }

            GridIG.DataSource = Session["GridIG"] = dt;
            GridIG.DataBind();
            GridIG.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
            GridIG.SettingsPager.PageSize = 20;
            GridIG.Settings.VerticalScrollableHeight = 210;
            GridIG.DataBind();
        }

        #endregion


        #region IP

        protected void lkb_IdentificadorPartida_Click(object sender, EventArgs e)
        {
            MostrarModal();
            MostrarModalIP();
            ModalTituloIP.InnerText = "Identificador Partida";

            //Carga gridIP
            string mensaje = "";
            DataTable dt = new DataTable();

            if (txt_IG.Text.Trim().Length > 0)
            {
                if (Session["Mantener_Info_IP"] == null)
                {
                    dt = catalogo.TraeIP(Int64.Parse(lblTP.Text.Trim()), lblCadena.Text, ref mensaje);
                    Session["GridIP"] = dt;
                }
            }
            else
                Session["GridIP"] = null;


            GridIP.DataSource = Session["GridIP"];
            GridIP.DataBind();
            GridIP.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
            GridIP.SettingsPager.PageSize = 20;
            GridIP.Settings.VerticalScrollableHeight = 210;
            GridIP.DataBind();
        }

        protected void btnGuardarIP_Click(object sender, EventArgs e)
        {
            MostrarModal();

            //colocar valores del Session["GridIN"] en las cajas de texto
            string v_ip = string.Empty;

            if (Session["GridIP"] != null)
            {
                foreach (DataRow fila in ((DataTable)(Session["GridIP"])).Rows)
                {
                    v_ip += fila["IDENTIFICADORP"].ToString().ToUpper().Trim() + ",";
                }
            }

            if (v_ip.Length > 0)
                v_ip = v_ip.Substring(0, v_ip.Length - 1);

            txt_IP.Text = v_ip;

            Session["Mantener_Info_IP"] = "Si";

            //Si hay datos en clave pedimento se bloquean los botones IG,IP e IN
            if ((txt_IG.Text.Trim().Length > 0 || txt_IP.Text.Trim().Length > 0 || txt_Incoterm.Text.Trim().Length > 0) && txt_ClavePedimento.Text.Trim().Length.Equals(0))
            {
                lkb_ClavePedimento.Enabled = false;
            }
            else
            {
                lkb_ClavePedimento.Enabled = true;
            }
        }

        protected void lkb_NuevoIP_Click(object sender, EventArgs e)
        {
            MostrarModal();
            MostrarModalIP();
            MostrarModalIP2();

            //Titulo del Modal IP2
            ModalTituloIP2.InnerText = "Nuevo Identificador Partida";


            //Limpiar Campo de IP2
            txtIP2_IdentificadorPartida.Text = string.Empty;

            Session["NuevoIP"] = "Si";
            Session["IDENTIFICADORP"] = null;

            //Actualiza los permisos de los botones en grid
            PermisosUsuario();
        }

        protected void chkConsultarIP_Init(object sender, EventArgs e)
        {
            try
            {
                ASPxCheckBox chb = sender as ASPxCheckBox;
                GridViewDataItemTemplateContainer container = chb.NamingContainer as GridViewDataItemTemplateContainer;

                chb.ClientInstanceName = String.Format("chkConsultarIP{0}", container.VisibleIndex);
                chb.ClientSideEvents.CheckedChanged = String.Format("function (s, e) {{ chkConsultarIPClick(s, e, {0}); }}", container.VisibleIndex);
            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "Cat_TiposArchivo-chkConsultarIP_Init", ex, lblCadena.Text, ref mensaje);
            }
        }

        protected void lkb_EditaIP_Click(object sender, EventArgs e)
        {
            int valida_select = 0;


            for (int i = 0; i < GridIP.VisibleRowCount; i++)
            {
                ASPxCheckBox chkConsultarIP = GridIP.FindRowCellTemplateControl(i, (GridViewDataColumn)GridIP.Columns["Seleccione"], "chkConsultarIP") as ASPxCheckBox;

                if (chkConsultarIP.Checked)
                {
                    MostrarModal();
                    MostrarModalIP();
                    MostrarModalIP2();
                    ModalTituloIP2.InnerText = "Editar Identificador Partida";

                    var rowValues = GridIP.GetRowValues(i, new string[] { "IPKEY", "IDENTIFICADORP" }) as object[];

                    Session["RENGLON_IP"] = i;
                    Session["IPKEY"] = rowValues[0].ToString();
                    Session["IDENTIFICADORP"] = txtIP2_IdentificadorPartida.Text = rowValues[1].ToString();
                    Session["NuevoIP"] = null;
                    valida_select = 1;
                    break;
                }
            }


            if (valida_select == 0)
            {
                MostrarModal();
                MostrarModalIP();
                AlertError("Debe seleccionar un registro para poder editar");
            }

            //Actualiza los permisos de los botones en grid
            PermisosUsuario();

        }

        protected void lkb_EliminarIP_Click(object sender, EventArgs e)
        {
            int valida_select = 0;
            MostrarModal();
            MostrarModalIP();

            for (int i = 0; i < GridIP.VisibleRowCount; i++)
            {
                ASPxCheckBox chkConsultarIP = GridIP.FindRowCellTemplateControl(i, (GridViewDataColumn)GridIP.Columns["Seleccione"], "chkConsultarIP") as ASPxCheckBox;

                if (chkConsultarIP.Checked)
                {
                    Session["RENGLON_IP"] = i;

                    //Modal Question
                    string valida = "¿Desea eliminar este registro?";
                    AlertQuestionIP(valida);
                    valida_select = 1;
                    break;
                }
            }

            if (valida_select == 0)
            {
                AlertError("Debe seleccionar un registro para poder eliminar");
            }
        }

        protected void btnOkIP_Click(object sender, EventArgs e)
        {
            MostrarModal();
            MostrarModalIP();

            //Recorro el gridIP y elimino el renglon seleccionado
            DataTable dt = new DataTable();

            dt.Columns.Add("IPKEY", typeof(Int64));
            dt.Columns.Add("ANTERIOR_IDENTIFICADORP", typeof(string));
            dt.Columns.Add("IDENTIFICADORP", typeof(string));

            if (Session["GridIP"] != null)
            {
                int renglon = 0;
                foreach (DataRow fila in ((DataTable)(Session["GridIP"])).Rows)
                {
                    if (renglon != int.Parse(Session["RENGLON_IP"].ToString()))
                        dt.Rows.Add(fila["IPKEY"].ToString(), fila["ANTERIOR_IDENTIFICADORP"].ToString().Trim().ToUpper(), fila["IDENTIFICADORP"].ToString().Trim().ToUpper());
                    else
                    {
                        HfIPBorrar.Value += "," + fila["IDENTIFICADORP"].ToString().Trim().ToUpper();
                    }


                    renglon += 1;
                }
            }

            GridIP.DataSource = Session["GridIP"] = dt;
            GridIP.DataBind();
            GridIP.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
            GridIP.SettingsPager.PageSize = 20;
            GridIP.Settings.VerticalScrollableHeight = 210;
            GridIP.DataBind();

            if (Session["GridIP"] != null)
                GridIP.Selection.UnselectAll();

            //Actualiza los permisos de los botones en grid
            PermisosUsuario();
        }

        protected void GridIP_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            if (e.ButtonID == "btnEditar")
            {

            }
            else if (e.ButtonID == "btnEliminar")
            {

            }
        }

        protected void btnGuardarIP2_Click(object sender, EventArgs e)
        {
            MostrarModal();
            MostrarModalIP();


            //Valida IP
            if (txtIP2_IdentificadorPartida.Text.Trim().Count() == 0)
            {
                MostrarModalIP2();
                AlertError("Debe escribir un Identificador Partida");
                return;
            }

            //Valida si existe el nuevo IP
            if (Session["GridIP"] != null)
            {
                foreach (DataRow fila in ((DataTable)(Session["GridIP"])).Rows)
                {
                    if (fila["IDENTIFICADORP"].ToString().Trim().ToUpper() == txtIP2_IdentificadorPartida.Text.Trim().ToUpper())
                    {
                        MostrarModalIP2();
                        AlertError("Ya existe el Identificador Partida: " + txtIP2_IdentificadorPartida.Text.Trim().ToUpper());
                        return;
                    }
                }
            }


            DataTable dt = new DataTable();
            dt.Columns.Add("IPKEY", typeof(Int64));
            dt.Columns.Add("ANTERIOR_IDENTIFICADORP", typeof(string));
            dt.Columns.Add("IDENTIFICADORP", typeof(string));

            Int64 renglon = 0;
            if (Session["GridIP"] != null)
            {
                foreach (DataRow fila in ((DataTable)(Session["GridIP"])).Rows)
                {
                    if (Session["RENGLON_IP"] != null && Session["RENGLON_IP"].ToString() == renglon.ToString() && Session["IPKEY"] != null && Session["IDENTIFICADORP"] != null)
                    {
                        dt.Rows.Add(Int64.Parse(Session["IPKEY"].ToString()), Session["IDENTIFICADORP"].ToString().Trim().ToUpper(), txtIP2_IdentificadorPartida.Text.Trim().ToUpper());
                        Session["IPKEY"] = null;
                        Session["RENGLON_IP"] = null;
                        Session["IDENTIFICADORP"] = null;
                    }
                    else
                    {
                        dt.Rows.Add(Int64.Parse(fila["IPKEY"].ToString()), string.Empty, fila["IDENTIFICADORP"].ToString().Trim().ToUpper());
                    }
                    renglon += 1;
                }
            }

            if (Session["NuevoIP"] != null && Session["NuevoIP"].ToString().Contains("Si"))
            {
                dt.Rows.Add(renglon, string.Empty, txtIP2_IdentificadorPartida.Text.ToUpper().Trim());
                Session["NuevoIP"] = null;
            }

            GridIP.DataSource = Session["GridIP"] = dt;
            GridIP.DataBind();
            GridIP.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
            GridIP.SettingsPager.PageSize = 20;
            GridIP.Settings.VerticalScrollableHeight = 210;
            GridIP.DataBind();
        }

        #endregion


        #region IN

        protected void lkb_INCOTERM_Click(object sender, EventArgs e)
        {
            try
            {
                MostrarModal();
                MostrarModalIN();
                ModalTituloIN.InnerText = "INCOTERM";
                Session["lkb_INCOTERM"] = "viene de incoterm";


                string mensaje = "";
                DataTable dt = new DataTable();

                //Carga GridIN
                if (txt_Incoterm.Text.Trim().Length > 0)
                {
                    if (Session["Mantener_Info_IN"] == null)
                    {
                        dt = catalogo.TraeIncoterm(Int64.Parse(lblTP.Text.Trim()), lblCadena.Text, ref mensaje);
                        Session["GridIN"] = dt;
                    }
                }
                else
                    Session["GridIN"] = null;


                //Se limpia gridcp
                DataTable dtL = new DataTable();
                GridIN.DataSource = dtL;
                GridIN.DataBind();

                GridIN.DataSourceID = null;
                GridIN.DataBind();

                GridIN.DataSource = Session["GridIN"];
                GridIN.DataBind();
                GridIN.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                GridIN.SettingsPager.PageSize = 20;
                GridIN.Settings.VerticalScrollableHeight = 210;

                if (Session["GridIN"] != null)
                    GridIN.Selection.UnselectAll();
            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "Documentos-GridPartidasDetail_Init", ex, lblCadena.Text, ref mensaje);
                AlertError(ex.Message);
            }
        }

        protected void btnGuardarIN_Click(object sender, EventArgs e)
        {            
            MostrarModal();
            if (Session["lkb_INCOTERM"] != null && Session["lkb_INCOTERM"].ToString().ToUpper().Contains("CP"))
            {
                MostrarModalCP();
                MostrarModalCP2();
            }

            //colocar valores del Session["GridIN"] en las cajas de texto
            string v_incoterm = string.Empty;

            if (Session["GridIN"] != null)
            {
                foreach (DataRow fila in ((DataTable)(Session["GridIN"])).Rows)
                {
                    v_incoterm += fila["INCOTERM"].ToString().ToUpper().Trim() + ",";
                }
            }

            if (v_incoterm.Length > 0)
                v_incoterm = v_incoterm.Substring(0, v_incoterm.Length - 1);

            if (Session["lkb_INCOTERM"] != null && Session["lkb_INCOTERM"].ToString().ToUpper().Contains("CP"))
                txtCP2_Incoterm.Text = v_incoterm;
            else
                txt_Incoterm.Text = v_incoterm;


            Session["Mantener_Info_IN"] = "Si";

            //Si hay datos en clave pedimento se bloquean los botones IG,IP e IN
            if ((txt_IG.Text.Trim().Length > 0 || txt_IP.Text.Trim().Length > 0 || txt_Incoterm.Text.Trim().Length > 0) && txt_ClavePedimento.Text.Trim().Length.Equals(0))
            {
                lkb_ClavePedimento.Enabled = false;
            }
            else
            {
                lkb_ClavePedimento.Enabled = true;
            }
        }

        //Este botón se puede divir en 2 partes, puede venir de CP o directamente de Incoterm
        protected void lkb_NuevoIN_Click(object sender, EventArgs e)
        {            
            MostrarModal();
            if (Session["lkb_INCOTERM"] != null && Session["lkb_INCOTERM"].ToString().ToUpper().Contains("CP"))
            {
                MostrarModalCP();
                MostrarModalCP2();
            }
            MostrarModalIN();

            //Abre Modal IN2
            MostrarModalIN2();

            //Titulo del Modal CP3
            ModalTituloIN2.InnerText = "Nuevo INCOTERM";


            //Limpiar Campo de CP3
            txtIN2_Incoterm.Text = string.Empty;
            txtIN2_Incoterm.Focus();

            //Actualiza los permisos de los botones en grid
            PermisosUsuario();

            Session["NuevoIN"] = "Si";
            Session["IKEY"] = null;
            Session["RENGLON_IN"] = null;
            Session["INCOTERM"] = null;
        }

        protected void chkConsultarIN_Init(object sender, EventArgs e)
        {
            try
            {
                ASPxCheckBox chb = sender as ASPxCheckBox;
                GridViewDataItemTemplateContainer container = chb.NamingContainer as GridViewDataItemTemplateContainer;

                chb.ClientInstanceName = String.Format("chkConsultarIN{0}", container.VisibleIndex);
                chb.ClientSideEvents.CheckedChanged = String.Format("function (s, e) {{ chkConsultarINClick(s, e, {0}); }}", container.VisibleIndex);
            }
            catch (Exception ex)
            {
                string mensaje = string.Empty;
                int idusuario = 0;
                if (Session["IdUsuario"] != null)
                    idusuario = int.Parse(Session["IdUsuario"].ToString());
                excepcion.RegistrarExcepcion(idusuario, "Cat_TiposArchivo-chkConsultarIN_Init", ex, lblCadena.Text, ref mensaje);
            }
        }

        protected void lkb_EditaIN_Click(object sender, EventArgs e)
        {
            int valida_select = 0;
            

            for (int i = 0; i < GridIN.VisibleRowCount; i++)
            {
                ASPxCheckBox chkConsultarIN = GridIN.FindRowCellTemplateControl(i, (GridViewDataColumn)GridIN.Columns["Seleccione"], "chkConsultarIN") as ASPxCheckBox;

                if (chkConsultarIN.Checked)
                {
                    MostrarModal();
                    if (Session["lkb_INCOTERM"] != null && Session["lkb_INCOTERM"].ToString().ToUpper().Contains("CP"))
                    {
                        MostrarModalCP();
                        MostrarModalCP2();
                    }

                    MostrarModalIN();
                    MostrarModalIN2();
                    ModalTituloIN2.InnerText = "Editar INCOTERM";

                    var rowValues = GridIN.GetRowValues(i, new string[] { "IKEY", "INCOTERM" }) as object[];

                    Session["RENGLON_IN"] = i;
                    Session["IKEY"] = rowValues[0].ToString();
                    Session["INCOTERM"] = txtIN2_Incoterm.Text = rowValues[1].ToString();
                    Session["NuevoIN"] = null;
                    valida_select = 1;
                    break;
                }
            }


            if (valida_select == 0)
            {
                MostrarModal();
                if (Session["lkb_INCOTERM"] != null && Session["lkb_INCOTERM"].ToString().ToUpper().Contains("CP"))
                {
                    MostrarModalCP();
                    MostrarModalCP2();
                }

                MostrarModalIN();
                AlertError("Debe seleccionar un registro para poder editar");
            }

            //Actualiza los permisos de los botones en grid
            PermisosUsuario();

        }

        protected void lkb_EliminarIN_Click(object sender, EventArgs e)
        {
            int valida_select = 0;
            MostrarModal();
            if (Session["lkb_INCOTERM"] != null && Session["lkb_INCOTERM"].ToString().ToUpper().Contains("CP"))
            {
                MostrarModalCP();
                MostrarModalCP2();
            }
            MostrarModalIN();

            for (int i = 0; i < GridIN.VisibleRowCount; i++)
            {
                ASPxCheckBox chkConsultarIN = GridIN.FindRowCellTemplateControl(i, (GridViewDataColumn)GridIN.Columns["Seleccione"], "chkConsultarIN") as ASPxCheckBox;

                if (chkConsultarIN.Checked)
                {
                    Session["RENGLON_IN"] = i;

                    //Modal Question
                    string valida = "¿Desea eliminar este registro?";
                    AlertQuestionIN(valida);
                    valida_select = 1;
                    break;
                }
            }

            if (valida_select == 0)
            {
                AlertError("Debe seleccionar un registro para poder eliminar");
            }
        }

        protected void btnOkIN_Click(object sender, EventArgs e)
        {
            MostrarModal();
            if (Session["lkb_INCOTERM"] != null && Session["lkb_INCOTERM"].ToString().ToUpper().Contains("CP"))
            {
                MostrarModalCP();
                MostrarModalCP2();
            }
            MostrarModalIN();

            //Recorro el gridIN y elimino el renglon seleccionado
            DataTable dt = new DataTable();

            dt.Columns.Add("IKEY", typeof(Int64));
            dt.Columns.Add("ANTERIOR_INCOTERM", typeof(string));
            dt.Columns.Add("INCOTERM", typeof(string));

            if (Session["GridIN"] != null)
            {
                int renglon = 0;
                foreach (DataRow fila in ((DataTable)(Session["GridIN"])).Rows)
                {
                    if (renglon != int.Parse(Session["RENGLON_IN"].ToString()))
                        dt.Rows.Add(fila["IKEY"].ToString(), fila["ANTERIOR_INCOTERM"].ToString().Trim().ToUpper(), fila["INCOTERM"].ToString().Trim().ToUpper());
                    else
                    {
                        HfINBorrar.Value += "," + fila["INCOTERM"].ToString().Trim().ToUpper();
                    }

                    renglon += 1;
                }
            }

            GridIN.DataSource = Session["GridIN"] = dt;
            GridIN.DataBind();
            GridIN.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
            GridIN.SettingsPager.PageSize = 20;
            GridIN.Settings.VerticalScrollableHeight = 210;
            GridIN.DataBind();

            if (Session["GridIN"] != null)
                GridIN.Selection.UnselectAll();

            //Actualiza los permisos de los botones en grid
            PermisosUsuario();
        }

        protected void GridIN_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            Int64 tcpkey = 0;
            Int64 tpkey = 0;
            tcpkey = Int64.Parse(GridIN.GetRowValues(e.VisibleIndex, "TCPKEY").ToString());
            tpkey = Int64.Parse(GridIN.GetRowValues(e.VisibleIndex, "TPKEY").ToString());

            if (e.ButtonID == "btnEditar")
            {

            }
            else if (e.ButtonID == "btnEliminar")
            {

            }
        }       

        protected void btnGuardarIN2_Click(object sender, EventArgs e)
        {            
            MostrarModal();
            if (Session["lkb_INCOTERM"] != null && Session["lkb_INCOTERM"].ToString().ToUpper().Contains("CP"))
            {
                MostrarModalCP();
                MostrarModalCP2();
            }
            MostrarModalIN();


            //Valida Clave Pedimento
            if (txtIN2_Incoterm.Text.Trim().Count() == 0)
            {
                MostrarModalIN2();
                AlertError("Debe escribir un INCOTERM");
                return;
            }

            //Valida si existe el nuevo IN
            if (Session["GridIN"] != null)
            {
                foreach (DataRow fila in ((DataTable)(Session["GridIN"])).Rows)
                {
                    if (fila["INCOTERM"].ToString().Trim().ToUpper() == txtIN2_Incoterm.Text.Trim().ToUpper())
                    {
                        MostrarModalIN2();
                        AlertError("Ya existe el INCOTERM: " + txtIN2_Incoterm.Text.Trim().ToUpper());
                        return;
                    }
                }
            }


            DataTable dt = new DataTable();
            dt.Columns.Add("IKEY", typeof(Int64));
            dt.Columns.Add("ANTERIOR_INCOTERM", typeof(string));
            dt.Columns.Add("INCOTERM", typeof(string));

            Int64 renglon = 0;
            if (Session["GridIN"] != null)
            {
                foreach (DataRow fila in ((DataTable)(Session["GridIN"])).Rows)
                {
                    if (Session["RENGLON_IN"] != null && Session["RENGLON_IN"].ToString() == renglon.ToString() && Session["IKEY"] != null && Session["INCOTERM"] != null)
                    {
                        dt.Rows.Add(Int64.Parse(Session["IKEY"].ToString()), Session["INCOTERM"].ToString().Trim().ToUpper(), txtIN2_Incoterm.Text.Trim().ToUpper());
                        Session["IKEY"] = null;
                        Session["RENGLON_IN"] = null;
                        Session["INCOTERM"] = null;
                    }
                    else
                    {
                        dt.Rows.Add(Int64.Parse(fila["IKEY"].ToString()), string.Empty, fila["INCOTERM"].ToString().Trim().ToUpper());
                    }
                    renglon += 1;
                }
            }

            if (Session["NuevoIN"] != null && Session["NuevoIN"].ToString().Contains("Si"))
            {
                dt.Rows.Add(renglon, string.Empty, txtIN2_Incoterm.Text.ToUpper().Trim());
                Session["NuevoIN"] = null;
            }

            GridIN.DataSource = Session["GridIN"] = dt;
            GridIN.DataBind();
            GridIN.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
            GridIN.SettingsPager.PageSize = 20;
            GridIN.Settings.VerticalScrollableHeight = 210;
            GridIN.DataBind();
        }

        #endregion

        

        


        

        

        

        

    }
}